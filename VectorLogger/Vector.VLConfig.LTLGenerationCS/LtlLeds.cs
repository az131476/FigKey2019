using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.Data.DataAccess;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.LTLGenerationCS.Utility;

namespace Vector.VLConfig.LTLGenerationCS
{
	internal class LtlLeds : LTLGenericCodePart
	{
		private static readonly uint ConstDurationForLEDonTrigger = 1u;

		private static readonly string ltlvarCANErrorsDetected = "CANerrorsDetected";

		private static readonly string ltlvarLinErrorsDetected = "LINerrorsDetected";

		private static readonly string ltlFlagCANLinErrorsDetected = "CANLINerrorsDetected";

		private static readonly string ltlTimerStartOver = "TStartOver";

		private VLProject mVlProject;

		private ILoggerSpecifics mLoggerSpecifics;

		private LEDConfiguration mLedConfig;

		private CcpXcpGenerationInfo ccpXcpGenerationInfo;

		private StringBuilder mLtlLEDCodeBeforeOutputSection = new StringBuilder();

		private bool mLedOnTriggerAlreadyExists;

		private bool mAllCANsErrorsAlreadyExists;

		private bool mAllLinsErrorsAlreadyExists;

		private bool mStartOverTimerAlreadyExists;

		public LtlLeds(VLProject vlProject, ILoggerSpecifics loggerSpecifics, CcpXcpGenerationInfo ccpXcpGenerationInfo)
		{
			this.mVlProject = vlProject;
			this.mLoggerSpecifics = loggerSpecifics;
			this.mLedConfig = this.mVlProject.ProjectRoot.OutputConfiguration.LEDConfiguration;
			this.ccpXcpGenerationInfo = ccpXcpGenerationInfo;
			this.mLedOnTriggerAlreadyExists = false;
			this.mAllCANsErrorsAlreadyExists = false;
			this.mAllLinsErrorsAlreadyExists = false;
			this.mStartOverTimerAlreadyExists = false;
		}

