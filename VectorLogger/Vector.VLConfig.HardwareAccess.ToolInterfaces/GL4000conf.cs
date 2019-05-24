using System;

namespace Vector.VLConfig.HardwareAccess.ToolInterfaces
{
	internal class GL4000conf : GenericConfTool
	{
		public GL4000conf() : base("GL4000.ini")
		{
			base.FileName = "GL4000conf.exe";
			this.hasWebServer = true;
		}
	}
}
