using System;
using System.Collections.Generic;

namespace Vector.VLConfig.LoggerSpecifics
{
	public interface ICANSpecifics
	{
		uint NumberOfChannels
		{
			get;
		}

		uint NumberOfVirtualChannels
		{
			get;
		}

		IList<uint> DefaultActiveChannels
		{
			get;
		}

		IList<uint> ChannelsWithWakeUpSupport
		{
			get;
		}

		IList<uint> ChannelsWithOptionalTransceivers
		{
			get;
		}

		IList<uint> ChannelsWithOutputSupport
		{
			get;
		}

		uint MaxPrescalerValue
		{
			get;
		}

		uint AuxChannel
		{
			get;
		}

		uint AuxChannelMaxPrescalerValue
		{
			get;
		}

		bool IsFDSupported
		{
			get;
		}
	}
}
