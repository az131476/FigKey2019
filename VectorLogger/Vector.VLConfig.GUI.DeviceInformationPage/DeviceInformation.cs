using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.GUI.Common.FileManager;
using Vector.VLConfig.HardwareAccess.HardwareAbstraction;
using Vector.VLConfig.HardwareAccess.ToolInterfaces;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.DeviceInformationPage
{
	public class DeviceInformation : UserControl, IUpdateObserver<LoggerType>, IUpdateObserver, IPropertyWindow, IConfigClipboardClient, IHardwareFrontendClient
	{
		private LoggerType loggerType;

		private IContainer components;

		private DeviceInformationGL1000 deviceInformationGL1000;

		private DeviceInformationGL3000 deviceInformationGL3000;

		private DeviceInformationGL2000 deviceInformationGL2000;

		private DeviceInformationGL4000 deviceInformationGL4000;

		private DeviceInformationGL1020FTE deviceInformationGL1020FTE;

		private DeviceInformationVN16XXlog deviceInformationVN16XXlog;

		public event EventHandler FormatMemoryCardClicked;

		IModelValidator IPropertyWindow.ModelValidator
		{
			get;
			set;
		}

		ISemanticChecker IPropertyWindow.SemanticChecker
		{
			get;
			set;
		}

		IModelEditor IPropertyWindow.ModelEditor
		{
			get;
			set;
		}

		public DisplayMode DisplayMode
		{
			get;
			set;
		}

		IUpdateService IPropertyWindow.UpdateService
		{
			get;
			set;
		}

		IUpdateObserver IPropertyWindow.UpdateObserver
		{
			get
			{
				return this;
			}
		}

		PageType IPropertyWindow.Type
		{
			get
			{
				return PageType.DeviceInformation;
			}
		}

		bool IPropertyWindow.IsVisible
		{
			get
			{
				return base.Visible;
			}
			set
			{
				base.Visible = value;
			}
		}

		public IHardwareFrontend HardwareFrontend
		{
			get;
			set;
		}

		public List<Vector.VLConfig.Data.ConfigurationDataModel.Action> FocusedActions
		{
			get
			{
				return null;
			}
		}

		public DeviceInformation()
		{
			this.InitializeComponent();
			this.loggerType = LoggerType.Unknown;
		}

		bool IPropertyWindow.IsDisplayingFeature(Feature feature)
		{
			return false;
		}

		void IPropertyWindow.Init()
		{
			if (((IPropertyWindow)this).UpdateService != null)
			{
				((IPropertyWindow)this).UpdateService.AddUpdateObserver(this, UpdateContext.DeviceInformation);
			}
			this.deviceInformationGL2000.FormatMemoryCardClicked += new EventHandler(this.OnFormatMemoryCardClicked);
			this.deviceInformationVN16XXlog.FormatMemoryCardClicked += new EventHandler(this.OnFormatMemoryCardClicked);
		}

		void IPropertyWindow.Reset()
		{
		}

		bool IPropertyWindow.ValidateInput()
		{
			return true;
		}

		bool IPropertyWindow.HasErrors()
		{
			return false;
		}

		bool IPropertyWindow.HasGlobalErrors()
		{
			return false;
		}

		bool IPropertyWindow.HasLocalErrors()
		{
			return false;
		}

		bool IPropertyWindow.HasFormatErrors()
		{
			return false;
		}

		public void DeviceUpdated(ILoggerDevice device)
		{
			if (device == this.HardwareFrontend.PrimaryOnlineDevice)
			{
				this.RefreshView();
			}
		}

		public void DevicesAdded(IList<ILoggerDevice> devices)
		{
			foreach (ILoggerDevice current in devices)
			{
				if (current == this.HardwareFrontend.PrimaryOnlineDevice)
				{
					this.UpdateDeviceAsync(current);
					this.RefreshView();
					break;
				}
			}
		}

		public void DevicesRemoved(IList<ILoggerDevice> devices)
		{
			this.RefreshView();
		}

		public void AdditionalDrivesListChanged(IList<string> additionalDrivesList)
		{
		}

		void IUpdateObserver<LoggerType>.Update(LoggerType data)
		{
			if (this.loggerType == data)
			{
				return;
			}
			this.loggerType = data;
			ILoggerDevice firstOnlineLogger = this.GetFirstOnlineLogger();
			this.deviceInformationGL1000.Visible = false;
			this.deviceInformationGL1020FTE.Visible = false;
			this.deviceInformationGL2000.Visible = false;
			this.deviceInformationGL3000.Visible = false;
			this.deviceInformationGL4000.Visible = false;
			this.deviceInformationVN16XXlog.Visible = false;
			switch (data)
			{
			case LoggerType.GL1000:
				this.deviceInformationGL1000.Init(firstOnlineLogger, this.HardwareFrontend.IsDeviceWithoutMemoryCardConnected);
				this.deviceInformationGL1000.Visible = true;
				return;
			case LoggerType.GL1020FTE:
				this.deviceInformationGL1020FTE.Init(firstOnlineLogger);
				this.deviceInformationGL1020FTE.Visible = true;
				return;
			case LoggerType.GL2000:
				this.deviceInformationGL2000.Init(firstOnlineLogger);
				this.deviceInformationGL2000.Visible = true;
				return;
			case LoggerType.GL3000:
				this.deviceInformationGL3000.Init(firstOnlineLogger);
				this.deviceInformationGL3000.Visible = true;
				return;
			case LoggerType.GL4000:
				this.deviceInformationGL4000.Init(firstOnlineLogger);
				this.deviceInformationGL4000.Visible = true;
				return;
			case LoggerType.VN1630log:
				this.deviceInformationVN16XXlog.Init(firstOnlineLogger, this.HardwareFrontend.IsDeviceWithoutMemoryCardConnected);
				this.deviceInformationVN16XXlog.Visible = true;
				return;
			default:
				return;
			}
		}

		public ConfigClipboardManager.AcceptType Accept(Vector.VLConfig.Data.ConfigurationDataModel.Action action)
		{
			return ConfigClipboardManager.AcceptType.None;
		}

		public bool Insert(Vector.VLConfig.Data.ConfigurationDataModel.Action action)
		{
			return false;
		}

		public bool Insert(Event evt)
		{
			return false;
		}

		private void Raise_FormatMemoryCardClicked(object sender, EventArgs e)
		{
			if (this.FormatMemoryCardClicked != null)
			{
				this.FormatMemoryCardClicked(sender, e);
			}
		}

		public void RefreshView()
		{
			ILoggerDevice primaryOnlineDevice = this.HardwareFrontend.PrimaryOnlineDevice;
			if (primaryOnlineDevice != null && primaryOnlineDevice.LoggerSpecifics.Type != this.loggerType)
			{
				((IUpdateObserver<LoggerType>)this).Update(primaryOnlineDevice.LoggerSpecifics.Type);
				return;
			}
			switch (this.loggerType)
			{
			case LoggerType.GL1000:
				this.deviceInformationGL1000.Init(primaryOnlineDevice, this.HardwareFrontend.IsDeviceWithoutMemoryCardConnected);
				return;
			case LoggerType.GL1020FTE:
				this.deviceInformationGL1020FTE.Init(primaryOnlineDevice);
				return;
			case LoggerType.GL2000:
				this.deviceInformationGL2000.Init(primaryOnlineDevice);
				return;
			case LoggerType.GL3000:
				this.deviceInformationGL3000.Init(primaryOnlineDevice);
				return;
			case LoggerType.GL4000:
				this.deviceInformationGL4000.Init(primaryOnlineDevice);
				return;
			case LoggerType.VN1630log:
				this.deviceInformationVN16XXlog.Init(primaryOnlineDevice, this.HardwareFrontend.IsDeviceWithoutMemoryCardConnected);
				return;
			default:
				return;
			}
		}

		private void OnFormatMemoryCardClicked(object sender, EventArgs e)
		{
			this.Raise_FormatMemoryCardClicked(this, EventArgs.Empty);
		}

		private ILoggerDevice GetFirstOnlineLogger()
		{
			ILoggerDevice result = null;
			if (this.HardwareFrontend.CurrentOnlineLoggerDevices.Count > 0)
			{
				result = this.HardwareFrontend.CurrentOnlineLoggerDevices.First<ILoggerDevice>();
			}
			return result;
		}

		private void UpdateDeviceAsync(ILoggerDevice device)
		{
			if (device == null)
			{
				return;
			}
			if (!device.LogFileStorage.IsOutdated)
			{
				return;
			}
			ActivityIndicatorForm activityIndicatorForm = new ActivityIndicatorForm();
			ProcessExitedDelegate processExitedDelegate = new ProcessExitedDelegate(activityIndicatorForm.ProcessExited);
			activityIndicatorForm.Text = Resources.ActivityTitle;
			activityIndicatorForm.SetStatusText(Resources.ActivityTextLoading);
			BackgroundWorker backgroundWorker = new BackgroundWorker();
			backgroundWorker.DoWork += delegate(object sender, DoWorkEventArgs arguments)
			{
				try
				{
					device.LogFileStorage.UpdateFileList();
				}
				catch (Exception)
				{
				}
			};
			backgroundWorker.RunWorkerCompleted += delegate(object sender, RunWorkerCompletedEventArgs arguments)
			{
				processExitedDelegate();
			};
			backgroundWorker.RunWorkerAsync();
			activityIndicatorForm.ShowDialog();
			if (activityIndicatorForm != null)
			{
				activityIndicatorForm.Dispose();
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
			this.deviceInformationGL2000 = new DeviceInformationGL2000();
			this.deviceInformationGL1000 = new DeviceInformationGL1000();
			this.deviceInformationGL3000 = new DeviceInformationGL3000();
			this.deviceInformationGL4000 = new DeviceInformationGL4000();
			this.deviceInformationGL1020FTE = new DeviceInformationGL1020FTE();
			this.deviceInformationVN16XXlog = new DeviceInformationVN16XXlog();
			base.SuspendLayout();
			this.deviceInformationGL2000.Dock = DockStyle.Fill;
			this.deviceInformationGL2000.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.deviceInformationGL2000.IsLoggerConnectedDisplayMode = true;
			this.deviceInformationGL2000.Location = new Point(0, 0);
			this.deviceInformationGL2000.Name = "deviceInformationGL2000";
			this.deviceInformationGL2000.Size = new Size(767, 665);
			this.deviceInformationGL2000.TabIndex = 2;
			this.deviceInformationGL1000.Dock = DockStyle.Fill;
			this.deviceInformationGL1000.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.deviceInformationGL1000.IsLoggerConnectedDisplayMode = true;
			this.deviceInformationGL1000.Location = new Point(0, 0);
			this.deviceInformationGL1000.Name = "deviceInformationGL1000";
			this.deviceInformationGL1000.Size = new Size(767, 665);
			this.deviceInformationGL1000.TabIndex = 0;
			this.deviceInformationGL3000.Dock = DockStyle.Fill;
			this.deviceInformationGL3000.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.deviceInformationGL3000.IsLoggerConnectedDisplayMode = true;
			this.deviceInformationGL3000.Location = new Point(0, 0);
			this.deviceInformationGL3000.Name = "deviceInformationGL3000";
			this.deviceInformationGL3000.Size = new Size(767, 665);
			this.deviceInformationGL3000.TabIndex = 1;
			this.deviceInformationGL4000.Dock = DockStyle.Fill;
			this.deviceInformationGL4000.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.deviceInformationGL4000.IsLoggerConnectedDisplayMode = true;
			this.deviceInformationGL4000.Location = new Point(0, 0);
			this.deviceInformationGL4000.Name = "deviceInformationGL4000";
			this.deviceInformationGL4000.Size = new Size(767, 665);
			this.deviceInformationGL4000.TabIndex = 3;
			this.deviceInformationGL1020FTE.Dock = DockStyle.Fill;
			this.deviceInformationGL1020FTE.Font = new Font("Microsoft Sans Serif", 8.25f, FontStyle.Regular, GraphicsUnit.Point, 0);
			this.deviceInformationGL1020FTE.IsLoggerConnectedDisplayMode = true;
			this.deviceInformationGL1020FTE.Location = new Point(0, 0);
			this.deviceInformationGL1020FTE.Name = "deviceInformationGL1020FTE";
			this.deviceInformationGL1020FTE.Size = new Size(767, 665);
			this.deviceInformationGL1020FTE.TabIndex = 4;
			this.deviceInformationVN16XXlog.Dock = DockStyle.Fill;
			this.deviceInformationVN16XXlog.Font = new Font("Microsoft Sans Serif", 8.25f);
			this.deviceInformationVN16XXlog.IsLoggerConnectedDisplayMode = true;
			this.deviceInformationVN16XXlog.Location = new Point(0, 0);
			this.deviceInformationVN16XXlog.Name = "deviceInformationVN16XXlog";
			this.deviceInformationVN16XXlog.Size = new Size(767, 665);
			this.deviceInformationVN16XXlog.TabIndex = 5;
			base.AutoScaleDimensions = new SizeF(6f, 13f);
			base.AutoScaleMode = AutoScaleMode.Font;
			base.Controls.Add(this.deviceInformationVN16XXlog);
			base.Controls.Add(this.deviceInformationGL1020FTE);
			base.Controls.Add(this.deviceInformationGL4000);
			base.Controls.Add(this.deviceInformationGL2000);
			base.Controls.Add(this.deviceInformationGL1000);
			base.Controls.Add(this.deviceInformationGL3000);
			base.Name = "DeviceInformation";
			base.Size = new Size(767, 665);
			base.ResumeLayout(false);
		}
	}
}
