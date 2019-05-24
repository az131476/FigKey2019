using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.GUI.Helpers;
using Vector.VLConfig.HardwareAccess.HardwareAbstraction;
using Vector.VLConfig.HardwareAccess.HardwareAbstraction.VN16XXlog;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;
using Vector.XlApiNet;

namespace Vector.VLConfig.GUI.DeviceInformationPage
{
	internal class DeviceInformationVN16XXlog : UserControl
	{
		private ILoggerDevice mDevice;

		private VN16XXlogDevice mVN16XXlogDevice;

		private IContainer components;

		private GroupBox mGroupBoxDevice;

		private TableLayoutPanel tableLayoutPanel1;

		private Label labelDeviceType;

		private Label labelSerialNumber;

		private Label labelFirmwareVersion;

		private Label labelCapacity;

		private Label labelFreeSpace;

		private Label labelTriggeredFiles;

		private Label labelConfiguration;

		private Label labelChannel1;

		private Label labelChannel2;

		private Label labelAnalogInputs;

		private Label labelDigitalInOut;

		private Label labelConfigFile;

		private Label mLabelTypeValue;

		private Label mLabelSerialNumberValue;

		private Label mLabelFirmwareVersionValue;

		private Label mLabelCapacityValue;

		private Label mLabelFreeSpaceValue;

		private Label labelTriggeredFilesValue;

		private Label mLabelChannel1Value;

		private Label mLabelChannel2Value;

		private Label labelAnalogInputsValue;

		private Label labelDigitalInputsValue;

		private Label mLabelConfigFileValue;

		private Label mLabelNoLoggerDeviceConnected;

		private Label mLabelLoggerConnectedButInaccessible;

		private RichTextBox mRichTextBoxInfo;

		private Label labelChannel3;

		private Label labelChannel4;

		private Label mLabelChannel3Value;

		private Label mLabelChannel4Value;

		private LinkLabel mLinkLabelClusterSizeNotOptimal;

		private PictureBox mPictureBoxInfo;

		private PictureBox mPictureBoxError;

		private Label labelPackAndGo;

		private Label labelDriverVersion;

		private Label mLabelDriverVersionValue;

		private LinkLabel mLinkLabelPackAndGo;

		private Label labelFirmwareVersionInterfaceMode;

		private Label mLabelFirmwareVersioninterfaceModeValue;

		private Label labelLoggerDeviceConnectedWithoutMemCard;

		public event EventHandler FormatMemoryCardClicked;

		public bool IsLoggerConnectedDisplayMode
		{
			get;
			set;
		}

		public DeviceInformationVN16XXlog()
		{
			this.InitializeComponent();
			this.mDevice = null;
			this.mLinkLabelClusterSizeNotOptimal.Visible = false;
			this.mPictureBoxInfo.Image = SystemIcons.Information.ToBitmap();
			this.mPictureBoxError.Image = SystemIcons.Error.ToBitmap();
			this.IsLoggerConnectedDisplayMode = true;
			this.UpdateAdditionalInfo(true);
		}

		private void Raise_FormatMemoryCardClicked()
		{
			if (this.FormatMemoryCardClicked != null)
			{
				this.FormatMemoryCardClicked(this, EventArgs.Empty);
			}
		}

