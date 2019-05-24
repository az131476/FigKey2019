using System;
using System.Collections.Generic;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.BusinessLogic.Configuration
{
	public interface IDatabaseServices
	{
		CPType GetDatabaseCPConfiguration(Database databaseInModel, out IDictionary<string, bool> xcpEcus, out uint flexrayXcpChannel);

		List<string> GetXcpSlaveNamesOfFlexRayDatabase(Database databaseInModel, string configFolderPath);

		bool GetAllXcpFramesOfFlexrayXcpDatabase(Database databaseInModel, out IList<MessageDefinition> frames);

		bool GetFibexVersionString(Database db, out string fibexVersion);

		bool HasDatabasesConfiguredFor(BusType busType);

		bool HasDatabasesAccessibleForSignalConversion(FileConversionParameters fileConversionParameters, bool allowFlexRay);

		bool HasDatabasesAccessibleForSignalConversion(out int numOfDBsNotAccessible, FileConversionParameters fileConversionParameters, bool allowFlexRay);

		bool HasDatabasesAccessibleFromConfiguration(bool allowFlexRay);

		bool HasDatabasesAccessibleFromConfiguration(out int numOfDBsNotAccessible, bool allowFlexRay);

		IList<uint> GetChannelAssignmentOfDatabase(string databasePath, string networkName);

		bool IsSymbolicMessageDefined(string databasePath, string networkName, string messageName, uint channelNr, BusType busType, out MessageDefinition messageDef);

		bool IsSymbolicSignalDefined(string databasePath, string networkName, string messageName, string signalName, uint channelNr, BusType busType, out SignalDefinition signalDef);

		bool IsSignalValueTextEncoded(string databasePath, string networkName, string messageName, string signalName, double rawSignalValue, out string textSymbol);

		bool GetFlexrayFrameOrPDUInfo(string databasePath, string networkName, string messageName, bool isPDU, out IList<MessageDefinition> affectedFrames, out IList<string> affectedPDUs, out bool isFlexrayDbVersionGreater20);

		bool RawSignalValueToPhysicalValue(string databasePath, string networkName, string messageName, string signalName, double rawValue, out double physicalValue);

		bool PhysicalSignalValueToRawValue(string databasePath, string networkName, string messageName, string signalName, double physicalValue, out double rawValue);

		bool IsSymbolicMessageInsertAllowed(string messageName, string networkName, string databasePath, BusType busType, out string errorText);

		bool IsSymbolicSignalInsertAllowed(string signalName, string messageName, string networkName, string databasePath, BusType busType, out string errorText);

		bool IsSymbolicSignalInsertAllowed(string signalName, string messageName, string networkName, string databasePath, BusType busType, uint maxSignalLen, out string errorText);

		bool IsCaplKeyword(string symbolText);

		bool GetLinChipConfigFromLdfDatabase(uint channelNr, out int baudrate, out string protocol, out int numDatabases);
	}
}
