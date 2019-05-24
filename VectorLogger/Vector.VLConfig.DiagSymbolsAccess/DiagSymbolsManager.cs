using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Vector.DiagSymbols;
using Vector.DiagSymbols.ServiceParameterDialog;
using Vector.SymbolSelection;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.GeneralUtil;

namespace Vector.VLConfig.DiagSymbolsAccess
{
	public class DiagSymbolsManager : IDiagSymbolsManager
	{
		private const string ODX_ID = "ODX:";

		private const char ODX_SEP = '*';

		private readonly string CommonVariantQualifier = "COMMON_DIAGNOSTICS";

		public static readonly uint CANDbIsExtendedIdMask = 2147483648u;

		public static readonly uint ExtendedCANIdMask = 536870911u;

		public static readonly uint StandardCANIdMask = 2047u;

		private string helpFilePath;

		private string diagServiceParamDialogHelpId;

		private SymbolSelectionDialog symbSel;

		private IDsManager dsManager;

		private Dictionary<string, IDsDatabase> databaseList;

		private Dictionary<string, string> ecuQualifierToDatabasePath;

		private static DiagSymbolsManager sInstance;

		private static readonly string[] FILEEXTS_PATH = new string[]
		{
			Vocabulary.FileExtensionDotCDD,
			Vocabulary.FileExtensionDotMDX,
			Vocabulary.FileExtensionDotECD
		};

		private static readonly string[] FILEEXTS_ODX = new string[]
		{
			Vocabulary.FileExtensionDotPDX,
			Vocabulary.FileExtensionDotECD
		};

		public static DiagSymbolsManager Instance()
		{
			if (DiagSymbolsManager.sInstance == null)
			{
				DiagSymbolsManager.sInstance = new DiagSymbolsManager();
			}
			return DiagSymbolsManager.sInstance;
		}

		private DiagSymbolsManager()
		{
		}

		public void Init(string helpFileName, string helpIdForDiagServiceParameterDlg)
		{
			DsManagerFactory dsManagerFactory = new DsManagerFactory();
			this.dsManager = dsManagerFactory.Create();
			this.databaseList = new Dictionary<string, IDsDatabase>();
			this.ecuQualifierToDatabasePath = new Dictionary<string, string>();
			this.symbSel = new SymbolSelectionDialog();
			this.symbSel.Options.MultiSelect = true;
			this.symbSel.ShowApplyButton = false;
			this.symbSel.ShowHelpButton = false;
			this.symbSel.Options.DiagnosticsServiceFilter = DiagnosticsServiceFilter.ReadOnly;
			this.symbSel.Appearances.DiagnosticsPrimitive.Visible = false;
			this.helpFilePath = helpFileName;
			this.diagServiceParamDialogHelpId = helpIdForDiagServiceParameterDlg;
		}

		~DiagSymbolsManager()
		{
			this.CloseAllDatabases();
		}

		public DSMResult GetAllEcuQualifiersOfDatabaseFile(string absDatabasePath, out IList<string> ecuQualifiers)
		{
			ecuQualifiers = new List<string>();
			IList<IDsEcu> list = null;
			if (!this.databaseList.ContainsKey(absDatabasePath))
			{
				IDsDatabase dsDatabase;
				try
				{
					if (!File.Exists(absDatabasePath))
					{
						DSMResult result = DSMResult.FileNotFound;
						return result;
					}
					dsDatabase = this.dsManager.LoadDatabase(absDatabasePath);
					if (dsDatabase == null)
					{
						string extension = Path.GetExtension(absDatabasePath);
						DSMResult result;
						if (string.Compare(extension, Vocabulary.FileExtensionDotCDD, true) == 0)
						{
							result = DSMResult.FailedToLoadCddDescFile;
							return result;
						}
						result = DSMResult.FailedToLoadDescFile;
						return result;
					}
				}
				catch
				{
					DSMResult result = DSMResult.UnknownFileType;
					return result;
				}
				DSMResult result2 = DSMResult.NoEcuInDatabase;
				if (dsDatabase.GetEcuList(out list) == DsResult.Ok && list.Count > 0)
				{
					foreach (IDsEcu current in list)
					{
						ecuQualifiers.Add(current.Qualifier);
					}
					result2 = DSMResult.OK;
				}
				this.dsManager.UnloadDatabase(absDatabasePath);
				return result2;
			}
			if (this.databaseList[absDatabasePath].GetEcuList(out list) == DsResult.Ok && list.Count > 0)
			{
				foreach (IDsEcu current2 in list)
				{
					ecuQualifiers.Add(current2.Qualifier);
				}
				return DSMResult.OK;
			}
			return DSMResult.NoEcuInDatabase;
		}

		public void CloseAllDatabases()
		{
			this.dsManager.UnloadDatabases();
			this.databaseList.Clear();
			this.ecuQualifierToDatabasePath.Clear();
			this.symbSel.SuspendUpdate();
			this.symbSel.DataSources.CANdelaDatabases.Clear();
			this.symbSel.ResumeUpdate();
		}

		public void UpdateDatabaseConfiguration(DiagnosticsDatabaseConfiguration config, DiagnosticActionsConfiguration actionsConfig, string configFolderPath)
		{
			List<string> list = new List<string>(this.databaseList.Keys);
			foreach (DiagnosticsDatabase current in config.Databases)
			{
				string absolutePath = FileSystemServices.GetAbsolutePath(current.FilePath.Value, configFolderPath);
				if (!this.databaseList.ContainsKey(absolutePath))
				{
					IDsDatabase dsDatabase = null;
					try
					{
						if (!File.Exists(absolutePath))
						{
							continue;
						}
						dsDatabase = this.dsManager.LoadDatabase(absolutePath);
						IList<IDsEcu> list2;
						if (dsDatabase.GetEcuList(out list2) == DsResult.Ok)
						{
							current.TotalNumberOfEcusInFile = (uint)list2.Count;
						}
					}
					catch (Exception)
					{
						continue;
					}
					this.databaseList.Add(absolutePath, dsDatabase);
					this.UpdateReadPdus(current, absolutePath, actionsConfig);
				}
				else
				{
					list.Remove(absolutePath);
				}
				foreach (DiagnosticsECU current2 in current.ECUs)
				{
					if (this.IsEcuInDatabase(absolutePath, current2.Qualifier.Value))
					{
						if (!this.ecuQualifierToDatabasePath.ContainsKey(current2.Qualifier.Value))
						{
							this.ecuQualifierToDatabasePath.Add(current2.Qualifier.Value, absolutePath);
						}
						this.AddOrUpdateEcuInSymbolSelection(current2.Qualifier.Value, current2.Variant.Value);
					}
				}
			}
			foreach (string current3 in list)
			{
				this.RemoveDiagnosticsDatabase(current3);
			}
		}

