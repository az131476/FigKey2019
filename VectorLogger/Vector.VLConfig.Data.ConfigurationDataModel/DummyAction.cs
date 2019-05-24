using System;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	public class DummyAction : Action
	{
		public DummyAction(DummyAction other)
		{
			this.Comment = new ValidatedProperty<string>(other.Comment.Value);
			this.IsActive = new ValidatedProperty<bool>(other.IsActive.Value);
			this.Event = (Event)other.Event.Clone();
		}

		public DummyAction(Event evt)
		{
			this.Comment = new ValidatedProperty<string>(string.Empty);
			this.IsActive = new ValidatedProperty<bool>(true);
			this.Event = (Event)evt.Clone();
		}
	}
}
