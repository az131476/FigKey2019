using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.LoggerSpecifics;

namespace Vector.VLConfig.LTLGenerationCS
{
	internal class LTLActionTrigger : LTLGenericAction
	{
		private TriggerConfiguration triggerConfig;

		private List<int> beepTriggers;

		private bool isInterfaceModeUsed;

		private RingBufferOperatingMode rbOpMode;

		protected override string SectionHeaderComment
		{
			get
			{
				if (this.loggerSpecifics.DataStorage.NumberOfMemories > 1u)
				{
					return string.Format("TRIGGERED LOGGING ON MEMORY {0}", this.triggerConfig.MemoryNr);
				}
				return "TRIGGERED LOGGING";
			}
		}

		public LTLActionTrigger(TriggerConfiguration triggerConfig, ILoggerSpecifics loggerSpecifics, IApplicationDatabaseManager databaseManager, string configurationFolderPath, bool isInterfaceModeUsed, RingBufferOperatingMode rbOpMode) : base(loggerSpecifics, databaseManager, configurationFolderPath)
		{
			this.triggerConfig = triggerConfig;
			this.beepTriggers = new List<int>();
			this.isInterfaceModeUsed = isInterfaceModeUsed;
			this.rbOpMode = rbOpMode;
		}

		protected override bool IsFeatureSupportedByLogger()
		{
			return true;
		}

		protected override bool IsNoActionAvtive(out string textForComment)
		{
			textForComment = "no triggers configured";
			return this.triggerConfig.TriggerMode.Value != TriggerMode.Triggered || !this.triggerConfig.ActiveTriggers.Any<RecordTrigger>();
		}

		protected override List<Vector.VLConfig.Data.ConfigurationDataModel.Action> GetActionList()
		{
			List<Vector.VLConfig.Data.ConfigurationDataModel.Action> list = new List<Vector.VLConfig.Data.ConfigurationDataModel.Action>();
			ReadOnlyCollection<RecordTrigger> triggers = this.triggerConfig.Triggers;
			foreach (RecordTrigger current in triggers)
			{
				Vector.VLConfig.Data.ConfigurationDataModel.Action item = new PseudoActionTrigger(current, this.triggerConfig.MemoryNr);
				list.Add(item);
			}
			return list;
		}

		protected override string GetSetVariableName(int index)
		{
			if (this.loggerSpecifics.DataStorage.NumberOfMemories > 1u)
			{
				return string.Format("Trigger_{0}_{1:D2}", this.triggerConfig.MemoryNr, index);
			}
			return string.Format("Trigger{0:D2}", index);
		}

		protected override string GetResetVariableName(int index)
		{
			return string.Format("ResetTrigger{0:D}", index);
		}

		protected override string GetSetResetFlagName(int index)
		{
			return string.Format("", new object[0]);
		}

		protected override string GetActionItemSubSectionComment(Vector.VLConfig.Data.ConfigurationDataModel.Action action, int index)
		{
			string result = string.Format("Trigger {0:D}:", index);
			if (this.loggerSpecifics.DataStorage.NumberOfMemories > 1u)
			{
				PseudoActionTrigger pseudoActionTrigger = action as PseudoActionTrigger;
				if (pseudoActionTrigger == null)
				{
					return string.Empty;
				}
				result = string.Format("Trigger {0:D} on Memory {1:D}:", index, pseudoActionTrigger.MemoryNumber);
			}
			return result;
		}

		protected override LTLGenerator.LTLGenerationResult BuildCodeBeforeActions(out string code)
		{
			code = string.Empty;
			this.beepTriggers.Clear();
			return LTLGenerator.LTLGenerationResult.OK;
		}

		protected override string GetOptionalCodeBeforeEvent(Vector.VLConfig.Data.ConfigurationDataModel.Action action, int index)
		{
			PseudoActionTrigger pseudoActionTrigger = action as PseudoActionTrigger;
			bool flag = pseudoActionTrigger != null && pseudoActionTrigger.Trigger.TriggerEffect.Value == TriggerEffect.Single;
			if (flag)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat("VAR {0} = FREE[1] PERSISTENT", this.GetSingleTriggerVariableName(index));
				stringBuilder.AppendLine();
				return stringBuilder.ToString();
			}
			return base.GetOptionalCodeBeforeEvent(action, index);
		}

