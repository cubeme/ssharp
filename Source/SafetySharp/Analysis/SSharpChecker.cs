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
	using System.Diagnostics;
	using Modeling;
	using Runtime;
	using Runtime.Serialization;
	using Utilities;

	/// <summary>
	///   Represents a model checker specifically created to check S# models.
	/// </summary>
	public class SSharpChecker
	{
		/// <summary>
		///   The model checker's configuration that determines certain model checker settings.
		/// </summary>
		public AnalysisConfiguration Configuration = AnalysisConfiguration.Default;

		/// <summary>
		///   Raised when the model checker has written an output. The output is always written to the console by default.
		/// </summary>
		public event Action<string> OutputWritten = Console.WriteLine;

		/// <summary>
		///   Checks the invariant encoded into the model created by <paramref name="createModel" />.
		/// </summary>
		internal AnalysisResult CheckInvariant(Func<RuntimeModel> createModel)
		{
			Requires.That(IntPtr.Size == 8, "Model checking is only supported in 64bit processes.");

			var stopwatch = new Stopwatch();
			stopwatch.Start();

			using (var checker = new InvariantChecker(createModel, message => OutputWritten?.Invoke(message), Configuration))
			{
				var result = default(AnalysisResult);
				var initializationTime = stopwatch.Elapsed;
				stopwatch.Restart();

				try
				{
					result = checker.Check();
					return result;
				}
				finally
				{
					stopwatch.Stop();

					if (!Configuration.ProgressReportsOnly)
					{
						OutputWritten?.Invoke(String.Empty);
						OutputWritten?.Invoke("===============================================");
						OutputWritten?.Invoke($"Initialization time: {initializationTime}");
						OutputWritten?.Invoke($"Model checking time: {stopwatch.Elapsed}");
						OutputWritten?.Invoke($"{(int)(result.StateCount / stopwatch.Elapsed.TotalSeconds):n0} states per second");
						OutputWritten?.Invoke($"{(int)(result.TransitionCount / stopwatch.Elapsed.TotalSeconds):n0} transitions per second");
						OutputWritten?.Invoke("===============================================");
						OutputWritten?.Invoke(String.Empty);
					}
				}
			}
		}

		/// <summary>
		///   Checks whether the <paramref name="invariant" /> holds in all states of the <paramref name="model" />.
		/// </summary>
		/// <param name="model">The model that should be checked.</param>
		/// <param name="invariant">The invariant that should be checked.</param>
		public AnalysisResult CheckInvariant(ModelBase model, Formula invariant)
		{
			Requires.NotNull(model, nameof(model));
			Requires.NotNull(invariant, nameof(invariant));

			var serializer = new RuntimeModelSerializer();
			serializer.Serialize(model, invariant);

			return CheckInvariant(serializer.Load);
		}
	}
}