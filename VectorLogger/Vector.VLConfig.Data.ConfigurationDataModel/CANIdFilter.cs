using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "CANIdFilter")]
	public class CANIdFilter : Filter
	{
		[DataMember(Name = "FilterChannelNumber")]
		public ValidatedProperty<uint> ChannelNumber
		{
			get;
			set;
		}

		[DataMember(Name = "FilterIsIdRange")]
		public ValidatedProperty<bool> IsIdRange
		{
			get;
			set;
		}

		[DataMember(Name = "FilterIsExtendedId")]
		public ValidatedProperty<bool> IsExtendedId
		{
			get;
			set;
		}

		[DataMember(Name = "FilterCANId")]
		public ValidatedProperty<uint> CANId
		{
			get;
			set;
		}

		[DataMember(Name = "FilterCANIdLast")]
		public ValidatedProperty<uint> CANIdLast
		{
			get;
			set;
		}

		public CANIdFilter()
		{
			this.ChannelNumber = new ValidatedProperty<uint>(1u);
			this.IsIdRange = new ValidatedProperty<bool>(false);
			this.IsExtendedId = new ValidatedProperty<bool>(false);
			this.CANId = new ValidatedProperty<uint>(0u);
			this.CANIdLast = new ValidatedProperty<uint>(0u);
		}
	}
}
