using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Vector.VLConfig.ValidationFramework
{
	public class GUIElementManager_Internal_Control
	{
		private IDictionary<Control, IValidatedGUIElement> dictControl2GUIElement;

		public GUIElementManager_Internal_Control()
		{
			this.dictControl2GUIElement = new Dictionary<Control, IValidatedGUIElement>();
		}

		public IValidatedGUIElement CreateOrGetGUIElementForControl(Control control)
		{
			if (!this.dictControl2GUIElement.Keys.Contains(control))
			{
				this.dictControl2GUIElement[control] = new GUIElement_Control(control);
			}
			return this.dictControl2GUIElement[control];
		}

		public void Reset()
		{
			this.dictControl2GUIElement.Clear();
		}
	}
}
