using System;
using System.IO;

namespace Vector.VLConfig.GeneralUtil.FileSystem
{
	internal class ZipArchiveDirectoryInfo : IDirectoryInfo
	{
		private FileAttributes mFileAttributes;

		public FileAttributes Attributes
		{
			get
			{
				return this.mFileAttributes;
			}
			set
			{
				this.mFileAttributes = value;
			}
		}

		public ZipArchiveDirectoryInfo(string path, string zipFile, string zipEntry)
		{
			using (new ZipArchiveDirectory(path, zipFile, zipEntry))
			{
				this.mFileAttributes = FileAttributes.Normal;
			}
		}
	}
}
