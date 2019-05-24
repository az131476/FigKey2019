using DevExpress.XtraTreeList.Columns;
using System;
using System.Collections.Generic;

namespace Vector.VLConfig.ValidationFramework
{
	internal class GUIElementManager_Internal_TreeCell
	{
		private readonly Dictionary<int, IValidatedGUIElement> mDictTreeCell2GuiElement = new Dictionary<int, IValidatedGUIElement>();

		public IValidatedGUIElement GetOrCreateGuiElementForTreeCell(TreeListColumn treeListColumn, int dataSourceRowIndex)
		{
			int key = (dataSourceRowIndex << 8) + treeListColumn.AbsoluteIndex;
			if (!this.mDictTreeCell2GuiElement.ContainsKey(key))
			{
				this.mDictTreeCell2GuiElement.Add(key, new GUIElement_TreeCell(treeListColumn, dataSourceRowIndex));
			}
			return this.mDictTreeCell2GuiElement[key];
		}

		public void Reset()
		{
			this.mDictTreeCell2GuiElement.Clear();
		}
	}
}
