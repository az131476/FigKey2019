using System;
using System.ComponentModel;
using System.Windows.Forms;
using Vector.VLConfig.GUI.DeviceInformationPage;
using Vector.VLConfig.HardwareAccess.HardwareAbstraction;

namespace Vector.VLConfig.GUI.CardReaderPage
{
	public class CardReaderDeviceInformation : Form
	{
		private static CardReaderDeviceInformation _cardReaderDeviceInfo;

		private IContainer components;

		private Panel panelDeviceInfo;

		private DeviceInformationGL4000 deviceInformationGL4000;

		private Button buttonClose;

		private DeviceInformationGL3000 deviceInformationGL3000;

		private DeviceInformationGL1000 deviceInformationGL1000;

		private DeviceInformationGL1020FTE deviceInformationGL1020FTE;

		private DeviceInformationGL2000 deviceInformationGL2000;

		public ILoggerDevice Device
		{
			get;
			set;
		}

		public static void Display(ILoggerDevice device)
		{
			if (CardReaderDeviceInformation._cardReaderDeviceInfo == null)
			{
				CardReaderDeviceInformation._cardReaderDeviceInfo = new CardReaderDeviceInformation();
			}
			CardReaderDeviceInformation._cardReaderDeviceInfo.Device = device;
			CardReaderDeviceInformation._cardReaderDeviceInfo.ShowDialog();
		}

		public CardReaderDeviceInformation()
		{
			this.InitializeComponent();
			this.deviceInformationGL1000.IsLoggerConnectedDisplayMode = false;
			this.deviceInformationGL1020FTE.IsLoggerConnectedDisplayMode = false;
			this.deviceInformationGL2000.IsLoggerConnectedDisplayMode = false;
			this.deviceInformationGL3000.IsLoggerConnectedDisplayMode = false;
			this.deviceInformationGL4000.IsLoggerConnectedDisplayMode = false;
		}

		private void buttonClose_Click(object sender, EventArgs e)
		{
			base.DialogResult = DialogResult.OK;
		}

		private void CardReaderDeviceInformation_Shown(object sender, EventArgs e)
		{
			if (this.Device == null)
			{
				return;
			}
			this.deviceInformationGL1000.Visible = false;
			this.deviceInformationGL1020FTE.Visible = false;
			this.deviceInformationGL2000.Visible = false;
			this.deviceInformationGL3000.Visible = false;
			this.deviceInformationGL4000.Visible = false;
			if (this.Device is IGL1000Device)
			{
				this.deviceInformationGL1000.Visible = true;
				this.deviceInformationGL1000.Init(this.Device, false);
				return;
			}
			if (this.Device is IGL1020FTEDevice)
			{
				this.deviceInformationGL1020FTE.Visible = true;
				this.deviceInformationGL1020FTE.Init(this.Device);
				return;
			}
			if (this.Device is IGL2000Device)
			{
				this.deviceInformationGL2000.Visible = true;
				this.deviceInformationGL2000.Init(this.Device);
				return;
			}
			if (this.Device is IGL3000Device)
			{
				this.deviceInformationGL3000.Visible = true;
				this.deviceInformationGL3000.Init(this.Device);
				return;
			}
			if (this.Device is IGL4000Device)
			{
				this.deviceInformationGL4000.Visible = true;
				this.deviceInformationGL4000.Init(this.Device);
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
			ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(CardReaderDeviceInformation));
			this.panelDeviceInfo = new Panel();
			this.buttonClose = new Button();
			this.deviceInformationGL4000 = new DeviceInformationGL4000();
			this.deviceInformationGL1000 = new DeviceInformationGL1000();
			this.deviceInformationGL1020FTE = new DeviceInformationGL1020FTE();
			this.deviceInformationGL2000 = new DeviceInformationGL2000();
			this.deviceInformationGL3000 = new DeviceInformationGL3000();
			this.panelDeviceInfo.SuspendLayout();
			base.SuspendLayout();
			componentResourceManager.ApplyResources(this.panelDeviceInfo, "panelDeviceInfo");
			this.panelDeviceInfo.Controls.Add(this.deviceInformationGL4000);
			this.panelDeviceInfo.Controls.Add(this.deviceInformationGL1000);
			this.panelDeviceInfo.Controls.Add(this.deviceInformationGL1020FTE);
			this.panelDeviceInfo.Controls.Add(this.deviceInformationGL2000);
			this.panelDeviceInfo.Controls.Add(this.deviceInformationGL3000);
			this.panelDeviceInfo.Name = "panelDeviceInfo";
			componentResourceManager.ApplyResources(this.buttonClose, "buttonClose");
			this.buttonClose.Name = "buttonClose";
			this.buttonClose.UseVisualStyleBackColor = true;
			this.buttonClose.Click += new EventHandler(this.buttonClose_Click);
			componentResourceManager.ApplyResources(this.deviceInformationGL4000, "deviceInformationGL4000");
			this.deviceInformationGL4000.IsLoggerConnectedDisplayMode = true;
			this.deviceInformationGL4000.Name = "deviceInformationGL4000";
			componentResourceManager.ApplyResources(this.deviceInformationGL1000, "deviceInformationGL1000");
			this.deviceInformationGL1000.IsLoggerConnectedDisplayMode = true;
			this.deviceInformationGL1000.Name = "deviceInformationGL1000";
			componentResourceManager.ApplyResources(this.deviceInformationGL1020FTE, "deviceInformationGL1020FTE");
			this.deviceInformationGL1020FTE.IsLoggerConnectedDisplayMode = true;
			this.deviceInformationGL1020FTE.Name = "deviceInformationGL1020FTE";
			componentResourceManager.ApplyResources(this.deviceInformationGL2000, "deviceInformationGL2000");
			this.deviceInformationGL2000.IsLoggerConnectedDisplayMode = true;
			this.deviceInformationGL2000.Name = "deviceInformationGL2000";
			componentResourceManager.ApplyResources(this.deviceInformationGL3000, "deviceInformationGL3000");
			this.deviceInformationGL3000.IsLoggerConnectedDisplayMode = true;
			this.deviceInformationGL3000.Name = "deviceInformationGL3000";
			base.AcceptButton = this.buttonClose;
			componentResourceManager.ApplyResources(this, "$this");
			base.AutoScaleMode = AutoScaleMode.Font;
			base.ControlBox = false;
			base.Controls.Add(this.buttonClose);
			base.Controls.Add(this.panelDeviceInfo);
			base.FormBorderStyle = FormBorderStyle.FixedDialog;
			base.Name = "CardReaderDeviceInformation";
			base.Shown += new EventHandler(this.CardReaderDeviceInformation_Shown);
			this.panelDeviceInfo.ResumeLayout(false);
			base.ResumeLayout(false);
		}
	}
}
