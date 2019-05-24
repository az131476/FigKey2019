using System;
using System.Collections.Generic;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	public interface IFeatureEvents
	{
		IList<Event> GetEvents(bool activeEventsOnly);
	}
}
