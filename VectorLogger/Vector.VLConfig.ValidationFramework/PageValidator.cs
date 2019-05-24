using System;
using System.Collections.Generic;
using System.Globalization;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.ValidationFramework
{
	internal class PageValidator : IModelValidationResultCollector, IPageValidatorGeneral, IPageValidatorControl, IPageValidatorGrid, IPageValidatorTree
	{
		private readonly CustomErrorProvider mCustomErrorProvider;

		private readonly IDictionary<ValidationErrorClass, IDictionary<IValidatedProperty, string>> mDictErrorClass2Errors;

		private readonly IDictionary<IValidatedProperty, IDictionary<IValidatedGUIElement, bool>> mDictModelElement2GuiElements;

		public IPageValidatorGeneral General
		{
			get
			{
				return this;
			}
		}

		public IPageValidatorControl Control
		{
			get
			{
				return this;
			}
		}

		public IPageValidatorGrid Grid
		{
			get
			{
				return this;
			}
		}

		public IPageValidatorTree Tree
		{
			get
			{
				return this;
			}
		}

		public IModelValidationResultCollector ResultCollector
		{
			get
			{
				return this;
			}
		}

		public PageValidator(CustomErrorProvider customErrorProvider)
		{
			this.mCustomErrorProvider = customErrorProvider;
			this.mDictModelElement2GuiElements = new Dictionary<IValidatedProperty, IDictionary<IValidatedGUIElement, bool>>();
			this.mDictErrorClass2Errors = new Dictionary<ValidationErrorClass, IDictionary<IValidatedProperty, string>>();
		}

		void IPageValidatorGeneral.Reset()
		{
			this.General.ResetAllErrorProviders();
			this.mDictErrorClass2Errors.Clear();
			this.mDictModelElement2GuiElements.Clear();
		}

		void IPageValidatorGeneral.ResetAllFormatErrors()
		{
			((IModelValidationResultCollector)this).ResetAllErrorsOfClass(ValidationErrorClass.FormatError);
		}

		void IPageValidatorGeneral.ResetAllErrorProviders()
		{
			foreach (IDictionary<IValidatedGUIElement, bool> current in this.mDictModelElement2GuiElements.Values)
			{
				foreach (IValidatedGUIElement current2 in current.Keys)
				{
					this.mCustomErrorProvider.General.ResetErrorString(current2);
				}
			}
		}

		string IPageValidatorGeneral.GetInvalidFormatErrorString_Int32()
		{
			return Resources.InvalidValueInteger;
		}

		string IPageValidatorGeneral.GetInvalidFormatErrorString_Double()
		{
			return Resources.InvalidValueDouble;
		}

		string IPageValidatorGeneral.GetInvalidFormatErrorString_Byte()
		{
			return Resources.InvalidValueByte;
		}

		string IPageValidatorGeneral.GetInvalidFormatErrorString_CANId()
		{
			return Resources.ErrorCANIdExpected;
		}

		void IPageValidatorGeneral.ActivateErrorProvidersForFormatAndModelErrors()
		{
			foreach (ValidationErrorClass validationErrorClass in Enum.GetValues(typeof(ValidationErrorClass)))
			{
				IDictionary<IValidatedProperty, string> errorDict4ErrorClass = this.GetErrorDict4ErrorClass(validationErrorClass);
				foreach (KeyValuePair<IValidatedProperty, string> current in errorDict4ErrorClass)
				{
					IValidatedProperty key = current.Key;
					string value = current.Value;
					if (this.mDictModelElement2GuiElements.ContainsKey(key))
					{
						IDictionary<IValidatedGUIElement, bool> dictionary = this.mDictModelElement2GuiElements[key];
						foreach (IValidatedGUIElement current2 in dictionary.Keys)
						{
							this.mCustomErrorProvider.General.SetErrorString(current2, validationErrorClass, value);
						}
					}
				}
			}
		}

		bool IPageValidatorGeneral.HasErrors(params ValidationErrorClass[] errorClasses)
		{
			for (int i = 0; i < errorClasses.Length; i++)
			{
				ValidationErrorClass key = errorClasses[i];
				if (this.mDictErrorClass2Errors.ContainsKey(key))
				{
					foreach (KeyValuePair<IValidatedProperty, string> current in this.mDictErrorClass2Errors[key])
					{
						if (!string.IsNullOrEmpty(current.Value))
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		bool IPageValidatorGeneral.HasFormatError(IValidatedProperty validatedPropertyModel)
		{
			return this.mDictErrorClass2Errors.ContainsKey(ValidationErrorClass.FormatError) && this.mDictErrorClass2Errors[ValidationErrorClass.FormatError].ContainsKey(validatedPropertyModel) && !string.IsNullOrEmpty(this.mDictErrorClass2Errors[ValidationErrorClass.FormatError][validatedPropertyModel]);
		}

		bool IPageValidatorGeneral.HasError(IValidatedProperty validatedPropertyModel)
		{
			foreach (IDictionary<IValidatedProperty, string> current in this.mDictErrorClass2Errors.Values)
			{
				if (current.ContainsKey(validatedPropertyModel) && !string.IsNullOrEmpty(current[validatedPropertyModel]))
				{
					return true;
				}
			}
			return false;
		}

		bool IPageValidatorControl.ValidateFormatAndUpdateModel_Int32(string valueAsStringFromGui, IValidatedProperty<int> dataModelElementToStoreIn, IValidatedGUIElement sourceGuiElement, out bool valueChanged)
		{
			this.StoreMappingModel2GUI(dataModelElementToStoreIn, sourceGuiElement);
			valueChanged = false;
			int num;
			if (int.TryParse(valueAsStringFromGui, out num))
			{
				valueChanged = (dataModelElementToStoreIn.Value != num);
				dataModelElementToStoreIn.Value = num;
				return true;
			}
			this.ResultCollector.SetErrorText(ValidationErrorClass.FormatError, dataModelElementToStoreIn, this.General.GetInvalidFormatErrorString_Int32());
			return false;
		}

		bool IPageValidatorControl.ValidateFormatAndUpdateModel_UInt32(string valueAsStringFromGui, IValidatedProperty<uint> dataModelElementToStoreIn, IValidatedGUIElement sourceGuiElement, out bool valueChanged)
		{
			this.StoreMappingModel2GUI(dataModelElementToStoreIn, sourceGuiElement);
			valueChanged = false;
			uint num;
			if (uint.TryParse(valueAsStringFromGui, out num))
			{
				valueChanged = (dataModelElementToStoreIn.Value != num);
				dataModelElementToStoreIn.Value = num;
				return true;
			}
			this.ResultCollector.SetErrorText(ValidationErrorClass.FormatError, dataModelElementToStoreIn, this.General.GetInvalidFormatErrorString_Int32());
			return false;
		}

		bool IPageValidatorControl.ValidateFormatAndUpdateModel_Double(string valueAsStringFromGui, IValidatedProperty<double> dataModelElementToStoreIn, IValidatedGUIElement sourceGuiElement, out bool valueChanged)
		{
			this.StoreMappingModel2GUI(dataModelElementToStoreIn, sourceGuiElement);
			valueChanged = false;
			double num;
			if (GUIUtil.DisplayStringToFloatNumber(valueAsStringFromGui, out num))
			{
				valueChanged = (dataModelElementToStoreIn.Value != num);
				dataModelElementToStoreIn.Value = num;
				return true;
			}
			this.ResultCollector.SetErrorText(ValidationErrorClass.FormatError, dataModelElementToStoreIn, this.General.GetInvalidFormatErrorString_Double());
			return false;
		}

		bool IPageValidatorControl.ValidateFormatAndUpdateModel_Byte(string valueAsStringFromGui, IValidatedProperty<byte> dataModelElementToStoreIn, IValidatedGUIElement sourceGuiElement, out bool valueChanged)
		{
			this.StoreMappingModel2GUI(dataModelElementToStoreIn, sourceGuiElement);
			valueChanged = false;
			byte b;
			if (byte.TryParse(valueAsStringFromGui, NumberStyles.HexNumber, ProgramUtils.Culture, out b))
			{
				valueChanged = (dataModelElementToStoreIn.Value != b);
				dataModelElementToStoreIn.Value = b;
				return true;
			}
			this.ResultCollector.SetErrorText(ValidationErrorClass.FormatError, dataModelElementToStoreIn, this.General.GetInvalidFormatErrorString_Byte());
			return false;
		}

		bool IPageValidatorControl.ValidateFormatAndUpdateModel_CANId(string valueAsStringFromGui, IValidatedProperty<uint> dataModelPureCanIdValue, IValidatedProperty<bool> dataModelIsExtendedCanId, IValidatedGUIElement sourceGuiElement, out bool valueChanged)
		{
			valueChanged = false;
			this.StoreMappingModel2GUI(dataModelPureCanIdValue, sourceGuiElement);
			this.StoreMappingModel2GUI(dataModelIsExtendedCanId, sourceGuiElement);
			uint num;
			bool flag;
			if (GUIUtil.DisplayStringToCANId(valueAsStringFromGui, out num, out flag))
			{
				valueChanged = (dataModelPureCanIdValue.Value != num || dataModelIsExtendedCanId.Value != flag);
				dataModelPureCanIdValue.Value = num;
				dataModelIsExtendedCanId.Value = flag;
				return true;
			}
			this.ResultCollector.SetErrorText(ValidationErrorClass.FormatError, dataModelPureCanIdValue, this.General.GetInvalidFormatErrorString_CANId());
			return false;
		}

		bool IPageValidatorControl.UpdateModel<T>(T value, IValidatedProperty<T> dataModelElementToStoreIn, IValidatedGUIElement sourceGuiElement, out bool valueChanged)
		{
			this.StoreMappingModel2GUI(dataModelElementToStoreIn, sourceGuiElement);
			bool arg_48_1;
			if (dataModelElementToStoreIn.Value != null || value == null)
			{
				T value2 = dataModelElementToStoreIn.Value;
				arg_48_1 = !value2.Equals(value);
			}
			else
			{
				arg_48_1 = true;
			}
			valueChanged = arg_48_1;
			dataModelElementToStoreIn.Value = value;
			return true;
		}

		bool IPageValidatorGrid.UpdateModel<T>(T value, IValidatedProperty<T> dataModelElementToStoreIn, out bool valueChanged)
		{
			valueChanged = false;
			if (dataModelElementToStoreIn.Value == null && value == null)
			{
				return false;
			}
			bool arg_7A_1;
			if ((dataModelElementToStoreIn.Value != null || value == null) && (dataModelElementToStoreIn.Value == null || value != null))
			{
				T value2 = dataModelElementToStoreIn.Value;
				arg_7A_1 = !value2.Equals(value);
			}
			else
			{
				arg_7A_1 = true;
			}
			valueChanged = arg_7A_1;
			dataModelElementToStoreIn.Value = value;
			return true;
		}

		void IPageValidatorGrid.StoreMapping(IValidatedProperty dataModelElement, IValidatedGUIElement guiElement)
		{
			this.StoreMappingModel2GUI(dataModelElement, guiElement);
		}

		bool IPageValidatorTree.UpdateModel<T>(T value, IValidatedProperty<T> dataModelElementToStoreIn, out bool valueChanged)
		{
			valueChanged = false;
			if (dataModelElementToStoreIn.Value == null && value == null)
			{
				return false;
			}
			bool arg_7A_1;
			if ((dataModelElementToStoreIn.Value != null || value == null) && (dataModelElementToStoreIn.Value == null || value != null))
			{
				T value2 = dataModelElementToStoreIn.Value;
				arg_7A_1 = !value2.Equals(value);
			}
			else
			{
				arg_7A_1 = true;
			}
			valueChanged = arg_7A_1;
			dataModelElementToStoreIn.Value = value;
			return true;
		}

		void IPageValidatorTree.StoreMapping(IValidatedProperty dataModelElement, IValidatedGUIElement guiElement)
		{
			this.StoreMappingModel2GUI(dataModelElement, guiElement);
		}

		void IModelValidationResultCollector.SetErrorText(ValidationErrorClass validationErrorClass, IValidatedProperty validatedModelElementModel, string errorText)
		{
			this.GetErrorDict4ErrorClass(validationErrorClass)[validatedModelElementModel] = errorText;
		}

		string IModelValidationResultCollector.GetErrorText(ValidationErrorClass validationErrorClass, IValidatedProperty validatedModelElementModel)
		{
			if (this.GetErrorDict4ErrorClass(validationErrorClass).ContainsKey(validatedModelElementModel))
			{
				return this.GetErrorDict4ErrorClass(validationErrorClass)[validatedModelElementModel];
			}
			return string.Empty;
		}

		void IModelValidationResultCollector.ResetAllErrorsOfClass(ValidationErrorClass validationErrorClass)
		{
			this.GetErrorDict4ErrorClass(validationErrorClass).Clear();
		}

		void IModelValidationResultCollector.ResetAllModelErrors()
		{
			foreach (ValidationErrorClass validationErrorClass in Enum.GetValues(typeof(ValidationErrorClass)))
			{
				if (validationErrorClass != ValidationErrorClass.FormatError)
				{
					((IModelValidationResultCollector)this).ResetAllErrorsOfClass(validationErrorClass);
				}
			}
		}

		private void StoreMappingModel2GUI(IValidatedProperty modelElement, IValidatedGUIElement sourceGuiElement)
		{
			if (!this.mDictModelElement2GuiElements.ContainsKey(modelElement))
			{
				IDictionary<IValidatedGUIElement, bool> value = new Dictionary<IValidatedGUIElement, bool>();
				this.mDictModelElement2GuiElements[modelElement] = value;
			}
			this.mDictModelElement2GuiElements[modelElement][sourceGuiElement] = true;
		}

		private IDictionary<IValidatedProperty, string> GetErrorDict4ErrorClass(ValidationErrorClass validationErrorClass)
		{
			IDictionary<IValidatedProperty, string> dictionary;
			if (!this.mDictErrorClass2Errors.ContainsKey(validationErrorClass))
			{
				dictionary = new Dictionary<IValidatedProperty, string>();
				this.mDictErrorClass2Errors[validationErrorClass] = dictionary;
			}
			else
			{
				dictionary = this.mDictErrorClass2Errors[validationErrorClass];
			}
			return dictionary;
		}
	}
}
