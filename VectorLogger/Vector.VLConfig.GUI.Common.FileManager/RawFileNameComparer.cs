using System;
using System.Collections.Generic;

namespace Vector.VLConfig.GUI.Common.FileManager
{
	public class RawFileNameComparer : IComparer<string>
	{
		public int Compare(string x, string y)
		{
			if (x.Length < y.Length)
			{
				return -1;
			}
			if (x.Length > y.Length)
			{
				return 1;
			}
			return x.CompareTo(y);
		}
	}
}
