using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "BufferSizeCalculatorInformation")]
	public class BufferSizeCalculatorInformation
	{
		[DataMember(Name = "BufferSizeCalculatorInformationPreTriggerTimeSeconds")]
		private ValidatedProperty<uint> mPreTriggerTimeSeconds;

		[DataMember(Name = "BufferSizeCalculatorInformationChannelItems")]
		private List<BufferSizeCalculatorChannelItem> mChannelItems;

		public ValidatedProperty<uint> PreTriggerTimeSeconds
		{
			get
			{
				ValidatedProperty<uint> arg_1B_0;
				if ((arg_1B_0 = this.mPreTriggerTimeSeconds) == null)
				{
					arg_1B_0 = (this.mPreTriggerTimeSeconds = new ValidatedProperty<uint>());
				}
				return arg_1B_0;
			}
			set
			{
				this.mPreTriggerTimeSeconds = value;
			}
		}

		public List<BufferSizeCalculatorChannelItem> ChannelItems
		{
			get
			{
				List<BufferSizeCalculatorChannelItem> arg_1B_0;
				if ((arg_1B_0 = this.mChannelItems) == null)
				{
					arg_1B_0 = (this.mChannelItems = new List<BufferSizeCalculatorChannelItem>());
				}
				return arg_1B_0;
			}
		}
	}
}
