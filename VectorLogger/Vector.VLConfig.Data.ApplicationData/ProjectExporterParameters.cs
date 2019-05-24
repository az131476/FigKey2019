using System;

namespace Vector.VLConfig.Data.ApplicationData
{
	public class ProjectExporterParameters
	{
		public bool ExportToMemoryCard
		{
			get;
			set;
		}

		public bool AskPriorToExport
		{
			get;
			set;
		}

		public bool ExportBusDatabases
		{
			get;
			set;
		}

		public bool ExportCCPXCPDatabases
		{
			get;
			set;
		}

		public bool ExportNonBusDatabases
		{
			get;
			set;
		}

		public bool ExportDiagnosticDescriptions
		{
			get;
			set;
		}

		public bool ExportIncludeFiles
		{
			get;
			set;
		}

		public bool ExportWebDisplay
		{
			get;
			set;
		}

		public bool ProtectWithPassword
		{
			get;
			set;
		}

		public bool SetPasswordOnDemand
		{
			get;
			set;
		}

		public string Password
		{
			get;
			set;
		}

		public ProjectExporterParameters(ProjectExporterParameters other)
		{
			this.ExportToMemoryCard = other.ExportToMemoryCard;
			this.AskPriorToExport = other.AskPriorToExport;
			this.ExportBusDatabases = other.ExportBusDatabases;
			this.ExportCCPXCPDatabases = other.ExportCCPXCPDatabases;
			this.ExportNonBusDatabases = other.ExportNonBusDatabases;
			this.ExportDiagnosticDescriptions = other.ExportDiagnosticDescriptions;
			this.ExportIncludeFiles = other.ExportIncludeFiles;
			this.ExportWebDisplay = other.ExportWebDisplay;
			this.ProtectWithPassword = other.ProtectWithPassword;
			this.SetPasswordOnDemand = other.SetPasswordOnDemand;
			this.Password = other.Password;
		}

		public ProjectExporterParameters()
		{
			this.ExportToMemoryCard = false;
			this.AskPriorToExport = false;
			this.ExportBusDatabases = true;
			this.ExportCCPXCPDatabases = true;
			this.ExportNonBusDatabases = true;
			this.ExportDiagnosticDescriptions = true;
			this.ExportIncludeFiles = true;
			this.ExportWebDisplay = true;
			this.ProtectWithPassword = false;
			this.SetPasswordOnDemand = false;
			this.Password = null;
		}

		public void RememberPassword(string password)
		{
			this.SetPasswordOnDemand = false;
			this.ProtectWithPassword = true;
			this.Password = password;
		}
	}
}
