using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "LEDConfiguration")]
	public class LEDConfiguration : Feature
	{
		[DataMember(Name = "LEDConfigurationItemList")]
		private List<LEDConfigListItem> mLedItemList;

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

		public uint NumberOfLEDs
		{
			get
			{
				return (uint)this.mLedItemList.Count;
			}
			set
			{
				this.mLedItemList.Clear();
				for (uint num = 0u; num < value; num += 1u)
				{
					this.mLedItemList.Add(new LEDConfigListItem());
				}
			}
		}

		public ReadOnlyCollection<LEDConfigListItem> LEDConfigList
		{
			get
			{
				return new ReadOnlyCollection<LEDConfigListItem>(this.mLedItemList);
			}
		}

		public override bool UpdateOnDependency(Feature otherFeature, IUpdateServiceForFeature updateService)
		{
			if (otherFeature is LEDConfiguration || otherFeature is SendMessageConfiguration || otherFeature is TriggerConfiguration || otherFeature is CANChannelConfiguration || otherFeature is LINChannelConfiguration)
			{
				updateService.Notify<LEDConfiguration>(this);
				return true;
			}
			return false;
		}

		public override void Update(IUpdateServiceForFeature updateService)
		{
			updateService.Notify<LEDConfiguration>(this);
		}

		public LEDConfiguration()
		{
			this.mLedItemList = new List<LEDConfigListItem>();
		}
	}
}
