using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Vector.ConfigurationDialog;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.Options
{
	public class ProjectExport : ConfigurationPageView, IConfigurationPageController, IDisposable
	{
		private IContainer components;

		private CheckBox checkBoxShowPassword;

		private Label labelPassword;

		private TextBox textBoxPassword;

		private CheckBox checkBoxSetPasswordOnDemand;

		private CheckBox checkBoxProtectWithPassword;

		private CheckBox checkBoxExportIncludeFiles;

		private CheckBox checkBoxExportDiagnosticDescriptions;

		private CheckBox checkBoxExportNonBusDatabases;

		private CheckBox checkBoxExportCCPXCPDatabases;

		private CheckBox checkBoxExportBusDatabases;

		private CheckBox checkBoxAskPriorToExport;

		private CheckBox checkBoxExportToMemoryCard;

		private TitledGroup titledGroupMemoryCard;

		private TitledGroup titledGroupPassword;

		private TitledGroup titledGroupFiltering;

		private Label labelConfirm;

		private TextBox textBoxConfirmPassword;

		private CheckBox checkBoxExportWebDisplay;

		private ProjectExporterParameters Parameters
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
				return 2u;
			}
		}

		public ProjectExport(ProjectExporterParameters parameters, bool exportToDeviceEnabled)
		{
			this.InitializeComponent();
			this.Parameters = parameters;
			this.Init(exportToDeviceEnabled);
		}

		public void Init(bool exportToDeviceEnabled)
		{
			this.titledGroupMemoryCard.Visible = exportToDeviceEnabled;
			if (this.Parameters != null)
			{
				this.checkBoxExportToMemoryCard.Checked = this.Parameters.ExportToMemoryCard;
				this.checkBoxAskPriorToExport.Checked = this.Parameters.AskPriorToExport;
				this.checkBoxExportBusDatabases.Checked = this.Parameters.ExportBusDatabases;
				this.checkBoxExportCCPXCPDatabases.Checked = this.Parameters.ExportCCPXCPDatabases;
				this.checkBoxExportNonBusDatabases.Checked = this.Parameters.ExportNonBusDatabases;
				this.checkBoxExportDiagnosticDescriptions.Checked = this.Parameters.ExportDiagnosticDescriptions;
				this.checkBoxExportIncludeFiles.Checked = this.Parameters.ExportIncludeFiles;
				this.checkBoxExportWebDisplay.Checked = this.Parameters.ExportWebDisplay;
				this.checkBoxProtectWithPassword.Checked = this.Parameters.ProtectWithPassword;
				this.checkBoxSetPasswordOnDemand.Checked = this.Parameters.SetPasswordOnDemand;
				this.textBoxPassword.Text = this.Parameters.Password;
				this.textBoxConfirmPassword.Text = this.Parameters.Password;
				if (this.textBoxPassword.Text.Length == 0 && !this.checkBoxSetPasswordOnDemand.Checked)
				{
					this.checkBoxProtectWithPassword.Checked = false;
					this.Parameters.ProtectWithPassword = false;
				}
				this.checkBoxShowPassword.Checked = false;
				this.UpdateUI();
			}
		}

		public void UpdateUI()
		{
			this.checkBoxSetPasswordOnDemand.Enabled = this.checkBoxProtectWithPassword.Checked;
			this.labelPassword.Enabled = (this.checkBoxProtectWithPassword.Checked && !this.checkBoxSetPasswordOnDemand.Checked);
			this.labelConfirm.Enabled = (this.labelPassword.Enabled && !this.checkBoxShowPassword.Checked);
			this.textBoxConfirmPassword.Enabled = (this.labelPassword.Enabled && !this.checkBoxShowPassword.Checked);
			this.textBoxPassword.Enabled = (this.checkBoxProtectWithPassword.Checked && !this.checkBoxSetPasswordOnDemand.Checked);
			this.checkBoxShowPassword.Enabled = (this.checkBoxProtectWithPassword.Checked && !this.checkBoxSetPasswordOnDemand.Checked);
			this.checkBoxAskPriorToExport.Enabled = this.checkBoxExportToMemoryCard.Checked;
			this.textBoxPassword.UseSystemPasswordChar = !this.checkBoxShowPassword.Checked;
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
				this.Parameters.ExportBusDatabases = this.checkBoxExportBusDatabases.Checked;
				this.Parameters.ExportCCPXCPDatabases = this.checkBoxExportCCPXCPDatabases.Checked;
				this.Parameters.ExportNonBusDatabases = this.checkBoxExportNonBusDatabases.Checked;
				this.Parameters.ExportDiagnosticDescriptions = this.checkBoxExportDiagnosticDescriptions.Checked;
				this.Parameters.ExportIncludeFiles = this.checkBoxExportIncludeFiles.Checked;
				this.Parameters.ExportWebDisplay = this.checkBoxExportWebDisplay.Checked;
				this.Parameters.ProtectWithPassword = this.checkBoxProtectWithPassword.Checked;
				this.Parameters.SetPasswordOnDemand = this.checkBoxSetPasswordOnDemand.Checked;
				this.Parameters.Password = this.textBoxPassword.Text;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(ProjectExport));
			this.checkBoxShowPassword = new CheckBox();
			this.labelPassword = new Label();
			this.textBoxPassword = new TextBox();
			this.checkBoxSetPasswordOnDemand = new CheckBox();
			this.checkBoxProtectWithPassword = new CheckBox();
			this.checkBoxAskPriorToExport = new CheckBox();
			this.checkBoxExportToMemoryCard = new CheckBox();
			this.checkBoxExportIncludeFiles = new CheckBox();
			this.checkBoxExportDiagnosticDescriptions = new CheckBox();
			this.checkBoxExportNonBusDatabases = new CheckBox();
			this.checkBoxExportCCPXCPDatabases = new CheckBox();
			this.checkBoxExportBusDatabases = new CheckBox();
			this.titledGroupMemoryCard = new TitledGroup();
			this.titledGroupPassword = new TitledGroup();
			this.textBoxConfirmPassword = new TextBox();
			this.labelConfirm = new Label();
			this.titledGroupFiltering = new TitledGroup();
			this.checkBoxExportWebDisplay = new CheckBox();
			this.titledGroupMemoryCard.SuspendLayout();
			this.titledGroupPassword.SuspendLayout();
			this.titledGroupFiltering.SuspendLayout();
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
			componentResourceManager.ApplyResources(this.checkBoxExportIncludeFiles, "checkBoxExportIncludeFiles");
			this.checkBoxExportIncludeFiles.Checked = true;
			this.checkBoxExportIncludeFiles.CheckState = CheckState.Checked;
			this.checkBoxExportIncludeFiles.Name = "checkBoxExportIncludeFiles";
			this.checkBoxExportIncludeFiles.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.checkBoxExportDiagnosticDescriptions, "checkBoxExportDiagnosticDescriptions");
			this.checkBoxExportDiagnosticDescriptions.Checked = true;
			this.checkBoxExportDiagnosticDescriptions.CheckState = CheckState.Checked;
			this.checkBoxExportDiagnosticDescriptions.Name = "checkBoxExportDiagnosticDescriptions";
			this.checkBoxExportDiagnosticDescriptions.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.checkBoxExportNonBusDatabases, "checkBoxExportNonBusDatabases");
			this.checkBoxExportNonBusDatabases.Checked = true;
			this.checkBoxExportNonBusDatabases.CheckState = CheckState.Checked;
			this.checkBoxExportNonBusDatabases.Name = "checkBoxExportNonBusDatabases";
			this.checkBoxExportNonBusDatabases.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.checkBoxExportCCPXCPDatabases, "checkBoxExportCCPXCPDatabases");
			this.checkBoxExportCCPXCPDatabases.Checked = true;
			this.checkBoxExportCCPXCPDatabases.CheckState = CheckState.Checked;
			this.checkBoxExportCCPXCPDatabases.Name = "checkBoxExportCCPXCPDatabases";
			this.checkBoxExportCCPXCPDatabases.UseVisualStyleBackColor = true;
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
			this.titledGroupFiltering.Controls.Add(this.checkBoxExportWebDisplay);
			this.titledGroupFiltering.Controls.Add(this.checkBoxExportIncludeFiles);
			this.titledGroupFiltering.Controls.Add(this.checkBoxExportDiagnosticDescriptions);
			this.titledGroupFiltering.Controls.Add(this.checkBoxExportBusDatabases);
			this.titledGroupFiltering.Controls.Add(this.checkBoxExportNonBusDatabases);
			this.titledGroupFiltering.Controls.Add(this.checkBoxExportCCPXCPDatabases);
			this.titledGroupFiltering.Image = null;
			this.titledGroupFiltering.Name = "titledGroupFiltering";
			componentResourceManager.ApplyResources(this.checkBoxExportWebDisplay, "checkBoxExportWebDisplay");
			this.checkBoxExportWebDisplay.Checked = true;
			this.checkBoxExportWebDisplay.CheckState = CheckState.Checked;
			this.checkBoxExportWebDisplay.Name = "checkBoxExportWebDisplay";
			this.checkBoxExportWebDisplay.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.titledGroupFiltering);
			base.Controls.Add(this.titledGroupPassword);
			base.Controls.Add(this.titledGroupMemoryCard);
			base.Name = "ProjectExport";
			this.titledGroupMemoryCard.ResumeLayout(false);
			this.titledGroupMemoryCard.PerformLayout();
			this.titledGroupPassword.ResumeLayout(false);
			this.titledGroupPassword.PerformLayout();
			this.titledGroupFiltering.ResumeLayout(false);
			this.titledGroupFiltering.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
