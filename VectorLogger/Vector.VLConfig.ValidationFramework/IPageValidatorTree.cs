using System;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.ValidationFramework
{
	internal interface IPageValidatorTree
	{
		void StoreMapping(IValidatedProperty dataModelElement, IValidatedGUIElement guiElement);

		bool UpdateModel<T>(T value, IValidatedProperty<T> dataModelElementToStoreIn, out bool valueChanged);
	}
}
