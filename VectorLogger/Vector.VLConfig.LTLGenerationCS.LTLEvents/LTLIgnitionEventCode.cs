using System;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.LTLGenerationCS.LTLEvents
{
	public class LTLIgnitionEventCode : LTLGenericEventCode
	{
		private IgnitionEvent ignitionEvent;

		public LTLIgnitionEventCode(string eventFlagName, IgnitionEvent ev) : base(eventFlagName)
		{
			this.ignitionEvent = ev;
		}

		public override LTLGenerator.LTLGenerationResult GenerateCode(out string ltlCode)
		{
			base.PreEventCode = null;
			if (this.ignitionEvent.IsOn.Value)
			{
				base.TriggerEvent = "SET (IgnitionInput)";
			}
			else
			{
				base.TriggerEvent = "RESET (IgnitionInput)";
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
			if (this.ignitionEvent.IsOn.Value)
			{
				ltlVariable = "IgnitionInput";
			}
			else
			{
				ltlVariable = "NOT IgnitionInput";
			}
			return LTLGenerator.LTLGenerationResult.OK;
		}

		protected override string GetComment()
		{
			return string.Format("Event on ignition {0}", this.ignitionEvent.IsOn.Value ? "On" : "Off");
		}
	}
}
