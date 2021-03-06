// The MIT License (MIT)
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

namespace SafetySharp.CaseStudies.HemodialysisMachine.Modeling.DialyzingFluidDeliverySystem
{
	using SafetySharp.Modeling;

	public class Pump : Component
	{
		public readonly DialyzingFluidFlowInToOut MainFlow;
		
		public Pump()
		{
			MainFlow = new DialyzingFluidFlowInToOut();
			MainFlow.UpdateBackward = SetMainFlowSuction;
			MainFlow.UpdateForward = SetMainFlow;
		}

		[Range(0, 8, OverflowBehavior.Error)]
		public int PumpSpeed = 0;
		
		public void SetMainFlow(DialyzingFluid  toSuccessor, DialyzingFluid  fromPredecessor)
		{
			toSuccessor.CopyValuesFrom(fromPredecessor);
		}
		
		public virtual void SetMainFlowSuction(Suction fromSuccessor, Suction toPredecessor)
		{
			toPredecessor.SuctionType = SuctionType.CustomSuction;
			toPredecessor.CustomSuctionValue = PumpSpeed;
		}
		
		public readonly Fault PumpDefect = new TransientFault();

		[FaultEffect(Fault = nameof(PumpDefect))]
		public class PumpDefectEffect : Pump
		{
			[Provided]
			public override void SetMainFlowSuction(Suction fromSuccessor, Suction toPredecessor)
			{
				toPredecessor.SuctionType = SuctionType.CustomSuction;
				toPredecessor.CustomSuctionValue = 0;
			}
		}
	}
}