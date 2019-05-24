using System;
using System.Collections.Generic;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.BusinessLogic.Configuration
{
	public class ConfigClipboardManager
	{
		public enum AcceptType
		{
			None,
			Action,
			Event
		}

		private static List<Vector.VLConfig.Data.ConfigurationDataModel.Action> storedActions;

		public static IConfigClipboardClient ActiveClient
		{
			get;
			set;
		}

		public static bool PasteApplicable
		{
			get
			{
				if (ConfigClipboardManager.ActiveClient == null)
				{
					return false;
				}
				if (ConfigClipboardManager.storedActions == null || ConfigClipboardManager.storedActions.Count < 1)
				{
					return false;
				}
				bool flag = false;
				foreach (Vector.VLConfig.Data.ConfigurationDataModel.Action current in ConfigClipboardManager.storedActions)
				{
					flag |= (ConfigClipboardManager.ActiveClient.Accept(current) != ConfigClipboardManager.AcceptType.None);
				}
				return flag;
			}
		}

		public static bool Copy()
		{
			if (ConfigClipboardManager.ActiveClient != null && ConfigClipboardManager.ActiveClient.FocusedActions != null && ConfigClipboardManager.ActiveClient.FocusedActions.Count > 0)
			{
				ConfigClipboardManager.storedActions = ConfigClipboardManager.CopyActions(ConfigClipboardManager.ActiveClient.FocusedActions);
				return true;
			}
			return false;
		}

		public static bool Paste()
		{
			bool flag = false;
			if (ConfigClipboardManager.ActiveClient != null && ConfigClipboardManager.storedActions != null)
			{
				foreach (Vector.VLConfig.Data.ConfigurationDataModel.Action current in ConfigClipboardManager.CopyActions(ConfigClipboardManager.storedActions))
				{
					switch (ConfigClipboardManager.ActiveClient.Accept(current))
					{
					case ConfigClipboardManager.AcceptType.Action:
						flag |= ConfigClipboardManager.ActiveClient.Insert(current);
						break;
					case ConfigClipboardManager.AcceptType.Event:
						flag |= ConfigClipboardManager.ActiveClient.Insert(current.Event);
						break;
					}
				}
			}
			return flag;
		}

		private static List<Vector.VLConfig.Data.ConfigurationDataModel.Action> CopyActions(List<Vector.VLConfig.Data.ConfigurationDataModel.Action> actions)
		{
			List<Vector.VLConfig.Data.ConfigurationDataModel.Action> list = new List<Vector.VLConfig.Data.ConfigurationDataModel.Action>();
			foreach (Vector.VLConfig.Data.ConfigurationDataModel.Action current in actions)
			{
				if (current is RecordTrigger)
				{
					list.Add(new RecordTrigger(current as RecordTrigger));
				}
				else if (current is DummyAction)
				{
					list.Add(new DummyAction(current as DummyAction));
				}
				else if (current is DataTransferTrigger)
				{
					list.Add(new DataTransferTrigger(current as DataTransferTrigger));
				}
				else if (current is ActionSendMessage)
				{
					list.Add(new ActionSendMessage(current as ActionSendMessage));
				}
				else if (current is ActionDigitalOutput)
				{
					list.Add(new ActionDigitalOutput(current as ActionDigitalOutput));
				}
			}
			return list;
		}
	}
}
