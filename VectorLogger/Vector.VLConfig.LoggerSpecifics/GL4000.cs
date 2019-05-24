using System;
using System.Collections.Generic;

namespace Vector.VLConfig.LoggerSpecifics
{
	public class GL4000 : ILoggerSpecifics, IMultibusSpecifics, ICANSpecifics, ILINSpecifics, IFlexraySpecifics, IDataStorageSpecifics, IFileConversionSpecifics, IIOSpecifics, IGPSSpecifics, IDeviceAccessSpecifics, IRecordingSpecifics, IDataTransferSpecifics, IConfigurationSpecifics
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
				return "GL4X00";
			}
		}

		LoggerType ILoggerSpecifics.Type
		{
			get
			{
				return LoggerType.GL4000;
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
				return 9u;
			}
		}

		uint ICANSpecifics.NumberOfVirtualChannels
		{
			get
			{
				return 7u;
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
				return 9u;
			}
		}

		uint ICANSpecifics.AuxChannelMaxPrescalerValue
		{
			get
			{
				return 10u;
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
				return 14u;
			}
		}

		uint IFlexraySpecifics.NumberOfChannels
		{
			get
			{
				return 2u;
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
				return 2u;
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
				return LogDataMemoryType.CFCardUSB;
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
				return true;
			}
		}

		uint IDataStorageSpecifics.MinRingBufferSize
		{
			get
			{
				return 50u;
			}
		}

		uint IDataStorageSpecifics.MaxRingBufferSize
		{
			get
			{
				return 239616u;
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
				return 64u;
			}
		}

		uint IDataStorageSpecifics.MaxMemoryCardSize
		{
			get
			{
				return 67108864u;
			}
		}

		bool IDataStorageSpecifics.HasOptimizeFileSystemSetting
		{
			get
			{
				return true;
			}
		}

		bool IDataStorageSpecifics.HasPackableLogData
		{
			get
			{
				return true;
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
				return false;
			}
		}

		string IDataStorageSpecifics.LogDataIniFileName
		{
			get
			{
				return Constants.LogDataIniFileName;
			}
		}

		string IDataStorageSpecifics.LogDataIniFile2Name
		{
			get
			{
				return Constants.LogDataIniFile2Name;
			}
		}

		string IDataStorageSpecifics.ErrorFilePath
		{
			get
			{
				return "configure\\UpdateFailure.txt";
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
				return FileConversionDestFormat.ASC;
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
				return true;
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
				return 16u;
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
				return 4u;
			}
		}

		bool IFileConversionSpecifics.IsNavigatorViewSupported
		{
			get
			{
				return true;
			}
		}

		bool IFileConversionSpecifics.IsQuickViewSupported
		{
			get
			{
				return true;
			}
		}

		uint IIOSpecifics.NumberOfLEDsTotal
		{
			get
			{
				return 11u;
			}
		}

		uint IIOSpecifics.NumberOfLEDsOnBoard
		{
			get
			{
				return 5u;
			}
		}

		bool IIOSpecifics.AnalogMapToSystemChannel
		{
			get
			{
				return true;
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
				return 14u;
			}
		}

		uint IIOSpecifics.NumberOfAnalogInputsOnboard
		{
			get
			{
				return 6u;
			}
		}

		uint IIOSpecifics.NumberOfDigitalInputs
		{
			get
			{
				return 8u;
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
				return false;
			}
		}

		uint IIOSpecifics.NumberOfPanelKeys
		{
			get
			{
				return 4u;
			}
		}

		uint IIOSpecifics.NumberOfKeys
		{
			get
			{
				return 2u;
			}
		}

		uint IIOSpecifics.NumberOfCasKeys
		{
			get
			{
				return 1u;
			}
		}

		uint IIOSpecifics.DefaultAnalogInputsChannel
		{
			get
			{
				return 10u;
			}
		}

		uint IIOSpecifics.DefaultDigitalInputsChannel
		{
			get
			{
				return 10u;
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
				return true;
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
				return ((ICANSpecifics)this).AuxChannel;
			}
		}

		string IDeviceAccessSpecifics.DefaultDummySerialNumber
		{
			get
			{
				return "20100703-001";
			}
		}

		uint IDeviceAccessSpecifics.DeviceType
		{
			get
			{
				return 10272u;
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
				return true;
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
					this.internalDriveModelNames.Add("Linux File-Stor Gadget USB Device");
					this.internalDriveModelNames.Add("GL4000 Data Logger USB Device");
				}
				return this.internalDriveModelNames;
			}
		}

		string IDeviceAccessSpecifics.VID_PID
		{
			get
			{
				return "VID_09AF&PID_2820";
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
				return true;
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
				return true;
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
				return true;
			}
		}

		bool IRecordingSpecifics.IsCameraSupported
		{
			get
			{
				return true;
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
				return true;
			}
		}

		bool IRecordingSpecifics.HasEthernet
		{
			get
			{
				return true;
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
				return 10u;
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
				return true;
			}
		}

		bool IDataTransferSpecifics.Has3G
		{
			get
			{
				return true;
			}
		}

		bool IDataTransferSpecifics.HasInterfaceMode
		{
			get
			{
				return true;
			}
		}

		bool IDataTransferSpecifics.HasWebServer
		{
			get
			{
				return true;
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
				return true;
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
				return "configure";
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

		bool IConfigurationSpecifics.SupportsPackAndGoImport
		{
			get
			{
				return true;
			}
		}

		public GL4000()
		{
			this.logDataMemoryTypes = new List<LogDataMemoryType>();
			this.logDataMemoryTypes.Add(LogDataMemoryType.CFCardUSB);
			this.defaultActiveCANChannels = new List<uint>();
			this.defaultActiveCANChannels.Add(5u);
			this.defaultActiveCANChannels.Add(6u);
			this.defaultActiveCANChannels.Add(7u);
			this.defaultActiveCANChannels.Add(8u);
			this.defaultActiveCANChannels.Add(9u);
			this.canChannelsWithWakeUpSupport = new List<uint>();
			this.canChannelsWithWakeUpSupport.Add(1u);
			this.canChannelsWithWakeUpSupport.Add(2u);
			this.canChannelsWithWakeUpSupport.Add(3u);
			this.canChannelsWithWakeUpSupport.Add(4u);
			this.canChannelsWithWakeUpSupport.Add(5u);
			this.canChannelsWithWakeUpSupport.Add(6u);
			this.canChannelsWithWakeUpSupport.Add(7u);
			this.canChannelsWithWakeUpSupport.Add(8u);
			this.canChannelsWithOptionalTransceivers = new List<uint>();
			this.canChannelsWithOptionalTransceivers.Add(1u);
			this.canChannelsWithOptionalTransceivers.Add(2u);
			this.canChannelsWithOptionalTransceivers.Add(3u);
			this.canChannelsWithOptionalTransceivers.Add(4u);
			this.canChannelsWithOutputSupport = new List<uint>();
			this.canChannelsWithOutputSupport.Add(1u);
			this.canChannelsWithOutputSupport.Add(2u);
			this.canChannelsWithOutputSupport.Add(3u);
			this.canChannelsWithOutputSupport.Add(4u);
			this.canChannelsWithOutputSupport.Add(5u);
			this.canChannelsWithOutputSupport.Add(6u);
			this.canChannelsWithOutputSupport.Add(7u);
			this.canChannelsWithOutputSupport.Add(8u);
			this.canChannelsWithOutputSupport.Add(9u);
			this.defaultActiveLINChannels = new List<uint>();
			this.linChannelsWithWakeUpSupport = new List<uint>();
			this.linChannelsWithWakeUpSupport.Add(1u);
			this.linChannelsWithWakeUpSupport.Add(2u);
			this.defaultActiveFlexrayChannels = new List<uint>();
		}
	}
}
