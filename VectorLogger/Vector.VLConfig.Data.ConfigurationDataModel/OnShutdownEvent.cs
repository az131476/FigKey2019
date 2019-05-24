using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "OnShutdownEvent")]
	public class OnShutdownEvent : Event
	{
		private ValidatedProperty<bool> mIsPointInTime;

		public override ValidatedProperty<bool> IsPointInTime
		{
			get
			{
				ValidatedProperty<bool> arg_1C_0;
				if ((arg_1C_0 = this.mIsPointInTime) == null)
				{
					arg_1C_0 = (this.mIsPointInTime = new ValidatedProperty<bool>(true));
				}
				return arg_1C_0;
			}
		}

		public OnShutdownEvent()
		{
		}

		public OnShutdownEvent(OnShutdownEvent other) : this()
		{
			this.Assign(other);
		}

		public override object Clone()
		{
			return new OnShutdownEvent(this);
		}

		public override bool Equals(object obj)
		{
			return obj != null && !(base.GetType() != obj.GetType());
		}

		public override int GetHashCode()
		{
			return 0;
		}

		public void Assign(OnShutdownEvent other)
		{
		}
	}
}
