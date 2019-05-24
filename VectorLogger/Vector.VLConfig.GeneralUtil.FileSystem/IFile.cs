using System;
using System.IO;

namespace Vector.VLConfig.GeneralUtil.FileSystem
{
	internal interface IFile : IDisposable
	{
		string Path
		{
			get;
		}

		bool Exists();

		StreamReader OpenText();

		Stream OpenRead();

		void Copy(string destFileName);

		void Copy(string destFileName, bool overwrite);
	}
}
