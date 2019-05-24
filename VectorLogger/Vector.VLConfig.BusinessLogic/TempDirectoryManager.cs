using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.BusinessLogic
{
	public class TempDirectoryManager : IDisposable
	{
		private static readonly string vlConfigTempDirectoryPrefix = "VLConfigTemp";

		private string sessionTempDirectory;

		private List<string> tempDirectoryList;

		private static TempDirectoryManager _instance;

		private string SessionTempDirectory
		{
			get
			{
				if (this.sessionTempDirectory == null || this.sessionTempDirectory.Length <= 0)
				{
					this.sessionTempDirectory = Path.Combine(Path.GetTempPath(), TempDirectoryManager.vlConfigTempDirectoryPrefix + Guid.NewGuid().ToString());
				}
				return this.sessionTempDirectory;
			}
		}

		public List<string> TempDirectoryList
		{
			get
			{
				if (this.tempDirectoryList == null)
				{
					this.tempDirectoryList = new List<string>();
				}
				return this.tempDirectoryList;
			}
			set
			{
				this.tempDirectoryList = value;
			}
		}

		public static TempDirectoryManager Instance
		{
			get
			{
				if (TempDirectoryManager._instance == null)
				{
					TempDirectoryManager._instance = new TempDirectoryManager();
				}
				return TempDirectoryManager._instance;
			}
		}

		private TempDirectoryManager()
		{
			if (!Directory.Exists(this.SessionTempDirectory))
			{
				try
				{
					Directory.CreateDirectory(this.SessionTempDirectory);
				}
				catch
				{
					InformMessageBox.Error(Resources.ErrorCannotCreateTemporaryDirectory);
				}
			}
		}

		public bool CreateNewTempDirectory(out string tempSubDirectoryName)
		{
			tempSubDirectoryName = "";
			if (!Directory.Exists(this.SessionTempDirectory))
			{
				return false;
			}
			string text = "VLConfig_" + Guid.NewGuid().ToString();
			string fullTempDirectoryPath = this.GetFullTempDirectoryPath(text);
			if (Directory.Exists(fullTempDirectoryPath))
			{
				return false;
			}
			try
			{
				Directory.CreateDirectory(fullTempDirectoryPath);
			}
			catch
			{
				return false;
			}
			this.TempDirectoryList.Add(text);
			tempSubDirectoryName = text;
			return true;
		}

		public string GetFullTempDirectoryPath(string tempDirectoryName)
		{
			return Path.Combine(this.SessionTempDirectory, tempDirectoryName);
		}

		public bool GetNewTempFilename(string tempDirectoryName, string filename, out string newTempFilename)
		{
			newTempFilename = "";
			if (!Directory.Exists(this.GetFullTempDirectoryPath(tempDirectoryName)))
			{
				return false;
			}
			newTempFilename = Path.Combine(this.GetFullTempDirectoryPath(tempDirectoryName), filename);
			return this.TryDeleteTempFileIfExists(newTempFilename);
		}

		public bool ReleaseTempDirectory(string tempDirectoryName)
		{
			if (this.tempDirectoryList.IndexOf(tempDirectoryName) < 0)
			{
				return false;
			}
			if (!Directory.Exists(this.GetFullTempDirectoryPath(tempDirectoryName)))
			{
				return false;
			}
			bool result;
			try
			{
				string[] files = Directory.GetFiles(this.GetFullTempDirectoryPath(tempDirectoryName));
				for (int i = 0; i < files.Length; i++)
				{
					string path = files[i];
					if ((File.GetAttributes(path) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
					{
						File.SetAttributes(path, File.GetAttributes(path) & ~FileAttributes.ReadOnly);
					}
				}
				Directory.Delete(this.GetFullTempDirectoryPath(tempDirectoryName), true);
				result = true;
			}
			catch
			{
				result = false;
			}
			finally
			{
				this.TempDirectoryList.Remove(tempDirectoryName);
			}
			return result;
		}

		private bool TryDeleteTempFileIfExists(string tempFile)
		{
			if (!File.Exists(tempFile))
			{
				return true;
			}
			try
			{
				File.Delete(tempFile);
			}
			catch
			{
				return false;
			}
			return true;
		}

		public void Dispose()
		{
			if (!Directory.Exists(this.SessionTempDirectory))
			{
				return;
			}
			for (int i = this.TempDirectoryList.Count<string>() - 1; i >= 0; i--)
			{
				string text = this.TempDirectoryList[i];
				if (text != null && text.Length > 0)
				{
					this.ReleaseTempDirectory(text);
				}
			}
			TempDirectoryManager._instance = null;
			try
			{
				Directory.Delete(this.SessionTempDirectory);
			}
			catch
			{
			}
		}
	}
}
