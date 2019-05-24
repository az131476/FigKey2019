using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Vector.VLConfig.GUI.CLFExportPage
{
	public class MetaInfoDialog : Form
	{
		private IContainer components;

		private Button buttonClose;

		private RichTextBox textBoxMetaInfo;

		public MetaInfoDialog(string metaInfo)
		{
			this.InitializeComponent();
			string text = "{\\rtf1\\ansi\\ansicpg1252 ";
			string[] array = metaInfo.Split(new string[]
			{
				"\n",
				"\r\n"
			}, StringSplitOptions.RemoveEmptyEntries);
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string text2 = array2[i];
				string text3 = null;
				int num = text2.IndexOf(':');
				string text4;
				if (num > 0)
				{
					text3 = text2.Substring(0, num + 1);
					text4 = text2.Substring(num + 1);
				}
				else
				{
					text4 = text2;
				}
				if (text3 != null)
				{
					text += "\\b ";
					text += text3;
					text += "\\b0 ";
				}
				if (text4 != null)
				{
					text += text4.Replace("\\", "\\\\");
				}
				text += "\\par ";
			}
			text += "}";
			this.textBoxMetaInfo.Rtf = text;
		}

		private void buttonClose_Click(object sender, EventArgs e)
		{
			base.Hide();
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(MetaInfoDialog));
			this.buttonClose = new Button();
			this.textBoxMetaInfo = new RichTextBox();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.buttonClose, "buttonClose");
			this.buttonClose.Name = "buttonClose";
			this.buttonClose.UseVisualStyleBackColor = true;
			this.buttonClose.Click += new EventHandler(this.buttonClose_Click);
			this.textBoxMetaInfo.BackColor = SystemColors.ControlLightLight;
			this.textBoxMetaInfo.BorderStyle = BorderStyle.FixedSingle;
			componentResourceManager.ApplyResources(this.textBoxMetaInfo, "textBoxMetaInfo");
			this.textBoxMetaInfo.Name = "textBoxMetaInfo";
			this.textBoxMetaInfo.ReadOnly = true;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.textBoxMetaInfo);
			base.Controls.Add(this.buttonClose);
			base.FormBorderStyle = FormBorderStyle.FixedSingle;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "MetaInfoDialog";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.ResumeLayout(false);
		}
	}
}
