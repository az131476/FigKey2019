using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Vector.ConfigurationDialog;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.Options
{
	public class General : ConfigurationPageView, IConfigurationPageController, IDisposable
	{
		private bool isStandaloneMode;

		private IContainer components;

		private ComboBox comboBoxLogger;

		private Label label1;

		private TitledGroup titledGroup4;

		private TitledGroup titledGroupLanguage;

		private RadioButton radioButtonEnglish;

		private RadioButton radioButtonGerman;

		private TitledGroup titledGroup1;

		private CheckBox mCheckBoxCaplCompliance;

		public Array ComboxBoxItems
		{
			get
			{
				return Enum.GetValues(typeof(LoggerType));
			}
		}

		private GlobalOptions GlobalOptions
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
				return 0u;
			}
		}

		public General(GlobalOptions globalOptions, bool standaloneMode)
		{
			this.InitializeComponent();
			Array values = Enum.GetValues(typeof(LoggerType));
			foreach (LoggerType loggerType in values)
			{
				if (loggerType != LoggerType.Unknown)
				{
					this.comboBoxLogger.Items.Add(GUIUtil.MapLoggerType2String(loggerType));
				}
			}
			this.GlobalOptions = globalOptions;
			this.isStandaloneMode = standaloneMode;
			this.Init();
		}

		public void Init()
		{
			if (this.GlobalOptions != null)
			{
				this.comboBoxLogger.SelectedItem = GUIUtil.MapLoggerType2String(this.GlobalOptions.LoggerTypeToStartWith);
				if (this.GlobalOptions.Language == GlobalOptions.Languages.German)
				{
					this.radioButtonGerman.Checked = true;
				}
				else
				{
					this.radioButtonEnglish.Checked = true;
				}
				this.mCheckBoxCaplCompliance.Checked = this.GlobalOptions.CaplComplianceTest;
				if (this.isStandaloneMode)
				{
					this.HideOptionsInStandaloneMode();
				}
			}
			this.UpdateUI();
		}

		public void UpdateUI()
		{
			this.Refresh();
		}

		public bool CheckOptions()
		{
			return true;
		}

		public void UpdateOptions()
		{
			this.GlobalOptions.LoggerTypeToStartWith = GUIUtil.MapString2LoggerType(this.comboBoxLogger.SelectedItem.ToString());
			GlobalOptions.Languages language = this.GlobalOptions.Language;
			if (this.radioButtonGerman.Checked)
			{
				this.GlobalOptions.Language = GlobalOptions.Languages.German;
			}
			else
			{
				this.GlobalOptions.Language = GlobalOptions.Languages.English;
			}
			this.GlobalOptions.CaplComplianceTest = this.mCheckBoxCaplCompliance.Checked;
			if (language != this.GlobalOptions.Language)
			{
				InformMessageBox.Info(Resources.OptionInfoLanguageChange);
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
			if (this.CheckOptions())
			{
				this.UpdateOptions();
				return true;
			}
			return false;
		}

		public void CancelPageData()
		{
		}

		public bool OnViewClosing(DialogResult closingReason)
		{
			return true;
		}

		public bool OnViewClosing()
		{
			return true;
		}

		public void HideOptionsInStandaloneMode()
		{
			this.titledGroup1.Visible = false;
			this.titledGroup4.Visible = false;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(General));
			this.comboBoxLogger = new ComboBox();
			this.label1 = new Label();
			this.titledGroup4 = new TitledGroup();
			this.titledGroupLanguage = new TitledGroup();
			this.radioButtonEnglish = new RadioButton();
			this.radioButtonGerman = new RadioButton();
			this.titledGroup1 = new TitledGroup();
			this.mCheckBoxCaplCompliance = new CheckBox();
			this.titledGroup4.SuspendLayout();
			this.titledGroupLanguage.SuspendLayout();
			this.titledGroup1.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.comboBoxLogger, "comboBoxLogger");
			this.comboBoxLogger.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxLogger.FormattingEnabled = true;
			this.comboBoxLogger.Name = "comboBoxLogger";
			this.comboBoxLogger.Tag = "";
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			this.titledGroup4.AllowDrop = true;
			componentResourceManager.ApplyResources(this.titledGroup4, "titledGroup4");
			this.titledGroup4.AutoSizeGroup = true;
			this.titledGroup4.BackColor = SystemColors.Window;
			this.titledGroup4.Controls.Add(this.comboBoxLogger);
			this.titledGroup4.Controls.Add(this.label1);
			this.titledGroup4.Image = null;
			this.titledGroup4.Name = "titledGroup4";
			this.titledGroupLanguage.AllowDrop = true;
			componentResourceManager.ApplyResources(this.titledGroupLanguage, "titledGroupLanguage");
			this.titledGroupLanguage.AutoSizeGroup = true;
			this.titledGroupLanguage.BackColor = SystemColors.Window;
			this.titledGroupLanguage.Controls.Add(this.radioButtonEnglish);
			this.titledGroupLanguage.Controls.Add(this.radioButtonGerman);
			this.titledGroupLanguage.Image = null;
			this.titledGroupLanguage.Name = "titledGroupLanguage";
			componentResourceManager.ApplyResources(this.radioButtonEnglish, "radioButtonEnglish");
			this.radioButtonEnglish.Name = "radioButtonEnglish";
			this.radioButtonEnglish.TabStop = true;
			this.radioButtonEnglish.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.radioButtonGerman, "radioButtonGerman");
			this.radioButtonGerman.Name = "radioButtonGerman";
			this.radioButtonGerman.TabStop = true;
			this.radioButtonGerman.UseVisualStyleBackColor = true;
			this.titledGroup1.AllowDrop = true;
			componentResourceManager.ApplyResources(this.titledGroup1, "titledGroup1");
			this.titledGroup1.AutoSizeGroup = true;
			this.titledGroup1.BackColor = SystemColors.Window;
			this.titledGroup1.Controls.Add(this.mCheckBoxCaplCompliance);
			this.titledGroup1.Image = null;
			this.titledGroup1.Name = "titledGroup1";
			componentResourceManager.ApplyResources(this.mCheckBoxCaplCompliance, "mCheckBoxCaplCompliance");
			this.mCheckBoxCaplCompliance.Name = "mCheckBoxCaplCompliance";
			this.mCheckBoxCaplCompliance.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.titledGroup1);
			base.Controls.Add(this.titledGroupLanguage);
			base.Controls.Add(this.titledGroup4);
			base.Name = "General";
			this.titledGroup4.ResumeLayout(false);
			this.titledGroup4.PerformLayout();
			this.titledGroupLanguage.ResumeLayout(false);
			this.titledGroupLanguage.PerformLayout();
			this.titledGroup1.ResumeLayout(false);
			this.titledGroup1.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
