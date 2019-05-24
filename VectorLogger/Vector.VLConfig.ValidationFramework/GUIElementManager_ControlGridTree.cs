using DevExpress.XtraGrid.Columns;
using DevExpress.XtraTreeList.Columns;
using System;
using System.Windows.Forms;

namespace Vector.VLConfig.ValidationFramework
{
	public class GUIElementManager_ControlGridTree
	{
		private readonly GUIElementManager_Internal_Control mGuiIElementManagerInternalControl = new GUIElementManager_Internal_Control();

		private readonly GUIElementManager_Internal_GridCell mGuiIElementManagerInternalGridCell = new GUIElementManager_Internal_GridCell();

		private readonly GUIElementManager_Internal_TreeCell mGuiIElementManagerInternalTreeCell = new GUIElementManager_Internal_TreeCell();

		public IValidatedGUIElement GetGUIElement(Control control)
		{
			return this.mGuiIElementManagerInternalControl.CreateOrGetGUIElementForControl(control);
		}

		public IValidatedGUIElement GetGUIElement(GridColumn gridColumn, int dataSourceRowIndex)
		{
			return this.mGuiIElementManagerInternalGridCell.CreateOrGetGUIElementForGridCell(gridColumn, dataSourceRowIndex);
		}

		public IValidatedGUIElement GetGUIElement(TreeListColumn treeColumn, int dataSourceRowIndex)
		{
			return this.mGuiIElementManagerInternalTreeCell.GetOrCreateGuiElementForTreeCell(treeColumn, dataSourceRowIndex);
		}

		public void Reset()
		{
			this.mGuiIElementManagerInternalControl.Reset();
			this.mGuiIElementManagerInternalGridCell.Reset();
			this.mGuiIElementManagerInternalTreeCell.Reset();
		}
	}
}