		public void GenerateLtlLedCode()
		{
			base.LtlCode = new StringBuilder();
			StringBuilder stringBuilder = new StringBuilder();
			this.mLtlLEDCodeBeforeOutputSection = new StringBuilder();
			base.LtlCode.AppendLine();
			base.LtlCode.AppendLine(LTLUtil.GetFormattedHeaderComment("LEDS"));
			base.LtlCode.AppendLine();
			stringBuilder.AppendLine("OUTPUT");
			uint numberOfLEDs = this.mLedConfig.NumberOfLEDs;
			if (numberOfLEDs != this.mLoggerSpecifics.IO.NumberOfLEDsTotal)
			{
				throw new LTLGenerationException(LTLGenerator.LTLGenerationResult.LEDError);
			}
			uint numberOfLEDsOnBoard = this.mLoggerSpecifics.IO.NumberOfLEDsOnBoard;
			uint arg_A0_0 = this.mLoggerSpecifics.IO.NumberOfLEDsTotal;
			uint arg_B1_0 = this.mLoggerSpecifics.IO.NumberOfLEDsOnBoard;
			for (uint num = 1u; num <= numberOfLEDs; num += 1u)
			{
				LEDState value = this.mLedConfig.LEDConfigList[(int)(num - 1u)].State.Value;
				uint num2 = num;
				if (num > numberOfLEDsOnBoard)
				{
					num2 = 6u + (num - numberOfLEDsOnBoard) - 1u;
				}
				try
				{
					switch (value)
					{
					case LEDState.Disabled:
						stringBuilder.AppendFormat("{{ LED{0:D} Off}}", num2);
						break;
					case LEDState.AlwaysOn:
						stringBuilder.AppendFormat("  LED{0:D} = (On)", num2);
						break;
					case LEDState.AlwaysBlinking:
						stringBuilder.AppendFormat("  LED{0:D} = (_10msec < 50)", num2);
						break;
					case LEDState.TriggerActive:
					{
						uint memoryNumberFromParameter = this.GetMemoryNumberFromParameter(num);
						string arg = this.GenerateTriggerActiveLed(memoryNumberFromParameter);
						stringBuilder.AppendFormat("  LED{0:D} = {1}", num2, arg);
						break;
					}
					case LEDState.LoggingActive:
					{
						uint memoryNumberFromParameter2 = this.GetMemoryNumberFromParameter(num);
						string arg2 = this.GenerateLedOnLoggingActive(memoryNumberFromParameter2);
						stringBuilder.AppendFormat("  LED{0:D} = {1}", num2, arg2);
						break;
					}
					case LEDState.EndOfMeasurement:
					{
						uint memoryNumberFromParameter3 = this.GetMemoryNumberFromParameter(num);
						string arg3 = this.GenerateEndOfMeasurementLed(memoryNumberFromParameter3);
						stringBuilder.AppendFormat("  LED{0:D} = {1}", num2, arg3);
						break;
					}
					case LEDState.MemoryFull:
					{
						uint memoryNumberFromParameter4 = this.GetMemoryNumberFromParameter(num);
						string arg4 = this.GenerateLedOnMemoryFull(memoryNumberFromParameter4);
						stringBuilder.AppendFormat("  LED{0:D} = {1}", num2, arg4);
						break;
					}
					case LEDState.CANLINError:
					{
						string arg5 = this.GenerateCANLinError();
						stringBuilder.AppendFormat("  LED{0:D} = {1}", num2, arg5);
						break;
					}
					case LEDState.CANError:
					{
						uint channelNumberFromParameter = this.GetChannelNumberFromParameter(num);
						string arg6 = this.GenerateCANError(num2, channelNumberFromParameter);
						stringBuilder.AppendFormat("  LED{0:D} = {1}", num2, arg6);
						break;
					}
					case LEDState.LINError:
					{
						uint channelNumberFromParameter2 = this.GetChannelNumberFromParameter(num);
						string arg7 = this.GenerateLinErrorCondition(num2, channelNumberFromParameter2);
						stringBuilder.AppendFormat("  LED{0:D} = {1}", num2, arg7);
						break;
					}
					case LEDState.CANoeMeasurementOn:
						stringBuilder.AppendFormat("  LED{0:D} = (CANoeMeasurement)", num2);
						break;
					case LEDState.MOSTLock:
						if (this.mVlProject.ProjectRoot.HardwareConfiguration.MOST150ChannelConfiguration.IsChannelEnabled.Value && this.mLoggerSpecifics.Recording.IsMOST150Supported)
						{
							stringBuilder.AppendFormat("  LED{0:D} = (MOST1busStatus = 0x01) OR (MOST1busStatus = 0x10)", num2);
						}
						else
						{
							stringBuilder.AppendFormat("  LED{0:D} = (Off)  {{LED on MOST lock is always off, because MOST150 is not activated}}", num2);
						}
						break;
					case LEDState.CcpXcpError:
						if (this.ccpXcpGenerationInfo.DatabaseInfos.Any<CcpXcpDatabaseInfo>())
						{
							StringBuilder stringBuilder2 = new StringBuilder();
							foreach (CcpXcpDatabaseInfo current in this.ccpXcpGenerationInfo.DatabaseInfos)
							{
								if (current.IncludeFileInfo != null)
								{
									foreach (string current2 in current.IncludeFileInfo.TimeoutVariableNameList)
									{
										if (stringBuilder2.Length > 0)
										{
											stringBuilder2.Append(" OR ");
										}
										stringBuilder2.AppendFormat("({0} < 0)", current2.Replace("%2%", current.IncludeFileInfo.Prefix));
									}
								}
							}
							stringBuilder.AppendFormat("  LED{0:D} = ", num2);
							stringBuilder.Append(stringBuilder2);
						}
						else
						{
							stringBuilder.AppendFormat("  LED{0:D} = (Off)  {{LED on CCP/XCP error is always off, because no CCP/XCP is configured}}", num2);
						}
						break;
					case LEDState.CcpXcpOk:
						if (this.ccpXcpGenerationInfo.DatabaseInfos.Any<CcpXcpDatabaseInfo>())
						{
							StringBuilder stringBuilder3 = new StringBuilder();
							foreach (CcpXcpDatabaseInfo current3 in this.ccpXcpGenerationInfo.DatabaseInfos)
							{
								if (current3.IncludeFileInfo != null)
								{
									foreach (string current4 in current3.IncludeFileInfo.OkayFlagNameList)
									{
										if (stringBuilder3.Length <= 0)
										{
											stringBuilder3.Append("(");
										}
										else
										{
											stringBuilder3.Append(" AND ");
										}
										stringBuilder3.AppendFormat("{0}", current4.Replace("%2%", current3.IncludeFileInfo.Prefix));
									}
								}
							}
							if (stringBuilder3.Length > 0)
							{
								stringBuilder3.Append(")");
							}
							stringBuilder.AppendFormat("  LED{0:D} = ", num2);
							stringBuilder.Append(stringBuilder3);
						}
						else
						{
							stringBuilder.AppendFormat("  LED{0:D} = (Off)  {{LED on CCP/XCP OK is always off, because no CCP/XCP is configured}}", num2);
						}
						break;
					default:
						stringBuilder.AppendFormat("  {{ no setting for LED{0:D} }}", num2);
						stringBuilder.AppendLine();
						throw new LTLGenerationException(LTLGenerator.LTLGenerationResult.LEDError, string.Format("LED{0:D}: {1} unknown", num, value));
					}
					stringBuilder.AppendLine();
					if (value != LEDState.Disabled && num2 >= 8u && num2 <= 11u)
					{
						GlobalSettings.UseAuxSwitchBox();
					}
				}
				catch (LTLGenerationException ex)
				{
					ex.InsertAdditionalInfo(string.Format("LED{0:D}: {1}", num, value));
					throw ex;
				}
			}
			base.LtlCode.Append(this.mLtlLEDCodeBeforeOutputSection);
			base.LtlCode.Append(stringBuilder);
		}

