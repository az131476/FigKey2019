using System;

namespace Vector.VLConfig.GeneralUtil.FileSystem
{
	public class FileSystemHelpers
	{
		public static string MakeNicePath(string path)
		{
			string text = path.Trim().Trim(new char[]
			{
				'"'
			}).Trim();
			if (text.IndexOf(" ") >= 0)
			{
				return "\"" + text + "\"";
			}
			return text;
		}
	}
}
