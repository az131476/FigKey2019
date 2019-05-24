using System;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.ValidationFramework
{
	public interface IModelValidationResultCollector
	{
		void ResetAllModelErrors();

		void ResetAllErrorsOfClass(ValidationErrorClass validationErrorClass);

		void SetErrorText(ValidationErrorClass validationErrorClass, IValidatedProperty validatedModelElementModel, string errorText);

		string GetErrorText(ValidationErrorClass validationErrorClass, IValidatedProperty validatedModelElementModel);
	}
}
