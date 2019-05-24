using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.GUI.DiagnosticsDatabasesPage
{
	public class DiagnosticsDatabaseConfigurationInternal : IDiagnosticsDatabaseConfiguration
	{
		private List<DiagnosticsDatabase> databaseList;

		public ReadOnlyCollection<DiagnosticsDatabase> Databases
		{
			get
			{
				return new ReadOnlyCollection<DiagnosticsDatabase>(this.databaseList);
			}
		}

		public ReadOnlyCollection<DiagnosticsECU> ECUs
		{
			get
			{
				List<DiagnosticsECU> list = new List<DiagnosticsECU>();
				foreach (DiagnosticsDatabase current in this.databaseList)
				{
					foreach (DiagnosticsECU current2 in current.ECUs)
					{
						list.Add(current2);
					}
				}
				return new ReadOnlyCollection<DiagnosticsECU>(list);
			}
		}

		public DiagnosticsDatabaseConfigurationInternal()
		{
			this.databaseList = new List<DiagnosticsDatabase>();
		}

		public DiagnosticsDatabaseConfigurationInternal(DiagnosticsDatabaseConfiguration other)
		{
			this.databaseList = new List<DiagnosticsDatabase>();
			foreach (DiagnosticsDatabase current in other.Databases)
			{
				this.databaseList.Add(new DiagnosticsDatabase(current));
			}
		}

		public void AddDatabase(DiagnosticsDatabase db)
		{
			this.databaseList.Add(db);
		}

		public bool RemoveDatabase(DiagnosticsDatabase db)
		{
			if (this.databaseList.Contains(db))
			{
				this.databaseList.Remove(db);
				return true;
			}
			return false;
		}

		public void RemoveAllDatabases()
		{
			this.databaseList.Clear();
		}
	}
}
