using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "StopType")]
	public abstract class StopType : ICloneable
	{
		public virtual object Clone()
		{
			return new object();
		}
	}
}
