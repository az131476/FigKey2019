using System;
using System.Text;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.Data.DataAccess;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.LTLGenerationCS.LTLEvents;
using Vector.VLConfig.LTLGenerationCS.Trigger;

namespace Vector.VLConfig.LTLGenerationCS
{
	internal class LTLTrigger : LTLGenericCodePart
	{
		private VLProject vlProject;

		private ILoggerSpecifics loggerSpecifics;

		private IApplicationDatabaseManager databaseManager;

		private LTLSystemSettings ltlSystemSettings;

		private string configurationFolderPath;

		internal static bool isPowerDownDetectionAlreadyConfigured;

		private LoggingConfiguration LoggingConfiguration
		{
			get
			{
				return this.vlProject.ProjectRoot.LoggingConfiguration;
			}
		}

		public LTLTrigger(VLProject vlProject, ILoggerSpecifics loggerSpecifics, IApplicationDatabaseManager databaseManager, LTLSystemSettings ltlSystemSettings, string configurationFolderPath)
		{
			this.vlProject = vlProject;
			this.loggerSpecifics = loggerSpecifics;
			this.databaseManager = databaseManager;
			this.ltlSystemSettings = ltlSystemSettings;
			this.configurationFolderPath = configurationFolderPath;
			LTLTrigger.isPowerDownDetectionAlreadyConfigured = false;
		}

		public LTLGenerator.LTLGenerationResult GenerateLTLTriggerCode()
		{
			foreach (TriggerConfiguration current in this.LoggingConfiguration.TriggerConfigurations)
			{
				uint num;
				bool flag;
				if (current.MemoryRingBuffer.IsActive.Value)
				{
					num = current.MemoryRingBuffer.Size.Value;
					flag = this.loggerSpecifics.DataStorage.IsUsingStoreRAM;
				}
				else
				{
					num = 50u;
					flag = false;
				}
				uint num2 = current.MemoryRingBuffer.MaxNumberOfFiles.Value;
				if (num2 <= 1u)
				{
					num2 = 2u;
				}
				base.LtlSystemCode.AppendFormat("Logger{0:D}Size        = {1:D}", current.MemoryNr, num);
				base.LtlSystemCode.AppendLine();
				base.LtlSystemCode.AppendFormat("Logger{0:D}Files       = {1:D}", current.MemoryNr, num2);
				base.LtlSystemCode.AppendLine();
				base.LtlSystemCode.AppendFormat("Logger{0:D}StoreRAM    = {1}", current.MemoryNr, flag ? "On" : "Off");
				base.LtlSystemCode.AppendLine();
			}
			base.LtlSystemCode.AppendLine();
			base.LtlSystemCode.AppendFormat("StandardDelay      = {0:D}", this.vlProject.ProjectRoot.LoggingConfiguration.TriggerConfigurationsOfActiveMemories[0].PostTriggerTime.Value);
			base.LtlSystemCode.AppendLine();
			base.LtlSystemCode.AppendLine("StandardPreTrigger = 0");
			base.LtlCode = new StringBuilder();
			LTLGenerator.LTLGenerationResult result = LTLGenerator.LTLGenerationResult.TriggerError;
			foreach (TriggerConfiguration current2 in this.LoggingConfiguration.TriggerConfigurationsOfActiveMemories)
			{
				LTLCombinedEventCode.StaticClear();
				switch (current2.TriggerMode.Value)
				{
				case TriggerMode.Triggered:
				{
					LTLTriggerTriggered lTLTriggerTriggered = new LTLTriggerTriggered(this.vlProject, this.loggerSpecifics, this.databaseManager, this.configurationFolderPath);
					result = lTLTriggerTriggered.GenerateLTLCode(current2, this.vlProject.ProjectRoot.HardwareConfiguration.InterfaceModeConfiguration.UseInterfaceMode.Value);
					base.LtlCode.Append(lTLTriggerTriggered.LtlCode);
					base.LtlSystemCode.Append(lTLTriggerTriggered.LtlSystemCode);
					break;
				}
				case TriggerMode.Permanent:
				{
					LTLTriggerPermanent lTLTriggerPermanent = new LTLTriggerPermanent(this.vlProject, current2, this.loggerSpecifics, this.databaseManager, this.configurationFolderPath, current2.MemoryRingBuffer.OperatingMode.Value);
					result = lTLTriggerPermanent.GenerateLtlCode();
					base.LtlSystemCode.Append(lTLTriggerPermanent.LtlSystemCode);
					base.LtlCode.Append(lTLTriggerPermanent.LtlCode);
					break;
				}
				case TriggerMode.OnOff:
				{
					LTLTriggerConditioned lTLTriggerConditioned = new LTLTriggerConditioned(this.vlProject, this.loggerSpecifics, this.databaseManager, this.configurationFolderPath);
					result = lTLTriggerConditioned.GenerateLtlCode(current2);
					base.LtlCode.Append(lTLTriggerConditioned.LtlCode);
					base.LtlSystemCode.Append(lTLTriggerConditioned.LtlSystemCode);
					break;
				}
				}
			}
			return result;
		}

		private static string GetPowerOffTriggerName(int memoryNr)
		{
			return string.Format("PowerOffTrigger{0:D}", memoryNr);
		}
	}
}
