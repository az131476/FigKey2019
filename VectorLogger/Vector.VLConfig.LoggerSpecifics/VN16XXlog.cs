using System;
using System.Collections.Generic;

namespace Vector.VLConfig.LoggerSpecifics
{
	public class VN16XXlog : ILoggerSpecifics, IMultibusSpecifics, ICANSpecifics, ILINSpecifics, IFlexraySpecifics, IDataStorageSpecifics, IFileConversionSpecifics, IIOSpecifics, IGPSSpecifics, IDeviceAccessSpecifics, IRecordingSpecifics, IDataTransferSpecifics, IConfigurationSpecifics
	{
		private List<LogDataMemoryType> logDataMemoryTypes;

		private List<uint> defaultActiveCANChannels;

		private List<uint> canChannelsWithWakeUpSupport;

		private List<uint> canChannelsWithOptionalTransceivers;

		private List<uint> canChannelsWithOutputSupport;

		private List<uint> defaultActiveLINChannels;

		private List<uint> linChannelsWithWakeUpSupport;

		private List<uint> defaultActiveFlexrayChannels;

		private List<string> internalDriveModelNames;

		private readonly List<FileConversionDestFormat> fileConversionDestFormats = new List<FileConversionDestFormat>
		{
			FileConversionDestFormat.BinlogASC,
			FileConversionDestFormat.BinlogBLF,
			FileConversionDestFormat.MF4
		};

		private readonly List<FileConversionFilenameFormat> fileConversionFilenameFormat = new List<FileConversionFilenameFormat>
		{
			FileConversionFilenameFormat.UseOriginalName
		};

		string ILoggerSpecifics.Name
		{
			get
			{
				return "VN1630log";
			}
		}

		LoggerType ILoggerSpecifics.Type
		{
			get
			{
				return LoggerType.VN1630log;
			}
		}

		IMultibusSpecifics ILoggerSpecifics.Multibus
		{
			get
			{
				return this;
			}
		}

		ICANSpecifics ILoggerSpecifics.CAN
		{
			get
			{
				return this;
			}
		}

		ILINSpecifics ILoggerSpecifics.LIN
		{
			get
			{
				return this;
			}
		}

		IFlexraySpecifics ILoggerSpecifics.Flexray
		{
			get
			{
				return this;
			}
		}

		IDataStorageSpecifics ILoggerSpecifics.DataStorage
		{
			get
			{
				return this;
			}
		}

		IFileConversionSpecifics ILoggerSpecifics.FileConversion
		{
			get
			{
				return this;
			}
		}

		IIOSpecifics ILoggerSpecifics.IO
		{
			get
			{
				return this;
			}
		}

		IGPSSpecifics ILoggerSpecifics.GPS
		{
			get
			{
				return this;
			}
		}

		IDeviceAccessSpecifics ILoggerSpecifics.DeviceAccess
		{
			get
			{
				return this;
			}
		}

		IRecordingSpecifics ILoggerSpecifics.Recording
		{
			get
			{
				return this;
			}
		}

		IDataTransferSpecifics ILoggerSpecifics.DataTransfer
		{
			get
			{
				return this;
			}
		}

		IConfigurationSpecifics ILoggerSpecifics.Configuration
		{
			get
			{
				return this;
			}
		}

		uint IMultibusSpecifics.NumberOfChannels
		{
			get
			{
				return 4u;
			}
		}

		uint IMultibusSpecifics.NumberOfPiggyConfigurableChannels
		{
			get
			{
				return 2u;
			}
		}

		uint IMultibusSpecifics.PiggyConfigurableChannelsStartIndex
		{
			get
			{
				return 1u;
			}
		}

		uint IMultibusSpecifics.ChannelLINStartIndex
		{
			get
			{
				return 1u;
			}
		}

		uint IMultibusSpecifics.NumberOfBuildInCANTransceivers
		{
			get
			{
				return 2u;
			}
		}

		uint ICANSpecifics.NumberOfChannels
		{
			get
			{
				return 0u;
			}
		}

