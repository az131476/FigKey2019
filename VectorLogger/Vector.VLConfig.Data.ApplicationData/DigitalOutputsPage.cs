using System;

namespace Vector.VLConfig.Data.ApplicationData
{
	public sealed class DigitalOutputsPage : IApplicationDataSettings
	{
		string IApplicationDataSettings.SettingName
		{
			get
			{
				return "DigitalOutputsPage";
			}
		}

		public string DigitalOutputsGridLayout
		{
			get;
			set;
		}
	}
}
