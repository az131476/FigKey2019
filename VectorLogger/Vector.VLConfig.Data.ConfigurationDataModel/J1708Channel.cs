using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "J1708Channel")]
	public class J1708Channel : HardwareChannel
	{
		[DataMember(Name = "J1708ChannelMaxInterCharBitTime")]
		public ValidatedProperty<uint> MaxInterCharBitTime
		{
			get;
			set;
		}

		[DataMember(Name = "J1708ChannelBaudrate")]
		public ValidatedProperty<uint> SpeedRate
		{
			get;
			set;
		}

		public J1708Channel()
		{
			this.MaxInterCharBitTime = new ValidatedProperty<uint>(2u);
			this.SpeedRate = new ValidatedProperty<uint>(0u);
		}
	}
}
