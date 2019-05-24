using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.BusinessLogic.Configuration
{
	public class AnalysisFileCollector : IDisposable
	{
		public class AnalysisFile
		{
			public string Path
			{
				get;
				set;
			}

			public string Network
			{
				get;
				set;
			}

			public BusType BusType
			{
				get;
				set;
			}

			public uint Channel
			{
				get;
				set;
			}

			public ExportDatabase.DBType Type
			{
				get;
				set;
			}

			public Database FlexrayDB
			{
				get;
				set;
			}

			public AnalysisFile(string path, string network, BusType busType, uint channel, ExportDatabase.DBType type, Database flexRayDB)
			{
				this.Path = path;
				this.Network = network;
				this.BusType = busType;
				this.Channel = channel;
				this.Type = type;
				this.FlexrayDB = flexRayDB;
			}
		}

		private static AnalysisFileCollector sInstance;

		public string GeneralPurposeTempDirectory
		{
			get;
			private set;
		}

		public List<AnalysisFileCollector.AnalysisFile> GeneratedFiles
		{
			get;
			private set;
		}

		public bool SoundnessCheckPerformed
		{
			get;
			set;
		}

		public static AnalysisFileCollector Create()
		{
			if (AnalysisFileCollector.sInstance != null)
			{
				throw new InvalidOperationException(string.Format(Resources.InternalError, "Only one instance of AnalysisFileCollector allowed!"));
			}
			return AnalysisFileCollector.sInstance = new AnalysisFileCollector();
		}

		private AnalysisFileCollector()
		{
			this.GeneratedFiles = new List<AnalysisFileCollector.AnalysisFile>();
			string tempDirectoryName;
			if (TempDirectoryManager.Instance.CreateNewTempDirectory(out tempDirectoryName))
			{
				this.GeneralPurposeTempDirectory = TempDirectoryManager.Instance.GetFullTempDirectoryPath(tempDirectoryName);
			}
			else
			{
				InformMessageBox.Error(Resources.ErrorCannotCreateTemporaryFile);
				this.GeneralPurposeTempDirectory = string.Empty;
			}
			GenerationUtil.AnalysisFileCollector = this;
			GenerationUtilVN.AnalysisFileCollector = this;
		}

		public void Dispose()
		{
			this.GeneratedFiles = null;
			GenerationUtil.AnalysisFileCollector = null;
			GenerationUtilVN.AnalysisFileCollector = null;
			TempDirectoryManager.Instance.ReleaseTempDirectory(this.GeneralPurposeTempDirectory);
			AnalysisFileCollector.sInstance = null;
		}

		public void AddFile(string fullPath, string network, BusType busType, uint channel, ExportDatabase.DBType type, Database flexRayDB = null)
		{
			this.GeneratedFiles.Add(new AnalysisFileCollector.AnalysisFile(fullPath, network, busType, channel, type, flexRayDB));
		}

		public static string SubDirectoryForFilesForAnalysis()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(Path.DirectorySeparatorChar);
			stringBuilder.Append("Analysis");
			stringBuilder.Append(Path.DirectorySeparatorChar);
			DateTime now = DateTime.Now;
			stringBuilder.AppendFormat(Resources.FormatDateTimeForFilenames, new object[]
			{
				now.Year,
				now.Month,
				now.Day,
				now.Hour,
				now.Minute,
				now.Second
			});
			return stringBuilder.ToString();
		}
	}
}
