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

namespace SafetySharp.CaseStudies.HeightControl.Analysis
{
	using System;
	using System.Collections;
	using System.Linq;
	using FluentAssertions;
	using Modeling;
	using Modeling.Controllers;
	using NUnit.Framework;
	using SafetySharp.Analysis;
	using SafetySharp.Modeling;

	public class ModelCheckingTests
	{
		[Test]
		public void EnumerateAllStatesOriginalDesign()
		{
			var model = Model.CreateOriginal();
			var result = ModelChecker.CheckInvariant(model, true);

			result.FormulaHolds.Should().BeTrue();
		}

		[Test]
		public void CollisionOriginalDesign()
		{
			var model = Model.CreateOriginal();

			// As collisions cannot occur without any overheight vehicles driving on the left lane, we 
			// force the activation of the LeftOHV fault to improve safety analysis times significantly
			model.LeftOHV.Activation = Activation.Forced; 

			var result = SafetyAnalysis.AnalyzeHazard(model, model.Collision);

			result.SaveCounterExamples("counter examples/height control/dcca/collision/original");
			Console.WriteLine(result);
		}

		[Test]
		public void FalseAlarmOriginalDesign()
		{
			var model = Model.CreateOriginal();

			// As false alarms cannot occur with any overheight vehicle on the left lane in the original design, we 
			// suppress the activation of the LeftOHV fault to improve safety analysis times significantly
			model.LeftOHV.Activation = Activation.Suppressed;

			var result = SafetyAnalysis.AnalyzeHazard(model, model.FalseAlarm);

			result.SaveCounterExamples("counter examples/height control/dcca/false alarm/original");
			Console.WriteLine(result);
		}

		[Test, TestCaseSource(nameof(CreateModelVariants))]
		public void EnumerateAllStates(Model model, string variantName)
		{
			var result = ModelChecker.CheckInvariant(model, true);
			result.FormulaHolds.Should().BeTrue();
		}

		[Test, TestCaseSource(nameof(CreateModelVariants))]
		public void Collision(Model model, string variantName)
		{
			// As collisions cannot occur without any overheight vehicles driving on the left lane, we 
			// force the activation of the LeftOHV fault to improve safety analysis times significantly
			model.LeftOHV.Activation = Activation.Forced;

			var result = SafetyAnalysis.AnalyzeHazard(model, model.Collision, maxCardinality: 3);

			result.SaveCounterExamples($"counter examples/height control/dcca/collision/{variantName}");
			Console.WriteLine(result);
		}

		[Test, TestCaseSource(nameof(CreateModelVariants))]
		public void FalseAlarm(Model model, string variantName)
		{
			// We cannot suppress LeftOHV here as at least some design variants might potentially be
			// affected by overheight vehicles on the left lane at the pre control

			var result = SafetyAnalysis.AnalyzeHazard(model, model.FalseAlarm, maxCardinality: 3);

			result.SaveCounterExamples($"counter examples/height control/dcca/false alarm/{variantName}");
			Console.WriteLine(result);
		}

		private static IEnumerable CreateModelVariants()
		{
			return from model in Model.CreateVariants()
				   let name = $"{model.HeightControl.PreControl.GetType().Name.Substring(nameof(PreControl).Length)}-" +
							  $"{model.HeightControl.MainControl.GetType().Name.Substring(nameof(MainControl).Length)}-" +
							  $"{model.HeightControl.EndControl.GetType().Name.Substring(nameof(EndControl).Length)}"
				   select new TestCaseData(model, name).SetName(name);
		}
	}
}