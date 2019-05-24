using System;
using System.Collections.Generic;
using Vector.VLConfig.LoggingNavigator.Data;
using Vector.VLConfig.LoggingNavigator.Export;
using Vector.VLConfig.LoggingNavigator.GUI;

namespace Vector.VLConfig.LoggingNavigator
{
	public interface INavigationGUI
	{
		event EventHandler IsValidChanged;

		bool IsValid
		{
			get;
		}

		bool RecoverMeasurements
		{
			get;
			set;
		}

		void OpenIndexFilesFromSource(string path);

		bool GetExportJobs(out IList<ExportJob> jobs);

		bool IsEmpty();

		void SetActiveLoggerType(LoggerType type);

		void Close();

		ExportType GetExportType();

		LayoutSerializationContainer SerializeGrid();

		void DeSerializeGrid(LayoutSerializationContainer layout);

		bool HasMissingLogfiles();

		bool HasErrorInIndexFile();

		IEnumerable<string> RestoreMarkerTypeSelection(IEnumerable<string> markerTypeList);

		IEnumerable<string> GetMarkerTypeSelection();

		IEnumerable<string> RestoreTriggerTypeSelection(IEnumerable<string> triggerTypeList);

		IEnumerable<string> GetTriggerTypeSelection();

		string GetCurrentPath();
	}
}
