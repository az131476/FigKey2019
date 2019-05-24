using System;

namespace Vector.VLConfig.LoggerSpecifics
{
	public interface ILoggerSpecifics
	{
		string Name
		{
			get;
		}

		LoggerType Type
		{
			get;
		}

		IMultibusSpecifics Multibus
		{
			get;
		}

		ICANSpecifics CAN
		{
			get;
		}

		ILINSpecifics LIN
		{
			get;
		}

		IFlexraySpecifics Flexray
		{
			get;
		}

		IDataStorageSpecifics DataStorage
		{
			get;
		}

		IFileConversionSpecifics FileConversion
		{
			get;
		}

		IIOSpecifics IO
		{
			get;
		}

		IGPSSpecifics GPS
		{
			get;
		}

		IDeviceAccessSpecifics DeviceAccess
		{
			get;
		}

		IRecordingSpecifics Recording
		{
			get;
		}

		IDataTransferSpecifics DataTransfer
		{
			get;
		}

		IConfigurationSpecifics Configuration
		{
			get;
		}
	}
}
