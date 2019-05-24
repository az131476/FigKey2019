using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Vector.VLConfig.LoggingNavigator.Data
{
	public class CustomIndexFileNameComparer : IComparer<string>
	{
		public int Compare(string x, string y)
		{
			int num = 1;
			int num2 = 0;
			MatchCollection matchCollection = IndexFileCollection.regex.Matches(x);
			if (matchCollection.Count > 0 && matchCollection[0].Groups.Count > 1)
			{
				num = IndexManager.ConvertToInt(matchCollection[0].Groups[1].ToString());
			}
			if (matchCollection.Count > 0 && matchCollection[0].Groups.Count > 2)
			{
				num2 = IndexManager.ConvertToInt(matchCollection[0].Groups[2].ToString());
			}
			int num3 = 1;
			int value = 0;
			matchCollection = IndexFileCollection.regex.Matches(y);
			if (matchCollection.Count > 0 && matchCollection[0].Groups.Count > 1)
			{
				num3 = IndexManager.ConvertToInt(matchCollection[0].Groups[1].ToString());
			}
			if (matchCollection.Count > 0 && matchCollection[0].Groups.Count > 2)
			{
				value = IndexManager.ConvertToInt(matchCollection[0].Groups[2].ToString());
			}
			if (num != num3)
			{
				return num.CompareTo(num3);
			}
			return num2.CompareTo(value);
		}
	}
}
