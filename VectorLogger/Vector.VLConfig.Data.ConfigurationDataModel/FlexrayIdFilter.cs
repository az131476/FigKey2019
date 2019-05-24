using System;
using System.Runtime.Serialization;
using Vector.VLConfig.LoggerSpecifics;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "FlexrayIdFilter")]
	public class FlexrayIdFilter : Filter
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

		[DataMember(Name = "FilterFlexrayId")]
		public ValidatedProperty<uint> FlexrayId
		{
			get;
			set;
		}

		[DataMember(Name = "FilterFlexrayIdLast")]
		public ValidatedProperty<uint> FlexrayIdLast
		{
			get;
			set;
		}

		[DataMember(Name = "FilterBaseCycle")]
		public ValidatedProperty<uint> BaseCycle
		{
			get;
			set;
		}

		[DataMember(Name = "FilterCycleRepetition")]
		public ValidatedProperty<uint> CycleRepetition
		{
			get;
			set;
		}

		public FlexrayIdFilter()
		{
			this.ChannelNumber = new ValidatedProperty<uint>(1u);
			this.IsIdRange = new ValidatedProperty<bool>(false);
			this.FlexrayId = new ValidatedProperty<uint>(Constants.MinimumFlexraySlotId);
			this.FlexrayIdLast = new ValidatedProperty<uint>(Constants.MinimumFlexraySlotId);
			this.BaseCycle = new ValidatedProperty<uint>(0u);
			this.CycleRepetition = new ValidatedProperty<uint>(1u);
		}
	}
}
