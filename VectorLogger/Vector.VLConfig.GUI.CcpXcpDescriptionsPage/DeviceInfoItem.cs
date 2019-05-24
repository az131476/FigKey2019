using System;
using Vector.McModule;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.CcpXcpDescriptionsPage
{
	internal class DeviceInfoItem
	{
		private IDeviceInfo mDeviceInfo;

		public IDeviceInfo DeviceInfo
		{
			get
			{
				return this.mDeviceInfo;
			}
		}

		public DeviceInfoItem(IDeviceInfo deviceInfo)
		{
			this.mDeviceInfo = deviceInfo;
		}

		public override string ToString()
		{
			string text;
			if (this.mDeviceInfo.ProtocolType == EnumProtocolType.ProtocolCcp)
			{
				text = Vocabulary.CPTypeShortNameCCP;
			}
			else
			{
				switch (this.mDeviceInfo.TransportType)
				{
				case EnumXcpTransportType.kCan:
					text = Vocabulary.XcpOnCan;
					break;
				case EnumXcpTransportType.kTcp:
					text = Vocabulary.XcpOnEthernetTcp;
					break;
				case EnumXcpTransportType.kUdp:
					text = Vocabulary.XcpOnEthernetUdp;
					break;
				case EnumXcpTransportType.kFlexRay:
					text = Vocabulary.XcpOnFlexRay;
					break;
				default:
					throw new Exception("No string available!");
				}
				if (!string.IsNullOrWhiteSpace(this.mDeviceInfo.TransportLayerInstanceName))
				{
					if (this.mDeviceInfo.TransportLayerInstanceName == CcpXcpEcu.DefaultTransportLayerInstanceNameForVx())
					{
						text = text + " - " + Resources_CcpXcp.CcpXcpEcuVxDefaultInterface;
					}
					else
					{
						text = text + " - " + this.mDeviceInfo.TransportLayerInstanceName;
					}
				}
			}
			return text;
		}
	}
}
