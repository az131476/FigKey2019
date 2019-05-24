using System;
using System.Windows.Forms;

namespace Vector.VLConfig.ValidationFramework
{
	internal interface ICustomErrorProviderGeneral
	{
		void SetErrorString(IValidatedGUIElement guiElement, ValidationErrorClass validationErrorClass, string errorString);

		void ResetErrorString(IValidatedGUIElement guiElement);

		void ResetErrorString(IValidatedGUIElement guiElement, ValidationErrorClass validationErrorClass);

		void RegisterErrorProviderForErrorClass(ValidationErrorClass validationErrorClass, ErrorProvider errorProvider);
	}
}
