using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.Data.DataAccess;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.LoggerSpecifics;

namespace Vector.VLConfig.LTLGenerationCS
{
	internal class LTLSystemSettings
	{
		private StringBuilder ltlSystemHeader;

		private StringBuilder ltlCANChannelSettingsCode;

		private StringBuilder ltlLINChannelSettingsCode;

		private StringBuilder ltlMOST150ChannelSettingsCode;

		private StringBuilder ltlFRChannelSettingsCode;

		private StringBuilder ltlGeneralSystemSettingsCode;

		private StringBuilder ltlExternalSystemSettings;

		private VLProject vlProject;

		private ILoggerSpecifics loggerSpecifics;

		private IApplicationDatabaseManager applicationDatabaseManager;

		public LTLSystemSettings(VLProject vlProject, ILoggerSpecifics loggerSpecifics, IApplicationDatabaseManager applicationDatabaseManager)
		{
			this.vlProject = vlProject;
			this.loggerSpecifics = loggerSpecifics;
			this.applicationDatabaseManager = applicationDatabaseManager;
			this.ltlExternalSystemSettings = new StringBuilder();
		}

		public LTLGenerator.LTLGenerationResult GenerateLTLSystemSettings()
		{
			this.ltlSystemHeader = new StringBuilder();
			this.ltlSystemHeader.AppendLine(LTLUtil.GetFormattedHeaderComment("System Settings"));
			this.ltlSystemHeader.AppendLine();
			this.ltlSystemHeader.AppendLine("SYSTEM");
			LTLGenerator.LTLGenerationResult lTLGenerationResult = this.GenerateCANSystemSettings();
			if (lTLGenerationResult != LTLGenerator.LTLGenerationResult.OK)
			{
				return lTLGenerationResult;
			}
			lTLGenerationResult = this.GenerateLINSystemSettings();
			if (lTLGenerationResult != LTLGenerator.LTLGenerationResult.OK)
			{
				return lTLGenerationResult;
			}
			lTLGenerationResult = this.GenerateFlexRaySystemSettings();
			if (lTLGenerationResult != LTLGenerator.LTLGenerationResult.OK)
			{
				return lTLGenerationResult;
			}
			lTLGenerationResult = this.GenerateMOST150SystemSettings();
			if (lTLGenerationResult != LTLGenerator.LTLGenerationResult.OK)
			{
				return lTLGenerationResult;
			}
			this.ltlGeneralSystemSettingsCode = new StringBuilder();
			HardwareConfiguration hardwareConfiguration = this.vlProject.ProjectRoot.HardwareConfiguration;
			if (hardwareConfiguration.LogDataStorage.IsEnterSleepModeEnabled.Value)
			{
				this.ltlGeneralSystemSettingsCode.AppendFormat("SleepSeconds       = {0:D}", hardwareConfiguration.LogDataStorage.TimeoutToSleep.Value);
			}
			else
			{
				this.ltlGeneralSystemSettingsCode.Append("SleepSeconds       = 0");
			}
			this.ltlGeneralSystemSettingsCode.AppendLine();
			if (this.loggerSpecifics.Recording.HasFastwakeUp)
			{
				this.ltlGeneralSystemSettingsCode.AppendFormat("FastWakeup         = {0:D}", hardwareConfiguration.LogDataStorage.IsFastWakeUpEnabled.Value ? "On" : "Off");
				this.ltlGeneralSystemSettingsCode.AppendLine();
			}
			if (this.loggerSpecifics.DataStorage.HasDataCompression)
			{
				this.ltlGeneralSystemSettingsCode.AppendFormat("DataCompression    = {0:D}", hardwareConfiguration.LogDataStorage.UseDataCompression.Value ? "On" : "Off");
				this.ltlGeneralSystemSettingsCode.AppendLine();
			}
			this.ltlGeneralSystemSettingsCode.AppendLine();
			this.ltlGeneralSystemSettingsCode.AppendLine("Pause              = 0  {When changing pause to >0, take care of the start event!}");
			this.ltlGeneralSystemSettingsCode.AppendLine();
			this.ltlGeneralSystemSettingsCode.AppendFormat("BusLoadInterval    = {0:D}", LTLUtil.BusLoadInterval);
			this.ltlGeneralSystemSettingsCode.AppendLine();
			if (this.loggerSpecifics.DataStorage.HasOptimizeFileSystemSetting)
			{
				this.ltlGeneralSystemSettingsCode.AppendLine();
				this.ltlGeneralSystemSettingsCode.AppendLine("OptimizeFileSystem = On");
				this.ltlGeneralSystemSettingsCode.AppendLine();
			}
			if (this.loggerSpecifics.Recording.HasMarkerSupport)
			{
				this.ltlGeneralSystemSettingsCode.AppendLine();
				this.ltlGeneralSystemSettingsCode.AppendLine("IndexFile = On");
				this.ltlGeneralSystemSettingsCode.AppendLine();
			}
			return LTLGenerator.LTLGenerationResult.OK;
		}

