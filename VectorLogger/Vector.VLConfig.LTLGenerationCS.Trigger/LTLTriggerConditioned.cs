using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.Data.DataAccess;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.LTLGenerationCS.LTLEvents;
using Vector.VLConfig.LTLGenerationCS.Utility;

namespace Vector.VLConfig.LTLGenerationCS.Trigger
{
	internal class LTLTriggerConditioned : LTLTriggerModeGeneral
	{
		private StringBuilder ltlEventSection;

		private StringBuilder ltlStartStopSection;

		private IDictionary<RecordTrigger, int> onOffTriggers;

		public LTLTriggerConditioned(VLProject vlProject, ILoggerSpecifics loggerSpecifics, IApplicationDatabaseManager databaseManager, string configurationFolderPath) : base(vlProject, loggerSpecifics, databaseManager, configurationFolderPath)
		{
		}

		public LTLGenerator.LTLGenerationResult GenerateLtlCode(TriggerConfiguration triggerConfig)
		{
			LTLGenerator.LTLGenerationResult result;
			try
			{
				if (this.vlProject.ProjectRoot.LoggingConfiguration.NumberOfMemoryUnits > 1u)
				{
					base.LtlCode.Append(LTLTriggerModeGeneral.GetTriggerModeHeadlineComment(string.Format("CONDITIONED LONG-TERM LOGGING ON MEMORY {0}", triggerConfig.MemoryNr)));
				}
				else
				{
					base.LtlCode.Append(LTLTriggerModeGeneral.GetTriggerModeHeadlineComment("CONDITIONED LONG-TERM LOGGING"));
				}
				base.LtlCode.AppendFormat("VAR LoggingOn{0:D}  = FREE[1]", triggerConfig.MemoryNr);
				base.LtlCode.AppendLine();
				base.LtlCode.AppendLine();
				int num;
				this.BuildEventList(triggerConfig.OnOffTriggersOnly, triggerConfig.MemoryNr, TriggerMode.OnOff, out num);
				if (num <= 0)
				{
					base.LtlCode.AppendLine("{ no triggers configured }");
					result = LTLGenerator.LTLGenerationResult.OK;
				}
				else
				{
					this.BuildStartStopSection(triggerConfig);
					base.LtlCode.Append(this.ltlEventSection);
					base.LtlCode.AppendLine();
					base.LtlCode.Append(this.ltlStartStopSection);
					base.LtlCode.AppendLine();
					base.LtlSystemCode.AppendLine();
					base.LtlSystemCode.AppendFormat("Logger{0:D}PackDelay0  = On", triggerConfig.MemoryNr);
					base.LtlSystemCode.AppendLine();
					result = LTLGenerator.LTLGenerationResult.OK;
				}
			}
			catch (LTLGenerationException ex)
			{
				result = ex.Result;
			}
			return result;
		}

