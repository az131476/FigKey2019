using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "SymbolicMessageFilter")]
	public class SymbolicMessageFilter : Filter, ISymbolicMessage
	{
		[DataMember(Name = "FilterBusType")]
		public ValidatedProperty<BusType> BusType
		{
			get;
			set;
		}

		[DataMember(Name = "FilterChannelNumber")]
		public ValidatedProperty<uint> ChannelNumber
		{
			get;
			set;
		}

		[DataMember(Name = "FilterDatabaseName")]
		public ValidatedProperty<string> DatabaseName
		{
			get;
			set;
		}

		[DataMember(Name = "FilterDatabasePath")]
		public ValidatedProperty<string> DatabasePath
		{
			get;
			set;
		}

		[DataMember(Name = "FilterNetworkName")]
		public ValidatedProperty<string> NetworkName
		{
			get;
			set;
		}

		[DataMember(Name = "FilterMessageName")]
		public ValidatedProperty<string> MessageName
		{
			get;
			set;
		}

		[DataMember(Name = "FilterMessageIsFlexrayPDU")]
		public ValidatedProperty<bool> IsFlexrayPDU
		{
			get;
			set;
		}

		public bool IsFibexVersionGreaterThan2
		{
			get;
			set;
		}

		public SymbolicMessageFilter()
		{
			this.BusType = new ValidatedProperty<BusType>(Vector.VLConfig.Data.ConfigurationDataModel.BusType.Bt_Wildcard);
			this.ChannelNumber = new ValidatedProperty<uint>(1u);
			this.DatabaseName = new ValidatedProperty<string>(string.Empty);
			this.DatabasePath = new ValidatedProperty<string>(string.Empty);
			this.NetworkName = new ValidatedProperty<string>(string.Empty);
			this.MessageName = new ValidatedProperty<string>(string.Empty);
			this.IsFlexrayPDU = new ValidatedProperty<bool>(false);
			this.IsFibexVersionGreaterThan2 = false;
		}
	}
}
