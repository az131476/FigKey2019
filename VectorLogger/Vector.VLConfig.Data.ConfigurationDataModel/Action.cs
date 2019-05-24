using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "Action")]
	public abstract class Action : ICloneable
	{
		[DataMember(Name = "ActionIsActive")]
		public ValidatedProperty<bool> IsActive;

		[DataMember(Name = "ActionComment")]
		public ValidatedProperty<string> Comment;

		[DataMember(Name = "ActionEvent")]
		public Event Event;

		[DataMember(Name = "ActionStopType")]
		public StopType StopType;

		public Action()
		{
			this.IsActive = new ValidatedProperty<bool>(true);
			this.Comment = new ValidatedProperty<string>("");
		}

		public Action(Action other) : this()
		{
			this.Assign(other);
		}

		public virtual object Clone()
		{
			return new object();
		}

		public virtual bool Equals(Action other)
		{
			if (this.IsActive.Value != other.IsActive.Value)
			{
				return false;
			}
			if (this.Comment.Value != other.Comment.Value)
			{
				return false;
			}
			if (this.Event != null)
			{
				if (!this.Event.Equals(other.Event))
				{
					return false;
				}
			}
			else if (other.Event != null)
			{
				return false;
			}
			if (this.StopType != null)
			{
				if (!this.StopType.Equals(other.StopType))
				{
					return false;
				}
			}
			else if (other.StopType != null)
			{
				return false;
			}
			return true;
		}

		public override int GetHashCode()
		{
			int num = 0;
			if (this.Event != null)
			{
				num = this.Event.GetHashCode();
			}
			int num2 = 0;
			if (this.StopType != null)
			{
				num2 = this.StopType.GetHashCode();
			}
			return this.IsActive.Value.GetHashCode() ^ this.Comment.GetHashCode() ^ num ^ num2;
		}

		public void Assign(Action other)
		{
			this.IsActive.Value = other.IsActive.Value;
			this.Comment.Value = other.Comment.Value;
			this.Event = other.Event;
			this.StopType = other.StopType;
		}
	}
}
