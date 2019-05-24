using System;
using System.IO;

namespace Vector.VLConfig.GeneralUtil.FileSystem
{
	public class FileInfoProxy : IFileInfo
	{
		private IFileInfo concreteFileInfoProxy;

		public string Name
		{
			get
			{
				if (this.concreteFileInfoProxy == null)
				{
					return "";
				}
				return this.concreteFileInfoProxy.Name;
			}
		}

		public string FullName
		{
			get
			{
				if (this.concreteFileInfoProxy == null)
				{
					return "";
				}
				return this.concreteFileInfoProxy.FullName;
			}
		}

		public string DirectoryName
		{
			get
			{
				if (this.concreteFileInfoProxy == null)
				{
					return "";
				}
				return this.concreteFileInfoProxy.DirectoryName;
			}
		}

		public string Extension
		{
			get
			{
				if (this.concreteFileInfoProxy == null)
				{
					return "";
				}
				return this.concreteFileInfoProxy.Extension;
			}
		}

		public bool Exists
		{
			get
			{
				return this.concreteFileInfoProxy != null && this.concreteFileInfoProxy.Exists;
			}
		}

		public long Length
		{
			get
			{
				if (this.concreteFileInfoProxy == null)
				{
					return -1L;
				}
				return this.concreteFileInfoProxy.Length;
			}
		}

		public DateTime LastWriteTime
		{
			get
			{
				if (this.concreteFileInfoProxy == null)
				{
					return default(DateTime);
				}
				return this.concreteFileInfoProxy.LastWriteTime;
			}
			set
			{
				if (this.concreteFileInfoProxy != null)
				{
					this.concreteFileInfoProxy.LastWriteTime = value;
				}
			}
		}

		public FileAttributes Attributes
		{
			get
			{
				if (this.concreteFileInfoProxy == null)
				{
					return FileAttributes.Normal;
				}
				return this.concreteFileInfoProxy.Attributes;
			}
			set
			{
				if (this.concreteFileInfoProxy != null)
				{
					this.concreteFileInfoProxy.Attributes = value;
				}
			}
		}

		public FileInfoProxy(string path)
		{
			this.CreateConcreteFileInfo(path);
		}

		private void CreateConcreteFileInfo(string path)
		{
			string zipFile;
			string zipEntry;
			if (ZipArchiveFile.SplitZipPath(path, out zipFile, out zipEntry))
			{
				this.concreteFileInfoProxy = new ZipArchiveFileInfo(path, zipFile, zipEntry);
				return;
			}
			this.concreteFileInfoProxy = new WinFSFileInfo(path);
		}
	}
}