		public void UpdateDatebaseEcuList(string absDatabasePath, uint channelNumberForNewEcus, IList<string> selectedEcuQualifiers, ref DiagnosticsDatabase db)
		{
			if (!this.databaseList.ContainsKey(absDatabasePath))
			{
				return;
			}
			IDsDatabase dsDatabase = this.databaseList[absDatabasePath];
			IList<IDsEcu> list = null;
			if (dsDatabase.GetEcuList(out list) != DsResult.Ok)
			{
				return;
			}
			db.TotalNumberOfEcusInFile = (uint)list.Count;
			List<string> list2 = new List<string>(selectedEcuQualifiers);
			List<DiagnosticsECU> list3 = new List<DiagnosticsECU>();
			foreach (DiagnosticsECU current in db.ECUs)
			{
				if (list2.Contains(current.Qualifier.Value))
				{
					list2.Remove(current.Qualifier.Value);
				}
				else
				{
					list3.Add(current);
				}
			}
			foreach (DiagnosticsECU current2 in list3)
			{
				if (this.ecuQualifierToDatabasePath.ContainsKey(current2.Qualifier.Value))
				{
					this.RemoveEcuInSymbolSelection(current2.Qualifier.Value);
					this.ecuQualifierToDatabasePath.Remove(current2.Qualifier.Value);
				}
				db.RemoveECU(current2);
			}
			list3.Clear();
			foreach (IDsEcu current3 in list)
			{
				if (list2.Contains(current3.Qualifier))
				{
					DiagnosticsECU diagnosticsECU = new DiagnosticsECU();
					diagnosticsECU.Qualifier.Value = current3.Qualifier;
					this.ecuQualifierToDatabasePath.Add(diagnosticsECU.Qualifier.Value, absDatabasePath);
					IList<IDsVariant> list4 = null;
					if (current3.GetVariantList(out list4) == DsResult.Ok && list4.Count != 0)
					{
						diagnosticsECU.Variant.Value = list4[0].Qualifier;
						diagnosticsECU.ChannelNumber.Value = channelNumberForNewEcus;
						db.AddECU(diagnosticsECU);
						this.AddOrUpdateEcuInSymbolSelection(diagnosticsECU.Qualifier.Value, diagnosticsECU.Variant.Value);
					}
				}
			}
		}

		public DSMResult LoadDiagnosticsDatabase(DiagnosticActionsConfiguration config, string absDatabasePath, uint channelNumber, IList<string> ecusToLoad, out DiagnosticsDatabase diagDatabase)
		{
			diagDatabase = null;
			if (this.databaseList.ContainsKey(absDatabasePath))
			{
				return DSMResult.DuplicateDatabase;
			}
			IDsDatabase dsDatabase;
			try
			{
				if (!File.Exists(absDatabasePath))
				{
					DSMResult result = DSMResult.FileNotFound;
					return result;
				}
				dsDatabase = this.dsManager.LoadDatabase(absDatabasePath);
				if (dsDatabase == null)
				{
					string extension = Path.GetExtension(absDatabasePath);
					DSMResult result;
					if (string.Compare(extension, Vocabulary.FileExtensionDotCDD, true) == 0)
					{
						result = DSMResult.FailedToLoadCddDescFile;
						return result;
					}
					result = DSMResult.FailedToLoadDescFile;
					return result;
				}
			}
			catch
			{
				DSMResult result = DSMResult.UnknownFileType;
				return result;
			}
			diagDatabase = new DiagnosticsDatabase();
			string extension2 = Path.GetExtension(absDatabasePath);
			DiagDbType value;
			if (string.Compare(extension2, Vocabulary.FileExtensionDotCDD, true) == 0)
			{
				value = DiagDbType.CDD;
			}
			else if (string.Compare(extension2, Vocabulary.FileExtensionDotPDX, true) == 0)
			{
				value = DiagDbType.PDX;
			}
			else if (string.Compare(extension2, Vocabulary.FileExtensionDotODX, true) == 0)
			{
				value = DiagDbType.ODX;
			}
			else if (string.Compare(extension2, Vocabulary.FileExtensionDotECD, true) == 0)
			{
				value = DiagDbType.OBD;
			}
			else
			{
				if (string.Compare(extension2, Vocabulary.FileExtensionDotMDX, true) != 0)
				{
					return DSMResult.UnknownFileType;
				}
				value = DiagDbType.MDX;
			}
			diagDatabase.FilePath.Value = absDatabasePath;
			diagDatabase.Type.Value = value;
			IList<IDsEcu> list = null;
			if (dsDatabase.GetEcuList(out list) == DsResult.Ok)
			{
				if (list.Count == 0)
				{
					return DSMResult.NoEcuInDatabase;
				}
				this.databaseList.Add(absDatabasePath, dsDatabase);
				diagDatabase.TotalNumberOfEcusInFile = (uint)list.Count;
				foreach (IDsEcu current in list)
				{
					if (ecusToLoad.Contains(current.Qualifier))
					{
						DiagnosticsECU diagnosticsECU = new DiagnosticsECU();
						diagnosticsECU.Qualifier.Value = current.Qualifier;
						if (this.ecuQualifierToDatabasePath.ContainsKey(diagnosticsECU.Qualifier.Value))
						{
							DSMResult result = DSMResult.DuplicateEcuQualifier;
							return result;
						}
						this.ecuQualifierToDatabasePath.Add(diagnosticsECU.Qualifier.Value, absDatabasePath);
						IList<IDsVariant> list2 = null;
						try
						{
							if (current.GetVariantList(out list2) != DsResult.Ok)
							{
								continue;
							}
						}
						catch (Exception)
						{
							continue;
						}
						if (list2.Count != 0)
						{
							diagnosticsECU.Variant.Value = list2[0].Qualifier;
							diagnosticsECU.ChannelNumber.Value = channelNumber;
							diagDatabase.AddECU(diagnosticsECU);
						}
					}
				}
			}
			if (diagDatabase.ECUs.Count == 0)
			{
				this.databaseList.Remove(absDatabasePath);
				return DSMResult.NoEcuInDatabase;
			}
			foreach (DiagnosticsECU current2 in diagDatabase.ECUs)
			{
				this.AddOrUpdateEcuInSymbolSelection(current2.Qualifier.Value, current2.Variant.Value);
			}
			this.UpdateReadPdus(diagDatabase, absDatabasePath, config);
			return DSMResult.OK;
		}

