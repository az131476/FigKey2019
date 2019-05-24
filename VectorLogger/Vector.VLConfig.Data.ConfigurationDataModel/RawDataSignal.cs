using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "RawDataSignal")]
	public abstract class RawDataSignal
	{
		public abstract IEnumerable<IValidatedProperty> ValidatedPropertyListCondition
		{
			get;
		}
	}
}