		public string GetSystemSettingsLTLCode()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(this.ltlSystemHeader);
			stringBuilder.AppendLine();
			stringBuilder.Append(this.ltlCANChannelSettingsCode);
			stringBuilder.AppendLine();
			stringBuilder.Append(this.ltlLINChannelSettingsCode);
			stringBuilder.AppendLine();
			stringBuilder.Append(this.ltlFRChannelSettingsCode);
			stringBuilder.AppendLine();
			stringBuilder.Append(this.ltlMOST150ChannelSettingsCode);
			stringBuilder.AppendLine();
			stringBuilder.Append(this.ltlGeneralSystemSettingsCode);
			stringBuilder.AppendLine();
			stringBuilder.Append(this.ltlExternalSystemSettings);
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			return stringBuilder.ToString();
		}

		public void AppendExternalSpecificSystemSettings(StringBuilder externalSystemSettings)
		{
			if (externalSystemSettings.Length > 0)
			{
				this.ltlExternalSystemSettings.Append(externalSystemSettings);
				this.ltlExternalSystemSettings.AppendLine();
			}
		}

		private LTLGenerator.LTLGenerationResult GenerateCANSystemSettings()
		{
			this.ltlCANChannelSettingsCode = new StringBuilder();
			int num = this.vlProject.ProjectRoot.HardwareConfiguration.CANChannelConfiguration.CANChannels.Count<CANChannel>();
			if ((long)num != (long)((ulong)this.loggerSpecifics.CAN.NumberOfChannels))
			{
				return LTLGenerator.LTLGenerationResult.CANchannelError;
			}
			uint num2 = 1u;
			while ((ulong)num2 <= (ulong)((long)num))
			{
				CANChannel cANChannel = this.vlProject.ProjectRoot.HardwareConfiguration.CANChannelConfiguration.GetCANChannel(num2);
				if (cANChannel == null)
				{
					return LTLGenerator.LTLGenerationResult.CANchannelError;
				}
				if (cANChannel.IsActive.Value)
				{
					this.ltlCANChannelSettingsCode.AppendFormat("CAN{0:D}Timing         = {1}", num2, LTLSystemSettings.GetCANStdTimingString(cANChannel.CANChipConfiguration as CANStdChipConfiguration));
					this.ltlCANChannelSettingsCode.AppendLine();
					this.ltlCANChannelSettingsCode.AppendFormat("CAN{0:D}Output         = {1}", num2, (cANChannel.IsOutputActive.Value && this.loggerSpecifics.CAN.ChannelsWithOutputSupport.Contains(num2)) ? "On" : "Off");
					this.ltlCANChannelSettingsCode.AppendLine();
					this.ltlCANChannelSettingsCode.AppendFormat("CAN{0:D}KeepAwake      = {1}", num2, cANChannel.IsKeepAwakeActive.Value ? "On" : "Off");
					this.ltlCANChannelSettingsCode.AppendLine();
					if (this.loggerSpecifics.CAN.ChannelsWithWakeUpSupport.Contains(num2))
					{
						this.ltlCANChannelSettingsCode.AppendFormat("CAN{0:D}WakeUp         = {1}", num2, cANChannel.IsWakeUpEnabled.Value ? "On" : "Off");
						this.ltlCANChannelSettingsCode.AppendLine();
					}
				}
				else
				{
					this.ltlCANChannelSettingsCode.AppendFormat("CAN{0:D}Timing         = Off", num2);
					this.ltlCANChannelSettingsCode.AppendLine();
					this.ltlCANChannelSettingsCode.AppendFormat("CAN{0:D}Output         = Off", num2);
					this.ltlCANChannelSettingsCode.AppendLine();
					this.ltlCANChannelSettingsCode.AppendFormat("CAN{0:D}KeepAwake      = Off", num2);
					this.ltlCANChannelSettingsCode.AppendLine();
					if (this.loggerSpecifics.CAN.ChannelsWithWakeUpSupport.Contains(num2))
					{
						this.ltlCANChannelSettingsCode.AppendFormat("CAN{0:D}WakeUp         = Off", num2);
						this.ltlCANChannelSettingsCode.AppendLine();
					}
				}
				num2 += 1u;
			}
			CANChannelConfiguration chnConfig = this.vlProject.ProjectRoot.HardwareConfiguration.CANChannelConfiguration;
			List<int> activeMemoryNumbers = (from triggerConfig in this.vlProject.ProjectRoot.LoggingConfiguration.TriggerConfigurationsOfActiveMemories
			select triggerConfig.MemoryNr).ToList<int>();
			if ((from chn in chnConfig.ActiveCanChannels
			where chn.LogErrorFrames.Value
			select chn).Any<CANChannel>() && (from mem in chnConfig.LogErrorFramesOnMemories
			where mem.Value && activeMemoryNumbers.Contains(chnConfig.LogErrorFramesOnMemories.IndexOf(mem) + 1)
			select mem).Any<ValidatedProperty<bool>>())
			{
				string arg;
				if (chnConfig.LogErrorFramesOnMemories.Count<ValidatedProperty<bool>>() < 2)
				{
					arg = "On";
				}
				else if ((long)(from activeErrLogging in chnConfig.LogErrorFramesOnMemories
				where activeErrLogging.Value
				select activeErrLogging).Count<ValidatedProperty<bool>>() == (long)((ulong)this.loggerSpecifics.DataStorage.NumberOfMemories))
				{
					arg = "0x01";
				}
				else
				{
					int num3 = 0;
					for (int i = 1; i <= chnConfig.LogErrorFramesOnMemories.Count<ValidatedProperty<bool>>(); i++)
					{
						if (chnConfig.LogErrorFramesOnMemories[i - 1].Value)
						{
							num3 |= 1 << i;
						}
					}
					arg = string.Format("0x{0:X2}", num3);
				}
				this.ltlCANChannelSettingsCode.AppendFormat("LogErrorFrames     = {0}", arg);
				this.ltlCANChannelSettingsCode.AppendLine();
				int num4 = 0;
				foreach (CANChannel current in from chn in chnConfig.ActiveCanChannels
				where !chn.LogErrorFrames.Value
				select chn)
				{
					num4 |= 1 << chnConfig.CANChannels.IndexOf(current);
				}
				this.ltlCANChannelSettingsCode.AppendFormat("ErrorFrameFilter   = 0x{0:X3}", num4);
			}
			else
			{
				this.ltlCANChannelSettingsCode.AppendLine("LogErrorFrames     = Off");
			}
			this.ltlCANChannelSettingsCode.AppendLine();
			return LTLGenerator.LTLGenerationResult.OK;
		}

