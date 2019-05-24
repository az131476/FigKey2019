using System;

namespace Vector.VLConfig.LoggerSpecifics
{
	public enum LEDState
	{
		Disabled,
		AlwaysOn,
		AlwaysBlinking,
		TriggerActive,
		LoggingActive,
		EndOfMeasurement,
		MemoryFull,
		CANLINError,
		CANError,
		LINError,
		CANoeMeasurementOn,
		MOSTLock,
		CcpXcpError,
		CcpXcpOk
	}
}
