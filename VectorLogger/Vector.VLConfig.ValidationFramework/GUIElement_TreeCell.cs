using DevExpress.XtraTreeList.Columns;
using System;

namespace Vector.VLConfig.ValidationFramework
{
	internal class GUIElement_TreeCell : IValidatedGUIElement
	{
		public readonly int DataSourceRowIndex;

		public readonly TreeListColumn Column;

		public GUIElement_TreeCell(TreeListColumn column, int dataSourceRowIndex)
		{
			this.Column = column;
			this.DataSourceRowIndex = dataSourceRowIndex;
		}
	}
}
