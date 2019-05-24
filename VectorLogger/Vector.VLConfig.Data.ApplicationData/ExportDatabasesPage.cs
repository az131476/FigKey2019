using System;

namespace Vector.VLConfig.Data.ApplicationData
{
	public sealed class ExportDatabasesPage : IApplicationDataSettings
	{
		string IApplicationDataSettings.SettingName
		{
			get
			{
				return "ExportDatabasesPage";
			}
		}

		public string DatabasesGridLayout
		{
			get;
			set;
		}

		public FileConversionParameters FileConversionParameters
		{
			get;
			set;
		}
	}
}
