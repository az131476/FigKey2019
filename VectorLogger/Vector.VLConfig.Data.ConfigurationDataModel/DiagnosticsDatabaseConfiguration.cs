using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "DiagnosticsDatabaseConfiguration")]
	public class DiagnosticsDatabaseConfiguration : Feature, IFeatureReferencedFiles, IFeatureTransmitMessages, IDiagnosticsDatabaseConfiguration
	{
		private List<IFile> referencedFiles;

		[DataMember(Name = "DiagnosticsDatabaseConfigurationDatabaseList")]
		private List<DiagnosticsDatabase> databaseList;

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
				foreach (DiagnosticsDatabase current in this.databaseList)
				{
					this.referencedFiles.Add(current);
				}
				return this.referencedFiles;
			}
		}

		IList<ITransmitMessageChannel> IFeatureTransmitMessages.ActiveTransmitMessageChannels
		{
			get
			{
				List<ITransmitMessageChannel> list = new List<ITransmitMessageChannel>();
				foreach (DiagnosticsDatabase current in this.databaseList)
				{
					foreach (DiagnosticsECU current2 in current.ECUs)
					{
						list.Add(current2);
					}
				}
				return list;
			}
		}

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

		public override bool UpdateOnDependency(Feature otherFeature, IUpdateServiceForFeature updateService)
		{
			if (otherFeature is DiagnosticsDatabaseConfiguration || otherFeature is CANChannelConfiguration || otherFeature is LINChannelConfiguration || otherFeature is FlexrayChannelConfiguration)
			{
				updateService.Notify<DiagnosticsDatabaseConfiguration>(this);
				return true;
			}
			return false;
		}

		public override void Update(IUpdateServiceForFeature updateService)
		{
			updateService.Notify<DiagnosticsDatabaseConfiguration>(this);
		}

		public DiagnosticsDatabaseConfiguration()
		{
			this.databaseList = new List<DiagnosticsDatabase>();
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

		public bool ReplaceDatabase(DiagnosticsDatabase existingDb, DiagnosticsDatabase newDb)
		{
			int num = this.databaseList.IndexOf(existingDb);
			if (num < 0)
			{
				return false;
			}
			this.databaseList.Remove(existingDb);
			this.databaseList.Insert(num, newDb);
			return true;
		}

		public bool TryGetDiagnosticsDatabase(string databasePath, out DiagnosticsDatabase database)
		{
			database = null;
			foreach (DiagnosticsDatabase current in this.databaseList)
			{
				if (current.FilePath.Value == databasePath)
				{
					database = current;
					return true;
				}
			}
			return false;
		}
	}
}
