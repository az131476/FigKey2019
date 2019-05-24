using System;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.IncludeFilesPage
{
	internal class IncludeFileParameter
	{
		public enum ParamType
		{
			Dummy,
			Instance,
			In,
			Out
		}

		public IncludeFileParameter.ParamType Type
		{
			get;
			private set;
		}

		public int ParamNumber
		{
			get;
			private set;
		}

		public string LtlName
		{
			get;
			private set;
		}

		public string Name
		{
			get;
			private set;
		}

		public string Description
		{
			get;
			private set;
		}

		public string ExtendedDescription
		{
			get;
			set;
		}

		public static IncludeFileParameter CreateDummyParameter(int paramNumber)
		{
			return new IncludeFileParameter(IncludeFileParameter.ParamType.Dummy, paramNumber, string.Empty, string.Empty, string.Empty);
		}

		public static IncludeFileParameter CreateInstanceParam(int paramNumber, string paramName = "", string description = "")
		{
			return new IncludeFileParameter(IncludeFileParameter.ParamType.Instance, paramNumber, string.Empty, paramName, description);
		}

		public static IncludeFileParameter CreateInParam(int paramNumber, string paramName = "", string description = "")
		{
			return new IncludeFileParameter(IncludeFileParameter.ParamType.In, paramNumber, string.Empty, paramName, description);
		}

		public static IncludeFileParameter CreateOutParam(string ltlName, string paramName = "", string description = "")
		{
			return new IncludeFileParameter(IncludeFileParameter.ParamType.Out, -1, ltlName, paramName, description);
		}

		public IncludeFileParameter(IncludeFileParameter.ParamType type, int paramNumber, string ltlName, string paramName, string description)
		{
			this.Type = type;
			this.ParamNumber = paramNumber;
			this.LtlName = ltlName;
			this.SetName(paramName);
			this.Description = description;
			this.ExtendedDescription = string.Empty;
		}

		private void SetName(string paramName)
		{
			if (string.IsNullOrEmpty(paramName))
			{
				switch (this.Type)
				{
				case IncludeFileParameter.ParamType.Dummy:
					paramName = string.Empty;
					break;
				case IncludeFileParameter.ParamType.Instance:
					paramName = Resources_IncFiles.InstanceParameterDefaultName;
					break;
				case IncludeFileParameter.ParamType.In:
					paramName = string.Format(Resources_IncFiles.InParameterDefaultName, this.ParamNumber);
					break;
				case IncludeFileParameter.ParamType.Out:
					paramName = this.LtlName;
					break;
				default:
					paramName = string.Empty;
					break;
				}
			}
			this.Name = paramName;
		}
	}
}
