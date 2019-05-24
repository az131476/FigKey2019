using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Vector.ConfigurationDialog;
using Vector.UtilityFunctions;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.Options
{
	public class AnalysisPackage : ConfigurationPageView, IConfigurationPageController, IDisposable
	{
		private bool isStandaloneMode;

		private IContainer components;

		private CheckBox checkBoxShowPassword;

		private Label labelPassword;

		private TextBox textBoxPassword;

		private CheckBox checkBoxSetPasswordOnDemand;

		private CheckBox checkBoxProtectWithPassword;

		private CheckBox checkBoxExportBusDatabases;

		private CheckBox checkBoxAskPriorToExport;

		private CheckBox checkBoxExportToMemoryCard;

		private TitledGroup titledGroupMemoryCard;

		private TitledGroup titledGroupPassword;

		private TitledGroup titledGroupFiltering;

		private Label labelConfirm;

		private TextBox textBoxConfirmPassword;

		private TitledGroup titledGroupAnalysisImport;

		private RadioButton radioButton_automatically_remove_export_dbs;

		private RadioButton radioButton_automatically_keep_export_dbs;

		private RadioButton radioButton_askEveryTime;

		private Label label_AnalysisPackageImport;

		private TitledGroup titledGroupAutomaticalImport;

		private Button buttonSelectFolder;

		private TextBox textBoxSearchFolder;

		private Label labelPoolFolder;

		private AnalysisPackageParameters Parameters
		{
			get;
			set;
		}

		public Control ConfigurationPageView
		{
			get
			{
				return this;
			}
		}

		public uint HelpID
		{
			get
			{
				return (uint)GUIUtil.HelpPageID_OptionsDialog;
			}
		}

		public uint PageID
		{
			get
			{
				return 5u;
			}
		}

		public AnalysisPackage(AnalysisPackageParameters parameters, bool exportToDeviceEnabled, bool standaloneMode)
		{
			this.InitializeComponent();
			this.Parameters = parameters;
			this.Init(exportToDeviceEnabled, standaloneMode);
		}

		public void Init(bool exportToDeviceEnabled, bool isStandaloneMode)
		{
			this.titledGroupMemoryCard.Visible = exportToDeviceEnabled;
			this.isStandaloneMode = isStandaloneMode;
			if (this.Parameters != null)
			{
				this.checkBoxExportBusDatabases.Checked = this.Parameters.StoreBusDatabases;
				this.checkBoxProtectWithPassword.Checked = this.Parameters.ProtectWithPassword;
				this.checkBoxSetPasswordOnDemand.Checked = this.Parameters.SetPasswordOnDemand;
				this.textBoxPassword.Text = this.Parameters.Password;
				this.textBoxConfirmPassword.Text = this.Parameters.Password;
				this.checkBoxExportToMemoryCard.Checked = this.Parameters.ExportToMemoryCard;
				this.checkBoxAskPriorToExport.Checked = this.Parameters.AskPriorToExport;
				this.radioButton_automatically_remove_export_dbs.Checked = (this.Parameters.DoNotAskToRemoveExistingDatabases && this.Parameters.AutomaticallyRemoveExistingDatabases);
				this.radioButton_automatically_keep_export_dbs.Checked = (this.Parameters.DoNotAskToRemoveExistingDatabases && !this.Parameters.AutomaticallyRemoveExistingDatabases);
				this.radioButton_askEveryTime.Checked = !this.Parameters.DoNotAskToRemoveExistingDatabases;
				if (this.textBoxPassword.Text.Length == 0 && !this.checkBoxSetPasswordOnDemand.Checked)
				{
					this.checkBoxProtectWithPassword.Checked = false;
					this.Parameters.ProtectWithPassword = false;
				}
				this.checkBoxShowPassword.Checked = false;
				this.textBoxSearchFolder.Text = this.Parameters.PoolPath;
				this.UpdateUI();
			}
			if (isStandaloneMode)
			{
				this.HideOptionsInVLExportMode();
			}
		}

		public void UpdateUI()
		{
			this.checkBoxSetPasswordOnDemand.Enabled = (this.checkBoxProtectWithPassword.Checked && !this.isStandaloneMode);
			this.labelPassword.Enabled = (this.checkBoxProtectWithPassword.Checked && !this.checkBoxSetPasswordOnDemand.Checked);
			this.labelConfirm.Enabled = (this.labelPassword.Enabled && !this.checkBoxShowPassword.Checked);
			this.textBoxConfirmPassword.Enabled = (this.labelPassword.Enabled && !this.checkBoxShowPassword.Checked);
			this.textBoxPassword.Enabled = (this.checkBoxProtectWithPassword.Checked && !this.checkBoxSetPasswordOnDemand.Checked);
			this.checkBoxShowPassword.Enabled = (this.checkBoxProtectWithPassword.Checked && !this.checkBoxSetPasswordOnDemand.Checked);
			this.checkBoxAskPriorToExport.Enabled = this.checkBoxExportToMemoryCard.Checked;
			this.textBoxPassword.UseSystemPasswordChar = !this.checkBoxShowPassword.Checked;
			this.textBoxSearchFolder.Enabled = true;
			this.buttonSelectFolder.Enabled = true;
		}

		public bool CheckOptions()
		{
			if (this.checkBoxProtectWithPassword.Checked && !this.checkBoxSetPasswordOnDemand.Checked && this.textBoxPassword.Text.Length == 0)
			{
				InformMessageBox.Error(Resources.ErrorPasswordNotSet);
				this.textBoxPassword.Focus();
				return false;
			}
			if (this.checkBoxProtectWithPassword.Checked && !this.checkBoxShowPassword.Checked && this.textBoxPassword.Text != this.textBoxConfirmPassword.Text)
			{
				InformMessageBox.Error(Resources.ErrorPasswordsDontMatch);
				this.textBoxPassword.Focus();
				return false;
			}
			return true;
		}

		public void UpdateOptions()
		{
			if (this.Parameters != null)
			{
				this.Parameters.ExportToMemoryCard = this.checkBoxExportToMemoryCard.Checked;
				this.Parameters.AskPriorToExport = this.checkBoxAskPriorToExport.Checked;
				this.Parameters.StoreBusDatabases = this.checkBoxExportBusDatabases.Checked;
				this.Parameters.ProtectWithPassword = this.checkBoxProtectWithPassword.Checked;
				this.Parameters.SetPasswordOnDemand = this.checkBoxSetPasswordOnDemand.Checked;
				this.Parameters.Password = this.textBoxPassword.Text;
				this.Parameters.DoNotAskToRemoveExistingDatabases = !this.radioButton_askEveryTime.Checked;
				this.Parameters.AutomaticallyRemoveExistingDatabases = (!this.radioButton_askEveryTime.Checked && this.radioButton_automatically_remove_export_dbs.Checked);
				this.Parameters.PoolPath = this.textBoxSearchFolder.Text;
			}
		}

		private void checkBoxExport2MemoryCard_CheckedChanged(object sender, EventArgs e)
		{
			this.UpdateUI();
		}

		private void checkBoxProtectWithPassword_CheckedChanged(object sender, EventArgs e)
		{
			this.UpdateUI();
		}

		private void checkBoxShowPassword_CheckedChanged(object sender, EventArgs e)
		{
			this.UpdateUI();
		}

		private void checkBoxPasswordSetOnDemand_CheckedChanged(object sender, EventArgs e)
		{
			this.UpdateUI();
		}

		private void checkBoxDoNotAskToRemoveExistingDatabases_CheckedChanged(object sender, EventArgs e)
		{
			this.UpdateUI();
		}

		private void checkBoxAutomaticSearch_CheckedChanged(object sender, EventArgs e)
		{
			this.UpdateUI();
		}

		private void buttonSelectFolder_Click(object sender, EventArgs e)
		{
			using (BrowseFolderDialog browseFolderDialog = new BrowseFolderDialog())
			{
				if (!string.IsNullOrEmpty(this.Parameters.PoolPath))
				{
					browseFolderDialog.SelectedPath = this.Parameters.PoolPath;
				}
				else
				{
					browseFolderDialog.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
				}
				browseFolderDialog.Title = Resources.SelectDirectory;
				if (DialogResult.OK == browseFolderDialog.ShowDialog())
				{
					if (!string.IsNullOrEmpty(browseFolderDialog.SelectedPath) && Directory.Exists(browseFolderDialog.SelectedPath))
					{
						this.textBoxSearchFolder.Text = browseFolderDialog.SelectedPath;
					}
				}
			}
		}

		public void OnViewOpened()
		{
		}

		public void OnViewOpening()
		{
		}

		public void OnViewClosed()
		{
		}

		public bool SavePageData()
		{
			this.UpdateOptions();
			return true;
		}

		public void CancelPageData()
		{
		}

		public bool OnViewClosing(DialogResult closingReason)
		{
			return (closingReason != DialogResult.OK && closingReason != DialogResult.None) || this.CheckOptions();
		}

		public bool OnViewClosing()
		{
			return true;
		}

		public void HideOptionsInVLExportMode()
		{
			this.titledGroupMemoryCard.Enabled = false;
			TitledGroup expr_12 = this.titledGroupMemoryCard;
			expr_12.Title = expr_12.Title + " (" + Resources.VLExportOnlyAvailableInVLConfig + ")";
			this.titledGroupFiltering.Enabled = false;
			TitledGroup expr_43 = this.titledGroupFiltering;
			expr_43.Title = expr_43.Title + " (" + Resources.VLExportOnlyAvailableInVLConfig + ")";
			this.checkBoxSetPasswordOnDemand.Enabled = false;
			this.checkBoxSetPasswordOnDemand.Visible = false;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(AnalysisPackage));
			this.checkBoxShowPassword = new CheckBox();
			this.labelPassword = new Label();
			this.textBoxPassword = new TextBox();
			this.checkBoxSetPasswordOnDemand = new CheckBox();
			this.checkBoxProtectWithPassword = new CheckBox();
			this.checkBoxAskPriorToExport = new CheckBox();
			this.checkBoxExportToMemoryCard = new CheckBox();
			this.checkBoxExportBusDatabases = new CheckBox();
			this.titledGroupMemoryCard = new TitledGroup();
			this.titledGroupPassword = new TitledGroup();
			this.textBoxConfirmPassword = new TextBox();
			this.labelConfirm = new Label();
			this.titledGroupFiltering = new TitledGroup();
			this.titledGroupAnalysisImport = new TitledGroup();
			this.radioButton_automatically_remove_export_dbs = new RadioButton();
			this.radioButton_automatically_keep_export_dbs = new RadioButton();
			this.radioButton_askEveryTime = new RadioButton();
			this.label_AnalysisPackageImport = new Label();
			this.titledGroupAutomaticalImport = new TitledGroup();
			this.buttonSelectFolder = new Button();
			this.textBoxSearchFolder = new TextBox();
			this.labelPoolFolder = new Label();
			this.titledGroupMemoryCard.SuspendLayout();
			this.titledGroupPassword.SuspendLayout();
			this.titledGroupFiltering.SuspendLayout();
			this.titledGroupAnalysisImport.SuspendLayout();
			this.titledGroupAutomaticalImport.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.checkBoxShowPassword, "checkBoxShowPassword");
			this.checkBoxShowPassword.Name = "checkBoxShowPassword";
			this.checkBoxShowPassword.UseVisualStyleBackColor = true;
			this.checkBoxShowPassword.CheckedChanged += new EventHandler(this.checkBoxShowPassword_CheckedChanged);
			componentResourceManager.ApplyResources(this.labelPassword, "labelPassword");
			this.labelPassword.Name = "labelPassword";
			componentResourceManager.ApplyResources(this.textBoxPassword, "textBoxPassword");
			this.textBoxPassword.Name = "textBoxPassword";
			this.textBoxPassword.UseSystemPasswordChar = true;
			componentResourceManager.ApplyResources(this.checkBoxSetPasswordOnDemand, "checkBoxSetPasswordOnDemand");
			this.checkBoxSetPasswordOnDemand.Name = "checkBoxSetPasswordOnDemand";
			this.checkBoxSetPasswordOnDemand.UseVisualStyleBackColor = true;
			this.checkBoxSetPasswordOnDemand.CheckedChanged += new EventHandler(this.checkBoxPasswordSetOnDemand_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxProtectWithPassword, "checkBoxProtectWithPassword");
			this.checkBoxProtectWithPassword.Name = "checkBoxProtectWithPassword";
			this.checkBoxProtectWithPassword.UseVisualStyleBackColor = true;
			this.checkBoxProtectWithPassword.CheckedChanged += new EventHandler(this.checkBoxProtectWithPassword_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxAskPriorToExport, "checkBoxAskPriorToExport");
			this.checkBoxAskPriorToExport.Name = "checkBoxAskPriorToExport";
			this.checkBoxAskPriorToExport.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.checkBoxExportToMemoryCard, "checkBoxExportToMemoryCard");
			this.checkBoxExportToMemoryCard.Name = "checkBoxExportToMemoryCard";
			this.checkBoxExportToMemoryCard.UseVisualStyleBackColor = true;
			this.checkBoxExportToMemoryCard.CheckedChanged += new EventHandler(this.checkBoxExport2MemoryCard_CheckedChanged);
			componentResourceManager.ApplyResources(this.checkBoxExportBusDatabases, "checkBoxExportBusDatabases");
			this.checkBoxExportBusDatabases.Checked = true;
			this.checkBoxExportBusDatabases.CheckState = CheckState.Checked;
			this.checkBoxExportBusDatabases.Name = "checkBoxExportBusDatabases";
			this.checkBoxExportBusDatabases.UseVisualStyleBackColor = true;
			this.titledGroupMemoryCard.AllowDrop = true;
			componentResourceManager.ApplyResources(this.titledGroupMemoryCard, "titledGroupMemoryCard");
			this.titledGroupMemoryCard.AutoSizeGroup = true;
			this.titledGroupMemoryCard.BackColor = SystemColors.Window;
			this.titledGroupMemoryCard.Controls.Add(this.checkBoxAskPriorToExport);
			this.titledGroupMemoryCard.Controls.Add(this.checkBoxExportToMemoryCard);
			this.titledGroupMemoryCard.Image = null;
			this.titledGroupMemoryCard.Name = "titledGroupMemoryCard";
			this.titledGroupPassword.AllowDrop = true;
			componentResourceManager.ApplyResources(this.titledGroupPassword, "titledGroupPassword");
			this.titledGroupPassword.AutoSizeGroup = true;
			this.titledGroupPassword.BackColor = SystemColors.Window;
			this.titledGroupPassword.Controls.Add(this.textBoxConfirmPassword);
			this.titledGroupPassword.Controls.Add(this.labelConfirm);
			this.titledGroupPassword.Controls.Add(this.checkBoxShowPassword);
			this.titledGroupPassword.Controls.Add(this.labelPassword);
			this.titledGroupPassword.Controls.Add(this.checkBoxProtectWithPassword);
			this.titledGroupPassword.Controls.Add(this.textBoxPassword);
			this.titledGroupPassword.Controls.Add(this.checkBoxSetPasswordOnDemand);
			this.titledGroupPassword.Image = null;
			this.titledGroupPassword.Name = "titledGroupPassword";
			componentResourceManager.ApplyResources(this.textBoxConfirmPassword, "textBoxConfirmPassword");
			this.textBoxConfirmPassword.Name = "textBoxConfirmPassword";
			this.textBoxConfirmPassword.UseSystemPasswordChar = true;
			componentResourceManager.ApplyResources(this.labelConfirm, "labelConfirm");
			this.labelConfirm.Name = "labelConfirm";
			this.titledGroupFiltering.AllowDrop = true;
			componentResourceManager.ApplyResources(this.titledGroupFiltering, "titledGroupFiltering");
			this.titledGroupFiltering.AutoSizeGroup = true;
			this.titledGroupFiltering.BackColor = SystemColors.Window;
			this.titledGroupFiltering.Controls.Add(this.checkBoxExportBusDatabases);
			this.titledGroupFiltering.Image = null;
			this.titledGroupFiltering.Name = "titledGroupFiltering";
			this.titledGroupAnalysisImport.AllowDrop = true;
			componentResourceManager.ApplyResources(this.titledGroupAnalysisImport, "titledGroupAnalysisImport");
			this.titledGroupAnalysisImport.AutoSizeGroup = true;
			this.titledGroupAnalysisImport.BackColor = SystemColors.Window;
			this.titledGroupAnalysisImport.Controls.Add(this.radioButton_automatically_remove_export_dbs);
			this.titledGroupAnalysisImport.Controls.Add(this.radioButton_automatically_keep_export_dbs);
			this.titledGroupAnalysisImport.Controls.Add(this.radioButton_askEveryTime);
			this.titledGroupAnalysisImport.Controls.Add(this.label_AnalysisPackageImport);
			this.titledGroupAnalysisImport.Image = null;
			this.titledGroupAnalysisImport.Name = "titledGroupAnalysisImport";
			componentResourceManager.ApplyResources(this.radioButton_automatically_remove_export_dbs, "radioButton_automatically_remove_export_dbs");
			this.radioButton_automatically_remove_export_dbs.Name = "radioButton_automatically_remove_export_dbs";
			this.radioButton_automatically_remove_export_dbs.TabStop = true;
			this.radioButton_automatically_remove_export_dbs.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.radioButton_automatically_keep_export_dbs, "radioButton_automatically_keep_export_dbs");
			this.radioButton_automatically_keep_export_dbs.Name = "radioButton_automatically_keep_export_dbs";
			this.radioButton_automatically_keep_export_dbs.TabStop = true;
			this.radioButton_automatically_keep_export_dbs.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.radioButton_askEveryTime, "radioButton_askEveryTime");
			this.radioButton_askEveryTime.Name = "radioButton_askEveryTime";
			this.radioButton_askEveryTime.TabStop = true;
			this.radioButton_askEveryTime.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.label_AnalysisPackageImport, "label_AnalysisPackageImport");
			this.label_AnalysisPackageImport.Name = "label_AnalysisPackageImport";
			this.titledGroupAutomaticalImport.AllowDrop = true;
			componentResourceManager.ApplyResources(this.titledGroupAutomaticalImport, "titledGroupAutomaticalImport");
			this.titledGroupAutomaticalImport.AutoSizeGroup = true;
			this.titledGroupAutomaticalImport.BackColor = SystemColors.Window;
			this.titledGroupAutomaticalImport.Controls.Add(this.buttonSelectFolder);
			this.titledGroupAutomaticalImport.Controls.Add(this.textBoxSearchFolder);
			this.titledGroupAutomaticalImport.Controls.Add(this.labelPoolFolder);
			this.titledGroupAutomaticalImport.Image = null;
			this.titledGroupAutomaticalImport.Name = "titledGroupAutomaticalImport";
			componentResourceManager.ApplyResources(this.buttonSelectFolder, "buttonSelectFolder");
			this.buttonSelectFolder.Name = "buttonSelectFolder";
			this.buttonSelectFolder.UseVisualStyleBackColor = true;
			this.buttonSelectFolder.Click += new EventHandler(this.buttonSelectFolder_Click);
			componentResourceManager.ApplyResources(this.textBoxSearchFolder, "textBoxSearchFolder");
			this.textBoxSearchFolder.Name = "textBoxSearchFolder";
			componentResourceManager.ApplyResources(this.labelPoolFolder, "labelPoolFolder");
			this.labelPoolFolder.Name = "labelPoolFolder";
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.titledGroupAutomaticalImport);
			base.Controls.Add(this.titledGroupAnalysisImport);
			base.Controls.Add(this.titledGroupFiltering);
			base.Controls.Add(this.titledGroupPassword);
			base.Controls.Add(this.titledGroupMemoryCard);
			base.Name = "AnalysisPackage";
			this.titledGroupMemoryCard.ResumeLayout(false);
			this.titledGroupMemoryCard.PerformLayout();
			this.titledGroupPassword.ResumeLayout(false);
			this.titledGroupPassword.PerformLayout();
			this.titledGroupFiltering.ResumeLayout(false);
			this.titledGroupFiltering.PerformLayout();
			this.titledGroupAnalysisImport.ResumeLayout(false);
			this.titledGroupAnalysisImport.PerformLayout();
			this.titledGroupAutomaticalImport.ResumeLayout(false);
			this.titledGroupAutomaticalImport.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