		private LTLGenerator.LTLGenerationResult GenerateLINSystemSettings()
		{
			this.ltlLINChannelSettingsCode = new StringBuilder();
			if (this.loggerSpecifics.LIN.NumberOfChannels == 0u)
			{
				return LTLGenerator.LTLGenerationResult.OK;
			}
			int num = this.vlProject.ProjectRoot.HardwareConfiguration.LINChannelConfiguration.LINChannels.Count<LINChannel>();
			if ((long)num != (long)((ulong)this.loggerSpecifics.LIN.NumberOfChannels))
			{
				return LTLGenerator.LTLGenerationResult.LINchannelError;
			}
			for (uint num2 = 1u; num2 <= this.loggerSpecifics.LIN.NumberOfChannels; num2 += 1u)
			{
				LINChannel lINChannel = this.vlProject.ProjectRoot.HardwareConfiguration.LINChannelConfiguration.GetLINChannel(num2);
				if (lINChannel == null)
				{
					return LTLGenerator.LTLGenerationResult.LINchannelError;
				}
				if (lINChannel.IsActive.Value)
				{
					uint num3 = lINChannel.SpeedRate.Value;
					int num4;
					string text;
					int num5;
					if (lINChannel.UseDbConfigValues.Value && LTLUtil.GetLinChipConfigFromLdfDatabase(this.vlProject.ProjectRoot.LoggingConfiguration.DatabaseConfiguration, this.applicationDatabaseManager, this.vlProject.GetProjectFolder(), num2, out num4, out text, out num5))
					{
						num3 = (uint)num4;
					}
					this.ltlLINChannelSettingsCode.AppendFormat("LIN{0:D}Baudrate       = {1}", num2, num3);
					this.ltlLINChannelSettingsCode.AppendLine();
					this.ltlLINChannelSettingsCode.AppendFormat("LIN{0:D}KeepAwake      = {1}", num2, lINChannel.IsKeepAwakeActive.Value ? "On" : "Off");
					this.ltlLINChannelSettingsCode.AppendLine();
					if (this.loggerSpecifics.LIN.ChannelsWithWakeUpSupport.Contains(num2))
					{
						this.ltlLINChannelSettingsCode.AppendFormat("LIN{0:D}WakeUp         = {1}", num2, lINChannel.IsWakeUpEnabled.Value ? "On" : "Off");
						this.ltlLINChannelSettingsCode.AppendLine();
					}
				}
				else
				{
					this.ltlLINChannelSettingsCode.AppendFormat("LIN{0:D}Baudrate       = Off", num2);
					this.ltlLINChannelSettingsCode.AppendLine();
					this.ltlLINChannelSettingsCode.AppendFormat("LIN{0:D}KeepAwake      = Off", num2);
					this.ltlLINChannelSettingsCode.AppendLine();
					if (this.loggerSpecifics.LIN.ChannelsWithWakeUpSupport.Contains(num2))
					{
						this.ltlLINChannelSettingsCode.AppendFormat("LIN{0:D}WakeUp         = Off", num2);
						this.ltlLINChannelSettingsCode.AppendLine();
					}
				}
			}
			if (this.vlProject.ProjectRoot.HardwareConfiguration.LINChannelConfiguration.IsUsingLinProbe.Value)
			{
				this.ltlLINChannelSettingsCode.AppendFormat("ExternalLIN_CAN    = {0:D}        {{ CAN channel used for LINprobe }}", this.vlProject.ProjectRoot.HardwareConfiguration.LINChannelConfiguration.CANChannelNrUsedForLinProbe.Value);
				this.ltlLINChannelSettingsCode.AppendLine();
				for (uint num6 = this.loggerSpecifics.LIN.NumberOfChannels + 1u; num6 <= this.loggerSpecifics.LIN.NumberOfChannels + this.loggerSpecifics.LIN.NumberOfLINprobeChannels; num6 += 1u)
				{
					LINprobeChannel lINprobeChannel = this.vlProject.ProjectRoot.HardwareConfiguration.LINChannelConfiguration.GetLINprobeChannel(num6);
					if (!lINprobeChannel.UseFixLINprobeBaudrate.Value)
					{
						uint num7 = lINprobeChannel.SpeedRate.Value;
						int num8;
						string text2;
						int num9;
						if (lINprobeChannel.UseDbConfigValues.Value && LTLUtil.GetLinChipConfigFromLdfDatabase(this.vlProject.ProjectRoot.LoggingConfiguration.DatabaseConfiguration, this.applicationDatabaseManager, this.vlProject.GetProjectFolder(), num6, out num8, out text2, out num9))
						{
							num7 = (uint)num8;
						}
						this.ltlLINChannelSettingsCode.AppendFormat("LIN{0:D}Baudrate       = {1}     {{ LINprobe channel baud rate configured by logger }}", num6, num7);
					}
					else
					{
						this.ltlLINChannelSettingsCode.AppendFormat("LIN{0:D}Baudrate       = 19200   {{ LINprobe channel uses fix baud rate set by LINprobe Configurator }}", num6);
					}
					this.ltlLINChannelSettingsCode.AppendLine();
					this.ltlLINChannelSettingsCode.AppendFormat("LIN{0:D}KeepAwake      = On", num6);
					this.ltlLINChannelSettingsCode.AppendLine();
				}
			}
			else
			{
				this.ltlLINChannelSettingsCode.Append("ExternalLIN_CAN    = Off          { no LINprobe configured }");
				this.ltlLINChannelSettingsCode.AppendLine();
			}
			return LTLGenerator.LTLGenerationResult.OK;
		}

