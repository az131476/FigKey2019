using System;
using System.Text;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.LTLGenerationCS.LTLEvents;

namespace Vector.VLConfig.LTLGenerationCS
{
	internal class LTLCommon : LTLGenericCodePart
	{
		private static readonly string Name_VehicleSleepIndicationFlag = "VehicleSleepIndication";

		private LogDataStorage logDataStorageConfig;

		private ILoggerSpecifics loggerSpecifics;

		private IApplicationDatabaseManager databaseManager;

		private string configurationFolderPath;

		public LTLCommon(LogDataStorage logDataStorage, ILoggerSpecifics loggerSpecifics, IApplicationDatabaseManager databaseManager, string configurationFolderPath)
		{
			this.logDataStorageConfig = logDataStorage;
			this.loggerSpecifics = loggerSpecifics;
			this.databaseManager = databaseManager;
			this.configurationFolderPath = configurationFolderPath;
		}

		public LTLGenerator.LTLGenerationResult GenerateCommonLTLCode()
		{
			string empty = string.Empty;
			base.LtlCode = new StringBuilder();
			new StringBuilder();
			base.LtlCode.AppendLine();
			base.LtlCode.AppendLine(LTLUtil.GetFormattedHeaderComment("Common LTL Code"));
			base.LtlCode.AppendLine();
			string value;
			LTLGenerator.LTLGenerationResult lTLGenerationResult = this.GenerateErrorframeRateCode(out value);
			if (lTLGenerationResult != LTLGenerator.LTLGenerationResult.OK)
			{
				return lTLGenerationResult;
			}
			base.LtlCode.Append(value);
			base.LtlCode.AppendLine();
			string value2;
			if ((lTLGenerationResult = this.GenerateStartTimeDalay(out value2)) != LTLGenerator.LTLGenerationResult.OK)
			{
				return lTLGenerationResult;
			}
			base.LtlCode.Append(value2);
			base.LtlCode.AppendLine();
			base.LtlCode.AppendLine();
			string value3;
			if ((lTLGenerationResult = this.GenerateVehicleSleepIndicationFlag(out value3, out empty)) != LTLGenerator.LTLGenerationResult.OK)
			{
				return lTLGenerationResult;
			}
			base.LtlCode.Append(value3);
			GlobalSettings.VehicleSleepIndicationFlag = empty;
			return LTLGenerator.LTLGenerationResult.OK;
		}

		private LTLGenerator.LTLGenerationResult GenerateErrorframeRateCode(out string ltlcode)
		{
			StringBuilder stringBuilder = new StringBuilder();
			StringBuilder stringBuilder2 = new StringBuilder();
			stringBuilder.AppendFormat("EVENT ON CYCLE ({0:D}) BEGIN", LTLUtil.BusLoadInterval);
			stringBuilder.AppendLine();
			for (uint num = 1u; num <= this.loggerSpecifics.CAN.NumberOfChannels; num += 1u)
			{
				stringBuilder2.AppendFormat("{0} {1} = FREE[16]", (num == 1u) ? "VAR" : "   ", LTLUtil.GetCANErrorrateVariableName(num));
				stringBuilder2.AppendLine();
				stringBuilder.AppendFormat("  CALC {0} = (CAN{1:D}Errors)", LTLUtil.GetCANErrorrateVariableName(num), num);
				stringBuilder.AppendLine();
				stringBuilder.AppendFormat("  CALC CAN{0:D}Errors    = (0)", num);
				stringBuilder.AppendLine();
			}
			stringBuilder.AppendLine("END");
			stringBuilder2.AppendLine();
			ltlcode = stringBuilder2.ToString() + stringBuilder.ToString();
			return LTLGenerator.LTLGenerationResult.OK;
		}

		private LTLGenerator.LTLGenerationResult GenerateStartTimeDalay(out string ltlCode)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("TIMER {0} TIME = {1:D} (1)", LTLUtil.StartTimeDelayFlag, this.logDataStorageConfig.EventActivationDelayAfterStart.Value);
			ltlCode = stringBuilder.ToString();
			return LTLGenerator.LTLGenerationResult.OK;
		}

		private LTLGenerator.LTLGenerationResult GenerateVehicleSleepIndicationFlag(out string ltlCode, out string vehicleSleepIndicationFlag)
		{
			ltlCode = string.Empty;
			vehicleSleepIndicationFlag = string.Empty;
			Event stopCyclicCommunicationEvent = this.logDataStorageConfig.StopCyclicCommunicationEvent;
			if (stopCyclicCommunicationEvent == null)
			{
				return LTLGenerator.LTLGenerationResult.OK;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(LTLUtil.GetFormattedSubsectionHeaderComment("Vehicle Sleep Indication"));
			stringBuilder.AppendFormat("VAR {0} = FREE[1] {{dummy}}", LTLCommon.Name_VehicleSleepIndicationFlag);
			stringBuilder.AppendLine();
			LTLGenericEventCode lTLGenericEventCode;
			if (!LTLEventFactory.GetLtlEventCodeObject(stopCyclicCommunicationEvent, LTLCommon.Name_VehicleSleepIndicationFlag, this.databaseManager, this.configurationFolderPath, out lTLGenericEventCode))
			{
				return LTLGenerator.LTLGenerationResult.StopCyclicEventsError;
			}
			string value;
			lTLGenericEventCode.GenerateCode(out value);
			stringBuilder.AppendLine(value);
			if (lTLGenericEventCode.GetCurrentStateFlag(out vehicleSleepIndicationFlag) != LTLGenerator.LTLGenerationResult.OK)
			{
				return LTLGenerator.LTLGenerationResult.StopCyclicEventsError;
			}
			ltlCode = stringBuilder.ToString();
			return LTLGenerator.LTLGenerationResult.OK;
		}
	}
}
