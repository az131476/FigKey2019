using System;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.GUI
{
	public interface IPropertyWindow : IConfigClipboardClient
	{
		IModelValidator ModelValidator
		{
			get;
			set;
		}

		ISemanticChecker SemanticChecker
		{
			get;
			set;
		}

		IModelEditor ModelEditor
		{
			get;
			set;
		}

		IUpdateService UpdateService
		{
			get;
			set;
		}

		IUpdateObserver UpdateObserver
		{
			get;
		}

		PageType Type
		{
			get;
		}

		bool IsVisible
		{
			get;
			set;
		}

		bool IsDisplayingFeature(Feature feature);

		bool ValidateInput();

		bool HasErrors();

		bool HasGlobalErrors();

		bool HasLocalErrors();

		bool HasFormatErrors();

		void Init();

		void Reset();
	}
}
