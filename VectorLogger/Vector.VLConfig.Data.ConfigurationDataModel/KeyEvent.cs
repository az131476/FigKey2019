using System;
using System.Runtime.Serialization;
using Vector.VLConfig.LoggerSpecifics;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "KeyEvent")]
	public class KeyEvent : Event
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

		[DataMember(Name = "KeyEventNumber")]
		public ValidatedProperty<uint> Number
		{
			get;
			set;
		}

		[DataMember(Name = "KeyEventIsOnPanel")]
		public ValidatedProperty<bool> IsOnPanel
		{
			get;
			set;
		}

		public bool IsCasKey
		{
			get
			{
				return this.Number == null || this.Number.Value > Constants.CasKeyOffset;
			}
		}

		public KeyEvent()
		{
			this.Number = new ValidatedProperty<uint>(1u);
			this.IsOnPanel = new ValidatedProperty<bool>(false);
		}

		public KeyEvent(KeyEvent other) : this()
		{
			this.Assign(other);
		}

		public override object Clone()
		{
			return new KeyEvent(this);
		}

		public override bool Equals(object obj)
		{
			if (obj == null || base.GetType() != obj.GetType())
			{
				return false;
			}
			KeyEvent keyEvent = (KeyEvent)obj;
			return this.IsOnPanel.Value == keyEvent.IsOnPanel.Value && this.Number.Value == keyEvent.Number.Value;
		}

		public override int GetHashCode()
		{
			return this.IsOnPanel.Value.GetHashCode() ^ this.Number.Value.GetHashCode();
		}

		public void Assign(KeyEvent other)
		{
			this.IsOnPanel.Value = other.IsOnPanel.Value;
			this.Number.Value = other.Number.Value;
		}
	}
}
