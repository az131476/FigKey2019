using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	public enum EthernetProtocol
	{
		[EnumMember(Value = "Unspecified")]
		Unspecified,
		[EnumMember(Value = "TCP")]
		TCP,
		[EnumMember(Value = "UDP")]
		UDP
	}
}
