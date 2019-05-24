using System;
using System.Collections.Generic;
using System.Text;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.LoggerSpecifics;

namespace Vector.VLConfig.LTLGenerationCS
{
	internal class LTLActionWLAN3G : LTLGenericAction
	{
		private WLANConfiguration wlanConfig;

		protected override string SectionHeaderComment
		{
			get
			{
				return "3G";
			}
		}

		public LTLActionWLAN3G(WLANConfiguration wlanConfiguration, ILoggerSpecifics loggerSpecifics, IApplicationDatabaseManager databaseManager, string configurationFolderPath) : base(loggerSpecifics, databaseManager, configurationFolderPath)
		{
			this.wlanConfig = wlanConfiguration;
			this.usesUserComment = false;
		}

		protected override bool IsFeatureSupportedByLogger()
		{
			return this.loggerSpecifics.DataTransfer.HasWLAN || this.loggerSpecifics.DataTransfer.Has3G;
		}

		protected override bool IsNoActionAvtive(out string textForComment)
		{
			textForComment = "no send messages configured";
			return this.wlanConfig.IsStartThreeGOnEventEnabled.Value && this.wlanConfig.ThreeGDataTransferTriggerConfiguration.ActiveDataTransferTriggers.Count <= 0;
		}

		protected override List<Vector.VLConfig.Data.ConfigurationDataModel.Action> GetActionList()
		{
			List<Vector.VLConfig.Data.ConfigurationDataModel.Action> list = new List<Vector.VLConfig.Data.ConfigurationDataModel.Action>();
			foreach (Vector.VLConfig.Data.ConfigurationDataModel.Action current in this.wlanConfig.ThreeGDataTransferTriggerConfiguration.ActiveDataTransferTriggers)
			{
				list.Add(current);
			}
			return list;
		}

		protected override string GetSetVariableName(int index)
		{
			return string.Format("Connect3G_{0:D}", index);
		}

		protected override string GetActionItemSubSectionComment(Vector.VLConfig.Data.ConfigurationDataModel.Action action, int index)
		{
			return string.Format("3G connection event {0:D}:", index);
		}

		protected override LTLGenerator.LTLGenerationResult BuildCodeBeforeActions(out string code)
		{
			code = string.Empty;
			return LTLGenerator.LTLGenerationResult.OK;
		}

		protected override LTLGenerator.LTLGenerationResult BuildActionCode(Vector.VLConfig.Data.ConfigurationDataModel.Action action, int index, out string code)
		{
			code = string.Empty;
			DataTransferTrigger dataTransferTrigger = action as DataTransferTrigger;
			if (dataTransferTrigger == null)
			{
				return LTLGenerator.LTLGenerationResult.Error;
			}
			string arg;
			LTLGenerator.LTLGenerationResult dataSelectionCode = LTLWLANSettings.GetDataSelectionCode(dataTransferTrigger.IsDownloadRingbufferEnabled.Value, dataTransferTrigger.MemoriesToDownload.Value, this.loggerSpecifics, dataTransferTrigger.IsDownloadClassifEnabled.Value, dataTransferTrigger.IsDownloadDriveRecEnabled.Value, out arg);
			if (dataSelectionCode != LTLGenerator.LTLGenerationResult.OK)
			{
				return dataSelectionCode;
			}
			string arg2 = "TransferRequest";
			if (dataTransferTrigger.DataTransferMode.Value == DataTransferModeType.StopWhileDataTransfer)
			{
				arg2 = "ConnectionRequest";
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (action.Event is NextLogSessionStartEvent || action.Event is OnShutdownEvent)
			{
				stringBuilder.AppendFormat("EVENT ON SYSTEM (Shutdown) BEGIN", this.GetSetVariableName(index));
			}
			else
			{
				stringBuilder.AppendFormat("EVENT ON SET ({0}) BEGIN", this.GetSetVariableName(index));
			}
			stringBuilder.AppendLine();
			stringBuilder.AppendFormat("{0}", arg);
			stringBuilder.AppendFormat("  CALC {0} = (0)", this.GetSetVariableName(index));
			stringBuilder.AppendLine();
			if (this.loggerSpecifics.DataTransfer.HasDifferentConnectionRequestTypes)
			{
				stringBuilder.AppendFormat("  CALC SystemRequest = {0} + ConnectionRequest3G", arg2);
			}
			else
			{
				stringBuilder.AppendFormat("  CALC SystemRequest = {0}", arg2);
			}
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
