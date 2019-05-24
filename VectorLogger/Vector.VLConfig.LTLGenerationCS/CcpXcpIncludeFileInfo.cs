using System;
using System.Collections.Generic;

namespace Vector.VLConfig.LTLGenerationCS
{
	public class CcpXcpIncludeFileInfo
	{
		public string IncFilePath
		{
			get;
			private set;
		}

		public List<string> OkayFlagNameList
		{
			get;
			private set;
		}

		public List<string> TimeoutVariableNameList
		{
			get;
			private set;
		}

		public string Prefix
		{
			get;
			private set;
		}

		public CcpXcpIncludeFileInfo(string aIncFilePath, List<string> aOkayFlagNameList, List<string> aTimeoutVariableNameList, string aPrefix)
		{
			this.IncFilePath = aIncFilePath;
			this.OkayFlagNameList = aOkayFlagNameList;
			this.TimeoutVariableNameList = aTimeoutVariableNameList;
			this.Prefix = aPrefix;
		}
	}
}