		public void Init(ILoggerDevice currDevice, bool isDeviceWithoutMemoryCardConnected)
		{
			this.UpdateAdditionalInfo(true);
			this.mDevice = currDevice;
			this.mVN16XXlogDevice = (this.mDevice as VN16XXlogDevice);
			if (this.mDevice == null || this.mDevice.LoggerType != LoggerType.VN1630log || this.mVN16XXlogDevice == null)
			{
				this.mGroupBoxDevice.Visible = false;
				this.mLabelLoggerConnectedButInaccessible.Visible = false;
				this.mLabelNoLoggerDeviceConnected.Visible = !isDeviceWithoutMemoryCardConnected;
				this.labelLoggerDeviceConnectedWithoutMemCard.Visible = isDeviceWithoutMemoryCardConnected;
				return;
			}
			if (!VN16XXlogScanner.IsMemoryCardContentCompatibleToLogger(this.mDevice.HardwareKey))
			{
				this.mGroupBoxDevice.Visible = false;
				this.mLabelLoggerConnectedButInaccessible.Visible = true;
				this.mLabelNoLoggerDeviceConnected.Visible = false;
				this.labelLoggerDeviceConnectedWithoutMemCard.Visible = false;
				return;
			}
			this.mGroupBoxDevice.Visible = true;
			this.mLabelLoggerConnectedButInaccessible.Visible = false;
			this.mLabelNoLoggerDeviceConnected.Visible = false;
			this.labelLoggerDeviceConnectedWithoutMemCard.Visible = false;
			this.mLabelTypeValue.Text = ((this.mVN16XXlogDevice != null) ? this.mVN16XXlogDevice.GetHwTypeName() : XlUtils.GetHwName(XlHwType.XL_HWTYPE_VN1630_LOG));
			this.mLabelSerialNumberValue.Text = this.mDevice.SerialNumber;
			this.mLabelDriverVersionValue.Text = ((this.mVN16XXlogDevice != null) ? this.mVN16XXlogDevice.DriverVersion : Resources.Unknown);
			this.mLabelFirmwareVersioninterfaceModeValue.Text = ((this.mVN16XXlogDevice != null) ? this.mVN16XXlogDevice.FirmwareVersionInterfaceMode : Resources.Unknown);
			this.mLabelFirmwareVersionValue.Text = this.mDevice.FirmwareVersion;
			this.mLabelCapacityValue.Text = ((this.mDevice.IsLocatedAtNetwork || !this.mDevice.IsMemoryCardReady) ? Resources.Unknown : GUIUtil.GetSizeStringMBForBytes(this.mDevice.LogFileStorage.TotalSpace));
			this.mLabelFreeSpaceValue.Text = ((this.mDevice.IsLocatedAtNetwork || !this.mDevice.IsMemoryCardReady) ? Resources.Unknown : GUIUtil.GetSizeStringMBForBytes(this.mDevice.LogFileStorage.FreeSpace));
			this.DisplayNumberOfFiles();
			this.mLabelConfigFileValue.Text = ((this.mDevice.IsLocatedAtNetwork || !this.mDevice.IsMemoryCardReady) ? Resources.Unknown : ((!string.IsNullOrEmpty(this.mDevice.Name)) ? string.Format("{0}", this.mDevice.CompileDateTime) : "-"));
			this.PopulatePackAndGoLinkList();
			if (this.mVN16XXlogDevice != null)
			{
				this.mLabelChannel1Value.Text = this.mVN16XXlogDevice.GetTransceiverTypeName(1);
				this.mLabelChannel2Value.Text = this.mVN16XXlogDevice.GetTransceiverTypeName(2);
				this.mLabelChannel3Value.Text = this.mVN16XXlogDevice.GetTransceiverTypeName(3);
				this.mLabelChannel4Value.Text = this.mVN16XXlogDevice.GetTransceiverTypeName(4);
			}
			this.UpdateAdditionalInfo(false);
			if (!this.IsLoggerConnectedDisplayMode && !string.IsNullOrEmpty(this.mDevice.HardwareKey))
			{
				this.mGroupBoxDevice.Text = ((this.mDevice.HardwareKey.Length > 3) ? this.mDevice.HardwareKey : FileSystemServices.GetCardReaderDisplayName(this.mDevice.HardwareKey));
			}
		}

		private void DisplayNumberOfFiles()
		{
			if (!this.mDevice.IsMemoryCardReady)
			{
				this.labelTriggeredFilesValue.Text = Resources.Unknown;
				return;
			}
			string text = string.Format("{0}", this.mDevice.LogFileStorage.NumberOfLogFilesOnMemory(1u) + this.mDevice.LogFileStorage.NumberOfCompLogFilesOnMemory(1u));
			this.labelTriggeredFilesValue.Text = (this.mDevice.HasSnapshotFolderContainingLogData ? (text + "; " + Resources.SnapshotDataPresent) : text);
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
		}