		uint ICANSpecifics.NumberOfVirtualChannels
		{
			get
			{
				return 0u;
			}
		}

		IList<uint> ICANSpecifics.DefaultActiveChannels
		{
			get
			{
				return this.defaultActiveCANChannels;
			}
		}

		IList<uint> ICANSpecifics.ChannelsWithWakeUpSupport
		{
			get
			{
				return this.canChannelsWithWakeUpSupport;
			}
		}

		IList<uint> ICANSpecifics.ChannelsWithOptionalTransceivers
		{
			get
			{
				return this.canChannelsWithOptionalTransceivers;
			}
		}

		IList<uint> ICANSpecifics.ChannelsWithOutputSupport
		{
			get
			{
				return this.canChannelsWithOutputSupport;
			}
		}

		uint ICANSpecifics.MaxPrescalerValue
		{
			get
			{
				return 0u;
			}
		}

		uint ICANSpecifics.AuxChannel
		{
			get
			{
				return 0u;
			}
		}

		uint ICANSpecifics.AuxChannelMaxPrescalerValue
		{
			get
			{
				return 0u;
			}
		}

		bool ICANSpecifics.IsFDSupported
		{
			get
			{
				return true;
			}
		}

		uint ILINSpecifics.NumberOfChannels
		{
			get
			{
				return 0u;
			}
		}

		IList<uint> ILINSpecifics.DefaultActiveChannels
		{
			get
			{
				return this.defaultActiveLINChannels;
			}
		}

		IList<uint> ILINSpecifics.ChannelsWithWakeUpSupport
		{
			get
			{
				return this.linChannelsWithWakeUpSupport;
			}
		}

		uint ILINSpecifics.NumberOfLINprobeChannels
		{
			get
			{
				return 0u;
			}
		}

		uint IFlexraySpecifics.NumberOfChannels
		{
			get
			{
				return 0u;
			}
		}

		IList<uint> IFlexraySpecifics.DefaultActiveChannels
		{
			get
			{
				return this.defaultActiveFlexrayChannels;
			}
		}

		bool IDataStorageSpecifics.IsUsingWindowsFileSystem
		{
			get
			{
				return true;
			}
		}

		uint IDataStorageSpecifics.NumberOfMemories
		{
			get
			{
				return 1u;
			}
		}

		uint IDataStorageSpecifics.MinLoggerFiles
		{
			get
			{
				return 2u;
			}
		}

		uint IDataStorageSpecifics.MaxLoggerFiles
		{
			get
			{
				return 2147483647u;
			}
		}

		uint IDataStorageSpecifics.MaxLogFileSizeLimitationMB
		{
			get
			{
				return 1024u;
			}
		}

		LogDataMemoryType IDataStorageSpecifics.DefaultLogDataMemoryType
		{
			get
			{
				return LogDataMemoryType.SDCard;
			}
		}

		IList<LogDataMemoryType> IDataStorageSpecifics.LogDataMemoryTypes
		{
			get
			{
				return this.logDataMemoryTypes;
			}
		}

		RingBufferOperatingMode IDataStorageSpecifics.DefaultOperatingMode
		{
			get
			{
				return RingBufferOperatingMode.overwriteOldest;
			}
		}

		bool IDataStorageSpecifics.HasDataCompression
		{
			get
			{
				return false;
			}
		}

		uint IDataStorageSpecifics.MinRingBufferSize
		{
			get
			{
				return 1024u;
			}
		}

		uint IDataStorageSpecifics.MaxRingBufferSize
		{
			get
			{
				return 32768u;
			}
		}

		uint IDataStorageSpecifics.DefaultRingBufferSize
		{
			get
			{
				return 32768u;
			}
		}

		bool IDataStorageSpecifics.RingBufferSizeAppliesToPreTriggerTimeOnly
		{
			get
			{
				return true;
			}
		}

		uint IDataStorageSpecifics.HardDiskCapacity
		{
			get
			{
				return 0u;
			}
		}

		uint IDataStorageSpecifics.MaxMemoryCardSize
		{
			get
			{
				return 33554432u;
			}
		}

