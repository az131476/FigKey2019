using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "FlexrayChannel")]
	public class FlexrayChannel : HardwareChannel
	{
		[DataMember(Name = "FlexrayChannelIsKeepAwakeActive")]
		public ValidatedProperty<bool> IsKeepAwakeActive
		{
			get;
			set;
		}

		[DataMember(Name = "FlexrayChannelIsWakeUpEnabled")]
		public ValidatedProperty<bool> IsWakeUpEnabled
		{
			get;
			set;
		}

		public FlexrayChannel()
		{
			this.IsKeepAwakeActive = new ValidatedProperty<bool>(false);
			this.IsWakeUpEnabled = new ValidatedProperty<bool>(true);
		}
	}
}
