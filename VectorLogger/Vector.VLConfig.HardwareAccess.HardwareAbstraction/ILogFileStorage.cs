using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.HardwareAccess.HardwareAbstraction
{
	public interface ILogFileStorage
	{
		long TotalSpace
		{
			get;
		}

		long FreeSpace
		{
			get;
		}

		uint NumberOfTriggeredBuffers
		{
			get;
		}

		uint NumberOfRecordingBuffers
		{
			get;
		}

		uint[] HighestTriggerFileIndices
		{
			get;
		}

		ReadOnlyCollection<ILogFile> LogFiles
		{
			get;
		}

		bool IsOutdated
		{
			get;
		}

		bool HasMixedCompUncompFiles
		{
			get;
		}

		bool IsPrimaryFileGroupCompressed
		{
			get;
		}

		DateTime LatestDateTimeCompressedLogfiles
		{
			get;
		}

		DateTime LatestDateTimeUncompressedLogFiles
		{
			get;
		}

		uint NumberOfClassificFiles
		{
			get;
		}

		uint NumberOfJpegFiles
		{
			get;
		}

		uint NumberOfZipArchives
		{
			get;
		}

		uint NumberOfWavFiles
		{
			get;
		}

		uint NumberOfDriveRecorderFiles
		{
			get;
		}

		string DestSubFolderNamePrimary
		{
			get;
		}

		string DestSubFolderNameSecondary
		{
			get;
		}

		void DataSourceHasChanged();

		bool UpdateFileList();

		uint NumberOfLogFilesOnMemory(uint memoryNr);

		uint NumberOfCompLogFilesOnMemory(uint memoryNr);

		bool GenerateFolderNameFromLatestFileDate(out string folderName);

		IList<string> GetFileNamesOfPrimaryGroup();

		IList<string> GetFileNamesOfSecondaryGroup();

		IList<string> GetDriveRecorderFileNamesOfPrimaryGroup();

		IList<string> GetDriveRecorderFileNamesOfSecondaryGroup();

		bool ConvertAllLogFiles(FileConversionParameters conversionParameters, DatabaseConfiguration databaseConfiguration, string configurationFolderPath);

		bool ConvertSelectedLogFiles(FileConversionParameters conversionParameters, List<ConversionJob> conversionJobs, DatabaseConfiguration databaseConfiguration, string configurationFolderPath);

		bool CopyAndBatchExportSelectedLogFiles(FileConversionParameters conversionParameters, ConversionJob allInOneJob, string pathToExportBatchFile, ref string destination);

		bool DeleteAllLogFiles();

		void Reset();
	}
}