		bool IDataStorageSpecifics.HasOptimizeFileSystemSetting
		{
			get
			{
				return false;
			}
		}

		bool IDataStorageSpecifics.HasPackableLogData
		{
			get
			{
				return false;
			}
		}

		bool IDataStorageSpecifics.IsUsingStoreRAM
		{
			get
			{
				return false;
			}
		}

		bool IDataStorageSpecifics.DoCloseRingbufferAfterPowerOffInPermanentLogging
		{
			get
			{
				return true;
			}
		}

		string IDataStorageSpecifics.LogDataIniFileName
		{
			get
			{
				return "";
			}
		}

		string IDataStorageSpecifics.LogDataIniFile2Name
		{
			get
			{
				return "";
			}
		}

		string IDataStorageSpecifics.ErrorFilePath
		{
			get
			{
				return "errlog.txt";
			}
		}

		IEnumerable<FileConversionDestFormat> IFileConversionSpecifics.DestFormats
		{
			get
			{
				return this.fileConversionDestFormats;
			}
		}

		IList<FileConversionFilenameFormat> IFileConversionSpecifics.FilenameFormats
		{
			get
			{
				return this.fileConversionFilenameFormat;
			}
		}

		FileConversionDestFormat IFileConversionSpecifics.DefaultDestFormat
		{
			get
			{
				return FileConversionDestFormat.BinlogASC;
			}
		}

		string IFileConversionSpecifics.RawFileFormat
		{
			get
			{
				return Constants.FileConversionRawFileFormatMF4U;
			}
		}

		bool IFileConversionSpecifics.HasAdvancedSettings
		{
			get
			{
				return false;
			}
		}

		bool IFileConversionSpecifics.HasChannelMapping
		{
			get
			{
				return false;
			}
		}

		bool IFileConversionSpecifics.HasSplittingOptions
		{
			get
			{
				return false;
			}
		}

		bool IFileConversionSpecifics.HasSelectableLogFiles
		{
			get
			{
				return true;
			}
		}

		bool IFileConversionSpecifics.HasExportDatabases
		{
			get
			{
				return false;
			}
		}

		uint IFileConversionSpecifics.NumberOfCanMappingChannels
		{
			get
			{
				return 8u;
			}
		}

		uint IFileConversionSpecifics.NumberOfLinMappingChannels
		{
			get
			{
				return 2u;
			}
		}

		uint IFileConversionSpecifics.NumberOfFlexRayMappingChannels
		{
			get
			{
				return 0u;
			}
		}

		bool IFileConversionSpecifics.IsNavigatorViewSupported
		{
			get
			{
				return false;
			}
		}

		bool IFileConversionSpecifics.IsQuickViewSupported
		{
			get
			{
				return false;
			}
		}

		uint IIOSpecifics.NumberOfLEDsTotal
		{
			get
			{
				return 0u;
			}
		}

		uint IIOSpecifics.NumberOfLEDsOnBoard
		{
			get
			{
				return 0u;
			}
		}

		bool IIOSpecifics.AnalogMapToSystemChannel
		{
			get
			{
				return false;
			}
		}

		bool IIOSpecifics.AnalogMapToCANMessage
		{
			get
			{
				return false;
			}
		}

		uint IIOSpecifics.NumberOfAnalogInputs
		{
			get
			{
				return 1u;
			}
		}

		uint IIOSpecifics.NumberOfAnalogInputsOnboard
		{
			get
			{
				return 0u;
			}
		}

		uint IIOSpecifics.NumberOfDigitalInputs
		{
			get
			{
				return 2u;
			}
		}

		bool IIOSpecifics.IsDigitalOutputSupported
		{
			get
			{
				return false;
			}
		}

		bool IIOSpecifics.IsDigitalInputOutputCommonPin
		{
			get
			{
				return false;
			}
		}

		uint IIOSpecifics.NumberOfPanelKeys
		{
			get
			{
				return 0u;
			}
		}

		uint IIOSpecifics.NumberOfKeys
		{
			get
			{
				return 0u;
			}
		}

		uint IIOSpecifics.NumberOfCasKeys
		{
			get
			{
				return 0u;
			}
		}

