using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	public enum DataTransferModeType
	{
		[EnumMember(Value = "StopWhileDataTransfer")]
		StopWhileDataTransfer,
		[EnumMember(Value = "RecordWhileDataTransfer")]
		RecordWhileDataTransfer
	}
}
