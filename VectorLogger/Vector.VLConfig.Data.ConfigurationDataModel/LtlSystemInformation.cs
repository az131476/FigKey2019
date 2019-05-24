using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	public enum LtlSystemInformation
	{
		[EnumMember(Value = "None")]
		None,
		[EnumMember(Value = "Logger1Files")]
		Logger1Files,
		[EnumMember(Value = "Logger2Files")]
		Logger2Files,
		[EnumMember(Value = "LoggerFilesTotal")]
		LoggerFilesTotal,
		[EnumMember(Value = "LoggerMBsFree")]
		LoggerMBsFree,
		[EnumMember(Value = "Stopped1")]
		Stopped1,
		[EnumMember(Value = "Stopped2")]
		Stopped2,
		[EnumMember(Value = "NotStopped1")]
		NotStopped1,
		[EnumMember(Value = "NotStopped2")]
		NotStopped2,
		[EnumMember(Value = "FlashFull")]
		FlashFull
	}
}
