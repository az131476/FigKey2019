using System;
using System.ComponentModel;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;
using Vector.VLConfig.ValidationFramework;

namespace Vector.VLConfig.GUI.WLANSettingsPage
{
	public class WLANSettingsGL3Plus : UserControl
	{
		private GUIElementManager_Control guiElementManager;

		private CustomErrorProvider customErrorProvider;

		private PageValidator pageValidator;

		private WLANConfiguration wlanConfiguration;

		private DisplayMode displayMode;

		private bool isInitControls;

		private string oldWLANStartConnectionEventName;

		private IContainer components;

		private CheckBox checkBoxStartWLANor3GConnectOnShutdown;

		private ComboBox comboBoxAnalogInputNumber;

		private CheckBox checkBoxStartWLANConnectOn;

		private ComboBox comboBoxStartWLANConnectionOn;

		private CheckBox checkBoxWLANor3GDownloadRingbuffer;

		private CheckBox checkBoxWLANor3GDownloadClassification;

		private CheckBox checkBoxWLANor3GDownloadDriveRecorder;

		private ComboBox comboBoxWLANor3GDownloadRingbuffer;

		private ErrorProvider errorProviderFormat;

		private ErrorProvider errorProviderLocalModel;

		private ErrorProvider errorProviderGlobalModel;

		private ComboBox comboBoxWLANDownloadRingbuffer;

		private CheckBox checkBoxWLANDownloadRingbuffer;

		private CheckBox checkBoxStart3GConnectOn;

		private CheckBox checkBoxFallbackTo3G;

		private CheckBox checkBoxWLANDownloadDriveRecorder;

		private CheckBox checkBoxWLANDownloadClassification;

		private Label labelStartWLAN3GNote;

		private DataTransferTriggerGrid threeGDataTransferTriggerGrid;

		private ComboBox comboBoxThreeGTransferEventType;

		private Button buttonRemoveThreeGTransferTrigger;

		private Button buttonAddThreeGTransferTrigger;

		private ComboBox comboBoxPartialDownload;

		private Label labelPartialDownload;

		private Label labelPartialDownloadInfo;

		public IModelValidator ModelValidator
		{
			get
			{
				return this.threeGDataTransferTriggerGrid.ModelValidator;
			}
			set
			{
				this.threeGDataTransferTriggerGrid.ModelValidator = value;
			}
		}

		public DisplayMode DisplayMode
		{
			get
			{
				return this.displayMode;
			}
			set
			{
				this.displayMode = value;
				this.UpdateGUI();
			}
		}

		public WLANConfiguration WLANConfiguration
		{
			get
			{
				return this.wlanConfiguration;
			}
			set
			{
				this.wlanConfiguration = value;
				if (this.wlanConfiguration != null)
				{
					this.threeGDataTransferTriggerGrid.DataTransferTriggerConfiguration = this.wlanConfiguration.ThreeGDataTransferTriggerConfiguration;
					this.UpdateGUI();
				}
			}
		}

