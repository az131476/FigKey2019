using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "MOST150ChannelConfiguration")]
	public class MOST150ChannelConfiguration : Feature
	{
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

		[DataMember(Name = "MOST150ChannelConfigIsChannelEnabled")]
		public ValidatedProperty<bool> IsChannelEnabled
		{
			get;
			set;
		}

		[DataMember(Name = "MOST150ChannelConfigIsLogStatusEventsEnabled")]
		public ValidatedProperty<bool> IsLogStatusEventsEnabled
		{
			get;
			set;
		}

		[DataMember(Name = "MOST150ChannelConfigIsLogControlMsgsEnabled")]
		public ValidatedProperty<bool> IsLogControlMsgsEnabled
		{
			get;
			set;
		}

		[DataMember(Name = "MOST150ChannelConfigIsLogAsyncMDPEnabled")]
		public ValidatedProperty<bool> IsLogAsyncMDPEnabled
		{
			get;
			set;
		}

		[DataMember(Name = "MOST150ChannelConfigIsLogAsyncMEPEnabled")]
		public ValidatedProperty<bool> IsLogAsyncMEPEnabled
		{
			get;
			set;
		}

		[DataMember(Name = "MOST150ChannelConfigIsKeepAwakeEnabled")]
		public ValidatedProperty<bool> IsKeepAwakeEnabled
		{
			get;
			set;
		}

		[DataMember(Name = "MOST150ChannelConfigIsAutoStatusEventRepEnabled")]
		public ValidatedProperty<bool> IsAutoStatusEventRepEnabled
		{
			get;
			set;
		}

		public override bool UpdateOnDependency(Feature otherFeature, IUpdateServiceForFeature updateService)
		{
			if (otherFeature is MOST150ChannelConfiguration)
			{
				updateService.Notify<MOST150ChannelConfiguration>(this);
				return true;
			}
			return false;
		}

		public override void Update(IUpdateServiceForFeature updateService)
		{
			updateService.Notify<MOST150ChannelConfiguration>(this);
		}

		public MOST150ChannelConfiguration()
		{
			this.IsChannelEnabled = new ValidatedProperty<bool>(false);
			this.IsLogStatusEventsEnabled = new ValidatedProperty<bool>(true);
			this.IsLogControlMsgsEnabled = new ValidatedProperty<bool>(true);
			this.IsLogAsyncMDPEnabled = new ValidatedProperty<bool>(true);
			this.IsLogAsyncMEPEnabled = new ValidatedProperty<bool>(true);
			this.IsKeepAwakeEnabled = new ValidatedProperty<bool>(true);
			this.IsAutoStatusEventRepEnabled = new ValidatedProperty<bool>(true);
		}
	}
}
