using DevComponents.DotNetBar;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using Vector.ConfigurationDialog;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.Options
{
	public class OptionsDialog : ConfigurationDialog
	{
		private readonly ProjectExport mProjectExport;

		private readonly General mGeneralOptions;

		private readonly AdditionalDrives mAdditionalDrives;

		private readonly Conversion mConversionOptions;

		private readonly GeneratedFiles mGeneratedFiles;

		private readonly AnalysisPackage mAnalysisPackage;

		private readonly QuickViewOptions mQuickViewOptions;

		private bool mIsInitialzed;

		private readonly bool mIsVlExportMode;

		private readonly LoggerType mLoggerType;

		private IContainer components;

		private ButtonItem mButtonGeneralOptions;

		private ButtonItem mButtonProjectExportOptions;

		private ButtonItem mButtonAdditionalDrives;

		private ButtonItem mButtonConversionProfiles;

		private ButtonItem mButtonGeneratedFiles;

		private ButtonItem mButtonAnalysisPackage;

		private ButtonItem mButtonQuickView;

		public GlobalOptions GlobalOptions
		{
			get;
			private set;
		}

		public OptionsDialog(AppDataRoot appDataRoot, LoggerType loggerType, bool standaloneMode = false)
		{
			this.InitializeComponent();
			this.mLoggerType = loggerType;
			this.mProjectExport = new ProjectExport(appDataRoot.ProjectExporterPage.ProjectExporterParameters, true);
			this.mGeneralOptions = new General(appDataRoot.GlobalOptions, standaloneMode);
			this.mAdditionalDrives = new AdditionalDrives(appDataRoot.GlobalOptions);
			this.mConversionOptions = new Conversion(loggerType);
			this.mGeneratedFiles = new GeneratedFiles(appDataRoot.GlobalOptions, loggerType);
			this.mAnalysisPackage = new AnalysisPackage(appDataRoot.AnalysisPackageSettings.AnalysisPackageParameters, true, standaloneMode);
			this.mQuickViewOptions = new QuickViewOptions();
			this.GlobalOptions = appDataRoot.GlobalOptions;
			this.mIsVlExportMode = standaloneMode;
		}

		protected override void InitializeConfigurationPages()
		{
			if (!this.mIsInitialzed)
			{
				base.AddConfigurationPage(this.mButtonGeneralOptions, this.mGeneralOptions);
				base.AddConfigurationPage(this.mButtonAdditionalDrives, this.mAdditionalDrives);
				if (!this.mIsVlExportMode)
				{
					base.AddConfigurationPage(this.mButtonProjectExportOptions, this.mProjectExport);
				}
				base.AddConfigurationPage(this.mButtonConversionProfiles, this.mConversionOptions);
				base.AddConfigurationPage(this.mButtonAnalysisPackage, this.mAnalysisPackage);
				if (!this.mIsVlExportMode)
				{
					base.AddConfigurationPage(this.mButtonGeneratedFiles, this.mGeneratedFiles);
				}
				if (this.mQuickViewOptions.IsVisibleForLoggerType(this.mLoggerType))
				{
					base.AddConfigurationPage(this.mButtonQuickView, this.mQuickViewOptions);
				}
				this.mIsInitialzed = true;
			}
		}

		protected override void ShowHelp()
		{
			int helpID = (int)base.HelpID;
			if (this.mIsVlExportMode)
			{
				Help.ShowHelp(new Form(), Resources.HelpFileNameExport, HelpNavigator.TopicId, helpID.ToString(CultureInfo.InvariantCulture));
				return;
			}
			MainWindow.ShowHelpForDialog(helpID);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(OptionsDialog));
			this.mButtonGeneralOptions = new ButtonItem();
			this.mButtonProjectExportOptions = new ButtonItem();
			this.mButtonAdditionalDrives = new ButtonItem();
			this.mButtonConversionProfiles = new ButtonItem();
			this.mButtonGeneratedFiles = new ButtonItem();
			this.mButtonAnalysisPackage = new ButtonItem();
			this.mButtonQuickView = new ButtonItem();
			this.mNavigationPanel.SuspendLayout();
			this.mNavigationArea.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.mHelp, "mHelp");
			componentResourceManager.ApplyResources(this.mCancel, "mCancel");
			componentResourceManager.ApplyResources(this.mOK, "mOK");
			componentResourceManager.ApplyResources(this.mNavigationPanel, "mNavigationPanel");
			this.mButtonsPanel.BackgroundStyle.BackColor = SystemColors.Window;
			this.mButtonsPanel.BackgroundStyle.BorderColor = SystemColors.Window;
			this.mButtonsPanel.BackgroundStyle.Class = "ItemPanel";
			this.mButtonsPanel.BackgroundStyle.CornerType = eCornerType.Square;
			this.mButtonsPanel.ColorScheme.ItemCheckedBackground = Color.FromArgb(152, 203, 255);
			this.mButtonsPanel.ColorScheme.ItemCheckedBackground2 = Color.FromArgb(152, 203, 255);
			this.mButtonsPanel.ColorScheme.ItemCheckedBorder = SystemColors.Highlight;
			this.mButtonsPanel.ColorScheme.ItemCheckedText = SystemColors.HighlightText;
			this.mButtonsPanel.ColorScheme.ItemHotBackground = Color.FromArgb(196, 225, 255);
			this.mButtonsPanel.ColorScheme.ItemHotBackground2 = Color.FromArgb(196, 225, 255);
			this.mButtonsPanel.ColorScheme.ItemHotBorder = SystemColors.Highlight;
			this.mButtonsPanel.ColorScheme.ItemPressedBackground = Color.FromArgb(152, 203, 255);
			this.mButtonsPanel.ColorScheme.ItemPressedBackground2 = Color.FromArgb(152, 203, 255);
			this.mButtonsPanel.ColorScheme.ItemPressedBorder = SystemColors.Highlight;
			this.mButtonsPanel.Items.AddRange(new BaseItem[]
			{
				this.mButtonGeneralOptions,
				this.mButtonAdditionalDrives,
				this.mButtonProjectExportOptions,
				this.mButtonConversionProfiles,
				this.mButtonAnalysisPackage,
				this.mButtonGeneratedFiles,
				this.mButtonQuickView
			});
			componentResourceManager.ApplyResources(this.mButtonsPanel, "mButtonsPanel");
			componentResourceManager.ApplyResources(this.mConfigurationPagePanel, "mConfigurationPagePanel");
			componentResourceManager.ApplyResources(this.mNavigationArea, "mNavigationArea");
			componentResourceManager.ApplyResources(this.mShowMoreButton, "mShowMoreButton");
			this.mButtonGeneralOptions.ButtonStyle = eButtonStyle.ImageAndText;
			this.mButtonGeneralOptions.ColorTable = eButtonColor.Flat;
			this.mButtonGeneralOptions.ForeColor = SystemColors.ControlText;
			this.mButtonGeneralOptions.HotForeColor = SystemColors.ActiveCaptionText;
			this.mButtonGeneralOptions.Icon = Resources.IconGeneral;
			this.mButtonGeneralOptions.ImagePaddingVertical = 5;
			this.mButtonGeneralOptions.Name = "mButtonGeneralOptions";
			componentResourceManager.ApplyResources(this.mButtonGeneralOptions, "mButtonGeneralOptions");
			this.mButtonProjectExportOptions.ButtonStyle = eButtonStyle.ImageAndText;
			this.mButtonProjectExportOptions.ColorTable = eButtonColor.Flat;
			this.mButtonProjectExportOptions.ForeColor = SystemColors.ControlText;
			this.mButtonProjectExportOptions.Icon = Resources.IconWriteToDevice;
			this.mButtonProjectExportOptions.ImagePaddingVertical = 5;
			this.mButtonProjectExportOptions.Name = "mButtonProjectExportOptions";
			componentResourceManager.ApplyResources(this.mButtonProjectExportOptions, "mButtonProjectExportOptions");
			this.mButtonAdditionalDrives.ButtonStyle = eButtonStyle.ImageAndText;
			this.mButtonAdditionalDrives.ColorTable = eButtonColor.Flat;
			this.mButtonAdditionalDrives.ForeColor = SystemColors.ControlText;
			this.mButtonAdditionalDrives.Image = Resources.ImageWriteToCard;
			this.mButtonAdditionalDrives.ImagePaddingVertical = 5;
			this.mButtonAdditionalDrives.Name = "mButtonAdditionalDrives";
			componentResourceManager.ApplyResources(this.mButtonAdditionalDrives, "mButtonAdditionalDrives");
			this.mButtonConversionProfiles.ButtonStyle = eButtonStyle.ImageAndText;
			this.mButtonConversionProfiles.ColorTable = eButtonColor.Flat;
			this.mButtonConversionProfiles.ForeColor = SystemColors.ControlText;
			this.mButtonConversionProfiles.Image = Resources.ImageConvertSettings;
			this.mButtonConversionProfiles.ImagePaddingVertical = 5;
			this.mButtonConversionProfiles.Name = "mButtonConversionProfiles";
			componentResourceManager.ApplyResources(this.mButtonConversionProfiles, "mButtonConversionProfiles");
			this.mButtonGeneratedFiles.ButtonStyle = eButtonStyle.ImageAndText;
			this.mButtonGeneratedFiles.ColorTable = eButtonColor.Flat;
			this.mButtonGeneratedFiles.ForeColor = SystemColors.ControlText;
			this.mButtonGeneratedFiles.Icon = Resources.IconFileSet;
			this.mButtonGeneratedFiles.ImagePaddingVertical = 5;
			this.mButtonGeneratedFiles.Name = "mButtonGeneratedFiles";
			componentResourceManager.ApplyResources(this.mButtonGeneratedFiles, "mButtonGeneratedFiles");
			this.mButtonAnalysisPackage.ButtonStyle = eButtonStyle.ImageAndText;
			this.mButtonAnalysisPackage.ColorTable = eButtonColor.Flat;
			this.mButtonAnalysisPackage.ForeColor = SystemColors.ControlText;
			this.mButtonAnalysisPackage.Icon = Resources.IconDatabases;
			this.mButtonAnalysisPackage.ImagePaddingVertical = 5;
			this.mButtonAnalysisPackage.Name = "mButtonAnalysisPackage";
			componentResourceManager.ApplyResources(this.mButtonAnalysisPackage, "mButtonAnalysisPackage");
			this.mButtonQuickView.ButtonStyle = eButtonStyle.ImageAndText;
			this.mButtonQuickView.ColorTable = eButtonColor.Flat;
			this.mButtonQuickView.ForeColor = SystemColors.ControlText;
			this.mButtonQuickView.Icon = Resources.IconFileManager;
			this.mButtonQuickView.ImagePaddingVertical = 5;
			this.mButtonQuickView.Name = "mButtonQuickView";
			componentResourceManager.ApplyResources(this.mButtonQuickView, "mButtonQuickView");
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ControlBox = false;
			base.Name = "OptionsDialog";
			this.mNavigationPanel.ResumeLayout(false);
			this.mNavigationPanel.PerformLayout();
			this.mNavigationArea.ResumeLayout(false);
			this.mNavigationArea.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
