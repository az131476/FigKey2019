using System;

namespace Vector.VLConfig.LoggerSpecifics
{
	public class Constants
	{
		public static readonly uint MinimumRingBufferSize = 1u;

		public static readonly uint DefaultRingBufferSize = 5u;

		public static readonly uint DefaultMemoryCardSize = 2048u;

		public static readonly uint MinimumTimeoutToSleep = 1u;

		public static readonly uint MaximumTimeoutToSleep = 18000u;

		public static readonly uint DefaultTimeoutToSleep = 60u;

		public static readonly uint DefaultEventActivationDelay_ms = 2000u;

		public static readonly uint MaxEventActivationDelay_ms = 10000u;

		public static readonly uint MaximumExtendedCANId = 536870911u;

		public static readonly uint MaximumStandardCANId = 2047u;

		public static readonly uint CANDbIsExtendedIdMask = 2147483648u;

		public static readonly uint MaxCANDataBytes = 8u;

		public static readonly uint MaxCANFDDataBytes = 64u;

		public static readonly uint DefaultCANBaudrateHighSpeed = 500000u;

		public static readonly uint DefaultCANFDDataRate = 4000000u;

		public static readonly uint MaximumCANLowSpeedTransceiverRate = 125000u;

		public static readonly uint AuxChannelBaudrate = 500000u;

		public static readonly uint MaximumLINId = 63u;

		public static readonly uint MaxLINDataBytes = 8u;

		public static readonly uint DefaultLINBaudrate = 19200u;

		public static readonly int DefaultLINProtocolVersion = 6;

		public static readonly uint MaximumNumberOfLINprobeChannels = 14u;

		public static readonly uint DefaultJ1708Baudrate = 9600u;

		public static readonly uint MinimumFlexraySlotId = 1u;

		public static readonly uint MaximumFlexraySlotId = 2047u;

		public static readonly uint MaximumFlexrayBaseCycle = 63u;

		public static readonly string FlexrayChannelA_Postfix = "_Ch_A";

		public static readonly string FlexrayChannelB_Postfix = "_Ch_B";

		public static readonly uint DefaultAnalogInputFrequency = 1u;

		public static readonly uint DefaultAnalogInputMappedCANId = 536870897u;

		public static readonly uint NumberOfAnalogInputsOnOneMessage = 4u;

		public static readonly uint DefaultAnalogInputThreshold_mV = 12000u;

		public static readonly uint DefaultAnalogInputHysTolerance_mV = 500u;

		public static readonly uint MaximumAnalogInputHysTolerance_mV = 1000u;

		public static readonly int MinimumAnalogInputIntervalToMinMax_mV = 100;

		public static readonly bool AnalogInputsAveraging = true;

		public static readonly uint DefaultDigitalInputFrequency = 1u;

		public static readonly uint DefaultDigitalInputMappedCANId = 536870881u;

		public static readonly uint NumberOfDigitalInputsOnOneMessage = 4u;

		public static readonly uint MaxLimitInterval_ms = 60000u;

		public static readonly uint DefaultLimitInterval_ms = 1000u;

		public static readonly uint MaxCANBitPosition = 63u;

		public static readonly uint MaxCANFDBitPosition = 511u;

		public static readonly uint MaxLINBitPosition = 63u;

		public static readonly uint MinRawSignalLength = 1u;

		public static readonly uint DefaultAverageMsgsPerSec = 100u;

		public static readonly uint MaximumPostTriggerTime = 60000u;

		public static readonly uint MaximumVoCanRecordingDuration_s = 60u;

		public static readonly uint MinimumVoCanRecordingDuration_s = 1u;

		public static readonly uint DefaultVoCanRecordingDuration_s = 5u;

		public static readonly uint DefaultDigInDebaunceTime_ms = 50u;

		public static readonly uint MinimumTimerValue_ms = 100u;

		public static readonly uint DefaultUserDefinedCycleTime = 1000u;

		public static readonly uint DefaultMaxDelay = 20u;

		public static readonly uint OnMsgTimeoutMaxResultingDelay = 5000u;

		public static readonly string FileSystemFormatFAT32 = "FAT32";

		public static readonly string FileSystemFormatFAT = "FAT";

