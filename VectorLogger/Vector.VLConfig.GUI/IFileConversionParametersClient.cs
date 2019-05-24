using System;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.LoggerSpecifics;

namespace Vector.VLConfig.GUI
{
	public interface IFileConversionParametersClient
	{
		EnumViewType ViewType
		{
			get;
		}

		LoggerType LoggerType
		{
			get;
		}

		FileConversionParameters FileConversionParameters
		{
			get;
			set;
		}

		bool CanConvert
		{
			get;
		}

		string ConversionTargetFolder
		{
			get;
		}
	}
}
