using System;
using System.ComponentModel;
using System.Globalization;
using System.Net;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.Properties;
using Vector.VLConfig.ValidationFramework;

namespace Vector.VLConfig.GUI.WLANSettingsPage
{
	public class WLANSettingsGL2000 : UserControl
	{
		private GUIElementManager_Control guiElementManager;

		private CustomErrorProvider customErrorProvider;

		private PageValidator pageValidator;

		private WLANConfiguration wlanConfiguration;

		private DisplayMode displayMode;

		private bool isInitControls;

		private IContainer components;

		private GroupBox groupBox1;

		private Label labelPartialDownloadInfo;

		private ComboBox comboBoxPartialDownload;

		private Label labelPartialDownload;

		private ComboBox comboBoxThreeGTransferEventType;

		private Button buttonRemoveThreeGTransferTrigger;

		private Button buttonAddThreeGTransferTrigger;

		private DataTransferTriggerGrid threeGDataTransferTriggerGrid;

		private CheckBox checkBoxStart3GConnectOn;

		private Label label1;

		private MaskedTextBox maskedTextBoxSubnetMask;

		private Label label3;

		private MaskedTextBox maskedTextBoxGatewayIp;

		private Label label4;

		private MaskedTextBox maskedTextBoxMlserverIp;

		private Label label2;

		private MaskedTextBox maskedTextBoxLoggerIp;

		private ErrorProvider errorProviderFormat;

		private ErrorProvider errorProviderLocalModel;

		private ErrorProvider errorProviderGlobalModel;

		private GroupBox groupBox2;

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

