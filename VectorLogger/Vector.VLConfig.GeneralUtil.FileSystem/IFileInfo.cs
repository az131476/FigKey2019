using System;
using System.IO;

namespace Vector.VLConfig.GeneralUtil.FileSystem
{
	public interface IFileInfo
	{
		string Name
		{
			get;
		}

		string FullName
		{
			get;
		}

		string DirectoryName
		{
			get;
		}

		string Extension
		{
			get;
		}

		bool Exists
		{
			get;
		}

		long Length
		{
			get;
		}

		DateTime LastWriteTime
		{
			get;
			set;
		}

		FileAttributes Attributes
		{
			get;
			set;
		}
	}
}
