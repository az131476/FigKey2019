using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.LoggerSpecifics;

namespace Vector.VLConfig.LTLGenerationCS
{
	internal class LTLInterfaceMode : LTLGenericCodePart
	{
		private InterfaceModeConfiguration interfaceModeConfig;

		private ILoggerSpecifics loggerSpecifics;

		private IApplicationDatabaseManager dbManager;

		private string configurationFolderPath;

		private readonly int valueIndet = 30;

		public LTLInterfaceMode(InterfaceModeConfiguration interfaceModeConfiguration, ILoggerSpecifics loggerSpecifics, IApplicationDatabaseManager dbManager, string configurationFolderPath)
		{
			this.interfaceModeConfig = interfaceModeConfiguration;
			this.loggerSpecifics = loggerSpecifics;
			this.dbManager = dbManager;
			this.configurationFolderPath = configurationFolderPath;
		}

		public LTLGenerator.LTLGenerationResult GenerateInterfaceModeCode()
		{
			LTLGenerator.LTLGenerationResult result;
			if ((result = this.GenerateMonitoringModeCode()) != LTLGenerator.LTLGenerationResult.OK)
			{
				return result;
			}
			if ((result = this.GenerateExportList()) != LTLGenerator.LTLGenerationResult.OK)
			{
				return result;
			}
			return LTLGenerator.LTLGenerationResult.OK;
		}

		private LTLGenerator.LTLGenerationResult GenerateMonitoringModeCode()
		{
			if (this.interfaceModeConfig.UseInterfaceMode.Value)
			{
				base.LtlCode.AppendLine(LTLUtil.GetFormattedHeaderComment("INTERFACE MODE MARKER"));
				base.LtlCode.AppendLine();
				base.LtlCode.AppendLine("VAR CANoeMeasurementRunning = FREE[1] VALUETABLE = [ 0 = \"Off\" 1 = \"On\" ]");
				base.LtlCode.AppendLine("EVENT ON SYSTEM (PreCANoeMeasurement) BEGIN");
				base.LtlCode.AppendLine("  CALC CANoeMeasurementRunning = (1)");
				base.LtlCode.AppendFormat("  TRANSMIT {0} \"CANoeInterfaceStatus\" [0(7:1) CANoeMeasurementRunning Year Month Day Hour Minute Second] LOG ONLY", LTLUtil.GetIdString(BusType.Bt_CAN, this.interfaceModeConfig.MarkerChannel.Value, this.interfaceModeConfig.IsMarkerCANIdExtended.Value, this.interfaceModeConfig.MarkerCANId.Value, 0u, 1u, false, 0u));
				base.LtlCode.AppendLine();
				base.LtlCode.AppendLine("END");
				base.LtlCode.AppendLine("EVENT ON SYSTEM (PostCANoeMeasurement) BEGIN");
				base.LtlCode.AppendLine("  CALC CANoeMeasurementRunning = (0)");
				base.LtlCode.AppendFormat("  TRANSMIT {0} \"CANoeInterfaceStatus\" [0(7:1) CANoeMeasurementRunning Year Month Day Hour Minute Second] LOG ONLY", LTLUtil.GetIdString(BusType.Bt_CAN, this.interfaceModeConfig.MarkerChannel.Value, this.interfaceModeConfig.IsMarkerCANIdExtended.Value, this.interfaceModeConfig.MarkerCANId.Value, 0u, 1u, false, 0u));
				base.LtlCode.AppendLine();
				base.LtlCode.AppendLine("END");
				base.LtlCode.AppendLine();
				base.LtlSystemCode.AppendFormat("CANoeInterface     = \"{0}/{1}/{2}\"", this.interfaceModeConfig.IpAddress.Value, this.interfaceModeConfig.Port.Value, this.interfaceModeConfig.SubnetMask.Value);
				base.LtlSystemCode.AppendLine();
				ushort num = 0;
				if (this.interfaceModeConfig.IsSendPhysTxActive.Value)
				{
					num |= 1;
				}
				if (this.interfaceModeConfig.IsSendLoggedTxActive.Value)
				{
					num |= 2;
				}
				if (this.interfaceModeConfig.IsSendErrorFramesActive.Value)
				{
					num |= 4;
				}
				num |= 8;
				base.LtlSystemCode.AppendFormat("CANoeFlags         = 0x{0:X2}", num);
				base.LtlSystemCode.AppendLine();
			}
			return LTLGenerator.LTLGenerationResult.OK;
		}

