using System;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.HardwareAccess.HardwareAbstraction
{
	public interface IMultibusChannelDevice
	{
		string GetTransceiverTypeName(int channelNumber);

		BusType GetTransceiverBusType(int channelNumber);

		bool GetCANTransceiverCapabilities(int channelNumber, out bool isLowSpeed, out bool isFD);
	}
}
