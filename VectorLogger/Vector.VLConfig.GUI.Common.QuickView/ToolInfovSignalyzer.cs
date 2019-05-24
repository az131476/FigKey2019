using System;
using System.Drawing;
using System.IO;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.GeneralUtil;

namespace Vector.VLConfig.GUI.Common.QuickView
{
	public class ToolInfovSignalyzer : ToolInfo
	{
		private static string sPathOfExecutable;

		public override QuickViewTool QuickViewTool
		{
			get
			{
				return QuickViewTool.vSignalyzer;
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
				return MainImageList.Instance.GetImage(MainImageList.IconIndex.vSignalyzer);
			}
		}

		public override string PathOfExecutable
		{
			get
			{
				if (ToolInfovSignalyzer.sPathOfExecutable == null)
				{
					ToolInfovSignalyzer.sPathOfExecutable = string.Empty;
					string path;
					if (RegistryServices.IsVectorvSignalyzerInstalled(out path))
					{
						string path2 = Path.Combine(path, "Exec", "vSignalyzer32.exe");
						if (File.Exists(path2))
						{
							ToolInfovSignalyzer.sPathOfExecutable = path2;
						}
					}
				}
				return ToolInfovSignalyzer.sPathOfExecutable;
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
