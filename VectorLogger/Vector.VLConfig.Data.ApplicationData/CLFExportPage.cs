using System;

namespace Vector.VLConfig.Data.ApplicationData
{
	public sealed class CLFExportPage : IApplicationDataSettings
	{
		string IApplicationDataSettings.SettingName
		{
			get
			{
				return "CLFExportPage";
			}
		}

		public FileConversionParameters FileConversionParameters
		{
			get;
			set;
		}

		public string SourceFolderPath
		{
			get;
			set;
		}

		public string CLFExportGridLayout
		{
			get;
			set;
		}
	}
}
