using System;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.ValidationFramework
{
	internal interface IPageValidatorGeneral
	{
		void Reset();

		void ResetAllFormatErrors();

		void ResetAllErrorProviders();

		string GetInvalidFormatErrorString_Int32();

		string GetInvalidFormatErrorString_Double();

		string GetInvalidFormatErrorString_CANId();

		string GetInvalidFormatErrorString_Byte();

		void ActivateErrorProvidersForFormatAndModelErrors();

		bool HasErrors(params ValidationErrorClass[] errorClasses);

		bool HasFormatError(IValidatedProperty validatedPropertyModel);

		bool HasError(IValidatedProperty validatedPropertyModel);
	}
}
