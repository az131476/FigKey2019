using System;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.GUI.TriggersPage
{
	internal class ChannelItem
	{
		private uint mBusLoad;

		private uint mMsgsPerSec;

		private uint mPayload;

		private HardwareChannel mHardwareChannel;

		private uint mSpeedRate;

		private uint mMaxMsgsPerSec;

		public string Name
		{
			get;
			private set;
		}

		public uint BusLoad
		{
			get
			{
				return this.mBusLoad;
			}
			set
			{
				if (value > 100u)
				{
					value = 100u;
				}
				if (value == this.mBusLoad)
				{
					return;
				}
				this.mBusLoad = value;
				this.UpdateInfo();
			}
		}

		public uint MsgsPerSec
		{
			get
			{
				return this.mMsgsPerSec;
			}
			set
			{
				if (value == this.mMsgsPerSec)
				{
					return;
				}
				this.mMsgsPerSec = value;
				this.UpdateInfo();
			}
		}

		public uint Payload
		{
			get
			{
				return this.mPayload;
			}
			set
			{
				if (value > 255u)
				{
					value = 255u;
				}
				if (value == this.mPayload)
				{
					return;
				}
				this.mPayload = value;
				this.UpdateInfo();
			}
		}

		public uint BytesPerSec
		{
			get;
			private set;
		}

		public string Info
		{
			get;
			private set;
		}

		private CANChannel CANChannel
		{
			get
			{
				return this.mHardwareChannel as CANChannel;
			}
		}

		private bool IsCANFD
		{
			get
			{
				return this.CANChannel != null && this.CANChannel.CANChipConfiguration.IsCANFD;
			}
		}

		private LINChannel LINChannel
		{
			get
			{
				return this.mHardwareChannel as LINChannel;
			}
		}

		private FlexrayChannel FlexrayChannel
		{
			get
			{
				return this.mHardwareChannel as FlexrayChannel;
			}
		}

		public bool IsActive
		{
			get
			{
				return this.mHardwareChannel != null && this.mHardwareChannel.IsActive.Value;
			}
		}

		public BusType BusType
		{
			get;
			set;
		}

		public ChannelItem(BufferSizeCalculatorChannelItem metaDataItem)
		{
			this.Name = metaDataItem.Name.Value;
			this.mBusLoad = metaDataItem.BusLoad.Value;
			this.mMsgsPerSec = metaDataItem.MsgsPerSec.Value;
			this.mPayload = metaDataItem.Payload.Value;
			this.mHardwareChannel = null;
			this.BusType = BusType.Bt_None;
		}

		private void Init(bool reset)
		{
			if (reset)
			{
				this.BusType = BusType.Bt_None;
			}
			if (this.CANChannel != null)
			{
				this.BusType = BusType.Bt_CAN;
				if (this.CANChannel.CANChipConfiguration is CANFDChipConfiguration)
				{
					this.SetSpeedRate((this.CANChannel.CANChipConfiguration as CANFDChipConfiguration).DataBaudrate);
				}
				else
				{
					this.SetSpeedRate(this.CANChannel.CANChipConfiguration.Baudrate);
				}
				if (reset)
				{
					this.BusLoad = 50u;
				}
			}
			else if (this.LINChannel != null)
			{
				this.BusType = BusType.Bt_LIN;
				this.SetSpeedRate(this.LINChannel.SpeedRate.Value);
				if (reset)
				{
					this.BusLoad = 50u;
				}
			}
			else if (this.FlexrayChannel != null)
			{
				this.BusType = BusType.Bt_FlexRay;
				this.SetSpeedRate(0u);
				if (reset)
				{
					this.MsgsPerSec = 1000u;
					this.Payload = 42u;
				}
			}
			this.UpdateInfo();
		}

		public void Update(HardwareChannel hardwareChannel, bool reset)
		{
			this.mHardwareChannel = hardwareChannel;
			this.Init(reset);
		}

		private void SetSpeedRate(uint speedRate = 0u)
		{
			if (speedRate != 0u)
			{
				this.mSpeedRate = speedRate;
				return;
			}
			switch (this.BusType)
			{
			case BusType.Bt_CAN:
				if (this.IsCANFD)
				{
					this.mSpeedRate = (uint)BufferSizeCalculator.Constants.CanFdDefaultSpeedRate;
					return;
				}
				this.mSpeedRate = 500000u;
				return;
			case BusType.Bt_LIN:
				this.mSpeedRate = 19200u;
				return;
			case BusType.Bt_FlexRay:
				this.mSpeedRate = 10000000u;
				return;
			default:
				this.mSpeedRate = 0u;
				return;
			}
		}

		private void CalcDataRate()
		{
			switch (this.BusType)
			{
			case BusType.Bt_CAN:
				if (this.IsCANFD)
				{
					this.mMaxMsgsPerSec = (uint)(this.mSpeedRate / BufferSizeCalculator.Constants.CanFdDefaultSpeedRate * BufferSizeCalculator.Constants.CanFdMsgsPerSecAtDefaultSpeedRate);
					this.MsgsPerSec = this.mMaxMsgsPerSec * this.BusLoad / 100u;
					this.BytesPerSec = this.MsgsPerSec * BufferSizeCalculator.Constants.CanFdBytesPerMessage;
					return;
				}
				this.mMaxMsgsPerSec = (uint)(this.mSpeedRate / 500000.0 * 4000.0);
				this.MsgsPerSec = this.mMaxMsgsPerSec * this.BusLoad / 100u;
				this.BytesPerSec = this.MsgsPerSec * 20u;
				return;
			case BusType.Bt_LIN:
				this.mMaxMsgsPerSec = (uint)(this.mSpeedRate / 19200.0 * 200.0);
				this.MsgsPerSec = this.mMaxMsgsPerSec * this.BusLoad / 100u;
				this.BytesPerSec = this.MsgsPerSec * 20u;
				return;
			case BusType.Bt_FlexRay:
				this.mMaxMsgsPerSec = this.mSpeedRate / (12u + this.Payload);
				this.BytesPerSec = (this.Payload + 12u) * this.MsgsPerSec;
				return;
			default:
				return;
			}
		}

		private void UpdateInfo()
		{
			this.CalcDataRate();
			this.Info = string.Empty;
			switch (this.BusType)
			{
			case BusType.Bt_CAN:
			case BusType.Bt_LIN:
			{
				if (this.IsCANFD)
				{
					this.Info += "FD data: ";
				}
				object info = this.Info;
				this.Info = string.Concat(new object[]
				{
					info,
					this.BaudRateString(this.mSpeedRate),
					" => ",
					this.MsgsPerSec,
					" msgs/s, ",
					BufferSizeCalculator.DataRate(this.BytesPerSec)
				});
				return;
			}
			case BusType.Bt_FlexRay:
				this.Info = this.Info + this.BaudRateString(this.mSpeedRate) + " => " + BufferSizeCalculator.DataRate(this.BytesPerSec);
				return;
			default:
				return;
			}
		}

		private string BaudRateString(uint bitPerSec)
		{
			if (bitPerSec >= 1000000u)
			{
				return string.Format("{0} MBit/s", bitPerSec / 1000000u);
			}
			if (bitPerSec >= 100000u)
			{
				return string.Format("{0} kBaud", bitPerSec / 1000u);
			}
			return string.Format("{0} Baud", bitPerSec);
		}
	}
}
