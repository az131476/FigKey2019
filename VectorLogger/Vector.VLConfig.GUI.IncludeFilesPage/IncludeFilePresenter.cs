using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.GUI.IncludeFilesPage
{
	internal class IncludeFilePresenter
	{
		private readonly List<IncludeFileParameterPresenter> mParameters = new List<IncludeFileParameterPresenter>();

		public static readonly int ImageIndexOk = MainImageList.Instance.GetImageIndex(MainImageList.IconIndex.NoImage, false);

		public static readonly int ImageIndexWarning = MainImageList.Instance.GetImageIndex(MainImageList.IconIndex.Warning, false);

		public readonly ValidatedProperty<string> IncFileProperty = new ValidatedProperty<string>();

		public string FileName
		{
			get;
			private set;
		}

		public int Index
		{
			get;
			private set;
		}

		public int GroupImageIndex
		{
			get
			{
				if (!this.HasError)
				{
					return IncludeFilePresenter.ImageIndexOk;
				}
				return IncludeFilePresenter.ImageIndexWarning;
			}
		}

		public IEnumerable<IncludeFileParameterPresenter> Parameters
		{
			get
			{
				return this.mParameters;
			}
		}

		public IncludeFile IncludeFile
		{
			get;
			private set;
		}

		public string AbsoluteFilePath
		{
			get;
			private set;
		}

		public string Description
		{
			get;
			private set;
		}

		public bool HasError
		{
			get;
			set;
		}

		public IncludeFileParameterPresenter InstanceParameter
		{
			get
			{
				return this.mParameters.FirstOrDefault((IncludeFileParameterPresenter param) => param.ParameterType == IncludeFileParameter.ParamType.Instance);
			}
		}

		public string InstanceValue
		{
			get
			{
				if (this.InstanceParameter == null)
				{
					return string.Empty;
				}
				return this.InstanceParameter.Value;
			}
		}

		public bool IsSingleInstance
		{
			get
			{
				return IncludeFileManager.Instance.Files.Count((IncludeFilePresenter file) => file.AbsoluteFilePath.Equals(this.AbsoluteFilePath)) == 1;
			}
		}

		public IncludeFilePresenter(IncludeFile includeFile, int includeFileIndex, string absoluteIncludeFilePath, out bool hasChanged)
		{
			this.IncludeFile = includeFile;
			this.Index = includeFileIndex;
			this.AbsoluteFilePath = absoluteIncludeFilePath;
			this.FileName = Path.GetFileNameWithoutExtension(absoluteIncludeFilePath);
			this.CreateParameters(out hasChanged);
		}

		private void CreateParameters(out bool hasChanged)
		{
			hasChanged = false;
			this.mParameters.Clear();
			this.HasError = false;
			IncludeFileParser includeFileParser = new IncludeFileParser(this.AbsoluteFilePath);
			if (includeFileParser.HasFileNotFoundError)
			{
				this.HasError = true;
				for (int i = 0; i < this.IncludeFile.NumberOfParameters; i++)
				{
					includeFileParser.Parameters.Add(IncludeFileParameter.CreateInParam(i + 1, "", ""));
				}
			}
			while (this.IncludeFile.NumberOfParameters > includeFileParser.Parameters.Count)
			{
				this.IncludeFile.Parameters.RemoveAt(this.IncludeFile.NumberOfParameters - 1);
				hasChanged = true;
			}
			while (this.IncludeFile.NumberOfParameters < includeFileParser.Parameters.Count)
			{
				this.IncludeFile.Parameters.Add(new ValidatedProperty<string>(string.Empty));
				hasChanged = true;
			}
			for (int j = 0; j < this.IncludeFile.NumberOfParameters; j++)
			{
				IncludeFileParameter includeFileParameter = includeFileParser.Parameters[j];
				if (includeFileParameter.Type == IncludeFileParameter.ParamType.Dummy || includeFileParameter.Type == IncludeFileParameter.ParamType.Out)
				{
					hasChanged |= !string.IsNullOrEmpty(this.IncludeFile.Parameters[j].Value);
					this.IncludeFile.Parameters[j].Value = string.Empty;
				}
			}
			this.mParameters.AddRange(from param in includeFileParser.Parameters
			select new IncludeFileParameterPresenter(this, param));
			this.Description = includeFileParser.FileDescription;
			IncludeFileParameterPresenter includeFileParameterPresenter = this.mParameters.FirstOrDefault((IncludeFileParameterPresenter param) => param.ParameterType == IncludeFileParameter.ParamType.Out);
			this.IncludeFile.HighestInParameterIndex = ((includeFileParameterPresenter != null) ? (includeFileParameterPresenter.ParameterIndex - 1) : (this.mParameters.Count - 1));
		}

		public int IndexOf(IncludeFileParameterPresenter child)
		{
			return this.mParameters.IndexOf(child);
		}

		public bool InstanceValueEquals(string instanceValue)
		{
			return (this.InstanceParameter == null && instanceValue == null) || (this.InstanceParameter != null && instanceValue != null && this.InstanceParameter.Value.Equals(instanceValue));
		}
	}
}
