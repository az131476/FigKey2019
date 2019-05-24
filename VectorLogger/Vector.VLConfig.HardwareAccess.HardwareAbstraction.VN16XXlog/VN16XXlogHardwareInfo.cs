using System;
using System.Collections.Generic;
using System.Linq;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.XlApiNet;

namespace Vector.VLConfig.HardwareAccess.HardwareAbstraction.VN16XXlog
{
	public class VN16XXlogHardwareInfo
	{
		private const byte cDefaultHwType = 79;

		private const byte cDefaultHwIndex = 0;

		private const uint cMemBaseAddr = 1895825408u;

		public readonly IList<s_xl_channel_config> ChannelInfos;

		public byte HwType
		{
			get;
			private set;
		}

		public byte HwIndex
		{
			get;
			private set;
		}

		public s_xl_cobfwk_info CobInfo
		{
			get;
			set;
		}

		public uint SerialNumber
		{
			get
			{
				if (!this.ChannelInfos.Any<s_xl_channel_config>())
				{
					return 0u;
				}
				return this.ChannelInfos[0].serialNumber;
			}
		}

		public uint DriverVersion
		{
			get
			{
				if (!this.ChannelInfos.Any<s_xl_channel_config>())
				{
					return 0u;
				}
				return this.ChannelInfos[0].driverVersion;
			}
		}

		public uint FirmwareVersion
		{
			get
			{
				if (!this.ChannelInfos.Any<s_xl_channel_config>())
				{
					return 0u;
				}
				return this.ChannelInfos[0].raw_data[1];
			}
		}

		public uint[] GetChannels(XlBusType busType)
		{
			List<uint> list = new List<uint>();
			foreach (s_xl_channel_config current in this.ChannelInfos)
			{
				if (current.busParams.busType == (uint)busType)
				{
					list.Add((uint)(current.hwChannel + 1));
				}
			}
			return list.ToArray();
		}

		public bool GetFirstCanChannel(out s_xl_channel_config firstCanChannel)
		{
			firstCanChannel = default(s_xl_channel_config).Init();
			foreach (s_xl_channel_config current in this.ChannelInfos)
			{
				if (current.busParams.busType == 1u)
				{
					firstCanChannel = current;
					return true;
				}
			}
			return false;
		}

		public VN16XXlogHardwareInfo(byte hwType, byte hwIndex)
		{
			this.HwType = hwType;
			this.HwIndex = hwIndex;
			this.ChannelInfos = new List<s_xl_channel_config>();
		}

		public static VN16XXlogHardwareInfo CreateFromConfig()
		{
			VN16XXlogHardwareInfo vN16XXlogHardwareInfo = new VN16XXlogHardwareInfo(79, 0);
			MultibusChannelConfiguration multibusChannelConfiguration = GenerationUtilVN.ConfigManager.VLProject.ProjectRoot.HardwareConfiguration.MultibusChannelConfiguration;
			s_xl_cobfwk_info cobInfo = default(s_xl_cobfwk_info).Init();
			cobInfo.memBaseAddr = 1895825408u;
			vN16XXlogHardwareInfo.CobInfo = cobInfo;
			byte b = 0;
			while ((int)b < multibusChannelConfiguration.Channels.Count)
			{
				CANChannel cANChannel = multibusChannelConfiguration.Channels[(int)b] as CANChannel;
				LINChannel lINChannel = multibusChannelConfiguration.Channels[(int)b] as LINChannel;
				if (cANChannel != null)
				{
					s_xl_channel_config item = default(s_xl_channel_config).Init();
					item.channelIndex = b;
					item.hwChannel = b;
					item.busParams.busType = 1u;
					item.busParams.raw[0] = 32;
					item.busParams.raw[1] = 161;
					item.busParams.raw[2] = 7;
					item.busParams.raw[3] = 0;
					item.channelBusCapabilities = 65537u;
					item.channelCapabilities = 570466327u;
					item.transceiverName = "On board CAN 1051cap(Highspeed)";
					item.transceiverType = 316;
					vN16XXlogHardwareInfo.ChannelInfos.Add(item);
				}
				else if (lINChannel != null)
				{
					s_xl_channel_config item2 = default(s_xl_channel_config).Init();
					item2.channelIndex = b;
					item2.hwChannel = b;
					item2.busParams.busType = 2u;
					item2.busParams.raw[0] = 0;
					item2.busParams.raw[1] = 0;
					item2.busParams.raw[2] = 0;
					item2.busParams.raw[3] = 0;
					item2.channelBusCapabilities = 134351107u;
					item2.channelCapabilities = 40967u;
					item2.transceiverName = "LINpiggy 7269mag";
					item2.transceiverType = 307;
					vN16XXlogHardwareInfo.ChannelInfos.Add(item2);
				}
				b += 1;
			}
			s_xl_channel_config item3 = default(s_xl_channel_config).Init();
			item3.channelIndex = (byte)(multibusChannelConfiguration.LINChannels.Count + multibusChannelConfiguration.CANChannels.Count);
			item3.hwChannel = item3.channelIndex;
			item3.busParams.busType = 64u;
			item3.busParams.raw[0] = 0;
			item3.busParams.raw[1] = 0;
			item3.busParams.raw[2] = 0;
			item3.busParams.raw[3] = 0;
			item3.channelBusCapabilities = 4194368u;
			item3.channelCapabilities = 40967u;
			item3.transceiverName = "On board D/A IO 1021";
			item3.transceiverType = 317;
			vN16XXlogHardwareInfo.ChannelInfos.Add(item3);
			return vN16XXlogHardwareInfo;
		}
	}
}
