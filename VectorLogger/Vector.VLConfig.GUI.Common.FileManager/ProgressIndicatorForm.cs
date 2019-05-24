using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.HardwareAccess.ToolInterfaces;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.Common.FileManager
{
	public class ProgressIndicatorForm : Form, IProgressIndicator
	{
		private delegate void SetTextCallback(string text);

		private delegate void SetValueCallback(int value);

		private delegate void SetMinMaxCallback(int min, int max);

		private delegate void CloseCallback();

		private bool cancelled;

		private bool cancelRequested;

		private IContainer components;

		private ProgressBar progressBar;

		private Button btnCancel;

		private TextBox tbStatusText;

		public bool ConfirmCancel
		{
			get;
			set;
		}

		public ProgressIndicatorForm()
		{
			this.InitializeComponent();
			this.cancelled = false;
			this.cancelRequested = false;
			this.ConfirmCancel = false;
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
				ProgressIndicatorForm.CloseCallback method = new ProgressIndicatorForm.CloseCallback(base.Close);
				base.Invoke(method);
				return;
			}
			base.Close();
			TaskbarProgress.SetState(TaskbarProgress.TaskbarStates.NoProgress);
		}

		private void ProgressIndicatorForm_HelpRequested(object sender, HelpEventArgs hlpevent)
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
			Monitor.Enter(this);
			this.progressBar.Minimum = min;
			this.progressBar.Maximum = max;
			TaskbarProgress.SetState(TaskbarProgress.TaskbarStates.NoProgress);
			Monitor.Exit(this);
		}

		public void SetValue(int value)
		{
			if (this.progressBar.InvokeRequired)
			{
				ProgressIndicatorForm.SetValueCallback method = new ProgressIndicatorForm.SetValueCallback(this.SetValue);
				base.Invoke(method, new object[]
				{
					value
				});
				return;
			}
			if (value >= 0)
			{
				if (this.progressBar.Style != ProgressBarStyle.Continuous)
				{
					this.progressBar.Style = ProgressBarStyle.Continuous;
					TaskbarProgress.SetState(TaskbarProgress.TaskbarStates.Normal);
				}
				this.progressBar.Value = value;
				TaskbarProgress.SetValue((double)value, (double)(this.progressBar.Maximum - this.progressBar.Minimum));
				return;
			}
			this.progressBar.Style = ProgressBarStyle.Marquee;
			TaskbarProgress.SetState(TaskbarProgress.TaskbarStates.Indeterminate);
		}

		public void SetMinMaxInvoked(int min, int max)
		{
			if (this.progressBar.InvokeRequired)
			{
				ProgressIndicatorForm.SetMinMaxCallback method = new ProgressIndicatorForm.SetMinMaxCallback(this.SetMinMax);
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
				ProgressIndicatorForm.SetTextCallback method = new ProgressIndicatorForm.SetTextCallback(this.SetStatusText);
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

		private void ProgressIndicatorForm_FormClosing(object sender, FormClosingEventArgs e)
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(ProgressIndicatorForm));
			this.progressBar = new ProgressBar();
			this.btnCancel = new Button();
			this.tbStatusText = new TextBox();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.progressBar, "progressBar");
			this.progressBar.Name = "progressBar";
			this.progressBar.Style = ProgressBarStyle.Continuous;
			this.btnCancel.DialogResult = DialogResult.Cancel;
			componentResourceManager.ApplyResources(this.btnCancel, "btnCancel");
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new EventHandler(this.btnCancel_Click);
			componentResourceManager.ApplyResources(this.tbStatusText, "tbStatusText");
			this.tbStatusText.Name = "tbStatusText";
			this.tbStatusText.ReadOnly = true;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.CancelButton = this.btnCancel;
			base.ControlBox = false;
			base.Controls.Add(this.tbStatusText);
			base.Controls.Add(this.btnCancel);
			base.Controls.Add(this.progressBar);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "ProgressIndicatorForm";
			base.ShowInTaskbar = false;
			base.FormClosing += new FormClosingEventHandler(this.ProgressIndicatorForm_FormClosing);
			base.HelpRequested += new HelpEventHandler(this.ProgressIndicatorForm_HelpRequested);
			base.ResumeLayout(false);
			base.PerformLayout();
		}

		void IProgressIndicator.Activate()
		{
			base.Activate();
		}
	}
}
