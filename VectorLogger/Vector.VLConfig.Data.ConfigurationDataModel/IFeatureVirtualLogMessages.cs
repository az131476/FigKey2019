using System;
using System.Collections.Generic;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	public interface IFeatureVirtualLogMessages
	{
		IList<IVirtualCANLogMessage> ActiveVirtualCANLogMessages
		{
			get;
		}
	}
}