		private LTLGenerator.LTLGenerationResult GenerateExportList()
		{
			if (this.interfaceModeConfig.UseSignalExport.Value)
			{
				base.LtlSystemCode.AppendFormat("EXPORTCYCLE        = {0}", this.interfaceModeConfig.ExportCycle.Value);
				base.LtlSystemCode.AppendLine();
				if (this.interfaceModeConfig.EnableAlwaysOnline.Value)
				{
					base.LtlSystemCode.AppendLine("AlwaysOnline       = On");
				}
				base.LtlSystemCode.AppendLine();
				if (this.interfaceModeConfig.ActiveWebDisplayExportSignals.Count < 1)
				{
					return LTLGenerator.LTLGenerationResult.OK;
				}
				base.LtlCode.AppendLine(LTLUtil.GetFormattedHeaderComment("SIGNAL EXPORT"));
				base.LtlCode.AppendLine();
				bool flag = false;
				StringBuilder stringBuilder = new StringBuilder();
				StringBuilder stringBuilder2 = new StringBuilder();
				if ((from WebDisplayExportSignal expSig in this.interfaceModeConfig.ActiveWebDisplayExportSignals
				where expSig.LtlSystemInformation.Value == LtlSystemInformation.LoggerFilesTotal
				select expSig).Any<WebDisplayExportSignal>())
				{
					stringBuilder.AppendLine("VAR LoggerFilesTotal               = FREE[16] UNSIGNED");
					stringBuilder2.AppendLine("CALC LoggerFilesTotal              = Logger1Files + Logger2Files");
					flag = true;
				}
				if ((from WebDisplayExportSignal expSig in this.interfaceModeConfig.ActiveWebDisplayExportSignals
				where expSig.LtlSystemInformation.Value == LtlSystemInformation.NotStopped1
				select expSig).Any<WebDisplayExportSignal>())
				{
					stringBuilder.AppendLine("VAR NotStopped1                    = FREE[1]");
					stringBuilder2.AppendLine("CALC NotStopped1                   = (NOT Stopped1)");
					flag = true;
				}
				if ((from WebDisplayExportSignal expSig in this.interfaceModeConfig.ActiveWebDisplayExportSignals
				where expSig.LtlSystemInformation.Value == LtlSystemInformation.NotStopped2
				select expSig).Any<WebDisplayExportSignal>())
				{
					stringBuilder.AppendLine("VAR NotStopped2                    = FREE[1]");
					stringBuilder2.AppendLine("CALC NotStopped2                   = (NOT Stopped2)");
					flag = true;
				}
				foreach (WebDisplayExportSignal current in this.interfaceModeConfig.ActiveWebDisplayExportSignals)
				{
					if (current.Type == WebDisplayExportSignalType.Signal)
					{
						string value;
						LTLGenerator.LTLGenerationResult lTLGenerationResult;
						if ((lTLGenerationResult = this.GenerateLtlSignalDefinition(current, out value)) != LTLGenerator.LTLGenerationResult.OK)
						{
							LTLGenerator.LTLGenerationResult result = lTLGenerationResult;
							return result;
						}
						stringBuilder.Append(value);
						flag = true;
					}
				}
				if (flag)
				{
					base.LtlCode.Append(stringBuilder.ToString());
					base.LtlCode.AppendLine();
					base.LtlCode.Append(stringBuilder2.ToString());
					base.LtlCode.AppendLine();
				}
				base.LtlCode.AppendLine("EXPORT");
				foreach (WebDisplayExportSignal current2 in this.interfaceModeConfig.ActiveWebDisplayExportSignals)
				{
					LTLGenerator.LTLGenerationResult lTLGenerationResult;
					string ltlVarName;
					if ((lTLGenerationResult = this.GetLtlVarName(current2, out ltlVarName)) != LTLGenerator.LTLGenerationResult.OK)
					{
						LTLGenerator.LTLGenerationResult result = lTLGenerationResult;
						return result;
					}
					this.AppendExportLine(current2.Name.Value, ltlVarName, current2.Comment.Value);
				}
				base.LtlCode.AppendLine();
				return LTLGenerator.LTLGenerationResult.OK;
			}
			return LTLGenerator.LTLGenerationResult.OK;
		}

