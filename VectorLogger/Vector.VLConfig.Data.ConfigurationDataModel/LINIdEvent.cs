using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "LINIdEvent")]
	public class LINIdEvent : IdEvent
	{
		public LINIdEvent()
		{
		}

		public LINIdEvent(LINIdEvent other) : this()
		{
			this.Assign(other);
		}

		public override object Clone()
		{
			return new LINIdEvent(this);
		}

		public override bool Equals(object obj)
		{
			return obj != null && !(base.GetType() != obj.GetType()) && base.Equals(obj);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public void Assign(LINIdEvent other)
		{
			base.Assign(other);
		}
	}
}
