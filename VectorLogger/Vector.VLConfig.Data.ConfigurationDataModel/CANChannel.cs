using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "CANChannel")]
	public class CANChannel : HardwareChannel
	{
		[DataMember(Name = "CANChannelIsKeepAwakeActive")]
		public ValidatedProperty<bool> IsKeepAwakeActive
		{
			get;
			set;
		}

		[DataMember(Name = "CANChannelIsWakeUpEnabled")]
		public ValidatedProperty<bool> IsWakeUpEnabled
		{
			get;
			set;
		}

		[DataMember(Name = "CANChannelIsOutputActive")]
		public ValidatedProperty<bool> IsOutputActive
		{
			get;
			set;
		}

		[DataMember(Name = "CANChannelChipConfiguration")]
		public ICANChipConfiguration CANChipConfiguration
		{
			get;
			set;
		}

		[DataMember(Name = "CANChannelLogErrorFrames")]
		public ValidatedProperty<bool> LogErrorFrames
		{
			get;
			set;
		}

		public CANChannel()
		{
			this.IsKeepAwakeActive = new ValidatedProperty<bool>(false);
			this.IsWakeUpEnabled = new ValidatedProperty<bool>(true);
			this.IsOutputActive = new ValidatedProperty<bool>(false);
			this.CANChipConfiguration = new CANStdChipConfiguration();
			this.LogErrorFrames = new ValidatedProperty<bool>(false);
		}
	}
}
