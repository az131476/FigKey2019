using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "CANChannelConfiguration")]
	public class CANChannelConfiguration : Feature
	{
		[DataMember(Name = "CANChannelConfigurationCANChannelList")]
		private List<CANChannel> canChannelList;

		[DataMember(Name = "CANChannelConfigurationLogErrorFramesOnMemories")]
		public List<ValidatedProperty<bool>> LogErrorFramesOnMemories;

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

		public ReadOnlyCollection<CANChannel> CANChannels
		{
			get
			{
				return new ReadOnlyCollection<CANChannel>(this.canChannelList);
			}
		}

		public ReadOnlyCollection<CANChannel> ActiveCanChannels
		{
			get
			{
				return new ReadOnlyCollection<CANChannel>((from chn in this.canChannelList
				where chn.IsActive.Value
				select chn).ToList<CANChannel>());
			}
		}

		public override bool UpdateOnDependency(Feature otherFeature, IUpdateServiceForFeature updateService)
		{
			if (otherFeature is CANChannelConfiguration || otherFeature is LINChannelConfiguration || otherFeature is TriggerConfiguration)
			{
				updateService.Notify<CANChannelConfiguration>(this);
				return true;
			}
			return false;
		}

		public override void Update(IUpdateServiceForFeature updateService)
		{
			updateService.Notify<CANChannelConfiguration>(this);
		}

		public CANChannelConfiguration()
		{
			this.canChannelList = new List<CANChannel>();
			this.LogErrorFramesOnMemories = new List<ValidatedProperty<bool>>();
		}

		public void AddCANChannel(CANChannel canChannel)
		{
			this.canChannelList.Add(canChannel);
		}

		public bool RemoveCANChannel(CANChannel canChannel)
		{
			if (this.canChannelList.Contains(canChannel))
			{
				this.canChannelList.Remove(canChannel);
				return true;
			}
			return false;
		}

		public CANChannel GetCANChannel(uint channelNumber)
		{
			if ((ulong)channelNumber > (ulong)((long)this.canChannelList.Count) || channelNumber < 1u)
			{
				return null;
			}
			return this.canChannelList[(int)(channelNumber - 1u)];
		}

		public IList<uint> GetActiveCANChannelNumbers()
		{
			List<uint> list = new List<uint>();
			uint num = 1u;
			while ((ulong)num <= (ulong)((long)this.canChannelList.Count))
			{
				if (this.GetCANChannel(num).IsActive.Value)
				{
					list.Add(num);
				}
				num += 1u;
			}
			return list;
		}

		public void ClearCANChannels()
		{
			this.canChannelList.Clear();
		}

		public void ActivateAllChannels()
		{
			foreach (CANChannel current in this.canChannelList)
			{
				current.IsActive.Value = true;
			}
		}

		[OnDeserializing]
		private void OnDeserializing(StreamingContext context)
		{
			this.canChannelList = new List<CANChannel>();
		}
	}
}
