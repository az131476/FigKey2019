using System;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.HardwareAccess.ToolInterfaces;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.HardwareAccess.HardwareAbstraction
{
	internal class GL3000Device : LoggerDeviceWindowsFileSystem, IGL3000Device
	{
		private CANTransceiverType can1TransceiverType;

		private CANTransceiverType can2TransceiverType;

		private CANTransceiverType can3TransceiverType;

		private CANTransceiverType can4TransceiverType;

		private CANTransceiverType can5TransceiverType;

		private CANTransceiverType can6TransceiverType;

		private CANTransceiverType can7TransceiverType;

		private CANTransceiverType can8TransceiverType;

		private CANTransceiverType can9TransceiverType;

		private bool isAnalogExtensionBoardInstalled;

		private bool isWLANExtensionBoardInstalled;

		private string installedWLANModelName;

		public override LoggerType LoggerType
		{
			get
			{
				return LoggerType.GL3000;
			}
		}

		public override ILoggerSpecifics LoggerSpecifics
		{
			get
			{
				return GL3000Scanner.LoggerSpecifics;
			}
		}

		public override bool HasIndexFile
		{
			get
			{
				return base.IsIndexFileExisting();
			}
		}

		public override ILogFileStorage LogFileStorage
		{
			get
			{
				return this;
			}
		}

		public override bool HasProperClusterSize
		{
			get
			{
				return true;
			}
		}

		public CANTransceiverType CAN1TransceiverType
		{
			get
			{
				return this.can1TransceiverType;
			}
		}

		public CANTransceiverType CAN2TransceiverType
		{
			get
			{
				return this.can2TransceiverType;
			}
		}

		public CANTransceiverType CAN3TransceiverType
		{
			get
			{
				return this.can3TransceiverType;
			}
		}

		public CANTransceiverType CAN4TransceiverType
		{
			get
			{
				return this.can4TransceiverType;
			}
		}

		public CANTransceiverType CAN5TransceiverType
		{
			get
			{
				return this.can5TransceiverType;
			}
		}

		public CANTransceiverType CAN6TransceiverType
		{
			get
			{
				return this.can6TransceiverType;
			}
		}

		public CANTransceiverType CAN7TransceiverType
		{
			get
			{
				return this.can7TransceiverType;
			}
		}

		public CANTransceiverType CAN8TransceiverType
		{
			get
			{
				return this.can8TransceiverType;
			}
		}

		public CANTransceiverType CAN9TransceiverType
		{
			get
			{
				return this.can9TransceiverType;
			}
		}

		public bool IsAnalogExtensionBoardInstalled
		{
			get
			{
				return this.isAnalogExtensionBoardInstalled;
			}
		}

		public bool IsWLANExtensionBoardInstalled
		{
			get
			{
				return this.isWLANExtensionBoardInstalled;
			}
		}

		public string InstalledWLANBoardModelName
		{
			get
			{
				return this.installedWLANModelName;
			}
		}

		public GL3000Device(string hardwareKey, bool isOnline)
		{
			this.hardwareKey = hardwareKey;
			this.isOnline = isOnline;
			this.can1TransceiverType = CANTransceiverType.None;
			this.can2TransceiverType = CANTransceiverType.None;
			this.can3TransceiverType = CANTransceiverType.None;
			this.can4TransceiverType = CANTransceiverType.None;
			this.can5TransceiverType = CANTransceiverType.None;
			this.can6TransceiverType = CANTransceiverType.None;
			this.can7TransceiverType = CANTransceiverType.None;
			this.can8TransceiverType = CANTransceiverType.None;
			this.can9TransceiverType = CANTransceiverType.None;
			this.isAnalogExtensionBoardInstalled = false;
			this.isWLANExtensionBoardInstalled = false;
			this.installedWLANModelName = "";
		}

		public override bool Update()
		{
			this.isMemoryCardReady = base.IsWindowsFormattedMediaAccessible();
			if (!this.isMemoryCardReady)
			{
				this.hasLoggerInfo = false;
				return false;
			}
			bool flag = false;
			base.UpdateFromMlRtIniFile(out flag);
			if (flag)
			{
				this.can1TransceiverType = base.GetMlRtIniFileTransceiverType(1, CANTransceiverType.None);
				this.can2TransceiverType = base.GetMlRtIniFileTransceiverType(2, CANTransceiverType.None);
				this.can3TransceiverType = base.GetMlRtIniFileTransceiverType(3, CANTransceiverType.None);
				this.can4TransceiverType = base.GetMlRtIniFileTransceiverType(4, CANTransceiverType.None);
			}
			else
			{
				this.can1TransceiverType = CANTransceiverType.Unknown;
				this.can2TransceiverType = CANTransceiverType.Unknown;
				this.can3TransceiverType = CANTransceiverType.Unknown;
				this.can4TransceiverType = CANTransceiverType.Unknown;
			}
			this.can5TransceiverType = base.GetMlRtIniFileTransceiverType(5, CANTransceiverType.TJA1041A_or_TJA1043);
			this.can6TransceiverType = base.GetMlRtIniFileTransceiverType(6, CANTransceiverType.TJA1041A_or_TJA1043);
			this.can7TransceiverType = base.GetMlRtIniFileTransceiverType(7, CANTransceiverType.TJA1041A_or_TJA1043);
			this.can8TransceiverType = base.GetMlRtIniFileTransceiverType(8, CANTransceiverType.TJA1041A_or_TJA1043);
			this.can9TransceiverType = base.GetMlRtIniFileTransceiverType(9, CANTransceiverType.PCA82C251_or_TJA1042);
			this.isWLANExtensionBoardInstalled = base.IsWLANBoardInstalled(out this.installedWLANModelName);
			this.isAnalogExtensionBoardInstalled = base.IsAnalogInputBoardInstalled();
			return true;
		}

		public override bool FormatCard()
		{
			throw new NotImplementedException();
		}

		public override bool WriteLicense(string licenseFilePath)
		{
			if (base.InstallLicenseOnMemoryCard(base.HardwareKey, licenseFilePath))
			{
				InformMessageBox.Info(Resources.LicenseSuccessfullyWritten);
				return true;
			}
			return false;
		}

		public override bool SetRealTimeClock()
		{
			GL3000ctrl gL3000ctrl = new GL3000ctrl();
			string message = "";
			if (!gL3000ctrl.SetRealTimeClock(base.HardwareKey, out message))
			{
				InformMessageBox.Error(message);
				return false;
			}
			return true;
		}

		public override bool SetVehicleName(string name)
		{
			GL3000ctrl gL3000ctrl = new GL3000ctrl();
			string message = "";
			if (!this.isOnline)
			{
				InformMessageBox.Error(Resources.ErrorNoLoggerDevConnected);
				return false;
			}
			if (!gL3000ctrl.SetVehicleName(name, out message))
			{
				InformMessageBox.Error(message);
				return false;
			}
			return true;
		}

		public override bool GetCANTransceiverTypeForChannel(uint channelNr, out CANTransceiverType transceiverType)
		{
			transceiverType = CANTransceiverType.None;
			switch (channelNr)
			{
			case 1u:
				transceiverType = this.can1TransceiverType;
				return true;
			case 2u:
				transceiverType = this.can2TransceiverType;
				return true;
			case 3u:
				transceiverType = this.can3TransceiverType;
				return true;
			case 4u:
				transceiverType = this.can4TransceiverType;
				return true;
			case 5u:
				transceiverType = this.can5TransceiverType;
				return true;
			case 6u:
				transceiverType = this.can6TransceiverType;
				return true;
			case 7u:
				transceiverType = this.can7TransceiverType;
				return true;
			case 8u:
				transceiverType = this.can8TransceiverType;
				return true;
			case 9u:
				transceiverType = this.can9TransceiverType;
				return true;
			default:
				return false;
			}
		}
	}
}
