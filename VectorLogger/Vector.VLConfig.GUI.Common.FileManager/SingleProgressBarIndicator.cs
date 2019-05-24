using System;
using System.Threading;
using System.Windows.Forms;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.HardwareAccess.ToolInterfaces;

namespace Vector.VLConfig.GUI.Common.FileManager
{
	public class SingleProgressBarIndicator : IProgressIndicator
	{
		private delegate void SetTextCallback(string text);

		private delegate void SetValueCallback(int value);

		private delegate void SetMinMaxCallback(int min, int max);

		private delegate void CloseCallback();

		private TextBox textBoxStatus;

		private ProgressBar progressBar;

		private Form parentForm;

		private bool useToolbarProgress;

		protected bool cancelled;

		public SingleProgressBarIndicator(Form parent, ref TextBox statusTextBox, ref ProgressBar bar, bool useToolbarToDisplayProgress = false)
		{
			this.parentForm = parent;
			this.textBoxStatus = statusTextBox;
			this.progressBar = bar;
			this.cancelled = false;
			this.useToolbarProgress = useToolbarToDisplayProgress;
		}

		public void ProcessExited()
		{
			if (this.parentForm.InvokeRequired)
			{
				SingleProgressBarIndicator.CloseCallback method = new SingleProgressBarIndicator.CloseCallback(this.parentForm.Close);
				this.parentForm.Invoke(method);
				return;
			}
			this.parentForm.Close();
		}

		public void Cancel()
		{
			this.cancelled = true;
		}

		public void DisplayProgress()
		{
			this.parentForm.Show();
		}

		public void Activate()
		{
		}

		public void Deactivate()
		{
		}

		public void SetMinMax(int min, int max)
		{
			if (this.progressBar.InvokeRequired)
			{
				SingleProgressBarIndicator.SetMinMaxCallback method = new SingleProgressBarIndicator.SetMinMaxCallback(this.SetMinMax);
				this.parentForm.Invoke(method, new object[]
				{
					min,
					max
				});
				return;
			}
			Monitor.Enter(this);
			this.progressBar.Minimum = min;
			this.progressBar.Maximum = max;
			Monitor.Exit(this);
		}

		public void SetValue(int value)
		{
			if (this.progressBar.InvokeRequired)
			{
				SingleProgressBarIndicator.SetValueCallback method = new SingleProgressBarIndicator.SetValueCallback(this.SetValue);
				this.parentForm.Invoke(method, new object[]
				{
					value
				});
				return;
			}
			Monitor.Enter(this);
			if (value <= this.progressBar.Maximum && value >= this.progressBar.Minimum)
			{
				this.progressBar.Value = value;
				if (this.useToolbarProgress)
				{
					TaskbarProgress.SetValue((double)value, (double)(this.progressBar.Maximum - this.progressBar.Minimum));
				}
			}
			Monitor.Exit(this);
		}

		public void SetStatusText(string text)
		{
			if (this.textBoxStatus.InvokeRequired)
			{
				SingleProgressBarIndicator.SetTextCallback method = new SingleProgressBarIndicator.SetTextCallback(this.SetStatusText);
				this.parentForm.Invoke(method, new object[]
				{
					text
				});
				return;
			}
			Monitor.Enter(this);
			this.textBoxStatus.Text = text;
			Monitor.Exit(this);
		}

		public bool Cancelled()
		{
			Monitor.Enter(this);
			bool result = this.cancelled;
			Monitor.Exit(this);
			return result;
		}
	}
}
