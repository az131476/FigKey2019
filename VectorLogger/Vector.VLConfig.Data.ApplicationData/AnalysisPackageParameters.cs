using System;

namespace Vector.VLConfig.Data.ApplicationData
{
	public class AnalysisPackageParameters
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

		public bool StoreBusDatabases
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

		public string PoolPath
		{
			get;
			set;
		}

		public bool DoNotAskToRemoveExistingDatabases
		{
			get;
			set;
		}

		public bool AutomaticallyRemoveExistingDatabases
		{
			get;
			set;
		}

		public AnalysisPackageParameters(AnalysisPackageParameters other)
		{
			this.ExportToMemoryCard = other.ExportToMemoryCard;
			this.AskPriorToExport = other.AskPriorToExport;
			this.StoreBusDatabases = other.StoreBusDatabases;
			this.ProtectWithPassword = other.ProtectWithPassword;
			this.SetPasswordOnDemand = other.SetPasswordOnDemand;
			this.Password = other.Password;
			this.PoolPath = other.PoolPath;
			this.DoNotAskToRemoveExistingDatabases = other.DoNotAskToRemoveExistingDatabases;
			this.AutomaticallyRemoveExistingDatabases = other.AutomaticallyRemoveExistingDatabases;
		}

		public AnalysisPackageParameters()
		{
			this.ExportToMemoryCard = false;
			this.AskPriorToExport = false;
			this.StoreBusDatabases = true;
			this.ProtectWithPassword = false;
			this.SetPasswordOnDemand = false;
			this.Password = "";
			this.DoNotAskToRemoveExistingDatabases = false;
			this.AutomaticallyRemoveExistingDatabases = false;
			this.PoolPath = "";
		}

		public void RememberPassword(string password)
		{
			this.SetPasswordOnDemand = false;
			this.ProtectWithPassword = true;
			this.Password = password;
		}
	}
}
