using DevExpress.XtraTreeList;
using System;

namespace Vector.VLConfig.GUI.XtraTreeListUtils
{
	internal class TreeListBatchUpdate : IDisposable
	{
		private readonly TreeList mTreeList;

		private readonly TreeListStateService mTreeListStateService;

		public TreeListBatchUpdate(TreeList treeList)
		{
			if (treeList == null)
			{
				return;
			}
			this.mTreeList = treeList;
			this.mTreeListStateService = new TreeListStateService(this.mTreeList, false);
			this.mTreeList.BeginUpdate();
			this.mTreeListStateService.StoreExpandState();
		}

		public void Dispose()
		{
			if (this.mTreeList == null)
			{
				return;
			}
			this.mTreeListStateService.RestoreExpandState();
			this.mTreeList.EndUpdate();
		}
	}
}
