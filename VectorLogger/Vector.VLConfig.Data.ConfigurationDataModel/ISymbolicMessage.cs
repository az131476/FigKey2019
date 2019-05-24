using System;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	public interface ISymbolicMessage
	{
		ValidatedProperty<BusType> BusType
		{
			get;
			set;
		}

		ValidatedProperty<uint> ChannelNumber
		{
			get;
			set;
		}

		ValidatedProperty<string> DatabaseName
		{
			get;
			set;
		}

		ValidatedProperty<string> DatabasePath
		{
			get;
			set;
		}

		ValidatedProperty<string> NetworkName
		{
			get;
			set;
		}

		ValidatedProperty<string> MessageName
		{
			get;
			set;
		}
	}
}
