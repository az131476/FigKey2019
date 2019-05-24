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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Vector.UtilityFunctions;
using Vector.UtilityFunctions.XtraGrid;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.GUI.Common.EventConditions;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;
using Vector.VLConfig.ValidationFramework;

namespace Vector.VLConfig.GUI.WLANSettingsPage
{
	public class DataTransferTriggerGrid : UserControl
	{
		private DataTransferTriggerConfiguration triggerConfiguration;

		private GUIElementManager_ControlGridTree guiElementManager;

		private CustomErrorProvider customErrorProvider;

		private PageValidator pageValidator;

		private KeyCondition keyConditionDialog;

		private ClockTimedCondition clockTimedConditionDialog;

		private GeneralService utilService;

		private IContainer components;

		private GridControl gridControlTransferTriggers;

		private GridView gridViewTransferTriggers;

		private GridColumn colActive;

		private RepositoryItemCheckEdit repositoryItemCheckEditIsActive;

		private GridColumn colEventType;

		private RepositoryItemComboBox repositoryItemComboBoxEvent;

		private GridColumn colCondition;

		private GridColumn colDataSelection;

		private GridColumn colTransferMode;

		private RepositoryItemButtonEdit repositoryItemButtonEditCondition;

		private RepositoryItemButtonEdit repositoryItemButtonEditDataSelection;

		private RepositoryItemComboBox repositoryItemComboBoxTransferMode;

		private BindingSource bindingSourceDataTransferTrigger;

		private XtraToolTipController toolTipController;

		private ErrorProvider errorProviderFormat;

		private ErrorProvider errorProviderLocalModel;

		private ErrorProvider errorProviderGlobalModel;

		private RepositoryItemComboBox repositoryItemComboBoxKeyCondition;

		private RepositoryItemTextEdit repositoryItemTextEditDummy;

		private RepositoryItemComboBox repositoryItemComboBoxRefRecordTriggerName;

		public event EventHandler SelectionChanged;

		public DataTransferTriggerConfiguration DataTransferTriggerConfiguration
		{
			get
			{
				return this.triggerConfiguration;
			}
			set
			{
				this.triggerConfiguration = value;
				if (value != null)
				{
					int focusedRowHandle = this.gridViewTransferTriggers.FocusedRowHandle;
					this.gridControlTransferTriggers.DataSource = this.triggerConfiguration.DataTransferTriggers;
					if (focusedRowHandle >= 0 && focusedRowHandle < this.gridViewTransferTriggers.RowCount)
					{
						this.gridViewTransferTriggers.FocusedRowHandle = focusedRowHandle;
						return;
					}
					this.gridViewTransferTriggers.FocusedRowHandle = -1;
				}
			}
		}

		public IModelValidator ModelValidator
		{
			get;
			set;
		}

