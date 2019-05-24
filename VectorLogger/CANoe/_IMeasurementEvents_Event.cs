using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace CANoe
{
	[CompilerGenerated, ComEventInterface(typeof(_IMeasurementEvents), typeof(_IMeasurementEvents)), TypeIdentifier("7f31deb0-5bcc-11d3-8562-00105a3e017b", "CANoe._IMeasurementEvents_Event")]
	[ComImport]
	public interface _IMeasurementEvents_Event
	{
		event _IMeasurementEvents_OnInitEventHandler OnInit;

		event _IMeasurementEvents_OnStartEventHandler OnStart;

		event _IMeasurementEvents_OnStopEventHandler OnStop;

		event _IMeasurementEvents_OnExitEventHandler OnExit;
	}
}
