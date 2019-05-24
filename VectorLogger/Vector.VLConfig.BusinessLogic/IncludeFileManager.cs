using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.GUI.IncludeFilesPage;
using Vector.VLConfig.Properties;
using Vector.VLConfig.ValidationFramework;

namespace Vector.VLConfig.BusinessLogic
{
	internal class IncludeFileManager
	{
		private static IncludeFileManager sInstance;

		private readonly List<IncludeFilePresenter> mIncludeFilePresenters = new List<IncludeFilePresenter>();

		private readonly List<IncludeFileParameterPresenter> mIncludeFileParameterPresenters = new List<IncludeFileParameterPresenter>();

		public IModelValidationResultCollector ResultCollector
		{
			get;
			set;
		}

		public static IncludeFileManager Instance
		{
			get
			{
				IncludeFileManager arg_17_0;
				if ((arg_17_0 = IncludeFileManager.sInstance) == null)
				{
					arg_17_0 = (IncludeFileManager.sInstance = new IncludeFileManager());
				}
				return arg_17_0;
			}
		}

		public IEnumerable<IncludeFilePresenter> Files
		{
			get
			{
				return this.mIncludeFilePresenters;
			}
		}

		public IEnumerable<IncludeFileParameterPresenter> Parameters
		{
			get
			{
				return this.mIncludeFileParameterPresenters;
			}
		}

		public IEnumerable<IncludeFileParameterPresenter> OutParameters
		{
			get
			{
				return (from param in this.mIncludeFileParameterPresenters
				where param.ParameterType == IncludeFileParameter.ParamType.Out
				select param).ToList<IncludeFileParameterPresenter>();
			}
		}

		public void Refresh(IncludeFileConfiguration includeFileConfiguration, out bool hasChanged)
		{
			hasChanged = false;
			this.mIncludeFileParameterPresenters.Clear();
			this.mIncludeFilePresenters.Clear();
			if (includeFileConfiguration == null)
			{
				return;
			}
			for (int i = 0; i < includeFileConfiguration.IncludeFiles.Count; i++)
			{
				IncludeFile includeFile = includeFileConfiguration.IncludeFiles[i];
				string absoluteFilePath = this.GetAbsoluteFilePath(includeFile.FilePath.Value);
				bool flag;
				IncludeFilePresenter includeFilePresenter = new IncludeFilePresenter(includeFile, i, absoluteFilePath, ref flag);
				hasChanged |= flag;
				this.mIncludeFilePresenters.Add(includeFilePresenter);
				this.mIncludeFileParameterPresenters.AddRange(includeFilePresenter.Parameters);
			}
		}

		private string GetAbsoluteFilePath(string pathRelativeToConfiguration)
		{
			return GenerationUtil.ConfigManager.Service.GetAbsoluteFilePath(pathRelativeToConfiguration);
		}

		public bool ValidateParsedData()
		{
			if (this.ResultCollector == null)
			{
				return true;
			}
			bool flag = true;
			foreach (IncludeFilePresenter current in this.mIncludeFilePresenters)
			{
				current.HasError = false;
			}
			flag &= this.ValidateInstanceParametersGlobal();
			foreach (IncludeFilePresenter current2 in this.mIncludeFilePresenters)
			{
				flag &= this.ValidateFileExists(current2);
				flag &= this.ValidateInstanceParameters(current2);
				flag &= this.ValidateInParameters(current2);
				flag &= this.ValidateOutParameters(current2);
			}
			return flag;
		}

