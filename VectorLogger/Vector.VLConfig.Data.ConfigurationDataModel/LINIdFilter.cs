using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "LINIdFilter")]
	public class LINIdFilter : Filter
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

		[DataMember(Name = "FilterLINId")]
		public ValidatedProperty<uint> LINId
		{
			get;
			set;
		}

		[DataMember(Name = "FilterLINIdLast")]
		public ValidatedProperty<uint> LINIdLast
		{
			get;
			set;
		}

		public LINIdFilter()
		{
			this.ChannelNumber = new ValidatedProperty<uint>(1u);
			this.IsIdRange = new ValidatedProperty<bool>(false);
			this.LINId = new ValidatedProperty<uint>(0u);
			this.LINIdLast = new ValidatedProperty<uint>(0u);
		}
	}
}
