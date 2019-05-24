using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.LTLGenerationCS.LTLEvents
{
	public class LTLReferencedTriggerNameEventCode : LTLGenericEventCode
	{
		private ReferencedRecordTriggerNameEvent referencedTriggerNameEvent;

		public LTLReferencedTriggerNameEventCode(string eventFlagName, ReferencedRecordTriggerNameEvent ev) : base(eventFlagName)
		{
			this.referencedTriggerNameEvent = ev;
		}

		public override LTLGenerator.LTLGenerationResult GenerateCode(out string ltlCode)
		{
			ltlCode = "";
			if (this.referencedTriggerNameEvent.NameOfTrigger.Value == null || this.referencedTriggerNameEvent.NameOfTrigger.Value.Contains("*"))
			{
				Match match = Regex.Match(this.referencedTriggerNameEvent.NameOfTrigger.Value, "\\*(\\d)");
				if (!match.Success || match.Groups.Count < 2)
				{
					return LTLGenerator.LTLGenerationResult.TriggerError_TriggerNameResolve;
				}
				int num;
				try
				{
					string value = match.Groups[1].Value;
					num = Convert.ToInt32(value);
				}
				catch
				{
					LTLGenerator.LTLGenerationResult result = LTLGenerator.LTLGenerationResult.TriggerError_TriggerNameResolve;
					return result;
				}
				StringBuilder stringBuilder = new StringBuilder();
				IList<int> fromProjectMemoriesThatUseTriggeredLogging = GlobalSettings.GetFromProjectMemoriesThatUseTriggeredLogging();
				if (fromProjectMemoriesThatUseTriggeredLogging.Count <= 0 || !fromProjectMemoriesThatUseTriggeredLogging.Contains(num))
				{
					stringBuilder.AppendLine("{ The wildcard-trigger's memory is not configured for Triggered Logging - thus the event can never occur -> no LTL code}");
					stringBuilder.AppendFormat("VAR {0}_dummy = FREE[1]", this.eventFlagName);
					base.PreEventCode = stringBuilder.ToString();
					base.TriggerEvent = string.Format("SET ({0}_dummy)", this.eventFlagName);
					base.WhenCondition = string.Format("0", new object[0]);
					ltlCode = base.BuildTriggerEventBlock();
					return LTLGenerator.LTLGenerationResult.OK;
				}
				stringBuilder.AppendFormat("VAR {0}_lastLogger{1:D}LastFile = FREE[16]", this.eventFlagName, num);
				stringBuilder.AppendLine();
				base.PreEventCode = stringBuilder.ToString();
				base.TriggerEvent = "CYCLE (1000)";
				base.AdditionalCodeBeforeFlagSet = null;
				StringBuilder stringBuilder2 = new StringBuilder();
				stringBuilder2.AppendFormat("Logger{1:D}LastFile > {0}_lastLogger{1:D}LastFile", this.eventFlagName, num);
				base.WhenCondition = stringBuilder2.ToString();
				StringBuilder stringBuilder3 = new StringBuilder();
				stringBuilder3.AppendFormat("  CALC {0}_lastLogger{1:D}LastFile = (Logger{1:D}LastFile)", this.eventFlagName, num);
				base.AdditionalCodeAfterFlagSet = stringBuilder3.ToString();
				base.PostEventCode = null;
			}
			else
			{
				string arg;
				int num2;
				try
				{
					GlobalSettings.TNE_GetInfosFromTriggerName(this.referencedTriggerNameEvent.NameOfTrigger.Value, out arg, out num2);
				}
				catch (LTLGenerationException ex)
				{
					LTLGenerator.LTLGenerationResult result = ex.Result;
					return result;
				}
				StringBuilder stringBuilder4 = new StringBuilder();
				stringBuilder4.AppendFormat("VAR {0}_lastLogger{1:D}LastFile = FREE[16]", this.eventFlagName, num2);
				stringBuilder4.AppendLine();
				stringBuilder4.AppendFormat("VAR {0}_triggerOccured = FREE[1]", this.eventFlagName);
				stringBuilder4.AppendLine();
				stringBuilder4.AppendFormat("EVENT ON CALC ({0} = (1)) BEGIN", arg);
				stringBuilder4.AppendLine();
				stringBuilder4.AppendFormat("  CALC {0}_lastLogger{1:D}LastFile = (Logger{1:D}LastFile)", this.eventFlagName, num2);
				stringBuilder4.AppendLine();
				stringBuilder4.AppendFormat("  CALC {0}_triggerOccured = (1)", this.eventFlagName);
				stringBuilder4.AppendLine();
				stringBuilder4.AppendLine("END");
				base.PreEventCode = stringBuilder4.ToString();
				base.TriggerEvent = "CYCLE (1000)";
				base.AdditionalCodeBeforeFlagSet = null;
				StringBuilder stringBuilder5 = new StringBuilder();
				stringBuilder5.AppendFormat("{0}_triggerOccured AND Logger{1:D}LastFile > {0}_lastLogger{1:D}LastFile", this.eventFlagName, num2);
				base.WhenCondition = stringBuilder5.ToString();
				StringBuilder stringBuilder6 = new StringBuilder();
				stringBuilder6.AppendFormat("  CALC {0}_triggerOccured = (0) WHEN ({0} = 1)", this.eventFlagName);
				base.AdditionalCodeAfterFlagSet = stringBuilder6.ToString();
				base.PostEventCode = null;
			}
			ltlCode = base.BuildTriggerEventBlock();
			return LTLGenerator.LTLGenerationResult.OK;
		}

		protected override string GetComment()
		{
			return string.Format("After referenced trigger event \"{0}\".", this.referencedTriggerNameEvent.NameOfTrigger.Value);
		}
	}
}
