using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Vector.VLConfig.GeneralUtil.FileSystem
{
	public class DirectoryProxy : IDirectory, IDisposable
	{
		private IDirectory concreteDirectory;

		private static readonly FileAttributes[] sAllAttributes = (FileAttributes[])Enum.GetValues(typeof(FileAttributes));

		string IDirectory.Path
		{
			get
			{
				if (this.concreteDirectory == null)
				{
					return "";
				}
				return this.concreteDirectory.Path;
			}
		}

		private DirectoryProxy(string path)
		{
			this.CreateConcreteDirectory(path);
		}

		private void CreateConcreteDirectory(string path)
		{
			string zipFile;
			string zipEntry;
			if (ZipArchiveDirectory.SplitZipPath(path, out zipFile, out zipEntry))
			{
				this.concreteDirectory = new ZipArchiveDirectory(path, zipFile, zipEntry);
				return;
			}
			if (Directory.Exists(path))
			{
				this.concreteDirectory = new WinFSDirectory(path);
			}
		}

		public void Dispose()
		{
			if (this.concreteDirectory != null)
			{
				this.concreteDirectory.Dispose();
				this.concreteDirectory = null;
			}
		}

		bool IDirectory.Exists()
		{
			return this.concreteDirectory != null && this.concreteDirectory.Exists();
		}

		string[] IDirectory.GetFiles()
		{
			if (this.concreteDirectory != null)
			{
				return this.concreteDirectory.GetFiles();
			}
			return new string[0];
		}

		string[] IDirectory.GetFiles(string searchPattern)
		{
			if (this.concreteDirectory != null)
			{
				return this.concreteDirectory.GetFiles(searchPattern);
			}
			return new string[0];
		}

		string[] IDirectory.GetFiles(string searchPattern, SearchOption searchOption)
		{
			if (this.concreteDirectory != null)
			{
				return this.concreteDirectory.GetFiles(searchPattern, searchOption);
			}
			return new string[0];
		}

		string[] IDirectory.GetDirectories()
		{
			if (this.concreteDirectory != null)
			{
				return this.concreteDirectory.GetDirectories();
			}
			return new string[0];
		}

		string[] IDirectory.GetDirectories(string searchPattern)
		{
			if (this.concreteDirectory != null)
			{
				return this.concreteDirectory.GetDirectories(searchPattern);
			}
			return new string[0];
		}

		string[] IDirectory.GetDirectories(string searchPattern, SearchOption searchOption)
		{
			if (this.concreteDirectory != null)
			{
				return this.concreteDirectory.GetDirectories(searchPattern, searchOption);
			}
			return new string[0];
		}

		IEnumerable<string> IDirectory.EnumerateDirectories()
		{
			if (this.concreteDirectory != null)
			{
				return this.concreteDirectory.EnumerateDirectories();
			}
			return new List<string>();
		}

		IEnumerable<string> IDirectory.EnumerateDirectories(string searchPattern)
		{
			if (this.concreteDirectory != null)
			{
				return this.concreteDirectory.EnumerateDirectories(searchPattern);
			}
			return new List<string>();
		}

		IEnumerable<string> IDirectory.EnumerateDirectories(string searchPattern, SearchOption searchOption)
		{
			if (this.concreteDirectory != null)
			{
				return this.concreteDirectory.EnumerateDirectories(searchPattern, searchOption);
			}
			return new List<string>();
		}

		void IDirectory.GetAccessControl()
		{
			if (this.concreteDirectory != null)
			{
				this.concreteDirectory.GetAccessControl();
			}
		}

		public static bool Exists(string path)
		{
			if (string.IsNullOrWhiteSpace(path))
			{
				return false;
			}
			IDirectory directory = new DirectoryProxy(path);
			bool result = directory.Exists();
			directory.Dispose();
			return result;
		}

		public static string[] GetFiles(string path)
		{
			IDirectory directory = new DirectoryProxy(path);
			string[] files = directory.GetFiles();
			directory.Dispose();
			return files;
		}

		public static string[] GetFiles(string path, string searchPattern)
		{
			IDirectory directory = new DirectoryProxy(path);
			string[] files = directory.GetFiles(searchPattern);
			directory.Dispose();
			return files;
		}

		public static string[] GetFiles(string path, string searchPattern, SearchOption searchOption)
		{
			IDirectory directory = new DirectoryProxy(path);
			string[] files = directory.GetFiles(searchPattern, searchOption);
			directory.Dispose();
			return files;
		}

		public static string[] GetDirectories(string path)
		{
			IDirectory directory = new DirectoryProxy(path);
			string[] directories = directory.GetDirectories();
			directory.Dispose();
			return directories;
		}

		public static string[] GetDirectories(string path, string searchPattern)
		{
			IDirectory directory = new DirectoryProxy(path);
			string[] directories = directory.GetDirectories(searchPattern);
			directory.Dispose();
			return directories;
		}

		public static string[] GetDirectories(string path, string searchPattern, SearchOption searchOption)
		{
			IDirectory directory = new DirectoryProxy(path);
			string[] directories = directory.GetDirectories(searchPattern, searchOption);
			directory.Dispose();
			return directories;
		}

		public static IEnumerable<string> EnumerateDirectories(string path)
		{
			IDirectory directory = new DirectoryProxy(path);
			IEnumerable<string> result = directory.EnumerateDirectories();
			directory.Dispose();
			return result;
		}

		public static IEnumerable<string> EnumerateDirectories(string path, string searchPattern)
		{
			IDirectory directory = new DirectoryProxy(path);
			IEnumerable<string> result = directory.EnumerateDirectories(searchPattern);
			directory.Dispose();
			return result;
		}

		public static IEnumerable<string> EnumerateDirectories(string path, string searchPattern, SearchOption searchOption)
		{
			IDirectory directory = new DirectoryProxy(path);
			IEnumerable<string> result = directory.EnumerateDirectories(searchPattern, searchOption);
			directory.Dispose();
			return result;
		}

		public static void GetAccessControl(string path)
		{
			IDirectory directory = new DirectoryProxy(path);
			try
			{
				directory.GetAccessControl();
			}
			finally
			{
				directory.Dispose();
			}
		}

		public static string[] ExcludeByAttributes(string[] directories, FileAttributes attributesToExclude)
		{
			List<FileAttributes> list = (from tmpAttrib in DirectoryProxy.sAllAttributes
			where attributesToExclude.HasFlag(tmpAttrib)
			select tmpAttrib).ToList<FileAttributes>();
			List<string> list2 = new List<string>();
			for (int i = 0; i < directories.Length; i++)
			{
				string text = directories[i];
				DirectoryInfoProxy directoryInfoProxy = new DirectoryInfoProxy(text);
				bool flag = false;
				foreach (FileAttributes current in list)
				{
					flag |= directoryInfoProxy.Attributes.HasFlag(current);
				}
				if (!flag)
				{
					list2.Add(text);
				}
			}
			return list2.ToArray();
		}
	}
}
