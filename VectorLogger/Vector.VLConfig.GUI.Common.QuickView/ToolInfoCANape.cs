using System;
using System.Drawing;
using System.IO;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.GeneralUtil;

namespace Vector.VLConfig.GUI.Common.QuickView
{
	public class ToolInfoCANape : ToolInfo
	{
		private static string sPathOfExecutable;

		public override QuickViewTool QuickViewTool
		{
			get
			{
				return QuickViewTool.CANape;
			}
		}

		public override string Name
		{
			get
			{
				return this.QuickViewTool.ToString();
			}
		}

		public override Image IconImage
		{
			get
			{
				return MainImageList.Instance.GetImage(MainImageList.IconIndex.CANape);
			}
		}

		public override string PathOfExecutable
		{
			get
			{
				if (ToolInfoCANape.sPathOfExecutable == null)
				{
					ToolInfoCANape.sPathOfExecutable = string.Empty;
					string path;
					if (RegistryServices.IsVectorCANapeInstalled(out path))
					{
						string path2 = Path.Combine(path, "Exec", "canape32.exe");
						if (File.Exists(path2))
						{
							ToolInfoCANape.sPathOfExecutable = path2;
						}
					}
				}
				return ToolInfoCANape.sPathOfExecutable;
			}
		}

		public override bool CheckPathOfExecutableBeforeStarting
		{
			get
			{
				return true;
			}
		}
	}
}
