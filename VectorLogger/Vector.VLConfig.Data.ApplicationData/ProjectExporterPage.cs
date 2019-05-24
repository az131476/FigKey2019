using System;

namespace Vector.VLConfig.Data.ApplicationData
{
	public sealed class ProjectExporterPage : IApplicationDataSettings
	{
		string IApplicationDataSettings.SettingName
		{
			get
			{
				return "ProjectExporterPage";
			}
		}

		public ProjectExporterParameters ProjectExporterParameters
		{
			get;
			set;
		}
	}
}
