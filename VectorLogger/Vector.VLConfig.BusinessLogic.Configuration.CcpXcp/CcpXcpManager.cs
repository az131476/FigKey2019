using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using Vector.McModule;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.GUI;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.BusinessLogic.Configuration.CcpXcp
{
	internal class CcpXcpManager : IDatabaseAccessObserver, IUpdateObserver<DatabaseConfiguration>, IUpdateObserver<CcpXcpSignalConfiguration>, IUpdateObserver
	{
		public enum EnumValType
		{
			VtUByte,
			VtSByte,
			VtUWord,
			VtSWord,
			VtULong,
			VtSLong,
			VtFloat,
			VtDouble,
			VtUInt64,
			VtSInt64,
			VtUnknown
		}

		private static CcpXcpManager sInstance;

		private Dictionary<string, A2LDatabase> mA2LDatabases;

		private DatabaseConfiguration mDatabaseConfiguration;

		private CcpXcpSignalConfiguration mCcpXcpSignalConfiguration;

		public IUpdateService UpdateService
		{
			get;
			set;
		}

		public IConfigurationManagerService ConfigurationManagerService
		{
			get;
			set;
		}

		private IApplicationDatabaseManager ApplicationDatabaseManager
		{
			get
			{
				return this.ConfigurationManagerService.ApplicationDatabaseManager;
			}
		}

		public static CcpXcpManager Instance()
		{
			if (CcpXcpManager.sInstance == null)
			{
				CcpXcpManager.sInstance = new CcpXcpManager();
			}
			return CcpXcpManager.sInstance;
		}

		public void Init()
		{
			if (this.UpdateService != null)
			{
				this.UpdateService.AddUpdateObserver(this, UpdateContext.CcpXcpManager);
			}
			if (this.ApplicationDatabaseManager != null)
			{
				this.ApplicationDatabaseManager.Subscribe(this);
			}
		}

		void IUpdateObserver<DatabaseConfiguration>.Update(DatabaseConfiguration data)
		{
			this.mDatabaseConfiguration = data;
			this.ReleaseUnusedA2lDatabases();
		}

		void IUpdateObserver<CcpXcpSignalConfiguration>.Update(CcpXcpSignalConfiguration data)
		{
			this.mCcpXcpSignalConfiguration = data;
		}

		public bool LoadDatabases()
		{
			bool result = false;
			foreach (Database current in this.ConfigurationManagerService.DatabaseConfiguration.CCPXCPDatabases)
			{
				if (this.LoadA2LDatabase(current) == Vector.McModule.Result.kOk)
				{
					A2LDatabase a2LDatabase = CcpXcpManager.Instance().GetA2LDatabase(current);
					string text;
					if (a2LDatabase != null && a2LDatabase.CreateDeviceConfig(current, out text, false))
					{
						result = true;
					}
				}
			}
			return result;
		}

		public bool UnloadDatabases()
		{
			bool result = false;
			foreach (Database current in this.ConfigurationManagerService.DatabaseConfiguration.CCPXCPDatabases)
			{
				if (this.IsA2lDatabaseLoaded(current) && this.ReleaseA2LDatabase(current) == Vector.McModule.Result.kOk)
				{
					result = true;
				}
			}
			return result;
		}

		public void OnDatabaseChange(string absDbFilePath)
		{
			foreach (KeyValuePair<string, A2LDatabase> current in this.mA2LDatabases)
			{
				string absolutePath = FileSystemServices.GetAbsolutePath(current.Key, this.ConfigurationManagerService.ConfigFolderPath);
				if (string.Compare(absolutePath, absDbFilePath.ToLowerInvariant(), StringComparison.OrdinalIgnoreCase) == 0)
				{
					Database databaseFromPath = this.GetDatabaseFromPath(current.Key);
					if (databaseFromPath != null)
					{
						this.UpdateA2LDatabase(databaseFromPath);
					}
				}
			}
		}

		public void OnDatabaseLoaded()
		{
			if (this.UpdateService != null)
			{
				this.UpdateService.Notify<DatabaseConfiguration>(this.ConfigurationManagerService.DatabaseConfiguration);
			}
		}

		public CcpXcpManager()
		{
			this.mA2LDatabases = new Dictionary<string, A2LDatabase>();
		}

		public A2LDatabase GetA2LDatabase(string path)
		{
			A2LDatabase result;
			if (this.mA2LDatabases.TryGetValue(path.ToLowerInvariant(), out result))
			{
				return result;
			}
			return null;
		}

		public A2LDatabase GetA2LDatabase(Database database)
		{
			if (database != null)
			{
				return this.GetA2LDatabase(database.FilePath.Value);
			}
			return null;
		}

		public A2LDatabase GetA2LDatabase(CcpXcpSignal signal)
		{
			if (signal == null)
			{
				return null;
			}
			return this.GetA2LDatabase(this.GetDatabase(signal));
		}

		public bool IsA2lDatabaseLoaded(string path)
		{
			return this.mA2LDatabases.ContainsKey(path.ToLowerInvariant());
		}

		public bool IsA2lDatabaseLoaded(Database database)
		{
			return this.IsA2lDatabaseLoaded(database.FilePath.Value);
		}

		public bool IsA2lDatabaseLoaded(CcpXcpSignal signal)
		{
			Database database = this.GetDatabase(signal);
			return database != null && this.GetA2LDatabase(database) != null;
		}

		public string GetPathToA2LDatabase(A2LDatabase database)
		{
			return this.mA2LDatabases.FirstOrDefault((KeyValuePair<string, A2LDatabase> db) => db.Value == database).Key;
		}

		public Database GetDatabaseFromPath(string path)
		{
			foreach (Database current in this.ConfigurationManagerService.DatabaseConfiguration.CCPXCPDatabases)
			{
				if (string.Compare(current.FilePath.Value, path, StringComparison.OrdinalIgnoreCase) == 0)
				{
					return current;
				}
			}
			return null;
		}

		public Database GetDatabase(CcpXcpSignal signal)
		{
			foreach (Database current in this.ConfigurationManagerService.DatabaseConfiguration.CCPXCPDatabases)
			{
				if (current.CcpXcpEcuList != null && current.CcpXcpEcuList.Any<CcpXcpEcu>() && string.Compare(current.CcpXcpEcuList[0].CcpXcpEcuDisplayName, signal.EcuName.Value, StringComparison.InvariantCultureIgnoreCase) == 0)
				{
					return current;
				}
			}
			return null;
		}

		public Database GetDatabase(CcpXcpEcu ecu)
		{
			foreach (Database current in this.ConfigurationManagerService.DatabaseConfiguration.CCPXCPDatabases)
			{
				if (current.CcpXcpEcuList != null)
				{
					foreach (CcpXcpEcu current2 in current.CcpXcpEcuList)
					{
						if (current2 == ecu)
						{
							return current;
						}
					}
				}
			}
			return null;
		}

		public Vector.McModule.Result LoadA2LDatabase(Database database)
		{
			string absolutePath = FileSystemServices.GetAbsolutePath(database.FilePath.Value, this.ConfigurationManagerService.ConfigFolderPath);
			if (string.IsNullOrEmpty(database.FilePath.Value) || string.IsNullOrWhiteSpace(database.FilePath.Value))
			{
				return Vector.McModule.Result.kObjectNotFound;
			}
			if (this.mA2LDatabases.Keys.Contains(database.FilePath.Value.ToLowerInvariant()))
			{
				return Vector.McModule.Result.kObjectCreationFailed;
			}
			A2LDatabase a2LDatabase = new A2LDatabase();
			database.IsInconsistent = false;
			if (!File.Exists(absolutePath))
			{
				return Vector.McModule.Result.kObjectNotFound;
			}
			if (string.Compare(Path.GetExtension(database.FilePath.Value), "a2l", StringComparison.OrdinalIgnoreCase) == 0)
			{
				return Vector.McModule.Result.kFunctionNotImplemented;
			}
			if (database.FileType != DatabaseFileType.A2L)
			{
				return Vector.McModule.Result.kFunctionNotImplemented;
			}
			if (a2LDatabase.IsLoaded())
			{
				a2LDatabase.Release();
			}
			Vector.McModule.Result result = a2LDatabase.Create(absolutePath);
			if (result != Vector.McModule.Result.kOk)
			{
				return result;
			}
			this.mA2LDatabases.Add(database.FilePath.Value.ToLowerInvariant(), a2LDatabase);
			return Vector.McModule.Result.kOk;
		}

		public Vector.McModule.Result ReleaseA2LDatabase(Database database)
		{
			if (database.FileType != DatabaseFileType.A2L)
			{
				return Vector.McModule.Result.kFunctionNotImplemented;
			}
			database.IsInconsistent = false;
			if (!this.IsA2lDatabaseLoaded(database))
			{
				return Vector.McModule.Result.kObjectNotFound;
			}
			A2LDatabase a2LDatabase = this.GetA2LDatabase(database);
			if (this.ReleaseA2LDatabase(a2LDatabase) != Vector.McModule.Result.kOk)
			{
				return Vector.McModule.Result.kUnspecificError;
			}
			return Vector.McModule.Result.kOk;
		}

		private Vector.McModule.Result ReleaseA2LDatabase(A2LDatabase database)
		{
			if (database != null && this.mA2LDatabases.Values.Contains(database))
			{
				database.ReleaseLoggerEcu();
				database.ReleaseDeviceConfig();
				database.Release();
				string pathToA2LDatabase = this.GetPathToA2LDatabase(database);
				if (this.mA2LDatabases.ContainsKey(pathToA2LDatabase))
				{
					this.mA2LDatabases.Remove(pathToA2LDatabase);
				}
				return Vector.McModule.Result.kOk;
			}
			return Vector.McModule.Result.kParameterInvalid;
		}

		public Vector.McModule.Result UpdateA2LDatabase(Database database)
		{
			string absolutePath = FileSystemServices.GetAbsolutePath(database.FilePath.Value, this.ConfigurationManagerService.ConfigFolderPath);
			if (!this.IsA2lDatabaseLoaded(database))
			{
				return Vector.McModule.Result.kObjectNotFound;
			}
			if (!File.Exists(absolutePath))
			{
				return Vector.McModule.Result.kObjectNotFound;
			}
			if (database.FileType != DatabaseFileType.A2L)
			{
				return Vector.McModule.Result.kFunctionNotImplemented;
			}
			A2LDatabase a2LDatabase = this.GetA2LDatabase(database);
			if (a2LDatabase == null)
			{
				return Vector.McModule.Result.kUnspecificError;
			}
			database.IsInconsistent = false;
			if (a2LDatabase.IsLoaded())
			{
				a2LDatabase.ReleaseLoggerEcu();
				a2LDatabase.ReleaseDeviceConfig();
			}
			Vector.McModule.Result result = a2LDatabase.Update();
			if (result != Vector.McModule.Result.kOk)
			{
				return result;
			}
			string text;
			if (!a2LDatabase.CreateDeviceConfig(database, out text, false))
			{
				return Vector.McModule.Result.kObjectCreationFailed;
			}
			return Vector.McModule.Result.kOk;
		}

		public void ReleaseUnusedA2lDatabases()
		{
			List<A2LDatabase> list = new List<A2LDatabase>();
			using (Dictionary<string, A2LDatabase>.Enumerator enumerator = this.mA2LDatabases.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, A2LDatabase> a2lDb = enumerator.Current;
					if (!this.mDatabaseConfiguration.CCPXCPDatabases.Cast<Database>().Where(delegate(Database db)
					{
						string arg_1A_0 = db.FilePath.Value;
						KeyValuePair<string, A2LDatabase> a2lDb2 = a2lDb;
						return string.Compare(arg_1A_0, a2lDb2.Key, StringComparison.OrdinalIgnoreCase) == 0;
					}).Any<Database>())
					{
						List<A2LDatabase> arg_6F_0 = list;
						KeyValuePair<string, A2LDatabase> a2lDb3 = a2lDb;
						arg_6F_0.Add(a2lDb3.Value);
					}
				}
			}
			foreach (A2LDatabase current in list)
			{
				this.ReleaseA2LDatabase(current);
			}
		}

		public void UpdateDatabaseFilePaths(string oldConfigFolderPath, string newConfigFolderPath)
		{
			Dictionary<string, A2LDatabase> dictionary = new Dictionary<string, A2LDatabase>();
			foreach (KeyValuePair<string, A2LDatabase> current in this.mA2LDatabases)
			{
				dictionary.Add(FileSystemServices.AdjustReferencedFilePath(current.Key, oldConfigFolderPath, newConfigFolderPath).ToLowerInvariant(), current.Value);
			}
			this.mA2LDatabases = dictionary;
		}

		public Dictionary<uint, string> CreateDaqEventList(ISignal signal, Database database)
		{
			Dictionary<uint, string> dictionary = new Dictionary<uint, string>();
			A2LDatabase a2LDatabase = CcpXcpManager.Instance().GetA2LDatabase(database);
			if (a2LDatabase == null || a2LDatabase.DeviceConfig == null || signal == null)
			{
				return dictionary;
			}
			IList<IDaqEvent> daqEventListForSignal = a2LDatabase.DeviceConfig.GetDaqEventListForSignal(signal);
			foreach (IDaqEvent current in daqEventListForSignal)
			{
				IDaqEvent daqEvent;
				if (!dictionary.ContainsKey(current.Id) && a2LDatabase.DaqEvents.TryGetValue(current.Id, out daqEvent))
				{
					dictionary.Add(current.Id, daqEvent.Name);
				}
			}
			return dictionary;
		}

		public uint GetSignalDimensionAndValueType(CcpXcpSignal signal, out CcpXcpManager.EnumValType valType)
		{
			valType = CcpXcpManager.EnumValType.VtUnknown;
			Database database = this.GetDatabase(signal);
			if (database != null && database.IsCPActive.Value)
			{
				A2LDatabase a2LDatabase = this.GetA2LDatabase(database);
				if (a2LDatabase != null)
				{
					ISignal signal2 = a2LDatabase.GetSignal(signal.Name.Value);
					if (signal2 != null)
					{
						switch (signal2.ValueType)
						{
						case EnumValueType.kUByte:
							valType = CcpXcpManager.EnumValType.VtUByte;
							break;
						case EnumValueType.kSByte:
							valType = CcpXcpManager.EnumValType.VtSByte;
							break;
						case EnumValueType.kUWord:
							valType = CcpXcpManager.EnumValType.VtUWord;
							break;
						case EnumValueType.kSWord:
							valType = CcpXcpManager.EnumValType.VtSWord;
							break;
						case EnumValueType.kULong:
							valType = CcpXcpManager.EnumValType.VtULong;
							break;
						case EnumValueType.kSLong:
							valType = CcpXcpManager.EnumValType.VtSLong;
							break;
						case EnumValueType.kFloat:
							valType = CcpXcpManager.EnumValType.VtFloat;
							break;
						case EnumValueType.kDouble:
							valType = CcpXcpManager.EnumValType.VtDouble;
							break;
						case EnumValueType.kUInt64:
							valType = CcpXcpManager.EnumValType.VtUInt64;
							break;
						case EnumValueType.kSInt64:
							valType = CcpXcpManager.EnumValType.VtSInt64;
							break;
						default:
							valType = CcpXcpManager.EnumValType.VtUnknown;
							break;
						}
						return signal2.Dimension;
					}
				}
			}
			return 0u;
		}

		public SignalDefinition GetSignalDefinition(CcpXcpSignal signal)
		{
			double factor = 1.0;
			double offset = 0.0;
			bool isSigned = false;
			bool isIntegerType = false;
			bool hasLinearConversion = false;
			Database database = this.GetDatabase(signal);
			if (database != null && database.IsCPActive.Value)
			{
				A2LDatabase a2LDatabase = this.GetA2LDatabase(database);
				if (a2LDatabase != null)
				{
					ISignal signal2 = a2LDatabase.GetSignal(signal.Name.Value);
					if (signal2 != null)
					{
						uint length;
						if (signal2.BitMask != 0L && signal2.BitMask != 9223372036854775807L && signal2.BitMask != (long)((ulong)-1))
						{
							ulong num = (ulong)signal2.BitMask;
							uint num2 = 0u;
							while (num != 0uL)
							{
								if ((num & 1uL) == 1uL)
								{
									num2 += 1u;
								}
								num >>= 1;
							}
							length = num2;
						}
						else
						{
							length = signal2.ValueTypeSizeInBytes(signal2.ValueType) * 8u;
						}
						IValueConversionLinear valueConversionLinear = signal2.ValueConversion as IValueConversionLinear;
						if (valueConversionLinear != null)
						{
							factor = valueConversionLinear.Factor;
							offset = valueConversionLinear.Offset;
							hasLinearConversion = true;
						}
						if (signal2.ValueType == EnumValueType.kDouble || signal2.ValueType == EnumValueType.kFloat || signal2.ValueType == EnumValueType.kSByte || signal2.ValueType == EnumValueType.kSInt64 || signal2.ValueType == EnumValueType.kSLong || signal2.ValueType == EnumValueType.kSWord)
						{
							isSigned = true;
						}
						if (signal2.ValueType == EnumValueType.kUByte || signal2.ValueType == EnumValueType.kSByte || signal2.ValueType == EnumValueType.kUWord || signal2.ValueType == EnumValueType.kSWord || signal2.ValueType == EnumValueType.kULong || signal2.ValueType == EnumValueType.kSLong || signal2.ValueType == EnumValueType.kUInt64 || signal2.ValueType == EnumValueType.kSInt64)
						{
							isIntegerType = true;
						}
						return new SignalDefinition(length, isSigned, isIntegerType, factor, offset, hasLinearConversion);
					}
				}
			}
			return null;
		}

		public SignalDefinition GetSignalDefinition(string signalName, string ecuName)
		{
			if (string.IsNullOrEmpty(signalName.Trim()) || string.IsNullOrEmpty(ecuName.Trim()))
			{
				return null;
			}
			foreach (CcpXcpSignal current in this.mCcpXcpSignalConfiguration.ActiveSignals)
			{
				if (current.EcuName != null && string.Compare(current.EcuName.Value, ecuName, StringComparison.InvariantCultureIgnoreCase) == 0 && current.Name != null && string.Compare(current.Name.Value, signalName, StringComparison.InvariantCulture) == 0)
				{
					return this.GetSignalDefinition(current);
				}
			}
			return null;
		}

		public BusType GetBusType(string ecuName)
		{
			Database database = this.GetDatabase(ecuName);
			if (database != null)
			{
				return database.BusType.Value;
			}
			return BusType.Bt_None;
		}

		public uint GetChannelNumber(string ecuName)
		{
			Database database = this.GetDatabase(ecuName);
			if (database != null)
			{
				return database.ChannelNumber.Value;
			}
			return 0u;
		}

		public Database GetDatabase(string ecuName)
		{
			foreach (Database current in this.ConfigurationManagerService.DatabaseConfiguration.CCPXCPDatabases)
			{
				if (current.CcpXcpEcuList != null && current.CcpXcpEcuList.Any<CcpXcpEcu>() && string.Compare(current.CcpXcpEcuList[0].CcpXcpEcuDisplayName, ecuName, StringComparison.InvariantCultureIgnoreCase) == 0)
				{
					return current;
				}
			}
			return null;
		}

		public Database GetDatabase(IDatabase database)
		{
			Database result;
			try
			{
				A2LDatabase db;
				result = this.ConfigurationManagerService.DatabaseConfiguration.CCPXCPDatabases.FirstOrDefault((Database d) => (db = this.GetA2LDatabase(d)) != null && db.McDatabase == database);
			}
			catch (Exception)
			{
				result = null;
			}
			return result;
		}

		public bool IsCcpXcpTriggerSignalValid(CcpXcpSignal signal)
		{
			if (!CcpXcpManager.Instance().IsA2lDatabaseLoaded(signal))
			{
				return false;
			}
			A2LDatabase a2LDatabase = CcpXcpManager.Instance().GetA2LDatabase(signal);
			if (!a2LDatabase.IsLoaded())
			{
				return false;
			}
			if (a2LDatabase.GetSignal(signal.Name.Value) == null)
			{
				return false;
			}
			Database database = this.GetDatabase(signal);
			return database != null && database.IsCPActive.Value;
		}

		public double RawSignalValueToPhysicalValue(double rawValue, SignalDefinition sigDef)
		{
			if (sigDef != null)
			{
				return rawValue * sigDef.Factor + sigDef.Offset;
			}
			return 0.0;
		}

		public bool PhysicalSignalValueToRawValue(double physicalValue, SignalDefinition sigDef, out double rawValue)
		{
			rawValue = 0.0;
			if (sigDef == null || Math.Abs(sigDef.Factor) < 4.94065645841247E-324)
			{
				return false;
			}
			rawValue = (physicalValue - sigDef.Offset) / sigDef.Factor;
			rawValue = Math.Round(rawValue);
			if (sigDef.IsSigned)
			{
				double maximumValueForBitWidth = this.GetMaximumValueForBitWidth(sigDef.Length);
				if (rawValue < 0.0)
				{
					if (rawValue < maximumValueForBitWidth * -1.0 - 1.0)
					{
						return false;
					}
				}
				else if (rawValue > maximumValueForBitWidth)
				{
					return false;
				}
			}
			else
			{
				double maximumValueForBitWidth2 = this.GetMaximumValueForBitWidth(sigDef.Length);
				if (rawValue > maximumValueForBitWidth2 || rawValue < 0.0)
				{
					return false;
				}
			}
			return true;
		}

		public bool CheckTriggerSignalEvents()
		{
			ReadOnlyCollection<CcpXcpSignal> activeSignals = this.mCcpXcpSignalConfiguration.ActiveSignals;
			if (!activeSignals.Any<CcpXcpSignal>())
			{
				InformMessageBox.Error(Resources_CcpXcp.ErrorNoCcpXcpSignalRequests);
				return false;
			}
			List<CcpXcpSignal> list = new List<CcpXcpSignal>();
			foreach (CcpXcpSignal current in activeSignals)
			{
				if (this.IsCcpXcpTriggerSignalValid(current))
				{
					list.Add(current);
				}
			}
			if (!list.Any<CcpXcpSignal>())
			{
				InformMessageBox.Error(Resources_CcpXcp.ErrorNoValidCcpXcpSignalRequests);
				return false;
			}
			bool flag = true;
			foreach (CcpXcpSignal current2 in list)
			{
				Database database = this.GetDatabase(current2);
				if (database != null && database.BusType.Value != BusType.Bt_FlexRay)
				{
					flag = false;
					break;
				}
			}
			if (flag)
			{
				InformMessageBox.Error(Resources_CcpXcp.ErrorOnlySignalsFromFlexrayXCPFrame);
				return false;
			}
			return true;
		}

		public bool ReplaceDatabase(string filepath, Database dbToReplace)
		{
			if (dbToReplace == null || string.IsNullOrEmpty(filepath))
			{
				return false;
			}
			Database database = new Database(dbToReplace);
			FileSystemServices.TryMakeFilePathRelativeToConfiguration(this.ConfigurationManagerService.ConfigFolderPath, ref filepath);
			database.FilePath.Value = filepath;
			if (string.Compare(dbToReplace.FilePath.Value, filepath, true, CultureInfo.InvariantCulture) == 0)
			{
				return this.UpdateA2LDatabase(dbToReplace) == Vector.McModule.Result.kOk;
			}
			if (this.IsA2lDatabaseLoaded(database))
			{
				InformMessageBox.Error(Resources_CcpXcp.ErrorCcpXcpDatabaseAlreadyAdded);
				return false;
			}
			if (this.LoadA2LDatabase(database) != Vector.McModule.Result.kOk)
			{
				return false;
			}
			A2LDatabase a2LDatabase = this.GetA2LDatabase(database);
			if (a2LDatabase == null)
			{
				return false;
			}
			string text;
			if (!a2LDatabase.CreateDeviceConfig(database, out text, true))
			{
				this.ReleaseA2LDatabase(a2LDatabase);
				InformMessageBox.Error(Resources_CcpXcp.ErrorCcpXcpDatabaseInconsistency);
				return false;
			}
			this.ReleaseA2LDatabase(dbToReplace);
			this.mDatabaseConfiguration.ReplaceDatabase(dbToReplace, database);
			return true;
		}

		public double CountBytesPerSecondOfSignal(CcpXcpSignal signal)
		{
			uint num;
			double result;
			this.TryGetByteCountOfSignal(signal, out num, out result);
			return result;
		}

		public bool TryGetByteCountOfSignal(CcpXcpSignal signal, out uint bytes, out double bytesPerSecond)
		{
			bytes = 0u;
			bytesPerSecond = 0.0;
			A2LDatabase a2LDatabase = this.GetA2LDatabase(signal);
			if (a2LDatabase == null)
			{
				return false;
			}
			if (!this.TryGetByteCountOfSignal(signal, out bytes))
			{
				return false;
			}
			uint num2;
			if (signal.MeasurementMode.Value == CcpXcpMeasurementMode.DAQ)
			{
				double num;
				if ((!a2LDatabase.DaqEventFactors.TryGetValue(signal.DaqEventId.Value, out num) || num < 0.5) && this.ConfigurationManagerService.CcpXcpSignalConfiguration.CycleTimeForNonCyclicDaqEvents.Value != 0u)
				{
					num = 1000000.0 / this.ConfigurationManagerService.CcpXcpSignalConfiguration.CycleTimeForNonCyclicDaqEvents.Value;
				}
				bytesPerSecond = bytes * num;
			}
			else if (uint.TryParse(signal.PollingTime.Value, out num2) && num2 > 0u)
			{
				bytesPerSecond = bytes * 1000.0 / num2;
			}
			return true;
		}

		public bool TryGetByteCountOfSignal(CcpXcpSignal signal, out uint bytes)
		{
			bytes = 0u;
			if (signal.ByteCount != 0u)
			{
				bytes = signal.ByteCount;
				return true;
			}
			A2LDatabase a2LDatabase = this.GetA2LDatabase(signal);
			if (a2LDatabase == null)
			{
				return false;
			}
			string value = signal.Name.Value;
			ISignal signal2 = a2LDatabase.GetSignal(value);
			if (signal2 == null)
			{
				return false;
			}
			signal.SetByteCount(bytes = signal2.ValueTypeSizeInBytes(signal2.ValueType) * signal2.Dimension);
			return true;
		}

		public bool SetDatabaseDefaultsForMissingEcuSettings(ProjectRoot projectRoot, out string correctionReportCcpXcpRtf)
		{
			correctionReportCcpXcpRtf = string.Empty;
			bool flag = false;
			if (projectRoot != null && projectRoot.LoggingConfiguration != null && projectRoot.LoggingConfiguration.DatabaseConfiguration != null)
			{
				foreach (Database current in projectRoot.LoggingConfiguration.DatabaseConfiguration.CCPXCPDatabases)
				{
					if (current.CcpXcpEcuList != null && current.FileType == DatabaseFileType.A2L)
					{
						CcpXcpEcu ccpXcpEcu = current.CcpXcpEcuList.FirstOrDefault<CcpXcpEcu>();
						if (ccpXcpEcu != null && !ccpXcpEcu.UseDbParams)
						{
							IDeviceConfig dc;
							this.GetOrCreateDeviceConfig(current, out dc);
							if (current.CPType.Value == CPType.XCP)
							{
								if (string.IsNullOrEmpty(ccpXcpEcu.UseEcuTimestampValidatedProperty.Value))
								{
									ccpXcpEcu.UseEcuTimestamp = CcpXcpManager.GetUseEcuTimestampDbParamOrDefaultValue(dc, ccpXcpEcu.UseVxModule);
									flag = true;
								}
								if (string.IsNullOrEmpty(ccpXcpEcu.EcuTimestampWidthValidatedProperty.Value))
								{
									ccpXcpEcu.EcuTimestampWidth = CcpXcpManager.GetEcuTimestampWidthDbParamOrDefaultValue(dc);
									flag = true;
								}
								if (string.IsNullOrEmpty(ccpXcpEcu.EcuTimestampUnitValidatedProperty.Value))
								{
									ccpXcpEcu.EcuTimestampUnit = CcpXcpManager.GetEcuTimestampUnitDbParamOrDefaultValue(dc);
									flag = true;
								}
								if (string.IsNullOrEmpty(ccpXcpEcu.EcuTimestampTicksValidatedProperty.Value))
								{
									ccpXcpEcu.EcuTimestampTicks = CcpXcpManager.GetEcuTimestampTicksDbParamOrDefaultValue(dc);
									flag = true;
								}
							}
						}
					}
				}
				if (flag)
				{
					correctionReportCcpXcpRtf = Resources_CcpXcp.CcpXcpAdaptionsOnLoad;
					correctionReportCcpXcpRtf += Resources_CcpXcp.CcpXcpAdaptionsEcuSettings;
				}
			}
			return flag;
		}

		public bool GetCanRequestId(Database db, out uint canRequestId)
		{
			canRequestId = 0u;
			if (db == null || db.CcpXcpEcuList == null || !db.CcpXcpEcuList.Any<CcpXcpEcu>())
			{
				return false;
			}
			if (!db.CcpXcpEcuList[0].UseDbParams)
			{
				canRequestId = db.CcpXcpEcuList[0].CanRequestID;
				return true;
			}
			IDeviceConfig deviceConfig;
			if (!this.GetOrCreateDeviceConfig(db, out deviceConfig))
			{
				return false;
			}
			IXcpDeviceConfig xcpDeviceConfig = deviceConfig as IXcpDeviceConfig;
			IXcpCanConfig xcpCanConfig = null;
			if (xcpDeviceConfig != null)
			{
				xcpCanConfig = (xcpDeviceConfig.TransportConfig as IXcpCanConfig);
			}
			if (xcpCanConfig != null)
			{
				canRequestId = xcpCanConfig.IdMaster;
			}
			else
			{
				ICcpDeviceConfig ccpDeviceConfig = deviceConfig as ICcpDeviceConfig;
				if (ccpDeviceConfig == null)
				{
					return false;
				}
				canRequestId = ccpDeviceConfig.CanIdMaster;
			}
			return true;
		}

		public bool GetCanResponseId(Database db, out uint canResponseId)
		{
			canResponseId = 0u;
			if (db == null || db.CcpXcpEcuList == null || !db.CcpXcpEcuList.Any<CcpXcpEcu>())
			{
				return false;
			}
			if (!db.CcpXcpEcuList[0].UseDbParams)
			{
				canResponseId = db.CcpXcpEcuList[0].CanResponseID;
				return true;
			}
			IDeviceConfig deviceConfig;
			if (!this.GetOrCreateDeviceConfig(db, out deviceConfig))
			{
				return false;
			}
			IXcpDeviceConfig xcpDeviceConfig = deviceConfig as IXcpDeviceConfig;
			IXcpCanConfig xcpCanConfig = null;
			if (xcpDeviceConfig != null)
			{
				xcpCanConfig = (xcpDeviceConfig.TransportConfig as IXcpCanConfig);
			}
			if (xcpCanConfig != null)
			{
				canResponseId = xcpCanConfig.IdSlave;
			}
			else
			{
				ICcpDeviceConfig ccpDeviceConfig = deviceConfig as ICcpDeviceConfig;
				if (ccpDeviceConfig == null)
				{
					return false;
				}
				canResponseId = ccpDeviceConfig.CanIdSlave;
			}
			return true;
		}

		public bool GetCanFirstID(Database db, out uint canFirstID)
		{
			canFirstID = 0u;
			return db != null && db.CcpXcpEcuList != null && db.CcpXcpEcuList.Any<CcpXcpEcu>() && this.GetCanFirstID(db, out canFirstID, db.CcpXcpEcuList[0].UseDbParams);
		}

		public bool GetCanFirstID(Database db, out uint canFirstID, bool useDbOrDefaultParams)
		{
			canFirstID = 0u;
			if (useDbOrDefaultParams)
			{
				canFirstID = 268435456u;
				return true;
			}
			if (db == null || db.CcpXcpEcuList == null || !db.CcpXcpEcuList.Any<CcpXcpEcu>())
			{
				return false;
			}
			canFirstID = db.CcpXcpEcuList[0].CanFirstID;
			return true;
		}

		public bool GetHostIp(Database db, out IPAddress ipAddressHost)
		{
			byte[] address = new byte[4];
			ipAddressHost = new IPAddress(address);
			if (db == null || db.CcpXcpEcuList == null || !db.CcpXcpEcuList.Any<CcpXcpEcu>())
			{
				return false;
			}
			if (db.CcpXcpEcuList[0].UseDbParams)
			{
				IDeviceConfig deviceConfig;
				if (!this.GetOrCreateDeviceConfig(db, out deviceConfig))
				{
					return false;
				}
				IXcpDeviceConfig xcpDeviceConfig = deviceConfig as IXcpDeviceConfig;
				if (xcpDeviceConfig == null)
				{
					return false;
				}
				IXcpIpConfig xcpIpConfig = xcpDeviceConfig.TransportConfig as IXcpIpConfig;
				if (xcpIpConfig == null)
				{
					return false;
				}
				if (string.IsNullOrEmpty(xcpIpConfig.Host))
				{
					return false;
				}
				if (!IPAddress.TryParse(xcpIpConfig.Host, out ipAddressHost))
				{
					return false;
				}
			}
			else
			{
				if (db.CcpXcpEcuList[0].EthernetHost == null || string.IsNullOrEmpty(db.CcpXcpEcuList[0].EthernetHost))
				{
					return false;
				}
				if (!IPAddress.TryParse(db.CcpXcpEcuList[0].EthernetHost, out ipAddressHost))
				{
					return false;
				}
			}
			return true;
		}

		public bool GetHostPort(Database db, out uint hostPort)
		{
			hostPort = 0u;
			if (db == null || db.CcpXcpEcuList == null || !db.CcpXcpEcuList.Any<CcpXcpEcu>())
			{
				return false;
			}
			if (db.CcpXcpEcuList[0].UseDbParams)
			{
				IDeviceConfig deviceConfig;
				if (!this.GetOrCreateDeviceConfig(db, out deviceConfig))
				{
					return false;
				}
				IXcpDeviceConfig xcpDeviceConfig = deviceConfig as IXcpDeviceConfig;
				if (xcpDeviceConfig == null)
				{
					return false;
				}
				IXcpIpConfig xcpIpConfig = xcpDeviceConfig.TransportConfig as IXcpIpConfig;
				if (xcpIpConfig == null)
				{
					return false;
				}
				hostPort = (uint)xcpIpConfig.Port;
			}
			else
			{
				hostPort = db.CcpXcpEcuList[0].EthernetPort;
			}
			return true;
		}

		public bool GetHostPort2(Database db, out uint hostPort2)
		{
			hostPort2 = 0u;
			if (db == null || db.CcpXcpEcuList == null || !db.CcpXcpEcuList.Any<CcpXcpEcu>())
			{
				return false;
			}
			if (db.CcpXcpEcuList[0].UseDbParams)
			{
				hostPort2 = 5554u;
			}
			else
			{
				hostPort2 = db.CcpXcpEcuList[0].EthernetPort2;
			}
			return true;
		}

		public bool GetMaxDTO(Database db, out uint maxDTO)
		{
			maxDTO = 0u;
			if (db == null || db.CcpXcpEcuList == null || !db.CcpXcpEcuList.Any<CcpXcpEcu>())
			{
				return false;
			}
			if (db.CcpXcpEcuList[0].UseDbParams)
			{
				IDeviceConfig deviceConfig;
				if (!this.GetOrCreateDeviceConfig(db, out deviceConfig))
				{
					return false;
				}
				IXcpDeviceConfig xcpDeviceConfig = deviceConfig as IXcpDeviceConfig;
				if (xcpDeviceConfig == null)
				{
					return false;
				}
				maxDTO = xcpDeviceConfig.ProtocolConfig.MaxDto;
			}
			else
			{
				maxDTO = db.CcpXcpEcuList[0].MaxDTO;
			}
			return true;
		}

		public bool GetMaxCTO(Database db, out uint maxCTO)
		{
			maxCTO = 0u;
			if (db == null || db.CcpXcpEcuList == null || !db.CcpXcpEcuList.Any<CcpXcpEcu>())
			{
				return false;
			}
			if (db.CcpXcpEcuList[0].UseDbParams)
			{
				IDeviceConfig deviceConfig;
				if (!this.GetOrCreateDeviceConfig(db, out deviceConfig))
				{
					return false;
				}
				IXcpDeviceConfig xcpDeviceConfig = deviceConfig as IXcpDeviceConfig;
				if (xcpDeviceConfig == null)
				{
					return false;
				}
				maxCTO = xcpDeviceConfig.ProtocolConfig.MaxCto;
			}
			else
			{
				maxCTO = db.CcpXcpEcuList[0].MaxCTO;
			}
			return true;
		}

		public bool GetNodeAddress(Database db, out uint nodeAdress)
		{
			nodeAdress = 0u;
			if (db == null || db.CcpXcpEcuList == null || !db.CcpXcpEcuList.Any<CcpXcpEcu>())
			{
				return false;
			}
			if (db.CcpXcpEcuList[0].UseDbParams || string.IsNullOrEmpty(db.CcpXcpEcuList[0].FlexRayXcpNodeAdressValidatedProperty.Value))
			{
				IDeviceConfig dc;
				if (!this.GetOrCreateDeviceConfig(db, out dc))
				{
					return false;
				}
				nodeAdress = CcpXcpManager.GetNodeAddressDbParamOrDefaultValue(dc);
			}
			else
			{
				nodeAdress = db.CcpXcpEcuList[0].FlexRayXcpNodeAdress;
			}
			return true;
		}

		public static uint GetNodeAddressDbParamOrDefaultValue(IDeviceConfig dc)
		{
			IXcpDeviceConfig xcpDeviceConfig = dc as IXcpDeviceConfig;
			if (xcpDeviceConfig == null)
			{
				return 0u;
			}
			IXcpFlexRayConfig xcpFlexRayConfig = xcpDeviceConfig.TransportConfig as IXcpFlexRayConfig;
			if (xcpFlexRayConfig == null)
			{
				return 0u;
			}
			return xcpFlexRayConfig.Nax;
		}

		public bool GetSendSetSessionStatus(Database db, out bool sendSetSessionStatus)
		{
			sendSetSessionStatus = false;
			if (db == null || db.CcpXcpEcuList == null || !db.CcpXcpEcuList.Any<CcpXcpEcu>())
			{
				return false;
			}
			if (db.CcpXcpEcuList[0].UseDbParams || string.IsNullOrEmpty(db.CcpXcpEcuList[0].SendSetSessionStatusValidatedProperty.Value))
			{
				IDeviceConfig dc;
				if (!this.GetOrCreateDeviceConfig(db, out dc))
				{
					return false;
				}
				sendSetSessionStatus = CcpXcpManager.GetUseEcuTimestampDbParamOrDefaultValue(dc, db.CcpXcpEcuList[0].UseVxModule);
			}
			else
			{
				sendSetSessionStatus = db.CcpXcpEcuList[0].SendSetSessionStatus;
			}
			return true;
		}

		public static bool GetSendSetSessionStatusDbParamOrDefaultValue(IDeviceConfig dc)
		{
			ICcpDeviceConfig ccpDeviceConfig = dc as ICcpDeviceConfig;
			return ccpDeviceConfig != null && ccpDeviceConfig.IsOptionalCommandInA2L(12);
		}

		public bool GetUseEcuTimestamp(Database db, out bool useEcuTimestamp)
		{
			useEcuTimestamp = false;
			if (db == null || db.CcpXcpEcuList == null || !db.CcpXcpEcuList.Any<CcpXcpEcu>())
			{
				return false;
			}
			if (db.CcpXcpEcuList[0].UseDbParams || string.IsNullOrEmpty(db.CcpXcpEcuList[0].UseEcuTimestampValidatedProperty.Value))
			{
				IDeviceConfig dc;
				if (!this.GetOrCreateDeviceConfig(db, out dc))
				{
					return false;
				}
				useEcuTimestamp = CcpXcpManager.GetUseEcuTimestampDbParamOrDefaultValue(dc, db.CcpXcpEcuList[0].UseVxModule);
			}
			else
			{
				useEcuTimestamp = db.CcpXcpEcuList[0].UseEcuTimestamp;
			}
			return true;
		}

		public static bool GetUseEcuTimestampDbParamOrDefaultValue(IDeviceConfig dc, bool useVxModule)
		{
			if (useVxModule)
			{
				return true;
			}
			IXcpDeviceConfig xcpDeviceConfig = dc as IXcpDeviceConfig;
			return xcpDeviceConfig != null && xcpDeviceConfig.Daq != null && (xcpDeviceConfig.Daq.TimestampsSupported() && xcpDeviceConfig.Daq.TimestampSize > 0u) && (xcpDeviceConfig.TransportConfig.TransportType != EnumXcpTransportType.kCan || xcpDeviceConfig.Daq.TimestampsFixed());
		}

		public bool GetEcuTimestampWidth(Database db, out uint ecuTimeStampWidth)
		{
			ecuTimeStampWidth = 0u;
			if (db == null || db.CcpXcpEcuList == null || !db.CcpXcpEcuList.Any<CcpXcpEcu>())
			{
				return false;
			}
			if (db.CcpXcpEcuList[0].UseDbParams || string.IsNullOrEmpty(db.CcpXcpEcuList[0].EcuTimestampWidthValidatedProperty.Value))
			{
				IDeviceConfig dc;
				if (!this.GetOrCreateDeviceConfig(db, out dc))
				{
					return false;
				}
				ecuTimeStampWidth = CcpXcpManager.GetEcuTimestampWidthDbParamOrDefaultValue(dc);
			}
			else
			{
				ecuTimeStampWidth = db.CcpXcpEcuList[0].EcuTimestampWidth;
			}
			return true;
		}

		public static uint GetEcuTimestampWidthDbParamOrDefaultValue(IDeviceConfig dc)
		{
			IXcpDeviceConfig xcpDeviceConfig = dc as IXcpDeviceConfig;
			if (xcpDeviceConfig == null || xcpDeviceConfig.Daq == null)
			{
				return 4u;
			}
			return xcpDeviceConfig.Daq.TimestampSize;
		}

		public bool GetEcuTimestampUnit(Database db, out CcpXcpEcuTimestampUnit ecuTimeStampUnit)
		{
			ecuTimeStampUnit = CcpXcpEcuTimestampUnit.TU_1Milliseconds;
			if (db == null || db.CcpXcpEcuList == null || !db.CcpXcpEcuList.Any<CcpXcpEcu>())
			{
				return false;
			}
			if (db.CcpXcpEcuList[0].UseDbParams || string.IsNullOrEmpty(db.CcpXcpEcuList[0].EcuTimestampUnitValidatedProperty.Value))
			{
				IDeviceConfig dc;
				if (!this.GetOrCreateDeviceConfig(db, out dc))
				{
					return false;
				}
				ecuTimeStampUnit = CcpXcpManager.GetEcuTimestampUnitDbParamOrDefaultValue(dc);
			}
			else
			{
				ecuTimeStampUnit = db.CcpXcpEcuList[0].EcuTimestampUnit;
			}
			return true;
		}

		public static CcpXcpEcuTimestampUnit GetEcuTimestampUnitDbParamOrDefaultValue(IDeviceConfig dc)
		{
			IXcpDeviceConfig xcpDeviceConfig = dc as IXcpDeviceConfig;
			if (xcpDeviceConfig == null || xcpDeviceConfig.Daq == null)
			{
				return CcpXcpEcuTimestampUnit.TU_1Milliseconds;
			}
			return A2LDatabase.GetTimestampUnitFromSampleUnit(xcpDeviceConfig.Daq.Unit);
		}

		public bool GetEcuTimestampTicks(Database db, out uint ecuTimeStampTicks)
		{
			ecuTimeStampTicks = 0u;
			if (db == null || db.CcpXcpEcuList == null || !db.CcpXcpEcuList.Any<CcpXcpEcu>())
			{
				return false;
			}
			if (db.CcpXcpEcuList[0].UseDbParams || string.IsNullOrEmpty(db.CcpXcpEcuList[0].EcuTimestampTicksValidatedProperty.Value))
			{
				IDeviceConfig dc;
				if (!this.GetOrCreateDeviceConfig(db, out dc))
				{
					return false;
				}
				ecuTimeStampTicks = CcpXcpManager.GetEcuTimestampTicksDbParamOrDefaultValue(dc);
			}
			else
			{
				ecuTimeStampTicks = db.CcpXcpEcuList[0].EcuTimestampTicks;
			}
			return true;
		}

		public static uint GetEcuTimestampTicksDbParamOrDefaultValue(IDeviceConfig dc)
		{
			IXcpDeviceConfig xcpDeviceConfig = dc as IXcpDeviceConfig;
			if (xcpDeviceConfig == null || xcpDeviceConfig.Daq == null)
			{
				return 1u;
			}
			return xcpDeviceConfig.Daq.TimestampTicks;
		}

		private double GetMaximumValueForBitWidth(uint numberOfBits)
		{
			if (numberOfBits > 32u)
			{
				numberOfBits = 32u;
			}
			return Math.Pow(2.0, numberOfBits) - 1.0;
		}

		public bool GetOrCreateDeviceConfig(Database database, out IDeviceConfig devConfig)
		{
			devConfig = null;
			A2LDatabase a2LDatabase = this.GetA2LDatabase(database);
			if (a2LDatabase == null)
			{
				return false;
			}
			if (a2LDatabase.DeviceConfig == null)
			{
				string text;
				a2LDatabase.CreateDeviceConfig(database, out text, false);
			}
			if (a2LDatabase.DeviceConfig == null)
			{
				return false;
			}
			devConfig = a2LDatabase.DeviceConfig;
			return true;
		}
	}
}
