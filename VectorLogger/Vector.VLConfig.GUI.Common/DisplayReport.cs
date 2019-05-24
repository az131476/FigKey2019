using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Vector.VLConfig.GUI.Common
{
	public class DisplayReport : Form
	{
		private IContainer components;

		private RichTextBox richTextBoxReport;

		private Button buttonOK;

		private Label labelHeadline;

		public static void ShowDisplayReportDialog(string windowTitle, string headline, string reportText, bool isRtf)
		{
			DisplayReport displayReport = new DisplayReport();
			displayReport.Text = windowTitle;
			displayReport.labelHeadline.Text = headline;
			if (isRtf)
			{
				displayReport.richTextBoxReport.Rtf = reportText;
			}
			else
			{
				displayReport.richTextBoxReport.Text = reportText;
			}
			displayReport.ShowDialog();
		}

		private DisplayReport()
		{
			this.InitializeComponent();
		}

		private void DisplayReport_HelpRequested(object sender, HelpEventArgs hlpevent)
		{
			MainWindow.ShowHelpForDialog(this);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(DisplayReport));
			this.richTextBoxReport = new RichTextBox();
			this.buttonOK = new Button();
			this.labelHeadline = new Label();
			base.SuspendLayout();
			this.richTextBoxReport.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.richTextBoxReport.BorderStyle = BorderStyle.FixedSingle;
			this.richTextBoxReport.Location = new Point(12, 33);
			this.richTextBoxReport.Name = "richTextBoxReport";
			this.richTextBoxReport.ReadOnly = true;
			this.richTextBoxReport.ScrollBars = RichTextBoxScrollBars.Vertical;
			this.richTextBoxReport.Size = new Size(760, 158);
			this.richTextBoxReport.TabIndex = 0;
			this.richTextBoxReport.Text = "";
			this.buttonOK.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
			this.buttonOK.DialogResult = DialogResult.Cancel;
			this.buttonOK.Location = new Point(697, 197);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new Size(75, 23);
			this.buttonOK.TabIndex = 1;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.labelHeadline.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.labelHeadline.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Bold, GraphicsUnit.Point, 0);
			this.labelHeadline.Location = new Point(9, 9);
			this.labelHeadline.Name = "labelHeadline";
			this.labelHeadline.Size = new Size(763, 21);
			this.labelHeadline.TabIndex = 2;
			this.labelHeadline.Text = "labelHeadline";
			base.AcceptButton = this.buttonOK;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.CancelButton = this.buttonOK;
			base.ClientSize = new Size(784, 232);
			base.Controls.Add(this.labelHeadline);
			base.Controls.Add(this.buttonOK);
			base.Controls.Add(this.richTextBoxReport);
			this.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			base.Icon = (Icon)componentResourceManager.GetObject("$this.Icon");
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			this.MinimumSize = new Size(600, 200);
			base.Name = "DisplayReport";
			base.StartPosition = FormStartPosition.CenterParent;
			this.Text = "DisplayReport";
			base.HelpRequested += new HelpEventHandler(this.DisplayReport_HelpRequested);
			base.ResumeLayout(false);
		}
	}
}
