using System;
using System.Collections.Generic;
using Vector.DiagSymbols;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.DiagSymbolsAccess
{
	public interface IDiagSymbolsManager
	{
		DSMResult GetAllEcuQualifiersOfDatabaseFile(string absDatabasePath, out IList<string> ecuQualifiers);

		void CloseAllDatabases();

		void UpdateDatabaseConfiguration(DiagnosticsDatabaseConfiguration config, DiagnosticActionsConfiguration actionsConfig, string configFolderPath);

		void UpdateDatebaseEcuList(string absDatabasePath, uint channelNumberForNewEcus, IList<string> selectedEcuQualifiers, ref DiagnosticsDatabase db);

		DSMResult LoadDiagnosticsDatabase(DiagnosticActionsConfiguration config, string absDatabasePath, uint channelNumber, IList<string> ecusToLoad, out DiagnosticsDatabase diagDatabase);

		bool RemoveDiagnosticsDatabase(string absDatabasePath);

		bool GetEcusInDiagnosticsDatabaseFromOEMHeuristic(string absDatabasePath, string substringToBeFound, out IList<string> ecuQualifiers);

		bool GetVariantQualifiers(string absDatabasePath, string ecuQualifier, out IList<string> variantQualifiers);

		bool GetDids(string ecuQualifier, string variantQualifier, out IList<IDsDid> dids);

		bool ResolveEcuQualifier(string absDatabasePath, string ecuQualifier);

		bool ResolveEcuVariant(string absDatabasePath, string ecuQualifier, string variantQualifier);

		bool ResolveService(string absDatabasePath, string ecuQualifier, string variantQualifier, string service, out bool hasOnlyConstParams);

		bool ResolveSignal(string absDatabasePath, string ecuQualifier, string variantQualifier, string didId, string signal);

		bool EditServiceParameter(string ecuQualifier, string variantQualifier, string serviceQualifier, ref byte[] messageData, ref DiagSessionType session);

		bool InitDefaultServiceParameter(string ecuQualifier, string variantQualifier, string serviceQualifier, out byte[] messageData, out DiagSessionType session);

		bool GetDisassembledMessageParams(string absDatabasePath, string ecuQualifier, string variantQualifier, string serviceQualifier, byte[] messageData, out IList<KeyValuePair<string, string>> paramsAndValues);

		bool GetDisassembledMessageParams(IDsGenericService service, byte[] messageData, out IList<KeyValuePair<string, string>> paramsAndValues);

		DSMResult GetServiceComplexityStatus(string ecuQualifier, string variantQualifier, string serviceQualifier, out bool hasOnlyConstantParams);

		DiagSessionType GetServiceSessionTypeFromDB(string ecuQualifier, string variantQualifier, string serviceQualifier, byte[] messageData);

		bool GetValidCommInterfaceQualifiers(string ecuQualifier, out IList<string> commInterfaceQualifiers);

		bool GetAllCommInterfaceQualifiers(string ecuQualifier, out IList<string> validCommInterfaceQualifiers, out IList<string> invalidCommInterfaceQualifiers);

		bool ResolveCommInterfaceQualifier(string ecuQualifier, string interfaceQualifier);

		bool GetTesterPresentMessage(string ecuQualifier, string variantQualifier, ref DiagnosticCommParamsECU ecuCommParams);

		bool GetCommInterfaceParamValuesFromDb(string ecuQualifier, string commInterfaceQualifier, ref DiagnosticCommParamsECU ecuCommParams);

		bool GetCommInterfaceName(string ecuQualifier, string commInterfaceQualifier, out string commInterfaceName);

		bool GetSessionControlParamValuesFromDb(string ecuQualifier, string variantQualifier, ref DiagnosticCommParamsECU ecuCommParams);

		bool GetAvailableSessions(string ecuQualifier, string variantQualifier, out Dictionary<string, ulong> sessions);

		bool GetSessionNameForQualifier(string ecuQualifier, string variantQualifier, string sessionQualifier, out string sessionName);

		bool GetDtcLength(string absDatabasePath, string ecuQualifier, string variantQualifier, out uint dtclength);

		bool GetSignalDefinition(string ecuQualifier, string variantQualifier, string didId, string signalName, out SignalDefinition signalDefinition);

		bool SelectDiagnosticServiceAction(bool isMultiSelectEnabled, out List<DiagnosticAction> actions);

		bool SelectDiagnosticSignal(out List<string> databasePaths, out List<string> ecuQualifiers, out List<string> serviceQualifiers, out List<string> parameterAddresses);

		bool SelectDiagnosticsTroubleCode(out List<string> databasePaths, out List<string> ecuQualifiers, out List<string> troubleCodes);
	}
}
