using System;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	public class ExportDatabase : Database
	{
		public enum ReferenceType
		{
			AnalysisRelative,
			AnalysisAbsolute,
			Path,
			DBconfig
		}

		public enum DBType
		{
			NonBus,
			Bus,
			CCPXCP,
			vSysVar
		}

		public ExportDatabase.ReferenceType RefType
		{
			get;
			set;
		}

		public string OriginalPath
		{
			get;
			set;
		}

		public bool IsGenerated
		{
			get;
			set;
		}

		public ExportDatabase.DBType Type
		{
			get;
			set;
		}

		public ValidatedProperty<string> AnalysisPackagePath
		{
			get;
			set;
		}

		public ExportDatabase()
		{
			this.RefType = ExportDatabase.ReferenceType.Path;
			this.OriginalPath = "";
			this.AnalysisPackagePath = new ValidatedProperty<string>(string.Empty);
			this.IsGenerated = false;
		}

		public ExportDatabase(ExportDatabase other) : base(other)
		{
			this.RefType = other.RefType;
			this.OriginalPath = other.OriginalPath;
			this.AnalysisPackagePath = new ValidatedProperty<string>(other.AnalysisPackagePath.Value);
			this.IsGenerated = other.IsGenerated;
		}

		public ExportDatabase(Database other) : base(other)
		{
			this.RefType = ExportDatabase.ReferenceType.Path;
			this.OriginalPath = "";
			this.AnalysisPackagePath = new ValidatedProperty<string>(string.Empty);
			this.IsGenerated = false;
		}

		public bool IsFromAnalysisPackage()
		{
			return this.RefType == ExportDatabase.ReferenceType.AnalysisRelative || this.RefType == ExportDatabase.ReferenceType.AnalysisAbsolute;
		}

		public bool IsInsideAnalysisPackage()
		{
			return this.RefType == ExportDatabase.ReferenceType.AnalysisRelative;
		}
	}
}
