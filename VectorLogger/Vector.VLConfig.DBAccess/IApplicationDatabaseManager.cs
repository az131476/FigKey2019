using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Vector.McModule;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.DBAccess
{
	public interface IApplicationDatabaseManager
	{
		event EventHandler DatabaseFileChanged;

		bool IsHexDisplay
		{
			get;
			set;
		}

		string SymSelHelpFileName
		{
			get;
			set;
		}

		void UpdateApplicationDatabaseConfiguration(DatabaseConfiguration databaseConfiguration, MultibusChannelConfiguration multibusChannelConfiguration, CANChannelConfiguration canChannelConfiguration, string configFolderPath);

		DatabaseOpenStatus GetDatabaseOpenStatus(Database databaseInModel, string configFolderPath);

		void CloseAllDatabases();

		bool SelectMessageInDatabase(ref string messageName, ref string databaseName, ref string databasePath, ref string networkName, ref BusType busType, ref bool isFlexrayPDU);

		bool SelectMessageInDatabase(BusType busType, ref string[] messageNameList, ref string[] databaseNameList, ref string[] databasePathList, ref string[] networkNameList, ref BusType[] busTypeList, ref bool[] isFlexrayPDUList, bool allowMultiSelection);

		bool SelectSignalInDatabase(ref string messageName, ref string signalName, ref string databaseName, ref string databasePath, ref string networkName, ref BusType busType, ref bool isFlexRayPDU);

		bool ResolveMessageSymbolInDatabase(string databasePath, string networkName, string messageSymbol, BusType busType, out MessageDefinition messageId);

		bool ResolveSignalSymbolInDatabase(string databasePath, string networkName, string messageSymbol, string signalSymbol, out SignalDefinition signalDefinition);

		bool GetTextEncodedSignalValueTable(string databasePath, string networkName, string messageSymbol, string signalSymbol, out Dictionary<double, string> textEncValueTable);

		bool IsSignalValueTextEncoded(string databasePath, string networkName, string messageSymbol, string signalSymbol, double rawSignalValue, out string textSymbol);

		bool GetDuplicateMsgIDsOfConfiguredDatabases(IList<Database> databaseList, string configFolderPath, BusType busType, out IDictionary<uint, Dictionary<string, string>> symbolicNamesInDatabasesForIDs);

		bool PhysicalSignalValueToRawValue(string databasePath, string networkName, string messageSymbol, string signalSymbol, double physicalValue, out double rawValue);

		bool RawSignalValueToPhysicalValue(string databasePath, string networkName, string messageSymbol, string signalSymbol, double rawValue, out double physicalValue);

		bool FlexRayDatabasesContainXcpSlave(string ecuName, string configFolderPath, uint channelNumber = 0u);

		List<string> GetXcpSlaveNamesOfFlexRayDatabase(Database databaseInModel, string configFolderPath, uint channelNumber = 0u);

		CPType GetDatabaseCPConfiguration(Database databaseInModel, string configFolderPath, out IDictionary<string, bool> xcpEcus, out uint flexrayXcpChannel);

		bool GetShortestCPTimeout(Database databaseInModel, string configFolderPath, out uint minCPTimeout_ms);

		bool SetRuntimeInformationInCcpXcpEvents(IEnumerable<Event> activeEvents, DatabaseConfiguration databaseConfiguration);

		bool GetAllMessageIdsWithNames(Database databaseInModel, string configFolderPath, out IDictionary<uint, string> idsAndNames);

		bool FindMessagesForSignalName(DatabaseConfiguration databaseConfiguration, string configFolderPath, string signalName, BusType busType, uint channelNumber, out IDictionary<string, MessageDefinition> messages);

		bool IsGMLANDatabase(string absoluteFilePath);

		bool IsMessageWithExtendedGMId(string databasePath, string networkName, string messageSymbol, BusType busType);

		bool IsFlexrayDatabase(string absoluteFilePath, out IDictionary<string, bool> flexrayNetworkNamesIsDualChannel);

		bool IsFlexrayXCPFrame(string flexrayDbPath, string networkName, string messageSymbol);

		bool GetFibexVersionString(Database db, string configFolderPath, out string fibexVersion);

		bool GetFlexrayFrameOrPDUInfo(string databasePath, string networkName, string pduOrFrameName, bool isPDU, out IList<MessageDefinition> affectedFrames, out IList<string> affectedPDUs, out bool isFlexrayDBVersionGreater20);

		bool MapFlexraySignalsFromPduToFrames(string databasePath, string networkName, string pduOrFrameName, string signalName, out IList<SignalDefinition> signalList);

		bool GetAllXcpFramesOfFlexrayXcpDatabase(Database databaseInModel, string configFolderPath, out IList<MessageDefinition> frames);

		List<Tuple<Database, uint>> GetFlexRayDatabasesSuitedForXcp(uint channelNumber, ReadOnlyCollection<Database> flexrayDatabases, string configFolderPath);

		bool ProvideFlexRayFibexDataForXcp(IFibexFlexRayDataProvider provider, Database databaseInModel, string configFolderPath);

		bool IsAutosarDescriptionFile(string absFilePath, out IDictionary<string, BusType> networks);

		bool IsCanFdDatabase(string absoluteFilePath, string networkName);

		void Subscribe(IDatabaseAccessObserver observer);

		void Unsubscribe(IDatabaseAccessObserver observer);

		bool ResolveGPSMessageSymbolsInDatabase(string databasePath, out uint[] canIDs, out int modeLatitudeLongitude, out double factorAltitude);

		bool IsEthernetDatabase(string absoluteFilePath, out string networkName);

		bool GetLinChipConfigFromLdfDatabase(string absDatabasePath, out int baudrate);

		bool GetLinChipConfigFromLdfDatabase(string absDatabasePath, out int baudrate, out string protocol);

		void UpdateApplicationExportDatabaseConfiguration(ExportDatabaseConfiguration exportDatabaseConfiguration, string configFolderPath);

		void AddExportDatabaseConfiguration(ExportDatabaseConfiguration exportDatabaseConfiguration);
	}
}
