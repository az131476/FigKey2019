using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.HardwareAccess.ToolInterfaces;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.Common.FileManager
{
	public class ActivityIndicatorForm : Form, IProgressIndicator
	{
		private delegate void SetTextCallback(string text);

		private delegate void SetValueCallback(int value);

		private delegate void SetMinMaxCallback(int min, int max);

		private delegate void CloseCallback();

		public const int DIALOG_DELAY = 500;

		private bool cancelled;

		private bool cancelRequested;

		private IContainer components;

		private ProgressBar progressBar;

		private TextBox tbStatusText;

		public bool ConfirmCancel
		{
			get;
			set;
		}

		public ActivityIndicatorForm()
		{
			this.InitializeComponent();
			this.cancelled = false;
			this.cancelRequested = false;
			this.ConfirmCancel = false;
			this.progressBar.Select();
			this.SetValue(-1);
		}

		private void btnCancel_Click(object sender, EventArgs e)
		{
			Monitor.Enter(this);
			if (this.ConfirmCancel)
			{
				this.cancelRequested = true;
			}
			else
			{
				this.cancelled = true;
			}
			Monitor.Exit(this);
		}

		public void ProcessExited()
		{
			if (base.InvokeRequired)
			{
				ActivityIndicatorForm.CloseCallback method = new ActivityIndicatorForm.CloseCallback(base.Close);
				base.Invoke(method);
				return;
			}
			base.Close();
			TaskbarProgress.SetState(TaskbarProgress.TaskbarStates.NoProgress);
		}

		private void ActivityIndicatorForm_HelpRequested(object sender, HelpEventArgs hlpevent)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		public void DisplayProgress()
		{
			base.Show();
		}

		public new void Deactivate()
		{
		}

		public void SetMinMax(int min, int max)
		{
		}

		public void SetValue(int value)
		{
			if (this.progressBar.InvokeRequired)
			{
				ActivityIndicatorForm.SetValueCallback method = new ActivityIndicatorForm.SetValueCallback(this.SetValue);
				base.Invoke(method, new object[]
				{
					value
				});
				return;
			}
			this.progressBar.Style = ProgressBarStyle.Marquee;
			TaskbarProgress.SetState(TaskbarProgress.TaskbarStates.Indeterminate);
		}

		public void SetMinMaxInvoked(int min, int max)
		{
			if (this.progressBar.InvokeRequired)
			{
				ActivityIndicatorForm.SetMinMaxCallback method = new ActivityIndicatorForm.SetMinMaxCallback(this.SetMinMax);
				base.Invoke(method, new object[]
				{
					min,
					max
				});
			}
		}

		public void SetStatusText(string text)
		{
			if (this.tbStatusText.InvokeRequired)
			{
				ActivityIndicatorForm.SetTextCallback method = new ActivityIndicatorForm.SetTextCallback(this.SetStatusText);
				base.Invoke(method, new object[]
				{
					text
				});
				return;
			}
			this.tbStatusText.Text = text;
		}

		public bool Cancelled()
		{
			Monitor.Enter(this);
			bool result = this.cancelled;
			Monitor.Exit(this);
			return result;
		}

		private void ActivityIndicatorForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			Monitor.Enter(this);
			if (this.ConfirmCancel && this.cancelRequested)
			{
				this.cancelRequested = false;
				if (DialogResult.Yes == InformMessageBox.Show(EnumQuestionType.Question, Resources.QuestionAreYouSureToCancel))
				{
					this.cancelled = true;
				}
				else
				{
					e.Cancel = true;
				}
			}
			if (!e.Cancel)
			{
				TaskbarProgress.SetState(TaskbarProgress.TaskbarStates.NoProgress);
			}
			Monitor.Exit(this);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(ActivityIndicatorForm));
			this.progressBar = new ProgressBar();
			this.tbStatusText = new TextBox();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.progressBar, "progressBar");
			this.progressBar.MarqueeAnimationSpeed = 50;
			this.progressBar.Maximum = 50;
			this.progressBar.Name = "progressBar";
			this.progressBar.Style = ProgressBarStyle.Marquee;
			componentResourceManager.ApplyResources(this.tbStatusText, "tbStatusText");
			this.tbStatusText.Name = "tbStatusText";
			this.tbStatusText.ReadOnly = true;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ControlBox = false;
			base.Controls.Add(this.tbStatusText);
			base.Controls.Add(this.progressBar);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "ActivityIndicatorForm";
			base.ShowInTaskbar = false;
			base.FormClosing += new FormClosingEventHandler(this.ActivityIndicatorForm_FormClosing);
			base.HelpRequested += new HelpEventHandler(this.ActivityIndicatorForm_HelpRequested);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		void IProgressIndicator.Activate()
		{
			base.Activate();
		}
	}
}
