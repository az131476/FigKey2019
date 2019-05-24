using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using Vector.CANalyzer;
using Vector.COMdbLib;
using Vector.McModule;
using Vector.SymbolSelection;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.GeneralUtil;

namespace Vector.VLConfig.DBAccess
{
	public class ApplicationDatabaseManager : IApplicationDatabaseManager, IDisposable
	{
		private DatabaseConfiguration mDatabaseConfiguration;

		private IDBManager dbManager;

		private List<ExportDatabaseConfiguration> mExportDatabaseConfigurationList;

		private SymbolSelectionDialog symbSel;

		private string helpFileName;

		private string mConfigFolderPath;

		private bool isHexDisplay;

		private Dictionary<string, Vector.COMdbLib.IDatabase> uniqueNameToCOMdbLibDatabase;

		private List<IDatabaseAccessObserver> mDatabaseAccessObservers = new List<IDatabaseAccessObserver>();

		private Dictionary<FileSystemWatcher, string> fileWatcherToDbPath;

		private Dictionary<string, bool> dbPathToIsDbChanged;

		private Dictionary<string, HashSet<string>> dbPathToNetworkNames;

		private EventHandler databaseFileChanged;

		private static readonly string AttributeGenMsgCycleTime = "GenMsgCycleTime";

		private static readonly string AttributeGenMsgSendType = "GenMsgSendType";

		private static readonly string AttributeGenSigSendType = "GenSigSendType";

		private static readonly string AttributeBusType = "BusType";

		private static readonly string AttributeBusTypeValue_CANFD = "CAN FD";

		private static readonly string AttributeCCPVersion = "CCP_VERSION";

		private static readonly string AttributeXCPRevision = "XCP_REVISION";

		private static readonly string AttributeCCPTimeout = "CCP_TIMEOUT";

		private static readonly string AttributeCCPResourceProtectionStatus = "CCP_RESOURCE_PROTECTION_STATUS";

		private static readonly string AttributeXCPResourceProtectionStatus = "XCP_RESOURCE_PROTECTION_STATUS";

		private static readonly int ProtectionStatusCCPMask = 2;

		private static readonly int ProtectionStatusXCPMask = 4;

		private static readonly string AttributeUseGMParamIDs = "UseGMParameterIDs";

		private static readonly string AttributeMsgPrio = "Prio";

		private static readonly string NodeNameXCPMaster = "XCP_Master";

		private static readonly string ProtectedResourceMode_File = "FILE";

		private static readonly string InitCmdTypeName = "init_CMD";

		private static readonly string InitResErrTypeName = "init_RES_ERR";

		private static readonly string FlexrayXCPFrameNamePrefix = "XCP_DTO_CONTAINER";

		private static readonly string GPSSignalCANapeAltitude = "GPS_z";

		private static readonly string GPSSignalCANapeCourse = "GPS_course";

		private static readonly string GPSSignalCANapeLatitude = "GPS_y";

		private static readonly string GPSSignalCANapeLongitude = "GPS_x";

		private static readonly string GPSSignalCANapeSpeed = "GPS_speed";

		private static readonly string GPSSignalCANoeAltitude = "Altitude";

		private static readonly string GPSSignalCANoeCourse = "Course";

		private static readonly string GPSSignalCANoeLatitude = "Latitude";

		private static readonly string GPSSignalCANoeLatitudeD = "LatitudeD";

		private static readonly string GPSSignalCANoeLatitudeSi = "LatitudeSi";

		private static readonly string GPSSignalCANoeLatitudeM = "LatitudeM";

		private static readonly string GPSSignalCANoeLatitudeS = "LatitudeS";

		private static readonly string GPSSignalCANoeLongitude = "Longitude";

		private static readonly string GPSSignalCANoeLongitudeD = "LongitudeD";

		private static readonly string GPSSignalCANoeLongitudeSi = "LongitudeSi";

		private static readonly string GPSSignalCANoeLongitudeM = "LongitudeM";

		private static readonly string GPSSignalCANoeLongitudeS = "LongitudeS";

		private static readonly string GPSSignalCANoeSpeed = "Speed";

		private static readonly string GPSSignalDay = "Day";

		private static readonly string GPSSignalHours = "Hours";

		private static readonly string GPSSignalMinutes = "Minutes";

		private static readonly string GPSSignalMonth = "Month";

		private static readonly string GPSSignalSeconds = "Seconds";

		private static readonly string GPSSignalYear = "Year";

		private static readonly string GPSSignalCANoeAltitudeDE = "Hoehe";

		private static readonly string GPSSignalCANoeCourseDE = "Richtung";

		private static readonly string GPSSignalCANoeLatitudeDE = "Breite";

		private static readonly string GPSSignalCANoeLatitudeDDE = "BreiteG";

		private static readonly string GPSSignalCANoeLatitudeSiDE = "BreiteVZ";

		private static readonly string GPSSignalCANoeLatitudeMDE = "BreiteM";

		private static readonly string GPSSignalCANoeLatitudeSDE = "BreiteS";

		private static readonly string GPSSignalCANoeLongitudeDE = "Laenge";

		private static readonly string GPSSignalCANoeLongitudeDDE = "LaengeG";

		private static readonly string GPSSignalCANoeLongitudeSiDE = "LaengeVZ";

		private static readonly string GPSSignalCANoeLongitudeMDE = "LaengeM";

		private static readonly string GPSSignalCANoeLongitudeSDE = "LaengeS";

		private static readonly string GPSSignalCANoeSpeedDE = "Geschwindigkeit";

		private static readonly string GPSSignalDayDE = "Tag";

		private static readonly string GPSSignalHoursDE = "Stunde";

		private static readonly string GPSSignalMinutesDE = "Minute";

		private static readonly string GPSSignalMonthDE = "Monat";

		private static readonly string GPSSignalSecondsDE = "Sekunde";

		private static readonly string GPSSignalYearDE = "Jahr";

		public event EventHandler DatabaseFileChanged
		{
			add
			{
				this.databaseFileChanged = (EventHandler)Delegate.Combine(this.databaseFileChanged, value);
			}
			remove
			{
				this.databaseFileChanged = (EventHandler)Delegate.Remove(this.databaseFileChanged, value);
			}
		}

		private SymbolSelectionDialog SymbSel
		{
			get
			{
				if (this.symbSel == null)
				{
					this.symbSel = new SymbolSelectionDialog();
					this.symbSel.SuspendUpdate();
					this.symbSel.Options.MultiSelect = false;
					this.symbSel.ShowApplyButton = false;
					this.symbSel.ShowHelpButton = false;
					this.symbSel.FormApplied += new FormAppliedEventHandler(this.symbSel_FormApplied);
					this.symbSel.HelpButtonClicked += new CancelEventHandler(this.symbSel_HelpButtonClicked);
					this.symbSel.ResumeUpdate();
				}
				return this.symbSel;
			}
		}

		public bool IsHexDisplay
		{
			get
			{
				return this.isHexDisplay;
			}
			set
			{
				this.isHexDisplay = value;
			}
		}

		public string SymSelHelpFileName
		{
			get
			{
				return this.helpFileName;
			}
			set
			{
				this.helpFileName = value;
			}
		}

		public void UpdateApplicationDatabaseConfiguration(DatabaseConfiguration databaseConfiguration, MultibusChannelConfiguration multibusChannelConfiguration, CANChannelConfiguration canChannelConfiguration, string configFolderPath)
		{
			this.mDatabaseConfiguration = databaseConfiguration;
			this.mConfigFolderPath = configFolderPath;
			this.ClearFileWatchers();
			foreach (IDatabaseAccessObserver current in this.mDatabaseAccessObservers)
			{
				if (current.LoadDatabases() && this.databaseFileChanged != null)
				{
					this.databaseFileChanged(this, EventArgs.Empty);
				}
			}
			IList<Vector.COMdbLib.IDatabase> list = new List<Vector.COMdbLib.IDatabase>(this.dbManager.Databases);
			foreach (Vector.VLConfig.Data.ConfigurationDataModel.Database current2 in databaseConfiguration.Databases)
			{
				if (Vector.VLConfig.Data.ConfigurationDataModel.BusType.Bt_CAN != current2.BusType.Value || (ulong)current2.ChannelNumber.Value <= (ulong)((long)canChannelConfiguration.CANChannels.Count) || (ulong)current2.ChannelNumber.Value <= (ulong)((long)multibusChannelConfiguration.Channels.Count))
				{
					Vector.COMdbLib.IDatabase cOMdbLibDatabase = this.GetCOMdbLibDatabase(current2, configFolderPath);
					if (cOMdbLibDatabase == null)
					{
						OpenDatabaseResult openDatabaseResult = this.OpenDatabase(this.GetAbsoluteFilePath(current2, configFolderPath), current2.NetworkName.Value, ref cOMdbLibDatabase);
						OpenDatabaseResult openDatabaseResult2 = openDatabaseResult;
						if (openDatabaseResult2 != OpenDatabaseResult.OpenSuccess)
						{
							continue;
						}
						if (cOMdbLibDatabase.IsConvertedASRFile())
						{
							current2.IsAUTOSARFile = true;
						}
						uint extraCPChannel = 0u;
						IDictionary<string, bool> dictionary;
						this.GetDatabaseCPConfiguration(current2, configFolderPath, out dictionary, out extraCPChannel);
						if (current2.CPType.Value != CPType.None && dictionary != null)
						{
							current2.CcpXcpEcuDisplayName.Value = Vector.VLConfig.Data.ConfigurationDataModel.Database.MakeCpEcuDisplayName(dictionary.Keys);
							current2.CcpXcpIsSeedAndKeyUsed = (dictionary.Count > 0 && dictionary.First<KeyValuePair<string, bool>>().Value);
						}
						if (Vector.VLConfig.Data.ConfigurationDataModel.BusType.Bt_FlexRay == current2.BusType.Value)
						{
							current2.ExtraCPChannel = extraCPChannel;
						}
						using (List<IDatabaseAccessObserver>.Enumerator enumerator3 = this.mDatabaseAccessObservers.GetEnumerator())
						{
							while (enumerator3.MoveNext())
							{
								IDatabaseAccessObserver current3 = enumerator3.Current;
								current3.OnDatabaseLoaded();
							}
							continue;
						}
					}
					list.Remove(cOMdbLibDatabase);
				}
			}
			this.UpdateDatabaseFileWatchers(databaseConfiguration, configFolderPath);
			foreach (Vector.COMdbLib.IDatabase current4 in list)
			{
				this.CloseDatabase(current4);
			}
		}

		public void UpdateApplicationExportDatabaseConfiguration(ExportDatabaseConfiguration exportDatabaseConfiguration, string configFolderPath)
		{
			foreach (IDatabaseAccessObserver current in this.mDatabaseAccessObservers)
			{
				if (current.LoadDatabases() && this.databaseFileChanged != null)
				{
					this.databaseFileChanged(this, EventArgs.Empty);
				}
			}
			IList<Vector.COMdbLib.IDatabase> list = new List<Vector.COMdbLib.IDatabase>(this.dbManager.Databases);
			foreach (Vector.VLConfig.Data.ConfigurationDataModel.Database current2 in exportDatabaseConfiguration.ExportDatabases)
			{
				Vector.COMdbLib.IDatabase cOMdbLibDatabase = this.GetCOMdbLibDatabase(current2, configFolderPath);
				if (cOMdbLibDatabase == null)
				{
					OpenDatabaseResult openDatabaseResult = this.OpenDatabase(this.GetAbsoluteFilePath(current2, configFolderPath), current2.NetworkName.Value, ref cOMdbLibDatabase);
					OpenDatabaseResult openDatabaseResult2 = openDatabaseResult;
					if (openDatabaseResult2 == OpenDatabaseResult.OpenSuccess)
					{
						if (cOMdbLibDatabase.IsConvertedASRFile())
						{
							current2.IsAUTOSARFile = true;
						}
						uint extraCPChannel = 0u;
						IDictionary<string, bool> dictionary;
						this.GetDatabaseCPConfiguration(current2, configFolderPath, out dictionary, out extraCPChannel);
						if (current2.CPType.Value != CPType.None && dictionary != null)
						{
							current2.CcpXcpEcuDisplayName.Value = Vector.VLConfig.Data.ConfigurationDataModel.Database.MakeCpEcuDisplayName(dictionary.Keys);
							current2.CcpXcpIsSeedAndKeyUsed = (dictionary.Count > 0 && dictionary.First<KeyValuePair<string, bool>>().Value);
						}
						if (Vector.VLConfig.Data.ConfigurationDataModel.BusType.Bt_FlexRay == current2.BusType.Value)
						{
							current2.ExtraCPChannel = extraCPChannel;
						}
					}
				}
				else
				{
					list.Remove(cOMdbLibDatabase);
				}
			}
			if (this.mDatabaseConfiguration != null)
			{
				foreach (Vector.VLConfig.Data.ConfigurationDataModel.Database current3 in this.mDatabaseConfiguration.Databases)
				{
					Vector.COMdbLib.IDatabase cOMdbLibDatabase2 = this.GetCOMdbLibDatabase(current3, configFolderPath);
					if (cOMdbLibDatabase2 != null)
					{
						list.Remove(cOMdbLibDatabase2);
					}
				}
			}
			foreach (ExportDatabaseConfiguration current4 in this.mExportDatabaseConfigurationList)
			{
				if (current4 != exportDatabaseConfiguration)
				{
					foreach (Vector.VLConfig.Data.ConfigurationDataModel.Database current5 in current4.ExportDatabases)
					{
						Vector.COMdbLib.IDatabase cOMdbLibDatabase3 = this.GetCOMdbLibDatabase(current5, configFolderPath);
						if (cOMdbLibDatabase3 != null)
						{
							list.Remove(cOMdbLibDatabase3);
						}
					}
				}
			}
			foreach (Vector.COMdbLib.IDatabase current6 in list)
			{
				this.CloseDatabase(current6);
			}
		}

		public bool IsGMLANDatabase(string absoluteFilePath)
		{
			foreach (Vector.COMdbLib.IDatabase current in this.dbManager.Databases)
			{
				if (string.Compare(current.DatabaseFile, absoluteFilePath, true) == 0)
				{
					return this.IsGMLANDatabase(current);
				}
			}
			bool result = false;
			Vector.COMdbLib.IDatabase database = null;
			if (this.dbManager.OpenDatabase(absoluteFilePath, ref database) == OpenDatabaseResult.OpenSuccess)
			{
				result = this.IsGMLANDatabase(database);
			}
			if (database != null)
			{
				this.dbManager.CloseDatabase(database);
			}
			return result;
		}

		private bool IsGMLANDatabase(Vector.COMdbLib.IDatabase comDbLibDatabase)
		{
			bool result = false;
			foreach (IAttributeDefinition current in comDbLibDatabase.AttributeDefinitions)
			{
				if (string.Compare(current.Name, ApplicationDatabaseManager.AttributeUseGMParamIDs, true) == 0)
				{
					result = true;
					break;
				}
			}
			return result;
		}

		public bool IsMessageWithExtendedGMId(string databasePath, string networkName, string messageSymbol, Vector.VLConfig.Data.ConfigurationDataModel.BusType busType)
		{
			Vector.COMdbLib.IDatabase databaseForFilePathAndNetworkName = this.GetDatabaseForFilePathAndNetworkName(databasePath, networkName);
			if (databaseForFilePathAndNetworkName == null)
			{
				return false;
			}
			IDBNetwork dBNetwork = databaseForFilePathAndNetworkName.DBNetwork;
			if (dBNetwork == null)
			{
				return false;
			}
			if (Vector.VLConfig.Data.ConfigurationDataModel.BusType.Bt_CAN == busType && this.IsGMLANDatabase(databaseForFilePathAndNetworkName))
			{
				IFrameOrPDU frameOrPDUByName = dBNetwork.FrameOrPDUs.GetFrameOrPDUByName(messageSymbol);
				if (frameOrPDUByName != null && frameOrPDUByName.Attributes.AttributeExists(ApplicationDatabaseManager.AttributeMsgPrio))
				{
					return true;
				}
			}
			return false;
		}

