using System;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.CaplGeneration.DataPresenter
{
	internal class DpHardwareChannelCan : DpHardwareChannel
	{
		public CANChannel CanChannel
		{
			get;
			private set;
		}

		private ICANChipConfiguration CANChipConfiguration
		{
			get
			{
				return this.CanChannel.CANChipConfiguration;
			}
		}

		private CANFDChipConfiguration CANFDChipConfiguration
		{
			get
			{
				return this.CanChannel.CANChipConfiguration as CANFDChipConfiguration;
			}
		}

		public bool IsCanFD
		{
			get
			{
				return this.CanChannel.CANChipConfiguration.IsCANFD;
			}
		}

		public uint Baudrate
		{
			get
			{
				return this.CANChipConfiguration.Baudrate;
			}
		}

		public uint Tseg1
		{
			get
			{
				return this.CANChipConfiguration.TSeg1.Value;
			}
		}

		public uint Tseg2
		{
			get
			{
				return this.CANChipConfiguration.TSeg2.Value;
			}
		}

		public uint Sjw
		{
			get
			{
				return this.CANChipConfiguration.SJW.Value;
			}
		}

		public uint Sam
		{
			get
			{
				return this.CANChipConfiguration.SAM.Value;
			}
		}

		public uint Flags
		{
			get
			{
				if (!this.CanChannel.IsOutputActive.Value)
				{
					return 1u;
				}
				return 0u;
			}
		}

		public uint DataBaudrate
		{
			get
			{
				if (!this.IsCanFD)
				{
					return this.Baudrate;
				}
				return this.CANFDChipConfiguration.DataBaudrate;
			}
		}

		public uint DataTseg1
		{
			get
			{
				if (!this.IsCanFD)
				{
					return this.Tseg1;
				}
				return this.CANFDChipConfiguration.DataTSeg1.Value;
			}
		}

		public uint DataTseg2
		{
			get
			{
				if (!this.IsCanFD)
				{
					return this.Tseg2;
				}
				return this.CANFDChipConfiguration.DataTSeg2.Value;
			}
		}

		public uint DataSjw
		{
			get
			{
				if (!this.IsCanFD)
				{
					return this.Sjw;
				}
				return this.CANFDChipConfiguration.DataSJW.Value;
			}
		}

		public uint DataSam
		{
			get
			{
				return this.Sam;
			}
		}

		public uint DataFlags
		{
			get
			{
				return this.Flags;
			}
		}

		public override string NameOfCaplEventHandler
		{
			get
			{
				return CaplHelper.MakeCanMsgHandlerString(this.ChannelString);
			}
		}

		public override string ForwardEventFunctionCall
		{
			get
			{
				return "logWriteEvent(this)";
			}
		}

		public DpHardwareChannelCan(CANChannel channel, uint channelNumber) : base(channelNumber)
		{
			this.CanChannel = channel;
		}
	}
}
