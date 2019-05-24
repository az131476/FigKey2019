using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.Common.DeviceInteraction
{
	public class WrittenToDialog : Form
	{
		private IContainer components;

		private PictureBox mPictureBoxIcon;

		private Button buttonOK;

		private Label mLabelSuccess;

		private CheckBox mCheckBoxEject;

		private TableLayoutPanel mTableLayoutPanel;

		private LinkLabel mLinkLabelAnalysisFolder;

		private Label mLabelAnalysisSuccess;

		private Label mLabelFiles;

		private Label mLabelPath;

		private RichTextBox mRichTextBoxFiles;

		public string SuccessMessage
		{
			get;
			set;
		}

		public string EjectOptionMessage
		{
			get;
			set;
		}

		public bool IsEjectEnabled
		{
			get;
			set;
		}

		public string AnalysisFolder
		{
			get;
			set;
		}

		public List<string> AnalysisFiles
		{
			get;
			set;
		}

		private bool IsAnalysisPackageWrittenToMemoryCard
		{
			get;
			set;
		}

		public bool IsEjectOptionEnabled
		{
			get
			{
				return !string.IsNullOrEmpty(this.EjectOptionMessage);
			}
		}

		public static void Display(string successMsg, string analysisFolder, List<string> analysisFiles, bool analysisPackageWrittenToMemoryCard)
		{
			bool flag = false;
			WrittenToDialog.DisplayWithEjectOption(successMsg, null, ref flag, analysisFolder, analysisFiles, analysisPackageWrittenToMemoryCard);
		}

		public static void DisplayWithEjectOption(string successMsg, string ejectOptionMessage, ref bool isEjectEnabled, string analysisFolder, List<string> analysisFiles, bool analysisPackageWrittenToMemoryCard)
		{
			using (WrittenToDialog writtenToDialog = new WrittenToDialog())
			{
				writtenToDialog.EjectOptionMessage = ejectOptionMessage;
				writtenToDialog.SuccessMessage = successMsg;
				writtenToDialog.AnalysisFolder = analysisFolder;
				writtenToDialog.AnalysisFiles = analysisFiles;
				writtenToDialog.IsAnalysisPackageWrittenToMemoryCard = analysisPackageWrittenToMemoryCard;
				writtenToDialog.IsEjectEnabled = isEjectEnabled;
				writtenToDialog.ShowDialog();
				isEjectEnabled = writtenToDialog.IsEjectEnabled;
			}
		}

		public WrittenToDialog()
		{
			this.InitializeComponent();
			this.mPictureBoxIcon.Image = SystemIcons.Information.ToBitmap();
		}

		private void WrittenToDialog_Load(object sender, EventArgs e)
		{
			Control control = this.mLabelSuccess;
			if (!string.IsNullOrEmpty(this.SuccessMessage))
			{
				this.mLabelSuccess.Text = this.SuccessMessage;
			}
			if (string.IsNullOrEmpty(this.AnalysisFolder) || this.AnalysisFiles == null || this.AnalysisFiles.Count < 1)
			{
				this.mLabelAnalysisSuccess.Hide();
				this.mLabelPath.Hide();
				this.mLinkLabelAnalysisFolder.Hide();
				this.mLabelFiles.Hide();
				this.mRichTextBoxFiles.Hide();
			}
			else
			{
				string text;
				if (this.IsAnalysisPackageWrittenToMemoryCard)
				{
					text = string.Format(Resources.AnalysisPackageCreatedAndWrittenToMemmoryCard, this.AnalysisFiles.Count);
				}
				else
				{
					text = string.Format(Resources.FilesGeneratedForAnalysis, this.AnalysisFiles.Count);
				}
				if (string.IsNullOrEmpty(this.SuccessMessage))
				{
					this.mLabelSuccess.Text = text;
					this.mLabelAnalysisSuccess.Hide();
				}
				else
				{
					this.mLabelAnalysisSuccess.Text = text;
				}
				this.mLinkLabelAnalysisFolder.Text = this.AnalysisFolder;
				StringBuilder stringBuilder = new StringBuilder();
				this.AnalysisFiles.Sort();
				foreach (string current in this.AnalysisFiles)
				{
					stringBuilder.AppendLine(Path.GetFileName(current));
				}
				this.mRichTextBoxFiles.Text = stringBuilder.ToString();
				control = this.mRichTextBoxFiles;
			}
			if (this.IsEjectOptionEnabled)
			{
				this.mCheckBoxEject.Text = this.EjectOptionMessage;
				this.mCheckBoxEject.Checked = this.IsEjectEnabled;
				control = this.mCheckBoxEject;
			}
			else
			{
				this.mCheckBoxEject.Hide();
			}
			int num = this.buttonOK.Top - control.Bottom - 20;
			base.Height -= num;
		}

		private void mCheckBoxEject_CheckedChanged(object sender, EventArgs e)
		{
			if (this.IsEjectOptionEnabled)
			{
				this.IsEjectEnabled = this.mCheckBoxEject.Checked;
			}
		}

		private void mLinkLabelAnalysisFolder_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			FileSystemServices.LaunchDirectoryBrowser(this.AnalysisFolder);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(WrittenToDialog));
			this.mPictureBoxIcon = new PictureBox();
			this.buttonOK = new Button();
			this.mLabelSuccess = new Label();
			this.mCheckBoxEject = new CheckBox();
			this.mTableLayoutPanel = new TableLayoutPanel();
			this.mLabelAnalysisSuccess = new Label();
			this.mLabelFiles = new Label();
			this.mLinkLabelAnalysisFolder = new LinkLabel();
			this.mLabelPath = new Label();
			this.mRichTextBoxFiles = new RichTextBox();
			((ISupportInitialize)this.mPictureBoxIcon).BeginInit();
			this.mTableLayoutPanel.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.mPictureBoxIcon, "mPictureBoxIcon");
			this.mPictureBoxIcon.Name = "mPictureBoxIcon";
			this.mPictureBoxIcon.TabStop = false;
			componentResourceManager.ApplyResources(this.buttonOK, "buttonOK");
			this.buttonOK.DialogResult = DialogResult.OK;
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.mLabelSuccess, "mLabelSuccess");
			this.mTableLayoutPanel.SetColumnSpan(this.mLabelSuccess, 2);
			this.mLabelSuccess.Name = "mLabelSuccess";
			componentResourceManager.ApplyResources(this.mCheckBoxEject, "mCheckBoxEject");
			this.mTableLayoutPanel.SetColumnSpan(this.mCheckBoxEject, 2);
			this.mCheckBoxEject.Name = "mCheckBoxEject";
			this.mCheckBoxEject.UseVisualStyleBackColor = true;
			this.mCheckBoxEject.CheckedChanged += new EventHandler(this.mCheckBoxEject_CheckedChanged);
			componentResourceManager.ApplyResources(this.mTableLayoutPanel, "mTableLayoutPanel");
			this.mTableLayoutPanel.Controls.Add(this.mLabelSuccess, 1, 0);
			this.mTableLayoutPanel.Controls.Add(this.mPictureBoxIcon, 0, 0);
			this.mTableLayoutPanel.Controls.Add(this.mLabelAnalysisSuccess, 1, 1);
			this.mTableLayoutPanel.Controls.Add(this.mLabelFiles, 1, 3);
			this.mTableLayoutPanel.Controls.Add(this.mLinkLabelAnalysisFolder, 2, 2);
			this.mTableLayoutPanel.Controls.Add(this.mLabelPath, 1, 2);
			this.mTableLayoutPanel.Controls.Add(this.mRichTextBoxFiles, 2, 3);
			this.mTableLayoutPanel.Controls.Add(this.mCheckBoxEject, 1, 4);
			this.mTableLayoutPanel.Name = "mTableLayoutPanel";
			componentResourceManager.ApplyResources(this.mLabelAnalysisSuccess, "mLabelAnalysisSuccess");
			this.mTableLayoutPanel.SetColumnSpan(this.mLabelAnalysisSuccess, 2);
			this.mLabelAnalysisSuccess.Name = "mLabelAnalysisSuccess";
			componentResourceManager.ApplyResources(this.mLabelFiles, "mLabelFiles");
			this.mLabelFiles.Name = "mLabelFiles";
			componentResourceManager.ApplyResources(this.mLinkLabelAnalysisFolder, "mLinkLabelAnalysisFolder");
			this.mLinkLabelAnalysisFolder.Name = "mLinkLabelAnalysisFolder";
			this.mLinkLabelAnalysisFolder.TabStop = true;
			this.mLinkLabelAnalysisFolder.LinkClicked += new LinkLabelLinkClickedEventHandler(this.mLinkLabelAnalysisFolder_LinkClicked);
			componentResourceManager.ApplyResources(this.mLabelPath, "mLabelPath");
			this.mLabelPath.Name = "mLabelPath";
			componentResourceManager.ApplyResources(this.mRichTextBoxFiles, "mRichTextBoxFiles");
			this.mRichTextBoxFiles.Name = "mRichTextBoxFiles";
			this.mRichTextBoxFiles.ReadOnly = true;
			base.AcceptButton = this.buttonOK;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			this.BackColor = SystemColors.Control;
			base.CancelButton = this.buttonOK;
			base.ControlBox = false;
			base.Controls.Add(this.mTableLayoutPanel);
			base.Controls.Add(this.buttonOK);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.HelpButton = true;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "WrittenToDialog";
			base.ShowInTaskbar = false;
			base.Load += new EventHandler(this.WrittenToDialog_Load);
			((ISupportInitialize)this.mPictureBoxIcon).EndInit();
			this.mTableLayoutPanel.ResumeLayout(false);
			this.mTableLayoutPanel.PerformLayout();
			base.ResumeLayout(false);
		}
	}
}
