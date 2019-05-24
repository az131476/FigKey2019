using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	public enum TimeUnit
	{
		[EnumMember(Value = "MilliSec")]
		MilliSec,
		[EnumMember(Value = "Sec")]
		Sec,
		[EnumMember(Value = "Min")]
		Min
	}
}
