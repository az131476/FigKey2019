using System;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.LTLGenerationCS.LTLEvents
{
	public class LTLNextLoggerStartEventCode : LTLGenericEventCode
	{
		private NextLogSessionStartEvent nextSessionStartEvent;

		public LTLNextLoggerStartEventCode(string eventFlagName, NextLogSessionStartEvent ev) : base(eventFlagName)
		{
			this.nextSessionStartEvent = ev;
		}

		public override LTLGenerator.LTLGenerationResult GenerateCode(out string ltlCode)
		{
			base.PreEventCode = null;
			base.TriggerEvent = "SYSTEM (Shutdown)";
			base.WhenCondition = null;
			base.AdditionalCodeAfterFlagSet = null;
			base.PostEventCode = null;
			ltlCode = base.BuildTriggerEventBlock();
			return LTLGenerator.LTLGenerationResult.OK;
		}

		protected override string GetComment()
		{
			return string.Format("Event on next logger start", new object[0]);
		}
	}
}
