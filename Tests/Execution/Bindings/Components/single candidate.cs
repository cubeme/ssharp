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

namespace Tests.Execution.Bindings.Components
{
	using Shouldly;
	using Utilities;

	internal class Z1 : TestComponent
	{
		public Z1()
		{
			Bind(nameof(N), nameof(M));
		}

		private int M()
		{
			return 1;
		}

		private extern int N();

		protected override void Check()
		{
			N().ShouldBe(1);
		}
	}

	internal class Z1b : TestComponent
	{
		private Z1b _f;

		public Z1b()
		{
			_f = this;
			Bind(nameof(_f.N), nameof(_f.M));
		}

		private int M()
		{
			return 1;
		}

		private extern int N();

		protected override void Check()
		{
			N().ShouldBe(1);
		}
	}

	internal class Z1c : TestComponent
	{
		private Z1c F { get; set; }

		public Z1c()
		{
			F = this;
			Bind(nameof(F.N), nameof(F.M));
		}

		private int M()
		{
			return 1;
		}

		private extern int N();

		protected override void Check()
		{
			N().ShouldBe(1);
		}
	}
}