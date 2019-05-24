using System;
using Vector.VLConfig.GUI;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.BusinessLogic
{
	public class FileConversionProfileError
	{
		public EnumViewType ViewType
		{
			get;
			private set;
		}

		public EnumProfileError Error
		{
			get;
			private set;
		}

		public string TagName
		{
			get;
			private set;
		}

		public string Text
		{
			get;
			private set;
		}

		public bool IsError
		{
			get
			{
				return this.Error == EnumProfileError.FileDoesNotExist || this.Error == EnumProfileError.InvalidFileContent;
			}
		}

		public bool IsWarning
		{
			get
			{
				return !this.IsError && !this.IsInfo;
			}
		}

		public bool IsInfo
		{
			get
			{
				return this.Error == EnumProfileError.ViewTypeMissmatch || this.Error == EnumProfileError.LoggerTypeMissmatch;
			}
		}

		public bool IsVolatile
		{
			get
			{
				return this.Error == EnumProfileError.MarkerRestoration || this.Error == EnumProfileError.TriggerRestoration;
			}
		}

		public FileConversionProfileError(EnumViewType viewType, EnumProfileError error, string tagName)
		{
			this.ViewType = viewType;
			this.Error = error;
			this.TagName = tagName;
			this.Text = this.GetText();
		}

		private string GetText()
		{
			switch (this.Error)
			{
			case EnumProfileError.FileDoesNotExist:
				return Resources.FileConversionProfileErrorFileDoesNotExist;
			case EnumProfileError.InvalidFileContent:
				return Resources.FileConversionProfileErrorInvalidFileContent;
			case EnumProfileError.TagMissing:
				return string.Format(Resources.FileConversionProfileErrorTagMissing, this.TagName);
			case EnumProfileError.ArrayValueMissing:
				return string.Format(Resources.FileConversionProfileErrorArrayValueMissing, this.TagName);
			case EnumProfileError.InvalidValue:
				return string.Format(Resources.FileConversionProfileErrorInvalidValue, this.TagName);
			case EnumProfileError.MarkerRestoration:
				return Resources.FileConversionProfileErrorMarkerRestoration;
			case EnumProfileError.TriggerRestoration:
				return Resources.FileConversionProfileErrorTriggerRestoration;
			case EnumProfileError.ViewTypeMissmatch:
				return string.Format(Resources.FileConversionProfileErrorViewTypeMissmatch, this.TagName);
			case EnumProfileError.LoggerTypeMissmatch:
				return string.Format(Resources.FileConversionProfileErrorLoggerTypeMissmatch, this.TagName);
			default:
				return string.Empty;
			}
		}
	}
}