		private string GenerateLedOnMemoryFull(uint memoryNo)
		{
			string result;
			if (memoryNo == 2147483647u)
			{
				AndList andList = new AndList();
				int num = 1;
				while ((long)num <= (long)((ulong)this.mLoggerSpecifics.DataStorage.NumberOfMemories))
				{
					andList.Add(string.Format("FlashFull{0:D}", num));
					num++;
				}
				result = andList.ToLTLCode();
			}
			else if (memoryNo == 2147483646u)
			{
				OrList orList = new OrList();
				int num2 = 1;
				while ((long)num2 <= (long)((ulong)this.mLoggerSpecifics.DataStorage.NumberOfMemories))
				{
					orList.Add(string.Format("FlashFull{0:D}", num2));
					num2++;
				}
				result = orList.ToLTLCode();
			}
			else
			{
				result = string.Format("(FlashFull{0:D})", memoryNo);
			}
			return result;
		}

		private string GenerateEndOfMeasurementLed(uint memoryNo)
		{
			IList<TriggerConfiguration> triggerConfigurationsOfActiveMemories = this.mVlProject.ProjectRoot.LoggingConfiguration.TriggerConfigurationsOfActiveMemories;
			string result;
			if (!triggerConfigurationsOfActiveMemories.Any<TriggerConfiguration>())
			{
				result = "(On)  {no active memory}";
			}
			else if (memoryNo == 2147483647u)
			{
				AndList andList = new AndList();
				foreach (TriggerConfiguration current in triggerConfigurationsOfActiveMemories)
				{
					andList.Add(string.Format("Stopped{0:D}", current.MemoryNr));
				}
				result = andList.ToLTLCode();
			}
			else if (memoryNo == 2147483646u)
			{
				OrList orList = new OrList();
				foreach (TriggerConfiguration current2 in triggerConfigurationsOfActiveMemories)
				{
					orList.Add(string.Format("Stopped{0:D}", current2.MemoryNr));
				}
				result = orList.ToLTLCode();
			}
			else
			{
				result = string.Format("(Stopped{0:D})", memoryNo);
			}
			return result;
		}

