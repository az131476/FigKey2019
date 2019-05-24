using DevExpress.Data;
using DevExpress.LookAndFeel;
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
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Vector.UtilityFunctions;
using Vector.UtilityFunctions.XtraGrid;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.GUI.Common;
using Vector.VLConfig.GUI.Helpers;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;
using Vector.VLConfig.ValidationFramework;

namespace Vector.VLConfig.GUI.InterfaceModePage
{
	public class SignalExportList : UserControl, ISplitButtonExClient
	{
		private readonly GeneralService gridViewGeneralService;

		private readonly KeyboardNavigationService gridViewKeyboardNavigationService;

		private InterfaceModeConfiguration interfaceModeConfiguration;

		private GUIElementManager_ControlGridTree guiElementManager;

		private CustomErrorProvider customErrorProvider;

		private PageValidator pageValidator;

		private bool isInitControls;

		private readonly SplitButtonEx splitButtonEx;

		private readonly char signalNameIndexSeparator = '_';

		private IContainer components;

		private GridControl gridControlSignalExportList;

		private GridView gridViewExportList;

		private CheckBox checkBoxEnablePublic3GAccess;

		private CheckBox checkBoxAllowSignalExport;

		private GridColumn gridColumnActive;

		private GridColumn gridColumnSignal;

		private GridColumn gridColumnName;

		private GridColumn gridColumnComment;

		private Label labelAddSignal;

		private SplitButton splitButton;

		private Button buttonRemove;

		private ErrorProvider errorProviderFormat;

		private ErrorProvider errorProviderLocalModel;

		private ErrorProvider errorProviderGlobalModel;

		private CheckBox checkBoxUseWebDisplay;

		private Button buttonChooseWebDisplayPath;

		private TextBox textBoxWebDisplayPath;

		private TextBox textBoxExportCycle;

		private Label label1;

		private GridColumn gridColumnChannel;

		private RepositoryItemComboBox repositoryItemComboBoxChannel;

		private RepositoryItemCheckEdit repositoryItemCheckEditIsEnabled;

		private XtraToolTipController xtraToolTipController;

		private RepositoryItemButtonEdit repositoryItemButtonSignal;

		private RepositoryItemTextEdit repositoryItemTextEditDummy;

		private RepositoryItemComboBox repositoryItemComboBoxVariables;

		private GridColumn gridColumnIcon;

		public new event EventHandler Validating;

		public IModelValidator ModelValidator
		{
			get;
			set;
		}

		public IApplicationDatabaseManager ApplicationDatabaseManager
		{
			get;
			set;
		}

		public InterfaceModeConfiguration InterfaceModeConfiguration
		{
			get
			{
				return this.interfaceModeConfiguration;
			}
			set
			{
				this.interfaceModeConfiguration = value;
				if (this.interfaceModeConfiguration != null)
				{
					List<int> selectedDataRows = this.StoreCurrentSelection();
					int idx = this.StoreFocusedRow();
					this.ResetValidationFramework();
					this.gridControlSignalExportList.DataSource = this.interfaceModeConfiguration.WebDisplayExportSignals;
					this.gridViewExportList.ClearSelection();
					this.RestoreFocusedRow(idx);
					this.RestoreSelection(selectedDataRows);
				}
				this.UpdateGUI();
				this.ValidateInput(false);
				this.gridViewExportList.RefreshData();
			}
		}

		public string ConfigurationFolderPath
		{
			get;
			set;
		}

		public SplitButton SplitButton
		{
			get
			{
				return this.splitButton;
			}
		}

		public string SplitButtonEmptyDefault
		{
			get
			{
				return Resources.SplitButtonEmptyDefault;
			}
		}

