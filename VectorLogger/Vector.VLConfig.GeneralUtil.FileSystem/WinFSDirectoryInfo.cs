using System;
using System.IO;

namespace Vector.VLConfig.GeneralUtil.FileSystem
{
	internal class WinFSDirectoryInfo : IDirectoryInfo
	{
		private readonly DirectoryInfo mDirectoryInfo;

		public FileAttributes Attributes
		{
			get
			{
				return this.mDirectoryInfo.Attributes;
			}
			set
			{
				this.mDirectoryInfo.Attributes = value;
			}
		}

		public WinFSDirectoryInfo(string path)
		{
			this.mDirectoryInfo = new DirectoryInfo(path);
		}
	}
}
