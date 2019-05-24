using System;

namespace Vector.VLConfig.Data.ApplicationData
{
	public sealed class AnalysisPackageSettings : IApplicationDataSettings
	{
		string IApplicationDataSettings.SettingName
		{
			get
			{
				return "AnalysisPackageSettings";
			}
		}

		public AnalysisPackageParameters AnalysisPackageParameters
		{
			get;
			set;
		}
	}
}
