﻿// The MIT License (MIT)
// 
// Copyright (c) 2014-2016, Institute for Software & Systems Engineering
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

namespace SafetySharp.Analysis
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.IO;
	using System.Linq;
	using System.Text;
	using Modeling;
	using Runtime;
	using Runtime.Serialization;
	using Utilities;

	/// <summary>
	///   Performs safety analyses on a model.
	/// </summary>
	public sealed class SafetyAnalysis
	{
		/// <summary>
		///   The model checker that is used for the analysis.
		/// </summary>
		private readonly SSharpChecker _modelChecker = new SSharpChecker();

		/// <summary>
		///   The model checker's configuration that determines certain model checker settings.
		/// </summary>
		public AnalysisConfiguration Configuration = AnalysisConfiguration.Default;

		/// <summary>
		///   Initializes a new instance.
		/// </summary>
		public SafetyAnalysis()
		{
			Configuration.ProgressReportsOnly = true;
		}

		/// <summary>
		///   Raised when the model checker has written an output. The output is always written to the console by default.
		/// </summary>
		public event Action<string> OutputWritten
		{
			add { _modelChecker.OutputWritten += value; }
			remove { _modelChecker.OutputWritten += value; }
		}

		/// <summary>
		///   Computes the minimal critical sets for the <paramref name="hazard" />.
		/// </summary>
		/// <param name="model">The model the safety analysis should be conducted for.</param>
		/// <param name="hazard">The hazard the minimal critical sets should be computed for.</param>
		/// <param name="maxCardinality">
		///   The maximum cardinality of the fault sets that should be checked. By default, all minimal
		///   critical fault sets are determined.
		/// </param>
		public static Result AnalyzeHazard(ModelBase model, Formula hazard, int maxCardinality = Int32.MaxValue)
		{
			return new SafetyAnalysis().ComputeMinimalCriticalSets(model, hazard, maxCardinality);
		}

		/// <summary>
		///   Computes the minimal critical sets for the <paramref name="hazard" />.
		/// </summary>
		/// <param name="model">The model the safety analysis should be conducted for.</param>
		/// <param name="hazard">The hazard the minimal critical sets should be computed for.</param>
		/// <param name="maxCardinality">
		///   The maximum cardinality of the fault sets that should be checked. By default, all minimal
		///   critical fault sets are determined.
		/// </param>
		public Result ComputeMinimalCriticalSets(ModelBase model, Formula hazard, int maxCardinality = Int32.MaxValue)
		{
			Requires.NotNull(model, nameof(model));
			Requires.NotNull(hazard, nameof(hazard));

			_modelChecker.Configuration = Configuration;
			ConsoleHelpers.WriteLine("Running Deductive Cause Consequence Analysis.");

			var stopwatch = new Stopwatch();
			stopwatch.Start();

			var allFaults = model.Faults;
			FaultSet.CheckFaultCount(allFaults.Length);

			var forcedFaults = allFaults.Where(fault => fault.Activation == Activation.Forced).ToArray();
			var suppressedFaults = allFaults.Where(fault => fault.Activation == Activation.Suppressed).ToArray();
			var nondeterministicFaults = allFaults.Where(fault => fault.Activation == Activation.Nondeterministic).ToArray();

			var suppressedSet = new FaultSet(suppressedFaults);
			var forcedSet = new FaultSet(forcedFaults);

			var isComplete = true;
			var safeSets = new HashSet<FaultSet>();
			var criticalSets = new HashSet<FaultSet>();
			var checkedSets = new HashSet<FaultSet>();
			var counterExamples = new Dictionary<FaultSet, CounterExample>();
			var exceptions = new Dictionary<FaultSet, Exception>();

			// Store the serialized model to improve performance
			var serializer = new RuntimeModelSerializer();
			serializer.Serialize(model, !hazard);

			// We check fault sets by increasing cardinality; this is, we check the empty set first, then
			// all singleton sets, then all sets with two elements, etc. We don't check sets that we
			// know are going to be critical sets due to monotonicity
			for (var cardinality = 0; cardinality <= allFaults.Length; ++cardinality)
			{
				// Generate the sets for the current level that we'll have to check
				var sets = GeneratePowerSetLevel(safeSets, criticalSets, cardinality, allFaults);

				// Clear the safe sets, we don't need the previous level to generate the next one
				safeSets.Clear();

				// If there are no sets to check, we're done; this happens when there are so many critical sets
				// that this level does not contain any set that is not a super set of any of those critical sets
				if (sets.Count == 0)
					break;

				// Remove all sets that conflict with the forced or suppressed faults; these sets are considered to be safe.
				// If no sets remain, skip to the next level
				sets = RemoveInvalidSets(sets, suppressedSet, forcedSet, safeSets);
				if (sets.Count == 0)
					continue;

				// Abort if we've exceeded the maximum fault set cardinality; doing the check here allows us
				// to report the analysis as complete if the maximum cardinality is never reached
				if (cardinality > maxCardinality)
				{
					isComplete = false;
					break;
				}

				if (cardinality == 0)
					ConsoleHelpers.WriteLine("Checking the empty fault set...");
				else
					ConsoleHelpers.WriteLine($"Checking {sets.Count} sets of cardinality {cardinality}...");

				// We have to check each set; if one of them is a critical set, it has no effect on the other
				// sets we have to check
				foreach (var set in sets)
				{
					// Enable or disable the faults that the set represents
					set.SetActivation(nondeterministicFaults);

					// If there was a counter example, the set is a critical set
					try
					{
						var result = _modelChecker.CheckInvariant(CreateRuntimeModel(serializer, allFaults));

						if (!result.FormulaHolds)
						{
							ConsoleHelpers.WriteLine($"    critical:  {{ {set.ToString(allFaults)} }}", ConsoleColor.DarkRed);
							criticalSets.Add(set);
						}
						else
							safeSets.Add(set);

						checkedSets.Add(set);

						if (result.CounterExample != null)
							counterExamples.Add(set, result.CounterExample);
					}
					catch (AnalysisException e)
					{
						ConsoleHelpers.WriteLine($"    critical:  {{ {set.ToString(allFaults)} }} [exception thrown]", ConsoleColor.DarkRed);

						checkedSets.Add(set);
						criticalSets.Add(set);

						exceptions.Add(set, e.InnerException);

						if (e.CounterExample != null)
							counterExamples.Add(set, e.CounterExample);
					}
				}
			}

			// Reset the nondeterministic faults so as to not influence subsequent analyses
			foreach (var fault in nondeterministicFaults)
				fault.Activation = Activation.Nondeterministic;

			return new Result(
				model, isComplete, criticalSets, checkedSets,
				allFaults, suppressedFaults, forcedFaults,
				counterExamples, exceptions, stopwatch.Elapsed);
		}

		/// <summary>
		///   Creates a <see cref="RuntimeModel" /> instance.
		/// </summary>
		private static Func<RuntimeModel> CreateRuntimeModel(RuntimeModelSerializer serializer, Fault[] faultTemplates)
		{
			return () =>
			{
				var serializedData = serializer.LoadSerializedData();
				var faults = serializedData.ObjectTable.OfType<Fault>().Where(f => f.Identifier >= 0).OrderBy(f => f.Identifier).ToArray();
				Requires.That(faults.Length == faultTemplates.Length, "Unexpected fault count.");

				for (var i = 0; i < faults.Length; ++i)
				{
					Requires.That(faults[i].Identifier == faultTemplates[i].Identifier, "Fault mismatch.");
					faults[i].Activation = faultTemplates[i].Activation;
				}

				return new RuntimeModel(serializedData);
			};
		}

		/// <summary>
		///   Generates a level of the power set.
		/// </summary>
		/// <param name="safeSets">The set of safe sets generated at the previous level.</param>
		/// <param name="criticalSets">The sets that are known to be critical sets. All super sets are discarded.</param>
		/// <param name="cardinality">The cardinality of the sets that should be generated.</param>
		/// <param name="faults">The fault set the power set is generated for.</param>
		private static HashSet<FaultSet> GeneratePowerSetLevel(HashSet<FaultSet> safeSets, HashSet<FaultSet> criticalSets, int cardinality,
															   Fault[] faults)
		{
			var result = new HashSet<FaultSet>();

			switch (cardinality)
			{
				case 0:
					// There is only the empty set with a cardinality of 0
					result.Add(new FaultSet());
					break;
				case 1:
					// We have to kick things off by explicitly generating the singleton sets; at this point,
					// we know that there are no further minimal critical sets if we've already found one (= the empty set)
					if (criticalSets.Count == 0)
					{
						foreach (var fault in faults)
							result.Add(new FaultSet(fault));
					}
					break;
				default:
					// We now generate the sets with the requested cardinality based on the sets from the previous level 
					// which had a cardinality that is one less than the sets we're going to generate now. The basic
					// idea is that we create the union between all safe sets and all singleton sets and discard
					// the ones we don't want
					foreach (var safeSet in safeSets)
					{
						foreach (var fault in faults)
						{
							// If we're trying to add an element to the set that it already contains, we get a set
							// we've already checked before; discard it
							if (safeSet.Contains(fault))
								continue;

							var set = safeSet.Add(fault);

							// Check if the newly generated set it a super set of any critical sets; if so, discard it
							if (criticalSets.All(s => !s.IsSubsetOf(set)))
								result.Add(set);
						}
					}
					break;
			}

			return result;
		}

		/// <summary>
		///   Removes all invalid sets from <paramref name="sets" /> that conflict with either <paramref name="suppressedFaults" /> or
		///   <paramref name="forcedFaults" />.
		/// </summary>
		private static HashSet<FaultSet> RemoveInvalidSets(HashSet<FaultSet> sets, FaultSet suppressedFaults, FaultSet forcedFaults,
														   HashSet<FaultSet> safeSets)
		{
			var validSets = new HashSet<FaultSet>();

			foreach (var set in sets)
			{
				// The set must contain all forced faults, hence it must be a superset of those
				if (!forcedFaults.IsSubsetOf(set))
				{
					safeSets.Add(set);
					continue;
				}

				// The set is not allowed to contain any suppressed faults, hence the intersection must be empty
				if (!suppressedFaults.GetIntersection(set).IsEmpty)
				{
					safeSets.Add(set);
					continue;
				}

				validSets.Add(set);
			}

			return validSets;
		}

		/// <summary>
		///   Represents the result of a safety analysis.
		/// </summary>
		public struct Result
		{
			/// <summary>
			///   Gets the faults whose activations have been completely suppressed during analysis.
			/// </summary>
			public IEnumerable<Fault> SuppressedFaults { get; }

			/// <summary>
			///   Gets the faults whose activations have been forced during analysis.
			/// </summary>
			public IEnumerable<Fault> ForcedFaults { get; }

			/// <summary>
			///   Gets the minimal critical sets, each critical set containing the faults that potentially result in the occurrence of a
			///   hazard.
			/// </summary>
			public ISet<ISet<Fault>> MinimalCriticalSets { get; }

			/// <summary>
			///   Gets all of the fault sets that were checked for criticality. Some sets might not have been checked as they were known to
			///   be critical sets due to the monotonicity of the critical set property.
			/// </summary>
			public ISet<ISet<Fault>> CheckedSets { get; }

			/// <summary>
			///   Gets the exception that has been thrown during the analysis, if any.
			/// </summary>
			public IDictionary<ISet<Fault>, Exception> Exceptions { get; }

			/// <summary>
			///   Gets the faults that have been checked.
			/// </summary>
			public IEnumerable<Fault> Faults { get; }

			/// <summary>
			///   Gets the counter examples that were generated for the critical fault sets.
			/// </summary>
			public IDictionary<ISet<Fault>, CounterExample> CounterExamples { get; }

			/// <summary>
			///   Gets a value indicating whether the analysis might is complete, i.e., all fault sets have been checked for criticality.
			/// </summary>
			public bool IsComplete { get; }

			/// <summary>
			///   Gets the <see cref="Model" /> instance the safety analysis was conducted for.
			/// </summary>
			public ModelBase Model { get; }

			/// <summary>
			///   Gets the time it took to complete the analysis.
			/// </summary>
			public TimeSpan Time { get; }

			/// <summary>
			///   Initializes a new instance.
			/// </summary>
			/// <param name="model">The <see cref="Model" /> instance the safety analysis was conducted for.</param>
			/// <param name="isComplete">Indicates whether the analysis is complete.</param>
			/// <param name="criticalSets">The minimal critical sets.</param>
			/// <param name="checkedSets">The sets that have been checked.</param>
			/// <param name="faults">The faults that have been checked.</param>
			/// <param name="counterExamples">The counter examples that were generated for the critical fault sets.</param>
			/// <param name="exceptions">The exceptions that have been thrown during the analysis.</param>
			/// <param name="time">The time it took to complete the analysis.</param>
			/// <param name="suppressedFaults">The faults whose activations have been completely suppressed during analysis.</param>
			/// <param name="forcedFaults">The faults whose activations have been forced during analysis.</param>
			internal Result(ModelBase model, bool isComplete, HashSet<FaultSet> criticalSets, HashSet<FaultSet> checkedSets,
							Fault[] faults, Fault[] suppressedFaults, Fault[] forcedFaults,
							Dictionary<FaultSet, CounterExample> counterExamples, Dictionary<FaultSet, Exception> exceptions, TimeSpan time)
			{
				var knownFaultSets = new Dictionary<FaultSet, ISet<Fault>>();

				Time = time;
				Model = model;
				IsComplete = isComplete;
				MinimalCriticalSets = Convert(knownFaultSets, criticalSets, faults);
				CheckedSets = Convert(knownFaultSets, checkedSets, faults);
				Faults = faults;
				CounterExamples = counterExamples.ToDictionary(pair => Convert(knownFaultSets, pair.Key, faults), pair => pair.Value);
				Exceptions = exceptions.ToDictionary(pair => Convert(knownFaultSets, pair.Key, faults), pair => pair.Value);
				SuppressedFaults = suppressedFaults;
				ForcedFaults = forcedFaults;
			}

			/// <summary>
			///   Converts the integer-based sets to a sets of fault sets.
			/// </summary>
			private static ISet<ISet<Fault>> Convert(Dictionary<FaultSet, ISet<Fault>> knownSets, HashSet<FaultSet> sets, Fault[] faults)
			{
				var result = new HashSet<ISet<Fault>>(ReferenceEqualityComparer<ISet<Fault>>.Default);

				foreach (var set in sets)
					result.Add(Convert(knownSets, set, faults));

				return result;
			}

			/// <summary>
			///   Converts the integer-based set to a set faults.
			/// </summary>
			private static ISet<Fault> Convert(Dictionary<FaultSet, ISet<Fault>> knownSets, FaultSet set, Fault[] faults)
			{
				ISet<Fault> faultSet;
				if (knownSets.TryGetValue(set, out faultSet))
					return faultSet;

				faultSet = new HashSet<Fault>(set.ToFaultSequence(faults));
				knownSets.Add(set, faultSet);

				return faultSet;
			}

			/// <summary>
			/// 
			/// </summary>
			/// <param name="directory">The directory the generated counter examples should be written to.</param>
			/// <param name="clearFiles">Indicates whether all files in the directory should be cleared before saving the counter examples.</param>
			public void SaveCounterExamples(string directory, bool clearFiles = true)
			{
				Requires.NotNullOrWhitespace(directory, nameof(directory));

				if (clearFiles && Directory.Exists(directory))
				{
					foreach (var file in new DirectoryInfo(directory).GetFiles())
						file.Delete();
				}

				foreach (var pair in CounterExamples)
				{
					var fileName = String.Join("_", pair.Key.Select(f => f.Name));
					if (String.IsNullOrWhiteSpace(fileName))
						fileName = "emptyset";

					pair.Value.Save(Path.Combine(directory, $"{fileName}{CounterExample.FileExtension}"));
				}
			}

			/// <summary>
			///   Returns a string representation of the minimal critical fault sets.
			/// </summary>
			public override string ToString()
			{
				var builder = new StringBuilder();
				var percentage = CheckedSets.Count / (float)(1 << Faults.Count()) * 100;

				builder.AppendLine();
				builder.AppendLine("=======================================================================");
				builder.AppendLine("=======      Deductive Cause Consequence Analysis: Results      =======");
				builder.AppendLine("=======================================================================");
				builder.AppendLine();

				if (Exceptions.Any())
				{
					builder.AppendLine("*** Warning: Unhandled exceptions have been thrown during the analysis. ***");
					builder.AppendLine();
				}

				if (!IsComplete)
				{
					builder.AppendLine("*** Warning: Analysis might be incomplete; not all fault sets have been checked. ***");
					builder.AppendLine();
				}

				Func<IEnumerable<Fault>, string> getFaultString =
					faults => String.Join(", ", faults.Select(fault => fault.Name).OrderBy(name => name));

				builder.AppendFormat("Elapsed Time: {0}", Time);
				builder.AppendLine();
				builder.AppendFormat("Fault Count: {0}", Faults.Count());
				builder.AppendLine();
				builder.AppendFormat("Faults: {0}", getFaultString(Faults));
				builder.AppendLine();

				if (ForcedFaults.Any())
				{
					builder.AppendFormat("Forced Faults: {0}", getFaultString(ForcedFaults));
					builder.AppendLine();
				}

				if (SuppressedFaults.Any())
				{
					builder.AppendFormat("Suppressed Faults: {0}", getFaultString(SuppressedFaults));
					builder.AppendLine();
				}

				builder.AppendLine();

				builder.AppendFormat("Checked Fault Sets: {0} ({1:F0}% of all fault sets)", CheckedSets.Count, percentage);
				builder.AppendLine();

				builder.AppendFormat("Minimal Critical Sets: {0}", MinimalCriticalSets.Count);
				builder.AppendLine();
				builder.AppendLine();

				var i = 1;
				foreach (var criticalSet in MinimalCriticalSets)
				{
					builder.AppendFormat("   ({1}) {{ {0} }}", String.Join(", ", criticalSet.Select(fault => fault.Name).OrderBy(name => name)), i++);

					Exception e;
					if (Exceptions.TryGetValue(criticalSet, out e))
					{
						builder.AppendLine();
						builder.AppendFormat(
							"    An unhandled exception of type {0} was thrown while checking the fault set: {1}",
							e.GetType().FullName, e.Message);
					}

					builder.AppendLine();
				}

				return builder.ToString();
			}
		}
	}
}