		private string GenerateLedOnLoggingActive(uint memoryNo)
		{
			IList<TriggerConfiguration> triggerConfigurationsOfActiveMemories = this.mVlProject.ProjectRoot.LoggingConfiguration.TriggerConfigurationsOfActiveMemories;
			string result;
			if (!triggerConfigurationsOfActiveMemories.Any<TriggerConfiguration>())
			{
				result = "(Off)  {no active memory}";
			}
			else if (memoryNo == 2147483647u)
			{
				AndList andList = new AndList();
				foreach (TriggerConfiguration current in triggerConfigurationsOfActiveMemories)
				{
					andList.Add(string.Format("NOT Stopped{0:D}", current.MemoryNr));
				}
				result = andList.ToLTLCode();
			}
			else if (memoryNo == 2147483646u)
			{
				OrList orList = new OrList();
				foreach (TriggerConfiguration current2 in triggerConfigurationsOfActiveMemories)
				{
					orList.Add(string.Format("NOT Stopped{0:D}", current2.MemoryNr));
				}
				result = orList.ToLTLCode();
			}
			else
			{
				List<TriggerConfiguration> source = (from tc in triggerConfigurationsOfActiveMemories
				where (long)tc.MemoryNr == (long)((ulong)memoryNo)
				select tc).ToList<TriggerConfiguration>();
				if (source.Count<TriggerConfiguration>() != 1)
				{
					result = string.Format("(Off)  {{Memory {0:D} not active}}", memoryNo);
				}
				else
				{
					result = string.Format("(NOT Stopped{0:D})", memoryNo);
				}
			}
			return result;
		}

