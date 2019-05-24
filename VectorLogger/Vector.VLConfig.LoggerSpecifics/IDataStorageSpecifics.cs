using System;
using System.Collections.Generic;

namespace Vector.VLConfig.LoggerSpecifics
{
	public interface IDataStorageSpecifics
	{
		bool IsUsingWindowsFileSystem
		{
			get;
		}

		uint NumberOfMemories
		{
			get;
		}

		uint MinLoggerFiles
		{
			get;
		}

		uint MaxLoggerFiles
		{
			get;
		}

		uint MaxLogFileSizeLimitationMB
		{
			get;
		}

		LogDataMemoryType DefaultLogDataMemoryType
		{
			get;
		}

		IList<LogDataMemoryType> LogDataMemoryTypes
		{
			get;
		}

		RingBufferOperatingMode DefaultOperatingMode
		{
			get;
		}

		bool HasDataCompression
		{
			get;
		}

		uint MinRingBufferSize
		{
			get;
		}

		uint MaxRingBufferSize
		{
			get;
		}

		uint DefaultRingBufferSize
		{
			get;
		}

		bool RingBufferSizeAppliesToPreTriggerTimeOnly
		{
			get;
		}

		uint HardDiskCapacity
		{
			get;
		}

		uint MaxMemoryCardSize
		{
			get;
		}

		bool HasOptimizeFileSystemSetting
		{
			get;
		}

		bool HasPackableLogData
		{
			get;
		}

		bool IsUsingStoreRAM
		{
			get;
		}

		bool DoCloseRingbufferAfterPowerOffInPermanentLogging
		{
			get;
		}

		string LogDataIniFileName
		{
			get;
		}

		string LogDataIniFile2Name
		{
			get;
		}

		string ErrorFilePath
		{
			get;
		}
	}
}
