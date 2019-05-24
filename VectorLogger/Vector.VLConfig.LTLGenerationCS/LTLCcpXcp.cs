using System;
using System.IO;
using System.Linq;
using System.Text;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.Data.DataAccess;

namespace Vector.VLConfig.LTLGenerationCS
{
	internal class LTLCcpXcp : LTLGenericCodePart
	{
		private DatabaseConfiguration databaseConfiguration;

		private EthernetConfiguration ethernetConfiguration;

		private CcpXcpSignalConfiguration ccpXcpSignalConfiguration;

		private CcpXcpGenerationInfo ccpXcpGenerationInfo;

		private bool usesDisconnectCode;

		public LTLCcpXcp(VLProject vlProject, CcpXcpGenerationInfo ccpXcpGenerationInfo)
		{
			this.databaseConfiguration = vlProject.ProjectRoot.LoggingConfiguration.DatabaseConfiguration;
			this.ethernetConfiguration = vlProject.ProjectRoot.HardwareConfiguration.EthernetConfiguration;
			this.ccpXcpSignalConfiguration = vlProject.ProjectRoot.LoggingConfiguration.CcpXcpSignalConfiguration;
			this.ccpXcpGenerationInfo = ccpXcpGenerationInfo;
			this.usesDisconnectCode = vlProject.HasStopCyclicCommunicationEventConfigured();
		}

		public LTLGenerator.LTLGenerationResult GenerateCcpXcpCode()
		{
			if (this.databaseConfiguration == null)
			{
				return LTLGenerator.LTLGenerationResult.CCPXCPError;
			}
			if (this.ccpXcpSignalConfiguration == null)
			{
				return LTLGenerator.LTLGenerationResult.OK;
			}
			if (this.databaseConfiguration.ActiveCCPXCPDatabases.Any<Database>())
			{
				base.LtlCode = new StringBuilder();
				base.LtlCode.AppendLine();
				base.LtlCode.AppendLine(LTLUtil.GetFormattedHeaderComment("CCP/XCP Configuration"));
				base.LtlCode.AppendLine();
				bool flag = false;
				foreach (CcpXcpDatabaseInfo current in this.ccpXcpGenerationInfo.DatabaseInfos)
				{
					if (current.IncludeFileInfo != null)
					{
						string fileName = Path.GetFileName(current.IncludeFileInfo.IncFilePath);
						if (current.BusType == BusType.Bt_Ethernet)
						{
							flag = true;
						}
						string text;
						if (GlobalSettings.VehicleSleepIndicationFlag.Length > 0)
						{
							text = "NOT " + GlobalSettings.VehicleSleepIndicationFlag;
						}
						else
						{
							text = "1";
						}
						if (this.usesDisconnectCode)
						{
							base.LtlCode.AppendFormat(" #include(\"{0}\" \"{1}\" \"{2}\" \"{3}\")", new object[]
							{
								fileName,
								LTLUtil.GetChannelString(current.BusType, current.ChannelNumber),
								current.IncludeFileInfo.Prefix,
								text
							});
						}
						else
						{
							base.LtlCode.AppendFormat(" #include(\"{0}\" \"{1}\" \"{2}\")", fileName, LTLUtil.GetChannelString(current.BusType, current.ChannelNumber), current.IncludeFileInfo.Prefix);
						}
						base.LtlCode.AppendLine();
					}
				}
				base.LtlCode.AppendLine();
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("LogCROmessages     = On");
				stringBuilder.AppendFormat("CCPExchangeID      = {0}", this.databaseConfiguration.EnableExchangeIdHandling.Value ? 2 : 0);
				stringBuilder.AppendLine();
				if (flag)
				{
					stringBuilder.AppendFormat("Eth1IP                = {0}", this.ethernetConfiguration.Eth1Ip.Value);
					stringBuilder.AppendLine();
					stringBuilder.AppendFormat("ETH1keepAwake         = {0}", this.ethernetConfiguration.Eth1KeepAwake.Value ? 1 : 0);
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("GET_DAQ_CLOCK_Seconds = 5");
				}
				base.LtlSystemCode.Append(stringBuilder);
			}
			return LTLGenerator.LTLGenerationResult.OK;
		}
	}
}
