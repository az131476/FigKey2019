using System;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Windows.Forms;
using Vector.ConfigurationDialog;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.GeneralUtil.QuickView;
using Vector.VLConfig.GUI.Common.QuickView;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.Options
{
	public class QuickViewOptions : ConfigurationPageView, IConfigurationPageController, IDisposable
	{
		private class ToolComboboxItem
		{
			public QuickViewTool QuickViewTool
			{
				get;
				private set;
			}

			public ToolComboboxItem(QuickViewTool quickViewTool)
			{
				this.QuickViewTool = quickViewTool;
			}

			public override string ToString()
			{
				if (this.QuickViewTool == QuickViewTool.Unspecified)
				{
					return Resources.SplitButtonEmptyDefault;
				}
				return this.QuickViewTool.ToString();
			}
		}

		private IContainer components;

		private TitledGroup titledGroup1;

		private Button mButtonSelectUserDefinedTemplateCANoe;

		private TextBox mTextBoxUserDefinedTemplateCANoe;

		private RadioButton mRadioButtonUseUserDefinedTemplateCANoe;

		private RadioButton mRadioButtonUseDefaultTemplatesCANoe;

		private Button mButtonSelectDefaultGenerationFolder;

		private TextBox mTextBoxGenerationFolder;

		private Label mLabelGenerationFolder;

		private Label label2;

		private TextBox mTextBoxMaxPlaybackDuration;

		private Label label1;

		private Label label3;

		private ComboBox mComboBoxDefaultApplication;

		private Button mButtonSelectUserDefinedTemplatevSignalyzer;

		private TextBox mTextBoxUserDefinedTemplatevSignalyzer;

		private RadioButton mRadioButtonUseUserDefinedTemplatevSignalyzer;

		private RadioButton mRadioButtonUseDefaultTemplatesvSignalyzer;

		private Button mButtonSelectUserDefinedTemplateCANape;

		private TextBox mTextBoxUserDefinedTemplateCANape;

		private RadioButton mRadioButtonUseUserDefinedTemplateCANape;

		private RadioButton mRadioButtonUseDefaultTemplatesCANalyzer;

		private Button mButtonSelectUserDefinedTemplateCANalyzer;

		private TextBox mTextBoxUserDefinedTemplateCANalyzer;

		private RadioButton mRadioButtonUseUserDefinedTemplateCANalyzer;

		private RadioButton mRadioButtonUseDefaultTemplatesCANape;

		private Panel mPanelCANoe;

		private Panel mPanelCANalyzer;

		private Panel mPanelCANape;

		private Panel mPanelvSignalyzer;

		private GlobalOptions GlobalOptions
		{
			get
			{
				return GlobalOptionsManager.GlobalOptions;
			}
		}

		public uint PageID
		{
			get
			{
				return 6u;
			}
		}

		public uint HelpID
		{
			get
			{
				return (uint)GUIUtil.HelpPageID_OptionsDialog;
			}
		}

		public Control ConfigurationPageView
		{
			get
			{
				return this;
			}
		}

		public QuickViewOptions()
		{
			this.InitializeComponent();
			this.Init();
		}

		public void Init()
		{
			this.InitComboboxDefaultApplication();
			this.mTextBoxGenerationFolder.Text = (string.IsNullOrEmpty(this.GlobalOptions.GenerationFolder) ? QuickView.DefaultTempFolder : this.GlobalOptions.GenerationFolder);
			this.mTextBoxUserDefinedTemplateCANoe.Text = this.GlobalOptions.UserDefinedTemplateCANoe;
			this.mRadioButtonUseDefaultTemplatesCANoe.Checked = !this.GlobalOptions.UseUserDefinedTemplateCANoe;
			this.mRadioButtonUseUserDefinedTemplateCANoe.Checked = this.GlobalOptions.UseUserDefinedTemplateCANoe;
			this.mTextBoxUserDefinedTemplateCANalyzer.Text = this.GlobalOptions.UserDefinedTemplateCANalyzer;
			this.mRadioButtonUseDefaultTemplatesCANalyzer.Checked = !this.GlobalOptions.UseUserDefinedTemplateCANalyzer;
			this.mRadioButtonUseUserDefinedTemplateCANalyzer.Checked = this.GlobalOptions.UseUserDefinedTemplateCANalyzer;
			this.mTextBoxUserDefinedTemplateCANape.Text = this.GlobalOptions.UserDefinedTemplateCANape;
			this.mRadioButtonUseDefaultTemplatesCANape.Checked = !this.GlobalOptions.UseUserDefinedTemplateCANape;
			this.mRadioButtonUseUserDefinedTemplateCANape.Checked = this.GlobalOptions.UseUserDefinedTemplateCANape;
			this.mTextBoxUserDefinedTemplatevSignalyzer.Text = this.GlobalOptions.UserDefinedTemplatevSignalyzer;
			this.mRadioButtonUseDefaultTemplatesvSignalyzer.Checked = !this.GlobalOptions.UseUserDefinedTemplatevSignalyzer;
			this.mRadioButtonUseUserDefinedTemplatevSignalyzer.Checked = this.GlobalOptions.UseUserDefinedTemplatevSignalyzer;
			this.mTextBoxMaxPlaybackDuration.Text = this.GlobalOptions.MaxPlaybackDuration.ToString(CultureInfo.InvariantCulture);
			this.UpdateControls();
		}

		public void InitComboboxDefaultApplication()
		{
			this.mComboBoxDefaultApplication.Items.Clear();
			foreach (QuickViewTool quickViewTool in Enum.GetValues(typeof(QuickViewTool)))
			{
				this.mComboBoxDefaultApplication.Items.Add(new QuickViewOptions.ToolComboboxItem(quickViewTool));
			}
			object obj = null;
			foreach (object current in this.mComboBoxDefaultApplication.Items)
			{
				if (current is QuickViewOptions.ToolComboboxItem && (current as QuickViewOptions.ToolComboboxItem).QuickViewTool == this.GlobalOptions.QuickViewTool)
				{
					obj = current;
				}
			}
			if (obj != null)
			{
				this.mComboBoxDefaultApplication.SelectedItem = obj;
				return;
			}
			this.mComboBoxDefaultApplication.SelectedIndex = 0;
		}

		public bool SavePageData()
		{
			QuickViewOptions.ToolComboboxItem toolComboboxItem = this.mComboBoxDefaultApplication.SelectedItem as QuickViewOptions.ToolComboboxItem;
			if (toolComboboxItem == null)
			{
				return false;
			}
			uint num = uint.Parse(this.mTextBoxMaxPlaybackDuration.Text);
			if (this.GlobalOptions.QuickViewTool == toolComboboxItem.QuickViewTool && this.GlobalOptions.GenerationFolder.Equals(this.mTextBoxGenerationFolder.Text) && this.GlobalOptions.UserDefinedTemplateCANoe.Equals(this.mTextBoxUserDefinedTemplateCANoe.Text) && this.GlobalOptions.UseUserDefinedTemplateCANoe == this.mRadioButtonUseUserDefinedTemplateCANoe.Checked && this.GlobalOptions.UserDefinedTemplateCANalyzer.Equals(this.mTextBoxUserDefinedTemplateCANalyzer.Text) && this.GlobalOptions.UseUserDefinedTemplateCANalyzer == this.mRadioButtonUseUserDefinedTemplateCANalyzer.Checked && this.GlobalOptions.UserDefinedTemplateCANape.Equals(this.mTextBoxUserDefinedTemplateCANape.Text) && this.GlobalOptions.UseUserDefinedTemplateCANape == this.mRadioButtonUseUserDefinedTemplateCANape.Checked && this.GlobalOptions.UserDefinedTemplatevSignalyzer.Equals(this.mTextBoxUserDefinedTemplatevSignalyzer.Text) && this.GlobalOptions.UseUserDefinedTemplatevSignalyzer == this.mRadioButtonUseUserDefinedTemplatevSignalyzer.Checked && this.GlobalOptions.MaxPlaybackDuration == num)
			{
				return false;
			}
			this.GlobalOptions.QuickViewTool = toolComboboxItem.QuickViewTool;
			this.GlobalOptions.GenerationFolder = this.mTextBoxGenerationFolder.Text;
			this.GlobalOptions.UserDefinedTemplateCANoe = this.mTextBoxUserDefinedTemplateCANoe.Text;
			this.GlobalOptions.UseUserDefinedTemplateCANoe = this.mRadioButtonUseUserDefinedTemplateCANoe.Checked;
			this.GlobalOptions.UserDefinedTemplateCANalyzer = this.mTextBoxUserDefinedTemplateCANalyzer.Text;
			this.GlobalOptions.UseUserDefinedTemplateCANalyzer = this.mRadioButtonUseUserDefinedTemplateCANalyzer.Checked;
			this.GlobalOptions.UserDefinedTemplateCANape = this.mTextBoxUserDefinedTemplateCANape.Text;
			this.GlobalOptions.UseUserDefinedTemplateCANape = this.mRadioButtonUseUserDefinedTemplateCANape.Checked;
			this.GlobalOptions.UserDefinedTemplatevSignalyzer = this.mTextBoxUserDefinedTemplatevSignalyzer.Text;
			this.GlobalOptions.UseUserDefinedTemplatevSignalyzer = this.mRadioButtonUseUserDefinedTemplatevSignalyzer.Checked;
			this.GlobalOptions.MaxPlaybackDuration = num;
			QuickView.OnGlobalOptionsChanged();
			return true;
		}

		public void CancelPageData()
		{
		}

		public void OnViewOpening()
		{
		}

		public void OnViewOpened()
		{
		}

		public bool OnViewClosing(DialogResult closingReason)
		{
			if (closingReason != DialogResult.OK && closingReason != DialogResult.None)
			{
				return true;
			}
			try
			{
				new DirectoryInfo(this.mTextBoxGenerationFolder.Text);
			}
			catch
			{
				this.mTextBoxGenerationFolder.Text = QuickView.DefaultTempFolder;
			}
			if (this.mRadioButtonUseUserDefinedTemplateCANoe.Checked && !File.Exists(this.mTextBoxUserDefinedTemplateCANoe.Text))
			{
				this.mRadioButtonUseDefaultTemplatesCANoe.Checked = true;
			}
			if (this.mRadioButtonUseUserDefinedTemplateCANalyzer.Checked && !File.Exists(this.mTextBoxUserDefinedTemplateCANalyzer.Text))
			{
				this.mRadioButtonUseDefaultTemplatesCANalyzer.Checked = true;
			}
			if (this.mRadioButtonUseUserDefinedTemplateCANape.Checked && !File.Exists(this.mTextBoxUserDefinedTemplateCANape.Text))
			{
				this.mRadioButtonUseDefaultTemplatesCANape.Checked = true;
			}
			if (this.mRadioButtonUseUserDefinedTemplatevSignalyzer.Checked && !File.Exists(this.mTextBoxUserDefinedTemplatevSignalyzer.Text))
			{
				this.mRadioButtonUseDefaultTemplatesvSignalyzer.Checked = true;
			}
			uint num;
			if (!uint.TryParse(this.mTextBoxMaxPlaybackDuration.Text, out num) || num < 1u)
			{
				this.mTextBoxMaxPlaybackDuration.Text = OfflineSourceConfig.DefaultPlaybackDuration.ToString(CultureInfo.InvariantCulture);
			}
			this.SavePageData();
			return true;
		}

		public void OnViewClosed()
		{
		}

		public bool IsVisibleForLoggerType(LoggerType loggerType)
		{
			return loggerType == LoggerType.GL2000 || loggerType == LoggerType.GL3000 || loggerType == LoggerType.GL4000;
		}

		private void ButtonSelectDefaultGenerationFolder_Click(object sender, EventArgs e)
		{
			using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
			{
				folderBrowserDialog.SelectedPath = this.mTextBoxGenerationFolder.Text;
				if (DialogResult.OK == folderBrowserDialog.ShowDialog(this))
				{
					this.mTextBoxGenerationFolder.Text = folderBrowserDialog.SelectedPath;
				}
			}
		}

		private void ButtonSelectUserDefinedTemplate_Click(object sender, EventArgs e)
		{
			switch (this.GetQuickViewTool())
			{
			case QuickViewTool.CANoe:
				this.ButtonSelectUserDefinedTemplate_ClickInternal(this.mTextBoxUserDefinedTemplateCANoe, Resources_Files.FileFilterCANoeTemplates);
				return;
			case QuickViewTool.CANalyzer:
				this.ButtonSelectUserDefinedTemplate_ClickInternal(this.mTextBoxUserDefinedTemplateCANalyzer, Resources_Files.FileFilterCANalyzerTemplates);
				return;
			case QuickViewTool.CANape:
				this.ButtonSelectUserDefinedTemplate_ClickInternal(this.mTextBoxUserDefinedTemplateCANape, Resources_Files.FileFilterCANapeProjects);
				return;
			case QuickViewTool.vSignalyzer:
				this.ButtonSelectUserDefinedTemplate_ClickInternal(this.mTextBoxUserDefinedTemplatevSignalyzer, Resources_Files.FileFiltervSignalyzerProjects);
				return;
			default:
				return;
			}
		}

		private void ButtonSelectUserDefinedTemplate_ClickInternal(TextBox textBox, string fileFilter)
		{
			string initialDirectory;
			try
			{
				FileInfo fileInfo = new FileInfo(textBox.Text);
				if (fileInfo.Directory == null)
				{
					initialDirectory = string.Empty;
				}
				else
				{
					initialDirectory = fileInfo.Directory.FullName;
				}
			}
			catch
			{
				initialDirectory = string.Empty;
			}
			using (OpenFileDialog openFileDialog = new OpenFileDialog())
			{
				openFileDialog.InitialDirectory = initialDirectory;
				openFileDialog.Multiselect = false;
				openFileDialog.Filter = fileFilter;
				if (DialogResult.OK == openFileDialog.ShowDialog(this))
				{
					textBox.Text = openFileDialog.FileName;
				}
			}
		}

		private void RadioButtonUseDefaultTemplatesCANoe_CheckedChanged(object sender, EventArgs e)
		{
			this.UpdateControls();
		}

		private void RadioButtonUseDefaultTemplatesvSignalyzer_CheckedChanged(object sender, EventArgs e)
		{
			this.UpdateControls();
		}

		private void RadioButtonUseDefaultTemplatesCANape_CheckedChanged(object sender, EventArgs e)
		{
			this.UpdateControls();
		}

		private void RadioButtonUseDefaultTemplatesCANalyzer_CheckedChanged(object sender, EventArgs e)
		{
			this.UpdateControls();
		}

		private void ComboBoxDefaultApplication_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.ShowHideControls();
			this.UpdateControls();
		}

		private QuickViewTool GetQuickViewTool()
		{
			QuickViewOptions.ToolComboboxItem toolComboboxItem = this.mComboBoxDefaultApplication.SelectedItem as QuickViewOptions.ToolComboboxItem;
			if (toolComboboxItem == null)
			{
				return QuickViewTool.Unspecified;
			}
			return toolComboboxItem.QuickViewTool;
		}

		private void ShowHideControls()
		{
			QuickViewTool quickViewTool = this.GetQuickViewTool();
			this.mPanelCANoe.Visible = (quickViewTool == QuickViewTool.CANoe);
			this.mPanelCANalyzer.Visible = (quickViewTool == QuickViewTool.CANalyzer);
			this.mPanelCANape.Visible = (quickViewTool == QuickViewTool.CANape);
			this.mPanelvSignalyzer.Visible = (quickViewTool == QuickViewTool.vSignalyzer);
		}

		private void UpdateControls()
		{
			switch (this.GetQuickViewTool())
			{
			case QuickViewTool.CANoe:
			{
				bool @checked = this.mRadioButtonUseUserDefinedTemplateCANoe.Checked;
				this.mTextBoxUserDefinedTemplateCANoe.Enabled = @checked;
				this.mButtonSelectUserDefinedTemplateCANoe.Enabled = @checked;
				return;
			}
			case QuickViewTool.CANalyzer:
			{
				bool checked2 = this.mRadioButtonUseUserDefinedTemplateCANalyzer.Checked;
				this.mTextBoxUserDefinedTemplateCANalyzer.Enabled = checked2;
				this.mButtonSelectUserDefinedTemplateCANalyzer.Enabled = checked2;
				return;
			}
			case QuickViewTool.CANape:
			{
				bool checked3 = this.mRadioButtonUseUserDefinedTemplateCANape.Checked;
				this.mTextBoxUserDefinedTemplateCANape.Enabled = checked3;
				this.mButtonSelectUserDefinedTemplateCANape.Enabled = checked3;
				return;
			}
			case QuickViewTool.vSignalyzer:
			{
				bool checked4 = this.mRadioButtonUseUserDefinedTemplatevSignalyzer.Checked;
				this.mTextBoxUserDefinedTemplatevSignalyzer.Enabled = checked4;
				this.mButtonSelectUserDefinedTemplatevSignalyzer.Enabled = checked4;
				return;
			}
			default:
				return;
			}
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(QuickViewOptions));
			this.titledGroup1 = new TitledGroup();
			this.mPanelCANoe = new Panel();
			this.mRadioButtonUseDefaultTemplatesCANoe = new RadioButton();
			this.mRadioButtonUseUserDefinedTemplateCANoe = new RadioButton();
			this.mTextBoxUserDefinedTemplateCANoe = new TextBox();
			this.mButtonSelectUserDefinedTemplateCANoe = new Button();
			this.mPanelCANalyzer = new Panel();
			this.mRadioButtonUseDefaultTemplatesCANalyzer = new RadioButton();
			this.mRadioButtonUseUserDefinedTemplateCANalyzer = new RadioButton();
			this.mTextBoxUserDefinedTemplateCANalyzer = new TextBox();
			this.mButtonSelectUserDefinedTemplateCANalyzer = new Button();
			this.mPanelCANape = new Panel();
			this.mRadioButtonUseDefaultTemplatesCANape = new RadioButton();
			this.mRadioButtonUseUserDefinedTemplateCANape = new RadioButton();
			this.mTextBoxUserDefinedTemplateCANape = new TextBox();
			this.mButtonSelectUserDefinedTemplateCANape = new Button();
			this.mPanelvSignalyzer = new Panel();
			this.mRadioButtonUseDefaultTemplatesvSignalyzer = new RadioButton();
			this.mRadioButtonUseUserDefinedTemplatevSignalyzer = new RadioButton();
			this.mTextBoxUserDefinedTemplatevSignalyzer = new TextBox();
			this.mButtonSelectUserDefinedTemplatevSignalyzer = new Button();
			this.label3 = new Label();
			this.mComboBoxDefaultApplication = new ComboBox();
			this.label2 = new Label();
			this.mTextBoxMaxPlaybackDuration = new TextBox();
			this.label1 = new Label();
			this.mButtonSelectDefaultGenerationFolder = new Button();
			this.mTextBoxGenerationFolder = new TextBox();
			this.mLabelGenerationFolder = new Label();
			this.titledGroup1.SuspendLayout();
			this.mPanelCANoe.SuspendLayout();
			this.mPanelCANalyzer.SuspendLayout();
			this.mPanelCANape.SuspendLayout();
			this.mPanelvSignalyzer.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.titledGroup1, "titledGroup1");
			this.titledGroup1.AutoSizeGroup = true;
			this.titledGroup1.BackColor = SystemColors.Window;
			this.titledGroup1.Controls.Add(this.mPanelCANoe);
			this.titledGroup1.Controls.Add(this.mPanelCANalyzer);
			this.titledGroup1.Controls.Add(this.mPanelCANape);
			this.titledGroup1.Controls.Add(this.mPanelvSignalyzer);
			this.titledGroup1.Controls.Add(this.label3);
			this.titledGroup1.Controls.Add(this.mComboBoxDefaultApplication);
			this.titledGroup1.Controls.Add(this.label2);
			this.titledGroup1.Controls.Add(this.mTextBoxMaxPlaybackDuration);
			this.titledGroup1.Controls.Add(this.label1);
			this.titledGroup1.Controls.Add(this.mButtonSelectDefaultGenerationFolder);
			this.titledGroup1.Controls.Add(this.mTextBoxGenerationFolder);
			this.titledGroup1.Controls.Add(this.mLabelGenerationFolder);
			this.titledGroup1.Image = null;
			this.titledGroup1.Name = "titledGroup1";
			componentResourceManager.ApplyResources(this.mPanelCANoe, "mPanelCANoe");
			this.mPanelCANoe.Controls.Add(this.mRadioButtonUseDefaultTemplatesCANoe);
			this.mPanelCANoe.Controls.Add(this.mRadioButtonUseUserDefinedTemplateCANoe);
			this.mPanelCANoe.Controls.Add(this.mTextBoxUserDefinedTemplateCANoe);
			this.mPanelCANoe.Controls.Add(this.mButtonSelectUserDefinedTemplateCANoe);
			this.mPanelCANoe.Name = "mPanelCANoe";
			componentResourceManager.ApplyResources(this.mRadioButtonUseDefaultTemplatesCANoe, "mRadioButtonUseDefaultTemplatesCANoe");
			this.mRadioButtonUseDefaultTemplatesCANoe.Name = "mRadioButtonUseDefaultTemplatesCANoe";
			this.mRadioButtonUseDefaultTemplatesCANoe.UseVisualStyleBackColor = true;
			this.mRadioButtonUseDefaultTemplatesCANoe.CheckedChanged += new EventHandler(this.RadioButtonUseDefaultTemplatesCANoe_CheckedChanged);
			componentResourceManager.ApplyResources(this.mRadioButtonUseUserDefinedTemplateCANoe, "mRadioButtonUseUserDefinedTemplateCANoe");
			this.mRadioButtonUseUserDefinedTemplateCANoe.Name = "mRadioButtonUseUserDefinedTemplateCANoe";
			this.mRadioButtonUseUserDefinedTemplateCANoe.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.mTextBoxUserDefinedTemplateCANoe, "mTextBoxUserDefinedTemplateCANoe");
			this.mTextBoxUserDefinedTemplateCANoe.Name = "mTextBoxUserDefinedTemplateCANoe";
			componentResourceManager.ApplyResources(this.mButtonSelectUserDefinedTemplateCANoe, "mButtonSelectUserDefinedTemplateCANoe");
			this.mButtonSelectUserDefinedTemplateCANoe.Name = "mButtonSelectUserDefinedTemplateCANoe";
			this.mButtonSelectUserDefinedTemplateCANoe.UseVisualStyleBackColor = true;
			this.mButtonSelectUserDefinedTemplateCANoe.Click += new EventHandler(this.ButtonSelectUserDefinedTemplate_Click);
			componentResourceManager.ApplyResources(this.mPanelCANalyzer, "mPanelCANalyzer");
			this.mPanelCANalyzer.Controls.Add(this.mRadioButtonUseDefaultTemplatesCANalyzer);
			this.mPanelCANalyzer.Controls.Add(this.mRadioButtonUseUserDefinedTemplateCANalyzer);
			this.mPanelCANalyzer.Controls.Add(this.mTextBoxUserDefinedTemplateCANalyzer);
			this.mPanelCANalyzer.Controls.Add(this.mButtonSelectUserDefinedTemplateCANalyzer);
			this.mPanelCANalyzer.Name = "mPanelCANalyzer";
			componentResourceManager.ApplyResources(this.mRadioButtonUseDefaultTemplatesCANalyzer, "mRadioButtonUseDefaultTemplatesCANalyzer");
			this.mRadioButtonUseDefaultTemplatesCANalyzer.Name = "mRadioButtonUseDefaultTemplatesCANalyzer";
			this.mRadioButtonUseDefaultTemplatesCANalyzer.UseVisualStyleBackColor = true;
			this.mRadioButtonUseDefaultTemplatesCANalyzer.CheckedChanged += new EventHandler(this.RadioButtonUseDefaultTemplatesCANalyzer_CheckedChanged);
			componentResourceManager.ApplyResources(this.mRadioButtonUseUserDefinedTemplateCANalyzer, "mRadioButtonUseUserDefinedTemplateCANalyzer");
			this.mRadioButtonUseUserDefinedTemplateCANalyzer.Name = "mRadioButtonUseUserDefinedTemplateCANalyzer";
			this.mRadioButtonUseUserDefinedTemplateCANalyzer.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.mTextBoxUserDefinedTemplateCANalyzer, "mTextBoxUserDefinedTemplateCANalyzer");
			this.mTextBoxUserDefinedTemplateCANalyzer.Name = "mTextBoxUserDefinedTemplateCANalyzer";
			componentResourceManager.ApplyResources(this.mButtonSelectUserDefinedTemplateCANalyzer, "mButtonSelectUserDefinedTemplateCANalyzer");
			this.mButtonSelectUserDefinedTemplateCANalyzer.Name = "mButtonSelectUserDefinedTemplateCANalyzer";
			this.mButtonSelectUserDefinedTemplateCANalyzer.UseVisualStyleBackColor = true;
			this.mButtonSelectUserDefinedTemplateCANalyzer.Click += new EventHandler(this.ButtonSelectUserDefinedTemplate_Click);
			componentResourceManager.ApplyResources(this.mPanelCANape, "mPanelCANape");
			this.mPanelCANape.Controls.Add(this.mRadioButtonUseDefaultTemplatesCANape);
			this.mPanelCANape.Controls.Add(this.mRadioButtonUseUserDefinedTemplateCANape);
			this.mPanelCANape.Controls.Add(this.mTextBoxUserDefinedTemplateCANape);
			this.mPanelCANape.Controls.Add(this.mButtonSelectUserDefinedTemplateCANape);
			this.mPanelCANape.Name = "mPanelCANape";
			componentResourceManager.ApplyResources(this.mRadioButtonUseDefaultTemplatesCANape, "mRadioButtonUseDefaultTemplatesCANape");
			this.mRadioButtonUseDefaultTemplatesCANape.Name = "mRadioButtonUseDefaultTemplatesCANape";
			this.mRadioButtonUseDefaultTemplatesCANape.UseVisualStyleBackColor = true;
			this.mRadioButtonUseDefaultTemplatesCANape.CheckedChanged += new EventHandler(this.RadioButtonUseDefaultTemplatesCANape_CheckedChanged);
			componentResourceManager.ApplyResources(this.mRadioButtonUseUserDefinedTemplateCANape, "mRadioButtonUseUserDefinedTemplateCANape");
			this.mRadioButtonUseUserDefinedTemplateCANape.Name = "mRadioButtonUseUserDefinedTemplateCANape";
			this.mRadioButtonUseUserDefinedTemplateCANape.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.mTextBoxUserDefinedTemplateCANape, "mTextBoxUserDefinedTemplateCANape");
			this.mTextBoxUserDefinedTemplateCANape.Name = "mTextBoxUserDefinedTemplateCANape";
			componentResourceManager.ApplyResources(this.mButtonSelectUserDefinedTemplateCANape, "mButtonSelectUserDefinedTemplateCANape");
			this.mButtonSelectUserDefinedTemplateCANape.Name = "mButtonSelectUserDefinedTemplateCANape";
			this.mButtonSelectUserDefinedTemplateCANape.UseVisualStyleBackColor = true;
			this.mButtonSelectUserDefinedTemplateCANape.Click += new EventHandler(this.ButtonSelectUserDefinedTemplate_Click);
			componentResourceManager.ApplyResources(this.mPanelvSignalyzer, "mPanelvSignalyzer");
			this.mPanelvSignalyzer.Controls.Add(this.mRadioButtonUseDefaultTemplatesvSignalyzer);
			this.mPanelvSignalyzer.Controls.Add(this.mRadioButtonUseUserDefinedTemplatevSignalyzer);
			this.mPanelvSignalyzer.Controls.Add(this.mTextBoxUserDefinedTemplatevSignalyzer);
			this.mPanelvSignalyzer.Controls.Add(this.mButtonSelectUserDefinedTemplatevSignalyzer);
			this.mPanelvSignalyzer.Name = "mPanelvSignalyzer";
			componentResourceManager.ApplyResources(this.mRadioButtonUseDefaultTemplatesvSignalyzer, "mRadioButtonUseDefaultTemplatesvSignalyzer");
			this.mRadioButtonUseDefaultTemplatesvSignalyzer.Name = "mRadioButtonUseDefaultTemplatesvSignalyzer";
			this.mRadioButtonUseDefaultTemplatesvSignalyzer.UseVisualStyleBackColor = true;
			this.mRadioButtonUseDefaultTemplatesvSignalyzer.CheckedChanged += new EventHandler(this.RadioButtonUseDefaultTemplatesvSignalyzer_CheckedChanged);
			componentResourceManager.ApplyResources(this.mRadioButtonUseUserDefinedTemplatevSignalyzer, "mRadioButtonUseUserDefinedTemplatevSignalyzer");
			this.mRadioButtonUseUserDefinedTemplatevSignalyzer.Name = "mRadioButtonUseUserDefinedTemplatevSignalyzer";
			this.mRadioButtonUseUserDefinedTemplatevSignalyzer.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.mTextBoxUserDefinedTemplatevSignalyzer, "mTextBoxUserDefinedTemplatevSignalyzer");
			this.mTextBoxUserDefinedTemplatevSignalyzer.Name = "mTextBoxUserDefinedTemplatevSignalyzer";
			componentResourceManager.ApplyResources(this.mButtonSelectUserDefinedTemplatevSignalyzer, "mButtonSelectUserDefinedTemplatevSignalyzer");
			this.mButtonSelectUserDefinedTemplatevSignalyzer.Name = "mButtonSelectUserDefinedTemplatevSignalyzer";
			this.mButtonSelectUserDefinedTemplatevSignalyzer.UseVisualStyleBackColor = true;
			this.mButtonSelectUserDefinedTemplatevSignalyzer.Click += new EventHandler(this.ButtonSelectUserDefinedTemplate_Click);
			componentResourceManager.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			this.mComboBoxDefaultApplication.DropDownStyle = ComboBoxStyle.DropDownList;
			this.mComboBoxDefaultApplication.FormattingEnabled = true;
			componentResourceManager.ApplyResources(this.mComboBoxDefaultApplication, "mComboBoxDefaultApplication");
			this.mComboBoxDefaultApplication.Name = "mComboBoxDefaultApplication";
			this.mComboBoxDefaultApplication.SelectedIndexChanged += new EventHandler(this.ComboBoxDefaultApplication_SelectedIndexChanged);
			componentResourceManager.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			componentResourceManager.ApplyResources(this.mTextBoxMaxPlaybackDuration, "mTextBoxMaxPlaybackDuration");
			this.mTextBoxMaxPlaybackDuration.Name = "mTextBoxMaxPlaybackDuration";
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.mButtonSelectDefaultGenerationFolder, "mButtonSelectDefaultGenerationFolder");
			this.mButtonSelectDefaultGenerationFolder.Name = "mButtonSelectDefaultGenerationFolder";
			this.mButtonSelectDefaultGenerationFolder.UseVisualStyleBackColor = true;
			this.mButtonSelectDefaultGenerationFolder.Click += new EventHandler(this.ButtonSelectDefaultGenerationFolder_Click);
			componentResourceManager.ApplyResources(this.mTextBoxGenerationFolder, "mTextBoxGenerationFolder");
			this.mTextBoxGenerationFolder.Name = "mTextBoxGenerationFolder";
			componentResourceManager.ApplyResources(this.mLabelGenerationFolder, "mLabelGenerationFolder");
			this.mLabelGenerationFolder.Name = "mLabelGenerationFolder";
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			this.BackColor = SystemColors.Window;
			base.Controls.Add(this.titledGroup1);
			base.Name = "QuickViewOptions";
			this.titledGroup1.ResumeLayout(false);
			this.titledGroup1.PerformLayout();
			this.mPanelCANoe.ResumeLayout(false);
			this.mPanelCANoe.PerformLayout();
			this.mPanelCANalyzer.ResumeLayout(false);
			this.mPanelCANalyzer.PerformLayout();
			this.mPanelCANape.ResumeLayout(false);
			this.mPanelCANape.PerformLayout();
			this.mPanelvSignalyzer.ResumeLayout(false);
			this.mPanelvSignalyzer.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
