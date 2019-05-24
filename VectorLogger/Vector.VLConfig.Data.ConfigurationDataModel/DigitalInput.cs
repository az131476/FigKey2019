using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "DigitalInput")]
	public class DigitalInput
	{
		[DataMember(Name = "DigitalInputIsActiveOnChange")]
		public ValidatedProperty<bool> IsActiveOnChange
		{
			get;
			set;
		}

		[DataMember(Name = "DigitalInputIsActiveFrequency")]
		public ValidatedProperty<bool> IsActiveFrequency
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

		public DigitalInput()
		{
			this.IsActiveOnChange = new ValidatedProperty<bool>(false);
			this.IsActiveFrequency = new ValidatedProperty<bool>(false);
			this.Frequency = new ValidatedProperty<uint>(0u);
			this.MappedCANId = new ValidatedProperty<uint>(0u);
			this.IsMappedCANIdExtended = new ValidatedProperty<bool>(false);
		}
	}
}
