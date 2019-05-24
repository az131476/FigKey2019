using DevExpress.XtraTreeList;
using System;

namespace Vector.VLConfig.ValidationFramework
{
	internal interface ICustomErrorProviderTree
	{
		bool DisplayError(IValidatedGUIElement guiElement, CustomDrawNodeCellEventArgs e);
	}
}
