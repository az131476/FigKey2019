using System;

namespace Vector.VLConfig.Data.ApplicationData
{
	public sealed class CardReaderPage : IApplicationDataSettings
	{
		string IApplicationDataSettings.SettingName
		{
			get
			{
				return "CardReaderPage";
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

		public string CardReaderGridLayout
		{
			get;
			set;
		}
	}
}