		public DataTransferTriggerGrid()
		{
			this.InitializeComponent();
			this.guiElementManager = new GUIElementManager_ControlGridTree();
			this.customErrorProvider = new CustomErrorProvider(this.errorProviderFormat);
			this.customErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.LocalModelError, this.errorProviderLocalModel);
			this.customErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.GlobalModelError, this.errorProviderGlobalModel);
			this.pageValidator = new PageValidator(this.customErrorProvider);
			this.utilService = new GeneralService(this.gridViewTransferTriggers);
			this.utilService.InitAppearance();
		}

		public void Init()
		{
		}

		private void Raise_SelectionChanged(object sender, EventArgs e)
		{
			if (this.SelectionChanged != null)
			{
				this.SelectionChanged(sender, e);
			}
		}

		public void AddTrigger(DataTransferTrigger trigger)
		{
			this.triggerConfiguration.AddDataTransferTrigger(trigger);
			this.gridViewTransferTriggers.RefreshData();
			this.ValidateInput(true);
		}

		public bool RemoveTrigger()
		{
			DataTransferTrigger trigger;
			if (!this.TryGetSelectedTrigger(out trigger))
			{
				return false;
			}
			this.triggerConfiguration.RemoveDataTransferTrigger(trigger);
			this.gridViewTransferTriggers.RefreshData();
			this.ValidateInput(true);
			return true;
		}

		private void gridViewTransferTriggers_FocusedRowChanged(object sender, FocusedRowChangedEventArgs e)
		{
			this.Raise_SelectionChanged(this, EventArgs.Empty);
		}

		private void gridViewTransferTriggers_CustomUnboundColumnData(object sender, CustomColumnDataEventArgs e)
		{
			DataTransferTrigger trigger;
			if (!this.TryGetTrigger(e.ListSourceRowIndex, out trigger))
			{
				return;
			}
			if (e.Column == this.colActive)
			{
				this.UnboundColumnDataTriggerActive(trigger, e);
				return;
			}
			if (e.Column == this.colEventType)
			{
				this.UnboundColumnEventType(trigger, e);
				return;
			}
			if (e.Column == this.colCondition)
			{
				this.UnboundColumnDataCondition(trigger, e);
				return;
			}
			if (e.Column == this.colDataSelection)
			{
				this.UnboundColumnDataSelection(trigger, e);
				return;
			}
			if (e.Column == this.colTransferMode)
			{
				this.UnboundColumnTransferMode(trigger, e);
			}
		}

		private void UnboundColumnDataTriggerActive(DataTransferTrigger trigger, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				e.Value = trigger.IsActive.Value;
				return;
			}
			bool value = Convert.ToBoolean(e.Value);
			bool flag;
			this.pageValidator.Grid.UpdateModel<bool>(value, trigger.IsActive, out flag);
			if (flag)
			{
				this.ValidateInput(true);
			}
		}

		private void UnboundColumnEventType(DataTransferTrigger trigger, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				if (trigger.Event is KeyEvent)
				{
					e.Value = Resources_Trigger.TriggerTypeNameColKey;
					return;
				}
				if (trigger.Event is ClockTimedEvent)
				{
					e.Value = Resources_Trigger.TriggerTypeNameColClockTime;
					return;
				}
				if (trigger.Event is NextLogSessionStartEvent)
				{
					e.Value = Resources_Trigger.TriggerTypeNameColStartNextLogSession;
					return;
				}
				if (trigger.Event is OnShutdownEvent)
				{
					e.Value = Resources_Trigger.TriggerTypeNamesColShutdown;
					return;
				}
				if (trigger.Event is ReferencedRecordTriggerNameEvent)
				{
					e.Value = Resources_Trigger.TriggerTypeNameColRefLoggingTrigger;
					return;
				}
			}
			else
			{
				bool flag = false;
				if (flag)
				{
					this.ValidateInput(true);
				}
			}
		}

		private void UnboundColumnDataCondition(DataTransferTrigger trigger, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				e.Value = this.GetUnboundColumnDataConditionString(trigger);
				return;
			}
			if (e.IsSetData)
			{
				if (trigger.Event is KeyEvent)
				{
					bool flag = false;
					bool value = false;
					uint value2 = GUIUtil.MapStringToKeyNumber(e.Value.ToString(), out value);
					bool flag2;
					this.pageValidator.Grid.UpdateModel<uint>(value2, (trigger.Event as KeyEvent).Number, out flag2);
					this.pageValidator.Grid.UpdateModel<bool>(value, (trigger.Event as KeyEvent).IsOnPanel, out flag);
					if (flag2 || flag)
					{
						this.ValidateInput(true);
						return;
					}
				}
				else if (trigger.Event is ReferencedRecordTriggerNameEvent)
				{
					bool flag3 = false;
					string text = e.Value.ToString();
					if (this.ModelValidator.LoggerSpecifics.DataStorage.NumberOfMemories > 1u)
					{
						for (uint num = 1u; num <= this.ModelValidator.LoggerSpecifics.DataStorage.NumberOfMemories; num += 1u)
						{
							if (string.Compare(text, string.Format(Resources_Trigger.AllLoggingTriggersOnMem, num)) == 0)
							{
								text = string.Format(ReferencedRecordTriggerNameEvent.WildcardTriggerNameOnMemory, num);
							}
						}
					}
					else if (string.Compare(text, Resources_Trigger.AllLoggingTriggers) == 0)
					{
						text = string.Format(ReferencedRecordTriggerNameEvent.WildcardTriggerNameOnMemory, 1);
					}
					ReferencedRecordTriggerNameEvent referencedRecordTriggerNameEvent = trigger.Event as ReferencedRecordTriggerNameEvent;
					this.pageValidator.Grid.UpdateModel<string>(text, referencedRecordTriggerNameEvent.NameOfTrigger, out flag3);
					if (flag3)
					{
						IDictionary<ulong, string> uniqueIdAndNameOfActiveRecordTriggers = this.ModelValidator.GetUniqueIdAndNameOfActiveRecordTriggers();
						foreach (ulong current in uniqueIdAndNameOfActiveRecordTriggers.Keys)
						{
							if (uniqueIdAndNameOfActiveRecordTriggers[current] == text)
							{
								referencedRecordTriggerNameEvent.UniqueIdOfTrigger = current;
								break;
							}
						}
						this.ValidateInput(true);
					}
				}
			}
		}

		private string GetUnboundColumnDataConditionString(DataTransferTrigger trigger)
		{
			if (trigger.Event is KeyEvent)
			{
				return GUIUtil.MapKeyNumber2String((trigger.Event as KeyEvent).Number.Value, (trigger.Event as KeyEvent).IsOnPanel.Value);
			}
			if (trigger.Event is ClockTimedEvent)
			{
				return GUIUtil.MapEventCondition2String(trigger.Event as ClockTimedEvent);
			}
			if (!(trigger.Event is ReferencedRecordTriggerNameEvent))
			{
				return "";
			}
			string value = (trigger.Event as ReferencedRecordTriggerNameEvent).NameOfTrigger.Value;
			uint num;
			if (!ReferencedRecordTriggerNameEvent.IsWildcardTriggerNameOnMemory(value, out num))
			{
				return value;
			}
			if (this.ModelValidator.LoggerSpecifics.DataStorage.NumberOfMemories > 1u)
			{
				return string.Format(Resources_Trigger.AllLoggingTriggersOnMem, num);
			}
			return Resources_Trigger.AllLoggingTriggers;
		}

		private void UnboundColumnDataSelection(DataTransferTrigger trigger, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				StringBuilder stringBuilder = new StringBuilder();
				if (trigger.IsDownloadRingbufferEnabled.Value)
				{
					if (trigger.MemoriesToDownload.Value == 1u)
					{
						stringBuilder.Append(string.Format(Resources.MemoryName, 1));
					}
					else if (trigger.MemoriesToDownload.Value == 2u)
					{
						stringBuilder.Append(string.Format(Resources.MemoryName, 2));
					}
					else if (trigger.MemoriesToDownload.Value == 2147483647u)
					{
						stringBuilder.Append(string.Format(Resources.MemoryName, 1));
						for (uint num = 2u; num <= this.ModelValidator.LoggerSpecifics.DataStorage.NumberOfMemories; num += 1u)
						{
							stringBuilder.Append(" + " + num);
						}
					}
				}
				if (trigger.IsDownloadClassifEnabled.Value)
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append("; ");
					}
					stringBuilder.Append(Resources.Classifications);
				}
				if (trigger.IsDownloadDriveRecEnabled.Value)
				{
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append("; ");
					}
					stringBuilder.Append(Resources.DriveRecorder);
				}
				e.Value = stringBuilder.ToString();
				return;
			}
			bool arg_15C_0 = e.IsSetData;
		}

		private void UnboundColumnTransferMode(DataTransferTrigger trigger, CustomColumnDataEventArgs e)
		{
			if (e.IsGetData)
			{
				e.Value = GUIUtil.MapDataTransferMode2String(trigger.DataTransferMode.Value);
				return;
			}
			if (e.IsSetData)
			{
				bool flag;
				this.pageValidator.Grid.UpdateModel<DataTransferModeType>(GUIUtil.MapString2DataTransferMode(e.Value.ToString()), trigger.DataTransferMode, out flag);
				if (flag)
				{
					this.ValidateInput(true);
				}
			}
		}

		private void gridViewTransferTriggers_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
		{
			if (e.MenuType == GridMenuType.Column)
			{
				this.CustomizeColumnHeaderMenu(e);
			}
		}

		private void CustomizeColumnHeaderMenu(PopupMenuShowingEventArgs e)
		{
			string localizedString = GridLocalizer.Active.GetLocalizedString(GridStringId.MenuColumnGroupBox);
			string localizedString2 = GridLocalizer.Active.GetLocalizedString(GridStringId.MenuGroupPanelShow);
			string localizedString3 = GridLocalizer.Active.GetLocalizedString(GridStringId.MenuColumnFilter);
			string localizedString4 = GridLocalizer.Active.GetLocalizedString(GridStringId.MenuColumnFilterEditor);
			string localizedString5 = GridLocalizer.Active.GetLocalizedString(GridStringId.MenuColumnGroup);
			string localizedString6 = GridLocalizer.Active.GetLocalizedString(GridStringId.MenuColumnUnGroup);
			string localizedString7 = GridLocalizer.Active.GetLocalizedString(GridStringId.MenuColumnRemoveColumn);
			string localizedString8 = GridLocalizer.Active.GetLocalizedString(GridStringId.MenuColumnColumnCustomization);
			for (int i = e.Menu.Items.Count - 1; i >= 0; i--)
			{
				string caption = e.Menu.Items[i].Caption;
				if (localizedString5 == caption || localizedString6 == caption || caption.Contains(localizedString) || localizedString2 == caption || localizedString3 == caption || localizedString4 == caption || localizedString7 == caption || localizedString8 == caption)
				{
					e.Menu.Items.RemoveAt(i);
				}
			}
		}

		private void gridViewTransferTriggers_ShowingEditor(object sender, CancelEventArgs e)
		{
			DataTransferTrigger dataTransferTrigger;
			if (this.gridViewTransferTriggers.FocusedColumn == this.colCondition && this.TryGetSelectedTrigger(out dataTransferTrigger) && (dataTransferTrigger.Event is NextLogSessionStartEvent || dataTransferTrigger.Event is OnShutdownEvent))
			{
				e.Cancel = true;
			}
		}

		private void gridViewTransferTriggers_ShownEditor(object sender, EventArgs e)
		{
			if (this.gridViewTransferTriggers.FocusedColumn == this.colEventType)
			{
				this.ShownEditorEventType();
				return;
			}
			if (this.gridViewTransferTriggers.FocusedColumn == this.colTransferMode)
			{
				this.ShownEditorTransferMode();
				return;
			}
			DataTransferTrigger dataTransferTrigger;
			if (this.gridViewTransferTriggers.FocusedColumn == this.colCondition && this.TryGetSelectedTrigger(out dataTransferTrigger))
			{
				if (dataTransferTrigger.Event is KeyEvent)
				{
					this.ShownEditorKeyEvents();
					return;
				}
				if (dataTransferTrigger.Event is ReferencedRecordTriggerNameEvent)
				{
					this.ShownEditorRecordTriggerNames();
				}
			}
		}

		private void ShownEditorEventType()
		{
			ComboBoxEdit comboBoxEdit = this.gridViewTransferTriggers.ActiveEditor as ComboBoxEdit;
			if (comboBoxEdit == null)
			{
				return;
			}
			comboBoxEdit.Properties.Items.Clear();
			comboBoxEdit.Properties.Items.Add(Resources_Trigger.TriggerTypeNameColStartNextLogSession);
			comboBoxEdit.Properties.Items.Add(Resources_Trigger.TriggerTypeNamesColShutdown);
			comboBoxEdit.Properties.Items.Add(Resources_Trigger.TriggerTypeNameColKey);
			comboBoxEdit.Properties.Items.Add(Resources_Trigger.TriggerTypeNameColClockTime);
			comboBoxEdit.Properties.Items.Add(Resources_Trigger.TriggerTypeNameColRefLoggingTrigger);
		}

		private void ShownEditorTransferMode()
		{
			ComboBoxEdit comboBoxEdit = this.gridViewTransferTriggers.ActiveEditor as ComboBoxEdit;
			if (comboBoxEdit == null)
			{
				return;
			}
			comboBoxEdit.Properties.Items.Clear();
			DataTransferTrigger dataTransferTrigger = null;
			bool flag = true;
			bool flag2 = true;
			if (this.TryGetSelectedTrigger(out dataTransferTrigger))
			{
				if (dataTransferTrigger.Event is NextLogSessionStartEvent)
				{
					flag = false;
				}
				if (dataTransferTrigger.Event is OnShutdownEvent)
				{
					flag2 = false;
				}
			}
			if (flag)
			{
				comboBoxEdit.Properties.Items.Add(Resources.DataTransferModeStop);
			}
			if (flag2)
			{
				comboBoxEdit.Properties.Items.Add(Resources.DataTransferModeRecord);
			}
		}

		private void ShownEditorKeyEvents()
		{
			ComboBoxEdit comboBoxEdit = this.gridViewTransferTriggers.ActiveEditor as ComboBoxEdit;
			if (comboBoxEdit != null)
			{
				comboBoxEdit.Properties.Items.Clear();
				for (uint num = 1u; num <= this.ModelValidator.LoggerSpecifics.IO.NumberOfKeys; num += 1u)
				{
					comboBoxEdit.Properties.Items.Add(GUIUtil.MapKeyNumber2String(num, false));
				}
				for (uint num2 = 1u; num2 <= this.ModelValidator.LoggerSpecifics.IO.NumberOfPanelKeys; num2 += 1u)
				{
					comboBoxEdit.Properties.Items.Add(GUIUtil.MapKeyNumber2String(num2, true));
				}
				for (uint num3 = 1u + Constants.CasKeyOffset; num3 <= this.ModelValidator.LoggerSpecifics.IO.NumberOfCasKeys + Constants.CasKeyOffset; num3 += 1u)
				{
					comboBoxEdit.Properties.Items.Add(GUIUtil.MapKeyNumber2String(num3, false));
				}
			}
		}

		private void ShownEditorRecordTriggerNames()
		{
			ComboBoxEdit comboBoxEdit = this.gridViewTransferTriggers.ActiveEditor as ComboBoxEdit;
			if (comboBoxEdit != null)
			{
				comboBoxEdit.Properties.Items.Clear();
				IList<string> sortedNamesOfActiveRecordTriggers = this.ModelValidator.GetSortedNamesOfActiveRecordTriggers();
				foreach (string current in sortedNamesOfActiveRecordTriggers)
				{
					comboBoxEdit.Properties.Items.Add(current);
				}
				if (this.ModelValidator.LoggerSpecifics.DataStorage.NumberOfMemories > 1u)
				{
					for (uint num = 1u; num <= this.ModelValidator.LoggerSpecifics.DataStorage.NumberOfMemories; num += 1u)
					{
						comboBoxEdit.Properties.Items.Add(string.Format(Resources_Trigger.AllLoggingTriggersOnMem, num));
					}
					return;
				}
				comboBoxEdit.Properties.Items.Add(Resources_Trigger.AllLoggingTriggers);
			}
		}

		private void gridViewTransferTriggers_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
		{
			IValidatedGUIElement gUIElement = this.guiElementManager.GetGUIElement(e.Column, this.gridViewTransferTriggers.GetDataSourceRowIndex(e.RowHandle));
			this.customErrorProvider.Grid.DisplayError(gUIElement, e);
		}

		private void gridViewTransferTriggers_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs e)
		{
			DataTransferTrigger dataTransferTrigger;
			if (e.Column == this.colCondition && this.TryGetTrigger(this.gridViewTransferTriggers.GetDataSourceRowIndex(e.RowHandle), out dataTransferTrigger))
			{
				if (dataTransferTrigger.Event is KeyEvent)
				{
					e.RepositoryItem = this.repositoryItemComboBoxKeyCondition;
					return;
				}
				if (dataTransferTrigger.Event is ClockTimedEvent)
				{
					e.RepositoryItem = this.repositoryItemButtonEditCondition;
					return;
				}
				if (dataTransferTrigger.Event is ReferencedRecordTriggerNameEvent)
				{
					e.RepositoryItem = this.repositoryItemComboBoxRefRecordTriggerName;
					return;
				}
				e.RepositoryItem = this.repositoryItemTextEditDummy;
			}
		}

		private void gridViewTransferTriggers_RowStyle(object sender, RowStyleEventArgs e)
		{
			DataTransferTrigger dataTransferTrigger;
			if (this.TryGetTrigger(this.gridViewTransferTriggers.GetDataSourceRowIndex(e.RowHandle), out dataTransferTrigger) && !dataTransferTrigger.IsActive.Value)
			{
				e.Appearance.ForeColor = SystemColors.GrayText;
			}
		}

		private void repositoryItemCheckEditIsActive_CheckedChanged(object sender, EventArgs e)
		{
			this.gridViewTransferTriggers.PostEditor();
			this.gridViewTransferTriggers.CloseEditor();
		}

		private void repositoryItemComboBoxEvent_SelectedIndexChanged(object sender, EventArgs e)
		{
			DataTransferTrigger dataTransferTrigger;
			if (!this.TryGetSelectedTrigger(out dataTransferTrigger))
			{
				return;
			}
			ComboBoxEdit comboBoxEdit = sender as ComboBoxEdit;
			string a = (comboBoxEdit != null) ? comboBoxEdit.SelectedItem.ToString() : string.Empty;
			if (a == Resources_Trigger.TriggerTypeNameColStartNextLogSession && !(dataTransferTrigger.Event is NextLogSessionStartEvent))
			{
				if (InformMessageBox.Question(Resources.QuestionChangeEvTypeDiscardsCond) == DialogResult.Yes)
				{
					dataTransferTrigger.Event = new NextLogSessionStartEvent();
					dataTransferTrigger.DataTransferMode.Value = DataTransferModeType.RecordWhileDataTransfer;
					this.ValidateInput(true);
				}
			}
			else if (a == Resources_Trigger.TriggerTypeNamesColShutdown && !(dataTransferTrigger.Event is OnShutdownEvent))
			{
				if (InformMessageBox.Question(Resources.QuestionChangeEvTypeDiscardsCond) == DialogResult.Yes)
				{
					dataTransferTrigger.Event = new OnShutdownEvent();
					dataTransferTrigger.DataTransferMode.Value = DataTransferModeType.StopWhileDataTransfer;
					this.ValidateInput(true);
				}
			}
			else if (a == Resources_Trigger.TriggerTypeNameColClockTime && !(dataTransferTrigger.Event is ClockTimedEvent))
			{
				if (InformMessageBox.Question(Resources.QuestionChangeEvTypeDiscardsCond) == DialogResult.Yes)
				{
					dataTransferTrigger.Event = new ClockTimedEvent();
					this.ValidateInput(true);
				}
			}
			else if (a == Resources_Trigger.TriggerTypeNameColKey && !(dataTransferTrigger.Event is KeyEvent) && InformMessageBox.Question(Resources.QuestionChangeEvTypeDiscardsCond) == DialogResult.Yes)
			{
				dataTransferTrigger.Event = new KeyEvent();
				this.ValidateInput(true);
			}
			this.gridViewTransferTriggers.PostEditor();
			this.gridViewTransferTriggers.CloseEditor();
		}

		private void repositoryItemButtonEditCondition_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			int arg_0B_0 = this.gridViewTransferTriggers.FocusedRowHandle;
			DataTransferTrigger trigger;
			if (!this.TryGetSelectedTrigger(out trigger))
			{
				return;
			}
			if (this.EditTriggerCondition(trigger))
			{
				this.gridViewTransferTriggers.RefreshData();
				this.ValidateInput(true);
				this.gridViewTransferTriggers.PostEditor();
				this.gridViewTransferTriggers.CloseEditor();
			}
		}

		private void repositoryItemButtonEditDataSelection_ButtonClick(object sender, ButtonPressedEventArgs e)
		{
			DataTransferTrigger dataTransferTrigger;
			if (!this.TryGetSelectedTrigger(out dataTransferTrigger))
			{
				return;
			}
			using (DataSelection dataSelection = new DataSelection(this.ModelValidator))
			{
				dataSelection.DataTransferTrigger = dataTransferTrigger;
				if (dataSelection.ShowDialog() == DialogResult.OK)
				{
					dataTransferTrigger = dataSelection.DataTransferTrigger;
					this.gridViewTransferTriggers.RefreshData();
					this.ValidateInput(true);
					this.gridViewTransferTriggers.PostEditor();
					this.gridViewTransferTriggers.CloseEditor();
				}
			}
		}

		private void repositoryItemComboBoxTransferMode_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.gridViewTransferTriggers.PostEditor();
			this.gridViewTransferTriggers.CloseEditor();
		}

		private void repositoryItemComboBoxKeyCondition_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.gridViewTransferTriggers.PostEditor();
			this.gridViewTransferTriggers.CloseEditor();
		}

		private void repositoryItemComboBoxRefRecordTriggerName_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.gridViewTransferTriggers.PostEditor();
			this.gridViewTransferTriggers.CloseEditor();
		}

		private bool EditTriggerCondition(DataTransferTrigger trigger)
		{
			if (trigger.Event is KeyEvent)
			{
				return this.EditKeyCondition(trigger.Event as KeyEvent);
			}
			return trigger.Event is ClockTimedEvent && this.EditClockTimedCondition(trigger.Event as ClockTimedEvent);
		}

		private bool EditKeyCondition(KeyEvent keyEvent)
		{
			if (keyEvent == null)
			{
				return false;
			}
			KeyCondition keyCondition = this.GetKeyConditionDialog();
			keyCondition.KeyEvent = new KeyEvent(keyEvent);
			if (DialogResult.OK == keyCondition.ShowDialog() && !keyEvent.Equals(keyCondition.KeyEvent))
			{
				keyEvent.Assign(keyCondition.KeyEvent);
				return true;
			}
			return false;
		}

		private bool EditClockTimedCondition(ClockTimedEvent clockTimedEvent)
		{
			if (clockTimedEvent == null)
			{
				return false;
			}
			ClockTimedCondition clockTimedCondition = this.GetClockTimedConditionDialog();
			clockTimedCondition.ClockTimedEvent = new ClockTimedEvent(clockTimedEvent);
			if (DialogResult.OK == clockTimedCondition.ShowDialog() && !clockTimedEvent.Equals(clockTimedCondition.ClockTimedEvent))
			{
				clockTimedEvent.Assign(clockTimedCondition.ClockTimedEvent);
				return true;
			}
			return false;
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
			flag &= this.ModelValidator.ValidateThreeGDataTransferTriggers(this.triggerConfiguration, isDataChanged, this.pageValidator);
			this.pageValidator.General.ActivateErrorProvidersForFormatAndModelErrors();
			this.gridViewTransferTriggers.RefreshData();
			return flag;
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
			PageValidatorGridUtil.StoreRowMappingForVisibleRows(this.gridViewTransferTriggers, new PageValidatorGridUtil.StoreRowMappingHandler(this.OnPageValidatorGridUtilStoreRowMapping));
		}

		private void OnPageValidatorGridUtilStoreRowMapping(int dataSourceRowIdx)
		{
			if (dataSourceRowIdx < 0 || dataSourceRowIdx >= this.DataTransferTriggerConfiguration.DataTransferTriggers.Count)
			{
				return;
			}
			DataTransferTrigger trigger = this.DataTransferTriggerConfiguration.DataTransferTriggers[dataSourceRowIdx];
			this.StoreMapping4VisibleColumns(trigger, dataSourceRowIdx);
		}

		private void StoreMapping4VisibleColumns(DataTransferTrigger trigger, int dataSourceIdx)
		{
			if (PageValidatorGridUtil.IsColumnVisible(this.colActive, this.gridViewTransferTriggers))
			{
				IValidatedGUIElement gUIElement = this.guiElementManager.GetGUIElement(this.colActive, dataSourceIdx);
				this.pageValidator.Grid.StoreMapping(trigger.IsActive, gUIElement);
			}
			if (PageValidatorGridUtil.IsColumnVisible(this.colCondition, this.gridViewTransferTriggers))
			{
				IValidatedGUIElement gUIElement = this.guiElementManager.GetGUIElement(this.colCondition, dataSourceIdx);
				IEnumerable<IValidatedProperty> validatedPropertyListCondition = this.GetValidatedPropertyListCondition(trigger);
				foreach (IValidatedProperty current in validatedPropertyListCondition)
				{
					this.pageValidator.Grid.StoreMapping(current, gUIElement);
				}
			}
			if (PageValidatorGridUtil.IsColumnVisible(this.colDataSelection, this.gridViewTransferTriggers))
			{
				IValidatedGUIElement gUIElement = this.guiElementManager.GetGUIElement(this.colCondition, dataSourceIdx);
				this.pageValidator.Grid.StoreMapping(trigger.IsDownloadRingbufferEnabled, gUIElement);
				this.pageValidator.Grid.StoreMapping(trigger.MemoriesToDownload, gUIElement);
				this.pageValidator.Grid.StoreMapping(trigger.IsDownloadClassifEnabled, gUIElement);
				this.pageValidator.Grid.StoreMapping(trigger.IsDownloadDriveRecEnabled, gUIElement);
			}
		}

		private IEnumerable<IValidatedProperty> GetValidatedPropertyListCondition(DataTransferTrigger trigger)
		{
			List<IValidatedProperty> list = new List<IValidatedProperty>();
			if (trigger.Event is KeyEvent)
			{
				list.Add((trigger.Event as KeyEvent).Number);
				list.Add((trigger.Event as KeyEvent).IsOnPanel);
			}
			else if (trigger.Event is ClockTimedEvent)
			{
				list.Add((trigger.Event as ClockTimedEvent).StartTime);
				list.Add((trigger.Event as ClockTimedEvent).RepetitionInterval);
			}
			else if (trigger.Event is NextLogSessionStartEvent)
			{
				list.Add((trigger.Event as NextLogSessionStartEvent).Delay);
			}
			else if (trigger.Event is ReferencedRecordTriggerNameEvent)
			{
				list.Add((trigger.Event as ReferencedRecordTriggerNameEvent).NameOfTrigger);
			}
			return list;
		}

		private KeyCondition GetKeyConditionDialog()
		{
			if (this.keyConditionDialog == null)
			{
				this.keyConditionDialog = new KeyCondition(this.ModelValidator);
			}
			return this.keyConditionDialog;
		}

		private ClockTimedCondition GetClockTimedConditionDialog()
		{
			if (this.clockTimedConditionDialog == null)
			{
				this.clockTimedConditionDialog = new ClockTimedCondition();
			}
			return this.clockTimedConditionDialog;
		}

		public bool TryGetSelectedTrigger(out DataTransferTrigger trigger)
		{
			int num;
			return this.TryGetSelectedTrigger(out trigger, out num);
		}

		private bool TryGetSelectedTrigger(out DataTransferTrigger trigger, out int index)
		{
			trigger = null;
			index = this.gridViewTransferTriggers.GetFocusedDataSourceRowIndex();
			if (index < 0 || index > this.triggerConfiguration.DataTransferTriggers.Count - 1)
			{
				return false;
			}
			trigger = this.triggerConfiguration.DataTransferTriggers[index];
			return true;
		}

		private bool TryGetTrigger(int listSourceRowIndex, out DataTransferTrigger trigger)
		{
			trigger = null;
			if (this.DataTransferTriggerConfiguration == null)
			{
				return false;
			}
			if (listSourceRowIndex < 0 || listSourceRowIndex > this.DataTransferTriggerConfiguration.DataTransferTriggers.Count - 1)
			{
				return false;
			}
			trigger = this.DataTransferTriggerConfiguration.DataTransferTriggers[listSourceRowIndex];
			return trigger != null;
		}

		public bool Serialize<T>(T wlanSettingsPage)
		{
			if (wlanSettingsPage == null)
			{
				return false;
			}
			string empty = string.Empty;
			if (this.Serialize(ref empty))
			{
				if (wlanSettingsPage is WLANSettingsGL3PlusPage)
				{
					(wlanSettingsPage as WLANSettingsGL3PlusPage).ThreeGDataTransferTriggerGridLayout = empty;
				}
				if (wlanSettingsPage is WLANSettingsGL2000Page)
				{
					(wlanSettingsPage as WLANSettingsGL2000Page).ThreeGDataTransferTriggerGridLayout = empty;
				}
				return true;
			}
			return false;
		}

		private bool Serialize(ref string data)
		{
			bool result = true;
			MemoryStream memoryStream = null;
			try
			{
				memoryStream = new MemoryStream();
				this.gridViewTransferTriggers.SaveLayoutToStream(memoryStream);
				memoryStream.Seek(0L, SeekOrigin.Begin);
				byte[] array = new byte[memoryStream.Length];
				memoryStream.Read(array, 0, (int)memoryStream.Length);
				data = Convert.ToBase64String(array, 0, array.Length);
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

		public bool DeSerialize<T>(T wlanSettingsPage)
		{
			if (wlanSettingsPage == null)
			{
				return false;
			}
			string text = string.Empty;
			if (typeof(WLANSettingsGL3PlusPage) == typeof(T))
			{
				WLANSettingsGL3PlusPage wLANSettingsGL3PlusPage = wlanSettingsPage as WLANSettingsGL3PlusPage;
				if (wLANSettingsGL3PlusPage == null)
				{
					return false;
				}
				text = wLANSettingsGL3PlusPage.ThreeGDataTransferTriggerGridLayout;
			}
			if (typeof(WLANSettingsGL2000Page) == typeof(T))
			{
				WLANSettingsGL2000Page wLANSettingsGL2000Page = wlanSettingsPage as WLANSettingsGL2000Page;
				if (wLANSettingsGL2000Page == null)
				{
					return false;
				}
				text = wLANSettingsGL2000Page.ThreeGDataTransferTriggerGridLayout;
			}
			if (string.IsNullOrEmpty(text))
			{
				return false;
			}
			byte[] buffer = Convert.FromBase64String(text);
			return this.DeSerialize(buffer);
		}

		private bool DeSerialize(byte[] buffer)
		{
			bool result = true;
			MemoryStream memoryStream = null;
			try
			{
				memoryStream = new MemoryStream(buffer);
				this.gridViewTransferTriggers.RestoreLayoutFromStream(memoryStream);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(DataTransferTriggerGrid));
			this.gridControlTransferTriggers = new GridControl();
			this.bindingSourceDataTransferTrigger = new BindingSource(this.components);
			this.gridViewTransferTriggers = new GridView();
			this.colActive = new GridColumn();
			this.repositoryItemCheckEditIsActive = new RepositoryItemCheckEdit();
			this.colEventType = new GridColumn();
			this.repositoryItemComboBoxEvent = new RepositoryItemComboBox();
			this.colCondition = new GridColumn();
			this.repositoryItemButtonEditCondition = new RepositoryItemButtonEdit();
			this.colDataSelection = new GridColumn();
			this.repositoryItemButtonEditDataSelection = new RepositoryItemButtonEdit();
			this.colTransferMode = new GridColumn();
			this.repositoryItemComboBoxTransferMode = new RepositoryItemComboBox();
			this.repositoryItemComboBoxKeyCondition = new RepositoryItemComboBox();
			this.repositoryItemTextEditDummy = new RepositoryItemTextEdit();
			this.repositoryItemComboBoxRefRecordTriggerName = new RepositoryItemComboBox();
			this.toolTipController = new XtraToolTipController(this.components);
			this.errorProviderFormat = new ErrorProvider(this.components);
			this.errorProviderLocalModel = new ErrorProvider(this.components);
			this.errorProviderGlobalModel = new ErrorProvider(this.components);
			((ISupportInitialize)this.gridControlTransferTriggers).BeginInit();
			((ISupportInitialize)this.bindingSourceDataTransferTrigger).BeginInit();
			((ISupportInitialize)this.gridViewTransferTriggers).BeginInit();
			((ISupportInitialize)this.repositoryItemCheckEditIsActive).BeginInit();
			((ISupportInitialize)this.repositoryItemComboBoxEvent).BeginInit();
			((ISupportInitialize)this.repositoryItemButtonEditCondition).BeginInit();
			((ISupportInitialize)this.repositoryItemButtonEditDataSelection).BeginInit();
			((ISupportInitialize)this.repositoryItemComboBoxTransferMode).BeginInit();
			((ISupportInitialize)this.repositoryItemComboBoxKeyCondition).BeginInit();
			((ISupportInitialize)this.repositoryItemTextEditDummy).BeginInit();
			((ISupportInitialize)this.repositoryItemComboBoxRefRecordTriggerName).BeginInit();
			((ISupportInitialize)this.errorProviderFormat).BeginInit();
			((ISupportInitialize)this.errorProviderLocalModel).BeginInit();
			((ISupportInitialize)this.errorProviderGlobalModel).BeginInit();
			base.SuspendLayout();
			this.gridControlTransferTriggers.DataSource = this.bindingSourceDataTransferTrigger;
			componentResourceManager.ApplyResources(this.gridControlTransferTriggers, "gridControlTransferTriggers");
			this.gridControlTransferTriggers.MainView = this.gridViewTransferTriggers;
			this.gridControlTransferTriggers.Name = "gridControlTransferTriggers";
			this.gridControlTransferTriggers.RepositoryItems.AddRange(new RepositoryItem[]
			{
				this.repositoryItemCheckEditIsActive,
				this.repositoryItemComboBoxEvent,
				this.repositoryItemButtonEditCondition,
				this.repositoryItemButtonEditDataSelection,
				this.repositoryItemComboBoxTransferMode,
				this.repositoryItemComboBoxKeyCondition,
				this.repositoryItemTextEditDummy,
				this.repositoryItemComboBoxRefRecordTriggerName
			});
			this.gridControlTransferTriggers.ToolTipController = this.toolTipController;
			this.gridControlTransferTriggers.ViewCollection.AddRange(new BaseView[]
			{
				this.gridViewTransferTriggers
			});
			this.bindingSourceDataTransferTrigger.DataSource = typeof(DataTransferTrigger);
			this.gridViewTransferTriggers.Columns.AddRange(new GridColumn[]
			{
				this.colActive,
				this.colEventType,
				this.colCondition,
				this.colDataSelection,
				this.colTransferMode
			});
			this.gridViewTransferTriggers.GridControl = this.gridControlTransferTriggers;
			this.gridViewTransferTriggers.Name = "gridViewTransferTriggers";
			this.gridViewTransferTriggers.OptionsBehavior.EditorShowMode = EditorShowMode.MouseDown;
			this.gridViewTransferTriggers.OptionsCustomization.AllowFilter = false;
			this.gridViewTransferTriggers.OptionsCustomization.AllowGroup = false;
			this.gridViewTransferTriggers.OptionsCustomization.AllowQuickHideColumns = false;
			this.gridViewTransferTriggers.OptionsView.ShowGroupPanel = false;
			this.gridViewTransferTriggers.OptionsView.ShowIndicator = false;
			this.gridViewTransferTriggers.PaintStyleName = "WindowsXP";
			this.gridViewTransferTriggers.RowHeight = 20;
			this.gridViewTransferTriggers.CustomDrawCell += new RowCellCustomDrawEventHandler(this.gridViewTransferTriggers_CustomDrawCell);
			this.gridViewTransferTriggers.RowStyle += new RowStyleEventHandler(this.gridViewTransferTriggers_RowStyle);
			this.gridViewTransferTriggers.CustomRowCellEdit += new CustomRowCellEditEventHandler(this.gridViewTransferTriggers_CustomRowCellEdit);
			this.gridViewTransferTriggers.PopupMenuShowing += new PopupMenuShowingEventHandler(this.gridViewTransferTriggers_PopupMenuShowing);
			this.gridViewTransferTriggers.ShowingEditor += new CancelEventHandler(this.gridViewTransferTriggers_ShowingEditor);
			this.gridViewTransferTriggers.ShownEditor += new EventHandler(this.gridViewTransferTriggers_ShownEditor);
			this.gridViewTransferTriggers.FocusedRowChanged += new FocusedRowChangedEventHandler(this.gridViewTransferTriggers_FocusedRowChanged);
			this.gridViewTransferTriggers.CustomUnboundColumnData += new CustomColumnDataEventHandler(this.gridViewTransferTriggers_CustomUnboundColumnData);
			componentResourceManager.ApplyResources(this.colActive, "colActive");
			this.colActive.ColumnEdit = this.repositoryItemCheckEditIsActive;
			this.colActive.FieldName = "anyBoolean1";
			this.colActive.Name = "colActive";
			this.colActive.UnboundType = UnboundColumnType.Boolean;
			componentResourceManager.ApplyResources(this.repositoryItemCheckEditIsActive, "repositoryItemCheckEditIsActive");
			this.repositoryItemCheckEditIsActive.Name = "repositoryItemCheckEditIsActive";
			this.repositoryItemCheckEditIsActive.CheckedChanged += new EventHandler(this.repositoryItemCheckEditIsActive_CheckedChanged);
			componentResourceManager.ApplyResources(this.colEventType, "colEventType");
			this.colEventType.ColumnEdit = this.repositoryItemComboBoxEvent;
			this.colEventType.FieldName = "colEventType";
			this.colEventType.Name = "colEventType";
			this.colEventType.UnboundType = UnboundColumnType.String;
			componentResourceManager.ApplyResources(this.repositoryItemComboBoxEvent, "repositoryItemComboBoxEvent");
			this.repositoryItemComboBoxEvent.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton((ButtonPredefines)componentResourceManager.GetObject("repositoryItemComboBoxEvent.Buttons"))
			});
			this.repositoryItemComboBoxEvent.Name = "repositoryItemComboBoxEvent";
			this.repositoryItemComboBoxEvent.TextEditStyle = TextEditStyles.DisableTextEditor;
			this.repositoryItemComboBoxEvent.SelectedIndexChanged += new EventHandler(this.repositoryItemComboBoxEvent_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.colCondition, "colCondition");
			this.colCondition.ColumnEdit = this.repositoryItemButtonEditCondition;
			this.colCondition.FieldName = "colCondition";
			this.colCondition.Name = "colCondition";
			this.colCondition.UnboundType = UnboundColumnType.String;
			componentResourceManager.ApplyResources(this.repositoryItemButtonEditCondition, "repositoryItemButtonEditCondition");
			this.repositoryItemButtonEditCondition.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton()
			});
			this.repositoryItemButtonEditCondition.Name = "repositoryItemButtonEditCondition";
			this.repositoryItemButtonEditCondition.TextEditStyle = TextEditStyles.DisableTextEditor;
			this.repositoryItemButtonEditCondition.ButtonClick += new ButtonPressedEventHandler(this.repositoryItemButtonEditCondition_ButtonClick);
			componentResourceManager.ApplyResources(this.colDataSelection, "colDataSelection");
			this.colDataSelection.ColumnEdit = this.repositoryItemButtonEditDataSelection;
			this.colDataSelection.FieldName = "colDataSelection";
			this.colDataSelection.Name = "colDataSelection";
			this.colDataSelection.UnboundType = UnboundColumnType.String;
			componentResourceManager.ApplyResources(this.repositoryItemButtonEditDataSelection, "repositoryItemButtonEditDataSelection");
			this.repositoryItemButtonEditDataSelection.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton()
			});
			this.repositoryItemButtonEditDataSelection.Name = "repositoryItemButtonEditDataSelection";
			this.repositoryItemButtonEditDataSelection.TextEditStyle = TextEditStyles.DisableTextEditor;
			this.repositoryItemButtonEditDataSelection.ButtonClick += new ButtonPressedEventHandler(this.repositoryItemButtonEditDataSelection_ButtonClick);
			componentResourceManager.ApplyResources(this.colTransferMode, "colTransferMode");
			this.colTransferMode.ColumnEdit = this.repositoryItemComboBoxTransferMode;
			this.colTransferMode.FieldName = "colTransferMode";
			this.colTransferMode.Name = "colTransferMode";
			this.colTransferMode.UnboundType = UnboundColumnType.String;
			componentResourceManager.ApplyResources(this.repositoryItemComboBoxTransferMode, "repositoryItemComboBoxTransferMode");
			this.repositoryItemComboBoxTransferMode.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton((ButtonPredefines)componentResourceManager.GetObject("repositoryItemComboBoxTransferMode.Buttons"))
			});
			this.repositoryItemComboBoxTransferMode.Name = "repositoryItemComboBoxTransferMode";
			this.repositoryItemComboBoxTransferMode.TextEditStyle = TextEditStyles.DisableTextEditor;
			this.repositoryItemComboBoxTransferMode.SelectedIndexChanged += new EventHandler(this.repositoryItemComboBoxTransferMode_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.repositoryItemComboBoxKeyCondition, "repositoryItemComboBoxKeyCondition");
			this.repositoryItemComboBoxKeyCondition.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton((ButtonPredefines)componentResourceManager.GetObject("repositoryItemComboBoxKeyCondition.Buttons"))
			});
			this.repositoryItemComboBoxKeyCondition.Name = "repositoryItemComboBoxKeyCondition";
			this.repositoryItemComboBoxKeyCondition.TextEditStyle = TextEditStyles.DisableTextEditor;
			this.repositoryItemComboBoxKeyCondition.SelectedIndexChanged += new EventHandler(this.repositoryItemComboBoxKeyCondition_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.repositoryItemTextEditDummy, "repositoryItemTextEditDummy");
			this.repositoryItemTextEditDummy.Name = "repositoryItemTextEditDummy";
			this.repositoryItemTextEditDummy.ReadOnly = true;
			componentResourceManager.ApplyResources(this.repositoryItemComboBoxRefRecordTriggerName, "repositoryItemComboBoxRefRecordTriggerName");
			this.repositoryItemComboBoxRefRecordTriggerName.Buttons.AddRange(new EditorButton[]
			{
				new EditorButton((ButtonPredefines)componentResourceManager.GetObject("repositoryItemComboBoxRefRecordTriggerName.Buttons"))
			});
			this.repositoryItemComboBoxRefRecordTriggerName.Name = "repositoryItemComboBoxRefRecordTriggerName";
			this.repositoryItemComboBoxRefRecordTriggerName.TextEditStyle = TextEditStyles.DisableTextEditor;
			this.repositoryItemComboBoxRefRecordTriggerName.SelectedIndexChanged += new EventHandler(this.repositoryItemComboBoxRefRecordTriggerName_SelectedIndexChanged);
			this.toolTipController.Appearance.BackColor = (Color)componentResourceManager.GetObject("toolTipController.Appearance.BackColor");
			this.toolTipController.Appearance.ForeColor = (Color)componentResourceManager.GetObject("toolTipController.Appearance.ForeColor");
			this.toolTipController.Appearance.Options.UseBackColor = true;
			this.toolTipController.Appearance.Options.UseForeColor = true;
			this.toolTipController.Appearance.Options.UseTextOptions = true;
			this.toolTipController.Appearance.TextOptions.HotkeyPrefix = HKeyPrefix.Show;
			this.toolTipController.AppearanceTitle.BackColor = (Color)componentResourceManager.GetObject("toolTipController.AppearanceTitle.BackColor");
			this.toolTipController.AppearanceTitle.ForeColor = (Color)componentResourceManager.GetObject("toolTipController.AppearanceTitle.ForeColor");
			this.toolTipController.AppearanceTitle.Options.UseBackColor = true;
			this.toolTipController.AppearanceTitle.Options.UseForeColor = true;
			this.toolTipController.AppearanceTitle.Options.UseTextOptions = true;
			this.toolTipController.AppearanceTitle.TextOptions.HotkeyPrefix = HKeyPrefix.Show;
			this.toolTipController.MaxWidth = 500;
			this.toolTipController.ShowPrefix = true;
			this.toolTipController.UseNativeLookAndFeel = true;
			this.errorProviderFormat.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderFormat.ContainerControl = this;
			this.errorProviderLocalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderLocalModel.ContainerControl = this;
			this.errorProviderGlobalModel.ContainerControl = this;
			componentResourceManager.ApplyResources(this.errorProviderGlobalModel, "errorProviderGlobalModel");
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.gridControlTransferTriggers);
			base.Name = "DataTransferTriggerGrid";
			((ISupportInitialize)this.gridControlTransferTriggers).EndInit();
			((ISupportInitialize)this.bindingSourceDataTransferTrigger).EndInit();
			((ISupportInitialize)this.gridViewTransferTriggers).EndInit();
			((ISupportInitialize)this.repositoryItemCheckEditIsActive).EndInit();
			((ISupportInitialize)this.repositoryItemComboBoxEvent).EndInit();
			((ISupportInitialize)this.repositoryItemButtonEditCondition).EndInit();
			((ISupportInitialize)this.repositoryItemButtonEditDataSelection).EndInit();
			((ISupportInitialize)this.repositoryItemComboBoxTransferMode).EndInit();
			((ISupportInitialize)this.repositoryItemComboBoxKeyCondition).EndInit();
			((ISupportInitialize)this.repositoryItemTextEditDummy).EndInit();
			((ISupportInitialize)this.repositoryItemComboBoxRefRecordTriggerName).EndInit();
			((ISupportInitialize)this.errorProviderFormat).EndInit();
			((ISupportInitialize)this.errorProviderLocalModel).EndInit();
			((ISupportInitialize)this.errorProviderGlobalModel).EndInit();
			base.ResumeLayout(false);
		}
	}
}
