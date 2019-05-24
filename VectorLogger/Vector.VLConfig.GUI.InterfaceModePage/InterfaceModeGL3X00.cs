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
using Vector.VLConfig.Properties;
using Vector.VLConfig.ValidationFramework;

namespace Vector.VLConfig.GUI.InterfaceModePage
{
	internal class InterfaceModeGL3X00 : UserControl
	{
		private GUIElementManager_Control guiElementManager;

		private CustomErrorProvider customErrorProvider;

		private PageValidator pageValidator;

		private bool isInitControls;

		private InterfaceModeConfiguration interfaceModeConfiguration;

		private DisplayMode displayMode;

		private IContainer components;

		private GroupBox groupBoxInterfaceMode;

		private Label labelPort;

		private Label labelSubnetMask;

		private Label labelIpAddress;

		private CheckBox checkBoxUseInterface;

		private MaskedTextBox maskedTextBoxIpAddress;

		private MaskedTextBox maskedTextBoxSubnetMask;

		private TextBox textBoxPort;

		private GroupBox groupBoxDesc;

		private Label labelDesc;

		private ErrorProvider errorProviderFormat;

		private ErrorProvider errorProviderLocalModel;

		private ErrorProvider errorProviderGlobalModel;

		private CheckBox checkBoxSendPhysTxEvents;

		private CheckBox checkBoxSendErrorFrames;

		private CheckBox checkBoxSendLoggedTxEvents;

		private Label labelTransmit;

		private TextBox textBoxMarkerMsgId;

		private Label labelMarker;

		private Label labelMsgId;

		private Label labelSendOnChannel;

		private ComboBox comboBoxMarkerChannel;

		public new event EventHandler Validating;

		public InterfaceModeConfiguration InterfaceModeConfiguration
		{
			get
			{
				return this.interfaceModeConfiguration;
			}
			set
			{
				this.interfaceModeConfiguration = value;
				this.UpdateGUI();
			}
		}

		public IModelValidator ModelValidator
		{
			get;
			set;
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
				if (this.interfaceModeConfiguration != null)
				{
					this.textBoxMarkerMsgId.Text = GUIUtil.CANIdToDisplayString(this.interfaceModeConfiguration.MarkerCANId.Value, this.interfaceModeConfiguration.IsMarkerCANIdExtended.Value);
					this.ValidateInput();
				}
			}
		}

