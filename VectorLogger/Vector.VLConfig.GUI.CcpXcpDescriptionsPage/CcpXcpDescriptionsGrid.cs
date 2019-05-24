using DevExpress.Data;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Localization;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Vector.McModule;
using Vector.UtilityFunctions;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.BusinessLogic.Configuration.CcpXcp;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.GUI.Common;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;
using Vector.VLConfig.ValidationFramework;

namespace Vector.VLConfig.GUI.CcpXcpDescriptionsPage
{
	public class CcpXcpDescriptionsGrid : UserControl
	{
		private DatabaseConfiguration databaseConfiguration;

		private GUIElementManager_ControlGridTree guiElementManager;

		private CustomErrorProvider customErrorProvider;

		private PageValidator pageValidator;

		private ProtocolSettingsDialog protocolSettingsDialog;

		private IContainer components;

		private GridControl gridControlCcpXcpDescriptions;

		private GridView gridViewCcpXcpDescriptions;

		private GridColumn colDatabase;

		private GridColumn colProtcolType;

		private GridColumn colEcu;

		private GridColumn colChannel;

		private BindingSource ccpXcpDatabaseBindingSource;

		private RepositoryItemComboBox repositoryItemComboBox;

		private RepositoryItemButtonEdit repositoryItemButtonEdit;

		private ErrorProvider errorProviderFormat;

		private ErrorProvider errorProviderLocalModel;

		private ErrorProvider errorProviderGlobalModel;

		private GridColumn colIsCPEnabled;

		private RepositoryItemCheckEdit repositoryItemCheckEditIsEnabled;

		private RepositoryItemTextEdit repositoryItemTextEditDummy;

		private GridColumn colSettings;

		private RepositoryItemComboBox mRepositoryItemComboBoxECU;

		private XtraToolTipController mXtraToolTipController;

		public event EventHandler SelectionChanged;

		public DatabaseConfiguration DatabaseConfiguration
		{
			get
			{
				return this.databaseConfiguration;
			}
			set
			{
				this.databaseConfiguration = value;
				if (value != null)
				{
					int idx = this.StoreFocusedRow();
					this.ResetValidationFramework();
					this.gridControlCcpXcpDescriptions.DataSource = this.databaseConfiguration.CCPXCPDatabases;
					this.RestoreFocusedRow(idx);
				}
			}
		}

		public IModelEditor ModelEditor
		{
			get;
			set;
		}

		public IApplicationDatabaseManager ApplicationDatabaseManager
		{
			get;
			set;
		}

		public IConfigurationManagerService ConfigManager
		{
			get;
			set;
		}

		public ProjectRoot ProjectRoot
		{
			get
			{
				return this.ConfigManager.ProjectRoot;
			}
		}

		public IModelValidator ModelValidator
		{
			get;
			set;
		}

		public ISemanticChecker SemanticChecker
		{
			get;
			set;
		}

		public ILoggerSpecifics CurrentLogger
		{
			get;
			set;
		}

		public string ConfigurationFolderPath
		{
			get;
			set;
		}

		internal CcpXcpDescriptions CcpXcpDescriptionsPage
		{
			get;
			set;
		}

