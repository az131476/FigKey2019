using System;
using System.Collections.Generic;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	public interface IFeatureCcpXcpSignalDefinitions
	{
		IList<ICcpXcpSignal> CcpXcpSignals
		{
			get;
		}
	}
}
