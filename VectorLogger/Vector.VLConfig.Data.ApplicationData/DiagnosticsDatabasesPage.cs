using System;

namespace Vector.VLConfig.Data.ApplicationData
{
	public sealed class DiagnosticsDatabasesPage : IApplicationDataSettings
	{
		string IApplicationDataSettings.SettingName
		{
			get
			{
				return "DiagnosticsDatabasesPage";
			}
		}

		public string DatabasesGridLayout
		{
			get;
			set;
		}
	}
}
