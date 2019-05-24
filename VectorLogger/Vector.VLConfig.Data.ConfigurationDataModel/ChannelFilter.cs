using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "ChannelFilter")]
	public class ChannelFilter : Filter
	{
		[DataMember(Name = "ChannelFilterBusType")]
		public ValidatedProperty<BusType> BusType
		{
			get;
			set;
		}

		[DataMember(Name = "ChannelFilterChannelNumber")]
		public ValidatedProperty<uint> ChannelNumber
		{
			get;
			set;
		}

		public ChannelFilter()
		{
			this.BusType = new ValidatedProperty<BusType>(Vector.VLConfig.Data.ConfigurationDataModel.BusType.Bt_CAN);
			this.ChannelNumber = new ValidatedProperty<uint>(1u);
		}
	}
}
