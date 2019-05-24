using System;
using System.IO;

namespace Vector.VLConfig.GeneralUtil.FileSystem
{
	internal class WinFSFile : IFile, IDisposable
	{
		private string path;

		string IFile.Path
		{
			get
			{
				return this.path;
			}
		}

		public WinFSFile(string filePath)
		{
			this.path = filePath;
		}

		public void Dispose()
		{
		}

		bool IFile.Exists()
		{
			return File.Exists(this.path);
		}

		StreamReader IFile.OpenText()
		{
			return File.OpenText(this.path);
		}

		Stream IFile.OpenRead()
		{
			return File.OpenRead(this.path);
		}

		void IFile.Copy(string destFileName)
		{
			File.Copy(this.path, destFileName);
		}

		void IFile.Copy(string destFileName, bool overwrite)
		{
			File.Copy(this.path, destFileName, overwrite);
		}
	}
}
