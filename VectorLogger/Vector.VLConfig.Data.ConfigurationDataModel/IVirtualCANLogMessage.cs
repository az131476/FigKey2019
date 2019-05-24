using System;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	public interface IVirtualCANLogMessage
	{
		ValidatedProperty<uint> Id
		{
			get;
		}

		ValidatedProperty<bool> IsIdExtended
		{
			get;
		}

		ValidatedProperty<uint> ChannelNr
		{
			get;
		}
	}
}
