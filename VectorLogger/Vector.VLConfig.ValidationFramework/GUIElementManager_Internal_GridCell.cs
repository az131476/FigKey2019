using DevExpress.XtraGrid.Columns;
using System;
using System.Collections.Generic;

namespace Vector.VLConfig.ValidationFramework
{
	public class GUIElementManager_Internal_GridCell
	{
		private IDictionary<GridColumn, IDictionary<int, IValidatedGUIElement>> dictGridCell2GUIElement;

		public GUIElementManager_Internal_GridCell()
		{
			this.dictGridCell2GUIElement = new Dictionary<GridColumn, IDictionary<int, IValidatedGUIElement>>();
		}

		public IValidatedGUIElement CreateOrGetGUIElementForGridCell(GridColumn gridColumn, int dataSourceRowIndex)
		{
			IDictionary<int, IValidatedGUIElement> dictionary;
			if (!this.dictGridCell2GUIElement.Keys.Contains(gridColumn))
			{
				dictionary = new Dictionary<int, IValidatedGUIElement>();
				this.dictGridCell2GUIElement[gridColumn] = dictionary;
			}
			else
			{
				dictionary = this.dictGridCell2GUIElement[gridColumn];
			}
			if (!dictionary.Keys.Contains(dataSourceRowIndex))
			{
				dictionary[dataSourceRowIndex] = new GUIElement_GridCell(gridColumn, dataSourceRowIndex);
			}
			return dictionary[dataSourceRowIndex];
		}

		public void Reset()
		{
			this.dictGridCell2GUIElement.Clear();
		}
	}
}
