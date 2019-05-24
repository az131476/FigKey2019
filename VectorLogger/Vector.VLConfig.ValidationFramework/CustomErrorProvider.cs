using DevExpress.XtraEditors;
using DevExpress.XtraEditors.DXErrorProvider;
using DevExpress.XtraEditors.ViewInfo;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraTreeList;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Vector.VLConfig.ValidationFramework
{
	internal class CustomErrorProvider : ICustomErrorProviderGeneral, ICustomErrorProviderControl, ICustomErrorProviderGrid, ICustomErrorProviderTree, IDisposable
	{
		private readonly IDictionary<IValidatedGUIElement, ErrorString2ErrorClass> mDictGuiElement2ErrorString = new Dictionary<IValidatedGUIElement, ErrorString2ErrorClass>();

		private readonly IDictionary<ValidationErrorClass, ErrorProvider> mDictErrorClass2ErrorProvider = new Dictionary<ValidationErrorClass, ErrorProvider>();

		public ICustomErrorProviderGeneral General
		{
			get
			{
				return this;
			}
		}

		public ICustomErrorProviderControl Control
		{
			get
			{
				return this;
			}
		}

		public ICustomErrorProviderGrid Grid
		{
			get
			{
				return this;
			}
		}

		public ICustomErrorProviderTree Tree
		{
			get
			{
				return this;
			}
		}

		public CustomErrorProvider(ErrorProvider standardErrorProvider)
		{
			if (standardErrorProvider == null)
			{
				throw new ArgumentNullException("standardErrorProvider");
			}
			this.RegisterErrorProviderForAllErrorClasses(standardErrorProvider);
		}

		void ICustomErrorProviderGeneral.RegisterErrorProviderForErrorClass(ValidationErrorClass validationErrorClass, ErrorProvider errorProvider)
		{
			this.mDictErrorClass2ErrorProvider[validationErrorClass] = errorProvider;
		}

		void ICustomErrorProviderGeneral.SetErrorString(IValidatedGUIElement guiElement, ValidationErrorClass validationErrorClass, string errorString)
		{
			ErrorString2ErrorClass value;
			if (this.mDictGuiElement2ErrorString.ContainsKey(guiElement))
			{
				value = this.mDictGuiElement2ErrorString[guiElement];
				if (value.validationErrorClass < validationErrorClass)
				{
					return;
				}
			}
			value.errorString = errorString;
			value.validationErrorClass = validationErrorClass;
			this.mDictGuiElement2ErrorString[guiElement] = value;
			GUIElement_Control gUIElement_Control = guiElement as GUIElement_Control;
			ErrorProvider standardErrorProvider = this.GetStandardErrorProvider(validationErrorClass);
			if (gUIElement_Control != null)
			{
				this.ResetStandardErrorProvidersForGUIElement(guiElement);
				standardErrorProvider.SetError(gUIElement_Control.Control, errorString);
			}
		}

		void ICustomErrorProviderGeneral.ResetErrorString(IValidatedGUIElement guiElement)
		{
			this.mDictGuiElement2ErrorString.Remove(guiElement);
			GUIElement_Control gUIElement_Control = guiElement as GUIElement_Control;
			if (gUIElement_Control != null)
			{
				this.ResetStandardErrorProvidersForGUIElement(guiElement);
			}
		}

		void ICustomErrorProviderGeneral.ResetErrorString(IValidatedGUIElement guiElement, ValidationErrorClass errorClass)
		{
			if (this.mDictGuiElement2ErrorString.ContainsKey(guiElement) && errorClass == this.mDictGuiElement2ErrorString[guiElement].validationErrorClass)
			{
				this.mDictGuiElement2ErrorString.Remove(guiElement);
			}
			GUIElement_Control gUIElement_Control = guiElement as GUIElement_Control;
			if (gUIElement_Control != null)
			{
				this.ResetStandardErrorProvidersForGUIElement(guiElement, errorClass);
			}
		}

		bool ICustomErrorProviderGrid.DisplayError(IValidatedGUIElement guiElement, RowCellCustomDrawEventArgs e)
		{
			if (e.Column == null)
			{
				return false;
			}
			GridView gridView = e.Column.View as GridView;
			if (gridView == null)
			{
				return false;
			}
			GridCellInfo gridCellInfo = e.Cell as GridCellInfo;
			if (gridCellInfo == null)
			{
				return false;
			}
			bool result = false;
			if (this.mDictGuiElement2ErrorString.ContainsKey(guiElement))
			{
				ValidationErrorClass validationErrorClass = this.mDictGuiElement2ErrorString[guiElement].validationErrorClass;
				ErrorProvider errorProvider = this.mDictErrorClass2ErrorProvider[validationErrorClass];
				string errorString = this.mDictGuiElement2ErrorString[guiElement].errorString;
				result = !string.IsNullOrEmpty(errorString);
				if (gridView.ActiveEditor != null && gridView.FocusedColumn == e.Column && gridView.FocusedRowHandle == e.RowHandle)
				{
					gridView.ActiveEditor.ErrorIcon = errorProvider.Icon.ToBitmap();
					gridView.ActiveEditor.ErrorText = errorString;
				}
				else
				{
					gridCellInfo.ViewInfo.ErrorIconText = errorString;
					gridCellInfo.ViewInfo.ShowErrorIcon = true;
					gridCellInfo.ViewInfo.ErrorIcon = errorProvider.Icon.ToBitmap();
					gridCellInfo.ViewInfo.CalcViewInfo(e.Graphics);
				}
			}
			else
			{
				gridCellInfo.ViewInfo.ErrorIconText = string.Empty;
				gridCellInfo.ViewInfo.ShowErrorIcon = false;
				gridCellInfo.ViewInfo.ErrorIcon = null;
				gridCellInfo.ViewInfo.CalcViewInfo(e.Graphics);
			}
			return result;
		}

		private void OnDXErrorProviderGetErrorIcon(GetErrorIconEventArgs e)
		{
			ValidationErrorClass key = this.MapDxErrorProviderType2ErrorClass(e.ErrorType);
			e.ErrorIcon = this.mDictErrorClass2ErrorProvider[key].Icon.ToBitmap();
		}

		private ErrorType MapErrorClass2DxErrorProviderType(ValidationErrorClass errorClass)
		{
			return (ErrorType)(errorClass + 1);
		}

		private ValidationErrorClass MapDxErrorProviderType2ErrorClass(ErrorType errorType)
		{
			return (ValidationErrorClass)(errorType - 1);
		}

		bool ICustomErrorProviderTree.DisplayError(IValidatedGUIElement guiElement, CustomDrawNodeCellEventArgs e)
		{
			if (e == null || e.Column == null || e.Column.TreeList == null || e.EditViewInfo == null)
			{
				return false;
			}
			TreeList treeList = e.Column.TreeList;
			BaseEditViewInfo editViewInfo = e.EditViewInfo;
			bool result = false;
			if (this.mDictGuiElement2ErrorString.ContainsKey(guiElement))
			{
				ValidationErrorClass validationErrorClass = this.mDictGuiElement2ErrorString[guiElement].validationErrorClass;
				ErrorProvider errorProvider = this.mDictErrorClass2ErrorProvider[validationErrorClass];
				string errorString = this.mDictGuiElement2ErrorString[guiElement].errorString;
				result = !string.IsNullOrEmpty(errorString);
				if (treeList.ActiveEditor != null && treeList.FocusedColumn == e.Column && treeList.FocusedNode == e.Node)
				{
					treeList.ActiveEditor.ErrorIcon = errorProvider.Icon.ToBitmap();
					treeList.ActiveEditor.ErrorText = errorString;
				}
				else
				{
					editViewInfo.ErrorIconText = errorString;
					editViewInfo.ShowErrorIcon = true;
					editViewInfo.ErrorIcon = errorProvider.Icon.ToBitmap();
					editViewInfo.CalcViewInfo(e.Graphics);
				}
			}
			else
			{
				editViewInfo.ErrorIconText = string.Empty;
				editViewInfo.ShowErrorIcon = false;
				editViewInfo.ErrorIcon = null;
				editViewInfo.CalcViewInfo(e.Graphics);
			}
			return result;
		}

		private ErrorProvider GetStandardErrorProvider(ValidationErrorClass validationErrorClass)
		{
			return this.mDictErrorClass2ErrorProvider[validationErrorClass];
		}

		private void ResetStandardErrorProvidersForGUIElement(IValidatedGUIElement guiElement)
		{
			GUIElement_Control gUIElement_Control = guiElement as GUIElement_Control;
			if (gUIElement_Control != null)
			{
				foreach (ErrorProvider current in this.mDictErrorClass2ErrorProvider.Values)
				{
					current.SetError(gUIElement_Control.Control, string.Empty);
				}
			}
		}

		private void ResetStandardErrorProvidersForGUIElement(IValidatedGUIElement guiElement, ValidationErrorClass errorClass)
		{
			GUIElement_Control gUIElement_Control = guiElement as GUIElement_Control;
			if (gUIElement_Control != null && this.mDictErrorClass2ErrorProvider.ContainsKey(errorClass))
			{
				this.mDictErrorClass2ErrorProvider[errorClass].SetError(gUIElement_Control.Control, string.Empty);
			}
		}

		private void DisplayError(BaseEdit inplaceEditor, ErrorProvider errorProvider, string errorText)
		{
			foreach (ErrorProvider current in this.mDictErrorClass2ErrorProvider.Values)
			{
				if (current != errorProvider)
				{
					current.SetError(inplaceEditor, string.Empty);
				}
			}
			errorProvider.SetIconAlignment(inplaceEditor, ErrorIconAlignment.MiddleLeft);
			errorProvider.SetIconPadding(inplaceEditor, -18);
			errorProvider.SetError(inplaceEditor, errorText);
		}

		private void RegisterErrorProviderForAllErrorClasses(ErrorProvider errorProvider)
		{
			foreach (ValidationErrorClass key in Enum.GetValues(typeof(ValidationErrorClass)))
			{
				this.mDictErrorClass2ErrorProvider[key] = errorProvider;
			}
		}

		public void Dispose()
		{
			DXErrorProvider.GetErrorIcon -= new GetErrorIconEventHandler(this.OnDXErrorProviderGetErrorIcon);
		}
	}
}