		private void UpdateReadPdus(DiagnosticsDatabase diagDatabase, string absDatabasePath, DiagnosticActionsConfiguration config)
		{
			using (IEnumerator<DiagnosticsECU> enumerator = diagDatabase.ECUs.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					DiagnosticsECU ecu = enumerator.Current;
					IList<IDsDid> source = null;
					if (this.GetDids(ecu.Qualifier.Value, ecu.Variant.Value, out source))
					{
						using (IEnumerator<DiagnosticSignalRequest> enumerator2 = (from a in config.DiagnosticActions
						where a.EcuQualifier.Value == ecu.Qualifier.Value && a is DiagnosticSignalRequest
						select a as DiagnosticSignalRequest).GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								DiagnosticSignalRequest action = enumerator2.Current;
								IDsDid dsDid = source.FirstOrDefault((IDsDid d) => action.DidId.Value == d.GetIdentifier());
								byte[] value;
								if (dsDid != null && dsDid.ComputeReadPdu(out value) == DsResult.Ok)
								{
									action.MessageData.Value = value;
								}
							}
						}
					}
				}
			}
		}

		public bool RemoveDiagnosticsDatabase(string absDatabasePath)
		{
			if (!this.databaseList.ContainsKey(absDatabasePath))
			{
				return false;
			}
			this.dsManager.UnloadDatabase(absDatabasePath);
			this.databaseList.Remove(absDatabasePath);
			this.RemoveDatabaseInSymbolSelection(absDatabasePath);
			List<string> list = new List<string>();
			foreach (string current in this.ecuQualifierToDatabasePath.Keys)
			{
				if (this.ecuQualifierToDatabasePath[current] == absDatabasePath)
				{
					list.Add(current);
				}
			}
			foreach (string current2 in list)
			{
				this.ecuQualifierToDatabasePath.Remove(current2);
			}
			return true;
		}

		public bool GetEcusInDiagnosticsDatabaseFromOEMHeuristic(string absDatabasePath, string substringToBeFound, out IList<string> ecuQualifiers)
		{
			ecuQualifiers = new List<string>();
			substringToBeFound = substringToBeFound.ToLower();
			if (this.databaseList.ContainsKey(absDatabasePath))
			{
				IDsDatabase dsDatabase = this.databaseList[absDatabasePath];
				IList<IDsEcu> list;
				if (dsDatabase.GetEcuList(out list) == DsResult.Ok)
				{
					foreach (IDsEcu current in list)
					{
						string text = current.GetOEM().ToLower();
						if (text.IndexOf(substringToBeFound) >= 0)
						{
							ecuQualifiers.Add(current.Qualifier);
						}
					}
				}
				return true;
			}
			return false;
		}

		public bool GetVariantQualifiers(string absDatabasePath, string ecuQualifier, out IList<string> variantQualifiers)
		{
			variantQualifiers = new List<string>();
			if (this.databaseList.ContainsKey(absDatabasePath))
			{
				IDsDatabase dsDatabase = this.databaseList[absDatabasePath];
				IList<IDsEcu> list = null;
				if (dsDatabase.GetEcuList(out list) != DsResult.Ok)
				{
					return false;
				}
				foreach (IDsEcu current in list)
				{
					if (current.Qualifier == ecuQualifier)
					{
						IList<IDsVariant> list2 = null;
						if (current.GetVariantList(out list2) == DsResult.Ok)
						{
							foreach (IDsVariant current2 in list2)
							{
								variantQualifiers.Add(current2.Qualifier);
							}
						}
					}
				}
			}
			return variantQualifiers.Count > 0;
		}

		public bool GetDids(string ecuQualifier, string variantQualifier, out IList<IDsDid> dids)
		{
			dids = new List<IDsDid>();
			foreach (IDsDatabase current in this.databaseList.Values)
			{
				IList<IDsEcu> list = null;
				if (current.GetEcuList(out list) != DsResult.Ok)
				{
					return false;
				}
				foreach (IDsEcu current2 in list)
				{
					if (current2.Qualifier == ecuQualifier)
					{
						IList<IDsVariant> list2 = null;
						if (current2.GetVariantList(out list2) == DsResult.Ok)
						{
							foreach (IDsVariant current3 in list2)
							{
								if (current3.Qualifier == variantQualifier)
								{
									IList<IDsDid> list3 = null;
									if (current3.GetDidList(out list3) == DsResult.Ok)
									{
										foreach (IDsDid current4 in list3)
										{
											dids.Add(current4);
										}
									}
								}
							}
						}
					}
				}
			}
			return dids.Count > 0;
		}

		public bool ResolveEcuQualifier(string absDatabasePath, string ecuQualifier)
		{
			return this.IsEcuInDatabase(absDatabasePath, ecuQualifier);
		}

		private bool IsEcuInDatabase(string absDatabasePath, string ecuQualifier)
		{
			if (this.databaseList.ContainsKey(absDatabasePath))
			{
				IList<IDsEcu> list = null;
				if (this.databaseList[absDatabasePath].GetEcuList(out list) == DsResult.Ok)
				{
					foreach (IDsEcu current in list)
					{
						if (current.Qualifier == ecuQualifier)
						{
							return true;
						}
					}
					return false;
				}
			}
			return false;
		}

		public bool ResolveEcuVariant(string absDatabasePath, string ecuQualifier, string variantQualifier)
		{
			IDsVariant dsVariant = null;
			return this.ResolveEcuVariant(absDatabasePath, ecuQualifier, variantQualifier, out dsVariant);
		}

		public bool ResolveEcuVariant(string absDatabasePath, string ecuQualifier, string variantQualifier, out IDsVariant dsVariant)
		{
			if (this.databaseList.ContainsKey(absDatabasePath))
			{
				IDsDatabase dsDatabase = this.databaseList[absDatabasePath];
				IList<IDsEcu> list = null;
				if (dsDatabase.GetEcuList(out list) == DsResult.Ok)
				{
					foreach (IDsEcu current in list)
					{
						if (current.Qualifier == ecuQualifier)
						{
							IList<IDsVariant> list2 = null;
							if (current.GetVariantList(out list2) == DsResult.Ok)
							{
								foreach (IDsVariant current2 in list2)
								{
									dsVariant = current2;
									if (current2.Qualifier == variantQualifier)
									{
										return true;
									}
								}
							}
						}
					}
				}
			}
			dsVariant = null;
			return false;
		}

		public bool ResolveService(string absDatabasePath, string ecuQualifier, string variantQualifier, string serviceQualifier, out bool hasOnlyConstParams)
		{
			hasOnlyConstParams = false;
			if (this.databaseList.ContainsKey(absDatabasePath))
			{
				IDsDatabase dsDatabase = this.databaseList[absDatabasePath];
				IList<IDsEcu> list = null;
				if (dsDatabase.GetEcuList(out list) == DsResult.Ok)
				{
					foreach (IDsEcu current in list)
					{
						if (current.Qualifier == ecuQualifier)
						{
							IList<IDsVariant> list2 = null;
							if (current.GetVariantList(out list2) == DsResult.Ok)
							{
								foreach (IDsVariant current2 in list2)
								{
									if (current2.Qualifier == variantQualifier)
									{
										IList<IDsGenericService> list3 = null;
										if (current2.GetAllSupportedServices(out list3) == DsResult.Ok)
										{
											foreach (IDsGenericService current3 in list3)
											{
												if (current3.Qualifier == serviceQualifier)
												{
													IDsServicePrimitive dsServicePrimitive = null;
													if (current3.GetRequest(out dsServicePrimitive) == DsResult.Ok)
													{
														hasOnlyConstParams = dsServicePrimitive.HasOnlyConstantParameters();
													}
													return true;
												}
											}
										}
									}
								}
							}
						}
					}
					return false;
				}
			}
			return false;
		}

		public bool ResolveSignal(string absDatabasePath, string ecuQualifier, string variantQualifier, string didId, string signal)
		{
			IList<IDsDid> list;
			if (this.GetDids(ecuQualifier, variantQualifier, out list))
			{
				foreach (IDsDid current in list)
				{
					IList<IDsStaticSignal> source;
					if (!(current.GetIdentifier() != didId) && current.GetParams(out source) == DsResult.Ok)
					{
						if (source.Any((IDsStaticSignal s) => s.Qualifier == signal))
						{
							return true;
						}
					}
				}
				return false;
			}
			return false;
		}

		public bool GetDisassembledMessageParams(string absDatabasePath, string ecuQualifier, string variantQualifier, string serviceQualifier, byte[] messageData, out IList<KeyValuePair<string, string>> paramsAndValues)
		{
			if (this.databaseList.ContainsKey(absDatabasePath))
			{
				IDsDatabase dsDatabase = this.databaseList[absDatabasePath];
				IList<IDsEcu> list;
				if (dsDatabase.GetEcuList(out list) == DsResult.Ok)
				{
					foreach (IDsEcu current in list)
					{
						IList<IDsVariant> list2;
						if (current.Qualifier == ecuQualifier && current.GetVariantList(out list2) == DsResult.Ok)
						{
							foreach (IDsVariant current2 in list2)
							{
								IList<IDsGenericService> list3;
								if (current2.Qualifier == variantQualifier && current2.GetAllSupportedServices(out list3) == DsResult.Ok)
								{
									foreach (IDsGenericService current3 in list3)
									{
										if (current3.Qualifier == serviceQualifier && this.GetDisassembledMessageParams(current3, messageData, out paramsAndValues))
										{
											return true;
										}
									}
								}
							}
						}
					}
				}
			}
			paramsAndValues = new List<KeyValuePair<string, string>>();
			return false;
		}

		public bool GetDisassembledMessageParams(IDsGenericService service, byte[] messageData, out IList<KeyValuePair<string, string>> paramsAndValues)
		{
			paramsAndValues = new List<KeyValuePair<string, string>>();
			IDsServicePrimitive pServicePrimitive;
			if (service.GetRequest(out pServicePrimitive) == DsResult.Ok)
			{
				IDsMessage dsMessage = this.dsManager.CreateMessage(pServicePrimitive);
				if (dsMessage == null)
				{
					return false;
				}
				if (messageData == null)
				{
					return false;
				}
				dsMessage.SetByteStream(ref messageData);
				if (dsMessage.Disassemble() != DsResult.Ok)
				{
					return false;
				}
				IList<IDsMessageParam> list;
				if (dsMessage.GetMessageParamList(out list) == DsResult.Ok)
				{
					foreach (IDsMessageParam current in list)
					{
						if (current.ServiceParam.ServiceParamKind == ServiceParamKind.Leaf)
						{
							IDsServiceParamLeaf dsServiceParamLeaf = current.ServiceParam as IDsServiceParamLeaf;
							if (dsServiceParamLeaf != null)
							{
								ConversionType conversionType = dsServiceParamLeaf.ConversionType;
								string qualifier = current.ServiceParam.Qualifier;
								ConversionType conversionType2 = conversionType;
								switch (conversionType2)
								{
								case ConversionType.Identity:
								case ConversionType.TextTable:
								case ConversionType.DtcTable:
								case ConversionType.ValueTable:
								case ConversionType.Linear:
									break;
								case ConversionType.GroupOfDtcTable:
								case ConversionType.ValueTableInt:
									goto IL_114;
								default:
									if (conversionType2 != ConversionType.TableRowTable)
									{
										goto IL_114;
									}
									break;
								}
								string value;
								if (current.GetSymbolicValue(out value) == DsResult.Ok)
								{
									paramsAndValues.Add(new KeyValuePair<string, string>(qualifier, value));
									continue;
								}
								paramsAndValues.Add(new KeyValuePair<string, string>(qualifier, "?"));
								continue;
								IL_114:
								paramsAndValues.Add(new KeyValuePair<string, string>(qualifier, "?"));
							}
						}
					}
					return true;
				}
			}
			return false;
		}

		public DSMResult GetServiceComplexityStatus(string ecuQualifier, string variantQualifier, string serviceQualifier, out bool hasOnlyConstantParams)
		{
			hasOnlyConstantParams = false;
			IDsGenericService dsGenericService;
			IDsServicePrimitive dsServicePrimitive;
			if (!this.GetServiceObject(ecuQualifier, variantQualifier, serviceQualifier, out dsGenericService) || dsGenericService.GetRequest(out dsServicePrimitive) != DsResult.Ok)
			{
				return DSMResult.ServiceNotSupported;
			}
			hasOnlyConstantParams = dsServicePrimitive.HasOnlyConstantParameters();
			switch (dsServicePrimitive.IsTooComplicated())
			{
			case ServiceComplexResult.kSCR_Ok:
				return DSMResult.OK;
			case ServiceComplexResult.kSCR_TooLong:
				return DSMResult.ServiceTooLong;
			case ServiceComplexResult.kSCR_TooComplexIterator:
				return DSMResult.ServiceTooComplexIterator;
			case ServiceComplexResult.kSCR_TooComplexMux:
				return DSMResult.ServiceTooComplexMultiplexor;
			default:
				return DSMResult.ServiceNotSupported;
			}
		}

		public DiagSessionType GetServiceSessionTypeFromDB(string ecuQualifier, string variantQualifier, string serviceQualifier, byte[] messageData)
		{
			IDsGenericService dsGenericService;
			if (this.GetServiceObject(ecuQualifier, variantQualifier, serviceQualifier, out dsGenericService))
			{
				SessionType minRequiredSession = dsGenericService.GetMinRequiredSession(ref messageData);
				if (minRequiredSession == SessionType.kST_Default)
				{
					return DiagSessionType.Default;
				}
				if (minRequiredSession == SessionType.kST_Extended)
				{
					return DiagSessionType.Extended;
				}
			}
			return DiagSessionType.Default;
		}

		public bool GetValidCommInterfaceQualifiers(string ecuQualifier, out IList<string> commInterfaceQualifiers)
		{
			IList<string> list;
			return this.GetAllCommInterfaceQualifiers(ecuQualifier, out commInterfaceQualifiers, out list);
		}

		public bool GetAllCommInterfaceQualifiers(string ecuQualifier, out IList<string> validCommInterfaceQualifiers, out IList<string> invalidCommInterfaceQualifiers)
		{
			validCommInterfaceQualifiers = new List<string>();
			invalidCommInterfaceQualifiers = new List<string>();
			if (this.ecuQualifierToDatabasePath.ContainsKey(ecuQualifier))
			{
				string key = this.ecuQualifierToDatabasePath[ecuQualifier];
				if (this.databaseList.ContainsKey(key))
				{
					IDsDatabase dsDatabase = this.databaseList[key];
					IList<IDsEcu> list = null;
					if (dsDatabase.GetEcuList(out list) == DsResult.Ok)
					{
						foreach (IDsEcu current in list)
						{
							if (current.Qualifier == ecuQualifier)
							{
								IList<IDsCommInterface> list2 = null;
								if (current.GetCommInterfaceList(out list2) == DsResult.Ok)
								{
									foreach (IDsCommInterface current2 in list2)
									{
										if (current2.GetBusType() == Vector.DiagSymbols.BusType.Can && current2.GetProtocolStandard() != ProtocolStandard.J1939 && current2.GetProtocolStandard() != ProtocolStandard.OBD2)
										{
											CanAddressMode canAddressMode = current2.GetCanAddressMode();
											if (canAddressMode != CanAddressMode.Normal && canAddressMode != CanAddressMode.NormalFixed && canAddressMode != CanAddressMode.Extended)
											{
												invalidCommInterfaceQualifiers.Add(current2.Qualifier);
											}
											else
											{
												validCommInterfaceQualifiers.Add(current2.Qualifier);
											}
										}
									}
									return true;
								}
							}
						}
						return false;
					}
				}
			}
			return false;
		}

		public bool ResolveCommInterfaceQualifier(string ecuQualifier, string interfaceQualifier)
		{
			IList<string> list = null;
			return this.GetValidCommInterfaceQualifiers(ecuQualifier, out list) && list.Contains(interfaceQualifier);
		}

		public bool GetCommInterfaceName(string ecuQualifier, string commInterfaceQualifier, out string commInterfaceName)
		{
			commInterfaceName = "";
			if (this.ecuQualifierToDatabasePath.ContainsKey(ecuQualifier))
			{
				string key = this.ecuQualifierToDatabasePath[ecuQualifier];
				if (this.databaseList.ContainsKey(key))
				{
					IDsDatabase dsDatabase = this.databaseList[key];
					IList<IDsEcu> list = null;
					if (dsDatabase.GetEcuList(out list) == DsResult.Ok)
					{
						foreach (IDsEcu current in list)
						{
							if (current.Qualifier == ecuQualifier)
							{
								IList<IDsCommInterface> list2 = null;
								if (current.GetCommInterfaceList(out list2) == DsResult.Ok)
								{
									foreach (IDsCommInterface current2 in list2)
									{
										if (current2.Qualifier == commInterfaceQualifier)
										{
											commInterfaceName = current2.Name;
											return true;
										}
									}
								}
							}
						}
						return false;
					}
				}
			}
			return false;
		}

		public bool GetTesterPresentMessage(string ecuQualifier, string variantQualifier, ref DiagnosticCommParamsECU ecuCommParams)
		{
			if (this.ecuQualifierToDatabasePath.ContainsKey(ecuQualifier))
			{
				string key = this.ecuQualifierToDatabasePath[ecuQualifier];
				if (this.databaseList.ContainsKey(key))
				{
					IDsDatabase dsDatabase = this.databaseList[key];
					IList<IDsEcu> list = null;
					if (dsDatabase.GetEcuList(out list) == DsResult.Ok)
					{
						foreach (IDsEcu current in list)
						{
							IList<IDsVariant> list2;
							if (current.Qualifier == ecuQualifier && current.GetVariantList(out list2) == DsResult.Ok)
							{
								foreach (IDsVariant current2 in list2)
								{
									byte[] array;
									if (current2.Qualifier == variantQualifier && current2.GetPhysTesterPresentMessage(out array) == DsResult.Ok)
									{
										return true;
									}
								}
							}
						}
						return false;
					}
				}
			}
			return false;
		}

		public bool GetCommInterfaceParamValuesFromDb(string ecuQualifier, string commInterfaceQualifier, ref DiagnosticCommParamsECU ecuCommParams)
		{
			if (this.ecuQualifierToDatabasePath.ContainsKey(ecuQualifier))
			{
				string key = this.ecuQualifierToDatabasePath[ecuQualifier];
				if (this.databaseList.ContainsKey(key))
				{
					IDsDatabase dsDatabase = this.databaseList[key];
					IList<IDsEcu> list = null;
					if (dsDatabase.GetEcuList(out list) == DsResult.Ok)
					{
						foreach (IDsEcu current in list)
						{
							if (current.Qualifier == ecuQualifier)
							{
								IList<IDsCommInterface> list2 = null;
								if (current.GetCommInterfaceList(out list2) == DsResult.Ok)
								{
									uint num = 0u;
									foreach (IDsCommInterface current2 in list2)
									{
										if (current2.Qualifier == commInterfaceQualifier)
										{
											ecuCommParams.ResetHasFromDbFlags();
											ecuCommParams.InterfaceQualifier.Value = commInterfaceQualifier;
											ecuCommParams.UseParamValuesFromDb.Value = true;
											ProtocolStandard protocolStandard = current2.GetProtocolStandard();
											if (protocolStandard == ProtocolStandard.Undef)
											{
												protocolStandard = current.GetProtocolStandard();
											}
											if (protocolStandard != ProtocolStandard.Undef)
											{
												ecuCommParams.HasDiagProtocolFromDb = true;
												ecuCommParams.DiagProtocol.Value = this.GetDatamodelDiagProtTypeForInternalProtType(protocolStandard);
											}
											CanAddressMode canAddressMode = current2.GetCanAddressMode();
											if (canAddressMode != CanAddressMode.Undefined)
											{
												ecuCommParams.HasDiagAddrModeFromDb = true;
												ecuCommParams.DiagAddressingMode.Value = this.GetDatamodelDiagAddrModeForInternalMode(canAddressMode);
												if (ecuCommParams.DiagAddressingMode.Value == DiagnosticsAddressingMode.Extended)
												{
													uint num2;
													uint value;
													if (current2.GetCanRequestAddress(out num2, out value) == DsResult.Ok)
													{
														ecuCommParams.HasExtAddressingModeRqExtension = true;
														ecuCommParams.ExtAddressingModeRqExtension.Value = value;
													}
													if (current2.GetCanResponseAddress(out num2, out value) == DsResult.Ok)
													{
														ecuCommParams.HasExtAddressingModeRsExtension = true;
														ecuCommParams.ExtAddressingModeRsExtension.Value = value;
													}
												}
											}
											if (current2.GetP2Max(out num) == DsResult.Ok)
											{
												ecuCommParams.P2Timeout.Value = num;
												ecuCommParams.HasP2TimeoutFromDb = true;
											}
											if (current2.GetP2Star(out num) == DsResult.Ok)
											{
												ecuCommParams.P2Extension.Value = num;
												ecuCommParams.HasP2ExtensionFromDb = true;
											}
											if (current2.GetCanPhysReqId(out num) == DsResult.Ok)
											{
												ecuCommParams.PhysRequestMsgIsExtendedId.Value = ((DiagSymbolsManager.CANDbIsExtendedIdMask & num) == DiagSymbolsManager.CANDbIsExtendedIdMask);
												if (ecuCommParams.PhysRequestMsgIsExtendedId.Value)
												{
													ecuCommParams.PhysRequestMsgId.Value = (num & DiagSymbolsManager.ExtendedCANIdMask);
												}
												else
												{
													ecuCommParams.PhysRequestMsgId.Value = (num & DiagSymbolsManager.StandardCANIdMask);
												}
												ecuCommParams.HasPhysRequestMsgFromDb = true;
											}
											if (current2.GetCanRespUSDTId(out num) == DsResult.Ok)
											{
												ecuCommParams.ResponseMsgIsExtendedId.Value = ((DiagSymbolsManager.CANDbIsExtendedIdMask & num) == DiagSymbolsManager.CANDbIsExtendedIdMask);
												if (ecuCommParams.ResponseMsgIsExtendedId.Value)
												{
													ecuCommParams.ResponseMsgId.Value = (num & DiagSymbolsManager.ExtendedCANIdMask);
												}
												else
												{
													ecuCommParams.ResponseMsgId.Value = (num & DiagSymbolsManager.StandardCANIdMask);
												}
												ecuCommParams.HasRespMsgFromDb = true;
											}
											bool value2;
											if (current2.GetCanFillerByteHandling(out value2) == DsResult.Ok)
											{
												ecuCommParams.UsePaddingBytes.Value = value2;
												ecuCommParams.HasUsePaddingBytesFromDb = true;
											}
											byte value3;
											if (current2.GetCanFillerByte(out value3) == DsResult.Ok)
											{
												ecuCommParams.PaddingByteValue.Value = value3;
												ecuCommParams.HasPaddingBytesFromDb = true;
											}
											if (current2.GetCanFirstConsecutiveFrameValue(out num) == DsResult.Ok)
											{
												ecuCommParams.FirstConsecFrameValue.Value = (byte)num;
												ecuCommParams.HasFirstConsecFrameValueFromDb = true;
											}
											return true;
										}
									}
								}
							}
						}
						return false;
					}
				}
			}
			return false;
		}

		public bool GetSessionControlParamValuesFromDb(string ecuQualifier, string variantQualifier, ref DiagnosticCommParamsECU ecuCommParams)
		{
			string value = "default";
			uint num = 4097u;
			bool flag = false;
			string value2 = "extended";
			uint num2 = 4099u;
			bool flag2 = false;
			IList<IDsSessionService> list = null;
			if (this.GetSessionServiceList(ecuQualifier, variantQualifier, out list))
			{
				if (ecuCommParams.DiagProtocol.Value != DiagnosticsProtocolType.UDS)
				{
					using (IEnumerator<IDsSessionService> enumerator = list.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							IDsSessionService current = enumerator.Current;
							string text = current.Name.ToLower(CultureInfo.InvariantCulture);
							IDsServicePrimitive pServicePrimitive;
							if (!flag && text.IndexOf(value) >= 0 && current.GetRequest(out pServicePrimitive) == DsResult.Ok)
							{
								IDsMessage dsMessage = this.dsManager.CreateMessage(pServicePrimitive);
								dsMessage.Assemble();
								byte[] array;
								if (dsMessage.GetByteStream(out array) == DsResult.Ok)
								{
									Array.Reverse(array);
									Array.Resize<byte>(ref array, 8);
									ecuCommParams.DefaultSessionId.Value = BitConverter.ToUInt64(array, 0);
									ecuCommParams.DefaultSessionName.Value = current.Qualifier;
									ecuCommParams.DefaultSessionSource.Value = DiagnosticsSessionSource.DatabaseDefined;
									flag = true;
								}
							}
							IDsServicePrimitive pServicePrimitive2;
							if (!flag2 && text.IndexOf(value2) >= 0 && current.GetRequest(out pServicePrimitive2) == DsResult.Ok)
							{
								IDsMessage dsMessage2 = this.dsManager.CreateMessage(pServicePrimitive2);
								dsMessage2.Assemble();
								byte[] array2;
								if (dsMessage2.GetByteStream(out array2) == DsResult.Ok)
								{
									Array.Reverse(array2);
									Array.Resize<byte>(ref array2, 8);
									ecuCommParams.ExtendedSessionId.Value = BitConverter.ToUInt64(array2, 0);
									ecuCommParams.ExtendedSessionName.Value = current.Qualifier;
									ecuCommParams.ExtendedSessionSource.Value = DiagnosticsSessionSource.DatabaseDefined;
									flag2 = true;
								}
							}
							if (flag && flag2)
							{
								bool result = true;
								return result;
							}
						}
						return false;
					}
				}
				foreach (IDsSessionService current2 in list)
				{
					IDsServicePrimitive pServicePrimitive3;
					if (current2.GetRequest(out pServicePrimitive3) == DsResult.Ok)
					{
						IDsMessage dsMessage3 = this.dsManager.CreateMessage(pServicePrimitive3);
						dsMessage3.Assemble();
						byte[] array3;
						if (dsMessage3.GetByteStream(out array3) == DsResult.Ok)
						{
							Array.Reverse(array3);
							Array.Resize<byte>(ref array3, 8);
							ulong num3 = BitConverter.ToUInt64(array3, 0);
							if (!flag && (ulong)num == num3)
							{
								ecuCommParams.DefaultSessionId.Value = num3;
								ecuCommParams.DefaultSessionName.Value = current2.Qualifier;
								ecuCommParams.DefaultSessionSource.Value = DiagnosticsSessionSource.DatabaseDefined;
								flag = true;
							}
							if (!flag2 && (ulong)num2 == num3)
							{
								ecuCommParams.ExtendedSessionId.Value = num3;
								ecuCommParams.ExtendedSessionName.Value = current2.Qualifier;
								ecuCommParams.ExtendedSessionSource.Value = DiagnosticsSessionSource.DatabaseDefined;
								flag2 = true;
							}
						}
					}
					if (flag && flag2)
					{
						bool result = true;
						return result;
					}
				}
				return false;
			}
			return false;
		}

		public bool GetAvailableSessions(string ecuQualifier, string variantQualifier, out Dictionary<string, ulong> sessions)
		{
			sessions = new Dictionary<string, ulong>();
			IList<IDsSessionService> list = null;
			if (this.GetSessionServiceList(ecuQualifier, variantQualifier, out list))
			{
				foreach (IDsSessionService current in list)
				{
					IDsServicePrimitive pServicePrimitive;
					if (current.GetRequest(out pServicePrimitive) == DsResult.Ok)
					{
						IDsMessage dsMessage = this.dsManager.CreateMessage(pServicePrimitive);
						dsMessage.Assemble();
						byte[] array;
						if (dsMessage.GetByteStream(out array) == DsResult.Ok)
						{
							Array.Reverse(array);
							Array.Resize<byte>(ref array, 8);
							ulong value = BitConverter.ToUInt64(array, 0);
							sessions.Add(current.Qualifier, value);
						}
					}
				}
				return true;
			}
			return false;
		}

		public bool GetSessionNameForQualifier(string ecuQualifier, string variantQualifier, string sessionQualifier, out string sessionName)
		{
			sessionName = "";
			IList<IDsSessionService> list = null;
			if (this.GetSessionServiceList(ecuQualifier, variantQualifier, out list))
			{
				foreach (IDsSessionService current in list)
				{
					if (current.Qualifier == sessionQualifier)
					{
						sessionName = current.Name;
						return true;
					}
				}
				return false;
			}
			return false;
		}

		private bool GetSessionServiceList(string ecuQualifier, string variantQualifier, out IList<IDsSessionService> sessionServiceList)
		{
			sessionServiceList = null;
			if (this.ecuQualifierToDatabasePath.ContainsKey(ecuQualifier))
			{
				string key = this.ecuQualifierToDatabasePath[ecuQualifier];
				if (this.databaseList.ContainsKey(key))
				{
					IDsDatabase dsDatabase = this.databaseList[key];
					IList<IDsEcu> list = null;
					if (dsDatabase.GetEcuList(out list) == DsResult.Ok)
					{
						foreach (IDsEcu current in list)
						{
							IList<IDsVariant> list2;
							if (current.Qualifier == ecuQualifier && current.GetVariantList(out list2) == DsResult.Ok)
							{
								foreach (IDsVariant current2 in list2)
								{
									if (current2.Qualifier == variantQualifier)
									{
										IDsSessionClass dsSessionClass = null;
										if (current2.GetSessionClass(out dsSessionClass) == DsResult.Ok)
										{
											dsSessionClass.GetServiceList(out sessionServiceList);
											return true;
										}
									}
								}
							}
						}
						return false;
					}
				}
			}
			return false;
		}

		public bool GetDtcLength(string absDatabasePath, string ecuQualifier, string variantQualifier, out uint dtclength)
		{
			IDsVariant dsVariant;
			if (!this.ResolveEcuVariant(absDatabasePath, ecuQualifier, variantQualifier, out dsVariant))
			{
				dtclength = 0u;
				return false;
			}
			IDsFaultMemoryClass dsFaultMemoryClass;
			dsVariant.GetFaultMemoryClass(out dsFaultMemoryClass);
			dtclength = dsFaultMemoryClass.GetDTCLength();
			return true;
		}

		public bool GetSignalDefinition(string ecuQualifier, string variantQualifier, string didId, string signalName, out SignalDefinition signalDefinition)
		{
			double factor = 1.0;
			double offset = 0.0;
			bool hasLinearConversion = false;
			IList<IDsDid> list = null;
			if (this.GetDids(ecuQualifier, variantQualifier, out list))
			{
				foreach (IDsDid current in list)
				{
					if (current.GetIdentifier() == didId)
					{
						IList<IDsStaticSignal> list2 = null;
						if (current.GetParams(out list2) == DsResult.Ok)
						{
							foreach (IDsStaticSignal current2 in list2)
							{
								if (current2.Qualifier == signalName)
								{
									uint bitPosition = (uint)current2.GetBitPosition();
									bool isMotorola = current2.IsByteOrderHighLow();
									bool isSigned = current2.IsSigned();
									uint bitLength = current2.GetBitLength();
									if (current2.ConversionType == ConversionType.Linear)
									{
										hasLinearConversion = true;
										DsResult linearConversionValues = current2.GetLinearConversionValues(out factor, out offset);
										if (linearConversionValues != DsResult.Ok)
										{
											hasLinearConversion = false;
											factor = 1.0;
											offset = 0.0;
										}
									}
									bool isIntegerType = false;
									switch (current2.EncodingType)
									{
									case EncodingType.ENC_none:
									case EncodingType.ENC_uns:
									case EncodingType.ENC_sgn:
									case EncodingType.ENC_bcd:
									case EncodingType.ENC_asc:
									case EncodingType.ENC_ucs2:
									case EncodingType.ENC_utf8:
										isIntegerType = true;
										break;
									}
									signalDefinition = new SignalDefinition();
									signalDefinition.SetSignal(new MessageDefinition(0u), bitPosition, bitLength, isMotorola, isSigned, isIntegerType, string.Empty, factor, offset, false, 0, false, hasLinearConversion);
									return true;
								}
							}
						}
					}
				}
			}
			signalDefinition = null;
			return false;
		}

		private IDsServiceParam GetServiceParamFromMux(IDsServiceParam responseParam, string signalName)
		{
			if (responseParam is IDsServiceParamMux)
			{
				IDsServiceParamMux dsServiceParamMux = responseParam as IDsServiceParamMux;
				IList<string> list;
				dsServiceParamMux.GetMuxKeyList(out list);
				foreach (string current in list)
				{
					dsServiceParamMux.SetSelectorKey(current);
					IList<IDsServiceParam> list2;
					dsServiceParamMux.GetParamInCurrentMuxCaseList(out list2);
					foreach (IDsServiceParam current2 in list2)
					{
						if (current2.Qualifier == signalName)
						{
							return current2;
						}
					}
				}
				return responseParam;
			}
			return responseParam;
		}

		public bool EditServiceParameter(string ecuQualifier, string variantQualifier, string serviceQualifier, ref byte[] messageData, ref DiagSessionType session)
		{
			IDsGenericService dsGenericService = null;
			if (this.GetServiceObject(ecuQualifier, variantQualifier, serviceQualifier, out dsGenericService))
			{
				IDsServicePrimitive pServicePrimitive = null;
				if (dsGenericService != null && dsGenericService.GetRequest(out pServicePrimitive) == DsResult.Ok)
				{
					IDsMessage dsMessage = this.dsManager.CreateMessage(pServicePrimitive);
					if (messageData != null)
					{
						dsMessage.SetByteStream(ref messageData);
						dsMessage.Disassemble();
					}
					else
					{
						dsMessage.Assemble();
					}
					DiagServiceParameterDialog diagServiceParameterDialog = new DiagServiceParameterDialog(this.dsManager, dsGenericService, dsMessage, DiagSymbolsManager.GetInternalSessionType(session));
					DiagServiceParameterDialog.SetHelpParameters(this.helpFilePath, HelpNavigator.TopicId, this.diagServiceParamDialogHelpId);
					if (DialogResult.Cancel == diagServiceParameterDialog.ShowDialog())
					{
						return false;
					}
					session = DiagSymbolsManager.GetDataModelSessionType(diagServiceParameterDialog.GetSessionType());
					dsMessage.GetByteStream(out messageData);
					Console.WriteLine("New Message: " + messageData);
					return true;
				}
			}
			return false;
		}

		public bool InitDefaultServiceParameter(string ecuQualifier, string variantQualifier, string serviceQualifier, out byte[] messageData, out DiagSessionType session)
		{
			messageData = null;
			session = DiagSessionType.DynamicFromDB;
			IDsGenericService dsGenericService = null;
			if (this.GetServiceObject(ecuQualifier, variantQualifier, serviceQualifier, out dsGenericService))
			{
				IDsServicePrimitive pServicePrimitive = null;
				if (dsGenericService.GetRequest(out pServicePrimitive) == DsResult.Ok)
				{
					IDsMessage dsMessage = this.dsManager.CreateMessage(pServicePrimitive);
					dsMessage.Assemble();
					dsMessage.GetByteStream(out messageData);
					return true;
				}
			}
			return false;
		}

		private static DiagSessionType GetDataModelSessionType(SessionType diagSymbolsSessionType)
		{
			switch (diagSymbolsSessionType)
			{
			case SessionType.kST_Default:
				return DiagSessionType.Default;
			case SessionType.kST_Extended:
				return DiagSessionType.Extended;
			case SessionType.kST_Dynamic:
				return DiagSessionType.DynamicFromDB;
			default:
				return DiagSessionType.Unknown;
			}
		}

		private static SessionType GetInternalSessionType(DiagSessionType dataModelSessionType)
		{
			switch (dataModelSessionType)
			{
			case DiagSessionType.Default:
				return SessionType.kST_Default;
			case DiagSessionType.Extended:
				return SessionType.kST_Extended;
			case DiagSessionType.DynamicFromDB:
				return SessionType.kST_Dynamic;
			default:
				return SessionType.kST_Unknown;
			}
		}

		private bool GetServiceObject(string ecuQualifier, string variantQualifier, string serviceQualifier, out IDsGenericService serviceObj)
		{
			serviceObj = null;
			if (!this.ecuQualifierToDatabasePath.ContainsKey(ecuQualifier))
			{
				return false;
			}
			string key = this.ecuQualifierToDatabasePath[ecuQualifier];
			if (!this.databaseList.ContainsKey(key))
			{
				return false;
			}
			IDsDatabase dsDatabase = this.databaseList[key];
			IList<IDsEcu> list = null;
			if (dsDatabase.GetEcuList(out list) == DsResult.Ok)
			{
				foreach (IDsEcu current in list)
				{
					if (current.Qualifier == ecuQualifier)
					{
						IList<IDsVariant> list2 = null;
						if (current.GetVariantList(out list2) == DsResult.Ok)
						{
							foreach (IDsVariant current2 in list2)
							{
								if (current2.Qualifier == variantQualifier)
								{
									IList<IDsGenericService> list3 = null;
									if (current2.GetAllSupportedServices(out list3) == DsResult.Ok)
									{
										foreach (IDsGenericService current3 in list3)
										{
											if (current3.Qualifier == serviceQualifier)
											{
												serviceObj = current3;
												return true;
											}
										}
									}
								}
							}
						}
					}
				}
				return false;
			}
			return false;
		}

		private static string GetSupportedFileTypes()
		{
			return string.Join(", ", DiagSymbolsManager.FILEEXTS_PATH) + ", " + string.Join(", ", DiagSymbolsManager.FILEEXTS_ODX);
		}

		private static string GetInfoString(string filePath, string ecuQualifier)
		{
			bool flag;
			if (!DiagSymbolsManager.IsSupportedFileType(Path.GetExtension(filePath), out flag))
			{
				return "";
			}
			if (flag)
			{
				return string.Concat(new object[]
				{
					"ODX:",
					ecuQualifier,
					'*',
					filePath
				});
			}
			return filePath;
		}

		private static bool IsSupportedFileType(string fileExtension, out bool useODXstring)
		{
			useODXstring = false;
			if (DiagSymbolsManager.FILEEXTS_PATH.Contains(fileExtension, StringComparer.OrdinalIgnoreCase))
			{
				useODXstring = false;
				return true;
			}
			if (DiagSymbolsManager.FILEEXTS_ODX.Contains(fileExtension, StringComparer.OrdinalIgnoreCase))
			{
				useODXstring = true;
				return true;
			}
			return false;
		}

		private static string ExtractFilePathFromEcuPath(string candelaPath)
		{
			if (!candelaPath.StartsWith("ODX:"))
			{
				return candelaPath;
			}
			int num = candelaPath.IndexOf('*');
			if (num + 1 < candelaPath.Length)
			{
				return candelaPath.Substring(num + 1);
			}
			return "";
		}

		public bool GetAbsFilePathOrInfoStringForEcuQualifier(string ecuQualifier, out string absPathOrInfoStr)
		{
			absPathOrInfoStr = string.Empty;
			if (!this.ecuQualifierToDatabasePath.ContainsKey(ecuQualifier))
			{
				return false;
			}
			string filePath = this.ecuQualifierToDatabasePath[ecuQualifier];
			absPathOrInfoStr = DiagSymbolsManager.GetInfoString(filePath, ecuQualifier);
			return !string.IsNullOrEmpty(absPathOrInfoStr);
		}

		public bool SelectDiagnosticServiceAction(bool isMultiSelectEnabled, out List<DiagnosticAction> actions)
		{
			this.symbSel.SelectableSymbolTypes = SymbolTypes.DiagnosticsService;
			this.symbSel.Options.MultiSelect = isMultiSelectEnabled;
			actions = new List<DiagnosticAction>();
			if (this.symbSel.ShowDialog() != DialogResult.OK)
			{
				return false;
			}
			if (this.symbSel.SelectedItems.Count == 0)
			{
				return false;
			}
			for (int i = 0; i < this.symbSel.SelectedItems.Count; i++)
			{
				DiagnosticAction diagnosticAction = new DiagnosticAction();
				diagnosticAction.ServiceQualifier.Value = this.symbSel.SelectedItems[i].SignalName;
				diagnosticAction.EcuQualifier.Value = this.symbSel.SelectedItems[i].DatabaseName;
				if (this.ecuQualifierToDatabasePath.ContainsKey(diagnosticAction.EcuQualifier.Value))
				{
					diagnosticAction.DatabasePath.Value = this.ecuQualifierToDatabasePath[diagnosticAction.EcuQualifier.Value];
					actions.Add(diagnosticAction);
				}
			}
			return actions.Count > 0;
		}

		public bool SelectDiagnosticSignal(out List<string> databasePaths, out List<string> ecuQualifiers, out List<string> serviceQualifiers, out List<string> parameterAddresses)
		{
			this.symbSel.SelectableSymbolTypes = SymbolTypes.DiagnosticsParameter;
			databasePaths = new List<string>();
			ecuQualifiers = new List<string>();
			serviceQualifiers = new List<string>();
			parameterAddresses = new List<string>();
			if (this.symbSel.ShowDialog() != DialogResult.OK)
			{
				return false;
			}
			if (this.symbSel.SelectedItems.Count == 0)
			{
				return false;
			}
			for (int i = 0; i < this.symbSel.SelectedItems.Count; i++)
			{
				if (this.ecuQualifierToDatabasePath.ContainsKey(this.symbSel.SelectedItems[i].DatabaseName))
				{
					ecuQualifiers.Add(this.symbSel.SelectedItems[i].DatabaseName);
					serviceQualifiers.Add(this.symbSel.SelectedItems[i].SignalName);
					parameterAddresses.Add(this.symbSel.SelectedItems[i].Name);
					string item = this.ecuQualifierToDatabasePath[this.symbSel.SelectedItems[i].DatabaseName];
					databasePaths.Add(item);
				}
			}
			return parameterAddresses.Count > 0;
		}

		public bool SelectDiagnosticsTroubleCode(out List<string> databasePaths, out List<string> ecuQualifiers, out List<string> troubleCodes)
		{
			this.symbSel.SelectableSymbolTypes = SymbolTypes.DiagnosticsDtc;
			databasePaths = new List<string>();
			ecuQualifiers = new List<string>();
			troubleCodes = new List<string>();
			if (this.symbSel.ShowDialog() != DialogResult.OK)
			{
				return false;
			}
			for (int i = 0; i < this.symbSel.SelectedItems.Count; i++)
			{
				if (this.ecuQualifierToDatabasePath.ContainsKey(this.symbSel.SelectedItems[i].NodeName))
				{
					ecuQualifiers.Add(this.symbSel.SelectedItems[i].NodeName);
					troubleCodes.Add(this.symbSel.SelectedItems[i].SignalName);
					string item = this.ecuQualifierToDatabasePath[this.symbSel.SelectedItems[i].NodeName];
					databasePaths.Add(item);
				}
			}
			return troubleCodes.Count > 0;
		}

		private void RemoveDatabaseInSymbolSelection(string absDatabasePath)
		{
			List<CANdelaDatabase> list = new List<CANdelaDatabase>();
			foreach (CANdelaDatabase current in this.symbSel.DataSources.CANdelaDatabases)
			{
				if (string.Compare(DiagSymbolsManager.ExtractFilePathFromEcuPath(current.Path), absDatabasePath, true) == 0)
				{
					list.Add(current);
				}
			}
			this.symbSel.SuspendUpdate();
			foreach (CANdelaDatabase current2 in list)
			{
				this.symbSel.DataSources.CANdelaDatabases.Remove(current2);
			}
			this.symbSel.ResumeUpdate();
		}

		private void AddOrUpdateEcuInSymbolSelection(string ecuQualifer, string variant)
		{
			string text;
			this.GetAbsFilePathOrInfoStringForEcuQualifier(ecuQualifer, out text);
			foreach (CANdelaDatabase current in this.symbSel.DataSources.CANdelaDatabases)
			{
				if (current.Path == text && current.EcuQualifier == ecuQualifer)
				{
					if (string.IsNullOrEmpty(variant))
					{
						variant = this.CommonVariantQualifier;
					}
					if (current.VariantQualifier != variant)
					{
						this.symbSel.SuspendUpdate();
						current.VariantQualifier = variant;
						this.symbSel.ResumeUpdate();
					}
					return;
				}
			}
			CANdelaDatabase cANdelaDatabase = new CANdelaDatabase();
			cANdelaDatabase.EcuQualifier = ecuQualifer;
			cANdelaDatabase.VariantQualifier = variant;
			cANdelaDatabase.Path = text;
			this.symbSel.SuspendUpdate();
			this.symbSel.DataSources.CANdelaDatabases.Add(cANdelaDatabase);
			this.symbSel.ResumeUpdate();
		}

		private void RemoveEcuInSymbolSelection(string ecuQualifer)
		{
			CANdelaDatabase cANdelaDatabase = null;
			string strB;
			if (!this.GetAbsFilePathOrInfoStringForEcuQualifier(ecuQualifer, out strB))
			{
				return;
			}
			foreach (CANdelaDatabase current in this.symbSel.DataSources.CANdelaDatabases)
			{
				if (string.Compare(current.Path, strB, true) == 0 && current.EcuQualifier == ecuQualifer)
				{
					cANdelaDatabase = current;
					break;
				}
			}
			if (cANdelaDatabase != null)
			{
				this.symbSel.SuspendUpdate();
				this.symbSel.DataSources.CANdelaDatabases.Remove(cANdelaDatabase);
				this.symbSel.ResumeUpdate();
			}
		}

		private DiagnosticsProtocolType GetDatamodelDiagProtTypeForInternalProtType(ProtocolStandard internalType)
		{
			switch (internalType)
			{
			case ProtocolStandard.KWP:
				return DiagnosticsProtocolType.KWP;
			case ProtocolStandard.UDS:
				return DiagnosticsProtocolType.UDS;
			default:
				return DiagnosticsProtocolType.Undefined;
			}
		}

		private DiagnosticsAddressingMode GetDatamodelDiagAddrModeForInternalMode(CanAddressMode internalMode)
		{
			switch (internalMode)
			{
			case CanAddressMode.Normal:
				return DiagnosticsAddressingMode.Normal;
			case CanAddressMode.Extended:
				return DiagnosticsAddressingMode.Extended;
			case CanAddressMode.NormalFixed:
				return DiagnosticsAddressingMode.NormalFixed;
			default:
				return DiagnosticsAddressingMode.Undefined;
			}
		}
	}
}
