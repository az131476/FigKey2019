using System;
using System.Windows.Forms;

namespace Vector.VLConfig.ValidationFramework
{
	internal interface IValidationHost
	{
		GUIElementManager_Control GUIElementManager
		{
			get;
		}

		PageValidator PageValidator
		{
			get;
		}

		bool ValidateInput();

		void RegisterForErrorProvider(Control control);
	}
}
