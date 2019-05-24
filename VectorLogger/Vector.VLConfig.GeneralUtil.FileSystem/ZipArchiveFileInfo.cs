using System;
using System.IO;

namespace Vector.VLConfig.GeneralUtil.FileSystem
{
	internal class ZipArchiveFileInfo : IFileInfo
	{
		private string name;

		private string fullName;

		private string directoryName;

		private string extension;

		private bool exists;

		private long length;

		private DateTime lastWriteTime;

		private FileAttributes fileAttributes;

		public string Name
		{
			get
			{
				return this.name;
			}
		}

		public string FullName
		{
			get
			{
				return this.fullName;
			}
		}

		public string DirectoryName
		{
			get
			{
				return this.directoryName;
			}
		}

		public string Extension
		{
			get
			{
				return this.extension;
			}
		}

		public bool Exists
		{
			get
			{
				return this.exists;
			}
		}

		public long Length
		{
			get
			{
				return this.length;
			}
		}

		public DateTime LastWriteTime
		{
			get
			{
				return this.lastWriteTime;
			}
			set
			{
				this.lastWriteTime = value;
			}
		}

		public FileAttributes Attributes
		{
			get
			{
				return this.fileAttributes;
			}
			set
			{
				this.fileAttributes = value;
			}
		}

		public ZipArchiveFileInfo(string path, string zipFile, string zipEntry)
		{
			using (ZipArchiveFile zipArchiveFile = new ZipArchiveFile(path, zipFile, zipEntry, ""))
			{
				this.name = zipArchiveFile.Name;
				this.fullName = zipArchiveFile.FullName;
				this.directoryName = zipArchiveFile.DirectoryName;
				this.extension = zipArchiveFile.Extension;
				this.exists = zipArchiveFile.Exists;
				this.length = zipArchiveFile.Length;
				this.lastWriteTime = zipArchiveFile.LastWriteTime;
				this.fileAttributes = FileAttributes.Normal;
			}
		}
	}
}
