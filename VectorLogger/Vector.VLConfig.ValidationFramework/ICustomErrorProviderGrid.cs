using DevExpress.XtraGrid.Views.Base;
using System;

namespace Vector.VLConfig.ValidationFramework
{
	internal interface ICustomErrorProviderGrid
	{
		bool DisplayError(IValidatedGUIElement guiElement, RowCellCustomDrawEventArgs e);
	}
}
