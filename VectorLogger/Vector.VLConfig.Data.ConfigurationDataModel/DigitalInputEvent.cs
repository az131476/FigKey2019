using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "DigitalInputEvent")]
	public class DigitalInputEvent : Event
	{
		private ValidatedProperty<bool> mIsPointInTime;

		public override ValidatedProperty<bool> IsPointInTime
		{
			get
			{
				ValidatedProperty<bool> arg_1C_0;
				if ((arg_1C_0 = this.mIsPointInTime) == null)
				{
					arg_1C_0 = (this.mIsPointInTime = new ValidatedProperty<bool>(false));
				}
				return arg_1C_0;
			}
		}

		[DataMember(Name = "DigitalInputEventDigitaInput")]
		public ValidatedProperty<uint> DigitalInput
		{
			get;
			set;
		}

		[DataMember(Name = "DigitalInputEventEdge")]
		public ValidatedProperty<bool> Edge
		{
			get;
			set;
		}

		[DataMember(Name = "DigitalInputIsDebounceActive")]
		public ValidatedProperty<bool> IsDebounceActive
		{
			get;
			set;
		}

		[DataMember(Name = "DigitalInputDebounceTime")]
		public ValidatedProperty<uint> DebounceTime
		{
			get;
			set;
		}

		public DigitalInputEvent()
		{
			this.DigitalInput = new ValidatedProperty<uint>(1u);
			this.Edge = new ValidatedProperty<bool>(false);
			this.IsDebounceActive = new ValidatedProperty<bool>(true);
			this.DebounceTime = new ValidatedProperty<uint>(50u);
		}

		public DigitalInputEvent(DigitalInputEvent other) : this()
		{
			this.Assign(other);
		}

		public override object Clone()
		{
			return new DigitalInputEvent(this);
		}

		public override bool Equals(object obj)
		{
			if (obj == null || base.GetType() != obj.GetType())
			{
				return false;
			}
			DigitalInputEvent digitalInputEvent = (DigitalInputEvent)obj;
			return this.DigitalInput.Value == digitalInputEvent.DigitalInput.Value && this.Edge.Value == digitalInputEvent.Edge.Value && this.IsDebounceActive.Value == digitalInputEvent.IsDebounceActive.Value && this.DebounceTime.Value == digitalInputEvent.DebounceTime.Value;
		}

		public override int GetHashCode()
		{
			return this.DigitalInput.Value.GetHashCode() ^ this.Edge.Value.GetHashCode() ^ this.IsDebounceActive.Value.GetHashCode() ^ this.DebounceTime.Value.GetHashCode();
		}

		public void Assign(DigitalInputEvent other)
		{
			this.DigitalInput.Value = other.DigitalInput.Value;
			this.Edge.Value = other.Edge.Value;
			this.IsDebounceActive.Value = other.IsDebounceActive.Value;
			this.DebounceTime.Value = other.DebounceTime.Value;
		}
	}
}
