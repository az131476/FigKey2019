using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	public enum CcpXcpMeasurementMode
	{
		[EnumMember(Value = "DAQ")]
		DAQ,
		[EnumMember(Value = "Polling")]
		Polling
	}
}
