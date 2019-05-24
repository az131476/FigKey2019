using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Vector.VLConfig.LoggingNavigator.Export;

namespace Vector.VLConfig.LoggingNavigator.Data
{
	public abstract class FileAccessManager
	{
		public static FileAccessManager _instance = null;

		public static readonly Regex regex = new Regex(".*!D(\\d+)F(\\d*)X", RegexOptions.IgnoreCase | RegexOptions.Compiled);

		public abstract Dictionary<string, Stream> GetListOfIndexFiles();

		public abstract Stream GetIniFileAsStream(string iniFileName);

		public abstract Stream GetWavFileAsStream(string path);

		public abstract bool ExistsOnFilesystem(LogFile logFile);

		public abstract bool ExistsOnFilesystem(string path);

		public abstract void SetFilesToCopy(string path, IList<ExportJob> jobs);

		public abstract bool GetNextFileToCopy(ref string name, ref long size, ref long maxSize, ref long count, ref long countTotal);

		public abstract bool CopyNextFileToDestination(string targetPath);

		public abstract string[] GetTopLevelFiles();

		public abstract long GetFilesSize(string path);

		public abstract void Close();

		public static FileAccessManager CreateFileAccessManager(string sourcePath)
		{
			if (sourcePath.ToLower().EndsWith(".zip") && !Directory.Exists(sourcePath))
			{
				FileAccessManager._instance = new ZipFileAccessManager(sourcePath);
			}
			else
			{
				FileAccessManager._instance = new FileSystemFileAccessManager(sourcePath);
			}
			return FileAccessManager._instance;
		}

		public static FileAccessManager GetInstance()
		{
			if (FileAccessManager._instance == null)
			{
				throw new IOException("FileAccessManager has not been initialized.");
			}
			return FileAccessManager._instance;
		}

		public static void CloseCurrentInstance()
		{
			if (FileAccessManager._instance != null)
			{
				FileAccessManager._instance.Close();
			}
		}

		public static bool IsZipSupportEnabled()
		{
			return true;
		}
	}
}
