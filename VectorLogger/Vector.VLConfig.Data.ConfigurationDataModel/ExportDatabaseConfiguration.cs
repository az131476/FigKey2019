using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	public class ExportDatabaseConfiguration : Feature
	{
		public enum DBSelectionMode
		{
			FromConfig,
			Manual,
			FromAnalysisPackage
		}

		[DataMember(Name = "ExportDatabaseConfigurationDatabaseList")]
		private List<ExportDatabase> databaseList;

		public override IFeatureReferencedFiles ReferencedFiles
		{
			get
			{
				return this as IFeatureReferencedFiles;
			}
		}

		public override IFeatureSymbolicDefinitions SymbolicDefinitions
		{
			get
			{
				return null;
			}
		}

		public override IFeatureCcpXcpSignalDefinitions CcpXcpSignalDefinitions
		{
			get
			{
				return null;
			}
		}

		public override IFeatureTransmitMessages TransmitMessages
		{
			get
			{
				return null;
			}
		}

		public override IFeatureVirtualLogMessages VirtualLogMessages
		{
			get
			{
				return null;
			}
		}

		public override IFeatureEvents Events
		{
			get
			{
				return null;
			}
		}

		public ExportDatabaseConfiguration.DBSelectionMode Mode
		{
			get;
			set;
		}

		public bool IsExportDatabaseConfigurationEnabled
		{
			get
			{
				return this.Mode != ExportDatabaseConfiguration.DBSelectionMode.FromConfig;
			}
		}

		public List<ExportDatabase> AnalysisPackageDatabaseList
		{
			get;
			set;
		}

		public ReadOnlyCollection<ExportDatabase> ExportDatabases
		{
			get
			{
				if (this.Mode == ExportDatabaseConfiguration.DBSelectionMode.FromAnalysisPackage)
				{
					return new ReadOnlyCollection<ExportDatabase>(this.AnalysisPackageDatabaseList);
				}
				return this.Databases;
			}
		}

		public ReadOnlyCollection<ExportDatabase> Databases
		{
			get
			{
				return new ReadOnlyCollection<ExportDatabase>((from ExportDatabase db in this.databaseList
				where db.FileType != DatabaseFileType.A2L
				select db).ToList<ExportDatabase>());
			}
		}

		public ReadOnlyCollection<ExportDatabase> BusDatabases
		{
			get
			{
				return new ReadOnlyCollection<ExportDatabase>((from ExportDatabase db in this.databaseList
				where db.CPType.Value == CPType.None && db.BusType.Value != BusType.Bt_None
				select db).ToList<ExportDatabase>());
			}
		}

		public ReadOnlyCollection<ExportDatabase> AllDescriptionFiles
		{
			get
			{
				return new ReadOnlyCollection<ExportDatabase>(this.databaseList);
			}
		}

		public ReadOnlyCollection<Database> CCPXCPDatabases
		{
			get
			{
				return new ReadOnlyCollection<Database>((from Database db in this.databaseList
				where db.CPType.Value != CPType.None
				select db).ToList<Database>());
			}
		}

		public ReadOnlyCollection<Database> ActiveCCPXCPDatabases
		{
			get
			{
				return new ReadOnlyCollection<Database>((from Database db in this.databaseList
				where db.CPType.Value != CPType.None && db.IsCPActive.Value
				select db).ToList<Database>());
			}
		}

		public ReadOnlyCollection<Database> FlexrayDatabases
		{
			get
			{
				return new ReadOnlyCollection<Database>((from Database db in this.databaseList
				where db.BusType.Value == BusType.Bt_FlexRay && db.FileType != DatabaseFileType.A2L
				select db).ToList<Database>());
			}
		}

		public ReadOnlyCollection<Database> FlexrayFibexDatabases
		{
			get
			{
				return new ReadOnlyCollection<Database>((from Database db in this.databaseList
				where db.BusType.Value == BusType.Bt_FlexRay && db.FileType == DatabaseFileType.XML && !db.IsAUTOSARFile
				select db).ToList<Database>());
			}
		}

		public ReadOnlyCollection<Database> AllFlexrayDescriptionFiles
		{
			get
			{
				return new ReadOnlyCollection<Database>((from Database db in this.databaseList
				where db.BusType.Value == BusType.Bt_FlexRay
				select db).ToList<Database>());
			}
		}

		public override bool UpdateOnDependency(Feature otherFeature, IUpdateServiceForFeature updateService)
		{
			if (otherFeature is ExportDatabaseConfiguration)
			{
				updateService.Notify<ExportDatabaseConfiguration>(this);
				return true;
			}
			return false;
		}

		public override void Update(IUpdateServiceForFeature updateService)
		{
			updateService.Notify<ExportDatabaseConfiguration>(this);
		}

		public ExportDatabaseConfiguration()
		{
			this.databaseList = new List<ExportDatabase>();
			this.Mode = ExportDatabaseConfiguration.DBSelectionMode.FromConfig;
			this.AnalysisPackageDatabaseList = new List<ExportDatabase>();
		}

		public void AddDatabase(ExportDatabase db)
		{
			this.databaseList.Add(db);
		}

		public void AddDatabaseList(List<ExportDatabase> list)
		{
			this.databaseList.AddRange(list);
		}

		public Database GetDatabase(uint globalChannelNumber)
		{
			return this.databaseList.FirstOrDefault((ExportDatabase db) => db.ChannelNumber.Value == globalChannelNumber);
		}

		public IList<Database> GetDatabases(string filepath, string networkName)
		{
			List<Database> list = new List<Database>();
			foreach (Database current in this.databaseList)
			{
				if (current.FilePath.Value == filepath && (string.IsNullOrEmpty(networkName) || current.NetworkName.Value == networkName))
				{
					list.Add(current);
				}
			}
			return list;
		}

		public bool TryGetDatabase(string filePath, string networkName, uint channelNr, BusType busType, out Database database)
		{
			database = null;
			if (BusType.Bt_FlexRay != busType)
			{
				using (IEnumerator<ExportDatabase> enumerator = this.Databases.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Database current = enumerator.Current;
						if (current.FilePath.Value == filePath && (string.IsNullOrEmpty(networkName) || current.NetworkName.Value == networkName) && current.ChannelNumber.Value == channelNr && current.BusType.Value == busType)
						{
							database = current;
							bool result = true;
							return result;
						}
					}
					return false;
				}
			}
			foreach (Database current2 in this.Databases)
			{
				if (current2.FilePath.Value == filePath && (string.IsNullOrEmpty(networkName) || current2.NetworkName.Value == networkName) && (current2.ChannelNumber.Value == Database.ChannelNumber_FlexrayAB || current2.ChannelNumber.Value == channelNr))
				{
					database = current2;
					bool result = true;
					return result;
				}
			}
			return false;
		}

		public bool TryGetDatabaseByFileName(string fileName, uint channelNr, BusType busType, out Database database)
		{
			database = null;
			foreach (Database current in this.Databases)
			{
				if (!string.IsNullOrEmpty(current.FilePath.Value))
				{
					string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(current.FilePath.Value);
					if (fileNameWithoutExtension == fileName && current.ChannelNumber.Value == channelNr && current.BusType.Value == busType)
					{
						database = current;
						return true;
					}
				}
			}
			return false;
		}

		public bool RemoveDatabase(ExportDatabase database)
		{
			if (this.databaseList.Contains(database))
			{
				this.databaseList.Remove(database);
				return true;
			}
			return false;
		}

		public bool ReplaceDatabase(ExportDatabase databaseToReplace, ExportDatabase newDatabase)
		{
			if (this.databaseList.Contains(databaseToReplace))
			{
				try
				{
					int index = this.databaseList.IndexOf(databaseToReplace);
					this.databaseList.Remove(databaseToReplace);
					this.databaseList.Insert(index, newDatabase);
				}
				catch (ArgumentOutOfRangeException)
				{
					return false;
				}
				return true;
			}
			return false;
		}

		public bool IsDatabaseAvailableFor(BusType busType)
		{
			bool result = false;
			foreach (Database current in this.Databases)
			{
				if (current.BusType.Value == busType)
				{
					result = true;
					break;
				}
			}
			return result;
		}

		public bool IsDatabaseAvailableFor(BusType busType, uint channelNumber)
		{
			bool result = false;
			foreach (Database current in this.Databases)
			{
				if (current.BusType.Value == busType && current.ChannelNumber.Value == channelNumber)
				{
					result = true;
					break;
				}
			}
			return result;
		}

		public string GetAnalysisPackagePath()
		{
			string result = string.Empty;
			ExportDatabase exportDatabase = this.AnalysisPackageDatabaseList.FirstOrDefault((ExportDatabase x) => x.IsFromAnalysisPackage() && !string.IsNullOrEmpty(x.AnalysisPackagePath.Value));
			if (exportDatabase != null)
			{
				result = exportDatabase.AnalysisPackagePath.Value;
			}
			return result;
		}

		public void ClearDatabases()
		{
			this.databaseList.Clear();
		}

		public void ClearDatabasesFromAnalysisPackage()
		{
			this.databaseList.RemoveAll((ExportDatabase x) => x.IsFromAnalysisPackage());
		}
	}
}
