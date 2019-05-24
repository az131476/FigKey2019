using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.LoggerSpecifics;

namespace Vector.VLConfig.Data.ApplicationData
{
	public class FileConversionParameters
	{
		private ExportDatabaseConfiguration exportDatabaseConfiguration;

		public string DestinationFolder
		{
			get;
			set;
		}

		public FileConversionDestFormat DestinationFormat
		{
			get;
			set;
		}

		public string[] DestinationFormatVersionStrings
		{
			get;
			set;
		}

		public string[] DestinationFormatExtensions
		{
			get;
			set;
		}

		public bool DeleteSourceFilesWhenDone
		{
			get;
			set;
		}

		public bool OverwriteDestinationFiles
		{
			get;
			set;
		}

		public bool SaveRawFile
		{
			get;
			set;
		}

		public bool HexadecimalNotation
		{
			get;
			set;
		}

		public bool SingleFile
		{
			get;
			set;
		}

		public bool GlobalTimestamps
		{
			get;
			set;
		}

		public bool RelativeTimestamps
		{
			get;
			set;
		}

		public bool GermanMSExcelFormat
		{
			get;
			set;
		}

		public bool UseUnlimitedFileSize
		{
			get;
			set;
		}

		public bool SplitFilesBySize
		{
			get;
			set;
		}

		public int FileFractionSize
		{
			get;
			set;
		}

		public bool SplitFilesByTime
		{
			get;
			set;
		}

		public bool UseRealTimeRaster
		{
			get;
			set;
		}

		public FileConversionTimeBase TimeBase
		{
			get;
			set;
		}

		public int FileFractionTime
		{
			get;
			set;
		}

		public bool GenerateVsysvarFile
		{
			get;
			set;
		}

		public bool JumpOverSleepTime
		{
			get;
			set;
		}

		public bool RecoveryMode
		{
			get;
			set;
		}

		public bool SuppressBufferConcat
		{
			get;
			set;
		}

		public FileConversionFilenameFormat FilenameFormat
		{
			get;
			set;
		}

		public string Prefix
		{
			get;
			set;
		}

		public string CustomFilename
		{
			get;
			set;
		}

		public bool CopyMediaFiles
		{
			get;
			set;
		}

		public bool UseChannelMapping
		{
			get;
			set;
		}

		public uint[] CanChannelMapping
		{
			get;
			set;
		}

		public uint[] LinChannelMapping
		{
			get;
			set;
		}

		public uint[] FlexRayChannelMapping
		{
			get;
			set;
		}

		public bool HideChannelMappingIdentities
		{
			get;
			set;
		}

		public bool UseDateTimeFromMeasurementStart
		{
			get;
			set;
		}

		public bool UseMinimumDigitsForTriggerIndex
		{
			get;
			set;
		}

		public int MinimumDigitsForTriggerIndex
		{
			get;
			set;
		}

		public bool WriteRawValues
		{
			get;
			set;
		}

		public FileConversionMDF3SignalFormat MDF3SignalFormat
		{
			get;
			set;
		}

		public string MDF3SignalFormatDelimiter
		{
			get;
			set;
		}

		public List<ExportDatabase> PersistenceExportDatabaseList
		{
			get;
			set;
		}

		public ExportDatabaseConfiguration.DBSelectionMode PersistenceExportDatabaseConfigurationMode
		{
			get;
			set;
		}

		[XmlIgnore]
		public ExportDatabaseConfiguration ExportDatabaseConfiguration
		{
			get
			{
				if (this.exportDatabaseConfiguration == null)
				{
					this.exportDatabaseConfiguration = new ExportDatabaseConfiguration();
				}
				return this.exportDatabaseConfiguration;
			}
			set
			{
				this.exportDatabaseConfiguration = value;
			}
		}

		[XmlIgnore]
		public bool UseExportDatabaseConfiguration
		{
			get
			{
				return this.exportDatabaseConfiguration != null && this.exportDatabaseConfiguration.IsExportDatabaseConfigurationEnabled && this.exportDatabaseConfiguration.Databases.Count > 0;
			}
		}

		[XmlIgnore]
		public bool IsDeletionOfSourceFilesAllowed
		{
			get;
			set;
		}

