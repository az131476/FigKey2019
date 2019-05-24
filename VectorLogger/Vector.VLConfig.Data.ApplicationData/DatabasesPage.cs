using System;

namespace Vector.VLConfig.Data.ApplicationData
{
	public sealed class DatabasesPage : IApplicationDataSettings
	{
		string IApplicationDataSettings.SettingName
		{
			get
			{
				return "DatabasesPage";
			}
		}

		public string DatabasesGridLayout
		{
			get;
			set;
		}
	}
}
