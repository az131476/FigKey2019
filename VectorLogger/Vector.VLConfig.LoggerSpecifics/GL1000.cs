using System;
using System.Collections.Generic;

namespace Vector.VLConfig.LoggerSpecifics
{
	public class GL1000 : ILoggerSpecifics, IMultibusSpecifics, ICANSpecifics, ILINSpecifics, IFlexraySpecifics, IDataStorageSpecifics, IFileConversionSpecifics, IIOSpecifics, IGPSSpecifics, IDeviceAccessSpecifics, IRecordingSpecifics, IDataTransferSpecifics, IConfigurationSpecifics
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
			FileConversionDestFormat.ASC,
			FileConversionDestFormat.BLF,
			FileConversionDestFormat.MDF,
			FileConversionDestFormat.TXT,
			FileConversionDestFormat.XLS,
			FileConversionDestFormat.IMG,
			FileConversionDestFormat.MAT,
			FileConversionDestFormat.HDF5,
			FileConversionDestFormat.CLF
		};

		private readonly List<FileConversionFilenameFormat> fileConversionFilenameFormat = new List<FileConversionFilenameFormat>
		{
			FileConversionFilenameFormat.UseOriginalName,
			FileConversionFilenameFormat.AddPrefix,
			FileConversionFilenameFormat.UseCustomName
		};

		string ILoggerSpecifics.Name
		{
			get
			{
				return "GL10X0";
			}
		}

		LoggerType ILoggerSpecifics.Type
		{
			get
			{
				return LoggerType.GL1000;
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
				return 0u;
			}
		}

		uint IMultibusSpecifics.NumberOfPiggyConfigurableChannels
		{
			get
			{
				return 0u;
			}
		}

		uint IMultibusSpecifics.PiggyConfigurableChannelsStartIndex
		{
			get
			{
				return 0u;
			}
		}

		uint IMultibusSpecifics.ChannelLINStartIndex
		{
			get
			{
				return 0u;
			}
		}

		uint IMultibusSpecifics.NumberOfBuildInCANTransceivers
		{
			get
			{
				return 0u;
			}
		}

		uint ICANSpecifics.NumberOfChannels
		{
			get
			{
				return 2u;
			}
		}

		uint ICANSpecifics.NumberOfVirtualChannels
		{
			get
			{
				return 6u;
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
				return 21u;
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
				return false;
			}
		}

		uint ILINSpecifics.NumberOfChannels
		{
			get
			{
				return 2u;
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
				return 10u;
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
				return false;
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
				return 30000u;
			}
		}

		uint IDataStorageSpecifics.MaxLogFileSizeLimitationMB
		{
			get
			{
				return ((IDataStorageSpecifics)this).MaxRingBufferSize / 1024u;
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
				return 2048u;
			}
		}

		uint IDataStorageSpecifics.MaxRingBufferSize
		{
			get
			{
				return 2097152u;
			}
		}

		uint IDataStorageSpecifics.DefaultRingBufferSize
		{
			get
			{
				return 5120u;
			}
		}

		bool IDataStorageSpecifics.RingBufferSizeAppliesToPreTriggerTimeOnly
		{
			get
			{
				return false;
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
				return true;
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
				return "";
			}
		}

		IEnumerable<FileConversionDestFormat> IFileConversionSpecifics.DestFormats
		{
			get
			{
				return this.fileConversionDestFormats;
			}
		}

		FileConversionDestFormat IFileConversionSpecifics.DefaultDestFormat
		{
			get
			{
				return FileConversionDestFormat.ASC;
			}
		}

		IList<FileConversionFilenameFormat> IFileConversionSpecifics.FilenameFormats
		{
			get
			{
				return this.fileConversionFilenameFormat;
			}
		}

		string IFileConversionSpecifics.RawFileFormat
		{
			get
			{
				return Constants.FileConversionRawFileFormatCLF;
			}
		}

		bool IFileConversionSpecifics.HasAdvancedSettings
		{
			get
			{
				return true;
			}
		}

		bool IFileConversionSpecifics.HasChannelMapping
		{
			get
			{
				return true;
			}
		}

		bool IFileConversionSpecifics.HasSplittingOptions
		{
			get
			{
				return true;
			}
		}

		bool IFileConversionSpecifics.HasSelectableLogFiles
		{
			get
			{
				return false;
			}
		}

		bool IFileConversionSpecifics.HasExportDatabases
		{
			get
			{
				return true;
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
				return 4u;
			}
		}

		uint IIOSpecifics.NumberOfLEDsOnBoard
		{
			get
			{
				return 4u;
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
				return true;
			}
		}

		uint IIOSpecifics.NumberOfAnalogInputs
		{
			get
			{
				return 4u;
			}
		}

		uint IIOSpecifics.NumberOfAnalogInputsOnboard
		{
			get
			{
				return 4u;
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
				return true;
			}
		}

		bool IIOSpecifics.IsDigitalInputOutputCommonPin
		{
			get
			{
				return true;
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
				return DigitalInputsMappingMode.GroupedCombinedIDs;
			}
		}

		AnalogInputsCANMappingMode IIOSpecifics.DefaultAnalogInputsMappingMode
		{
			get
			{
				return AnalogInputsCANMappingMode.SameFixedIDs;
			}
		}

		uint IIOSpecifics.MaximumAnalogInputVoltage_mV
		{
			get
			{
				return 16000u;
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
				return "20100701-001";
			}
		}

		uint IDeviceAccessSpecifics.DeviceType
		{
			get
			{
				return 9984u;
			}
		}

		bool IDeviceAccessSpecifics.IsUsingSecWrite
		{
			get
			{
				return true;
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
				return true;
			}
		}

		IList<string> IDeviceAccessSpecifics.InternalCardReaderModelNames
		{
			get
			{
				if (this.internalDriveModelNames == null)
				{
					this.internalDriveModelNames = new List<string>();
					this.internalDriveModelNames.Add("GiN mbH L1000 HS-SD USB Device");
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
				return true;
			}
		}

		bool IDeviceAccessSpecifics.IsWriteLicenseSupported
		{
			get
			{
				return true;
			}
		}

		bool IDeviceAccessSpecifics.IsUpdateFirmwareSupported
		{
			get
			{
				return false;
			}
		}

		bool IDeviceAccessSpecifics.IsMemoryCardFormattingSupported
		{
			get
			{
				return false;
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
				return true;
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
				return true;
			}
		}

		bool IRecordingSpecifics.IsDiagnosticsSupported
		{
			get
			{
				return true;
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
				return true;
			}
		}

		bool IRecordingSpecifics.IsCcpXcpSupported
		{
			get
			{
				return true;
			}
		}

		bool IRecordingSpecifics.IsCCPXCPSignalEventSupported
		{
			get
			{
				return true;
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
				return 16u;
			}
		}

		uint IRecordingSpecifics.MaximumSignalLength
		{
			get
			{
				return 32u;
			}
		}

		uint IRecordingSpecifics.MaximumExportSignalLength
		{
			get
			{
				return 16u;
			}
		}

		uint IRecordingSpecifics.DefaultPreTriggerTimeSeconds
		{
			get
			{
				return 0u;
			}
		}

		uint IRecordingSpecifics.DefaultPostTriggerTimeMilliseconds
		{
			get
			{
				return 0u;
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
				return true;
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
				return EnumCompilerType.LTL;
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
				return EnumCodeLanguage.LTL;
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
				return true;
			}
		}

		public bool SupportsPackAndGoImport
		{
			get
			{
				return true;
			}
		}

		public GL1000()
		{
			this.logDataMemoryTypes = new List<LogDataMemoryType>();
			this.logDataMemoryTypes.Add(LogDataMemoryType.SDCard);
			this.defaultActiveCANChannels = new List<uint>();
			this.defaultActiveCANChannels.Add(1u);
			this.defaultActiveCANChannels.Add(2u);
			this.canChannelsWithWakeUpSupport = new List<uint>();
			this.canChannelsWithOptionalTransceivers = new List<uint>();
			this.canChannelsWithOptionalTransceivers.Add(1u);
			this.canChannelsWithOptionalTransceivers.Add(2u);
			this.canChannelsWithOutputSupport = new List<uint>();
			this.canChannelsWithOutputSupport.Add(1u);
			this.canChannelsWithOutputSupport.Add(2u);
			this.defaultActiveLINChannels = new List<uint>();
			this.linChannelsWithWakeUpSupport = new List<uint>();
			this.defaultActiveFlexrayChannels = new List<uint>();
		}
	}
}
