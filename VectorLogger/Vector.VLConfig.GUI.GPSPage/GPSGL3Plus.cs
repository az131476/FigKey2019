using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GUI.Common;
using Vector.VLConfig.Properties;
using Vector.VLConfig.ValidationFramework;

namespace Vector.VLConfig.GUI.GPSPage
{
	public class GPSGL3Plus : UserControl
	{
		private GUIElementManager_Control guiElementManager;

		private CustomErrorProvider customErrorProvider;

		private PageValidator pageValidator;

		private GPSConfiguration gpsConfig;

		private DisplayMode displayMode;

		private bool isInitControls;

		private IContainer components;

		private GroupBox groupBoxGPS;

		private TextBox textBoxDatabase;

		private Label labelDatabase;

		private CheckBox checkBoxMapToSystemChannel;

		private ComboBox comboBoxCANChannel;

		private Label labelSendChannel;

		private Label labelCANId;

		private TextBox textBoxCANId3;

		private Label labelVelDir;

		private TextBox textBoxCANId2;

		private TextBox textBoxCANId1;

		private Label labelLongLat;

		private Label labelDateTimeAltitude;

		private Label labelGPSData;

		private Button buttonSelectDatabase;

		private ErrorProvider errorProviderFormat;

		private ErrorProvider errorProviderLocalModel;

		private ErrorProvider errorProviderGlobalModel;

		private ToolTip toolTip;

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

		public IApplicationDatabaseManager ApplicationDatabaseManager
		{
			get;
			set;
		}

		public GPSGL3Plus()
		{
			this.InitializeComponent();
			this.guiElementManager = new GUIElementManager_Control();
			this.customErrorProvider = new CustomErrorProvider(this.errorProviderFormat);
			this.customErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.LocalModelError, this.errorProviderLocalModel);
			this.customErrorProvider.General.RegisterErrorProviderForErrorClass(ValidationErrorClass.GlobalModelError, this.errorProviderGlobalModel);
			this.pageValidator = new PageValidator(this.customErrorProvider);
		}

		public void Init()
		{
			this.isInitControls = true;
			this.InitChannelComboBox();
			this.isInitControls = false;
		}

		private void InitChannelComboBox()
		{
			this.comboBoxCANChannel.Items.Clear();
			for (uint num = 1u; num <= this.ModelValidator.LoggerSpecifics.CAN.NumberOfChannels; num += 1u)
			{
				this.comboBoxCANChannel.Items.Add(GUIUtil.MapCANChannelNumber2String(num));
			}
			if (this.comboBoxCANChannel.Items.Count > 0)
			{
				if ((long)this.comboBoxCANChannel.Items.Count >= (long)((ulong)this.ModelValidator.LoggerSpecifics.GPS.DefaultLogGPSChannel))
				{
					this.comboBoxCANChannel.SelectedIndex = (int)(this.ModelValidator.LoggerSpecifics.GPS.DefaultLogGPSChannel - 1u);
					return;
				}
				this.comboBoxCANChannel.SelectedIndex = 0;
			}
		}

		private void UpdateGUI()
		{
			if (this.gpsConfig == null)
			{
				return;
			}
			this.isInitControls = true;
			this.checkBoxMapToSystemChannel.Checked = this.gpsConfig.MapToSystemChannel.Value;
			this.textBoxDatabase.Text = this.gpsConfig.Database.Value;
			this.UpdateCANIDTextBoxes();
			if (!this.pageValidator.General.HasFormatError(this.gpsConfig.CANChannel))
			{
				if (this.gpsConfig.CANChannel.Value <= this.ModelValidator.LoggerSpecifics.CAN.NumberOfChannels)
				{
					this.comboBoxCANChannel.SelectedItem = GUIUtil.MapCANChannelNumber2String(this.gpsConfig.CANChannel.Value);
				}
				else
				{
					this.comboBoxCANChannel.SelectedItem = GUIUtil.MapCANChannelNumber2String(this.gpsConfig.CANChannel.Value) + Resources.VirtualChannelPostfix;
				}
			}
			this.EnableDisableControls();
			this.isInitControls = false;
			this.ValidateInput();
		}

		private bool AcceptFileDrop(DragEventArgs e, out string accteptedFile)
		{
			bool result = false;
			accteptedFile = "";
			if (e.Data.GetDataPresent(DataFormats.FileDrop))
			{
				string[] array = (string[])e.Data.GetData(DataFormats.FileDrop);
				if (array.Length == 1)
				{
					try
					{
						string extension = Path.GetExtension(array[0]);
						result = (string.Compare(extension, Vocabulary.FileExtensionDotDBC, true) == 0);
						accteptedFile = array[0];
					}
					catch
					{
					}
				}
			}
			return result;
		}

