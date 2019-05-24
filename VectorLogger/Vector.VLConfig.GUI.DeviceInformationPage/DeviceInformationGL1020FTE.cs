using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.HardwareAccess;
using Vector.VLConfig.HardwareAccess.HardwareAbstraction;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.DeviceInformationPage
{
	internal class DeviceInformationGL1020FTE : UserControl
	{
		private ILoggerDevice device;

		private IContainer components;

		private GroupBox groupBoxDevice;

		private TableLayoutPanel tableLayoutPanel1;

		private Label labelDeviceType;

		private Label labelSerialNumber;

		private Label labelFirmwareVersion;

		private Label labelCapacity;

		private Label labelFreeSpace;

		private Label labelTriggeredFiles;

		private Label labelRecordingFiles;

		private Label labelConfiguration;

		private Label labelCAN1;

		private Label labelCAN2;

		private Label labelAnalogInputs;

		private Label labelConfigFile;

		private Label labelInstalledLicenses;

		private Label labelTypeValue;

		private Label labelSerialNumberValue;

		private Label labelFirmwareVersionValue;

		private Label labelCapacityValue;

		private Label labelFreeSpaceValue;

		private Label labelTriggeredFilesValue;

		private Label labelRecordingFilesValue;

		private Label labelCAN1Value;

		private Label labelCAN2Value;

		private Label labelAnalogInputsValue;

		private Label labelConfigFileValue;

		private Label labelInstalledLicensesValue;

		private Label labelNoLoggerDeviceConnected;

		private Label labelLoggerConnectedButInaccessible;

		private RichTextBox richTextBoxDeviceConnectedButCardEmpty;

		private Label labelVehicleName;

		private Label labelVehicleNameValue;

		private Label labelPackAndGo;

		private Label mLabelPackAndGo;

		private Label labelAnalysisPackage;

		private Label labelAnalysisPackageValue;

		public bool IsLoggerConnectedDisplayMode
		{
			get;
			set;
		}

		public DeviceInformationGL1020FTE()
		{
			this.InitializeComponent();
			this.device = null;
			this.richTextBoxDeviceConnectedButCardEmpty.Rtf = Resources.DeviceGL1000ConnectedButCardEmpty;
			this.IsLoggerConnectedDisplayMode = true;
		}

		public void Init(ILoggerDevice currDevice)
		{
			this.device = currDevice;
			if (this.device == null || this.device.LoggerType != LoggerType.GL1020FTE)
			{
				this.groupBoxDevice.Visible = false;
				this.labelLoggerConnectedButInaccessible.Visible = false;
				this.richTextBoxDeviceConnectedButCardEmpty.Visible = false;
				this.labelNoLoggerDeviceConnected.Visible = true;
				return;
			}
			if (!this.device.IsMemoryCardReady)
			{
				this.groupBoxDevice.Visible = false;
				this.labelLoggerConnectedButInaccessible.Visible = true;
				this.richTextBoxDeviceConnectedButCardEmpty.Visible = false;
				this.labelNoLoggerDeviceConnected.Visible = false;
				return;
			}
			if (!this.device.HasLoggerInfo)
			{
				this.groupBoxDevice.Visible = true;
				this.labelLoggerConnectedButInaccessible.Visible = false;
				this.richTextBoxDeviceConnectedButCardEmpty.Visible = this.IsLoggerConnectedDisplayMode;
				this.labelNoLoggerDeviceConnected.Visible = false;
				this.labelSerialNumberValue.Text = Resources.Unknown;
				this.labelVehicleNameValue.Text = Resources.Unknown;
				if (!string.IsNullOrEmpty(this.device.Name))
				{
					this.labelConfigFileValue.Text = string.Format("{0} ({1})", this.device.Name, this.device.CompileDateTime.ToString());
				}
				else
				{
					this.labelConfigFileValue.Text = "-";
				}
				this.PopulatePackAndGoLinkList();
				this.labelAnalysisPackageValue.Text = Resources.Unknown;
				this.labelInstalledLicensesValue.Text = Resources.Unknown;
				this.labelFirmwareVersionValue.Text = Resources.Unknown;
				if (this.device.IsLocatedAtNetwork)
				{
					this.labelFreeSpaceValue.Text = Resources.Unknown;
					this.labelCapacityValue.Text = Resources.Unknown;
				}
				else
				{
					if (this.device.LogFileStorage.FreeSpace == 0L)
					{
						this.labelFreeSpaceValue.Text = Resources.Unknown;
					}
					else if (!this.IsLoggerConnectedDisplayMode && !string.IsNullOrEmpty(this.device.HardwareKey) && this.device.HardwareKey.Length > 3)
					{
						this.labelFreeSpaceValue.Text = GUIUtil.GetSizeStringMBForBytes(this.device.LogFileStorage.FreeSpace);
					}
					else
					{
						this.labelFreeSpaceValue.Text = string.Format("{0} {1}", GUIUtil.GetSizeStringMBForBytes(this.device.LogFileStorage.FreeSpace), GUIUtil.GetPercentageStringForFreeSpace(this.device.LogFileStorage.TotalSpace, this.device.LogFileStorage.FreeSpace));
					}
					if (this.device.LogFileStorage.TotalSpace == 0L)
					{
						this.labelCapacityValue.Text = Resources.Unknown;
					}
					else
					{
						this.labelCapacityValue.Text = GUIUtil.GetSizeStringMBForBytes(this.device.LogFileStorage.TotalSpace);
					}
				}
				this.labelTriggeredFilesValue.Text = this.device.LogFileStorage.NumberOfTriggeredBuffers.ToString();
				this.labelRecordingFilesValue.Text = this.device.LogFileStorage.NumberOfRecordingBuffers.ToString();
				this.labelCAN1Value.Text = Resources.Unknown;
				this.labelCAN2Value.Text = Resources.Unknown;
			}
			else
			{
				this.groupBoxDevice.Visible = true;
				this.labelLoggerConnectedButInaccessible.Visible = false;
				this.richTextBoxDeviceConnectedButCardEmpty.Visible = false;
				this.labelNoLoggerDeviceConnected.Visible = false;
				this.labelSerialNumberValue.Text = this.device.SerialNumber;
				this.labelVehicleNameValue.Text = this.device.VehicleName;
				if (!string.IsNullOrEmpty(this.device.Name))
				{
					this.labelConfigFileValue.Text = string.Format("{0} ({1})", this.device.Name, this.device.CompileDateTime.ToString());
				}
				else
				{
					this.labelConfigFileValue.Text = "-";
				}
				this.PopulatePackAndGoLinkList();
				this.labelAnalysisPackageValue.Text = "-";
				string analysisPackagePath = this.device.GetAnalysisPackagePath();
				if (!string.IsNullOrEmpty(analysisPackagePath))
				{
					this.labelAnalysisPackageValue.Text = analysisPackagePath;
				}
				if (!string.IsNullOrEmpty(this.device.InstalledLicenses))
				{
					this.labelInstalledLicensesValue.Text = this.device.InstalledLicenses;
				}
				else
				{
					this.labelInstalledLicensesValue.Text = Resources.NoInstalledLicense;
				}
				this.labelFirmwareVersionValue.Text = this.device.FirmwareVersion;
				if (this.device.IsLocatedAtNetwork)
				{
					this.labelFreeSpaceValue.Text = Resources.Unknown;
					this.labelCapacityValue.Text = Resources.Unknown;
				}
				else
				{
					if (!this.IsLoggerConnectedDisplayMode && !string.IsNullOrEmpty(this.device.HardwareKey) && this.device.HardwareKey.Length > 3)
					{
						this.labelFreeSpaceValue.Text = GUIUtil.GetSizeStringMBForBytes(this.device.LogFileStorage.FreeSpace);
					}
					else
					{
						this.labelFreeSpaceValue.Text = string.Format("{0} {1}", GUIUtil.GetSizeStringMBForBytes(this.device.LogFileStorage.FreeSpace), GUIUtil.GetPercentageStringForFreeSpace(this.device.LogFileStorage.TotalSpace, this.device.LogFileStorage.FreeSpace));
					}
					this.labelCapacityValue.Text = GUIUtil.GetSizeStringMBForBytes(this.device.LogFileStorage.TotalSpace);
				}
				this.labelTriggeredFilesValue.Text = this.device.LogFileStorage.NumberOfTriggeredBuffers.ToString();
				this.labelRecordingFilesValue.Text = this.device.LogFileStorage.NumberOfRecordingBuffers.ToString();
				if (this.device is GL1020FTEDevice)
				{
					GL1020FTEDevice gL1020FTEDevice = this.device as GL1020FTEDevice;
					if (gL1020FTEDevice.CAN1TransceiverType != CANTransceiverType.Unknown)
					{
						this.labelCAN1Value.Text = GUIUtil.MapTransceiverTypeToName(gL1020FTEDevice.CAN1TransceiverType);
					}
					else
					{
						this.labelCAN1Value.Text = string.Format(Resources.UnknownGenericTranceiver, gL1020FTEDevice.GetGenericTransceiverTypeName(1));
					}
					if (gL1020FTEDevice.CAN2TransceiverType != CANTransceiverType.Unknown)
					{
						this.labelCAN2Value.Text = GUIUtil.MapTransceiverTypeToName(gL1020FTEDevice.CAN2TransceiverType);
					}
					else
					{
						this.labelCAN2Value.Text = string.Format(Resources.UnknownGenericTranceiver, gL1020FTEDevice.GetGenericTransceiverTypeName(2));
					}
				}
			}
			if (!this.IsLoggerConnectedDisplayMode && !string.IsNullOrEmpty(this.device.HardwareKey))
			{
				if (this.device.HardwareKey.Length > 3)
				{
					this.groupBoxDevice.Text = this.device.HardwareKey;
					return;
				}
				this.groupBoxDevice.Text = FileSystemServices.GetCardReaderDisplayName(this.device.HardwareKey);
			}
		}

		private void packAndGo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
		}

		private void PopulatePackAndGoLinkList()
		{
			int num = 60;
			string text = "-";
			string[] projectZIPFilePath = this.device.GetProjectZIPFilePath();
			if (projectZIPFilePath != null && projectZIPFilePath.Count<string>() > 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				string fileName = Path.GetFileName(projectZIPFilePath[0]);
				if (!string.IsNullOrEmpty(fileName))
				{
					stringBuilder.Append(fileName);
					text = stringBuilder.ToString();
					if (text.Length > num)
					{
						text = text.Substring(0, num) + "...";
					}
				}
			}
			this.mLabelPackAndGo.Text = text;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(DeviceInformationGL1020FTE));
			this.groupBoxDevice = new GroupBox();
			this.tableLayoutPanel1 = new TableLayoutPanel();
			this.labelDeviceType = new Label();
			this.labelSerialNumber = new Label();
			this.labelFirmwareVersion = new Label();
			this.labelCapacity = new Label();
			this.labelFreeSpace = new Label();
			this.labelTriggeredFiles = new Label();
			this.labelRecordingFiles = new Label();
			this.labelTypeValue = new Label();
			this.labelSerialNumberValue = new Label();
			this.labelFirmwareVersionValue = new Label();
			this.labelCapacityValue = new Label();
			this.labelFreeSpaceValue = new Label();
			this.labelTriggeredFilesValue = new Label();
			this.labelRecordingFilesValue = new Label();
			this.labelInstalledLicenses = new Label();
			this.labelInstalledLicensesValue = new Label();
			this.labelConfiguration = new Label();
			this.labelCAN1 = new Label();
			this.labelCAN2 = new Label();
			this.labelCAN1Value = new Label();
			this.labelCAN2Value = new Label();
			this.labelAnalogInputs = new Label();
			this.labelAnalogInputsValue = new Label();
			this.labelVehicleName = new Label();
			this.labelVehicleNameValue = new Label();
			this.labelConfigFile = new Label();
			this.labelConfigFileValue = new Label();
			this.labelPackAndGo = new Label();
			this.mLabelPackAndGo = new Label();
			this.labelAnalysisPackage = new Label();
			this.labelAnalysisPackageValue = new Label();
			this.labelNoLoggerDeviceConnected = new Label();
			this.labelLoggerConnectedButInaccessible = new Label();
			this.richTextBoxDeviceConnectedButCardEmpty = new RichTextBox();
			this.groupBoxDevice.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			base.SuspendLayout();
			this.groupBoxDevice.Controls.Add(this.tableLayoutPanel1);
			componentResourceManager.ApplyResources(this.groupBoxDevice, "groupBoxDevice");
			this.groupBoxDevice.Name = "groupBoxDevice";
			this.groupBoxDevice.TabStop = false;
			componentResourceManager.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.labelDeviceType, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.labelSerialNumber, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.labelFirmwareVersion, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this.labelCapacity, 0, 6);
			this.tableLayoutPanel1.Controls.Add(this.labelFreeSpace, 0, 7);
			this.tableLayoutPanel1.Controls.Add(this.labelTriggeredFiles, 0, 8);
			this.tableLayoutPanel1.Controls.Add(this.labelRecordingFiles, 0, 9);
			this.tableLayoutPanel1.Controls.Add(this.labelTypeValue, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.labelSerialNumberValue, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.labelFirmwareVersionValue, 1, 4);
			this.tableLayoutPanel1.Controls.Add(this.labelCapacityValue, 1, 6);
			this.tableLayoutPanel1.Controls.Add(this.labelFreeSpaceValue, 1, 7);
			this.tableLayoutPanel1.Controls.Add(this.labelTriggeredFilesValue, 1, 8);
			this.tableLayoutPanel1.Controls.Add(this.labelRecordingFilesValue, 1, 9);
			this.tableLayoutPanel1.Controls.Add(this.labelInstalledLicenses, 0, 5);
			this.tableLayoutPanel1.Controls.Add(this.labelInstalledLicensesValue, 1, 5);
			this.tableLayoutPanel1.Controls.Add(this.labelConfiguration, 0, 13);
			this.tableLayoutPanel1.Controls.Add(this.labelCAN1, 0, 14);
			this.tableLayoutPanel1.Controls.Add(this.labelCAN2, 0, 15);
			this.tableLayoutPanel1.Controls.Add(this.labelCAN1Value, 1, 14);
			this.tableLayoutPanel1.Controls.Add(this.labelCAN2Value, 1, 15);
			this.tableLayoutPanel1.Controls.Add(this.labelAnalogInputs, 0, 16);
			this.tableLayoutPanel1.Controls.Add(this.labelAnalogInputsValue, 1, 16);
			this.tableLayoutPanel1.Controls.Add(this.labelVehicleName, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.labelVehicleNameValue, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.labelConfigFile, 0, 10);
			this.tableLayoutPanel1.Controls.Add(this.labelConfigFileValue, 1, 10);
			this.tableLayoutPanel1.Controls.Add(this.labelPackAndGo, 0, 11);
			this.tableLayoutPanel1.Controls.Add(this.mLabelPackAndGo, 1, 11);
			this.tableLayoutPanel1.Controls.Add(this.labelAnalysisPackage, 0, 12);
			this.tableLayoutPanel1.Controls.Add(this.labelAnalysisPackageValue, 1, 12);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			componentResourceManager.ApplyResources(this.labelDeviceType, "labelDeviceType");
			this.labelDeviceType.Name = "labelDeviceType";
			componentResourceManager.ApplyResources(this.labelSerialNumber, "labelSerialNumber");
			this.labelSerialNumber.Name = "labelSerialNumber";
			componentResourceManager.ApplyResources(this.labelFirmwareVersion, "labelFirmwareVersion");
			this.labelFirmwareVersion.Name = "labelFirmwareVersion";
			componentResourceManager.ApplyResources(this.labelCapacity, "labelCapacity");
			this.labelCapacity.Name = "labelCapacity";
			componentResourceManager.ApplyResources(this.labelFreeSpace, "labelFreeSpace");
			this.labelFreeSpace.Name = "labelFreeSpace";
			componentResourceManager.ApplyResources(this.labelTriggeredFiles, "labelTriggeredFiles");
			this.labelTriggeredFiles.Name = "labelTriggeredFiles";
			componentResourceManager.ApplyResources(this.labelRecordingFiles, "labelRecordingFiles");
			this.labelRecordingFiles.Name = "labelRecordingFiles";
			componentResourceManager.ApplyResources(this.labelTypeValue, "labelTypeValue");
			this.labelTypeValue.Name = "labelTypeValue";
			componentResourceManager.ApplyResources(this.labelSerialNumberValue, "labelSerialNumberValue");
			this.labelSerialNumberValue.Name = "labelSerialNumberValue";
			componentResourceManager.ApplyResources(this.labelFirmwareVersionValue, "labelFirmwareVersionValue");
			this.labelFirmwareVersionValue.Name = "labelFirmwareVersionValue";
			componentResourceManager.ApplyResources(this.labelCapacityValue, "labelCapacityValue");
			this.labelCapacityValue.Name = "labelCapacityValue";
			componentResourceManager.ApplyResources(this.labelFreeSpaceValue, "labelFreeSpaceValue");
			this.labelFreeSpaceValue.Name = "labelFreeSpaceValue";
			componentResourceManager.ApplyResources(this.labelTriggeredFilesValue, "labelTriggeredFilesValue");
			this.labelTriggeredFilesValue.Name = "labelTriggeredFilesValue";
			componentResourceManager.ApplyResources(this.labelRecordingFilesValue, "labelRecordingFilesValue");
			this.labelRecordingFilesValue.Name = "labelRecordingFilesValue";
			componentResourceManager.ApplyResources(this.labelInstalledLicenses, "labelInstalledLicenses");
			this.labelInstalledLicenses.Name = "labelInstalledLicenses";
			componentResourceManager.ApplyResources(this.labelInstalledLicensesValue, "labelInstalledLicensesValue");
			this.labelInstalledLicensesValue.Name = "labelInstalledLicensesValue";
			componentResourceManager.ApplyResources(this.labelConfiguration, "labelConfiguration");
			this.labelConfiguration.Name = "labelConfiguration";
			componentResourceManager.ApplyResources(this.labelCAN1, "labelCAN1");
			this.labelCAN1.Name = "labelCAN1";
			componentResourceManager.ApplyResources(this.labelCAN2, "labelCAN2");
			this.labelCAN2.Name = "labelCAN2";
			componentResourceManager.ApplyResources(this.labelCAN1Value, "labelCAN1Value");
			this.labelCAN1Value.Name = "labelCAN1Value";
			componentResourceManager.ApplyResources(this.labelCAN2Value, "labelCAN2Value");
			this.labelCAN2Value.Name = "labelCAN2Value";
			componentResourceManager.ApplyResources(this.labelAnalogInputs, "labelAnalogInputs");
			this.labelAnalogInputs.Name = "labelAnalogInputs";
			componentResourceManager.ApplyResources(this.labelAnalogInputsValue, "labelAnalogInputsValue");
			this.labelAnalogInputsValue.Name = "labelAnalogInputsValue";
			componentResourceManager.ApplyResources(this.labelVehicleName, "labelVehicleName");
			this.labelVehicleName.Name = "labelVehicleName";
			componentResourceManager.ApplyResources(this.labelVehicleNameValue, "labelVehicleNameValue");
			this.labelVehicleNameValue.Name = "labelVehicleNameValue";
			componentResourceManager.ApplyResources(this.labelConfigFile, "labelConfigFile");
			this.labelConfigFile.Name = "labelConfigFile";
			componentResourceManager.ApplyResources(this.labelConfigFileValue, "labelConfigFileValue");
			this.labelConfigFileValue.Name = "labelConfigFileValue";
			componentResourceManager.ApplyResources(this.labelPackAndGo, "labelPackAndGo");
			this.labelPackAndGo.Name = "labelPackAndGo";
			componentResourceManager.ApplyResources(this.mLabelPackAndGo, "mLabelPackAndGo");
			this.mLabelPackAndGo.Name = "mLabelPackAndGo";
			componentResourceManager.ApplyResources(this.labelAnalysisPackage, "labelAnalysisPackage");
			this.labelAnalysisPackage.Name = "labelAnalysisPackage";
			componentResourceManager.ApplyResources(this.labelAnalysisPackageValue, "labelAnalysisPackageValue");
			this.labelAnalysisPackageValue.Name = "labelAnalysisPackageValue";
			componentResourceManager.ApplyResources(this.labelNoLoggerDeviceConnected, "labelNoLoggerDeviceConnected");
			this.labelNoLoggerDeviceConnected.ForeColor = Color.Red;
			this.labelNoLoggerDeviceConnected.Name = "labelNoLoggerDeviceConnected";
			componentResourceManager.ApplyResources(this.labelLoggerConnectedButInaccessible, "labelLoggerConnectedButInaccessible");
			this.labelLoggerConnectedButInaccessible.ForeColor = Color.Red;
			this.labelLoggerConnectedButInaccessible.Name = "labelLoggerConnectedButInaccessible";
			this.richTextBoxDeviceConnectedButCardEmpty.BorderStyle = BorderStyle.None;
			componentResourceManager.ApplyResources(this.richTextBoxDeviceConnectedButCardEmpty, "richTextBoxDeviceConnectedButCardEmpty");
			this.richTextBoxDeviceConnectedButCardEmpty.Name = "richTextBoxDeviceConnectedButCardEmpty";
			this.richTextBoxDeviceConnectedButCardEmpty.ReadOnly = true;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.groupBoxDevice);
			base.Controls.Add(this.labelNoLoggerDeviceConnected);
			base.Controls.Add(this.labelLoggerConnectedButInaccessible);
			base.Controls.Add(this.richTextBoxDeviceConnectedButCardEmpty);
			base.Name = "DeviceInformationGL1020FTE";
			this.groupBoxDevice.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
