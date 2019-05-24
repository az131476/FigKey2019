using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "DataItem")]
	public class DataItem : ICloneable
	{
		[DataMember(Name = "DataByte")]
		public ValidatedProperty<byte> Byte;

		public DataItem()
		{
			this.Byte = new ValidatedProperty<byte>(0);
		}

		public DataItem(DataItem other) : this()
		{
			this.Assign(other);
		}

		public virtual object Clone()
		{
			return new DataItem(this);
		}

		public override bool Equals(object obj)
		{
			if (obj == null || base.GetType() != obj.GetType())
			{
				return false;
			}
			DataItem dataItem = (DataItem)obj;
			return this.Byte.Value == dataItem.Byte.Value;
		}

		public override int GetHashCode()
		{
			return this.Byte.Value.GetHashCode();
		}

		public void Assign(DataItem other)
		{
			this.Byte.Value = other.Byte.Value;
		}
	}
}
