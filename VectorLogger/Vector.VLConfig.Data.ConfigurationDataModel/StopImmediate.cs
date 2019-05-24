using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "StopImmediate")]
	public class StopImmediate : StopType
	{
		public StopImmediate()
		{
		}

		public StopImmediate(StopImmediate other) : this()
		{
			this.Assign(other);
		}

		public override object Clone()
		{
			return new StopImmediate(this);
		}

		public override bool Equals(object obj)
		{
			return obj != null && !(base.GetType() != obj.GetType());
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public void Assign(StopImmediate other)
		{
		}
	}
}
