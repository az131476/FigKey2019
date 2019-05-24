using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "DatabaseConfiguration")]
	public class DatabaseConfiguration : Feature, IFeatureReferencedFiles, IFeatureTransmitMessages
	{
		private List<IFile> referencedFiles;

		[DataMember(Name = "DatabaseConfigurationDatabaseList")]
		private List<Database> databaseList;

		public override IFeatureReferencedFiles ReferencedFiles
		{
			get
			{
				return this;
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
				return this;
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

		IList<IFile> IFeatureReferencedFiles.ReferencedFiles
		{
			get
			{
				if (this.referencedFiles == null)
				{
					this.referencedFiles = new List<IFile>();
				}
				this.referencedFiles.Clear();
				foreach (Database current in this.databaseList)
				{
					this.referencedFiles.Add(current);
					foreach (CPProtection current2 in current.CpProtections)
					{
						if (current2.HasSeedAndKey.Value)
						{
							this.referencedFiles.Add(current2);
						}
					}
				}
				return this.referencedFiles;
			}
		}

		IList<ITransmitMessageChannel> IFeatureTransmitMessages.ActiveTransmitMessageChannels
		{
			get
			{
				List<ITransmitMessageChannel> list = new List<ITransmitMessageChannel>();
				foreach (Database current in this.databaseList)
				{
					if (current.IsCPActive.Value)
					{
						list.Add(current);
					}
				}
				return list;
			}
		}

		public ReadOnlyCollection<Database> Databases
		{
			get
			{
				return new ReadOnlyCollection<Database>((from Database db in this.databaseList
				where db.FileType != DatabaseFileType.A2L
				select db).ToList<Database>());
			}
		}

		public ReadOnlyCollection<Database> BusDatabases
		{
			get
			{
				return new ReadOnlyCollection<Database>((from Database db in this.databaseList
				where db.CPType.Value == CPType.None && db.BusType.Value != BusType.Bt_None
				select db).ToList<Database>());
			}
		}

		public ReadOnlyCollection<Database> AllDescriptionFiles
		{
			get
			{
				return new ReadOnlyCollection<Database>(this.databaseList);
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

		[DataMember(Name = "DatabaseConfigurationEnableExchangeIdHandling")]
		public ValidatedProperty<bool> EnableExchangeIdHandling
		{
			get;
			set;
		}

		public override bool UpdateOnDependency(Feature otherFeature, IUpdateServiceForFeature updateService)
		{
			if (otherFeature is DatabaseConfiguration || otherFeature is CANChannelConfiguration || otherFeature is LINChannelConfiguration || otherFeature is FlexrayChannelConfiguration || otherFeature is MultibusChannelConfiguration)
			{
				updateService.Notify<DatabaseConfiguration>(this);
				return true;
			}
			return false;
		}

		public override void Update(IUpdateServiceForFeature updateService)
		{
			updateService.Notify<DatabaseConfiguration>(this);
		}

		public DatabaseConfiguration()
		{
			this.databaseList = new List<Database>();
			this.EnableExchangeIdHandling = new ValidatedProperty<bool>(true);
		}

		public Database AddDatabase()
		{
			Database database = new Database();
			this.databaseList.Add(database);
			return database;
		}

		public void AddDatabase(Database db)
		{
			this.databaseList.Add(db);
		}

		public Database GetDatabase(uint globalChannelNumber)
		{
			return this.databaseList.FirstOrDefault((Database db) => db.ChannelNumber.Value == globalChannelNumber);
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
				using (IEnumerator<Database> enumerator = this.Databases.GetEnumerator())
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

		public bool RemoveDatabase(Database database)
		{
			if (this.databaseList.Contains(database))
			{
				this.databaseList.Remove(database);
				return true;
			}
			return false;
		}

		public bool ReplaceDatabase(Database databaseToReplace, Database newDatabase)
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

		public IList<Database> GetDatabases(BusType busType, uint channelNumber)
		{
			List<Database> list = new List<Database>();
			foreach (Database current in this.Databases)
			{
				if (current.BusType.Value == busType && current.ChannelNumber.Value == channelNumber)
				{
					list.Add(current);
				}
			}
			return list;
		}

		public void ClearDatabases()
		{
			this.databaseList.Clear();
		}
	}
}
