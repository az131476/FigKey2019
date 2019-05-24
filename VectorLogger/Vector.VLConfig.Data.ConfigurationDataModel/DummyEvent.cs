using System;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	public class DummyEvent : Event
	{
		private readonly ValidatedProperty<bool> mIsPointInTime = new ValidatedProperty<bool>(false);

		public override ValidatedProperty<bool> IsPointInTime
		{
			get
			{
				return this.mIsPointInTime;
			}
		}

		public DummyEvent()
		{
		}

		public DummyEvent(DummyEvent other) : this()
		{
		}

		public override object Clone()
		{
			return new DummyEvent(this);
		}
	}
}
