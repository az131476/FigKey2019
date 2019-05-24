using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.LoggerSpecifics;

namespace Vector.VLConfig.LTLGenerationCS
{
	internal class LTLMarker : LTLGenericAction
	{
		private IList<TriggerConfiguration> activeTriggerConfigs;

		protected override string SectionHeaderComment
		{
			get
			{
				return "MARKER";
			}
		}

		public LTLMarker(IList<TriggerConfiguration> activeTriggerConfigs, ILoggerSpecifics loggerSpecifics, IApplicationDatabaseManager databaseManager, string configurationFolderPath) : base(loggerSpecifics, databaseManager, configurationFolderPath)
		{
			this.activeTriggerConfigs = activeTriggerConfigs;
		}

		protected override bool IsFeatureSupportedByLogger()
		{
			return this.loggerSpecifics.Recording.HasMarkerSupport;
		}

		protected override bool IsNoActionAvtive(out string textForComment)
		{
			textForComment = "no markers configured";
			bool result = true;
			foreach (TriggerConfiguration current in this.activeTriggerConfigs)
			{
				if (current.TriggerMode.Value == TriggerMode.Permanent && current.ActivePermanentMarkers.Count > 0)
				{
					result = false;
					break;
				}
				if (current.TriggerMode.Value == TriggerMode.OnOff && current.ActiveOnOffMarkersOnly.Count > 0)
				{
					result = false;
					break;
				}
			}
			return result;
		}

		protected override List<Vector.VLConfig.Data.ConfigurationDataModel.Action> GetActionList()
		{
			List<Vector.VLConfig.Data.ConfigurationDataModel.Action> list = new List<Vector.VLConfig.Data.ConfigurationDataModel.Action>();
			foreach (TriggerConfiguration current in this.activeTriggerConfigs)
			{
				ReadOnlyCollection<RecordTrigger> readOnlyCollection;
				if (current.TriggerMode.Value == TriggerMode.Permanent)
				{
					readOnlyCollection = current.ActivePermanentMarkers;
				}
				else
				{
					readOnlyCollection = current.ActiveOnOffMarkersOnly;
				}
				foreach (RecordTrigger current2 in readOnlyCollection)
				{
					Vector.VLConfig.Data.ConfigurationDataModel.Action item = new PseudoActionTrigger(current2, current.MemoryNr);
					list.Add(item);
				}
			}
			return list;
		}

		protected override string GetSetVariableName(int index)
		{
			return string.Format("SetMarker{0:D}", index);
		}

		protected override string GetResetVariableName(int index)
		{
			return string.Format("ResetMarker{0:D}", index);
		}

		protected override string GetSetResetFlagName(int index)
		{
			return string.Format("SetMarkerFlag", new object[0]);
		}

		protected override string GetActionItemSubSectionComment(Vector.VLConfig.Data.ConfigurationDataModel.Action action, int index)
		{
			string result = string.Format("Set Marker {0:D}:", index);
			if (this.loggerSpecifics.DataStorage.NumberOfMemories > 1u)
			{
				PseudoActionTrigger pseudoActionTrigger = action as PseudoActionTrigger;
				if (pseudoActionTrigger != null)
				{
					result = string.Format("Set Marker {0:D} on Memory {1:D}:", index, pseudoActionTrigger.MemoryNumber);
				}
			}
			return result;
		}

		protected override LTLGenerator.LTLGenerationResult BuildCodeBeforeActions(out string code)
		{
			code = string.Empty;
			return LTLGenerator.LTLGenerationResult.OK;
		}

		protected override LTLGenerator.LTLGenerationResult BuildActionCode(Vector.VLConfig.Data.ConfigurationDataModel.Action action, int index, out string code)
		{
			PseudoActionTrigger pseudoActionTrigger = action as PseudoActionTrigger;
			code = "";
			if (pseudoActionTrigger == null)
			{
				return LTLGenerator.LTLGenerationResult.Error;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("EVENT ON SET ({0}) BEGIN", this.GetSetVariableName(index));
			stringBuilder.AppendLine();
			string text = pseudoActionTrigger.Trigger.Name.Value;
			text = text.Replace("%", "\\%");
			text = text.Replace("\"", "\\\"");
			text = text.Replace("\r", "");
			text = text.Replace("\n", "");
			if (text.Length > 200)
			{
				text = text.Substring(0, 200);
			}
			stringBuilder.AppendFormat("  TRANSMIT MARKER{0:D} \"{1}\"", pseudoActionTrigger.MemoryNumber, pseudoActionTrigger.Trigger.Name.Value);
			stringBuilder.AppendLine();
			stringBuilder.AppendFormat("  CALC {0} = (0)", this.GetSetVariableName(index));
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("END");
			code = stringBuilder.ToString();
			return LTLGenerator.LTLGenerationResult.OK;
		}

		protected override LTLGenerator.LTLGenerationResult BuildInactiveActionCode(Vector.VLConfig.Data.ConfigurationDataModel.Action action, int index, out string code)
		{
			code = string.Empty;
			return LTLGenerator.LTLGenerationResult.OK;
		}

		protected override LTLGenerator.LTLGenerationResult BuildCodeAfterActions(out string code)
		{
			code = string.Empty;
			return LTLGenerator.LTLGenerationResult.OK;
		}
	}
}
