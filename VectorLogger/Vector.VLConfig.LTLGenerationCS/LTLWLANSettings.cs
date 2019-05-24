using System;
using System.Text;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.LoggerSpecifics;

namespace Vector.VLConfig.LTLGenerationCS
{
	internal class LTLWLANSettings : LTLGenericCodePart
	{
		private WLANConfiguration wlanConfiguration;

		private ILoggerSpecifics loggerSpecifics;

		private IApplicationDatabaseManager dbManager;

		private string configurationFolderPath;

		private bool isAtLeastOneFeatureUsed;

		public LTLWLANSettings(WLANConfiguration configuration, ILoggerSpecifics loggerSpecifics, IApplicationDatabaseManager dbManager, string configurationFolderPath)
		{
			this.wlanConfiguration = configuration;
			this.loggerSpecifics = loggerSpecifics;
			this.dbManager = dbManager;
			this.configurationFolderPath = configurationFolderPath;
			this.isAtLeastOneFeatureUsed = false;
		}

		public LTLGenerator.LTLGenerationResult GenerateWLANSettingsCode()
		{
			if (!this.loggerSpecifics.DataTransfer.HasWLAN && !this.loggerSpecifics.DataTransfer.Has3G)
			{
				return LTLGenerator.LTLGenerationResult.OK;
			}
			base.LtlCode = new StringBuilder();
			base.LtlCode.AppendLine(LTLUtil.GetFormattedHeaderComment("WLAN/3G CONNECTION"));
			base.LtlCode.AppendLine();
			LTLGenerator.LTLGenerationResult result;
			if (this.loggerSpecifics.DataTransfer.HasWLAN)
			{
				if ((result = this.GenerateAutoConnectAnaInFeature()) != LTLGenerator.LTLGenerationResult.OK)
				{
					return result;
				}
				if ((result = this.GenerateWLANConnection()) != LTLGenerator.LTLGenerationResult.OK)
				{
					return result;
				}
			}
			if (this.loggerSpecifics.DataTransfer.Has3G && (result = this.Generate3GConnection()) != LTLGenerator.LTLGenerationResult.OK)
			{
				return result;
			}
			if (!this.isAtLeastOneFeatureUsed)
			{
				base.LtlCode.AppendLine("  {no WLAN or 3G connection configured}");
				base.LtlCode.AppendLine();
			}
			else
			{
				if ((result = this.GeneratePartialDownloadCode()) != LTLGenerator.LTLGenerationResult.OK)
				{
					return result;
				}
				if (this.loggerSpecifics.DataTransfer.IsMLserverSetupInLTL && (result = this.GenerateMLserverSetupCode()) != LTLGenerator.LTLGenerationResult.OK)
				{
					return result;
				}
			}
			return LTLGenerator.LTLGenerationResult.OK;
		}

