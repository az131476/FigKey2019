using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Management;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.GUI.Common.FileManager;
using Vector.VLConfig.HardwareAccess.HardwareAbstraction.VN16XXlog;
using Vector.VLConfig.HardwareAccess.ToolInterfaces;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.HardwareAccess.HardwareAbstraction
{
	public class HardwareFrontend : IHardwareFrontend, IDisposable
	{
		public delegate void OnDiskEventArrivedCallback(object sender, EventArrivedEventArgs args);

		private IList<ILoggerDevice> currentLoggerDevices;

		private LoggerType currentLoggerType;

		private IDictionary<LoggerType, ILoggerDeviceScanner> loggerDeviceScanners;

		private List<string> additionalDrives;

		private List<IHardwareFrontendClient> clients;

		private Dictionary<ILoggerDevice, FileSystemWatcher> deviceWatchers;

		private bool isClientNotificationEnabled;

		private Dictionary<IHardwareFrontendClient, ILoggerDevice> customDevicesForClients;

		private Dictionary<ILoggerDevice, FileSystemWatcher> customDeviceWatchers;

		private ManagementScope managementScope;

		private WqlEventQuery wqlEventQuery;

		private ManagementEventWatcher managementEventWatcher;

		private System.Windows.Forms.Timer diskEventDelayTimer;

		private bool isLoggerDevicesUpdating;

		private System.Windows.Forms.Timer deviceWatcherDelayTimer;

		private ILoggerDevice lastChangedWatchedDevice;

		public IList<ILoggerDevice> CurrentLoggerDevices
		{
			get
			{
				return this.currentLoggerDevices;
			}
		}

		public ReadOnlyCollection<ILoggerDevice> CurrentOnlineLoggerDevices
		{
			get
			{
				return new ReadOnlyCollection<ILoggerDevice>((from ILoggerDevice device in this.currentLoggerDevices
				where device.IsOnline
				select device).ToList<ILoggerDevice>());
			}
		}

		public ReadOnlyCollection<ILoggerDevice> CurrentOfflineLoggerDevices
		{
			get
			{
				return new ReadOnlyCollection<ILoggerDevice>((from ILoggerDevice device in this.currentLoggerDevices
				where !device.IsOnline
				select device).ToList<ILoggerDevice>());
			}
		}

		public ReadOnlyCollection<ILoggerDevice> CurrentLoggerDevicesWithLoggerInfo
		{
			get
			{
				return new ReadOnlyCollection<ILoggerDevice>((from ILoggerDevice device in this.currentLoggerDevices
				where device.HasLoggerInfo
				select device).ToList<ILoggerDevice>());
			}
		}

		public ReadOnlyCollection<ILoggerDevice> CurrentLoggerDevicesWithIndexFile
		{
			get
			{
				return new ReadOnlyCollection<ILoggerDevice>((from ILoggerDevice device in this.currentLoggerDevices
				where device.HasIndexFile
				select device).ToList<ILoggerDevice>());
			}
		}

		public bool IsDeviceWithoutMemoryCardConnected
		{
			get
			{
				return this.currentLoggerType != LoggerType.Unknown && this.loggerDeviceScanners != null && this.loggerDeviceScanners[this.currentLoggerType].IsDeviceWithoutMemoryCardConnected;
			}
		}

		public LoggerType LoggerTypeToScan
		{
			get
			{
				return this.currentLoggerType;
			}
			set
			{
				this.currentLoggerType = value;
			}
		}

		public bool IsScanForAllLoggerTypesEnabled
		{
			get
			{
				return this.currentLoggerType == LoggerType.Unknown;
			}
		}

		public List<string> AdditionalDrives
		{
			get
			{
				return this.additionalDrives;
			}
			set
			{
				List<string> previousAdditionalDrives = this.additionalDrives;
				this.additionalDrives = value;
				this.PropagateAdditionalDrivesListChangedToClients(previousAdditionalDrives);
			}
		}

		public ILoggerDevice PrimaryOnlineDevice
		{
			get
			{
				if (this.CurrentOnlineLoggerDevices.Count > 0)
				{
					return this.CurrentOnlineLoggerDevices.First<ILoggerDevice>();
				}
				return null;
			}
		}

		public ILoggerDevice GetLoggerDeviceByDriveName(string driveName)
		{
			if (string.IsNullOrEmpty(driveName))
			{
				return null;
			}
			string value = driveName.Substring(0, 1);
			foreach (ILoggerDevice current in this.CurrentLoggerDevices)
			{
				if (current.HardwareKey.IndexOf(value) == 0)
				{
					return current;
				}
			}
			return null;
		}

		public void UpdateLoggerDeviceList()
		{
			this.isLoggerDevicesUpdating = true;
			this.ClearDeviceWatchers();
			List<ILoggerDevice> previousLoggerDevices = new List<ILoggerDevice>(this.currentLoggerDevices);
			if (this.LoggerTypeToScan != LoggerType.Unknown)
			{
				this.UpdateLoggerDeviceList(this.LoggerTypeToScan, this.AdditionalDrives);
			}
			else
			{
				this.UpdateLoggerDeviceListAllLoggerTypes(this.AdditionalDrives);
			}
			if (this.isClientNotificationEnabled)
			{
				this.PropagateDeviceListChangedToClients(previousLoggerDevices);
				this.SetupDeviceWatchers();
				this.FireAllNonFilesystemDevicesUpdatedToClients();
			}
			this.isLoggerDevicesUpdating = false;
		}

		public bool EnforceExplicitUpdateOfLoggerDevice(ILoggerDevice scannedLoggerDevice)
		{
			if (!this.currentLoggerDevices.Contains(scannedLoggerDevice))
			{
				return false;
			}
			scannedLoggerDevice.LogFileStorage.DataSourceHasChanged();
			bool result = scannedLoggerDevice.Update();
			foreach (IHardwareFrontendClient current in this.clients)
			{
				current.DeviceUpdated(scannedLoggerDevice);
			}
			return result;
		}

		public bool IsCustomLoggerDevice(ILoggerDevice device)
		{
			return device != null && !this.currentLoggerDevices.Contains(device);
		}

		public ILoggerDevice CreateCustomLoggerDevice(LoggerType type, string drivePath, IHardwareFrontendClient client)
		{
			if (this.customDevicesForClients.ContainsKey(client))
			{
				ILoggerDevice loggerDevice = this.customDevicesForClients[client];
				if (loggerDevice.HardwareKey == drivePath && loggerDevice.LoggerType == type)
				{
					return loggerDevice;
				}
			}
			ILoggerDevice loggerDevice2;
			switch (type)
			{
			case LoggerType.GL1000:
				loggerDevice2 = new GL1000Device(drivePath, false);
				break;
			case LoggerType.GL1020FTE:
				loggerDevice2 = new GL1020FTEDevice(drivePath, false);
				break;
			case LoggerType.GL2000:
				loggerDevice2 = new GL2000Device(drivePath, false);
				break;
			case LoggerType.GL3000:
				loggerDevice2 = new GL3000Device(drivePath, false);
				break;
			case LoggerType.GL4000:
				loggerDevice2 = new GL4000Device(drivePath, false);
				break;
			case LoggerType.VN1630log:
				loggerDevice2 = new VN16XXlogDevice(79, 255, drivePath, false);
				break;
			default:
				return null;
			}
			loggerDevice2.Update();
			this.SetCustomLoggerDevice(loggerDevice2, client);
			return loggerDevice2;
		}

		public bool SetCustomLoggerDevice(ILoggerDevice device, IHardwareFrontendClient client)
		{
			if (device == null || client == null)
			{
				return false;
			}
			if (!this.clients.Contains(client))
			{
				return false;
			}
			if (!device.LoggerSpecifics.DataStorage.IsUsingWindowsFileSystem)
			{
				return false;
			}
			if (this.customDevicesForClients.ContainsKey(client))
			{
				this.ReleaseCustomDeviceWatcher(this.customDevicesForClients[client]);
				this.customDevicesForClients[client] = device;
			}
			else
			{
				this.customDevicesForClients.Add(client, device);
			}
			if (this.isClientNotificationEnabled)
			{
				this.SetupCustomDeviceWatcher(device);
			}
			return true;
		}

		public bool ReleaseCustomLoggerDevice(IHardwareFrontendClient client)
		{
			if (!this.clients.Contains(client))
			{
				return false;
			}
			if (this.customDevicesForClients.ContainsKey(client))
			{
				this.ReleaseCustomDeviceWatcher(this.customDevicesForClients[client]);
				this.customDevicesForClients.Remove(client);
				return true;
			}
			return false;
		}

		public void RegisterClient(IHardwareFrontendClient client)
		{
			if (client != null && !this.clients.Contains(client))
			{
				this.clients.Add(client);
				client.HardwareFrontend = this;
			}
		}

		public void EnableClientNotification(bool isEnabled)
		{
			bool flag = !this.isClientNotificationEnabled && isEnabled;
			this.isClientNotificationEnabled = isEnabled;
			if (flag)
			{
				foreach (IHardwareFrontendClient current in this.clients)
				{
					this.PropagateDeviceListToClient(current);
				}
				this.SetupDeviceWatchers();
				this.PropagateAdditionalDrivesListChangedToClients(this.additionalDrives);
				this.FireAllNonFilesystemDevicesUpdatedToClients();
				foreach (IHardwareFrontendClient current2 in this.customDevicesForClients.Keys)
				{
					this.SetupCustomDeviceWatcher(this.customDevicesForClients[current2]);
				}
			}
		}

		public void EnableDeviceWatcher(ILoggerDevice device, bool isEnabled)
		{
			if (!device.LoggerSpecifics.DataStorage.IsUsingWindowsFileSystem)
			{
				return;
			}
			if (this.deviceWatchers.ContainsKey(device))
			{
				FileSystemWatcher fileSystemWatcher = this.deviceWatchers[device];
				if (isEnabled)
				{
					fileSystemWatcher.Path = device.HardwareKey;
					fileSystemWatcher.EnableRaisingEvents = true;
					return;
				}
				fileSystemWatcher.EnableRaisingEvents = false;
				fileSystemWatcher.Path = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			}
		}

		public bool Eject(ILoggerDevice device)
		{
			if (device == null)
			{
				return false;
			}
			if (!this.currentLoggerDevices.Contains(device))
			{
				return false;
			}
			this.EnableDeviceWatcher(device, false);
			this.ReleaseAllCustomDeviceWatchersBelow(device);
			string text = device.HardwareKey;
			if (text.Length == 1)
			{
				text = text + Path.VolumeSeparatorChar + Path.DirectorySeparatorChar;
			}
			RemovableDrive.Eject(text);
			Thread.Sleep(1000);
			Application.DoEvents();
			if (device.LoggerSpecifics.DataStorage.IsUsingWindowsFileSystem && Directory.Exists(text))
			{
				InformMessageBox.Error(Resources.ErrorUnableToEject);
				return false;
			}
			return true;
		}

		private ILoggerDeviceScanner GetLoggerDeviceScanner(LoggerType loggerType)
		{
			if (this.loggerDeviceScanners == null)
			{
				this.loggerDeviceScanners = new Dictionary<LoggerType, ILoggerDeviceScanner>();
				this.loggerDeviceScanners[LoggerType.GL1000] = new GL1000Scanner();
				this.loggerDeviceScanners[LoggerType.GL1020FTE] = new GL1020FTEScanner();
				this.loggerDeviceScanners[LoggerType.GL2000] = new GL2000Scanner();
				this.loggerDeviceScanners[LoggerType.GL3000] = new GL3000Scanner();
				this.loggerDeviceScanners[LoggerType.GL4000] = new GL4000Scanner();
				this.loggerDeviceScanners[LoggerType.VN1630log] = new VN16XXlogScanner();
			}
			return this.loggerDeviceScanners[loggerType];
		}

		private void UpdateLoggerDeviceListAllLoggerTypes(List<string> additionalDrives)
		{
			List<LoggerType> list = Enum.GetValues(typeof(LoggerType)).OfType<LoggerType>().ToList<LoggerType>();
			if (Application.OpenForms != null && Application.OpenForms.Count < 1)
			{
				this.UpdateLoggerDeviceListSelectedLoggerTypes(list, additionalDrives);
				this.LoggerTypeToScan = LoggerType.Unknown;
				return;
			}
			ActivityIndicatorForm activityIndicatorForm = new ActivityIndicatorForm();
			ProcessExitedDelegate processExitedDelegate = new ProcessExitedDelegate(activityIndicatorForm.ProcessExited);
			activityIndicatorForm.Text = Resources.ActivityTitle;
			activityIndicatorForm.SetStatusText(Resources.ActivityTextScanning);
			System.Timers.Timer showDialogTimer = new System.Timers.Timer(500.0);
			ManualResetEvent finishedEvent = new ManualResetEvent(false);
			bool showDialog = false;
			BackgroundWorker backgroundWorker = new BackgroundWorker();
			backgroundWorker.DoWork += delegate(object sender, DoWorkEventArgs arguments)
			{
				try
				{
					showDialogTimer.Start();
					this.UpdateLoggerDeviceListSelectedLoggerTypes(list, additionalDrives);
					this.LoggerTypeToScan = LoggerType.Unknown;
					finishedEvent.Set();
				}
				catch (Exception)
				{
					finishedEvent.Set();
				}
			};
			backgroundWorker.RunWorkerCompleted += delegate(object sender, RunWorkerCompletedEventArgs arguments)
			{
				showDialogTimer.Stop();
				finishedEvent.Set();
				processExitedDelegate();
			};
			showDialogTimer.Elapsed += delegate(object sender, ElapsedEventArgs arguments)
			{
				showDialog = true;
				showDialogTimer.Stop();
				finishedEvent.Set();
			};
			backgroundWorker.RunWorkerAsync();
			finishedEvent.WaitOne();
			if (showDialog)
			{
				activityIndicatorForm.ShowDialog();
				if (activityIndicatorForm != null)
				{
					activityIndicatorForm.Dispose();
				}
			}
		}

		private void UpdateLoggerDeviceListSelectedLoggerTypes(List<LoggerType> loggerTypes, List<string> additionalDrives)
		{
			List<ILoggerDevice> list = new List<ILoggerDevice>();
			foreach (LoggerType current in loggerTypes)
			{
				if (current != LoggerType.Unknown)
				{
					list.AddRange(this.GetAvailableLoggerDevicesOfType(current, additionalDrives));
				}
			}
			this.currentLoggerDevices.Clear();
			List<ILoggerDevice> list2 = new List<ILoggerDevice>();
			foreach (ILoggerDevice current2 in list)
			{
				if (current2.IsOnline)
				{
					this.currentLoggerDevices.Add(current2);
				}
				else
				{
					list2.Add(current2);
				}
			}
			list2 = (from x in list2
			orderby x.LoggerType == LoggerType.GL2000 descending
			select x).ToList<ILoggerDevice>();
			foreach (ILoggerDevice current3 in list2)
			{
				this.currentLoggerDevices.Add(current3);
			}
			this.currentLoggerDevices = (from x in this.currentLoggerDevices
			group x by x.HardwareKey[0] into y
			select y.First<ILoggerDevice>()).ToList<ILoggerDevice>();
		}

		private void UpdateLoggerDeviceList(LoggerType loggerType, List<string> additionalDrives)
		{
			Cursor.Current = Cursors.WaitCursor;
			if (Application.OpenForms != null && Application.OpenForms.Count < 1)
			{
				this.currentLoggerDevices = this.GetAvailableLoggerDevicesOfType(loggerType, additionalDrives);
				this.currentLoggerType = loggerType;
				return;
			}
			ActivityIndicatorForm activityIndicatorForm = new ActivityIndicatorForm();
			ProcessExitedDelegate processExitedDelegate = new ProcessExitedDelegate(activityIndicatorForm.ProcessExited);
			activityIndicatorForm.Text = Resources.ActivityTitle;
			activityIndicatorForm.SetStatusText(Resources.ActivityTextScanning);
			System.Timers.Timer showDialogTimer = new System.Timers.Timer(500.0);
			ManualResetEvent finishedEvent = new ManualResetEvent(false);
			bool showDialog = false;
			BackgroundWorker backgroundWorker = new BackgroundWorker();
			backgroundWorker.DoWork += delegate(object sender, DoWorkEventArgs arguments)
			{
				try
				{
					showDialogTimer.Start();
					this.currentLoggerDevices = this.GetAvailableLoggerDevicesOfType(loggerType, additionalDrives);
					this.currentLoggerType = loggerType;
					finishedEvent.Set();
				}
				catch (Exception)
				{
					finishedEvent.Set();
				}
			};
			backgroundWorker.RunWorkerCompleted += delegate(object sender, RunWorkerCompletedEventArgs arguments)
			{
				showDialogTimer.Stop();
				finishedEvent.Set();
				processExitedDelegate();
			};
			showDialogTimer.Elapsed += delegate(object sender, ElapsedEventArgs arguments)
			{
				showDialog = true;
				showDialogTimer.Stop();
				finishedEvent.Set();
			};
			backgroundWorker.RunWorkerAsync();
			finishedEvent.WaitOne();
			if (showDialog)
			{
				activityIndicatorForm.ShowDialog();
				if (activityIndicatorForm != null)
				{
					activityIndicatorForm.Dispose();
				}
			}
			Cursor.Current = Cursors.Default;
		}

		private List<ILoggerDevice> GetAvailableLoggerDevicesOfType(LoggerType loggerType, List<string> additionalDrives)
		{
			List<string> list = new List<string>();
			if (additionalDrives != null)
			{
				foreach (string current in additionalDrives)
				{
					if (FileSystemServices.IsDriveAvailable(current))
					{
						list.Add(current);
					}
				}
			}
			IList<ILoggerDevice> temporaryLoggerDeviceList = this.GetTemporaryLoggerDeviceList(loggerType, list);
			List<ILoggerDevice> list2 = new List<ILoggerDevice>();
			foreach (ILoggerDevice current2 in temporaryLoggerDeviceList)
			{
				ILoggerDevice loggerDeviceByDriveName = this.GetLoggerDeviceByDriveName(current2.HardwareKey);
				if (loggerDeviceByDriveName != null && current2.LoggerType == loggerDeviceByDriveName.LoggerType && current2.IsOnline == loggerDeviceByDriveName.IsOnline)
				{
					list2.Add(loggerDeviceByDriveName);
					loggerDeviceByDriveName.Update();
				}
				else
				{
					list2.Add(current2);
					current2.Update();
				}
			}
			return list2;
		}

		private IList<ILoggerDevice> GetTemporaryLoggerDeviceList(LoggerType loggerType, List<string> additionalDrives)
		{
			ILoggerDeviceScanner loggerDeviceScanner = this.GetLoggerDeviceScanner(loggerType);
			loggerDeviceScanner.AdditionalDrives = additionalDrives;
			return loggerDeviceScanner.ScanForLoggerDevices();
		}

		private void InitDeviceChangedNotification()
		{
			new ManagementOperationObserver();
			this.managementScope = new ManagementScope("root\\CIMV2");
			this.managementScope.Options.EnablePrivileges = true;
			try
			{
				this.wqlEventQuery = new WqlEventQuery();
				this.wqlEventQuery.EventClassName = "__InstanceOperationEvent";
				this.wqlEventQuery.WithinInterval = new TimeSpan(0, 0, 3);
				this.wqlEventQuery.Condition = "TargetInstance ISA 'Win32_DiskDrive' ";
				this.managementEventWatcher = new ManagementEventWatcher(this.managementScope, this.wqlEventQuery);
				this.managementEventWatcher.EventArrived += new EventArrivedEventHandler(this.DiskEventArrived);
				this.managementEventWatcher.Start();
			}
			catch (Exception)
			{
			}
		}

		private void DiskEventArrived(object sender, EventArrivedEventArgs args)
		{
			if (Application.OpenForms.Count == 0)
			{
				return;
			}
			if (Application.OpenForms[0].InvokeRequired)
			{
				HardwareFrontend.OnDiskEventArrivedCallback method = new HardwareFrontend.OnDiskEventArrivedCallback(this.DiskEventArrived);
				Application.OpenForms[0].Invoke(method, new object[]
				{
					sender,
					args
				});
				return;
			}
			if (this.diskEventDelayTimer.Enabled)
			{
				this.diskEventDelayTimer.Stop();
			}
			this.diskEventDelayTimer.Start();
		}

		private void DiskEventDelayTimer_Tick(object sender, EventArgs e)
		{
			this.diskEventDelayTimer.Enabled = false;
			if (this.isLoggerDevicesUpdating)
			{
				this.diskEventDelayTimer.Start();
				return;
			}
			this.UpdateLoggerDeviceList();
		}

		private void PropagateDeviceListChangedToClients(IList<ILoggerDevice> previousLoggerDevices)
		{
			List<ILoggerDevice> list = new List<ILoggerDevice>();
			List<ILoggerDevice> list2 = new List<ILoggerDevice>();
			foreach (ILoggerDevice current in this.currentLoggerDevices)
			{
				if (!previousLoggerDevices.Contains(current))
				{
					list.Add(current);
				}
			}
			foreach (ILoggerDevice current2 in previousLoggerDevices)
			{
				if (!this.currentLoggerDevices.Contains(current2))
				{
					list2.Add(current2);
				}
			}
			foreach (IHardwareFrontendClient current3 in this.clients)
			{
				if (!this.IsScanForAllLoggerTypesEnabled && this.customDevicesForClients.ContainsKey(current3))
				{
					ILoggerDevice loggerDevice = this.customDevicesForClients[current3];
					if (loggerDevice != null && loggerDevice.LoggerSpecifics.Type != this.LoggerTypeToScan)
					{
						list2.Add(loggerDevice);
						this.ReleaseCustomLoggerDevice(current3);
					}
				}
				if (list2.Count > 0)
				{
					current3.DevicesRemoved(list2);
				}
				if (list.Count > 0)
				{
					current3.DevicesAdded(list);
				}
			}
		}

		private void PropagateDeviceListToClient(IHardwareFrontendClient client)
		{
			client.DevicesAdded(this.currentLoggerDevices);
		}

		private void SetupDeviceWatchers()
		{
			if (Application.OpenForms.Count == 0)
			{
				return;
			}
			foreach (ILoggerDevice current in this.currentLoggerDevices)
			{
				if (Directory.Exists(current.HardwareKey) && current.LoggerSpecifics.DataStorage.IsUsingWindowsFileSystem && !this.deviceWatchers.ContainsKey(current))
				{
					FileSystemWatcher fileSystemWatcher = new FileSystemWatcher(current.HardwareKey);
					fileSystemWatcher.SynchronizingObject = Application.OpenForms[0];
					fileSystemWatcher.Changed += new FileSystemEventHandler(this.OnDeviceContentChanged);
					fileSystemWatcher.Created += new FileSystemEventHandler(this.OnDeviceContentChanged);
					fileSystemWatcher.Deleted += new FileSystemEventHandler(this.OnDeviceContentChanged);
					fileSystemWatcher.Renamed += new RenamedEventHandler(this.OnFileOnDeviceRenamed);
					fileSystemWatcher.IncludeSubdirectories = true;
					fileSystemWatcher.EnableRaisingEvents = true;
					this.deviceWatchers.Add(current, fileSystemWatcher);
				}
			}
			List<ILoggerDevice> list = new List<ILoggerDevice>();
			foreach (ILoggerDevice current2 in this.deviceWatchers.Keys)
			{
				if (!this.currentLoggerDevices.Contains(current2))
				{
					FileSystemWatcher fileSystemWatcher2 = this.deviceWatchers[current2];
					fileSystemWatcher2.EnableRaisingEvents = false;
					fileSystemWatcher2.Changed -= new FileSystemEventHandler(this.OnDeviceContentChanged);
					fileSystemWatcher2.Created -= new FileSystemEventHandler(this.OnDeviceContentChanged);
					fileSystemWatcher2.Deleted -= new FileSystemEventHandler(this.OnDeviceContentChanged);
					fileSystemWatcher2.Renamed -= new RenamedEventHandler(this.OnFileOnDeviceRenamed);
					list.Add(current2);
				}
			}
			foreach (ILoggerDevice current3 in list)
			{
				this.deviceWatchers.Remove(current3);
			}
		}

		private void ClearDeviceWatchers()
		{
			if (this.deviceWatcherDelayTimer.Enabled)
			{
				this.deviceWatcherDelayTimer.Stop();
			}
			foreach (ILoggerDevice current in this.deviceWatchers.Keys)
			{
				this.EnableDeviceWatcher(current, false);
			}
			this.deviceWatchers.Clear();
		}

		private void SetupCustomDeviceWatcher(ILoggerDevice device)
		{
			if (Application.OpenForms.Count == 0)
			{
				return;
			}
			if (this.customDeviceWatchers.ContainsKey(device))
			{
				return;
			}
			FileSystemWatcher fileSystemWatcher;
			if (File.Exists(device.HardwareKey))
			{
				string extension = Path.GetExtension(device.HardwareKey);
				if (string.IsNullOrEmpty(extension))
				{
					return;
				}
				if (string.Compare(extension, Vocabulary.FileExtensionDotZIP, true) != 0)
				{
					return;
				}
				try
				{
					fileSystemWatcher = new FileSystemWatcher();
					fileSystemWatcher.Path = Path.GetDirectoryName(device.HardwareKey);
					fileSystemWatcher.Filter = Path.GetFileName(device.HardwareKey);
					goto IL_A6;
				}
				catch
				{
					return;
				}
			}
			try
			{
				fileSystemWatcher = new FileSystemWatcher(device.HardwareKey);
			}
			catch
			{
				return;
			}
			IL_A6:
			fileSystemWatcher.SynchronizingObject = Application.OpenForms[0];
			fileSystemWatcher.Changed += new FileSystemEventHandler(this.OnDeviceContentChanged);
			fileSystemWatcher.Created += new FileSystemEventHandler(this.OnDeviceContentChanged);
			fileSystemWatcher.Deleted += new FileSystemEventHandler(this.OnDeviceContentChanged);
			fileSystemWatcher.Renamed += new RenamedEventHandler(this.OnFileOnDeviceRenamed);
			fileSystemWatcher.IncludeSubdirectories = true;
			fileSystemWatcher.EnableRaisingEvents = true;
			this.customDeviceWatchers.Add(device, fileSystemWatcher);
		}

		private void ReleaseCustomDeviceWatcher(ILoggerDevice device)
		{
			if (!this.customDeviceWatchers.ContainsKey(device))
			{
				return;
			}
			FileSystemWatcher fileSystemWatcher = this.customDeviceWatchers[device];
			fileSystemWatcher.EnableRaisingEvents = false;
			fileSystemWatcher.Changed -= new FileSystemEventHandler(this.OnDeviceContentChanged);
			fileSystemWatcher.Created -= new FileSystemEventHandler(this.OnDeviceContentChanged);
			fileSystemWatcher.Deleted -= new FileSystemEventHandler(this.OnDeviceContentChanged);
			fileSystemWatcher.Renamed -= new RenamedEventHandler(this.OnFileOnDeviceRenamed);
			this.customDeviceWatchers.Remove(device);
		}

		private void ReleaseAllCustomDeviceWatchersBelow(ILoggerDevice scannedLoggerDevice)
		{
			if (!scannedLoggerDevice.LoggerSpecifics.DataStorage.IsUsingWindowsFileSystem)
			{
				return;
			}
			List<ILoggerDevice> list = new List<ILoggerDevice>();
			foreach (ILoggerDevice current in this.customDeviceWatchers.Keys)
			{
				string pathRoot = Path.GetPathRoot(current.HardwareKey);
				if (!string.IsNullOrEmpty(pathRoot) && string.Compare(pathRoot, scannedLoggerDevice.HardwareKey, true) == 0)
				{
					list.Add(current);
				}
			}
			List<IHardwareFrontendClient> list2 = new List<IHardwareFrontendClient>();
			foreach (ILoggerDevice current2 in list)
			{
				this.ReleaseCustomDeviceWatcher(current2);
				foreach (IHardwareFrontendClient current3 in this.customDevicesForClients.Keys)
				{
					if (this.customDevicesForClients[current3] == current2)
					{
						list2.Add(current3);
					}
				}
			}
			foreach (IHardwareFrontendClient current4 in list2)
			{
				this.customDevicesForClients.Remove(current4);
			}
		}

		private void OnDeviceContentChanged(object source, FileSystemEventArgs e)
		{
			this.UpdateChangedDevice(source);
			if (!this.isClientNotificationEnabled)
			{
				return;
			}
			this.NotifyDeviceChangedToClients(source);
		}

		private void OnFileOnDeviceRenamed(object source, RenamedEventArgs e)
		{
			this.UpdateChangedDevice(source);
			if (!this.isClientNotificationEnabled)
			{
				return;
			}
			this.NotifyDeviceChangedToClients(source);
		}

		private void UpdateChangedDevice(object source)
		{
			foreach (ILoggerDevice current in this.deviceWatchers.Keys)
			{
				if (this.deviceWatchers[current] == source)
				{
					current.Update();
					break;
				}
			}
		}

		private void NotifyDeviceChangedToClients(object source)
		{
			foreach (ILoggerDevice current in this.deviceWatchers.Keys)
			{
				if (this.deviceWatchers[current] == source)
				{
					this.SetupDelayTimerForChangedDevice(current);
					return;
				}
			}
			foreach (ILoggerDevice current2 in this.customDeviceWatchers.Keys)
			{
				if (this.customDeviceWatchers[current2] == source)
				{
					this.SetupDelayTimerForChangedDevice(current2);
					break;
				}
			}
		}

		private void SetupDelayTimerForChangedDevice(ILoggerDevice device)
		{
			if (this.deviceWatcherDelayTimer.Enabled)
			{
				this.deviceWatcherDelayTimer.Stop();
			}
			device.LogFileStorage.DataSourceHasChanged();
			if (this.lastChangedWatchedDevice != null && this.lastChangedWatchedDevice != device)
			{
				this.FireDeviceUpdatedToClients(this.lastChangedWatchedDevice);
				this.lastChangedWatchedDevice = null;
			}
			this.lastChangedWatchedDevice = device;
			this.deviceWatcherDelayTimer.Start();
		}

		private void DeviceWatcherDelayTimer_Tick(object sender, EventArgs e)
		{
			this.deviceWatcherDelayTimer.Enabled = false;
			if (this.lastChangedWatchedDevice != null)
			{
				this.FireDeviceUpdatedToClients(this.lastChangedWatchedDevice);
			}
			this.lastChangedWatchedDevice = null;
		}

		private void FireDeviceUpdatedToClients(ILoggerDevice device)
		{
			foreach (IHardwareFrontendClient current in this.customDevicesForClients.Keys)
			{
				if (this.customDevicesForClients[current] == device)
				{
					current.DeviceUpdated(device);
					return;
				}
			}
			foreach (IHardwareFrontendClient current2 in this.clients)
			{
				current2.DeviceUpdated(device);
			}
		}

		private void FireAllNonFilesystemDevicesUpdatedToClients()
		{
			foreach (ILoggerDevice current in this.currentLoggerDevices)
			{
				if (!current.LoggerSpecifics.DataStorage.IsUsingWindowsFileSystem)
				{
					this.FireDeviceUpdatedToClients(current);
				}
			}
		}

		private void PropagateAdditionalDrivesListChangedToClients(IList<string> previousAdditionalDrives)
		{
			bool flag = false;
			if (previousAdditionalDrives.Count != this.additionalDrives.Count)
			{
				flag = true;
			}
			else
			{
				foreach (string current in previousAdditionalDrives)
				{
					if (!this.additionalDrives.Contains(current))
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				return;
			}
			foreach (IHardwareFrontendClient current2 in this.clients)
			{
				current2.AdditionalDrivesListChanged(this.additionalDrives);
			}
		}

		public HardwareFrontend()
		{
			this.currentLoggerDevices = new List<ILoggerDevice>();
			this.currentLoggerType = LoggerType.Unknown;
			this.additionalDrives = new List<string>();
			this.clients = new List<IHardwareFrontendClient>();
			this.deviceWatchers = new Dictionary<ILoggerDevice, FileSystemWatcher>();
			this.customDevicesForClients = new Dictionary<IHardwareFrontendClient, ILoggerDevice>();
			this.customDeviceWatchers = new Dictionary<ILoggerDevice, FileSystemWatcher>();
			this.isClientNotificationEnabled = false;
			this.diskEventDelayTimer = new System.Windows.Forms.Timer();
			this.diskEventDelayTimer.Interval = 500;
			this.diskEventDelayTimer.Tick += new EventHandler(this.DiskEventDelayTimer_Tick);
			this.deviceWatcherDelayTimer = new System.Windows.Forms.Timer();
			this.deviceWatcherDelayTimer.Interval = 1000;
			this.deviceWatcherDelayTimer.Tick += new EventHandler(this.DeviceWatcherDelayTimer_Tick);
			this.lastChangedWatchedDevice = null;
			this.isLoggerDevicesUpdating = false;
			this.InitDeviceChangedNotification();
		}

		public void Dispose()
		{
			try
			{
				this.managementEventWatcher.Stop();
			}
			catch (Exception)
			{
			}
			this.diskEventDelayTimer.Stop();
			this.deviceWatcherDelayTimer.Stop();
			foreach (ILoggerDevice current in this.deviceWatchers.Keys)
			{
				this.EnableDeviceWatcher(current, false);
			}
			List<ILoggerDevice> list = new List<ILoggerDevice>(this.customDeviceWatchers.Keys);
			foreach (ILoggerDevice current2 in list)
			{
				this.ReleaseCustomDeviceWatcher(current2);
			}
		}
	}
}
