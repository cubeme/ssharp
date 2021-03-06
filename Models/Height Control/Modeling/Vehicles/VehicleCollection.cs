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

namespace SafetySharp.CaseStudies.HeightControl.Modeling.Vehicles
{
	using SafetySharp.Modeling;
	using Sensors;

	/// <summary>
	///   Represents a collection of vehicles.
	/// </summary>
	public sealed class VehicleCollection : Component
	{
		/// <summary>
		///   The vehicles contained in the collection.
		/// </summary>
		[Hidden(HideElements = true)]
		public readonly Vehicle[] Vehicles;

		/// <summary>
		///   Initializes a new instance.
		/// </summary>
		public VehicleCollection(params Vehicle[] vehicles)
		{
			Vehicles = vehicles;

			foreach (var vehicle in Vehicles)
				Bind(nameof(vehicle.IsTunnelClosed), nameof(ForwardIsTunnelClosed));
		}

		// TODO: Remove once S# supports port forwardings
		private bool ForwardIsTunnelClosed => IsTunnelClosed;

		/// <summary>
		///   Informs the vehicle whether the tunnel is closed.
		/// </summary>
		public extern bool IsTunnelClosed { get; }

		/// <summary>
		///   Updates the state of the component.
		/// </summary>
		public override void Update()
		{
			Update(Vehicles);
		}

		/// <summary>
		///   Checks whether the <paramref name="detector" /> detects any vehicles.
		/// </summary>
		/// <param name="detector">The detector that should observe the vehicles.</param>
		public bool ObserveVehicles(VehicleDetector detector)
		{
			// Ideally, we'd just use the following line instead of the for-loop below; however, it generates
			// a delegate and probably an interator each time the method is called, therefore increasing the 
			// pressure on the garbage collector; roughly 250 million heap allocations would be required to
			// check the case study's original design. All in all, model checking times increase noticeably, in 
			// some cases by 40% or more...

			// return Vehicles.Any(detector.DetectsVehicle);

			foreach (var vehicle in Vehicles)
			{
				if (detector.DetectsVehicle(vehicle))
					return true;
			}

			return false;
		}
	}
}