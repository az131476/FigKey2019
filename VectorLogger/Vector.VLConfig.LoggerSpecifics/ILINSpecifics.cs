using System;
using System.Collections.Generic;

namespace Vector.VLConfig.LoggerSpecifics
{
	public interface ILINSpecifics
	{
		uint NumberOfChannels
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

		uint NumberOfLINprobeChannels
		{
			get;
		}
	}
}
