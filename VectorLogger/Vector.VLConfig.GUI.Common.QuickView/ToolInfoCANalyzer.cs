using System;
using System.Drawing;
using Vector.VLConfig.Data.ApplicationData;

namespace Vector.VLConfig.GUI.Common.QuickView
{
	public class ToolInfoCANalyzer : ToolInfo
	{
		public override QuickViewTool QuickViewTool
		{
			get
			{
				return QuickViewTool.CANalyzer;
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
				return MainImageList.Instance.GetImage(MainImageList.IconIndex.CANalyzer);
			}
		}

		public override string PathOfExecutable
		{
			get
			{
				return string.Empty;
			}
		}

		public override bool CheckPathOfExecutableBeforeStarting
		{
			get
			{
				return false;
			}
		}
	}
}
