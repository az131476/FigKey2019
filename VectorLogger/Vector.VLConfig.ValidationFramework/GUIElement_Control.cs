using System;
using System.Windows.Forms;

namespace Vector.VLConfig.ValidationFramework
{
	public class GUIElement_Control : IValidatedGUIElement
	{
		private Control control;

		public Control Control
		{
			get
			{
				return this.control;
			}
		}

		public GUIElement_Control(Control control)
		{
			this.control = control;
		}
	}
}