		public bool IsFlexrayXCPFrame(string flexrayDbPath, string networkName, string messageSymbol)
		{
			if (!messageSymbol.StartsWith(ApplicationDatabaseManager.FlexrayXCPFrameNamePrefix))
			{
				return false;
			}
			Vector.COMdbLib.IDatabase databaseForFilePathAndNetworkName = this.GetDatabaseForFilePathAndNetworkName(flexrayDbPath, networkName);
			if (databaseForFilePathAndNetworkName == null)
			{
				return false;
			}
			if (databaseForFilePathAndNetworkName.FileType != Vector.COMdbLib.DatabaseFileType.kFileTypeXML)
			{
				return false;
			}
			IDBNetwork dBNetwork = databaseForFilePathAndNetworkName.DBNetwork;
			if (dBNetwork == null)
			{
				return false;
			}
			IFrameOrPDU frameOrPDUByName = dBNetwork.FrameOrPDUs.GetFrameOrPDUByName(messageSymbol);
			if (frameOrPDUByName != null && frameOrPDUByName is IFlexRayFrameOrPDU)
			{
				IFlexRayFrameOrPDU flexRayFrameOrPDU = frameOrPDUByName as IFlexRayFrameOrPDU;
				if (flexRayFrameOrPDU.XCPFrameExtension != null)
				{
					return true;
				}
			}
			return false;
		}

		public bool IsFlexrayDatabase(string absoluteFilePath, out IDictionary<string, bool> flexrayNetworkNamesIsDualChannel)
		{
			Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
			INetworkInfoSet networkInfosOfDatabaseFile = this.dbManager.GetNetworkInfosOfDatabaseFile(absoluteFilePath);
			if (networkInfosOfDatabaseFile.Count<INetworkInfo>() > 0)
			{
				foreach (INetworkInfo current in networkInfosOfDatabaseFile)
				{
					if (current.TypeOfNetwork == NetworkType.kNetworkTypeFlexRay)
					{
						Vector.COMdbLib.IDatabase database = null;
						if (this.OpenDatabase(absoluteFilePath, current.NetworkName, ref database) == OpenDatabaseResult.OpenSuccess && database.FIBEXVersion != null && database.FIBEXVersion.VersionType != FIBEXVersionType.FIBEXVersionUnspec && database.FIBEXVersion.VersionType != FIBEXVersionType.FIBEXVersion_4_0)
						{
							IFlexRayCluster flexRayCluster = database.DBNetwork as IFlexRayCluster;
							if (flexRayCluster != null)
							{
								bool value = flexRayCluster.ChannelMask == FlexRayChannel.ChannelAB;
								dictionary.Add(current.NetworkName, value);
							}
						}
						if (database != null)
						{
							this.CloseDatabase(database);
						}
					}
				}
			}
			flexrayNetworkNamesIsDualChannel = dictionary;
			return flexrayNetworkNamesIsDualChannel.Count > 0;
		}

		public bool GetFibexVersionString(Vector.VLConfig.Data.ConfigurationDataModel.Database db, string configFolderPath, out string fibexVersion)
		{
			fibexVersion = "";
			Vector.COMdbLib.IDatabase cOMdbLibDatabase = this.GetCOMdbLibDatabase(db, configFolderPath);
			if (cOMdbLibDatabase == null)
			{
				return false;
			}
			if (cOMdbLibDatabase.FileType == Vector.COMdbLib.DatabaseFileType.kFileTypeXML)
			{
				fibexVersion = cOMdbLibDatabase.FIBEXVersion.VersionString;
				return true;
			}
			return false;
		}

		public bool GetAllXcpFramesOfFlexrayXcpDatabase(Vector.VLConfig.Data.ConfigurationDataModel.Database databaseInModel, string configFolderPath, out IList<MessageDefinition> frames)
		{
			frames = new List<MessageDefinition>();
			if (databaseInModel.BusType.Value != Vector.VLConfig.Data.ConfigurationDataModel.BusType.Bt_FlexRay)
			{
				return false;
			}
			if (databaseInModel.CPType.Value != CPType.XCP)
			{
				return false;
			}
			Vector.COMdbLib.IDatabase cOMdbLibDatabase = this.GetCOMdbLibDatabase(databaseInModel, configFolderPath);
			if (cOMdbLibDatabase == null)
			{
				return false;
			}
			if (cOMdbLibDatabase.FileType != Vector.COMdbLib.DatabaseFileType.kFileTypeXML)
			{
				return false;
			}
			if (cOMdbLibDatabase.DBNetwork == null)
			{
				return false;
			}
			if (cOMdbLibDatabase.DBNetwork is IFlexRayCluster)
			{
				IFlexRayCluster flexRayCluster = cOMdbLibDatabase.DBNetwork as IFlexRayCluster;
				foreach (IFlexRayNode current in flexRayCluster.FlexRayNodes)
				{
					foreach (IFlexRayFrame current2 in current.TxFlexRayFrames)
					{
						if (current2.ServiceType == FlexRayServiceType.FlexRayServiceType_XCP_PreConfigured || current2.ServiceType == FlexRayServiceType.FlexRayServiceType_XCP_RuntimeConfigured)
						{
							frames.Add(new MessageDefinition((uint)current2.SlotID, current2.BaseCycle, current2.CycleRepetition));
						}
					}
				}
			}
			return true;
		}

		public List<Tuple<Vector.VLConfig.Data.ConfigurationDataModel.Database, uint>> GetFlexRayDatabasesSuitedForXcp(uint channelNumber, ReadOnlyCollection<Vector.VLConfig.Data.ConfigurationDataModel.Database> flexrayDatabases, string configFolderPath)
		{
			List<Tuple<Vector.VLConfig.Data.ConfigurationDataModel.Database, uint>> list = new List<Tuple<Vector.VLConfig.Data.ConfigurationDataModel.Database, uint>>();
			uint num;
			uint num2;
			if (channelNumber == Vector.VLConfig.Data.ConfigurationDataModel.Database.ChannelNumber_FlexrayAB)
			{
				num = 1u;
				num2 = 2u;
			}
			else
			{
				num2 = channelNumber;
				num = channelNumber;
			}
			foreach (Vector.VLConfig.Data.ConfigurationDataModel.Database current in flexrayDatabases)
			{
				if ((channelNumber == Vector.VLConfig.Data.ConfigurationDataModel.Database.ChannelNumber_FlexrayAB || (current.ChannelNumber.Value >= num && current.ChannelNumber.Value <= num2)) && current.BusType.Value == Vector.VLConfig.Data.ConfigurationDataModel.BusType.Bt_FlexRay)
				{
					Vector.COMdbLib.IDatabase cOMdbLibDatabase = this.GetCOMdbLibDatabase(current, configFolderPath);
					if (cOMdbLibDatabase != null && cOMdbLibDatabase.FileType == Vector.COMdbLib.DatabaseFileType.kFileTypeXML && cOMdbLibDatabase.DBNetwork != null)
					{
						IFlexRayCluster flexRayCluster = cOMdbLibDatabase.DBNetwork as IFlexRayCluster;
						if (flexRayCluster != null)
						{
							foreach (IFlexRayNode current2 in flexRayCluster.FlexRayNodes)
							{
								if (string.Compare(current2.Name, ApplicationDatabaseManager.NodeNameXCPMaster, StringComparison.InvariantCultureIgnoreCase) == 0 && (current2.XCPECUExtension == null || current2.XCPECUExtension.LoggingConfigurations == null || !current2.XCPECUExtension.LoggingConfigurations.Any<IXCPLoggingConfiguration>()))
								{
									IFlexRayFrame flexRayFrame = current2.TxFlexRayFrames.FirstOrDefault((IFlexRayFrame pdu) => pdu.ServiceType == FlexRayServiceType.FlexRayServiceType_XCP_PreConfigured || pdu.ServiceType == FlexRayServiceType.FlexRayServiceType_XCP_RuntimeConfigured);
									IFlexRayFrame flexRayFrame2 = current2.RxFlexRayFrames.FirstOrDefault((IFlexRayFrame pdu) => pdu.ServiceType == FlexRayServiceType.FlexRayServiceType_XCP_PreConfigured || pdu.ServiceType == FlexRayServiceType.FlexRayServiceType_XCP_RuntimeConfigured);
									if (flexRayFrame != null && flexRayFrame2 != null && flexRayFrame.ChannelMask == flexRayFrame2.ChannelMask)
									{
										uint num3 = 1u;
										if (flexRayFrame.ChannelMask == FlexRayChannel.ChannelB)
										{
											num3 = 2u;
										}
										if (num3 >= num && num3 <= num2 && (current.ChannelNumber.Value == Vector.VLConfig.Data.ConfigurationDataModel.Database.ChannelNumber_FlexrayAB || num3 == current.ChannelNumber.Value))
										{
											list.Add(new Tuple<Vector.VLConfig.Data.ConfigurationDataModel.Database, uint>(current, num3));
											break;
										}
									}
								}
							}
						}
					}
				}
			}
			return list;
		}

		public bool ProvideFlexRayFibexDataForXcp(IFibexFlexRayDataProvider provider, Vector.VLConfig.Data.ConfigurationDataModel.Database databaseInModel, string configFolderPath)
		{
			if (databaseInModel.BusType.Value != Vector.VLConfig.Data.ConfigurationDataModel.BusType.Bt_FlexRay)
			{
				return false;
			}
			Vector.COMdbLib.IDatabase cOMdbLibDatabase = this.GetCOMdbLibDatabase(databaseInModel, configFolderPath);
			if (cOMdbLibDatabase == null)
			{
				return false;
			}
			if (cOMdbLibDatabase.FileType != Vector.COMdbLib.DatabaseFileType.kFileTypeXML)
			{
				return false;
			}
			if (cOMdbLibDatabase.DBNetwork == null)
			{
				return false;
			}
			IFlexRayCluster flexRayCluster = cOMdbLibDatabase.DBNetwork as IFlexRayCluster;
			if (flexRayCluster == null)
			{
				return false;
			}
			provider.Cluster = flexRayCluster.ClusterID;
			provider.Network = flexRayCluster.Name;
			provider.Fibex = Path.GetFileName(flexRayCluster.Database.DatabaseFile);
			provider.FibexPath = Path.GetDirectoryName(flexRayCluster.Database.DatabaseFile);
			provider.IsFibexPlus = flexRayCluster.Database.FIBEXVersion.SupportsPDUModel();
			IFibexFlexRayCCParameters lowLevelParameter = provider.GetLowLevelParameter();
			lowLevelParameter.NumberOfStaticSlots = flexRayCluster.NumberOfStaticSlots;
			lowLevelParameter.PayloadLengthStatic = flexRayCluster.PayloadLengthStatic;
			lowLevelParameter.Cycle = flexRayCluster.Cycle;
			foreach (IFrameOrPDU current in flexRayCluster.FrameOrPDUs)
			{
				IFlexRayPDU flexRayPDU = current as IFlexRayPDU;
				if (flexRayPDU != null)
				{
					using (IEnumerator<IMappedFlexRayPDU> enumerator2 = flexRayPDU.MappedFlexRayPDUs.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							IMappedFlexRayPDU current2 = enumerator2.Current;
							if (current2.FlexRayFrame.MappedFlexRayPDUs.Count == 1u && (current2.FlexRayFrame.ServiceType == FlexRayServiceType.FlexRayServiceType_XCP_PreConfigured || current2.FlexRayFrame.ServiceType == FlexRayServiceType.FlexRayServiceType_XCP_RuntimeConfigured))
							{
								EnumFrameType type = (current2.FlexRayFrame.ServiceType == FlexRayServiceType.FlexRayServiceType_XCP_PreConfigured) ? EnumFrameType.kXcpPreConfigured : EnumFrameType.kXcpRuntimeConfigured;
								foreach (IAbsolutelyScheduledTiming current3 in current2.FlexRayFrame.AbsolutelyScheduledTimings)
								{
									IFibexFlexRayFrameTiming fibexFlexRayFrameTiming = provider.AddImportXcpFrame();
									fibexFlexRayFrameTiming.SlotId = current3.SlotID;
									fibexFlexRayFrameTiming.BaseCycle = current3.BaseCycle;
									fibexFlexRayFrameTiming.Repetition = current3.CycleRepetition;
									fibexFlexRayFrameTiming.Channel = ((current2.FlexRayFrame.ChannelMask == FlexRayChannel.ChannelA) ? EnumFlexRayChannel.kChA : EnumFlexRayChannel.kChB);
									IFibexFlexRayFrameInfo frameInfo = fibexFlexRayFrameTiming.FrameInfo;
									frameInfo.IsPdu = true;
									frameInfo.PduPosition = (int)current2.Startbit;
									int num = -1;
									if (!current2.GetUpdateBitPosition(ref num).PropertyOK || num < 0)
									{
										frameInfo.UseUpdateBit = false;
										frameInfo.UpdateBitPosition = -1;
									}
									else
									{
										frameInfo.UseUpdateBit = true;
										if (flexRayCluster.BitCountingPolicy == FlexRayBitCountingPolicy.Sawtooth)
										{
											uint num2 = (uint)Math.Ceiling((double)num / 8.0);
											uint num3 = (uint)(num % 8);
											num = (int)(num2 * 8u + (7u - num3));
										}
										frameInfo.UpdateBitPosition = num;
									}
									frameInfo.PayloadLen = (int)(flexRayPDU.Bitcount / 8u);
									frameInfo.CarrierPayloadLen = current2.FlexRayFrame.PayLoadLength;
									frameInfo.Type = type;
									frameInfo.Name = flexRayPDU.Name;
								}
							}
						}
						continue;
					}
				}
				IFlexRayFrame flexRayFrame = current as IFlexRayFrame;
				if (flexRayFrame != null && (flexRayFrame.ServiceType == FlexRayServiceType.FlexRayServiceType_XCP_PreConfigured || flexRayFrame.ServiceType == FlexRayServiceType.FlexRayServiceType_XCP_RuntimeConfigured))
				{
					EnumFrameType type2 = (flexRayFrame.ServiceType == FlexRayServiceType.FlexRayServiceType_XCP_PreConfigured) ? EnumFrameType.kXcpPreConfigured : EnumFrameType.kXcpRuntimeConfigured;
					foreach (IAbsolutelyScheduledTiming current4 in flexRayFrame.AbsolutelyScheduledTimings)
					{
						IFibexFlexRayFrameTiming fibexFlexRayFrameTiming2 = provider.AddImportXcpFrame();
						fibexFlexRayFrameTiming2.SlotId = current4.SlotID;
						fibexFlexRayFrameTiming2.BaseCycle = current4.BaseCycle;
						fibexFlexRayFrameTiming2.Repetition = current4.CycleRepetition;
						fibexFlexRayFrameTiming2.Channel = ((flexRayFrame.ChannelMask == FlexRayChannel.ChannelA) ? EnumFlexRayChannel.kChA : EnumFlexRayChannel.kChB);
						IFibexFlexRayFrameInfo frameInfo2 = fibexFlexRayFrameTiming2.FrameInfo;
						frameInfo2.IsPdu = false;
						frameInfo2.PduPosition = 0;
						frameInfo2.UseUpdateBit = false;
						frameInfo2.UpdateBitPosition = -1;
						frameInfo2.PayloadLen = flexRayFrame.PayLoadLength;
						frameInfo2.CarrierPayloadLen = flexRayFrame.PayLoadLength;
						frameInfo2.Type = type2;
						frameInfo2.Name = flexRayFrame.Name;
					}
				}
			}
			return true;
		}

		public bool IsAutosarDescriptionFile(string absFilePath, out IDictionary<string, Vector.VLConfig.Data.ConfigurationDataModel.BusType> networks)
		{
			networks = new Dictionary<string, Vector.VLConfig.Data.ConfigurationDataModel.BusType>();
			bool flag = false;
			INetworkInfoSet networkInfosOfDatabaseFile = this.dbManager.GetNetworkInfosOfDatabaseFile(absFilePath);
			if (networkInfosOfDatabaseFile.Count<INetworkInfo>() > 0)
			{
				Vector.COMdbLib.IDatabase database = null;
				if (this.OpenDatabase(absFilePath, networkInfosOfDatabaseFile.First<INetworkInfo>().NetworkName, ref database) == OpenDatabaseResult.OpenSuccess)
				{
					if (database.IsConvertedASRFile())
					{
						flag = true;
					}
					this.CloseDatabase(database);
				}
				if (flag)
				{
					foreach (INetworkInfo current in networkInfosOfDatabaseFile)
					{
						if (current.TypeOfNetwork == NetworkType.kNetworkTypeCAN)
						{
							networks.Add(current.NetworkName, Vector.VLConfig.Data.ConfigurationDataModel.BusType.Bt_CAN);
						}
						else if (current.TypeOfNetwork == NetworkType.kNetworkTypeLIN)
						{
							networks.Add(current.NetworkName, Vector.VLConfig.Data.ConfigurationDataModel.BusType.Bt_LIN);
						}
						else if (current.TypeOfNetwork == NetworkType.kNetworkTypeFlexRay)
						{
							networks.Add(current.NetworkName, Vector.VLConfig.Data.ConfigurationDataModel.BusType.Bt_FlexRay);
						}
					}
				}
			}
			return flag;
		}

