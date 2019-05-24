using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Vector.VLConfig.GeneralUtil.FileSystem
{
	public class FileProxy : IFile, IDisposable
	{
		public delegate string PasswordHandler();

		private IFile concreteFile;

		private static readonly FileAttributes[] sAllAttributes = (FileAttributes[])Enum.GetValues(typeof(FileAttributes));

		public static event FileProxy.PasswordHandler PasswordHook;

		string IFile.Path
		{
			get
			{
				if (this.concreteFile == null)
				{
					return "";
				}
				return this.concreteFile.Path;
			}
		}

		private FileProxy(string path)
		{
			this.CreateConcreteFile(path);
		}

		private void CreateConcreteFile(string path)
		{
			string zipPassword = string.Empty;
			if (FileProxy.PasswordHook != null)
			{
				zipPassword = FileProxy.PasswordHook();
			}
			else
			{
				zipPassword = string.Empty;
			}
			string zipFile;
			string zipEntry;
			if (ZipArchiveFile.SplitZipPath(path, out zipFile, out zipEntry))
			{
				this.concreteFile = new ZipArchiveFile(path, zipFile, zipEntry, zipPassword);
				return;
			}
			this.concreteFile = new WinFSFile(path);
		}

		public void Dispose()
		{
			if (this.concreteFile != null)
			{
				this.concreteFile.Dispose();
				this.concreteFile = null;
			}
		}

		bool IFile.Exists()
		{
			return this.concreteFile != null && this.concreteFile.Exists();
		}

		StreamReader IFile.OpenText()
		{
			if (this.concreteFile != null)
			{
				return this.concreteFile.OpenText();
			}
			return null;
		}

		Stream IFile.OpenRead()
		{
			if (this.concreteFile != null)
			{
				return this.concreteFile.OpenRead();
			}
			return null;
		}

		void IFile.Copy(string destFileName)
		{
			if (this.concreteFile != null)
			{
				this.concreteFile.Copy(destFileName);
			}
		}

		void IFile.Copy(string destFileName, bool overwrite)
		{
			if (this.concreteFile != null)
			{
				this.concreteFile.Copy(destFileName, overwrite);
			}
		}

		public static bool Exists(string path)
		{
			if (string.IsNullOrWhiteSpace(path))
			{
				return false;
			}
			IFile file = new FileProxy(path);
			bool result = file.Exists();
			file.Dispose();
			return result;
		}

		public static StreamReader OpenText(string path)
		{
			IFile file = new FileProxy(path);
			StreamReader result = file.OpenText();
			file.Dispose();
			return result;
		}

		public static Stream OpenRead(string path)
		{
			IFile file = new FileProxy(path);
			Stream result = file.OpenRead();
			file.Dispose();
			return result;
		}

		public static void Copy(string sourceFileName, string destFileName)
		{
			IFile file = new FileProxy(sourceFileName);
			file.Copy(destFileName);
			file.Dispose();
		}

		public static void Copy(string sourceFileName, string destFileName, bool overwrite)
		{
			IFile file = new FileProxy(sourceFileName);
			file.Copy(destFileName, overwrite);
			file.Dispose();
		}

		public static string[] ExcludeByAttributes(string[] files, FileAttributes attributesToExclude)
		{
			List<FileAttributes> list = (from tmpAttrib in FileProxy.sAllAttributes
			where attributesToExclude.HasFlag(tmpAttrib)
			select tmpAttrib).ToList<FileAttributes>();
			List<string> list2 = new List<string>();
			for (int i = 0; i < files.Length; i++)
			{
				string text = files[i];
				FileInfoProxy fileInfoProxy = new FileInfoProxy(text);
				bool flag = false;
				foreach (FileAttributes current in list)
				{
					flag |= fileInfoProxy.Attributes.HasFlag(current);
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
