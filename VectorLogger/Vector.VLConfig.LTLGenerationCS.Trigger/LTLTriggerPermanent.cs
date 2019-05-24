using System;
using System.Collections.Generic;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.Data.DataAccess;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.LTLGenerationCS.Utility;

namespace Vector.VLConfig.LTLGenerationCS.Trigger
{
	internal class LTLTriggerPermanent : LTLTriggerModeGeneral
	{
		private TriggerConfiguration triggerConfig;

		private RingBufferOperatingMode rbOpMode;

		public LTLTriggerPermanent(VLProject vlProject, TriggerConfiguration triggerConfig, ILoggerSpecifics loggerSpecifics, IApplicationDatabaseManager databaseManager, string configurationFolderPath, RingBufferOperatingMode rbOpMode) : base(vlProject, loggerSpecifics, databaseManager, configurationFolderPath)
		{
			this.triggerConfig = triggerConfig;
			this.loggerSpecifics = loggerSpecifics;
			this.rbOpMode = rbOpMode;
		}

		public LTLGenerator.LTLGenerationResult GenerateLtlCode()
		{
			base.LtlSystemCode.AppendLine();
			base.LtlSystemCode.AppendFormat("Logger{0:D}PackDelay0  = On", this.triggerConfig.MemoryNr);
			base.LtlSystemCode.AppendLine();
			base.LtlCode.Append(LTLTriggerModeGeneral.GetTriggerModeHeadlineComment(string.Format("PERMANENT LONG-TERM LOGGING ON MEMORY {0:D}", this.triggerConfig.MemoryNr)));
			base.LtlCode.AppendLine(base.GetShutdownAndPowerDownDetectionCode());
			base.LtlCode.AppendLine();
			base.LtlCode.AppendLine();
			base.LtlCode.AppendFormat("STOP  {0} (LoggerAtEnd{0} DELAY = 0) OR ({1} DELAY = 10)", this.triggerConfig.MemoryNr, LTLTriggerModeGeneral.GetShutdownTriggerName());
			if (this.loggerSpecifics.DataStorage.DoCloseRingbufferAfterPowerOffInPermanentLogging)
			{
				base.LtlCode.AppendFormat(" OR ({0} DELAY=2)", LTLTriggerModeGeneral.GetPowerOffTriggerName(this.triggerConfig.MemoryNr));
			}
			base.LtlCode.AppendLine();
			base.LtlCode.AppendLine();
			AndList andList = new AndList();
			andList.Add("NOT " + LTLTriggerModeGeneral.GetShutdownTriggerName());
			if (this.rbOpMode == RingBufferOperatingMode.stopLogging)
			{
				andList.Add(string.Format("NOT FlashFull{0:D}", this.triggerConfig.MemoryNr));
			}
			IList<string> triggerConfigLocks = base.GetTriggerConfigLocks();
			foreach (string current in triggerConfigLocks)
			{
				andList.Add(string.Format("NOT {0} ", current));
			}
			base.LtlCode.AppendFormat("START {0} {1}", this.triggerConfig.MemoryNr, andList.ToLTLCode());
			base.LtlCode.AppendLine();
			base.LtlCode.AppendLine();
			return LTLGenerator.LTLGenerationResult.OK;
		}
	}
}
