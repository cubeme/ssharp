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

namespace Tests.Analysis.Dcca
{
	using System.Linq;
	using SafetySharp.Modeling;
	using Shouldly;

	internal class AllFaultsForced : AnalysisTestObject
	{
		protected override void Check()
		{
			var c = new C();
			c.F1.ForceActivation();
			c.F2.ForceActivation();
			c.F3.ForceActivation();
			var result = Dcca(c.X > 4, c);

			result.CheckedSets.Count.ShouldBe(1);
			result.MinimalCriticalSets.Count.ShouldBe(1);
			result.Exceptions.ShouldBeEmpty();
			result.IsComplete.ShouldBe(true);
			result.ForcedFaults.ShouldBe(new[] { c.F1, c.F2, c.F3 }, ignoreOrder: true);
			result.SuppressedFaults.ShouldBeEmpty();

			ShouldContain(result.CheckedSets, c.F1, c.F2, c.F3);
			ShouldContain(result.MinimalCriticalSets, c.F1, c.F2, c.F3);

			result.CounterExamples.Count.ShouldBe(1);
			foreach (var set in result.MinimalCriticalSets)
				result.CounterExamples.ContainsKey(set).ShouldBe(true);

			SimulateCounterExample(result.CounterExamples.Values.Single(), simulator =>
			{
				var component = (C)simulator.Model.Roots[0];

				component.X.ShouldBe(0);

				while (!simulator.IsCompleted)
					simulator.SimulateStep();

				component.X.ShouldBe(6);
			});

			result = DccaWithMaxCardinality(c.X > 4, 1, c);

			result.CheckedSets.Count.ShouldBe(0);
			result.MinimalCriticalSets.ShouldBeEmpty();
			result.Exceptions.ShouldBeEmpty();
			result.IsComplete.ShouldBe(false);
			result.ForcedFaults.ShouldBe(new[] { c.F1, c.F2, c.F3 }, ignoreOrder: true);
			result.SuppressedFaults.ShouldBeEmpty();
		}

		private class C : Component
		{
			public readonly Fault F1 = new TransientFault();
			public readonly Fault F2 = new PermanentFault();
			public readonly Fault F3 = new PermanentFault();
			public int X;

			public override void Update()
			{
				X = 3;
			}

			[FaultEffect(Fault = nameof(F1))]
			[Priority(1)]
			private class E1 : C
			{
				public override void Update()
				{
					base.Update();
					X += 1;
				}
			}

			[FaultEffect(Fault = nameof(F2))]
			[Priority(2)]
			private class E2 : C
			{
				public override void Update()
				{
					base.Update();
					X += 1;
				}
			}

			[FaultEffect(Fault = nameof(F3))]
			[Priority(3)]
			private class E3 : C
			{
				public override void Update()
				{
					base.Update();
					X += 1;
				}
			}
		}
	}
}