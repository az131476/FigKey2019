using System;
using System.Collections.Generic;
using System.IO;

namespace Vector.VLConfig.GeneralUtil.FileSystem
{
	public interface IDirectory : IDisposable
	{
		string Path
		{
			get;
		}

		bool Exists();

		string[] GetFiles();

		string[] GetFiles(string searchPattern);

		string[] GetFiles(string searchPattern, SearchOption searchOption);

		string[] GetDirectories();

		string[] GetDirectories(string searchPattern);

		string[] GetDirectories(string searchPattern, SearchOption searchOption);

		IEnumerable<string> EnumerateDirectories();

		IEnumerable<string> EnumerateDirectories(string searchPattern);

		IEnumerable<string> EnumerateDirectories(string searchPattern, SearchOption searchOption);

		void GetAccessControl();
	}
}
