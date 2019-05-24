using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "AnalogInput")]
	public class AnalogInput
	{
		[DataMember(Name = "AnalogInputIsActive")]
		public ValidatedProperty<bool> IsActive
		{
			get;
			set;
		}

		[DataMember(Name = "Frequency")]
		public ValidatedProperty<uint> Frequency
		{
			get;
			set;
		}

		[DataMember(Name = "MappedCANId")]
		public ValidatedProperty<uint> MappedCANId
		{
			get;
			set;
		}

		[DataMember(Name = "IsMappedCANIdExtended")]
		public ValidatedProperty<bool> IsMappedCANIdExtended
		{
			get;
			set;
		}

		public AnalogInput()
		{
			this.IsActive = new ValidatedProperty<bool>(false);
			this.Frequency = new ValidatedProperty<uint>(0u);
			this.MappedCANId = new ValidatedProperty<uint>(0u);
			this.IsMappedCANIdExtended = new ValidatedProperty<bool>(false);
		}
	}
}
