using System;
using System.Collections.Generic;
using System.Text;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.LoggerSpecifics;

namespace Vector.VLConfig.LTLGenerationCS
{
	internal class LTLSendMessage : LTLGenericAction
	{
		private SendMessageConfiguration sendMessageConfig;

		protected override string SectionHeaderComment
		{
			get
			{
				return "SEND MESSAGE";
			}
		}

		public LTLSendMessage(SendMessageConfiguration sendMessageConfiguration, ILoggerSpecifics loggerSpecifics, IApplicationDatabaseManager databaseManager, string configurationFolderPath) : base(loggerSpecifics, databaseManager, configurationFolderPath)
		{
			this.sendMessageConfig = sendMessageConfiguration;
		}

		protected override bool IsFeatureSupportedByLogger()
		{
			return true;
		}

		protected override bool IsNoActionAvtive(out string textForComment)
		{
			textForComment = "no send messages configured";
			return this.sendMessageConfig.GetActiveActionsCount() <= 0;
		}

		protected override List<Vector.VLConfig.Data.ConfigurationDataModel.Action> GetActionList()
		{
			List<Vector.VLConfig.Data.ConfigurationDataModel.Action> list = new List<Vector.VLConfig.Data.ConfigurationDataModel.Action>();
			foreach (Vector.VLConfig.Data.ConfigurationDataModel.Action current in this.sendMessageConfig.Actions)
			{
				list.Add(current);
			}
			return list;
		}

		protected override string GetSetVariableName(int index)
		{
			return string.Format("SendMessage{0:D}", index);
		}

		protected override string GetResetVariableName(int index)
		{
			return string.Format("StopSendMessage{0:D}", index);
		}

		protected override string GetSetResetFlagName(int index)
		{
			return string.Format("SendMessageFlag", new object[0]);
		}

		protected override string GetActionItemSubSectionComment(Vector.VLConfig.Data.ConfigurationDataModel.Action action, int index)
		{
			return string.Format("Send Message {0:D}:", index);
		}

		protected override LTLGenerator.LTLGenerationResult BuildCodeBeforeActions(out string code)
		{
			code = string.Empty;
			return LTLGenerator.LTLGenerationResult.OK;
		}

		protected override LTLGenerator.LTLGenerationResult BuildActionCode(Vector.VLConfig.Data.ConfigurationDataModel.Action action, int index, out string code)
		{
			code = string.Empty;
			ActionSendMessage actionSendMessage = action as ActionSendMessage;
			if (actionSendMessage == null)
			{
				return LTLGenerator.LTLGenerationResult.Error;
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (actionSendMessage.MessageData.Count > 8)
			{
				return LTLGenerator.LTLGenerationResult.ActionsSendMessageError;
			}
			uint id;
			bool isCanExtendedId;
			if (actionSendMessage.IsSymbolic.Value)
			{
				MessageDefinition messageDefinition;
				if (!this.databaseManager.ResolveMessageSymbolInDatabase(FileSystemServices.GetAbsolutePath(actionSendMessage.DatabasePath.Value, this.configurationFolderPath), actionSendMessage.NetworkName.Value, actionSendMessage.SymbolName.Value, BusType.Bt_CAN, out messageDefinition))
				{
					return LTLGenerator.LTLGenerationResult.ActionsSendMessageError_MessageResolve;
				}
				id = messageDefinition.ActualMessageId;
				isCanExtendedId = messageDefinition.IsExtendedId;
			}
			else
			{
				id = actionSendMessage.ID.Value;
				isCanExtendedId = actionSendMessage.IsExtendedId.Value;
			}
			stringBuilder.AppendFormat("EVENT ON SET ({0}) BEGIN", this.GetSetVariableName(index));
			stringBuilder.AppendLine();
			stringBuilder.AppendFormat("  TRANSMIT {0} [{1}] LOG{2}", LTLUtil.GetIdString(BusType.Bt_CAN, actionSendMessage.ChannelNumber.Value, isCanExtendedId, id), this.GetDataBytesLTLString(actionSendMessage.MessageData), actionSendMessage.IsVirtual.Value ? " ONLY" : "");
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

		private string GetDataBytesLTLString(List<DataItem> messageData)
		{
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = true;
			foreach (DataItem current in messageData)
			{
				if (!flag)
				{
					stringBuilder.Append(" ");
				}
				stringBuilder.AppendFormat("0x{0:X2}(7:0)", current.Byte.Value);
				flag = false;
			}
			return stringBuilder.ToString();
		}
	}
}
