using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Vector.VLConfig.GUI.Common
{
	public class DisplayReportWithQuestion : Form
	{
		private IContainer components;

		private RichTextBox richTextBoxReport;

		private Button buttonYes;

		private Label labelHeadline;

		private Button buttonNo;

		private Label labelQuestion;

		public static DialogResult ShowDisplayReportDialog(string windowTitle, string headline, string reportText, string question, bool isRtf)
		{
			DisplayReportWithQuestion displayReportWithQuestion = new DisplayReportWithQuestion();
			displayReportWithQuestion.Text = windowTitle;
			displayReportWithQuestion.labelHeadline.Text = headline;
			displayReportWithQuestion.labelQuestion.Text = question;
			if (isRtf)
			{
				displayReportWithQuestion.richTextBoxReport.Rtf = reportText;
			}
			else
			{
				displayReportWithQuestion.richTextBoxReport.Text = reportText;
			}
			return displayReportWithQuestion.ShowDialog();
		}

		private DisplayReportWithQuestion()
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(DisplayReportWithQuestion));
			this.richTextBoxReport = new RichTextBox();
			this.buttonYes = new Button();
			this.labelHeadline = new Label();
			this.buttonNo = new Button();
			this.labelQuestion = new Label();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.richTextBoxReport, "richTextBoxReport");
			this.richTextBoxReport.BorderStyle = BorderStyle.FixedSingle;
			this.richTextBoxReport.Name = "richTextBoxReport";
			this.richTextBoxReport.ReadOnly = true;
			componentResourceManager.ApplyResources(this.buttonYes, "buttonYes");
			this.buttonYes.DialogResult = DialogResult.Yes;
			this.buttonYes.Name = "buttonYes";
			this.buttonYes.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.labelHeadline, "labelHeadline");
			this.labelHeadline.Name = "labelHeadline";
			componentResourceManager.ApplyResources(this.buttonNo, "buttonNo");
			this.buttonNo.DialogResult = DialogResult.No;
			this.buttonNo.Name = "buttonNo";
			this.buttonNo.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.labelQuestion, "labelQuestion");
			this.labelQuestion.Name = "labelQuestion";
			base.AcceptButton = this.buttonYes;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.CancelButton = this.buttonNo;
			base.Controls.Add(this.labelQuestion);
			base.Controls.Add(this.buttonNo);
			base.Controls.Add(this.labelHeadline);
			base.Controls.Add(this.buttonYes);
			base.Controls.Add(this.richTextBoxReport);
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "DisplayReportWithQuestion";
			base.HelpRequested += new HelpEventHandler(this.DisplayReport_HelpRequested);
			base.ResumeLayout(false);
		}
	}
}
