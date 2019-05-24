using System;
using System.Collections.Generic;
using System.IO;

namespace Vector.VLConfig.GeneralUtil.FileSystem
{
	internal class WinFSDirectory : IDirectory, IDisposable
	{
		private string path;

		string IDirectory.Path
		{
			get
			{
				return this.path;
			}
		}

		public WinFSDirectory(string dirPath)
		{
			this.path = dirPath;
		}

		public void Dispose()
		{
		}

		bool IDirectory.Exists()
		{
			return Directory.Exists(this.path);
		}

		string[] IDirectory.GetFiles()
		{
			return Directory.GetFiles(this.path);
		}

		string[] IDirectory.GetFiles(string searchPattern)
		{
			return Directory.GetFiles(this.path, searchPattern);
		}

		string[] IDirectory.GetFiles(string searchPattern, SearchOption searchOption)
		{
			return Directory.GetFiles(this.path, searchPattern, searchOption);
		}

		string[] IDirectory.GetDirectories()
		{
			return Directory.GetDirectories(this.path);
		}

		string[] IDirectory.GetDirectories(string searchPattern)
		{
			return Directory.GetDirectories(this.path, searchPattern);
		}

		string[] IDirectory.GetDirectories(string searchPattern, SearchOption searchOption)
		{
			return Directory.GetDirectories(this.path, searchPattern, searchOption);
		}

		IEnumerable<string> IDirectory.EnumerateDirectories()
		{
			return Directory.EnumerateDirectories(this.path);
		}

		IEnumerable<string> IDirectory.EnumerateDirectories(string searchPattern)
		{
			return Directory.EnumerateDirectories(this.path, searchPattern);
		}

		IEnumerable<string> IDirectory.EnumerateDirectories(string searchPattern, SearchOption searchOption)
		{
			return Directory.EnumerateDirectories(this.path, searchPattern, searchOption);
		}

		void IDirectory.GetAccessControl()
		{
			Directory.GetAccessControl(this.path);
		}
	}
}
