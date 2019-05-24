using System;
using System.IO;

namespace Vector.VLConfig.GeneralUtil.FileSystem
{
	internal class WinFSFileInfo : IFileInfo
	{
		private FileInfo FileInfo
		{
			get;
			set;
		}

		public string Name
		{
			get
			{
				return this.FileInfo.Name;
			}
		}

		public string FullName
		{
			get
			{
				return this.FileInfo.FullName;
			}
		}

		public string DirectoryName
		{
			get
			{
				return this.FileInfo.DirectoryName;
			}
		}

		public string Extension
		{
			get
			{
				return this.FileInfo.Extension;
			}
		}

		public bool Exists
		{
			get
			{
				return this.FileInfo.Exists;
			}
		}

		public long Length
		{
			get
			{
				return this.FileInfo.Length;
			}
		}

		public DateTime LastWriteTime
		{
			get
			{
				return this.FileInfo.LastWriteTime;
			}
			set
			{
				this.FileInfo.LastWriteTime = value;
			}
		}

		public FileAttributes Attributes
		{
			get
			{
				return this.FileInfo.Attributes;
			}
			set
			{
				this.FileInfo.Attributes = value;
			}
		}

		public WinFSFileInfo(string path)
		{
			this.FileInfo = new FileInfo(path);
		}
	}
}
