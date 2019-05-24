using System;
using System.Globalization;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.GUI.IncludeFilesPage
{
	internal class IncludeFileParameterPresenter
	{
		private const string NoValue = "-";

		private readonly IncludeFilePresenter mParent;

		private readonly int mParameterNumber;

		private readonly string mName;

		private readonly string mDescription;

		public readonly ValidatedProperty<string> NameProperty = new ValidatedProperty<string>();

		public string FileName
		{
			get
			{
				return this.mParent.FileName;
			}
		}

		public int IncludeFileIndex
		{
			get
			{
				return this.mParent.Index;
			}
		}

		public int GroupImageIndex
		{
			get
			{
				return this.mParent.GroupImageIndex;
			}
		}

		public string ParameterNumber
		{
			get
			{
				if (this.ParameterType == IncludeFileParameter.ParamType.Out)
				{
					return string.Empty;
				}
				return this.mParameterNumber.ToString(CultureInfo.InvariantCulture);
			}
		}

		public int TypeIndex
		{
			get
			{
				return (int)this.ParameterType;
			}
		}

		public string Name
		{
			get
			{
				if (this.ParameterType != IncludeFileParameter.ParamType.Dummy)
				{
					return this.mName;
				}
				return "-";
			}
		}

		public string Value
		{
			get
			{
				if (this.ParameterType != IncludeFileParameter.ParamType.Out && this.ParameterType != IncludeFileParameter.ParamType.Dummy)
				{
					return this.mParent.IncludeFile.Parameters[this.ParameterIndex].Value;
				}
				return "-";
			}
			set
			{
				if (this.ParameterType == IncludeFileParameter.ParamType.Out || this.ParameterType == IncludeFileParameter.ParamType.Dummy)
				{
					return;
				}
				this.mParent.IncludeFile.Parameters[this.ParameterIndex].Value = value;
			}
		}

		public string Description
		{
			get
			{
				if (this.ParameterType != IncludeFileParameter.ParamType.Dummy)
				{
					return this.mDescription;
				}
				return "-";
			}
		}

		public IncludeFileParameter.ParamType ParameterType
		{
			get;
			private set;
		}

		public string LtlName
		{
			get;
			private set;
		}

		public string ExtendedDescription
		{
			get;
			private set;
		}

		public int ParameterIndex
		{
			get
			{
				return this.mParent.IndexOf(this);
			}
		}

		public IncludeFile IncludeFile
		{
			get
			{
				return this.mParent.IncludeFile;
			}
		}

		public IncludeFilePresenter Parent
		{
			get
			{
				return this.mParent;
			}
		}

		public string AbsoluteFilePath
		{
			get
			{
				return this.mParent.AbsoluteFilePath;
			}
		}

		public string FileDescription
		{
			get
			{
				return this.mParent.Description;
			}
		}

		public ValidatedProperty<string> ValueProperty
		{
			get
			{
				if (this.ParameterType == IncludeFileParameter.ParamType.Out || this.ParameterType == IncludeFileParameter.ParamType.Dummy)
				{
					return new ValidatedProperty<string>();
				}
				return this.mParent.IncludeFile.Parameters[this.ParameterIndex];
			}
		}

		public IncludeFileParameterPresenter(IncludeFilePresenter includeFilePresenter, IncludeFileParameter incFileParam)
		{
			this.mParent = includeFilePresenter;
			this.ParameterType = incFileParam.Type;
			this.mParameterNumber = incFileParam.ParamNumber;
			this.mName = incFileParam.Name;
			this.LtlName = incFileParam.LtlName;
			this.mDescription = incFileParam.Description;
			this.ExtendedDescription = incFileParam.ExtendedDescription;
		}
	}
}
