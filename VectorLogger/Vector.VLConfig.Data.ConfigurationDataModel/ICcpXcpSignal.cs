using System;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	public interface ICcpXcpSignal
	{
		ValidatedProperty<string> Name
		{
			get;
			set;
		}

		ValidatedProperty<string> EcuName
		{
			get;
			set;
		}
	}
}
