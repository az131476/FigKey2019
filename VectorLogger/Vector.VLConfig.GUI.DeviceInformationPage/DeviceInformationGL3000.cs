using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GeneralUtil.FileSystem;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.HardwareAccess;
using Vector.VLConfig.HardwareAccess.HardwareAbstraction;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.DeviceInformationPage
{
	internal class DeviceInformationGL3000 : UserControl
	{
		private ILoggerDevice mDevice;

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

		private Label labelLIN1and2;

		private Label labelAnalogInputs;

		private Label labelDigitalInOut;

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

		private Label labelLIN1and2Value;

		private Label labelAnalogInputsValue;

		private Label labelDigitalInputsValue;

		private Label labelConfigFileValue;

		private Label labelInstalledLicensesValue;

		private Label labelNoLoggerDeviceConnected;

		private Label labelLoggerConnectedButInaccessible;

		private RichTextBox richTextBoxDeviceConnectedButCardEmpty;

		private Label labelCAN3;

		private Label labelCAN4;

		private Label labelCAN5to8;

		private Label labelCAN3Value;

		private Label labelCAN4Value;

		private Label labelCAN5to8Value;

		private Label labelCAN9;

		private Label labelCAN9Value;

		private Label labelWLAN;

		private Label labelWLANValue;

		private Label labelNaviFiles;

		private Label labelNaviFilesValue;

		private Label labelConfigFile;

		private Label labelPackAndGo;

		private Label labelVehicleName;

		private Label labelVehicleNameValue;

		private LinkLabel mLinkLabelPackAndGo;

		private Label labelAnalysisPackage;

		private LinkLabel linkLabelAnalysisPackageValue;

		public bool IsLoggerConnectedDisplayMode
		{
			get;
			set;
		}

		public DeviceInformationGL3000()
		{
			this.InitializeComponent();
			this.mDevice = null;
			this.richTextBoxDeviceConnectedButCardEmpty.Rtf = Resources.DeviceGL3000ConnectedButCardEmpty;
			this.IsLoggerConnectedDisplayMode = true;
		}

		public void Init(ILoggerDevice currDevice)
		{
			this.mDevice = currDevice;
			if (this.mDevice == null || this.mDevice.LoggerType != LoggerType.GL3000)
			{
				this.groupBoxDevice.Visible = false;
				this.labelLoggerConnectedButInaccessible.Visible = false;
				this.richTextBoxDeviceConnectedButCardEmpty.Visible = false;
				this.labelNoLoggerDeviceConnected.Visible = true;
				return;
			}
			if (!this.mDevice.IsMemoryCardReady)
			{
				this.groupBoxDevice.Visible = false;
				this.labelLoggerConnectedButInaccessible.Visible = true;
				this.richTextBoxDeviceConnectedButCardEmpty.Visible = false;
				this.labelNoLoggerDeviceConnected.Visible = false;
				return;
			}
			if (!this.mDevice.HasLoggerInfo)
			{
				this.groupBoxDevice.Visible = true;
				this.labelLoggerConnectedButInaccessible.Visible = false;
				this.richTextBoxDeviceConnectedButCardEmpty.Visible = this.IsLoggerConnectedDisplayMode;
				this.labelTypeValue.Text = GUIUtil.GetGL3000LoggerSubtypeNameFromSerialNr("", "");
				this.labelSerialNumberValue.Text = Resources.Unknown;
				this.labelVehicleNameValue.Text = Resources.Unknown;
				if (!string.IsNullOrEmpty(this.mDevice.Name))
				{
					this.labelConfigFileValue.Text = string.Format("{0} ({1})", this.mDevice.Name, this.mDevice.CompileDateTime.ToString());
				}
				else
				{
					this.labelConfigFileValue.Text = "-";
				}
				this.PopulatePackAndGoLinkList();
				this.DisplayAnalysisPackageLink();
				this.labelInstalledLicensesValue.Text = Resources.Unknown;
				this.labelFirmwareVersionValue.Text = Resources.Unknown;
				if (this.mDevice.IsLocatedAtNetwork)
				{
					this.labelFreeSpaceValue.Text = Resources.Unknown;
					this.labelCapacityValue.Text = Resources.Unknown;
				}
				else
				{
					if (!this.IsLoggerConnectedDisplayMode && !string.IsNullOrEmpty(this.mDevice.HardwareKey) && this.mDevice.HardwareKey.Length > 3)
					{
						this.labelFreeSpaceValue.Text = GUIUtil.GetSizeStringMBForBytes(this.mDevice.LogFileStorage.FreeSpace);
					}
					else
					{
						this.labelFreeSpaceValue.Text = string.Format("{0} {1}", GUIUtil.GetSizeStringMBForBytes(this.mDevice.LogFileStorage.FreeSpace), GUIUtil.GetPercentageStringForFreeSpace(this.mDevice.LogFileStorage.TotalSpace, this.mDevice.LogFileStorage.FreeSpace));
					}
					this.labelCapacityValue.Text = GUIUtil.GetSizeStringMBForBytes(this.mDevice.LogFileStorage.TotalSpace);
				}
				this.DisplayNumberOfFiles();
				if (this.mDevice is GL3000Device)
				{
					GL3000Device gL3000Device = this.mDevice as GL3000Device;
					this.labelCAN1Value.Text = Resources.Unknown;
					this.labelCAN2Value.Text = Resources.Unknown;
					this.labelCAN3Value.Text = Resources.Unknown;
					this.labelCAN4Value.Text = Resources.Unknown;
					this.labelCAN5to8Value.Text = this.GetTransceiverText(gL3000Device.CAN5TransceiverType, 5, gL3000Device);
					this.labelCAN9Value.Text = this.GetTransceiverText(gL3000Device.CAN9TransceiverType, 9, gL3000Device);
				}
				this.labelAnalogInputsValue.Text = "6 + " + Resources.Unknown;
				this.labelWLANValue.Text = Resources.Unknown;
			}
			else
			{
				this.groupBoxDevice.Visible = true;
				this.labelLoggerConnectedButInaccessible.Visible = false;
				this.richTextBoxDeviceConnectedButCardEmpty.Visible = false;
				this.labelNoLoggerDeviceConnected.Visible = false;
				this.labelTypeValue.Text = GUIUtil.GetGL3000LoggerSubtypeNameFromSerialNr(this.mDevice.SerialNumber, this.mDevice.LogDiskInfo);
				this.labelSerialNumberValue.Text = this.mDevice.SerialNumber;
				this.labelVehicleNameValue.Text = this.mDevice.VehicleName;
				this.labelConfigFileValue.Text = this.mDevice.Name;
				if (!string.IsNullOrEmpty(this.mDevice.Name))
				{
					this.labelConfigFileValue.Text = string.Format("{0} ({1})", this.mDevice.Name, this.mDevice.CompileDateTime.ToString());
				}
				else
				{
					this.labelConfigFileValue.Text = "-";
				}
				this.PopulatePackAndGoLinkList();
				this.DisplayAnalysisPackageLink();
				if (!string.IsNullOrEmpty(this.mDevice.InstalledLicenses))
				{
					this.labelInstalledLicensesValue.Text = this.mDevice.InstalledLicenses;
				}
				else
				{
					this.labelInstalledLicensesValue.Text = Resources.NoInstalledLicense;
				}
				this.labelFirmwareVersionValue.Text = this.mDevice.FirmwareVersion;
				if (this.mDevice.IsLocatedAtNetwork)
				{
					this.labelFreeSpaceValue.Text = Resources.Unknown;
					this.labelCapacityValue.Text = Resources.Unknown;
				}
				else
				{
					if (!this.IsLoggerConnectedDisplayMode && !string.IsNullOrEmpty(this.mDevice.HardwareKey) && this.mDevice.HardwareKey.Length > 3)
					{
						this.labelFreeSpaceValue.Text = GUIUtil.GetSizeStringMBForBytes(this.mDevice.LogFileStorage.FreeSpace);
					}
					else
					{
						this.labelFreeSpaceValue.Text = string.Format("{0} {1}", GUIUtil.GetSizeStringMBForBytes(this.mDevice.LogFileStorage.FreeSpace), GUIUtil.GetPercentageStringForFreeSpace(this.mDevice.LogFileStorage.TotalSpace, this.mDevice.LogFileStorage.FreeSpace));
					}
					this.labelCapacityValue.Text = GUIUtil.GetSizeStringMBForBytes(this.mDevice.LogFileStorage.TotalSpace);
				}
				this.DisplayNumberOfFiles();
				if (this.mDevice is GL3000Device)
				{
					GL3000Device gL3000Device2 = this.mDevice as GL3000Device;
					this.labelCAN1Value.Text = this.GetTransceiverText(gL3000Device2.CAN1TransceiverType, 1, gL3000Device2);
					this.labelCAN2Value.Text = this.GetTransceiverText(gL3000Device2.CAN2TransceiverType, 2, gL3000Device2);
					this.labelCAN3Value.Text = this.GetTransceiverText(gL3000Device2.CAN3TransceiverType, 3, gL3000Device2);
					this.labelCAN4Value.Text = this.GetTransceiverText(gL3000Device2.CAN4TransceiverType, 4, gL3000Device2);
					this.labelCAN5to8Value.Text = this.GetTransceiverText(gL3000Device2.CAN5TransceiverType, 5, gL3000Device2);
					this.labelCAN9Value.Text = this.GetTransceiverText(gL3000Device2.CAN9TransceiverType, 9, gL3000Device2);
					if (gL3000Device2.IsAnalogExtensionBoardInstalled)
					{
						this.labelAnalogInputsValue.Text = "6 + 8 (" + Resources.OnAnalogInputExtBoard + ")";
					}
					else
					{
						this.labelAnalogInputsValue.Text = "6";
					}
					if (gL3000Device2.IsWLANExtensionBoardInstalled)
					{
						this.labelWLANValue.Text = Resources.Yes + " (" + gL3000Device2.InstalledWLANBoardModelName + ")";
					}
					else
					{
						this.labelWLANValue.Text = Resources.No;
					}
				}
			}
			string gL3PlusLINTransceiverTypeFromSerialNr = GUIUtil.GetGL3PlusLINTransceiverTypeFromSerialNr(this.mDevice.SerialNumber);
			this.labelLIN1and2Value.Text = gL3PlusLINTransceiverTypeFromSerialNr;
			if (!this.IsLoggerConnectedDisplayMode && !string.IsNullOrEmpty(this.mDevice.HardwareKey))
			{
				if (this.mDevice.HardwareKey.Length > 3)
				{
					this.groupBoxDevice.Text = this.mDevice.HardwareKey;
					return;
				}
				this.groupBoxDevice.Text = FileSystemServices.GetCardReaderDisplayName(this.mDevice.HardwareKey);
			}
		}

		private string GetTransceiverText(CANTransceiverType type, int channelNumber, GL3000Device device)
		{
			if (type != CANTransceiverType.Unknown)
			{
				return GUIUtil.MapTransceiverTypeToName(type);
			}
			return string.Format(Resources.UnknownGenericTranceiver, device.GetGenericTransceiverTypeName(channelNumber));
		}

		private void DisplayNumberOfFiles()
		{
			string str = "";
			if (this.mDevice.LogFileStorage.NumberOfCompLogFilesOnMemory(1u) > 0u || this.mDevice.LogFileStorage.NumberOfCompLogFilesOnMemory(2u) > 0u)
			{
				str = " " + Resources.CompressedPostfix;
			}
			string text = string.Format(Resources.NumOfFilesOnMemory, this.mDevice.LogFileStorage.NumberOfLogFilesOnMemory(1u) + this.mDevice.LogFileStorage.NumberOfCompLogFilesOnMemory(1u), 1) + ", " + string.Format(Resources.NumOfFilesOnMemory, this.mDevice.LogFileStorage.NumberOfLogFilesOnMemory(2u) + this.mDevice.LogFileStorage.NumberOfCompLogFilesOnMemory(2u), 2) + str;
			if (!this.mDevice.HasSnapshotFolderContainingLogData)
			{
				this.labelTriggeredFilesValue.Text = text;
			}
			else
			{
				this.labelTriggeredFilesValue.Text = text + "; " + Resources.SnapshotDataPresent;
			}
			StringBuilder stringBuilder = new StringBuilder();
			if (this.mDevice.LogFileStorage.NumberOfWavFiles > 0u)
			{
				stringBuilder.AppendFormat("{0} {1}", this.mDevice.LogFileStorage.NumberOfWavFiles, Resources.AudioFiles);
			}
			if (this.mDevice.LogFileStorage.NumberOfJpegFiles > 0u)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append("; ");
				}
				stringBuilder.AppendFormat("{0} {1}", this.mDevice.LogFileStorage.NumberOfJpegFiles, Resources.ImageFiles);
			}
			if (this.mDevice.LogFileStorage.NumberOfZipArchives > 0u)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append("; ");
				}
				stringBuilder.AppendFormat("{0} {1}", this.mDevice.LogFileStorage.NumberOfZipArchives, Resources.ImageArchives);
			}
			if (this.mDevice.LogFileStorage.NumberOfClassificFiles > 0u)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append("; ");
				}
				stringBuilder.AppendFormat("{0} {1}", this.mDevice.LogFileStorage.NumberOfClassificFiles, Resources.ClassificFiles);
			}
			if (this.mDevice.LogFileStorage.NumberOfDriveRecorderFiles > 0u)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append("; ");
				}
				stringBuilder.AppendFormat("{0} {1}", this.mDevice.LogFileStorage.NumberOfDriveRecorderFiles, Resources.DriveRecorderFiles);
			}
			if (stringBuilder.Length > 0)
			{
				this.labelRecordingFilesValue.Text = stringBuilder.ToString();
			}
			else
			{
				this.labelRecordingFilesValue.Text = Resources.None;
			}
			if (this.mDevice.HasIndexFile)
			{
				this.labelNaviFilesValue.Text = Resources.Yes;
				return;
			}
			this.labelNaviFilesValue.Text = Resources.No;
		}

		private void packAndGo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			string text = e.Link.LinkData.ToString();
			if (!FileProxy.Exists(text))
			{
				InformMessageBox.Error(string.Format(Resources.ErrorFileWithNameNotFound, Path.GetFileName(text)));
				return;
			}
			GUIUtil.ExtractIfRequiredAndLaunchFile(text);
		}

		private void linkLabelAnalysisPackageValue_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			string analysisPackagePath = this.mDevice.GetAnalysisPackagePath();
			if (string.IsNullOrEmpty(analysisPackagePath))
			{
				InformMessageBox.Error(string.Format(Resources.ErrorFileWithNameNotFound, this.linkLabelAnalysisPackageValue.Text));
				return;
			}
			GUIUtil.ExtractIfRequiredAndLaunchFile(analysisPackagePath);
		}

		private void PopulatePackAndGoLinkList()
		{
			int num = 60;
			string text = "-";
			this.mLinkLabelPackAndGo.Links.Clear();
			string[] projectZIPFilePath = this.mDevice.GetProjectZIPFilePath();
			if (projectZIPFilePath != null && projectZIPFilePath.Count<string>() > 0)
			{
				StringBuilder stringBuilder = new StringBuilder();
				string fileName = Path.GetFileName(projectZIPFilePath[0]);
				if (!string.IsNullOrEmpty(fileName))
				{
					int num2 = 0;
					stringBuilder.Append(fileName);
					int length = fileName.Length;
					if (num2 + fileName.Length > num)
					{
						length = num - num2;
					}
					this.mLinkLabelPackAndGo.Links.Add(num2, length, projectZIPFilePath[0]);
					num2 += fileName.Length + 2;
					if (projectZIPFilePath.Count<string>() > 1)
					{
						for (int i = 1; i < projectZIPFilePath.Count<string>(); i++)
						{
							fileName = Path.GetFileName(projectZIPFilePath[i]);
							if (!string.IsNullOrEmpty(fileName))
							{
								stringBuilder.Append(", " + fileName);
								if (num2 < num)
								{
									length = fileName.Length;
									if (num2 + fileName.Length > num)
									{
										length = num - num2;
									}
									this.mLinkLabelPackAndGo.Links.Add(num2, length, projectZIPFilePath[i]);
								}
								num2 += fileName.Length + 2;
							}
						}
					}
					text = stringBuilder.ToString();
					if (text.Length > num)
					{
						text = text.Substring(0, num) + "...";
					}
				}
			}
			this.mLinkLabelPackAndGo.Text = text;
		}

		private void DisplayAnalysisPackageLink()
		{
			this.linkLabelAnalysisPackageValue.Text = "-";
			this.linkLabelAnalysisPackageValue.Links.Clear();
			string analysisPackagePath = this.mDevice.GetAnalysisPackagePath();
			if (!string.IsNullOrEmpty(analysisPackagePath))
			{
				this.linkLabelAnalysisPackageValue.Text = Path.GetFileName(analysisPackagePath);
				this.linkLabelAnalysisPackageValue.LinkArea = new LinkArea(0, analysisPackagePath.Length);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(DeviceInformationGL3000));
			this.groupBoxDevice = new GroupBox();
			this.tableLayoutPanel1 = new TableLayoutPanel();
			this.labelDeviceType = new Label();
			this.labelSerialNumber = new Label();
			this.labelTypeValue = new Label();
			this.labelSerialNumberValue = new Label();
			this.labelWLAN = new Label();
			this.labelDigitalInOut = new Label();
			this.labelAnalogInputs = new Label();
			this.labelLIN1and2 = new Label();
			this.labelCAN9 = new Label();
			this.labelCAN5to8 = new Label();
			this.labelCAN4 = new Label();
			this.labelCAN3 = new Label();
			this.labelCAN2 = new Label();
			this.labelCAN1 = new Label();
			this.labelConfiguration = new Label();
			this.labelPackAndGo = new Label();
			this.labelConfigFile = new Label();
			this.labelNaviFiles = new Label();
			this.labelRecordingFiles = new Label();
			this.labelTriggeredFiles = new Label();
			this.labelFreeSpace = new Label();
			this.labelCapacity = new Label();
			this.labelInstalledLicenses = new Label();
			this.labelFirmwareVersion = new Label();
			this.labelWLANValue = new Label();
			this.labelDigitalInputsValue = new Label();
			this.labelAnalogInputsValue = new Label();
			this.labelLIN1and2Value = new Label();
			this.labelCAN9Value = new Label();
			this.labelCAN5to8Value = new Label();
			this.labelCAN4Value = new Label();
			this.labelCAN3Value = new Label();
			this.labelCAN2Value = new Label();
			this.labelCAN1Value = new Label();
			this.labelConfigFileValue = new Label();
			this.labelNaviFilesValue = new Label();
			this.labelRecordingFilesValue = new Label();
			this.labelTriggeredFilesValue = new Label();
			this.labelFreeSpaceValue = new Label();
			this.labelCapacityValue = new Label();
			this.labelInstalledLicensesValue = new Label();
			this.labelFirmwareVersionValue = new Label();
			this.labelVehicleName = new Label();
			this.labelVehicleNameValue = new Label();
			this.mLinkLabelPackAndGo = new LinkLabel();
			this.labelAnalysisPackage = new Label();
			this.labelNoLoggerDeviceConnected = new Label();
			this.labelLoggerConnectedButInaccessible = new Label();
			this.richTextBoxDeviceConnectedButCardEmpty = new RichTextBox();
			this.linkLabelAnalysisPackageValue = new LinkLabel();
			this.groupBoxDevice.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			base.SuspendLayout();
			this.groupBoxDevice.Controls.Add(this.tableLayoutPanel1);
			componentResourceManager.ApplyResources(this.groupBoxDevice, "groupBoxDevice");
			this.groupBoxDevice.Name = "groupBoxDevice";
			this.groupBoxDevice.TabStop = false;
			this.tableLayoutPanel1.BackColor = Color.Transparent;
			componentResourceManager.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.labelDeviceType, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.labelSerialNumber, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.labelTypeValue, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.labelSerialNumberValue, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.labelWLAN, 0, 24);
			this.tableLayoutPanel1.Controls.Add(this.labelDigitalInOut, 0, 23);
			this.tableLayoutPanel1.Controls.Add(this.labelAnalogInputs, 0, 22);
			this.tableLayoutPanel1.Controls.Add(this.labelLIN1and2, 0, 21);
			this.tableLayoutPanel1.Controls.Add(this.labelCAN9, 0, 20);
			this.tableLayoutPanel1.Controls.Add(this.labelCAN5to8, 0, 19);
			this.tableLayoutPanel1.Controls.Add(this.labelCAN4, 0, 18);
			this.tableLayoutPanel1.Controls.Add(this.labelCAN3, 0, 17);
			this.tableLayoutPanel1.Controls.Add(this.labelCAN2, 0, 16);
			this.tableLayoutPanel1.Controls.Add(this.labelCAN1, 0, 15);
			this.tableLayoutPanel1.Controls.Add(this.labelConfiguration, 0, 14);
			this.tableLayoutPanel1.Controls.Add(this.labelPackAndGo, 0, 12);
			this.tableLayoutPanel1.Controls.Add(this.labelConfigFile, 0, 11);
			this.tableLayoutPanel1.Controls.Add(this.labelNaviFiles, 0, 10);
			this.tableLayoutPanel1.Controls.Add(this.labelRecordingFiles, 0, 9);
			this.tableLayoutPanel1.Controls.Add(this.labelTriggeredFiles, 0, 8);
			this.tableLayoutPanel1.Controls.Add(this.labelFreeSpace, 0, 7);
			this.tableLayoutPanel1.Controls.Add(this.labelCapacity, 0, 6);
			this.tableLayoutPanel1.Controls.Add(this.labelInstalledLicenses, 0, 5);
			this.tableLayoutPanel1.Controls.Add(this.labelFirmwareVersion, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this.labelWLANValue, 1, 24);
			this.tableLayoutPanel1.Controls.Add(this.labelDigitalInputsValue, 1, 23);
			this.tableLayoutPanel1.Controls.Add(this.labelAnalogInputsValue, 1, 22);
			this.tableLayoutPanel1.Controls.Add(this.labelLIN1and2Value, 1, 21);
			this.tableLayoutPanel1.Controls.Add(this.labelCAN9Value, 1, 20);
			this.tableLayoutPanel1.Controls.Add(this.labelCAN5to8Value, 1, 19);
			this.tableLayoutPanel1.Controls.Add(this.labelCAN4Value, 1, 18);
			this.tableLayoutPanel1.Controls.Add(this.labelCAN3Value, 1, 17);
			this.tableLayoutPanel1.Controls.Add(this.labelCAN2Value, 1, 16);
			this.tableLayoutPanel1.Controls.Add(this.labelCAN1Value, 1, 15);
			this.tableLayoutPanel1.Controls.Add(this.labelConfigFileValue, 1, 11);
			this.tableLayoutPanel1.Controls.Add(this.labelNaviFilesValue, 1, 10);
			this.tableLayoutPanel1.Controls.Add(this.labelRecordingFilesValue, 1, 9);
			this.tableLayoutPanel1.Controls.Add(this.labelTriggeredFilesValue, 1, 8);
			this.tableLayoutPanel1.Controls.Add(this.labelFreeSpaceValue, 1, 7);
			this.tableLayoutPanel1.Controls.Add(this.labelCapacityValue, 1, 6);
			this.tableLayoutPanel1.Controls.Add(this.labelInstalledLicensesValue, 1, 5);
			this.tableLayoutPanel1.Controls.Add(this.labelFirmwareVersionValue, 1, 4);
			this.tableLayoutPanel1.Controls.Add(this.labelVehicleName, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.labelVehicleNameValue, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.mLinkLabelPackAndGo, 1, 12);
			this.tableLayoutPanel1.Controls.Add(this.labelAnalysisPackage, 0, 13);
			this.tableLayoutPanel1.Controls.Add(this.linkLabelAnalysisPackageValue, 1, 13);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			componentResourceManager.ApplyResources(this.labelDeviceType, "labelDeviceType");
			this.labelDeviceType.Name = "labelDeviceType";
			componentResourceManager.ApplyResources(this.labelSerialNumber, "labelSerialNumber");
			this.labelSerialNumber.Name = "labelSerialNumber";
			componentResourceManager.ApplyResources(this.labelTypeValue, "labelTypeValue");
			this.labelTypeValue.Name = "labelTypeValue";
			componentResourceManager.ApplyResources(this.labelSerialNumberValue, "labelSerialNumberValue");
			this.labelSerialNumberValue.Name = "labelSerialNumberValue";
			componentResourceManager.ApplyResources(this.labelWLAN, "labelWLAN");
			this.labelWLAN.Name = "labelWLAN";
			componentResourceManager.ApplyResources(this.labelDigitalInOut, "labelDigitalInOut");
			this.labelDigitalInOut.Name = "labelDigitalInOut";
			componentResourceManager.ApplyResources(this.labelAnalogInputs, "labelAnalogInputs");
			this.labelAnalogInputs.Name = "labelAnalogInputs";
			componentResourceManager.ApplyResources(this.labelLIN1and2, "labelLIN1and2");
			this.labelLIN1and2.Name = "labelLIN1and2";
			componentResourceManager.ApplyResources(this.labelCAN9, "labelCAN9");
			this.labelCAN9.Name = "labelCAN9";
			componentResourceManager.ApplyResources(this.labelCAN5to8, "labelCAN5to8");
			this.labelCAN5to8.Name = "labelCAN5to8";
			componentResourceManager.ApplyResources(this.labelCAN4, "labelCAN4");
			this.labelCAN4.Name = "labelCAN4";
			componentResourceManager.ApplyResources(this.labelCAN3, "labelCAN3");
			this.labelCAN3.Name = "labelCAN3";
			componentResourceManager.ApplyResources(this.labelCAN2, "labelCAN2");
			this.labelCAN2.Name = "labelCAN2";
			componentResourceManager.ApplyResources(this.labelCAN1, "labelCAN1");
			this.labelCAN1.Name = "labelCAN1";
			componentResourceManager.ApplyResources(this.labelConfiguration, "labelConfiguration");
			this.labelConfiguration.Name = "labelConfiguration";
			componentResourceManager.ApplyResources(this.labelPackAndGo, "labelPackAndGo");
			this.labelPackAndGo.Name = "labelPackAndGo";
			componentResourceManager.ApplyResources(this.labelConfigFile, "labelConfigFile");
			this.labelConfigFile.Name = "labelConfigFile";
			componentResourceManager.ApplyResources(this.labelNaviFiles, "labelNaviFiles");
			this.labelNaviFiles.Name = "labelNaviFiles";
			componentResourceManager.ApplyResources(this.labelRecordingFiles, "labelRecordingFiles");
			this.labelRecordingFiles.Name = "labelRecordingFiles";
			componentResourceManager.ApplyResources(this.labelTriggeredFiles, "labelTriggeredFiles");
			this.labelTriggeredFiles.Name = "labelTriggeredFiles";
			componentResourceManager.ApplyResources(this.labelFreeSpace, "labelFreeSpace");
			this.labelFreeSpace.Name = "labelFreeSpace";
			componentResourceManager.ApplyResources(this.labelCapacity, "labelCapacity");
			this.labelCapacity.Name = "labelCapacity";
			componentResourceManager.ApplyResources(this.labelInstalledLicenses, "labelInstalledLicenses");
			this.labelInstalledLicenses.Name = "labelInstalledLicenses";
			componentResourceManager.ApplyResources(this.labelFirmwareVersion, "labelFirmwareVersion");
			this.labelFirmwareVersion.Name = "labelFirmwareVersion";
			componentResourceManager.ApplyResources(this.labelWLANValue, "labelWLANValue");
			this.labelWLANValue.Name = "labelWLANValue";
			componentResourceManager.ApplyResources(this.labelDigitalInputsValue, "labelDigitalInputsValue");
			this.labelDigitalInputsValue.Name = "labelDigitalInputsValue";
			componentResourceManager.ApplyResources(this.labelAnalogInputsValue, "labelAnalogInputsValue");
			this.labelAnalogInputsValue.Name = "labelAnalogInputsValue";
			componentResourceManager.ApplyResources(this.labelLIN1and2Value, "labelLIN1and2Value");
			this.labelLIN1and2Value.Name = "labelLIN1and2Value";
			componentResourceManager.ApplyResources(this.labelCAN9Value, "labelCAN9Value");
			this.labelCAN9Value.Name = "labelCAN9Value";
			componentResourceManager.ApplyResources(this.labelCAN5to8Value, "labelCAN5to8Value");
			this.labelCAN5to8Value.Name = "labelCAN5to8Value";
			componentResourceManager.ApplyResources(this.labelCAN4Value, "labelCAN4Value");
			this.labelCAN4Value.Name = "labelCAN4Value";
			componentResourceManager.ApplyResources(this.labelCAN3Value, "labelCAN3Value");
			this.labelCAN3Value.Name = "labelCAN3Value";
			componentResourceManager.ApplyResources(this.labelCAN2Value, "labelCAN2Value");
			this.labelCAN2Value.Name = "labelCAN2Value";
			componentResourceManager.ApplyResources(this.labelCAN1Value, "labelCAN1Value");
			this.labelCAN1Value.Name = "labelCAN1Value";
			componentResourceManager.ApplyResources(this.labelConfigFileValue, "labelConfigFileValue");
			this.labelConfigFileValue.Name = "labelConfigFileValue";
			componentResourceManager.ApplyResources(this.labelNaviFilesValue, "labelNaviFilesValue");
			this.labelNaviFilesValue.Name = "labelNaviFilesValue";
			componentResourceManager.ApplyResources(this.labelRecordingFilesValue, "labelRecordingFilesValue");
			this.labelRecordingFilesValue.Name = "labelRecordingFilesValue";
			componentResourceManager.ApplyResources(this.labelTriggeredFilesValue, "labelTriggeredFilesValue");
			this.labelTriggeredFilesValue.Name = "labelTriggeredFilesValue";
			componentResourceManager.ApplyResources(this.labelFreeSpaceValue, "labelFreeSpaceValue");
			this.labelFreeSpaceValue.Name = "labelFreeSpaceValue";
			componentResourceManager.ApplyResources(this.labelCapacityValue, "labelCapacityValue");
			this.labelCapacityValue.Name = "labelCapacityValue";
			componentResourceManager.ApplyResources(this.labelInstalledLicensesValue, "labelInstalledLicensesValue");
			this.labelInstalledLicensesValue.Name = "labelInstalledLicensesValue";
			componentResourceManager.ApplyResources(this.labelFirmwareVersionValue, "labelFirmwareVersionValue");
			this.labelFirmwareVersionValue.Name = "labelFirmwareVersionValue";
			componentResourceManager.ApplyResources(this.labelVehicleName, "labelVehicleName");
			this.labelVehicleName.Name = "labelVehicleName";
			componentResourceManager.ApplyResources(this.labelVehicleNameValue, "labelVehicleNameValue");
			this.labelVehicleNameValue.Name = "labelVehicleNameValue";
			componentResourceManager.ApplyResources(this.mLinkLabelPackAndGo, "mLinkLabelPackAndGo");
			this.mLinkLabelPackAndGo.Name = "mLinkLabelPackAndGo";
			this.mLinkLabelPackAndGo.LinkClicked += new LinkLabelLinkClickedEventHandler(this.packAndGo_LinkClicked);
			componentResourceManager.ApplyResources(this.labelAnalysisPackage, "labelAnalysisPackage");
			this.labelAnalysisPackage.Name = "labelAnalysisPackage";
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
			componentResourceManager.ApplyResources(this.linkLabelAnalysisPackageValue, "linkLabelAnalysisPackageValue");
			this.linkLabelAnalysisPackageValue.Name = "linkLabelAnalysisPackageValue";
			this.linkLabelAnalysisPackageValue.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkLabelAnalysisPackageValue_LinkClicked);
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.groupBoxDevice);
			base.Controls.Add(this.labelNoLoggerDeviceConnected);
			base.Controls.Add(this.labelLoggerConnectedButInaccessible);
			base.Controls.Add(this.richTextBoxDeviceConnectedButCardEmpty);
			base.Name = "DeviceInformationGL3000";
			this.groupBoxDevice.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