		uint IIOSpecifics.DefaultAnalogInputsChannel
		{
			get
			{
				return 3u;
			}
		}

		uint IIOSpecifics.DefaultDigitalInputsChannel
		{
			get
			{
				return 3u;
			}
		}

		DigitalInputsMappingMode IIOSpecifics.DefaultDigitalInputsMappingMode
		{
			get
			{
				return DigitalInputsMappingMode.ContinuousIndividualIDs;
			}
		}

		AnalogInputsCANMappingMode IIOSpecifics.DefaultAnalogInputsMappingMode
		{
			get
			{
				return AnalogInputsCANMappingMode.ContinuousIndividualIDs;
			}
		}

		uint IIOSpecifics.MaximumAnalogInputVoltage_mV
		{
			get
			{
				return 18000u;
			}
		}

		bool IGPSSpecifics.HasSerialGPS
		{
			get
			{
				return false;
			}
		}

		bool IGPSSpecifics.HasCANgpsSupport
		{
			get
			{
				return false;
			}
		}

		uint IGPSSpecifics.DefaultLogSerialGPSStartCANId
		{
			get
			{
				return 536870864u;
			}
		}

		bool IGPSSpecifics.DefaultLogSerialGPSIsExtendedStartCANId
		{
			get
			{
				return true;
			}
		}

		uint IGPSSpecifics.DefaultLogGPSChannel
		{
			get
			{
				return 3u;
			}
		}

		string IDeviceAccessSpecifics.DefaultDummySerialNumber
		{
			get
			{
				return "1630-001";
			}
		}

		uint IDeviceAccessSpecifics.DeviceType
		{
			get
			{
				return 5680u;
			}
		}

		bool IDeviceAccessSpecifics.IsUsingSecWrite
		{
			get
			{
				return false;
			}
		}

		bool IDeviceAccessSpecifics.AccessCardReaderDrivesOnly
		{
			get
			{
				return true;
			}
		}

		bool IDeviceAccessSpecifics.IsUSBConnectionSupported
		{
			get
			{
				return true;
			}
		}

		bool IDeviceAccessSpecifics.HasRealtimeClockAccessBySerialPort
		{
			get
			{
				return false;
			}
		}

		bool IDeviceAccessSpecifics.RequiresDisconnectAfterSettingRealtimeClock
		{
			get
			{
				return false;
			}
		}

		IList<string> IDeviceAccessSpecifics.InternalCardReaderModelNames
		{
			get
			{
				if (this.internalDriveModelNames == null)
				{
					this.internalDriveModelNames = new List<string>();
					this.internalDriveModelNames.Add("Vector Logger Drive");
				}
				return this.internalDriveModelNames;
			}
		}

		string IDeviceAccessSpecifics.VID_PID
		{
			get
			{
				return "";
			}
		}

		bool IDeviceAccessSpecifics.IsSetVehicleNameSupported
		{
			get
			{
				return false;
			}
		}

		bool IDeviceAccessSpecifics.IsWriteLicenseSupported
		{
			get
			{
				return false;
			}
		}

		bool IDeviceAccessSpecifics.IsUpdateFirmwareSupported
		{
			get
			{
				return true;
			}
		}

		bool IDeviceAccessSpecifics.IsMemoryCardFormattingSupported
		{
			get
			{
				return true;
			}
		}

		bool IRecordingSpecifics.HasFastwakeUp
		{
			get
			{
				return false;
			}
		}

		bool IRecordingSpecifics.IsLimitFilterSupported
		{
			get
			{
				return false;
			}
		}

		bool IRecordingSpecifics.HasMarkerSupport
		{
			get
			{
				return false;
			}
		}

		bool IRecordingSpecifics.HasEnhancedTriggerSupport
		{
			get
			{
				return false;
			}
		}

		bool IRecordingSpecifics.IsDiagnosticsSupported
		{
			get
			{
				return false;
			}
		}

		bool IRecordingSpecifics.IsVoCANSupported
		{
			get
			{
				return false;
			}
		}

