using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;
using Vector.VLConfig.ValidationFramework;

namespace Vector.VLConfig.GUI.GPSPage
{
	public class GPSGL2000 : UserControl
	{
		private GUIElementManager_Control guiElementManager;

		private CustomErrorProvider customErrorProvider;

		private PageValidator pageValidator;

		private GPSConfiguration gpsConfig;

		private DisplayMode displayMode;

		private bool isInitControls;

		private List<TextBox> canIdTextBoxes;

		private IContainer components;

		private CheckBox checkBoxMapToCANMessage;

		private GroupBox groupBoxGPS;

		private TextBox textBoxCANId6;

		private Label labelPrecision;

		private TextBox textBoxCANId5;

		private TextBox textBoxCANId4;

		private TextBox textBoxCANId3;

		private Label labelVelDir;

		private TextBox textBoxCANId2;

		private TextBox textBoxCANId1;

		private Label labelLongLat;

		private Label labelDateTimeAltitude;

		private Label labelGPSData;

		private ComboBox comboBoxSendOnChannel;

		private Label labelSendChannel;

		private Label labelCANId;

		private Label labelLongLat2;

		private Label labelSatDetails;

		private ErrorProvider errorProviderFormat;

		private ErrorProvider errorProviderLocalModel;

		private ErrorProvider errorProviderGlobalModel;

		private CheckBox checkBoxMapToSystemChannel;

		public IModelValidator ModelValidator
		{
			get;
			set;
		}

		public GPSConfiguration GPSConfiguration
		{
			get
			{
				return this.gpsConfig;
			}
			set
			{
				this.gpsConfig = value;
				this.UpdateGUI();
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

		public GPSGL2000()
		{
			this.InitializeComponent();
			this.isInitControls = false;
			this.guiElementManager = new GUIElementManager_Control();
			this.customErrorProvider = new CustomErrorProvider(this.errorProviderFormat);
			this.customErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.LocalModelError, this.errorProviderLocalModel);
			this.customErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.GlobalModelError, this.errorProviderGlobalModel);
			this.pageValidator = new PageValidator(this.customErrorProvider);
			this.canIdTextBoxes = new List<TextBox>();
			this.canIdTextBoxes.Add(this.textBoxCANId1);
			this.canIdTextBoxes.Add(this.textBoxCANId2);
			this.canIdTextBoxes.Add(this.textBoxCANId3);
			this.canIdTextBoxes.Add(this.textBoxCANId4);
			this.canIdTextBoxes.Add(this.textBoxCANId5);
			this.canIdTextBoxes.Add(this.textBoxCANId6);
		}

		public void Init()
		{
			this.isInitControls = true;
			this.InitChannelComboBox();
			this.isInitControls = false;
		}

		public void Reset()
		{
			this.ResetValidationFramework();
		}

		private void InitChannelComboBox()
		{
			this.comboBoxSendOnChannel.Items.Clear();
			for (uint num = 1u; num <= this.ModelValidator.LoggerSpecifics.CAN.NumberOfChannels; num += 1u)
			{
				this.comboBoxSendOnChannel.Items.Add(GUIUtil.MapCANChannelNumber2String(num));
			}
			uint num2 = this.ModelValidator.LoggerSpecifics.CAN.NumberOfChannels + 1u;
			for (uint num3 = num2; num3 < this.ModelValidator.LoggerSpecifics.CAN.NumberOfVirtualChannels + num2; num3 += 1u)
			{
				this.comboBoxSendOnChannel.Items.Add(GUIUtil.MapCANChannelNumber2String(num3) + Resources.VirtualChannelPostfix);
			}
			if (this.comboBoxSendOnChannel.Items.Count > 0)
			{
				this.comboBoxSendOnChannel.SelectedIndex = 0;
			}
		}