		protected override LTLGenerator.LTLGenerationResult BuildActionCode(Vector.VLConfig.Data.ConfigurationDataModel.Action action, int index, out string code)
		{
			PseudoActionTrigger pseudoActionTrigger = action as PseudoActionTrigger;
			code = "";
			if (pseudoActionTrigger == null)
			{
				return LTLGenerator.LTLGenerationResult.Error;
			}
			string text = pseudoActionTrigger.Trigger.Name.Value;
			text = text.Replace("%", "\\%");
			text = text.Replace("\"", "\\\"");
			text = text.Replace("\r", "");
			text = text.Replace("\n", "");
			if (text.Length > 200)
			{
				text = text.Substring(0, 200);
			}
			bool flag = pseudoActionTrigger.Trigger.TriggerEffect.Value == TriggerEffect.Single;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("TRIGGER_CONFIGURATION ");
			stringBuilder.AppendFormat("{0} (\"{1}\" {2:D}, 1, On) BEGIN", this.GetTCName(index), text, this.triggerConfig.MemoryNr);
			stringBuilder.AppendLine();
			stringBuilder.AppendFormat("  Level1 = ({0})", this.GetSetVariableName(index));
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("END");
			if (flag)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendFormat("EVENT ON SET ({0}) BEGIN", this.GetTCName(index));
				stringBuilder.AppendLine();
				stringBuilder.AppendFormat("  CALC {0} = (1)", this.GetSingleTriggerVariableName(index));
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("END");
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendFormat("EVENT ON SYSTEM (Logger{0:D}_Stop) BEGIN", pseudoActionTrigger.MemoryNumber);
			stringBuilder.AppendLine();
			if (pseudoActionTrigger.Trigger.TriggerEffect.Value == TriggerEffect.EndMeasurement)
			{
				foreach (int current in GlobalSettings.GetFromProjectMemoriesThatUseTriggeredLogging())
				{
					stringBuilder.AppendFormat("  CALC LockTriggerConfigs{0:D} = (1) WHEN ({1})", current, this.GetSetVariableName(index));
					stringBuilder.AppendLine();
				}
			}
			stringBuilder.AppendFormat("  CALC {0} = (0) ", this.GetSetVariableName(index));
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("END");
			code = stringBuilder.ToString();
			if (pseudoActionTrigger.Trigger.Action.Value == TriggerAction.Beep)
			{
				this.beepTriggers.Add(index);
			}
			GlobalSettings.TNE_AddTriggernameEvent(pseudoActionTrigger.Trigger.Name.Value, this.GetSetVariableName(index), pseudoActionTrigger.MemoryNumber);
			return LTLGenerator.LTLGenerationResult.OK;
		}

		protected override LTLGenerator.LTLGenerationResult BuildInactiveActionCode(Vector.VLConfig.Data.ConfigurationDataModel.Action action, int index, out string code)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("{{---- {0} deactivated ----}}", this.GetActionItemSubSectionComment(action, index));
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			code = stringBuilder.ToString();
			return LTLGenerator.LTLGenerationResult.OK;
		}

		protected override IList<string> GetBlockConditions(Vector.VLConfig.Data.ConfigurationDataModel.Action action, int index)
		{
			IList<string> list = new List<string>();
			PseudoActionTrigger pseudoActionTrigger = action as PseudoActionTrigger;
			if (pseudoActionTrigger != null)
			{
				list.Add(string.Format("Stopped{0:D}", pseudoActionTrigger.MemoryNumber));
				if (pseudoActionTrigger.Trigger.TriggerEffect.Value == TriggerEffect.Single)
				{
					list.Add(this.GetSingleTriggerVariableName(index));
				}
				if (this.isInterfaceModeUsed)
				{
					list.Add("CANoeMeasurement");
				}
			}
			return list;
		}

		protected override LTLGenerator.LTLGenerationResult BuildCodeAfterActions(out string code)
		{
			code = string.Empty;
			if (this.beepTriggers.Any<int>())
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (int current in this.beepTriggers)
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append(" OR ");
					}
					stringBuilder.AppendFormat("({0})", this.GetSetVariableName(current));
				}
				StringBuilder stringBuilder2 = new StringBuilder();
				stringBuilder2.AppendLine();
				stringBuilder2.AppendLine();
				stringBuilder2.AppendLine("{---- Action Beep on Trigger: ----}");
				stringBuilder2.AppendLine();
				stringBuilder2.AppendFormat("VAR DoBeep{0} = FREE[1]", this.triggerConfig.MemoryNr);
				stringBuilder2.AppendLine();
				stringBuilder2.AppendFormat("FLAG Beep{0} = (DoBeep{0}) SOUND (LO)", this.triggerConfig.MemoryNr);
				stringBuilder2.AppendLine();
				stringBuilder2.AppendFormat("EVENT ON SYSTEM (Logger{0}_Trigger) BEGIN", this.triggerConfig.MemoryNr);
				stringBuilder2.AppendLine();
				stringBuilder2.AppendFormat("  CALC DoBeep{0} = (1) WHEN {1}", this.triggerConfig.MemoryNr, stringBuilder);
				stringBuilder2.AppendLine();
				stringBuilder2.AppendLine("END");
				stringBuilder2.AppendFormat("EVENT ON SYSTEM (Logger{0}_Start) BEGIN", this.triggerConfig.MemoryNr);
				stringBuilder2.AppendLine();
				stringBuilder2.AppendFormat("  CALC DoBeep{0} = (0)", this.triggerConfig.MemoryNr);
				stringBuilder2.AppendLine();
				stringBuilder2.AppendLine("END");
				stringBuilder2.AppendLine();
				code += stringBuilder2.ToString();
			}
			return LTLGenerator.LTLGenerationResult.OK;
		}

		private string GetTCName(int index)
		{
			if (this.loggerSpecifics.DataStorage.NumberOfMemories > 1u)
			{
				return string.Format("TriggerConfig_{0}_{1:D2}", this.triggerConfig.MemoryNr, index);
			}
			return string.Format("TriggerConfig_{0:D2}", index);
		}

		private string GetSingleTriggerVariableName(int index)
		{
			return string.Format("Single_{0}_AlreadyTriggered", this.GetSetVariableName(index));
		}
	}
}
