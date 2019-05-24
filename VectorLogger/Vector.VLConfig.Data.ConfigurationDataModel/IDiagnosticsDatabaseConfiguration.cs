using System;
using System.Collections.ObjectModel;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	public interface IDiagnosticsDatabaseConfiguration
	{
		ReadOnlyCollection<DiagnosticsDatabase> Databases
		{
			get;
		}

		ReadOnlyCollection<DiagnosticsECU> ECUs
		{
			get;
		}

		void AddDatabase(DiagnosticsDatabase db);

		bool RemoveDatabase(DiagnosticsDatabase db);

		void RemoveAllDatabases();
	}
}
