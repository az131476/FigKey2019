using System;

namespace Vector.VLConfig.LoggerSpecifics
{
	public interface IRecordingSpecifics
	{
		bool HasFastwakeUp
		{
			get;
		}

		bool IsLimitFilterSupported
		{
			get;
		}

		bool HasMarkerSupport
		{
			get;
		}

		bool HasEnhancedTriggerSupport
		{
			get;
		}

		bool IsDiagnosticsSupported
		{
			get;
		}

		bool IsVoCANSupported
		{
			get;
		}

		bool IsCameraSupported
		{
			get;
		}

		bool IsIgnitionEventSupported
		{
			get;
		}

		bool IsCcpXcpSupported
		{
			get;
		}

		bool IsCCPXCPSignalEventSupported
		{
			get;
		}

		bool IsMOST150Supported
		{
			get;
		}

		bool HasEthernet
		{
			get;
		}

		bool IsIncludeFilesSupported
		{
			get;
		}

		uint MaxRawSignalLength
		{
			get;
		}

		uint MaximumSignalLength
		{
			get;
		}

		uint MaximumExportSignalLength
		{
			get;
		}

		uint DefaultPreTriggerTimeSeconds
		{
			get;
		}

		uint DefaultPostTriggerTimeMilliseconds
		{
			get;
		}

		uint DefaultLogDateTimeChannel
		{
			get;
		}

		uint DefaultLogDateTimeCANId
		{
			get;
		}

		bool DefaultLogDateTimeIsExtendedCANId
		{
			get;
		}
	}
}
