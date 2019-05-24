using System;

namespace Vector.VLConfig.LoggerSpecifics
{
	public interface IIOSpecifics
	{
		uint NumberOfLEDsTotal
		{
			get;
		}

		uint NumberOfLEDsOnBoard
		{
			get;
		}

		bool AnalogMapToSystemChannel
		{
			get;
		}

		bool AnalogMapToCANMessage
		{
			get;
		}

		uint NumberOfAnalogInputs
		{
			get;
		}

		uint NumberOfAnalogInputsOnboard
		{
			get;
		}

		uint NumberOfDigitalInputs
		{
			get;
		}

		bool IsDigitalOutputSupported
		{
			get;
		}

		bool IsDigitalInputOutputCommonPin
		{
			get;
		}

		uint NumberOfPanelKeys
		{
			get;
		}

		uint NumberOfKeys
		{
			get;
		}

		uint NumberOfCasKeys
		{
			get;
		}

		uint DefaultAnalogInputsChannel
		{
			get;
		}

		uint DefaultDigitalInputsChannel
		{
			get;
		}

		DigitalInputsMappingMode DefaultDigitalInputsMappingMode
		{
			get;
		}

		AnalogInputsCANMappingMode DefaultAnalogInputsMappingMode
		{
			get;
		}

		uint MaximumAnalogInputVoltage_mV
		{
			get;
		}
	}
}