		private string GenerateTriggerActiveLed(uint memoryNo)
		{
			IList<int> fromProjectMemoriesThatUseTriggeredLogging = GlobalSettings.GetFromProjectMemoriesThatUseTriggeredLogging();
			if (fromProjectMemoriesThatUseTriggeredLogging.Count <= 0)
			{
				return string.Format("(Off)  {{LED on trigger active is disabled since no memory uses triggered logging mode}}", new object[0]);
			}
			if (!this.mLedOnTriggerAlreadyExists)
			{
				using (IEnumerator<int> enumerator = fromProjectMemoriesThatUseTriggeredLogging.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						uint current = (uint)enumerator.Current;
						string arg = string.Format("LEDonTriggerMemory{0:D}", current);
						this.mLtlLEDCodeBeforeOutputSection.AppendFormat("VAR {0} = FREE[8]", arg);
						this.mLtlLEDCodeBeforeOutputSection.AppendLine();
						this.mLtlLEDCodeBeforeOutputSection.AppendFormat("EVENT ON SYSTEM (Logger{0:D}_Trigger) BEGIN", current);
						this.mLtlLEDCodeBeforeOutputSection.AppendLine();
						this.mLtlLEDCodeBeforeOutputSection.AppendFormat("  CALC {0} = ({1:D})  {{set {2:D} sec timer}}", arg, LtlLeds.ConstDurationForLEDonTrigger * 10u + 1u, LtlLeds.ConstDurationForLEDonTrigger);
						this.mLtlLEDCodeBeforeOutputSection.AppendLine();
						this.mLtlLEDCodeBeforeOutputSection.AppendLine("END");
						this.mLtlLEDCodeBeforeOutputSection.AppendLine("EVENT ON CYCLE (100) BEGIN");
						this.mLtlLEDCodeBeforeOutputSection.AppendFormat("  CALC {0} = ({0} - 1) WHEN ({0} > 0)", arg);
						this.mLtlLEDCodeBeforeOutputSection.AppendLine();
						this.mLtlLEDCodeBeforeOutputSection.AppendLine("END");
						this.mLtlLEDCodeBeforeOutputSection.AppendLine();
					}
				}
				this.mLedOnTriggerAlreadyExists = true;
			}
			if (memoryNo == 2147483647u || memoryNo == 0u)
			{
				throw new LTLGenerationException(LTLGenerator.LTLGenerationResult.LEDError, "LED on Trigger in NO or ALL memories!?");
			}
			string result;
			if (memoryNo == 2147483646u)
			{
				OrList orList = new OrList();
				using (IEnumerator<int> enumerator2 = fromProjectMemoriesThatUseTriggeredLogging.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						uint current2 = (uint)enumerator2.Current;
						orList.Add(string.Format("LEDonTriggerMemory{0} > 0", current2));
					}
				}
				result = orList.ToLTLCode();
			}
			else if (!fromProjectMemoriesThatUseTriggeredLogging.Contains((int)memoryNo))
			{
				result = string.Format("(Off)  {{LED on trigger active in memory {0:D} is disabled because this memory doesn't use triggered logging mode}}", memoryNo);
			}
			else
			{
				result = string.Format("(LEDonTriggerMemory{0} > 0)", memoryNo);
			}
			return result;
		}

		private string GenerateCANLinError()
		{
			this.MakeSureGenerateAnyCANerrorDetection();
			this.MakeSureGenerateAnyLinErrorDetection();
			OrList orList = new OrList();
			orList.Add(string.Format("{0} > 0", LtlLeds.ltlvarCANErrorsDetected));
			if (this.mAllLinsErrorsAlreadyExists)
			{
				orList.Add(string.Format("{0} > 0", LtlLeds.ltlvarLinErrorsDetected));
			}
			this.mLtlLEDCodeBeforeOutputSection.AppendLine();
			this.mLtlLEDCodeBeforeOutputSection.AppendFormat("FLAG {0} = {1}", LtlLeds.ltlFlagCANLinErrorsDetected, orList.ToLTLCode());
			this.mLtlLEDCodeBeforeOutputSection.AppendLine();
			this.mLtlLEDCodeBeforeOutputSection.AppendLine();
			return string.Format("({0})", LtlLeds.ltlFlagCANLinErrorsDetected);
		}

		private void MakeSureGenerateAnyCANerrorDetection()
		{
			if (this.mAllCANsErrorsAlreadyExists)
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (uint num = 1u; num <= this.mLoggerSpecifics.CAN.NumberOfChannels; num += 1u)
			{
				if (num > 1u)
				{
					stringBuilder.Append(" OR ");
				}
				stringBuilder.AppendFormat("({0} > 0)", LTLUtil.GetCANErrorrateVariableName(num));
				if (num % 2u == 0u)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendFormat("{0,-32}", " ");
				}
			}
			this.mLtlLEDCodeBeforeOutputSection.AppendLine();
			this.mLtlLEDCodeBeforeOutputSection.AppendFormat("VAR {0} = FREE[8]", LtlLeds.ltlvarCANErrorsDetected);
			this.mLtlLEDCodeBeforeOutputSection.AppendLine();
			this.mLtlLEDCodeBeforeOutputSection.AppendLine("EVENT ON CYCLE (100) BEGIN");
			this.mLtlLEDCodeBeforeOutputSection.AppendFormat("  CALC {0} = (21) WHEN {1}", LtlLeds.ltlvarCANErrorsDetected, stringBuilder);
			this.mLtlLEDCodeBeforeOutputSection.AppendLine();
			this.mLtlLEDCodeBeforeOutputSection.AppendFormat("  CALC {0} = ({0} - 1) WHEN ({0} > 0)", LtlLeds.ltlvarCANErrorsDetected);
			this.mLtlLEDCodeBeforeOutputSection.AppendLine();
			this.mLtlLEDCodeBeforeOutputSection.AppendLine("END");
			this.mLtlLEDCodeBeforeOutputSection.AppendLine();
			this.mAllCANsErrorsAlreadyExists = true;
		}

		private void MakeSureGenerateAnyLinErrorDetection()
		{
			if (this.mAllLinsErrorsAlreadyExists)
			{
				return;
			}
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = false;
			uint num = 0u;
			for (uint num2 = 1u; num2 <= this.mLoggerSpecifics.LIN.NumberOfChannels; num2 += 1u)
			{
				LINChannel lINChannel = this.mVlProject.ProjectRoot.HardwareConfiguration.LINChannelConfiguration.GetLINChannel(num2);
				if (lINChannel.IsActive.Value)
				{
					stringBuilder.AppendFormat(" LIN{0:D} DATA 0x40 - 0x7FF  ", num2);
					flag = true;
					num += 1u;
				}
			}
			if (this.mVlProject.ProjectRoot.HardwareConfiguration.LINChannelConfiguration.IsUsingLinProbe.Value)
			{
				for (uint num3 = this.mLoggerSpecifics.LIN.NumberOfChannels + 1u; num3 <= this.mLoggerSpecifics.LIN.NumberOfChannels + this.mLoggerSpecifics.LIN.NumberOfLINprobeChannels; num3 += 1u)
				{
					if (num > 0u && num % 2u == 0u)
					{
						stringBuilder.AppendLine();
						stringBuilder.AppendFormat("{0, -18}", " ");
					}
					stringBuilder.AppendFormat(" LIN{0:D} DATA 0x40 - 0x7FF  ", num3);
					flag = true;
					num += 1u;
				}
			}
			if (flag)
			{
				this.MakeSureGenerateStartOverTimer();
				this.mLtlLEDCodeBeforeOutputSection.AppendLine();
				this.mLtlLEDCodeBeforeOutputSection.AppendFormat("VAR {0} = FREE[8]", LtlLeds.ltlvarLinErrorsDetected);
				this.mLtlLEDCodeBeforeOutputSection.AppendLine();
				this.mLtlLEDCodeBeforeOutputSection.AppendFormat("EVENT ON RECEIVE ({0}) BEGIN", stringBuilder);
				this.mLtlLEDCodeBeforeOutputSection.AppendLine();
				this.mLtlLEDCodeBeforeOutputSection.AppendFormat("  CALC {0} = (21) WHEN ({1})", LtlLeds.ltlvarLinErrorsDetected, LtlLeds.ltlTimerStartOver);
				this.mLtlLEDCodeBeforeOutputSection.AppendLine();
				this.mLtlLEDCodeBeforeOutputSection.AppendLine("END");
				this.mLtlLEDCodeBeforeOutputSection.AppendLine("EVENT ON CYCLE (100) BEGIN");
				this.mLtlLEDCodeBeforeOutputSection.AppendFormat("  CALC {0} = ({0} - 1) WHEN ({0} > 0)", LtlLeds.ltlvarLinErrorsDetected);
				this.mLtlLEDCodeBeforeOutputSection.AppendLine();
				this.mLtlLEDCodeBeforeOutputSection.AppendLine("END");
				this.mLtlLEDCodeBeforeOutputSection.AppendLine();
				this.mAllLinsErrorsAlreadyExists = true;
			}
		}

		private void MakeSureGenerateStartOverTimer()
		{
			if (!this.mStartOverTimerAlreadyExists)
			{
				this.mLtlLEDCodeBeforeOutputSection.AppendFormat("TIMER {0} TIME = 1000 (1)  {{ don't look at LIN errors during logger start }}", LtlLeds.ltlTimerStartOver);
				this.mLtlLEDCodeBeforeOutputSection.AppendLine();
				this.mStartOverTimerAlreadyExists = true;
			}
		}

		private string GenerateCANError(uint ltlLedIndex, uint channel)
		{
			if (channel == 0u)
			{
				throw new LTLGenerationException(LTLGenerator.LTLGenerationResult.LEDError, "Undefined CAN channel");
			}
			string result;
			if (channel == 4294967295u)
			{
				this.MakeSureGenerateAnyCANerrorDetection();
				result = string.Format("({0} > 0)", LtlLeds.ltlvarCANErrorsDetected);
			}
			else
			{
				string arg = string.Format("Led{0:D}_onCANError", ltlLedIndex);
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("VAR {0} = FREE[8]");
				stringBuilder.AppendLine("EVENT ON CYCLE (100) BEGIN");
				stringBuilder.AppendLine("  CALC {0} = (21) WHEN ({1} > 0)");
				stringBuilder.AppendLine("  CALC {0} = ({0} - 1) WHEN ({0} > 0)");
				stringBuilder.AppendLine("END");
				this.mLtlLEDCodeBeforeOutputSection.AppendFormat(stringBuilder.ToString(), arg, LTLUtil.GetCANErrorrateVariableName(channel));
				this.mLtlLEDCodeBeforeOutputSection.AppendLine();
				result = string.Format("({0} > 0)", arg);
			}
			return result;
		}

		private string GenerateLinErrorCondition(uint ltlLedIndex, uint channel)
		{
			if (channel == 0u)
			{
				throw new LTLGenerationException(LTLGenerator.LTLGenerationResult.LEDError, "Undefined CAN channel");
			}
			string result;
			if (channel == 4294967295u)
			{
				this.MakeSureGenerateAnyLinErrorDetection();
				if (this.mAllLinsErrorsAlreadyExists)
				{
					result = string.Format("({0} > 0)", LtlLeds.ltlvarLinErrorsDetected);
				}
				else
				{
					result = "(Off) {no LIN error possible because all LINs are inactive}";
				}
			}
			else
			{
				this.MakeSureGenerateStartOverTimer();
				string arg = string.Format("Led{0:D}_onLINerror", ltlLedIndex);
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("VAR {0} = FREE[8]");
				stringBuilder.AppendLine("EVENT ON RECEIVE ({1} DATA 0x40 - 0x7FF) BEGIN");
				stringBuilder.AppendLine("  CALC {0} = (21) WHEN ({2})");
				stringBuilder.AppendLine("END");
				stringBuilder.AppendLine("EVENT ON CYCLE (100) BEGIN");
				stringBuilder.AppendLine("  CALC {0} = ({0} - 1) WHEN ({0} > 0)");
				stringBuilder.AppendLine("END");
				this.mLtlLEDCodeBeforeOutputSection.AppendFormat(stringBuilder.ToString(), arg, LTLUtil.GetChannelString(BusType.Bt_LIN, channel), LtlLeds.ltlTimerStartOver);
				result = string.Format("({0} > 0)", arg);
			}
			return result;
		}

		private uint GetMemoryNumberFromParameter(uint ledIndexInModel)
		{
			if ((long)this.mLedConfig.LEDConfigList.Count < (long)((ulong)ledIndexInModel) || this.mLedConfig.LEDConfigList[(int)(ledIndexInModel - 1u)] == null)
			{
				throw new LTLGenerationException(LTLGenerator.LTLGenerationResult.LEDError);
			}
			LEDConfigListItem lEDConfigListItem = this.mLedConfig.LEDConfigList[(int)(ledIndexInModel - 1u)];
			if (lEDConfigListItem.ParameterMemory.Value == 0u)
			{
				throw new LTLGenerationException(LTLGenerator.LTLGenerationResult.LEDError);
			}
			return lEDConfigListItem.ParameterMemory.Value;
		}

		private uint GetChannelNumberFromParameter(uint ledIndexInModel)
		{
			if ((long)this.mLedConfig.LEDConfigList.Count < (long)((ulong)ledIndexInModel) || this.mLedConfig.LEDConfigList[(int)(ledIndexInModel - 1u)] == null)
			{
				throw new LTLGenerationException(LTLGenerator.LTLGenerationResult.LEDError);
			}
			LEDConfigListItem lEDConfigListItem = this.mLedConfig.LEDConfigList[(int)(ledIndexInModel - 1u)];
			if (lEDConfigListItem.ParameterChannelNumber.Value == 0u)
			{
				throw new LTLGenerationException(LTLGenerator.LTLGenerationResult.LEDError);
			}
			return lEDConfigListItem.ParameterChannelNumber.Value;
		}
	}
}
