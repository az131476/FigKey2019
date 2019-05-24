using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using Vector.UtilityFunctions;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.GUI.Helpers;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.Common.FileManager
{
	public class FileConversionSettings : UserControl
	{
		public delegate void FileConversionParametersChangedHandler(object sender, EventArgs e);

		public delegate void StartConversionHandler(object sender, EventArgs e);

		private ILoggerSpecifics loggerSpecifics;

		private FileConversionParameters parameters;

		private bool isInitControls;

		private ContextMenuStrip insertMacroContextMenuStrip;

		private bool isCLFConversion;

		private IContainer components;

		private GroupBox groupBoxGeneral;

		private ComboBox comboBoxDestinationFormat;

		private Label labelConvertTo;

		private CheckBox checkBoxSaveRawFile;

		private CheckBox checkBoxOverwriteDestination;

		private Label labelDestFolder;

		private TextBox textBoxDestinationFolder;

		private Label labelFilenameFormat;

		private ComboBox comboBoxFilenameFormat;

		private Label labelNewName;

		private TextBox textBoxPrefix;

		private Label labelPrefix;

		private GroupBox groupBoxAdvanced;

		private CheckBox checkBoxHexadecimal;

		private CheckBox checkBoxGlobalTimestamps;

		private CheckBox checkBoxRelativeTimestamps;

		private CheckBox checkBoxGermanExcelFormat;

		private CheckBox checkBoxSuppressBufferConcat;

		private ErrorProvider errorProvider;

		private ToolTip toolTip;

		private CheckBox checkBoxSingleFile;

		private SplitButton splitButtonInsertMacro;

		private Label labelCustomNameExample;

		private SplitButton splitButtonSelectDestinationFolder;

		private Label labelFractionMB;

		private TextBox textBoxFileFractionSize;

		private CheckBox checkBoxSplitByLoc;

		private CheckBox checkBoxSplitFileBySize;

		private CheckBox checkBoxCreateVsysvarFile;

		private GroupBox groupBox1;

		private ComboBox comboBoxTimeBase;

		private TextBox textBoxFileFractionTime;

		private CheckBox checkBoxAbsoluteTimestamps;

		private CheckBox checkBoxSplitFileByTime;

		private CheckBox checkBoxUseDateTimeFromMeasurementStart;

		private ComboBox comboBoxDestinationFormatVersion;

		private Label labelVersion;

		private CheckBox checkBoxMinDigitsForTriggerIndex;

		private NumericUpDownEx numericUpDownMinDigitsForTriggerIndex;

		private ComboBox comboBoxDestinationFormatExtension;

		private Label labelExtension;

		private CheckBox checkBoxCopyMediaFiles;

		private CheckBox checkBoxWriteRawValues;

		private CheckBox checkBoxRecoveryMode;

		private TextBox textBoxMDF3SignalDelimiter;

		private Label labelMDF3SignalDelimiter;

		private ComboBox comboBoxMDF3SignalFormat;

		private Label labelMDF3SignalFormat;

		public event FileConversionSettings.FileConversionParametersChangedHandler ParametersChanged;

		public FileConversionParameters FileConversionParameters
		{
			get
			{
				return this.parameters;
			}
			set
			{
				this.parameters = value;
				if (this.loggerSpecifics != null)
				{
					this.ApplyParametersToControls();
				}
			}
		}

		public bool HasError
		{
			get
			{
				return (this.parameters.FilenameFormat != FileConversionFilenameFormat.UseOriginalName && !this.ValidateFilenameString()) || (this.parameters.SplitFilesBySize && !this.ValidateFileFractionSizeString()) || (this.parameters.SplitFilesByTime && !this.ValidateFileFractionTimeString());
			}
		}

		public ILoggerSpecifics LoggerSpecifics
		{
			get
			{
				return this.loggerSpecifics;
			}
			set
			{
				if (this.loggerSpecifics != null && this.loggerSpecifics.Type != value.Type)
				{
					this.loggerSpecifics = value;
					this.InitLoggerSpecificsDependencies();
					this.FillDestinationFormatComboBox(this.isCLFConversion);
					this.ApplyParametersToControls();
					this.InitializeSplitButtonInsertMacro();
					this.ValidateFilenameString();
				}
			}
		}

		public FileConversionSettings()
		{
			this.InitializeComponent();
			this.isInitControls = false;
			this.splitButtonInsertMacro.AutoSize = false;
			this.splitButtonSelectDestinationFolder.AutoSize = false;
			this.splitButtonSelectDestinationFolder.SplitMenu = new ContextMenu();
			this.UpdateSplitButtonSelectDestinationFolder();
		}

		public void Init(ILoggerSpecifics loggerSpecs, bool isCLFConversionMode)
		{
			this.loggerSpecifics = loggerSpecs;
			this.isCLFConversion = isCLFConversionMode;
			this.isInitControls = true;
			this.InitLoggerSpecificsDependencies();
			this.InitializeSplitButtonInsertMacro();
			this.FillDestinationFormatComboBox(isCLFConversionMode);
			this.FillFilenameFormatComboBox(isCLFConversionMode);
			this.FillTimeBaseComboBox();
			if (isCLFConversionMode)
			{
				this.checkBoxSaveRawFile.Visible = false;
				this.checkBoxSuppressBufferConcat.Visible = false;
			}
			else
			{
				this.checkBoxSaveRawFile.Visible = true;
				this.checkBoxSaveRawFile.Checked = true;
				this.checkBoxSaveRawFile.Enabled = true;
				this.checkBoxSuppressBufferConcat.Visible = true;
			}
			this.isInitControls = false;
			this.textBoxFileFractionSize.Enabled = this.checkBoxSplitFileBySize.Checked;
			Dictionary<decimal, string> dictionary = new Dictionary<decimal, string>();
			dictionary.Add(0m, "Auto");
			this.numericUpDownMinDigitsForTriggerIndex.Initialize(0m, Constants.MaxMinimumDigitsForTriggerIndex, dictionary);
		}

		private void InitLoggerSpecificsDependencies()
		{
			this.checkBoxSaveRawFile.Text = string.Format(Resources.FileConversionSaveRawFile, this.loggerSpecifics.FileConversion.RawFileFormat);
			this.checkBoxCopyMediaFiles.Visible = ((this.loggerSpecifics.Recording.IsVoCANSupported || this.loggerSpecifics.Recording.IsCameraSupported) && !this.isCLFConversion);
		}

		private void InitializeSplitButtonInsertMacro()
		{
			this.splitButtonInsertMacro.ShowSplit = true;
			this.splitButtonInsertMacro.ShowSplitAlways = true;
			this.insertMacroContextMenuStrip = new ContextMenuStrip();
			ILoggerSpecifics loggerSpecifics = null;
			if (!this.isCLFConversion)
			{
				loggerSpecifics = this.loggerSpecifics;
			}
			foreach (string current in FileConversionHelper.GetBasicMacroNames(loggerSpecifics))
			{
				this.insertMacroContextMenuStrip.Items.Add(current);
			}
			this.insertMacroContextMenuStrip.Items.Add(new ToolStripSeparator());
			foreach (string current2 in FileConversionHelper.GetNavigatorMacroNames())
			{
				this.insertMacroContextMenuStrip.Items.Add(current2);
			}
			this.insertMacroContextMenuStrip.Items.Add(new ToolStripSeparator());
			foreach (string current3 in FileConversionHelper.GetSuperMacroNames())
			{
				this.insertMacroContextMenuStrip.Items.Add(current3);
			}
			this.splitButtonInsertMacro.ContextMenuStrip = this.insertMacroContextMenuStrip;
			this.insertMacroContextMenuStrip.ItemClicked += new ToolStripItemClickedEventHandler(this.insertMacroContextMenuStrip_ItemClicked);
		}

		private void Raise_ParametersChanged(object sender, EventArgs e)
		{
			if (this.ParametersChanged != null)
			{
				this.ParametersChanged(sender, e);
			}
		}

		public void RefreshStatus()
		{
			this.DisplayFreeCapacityOnDestinationVolume();
		}

		private void FileConversionSettings_VisibleChanged(object sender, EventArgs e)
		{
			if (base.Visible)
			{
				this.UpdateSplitButtonSelectDestinationFolder();
			}
		}

		private void splitButtonSelectDestinationFolder_Click(object sender, EventArgs e)
		{
			using (BrowseFolderDialog browseFolderDialog = new BrowseFolderDialog())
			{
				browseFolderDialog.Title = Resources.SelectDestFolder;
				browseFolderDialog.SelectedPath = this.parameters.DestinationFolder;
				if (DialogResult.OK == browseFolderDialog.ShowDialog())
				{
					if (!string.IsNullOrEmpty(browseFolderDialog.SelectedPath) && Directory.Exists(browseFolderDialog.SelectedPath))
					{
						if (!GUIUtil.FolderAccessible(browseFolderDialog.SelectedPath))
						{
							InformMessageBox.Error(Resources.ErrorUnaccessiblePath);
						}
						else
						{
							this.SelectCustomDirectory(browseFolderDialog.SelectedPath);
						}
					}
				}
			}
		}

		private void splitButtonSelectDestinationFolderMenuItem_Click(object sender, EventArgs e)
		{
			if (sender is MenuItem)
			{
				MenuItem menuItem = sender as MenuItem;
				if (menuItem == null)
				{
					return;
				}
				string text = "";
				if (menuItem.Tag is string)
				{
					text = (menuItem.Tag as string);
				}
				bool flag = GlobalOptionsManager.FolderAccessible(GlobalOptionsManager.ListSelector.DestinationFolders, text);
				this.UpdateSplitButtonSelectDestinationFolder();
				if (flag)
				{
					this.SelectCustomDirectory(text);
				}
			}
		}

		private void comboBoxDestinationFormat_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			FileConversionDestFormat selectedFormat = FileConversionDestFormatComboboxItem.GetSelectedFormat(this.comboBoxDestinationFormat, this.loggerSpecifics.FileConversion.DefaultDestFormat);
			if (selectedFormat != this.parameters.DestinationFormat)
			{
				this.parameters.DestinationFormat = selectedFormat;
				this.Raise_ParametersChanged(this, new FileConversionParametersChangedEventArgs(FileConversionParametersChangedEventArgs.Parameter.DestinationFormat));
				this.UpdateVersionComboBoxDependingOnDestinationFormat();
				this.UpdateExtensionComboBoxDependingOnDestinationFormatAndVersion();
				this.EnableControlsDependingOnDestinationFormat();
				this.EnableCheckboxUseDateTimeFromMeasurementStart();
			}
		}

		private void comboBoxFilenameFormat_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			FileConversionFilenameFormat filenameFormat = this.parameters.FilenameFormat;
			this.parameters.FilenameFormat = FileConversionHelper.String2FileConversionFilenameFormat(this.comboBoxFilenameFormat.SelectedItem.ToString());
			if (filenameFormat != this.parameters.FilenameFormat)
			{
				if (this.parameters.FilenameFormat == FileConversionFilenameFormat.UseCustomName)
				{
					this.textBoxPrefix.Text = this.parameters.CustomFilename;
				}
				else if (this.parameters.FilenameFormat == FileConversionFilenameFormat.AddPrefix)
				{
					this.textBoxPrefix.Text = this.parameters.Prefix;
				}
			}
			this.ShowControlsForFilenameFormat();
			if (this.textBoxPrefix.Visible)
			{
				this.ValidateFilenameString();
			}
			else if (!string.IsNullOrEmpty(this.errorProvider.GetError(this.textBoxPrefix)))
			{
				this.errorProvider.SetError(this.textBoxPrefix, "");
			}
			this.Raise_ParametersChanged(this, EventArgs.Empty);
		}

		private void ComboBoxTimeBase_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			FileConversionTimeBase timeBase = FileConversionHelper.String2FileConversionTimeBase(this.comboBoxTimeBase.SelectedItem.ToString());
			if (this.ValidateFileFractionTimeString())
			{
				this.parameters.TimeBase = timeBase;
				this.parameters.FileFractionTime = Convert.ToInt32(this.textBoxFileFractionTime.Text);
				this.Raise_ParametersChanged(this, EventArgs.Empty);
			}
		}

		private void numericUpDownMinDigitsForTriggerIndex_ValueChanged(object sender, EventArgs e)
		{
			if (this.isInitControls || this.parameters == null)
			{
				return;
			}
			this.parameters.MinimumDigitsForTriggerIndex = (int)Math.Round(this.numericUpDownMinDigitsForTriggerIndex.Value);
			this.Raise_ParametersChanged(this, EventArgs.Empty);
		}

		private void checkBoxOverwriteDestination_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.parameters.OverwriteDestinationFiles = this.checkBoxOverwriteDestination.Checked;
			this.Raise_ParametersChanged(this, EventArgs.Empty);
		}

		private void checkBoxSaveRawFile_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.parameters.SaveRawFile = this.checkBoxSaveRawFile.Checked;
			this.Raise_ParametersChanged(this, EventArgs.Empty);
		}

		private void checkBoxHexadecimal_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.parameters.HexadecimalNotation = this.checkBoxHexadecimal.Checked;
			this.Raise_ParametersChanged(this, EventArgs.Empty);
		}

		private void checkBoxSingleFile_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.parameters.SingleFile = this.checkBoxSingleFile.Checked;
			this.EnableSingleFileOrGlobalTimestampsOption();
			this.Raise_ParametersChanged(this, EventArgs.Empty);
		}

		private void checkBoxGlobalTimestamps_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.parameters.GlobalTimestamps = this.checkBoxGlobalTimestamps.Checked;
			this.EnableSingleFileOrGlobalTimestampsOption();
			this.Raise_ParametersChanged(this, EventArgs.Empty);
		}

		private void checkBoxRelativeTimestamps_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.parameters.RelativeTimestamps = this.checkBoxRelativeTimestamps.Checked;
			this.Raise_ParametersChanged(this, EventArgs.Empty);
		}

		private void checkBoxGermanExcelFormat_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.parameters.GermanMSExcelFormat = this.checkBoxGermanExcelFormat.Checked;
			this.Raise_ParametersChanged(this, EventArgs.Empty);
		}

		private void checkBoxSuppressBufferConcat_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.parameters.SuppressBufferConcat = this.checkBoxSuppressBufferConcat.Checked;
			this.EnableSingleFileOrGlobalTimestampsOption();
			this.Raise_ParametersChanged(this, EventArgs.Empty);
		}

		private void checkBoxSplitByFileSize_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.textBoxFileFractionSize.Enabled = this.checkBoxSplitFileBySize.Checked;
			this.parameters.SplitFilesBySize = this.checkBoxSplitFileBySize.Checked;
			this.EnableCheckboxUseDateTimeFromMeasurementStart();
			this.Raise_ParametersChanged(this, EventArgs.Empty);
		}

		private void CheckBoxSplitFileByTime_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.textBoxFileFractionTime.Enabled = this.checkBoxSplitFileByTime.Checked;
			this.checkBoxAbsoluteTimestamps.Enabled = this.checkBoxSplitFileByTime.Checked;
			this.comboBoxTimeBase.Enabled = this.checkBoxSplitFileByTime.Checked;
			this.parameters.SplitFilesByTime = this.checkBoxSplitFileByTime.Checked;
			this.EnableCheckboxUseDateTimeFromMeasurementStart();
			this.Raise_ParametersChanged(this, EventArgs.Empty);
		}

		private void CheckBoxAbsoluteTimestamps_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.parameters.UseRealTimeRaster = this.checkBoxAbsoluteTimestamps.Checked;
			this.Raise_ParametersChanged(this, EventArgs.Empty);
		}

		private void CheckBoxSplitByLoc_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.parameters.UseUnlimitedFileSize = !this.checkBoxSplitByLoc.Checked;
			this.EnableCheckboxUseDateTimeFromMeasurementStart();
			this.Raise_ParametersChanged(this, EventArgs.Empty);
		}

		private void checkBoxCreateVsysvarFile_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.parameters.GenerateVsysvarFile = this.checkBoxCreateVsysvarFile.Checked;
			this.Raise_ParametersChanged(this, EventArgs.Empty);
		}

		private void checkBoxUseDateTimeFromMeasurementStart_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.parameters.UseDateTimeFromMeasurementStart = this.checkBoxUseDateTimeFromMeasurementStart.Checked;
			this.Raise_ParametersChanged(this, EventArgs.Empty);
		}

		private void checkBoxMinDigitsForTriggerIndex_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls || this.parameters == null)
			{
				return;
			}
			this.parameters.UseMinimumDigitsForTriggerIndex = this.checkBoxMinDigitsForTriggerIndex.Checked;
			this.numericUpDownMinDigitsForTriggerIndex.Enabled = this.checkBoxMinDigitsForTriggerIndex.Checked;
			this.Raise_ParametersChanged(this, EventArgs.Empty);
		}

		private void textBoxPrefix_Validating(object sender, CancelEventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			if (this.ValidateFilenameString())
			{
				if (this.parameters.FilenameFormat == FileConversionFilenameFormat.UseCustomName)
				{
					this.parameters.CustomFilename = this.textBoxPrefix.Text;
				}
				else
				{
					this.parameters.Prefix = this.textBoxPrefix.Text;
				}
				this.Raise_ParametersChanged(this, EventArgs.Empty);
			}
		}

		private void textBoxFileFractionSize_Validating(object sender, CancelEventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			if (this.ValidateFileFractionSizeString())
			{
				this.parameters.FileFractionSize = Convert.ToInt32(this.textBoxFileFractionSize.Text);
				this.Raise_ParametersChanged(this, EventArgs.Empty);
			}
		}

		private void textBoxFileFractionSize_EnabledChanged(object sender, EventArgs e)
		{
			if (!this.textBoxFileFractionSize.Enabled)
			{
				this.errorProvider.SetError(this.textBoxFileFractionSize, "");
				return;
			}
			this.ValidateFileFractionSizeString();
		}

		private void TextBoxFileFractionTime_Validating(object sender, CancelEventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			if (this.ValidateFileFractionTimeString())
			{
				this.parameters.FileFractionTime = Convert.ToInt32(this.textBoxFileFractionTime.Text);
				this.parameters.TimeBase = FileConversionHelper.String2FileConversionTimeBase(this.comboBoxTimeBase.SelectedItem.ToString());
				this.Raise_ParametersChanged(this, EventArgs.Empty);
			}
		}

		private void TextBoxFileFractionTime_EnabledChanged(object sender, EventArgs e)
		{
			if (!this.textBoxFileFractionTime.Enabled)
			{
				this.errorProvider.SetError(this.textBoxFileFractionTime, "");
				return;
			}
			this.ValidateFileFractionTimeString();
		}

		private void textBoxDestinationFolder_MouseEnter(object sender, EventArgs e)
		{
			this.toolTip.SetToolTip(this.textBoxDestinationFolder, this.textBoxDestinationFolder.Text);
		}

		private void textBoxDestinationFolder_DoubleClick(object sender, EventArgs e)
		{
			FileSystemServices.LaunchDirectoryBrowser(this.textBoxDestinationFolder.Text);
		}

		private void textBoxPrefix_MouseEnter(object sender, EventArgs e)
		{
			this.toolTip.SetToolTip(this.textBoxPrefix, this.textBoxPrefix.Text);
		}

		private void splitButtonInsertMacro_Click(object sender, EventArgs e)
		{
			this.splitButtonInsertMacro.Focus();
			SendKeys.Send("{DOWN}");
		}

		private void insertMacroContextMenuStrip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			this.textBoxPrefix.Paste(FileConversionHelper.GetMacroSymbolForName(e.ClickedItem.ToString()));
			if (this.ValidateFilenameString() && this.parameters.FilenameFormat == FileConversionFilenameFormat.UseCustomName)
			{
				this.parameters.CustomFilename = this.textBoxPrefix.Text;
				this.Raise_ParametersChanged(this, EventArgs.Empty);
			}
			base.ActiveControl = this.textBoxPrefix;
		}

		private void textBoxDestinationFolder_DragEnter(object sender, DragEventArgs e)
		{
			if (GUIUtil.GetKindOfDrop(e) != GUIUtil.FileDropContent.IllegalDrop)
			{
				e.Effect = DragDropEffects.Copy;
				return;
			}
			e.Effect = DragDropEffects.None;
		}

		private void textBoxDestinationFolder_DragDrop(object sender, DragEventArgs e)
		{
			GUIUtil.FileDropContent kindOfDrop = GUIUtil.GetKindOfDrop(e);
			if (kindOfDrop == GUIUtil.FileDropContent.IllegalDrop)
			{
				e.Effect = DragDropEffects.None;
				return;
			}
			string[] array = (string[])e.Data.GetData(DataFormats.FileDrop);
			string text = array[0];
			if (kindOfDrop != GUIUtil.FileDropContent.Folder)
			{
				text = Path.GetDirectoryName(text);
			}
			if (!GUIUtil.FolderAccessible(text))
			{
				InformMessageBox.Error(Resources.ErrorUnaccessiblePath);
				return;
			}
			this.SelectCustomDirectory(text);
		}

		private void checkBoxCopyMediaFiles_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.parameters.CopyMediaFiles = this.checkBoxCopyMediaFiles.Checked;
			this.Raise_ParametersChanged(this, EventArgs.Empty);
		}

		private void checkBoxWriteRawValues_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.parameters.WriteRawValues = this.checkBoxWriteRawValues.Checked;
			this.Raise_ParametersChanged(this, EventArgs.Empty);
		}

		private void SelectCustomDirectory(string selectedPath)
		{
			if (this.parameters.DestinationFolder != selectedPath)
			{
				this.parameters.DestinationFolder = selectedPath;
				this.textBoxDestinationFolder.Text = this.parameters.DestinationFolder;
				this.DisplayFreeCapacityOnDestinationVolume();
				GlobalOptionsManager.AddMostRecentFolder(GlobalOptionsManager.ListSelector.DestinationFolders, selectedPath);
				this.UpdateSplitButtonSelectDestinationFolder();
				this.Raise_ParametersChanged(this, new FileConversionParametersChangedEventArgs(FileConversionParametersChangedEventArgs.Parameter.DestinationFolder));
			}
		}

		private void UpdateSplitButtonSelectDestinationFolder()
		{
			List<string> recentFolders = GlobalOptionsManager.GetRecentFolders(GlobalOptionsManager.ListSelector.DestinationFolders);
			if (recentFolders == null)
			{
				return;
			}
			if (recentFolders.Count > 0)
			{
				this.splitButtonSelectDestinationFolder.SplitMenu.MenuItems.Clear();
				foreach (string current in recentFolders)
				{
					MenuItem menuItem = new MenuItem();
					string shortenedPath = PathUtil.GetShortenedPath(current, this.Font, GlobalOptionsManager.RecentFolderListWidth, false);
					menuItem.Text = shortenedPath;
					menuItem.Tag = current;
					menuItem.Click += new EventHandler(this.splitButtonSelectDestinationFolderMenuItem_Click);
					this.splitButtonSelectDestinationFolder.SplitMenu.MenuItems.Add(menuItem);
				}
				this.splitButtonSelectDestinationFolder.ShowSplit = true;
				return;
			}
			this.splitButtonSelectDestinationFolder.ShowSplit = false;
		}

		private void FillDestinationFormatComboBox(bool isCLFConversionMode)
		{
			this.comboBoxDestinationFormat.SelectedIndexChanged -= new EventHandler(this.comboBoxDestinationFormat_SelectedIndexChanged);
			this.comboBoxDestinationFormat.Items.Clear();
			foreach (FileConversionDestFormat current in this.loggerSpecifics.FileConversion.DestFormats)
			{
				if (!isCLFConversionMode || FileConversionDestFormat.CLF != current)
				{
					this.comboBoxDestinationFormat.Items.Add(new FileConversionDestFormatComboboxItem(current));
				}
			}
			FileConversionDestFormatComboboxItem.SelectItem(this.comboBoxDestinationFormat, this.loggerSpecifics.FileConversion.DefaultDestFormat);
			this.comboBoxDestinationFormat.SelectedIndexChanged += new EventHandler(this.comboBoxDestinationFormat_SelectedIndexChanged);
		}

		private void FillFilenameFormatComboBox(bool isCLFConversionMode)
		{
			this.comboBoxFilenameFormat.SelectedIndexChanged -= new EventHandler(this.comboBoxFilenameFormat_SelectedIndexChanged);
			this.comboBoxFilenameFormat.Items.Clear();
			foreach (FileConversionFilenameFormat fileConversionFilenameFormat in Enum.GetValues(typeof(FileConversionFilenameFormat)))
			{
				if (!isCLFConversionMode || FileConversionFilenameFormat.UseCustomName != fileConversionFilenameFormat)
				{
					this.comboBoxFilenameFormat.Items.Add(FileConversionHelper.FileConversionFilenameFormat2String(fileConversionFilenameFormat));
				}
			}
			if (this.comboBoxFilenameFormat.Items.Count > 0)
			{
				this.comboBoxFilenameFormat.SelectedIndex = 0;
			}
			this.comboBoxFilenameFormat.SelectedIndexChanged += new EventHandler(this.comboBoxFilenameFormat_SelectedIndexChanged);
		}

		private void FillTimeBaseComboBox()
		{
			this.comboBoxTimeBase.SelectedIndexChanged -= new EventHandler(this.ComboBoxTimeBase_SelectedIndexChanged);
			this.comboBoxTimeBase.Items.Clear();
			foreach (FileConversionTimeBase timeBase in Enum.GetValues(typeof(FileConversionTimeBase)))
			{
				this.comboBoxTimeBase.Items.Add(FileConversionHelper.FileConversionTimeBase2String(timeBase));
			}
			if (this.comboBoxTimeBase.Items.Count > 0)
			{
				this.comboBoxTimeBase.SelectedIndex = 0;
			}
			this.comboBoxTimeBase.SelectedIndexChanged += new EventHandler(this.ComboBoxTimeBase_SelectedIndexChanged);
		}

		private void ApplyParametersToControls()
		{
			if (this.parameters == null)
			{
				return;
			}
			this.isInitControls = true;
			this.parameters.DestinationFormat = FileConversionDestFormatComboboxItem.SelectItem(this.comboBoxDestinationFormat, this.parameters.DestinationFormat);
			this.textBoxDestinationFolder.Text = this.parameters.DestinationFolder;
			this.DisplayFreeCapacityOnDestinationVolume();
			this.checkBoxOverwriteDestination.Checked = this.parameters.OverwriteDestinationFiles;
			this.checkBoxSaveRawFile.Checked = this.parameters.SaveRawFile;
			this.checkBoxHexadecimal.Checked = this.parameters.HexadecimalNotation;
			this.checkBoxSingleFile.Checked = this.parameters.SingleFile;
			this.checkBoxGlobalTimestamps.Checked = this.parameters.GlobalTimestamps;
			this.checkBoxRelativeTimestamps.Checked = this.parameters.RelativeTimestamps;
			this.checkBoxGermanExcelFormat.Checked = this.parameters.GermanMSExcelFormat;
			this.checkBoxSuppressBufferConcat.Checked = this.parameters.SuppressBufferConcat;
			this.checkBoxSplitFileBySize.Checked = this.parameters.SplitFilesBySize;
			this.checkBoxSplitByLoc.Checked = !this.parameters.UseUnlimitedFileSize;
			this.checkBoxSplitFileByTime.Checked = this.parameters.SplitFilesByTime;
			this.comboBoxTimeBase.SelectedItem = FileConversionHelper.FileConversionTimeBase2String(this.parameters.TimeBase);
			this.checkBoxAbsoluteTimestamps.Checked = this.parameters.UseRealTimeRaster;
			this.checkBoxCreateVsysvarFile.Checked = this.parameters.GenerateVsysvarFile;
			this.checkBoxUseDateTimeFromMeasurementStart.Checked = this.parameters.UseDateTimeFromMeasurementStart;
			if (this.parameters.FileFractionSize != 0)
			{
				this.textBoxFileFractionSize.Text = this.parameters.FileFractionSize.ToString();
			}
			if (this.parameters.FileFractionTime != 0)
			{
				this.textBoxFileFractionTime.Text = this.parameters.FileFractionTime.ToString();
			}
			this.comboBoxFilenameFormat.SelectedItem = FileConversionHelper.FileConversionFilenameFormat2String(this.parameters.FilenameFormat);
			this.textBoxPrefix.Text = this.parameters.Prefix;
			this.checkBoxMinDigitsForTriggerIndex.Checked = this.parameters.UseMinimumDigitsForTriggerIndex;
			this.numericUpDownMinDigitsForTriggerIndex.Enabled = this.checkBoxMinDigitsForTriggerIndex.Checked;
			this.numericUpDownMinDigitsForTriggerIndex.Value = this.parameters.MinimumDigitsForTriggerIndex;
			this.checkBoxWriteRawValues.Checked = this.parameters.WriteRawValues;
			this.comboBoxMDF3SignalFormat.SelectedIndex = (int)this.parameters.MDF3SignalFormat;
			this.textBoxMDF3SignalDelimiter.Text = this.parameters.MDF3SignalFormatDelimiter;
			this.ShowControlsForFilenameFormat();
			if (this.parameters.FilenameFormat == FileConversionFilenameFormat.AddPrefix)
			{
				this.textBoxPrefix.Text = this.parameters.Prefix;
			}
			else if (this.parameters.FilenameFormat == FileConversionFilenameFormat.UseCustomName)
			{
				this.textBoxPrefix.Text = this.parameters.CustomFilename;
			}
			else
			{
				this.textBoxPrefix.Text = string.Empty;
			}
			this.ValidateFilenameString();
			this.checkBoxCopyMediaFiles.Checked = this.parameters.CopyMediaFiles;
			this.UpdateVersionComboBoxDependingOnDestinationFormat();
			this.UpdateExtensionComboBoxDependingOnDestinationFormatAndVersion();
			this.EnableControlsDependingOnDestinationFormat();
			this.EnableCheckboxUseDateTimeFromMeasurementStart();
			this.isInitControls = false;
		}

		private bool ValidateFilenameString()
		{
			this.labelCustomNameExample.Text = "";
			string text = this.textBoxPrefix.Text;
			string text2 = FileConversionHelper.FileNameAllowedAdditionalChars;
			string value = string.Format(Resources.ErrorInvalidCharsFoundGen, GUIUtil.GetBlankSeparatedCharList(text2));
			if (this.parameters.FilenameFormat == FileConversionFilenameFormat.UseCustomName)
			{
				text2 += FileConversionHelper.FileNameMacroChars;
				value = string.Format(Resources.ErrorInvalidCharsFoundGen, GUIUtil.GetBlankSeparatedCharList(text2));
			}
			for (int i = 0; i < text.Length; i++)
			{
				if (!char.IsLetterOrDigit(text[i]) && text2.IndexOf(text[i]) < 0)
				{
					this.errorProvider.SetError(this.textBoxPrefix, value);
					return false;
				}
			}
			string arg = "";
			if (this.parameters.FilenameFormat == FileConversionFilenameFormat.UseCustomName && !FileConversionHelper.ValidateMacrosInFileNamePattern(ref this.textBoxPrefix, ref this.errorProvider, this.isCLFConversion, true, this.loggerSpecifics, out arg))
			{
				return false;
			}
			this.labelCustomNameExample.Text = string.Format(Resources.ExampleName, arg);
			this.errorProvider.SetError(this.textBoxPrefix, "");
			return true;
		}

		private bool ValidateFileFractionSizeString()
		{
			int num = 1500;
			int num2 = 5;
			string value = string.Format(Resources.ErrorGenValueOutOfRangeWithBorders, num2, num);
			int num3;
			try
			{
				num3 = Convert.ToInt32(this.textBoxFileFractionSize.Text);
			}
			catch (Exception)
			{
				this.errorProvider.SetError(this.textBoxFileFractionSize, value);
				bool result = false;
				return result;
			}
			if (num3 < num2 || num3 > num)
			{
				this.errorProvider.SetError(this.textBoxFileFractionSize, value);
				return false;
			}
			this.errorProvider.SetError(this.textBoxFileFractionSize, "");
			return true;
		}

		private bool ValidateFileFractionTimeString()
		{
			int num = 1;
			int num2 = 23;
			FileConversionTimeBase fileConversionTimeBase = FileConversionHelper.String2FileConversionTimeBase(this.comboBoxTimeBase.SelectedItem.ToString());
			if (fileConversionTimeBase == FileConversionTimeBase.Day)
			{
				num2 = 1;
			}
			else if (fileConversionTimeBase == FileConversionTimeBase.Minute)
			{
				num2 = 1439;
			}
			string value = string.Format(Resources.ErrorGenValueOutOfRangeWithBorders, num, num2);
			int num3;
			try
			{
				num3 = Convert.ToInt32(this.textBoxFileFractionTime.Text);
			}
			catch (Exception)
			{
				this.errorProvider.SetError(this.textBoxFileFractionTime, value);
				bool result = false;
				return result;
			}
			if (num3 < num || num3 > num2)
			{
				this.errorProvider.SetError(this.textBoxFileFractionTime, value);
				return false;
			}
			this.errorProvider.SetError(this.textBoxFileFractionTime, "");
			return true;
		}

		private void UpdateVersionComboBoxDependingOnDestinationFormat()
		{
			this.comboBoxDestinationFormatVersion.Items.Clear();
			IList<string> destinationFormatVersions = FileConversionHelper.GetDestinationFormatVersions(this.parameters);
			foreach (string current in destinationFormatVersions)
			{
				if (current != null && current != string.Empty)
				{
					this.comboBoxDestinationFormatVersion.Items.Add(current);
				}
			}
			if (this.comboBoxDestinationFormatVersion.Items.Count > 0)
			{
				this.comboBoxDestinationFormatVersion.SelectedItem = FileConversionHelper.GetConfiguredDestinationFormatVersion(this.parameters);
			}
		}

		private void UpdateExtensionComboBoxDependingOnDestinationFormatAndVersion()
		{
			this.comboBoxDestinationFormatExtension.Items.Clear();
			IList<string> destinationFormatExtensions = FileConversionHelper.GetDestinationFormatExtensions(this.parameters);
			string configuredDestinationFormatExtension = FileConversionHelper.GetConfiguredDestinationFormatExtension(this.parameters);
			foreach (string current in destinationFormatExtensions)
			{
				if (current != null && current != string.Empty)
				{
					this.comboBoxDestinationFormatExtension.Items.Add(current);
				}
			}
			if (this.comboBoxDestinationFormatExtension.Items.Count > 0)
			{
				this.comboBoxDestinationFormatExtension.SelectedItem = configuredDestinationFormatExtension;
				if (this.comboBoxDestinationFormatExtension.SelectedItem == null)
				{
					this.comboBoxDestinationFormatExtension.SelectedItem = FileConversionHelper.GetDefaultFileExtension(this.parameters);
				}
			}
		}

		private void EnableControlsDependingOnDestinationFormat()
		{
			this.parameters.HexadecimalNotation = FileConversionHelper.EnableAndSetByFormat(this.checkBoxHexadecimal, this.parameters.DestinationFormat, EnumFileConversionParameter.HexadecimalNotation);
			this.parameters.SingleFile = FileConversionHelper.EnableAndSetByFormat(this.checkBoxSingleFile, this.parameters.DestinationFormat, EnumFileConversionParameter.SingleFile);
			this.parameters.GlobalTimestamps = FileConversionHelper.EnableAndSetByFormat(this.checkBoxGlobalTimestamps, this.parameters.DestinationFormat, EnumFileConversionParameter.GlobalTimestamps);
			this.parameters.RelativeTimestamps = FileConversionHelper.EnableAndSetByFormat(this.checkBoxRelativeTimestamps, this.parameters.DestinationFormat, EnumFileConversionParameter.RelativeTimestamps);
			this.parameters.GermanMSExcelFormat = FileConversionHelper.EnableAndSetByFormat(this.checkBoxGermanExcelFormat, this.parameters.DestinationFormat, EnumFileConversionParameter.GermanMSExcelFormat);
			this.parameters.SaveRawFile = FileConversionHelper.EnableAndSetByFormat(this.checkBoxSaveRawFile, this.parameters.DestinationFormat, EnumFileConversionParameter.SaveRawFile);
			this.parameters.SuppressBufferConcat = FileConversionHelper.EnableAndSetByFormat(this.checkBoxSuppressBufferConcat, this.parameters.DestinationFormat, EnumFileConversionParameter.SuppressBufferConcat);
			this.parameters.SplitFilesBySize = FileConversionHelper.EnableAndSetByFormat(this.checkBoxSplitFileBySize, this.parameters.DestinationFormat, EnumFileConversionParameter.SplitFilesBySize);
			this.parameters.UseUnlimitedFileSize = !FileConversionHelper.EnableAndSetByFormat(this.checkBoxSplitByLoc, this.parameters.DestinationFormat, EnumFileConversionParameter.SplitByLoc);
			this.parameters.GenerateVsysvarFile = FileConversionHelper.EnableAndSetByFormat(this.checkBoxCreateVsysvarFile, this.parameters.DestinationFormat, EnumFileConversionParameter.CreateVsysvarFile);
			this.parameters.SplitFilesByTime = FileConversionHelper.EnableAndSetByFormat(this.checkBoxSplitFileByTime, this.parameters.DestinationFormat, EnumFileConversionParameter.SplitFilesByTime);
			this.parameters.UseMinimumDigitsForTriggerIndex = FileConversionHelper.EnableAndSetByFormat(this.checkBoxMinDigitsForTriggerIndex, this.parameters.DestinationFormat, EnumFileConversionParameter.UseMinDigitsForTriggerIndex);
			this.parameters.CopyMediaFiles = FileConversionHelper.EnableAndSetByFormat(this.checkBoxCopyMediaFiles, this.parameters.DestinationFormat, EnumFileConversionParameter.CopyMediaFiles);
			this.parameters.WriteRawValues = FileConversionHelper.EnableAndSetByFormat(this.checkBoxWriteRawValues, this.parameters.DestinationFormat, EnumFileConversionParameter.WriteRawValues);
			this.textBoxFileFractionSize.Enabled = (this.checkBoxSplitFileBySize.Enabled && this.checkBoxSplitFileBySize.Checked);
			this.labelFractionMB.Enabled = this.checkBoxSplitFileBySize.Enabled;
			this.textBoxFileFractionTime.Enabled = (this.checkBoxSplitFileByTime.Enabled && this.checkBoxSplitFileByTime.Checked);
			this.comboBoxTimeBase.Enabled = (this.checkBoxSplitFileByTime.Enabled && this.checkBoxSplitFileByTime.Checked);
			this.checkBoxAbsoluteTimestamps.Enabled = (this.checkBoxSplitFileByTime.Enabled && this.checkBoxSplitFileByTime.Checked);
			this.numericUpDownMinDigitsForTriggerIndex.Enabled = (this.checkBoxMinDigitsForTriggerIndex.Checked && this.checkBoxMinDigitsForTriggerIndex.Enabled);
			this.comboBoxDestinationFormatVersion.Enabled = (this.comboBoxDestinationFormatVersion.Items.Count > 1);
			this.comboBoxDestinationFormatExtension.Enabled = (this.comboBoxDestinationFormatExtension.Items.Count > 1);
			string configuredDestinationFormatVersion = FileConversionHelper.GetConfiguredDestinationFormatVersion(this.parameters);
			this.comboBoxMDF3SignalFormat.Enabled = (this.parameters.DestinationFormat == FileConversionDestFormat.MDF && configuredDestinationFormatVersion.StartsWith("3.") && !configuredDestinationFormatVersion.EndsWith("GiN"));
			this.textBoxMDF3SignalDelimiter.Enabled = this.comboBoxMDF3SignalFormat.Enabled;
			this.labelMDF3SignalDelimiter.Enabled = this.comboBoxMDF3SignalFormat.Enabled;
			this.labelMDF3SignalFormat.Enabled = this.comboBoxMDF3SignalFormat.Enabled;
			this.EnableSingleFileOrGlobalTimestampsOption();
			this.EnableCheckboxUseDateTimeFromMeasurementStart();
		}

		private void EnableSingleFileOrGlobalTimestampsOption()
		{
			if (this.checkBoxSingleFile.Enabled)
			{
				this.checkBoxGlobalTimestamps.Enabled = !this.checkBoxSingleFile.Checked;
				this.checkBoxSuppressBufferConcat.Enabled = !this.checkBoxSingleFile.Checked;
				if (this.checkBoxSingleFile.Checked)
				{
					this.checkBoxGlobalTimestamps.Checked = false;
					this.checkBoxSuppressBufferConcat.Checked = false;
				}
			}
			if (this.checkBoxGlobalTimestamps.Enabled || this.checkBoxSuppressBufferConcat.Enabled)
			{
				this.checkBoxSingleFile.Enabled = (!this.checkBoxGlobalTimestamps.Checked && !this.checkBoxSuppressBufferConcat.Checked);
				if (this.checkBoxGlobalTimestamps.Checked || this.checkBoxSuppressBufferConcat.Checked)
				{
					this.checkBoxSingleFile.Checked = false;
				}
			}
		}

		private void DisplayFreeCapacityOnDestinationVolume()
		{
			if (!string.IsNullOrEmpty(this.parameters.DestinationFolder))
			{
				string pathRoot = Path.GetPathRoot(this.parameters.DestinationFolder);
				if (!string.IsNullOrEmpty(pathRoot) && pathRoot != Path.DirectorySeparatorChar.ToString())
				{
					DriveInfo driveInfo;
					try
					{
						driveInfo = new DriveInfo(pathRoot.Substring(0, 1));
					}
					catch (Exception)
					{
						this.labelDestFolder.Text = string.Format(Resources.DestFolderFree, Resources.Unknown);
						return;
					}
					if (driveInfo.IsReady)
					{
						this.labelDestFolder.Text = string.Format(Resources.DestFolderFree, GUIUtil.GetSizeStringMBForBytes(driveInfo.AvailableFreeSpace));
						return;
					}
					this.SelectCustomDirectory(Environment.GetFolderPath(Environment.SpecialFolder.Personal));
					return;
				}
			}
			this.labelDestFolder.Text = string.Format(Resources.DestFolderFree, Resources.Unknown);
		}

		private void ShowControlsForFilenameFormat()
		{
			switch (this.parameters.FilenameFormat)
			{
			case FileConversionFilenameFormat.AddPrefix:
				this.labelPrefix.Visible = true;
				this.labelNewName.Visible = false;
				this.textBoxPrefix.Visible = true;
				this.splitButtonInsertMacro.Visible = false;
				this.labelCustomNameExample.Visible = false;
				goto IL_E1;
			case FileConversionFilenameFormat.UseCustomName:
				this.labelPrefix.Visible = false;
				this.labelNewName.Visible = true;
				this.textBoxPrefix.Visible = true;
				this.splitButtonInsertMacro.Visible = true;
				this.labelCustomNameExample.Visible = true;
				goto IL_E1;
			}
			this.labelPrefix.Visible = false;
			this.labelNewName.Visible = false;
			this.textBoxPrefix.Visible = false;
			this.splitButtonInsertMacro.Visible = false;
			this.labelCustomNameExample.Visible = false;
			IL_E1:
			this.EnableCheckboxUseDateTimeFromMeasurementStart();
		}

		private void EnableCheckboxUseDateTimeFromMeasurementStart()
		{
			bool flag = false;
			flag |= (this.checkBoxSplitFileBySize.Checked && this.checkBoxSplitFileBySize.Enabled);
			flag |= (this.checkBoxSplitFileByTime.Checked && this.checkBoxSplitFileByTime.Enabled);
			flag |= (this.checkBoxSplitByLoc.Checked && this.checkBoxSplitByLoc.Enabled);
			flag &= (this.parameters.FilenameFormat == FileConversionFilenameFormat.UseCustomName);
			this.checkBoxUseDateTimeFromMeasurementStart.Enabled = flag;
		}

		private void comboBoxDestinationFormatVersion_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			string text = this.comboBoxDestinationFormatVersion.SelectedItem.ToString();
			if (text != this.parameters.DestinationFormatVersionStrings[(int)this.parameters.DestinationFormat])
			{
				this.parameters.DestinationFormatVersionStrings[(int)this.parameters.DestinationFormat] = text;
				this.Raise_ParametersChanged(this, new FileConversionParametersChangedEventArgs(FileConversionParametersChangedEventArgs.Parameter.DestinationFormatVersion));
				this.UpdateExtensionComboBoxDependingOnDestinationFormatAndVersion();
				this.EnableControlsDependingOnDestinationFormat();
			}
		}

		private void comboBoxDestinationFormatExtension_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			string text = this.comboBoxDestinationFormatExtension.SelectedItem.ToString();
			if (text != FileConversionHelper.GetConfiguredDestinationFormatExtension(this.parameters))
			{
				this.parameters.DestinationFormatExtensions[(int)this.parameters.DestinationFormat] = text;
				this.Raise_ParametersChanged(this, new FileConversionParametersChangedEventArgs(FileConversionParametersChangedEventArgs.Parameter.DestinationFormatExtension));
			}
		}

		private void comboBoxMDF3SignalFormat_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.parameters.MDF3SignalFormat = (FileConversionMDF3SignalFormat)this.comboBoxMDF3SignalFormat.SelectedIndex;
			this.Raise_ParametersChanged(this, EventArgs.Empty);
		}

		private void textBoxMDF3SignalDelimiter_TextChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.parameters.MDF3SignalFormatDelimiter = this.textBoxMDF3SignalDelimiter.Text;
			this.Raise_ParametersChanged(this, EventArgs.Empty);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FileConversionSettings));
			this.groupBoxGeneral = new GroupBox();
			this.comboBoxDestinationFormatExtension = new ComboBox();
			this.labelExtension = new Label();
			this.comboBoxDestinationFormatVersion = new ComboBox();
			this.labelVersion = new Label();
			this.splitButtonSelectDestinationFolder = new SplitButton();
			this.labelCustomNameExample = new Label();
			this.splitButtonInsertMacro = new SplitButton();
			this.labelPrefix = new Label();
			this.textBoxPrefix = new TextBox();
			this.labelNewName = new Label();
			this.comboBoxFilenameFormat = new ComboBox();
			this.labelFilenameFormat = new Label();
			this.textBoxDestinationFolder = new TextBox();
			this.labelDestFolder = new Label();
			this.checkBoxSaveRawFile = new CheckBox();
			this.checkBoxOverwriteDestination = new CheckBox();
			this.comboBoxDestinationFormat = new ComboBox();
			this.labelConvertTo = new Label();
			this.groupBoxAdvanced = new GroupBox();
			this.textBoxMDF3SignalDelimiter = new TextBox();
			this.labelMDF3SignalDelimiter = new Label();
			this.comboBoxMDF3SignalFormat = new ComboBox();
			this.labelMDF3SignalFormat = new Label();
			this.checkBoxRecoveryMode = new CheckBox();
			this.checkBoxWriteRawValues = new CheckBox();
			this.checkBoxCopyMediaFiles = new CheckBox();
			this.checkBoxMinDigitsForTriggerIndex = new CheckBox();
			this.numericUpDownMinDigitsForTriggerIndex = new NumericUpDownEx();
			this.checkBoxCreateVsysvarFile = new CheckBox();
			this.checkBoxSingleFile = new CheckBox();
			this.checkBoxGermanExcelFormat = new CheckBox();
			this.checkBoxRelativeTimestamps = new CheckBox();
			this.checkBoxGlobalTimestamps = new CheckBox();
			this.checkBoxHexadecimal = new CheckBox();
			this.checkBoxUseDateTimeFromMeasurementStart = new CheckBox();
			this.textBoxFileFractionSize = new TextBox();
			this.checkBoxSplitFileBySize = new CheckBox();
			this.labelFractionMB = new Label();
			this.checkBoxSplitByLoc = new CheckBox();
			this.checkBoxSuppressBufferConcat = new CheckBox();
			this.errorProvider = new ErrorProvider(this.components);
			this.textBoxFileFractionTime = new TextBox();
			this.groupBox1 = new GroupBox();
			this.comboBoxTimeBase = new ComboBox();
			this.checkBoxAbsoluteTimestamps = new CheckBox();
			this.checkBoxSplitFileByTime = new CheckBox();
			this.toolTip = new ToolTip(this.components);
			this.groupBoxGeneral.SuspendLayout();
			this.groupBoxAdvanced.SuspendLayout();
			((ISupportInitialize)this.numericUpDownMinDigitsForTriggerIndex).BeginInit();
			((ISupportInitialize)this.errorProvider).BeginInit();
			this.groupBox1.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.groupBoxGeneral, "groupBoxGeneral");
			this.groupBoxGeneral.Controls.Add(this.comboBoxDestinationFormatExtension);
			this.groupBoxGeneral.Controls.Add(this.labelExtension);
			this.groupBoxGeneral.Controls.Add(this.comboBoxDestinationFormatVersion);
			this.groupBoxGeneral.Controls.Add(this.labelVersion);
			this.groupBoxGeneral.Controls.Add(this.splitButtonSelectDestinationFolder);
			this.groupBoxGeneral.Controls.Add(this.labelCustomNameExample);
			this.groupBoxGeneral.Controls.Add(this.splitButtonInsertMacro);
			this.groupBoxGeneral.Controls.Add(this.labelPrefix);
			this.groupBoxGeneral.Controls.Add(this.textBoxPrefix);
			this.groupBoxGeneral.Controls.Add(this.labelNewName);
			this.groupBoxGeneral.Controls.Add(this.comboBoxFilenameFormat);
			this.groupBoxGeneral.Controls.Add(this.labelFilenameFormat);
			this.groupBoxGeneral.Controls.Add(this.textBoxDestinationFolder);
			this.groupBoxGeneral.Controls.Add(this.labelDestFolder);
			this.groupBoxGeneral.Controls.Add(this.checkBoxSaveRawFile);
			this.groupBoxGeneral.Controls.Add(this.checkBoxOverwriteDestination);
			this.groupBoxGeneral.Controls.Add(this.comboBoxDestinationFormat);
			this.groupBoxGeneral.Controls.Add(this.labelConvertTo);
			this.groupBoxGeneral.Name = "groupBoxGeneral";
			this.groupBoxGeneral.TabStop = false;
			this.comboBoxDestinationFormatExtension.DropDownStyle = ComboBoxStyle.DropDownList;
			componentResourceManager.ApplyResources(this.comboBoxDestinationFormatExtension, "comboBoxDestinationFormatExtension");
			this.comboBoxDestinationFormatExtension.FormattingEnabled = true;
			this.comboBoxDestinationFormatExtension.Name = "comboBoxDestinationFormatExtension";
			this.comboBoxDestinationFormatExtension.SelectedIndexChanged += new EventHandler(this.comboBoxDestinationFormatExtension_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.labelExtension, "labelExtension");
			this.labelExtension.Name = "labelExtension";
			this.comboBoxDestinationFormatVersion.DropDownStyle = ComboBoxStyle.DropDownList;
			componentResourceManager.ApplyResources(this.comboBoxDestinationFormatVersion, "comboBoxDestinationFormatVersion");
			this.comboBoxDestinationFormatVersion.FormattingEnabled = true;
			this.comboBoxDestinationFormatVersion.Name = "comboBoxDestinationFormatVersion";
			this.comboBoxDestinationFormatVersion.SelectedIndexChanged += new EventHandler(this.comboBoxDestinationFormatVersion_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.labelVersion, "labelVersion");
			this.labelVersion.Name = "labelVersion";
			componentResourceManager.ApplyResources(this.splitButtonSelectDestinationFolder, "splitButtonSelectDestinationFolder");
			this.splitButtonSelectDestinationFolder.Name = "splitButtonSelectDestinationFolder";
			this.splitButtonSelectDestinationFolder.ShowSplitAlways = true;
			this.splitButtonSelectDestinationFolder.UseVisualStyleBackColor = true;
			this.splitButtonSelectDestinationFolder.Click += new EventHandler(this.splitButtonSelectDestinationFolder_Click);
			componentResourceManager.ApplyResources(this.labelCustomNameExample, "labelCustomNameExample");
			this.labelCustomNameExample.Name = "labelCustomNameExample";
			componentResourceManager.ApplyResources(this.splitButtonInsertMacro, "splitButtonInsertMacro");
			this.splitButtonInsertMacro.Name = "splitButtonInsertMacro";
			this.splitButtonInsertMacro.UseVisualStyleBackColor = true;
			this.splitButtonInsertMacro.Click += new EventHandler(this.splitButtonInsertMacro_Click);
			componentResourceManager.ApplyResources(this.labelPrefix, "labelPrefix");
			this.labelPrefix.Name = "labelPrefix";
			this.errorProvider.SetIconAlignment(this.textBoxPrefix, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxPrefix.IconAlignment"));
			componentResourceManager.ApplyResources(this.textBoxPrefix, "textBoxPrefix");
			this.textBoxPrefix.Name = "textBoxPrefix";
			this.textBoxPrefix.MouseEnter += new EventHandler(this.textBoxPrefix_MouseEnter);
			this.textBoxPrefix.Validating += new CancelEventHandler(this.textBoxPrefix_Validating);
			componentResourceManager.ApplyResources(this.labelNewName, "labelNewName");
			this.labelNewName.Name = "labelNewName";
			this.comboBoxFilenameFormat.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxFilenameFormat.FormattingEnabled = true;
			componentResourceManager.ApplyResources(this.comboBoxFilenameFormat, "comboBoxFilenameFormat");
			this.comboBoxFilenameFormat.Name = "comboBoxFilenameFormat";
			this.comboBoxFilenameFormat.SelectedIndexChanged += new EventHandler(this.comboBoxFilenameFormat_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.labelFilenameFormat, "labelFilenameFormat");
			this.labelFilenameFormat.Name = "labelFilenameFormat";
			this.textBoxDestinationFolder.AllowDrop = true;
			componentResourceManager.ApplyResources(this.textBoxDestinationFolder, "textBoxDestinationFolder");
			this.textBoxDestinationFolder.Name = "textBoxDestinationFolder";
			this.textBoxDestinationFolder.ReadOnly = true;
			this.textBoxDestinationFolder.DragDrop += new DragEventHandler(this.textBoxDestinationFolder_DragDrop);
			this.textBoxDestinationFolder.DragEnter += new DragEventHandler(this.textBoxDestinationFolder_DragEnter);
			this.textBoxDestinationFolder.DoubleClick += new EventHandler(this.textBoxDestinationFolder_DoubleClick);
			this.textBoxDestinationFolder.MouseEnter += new EventHandler(this.textBoxDestinationFolder_MouseEnter);
			componentResourceManager.ApplyResources(this.labelDestFolder, "labelDestFolder");
			this.labelDestFolder.Name = "labelDestFolder";
			componentResourceManager.ApplyResources(this.checkBoxSaveRawFile, "checkBoxSaveRawFile");
			this.checkBoxSaveRawFile.Name = "checkBoxSaveRawFile";
			this.checkBoxSaveRawFile.UseVisualStyleBackColor = true;
			this.checkBoxSaveRawFile.CheckedChanged += new EventHandler(this.checkBoxSaveRawFile_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxOverwriteDestination, "checkBoxOverwriteDestination");
			this.checkBoxOverwriteDestination.Name = "checkBoxOverwriteDestination";
			this.checkBoxOverwriteDestination.UseVisualStyleBackColor = true;
			this.checkBoxOverwriteDestination.CheckedChanged += new EventHandler(this.checkBoxOverwriteDestination_CheckedChanged);
			this.comboBoxDestinationFormat.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxDestinationFormat.FormattingEnabled = true;
			componentResourceManager.ApplyResources(this.comboBoxDestinationFormat, "comboBoxDestinationFormat");
			this.comboBoxDestinationFormat.Name = "comboBoxDestinationFormat";
			this.comboBoxDestinationFormat.SelectedIndexChanged += new EventHandler(this.comboBoxDestinationFormat_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.labelConvertTo, "labelConvertTo");
			this.labelConvertTo.Name = "labelConvertTo";
			componentResourceManager.ApplyResources(this.groupBoxAdvanced, "groupBoxAdvanced");
			this.groupBoxAdvanced.Controls.Add(this.textBoxMDF3SignalDelimiter);
			this.groupBoxAdvanced.Controls.Add(this.labelMDF3SignalDelimiter);
			this.groupBoxAdvanced.Controls.Add(this.comboBoxMDF3SignalFormat);
			this.groupBoxAdvanced.Controls.Add(this.labelMDF3SignalFormat);
			this.groupBoxAdvanced.Controls.Add(this.checkBoxRecoveryMode);
			this.groupBoxAdvanced.Controls.Add(this.checkBoxWriteRawValues);
			this.groupBoxAdvanced.Controls.Add(this.checkBoxCopyMediaFiles);
			this.groupBoxAdvanced.Controls.Add(this.checkBoxMinDigitsForTriggerIndex);
			this.groupBoxAdvanced.Controls.Add(this.numericUpDownMinDigitsForTriggerIndex);
			this.groupBoxAdvanced.Controls.Add(this.checkBoxCreateVsysvarFile);
			this.groupBoxAdvanced.Controls.Add(this.checkBoxSingleFile);
			this.groupBoxAdvanced.Controls.Add(this.checkBoxGermanExcelFormat);
			this.groupBoxAdvanced.Controls.Add(this.checkBoxRelativeTimestamps);
			this.groupBoxAdvanced.Controls.Add(this.checkBoxGlobalTimestamps);
			this.groupBoxAdvanced.Controls.Add(this.checkBoxHexadecimal);
			this.groupBoxAdvanced.Name = "groupBoxAdvanced";
			this.groupBoxAdvanced.TabStop = false;
			componentResourceManager.ApplyResources(this.textBoxMDF3SignalDelimiter, "textBoxMDF3SignalDelimiter");
			this.textBoxMDF3SignalDelimiter.Name = "textBoxMDF3SignalDelimiter";
			this.textBoxMDF3SignalDelimiter.TextChanged += new EventHandler(this.textBoxMDF3SignalDelimiter_TextChanged);
			componentResourceManager.ApplyResources(this.labelMDF3SignalDelimiter, "labelMDF3SignalDelimiter");
			this.labelMDF3SignalDelimiter.Name = "labelMDF3SignalDelimiter";
			this.comboBoxMDF3SignalFormat.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxMDF3SignalFormat.FormattingEnabled = true;
			this.comboBoxMDF3SignalFormat.Items.AddRange(new object[]
			{
				componentResourceManager.GetString("comboBoxMDF3SignalFormat.Items"),
				componentResourceManager.GetString("comboBoxMDF3SignalFormat.Items1"),
				componentResourceManager.GetString("comboBoxMDF3SignalFormat.Items2")
			});
			componentResourceManager.ApplyResources(this.comboBoxMDF3SignalFormat, "comboBoxMDF3SignalFormat");
			this.comboBoxMDF3SignalFormat.Name = "comboBoxMDF3SignalFormat";
			this.comboBoxMDF3SignalFormat.SelectedIndexChanged += new EventHandler(this.comboBoxMDF3SignalFormat_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.labelMDF3SignalFormat, "labelMDF3SignalFormat");
			this.labelMDF3SignalFormat.Name = "labelMDF3SignalFormat";
			componentResourceManager.ApplyResources(this.checkBoxRecoveryMode, "checkBoxRecoveryMode");
			this.checkBoxRecoveryMode.Name = "checkBoxRecoveryMode";
			this.checkBoxRecoveryMode.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.checkBoxWriteRawValues, "checkBoxWriteRawValues");
			this.checkBoxWriteRawValues.Name = "checkBoxWriteRawValues";
			this.checkBoxWriteRawValues.UseVisualStyleBackColor = true;
			this.checkBoxWriteRawValues.CheckedChanged += new EventHandler(this.checkBoxWriteRawValues_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxCopyMediaFiles, "checkBoxCopyMediaFiles");
			this.checkBoxCopyMediaFiles.Name = "checkBoxCopyMediaFiles";
			this.checkBoxCopyMediaFiles.UseVisualStyleBackColor = true;
			this.checkBoxCopyMediaFiles.CheckedChanged += new EventHandler(this.checkBoxCopyMediaFiles_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxMinDigitsForTriggerIndex, "checkBoxMinDigitsForTriggerIndex");
			this.checkBoxMinDigitsForTriggerIndex.Name = "checkBoxMinDigitsForTriggerIndex";
			this.checkBoxMinDigitsForTriggerIndex.UseVisualStyleBackColor = true;
			this.checkBoxMinDigitsForTriggerIndex.CheckedChanged += new EventHandler(this.checkBoxMinDigitsForTriggerIndex_CheckedChanged);
			componentResourceManager.ApplyResources(this.numericUpDownMinDigitsForTriggerIndex, "numericUpDownMinDigitsForTriggerIndex");
			NumericUpDown arg_C3B_0 = this.numericUpDownMinDigitsForTriggerIndex;
			int[] array = new int[4];
			array[0] = 15;
			arg_C3B_0.Maximum = new decimal(array);
			this.numericUpDownMinDigitsForTriggerIndex.Name = "numericUpDownMinDigitsForTriggerIndex";
			this.numericUpDownMinDigitsForTriggerIndex.ValueChanged += new EventHandler(this.numericUpDownMinDigitsForTriggerIndex_ValueChanged);
			componentResourceManager.ApplyResources(this.checkBoxCreateVsysvarFile, "checkBoxCreateVsysvarFile");
			this.checkBoxCreateVsysvarFile.Name = "checkBoxCreateVsysvarFile";
			this.checkBoxCreateVsysvarFile.UseVisualStyleBackColor = true;
			this.checkBoxCreateVsysvarFile.CheckedChanged += new EventHandler(this.checkBoxCreateVsysvarFile_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxSingleFile, "checkBoxSingleFile");
			this.checkBoxSingleFile.Name = "checkBoxSingleFile";
			this.checkBoxSingleFile.UseVisualStyleBackColor = true;
			this.checkBoxSingleFile.CheckedChanged += new EventHandler(this.checkBoxSingleFile_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxGermanExcelFormat, "checkBoxGermanExcelFormat");
			this.checkBoxGermanExcelFormat.Name = "checkBoxGermanExcelFormat";
			this.checkBoxGermanExcelFormat.UseVisualStyleBackColor = true;
			this.checkBoxGermanExcelFormat.CheckedChanged += new EventHandler(this.checkBoxGermanExcelFormat_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxRelativeTimestamps, "checkBoxRelativeTimestamps");
			this.checkBoxRelativeTimestamps.Name = "checkBoxRelativeTimestamps";
			this.checkBoxRelativeTimestamps.UseVisualStyleBackColor = true;
			this.checkBoxRelativeTimestamps.CheckedChanged += new EventHandler(this.checkBoxRelativeTimestamps_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxGlobalTimestamps, "checkBoxGlobalTimestamps");
			this.checkBoxGlobalTimestamps.Name = "checkBoxGlobalTimestamps";
			this.checkBoxGlobalTimestamps.UseVisualStyleBackColor = true;
			this.checkBoxGlobalTimestamps.CheckedChanged += new EventHandler(this.checkBoxGlobalTimestamps_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxHexadecimal, "checkBoxHexadecimal");
			this.checkBoxHexadecimal.Name = "checkBoxHexadecimal";
			this.checkBoxHexadecimal.UseVisualStyleBackColor = true;
			this.checkBoxHexadecimal.CheckedChanged += new EventHandler(this.checkBoxHexadecimal_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxUseDateTimeFromMeasurementStart, "checkBoxUseDateTimeFromMeasurementStart");
			this.checkBoxUseDateTimeFromMeasurementStart.Name = "checkBoxUseDateTimeFromMeasurementStart";
			this.checkBoxUseDateTimeFromMeasurementStart.UseVisualStyleBackColor = true;
			this.checkBoxUseDateTimeFromMeasurementStart.CheckedChanged += new EventHandler(this.checkBoxUseDateTimeFromMeasurementStart_CheckedChanged);
			this.errorProvider.SetIconAlignment(this.textBoxFileFractionSize, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxFileFractionSize.IconAlignment"));
			componentResourceManager.ApplyResources(this.textBoxFileFractionSize, "textBoxFileFractionSize");
			this.textBoxFileFractionSize.Name = "textBoxFileFractionSize";
			this.textBoxFileFractionSize.EnabledChanged += new EventHandler(this.textBoxFileFractionSize_EnabledChanged);
			this.textBoxFileFractionSize.Validating += new CancelEventHandler(this.textBoxFileFractionSize_Validating);
			componentResourceManager.ApplyResources(this.checkBoxSplitFileBySize, "checkBoxSplitFileBySize");
			this.checkBoxSplitFileBySize.Name = "checkBoxSplitFileBySize";
			this.checkBoxSplitFileBySize.UseVisualStyleBackColor = true;
			this.checkBoxSplitFileBySize.CheckedChanged += new EventHandler(this.checkBoxSplitByFileSize_CheckedChanged);
			componentResourceManager.ApplyResources(this.labelFractionMB, "labelFractionMB");
			this.labelFractionMB.Name = "labelFractionMB";
			componentResourceManager.ApplyResources(this.checkBoxSplitByLoc, "checkBoxSplitByLoc");
			this.checkBoxSplitByLoc.Name = "checkBoxSplitByLoc";
			this.checkBoxSplitByLoc.UseVisualStyleBackColor = true;
			this.checkBoxSplitByLoc.CheckedChanged += new EventHandler(this.CheckBoxSplitByLoc_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxSuppressBufferConcat, "checkBoxSuppressBufferConcat");
			this.checkBoxSuppressBufferConcat.Name = "checkBoxSuppressBufferConcat";
			this.checkBoxSuppressBufferConcat.UseVisualStyleBackColor = true;
			this.checkBoxSuppressBufferConcat.CheckedChanged += new EventHandler(this.checkBoxSuppressBufferConcat_CheckedChanged);
			this.errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProvider.ContainerControl = this;
			this.errorProvider.SetIconAlignment(this.textBoxFileFractionTime, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxFileFractionTime.IconAlignment"));
			componentResourceManager.ApplyResources(this.textBoxFileFractionTime, "textBoxFileFractionTime");
			this.textBoxFileFractionTime.Name = "textBoxFileFractionTime";
			this.textBoxFileFractionTime.EnabledChanged += new EventHandler(this.TextBoxFileFractionTime_EnabledChanged);
			this.textBoxFileFractionTime.Validating += new CancelEventHandler(this.TextBoxFileFractionTime_Validating);
			componentResourceManager.ApplyResources(this.groupBox1, "groupBox1");
			this.groupBox1.Controls.Add(this.labelFractionMB);
			this.groupBox1.Controls.Add(this.textBoxFileFractionSize);
			this.groupBox1.Controls.Add(this.checkBoxSplitFileBySize);
			this.groupBox1.Controls.Add(this.checkBoxUseDateTimeFromMeasurementStart);
			this.groupBox1.Controls.Add(this.comboBoxTimeBase);
			this.groupBox1.Controls.Add(this.textBoxFileFractionTime);
			this.groupBox1.Controls.Add(this.checkBoxAbsoluteTimestamps);
			this.groupBox1.Controls.Add(this.checkBoxSplitFileByTime);
			this.groupBox1.Controls.Add(this.checkBoxSplitByLoc);
			this.groupBox1.Controls.Add(this.checkBoxSuppressBufferConcat);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.TabStop = false;
			this.comboBoxTimeBase.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxTimeBase.FormattingEnabled = true;
			componentResourceManager.ApplyResources(this.comboBoxTimeBase, "comboBoxTimeBase");
			this.comboBoxTimeBase.Name = "comboBoxTimeBase";
			this.comboBoxTimeBase.SelectedIndexChanged += new EventHandler(this.ComboBoxTimeBase_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.checkBoxAbsoluteTimestamps, "checkBoxAbsoluteTimestamps");
			this.checkBoxAbsoluteTimestamps.Name = "checkBoxAbsoluteTimestamps";
			this.checkBoxAbsoluteTimestamps.UseVisualStyleBackColor = true;
			this.checkBoxAbsoluteTimestamps.CheckedChanged += new EventHandler(this.CheckBoxAbsoluteTimestamps_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxSplitFileByTime, "checkBoxSplitFileByTime");
			this.checkBoxSplitFileByTime.Name = "checkBoxSplitFileByTime";
			this.checkBoxSplitFileByTime.UseVisualStyleBackColor = true;
			this.checkBoxSplitFileByTime.CheckedChanged += new EventHandler(this.CheckBoxSplitFileByTime_CheckedChanged);
			base.AutoScaleMode = AutoScaleMode.Inherit;
			base.Controls.Add(this.groupBoxGeneral);
			base.Controls.Add(this.groupBox1);
			base.Controls.Add(this.groupBoxAdvanced);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "FileConversionSettings";
			base.VisibleChanged += new EventHandler(this.FileConversionSettings_VisibleChanged);
			this.groupBoxGeneral.ResumeLayout(false);
			this.groupBoxGeneral.PerformLayout();
			this.groupBoxAdvanced.ResumeLayout(false);
			this.groupBoxAdvanced.PerformLayout();
			((ISupportInitialize)this.numericUpDownMinDigitsForTriggerIndex).EndInit();
			((ISupportInitialize)this.errorProvider).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
