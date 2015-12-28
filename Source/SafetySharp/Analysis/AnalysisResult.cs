﻿// The MIT License (MIT)
// 
// Copyright (c) 2014-2015, Institute for Software & Systems Engineering
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
	/// <summary>
	///   Describes the result of a model checking based analysis.
	/// </summary>
	public struct AnalysisResult
	{
		/// <summary>
		///   Gets the counter example that has been generated by the model checker, if any.
		/// </summary>
		public CounterExample CounterExample { get; }

		/// <summary>
		///   The number of states checked by the model checker.
		/// </summary>
		public int StateCount { get; }

		/// <summary>
		///   The number of transitions checked by the model checker.
		/// </summary>
		public long TransitionCount { get; }

		/// <summary>
		///   The number of levels checked by the model checker.
		/// </summary>
		public int LevelCount { get; }

		/// <summary>
		///   Initializes a new instance.
		/// </summary>
		internal AnalysisResult(CounterExample counterExample, int stateCount, long transitionCount, int levelCount)
		{
			CounterExample = counterExample;
			StateCount = stateCount;
			TransitionCount = transitionCount;
			LevelCount = levelCount;
		}
	}
}