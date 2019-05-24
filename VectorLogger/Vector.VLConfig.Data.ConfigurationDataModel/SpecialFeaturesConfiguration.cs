using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "SpecialFeaturesConfiguration")]
	public class SpecialFeaturesConfiguration : Feature, IFeatureVirtualLogMessages, IVirtualCANLogMessage
	{
		private List<IVirtualCANLogMessage> activeVirtCANMsgs;

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
				return this;
			}
		}

		public override IFeatureEvents Events
		{
			get
			{
				return null;
			}
		}

		IList<IVirtualCANLogMessage> IFeatureVirtualLogMessages.ActiveVirtualCANLogMessages
		{
			get
			{
				if (this.activeVirtCANMsgs == null)
				{
					this.activeVirtCANMsgs = new List<IVirtualCANLogMessage>();
				}
				this.activeVirtCANMsgs.Clear();
				if (this.IsLogDateTimeEnabled.Value)
				{
					this.activeVirtCANMsgs.Add(this);
				}
				return this.activeVirtCANMsgs;
			}
		}

		ValidatedProperty<uint> IVirtualCANLogMessage.Id
		{
			get
			{
				return this.LogDateTimeCANId;
			}
		}

		ValidatedProperty<bool> IVirtualCANLogMessage.IsIdExtended
		{
			get
			{
				return this.LogDateTimeIsExtendedId;
			}
		}

		ValidatedProperty<uint> IVirtualCANLogMessage.ChannelNr
		{
			get
			{
				return this.LogDateTimeChannel;
			}
		}

		[DataMember(Name = "SpecialFeaturesIsLogDateTimeEnabled")]
		public ValidatedProperty<bool> IsLogDateTimeEnabled
		{
			get;
			set;
		}

		[DataMember(Name = "SpecialFeaturesLogDateTimeChannel")]
		public ValidatedProperty<uint> LogDateTimeChannel
		{
			get;
			set;
		}

		[DataMember(Name = "SpecialFeaturesLogDateTimeIsExtendedId")]
		public ValidatedProperty<bool> LogDateTimeIsExtendedId
		{
			get;
			set;
		}

		[DataMember(Name = "SpecialFeaturesLogDateTimeCANId")]
		public ValidatedProperty<uint> LogDateTimeCANId
		{
			get;
			set;
		}

		[DataMember(Name = "SpecialFeaturesIsIncludeLTLCodeEnabled")]
		public ValidatedProperty<bool> IsIncludeLTLCodeEnabled
		{
			get;
			set;
		}

		[DataMember(Name = "SpecialFeaturesIsIgnitionKeepsLoggerAwakeEnabled")]
		public ValidatedProperty<bool> IsIgnitionKeepsLoggerAwakeEnabled
		{
			get;
			set;
		}

		[DataMember(Name = "SpecialFeaturesIsOverloadBuzzerActive")]
		public ValidatedProperty<bool> IsOverloadBuzzerActive
		{
			get;
			set;
		}

		[DataMember(Name = "SpecialFeaturesMaxLogFileSize")]
		public ValidatedProperty<uint> MaxLogFileSize
		{
			get;
			set;
		}

		public override bool UpdateOnDependency(Feature otherFeature, IUpdateServiceForFeature updateService)
		{
			if (otherFeature is SpecialFeaturesConfiguration || otherFeature is CANChannelConfiguration)
			{
				updateService.Notify<SpecialFeaturesConfiguration>(this);
				return true;
			}
			return false;
		}

		public override void Update(IUpdateServiceForFeature updateService)
		{
			updateService.Notify<SpecialFeaturesConfiguration>(this);
		}

		public SpecialFeaturesConfiguration()
		{
			this.IsLogDateTimeEnabled = new ValidatedProperty<bool>(false);
			this.LogDateTimeChannel = new ValidatedProperty<uint>(1u);
			this.LogDateTimeCANId = new ValidatedProperty<uint>(0u);
			this.LogDateTimeIsExtendedId = new ValidatedProperty<bool>(false);
			this.IsIncludeLTLCodeEnabled = new ValidatedProperty<bool>(true);
			this.IsIgnitionKeepsLoggerAwakeEnabled = new ValidatedProperty<bool>(false);
			this.IsOverloadBuzzerActive = new ValidatedProperty<bool>(true);
			this.MaxLogFileSize = new ValidatedProperty<uint>(1024u);
		}
	}
}
