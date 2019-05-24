using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CANalyzer
{
	[CompilerGenerated, ComEventInterface(typeof(_IMeasurementEvents), typeof(_IMeasurementEvents)), TypeIdentifier("4cb02fc0-4f33-11d3-854d-00105a3e017b", "CANalyzer._IMeasurementEvents_Event")]
	[ComImport]
	public interface _IMeasurementEvents_Event
	{
		event _IMeasurementEvents_OnInitEventHandler OnInit;

		event _IMeasurementEvents_OnStartEventHandler OnStart;

		event _IMeasurementEvents_OnStopEventHandler OnStop;

		event _IMeasurementEvents_OnExitEventHandler OnExit;
	}
}
