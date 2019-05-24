using System;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.LTLGenerationCS.LTLEvents
{
	public class LTLShutdownEventCode : LTLGenericEventCode
	{
		private OnShutdownEvent loggerShutdownEvent;

		public LTLShutdownEventCode(string eventFlagName, OnShutdownEvent ev) : base(eventFlagName)
		{
			this.loggerShutdownEvent = ev;
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
			return string.Format("Event on logger shutdown", new object[0]);
		}
	}
}
