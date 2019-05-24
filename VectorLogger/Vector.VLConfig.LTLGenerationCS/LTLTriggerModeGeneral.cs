using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.Data.DataAccess;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.LoggerSpecifics;

namespace Vector.VLConfig.LTLGenerationCS
{
	internal abstract class LTLTriggerModeGeneral : LTLGenericCodePart
	{
		protected VLProject vlProject;

		protected ILoggerSpecifics loggerSpecifics;

		protected IApplicationDatabaseManager databaseManager;

		protected string configurationFolderPath;

		private LoggingConfiguration LoggingConfiguration
		{
			get
			{
				return this.vlProject.ProjectRoot.LoggingConfiguration;
			}
		}

		public LTLTriggerModeGeneral(VLProject vlProject, ILoggerSpecifics loggerSpecifics, IApplicationDatabaseManager databaseManager, string configurationFolderPath)
		{
			this.vlProject = vlProject;
			this.loggerSpecifics = loggerSpecifics;
			this.databaseManager = databaseManager;
			this.configurationFolderPath = configurationFolderPath;
		}

		protected static string GetShutdownTriggerName()
		{
			return "ShutdownTrigger";
		}

		protected static string GetPowerOffTriggerName(int memoryNr)
		{
			return string.Format("PowerOffTrigger{0:D}", memoryNr);
		}

		protected static string GetTriggerModeHeadlineComment(string modeHeadline)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine();
			stringBuilder.AppendLine(LTLUtil.GetFormattedHeaderComment(modeHeadline));
			stringBuilder.AppendLine();
			return stringBuilder.ToString();
		}

		protected string GetShutdownAndPowerDownDetectionCode()
		{
			StringBuilder stringBuilder = new StringBuilder();
			string text = "CorrectShutdown";
			List<int> list = new List<int>();
			if (LTLTrigger.isPowerDownDetectionAlreadyConfigured)
			{
				return stringBuilder.ToString();
			}
			foreach (TriggerConfiguration current in this.LoggingConfiguration.TriggerConfigurations)
			{
				switch (current.TriggerMode.Value)
				{
				case TriggerMode.Permanent:
				case TriggerMode.OnOff:
					list.Add(current.MemoryNr);
					break;
				}
			}
			stringBuilder.AppendFormat("VAR {0} = FREE[1]", LTLTriggerModeGeneral.GetShutdownTriggerName());
			stringBuilder.AppendLine();
			if (this.loggerSpecifics.DataStorage.DoCloseRingbufferAfterPowerOffInPermanentLogging)
			{
				stringBuilder.AppendFormat("    {0} = FREE[1] PERSISTENT INIT=1", text);
				stringBuilder.AppendLine();
				foreach (int current2 in list)
				{
					stringBuilder.AppendFormat("    {0} = FREE[1]", LTLTriggerModeGeneral.GetPowerOffTriggerName(current2));
					stringBuilder.AppendLine();
				}
				stringBuilder.AppendLine();
			}
			stringBuilder.AppendLine("EVENT ON SYSTEM (Shutdown) BEGIN");
			stringBuilder.AppendFormat("  CALC {0} = (1)", LTLTriggerModeGeneral.GetShutdownTriggerName());
			stringBuilder.AppendLine();
			if (this.loggerSpecifics.DataStorage.DoCloseRingbufferAfterPowerOffInPermanentLogging)
			{
				stringBuilder.AppendFormat("  CALC {0} = (1)", text);
				stringBuilder.AppendLine();
			}
			stringBuilder.AppendLine("END");
			if (this.loggerSpecifics.DataStorage.DoCloseRingbufferAfterPowerOffInPermanentLogging)
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("EVENT ON SYSTEM (Startup) BEGIN");
				foreach (int current3 in list)
				{
					stringBuilder.AppendFormat("  CALC {0} = (1) WHEN (NOT {1})", LTLTriggerModeGeneral.GetPowerOffTriggerName(current3), text);
					stringBuilder.AppendLine();
				}
				stringBuilder.AppendFormat("  CALC {0} = (0)", text);
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("END");
				foreach (int current4 in list)
				{
					if (this.loggerSpecifics.DataStorage.DoCloseRingbufferAfterPowerOffInPermanentLogging)
					{
						stringBuilder.AppendFormat("EVENT ON SYSTEM (Logger{0:D}_Trigger) BEGIN", current4);
						stringBuilder.AppendLine();
						stringBuilder.AppendFormat("  CALC {0} = (0)", LTLTriggerModeGeneral.GetPowerOffTriggerName(current4));
						stringBuilder.AppendLine();
						stringBuilder.AppendLine("END");
					}
				}
			}
			LTLTrigger.isPowerDownDetectionAlreadyConfigured = true;
			return stringBuilder.ToString();
		}

		protected IList<string> GetTriggerConfigLocks()
		{
			IList<string> list = new List<string>();
			foreach (TriggerConfiguration current in this.vlProject.ProjectRoot.LoggingConfiguration.TriggerConfigurationsOfActiveMemories)
			{
				if (current.TriggerMode.Value == TriggerMode.Triggered)
				{
					if ((from recordTrigger in current.ActiveTriggers
					where recordTrigger.TriggerEffect.Value == TriggerEffect.EndMeasurement
					select recordTrigger).Any<RecordTrigger>())
					{
						list.Add(string.Format("LockTriggerConfigs{0:D}", current.MemoryNr));
					}
				}
			}
			return list;
		}
	}
}