		private void checkBoxMapToSystemChannel_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		private void checkBoxMapToCAN_CheckedChanged(object sender, EventArgs e)
		{
			bool @checked = this.checkBoxMapToCANMessage.Checked;
			this.textBoxCANId1.Enabled = @checked;
			this.comboBoxSendOnChannel.Enabled = @checked;
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		private void textBoxCANId1_Validating(object sender, CancelEventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		private void comboBoxSendOnChannel_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		public bool ValidateInput()
		{
			if (this.gpsConfig == null)
			{
				return false;
			}
			bool flag = true;
			bool flag2 = false;
			this.pageValidator.General.ResetAllErrorProviders();
			this.pageValidator.General.ResetAllFormatErrors();
			this.ResetErrorProviderForDisplayControls();
			bool flag3;
			flag &= this.pageValidator.Control.UpdateModel<bool>(this.checkBoxMapToSystemChannel.Checked, this.gpsConfig.MapToSystemChannel, this.guiElementManager.GetGUIElement(this.checkBoxMapToSystemChannel), out flag3);
			flag2 |= flag3;
			flag &= this.pageValidator.Control.UpdateModel<bool>(this.checkBoxMapToCANMessage.Checked, this.gpsConfig.MapToCANMessage, this.guiElementManager.GetGUIElement(this.checkBoxMapToCANMessage), out flag3);
			flag2 |= flag3;
			if (this.gpsConfig.MapToCANMessage.Value)
			{
				flag &= this.pageValidator.Control.ValidateFormatAndUpdateModel_CANId(this.textBoxCANId1.Text, this.gpsConfig.StartCANId, this.gpsConfig.IsExtendedStartCANId, this.guiElementManager.GetGUIElement(this.textBoxCANId1), out flag3);
				flag2 |= flag3;
				if (flag)
				{
					this.DisplaySubsequentCANIds(this.gpsConfig.StartCANId.Value, this.gpsConfig.IsExtendedStartCANId.Value);
					IList<ValidatedProperty<uint>> subsequentCANIds = this.gpsConfig.SubsequentCANIds;
					bool flag4;
					this.pageValidator.Control.ValidateFormatAndUpdateModel_CANId(this.textBoxCANId2.Text, subsequentCANIds[0], this.gpsConfig.IsExtendedStartCANId, this.guiElementManager.GetGUIElement(this.textBoxCANId2), out flag4);
					this.pageValidator.Control.ValidateFormatAndUpdateModel_CANId(this.textBoxCANId3.Text, subsequentCANIds[1], this.gpsConfig.IsExtendedStartCANId, this.guiElementManager.GetGUIElement(this.textBoxCANId3), out flag4);
					this.pageValidator.Control.ValidateFormatAndUpdateModel_CANId(this.textBoxCANId4.Text, subsequentCANIds[2], this.gpsConfig.IsExtendedStartCANId, this.guiElementManager.GetGUIElement(this.textBoxCANId4), out flag4);
					this.pageValidator.Control.ValidateFormatAndUpdateModel_CANId(this.textBoxCANId5.Text, subsequentCANIds[3], this.gpsConfig.IsExtendedStartCANId, this.guiElementManager.GetGUIElement(this.textBoxCANId5), out flag4);
					this.pageValidator.Control.ValidateFormatAndUpdateModel_CANId(this.textBoxCANId6.Text, subsequentCANIds[4], this.gpsConfig.IsExtendedStartCANId, this.guiElementManager.GetGUIElement(this.textBoxCANId6), out flag4);
				}
				flag &= this.pageValidator.Control.UpdateModel<uint>(GUIUtil.MapCANChannelString2Number(this.comboBoxSendOnChannel.SelectedItem.ToString()), this.gpsConfig.CANChannel, this.guiElementManager.GetGUIElement(this.comboBoxSendOnChannel), out flag3);
				flag2 |= flag3;
			}
			flag &= this.ModelValidator.Validate(this.gpsConfig, flag2, this.pageValidator);
			if (flag)
			{
				flag &= this.ValidateCanIdsUniqueness();
			}
			this.pageValidator.General.ActivateErrorProvidersForFormatAndModelErrors();
			return flag;
		}

		private bool ValidateCanIdsUniqueness()
		{
			return true;
		}

		private void ResetErrorProviderForDisplayControls()
		{
			for (int i = 1; i < this.canIdTextBoxes.Count; i++)
			{
				this.errorProviderGlobalModel.SetError(this.canIdTextBoxes[i], string.Empty);
			}
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

		private void UpdateGUI()
		{
			if (this.gpsConfig == null)
			{
				return;
			}
			this.isInitControls = true;
			this.checkBoxMapToSystemChannel.Checked = this.gpsConfig.MapToSystemChannel.Value;
			this.checkBoxMapToCANMessage.Checked = this.gpsConfig.MapToCANMessage.Value;
			this.EnableControlsForActivity(this.gpsConfig.MapToCANMessage.Value);
			this.textBoxCANId1.Text = GUIUtil.CANIdToDisplayString(this.gpsConfig.StartCANId.Value, this.gpsConfig.IsExtendedStartCANId.Value);
			this.DisplaySubsequentCANIds(this.gpsConfig.StartCANId.Value, this.gpsConfig.IsExtendedStartCANId.Value);
			if (this.gpsConfig.CANChannel.Value > this.ModelValidator.LoggerSpecifics.CAN.NumberOfChannels)
			{
				this.comboBoxSendOnChannel.SelectedItem = GUIUtil.MapCANChannelNumber2String(this.gpsConfig.CANChannel.Value) + Resources.VirtualChannelPostfix;
			}
			else
			{
				this.comboBoxSendOnChannel.SelectedItem = GUIUtil.MapCANChannelNumber2String(this.gpsConfig.CANChannel.Value);
			}
			this.isInitControls = false;
			this.ValidateInput();
		}

		private void EnableControlsForActivity(bool isActive)
		{
			foreach (TextBox current in this.canIdTextBoxes)
			{
				current.Enabled = isActive;
			}
			this.comboBoxSendOnChannel.Enabled = isActive;
		}

		private void DisplaySubsequentCANIds(uint startCANId, bool isExtendedStartCANId)
		{
			for (int i = 1; i < this.canIdTextBoxes.Count; i++)
			{
				this.canIdTextBoxes[i].Text = this.GetCANIdStringForStartAndOffset(this.gpsConfig.StartCANId.Value, this.gpsConfig.IsExtendedStartCANId.Value, (uint)i);
			}
		}

		private string GetCANIdStringForStartAndOffset(uint startCANId, bool isExtendedStartCANId, uint offset)
		{
			uint num = startCANId + offset;
			if (isExtendedStartCANId)
			{
				if (num > Constants.MaximumExtendedCANId)
				{
					num = Constants.MaximumExtendedCANId;
				}
			}
			else if (num > Constants.MaximumStandardCANId)
			{
				num = Constants.MaximumStandardCANId;
			}
			return GUIUtil.CANIdToDisplayString(num, isExtendedStartCANId);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(GPSGL2000));
			this.checkBoxMapToCANMessage = new CheckBox();
			this.groupBoxGPS = new GroupBox();
			this.checkBoxMapToSystemChannel = new CheckBox();
			this.comboBoxSendOnChannel = new ComboBox();
			this.labelSendChannel = new Label();
			this.labelCANId = new Label();
			this.labelLongLat2 = new Label();
			this.labelSatDetails = new Label();
			this.textBoxCANId6 = new TextBox();
			this.labelPrecision = new Label();
			this.textBoxCANId5 = new TextBox();
			this.textBoxCANId4 = new TextBox();
			this.textBoxCANId3 = new TextBox();
			this.labelVelDir = new Label();
			this.textBoxCANId2 = new TextBox();
			this.textBoxCANId1 = new TextBox();
			this.labelLongLat = new Label();
			this.labelDateTimeAltitude = new Label();
			this.labelGPSData = new Label();
			this.errorProviderFormat = new ErrorProvider(this.components);
			this.errorProviderLocalModel = new ErrorProvider(this.components);
			this.errorProviderGlobalModel = new ErrorProvider(this.components);
			this.groupBoxGPS.SuspendLayout();
			((ISupportInitialize)this.errorProviderFormat).BeginInit();
			((ISupportInitialize)this.errorProviderLocalModel).BeginInit();
			((ISupportInitialize)this.errorProviderGlobalModel).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.checkBoxMapToCANMessage, "checkBoxMapToCANMessage");
			this.errorProviderGlobalModel.SetError(this.checkBoxMapToCANMessage, componentResourceManager.GetString("checkBoxMapToCANMessage.Error"));
			this.errorProviderFormat.SetError(this.checkBoxMapToCANMessage, componentResourceManager.GetString("checkBoxMapToCANMessage.Error1"));
			this.errorProviderLocalModel.SetError(this.checkBoxMapToCANMessage, componentResourceManager.GetString("checkBoxMapToCANMessage.Error2"));
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxMapToCANMessage, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxMapToCANMessage.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.checkBoxMapToCANMessage, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxMapToCANMessage.IconAlignment1"));
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxMapToCANMessage, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxMapToCANMessage.IconAlignment2"));
			this.errorProviderLocalModel.SetIconPadding(this.checkBoxMapToCANMessage, (int)componentResourceManager.GetObject("checkBoxMapToCANMessage.IconPadding"));
			this.errorProviderFormat.SetIconPadding(this.checkBoxMapToCANMessage, (int)componentResourceManager.GetObject("checkBoxMapToCANMessage.IconPadding1"));
			this.errorProviderGlobalModel.SetIconPadding(this.checkBoxMapToCANMessage, (int)componentResourceManager.GetObject("checkBoxMapToCANMessage.IconPadding2"));
			this.checkBoxMapToCANMessage.Name = "checkBoxMapToCANMessage";
			this.checkBoxMapToCANMessage.UseVisualStyleBackColor = true;
			this.checkBoxMapToCANMessage.CheckedChanged += new EventHandler(this.checkBoxMapToCAN_CheckedChanged);
			componentResourceManager.ApplyResources(this.groupBoxGPS, "groupBoxGPS");
			this.groupBoxGPS.Controls.Add(this.checkBoxMapToSystemChannel);
			this.groupBoxGPS.Controls.Add(this.comboBoxSendOnChannel);
			this.groupBoxGPS.Controls.Add(this.labelSendChannel);
			this.groupBoxGPS.Controls.Add(this.labelCANId);
			this.groupBoxGPS.Controls.Add(this.labelLongLat2);
			this.groupBoxGPS.Controls.Add(this.labelSatDetails);
			this.groupBoxGPS.Controls.Add(this.textBoxCANId6);
			this.groupBoxGPS.Controls.Add(this.labelPrecision);
			this.groupBoxGPS.Controls.Add(this.textBoxCANId5);
			this.groupBoxGPS.Controls.Add(this.textBoxCANId4);
			this.groupBoxGPS.Controls.Add(this.textBoxCANId3);
			this.groupBoxGPS.Controls.Add(this.labelVelDir);
			this.groupBoxGPS.Controls.Add(this.textBoxCANId2);
			this.groupBoxGPS.Controls.Add(this.textBoxCANId1);
			this.groupBoxGPS.Controls.Add(this.labelLongLat);
			this.groupBoxGPS.Controls.Add(this.labelDateTimeAltitude);
			this.groupBoxGPS.Controls.Add(this.labelGPSData);
			this.groupBoxGPS.Controls.Add(this.checkBoxMapToCANMessage);
			this.errorProviderFormat.SetError(this.groupBoxGPS, componentResourceManager.GetString("groupBoxGPS.Error"));
			this.errorProviderGlobalModel.SetError(this.groupBoxGPS, componentResourceManager.GetString("groupBoxGPS.Error1"));
			this.errorProviderLocalModel.SetError(this.groupBoxGPS, componentResourceManager.GetString("groupBoxGPS.Error2"));
			this.errorProviderFormat.SetIconAlignment(this.groupBoxGPS, (ErrorIconAlignment)componentResourceManager.GetObject("groupBoxGPS.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this.groupBoxGPS, (ErrorIconAlignment)componentResourceManager.GetObject("groupBoxGPS.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.groupBoxGPS, (ErrorIconAlignment)componentResourceManager.GetObject("groupBoxGPS.IconAlignment2"));
			this.errorProviderGlobalModel.SetIconPadding(this.groupBoxGPS, (int)componentResourceManager.GetObject("groupBoxGPS.IconPadding"));
			this.errorProviderLocalModel.SetIconPadding(this.groupBoxGPS, (int)componentResourceManager.GetObject("groupBoxGPS.IconPadding1"));
			this.errorProviderFormat.SetIconPadding(this.groupBoxGPS, (int)componentResourceManager.GetObject("groupBoxGPS.IconPadding2"));
			this.groupBoxGPS.Name = "groupBoxGPS";
			this.groupBoxGPS.TabStop = false;
			componentResourceManager.ApplyResources(this.checkBoxMapToSystemChannel, "checkBoxMapToSystemChannel");
			this.errorProviderGlobalModel.SetError(this.checkBoxMapToSystemChannel, componentResourceManager.GetString("checkBoxMapToSystemChannel.Error"));
			this.errorProviderFormat.SetError(this.checkBoxMapToSystemChannel, componentResourceManager.GetString("checkBoxMapToSystemChannel.Error1"));
			this.errorProviderLocalModel.SetError(this.checkBoxMapToSystemChannel, componentResourceManager.GetString("checkBoxMapToSystemChannel.Error2"));
			this.errorProviderGlobalModel.SetIconAlignment(this.checkBoxMapToSystemChannel, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxMapToSystemChannel.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.checkBoxMapToSystemChannel, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxMapToSystemChannel.IconAlignment1"));
			this.errorProviderLocalModel.SetIconAlignment(this.checkBoxMapToSystemChannel, (ErrorIconAlignment)componentResourceManager.GetObject("checkBoxMapToSystemChannel.IconAlignment2"));
			this.errorProviderLocalModel.SetIconPadding(this.checkBoxMapToSystemChannel, (int)componentResourceManager.GetObject("checkBoxMapToSystemChannel.IconPadding"));
			this.errorProviderFormat.SetIconPadding(this.checkBoxMapToSystemChannel, (int)componentResourceManager.GetObject("checkBoxMapToSystemChannel.IconPadding1"));
			this.errorProviderGlobalModel.SetIconPadding(this.checkBoxMapToSystemChannel, (int)componentResourceManager.GetObject("checkBoxMapToSystemChannel.IconPadding2"));
			this.checkBoxMapToSystemChannel.Name = "checkBoxMapToSystemChannel";
			this.checkBoxMapToSystemChannel.UseVisualStyleBackColor = true;
			this.checkBoxMapToSystemChannel.CheckedChanged += new EventHandler(this.checkBoxMapToSystemChannel_CheckedChanged);
			componentResourceManager.ApplyResources(this.comboBoxSendOnChannel, "comboBoxSendOnChannel");
			this.comboBoxSendOnChannel.DropDownStyle = ComboBoxStyle.DropDownList;
			this.errorProviderLocalModel.SetError(this.comboBoxSendOnChannel, componentResourceManager.GetString("comboBoxSendOnChannel.Error"));
			this.errorProviderGlobalModel.SetError(this.comboBoxSendOnChannel, componentResourceManager.GetString("comboBoxSendOnChannel.Error1"));
			this.errorProviderFormat.SetError(this.comboBoxSendOnChannel, componentResourceManager.GetString("comboBoxSendOnChannel.Error2"));
			this.comboBoxSendOnChannel.FormattingEnabled = true;
			this.errorProviderGlobalModel.SetIconAlignment(this.comboBoxSendOnChannel, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxSendOnChannel.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.comboBoxSendOnChannel, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxSendOnChannel.IconAlignment1"));
			this.errorProviderLocalModel.SetIconAlignment(this.comboBoxSendOnChannel, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxSendOnChannel.IconAlignment2"));
			this.errorProviderGlobalModel.SetIconPadding(this.comboBoxSendOnChannel, (int)componentResourceManager.GetObject("comboBoxSendOnChannel.IconPadding"));
			this.errorProviderFormat.SetIconPadding(this.comboBoxSendOnChannel, (int)componentResourceManager.GetObject("comboBoxSendOnChannel.IconPadding1"));
			this.errorProviderLocalModel.SetIconPadding(this.comboBoxSendOnChannel, (int)componentResourceManager.GetObject("comboBoxSendOnChannel.IconPadding2"));
			this.comboBoxSendOnChannel.Name = "comboBoxSendOnChannel";
			this.comboBoxSendOnChannel.SelectedIndexChanged += new EventHandler(this.comboBoxSendOnChannel_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.labelSendChannel, "labelSendChannel");
			this.errorProviderFormat.SetError(this.labelSendChannel, componentResourceManager.GetString("labelSendChannel.Error"));
			this.errorProviderGlobalModel.SetError(this.labelSendChannel, componentResourceManager.GetString("labelSendChannel.Error1"));
			this.errorProviderLocalModel.SetError(this.labelSendChannel, componentResourceManager.GetString("labelSendChannel.Error2"));
			this.errorProviderLocalModel.SetIconAlignment(this.labelSendChannel, (ErrorIconAlignment)componentResourceManager.GetObject("labelSendChannel.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.labelSendChannel, (ErrorIconAlignment)componentResourceManager.GetObject("labelSendChannel.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.labelSendChannel, (ErrorIconAlignment)componentResourceManager.GetObject("labelSendChannel.IconAlignment2"));
			this.errorProviderGlobalModel.SetIconPadding(this.labelSendChannel, (int)componentResourceManager.GetObject("labelSendChannel.IconPadding"));
			this.errorProviderLocalModel.SetIconPadding(this.labelSendChannel, (int)componentResourceManager.GetObject("labelSendChannel.IconPadding1"));
			this.errorProviderFormat.SetIconPadding(this.labelSendChannel, (int)componentResourceManager.GetObject("labelSendChannel.IconPadding2"));
			this.labelSendChannel.Name = "labelSendChannel";
			componentResourceManager.ApplyResources(this.labelCANId, "labelCANId");
			this.errorProviderFormat.SetError(this.labelCANId, componentResourceManager.GetString("labelCANId.Error"));
			this.errorProviderGlobalModel.SetError(this.labelCANId, componentResourceManager.GetString("labelCANId.Error1"));
			this.errorProviderLocalModel.SetError(this.labelCANId, componentResourceManager.GetString("labelCANId.Error2"));
			this.errorProviderLocalModel.SetIconAlignment(this.labelCANId, (ErrorIconAlignment)componentResourceManager.GetObject("labelCANId.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.labelCANId, (ErrorIconAlignment)componentResourceManager.GetObject("labelCANId.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.labelCANId, (ErrorIconAlignment)componentResourceManager.GetObject("labelCANId.IconAlignment2"));
			this.errorProviderGlobalModel.SetIconPadding(this.labelCANId, (int)componentResourceManager.GetObject("labelCANId.IconPadding"));
			this.errorProviderLocalModel.SetIconPadding(this.labelCANId, (int)componentResourceManager.GetObject("labelCANId.IconPadding1"));
			this.errorProviderFormat.SetIconPadding(this.labelCANId, (int)componentResourceManager.GetObject("labelCANId.IconPadding2"));
			this.labelCANId.Name = "labelCANId";
			componentResourceManager.ApplyResources(this.labelLongLat2, "labelLongLat2");
			this.errorProviderFormat.SetError(this.labelLongLat2, componentResourceManager.GetString("labelLongLat2.Error"));
			this.errorProviderGlobalModel.SetError(this.labelLongLat2, componentResourceManager.GetString("labelLongLat2.Error1"));
			this.errorProviderLocalModel.SetError(this.labelLongLat2, componentResourceManager.GetString("labelLongLat2.Error2"));
			this.errorProviderLocalModel.SetIconAlignment(this.labelLongLat2, (ErrorIconAlignment)componentResourceManager.GetObject("labelLongLat2.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.labelLongLat2, (ErrorIconAlignment)componentResourceManager.GetObject("labelLongLat2.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.labelLongLat2, (ErrorIconAlignment)componentResourceManager.GetObject("labelLongLat2.IconAlignment2"));
			this.errorProviderGlobalModel.SetIconPadding(this.labelLongLat2, (int)componentResourceManager.GetObject("labelLongLat2.IconPadding"));
			this.errorProviderLocalModel.SetIconPadding(this.labelLongLat2, (int)componentResourceManager.GetObject("labelLongLat2.IconPadding1"));
			this.errorProviderFormat.SetIconPadding(this.labelLongLat2, (int)componentResourceManager.GetObject("labelLongLat2.IconPadding2"));
			this.labelLongLat2.Name = "labelLongLat2";
			componentResourceManager.ApplyResources(this.labelSatDetails, "labelSatDetails");
			this.errorProviderFormat.SetError(this.labelSatDetails, componentResourceManager.GetString("labelSatDetails.Error"));
			this.errorProviderGlobalModel.SetError(this.labelSatDetails, componentResourceManager.GetString("labelSatDetails.Error1"));
			this.errorProviderLocalModel.SetError(this.labelSatDetails, componentResourceManager.GetString("labelSatDetails.Error2"));
			this.errorProviderLocalModel.SetIconAlignment(this.labelSatDetails, (ErrorIconAlignment)componentResourceManager.GetObject("labelSatDetails.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.labelSatDetails, (ErrorIconAlignment)componentResourceManager.GetObject("labelSatDetails.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.labelSatDetails, (ErrorIconAlignment)componentResourceManager.GetObject("labelSatDetails.IconAlignment2"));
			this.errorProviderGlobalModel.SetIconPadding(this.labelSatDetails, (int)componentResourceManager.GetObject("labelSatDetails.IconPadding"));
			this.errorProviderLocalModel.SetIconPadding(this.labelSatDetails, (int)componentResourceManager.GetObject("labelSatDetails.IconPadding1"));
			this.errorProviderFormat.SetIconPadding(this.labelSatDetails, (int)componentResourceManager.GetObject("labelSatDetails.IconPadding2"));
			this.labelSatDetails.Name = "labelSatDetails";
			componentResourceManager.ApplyResources(this.textBoxCANId6, "textBoxCANId6");
			this.errorProviderGlobalModel.SetError(this.textBoxCANId6, componentResourceManager.GetString("textBoxCANId6.Error"));
			this.errorProviderFormat.SetError(this.textBoxCANId6, componentResourceManager.GetString("textBoxCANId6.Error1"));
			this.errorProviderLocalModel.SetError(this.textBoxCANId6, componentResourceManager.GetString("textBoxCANId6.Error2"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxCANId6, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxCANId6.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxCANId6, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxCANId6.IconAlignment1"));
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxCANId6, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxCANId6.IconAlignment2"));
			this.errorProviderGlobalModel.SetIconPadding(this.textBoxCANId6, (int)componentResourceManager.GetObject("textBoxCANId6.IconPadding"));
			this.errorProviderLocalModel.SetIconPadding(this.textBoxCANId6, (int)componentResourceManager.GetObject("textBoxCANId6.IconPadding1"));
			this.errorProviderFormat.SetIconPadding(this.textBoxCANId6, (int)componentResourceManager.GetObject("textBoxCANId6.IconPadding2"));
			this.textBoxCANId6.Name = "textBoxCANId6";
			this.textBoxCANId6.ReadOnly = true;
			componentResourceManager.ApplyResources(this.labelPrecision, "labelPrecision");
			this.errorProviderFormat.SetError(this.labelPrecision, componentResourceManager.GetString("labelPrecision.Error"));
			this.errorProviderGlobalModel.SetError(this.labelPrecision, componentResourceManager.GetString("labelPrecision.Error1"));
			this.errorProviderLocalModel.SetError(this.labelPrecision, componentResourceManager.GetString("labelPrecision.Error2"));
			this.errorProviderLocalModel.SetIconAlignment(this.labelPrecision, (ErrorIconAlignment)componentResourceManager.GetObject("labelPrecision.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.labelPrecision, (ErrorIconAlignment)componentResourceManager.GetObject("labelPrecision.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.labelPrecision, (ErrorIconAlignment)componentResourceManager.GetObject("labelPrecision.IconAlignment2"));
			this.errorProviderGlobalModel.SetIconPadding(this.labelPrecision, (int)componentResourceManager.GetObject("labelPrecision.IconPadding"));
			this.errorProviderLocalModel.SetIconPadding(this.labelPrecision, (int)componentResourceManager.GetObject("labelPrecision.IconPadding1"));
			this.errorProviderFormat.SetIconPadding(this.labelPrecision, (int)componentResourceManager.GetObject("labelPrecision.IconPadding2"));
			this.labelPrecision.Name = "labelPrecision";
			componentResourceManager.ApplyResources(this.textBoxCANId5, "textBoxCANId5");
			this.errorProviderGlobalModel.SetError(this.textBoxCANId5, componentResourceManager.GetString("textBoxCANId5.Error"));
			this.errorProviderFormat.SetError(this.textBoxCANId5, componentResourceManager.GetString("textBoxCANId5.Error1"));
			this.errorProviderLocalModel.SetError(this.textBoxCANId5, componentResourceManager.GetString("textBoxCANId5.Error2"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxCANId5, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxCANId5.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxCANId5, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxCANId5.IconAlignment1"));
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxCANId5, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxCANId5.IconAlignment2"));
			this.errorProviderGlobalModel.SetIconPadding(this.textBoxCANId5, (int)componentResourceManager.GetObject("textBoxCANId5.IconPadding"));
			this.errorProviderLocalModel.SetIconPadding(this.textBoxCANId5, (int)componentResourceManager.GetObject("textBoxCANId5.IconPadding1"));
			this.errorProviderFormat.SetIconPadding(this.textBoxCANId5, (int)componentResourceManager.GetObject("textBoxCANId5.IconPadding2"));
			this.textBoxCANId5.Name = "textBoxCANId5";
			this.textBoxCANId5.ReadOnly = true;
			componentResourceManager.ApplyResources(this.textBoxCANId4, "textBoxCANId4");
			this.errorProviderGlobalModel.SetError(this.textBoxCANId4, componentResourceManager.GetString("textBoxCANId4.Error"));
			this.errorProviderFormat.SetError(this.textBoxCANId4, componentResourceManager.GetString("textBoxCANId4.Error1"));
			this.errorProviderLocalModel.SetError(this.textBoxCANId4, componentResourceManager.GetString("textBoxCANId4.Error2"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxCANId4, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxCANId4.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxCANId4, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxCANId4.IconAlignment1"));
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxCANId4, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxCANId4.IconAlignment2"));
			this.errorProviderGlobalModel.SetIconPadding(this.textBoxCANId4, (int)componentResourceManager.GetObject("textBoxCANId4.IconPadding"));
			this.errorProviderLocalModel.SetIconPadding(this.textBoxCANId4, (int)componentResourceManager.GetObject("textBoxCANId4.IconPadding1"));
			this.errorProviderFormat.SetIconPadding(this.textBoxCANId4, (int)componentResourceManager.GetObject("textBoxCANId4.IconPadding2"));
			this.textBoxCANId4.Name = "textBoxCANId4";
			this.textBoxCANId4.ReadOnly = true;
			componentResourceManager.ApplyResources(this.textBoxCANId3, "textBoxCANId3");
			this.errorProviderGlobalModel.SetError(this.textBoxCANId3, componentResourceManager.GetString("textBoxCANId3.Error"));
			this.errorProviderFormat.SetError(this.textBoxCANId3, componentResourceManager.GetString("textBoxCANId3.Error1"));
			this.errorProviderLocalModel.SetError(this.textBoxCANId3, componentResourceManager.GetString("textBoxCANId3.Error2"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxCANId3, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxCANId3.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxCANId3, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxCANId3.IconAlignment1"));
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxCANId3, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxCANId3.IconAlignment2"));
			this.errorProviderGlobalModel.SetIconPadding(this.textBoxCANId3, (int)componentResourceManager.GetObject("textBoxCANId3.IconPadding"));
			this.errorProviderLocalModel.SetIconPadding(this.textBoxCANId3, (int)componentResourceManager.GetObject("textBoxCANId3.IconPadding1"));
			this.errorProviderFormat.SetIconPadding(this.textBoxCANId3, (int)componentResourceManager.GetObject("textBoxCANId3.IconPadding2"));
			this.textBoxCANId3.Name = "textBoxCANId3";
			this.textBoxCANId3.ReadOnly = true;
			componentResourceManager.ApplyResources(this.labelVelDir, "labelVelDir");
			this.errorProviderFormat.SetError(this.labelVelDir, componentResourceManager.GetString("labelVelDir.Error"));
			this.errorProviderGlobalModel.SetError(this.labelVelDir, componentResourceManager.GetString("labelVelDir.Error1"));
			this.errorProviderLocalModel.SetError(this.labelVelDir, componentResourceManager.GetString("labelVelDir.Error2"));
			this.errorProviderLocalModel.SetIconAlignment(this.labelVelDir, (ErrorIconAlignment)componentResourceManager.GetObject("labelVelDir.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.labelVelDir, (ErrorIconAlignment)componentResourceManager.GetObject("labelVelDir.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.labelVelDir, (ErrorIconAlignment)componentResourceManager.GetObject("labelVelDir.IconAlignment2"));
			this.errorProviderGlobalModel.SetIconPadding(this.labelVelDir, (int)componentResourceManager.GetObject("labelVelDir.IconPadding"));
			this.errorProviderLocalModel.SetIconPadding(this.labelVelDir, (int)componentResourceManager.GetObject("labelVelDir.IconPadding1"));
			this.errorProviderFormat.SetIconPadding(this.labelVelDir, (int)componentResourceManager.GetObject("labelVelDir.IconPadding2"));
			this.labelVelDir.Name = "labelVelDir";
			componentResourceManager.ApplyResources(this.textBoxCANId2, "textBoxCANId2");
			this.errorProviderGlobalModel.SetError(this.textBoxCANId2, componentResourceManager.GetString("textBoxCANId2.Error"));
			this.errorProviderFormat.SetError(this.textBoxCANId2, componentResourceManager.GetString("textBoxCANId2.Error1"));
			this.errorProviderLocalModel.SetError(this.textBoxCANId2, componentResourceManager.GetString("textBoxCANId2.Error2"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxCANId2, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxCANId2.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxCANId2, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxCANId2.IconAlignment1"));
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxCANId2, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxCANId2.IconAlignment2"));
			this.errorProviderGlobalModel.SetIconPadding(this.textBoxCANId2, (int)componentResourceManager.GetObject("textBoxCANId2.IconPadding"));
			this.errorProviderLocalModel.SetIconPadding(this.textBoxCANId2, (int)componentResourceManager.GetObject("textBoxCANId2.IconPadding1"));
			this.errorProviderFormat.SetIconPadding(this.textBoxCANId2, (int)componentResourceManager.GetObject("textBoxCANId2.IconPadding2"));
			this.textBoxCANId2.Name = "textBoxCANId2";
			this.textBoxCANId2.ReadOnly = true;
			componentResourceManager.ApplyResources(this.textBoxCANId1, "textBoxCANId1");
			this.errorProviderGlobalModel.SetError(this.textBoxCANId1, componentResourceManager.GetString("textBoxCANId1.Error"));
			this.errorProviderFormat.SetError(this.textBoxCANId1, componentResourceManager.GetString("textBoxCANId1.Error1"));
			this.errorProviderLocalModel.SetError(this.textBoxCANId1, componentResourceManager.GetString("textBoxCANId1.Error2"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxCANId1, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxCANId1.IconAlignment"));
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxCANId1, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxCANId1.IconAlignment1"));
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxCANId1, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxCANId1.IconAlignment2"));
			this.errorProviderGlobalModel.SetIconPadding(this.textBoxCANId1, (int)componentResourceManager.GetObject("textBoxCANId1.IconPadding"));
			this.errorProviderLocalModel.SetIconPadding(this.textBoxCANId1, (int)componentResourceManager.GetObject("textBoxCANId1.IconPadding1"));
			this.errorProviderFormat.SetIconPadding(this.textBoxCANId1, (int)componentResourceManager.GetObject("textBoxCANId1.IconPadding2"));
			this.textBoxCANId1.Name = "textBoxCANId1";
			this.textBoxCANId1.Validating += new CancelEventHandler(this.textBoxCANId1_Validating);
			componentResourceManager.ApplyResources(this.labelLongLat, "labelLongLat");
			this.errorProviderFormat.SetError(this.labelLongLat, componentResourceManager.GetString("labelLongLat.Error"));
			this.errorProviderGlobalModel.SetError(this.labelLongLat, componentResourceManager.GetString("labelLongLat.Error1"));
			this.errorProviderLocalModel.SetError(this.labelLongLat, componentResourceManager.GetString("labelLongLat.Error2"));
			this.errorProviderLocalModel.SetIconAlignment(this.labelLongLat, (ErrorIconAlignment)componentResourceManager.GetObject("labelLongLat.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.labelLongLat, (ErrorIconAlignment)componentResourceManager.GetObject("labelLongLat.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.labelLongLat, (ErrorIconAlignment)componentResourceManager.GetObject("labelLongLat.IconAlignment2"));
			this.errorProviderGlobalModel.SetIconPadding(this.labelLongLat, (int)componentResourceManager.GetObject("labelLongLat.IconPadding"));
			this.errorProviderLocalModel.SetIconPadding(this.labelLongLat, (int)componentResourceManager.GetObject("labelLongLat.IconPadding1"));
			this.errorProviderFormat.SetIconPadding(this.labelLongLat, (int)componentResourceManager.GetObject("labelLongLat.IconPadding2"));
			this.labelLongLat.Name = "labelLongLat";
			componentResourceManager.ApplyResources(this.labelDateTimeAltitude, "labelDateTimeAltitude");
			this.errorProviderFormat.SetError(this.labelDateTimeAltitude, componentResourceManager.GetString("labelDateTimeAltitude.Error"));
			this.errorProviderGlobalModel.SetError(this.labelDateTimeAltitude, componentResourceManager.GetString("labelDateTimeAltitude.Error1"));
			this.errorProviderLocalModel.SetError(this.labelDateTimeAltitude, componentResourceManager.GetString("labelDateTimeAltitude.Error2"));
			this.errorProviderLocalModel.SetIconAlignment(this.labelDateTimeAltitude, (ErrorIconAlignment)componentResourceManager.GetObject("labelDateTimeAltitude.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.labelDateTimeAltitude, (ErrorIconAlignment)componentResourceManager.GetObject("labelDateTimeAltitude.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.labelDateTimeAltitude, (ErrorIconAlignment)componentResourceManager.GetObject("labelDateTimeAltitude.IconAlignment2"));
			this.errorProviderGlobalModel.SetIconPadding(this.labelDateTimeAltitude, (int)componentResourceManager.GetObject("labelDateTimeAltitude.IconPadding"));
			this.errorProviderLocalModel.SetIconPadding(this.labelDateTimeAltitude, (int)componentResourceManager.GetObject("labelDateTimeAltitude.IconPadding1"));
			this.errorProviderFormat.SetIconPadding(this.labelDateTimeAltitude, (int)componentResourceManager.GetObject("labelDateTimeAltitude.IconPadding2"));
			this.labelDateTimeAltitude.Name = "labelDateTimeAltitude";
			componentResourceManager.ApplyResources(this.labelGPSData, "labelGPSData");
			this.errorProviderFormat.SetError(this.labelGPSData, componentResourceManager.GetString("labelGPSData.Error"));
			this.errorProviderGlobalModel.SetError(this.labelGPSData, componentResourceManager.GetString("labelGPSData.Error1"));
			this.errorProviderLocalModel.SetError(this.labelGPSData, componentResourceManager.GetString("labelGPSData.Error2"));
			this.errorProviderLocalModel.SetIconAlignment(this.labelGPSData, (ErrorIconAlignment)componentResourceManager.GetObject("labelGPSData.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.labelGPSData, (ErrorIconAlignment)componentResourceManager.GetObject("labelGPSData.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.labelGPSData, (ErrorIconAlignment)componentResourceManager.GetObject("labelGPSData.IconAlignment2"));
			this.errorProviderGlobalModel.SetIconPadding(this.labelGPSData, (int)componentResourceManager.GetObject("labelGPSData.IconPadding"));
			this.errorProviderLocalModel.SetIconPadding(this.labelGPSData, (int)componentResourceManager.GetObject("labelGPSData.IconPadding1"));
			this.errorProviderFormat.SetIconPadding(this.labelGPSData, (int)componentResourceManager.GetObject("labelGPSData.IconPadding2"));
			this.labelGPSData.Name = "labelGPSData";
			this.errorProviderFormat.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderFormat.ContainerControl = this;
			componentResourceManager.ApplyResources(this.errorProviderFormat, "errorProviderFormat");
			this.errorProviderLocalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderLocalModel.ContainerControl = this;
			componentResourceManager.ApplyResources(this.errorProviderLocalModel, "errorProviderLocalModel");
			this.errorProviderGlobalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderGlobalModel.ContainerControl = this;
			componentResourceManager.ApplyResources(this.errorProviderGlobalModel, "errorProviderGlobalModel");
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.groupBoxGPS);
			this.errorProviderLocalModel.SetError(this, componentResourceManager.GetString("$this.Error"));
			this.errorProviderFormat.SetError(this, componentResourceManager.GetString("$this.Error1"));
			this.errorProviderGlobalModel.SetError(this, componentResourceManager.GetString("$this.Error2"));
			this.errorProviderGlobalModel.SetIconAlignment(this, (ErrorIconAlignment)componentResourceManager.GetObject("$this.IconAlignment"));
			this.errorProviderLocalModel.SetIconAlignment(this, (ErrorIconAlignment)componentResourceManager.GetObject("$this.IconAlignment1"));
			this.errorProviderFormat.SetIconAlignment(this, (ErrorIconAlignment)componentResourceManager.GetObject("$this.IconAlignment2"));
			this.errorProviderFormat.SetIconPadding(this, (int)componentResourceManager.GetObject("$this.IconPadding"));
			this.errorProviderGlobalModel.SetIconPadding(this, (int)componentResourceManager.GetObject("$this.IconPadding1"));
			this.errorProviderLocalModel.SetIconPadding(this, (int)componentResourceManager.GetObject("$this.IconPadding2"));
			base.Name = "GPSGL2000";
			this.groupBoxGPS.ResumeLayout(false);
			this.groupBoxGPS.PerformLayout();
			((ISupportInitialize)this.errorProviderFormat).EndInit();
			((ISupportInitialize)this.errorProviderLocalModel).EndInit();
			((ISupportInitialize)this.errorProviderGlobalModel).EndInit();
			base.ResumeLayout(false);
		}
	}
}
