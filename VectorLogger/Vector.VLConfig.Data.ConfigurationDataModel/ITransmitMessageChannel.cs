using System;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	public interface ITransmitMessageChannel
	{
		ValidatedProperty<BusType> BusType
		{
			get;
		}

		ValidatedProperty<uint> ChannelNumber
		{
			get;
		}
	}
}
