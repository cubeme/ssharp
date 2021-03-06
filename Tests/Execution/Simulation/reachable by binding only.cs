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

namespace Tests.Execution.Simulation
{
	using System.Linq;
	using SafetySharp.Analysis;
	using SafetySharp.Modeling;
	using Shouldly;
	using Utilities;

	internal class ReachableByBindingOnly : TestObject
	{
		protected override void Check()
		{
			var c = new C();

			var simulator = new Simulator(TestModel.InitializeModel(c));
			c = (C)simulator.Model.Roots[0];

			simulator.SimulateStep();
			c.X.ShouldBe(7);

			simulator.Model.Components.Length.ShouldBe(2);
			simulator.Model.Components.OfType<C>().Count().ShouldBe(1);
			simulator.Model.Components.OfType<D>().Count().ShouldBe(1);
		}

		private class C : Component
		{
			public int X;

			private extern int Y { get; }

			public override void Update()
			{
				X = Y;
			}

			protected internal override void CreateBindings()
			{
				var d = new D();
				Bind(nameof(Y), nameof(d.Z));
			}
		}

		private class D : Component
		{
			public int Z => 7;
		}
	}
}