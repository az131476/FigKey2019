using System;
using System.Text;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.LTLGenerationCS.LTLEvents
{
	public class LTLVoCANEventCode : LTLGenericEventCode
	{
		private VoCanRecordingEvent voCANEvent;

		private string sKey;

		public LTLVoCANEventCode(string eventFlagName, VoCanRecordingEvent ev) : base(eventFlagName)
		{
			this.voCANEvent = ev;
		}

		public override LTLGenerator.LTLGenerationResult GenerateCode(out string ltlCode)
		{
			if (GlobalSettings.IsVoCANalreadyConfigured)
			{
				ltlCode = string.Empty;
				return LTLGenerator.LTLGenerationResult.EventsVoCANMultipleConfiguredError;
			}
			bool value = this.voCANEvent.IsUsingCASM2T3L.Value;
			if (value)
			{
				this.sKey = "Key9";
			}
			else
			{
				this.sKey = "Key8";
			}
			base.PreEventCode = null;
			base.TriggerEvent = string.Format("SET ({0})", this.sKey);
			base.WhenCondition = null;
			base.AdditionalCodeAfterFlagSet = null;
			StringBuilder stringBuilder = new StringBuilder();
			if (this.voCANEvent.IsFixedRecordDuration.Value)
			{
				base.AdditionalCodeBeforeFlagSet = string.Format("CALC VoiceRecording = {0:D}", this.voCANEvent.Duration_s.Value * 1000u);
				base.PostEventCode = null;
			}
			else
			{
				base.AdditionalCodeBeforeFlagSet = string.Format("CALC VoiceRecording = 2000 {{dummy value}}", new object[0]);
				stringBuilder.AppendLine("EVENT ON CYCLE (1000) BEGIN");
				stringBuilder.AppendFormat("  CALC VoiceRecording = 2000 WHEN ({0})", this.sKey);
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("END");
				stringBuilder.AppendFormat("EVENT ON RESET ({0}) BEGIN", this.sKey);
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("  CALC VoiceRecording = 0");
				stringBuilder.AppendLine("END");
				stringBuilder.AppendLine();
			}
			if (this.voCANEvent.IsRecordingLEDActive.Value)
			{
				stringBuilder.AppendLine("OUTPUT LED8 = (VoiceRecording > 0)");
				stringBuilder.AppendLine();
			}
			if (this.voCANEvent.IsBeepOnEndOn.Value)
			{
				stringBuilder.AppendLine("VAR VoiceRecordingStopped = FREE[1]");
				stringBuilder.AppendLine("EVENT ON RESET (VoiceRecording > 0) BEGIN");
				stringBuilder.AppendLine("  CALC VoiceRecordingStopped = (1)");
				stringBuilder.AppendLine("END");
				stringBuilder.AppendLine("FLAG BeepOnVoiceRecordingStopped SET = (VoiceRecordingStopped) RESET (VoiceRecording > 0)  SOUND (LO)");
				stringBuilder.AppendLine("EVENT ON SET (BeepOnVoiceRecordingStopped) BEGIN");
				stringBuilder.AppendLine("  CALC VoiceRecordingStopped = (0)");
				stringBuilder.AppendLine("END");
				stringBuilder.AppendLine();
			}
			base.PostEventCode = stringBuilder.ToString();
			GlobalSettings.UseAuxSwitchBox();
			GlobalSettings.UseVoCAN();
			ltlCode = base.BuildTriggerEventBlock();
			return LTLGenerator.LTLGenerationResult.OK;
		}

		protected override string GetComment()
		{
			if (this.voCANEvent.IsFixedRecordDuration.Value)
			{
				return string.Format("Event on VoCAN recording of {0} s.", this.voCANEvent.Duration_s.Value);
			}
			return "Event on VoCAN recording while key is pressed.";
		}

		public override LTLGenerator.LTLGenerationResult GetCurrentStateFlag(out string ltlVariable)
		{
			ltlVariable = this.sKey;
			return LTLGenerator.LTLGenerationResult.OK;
		}
	}
}
