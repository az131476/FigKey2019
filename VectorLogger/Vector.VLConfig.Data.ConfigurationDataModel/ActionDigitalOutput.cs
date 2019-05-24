using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "ActionDigitalOutput")]
	public class ActionDigitalOutput : Action
	{
		public ActionDigitalOutput()
		{
		}

		public ActionDigitalOutput(ActionDigitalOutput other) : this()
		{
			base.Assign(other);
		}

		public override object Clone()
		{
			return new ActionDigitalOutput(this);
		}

		public override bool Equals(Action action)
		{
			return action != null && !(base.GetType() != action.GetType()) && base.Equals(action);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public void Assign(ActionDigitalOutput other)
		{
			base.Assign(other);
		}
	}
}