		private void EnableDisableControls()
		{
			this.comboBoxCANChannel.Enabled = this.checkBoxMapToSystemChannel.Checked;
			this.textBoxDatabase.Enabled = this.checkBoxMapToSystemChannel.Checked;
			this.buttonSelectDatabase.Enabled = this.checkBoxMapToSystemChannel.Checked;
			this.textBoxCANId1.Enabled = this.checkBoxMapToSystemChannel.Checked;
			this.textBoxCANId2.Enabled = this.checkBoxMapToSystemChannel.Checked;
			this.textBoxCANId3.Enabled = this.checkBoxMapToSystemChannel.Checked;
		}

		private void UpdateCANIDTextBoxes()
		{
			this.textBoxCANId1.Text = "";
			this.textBoxCANId2.Text = "";
			this.textBoxCANId3.Text = "";
			if (this.gpsConfig.CANIdDateTimeAltitude.Value > 0u)
			{
				this.textBoxCANId1.Text = GUIUtil.CANIdToDisplayString(this.gpsConfig.CANIdDateTimeAltitude.Value, false);
			}
			if (this.gpsConfig.CANIdLongitudeLatitude.Value > 0u)
			{
				this.textBoxCANId2.Text = GUIUtil.CANIdToDisplayString(this.gpsConfig.CANIdLongitudeLatitude.Value, false);
			}
			if (this.gpsConfig.CANIdVelocityDirection.Value > 0u)
			{
				this.textBoxCANId3.Text = GUIUtil.CANIdToDisplayString(this.gpsConfig.CANIdVelocityDirection.Value, false);
			}
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
			bool flag3;
			flag &= this.pageValidator.Control.UpdateModel<bool>(this.checkBoxMapToSystemChannel.Checked, this.gpsConfig.MapToSystemChannel, this.guiElementManager.GetGUIElement(this.checkBoxMapToSystemChannel), out flag3);
			flag2 |= flag3;
			if (this.gpsConfig.MapToSystemChannel.Value)
			{
				flag &= this.pageValidator.Control.UpdateModel<uint>(GUIUtil.MapCANChannelString2Number(this.comboBoxCANChannel.SelectedItem.ToString()), this.gpsConfig.CANChannel, this.guiElementManager.GetGUIElement(this.comboBoxCANChannel), out flag3);
				flag2 |= flag3;
				flag &= this.pageValidator.Control.UpdateModel<string>(this.textBoxDatabase.Text, this.gpsConfig.Database, this.guiElementManager.GetGUIElement(this.textBoxDatabase), out flag3);
				uint[] array;
				int longitudeLatitudeMode;
				double altitudeFactor;
				this.ApplicationDatabaseManager.ResolveGPSMessageSymbolsInDatabase(this.ModelValidator.GetAbsoluteFilePath(this.gpsConfig.Database.Value), out array, out longitudeLatitudeMode, out altitudeFactor);
				bool flag4;
				this.pageValidator.Control.UpdateModel<uint>(array[0], this.gpsConfig.CANIdDateTimeAltitude, this.guiElementManager.GetGUIElement(this.textBoxCANId1), out flag4);
				this.pageValidator.Control.UpdateModel<uint>(array[1], this.gpsConfig.CANIdLongitudeLatitude, this.guiElementManager.GetGUIElement(this.textBoxCANId2), out flag4);
				this.pageValidator.Control.UpdateModel<uint>(array[2], this.gpsConfig.CANIdVelocityDirection, this.guiElementManager.GetGUIElement(this.textBoxCANId3), out flag4);
				this.gpsConfig.LongitudeLatitudeMode = longitudeLatitudeMode;
				this.gpsConfig.AltitudeFactor = altitudeFactor;
				flag2 |= flag3;
			}
			flag &= this.ModelValidator.Validate(this.gpsConfig, flag2, this.pageValidator);
			this.pageValidator.General.ActivateErrorProvidersForFormatAndModelErrors();
			return flag;
		}

		private void checkBoxMapToSystemChannel_CheckedChanged(object sender, EventArgs e)
		{
			this.EnableDisableControls();
			if (this.isInitControls)
			{
				return;
			}
			if (this.gpsConfig.Database.Value.Length == 0 && this.checkBoxMapToSystemChannel.Checked && GenericOpenFileDialog.ShowDialog(Resources.OpenCANgpsDatabase, FileType.DBCDatabase) == DialogResult.OK)
			{
				this.textBoxDatabase.Text = this.ModelValidator.GetFilePathRelativeToConfiguration(GenericOpenFileDialog.FileName);
			}
			this.ValidateInput();
		}

