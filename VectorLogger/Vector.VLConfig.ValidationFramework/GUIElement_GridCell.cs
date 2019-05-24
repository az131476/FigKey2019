using DevExpress.XtraGrid.Columns;
using System;

namespace Vector.VLConfig.ValidationFramework
{
	public class GUIElement_GridCell : IValidatedGUIElement
	{
		private int dataSourceRowIndex;

		private GridColumn gridColumn;

		public GridColumn GridColumn
		{
			get
			{
				return this.gridColumn;
			}
		}

		public int DataSourceRowIndex
		{
			get
			{
				return this.dataSourceRowIndex;
			}
		}

		public GUIElement_GridCell(GridColumn gridColumn, int dataSourceRowIndex)
		{
			this.gridColumn = gridColumn;
			this.dataSourceRowIndex = dataSourceRowIndex;
		}
	}
}
