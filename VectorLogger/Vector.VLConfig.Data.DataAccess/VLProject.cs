using System;
using System.Collections.Generic;
using System.IO;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.Data.Persistency;
using Vector.VLConfig.LoggerSpecifics;

namespace Vector.VLConfig.Data.DataAccess
{
	public class VLProject : IFeatureRegistration
	{
		private static VLProject _vlProject;

		private List<Feature> features;

		private ProjectRoot projectRoot;

		private string filePath;

		private bool isImported;

		public static IFeatureRegistration FeatureRegistration
		{
			get
			{
				return VLProject._vlProject;
			}
		}

		IList<Feature> IFeatureRegistration.Features
		{
			get
			{
				return this.features;
			}
		}

		IList<IFeatureReferencedFiles> IFeatureRegistration.FeaturesUsingReferencedFiles
		{
			get
			{
				List<IFeatureReferencedFiles> list = new List<IFeatureReferencedFiles>();
				foreach (Feature current in this.features)
				{
					if (current is IFeatureReferencedFiles)
					{
						list.Add(current as IFeatureReferencedFiles);
					}
				}
				return list;
			}
		}

		IList<IFeatureSymbolicDefinitions> IFeatureRegistration.FeaturesUsingSymbolicDefinitions
		{
			get
			{
				List<IFeatureSymbolicDefinitions> list = new List<IFeatureSymbolicDefinitions>();
				foreach (Feature current in this.features)
				{
					if (current is IFeatureSymbolicDefinitions)
					{
						list.Add(current as IFeatureSymbolicDefinitions);
					}
				}
				return list;
			}
		}

		IList<IFeatureCcpXcpSignalDefinitions> IFeatureRegistration.FeaturesUsingCcpXcpSignalDefinitions
		{
			get
			{
				List<IFeatureCcpXcpSignalDefinitions> list = new List<IFeatureCcpXcpSignalDefinitions>();
				foreach (Feature current in this.features)
				{
					if (current is IFeatureCcpXcpSignalDefinitions)
					{
						list.Add(current as IFeatureCcpXcpSignalDefinitions);
					}
				}
				return list;
			}
		}

		IList<IFeatureTransmitMessages> IFeatureRegistration.FeaturesUsingTransmitMessages
		{
			get
			{
				List<IFeatureTransmitMessages> list = new List<IFeatureTransmitMessages>();
				foreach (Feature current in this.features)
				{
					if (current is IFeatureTransmitMessages)
					{
						list.Add(current as IFeatureTransmitMessages);
					}
				}
				return list;
			}
		}

		IList<IFeatureVirtualLogMessages> IFeatureRegistration.FeaturesUsingVirtualLogMessages
		{
			get
			{
				List<IFeatureVirtualLogMessages> list = new List<IFeatureVirtualLogMessages>();
				foreach (Feature current in this.features)
				{
					if (current is IFeatureVirtualLogMessages)
					{
						list.Add(current as IFeatureVirtualLogMessages);
					}
				}
				return list;
			}
		}

		IList<IFeatureEvents> IFeatureRegistration.FeaturesUsingEvents
		{
			get
			{
				List<IFeatureEvents> list = new List<IFeatureEvents>();
				foreach (Feature current in this.features)
				{
					if (current is IFeatureEvents)
					{
						list.Add(current as IFeatureEvents);
					}
				}
				return list;
			}
		}

		public ProjectRoot ProjectRoot
		{
			get
			{
				return this.projectRoot;
			}
		}

		public bool IsDirty
		{
			get;
			set;
		}

		public bool IsImported
		{
			get
			{
				return this.isImported;
			}
		}

		public string FilePath
		{
			get
			{
				return this.filePath;
			}
		}

		public VLProject()
		{
			VLProject._vlProject = this;
		}

		bool IFeatureRegistration.Register(Feature feature)
		{
			if (this.features == null)
			{
				this.features = new List<Feature>();
			}
			this.features.Add(feature);
			return true;
		}

		private void ClearFeatures()
		{
			if (this.features != null)
			{
				this.features.Clear();
			}
		}

		public string GetProjectFileName()
		{
			if (string.IsNullOrEmpty(this.filePath))
			{
				return "";
			}
			return Path.GetFileName(this.filePath);
		}

		public string GetProjectFolder()
		{
			if (string.IsNullOrEmpty(this.filePath))
			{
				return "";
			}
			string text = Path.GetDirectoryName(this.filePath);
			if (!string.IsNullOrEmpty(text))
			{
				return text;
			}
			text = Path.GetPathRoot(this.filePath);
			if (!string.IsNullOrEmpty(text))
			{
				return text;
			}
			return "";
		}

		public bool SetFilePathFromImportedVLCFile(string importedFilePath)
		{
			if (!string.IsNullOrEmpty(this.filePath))
			{
				return false;
			}
			this.filePath = importedFilePath;
			this.isImported = true;
			return true;
		}

		public void CreateEmptyProject(LoggerType loggerType)
		{
			this.ClearFeatures();
			this.projectRoot = new ProjectRoot();
			this.projectRoot.LoggerType = loggerType;
			this.filePath = "";
			this.isImported = false;
		}

		public void CloseProject()
		{
			this.ClearFeatures();
			this.projectRoot = null;
			this.filePath = "";
		}

		public bool LoadProjectFile(string aFileAndPath, out ILoggerSpecifics newLoggerSpecifics, out bool isIncompatibleFileVersion)
		{
			this.ClearFeatures();
			bool flag = ConfigurationReader.BuildRawDataFromProjectFile(aFileAndPath, ref this.projectRoot, out isIncompatibleFileVersion);
			if (flag)
			{
				LoggerType loggerType = this.projectRoot.LoggerType;
				newLoggerSpecifics = LoggerSpecificsFactory.CreateLoggerSpecifics(loggerType);
				this.filePath = aFileAndPath;
				this.IsDirty = false;
			}
			else
			{
				newLoggerSpecifics = null;
			}
			this.isImported = false;
			return flag;
		}

		public bool SaveProjectFile(string aFileAndPath)
		{
			bool flag = ConfigurationWriter.SaveRawDataToProjectFile(aFileAndPath, this.projectRoot);
			if (flag)
			{
				this.filePath = aFileAndPath;
				this.IsDirty = false;
			}
			this.isImported = false;
			return flag;
		}

		public bool HasCCPXCPSetup()
		{
			return this.ProjectRoot.LoggingConfiguration.DatabaseConfiguration.ActiveCCPXCPDatabases.Count > 0;
		}

		public bool HasStopCyclicCommunicationEventConfigured()
		{
			return this.ProjectRoot.HardwareConfiguration.LogDataStorage.StopCyclicCommunicationEvent != null;
		}
	}
}
