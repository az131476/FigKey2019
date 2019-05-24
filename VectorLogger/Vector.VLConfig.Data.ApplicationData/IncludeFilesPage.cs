using System;

namespace Vector.VLConfig.Data.ApplicationData
{
	public sealed class IncludeFilesPage : IApplicationDataSettings
	{
		string IApplicationDataSettings.SettingName
		{
			get
			{
				return "IncludeFilesPage";
			}
		}

		public string IncludeFilesGridLayout
		{
			get;
			set;
		}
	}
}
