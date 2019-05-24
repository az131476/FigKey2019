using System;
using System.Text;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.LoggerSpecifics;

namespace Vector.VLConfig.LTLGenerationCS
{
	internal class LTLSpecialFeatures
	{
		private SpecialFeaturesConfiguration specialFeaturesConfiguration;

		private ILoggerSpecifics loggerSpecifics;

		private TriggerMode triggerMode;

		private StringBuilder ltlSpecialFeatureDateTimeLoggingCode;

		private StringBuilder ltlSpecialFeatureIgnitionKeepsLoggerAwake;

		public StringBuilder LtlSpecialFeatureDateTimeLoggingCode
		{
			get
			{
				return this.ltlSpecialFeatureDateTimeLoggingCode;
			}
			private set
			{
				this.ltlSpecialFeatureDateTimeLoggingCode = value;
			}
		}

		public StringBuilder LtlSpecialFeatureIgnitionKeepsLoggerAwake
		{
			get
			{
				return this.ltlSpecialFeatureIgnitionKeepsLoggerAwake;
			}
			private set
			{
				this.ltlSpecialFeatureIgnitionKeepsLoggerAwake = value;
			}
		}

		public LTLSpecialFeatures(SpecialFeaturesConfiguration specialFeaturesConfiguration, ILoggerSpecifics loggerSpecifics, TriggerMode triggerMode)
		{
			this.specialFeaturesConfiguration = specialFeaturesConfiguration;
			this.loggerSpecifics = loggerSpecifics;
			this.triggerMode = triggerMode;
		}

		public LTLGenerator.LTLGenerationResult GenerateSpecialFeaturesCode()
		{
			LTLGenerator.LTLGenerationResult result;
			if ((result = this.GenerateDateTimeLogging()) != LTLGenerator.LTLGenerationResult.OK)
			{
				return result;
			}
			if ((result = this.GenerateIgnitionKeepsLoggerAwake()) != LTLGenerator.LTLGenerationResult.OK)
			{
				return result;
			}
			return LTLGenerator.LTLGenerationResult.OK;
		}

		private LTLGenerator.LTLGenerationResult GenerateDateTimeLogging()
		{
			if (this.specialFeaturesConfiguration.IsLogDateTimeEnabled.Value)
			{
				this.LtlSpecialFeatureDateTimeLoggingCode = new StringBuilder();
				this.LtlSpecialFeatureDateTimeLoggingCode.AppendLine();
				this.LtlSpecialFeatureDateTimeLoggingCode.AppendLine(LTLUtil.GetFormattedHeaderComment("LOG DATE AND TIME"));
				this.LtlSpecialFeatureDateTimeLoggingCode.AppendLine();
				this.LtlSpecialFeatureDateTimeLoggingCode.AppendLine("EVENT ON CYCLE (1000) BEGIN");
				this.LtlSpecialFeatureDateTimeLoggingCode.AppendFormat("  TRANSMIT CAN{0:D} {1} 0x{2:X} \"DateTime\" [Year Month Day Hour Minute Second] LOG ONLY", this.specialFeaturesConfiguration.LogDateTimeChannel.Value, this.specialFeaturesConfiguration.LogDateTimeIsExtendedId.Value ? "XDATA" : "DATA", this.specialFeaturesConfiguration.LogDateTimeCANId.Value);
				this.LtlSpecialFeatureDateTimeLoggingCode.AppendLine();
				this.LtlSpecialFeatureDateTimeLoggingCode.AppendLine("END");
				this.LtlSpecialFeatureDateTimeLoggingCode.AppendLine();
			}
			return LTLGenerator.LTLGenerationResult.OK;
		}

		private LTLGenerator.LTLGenerationResult GenerateIgnitionKeepsLoggerAwake()
		{
			if (this.specialFeaturesConfiguration.IsIgnitionKeepsLoggerAwakeEnabled.Value)
			{
				this.LtlSpecialFeatureIgnitionKeepsLoggerAwake = new StringBuilder();
				this.LtlSpecialFeatureIgnitionKeepsLoggerAwake.AppendLine(LTLUtil.GetFormattedHeaderComment("IGNTITION KEEPS LOGGER AWAKE"));
				this.LtlSpecialFeatureIgnitionKeepsLoggerAwake.AppendLine();
				this.LtlSpecialFeatureIgnitionKeepsLoggerAwake.AppendLine("EVENT ON CYCLE (1000) BEGIN");
				this.LtlSpecialFeatureIgnitionKeepsLoggerAwake.AppendLine("  CALC SystemRequest = KeepAliveRequest WHEN (IgnitionInput)");
				this.LtlSpecialFeatureIgnitionKeepsLoggerAwake.AppendLine("END");
				this.LtlSpecialFeatureIgnitionKeepsLoggerAwake.AppendLine();
			}
			return LTLGenerator.LTLGenerationResult.OK;
		}
	}
}
