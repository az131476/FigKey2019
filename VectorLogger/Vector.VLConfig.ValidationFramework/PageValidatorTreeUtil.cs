using DevExpress.XtraTreeList;
using DevExpress.XtraTreeList.Nodes;
using DevExpress.XtraTreeList.ViewInfo;
using System;

namespace Vector.VLConfig.ValidationFramework
{
	internal static class PageValidatorTreeUtil
	{
		public delegate void StoreNodeMappingHandler(TreeListNode treeNode);

		public static void StoreNodeMappingForVisibleNodes(TreeList treeList, PageValidatorTreeUtil.StoreNodeMappingHandler storeNodeMappingMethod)
		{
			foreach (RowInfo rowInfo in treeList.ViewInfo.RowsInfo.Rows)
			{
				storeNodeMappingMethod(rowInfo.Node);
			}
		}
	}
}
