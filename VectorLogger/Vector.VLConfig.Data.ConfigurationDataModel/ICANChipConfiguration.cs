using System;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	public interface ICANChipConfiguration
	{
		bool IsCANFD
		{
			get;
		}

		ValidatedProperty<uint> SAM
		{
			get;
		}

		ValidatedProperty<uint> SJW
		{
			get;
		}

		ValidatedProperty<uint> TSeg1
		{
			get;
		}

		ValidatedProperty<uint> TSeg2
		{
			get;
		}

		ValidatedProperty<uint> Prescaler
		{
			get;
		}

		uint Baudrate
		{
			get;
		}
	}
}
