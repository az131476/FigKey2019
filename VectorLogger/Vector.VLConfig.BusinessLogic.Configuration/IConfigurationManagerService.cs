using System;
using System.Collections.Generic;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.Data.DataAccess;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.DiagSymbolsAccess;
using Vector.VLConfig.GUI;
using Vector.VLConfig.LoggerSpecifics;

namespace Vector.VLConfig.BusinessLogic.Configuration
{
	public interface IConfigurationManagerService
	{
		ILoggerSpecifics LoggerSpecifics
		{
			get;
			set;
		}

		IApplicationDatabaseManager ApplicationDatabaseManager
		{
			get;
		}

		IDiagSymbolsManager DiagSymbolsManager
		{
			get;
		}

		string ConfigFolderPath
		{
			get;
		}

		IDatabaseServices DatabaseServices
		{
			get;
		}

		IFeatureRegistration FeatureRegistration
		{
			get;
		}

		IUpdateService UpdateService
		{
			get;
		}

		ProjectRoot ProjectRoot
		{
			get;
		}

		MultibusChannelConfiguration MultibusChannelConfiguration
		{
			get;
		}

		CANChannelConfiguration CANChannelConfiguration
		{
			get;
		}

		LINChannelConfiguration LINChannelConfiguration
		{
			get;
		}

		FlexrayChannelConfiguration FlexrayChannelConfiguration
		{
			get;
		}

		MOST150ChannelConfiguration MOST150ChannelConfiguration
		{
			get;
		}

		LogDataStorage LogDataStorage
		{
			get;
		}

		DatabaseConfiguration DatabaseConfiguration
		{
			get;
		}

		DiagnosticsDatabaseConfiguration DiagnosticsDatabaseConfiguration
		{
			get;
		}

		DiagnosticActionsConfiguration DiagnosticActionsConfiguration
		{
			get;
		}

		IList<FilterConfiguration> FilterConfigurations
		{
			get;
		}

		IList<TriggerConfiguration> TriggerConfigurations
		{
			get;
		}

		SendMessageConfiguration SendMessageConfiguration
		{
			get;
		}

		DigitalOutputsConfiguration DigitalOutputsConfiguration
		{
			get;
		}

		DigitalInputConfiguration DigitalInputConfiguration
		{
			get;
		}

		SpecialFeaturesConfiguration SpecialFeaturesConfiguration
		{
			get;
		}

		InterfaceModeConfiguration InterfaceModeConfiguration
		{
			get;
		}

		AnalogInputConfiguration AnalogInputConfiguration
		{
			get;
		}

		IncludeFileConfiguration IncludeFileConfiguration
		{
			get;
		}

		GPSConfiguration GPSConfiguration
		{
			get;
		}

		WLANConfiguration WLANConfiguration
		{
			get;
		}

		EthernetConfiguration EthernetConfiguration
		{
			get;
		}

		LEDConfiguration LEDConfiguration
		{
			get;
		}

		MetaInformation MetaInformation
		{
			get;
		}

		CcpXcpSignalConfiguration CcpXcpSignalConfiguration
		{
			get;
		}

		string GetFilePathRelativeToConfiguration(string absFilePath);

		string GetAbsoluteFilePath(string pathRelativeToConfiguration);

		void DataModelHasChanged(Feature changedFeature);

		void NotifyAllDependentFeatures(Feature changedFeature);

		uint GetFirstActiveOrDefaultChannel(BusType busType);

		uint GetTotalNumberOfLogicalChannels(BusType busType);

		bool IsHardwareChannelAvailable(BusType busType, uint channelNr);

		bool IsHardwareChannelActive(BusType busType, uint channelNr);

		HardwareChannel GetHardwareChannel(BusType busType, uint channelNr);

		bool IsCANChannelFDModeActive(uint canChannelNr);
	}
}
