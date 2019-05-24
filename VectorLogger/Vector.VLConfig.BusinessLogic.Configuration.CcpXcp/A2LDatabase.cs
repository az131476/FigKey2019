using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Vector.McLoggerDatabaseGenerator;
using Vector.McModule;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.Data.DataAccess;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GUI.Helpers;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.LTLGenerationCS;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.BusinessLogic.Configuration.CcpXcp
{
	internal class A2LDatabase
	{
		private class SlaveInfo
		{
			public CcpXcpEcuInfo ecuInfo;

			public IDeviceConfig deviceConfig;

			public ISignalDatabaseConfig signalDatabaseConfig;

			public ILoggerEcu loggerEcu;
		}

		private static IModule sMcModule;

		private static IGenerator sMcGenerator;

		private IDatabase mcDatabase;

		private IDeviceConfig deviceConfig;

		private ILoggerConfig loggerConfig;

		private ILoggerEcu loggerEcu;

		private IList<IDaqList> daqLists;

		private Dictionary<uint, IDaqEvent> daqEvents;

		private Dictionary<uint, double> daqEventFactors;

		public static IModule McModule
		{
			get
			{
				if (A2LDatabase.sMcModule == null)
				{
					McModuleFactory mcModuleFactory = new McModuleFactory();
					A2LDatabase.sMcModule = mcModuleFactory.Create();
				}
				return A2LDatabase.sMcModule;
			}
		}

		public static IGenerator McGenerator
		{
			get
			{
				if (A2LDatabase.sMcGenerator == null)
				{
					GeneratorFactory generatorFactory = new GeneratorFactory();
					A2LDatabase.sMcGenerator = generatorFactory.Create();
				}
				return A2LDatabase.sMcGenerator;
			}
		}

		public IDatabase McDatabase
		{
			get
			{
				return this.mcDatabase;
			}
		}

		public IDeviceConfig DeviceConfig
		{
			get
			{
				return this.deviceConfig;
			}
		}

		public ILoggerConfig LoggerConfig
		{
			get
			{
				return this.loggerConfig;
			}
		}

		public ILoggerEcu LoggerEcu
		{
			get
			{
				return this.loggerEcu;
			}
		}

		public IList<IDaqList> DaqLists
		{
			get
			{
				return this.daqLists;
			}
		}

		public Dictionary<uint, IDaqEvent> DaqEvents
		{
			get
			{
				return this.daqEvents;
			}
		}

		public Dictionary<uint, double> DaqEventFactors
		{
			get
			{
				return this.daqEventFactors;
			}
		}

		public static Vector.VLConfig.BusinessLogic.Configuration.Result GenerateDbcOrFibexDatabase(CcpXcpDatabaseInfo di, IConfigurationManagerService configurationManagerService)
		{
			Vector.VLConfig.BusinessLogic.Configuration.Result result = Vector.VLConfig.BusinessLogic.Configuration.Result.OK;
			di.SetErrorText(string.Empty, null);
			string[] aMcModuleProcessingErrorText = new string[]
			{
				string.Empty
			};
			EventHandler<InformationEventArgs> value = delegate(object sender, InformationEventArgs args)
			{
				if (args.Severity == EnumSeverity.kError)
				{
					string[] aMcModuleProcessingErrorText;
					(aMcModuleProcessingErrorText = aMcModuleProcessingErrorText)[0] = aMcModuleProcessingErrorText[0] + args.Text;
				}
				FileSystemServices.WriteProtocolText(args.Text);
			};
			A2LDatabase.McGenerator.OnGeneratorMessage += value;
			A2LDatabase.McModule.OnParserMessage += value;
			if (!di.EcuInfoList.Any<CcpXcpEcuInfo>())
			{
				di.SetErrorText("Internal error: No ECUs for logger configuration available", null);
				result = Vector.VLConfig.BusinessLogic.Configuration.Result.Error;
			}
			List<A2LDatabase.SlaveInfo> list = new List<A2LDatabase.SlaveInfo>();
			ILoggerConfig loggerConfig = null;
			if (result == Vector.VLConfig.BusinessLogic.Configuration.Result.OK && !A2LDatabase.McGenerator.CreateLoggerConfig(out loggerConfig))
			{
				di.SetErrorText("Internal error: Cannot create LoggerConfig", null);
				result = Vector.VLConfig.BusinessLogic.Configuration.Result.Error;
			}
			if (result == Vector.VLConfig.BusinessLogic.Configuration.Result.OK)
			{
				result = A2LDatabase.GeneratorSetBusType(di);
			}
			if (result == Vector.VLConfig.BusinessLogic.Configuration.Result.OK && loggerConfig != null)
			{
				foreach (CcpXcpEcuInfo current in di.EcuInfoList)
				{
					result = A2LDatabase.GeneratorPrepareEcu(current, di, loggerConfig, list, configurationManagerService);
					if (result != Vector.VLConfig.BusinessLogic.Configuration.Result.OK)
					{
						break;
					}
				}
			}
			if (result == Vector.VLConfig.BusinessLogic.Configuration.Result.OK)
			{
				result = A2LDatabase.GeneratorSetDestinationFilePath(di);
			}
			if (result == Vector.VLConfig.BusinessLogic.Configuration.Result.OK && loggerConfig != null)
			{
				aMcModuleProcessingErrorText[0] = string.Empty;
				LcResult lcResult = loggerConfig.WriteDatabaseForLogger(di.DestinationFilePath);
				if (lcResult != LcResult.kOk)
				{
					CcpXcpEcuInfo aErrorEcuInfo;
					string aErrorText = A2LDatabase.GeneratorGetErrorText(lcResult, aMcModuleProcessingErrorText[0], list, configurationManagerService, out aErrorEcuInfo);
					di.SetErrorText(aErrorText, aErrorEcuInfo);
					result = Vector.VLConfig.BusinessLogic.Configuration.Result.Error;
				}
			}
			if (result == Vector.VLConfig.BusinessLogic.Configuration.Result.OK && di.BusType == BusType.Bt_Ethernet)
			{
				di.DestinationFilePath += Vocabulary.FileExtensionDotXML;
			}
			A2LDatabase.GeneratorCleanUp(loggerConfig, list);
			if (result == Vector.VLConfig.BusinessLogic.Configuration.Result.Error && string.IsNullOrEmpty(di.ErrorText))
			{
				di.SetErrorText(Resources_CcpXcp.CcpXcpErrorGenerating, null);
			}
			A2LDatabase.McModule.OnParserMessage -= value;
			A2LDatabase.McGenerator.OnGeneratorMessage -= value;
			return result;
		}

		private static Vector.VLConfig.BusinessLogic.Configuration.Result GeneratorPrepareEcu(CcpXcpEcuInfo aEcuInfo, CcpXcpDatabaseInfo di, ILoggerConfig aLoggerConfig, List<A2LDatabase.SlaveInfo> aSlaveInfoList, IConfigurationManagerService configurationManagerService)
		{
			bool flag = true;
			A2LDatabase a2LDatabase;
			if (aEcuInfo.Database.FileType != DatabaseFileType.A2L || (a2LDatabase = CcpXcpManager.Instance().GetA2LDatabase(aEcuInfo.Database)) == null)
			{
				return Vector.VLConfig.BusinessLogic.Configuration.Result.Error;
			}
			IDatabase database = a2LDatabase.McDatabase;
			A2LDatabase.SlaveInfo slaveInfo = new A2LDatabase.SlaveInfo
			{
				ecuInfo = aEcuInfo
			};
			aSlaveInfoList.Add(slaveInfo);
			string aErrorText;
			if (!A2LDatabase.CreateDeviceConfigInternal(aEcuInfo.Database, database, out slaveInfo.deviceConfig, out aErrorText, false))
			{
				di.SetErrorText(aErrorText, aEcuInfo);
				flag = false;
			}
			if (flag && database.CreateSignalDatabaseConfig(out slaveInfo.signalDatabaseConfig) != Vector.McModule.Result.kOk)
			{
				di.SetErrorText(Resources_CcpXcp.CcpXcpErrorGenerating + " (#1)", aEcuInfo);
				flag = false;
			}
			if (flag)
			{
				aLoggerConfig.CreateLoggerEcu(out slaveInfo.loggerEcu, database, slaveInfo.deviceConfig);
				A2LDatabase.GeneratorGetSignalListsForEcu(database, aEcuInfo, di, configurationManagerService);
				List<CcpXcpSignalListInfo> signalListInfos = (from signalListInfo in aEcuInfo.SignalListInfos
				where !signalListInfo.IsSecondarySlaveList
				select signalListInfo).ToList<CcpXcpSignalListInfo>();
				flag = A2LDatabase.GeneratorFillSignalConfig(database, signalListInfos, slaveInfo, out aErrorText);
				if (!flag)
				{
					di.SetErrorText(aErrorText, aEcuInfo);
				}
			}
			if (flag)
			{
				slaveInfo.loggerEcu.Data.DeviceName = aEcuInfo.Ecu.CcpXcpEcuDisplayName;
				if (!aEcuInfo.Ecu.UseDbParams)
				{
					slaveInfo.loggerEcu.Data.DaqTimeout = aEcuInfo.Ecu.DaqTimeout;
					if (aEcuInfo.Ecu.ExtendStaticDaqListToMaxSize)
					{
						A2LDatabase.ExtendSizeOfStaticDaqLists(slaveInfo.deviceConfig, true);
					}
				}
				if (slaveInfo.deviceConfig is ICcpDeviceConfig)
				{
					flag = A2LDatabase.GeneratorPrepareEcuCcp(slaveInfo, configurationManagerService);
				}
				else if (slaveInfo.deviceConfig is IXcpDeviceConfig)
				{
					flag = A2LDatabase.GeneratorPrepareEcuXcp(slaveInfo, aSlaveInfoList, aLoggerConfig, di, aEcuInfo, configurationManagerService);
				}
			}
			if (!flag)
			{
				return Vector.VLConfig.BusinessLogic.Configuration.Result.Error;
			}
			return Vector.VLConfig.BusinessLogic.Configuration.Result.OK;
		}

		private static bool GeneratorPrepareEcuXcp(A2LDatabase.SlaveInfo slaveInfo, List<A2LDatabase.SlaveInfo> aSlaveInfoList, ILoggerConfig aLoggerConfig, CcpXcpDatabaseInfo di, CcpXcpEcuInfo ecuInfo, IConfigurationManagerService configurationManagerService)
		{
			bool flag = true;
			CcpXcpEcuInfo ecuInfo2 = slaveInfo.ecuInfo;
			IXcpDeviceConfig xcpDeviceConfig = slaveInfo.deviceConfig as IXcpDeviceConfig;
			if (xcpDeviceConfig == null)
			{
				return false;
			}
			A2LDatabase.GeneratorSetCommonParametersForXcp(xcpDeviceConfig, ecuInfo2.Ecu);
			IXcpDeviceConfig xcpDeviceConfig2 = null;
			IXcpCanConfig xcpCanConfig = xcpDeviceConfig.TransportConfig as IXcpCanConfig;
			IXcpFlexRayConfig xcpFlexRayConfig = xcpDeviceConfig.TransportConfig as IXcpFlexRayConfig;
			IXcpIpConfig xcpIpConfig = xcpDeviceConfig.TransportConfig as IXcpIpConfig;
			if (xcpCanConfig != null)
			{
				if (xcpDeviceConfig.Daq.Identifier != EnumIdentifier.kDAQ_ID_FIELD_ABSOLUTE_ODT)
				{
					di.SetErrorText(Resources_CcpXcp.CcpXcpErrorAbsoluteOdtOnly, ecuInfo2);
					flag = false;
				}
				else
				{
					xcpCanConfig.CanFD = false;
					xcpCanConfig.UseBitrateSwitch = false;
					if (!ecuInfo2.Ecu.UseDbParams)
					{
						xcpCanConfig.IdMaster = A2LDatabase.MakeCanId(ecuInfo2.Ecu.CanRequestID, ecuInfo2.Ecu.IsCanRequestIdExtended);
						xcpCanConfig.IdSlave = A2LDatabase.MakeCanId(ecuInfo2.Ecu.CanResponseID, ecuInfo2.Ecu.IsCanResponseIdExtended);
						slaveInfo.loggerEcu.Data.PseudoCanIdStart = A2LDatabase.MakeCanId(ecuInfo2.Ecu.CanFirstID, ecuInfo2.Ecu.IsCanFirstIdExtended);
					}
					if (xcpDeviceConfig.ProtocolConfig.MaxCto > Constants.MaxCcpXcpCanCTO)
					{
						xcpDeviceConfig.ProtocolConfig.MaxCto = Constants.MaxCcpXcpCanCTO;
					}
					if (xcpDeviceConfig.ProtocolConfig.MaxDto > Constants.MaxCcpXcpCanDTO)
					{
						xcpDeviceConfig.ProtocolConfig.MaxDto = Constants.MaxCcpXcpCanDTO;
					}
				}
				ecuInfo2.EffectiveIsCanRequestIdExtended = ((xcpCanConfig.IdMaster & Constants.CANDbIsExtendedIdMask) == Constants.CANDbIsExtendedIdMask);
				ecuInfo2.EffectiveCanRequestID = (xcpCanConfig.IdMaster & ~Constants.CANDbIsExtendedIdMask);
			}
			else if (xcpFlexRayConfig != null)
			{
				if (!ecuInfo2.Ecu.UseDbParams)
				{
					xcpFlexRayConfig.DtoBuffering = ecuInfo2.Ecu.FlexRayEcuTxQueue;
					xcpFlexRayConfig.Nax = ecuInfo2.Ecu.FlexRayXcpNodeAdress;
					xcpDeviceConfig.ProtocolConfig.MaxCto = ecuInfo2.Ecu.MaxCTO;
					xcpDeviceConfig.ProtocolConfig.MaxDto = ecuInfo2.Ecu.MaxDTO;
				}
				List<Tuple<Database, uint>> flexRayDatabasesSuitedForXcp = configurationManagerService.ApplicationDatabaseManager.GetFlexRayDatabasesSuitedForXcp(ecuInfo2.Database.ChannelNumber.Value, configurationManagerService.DatabaseConfiguration.FlexrayFibexDatabases, configurationManagerService.ConfigFolderPath);
				if (flexRayDatabasesSuitedForXcp.Count == 1)
				{
					di.FlexRayDatabase = flexRayDatabasesSuitedForXcp[0].Item1;
					di.FlexRayXcpChannel = flexRayDatabasesSuitedForXcp[0].Item2;
					di.ChannelNumber = di.FlexRayXcpChannel;
				}
				else if (flexRayDatabasesSuitedForXcp.Count == 0)
				{
					di.SetErrorText(Resources_CcpXcp.CcpXcpErrorNoFibexDatabaseAssigned, ecuInfo2);
					flag = false;
				}
				else
				{
					di.SetErrorText(Resources_CcpXcp.CcpXcpErrorMultipleFibexDatabasesAssigned, ecuInfo2);
					flag = false;
				}
				if (flag)
				{
					XcpFlexRayBufferChannel bufferChannel = xcpFlexRayConfig.InitialCmdBuffer.BufferChannel;
					if (bufferChannel != xcpFlexRayConfig.InitialResBuffer.BufferChannel || (bufferChannel != XcpFlexRayBufferChannel.kChA && bufferChannel != XcpFlexRayBufferChannel.kChB))
					{
						di.SetErrorText(Resources_CcpXcp.CcpXcpErrorFlexRayChannelAsap2, ecuInfo2);
						flag = false;
					}
					else
					{
						uint channelNumber = 0u;
						switch (bufferChannel)
						{
						case XcpFlexRayBufferChannel.kChA:
							channelNumber = 1u;
							break;
						case XcpFlexRayBufferChannel.kChB:
							channelNumber = 2u;
							break;
						}
						if (!configurationManagerService.ApplicationDatabaseManager.FlexRayDatabasesContainXcpSlave(slaveInfo.loggerEcu.Data.DeviceName, configurationManagerService.ConfigFolderPath, channelNumber))
						{
							di.SetErrorText(Resources_CcpXcp.CcpXcpErrorFlexRayChannelConflict, ecuInfo2);
							flag = false;
						}
					}
				}
				if (flag)
				{
					if (configurationManagerService.ApplicationDatabaseManager.ProvideFlexRayFibexDataForXcp(xcpFlexRayConfig.FlexRayDataProvider, di.FlexRayDatabase, configurationManagerService.ConfigFolderPath))
					{
						xcpFlexRayConfig.Fibex = xcpFlexRayConfig.FlexRayDataProvider.Fibex;
						xcpFlexRayConfig.ClusterId = xcpFlexRayConfig.FlexRayDataProvider.Cluster;
						di.FlexRayNetworkName = xcpFlexRayConfig.FlexRayDataProvider.Network;
					}
					else
					{
						di.SetErrorText(Resources_CcpXcp.CcpXcpErrorUnableToConfigureFlexRay, ecuInfo2);
						flag = false;
					}
				}
			}
			else if (xcpIpConfig != null)
			{
				if (xcpIpConfig.TransportType == EnumXcpTransportType.kUdp)
				{
					if (ecuInfo2.Ecu.UseVxModule)
					{
						if (xcpDeviceConfig.Daq.Identifier == EnumIdentifier.kDAQ_ID_FIELD_ABSOLUTE_ODT)
						{
							di.SetErrorText(Resources_CcpXcp.ErrorVxModuleDoesNotSupportAbsoluteIdFields, ecuInfo2);
							flag = false;
						}
						if (!xcpDeviceConfig.Daq.UseTimestamps)
						{
							di.SetErrorText(Resources_CcpXcp.ErrorVxModuleNeedsTimeStamps, ecuInfo2);
							flag = false;
						}
					}
					if (flag)
					{
						string configComment = (configurationManagerService.ProjectRoot.LoggingConfiguration.CommentConfiguration == null) ? string.Empty : configurationManagerService.ProjectRoot.LoggingConfiguration.CommentConfiguration.Comment.Value;
						A2LDatabase.GeneratorSetEthernetParameter(ecuInfo2.Ecu, xcpIpConfig, xcpDeviceConfig, true, configComment);
						CcpXcpSignalListInfo ccpXcpSignalListInfo = null;
						CcpXcpSignalListInfo ccpXcpSignalListInfo2 = null;
						foreach (CcpXcpSignalListInfo current in ecuInfo.SignalListInfos)
						{
							if (current.IsSecondarySlaveList)
							{
								ccpXcpSignalListInfo2 = current;
							}
							else
							{
								ccpXcpSignalListInfo = current;
							}
						}
						if (ccpXcpSignalListInfo != null)
						{
							A2LDatabase.GeneratorSetEthernetLoggerConfigParameter(ecuInfo2.Ecu, slaveInfo.loggerEcu, configComment, ccpXcpSignalListInfo.Signals);
						}
						if (ccpXcpSignalListInfo2 != null && ccpXcpSignalListInfo2.Signals.Any<CcpXcpSignal>())
						{
							A2LDatabase.SlaveInfo slaveInfo2 = new A2LDatabase.SlaveInfo
							{
								ecuInfo = ecuInfo2
							};
							aSlaveInfoList.Add(slaveInfo2);
							bool flag2 = false;
							IDatabase database = null;
							A2LDatabase a2LDatabase = null;
							flag = (ecuInfo2.Database.FileType == DatabaseFileType.A2L && null != (a2LDatabase = CcpXcpManager.Instance().GetA2LDatabase(ecuInfo2.Database)));
							if (a2LDatabase != null)
							{
								database = a2LDatabase.McDatabase;
								string aErrorText;
								flag = A2LDatabase.CreateDeviceConfigInternal(ecuInfo2.Database, database, out slaveInfo2.deviceConfig, out aErrorText, false);
								if (!flag)
								{
									di.SetErrorText(aErrorText, ecuInfo2);
								}
							}
							if (flag)
							{
								xcpDeviceConfig2 = (slaveInfo2.deviceConfig as IXcpDeviceConfig);
								if (xcpDeviceConfig2 != null)
								{
									A2LDatabase.GeneratorSetCommonParametersForXcp(xcpDeviceConfig2, ecuInfo2.Ecu);
									IXcpIpConfig xcpIpConfig2 = xcpDeviceConfig2.TransportConfig as IXcpIpConfig;
									if (xcpIpConfig2 != null && database.CreateSignalDatabaseConfig(out slaveInfo2.signalDatabaseConfig) == Vector.McModule.Result.kOk)
									{
										aLoggerConfig.CreateLoggerEcu(out slaveInfo2.loggerEcu, database, slaveInfo2.deviceConfig);
										List<CcpXcpSignalListInfo> signalListInfos = new List<CcpXcpSignalListInfo>
										{
											ccpXcpSignalListInfo2
										};
										string text;
										if (A2LDatabase.GeneratorFillSignalConfig(database, signalListInfos, slaveInfo2, out text))
										{
											flag2 = true;
											slaveInfo2.loggerEcu.Data.DeviceName = ecuInfo2.Ecu.CcpXcpEcuDisplayName + "_Secondary";
											slaveInfo2.loggerEcu.Data.PrimaryXcpSlave = slaveInfo.loggerEcu;
											if (!ecuInfo2.Ecu.UseDbParams)
											{
												slaveInfo2.loggerEcu.Data.DaqTimeout = ecuInfo2.Ecu.DaqTimeout;
											}
											A2LDatabase.GeneratorSetEthernetParameter(ecuInfo2.Ecu, xcpIpConfig2, xcpDeviceConfig2, false, configComment);
											A2LDatabase.GeneratorSetEthernetLoggerConfigParameter(ecuInfo2.Ecu, slaveInfo2.loggerEcu, configComment, ccpXcpSignalListInfo2.Signals);
										}
									}
								}
								if (!flag2)
								{
									flag = false;
									di.SetErrorText(Resources_CcpXcp.CcpXcpErrorGenerating + " (#2)", ecuInfo2);
								}
							}
						}
					}
				}
				else
				{
					di.SetErrorText(Resources_CcpXcp.CcpXcpErrorNetworkTypeNotSupported + " (#1)", ecuInfo2);
					flag = false;
				}
			}
			else
			{
				di.SetErrorText(Resources_CcpXcp.CcpXcpErrorNetworkTypeNotSupported + " (#2)", ecuInfo2);
				flag = false;
			}
			if (flag)
			{
				if (ecuInfo2.Database.CpProtectionsWithSeedAndKeyRequired.Count > 0 && (ecuInfo2.Ecu.UseDbParams || ecuInfo2.Ecu.IsSeedAndKeyUsed))
				{
					string absolutePath = FileSystemServices.GetAbsolutePath(ecuInfo2.Database.CpProtectionsWithSeedAndKeyRequired[0].SeedAndKeyFilePath.Value, configurationManagerService.ConfigFolderPath);
					xcpDeviceConfig.ProtocolConfig.SeedAndKeyPathDaq = absolutePath;
				}
				else if (!ecuInfo2.Ecu.UseDbParams && ecuInfo2.Ecu.IsSeedAndKeyUsed && ecuInfo2.Database.CpProtections.Count > 0)
				{
					string absolutePath2 = FileSystemServices.GetAbsolutePath(ecuInfo2.Database.CpProtections[0].SeedAndKeyFilePath.Value, configurationManagerService.ConfigFolderPath);
					xcpDeviceConfig.ProtocolConfig.SeedAndKeyPathDaq = absolutePath2;
				}
				else
				{
					xcpDeviceConfig.ProtocolConfig.SeedAndKeyPathDaq = string.Empty;
				}
				xcpDeviceConfig.ProtocolConfig.SeedAndKeyPathCal = string.Empty;
				if (xcpDeviceConfig2 != null)
				{
					xcpDeviceConfig2.ProtocolConfig.SeedAndKeyPathDaq = xcpDeviceConfig.ProtocolConfig.SeedAndKeyPathDaq;
					xcpDeviceConfig2.ProtocolConfig.SeedAndKeyPathCal = xcpDeviceConfig.ProtocolConfig.SeedAndKeyPathCal;
				}
			}
			return flag;
		}

		private static void GeneratorSetCommonParametersForXcp(IXcpDeviceConfig xcpDeviceConfig, CcpXcpEcu ccpXcpEcu)
		{
			if (ccpXcpEcu.UseDbParams)
			{
				xcpDeviceConfig.Daq.UseTimestamps = CcpXcpManager.GetUseEcuTimestampDbParamOrDefaultValue(xcpDeviceConfig, ccpXcpEcu.UseVxModule);
				return;
			}
			xcpDeviceConfig.ProtocolConfig.ByteOrderIntel = ccpXcpEcu.IsProtocolByteOrderIntel;
			xcpDeviceConfig.Daq.UseTimestamps = ccpXcpEcu.UseEcuTimestamp;
			xcpDeviceConfig.Daq.TimestampSize = ccpXcpEcu.EcuTimestampWidth;
			xcpDeviceConfig.Daq.Unit = A2LDatabase.GetSampleUnitFromTimestampUnit(ccpXcpEcu.EcuTimestampUnit);
			xcpDeviceConfig.Daq.TimestampTicks = ccpXcpEcu.EcuTimestampTicks;
			xcpDeviceConfig.Daq.TimestampTicksPerUnit = (ccpXcpEcu.EcuTimestampCalculationMethod == CcpXcpEcuTimestampCalculationMethod.Division);
		}

		private static bool GeneratorPrepareEcuCcp(A2LDatabase.SlaveInfo slaveInfo, IConfigurationManagerService configurationManagerService)
		{
			CcpXcpEcuInfo ecuInfo = slaveInfo.ecuInfo;
			ICcpDeviceConfig ccpDeviceConfig = slaveInfo.deviceConfig as ICcpDeviceConfig;
			if (ccpDeviceConfig == null)
			{
				return false;
			}
			ccpDeviceConfig.CanChannel = ecuInfo.Database.ChannelNumber.Value;
			if (ecuInfo.Ecu.UseDbParams)
			{
				ccpDeviceConfig.UseSetSessionStatusDAQ = CcpXcpManager.GetSendSetSessionStatusDbParamOrDefaultValue(ccpDeviceConfig);
			}
			else
			{
				ccpDeviceConfig.CanIdMaster = A2LDatabase.MakeCanId(ecuInfo.Ecu.CanRequestID, ecuInfo.Ecu.IsCanRequestIdExtended);
				ccpDeviceConfig.CanIdSlave = A2LDatabase.MakeCanId(ecuInfo.Ecu.CanResponseID, ecuInfo.Ecu.IsCanResponseIdExtended);
				slaveInfo.loggerEcu.Data.PseudoCanIdStart = A2LDatabase.MakeCanId(ecuInfo.Ecu.CanFirstID, ecuInfo.Ecu.IsCanFirstIdExtended);
				ccpDeviceConfig.UseCcpV2_0 = ecuInfo.Ecu.UseCcpVersion2_0;
				ccpDeviceConfig.ByteOrderMotorola = !ecuInfo.Database.CcpXcpEcuList[0].IsProtocolByteOrderIntel;
				ccpDeviceConfig.UseSetSessionStatusDAQ = ecuInfo.Ecu.SendSetSessionStatus;
			}
			slaveInfo.loggerEcu.Data.ExchangeIdHandling = configurationManagerService.ProjectRoot.LoggingConfiguration.DatabaseConfiguration.EnableExchangeIdHandling.Value;
			if (ecuInfo.Database.CpProtectionsWithSeedAndKeyRequired.Count > 0 && (ecuInfo.Ecu.UseDbParams || ecuInfo.Ecu.IsSeedAndKeyUsed))
			{
				string absolutePath = FileSystemServices.GetAbsolutePath(ecuInfo.Database.CpProtectionsWithSeedAndKeyRequired[0].SeedAndKeyFilePath.Value, configurationManagerService.ConfigFolderPath);
				ccpDeviceConfig.SeedAndKeyPathDaq = absolutePath;
			}
			else if (!ecuInfo.Ecu.UseDbParams && ecuInfo.Ecu.IsSeedAndKeyUsed && ecuInfo.Database.CpProtections.Count > 0)
			{
				string absolutePath2 = FileSystemServices.GetAbsolutePath(ecuInfo.Database.CpProtections[0].SeedAndKeyFilePath.Value, configurationManagerService.ConfigFolderPath);
				ccpDeviceConfig.SeedAndKeyPathDaq = absolutePath2;
			}
			else
			{
				ccpDeviceConfig.SeedAndKeyPathDaq = string.Empty;
			}
			ccpDeviceConfig.SeedAndKeyPathCal = string.Empty;
			ecuInfo.EffectiveIsCanRequestIdExtended = ((ccpDeviceConfig.CanIdMaster & Constants.CANDbIsExtendedIdMask) == Constants.CANDbIsExtendedIdMask);
			ecuInfo.EffectiveCanRequestID = (ccpDeviceConfig.CanIdMaster & ~Constants.CANDbIsExtendedIdMask);
			return true;
		}

		private static void GeneratorCleanUp(ILoggerConfig aLoggerConfig, List<A2LDatabase.SlaveInfo> slaveInfoList)
		{
			if (aLoggerConfig != null)
			{
				foreach (A2LDatabase.SlaveInfo current in slaveInfoList)
				{
					if (current.loggerEcu != null)
					{
						aLoggerConfig.ReleaseLoggerEcu(current.loggerEcu);
					}
				}
				A2LDatabase.McGenerator.ReleaseLoggerConfig(aLoggerConfig);
			}
			foreach (A2LDatabase.SlaveInfo current2 in slaveInfoList)
			{
				A2LDatabase a2LDatabase;
				if (current2.ecuInfo != null && current2.ecuInfo.Database.FileType == DatabaseFileType.A2L && (a2LDatabase = CcpXcpManager.Instance().GetA2LDatabase(current2.ecuInfo.Database)) != null)
				{
					IDatabase database = a2LDatabase.McDatabase;
					current2.signalDatabaseConfig = null;
					if (current2.deviceConfig != null)
					{
						database.ReleaseDeviceConfig(current2.deviceConfig);
					}
				}
			}
		}

		private static Vector.VLConfig.BusinessLogic.Configuration.Result GeneratorSetBusType(CcpXcpDatabaseInfo di)
		{
			int count = di.EcuInfoList.Count;
			if (count < 1)
			{
				return Vector.VLConfig.BusinessLogic.Configuration.Result.Error;
			}
			di.BusType = di.EcuInfoList.First<CcpXcpEcuInfo>().Database.BusType.Value;
			return Vector.VLConfig.BusinessLogic.Configuration.Result.OK;
		}

		private static Vector.VLConfig.BusinessLogic.Configuration.Result GeneratorSetDestinationFilePath(CcpXcpDatabaseInfo di)
		{
			int count = di.EcuInfoList.Count;
			string text;
			if (di.BusType == BusType.Bt_Ethernet || di.BusType == BusType.Bt_FlexRay)
			{
				if (count > 1)
				{
					text = "XCP";
				}
				else
				{
					text = di.EcuInfoList.First<CcpXcpEcuInfo>().Ecu.CcpXcpEcuDisplayName;
				}
			}
			else
			{
				text = di.EcuInfoList.First<CcpXcpEcuInfo>().Ecu.CcpXcpEcuDisplayName;
			}
			text += "_";
			text += LTLUtil.GetPrettyChannelString(di.BusType, di.EcuInfoList.First<CcpXcpEcuInfo>().Database.ChannelNumber.Value);
			if (di.BusType == BusType.Bt_CAN)
			{
				text += Vocabulary.FileExtensionDotDBC;
			}
			else if (di.BusType != BusType.Bt_Ethernet)
			{
				text += Vocabulary.FileExtensionDotXML;
			}
			di.DestinationFilePath = Path.Combine(di.DestinationFolderPath, text);
			if (File.Exists(di.DestinationFilePath))
			{
				File.Delete(di.DestinationFilePath);
			}
			return Vector.VLConfig.BusinessLogic.Configuration.Result.OK;
		}

		private static string GeneratorGetErrorText(LcResult lcResult, string aMcModuleProcessingErrorText, List<A2LDatabase.SlaveInfo> aSlaveInfoList, IConfigurationManagerService configurationManagerService, out CcpXcpEcuInfo errorEcuInfo)
		{
			errorEcuInfo = ((aSlaveInfoList.Count == 1) ? aSlaveInfoList.First<A2LDatabase.SlaveInfo>().ecuInfo : null);
			string result;
			switch (lcResult)
			{
			case LcResult.kFailedToLoadModule:
				result = Resources_CcpXcp.CcpXcpErrorFailedToLoadModule;
				break;
			case LcResult.kErrorDeviceConfig:
				result = Resources_CcpXcp.CcpXcpErrorDeviceConfig;
				break;
			case LcResult.kErrorSignalConfig:
				result = Resources_CcpXcp.CcpXcpErrorSignalConfig;
				break;
			case LcResult.kErrorDaqConfig:
			{
				string text = string.Empty;
				string text2 = string.Empty;
				if (!string.IsNullOrEmpty(aMcModuleProcessingErrorText) && configurationManagerService != null)
				{
					Regex regex = new Regex("Failed to convert signal description of signal (\\S+) to Daq");
					Match match = regex.Match(aMcModuleProcessingErrorText);
					if (!match.Success)
					{
						Regex regex2 = new Regex("Failed to convert chunk description with signals (\\S+),\\s");
						match = regex2.Match(aMcModuleProcessingErrorText);
					}
					if (match.Success && match.Groups.Count > 1)
					{
						text = match.Groups[1].Value;
						LoggingConfiguration loggingConfiguration = configurationManagerService.ProjectRoot.LoggingConfiguration;
						foreach (CcpXcpSignal current in loggingConfiguration.CcpXcpSignalConfiguration.ActiveSignals)
						{
							if (string.Compare(current.Name.Value, text, StringComparison.InvariantCulture) == 0 && current.MeasurementMode.Value == CcpXcpMeasurementMode.DAQ)
							{
								if (errorEcuInfo == null)
								{
									foreach (A2LDatabase.SlaveInfo current2 in aSlaveInfoList)
									{
										if (string.Compare(current.EcuName.Value, current2.ecuInfo.Ecu.CcpXcpEcuDisplayName, StringComparison.InvariantCulture) == 0)
										{
											errorEcuInfo = current2.ecuInfo;
											break;
										}
									}
								}
								A2LDatabase a2LDatabase;
								IDaqEvent daqEvent;
								if (errorEcuInfo != null && errorEcuInfo.Database.FileType == DatabaseFileType.A2L && (a2LDatabase = CcpXcpManager.Instance().GetA2LDatabase(errorEcuInfo.Database)) != null && a2LDatabase.daqEvents.TryGetValue(current.DaqEventId.Value, out daqEvent))
								{
									text2 = daqEvent.Name;
									break;
								}
							}
						}
					}
				}
				if (string.IsNullOrEmpty(text) || string.IsNullOrEmpty(text2))
				{
					result = Resources_CcpXcp.CcpXcpErrorDaqConfig;
				}
				else
				{
					result = string.Format(Resources_CcpXcp.CcpXcpErrorDaqConfigEventSignal, text2, text);
				}
				break;
			}
			case LcResult.kErrorNetworkTypeNotSupported:
				result = Resources_CcpXcp.CcpXcpErrorNetworkTypeNotSupported;
				break;
			case LcResult.kErrorProtocolNotSupported:
				result = Resources_CcpXcp.CcpXcpErrorProtocolNotSupported;
				break;
			case LcResult.kWritingFileFailed:
				result = Resources_CcpXcp.CcpXcpErrorWritingFileFailed;
				break;
			case LcResult.kSignalNotFound:
				result = Resources_CcpXcp.CcpXcpErrorSignalConfig;
				break;
			case LcResult.kSignalNotSupported:
				result = Resources_CcpXcp.CcpXcpErrorSignalNotSupported;
				break;
			case LcResult.kErrorConfigurationSequence:
				result = Resources_CcpXcp.CcpXcpErrorConfigurationSequence;
				break;
			case LcResult.kErrorSeedAndKeyFile:
				result = Resources_CcpXcp.CcpXcpErrorSeedAndKeyFile;
				break;
			case LcResult.kErrorDaqConfigTooManyBytes:
				result = Resources_CcpXcp.CcpXcpErrorDaqConfigTooManyBytes;
				break;
			default:
				result = Resources_CcpXcp.CcpXcpErrorGenerating;
				break;
			}
			return result;
		}

		private static void GeneratorSetEthernetParameter(CcpXcpEcu ecu, IXcpIpConfig ip, IXcpDeviceConfig xcpDeviceConfig, bool bFirstPort, string configComment)
		{
			if (ecu.UseVxModule)
			{
				bool flag = configComment == null || !configComment.Contains("DisableAutomaticVxSetup");
				if (flag)
				{
					xcpDeviceConfig.ProtocolConfig.ConnectMode = 91;
				}
				else
				{
					xcpDeviceConfig.ProtocolConfig.ConnectMode = 0;
				}
			}
			if (ecu.UseDbParams)
			{
				ecu.EthernetHost = ip.Host;
				if (bFirstPort)
				{
					ecu.EthernetPort = (uint)ip.Port;
				}
				else
				{
					ip.Port = 5554;
				}
			}
			else
			{
				ip.Host = ecu.EthernetHost;
				if (bFirstPort)
				{
					ip.Port = (int)ecu.EthernetPort;
				}
				else
				{
					ip.Port = (int)ecu.EthernetPort2;
				}
				xcpDeviceConfig.ProtocolConfig.MaxCto = ecu.MaxCTO;
				xcpDeviceConfig.ProtocolConfig.MaxDto = ecu.MaxDTO;
			}
			if (xcpDeviceConfig.ProtocolConfig.MaxCto > Constants.MaxCcpXcpEthernetCTO)
			{
				xcpDeviceConfig.ProtocolConfig.MaxCto = Constants.MaxCcpXcpEthernetCTO;
			}
			if (xcpDeviceConfig.ProtocolConfig.MaxDto > Constants.MaxCcpXcpUdpDTO)
			{
				xcpDeviceConfig.ProtocolConfig.MaxDto = Constants.MaxCcpXcpUdpDTO;
			}
		}

		private static void GeneratorSetEthernetLoggerConfigParameter(CcpXcpEcu ecu, ILoggerEcu aLoggerEcu, string configComment, IEnumerable<CcpXcpSignal> signalList)
		{
			if (ecu.UseVxModule)
			{
				bool flag = configComment == null || !configComment.Contains("DisableVxForceUdpTransmission");
				if (flag)
				{
					double num = A2LDatabase.GeneratorCountBytesPerSecondOfSignals(signalList);
					if (num > Constants.MaxCcpXcpUdpDTO * 1000.0 / 2.0)
					{
						flag = false;
					}
				}
				if (flag)
				{
					aLoggerEcu.Data.DaqPriority = 255u;
					return;
				}
				aLoggerEcu.Data.DaqPriority = 4294967295u;
			}
		}

		private static void GeneratorGetSignalListsForEcu(IDatabase aMcDatabase, CcpXcpEcuInfo ecuInfo, CcpXcpDatabaseInfo di, IConfigurationManagerService configurationManagerService)
		{
			CcpXcpEcu ecu = ecuInfo.Ecu;
			LoggingConfiguration loggingConfiguration = configurationManagerService.ProjectRoot.LoggingConfiguration;
			ReadOnlyCollection<ActionCcpXcp> activeActions = loggingConfiguration.CcpXcpSignalConfiguration.ActiveActions;
			if (di.BusType == BusType.Bt_CAN)
			{
				CcpXcpSignalListInfo ccpXcpSignalListInfo = null;
				CcpXcpSignalListInfo ccpXcpSignalListInfo2 = null;
				uint num = 0u;
				foreach (ActionCcpXcp current in activeActions)
				{
					Dictionary<string, CcpXcpSignalListInfo> dictionary = new Dictionary<string, CcpXcpSignalListInfo>();
					string text = null;
					foreach (CcpXcpSignal current2 in current.ActiveSignals)
					{
						if (string.Compare(current2.EcuName.Value, ecu.CcpXcpEcuDisplayName, StringComparison.InvariantCulture) == 0)
						{
							switch (current2.MeasurementMode.Value)
							{
							case CcpXcpMeasurementMode.DAQ:
								if (ccpXcpSignalListInfo == null)
								{
									ccpXcpSignalListInfo = new CcpXcpSignalListInfo(false, current);
								}
								ccpXcpSignalListInfo.Signals.Add(current2);
								break;
							case CcpXcpMeasurementMode.Polling:
								if (current.Mode == ActionCcpXcp.ActivationMode.Always)
								{
									if (ccpXcpSignalListInfo2 == null)
									{
										ccpXcpSignalListInfo2 = new CcpXcpSignalListInfo(true, current);
									}
									ccpXcpSignalListInfo2.Signals.Add(current2);
								}
								else
								{
									string key;
									if (current.Mode == ActionCcpXcp.ActivationMode.Triggered)
									{
										key = "n.a.";
									}
									else
									{
										key = current2.PollingTime.Value;
									}
									CcpXcpSignalListInfo ccpXcpSignalListInfo3;
									if (!dictionary.TryGetValue(key, out ccpXcpSignalListInfo3))
									{
										ccpXcpSignalListInfo3 = new CcpXcpSignalListInfo(true, current);
										dictionary[key] = ccpXcpSignalListInfo3;
									}
									ccpXcpSignalListInfo3.Signals.Add(current2);
								}
								break;
							}
						}
					}
					uint num2 = 0u;
					foreach (KeyValuePair<string, CcpXcpSignalListInfo> current3 in dictionary)
					{
						if (string.IsNullOrEmpty(text))
						{
							text = "Data" + num;
							num += 1u;
						}
						current3.Value.NameIfTriggeredPollingList = text + "_" + num2;
						ecuInfo.SignalListInfos.Add(current3.Value);
						num2 += 1u;
					}
				}
				if (ccpXcpSignalListInfo != null)
				{
					ecuInfo.SignalListInfos.Add(ccpXcpSignalListInfo);
				}
				if (ccpXcpSignalListInfo2 != null)
				{
					ecuInfo.SignalListInfos.Add(ccpXcpSignalListInfo2);
					return;
				}
			}
			else
			{
				List<CcpXcpSignal> list = new List<CcpXcpSignal>();
				foreach (ActionCcpXcp current4 in activeActions)
				{
					foreach (CcpXcpSignal current5 in current4.ActiveSignals)
					{
						if (string.Compare(current5.EcuName.Value, ecu.CcpXcpEcuDisplayName, StringComparison.InvariantCulture) == 0)
						{
							list.Add(current5);
						}
					}
				}
				if (ecu.UseVxModule)
				{
					A2LDatabase.GeneratorGetSignalListsForEcuVx(aMcDatabase, ecuInfo, list);
					return;
				}
				CcpXcpSignalListInfo ccpXcpSignalListInfo4 = new CcpXcpSignalListInfo(false, null);
				ccpXcpSignalListInfo4.Signals = list;
				ecuInfo.SignalListInfos.Add(ccpXcpSignalListInfo4);
			}
		}

		private static void GeneratorGetSignalListsForEcuVx(IDatabase aMcDatabase, CcpXcpEcuInfo ecuInfo, List<CcpXcpSignal> listAllSignals)
		{
			CcpXcpEcu ecu = ecuInfo.Ecu;
			ecuInfo.PreventDeconcatinationForFirstXcpOnUdpSlave = false;
			List<CcpXcpSignalEvent> list = new List<CcpXcpSignalEvent>();
			foreach (IFeatureEvents current in VLProject.FeatureRegistration.FeaturesUsingEvents)
			{
				list.AddRange(current.GetEvents(true).OfType<CcpXcpSignalEvent>());
			}
			List<CcpXcpSignalEvent> list2 = new List<CcpXcpSignalEvent>(from ccpXcpSignalEvent in list
			where ccpXcpSignalEvent.CcpXcpEcuName.Value != null && ccpXcpSignalEvent.CcpXcpEcuName.Value.Equals(ecu.CcpXcpEcuDisplayName, StringComparison.InvariantCultureIgnoreCase)
			select ccpXcpSignalEvent);
			if (!list2.Any<CcpXcpSignalEvent>())
			{
				CcpXcpSignalListInfo ccpXcpSignalListInfo = new CcpXcpSignalListInfo(false, null);
				ccpXcpSignalListInfo.Signals = listAllSignals;
				ecuInfo.SignalListInfos.Add(ccpXcpSignalListInfo);
				ecuInfo.PreventDeconcatinationForFirstXcpOnUdpSlave = true;
				return;
			}
			uint num = A2LDatabase.CountBytesOfSignals(aMcDatabase, listAllSignals);
			uint num2;
			CcpXcpManager.Instance().GetMaxDTO(CcpXcpManager.Instance().GetDatabase(ecu), out num2);
			if (num + 8u < num2)
			{
				CcpXcpSignalListInfo ccpXcpSignalListInfo2 = new CcpXcpSignalListInfo(false, null);
				ccpXcpSignalListInfo2.Signals = listAllSignals;
				ecuInfo.SignalListInfos.Add(ccpXcpSignalListInfo2);
				return;
			}
			List<CcpXcpSignal> list3 = new List<CcpXcpSignal>();
			List<CcpXcpSignal> list4 = null;
			using (List<CcpXcpSignal>.Enumerator enumerator2 = listAllSignals.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					CcpXcpSignal ccpXcpSignal = enumerator2.Current;
					if (list2.Find((CcpXcpSignalEvent s) => string.Compare(s.SignalName.Value, ccpXcpSignal.Name.Value, StringComparison.InvariantCulture) == 0) != null)
					{
						list3.Add(ccpXcpSignal);
					}
					else
					{
						if (list4 == null)
						{
							list4 = new List<CcpXcpSignal>();
						}
						list4.Add(ccpXcpSignal);
					}
				}
			}
			CcpXcpSignalListInfo ccpXcpSignalListInfo3 = new CcpXcpSignalListInfo(false, null);
			ecuInfo.SignalListInfos.Add(ccpXcpSignalListInfo3);
			if (list3.Any<CcpXcpSignal>() || list4 == null || !list4.Any<CcpXcpSignal>())
			{
				ccpXcpSignalListInfo3.Signals = list3;
				if (list4 != null && list4.Any<CcpXcpSignal>())
				{
					CcpXcpSignalListInfo ccpXcpSignalListInfo4 = new CcpXcpSignalListInfo(false, null);
					ecuInfo.SignalListInfos.Add(ccpXcpSignalListInfo4);
					ccpXcpSignalListInfo4.Signals = list4;
					ccpXcpSignalListInfo4.IsSecondarySlaveList = true;
					return;
				}
			}
			else
			{
				ccpXcpSignalListInfo3.Signals = list4;
				ecuInfo.PreventDeconcatinationForFirstXcpOnUdpSlave = true;
			}
		}

		private static bool GeneratorFillSignalConfig(IDatabase aMcDatabase, IEnumerable<CcpXcpSignalListInfo> signalListInfos, A2LDatabase.SlaveInfo aSlaveInfo, out string errorText)
		{
			uint num = 0u;
			errorText = string.Empty;
			Dictionary<uint, uint> dictionary = new Dictionary<uint, uint>();
			int num2 = 0;
			foreach (CcpXcpSignalListInfo current in signalListInfos)
			{
				if (current.IsAlwaysActive())
				{
					using (List<CcpXcpSignal>.Enumerator enumerator2 = current.Signals.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							CcpXcpSignal current2 = enumerator2.Current;
							string value = current2.Name.Value;
							ISignal signal = aMcDatabase.GetSignal(value);
							if (signal != null)
							{
								switch (current2.MeasurementMode.Value)
								{
								case CcpXcpMeasurementMode.DAQ:
								{
									ushort daqEventId = (ushort)current2.DaqEventId.Value;
									aSlaveInfo.signalDatabaseConfig.AddSignal(signal, daqEventId, 1);
									aSlaveInfo.loggerEcu.Data.AddSignalDaq(signal, (uint)daqEventId);
									num += 1u;
									break;
								}
								case CcpXcpMeasurementMode.Polling:
								{
									uint num3;
									if (uint.TryParse(current2.PollingTime.Value, out num3))
									{
										uint num4;
										dictionary.TryGetValue(num3, out num4);
										num4 += signal.ValueTypeSizeInBytes(signal.ValueType) * signal.Dimension;
										if (num4 > Constants.MaxCcpXcpPollingCycleBytes)
										{
											errorText = string.Format(Resources_CcpXcp.CcpXcpErrorPollingCycleBytes, num3, Constants.MaxCcpXcpPollingCycleBytes);
											bool result = false;
											return result;
										}
										dictionary[num3] = num4;
										aSlaveInfo.signalDatabaseConfig.AddSignal(signal);
										aSlaveInfo.loggerEcu.Data.AddSignalPolling(signal, num3);
										num += 1u;
									}
									break;
								}
								}
							}
						}
						continue;
					}
				}
				if (string.IsNullOrEmpty(current.NameIfTriggeredPollingList))
				{
					errorText = "Internal error: Triggered measurement list has no name.";
					bool result = false;
					return result;
				}
				num2++;
				int count = current.Signals.Count;
				if ((long)current.Signals.Count > (long)((ulong)Constants.MaxCcpXcpPollingSignalsPerTriggeredList))
				{
					errorText = string.Format(Resources_CcpXcp.CcpXcpErrorPollingNumberOfSignalsPerList, count, Constants.MaxCcpXcpPollingSignalsPerTriggeredList);
					bool result = false;
					return result;
				}
				foreach (CcpXcpSignal current3 in current.Signals)
				{
					string value2 = current3.Name.Value;
					ISignal signal2 = aMcDatabase.GetSignal(value2);
					if (signal2 != null)
					{
						switch (current3.MeasurementMode.Value)
						{
						case CcpXcpMeasurementMode.DAQ:
						{
							errorText = "Internal error: Triggered DAQ measurements not supported yet.";
							bool result = false;
							return result;
						}
						case CcpXcpMeasurementMode.Polling:
							aSlaveInfo.signalDatabaseConfig.AddSignal(signal2);
							aSlaveInfo.loggerEcu.Data.AddSignalPollingNonCyclic(signal2, current.NameIfTriggeredPollingList);
							num += 1u;
							break;
						}
					}
				}
			}
			int num5 = dictionary.Count + num2;
			if ((long)num5 > (long)((ulong)Constants.MaxCcpXcpPollingCycles))
			{
				errorText = string.Format(Resources_CcpXcp.CcpXcpErrorPollingCycles, num5, Constants.MaxCcpXcpPollingCycles);
				return false;
			}
			if (num < 1u)
			{
				errorText = Resources_CcpXcp.CcpXcpErrorNoSignals;
				return false;
			}
			return true;
		}

		private static double GeneratorCountBytesPerSecondOfSignals(IEnumerable<CcpXcpSignal> signalList)
		{
			return signalList.Sum((CcpXcpSignal ccpXcpSignal) => CcpXcpManager.Instance().CountBytesPerSecondOfSignal(ccpXcpSignal));
		}

		public Vector.McModule.Result Create(string path)
		{
			Cursor.Current = Cursors.WaitCursor;
			A2LDatabase.McModule.OnParserMessage += new EventHandler<InformationEventArgs>(this.McModuleOnParserMessage);
			A2LDatabase.McGenerator.OnGeneratorMessage += new EventHandler<InformationEventArgs>(this.McGeneratorOnGeneratorMessage);
			Vector.McModule.Result result = A2LDatabase.McModule.CreateDatabase(out this.mcDatabase, path, false);
			A2LDatabase.McGenerator.OnGeneratorMessage -= new EventHandler<InformationEventArgs>(this.McGeneratorOnGeneratorMessage);
			A2LDatabase.McModule.OnParserMessage -= new EventHandler<InformationEventArgs>(this.McModuleOnParserMessage);
			Cursor.Current = Cursors.Default;
			if (result != Vector.McModule.Result.kOk)
			{
				this.Release();
				return result;
			}
			return Vector.McModule.Result.kOk;
		}

		private void McGeneratorOnGeneratorMessage(object sender, InformationEventArgs args)
		{
			FileSystemServices.WriteProtocolText(args.Text);
		}

		private void McModuleOnParserMessage(object sender, InformationEventArgs args)
		{
			if (args.Text != null)
			{
				FileSystemServices.WriteProtocolText(args.Text + Environment.NewLine);
			}
		}

		public bool IsLoaded()
		{
			return this.mcDatabase != null;
		}

		public bool Equals(IDatabase db)
		{
			return db != null && this.mcDatabase != null && this.mcDatabase.Equals(db);
		}

		public void Release()
		{
			if (this.mcDatabase != null)
			{
				A2LDatabase.McModule.ReleaseDatabase(this.mcDatabase);
			}
			this.mcDatabase = null;
		}

		public Vector.McModule.Result Update()
		{
			Vector.McModule.Result result;
			using (new WaitCursor())
			{
				string fullPath = this.mcDatabase.FullPath;
				this.Release();
				if (!File.Exists(fullPath))
				{
					result = Vector.McModule.Result.kObjectNotFound;
				}
				else
				{
					Vector.McModule.Result result2 = this.Create(fullPath);
					result = result2;
				}
			}
			return result;
		}

		public void UpdateDaqLists()
		{
			if (this.deviceConfig == null)
			{
				return;
			}
			this.daqLists = this.deviceConfig.Daq.DaqListsCustom;
		}

		public bool CreateDeviceConfig(Database database, out string createDeviceConfigErrorText, bool replacingDatabase)
		{
			createDeviceConfigErrorText = string.Empty;
			if (this.mcDatabase == null)
			{
				return false;
			}
			if (this.deviceConfig != null)
			{
				this.ReleaseDeviceConfig();
				this.deviceConfig = null;
			}
			if (!A2LDatabase.CreateDeviceConfigInternal(database, this.mcDatabase, out this.deviceConfig, out createDeviceConfigErrorText, replacingDatabase))
			{
				return false;
			}
			this.daqEvents = this.CreateDaqEventList();
			database.IsInconsistent |= !this.IsConsistentTo(database);
			if (!database.IsInconsistent)
			{
				this.CreateLoggerEcu();
				bool flag = database.CcpXcpEcuList.Any<CcpXcpEcu>() && database.CcpXcpEcuList[0].UseVxModule;
				if (flag)
				{
					database.UpdateSeedAndKeyUsedStatus(false);
				}
				else
				{
					database.UpdateSeedAndKeyUsedStatus(this.HasSeedAndKey());
				}
			}
			this.daqLists = this.deviceConfig.Daq.DaqListsCustom;
			this.daqEvents = new Dictionary<uint, IDaqEvent>();
			this.daqEventFactors = new Dictionary<uint, double>();
			IList<IDaqEvent> daqEventListA2L = this.deviceConfig.Daq.DaqEventListA2L;
			foreach (IDaqEvent current in daqEventListA2L)
			{
				if (!this.daqEvents.ContainsKey(current.Id))
				{
					this.daqEvents.Add(current.Id, current);
					double num = 0.0;
					switch (current.Unit)
					{
					case EnumSampleUnit.kSampleUnit1ns:
						num = 1000000000.0;
						break;
					case EnumSampleUnit.kSampleUnit10ns:
						num = 100000000.0;
						break;
					case EnumSampleUnit.kSampleUnit100ns:
						num = 10000000.0;
						break;
					case EnumSampleUnit.kSampleUnit1us:
						num = 1000000.0;
						break;
					case EnumSampleUnit.kSampleUnit10us:
						num = 100000.0;
						break;
					case EnumSampleUnit.kSampleUnit100us:
						num = 10000.0;
						break;
					case EnumSampleUnit.kSampleUnit1ms:
						num = 1000.0;
						break;
					case EnumSampleUnit.kSampleUnit10ms:
						num = 100.0;
						break;
					case EnumSampleUnit.kSampleUnit100ms:
						num = 10.0;
						break;
					case EnumSampleUnit.kSampleUnit1s:
						num = 1.0;
						break;
					}
					if (current.Rate != 0u)
					{
						num /= current.Rate;
					}
					else
					{
						num = 0.0;
					}
					this.daqEventFactors.Add(current.Id, num);
				}
			}
			return true;
		}

		private static bool CreateDeviceConfigInternal(Database database, IDatabase aMcDatabase, out IDeviceConfig aDeviceConfig, out string createDeviceConfigErrorText, bool replacingDatabase)
		{
			aDeviceConfig = null;
			createDeviceConfigErrorText = string.Empty;
			if (aMcDatabase == null)
			{
				return false;
			}
			BusType busType = database.BusType.Value;
			string tlin = database.CcpXcpEcuList.Any<CcpXcpEcu>() ? database.CcpXcpEcuList[0].TransportLayerInstanceName : string.Empty;
			bool flag = database.CcpXcpEcuList.Any<CcpXcpEcu>() && database.CcpXcpEcuList[0].UseVxModule;
			EnumProtocolType enumProtocolType;
			if (flag)
			{
				enumProtocolType = EnumProtocolType.ProtocolXcp;
				busType = BusType.Bt_Ethernet;
			}
			else if (database.CPType.Value == CPType.CCP || database.CPType.Value == CPType.CCP101)
			{
				enumProtocolType = EnumProtocolType.ProtocolCcp;
			}
			else if (database.CPType.Value == CPType.XCP)
			{
				enumProtocolType = EnumProtocolType.ProtocolXcp;
			}
			else
			{
				database.CPType.Value = CPType.XCP;
				enumProtocolType = EnumProtocolType.ProtocolXcp;
			}
			IDeviceInfo deviceInfo = null;
			IList<IDeviceInfo> list = aMcDatabase.CreateDeviceInfoList();
			if (enumProtocolType == EnumProtocolType.ProtocolCcp)
			{
				deviceInfo = list.FirstOrDefault((IDeviceInfo d) => d.ProtocolType == EnumProtocolType.ProtocolCcp);
			}
			else if (enumProtocolType == EnumProtocolType.ProtocolXcp)
			{
				if (busType == BusType.Bt_CAN)
				{
					deviceInfo = A2LDatabase.FindDeviceInfo(list, EnumXcpTransportType.kCan, tlin);
				}
				else if (busType == BusType.Bt_FlexRay)
				{
					deviceInfo = A2LDatabase.FindDeviceInfo(list, EnumXcpTransportType.kFlexRay, tlin);
				}
				else if (busType == BusType.Bt_Ethernet)
				{
					if (flag)
					{
						if (CcpXcpEcu.IsDefaultTransportLayerInstanceForVx(tlin))
						{
							deviceInfo = A2LDatabase.CreateDeviceInfoDefaultForVxModule(aMcDatabase);
						}
						else
						{
							deviceInfo = A2LDatabase.FindDeviceInfo(list, EnumXcpTransportType.kUdp, tlin);
						}
					}
					else if (database.CcpXcpEcuList.Any<CcpXcpEcu>() && database.CcpXcpEcuList[0].EthernetProtocol == EthernetProtocol.UDP)
					{
						deviceInfo = A2LDatabase.FindDeviceInfo(list, EnumXcpTransportType.kUdp, tlin);
					}
				}
			}
			if (deviceInfo == null)
			{
				if (replacingDatabase)
				{
					database.IsInconsistent = true;
					return false;
				}
				deviceInfo = aMcDatabase.CreateEmptyDeviceInfo();
				deviceInfo.ProtocolType = EnumProtocolType.ProtocolXcp;
				if (busType == BusType.Bt_CAN)
				{
					deviceInfo.TransportType = EnumXcpTransportType.kCan;
				}
				else
				{
					if (busType != BusType.Bt_Ethernet)
					{
						database.IsInconsistent = true;
						return false;
					}
					deviceInfo.TransportType = EnumXcpTransportType.kUdp;
				}
			}
			aMcDatabase.CreateDeviceConfig(out aDeviceConfig, deviceInfo);
			if (aDeviceConfig == null)
			{
				createDeviceConfigErrorText = Resources_CcpXcp.CcpXcpErrorDeviceConfig;
				return false;
			}
			if (flag && CcpXcpEcu.IsDefaultTransportLayerInstanceForVx(tlin))
			{
				IXcpDeviceConfig xcpDeviceConfig = aDeviceConfig as IXcpDeviceConfig;
				if (xcpDeviceConfig != null)
				{
					xcpDeviceConfig.ProtocolConfig.MaxCto = Constants.DefaultCcpXcpUdpCTO;
					xcpDeviceConfig.ProtocolConfig.MaxDto = Constants.DefaultCcpXcpUdpDTO;
					xcpDeviceConfig.ProtocolConfig.ByteOrderIntel = true;
					xcpDeviceConfig.Daq.Identifier = EnumIdentifier.kDAQ_ID_FIELD_REL_ODT_BYTE;
					xcpDeviceConfig.Daq.UseTimestamps = true;
					xcpDeviceConfig.Daq.TimestampSize = 4u;
					xcpDeviceConfig.Daq.TimestampTicks = 1u;
					xcpDeviceConfig.Daq.Unit = EnumSampleUnit.kSampleUnit1us;
					IXcpIpConfig xcpIpConfig = xcpDeviceConfig.TransportConfig as IXcpIpConfig;
					if (xcpIpConfig != null)
					{
						xcpIpConfig.Host = "192.168.165.1";
						xcpIpConfig.Port = 5555;
					}
				}
			}
			ICcpDeviceConfig ccpDeviceConfig = aDeviceConfig as ICcpDeviceConfig;
			if (ccpDeviceConfig != null)
			{
				ccpDeviceConfig.UseCcpV2_0 = (ccpDeviceConfig.MajorVersion == 2u && ccpDeviceConfig.MinorVersion == 0u);
			}
			return true;
		}

		public void ReleaseDeviceConfig()
		{
			this.ReleaseLoggerEcu();
			A2LDatabase.ReleaseDeviceConfigInternal(this.mcDatabase, this.deviceConfig);
			this.deviceConfig = null;
		}

		private static void ReleaseDeviceConfigInternal(IDatabase aMcDatabase, IDeviceConfig aDeviceConfig)
		{
			if (aMcDatabase != null && aDeviceConfig != null)
			{
				aMcDatabase.ReleaseDeviceConfig(aDeviceConfig);
			}
		}

		private Dictionary<uint, IDaqEvent> CreateDaqEventList()
		{
			if (this.deviceConfig == null)
			{
				return null;
			}
			if (this.deviceConfig.Daq.DaqEventListA2L == null)
			{
				return null;
			}
			Dictionary<uint, IDaqEvent> dictionary = new Dictionary<uint, IDaqEvent>();
			foreach (IDaqEvent current in this.deviceConfig.Daq.DaqEventListA2L)
			{
				if (!dictionary.ContainsKey(current.Id))
				{
					dictionary.Add(current.Id, current);
				}
			}
			return dictionary;
		}

		public void CreateLoggerEcu()
		{
			if (this.mcDatabase != null && this.deviceConfig != null && A2LDatabase.sMcGenerator.CreateLoggerConfig(out this.loggerConfig))
			{
				this.loggerConfig.CreateLoggerEcu(out this.loggerEcu, this.mcDatabase, this.DeviceConfig);
			}
		}

		public void ReleaseLoggerEcu()
		{
			if (this.mcDatabase != null && this.loggerConfig != null && this.loggerEcu != null)
			{
				this.loggerConfig.ReleaseLoggerEcu(this.loggerEcu);
				this.loggerEcu = null;
				A2LDatabase.sMcGenerator.ReleaseLoggerConfig(this.loggerConfig);
				this.loggerConfig = null;
			}
		}

		public bool IsConsistentTo(Database database)
		{
			if (this.mcDatabase == null)
			{
				return false;
			}
			if (database.CPType.Value == CPType.XCP)
			{
				string tlin = database.CcpXcpEcuList[0].TransportLayerInstanceName;
				IList<IDeviceInfo> list = this.CreateDeviceInfoList();
				if (list != null)
				{
					if (list.Any((IDeviceInfo di) => di.TransportType != EnumXcpTransportType.kTcp))
					{
						List<BusType> list2 = list.Select(new Func<IDeviceInfo, BusType>(A2LDatabase.GetBusTypeFromDeviceInfo)).ToList<BusType>();
						if (CcpXcpEcu.IsDefaultTransportLayerInstanceForVx(tlin))
						{
							list2.Add(BusType.Bt_Ethernet);
						}
						if (database.BusType.Value != BusType.Bt_None && database.BusType.Value != BusType.Bt_Wildcard && !list2.Contains(database.BusType.Value))
						{
							return false;
						}
						goto IL_122;
					}
				}
				if (database.BusType.Value != BusType.Bt_None && database.BusType.Value != BusType.Bt_Wildcard && database.BusType.Value != BusType.Bt_CAN && database.BusType.Value != BusType.Bt_Ethernet)
				{
					return false;
				}
				IL_122:
				if (list != null && !string.IsNullOrEmpty(tlin))
				{
					if (CcpXcpEcu.IsDefaultTransportLayerInstanceForVx(tlin))
					{
						if (list.Any<IDeviceInfo>())
						{
							if (list.All((IDeviceInfo d) => d.ProtocolType != EnumProtocolType.ProtocolXcp))
							{
								return false;
							}
						}
					}
					else if (list.Any<IDeviceInfo>())
					{
						if (list.All((IDeviceInfo d) => string.Compare(tlin, d.TransportLayerInstanceName, StringComparison.InvariantCulture) != 0))
						{
							return false;
						}
					}
				}
				if (list != null && database.BusType.Value == BusType.Bt_Ethernet)
				{
					if (database.CcpXcpEcuList[0].EthernetProtocol == EthernetProtocol.UDP && !database.CcpXcpEcuList[0].UseVxModule && list.Any<IDeviceInfo>())
					{
						if (list.All((IDeviceInfo d) => d.TransportType != EnumXcpTransportType.kUdp))
						{
							return false;
						}
					}
					if (database.CcpXcpEcuList[0].EthernetProtocol == EthernetProtocol.TCP && list.Any<IDeviceInfo>())
					{
						if (list.All((IDeviceInfo d) => d.TransportType != EnumXcpTransportType.kTcp))
						{
							return false;
						}
					}
				}
			}
			else if ((database.CPType.Value == CPType.CCP || database.CPType.Value == CPType.CCP101) && database.BusType.Value != BusType.Bt_CAN && database.BusType.Value != BusType.Bt_Wildcard)
			{
				return false;
			}
			return true;
		}

		public bool HasSeedAndKey()
		{
			if (this.DeviceConfig != null)
			{
				IXcpDeviceConfig xcpDeviceConfig = this.DeviceConfig as IXcpDeviceConfig;
				if (xcpDeviceConfig != null)
				{
					return !string.IsNullOrEmpty(xcpDeviceConfig.ProtocolConfig.SeedAndKeyPathDaq);
				}
				ICcpDeviceConfig ccpDeviceConfig = this.DeviceConfig as ICcpDeviceConfig;
				if (ccpDeviceConfig != null)
				{
					return !string.IsNullOrEmpty(ccpDeviceConfig.SeedAndKeyPathDaq);
				}
			}
			return false;
		}

		public IList<IDeviceInfo> CreateDeviceInfoList()
		{
			return this.mcDatabase.CreateDeviceInfoList();
		}

		public static IDeviceInfo CreateDeviceInfoDefaultForVxModule(IDatabase aMcDatabase)
		{
			if (aMcDatabase == null)
			{
				return null;
			}
			IDeviceInfo deviceInfo = aMcDatabase.CreateEmptyDeviceInfo();
			deviceInfo.ProtocolType = EnumProtocolType.ProtocolXcp;
			deviceInfo.TransportType = EnumXcpTransportType.kUdp;
			return deviceInfo;
		}

		public ISignal GetSignal(string signalName)
		{
			if (this.IsLoaded())
			{
				return this.mcDatabase.GetSignal(signalName);
			}
			return null;
		}

		public static bool ExtendSizeOfStaticDaqLists(IDeviceConfig aDeviceConfig, bool bSetReset)
		{
			bool result = false;
			IList<IDaqList> daqListsA2L = aDeviceConfig.Daq.DaqListsA2L;
			IList<IDaqList> daqListsCustom = aDeviceConfig.Daq.DaqListsCustom;
			if (bSetReset)
			{
				byte b = 0;
				if (aDeviceConfig.ProtocolType == EnumProtocolType.ProtocolCcp)
				{
					b = Constants.MaxDaqPidCcp;
				}
				else if (aDeviceConfig.ProtocolType == EnumProtocolType.ProtocolXcp)
				{
					b = Constants.MaxDaqPidXcp;
				}
				using (IEnumerator<IDaqList> enumerator = daqListsA2L.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						IDaqList current = enumerator.Current;
						if (current.NumberOdts != 0)
						{
							uint firstPid = (uint)current.FirstPid;
							uint num = (uint)b;
							foreach (IDaqList current2 in daqListsA2L)
							{
								if ((uint)current2.FirstPid > firstPid && (uint)current2.FirstPid < num)
								{
									num = (uint)current2.FirstPid;
								}
							}
							if ((ulong)num > (ulong)((long)(current.FirstPid + current.NumberOdts)))
							{
								byte b2 = (byte)(num - (uint)current.FirstPid);
								if (b2 != current.NumberOdts && A2LDatabase.SetMaxSizeOfStaticDaqList(daqListsCustom, (uint)current.DaqListId, b2))
								{
									result = true;
								}
							}
						}
					}
					goto IL_1D5;
				}
			}
			foreach (IDaqList current3 in daqListsA2L)
			{
				foreach (IDaqList current4 in daqListsCustom)
				{
					if (current4.DaqListId == current3.DaqListId && current4.NumberOdts != current3.NumberOdts)
					{
						current4.NumberOdts = current3.NumberOdts;
						result = true;
					}
				}
			}
			IL_1D5:
			aDeviceConfig.Daq.SetUseA2lDaqLists(!bSetReset);
			return result;
		}

		private static bool SetMaxSizeOfStaticDaqList(IEnumerable<IDaqList> daqListListCustom, uint daqListId, byte newMaxSize)
		{
			foreach (IDaqList current in daqListListCustom)
			{
				if ((uint)current.DaqListId == daqListId)
				{
					current.NumberOdts = newMaxSize;
					return true;
				}
			}
			return false;
		}

		public static CcpXcpEcuTimestampUnit GetTimestampUnitFromSampleUnit(EnumSampleUnit sampleUnit)
		{
			switch (sampleUnit)
			{
			case EnumSampleUnit.kSampleUnit1ps:
				return CcpXcpEcuTimestampUnit.TU_1Picoseconds;
			case EnumSampleUnit.kSampleUnit10ps:
				return CcpXcpEcuTimestampUnit.TU_10Picoseconds;
			case EnumSampleUnit.kSampleUnit100ps:
				return CcpXcpEcuTimestampUnit.TU_100Picoseconds;
			case EnumSampleUnit.kSampleUnit1ns:
				return CcpXcpEcuTimestampUnit.TU_1Nanoseconds;
			case EnumSampleUnit.kSampleUnit10ns:
				return CcpXcpEcuTimestampUnit.TU_10Nanoseconds;
			case EnumSampleUnit.kSampleUnit100ns:
				return CcpXcpEcuTimestampUnit.TU_100Nanoseconds;
			case EnumSampleUnit.kSampleUnit1us:
				return CcpXcpEcuTimestampUnit.TU_1Microseconds;
			case EnumSampleUnit.kSampleUnit10us:
				return CcpXcpEcuTimestampUnit.TU_10Microseconds;
			case EnumSampleUnit.kSampleUnit100us:
				return CcpXcpEcuTimestampUnit.TU_100Microseconds;
			case EnumSampleUnit.kSampleUnit1ms:
				return CcpXcpEcuTimestampUnit.TU_1Milliseconds;
			case EnumSampleUnit.kSampleUnit10ms:
				return CcpXcpEcuTimestampUnit.TU_10Milliseconds;
			case EnumSampleUnit.kSampleUnit100ms:
				return CcpXcpEcuTimestampUnit.TU_100Milliseconds;
			case EnumSampleUnit.kSampleUnit1s:
				return CcpXcpEcuTimestampUnit.TU_1Seconds;
			default:
				return CcpXcpEcuTimestampUnit.TU_Unspecified;
			}
		}

		public static EnumSampleUnit GetSampleUnitFromTimestampUnit(CcpXcpEcuTimestampUnit timestampUnit)
		{
			foreach (EnumSampleUnit enumSampleUnit in Enum.GetValues(typeof(EnumSampleUnit)))
			{
				if (timestampUnit == A2LDatabase.GetTimestampUnitFromSampleUnit(enumSampleUnit))
				{
					return enumSampleUnit;
				}
			}
			return EnumSampleUnit.kSampleUnit1s;
		}

		private static uint MakeCanId(uint canId, bool isExtended)
		{
			if (!isExtended)
			{
				return canId;
			}
			return canId | Constants.CANDbIsExtendedIdMask;
		}

		private static uint CountBytesOfSignals(IDatabase aMcDatabase, IEnumerable<CcpXcpSignal> signalList)
		{
			uint num = 0u;
			foreach (CcpXcpSignal current in signalList)
			{
				string value = current.Name.Value;
				ISignal signal = aMcDatabase.GetSignal(value);
				if (signal != null)
				{
					num += signal.ValueTypeSizeInBytes(signal.ValueType) * signal.Dimension;
				}
			}
			return num;
		}

		private static IDeviceInfo FindDeviceInfo(IEnumerable<IDeviceInfo> deviceInfos, EnumXcpTransportType xcpTransportType, string tlin)
		{
			return deviceInfos.FirstOrDefault((IDeviceInfo d) => d.TransportType == xcpTransportType && d.ProtocolType == EnumProtocolType.ProtocolXcp && string.Compare(d.TransportLayerInstanceName, tlin, StringComparison.InvariantCulture) == 0);
		}

		private static BusType GetBusTypeFromDeviceInfo(IDeviceInfo deviceInfo)
		{
			switch (deviceInfo.TransportType)
			{
			case EnumXcpTransportType.kCan:
				return BusType.Bt_CAN;
			case EnumXcpTransportType.kTcp:
			case EnumXcpTransportType.kUdp:
				return BusType.Bt_Ethernet;
			case EnumXcpTransportType.kFlexRay:
				return BusType.Bt_FlexRay;
			default:
				return BusType.Bt_None;
			}
		}
	}
}
