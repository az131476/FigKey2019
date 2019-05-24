using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.DiagSymbolsAccess;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;
using Vector.VLConfig.ValidationFramework;

namespace Vector.VLConfig.GUI.DiagnosticsDatabasesPage
{
	internal class EcuParameters : UserControl
	{
		private DiagnosticCommParamsECU commParamsEcu;

		private GUIElementManager_Control guiElementManager;

		private CustomErrorProvider customErrorProvider;

		private PageValidator pageValidator;

		private bool isInitControls;

		private Dictionary<string, string> displayedNameToQualifier;

		private Dictionary<string, ulong> sessionNameToId;

		private string variantQualifier;

		private IContainer components;

		private Label labelEcu;

		private CheckBox checkBoxUseDbParams;

		private TextBox textBoxP2Timeout;

		private Label labelP2Extension;

		private Label labelP2Timeout;

		private Label labelMs2;

		private Label labelMs;

		private TextBox textBoxP2Extension;

		private Label labelRespCANId;

		private Label labelPhysReqCANId;

		private Label labelMs4;

		private TextBox textBoxSTMin;

		private TextBox textBoxRespCANId;

		private TextBox textBoxPhysReqCANId;

		private Label labelSTMin;

		private ErrorProvider errorProviderFormat;

		private ErrorProvider errorProviderLocalModel;

		private GroupBox groupBoxCommParams;

		private ComboBox comboBoxCommInterfaces;

		private CheckBox checkBoxUsePaddingBytes;

		private TextBox textBoxPaddingByteValue;

		private Label labelFirstConsecValue;

		private ComboBox comboBoxFirstConsecFrameValue;

		private ToolTip toolTip;

		private ErrorProvider errorProviderGlobalModel;

		private GroupBox groupBoxOemSpecific;

		private CheckBox checkBoxUseStopCommRequest;

		private GroupBox groupBoxSessions;

		private ComboBox comboBoxExtendedSession;

		private ComboBox comboBoxDefaultSession;

		private Label labelExtendedSession;

		private Label labelDefaultSession;

		private TextBox textBoxExtendedSession;

		private TextBox textBoxDefaultSession;

		private ComboBox comboBoxDiagProtocol;

		private Label labelDiagProtocol;

		private Label labelCommInterface;

		private Label labelIdExt;

		private Label labelIdDefault;

		private TextBox textBoxRespAddrExt;

		private Label labelSlash2;

		private ComboBox comboBoxAddressingMode;

		private Label labelAddrMode;

		private Label labelSlash1;

		private TextBox textBoxPhysReqAddrExt;

		public event EventHandler DataChanged;

		public IModelValidator ModelValidator
		{
			get;
			set;
		}

		public string EcuQualifier
		{
			get;
			set;
		}

		public string VariantQualifier
		{
			get
			{
				return this.variantQualifier;
			}
			set
			{
				this.variantQualifier = value;
			}
		}

		public DiagnosticCommParamsECU DiagnosticCommParamsECU
		{
			get
			{
				return this.commParamsEcu;
			}
			set
			{
				this.commParamsEcu = value;
			}
		}

		public DiagnosticsDatabaseConfigurationInternal DiagnosticsDatabaseConfiguration
		{
			get;
			set;
		}

		public IDiagSymbolsManager DiagSymbolsManager
		{
			get;
			set;
		}

		public PageValidator PageValidator
		{
			get
			{
				return this.pageValidator;
			}
		}

