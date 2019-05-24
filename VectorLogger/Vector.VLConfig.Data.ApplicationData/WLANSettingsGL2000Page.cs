using System;

namespace Vector.VLConfig.Data.ApplicationData
{
	public sealed class WLANSettingsGL2000Page : IApplicationDataSettings
	{
		string IApplicationDataSettings.SettingName
		{
			get
			{
				return "WLANSettingsGL2000Page";
			}
		}

		public string ThreeGDataTransferTriggerGridLayout
		{
			get;
			set;
		}
	}
}
