using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "SignalListFileFilter")]
	public class SignalListFileFilter : Filter
	{
		[DataMember(Name = "FilterChannelNumber")]
		public ValidatedProperty<uint> ChannelNumber
		{
			get;
			set;
		}

		[DataMember(Name = "BusType")]
		public ValidatedProperty<BusType> BusType
		{
			get;
			set;
		}

		[DataMember(Name = "FilePath")]
		public ValidatedProperty<string> FilePath
		{
			get;
			set;
		}

		[DataMember(Name = "Column")]
		public ValidatedProperty<uint> Column
		{
			get;
			set;
		}

		public SignalListFileFilter(BusType busType)
		{
			this.ChannelNumber = new ValidatedProperty<uint>(1u);
			this.FilePath = new ValidatedProperty<string>(string.Empty);
			this.Column = new ValidatedProperty<uint>(0u);
			this.BusType = new ValidatedProperty<BusType>(busType);
		}
	}
}
