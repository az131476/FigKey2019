using System;
using System.Text;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.LTLGenerationCS.LTLEvents
{
	public class LTLOnStartEventCode : LTLGenericEventCode
	{
		private OnStartEvent onStartEvent;

		public LTLOnStartEventCode(string eventFlagName, OnStartEvent ev) : base(eventFlagName)
		{
			this.onStartEvent = ev;
		}

		public override LTLGenerator.LTLGenerationResult GenerateCode(out string ltlCode)
		{
			string eventFlagNameWithoutDot = base.GetEventFlagNameWithoutDot();
			if (this.onStartEvent.Delay.Value > 0u)
			{
				base.PreEventCode = string.Format("TIMER {0}_tDelayCyclesAfterStart TIME = {1} (1)", eventFlagNameWithoutDot, this.onStartEvent.Delay.Value);
				base.TriggerEvent = string.Format("SET ({0}_tDelayCyclesAfterStart)", eventFlagNameWithoutDot);
				base.AdditionalCodeBeforeFlagSet = null;
				base.WhenCondition = null;
				base.AdditionalCodeAfterFlagSet = null;
				base.PostEventCode = null;
			}
			else
			{
				base.PreEventCode = null;
				base.TriggerEvent = string.Format("SYSTEM (StartUp)", new object[0]);
				base.AdditionalCodeBeforeFlagSet = null;
				base.WhenCondition = null;
				base.AdditionalCodeAfterFlagSet = null;
				base.PostEventCode = null;
			}
			ltlCode = base.BuildTriggerEventBlock();
			return LTLGenerator.LTLGenerationResult.OK;
		}

		protected override string GetComment()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("Event on start");
			if (this.onStartEvent.Delay.Value > 0u)
			{
				stringBuilder.AppendFormat(", delay = {0}", this.onStartEvent.Delay.Value);
			}
			return stringBuilder.ToString();
		}
	}
}
