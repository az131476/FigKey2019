using System;

namespace Vector.VLConfig.Data.ApplicationData
{
	public sealed class WLANSettingsGL3PlusPage : IApplicationDataSettings
	{
		string IApplicationDataSettings.SettingName
		{
			get
			{
				return "WLANSettingsGL3PlusPage";
			}
		}

		public string ThreeGDataTransferTriggerGridLayout
		{
			get;
			set;
		}
	}
}
