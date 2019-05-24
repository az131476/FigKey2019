using System;
using Vector.VLConfig.ChipCfgWrapper;
using Vector.VLConfig.GUI;

namespace Vector.VLConfig.BusinessLogic.Configuration
{
	internal class CANHardwareCallbackReceiver : ICANHardwareCallbackReceiver
	{
		private uint helpPageId;

		public CANHardwareCallbackReceiver(uint helpId)
		{
			this.helpPageId = helpId;
		}

		public void CallbackFired(int action)
		{
			if (action == 0)
			{
				MainWindow.ShowHelpForDialog(GUIUtil.HelpPageID_CANHardwareSettings);
			}
		}
	}
}