		public WLANSettingsGL2000()
		{
			this.InitializeComponent();
			this.isInitControls = false;
			this.guiElementManager = new GUIElementManager_Control();
			this.customErrorProvider = new CustomErrorProvider(this.errorProviderFormat);
			this.customErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.LocalModelError, this.errorProviderLocalModel);
			this.customErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.GlobalModelError, this.errorProviderGlobalModel);
			this.customErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.FormatError, this.errorProviderFormat);
			this.pageValidator = new PageValidator(this.customErrorProvider);
			this.threeGDataTransferTriggerGrid.SelectionChanged += new EventHandler(this.OnThreeGDataTransferTriggerSelectionChanged);
		}

		public void Init()
		{
			this.isInitControls = true;
			this.InitThreeGTransferEventTypeCombobox();
			this.InitPartialDownloadCombobox();
			this.isInitControls = false;
		}

		public void Reset()
		{
			this.ResetValidationFramework();
			this.UpdateGUI();
			this.ValidateInput();
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

		private void maskedTextBoxLoggerIp_Validating(object sender, CancelEventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		private void maskedTextBoxGatewayIp_Validating(object sender, CancelEventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		private void maskedTextBoxMlserverIp_Validating(object sender, CancelEventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		private void maskedTextBoxSubnetMask_Validating(object sender, CancelEventArgs e)
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
			flag &= this.pageValidator.Control.UpdateModel<bool>(this.checkBoxStart3GConnectOn.Checked, this.wlanConfiguration.IsStartThreeGOnEventEnabled, this.guiElementManager.GetGUIElement(this.checkBoxStart3GConnectOn), out flag3);
			flag2 |= flag3;
			if (this.wlanConfiguration.IsStartThreeGOnEventEnabled.Value)
			{
				PartialDownloadType value = GUIUtil.MapString2PartialDownloadType(this.comboBoxPartialDownload.SelectedItem.ToString());
				flag &= this.pageValidator.Control.UpdateModel<PartialDownloadType>(value, this.wlanConfiguration.PartialDownload, this.guiElementManager.GetGUIElement(this.comboBoxPartialDownload), out flag3);
				flag2 |= flag3;
				flag &= this.ValidateIp(this.maskedTextBoxLoggerIp, this.wlanConfiguration.LoggerIp, ref flag2, false, false);
				flag &= this.ValidateIp(this.maskedTextBoxGatewayIp, this.wlanConfiguration.GatewayIp, ref flag2, false, true);
				flag &= this.ValidateIp(this.maskedTextBoxSubnetMask, this.wlanConfiguration.SubnetMask, ref flag2, true, true);
				flag &= this.ValidateIp(this.maskedTextBoxMlserverIp, this.wlanConfiguration.MLserverIp, ref flag2, false, false);
			}
			if (this.wlanConfiguration.IsStartThreeGOnEventEnabled.Value)
			{
				flag &= this.threeGDataTransferTriggerGrid.ValidateInput(false);
			}
			else
			{
				this.threeGDataTransferTriggerGrid.ResetValidationFramework();
			}
			flag &= this.ModelValidator.Validate(this.wlanConfiguration, flag2, this.pageValidator);
			this.pageValidator.General.ActivateErrorProvidersForFormatAndModelErrors();
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
		}

		private void UpdateGUI()
		{
			if (this.wlanConfiguration == null)
			{
				return;
			}
			this.isInitControls = true;
			this.checkBoxStart3GConnectOn.Checked = this.wlanConfiguration.IsStartThreeGOnEventEnabled.Value;
			this.SetControlEnabledStatesFor3GOption(this.checkBoxStart3GConnectOn.Checked);
			this.comboBoxPartialDownload.SelectedItem = GUIUtil.MapPartialDownloadType2String(this.wlanConfiguration.PartialDownload.Value);
			this.SetIP(this.maskedTextBoxLoggerIp, this.wlanConfiguration.LoggerIp);
			this.SetIP(this.maskedTextBoxGatewayIp, this.wlanConfiguration.GatewayIp);
			this.SetIP(this.maskedTextBoxSubnetMask, this.wlanConfiguration.SubnetMask);
			this.SetIP(this.maskedTextBoxMlserverIp, this.wlanConfiguration.MLserverIp);
			this.isInitControls = false;
			this.ValidateInput();
		}

		private void SetIP(Control ipControl, ValidatedProperty<string> ip)
		{
			if (ipControl == null || ip == null || ip.Value == null)
			{
				return;
			}
			if (this.pageValidator.General.HasFormatError(ip))
			{
				return;
			}
			string[] array = ip.Value.Split(new char[]
			{
				'.'
			});
			if (array.Length < 4)
			{
				return;
			}
			if (ipControl is MaskedTextBox)
			{
				(ipControl as MaskedTextBox).Text = string.Format("{0,3}{1,3}{2,3}{3,3}", array);
			}
		}

		private void SetControlEnabledStatesFor3GOption(bool isEnabled)
		{
			this.threeGDataTransferTriggerGrid.Enabled = isEnabled;
			this.comboBoxThreeGTransferEventType.Enabled = isEnabled;
			this.comboBoxPartialDownload.Enabled = isEnabled;
			this.buttonAddThreeGTransferTrigger.Enabled = isEnabled;
			this.maskedTextBoxLoggerIp.Enabled = isEnabled;
			this.maskedTextBoxGatewayIp.Enabled = isEnabled;
			this.maskedTextBoxMlserverIp.Enabled = isEnabled;
			this.maskedTextBoxSubnetMask.Enabled = isEnabled;
			if (!isEnabled)
			{
				this.buttonRemoveThreeGTransferTrigger.Enabled = false;
				return;
			}
			DataTransferTrigger dataTransferTrigger;
			this.buttonRemoveThreeGTransferTrigger.Enabled = this.threeGDataTransferTriggerGrid.TryGetSelectedTrigger(out dataTransferTrigger);
		}

		private bool ValidateByteValue(string textValue, ref byte value)
		{
			return byte.TryParse(textValue, out value);
		}

		private bool ValidateIp(Control ipControl, IValidatedProperty<string> ipProperty, ref bool valueChanged, bool isSubnetMask, bool onlyZerosAllowed)
		{
			if (ipControl == null || !(ipControl is MaskedTextBox) || ipProperty == null)
			{
				return false;
			}
			byte b = 0;
			byte b2 = 0;
			byte b3 = 0;
			byte b4 = 0;
			bool flag = true;
			string[] array = (ipControl as MaskedTextBox).Text.Split(new char[]
			{
				'.'
			});
			if (array.Length >= 4)
			{
				if (!this.ValidateByteValue(array[0], ref b))
				{
					flag = false;
				}
				if (!this.ValidateByteValue(array[1], ref b2))
				{
					flag = false;
				}
				if (!this.ValidateByteValue(array[2], ref b3))
				{
					flag = false;
				}
				if (!this.ValidateByteValue(array[3], ref b4))
				{
					flag = false;
				}
				if (!flag)
				{
					((IModelValidationResultCollector)this.pageValidator).SetErrorText(ValidationErrorClass.FormatError, ipProperty, string.Format(Resources.ErrorGenValueOutOfRangeWithBorders, 0, 255));
					return false;
				}
			}
			IPAddress iPAddress;
			if (!IPAddress.TryParse((ipControl as MaskedTextBox).Text.Replace(" ", string.Empty), out iPAddress))
			{
				((IModelValidationResultCollector)this.pageValidator).SetErrorText(ValidationErrorClass.FormatError, ipProperty, Resources.InvalidIP);
				return false;
			}
			if (!onlyZerosAllowed && b == 0 && b2 == 0 && b3 == 0 && b4 == 0)
			{
				((IModelValidationResultCollector)this.pageValidator).SetErrorText(ValidationErrorClass.FormatError, ipProperty, string.Format(Resources.IpNotAllowed, iPAddress.ToString()));
				return false;
			}
			if (isSubnetMask && !SerialPortServices.IsValidSubnetMask((uint)b, (uint)b2, (uint)b3, (uint)b4))
			{
				((IModelValidationResultCollector)this.pageValidator).SetErrorText(ValidationErrorClass.FormatError, ipProperty, Resources.ErrorInvalidSubnetMask);
				return false;
			}
			bool flag2;
			this.pageValidator.Control.UpdateModel<string>(iPAddress.ToString(), ipProperty, this.guiElementManager.GetGUIElement(ipControl), out flag2);
			valueChanged |= flag2;
			return true;
		}

		public bool Serialize(WLANSettingsGL2000Page wlanSettingsGL2000Page)
		{
			if (wlanSettingsGL2000Page == null)
			{
				return false;
			}
			bool flag = true;
			return flag & this.threeGDataTransferTriggerGrid.Serialize<WLANSettingsGL2000Page>(wlanSettingsGL2000Page);
		}

		public bool DeSerialize(WLANSettingsGL2000Page wlanSettingsGL2000Page)
		{
			if (wlanSettingsGL2000Page == null)
			{
				return false;
			}
			bool flag = true;
			return flag & this.threeGDataTransferTriggerGrid.DeSerialize<WLANSettingsGL2000Page>(wlanSettingsGL2000Page);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(WLANSettingsGL2000));
			this.groupBox1 = new GroupBox();
			this.maskedTextBoxSubnetMask = new MaskedTextBox();
			this.label3 = new Label();
			this.maskedTextBoxGatewayIp = new MaskedTextBox();
			this.label4 = new Label();
			this.maskedTextBoxMlserverIp = new MaskedTextBox();
			this.label2 = new Label();
			this.maskedTextBoxLoggerIp = new MaskedTextBox();
			this.label1 = new Label();
			this.labelPartialDownloadInfo = new Label();
			this.comboBoxPartialDownload = new ComboBox();
			this.labelPartialDownload = new Label();
			this.comboBoxThreeGTransferEventType = new ComboBox();
			this.buttonRemoveThreeGTransferTrigger = new Button();
			this.buttonAddThreeGTransferTrigger = new Button();
			this.checkBoxStart3GConnectOn = new CheckBox();
			this.errorProviderFormat = new ErrorProvider(this.components);
			this.groupBox2 = new GroupBox();
			this.threeGDataTransferTriggerGrid = new DataTransferTriggerGrid();
			this.errorProviderLocalModel = new ErrorProvider(this.components);
			this.errorProviderGlobalModel = new ErrorProvider(this.components);
			this.groupBox1.SuspendLayout();
			((ISupportInitialize)this.errorProviderFormat).BeginInit();
			this.groupBox2.SuspendLayout();
			((ISupportInitialize)this.errorProviderLocalModel).BeginInit();
			((ISupportInitialize)this.errorProviderGlobalModel).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Controls.Add(this.maskedTextBoxSubnetMask);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.maskedTextBoxGatewayIp);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.maskedTextBoxMlserverIp);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.maskedTextBoxLoggerIp);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			this.maskedTextBoxSubnetMask.Culture = new CultureInfo("en-US");
			this.errorProviderLocalModel.SetIconAlignment(this.maskedTextBoxSubnetMask, (ErrorIconAlignment)componentResourceManager.GetObject("maskedTextBoxSubnetMask.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.maskedTextBoxSubnetMask, (ErrorIconAlignment)componentResourceManager.GetObject("maskedTextBoxSubnetMask.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.maskedTextBoxSubnetMask, (ErrorIconAlignment)componentResourceManager.GetObject("maskedTextBoxSubnetMask.IconAlignment2"));
			componentResourceManager.ApplyResources(this.maskedTextBoxSubnetMask, "maskedTextBoxSubnetMask");
			this.maskedTextBoxSubnetMask.Name = "maskedTextBoxSubnetMask";
			this.maskedTextBoxSubnetMask.ResetOnSpace = false;
			this.maskedTextBoxSubnetMask.Validating += new CancelEventHandler(this.maskedTextBoxSubnetMask_Validating);
			componentResourceManager.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			this.maskedTextBoxGatewayIp.Culture = new CultureInfo("en-US");
			this.errorProviderLocalModel.SetIconAlignment(this.maskedTextBoxGatewayIp, (ErrorIconAlignment)componentResourceManager.GetObject("maskedTextBoxGatewayIp.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.maskedTextBoxGatewayIp, (ErrorIconAlignment)componentResourceManager.GetObject("maskedTextBoxGatewayIp.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.maskedTextBoxGatewayIp, (ErrorIconAlignment)componentResourceManager.GetObject("maskedTextBoxGatewayIp.IconAlignment2"));
			componentResourceManager.ApplyResources(this.maskedTextBoxGatewayIp, "maskedTextBoxGatewayIp");
			this.maskedTextBoxGatewayIp.Name = "maskedTextBoxGatewayIp";
			this.maskedTextBoxGatewayIp.ResetOnSpace = false;
			this.maskedTextBoxGatewayIp.Validating += new CancelEventHandler(this.maskedTextBoxGatewayIp_Validating);
			componentResourceManager.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			this.maskedTextBoxMlserverIp.Culture = new CultureInfo("en-US");
			this.errorProviderLocalModel.SetIconAlignment(this.maskedTextBoxMlserverIp, (ErrorIconAlignment)componentResourceManager.GetObject("maskedTextBoxMlserverIp.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.maskedTextBoxMlserverIp, (ErrorIconAlignment)componentResourceManager.GetObject("maskedTextBoxMlserverIp.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.maskedTextBoxMlserverIp, (ErrorIconAlignment)componentResourceManager.GetObject("maskedTextBoxMlserverIp.IconAlignment2"));
			componentResourceManager.ApplyResources(this.maskedTextBoxMlserverIp, "maskedTextBoxMlserverIp");
			this.maskedTextBoxMlserverIp.Name = "maskedTextBoxMlserverIp";
			this.maskedTextBoxMlserverIp.ResetOnSpace = false;
			this.maskedTextBoxMlserverIp.Validating += new CancelEventHandler(this.maskedTextBoxMlserverIp_Validating);
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			this.maskedTextBoxLoggerIp.Culture = new CultureInfo("en-US");
			this.errorProviderLocalModel.SetIconAlignment(this.maskedTextBoxLoggerIp, (ErrorIconAlignment)componentResourceManager.GetObject("maskedTextBoxLoggerIp.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.maskedTextBoxLoggerIp, (ErrorIconAlignment)componentResourceManager.GetObject("maskedTextBoxLoggerIp.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.maskedTextBoxLoggerIp, (ErrorIconAlignment)componentResourceManager.GetObject("maskedTextBoxLoggerIp.IconAlignment2"));
			componentResourceManager.ApplyResources(this.maskedTextBoxLoggerIp, "maskedTextBoxLoggerIp");
			this.maskedTextBoxLoggerIp.Name = "maskedTextBoxLoggerIp";
			this.maskedTextBoxLoggerIp.ResetOnSpace = false;
			this.maskedTextBoxLoggerIp.Validating += new CancelEventHandler(this.maskedTextBoxLoggerIp_Validating);
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.labelPartialDownloadInfo, "labelPartialDownloadInfo");
			this.labelPartialDownloadInfo.Name = "labelPartialDownloadInfo";
			this.comboBoxPartialDownload.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxPartialDownload.FormattingEnabled = true;
			componentResourceManager.ApplyResources(this.comboBoxPartialDownload, "comboBoxPartialDownload");
			this.comboBoxPartialDownload.Name = "comboBoxPartialDownload";
			this.comboBoxPartialDownload.SelectedIndexChanged += new EventHandler(this.comboBoxPartialDownload_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.labelPartialDownload, "labelPartialDownload");
			this.labelPartialDownload.Name = "labelPartialDownload";
			this.comboBoxThreeGTransferEventType.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxThreeGTransferEventType.FormattingEnabled = true;
			componentResourceManager.ApplyResources(this.comboBoxThreeGTransferEventType, "comboBoxThreeGTransferEventType");
			this.comboBoxThreeGTransferEventType.Name = "comboBoxThreeGTransferEventType";
			componentResourceManager.ApplyResources(this.buttonRemoveThreeGTransferTrigger, "buttonRemoveThreeGTransferTrigger");
			this.buttonRemoveThreeGTransferTrigger.Name = "buttonRemoveThreeGTransferTrigger";
			this.buttonRemoveThreeGTransferTrigger.UseVisualStyleBackColor = true;
			this.buttonRemoveThreeGTransferTrigger.Click += new EventHandler(this.buttonRemoveThreeGTransferTrigger_Click);
			componentResourceManager.ApplyResources(this.buttonAddThreeGTransferTrigger, "buttonAddThreeGTransferTrigger");
			this.buttonAddThreeGTransferTrigger.Name = "buttonAddThreeGTransferTrigger";
			this.buttonAddThreeGTransferTrigger.UseVisualStyleBackColor = true;
			this.buttonAddThreeGTransferTrigger.Click += new EventHandler(this.buttonAddThree3TransferTrigger_Click);
			componentResourceManager.ApplyResources(this.checkBoxStart3GConnectOn, "checkBoxStart3GConnectOn");
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxStart3GConnectOn, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxStart3GConnectOn.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxStart3GConnectOn, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxStart3GConnectOn.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.checkBoxStart3GConnectOn, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxStart3GConnectOn.IconAlignment2"));
			this.checkBoxStart3GConnectOn.Name = "checkBoxStart3GConnectOn";
			this.checkBoxStart3GConnectOn.UseVisualStyleBackColor = true;
			this.checkBoxStart3GConnectOn.CheckedChanged += new EventHandler(this.checkBoxStart3GConnectOn_CheckedChanged);
			this.errorProviderFormat.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderFormat.ContainerControl = this;
			componentResourceManager.ApplyResources(this.groupBox2, "groupBox2");
			this.groupBox2.Controls.Add(this.checkBoxStart3GConnectOn);
			this.groupBox2.Controls.Add(this.labelPartialDownloadInfo);
			this.groupBox2.Controls.Add(this.threeGDataTransferTriggerGrid);
			this.groupBox2.Controls.Add(this.comboBoxPartialDownload);
			this.groupBox2.Controls.Add(this.buttonAddThreeGTransferTrigger);
			this.groupBox2.Controls.Add(this.labelPartialDownload);
			this.groupBox2.Controls.Add(this.buttonRemoveThreeGTransferTrigger);
			this.groupBox2.Controls.Add(this.comboBoxThreeGTransferEventType);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.TabStop = false;
			componentResourceManager.ApplyResources(this.threeGDataTransferTriggerGrid, "threeGDataTransferTriggerGrid");
			this.threeGDataTransferTriggerGrid.DataTransferTriggerConfiguration = null;
			this.threeGDataTransferTriggerGrid.ModelValidator = null;
			this.threeGDataTransferTriggerGrid.Name = "threeGDataTransferTriggerGrid";
			this.errorProviderLocalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderLocalModel.ContainerControl = this;
			this.errorProviderGlobalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderGlobalModel.ContainerControl = this;
			componentResourceManager.ApplyResources(this.errorProviderGlobalModel, "errorProviderGlobalModel");
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.groupBox2);
			base.Controls.Add(this.groupBox1);
			base.Name = "WLANSettingsGL2000";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((ISupportInitialize)this.errorProviderFormat).EndInit();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((ISupportInitialize)this.errorProviderLocalModel).EndInit();
			((ISupportInitialize)this.errorProviderGlobalModel).EndInit();
			base.ResumeLayout(false);
		}
	}
}
