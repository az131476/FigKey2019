using System;
using System.Windows.Forms;

namespace Vector.VLConfig.ValidationFramework
{
	public class GUIElementManager_Control
	{
		private GUIElementManager_Internal_Control guiElementManager_Internal_Control;

		public GUIElementManager_Control()
		{
			this.guiElementManager_Internal_Control = new GUIElementManager_Internal_Control();
		}

		public IValidatedGUIElement GetGUIElement(Control control)
		{
			return this.guiElementManager_Internal_Control.CreateOrGetGUIElementForControl(control);
		}

		public void Reset()
		{
			this.guiElementManager_Internal_Control.Reset();
		}
	}
}
