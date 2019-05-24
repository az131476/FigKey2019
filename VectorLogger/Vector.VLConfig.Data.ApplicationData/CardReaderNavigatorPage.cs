using System;

namespace Vector.VLConfig.Data.ApplicationData
{
	public sealed class CardReaderNavigatorPage : IApplicationDataSettings
	{
		string IApplicationDataSettings.SettingName
		{
			get
			{
				return "CardReaderNavigatorPage";
			}
		}

		public FileConversionParameters FileConversionParameters
		{
			get;
			set;
		}

		public string SelectedDevicePath
		{
			get;
			set;
		}

		public string NavigatorGridLogfilesLayout
		{
			get;
			set;
		}

		public string NavigatorGridMarkerLayout
		{
			get;
			set;
		}

		public string NavigatorGridMarkerSelectionLayout
		{
			get;
			set;
		}

		public string NavigatorGridMeasurementsLayout
		{
			get;
			set;
		}
	}
}