		public static readonly string FileSystemFormatNTFS = "NTFS";

		public static readonly int EarliestValidLogDateYear = 2000;

		public static readonly string DefaultRawLogDataFileName = "Trigger.clf";

		public static readonly string LogDataFilePrefixOnMemory1 = "Data1";

		public static readonly string LogDataFilePrefixOnMemory2 = "Data2";

		public static readonly string LogDataFilePrefixOnMemory = "Data{0}F";

		public static readonly string DriveRecFilePrefix = "Data0F";

		public static readonly string LogDataFolderSearchPattern = "!D?F*X";

		public static readonly string LogDataFileInSubFolderPrefixOnMemory = "D{0}F";

		public static readonly string DBCFileNameConvertedFromARXML = "{0}_{1}_{2}_converted.dbc";

		public static readonly string LogDataIniFileName = "ml_rt.ini";

		public static readonly string LogDataIniFile2Name = "ml_rt2.ini";

		public static readonly ulong CardSize512MB_Bytes = 536870912uL;

		public static readonly ulong CardSize1GB_Bytes = 1073741824uL;

		public static readonly ulong CardSize2GB_Bytes = (ulong)-2147483648;

		public static readonly ulong CardSize4GB_Bytes = 4294967296uL;

		public static readonly ulong CardSize8GB_Bytes = 8589934592uL;

		public static readonly ulong CardSize16GB_Bytes = 17179869184uL;

		public static readonly ulong ClusterSize512_Bytes = 512uL;

		public static readonly ulong ClusterSize1k_Bytes = 1024uL;

		public static readonly ulong ClusterSize2k_Bytes = 2048uL;

		public static readonly ulong ClusterSize4k_Bytes = 4096uL;

		public static readonly ulong ClusterSize8k_Bytes = 8192uL;

		public static readonly ulong ClusterSize16k_Bytes = 16384uL;

		public static readonly ulong ClusterSize32k_Bytes = 32768uL;

		public static readonly ulong ClusterSize64k_Bytes = 65536uL;

		public static readonly uint MinimumP2Timeout = 1u;

		public static readonly uint MaximumP2Timeout = 60000u;

		public static readonly uint MinimumP2Extension = 1u;

		public static readonly uint MaximumP2Extension = 60000u;

		public static readonly uint MinimumTestPresentTime = 1u;

		public static readonly uint MaximumTestPresentTime = 60000u;

		public static readonly byte MinimumFirstConsecFrameValue = 32;

		public static readonly byte MaximumFirstConsecFrameValue = 47;

		public static readonly uint DefaultP2Timeout = 250u;

		public static readonly uint DefaultP2Extension = 2000u;

		public static readonly uint DefaultTesterPresentTime = 1000u;

		public static readonly uint DefaultTesterPresentMsg = 0u;

		public static readonly uint DefaultPhysRequestMsgId = 0u;

		public static readonly bool DefaultPhysRequestMsgIsExtendedId = false;

		public static readonly uint DefaultResponseMsgId = 0u;

		public static readonly bool DefaultResponseMsgIsExtendedId = false;

		public static readonly uint DefaultSTMin = 0u;

		public static readonly uint MinimumSTMin = 0u;

		public static readonly uint MaximumSTMin = 127u;

		public static readonly bool DefaultUsePaddingBytes = true;

		public static readonly byte DefaultPaddingByteValue = 0;

		public static readonly byte DefaultFirstConsecFrameValue = 33;

		public static readonly string ServiceIDRequestParam = "SID_RQ";

		public static readonly ulong SessionId_KWPDefault = 4225uL;

		public static readonly ulong SessionId_UDSDefault = 4097uL;

		public static readonly ulong SessionId_UDSExtended = 4099uL;

		public static readonly ulong SessionId_InitialDefaultUserDef = 4097uL;

		public static readonly ulong SessionId_InitialExtendedUserDef = 4099uL;

		public static readonly uint MinimumOnStartDelay = 0u;

		public static readonly uint MaximumOnStartDelay = 3000u;

		public static readonly uint MinimumOnStartDelayCycles = 0u;

		public static readonly uint MaximumOnStartDelayCycles = 255u;

		public static readonly uint DefaultOnStartDelay = 10000u;