		private bool ValidateInstanceParametersGlobal()
		{
			bool result = true;
			Dictionary<string, List<IncludeFilePresenter>> dictionary = (from pres in this.mIncludeFilePresenters
			group pres by pres.AbsoluteFilePath).ToDictionary((IGrouping<string, IncludeFilePresenter> grp) => grp.Key, (IGrouping<string, IncludeFilePresenter> grp) => grp.ToList<IncludeFilePresenter>());
			using (Dictionary<string, List<IncludeFilePresenter>>.Enumerator enumerator = dictionary.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, List<IncludeFilePresenter>> entry = enumerator.Current;
					KeyValuePair<string, List<IncludeFilePresenter>> entry5 = entry;
					if (entry5.Value.Count<IncludeFilePresenter>() > 1)
					{
						List<IncludeFileParameterPresenter> list = this.mIncludeFileParameterPresenters.Where(delegate(IncludeFileParameterPresenter param)
						{
							if (param.ParameterType == IncludeFileParameter.ParamType.Instance)
							{
								string arg_20_0 = param.AbsoluteFilePath;
								KeyValuePair<string, List<IncludeFilePresenter>> entry4 = entry;
								return arg_20_0 == entry4.Key;
							}
							return false;
						}).ToList<IncludeFileParameterPresenter>();
						int arg_F4_0 = list.Count;
						KeyValuePair<string, List<IncludeFilePresenter>> entry2 = entry;
						if (arg_F4_0 < entry2.Value.Count)
						{
							result = false;
							KeyValuePair<string, List<IncludeFilePresenter>> entry3 = entry;
							foreach (IncludeFilePresenter current in entry3.Value)
							{
								current.HasError = true;
								this.ResultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, current.IncFileProperty, Resources_IncFiles.ErrorInstanceParameterMissing);
							}
						}
						List<string> source = (from param in list
						select param.Value).Distinct<string>().ToList<string>();
						if (source.Count<string>() != list.Count<IncludeFileParameterPresenter>())
						{
							result = false;
							foreach (IncludeFileParameterPresenter current2 in list)
							{
								this.ResultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, current2.ValueProperty, Resources_IncFiles.ErrorInstanceParameterValuesNotUnique);
							}
						}
					}
				}
			}
			return result;
		}

		private bool ValidateFileExists(IncludeFilePresenter incFilePres)
		{
			bool result = true;
			if (!File.Exists(incFilePres.AbsoluteFilePath))
			{
				result = false;
				this.ResultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, incFilePres.IncFileProperty, Resources_IncFiles.ErrorFileDoesNotExist);
				incFilePres.HasError = true;
			}
			return result;
		}

		private bool ValidateInstanceParameters(IncludeFilePresenter incFilePres)
		{
			bool result = true;
			List<IncludeFileParameterPresenter> list = (from param in incFilePres.Parameters
			where param.ParameterType == IncludeFileParameter.ParamType.Instance
			select param).ToList<IncludeFileParameterPresenter>();
			if (list.Count > 1)
			{
				result = false;
				using (List<IncludeFileParameterPresenter>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						IncludeFileParameterPresenter current = enumerator.Current;
						this.ResultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, current.NameProperty, Resources_IncFiles.ErrorInstanceParameterCount);
					}
					return result;
				}
			}
			if (list.Count == 1 && !incFilePres.IsSingleInstance && !GenerationUtil.IsLtlCompliantNamePostfix(list.First<IncludeFileParameterPresenter>().Value))
			{
				result = false;
				this.ResultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, list.First<IncludeFileParameterPresenter>().ValueProperty, Resources_IncFiles.ErrorInstanceParameterValueNotLtlComplient);
			}
			return result;
		}

		private bool ValidateInParameters(IncludeFilePresenter incFilePres)
		{
			bool result = true;
			Dictionary<string, List<IncludeFileParameterPresenter>> dictionary = (from param in incFilePres.Parameters
			where param.ParameterType == IncludeFileParameter.ParamType.In || param.ParameterType == IncludeFileParameter.ParamType.Instance
			group param by param.ParameterNumber).ToDictionary((IGrouping<string, IncludeFileParameterPresenter> grp) => grp.Key, (IGrouping<string, IncludeFileParameterPresenter> grp) => grp.ToList<IncludeFileParameterPresenter>());
			foreach (string current in dictionary.Keys)
			{
				if (dictionary[current].Count > 1)
				{
					foreach (IncludeFileParameterPresenter current2 in dictionary[current])
					{
						result = false;
						this.ResultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, current2.NameProperty, Resources_IncFiles.ErrorParameterIdsNotUnique);
					}
				}
			}
			return result;
		}

		private bool ValidateOutParameters(IncludeFilePresenter incFilePres)
		{
			bool result = true;
			Dictionary<string, List<IncludeFileParameterPresenter>> dictionary = (from param in incFilePres.Parameters
			where param.ParameterType == IncludeFileParameter.ParamType.Out
			group param by param.LtlName).ToDictionary((IGrouping<string, IncludeFileParameterPresenter> grp) => grp.Key, (IGrouping<string, IncludeFileParameterPresenter> grp) => grp.ToList<IncludeFileParameterPresenter>());
			foreach (string current in dictionary.Keys)
			{
				if (dictionary[current].Count > 1)
				{
					using (List<IncludeFileParameterPresenter>.Enumerator enumerator2 = dictionary[current].GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							IncludeFileParameterPresenter current2 = enumerator2.Current;
							result = false;
							this.ResultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, current2.NameProperty, Resources_IncFiles.ErrorOutParameterLtlNamesNotUnique);
						}
						continue;
					}
				}
				if (dictionary[current].Count == 1 && !GenerationUtil.IsLtlCompliantName(current))
				{
					result = false;
					this.ResultCollector.SetErrorText(ValidationErrorClass.GlobalModelError, dictionary[current].First<IncludeFileParameterPresenter>().NameProperty, Resources_IncFiles.ErrorOutParameterLtlNameNotLtlComplient);
				}
			}
			return result;
		}

		public bool TryGetIncludeFile(IncEvent incEvent, out IncludeFilePresenter incFile)
		{
			incFile = this.mIncludeFilePresenters.FirstOrDefault((IncludeFilePresenter file) => file.IncludeFile.FilePath.Value.Equals(incEvent.FilePath.Value));
			return incFile != null;
		}

		public bool TryGetOutParameter(IncEvent incEvent, out IncludeFileParameterPresenter outParameter, bool ignoreInstance = false)
		{
			if (ignoreInstance)
			{
				outParameter = this.mIncludeFileParameterPresenters.FirstOrDefault((IncludeFileParameterPresenter param) => param.IncludeFile.FilePath.Value.Equals(incEvent.FilePath.Value) && param.ParameterIndex.Equals(incEvent.ParamIndex.Value));
			}
			else
			{
				outParameter = this.mIncludeFileParameterPresenters.FirstOrDefault((IncludeFileParameterPresenter param) => param.IncludeFile.FilePath.Value.Equals(incEvent.FilePath.Value) && param.Parent.InstanceValue.Equals(incEvent.InstanceValue.Value) && param.ParameterIndex.Equals(incEvent.ParamIndex.Value));
			}
			return outParameter != null;
		}

		public bool IsValidOutParameterForTriggers(IncludeFileParameterPresenter outParameter)
		{
			if (outParameter.ParameterType != IncludeFileParameter.ParamType.Out)
			{
				return false;
			}
			IncludeFilePresenter parent = outParameter.Parent;
			if (this.HasError(parent))
			{
				return false;
			}
			IncludeFileParameterPresenter includeFileParameterPresenter = parent.Parameters.FirstOrDefault((IncludeFileParameterPresenter param) => param.ParameterType == IncludeFileParameter.ParamType.Instance);
			return (includeFileParameterPresenter == null || !this.HasError(includeFileParameterPresenter)) && !this.HasError(outParameter);
		}

		public string GetFirstErrorTextForTriggers(IncludeFileParameterPresenter outParameter)
		{
			IncludeFilePresenter parent = outParameter.Parent;
			string firstErrorText;
			if (!string.IsNullOrEmpty(firstErrorText = this.GetFirstErrorText(parent)))
			{
				return firstErrorText;
			}
			IncludeFileParameterPresenter includeFileParameterPresenter = parent.Parameters.FirstOrDefault((IncludeFileParameterPresenter param) => param.ParameterType == IncludeFileParameter.ParamType.Instance);
			if (includeFileParameterPresenter != null && !string.IsNullOrEmpty(firstErrorText = this.GetFirstErrorText(includeFileParameterPresenter)))
			{
				return firstErrorText;
			}
			return this.GetFirstErrorText(outParameter);
		}

		private bool HasError(IncludeFilePresenter incFilePres)
		{
			return !string.IsNullOrEmpty(this.GetFirstErrorText(incFilePres));
		}

		private bool HasError(IncludeFileParameterPresenter paramPres)
		{
			return !string.IsNullOrEmpty(this.GetFirstErrorText(paramPres));
		}

		public string GetFirstErrorText(IncludeFilePresenter incFilePres)
		{
			string errorText = this.ResultCollector.GetErrorText(ValidationErrorClass.FormatError, incFilePres.IncFileProperty);
			if (!string.IsNullOrEmpty(errorText))
			{
				return errorText;
			}
			errorText = this.ResultCollector.GetErrorText(ValidationErrorClass.LocalModelError, incFilePres.IncFileProperty);
			if (!string.IsNullOrEmpty(errorText))
			{
				return errorText;
			}
			errorText = this.ResultCollector.GetErrorText(ValidationErrorClass.GlobalModelError, incFilePres.IncFileProperty);
			if (!string.IsNullOrEmpty(errorText))
			{
				return errorText;
			}
			return string.Empty;
		}

		public string GetFirstErrorText(IncludeFileParameterPresenter paramPres)
		{
			string errorText = this.ResultCollector.GetErrorText(ValidationErrorClass.FormatError, paramPres.NameProperty);
			if (!string.IsNullOrEmpty(errorText))
			{
				return errorText;
			}
			errorText = this.ResultCollector.GetErrorText(ValidationErrorClass.FormatError, paramPres.ValueProperty);
			if (!string.IsNullOrEmpty(errorText))
			{
				return errorText;
			}
			errorText = this.ResultCollector.GetErrorText(ValidationErrorClass.LocalModelError, paramPres.NameProperty);
			if (!string.IsNullOrEmpty(errorText))
			{
				return errorText;
			}
			errorText = this.ResultCollector.GetErrorText(ValidationErrorClass.LocalModelError, paramPres.ValueProperty);
			if (!string.IsNullOrEmpty(errorText))
			{
				return errorText;
			}
			errorText = this.ResultCollector.GetErrorText(ValidationErrorClass.GlobalModelError, paramPres.NameProperty);
			if (!string.IsNullOrEmpty(errorText))
			{
				return errorText;
			}
			errorText = this.ResultCollector.GetErrorText(ValidationErrorClass.GlobalModelError, paramPres.ValueProperty);
			if (!string.IsNullOrEmpty(errorText))
			{
				return errorText;
			}
			return string.Empty;
		}
	}
}
