using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Vector.ConfigurationDialog;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.LoggerSpecifics;

namespace Vector.VLConfig.GUI.Options
{
	public class GeneratedFiles : ConfigurationPageView, IConfigurationPageController, IDisposable
	{
		private readonly GlobalOptions mGlobalOptions;

		private IContainer components;

		private TitledGroup titledGroup1;

		private Label label2;

		private RadioButton mRadioButtonGPSOtimizedForCANape;

		private Label label1;

		private RadioButton mRadioButtonGPSStandardDBC;

		private Label label3;

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
				return 4u;
			}
		}

		public GeneratedFiles(GlobalOptions globalOptions, LoggerType loggerType)
		{
			this.mGlobalOptions = globalOptions;
			this.InitializeComponent();
		}

		public void CancelPageData()
		{
		}

		public void OnViewClosed()
		{
		}

		public bool OnViewClosing(DialogResult closingReason)
		{
			return this.mRadioButtonGPSStandardDBC.Checked || this.mRadioButtonGPSOtimizedForCANape.Checked;
		}

		public void OnViewOpened()
		{
		}

		public void OnViewOpening()
		{
			this.mRadioButtonGPSStandardDBC.Checked = !this.mGlobalOptions.GenerateCANapeDBC;
			this.mRadioButtonGPSOtimizedForCANape.Checked = this.mGlobalOptions.GenerateCANapeDBC;
		}

		public bool SavePageData()
		{
			if (this.mGlobalOptions.GenerateCANapeDBC == this.mRadioButtonGPSOtimizedForCANape.Checked)
			{
				return false;
			}
			this.mGlobalOptions.GenerateCANapeDBC = this.mRadioButtonGPSOtimizedForCANape.Checked;
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(GeneratedFiles));
			this.titledGroup1 = new TitledGroup();
			this.label3 = new Label();
			this.label2 = new Label();
			this.mRadioButtonGPSOtimizedForCANape = new RadioButton();
			this.label1 = new Label();
			this.mRadioButtonGPSStandardDBC = new RadioButton();
			this.titledGroup1.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.titledGroup1, "titledGroup1");
			this.titledGroup1.AutoSizeGroup = true;
			this.titledGroup1.BackColor = SystemColors.Window;
			this.titledGroup1.Controls.Add(this.label3);
			this.titledGroup1.Controls.Add(this.label2);
			this.titledGroup1.Controls.Add(this.mRadioButtonGPSOtimizedForCANape);
			this.titledGroup1.Controls.Add(this.label1);
			this.titledGroup1.Controls.Add(this.mRadioButtonGPSStandardDBC);
			this.titledGroup1.Image = null;
			this.titledGroup1.Name = "titledGroup1";
			componentResourceManager.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			componentResourceManager.ApplyResources(this.mRadioButtonGPSOtimizedForCANape, "mRadioButtonGPSOtimizedForCANape");
			this.mRadioButtonGPSOtimizedForCANape.Name = "mRadioButtonGPSOtimizedForCANape";
			this.mRadioButtonGPSOtimizedForCANape.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.mRadioButtonGPSStandardDBC, "mRadioButtonGPSStandardDBC");
			this.mRadioButtonGPSStandardDBC.Name = "mRadioButtonGPSStandardDBC";
			this.mRadioButtonGPSStandardDBC.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.titledGroup1);
			base.Name = "GeneratedFiles";
			this.titledGroup1.ResumeLayout(false);
			this.titledGroup1.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