		private LTLGenerator.LTLGenerationResult GenerateAutoConnectAnaInFeature()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.wlanConfiguration.IsStartWLANor3GOnShutdownEnabled.Value)
			{
				stringBuilder.AppendFormat("AutoConnectAnaIn   = {0:D}", this.wlanConfiguration.AnalogInputNumber.Value);
				StringBuilder stringBuilder2 = new StringBuilder();
				string value;
				LTLGenerator.LTLGenerationResult dataSelectionCode;
				if ((dataSelectionCode = LTLWLANSettings.GetDataSelectionCode(this.wlanConfiguration.IsWLANor3GDownloadRingbufferEnabled.Value, this.wlanConfiguration.WLANor3GRingbuffersToDownload.Value, this.loggerSpecifics, this.wlanConfiguration.IsWLANor3GDownloadClassificationEnabled.Value, this.wlanConfiguration.IsWLANor3GDownloadDriveRecorderEnabled.Value, out value)) != LTLGenerator.LTLGenerationResult.OK)
				{
					return dataSelectionCode;
				}
				stringBuilder2.AppendLine("EVENT ON SYSTEM (Startup) BEGIN");
				stringBuilder2.AppendLine(value);
				stringBuilder2.AppendLine("END");
				base.LtlCode.Append(stringBuilder2.ToString());
				this.isAtLeastOneFeatureUsed = true;
			}
			else
			{
				stringBuilder.AppendFormat("AutoConnectAnaIn   = 0", new object[0]);
			}
			base.LtlSystemCode.AppendLine(stringBuilder.ToString());
			return LTLGenerator.LTLGenerationResult.OK;
		}

		private LTLGenerator.LTLGenerationResult GenerateWLANConnection()
		{
			if (!this.wlanConfiguration.IsStartWLANOnEventEnabled.Value)
			{
				return LTLGenerator.LTLGenerationResult.OK;
			}
			bool value = this.wlanConfiguration.IsWLANDownloadRingbufferEnabled.Value;
			uint value2 = this.wlanConfiguration.WLANRingbuffersToDownload.Value;
			bool value3 = this.wlanConfiguration.IsWLANDownloadClassificationEnabled.Value;
			bool value4 = this.wlanConfiguration.IsWLANDownloadDriveRecorderEnabled.Value;
			Event startWLANEvent = this.wlanConfiguration.StartWLANEvent;
			string arg;
			if (startWLANEvent is KeyEvent)
			{
				KeyEvent keyEvent = startWLANEvent as KeyEvent;
				if (keyEvent.IsOnPanel.Value)
				{
					arg = string.Format("PanelKey{0:D}", keyEvent.Number.Value);
				}
				else
				{
					if (keyEvent.IsCasKey)
					{
						GlobalSettings.UseAuxSwitchBox();
					}
					arg = string.Format("Key{0:D}", keyEvent.Number.Value);
				}
			}
			else
			{
				if (!(startWLANEvent is DigitalInputEvent))
				{
					return LTLGenerator.LTLGenerationResult.WLANConfigurationError;
				}
				DigitalInputEvent digitalInputEvent = startWLANEvent as DigitalInputEvent;
				arg = string.Format("DigIn{0:D}", digitalInputEvent.DigitalInput.Value);
			}
			string value5;
			LTLGenerator.LTLGenerationResult dataSelectionCode;
			if ((dataSelectionCode = LTLWLANSettings.GetDataSelectionCode(value, value2, this.loggerSpecifics, value3, value4, out value5)) != LTLGenerator.LTLGenerationResult.OK)
			{
				return dataSelectionCode;
			}
			string arg2;
			if (this.wlanConfiguration.IsWLANFallbackTo3GEnabled.Value)
			{
				arg2 = "ConnectionRequest";
			}
			else
			{
				arg2 = "ConnectionRequestWLAN";
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("EVENT ON SET ({0}) BEGIN", arg);
			stringBuilder.AppendLine();
			stringBuilder.AppendLine(value5);
			stringBuilder.AppendFormat("  CALC SystemRequest = ({0})", arg2);
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("END");
			base.LtlCode.Append(stringBuilder.ToString());
			this.isAtLeastOneFeatureUsed = true;
			return LTLGenerator.LTLGenerationResult.OK;
		}

		private LTLGenerator.LTLGenerationResult Generate3GConnection()
		{
			if (!this.wlanConfiguration.IsStartThreeGOnEventEnabled.Value)
			{
				return LTLGenerator.LTLGenerationResult.OK;
			}
			LTLActionWLAN3G lTLActionWLAN3G = new LTLActionWLAN3G(this.wlanConfiguration, this.loggerSpecifics, this.dbManager, this.configurationFolderPath);
			LTLGenerator.LTLGenerationResult lTLGenerationResult = lTLActionWLAN3G.GenerateLTLActionCode();
			if (lTLGenerationResult != LTLGenerator.LTLGenerationResult.OK)
			{
				return lTLGenerationResult;
			}
			base.LtlCode.Append(lTLActionWLAN3G.LtlCode);
			this.isAtLeastOneFeatureUsed = true;
			return LTLGenerator.LTLGenerationResult.OK;
		}

		private LTLGenerator.LTLGenerationResult GeneratePartialDownloadCode()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.wlanConfiguration.PartialDownload.Value == PartialDownloadType.PartialDownloadOn)
			{
				stringBuilder.AppendLine("PartialDownload    = On");
			}
			else if (this.wlanConfiguration.PartialDownload.Value == PartialDownloadType.PartialDownloadOnInSameFolder)
			{
				stringBuilder.AppendLine("PartialDownload    = 2");
			}
			else
			{
				stringBuilder.AppendLine("PartialDownload    = Off");
			}
			base.LtlSystemCode.AppendLine(stringBuilder.ToString());
			return LTLGenerator.LTLGenerationResult.OK;
		}

		private LTLGenerator.LTLGenerationResult GenerateMLserverSetupCode()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.wlanConfiguration.IsStartThreeGOnEventEnabled.Value)
			{
				stringBuilder.AppendLine(string.Format("MLserverSetup      = \"{0}/{1}/{2}/{3}\"", new object[]
				{
					this.wlanConfiguration.LoggerIp.Value,
					this.wlanConfiguration.MLserverIp.Value,
					this.wlanConfiguration.GatewayIp.Value,
					this.wlanConfiguration.SubnetMask.Value
				}));
			}
			base.LtlSystemCode.AppendLine(stringBuilder.ToString());
			return LTLGenerator.LTLGenerationResult.OK;
		}

		public static LTLGenerator.LTLGenerationResult GetDataSelectionCode(bool isDataSelectedRingbuffers, uint dataSelectionRingbuffers, ILoggerSpecifics loggerSpecifics, bool isDataSelectedClassify, bool isDataSelectedDriveRecorder, out string ltlCodeSnippet)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (isDataSelectedRingbuffers)
			{
				if (dataSelectionRingbuffers == 2147483647u)
				{
					stringBuilder.AppendLine("  CALC NoLogger1Download  = (Off)");
					if (loggerSpecifics.DataStorage.NumberOfMemories > 1u)
					{
						stringBuilder.AppendLine("       NoLogger2Download  = (Off)");
					}
				}
				else if (dataSelectionRingbuffers == 1u)
				{
					stringBuilder.AppendLine("  CALC NoLogger1Download  = (Off)");
					if (loggerSpecifics.DataStorage.NumberOfMemories > 1u)
					{
						stringBuilder.AppendLine("       NoLogger2Download  = (On)");
					}
				}
				else
				{
					if (dataSelectionRingbuffers != 2u)
					{
						ltlCodeSnippet = string.Empty;
						return LTLGenerator.LTLGenerationResult.WLANConfigurationError;
					}
					stringBuilder.AppendLine("  CALC NoLogger1Download  = (On)");
					stringBuilder.AppendLine("       NoLogger2Download  = (Off)");
				}
			}
			else
			{
				stringBuilder.AppendLine("  CALC NoLogger1Download  = (On)");
				if (loggerSpecifics.DataStorage.NumberOfMemories > 1u)
				{
					stringBuilder.AppendLine("       NoLogger2Download  = (On)");
				}
			}
			if (isDataSelectedClassify)
			{
				stringBuilder.AppendLine("       NoClassDownload    = (Off)");
			}
			else
			{
				stringBuilder.AppendLine("       NoClassDownload    = (On)");
			}
			if (isDataSelectedDriveRecorder)
			{
				stringBuilder.AppendLine("       NoDriverecDownload = (Off)");
			}
			else
			{
				stringBuilder.AppendLine("       NoDriverecDownload = (On)");
			}
			ltlCodeSnippet = stringBuilder.ToString();
			return LTLGenerator.LTLGenerationResult.OK;
		}
	}
}
