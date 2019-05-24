using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Vector.VLConfig.GUI.FlexrayChannelsPage
{
	public class FlexrayChannelsGL1000 : UserControl
	{
		private IContainer components;

		private Label labelNotAvailable;

		public FlexrayChannelsGL1000()
		{
			this.InitializeComponent();
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(FlexrayChannelsGL1000));
			this.labelNotAvailable = new Label();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.labelNotAvailable, "labelNotAvailable");
			this.labelNotAvailable.Name = "labelNotAvailable";
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.labelNotAvailable);
			base.Name = "FlexrayChannelsGL1000";
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