		private LTLGenerator.LTLGenerationResult GenerateMOST150SystemSettings()
		{
			this.ltlMOST150ChannelSettingsCode = new StringBuilder();
			if (this.loggerSpecifics.Recording.IsMOST150Supported)
			{
				if (this.vlProject.ProjectRoot.HardwareConfiguration.MOST150ChannelConfiguration.IsChannelEnabled.Value)
				{
					byte b = 0;
					if (this.vlProject.ProjectRoot.HardwareConfiguration.MOST150ChannelConfiguration.IsLogStatusEventsEnabled.Value)
					{
						b |= 1;
						if (this.vlProject.ProjectRoot.HardwareConfiguration.MOST150ChannelConfiguration.IsAutoStatusEventRepEnabled.Value)
						{
							b |= 16;
						}
					}
					if (this.vlProject.ProjectRoot.HardwareConfiguration.MOST150ChannelConfiguration.IsLogControlMsgsEnabled.Value)
					{
						b |= 2;
					}
					if (this.vlProject.ProjectRoot.HardwareConfiguration.MOST150ChannelConfiguration.IsLogAsyncMDPEnabled.Value)
					{
						b |= 4;
					}
					if (this.vlProject.ProjectRoot.HardwareConfiguration.MOST150ChannelConfiguration.IsLogAsyncMEPEnabled.Value)
					{
						b |= 8;
					}
					this.ltlMOST150ChannelSettingsCode.AppendFormat("MOST1              = 0x{0:X2}", b);
					this.ltlMOST150ChannelSettingsCode.AppendLine();
					if (this.vlProject.ProjectRoot.HardwareConfiguration.MOST150ChannelConfiguration.IsKeepAwakeEnabled.Value)
					{
						this.ltlMOST150ChannelSettingsCode.Append("MOST1KeepAwake     = On");
					}
					else
					{
						this.ltlMOST150ChannelSettingsCode.Append("MOST1KeepAwake     = Off");
					}
					this.ltlMOST150ChannelSettingsCode.AppendLine();
				}
				else
				{
					this.ltlMOST150ChannelSettingsCode.Append("MOST1              = Off");
				}
				this.ltlMOST150ChannelSettingsCode.AppendLine();
			}
			return LTLGenerator.LTLGenerationResult.OK;
		}

