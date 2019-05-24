using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Vector.ConfigurationDialog;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.LoggerSpecifics;

namespace Vector.VLConfig.GUI.Options
{
	public class Conversion : ConfigurationPageView, IConfigurationPageController, IDisposable
	{
		private IContainer components;

		private TableLayoutPanel tableLayoutPanel1;

		private ConversionProfiles conversionProfiles;

		private TitledGroup titledGroupConversionOptions;

		private CheckBox checkBoxSortMDF3Files;

		private ComboBox comboBoxMDFCompression;

		private Label labelMDFCompression;

		private TitledGroup titledGroup1;

		private Label labelHDF5Compression;

		private ComboBox comboBoxHDF5Compression;

		private Label label2;

		private ComboBox comboBoxSignalNameLength;

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
				return 3u;
			}
		}

		public Conversion(LoggerType loggerType)
		{
			this.InitializeComponent();
			this.conversionProfiles.LoggerType = loggerType;
		}

		public void CancelPageData()
		{
			this.conversionProfiles.Cancel();
		}

		public void OnViewClosed()
		{
		}

		public bool OnViewClosing(DialogResult closingReason)
		{
			return true;
		}

		public void OnViewOpened()
		{
		}

		public void OnViewOpening()
		{
			this.conversionProfiles.Init();
			this.checkBoxSortMDF3Files.Checked = GlobalOptionsManager.GlobalOptions.SortMDF3Files;
			if (GlobalOptionsManager.GlobalOptions.MDFCompressionLevel >= 0 && GlobalOptionsManager.GlobalOptions.MDFCompressionLevel <= 3)
			{
				this.comboBoxMDFCompression.SelectedIndex = GlobalOptionsManager.GlobalOptions.MDFCompressionLevel;
			}
			else
			{
				this.comboBoxMDFCompression.SelectedIndex = 0;
			}
			if (GlobalOptionsManager.GlobalOptions.HDF5CompressionLevel >= 0 && GlobalOptionsManager.GlobalOptions.HDF5CompressionLevel <= 3)
			{
				this.comboBoxHDF5Compression.SelectedIndex = GlobalOptionsManager.GlobalOptions.HDF5CompressionLevel;
			}
			else
			{
				this.comboBoxHDF5Compression.SelectedIndex = 0;
			}
			this.comboBoxSignalNameLength.SelectedIndex = (GlobalOptionsManager.GlobalOptions.HDF5WriteUnlimitedSignalNames ? 1 : 0);
		}

		public bool SavePageData()
		{
			this.conversionProfiles.Save();
			GlobalOptionsManager.GlobalOptions.SortMDF3Files = this.checkBoxSortMDF3Files.Checked;
			GlobalOptionsManager.GlobalOptions.MDFCompressionLevel = ((this.comboBoxMDFCompression.SelectedIndex >= 0) ? this.comboBoxMDFCompression.SelectedIndex : 0);
			GlobalOptionsManager.GlobalOptions.HDF5CompressionLevel = ((this.comboBoxHDF5Compression.SelectedIndex >= 0) ? this.comboBoxHDF5Compression.SelectedIndex : 0);
			GlobalOptionsManager.GlobalOptions.HDF5WriteUnlimitedSignalNames = (this.comboBoxSignalNameLength.SelectedIndex == 1);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(Conversion));
			this.tableLayoutPanel1 = new TableLayoutPanel();
			this.titledGroupConversionOptions = new TitledGroup();
			this.labelMDFCompression = new Label();
			this.comboBoxMDFCompression = new ComboBox();
			this.checkBoxSortMDF3Files = new CheckBox();
			this.conversionProfiles = new ConversionProfiles();
			this.titledGroup1 = new TitledGroup();
			this.label2 = new Label();
			this.comboBoxSignalNameLength = new ComboBox();
			this.labelHDF5Compression = new Label();
			this.comboBoxHDF5Compression = new ComboBox();
			this.tableLayoutPanel1.SuspendLayout();
			this.titledGroupConversionOptions.SuspendLayout();
			this.titledGroup1.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.titledGroupConversionOptions, 0, 1);
			this.tableLayoutPanel1.Controls.Add(this.conversionProfiles, 0, 0);
			this.tableLayoutPanel1.Controls.Add(this.titledGroup1, 0, 2);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			this.titledGroupConversionOptions.AllowDrop = true;
			this.titledGroupConversionOptions.AutoSizeGroup = true;
			this.titledGroupConversionOptions.BackColor = SystemColors.Window;
			this.titledGroupConversionOptions.Controls.Add(this.labelMDFCompression);
			this.titledGroupConversionOptions.Controls.Add(this.comboBoxMDFCompression);
			this.titledGroupConversionOptions.Controls.Add(this.checkBoxSortMDF3Files);
			componentResourceManager.ApplyResources(this.titledGroupConversionOptions, "titledGroupConversionOptions");
			this.titledGroupConversionOptions.Image = null;
			this.titledGroupConversionOptions.Name = "titledGroupConversionOptions";
			componentResourceManager.ApplyResources(this.labelMDFCompression, "labelMDFCompression");
			this.labelMDFCompression.Name = "labelMDFCompression";
			this.comboBoxMDFCompression.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxMDFCompression.FormattingEnabled = true;
			this.comboBoxMDFCompression.Items.AddRange(new object[]
			{
				componentResourceManager.GetString("comboBoxMDFCompression.Items"),
				componentResourceManager.GetString("comboBoxMDFCompression.Items1"),
				componentResourceManager.GetString("comboBoxMDFCompression.Items2"),
				componentResourceManager.GetString("comboBoxMDFCompression.Items3")
			});
			componentResourceManager.ApplyResources(this.comboBoxMDFCompression, "comboBoxMDFCompression");
			this.comboBoxMDFCompression.Name = "comboBoxMDFCompression";
			componentResourceManager.ApplyResources(this.checkBoxSortMDF3Files, "checkBoxSortMDF3Files");
			this.checkBoxSortMDF3Files.Name = "checkBoxSortMDF3Files";
			this.checkBoxSortMDF3Files.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.conversionProfiles, "conversionProfiles");
			this.conversionProfiles.BackColor = SystemColors.Window;
			this.conversionProfiles.Name = "conversionProfiles";
			componentResourceManager.ApplyResources(this.titledGroup1, "titledGroup1");
			this.titledGroup1.AutoSizeGroup = true;
			this.titledGroup1.BackColor = SystemColors.Window;
			this.titledGroup1.Controls.Add(this.label2);
			this.titledGroup1.Controls.Add(this.comboBoxSignalNameLength);
			this.titledGroup1.Controls.Add(this.labelHDF5Compression);
			this.titledGroup1.Controls.Add(this.comboBoxHDF5Compression);
			this.titledGroup1.Image = null;
			this.titledGroup1.Name = "titledGroup1";
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			this.comboBoxSignalNameLength.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxSignalNameLength.FormattingEnabled = true;
			this.comboBoxSignalNameLength.Items.AddRange(new object[]
			{
				componentResourceManager.GetString("comboBoxSignalNameLength.Items"),
				componentResourceManager.GetString("comboBoxSignalNameLength.Items1")
			});
			componentResourceManager.ApplyResources(this.comboBoxSignalNameLength, "comboBoxSignalNameLength");
			this.comboBoxSignalNameLength.Name = "comboBoxSignalNameLength";
			componentResourceManager.ApplyResources(this.labelHDF5Compression, "labelHDF5Compression");
			this.labelHDF5Compression.Name = "labelHDF5Compression";
			this.comboBoxHDF5Compression.DropDownStyle = ComboBoxStyle.DropDownList;
			this.comboBoxHDF5Compression.FormattingEnabled = true;
			this.comboBoxHDF5Compression.Items.AddRange(new object[]
			{
				componentResourceManager.GetString("comboBoxHDF5Compression.Items"),
				componentResourceManager.GetString("comboBoxHDF5Compression.Items1"),
				componentResourceManager.GetString("comboBoxHDF5Compression.Items2"),
				componentResourceManager.GetString("comboBoxHDF5Compression.Items3")
			});
			componentResourceManager.ApplyResources(this.comboBoxHDF5Compression, "comboBoxHDF5Compression");
			this.comboBoxHDF5Compression.Name = "comboBoxHDF5Compression";
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.tableLayoutPanel1);
			base.Name = "Conversion";
			this.tableLayoutPanel1.ResumeLayout(false);
			this.titledGroupConversionOptions.ResumeLayout(false);
			this.titledGroupConversionOptions.PerformLayout();
			this.titledGroup1.ResumeLayout(false);
			this.titledGroup1.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
