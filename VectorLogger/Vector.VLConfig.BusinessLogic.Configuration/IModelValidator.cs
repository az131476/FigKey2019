using System;
using System.Collections.Generic;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.GUI;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.ValidationFramework;

namespace Vector.VLConfig.BusinessLogic.Configuration
{
	public interface IModelValidator
	{
		ILoggerSpecifics LoggerSpecifics
		{
			get;
			set;
		}

		IDatabaseServices DatabaseServices
		{
			get;
		}

		IList<int> GetActiveMemoryNumbers
		{
			get;
		}

		string GetFilePathRelativeToConfiguration(string absFilePath);

		string GetAbsoluteFilePath(string pathRelativeToConfiguration);

		uint GetFirstActiveOrDefaultChannel(BusType busType);

		uint GetTotalNumberOfLogicalChannels(BusType busType);

		bool IsHardwareChannelAvailable(BusType busType, uint channelNr);

		bool IsHardwareChannelActive(BusType busType, uint channelNr);

		HardwareChannel GetHardwareChannel(BusType busType, uint channelNr);

		bool IsCANChannelFDModeActive(uint canChannelNr);

		bool IsActiveVoCANEventConfigured(out int numberOfConfiguredActiveVoCANEvents, out bool isLed8Used);

		bool IsActiveCasKeyEventConfigured(out int numberOfActiveCasKeyEvents);

		bool Validate(Feature feature, bool isDataChanged, IModelValidationResultCollector resultCollector);

		bool Validate(Feature feature, PageType pageContext, bool isDataChanged, IModelValidationResultCollector resultCollector);

		bool IsPrescalerValueOfCANChannelValid(uint channelNr, CANChannelConfiguration canChannelConfig);

		uint GetMaxPrescalerValue(uint channelNr);

		uint GetEventActivationDelayAfterStart();

		bool ValidateCcpXcpDatabaseSettings(Database database, EthernetConfiguration ethernetConfiguration, IModelValidationResultCollector resultCollector);

		bool ValidateCcpXcpEcuIdSettings(Database database, IModelValidationResultCollector resultCollector);

		bool ValidateDiagnosticActionsGeneralSettings(DiagnosticActionsConfiguration actionsConfig, bool isDataChanged, IModelValidationResultCollector resultCollector);

		bool ValidateIndependentDiagCommParamsEcu(DiagnosticCommParamsECU commParamsEcu, IModelValidationResultCollector resultCollector);

		bool ValidateOverallCommParamsSessionSettings(IDiagnosticsDatabaseConfiguration config, IModelValidationResultCollector resultCollector, bool useSingleErrorPerEcu);

		bool ValidateOverallCommParamsRequestResponseIds(IDiagnosticsDatabaseConfiguration config, IModelValidationResultCollector resultCollector, bool useSingleErrorPerEcu);

		List<string> GetUndefinedEcuQualifiersOfDiagDatabase(DiagnosticsDatabase database);

		bool ValidateDatabaseFileExistence(DiagnosticsDatabase db, IModelValidationResultCollector resultCollector);

		bool ValidateDatabaseConsistency(Database db, IModelValidationResultCollector resultCollector);

		bool HasDiagnosticsDatabasesConfigured();

		bool HasChannelAssignedForDiagnosticsECU(string descFilePath, string ecuQualifier, out uint channelNr);

		bool IsDiagECUConfigured(string descFilePath, string ecuQualifier, out string currentVariantNameOfEcu);

		bool ValidateRingBufferSettings(TriggerConfiguration triggerConfiguration, bool isDataChanged, IModelValidationResultCollector resultCollector);

		bool ValidatePostTriggerTime(TriggerConfiguration triggerConfiguration, bool isDataChanged, IModelValidationResultCollector resultCollector);

		bool ValidateSymbolicMessageChannelFromFlexrayDb(string databasePath, string networkName, string messageName, uint messageChannel);

		string ReplaceInvalidMarkerNameCharactersIfPossible(string str);

		IDictionary<ulong, string> GetUniqueIdAndNameOfActiveRecordTriggers();

		IList<ulong> GetUniqueIdsForNameOfActiveRecordTrigger(string triggerName);

		IList<string> GetSortedNamesOfActiveRecordTriggers();

		bool IsCommonDigitalInputOutputPinUsedAsInput(int outputNr);

		bool ValidateThreeGDataTransferTriggers(DataTransferTriggerConfiguration threeGDataTransferTriggerConfig, bool isDataChanged, IModelValidationResultCollector resultCollector);

		bool IsLedDisabled(uint ledNumber);

		bool ValidateEvent(Event ev, IModelValidationResultCollector resultCollector);
	}
}
