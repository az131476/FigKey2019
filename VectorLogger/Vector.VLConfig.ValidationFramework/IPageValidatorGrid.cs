using System;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.ValidationFramework
{
	internal interface IPageValidatorGrid
	{
		void StoreMapping(IValidatedProperty dataModelElement, IValidatedGUIElement guiElement);

		bool UpdateModel<T>(T value, IValidatedProperty<T> dataModelElementToStoreIn, out bool valueChanged);
	}
}
