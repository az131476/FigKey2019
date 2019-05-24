using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "EthernetConfiguration")]
	public class EthernetConfiguration : Feature
	{
		public const string kDefaultEth1Ip = "192.168.165.10";

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
				return null;
			}
		}

		[DataMember(Name = "EthernetConfigurationEth1Ip")]
		public ValidatedProperty<string> Eth1Ip
		{
			get;
			set;
		}

		[DataMember(Name = "EthernetConfigurationEth1KeepAwake")]
		public ValidatedProperty<bool> Eth1KeepAwake
		{
			get;
			set;
		}

		public override bool UpdateOnDependency(Feature otherFeature, IUpdateServiceForFeature updateService)
		{
			if (otherFeature is EthernetConfiguration)
			{
				updateService.Notify<EthernetConfiguration>(this);
				return true;
			}
			return false;
		}

		public override void Update(IUpdateServiceForFeature updateService)
		{
			updateService.Notify<EthernetConfiguration>(this);
		}

		public EthernetConfiguration()
		{
			this.Eth1Ip = new ValidatedProperty<string>("192.168.165.10");
			this.Eth1KeepAwake = new ValidatedProperty<bool>(false);
		}

		public EthernetConfiguration(EthernetConfiguration other)
		{
			this.Eth1Ip = new ValidatedProperty<string>(other.Eth1Ip.Value);
			this.Eth1KeepAwake = new ValidatedProperty<bool>(other.Eth1KeepAwake.Value);
		}
	}
}