		public InterfaceModeGL3X00()
		{
			this.InitializeComponent();
			this.guiElementManager = new GUIElementManager_Control();
			this.customErrorProvider = new CustomErrorProvider(this.errorProviderFormat);
			this.customErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.LocalModelError, this.errorProviderLocalModel);
			this.customErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.GlobalModelError, this.errorProviderGlobalModel);
			this.pageValidator = new PageValidator(this.customErrorProvider);
			this.isInitControls = false;
		}

		private void Raise_Validating(object sender, EventArgs e)
		{
			if (this.Validating != null)
			{
				this.Validating(sender, e);
			}
		}

		public void Init()
		{
			this.InitChannelCombobox();
		}

		private void InitChannelCombobox()
		{
			this.isInitControls = true;
			this.comboBoxMarkerChannel.Items.Clear();
			for (uint num = 1u; num <= this.ModelValidator.LoggerSpecifics.CAN.NumberOfChannels; num += 1u)
			{
				this.comboBoxMarkerChannel.Items.Add(GUIUtil.MapCANChannelNumber2String(num));
			}
			uint num2 = this.ModelValidator.LoggerSpecifics.CAN.NumberOfChannels + 1u;
			for (uint num3 = num2; num3 < this.ModelValidator.LoggerSpecifics.CAN.NumberOfVirtualChannels + num2; num3 += 1u)
			{
				this.comboBoxMarkerChannel.Items.Add(GUIUtil.MapCANChannelNumber2String(num3) + Resources.VirtualChannelPostfix);
			}
			if (this.comboBoxMarkerChannel.Items.Count > 0)
			{
				this.comboBoxMarkerChannel.SelectedIndex = 0;
			}
			this.isInitControls = false;
		}

		private void checkBoxUseInterface_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.EnableControls();
			this.UpdateDependentControls();
			this.ValidateInput();
		}

		private void maskedTextBox_KeyDown(object sender, KeyEventArgs e)
		{
			MaskedTextBox maskedTextBox = sender as MaskedTextBox;
			if (e.KeyCode == Keys.Decimal && maskedTextBox != null && maskedTextBox.MaskedTextProvider != null)
			{
				int selectionStart = maskedTextBox.SelectionStart;
				int num = maskedTextBox.MaskedTextProvider.Length - maskedTextBox.MaskedTextProvider.EditPositionCount;
				int num2 = 0;
				for (int i = 0; i < maskedTextBox.MaskedTextProvider.Length; i++)
				{
					if (!maskedTextBox.MaskedTextProvider.IsEditPosition(i) && selectionStart + num >= i)
					{
						num2 = i;
					}
				}
				num2++;
				maskedTextBox.SelectionStart = num2;
			}
		}

		private void maskedTextBox_KeyPress(object sender, KeyPressEventArgs e)
		{
			MaskedTextBox maskedTextBox = sender as MaskedTextBox;
			if (e.KeyChar == '.' && maskedTextBox != null && maskedTextBox.MaskedTextProvider != null)
			{
				int selectionStart = maskedTextBox.SelectionStart;
				int num = maskedTextBox.MaskedTextProvider.Length - maskedTextBox.MaskedTextProvider.EditPositionCount;
				int num2 = 0;
				for (int i = 0; i < maskedTextBox.MaskedTextProvider.Length; i++)
				{
					if (!maskedTextBox.MaskedTextProvider.IsEditPosition(i) && selectionStart + num >= i)
					{
						num2 = i;
					}
				}
				num2++;
				maskedTextBox.SelectionStart = num2;
			}
		}

		private void maskedTextBox_Validating(object sender, CancelEventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		private void textBoxPort_Validating(object sender, CancelEventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		private void textBoxMarkerMsgId_Validating(object sender, CancelEventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		private void comboBoxMarkerChannel_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		private void checkBoxSendPhysTxEvents_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		private void checkBoxSendLoggedTxEvents_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		private void checkBoxSendErrorFrames_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		public bool ValidateInput()
		{
			if (this.interfaceModeConfiguration == null)
			{
				return false;
			}
			bool flag = true;
			bool flag2 = false;
			this.pageValidator.General.ResetAllErrorProviders();
			this.pageValidator.General.ResetAllFormatErrors();
			bool flag3;
			flag &= this.pageValidator.Control.UpdateModel<bool>(this.checkBoxUseInterface.Checked, this.interfaceModeConfiguration.UseInterfaceMode, this.guiElementManager.GetGUIElement(this.checkBoxUseInterface), out flag3);
			flag2 |= flag3;
			if (this.interfaceModeConfiguration.UseInterfaceMode.Value)
			{
				flag &= this.ValidateIp(this.maskedTextBoxIpAddress, this.interfaceModeConfiguration.IpAddress, ref flag2, false, false);
				flag &= this.ValidateIp(this.maskedTextBoxSubnetMask, this.interfaceModeConfiguration.SubnetMask, ref flag2, true, true);
				flag &= this.pageValidator.Control.ValidateFormatAndUpdateModel_UInt32(this.textBoxPort.Text, this.interfaceModeConfiguration.Port, this.guiElementManager.GetGUIElement(this.textBoxPort), out flag3);
				flag2 |= flag3;
				flag &= this.pageValidator.Control.ValidateFormatAndUpdateModel_CANId(this.textBoxMarkerMsgId.Text, this.interfaceModeConfiguration.MarkerCANId, this.interfaceModeConfiguration.IsMarkerCANIdExtended, this.guiElementManager.GetGUIElement(this.textBoxMarkerMsgId), out flag3);
				flag2 |= flag3;
				uint value = GUIUtil.MapCANChannelString2Number(this.comboBoxMarkerChannel.SelectedItem.ToString());
				flag &= this.pageValidator.Control.UpdateModel<uint>(value, this.interfaceModeConfiguration.MarkerChannel, this.guiElementManager.GetGUIElement(this.comboBoxMarkerChannel), out flag3);
				flag2 |= flag3;
				flag &= this.pageValidator.Control.UpdateModel<bool>(this.checkBoxSendPhysTxEvents.Checked, this.interfaceModeConfiguration.IsSendPhysTxActive, this.guiElementManager.GetGUIElement(this.checkBoxSendPhysTxEvents), out flag3);
				flag2 |= flag3;
				flag &= this.pageValidator.Control.UpdateModel<bool>(this.checkBoxSendLoggedTxEvents.Checked, this.interfaceModeConfiguration.IsSendLoggedTxActive, this.guiElementManager.GetGUIElement(this.checkBoxSendLoggedTxEvents), out flag3);
				flag2 |= flag3;
				flag &= this.pageValidator.Control.UpdateModel<bool>(this.checkBoxSendErrorFrames.Checked, this.interfaceModeConfiguration.IsSendErrorFramesActive, this.guiElementManager.GetGUIElement(this.checkBoxSendErrorFrames), out flag3);
				flag2 |= flag3;
				flag &= this.ModelValidator.Validate(this.interfaceModeConfiguration, flag2, this.pageValidator);
				this.pageValidator.General.ActivateErrorProvidersForFormatAndModelErrors();
			}
			this.Raise_Validating(this, EventArgs.Empty);
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

		private void EnableControls()
		{
			bool @checked = this.checkBoxUseInterface.Checked;
			this.maskedTextBoxIpAddress.Enabled = @checked;
			this.maskedTextBoxSubnetMask.Enabled = @checked;
			this.textBoxPort.Enabled = @checked;
			this.textBoxMarkerMsgId.Enabled = @checked;
			this.comboBoxMarkerChannel.Enabled = @checked;
			this.checkBoxSendPhysTxEvents.Enabled = @checked;
			this.checkBoxSendLoggedTxEvents.Enabled = @checked;
			this.checkBoxSendErrorFrames.Enabled = @checked;
		}

		private void UpdateGUI()
		{
			if (this.interfaceModeConfiguration == null)
			{
				return;
			}
			this.isInitControls = true;
			this.checkBoxUseInterface.Checked = this.interfaceModeConfiguration.UseInterfaceMode.Value;
			this.EnableControls();
			this.UpdateDependentControls();
			this.isInitControls = false;
			this.ValidateInput();
		}

		private void UpdateDependentControls()
		{
			this.isInitControls = true;
			if (!this.pageValidator.General.HasFormatError(this.interfaceModeConfiguration.IpAddress))
			{
				this.SetIP(this.maskedTextBoxIpAddress, this.interfaceModeConfiguration.IpAddress);
			}
			if (!this.pageValidator.General.HasFormatError(this.interfaceModeConfiguration.SubnetMask))
			{
				this.SetIP(this.maskedTextBoxSubnetMask, this.interfaceModeConfiguration.SubnetMask);
			}
			if (!this.pageValidator.General.HasFormatError(this.interfaceModeConfiguration.Port))
			{
				this.textBoxPort.Text = this.interfaceModeConfiguration.Port.Value.ToString();
			}
			if (!this.pageValidator.General.HasFormatError(this.interfaceModeConfiguration.MarkerCANId))
			{
				this.textBoxMarkerMsgId.Text = GUIUtil.CANIdToDisplayString(this.interfaceModeConfiguration.MarkerCANId.Value, this.interfaceModeConfiguration.IsMarkerCANIdExtended.Value);
			}
			if (!this.pageValidator.General.HasFormatError(this.interfaceModeConfiguration.MarkerChannel))
			{
				if (this.interfaceModeConfiguration.MarkerChannel.Value <= this.ModelValidator.LoggerSpecifics.CAN.NumberOfChannels)
				{
					this.comboBoxMarkerChannel.SelectedItem = GUIUtil.MapCANChannelNumber2String(this.interfaceModeConfiguration.MarkerChannel.Value);
				}
				else
				{
					this.comboBoxMarkerChannel.SelectedItem = GUIUtil.MapCANChannelNumber2String(this.interfaceModeConfiguration.MarkerChannel.Value) + Resources.VirtualChannelPostfix;
				}
			}
			if (!this.pageValidator.General.HasFormatError(this.interfaceModeConfiguration.IsSendPhysTxActive))
			{
				this.checkBoxSendPhysTxEvents.Checked = this.interfaceModeConfiguration.IsSendPhysTxActive.Value;
			}
			if (!this.pageValidator.General.HasFormatError(this.interfaceModeConfiguration.IsSendLoggedTxActive))
			{
				this.checkBoxSendLoggedTxEvents.Checked = this.interfaceModeConfiguration.IsSendLoggedTxActive.Value;
			}
			if (!this.pageValidator.General.HasFormatError(this.interfaceModeConfiguration.IsSendErrorFramesActive))
			{
				this.checkBoxSendErrorFrames.Checked = this.interfaceModeConfiguration.IsSendErrorFramesActive.Value;
			}
			this.isInitControls = false;
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

		private static bool ValidateByteValue(string textValue, ref uint value)
		{
			return uint.TryParse(textValue, out value) && value <= 255u;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(InterfaceModeGL3X00));
			this.groupBoxInterfaceMode = new GroupBox();
			this.comboBoxMarkerChannel = new ComboBox();
			this.labelSendOnChannel = new Label();
			this.labelMsgId = new Label();
			this.textBoxMarkerMsgId = new TextBox();
			this.labelMarker = new Label();
			this.labelTransmit = new Label();
			this.checkBoxSendErrorFrames = new CheckBox();
			this.checkBoxSendLoggedTxEvents = new CheckBox();
			this.checkBoxSendPhysTxEvents = new CheckBox();
			this.textBoxPort = new TextBox();
			this.maskedTextBoxSubnetMask = new MaskedTextBox();
			this.maskedTextBoxIpAddress = new MaskedTextBox();
			this.labelPort = new Label();
			this.labelSubnetMask = new Label();
			this.labelIpAddress = new Label();
			this.checkBoxUseInterface = new CheckBox();
			this.groupBoxDesc = new GroupBox();
			this.labelDesc = new Label();
			this.errorProviderFormat = new ErrorProvider();
			this.errorProviderLocalModel = new ErrorProvider();
			this.errorProviderGlobalModel = new ErrorProvider();
			this.groupBoxInterfaceMode.SuspendLayout();
			this.groupBoxDesc.SuspendLayout();
			((ISupportInitialize)this.errorProviderFormat).BeginInit();
			((ISupportInitialize)this.errorProviderLocalModel).BeginInit();
			((ISupportInitialize)this.errorProviderGlobalModel).BeginInit();
			base.SuspendLayout();
			this.groupBoxInterfaceMode.Controls.Add(this.comboBoxMarkerChannel);
			this.groupBoxInterfaceMode.Controls.Add(this.labelSendOnChannel);
			this.groupBoxInterfaceMode.Controls.Add(this.labelMsgId);
			this.groupBoxInterfaceMode.Controls.Add(this.textBoxMarkerMsgId);
			this.groupBoxInterfaceMode.Controls.Add(this.labelMarker);
			this.groupBoxInterfaceMode.Controls.Add(this.labelTransmit);
			this.groupBoxInterfaceMode.Controls.Add(this.checkBoxSendErrorFrames);
			this.groupBoxInterfaceMode.Controls.Add(this.checkBoxSendLoggedTxEvents);
			this.groupBoxInterfaceMode.Controls.Add(this.checkBoxSendPhysTxEvents);
			this.groupBoxInterfaceMode.Controls.Add(this.textBoxPort);
			this.groupBoxInterfaceMode.Controls.Add(this.maskedTextBoxSubnetMask);
			this.groupBoxInterfaceMode.Controls.Add(this.maskedTextBoxIpAddress);
			this.groupBoxInterfaceMode.Controls.Add(this.labelPort);
			this.groupBoxInterfaceMode.Controls.Add(this.labelSubnetMask);
			this.groupBoxInterfaceMode.Controls.Add(this.labelIpAddress);
			this.groupBoxInterfaceMode.Controls.Add(this.checkBoxUseInterface);
			componentResourceManager.ApplyResources(this.groupBoxInterfaceMode, "groupBoxInterfaceMode");
			this.groupBoxInterfaceMode.Name = "groupBoxInterfaceMode";
			this.groupBoxInterfaceMode.TabStop = false;
			this.comboBoxMarkerChannel.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxMarkerChannel.FormattingEnabled = true;
			this.errorProviderGlobalModel.SetIconAlignment(this.comboBoxMarkerChannel, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxMarkerChannel.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.comboBoxMarkerChannel, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxMarkerChannel.IconAlignment1"));
			this.errorProviderLocalModel.SetIconAlignment(this.comboBoxMarkerChannel, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxMarkerChannel.IconAlignment2"));
			componentResourceManager.ApplyResources(this.comboBoxMarkerChannel, "comboBoxMarkerChannel");
			this.comboBoxMarkerChannel.Name = "comboBoxMarkerChannel";
			this.comboBoxMarkerChannel.SelectedIndexChanged += new EventHandler(this.comboBoxMarkerChannel_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.labelSendOnChannel, "labelSendOnChannel");
			this.labelSendOnChannel.Name = "labelSendOnChannel";
			componentResourceManager.ApplyResources(this.labelMsgId, "labelMsgId");
			this.labelMsgId.Name = "labelMsgId";
			this.errorProviderFormat.SetIconAlignment(this.textBoxMarkerMsgId, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxMarkerMsgId.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxMarkerMsgId, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxMarkerMsgId.IconAlignment1"));
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxMarkerMsgId, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxMarkerMsgId.IconAlignment2"));
			componentResourceManager.ApplyResources(this.textBoxMarkerMsgId, "textBoxMarkerMsgId");
			this.textBoxMarkerMsgId.Name = "textBoxMarkerMsgId";
			this.textBoxMarkerMsgId.Validating += new CancelEventHandler(this.textBoxMarkerMsgId_Validating);
			componentResourceManager.ApplyResources(this.labelMarker, "labelMarker");
			this.labelMarker.Name = "labelMarker";
			componentResourceManager.ApplyResources(this.labelTransmit, "labelTransmit");
			this.labelTransmit.Name = "labelTransmit";
			componentResourceManager.ApplyResources(this.checkBoxSendErrorFrames, "checkBoxSendErrorFrames");
			this.checkBoxSendErrorFrames.Name = "checkBoxSendErrorFrames";
			this.checkBoxSendErrorFrames.UseVisualStyleBackColor = true;
			this.checkBoxSendErrorFrames.CheckedChanged += new EventHandler(this.checkBoxSendErrorFrames_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxSendLoggedTxEvents, "checkBoxSendLoggedTxEvents");
			this.checkBoxSendLoggedTxEvents.Name = "checkBoxSendLoggedTxEvents";
			this.checkBoxSendLoggedTxEvents.UseVisualStyleBackColor = true;
			this.checkBoxSendLoggedTxEvents.CheckedChanged += new EventHandler(this.checkBoxSendLoggedTxEvents_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxSendPhysTxEvents, "checkBoxSendPhysTxEvents");
			this.checkBoxSendPhysTxEvents.Name = "checkBoxSendPhysTxEvents";
			this.checkBoxSendPhysTxEvents.UseVisualStyleBackColor = true;
			this.checkBoxSendPhysTxEvents.CheckedChanged += new EventHandler(this.checkBoxSendPhysTxEvents_CheckedChanged);
			this.errorProviderFormat.SetIconAlignment(this.textBoxPort, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxPort.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxPort, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxPort.IconAlignment1"));
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxPort, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxPort.IconAlignment2"));
			componentResourceManager.ApplyResources(this.textBoxPort, "textBoxPort");
			this.textBoxPort.Name = "textBoxPort";
			this.textBoxPort.Validating += new CancelEventHandler(this.textBoxPort_Validating);
			this.maskedTextBoxSubnetMask.Culture = new CultureInfo("en-US");
			this.errorProviderGlobalModel.SetIconAlignment(this.maskedTextBoxSubnetMask, (ErrorIconAlignment)componentResourceManager.GetObject("maskedTextBoxSubnetMask.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.maskedTextBoxSubnetMask, (ErrorIconAlignment)componentResourceManager.GetObject("maskedTextBoxSubnetMask.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.maskedTextBoxSubnetMask, (ErrorIconAlignment)componentResourceManager.GetObject("maskedTextBoxSubnetMask.IconAlignment2"));
			componentResourceManager.ApplyResources(this.maskedTextBoxSubnetMask, "maskedTextBoxSubnetMask");
			this.maskedTextBoxSubnetMask.Name = "maskedTextBoxSubnetMask";
			this.maskedTextBoxSubnetMask.KeyDown += new KeyEventHandler(this.maskedTextBox_KeyDown);
			this.maskedTextBoxSubnetMask.KeyPress += new KeyPressEventHandler(this.maskedTextBox_KeyPress);
			this.maskedTextBoxSubnetMask.Validating += new CancelEventHandler(this.maskedTextBox_Validating);
			this.maskedTextBoxIpAddress.Culture = new CultureInfo("en-US");
			this.errorProviderGlobalModel.SetIconAlignment(this.maskedTextBoxIpAddress, (ErrorIconAlignment)componentResourceManager.GetObject("maskedTextBoxIpAddress.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.maskedTextBoxIpAddress, (ErrorIconAlignment)componentResourceManager.GetObject("maskedTextBoxIpAddress.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this.maskedTextBoxIpAddress, (ErrorIconAlignment)componentResourceManager.GetObject("maskedTextBoxIpAddress.IconAlignment2"));
			componentResourceManager.ApplyResources(this.maskedTextBoxIpAddress, "maskedTextBoxIpAddress");
			this.maskedTextBoxIpAddress.Name = "maskedTextBoxIpAddress";
			this.maskedTextBoxIpAddress.ResetOnSpace = false;
			this.maskedTextBoxIpAddress.KeyDown += new KeyEventHandler(this.maskedTextBox_KeyDown);
			this.maskedTextBoxIpAddress.KeyPress += new KeyPressEventHandler(this.maskedTextBox_KeyPress);
			this.maskedTextBoxIpAddress.Validating += new CancelEventHandler(this.maskedTextBox_Validating);
			componentResourceManager.ApplyResources(this.labelPort, "labelPort");
			this.labelPort.Name = "labelPort";
			componentResourceManager.ApplyResources(this.labelSubnetMask, "labelSubnetMask");
			this.labelSubnetMask.Name = "labelSubnetMask";
			componentResourceManager.ApplyResources(this.labelIpAddress, "labelIpAddress");
			this.labelIpAddress.Name = "labelIpAddress";
			componentResourceManager.ApplyResources(this.checkBoxUseInterface, "checkBoxUseInterface");
			this.checkBoxUseInterface.Name = "checkBoxUseInterface";
			this.checkBoxUseInterface.UseVisualStyleBackColor = true;
			this.checkBoxUseInterface.CheckedChanged += new EventHandler(this.checkBoxUseInterface_CheckedChanged);
			this.groupBoxDesc.Controls.Add(this.labelDesc);
			componentResourceManager.ApplyResources(this.groupBoxDesc, "groupBoxDesc");
			this.groupBoxDesc.Name = "groupBoxDesc";
			this.groupBoxDesc.TabStop = false;
			componentResourceManager.ApplyResources(this.labelDesc, "labelDesc");
			this.labelDesc.Name = "labelDesc";
			this.errorProviderFormat.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderFormat.ContainerControl = this;
			this.errorProviderLocalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderLocalModel.ContainerControl = this;
			this.errorProviderGlobalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderGlobalModel.ContainerControl = this;
			componentResourceManager.ApplyResources(this.errorProviderGlobalModel, "errorProviderGlobalModel");
			base.AutoScaleMode = AutoScaleMode.Inherit;
			base.Controls.Add(this.groupBoxDesc);
			base.Controls.Add(this.groupBoxInterfaceMode);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "InterfaceModeGL3X00";
			this.groupBoxInterfaceMode.ResumeLayout(false);
			this.groupBoxInterfaceMode.PerformLayout();
			this.groupBoxDesc.ResumeLayout(false);
			((ISupportInitialize)this.errorProviderFormat).EndInit();
			((ISupportInitialize)this.errorProviderLocalModel).EndInit();
			((ISupportInitialize)this.errorProviderGlobalModel).EndInit();
			base.ResumeLayout(false);
		}
	}
}
