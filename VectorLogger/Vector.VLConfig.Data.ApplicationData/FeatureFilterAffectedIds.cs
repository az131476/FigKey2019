using System;
using System.Collections.Generic;

namespace Vector.VLConfig.Data.ApplicationData
{
	public class FeatureFilterAffectedIds
	{
		public Dictionary<string, Dictionary<uint, List<CANIdInterval>>> UsedCanIdsOnChannelsForFeature;

		public Dictionary<string, Dictionary<uint, Dictionary<uint, uint>>> UsedLinIdsOnChannelsForFeature;

		public Dictionary<string, Dictionary<uint, Dictionary<MessageDefinition, MessageDefinition>>> UsedFlexRayFramesOnChannelsForFeature;

		public FeatureFilterAffectedIds()
		{
			this.UsedCanIdsOnChannelsForFeature = new Dictionary<string, Dictionary<uint, List<CANIdInterval>>>();
			this.UsedLinIdsOnChannelsForFeature = new Dictionary<string, Dictionary<uint, Dictionary<uint, uint>>>();
			this.UsedFlexRayFramesOnChannelsForFeature = new Dictionary<string, Dictionary<uint, Dictionary<MessageDefinition, MessageDefinition>>>();
		}

		public FeatureFilterAffectedIds(Dictionary<string, Dictionary<uint, List<CANIdInterval>>> usedCanIds, Dictionary<string, Dictionary<uint, Dictionary<uint, uint>>> usedLinIds, Dictionary<string, Dictionary<uint, Dictionary<MessageDefinition, MessageDefinition>>> usedFlexRayFrames)
		{
			this.UsedCanIdsOnChannelsForFeature = usedCanIds;
			this.UsedLinIdsOnChannelsForFeature = usedLinIds;
			this.UsedFlexRayFramesOnChannelsForFeature = usedFlexRayFrames;
		}
	}
}
