using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.LoggingNavigator.Export;
using Vector.VLConfig.LoggingNavigator.Properties;

namespace Vector.VLConfig.LoggingNavigator.Data
{
	public class FileSystemFileAccessManager : FileAccessManager
	{
		private string mBasePath;

		private IList<FileInfo> mFiles;

		private long mMaxBytes;

		private int mFileCopyCurrentPosition;

		public FileSystemFileAccessManager(string basePath)
		{
			this.mBasePath = basePath;
		}

		private Stream GetFileAsStream(string path)
		{
			return File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
		}

		public override Dictionary<string, Stream> GetListOfIndexFiles()
		{
			Dictionary<string, Stream> dictionary = new Dictionary<string, Stream>();
			DirectoryInfo directoryInfo = new DirectoryInfo(this.mBasePath);
			FileInfo[] array = null;
			try
			{
				array = directoryInfo.GetFiles("*.glx", SearchOption.AllDirectories);
			}
			catch (IOException)
			{
				MessageBoxButtons buttons = MessageBoxButtons.OK;
				MessageBoxIcon icon = MessageBoxIcon.Hand;
				MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button1;
				string error = Resources.Error;
				MessageBox.Show(Resources.ErrorFilesCorrupt, error, buttons, icon, defaultButton);
				Dictionary<string, Stream> result = dictionary;
				return result;
			}
			if (array == null)
			{
				return dictionary;
			}
			FileInfo[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				FileInfo fileInfo = array2[i];
				string a = fileInfo.Extension.ToLower();
				bool flag = this.NormalizePath(fileInfo.Directory.Parent.FullName).Equals(this.NormalizePath(directoryInfo.FullName));
				flag &= FileAccessManager.regex.IsMatch(fileInfo.Directory.Name);
				bool flag2 = fileInfo.Directory.FullName.Equals(directoryInfo.FullName);
				if (fileInfo.Exists && a == Vocabulary.FileExtensionDotGLX && (flag || flag2))
				{
					dictionary.Add(fileInfo.FullName, this.GetFileAsStream(fileInfo.FullName));
				}
			}
			return dictionary;
		}

		private string NormalizePath(string path)
		{
			return Path.GetFullPath(new Uri(path).LocalPath).TrimEnd(new char[]
			{
				Path.DirectorySeparatorChar,
				Path.AltDirectorySeparatorChar
			}).ToLowerInvariant();
		}

		public override Stream GetIniFileAsStream(string iniFileName)
		{
			string text = Path.Combine(this.mBasePath, iniFileName);
			if (string.IsNullOrEmpty(text) || !File.Exists(text))
			{
				return null;
			}
			return this.GetFileAsStream(text);
		}

		public override Stream GetWavFileAsStream(string path)
		{
			string text = Path.Combine(this.mBasePath, path);
			if (string.IsNullOrEmpty(text) || !File.Exists(text))
			{
				return null;
			}
			return this.GetFileAsStream(text);
		}

		public override bool GetNextFileToCopy(ref string name, ref long size, ref long maxSize, ref long count, ref long countTotal)
		{
			if (this.mFileCopyCurrentPosition >= this.mFiles.Count)
			{
				return false;
			}
			FileInfo fileInfo = this.mFiles[this.mFileCopyCurrentPosition];
			name = fileInfo.Name;
			size = fileInfo.Length;
			maxSize = this.mMaxBytes;
			count = (long)this.mFileCopyCurrentPosition;
			countTotal = (long)this.mFiles.Count;
			return true;
		}

		public override bool CopyNextFileToDestination(string targetPath)
		{
			if (this.mFileCopyCurrentPosition >= this.mFiles.Count)
			{
				return false;
			}
			FileInfo fileInfo = this.mFiles[this.mFileCopyCurrentPosition];
			File.Copy(fileInfo.FullName, targetPath + fileInfo.Name, true);
			this.mFileCopyCurrentPosition++;
			return true;
		}

		public override bool ExistsOnFilesystem(LogFile logFile)
		{
			FileInfo fileInfo = this.GetFileInfo(logFile);
			return fileInfo != null;
		}

		public override bool ExistsOnFilesystem(string path)
		{
			return File.Exists(path);
		}

		public override void SetFilesToCopy(string path, IList<ExportJob> jobs)
		{
			this.mMaxBytes = 0L;
			this.mFiles = new List<FileInfo>();
			this.mFileCopyCurrentPosition = 0;
			foreach (ExportJob current in jobs)
			{
				foreach (LogFile current2 in current.LogFileList)
				{
					FileInfo fileInfo = this.GetFileInfo(current2);
					if (fileInfo != null)
					{
						this.mFiles.Add(fileInfo);
						this.mMaxBytes += fileInfo.Length;
					}
				}
			}
			path += "\\";
			FileInfo fileInfo2 = new FileInfo(path + "ml_rt.ini");
			if (!fileInfo2.Exists && this.mFiles.Count > 0 && this.mFiles[0] != null)
			{
				path = this.mFiles[0].Directory.Parent.FullName + "\\";
			}
			fileInfo2 = new FileInfo(path + "ml_rt.ini");
			if (fileInfo2.Exists)
			{
				this.mFiles.Add(fileInfo2);
				this.mMaxBytes += fileInfo2.Length;
			}
			fileInfo2 = new FileInfo(path + "ml_rt.ltl");
			if (fileInfo2.Exists)
			{
				this.mFiles.Add(fileInfo2);
				this.mMaxBytes += fileInfo2.Length;
			}
			fileInfo2 = new FileInfo(path + "ml_rt2.ini");
			if (fileInfo2.Exists)
			{
				this.mFiles.Add(fileInfo2);
				this.mMaxBytes += fileInfo2.Length;
			}
		}

		private FileInfo GetFileInfo(LogFile logFile)
		{
			FileInfo fileInfo = new FileInfo(logFile.Path + logFile.Name);
			FileInfo fileInfo2 = new FileInfo(logFile.Path + logFile.Name + Vocabulary.FileExtensionDotGZ);
			FileInfo fileInfo3 = new FileInfo(logFile.Path + logFile.Name_old);
			if (fileInfo.Exists)
			{
				return fileInfo;
			}
			if (fileInfo2.Exists)
			{
				return fileInfo2;
			}
			if (fileInfo3.Exists)
			{
				return fileInfo3;
			}
			return null;
		}

		public override string[] GetTopLevelFiles()
		{
			string[] result = null;
			try
			{
				result = Directory.GetFiles(this.mBasePath);
			}
			catch (UnauthorizedAccessException)
			{
			}
			return result;
		}

		public override long GetFilesSize(string path)
		{
			string text = Path.Combine(this.mBasePath, path);
			if (string.IsNullOrEmpty(text) || !File.Exists(text))
			{
				return 0L;
			}
			FileInfo fileInfo = new FileInfo(text);
			return fileInfo.Length;
		}

		public override void Close()
		{
		}
	}
}
