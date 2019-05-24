using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "RawDataSignalStartbitLength")]
	public class RawDataSignalStartbitLength : RawDataSignal
	{
		public override IEnumerable<IValidatedProperty> ValidatedPropertyListCondition
		{
			get
			{
				return new List<IValidatedProperty>
				{
					this.StartbitPos,
					this.Length,
					this.IsMotorola
				};
			}
		}

		[DataMember(Name = "StartbitPos")]
		public ValidatedProperty<uint> StartbitPos
		{
			get;
			set;
		}

		[DataMember(Name = "Length")]
		public ValidatedProperty<uint> Length
		{
			get;
			set;
		}

		[DataMember(Name = "IsMotorola")]
		public ValidatedProperty<bool> IsMotorola
		{
			get;
			set;
		}

		public RawDataSignalStartbitLength()
		{
			this.StartbitPos = new ValidatedProperty<uint>(0u);
			this.Length = new ValidatedProperty<uint>(0u);
			this.IsMotorola = new ValidatedProperty<bool>(false);
		}
	}
}
