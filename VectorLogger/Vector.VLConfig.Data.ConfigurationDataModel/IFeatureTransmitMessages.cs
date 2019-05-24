using System;
using System.Collections.Generic;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	public interface IFeatureTransmitMessages
	{
		IList<ITransmitMessageChannel> ActiveTransmitMessageChannels
		{
			get;
		}
	}
}