		bool IRecordingSpecifics.IsCameraSupported
		{
			get
			{
				return false;
			}
		}

		bool IRecordingSpecifics.IsIgnitionEventSupported
		{
			get
			{
				return false;
			}
		}

		bool IRecordingSpecifics.IsCcpXcpSupported
		{
			get
			{
				return false;
			}
		}

		bool IRecordingSpecifics.IsCCPXCPSignalEventSupported
		{
			get
			{
				return false;
			}
		}

		bool IRecordingSpecifics.IsMOST150Supported
		{
			get
			{
				return false;
			}
		}

		bool IRecordingSpecifics.HasEthernet
		{
			get
			{
				return false;
			}
		}

		uint IRecordingSpecifics.MaxRawSignalLength
		{
			get
			{
				return 64u;
			}
		}

		uint IRecordingSpecifics.MaximumSignalLength
		{
			get
			{
				return 64u;
			}
		}

		uint IRecordingSpecifics.MaximumExportSignalLength
		{
			get
			{
				return 64u;
			}
		}

		uint IRecordingSpecifics.DefaultPreTriggerTimeSeconds
		{
			get
			{
				return 20u;
			}
		}

		uint IRecordingSpecifics.DefaultPostTriggerTimeMilliseconds
		{
			get
			{
				return 10000u;
			}
		}

		uint IRecordingSpecifics.DefaultLogDateTimeChannel
		{
			get
			{
				return 3u;
			}
		}

		uint IRecordingSpecifics.DefaultLogDateTimeCANId
		{
			get
			{
				return 536870896u;
			}
		}

		bool IRecordingSpecifics.DefaultLogDateTimeIsExtendedCANId
		{
			get
			{
				return true;
			}
		}

		bool IRecordingSpecifics.IsIncludeFilesSupported
		{
			get
			{
				return false;
			}
		}

		bool IDataTransferSpecifics.HasWLAN
		{
			get
			{
				return false;
			}
		}

		bool IDataTransferSpecifics.Has3G
		{
			get
			{
				return false;
			}
		}

		bool IDataTransferSpecifics.HasInterfaceMode
		{
			get
			{
				return false;
			}
		}

		bool IDataTransferSpecifics.HasWebServer
		{
			get
			{
				return false;
			}
		}

		bool IDataTransferSpecifics.IsMLserverSetupInLTL
		{
			get
			{
				return false;
			}
		}

		bool IDataTransferSpecifics.HasDifferentConnectionRequestTypes
		{
			get
			{
				return false;
			}
		}

		EnumCompilerType IConfigurationSpecifics.CompilerType
		{
			get
			{
				return EnumCompilerType.CAPL;
			}
		}

		string IConfigurationSpecifics.ConfigurationDirectoryName
		{
			get
			{
				return string.Empty;
			}
		}

		EnumCodeLanguage IConfigurationSpecifics.CodeLanguage
		{
			get
			{
				return EnumCodeLanguage.CAPL;
			}
		}

		bool IConfigurationSpecifics.IsARXMLDatabaseConfigurationSupported
		{
			get
			{
				return true;
			}
		}

		bool IConfigurationSpecifics.SupportsAnalysisPackage
		{
			get
			{
				return false;
			}
		}

		bool IConfigurationSpecifics.SupportsPackAndGoImport
		{
			get
			{
				return false;
			}
		}

		public static uint DefaultLogFileSize
		{
			get
			{
				return 100u;
			}
		}

		public VN16XXlog()
		{
			this.logDataMemoryTypes = new List<LogDataMemoryType>();
			this.logDataMemoryTypes.Add(LogDataMemoryType.SDCard);
			this.defaultActiveCANChannels = new List<uint>();
			this.canChannelsWithWakeUpSupport = new List<uint>();
			this.canChannelsWithOptionalTransceivers = new List<uint>();
			this.canChannelsWithOutputSupport = new List<uint>();
			this.defaultActiveLINChannels = new List<uint>();
			this.linChannelsWithWakeUpSupport = new List<uint>();
			this.defaultActiveFlexrayChannels = new List<uint>();
		}
	}
}