		private string GetPackAndGoInfoText()
		{
			string[] projectZIPFilePath = this.mDevice.GetProjectZIPFilePath();
			if (projectZIPFilePath == null || !projectZIPFilePath.Any<string>())
			{
				return "-";
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(Path.GetFileName(projectZIPFilePath[0]));
			if (projectZIPFilePath.Count<string>() > 1)
			{
				for (int i = 1; i < projectZIPFilePath.Count<string>(); i++)
				{
					stringBuilder.Append(", " + Path.GetFileName(projectZIPFilePath[i]));
				}
			}
			return stringBuilder.ToString();
		}

		private void linkLabelClusterSizeNotOptimal_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			this.Raise_FormatMemoryCardClicked();
		}

		private void packAndGo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			string text = e.Link.LinkData.ToString();
			if (File.Exists(text))
			{
				Process.Start("explorer.exe", text);
				return;
			}
			InformMessageBox.Error(string.Format(Resources.ErrorFileWithNameNotFound, Path.GetFileName(text)));
		}

		private void PopulatePackAndGoLinkList()
		{
			this.mLinkLabelPackAndGo.Text = ((this.mDevice.IsLocatedAtNetwork || !this.mDevice.IsMemoryCardReady) ? Resources.Unknown : "-");
			this.mLinkLabelPackAndGo.Links.Clear();
			string[] projectZIPFilePath = this.mDevice.GetProjectZIPFilePath();
			if (projectZIPFilePath == null || !projectZIPFilePath.Any<string>())
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			string fileName = Path.GetFileName(projectZIPFilePath[0]);
			if (fileName != null)
			{
				int num = 0;
				stringBuilder.Append(fileName);
				int length = fileName.Length;
				if (num + fileName.Length > 60)
				{
					length = 60 - num;
				}
				this.mLinkLabelPackAndGo.Links.Add(num, length, projectZIPFilePath[0]);
				num += fileName.Length + 2;
				if (projectZIPFilePath.Count<string>() > 1)
				{
					for (int i = 1; i < projectZIPFilePath.Count<string>(); i++)
					{
						fileName = Path.GetFileName(projectZIPFilePath[i]);
						if (fileName != null)
						{
							stringBuilder.Append(", " + fileName);
							if (num < 60)
							{
								length = fileName.Length;
								if (num + fileName.Length > 60)
								{
									length = 60 - num;
								}
								this.mLinkLabelPackAndGo.Links.Add(num, length, projectZIPFilePath[i]);
							}
							num += fileName.Length + 2;
						}
					}
				}
			}
			string text = stringBuilder.ToString();
			if (text.Length > 60)
			{
				text = text.Substring(0, 60) + "...";
			}
			this.mLinkLabelPackAndGo.Text = text;
		}

