using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "DiagnosticsDatabase")]
	public class DiagnosticsDatabase : IFile
	{
		[DataMember(Name = "DiagnosticsDatabaseType")]
		public ValidatedProperty<DiagDbType> Type;

		[DataMember(Name = "DiagnosticsDatabaseEcuList")]
		private List<DiagnosticsECU> ecuList;

		public ReadOnlyCollection<DiagnosticsECU> ECUs
		{
			get
			{
				return new ReadOnlyCollection<DiagnosticsECU>(this.ecuList);
			}
		}

		[DataMember(Name = "DiagnosticsDatabaseFilePath")]
		public ValidatedProperty<string> FilePath
		{
			get;
			set;
		}

		public uint TotalNumberOfEcusInFile
		{
			get;
			set;
		}

		public DiagnosticsDatabase()
		{
			this.Type = new ValidatedProperty<DiagDbType>(DiagDbType.None);
			this.FilePath = new ValidatedProperty<string>("");
			this.ecuList = new List<DiagnosticsECU>();
			this.TotalNumberOfEcusInFile = 0u;
		}

		public DiagnosticsDatabase(DiagnosticsDatabase other)
		{
			this.Type = new ValidatedProperty<DiagDbType>(other.Type.Value);
			this.FilePath = new ValidatedProperty<string>(other.FilePath.Value);
			this.ecuList = new List<DiagnosticsECU>();
			foreach (DiagnosticsECU current in other.ECUs)
			{
				this.AddECU(new DiagnosticsECU(current));
			}
			this.TotalNumberOfEcusInFile = other.TotalNumberOfEcusInFile;
		}

		public void AddECU(DiagnosticsECU ecu)
		{
			this.ecuList.Add(ecu);
			ecu.Database = this;
		}

		public bool RemoveECU(DiagnosticsECU ecu)
		{
			if (this.ecuList.Contains(ecu))
			{
				this.ecuList.Remove(ecu);
				ecu.Database = null;
				return true;
			}
			return false;
		}

		public void RemoveAllECUs()
		{
			this.ecuList.Clear();
		}

		public bool TryGetECU(string ecuQualifier, out DiagnosticsECU foundEcu)
		{
			foundEcu = null;
			foreach (DiagnosticsECU current in this.ecuList)
			{
				if (current.Qualifier.Value == ecuQualifier)
				{
					foundEcu = current;
					return true;
				}
			}
			return false;
		}
	}
}