		public EcuParameters()
		{
			this.InitializeComponent();
			this.isInitControls = false;
			this.variantQualifier = "";
			this.guiElementManager = new GUIElementManager_Control();
			this.customErrorProvider = new CustomErrorProvider(this.errorProviderFormat);
			this.customErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.LocalModelError, this.errorProviderLocalModel);
			this.customErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.GlobalModelError, this.errorProviderGlobalModel);
			this.pageValidator = new PageValidator(this.customErrorProvider);
			this.displayedNameToQualifier = new Dictionary<string, string>();
			this.sessionNameToId = new Dictionary<string, ulong>();
		}

		private void Raise_DataChanged(object sender, EventArgs e)
		{
			if (this.DataChanged != null)
			{
				this.DataChanged(sender, e);
			}
		}

		private void checkBoxUseDbParams_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			if (this.checkBoxUseDbParams.Checked)
			{
				if (this.InitCommInterfacesCombobox(true) > 0)
				{
					this.UpdateControlsForSelectedCommInterface();
				}
				else
				{
					this.EnableParameterControls();
					this.UpdateParameterControls();
				}
				this.DiagSymbolsManager.GetSessionControlParamValuesFromDb(this.EcuQualifier, this.VariantQualifier, ref this.commParamsEcu);
			}
			else
			{
				this.InitCommInterfacesCombobox(false);
				this.EnableParameterControls();
				this.UpdateParameterControls();
			}
			this.InitSessionControls();
			this.UpdateSessionControls();
			this.ValidateInput();
		}

		private void textBoxCommParameter_Validating(object sender, CancelEventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			if (!this.commParamsEcu.UseParamValuesFromDb.Value && this.comboBoxCommInterfaces.SelectedItem.ToString() != Resources.UserDefined)
			{
				this.isInitControls = true;
				this.comboBoxCommInterfaces.SelectedItem = Resources.UserDefined;
				this.isInitControls = false;
			}
			this.ValidateInput();
		}

		private void comboBoxCommInterfaces_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.UpdateControlsForSelectedCommInterface();
			this.ValidateInput();
		}

		private void comboBoxCommInterfaces_MouseEnter(object sender, EventArgs e)
		{
			if (this.comboBoxCommInterfaces.SelectedIndex >= 0)
			{
				this.toolTip.SetToolTip(this.comboBoxCommInterfaces, this.comboBoxCommInterfaces.SelectedItem.ToString());
			}
		}

		private void comboBoxDiagProtocol_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		private void comboBoxAddressingMode_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			if (this.comboBoxAddressingMode.SelectedItem.ToString() == GUIUtil.MapDiagnosticsAddressingMode2String(DiagnosticsAddressingMode.Extended))
			{
				this.textBoxPhysReqAddrExt.Enabled = true;
				this.textBoxPhysReqAddrExt.Text = GUIUtil.NumberToDisplayString(this.commParamsEcu.ExtAddressingModeRqExtension.Value);
				this.textBoxRespAddrExt.Enabled = true;
				this.textBoxRespAddrExt.Text = GUIUtil.NumberToDisplayString(this.commParamsEcu.ExtAddressingModeRsExtension.Value);
				this.textBoxPhysReqAddrExt.ReadOnly = (this.commParamsEcu.UseParamValuesFromDb.Value && this.commParamsEcu.HasExtAddressingModeRqExtension);
				this.textBoxRespAddrExt.ReadOnly = (this.commParamsEcu.UseParamValuesFromDb.Value && this.commParamsEcu.HasExtAddressingModeRsExtension);
			}
			else
			{
				this.textBoxPhysReqAddrExt.Enabled = false;
				this.textBoxPhysReqAddrExt.Text = "";
				this.textBoxRespAddrExt.Enabled = false;
				this.textBoxRespAddrExt.Text = "";
			}
			this.ValidateInput();
		}

		private void checkBoxUsePaddingBytes_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.textBoxPaddingByteValue.Enabled = this.checkBoxUsePaddingBytes.Checked;
			if (!this.textBoxPaddingByteValue.Enabled && !string.IsNullOrEmpty(this.errorProviderFormat.GetError(this.textBoxPaddingByteValue)))
			{
				this.textBoxPaddingByteValue.Text = GUIUtil.NumberToDisplayStringHex((ulong)this.commParamsEcu.PaddingByteValue.Value, 1u);
			}
			this.ValidateInput();
		}

		private void comboBoxFirstConsecFrameValue_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		private void checkBoxUseStopCommRequest_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		private void comboBoxDefaultSession_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
			if (this.commParamsEcu.UseParamValuesFromDb.Value)
			{
				this.textBoxDefaultSession.ReadOnly = (this.commParamsEcu.DefaultSessionSource.Value != DiagnosticsSessionSource.UserDefined);
				return;
			}
			this.textBoxDefaultSession.ReadOnly = (this.commParamsEcu.DefaultSessionSource.Value == DiagnosticsSessionSource.KWP_Default || this.commParamsEcu.DefaultSessionSource.Value == DiagnosticsSessionSource.UDS_Default);
		}

		private void textBoxDefaultSession_Validating(object sender, CancelEventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			if (!this.commParamsEcu.UseParamValuesFromDb.Value && this.commParamsEcu.DefaultSessionSource.Value == DiagnosticsSessionSource.UserDefined)
			{
				this.isInitControls = true;
				this.comboBoxDefaultSession.SelectedItem = GUIUtil.MapDiagSessionSourceToString(DiagnosticsSessionSource.UserDefined);
				this.isInitControls = false;
			}
			this.ValidateInput();
		}

		private void comboBoxExtendedSession_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
			this.textBoxExtendedSession.ReadOnly = (this.commParamsEcu.ExtendedSessionSource.Value != DiagnosticsSessionSource.UserDefined);
		}

		private void textBoxExtendedSession_Validating(object sender, CancelEventArgs e)
		{
			if (!this.commParamsEcu.UseParamValuesFromDb.Value && this.commParamsEcu.ExtendedSessionSource.Value == DiagnosticsSessionSource.UserDefined)
			{
				this.isInitControls = true;
				this.comboBoxExtendedSession.SelectedItem = GUIUtil.MapDiagSessionSourceToString(DiagnosticsSessionSource.UserDefined);
				this.isInitControls = false;
			}
			this.ValidateInput();
		}

		public void UpdateControls()
		{
			this.UpdateStaticLabels();
			this.InitDiagProtocolsCombobox();
			this.InitDiagAddrModeCombobox();
			this.InitFirstConsecFrameCombobox();
			this.isInitControls = true;
			this.checkBoxUseDbParams.Checked = this.commParamsEcu.UseParamValuesFromDb.Value;
			this.isInitControls = false;
			this.InitSessionControls();
			if (this.checkBoxUseDbParams.Checked)
			{
				if (this.InitCommInterfacesCombobox(true) > 0)
				{
					this.UpdateControlsForSelectedCommInterface();
				}
				else
				{
					this.EnableParameterControls();
					this.UpdateParameterControls();
				}
			}
			else
			{
				this.InitCommInterfacesCombobox(false);
				this.EnableParameterControls();
				this.UpdateParameterControls();
			}
			this.UpdateSessionControls();
			this.ValidateInput();
		}

		public bool ValidateInput()
		{
			if (this.commParamsEcu == null)
			{
				return true;
			}
			bool flag = true;
			bool flag2 = false;
			this.ResetValidationFramework();
			this.ResetErrorProvider();
			bool flag3;
			flag &= this.pageValidator.Control.UpdateModel<bool>(this.checkBoxUseDbParams.Checked, this.commParamsEcu.UseParamValuesFromDb, this.guiElementManager.GetGUIElement(this.checkBoxUseDbParams), out flag3);
			flag2 |= flag3;
			if (this.checkBoxUseDbParams.Checked)
			{
				string value = "";
				if (this.comboBoxCommInterfaces.SelectedItem.ToString() != Resources.UserDefined && this.displayedNameToQualifier.ContainsKey(this.comboBoxCommInterfaces.SelectedItem.ToString()))
				{
					value = this.displayedNameToQualifier[this.comboBoxCommInterfaces.SelectedItem.ToString()];
				}
				flag &= this.pageValidator.Control.UpdateModel<string>(value, this.commParamsEcu.InterfaceQualifier, this.guiElementManager.GetGUIElement(this.comboBoxCommInterfaces), out flag3);
				flag2 |= flag3;
			}
			else
			{
				flag &= this.pageValidator.Control.UpdateModel<string>("", this.commParamsEcu.InterfaceQualifier, this.guiElementManager.GetGUIElement(this.comboBoxCommInterfaces), out flag3);
				flag2 |= flag3;
			}
			flag &= this.pageValidator.Control.UpdateModel<DiagnosticsProtocolType>(GUIUtil.MapString2DiagnosticsProtocol(this.comboBoxDiagProtocol.SelectedItem.ToString()), this.commParamsEcu.DiagProtocol, this.guiElementManager.GetGUIElement(this.comboBoxDiagProtocol), out flag3);
			flag2 |= flag3;
			flag &= this.pageValidator.Control.ValidateFormatAndUpdateModel_UInt32(this.textBoxP2Timeout.Text, this.commParamsEcu.P2Timeout, this.guiElementManager.GetGUIElement(this.textBoxP2Timeout), out flag3);
			flag2 |= flag3;
			flag &= this.pageValidator.Control.ValidateFormatAndUpdateModel_UInt32(this.textBoxP2Extension.Text, this.commParamsEcu.P2Extension, this.guiElementManager.GetGUIElement(this.textBoxP2Extension), out flag3);
			flag2 |= flag3;
			flag &= this.pageValidator.Control.ValidateFormatAndUpdateModel_CANId(this.textBoxPhysReqCANId.Text, this.commParamsEcu.PhysRequestMsgId, this.commParamsEcu.PhysRequestMsgIsExtendedId, this.guiElementManager.GetGUIElement(this.textBoxPhysReqCANId), out flag3);
			flag2 |= flag3;
			flag &= this.pageValidator.Control.ValidateFormatAndUpdateModel_CANId(this.textBoxRespCANId.Text, this.commParamsEcu.ResponseMsgId, this.commParamsEcu.ResponseMsgIsExtendedId, this.guiElementManager.GetGUIElement(this.textBoxRespCANId), out flag3);
			flag2 |= flag3;
			flag &= this.pageValidator.Control.UpdateModel<DiagnosticsAddressingMode>(GUIUtil.MapString2DiagnosticsAddressingMode(this.comboBoxAddressingMode.SelectedItem.ToString()), this.commParamsEcu.DiagAddressingMode, this.guiElementManager.GetGUIElement(this.comboBoxAddressingMode), out flag3);
			flag2 |= flag3;
			if (this.commParamsEcu.DiagAddressingMode.Value == DiagnosticsAddressingMode.Extended)
			{
				ValidatedProperty<bool> dataModelIsExtendedCANId = new ValidatedProperty<bool>(false);
				flag &= this.pageValidator.Control.ValidateFormatAndUpdateModel_CANId(this.textBoxPhysReqAddrExt.Text, this.commParamsEcu.ExtAddressingModeRqExtension, dataModelIsExtendedCANId, this.guiElementManager.GetGUIElement(this.textBoxPhysReqAddrExt), out flag3);
				flag2 |= flag3;
				flag &= this.pageValidator.Control.ValidateFormatAndUpdateModel_CANId(this.textBoxRespAddrExt.Text, this.commParamsEcu.ExtAddressingModeRsExtension, dataModelIsExtendedCANId, this.guiElementManager.GetGUIElement(this.textBoxRespAddrExt), out flag3);
				flag2 |= flag3;
			}
			flag &= this.pageValidator.Control.ValidateFormatAndUpdateModel_UInt32(this.textBoxSTMin.Text, this.commParamsEcu.STMin, this.guiElementManager.GetGUIElement(this.textBoxSTMin), out flag3);
			flag2 |= flag3;
			flag &= this.pageValidator.Control.UpdateModel<bool>(this.checkBoxUsePaddingBytes.Checked, this.commParamsEcu.UsePaddingBytes, this.guiElementManager.GetGUIElement(this.checkBoxUsePaddingBytes), out flag3);
			flag2 |= flag3;
			if (this.checkBoxUsePaddingBytes.Checked)
			{
				if (!this.pageValidator.Control.ValidateFormatAndUpdateModel_Byte(this.textBoxPaddingByteValue.Text, this.commParamsEcu.PaddingByteValue, this.guiElementManager.GetGUIElement(this.textBoxPaddingByteValue), out flag3))
				{
					flag = false;
				}
				else
				{
					this.textBoxPaddingByteValue.Text = GUIUtil.NumberToDisplayStringHex((ulong)this.commParamsEcu.PaddingByteValue.Value, 1u);
				}
				flag2 |= flag3;
			}
			string text = this.comboBoxFirstConsecFrameValue.SelectedItem.ToString();
			text = text.Substring(text.Length - 1);
			if (!this.pageValidator.Control.ValidateFormatAndUpdateModel_Byte(text, this.commParamsEcu.FirstConsecFrameValue, this.guiElementManager.GetGUIElement(this.comboBoxFirstConsecFrameValue), out flag3))
			{
				flag = false;
			}
			flag2 |= flag3;
			flag &= this.pageValidator.Control.UpdateModel<bool>(this.checkBoxUseStopCommRequest.Checked, this.commParamsEcu.UseStopCommRequest, this.guiElementManager.GetGUIElement(this.checkBoxUseStopCommRequest), out flag3);
			flag2 |= flag3;
			string text2;
			DiagnosticsSessionSource diagnosticsSessionSource = this.GetSelectedSession(this.comboBoxDefaultSession, out text2);
			if (!this.commParamsEcu.UseParamValuesFromDb.Value && diagnosticsSessionSource == DiagnosticsSessionSource.DatabaseDefined)
			{
				ulong num = this.GetFixedSessionId(diagnosticsSessionSource, text2);
				diagnosticsSessionSource = DiagnosticsSessionSource.UserDefined;
				flag &= this.pageValidator.Control.UpdateModel<DiagnosticsSessionSource>(diagnosticsSessionSource, this.commParamsEcu.DefaultSessionSource, this.guiElementManager.GetGUIElement(this.comboBoxDefaultSession), out flag3);
				flag2 |= flag3;
				this.isInitControls = true;
				this.textBoxDefaultSession.Text = GUIUtil.NumberToDisplayStringHex(num, 0u);
				this.isInitControls = false;
				flag &= this.pageValidator.Control.UpdateModel<ulong>(num, this.commParamsEcu.DefaultSessionId, this.guiElementManager.GetGUIElement(this.textBoxDefaultSession), out flag3);
			}
			else
			{
				flag &= this.pageValidator.Control.UpdateModel<DiagnosticsSessionSource>(diagnosticsSessionSource, this.commParamsEcu.DefaultSessionSource, this.guiElementManager.GetGUIElement(this.comboBoxDefaultSession), out flag3);
				flag2 |= flag3;
				if (diagnosticsSessionSource == DiagnosticsSessionSource.UserDefined)
				{
					flag &= this.pageValidator.Control.UpdateModel<string>("", this.commParamsEcu.DefaultSessionName, this.guiElementManager.GetGUIElement(this.comboBoxDefaultSession), out flag3);
					flag2 |= flag3;
					ulong num;
					bool flag5;
					bool flag4 = GUIUtil.DisplayStringHexToNumber(this.textBoxDefaultSession.Text, 7u, out num, out flag5);
					if (!flag4)
					{
						num = this.commParamsEcu.DefaultSessionId.Value;
					}
					flag &= this.pageValidator.Control.UpdateModel<ulong>(num, this.commParamsEcu.DefaultSessionId, this.guiElementManager.GetGUIElement(this.textBoxDefaultSession), out flag3);
					if (!flag4)
					{
						string errorText = Resources.ErrorHexdumpExpected;
						if (flag5)
						{
							errorText = string.Format(Resources.ErrorTooManyBytesWithBorder, 7);
						}
						((IModelValidationResultCollector)this.pageValidator).SetErrorText(ValidationErrorClass.FormatError, this.commParamsEcu.DefaultSessionId, errorText);
						flag = false;
					}
					else
					{
						this.textBoxDefaultSession.Text = GUIUtil.NumberToDisplayStringHex(num, 0u);
					}
				}
				else
				{
					if (diagnosticsSessionSource != DiagnosticsSessionSource.DatabaseDefined)
					{
						text2 = "";
					}
					flag &= this.pageValidator.Control.UpdateModel<string>(text2, this.commParamsEcu.DefaultSessionName, this.guiElementManager.GetGUIElement(this.comboBoxDefaultSession), out flag3);
					flag2 |= flag3;
					ulong num = this.GetFixedSessionId(diagnosticsSessionSource, text2);
					this.textBoxDefaultSession.Text = GUIUtil.NumberToDisplayStringHex(num, 0u);
					flag &= this.pageValidator.Control.UpdateModel<ulong>(num, this.commParamsEcu.DefaultSessionId, this.guiElementManager.GetGUIElement(this.textBoxDefaultSession), out flag3);
				}
			}
			flag2 |= flag3;
			DiagnosticsSessionSource diagnosticsSessionSource2 = this.GetSelectedSession(this.comboBoxExtendedSession, out text2);
			if (!this.commParamsEcu.UseParamValuesFromDb.Value && diagnosticsSessionSource2 == DiagnosticsSessionSource.DatabaseDefined)
			{
				ulong num2 = this.GetFixedSessionId(diagnosticsSessionSource2, text2);
				diagnosticsSessionSource2 = DiagnosticsSessionSource.UserDefined;
				flag &= this.pageValidator.Control.UpdateModel<DiagnosticsSessionSource>(diagnosticsSessionSource2, this.commParamsEcu.ExtendedSessionSource, this.guiElementManager.GetGUIElement(this.comboBoxExtendedSession), out flag3);
				flag2 |= flag3;
				this.isInitControls = true;
				this.textBoxExtendedSession.Text = GUIUtil.NumberToDisplayStringHex(num2, 0u);
				this.isInitControls = false;
				flag &= this.pageValidator.Control.UpdateModel<ulong>(num2, this.commParamsEcu.ExtendedSessionId, this.guiElementManager.GetGUIElement(this.textBoxExtendedSession), out flag3);
			}
			else
			{
				flag &= this.pageValidator.Control.UpdateModel<DiagnosticsSessionSource>(diagnosticsSessionSource2, this.commParamsEcu.ExtendedSessionSource, this.guiElementManager.GetGUIElement(this.comboBoxExtendedSession), out flag3);
				flag2 |= flag3;
				if (diagnosticsSessionSource2 == DiagnosticsSessionSource.UserDefined)
				{
					flag &= this.pageValidator.Control.UpdateModel<string>("", this.commParamsEcu.ExtendedSessionName, this.guiElementManager.GetGUIElement(this.comboBoxExtendedSession), out flag3);
					flag2 |= flag3;
					bool flag5;
					ulong num2;
					bool flag6 = GUIUtil.DisplayStringHexToNumber(this.textBoxExtendedSession.Text, 7u, out num2, out flag5);
					if (!flag6)
					{
						num2 = this.commParamsEcu.ExtendedSessionId.Value;
					}
					flag &= this.pageValidator.Control.UpdateModel<ulong>(num2, this.commParamsEcu.ExtendedSessionId, this.guiElementManager.GetGUIElement(this.textBoxExtendedSession), out flag3);
					if (!flag6)
					{
						string errorText2 = Resources.ErrorHexdumpExpected;
						if (flag5)
						{
							errorText2 = string.Format(Resources.ErrorTooManyBytesWithBorder, 7);
						}
						((IModelValidationResultCollector)this.pageValidator).SetErrorText(ValidationErrorClass.FormatError, this.commParamsEcu.ExtendedSessionId, errorText2);
						flag = false;
					}
					else
					{
						this.textBoxExtendedSession.Text = GUIUtil.NumberToDisplayStringHex(num2, 0u);
					}
				}
				else
				{
					if (diagnosticsSessionSource2 != DiagnosticsSessionSource.DatabaseDefined)
					{
						text2 = "";
					}
					flag &= this.pageValidator.Control.UpdateModel<string>(text2, this.commParamsEcu.ExtendedSessionName, this.guiElementManager.GetGUIElement(this.comboBoxExtendedSession), out flag3);
					flag2 |= flag3;
					ulong num2 = this.GetFixedSessionId(diagnosticsSessionSource2, text2);
					this.textBoxExtendedSession.Text = GUIUtil.NumberToDisplayStringHex(num2, 0u);
					flag &= this.pageValidator.Control.UpdateModel<ulong>(num2, this.commParamsEcu.ExtendedSessionId, this.guiElementManager.GetGUIElement(this.textBoxExtendedSession), out flag3);
				}
			}
			flag2 |= flag3;
			flag &= this.ModelValidator.ValidateIndependentDiagCommParamsEcu(this.commParamsEcu, this.pageValidator);
			this.ModelValidator.ValidateOverallCommParamsRequestResponseIds(this.DiagnosticsDatabaseConfiguration, this.pageValidator, false);
			this.ModelValidator.ValidateOverallCommParamsSessionSettings(this.DiagnosticsDatabaseConfiguration, this.pageValidator, false);
			this.pageValidator.General.ActivateErrorProvidersForFormatAndModelErrors();
			this.Raise_DataChanged(this, EventArgs.Empty);
			return flag;
		}

		private void ResetValidationFramework()
		{
			this.pageValidator.General.Reset();
			this.guiElementManager.Reset();
		}

		private void ResetErrorProvider()
		{
			this.pageValidator.General.ResetAllErrorProviders();
			this.pageValidator.General.ResetAllFormatErrors();
		}

		private int InitCommInterfacesCombobox(bool isUseValuesFromDbEnabled)
		{
			this.isInitControls = true;
			int num = 0;
			string text = "";
			IList<string> list = null;
			this.comboBoxCommInterfaces.Items.Clear();
			this.displayedNameToQualifier.Clear();
			if (this.DiagSymbolsManager.GetValidCommInterfaceQualifiers(this.EcuQualifier, out list))
			{
				foreach (string current in list)
				{
					string arg = "";
					this.DiagSymbolsManager.GetCommInterfaceName(this.EcuQualifier, current, out arg);
					string text2 = string.Format("{0} ({1})", current, arg);
					this.comboBoxCommInterfaces.Items.Add(text2);
					this.displayedNameToQualifier.Add(text2, current);
					if (current == this.commParamsEcu.InterfaceQualifier.Value)
					{
						text = text2;
					}
					num++;
				}
			}
			if (this.comboBoxCommInterfaces.Items.Count == 0 || !isUseValuesFromDbEnabled)
			{
				this.comboBoxCommInterfaces.Items.Add(Resources.UserDefined);
			}
			this.comboBoxCommInterfaces.SelectedIndex = 0;
			if (!isUseValuesFromDbEnabled)
			{
				this.comboBoxCommInterfaces.SelectedItem = Resources.UserDefined;
			}
			else if (!string.IsNullOrEmpty(text))
			{
				this.comboBoxCommInterfaces.SelectedItem = text;
			}
			this.isInitControls = false;
			return num;
		}

		private void InitDiagProtocolsCombobox()
		{
			if (this.comboBoxDiagProtocol.Items.Count > 0)
			{
				return;
			}
			this.isInitControls = true;
			foreach (DiagnosticsProtocolType diagnosticsProtocolType in Enum.GetValues(typeof(DiagnosticsProtocolType)))
			{
				if (diagnosticsProtocolType != DiagnosticsProtocolType.Undefined)
				{
					this.comboBoxDiagProtocol.Items.Add(GUIUtil.MapDiagnosticsProtocol2String(diagnosticsProtocolType));
				}
			}
			this.comboBoxDiagProtocol.SelectedIndex = 0;
			this.isInitControls = false;
		}

		private void InitDiagAddrModeCombobox()
		{
			if (this.comboBoxAddressingMode.Items.Count > 0)
			{
				return;
			}
			this.isInitControls = true;
			foreach (DiagnosticsAddressingMode diagnosticsAddressingMode in Enum.GetValues(typeof(DiagnosticsAddressingMode)))
			{
				if (diagnosticsAddressingMode != DiagnosticsAddressingMode.Undefined)
				{
					this.comboBoxAddressingMode.Items.Add(GUIUtil.MapDiagnosticsAddressingMode2String(diagnosticsAddressingMode));
				}
			}
			this.isInitControls = false;
		}

		private void InitFirstConsecFrameCombobox()
		{
			if (this.comboBoxFirstConsecFrameValue.Items.Count > 0)
			{
				return;
			}
			this.isInitControls = true;
			for (byte b = Constants.MinimumFirstConsecFrameValue; b <= Constants.MaximumFirstConsecFrameValue; b += 1)
			{
				this.comboBoxFirstConsecFrameValue.Items.Add(b.ToString("X2"));
			}
			this.comboBoxFirstConsecFrameValue.SelectedIndex = 0;
			this.isInitControls = false;
		}

		private void UpdateStaticLabels()
		{
			this.labelEcu.Text = string.Format(Resources.EcuAndName, this.EcuQualifier);
			string arg = Resources.DisplayModeDec;
			if (GUIUtil.IsHexadecimal)
			{
				arg = Resources.DisplayModeHex;
			}
			this.labelPhysReqCANId.Text = string.Format(Resources.PhysReqCANId, arg);
			this.labelRespCANId.Text = string.Format(Resources.RespCANId, arg);
		}

		private void UpdateControlsForSelectedCommInterface()
		{
			this.EnableParameterControls();
			if (this.comboBoxCommInterfaces.SelectedItem != null && this.comboBoxCommInterfaces.SelectedItem.ToString() != Resources.UserDefined)
			{
				string commInterfaceQualifier = this.displayedNameToQualifier[this.comboBoxCommInterfaces.SelectedItem.ToString()];
				if (this.DiagSymbolsManager.GetCommInterfaceParamValuesFromDb(this.EcuQualifier, commInterfaceQualifier, ref this.commParamsEcu) && this.checkBoxUseDbParams.Checked)
				{
					if (this.commParamsEcu.HasDiagProtocolFromDb)
					{
						this.comboBoxDiagProtocol.Enabled = false;
					}
					if (this.commParamsEcu.HasDiagAddrModeFromDb)
					{
						this.comboBoxAddressingMode.Enabled = false;
						if (this.commParamsEcu.DiagAddressingMode.Value == DiagnosticsAddressingMode.Extended)
						{
							this.textBoxPhysReqAddrExt.ReadOnly = this.commParamsEcu.HasExtAddressingModeRqExtension;
							this.textBoxRespAddrExt.ReadOnly = this.commParamsEcu.HasExtAddressingModeRsExtension;
						}
					}
					if (this.commParamsEcu.HasP2TimeoutFromDb)
					{
						this.textBoxP2Timeout.ReadOnly = true;
					}
					if (this.commParamsEcu.HasP2ExtensionFromDb)
					{
						this.textBoxP2Extension.ReadOnly = true;
					}
					if (this.commParamsEcu.HasPhysRequestMsgFromDb)
					{
						this.textBoxPhysReqCANId.ReadOnly = true;
					}
					if (this.commParamsEcu.HasRespMsgFromDb)
					{
						this.textBoxRespCANId.ReadOnly = true;
					}
					if (this.commParamsEcu.HasUsePaddingBytesFromDb)
					{
						this.checkBoxUsePaddingBytes.Enabled = false;
					}
					if (this.commParamsEcu.HasPaddingBytesFromDb)
					{
						this.textBoxPaddingByteValue.ReadOnly = true;
					}
					if (this.commParamsEcu.HasFirstConsecFrameValueFromDb)
					{
						this.comboBoxFirstConsecFrameValue.Enabled = false;
					}
				}
			}
			this.UpdateParameterControls();
		}

		private void UpdateParameterControls()
		{
			this.isInitControls = true;
			this.comboBoxDiagProtocol.SelectedItem = GUIUtil.MapDiagnosticsProtocol2String(this.commParamsEcu.DiagProtocol.Value);
			this.comboBoxAddressingMode.SelectedItem = GUIUtil.MapDiagnosticsAddressingMode2String(this.commParamsEcu.DiagAddressingMode.Value);
			if (this.commParamsEcu.DiagAddressingMode.Value == DiagnosticsAddressingMode.Extended)
			{
				this.textBoxPhysReqAddrExt.Enabled = true;
				this.textBoxRespAddrExt.Enabled = true;
				this.textBoxPhysReqAddrExt.Text = GUIUtil.NumberToDisplayString(this.commParamsEcu.ExtAddressingModeRqExtension.Value);
				this.textBoxRespAddrExt.Text = GUIUtil.NumberToDisplayString(this.commParamsEcu.ExtAddressingModeRsExtension.Value);
			}
			else
			{
				this.textBoxPhysReqAddrExt.Enabled = false;
				this.textBoxRespAddrExt.Enabled = false;
				this.textBoxPhysReqAddrExt.Text = "";
				this.textBoxRespAddrExt.Text = "";
			}
			this.textBoxP2Timeout.Text = this.commParamsEcu.P2Timeout.Value.ToString();
			this.textBoxP2Extension.Text = this.commParamsEcu.P2Extension.Value.ToString();
			this.textBoxPhysReqCANId.Text = GUIUtil.CANIdToDisplayString(this.commParamsEcu.PhysRequestMsgId.Value, this.commParamsEcu.PhysRequestMsgIsExtendedId.Value);
			this.textBoxRespCANId.Text = GUIUtil.CANIdToDisplayString(this.commParamsEcu.ResponseMsgId.Value, this.commParamsEcu.ResponseMsgIsExtendedId.Value);
			this.textBoxSTMin.Text = this.commParamsEcu.STMin.Value.ToString();
			this.checkBoxUsePaddingBytes.Checked = this.commParamsEcu.UsePaddingBytes.Value;
			this.textBoxPaddingByteValue.Enabled = this.checkBoxUsePaddingBytes.Checked;
			this.textBoxPaddingByteValue.Text = GUIUtil.NumberToDisplayStringHex((ulong)this.commParamsEcu.PaddingByteValue.Value, 1u);
			int num = (int)this.commParamsEcu.FirstConsecFrameValue.Value;
			if (num < (int)Constants.MinimumFirstConsecFrameValue || num > (int)Constants.MaximumFirstConsecFrameValue)
			{
				num = ((int)Constants.MinimumFirstConsecFrameValue | (num & 15));
			}
			string text = num.ToString("X2");
			if (this.comboBoxFirstConsecFrameValue.Items.Contains(text))
			{
				this.comboBoxFirstConsecFrameValue.SelectedItem = text;
			}
			this.checkBoxUseStopCommRequest.Checked = this.commParamsEcu.UseStopCommRequest.Value;
			this.isInitControls = false;
		}

		private void EnableParameterControls()
		{
			this.comboBoxDiagProtocol.Enabled = true;
			this.comboBoxAddressingMode.Enabled = true;
			this.textBoxPhysReqAddrExt.Enabled = true;
			this.textBoxPhysReqAddrExt.ReadOnly = false;
			this.textBoxRespAddrExt.Enabled = true;
			this.textBoxRespAddrExt.ReadOnly = false;
			this.textBoxP2Timeout.ReadOnly = false;
			this.textBoxP2Extension.ReadOnly = false;
			this.textBoxPhysReqCANId.ReadOnly = false;
			this.textBoxRespCANId.ReadOnly = false;
			this.textBoxSTMin.ReadOnly = false;
			this.checkBoxUsePaddingBytes.Enabled = true;
			this.textBoxPaddingByteValue.ReadOnly = false;
			this.comboBoxFirstConsecFrameValue.Enabled = true;
		}

		private void InitSessionControls()
		{
			this.isInitControls = true;
			this.comboBoxDefaultSession.Items.Clear();
			this.comboBoxExtendedSession.Items.Clear();
			this.sessionNameToId.Clear();
			Dictionary<string, ulong> dictionary;
			if (this.DiagSymbolsManager.GetAvailableSessions(this.EcuQualifier, this.VariantQualifier, out dictionary))
			{
				this.sessionNameToId = dictionary;
				foreach (string current in dictionary.Keys)
				{
					this.comboBoxDefaultSession.Items.Add(Resources.DescLabel + current);
					this.comboBoxExtendedSession.Items.Add(Resources.DescLabel + current);
				}
			}
			if (this.checkBoxUseDbParams.Checked)
			{
				DiagnosticCommParamsECU diagnosticCommParamsECU = new DiagnosticCommParamsECU();
				diagnosticCommParamsECU.DiagProtocol.Value = this.commParamsEcu.DiagProtocol.Value;
				this.DiagSymbolsManager.GetSessionControlParamValuesFromDb(this.EcuQualifier, this.VariantQualifier, ref diagnosticCommParamsECU);
				if (diagnosticCommParamsECU.DefaultSessionSource.Value != DiagnosticsSessionSource.DatabaseDefined)
				{
					this.comboBoxDefaultSession.Items.Add(GUIUtil.MapDiagSessionSourceToString(DiagnosticsSessionSource.KWP_Default));
					this.comboBoxDefaultSession.Items.Add(GUIUtil.MapDiagSessionSourceToString(DiagnosticsSessionSource.UDS_Default));
					if (this.sessionNameToId.Count == 0)
					{
						this.comboBoxDefaultSession.Items.Add(GUIUtil.MapDiagSessionSourceToString(DiagnosticsSessionSource.UserDefined));
					}
				}
				if (diagnosticCommParamsECU.ExtendedSessionSource.Value != DiagnosticsSessionSource.DatabaseDefined)
				{
					this.comboBoxExtendedSession.Items.Add(GUIUtil.MapDiagSessionSourceToString(DiagnosticsSessionSource.UDS_Extended));
					this.comboBoxExtendedSession.Items.Add(GUIUtil.MapDiagSessionSourceToString(DiagnosticsSessionSource.UserDefined));
				}
			}
			else
			{
				this.comboBoxDefaultSession.Items.Add(GUIUtil.MapDiagSessionSourceToString(DiagnosticsSessionSource.KWP_Default));
				this.comboBoxDefaultSession.Items.Add(GUIUtil.MapDiagSessionSourceToString(DiagnosticsSessionSource.UDS_Default));
				this.comboBoxExtendedSession.Items.Add(GUIUtil.MapDiagSessionSourceToString(DiagnosticsSessionSource.UDS_Extended));
				this.comboBoxDefaultSession.Items.Add(GUIUtil.MapDiagSessionSourceToString(DiagnosticsSessionSource.UserDefined));
				this.comboBoxExtendedSession.Items.Add(GUIUtil.MapDiagSessionSourceToString(DiagnosticsSessionSource.UserDefined));
			}
			this.comboBoxDefaultSession.SelectedIndex = 0;
			this.comboBoxExtendedSession.SelectedIndex = 0;
			this.isInitControls = false;
		}

		private void UpdateSessionControls()
		{
			this.isInitControls = true;
			switch (this.commParamsEcu.DefaultSessionSource.Value)
			{
			case DiagnosticsSessionSource.UserDefined:
				this.comboBoxDefaultSession.SelectedItem = GUIUtil.MapDiagSessionSourceToString(this.commParamsEcu.DefaultSessionSource.Value);
				this.textBoxDefaultSession.ReadOnly = false;
				break;
			case DiagnosticsSessionSource.DatabaseDefined:
				if (this.checkBoxUseDbParams.Checked)
				{
					string text = Resources.DescLabel + this.commParamsEcu.DefaultSessionName.Value;
					if (this.comboBoxDefaultSession.Items.Contains(text))
					{
						this.comboBoxDefaultSession.SelectedItem = text;
						this.textBoxDefaultSession.ReadOnly = true;
					}
					else if (this.commParamsEcu.DiagProtocol.Value == DiagnosticsProtocolType.KWP)
					{
						this.commParamsEcu.DefaultSessionSource.Value = DiagnosticsSessionSource.KWP_Default;
						this.comboBoxDefaultSession.SelectedItem = GUIUtil.MapDiagSessionSourceToString(DiagnosticsSessionSource.KWP_Default);
						this.commParamsEcu.DefaultSessionId.Value = this.GetFixedSessionId(DiagnosticsSessionSource.KWP_Default, "");
						this.textBoxDefaultSession.ReadOnly = true;
					}
					else if (this.commParamsEcu.DiagProtocol.Value == DiagnosticsProtocolType.UDS)
					{
						this.commParamsEcu.DefaultSessionSource.Value = DiagnosticsSessionSource.UDS_Default;
						this.comboBoxDefaultSession.SelectedItem = GUIUtil.MapDiagSessionSourceToString(DiagnosticsSessionSource.UDS_Default);
						this.textBoxDefaultSession.ReadOnly = true;
					}
					else
					{
						this.commParamsEcu.DefaultSessionSource.Value = DiagnosticsSessionSource.UserDefined;
						this.comboBoxDefaultSession.SelectedItem = GUIUtil.MapDiagSessionSourceToString(DiagnosticsSessionSource.UserDefined);
						this.textBoxDefaultSession.ReadOnly = false;
					}
				}
				else
				{
					this.commParamsEcu.DefaultSessionSource.Value = DiagnosticsSessionSource.UserDefined;
					this.comboBoxDefaultSession.SelectedItem = GUIUtil.MapDiagSessionSourceToString(DiagnosticsSessionSource.UserDefined);
					this.textBoxDefaultSession.ReadOnly = false;
				}
				break;
			case DiagnosticsSessionSource.UDS_Default:
			case DiagnosticsSessionSource.KWP_Default:
				this.comboBoxDefaultSession.SelectedItem = GUIUtil.MapDiagSessionSourceToString(this.commParamsEcu.DefaultSessionSource.Value);
				this.textBoxDefaultSession.ReadOnly = true;
				break;
			}
			this.textBoxDefaultSession.Text = GUIUtil.NumberToDisplayStringHex(this.commParamsEcu.DefaultSessionId.Value, 0u);
			switch (this.commParamsEcu.ExtendedSessionSource.Value)
			{
			case DiagnosticsSessionSource.UserDefined:
				this.comboBoxExtendedSession.SelectedItem = GUIUtil.MapDiagSessionSourceToString(this.commParamsEcu.ExtendedSessionSource.Value);
				this.textBoxExtendedSession.ReadOnly = false;
				break;
			case DiagnosticsSessionSource.DatabaseDefined:
				if (this.checkBoxUseDbParams.Checked)
				{
					string text = Resources.DescLabel + this.commParamsEcu.ExtendedSessionName.Value;
					if (this.comboBoxExtendedSession.Items.Contains(text))
					{
						this.comboBoxExtendedSession.SelectedItem = text;
						this.textBoxExtendedSession.ReadOnly = true;
					}
					else if (this.commParamsEcu.DiagProtocol.Value == DiagnosticsProtocolType.UDS)
					{
						this.commParamsEcu.ExtendedSessionSource.Value = DiagnosticsSessionSource.UDS_Extended;
						this.comboBoxExtendedSession.SelectedItem = GUIUtil.MapDiagSessionSourceToString(DiagnosticsSessionSource.UDS_Extended);
						this.textBoxExtendedSession.ReadOnly = true;
					}
					else
					{
						this.commParamsEcu.ExtendedSessionSource.Value = DiagnosticsSessionSource.UserDefined;
						this.comboBoxExtendedSession.SelectedItem = GUIUtil.MapDiagSessionSourceToString(DiagnosticsSessionSource.UserDefined);
						this.textBoxExtendedSession.ReadOnly = false;
					}
				}
				else
				{
					this.commParamsEcu.ExtendedSessionSource.Value = DiagnosticsSessionSource.UserDefined;
					this.comboBoxExtendedSession.SelectedItem = GUIUtil.MapDiagSessionSourceToString(DiagnosticsSessionSource.UserDefined);
					this.textBoxExtendedSession.ReadOnly = false;
				}
				break;
			case DiagnosticsSessionSource.UDS_Extended:
				this.comboBoxExtendedSession.SelectedItem = GUIUtil.MapDiagSessionSourceToString(this.commParamsEcu.ExtendedSessionSource.Value);
				this.textBoxExtendedSession.ReadOnly = true;
				break;
			}
			this.textBoxExtendedSession.Text = GUIUtil.NumberToDisplayStringHex(this.commParamsEcu.ExtendedSessionId.Value, 0u);
			this.isInitControls = false;
		}

		private DiagnosticsSessionSource GetSelectedSession(ComboBox box, out string sessionNameFromDb)
		{
			sessionNameFromDb = "";
			string text = box.SelectedItem.ToString();
			DiagnosticsSessionSource diagnosticsSessionSource = GUIUtil.MapStringToDiagSessionSource(text);
			if (DiagnosticsSessionSource.DatabaseDefined == diagnosticsSessionSource)
			{
				if (text.StartsWith(Resources.DescLabel))
				{
					sessionNameFromDb = text.Substring(Resources.DescLabel.Length);
				}
				else
				{
					sessionNameFromDb = text;
				}
			}
			return diagnosticsSessionSource;
		}

		private ulong GetFixedSessionId(DiagnosticsSessionSource source, string sessionNameFromDb)
		{
			ulong result = 0uL;
			switch (source)
			{
			case DiagnosticsSessionSource.DatabaseDefined:
				if (this.sessionNameToId.ContainsKey(sessionNameFromDb))
				{
					result = this.sessionNameToId[sessionNameFromDb];
				}
				break;
			case DiagnosticsSessionSource.UDS_Default:
				result = Constants.SessionId_UDSDefault;
				break;
			case DiagnosticsSessionSource.UDS_Extended:
				result = Constants.SessionId_UDSExtended;
				break;
			case DiagnosticsSessionSource.KWP_Default:
				result = Constants.SessionId_KWPDefault;
				break;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(EcuParameters));
			this.labelEcu = new Label();
			this.checkBoxUseDbParams = new CheckBox();
			this.labelMs2 = new Label();
			this.labelMs = new Label();
			this.textBoxP2Extension = new TextBox();
			this.textBoxP2Timeout = new TextBox();
			this.labelP2Extension = new Label();
			this.labelP2Timeout = new Label();
			this.comboBoxFirstConsecFrameValue = new ComboBox();
			this.textBoxPaddingByteValue = new TextBox();
			this.labelFirstConsecValue = new Label();
			this.labelMs4 = new Label();
			this.checkBoxUsePaddingBytes = new CheckBox();
			this.textBoxSTMin = new TextBox();
			this.textBoxRespCANId = new TextBox();
			this.textBoxPhysReqCANId = new TextBox();
			this.labelSTMin = new Label();
			this.labelRespCANId = new Label();
			this.labelPhysReqCANId = new Label();
			this.errorProviderFormat = new ErrorProvider(this.components);
			this.textBoxDefaultSession = new TextBox();
			this.comboBoxDiagProtocol = new ComboBox();
			this.comboBoxExtendedSession = new ComboBox();
			this.comboBoxDefaultSession = new ComboBox();
			this.textBoxExtendedSession = new TextBox();
			this.textBoxPhysReqAddrExt = new TextBox();
			this.textBoxRespAddrExt = new TextBox();
			this.groupBoxCommParams = new GroupBox();
			this.labelSlash2 = new Label();
			this.comboBoxAddressingMode = new ComboBox();
			this.labelAddrMode = new Label();
			this.labelSlash1 = new Label();
			this.labelCommInterface = new Label();
			this.comboBoxCommInterfaces = new ComboBox();
			this.labelDiagProtocol = new Label();
			this.groupBoxOemSpecific = new GroupBox();
			this.checkBoxUseStopCommRequest = new CheckBox();
			this.groupBoxSessions = new GroupBox();
			this.labelIdExt = new Label();
			this.labelIdDefault = new Label();
			this.labelExtendedSession = new Label();
			this.labelDefaultSession = new Label();
			this.errorProviderLocalModel = new ErrorProvider(this.components);
			this.toolTip = new ToolTip(this.components);
			this.errorProviderGlobalModel = new ErrorProvider(this.components);
			((ISupportInitialize)this.errorProviderFormat).BeginInit();
			this.groupBoxCommParams.SuspendLayout();
			this.groupBoxOemSpecific.SuspendLayout();
			this.groupBoxSessions.SuspendLayout();
			((ISupportInitialize)this.errorProviderLocalModel).BeginInit();
			((ISupportInitialize)this.errorProviderGlobalModel).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.labelEcu, "labelEcu");
			this.labelEcu.Name = "labelEcu";
			componentResourceManager.ApplyResources(this.checkBoxUseDbParams, "checkBoxUseDbParams");
			this.checkBoxUseDbParams.Name = "checkBoxUseDbParams";
			this.checkBoxUseDbParams.UseVisualStyleBackColor = true;
			this.checkBoxUseDbParams.CheckedChanged += new EventHandler(this.checkBoxUseDbParams_CheckedChanged);
			componentResourceManager.ApplyResources(this.labelMs2, "labelMs2");
			this.labelMs2.Name = "labelMs2";
			componentResourceManager.ApplyResources(this.labelMs, "labelMs");
			this.labelMs.Name = "labelMs";
			this.errorProviderFormat.SetIconAlignment(this.textBoxP2Extension, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxP2Extension.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxP2Extension, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxP2Extension.IconAlignment1"));
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxP2Extension, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxP2Extension.IconAlignment2"));
			componentResourceManager.ApplyResources(this.textBoxP2Extension, "textBoxP2Extension");
			this.textBoxP2Extension.Name = "textBoxP2Extension";
			this.textBoxP2Extension.Validating += new CancelEventHandler(this.textBoxCommParameter_Validating);
			this.errorProviderFormat.SetIconAlignment(this.textBoxP2Timeout, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxP2Timeout.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxP2Timeout, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxP2Timeout.IconAlignment1"));
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxP2Timeout, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxP2Timeout.IconAlignment2"));
			componentResourceManager.ApplyResources(this.textBoxP2Timeout, "textBoxP2Timeout");
			this.textBoxP2Timeout.Name = "textBoxP2Timeout";
			this.textBoxP2Timeout.Validating += new CancelEventHandler(this.textBoxCommParameter_Validating);
			componentResourceManager.ApplyResources(this.labelP2Extension, "labelP2Extension");
			this.labelP2Extension.Name = "labelP2Extension";
			componentResourceManager.ApplyResources(this.labelP2Timeout, "labelP2Timeout");
			this.labelP2Timeout.Name = "labelP2Timeout";
			this.comboBoxFirstConsecFrameValue.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxFirstConsecFrameValue.FormattingEnabled = true;
			this.errorProviderLocalModel.SetIconAlignment(this.comboBoxFirstConsecFrameValue, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxFirstConsecFrameValue.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.comboBoxFirstConsecFrameValue, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxFirstConsecFrameValue.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.comboBoxFirstConsecFrameValue, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxFirstConsecFrameValue.IconAlignment2"));
			componentResourceManager.ApplyResources(this.comboBoxFirstConsecFrameValue, "comboBoxFirstConsecFrameValue");
			this.comboBoxFirstConsecFrameValue.Name = "comboBoxFirstConsecFrameValue";
			this.comboBoxFirstConsecFrameValue.SelectedIndexChanged += new EventHandler(this.comboBoxFirstConsecFrameValue_SelectedIndexChanged);
			this.errorProviderFormat.SetIconAlignment(this.textBoxPaddingByteValue, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxPaddingByteValue.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxPaddingByteValue, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxPaddingByteValue.IconAlignment1"));
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxPaddingByteValue, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxPaddingByteValue.IconAlignment2"));
			componentResourceManager.ApplyResources(this.textBoxPaddingByteValue, "textBoxPaddingByteValue");
			this.textBoxPaddingByteValue.Name = "textBoxPaddingByteValue";
			this.textBoxPaddingByteValue.Validating += new CancelEventHandler(this.textBoxCommParameter_Validating);
			componentResourceManager.ApplyResources(this.labelFirstConsecValue, "labelFirstConsecValue");
			this.labelFirstConsecValue.Name = "labelFirstConsecValue";
			componentResourceManager.ApplyResources(this.labelMs4, "labelMs4");
			this.labelMs4.Name = "labelMs4";
			componentResourceManager.ApplyResources(this.checkBoxUsePaddingBytes, "checkBoxUsePaddingBytes");
			this.checkBoxUsePaddingBytes.Name = "checkBoxUsePaddingBytes";
			this.checkBoxUsePaddingBytes.UseVisualStyleBackColor = true;
			this.checkBoxUsePaddingBytes.CheckedChanged += new EventHandler(this.checkBoxUsePaddingBytes_CheckedChanged);
			this.errorProviderFormat.SetIconAlignment(this.textBoxSTMin, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxSTMin.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxSTMin, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxSTMin.IconAlignment1"));
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxSTMin, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxSTMin.IconAlignment2"));
			componentResourceManager.ApplyResources(this.textBoxSTMin, "textBoxSTMin");
			this.textBoxSTMin.Name = "textBoxSTMin";
			this.textBoxSTMin.Validating += new CancelEventHandler(this.textBoxCommParameter_Validating);
			this.errorProviderFormat.SetIconAlignment(this.textBoxRespCANId, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxRespCANId.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxRespCANId, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxRespCANId.IconAlignment1"));
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxRespCANId, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxRespCANId.IconAlignment2"));
			componentResourceManager.ApplyResources(this.textBoxRespCANId, "textBoxRespCANId");
			this.textBoxRespCANId.Name = "textBoxRespCANId";
			this.textBoxRespCANId.Validating += new CancelEventHandler(this.textBoxCommParameter_Validating);
			this.errorProviderFormat.SetIconAlignment(this.textBoxPhysReqCANId, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxPhysReqCANId.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxPhysReqCANId, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxPhysReqCANId.IconAlignment1"));
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxPhysReqCANId, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxPhysReqCANId.IconAlignment2"));
			componentResourceManager.ApplyResources(this.textBoxPhysReqCANId, "textBoxPhysReqCANId");
			this.textBoxPhysReqCANId.Name = "textBoxPhysReqCANId";
			this.textBoxPhysReqCANId.Validating += new CancelEventHandler(this.textBoxCommParameter_Validating);
			componentResourceManager.ApplyResources(this.labelSTMin, "labelSTMin");
			this.labelSTMin.Name = "labelSTMin";
			componentResourceManager.ApplyResources(this.labelRespCANId, "labelRespCANId");
			this.labelRespCANId.Name = "labelRespCANId";
			componentResourceManager.ApplyResources(this.labelPhysReqCANId, "labelPhysReqCANId");
			this.labelPhysReqCANId.Name = "labelPhysReqCANId";
			this.errorProviderFormat.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderFormat.ContainerControl = this;
			this.errorProviderFormat.SetIconAlignment(this.textBoxDefaultSession, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxDefaultSession.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxDefaultSession, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxDefaultSession.IconAlignment1"));
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxDefaultSession, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxDefaultSession.IconAlignment2"));
			componentResourceManager.ApplyResources(this.textBoxDefaultSession, "textBoxDefaultSession");
			this.textBoxDefaultSession.Name = "textBoxDefaultSession";
			this.textBoxDefaultSession.Validating += new CancelEventHandler(this.textBoxDefaultSession_Validating);
			this.comboBoxDiagProtocol.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxDiagProtocol.FormattingEnabled = true;
			this.errorProviderLocalModel.SetIconAlignment(this.comboBoxDiagProtocol, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxDiagProtocol.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.comboBoxDiagProtocol, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxDiagProtocol.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.comboBoxDiagProtocol, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxDiagProtocol.IconAlignment2"));
			componentResourceManager.ApplyResources(this.comboBoxDiagProtocol, "comboBoxDiagProtocol");
			this.comboBoxDiagProtocol.Name = "comboBoxDiagProtocol";
			this.comboBoxDiagProtocol.SelectedIndexChanged += new EventHandler(this.comboBoxDiagProtocol_SelectedIndexChanged);
			this.comboBoxExtendedSession.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxExtendedSession.FormattingEnabled = true;
			this.errorProviderLocalModel.SetIconAlignment(this.comboBoxExtendedSession, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxExtendedSession.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.comboBoxExtendedSession, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxExtendedSession.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.comboBoxExtendedSession, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxExtendedSession.IconAlignment2"));
			componentResourceManager.ApplyResources(this.comboBoxExtendedSession, "comboBoxExtendedSession");
			this.comboBoxExtendedSession.Name = "comboBoxExtendedSession";
			this.comboBoxExtendedSession.SelectedIndexChanged += new EventHandler(this.comboBoxExtendedSession_SelectedIndexChanged);
			this.comboBoxDefaultSession.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxDefaultSession.FormattingEnabled = true;
			this.errorProviderLocalModel.SetIconAlignment(this.comboBoxDefaultSession, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxDefaultSession.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.comboBoxDefaultSession, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxDefaultSession.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.comboBoxDefaultSession, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxDefaultSession.IconAlignment2"));
			componentResourceManager.ApplyResources(this.comboBoxDefaultSession, "comboBoxDefaultSession");
			this.comboBoxDefaultSession.Name = "comboBoxDefaultSession";
			this.comboBoxDefaultSession.SelectedIndexChanged += new EventHandler(this.comboBoxDefaultSession_SelectedIndexChanged);
			this.errorProviderFormat.SetIconAlignment(this.textBoxExtendedSession, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxExtendedSession.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxExtendedSession, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxExtendedSession.IconAlignment1"));
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxExtendedSession, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxExtendedSession.IconAlignment2"));
			componentResourceManager.ApplyResources(this.textBoxExtendedSession, "textBoxExtendedSession");
			this.textBoxExtendedSession.Name = "textBoxExtendedSession";
			this.textBoxExtendedSession.Validating += new CancelEventHandler(this.textBoxExtendedSession_Validating);
			componentResourceManager.ApplyResources(this.textBoxPhysReqAddrExt, "textBoxPhysReqAddrExt");
			this.errorProviderFormat.SetIconAlignment(this.textBoxPhysReqAddrExt, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxPhysReqAddrExt.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxPhysReqAddrExt, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxPhysReqAddrExt.IconAlignment1"));
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxPhysReqAddrExt, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxPhysReqAddrExt.IconAlignment2"));
			this.textBoxPhysReqAddrExt.Name = "textBoxPhysReqAddrExt";
			this.textBoxPhysReqAddrExt.Validating += new CancelEventHandler(this.textBoxCommParameter_Validating);
			componentResourceManager.ApplyResources(this.textBoxRespAddrExt, "textBoxRespAddrExt");
			this.errorProviderFormat.SetIconAlignment(this.textBoxRespAddrExt, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxRespAddrExt.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxRespAddrExt, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxRespAddrExt.IconAlignment1"));
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxRespAddrExt, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxRespAddrExt.IconAlignment2"));
			this.textBoxRespAddrExt.Name = "textBoxRespAddrExt";
			this.textBoxRespAddrExt.Validating += new CancelEventHandler(this.textBoxCommParameter_Validating);
			this.groupBoxCommParams.Controls.Add(this.textBoxRespAddrExt);
			this.groupBoxCommParams.Controls.Add(this.labelSlash2);
			this.groupBoxCommParams.Controls.Add(this.comboBoxAddressingMode);
			this.groupBoxCommParams.Controls.Add(this.labelAddrMode);
			this.groupBoxCommParams.Controls.Add(this.labelSlash1);
			this.groupBoxCommParams.Controls.Add(this.textBoxPhysReqAddrExt);
			this.groupBoxCommParams.Controls.Add(this.labelCommInterface);
			this.groupBoxCommParams.Controls.Add(this.comboBoxFirstConsecFrameValue);
			this.groupBoxCommParams.Controls.Add(this.comboBoxCommInterfaces);
			this.groupBoxCommParams.Controls.Add(this.labelDiagProtocol);
			this.groupBoxCommParams.Controls.Add(this.labelFirstConsecValue);
			this.groupBoxCommParams.Controls.Add(this.labelMs2);
			this.groupBoxCommParams.Controls.Add(this.textBoxPaddingByteValue);
			this.groupBoxCommParams.Controls.Add(this.comboBoxDiagProtocol);
			this.groupBoxCommParams.Controls.Add(this.checkBoxUsePaddingBytes);
			this.groupBoxCommParams.Controls.Add(this.labelP2Timeout);
			this.groupBoxCommParams.Controls.Add(this.labelMs4);
			this.groupBoxCommParams.Controls.Add(this.textBoxP2Timeout);
			this.groupBoxCommParams.Controls.Add(this.textBoxSTMin);
			this.groupBoxCommParams.Controls.Add(this.textBoxP2Extension);
			this.groupBoxCommParams.Controls.Add(this.labelMs);
			this.groupBoxCommParams.Controls.Add(this.labelSTMin);
			this.groupBoxCommParams.Controls.Add(this.labelP2Extension);
			this.groupBoxCommParams.Controls.Add(this.textBoxRespCANId);
			this.groupBoxCommParams.Controls.Add(this.labelPhysReqCANId);
			this.groupBoxCommParams.Controls.Add(this.textBoxPhysReqCANId);
			this.groupBoxCommParams.Controls.Add(this.labelRespCANId);
			componentResourceManager.ApplyResources(this.groupBoxCommParams, "groupBoxCommParams");
			this.groupBoxCommParams.Name = "groupBoxCommParams";
			this.groupBoxCommParams.TabStop = false;
			componentResourceManager.ApplyResources(this.labelSlash2, "labelSlash2");
			this.labelSlash2.Name = "labelSlash2";
			this.comboBoxAddressingMode.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxAddressingMode.FormattingEnabled = true;
			componentResourceManager.ApplyResources(this.comboBoxAddressingMode, "comboBoxAddressingMode");
			this.comboBoxAddressingMode.Name = "comboBoxAddressingMode";
			this.comboBoxAddressingMode.SelectedIndexChanged += new EventHandler(this.comboBoxAddressingMode_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.labelAddrMode, "labelAddrMode");
			this.labelAddrMode.Name = "labelAddrMode";
			componentResourceManager.ApplyResources(this.labelSlash1, "labelSlash1");
			this.labelSlash1.Name = "labelSlash1";
			componentResourceManager.ApplyResources(this.labelCommInterface, "labelCommInterface");
			this.labelCommInterface.Name = "labelCommInterface";
			this.comboBoxCommInterfaces.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxCommInterfaces.FormattingEnabled = true;
			componentResourceManager.ApplyResources(this.comboBoxCommInterfaces, "comboBoxCommInterfaces");
			this.comboBoxCommInterfaces.Name = "comboBoxCommInterfaces";
			this.comboBoxCommInterfaces.SelectedIndexChanged += new EventHandler(this.comboBoxCommInterfaces_SelectedIndexChanged);
			this.comboBoxCommInterfaces.MouseEnter += new EventHandler(this.comboBoxCommInterfaces_MouseEnter);
			componentResourceManager.ApplyResources(this.labelDiagProtocol, "labelDiagProtocol");
			this.labelDiagProtocol.Name = "labelDiagProtocol";
			this.groupBoxOemSpecific.Controls.Add(this.checkBoxUseStopCommRequest);
			componentResourceManager.ApplyResources(this.groupBoxOemSpecific, "groupBoxOemSpecific");
			this.groupBoxOemSpecific.Name = "groupBoxOemSpecific";
			this.groupBoxOemSpecific.TabStop = false;
			componentResourceManager.ApplyResources(this.checkBoxUseStopCommRequest, "checkBoxUseStopCommRequest");
			this.checkBoxUseStopCommRequest.Name = "checkBoxUseStopCommRequest";
			this.checkBoxUseStopCommRequest.UseVisualStyleBackColor = true;
			this.checkBoxUseStopCommRequest.CheckedChanged += new EventHandler(this.checkBoxUseStopCommRequest_CheckedChanged);
			this.groupBoxSessions.Controls.Add(this.labelIdExt);
			this.groupBoxSessions.Controls.Add(this.labelIdDefault);
			this.groupBoxSessions.Controls.Add(this.textBoxExtendedSession);
			this.groupBoxSessions.Controls.Add(this.textBoxDefaultSession);
			this.groupBoxSessions.Controls.Add(this.comboBoxExtendedSession);
			this.groupBoxSessions.Controls.Add(this.comboBoxDefaultSession);
			this.groupBoxSessions.Controls.Add(this.labelExtendedSession);
			this.groupBoxSessions.Controls.Add(this.labelDefaultSession);
			componentResourceManager.ApplyResources(this.groupBoxSessions, "groupBoxSessions");
			this.groupBoxSessions.Name = "groupBoxSessions";
			this.groupBoxSessions.TabStop = false;
			componentResourceManager.ApplyResources(this.labelIdExt, "labelIdExt");
			this.labelIdExt.Name = "labelIdExt";
			componentResourceManager.ApplyResources(this.labelIdDefault, "labelIdDefault");
			this.labelIdDefault.Name = "labelIdDefault";
			componentResourceManager.ApplyResources(this.labelExtendedSession, "labelExtendedSession");
			this.labelExtendedSession.Name = "labelExtendedSession";
			componentResourceManager.ApplyResources(this.labelDefaultSession, "labelDefaultSession");
			this.labelDefaultSession.Name = "labelDefaultSession";
			this.errorProviderLocalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderLocalModel.ContainerControl = this;
			this.errorProviderGlobalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderGlobalModel.ContainerControl = this;
			componentResourceManager.ApplyResources(this.errorProviderGlobalModel, "errorProviderGlobalModel");
			base.AutoScaleMode = AutoScaleMode.Inherit;
			base.Controls.Add(this.checkBoxUseDbParams);
			base.Controls.Add(this.groupBoxSessions);
			base.Controls.Add(this.groupBoxOemSpecific);
			base.Controls.Add(this.groupBoxCommParams);
			base.Controls.Add(this.labelEcu);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "EcuParameters";
			((ISupportInitialize)this.errorProviderFormat).EndInit();
			this.groupBoxCommParams.ResumeLayout(false);
			this.groupBoxCommParams.PerformLayout();
			this.groupBoxOemSpecific.ResumeLayout(false);
			this.groupBoxOemSpecific.PerformLayout();
			this.groupBoxSessions.ResumeLayout(false);
			this.groupBoxSessions.PerformLayout();
			((ISupportInitialize)this.errorProviderLocalModel).EndInit();
			((ISupportInitialize)this.errorProviderGlobalModel).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
