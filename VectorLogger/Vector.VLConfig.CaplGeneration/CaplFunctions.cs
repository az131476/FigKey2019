using System;
using System.Collections.Generic;
using System.IO;

namespace Vector.VLConfig.CaplGeneration
{
	internal class CaplFunctions
	{
		private IDictionary<string, IList<string>> mDict = new Dictionary<string, IList<string>>();

		public void AddCodeToFunction(string functionName, string code)
		{
			IList<string> list;
			if (!this.mDict.TryGetValue(functionName, out list))
			{
				list = new List<string>();
				this.mDict[functionName] = list;
			}
			list.Add(code);
		}

		public void AddCompleteFunctionDefinition(string functionDefinition)
		{
			this.AddCodeToFunction(string.Empty, functionDefinition);
		}

		public void Write(TextWriter tw)
		{
			foreach (KeyValuePair<string, IList<string>> current in this.mDict)
			{
				if (string.Compare(current.Key, "variables", StringComparison.Ordinal) == 0)
				{
					CaplFunctions.WriteFunction(tw, current);
					break;
				}
			}
			foreach (KeyValuePair<string, IList<string>> current2 in this.mDict)
			{
				if (string.Compare(current2.Key, "variables", StringComparison.Ordinal) != 0)
				{
					CaplFunctions.WriteFunction(tw, current2);
				}
			}
		}

		private static void WriteFunction(TextWriter tw, KeyValuePair<string, IList<string>> function)
		{
			if (function.Key != string.Empty)
			{
				tw.WriteLine(function.Key);
				tw.WriteLine("{");
			}
			foreach (string current in function.Value)
			{
				tw.WriteLine(current);
			}
			if (function.Key != string.Empty)
			{
				tw.WriteLine("}");
			}
			tw.WriteLine("");
		}
	}
}
