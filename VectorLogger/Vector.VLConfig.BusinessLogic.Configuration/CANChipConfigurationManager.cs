using System;
using System.Collections.Generic;
using System.Linq;
using Vector.VLConfig.ChipCfgWrapper;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.BusinessLogic.Configuration
{
	internal static class CANChipConfigurationManager
	{
		private static Dictionary<uint, CANStdChipConfiguration> _predefinedCANStdBaudrates;

		private static Dictionary<uint, CANStdChipConfiguration> _predefinedCANStdBaudratesMinPrescaler;

		private static Dictionary<uint, CANFDChipConfiguration> _predefinedCANFDArbBaudrates;

		private static Dictionary<uint, CANFDChipConfiguration> _predefinedCANFDDataBaudrates;

		private static Dictionary<uint, ICANChipConfiguration> _userdefinedSettingOnChannels;

		private static ChipCfgServices _chipCfgServies;

		private static ILoggerSpecifics _loggerSpecifics;

		static CANChipConfigurationManager()
		{
			CANChipConfigurationManager._predefinedCANStdBaudrates = new Dictionary<uint, CANStdChipConfiguration>();
			CANChipConfigurationManager._predefinedCANStdBaudratesMinPrescaler = new Dictionary<uint, CANStdChipConfiguration>();
			CANChipConfigurationManager._predefinedCANFDArbBaudrates = new Dictionary<uint, CANFDChipConfiguration>();
			CANChipConfigurationManager._predefinedCANFDDataBaudrates = new Dictionary<uint, CANFDChipConfiguration>();
			CANChipConfigurationManager._userdefinedSettingOnChannels = new Dictionary<uint, ICANChipConfiguration>();
			CANChipConfigurationManager._chipCfgServies = new ChipCfgServices();
			CANChipConfigurationManager._loggerSpecifics = null;
			CANChipConfigurationManager.InitializePredefinedSettings();
		}

		public static void InitializePredefinedSettings()
		{
			CANChipConfigurationManager._predefinedCANStdBaudrates.Clear();
			CANChipConfigurationManager._predefinedCANStdBaudratesMinPrescaler.Clear();
			CANChipConfigurationManager._predefinedCANFDArbBaudrates.Clear();
			CANChipConfigurationManager._predefinedCANFDDataBaudrates.Clear();
			long quartzFreqVal = 16000L;
			CANStdChipConfiguration value = new CANStdChipConfiguration(201u, 111u, quartzFreqVal);
			CANChipConfigurationManager._predefinedCANStdBaudratesMinPrescaler.Add(33333u, value);
			value = new CANStdChipConfiguration(7u, 77u, quartzFreqVal);
			CANChipConfigurationManager._predefinedCANStdBaudratesMinPrescaler.Add(50000u, value);
			value = new CANStdChipConfiguration(7u, 58u, quartzFreqVal);
			CANChipConfigurationManager._predefinedCANStdBaudratesMinPrescaler.Add(62500u, value);
			value = new CANStdChipConfiguration(5u, 58u, quartzFreqVal);
			CANChipConfigurationManager._predefinedCANStdBaudratesMinPrescaler.Add(83333u, value);
			value = new CANStdChipConfiguration(3u, 77u, quartzFreqVal);
			CANChipConfigurationManager._predefinedCANStdBaudratesMinPrescaler.Add(100000u, value);
			value = new CANStdChipConfiguration(3u, 58u, quartzFreqVal);
			CANChipConfigurationManager._predefinedCANStdBaudratesMinPrescaler.Add(125000u, value);
			value = new CANStdChipConfiguration(1u, 58u, quartzFreqVal);
			CANChipConfigurationManager._predefinedCANStdBaudratesMinPrescaler.Add(250000u, value);
			value = new CANStdChipConfiguration(0u, 58u, quartzFreqVal);
			CANChipConfigurationManager._predefinedCANStdBaudratesMinPrescaler.Add(500000u, value);
			value = new CANStdChipConfiguration(0u, 39u, quartzFreqVal);
			CANChipConfigurationManager._predefinedCANStdBaudratesMinPrescaler.Add(666666u, value);
			value = new CANStdChipConfiguration(64u, 22u, quartzFreqVal);
			CANChipConfigurationManager._predefinedCANStdBaudratesMinPrescaler.Add(800000u, value);
			value = new CANStdChipConfiguration(64u, 20u, quartzFreqVal);
			CANChipConfigurationManager._predefinedCANStdBaudratesMinPrescaler.Add(1000000u, value);
			foreach (uint current in GUIUtil.GetStandardCANBaudrates())
			{
				CANStdChipConfiguration cANStdChipConfiguration = CANChipConfigurationManager.CreateCANStdChipConfigurationForBaudrate(current);
				CANChipConfigurationManager._predefinedCANStdBaudrates.Add(cANStdChipConfiguration.Baudrate, cANStdChipConfiguration);
				CANChipConfigurationManager._predefinedCANStdBaudratesMinPrescaler.ContainsKey(cANStdChipConfiguration.Baudrate);
			}
			uint arbBaudrate = GUIUtil.GetStandardCANFDArbBaudrates()[0];
			uint dataBaudrate = GUIUtil.GetStandardCANFDDataBaudrates()[0];
			foreach (uint current2 in GUIUtil.GetStandardCANFDArbBaudrates())
			{
				CANFDChipConfiguration cANFDChipConfiguration = CANChipConfigurationManager.CreateCANFDChipConfigurationForBaudrate(current2, dataBaudrate);
				CANChipConfigurationManager._predefinedCANFDArbBaudrates.Add(cANFDChipConfiguration.Baudrate, cANFDChipConfiguration);
			}
			foreach (uint current3 in GUIUtil.GetStandardCANFDDataBaudrates())
			{
				CANFDChipConfiguration cANFDChipConfiguration2 = CANChipConfigurationManager.CreateCANFDChipConfigurationForBaudrate(arbBaudrate, current3);
				CANChipConfigurationManager._predefinedCANFDDataBaudrates.Add(cANFDChipConfiguration2.DataBaudrate, cANFDChipConfiguration2);
			}
		}

		public static void Reset(ILoggerSpecifics loggerSpecifics)
		{
			CANChipConfigurationManager._userdefinedSettingOnChannels.Clear();
			CANChipConfigurationManager._loggerSpecifics = loggerSpecifics;
		}

		public static bool IsEqualAnyPredefinedSetting(uint channelNr, CANStdChipConfiguration userdefChipCfg)
		{
			CANStdChipConfiguration obj;
			return CANChipConfigurationManager.GetPredefinedCANStdSetting(channelNr, userdefChipCfg.Baudrate, out obj) && userdefChipCfg.Equals(obj);
		}

		public static bool IsEqualPredefinedSettingForBaudrate(uint channelNr, uint baudrate, CANStdChipConfiguration chipCfg)
		{
			CANStdChipConfiguration cANStdChipConfiguration;
			return CANChipConfigurationManager.GetPredefinedCANStdSetting(channelNr, baudrate, out cANStdChipConfiguration) && cANStdChipConfiguration.Equals(chipCfg);
		}

		public static bool IsEqualAnyPredefinedFDArbSetting(CANFDChipConfiguration userdefChipCfg)
		{
			foreach (CANFDChipConfiguration current in CANChipConfigurationManager._predefinedCANFDArbBaudrates.Values)
			{
				if (userdefChipCfg.EqualsArbSettings(current))
				{
					return true;
				}
			}
			return false;
		}

		public static bool IsEqualPredefinedSettingForFDArbBaudrate(uint baudrate, CANFDChipConfiguration chipCfg)
		{
			return CANChipConfigurationManager._predefinedCANFDArbBaudrates.ContainsKey(baudrate) && CANChipConfigurationManager._predefinedCANFDArbBaudrates[baudrate].EqualsArbSettings(chipCfg);
		}

		public static bool IsEqualAnyPredefinedFDDataSetting(CANFDChipConfiguration userdefChipCfg)
		{
			foreach (CANFDChipConfiguration current in CANChipConfigurationManager._predefinedCANFDDataBaudrates.Values)
			{
				if (userdefChipCfg.EqualsDataSettings(current))
				{
					return true;
				}
			}
			return false;
		}

		public static bool IsEqualPredefinedSettingForFDDataBaudrate(uint baudrate, CANFDChipConfiguration chipCfg)
		{
			return CANChipConfigurationManager._predefinedCANFDDataBaudrates.ContainsKey(baudrate) && CANChipConfigurationManager._predefinedCANFDDataBaudrates[baudrate].EqualsDataSettings(chipCfg);
		}

		public static bool ApplyPredefinedSettingForBaudrate(uint channelNr, uint baudrate, ref CANStdChipConfiguration chipCfg)
		{
			CANStdChipConfiguration other;
			if (CANChipConfigurationManager.GetPredefinedCANStdSetting(channelNr, baudrate, out other))
			{
				chipCfg.Assign(other);
				return true;
			}
			return false;
		}

		public static bool ApplyPredefinedSettingForFDArbBaudrate(uint baudrate, ref CANFDChipConfiguration chipCfg)
		{
			if (!CANChipConfigurationManager._predefinedCANFDArbBaudrates.Keys.Contains(baudrate))
			{
				return false;
			}
			chipCfg.AssignArbSettings(CANChipConfigurationManager._predefinedCANFDArbBaudrates[baudrate]);
			return true;
		}

		public static bool ApplyPredefinedSettingForFDDataBaudrate(uint baudrate, ref CANFDChipConfiguration chipCfg)
		{
			if (!CANChipConfigurationManager._predefinedCANFDDataBaudrates.Keys.Contains(baudrate))
			{
				return false;
			}
			chipCfg.AssignDataSettings(CANChipConfigurationManager._predefinedCANFDDataBaudrates[baudrate]);
			return true;
		}

		public static bool IsEqualUserdefinedSetting(uint channelNr, CANStdChipConfiguration chipCfg)
		{
			return CANChipConfigurationManager._userdefinedSettingOnChannels.ContainsKey(channelNr) && CANChipConfigurationManager._userdefinedSettingOnChannels[channelNr] is CANStdChipConfiguration && chipCfg.Equals(CANChipConfigurationManager._userdefinedSettingOnChannels[channelNr] as CANStdChipConfiguration);
		}

		public static bool IsEqualUserdefinedFDArbSetting(uint channelNr, CANFDChipConfiguration chipCfg)
		{
			return CANChipConfigurationManager._userdefinedSettingOnChannels.ContainsKey(channelNr) && CANChipConfigurationManager._userdefinedSettingOnChannels[channelNr] is CANFDChipConfiguration && chipCfg.EqualsArbSettings(CANChipConfigurationManager._userdefinedSettingOnChannels[channelNr] as CANFDChipConfiguration);
		}

		public static bool IsEqualUserdefinedFDDataSetting(uint channelNr, CANFDChipConfiguration chipCfg)
		{
			return CANChipConfigurationManager._userdefinedSettingOnChannels.ContainsKey(channelNr) && CANChipConfigurationManager._userdefinedSettingOnChannels[channelNr] is CANFDChipConfiguration && chipCfg.EqualsDataSettings(CANChipConfigurationManager._userdefinedSettingOnChannels[channelNr] as CANFDChipConfiguration);
		}

		public static bool StoreUserdefinedSetting(uint channelNr, CANStdChipConfiguration chipCfg)
		{
			if (CANChipConfigurationManager.IsEqualAnyPredefinedSetting(channelNr, chipCfg))
			{
				return false;
			}
			CANChipConfigurationManager._userdefinedSettingOnChannels[channelNr] = new CANStdChipConfiguration(chipCfg);
			return true;
		}

		public static bool StoreUserdefinedFDArbSetting(uint channelNr, CANFDChipConfiguration chipCfg)
		{
			if (CANChipConfigurationManager.IsEqualAnyPredefinedFDArbSetting(chipCfg))
			{
				return false;
			}
			if (CANChipConfigurationManager._userdefinedSettingOnChannels.Keys.Contains(channelNr) && CANChipConfigurationManager._userdefinedSettingOnChannels[channelNr] is CANFDChipConfiguration)
			{
				(CANChipConfigurationManager._userdefinedSettingOnChannels[channelNr] as CANFDChipConfiguration).AssignArbSettings(chipCfg);
				return true;
			}
			CANChipConfigurationManager._userdefinedSettingOnChannels[channelNr] = new CANFDChipConfiguration(chipCfg);
			return true;
		}

		public static bool StoreUserdefinedFDDataSetting(uint channelNr, CANFDChipConfiguration chipCfg)
		{
			if (CANChipConfigurationManager.IsEqualAnyPredefinedFDDataSetting(chipCfg))
			{
				return false;
			}
			if (CANChipConfigurationManager._userdefinedSettingOnChannels.Keys.Contains(channelNr) && CANChipConfigurationManager._userdefinedSettingOnChannels[channelNr] is CANFDChipConfiguration)
			{
				(CANChipConfigurationManager._userdefinedSettingOnChannels[channelNr] as CANFDChipConfiguration).AssignDataSettings(chipCfg);
				return true;
			}
			CANChipConfigurationManager._userdefinedSettingOnChannels[channelNr] = new CANFDChipConfiguration(chipCfg);
			return true;
		}

		public static void DeleteUserdefinedSetting(uint channelNr)
		{
			if (CANChipConfigurationManager._userdefinedSettingOnChannels.Keys.Contains(channelNr))
			{
				CANChipConfigurationManager._userdefinedSettingOnChannels.Remove(channelNr);
			}
		}

		public static bool GetUserdefinedBaudrate(uint channelNr, out uint baudrate)
		{
			baudrate = 0u;
			if (CANChipConfigurationManager._userdefinedSettingOnChannels.Keys.Contains(channelNr) && CANChipConfigurationManager._userdefinedSettingOnChannels[channelNr] is CANStdChipConfiguration)
			{
				CANStdChipConfiguration cANStdChipConfiguration = CANChipConfigurationManager._userdefinedSettingOnChannels[channelNr] as CANStdChipConfiguration;
				if (!CANChipConfigurationManager.IsEqualAnyPredefinedSetting(channelNr, cANStdChipConfiguration))
				{
					baudrate = cANStdChipConfiguration.Baudrate;
					return true;
				}
			}
			return false;
		}

		public static bool GetUserdefinedFDArbBaudrate(uint channelNr, out uint baudrate)
		{
			baudrate = 0u;
			if (CANChipConfigurationManager._userdefinedSettingOnChannels.Keys.Contains(channelNr) && CANChipConfigurationManager._userdefinedSettingOnChannels[channelNr] is CANFDChipConfiguration)
			{
				CANFDChipConfiguration cANFDChipConfiguration = CANChipConfigurationManager._userdefinedSettingOnChannels[channelNr] as CANFDChipConfiguration;
				if (!CANChipConfigurationManager.IsEqualAnyPredefinedFDArbSetting(cANFDChipConfiguration))
				{
					baudrate = cANFDChipConfiguration.Baudrate;
					return true;
				}
			}
			return false;
		}

		public static bool GetUserdefinedFDDataBaudrate(uint channelNr, out uint baudrate)
		{
			baudrate = 0u;
			if (CANChipConfigurationManager._userdefinedSettingOnChannels.Keys.Contains(channelNr) && CANChipConfigurationManager._userdefinedSettingOnChannels[channelNr] is CANFDChipConfiguration)
			{
				CANFDChipConfiguration cANFDChipConfiguration = CANChipConfigurationManager._userdefinedSettingOnChannels[channelNr] as CANFDChipConfiguration;
				if (!CANChipConfigurationManager.IsEqualAnyPredefinedFDDataSetting(cANFDChipConfiguration))
				{
					baudrate = cANFDChipConfiguration.DataBaudrate;
					return true;
				}
			}
			return false;
		}

		public static bool EditUserdefinedCANStdSetting(uint channelNr, CANStdChipConfiguration chipCfgToStartFrom, out uint predefBaudrateToSelect)
		{
			predefBaudrateToSelect = 0u;
			CANStdChipConfiguration chipCfg = new CANStdChipConfiguration(chipCfgToStartFrom);
			return CANChipConfigurationManager.EditAndCompareCANStdUserdefinedSetting(channelNr, chipCfg, out predefBaudrateToSelect);
		}

		public static bool EditUserdefinedCANFDSetting(uint channelNr, CANFDChipConfiguration chipCfgToStartFrom, out uint predefArbBaudrateToSelect, out uint predefDataBaudrateToSelect)
		{
			predefArbBaudrateToSelect = 0u;
			predefDataBaudrateToSelect = 0u;
			CANFDChipConfiguration chipCfg = new CANFDChipConfiguration(chipCfgToStartFrom);
			return CANChipConfigurationManager.EditAndCompareCANFDUserdefinedSetting(channelNr, chipCfg, out predefArbBaudrateToSelect, out predefDataBaudrateToSelect);
		}

		public static bool HasUserdefinedSetting(uint channelNr)
		{
			return CANChipConfigurationManager._userdefinedSettingOnChannels.Keys.Contains(channelNr);
		}

		public static bool ApplyUserdefinedSetting(uint channelNr, ref CANStdChipConfiguration chipCfg)
		{
			if (!CANChipConfigurationManager._userdefinedSettingOnChannels.Keys.Contains(channelNr))
			{
				return false;
			}
			if (CANChipConfigurationManager._userdefinedSettingOnChannels[channelNr] is CANStdChipConfiguration)
			{
				chipCfg.Assign(CANChipConfigurationManager._userdefinedSettingOnChannels[channelNr] as CANStdChipConfiguration);
				return true;
			}
			return false;
		}

		public static bool ApplyUserdefinedFDArbSetting(uint channelNr, ref CANFDChipConfiguration chipCfg)
		{
			if (!CANChipConfigurationManager._userdefinedSettingOnChannels.Keys.Contains(channelNr))
			{
				return false;
			}
			if (CANChipConfigurationManager._userdefinedSettingOnChannels[channelNr] is CANFDChipConfiguration)
			{
				chipCfg.AssignArbSettings(CANChipConfigurationManager._userdefinedSettingOnChannels[channelNr] as CANFDChipConfiguration);
				return true;
			}
			return false;
		}

		public static bool ApplyUserdefinedFDDataSetting(uint channelNr, ref CANFDChipConfiguration chipCfg)
		{
			if (!CANChipConfigurationManager._userdefinedSettingOnChannels.Keys.Contains(channelNr))
			{
				return false;
			}
			if (CANChipConfigurationManager._userdefinedSettingOnChannels[channelNr] is CANFDChipConfiguration)
			{
				chipCfg.AssignDataSettings(CANChipConfigurationManager._userdefinedSettingOnChannels[channelNr] as CANFDChipConfiguration);
				return true;
			}
			return false;
		}

		private static bool GetPredefinedCANStdSetting(uint channelNr, uint predefinedBaudrate, out CANStdChipConfiguration chipCfg)
		{
			uint num = 0u;
			if (CANChipConfigurationManager._loggerSpecifics != null)
			{
				if (CANChipConfigurationManager._loggerSpecifics.CAN.AuxChannel == channelNr)
				{
					if (CANChipConfigurationManager._loggerSpecifics.CAN.AuxChannelMaxPrescalerValue > 0u)
					{
						num = CANChipConfigurationManager._loggerSpecifics.CAN.AuxChannelMaxPrescalerValue;
					}
				}
				else if (CANChipConfigurationManager._loggerSpecifics.CAN.MaxPrescalerValue > 0u)
				{
					num = CANChipConfigurationManager._loggerSpecifics.CAN.MaxPrescalerValue;
				}
			}
			if (num > 0u)
			{
				if (CANChipConfigurationManager._predefinedCANStdBaudrates.ContainsKey(predefinedBaudrate))
				{
					if (CANChipConfigurationManager._predefinedCANStdBaudrates[predefinedBaudrate].Prescaler.Value > num)
					{
						chipCfg = CANChipConfigurationManager._predefinedCANStdBaudratesMinPrescaler[predefinedBaudrate];
						return true;
					}
					chipCfg = CANChipConfigurationManager._predefinedCANStdBaudrates[predefinedBaudrate];
					return true;
				}
			}
			else if (CANChipConfigurationManager._predefinedCANStdBaudrates.ContainsKey(predefinedBaudrate))
			{
				chipCfg = CANChipConfigurationManager._predefinedCANStdBaudrates[predefinedBaudrate];
				return true;
			}
			chipCfg = null;
			return false;
		}

		private static bool EditAndCompareCANStdUserdefinedSetting(uint channelNr, CANStdChipConfiguration chipCfg, out uint predefBaudrateToSelect)
		{
			predefBaudrateToSelect = 0u;
			if (!CANChipConfigurationManager.EditCANStdChipConfiguration(channelNr, ref chipCfg))
			{
				return false;
			}
			if (CANChipConfigurationManager.IsEqualAnyPredefinedSetting(channelNr, chipCfg))
			{
				predefBaudrateToSelect = chipCfg.Baudrate;
				return false;
			}
			CANChipConfigurationManager._userdefinedSettingOnChannels[channelNr] = chipCfg;
			return true;
		}

		private static bool EditAndCompareCANFDUserdefinedSetting(uint channelNr, CANFDChipConfiguration chipCfg, out uint predefArbBaudrateToSelect, out uint predefDataBaudrateToSelect)
		{
			predefArbBaudrateToSelect = 0u;
			predefDataBaudrateToSelect = 0u;
			if (!CANChipConfigurationManager.EditCANFDChipConfiguration(channelNr, ref chipCfg))
			{
				return false;
			}
			bool flag = CANChipConfigurationManager.IsEqualAnyPredefinedFDArbSetting(chipCfg);
			bool flag2 = CANChipConfigurationManager.IsEqualAnyPredefinedFDDataSetting(chipCfg);
			bool flag3 = false;
			if (CANChipConfigurationManager._userdefinedSettingOnChannels.ContainsKey(channelNr) && CANChipConfigurationManager._userdefinedSettingOnChannels[channelNr] is CANFDChipConfiguration)
			{
				flag3 = true;
			}
			if (flag && flag2)
			{
				predefArbBaudrateToSelect = chipCfg.Baudrate;
				predefDataBaudrateToSelect = chipCfg.DataBaudrate;
				return false;
			}
			if (flag)
			{
				predefArbBaudrateToSelect = chipCfg.Baudrate;
				if (flag3)
				{
					(CANChipConfigurationManager._userdefinedSettingOnChannels[channelNr] as CANFDChipConfiguration).AssignDataSettings(chipCfg);
				}
				else
				{
					CANChipConfigurationManager._userdefinedSettingOnChannels[channelNr] = chipCfg;
				}
			}
			else if (flag2)
			{
				predefDataBaudrateToSelect = chipCfg.DataBaudrate;
				if (flag3)
				{
					(CANChipConfigurationManager._userdefinedSettingOnChannels[channelNr] as CANFDChipConfiguration).AssignArbSettings(chipCfg);
				}
				else
				{
					CANChipConfigurationManager._userdefinedSettingOnChannels[channelNr] = chipCfg;
				}
			}
			else
			{
				CANChipConfigurationManager._userdefinedSettingOnChannels[channelNr] = chipCfg;
			}
			return true;
		}

		private static bool EditCANStdChipConfiguration(uint channelNr, ref CANStdChipConfiguration chipCfg)
		{
			ChannelSettings channelSettings = new ChannelSettings();
			channelSettings.isCANFD = false;
			channelSettings.channel = 1;
			channelSettings.BTR0 = (int)chipCfg.BTR0.Value;
			channelSettings.BTR1 = (int)chipCfg.BTR1.Value;
			channelSettings.quartzFreq = (int)chipCfg.QuartzFreq.Value;
			channelSettings.dialogTitle = string.Format(Resources.ChipCfgChnCANHwConfig, channelNr);
			CANHardwareCallbackReceiver receiver = new CANHardwareCallbackReceiver(1000u);
			ChipCfgDialogControl chipCfgDialogControl = new ChipCfgDialogControl(ProgramUtils.GetCurrCultureLanguageCode(), receiver);
			chipCfgDialogControl.InitializeChannelSettings(channelSettings);
			if (chipCfgDialogControl.ShowDialog())
			{
				ChannelSettings channelSettings2 = new ChannelSettings();
				chipCfgDialogControl.GetChannelSettings(channelSettings2);
				CANStdChipConfiguration other = new CANStdChipConfiguration((uint)channelSettings2.BTR0, (uint)channelSettings2.BTR1, (long)((ulong)channelSettings2.quartzFreq));
				chipCfg.Assign(other);
				chipCfgDialogControl.Dispose();
				return true;
			}
			chipCfgDialogControl.Dispose();
			return false;
		}

		private static bool EditCANFDChipConfiguration(uint channelNr, ref CANFDChipConfiguration chipCfg)
		{
			ChannelSettings channelSettings = new ChannelSettings();
			channelSettings.isCANFD = true;
			channelSettings.channel = 1;
			channelSettings.tseg1 = (int)chipCfg.TSeg1.Value;
			channelSettings.tseg2 = (int)chipCfg.TSeg2.Value;
			channelSettings.prescaler = (int)chipCfg.Prescaler.Value;
			channelSettings.sjw = (int)chipCfg.SJW.Value;
			channelSettings.dataTseg1 = (int)chipCfg.DataTSeg1.Value;
			channelSettings.dataTseg2 = (int)chipCfg.DataTSeg2.Value;
			channelSettings.dataPrescaler = (int)chipCfg.DataPrescaler.Value;
			channelSettings.dataSJW = (int)chipCfg.DataSJW.Value;
			channelSettings.dialogTitle = string.Format(Resources.ChipCfgChnCANFDHwConfig, channelNr);
			CANHardwareCallbackReceiver receiver = new CANHardwareCallbackReceiver(1000u);
			ChipCfgDialogControl chipCfgDialogControl = new ChipCfgDialogControl(ProgramUtils.GetCurrCultureLanguageCode(), receiver);
			chipCfgDialogControl.InitializeChannelSettings(channelSettings);
			if (chipCfgDialogControl.ShowDialog())
			{
				ChannelSettings channelSettings2 = new ChannelSettings();
				chipCfgDialogControl.GetChannelSettings(channelSettings2);
				chipCfg.TSeg1.Value = (uint)channelSettings2.tseg1;
				chipCfg.TSeg2.Value = (uint)channelSettings2.tseg2;
				chipCfg.Prescaler.Value = (uint)channelSettings2.prescaler;
				chipCfg.SJW.Value = (uint)channelSettings2.sjw;
				chipCfg.DataTSeg1.Value = (uint)channelSettings2.dataTseg1;
				chipCfg.DataTSeg2.Value = (uint)channelSettings2.dataTseg2;
				chipCfg.DataPrescaler.Value = (uint)channelSettings2.dataPrescaler;
				chipCfg.DataSJW.Value = (uint)channelSettings2.dataSJW;
				chipCfgDialogControl.Dispose();
				return true;
			}
			chipCfgDialogControl.Dispose();
			return false;
		}

		private static CANStdChipConfiguration CreateCANStdChipConfigurationForBaudrate(uint baudrate)
		{
			ChannelSettings channelSettings = new ChannelSettings();
			double baudrate2 = baudrate;
			if (CANChipConfigurationManager._chipCfgServies.GetCANStdChipSettingsForBaudrate(baudrate2, channelSettings))
			{
				return new CANStdChipConfiguration((uint)channelSettings.BTR0, (uint)channelSettings.BTR1, (long)((ulong)channelSettings.quartzFreq));
			}
			return null;
		}

		private static CANFDChipConfiguration CreateCANFDChipConfigurationForBaudrate(uint arbBaudrate, uint dataBaudrate)
		{
			ChannelSettings channelSettings = new ChannelSettings();
			double arbBaudrate2 = arbBaudrate;
			double dataBaudrate2 = dataBaudrate;
			if (CANChipConfigurationManager._chipCfgServies.GetCANFDChipSettingsForBaudrates(arbBaudrate2, dataBaudrate2, channelSettings))
			{
				return new CANFDChipConfiguration((uint)channelSettings.tseg1, (uint)channelSettings.tseg2, (uint)channelSettings.sjw, (uint)channelSettings.prescaler, (uint)channelSettings.dataTseg1, (uint)channelSettings.dataTseg2, (uint)channelSettings.dataSJW, (uint)channelSettings.dataPrescaler, (long)channelSettings.quartzFreq);
			}
			return null;
		}
	}
}
