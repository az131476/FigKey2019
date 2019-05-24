using System;

namespace Vector.VLConfig.GUI.XtraTreeListUtils
{
	internal class TreeListStateEntry
	{
		private bool mExpanded;

		public bool Expanded
		{
			get
			{
				return this.mExpanded;
			}
			set
			{
				this.mExpanded = value;
				this.Updated = true;
			}
		}

		public bool Updated
		{
			get;
			private set;
		}

		public TreeListStateEntry(bool expanded)
		{
			this.Expanded = expanded;
		}

		public void PrepareForUpdate()
		{
			this.Updated = false;
		}
	}
}
