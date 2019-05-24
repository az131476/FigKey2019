using System;
using System.IO;

namespace Vector.VLConfig.GeneralUtil.FileSystem
{
	public class DirectoryInfoProxy : IDirectoryInfo
	{
		private readonly IDirectoryInfo mActualDirectoryInfo;

		public FileAttributes Attributes
		{
			get
			{
				if (this.mActualDirectoryInfo == null)
				{
					return FileAttributes.Normal;
				}
				return this.mActualDirectoryInfo.Attributes;
			}
			set
			{
				if (this.mActualDirectoryInfo != null)
				{
					this.mActualDirectoryInfo.Attributes = value;
				}
			}
		}

		public DirectoryInfoProxy(string path)
		{
			string zipFile;
			string zipEntry;
			if (ZipArchiveFile.SplitZipPath(path, out zipFile, out zipEntry))
			{
				this.mActualDirectoryInfo = new ZipArchiveDirectoryInfo(path, zipFile, zipEntry);
				return;
			}
			this.mActualDirectoryInfo = new WinFSDirectoryInfo(path);
		}
	}
}
