using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Vector.UtilityFunctions;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.BusinessLogic.Conversion;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.GUI.DatabasesPage;
using Vector.VLConfig.GUI.Helpers;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.Common.FileManager
{
	public class FileConversion : UserControl
	{
		public delegate void FileConversionParametersChangedHandler(object sender, EventArgs e);

		public delegate void StartConversionHandler(object sender, EventArgs e);

		private ILoggerSpecifics loggerSpecifics;

		private FileConversionParameters parameters;

		private bool isInitControls;

		private ContextMenuStrip insertMacroContextMenuStrip;

		private bool isCLFConversion;

		private IFileConversionParametersClient fileConversionParametersClient;

		private bool showExportDatabases;

		private IContainer components;

		private GroupBox groupBoxFileConversion;

		private TabControl tabControlSettings;

		private TabPage tabPageGeneralSettings;

		private TabPage tabPageAdvancedSettings;

		private TextBox textBoxDestinationFolder;

		private Label labelDestFolder;

		private CheckBox checkBoxSaveRawFile;

		private CheckBox checkBoxOverwriteDestination;

		private CheckBox checkBoxDeleteSourceFiles;

		private ComboBox comboBoxDestinationFormat;

		private Label labelConvertTo;

		private CheckBox checkBoxSingleFile;

		private CheckBox checkBoxHexadecimal;

		private CheckBox checkBoxRecoveryMode;

		private CheckBox checkBoxJumpOverSleepTime;

		private CheckBox checkBoxGermanExcelFormat;

		private CheckBox checkBoxRelativeTimestamps;

		private CheckBox checkBoxGlobalTimestamps;

		private Button buttonSameAsSource;

		private ToolTip toolTip;

		private TextBox textBoxPrefix;

		private ErrorProvider errorProvider;

		private ComboBox comboBoxFilenameFormat;

		private Label labelPrefix;

		private Label labelFilenameFormat;

		private Label labelNewName;

		private SplitButton splitButtonInsertMacro;

		private Label labelCustomNameExample;

		private SplitButton splitButtonSelectDestinationFolder;

		private CheckBox checkBoxCreateVsysvarFile;

		private TabPage tabPageSplitSettings;

		private TextBox textBoxFileFractionSize;

		private CheckBox checkBoxSplitFileBySize;

		private Label labelFractionMB;

		private CheckBox checkBoxSplitByLoc;

		private CheckBox checkBoxSuppressBufferConcat;

		private CheckBox checkBoxAbsoluteTimestamps;

		private CheckBox checkBoxSplitFileByTime;

		private ComboBox comboBoxTimeBase;

		private TextBox textBoxFileFractionTime;

		private TabPage tabPageChannelMapping;

		private ChannelMapping channelMapping;

		private SplitButton splitButtonConvert;

		private CheckBox checkBoxUseDateTimeFromMeasurementStart;

		private ComboBox comboBoxDestinationFormatVersion;

		private Label labelVersion;

		private LinkLabel linkLabelConversionInfo;

		private CheckBox checkBoxMinDigitsForTriggerIndex;

		private NumericUpDownEx numericUpDownMinDigitsForTriggerIndex;

		private ComboBox comboBoxDestinationFormatExtension;

		private Label labelExtension;

		private CheckBox checkBoxCopyMediaFiles;

		private TabPage tabPageExportDatabases;

		private ExportDatabases exportDatabases;

		private ImageList imageListErrorIcons;

		private CheckBox checkBoxWriteRawValues;

		private ComboBox comboBoxMDF3SignalFormat;

		private Label labelMDF3SignalFormat;

		private TextBox textBoxMDF3SignalDelimiter;

		private Label labelMDF3SignalDelimiter;

		public event FileConversion.FileConversionParametersChangedHandler ParametersChanged;

		public event FileConversion.StartConversionHandler StartConversion;

		public FileConversionParameters FileConversionParameters
		{
			get
			{
				return this.parameters;
			}
			set
			{
				this.parameters = value;
				this.channelMapping.FileConversionParameters = this.parameters;
				if (this.loggerSpecifics != null)
				{
					this.ApplyParametersToControls();
				}
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
				this.channelMapping.LoggerSpecifics = value;
				if (this.loggerSpecifics != null && this.loggerSpecifics.Type != value.Type)
				{
					this.loggerSpecifics = value;
					this.InitLoggerSpecificsDependencies();
					this.FillDestinationFormatComboBox(this.isCLFConversion);
					this.FillFilenameFormatComboBox();
					this.EnableControlsDependingOnDestinationFormat();
					this.ApplyParametersToControls();
					this.InitializeSplitButtonInsertMacro();
					this.ValidateFilenameString();
				}
			}
		}

		public LinkLabel ConversionInfo
		{
			get
			{
				return this.linkLabelConversionInfo;
			}
		}

		public bool IsConversionEnabled
		{
			get
			{
				return this.splitButtonConvert.Enabled;
			}
			set
			{
				this.splitButtonConvert.Enabled = value;
			}
		}

		public string SourceFolder
		{
			get;
			set;
		}

		public ExportDatabases ExportDatabases
		{
			get
			{
				return this.exportDatabases;
			}
		}

		public IConfigurationManagerService ConfigurationManagerService
		{
			get
			{
				return this.exportDatabases.ConfigurationManagerService;
			}
			set
			{
				this.exportDatabases.ConfigurationManagerService = value;
			}
		}

		public AnalysisPackageParameters AnalysisPackageParameters
		{
			get
			{
				return this.exportDatabases.AnalysisPackageParameters;
			}
			set
			{
				this.exportDatabases.AnalysisPackageParameters = value;
			}
		}

		public FileConversion()
		{
			this.InitializeComponent();
			this.isInitControls = false;
			this.splitButtonConvert.AutoSize = false;
			this.splitButtonInsertMacro.AutoSize = false;
			this.splitButtonSelectDestinationFolder.AutoSize = false;
			this.splitButtonSelectDestinationFolder.SplitMenu = new ContextMenu();
			this.UpdateSplitButtonSelectDestinationFolder();
			this.exportDatabases.Validating += new EventHandler(this.OnExportDatabaseValidating);
			this.imageListErrorIcons.Images.Add(Resources.IconError.ToBitmap());
			this.imageListErrorIcons.Images.Add(Resources.ImageWarning);
			this.imageListErrorIcons.Images.Add(Resources.ImageOK);
			this.imageListErrorIcons.Images.Add(Resources.ImageOKLock);
		}

		public void Init(ILoggerSpecifics loggerSpecs, bool isCLFConversionMode, IFileConversionParametersClient fileConversionParametersClient)
		{
			this.loggerSpecifics = loggerSpecs;
			this.channelMapping.LoggerSpecifics = this.loggerSpecifics;
			this.isCLFConversion = isCLFConversionMode;
			this.showExportDatabases = true;
			this.isInitControls = true;
			this.InitLoggerSpecificsDependencies();
			this.InitializeSplitButtonInsertMacro();
			this.InitSplitButtonConvert(fileConversionParametersClient);
			this.FillDestinationFormatComboBox(isCLFConversionMode);
			this.FillFilenameFormatComboBox();
			this.FillTimeBaseComboBox();
			this.channelMapping.IsCLFConversionMode = isCLFConversionMode;
			this.checkBoxSuppressBufferConcat.Visible = !isCLFConversionMode;
			this.checkBoxDeleteSourceFiles.Visible = (!isCLFConversionMode && !this.loggerSpecifics.FileConversion.HasSelectableLogFiles);
			this.buttonSameAsSource.Visible = isCLFConversionMode;
			this.checkBoxSaveRawFile.Visible = !isCLFConversionMode;
			if (!isCLFConversionMode)
			{
				this.checkBoxSaveRawFile.Checked = true;
				this.checkBoxSaveRawFile.Enabled = true;
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
			if (this.loggerSpecifics.FileConversion.HasAdvancedSettings && !this.tabControlSettings.TabPages.Contains(this.tabPageAdvancedSettings))
			{
				this.tabControlSettings.TabPages.Add(this.tabPageAdvancedSettings);
			}
			else if (!this.loggerSpecifics.FileConversion.HasAdvancedSettings && this.tabControlSettings.TabPages.Contains(this.tabPageAdvancedSettings))
			{
				this.tabControlSettings.TabPages.Remove(this.tabPageAdvancedSettings);
			}
			if (this.loggerSpecifics.FileConversion.HasSplittingOptions && !this.tabControlSettings.TabPages.Contains(this.tabPageSplitSettings))
			{
				this.tabControlSettings.TabPages.Add(this.tabPageSplitSettings);
			}
			else if (!this.loggerSpecifics.FileConversion.HasSplittingOptions && this.tabControlSettings.TabPages.Contains(this.tabPageSplitSettings))
			{
				this.tabControlSettings.TabPages.Remove(this.tabPageSplitSettings);
			}
			if (this.loggerSpecifics.FileConversion.HasChannelMapping && !this.tabControlSettings.TabPages.Contains(this.tabPageChannelMapping))
			{
				this.tabControlSettings.TabPages.Add(this.tabPageChannelMapping);
			}
			else if (!this.loggerSpecifics.FileConversion.HasChannelMapping && this.tabControlSettings.TabPages.Contains(this.tabPageChannelMapping))
			{
				this.tabControlSettings.TabPages.Remove(this.tabPageChannelMapping);
			}
			if (this.loggerSpecifics.FileConversion.HasExportDatabases && !this.tabControlSettings.TabPages.Contains(this.tabPageExportDatabases) && this.showExportDatabases)
			{
				this.tabControlSettings.TabPages.Add(this.tabPageExportDatabases);
			}
			else if (!this.loggerSpecifics.FileConversion.HasExportDatabases && this.tabControlSettings.TabPages.Contains(this.tabPageExportDatabases))
			{
				this.tabControlSettings.TabPages.Remove(this.tabPageExportDatabases);
			}
			this.checkBoxCopyMediaFiles.Visible = ((this.loggerSpecifics.Recording.IsVoCANSupported || this.loggerSpecifics.Recording.IsCameraSupported) && !this.isCLFConversion);
		}

		private void InitializeSplitButtonInsertMacro()
		{
			this.splitButtonInsertMacro.ShowSplit = true;
			this.splitButtonInsertMacro.ShowSplitAlways = true;
			this.insertMacroContextMenuStrip = new ContextMenuStrip();
			if (!this.isCLFConversion)
			{
				foreach (string current in FileConversionHelper.GetBasicMacroNames(this.loggerSpecifics))
				{
					this.insertMacroContextMenuStrip.Items.Add(current);
				}
				this.insertMacroContextMenuStrip.Items.Add(new ToolStripSeparator());
				using (List<string>.Enumerator enumerator2 = FileConversionHelper.GetSuperMacroNames().GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						string current2 = enumerator2.Current;
						this.insertMacroContextMenuStrip.Items.Add(current2);
					}
					goto IL_17E;
				}
			}
			foreach (string current3 in FileConversionHelper.GetBasicCLFExportMacroNames())
			{
				this.insertMacroContextMenuStrip.Items.Add(current3);
			}
			this.insertMacroContextMenuStrip.Items.Add(new ToolStripSeparator());
			foreach (string current4 in FileConversionHelper.GetExtraCLFExportMacroNames())
			{
				this.insertMacroContextMenuStrip.Items.Add(current4);
			}
			IL_17E:
			this.splitButtonInsertMacro.ContextMenuStrip = this.insertMacroContextMenuStrip;
			this.insertMacroContextMenuStrip.ItemClicked += new ToolStripItemClickedEventHandler(this.insertMacroContextMenuStrip_ItemClicked);
		}

		private void InitSplitButtonConvert(IFileConversionParametersClient client)
		{
			this.fileConversionParametersClient = client;
			this.splitButtonConvert.SplitMenuStrip = new ContextMenuStrip();
			this.splitButtonConvert.SplitMenuStrip.Opening += new CancelEventHandler(this.SplitMenuStripConvert_Opening);
			this.splitButtonConvert.SplitMenuStrip.ItemClicked += new ToolStripItemClickedEventHandler(this.SplitMenuStripConvert_ItemClicked);
			FileConversionProfileManager.InitDropDownItems(this.splitButtonConvert.SplitMenuStrip.Items, client);
		}

		private void Raise_ParametersChanged(object sender, EventArgs e)
		{
			if (this.ParametersChanged != null)
			{
				this.ParametersChanged(sender, e);
			}
		}

		private void Raise_StartConversion(object sender, EventArgs e)
		{
			if (this.StartConversion != null)
			{
				this.StartConversion(sender, e);
			}
		}

		public void RefreshStatus()
		{
			this.DisplayFreeCapacityOnDestinationVolume();
			if (this.loggerSpecifics != null && this.checkBoxCreateVsysvarFile != null)
			{
				this.checkBoxCreateVsysvarFile.Visible = (!this.isCLFConversion && this.loggerSpecifics.Type != LoggerType.GL1000 && this.loggerSpecifics.Type != LoggerType.GL1020FTE);
			}
			this.exportDatabases.ValidateInput();
		}

		public void ShowExportDatabaseTab(bool show)
		{
			this.showExportDatabases = show;
			this.exportDatabases.Visible = this.showExportDatabases;
			if (!show)
			{
				this.tabControlSettings.TabPages.Remove(this.tabPageExportDatabases);
			}
		}

		public void EnableParamaterDeleteSourceFilesWhenDone(bool enable)
		{
			enable &= !this.loggerSpecifics.FileConversion.HasSelectableLogFiles;
			this.checkBoxDeleteSourceFiles.Visible = enable;
			this.FileConversionParameters.IsDeletionOfSourceFilesAllowed = enable;
		}

		public void SetExportDatabaseConfiguration(ExportDatabaseConfiguration config, ExportDatabases exportDatabases)
		{
			this.FileConversionParameters.ExportDatabaseConfiguration = config;
			this.exportDatabases = exportDatabases;
		}

		private void splitButtonSelectDestinationFolder_Enter(object sender, EventArgs e)
		{
			this.UpdateSplitButtonSelectDestinationFolder();
		}

		private void FileConversion_VisibleChanged(object sender, EventArgs e)
		{
			this.UpdateSplitButtonSelectDestinationFolder();
		}

		private void splitButtonSelectDestinationFolder_Click(object sender, EventArgs e)
		{
			using (BrowseFolderDialog browseFolderDialog = new BrowseFolderDialog())
			{
				browseFolderDialog.SelectedPath = this.parameters.DestinationFolder;
				browseFolderDialog.Title = Resources.SelectDestFolder;
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
				this.UpdateTabErrorIcons();
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

		private void checkBoxDeleteSourceFiles_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.parameters.DeleteSourceFilesWhenDone = this.checkBoxDeleteSourceFiles.Checked;
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

		private void checkBoxJumpOverSleepTime_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.parameters.JumpOverSleepTime = this.checkBoxJumpOverSleepTime.Checked;
			this.Raise_ParametersChanged(this, EventArgs.Empty);
		}

		private void checkBoxRecoveryMode_CheckedChanged(object sender, EventArgs e)
		{
			if (this.isInitControls)
			{
				return;
			}
			this.parameters.RecoveryMode = this.checkBoxRecoveryMode.Checked;
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

		private void CheckBoxCreateVsysvarFile_CheckedChanged(object sender, EventArgs e)
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

		private void buttonSameAsSource_Click(object sender, EventArgs e)
		{
			if (this.SourceFolder == null)
			{
				return;
			}
			if (this.textBoxDestinationFolder.Text != this.SourceFolder)
			{
				this.textBoxDestinationFolder.Text = this.SourceFolder;
				this.FileConversionParameters.DestinationFolder = this.SourceFolder;
				this.Raise_ParametersChanged(this, EventArgs.Empty);
			}
			GlobalOptionsManager.AddMostRecentFolder(GlobalOptionsManager.ListSelector.DestinationFolders, this.SourceFolder);
			this.UpdateSplitButtonSelectDestinationFolder();
		}

		private void textBoxDestinationFolder_MouseEnter(object sender, EventArgs e)
		{
			this.toolTip.SetToolTip(this.textBoxDestinationFolder, this.textBoxDestinationFolder.Text);
		}

		private void textBoxPrefix_MouseEnter(object sender, EventArgs e)
		{
			this.toolTip.SetToolTip(this.textBoxPrefix, this.textBoxPrefix.Text);
		}

		private void textBoxDestinationFolder_DoubleClick(object sender, EventArgs e)
		{
			FileSystemServices.LaunchDirectoryBrowser(this.textBoxDestinationFolder.Text);
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

		private void SplitMenuStripConvert_Opening(object sender, CancelEventArgs e)
		{
			FileConversionProfileManager.InitDropDownItems(this.splitButtonConvert.SplitMenuStrip.Items, this.fileConversionParametersClient);
		}

		private void SplitMenuStripConvert_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
		{
			if (FileConversionProfileManager.OnDropDownItemClicked(e, this.fileConversionParametersClient, true))
			{
				this.SplitButtonConvert_Click(this, EventArgs.Empty);
			}
		}

		private void SplitButtonConvert_Click(object sender, EventArgs e)
		{
			if ((this.parameters.FilenameFormat != FileConversionFilenameFormat.UseOriginalName && !this.ValidateFilenameString()) || (this.parameters.SplitFilesBySize && !this.ValidateFileFractionSizeString()) || (this.parameters.SplitFilesByTime && !this.ValidateFileFractionTimeString()) || this.channelMapping.HasError)
			{
				InformMessageBox.Error(Resources.ErrorInvalidInputInDialog);
				return;
			}
			this.Raise_StartConversion(this, EventArgs.Empty);
		}

		private void linkLabelConversionInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			string text = e.Link.LinkData.ToString();
			if (Directory.Exists(text))
			{
				FileSystemServices.LaunchDirectoryBrowser(text);
				return;
			}
			InformMessageBox.Error(string.Format(Resources.ErrorFolderNotFound, text));
			this.linkLabelConversionInfo.Links.Clear();
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
			this.splitButtonSelectDestinationFolder.ShowSplit = false;
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
			}
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

		private void FillFilenameFormatComboBox()
		{
			this.comboBoxFilenameFormat.SelectedIndexChanged -= new EventHandler(this.comboBoxFilenameFormat_SelectedIndexChanged);
			this.comboBoxFilenameFormat.Items.Clear();
			foreach (FileConversionFilenameFormat current in this.loggerSpecifics.FileConversion.FilenameFormats)
			{
				this.comboBoxFilenameFormat.Items.Add(FileConversionHelper.FileConversionFilenameFormat2String(current));
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
			this.checkBoxDeleteSourceFiles.Checked = this.parameters.DeleteSourceFilesWhenDone;
			this.checkBoxOverwriteDestination.Checked = this.parameters.OverwriteDestinationFiles;
			this.checkBoxSaveRawFile.Checked = this.parameters.SaveRawFile;
			this.checkBoxHexadecimal.Checked = this.parameters.HexadecimalNotation;
			this.checkBoxSingleFile.Checked = this.parameters.SingleFile;
			this.checkBoxGlobalTimestamps.Checked = this.parameters.GlobalTimestamps;
			this.checkBoxRelativeTimestamps.Checked = this.parameters.RelativeTimestamps;
			this.checkBoxGermanExcelFormat.Checked = this.parameters.GermanMSExcelFormat;
			this.checkBoxJumpOverSleepTime.Checked = this.parameters.JumpOverSleepTime;
			this.checkBoxRecoveryMode.Checked = this.parameters.RecoveryMode;
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
			this.checkBoxMinDigitsForTriggerIndex.Checked = this.parameters.UseMinimumDigitsForTriggerIndex;
			this.numericUpDownMinDigitsForTriggerIndex.Enabled = this.checkBoxMinDigitsForTriggerIndex.Checked;
			this.numericUpDownMinDigitsForTriggerIndex.Value = this.parameters.MinimumDigitsForTriggerIndex;
			this.checkBoxWriteRawValues.Checked = this.parameters.WriteRawValues;
			this.comboBoxMDF3SignalFormat.SelectedIndex = (int)this.parameters.MDF3SignalFormat;
			this.textBoxMDF3SignalDelimiter.Text = this.parameters.MDF3SignalFormatDelimiter;
			if (!this.loggerSpecifics.FileConversion.FilenameFormats.Contains(this.parameters.FilenameFormat))
			{
				this.parameters.FilenameFormat = FileConversionFilenameFormat.UseOriginalName;
			}
			this.ShowControlsForFilenameFormat();
			if (this.parameters.FilenameFormat == FileConversionFilenameFormat.AddPrefix)
			{
				this.textBoxPrefix.Text = this.parameters.Prefix;
				this.ValidateFilenameString();
			}
			else if (this.parameters.FilenameFormat == FileConversionFilenameFormat.UseCustomName)
			{
				this.textBoxPrefix.Text = this.parameters.CustomFilename;
				this.ValidateFilenameString();
			}
			else
			{
				this.textBoxPrefix.Text = "";
			}
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
			if (this.parameters.FilenameFormat == FileConversionFilenameFormat.UseCustomName && !FileConversionHelper.ValidateMacrosInFileNamePattern(ref this.textBoxPrefix, ref this.errorProvider, this.isCLFConversion, false, this.loggerSpecifics, out arg))
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
			this.parameters.JumpOverSleepTime = FileConversionHelper.EnableAndSetByFormat(this.checkBoxJumpOverSleepTime, this.parameters.DestinationFormat, EnumFileConversionParameter.JumpOverSleepTime);
			this.parameters.RecoveryMode = FileConversionHelper.EnableAndSetByFormat(this.checkBoxRecoveryMode, this.parameters.DestinationFormat, EnumFileConversionParameter.RecoveryMode);
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
			this.numericUpDownMinDigitsForTriggerIndex.Enabled = (this.checkBoxMinDigitsForTriggerIndex.Enabled && this.checkBoxMinDigitsForTriggerIndex.Checked);
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
			if (this.parameters != null && !string.IsNullOrEmpty(this.parameters.DestinationFolder))
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
			if (text != FileConversionHelper.GetConfiguredDestinationFormatVersion(this.parameters))
			{
				this.parameters.DestinationFormatVersionStrings[(int)this.parameters.DestinationFormat] = text;
				this.Raise_ParametersChanged(this, new FileConversionParametersChangedEventArgs(FileConversionParametersChangedEventArgs.Parameter.DestinationFormatVersion));
				this.UpdateExtensionComboBoxDependingOnDestinationFormatAndVersion();
				this.EnableControlsDependingOnDestinationFormat();
				this.UpdateTabErrorIcons();
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

		private void tabControlSettings_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (this.tabControlSettings.SelectedTab == this.tabPageExportDatabases)
			{
				this.ExportDatabases.ValidateInput();
			}
		}

		private void OnExportDatabaseValidating(object sender, EventArgs e)
		{
			this.UpdateTabErrorIcons();
		}

		public void UpdateTabErrorIcons()
		{
			if (this.exportDatabases.HasValidationError())
			{
				this.tabPageExportDatabases.ImageIndex = 1;
				return;
			}
			if (this.FileConversionParameters != null && this.FileConversionParameters.ExportDatabaseConfiguration != null)
			{
				string analysisPackagePath = this.FileConversionParameters.ExportDatabaseConfiguration.GetAnalysisPackagePath();
				if (this.FileConversionParameters.ExportDatabaseConfiguration.Mode == ExportDatabaseConfiguration.DBSelectionMode.FromAnalysisPackage && !string.IsNullOrEmpty(analysisPackagePath))
				{
					if (AnalysisPackage.IsPasswordProtected(analysisPackagePath))
					{
						this.tabPageExportDatabases.ImageIndex = 3;
						return;
					}
					this.tabPageExportDatabases.ImageIndex = 2;
					return;
				}
				else
				{
					this.tabPageExportDatabases.ImageIndex = -1;
				}
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FileConversion));
			this.groupBoxFileConversion = new GroupBox();
			this.linkLabelConversionInfo = new LinkLabel();
			this.splitButtonConvert = new SplitButton();
			this.tabControlSettings = new TabControl();
			this.tabPageGeneralSettings = new TabPage();
			this.comboBoxDestinationFormatExtension = new ComboBox();
			this.labelExtension = new Label();
			this.comboBoxDestinationFormatVersion = new ComboBox();
			this.labelVersion = new Label();
			this.splitButtonSelectDestinationFolder = new SplitButton();
			this.labelCustomNameExample = new Label();
			this.splitButtonInsertMacro = new SplitButton();
			this.labelNewName = new Label();
			this.labelPrefix = new Label();
			this.labelFilenameFormat = new Label();
			this.comboBoxFilenameFormat = new ComboBox();
			this.textBoxPrefix = new TextBox();
			this.buttonSameAsSource = new Button();
			this.checkBoxSaveRawFile = new CheckBox();
			this.checkBoxOverwriteDestination = new CheckBox();
			this.checkBoxDeleteSourceFiles = new CheckBox();
			this.comboBoxDestinationFormat = new ComboBox();
			this.labelConvertTo = new Label();
			this.textBoxDestinationFolder = new TextBox();
			this.labelDestFolder = new Label();
			this.tabPageAdvancedSettings = new TabPage();
			this.textBoxMDF3SignalDelimiter = new TextBox();
			this.labelMDF3SignalDelimiter = new Label();
			this.comboBoxMDF3SignalFormat = new ComboBox();
			this.labelMDF3SignalFormat = new Label();
			this.checkBoxWriteRawValues = new CheckBox();
			this.checkBoxCopyMediaFiles = new CheckBox();
			this.checkBoxMinDigitsForTriggerIndex = new CheckBox();
			this.checkBoxCreateVsysvarFile = new CheckBox();
			this.checkBoxRecoveryMode = new CheckBox();
			this.checkBoxJumpOverSleepTime = new CheckBox();
			this.checkBoxGermanExcelFormat = new CheckBox();
			this.checkBoxRelativeTimestamps = new CheckBox();
			this.checkBoxGlobalTimestamps = new CheckBox();
			this.checkBoxSingleFile = new CheckBox();
			this.checkBoxHexadecimal = new CheckBox();
			this.numericUpDownMinDigitsForTriggerIndex = new NumericUpDownEx();
			this.tabPageSplitSettings = new TabPage();
			this.checkBoxUseDateTimeFromMeasurementStart = new CheckBox();
			this.comboBoxTimeBase = new ComboBox();
			this.textBoxFileFractionTime = new TextBox();
			this.checkBoxAbsoluteTimestamps = new CheckBox();
			this.checkBoxSplitFileByTime = new CheckBox();
			this.textBoxFileFractionSize = new TextBox();
			this.checkBoxSplitFileBySize = new CheckBox();
			this.labelFractionMB = new Label();
			this.checkBoxSplitByLoc = new CheckBox();
			this.checkBoxSuppressBufferConcat = new CheckBox();
			this.tabPageChannelMapping = new TabPage();
			this.channelMapping = new ChannelMapping();
			this.tabPageExportDatabases = new TabPage();
			this.exportDatabases = new ExportDatabases();
			this.imageListErrorIcons = new ImageList(this.components);
			this.toolTip = new ToolTip(this.components);
			this.errorProvider = new ErrorProvider(this.components);
			this.groupBoxFileConversion.SuspendLayout();
			this.tabControlSettings.SuspendLayout();
			this.tabPageGeneralSettings.SuspendLayout();
			this.tabPageAdvancedSettings.SuspendLayout();
			((ISupportInitialize)this.numericUpDownMinDigitsForTriggerIndex).BeginInit();
			this.tabPageSplitSettings.SuspendLayout();
			this.tabPageChannelMapping.SuspendLayout();
			this.tabPageExportDatabases.SuspendLayout();
			((ISupportInitialize)this.errorProvider).BeginInit();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.groupBoxFileConversion, "groupBoxFileConversion");
			this.groupBoxFileConversion.Controls.Add(this.linkLabelConversionInfo);
			this.groupBoxFileConversion.Controls.Add(this.splitButtonConvert);
			this.groupBoxFileConversion.Controls.Add(this.tabControlSettings);
			this.groupBoxFileConversion.Name = "groupBoxFileConversion";
			this.groupBoxFileConversion.TabStop = false;
			componentResourceManager.ApplyResources(this.linkLabelConversionInfo, "linkLabelConversionInfo");
			this.linkLabelConversionInfo.Name = "linkLabelConversionInfo";
			this.linkLabelConversionInfo.TabStop = true;
			this.linkLabelConversionInfo.LinkClicked += new LinkLabelLinkClickedEventHandler(this.linkLabelConversionInfo_LinkClicked);
			componentResourceManager.ApplyResources(this.splitButtonConvert, "splitButtonConvert");
			this.splitButtonConvert.Name = "splitButtonConvert";
			this.splitButtonConvert.ShowSplitAlways = true;
			this.splitButtonConvert.UseVisualStyleBackColor = true;
			this.splitButtonConvert.Click += new EventHandler(this.SplitButtonConvert_Click);
			componentResourceManager.ApplyResources(this.tabControlSettings, "tabControlSettings");
			this.tabControlSettings.Controls.Add(this.tabPageGeneralSettings);
			this.tabControlSettings.Controls.Add(this.tabPageAdvancedSettings);
			this.tabControlSettings.Controls.Add(this.tabPageSplitSettings);
			this.tabControlSettings.Controls.Add(this.tabPageChannelMapping);
			this.tabControlSettings.Controls.Add(this.tabPageExportDatabases);
			this.tabControlSettings.ImageList = this.imageListErrorIcons;
			this.tabControlSettings.Name = "tabControlSettings";
			this.tabControlSettings.SelectedIndex = 0;
			this.tabControlSettings.SelectedIndexChanged += new EventHandler(this.tabControlSettings_SelectedIndexChanged);
			this.tabPageGeneralSettings.BackColor = SystemColors.Control;
			this.tabPageGeneralSettings.Controls.Add(this.comboBoxDestinationFormatExtension);
			this.tabPageGeneralSettings.Controls.Add(this.labelExtension);
			this.tabPageGeneralSettings.Controls.Add(this.comboBoxDestinationFormatVersion);
			this.tabPageGeneralSettings.Controls.Add(this.labelVersion);
			this.tabPageGeneralSettings.Controls.Add(this.splitButtonSelectDestinationFolder);
			this.tabPageGeneralSettings.Controls.Add(this.labelCustomNameExample);
			this.tabPageGeneralSettings.Controls.Add(this.splitButtonInsertMacro);
			this.tabPageGeneralSettings.Controls.Add(this.labelNewName);
			this.tabPageGeneralSettings.Controls.Add(this.labelPrefix);
			this.tabPageGeneralSettings.Controls.Add(this.labelFilenameFormat);
			this.tabPageGeneralSettings.Controls.Add(this.comboBoxFilenameFormat);
			this.tabPageGeneralSettings.Controls.Add(this.textBoxPrefix);
			this.tabPageGeneralSettings.Controls.Add(this.buttonSameAsSource);
			this.tabPageGeneralSettings.Controls.Add(this.checkBoxSaveRawFile);
			this.tabPageGeneralSettings.Controls.Add(this.checkBoxOverwriteDestination);
			this.tabPageGeneralSettings.Controls.Add(this.checkBoxDeleteSourceFiles);
			this.tabPageGeneralSettings.Controls.Add(this.comboBoxDestinationFormat);
			this.tabPageGeneralSettings.Controls.Add(this.labelConvertTo);
			this.tabPageGeneralSettings.Controls.Add(this.textBoxDestinationFolder);
			this.tabPageGeneralSettings.Controls.Add(this.labelDestFolder);
			componentResourceManager.ApplyResources(this.tabPageGeneralSettings, "tabPageGeneralSettings");
			this.tabPageGeneralSettings.Name = "tabPageGeneralSettings";
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
			this.splitButtonSelectDestinationFolder.Enter += new EventHandler(this.splitButtonSelectDestinationFolder_Enter);
			componentResourceManager.ApplyResources(this.labelCustomNameExample, "labelCustomNameExample");
			this.labelCustomNameExample.Name = "labelCustomNameExample";
			componentResourceManager.ApplyResources(this.splitButtonInsertMacro, "splitButtonInsertMacro");
			this.splitButtonInsertMacro.Name = "splitButtonInsertMacro";
			this.splitButtonInsertMacro.UseVisualStyleBackColor = true;
			this.splitButtonInsertMacro.Click += new EventHandler(this.splitButtonInsertMacro_Click);
			componentResourceManager.ApplyResources(this.labelNewName, "labelNewName");
			this.labelNewName.Name = "labelNewName";
			componentResourceManager.ApplyResources(this.labelPrefix, "labelPrefix");
			this.labelPrefix.Name = "labelPrefix";
			componentResourceManager.ApplyResources(this.labelFilenameFormat, "labelFilenameFormat");
			this.labelFilenameFormat.Name = "labelFilenameFormat";
			this.comboBoxFilenameFormat.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxFilenameFormat.FormattingEnabled = true;
			componentResourceManager.ApplyResources(this.comboBoxFilenameFormat, "comboBoxFilenameFormat");
			this.comboBoxFilenameFormat.Name = "comboBoxFilenameFormat";
			this.comboBoxFilenameFormat.SelectedIndexChanged += new EventHandler(this.comboBoxFilenameFormat_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.textBoxPrefix, "textBoxPrefix");
			this.errorProvider.SetIconAlignment(this.textBoxPrefix, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxPrefix.IconAlignment"));
			this.textBoxPrefix.Name = "textBoxPrefix";
			this.textBoxPrefix.MouseEnter += new EventHandler(this.textBoxPrefix_MouseEnter);
			this.textBoxPrefix.Validating += new CancelEventHandler(this.textBoxPrefix_Validating);
			componentResourceManager.ApplyResources(this.buttonSameAsSource, "buttonSameAsSource");
			this.buttonSameAsSource.Name = "buttonSameAsSource";
			this.buttonSameAsSource.UseVisualStyleBackColor = true;
			this.buttonSameAsSource.Click += new EventHandler(this.buttonSameAsSource_Click);
			componentResourceManager.ApplyResources(this.checkBoxSaveRawFile, "checkBoxSaveRawFile");
			this.checkBoxSaveRawFile.Checked = true;
			this.checkBoxSaveRawFile.CheckState = CheckState.Checked;
			this.checkBoxSaveRawFile.Name = "checkBoxSaveRawFile";
			this.checkBoxSaveRawFile.UseVisualStyleBackColor = true;
			this.checkBoxSaveRawFile.CheckedChanged += new EventHandler(this.checkBoxSaveRawFile_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxOverwriteDestination, "checkBoxOverwriteDestination");
			this.checkBoxOverwriteDestination.Name = "checkBoxOverwriteDestination";
			this.checkBoxOverwriteDestination.UseVisualStyleBackColor = true;
			this.checkBoxOverwriteDestination.CheckedChanged += new EventHandler(this.checkBoxOverwriteDestination_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxDeleteSourceFiles, "checkBoxDeleteSourceFiles");
			this.checkBoxDeleteSourceFiles.Name = "checkBoxDeleteSourceFiles";
			this.checkBoxDeleteSourceFiles.UseVisualStyleBackColor = true;
			this.checkBoxDeleteSourceFiles.CheckedChanged += new EventHandler(this.checkBoxDeleteSourceFiles_CheckedChanged);
			this.comboBoxDestinationFormat.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxDestinationFormat.FormattingEnabled = true;
			componentResourceManager.ApplyResources(this.comboBoxDestinationFormat, "comboBoxDestinationFormat");
			this.comboBoxDestinationFormat.Name = "comboBoxDestinationFormat";
			this.comboBoxDestinationFormat.SelectedIndexChanged += new EventHandler(this.comboBoxDestinationFormat_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.labelConvertTo, "labelConvertTo");
			this.labelConvertTo.Name = "labelConvertTo";
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
			this.tabPageAdvancedSettings.BackColor = SystemColors.Control;
			this.tabPageAdvancedSettings.Controls.Add(this.textBoxMDF3SignalDelimiter);
			this.tabPageAdvancedSettings.Controls.Add(this.labelMDF3SignalDelimiter);
			this.tabPageAdvancedSettings.Controls.Add(this.comboBoxMDF3SignalFormat);
			this.tabPageAdvancedSettings.Controls.Add(this.labelMDF3SignalFormat);
			this.tabPageAdvancedSettings.Controls.Add(this.checkBoxWriteRawValues);
			this.tabPageAdvancedSettings.Controls.Add(this.checkBoxCopyMediaFiles);
			this.tabPageAdvancedSettings.Controls.Add(this.checkBoxMinDigitsForTriggerIndex);
			this.tabPageAdvancedSettings.Controls.Add(this.checkBoxCreateVsysvarFile);
			this.tabPageAdvancedSettings.Controls.Add(this.checkBoxRecoveryMode);
			this.tabPageAdvancedSettings.Controls.Add(this.checkBoxJumpOverSleepTime);
			this.tabPageAdvancedSettings.Controls.Add(this.checkBoxGermanExcelFormat);
			this.tabPageAdvancedSettings.Controls.Add(this.checkBoxRelativeTimestamps);
			this.tabPageAdvancedSettings.Controls.Add(this.checkBoxGlobalTimestamps);
			this.tabPageAdvancedSettings.Controls.Add(this.checkBoxSingleFile);
			this.tabPageAdvancedSettings.Controls.Add(this.checkBoxHexadecimal);
			this.tabPageAdvancedSettings.Controls.Add(this.numericUpDownMinDigitsForTriggerIndex);
			componentResourceManager.ApplyResources(this.tabPageAdvancedSettings, "tabPageAdvancedSettings");
			this.tabPageAdvancedSettings.Name = "tabPageAdvancedSettings";
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
			componentResourceManager.ApplyResources(this.checkBoxCreateVsysvarFile, "checkBoxCreateVsysvarFile");
			this.checkBoxCreateVsysvarFile.Name = "checkBoxCreateVsysvarFile";
			this.checkBoxCreateVsysvarFile.UseVisualStyleBackColor = true;
			this.checkBoxCreateVsysvarFile.CheckedChanged += new EventHandler(this.CheckBoxCreateVsysvarFile_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxRecoveryMode, "checkBoxRecoveryMode");
			this.checkBoxRecoveryMode.Name = "checkBoxRecoveryMode";
			this.checkBoxRecoveryMode.UseVisualStyleBackColor = true;
			this.checkBoxRecoveryMode.CheckedChanged += new EventHandler(this.checkBoxRecoveryMode_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxJumpOverSleepTime, "checkBoxJumpOverSleepTime");
			this.checkBoxJumpOverSleepTime.Name = "checkBoxJumpOverSleepTime";
			this.checkBoxJumpOverSleepTime.UseVisualStyleBackColor = true;
			this.checkBoxJumpOverSleepTime.CheckedChanged += new EventHandler(this.checkBoxJumpOverSleepTime_CheckedChanged);
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
			componentResourceManager.ApplyResources(this.checkBoxSingleFile, "checkBoxSingleFile");
			this.checkBoxSingleFile.Name = "checkBoxSingleFile";
			this.checkBoxSingleFile.UseVisualStyleBackColor = true;
			this.checkBoxSingleFile.CheckedChanged += new EventHandler(this.checkBoxSingleFile_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxHexadecimal, "checkBoxHexadecimal");
			this.checkBoxHexadecimal.Name = "checkBoxHexadecimal";
			this.checkBoxHexadecimal.UseVisualStyleBackColor = true;
			this.checkBoxHexadecimal.CheckedChanged += new EventHandler(this.checkBoxHexadecimal_CheckedChanged);
			componentResourceManager.ApplyResources(this.numericUpDownMinDigitsForTriggerIndex, "numericUpDownMinDigitsForTriggerIndex");
			NumericUpDown arg_11AB_0 = this.numericUpDownMinDigitsForTriggerIndex;
			int[] array = new int[4];
			array[0] = 15;
			arg_11AB_0.Maximum = new decimal(array);
			this.numericUpDownMinDigitsForTriggerIndex.Name = "numericUpDownMinDigitsForTriggerIndex";
			this.numericUpDownMinDigitsForTriggerIndex.ValueChanged += new EventHandler(this.numericUpDownMinDigitsForTriggerIndex_ValueChanged);
			this.tabPageSplitSettings.BackColor = SystemColors.Control;
			this.tabPageSplitSettings.Controls.Add(this.checkBoxUseDateTimeFromMeasurementStart);
			this.tabPageSplitSettings.Controls.Add(this.comboBoxTimeBase);
			this.tabPageSplitSettings.Controls.Add(this.textBoxFileFractionTime);
			this.tabPageSplitSettings.Controls.Add(this.checkBoxAbsoluteTimestamps);
			this.tabPageSplitSettings.Controls.Add(this.checkBoxSplitFileByTime);
			this.tabPageSplitSettings.Controls.Add(this.textBoxFileFractionSize);
			this.tabPageSplitSettings.Controls.Add(this.checkBoxSplitFileBySize);
			this.tabPageSplitSettings.Controls.Add(this.labelFractionMB);
			this.tabPageSplitSettings.Controls.Add(this.checkBoxSplitByLoc);
			this.tabPageSplitSettings.Controls.Add(this.checkBoxSuppressBufferConcat);
			componentResourceManager.ApplyResources(this.tabPageSplitSettings, "tabPageSplitSettings");
			this.tabPageSplitSettings.Name = "tabPageSplitSettings";
			componentResourceManager.ApplyResources(this.checkBoxUseDateTimeFromMeasurementStart, "checkBoxUseDateTimeFromMeasurementStart");
			this.checkBoxUseDateTimeFromMeasurementStart.Name = "checkBoxUseDateTimeFromMeasurementStart";
			this.checkBoxUseDateTimeFromMeasurementStart.UseVisualStyleBackColor = true;
			this.checkBoxUseDateTimeFromMeasurementStart.CheckedChanged += new EventHandler(this.checkBoxUseDateTimeFromMeasurementStart_CheckedChanged);
			this.comboBoxTimeBase.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxTimeBase.FormattingEnabled = true;
			componentResourceManager.ApplyResources(this.comboBoxTimeBase, "comboBoxTimeBase");
			this.comboBoxTimeBase.Name = "comboBoxTimeBase";
			this.comboBoxTimeBase.SelectedIndexChanged += new EventHandler(this.ComboBoxTimeBase_SelectedIndexChanged);
			this.errorProvider.SetIconAlignment(this.textBoxFileFractionTime, (ErrorIconAlignment)componentResourceManager.GetObject("textBoxFileFractionTime.IconAlignment"));
			componentResourceManager.ApplyResources(this.textBoxFileFractionTime, "textBoxFileFractionTime");
			this.textBoxFileFractionTime.Name = "textBoxFileFractionTime";
			this.textBoxFileFractionTime.EnabledChanged += new EventHandler(this.TextBoxFileFractionTime_EnabledChanged);
			this.textBoxFileFractionTime.Validating += new CancelEventHandler(this.TextBoxFileFractionTime_Validating);
			componentResourceManager.ApplyResources(this.checkBoxAbsoluteTimestamps, "checkBoxAbsoluteTimestamps");
			this.checkBoxAbsoluteTimestamps.Name = "checkBoxAbsoluteTimestamps";
			this.checkBoxAbsoluteTimestamps.UseVisualStyleBackColor = true;
			this.checkBoxAbsoluteTimestamps.CheckedChanged += new EventHandler(this.CheckBoxAbsoluteTimestamps_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxSplitFileByTime, "checkBoxSplitFileByTime");
			this.checkBoxSplitFileByTime.Name = "checkBoxSplitFileByTime";
			this.checkBoxSplitFileByTime.UseVisualStyleBackColor = true;
			this.checkBoxSplitFileByTime.CheckedChanged += new EventHandler(this.CheckBoxSplitFileByTime_CheckedChanged);
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
			this.tabPageChannelMapping.BackColor = SystemColors.Control;
			this.tabPageChannelMapping.Controls.Add(this.channelMapping);
			componentResourceManager.ApplyResources(this.tabPageChannelMapping, "tabPageChannelMapping");
			this.tabPageChannelMapping.Name = "tabPageChannelMapping";
			componentResourceManager.ApplyResources(this.channelMapping, "channelMapping");
			this.channelMapping.FileConversionParameters = null;
			this.channelMapping.IsCLFConversionMode = false;
			this.channelMapping.Name = "channelMapping";
			this.tabPageExportDatabases.BackColor = SystemColors.Control;
			this.tabPageExportDatabases.Controls.Add(this.exportDatabases);
			componentResourceManager.ApplyResources(this.tabPageExportDatabases, "tabPageExportDatabases");
			this.tabPageExportDatabases.Name = "tabPageExportDatabases";
			this.exportDatabases.AllowDrop = true;
			this.exportDatabases.AnalysisPackageParameters = null;
			componentResourceManager.ApplyResources(this.exportDatabases, "exportDatabases");
			this.exportDatabases.ApplicationDatabaseManager = null;
			this.exportDatabases.ConfigurationManagerService = null;
			this.exportDatabases.DisplayMode = null;
			this.exportDatabases.FileConversionParameters = null;
			this.exportDatabases.IsVLExportMode = false;
			this.exportDatabases.ModelValidator = null;
			this.exportDatabases.Name = "exportDatabases";
			this.exportDatabases.SemanticChecker = null;
			this.imageListErrorIcons.ColorDepth = ColorDepth.Depth8Bit;
			componentResourceManager.ApplyResources(this.imageListErrorIcons, "imageListErrorIcons");
			this.imageListErrorIcons.TransparentColor = Color.Transparent;
			this.errorProvider.BlinkStyle = ErrorBlinkStyle.NeverBlink;
			this.errorProvider.ContainerControl = this;
			base.AutoScaleMode = AutoScaleMode.Inherit;
			base.Controls.Add(this.groupBoxFileConversion);
			componentResourceManager.ApplyResources(this, "$this");
			base.Name = "FileConversion";
			base.VisibleChanged += new EventHandler(this.FileConversion_VisibleChanged);
			this.groupBoxFileConversion.ResumeLayout(false);
			this.groupBoxFileConversion.PerformLayout();
			this.tabControlSettings.ResumeLayout(false);
			this.tabPageGeneralSettings.ResumeLayout(false);
			this.tabPageGeneralSettings.PerformLayout();
			this.tabPageAdvancedSettings.ResumeLayout(false);
			this.tabPageAdvancedSettings.PerformLayout();
			((ISupportInitialize)this.numericUpDownMinDigitsForTriggerIndex).EndInit();
			this.tabPageSplitSettings.ResumeLayout(false);
			this.tabPageSplitSettings.PerformLayout();
			this.tabPageChannelMapping.ResumeLayout(false);
			this.tabPageExportDatabases.ResumeLayout(false);
			((ISupportInitialize)this.errorProvider).EndInit();
			base.ResumeLayout(false);
		}
	}
}