		public bool IsCanFdDatabase(string absoluteFilePath, string networkName)
		{
			bool result = false;
			foreach (Vector.COMdbLib.IDatabase current in this.dbManager.Databases)
			{
				if (string.Compare(current.DatabaseFile, absoluteFilePath, true) == 0 && (string.IsNullOrEmpty(networkName) || current.DBNetwork.Name == networkName))
				{
					string a = "";
					current.DBNetwork.Attributes.GetStringValue(ApplicationDatabaseManager.AttributeBusType, ref a);
					result = (a == ApplicationDatabaseManager.AttributeBusTypeValue_CANFD);
					return result;
				}
			}
			Vector.COMdbLib.IDatabase database = null;
			if (this.OpenDatabase(absoluteFilePath, networkName, ref database) == OpenDatabaseResult.OpenSuccess)
			{
				if (database.DBNetwork != null)
				{
					string a2 = "";
					database.DBNetwork.Attributes.GetStringValue(ApplicationDatabaseManager.AttributeBusType, ref a2);
					result = (a2 == ApplicationDatabaseManager.AttributeBusTypeValue_CANFD);
				}
				this.CloseDatabase(database);
			}
			return result;
		}

		private OpenDatabaseResult OpenDatabase(string absoluteFilePath, string networkName, ref Vector.COMdbLib.IDatabase db)
		{
			OpenDatabaseResult result;
			if (networkName == "")
			{
				result = this.dbManager.OpenDatabase(absoluteFilePath, ref db);
			}
			else
			{
				INetworkInfoSet networkInfoSet = this.dbManager.CreateEmptyNetworkInfoSet();
				INetworkInfo networkInfo = networkInfoSet.CreateAndAddNetworkInfo();
				networkInfo.DatabaseFile = absoluteFilePath;
				networkInfo.NetworkName = networkName;
				this.dbManager.OpenDatabasesByNetworkInfos(networkInfoSet);
				db = networkInfo.Database;
				result = networkInfo.OpenResult;
			}
			return result;
		}

		private void CloseDatabase(Vector.COMdbLib.IDatabase comDbLibDatabase)
		{
			this.dbManager.CloseDatabase(comDbLibDatabase);
		}

		private void UpdateDatabaseFileWatchers(DatabaseConfiguration databaseConfiguration, string configFolderPath)
		{
			this.ClearFileWatchers();
			foreach (Vector.VLConfig.Data.ConfigurationDataModel.Database current in databaseConfiguration.AllDescriptionFiles)
			{
				string absoluteFilePath = this.GetAbsoluteFilePath(current, configFolderPath);
				if (File.Exists(absoluteFilePath))
				{
					this.AddFileWatcher(absoluteFilePath, current.NetworkName.Value);
				}
			}
		}

		private void ClearFileWatchers()
		{
			foreach (FileSystemWatcher current in this.fileWatcherToDbPath.Keys)
			{
				current.EnableRaisingEvents = false;
				current.Deleted -= new FileSystemEventHandler(this.OnDatabaseChanged);
				current.Changed -= new FileSystemEventHandler(this.OnDatabaseChanged);
				current.Created -= new FileSystemEventHandler(this.OnDatabaseChanged);
				current.Renamed -= new RenamedEventHandler(this.OnDatabaseRenamed);
			}
			this.fileWatcherToDbPath.Clear();
			this.dbPathToIsDbChanged.Clear();
			this.dbPathToNetworkNames.Clear();
		}

		private void AddFileWatcher(string absDbFilePath, string networkName)
		{
			if (this.dbPathToIsDbChanged.Keys.Contains(absDbFilePath))
			{
				this.dbPathToNetworkNames[absDbFilePath].Add(networkName);
				return;
			}
			this.dbPathToIsDbChanged.Add(absDbFilePath, false);
			HashSet<string> hashSet = new HashSet<string>();
			hashSet.Add(networkName);
			this.dbPathToNetworkNames.Add(absDbFilePath, hashSet);
			FileSystemWatcher fileSystemWatcher = new FileSystemWatcher(Path.GetDirectoryName(absDbFilePath), Path.GetFileName(absDbFilePath));
			fileSystemWatcher.Deleted += new FileSystemEventHandler(this.OnDatabaseChanged);
			fileSystemWatcher.Changed += new FileSystemEventHandler(this.OnDatabaseChanged);
			fileSystemWatcher.Created += new FileSystemEventHandler(this.OnDatabaseChanged);
			fileSystemWatcher.Renamed += new RenamedEventHandler(this.OnDatabaseRenamed);
			fileSystemWatcher.EnableRaisingEvents = true;
			this.fileWatcherToDbPath.Add(fileSystemWatcher, absDbFilePath);
		}

		private void ProcessDatabaseChange(string absDbFilePath)
		{
			foreach (IDatabaseAccessObserver current in this.mDatabaseAccessObservers)
			{
				current.OnDatabaseChange(absDbFilePath);
			}
			List<Vector.COMdbLib.IDatabase> list = new List<Vector.COMdbLib.IDatabase>();
			foreach (Vector.COMdbLib.IDatabase current2 in this.dbManager.Databases)
			{
				if (string.Compare(current2.DatabaseFile, absDbFilePath, true) == 0)
				{
					list.Add(current2);
				}
			}
			for (int i = 0; i < list.Count; i++)
			{
				this.CloseDatabase(list[i]);
			}
			if (File.Exists(absDbFilePath))
			{
				Vector.COMdbLib.IDatabase database = null;
				foreach (string current3 in this.dbPathToNetworkNames[absDbFilePath])
				{
					this.OpenDatabase(absDbFilePath, current3, ref database);
				}
			}
			if (this.databaseFileChanged != null)
			{
				this.databaseFileChanged(this, EventArgs.Empty);
			}
		}

		private void OnApplicationIdle(object sender, EventArgs e)
		{
			List<string> list = new List<string>(this.dbPathToIsDbChanged.Keys.ToList<string>());
			foreach (string current in list)
			{
				if (this.dbPathToIsDbChanged.ContainsKey(current) && this.dbPathToIsDbChanged[current])
				{
					this.ProcessDatabaseChange(current);
					this.dbPathToIsDbChanged[current] = false;
				}
			}
		}

		private void OnDatabaseChanged(object sender, FileSystemEventArgs e)
		{
			FileSystemWatcher key = sender as FileSystemWatcher;
			if (this.fileWatcherToDbPath.ContainsKey(key))
			{
				string key2 = this.fileWatcherToDbPath[key];
				if (this.dbPathToIsDbChanged.ContainsKey(key2))
				{
					this.dbPathToIsDbChanged[key2] = true;
				}
			}
		}

		private void OnDatabaseRenamed(object source, RenamedEventArgs e)
		{
			FileSystemWatcher key = source as FileSystemWatcher;
			if (this.fileWatcherToDbPath.ContainsKey(key))
			{
				string key2 = this.fileWatcherToDbPath[key];
				if (this.dbPathToIsDbChanged.ContainsKey(key2))
				{
					this.dbPathToIsDbChanged[key2] = true;
				}
			}
		}

		private string GetAbsoluteFilePath(Vector.VLConfig.Data.ConfigurationDataModel.Database db, string configFolderPath)
		{
			return FileSystemServices.GetAbsolutePath(db.FilePath.Value, configFolderPath);
		}

		public DatabaseOpenStatus GetDatabaseOpenStatus(Vector.VLConfig.Data.ConfigurationDataModel.Database databaseInModel, string configFolderPath)
		{
			Vector.COMdbLib.IDatabase cOMdbLibDatabase = this.GetCOMdbLibDatabase(databaseInModel, configFolderPath);
			if (cOMdbLibDatabase == null)
			{
				return DatabaseOpenStatus.NotAvailable;
			}
			if (cOMdbLibDatabase.DBNetwork == null)
			{
				return DatabaseOpenStatus.Error;
			}
			if (cOMdbLibDatabase.GetFormattedErrorString(1).Length > 0)
			{
				return DatabaseOpenStatus.Warning;
			}
			return DatabaseOpenStatus.OK;
		}

		public void CloseAllDatabases()
		{
			this.ClearFileWatchers();
			IList<Vector.COMdbLib.IDatabase> list = new List<Vector.COMdbLib.IDatabase>(this.dbManager.Databases);
			foreach (Vector.COMdbLib.IDatabase current in list)
			{
				this.CloseDatabase(current);
			}
		}

		public Vector.COMdbLib.IDatabase GetCOMdbLibDatabase(Vector.VLConfig.Data.ConfigurationDataModel.Database databaseInModel, string configFolderPath)
		{
			string absoluteFilePath = this.GetAbsoluteFilePath(databaseInModel, configFolderPath);
			foreach (Vector.COMdbLib.IDatabase current in this.dbManager.Databases)
			{
				if (current.DatabaseFile == absoluteFilePath && (databaseInModel.NetworkName.Value == "" || current.DBNetwork.Name == databaseInModel.NetworkName.Value))
				{
					return current;
				}
			}
			return null;
		}

		public IFrameOrPDU GetCOMdbLibMessage(Vector.VLConfig.Data.ConfigurationDataModel.Database databaseInModel, string configFolderPath, string messageName)
		{
			Vector.COMdbLib.IDatabase cOMdbLibDatabase = this.GetCOMdbLibDatabase(databaseInModel, configFolderPath);
			if (cOMdbLibDatabase != null)
			{
				return cOMdbLibDatabase.DBNetwork.FrameOrPDUs.GetFrameOrPDUByName(messageName);
			}
			return null;
		}

		public IMappedSignal GetCOMdbLibMappedSignal(Vector.VLConfig.Data.ConfigurationDataModel.Database databaseInModel, string configFolderPath, string messageName, string signalName)
		{
			Vector.COMdbLib.IDatabase cOMdbLibDatabase = this.GetCOMdbLibDatabase(databaseInModel, configFolderPath);
			if (cOMdbLibDatabase != null)
			{
				IFrameOrPDU frameOrPDUByName = cOMdbLibDatabase.DBNetwork.FrameOrPDUs.GetFrameOrPDUByName(messageName);
				if (frameOrPDUByName != null)
				{
					return frameOrPDUByName.MappedSignals.GetMappedSignalBySignalName(signalName);
				}
			}
			return null;
		}

		private void symbSel_FormApplied(object sender, FormAppliedEventArgs e)
		{
		}

		private void symbSel_HelpButtonClicked(object sender, CancelEventArgs e)
		{
		}

		public bool SelectMessageInDatabase(ref string messageName, ref string databaseName, ref string databasePath, ref string networkName, ref Vector.VLConfig.Data.ConfigurationDataModel.BusType busType, ref bool isFlexrayPDU)
		{
			string[] array = null;
			string[] array2 = null;
			string[] array3 = null;
			string[] array4 = null;
			Vector.VLConfig.Data.ConfigurationDataModel.BusType[] array5 = null;
			bool[] array6 = null;
			if (this.SelectMessageInDatabase(busType, ref array, ref array3, ref array2, ref array4, ref array5, ref array6, false))
			{
				messageName = array[0];
				databaseName = array3[0];
				databasePath = array2[0];
				networkName = array4[0];
				busType = array5[0];
				isFlexrayPDU = array6[0];
				return true;
			}
			return false;
		}

		public bool SelectMessageInDatabase(Vector.VLConfig.Data.ConfigurationDataModel.BusType busType, ref string[] messageNameList, ref string[] databaseNameList, ref string[] databasePathList, ref string[] networkNameList, ref Vector.VLConfig.Data.ConfigurationDataModel.BusType[] busTypeList, ref bool[] isFlexrayPDUList, bool allowMultiSelection)
		{
			SymbolSelectionDialog symbolSelectionDialog = this.SymbSel;
			this.PrepareSymbolSelectionWithUniqueDatabaseNames(symbolSelectionDialog, busType);
			symbolSelectionDialog.SuspendUpdate();
			symbolSelectionDialog.Options.Hexadecimal = this.isHexDisplay;
			if (busType == Vector.VLConfig.Data.ConfigurationDataModel.BusType.Bt_FlexRay)
			{
				symbolSelectionDialog.SelectableSymbolTypes = (SymbolTypes.Message | SymbolTypes.Pdu);
			}
			else
			{
				symbolSelectionDialog.SelectableSymbolTypes = SymbolTypes.Message;
			}
			symbolSelectionDialog.Options.MultiSelect = allowMultiSelection;
			symbolSelectionDialog.ResumeUpdate();
			if (symbolSelectionDialog.ShowDialog() != DialogResult.OK)
			{
				return false;
			}
			if (symbolSelectionDialog.SelectedItems.Count < 1)
			{
				return false;
			}
			messageNameList = new string[symbolSelectionDialog.SelectedItems.Count];
			databaseNameList = new string[symbolSelectionDialog.SelectedItems.Count];
			databasePathList = new string[symbolSelectionDialog.SelectedItems.Count];
			networkNameList = new string[symbolSelectionDialog.SelectedItems.Count];
			busTypeList = new Vector.VLConfig.Data.ConfigurationDataModel.BusType[symbolSelectionDialog.SelectedItems.Count];
			isFlexrayPDUList = new bool[symbolSelectionDialog.SelectedItems.Count];
			for (int i = 0; i < symbolSelectionDialog.SelectedItems.Count; i++)
			{
				messageNameList[i] = symbolSelectionDialog.SelectedItems[i].MessageName;
				databaseNameList[i] = this.GetActualNameForUniqueDatabaseName(symbolSelectionDialog.SelectedItems[i].DatabaseName);
				if (databaseNameList[i] == "")
				{
					return false;
				}
				databasePathList[i] = this.GetFilePathForUniqueDatabaseName(symbolSelectionDialog.SelectedItems[i].DatabaseName);
				string text = Path.GetExtension(databasePathList[i]) ?? string.Empty;
				string a = text.ToLower();
				if (a == Vocabulary.FileExtensionDotXML || a == Vocabulary.FileExtensionDotARXML)
				{
					networkNameList[i] = symbolSelectionDialog.SelectedItems[i].DatabaseName;
				}
				else
				{
					networkNameList[i] = "";
				}
				busTypeList[i] = busType;
				isFlexrayPDUList[i] = false;
				if (Vector.VLConfig.Data.ConfigurationDataModel.BusType.Bt_FlexRay == busTypeList[i] && SymbolItemType.symPDU == symbolSelectionDialog.SelectedItems[i].ItemType)
				{
					isFlexrayPDUList[i] = true;
				}
			}
			return true;
		}

