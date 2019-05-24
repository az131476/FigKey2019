using DevExpress.XtraGrid.Views.Grid;
using System;

namespace Vector.VLConfig.GUI.XtraGridUtils
{
	internal class GridBatchUpdate : IDisposable
	{
		private readonly GridView mGridView;

		public GridBatchUpdate(GridView gridView)
		{
			if (gridView == null)
			{
				return;
			}
			this.mGridView = gridView;
			this.mGridView.BeginUpdate();
		}

		public void Dispose()
		{
			if (this.mGridView == null)
			{
				return;
			}
			this.mGridView.EndUpdate();
		}
	}
}