		private LTLGenerator.LTLGenerationResult GenerateFlexRaySystemSettings()
		{
			this.ltlFRChannelSettingsCode = new StringBuilder();
			int num = this.vlProject.ProjectRoot.HardwareConfiguration.FlexrayChannelConfiguration.FlexrayChannels.Count<FlexrayChannel>();
			if ((long)num != (long)((ulong)this.loggerSpecifics.Flexray.NumberOfChannels))
			{
				return LTLGenerator.LTLGenerationResult.FlexRayChannelError;
			}
			if (num <= 0)
			{
				return LTLGenerator.LTLGenerationResult.OK;
			}
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = false;
			for (uint num2 = 1u; num2 <= this.loggerSpecifics.Flexray.NumberOfChannels; num2 += 1u)
			{
				FlexrayChannel flexrayChannel = this.vlProject.ProjectRoot.HardwareConfiguration.FlexrayChannelConfiguration.GetFlexrayChannel(num2);
				if (flexrayChannel == null)
				{
					return LTLGenerator.LTLGenerationResult.FlexRayChannelError;
				}
				if (flexrayChannel.IsActive.Value)
				{
					flag = true;
					stringBuilder.AppendFormat("FR{0:D}KeepAwake       = {1}", num2, flexrayChannel.IsKeepAwakeActive.Value ? "On" : "Off");
					stringBuilder.AppendLine();
				}
				else
				{
					stringBuilder.AppendFormat("FR{0:D}KeepAwake       = Off", num2);
					stringBuilder.AppendLine();
				}
			}
			this.ltlFRChannelSettingsCode.AppendFormat("FlexRay            = {0}", flag ? "On" : "Off");
			this.ltlFRChannelSettingsCode.AppendLine();
			this.ltlFRChannelSettingsCode.Append(stringBuilder);
			return LTLGenerator.LTLGenerationResult.OK;
		}

		private static string GetCANTimingString(uint speedRate)
		{
			if (speedRate <= 100000u)
			{
				if (speedRate <= 50000u)
				{
					if (speedRate == 33333u)
					{
						return "Timing33k";
					}
					if (speedRate == 50000u)
					{
						return "Timing50k";
					}
				}
				else
				{
					if (speedRate == 62500u)
					{
						return "0x4f14      {62.5 kbaud}";
					}
					if (speedRate == 83333u)
					{
						return "Timing83k";
					}
					if (speedRate == 100000u)
					{
						return "Timing100k";
					}
				}
			}
			else if (speedRate <= 500000u)
			{
				if (speedRate == 125000u)
				{
					return "Timing125k";
				}
				if (speedRate == 250000u)
				{
					return "Timing250k";
				}
				if (speedRate == 500000u)
				{
					return "Timing500k";
				}
			}
			else
			{
				if (speedRate == 666667u)
				{
					return "Timing667K";
				}
				if (speedRate == 800000u)
				{
					return "0x4016      {800 kbaud}";
				}
				if (speedRate == 1000000u)
				{
					return "Timing1M";
				}
			}
			return "Off  {" + speedRate + "}";
		}

		private static string GetCANStdTimingString(CANStdChipConfiguration chipCfg)
		{
			string str = string.Format("0x{0:X2}{1:X2}", chipCfg.BTR0.Value, chipCfg.BTR1.Value);
			return str + "    { " + chipCfg.Baudrate.ToString("N0", CultureInfo.InvariantCulture) + " Bd }";
		}
	}
}