		public bool ResolveMessageSymbolInDatabase(string databasePath, string networkName, string messageSymbol, Vector.VLConfig.Data.ConfigurationDataModel.BusType busType, out MessageDefinition message)
		{
			message = new MessageDefinition(0u);
			Vector.COMdbLib.IDatabase databaseForFilePathAndNetworkName = this.GetDatabaseForFilePathAndNetworkName(databasePath, networkName);
			if (databaseForFilePathAndNetworkName == null)
			{
				return false;
			}
			IDBNetwork dBNetwork = databaseForFilePathAndNetworkName.DBNetwork;
			if (dBNetwork == null)
			{
				return false;
			}
			if (Vector.VLConfig.Data.ConfigurationDataModel.BusType.Bt_FlexRay != busType)
			{
				IFrameOrPDU frameOrPDUByName = dBNetwork.FrameOrPDUs.GetFrameOrPDUByName(messageSymbol);
				if (frameOrPDUByName != null)
				{
					message.CanDbMessageId = frameOrPDUByName.ID;
					message.DLC = frameOrPDUByName.Bytecount;
					double num = 0.0;
					if (Vector.VLConfig.Data.ConfigurationDataModel.BusType.Bt_CAN == busType)
					{
						double num2 = 0.0;
						if (frameOrPDUByName.Attributes.GetNumberValue(ApplicationDatabaseManager.AttributeGenMsgSendType, ref num2) == AttributeResult.AttributeOK)
						{
							int num3 = (int)num2;
							message.IsCyclic = (num3 == 0 || num3 == 4 || num3 == 5);
						}
						else
						{
							foreach (IMappedSignal current in frameOrPDUByName.MappedSignals)
							{
								if (current.Signal != null && current.Signal.Attributes.GetNumberValue(ApplicationDatabaseManager.AttributeGenSigSendType, ref num2) == AttributeResult.AttributeOK && (int)num2 == 0)
								{
									message.IsCyclic = true;
									break;
								}
							}
						}
						if (frameOrPDUByName.Attributes.GetNumberValue(ApplicationDatabaseManager.AttributeGenMsgCycleTime, ref num) == AttributeResult.AttributeOK)
						{
							message.CycleTime = (int)num;
						}
					}
					else if (Vector.VLConfig.Data.ConfigurationDataModel.BusType.Bt_LIN == busType)
					{
						ILINFrame iLINFrame = (ILINFrame)frameOrPDUByName;
						ILINBus iLINBus = (ILINBus)dBNetwork;
						double num4 = -1.0;
						foreach (ILINScheduleTable current2 in iLINBus.LINScheduleTables)
						{
							double num5 = 0.0;
							bool flag = false;
							foreach (ILINScheduleTableSlot current3 in current2.Slots)
							{
								num5 += current3.SlotDelay;
								if (current3.LINFrame.Name == iLINFrame.Name)
								{
									flag = true;
								}
							}
							if (flag && num5 > 0.0 && (num4 < 0.0 || num5 < num4))
							{
								num4 = num5;
							}
						}
						if (num4 > 0.0)
						{
							message.IsCyclic = true;
							message.CycleTime = (int)num4;
						}
						else
						{
							message.IsCyclic = false;
							message.CycleTime = 0;
						}
					}
					return true;
				}
			}
			else if (dBNetwork is IFlexRayCluster)
			{
				IFlexRayCluster flexRayCluster = dBNetwork as IFlexRayCluster;
				foreach (IFlexRayPDU current4 in flexRayCluster.FlexRayPDUs)
				{
					if (current4.Name == messageSymbol)
					{
						message.CanDbMessageId = current4.ID;
						message.FrBaseCycle = 0;
						message.FrCycleRepetition = 1;
						bool result = true;
						return result;
					}
				}
				foreach (IFlexRayFrame current5 in flexRayCluster.FlexRayFrames)
				{
					if (current5.Name == messageSymbol)
					{
						message.CanDbMessageId = current5.ID;
						message.FrBaseCycle = current5.BaseCycle;
						message.FrCycleRepetition = current5.CycleRepetition;
						bool result = true;
						return result;
					}
				}
				return false;
			}
			return false;
		}

		public bool GetDuplicateMsgIDsOfConfiguredDatabases(IList<Vector.VLConfig.Data.ConfigurationDataModel.Database> databaseList, string configFolderPath, Vector.VLConfig.Data.ConfigurationDataModel.BusType busType, out IDictionary<uint, Dictionary<string, string>> multipleSymNamesInDatabasesForIDs)
		{
			multipleSymNamesInDatabasesForIDs = new Dictionary<uint, Dictionary<string, string>>();
			Dictionary<uint, Dictionary<string, string>> dictionary = new Dictionary<uint, Dictionary<string, string>>();
			new HashSet<uint>();
			foreach (Vector.VLConfig.Data.ConfigurationDataModel.Database current in databaseList)
			{
				Vector.COMdbLib.IDatabase databaseForFilePathAndNetworkName = this.GetDatabaseForFilePathAndNetworkName(FileSystemServices.GetAbsolutePath(current.FilePath.Value, configFolderPath), current.NetworkName.Value);
				if (databaseForFilePathAndNetworkName != null)
				{
					IDBNetwork dBNetwork = databaseForFilePathAndNetworkName.DBNetwork;
					if (dBNetwork != null)
					{
						if (busType == Vector.VLConfig.Data.ConfigurationDataModel.BusType.Bt_CAN && dBNetwork is ICANBus)
						{
							ICANBus iCANBus = dBNetwork as ICANBus;
							using (IEnumerator<ICANFrame> enumerator2 = iCANBus.CANFrames.GetEnumerator())
							{
								while (enumerator2.MoveNext())
								{
									ICANFrame current2 = enumerator2.Current;
									if (!dictionary.ContainsKey(current2.ID))
									{
										dictionary.Add(current2.ID, new Dictionary<string, string>());
									}
									if (!dictionary[current2.ID].ContainsKey(databaseForFilePathAndNetworkName.DatabaseFile))
									{
										dictionary[current2.ID].Add(databaseForFilePathAndNetworkName.DatabaseFile, current2.Name);
									}
								}
								continue;
							}
						}
						if (busType == Vector.VLConfig.Data.ConfigurationDataModel.BusType.Bt_LIN && dBNetwork is ILINBus)
						{
							ILINBus iLINBus = dBNetwork as ILINBus;
							using (IEnumerator<ILINFrame> enumerator3 = iLINBus.LINFrames.GetEnumerator())
							{
								while (enumerator3.MoveNext())
								{
									ILINFrame current3 = enumerator3.Current;
									if (!dictionary.ContainsKey(current3.ID))
									{
										dictionary.Add(current3.ID, new Dictionary<string, string>());
									}
									if (!dictionary[current3.ID].ContainsKey(databaseForFilePathAndNetworkName.DatabaseFile))
									{
										dictionary[current3.ID].Add(databaseForFilePathAndNetworkName.DatabaseFile, current3.Name);
									}
								}
								continue;
							}
						}
						if (busType == Vector.VLConfig.Data.ConfigurationDataModel.BusType.Bt_FlexRay && dBNetwork is IFlexRayCluster)
						{
							IFlexRayCluster flexRayCluster = dBNetwork as IFlexRayCluster;
							foreach (IFlexRayFrame current4 in flexRayCluster.FlexRayFrames)
							{
								if (!dictionary.ContainsKey(current4.ID))
								{
									dictionary.Add(current4.ID, new Dictionary<string, string>());
								}
								if (!dictionary[current4.ID].ContainsKey(databaseForFilePathAndNetworkName.DatabaseFile))
								{
									dictionary[current4.ID].Add(databaseForFilePathAndNetworkName.DatabaseFile, current4.Name);
								}
							}
						}
					}
				}
			}
			foreach (uint current5 in dictionary.Keys)
			{
				if (dictionary[current5].Count > 1)
				{
					multipleSymNamesInDatabasesForIDs.Add(current5, dictionary[current5]);
				}
			}
			return multipleSymNamesInDatabasesForIDs.Count > 0;
		}

		public bool GetFlexrayFrameOrPDUInfo(string databasePath, string networkName, string pduOrFrameName, bool isPDU, out IList<MessageDefinition> affectedFrames, out IList<string> affectedPDUs, out bool isFlexrayDBVersionGreater20)
		{
			affectedFrames = new List<MessageDefinition>();
			affectedPDUs = new List<string>();
			isFlexrayDBVersionGreater20 = true;
			Vector.COMdbLib.IDatabase databaseForFilePathAndNetworkName = this.GetDatabaseForFilePathAndNetworkName(databasePath, networkName);
			if (databaseForFilePathAndNetworkName == null)
			{
				return false;
			}
			if (databaseForFilePathAndNetworkName.FileType != Vector.COMdbLib.DatabaseFileType.kFileTypeXML)
			{
				return false;
			}
			if (databaseForFilePathAndNetworkName.FIBEXVersion.VersionType == FIBEXVersionType.FIBEXVersion_1_1 || databaseForFilePathAndNetworkName.FIBEXVersion.VersionType == FIBEXVersionType.FIBEXVersion_1_2 || databaseForFilePathAndNetworkName.FIBEXVersion.VersionType == FIBEXVersionType.FIBEXVersion_2_0)
			{
				isFlexrayDBVersionGreater20 = false;
			}
			IDBNetwork dBNetwork = databaseForFilePathAndNetworkName.DBNetwork;
			if (dBNetwork == null)
			{
				return false;
			}
			if (!(dBNetwork is IFlexRayCluster))
			{
				return false;
			}
			IFlexRayCluster flexRayCluster = dBNetwork as IFlexRayCluster;
			if (isPDU)
			{
				IFlexRayPDU flexRayPDU = null;
				foreach (IFlexRayPDU current in flexRayCluster.FlexRayPDUs)
				{
					if (current.Name == pduOrFrameName)
					{
						flexRayPDU = current;
						break;
					}
				}
				if (flexRayPDU == null)
				{
					return false;
				}
				foreach (IMappedFlexRayPDU current2 in flexRayPDU.MappedFlexRayPDUs)
				{
					foreach (IAbsolutelyScheduledTiming current3 in current2.FlexRayFrame.AbsolutelyScheduledTimings)
					{
						MessageDefinition messageDefinition = new MessageDefinition((uint)current3.SlotID);
						messageDefinition.FrBaseCycle = current3.BaseCycle;
						messageDefinition.FrCycleRepetition = current3.CycleRepetition;
						messageDefinition.Name = current2.FlexRayFrame.Name;
						affectedFrames.Add(messageDefinition);
					}
					foreach (IMappedFlexRayPDU current4 in current2.FlexRayFrame.MappedFlexRayPDUs)
					{
						if (!(current4.FlexRayPDU.Name == pduOrFrameName) && !affectedPDUs.Contains(current4.FlexRayPDU.Name))
						{
							affectedPDUs.Add(current4.FlexRayPDU.Name);
						}
					}
				}
				return true;
			}
			else
			{
				IFlexRayFrame flexRayFrame = null;
				foreach (IFlexRayFrame current5 in flexRayCluster.FlexRayFrames)
				{
					if (current5.Name == pduOrFrameName)
					{
						flexRayFrame = current5;
						break;
					}
				}
				if (flexRayFrame == null)
				{
					return false;
				}
				foreach (IAbsolutelyScheduledTiming current6 in flexRayFrame.AbsolutelyScheduledTimings)
				{
					MessageDefinition messageDefinition2 = new MessageDefinition((uint)current6.SlotID);
					messageDefinition2.FrBaseCycle = current6.BaseCycle;
					messageDefinition2.FrCycleRepetition = current6.CycleRepetition;
					messageDefinition2.Name = flexRayFrame.Name;
					affectedFrames.Add(messageDefinition2);
				}
				foreach (IMappedFlexRayPDU current7 in flexRayFrame.MappedFlexRayPDUs)
				{
					if (!affectedPDUs.Contains(current7.FlexRayPDU.Name))
					{
						affectedPDUs.Add(current7.FlexRayPDU.Name);
					}
				}
				return true;
			}
		}

		private double GetMaximumValueForBitWidth(uint numberOfBits)
		{
			if (numberOfBits > 32u)
			{
				numberOfBits = 32u;
			}
			return Math.Pow(2.0, numberOfBits) - 1.0;
		}

		public bool PhysicalSignalValueToRawValue(string databasePath, string networkName, string messageSymbol, string signalSymbol, double physicalValue, out double rawValue)
		{
			rawValue = 0.0;
			IMappedSignal mappedSignal = null;
			if (!this.GetMappedSignal(databasePath, networkName, messageSymbol, signalSymbol, out mappedSignal))
			{
				return false;
			}
			Vector.COMdbLib.IValueConversion valueConversion = mappedSignal.Signal.ValueConversion;
			switch (mappedSignal.Signal.ValueType)
			{
			case RuntimeValueType.kSigvalInteger:
				if (mappedSignal.Signal.IsSigned)
				{
					long num = 0L;
					SigvalueResult sigvalueResult = valueConversion.Phys2RawInt64(physicalValue, ref num);
					if (sigvalueResult != SigvalueResult.SigvalueOK && SigvalueResult.SigvalueRangeError != sigvalueResult)
					{
						return false;
					}
					rawValue = (double)num;
					uint numberOfBits = 0u;
					if (mappedSignal.Signal.Bitcount > 0u)
					{
						numberOfBits = mappedSignal.Signal.Bitcount - 1u;
					}
					double maximumValueForBitWidth = this.GetMaximumValueForBitWidth(numberOfBits);
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
					uint num2 = 0u;
					SigvalueResult sigvalueResult = valueConversion.Phys2RawUInt32(physicalValue, ref num2);
					if (sigvalueResult != SigvalueResult.SigvalueOK && SigvalueResult.SigvalueRangeError != sigvalueResult)
					{
						return false;
					}
					rawValue = num2;
					double maximumValueForBitWidth2 = this.GetMaximumValueForBitWidth(mappedSignal.Signal.Bitcount);
					if (rawValue > maximumValueForBitWidth2)
					{
						return false;
					}
				}
				break;
			case RuntimeValueType.kSigvalFloat:
			case RuntimeValueType.kSigvalDouble:
			{
				SigvalueResult sigvalueResult = valueConversion.Phys2RawDouble(physicalValue, ref rawValue);
				if (sigvalueResult != SigvalueResult.SigvalueOK && SigvalueResult.SigvalueRangeError != sigvalueResult)
				{
					return false;
				}
				rawValue = Math.Round(rawValue);
				break;
			}
			default:
				return false;
			}
			return true;
		}

		public bool RawSignalValueToPhysicalValue(string databasePath, string networkName, string messageSymbol, string signalSymbol, double rawValue, out double physicalValue)
		{
			physicalValue = 0.0;
			IMappedSignal mappedSignal = null;
			if (!this.GetMappedSignal(databasePath, networkName, messageSymbol, signalSymbol, out mappedSignal))
			{
				return false;
			}
			Vector.COMdbLib.IValueConversion valueConversion = mappedSignal.Signal.ValueConversion;
			switch (mappedSignal.Signal.ValueType)
			{
			case RuntimeValueType.kSigvalInteger:
				if (mappedSignal.Signal.IsSigned)
				{
					SigvalueResult sigvalueResult = valueConversion.Raw2PhysInt64((long)rawValue, ref physicalValue);
					if (sigvalueResult != SigvalueResult.SigvalueOK)
					{
						return false;
					}
				}
				else
				{
					SigvalueResult sigvalueResult = valueConversion.Raw2PhysUInt32((uint)rawValue, ref physicalValue);
					if (sigvalueResult != SigvalueResult.SigvalueOK)
					{
						return false;
					}
				}
				break;
			case RuntimeValueType.kSigvalFloat:
			case RuntimeValueType.kSigvalDouble:
			{
				SigvalueResult sigvalueResult = valueConversion.Raw2PhysDouble(rawValue, ref physicalValue);
				if (sigvalueResult != SigvalueResult.SigvalueOK)
				{
					return false;
				}
				break;
			}
			default:
				return false;
			}
			return true;
		}

		public bool ResolveSignalSymbolInDatabase(string databasePath, string networkName, string messageSymbol, string signalSymbol, out SignalDefinition signalDefinition)
		{
			SignalDefinition signalDefinition2 = new SignalDefinition();
			signalDefinition = signalDefinition2;
			Vector.COMdbLib.IDatabase databaseForFilePathAndNetworkName = this.GetDatabaseForFilePathAndNetworkName(databasePath, networkName);
			if (databaseForFilePathAndNetworkName == null)
			{
				signalDefinition = signalDefinition2;
				return false;
			}
			IDBNetwork dBNetwork = databaseForFilePathAndNetworkName.DBNetwork;
			if (dBNetwork == null)
			{
				return false;
			}
			IFrameOrPDU frameOrPDUByName = dBNetwork.FrameOrPDUs.GetFrameOrPDUByName(messageSymbol);
			if (frameOrPDUByName != null)
			{
				IMappedSignal mappedSignalBySignalName = frameOrPDUByName.MappedSignals.GetMappedSignalBySignalName(signalSymbol);
				if (mappedSignalBySignalName != null)
				{
					return ApplicationDatabaseManager.CreateSignalDefinition(frameOrPDUByName, mappedSignalBySignalName, out signalDefinition);
				}
			}
			return false;
		}

		private static bool CreateSignalDefinition(IFrameOrPDU frameOrPdu, IMappedSignal mappedSignal, out SignalDefinition signalDefinition)
		{
			signalDefinition = new SignalDefinition();
			if (!ApplicationDatabaseManager.FillSignalDefinition(frameOrPdu, mappedSignal, signalDefinition))
			{
				return false;
			}
			if (mappedSignal.IsMultiplexed)
			{
				SignalDefinition signalDefinition2 = new SignalDefinition();
				uint multiplexorValue;
				if (!ApplicationDatabaseManager.FillSignalDefinitionMultiplexor(mappedSignal, signalDefinition2, out multiplexorValue))
				{
					return false;
				}
				signalDefinition.SetMultiplexor(signalDefinition2, multiplexorValue);
			}
			return true;
		}