		public SignalExportList()
		{
			this.InitializeComponent();
			this.guiElementManager = new GUIElementManager_ControlGridTree();
			this.customErrorProvider = new CustomErrorProvider(this.errorProviderFormat);
			this.customErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.LocalModelError, this.errorProviderLocalModel);
			this.customErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.GlobalModelError, this.errorProviderGlobalModel);
			this.pageValidator = new PageValidator(this.customErrorProvider);
			this.gridViewGeneralService = new GeneralService(this.gridViewExportList);
			this.gridViewKeyboardNavigationService = new KeyboardNavigationService(this.gridViewExportList);
			this.splitButtonEx = new SplitButtonEx(this);
			GUIUtil.InitSplitButtonMenuWebDisplayExportSignalTypes(this.splitButtonEx);
			this.FillVariablesCombobox();
			this.splitButton.AutoSize = false;
		}

		private void Raise_Validating(object sender, EventArgs e)
		{
			if (this.Validating != null)
			{
				this.Validating(sender, e);
			}
		}

		private void AddSymbolicSignal(BusType busType)
		{
			WebDisplayExportSignal webDisplayExportSignal;
			if (this.CreateSymbolicSignal(busType, out webDisplayExportSignal))
			{
				this.interfaceModeConfiguration.AddWebDisplayExportSignal(webDisplayExportSignal);
				this.ValidateInput(true);
				this.SelectRowOfSignals(new List<WebDisplayExportSignal>
				{
					webDisplayExportSignal
				});
			}
		}

		private void AddLtlSystemInformation()
		{
			string ltlSystemInformationDefaultName = this.GetLtlSystemInformationDefaultName(LtlSystemInformation.Logger1Files);
			GenerationUtil.IndexNameIfNecessary(ltlSystemInformationDefaultName, out ltlSystemInformationDefaultName, (from sig in this.interfaceModeConfiguration.WebDisplayExportSignals
			select sig.Name.Value).ToList<string>(), this.signalNameIndexSeparator.ToString(), true);
			WebDisplayExportSignal webDisplayExportSignal = new WebDisplayExportSignal(ltlSystemInformationDefaultName, this.LtlSystemInformationToDisplayString(LtlSystemInformation.Logger1Files), LtlSystemInformation.Logger1Files);
			this.interfaceModeConfiguration.AddWebDisplayExportSignal(webDisplayExportSignal);
			this.ValidateInput(true);
			this.SelectRowOfSignals(new List<WebDisplayExportSignal>
			{
				webDisplayExportSignal
			});
		}

		private void ReplaceSymbolicSignal(WebDisplayExportSignal expSigToReplace)
		{
			WebDisplayExportSignal webDisplayExportSignal;
			if (this.CreateSymbolicSignal(expSigToReplace.BusType.Value, out webDisplayExportSignal))
			{
				webDisplayExportSignal.Name.Value = expSigToReplace.Name.Value;
				webDisplayExportSignal.Comment.Value = expSigToReplace.Comment.Value;
				if (this.interfaceModeConfiguration.ReplaceWebDisplayExportSignal(expSigToReplace, webDisplayExportSignal))
				{
					this.ValidateInput(true);
				}
			}
		}

		private bool CreateSymbolicSignal(BusType busType, out WebDisplayExportSignal expSig)
		{
			string text = "";
			string text2 = "";
			string text3 = "";
			string databaseName = "";
			string networkName = "";
			expSig = new WebDisplayExportSignal(text2, text2, text, networkName, databaseName, text3, busType, 0u, false);
			bool isFlexrayPDU = false;
			if (!this.ModelValidator.DatabaseServices.HasDatabasesConfiguredFor(busType))
			{
				string arg;
				switch (busType)
				{
				case BusType.Bt_LIN:
					arg = Vocabulary.LIN;
					break;
				case BusType.Bt_FlexRay:
					arg = Vocabulary.Flexray;
					break;
				default:
					arg = Vocabulary.CAN;
					break;
				}
				InformMessageBox.Info(string.Format(Resources.NoDatabaseAvailableForBustype, arg));
				return false;
			}
			if (!this.ApplicationDatabaseManager.SelectSignalInDatabase(ref text, ref text2, ref databaseName, ref text3, ref networkName, ref busType, ref isFlexrayPDU))
			{
				return false;
			}
			string message;
			if (!this.ModelValidator.DatabaseServices.IsSymbolicSignalInsertAllowed(text2, text, networkName, text3, busType, this.ModelValidator.LoggerSpecifics.Recording.MaximumExportSignalLength, out message))
			{
				InformMessageBox.Error(message);
				return false;
			}
			text3 = this.ModelValidator.GetFilePathRelativeToConfiguration(text3);
			uint num = this.ModelValidator.GetFirstActiveOrDefaultChannel(busType);
			IList<uint> channelAssignmentOfDatabase = this.ModelValidator.DatabaseServices.GetChannelAssignmentOfDatabase(text3, networkName);
			if (channelAssignmentOfDatabase.Count > 0)
			{
				num = channelAssignmentOfDatabase[0];
				if (num == Database.ChannelNumber_FlexrayAB && BusType.Bt_FlexRay == busType)
				{
					num = 1u;
					if (text.EndsWith(Constants.FlexrayChannelB_Postfix))
					{
						num = 2u;
					}
				}
			}
			string filePathRelativeToConfiguration = this.ModelValidator.GetFilePathRelativeToConfiguration(text3);
			string webDisplayExportSignalDefaultName;
			if (!GenerationUtil.MakeStringCaplCompliant(text2, out webDisplayExportSignalDefaultName))
			{
				webDisplayExportSignalDefaultName = Resources.WebDisplayExportSignalDefaultName;
			}
			GenerationUtil.IndexNameIfNecessary(webDisplayExportSignalDefaultName, out webDisplayExportSignalDefaultName, (from sig in this.interfaceModeConfiguration.WebDisplayExportSignals
			select sig.Name.Value).ToList<string>(), this.signalNameIndexSeparator.ToString(), true);
			expSig = new WebDisplayExportSignal(webDisplayExportSignalDefaultName, text2, text, networkName, databaseName, filePathRelativeToConfiguration, busType, num, isFlexrayPDU);
			return true;
		}

		public void RemoveSelectedSignals()
		{
			int count = this.GetSelectedExportSignals().Count;
			if (count < 1)
			{
				return;
			}
			if (count > 1 && InformMessageBox.Show(EnumInfoType.Warning, EnumQuestionType.QuestionDefaultNo, Resources.WarningDeleteEvents) != DialogResult.Yes)
			{
				return;
			}
			int num = this.gridViewExportList.GetSelectedRows().Max() - count;
			foreach (WebDisplayExportSignal current in this.GetSelectedExportSignals())
			{
				this.interfaceModeConfiguration.RemoveWebDisplayExportSignal(current);
			}
			this.ValidateInput(true);
			int num2 = num + 1;
			int num3 = this.gridViewExportList.RowCount - 1;
			this.gridViewExportList.ClearSelection();
			if (this.gridViewExportList.RowCount > 0)
			{
				if (num2 > num3)
				{
					num2 = num3;
				}
				else if (num2 < 0)
				{
					num2 = 0;
				}
				this.gridViewExportList.SelectRow(num2);
				this.gridViewExportList.FocusedRowHandle = num2;
			}
			this.EnableControls();
			this.gridControlSignalExportList.Focus();
		}

		private void gridControlSignalExportList_Load(object sender, EventArgs e)
		{
			this.gridViewGeneralService.InitAppearance();
		}

		private void gridViewExportList_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
		{
			this.gridViewGeneralService.PopupMenuShowing(e);
			if (e.MenuType == GridMenuType.Column)
			{
				this.CustomizeColumnHeaderMenu(e);
			}
		}

		private void CustomizeColumnHeaderMenu(PopupMenuShowingEventArgs e)
		{
			string localizedString = GridLocalizer.Active.GetLocalizedString(GridStringId.MenuColumnColumnCustomization);
			for (int i = e.Menu.Items.Count - 1; i >= 0; i--)
			{
				if (localizedString == e.Menu.Items[i].Caption)
				{
					e.Menu.Items.RemoveAt(i);
				}
			}
		}

		private void gridViewExportList_KeyDown(object sender, KeyEventArgs e)
		{
			this.gridViewKeyboardNavigationService.GridControlProcessGridKey(e);
			if (e.KeyCode == Keys.Delete)
			{
				this.RemoveSelectedSignals();
				return;
			}
			if (e.KeyCode == Keys.Space && this.GetSelectedExportSignals().Count > 1)
			{
				this.ToggleSignalActiveState();
			}
		}

		private void gridViewExportList_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			this.EnableControls();
		}

		private void gridViewExportList_MouseDown(object sender, MouseEventArgs e)
		{
			if ((Control.ModifierKeys & Keys.Control) != Keys.Control && (Control.ModifierKeys & Keys.Shift) != Keys.Shift)
			{
				GridHitInfo gridHitInfo = this.gridViewExportList.CalcHitInfo(e.Location);
				if (gridHitInfo.InRowCell && gridHitInfo.Column.RealColumnEdit is RepositoryItemCheckEdit)
				{
					this.gridViewExportList.ClearSelection();
					this.gridViewExportList.FocusedColumn = gridHitInfo.Column;
					this.gridViewExportList.FocusedRowHandle = gridHitInfo.RowHandle;
					this.gridViewExportList.ShowEditor();
					CheckEdit checkEdit = this.gridViewExportList.ActiveEditor as CheckEdit;
					if (checkEdit != null)
					{
						checkEdit.Toggle();
						DXMouseEventArgs.GetMouseArgs(e).Handled = true;
					}
				}
			}
		}

		private void gridViewExportList_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
		{
			WebDisplayExportSignal webDisplayExportSignal;
			bool exportSignal = this.GetExportSignal(this.gridViewExportList.GetDataSourceRowIndex(e.RowHandle), out webDisplayExportSignal);
			if (e.Column == this.gridColumnIcon && exportSignal)
			{
				if (webDisplayExportSignal.Type == WebDisplayExportSignalType.Signal)
				{
					if (webDisplayExportSignal.BusType.Value == BusType.Bt_CAN)
					{
						GridUtil.DrawImageTextCell(e, Resources.ImageSymbSignalCAN);
						return;
					}
					if (webDisplayExportSignal.BusType.Value == BusType.Bt_LIN)
					{
						GridUtil.DrawImageTextCell(e, Resources.ImageSymbSignalLIN);
						return;
					}
					if (webDisplayExportSignal.BusType.Value == BusType.Bt_FlexRay)
					{
						GridUtil.DrawImageTextCell(e, Resources.ImageSymbSignalFlexRay);
						return;
					}
				}
				else
				{
					if (webDisplayExportSignal.Type == WebDisplayExportSignalType.LtlSystemInformation)
					{
						GridUtil.DrawImageTextCell(e, Resources.IconDeviceInformation.ToBitmap());
						return;
					}
					GridUtil.DrawImageTextCell(e, Resources.ImageSystemVariable);
					return;
				}
			}
			else
			{
				IValidatedGUIElement gUIElement = this.guiElementManager.GetGUIElement(e.Column, this.gridViewExportList.GetDataSourceRowIndex(e.RowHandle));
				this.customErrorProvider.Grid.DisplayError(gUIElement, e);
			}
		}

		private void gridViewExportList_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
		{
			WebDisplayExportSignal expSig;
			if (!this.GetExportSignal(e.ListSourceRowIndex, out expSig))
			{
				return;
			}
			if (e.Column == this.gridColumnActive)
			{
				this.UnboundColumnActive(expSig, e);
				return;
			}
			if (e.Column == this.gridColumnSignal)
			{
				this.UnboundColumnSignal(expSig, e);
				return;
			}
			if (e.Column == this.gridColumnChannel)
			{
				this.UnboundColumnChannel(expSig, e);
				return;
			}
			if (e.Column == this.gridColumnName)
			{
				this.UnboundColumnName(expSig, e);
				return;
			}
			if (e.Column == this.gridColumnComment)
			{
				this.UnboundColumnComment(expSig, e);
			}
		}

		private void gridViewExportList_ShowingEditor(object sender, CancelEventArgs e)
		{
			WebDisplayExportSignal dataRecord = this.gridViewExportList.GetRow(this.gridViewExportList.FocusedRowHandle) as WebDisplayExportSignal;
			this.gridViewGeneralService.ShowingEditor<WebDisplayExportSignal>(e, dataRecord, this.gridViewExportList.FocusedColumn, new GeneralService.IsReadOnlyAtAll<WebDisplayExportSignal>(this.IsCellReadOnlyAtAll), new GeneralService.IsReadOnlyByCellContent<WebDisplayExportSignal>(this.IsCellReadOnlyByCellContent));
		}

		private void gridViewExportList_ShownEditor(object sender, EventArgs e)
		{
			if (this.gridViewExportList.FocusedColumn == this.gridColumnChannel)
			{
				this.ShownEditorChannel();
			}
		}

		private void ShownEditorChannel()
		{
			ComboBoxEdit comboBoxEdit = this.gridViewExportList.ActiveEditor as ComboBoxEdit;
			if (comboBoxEdit != null)
			{
				ReadOnlyCollection<WebDisplayExportSignal> readOnlyCollection = this.gridControlSignalExportList.DataSource as ReadOnlyCollection<WebDisplayExportSignal>;
				if (readOnlyCollection == null)
				{
					return;
				}
				WebDisplayExportSignal webDisplayExportSignal = readOnlyCollection[this.gridViewExportList.GetFocusedDataSourceRowIndex()];
				if (webDisplayExportSignal.Type == WebDisplayExportSignalType.Signal)
				{
					if (webDisplayExportSignal.BusType.Value == BusType.Bt_CAN)
					{
						this.FillCANChannelCombobox(comboBoxEdit);
						return;
					}
					if (webDisplayExportSignal.BusType.Value == BusType.Bt_LIN)
					{
						this.FillLINChannelCombobox(comboBoxEdit);
						return;
					}
					if (webDisplayExportSignal.BusType.Value == BusType.Bt_FlexRay)
					{
						if (webDisplayExportSignal.ChannelNumber.Value != Database.ChannelNumber_FlexrayAB)
						{
							this.FillFlexrayChannelCombobox(comboBoxEdit);
							return;
						}
						comboBoxEdit.Properties.Items.Add(Vocabulary.FlexrayChannelAB);
					}
				}
			}
		}

		private void gridViewExportList_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
		{
			WebDisplayExportSignal webDisplayExportSignal;
			if (!this.GetExportSignal(this.gridViewExportList.GetDataSourceRowIndex(e.RowHandle), out webDisplayExportSignal))
			{
				return;
			}
			if (e.Column == this.gridColumnChannel && e.RowHandle == this.gridViewExportList.FocusedRowHandle)
			{
				if (webDisplayExportSignal.Type == WebDisplayExportSignalType.Signal)
				{
					e.RepositoryItem = this.repositoryItemComboBoxChannel;
				}
				else
				{
					e.RepositoryItem = this.repositoryItemTextEditDummy;
				}
			}
			if (e.Column == this.gridColumnSignal && e.RowHandle == this.gridViewExportList.FocusedRowHandle)
			{
				if (webDisplayExportSignal.Type == WebDisplayExportSignalType.Signal)
				{
					e.RepositoryItem = this.repositoryItemButtonSignal;
					return;
				}
				e.RepositoryItem = this.repositoryItemComboBoxVariables;
			}
		}

		private void repositoryItemButtonSignal_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			int focusedDataSourceRowIndex = this.gridViewExportList.GetFocusedDataSourceRowIndex();
			WebDisplayExportSignal expSigToReplace;
			if (!this.GetExportSignal(focusedDataSourceRowIndex, out expSigToReplace))
			{
				return;
			}
			this.ReplaceSymbolicSignal(expSigToReplace);
		}

		private void gridViewExportList_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
		{
			this.gridViewGeneralService.FocusedRowChanged(e);
			this.EnableControls();
		}

		private void repositoryItemCheckEditIsEnabled_CheckedChanged(object sender, EventArgs e)
		{
			this.gridViewExportList.PostEditor();
		}

		private void gridControlSignalExportList_VisibleChanged(object sender, EventArgs e)
		{
			if (base.Visible)
			{
				this.DisplayErrors();
			}
		}

		private void gridViewExportList_TopRowChanged(object sender, EventArgs e)
		{
			this.DisplayErrors();
		}

		private void gridViewExportList_LeftCoordChanged(object sender, EventArgs e)
		{
			this.DisplayErrors();
		}

		private void gridControlSignalExportList_Resize(object sender, EventArgs e)
		{
			this.DisplayErrors();
		}

		private void repositoryItemComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.gridViewExportList.PostEditor();
		}

		private void repositoryItemComboBoxVariables_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.gridViewExportList.PostEditor();
		}

		private void buttonRemove_Click(object sender, EventArgs e)
		{
			this.RemoveSelectedSignals();
		}

		private void checkBoxUseWebDisplay_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.EnableControls();
			this.ValidateInput(false);
		}

		private void checkBoxAllowSignalSamplingMode_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.EnableControls();
			this.ValidateInput(false);
		}

		private void checkBoxEnablePublic3GAccess_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput(false);
		}

		private void buttonChooseWebDisplayPath_Click(object sender, EventArgs e)
		{
			GenericOpenFileDialog.FileName = string.Empty;
			try
			{
				if (this.interfaceModeConfiguration != null && this.interfaceModeConfiguration.CustomWebDisplay.FilePath != null && !string.IsNullOrEmpty(this.interfaceModeConfiguration.CustomWebDisplay.FilePath.Value) && Directory.Exists(FileSystemServices.GetAbsolutePath(FileSystemServices.GetFolderPathFromFilePath(this.interfaceModeConfiguration.CustomWebDisplay.FilePath.Value), FileSystemServices.GetFolderPathFromFilePath(this.ConfigurationFolderPath))))
				{
					GenericOpenFileDialog.InitialDirectory = FileSystemServices.GetAbsolutePath(FileSystemServices.GetFolderPathFromFilePath(this.interfaceModeConfiguration.CustomWebDisplay.FilePath.Value), FileSystemServices.GetFolderPathFromFilePath(this.ConfigurationFolderPath));
				}
				else if (!string.IsNullOrEmpty(this.ConfigurationFolderPath) && Directory.Exists(FileSystemServices.GetFolderPathFromFilePath(this.ConfigurationFolderPath)))
				{
					GenericOpenFileDialog.InitialDirectory = this.ConfigurationFolderPath;
				}
				else
				{
					GenericOpenFileDialog.InitialDirectory = "";
				}
			}
			catch (Exception)
			{
				GenericOpenFileDialog.InitialDirectory = "";
			}
			if (DialogResult.OK == GenericOpenFileDialog.ShowDialog(string.Empty, Vocabulary.FileFilterWebDisplayIndex))
			{
				string fileName = GenericOpenFileDialog.FileName;
				if (GUIUtil.FileAccessible(fileName))
				{
					if (FileSystemServices.TryMakeFilePathRelativeToConfiguration(FileSystemServices.GetFolderPathFromFilePath(this.ConfigurationFolderPath), ref fileName))
					{
						this.textBoxWebDisplayPath.Text = fileName;
					}
					else
					{
						this.textBoxWebDisplayPath.Text = GenericOpenFileDialog.FileName;
					}
					this.ValidateInput(false);
				}
			}
		}

		private void textBoxExportCycle_Validating(object sender, CancelEventArgs e)
		{
			try
			{
				uint num = Convert.ToUInt32(this.textBoxExportCycle.Text);
				this.textBoxExportCycle.Text = ((num + 25u) / 50u * 50u).ToString();
			}
			catch (Exception)
			{
			}
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput(false);
		}

		private void textBoxWebDisplayPath_Validating(object sender, CancelEventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput(false);
		}

		private void textBoxWebDisplayPath_DragDrop(object sender, DragEventArgs e)
		{
			GUIUtil.FileDropContent kindOfDrop = GUIUtil.GetKindOfDrop(e);
			if (kindOfDrop != GUIUtil.FileDropContent.File)
			{
				e.Effect = DragDropEffects.None;
				return;
			}
			string[] array = (string[])e.Data.GetData(DataFormats.FileDrop);
			string text = array[0];
			if (GUIUtil.FileAccessible(text) && !FileSystemServices.TryMakeFilePathRelativeToConfiguration(FileSystemServices.GetFolderPathFromFilePath(this.ConfigurationFolderPath), ref text))
			{
				this.textBoxWebDisplayPath.Text = array[0];
			}
			this.textBoxWebDisplayPath.Text = text;
			this.ValidateInput(false);
		}

		private void textBoxWebDisplayPath_DragEnter(object sender, DragEventArgs e)
		{
			if (GUIUtil.GetKindOfDrop(e) == GUIUtil.FileDropContent.File)
			{
				e.Effect = DragDropEffects.Copy;
				return;
			}
			e.Effect = DragDropEffects.None;
		}

		private void UnboundColumnActive(WebDisplayExportSignal expSig, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				e.Value = expSig.IsActive.Value;
				return;
			}
			bool flag = false;
			this.pageValidator.Grid.UpdateModel<bool>((bool)e.Value, expSig.IsActive, out flag);
			if (flag)
			{
				this.ValidateInput(true);
			}
		}

		private void UnboundColumnSignal(WebDisplayExportSignal expSig, CustomColumnDataEventArgs e)
		{
			if (!e.IsGetData)
			{
				bool flag = false;
				if (expSig.Type == WebDisplayExportSignalType.LtlSystemInformation)
				{
					WebDisplayExportSignal webDisplayExportSignal;
					if (!this.GetFocusedExportSignal(out webDisplayExportSignal) || webDisplayExportSignal == null)
					{
						return;
					}
					string text = webDisplayExportSignal.Name.Value;
					string value = webDisplayExportSignal.Comment.Value;
					string text2;
					int num;
					if (GenerationUtil.NameHasIndex(text, this.signalNameIndexSeparator) && GenerationUtil.RemoveIndexFromName(text, this.signalNameIndexSeparator, out text2, out num))
					{
						text = text2;
					}
					LtlSystemInformation ltlSystemInformation = this.DisplayStringToLtlSystemInformation(e.Value.ToString());
					string ltlSystemInformationDefaultName = this.GetLtlSystemInformationDefaultName(ltlSystemInformation);
					string value2 = this.LtlSystemInformationToDisplayString(ltlSystemInformation);
					if (text == this.GetLtlSystemInformationDefaultName(webDisplayExportSignal.LtlSystemInformation.Value))
					{
						GenerationUtil.IndexNameIfNecessary(ltlSystemInformationDefaultName, out ltlSystemInformationDefaultName, (from sig in this.interfaceModeConfiguration.WebDisplayExportSignals
						select sig.Name.Value).ToList<string>(), this.signalNameIndexSeparator.ToString(), true);
						this.UnboundColumnName(expSig, new CustomColumnDataEventArgs(this.gridColumnName, e.ListSourceRowIndex, ltlSystemInformationDefaultName, false));
					}
					if (value == this.LtlSystemInformationToDisplayString(webDisplayExportSignal.LtlSystemInformation.Value))
					{
						this.UnboundColumnComment(expSig, new CustomColumnDataEventArgs(this.gridColumnComment, e.ListSourceRowIndex, value2, false));
					}
					this.pageValidator.Grid.UpdateModel<LtlSystemInformation>(ltlSystemInformation, expSig.LtlSystemInformation, out flag);
				}
				if (flag)
				{
					this.ValidateInput(true);
				}
				return;
			}
			if (expSig.Type == WebDisplayExportSignalType.Signal)
			{
				e.Value = string.Concat(new string[]
				{
					expSig.DatabaseName.Value,
					"::",
					expSig.MessageName.Value,
					"::",
					expSig.SignalName.Value
				});
				return;
			}
			e.Value = this.LtlSystemInformationToDisplayString(expSig.LtlSystemInformation.Value);
		}

		private void UnboundColumnChannel(WebDisplayExportSignal expSig, CustomColumnDataEventArgs e)
		{
			if (!e.IsGetData)
			{
				bool flag = false;
				uint value = 1u;
				if (BusType.Bt_CAN == expSig.BusType.Value)
				{
					value = GUIUtil.MapCANChannelString2Number(e.Value.ToString());
				}
				else if (BusType.Bt_LIN == expSig.BusType.Value)
				{
					value = GUIUtil.MapLINChannelString2Number(e.Value.ToString());
				}
				else if (BusType.Bt_FlexRay == expSig.BusType.Value)
				{
					value = GUIUtil.MapFlexrayChannelString2Number(e.Value.ToString());
				}
				this.pageValidator.Grid.UpdateModel<uint>(value, expSig.ChannelNumber, out flag);
				if (flag)
				{
					this.ValidateInput(true);
				}
				return;
			}
			if (expSig.Type != WebDisplayExportSignalType.Signal)
			{
				e.Value = string.Empty;
				return;
			}
			if (BusType.Bt_CAN == expSig.BusType.Value)
			{
				e.Value = GUIUtil.MapCANChannelNumber2String(expSig.ChannelNumber.Value);
			}
			else if (BusType.Bt_LIN == expSig.BusType.Value)
			{
				e.Value = GUIUtil.MapLINChannelNumber2String(expSig.ChannelNumber.Value, this.ModelValidator.LoggerSpecifics);
			}
			else if (BusType.Bt_FlexRay == expSig.BusType.Value)
			{
				e.Value = GUIUtil.MapFlexrayChannelNumber2String(expSig.ChannelNumber.Value);
			}
			this.pageValidator.Grid.StoreMapping(expSig.ChannelNumber, this.guiElementManager.GetGUIElement(e.Column, e.ListSourceRowIndex));
		}

		private void UnboundColumnName(WebDisplayExportSignal expSig, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				e.Value = expSig.Name.Value;
				return;
			}
			bool flag = false;
			this.pageValidator.Grid.UpdateModel<string>(e.Value.ToString(), expSig.Name, out flag);
			if (flag)
			{
				this.ValidateInput(true);
			}
		}

		private void UnboundColumnComment(WebDisplayExportSignal expSig, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				e.Value = expSig.Comment.Value;
				return;
			}
			bool flag = false;
			this.pageValidator.Grid.UpdateModel<string>(e.Value.ToString(), expSig.Comment, out flag);
			if (flag)
			{
				this.ValidateInput(true);
			}
		}

		public void DisplayErrors()
		{
			this.StoreMapping4VisibleCells();
			this.pageValidator.General.ActivateErrorProvidersForFormatAndModelErrors();
		}

		public bool ValidateInput(bool isDataChanged = false)
		{
			if (this.interfaceModeConfiguration == null)
			{
				return false;
			}
			bool flag = true;
			this.pageValidator.General.ResetAllErrorProviders();
			this.pageValidator.General.ResetAllFormatErrors();
			this.ResetValidationFramework();
			this.StoreMapping4VisibleCells();
			bool flag2;
			flag &= this.pageValidator.Control.UpdateModel<bool>(this.checkBoxAllowSignalExport.Checked, this.interfaceModeConfiguration.UseSignalExport, this.guiElementManager.GetGUIElement(this.checkBoxAllowSignalExport), out flag2);
			bool flag3 = isDataChanged | flag2;
			if (this.interfaceModeConfiguration.UseSignalExport.Value)
			{
				flag &= this.pageValidator.Control.ValidateFormatAndUpdateModel_UInt32(this.textBoxExportCycle.Text, this.interfaceModeConfiguration.ExportCycle, this.guiElementManager.GetGUIElement(this.textBoxExportCycle), out flag2);
				flag3 |= flag2;
				flag &= this.pageValidator.Control.UpdateModel<bool>(this.checkBoxEnablePublic3GAccess.Checked, this.interfaceModeConfiguration.EnableAlwaysOnline, this.guiElementManager.GetGUIElement(this.checkBoxEnablePublic3GAccess), out flag2);
				flag3 |= flag2;
				flag &= this.pageValidator.Control.UpdateModel<bool>(this.checkBoxUseWebDisplay.Checked, this.interfaceModeConfiguration.UseCustomWebDisplay, this.guiElementManager.GetGUIElement(this.checkBoxUseWebDisplay), out flag2);
				flag3 |= flag2;
				flag &= this.pageValidator.Control.UpdateModel<string>(this.textBoxWebDisplayPath.Text, this.interfaceModeConfiguration.CustomWebDisplay.FilePath, this.guiElementManager.GetGUIElement(this.textBoxWebDisplayPath), out flag2);
				flag3 |= flag2;
				flag &= this.ModelValidator.Validate(this.interfaceModeConfiguration, PageType.InterfaceMode_SignalExport, flag3, this.pageValidator);
				this.pageValidator.General.ActivateErrorProvidersForFormatAndModelErrors();
			}
			this.Raise_Validating(this, EventArgs.Empty);
			return flag;
		}

		public void Reset()
		{
			this.splitButtonEx.UpdateSplitMenu();
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

		public void ResetValidationFramework()
		{
			this.pageValidator.General.Reset();
			this.guiElementManager.Reset();
		}

		private void StoreMapping4VisibleCells()
		{
			PageValidatorGridUtil.StoreRowMappingForVisibleRows(this.gridViewExportList, new PageValidatorGridUtil.StoreRowMappingHandler(this.OnPageValidatorGridUtilStoreRowMapping));
		}

		private void OnPageValidatorGridUtilStoreRowMapping(int dataSourceRowIdx)
		{
			if (dataSourceRowIdx < 0 || dataSourceRowIdx >= this.interfaceModeConfiguration.WebDisplayExportSignals.Count)
			{
				return;
			}
			WebDisplayExportSignal expSig = this.interfaceModeConfiguration.WebDisplayExportSignals[dataSourceRowIdx];
			this.StoreMapping4VisibleColumns(expSig, dataSourceRowIdx);
		}

		private void StoreMapping4VisibleColumns(WebDisplayExportSignal expSig, int dataSourceIdx)
		{
			if (PageValidatorGridUtil.IsColumnVisible(this.gridColumnActive, this.gridViewExportList))
			{
				IValidatedGUIElement gUIElement = this.guiElementManager.GetGUIElement(this.gridColumnActive, dataSourceIdx);
				this.pageValidator.Grid.StoreMapping(expSig.IsActive, gUIElement);
			}
			if (PageValidatorGridUtil.IsColumnVisible(this.gridColumnSignal, this.gridViewExportList))
			{
				IValidatedGUIElement gUIElement = this.guiElementManager.GetGUIElement(this.gridColumnSignal, dataSourceIdx);
				this.pageValidator.Grid.StoreMapping(expSig.SignalName, gUIElement);
				this.pageValidator.Grid.StoreMapping(expSig.MessageName, gUIElement);
				this.pageValidator.Grid.StoreMapping(expSig.NetworkName, gUIElement);
				this.pageValidator.Grid.StoreMapping(expSig.DatabaseName, gUIElement);
				this.pageValidator.Grid.StoreMapping(expSig.DatabasePath, gUIElement);
			}
			if (PageValidatorGridUtil.IsColumnVisible(this.gridColumnChannel, this.gridViewExportList))
			{
				IValidatedGUIElement gUIElement = this.guiElementManager.GetGUIElement(this.gridColumnChannel, dataSourceIdx);
				this.pageValidator.Grid.StoreMapping(expSig.BusType, gUIElement);
				this.pageValidator.Grid.StoreMapping(expSig.ChannelNumber, gUIElement);
			}
			if (PageValidatorGridUtil.IsColumnVisible(this.gridColumnName, this.gridViewExportList))
			{
				IValidatedGUIElement gUIElement = this.guiElementManager.GetGUIElement(this.gridColumnName, dataSourceIdx);
				this.pageValidator.Grid.StoreMapping(expSig.Name, gUIElement);
			}
			if (PageValidatorGridUtil.IsColumnVisible(this.gridColumnComment, this.gridViewExportList))
			{
				IValidatedGUIElement gUIElement = this.guiElementManager.GetGUIElement(this.gridColumnComment, dataSourceIdx);
				this.pageValidator.Grid.StoreMapping(expSig.Comment, gUIElement);
			}
		}

		private void EnableControls()
		{
			bool @checked = this.checkBoxAllowSignalExport.Checked;
			this.checkBoxEnablePublic3GAccess.Enabled = @checked;
			IList<WebDisplayExportSignal> list = this.gridViewExportList.DataSource as IList<WebDisplayExportSignal>;
			this.splitButton.Enabled = (@checked && list != null && list.Count < 1000);
			this.buttonRemove.Enabled = (@checked && this.GetSelectedExportSignals().Count > 0);
			this.textBoxExportCycle.Enabled = @checked;
			this.gridControlSignalExportList.Enabled = @checked;
			this.checkBoxUseWebDisplay.Enabled = @checked;
			this.textBoxWebDisplayPath.Enabled = (@checked & this.checkBoxUseWebDisplay.Checked);
			this.buttonChooseWebDisplayPath.Enabled = (@checked & this.checkBoxUseWebDisplay.Checked);
		}

		public void SelectRowOfSignals(List<WebDisplayExportSignal> signalsToSelect)
		{
			this.gridViewExportList.ClearSelection();
			IList<WebDisplayExportSignal> list = this.gridViewExportList.DataSource as IList<WebDisplayExportSignal>;
			if (list == null)
			{
				return;
			}
			for (int i = 0; i < this.gridViewExportList.RowCount; i++)
			{
				WebDisplayExportSignal webDisplayExportSignal = list[i];
				foreach (WebDisplayExportSignal current in signalsToSelect)
				{
					if (webDisplayExportSignal == current)
					{
						this.RestoreFocusedRow(i);
						break;
					}
				}
			}
		}

		private List<int> StoreCurrentSelection()
		{
			int[] selectedRows = this.gridViewExportList.GetSelectedRows();
			List<int> list = new List<int>();
			int[] array = selectedRows;
			for (int i = 0; i < array.Length; i++)
			{
				int rowHandle = array[i];
				list.Add(this.gridViewExportList.GetDataSourceRowIndex(rowHandle));
			}
			return list;
		}

		private void RestoreSelection(List<int> selectedDataRows)
		{
			foreach (int current in selectedDataRows)
			{
				int rowHandle = this.gridViewExportList.GetRowHandle(current);
				if (current >= 0 && current < this.gridViewExportList.RowCount)
				{
					this.gridViewExportList.SelectRow(rowHandle);
				}
			}
		}

		private int StoreFocusedRow()
		{
			int focusedRowHandle = this.gridViewExportList.FocusedRowHandle;
			return this.gridViewExportList.GetDataSourceRowIndex(focusedRowHandle);
		}

		private void RestoreFocusedRow(int idx)
		{
			if (idx < 0)
			{
				return;
			}
			int rowHandle = this.gridViewExportList.GetRowHandle(idx);
			if (rowHandle >= 0 && rowHandle < this.gridViewExportList.RowCount)
			{
				this.gridViewExportList.FocusedRowHandle = rowHandle;
				this.gridViewExportList.SelectRow(rowHandle);
			}
		}

		private bool IsCellReadOnlyAtAll(WebDisplayExportSignal expSig, GridColumn column)
		{
			return this.GetSelectedExportSignals().Count > 1;
		}

		private bool IsCellReadOnlyByCellContent(WebDisplayExportSignal expSig, GridColumn column)
		{
			return column == this.gridColumnChannel && expSig.Type != WebDisplayExportSignalType.Signal;
		}

		private List<WebDisplayExportSignal> GetSelectedExportSignals()
		{
			List<WebDisplayExportSignal> list = new List<WebDisplayExportSignal>();
			int[] selectedRows = this.gridViewExportList.GetSelectedRows();
			for (int i = 0; i < selectedRows.Length; i++)
			{
				int rowHandle = selectedRows[i];
				list.Add(this.gridViewExportList.GetRow(rowHandle) as WebDisplayExportSignal);
			}
			return list;
		}

		private bool GetExportSignal(int listSourceRowIndex, out WebDisplayExportSignal expSig)
		{
			expSig = null;
			ReadOnlyCollection<WebDisplayExportSignal> readOnlyCollection = this.gridControlSignalExportList.DataSource as ReadOnlyCollection<WebDisplayExportSignal>;
			if (readOnlyCollection == null)
			{
				return false;
			}
			if (listSourceRowIndex < 0 || listSourceRowIndex > readOnlyCollection.Count - 1)
			{
				return false;
			}
			expSig = readOnlyCollection[listSourceRowIndex];
			return null != expSig;
		}

		private bool GetFocusedExportSignal(out WebDisplayExportSignal expSig)
		{
			expSig = null;
			ReadOnlyCollection<WebDisplayExportSignal> readOnlyCollection = this.gridControlSignalExportList.DataSource as ReadOnlyCollection<WebDisplayExportSignal>;
			if (readOnlyCollection == null)
			{
				return false;
			}
			int focusedRowHandle = this.gridViewExportList.FocusedRowHandle;
			int dataSourceRowIndex = this.gridViewExportList.GetDataSourceRowIndex(focusedRowHandle);
			if (focusedRowHandle < 0 || focusedRowHandle > readOnlyCollection.Count - 1)
			{
				return false;
			}
			expSig = readOnlyCollection[dataSourceRowIndex];
			return null != expSig;
		}

		private void UpdateGUI()
		{
			if (this.interfaceModeConfiguration == null)
			{
				return;
			}
			this.isInitControls = true;
			this.checkBoxAllowSignalExport.Checked = this.interfaceModeConfiguration.UseSignalExport.Value;
			this.UpdateDependentControls();
			this.EnableControls();
			this.isInitControls = false;
			this.ValidateInput(false);
		}

		private void UpdateDependentControls()
		{
			this.isInitControls = true;
			if (!this.pageValidator.General.HasFormatError(this.interfaceModeConfiguration.ExportCycle))
			{
				this.textBoxExportCycle.Text = this.interfaceModeConfiguration.ExportCycle.Value.ToString();
			}
			if (!this.pageValidator.General.HasFormatError(this.interfaceModeConfiguration.EnableAlwaysOnline))
			{
				this.checkBoxEnablePublic3GAccess.Checked = this.interfaceModeConfiguration.EnableAlwaysOnline.Value;
			}
			if (!this.pageValidator.General.HasFormatError(this.interfaceModeConfiguration.UseCustomWebDisplay))
			{
				this.checkBoxUseWebDisplay.Checked = this.interfaceModeConfiguration.UseCustomWebDisplay.Value;
			}
			if (!this.pageValidator.General.HasFormatError(this.interfaceModeConfiguration.CustomWebDisplay.FilePath))
			{
				this.textBoxWebDisplayPath.Text = this.interfaceModeConfiguration.CustomWebDisplay.FilePath.Value;
			}
			this.isInitControls = false;
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

		private void FillVariablesCombobox()
		{
			foreach (LtlSystemInformation ltlSystemInformation in Enum.GetValues(typeof(LtlSystemInformation)))
			{
				if (ltlSystemInformation != LtlSystemInformation.None)
				{
					this.repositoryItemComboBoxVariables.Items.Add(this.LtlSystemInformationToDisplayString(ltlSystemInformation));
				}
			}
		}

		private void ToggleSignalActiveState()
		{
			List<WebDisplayExportSignal> selectedExportSignals = this.GetSelectedExportSignals();
			bool flag = true;
			foreach (WebDisplayExportSignal current in selectedExportSignals)
			{
				flag &= current.IsActive.Value;
			}
			if (flag)
			{
				using (List<WebDisplayExportSignal>.Enumerator enumerator2 = selectedExportSignals.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						WebDisplayExportSignal current2 = enumerator2.Current;
						current2.IsActive.Value = false;
					}
					goto IL_CB;
				}
			}
			foreach (WebDisplayExportSignal current3 in selectedExportSignals)
			{
				current3.IsActive.Value = true;
			}
			IL_CB:
			this.gridViewExportList.RefreshData();
			this.ValidateInput(true);
		}

		private string LtlSystemInformationToDisplayString(LtlSystemInformation ltlSystemInformation)
		{
			switch (ltlSystemInformation)
			{
			case LtlSystemInformation.Logger1Files:
				return Resources.LtlSystemInformationLogger1Files;
			case LtlSystemInformation.Logger2Files:
				return Resources.LtlSystemInformationLogger2Files;
			case LtlSystemInformation.LoggerFilesTotal:
				return Resources.LtlSystemInformationLoggerFilesTotal;
			case LtlSystemInformation.LoggerMBsFree:
				return Resources.LtlSystemInformationLoggerMBsFree;
			case LtlSystemInformation.Stopped1:
				return Resources.LtlSystemInformationStopped1;
			case LtlSystemInformation.Stopped2:
				return Resources.LtlSystemInformationStopped2;
			case LtlSystemInformation.NotStopped1:
				return Resources.LtlSystemInformationNotStopped1;
			case LtlSystemInformation.NotStopped2:
				return Resources.LtlSystemInformationNotStopped2;
			case LtlSystemInformation.FlashFull:
				return Resources.LtlSystemInformationFlashFull;
			default:
				return string.Empty;
			}
		}

		private LtlSystemInformation DisplayStringToLtlSystemInformation(string ltlSystemInformation)
		{
			foreach (LtlSystemInformation ltlSystemInformation2 in Enum.GetValues(typeof(LtlSystemInformation)))
			{
				if (ltlSystemInformation == this.LtlSystemInformationToDisplayString(ltlSystemInformation2))
				{
					return ltlSystemInformation2;
				}
			}
			return LtlSystemInformation.None;
		}

		private string GetLtlSystemInformationDefaultName(LtlSystemInformation ltlSystemInformation)
		{
			switch (ltlSystemInformation)
			{
			case LtlSystemInformation.Logger1Files:
				return Resources.LtlSystemInformationDefaultNameLogger1Files;
			case LtlSystemInformation.Logger2Files:
				return Resources.LtlSystemInformationDefaultNameLogger2Files;
			case LtlSystemInformation.LoggerFilesTotal:
				return Resources.LtlSystemInformationDefaultNameLoggerFilesTotal;
			case LtlSystemInformation.LoggerMBsFree:
				return Resources.LtlSystemInformationDefaultNameLoggerMBsFree;
			case LtlSystemInformation.Stopped1:
				return Resources.LtlSystemInformationDefaultNameStopped1;
			case LtlSystemInformation.Stopped2:
				return Resources.LtlSystemInformationDefaultNameStopped2;
			case LtlSystemInformation.NotStopped1:
				return Resources.LtlSystemInformationDefaultNameNotStopped1;
			case LtlSystemInformation.NotStopped2:
				return Resources.LtlSystemInformationDefaultNameNotStopped2;
			case LtlSystemInformation.FlashFull:
				return Resources.LtlSystemInformationDefaultNameFlashFull;
			default:
				return Resources.LtlSystemInformationDefaultName;
			}
		}

		public bool IsItemVisible(ToolStripItem item)
		{
			string text = item.Text;
			return text == Resources_Trigger.TriggerTypeNameColSymbolicSigCAN || text == Resources_Trigger.TriggerTypeNameColSymbolicSigLIN || (text == Resources_Trigger.TriggerTypeNameColSymbolicSigFlexray && this.ModelValidator.LoggerSpecifics.Type == LoggerType.GL4000) || text == Resources.WebDisplayExportSignalTypeNameColLtlSystemInformation;
		}

		public void ItemClicked(ToolStripItem item)
		{
			this.AddItem(item.Text);
		}

		public void DefaultActionClicked()
		{
			this.AddItem(this.splitButtonEx.DefaultAction);
		}

		private void AddItem(string itemText)
		{
			if (itemText == Resources_Trigger.TriggerTypeNameColSymbolicSigCAN)
			{
				this.AddSymbolicSignal(BusType.Bt_CAN);
				return;
			}
			if (itemText == Resources_Trigger.TriggerTypeNameColSymbolicSigLIN)
			{
				this.AddSymbolicSignal(BusType.Bt_LIN);
				return;
			}
			if (itemText == Resources_Trigger.TriggerTypeNameColSymbolicSigFlexray)
			{
				this.AddSymbolicSignal(BusType.Bt_FlexRay);
				return;
			}
			if (itemText == Resources.WebDisplayExportSignalTypeNameColLtlSystemInformation)
			{
				this.AddLtlSystemInformation();
			}
		}

		public bool Serialize(SignalExportListPage signalExportListPage)
		{
			if (signalExportListPage == null)
			{
				return false;
			}
			bool result = true;
			MemoryStream memoryStream = null;
			try
			{
				memoryStream = new MemoryStream();
				this.gridViewExportList.SaveLayoutToStream(memoryStream);
				memoryStream.Seek(0L, SeekOrigin.Begin);
				byte[] array = new byte[memoryStream.Length];
				memoryStream.Read(array, 0, (int)memoryStream.Length);
				signalExportListPage.SignalExportGridLayout = Convert.ToBase64String(array, 0, array.Length);
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

		public bool DeSerialize(SignalExportListPage signalExportListPage)
		{
			if (signalExportListPage == null)
			{
				return false;
			}
			if (string.IsNullOrEmpty(signalExportListPage.SignalExportGridLayout))
			{
				return false;
			}
			bool result = true;
			MemoryStream memoryStream = null;
			try
			{
				byte[] buffer = Convert.FromBase64String(signalExportListPage.SignalExportGridLayout);
				memoryStream = new MemoryStream(buffer);
				this.gridViewExportList.RestoreLayoutFromStream(memoryStream);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(SignalExportList));
			this.labelAddSignal = new Label();
			this.splitButton = new SplitButton();
			this.buttonRemove = new Button();
			this.gridControlSignalExportList = new GridControl();
			this.gridViewExportList = new GridView();
			this.gridColumnActive = new GridColumn();
			this.repositoryItemCheckEditIsEnabled = new RepositoryItemCheckEdit();
			this.gridColumnIcon = new GridColumn();
			this.gridColumnName = new GridColumn();
			this.gridColumnSignal = new GridColumn();
			this.repositoryItemButtonSignal = new RepositoryItemButtonEdit();
			this.gridColumnChannel = new GridColumn();
			this.repositoryItemComboBoxChannel = new RepositoryItemComboBox();
			this.gridColumnComment = new GridColumn();
			this.repositoryItemTextEditDummy = new RepositoryItemTextEdit();
			this.repositoryItemComboBoxVariables = new RepositoryItemComboBox();
			this.xtraToolTipController = new XtraToolTipController(this.components);
			this.checkBoxEnablePublic3GAccess = new CheckBox();
			this.checkBoxAllowSignalExport = new CheckBox();
			this.textBoxExportCycle = new TextBox();
			this.textBoxWebDisplayPath = new TextBox();
			this.label1 = new Label();
			this.checkBoxUseWebDisplay = new CheckBox();
			this.buttonChooseWebDisplayPath = new Button();
			this.errorProviderFormat = new ErrorProvider(this.components);
			this.errorProviderLocalModel = new ErrorProvider(this.components);
			this.errorProviderGlobalModel = new ErrorProvider(this.components);
			((ISupportInitialize)this.gridControlSignalExportList).BeginInit();
			((ISupportInitialize)this.gridViewExportList).BeginInit();
			((ISupportInitialize)this.repositoryItemCheckEditIsEnabled).BeginInit();
			((ISupportInitialize)this.repositoryItemButtonSignal).BeginInit();
			((ISupportInitialize)this.repositoryItemComboBoxChannel).BeginInit();
			((ISupportInitialize)this.repositoryItemTextEditDummy).BeginInit();
			((ISupportInitialize)this.repositoryItemComboBoxVariables).BeginInit();
			((ISupportInitialize)this.errorProviderFormat).BeginInit();
			((ISupportInitialize)this.errorProviderLocalModel).BeginInit();
			((ISupportInitialize)this.errorProviderGlobalModel).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.labelAddSignal, "labelAddSignal");
			this.labelAddSignal.Name = "labelAddSignal";
			componentResourceManager.ApplyResources(this.splitButton, "splitButton");
			this.splitButton.Name = "splitButton";
			this.splitButton.ShowSplitAlways = true;
			this.splitButton.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.buttonRemove, "buttonRemove");
			this.buttonRemove.Image = Resources.ImageDelete;
			this.buttonRemove.Name = "buttonRemove";
			this.buttonRemove.UseVisualStyleBackColor = true;
			this.buttonRemove.Click += new EventHandler(this.buttonRemove_Click);
			componentResourceManager.ApplyResources(this.gridControlSignalExportList, "gridControlSignalExportList");
			this.gridControlSignalExportList.LookAndFeel.Style = LookAndFeelStyle.Style3D;
			this.gridControlSignalExportList.LookAndFeel.UseDefaultLookAndFeel = false;
			this.gridControlSignalExportList.LookAndFeel.UseWindowsXPTheme = true;
			this.gridControlSignalExportList.MainView = this.gridViewExportList;
			this.gridControlSignalExportList.Name = "gridControlSignalExportList";
			this.gridControlSignalExportList.RepositoryItems.AddRange(new RepositoryItem[]
			{
				this.repositoryItemComboBoxChannel,
				this.repositoryItemCheckEditIsEnabled,
				this.repositoryItemButtonSignal,
				this.repositoryItemTextEditDummy,
				this.repositoryItemComboBoxVariables
			});
			this.gridControlSignalExportList.ToolTipController = this.xtraToolTipController;
			this.gridControlSignalExportList.ViewCollection.AddRange(new BaseView[]
			{
				this.gridViewExportList
			});
			this.gridControlSignalExportList.Load += new EventHandler(this.gridControlSignalExportList_Load);
			this.gridControlSignalExportList.VisibleChanged += new EventHandler(this.gridControlSignalExportList_VisibleChanged);
			this.gridControlSignalExportList.Resize += new EventHandler(this.gridControlSignalExportList_Resize);
			this.gridViewExportList.Columns.AddRange(new GridColumn[]
			{
				this.gridColumnActive,
				this.gridColumnIcon,
				this.gridColumnName,
				this.gridColumnSignal,
				this.gridColumnChannel,
				this.gridColumnComment
			});
			this.gridViewExportList.GridControl = this.gridControlSignalExportList;
			this.gridViewExportList.Name = "gridViewExportList";
			this.gridViewExportList.OptionsBehavior.AllowAddRows = DefaultBoolean.False;
			this.gridViewExportList.OptionsBehavior.AllowDeleteRows = DefaultBoolean.False;
			this.gridViewExportList.OptionsBehavior.AutoSelectAllInEditor = false;
			this.gridViewExportList.OptionsBehavior.CacheValuesOnRowUpdating = CacheRowValuesMode.Disabled;
			this.gridViewExportList.OptionsBehavior.EditorShowMode = EditorShowMode.MouseUp;
			this.gridViewExportList.OptionsBehavior.ImmediateUpdateRowPosition = false;
			this.gridViewExportList.OptionsCustomization.AllowFilter = false;
			this.gridViewExportList.OptionsCustomization.AllowGroup = false;
			this.gridViewExportList.OptionsCustomization.AllowQuickHideColumns = false;
			this.gridViewExportList.OptionsDetail.EnableMasterViewMode = false;
			this.gridViewExportList.OptionsFilter.AllowFilterEditor = false;
			this.gridViewExportList.OptionsFind.AllowFindPanel = false;
			this.gridViewExportList.OptionsNavigation.UseTabKey = false;
			this.gridViewExportList.OptionsSelection.MultiSelect = true;
			this.gridViewExportList.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.Never;
			this.gridViewExportList.OptionsView.ShowGroupPanel = false;
			this.gridViewExportList.OptionsView.ShowIndicator = false;
			this.gridViewExportList.CustomDrawCell += new RowCellCustomDrawEventHandler(this.gridViewExportList_CustomDrawCell);
			this.gridViewExportList.CustomRowCellEdit += new CustomRowCellEditEventHandler(this.gridViewExportList_CustomRowCellEdit);
			this.gridViewExportList.LeftCoordChanged += new EventHandler(this.gridViewExportList_LeftCoordChanged);
			this.gridViewExportList.TopRowChanged += new EventHandler(this.gridViewExportList_TopRowChanged);
			this.gridViewExportList.PopupMenuShowing += new PopupMenuShowingEventHandler(this.gridViewExportList_PopupMenuShowing);
			this.gridViewExportList.SelectionChanged += new SelectionChangedEventHandler(this.gridViewExportList_SelectionChanged);
			this.gridViewExportList.ShowingEditor += new CancelEventHandler(this.gridViewExportList_ShowingEditor);
			this.gridViewExportList.ShownEditor += new EventHandler(this.gridViewExportList_ShownEditor);
			this.gridViewExportList.FocusedRowChanged += new FocusedRowChangedEventHandler(this.gridViewExportList_FocusedRowChanged);
			this.gridViewExportList.CustomUnboundColumnData += new CustomColumnDataEventHandler(this.gridViewExportList_CustomUnboundColumnData);
			this.gridViewExportList.KeyDown += new KeyEventHandler(this.gridViewExportList_KeyDown);
			this.gridViewExportList.MouseDown += new MouseEventHandler(this.gridViewExportList_MouseDown);
			componentResourceManager.ApplyResources(this.gridColumnActive, "gridColumnActive");
			this.gridColumnActive.ColumnEdit = this.repositoryItemCheckEditIsEnabled;
			this.gridColumnActive.FieldName = "gridColumnActive";
			this.gridColumnActive.Name = "gridColumnActive";
			this.gridColumnActive.OptionsColumn.AllowMove = false;
			this.gridColumnActive.OptionsColumn.FixedWidth = true;
			this.gridColumnActive.OptionsColumn.ShowCaption = false;
			this.gridColumnActive.UnboundType = UnboundColumnType.Boolean;
			componentResourceManager.ApplyResources(this.repositoryItemCheckEditIsEnabled, "repositoryItemCheckEditIsEnabled");
			this.repositoryItemCheckEditIsEnabled.Name = "repositoryItemCheckEditIsEnabled";
			this.repositoryItemCheckEditIsEnabled.CheckedChanged += new EventHandler(this.repositoryItemCheckEditIsEnabled_CheckedChanged);
			componentResourceManager.ApplyResources(this.gridColumnIcon, "gridColumnIcon");
			this.gridColumnIcon.Name = "gridColumnIcon";
			this.gridColumnIcon.OptionsColumn.AllowEdit = false;
			this.gridColumnIcon.OptionsColumn.AllowFocus = false;
			this.gridColumnIcon.OptionsColumn.AllowGroup = DefaultBoolean.False;
			this.gridColumnIcon.OptionsColumn.AllowIncrementalSearch = false;
			this.gridColumnIcon.OptionsColumn.AllowMerge = DefaultBoolean.False;
			this.gridColumnIcon.OptionsColumn.AllowMove = false;
			this.gridColumnIcon.OptionsColumn.AllowSize = false;
			this.gridColumnIcon.OptionsColumn.AllowSort = DefaultBoolean.False;
			this.gridColumnIcon.OptionsColumn.FixedWidth = true;
			this.gridColumnIcon.OptionsColumn.ReadOnly = true;
			this.gridColumnIcon.OptionsColumn.ShowCaption = false;
			this.gridColumnIcon.OptionsColumn.TabStop = false;
			componentResourceManager.ApplyResources(this.gridColumnName, "gridColumnName");
			this.gridColumnName.FieldName = "gridColumnName";
			this.gridColumnName.Name = "gridColumnName";
			this.gridColumnName.UnboundType = UnboundColumnType.String;
			componentResourceManager.ApplyResources(this.gridColumnSignal, "gridColumnSignal");
			this.gridColumnSignal.ColumnEdit = this.repositoryItemButtonSignal;
			this.gridColumnSignal.FieldName = "gridColumnSignal";
			this.gridColumnSignal.Name = "gridColumnSignal";
			this.gridColumnSignal.UnboundType = UnboundColumnType.String;
			componentResourceManager.ApplyResources(this.repositoryItemButtonSignal, "repositoryItemButtonSignal");
			this.repositoryItemButtonSignal.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton()
			});
			this.repositoryItemButtonSignal.Name = "repositoryItemButtonSignal";
			this.repositoryItemButtonSignal.TextEditStyle = TextEditStyles.DisableTextEditor;
			this.repositoryItemButtonSignal.ButtonClick += new ButtonPressedEventHandler(this.repositoryItemButtonSignal_ButtonClick);
			componentResourceManager.ApplyResources(this.gridColumnChannel, "gridColumnChannel");
			this.gridColumnChannel.ColumnEdit = this.repositoryItemComboBoxChannel;
			this.gridColumnChannel.FieldName = "gridColumnChannel";
			this.gridColumnChannel.Name = "gridColumnChannel";
			this.gridColumnChannel.OptionsColumn.FixedWidth = true;
			this.gridColumnChannel.UnboundType = UnboundColumnType.String;
			componentResourceManager.ApplyResources(this.repositoryItemComboBoxChannel, "repositoryItemComboBoxChannel");
			this.repositoryItemComboBoxChannel.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton((ButtonPredefines)componentResourceManager.GetObject("repositoryItemComboBoxChannel.Buttons"))
			});
			this.repositoryItemComboBoxChannel.Name = "repositoryItemComboBoxChannel";
			this.repositoryItemComboBoxChannel.TextEditStyle = TextEditStyles.DisableTextEditor;
			this.repositoryItemComboBoxChannel.SelectedIndexChanged += new EventHandler(this.repositoryItemComboBox_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.gridColumnComment, "gridColumnComment");
			this.gridColumnComment.FieldName = "gridColumnComment";
			this.gridColumnComment.Name = "gridColumnComment";
			this.gridColumnComment.UnboundType = UnboundColumnType.String;
			componentResourceManager.ApplyResources(this.repositoryItemTextEditDummy, "repositoryItemTextEditDummy");
			this.repositoryItemTextEditDummy.Name = "repositoryItemTextEditDummy";
			this.repositoryItemTextEditDummy.ReadOnly = true;
			componentResourceManager.ApplyResources(this.repositoryItemComboBoxVariables, "repositoryItemComboBoxVariables");
			this.repositoryItemComboBoxVariables.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton((ButtonPredefines)componentResourceManager.GetObject("repositoryItemComboBoxVariables.Buttons"))
			});
			this.repositoryItemComboBoxVariables.Name = "repositoryItemComboBoxVariables";
			this.repositoryItemComboBoxVariables.TextEditStyle = TextEditStyles.DisableTextEditor;
			this.repositoryItemComboBoxVariables.SelectedIndexChanged += new EventHandler(this.repositoryItemComboBoxVariables_SelectedIndexChanged);
			this.xtraToolTipController.Appearance.Options.UseBackColor = true;
			this.xtraToolTipController.Appearance.Options.UseForeColor = true;
			this.xtraToolTipController.Appearance.Options.UseTextOptions = true;
			this.xtraToolTipController.Appearance.TextOptions.HotkeyPrefix = HKeyPrefix.None;
			this.xtraToolTipController.AppearanceTitle.Options.UseBackColor = true;
			this.xtraToolTipController.AppearanceTitle.Options.UseForeColor = true;
			this.xtraToolTipController.AppearanceTitle.Options.UseTextOptions = true;
			this.xtraToolTipController.AppearanceTitle.TextOptions.HotkeyPrefix = HKeyPrefix.None;
			this.xtraToolTipController.MaxWidth = 500;
			this.xtraToolTipController.ShowPrefix = false;
			this.xtraToolTipController.UseNativeLookAndFeel = true;
			componentResourceManager.ApplyResources(this.checkBoxEnablePublic3GAccess, "checkBoxEnablePublic3GAccess");
			this.checkBoxEnablePublic3GAccess.Name = "checkBoxEnablePublic3GAccess";
			this.checkBoxEnablePublic3GAccess.UseVisualStyleBackColor = true;
			this.checkBoxEnablePublic3GAccess.CheckedChanged += new EventHandler(this.checkBoxEnablePublic3GAccess_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxAllowSignalExport, "checkBoxAllowSignalExport");
			this.checkBoxAllowSignalExport.Name = "checkBoxAllowSignalExport";
			this.checkBoxAllowSignalExport.UseVisualStyleBackColor = true;
			this.checkBoxAllowSignalExport.CheckedChanged += new EventHandler(this.checkBoxAllowSignalSamplingMode_CheckedChanged);
			componentResourceManager.ApplyResources(this.textBoxExportCycle, "textBoxExportCycle");
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxExportCycle, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxExportCycle.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxExportCycle, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxExportCycle.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxExportCycle, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxExportCycle.IconAlignment2"));
			this.textBoxExportCycle.Name = "textBoxExportCycle";
			this.textBoxExportCycle.Validating += new CancelEventHandler(this.textBoxExportCycle_Validating);
			this.textBoxWebDisplayPath.AllowDrop = true;
			componentResourceManager.ApplyResources(this.textBoxWebDisplayPath, "textBoxWebDisplayPath");
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxWebDisplayPath, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxWebDisplayPath.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxWebDisplayPath, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxWebDisplayPath.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxWebDisplayPath, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxWebDisplayPath.IconAlignment2"));
			this.textBoxWebDisplayPath.Name = "textBoxWebDisplayPath";
			this.textBoxWebDisplayPath.DragDrop += new DragEventHandler(this.textBoxWebDisplayPath_DragDrop);
			this.textBoxWebDisplayPath.DragEnter += new DragEventHandler(this.textBoxWebDisplayPath_DragEnter);
			this.textBoxWebDisplayPath.Validating += new CancelEventHandler(this.textBoxWebDisplayPath_Validating);
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.checkBoxUseWebDisplay, "checkBoxUseWebDisplay");
			this.checkBoxUseWebDisplay.Name = "checkBoxUseWebDisplay";
			this.checkBoxUseWebDisplay.UseVisualStyleBackColor = true;
			this.checkBoxUseWebDisplay.CheckedChanged += new EventHandler(this.checkBoxUseWebDisplay_CheckedChanged);
			componentResourceManager.ApplyResources(this.buttonChooseWebDisplayPath, "buttonChooseWebDisplayPath");
			this.buttonChooseWebDisplayPath.Name = "buttonChooseWebDisplayPath";
			this.buttonChooseWebDisplayPath.UseVisualStyleBackColor = true;
			this.buttonChooseWebDisplayPath.Click += new EventHandler(this.buttonChooseWebDisplayPath_Click);
			this.errorProviderFormat.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderFormat.ContainerControl = this;
			this.errorProviderLocalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderLocalModel.ContainerControl = this;
			this.errorProviderGlobalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderGlobalModel.ContainerControl = this;
			componentResourceManager.ApplyResources(this.errorProviderGlobalModel, "errorProviderGlobalModel");
			base.AutoScaleMode = AutoScaleMode.Inherit;
			base.Controls.Add(this.labelAddSignal);
			base.Controls.Add(this.splitButton);
			base.Controls.Add(this.checkBoxUseWebDisplay);
			base.Controls.Add(this.buttonRemove);
			base.Controls.Add(this.buttonChooseWebDisplayPath);
			base.Controls.Add(this.gridControlSignalExportList);
			base.Controls.Add(this.textBoxWebDisplayPath);
			base.Controls.Add(this.textBoxExportCycle);
			base.Controls.Add(this.checkBoxEnablePublic3GAccess);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.checkBoxAllowSignalExport);
			base.Name = "SignalExportList";
			componentResourceManager.ApplyResources(this, "$this");
			((ISupportInitialize)this.gridControlSignalExportList).EndInit();
			((ISupportInitialize)this.gridViewExportList).EndInit();
			((ISupportInitialize)this.repositoryItemCheckEditIsEnabled).EndInit();
			((ISupportInitialize)this.repositoryItemButtonSignal).EndInit();
			((ISupportInitialize)this.repositoryItemComboBoxChannel).EndInit();
			((ISupportInitialize)this.repositoryItemTextEditDummy).EndInit();
			((ISupportInitialize)this.repositoryItemComboBoxVariables).EndInit();
			((ISupportInitialize)this.errorProviderFormat).EndInit();
			((ISupportInitialize)this.errorProviderLocalModel).EndInit();
			((ISupportInitialize)this.errorProviderGlobalModel).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