		public FileConversionParameters(FileConversionParameters other)
		{
			this.DestinationFolder = other.DestinationFolder;
			this.DestinationFormat = other.DestinationFormat;
			this.DestinationFormatVersionStrings = new string[other.DestinationFormatVersionStrings.Length];
			Array.Copy(other.DestinationFormatVersionStrings, this.DestinationFormatVersionStrings, other.DestinationFormatVersionStrings.Length);
			this.DestinationFormatExtensions = new string[other.DestinationFormatExtensions.Length];
			Array.Copy(other.DestinationFormatExtensions, this.DestinationFormatExtensions, other.DestinationFormatExtensions.Length);
			this.DeleteSourceFilesWhenDone = other.DeleteSourceFilesWhenDone;
			this.OverwriteDestinationFiles = other.OverwriteDestinationFiles;
			this.SaveRawFile = other.SaveRawFile;
			this.HexadecimalNotation = other.HexadecimalNotation;
			this.SingleFile = other.SingleFile;
			this.GlobalTimestamps = other.GlobalTimestamps;
			this.RelativeTimestamps = other.RelativeTimestamps;
			this.GermanMSExcelFormat = other.GermanMSExcelFormat;
			this.UseUnlimitedFileSize = other.UseUnlimitedFileSize;
			this.SplitFilesBySize = other.SplitFilesBySize;
			this.SplitFilesByTime = other.SplitFilesByTime;
			this.TimeBase = other.TimeBase;
			this.FileFractionTime = other.FileFractionTime;
			this.UseRealTimeRaster = other.UseRealTimeRaster;
			this.FileFractionSize = other.FileFractionSize;
			this.GenerateVsysvarFile = other.GenerateVsysvarFile;
			this.JumpOverSleepTime = other.JumpOverSleepTime;
			this.RecoveryMode = other.RecoveryMode;
			this.SuppressBufferConcat = other.SuppressBufferConcat;
			this.FilenameFormat = other.FilenameFormat;
			this.Prefix = other.Prefix;
			this.CustomFilename = other.CustomFilename;
			this.CopyMediaFiles = other.CopyMediaFiles;
			this.UseChannelMapping = other.UseChannelMapping;
			this.CanChannelMapping = new uint[other.CanChannelMapping.Length];
			Array.Copy(other.CanChannelMapping, this.CanChannelMapping, other.CanChannelMapping.Length);
			this.LinChannelMapping = new uint[other.LinChannelMapping.Length];
			Array.Copy(other.LinChannelMapping, this.LinChannelMapping, other.LinChannelMapping.Length);
			this.FlexRayChannelMapping = new uint[other.FlexRayChannelMapping.Length];
			Array.Copy(other.FlexRayChannelMapping, this.FlexRayChannelMapping, other.FlexRayChannelMapping.Length);
			this.HideChannelMappingIdentities = other.HideChannelMappingIdentities;
			this.UseDateTimeFromMeasurementStart = other.UseDateTimeFromMeasurementStart;
			this.UseMinimumDigitsForTriggerIndex = other.UseMinimumDigitsForTriggerIndex;
			this.MinimumDigitsForTriggerIndex = other.MinimumDigitsForTriggerIndex;
			this.WriteRawValues = other.WriteRawValues;
			this.MDF3SignalFormat = other.MDF3SignalFormat;
			this.MDF3SignalFormatDelimiter = other.MDF3SignalFormatDelimiter;
			this.PersistenceExportDatabaseList = new List<ExportDatabase>(other.PersistenceExportDatabaseList);
			this.PersistenceExportDatabaseConfigurationMode = other.PersistenceExportDatabaseConfigurationMode;
			this.ExportDatabaseConfiguration = other.ExportDatabaseConfiguration;
			this.IsDeletionOfSourceFilesAllowed = other.IsDeletionOfSourceFilesAllowed;
		}

		public FileConversionParameters()
		{
			this.DestinationFormatVersionStrings = new string[Enum.GetValues(typeof(FileConversionDestFormat)).Length];
			this.DestinationFormatExtensions = new string[Enum.GetValues(typeof(FileConversionDestFormat)).Length];
			this.DestinationFolder = string.Empty;
			this.Prefix = string.Empty;
			this.CustomFilename = string.Empty;
			this.CopyMediaFiles = true;
			this.CanChannelMapping = new uint[0];
			this.LinChannelMapping = new uint[0];
			this.FlexRayChannelMapping = new uint[0];
			this.PersistenceExportDatabaseList = new List<ExportDatabase>();
			this.PersistenceExportDatabaseConfigurationMode = ExportDatabaseConfiguration.DBSelectionMode.FromConfig;
			this.ExportDatabaseConfiguration = null;
			this.IsDeletionOfSourceFilesAllowed = true;
			this.WriteRawValues = false;
			this.MDF3SignalFormat = FileConversionMDF3SignalFormat.DatabaseMessageAndName;
			this.MDF3SignalFormatDelimiter = Vocabulary.DefaultSignalDelimiter;
		}

		public void SyncExportDatabaseConfigurationToPersistence()
		{
			this.PersistenceExportDatabaseList.Clear();
			this.PersistenceExportDatabaseList.AddRange(this.ExportDatabaseConfiguration.AllDescriptionFiles);
			this.PersistenceExportDatabaseConfigurationMode = this.ExportDatabaseConfiguration.Mode;
		}

		public void ApplyExportDatabasePersistenceToConfiguration()
		{
			this.ExportDatabaseConfiguration.Mode = this.PersistenceExportDatabaseConfigurationMode;
			this.ExportDatabaseConfiguration.ClearDatabases();
			this.ExportDatabaseConfiguration.AddDatabaseList(this.PersistenceExportDatabaseList);
		}
	}
}
