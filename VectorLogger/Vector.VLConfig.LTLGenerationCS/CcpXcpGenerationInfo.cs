using System;
using System.Collections.Generic;

namespace Vector.VLConfig.LTLGenerationCS
{
	public class CcpXcpGenerationInfo
	{
		public List<CcpXcpDatabaseInfo> DatabaseInfos
		{
			get;
			private set;
		}

		public CcpXcpGenerationInfo()
		{
			this.DatabaseInfos = new List<CcpXcpDatabaseInfo>();
		}
	}
}