		private void BuildEventList(IList<RecordTrigger> onOffTriggerList, int memoryNr, TriggerMode triggerMode, out int numTriggers)
		{
			this.ltlEventSection = new StringBuilder();
			numTriggers = onOffTriggerList.Count<RecordTrigger>();
			if (numTriggers <= 0)
			{
				this.ltlEventSection.AppendLine("  {no triggers configured}");
				return;
			}
			this.onOffTriggers = new Dictionary<RecordTrigger, int>();
			for (int i = 1; i <= numTriggers; i++)
			{
				RecordTrigger recordTrigger = onOffTriggerList[i - 1];
				if (recordTrigger.IsActive.Value)
				{
					StringBuilder stringBuilder = new StringBuilder();
					StringBuilder stringBuilder2 = new StringBuilder();
					StringBuilder stringBuilder3 = new StringBuilder();
					StringBuilder stringBuilder4 = new StringBuilder();
					if (this.loggerSpecifics.DataStorage.NumberOfMemories > 1u)
					{
						stringBuilder.Append(LTLUtil.GetFormattedSubsectionHeaderComment(string.Format("Memory {0}: Trigger {1:D2}", memoryNr, i)));
					}
					else
					{
						stringBuilder.Append(LTLUtil.GetFormattedSubsectionHeaderComment(string.Format("Trigger {0:D2}:", i)));
					}
					stringBuilder.AppendLine();
					stringBuilder.Append("{ User comment: ");
					if (recordTrigger.Comment.Value != null)
					{
						if (recordTrigger.Comment.Value.IndexOf("\n") >= 0)
						{
							stringBuilder.AppendLine();
						}
						stringBuilder.AppendFormat("{0}", recordTrigger.Comment.Value);
					}
					stringBuilder.Append(" }");
					stringBuilder2.AppendFormat("VAR {0} = FREE[1]", this.GetTriggerVariableName(memoryNr, i));
					string value;
					this.BuildTriggerSetEvent(recordTrigger, memoryNr, i, out value);
					stringBuilder3.Append(value);
					StringBuilder stringBuilder5 = new StringBuilder();
					if (recordTrigger.TriggerEffect.Value == TriggerEffect.LoggingOn)
					{
						stringBuilder5.AppendFormat("ON CALC (LoggingOn{0:D} = (1))", memoryNr);
					}
					else
					{
						if (recordTrigger.TriggerEffect.Value != TriggerEffect.LoggingOff)
						{
							throw new LTLGenerationException(LTLGenerator.LTLGenerationResult.TriggerError_UnsupportedEvent);
						}
						stringBuilder5.AppendFormat("ON CALC (LoggingOn{0:D} = (0))", memoryNr);
					}
					stringBuilder4.AppendFormat("EVENT {0} BEGIN", stringBuilder5);
					stringBuilder4.AppendLine();
					stringBuilder4.AppendFormat("  CALC {0} = (0)", this.GetTriggerVariableName(memoryNr, i));
					stringBuilder4.AppendLine();
					stringBuilder4.AppendFormat("END", new object[0]);
					this.ltlEventSection.Append(stringBuilder);
					this.ltlEventSection.AppendLine();
					this.ltlEventSection.Append(stringBuilder2);
					this.ltlEventSection.AppendLine();
					this.ltlEventSection.Append(stringBuilder3);
					this.ltlEventSection.AppendLine();
					this.ltlEventSection.Append(stringBuilder4);
					this.ltlEventSection.AppendLine();
					this.ltlEventSection.AppendLine();
					this.onOffTriggers.Add(recordTrigger, i);
				}
			}
		}

		private void BuildTriggerSetEvent(RecordTrigger trigger, int memoryNr, int triggerNumber, out string setEventCode)
		{
			LTLGenericEventCode lTLGenericEventCode;
			if (!LTLEventFactory.GetLtlEventCodeObject(trigger.Event, this.GetTriggerVariableName(memoryNr, triggerNumber), this.databaseManager, this.configurationFolderPath, out lTLGenericEventCode))
			{
				setEventCode = "";
				throw new LTLGenerationException(LTLGenerator.LTLGenerationResult.TriggerError_UnsupportedEvent);
			}
			if (this.vlProject.ProjectRoot.HardwareConfiguration.InterfaceModeConfiguration.UseInterfaceMode.Value)
			{
				lTLGenericEventCode.AddBlockCondition("CANoeMeasurement");
			}
			LTLGenerator.LTLGenerationResult result;
			if ((result = lTLGenericEventCode.GenerateCode(out setEventCode)) != LTLGenerator.LTLGenerationResult.OK)
			{
				setEventCode = "";
				throw new LTLGenerationException(result);
			}
			if (lTLGenericEventCode.IsNeverActiveTrigger)
			{
				setEventCode = this.GetNeverActiveTriggerString(memoryNr, triggerNumber);
			}
		}

