using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "FlexrayChannelConfiguration")]
	public class FlexrayChannelConfiguration : Feature
	{
		[DataMember(Name = "FlexrayChannelConfigurationChannelList")]
		private List<FlexrayChannel> flexrayChannelList;

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

		public ReadOnlyCollection<FlexrayChannel> FlexrayChannels
		{
			get
			{
				return new ReadOnlyCollection<FlexrayChannel>(this.flexrayChannelList);
			}
		}

		public override bool UpdateOnDependency(Feature otherFeature, IUpdateServiceForFeature updateService)
		{
			if (otherFeature is FlexrayChannelConfiguration)
			{
				updateService.Notify<FlexrayChannelConfiguration>(this);
				return true;
			}
			return false;
		}

		public override void Update(IUpdateServiceForFeature updateService)
		{
			updateService.Notify<FlexrayChannelConfiguration>(this);
		}

		public FlexrayChannelConfiguration()
		{
			this.flexrayChannelList = new List<FlexrayChannel>();
		}

		public void AddFlexrayChannel(FlexrayChannel channel)
		{
			this.flexrayChannelList.Add(channel);
		}

		public bool RemoveFlexrayChannel(FlexrayChannel channel)
		{
			if (this.flexrayChannelList.Contains(channel))
			{
				this.flexrayChannelList.Remove(channel);
				return true;
			}
			return false;
		}

		public FlexrayChannel GetFlexrayChannel(uint channelNumber)
		{
			if ((ulong)channelNumber > (ulong)((long)this.flexrayChannelList.Count) || channelNumber < 1u)
			{
				return null;
			}
			return this.flexrayChannelList[(int)(channelNumber - 1u)];
		}

		public IList<uint> GetActiveFlexrayChannelNumbers()
		{
			List<uint> list = new List<uint>();
			uint num = 1u;
			while ((ulong)num <= (ulong)((long)this.flexrayChannelList.Count))
			{
				if (this.GetFlexrayChannel(num).IsActive.Value)
				{
					list.Add(num);
				}
				num += 1u;
			}
			return list;
		}

		public void ClearFlexrayChannels()
		{
			this.flexrayChannelList.Clear();
		}

		public void ActivateAllChannels()
		{
			foreach (FlexrayChannel current in this.flexrayChannelList)
			{
				current.IsActive.Value = true;
			}
		}
	}
}