		private LTLGenerator.LTLGenerationResult AppendExportLine(string exportID, string ltlVarName, string displayName)
		{
			if (string.IsNullOrEmpty(ltlVarName))
			{
				return LTLGenerator.LTLGenerationResult.Error;
			}
			displayName = displayName.Replace("\"", "\\\"");
			displayName = displayName.Replace("“", "\\“");
			displayName = displayName.Replace("„", "\\„");
			base.LtlCode.AppendFormat("{0} = ({1} \"{2}\")", exportID.PadRight(this.valueIndet + 4, ' '), ltlVarName, displayName);
			base.LtlCode.AppendLine();
			return LTLGenerator.LTLGenerationResult.OK;
		}

		private LTLGenerator.LTLGenerationResult GetLtlVarName(WebDisplayExportSignal expSig, out string ltlVarName)
		{
			ltlVarName = string.Empty;
			if (expSig.Type == WebDisplayExportSignalType.LtlSystemInformation)
			{
				switch (expSig.LtlSystemInformation.Value)
				{
				case LtlSystemInformation.Logger1Files:
					ltlVarName = "Logger1Files";
					break;
				case LtlSystemInformation.Logger2Files:
					ltlVarName = "Logger2Files";
					break;
				case LtlSystemInformation.LoggerFilesTotal:
					ltlVarName = "LoggerFilesTotal";
					break;
				case LtlSystemInformation.LoggerMBsFree:
					ltlVarName = "LoggerMBsFree";
					break;
				case LtlSystemInformation.Stopped1:
					ltlVarName = "Stopped1";
					break;
				case LtlSystemInformation.Stopped2:
					ltlVarName = "Stopped2";
					break;
				case LtlSystemInformation.NotStopped1:
					ltlVarName = "NotStopped1";
					break;
				case LtlSystemInformation.NotStopped2:
					ltlVarName = "NotStopped2";
					break;
				case LtlSystemInformation.FlashFull:
					ltlVarName = "FlashFull";
					break;
				default:
					return LTLGenerator.LTLGenerationResult.Error;
				}
			}
			else if (expSig.Type == WebDisplayExportSignalType.Signal)
			{
				ltlVarName = this.GetSignalLtlVarName(expSig);
			}
			return LTLGenerator.LTLGenerationResult.OK;
		}

		private string GetSignalLtlVarName(WebDisplayExportSignal expSig)
		{
			return string.Format("{0}_{1}", "ExportListSignal", expSig.Name.Value);
		}

		private LTLGenerator.LTLGenerationResult GenerateLtlSignalDefinition(WebDisplayExportSignal expSig, out string ltlVarDefinition)
		{
			string absolutePath = FileSystemServices.GetAbsolutePath(expSig.DatabasePath.Value, this.configurationFolderPath);
			SignalDefinition signalDefinition;
			if (!this.dbManager.ResolveSignalSymbolInDatabase(absolutePath, expSig.NetworkName.Value, expSig.MessageName.Value, expSig.SignalName.Value, out signalDefinition))
			{
				ltlVarDefinition = "";
				return LTLGenerator.LTLGenerationResult.SignalExportListError_SignalResolve;
			}
			Dictionary<double, string> textEncValueTable = null;
			if (signalDefinition.HasTextEncodedValueTable)
			{
				this.dbManager.GetTextEncodedSignalValueTable(absolutePath, expSig.NetworkName.Value, expSig.MessageName.Value, expSig.SignalName.Value, out textEncValueTable);
			}
			IList<SignalDefinition> frSignalList = new List<SignalDefinition>();
			if (expSig.BusType.Value == BusType.Bt_FlexRay && !this.dbManager.MapFlexraySignalsFromPduToFrames(absolutePath, expSig.NetworkName.Value, expSig.MessageName.Value, expSig.SignalName.Value, out frSignalList))
			{
				ltlVarDefinition = "";
				return LTLGenerator.LTLGenerationResult.SignalExportListError_SignalResolve;
			}
			string signalBaseName;
			LTLGenerator.LTLGenerationResult ltlVarName;
			if ((ltlVarName = this.GetLtlVarName(expSig, out signalBaseName)) != LTLGenerator.LTLGenerationResult.OK)
			{
				ltlVarDefinition = "";
				return ltlVarName;
			}
			string text;
			LTLUtil.GenerateLtlSignalDefinition(out ltlVarDefinition, signalDefinition, textEncValueTable, expSig.BusType.Value, expSig.ChannelNumber.Value, signalBaseName, 0L, frSignalList, true, false, out text);
			return LTLGenerator.LTLGenerationResult.OK;
		}
	}
}
