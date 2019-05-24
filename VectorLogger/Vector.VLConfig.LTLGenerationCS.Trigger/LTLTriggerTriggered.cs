using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.Data.DataAccess;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.LoggerSpecifics;

namespace Vector.VLConfig.LTLGenerationCS.Trigger
{
	internal class LTLTriggerTriggered : LTLTriggerModeGeneral
	{
		public LTLTriggerTriggered(VLProject vlProject, ILoggerSpecifics loggerSpecifics, IApplicationDatabaseManager databaseManager, string configurationFolderPath) : base(vlProject, loggerSpecifics, databaseManager, configurationFolderPath)
		{
			this.loggerSpecifics = loggerSpecifics;
			this.databaseManager = databaseManager;
			this.configurationFolderPath = configurationFolderPath;
		}

		public LTLGenerator.LTLGenerationResult GenerateLTLCode(TriggerConfiguration triggerConfig, bool useInterfaceMode)
		{
			LTLGenerator.LTLGenerationResult lTLGenerationResult = this.BuildTriggerConfigurationList(triggerConfig, useInterfaceMode);
			if (lTLGenerationResult != LTLGenerator.LTLGenerationResult.OK)
			{
				return lTLGenerationResult;
			}
			base.LtlCode.AppendLine();
			return LTLGenerator.LTLGenerationResult.OK;
		}

		private LTLGenerator.LTLGenerationResult BuildTriggerConfigurationList(TriggerConfiguration triggerConfig, bool useInterfaceMode)
		{
			new StringBuilder();
			if (triggerConfig.TriggerMode.Value != TriggerMode.Triggered)
			{
				return LTLGenerator.LTLGenerationResult.TriggerError;
			}
			IList<RecordTrigger> triggers = triggerConfig.Triggers;
			int memoryNr = triggerConfig.MemoryNr;
			int num = triggers.Count<RecordTrigger>();
			if (num <= 0)
			{
				return LTLGenerator.LTLGenerationResult.OK;
			}
			LTLActionTrigger lTLActionTrigger = new LTLActionTrigger(triggerConfig, this.loggerSpecifics, this.databaseManager, this.configurationFolderPath, useInterfaceMode, triggerConfig.MemoryRingBuffer.OperatingMode.Value);
			LTLGenerator.LTLGenerationResult result;
			if ((result = lTLActionTrigger.GenerateLTLActionCode()) != LTLGenerator.LTLGenerationResult.OK)
			{
				return result;
			}
			base.LtlCode.Append(lTLActionTrigger.LtlCode);
			RingBufferOperatingMode value = triggerConfig.MemoryRingBuffer.OperatingMode.Value;
			if (value == RingBufferOperatingMode.stopLogging)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat("EVENT ON SET (FlashFull{0}) BEGIN", memoryNr);
				stringBuilder.AppendLine();
				stringBuilder.AppendFormat("  CALC LockTriggerConfigs{0:D} = (1)", memoryNr);
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("END");
				base.LtlCode.Append(stringBuilder);
			}
			return LTLGenerator.LTLGenerationResult.OK;
		}
	}
}