		public WLANSettingsGL3Plus()
		{
			this.InitializeComponent();
			this.isInitControls = false;
			this.guiElementManager = new GUIElementManager_Control();
			this.customErrorProvider = new CustomErrorProvider(this.errorProviderFormat);
			this.customErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.LocalModelError, this.errorProviderLocalModel);
			this.customErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.GlobalModelError, this.errorProviderGlobalModel);
			this.pageValidator = new PageValidator(this.customErrorProvider);
			this.threeGDataTransferTriggerGrid.SelectionChanged += new EventHandler(this.OnThreeGDataTransferTriggerSelectionChanged);
			this.oldWLANStartConnectionEventName = string.Empty;
		}

		public void Init()
		{
			this.isInitControls = true;
			this.InitAnalogInputNumberCombobox();
			this.InitStartConnectionEventCombobox(ref this.comboBoxStartWLANConnectionOn);
			this.InitDownloadRingbuffersCombobox(ref this.comboBoxWLANor3GDownloadRingbuffer);
			this.InitDownloadRingbuffersCombobox(ref this.comboBoxWLANDownloadRingbuffer);
			this.InitThreeGTransferEventTypeCombobox();
			this.InitPartialDownloadCombobox();
			this.isInitControls = false;
		}

		public void Reset()
		{
			this.ResetValidationFramework();
		}

		public void InitAnalogInputNumberCombobox()
		{
			this.comboBoxAnalogInputNumber.Items.Clear();
			int num = 1;
			while ((long)num <= (long)((ulong)this.ModelValidator.LoggerSpecifics.IO.NumberOfAnalogInputsOnboard))
			{
				this.comboBoxAnalogInputNumber.Items.Add(num.ToString());
				num++;
			}
			if (this.comboBoxAnalogInputNumber.Items.Count > 0)
			{
				this.comboBoxAnalogInputNumber.SelectedIndex = 0;
			}
		}

		public void InitStartConnectionEventCombobox(ref ComboBox comboBox)
		{
			comboBox.Items.Clear();
			for (uint num = 1u; num <= this.ModelValidator.LoggerSpecifics.IO.NumberOfKeys; num += 1u)
			{
				comboBox.Items.Add(GUIUtil.MapKeyNumber2String(num, false));
			}
			for (uint num2 = 1u; num2 <= this.ModelValidator.LoggerSpecifics.IO.NumberOfPanelKeys; num2 += 1u)
			{
				comboBox.Items.Add(GUIUtil.MapKeyNumber2String(num2, true));
			}
			for (uint num3 = 1u + Constants.CasKeyOffset; num3 <= this.ModelValidator.LoggerSpecifics.IO.NumberOfCasKeys + Constants.CasKeyOffset; num3 += 1u)
			{
				comboBox.Items.Add(GUIUtil.MapKeyNumber2String(num3, false));
			}
			if (comboBox.Items.Count > 0)
			{
				comboBox.SelectedIndex = 0;
			}
		}

		public void InitDownloadRingbuffersCombobox(ref ComboBox comboBox)
		{
			comboBox.Items.Clear();
			comboBox.Items.Add(Resources.AllMemories);
			for (uint num = 1u; num <= this.ModelValidator.LoggerSpecifics.DataStorage.NumberOfMemories; num += 1u)
			{
				comboBox.Items.Add(GUIUtil.MapValueToMemoriesString(num));
			}
			if (comboBox.Items.Count > 0)
			{
				comboBox.SelectedIndex = 0;
			}
		}

		public void InitThreeGTransferEventTypeCombobox()
		{
			this.comboBoxThreeGTransferEventType.Items.Clear();
			this.comboBoxThreeGTransferEventType.Items.Add(Resources_Trigger.TriggerTypeNamesInComboStartNextLogSession);
			this.comboBoxThreeGTransferEventType.Items.Add(Resources_Trigger.TriggerTypeNamesInComboShutdown);
			this.comboBoxThreeGTransferEventType.Items.Add(Resources_Trigger.TriggerTypeNamesInComboKey);
			this.comboBoxThreeGTransferEventType.Items.Add(Resources_Trigger.TriggerTypeNamesInComboClockTime);
			this.comboBoxThreeGTransferEventType.Items.Add(Resources_Trigger.TriggerTypeNamesInComboRefLoggingTrigger);
			this.comboBoxThreeGTransferEventType.SelectedIndex = 0;
		}

		public void InitPartialDownloadCombobox()
		{
			this.comboBoxPartialDownload.Items.Clear();
			foreach (PartialDownloadType type in Enum.GetValues(typeof(PartialDownloadType)))
			{
				this.comboBoxPartialDownload.Items.Add(GUIUtil.MapPartialDownloadType2String(type));
			}
			this.comboBoxPartialDownload.SelectedIndex = 0;
		}

		private void checkBoxStartWLANor3GConnectOnShutdown_CheckedChanged(object sender, EventArgs e)
		{
			CheckBox checkBox = sender as CheckBox;
			if (checkBox == null)
			{
				return;
			}
			bool @checked = checkBox.Checked;
			this.SetControlEnabledStatesForWLANor3GOption(@checked);
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		private void checkBoxStartWLANConnectOn_CheckedChanged(object sender, EventArgs e)
		{
			CheckBox checkBox = sender as CheckBox;
			if (checkBox == null)
			{
				return;
			}
			bool @checked = checkBox.Checked;
			this.SetControlEnabledStatesForWLANOption(@checked);
			if (@checked && this.wlanConfiguration.StartWLANEvent == null)
			{
				this.wlanConfiguration.StartWLANEvent = new KeyEvent();
			}
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		private void checkBoxStart3GConnectOn_CheckedChanged(object sender, EventArgs e)
		{
			CheckBox checkBox = sender as CheckBox;
			if (checkBox == null)
			{
				return;
			}
			bool @checked = checkBox.Checked;
			this.SetControlEnabledStatesFor3GOption(@checked);
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		private void checkBoxWLANor3GDownloadRingbuffer_CheckedChanged(object sender, EventArgs e)
		{
			CheckBox checkBox = sender as CheckBox;
			if (checkBox == null)
			{
				return;
			}
			this.comboBoxWLANor3GDownloadRingbuffer.Enabled = checkBox.Checked;
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		private void checkBoxWLANDownloadRingbuffer_CheckedChanged(object sender, EventArgs e)
		{
			CheckBox checkBox = sender as CheckBox;
			if (checkBox == null)
			{
				return;
			}
			this.comboBoxWLANDownloadRingbuffer.Enabled = checkBox.Checked;
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		private void checkBox_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		private void comboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		private void buttonAddThree3TransferTrigger_Click(object sender, EventArgs e)
		{
			string a = this.comboBoxThreeGTransferEventType.SelectedItem.ToString();
			DataTransferTrigger dataTransferTrigger = null;
			if (a == Resources_Trigger.TriggerTypeNamesInComboClockTime)
			{
				dataTransferTrigger = DataTransferTrigger.CreateClockTimedTrigger();
			}
			else if (a == Resources_Trigger.TriggerTypeNamesInComboStartNextLogSession)
			{
				foreach (DataTransferTrigger current in this.wlanConfiguration.ThreeGDataTransferTriggerConfiguration.ActiveDataTransferTriggers)
				{
					if (current.Event is NextLogSessionStartEvent)
					{
						InformMessageBox.Error(Resources.ErrorOnlyOneNextLogSessionStartEvent);
						return;
					}
				}
				dataTransferTrigger = DataTransferTrigger.CreateNextLogSessionStartTrigger();
			}
			else if (a == Resources_Trigger.TriggerTypeNamesInComboShutdown)
			{
				foreach (DataTransferTrigger current2 in this.wlanConfiguration.ThreeGDataTransferTriggerConfiguration.ActiveDataTransferTriggers)
				{
					if (current2.Event is OnShutdownEvent)
					{
						InformMessageBox.Error(Resources.ErrorOnlyOneShutdownEvent);
						return;
					}
				}
				dataTransferTrigger = DataTransferTrigger.CreateOnShutdownTrigger();
			}
			else if (a == Resources_Trigger.TriggerTypeNamesInComboKey)
			{
				dataTransferTrigger = DataTransferTrigger.CreateKeyTrigger();
			}
			else if (a == Resources_Trigger.TriggerTypeNamesInComboRefLoggingTrigger)
			{
				dataTransferTrigger = DataTransferTrigger.CreateReferencedRecordTriggerNameTrigger();
			}
			if (dataTransferTrigger != null)
			{
				this.threeGDataTransferTriggerGrid.AddTrigger(dataTransferTrigger);
				return;
			}
		}

		private void buttonRemoveThreeGTransferTrigger_Click(object sender, EventArgs e)
		{
			this.threeGDataTransferTriggerGrid.RemoveTrigger();
		}

		private void OnThreeGDataTransferTriggerSelectionChanged(object sender, EventArgs e)
		{
			DataTransferTrigger dataTransferTrigger;
			this.buttonRemoveThreeGTransferTrigger.Enabled = this.threeGDataTransferTriggerGrid.TryGetSelectedTrigger(out dataTransferTrigger);
		}

		private void comboBoxPartialDownload_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		public void DisplayErrors()
		{
			this.threeGDataTransferTriggerGrid.DisplayErrors();
		}

		public bool ValidateInput()
		{
			if (this.wlanConfiguration == null)
			{
				return false;
			}
			bool flag = true;
			bool flag2 = false;
			this.pageValidator.General.ResetAllErrorProviders();
			this.pageValidator.General.ResetAllFormatErrors();
			bool flag3;
			flag &= this.pageValidator.Control.UpdateModel<bool>(this.checkBoxStartWLANor3GConnectOnShutdown.Checked, this.wlanConfiguration.IsStartWLANor3GOnShutdownEnabled, this.guiElementManager.GetGUIElement(this.checkBoxStartWLANor3GConnectOnShutdown), out flag3);
			flag2 |= flag3;
			if (this.wlanConfiguration.IsStartWLANor3GOnShutdownEnabled.Value)
			{
				uint value;
				if (!GUIUtil.DisplayStringToNumber(this.comboBoxAnalogInputNumber.SelectedItem.ToString(), out value))
				{
					value = 1u;
				}
				flag = this.pageValidator.Control.UpdateModel<uint>(value, this.wlanConfiguration.AnalogInputNumber, this.guiElementManager.GetGUIElement(this.comboBoxAnalogInputNumber), out flag3);
				flag2 |= flag3;
				flag &= this.pageValidator.Control.UpdateModel<bool>(this.checkBoxWLANor3GDownloadRingbuffer.Checked, this.wlanConfiguration.IsWLANor3GDownloadRingbufferEnabled, this.guiElementManager.GetGUIElement(this.checkBoxWLANor3GDownloadRingbuffer), out flag3);
				flag2 |= flag3;
				uint num = GUIUtil.MapMemoriesStringToValue(this.comboBoxWLANor3GDownloadRingbuffer.SelectedItem.ToString());
				if (this.wlanConfiguration.WLANor3GRingbuffersToDownload.Value != num)
				{
					this.wlanConfiguration.WLANor3GRingbuffersToDownload.Value = num;
					flag2 = true;
				}
				flag &= this.pageValidator.Control.UpdateModel<bool>(this.checkBoxWLANor3GDownloadClassification.Checked, this.wlanConfiguration.IsWLANor3GDownloadClassificationEnabled, this.guiElementManager.GetGUIElement(this.checkBoxWLANor3GDownloadClassification), out flag3);
				flag2 |= flag3;
				flag &= this.pageValidator.Control.UpdateModel<bool>(this.checkBoxWLANor3GDownloadDriveRecorder.Checked, this.wlanConfiguration.IsWLANor3GDownloadDriveRecorderEnabled, this.guiElementManager.GetGUIElement(this.checkBoxWLANor3GDownloadDriveRecorder), out flag3);
				flag2 |= flag3;
			}
			flag &= this.pageValidator.Control.UpdateModel<bool>(this.checkBoxStartWLANConnectOn.Checked, this.wlanConfiguration.IsStartWLANOnEventEnabled, this.guiElementManager.GetGUIElement(this.checkBoxStartWLANConnectOn), out flag3);
			flag2 |= flag3;
			if (this.wlanConfiguration.IsStartWLANOnEventEnabled.Value)
			{
				string text = this.comboBoxStartWLANConnectionOn.SelectedItem.ToString();
				if (this.oldWLANStartConnectionEventName != text || this.wlanConfiguration.StartWLANEvent == null)
				{
					bool value2 = false;
					uint value3;
					if (GUIUtil.TryMapStringToKeyNumber(text, out value3, out value2) && this.wlanConfiguration.StartWLANEvent is KeyEvent)
					{
						flag &= this.pageValidator.Control.UpdateModel<uint>(value3, (this.wlanConfiguration.StartWLANEvent as KeyEvent).Number, this.guiElementManager.GetGUIElement(this.comboBoxStartWLANConnectionOn), out flag3);
						flag2 |= flag3;
						flag &= this.pageValidator.Control.UpdateModel<bool>(value2, (this.wlanConfiguration.StartWLANEvent as KeyEvent).IsOnPanel, this.guiElementManager.GetGUIElement(this.comboBoxStartWLANConnectionOn), out flag3);
						flag2 |= flag3;
					}
					this.oldWLANStartConnectionEventName = text;
				}
				flag &= this.pageValidator.Control.UpdateModel<bool>(this.checkBoxWLANDownloadRingbuffer.Checked, this.wlanConfiguration.IsWLANDownloadRingbufferEnabled, this.guiElementManager.GetGUIElement(this.checkBoxWLANDownloadRingbuffer), out flag3);
				flag2 |= flag3;
				uint num2 = GUIUtil.MapMemoriesStringToValue(this.comboBoxWLANDownloadRingbuffer.SelectedItem.ToString());
				if (this.wlanConfiguration.WLANRingbuffersToDownload.Value != num2)
				{
					this.wlanConfiguration.WLANRingbuffersToDownload.Value = num2;
					flag2 = true;
				}
				flag &= this.pageValidator.Control.UpdateModel<bool>(this.checkBoxWLANDownloadClassification.Checked, this.wlanConfiguration.IsWLANDownloadClassificationEnabled, this.guiElementManager.GetGUIElement(this.checkBoxWLANDownloadClassification), out flag3);
				flag2 |= flag3;
				flag &= this.pageValidator.Control.UpdateModel<bool>(this.checkBoxWLANDownloadDriveRecorder.Checked, this.wlanConfiguration.IsWLANDownloadDriveRecorderEnabled, this.guiElementManager.GetGUIElement(this.checkBoxWLANDownloadDriveRecorder), out flag3);
				flag2 |= flag3;
				flag &= this.pageValidator.Control.UpdateModel<bool>(this.checkBoxFallbackTo3G.Checked, this.wlanConfiguration.IsWLANFallbackTo3GEnabled, this.guiElementManager.GetGUIElement(this.checkBoxFallbackTo3G), out flag3);
				flag2 |= flag3;
			}
			flag &= this.pageValidator.Control.UpdateModel<bool>(this.checkBoxStart3GConnectOn.Checked, this.wlanConfiguration.IsStartThreeGOnEventEnabled, this.guiElementManager.GetGUIElement(this.checkBoxStart3GConnectOn), out flag3);
			flag2 |= flag3;
			PartialDownloadType value4 = GUIUtil.MapString2PartialDownloadType(this.comboBoxPartialDownload.SelectedItem.ToString());
			flag &= this.pageValidator.Control.UpdateModel<PartialDownloadType>(value4, this.wlanConfiguration.PartialDownload, this.guiElementManager.GetGUIElement(this.comboBoxPartialDownload), out flag3);
			flag2 |= flag3;
			if (this.wlanConfiguration.IsStartThreeGOnEventEnabled.Value)
			{
				flag &= this.threeGDataTransferTriggerGrid.ValidateInput(false);
			}
			else
			{
				this.threeGDataTransferTriggerGrid.ResetValidationFramework();
			}
			flag &= this.ModelValidator.Validate(this.wlanConfiguration, flag2, this.pageValidator);
			this.errorProviderLocalModel.SetError(this.comboBoxStartWLANConnectionOn, "");
			if (this.wlanConfiguration.IsStartWLANOnEventEnabled.Value && this.wlanConfiguration.StartWLANEvent is KeyEvent && this.wlanConfiguration.IsStartThreeGOnEventEnabled.Value)
			{
				KeyEvent keyEvent = this.wlanConfiguration.StartWLANEvent as KeyEvent;
				foreach (DataTransferTrigger current in this.wlanConfiguration.ThreeGDataTransferTriggerConfiguration.ActiveDataTransferTriggers)
				{
					if (current.Event is KeyEvent)
					{
						KeyEvent obj = current.Event as KeyEvent;
						if (keyEvent.Equals(obj))
						{
							this.pageValidator.ResultCollector.SetErrorText(ValidationErrorClass.LocalModelError, (this.wlanConfiguration.StartWLANEvent as KeyEvent).IsOnPanel, Resources.ErrorStartConnCondMustDiffer);
							break;
						}
					}
				}
			}
			this.pageValidator.General.ActivateErrorProvidersForFormatAndModelErrors();
			this.threeGDataTransferTriggerGrid.DisplayErrors();
			return flag;
		}

		public bool HasErrors()
		{
			return this.pageValidator.General.HasErrors(Enum.GetValues(typeof(ValidationErrorClass)) as ValidationErrorClass[]) || this.threeGDataTransferTriggerGrid.HasErrors();
		}

		public bool HasGlobalErrors()
		{
			return this.pageValidator.General.HasErrors(new ValidationErrorClass[]
			{
				ValidationErrorClass.GlobalModelError
			}) || this.threeGDataTransferTriggerGrid.HasGlobalErrors();
		}

		public bool HasLocalErrors()
		{
			return this.pageValidator.General.HasErrors(new ValidationErrorClass[]
			{
				ValidationErrorClass.FormatError,
				ValidationErrorClass.LocalModelError
			}) || this.threeGDataTransferTriggerGrid.HasLocalErrors();
		}

		public bool HasFormatErrors()
		{
			IPageValidatorGeneral arg_13_0 = this.pageValidator.General;
			ValidationErrorClass[] errorClasses = new ValidationErrorClass[1];
			return arg_13_0.HasErrors(errorClasses) || this.threeGDataTransferTriggerGrid.HasFormatErrors();
		}

		private void ResetValidationFramework()
		{
			this.pageValidator.General.Reset();
			this.guiElementManager.Reset();
			this.oldWLANStartConnectionEventName = string.Empty;
		}

		private void UpdateGUI()
		{
			if (this.wlanConfiguration == null)
			{
				return;
			}
			this.isInitControls = true;
			this.checkBoxStartWLANor3GConnectOnShutdown.Checked = this.wlanConfiguration.IsStartWLANor3GOnShutdownEnabled.Value;
			this.comboBoxAnalogInputNumber.SelectedItem = this.wlanConfiguration.AnalogInputNumber.Value.ToString();
			this.checkBoxWLANor3GDownloadRingbuffer.Checked = this.wlanConfiguration.IsWLANor3GDownloadRingbufferEnabled.Value;
			this.comboBoxWLANor3GDownloadRingbuffer.SelectedItem = GUIUtil.MapValueToMemoriesString(this.wlanConfiguration.WLANor3GRingbuffersToDownload.Value);
			this.checkBoxWLANor3GDownloadClassification.Checked = this.wlanConfiguration.IsWLANor3GDownloadClassificationEnabled.Value;
			this.checkBoxWLANor3GDownloadDriveRecorder.Checked = this.wlanConfiguration.IsWLANor3GDownloadDriveRecorderEnabled.Value;
			this.SetControlEnabledStatesForWLANor3GOption(this.checkBoxStartWLANor3GConnectOnShutdown.Checked);
			this.checkBoxStartWLANConnectOn.Checked = this.wlanConfiguration.IsStartWLANOnEventEnabled.Value;
			string eventNameForTrigger = this.GetEventNameForTrigger(this.wlanConfiguration.StartWLANEvent);
			if (!string.IsNullOrEmpty(eventNameForTrigger))
			{
				this.comboBoxStartWLANConnectionOn.SelectedItem = eventNameForTrigger;
			}
			else
			{
				this.comboBoxStartWLANConnectionOn.SelectedIndex = 0;
			}
			this.checkBoxWLANDownloadRingbuffer.Checked = this.wlanConfiguration.IsWLANDownloadRingbufferEnabled.Value;
			this.comboBoxWLANDownloadRingbuffer.SelectedItem = GUIUtil.MapValueToMemoriesString(this.wlanConfiguration.WLANRingbuffersToDownload.Value);
			this.checkBoxWLANDownloadClassification.Checked = this.wlanConfiguration.IsWLANDownloadClassificationEnabled.Value;
			this.checkBoxWLANDownloadDriveRecorder.Checked = this.wlanConfiguration.IsWLANDownloadDriveRecorderEnabled.Value;
			this.checkBoxFallbackTo3G.Checked = this.wlanConfiguration.IsWLANFallbackTo3GEnabled.Value;
			this.SetControlEnabledStatesForWLANOption(this.checkBoxStartWLANConnectOn.Checked);
			this.checkBoxStart3GConnectOn.Checked = this.wlanConfiguration.IsStartThreeGOnEventEnabled.Value;
			this.SetControlEnabledStatesFor3GOption(this.checkBoxStart3GConnectOn.Checked);
			this.comboBoxPartialDownload.SelectedItem = GUIUtil.MapPartialDownloadType2String(this.wlanConfiguration.PartialDownload.Value);
			this.isInitControls = false;
			this.ValidateInput();
		}

		private string GetEventNameForTrigger(Event myEvent)
		{
			string result = "";
			if (myEvent is KeyEvent)
			{
				KeyEvent keyEvent = myEvent as KeyEvent;
				result = GUIUtil.MapKeyNumber2String(keyEvent.Number.Value, keyEvent.IsOnPanel.Value);
			}
			else if (myEvent is DigitalInputEvent)
			{
				DigitalInputEvent digitalInputEvent = myEvent as DigitalInputEvent;
				result = GUIUtil.MapDigitalInputNumber2String(digitalInputEvent.DigitalInput.Value);
			}
			return result;
		}

		private void SetControlEnabledStatesForWLANor3GOption(bool isEnabled)
		{
			this.comboBoxAnalogInputNumber.Enabled = isEnabled;
			this.checkBoxWLANor3GDownloadRingbuffer.Enabled = isEnabled;
			this.comboBoxWLANor3GDownloadRingbuffer.Enabled = (this.checkBoxWLANor3GDownloadRingbuffer.Enabled && this.checkBoxWLANor3GDownloadRingbuffer.Checked);
			this.checkBoxWLANor3GDownloadClassification.Enabled = isEnabled;
			this.checkBoxWLANor3GDownloadDriveRecorder.Enabled = isEnabled;
			this.labelStartWLAN3GNote.Enabled = isEnabled;
		}

		private void SetControlEnabledStatesForWLANOption(bool isEnabled)
		{
			this.comboBoxStartWLANConnectionOn.Enabled = isEnabled;
			this.checkBoxWLANDownloadRingbuffer.Enabled = isEnabled;
			this.comboBoxWLANDownloadRingbuffer.Enabled = (this.checkBoxWLANDownloadRingbuffer.Enabled && this.checkBoxWLANDownloadRingbuffer.Checked);
			this.checkBoxWLANDownloadClassification.Enabled = isEnabled;
			this.checkBoxWLANDownloadDriveRecorder.Enabled = isEnabled;
			this.checkBoxFallbackTo3G.Enabled = isEnabled;
		}

		private void SetControlEnabledStatesFor3GOption(bool isEnabled)
		{
			this.threeGDataTransferTriggerGrid.Enabled = isEnabled;
			this.comboBoxThreeGTransferEventType.Enabled = isEnabled;
			this.buttonAddThreeGTransferTrigger.Enabled = isEnabled;
			this.labelPartialDownloadInfo.Visible = isEnabled;
			if (!isEnabled)
			{
				this.buttonRemoveThreeGTransferTrigger.Enabled = false;
				return;
			}
			DataTransferTrigger dataTransferTrigger;
			this.buttonRemoveThreeGTransferTrigger.Enabled = this.threeGDataTransferTriggerGrid.TryGetSelectedTrigger(out dataTransferTrigger);
		}

		public bool Serialize(WLANSettingsGL3PlusPage wlanSettingsGL3PlusPage)
		{
			if (wlanSettingsGL3PlusPage == null)
			{
				return false;
			}
			bool flag = true;
			return flag & this.threeGDataTransferTriggerGrid.Serialize<WLANSettingsGL3PlusPage>(wlanSettingsGL3PlusPage);
		}

		public bool DeSerialize(WLANSettingsGL3PlusPage wlanSettingsGL3PlusPage)
		{
			if (wlanSettingsGL3PlusPage == null)
			{
				return false;
			}
			bool flag = true;
			return flag & this.threeGDataTransferTriggerGrid.DeSerialize<WLANSettingsGL3PlusPage>(wlanSettingsGL3PlusPage);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(WLANSettingsGL3Plus));
			this.checkBoxStartWLANor3GConnectOnShutdown = new CheckBox();
			this.comboBoxAnalogInputNumber = new ComboBox();
			this.checkBoxStartWLANConnectOn = new CheckBox();
			this.comboBoxStartWLANConnectionOn = new ComboBox();
			this.checkBoxWLANor3GDownloadRingbuffer = new CheckBox();
			this.checkBoxWLANor3GDownloadClassification = new CheckBox();
			this.checkBoxWLANor3GDownloadDriveRecorder = new CheckBox();
			this.comboBoxWLANor3GDownloadRingbuffer = new ComboBox();
			this.errorProviderFormat = new ErrorProvider(this.components);
			this.comboBoxWLANDownloadRingbuffer = new ComboBox();
			this.checkBoxWLANDownloadRingbuffer = new CheckBox();
			this.checkBoxWLANDownloadClassification = new CheckBox();
			this.checkBoxWLANDownloadDriveRecorder = new CheckBox();
			this.checkBoxFallbackTo3G = new CheckBox();
			this.checkBoxStart3GConnectOn = new CheckBox();
			this.comboBoxPartialDownload = new ComboBox();
			this.labelStartWLAN3GNote = new Label();
			this.buttonAddThreeGTransferTrigger = new Button();
			this.buttonRemoveThreeGTransferTrigger = new Button();
			this.comboBoxThreeGTransferEventType = new ComboBox();
			this.labelPartialDownload = new Label();
			this.labelPartialDownloadInfo = new Label();
			this.errorProviderLocalModel = new ErrorProvider(this.components);
			this.errorProviderGlobalModel = new ErrorProvider(this.components);
			this.threeGDataTransferTriggerGrid = new DataTransferTriggerGrid();
			((ISupportInitialize)this.errorProviderFormat).BeginInit();
			((ISupportInitialize)this.errorProviderLocalModel).BeginInit();
			((ISupportInitialize)this.errorProviderGlobalModel).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.checkBoxStartWLANor3GConnectOnShutdown, "checkBoxStartWLANor3GConnectOnShutdown");
			this.errorProviderFormat.SetIconAlignment(this.checkBoxStartWLANor3GConnectOnShutdown, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxStartWLANor3GConnectOnShutdown.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxStartWLANor3GConnectOnShutdown, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxStartWLANor3GConnectOnShutdown.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxStartWLANor3GConnectOnShutdown, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxStartWLANor3GConnectOnShutdown.IconAlignment2"));
			this.checkBoxStartWLANor3GConnectOnShutdown.Name = "checkBoxStartWLANor3GConnectOnShutdown";
			this.checkBoxStartWLANor3GConnectOnShutdown.UseVisualStyleBackColor = true;
			this.checkBoxStartWLANor3GConnectOnShutdown.CheckedChanged += new EventHandler(this.checkBoxStartWLANor3GConnectOnShutdown_CheckedChanged);
			this.comboBoxAnalogInputNumber.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxAnalogInputNumber.FormattingEnabled = true;
			this.errorProviderLocalModel.SetIconAlignment(this.comboBoxAnalogInputNumber, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxAnalogInputNumber.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.comboBoxAnalogInputNumber, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxAnalogInputNumber.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.comboBoxAnalogInputNumber, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxAnalogInputNumber.IconAlignment2"));
			componentResourceManager.ApplyResources(this.comboBoxAnalogInputNumber, "comboBoxAnalogInputNumber");
			this.comboBoxAnalogInputNumber.Name = "comboBoxAnalogInputNumber";
			this.comboBoxAnalogInputNumber.SelectedIndexChanged += new EventHandler(this.comboBox_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.checkBoxStartWLANConnectOn, "checkBoxStartWLANConnectOn");
			this.errorProviderFormat.SetIconAlignment(this.checkBoxStartWLANConnectOn, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxStartWLANConnectOn.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxStartWLANConnectOn, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxStartWLANConnectOn.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxStartWLANConnectOn, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxStartWLANConnectOn.IconAlignment2"));
			this.checkBoxStartWLANConnectOn.Name = "checkBoxStartWLANConnectOn";
			this.checkBoxStartWLANConnectOn.UseVisualStyleBackColor = true;
			this.checkBoxStartWLANConnectOn.CheckedChanged += new EventHandler(this.checkBoxStartWLANConnectOn_CheckedChanged);
			this.comboBoxStartWLANConnectionOn.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxStartWLANConnectionOn.FormattingEnabled = true;
			this.errorProviderLocalModel.SetIconAlignment(this.comboBoxStartWLANConnectionOn, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxStartWLANConnectionOn.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.comboBoxStartWLANConnectionOn, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxStartWLANConnectionOn.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.comboBoxStartWLANConnectionOn, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxStartWLANConnectionOn.IconAlignment2"));
			componentResourceManager.ApplyResources(this.comboBoxStartWLANConnectionOn, "comboBoxStartWLANConnectionOn");
			this.comboBoxStartWLANConnectionOn.Name = "comboBoxStartWLANConnectionOn";
			this.comboBoxStartWLANConnectionOn.SelectedIndexChanged += new EventHandler(this.comboBox_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.checkBoxWLANor3GDownloadRingbuffer, "checkBoxWLANor3GDownloadRingbuffer");
			this.errorProviderFormat.SetIconAlignment(this.checkBoxWLANor3GDownloadRingbuffer, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxWLANor3GDownloadRingbuffer.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxWLANor3GDownloadRingbuffer, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxWLANor3GDownloadRingbuffer.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxWLANor3GDownloadRingbuffer, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxWLANor3GDownloadRingbuffer.IconAlignment2"));
			this.checkBoxWLANor3GDownloadRingbuffer.Name = "checkBoxWLANor3GDownloadRingbuffer";
			this.checkBoxWLANor3GDownloadRingbuffer.UseVisualStyleBackColor = true;
			this.checkBoxWLANor3GDownloadRingbuffer.CheckedChanged += new EventHandler(this.checkBoxWLANor3GDownloadRingbuffer_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxWLANor3GDownloadClassification, "checkBoxWLANor3GDownloadClassification");
			this.errorProviderFormat.SetIconAlignment(this.checkBoxWLANor3GDownloadClassification, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxWLANor3GDownloadClassification.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxWLANor3GDownloadClassification, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxWLANor3GDownloadClassification.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxWLANor3GDownloadClassification, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxWLANor3GDownloadClassification.IconAlignment2"));
			this.checkBoxWLANor3GDownloadClassification.Name = "checkBoxWLANor3GDownloadClassification";
			this.checkBoxWLANor3GDownloadClassification.UseVisualStyleBackColor = true;
			this.checkBoxWLANor3GDownloadClassification.CheckedChanged += new EventHandler(this.checkBox_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxWLANor3GDownloadDriveRecorder, "checkBoxWLANor3GDownloadDriveRecorder");
			this.errorProviderFormat.SetIconAlignment(this.checkBoxWLANor3GDownloadDriveRecorder, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxWLANor3GDownloadDriveRecorder.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxWLANor3GDownloadDriveRecorder, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxWLANor3GDownloadDriveRecorder.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxWLANor3GDownloadDriveRecorder, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxWLANor3GDownloadDriveRecorder.IconAlignment2"));
			this.checkBoxWLANor3GDownloadDriveRecorder.Name = "checkBoxWLANor3GDownloadDriveRecorder";
			this.checkBoxWLANor3GDownloadDriveRecorder.UseVisualStyleBackColor = true;
			this.checkBoxWLANor3GDownloadDriveRecorder.CheckedChanged += new EventHandler(this.checkBox_CheckedChanged);
			this.comboBoxWLANor3GDownloadRingbuffer.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxWLANor3GDownloadRingbuffer.FormattingEnabled = true;
			this.errorProviderLocalModel.SetIconAlignment(this.comboBoxWLANor3GDownloadRingbuffer, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxWLANor3GDownloadRingbuffer.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.comboBoxWLANor3GDownloadRingbuffer, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxWLANor3GDownloadRingbuffer.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.comboBoxWLANor3GDownloadRingbuffer, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxWLANor3GDownloadRingbuffer.IconAlignment2"));
			componentResourceManager.ApplyResources(this.comboBoxWLANor3GDownloadRingbuffer, "comboBoxWLANor3GDownloadRingbuffer");
			this.comboBoxWLANor3GDownloadRingbuffer.Name = "comboBoxWLANor3GDownloadRingbuffer";
			this.comboBoxWLANor3GDownloadRingbuffer.SelectedIndexChanged += new EventHandler(this.comboBox_SelectedIndexChanged);
			this.errorProviderFormat.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderFormat.ContainerControl = this;
			this.comboBoxWLANDownloadRingbuffer.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxWLANDownloadRingbuffer.FormattingEnabled = true;
			this.errorProviderLocalModel.SetIconAlignment(this.comboBoxWLANDownloadRingbuffer, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxWLANDownloadRingbuffer.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.comboBoxWLANDownloadRingbuffer, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxWLANDownloadRingbuffer.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.comboBoxWLANDownloadRingbuffer, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxWLANDownloadRingbuffer.IconAlignment2"));
			componentResourceManager.ApplyResources(this.comboBoxWLANDownloadRingbuffer, "comboBoxWLANDownloadRingbuffer");
			this.comboBoxWLANDownloadRingbuffer.Name = "comboBoxWLANDownloadRingbuffer";
			this.comboBoxWLANDownloadRingbuffer.SelectedIndexChanged += new EventHandler(this.comboBox_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.checkBoxWLANDownloadRingbuffer, "checkBoxWLANDownloadRingbuffer");
			this.errorProviderFormat.SetIconAlignment(this.checkBoxWLANDownloadRingbuffer, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxWLANDownloadRingbuffer.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxWLANDownloadRingbuffer, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxWLANDownloadRingbuffer.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxWLANDownloadRingbuffer, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxWLANDownloadRingbuffer.IconAlignment2"));
			this.checkBoxWLANDownloadRingbuffer.Name = "checkBoxWLANDownloadRingbuffer";
			this.checkBoxWLANDownloadRingbuffer.UseVisualStyleBackColor = true;
			this.checkBoxWLANDownloadRingbuffer.CheckedChanged += new EventHandler(this.checkBoxWLANDownloadRingbuffer_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxWLANDownloadClassification, "checkBoxWLANDownloadClassification");
			this.errorProviderFormat.SetIconAlignment(this.checkBoxWLANDownloadClassification, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxWLANDownloadClassification.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxWLANDownloadClassification, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxWLANDownloadClassification.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxWLANDownloadClassification, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxWLANDownloadClassification.IconAlignment2"));
			this.checkBoxWLANDownloadClassification.Name = "checkBoxWLANDownloadClassification";
			this.checkBoxWLANDownloadClassification.UseVisualStyleBackColor = true;
			this.checkBoxWLANDownloadClassification.CheckedChanged += new EventHandler(this.checkBox_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxWLANDownloadDriveRecorder, "checkBoxWLANDownloadDriveRecorder");
			this.errorProviderFormat.SetIconAlignment(this.checkBoxWLANDownloadDriveRecorder, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxWLANDownloadDriveRecorder.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxWLANDownloadDriveRecorder, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxWLANDownloadDriveRecorder.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxWLANDownloadDriveRecorder, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxWLANDownloadDriveRecorder.IconAlignment2"));
			this.checkBoxWLANDownloadDriveRecorder.Name = "checkBoxWLANDownloadDriveRecorder";
			this.checkBoxWLANDownloadDriveRecorder.UseVisualStyleBackColor = true;
			this.checkBoxWLANDownloadDriveRecorder.CheckedChanged += new EventHandler(this.checkBox_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxFallbackTo3G, "checkBoxFallbackTo3G");
			this.errorProviderFormat.SetIconAlignment(this.checkBoxFallbackTo3G, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxFallbackTo3G.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxFallbackTo3G, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxFallbackTo3G.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxFallbackTo3G, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxFallbackTo3G.IconAlignment2"));
			this.checkBoxFallbackTo3G.Name = "checkBoxFallbackTo3G";
			this.checkBoxFallbackTo3G.UseVisualStyleBackColor = true;
			this.checkBoxFallbackTo3G.CheckedChanged += new EventHandler(this.checkBox_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxStart3GConnectOn, "checkBoxStart3GConnectOn");
			this.errorProviderFormat.SetIconAlignment(this.checkBoxStart3GConnectOn, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxStart3GConnectOn.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxStart3GConnectOn, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxStart3GConnectOn.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxStart3GConnectOn, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxStart3GConnectOn.IconAlignment2"));
			this.checkBoxStart3GConnectOn.Name = "checkBoxStart3GConnectOn";
			this.checkBoxStart3GConnectOn.UseVisualStyleBackColor = true;
			this.checkBoxStart3GConnectOn.CheckedChanged += new EventHandler(this.checkBoxStart3GConnectOn_CheckedChanged);
			this.comboBoxPartialDownload.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxPartialDownload.FormattingEnabled = true;
			this.errorProviderLocalModel.SetIconAlignment(this.comboBoxPartialDownload, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxPartialDownload.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.comboBoxPartialDownload, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxPartialDownload.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.comboBoxPartialDownload, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxPartialDownload.IconAlignment2"));
			componentResourceManager.ApplyResources(this.comboBoxPartialDownload, "comboBoxPartialDownload");
			this.comboBoxPartialDownload.Name = "comboBoxPartialDownload";
			this.comboBoxPartialDownload.SelectedIndexChanged += new EventHandler(this.comboBoxPartialDownload_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.labelStartWLAN3GNote, "labelStartWLAN3GNote");
			this.labelStartWLAN3GNote.Name = "labelStartWLAN3GNote";
			componentResourceManager.ApplyResources(this.buttonAddThreeGTransferTrigger, "buttonAddThreeGTransferTrigger");
			this.buttonAddThreeGTransferTrigger.Name = "buttonAddThreeGTransferTrigger";
			this.buttonAddThreeGTransferTrigger.UseVisualStyleBackColor = true;
			this.buttonAddThreeGTransferTrigger.Click += new EventHandler(this.buttonAddThree3TransferTrigger_Click);
			componentResourceManager.ApplyResources(this.buttonRemoveThreeGTransferTrigger, "buttonRemoveThreeGTransferTrigger");
			this.buttonRemoveThreeGTransferTrigger.Name = "buttonRemoveThreeGTransferTrigger";
			this.buttonRemoveThreeGTransferTrigger.UseVisualStyleBackColor = true;
			this.buttonRemoveThreeGTransferTrigger.Click += new EventHandler(this.buttonRemoveThreeGTransferTrigger_Click);
			this.comboBoxThreeGTransferEventType.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxThreeGTransferEventType.FormattingEnabled = true;
			componentResourceManager.ApplyResources(this.comboBoxThreeGTransferEventType, "comboBoxThreeGTransferEventType");
			this.comboBoxThreeGTransferEventType.Name = "comboBoxThreeGTransferEventType";
			componentResourceManager.ApplyResources(this.labelPartialDownload, "labelPartialDownload");
			this.labelPartialDownload.Name = "labelPartialDownload";
			componentResourceManager.ApplyResources(this.labelPartialDownloadInfo, "labelPartialDownloadInfo");
			this.labelPartialDownloadInfo.Name = "labelPartialDownloadInfo";
			this.errorProviderLocalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderLocalModel.ContainerControl = this;
			this.errorProviderGlobalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderGlobalModel.ContainerControl = this;
			componentResourceManager.ApplyResources(this.errorProviderGlobalModel, "errorProviderGlobalModel");
			componentResourceManager.ApplyResources(this.threeGDataTransferTriggerGrid, "threeGDataTransferTriggerGrid");
			this.threeGDataTransferTriggerGrid.DataTransferTriggerConfiguration = null;
			this.threeGDataTransferTriggerGrid.ModelValidator = null;
			this.threeGDataTransferTriggerGrid.Name = "threeGDataTransferTriggerGrid";
			base.AutoScaleMode = AutoScaleMode.Inherit;
			base.Controls.Add(this.labelPartialDownloadInfo);
			base.Controls.Add(this.comboBoxPartialDownload);
			base.Controls.Add(this.labelPartialDownload);
			base.Controls.Add(this.comboBoxThreeGTransferEventType);
			base.Controls.Add(this.buttonRemoveThreeGTransferTrigger);
			base.Controls.Add(this.buttonAddThreeGTransferTrigger);
			base.Controls.Add(this.threeGDataTransferTriggerGrid);
			base.Controls.Add(this.labelStartWLAN3GNote);
			base.Controls.Add(this.checkBoxStart3GConnectOn);
			base.Controls.Add(this.checkBoxFallbackTo3G);
			base.Controls.Add(this.checkBoxWLANDownloadDriveRecorder);
			base.Controls.Add(this.checkBoxWLANDownloadClassification);
			base.Controls.Add(this.comboBoxWLANDownloadRingbuffer);
			base.Controls.Add(this.checkBoxWLANDownloadRingbuffer);
			base.Controls.Add(this.comboBoxStartWLANConnectionOn);
			base.Controls.Add(this.checkBoxStartWLANConnectOn);
			base.Controls.Add(this.checkBoxWLANor3GDownloadDriveRecorder);
			base.Controls.Add(this.comboBoxWLANor3GDownloadRingbuffer);
			base.Controls.Add(this.checkBoxWLANor3GDownloadClassification);
			base.Controls.Add(this.comboBoxAnalogInputNumber);
			base.Controls.Add(this.checkBoxStartWLANor3GConnectOnShutdown);
			base.Controls.Add(this.checkBoxWLANor3GDownloadRingbuffer);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "WLANSettingsGL3Plus";
			((ISupportInitialize)this.errorProviderFormat).EndInit();
			((ISupportInitialize)this.errorProviderLocalModel).EndInit();
			((ISupportInitialize)this.errorProviderGlobalModel).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
