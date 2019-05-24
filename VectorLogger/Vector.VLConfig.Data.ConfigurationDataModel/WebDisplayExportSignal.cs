using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "WebDisplayExportSignal")]
	public class WebDisplayExportSignal : ISymbolicSignal
	{
		[DataMember(Name = "IsActive")]
		public ValidatedProperty<bool> IsActive
		{
			get;
			set;
		}

		[DataMember(Name = "WebDisplayExportSignalName")]
		public ValidatedProperty<string> Name
		{
			get;
			set;
		}

		[DataMember(Name = "WebDisplayExportSignalComment")]
		public ValidatedProperty<string> Comment
		{
			get;
			set;
		}

		[DataMember(Name = "WebDisplayExportSignalBusType")]
		public ValidatedProperty<BusType> BusType
		{
			get;
			set;
		}

		[DataMember(Name = "WebDisplayExportSignalLtlSystemInformation")]
		public ValidatedProperty<LtlSystemInformation> LtlSystemInformation
		{
			get;
			set;
		}

		[DataMember(Name = "WebDisplayExportSignalChannelNumber")]
		public ValidatedProperty<uint> ChannelNumber
		{
			get;
			set;
		}

		[DataMember(Name = "WebDisplayExportSignalNetworkName")]
		public ValidatedProperty<string> NetworkName
		{
			get;
			set;
		}

		[DataMember(Name = "WebDisplayExportSignalMessageName")]
		public ValidatedProperty<string> MessageName
		{
			get;
			set;
		}

		[DataMember(Name = "WebDisplayExportSignalSignalName")]
		public ValidatedProperty<string> SignalName
		{
			get;
			set;
		}

		[DataMember(Name = "WebDisplayExportSignalDatabaseName")]
		public ValidatedProperty<string> DatabaseName
		{
			get;
			set;
		}

		[DataMember(Name = "WebDisplayExportSignalDatabasePath")]
		public ValidatedProperty<string> DatabasePath
		{
			get;
			set;
		}

		[DataMember(Name = "WebDisplayExportSignalIsFlexrayPDU")]
		public ValidatedProperty<bool> IsFlexrayPDU
		{
			get;
			set;
		}

		public WebDisplayExportSignalType Type
		{
			get
			{
				if (this.LtlSystemInformation.Value != Vector.VLConfig.Data.ConfigurationDataModel.LtlSystemInformation.None)
				{
					return WebDisplayExportSignalType.LtlSystemInformation;
				}
				if (this.BusType.Value != Vector.VLConfig.Data.ConfigurationDataModel.BusType.Bt_None)
				{
					return WebDisplayExportSignalType.Signal;
				}
				return WebDisplayExportSignalType.Variable;
			}
		}

		public WebDisplayExportSignal(string name, string signalName, string messageName, string networkName, string databaseName, string databasePath, BusType busType, uint channelNumber, bool isFlexrayPDU)
		{
			this.IsActive = new ValidatedProperty<bool>(true);
			this.Name = new ValidatedProperty<string>(name);
			this.Comment = new ValidatedProperty<string>(signalName);
			this.SignalName = new ValidatedProperty<string>(signalName);
			this.MessageName = new ValidatedProperty<string>(messageName);
			this.DatabaseName = new ValidatedProperty<string>(databaseName);
			this.BusType = new ValidatedProperty<BusType>(busType);
			this.LtlSystemInformation = new ValidatedProperty<LtlSystemInformation>(Vector.VLConfig.Data.ConfigurationDataModel.LtlSystemInformation.None);
			this.ChannelNumber = new ValidatedProperty<uint>(channelNumber);
			this.NetworkName = new ValidatedProperty<string>(networkName);
			this.DatabasePath = new ValidatedProperty<string>(databasePath);
			this.IsFlexrayPDU = new ValidatedProperty<bool>(isFlexrayPDU);
		}

		public WebDisplayExportSignal(string name, string comment, LtlSystemInformation ltlSystemInformation)
		{
			this.IsActive = new ValidatedProperty<bool>(true);
			this.Name = new ValidatedProperty<string>(name);
			this.Comment = new ValidatedProperty<string>(comment);
			this.SignalName = new ValidatedProperty<string>(string.Empty);
			this.MessageName = new ValidatedProperty<string>(string.Empty);
			this.DatabaseName = new ValidatedProperty<string>(string.Empty);
			this.BusType = new ValidatedProperty<BusType>(Vector.VLConfig.Data.ConfigurationDataModel.BusType.Bt_None);
			this.LtlSystemInformation = new ValidatedProperty<LtlSystemInformation>(ltlSystemInformation);
			this.ChannelNumber = new ValidatedProperty<uint>(0u);
			this.NetworkName = new ValidatedProperty<string>(string.Empty);
			this.DatabasePath = new ValidatedProperty<string>(string.Empty);
			this.IsFlexrayPDU = new ValidatedProperty<bool>(false);
		}
	}
}
