using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "LINChannelConfiguration")]
	public class LINChannelConfiguration : Feature
	{
		[DataMember(Name = "LINChannelConfigurationLINChannelList")]
		private List<LINChannel> linChannelList;

		[DataMember(Name = "LINChannelConfigurationLINprobeChannelList")]
		private List<LINprobeChannel> linprobeChannelList;

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

		public ReadOnlyCollection<LINChannel> LINChannels
		{
			get
			{
				return new ReadOnlyCollection<LINChannel>(this.linChannelList);
			}
		}

		public ReadOnlyCollection<LINprobeChannel> LINprobeChannels
		{
			get
			{
				return new ReadOnlyCollection<LINprobeChannel>(this.linprobeChannelList);
			}
		}

		[DataMember(Name = "LINChannelConfigurationIsUsingLINProbe")]
		public ValidatedProperty<bool> IsUsingLinProbe
		{
			get;
			set;
		}

		[DataMember(Name = "LINChannelConfigurationCANChannelNrUsedForLINProbe")]
		public ValidatedProperty<uint> CANChannelNrUsedForLinProbe
		{
			get;
			set;
		}

		public override bool UpdateOnDependency(Feature otherFeature, IUpdateServiceForFeature updateService)
		{
			if (otherFeature is LINChannelConfiguration || otherFeature is CANChannelConfiguration)
			{
				updateService.Notify<LINChannelConfiguration>(this);
				return true;
			}
			return false;
		}

		public override void Update(IUpdateServiceForFeature updateService)
		{
			updateService.Notify<LINChannelConfiguration>(this);
		}

		public LINChannelConfiguration()
		{
			this.linChannelList = new List<LINChannel>();
			this.linprobeChannelList = new List<LINprobeChannel>();
			this.IsUsingLinProbe = new ValidatedProperty<bool>(false);
			this.CANChannelNrUsedForLinProbe = new ValidatedProperty<uint>(1u);
		}

		public void AddLINChannel(LINChannel linChannel)
		{
			this.linChannelList.Add(linChannel);
		}

		public void AddLINprobeChannel(LINprobeChannel linprobeChannel)
		{
			this.linprobeChannelList.Add(linprobeChannel);
		}

		public bool RemoveLINChannel(LINChannel linChannel)
		{
			if (this.linChannelList.Contains(linChannel))
			{
				this.linChannelList.Remove(linChannel);
				return true;
			}
			return false;
		}

		public bool RemoveLINprobeChannel(LINprobeChannel linprobeChannel)
		{
			if (this.linprobeChannelList.Contains(linprobeChannel))
			{
				this.linprobeChannelList.Remove(linprobeChannel);
				return true;
			}
			return false;
		}

		public LINChannel GetLINChannel(uint channelNumber)
		{
			if ((ulong)channelNumber > (ulong)((long)this.linChannelList.Count) || channelNumber < 1u)
			{
				return null;
			}
			return this.linChannelList[(int)(channelNumber - 1u)];
		}

		public IList<uint> GetActiveLINChannelNumbers()
		{
			List<uint> list = new List<uint>();
			uint num = 1u;
			while ((ulong)num <= (ulong)((long)this.linChannelList.Count))
			{
				if (this.GetLINChannel(num).IsActive.Value)
				{
					list.Add(num);
				}
				num += 1u;
			}
			return list;
		}

		public LINprobeChannel GetLINprobeChannel(uint channelNumber)
		{
			if ((ulong)channelNumber < (ulong)((long)(this.linChannelList.Count + 1)) || (ulong)channelNumber > (ulong)((long)(this.linChannelList.Count + this.linprobeChannelList.Count)))
			{
				return null;
			}
			return this.linprobeChannelList[(int)(channelNumber - (uint)(this.linChannelList.Count + 1))];
		}

		public void ClearLINChannels()
		{
			this.linChannelList.Clear();
		}

		public void ClearLINprobeChannels()
		{
			this.linprobeChannelList.Clear();
		}

		public void ActivateAllChannels()
		{
			foreach (LINChannel current in this.linChannelList)
			{
				current.IsActive.Value = true;
			}
		}

		[OnDeserializing]
		private void OnDeserializing(StreamingContext context)
		{
			this.linChannelList = new List<LINChannel>();
		}
	}
}
