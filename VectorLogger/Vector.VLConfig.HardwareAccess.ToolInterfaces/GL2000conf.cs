using System;

namespace Vector.VLConfig.HardwareAccess.ToolInterfaces
{
	internal class GL2000conf : GenericConfTool
	{
		public GL2000conf() : base("GL2000.ini")
		{
			base.FileName = "GL2000conf.exe";
			this.hasWebServer = false;
		}
	}
}
