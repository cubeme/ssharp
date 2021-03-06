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

namespace Tests
{
	using SafetySharp.Compiler.Normalization;
	using Utilities;
	using Xunit;

	public partial class NormalizationTests : Tests
	{
		[Theory, MemberData("DiscoverTests", "Normalization/LiftedExpressions")]
		public void LiftedExpressions(string test, string file)
		{
			CheckNormalization<LiftedExpressionNormalizer>(file);
		}

		[Theory, MemberData("DiscoverTests", "Normalization/Partial")]
		public void Partial(string test, string file)
		{
			CheckNormalization<PartialNormalizer>(file);
		}

		[Theory, MemberData("DiscoverTests", "Normalization/FaultNames")]
		public void FaultNames(string test, string file)
		{
			CheckNormalization<FaultNameNormalizer>(file);
		}

		[Theory, MemberData("DiscoverTests", "Normalization/LineCounts")]
		public void LineCounts(string test, string file)
		{
			ExecuteDynamicTests(file, file);
		}
	}
}