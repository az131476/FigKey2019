using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[Flags]
	public enum LEDItemParameter
	{
		[EnumMember(Value = "None")]
		None = 0,
		[EnumMember(Value = "MemorySingle")]
		MemorySingle = 1,
		[EnumMember(Value = "MemoryWithORWildcard")]
		MemoryWithORWildcard = 2,
		[EnumMember(Value = "MemoryWithANDWildcard")]
		MemoryWithANDWildcard = 4,
		[EnumMember(Value = "ChannelSingle")]
		ChannelSingle = 16,
		[EnumMember(Value = "ChannelWithWildcard")]
		ChannelWithWildcard = 32
	}
}