		public static readonly uint MinimumOnCyclicTimer = 1u;

		public static readonly uint MaximumOnCyclicTimer_ms = 60000u;

		public static readonly uint MaximumOnCyclicTimer_s = 600u;

		public static readonly uint MaximumOnCyclicTimer_m = 60u;

		public static readonly uint DefaultOnCyclicTimer_ms = 1000u;

		public static readonly uint DefaultOnCyclicTimer_s = 60u;

		public static readonly uint DefaultOnCyclicTimer_m = 1u;

		public static readonly uint MinimumDiagCommRestartTime = 1u;

		public static readonly uint MaximumDiagCommRestartTime = 60u;

		public static readonly string ManufacturerName_VW = "vw";

		public static readonly string DefaultInterfaceModeIPAddr = "127.0.0.1";

		public static readonly string DefaultInterfaceModeSubnetMask = "255.255.255.0";

		public static readonly uint DefaultInterfaceModePort = 2900u;

		public static readonly uint MinInterfaceModePort = 2000u;

		public static readonly uint MaxInterfaceModePort = 65535u;

		public static readonly uint DefaultInterfaceModeMarkerCANId = 536870895u;

		public static readonly uint ExportCycle_Min = 50u;

		public static readonly uint ExportCycle_Max = 5000u;

		public static readonly string DefaultWlanLoggerIp = "192.168.13.30";

		public static readonly string DefaultWlanGatewayIp = "192.168.13.31";

		public static readonly string DefaultWlanSubnetMask = "255.255.255.0";

		public static readonly string DefaultWlanMLserverIp = "127.0.0.1";

		public static readonly uint SerialNumberOffsetGL1010 = 100000u;

		public static readonly uint MaxSerialNumberGL1000_DeviantHwBehaviour = 1339u;

		public static readonly uint MaxSerialNumberGL1010_DeviantHwBehaviour = 100105u;

		public static readonly uint MinCcpXcpEthernetPort = 1u;

		public static readonly uint MaxCcpXcpEthernetPort = 65535u;

		public static readonly uint MinDaqTimeout = 20u;

		public static readonly uint MaxDaqTimeout = 30000u;

		public static readonly uint DefaultDaqTimeout = 1000u;

		public static readonly uint MinCcpXcpCTO = 8u;

		public static readonly uint MinCcpXcpDTO = 8u;

		public static readonly uint MaxCcpXcpCanCTO = 8u;

		public static readonly uint MaxCcpXcpCanDTO = 8u;

		public static readonly uint MaxCcpXcpFlexRayCTO = 254u;

		public static readonly uint MaxCcpXcpFlexRayDTO = 254u;

		public static readonly uint MaxCcpXcpEthernetCTO = 255u;

		public static readonly uint MaxCcpXcpEthernetDTO = 65535u;

		public static readonly uint MaxCcpXcpUdpDTO = 466u;

		public static readonly uint DefaultCcpXcpUdpDTO = 224u;

		public static readonly uint DefaultCcpXcpUdpCTO = 64u;

		public static readonly uint MinCcpXcpFlexRayNodeAdress = 0u;

		public static readonly uint MaxCcpXcpFlexRayNodeAdress = 255u;

		public static readonly byte MaxDaqPidCcp = 253;

		public static readonly byte MaxDaqPidXcp = 251;

		public static readonly uint MaxCcpXcpPollingCycleBytes = 1000u;

		public static readonly uint MaxCcpXcpPollingCycles = 32u;

		public static readonly uint MaxCcpXcpPollingSignalsPerTriggeredList = (uint)(Constants.MaxDaqPidXcp - 1);

		public static readonly uint MinCcpXcpEcuTimestampTicks = 1u;

		public static readonly uint MaxCcpXcpEcuTimestampTicks = 4294967295u;

		public static readonly decimal MaxMinimumDigitsForTriggerIndex = 15m;

		public static readonly int DefaultMinimumDigitsForTriggerIndex = 3;

		public static readonly string FileConversionRawFileFormatCLF = "clf";

		public static readonly string FileConversionRawFileFormatMF4U = "mf4u";

		public static readonly uint StartIndexForRemoteLeds = 5u;

		public static readonly uint CasKeyOffset = 7u;
	}
}
