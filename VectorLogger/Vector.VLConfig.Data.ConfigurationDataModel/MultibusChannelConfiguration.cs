using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "MultibusChannelConfiguration")]
	public class MultibusChannelConfiguration : Feature
	{
		[DataMember(Name = "MultibusChannelConfigurationChannelList")]
		private List<HardwareChannel> channelList;

		public override IFeatureReferencedFiles ReferencedFiles
		{
			get
			{
				return null;
			}
		}

		public override IFeatureSymbolicDefinitions SymbolicDefinitions
		{
			get
			{
				return null;
			}
		}

		public override IFeatureCcpXcpSignalDefinitions CcpXcpSignalDefinitions
		{
			get
			{
				return null;
			}
		}

		public override IFeatureTransmitMessages TransmitMessages
		{
			get
			{
				return null;
			}
		}

		public override IFeatureVirtualLogMessages VirtualLogMessages
		{
			get
			{
				return null;
			}
		}

		public override IFeatureEvents Events
		{
			get
			{
				return null;
			}
		}

		public ReadOnlyCollection<HardwareChannel> Channels
		{
			get
			{
				return new ReadOnlyCollection<HardwareChannel>(this.channelList);
			}
		}

		public Dictionary<uint, CANChannel> CANChannels
		{
			get
			{
				Dictionary<uint, CANChannel> dictionary = new Dictionary<uint, CANChannel>();
				uint num = 1u;
				while ((ulong)num <= (ulong)((long)this.channelList.Count))
				{
					if (this.channelList[(int)(num - 1u)] is CANChannel)
					{
						dictionary.Add(num, this.channelList[(int)(num - 1u)] as CANChannel);
					}
					num += 1u;
				}
				return dictionary;
			}
		}

		public Dictionary<uint, CANChannel> CANStdChannels
		{
			get
			{
				Dictionary<uint, CANChannel> dictionary = new Dictionary<uint, CANChannel>();
				uint num = 1u;
				while ((ulong)num <= (ulong)((long)this.channelList.Count))
				{
					CANChannel cANChannel = this.channelList[(int)(num - 1u)] as CANChannel;
					if (cANChannel != null && !cANChannel.CANChipConfiguration.IsCANFD)
					{
						dictionary.Add(num, this.channelList[(int)(num - 1u)] as CANChannel);
					}
					num += 1u;
				}
				return dictionary;
			}
		}

		public Dictionary<uint, CANChannel> CANFDChannels
		{
			get
			{
				Dictionary<uint, CANChannel> dictionary = new Dictionary<uint, CANChannel>();
				uint num = 1u;
				while ((ulong)num <= (ulong)((long)this.channelList.Count))
				{
					CANChannel cANChannel = this.channelList[(int)(num - 1u)] as CANChannel;
					if (cANChannel != null && cANChannel.CANChipConfiguration.IsCANFD)
					{
						dictionary.Add(num, this.channelList[(int)(num - 1u)] as CANChannel);
					}
					num += 1u;
				}
				return dictionary;
			}
		}

		public Dictionary<uint, LINChannel> LINChannels
		{
			get
			{
				Dictionary<uint, LINChannel> dictionary = new Dictionary<uint, LINChannel>();
				uint num = 1u;
				while ((ulong)num <= (ulong)((long)this.channelList.Count))
				{
					if (this.channelList[(int)(num - 1u)] is LINChannel)
					{
						dictionary.Add(num, this.channelList[(int)(num - 1u)] as LINChannel);
					}
					num += 1u;
				}
				return dictionary;
			}
		}

		public Dictionary<uint, J1708Channel> J1708Channels
		{
			get
			{
				Dictionary<uint, J1708Channel> dictionary = new Dictionary<uint, J1708Channel>();
				uint num = 1u;
				while ((ulong)num <= (ulong)((long)this.channelList.Count))
				{
					if (this.channelList[(int)(num - 1u)] is J1708Channel)
					{
						dictionary.Add(num, this.channelList[(int)(num - 1u)] as J1708Channel);
					}
					num += 1u;
				}
				return dictionary;
			}
		}

		public uint NumberOfChannels
		{
			get
			{
				return (uint)this.channelList.Count;
			}
			set
			{
				if ((long)this.channelList.Count < (long)((ulong)value))
				{
					int num = (int)(value - (uint)this.channelList.Count);
					for (int i = 0; i < num; i++)
					{
						this.channelList.Add(new CANChannel());
					}
					return;
				}
				if (value < (uint)this.channelList.Count)
				{
					while (value < (uint)this.channelList.Count)
					{
						this.channelList.RemoveAt(this.channelList.Count - 1);
					}
				}
			}
		}

		public override bool UpdateOnDependency(Feature otherFeature, IUpdateServiceForFeature updateService)
		{
			if (otherFeature is MultibusChannelConfiguration)
			{
				updateService.Notify<MultibusChannelConfiguration>(this);
				return true;
			}
			return false;
		}

		public override void Update(IUpdateServiceForFeature updateService)
		{
			updateService.Notify<MultibusChannelConfiguration>(this);
		}

		public MultibusChannelConfiguration()
		{
			this.channelList = new List<HardwareChannel>();
		}

		public BusType GetChannelBusType(uint channelNr)
		{
			HardwareChannel channel = this.GetChannel(channelNr);
			if (channel is CANChannel)
			{
				return BusType.Bt_CAN;
			}
			if (channel is LINChannel)
			{
				return BusType.Bt_LIN;
			}
			if (channel is J1708Channel)
			{
				return BusType.Bt_J1708;
			}
			Trace.Assert(false);
			return BusType.Bt_None;
		}

		public bool SetChannelBusType(uint channelNr, BusType type)
		{
			if ((ulong)channelNr > (ulong)((long)this.channelList.Count) || channelNr < 1u)
			{
				return false;
			}
			switch (type)
			{
			case BusType.Bt_CAN:
			{
				HardwareChannel item = new CANChannel();
				goto IL_60;
			}
			case BusType.Bt_LIN:
			{
				HardwareChannel item = new LINChannel();
				goto IL_60;
			}
			case BusType.Bt_J1708:
			{
				HardwareChannel item = new J1708Channel();
				goto IL_60;
			}
			}
			return false;
			IL_60:
			bool flag = (ulong)channelNr == (ulong)((long)this.channelList.Count);
			this.channelList.RemoveAt((int)(channelNr - 1u));
			if (flag)
			{
				HardwareChannel item;
				this.channelList.Add(item);
			}
			else
			{
				HardwareChannel item;
				this.channelList.Insert((int)(channelNr - 1u), item);
			}
			return true;
		}

		public HardwareChannel GetChannel(uint channelNr)
		{
			if ((ulong)channelNr <= (ulong)((long)this.channelList.Count))
			{
				return this.channelList[(int)(channelNr - 1u)];
			}
			return null;
		}
	}
}
