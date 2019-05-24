using System;

namespace Vector.VLConfig.HardwareAccess.ToolInterfaces
{
	internal class GL3000conf : GenericConfTool
	{
		public GL3000conf() : base("GL3000.ini")
		{
			base.FileName = "GL3000conf.exe";
			this.hasWebServer = true;
		}
	}
}
