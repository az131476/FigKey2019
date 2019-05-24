using System;
using Vector.VLConfig.LoggerSpecifics;

namespace Vector.VLConfig.HardwareAccess.HardwareAbstraction
{
	public interface ILoggerDevice
	{
		LoggerType LoggerType
		{
			get;
		}

		ILoggerSpecifics LoggerSpecifics
		{
			get;
		}

		string HardwareKey
		{
			get;
		}

		string SerialNumber
		{
			get;
		}

		string VehicleName
		{
			get;
		}

		string Name
		{
			get;
		}

		string FirmwareVersion
		{
			get;
		}

		DateTime CompileDateTime
		{
			get;
		}

		string InstalledLicenses
		{
			get;
		}

		string LogDiskInfo
		{
			get;
		}

		bool IsOnline
		{
			get;
		}

		bool IsMemoryCardReady
		{
			get;
		}

		bool HasLoggerInfo
		{
			get;
		}

		bool HasConfiguration
		{
			get;
		}

		bool HasIndexFile
		{
			get;
		}

		bool HasErrorFile
		{
			get;
		}

		bool IsLocatedAtNetwork
		{
			get;
		}

		bool IsZipArchive
		{
			get;
		}

		bool HasSnapshotFolderContainingLogData
		{
			get;
		}

		string SnapshotFolderPath
		{
			get;
		}

		bool HasProperClusterSize
		{
			get;
		}

		bool IsFAT32Formatted
		{
			get;
		}

		ILogFileStorage LogFileStorage
		{
			get;
		}

		string GetGenericTransceiverTypeName(int channelNumber);

		bool Update();

		bool Clear();

		bool FormatCard();

		bool WriteConfiguration(out string codMD5Hash, bool showProgressBar);

		bool WriteConfiguration(string codFilePath, bool showProgressBar);

		bool WriteAnalysisPackage(string analysisPackagePath);

		bool WriteProjectZIPFile(string zipFilePath);

		string[] GetProjectZIPFilePath();

		bool DownloadProjectZIPFile();

		bool CopyAnalysisPackage(string destFolder);

		bool HasAnalysisPackage();

		string GetAnalysisPackagePath();

		string[] GetCODFilePath();

		bool SetRealTimeClock();

		bool SetVehicleName(string name);

		bool WriteLicense(string licenseFilePath);

		bool GetCANTransceiverTypeForChannel(uint channelNr, out CANTransceiverType transceiverType);
	}
}
