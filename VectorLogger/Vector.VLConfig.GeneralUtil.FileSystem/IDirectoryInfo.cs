using System;
using System.IO;

namespace Vector.VLConfig.GeneralUtil.FileSystem
{
	internal interface IDirectoryInfo
	{
		FileAttributes Attributes
		{
			get;
			set;
		}
	}
}
