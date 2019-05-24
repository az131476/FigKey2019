using System;
using System.Collections.Generic;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.HardwareAccess.HardwareAbstraction;
using Vector.VLConfig.LoggerSpecifics;

namespace Vector.VLConfig.BusinessLogic.Configuration
{
	public interface ISemanticChecker
	{
		ILoggerSpecifics LoggerSpecifics
		{
			get;
		}

		bool IsConfigurationSound(GlobalOptions globalOptions, out string errorText);

		bool IsTriggerConfigurationSound(out string errorText);

		bool IsTriggerConfigurationCaplCompliant(out string errorText);

		bool IsDiagEventsCyclicTimerIntervalSound(out string errorText);

		bool IsCCPTimeoutConfigurationSound(out string errorText);

		bool IsConfigurationSoundForOnlineLogger(ILoggerDevice device, out string errorText);

		bool IsCANChannelConfigSoundForVoCANTrigger(out string errorText);

		bool IsWLANConfigSoundWithInstalledExtensionBoard(ILoggerDevice device, out string errorText);

		bool IsAnalogInputConfigSoundWithInstalledExtensionBoard(ILoggerDevice device, out string errorText);

		bool HasMultipleMsgIDsInDBsOnSameChannel(FileConversionParameters conversionParameters, out string msgText);

		bool IsFlexrayDbAddable(ref List<uint> freeChannelNumbers);

		bool IsDiagDescriptionContainingOemSpecificEcus(DiagnosticsDatabase db, string requiredOemName, DiagnosticsProtocolType requiredProtType);

		bool IsEthernetDbAddable();
	}
}
