using System;
using System.Windows.Forms;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.HardwareAccess.ToolInterfaces;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.HardwareAccess.HardwareAbstraction
{
	public class GL2000Device : LoggerDeviceWindowsFileSystem, IGL2000Device
	{
		private CANTransceiverType can1TransceiverType;

		private CANTransceiverType can2TransceiverType;

		private CANTransceiverType can3TransceiverType;

		private CANTransceiverType can4TransceiverType;

		private bool hasProperClusterSize;

		private ulong properClusterSizeInBytes;

		public override LoggerType LoggerType
		{
			get
			{
				return LoggerType.GL2000;
			}
		}

		public override ILoggerSpecifics LoggerSpecifics
		{
			get
			{
				return GL2000Scanner.LoggerSpecifics;
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
				return this.hasProperClusterSize;
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

		public GL2000Device(string hardwareKey, bool isOnline)
		{
			this.hardwareKey = hardwareKey;
			this.isOnline = isOnline;
			this.can1TransceiverType = CANTransceiverType.None;
			this.can2TransceiverType = CANTransceiverType.None;
			this.can3TransceiverType = CANTransceiverType.None;
			this.can4TransceiverType = CANTransceiverType.None;
			this.hasProperClusterSize = true;
			this.properClusterSizeInBytes = 0uL;
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
			this.can1TransceiverType = base.GetMlRtIniFileTransceiverType(1, CANTransceiverType.TJA1043);
			this.can2TransceiverType = base.GetMlRtIniFileTransceiverType(2, CANTransceiverType.TJA1043);
			if (flag)
			{
				this.can3TransceiverType = base.GetMlRtIniFileTransceiverType(3, CANTransceiverType.None);
				this.can4TransceiverType = base.GetMlRtIniFileTransceiverType(4, CANTransceiverType.None);
			}
			else
			{
				this.can3TransceiverType = CANTransceiverType.Unknown;
				this.can4TransceiverType = CANTransceiverType.Unknown;
			}
			this.DetermineProperClusterSize();
			return true;
		}

		public override bool FormatCard()
		{
			if (InformMessageBox.Question(string.Format(Resources.QuestionStartOfFormatting, this.hardwareKey)) == DialogResult.No)
			{
				return false;
			}
			FormatMemoryCard formatMemoryCard = new FormatMemoryCard();
			if (formatMemoryCard.FormatFAT32(base.HardwareKey, this.properClusterSizeInBytes, "GL2000_Card"))
			{
				InformMessageBox.Info(Resources.MemCardFormatSuccess);
				return true;
			}
			InformMessageBox.Error(Resources.MemCardFormatFailed);
			return false;
		}

		public override bool WriteLicense(string licenseFilePath)
		{
			string message = "";
			SecWrite secWrite = new SecWrite();
			if (!secWrite.InstallLicenseFile(licenseFilePath, this.LoggerSpecifics.DeviceAccess.DeviceType, out message))
			{
				InformMessageBox.Error(message);
				return false;
			}
			InformMessageBox.Info(Resources.LicenseSuccessfullyWritten);
			return true;
		}

		public override bool SetRealTimeClock()
		{
			GL2000ctrl gL2000ctrl = new GL2000ctrl();
			string message = "";
			if (!gL2000ctrl.SetRealTimeClock(base.HardwareKey, out message))
			{
				InformMessageBox.Error(message);
				return false;
			}
			return true;
		}

		public override bool SetVehicleName(string name)
		{
			GL2000ctrl gL2000ctrl = new GL2000ctrl();
			string message = "";
			if (!this.isOnline)
			{
				InformMessageBox.Error(Resources.ErrorNoLoggerDevConnected);
				return false;
			}
			if (!gL2000ctrl.SetVehicleName(name, out message))
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
			default:
				return false;
			}
		}

		private void DetermineProperClusterSize()
		{
			ulong num;
			ulong num2;
			if (FileSystemServices.GetMemoryCardClusterSize(this.hardwareKey, out num, out num2))
			{
				if (num2 <= Constants.CardSize512MB_Bytes)
				{
					this.properClusterSizeInBytes = Constants.ClusterSize4k_Bytes;
				}
				else if (num2 <= Constants.CardSize1GB_Bytes)
				{
					this.properClusterSizeInBytes = Constants.ClusterSize8k_Bytes;
				}
				else if (num2 <= Constants.CardSize2GB_Bytes)
				{
					this.properClusterSizeInBytes = Constants.ClusterSize16k_Bytes;
				}
				else
				{
					this.properClusterSizeInBytes = Constants.ClusterSize32k_Bytes;
				}
				this.hasProperClusterSize = (this.properClusterSizeInBytes == num);
			}
		}
	}
}
