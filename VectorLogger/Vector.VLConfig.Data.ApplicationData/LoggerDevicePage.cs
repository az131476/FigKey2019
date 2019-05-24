using System;

namespace Vector.VLConfig.Data.ApplicationData
{
	public sealed class LoggerDevicePage : IApplicationDataSettings
	{
		string IApplicationDataSettings.SettingName
		{
			get
			{
				return "LoggerDevicePage";
			}
		}

		public FileConversionParameters FileConversionParameters
		{
			get;
			set;
		}

		public string LoggerDeviceGridLayout
		{
			get;
			set;
		}
	}
}
