using System;
using System.Collections.ObjectModel;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.DiagSymbolsAccess;

namespace Vector.VLConfig.BusinessLogic.Configuration
{
	public interface IModelEditor
	{
		bool CheckAndProcessDatabaseChannelRemapping(Database db, uint newChannelNumber);

		void ReplaceDatabaseAndUpdateSymbolicDefinitions(Database dbToReplace, string newDatabasePath);

		bool ReplaceDatabase(string filepath, Database dbToReplace);

		DSMResult ReplaceDiagnosticsDatabase(ref DiagnosticsDatabase dbToReplace, string newDatabasePath);

		bool InitDiagCommParamsEcu(DiagnosticsECU ecu);

		bool AutoCorrectDiagCommParams(ReadOnlyCollection<DiagnosticsDatabase> diagDbs, bool isReplace, out string correctionReport);

		void SetChannelConfigurationForVoCAN();

		void ProclaimCcpXcpEcuRenaming(string ecuNameToReplace, string newEcuName);

		void UpdateReferencedTriggerNameInDataTransferTriggers(ulong uniqueIdOfLoggingTrigger, string newName);
	}
}
