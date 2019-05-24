using System;
using System.Collections.Generic;

namespace Vector.VLConfig.LoggerSpecifics
{
	public interface IFlexraySpecifics
	{
		uint NumberOfChannels
		{
			get;
		}

		IList<uint> DefaultActiveChannels
		{
			get;
		}
	}
}
