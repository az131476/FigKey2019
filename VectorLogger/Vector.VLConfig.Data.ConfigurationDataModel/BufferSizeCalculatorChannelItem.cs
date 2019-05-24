using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "BufferSizeCalculatorChannelItem")]
	public class BufferSizeCalculatorChannelItem
	{
		[DataMember(Name = "BufferSizeCalculatorChannelItemName")]
		private ValidatedProperty<string> mName;

		[DataMember(Name = "BufferSizeCalculatorChannelItemBusLoad")]
		private ValidatedProperty<uint> mBusLoad;

		[DataMember(Name = "BufferSizeCalculatorChannelItemMsgsPerSec")]
		private ValidatedProperty<uint> mMsgsPerSec;

		[DataMember(Name = "BufferSizeCalculatorChannelItemPayload")]
		private ValidatedProperty<uint> mPayload;

		public ValidatedProperty<string> Name
		{
			get
			{
				ValidatedProperty<string> arg_1B_0;
				if ((arg_1B_0 = this.mName) == null)
				{
					arg_1B_0 = (this.mName = new ValidatedProperty<string>());
				}
				return arg_1B_0;
			}
			set
			{
				this.mName = value;
			}
		}

		public ValidatedProperty<uint> BusLoad
		{
			get
			{
				ValidatedProperty<uint> arg_1B_0;
				if ((arg_1B_0 = this.mBusLoad) == null)
				{
					arg_1B_0 = (this.mBusLoad = new ValidatedProperty<uint>());
				}
				return arg_1B_0;
			}
			set
			{
				this.mBusLoad = value;
			}
		}

		public ValidatedProperty<uint> MsgsPerSec
		{
			get
			{
				ValidatedProperty<uint> arg_1B_0;
				if ((arg_1B_0 = this.mMsgsPerSec) == null)
				{
					arg_1B_0 = (this.mMsgsPerSec = new ValidatedProperty<uint>());
				}
				return arg_1B_0;
			}
			set
			{
				this.mMsgsPerSec = value;
			}
		}

		public ValidatedProperty<uint> Payload
		{
			get
			{
				ValidatedProperty<uint> arg_1B_0;
				if ((arg_1B_0 = this.mPayload) == null)
				{
					arg_1B_0 = (this.mPayload = new ValidatedProperty<uint>());
				}
				return arg_1B_0;
			}
			set
			{
				this.mPayload = value;
			}
		}

		public static string CreateName(HardwareChannel hardwareChannel, uint channelNumber)
		{
			if (hardwareChannel == null)
			{
				return "<unknown bustype> " + channelNumber;
			}
			if (hardwareChannel is CANChannel)
			{
				return "CAN " + channelNumber;
			}
			if (hardwareChannel is LINChannel)
			{
				return "LIN " + channelNumber;
			}
			if (hardwareChannel is FlexrayChannel)
			{
				return "FlexRay " + channelNumber;
			}
			return "<unknown bustype> " + channelNumber;
		}
	}
}
