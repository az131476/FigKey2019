using System;
using System.Collections.Generic;

namespace Vector.VLConfig.GUI
{
	public interface IFileConversionParametersExClient : IFileConversionParametersClient
	{
		IEnumerable<string> GetMarkerTypeSelection();

		IEnumerable<string> RestoreMarkerTypeSelection(IEnumerable<string> markers);

		IEnumerable<string> GetTriggerTypeSelection();

		IEnumerable<string> RestoreTriggerTypeSelection(IEnumerable<string> markers);
	}
}