		private static bool CreateEthernetSignalDefinition(IFrameOrPDU frameOrPdu, IMappedSignal mappedSignal, IApplicationEndPoint3 applicationEndpoint, string xcpSlaveNamePrefix, out SignalDefinition signalDefinition)
		{
			signalDefinition = new SignalDefinition();
			uint num;
			uint num2;
			string name;
			if (!ApplicationDatabaseManager.GetIpAddressAndPortNumber(applicationEndpoint, out num, out num2, out name))
			{
				return false;
			}
			if (!ApplicationDatabaseManager.CreateSignalDefinition(frameOrPdu, mappedSignal, out signalDefinition))
			{
				return false;
			}
			signalDefinition.Message.Name = name;
			signalDefinition.Message.NamePrefix = xcpSlaveNamePrefix;
			if (mappedSignal.IsMultiplexed)
			{
				SignalDefinition multiplexor = signalDefinition.Multiplexor;
				if (multiplexor == null)
				{
					return false;
				}
				multiplexor.Message.Name = name;
				multiplexor.Message.NamePrefix = xcpSlaveNamePrefix;
			}
			return true;
		}

		private static bool FillSignalDefinition(IFrameOrPDU frameOrPDU, IMappedSignal mappedSignal, SignalDefinition signalDef)
		{
			if (frameOrPDU == null || mappedSignal == null)
			{
				return false;
			}
			bool isMultiRanged = false;
			string unit = mappedSignal.Signal.GetHeuristicalUnit();
			double factor = mappedSignal.Signal.GetHeuristicalFactor();
			double offset = mappedSignal.Signal.GetHeuristicalOffset();
			if (mappedSignal.Signal.LinearEncodingAccess.LinearEncodings.Count<ILinearEncoding>() > 1)
			{
				isMultiRanged = true;
				foreach (ILinearEncoding current in mappedSignal.Signal.LinearEncodingAccess.LinearEncodings)
				{
					if (current.Factor != 1.0 && current.Offset != 0.0)
					{
						unit = current.Unit;
						factor = current.Factor;
						offset = current.Offset;
						break;
					}
				}
			}
			MessageDefinition messageDefinition = new MessageDefinition(frameOrPDU.ID);
			if (ApplicationDatabaseManager.GetModelBusType(frameOrPDU.DBNetwork.Database) == Vector.VLConfig.Data.ConfigurationDataModel.BusType.Bt_FlexRay && frameOrPDU is IFlexRayFrame)
			{
				IFlexRayFrame flexRayFrame = frameOrPDU as IFlexRayFrame;
				messageDefinition.FrBaseCycle = flexRayFrame.BaseCycle;
				messageDefinition.FrCycleRepetition = flexRayFrame.CycleRepetition;
			}
			bool isIntegerType = mappedSignal.Signal.ValueType == RuntimeValueType.kSigvalInteger;
			signalDef.SetSignal(messageDefinition, mappedSignal.Startbit, mappedSignal.Signal.Bitcount, mappedSignal.IsMotorola, mappedSignal.Signal.IsSigned, isIntegerType, unit, factor, offset, isMultiRanged, SignalDefinition.NoUpdateBit, mappedSignal.Signal.HasTextualEncodings, true);
			return true;
		}

		private static bool FillSignalDefinitionMultiplexor(IMappedSignal mappedSignal, SignalDefinition multiplexor, out uint multiplexorValue)
		{
			MessageDefinition messageDefinition = new MessageDefinition(mappedSignal.Multiplexor.FrameOrPDU.ID);
			if (mappedSignal.Multiplexor.FrameOrPDU is IFlexRayFrame)
			{
				IFlexRayFrame flexRayFrame = mappedSignal.Multiplexor.FrameOrPDU as IFlexRayFrame;
				messageDefinition.FrBaseCycle = flexRayFrame.BaseCycle;
				messageDefinition.FrCycleRepetition = flexRayFrame.CycleRepetition;
			}
			bool isIntegerType = mappedSignal.Multiplexor.Signal.ValueType == RuntimeValueType.kSigvalInteger;
			multiplexor.SetSignal(messageDefinition, mappedSignal.Multiplexor.Startbit, mappedSignal.Multiplexor.Signal.Bitcount, mappedSignal.Multiplexor.IsMotorola, mappedSignal.Multiplexor.Signal.IsSigned, isIntegerType);
			multiplexorValue = 0u;
			if (mappedSignal.MultiplexorValueRanges[0u] != null)
			{
				if (mappedSignal.MultiplexorValueRanges[0u].LowerBound != mappedSignal.MultiplexorValueRanges[0u].UpperBound)
				{
					return false;
				}
				multiplexorValue = mappedSignal.MultiplexorValueRanges[0u].LowerBound;
			}
			return true;
		}

		public bool GetTextEncodedSignalValueTable(string databasePath, string networkName, string messageSymbol, string signalSymbol, out Dictionary<double, string> textEncValueTable)
		{
			textEncValueTable = new Dictionary<double, string>();
			Vector.COMdbLib.IDatabase databaseForFilePathAndNetworkName = this.GetDatabaseForFilePathAndNetworkName(databasePath, networkName);
			if (databaseForFilePathAndNetworkName == null)
			{
				return false;
			}
			IDBNetwork dBNetwork = databaseForFilePathAndNetworkName.DBNetwork;
			if (dBNetwork == null)
			{
				return false;
			}
			IFrameOrPDU frameOrPDUByName = dBNetwork.FrameOrPDUs.GetFrameOrPDUByName(messageSymbol);
			if (frameOrPDUByName == null)
			{
				return false;
			}
			IMappedSignal mappedSignalBySignalName = frameOrPDUByName.MappedSignals.GetMappedSignalBySignalName(signalSymbol);
			if (mappedSignalBySignalName == null)
			{
				return false;
			}
			if (mappedSignalBySignalName.Signal.HasTextualEncodings)
			{
				ITextualEncodingSet textualEncodingsSortedByIndex = mappedSignalBySignalName.Signal.TextualEncodingAccess.TextualEncodings.GetTextualEncodingsSortedByIndex(true);
				foreach (ITextualEncoding current in textualEncodingsSortedByIndex)
				{
					if (current.LowerBound != current.UpperBound)
					{
						for (long num = current.LowerBound; num <= current.UpperBound; num += 1L)
						{
							textEncValueTable.Add(Convert.ToDouble(num), current.Text);
						}
					}
					else
					{
						textEncValueTable.Add(Convert.ToDouble(current.LowerBound), current.Text);
					}
				}
				return true;
			}
			return false;
		}

		public bool IsSignalValueTextEncoded(string databasePath, string networkName, string messageSymbol, string signalSymbol, double rawSignalValue, out string textSymbol)
		{
			textSymbol = string.Empty;
			Vector.COMdbLib.IDatabase databaseForFilePathAndNetworkName = this.GetDatabaseForFilePathAndNetworkName(databasePath, networkName);
			if (databaseForFilePathAndNetworkName == null)
			{
				return false;
			}
			IDBNetwork dBNetwork = databaseForFilePathAndNetworkName.DBNetwork;
			if (dBNetwork == null)
			{
				return false;
			}
			IFrameOrPDU frameOrPDUByName = dBNetwork.FrameOrPDUs.GetFrameOrPDUByName(messageSymbol);
			if (frameOrPDUByName == null)
			{
				return false;
			}
			IMappedSignal mappedSignalBySignalName = frameOrPDUByName.MappedSignals.GetMappedSignalBySignalName(signalSymbol);
			if (mappedSignalBySignalName == null)
			{
				return false;
			}
			if (mappedSignalBySignalName.Signal.HasTextualEncodings)
			{
				long num = Convert.ToInt64(rawSignalValue);
				new Dictionary<long, string>();
				ITextualEncodingSet textualEncodingsSortedByIndex = mappedSignalBySignalName.Signal.TextualEncodingAccess.TextualEncodings.GetTextualEncodingsSortedByIndex(true);
				foreach (ITextualEncoding current in textualEncodingsSortedByIndex)
				{
					if (num >= current.LowerBound && num <= current.UpperBound)
					{
						textSymbol = current.Text;
						return true;
					}
				}
				return false;
			}
			return false;
		}

