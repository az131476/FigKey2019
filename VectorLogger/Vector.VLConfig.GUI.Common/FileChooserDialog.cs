using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Vector.VLConfig.GeneralUtil;

namespace Vector.VLConfig.GUI.Common
{
	public class FileChooserDialog : Form
	{
		private IContainer components;

		private ListBox listBoxFiles;

		private Label labelMessage;

		private Button buttonCancel;

		private Button buttonOK;

		public string Message
		{
			set
			{
				this.labelMessage.Text = value;
			}
		}

		public string SelectedItem
		{
			get
			{
				if (this.listBoxFiles.SelectedItem != null)
				{
					return this.listBoxFiles.SelectedItem.ToString();
				}
				return string.Empty;
			}
		}

		public FileChooserDialog()
		{
			this.InitializeComponent();
		}

		public void AddItems(IList<string> entryList)
		{
			this.listBoxFiles.Items.AddRange(entryList.ToArray<string>());
			if (this.listBoxFiles.Items.Count > 0)
			{
				this.listBoxFiles.SelectedIndex = 0;
			}
		}

		private void listBoxFiles_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			if (!string.IsNullOrEmpty(this.SelectedItem))
			{
				FileSystemServices.LaunchDirectoryBrowser(this.SelectedItem);
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
			this.listBoxFiles = new ListBox();
			this.labelMessage = new Label();
			this.buttonCancel = new Button();
			this.buttonOK = new Button();
			base.SuspendLayout();
			this.listBoxFiles.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
			this.listBoxFiles.FormattingEnabled = true;
			this.listBoxFiles.HorizontalScrollbar = true;
			this.listBoxFiles.Location = new Point(12, 41);
			this.listBoxFiles.Name = "listBoxFiles";
			this.listBoxFiles.Size = new Size(560, 173);
			this.listBoxFiles.TabIndex = 1;
			this.listBoxFiles.MouseDoubleClick += new MouseEventHandler(this.listBoxFiles_MouseDoubleClick);
			this.labelMessage.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);
			this.labelMessage.AutoEllipsis = true;
			this.labelMessage.AutoSize = true;
			this.labelMessage.Location = new Point(13, 13);
			this.labelMessage.MaximumSize = new Size(560, 0);
			this.labelMessage.Name = "labelMessage";
			this.labelMessage.Size = new Size(35, 13);
			this.labelMessage.TabIndex = 2;
			this.labelMessage.Text = "label1";
			this.buttonCancel.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
			this.buttonCancel.DialogResult = DialogResult.Cancel;
			this.buttonCancel.Location = new Point(497, 227);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new Size(75, 23);
			this.buttonCancel.TabIndex = 3;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonOK.Anchor = (AnchorStyles.Bottom | AnchorStyles.Right);
			this.buttonOK.DialogResult = DialogResult.OK;
			this.buttonOK.Location = new Point(416, 227);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new Size(75, 23);
			this.buttonOK.TabIndex = 4;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ClientSize = new Size(584, 284);
			base.ControlBox = false;
			base.Controls.Add(this.buttonOK);
			base.Controls.Add(this.buttonCancel);
			base.Controls.Add(this.labelMessage);
			base.Controls.Add(this.listBoxFiles);
			base.MaximizeBox = false;
			this.MaximumSize = new Size(900, 600);
			base.MinimizeBox = false;
			this.MinimumSize = new Size(600, 300);
			base.Name = "FileChooserDialog";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.StartPosition = FormStartPosition.CenterParent;
			this.Text = "FileChooserDialog";
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
