using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "LINChannel")]
	public class LINChannel : HardwareChannel
	{
		[DataMember(Name = "LINChannelIsKeepAwakeActive")]
		public ValidatedProperty<bool> IsKeepAwakeActive
		{
			get;
			set;
		}

		[DataMember(Name = "LINChannelIsWakeUpEnabled")]
		public ValidatedProperty<bool> IsWakeUpEnabled
		{
			get;
			set;
		}

		[DataMember(Name = "LINChannelBaudrate")]
		public ValidatedProperty<uint> SpeedRate
		{
			get;
			set;
		}

		[DataMember(Name = "LINChannelProtocolVersion")]
		public ValidatedProperty<int> ProtocolVersion
		{
			get;
			set;
		}

		[DataMember(Name = "LINChannelUseDbConfigValues")]
		public ValidatedProperty<bool> UseDbConfigValues
		{
			get;
			set;
		}

		public LINChannel()
		{
			this.IsKeepAwakeActive = new ValidatedProperty<bool>(false);
			this.IsWakeUpEnabled = new ValidatedProperty<bool>(true);
			this.SpeedRate = new ValidatedProperty<uint>(0u);
			this.ProtocolVersion = new ValidatedProperty<int>(0);
			this.UseDbConfigValues = new ValidatedProperty<bool>(false);
		}
	}
}
