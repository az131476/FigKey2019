using System;
using System.Collections.Generic;
using Vector.VLConfig.HardwareAccess.ToolInterfaces;
using Vector.VLConfig.LoggerSpecifics;

namespace Vector.VLConfig.HardwareAccess.HardwareAbstraction
{
	internal class LTLCompiler
	{
		private LoggerType loggerType;

		private IDictionary<LoggerType, GenericConfTool> confTools;

		public LTLCompiler(LoggerType loggerType)
		{
			this.loggerType = loggerType;
		}

		public bool CompileLTL(string ltlFile, bool includeLTLSourceCode, bool generateDBCFilesForChannels, bool includeWebServerDisplayZip, out string errorText, string compileErrorFile)
		{
			GenericConfTool compiler = this.GetCompiler(this.loggerType);
			return compiler.Compile(ltlFile, includeLTLSourceCode, generateDBCFilesForChannels, includeWebServerDisplayZip, out errorText, compileErrorFile);
		}

		private GenericConfTool GetCompiler(LoggerType loggerType)
		{
			if (this.confTools == null)
			{
				this.confTools = new Dictionary<LoggerType, GenericConfTool>();
				this.confTools[LoggerType.GL1000] = new GL1000conf();
				this.confTools[LoggerType.GL1020FTE] = new GL1000conf();
				this.confTools[LoggerType.GL2000] = new GL2000conf();
				this.confTools[LoggerType.GL3000] = new GL3000conf();
				this.confTools[LoggerType.GL4000] = new GL4000conf();
			}
			return this.confTools[loggerType];
		}
	}
}
