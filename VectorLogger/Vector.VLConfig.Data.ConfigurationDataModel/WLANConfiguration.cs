using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "WLANConfiguration")]
	public class WLANConfiguration : Feature, IFeatureEvents
	{
		private IList<Event> events;

		[DataMember(Name = "WLANConfigurationThreeGDataTransferTriggerConfiguration")]
		private DataTransferTriggerConfiguration threeGDataTransferTriggerConfiguration;

		public override IFeatureReferencedFiles ReferencedFiles
		{
			get
			{
				return null;
			}
		}

		public override IFeatureSymbolicDefinitions SymbolicDefinitions
		{
			get
			{
				return null;
			}
		}

		public override IFeatureCcpXcpSignalDefinitions CcpXcpSignalDefinitions
		{
			get
			{
				return null;
			}
		}

		public override IFeatureTransmitMessages TransmitMessages
		{
			get
			{
				return null;
			}
		}

		public override IFeatureVirtualLogMessages VirtualLogMessages
		{
			get
			{
				return null;
			}
		}

		public override IFeatureEvents Events
		{
			get
			{
				return this;
			}
		}

		[DataMember(Name = "WLANConfigurationIsStartWLANor3GOnShutdownEnabled")]
		public ValidatedProperty<bool> IsStartWLANor3GOnShutdownEnabled
		{
			get;
			set;
		}

		[DataMember(Name = "WLANConfigurationAnalogInputNumber")]
		public ValidatedProperty<uint> AnalogInputNumber
		{
			get;
			set;
		}

		[DataMember(Name = "WLANConfigurationIsWLANor3GDownloadRingbufferEnabled")]
		public ValidatedProperty<bool> IsWLANor3GDownloadRingbufferEnabled
		{
			get;
			set;
		}

		[DataMember(Name = "WLANConfigurationWLANor3GRingbuffersToDownload")]
		public ValidatedProperty<uint> WLANor3GRingbuffersToDownload
		{
			get;
			set;
		}

		[DataMember(Name = "WLANConfigurationIsWLANor3GDownloadClassificationEnabled")]
		public ValidatedProperty<bool> IsWLANor3GDownloadClassificationEnabled
		{
			get;
			set;
		}

		[DataMember(Name = "WLANConfigurationIsWLANor3GDownloadDriveRecorderEnabled")]
		public ValidatedProperty<bool> IsWLANor3GDownloadDriveRecorderEnabled
		{
			get;
			set;
		}

		[DataMember(Name = "WLANConfigurationIsStartWLANOnEventEnabled")]
		public ValidatedProperty<bool> IsStartWLANOnEventEnabled
		{
			get;
			set;
		}

		[DataMember(Name = "WLANConfigurationStartWLANEvent")]
		public Event StartWLANEvent
		{
			get;
			set;
		}

		[DataMember(Name = "WLANConfigurationIsWLANDownloadRingbufferEnabled")]
		public ValidatedProperty<bool> IsWLANDownloadRingbufferEnabled
		{
			get;
			set;
		}

		[DataMember(Name = "WLANConfigurationWLANRingbuffersToDownload")]
		public ValidatedProperty<uint> WLANRingbuffersToDownload
		{
			get;
			set;
		}

		[DataMember(Name = "WLANConfigurationIsWLANDownloadClassificationEnabled")]
		public ValidatedProperty<bool> IsWLANDownloadClassificationEnabled
		{
			get;
			set;
		}

		[DataMember(Name = "WLANConfigurationIsWLANDownloadDriveRecorderEnabled")]
		public ValidatedProperty<bool> IsWLANDownloadDriveRecorderEnabled
		{
			get;
			set;
		}

		[DataMember(Name = "WLANConfigurationIsWLANFallbackTo3GEnabled")]
		public ValidatedProperty<bool> IsWLANFallbackTo3GEnabled
		{
			get;
			set;
		}

		[DataMember(Name = "WLANConfigurationIsStartThreeGOnEventEnabled")]
		public ValidatedProperty<bool> IsStartThreeGOnEventEnabled
		{
			get;
			set;
		}

		public DataTransferTriggerConfiguration ThreeGDataTransferTriggerConfiguration
		{
			get
			{
				return this.threeGDataTransferTriggerConfiguration;
			}
		}

		[DataMember(Name = "WLANConfigurationPartialDownload")]
		public ValidatedProperty<PartialDownloadType> PartialDownload
		{
			get;
			set;
		}

		[DataMember(Name = "WLANConfigurationLoggerIp")]
		public ValidatedProperty<string> LoggerIp
		{
			get;
			set;
		}

		[DataMember(Name = "WLANConfigurationGatewayIp")]
		public ValidatedProperty<string> GatewayIp
		{
			get;
			set;
		}

		[DataMember(Name = "WLANConfigurationMLserverIp")]
		public ValidatedProperty<string> MLserverIp
		{
			get;
			set;
		}

		[DataMember(Name = "WLANConfigurationSubnetMask")]
		public ValidatedProperty<string> SubnetMask
		{
			get;
			set;
		}

		public ReadOnlyCollection<Event> ActiveCasKeyEvents
		{
			get
			{
				List<Event> list = new List<Event>();
				foreach (Event current in this.Events.GetEvents(true))
				{
					if (current is KeyEvent && (current as KeyEvent).IsCasKey)
					{
						list.Add(current);
					}
				}
				return new ReadOnlyCollection<Event>(list);
			}
		}

		public override bool UpdateOnDependency(Feature otherFeature, IUpdateServiceForFeature updateService)
		{
			if (otherFeature is WLANConfiguration || otherFeature is TriggerConfiguration)
			{
				updateService.Notify<WLANConfiguration>(this);
				return true;
			}
			return false;
		}

		public override void Update(IUpdateServiceForFeature updateService)
		{
			updateService.Notify<WLANConfiguration>(this);
		}

		IList<Event> IFeatureEvents.GetEvents(bool activeEventsOnly)
		{
			if (this.events == null)
			{
				this.events = new List<Event>();
			}
			this.events.Clear();
			if (this.StartWLANEvent != null && (!activeEventsOnly || this.IsStartWLANOnEventEnabled.Value))
			{
				this.events.Add(this.StartWLANEvent);
			}
			if (!activeEventsOnly || this.IsStartThreeGOnEventEnabled.Value)
			{
				foreach (Event current in this.ThreeGDataTransferTriggerConfiguration.GetEvents(activeEventsOnly))
				{
					this.events.Add(current);
				}
			}
			return this.events;
		}

		public WLANConfiguration()
		{
			this.IsStartWLANor3GOnShutdownEnabled = new ValidatedProperty<bool>(false);
			this.AnalogInputNumber = new ValidatedProperty<uint>(1u);
			this.IsWLANor3GDownloadRingbufferEnabled = new ValidatedProperty<bool>(false);
			this.WLANor3GRingbuffersToDownload = new ValidatedProperty<uint>(2147483647u);
			this.IsWLANor3GDownloadClassificationEnabled = new ValidatedProperty<bool>(false);
			this.IsWLANor3GDownloadDriveRecorderEnabled = new ValidatedProperty<bool>(false);
			this.IsStartWLANOnEventEnabled = new ValidatedProperty<bool>(false);
			this.StartWLANEvent = null;
			this.IsWLANDownloadRingbufferEnabled = new ValidatedProperty<bool>(false);
			this.WLANRingbuffersToDownload = new ValidatedProperty<uint>(2147483647u);
			this.IsWLANDownloadClassificationEnabled = new ValidatedProperty<bool>(false);
			this.IsWLANDownloadDriveRecorderEnabled = new ValidatedProperty<bool>(false);
			this.IsWLANFallbackTo3GEnabled = new ValidatedProperty<bool>(false);
			this.IsStartThreeGOnEventEnabled = new ValidatedProperty<bool>(false);
			this.threeGDataTransferTriggerConfiguration = new DataTransferTriggerConfiguration();
			this.PartialDownload = new ValidatedProperty<PartialDownloadType>(PartialDownloadType.PartialDownloadOn);
			this.LoggerIp = new ValidatedProperty<string>(string.Empty);
			this.GatewayIp = new ValidatedProperty<string>(string.Empty);
			this.SubnetMask = new ValidatedProperty<string>(string.Empty);
			this.MLserverIp = new ValidatedProperty<string>(string.Empty);
		}
	}
}
