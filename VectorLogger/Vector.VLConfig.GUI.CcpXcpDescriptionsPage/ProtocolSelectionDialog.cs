using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Vector.McModule;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.CcpXcpDescriptionsPage
{
	public class ProtocolSelectionDialog : Form
	{
		private IList<IDeviceInfo> mDeviceInfoList;

		private IDeviceInfo mDeviceInfoVxDefault;

		private IContainer components;

		private Button mButtonCancel;

		private Button mButtonOk;

		private ComboBox mComboBoxProtocols;

		private Label label1;

		private CheckBox mCheckBoxUseVxModule;

		private Button mButtonHelp;

		public IDeviceInfo SelectedDeviceInfo
		{
			get
			{
				if (!(this.mComboBoxProtocols.SelectedItem is DeviceInfoItem))
				{
					return null;
				}
				return (this.mComboBoxProtocols.SelectedItem as DeviceInfoItem).DeviceInfo;
			}
		}

		public bool UseVxModule
		{
			get
			{
				return this.mCheckBoxUseVxModule.Checked;
			}
		}

		public ProtocolSelectionDialog(IList<IDeviceInfo> deviceInfoList, IDeviceInfo deviceInfoVxDefault, bool databaseHasDeviceInfo)
		{
			this.InitializeComponent();
			this.mDeviceInfoList = deviceInfoList;
			this.mDeviceInfoVxDefault = deviceInfoVxDefault;
			if (this.mDeviceInfoVxDefault == null)
			{
				this.mCheckBoxUseVxModule.Checked = false;
				this.mCheckBoxUseVxModule.Enabled = false;
			}
			if (!databaseHasDeviceInfo)
			{
				this.label1.Text = Resources.WarningDatabaseContainsNoTransportProtocol + " " + this.label1.Text;
				base.Size = new Size(base.Size.Width, base.Size.Height + 15);
			}
			this.FillComboBox();
		}

		private void FillComboBox()
		{
			IDeviceInfo deviceInfo = null;
			if (this.mComboBoxProtocols.SelectedItem is DeviceInfoItem)
			{
				deviceInfo = (this.mComboBoxProtocols.SelectedItem as DeviceInfoItem).DeviceInfo;
			}
			this.mComboBoxProtocols.Items.Clear();
			if (this.mCheckBoxUseVxModule.Checked && this.mDeviceInfoVxDefault != null)
			{
				this.mComboBoxProtocols.Items.Add(new DeviceInfoItem(this.mDeviceInfoVxDefault));
			}
			foreach (IDeviceInfo current in this.mDeviceInfoList)
			{
				if (!this.mCheckBoxUseVxModule.Checked || (this.mCheckBoxUseVxModule.Checked && current.TransportType == EnumXcpTransportType.kUdp))
				{
					this.mComboBoxProtocols.Items.Add(new DeviceInfoItem(current));
				}
			}
			if (this.mComboBoxProtocols.Items.Count <= 0)
			{
				this.mComboBoxProtocols.Enabled = false;
				return;
			}
			object obj = null;
			if (deviceInfo != null)
			{
				foreach (object current2 in this.mComboBoxProtocols.Items)
				{
					if (current2 is DeviceInfoItem && (current2 as DeviceInfoItem).DeviceInfo == deviceInfo)
					{
						obj = current2;
						break;
					}
				}
			}
			if (obj == null)
			{
				this.mComboBoxProtocols.SelectedIndex = 0;
				return;
			}
			this.mComboBoxProtocols.SelectedIndex = this.mComboBoxProtocols.Items.IndexOf(obj);
		}

		private void mCheckBoxUseVxModule_CheckedChanged(object sender, EventArgs e)
		{
			this.FillComboBox();
		}

		private void mButtonHelp_Click(object sender, EventArgs e)
		{
			MainWindow.ShowHelpForDialog(this);
		}

		private void ProtocolSelectionDialog_HelpRequested(object sender, HelpEventArgs hlpevent)
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(ProtocolSelectionDialog));
			this.mButtonCancel = new Button();
			this.mButtonOk = new Button();
			this.mComboBoxProtocols = new ComboBox();
			this.label1 = new Label();
			this.mCheckBoxUseVxModule = new CheckBox();
			this.mButtonHelp = new Button();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.mButtonCancel, "mButtonCancel");
			this.mButtonCancel.DialogResult = DialogResult.Cancel;
			this.mButtonCancel.Name = "mButtonCancel";
			this.mButtonCancel.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.mButtonOk, "mButtonOk");
			this.mButtonOk.DialogResult = DialogResult.OK;
			this.mButtonOk.Name = "mButtonOk";
			this.mButtonOk.UseVisualStyleBackColor = true;
			componentResourceManager.ApplyResources(this.mComboBoxProtocols, "mComboBoxProtocols");
			this.mComboBoxProtocols.DropDownStyle = ComboBoxStyle.DropDownList;
			this.mComboBoxProtocols.FormattingEnabled = true;
			this.mComboBoxProtocols.Name = "mComboBoxProtocols";
			componentResourceManager.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			componentResourceManager.ApplyResources(this.mCheckBoxUseVxModule, "mCheckBoxUseVxModule");
			this.mCheckBoxUseVxModule.Name = "mCheckBoxUseVxModule";
			this.mCheckBoxUseVxModule.UseVisualStyleBackColor = true;
			this.mCheckBoxUseVxModule.CheckedChanged += new EventHandler(this.mCheckBoxUseVxModule_CheckedChanged);
			componentResourceManager.ApplyResources(this.mButtonHelp, "mButtonHelp");
			this.mButtonHelp.Name = "mButtonHelp";
			this.mButtonHelp.UseVisualStyleBackColor = true;
			this.mButtonHelp.Click += new EventHandler(this.mButtonHelp_Click);
			base.AcceptButton = this.mButtonOk;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.CancelButton = this.mButtonCancel;
			base.ControlBox = false;
			base.Controls.Add(this.mButtonHelp);
			base.Controls.Add(this.mCheckBoxUseVxModule);
			base.Controls.Add(this.label1);
			base.Controls.Add(this.mComboBoxProtocols);
			base.Controls.Add(this.mButtonOk);
			base.Controls.Add(this.mButtonCancel);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.MaximizeBox = false;
			base.MinimizeBox = false;
			base.Name = "ProtocolSelectionDialog";
			base.ShowIcon = false;
			base.ShowInTaskbar = false;
			base.HelpRequested += new HelpEventHandler(this.ProtocolSelectionDialog_HelpRequested);
			base.ResumeLayout(false);
			base.PerformLayout();
		}
	}
}
