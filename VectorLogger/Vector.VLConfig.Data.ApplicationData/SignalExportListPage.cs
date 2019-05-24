using System;

namespace Vector.VLConfig.Data.ApplicationData
{
	public sealed class SignalExportListPage : IApplicationDataSettings
	{
		string IApplicationDataSettings.SettingName
		{
			get
			{
				return "SignalExportListPage";
			}
		}

		public string SignalExportGridLayout
		{
			get;
			set;
		}
	}
}
