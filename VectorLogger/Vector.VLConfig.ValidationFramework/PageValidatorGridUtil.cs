using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using System;

namespace Vector.VLConfig.ValidationFramework
{
	internal static class PageValidatorGridUtil
	{
		public delegate void StoreRowMappingHandler(int dataSourceRowIdx);

		public static bool IsColumnVisible(GridColumn column, GridView view)
		{
			if (column.View != view)
			{
				return false;
			}
			GridViewInfo gridViewInfo = view.GetViewInfo() as GridViewInfo;
			if (gridViewInfo == null)
			{
				return false;
			}
			GridColumnsInfo columnsInfo = gridViewInfo.ColumnsInfo;
			int visibleIndex = columnsInfo.FirstColumnInfo.Column.VisibleIndex;
			int visibleIndex2 = columnsInfo.LastColumnInfo.Column.VisibleIndex;
			return column.VisibleIndex >= visibleIndex && column.VisibleIndex <= visibleIndex2;
		}

		public static void StoreRowMappingForVisibleRows(GridView view, PageValidatorGridUtil.StoreRowMappingHandler storeRowMappingMethod)
		{
			GridViewInfo gridViewInfo = view.GetViewInfo() as GridViewInfo;
			if (gridViewInfo == null)
			{
				return;
			}
			GridRowInfoCollection rowsInfo = gridViewInfo.RowsInfo;
			foreach (GridRowInfo current in rowsInfo)
			{
				int dataSourceRowIndex = view.GetDataSourceRowIndex(current.RowHandle);
				storeRowMappingMethod(dataSourceRowIndex);
			}
		}
	}
}
