using System;

namespace Vector.VLConfig.HardwareAccess.ToolInterfaces
{
	internal class GL1000conf : GenericConfTool
	{
		public GL1000conf() : base("GL1000.ini")
		{
			base.FileName = "GL1000conf.exe";
			this.hasWebServer = false;
		}
	}
}