		private void UpdateAdditionalInfo(bool forceHide = false)
		{
			this.mPictureBoxInfo.Visible = false;
			this.mPictureBoxError.Visible = false;
			this.mRichTextBoxInfo.Visible = false;
			this.mRichTextBoxInfo.Clear();
			this.mLinkLabelClusterSizeNotOptimal.Visible = false;
			if (forceHide || this.mVN16XXlogDevice == null || !this.IsLoggerConnectedDisplayMode)
			{
				return;
			}
			switch (this.mVN16XXlogDevice.AdditionalInfoType)
			{
			case EnumInfoType.Info:
				this.mPictureBoxInfo.Visible = true;
				break;
			case EnumInfoType.Error:
				this.mPictureBoxError.Visible = true;
				break;
			}
			if (string.IsNullOrEmpty(this.mVN16XXlogDevice.AdditionalInfoText))
			{
				if (!this.mDevice.HasProperClusterSize)
				{
					this.mLinkLabelClusterSizeNotOptimal.Visible = true;
				}
				return;
			}
			this.mRichTextBoxInfo.Visible = true;
			if (RtfText.IsRtf(this.mVN16XXlogDevice.AdditionalInfoText))
			{
				this.mRichTextBoxInfo.Rtf = this.mVN16XXlogDevice.AdditionalInfoText;
				return;
			}
			this.mRichTextBoxInfo.Text = this.mVN16XXlogDevice.AdditionalInfoText;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(DeviceInformationVN16XXlog));
			this.mGroupBoxDevice = new GroupBox();
			this.tableLayoutPanel1 = new TableLayoutPanel();
			this.labelDeviceType = new Label();
			this.labelSerialNumber = new Label();
			this.labelFirmwareVersion = new Label();
			this.mLabelTypeValue = new Label();
			this.mLabelSerialNumberValue = new Label();
			this.mLabelFirmwareVersionValue = new Label();
			this.labelDriverVersion = new Label();
			this.mLabelDriverVersionValue = new Label();
			this.labelCapacity = new Label();
			this.labelFreeSpace = new Label();
			this.mLabelCapacityValue = new Label();
			this.mLabelFreeSpaceValue = new Label();
			this.labelTriggeredFiles = new Label();
			this.labelTriggeredFilesValue = new Label();
			this.labelConfigFile = new Label();
			this.mLabelConfigFileValue = new Label();
			this.labelPackAndGo = new Label();
			this.mLinkLabelPackAndGo = new LinkLabel();
			this.labelConfiguration = new Label();
			this.labelChannel1 = new Label();
			this.mLabelChannel1Value = new Label();
			this.labelChannel2 = new Label();
			this.mLabelChannel2Value = new Label();
			this.labelChannel3 = new Label();
			this.mLabelChannel3Value = new Label();
			this.labelChannel4 = new Label();
			this.mLabelChannel4Value = new Label();
			this.labelAnalogInputs = new Label();
			this.labelAnalogInputsValue = new Label();
			this.labelDigitalInOut = new Label();
			this.labelDigitalInputsValue = new Label();
			this.labelFirmwareVersionInterfaceMode = new Label();
			this.mLabelFirmwareVersioninterfaceModeValue = new Label();
			this.mLabelNoLoggerDeviceConnected = new Label();
			this.mLabelLoggerConnectedButInaccessible = new Label();
			this.mRichTextBoxInfo = new RichTextBox();
			this.mLinkLabelClusterSizeNotOptimal = new LinkLabel();
			this.mPictureBoxInfo = new PictureBox();
			this.mPictureBoxError = new PictureBox();
			this.labelLoggerDeviceConnectedWithoutMemCard = new Label();
			this.mGroupBoxDevice.SuspendLayout();
			this.tableLayoutPanel1.SuspendLayout();
			((ISupportInitialize)this.mPictureBoxInfo).BeginInit();
			((ISupportInitialize)this.mPictureBoxError).BeginInit();
			base.SuspendLayout();
			this.mGroupBoxDevice.Controls.Add(this.tableLayoutPanel1);
			componentResourceManager.ApplyResources(this.mGroupBoxDevice, "mGroupBoxDevice");
			this.mGroupBoxDevice.Name = "mGroupBoxDevice";
			this.mGroupBoxDevice.TabStop = false;
			componentResourceManager.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.labelDeviceType, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.labelSerialNumber, 0, 2);
			this.tableLayoutPanel1.Controls.Add(this.labelFirmwareVersion, 0, 5);
			this.tableLayoutPanel1.Controls.Add(this.mLabelTypeValue, 1, 1);
			this.tableLayoutPanel1.Controls.Add(this.mLabelSerialNumberValue, 1, 2);
			this.tableLayoutPanel1.Controls.Add(this.mLabelFirmwareVersionValue, 1, 5);
			this.tableLayoutPanel1.Controls.Add(this.labelDriverVersion, 0, 3);
			this.tableLayoutPanel1.Controls.Add(this.mLabelDriverVersionValue, 1, 3);
			this.tableLayoutPanel1.Controls.Add(this.labelCapacity, 0, 6);
			this.tableLayoutPanel1.Controls.Add(this.labelFreeSpace, 0, 7);
			this.tableLayoutPanel1.Controls.Add(this.mLabelCapacityValue, 1, 6);
			this.tableLayoutPanel1.Controls.Add(this.mLabelFreeSpaceValue, 1, 7);
			this.tableLayoutPanel1.Controls.Add(this.labelTriggeredFiles, 0, 8);
			this.tableLayoutPanel1.Controls.Add(this.labelTriggeredFilesValue, 1, 8);
			this.tableLayoutPanel1.Controls.Add(this.labelConfigFile, 0, 9);
			this.tableLayoutPanel1.Controls.Add(this.mLabelConfigFileValue, 1, 9);
			this.tableLayoutPanel1.Controls.Add(this.labelPackAndGo, 0, 10);
			this.tableLayoutPanel1.Controls.Add(this.mLinkLabelPackAndGo, 1, 10);
			this.tableLayoutPanel1.Controls.Add(this.labelConfiguration, 0, 11);
			this.tableLayoutPanel1.Controls.Add(this.labelChannel1, 0, 12);
			this.tableLayoutPanel1.Controls.Add(this.mLabelChannel1Value, 1, 12);
			this.tableLayoutPanel1.Controls.Add(this.labelChannel2, 0, 13);
			this.tableLayoutPanel1.Controls.Add(this.mLabelChannel2Value, 1, 13);
			this.tableLayoutPanel1.Controls.Add(this.labelChannel3, 0, 14);
			this.tableLayoutPanel1.Controls.Add(this.mLabelChannel3Value, 1, 14);
			this.tableLayoutPanel1.Controls.Add(this.labelChannel4, 0, 15);
			this.tableLayoutPanel1.Controls.Add(this.mLabelChannel4Value, 1, 15);
			this.tableLayoutPanel1.Controls.Add(this.labelAnalogInputs, 0, 16);
			this.tableLayoutPanel1.Controls.Add(this.labelAnalogInputsValue, 1, 16);
			this.tableLayoutPanel1.Controls.Add(this.labelDigitalInOut, 0, 17);
			this.tableLayoutPanel1.Controls.Add(this.labelDigitalInputsValue, 1, 17);
			this.tableLayoutPanel1.Controls.Add(this.labelFirmwareVersionInterfaceMode, 0, 4);
			this.tableLayoutPanel1.Controls.Add(this.mLabelFirmwareVersioninterfaceModeValue, 1, 4);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			componentResourceManager.ApplyResources(this.labelDeviceType, "labelDeviceType");
			this.labelDeviceType.Name = "labelDeviceType";
			componentResourceManager.ApplyResources(this.labelSerialNumber, "labelSerialNumber");
			this.labelSerialNumber.Name = "labelSerialNumber";
			componentResourceManager.ApplyResources(this.labelFirmwareVersion, "labelFirmwareVersion");
			this.labelFirmwareVersion.Name = "labelFirmwareVersion";
			componentResourceManager.ApplyResources(this.mLabelTypeValue, "mLabelTypeValue");
			this.mLabelTypeValue.Name = "mLabelTypeValue";
			componentResourceManager.ApplyResources(this.mLabelSerialNumberValue, "mLabelSerialNumberValue");
			this.mLabelSerialNumberValue.Name = "mLabelSerialNumberValue";
			componentResourceManager.ApplyResources(this.mLabelFirmwareVersionValue, "mLabelFirmwareVersionValue");
			this.mLabelFirmwareVersionValue.Name = "mLabelFirmwareVersionValue";
			componentResourceManager.ApplyResources(this.labelDriverVersion, "labelDriverVersion");
			this.labelDriverVersion.Name = "labelDriverVersion";
			componentResourceManager.ApplyResources(this.mLabelDriverVersionValue, "mLabelDriverVersionValue");
			this.mLabelDriverVersionValue.Name = "mLabelDriverVersionValue";
			componentResourceManager.ApplyResources(this.labelCapacity, "labelCapacity");
			this.labelCapacity.Name = "labelCapacity";
			componentResourceManager.ApplyResources(this.labelFreeSpace, "labelFreeSpace");
			this.labelFreeSpace.Name = "labelFreeSpace";
			componentResourceManager.ApplyResources(this.mLabelCapacityValue, "mLabelCapacityValue");
			this.mLabelCapacityValue.Name = "mLabelCapacityValue";
			componentResourceManager.ApplyResources(this.mLabelFreeSpaceValue, "mLabelFreeSpaceValue");
			this.mLabelFreeSpaceValue.Name = "mLabelFreeSpaceValue";
			componentResourceManager.ApplyResources(this.labelTriggeredFiles, "labelTriggeredFiles");
			this.labelTriggeredFiles.Name = "labelTriggeredFiles";
			componentResourceManager.ApplyResources(this.labelTriggeredFilesValue, "labelTriggeredFilesValue");
			this.labelTriggeredFilesValue.Name = "labelTriggeredFilesValue";
			componentResourceManager.ApplyResources(this.labelConfigFile, "labelConfigFile");
			this.labelConfigFile.Name = "labelConfigFile";
			componentResourceManager.ApplyResources(this.mLabelConfigFileValue, "mLabelConfigFileValue");
			this.mLabelConfigFileValue.Name = "mLabelConfigFileValue";
			componentResourceManager.ApplyResources(this.labelPackAndGo, "labelPackAndGo");
			this.labelPackAndGo.Name = "labelPackAndGo";
			componentResourceManager.ApplyResources(this.mLinkLabelPackAndGo, "mLinkLabelPackAndGo");
			this.mLinkLabelPackAndGo.Name = "mLinkLabelPackAndGo";
			this.mLinkLabelPackAndGo.LinkClicked += new LinkLabelLinkClickedEventHandler(this.packAndGo_LinkClicked);
			componentResourceManager.ApplyResources(this.labelConfiguration, "labelConfiguration");
			this.labelConfiguration.Name = "labelConfiguration";
			componentResourceManager.ApplyResources(this.labelChannel1, "labelChannel1");
			this.labelChannel1.Name = "labelChannel1";
			componentResourceManager.ApplyResources(this.mLabelChannel1Value, "mLabelChannel1Value");
			this.mLabelChannel1Value.Name = "mLabelChannel1Value";
			componentResourceManager.ApplyResources(this.labelChannel2, "labelChannel2");
			this.labelChannel2.Name = "labelChannel2";
			componentResourceManager.ApplyResources(this.mLabelChannel2Value, "mLabelChannel2Value");
			this.mLabelChannel2Value.Name = "mLabelChannel2Value";
			componentResourceManager.ApplyResources(this.labelChannel3, "labelChannel3");
			this.labelChannel3.Name = "labelChannel3";
			componentResourceManager.ApplyResources(this.mLabelChannel3Value, "mLabelChannel3Value");
			this.mLabelChannel3Value.Name = "mLabelChannel3Value";
			componentResourceManager.ApplyResources(this.labelChannel4, "labelChannel4");
			this.labelChannel4.Name = "labelChannel4";
			componentResourceManager.ApplyResources(this.mLabelChannel4Value, "mLabelChannel4Value");
			this.mLabelChannel4Value.Name = "mLabelChannel4Value";
			componentResourceManager.ApplyResources(this.labelAnalogInputs, "labelAnalogInputs");
			this.labelAnalogInputs.Name = "labelAnalogInputs";
			componentResourceManager.ApplyResources(this.labelAnalogInputsValue, "labelAnalogInputsValue");
			this.labelAnalogInputsValue.Name = "labelAnalogInputsValue";
			componentResourceManager.ApplyResources(this.labelDigitalInOut, "labelDigitalInOut");
			this.labelDigitalInOut.Name = "labelDigitalInOut";
			componentResourceManager.ApplyResources(this.labelDigitalInputsValue, "labelDigitalInputsValue");
			this.labelDigitalInputsValue.Name = "labelDigitalInputsValue";
			componentResourceManager.ApplyResources(this.labelFirmwareVersionInterfaceMode, "labelFirmwareVersionInterfaceMode");
			this.labelFirmwareVersionInterfaceMode.Name = "labelFirmwareVersionInterfaceMode";
			componentResourceManager.ApplyResources(this.mLabelFirmwareVersioninterfaceModeValue, "mLabelFirmwareVersioninterfaceModeValue");
			this.mLabelFirmwareVersioninterfaceModeValue.Name = "mLabelFirmwareVersioninterfaceModeValue";
			componentResourceManager.ApplyResources(this.mLabelNoLoggerDeviceConnected, "mLabelNoLoggerDeviceConnected");
			this.mLabelNoLoggerDeviceConnected.ForeColor = Color.Red;
			this.mLabelNoLoggerDeviceConnected.Name = "mLabelNoLoggerDeviceConnected";
			componentResourceManager.ApplyResources(this.mLabelLoggerConnectedButInaccessible, "mLabelLoggerConnectedButInaccessible");
			this.mLabelLoggerConnectedButInaccessible.ForeColor = Color.Red;
			this.mLabelLoggerConnectedButInaccessible.Name = "mLabelLoggerConnectedButInaccessible";
			this.mRichTextBoxInfo.BorderStyle = BorderStyle.None;
			componentResourceManager.ApplyResources(this.mRichTextBoxInfo, "mRichTextBoxInfo");
			this.mRichTextBoxInfo.Name = "mRichTextBoxInfo";
			this.mRichTextBoxInfo.ReadOnly = true;
			componentResourceManager.ApplyResources(this.mLinkLabelClusterSizeNotOptimal, "mLinkLabelClusterSizeNotOptimal");
			this.mLinkLabelClusterSizeNotOptimal.Name = "mLinkLabelClusterSizeNotOptimal";
			this.mLinkLabelClusterSizeNotOptimal.TabStop = true;
			this.mLinkLabelClusterSizeNotOptimal.UseCompatibleTextRendering = true;
			this.mLinkLabelClusterSizeNotOptimal.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkLabelClusterSizeNotOptimal_LinkClicked);
			componentResourceManager.ApplyResources(this.mPictureBoxInfo, "mPictureBoxInfo");
			this.mPictureBoxInfo.Name = "mPictureBoxInfo";
			this.mPictureBoxInfo.TabStop = false;
			componentResourceManager.ApplyResources(this.mPictureBoxError, "mPictureBoxError");
			this.mPictureBoxError.Name = "mPictureBoxError";
			this.mPictureBoxError.TabStop = false;
			componentResourceManager.ApplyResources(this.labelLoggerDeviceConnectedWithoutMemCard, "labelLoggerDeviceConnectedWithoutMemCard");
			this.labelLoggerDeviceConnectedWithoutMemCard.ForeColor = Color.Red;
			this.labelLoggerDeviceConnectedWithoutMemCard.Name = "labelLoggerDeviceConnectedWithoutMemCard";
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.mGroupBoxDevice);
			base.Controls.Add(this.mPictureBoxInfo);
			base.Controls.Add(this.mPictureBoxError);
			base.Controls.Add(this.mLinkLabelClusterSizeNotOptimal);
			base.Controls.Add(this.mLabelNoLoggerDeviceConnected);
			base.Controls.Add(this.mLabelLoggerConnectedButInaccessible);
			base.Controls.Add(this.mRichTextBoxInfo);
			base.Controls.Add(this.labelLoggerDeviceConnectedWithoutMemCard);
			base.Name = "DeviceInformationVN16XXlog";
			this.mGroupBoxDevice.ResumeLayout(false);
			this.tableLayoutPanel1.ResumeLayout(false);
			this.tableLayoutPanel1.PerformLayout();
			((ISupportInitialize)this.mPictureBoxInfo).EndInit();
			((ISupportInitialize)this.mPictureBoxError).EndInit();
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
