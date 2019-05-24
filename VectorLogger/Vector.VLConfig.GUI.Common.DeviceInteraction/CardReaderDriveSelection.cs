using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.HardwareAccess.HardwareAbstraction;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.Common.DeviceInteraction
{
	public class CardReaderDriveSelection : Form
	{
		private IEnumerable<DriveInfo> availableDrives;

		private Dictionary<string, DriveInfo> displayedDriveNameToDriveInfo;

		private IContainer components;

		private ComboBox comboBoxCardReaderDrives;

		private Label labelCurrentlyAvailableDrives;

		private Button buttonOK;

		private Button buttonCancel;

		public string SelectedDrive
		{
			get;
			set;
		}

		public CardReaderDriveSelection(IEnumerable<DriveInfo> cardReaderDrives)
		{
			this.InitializeComponent();
			this.availableDrives = cardReaderDrives;
			this.displayedDriveNameToDriveInfo = new Dictionary<string, DriveInfo>();
			this.FillCardReaderDrivesComboBox();
		}

		private void FillCardReaderDrivesComboBox()
		{
			foreach (DriveInfo current in this.availableDrives)
			{
				string cardReaderDisplayName = FileSystemServices.GetCardReaderDisplayName(current);
				this.comboBoxCardReaderDrives.Items.Add(cardReaderDisplayName);
				this.displayedDriveNameToDriveInfo.Add(cardReaderDisplayName, current);
			}
			if (this.comboBoxCardReaderDrives.Items.Count > 0)
			{
				this.comboBoxCardReaderDrives.SelectedIndex = 0;
			}
		}

		public static bool SelectCardReaderDrive(ILoggerSpecifics loggerSpecifics, IHardwareFrontend hardwareFrontend, List<string> additionalDrivesList, out string cardReaderDrive, bool isCardSelectedForReading = false, LoggerType loggerTypeToFilter = LoggerType.Unknown)
		{
			cardReaderDrive = string.Empty;
			List<string> additionalDrives = null;
			if (loggerSpecifics.DataStorage.IsUsingWindowsFileSystem)
			{
				additionalDrives = additionalDrivesList;
			}
			IEnumerable<DriveInfo> loggerSpecificDrives = CardReaderDriveSelection.GetLoggerSpecificDrives(hardwareFrontend, loggerSpecifics, additionalDrives, loggerTypeToFilter);
			if (loggerSpecificDrives.Count<DriveInfo>() > 0)
			{
				CardReaderDriveSelection cardReaderDriveSelection = new CardReaderDriveSelection(loggerSpecificDrives);
				if (DialogResult.OK == cardReaderDriveSelection.ShowDialog())
				{
					cardReaderDrive = cardReaderDriveSelection.SelectedDrive;
					return true;
				}
			}
			else if (!isCardSelectedForReading)
			{
				if (loggerSpecifics.Type == LoggerType.GL1000 || loggerSpecifics.Type == LoggerType.GL1020FTE)
				{
					InformMessageBox.Error(Resources.ErrorNoCardReaderDriveReady + Environment.NewLine + Environment.NewLine + Resources.HintCheckIfCardIsEmpty);
				}
				else
				{
					InformMessageBox.Error(Resources.ErrorNoCardReaderDriveReady + Environment.NewLine + Environment.NewLine + Resources.HintCheckIfCardHasWrongLoggerType);
				}
			}
			else
			{
				InformMessageBox.Error(Resources.ErrorNoCardReaderDriveReady);
			}
			return false;
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			this.SelectedDrive = this.displayedDriveNameToDriveInfo[this.comboBoxCardReaderDrives.SelectedItem.ToString()].Name;
		}

		private void CardReaderDriveSelection_HelpRequested(object sender, HelpEventArgs hlpevent)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private static IList<DriveInfo> GetLoggerSpecificDrives(IHardwareFrontend hardwareFrontend, ILoggerSpecifics loggerSpecifics, List<string> additionalDrives, LoggerType loggerTypeToFilter = LoggerType.Unknown)
		{
			hardwareFrontend.UpdateLoggerDeviceList();
			List<DriveInfo> list = new List<DriveInfo>();
			if (hardwareFrontend.CurrentLoggerDevices.Count > 0)
			{
				foreach (ILoggerDevice current in hardwareFrontend.CurrentLoggerDevices)
				{
					try
					{
						DriveInfo driveInfo = new DriveInfo(current.HardwareKey);
						if (loggerTypeToFilter != LoggerType.Unknown && loggerTypeToFilter != current.LoggerType)
						{
							int num = driveInfo.RootDirectory.GetFiles().Count<FileInfo>() + driveInfo.RootDirectory.GetDirectories().Count<DirectoryInfo>();
							if (num >= 1)
							{
								continue;
							}
						}
						list.Add(driveInfo);
					}
					catch (Exception)
					{
					}
				}
			}
			return list;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(CardReaderDriveSelection));
			this.comboBoxCardReaderDrives = new ComboBox();
			this.labelCurrentlyAvailableDrives = new Label();
			this.buttonOK = new Button();
			this.buttonCancel = new Button();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.comboBoxCardReaderDrives, "comboBoxCardReaderDrives");
			this.comboBoxCardReaderDrives.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxCardReaderDrives.FormattingEnabled = true;
			this.comboBoxCardReaderDrives.Name = "comboBoxCardReaderDrives";
			this.comboBoxCardReaderDrives.Sorted = true;
			componentResourceManager.ApplyResources(this.labelCurrentlyAvailableDrives, "labelCurrentlyAvailableDrives");
			this.labelCurrentlyAvailableDrives.Name = "labelCurrentlyAvailableDrives";
			componentResourceManager.ApplyResources(this.buttonOK, "buttonOK");
			this.buttonOK.DialogResult = DialogResult.OK;
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new EventHandler(this.buttonOK_Click);
			componentResourceManager.ApplyResources(this.buttonCancel, "buttonCancel");
			this.buttonCancel.DialogResult = DialogResult.Cancel;
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			base.AcceptButton = this.buttonOK;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.CancelButton = this.buttonCancel;
			base.ControlBox = false;
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.buttonOK);
			base.Controls.Add(this.labelCurrentlyAvailableDrives);
			base.Controls.Add(this.comboBoxCardReaderDrives);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "CardReaderDriveSelection";
			base.HelpRequested += new HelpEventHandler(this.CardReaderDriveSelection_HelpRequested);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
