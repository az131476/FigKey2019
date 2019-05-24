using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "RawDataSignalByte")]
	public class RawDataSignalByte : RawDataSignal
	{
		public override IEnumerable<IValidatedProperty> ValidatedPropertyListCondition
		{
			get
			{
				return new List<IValidatedProperty>
				{
					this.DataBytePos
				};
			}
		}

		[DataMember(Name = "DataBytePos")]
		public ValidatedProperty<uint> DataBytePos
		{
			get;
			set;
		}

		public RawDataSignalByte()
		{
			this.DataBytePos = new ValidatedProperty<uint>(0u);
		}
	}
}
