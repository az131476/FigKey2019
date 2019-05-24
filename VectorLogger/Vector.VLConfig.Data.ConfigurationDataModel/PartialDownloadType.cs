using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	public enum PartialDownloadType
	{
		[EnumMember(Value = "PartialDownloadOff")]
		PartialDownloadOff,
		[EnumMember(Value = "PartialDownloadOn")]
		PartialDownloadOn,
		[EnumMember(Value = "PartialDownloadOnInSameFolder")]
		PartialDownloadOnInSameFolder
	}
}
