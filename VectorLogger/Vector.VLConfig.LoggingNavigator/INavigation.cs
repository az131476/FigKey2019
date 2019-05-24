using System;
using System.Collections.Generic;
using Vector.VLConfig.LoggingNavigator.Data;

namespace Vector.VLConfig.LoggingNavigator
{
	public interface INavigation
	{
		bool RecoverMeasurements
		{
			get;
			set;
		}

		void Clear();

		bool IsEmpty();

		bool ReadIndexFilesFromSource(string path);

		IList<Measurement> GetMeasurements();

		IList<Measurement> GetMeasurements(string loggerMemNumber);

		IList<string> GetVoiceRecordFiles();

		string[] GetAvailableLoggerMemories();

		bool HasMissingLogfiles();

		bool HasIndexError();

		ulong GetGlobalBegin(string indexFilePath);
	}
}