		public bool MapFlexraySignalsFromPduToFrames(string databasePath, string networkName, string pduOrFrameName, string signalName, out IList<SignalDefinition> signalList)
		{
			signalList = new List<SignalDefinition>();
			Vector.COMdbLib.IDatabase databaseForFilePathAndNetworkName = this.GetDatabaseForFilePathAndNetworkName(databasePath, networkName);
			if (databaseForFilePathAndNetworkName == null)
			{
				return false;
			}
			IDBNetwork dBNetwork = databaseForFilePathAndNetworkName.DBNetwork;
			if (dBNetwork == null)
			{
				return false;
			}
			if (!(dBNetwork is IFlexRayCluster))
			{
				return false;
			}
			IFlexRayCluster flexRayCluster = dBNetwork as IFlexRayCluster;
			bool flag = false;
			IFlexRayPDU flexRayPDU = null;
			foreach (IFlexRayPDU current in flexRayCluster.FlexRayPDUs)
			{
				if (current.Name == pduOrFrameName)
				{
					flexRayPDU = current;
					flag = true;
					break;
				}
			}
			if (flag)
			{
				if (flexRayPDU == null)
				{
					return false;
				}
				IMappedSignal mappedSignalBySignalName = flexRayPDU.MappedSignals.GetMappedSignalBySignalName(signalName);
				if (mappedSignalBySignalName == null)
				{
					return false;
				}
				using (IEnumerator<IMappedFlexRayPDU> enumerator2 = flexRayPDU.MappedFlexRayPDUs.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						IMappedFlexRayPDU current2 = enumerator2.Current;
						IMappedFlexRayPDU mappedFlexRayPDUByPDUName = current2.FlexRayFrame.MappedFlexRayPDUs.GetMappedFlexRayPDUByPDUName(pduOrFrameName);
						if (mappedFlexRayPDUByPDUName == null)
						{
							bool result = false;
							return result;
						}
						MessageDefinition messageDefinition = new MessageDefinition(current2.FlexRayFrame.ID);
						messageDefinition.FrBaseCycle = current2.FlexRayFrame.BaseCycle;
						messageDefinition.FrCycleRepetition = current2.FlexRayFrame.CycleRepetition;
						messageDefinition.Name = current2.FlexRayFrame.Name;
						SignalDefinition signalDefinition = new SignalDefinition();
						int updateBit = -1;
						if (!mappedFlexRayPDUByPDUName.GetUpdateBitPosition(ref updateBit).PropertyOK)
						{
							updateBit = -1;
						}
						bool isIntegerType = mappedSignalBySignalName.Signal.ValueType == RuntimeValueType.kSigvalInteger;
						signalDefinition.SetSignal(messageDefinition, mappedSignalBySignalName.Startbit + mappedFlexRayPDUByPDUName.Startbit, mappedSignalBySignalName.Signal.Bitcount, mappedSignalBySignalName.IsMotorola, mappedSignalBySignalName.Signal.IsSigned, isIntegerType, mappedSignalBySignalName.Signal.GetHeuristicalUnit(), mappedSignalBySignalName.Signal.GetHeuristicalFactor(), mappedSignalBySignalName.Signal.GetHeuristicalOffset(), mappedSignalBySignalName.Signal.LinearEncodingAccess.LinearEncodings.Count<ILinearEncoding>() > 1, updateBit, mappedSignalBySignalName.Signal.HasTextualEncodings, true);
						if (mappedSignalBySignalName.IsMultiplexed)
						{
							SignalDefinition signalDefinition2 = new SignalDefinition();
							MessageDefinition messageDefinition2 = new MessageDefinition(mappedSignalBySignalName.Multiplexor.FrameOrPDU.ID);
							if (mappedSignalBySignalName.Multiplexor.FrameOrPDU is IFlexRayFrame)
							{
								IFlexRayFrame flexRayFrame = mappedSignalBySignalName.Multiplexor.FrameOrPDU as IFlexRayFrame;
								messageDefinition2.FrBaseCycle = flexRayFrame.BaseCycle;
								messageDefinition2.FrCycleRepetition = flexRayFrame.CycleRepetition;
							}
							isIntegerType = (mappedSignalBySignalName.Multiplexor.Signal.ValueType == RuntimeValueType.kSigvalInteger);
							signalDefinition2.SetSignal(messageDefinition2, mappedSignalBySignalName.Multiplexor.Startbit + mappedFlexRayPDUByPDUName.Startbit, mappedSignalBySignalName.Multiplexor.Signal.Bitcount, mappedSignalBySignalName.Multiplexor.IsMotorola, mappedSignalBySignalName.Multiplexor.Signal.IsSigned, isIntegerType);
							uint multiplexorValue = 0u;
							if (mappedSignalBySignalName.MultiplexorValueRanges[0u] != null)
							{
								if (mappedSignalBySignalName.MultiplexorValueRanges[0u].LowerBound != mappedSignalBySignalName.MultiplexorValueRanges[0u].UpperBound)
								{
									bool result = false;
									return result;
								}
								multiplexorValue = mappedSignalBySignalName.MultiplexorValueRanges[0u].LowerBound;
							}
							signalDefinition.SetMultiplexor(signalDefinition2, multiplexorValue);
						}
						signalList.Add(signalDefinition);
					}
					return true;
				}
			}
			SignalDefinition item;
			if (!this.ResolveSignalSymbolInDatabase(databasePath, networkName, pduOrFrameName, signalName, out item))
			{
				return false;
			}
			signalList.Add(item);
			return true;
		}

		public bool SelectSignalInDatabase(ref string messageName, ref string signalName, ref string databaseName, ref string databasePath, ref string networkName, ref Vector.VLConfig.Data.ConfigurationDataModel.BusType busType, ref bool isFlexRayPDU)
		{
			SymbolSelectionDialog symbolSelectionDialog = this.SymbSel;
			symbolSelectionDialog.SelectableSymbolTypes = SymbolTypes.Signal;
			symbolSelectionDialog.Options.Hexadecimal = this.isHexDisplay;
			symbolSelectionDialog.Options.MultiSelect = false;
			symbolSelectionDialog.DataSources.COMdbLibDatabases.Clear();
			this.PrepareSymbolSelectionWithUniqueDatabaseNames(symbolSelectionDialog, busType);
			if (symbolSelectionDialog.ShowDialog() != DialogResult.OK)
			{
				return false;
			}
			if (symbolSelectionDialog.SelectedItems.Count == 0)
			{
				return false;
			}
			messageName = symbolSelectionDialog.SelectedItems[0].MessageName;
			databaseName = this.GetActualNameForUniqueDatabaseName(symbolSelectionDialog.SelectedItems[0].DatabaseName);
			if (databaseName == "")
			{
				return false;
			}
			databasePath = this.GetFilePathForUniqueDatabaseName(symbolSelectionDialog.SelectedItems[0].DatabaseName);
			string text = Path.GetExtension(databasePath) ?? string.Empty;
			string a = text.ToLower();
			if (a == Vocabulary.FileExtensionDotXML || a == Vocabulary.FileExtensionDotARXML)
			{
				networkName = symbolSelectionDialog.SelectedItems[0].NetworkName;
			}
			else
			{
				networkName = "";
			}
			signalName = symbolSelectionDialog.SelectedItems[0].SignalName;
			isFlexRayPDU = false;
			if (busType == Vector.VLConfig.Data.ConfigurationDataModel.BusType.Bt_FlexRay)
			{
				Vector.COMdbLib.IDatabase databaseByFileAndPath = this.dbManager.Databases.GetDatabaseByFileAndPath(databasePath);
				if (databaseByFileAndPath != null)
				{
					IDBNetwork dBNetwork = databaseByFileAndPath.DBNetwork;
					if (dBNetwork != null)
					{
						IFrameOrPDU frameOrPDUByName = dBNetwork.FrameOrPDUs.GetFrameOrPDUByName(messageName);
						if (frameOrPDUByName != null)
						{
							isFlexRayPDU = (frameOrPDUByName is IFlexRayPDU);
						}
					}
				}
			}
			return true;
		}

		public bool FlexRayDatabasesContainXcpSlave(string ecuName, string configFolderPath, uint channelNumber = 0u)
		{
			foreach (Vector.VLConfig.Data.ConfigurationDataModel.Database current in this.mDatabaseConfiguration.FlexrayFibexDatabases)
			{
				if (this.GetXcpSlaveNamesOfFlexRayDatabase(current, configFolderPath, channelNumber).Contains(ecuName))
				{
					return true;
				}
			}
			return false;
		}

		public List<string> GetXcpSlaveNamesOfFlexRayDatabase(Vector.VLConfig.Data.ConfigurationDataModel.Database databaseInModel, string configFolderPath, uint channelNumber = 0u)
		{
			List<string> list = new List<string>();
			if (databaseInModel.BusType.Value != Vector.VLConfig.Data.ConfigurationDataModel.BusType.Bt_FlexRay)
			{
				return list;
			}
			Vector.COMdbLib.IDatabase cOMdbLibDatabase = this.GetCOMdbLibDatabase(databaseInModel, configFolderPath);
			if (cOMdbLibDatabase == null || cOMdbLibDatabase.DBNetwork == null || cOMdbLibDatabase.FileType != Vector.COMdbLib.DatabaseFileType.kFileTypeXML || !(cOMdbLibDatabase.DBNetwork is IFlexRayCluster))
			{
				return list;
			}
			uint num;
			uint num2;
			if (channelNumber == Vector.VLConfig.Data.ConfigurationDataModel.Database.ChannelNumber_FlexrayAB || channelNumber == 0u)
			{
				num = 1u;
				num2 = 2u;
			}
			else
			{
				num2 = channelNumber;
				num = channelNumber;
			}
			bool flag = false;
			IFlexRayCluster flexRayCluster = cOMdbLibDatabase.DBNetwork as IFlexRayCluster;
			foreach (IFlexRayNode current in flexRayCluster.FlexRayNodes)
			{
				IFlexRayFrame flexRayFrame = current.RxFlexRayFrames.FirstOrDefault((IFlexRayFrame pdu) => pdu.ServiceType == FlexRayServiceType.FlexRayServiceType_XCP_PreConfigured || pdu.ServiceType == FlexRayServiceType.FlexRayServiceType_XCP_RuntimeConfigured);
				IFlexRayFrame flexRayFrame2 = current.TxFlexRayFrames.FirstOrDefault((IFlexRayFrame pdu) => pdu.ServiceType == FlexRayServiceType.FlexRayServiceType_XCP_PreConfigured || pdu.ServiceType == FlexRayServiceType.FlexRayServiceType_XCP_RuntimeConfigured);
				if (flexRayFrame != null && flexRayFrame2 != null && flexRayFrame.ChannelMask == flexRayFrame2.ChannelMask)
				{
					uint num3 = 1u;
					if (flexRayFrame.ChannelMask == FlexRayChannel.ChannelB)
					{
						num3 = 2u;
					}
					if (num3 >= num && num3 <= num2)
					{
						if (string.Compare(current.Name, ApplicationDatabaseManager.NodeNameXCPMaster, StringComparison.InvariantCultureIgnoreCase) == 0)
						{
							flag = true;
						}
						else
						{
							list.Add(current.Name);
						}
					}
				}
			}
			if (!flag)
			{
				list.Clear();
			}
			return list;
		}

		public CPType GetDatabaseCPConfiguration(Vector.VLConfig.Data.ConfigurationDataModel.Database databaseInModel, string configFolderPath, out IDictionary<string, bool> xcpEcus, out uint flexrayXcpChannel)
		{
			CPType cPType = CPType.None;
			bool value = false;
			Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
			xcpEcus = dictionary;
			flexrayXcpChannel = 0u;
			Vector.COMdbLib.IDatabase database = null;
			OpenDatabaseResult openDatabaseResult = this.OpenDatabase(this.GetAbsoluteFilePath(databaseInModel, configFolderPath), databaseInModel.NetworkName.Value, ref database);
			if (openDatabaseResult != OpenDatabaseResult.OpenSuccess)
			{
				return cPType;
			}
			if (database != null && database.DBNetwork != null)
			{
				if (database.FileType == Vector.COMdbLib.DatabaseFileType.kFileTypeXML)
				{
					if (database.DBNetwork is IFlexRayCluster)
					{
						IFlexRayCluster flexRayCluster = database.DBNetwork as IFlexRayCluster;
						foreach (IFlexRayNode current in flexRayCluster.FlexRayNodes)
						{
							if (string.Compare(current.Name, ApplicationDatabaseManager.NodeNameXCPMaster, true) == 0 && current.XCPECUExtension != null)
							{
								foreach (IXCPLoggingConfiguration current2 in current.XCPECUExtension.LoggingConfigurations)
								{
									foreach (IXCPProtectedResource current3 in current2.ProtectedResources)
									{
										if (current3.Mode == ApplicationDatabaseManager.ProtectedResourceMode_File)
										{
											value = true;
											break;
										}
									}
									IFlexRayFrameOrPDU flexRayFrameOrPDU = null;
									IFlexRayFrameOrPDU flexRayFrameOrPDU2 = null;
									foreach (IXCPAssignedFrame current4 in current2.AssignedFrames)
									{
										if (current4.Item.ServiceType == FlexRayServiceType.FlexRayServiceType_XCP_PreConfigured || current4.Item.ServiceType == FlexRayServiceType.FlexRayServiceType_XCP_RuntimeConfigured)
										{
											if (current4.Type == ApplicationDatabaseManager.InitCmdTypeName)
											{
												flexRayFrameOrPDU = current4.Item;
											}
											else if (current4.Type == ApplicationDatabaseManager.InitResErrTypeName)
											{
												flexRayFrameOrPDU2 = current4.Item;
											}
										}
									}
									if (flexRayFrameOrPDU != null && flexRayFrameOrPDU2 != null && flexRayFrameOrPDU.ChannelMask == flexRayFrameOrPDU2.ChannelMask)
									{
										flexrayXcpChannel = 1u;
										if (flexRayFrameOrPDU.ChannelMask == FlexRayChannel.ChannelB)
										{
											flexrayXcpChannel = 2u;
										}
										cPType = CPType.XCP;
										dictionary.Add(current2.ID, value);
									}
								}
							}
						}
					}
					this.CloseDatabase(database);
					return cPType;
				}
				foreach (INode current5 in database.DBNetwork.Nodes)
				{
					string empty = string.Empty;
					if (current5.Attributes.GetStringValue(ApplicationDatabaseManager.AttributeCCPVersion, ref empty) == AttributeResult.AttributeOK && !string.IsNullOrEmpty(empty))
					{
						if (empty.Equals("0101"))
						{
							cPType = CPType.CCP101;
						}
						else
						{
							cPType = CPType.CCP;
						}
						string attributeName = ApplicationDatabaseManager.AttributeCCPResourceProtectionStatus;
						if (current5.Attributes.GetStringValue(ApplicationDatabaseManager.AttributeXCPRevision, ref empty) == AttributeResult.AttributeOK && !string.IsNullOrEmpty(empty))
						{
							cPType = CPType.XCP;
							attributeName = ApplicationDatabaseManager.AttributeXCPResourceProtectionStatus;
						}
						double value2 = 0.0;
						if (current5.Attributes.GetNumberValue(attributeName, ref value2) == AttributeResult.AttributeOK)
						{
							int num = Convert.ToInt32(value2);
							if (num > 0 && ((CPType.CCP == cPType && (num & ApplicationDatabaseManager.ProtectionStatusCCPMask) == ApplicationDatabaseManager.ProtectionStatusCCPMask) || (CPType.XCP == cPType && (num & ApplicationDatabaseManager.ProtectionStatusXCPMask) == ApplicationDatabaseManager.ProtectionStatusXCPMask)))
							{
								value = true;
							}
						}
						string key = string.Empty;
						if (database.DBNetwork.FrameOrPDUs != null)
						{
							foreach (IFrameOrPDU current6 in database.DBNetwork.FrameOrPDUs)
							{
								if (string.Compare(current6.Name, "DTO", true, CultureInfo.InvariantCulture) == 0 && current6.TxNodes != null && current6.TxNodes.Any<INode>())
								{
									key = current6.TxNodes.First<INode>().Name;
								}
							}
						}
						dictionary.Add(key, value);
						break;
					}
				}
			}
			this.CloseDatabase(database);
			return cPType;
		}

		public bool GetShortestCPTimeout(Vector.VLConfig.Data.ConfigurationDataModel.Database databaseInModel, string configFolderPath, out uint minCPTimeout_ms)
		{
			minCPTimeout_ms = 0u;
			Vector.COMdbLib.IDatabase databaseForFilePathAndNetworkName = this.GetDatabaseForFilePathAndNetworkName(FileSystemServices.GetAbsolutePath(databaseInModel.FilePath.Value, configFolderPath), databaseInModel.NetworkName.Value);
			if (databaseForFilePathAndNetworkName == null)
			{
				return false;
			}
			if (databaseForFilePathAndNetworkName.DBNetwork == null)
			{
				return false;
			}
			string value = "";
			bool flag = false;
			if (databaseForFilePathAndNetworkName.DBNetwork is IFlexRayCluster)
			{
				uint num = 0u;
				IFlexRayCluster flexRayCluster = databaseForFilePathAndNetworkName.DBNetwork as IFlexRayCluster;
				if (flexRayCluster.Database.FIBEXVersion.VersionType == FIBEXVersionType.FIBEXVersion_3_0 || flexRayCluster.Database.FIBEXVersion.VersionType == FIBEXVersionType.FIBEXVersion_3_1)
				{
					return false;
				}
				using (IEnumerator<IFlexRayNode> enumerator = flexRayCluster.FlexRayNodes.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						IFlexRayNode current = enumerator.Current;
						if (string.Compare(current.Name, ApplicationDatabaseManager.NodeNameXCPMaster, true) == 0 && current.XCPECUExtension != null)
						{
							foreach (IXCPLoggingConfiguration current2 in current.XCPECUExtension.LoggingConfigurations)
							{
								IOptionalPropertyResult timeout = current2.GetTimeout(ref num);
								if (timeout.PropertyOK && (!flag || num < minCPTimeout_ms))
								{
									minCPTimeout_ms = num;
									flag = true;
								}
							}
						}
					}
					return flag;
				}
			}
			if (databaseForFilePathAndNetworkName.DBNetwork is ICANBus)
			{
				double num2 = 0.0;
				ICANBus iCANBus = databaseForFilePathAndNetworkName.DBNetwork as ICANBus;
				foreach (ICANNode current3 in iCANBus.CANNodes)
				{
					if (current3.Attributes.GetStringValue(ApplicationDatabaseManager.AttributeCCPVersion, ref value) == AttributeResult.AttributeOK && !string.IsNullOrEmpty(value) && current3.Attributes.GetNumberValue(ApplicationDatabaseManager.AttributeCCPTimeout, ref num2) == AttributeResult.AttributeOK && (!flag || (uint)num2 < minCPTimeout_ms))
					{
						minCPTimeout_ms = (uint)num2;
						flag = true;
					}
				}
			}
			return flag;
		}

		public bool SetRuntimeInformationInCcpXcpEvents(IEnumerable<Event> activeEvents, DatabaseConfiguration databaseConfiguration)
		{
			List<CcpXcpSignalEvent> list = new List<CcpXcpSignalEvent>();
			list.AddRange(activeEvents.OfType<CcpXcpSignalEvent>());
			foreach (CcpXcpSignalEvent current in list)
			{
				current.SignalNameInGeneratedDatabase = null;
				current.SignalDefinitionInGeneratedDatabase = null;
				current.BusType = null;
				current.ChannelNumber = null;
				current.DatabasePath = null;
				current.DatabaseName = null;
				current.NetworkName = null;
				current.MessageName = null;
			}
			foreach (Vector.VLConfig.Data.ConfigurationDataModel.Database current2 in databaseConfiguration.ActiveCCPXCPDatabases)
			{
				if (current2.FileType == Vector.VLConfig.Data.ConfigurationDataModel.DatabaseFileType.A2L)
				{
					for (int i = 0; i < current2.CcpXcpEcuList.Count; i++)
					{
						if (i > 0)
						{
							break;
						}
						CcpXcpEcu ecu = current2.CcpXcpEcuList[i];
						if (!string.IsNullOrEmpty(ecu.GeneratedDbcOrFibexFile))
						{
							List<CcpXcpSignalEvent> list2 = new List<CcpXcpSignalEvent>(from ccpXcpSignalEvent in list
							where ccpXcpSignalEvent.CcpXcpEcuName.Value != null && ccpXcpSignalEvent.CcpXcpEcuName.Value.Equals(ecu.CcpXcpEcuDisplayName, StringComparison.InvariantCultureIgnoreCase)
							select ccpXcpSignalEvent);
							if (list2.Any<CcpXcpSignalEvent>())
							{
								if (current2.BusType.Value == Vector.VLConfig.Data.ConfigurationDataModel.BusType.Bt_CAN)
								{
									Vector.COMdbLib.IDatabase database = null;
									if (this.OpenDatabase(ecu.GeneratedDbcOrFibexFile, ecu.NetworkName, ref database) == OpenDatabaseResult.OpenSuccess)
									{
										INode nodeByName = database.DBNetwork.Nodes.GetNodeByName(ecu.CcpXcpEcuDisplayName);
										if (nodeByName != null)
										{
											foreach (IFrameOrPDU current3 in nodeByName.TxFrameOrPDUs)
											{
												foreach (IMappedSignal current4 in current3.MappedSignals)
												{
													Vector.COMdbLib.ISignal signal = current4.Signal;
													string text = string.Empty;
													if (signal.Attributes.GetStringValue("SignalLongName", ref text) != AttributeResult.AttributeOK || string.IsNullOrEmpty(text))
													{
														text = signal.Name;
													}
													if (!string.IsNullOrEmpty(text))
													{
														foreach (CcpXcpSignalEvent current5 in list2)
														{
															SignalDefinition signalDefinitionInGeneratedDatabase;
															if (text.Equals(current5.SignalName.Value, StringComparison.InvariantCulture) && ApplicationDatabaseManager.CreateSignalDefinition(current3, current4, out signalDefinitionInGeneratedDatabase))
															{
																current5.SignalNameInGeneratedDatabase = signal.Name;
																current5.SignalDefinitionInGeneratedDatabase = signalDefinitionInGeneratedDatabase;
																current5.BusType = new ValidatedProperty<Vector.VLConfig.Data.ConfigurationDataModel.BusType>(current2.BusType.Value);
																current5.ChannelNumber = new ValidatedProperty<uint>(current2.ChannelNumber.Value);
																current5.DatabasePath = new ValidatedProperty<string>(ecu.GeneratedDbcOrFibexFile);
																current5.DatabaseName = new ValidatedProperty<string>(database.DBNetwork.Name);
																current5.NetworkName = new ValidatedProperty<string>(database.DBNetwork.Name);
																current5.MessageName = new ValidatedProperty<string>(current3.Name);
															}
														}
													}
												}
											}
										}
										this.CloseDatabase(database);
									}
								}
								else if (current2.BusType.Value == Vector.VLConfig.Data.ConfigurationDataModel.BusType.Bt_FlexRay)
								{
									Vector.COMdbLib.IDatabase database2 = null;
									if (this.OpenDatabase(ecu.GeneratedDbcOrFibexFile, ecu.NetworkName, ref database2) == OpenDatabaseResult.OpenSuccess)
									{
										INode nodeByName2 = database2.DBNetwork.Nodes.GetNodeByName(ecu.CcpXcpEcuDisplayName);
										if (nodeByName2 != null)
										{
											foreach (IFrameOrPDU current6 in nodeByName2.TxFrameOrPDUs)
											{
												foreach (IMappedSignal current7 in current6.MappedSignals)
												{
													Vector.COMdbLib.ISignal signal2 = current7.Signal;
													string text2 = signal2.Name;
													if (!string.IsNullOrEmpty(signal2.Comment))
													{
														int num = signal2.Comment.LastIndexOf("$original_name:", StringComparison.InvariantCultureIgnoreCase);
														if (num >= 0)
														{
															text2 = signal2.Comment.Substring(num + "$original_name:".Length);
														}
													}
													if (!string.IsNullOrEmpty(text2))
													{
														foreach (CcpXcpSignalEvent current8 in list2)
														{
															SignalDefinition signalDefinitionInGeneratedDatabase2;
															if (text2.Equals(current8.SignalName.Value, StringComparison.InvariantCulture) && ApplicationDatabaseManager.CreateSignalDefinition(current6, current7, out signalDefinitionInGeneratedDatabase2))
															{
																current8.SignalNameInGeneratedDatabase = signal2.Name;
																current8.SignalDefinitionInGeneratedDatabase = signalDefinitionInGeneratedDatabase2;
																current8.BusType = new ValidatedProperty<Vector.VLConfig.Data.ConfigurationDataModel.BusType>(current2.BusType.Value);
																current8.ChannelNumber = new ValidatedProperty<uint>(current2.ExtraCPChannel);
																current8.DatabasePath = new ValidatedProperty<string>(ecu.GeneratedDbcOrFibexFile);
																current8.DatabaseName = new ValidatedProperty<string>(database2.DBNetwork.Name);
																current8.NetworkName = new ValidatedProperty<string>(ecu.NetworkName);
																current8.MessageName = new ValidatedProperty<string>(current6.Name);
															}
														}
													}
												}
											}
										}
										this.CloseDatabase(database2);
									}
								}
								else if (current2.BusType.Value == Vector.VLConfig.Data.ConfigurationDataModel.BusType.Bt_Ethernet)
								{
									Vector.COMdbLib.IDatabase database3 = null;
									if (this.OpenDatabase(ecu.GeneratedDbcOrFibexFile, "Ethernet1", ref database3) == OpenDatabaseResult.OpenSuccess)
									{
										INode nodeByName3 = database3.DBNetwork.Nodes.GetNodeByName(ecu.CcpXcpEcuDisplayName);
										if (nodeByName3 is IEthernetNode3)
										{
											IEthernetNode3 ethernetNode = nodeByName3 as IEthernetNode3;
											foreach (IApplicationEndPoint current9 in ethernetNode.TxApplicationEndPoints)
											{
												if (current9 is IApplicationEndPoint3)
												{
													uint num2 = 0u;
													uint num3 = 0u;
													IApplicationEndPoint3 applicationEndPoint = current9 as IApplicationEndPoint3;
													string serializationTechnology = applicationEndPoint.SerializationTechnology;
													if (string.CompareOrdinal(serializationTechnology, "XCP") == 0)
													{
														string text3;
														ApplicationDatabaseManager.GetIpAddressAndPortNumber(applicationEndPoint, out num2, out num3, out text3);
													}
													IPAddress iPAddress;
													if (IPAddress.TryParse(ecu.EthernetHost, out iPAddress) && iPAddress.Equals(new IPAddress((long)((ulong)num2))) && ecu.EthernetPort == num3)
													{
														foreach (IEthernetFrame current10 in applicationEndPoint.TxEthernetFrames)
														{
															if (current10.MappedEthernetPDUs != null)
															{
																foreach (IMappedEthernetPDU current11 in current10.MappedEthernetPDUs)
																{
																	IEthernetPDU ethernetPDU = current11.EthernetPDU;
																	if (ethernetPDU != null && ethernetPDU.MappedSignals != null)
																	{
																		foreach (IMappedSignal current12 in ethernetPDU.MappedSignals)
																		{
																			Vector.COMdbLib.ISignal signal3 = current12.Signal;
																			string text4 = signal3.Name;
																			if (!string.IsNullOrEmpty(signal3.Comment))
																			{
																				int num4 = signal3.Comment.LastIndexOf("$original_name:", StringComparison.InvariantCultureIgnoreCase);
																				if (num4 >= 0)
																				{
																					text4 = signal3.Comment.Substring(num4 + "$original_name:".Length);
																				}
																			}
																			if (!string.IsNullOrEmpty(text4))
																			{
																				foreach (CcpXcpSignalEvent current13 in list2)
																				{
																					SignalDefinition signalDefinitionInGeneratedDatabase3;
																					if (text4.Equals(current13.SignalName.Value, StringComparison.InvariantCulture) && ApplicationDatabaseManager.CreateEthernetSignalDefinition(ethernetPDU, current12, applicationEndPoint, current2.CcpXcpSlaveNamePrefix, out signalDefinitionInGeneratedDatabase3))
																					{
																						current13.SignalNameInGeneratedDatabase = signal3.Name;
																						current13.SignalDefinitionInGeneratedDatabase = signalDefinitionInGeneratedDatabase3;
																						current13.BusType = new ValidatedProperty<Vector.VLConfig.Data.ConfigurationDataModel.BusType>(current2.BusType.Value);
																						current13.ChannelNumber = new ValidatedProperty<uint>(1u);
																						current13.DatabasePath = new ValidatedProperty<string>(ecu.GeneratedDbcOrFibexFile);
																						current13.DatabaseName = new ValidatedProperty<string>(database3.DBNetwork.Name);
																						current13.NetworkName = new ValidatedProperty<string>(ecu.NetworkName);
																						current13.MessageName = new ValidatedProperty<string>(current10.Name);
																					}
																				}
																			}
																		}
																	}
																}
															}
														}
													}
												}
											}
										}
										this.CloseDatabase(database3);
									}
								}
							}
						}
					}
				}
			}
			return list.All((CcpXcpSignalEvent x) => x.SignalDefinitionInGeneratedDatabase != null);
		}

		private static bool GetIpAddressAndPortNumber(IApplicationEndPoint3 applicationEndPoint, out uint comIpAddress, out uint comPortNumber, out string ethFrameName)
		{
			comIpAddress = 0u;
			comPortNumber = 0u;
			ethFrameName = null;
			if (applicationEndPoint.EthernetTransportProtocolConfiguration is IEthernetTransportProtocolConfiguration2 && applicationEndPoint.TxEthernetFrames.Any<IEthernetFrame>())
			{
				IEthernetTransportProtocolConfiguration2 ethernetTransportProtocolConfiguration = applicationEndPoint.EthernetTransportProtocolConfiguration as IEthernetTransportProtocolConfiguration2;
				if (applicationEndPoint.EthernetNetworkEndpointAddress is IEthernetNetworkEndpointIPv4Address2)
				{
					IEthernetNetworkEndpointIPv4Address2 ethernetNetworkEndpointIPv4Address = applicationEndPoint.EthernetNetworkEndpointAddress as IEthernetNetworkEndpointIPv4Address2;
					if (ethernetNetworkEndpointIPv4Address.EthernetIPv4Address != null)
					{
						ethFrameName = applicationEndPoint.TxEthernetFrames.First<IEthernetFrame>().Name;
						comIpAddress = ethernetNetworkEndpointIPv4Address.EthernetIPv4Address.IPv4AddressAsUINT;
						comIpAddress = ((comIpAddress & 255u) << 24) + ((comIpAddress & 65280u) << 8) + ((comIpAddress & 16711680u) >> 8) + ((comIpAddress & 4278190080u) >> 24);
						comPortNumber = ethernetTransportProtocolConfiguration.PortNumber;
						return true;
					}
				}
			}
			return false;
		}

		public bool GetAllMessageIdsWithNames(Vector.VLConfig.Data.ConfigurationDataModel.Database databaseInModel, string configFolderPath, out IDictionary<uint, string> idsAndNames)
		{
			idsAndNames = new Dictionary<uint, string>();
			Vector.COMdbLib.IDatabase databaseForFilePathAndNetworkName = this.GetDatabaseForFilePathAndNetworkName(this.GetAbsoluteFilePath(databaseInModel, configFolderPath), databaseInModel.NetworkName.Value);
			if (databaseForFilePathAndNetworkName == null)
			{
				return false;
			}
			IDBNetwork dBNetwork = databaseForFilePathAndNetworkName.DBNetwork;
			if (dBNetwork == null)
			{
				return false;
			}
			foreach (IFrameOrPDU current in dBNetwork.FrameOrPDUs)
			{
				idsAndNames.Add(current.ID, current.Name);
			}
			return true;
		}

		public bool FindMessagesForSignalName(DatabaseConfiguration databaseConfiguration, string configFolderPath, string signalName, Vector.VLConfig.Data.ConfigurationDataModel.BusType busType, uint channelNumber, out IDictionary<string, MessageDefinition> messages)
		{
			messages = new Dictionary<string, MessageDefinition>();
			bool result = false;
			foreach (Vector.VLConfig.Data.ConfigurationDataModel.Database current in databaseConfiguration.Databases)
			{
				if (current.BusType.Value == busType && current.ChannelNumber.Value == channelNumber)
				{
					Vector.COMdbLib.IDatabase cOMdbLibDatabase = this.GetCOMdbLibDatabase(current, configFolderPath);
					if (cOMdbLibDatabase != null)
					{
						IDBNetwork dBNetwork = cOMdbLibDatabase.DBNetwork;
						if (dBNetwork != null)
						{
							foreach (IFrameOrPDU current2 in dBNetwork.FrameOrPDUs)
							{
								foreach (IMappedSignal current3 in current2.MappedSignals)
								{
									if (current3.Signal.Name == signalName)
									{
										messages.Add(current2.Name, new MessageDefinition(current2.ID));
										result = true;
									}
								}
							}
						}
					}
				}
			}
			return result;
		}

		public void Subscribe(IDatabaseAccessObserver observer)
		{
			this.mDatabaseAccessObservers.Add(observer);
		}

		public void Unsubscribe(IDatabaseAccessObserver observer)
		{
			this.mDatabaseAccessObservers.Remove(observer);
		}

		public bool ResolveGPSMessageSymbolsInDatabase(string databasePath, out uint[] canIDs, out int modeLatitudeLongitude, out double factorAltitude)
		{
			uint[] array = new uint[3];
			canIDs = array;
			Vector.COMdbLib.IDatabase database = null;
			OpenDatabaseResult openDatabaseResult = this.OpenDatabase(databasePath, "", ref database);
			factorAltitude = 1.0;
			modeLatitudeLongitude = 1;
			if (openDatabaseResult == OpenDatabaseResult.OpenSuccess && database != null)
			{
				IDBNetwork dBNetwork = database.DBNetwork;
				if (dBNetwork != null)
				{
					IMappedSignalSet mappedSignalsBySignalName = dBNetwork.MappedSignals.GetMappedSignalsBySignalName(ApplicationDatabaseManager.GPSSignalCANoeAltitude);
					if (mappedSignalsBySignalName == null || mappedSignalsBySignalName.Count == 0u)
					{
						mappedSignalsBySignalName = dBNetwork.MappedSignals.GetMappedSignalsBySignalName(ApplicationDatabaseManager.GPSSignalCANoeAltitudeDE);
					}
					if (mappedSignalsBySignalName == null || mappedSignalsBySignalName.Count == 0u)
					{
						mappedSignalsBySignalName = dBNetwork.MappedSignals.GetMappedSignalsBySignalName(ApplicationDatabaseManager.GPSSignalCANapeAltitude);
					}
					foreach (IMappedSignal current in mappedSignalsBySignalName)
					{
						if ((current.FrameOrPDU.MappedSignals.GetMappedSignalBySignalName(ApplicationDatabaseManager.GPSSignalSeconds) != null || current.FrameOrPDU.MappedSignals.GetMappedSignalBySignalName(ApplicationDatabaseManager.GPSSignalSecondsDE) != null) && (current.FrameOrPDU.MappedSignals.GetMappedSignalBySignalName(ApplicationDatabaseManager.GPSSignalMinutes) != null || current.FrameOrPDU.MappedSignals.GetMappedSignalBySignalName(ApplicationDatabaseManager.GPSSignalMinutesDE) != null) && (current.FrameOrPDU.MappedSignals.GetMappedSignalBySignalName(ApplicationDatabaseManager.GPSSignalHours) != null || current.FrameOrPDU.MappedSignals.GetMappedSignalBySignalName(ApplicationDatabaseManager.GPSSignalHoursDE) != null) && (current.FrameOrPDU.MappedSignals.GetMappedSignalBySignalName(ApplicationDatabaseManager.GPSSignalYear) != null || current.FrameOrPDU.MappedSignals.GetMappedSignalBySignalName(ApplicationDatabaseManager.GPSSignalYearDE) != null) && (current.FrameOrPDU.MappedSignals.GetMappedSignalBySignalName(ApplicationDatabaseManager.GPSSignalMonth) != null || current.FrameOrPDU.MappedSignals.GetMappedSignalBySignalName(ApplicationDatabaseManager.GPSSignalMonthDE) != null) && (current.FrameOrPDU.MappedSignals.GetMappedSignalBySignalName(ApplicationDatabaseManager.GPSSignalDay) != null || current.FrameOrPDU.MappedSignals.GetMappedSignalBySignalName(ApplicationDatabaseManager.GPSSignalDayDE) != null))
						{
							if (current.FrameOrPDU.ID > 0u && current.FrameOrPDU.ID <= 2016u)
							{
								canIDs[0] = current.FrameOrPDU.ID;
							}
							IMappedSignal mappedSignalBySignalName = current.FrameOrPDU.MappedSignals.GetMappedSignalBySignalName(ApplicationDatabaseManager.GPSSignalCANoeAltitude);
							if (mappedSignalBySignalName == null)
							{
								mappedSignalBySignalName = current.FrameOrPDU.MappedSignals.GetMappedSignalBySignalName(ApplicationDatabaseManager.GPSSignalCANoeAltitudeDE);
							}
							if (mappedSignalBySignalName == null)
							{
								mappedSignalBySignalName = current.FrameOrPDU.MappedSignals.GetMappedSignalBySignalName(ApplicationDatabaseManager.GPSSignalCANapeAltitude);
							}
							if (mappedSignalBySignalName != null && mappedSignalBySignalName.FieldDef.LinearEncodingAccess.LinearEncodings.Count > 0u)
							{
								factorAltitude = mappedSignalBySignalName.FieldDef.LinearEncodingAccess.LinearEncodings[0u].Factor;
								break;
							}
							break;
						}
					}
					mappedSignalsBySignalName = dBNetwork.MappedSignals.GetMappedSignalsBySignalName(ApplicationDatabaseManager.GPSSignalCANoeLongitude);
					if (mappedSignalsBySignalName == null || mappedSignalsBySignalName.Count == 0u)
					{
						mappedSignalsBySignalName = dBNetwork.MappedSignals.GetMappedSignalsBySignalName(ApplicationDatabaseManager.GPSSignalCANoeLongitudeDE);
					}
					if (mappedSignalsBySignalName == null || mappedSignalsBySignalName.Count == 0u)
					{
						mappedSignalsBySignalName = dBNetwork.MappedSignals.GetMappedSignalsBySignalName(ApplicationDatabaseManager.GPSSignalCANapeLongitude);
					}
					if (mappedSignalsBySignalName == null || mappedSignalsBySignalName.Count == 0u)
					{
						mappedSignalsBySignalName = dBNetwork.MappedSignals.GetMappedSignalsBySignalName(ApplicationDatabaseManager.GPSSignalCANoeLongitudeSi);
						if (mappedSignalsBySignalName != null)
						{
							modeLatitudeLongitude = 3;
						}
					}
					if (mappedSignalsBySignalName == null || mappedSignalsBySignalName.Count == 0u)
					{
						mappedSignalsBySignalName = dBNetwork.MappedSignals.GetMappedSignalsBySignalName(ApplicationDatabaseManager.GPSSignalCANoeLongitudeSiDE);
						if (mappedSignalsBySignalName != null)
						{
							modeLatitudeLongitude = 3;
						}
					}
					if (mappedSignalsBySignalName == null || mappedSignalsBySignalName.Count == 0u)
					{
						mappedSignalsBySignalName = dBNetwork.MappedSignals.GetMappedSignalsBySignalName(ApplicationDatabaseManager.GPSSignalCANoeLongitudeD);
						if (mappedSignalsBySignalName != null)
						{
							modeLatitudeLongitude = 2;
						}
					}
					if (mappedSignalsBySignalName == null || mappedSignalsBySignalName.Count == 0u)
					{
						mappedSignalsBySignalName = dBNetwork.MappedSignals.GetMappedSignalsBySignalName(ApplicationDatabaseManager.GPSSignalCANoeLongitudeDDE);
						if (mappedSignalsBySignalName != null)
						{
							modeLatitudeLongitude = 2;
						}
					}
					if (mappedSignalsBySignalName != null)
					{
						foreach (IMappedSignal current2 in mappedSignalsBySignalName)
						{
							if (modeLatitudeLongitude == 1)
							{
								if (current2.FrameOrPDU.MappedSignals.GetMappedSignalBySignalName(ApplicationDatabaseManager.GPSSignalCANoeLatitude) != null || current2.FrameOrPDU.MappedSignals.GetMappedSignalBySignalName(ApplicationDatabaseManager.GPSSignalCANoeLatitudeDE) != null || current2.FrameOrPDU.MappedSignals.GetMappedSignalBySignalName(ApplicationDatabaseManager.GPSSignalCANapeLatitude) != null)
								{
									if (current2.FrameOrPDU.ID > 0u && current2.FrameOrPDU.ID <= 2016u)
									{
										canIDs[1] = current2.FrameOrPDU.ID;
										break;
									}
									break;
								}
							}
							else if (modeLatitudeLongitude == 2)
							{
								if ((current2.FrameOrPDU.MappedSignals.GetMappedSignalBySignalName(ApplicationDatabaseManager.GPSSignalCANoeLongitudeM) != null || current2.FrameOrPDU.MappedSignals.GetMappedSignalBySignalName(ApplicationDatabaseManager.GPSSignalCANoeLongitudeMDE) != null) && (current2.FrameOrPDU.MappedSignals.GetMappedSignalBySignalName(ApplicationDatabaseManager.GPSSignalCANoeLongitudeS) != null || current2.FrameOrPDU.MappedSignals.GetMappedSignalBySignalName(ApplicationDatabaseManager.GPSSignalCANoeLongitudeSDE) != null) && (current2.FrameOrPDU.MappedSignals.GetMappedSignalBySignalName(ApplicationDatabaseManager.GPSSignalCANoeLatitudeD) != null || current2.FrameOrPDU.MappedSignals.GetMappedSignalBySignalName(ApplicationDatabaseManager.GPSSignalCANoeLatitudeDDE) != null) && (current2.FrameOrPDU.MappedSignals.GetMappedSignalBySignalName(ApplicationDatabaseManager.GPSSignalCANoeLatitudeM) != null || current2.FrameOrPDU.MappedSignals.GetMappedSignalBySignalName(ApplicationDatabaseManager.GPSSignalCANoeLatitudeMDE) != null) && (current2.FrameOrPDU.MappedSignals.GetMappedSignalBySignalName(ApplicationDatabaseManager.GPSSignalCANoeLatitudeS) != null || current2.FrameOrPDU.MappedSignals.GetMappedSignalBySignalName(ApplicationDatabaseManager.GPSSignalCANoeLatitudeSDE) != null))
								{
									if (current2.FrameOrPDU.ID > 0u && current2.FrameOrPDU.ID <= 2016u)
									{
										canIDs[1] = current2.FrameOrPDU.ID;
										break;
									}
									break;
								}
							}
							else if (modeLatitudeLongitude == 3 && (current2.FrameOrPDU.MappedSignals.GetMappedSignalBySignalName(ApplicationDatabaseManager.GPSSignalCANoeLongitudeD) != null || current2.FrameOrPDU.MappedSignals.GetMappedSignalBySignalName(ApplicationDatabaseManager.GPSSignalCANoeLongitudeMDE) != null) && (current2.FrameOrPDU.MappedSignals.GetMappedSignalBySignalName(ApplicationDatabaseManager.GPSSignalCANoeLongitudeM) != null || current2.FrameOrPDU.MappedSignals.GetMappedSignalBySignalName(ApplicationDatabaseManager.GPSSignalCANoeLongitudeMDE) != null) && (current2.FrameOrPDU.MappedSignals.GetMappedSignalBySignalName(ApplicationDatabaseManager.GPSSignalCANoeLongitudeS) != null || current2.FrameOrPDU.MappedSignals.GetMappedSignalBySignalName(ApplicationDatabaseManager.GPSSignalCANoeLongitudeSDE) != null) && (current2.FrameOrPDU.MappedSignals.GetMappedSignalBySignalName(ApplicationDatabaseManager.GPSSignalCANoeLatitudeD) != null || current2.FrameOrPDU.MappedSignals.GetMappedSignalBySignalName(ApplicationDatabaseManager.GPSSignalCANoeLatitudeDDE) != null) && (current2.FrameOrPDU.MappedSignals.GetMappedSignalBySignalName(ApplicationDatabaseManager.GPSSignalCANoeLatitudeSi) != null || current2.FrameOrPDU.MappedSignals.GetMappedSignalBySignalName(ApplicationDatabaseManager.GPSSignalCANoeLatitudeSiDE) != null) && (current2.FrameOrPDU.MappedSignals.GetMappedSignalBySignalName(ApplicationDatabaseManager.GPSSignalCANoeLatitudeM) != null || current2.FrameOrPDU.MappedSignals.GetMappedSignalBySignalName(ApplicationDatabaseManager.GPSSignalCANoeLatitudeMDE) != null) && (current2.FrameOrPDU.MappedSignals.GetMappedSignalBySignalName(ApplicationDatabaseManager.GPSSignalCANoeLatitudeS) != null || current2.FrameOrPDU.MappedSignals.GetMappedSignalBySignalName(ApplicationDatabaseManager.GPSSignalCANoeLatitudeSDE) != null))
							{
								if (current2.FrameOrPDU.ID > 0u && current2.FrameOrPDU.ID <= 2016u)
								{
									canIDs[1] = current2.FrameOrPDU.ID;
									break;
								}
								break;
							}
						}
					}
					mappedSignalsBySignalName = dBNetwork.MappedSignals.GetMappedSignalsBySignalName(ApplicationDatabaseManager.GPSSignalCANoeSpeed);
					if (mappedSignalsBySignalName == null || mappedSignalsBySignalName.Count == 0u)
					{
						mappedSignalsBySignalName = dBNetwork.MappedSignals.GetMappedSignalsBySignalName(ApplicationDatabaseManager.GPSSignalCANoeSpeedDE);
					}
					if (mappedSignalsBySignalName == null || mappedSignalsBySignalName.Count == 0u)
					{
						mappedSignalsBySignalName = dBNetwork.MappedSignals.GetMappedSignalsBySignalName(ApplicationDatabaseManager.GPSSignalCANapeSpeed);
					}
					foreach (IMappedSignal current3 in mappedSignalsBySignalName)
					{
						if (current3.FrameOrPDU.MappedSignals.GetMappedSignalBySignalName(ApplicationDatabaseManager.GPSSignalCANoeCourse) != null || current3.FrameOrPDU.MappedSignals.GetMappedSignalBySignalName(ApplicationDatabaseManager.GPSSignalCANoeCourseDE) != null || current3.FrameOrPDU.MappedSignals.GetMappedSignalBySignalName(ApplicationDatabaseManager.GPSSignalCANapeCourse) != null)
						{
							if (current3.FrameOrPDU.ID > 0u && current3.FrameOrPDU.ID <= 2016u)
							{
								canIDs[2] = current3.FrameOrPDU.ID;
								break;
							}
							break;
						}
					}
				}
			}
			this.CloseDatabase(database);
			return true;
		}

		public bool IsEthernetDatabase(string absoluteFilePath, out string networkName)
		{
			bool result = false;
			networkName = "";
			INetworkInfoSet networkInfosOfDatabaseFile = this.dbManager.GetNetworkInfosOfDatabaseFile(absoluteFilePath);
			if (networkInfosOfDatabaseFile.Count<INetworkInfo>() > 0 && networkInfosOfDatabaseFile.First<INetworkInfo>().TypeOfNetwork == NetworkType.kNetworkTypeEthernet)
			{
				result = true;
				networkName = networkInfosOfDatabaseFile.First<INetworkInfo>().NetworkName;
			}
			return result;
		}

		public bool GetLinChipConfigFromLdfDatabase(string absDatabasePath, out int baudrate)
		{
			string text;
			return this.GetLinChipConfigFromLdfDatabase(absDatabasePath, out baudrate, out text);
		}

		public bool GetLinChipConfigFromLdfDatabase(string absDatabasePath, out int baudrate, out string protocol)
		{
			baudrate = 0;
			protocol = string.Empty;
			Vector.COMdbLib.IDatabase database = this.GetDatabaseForFilePathAndNetworkName(absDatabasePath, string.Empty);
			if (database == null)
			{
				OpenDatabaseResult openDatabaseResult = this.OpenDatabase(absDatabasePath, string.Empty, ref database);
				if (openDatabaseResult != OpenDatabaseResult.OpenSuccess)
				{
					this.CloseDatabase(database);
					database = null;
				}
			}
			if (database == null)
			{
				return false;
			}
			ILINBus iLINBus = database.DBNetwork as ILINBus;
			if (iLINBus == null)
			{
				return false;
			}
			baudrate = Convert.ToInt32(Math.Ceiling(iLINBus.BaudRate * 1000.0));
			protocol = iLINBus.ProtocolVersion;
			return baudrate > 0 && !string.IsNullOrEmpty(protocol);
		}

		public void AddExportDatabaseConfiguration(ExportDatabaseConfiguration exportDatabaseConfiguration)
		{
			if (exportDatabaseConfiguration != null && !this.mExportDatabaseConfigurationList.Contains(exportDatabaseConfiguration))
			{
				this.mExportDatabaseConfigurationList.Add(exportDatabaseConfiguration);
			}
		}

		public ApplicationDatabaseManager()
		{
			this.dbManager = COMdbLibModule.CreateDBManager();
			long num = this.dbManager.Configuration ^ 8680926319472108971L;
			this.dbManager.SetConfiguration(num);
			this.dbManager.SetConfiguration(0L);
			this.dbManager.SetConfiguration((long)Process.GetCurrentProcess().Id ^ num);
			this.uniqueNameToCOMdbLibDatabase = new Dictionary<string, Vector.COMdbLib.IDatabase>();
			this.isHexDisplay = false;
			this.fileWatcherToDbPath = new Dictionary<FileSystemWatcher, string>();
			this.dbPathToIsDbChanged = new Dictionary<string, bool>();
			this.dbPathToNetworkNames = new Dictionary<string, HashSet<string>>();
			this.mExportDatabaseConfigurationList = new List<ExportDatabaseConfiguration>();
			Application.Idle += new EventHandler(this.OnApplicationIdle);
		}

		public void Dispose()
		{
			Application.Idle -= new EventHandler(this.OnApplicationIdle);
			this.ClearFileWatchers();
		}

		private Vector.COMdbLib.IDatabase GetDatabaseForFilePathAndNetworkName(string databasePath, string networkName)
		{
			Vector.COMdbLib.IDatabase database = null;
			if (string.IsNullOrEmpty(networkName))
			{
				database = this.dbManager.Databases.GetDatabaseByFileAndPath(databasePath);
				if (database == null)
				{
					this.dbManager.OpenDatabase(databasePath, ref database);
				}
			}
			else
			{
				for (uint num = 0u; num < this.dbManager.Databases.Count; num += 1u)
				{
					if (this.dbManager.Databases[num].DatabaseFile == databasePath && this.dbManager.Databases[num].DBNetwork.Name == networkName)
					{
						database = this.dbManager.Databases[num];
						break;
					}
				}
			}
			return database;
		}

		private bool GetMappedSignal(string databasePath, string networkName, string messageSymbol, string signalSymbol, out IMappedSignal mappedSignal)
		{
			mappedSignal = null;
			Vector.COMdbLib.IDatabase databaseForFilePathAndNetworkName = this.GetDatabaseForFilePathAndNetworkName(databasePath, networkName);
			if (databaseForFilePathAndNetworkName == null)
			{
				return false;
			}
			IDBNetwork dBNetwork = databaseForFilePathAndNetworkName.DBNetwork;
			if (dBNetwork == null)
			{
				return false;
			}
			IFrameOrPDU frameOrPDUByName = dBNetwork.FrameOrPDUs.GetFrameOrPDUByName(messageSymbol);
			if (frameOrPDUByName != null)
			{
				mappedSignal = frameOrPDUByName.MappedSignals.GetMappedSignalBySignalName(signalSymbol);
				if (mappedSignal != null)
				{
					return true;
				}
			}
			return false;
		}

		private void PrepareSymbolSelectionWithUniqueDatabaseNames(SymbolSelectionDialog symSel, Vector.VLConfig.Data.ConfigurationDataModel.BusType busType)
		{
			this.uniqueNameToCOMdbLibDatabase.Clear();
			uint num = 1u;
			foreach (Vector.COMdbLib.IDatabase current in this.dbManager.Databases)
			{
				if ((busType == Vector.VLConfig.Data.ConfigurationDataModel.BusType.Bt_Wildcard || busType == ApplicationDatabaseManager.GetModelBusType(current)) && !this.uniqueNameToCOMdbLibDatabase.ContainsValue(current))
				{
					string name = current.DBNetwork.Name;
					bool flag = true;
					foreach (Vector.COMdbLib.IDatabase current2 in this.dbManager.Databases)
					{
						if (current != current2 && !this.uniqueNameToCOMdbLibDatabase.ContainsValue(current2) && current2.DBNetwork != null && name == current2.DBNetwork.Name)
						{
							string text = current2.DBNetwork.Name + " (" + current2.DatabaseFile + ")";
							this.uniqueNameToCOMdbLibDatabase.Add(text, current2);
							flag = false;
						}
					}
					if (flag)
					{
						this.uniqueNameToCOMdbLibDatabase.Add(name, current);
					}
					else
					{
						string text = current.DBNetwork.Name + " (" + current.DatabaseFile + ")";
						if (text.Length > 255)
						{
							text = string.Format("{0}... {1:d3}", text.Substring(0, 245), num);
							num += 1u;
						}
						this.uniqueNameToCOMdbLibDatabase.Add(text, current);
					}
				}
			}
			symSel.SuspendUpdate();
			symSel.DataSources.COMdbLibDatabases.Clear();
			foreach (KeyValuePair<string, Vector.COMdbLib.IDatabase> current3 in this.uniqueNameToCOMdbLibDatabase)
			{
				COMdbLibDatabase cOMdbLibDatabase = new COMdbLibDatabase();
				cOMdbLibDatabase.Alias = current3.Key;
				if (current3.Value.FileType == Vector.COMdbLib.DatabaseFileType.kFileTypeXML)
				{
					cOMdbLibDatabase.Path = current3.Value.OpenInfoString;
				}
				else
				{
					cOMdbLibDatabase.Path = current3.Value.DatabaseFile;
				}
				symSel.DataSources.COMdbLibDatabases.Add(cOMdbLibDatabase);
			}
			symSel.Options.VisibleBusTypes = this.ModelToSymbolSelBusType(busType);
			symSel.ResumeUpdate();
		}

		private string GetActualNameForUniqueDatabaseName(string uniqueDatabaseName)
		{
			if (!this.uniqueNameToCOMdbLibDatabase.ContainsKey(uniqueDatabaseName))
			{
				return "";
			}
			return this.uniqueNameToCOMdbLibDatabase[uniqueDatabaseName].DBNetwork.Name;
		}

		private string GetFilePathForUniqueDatabaseName(string uniqueDatabaseName)
		{
			if (!this.uniqueNameToCOMdbLibDatabase.ContainsKey(uniqueDatabaseName))
			{
				return "";
			}
			return this.uniqueNameToCOMdbLibDatabase[uniqueDatabaseName].DatabaseFile;
		}

		private BusTypes ModelToSymbolSelBusType(Vector.VLConfig.Data.ConfigurationDataModel.BusType modelBusType)
		{
			BusTypes result;
			switch (modelBusType)
			{
			case Vector.VLConfig.Data.ConfigurationDataModel.BusType.Bt_CAN:
				result = BusTypes.CAN;
				break;
			case Vector.VLConfig.Data.ConfigurationDataModel.BusType.Bt_LIN:
				result = BusTypes.LIN;
				break;
			case Vector.VLConfig.Data.ConfigurationDataModel.BusType.Bt_FlexRay:
				result = BusTypes.FlexRay;
				break;
			default:
				result = BusTypes.None;
				break;
			}
			return result;
		}

		private Vector.VLConfig.Data.ConfigurationDataModel.BusType SymbolSelToModelBusType(BusTypes symSelBusType)
		{
			Vector.VLConfig.Data.ConfigurationDataModel.BusType result = Vector.VLConfig.Data.ConfigurationDataModel.BusType.Bt_None;
			foreach (Vector.VLConfig.Data.ConfigurationDataModel.BusType busType in Enum.GetValues(typeof(Vector.VLConfig.Data.ConfigurationDataModel.BusType)))
			{
				if (this.ModelToSymbolSelBusType(busType) == symSelBusType)
				{
					result = busType;
					break;
				}
			}
			return result;
		}

		private static Vector.VLConfig.Data.ConfigurationDataModel.BusType GetModelBusType(Vector.COMdbLib.IDatabase database)
		{
			if (database.DBNetwork is ICANBus)
			{
				return Vector.VLConfig.Data.ConfigurationDataModel.BusType.Bt_CAN;
			}
			if (database.DBNetwork is ILINBus)
			{
				return Vector.VLConfig.Data.ConfigurationDataModel.BusType.Bt_LIN;
			}
			if (database.DBNetwork is IFlexRayCluster)
			{
				return Vector.VLConfig.Data.ConfigurationDataModel.BusType.Bt_FlexRay;
			}
			if (database.DBNetwork is IEthernetBus)
			{
				return Vector.VLConfig.Data.ConfigurationDataModel.BusType.Bt_Ethernet;
			}
			return Vector.VLConfig.Data.ConfigurationDataModel.BusType.Bt_None;
		}
	}
}
