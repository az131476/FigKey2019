using System;
using System.Drawing;
using Vector.VLConfig.Data.ApplicationData;

namespace Vector.VLConfig.GUI.Common.QuickView
{
	public abstract class ToolInfo
	{
		public abstract QuickViewTool QuickViewTool
		{
			get;
		}

		public abstract string Name
		{
			get;
		}

		public abstract Image IconImage
		{
			get;
		}

		public abstract string PathOfExecutable
		{
			get;
		}

		public abstract bool CheckPathOfExecutableBeforeStarting
		{
			get;
		}
	}
}
