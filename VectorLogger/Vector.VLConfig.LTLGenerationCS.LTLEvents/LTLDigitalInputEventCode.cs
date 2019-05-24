using System;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.LTLGenerationCS.LTLEvents
{
	public class LTLDigitalInputEventCode : LTLGenericEventCode
	{
		private DigitalInputEvent digitalInputEvent;

		public LTLDigitalInputEventCode(string eventFlagName, DigitalInputEvent ev) : base(eventFlagName)
		{
			this.digitalInputEvent = ev;
		}

		public override LTLGenerator.LTLGenerationResult GenerateCode(out string ltlCode)
		{
			if (this.digitalInputEvent.IsDebounceActive.Value)
			{
				base.PreEventCode = string.Format("TIMER Debounced_{0} TIME = {1} (DigIn{2:D} = {3})", new object[]
				{
					base.GetEventFlagNameWithoutDot(),
					this.digitalInputEvent.DebounceTime.Value,
					this.digitalInputEvent.DigitalInput.Value,
					this.digitalInputEvent.Edge.Value ? 1 : 0
				});
				base.TriggerEvent = string.Format("SET (Debounced_{0})", base.GetEventFlagNameWithoutDot());
			}
			else
			{
				base.PreEventCode = "";
				if (this.digitalInputEvent.Edge.Value)
				{
					base.TriggerEvent = string.Format("SET (DigIn{0:D})", this.digitalInputEvent.DigitalInput.Value);
				}
				else
				{
					base.TriggerEvent = string.Format("RESET (DigIn{0:D})", this.digitalInputEvent.DigitalInput.Value);
				}
			}
			base.WhenCondition = null;
			base.AdditionalCodeBeforeFlagSet = null;
			base.AdditionalCodeAfterFlagSet = null;
			base.PostEventCode = null;
			ltlCode = base.BuildTriggerEventBlock();
			return LTLGenerator.LTLGenerationResult.OK;
		}

		public override LTLGenerator.LTLGenerationResult GetCurrentStateFlag(out string ltlVariable)
		{
			if (this.digitalInputEvent.IsDebounceActive.Value)
			{
				ltlVariable = string.Format("Debounced_{0}", base.GetEventFlagNameWithoutDot());
			}
			else if (this.digitalInputEvent.Edge.Value)
			{
				ltlVariable = string.Format("DigIn{0:D}", this.digitalInputEvent.DigitalInput.Value);
			}
			else
			{
				ltlVariable = string.Format("NOT DigIn{0:D}", this.digitalInputEvent.DigitalInput.Value);
			}
			return LTLGenerator.LTLGenerationResult.OK;
		}

		protected override string GetComment()
		{
			return string.Format("Event on DigitalInput {0} {1}", this.digitalInputEvent.DigitalInput.Value, this.digitalInputEvent.Edge.Value ? "LOW -> HIGH" : "HIGH -> LOW");
		}
	}
}