		private void BuildStartStopSection(TriggerConfiguration triggerConfig)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = triggerConfig.MemoryRingBuffer.OperatingMode.Value == RingBufferOperatingMode.stopLogging;
			stringBuilder.AppendFormat("CONST postTriggerTime{0} = {1:D}", triggerConfig.MemoryNr, triggerConfig.PostTriggerTime.Value);
			stringBuilder.AppendLine();
			stringBuilder.AppendFormat("TRIGGERINFO {0:D} [postTriggerTime{0} Year Month Day Hour Minute Second]", triggerConfig.MemoryNr);
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			stringBuilder.AppendLine(base.GetShutdownAndPowerDownDetectionCode());
			stringBuilder.AppendLine();
			if (this.vlProject.ProjectRoot.HardwareConfiguration.InterfaceModeConfiguration.UseInterfaceMode.Value)
			{
				StringBuilder stringBuilder2 = new StringBuilder();
				foreach (KeyValuePair<RecordTrigger, int> current in this.onOffTriggers)
				{
					RecordTrigger key = current.Key;
					int value = current.Value;
					if ((key.TriggerEffect.Value == TriggerEffect.LoggingOn || key.TriggerEffect.Value == TriggerEffect.LoggingOff) && (key.Event is CANDataEvent || key.Event is LINDataEvent || key.Event is SymbolicSignalEvent))
					{
						stringBuilder2.AppendFormat("  CALC {0}_LastState = (0)", this.GetTriggerVariableName(triggerConfig.MemoryNr, value));
						stringBuilder2.AppendLine();
					}
				}
				if (stringBuilder2.Length > 0)
				{
					stringBuilder.AppendLine("EVENT ON SYSTEM (PostCANoeMeasurement) BEGIN");
					stringBuilder.Append(stringBuilder2);
					stringBuilder.AppendLine("END");
					stringBuilder.AppendLine();
				}
			}
			if (triggerConfig.OnOffTriggersOnly.Count == 0)
			{
				return;
			}
			StringBuilder stringBuilder3 = new StringBuilder();
			StringBuilder stringBuilder4 = new StringBuilder();
			StringBuilder stringBuilder5 = new StringBuilder();
			OrList orList = new OrList();
			OrList orList2 = new OrList();
			foreach (KeyValuePair<RecordTrigger, int> current2 in this.onOffTriggers)
			{
				RecordTrigger key2 = current2.Key;
				int value2 = current2.Value;
				if (key2.TriggerEffect.Value == TriggerEffect.LoggingOn)
				{
					if (stringBuilder4.Length > 0)
					{
						stringBuilder4.AppendLine();
						stringBuilder4.Append("      ");
					}
					stringBuilder4.AppendFormat("ON SET ({0})", this.GetTriggerVariableName(triggerConfig.MemoryNr, value2));
					if (key2.Action.Value == TriggerAction.Beep)
					{
						orList.Add(this.GetTriggerVariableName(triggerConfig.MemoryNr, value2));
					}
				}
				else if (key2.TriggerEffect.Value == TriggerEffect.LoggingOff)
				{
					if (stringBuilder5.Length > 0)
					{
						stringBuilder5.AppendLine();
						stringBuilder5.Append("      ");
					}
					stringBuilder5.AppendFormat("ON SET ({0})", this.GetTriggerVariableName(triggerConfig.MemoryNr, value2));
					if (key2.Action.Value == TriggerAction.Beep)
					{
						orList2.Add(this.GetTriggerVariableName(triggerConfig.MemoryNr, value2));
					}
				}
			}
			bool flag2 = stringBuilder4.Length > 0;
			bool flag3 = stringBuilder5.Length > 0;
			bool flag4 = orList.Count() > 0;
			bool flag5 = orList2.Count() > 0;
			if (flag2 && flag4)
			{
				stringBuilder3.AppendFormat("VAR {0} = FREE[1]", this.GetOnBeepVariableName(triggerConfig.MemoryNr));
				stringBuilder3.AppendLine();
			}
			if (flag3 && flag5)
			{
				stringBuilder3.AppendFormat("VAR {0} = FREE[1]", this.GetOffBeepVariableName(triggerConfig.MemoryNr));
				stringBuilder3.AppendLine();
			}
			if (flag2)
			{
				stringBuilder3.AppendFormat("EVENT {0} BEGIN", stringBuilder4);
				stringBuilder3.AppendLine();
				if (flag5)
				{
					stringBuilder3.AppendFormat("  CALC {0} = (0)", this.GetOffBeepVariableName(triggerConfig.MemoryNr));
					stringBuilder3.AppendLine();
				}
				if (flag4)
				{
					stringBuilder3.AppendFormat("  CALC {0} = (1) WHEN {1}", this.GetOnBeepVariableName(triggerConfig.MemoryNr), orList.ToLTLCode());
					stringBuilder3.AppendLine();
				}
				stringBuilder3.AppendFormat("  CALC LoggingOn{0:D} = (1)", triggerConfig.MemoryNr);
				stringBuilder3.AppendLine();
				stringBuilder3.AppendFormat("END", new object[0]);
				stringBuilder3.AppendLine();
				if (flag4)
				{
					stringBuilder3.AppendFormat("FLAG BeepLoggingOn{0:D} = ({1}) SOUND (LO)", triggerConfig.MemoryNr, this.GetOnBeepVariableName(triggerConfig.MemoryNr));
					stringBuilder3.AppendLine();
					stringBuilder3.AppendLine();
				}
			}
			else
			{
				stringBuilder3.AppendLine("{no Logging On trigger configured}");
			}
			if (flag3)
			{
				stringBuilder3.AppendFormat("EVENT {0} BEGIN", stringBuilder5);
				stringBuilder3.AppendLine();
				if (flag4)
				{
					stringBuilder3.AppendFormat("  CALC {0} = (0)", this.GetOnBeepVariableName(triggerConfig.MemoryNr));
					stringBuilder3.AppendLine();
				}
				if (flag5)
				{
					stringBuilder3.AppendFormat("  CALC {0} = (1) WHEN {1}", this.GetOffBeepVariableName(triggerConfig.MemoryNr), orList2.ToLTLCode());
					stringBuilder3.AppendLine();
				}
				stringBuilder3.AppendFormat("  CALC LoggingOn{0:D} = (0)", triggerConfig.MemoryNr);
				stringBuilder3.AppendLine();
				stringBuilder3.AppendFormat("END", new object[0]);
				stringBuilder3.AppendLine();
				if (flag5)
				{
					stringBuilder3.AppendFormat("FLAG BeepLoggingOff{0:D} = ({1}) SOUND (LO 200 2 100)", triggerConfig.MemoryNr, this.GetOffBeepVariableName(triggerConfig.MemoryNr));
					stringBuilder3.AppendLine();
					stringBuilder3.AppendLine();
				}
			}
			else
			{
				stringBuilder3.AppendLine("{no Logging Off trigger configured}");
			}
			stringBuilder.Append(stringBuilder3);
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			stringBuilder.AppendFormat("STOP  {0:D} (NOT LoggingOn{0:D} DELAY=10) OR (LoggerAtEnd{0:D} DELAY=0) OR ({1} DELAY=10)", triggerConfig.MemoryNr, LTLTriggerModeGeneral.GetShutdownTriggerName());
			if (this.loggerSpecifics.DataStorage.DoCloseRingbufferAfterPowerOffInPermanentLogging)
			{
				stringBuilder.AppendFormat(" OR ({0} DELAY=2)", LTLTriggerModeGeneral.GetPowerOffTriggerName(triggerConfig.MemoryNr));
			}
			stringBuilder.AppendLine();
			AndList andList = new AndList();
			andList.Add(string.Format("LoggingOn{0:D}", triggerConfig.MemoryNr));
			andList.Add(string.Format("NOT {0}", LTLTriggerModeGeneral.GetShutdownTriggerName()));
			if (flag)
			{
				andList.Add(string.Format("NOT FlashFull{0:D}", triggerConfig.MemoryNr));
			}
			IList<string> triggerConfigLocks = base.GetTriggerConfigLocks();
			foreach (string current3 in triggerConfigLocks)
			{
				andList.Add(string.Format("NOT {0} ", current3));
			}
			stringBuilder.AppendFormat("START {0:D} {1}", triggerConfig.MemoryNr, andList.ToLTLCode());
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			this.ltlStartStopSection = stringBuilder;
		}

		private string GetTriggerVariableName(int memoryNr, int triggerIndex)
		{
			if (this.loggerSpecifics.DataStorage.NumberOfMemories > 1u)
			{
				return string.Format("Trigger_{0}_{1:D2}", memoryNr, triggerIndex);
			}
			return string.Format("Trigger{0:D2}", triggerIndex);
		}

		private string GetOnBeepVariableName(int memoryNr)
		{
			return string.Format("DoOnBeep{0:D}", memoryNr);
		}

		private string GetOffBeepVariableName(int memoryNr)
		{
			return string.Format("DoOffBeep{0:D}", memoryNr);
		}

		private string GetNeverActiveTriggerString(int memoryNr, int triggerNumber)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("CALC {0} = (0)  {{this trigger is never active!}}", this.GetTriggerVariableName(memoryNr, triggerNumber));
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			return stringBuilder.ToString();
		}
	}
}
