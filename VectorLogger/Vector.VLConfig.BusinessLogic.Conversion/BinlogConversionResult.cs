using System;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.GeneralUtil.GUI;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.BusinessLogic.Conversion
{
	public class BinlogConversionResult : GenericBackgroundWorkerResult
	{
		public enum EnumSpecificType
		{
			None,
			DestinationFileAlreadyExists,
			UnableToOpenSourceFile,
			UnableToOpenDestinationFile
		}

		public BinlogConversionResult.EnumSpecificType SpecificType
		{
			get;
			private set;
		}

		public override string ErrorInfo
		{
			get
			{
				switch (this.SpecificType)
				{
				case BinlogConversionResult.EnumSpecificType.None:
					return base.ErrorInfo;
				case BinlogConversionResult.EnumSpecificType.DestinationFileAlreadyExists:
					return Resources.Error + ": " + string.Format(Resources_General.GenericBackgroundWorkerResultDestinationFileAlreadyExists, base.ErrorText);
				case BinlogConversionResult.EnumSpecificType.UnableToOpenSourceFile:
					return Resources.Error + ": " + string.Format(Resources.BinlogConversionResultUnableToOpenSourceFile, base.ErrorText);
				case BinlogConversionResult.EnumSpecificType.UnableToOpenDestinationFile:
					return Resources.Error + ": " + string.Format(Resources.BinlogConversionResultUnableToOpenDestinationFile, base.ErrorText);
				default:
					return string.Format(Resources.InternalError, ": unknown error type: " + base.Type);
				}
			}
		}

		public BinlogConversionResult(GenericBackgroundWorkerResult.ResultType type, BinlogConversionResult.EnumSpecificType specificType = BinlogConversionResult.EnumSpecificType.None, string errorText = "") : base(type, errorText)
		{
			this.SpecificType = specificType;
		}
	}
}
