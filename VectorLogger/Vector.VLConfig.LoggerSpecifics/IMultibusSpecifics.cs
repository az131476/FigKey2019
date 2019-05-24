using System;

namespace Vector.VLConfig.LoggerSpecifics
{
	public interface IMultibusSpecifics
	{
		uint NumberOfChannels
		{
			get;
		}

		uint NumberOfPiggyConfigurableChannels
		{
			get;
		}

		uint PiggyConfigurableChannelsStartIndex
		{
			get;
		}

		uint ChannelLINStartIndex
		{
			get;
		}

		uint NumberOfBuildInCANTransceivers
		{
			get;
		}
	}
}
