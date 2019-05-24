using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Vector.UtilityFunctions;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.GeneralUtil.QuickView;
using Vector.VLConfig.HardwareAccess.HardwareAbstraction;
using Vector.VLConfig.LoggerSpecifics;

namespace Vector.VLConfig.GUI.Common.QuickView
{
	internal interface IQuickViewClient
	{
		SplitButton SplitButtonQuickView
		{
			get;
		}

		ContextMenuStrip ContextMenuStripQuickView
		{
			get;
		}

		IEnumerable<Control> OtherFeatureControls
		{
			get;
		}

		ILoggerSpecifics LoggerSpecifics
		{
			get;
		}

		ILoggerDevice CurrentDevice
		{
			get;
		}

		IConfigurationManagerService ConfigurationManagerService
		{
			get;
		}

		string LogDataIniFile2
		{
			get;
		}

		DatabaseConfiguration DatabaseConfiguration
		{
			get;
		}

		string ConfigurationFolderPath
		{
			get;
		}

		IPropertyWindow PropertyWindow
		{
			get;
		}

		OfflineSourceConfig GetOfflineSourceConfig(bool isLocalContext, bool isCANoeCANalyzer);

		IEnumerable<Database> GetConversionDatabases();
	}
}