		public CcpXcpDescriptionsGrid()
		{
			this.InitializeComponent();
			this.guiElementManager = new GUIElementManager_ControlGridTree();
			this.customErrorProvider = new CustomErrorProvider(this.errorProviderLocalModel);
			this.customErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.FormatError, this.errorProviderFormat);
			this.customErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.GlobalModelError, this.errorProviderGlobalModel);
			this.pageValidator = new PageValidator(this.customErrorProvider);
		}

		private int StoreFocusedRow()
		{
			int focusedRowHandle = this.gridViewCcpXcpDescriptions.FocusedRowHandle;
			return this.gridViewCcpXcpDescriptions.GetDataSourceRowIndex(focusedRowHandle);
		}

		private void RestoreFocusedRow(int idx)
		{
			int rowHandle = this.gridViewCcpXcpDescriptions.GetRowHandle(idx);
			if (rowHandle >= 0 && rowHandle < this.gridViewCcpXcpDescriptions.RowCount)
			{
				this.gridViewCcpXcpDescriptions.FocusedRowHandle = rowHandle;
				this.gridViewCcpXcpDescriptions.SelectRow(rowHandle);
			}
		}

		private void Raise_SelectionChanged(object sender, EventArgs e)
		{
			if (this.SelectionChanged != null)
			{
				this.SelectionChanged(sender, e);
			}
		}

		public void Init()
		{
		}

		public void AddDatabase(string filepath)
		{
			Cursor.Current = Cursors.WaitCursor;
			List<uint> list = new List<uint>();
			bool flag = false;
			FileInfo fileInfo = new FileInfo(filepath);
			BusType busType = BusType.Bt_CAN;
			string value = string.Empty;
			if (fileInfo.Extension.ToLower().Contains("xml"))
			{
				if (this.DatabaseConfiguration.AllFlexrayDescriptionFiles.Count > 0)
				{
					Cursor.Current = Cursors.Default;
					InformMessageBox.Error(Resources_CcpXcp.ErrorUnableToAddFlexRayDbWithCP);
					return;
				}
				if ((from Database database in this.databaseConfiguration.ActiveCCPXCPDatabases
				where database.BusType.Value == BusType.Bt_FlexRay && database.IsBusDatabase
				select database).Any<Database>())
				{
					Cursor.Current = Cursors.Default;
					InformMessageBox.Error(Resources_CcpXcp.ErrorOnlyOneCpFlexRayDatabaseAllowed);
					return;
				}
				Cursor.Current = Cursors.Default;
				if (!this.SemanticChecker.IsFlexrayDbAddable(ref list))
				{
					return;
				}
				Cursor.Current = Cursors.WaitCursor;
				busType = BusType.Bt_FlexRay;
				IDictionary<string, BusType> dictionary;
				IDictionary<string, bool> source;
				if (this.ApplicationDatabaseManager.IsAutosarDescriptionFile(fileInfo.FullName, out dictionary) || !this.ApplicationDatabaseManager.IsFlexrayDatabase(fileInfo.FullName, out source))
				{
					Cursor.Current = Cursors.Default;
					InformMessageBox.Error(Resources.ErrorXMLIsNotFlexrayDb);
					return;
				}
				flag = source.First<KeyValuePair<string, bool>>().Value;
				if (flag)
				{
					if (list.Count < 2)
					{
						Cursor.Current = Cursors.Default;
						InformMessageBox.Error(Resources.ErrorFlexrayDBNeedsBothChannels);
						return;
					}
					Cursor.Current = Cursors.Default;
					if (DialogResult.No == InformMessageBox.Question(Resources.FlexrayDbNeedsBothChannels))
					{
						return;
					}
					Cursor.Current = Cursors.WaitCursor;
				}
				if (source.Any<KeyValuePair<string, bool>>())
				{
					value = source.First<KeyValuePair<string, bool>>().Key;
				}
			}
			Database database2 = new Database();
			database2.FilePath.Value = fileInfo.FullName;
			database2.NetworkName.Value = value;
			database2.BusType.Value = busType;
			if (BusType.Bt_FlexRay == busType)
			{
				if (flag)
				{
					database2.ChannelNumber.Value = Database.ChannelNumber_FlexrayAB;
				}
				else
				{
					database2.ChannelNumber.Value = list[0];
				}
			}
			this.AddAndConfigureCCPXCPDatabase(database2, busType);
		}

		public void RemoveDatabase(Database db)
		{
			this.databaseConfiguration.RemoveDatabase(db);
			this.ValidateInput(true);
		}

		public void ReplaceDatabase(string filepath, Database dbToReplace)
		{
			bool flag = false;
			if (dbToReplace.FileType == DatabaseFileType.A2L)
			{
				flag |= CcpXcpManager.Instance().ReplaceDatabase(filepath, dbToReplace);
			}
			else if (dbToReplace.FileType == DatabaseFileType.DBC)
			{
				flag |= this.ModelEditor.ReplaceDatabase(filepath, dbToReplace);
			}
			this.ValidateInput(flag);
		}

		private bool AddA2lDatabase(Database db, ref CPProtection protectionNeedsSKB)
		{
			if (db.CcpXcpEcuList.Count == 0)
			{
				db.AddCcpXcpEcu(new CcpXcpEcu());
			}
			foreach (Database current in this.databaseConfiguration.CCPXCPDatabases)
			{
				if (string.Compare(db.FilePath.Value, current.FilePath.Value, StringComparison.OrdinalIgnoreCase) == 0)
				{
					InformMessageBox.Error(Resources_CcpXcp.ErrorCcpXcpDatabaseAlreadyAdded);
					bool result = false;
					return result;
				}
			}
			Cursor.Current = Cursors.WaitCursor;
			db.BusType.Value = BusType.Bt_Wildcard;
			Vector.McModule.Result result2 = CcpXcpManager.Instance().LoadA2LDatabase(db);
			A2LDatabase a2LDatabase = CcpXcpManager.Instance().GetA2LDatabase(db);
			Cursor.Current = Cursors.Default;
			if (result2 != Vector.McModule.Result.kOk || a2LDatabase == null)
			{
				InformMessageBox.Error(Resources.ErrorCannotReadDatabase);
				return false;
			}
			IList<IDeviceInfo> list = a2LDatabase.CreateDeviceInfoList();
			if (this.ModelValidator.LoggerSpecifics.Recording.HasEthernet)
			{
				IDeviceInfo deviceInfo;
				do
				{
					deviceInfo = list.FirstOrDefault((IDeviceInfo d) => d.TransportType == EnumXcpTransportType.kTcp);
					if (deviceInfo == null)
					{
						break;
					}
				}
				while (list.Remove(deviceInfo));
			}
			else if (list.Count > 0)
			{
				IDeviceInfo deviceInfo2;
				do
				{
					deviceInfo2 = list.FirstOrDefault((IDeviceInfo d) => d.TransportType == EnumXcpTransportType.kUdp || d.TransportType == EnumXcpTransportType.kTcp);
				}
				while (deviceInfo2 != null && list.Remove(deviceInfo2));
				if (list.Count == 0)
				{
					CcpXcpManager.Instance().ReleaseA2LDatabase(db);
					InformMessageBox.Error(Resources.ErrorNoEthernetA2lAllowed);
					return false;
				}
			}
			bool flag = this.ModelValidator.LoggerSpecifics.Recording.HasEthernet;
			if (list.Count > 0)
			{
				if (list.All((IDeviceInfo d) => d.ProtocolType == EnumProtocolType.ProtocolCcp))
				{
					flag = false;
				}
			}
			bool flag2 = list.Any<IDeviceInfo>();
			if (!flag2)
			{
				IDeviceInfo deviceInfo3 = a2LDatabase.McDatabase.CreateEmptyDeviceInfo();
				deviceInfo3.ProtocolType = EnumProtocolType.ProtocolXcp;
				deviceInfo3.TransportType = EnumXcpTransportType.kCan;
				list.Add(deviceInfo3);
				if (this.ModelValidator.LoggerSpecifics.Recording.HasEthernet)
				{
					deviceInfo3 = a2LDatabase.McDatabase.CreateEmptyDeviceInfo();
					deviceInfo3.ProtocolType = EnumProtocolType.ProtocolXcp;
					deviceInfo3.TransportType = EnumXcpTransportType.kUdp;
					list.Add(deviceInfo3);
				}
			}
			if (!flag && list.Count == 1)
			{
				this.SetCcpXcpProtocol(db, list[0], false);
			}
			else
			{
				IDeviceInfo deviceInfo4 = null;
				if (flag)
				{
					deviceInfo4 = A2LDatabase.CreateDeviceInfoDefaultForVxModule(a2LDatabase.McDatabase);
					deviceInfo4.TransportLayerInstanceName = CcpXcpEcu.DefaultTransportLayerInstanceNameForVx();
				}
				ProtocolSelectionDialog protocolSelectionDialog = new ProtocolSelectionDialog(list, deviceInfo4, flag2);
				if (DialogResult.OK != protocolSelectionDialog.ShowDialog(this))
				{
					CcpXcpManager.Instance().ReleaseA2LDatabase(db);
					return false;
				}
				this.SetCcpXcpProtocol(db, protocolSelectionDialog.SelectedDeviceInfo, protocolSelectionDialog.UseVxModule);
			}
			db.CcpXcpEcuDisplayName.Value = this.FindA2lEcuName(db);
			db.AddCpProtection(new CPProtection(db.CcpXcpEcuDisplayName.Value));
			if (db.BusType.Value == BusType.Bt_FlexRay && this.ModelValidator.LoggerSpecifics.Flexray.NumberOfChannels < 1u)
			{
				CcpXcpManager.Instance().ReleaseA2LDatabase(db);
				InformMessageBox.Error(Resources.FlexRayNotSupportedByLoggerType);
				return false;
			}
			string str;
			if (!a2LDatabase.CreateDeviceConfig(db, out str, false))
			{
				CcpXcpManager.Instance().ReleaseA2LDatabase(db);
				InformMessageBox.Error(Resources.ErrorCannotReadDatabase + "\n" + str);
				return false;
			}
			ICcpDeviceConfig ccpDeviceConfig = a2LDatabase.DeviceConfig as ICcpDeviceConfig;
			if (ccpDeviceConfig != null && ccpDeviceConfig.MajorVersion < 2u)
			{
				InformMessageBox.Error(string.Format(Resources_CcpXcp.ErrorCcpVersionTooOld, "2.0"));
				CcpXcpManager.Instance().ReleaseA2LDatabase(db);
				return false;
			}
			if (db.BusType.Value == BusType.Bt_CAN)
			{
				db.ChannelNumber.Value = this.ModelValidator.GetFirstActiveOrDefaultChannel(BusType.Bt_CAN);
			}
			else if (db.BusType.Value == BusType.Bt_FlexRay)
			{
				db.ChannelNumber.Value = Database.ChannelNumber_FlexrayAB;
			}
			if (db.CpProtections.Count > 0 && db.CpProtections[0].HasSeedAndKey.Value)
			{
				protectionNeedsSKB = db.CpProtections[0];
			}
			foreach (CcpXcpSignal current2 in from sig in this.ConfigManager.CcpXcpSignalConfiguration.Signals
			where sig.EcuName.Value == db.CcpXcpEcuDisplayName.Value
			select sig)
			{
				ISignal signal = a2LDatabase.GetSignal(current2.Name.Value);
				if (signal != null)
				{
					current2.DaqEvents = CcpXcpManager.Instance().CreateDaqEventList(signal, db);
				}
			}
			return true;
		}

		private void SetCcpXcpProtocol(Database database, IDeviceInfo deviceInfo, bool useVxModule)
		{
			database.CPType.Value = CPType.XCP;
			database.CcpXcpEcuList[0].TransportLayerInstanceName = deviceInfo.TransportLayerInstanceName;
			database.CcpXcpEcuList[0].UseVxModule = useVxModule;
			if (useVxModule)
			{
				database.BusType.Value = BusType.Bt_Ethernet;
				database.CcpXcpEcuList[0].EthernetProtocol = EthernetProtocol.UDP;
				return;
			}
			if (deviceInfo.ProtocolType != EnumProtocolType.ProtocolXcp)
			{
				if (deviceInfo.ProtocolType == EnumProtocolType.ProtocolCcp)
				{
					database.BusType.Value = BusType.Bt_CAN;
					database.CPType.Value = CPType.CCP;
				}
				return;
			}
			switch (deviceInfo.TransportType)
			{
			case EnumXcpTransportType.kCan:
				database.BusType.Value = BusType.Bt_CAN;
				return;
			case EnumXcpTransportType.kTcp:
				database.BusType.Value = BusType.Bt_Ethernet;
				database.CcpXcpEcuList[0].EthernetProtocol = EthernetProtocol.TCP;
				return;
			case EnumXcpTransportType.kUdp:
				database.BusType.Value = BusType.Bt_Ethernet;
				database.CcpXcpEcuList[0].EthernetProtocol = EthernetProtocol.UDP;
				return;
			case EnumXcpTransportType.kFlexRay:
				database.BusType.Value = BusType.Bt_FlexRay;
				return;
			default:
				return;
			}
		}

		private string FindA2lEcuName(Database db)
		{
			string ecuName = string.Empty;
			if (db.BusType.Value == BusType.Bt_FlexRay)
			{
				List<string> list = new List<string>();
				foreach (Database current in this.databaseConfiguration.FlexrayFibexDatabases)
				{
					list.AddRange(this.ModelValidator.DatabaseServices.GetXcpSlaveNamesOfFlexRayDatabase(current, this.ConfigurationFolderPath));
				}
				if (list.Count > 0)
				{
					ecuName = list[0];
				}
			}
			else
			{
				ecuName = Resources.EcuNamePrefeix;
				string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(db.FilePath.Value);
				if (fileNameWithoutExtension != null && !string.IsNullOrEmpty(fileNameWithoutExtension.Trim()))
				{
					ecuName = fileNameWithoutExtension;
					Regex regex = new Regex("[^a-zA-Z0-9_]");
					ecuName = regex.Replace(ecuName, "_");
				}
				if (ecuName != null)
				{
					if ((from Database database in this.databaseConfiguration.CCPXCPDatabases
					where string.Compare(database.CcpXcpEcuDisplayName.Value, ecuName, StringComparison.InvariantCultureIgnoreCase) == 0
					select db).Any<Database>())
					{
						int num = 1;
						foreach (Database current2 in this.databaseConfiguration.ActiveCCPXCPDatabases)
						{
							string value = current2.CcpXcpEcuDisplayName.Value;
							if (value.Length >= ecuName.Length && string.Compare(ecuName, value.Substring(0, ecuName.Length), StringComparison.InvariantCultureIgnoreCase) == 0)
							{
								string value2 = value.Substring(ecuName.Length, value.Length - ecuName.Length);
								if (string.IsNullOrEmpty(value2))
								{
									num++;
								}
								else
								{
									try
									{
										int num2 = Convert.ToInt32(value2);
										if (num2 >= num)
										{
											num = Math.Abs(num2 + 1);
										}
									}
									catch (Exception)
									{
									}
								}
							}
						}
						if (num > 1)
						{
							ecuName += num;
						}
					}
				}
			}
			return ecuName;
		}

		public void AddAndConfigureCCPXCPDatabase(Database db, BusType busType)
		{
			db.FilePath.Value = this.ModelValidator.GetFilePathRelativeToConfiguration(db.FilePath.Value);
			db.CPType.Value = CPType.None;
			db.IsCPActive.Value = true;
			CPProtection cPProtection = null;
			if (db.FileType == DatabaseFileType.A2L)
			{
				if (!this.AddA2lDatabase(db, ref cPProtection))
				{
					return;
				}
			}
			else
			{
				uint extraCPChannel = 0u;
				if (BusType.Bt_CAN == busType)
				{
					db.ChannelNumber.Value = this.ModelValidator.GetFirstActiveOrDefaultChannel(busType);
				}
				IDictionary<string, bool> dictionary;
				db.CPType.Value = this.ModelValidator.DatabaseServices.GetDatabaseCPConfiguration(db, out dictionary, out extraCPChannel);
				if (db.CPType.Value == CPType.None)
				{
					Cursor.Current = Cursors.Default;
					if (DialogResult.No == InformMessageBox.Question(string.Format(Resources_CcpXcp.DbContainsNoCCPXCPAddAnyway, db.FilePath.Value)))
					{
						return;
					}
					db.IsCPActive.Value = false;
					Cursor.Current = Cursors.WaitCursor;
				}
				else
				{
					db.CcpXcpEcuDisplayName.Value = Database.MakeCpEcuDisplayName(dictionary.Keys);
					if (db.CcpXcpEcuDisplayName.Value != Resources_CcpXcp.CcpXcpDefaultEcuName)
					{
						foreach (Database current in this.databaseConfiguration.CCPXCPDatabases)
						{
							if (db.CcpXcpEcuDisplayName == current.CcpXcpEcuDisplayName && current.FileType != DatabaseFileType.A2L && current.IsCPActive.Value)
							{
								InformMessageBox.Error(Resources.ErrorDuplicateEcuName);
								return;
							}
						}
					}
					if (string.IsNullOrEmpty(db.CcpXcpEcuDisplayName.Value))
					{
						db.CcpXcpEcuDisplayName.Value = Resources_CcpXcp.CcpXcpDefaultEcuName;
					}
					if (BusType.Bt_FlexRay == busType)
					{
						db.ExtraCPChannel = extraCPChannel;
						db.ChannelNumber.Value = Database.ChannelNumber_FlexrayAB;
						int num = 0;
						using (IEnumerator<string> enumerator2 = dictionary.Keys.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								string current2 = enumerator2.Current;
								CPProtection cPProtection2 = new CPProtection(current2, dictionary[current2]);
								db.AddCpProtection(cPProtection2);
								if (cPProtection2.HasSeedAndKey.Value)
								{
									num++;
									cPProtection = cPProtection2;
								}
								if (num > 1)
								{
									Cursor.Current = Cursors.Default;
									InformMessageBox.Error(Resources_CcpXcp.ErrorMultipleSKBFilesReq);
									return;
								}
							}
							goto IL_29C;
						}
					}
					CPProtection cPProtection3 = new CPProtection("", dictionary.First<KeyValuePair<string, bool>>().Value);
					db.AddCpProtection(cPProtection3);
					if (cPProtection3.HasSeedAndKey.Value)
					{
						cPProtection = cPProtection3;
					}
				}
			}
			IL_29C:
			if (cPProtection != null)
			{
				Cursor.Current = Cursors.Default;
				if (DialogResult.Yes == InformMessageBox.Question(string.Format(Resources_CcpXcp.DbRequiresSeedAndKeyAddNow, db.FilePath.Value)))
				{
					GenericOpenFileDialog.FileName = "";
					string titleText = string.Format(Resources.TitleSelectSKBFileForDb, Path.GetFileName(db.FilePath.Value));
					if (DialogResult.OK == GenericOpenFileDialog.ShowDialog(titleText, FileType.SKBFile))
					{
						cPProtection.SeedAndKeyFilePath.Value = GenericOpenFileDialog.FileName;
						cPProtection.SeedAndKeyFilePath.Value = this.ModelValidator.GetFilePathRelativeToConfiguration(GenericOpenFileDialog.FileName);
					}
				}
				Cursor.Current = Cursors.WaitCursor;
			}
			else
			{
				db.CcpXcpIsSeedAndKeyUsed = false;
			}
			if (db.CpProtectionsWithSeedAndKeyRequired.Count > 0)
			{
				db.CcpXcpIsSeedAndKeyUsed = true;
			}
			this.databaseConfiguration.AddDatabase(db);
			this.gridViewCcpXcpDescriptions.RefreshData();
			this.ValidateInput(true);
			this.SelectRowOfDatabase(db);
			Cursor.Current = Cursors.Default;
		}

		private void UpdateButtons()
		{
			Database database;
			bool flag = this.TryGetSelectedDatabase(out database);
			if (flag)
			{
				bool active = database.IsCPActive.Value;
				if (!this.ModelValidator.ValidateDatabaseConsistency(database, this.pageValidator) || database.IsFileNotFound)
				{
					active = false;
				}
				this.CcpXcpDescriptionsPage.UpdateSettingsButtonState(active);
			}
			this.CcpXcpDescriptionsPage.UpdateReplaceButtonState(flag && (database.FileType == DatabaseFileType.A2L || database.FileType == DatabaseFileType.DBC));
		}

		private void gridViewCCPXCPDatabases_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
		{
			Database database;
			if (e.Column == this.colDatabase && this.GetDatabase(this.gridViewCcpXcpDescriptions.GetDataSourceRowIndex(e.RowHandle), out database))
			{
				if (database.IsFileNotFound || !this.ModelValidator.ValidateDatabaseConsistency(database, this.pageValidator))
				{
					IValidatedGUIElement gUIElement = this.guiElementManager.GetGUIElement(e.Column, this.gridViewCcpXcpDescriptions.GetDataSourceRowIndex(e.RowHandle));
					this.customErrorProvider.Grid.DisplayError(gUIElement, e);
					return;
				}
				if (database.FileType == DatabaseFileType.A2L)
				{
					GridUtil.DrawImageTextCell(e, Resources.IconDatabaseA2L.ToBitmap());
					return;
				}
				if (database.FileType == DatabaseFileType.DBC)
				{
					GridUtil.DrawImageTextCell(e, Resources.IconDatabaseCANDBC.ToBitmap());
					return;
				}
				if (database.FileType == DatabaseFileType.XML)
				{
					GridUtil.DrawImageTextCell(e, Resources.IconDatabaseXML.ToBitmap());
					return;
				}
			}
			else
			{
				IValidatedGUIElement gUIElement2 = this.guiElementManager.GetGUIElement(e.Column, this.gridViewCcpXcpDescriptions.GetDataSourceRowIndex(e.RowHandle));
				this.customErrorProvider.Grid.DisplayError(gUIElement2, e);
			}
		}

		private void gridViewCCPXCPDatabases_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
		{
			if (e.MenuType == GridMenuType.Column)
			{
				string localizedString = GridLocalizer.Active.GetLocalizedString(GridStringId.MenuColumnGroupBox);
				string localizedString2 = GridLocalizer.Active.GetLocalizedString(GridStringId.MenuColumnFilter);
				string localizedString3 = GridLocalizer.Active.GetLocalizedString(GridStringId.MenuColumnFilterEditor);
				string localizedString4 = GridLocalizer.Active.GetLocalizedString(GridStringId.MenuColumnGroup);
				string localizedString5 = GridLocalizer.Active.GetLocalizedString(GridStringId.MenuColumnUnGroup);
				string localizedString6 = GridLocalizer.Active.GetLocalizedString(GridStringId.MenuColumnRemoveColumn);
				string localizedString7 = GridLocalizer.Active.GetLocalizedString(GridStringId.MenuGroupPanelShow);
				for (int i = e.Menu.Items.Count - 1; i >= 0; i--)
				{
					string caption = e.Menu.Items[i].Caption;
					if (localizedString4 == caption || localizedString5 == caption || caption.Contains(localizedString) || localizedString2 == caption || localizedString3 == caption || localizedString6 == caption || localizedString7 == caption)
					{
						e.Menu.Items.RemoveAt(i);
					}
				}
			}
		}

		private void gridViewCCPXCPDatabases_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
		{
			Database db;
			if (!this.GetDatabase(e.ListSourceRowIndex, out db))
			{
				return;
			}
			if (e.Column == this.colIsCPEnabled)
			{
				this.UnboundColumnIsCCPXCPEnabled(db, e);
				return;
			}
			if (e.Column == this.colDatabase)
			{
				this.UnboundColumnFileName(db, e);
				return;
			}
			if (e.Column == this.colProtcolType)
			{
				this.UnboundColumnProtocolType(db, e);
				return;
			}
			if (e.Column == this.colChannel)
			{
				this.UnboundColumnChannel(db, e);
				return;
			}
			if (e.Column == this.colEcu)
			{
				this.UnboundColumnEcuName(db, e);
				return;
			}
			if (e.Column == this.colSettings)
			{
				this.UnboundColumnSettings(db, e);
			}
		}

		private void gridViewCCPXCPDatabases_ShowingEditor(object sender, CancelEventArgs e)
		{
			Database database;
			if (this.gridViewCcpXcpDescriptions.FocusedColumn == this.colEcu && this.TryGetSelectedDatabase(out database) && (database.FileType != DatabaseFileType.A2L || !database.IsCPActive.Value))
			{
				e.Cancel = true;
			}
			Database database2;
			if ((this.gridViewCcpXcpDescriptions.FocusedColumn == this.colSettings || this.gridViewCcpXcpDescriptions.FocusedColumn == this.colChannel) && this.TryGetSelectedDatabase(out database2) && !database2.IsCPActive.Value)
			{
				e.Cancel = true;
			}
			Database database3;
			if (this.gridViewCcpXcpDescriptions.FocusedColumn == this.colChannel && this.TryGetSelectedDatabase(out database3) && database3.BusType.Value == BusType.Bt_Ethernet)
			{
				e.Cancel = true;
			}
		}

		private void gridViewCcpXcpDescriptions_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
		{
			Database database3;
			if (e.Column == this.colSettings)
			{
				this.repositoryItemButtonEdit.ReadOnly = false;
				this.repositoryItemButtonEdit.Buttons[0].Enabled = true;
				this.repositoryItemComboBox.ReadOnly = false;
				Database database;
				if (this.TryGetSelectedDatabase(out database))
				{
					if (!database.IsCPActive.Value)
					{
						this.repositoryItemButtonEdit.ReadOnly = true;
						this.repositoryItemButtonEdit.Buttons[0].Enabled = false;
						this.repositoryItemComboBox.ReadOnly = true;
						return;
					}
					if (!this.ModelValidator.ValidateDatabaseConsistency(database, this.pageValidator) || database.IsFileNotFound)
					{
						this.repositoryItemButtonEdit.ReadOnly = true;
						this.repositoryItemButtonEdit.Buttons[0].Enabled = false;
						return;
					}
				}
			}
			else if (e.Column == this.colEcu)
			{
				Database database2;
				if (this.TryGetSelectedDatabase(out database2))
				{
					if (database2.BusType.Value == BusType.Bt_FlexRay && database2.FileType == DatabaseFileType.A2L)
					{
						this.PopulateECUListComboBox();
						e.RepositoryItem = this.mRepositoryItemComboBoxECU;
						return;
					}
					e.RepositoryItem = this.repositoryItemTextEditDummy;
					return;
				}
			}
			else if (e.Column == this.colChannel && this.TryGetSelectedDatabase(out database3) && database3.BusType.Value == BusType.Bt_Ethernet && database3.FileType == DatabaseFileType.A2L)
			{
				e.RepositoryItem = this.repositoryItemTextEditDummy;
			}
		}

		private void gridViewCCPXCPDatabases_ShownEditor(object sender, EventArgs e)
		{
			if (this.gridViewCcpXcpDescriptions.FocusedColumn == this.colChannel)
			{
				this.ShownEditorChannel();
			}
			GridView gridView = sender as GridView;
			if (gridView != null && gridView.ActiveEditor is CheckEdit)
			{
				((CheckEdit)gridView.ActiveEditor).Toggle();
			}
		}

		private void gridViewCCPXCPDatabases_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
		{
			this.Raise_SelectionChanged(this, EventArgs.Empty);
			this.UpdateButtons();
		}

		private void gridViewCcpXcpDescriptions_RowCellStyle(object sender, RowCellStyleEventArgs e)
		{
			try
			{
				if (!Convert.ToBoolean(this.gridViewCcpXcpDescriptions.GetRowCellValue(e.RowHandle, this.colIsCPEnabled)))
				{
					e.Appearance.ForeColor = SystemColors.GrayText;
				}
				else
				{
					e.Appearance.ForeColor = SystemColors.ControlText;
				}
			}
			catch (Exception)
			{
			}
			if (e.Appearance.BackColor == SystemColors.Highlight || e.Appearance.BackColor == SystemColors.MenuHighlight)
			{
				e.Appearance.ForeColor = SystemColors.HighlightText;
			}
		}

		private void gridViewCCPXCPDatabases_KeyDown(object sender, KeyEventArgs e)
		{
			Database db;
			if (this.TryGetSelectedDatabase(out db))
			{
				if (e.KeyCode == Keys.Delete)
				{
					this.RemoveDatabase(db);
				}
				if (e.KeyCode == Keys.Space)
				{
					try
					{
						bool flag = Convert.ToBoolean(this.gridViewCcpXcpDescriptions.GetRowCellValue(this.gridViewCcpXcpDescriptions.FocusedRowHandle, this.colIsCPEnabled));
						this.gridViewCcpXcpDescriptions.SetRowCellValue(this.gridViewCcpXcpDescriptions.FocusedRowHandle, this.colIsCPEnabled, !flag);
					}
					catch (Exception)
					{
					}
				}
			}
		}

		private void gridViewCCPXCPDatabases_DoubleClick(object sender, EventArgs e)
		{
			Database database;
			if (this.gridViewCcpXcpDescriptions.FocusedColumn == this.colDatabase && this.TryGetSelectedDatabase(out database))
			{
				FileSystemServices.LaunchFile(this.ModelValidator.GetAbsoluteFilePath(database.FilePath.Value));
			}
		}

		private void gridViewCcpXcpDescriptions_TopRowChanged(object sender, EventArgs e)
		{
			this.DisplayErrors();
		}

		private void gridViewCcpXcpDescriptions_LeftCoordChanged(object sender, EventArgs e)
		{
			this.DisplayErrors();
		}

		private void CcpXcpDescriptionsGrid_Resize(object sender, EventArgs e)
		{
			this.DisplayErrors();
		}

		private void repositoryItemComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.gridViewCcpXcpDescriptions.PostEditor();
		}

		private void mRepositoryItemComboBoxECU_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.gridViewCcpXcpDescriptions.PostEditor();
		}

		private void repositoryItemButtonEdit_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			this.ShowProtocolSettingsDialog();
		}

		private void repositoryItemCheckEditIsEnabled_CheckedChanged(object sender, EventArgs e)
		{
			this.gridViewCcpXcpDescriptions.PostEditor();
		}

		private void mRepositoryItemComboBoxECU_QueryPopUp(object sender, CancelEventArgs e)
		{
			Database database;
			if (this.TryGetSelectedDatabase(out database) && database.BusType.Value == BusType.Bt_FlexRay && database.FileType == DatabaseFileType.A2L)
			{
				bool flag = false;
				foreach (Database current in this.databaseConfiguration.FlexrayFibexDatabases)
				{
					if (this.ModelValidator.DatabaseServices.GetXcpSlaveNamesOfFlexRayDatabase(current, this.ConfigurationFolderPath).Any<string>())
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					InformMessageBox.Error(Resources_CcpXcp.ErrorCcpXcpNoFibexWithEcuFound);
				}
			}
		}

		private void UnboundColumnIsCCPXCPEnabled(Database db, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				e.Value = db.IsCPActive.Value;
				return;
			}
			bool flag = false;
			this.pageValidator.Grid.UpdateModel<bool>((bool)e.Value, db.IsCPActive, out flag);
			if (flag)
			{
				this.UpdateButtons();
				this.ValidateInput(true);
			}
		}

		private void UnboundColumnFileName(Database db, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				e.Value = db.FilePath.Value;
			}
		}

		private void UnboundColumnProtocolType(Database db, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				e.Value = GUIUtil.GetShortCPTypeName(db.CPType.Value);
				if (db.CPType.Value == CPType.XCP && db.CcpXcpEcuList.Any<CcpXcpEcu>())
				{
					if (db.CcpXcpEcuList[0].UseVxModule)
					{
						e.Value += " (VX)";
					}
					string transportLayerInstanceName = db.CcpXcpEcuList[0].TransportLayerInstanceName;
					if (!string.IsNullOrEmpty(transportLayerInstanceName) && !CcpXcpEcu.IsDefaultTransportLayerInstanceForVx(transportLayerInstanceName))
					{
						object value = e.Value;
						e.Value = string.Concat(new object[]
						{
							value,
							" (",
							transportLayerInstanceName,
							")"
						});
					}
				}
			}
		}

		private void UnboundColumnChannel(Database db, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				if (db.BusType.Value == BusType.Bt_CAN)
				{
					e.Value = GUIUtil.MapCANChannelNumber2String(db.ChannelNumber.Value);
					return;
				}
				if (db.BusType.Value == BusType.Bt_LIN)
				{
					e.Value = GUIUtil.MapLINChannelNumber2String(db.ChannelNumber.Value, this.ModelValidator.LoggerSpecifics);
					return;
				}
				if (db.BusType.Value == BusType.Bt_FlexRay)
				{
					e.Value = GUIUtil.MapFlexrayChannelNumber2String(db.ChannelNumber.Value);
					return;
				}
				if (db.BusType.Value == BusType.Bt_Ethernet)
				{
					e.Value = GUIUtil.MapEthernetChannelNumber2String(db.ChannelNumber.Value);
					return;
				}
			}
			else
			{
				if (db.CcpXcpEcuList.Any<CcpXcpEcu>() && this.pageValidator.General.HasError(db.CcpXcpEcuList[0].CcpXcpEcuDisplayNameValidatedProperty))
				{
					InformMessageBox.Error(Resources.ErrorCorrectAllErrorsBeforeChangeOfChannelAllowed);
					return;
				}
				uint newChannelNumber;
				if (db.BusType.Value == BusType.Bt_CAN)
				{
					newChannelNumber = GUIUtil.MapCANChannelString2Number(e.Value.ToString());
				}
				else if (db.BusType.Value == BusType.Bt_LIN)
				{
					newChannelNumber = GUIUtil.MapLINChannelString2Number(e.Value.ToString());
				}
				else
				{
					if (db.BusType.Value != BusType.Bt_FlexRay)
					{
						return;
					}
					newChannelNumber = GUIUtil.MapFlexrayChannelString2Number(e.Value.ToString());
				}
				if (this.ModelEditor.CheckAndProcessDatabaseChannelRemapping(db, newChannelNumber))
				{
					this.ValidateInput(true);
				}
			}
		}

		private void UnboundColumnEcuName(Database db, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				e.Value = db.CcpXcpEcuDisplayName.Value;
			}
			if (e.IsSetData)
			{
				string ecuName = e.Value.ToString();
				if (!string.IsNullOrEmpty(ecuName) && !string.IsNullOrWhiteSpace(ecuName))
				{
					string oldEcuName = db.CcpXcpEcuDisplayName.Value;
					bool flag = (from Database database in this.DatabaseConfiguration.ActiveCCPXCPDatabases
					where database.CcpXcpEcuDisplayName.Value == oldEcuName
					select database).Count<Database>() > 1;
					bool flag2;
					this.pageValidator.Grid.UpdateModel<string>(ecuName, db.CcpXcpEcuDisplayName, out flag2);
					if (flag2)
					{
						foreach (CPProtection current in db.CpProtections)
						{
							current.ECUName.Value = ecuName;
						}
						if ((from Database database in this.DatabaseConfiguration.ActiveCCPXCPDatabases
						where database.CcpXcpEcuDisplayName.Value == ecuName
						select database).Count<Database>() < 2 && !flag)
						{
							this.ProclaimEcuRenaming(oldEcuName, ecuName);
						}
						this.ValidateInput(true);
					}
				}
			}
		}

		private void UnboundColumnSettings(Database db, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				e.Value = this.GetSettingsString(db);
			}
		}

		private void ShownEditorChannel()
		{
			ComboBoxEdit comboBoxEdit = this.gridViewCcpXcpDescriptions.ActiveEditor as ComboBoxEdit;
			if (comboBoxEdit != null)
			{
				ReadOnlyCollection<Database> readOnlyCollection = this.gridControlCcpXcpDescriptions.DataSource as ReadOnlyCollection<Database>;
				if (readOnlyCollection == null)
				{
					return;
				}
				Database database = readOnlyCollection[this.gridViewCcpXcpDescriptions.GetFocusedDataSourceRowIndex()];
				if (database.BusType.Value == BusType.Bt_CAN)
				{
					this.FillCANChannelCombobox(comboBoxEdit);
					return;
				}
				if (database.BusType.Value == BusType.Bt_LIN)
				{
					this.FillLINChannelCombobox(comboBoxEdit);
					return;
				}
				if (database.BusType.Value == BusType.Bt_FlexRay)
				{
					if (database.ChannelNumber.Value != Database.ChannelNumber_FlexrayAB)
					{
						this.FillFlexrayChannelCombobox(comboBoxEdit);
						return;
					}
					comboBoxEdit.Properties.Items.Add(Vocabulary.FlexrayChannelAB);
				}
			}
		}

		public void DisplayErrors()
		{
			this.StoreMapping4VisibleCells();
			this.pageValidator.General.ActivateErrorProvidersForFormatAndModelErrors();
		}

		public bool ValidateInput(bool isDataChanged = false)
		{
			this.ResetValidationFramework();
			this.StoreMapping4VisibleCells();
			bool flag = true;
			flag &= this.ModelValidator.Validate(this.DatabaseConfiguration, PageType.CcpXcpDescriptions, isDataChanged, this.pageValidator);
			this.pageValidator.General.ActivateErrorProvidersForFormatAndModelErrors();
			return flag;
		}

		public void Reset()
		{
			this.ResetValidationFramework();
		}

		public bool HasErrors()
		{
			return this.pageValidator.General.HasErrors(Enum.GetValues(typeof(ValidationErrorClass)) as ValidationErrorClass[]);
		}

		public bool HasGlobalErrors()
		{
			return this.pageValidator.General.HasErrors(new ValidationErrorClass[]
			{
				ValidationErrorClass.GlobalModelError
			});
		}

		public bool HasLocalErrors()
		{
			return this.pageValidator.General.HasErrors(new ValidationErrorClass[]
			{
				ValidationErrorClass.FormatError,
				ValidationErrorClass.LocalModelError
			});
		}

		public bool HasFormatErrors()
		{
			IPageValidatorGeneral arg_13_0 = this.pageValidator.General;
			ValidationErrorClass[] errorClasses = new ValidationErrorClass[1];
			return arg_13_0.HasErrors(errorClasses);
		}

		private void ResetValidationFramework()
		{
			this.pageValidator.General.Reset();
			this.guiElementManager.Reset();
		}

		private void StoreMapping4VisibleCells()
		{
			PageValidatorGridUtil.StoreRowMappingForVisibleRows(this.gridViewCcpXcpDescriptions, new PageValidatorGridUtil.StoreRowMappingHandler(this.OnPageValidatorGridUtilStoreRowMapping));
		}

		private void OnPageValidatorGridUtilStoreRowMapping(int dataSourceRowIdx)
		{
			if (dataSourceRowIdx < 0 || dataSourceRowIdx >= this.databaseConfiguration.CCPXCPDatabases.Count)
			{
				return;
			}
			Database database = this.DatabaseConfiguration.CCPXCPDatabases[dataSourceRowIdx];
			this.StoreMapping4VisibleColumns(database, dataSourceRowIdx);
		}

		private void StoreMapping4VisibleColumns(Database database, int dataSourceIdx)
		{
			if (PageValidatorGridUtil.IsColumnVisible(this.colIsCPEnabled, this.gridViewCcpXcpDescriptions))
			{
				IValidatedGUIElement gUIElement = this.guiElementManager.GetGUIElement(this.colIsCPEnabled, dataSourceIdx);
				this.pageValidator.Grid.StoreMapping(database.IsCPActive, gUIElement);
			}
			if (PageValidatorGridUtil.IsColumnVisible(this.colChannel, this.gridViewCcpXcpDescriptions))
			{
				IValidatedGUIElement gUIElement = this.guiElementManager.GetGUIElement(this.colChannel, dataSourceIdx);
				this.pageValidator.Grid.StoreMapping(database.ChannelNumber, gUIElement);
			}
			if (PageValidatorGridUtil.IsColumnVisible(this.colDatabase, this.gridViewCcpXcpDescriptions))
			{
				IValidatedGUIElement gUIElement = this.guiElementManager.GetGUIElement(this.colDatabase, dataSourceIdx);
				this.pageValidator.Grid.StoreMapping(database.FilePath, gUIElement);
			}
			if (PageValidatorGridUtil.IsColumnVisible(this.colEcu, this.gridViewCcpXcpDescriptions))
			{
				IValidatedGUIElement gUIElement = this.guiElementManager.GetGUIElement(this.colEcu, dataSourceIdx);
				this.pageValidator.Grid.StoreMapping(database.CcpXcpEcuDisplayName, gUIElement);
			}
			if (PageValidatorGridUtil.IsColumnVisible(this.colSettings, this.gridViewCcpXcpDescriptions))
			{
				IValidatedGUIElement gUIElement = this.guiElementManager.GetGUIElement(this.colSettings, dataSourceIdx);
				this.pageValidator.Grid.StoreMapping(database.CcpXcpIsSeedAndKeyUsedValidatedProperty, gUIElement);
			}
		}

		private void FillCANChannelCombobox(ComboBoxEdit comboBoxEdit)
		{
			for (uint num = 1u; num <= this.ModelValidator.LoggerSpecifics.CAN.NumberOfChannels; num += 1u)
			{
				comboBoxEdit.Properties.Items.Add(GUIUtil.MapCANChannelNumber2String(num));
			}
		}

		private void FillLINChannelCombobox(ComboBoxEdit comboBoxEdit)
		{
			for (uint num = 1u; num <= this.ModelValidator.LoggerSpecifics.LIN.NumberOfChannels; num += 1u)
			{
				comboBoxEdit.Properties.Items.Add(GUIUtil.MapLINChannelNumber2String(num, this.ModelValidator.LoggerSpecifics));
			}
		}

		private void FillFlexrayChannelCombobox(ComboBoxEdit comboBoxEdit)
		{
			for (uint num = 1u; num <= this.ModelValidator.LoggerSpecifics.Flexray.NumberOfChannels; num += 1u)
			{
				comboBoxEdit.Properties.Items.Add(GUIUtil.MapFlexrayChannelNumber2String(num));
			}
		}

		public bool TryGetSelectedDatabase(out Database database)
		{
			int num;
			return this.TryGetSelectedDatabase(out database, out num);
		}

		private bool TryGetSelectedDatabase(out Database database, out int idx)
		{
			database = null;
			idx = this.gridViewCcpXcpDescriptions.GetFocusedDataSourceRowIndex();
			if (idx < 0 || idx > this.databaseConfiguration.CCPXCPDatabases.Count - 1)
			{
				return false;
			}
			database = this.databaseConfiguration.CCPXCPDatabases[idx];
			return null != database;
		}

		private bool GetDatabase(int listSourceRowIndex, out Database database)
		{
			database = null;
			if (listSourceRowIndex < 0 || listSourceRowIndex > this.databaseConfiguration.CCPXCPDatabases.Count - 1)
			{
				return false;
			}
			database = this.databaseConfiguration.CCPXCPDatabases[listSourceRowIndex];
			return null != database;
		}

		public void SelectRowOfDatabase(Database db)
		{
			for (int i = 0; i < this.gridViewCcpXcpDescriptions.RowCount; i++)
			{
				IList<Database> list = this.gridViewCcpXcpDescriptions.DataSource as IList<Database>;
				if (list != null)
				{
					Database database = list[this.gridViewCcpXcpDescriptions.GetDataSourceRowIndex(i)];
					if (database == db)
					{
						this.gridViewCcpXcpDescriptions.FocusedRowHandle = i;
						return;
					}
				}
			}
		}

		public bool Serialize(CcpXcpDescriptionsPage page)
		{
			if (page == null)
			{
				return false;
			}
			bool result = true;
			MemoryStream memoryStream = null;
			try
			{
				memoryStream = new MemoryStream();
				this.gridViewCcpXcpDescriptions.SaveLayoutToStream(memoryStream);
				memoryStream.Seek(0L, SeekOrigin.Begin);
				byte[] array = new byte[memoryStream.Length];
				memoryStream.Read(array, 0, (int)memoryStream.Length);
				page.CcpXcpDescriptionsPageGridLayout = Convert.ToBase64String(array, 0, array.Length);
			}
			catch
			{
				result = false;
			}
			finally
			{
				if (memoryStream != null)
				{
					memoryStream.Close();
				}
			}
			return result;
		}

		public bool DeSerialize(CcpXcpDescriptionsPage page)
		{
			if (page == null)
			{
				return false;
			}
			if (string.IsNullOrEmpty(page.CcpXcpDescriptionsPageGridLayout))
			{
				return false;
			}
			bool result = true;
			MemoryStream memoryStream = null;
			try
			{
				byte[] buffer = Convert.FromBase64String(page.CcpXcpDescriptionsPageGridLayout);
				memoryStream = new MemoryStream(buffer);
				this.gridViewCcpXcpDescriptions.RestoreLayoutFromStream(memoryStream);
			}
			catch
			{
				result = false;
			}
			finally
			{
				if (memoryStream != null)
				{
					memoryStream.Close();
				}
			}
			return result;
		}

		private void CCPXCPDatabaseGrid_DragEnter(object sender, DragEventArgs e)
		{
			IList<string> list;
			if (this.AcceptFileDrop(e, out list))
			{
				e.Effect = DragDropEffects.Copy;
				return;
			}
			e.Effect = DragDropEffects.None;
		}

		private void CCPXCPDatabaseGrid_DragDrop(object sender, DragEventArgs e)
		{
			IList<string> list;
			if (this.AcceptFileDrop(e, out list))
			{
				using (IEnumerator<string> enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						string current = enumerator.Current;
						this.AddDatabase(current);
					}
					return;
				}
			}
			e.Effect = DragDropEffects.None;
		}

		private bool AcceptFileDrop(DragEventArgs e, out IList<string> acceptedFiles)
		{
			acceptedFiles = null;
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				acceptedFiles = new List<string>();
				string[] array = (string[])e.Data.GetData(DataFormats.FileDrop);
				bool result = false;
				string[] array2 = array;
				int i = 0;
				while (i < array2.Length)
				{
					string text = array2[i];
					string extension;
					try
					{
						extension = Path.GetExtension(text);
					}
					catch
					{
						goto IL_B9;
					}
					goto IL_5D;
					IL_B9:
					i++;
					continue;
					IL_5D:
					if (string.Compare(extension, Vocabulary.FileExtensionDotDBC, StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(extension, Vocabulary.FileExtensionDotA2L, StringComparison.OrdinalIgnoreCase) == 0 || (string.Compare(extension, Vocabulary.FileExtensionDotXML, StringComparison.OrdinalIgnoreCase) == 0 && this.ModelValidator.LoggerSpecifics.Flexray.NumberOfChannels > 0u))
					{
						acceptedFiles.Add(text);
						result = true;
						goto IL_B9;
					}
					goto IL_B9;
				}
				return result;
			}
			return false;
		}

		private void PopulateECUListComboBox()
		{
			this.mRepositoryItemComboBoxECU.Items.Clear();
			List<string> list = new List<string>();
			foreach (Database current in this.databaseConfiguration.FlexrayFibexDatabases)
			{
				list.AddRange(this.ModelValidator.DatabaseServices.GetXcpSlaveNamesOfFlexRayDatabase(current, this.ConfigurationFolderPath));
			}
			IEnumerable<string> enumerable = list.Distinct<string>();
			foreach (string current2 in enumerable)
			{
				this.mRepositoryItemComboBoxECU.Items.Add(current2);
			}
		}

		public void ShowProtocolSettingsDialog()
		{
			Database database;
			if (this.TryGetSelectedDatabase(out database))
			{
				ProtocolSettingsDialog protocolSettingsDialog = this.GetProtocolSettingsDialog(database);
				if (DialogResult.OK == protocolSettingsDialog.ShowDialog())
				{
					this.ConfigManager.EthernetConfiguration.Eth1Ip = new ValidatedProperty<string>(protocolSettingsDialog.EthernetConfiguration.Eth1Ip.Value);
					this.ConfigManager.EthernetConfiguration.Eth1KeepAwake = new ValidatedProperty<bool>(protocolSettingsDialog.EthernetConfiguration.Eth1KeepAwake.Value);
					this.DatabaseConfiguration.EnableExchangeIdHandling = new ValidatedProperty<bool>(protocolSettingsDialog.EnableExchangeIdHandling.Value);
					this.DatabaseConfiguration.ReplaceDatabase(database, protocolSettingsDialog.Database);
					this.ValidateInput(true);
				}
			}
		}

		public ProtocolSettingsDialog GetProtocolSettingsDialog(Database db)
		{
			if (this.protocolSettingsDialog == null)
			{
				this.protocolSettingsDialog = new ProtocolSettingsDialog(this.ModelValidator);
			}
			this.protocolSettingsDialog.ConfigurationFolderPath = this.ConfigurationFolderPath;
			this.protocolSettingsDialog.Database = db;
			this.protocolSettingsDialog.EnableExchangeIdHandling = new ValidatedProperty<bool>(this.DatabaseConfiguration.EnableExchangeIdHandling.Value);
			this.protocolSettingsDialog.EthernetConfiguration = this.ConfigManager.EthernetConfiguration;
			return this.protocolSettingsDialog;
		}

		private string GetSettingsString(Database db)
		{
			List<string> list = new List<string>();
			string text = string.Empty;
			if (!db.CcpXcpEcuList.Any<CcpXcpEcu>() || !db.CcpXcpEcuList[0].UseVxModule)
			{
				if (db.CpProtectionsWithSeedAndKeyRequired.Count > 0 || db.FileType == DatabaseFileType.A2L)
				{
					list.Add(Vocabulary.CcpXcpSettingSeedAndKey + GUIUtil.ConvertBool2ActiveString(db.CcpXcpIsSeedAndKeyUsed));
				}
				else
				{
					list.Add(Vocabulary.CcpXcpSettingSeedAndKey + Resources.NotAvailable);
				}
			}
			switch (db.BusType.Value)
			{
			case BusType.Bt_CAN:
				this.GetCanSettingsString(db, ref list);
				break;
			case BusType.Bt_FlexRay:
				this.GetFlexRaySettingsString(db, ref list);
				break;
			case BusType.Bt_Ethernet:
				this.GetEthernetSettingsString(db, ref list);
				break;
			}
			if (db.FileType == DatabaseFileType.A2L && db.CcpXcpUseDbParams)
			{
				list.Add(Resources_CcpXcp.CcpXcpSettingParametersFromDescription);
			}
			if (db.BusType.Value == BusType.Bt_CAN && db.FileType != DatabaseFileType.A2L && (db.CPType.Value == CPType.CCP || db.CPType.Value == CPType.CCP101))
			{
				list.Add("Exchange ID: " + GUIUtil.ConvertBool2OnOffString(this.DatabaseConfiguration.EnableExchangeIdHandling.Value));
			}
			for (int i = 0; i < list.Count; i++)
			{
				text += list[i];
				if (i < list.Count - 1)
				{
					text += ", ";
				}
			}
			return text;
		}

		private void GetEthernetSettingsString(Database db, ref List<string> settings)
		{
			if (db.FileType == DatabaseFileType.A2L && db.CcpXcpEcuList.Any<CcpXcpEcu>())
			{
				IPAddress iPAddress;
				if (!CcpXcpManager.Instance().GetHostIp(db, out iPAddress))
				{
					return;
				}
				settings.Add(Vocabulary.CcpXcpSettingIp + iPAddress.ToString());
			}
		}

		private void GetCanSettingsString(Database db, ref List<string> settings)
		{
		}

		private void GetFlexRaySettingsString(Database db, ref List<string> settings)
		{
		}

		private void ProclaimEcuRenaming(string ecuNameToReplace, string newEcuName)
		{
			this.ModelEditor.ProclaimCcpXcpEcuRenaming(ecuNameToReplace, newEcuName);
		}

		private void mXtraToolTipController_GetActiveObjectInfo(object sender, ToolTipControllerGetActiveObjectInfoEventArgs e)
		{
			try
			{
				GridHitInfo gridHitInfo = this.gridViewCcpXcpDescriptions.CalcHitInfo(e.ControlMousePosition);
				Database database;
				if (gridHitInfo.Column == this.colChannel && this.GetDatabase(this.gridViewCcpXcpDescriptions.GetDataSourceRowIndex(gridHitInfo.RowHandle), out database))
				{
					if (!this.pageValidator.General.HasError(database.ChannelNumber))
					{
						if (database.FileType == DatabaseFileType.XML && BusType.Bt_FlexRay == database.BusType.Value)
						{
							string arg = "A";
							if (database.ExtraCPChannel == 2u)
							{
								arg = "B";
							}
							ToolTipControlInfo info = new ToolTipControlInfo(new CellToolTipInfo(gridHitInfo.RowHandle, gridHitInfo.Column, "cell"), string.Format("Initialization on channel {0}", arg));
							e.Info = info;
						}
					}
				}
			}
			catch
			{
			}
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing && this.components != null)
			{
				this.components.Dispose();
			}
			base.Dispose(disposing);
		}

		private void InitializeComponent()
		{
			this.components = new Container();
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(CcpXcpDescriptionsGrid));
			this.gridControlCcpXcpDescriptions = new GridControl();
			this.gridViewCcpXcpDescriptions = new GridView();
			this.colIsCPEnabled = new GridColumn();
			this.repositoryItemCheckEditIsEnabled = new RepositoryItemCheckEdit();
			this.colEcu = new GridColumn();
			this.colChannel = new GridColumn();
			this.repositoryItemComboBox = new RepositoryItemComboBox();
			this.colProtcolType = new GridColumn();
			this.colDatabase = new GridColumn();
			this.colSettings = new GridColumn();
			this.repositoryItemButtonEdit = new RepositoryItemButtonEdit();
			this.repositoryItemTextEditDummy = new RepositoryItemTextEdit();
			this.mRepositoryItemComboBoxECU = new RepositoryItemComboBox();
			this.mXtraToolTipController = new XtraToolTipController(this.components);
			this.ccpXcpDatabaseBindingSource = new BindingSource(this.components);
			this.errorProviderFormat = new ErrorProvider(this.components);
			this.errorProviderLocalModel = new ErrorProvider(this.components);
			this.errorProviderGlobalModel = new ErrorProvider(this.components);
			((ISupportInitialize)this.gridControlCcpXcpDescriptions).BeginInit();
			((ISupportInitialize)this.gridViewCcpXcpDescriptions).BeginInit();
			((ISupportInitialize)this.repositoryItemCheckEditIsEnabled).BeginInit();
			((ISupportInitialize)this.repositoryItemComboBox).BeginInit();
			((ISupportInitialize)this.repositoryItemButtonEdit).BeginInit();
			((ISupportInitialize)this.repositoryItemTextEditDummy).BeginInit();
			((ISupportInitialize)this.mRepositoryItemComboBoxECU).BeginInit();
			((ISupportInitialize)this.ccpXcpDatabaseBindingSource).BeginInit();
			((ISupportInitialize)this.errorProviderFormat).BeginInit();
			((ISupportInitialize)this.errorProviderLocalModel).BeginInit();
			((ISupportInitialize)this.errorProviderGlobalModel).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.gridControlCcpXcpDescriptions, "gridControlCcpXcpDescriptions");
			this.gridControlCcpXcpDescriptions.MainView = this.gridViewCcpXcpDescriptions;
			this.gridControlCcpXcpDescriptions.Name = "gridControlCcpXcpDescriptions";
			this.gridControlCcpXcpDescriptions.RepositoryItems.AddRange(new RepositoryItem[]
			{
				this.repositoryItemComboBox,
				this.repositoryItemButtonEdit,
				this.repositoryItemCheckEditIsEnabled,
				this.repositoryItemTextEditDummy,
				this.mRepositoryItemComboBoxECU
			});
			this.gridControlCcpXcpDescriptions.ToolTipController = this.mXtraToolTipController;
			this.gridControlCcpXcpDescriptions.ViewCollection.AddRange(new BaseView[]
			{
				this.gridViewCcpXcpDescriptions
			});
			this.gridViewCcpXcpDescriptions.Columns.AddRange(new GridColumn[]
			{
				this.colIsCPEnabled,
				this.colEcu,
				this.colChannel,
				this.colProtcolType,
				this.colDatabase,
				this.colSettings
			});
			this.gridViewCcpXcpDescriptions.GridControl = this.gridControlCcpXcpDescriptions;
			this.gridViewCcpXcpDescriptions.Name = "gridViewCcpXcpDescriptions";
			this.gridViewCcpXcpDescriptions.OptionsBehavior.AllowAddRows = DefaultBoolean.False;
			this.gridViewCcpXcpDescriptions.OptionsBehavior.AllowDeleteRows = DefaultBoolean.False;
			this.gridViewCcpXcpDescriptions.OptionsBehavior.CacheValuesOnRowUpdating = CacheRowValuesMode.Disabled;
			this.gridViewCcpXcpDescriptions.OptionsBehavior.EditorShowMode = EditorShowMode.MouseUp;
			this.gridViewCcpXcpDescriptions.OptionsCustomization.AllowColumnMoving = false;
			this.gridViewCcpXcpDescriptions.OptionsCustomization.AllowFilter = false;
			this.gridViewCcpXcpDescriptions.OptionsCustomization.AllowGroup = false;
			this.gridViewCcpXcpDescriptions.OptionsCustomization.AllowQuickHideColumns = false;
			this.gridViewCcpXcpDescriptions.OptionsDetail.EnableMasterViewMode = false;
			this.gridViewCcpXcpDescriptions.OptionsView.ShowGroupPanel = false;
			this.gridViewCcpXcpDescriptions.OptionsView.ShowIndicator = false;
			this.gridViewCcpXcpDescriptions.PaintStyleName = "WindowsXP";
			this.gridViewCcpXcpDescriptions.RowHeight = 20;
			this.gridViewCcpXcpDescriptions.CustomDrawCell += new RowCellCustomDrawEventHandler(this.gridViewCCPXCPDatabases_CustomDrawCell);
			this.gridViewCcpXcpDescriptions.RowCellStyle += new RowCellStyleEventHandler(this.gridViewCcpXcpDescriptions_RowCellStyle);
			this.gridViewCcpXcpDescriptions.CustomRowCellEdit += new CustomRowCellEditEventHandler(this.gridViewCcpXcpDescriptions_CustomRowCellEdit);
			this.gridViewCcpXcpDescriptions.LeftCoordChanged += new EventHandler(this.gridViewCcpXcpDescriptions_LeftCoordChanged);
			this.gridViewCcpXcpDescriptions.TopRowChanged += new EventHandler(this.gridViewCcpXcpDescriptions_TopRowChanged);
			this.gridViewCcpXcpDescriptions.PopupMenuShowing += new PopupMenuShowingEventHandler(this.gridViewCCPXCPDatabases_PopupMenuShowing);
			this.gridViewCcpXcpDescriptions.ShowingEditor += new CancelEventHandler(this.gridViewCCPXCPDatabases_ShowingEditor);
			this.gridViewCcpXcpDescriptions.ShownEditor += new EventHandler(this.gridViewCCPXCPDatabases_ShownEditor);
			this.gridViewCcpXcpDescriptions.FocusedRowChanged += new FocusedRowChangedEventHandler(this.gridViewCCPXCPDatabases_FocusedRowChanged);
			this.gridViewCcpXcpDescriptions.CustomUnboundColumnData += new CustomColumnDataEventHandler(this.gridViewCCPXCPDatabases_CustomUnboundColumnData);
			this.gridViewCcpXcpDescriptions.KeyDown += new KeyEventHandler(this.gridViewCCPXCPDatabases_KeyDown);
			this.gridViewCcpXcpDescriptions.DoubleClick += new EventHandler(this.gridViewCCPXCPDatabases_DoubleClick);
			componentResourceManager.ApplyResources(this.colIsCPEnabled, "colIsCPEnabled");
			this.colIsCPEnabled.ColumnEdit = this.repositoryItemCheckEditIsEnabled;
			this.colIsCPEnabled.FieldName = "anyBoolean1";
			this.colIsCPEnabled.MaxWidth = 50;
			this.colIsCPEnabled.MinWidth = 50;
			this.colIsCPEnabled.Name = "colIsCPEnabled";
			this.colIsCPEnabled.OptionsColumn.AllowSize = false;
			this.colIsCPEnabled.OptionsColumn.FixedWidth = true;
			this.colIsCPEnabled.UnboundType = UnboundColumnType.Boolean;
			componentResourceManager.ApplyResources(this.repositoryItemCheckEditIsEnabled, "repositoryItemCheckEditIsEnabled");
			this.repositoryItemCheckEditIsEnabled.Name = "repositoryItemCheckEditIsEnabled";
			this.repositoryItemCheckEditIsEnabled.CheckedChanged += new EventHandler(this.repositoryItemCheckEditIsEnabled_CheckedChanged);
			componentResourceManager.ApplyResources(this.colEcu, "colEcu");
			this.colEcu.FieldName = "anyString1";
			this.colEcu.Name = "colEcu";
			this.colEcu.UnboundType = UnboundColumnType.String;
			componentResourceManager.ApplyResources(this.colChannel, "colChannel");
			this.colChannel.ColumnEdit = this.repositoryItemComboBox;
			this.colChannel.FieldName = "anyString2";
			this.colChannel.Name = "colChannel";
			this.colChannel.UnboundType = UnboundColumnType.String;
			componentResourceManager.ApplyResources(this.repositoryItemComboBox, "repositoryItemComboBox");
			this.repositoryItemComboBox.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton((ButtonPredefines)componentResourceManager.GetObject("repositoryItemComboBox.Buttons"))
			});
			this.repositoryItemComboBox.Name = "repositoryItemComboBox";
			this.repositoryItemComboBox.TextEditStyle = TextEditStyles.DisableTextEditor;
			this.repositoryItemComboBox.SelectedIndexChanged += new EventHandler(this.repositoryItemComboBox_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.colProtcolType, "colProtcolType");
			this.colProtcolType.FieldName = "anyString3";
			this.colProtcolType.Name = "colProtcolType";
			this.colProtcolType.OptionsColumn.AllowEdit = false;
			this.colProtcolType.UnboundType = UnboundColumnType.String;
			componentResourceManager.ApplyResources(this.colDatabase, "colDatabase");
			this.colDatabase.FieldName = "anyString4";
			this.colDatabase.Name = "colDatabase";
			this.colDatabase.OptionsColumn.AllowEdit = false;
			this.colDatabase.UnboundType = UnboundColumnType.String;
			componentResourceManager.ApplyResources(this.colSettings, "colSettings");
			this.colSettings.ColumnEdit = this.repositoryItemButtonEdit;
			this.colSettings.FieldName = "anyString5";
			this.colSettings.Name = "colSettings";
			this.colSettings.UnboundType = UnboundColumnType.String;
			componentResourceManager.ApplyResources(this.repositoryItemButtonEdit, "repositoryItemButtonEdit");
			this.repositoryItemButtonEdit.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton()
			});
			this.repositoryItemButtonEdit.Name = "repositoryItemButtonEdit";
			this.repositoryItemButtonEdit.TextEditStyle = TextEditStyles.DisableTextEditor;
			this.repositoryItemButtonEdit.ButtonClick += new ButtonPressedEventHandler(this.repositoryItemButtonEdit_ButtonClick);
			componentResourceManager.ApplyResources(this.repositoryItemTextEditDummy, "repositoryItemTextEditDummy");
			this.repositoryItemTextEditDummy.Name = "repositoryItemTextEditDummy";
			componentResourceManager.ApplyResources(this.mRepositoryItemComboBoxECU, "mRepositoryItemComboBoxECU");
			this.mRepositoryItemComboBoxECU.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton((ButtonPredefines)componentResourceManager.GetObject("mRepositoryItemComboBoxECU.Buttons"))
			});
			this.mRepositoryItemComboBoxECU.Name = "mRepositoryItemComboBoxECU";
			this.mRepositoryItemComboBoxECU.TextEditStyle = TextEditStyles.DisableTextEditor;
			this.mRepositoryItemComboBoxECU.SelectedIndexChanged += new EventHandler(this.mRepositoryItemComboBoxECU_SelectedIndexChanged);
			this.mRepositoryItemComboBoxECU.QueryPopUp += new CancelEventHandler(this.mRepositoryItemComboBoxECU_QueryPopUp);
			this.mXtraToolTipController.Appearance.BackColor = (Color)componentResourceManager.GetObject("mXtraToolTipController.Appearance.BackColor");
			this.mXtraToolTipController.Appearance.ForeColor = (Color)componentResourceManager.GetObject("mXtraToolTipController.Appearance.ForeColor");
			this.mXtraToolTipController.Appearance.Options.UseBackColor = true;
			this.mXtraToolTipController.Appearance.Options.UseForeColor = true;
			this.mXtraToolTipController.Appearance.Options.UseTextOptions = true;
			this.mXtraToolTipController.Appearance.TextOptions.HotkeyPrefix = HKeyPrefix.None;
			this.mXtraToolTipController.AppearanceTitle.BackColor = (Color)componentResourceManager.GetObject("mXtraToolTipController.AppearanceTitle.BackColor");
			this.mXtraToolTipController.AppearanceTitle.ForeColor = (Color)componentResourceManager.GetObject("mXtraToolTipController.AppearanceTitle.ForeColor");
			this.mXtraToolTipController.AppearanceTitle.Options.UseBackColor = true;
			this.mXtraToolTipController.AppearanceTitle.Options.UseForeColor = true;
			this.mXtraToolTipController.AppearanceTitle.Options.UseTextOptions = true;
			this.mXtraToolTipController.AppearanceTitle.TextOptions.HotkeyPrefix = HKeyPrefix.None;
			this.mXtraToolTipController.MaxWidth = 500;
			this.mXtraToolTipController.ShowPrefix = false;
			this.mXtraToolTipController.UseNativeLookAndFeel = true;
			this.mXtraToolTipController.GetActiveObjectInfo += new ToolTipControllerGetActiveObjectInfoEventHandler(this.mXtraToolTipController_GetActiveObjectInfo);
			this.errorProviderFormat.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderFormat.ContainerControl = this;
			this.errorProviderLocalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderLocalModel.ContainerControl = this;
			this.errorProviderGlobalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderGlobalModel.ContainerControl = this;
			componentResourceManager.ApplyResources(this.errorProviderGlobalModel, "errorProviderGlobalModel");
			this.AllowDrop = true;
			base.AutoScaleMode = AutoScaleMode.Inherit;
			base.Controls.Add(this.gridControlCcpXcpDescriptions);
			base.Name = "CcpXcpDescriptionsGrid";
			componentResourceManager.ApplyResources(this, "$this");
			base.DragDrop += new DragEventHandler(this.CCPXCPDatabaseGrid_DragDrop);
			base.DragEnter += new DragEventHandler(this.CCPXCPDatabaseGrid_DragEnter);
			base.Resize += new EventHandler(this.CcpXcpDescriptionsGrid_Resize);
			((ISupportInitialize)this.gridControlCcpXcpDescriptions).EndInit();
			((ISupportInitialize)this.gridViewCcpXcpDescriptions).EndInit();
			((ISupportInitialize)this.repositoryItemCheckEditIsEnabled).EndInit();
			((ISupportInitialize)this.repositoryItemComboBox).EndInit();
			((ISupportInitialize)this.repositoryItemButtonEdit).EndInit();
			((ISupportInitialize)this.repositoryItemTextEditDummy).EndInit();
			((ISupportInitialize)this.mRepositoryItemComboBoxECU).EndInit();
			((ISupportInitialize)this.ccpXcpDatabaseBindingSource).EndInit();
			((ISupportInitialize)this.errorProviderFormat).EndInit();
			((ISupportInitialize)this.errorProviderLocalModel).EndInit();
			((ISupportInitialize)this.errorProviderGlobalModel).EndInit();
			base.ResumeLayout(false);
		}
	}
}
