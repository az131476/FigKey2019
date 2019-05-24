using System;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.ValidationFramework
{
	internal interface IPageValidatorControl
	{
		bool ValidateFormatAndUpdateModel_Int32(string valueAsStringFromGUI, IValidatedProperty<int> dataModelElementToStoreIn, IValidatedGUIElement sourceGUIElement, out bool valueChanged);

		bool ValidateFormatAndUpdateModel_UInt32(string valueAsStringFromGUI, IValidatedProperty<uint> dataModelElementToStoreIn, IValidatedGUIElement sourceGUIElement, out bool valueChanged);

		bool ValidateFormatAndUpdateModel_Double(string valueAsStringFromGUI, IValidatedProperty<double> dataModelElementToStoreIn, IValidatedGUIElement sourceGUIElement, out bool valueChanged);

		bool ValidateFormatAndUpdateModel_Byte(string valueAsStringFromGUI, IValidatedProperty<byte> dataModelElementToStoreIn, IValidatedGUIElement sourceGUIElement, out bool valueChanged);

		bool ValidateFormatAndUpdateModel_CANId(string valueAsStringFromGUI, IValidatedProperty<uint> dataModelPureCANIdValue, IValidatedProperty<bool> dataModelIsExtendedCANId, IValidatedGUIElement sourceGUIElement, out bool valueChanged);

		bool UpdateModel<T>(T value, IValidatedProperty<T> dataModelElementToStoreIn, IValidatedGUIElement sourceGUIElement, out bool valueChanged);
	}
}
