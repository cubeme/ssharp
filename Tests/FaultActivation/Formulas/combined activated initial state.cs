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

namespace Tests.FaultActivation.Formulas
{
	using SafetySharp.Modeling;
	using Shouldly;

	internal class CombinedActivatedInitialState : AnalysisTestObject
	{
		protected override void Check()
		{
			var c = new C();
			CheckInvariant(!c.F1.IsActivated, c).ShouldBe(false);
			CheckInvariant(!c.F2.IsActivated, c).ShouldBe(false);
			CheckInvariant(!(c.F1.IsActivated && c.F2.IsActivated), c).ShouldBe(false);
		}

		private class C : Component
		{
			public readonly Fault F1 = new TransientFault();
			public readonly Fault F2 = new TransientFault();

			private int _x;

			public virtual int X => 1;

			protected internal override void Initialize()
			{
				_x = X;
			}

			[FaultEffect(Fault = nameof(F1)), Priority(0)]
			public class E1 : C
			{
				public override int X => base.X + 1;
			}

			[FaultEffect(Fault = nameof(F2)), Priority(1)]
			public class E2 : C
			{
				public override int X => base.X + 2;
			}
		}
	}
}