		private void comboBoxCANChannel_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.ValidateInput();
		}

		private void buttonSelectDatabase_Click(object sender, EventArgs e)
		{
			if (GenericOpenFileDialog.ShowDialog(Resources.OpenCANgpsDatabase, FileType.DBCDatabase) == DialogResult.OK)
			{
				this.textBoxDatabase.Text = this.ModelValidator.GetFilePathRelativeToConfiguration(GenericOpenFileDialog.FileName);
				this.ValidateInput();
			}
		}

		private void textBoxDatabase_DoubleClick(object sender, EventArgs e)
		{
			if (this.textBoxDatabase.Text.Length > 0)
			{
				FileSystemServices.LaunchFile(this.textBoxDatabase.Text);
			}
		}

		private void textBoxDatabase_MouseEnter(object sender, EventArgs e)
		{
			this.toolTip.SetToolTip(this.textBoxDatabase, this.textBoxDatabase.Text);
		}

		private void textBoxDatabase_DragDrop(object sender, DragEventArgs e)
		{
			string text;
			if (this.AcceptFileDrop(e, out text))
			{
				this.textBoxDatabase.Text = text;
				this.ValidateInput();
			}
		}

		private void textBoxDatabase_DragEnter(object sender, DragEventArgs e)
		{
			string text;
			if (this.AcceptFileDrop(e, out text))
			{
				e.Effect = DragDropEffects.Copy;
				return;
			}
			e.Effect = DragDropEffects.None;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(GPSGL3Plus));
			this.groupBoxGPS = new GroupBox();
			this.buttonSelectDatabase = new Button();
			this.textBoxDatabase = new TextBox();
			this.labelDatabase = new Label();
			this.checkBoxMapToSystemChannel = new CheckBox();
			this.comboBoxCANChannel = new ComboBox();
			this.labelSendChannel = new Label();
			this.labelCANId = new Label();
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
			this.toolTip = new ToolTip(this.components);
			this.groupBoxGPS.SuspendLayout();
			((ISupportInitialize)this.errorProviderFormat).BeginInit();
			((ISupportInitialize)this.errorProviderLocalModel).BeginInit();
			((ISupportInitialize)this.errorProviderGlobalModel).BeginInit();
			base.SuspendLayout();
			this.groupBoxGPS.Controls.Add(this.buttonSelectDatabase);
			this.groupBoxGPS.Controls.Add(this.textBoxDatabase);
			this.groupBoxGPS.Controls.Add(this.labelDatabase);
			this.groupBoxGPS.Controls.Add(this.checkBoxMapToSystemChannel);
			this.groupBoxGPS.Controls.Add(this.comboBoxCANChannel);
			this.groupBoxGPS.Controls.Add(this.labelSendChannel);
			this.groupBoxGPS.Controls.Add(this.labelCANId);
			this.groupBoxGPS.Controls.Add(this.textBoxCANId3);
			this.groupBoxGPS.Controls.Add(this.labelVelDir);
			this.groupBoxGPS.Controls.Add(this.textBoxCANId2);
			this.groupBoxGPS.Controls.Add(this.textBoxCANId1);
			this.groupBoxGPS.Controls.Add(this.labelLongLat);
			this.groupBoxGPS.Controls.Add(this.labelDateTimeAltitude);
			this.groupBoxGPS.Controls.Add(this.labelGPSData);
			componentResourceManager.ApplyResources(this.groupBoxGPS, "groupBoxGPS");
			this.groupBoxGPS.Name = "groupBoxGPS";
			this.groupBoxGPS.TabStop = false;
			componentResourceManager.ApplyResources(this.buttonSelectDatabase, "buttonSelectDatabase");
			this.buttonSelectDatabase.Name = "buttonSelectDatabase";
			this.buttonSelectDatabase.UseVisualStyleBackColor = true;
			this.buttonSelectDatabase.Click += new EventHandler(this.buttonSelectDatabase_Click);
			this.textBoxDatabase.AllowDrop = true;
			this.textBoxDatabase.Cursor = Cursors.IBeam;
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxDatabase, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxDatabase.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxDatabase, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxDatabase.IconAlignment1"));
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxDatabase, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxDatabase.IconAlignment2"));
			componentResourceManager.ApplyResources(this.textBoxDatabase, "textBoxDatabase");
			this.textBoxDatabase.Name = "textBoxDatabase";
			this.textBoxDatabase.ReadOnly = true;
			this.textBoxDatabase.DragDrop += new DragEventHandler(this.textBoxDatabase_DragDrop);
			this.textBoxDatabase.DragEnter += new DragEventHandler(this.textBoxDatabase_DragEnter);
			this.textBoxDatabase.DoubleClick += new EventHandler(this.textBoxDatabase_DoubleClick);
			this.textBoxDatabase.MouseEnter += new EventHandler(this.textBoxDatabase_MouseEnter);
			componentResourceManager.ApplyResources(this.labelDatabase, "labelDatabase");
			this.labelDatabase.Name = "labelDatabase";
			componentResourceManager.ApplyResources(this.checkBoxMapToSystemChannel, "checkBoxMapToSystemChannel");
			this.checkBoxMapToSystemChannel.Name = "checkBoxMapToSystemChannel";
			this.checkBoxMapToSystemChannel.UseVisualStyleBackColor = true;
			this.checkBoxMapToSystemChannel.CheckedChanged += new EventHandler(this.checkBoxMapToSystemChannel_CheckedChanged);
			this.comboBoxCANChannel.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxCANChannel.FormattingEnabled = true;
			this.errorProviderLocalModel.SetIconAlignment(this.comboBoxCANChannel, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxCANChannel.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.comboBoxCANChannel, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxCANChannel.IconAlignment1"));
			this.errorProviderGlobalModel.SetIconAlignment(this.comboBoxCANChannel, (ErrorIconAlignment)componentResourceManager.GetObject("comboBoxCANChannel.IconAlignment2"));
			componentResourceManager.ApplyResources(this.comboBoxCANChannel, "comboBoxCANChannel");
			this.comboBoxCANChannel.Name = "comboBoxCANChannel";
			this.comboBoxCANChannel.SelectedIndexChanged += new EventHandler(this.comboBoxCANChannel_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.labelSendChannel, "labelSendChannel");
			this.labelSendChannel.Name = "labelSendChannel";
			componentResourceManager.ApplyResources(this.labelCANId, "labelCANId");
			this.labelCANId.Name = "labelCANId";
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxCANId3, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxCANId3.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxCANId3, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxCANId3.IconAlignment1"));
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxCANId3, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxCANId3.IconAlignment2"));
			componentResourceManager.ApplyResources(this.textBoxCANId3, "textBoxCANId3");
			this.textBoxCANId3.Name = "textBoxCANId3";
			this.textBoxCANId3.ReadOnly = true;
			componentResourceManager.ApplyResources(this.labelVelDir, "labelVelDir");
			this.labelVelDir.Name = "labelVelDir";
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxCANId2, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxCANId2.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxCANId2, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxCANId2.IconAlignment1"));
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxCANId2, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxCANId2.IconAlignment2"));
			componentResourceManager.ApplyResources(this.textBoxCANId2, "textBoxCANId2");
			this.textBoxCANId2.Name = "textBoxCANId2";
			this.textBoxCANId2.ReadOnly = true;
			this.errorProviderGlobalModel.SetIconAlignment(this.textBoxCANId1, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxCANId1.IconAlignment"));
			this.errorProviderFormat.SetIconAlignment(this.textBoxCANId1, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxCANId1.IconAlignment1"));
			this.errorProviderLocalModel.SetIconAlignment(this.textBoxCANId1, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxCANId1.IconAlignment2"));
			componentResourceManager.ApplyResources(this.textBoxCANId1, "textBoxCANId1");
			this.textBoxCANId1.Name = "textBoxCANId1";
			this.textBoxCANId1.ReadOnly = true;
			componentResourceManager.ApplyResources(this.labelLongLat, "labelLongLat");
			this.labelLongLat.Name = "labelLongLat";
			componentResourceManager.ApplyResources(this.labelDateTimeAltitude, "labelDateTimeAltitude");
			this.labelDateTimeAltitude.Name = "labelDateTimeAltitude";
			componentResourceManager.ApplyResources(this.labelGPSData, "labelGPSData");
			this.labelGPSData.Name = "labelGPSData";
			this.errorProviderFormat.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderFormat.ContainerControl = this;
			this.errorProviderLocalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderLocalModel.ContainerControl = this;
			this.errorProviderGlobalModel.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProviderGlobalModel.ContainerControl = this;
			componentResourceManager.ApplyResources(this.errorProviderGlobalModel, "errorProviderGlobalModel");
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.groupBoxGPS);
			base.Name = "GPSGL3Plus";
			this.groupBoxGPS.ResumeLayout(false);
			this.groupBoxGPS.PerformLayout();
			((ISupportInitialize)this.errorProviderFormat).EndInit();
			((ISupportInitialize)this.errorProviderLocalModel).EndInit();
			((ISupportInitialize)this.errorProviderGlobalModel).EndInit();
			base.ResumeLayout(false);
		}
	}
}
