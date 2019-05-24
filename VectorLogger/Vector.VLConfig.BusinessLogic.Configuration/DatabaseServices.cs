using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Vector.VLConfig.BusinessLogic.Conversion;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GeneralUtil.FileSystem;
using Vector.VLConfig.HardwareAccess.ToolInterfaces;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.BusinessLogic.Configuration
{
	public class DatabaseServices : IDatabaseServices
	{
		private IConfigurationManagerService configManager;

		public DatabaseServices(IConfigurationManagerService configMan)
		{
			this.configManager = configMan;
		}

		CPType IDatabaseServices.GetDatabaseCPConfiguration(Database databaseInModel, out IDictionary<string, bool> xcpEcus, out uint flexrayXcpChannel)
		{
			return this.configManager.ApplicationDatabaseManager.GetDatabaseCPConfiguration(databaseInModel, this.configManager.ConfigFolderPath, out xcpEcus, out flexrayXcpChannel);
		}

		List<string> IDatabaseServices.GetXcpSlaveNamesOfFlexRayDatabase(Database databaseInModel, string configFolderPath)
		{
			return this.configManager.ApplicationDatabaseManager.GetXcpSlaveNamesOfFlexRayDatabase(databaseInModel, configFolderPath, 0u);
		}

		bool IDatabaseServices.GetAllXcpFramesOfFlexrayXcpDatabase(Database databaseInModel, out IList<MessageDefinition> frames)
		{
			return this.configManager.ApplicationDatabaseManager.GetAllXcpFramesOfFlexrayXcpDatabase(databaseInModel, this.configManager.ConfigFolderPath, out frames);
		}

		bool IDatabaseServices.GetFibexVersionString(Database db, out string fibexVersion)
		{
			return this.configManager.ApplicationDatabaseManager.GetFibexVersionString(db, this.configManager.ConfigFolderPath, out fibexVersion);
		}

		bool IDatabaseServices.HasDatabasesConfiguredFor(BusType busType)
		{
			return this.configManager.DatabaseConfiguration.IsDatabaseAvailableFor(busType);
		}

		bool IDatabaseServices.HasDatabasesAccessibleForSignalConversion(FileConversionParameters fileConversionParameters, bool allowFlexRay)
		{
			int num;
			return ((IDatabaseServices)this).HasDatabasesAccessibleForSignalConversion(out num, fileConversionParameters, allowFlexRay);
		}

		bool IDatabaseServices.HasDatabasesAccessibleForSignalConversion(out int numOfDBsNotAccessible, FileConversionParameters fileConversionParameters, bool allowFlexRay)
		{
			IEnumerable<Database> conversionDatabases = AnalysisPackage.GetConversionDatabases(fileConversionParameters, this.configManager.DatabaseConfiguration.Databases, false);
			return this.HasDatabasesAccessibleForSignalConversion(out numOfDBsNotAccessible, allowFlexRay, conversionDatabases);
		}

		bool IDatabaseServices.HasDatabasesAccessibleFromConfiguration(bool allowFlexRay)
		{
			int num;
			return ((IDatabaseServices)this).HasDatabasesAccessibleFromConfiguration(out num, allowFlexRay);
		}

		bool IDatabaseServices.HasDatabasesAccessibleFromConfiguration(out int numOfDBsNotAccessible, bool allowFlexRay)
		{
			return this.HasDatabasesAccessibleForSignalConversion(out numOfDBsNotAccessible, allowFlexRay, this.configManager.DatabaseConfiguration.Databases);
		}

		private bool HasDatabasesAccessibleForSignalConversion(out int numOfDBsNotAccessible, bool allowFlexRay, IEnumerable<Database> databaseList)
		{
			int num = 0;
			numOfDBsNotAccessible = 0;
			foreach (Database current in databaseList)
			{
				if (current.BusType.Value == BusType.Bt_CAN || current.BusType.Value == BusType.Bt_LIN || (allowFlexRay && current.BusType.Value == BusType.Bt_FlexRay) || current.BusType.Value == BusType.Bt_Ethernet)
				{
					string absoluteFilePath = this.configManager.GetAbsoluteFilePath(current.FilePath.Value);
					if (FileProxy.Exists(absoluteFilePath))
					{
						num++;
					}
					else
					{
						numOfDBsNotAccessible++;
					}
				}
			}
			return num > 0;
		}

		IList<uint> IDatabaseServices.GetChannelAssignmentOfDatabase(string databasePath, string networkName)
		{
			List<uint> list = new List<uint>();
			foreach (Database current in this.configManager.DatabaseConfiguration.GetDatabases(databasePath, networkName))
			{
				list.Add(current.ChannelNumber.Value);
			}
			return list;
		}

		bool IDatabaseServices.IsSymbolicMessageDefined(string databasePath, string networkName, string messageName, uint channelNr, BusType busType, out MessageDefinition messageDef)
		{
			messageDef = null;
			Database database;
			MessageDefinition messageDefinition;
			if (this.configManager.DatabaseConfiguration.TryGetDatabase(databasePath, networkName, channelNr, busType, out database) && this.configManager.ApplicationDatabaseManager.ResolveMessageSymbolInDatabase(this.configManager.GetAbsoluteFilePath(databasePath), networkName, messageName, busType, out messageDefinition))
			{
				messageDef = messageDefinition;
				return true;
			}
			return false;
		}

		bool IDatabaseServices.IsSymbolicSignalDefined(string databasePath, string networkName, string messageName, string signalName, uint channelNr, BusType busType, out SignalDefinition signalDef)
		{
			signalDef = null;
			IList<Database> databases = this.configManager.DatabaseConfiguration.GetDatabases(databasePath, networkName);
			if (databases.Count == 0)
			{
				return false;
			}
			foreach (Database current in databases)
			{
				SignalDefinition signalDefinition;
				if ((current.ChannelNumber.Value == channelNr || (current.BusType.Value == BusType.Bt_FlexRay && current.ChannelNumber.Value == Database.ChannelNumber_FlexrayAB)) && this.configManager.ApplicationDatabaseManager.ResolveSignalSymbolInDatabase(this.configManager.GetAbsoluteFilePath(databasePath), networkName, messageName, signalName, out signalDefinition))
				{
					signalDef = signalDefinition;
					return true;
				}
			}
			return false;
		}

		bool IDatabaseServices.IsSignalValueTextEncoded(string databasePath, string networkName, string messageName, string signalName, double rawSignalValue, out string textSymbol)
		{
			return this.configManager.ApplicationDatabaseManager.IsSignalValueTextEncoded(this.configManager.GetAbsoluteFilePath(databasePath), networkName, messageName, signalName, rawSignalValue, out textSymbol);
		}

		bool IDatabaseServices.GetFlexrayFrameOrPDUInfo(string databasePath, string networkName, string messageName, bool isPDU, out IList<MessageDefinition> affectedFrames, out IList<string> affectedPDUs, out bool isFlexrayDbVersionGreater20)
		{
			affectedFrames = null;
			affectedPDUs = null;
			isFlexrayDbVersionGreater20 = false;
			return this.configManager.ApplicationDatabaseManager.GetFlexrayFrameOrPDUInfo(this.configManager.GetAbsoluteFilePath(databasePath), networkName, messageName, isPDU, out affectedFrames, out affectedPDUs, out isFlexrayDbVersionGreater20);
		}

		bool IDatabaseServices.RawSignalValueToPhysicalValue(string databasePath, string networkName, string messageName, string signalName, double rawValue, out double physicalValue)
		{
			return this.configManager.ApplicationDatabaseManager.RawSignalValueToPhysicalValue(this.configManager.GetAbsoluteFilePath(databasePath), networkName, messageName, signalName, rawValue, out physicalValue);
		}

		bool IDatabaseServices.PhysicalSignalValueToRawValue(string databasePath, string networkName, string messageName, string signalName, double physicalValue, out double rawValue)
		{
			return this.configManager.ApplicationDatabaseManager.PhysicalSignalValueToRawValue(databasePath, networkName, messageName, signalName, physicalValue, out rawValue);
		}

		bool IDatabaseServices.IsSymbolicMessageInsertAllowed(string messageName, string networkName, string databasePath, BusType busType, out string errorText)
		{
			errorText = string.Empty;
			if (this.configManager.LoggerSpecifics.Configuration.CompilerType == EnumCompilerType.CAPL && this.IsSymMessageContainingCaplKeyword(messageName, networkName, out errorText))
			{
				return false;
			}
			if (this.configManager.ApplicationDatabaseManager.IsMessageWithExtendedGMId(databasePath, networkName, messageName, busType))
			{
				errorText = string.Format(Resources.ErrorExtGMIdSelected, messageName);
				return false;
			}
			return true;
		}

		bool IDatabaseServices.IsSymbolicSignalInsertAllowed(string signalName, string messageName, string networkName, string databasePath, BusType busType, out string errorText)
		{
			return ((IDatabaseServices)this).IsSymbolicSignalInsertAllowed(signalName, messageName, networkName, databasePath, busType, this.configManager.LoggerSpecifics.Recording.MaximumSignalLength, out errorText);
		}

		bool IDatabaseServices.IsSymbolicSignalInsertAllowed(string signalName, string messageName, string networkName, string databasePath, BusType busType, uint maxSignalLen, out string errorText)
		{
			errorText = string.Empty;
			if (this.configManager.ApplicationDatabaseManager.IsFlexrayXCPFrame(databasePath, networkName, messageName))
			{
				errorText = Resources_CcpXcp.ErrorSignalFromFlexrayXCPFrame;
				return false;
			}
			if (this.configManager.LoggerSpecifics.Configuration.CompilerType == EnumCompilerType.CAPL && this.IsSymSignalContainingCaplKeyword(signalName, messageName, networkName, out errorText))
			{
				return false;
			}
			if (this.configManager.ApplicationDatabaseManager.IsMessageWithExtendedGMId(databasePath, networkName, messageName, busType))
			{
				errorText = string.Format(Resources.ErrorExtGMIdSignalSelected, signalName, messageName);
				return false;
			}
			SignalDefinition signalDefinition;
			if (this.configManager.ApplicationDatabaseManager.ResolveSignalSymbolInDatabase(databasePath, networkName, messageName, signalName, out signalDefinition) && signalDefinition.Length > maxSignalLen)
			{
				errorText = string.Format(Resources.ErrorSignalLenExceedsMaximum, maxSignalLen);
				return false;
			}
			if (!signalDefinition.IsIntegerType)
			{
				errorText = Resources.ErrorSignalNotSupportedDataType;
				return false;
			}
			if (signalDefinition.Factor < 0.0)
			{
				errorText = Resources.ErrorSignalHasNegativeFactor;
				return false;
			}
			return true;
		}

		bool IDatabaseServices.IsCaplKeyword(string symbolText)
		{
			if (string.IsNullOrEmpty(symbolText))
			{
				return false;
			}
			foreach (string current in CaplCompiler.CaplKeywords)
			{
				if (string.Compare(symbolText, current, true) == 0)
				{
					return true;
				}
			}
			return false;
		}

		bool IDatabaseServices.GetLinChipConfigFromLdfDatabase(uint channelNr, out int baudrate, out string protocol, out int numDatabases)
		{
			baudrate = 0;
			protocol = string.Empty;
			IEnumerable<Database> source = from db in this.configManager.DatabaseConfiguration.GetDatabases(BusType.Bt_LIN, channelNr)
			where db.FilePath.Value.ToLower(CultureInfo.InvariantCulture).EndsWith(".ldf")
			select db;
			numDatabases = source.Count<Database>();
			if (numDatabases != 1)
			{
				return false;
			}
			string absolutePath = FileSystemServices.GetAbsolutePath(source.First<Database>().FilePath.Value, this.configManager.ConfigFolderPath);
			return this.configManager.ApplicationDatabaseManager.GetLinChipConfigFromLdfDatabase(absolutePath, out baudrate, out protocol);
		}

		private bool IsSymMessageContainingCaplKeyword(string messageName, string networkName, out string errorText)
		{
			errorText = string.Empty;
			if (((IDatabaseServices)this).IsCaplKeyword(messageName))
			{
				errorText = string.Format(Resources.ErrorSymMsgIllegal, messageName);
				return true;
			}
			if (((IDatabaseServices)this).IsCaplKeyword(networkName))
			{
				errorText = string.Format(Resources.ErrorSymNetworkIllegal, networkName);
				return true;
			}
			return false;
		}

		private bool IsSymSignalContainingCaplKeyword(string signalName, string messageName, string networkName, out string errorText)
		{
			errorText = string.Empty;
			if (((IDatabaseServices)this).IsCaplKeyword(signalName))
			{
				errorText = string.Format(Resources.ErrorSymSigIllegal, signalName);
				return true;
			}
			if (((IDatabaseServices)this).IsCaplKeyword(messageName))
			{
				errorText = string.Format(Resources.ErrorSymMsgIllegal, messageName);
				return true;
			}
			if (((IDatabaseServices)this).IsCaplKeyword(networkName))
			{
				errorText = string.Format(Resources.ErrorSymNetworkIllegal, networkName);
				return true;
			}
			return false;
		}
	}
}
