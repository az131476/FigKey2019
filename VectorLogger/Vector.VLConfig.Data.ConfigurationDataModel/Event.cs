using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "Event")]
	public abstract class Event : ICloneable
	{
		private static ulong sUniqueId;

		public ulong Id = Event.sUniqueId += 1uL;

		public abstract ValidatedProperty<bool> IsPointInTime
		{
			get;
		}

		public virtual object Clone()
		{
			return new object();
		}

		public virtual void SetUniqueId()
		{
			if (this.Id == 0uL)
			{
				this.Id = (Event.sUniqueId += 1uL);
			}
		}
	}
}
