using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.LTLGenerationCS.Utility;

namespace Vector.VLConfig.LTLGenerationCS
{
	public static class LTLUtil
	{
		private static readonly char[] WhiteSpaceChars = new char[]
		{
			'\t',
			'\n',
			'\v',
			'\f',
			'\r',
			' '
		};

		public static readonly uint BusLoadInterval = 1000u;

		public static readonly string StartTimeDelayFlag = "StartDelayOver";

		public static string GetBusTypeString(BusType busType)
		{
			switch (busType)
			{
			case BusType.Bt_CAN:
				return "CAN";
			case BusType.Bt_LIN:
				return "LIN";
			case BusType.Bt_FlexRay:
				return "FR";
			case BusType.Bt_Ethernet:
				return "ETH";
			}
			return string.Format("{{unsupported BusType ({0})}}", (int)busType);
		}

		public static string GetChannelString(BusType busType, uint channelNumber)
		{
			return string.Format("{0}{1}", LTLUtil.GetBusTypeString(busType), channelNumber);
		}

		public static string GetPrettyChannelString(BusType busType, uint channelNumber)
		{
			if (busType == BusType.Bt_FlexRay && channelNumber == Database.ChannelNumber_FlexrayAB)
			{
				return LTLUtil.GetBusTypeString(busType);
			}
			return LTLUtil.GetChannelString(busType, channelNumber);
		}

		public static uint GetHighestID(BusType bustype, bool isExtendedID)
		{
			if (bustype == BusType.Bt_CAN)
			{
				if (isExtendedID)
				{
					return Constants.MaximumExtendedCANId;
				}
				return Constants.MaximumStandardCANId;
			}
			else
			{
				if (bustype == BusType.Bt_LIN)
				{
					return Constants.MaximumLINId;
				}
				if (bustype == BusType.Bt_FlexRay)
				{
					return Constants.MaximumFlexraySlotId;
				}
				return 0u;
			}
		}

		public static string GetDATAString(bool isExtendedID, bool addExtraSpace)
		{
			if (isExtendedID)
			{
				return "XDATA";
			}
			if (addExtraSpace)
			{
				return " DATA";
			}
			return "DATA";
		}

		public static string GetDATAString(bool isExtendedID)
		{
			return LTLUtil.GetDATAString(isExtendedID, false);
		}

		public static string GetIdString(BusType busType, uint channelNumber, bool isCanExtendedId, uint id, uint frBaseCycle, uint frRepetition, bool isRange, uint lastId, bool equidistantFormat)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (equidistantFormat)
			{
				stringBuilder.AppendFormat("{0,4}{1:D} {2,-5}", LTLUtil.GetBusTypeString(busType), channelNumber, LTLUtil.GetDATAString(isCanExtendedId, true));
			}
			else
			{
				stringBuilder.AppendFormat("{0}{1:D} {2}", LTLUtil.GetBusTypeString(busType), channelNumber, LTLUtil.GetDATAString(isCanExtendedId, true));
			}
			if (busType == BusType.Bt_FlexRay)
			{
				if (isRange)
				{
					stringBuilder.AppendFormat(" {0:D}", id);
				}
				else if (equidistantFormat)
				{
					stringBuilder.AppendFormat(" {0,3:D}:{1,2:D}:{2,2:D}", id, frBaseCycle, frRepetition);
				}
				else
				{
					stringBuilder.AppendFormat(" {0:D}:{1:D}:{2:D}", id, frBaseCycle, frRepetition);
				}
			}
			else
			{
				stringBuilder.AppendFormat(" 0x{0:X}", id);
			}
			if (isRange)
			{
				if (busType == BusType.Bt_FlexRay)
				{
					stringBuilder.AppendFormat(" - {0:D}", lastId);
				}
				else
				{
					stringBuilder.AppendFormat(" - 0x{0:X}", lastId);
				}
			}
			return stringBuilder.ToString();
		}

		public static string GetIdString(BusType busType, uint channelNumber, bool isCanExtendedId, uint id, uint frBaseCycle, uint frRepetition, bool isRange, uint lastId)
		{
			return LTLUtil.GetIdString(busType, channelNumber, isCanExtendedId, id, frBaseCycle, frRepetition, isRange, lastId, false);
		}

		public static string GetIdString(BusType bustype, uint channelNumber, bool isCanExtendedId, uint id)
		{
			return LTLUtil.GetIdString(bustype, channelNumber, isCanExtendedId, id, 0u, 1u, false, 0u);
		}

		public static string GetXCPIncFilename(string dbcFilename, int index, CPType protocolType)
		{
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(dbcFilename);
			if (string.IsNullOrEmpty(fileNameWithoutExtension))
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder();
			string text = fileNameWithoutExtension;
			for (int i = 0; i < text.Length; i++)
			{
				char c = text[i];
				if (char.IsLetterOrDigit(c))
				{
					stringBuilder.Append(c);
				}
				else
				{
					stringBuilder.Append("_");
				}
			}
			string str = string.Format("{0}_{1}_{2:D2}", stringBuilder, LTLUtil.GetXCPProtocolSmallLetterString(protocolType), index);
			return str + Vocabulary.FileExtensionDotINC;
		}

		public static string GetCcpXcpSkbFilenameForDbc2Inc(string dbcFilenameOrEcuName)
		{
			string str;
			if (Path.HasExtension(dbcFilenameOrEcuName))
			{
				str = Path.GetFileNameWithoutExtension(dbcFilenameOrEcuName);
			}
			else
			{
				str = dbcFilenameOrEcuName;
			}
			return str + "_DAQ" + Vocabulary.FileExtensionDotSKB;
		}

		public static string GetXCPProtocolSmallLetterString(CPType cptype)
		{
			string result;
			if (cptype == CPType.CCP)
			{
				result = "ccp";
			}
			else if (cptype == CPType.CCP101)
			{
				result = "ccp101";
			}
			else
			{
				result = "xcp";
			}
			return result;
		}

		public static string BuildFilterLine(ILoggerSpecifics loggerSpecifics, uint memory, FilterActionType action, uint limitInterval, BusType busType, uint channelNumber, bool isExtendedId, uint id)
		{
			return LTLUtil.BuildFilterLine(loggerSpecifics, memory, action, limitInterval, busType, channelNumber, isExtendedId, id, 0u, 1u);
		}

		public static string BuildFilterLine(ILoggerSpecifics loggerSpecifics, uint memory, FilterActionType action, uint limitInterval, BusType busType, uint channelNumber, bool isExtendedId, uint id, bool isRange, uint lastId)
		{
			return LTLUtil.BuildFilterLine(loggerSpecifics, memory, action, limitInterval, busType, channelNumber, isExtendedId, id, 0u, 1u, isRange, lastId);
		}

		public static string BuildFilterLine(ILoggerSpecifics loggerSpecifics, uint memory, FilterActionType action, uint limitInterval, BusType busType, uint channelNumber, bool isCanExtendedId, uint id, uint frBaseCycle, uint frRepetition)
		{
			return LTLUtil.BuildFilterLine(loggerSpecifics, memory, action, limitInterval, busType, channelNumber, isCanExtendedId, id, frBaseCycle, frRepetition, false, 0u);
		}

		public static string BuildFilterLine(ILoggerSpecifics loggerSpecifics, uint memory, FilterActionType action, uint limitInterval, BusType busType, uint channelNumber, bool isExtendedId, uint id, uint frBaseCycle, uint frRepetition, bool isRange, uint lastId)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string arg;
			if (action == FilterActionType.Stop)
			{
				arg = "EXCLUDE";
			}
			else if (action == FilterActionType.Limit)
			{
				arg = string.Format("LIMIT CYCLE = {0}", limitInterval);
			}
			else
			{
				arg = "INCLUDE";
			}
			stringBuilder.AppendFormat("  {0} {1}", arg, LTLUtil.GetIdString(busType, channelNumber, isExtendedId, id, frBaseCycle, frRepetition, isRange, lastId, true));
			return stringBuilder.ToString();
		}

		public static bool GetLTLCompareOperatorString(CondRelation condRelation, out string operatorString)
		{
			switch (condRelation)
			{
			case CondRelation.Equal:
				operatorString = "=";
				return true;
			case CondRelation.NotEqual:
				operatorString = "<>";
				return true;
			case CondRelation.LessThan:
				operatorString = "<";
				return true;
			case CondRelation.LessThanOrEqual:
				operatorString = "<=";
				return true;
			case CondRelation.GreaterThan:
				operatorString = ">";
				return true;
			case CondRelation.GreaterThanOrEqual:
				operatorString = ">=";
				return true;
			default:
				operatorString = "";
				return false;
			}
		}

		public static LTLGenerator.LTLGenerationResult GenerateLtlSignalDefinition(out string ltlSignalDefinition, SignalDefinition signalDefinition, Dictionary<double, string> textEncValueTable, BusType busType, uint channelNumber, string signalBaseName, long initValue, IList<SignalDefinition> frSignalList, bool doAddMetaData, bool doAddSignalPostfixToName, out string multiplexorCondition)
		{
			ltlSignalDefinition = string.Empty;
			multiplexorCondition = string.Empty;
			StringBuilder stringBuilder = new StringBuilder();
			SignalDefinition signalDefinition2 = new SignalDefinition();
			SignalDefinition signalDefinition3 = new SignalDefinition();
			LTLGenerator.LTLGenerationResult result;
			if (signalDefinition.Length > 16u && signalDefinition.Length <= 32u && (result = LTLUtil.SplitSignalsIntoHighAndLowPart(signalDefinition, out signalDefinition2, out signalDefinition3)) != LTLGenerator.LTLGenerationResult.OK)
			{
				return result;
			}
			switch (busType)
			{
			case BusType.Bt_FlexRay:
			{
				StringBuilder stringBuilder2 = new StringBuilder();
				uint num = 0u;
				foreach (SignalDefinition current in frSignalList)
				{
					string idString = LTLUtil.GetIdString(busType, channelNumber, false, current.Message.ActualMessageId, (uint)current.Message.FrBaseCycle, (uint)current.Message.FrCycleRepetition, false, 0u, true);
					if (current.IsMultiplexed)
					{
						string signalPosition = LTLUtil.GetSignalPosition(current.Multiplexor.StartBit, current.Multiplexor.Length, current.Multiplexor.IsMotorola);
						stringBuilder.AppendFormat("VAR {0}_Multiplexor = {1} [{2}] {3}", new object[]
						{
							signalBaseName,
							idString,
							signalPosition,
							signalDefinition.Multiplexor.IsSigned ? "SIGNED" : ""
						});
						stringBuilder.AppendLine();
						multiplexorCondition = string.Format("{0}_Multiplexor = {1}", signalBaseName, current.MultiplexorValue);
					}
					if (current.HasUpdateBit)
					{
						string updateBitPositionCode = LTLUtil.GetUpdateBitPositionCode((uint)current.UpdateBit);
						stringBuilder2.AppendFormat("VAR {0}.Update..Bit{1} = {2} [{3}]", new object[]
						{
							signalBaseName,
							(num > 0u) ? string.Format("..{0}", num) : "",
							idString,
							updateBitPositionCode
						});
						if (num > 0u)
						{
							stringBuilder2.AppendFormat("  ALIASING {0}.Update..Bit", signalBaseName);
						}
						stringBuilder2.AppendLine();
					}
					if (current.Length <= 16u)
					{
						string signalPosition2 = LTLUtil.GetSignalPosition(current.StartBit, current.Length, current.IsMotorola);
						stringBuilder2.AppendFormat("VAR {0}{1}      = {2} [{3}]", new object[]
						{
							doAddSignalPostfixToName ? (signalBaseName + "_Signal") : signalBaseName,
							(num > 0u) ? string.Format("..{0:D}", num) : "",
							idString,
							signalPosition2
						});
						if (doAddMetaData)
						{
							stringBuilder2.AppendLine();
							stringBuilder2.Append(LTLUtil.GetSignalMetaData(signalDefinition, textEncValueTable));
						}
						if (num > 0u)
						{
							stringBuilder2.AppendFormat("  ALIASING {0}", doAddSignalPostfixToName ? (signalBaseName + "_Signal") : signalBaseName);
						}
						AndList andList = new AndList();
						if (current.HasUpdateBit)
						{
							andList.Add(string.Format("{0}.Update..Bit", signalBaseName));
						}
						if (current.IsMultiplexed)
						{
							andList.Add(multiplexorCondition);
						}
						if (andList.Count() > 0)
						{
							stringBuilder2.AppendFormat(" WHEN {0}", andList.ToLTLCode());
						}
					}
					else
					{
						SignalDefinition signalDefinition4;
						SignalDefinition signalDefinition5;
						LTLUtil.SplitSignalsIntoHighAndLowPart(current, out signalDefinition4, out signalDefinition5);
						string signalPosition3 = LTLUtil.GetSignalPosition(signalDefinition4.StartBit, signalDefinition4.Length, signalDefinition4.IsMotorola);
						stringBuilder2.AppendFormat("VAR {0}{1}.HIGH = {2} [{3}]", new object[]
						{
							signalBaseName + (doAddSignalPostfixToName ? "_Signal" : ""),
							(num > 0u) ? string.Format("..{0:D}", num) : "",
							idString,
							signalPosition3
						});
						if (num > 0u)
						{
							stringBuilder2.AppendFormat("  ALIASING {0}{1}.HIGH", signalBaseName, doAddSignalPostfixToName ? "_Signal" : "");
						}
						AndList andList2 = new AndList();
						if (current.HasUpdateBit)
						{
							andList2.Add(string.Format("{0}.Update..Bit", signalBaseName));
						}
						if (current.IsMultiplexed)
						{
							andList2.Add(multiplexorCondition);
						}
						if (andList2.Count() > 0)
						{
							stringBuilder2.AppendFormat(" WHEN {0}", andList2.ToLTLCode());
						}
						stringBuilder2.AppendLine();
						signalPosition3 = LTLUtil.GetSignalPosition(signalDefinition5.StartBit, signalDefinition5.Length, signalDefinition5.IsMotorola);
						stringBuilder2.AppendFormat("VAR {0}{1}.LOW  = {2} [{3}]", new object[]
						{
							signalBaseName + (doAddSignalPostfixToName ? "_Signal" : ""),
							(num > 0u) ? string.Format("..{0:D}", num) : "",
							idString,
							signalPosition3
						});
						if (num > 0u)
						{
							stringBuilder2.AppendFormat("  ALIASING {0}{1}.LOW", signalBaseName, doAddSignalPostfixToName ? "_Signal" : "");
						}
						if (andList2.Count() > 0)
						{
							stringBuilder2.AppendFormat(" WHEN {0}", andList2.ToLTLCode());
						}
					}
					stringBuilder2.AppendLine();
					num += 1u;
				}
				stringBuilder.Append(stringBuilder2);
				stringBuilder.AppendLine();
				goto IL_B72;
			}
			case BusType.Bt_J1708:
			{
				IL_66:
				string idString2 = LTLUtil.GetIdString(busType, channelNumber, signalDefinition.Message.IsExtendedId, signalDefinition.Message.ActualMessageId, (uint)signalDefinition.Message.FrBaseCycle, (uint)signalDefinition.Message.FrCycleRepetition, false, 0u);
				if (signalDefinition.IsMultiplexed)
				{
					string signalPosition4 = LTLUtil.GetSignalPosition(signalDefinition.Multiplexor.StartBit, signalDefinition.Multiplexor.Length, signalDefinition.Multiplexor.IsMotorola);
					stringBuilder.AppendFormat("VAR {0}_Multiplexor = {1} [{2}] {3}", new object[]
					{
						signalBaseName,
						idString2,
						signalPosition4,
						signalDefinition.Multiplexor.IsSigned ? "SIGNED" : ""
					});
					stringBuilder.AppendLine();
					multiplexorCondition = string.Format("{0}_Multiplexor = {1}", signalBaseName, signalDefinition.MultiplexorValue);
				}
				if (signalDefinition.Length <= 16u)
				{
					string signalPosition5 = LTLUtil.GetSignalPosition(signalDefinition.StartBit, signalDefinition.Length, signalDefinition.IsMotorola);
					stringBuilder.AppendFormat("VAR {0} = {1} [{2}] {3} {4}", new object[]
					{
						doAddSignalPostfixToName ? (signalBaseName + "_Signal") : signalBaseName,
						idString2,
						signalPosition5,
						signalDefinition.IsSigned ? "SIGNED" : "",
						LTLUtil.GetInitString(initValue)
					});
					if (doAddMetaData)
					{
						stringBuilder.AppendLine();
						stringBuilder.Append(LTLUtil.GetSignalMetaData(signalDefinition, textEncValueTable));
					}
					if (signalDefinition.IsMultiplexed)
					{
						stringBuilder.AppendFormat(" WHEN ({0})", multiplexorCondition);
					}
				}
				else
				{
					bool is17bitSigned = signalDefinition.Length == 17u && signalDefinition.IsSigned;
					long value = (long)LTLUtil.HighPart(initValue, is17bitSigned);
					string signalPosition6 = LTLUtil.GetSignalPosition(signalDefinition2.StartBit, signalDefinition2.Length, signalDefinition2.IsMotorola);
					stringBuilder.AppendFormat("VAR {0}.HIGH = {1} [{2}] {3} {4}", new object[]
					{
						signalBaseName + (doAddSignalPostfixToName ? "_Signal" : ""),
						idString2,
						signalPosition6,
						signalDefinition2.IsSigned ? "SIGNED" : "",
						LTLUtil.GetInitString(value)
					});
					if (signalDefinition.IsMultiplexed)
					{
						stringBuilder.AppendFormat(" WHEN ({0})", multiplexorCondition);
					}
					stringBuilder.AppendLine();
					long value2 = (long)((ulong)LTLUtil.LowPart(initValue, is17bitSigned));
					signalPosition6 = LTLUtil.GetSignalPosition(signalDefinition3.StartBit, signalDefinition3.Length, signalDefinition3.IsMotorola);
					stringBuilder.AppendFormat("VAR {0}.LOW = {1} [{2}] {3} {4}", new object[]
					{
						signalBaseName + (doAddSignalPostfixToName ? "_Signal" : ""),
						idString2,
						signalPosition6,
						signalDefinition3.IsSigned ? "SIGNED" : "",
						LTLUtil.GetInitString(value2)
					});
					if (signalDefinition.IsMultiplexed)
					{
						stringBuilder.AppendFormat(" WHEN ({0})", multiplexorCondition);
					}
				}
				stringBuilder.AppendLine();
				goto IL_B72;
			}
			case BusType.Bt_Ethernet:
			{
				string text = signalDefinition.Message.NamePrefix + signalDefinition.Message.Name;
				if (signalDefinition.IsMultiplexed)
				{
					string signalPosition7 = LTLUtil.GetSignalPosition(signalDefinition.Multiplexor.StartBit, signalDefinition.Multiplexor.Length, signalDefinition.Multiplexor.IsMotorola);
					stringBuilder.AppendFormat("VAR {0}_Multiplexor = {1} [{2}] {3}", new object[]
					{
						signalBaseName,
						text,
						signalPosition7,
						signalDefinition.Multiplexor.IsSigned ? "SIGNED" : ""
					});
					stringBuilder.AppendLine();
					multiplexorCondition = string.Format("{0}_Multiplexor = {1}", signalBaseName, signalDefinition.MultiplexorValue);
				}
				if (signalDefinition.Length <= 16u)
				{
					string signalPosition8 = LTLUtil.GetSignalPosition(signalDefinition.StartBit, signalDefinition.Length, signalDefinition.IsMotorola);
					stringBuilder.AppendFormat("VAR {0} = {1} [{2}] {3} {4}", new object[]
					{
						doAddSignalPostfixToName ? (signalBaseName + "_Signal") : signalBaseName,
						text,
						signalPosition8,
						signalDefinition.IsSigned ? "SIGNED" : "",
						LTLUtil.GetInitString(initValue)
					});
					if (signalDefinition.IsMultiplexed)
					{
						stringBuilder.AppendFormat(" WHEN ({0})", multiplexorCondition);
					}
				}
				else
				{
					bool is17bitSigned2 = signalDefinition.Length == 17u && signalDefinition.IsSigned;
					long value3 = (long)LTLUtil.HighPart(initValue, is17bitSigned2);
					string signalPosition9 = LTLUtil.GetSignalPosition(signalDefinition2.StartBit, signalDefinition2.Length, signalDefinition2.IsMotorola);
					stringBuilder.AppendFormat("VAR {0}.HIGH = {1} [{2}] {3} {4}", new object[]
					{
						signalBaseName + (doAddSignalPostfixToName ? "_Signal" : ""),
						text,
						signalPosition9,
						signalDefinition2.IsSigned ? "SIGNED" : "",
						LTLUtil.GetInitString(value3)
					});
					if (signalDefinition.IsMultiplexed)
					{
						stringBuilder.AppendFormat(" WHEN ({0})", multiplexorCondition);
					}
					stringBuilder.AppendLine();
					long value4 = (long)((ulong)LTLUtil.LowPart(initValue, is17bitSigned2));
					signalPosition9 = LTLUtil.GetSignalPosition(signalDefinition3.StartBit, signalDefinition3.Length, signalDefinition3.IsMotorola);
					stringBuilder.AppendFormat("VAR {0}.LOW = {1} [{2}] {3} {4}", new object[]
					{
						signalBaseName + (doAddSignalPostfixToName ? "_Signal" : ""),
						text,
						signalPosition9,
						signalDefinition3.IsSigned ? "SIGNED" : "",
						LTLUtil.GetInitString(value4)
					});
					if (signalDefinition.IsMultiplexed)
					{
						stringBuilder.AppendFormat(" WHEN ({0})", multiplexorCondition);
					}
				}
				stringBuilder.AppendLine();
				goto IL_B72;
			}
			}
			goto IL_66;
			IL_B72:
			ltlSignalDefinition = stringBuilder.ToString();
			return LTLGenerator.LTLGenerationResult.OK;
		}

		public static string GenerateLtlTriggerEventDefinition(bool is32BitSignal, string eventFlagNameWithoutDot)
		{
			string result;
			if (!is32BitSignal)
			{
				result = string.Format("RECEIVE ({0}_Signal)", eventFlagNameWithoutDot);
			}
			else
			{
				result = string.Format("RECEIVE ({0}_Signal.LOW)", eventFlagNameWithoutDot);
			}
			return result;
		}

		private static string GetSignalPosition(uint startBit, uint length, bool isMotorola)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (length == 0u)
			{
				throw new LTLGenerationException(LTLGenerator.LTLGenerationResult.TriggerError_Bitlength);
			}
			if (length > 16u)
			{
				throw new LTLGenerationException(LTLGenerator.LTLGenerationResult.TriggerError_Bitlength);
			}
			if (length == 1u)
			{
				stringBuilder.AppendFormat("{0:D}({1:D})", startBit / 8u, startBit % 8u);
				return stringBuilder.ToString();
			}
			if (isMotorola)
			{
				uint num = length;
				uint num2 = startBit / 8u;
				uint num3 = startBit % 8u;
				while (num > 0u && num2 < 1480u)
				{
					uint num4;
					if (num3 < num)
					{
						num4 = 0u;
					}
					else
					{
						num4 = num3 - num + 1u;
					}
					if (num3 == num4)
					{
						stringBuilder.AppendFormat(string.Format(" {0:D}({1:D})", num2, num3), new object[0]);
					}
					else
					{
						stringBuilder.AppendFormat(string.Format(" {0:D}({1:D}:{2:D})", num2, num3, num4), new object[0]);
					}
					num -= num3 + 1u - num4;
					num2 += 1u;
					num3 = 7u;
				}
				if (num > 0u)
				{
					throw new LTLGenerationException(LTLGenerator.LTLGenerationResult.TriggerError_Bitlength);
				}
				if (num2 > 1480u)
				{
					throw new LTLGenerationException(LTLGenerator.LTLGenerationResult.TriggerError_Bitlength);
				}
				return stringBuilder.ToString();
			}
			else
			{
				uint num5 = length;
				uint num6 = startBit / 8u;
				uint num7 = startBit % 8u;
				while (num5 > 0u && num6 < 1480u)
				{
					uint num8;
					if (num7 + num5 > 8u)
					{
						num8 = 7u;
					}
					else
					{
						num8 = num7 + num5 - 1u;
					}
					if (num8 == num7)
					{
						stringBuilder.Insert(0, string.Format(" {0:D}({1:D})", num6, num8));
					}
					else
					{
						stringBuilder.Insert(0, string.Format(" {0:D}({1:D}:{2:D})", num6, num8, num7));
					}
					num5 -= num8 + 1u - num7;
					num6 += 1u;
					num7 = 0u;
				}
				if (num5 > 0u)
				{
					throw new LTLGenerationException(LTLGenerator.LTLGenerationResult.TriggerError_Bitlength);
				}
				if (num6 > 1480u)
				{
					throw new LTLGenerationException(LTLGenerator.LTLGenerationResult.TriggerError_Bitlength);
				}
				return stringBuilder.ToString();
			}
		}

		private static string GetUpdateBitPositionCode(uint updateBitPosition)
		{
			uint num = updateBitPosition / 8u;
			uint num2 = 7u - updateBitPosition % 8u;
			return string.Format(" {0:D}({1:D})", num, num2);
		}

		private static string GetSignalMetaData(SignalDefinition signalDefinition, Dictionary<double, string> valueTable)
		{
			string unit = signalDefinition.Unit;
			double factor = signalDefinition.Factor;
			double offset = signalDefinition.Offset;
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(new string(' ', 37) + string.Format("FACTOR = {0}  OFFSET = {1}", factor.ToString(CultureInfo.InvariantCulture), offset.ToString(CultureInfo.InvariantCulture)));
			stringBuilder.AppendLine(new string(' ', 37) + string.Format("UNITSTRING = \"{0}\"", unit));
			if (valueTable != null && valueTable.Count > 0)
			{
				stringBuilder.Append(new string(' ', 37) + "VALUETABLE = [");
				foreach (KeyValuePair<double, string> current in valueTable)
				{
					stringBuilder.AppendFormat("{0:D} = \"{1}\" ", Convert.ToInt64(current.Key), current.Value);
				}
				stringBuilder.AppendLine("]");
			}
			return stringBuilder.ToString();
		}

		private static string GetInitString(long value)
		{
			if (value != 0L)
			{
				return string.Format("INIT = {0}", value);
			}
			return string.Empty;
		}

		public static LTLGenerator.LTLGenerationResult SplitSignalsIntoHighAndLowPart(SignalDefinition signalDefinition, out SignalDefinition signalDefinitionHIGH, out SignalDefinition signalDefinitionLOW)
		{
			signalDefinitionLOW = new SignalDefinition();
			signalDefinitionHIGH = new SignalDefinition();
			if (signalDefinition.Length <= 16u)
			{
				return LTLGenerator.LTLGenerationResult.TriggerError_Bitlength;
			}
			uint num = 16u;
			long num2 = 65536L;
			bool flag = signalDefinition.Length == 17u && signalDefinition.IsSigned;
			if (flag)
			{
				num = 15u;
				num2 = 32768L;
			}
			uint num3;
			uint startBit;
			if (signalDefinition.IsMotorola)
			{
				num3 = signalDefinition.StartBit;
				for (uint num4 = 0u; num4 < signalDefinition.Length - num; num4 += 1u)
				{
					if (num3 % 8u == 0u)
					{
						num3 += 16u;
					}
					num3 -= 1u;
				}
				startBit = signalDefinition.StartBit;
			}
			else
			{
				num3 = signalDefinition.StartBit;
				startBit = signalDefinition.StartBit + num;
			}
			signalDefinitionLOW.SetSignal(signalDefinition.Message, num3, num, signalDefinition.IsMotorola, false, signalDefinition.IsIntegerType, signalDefinition.Unit, signalDefinition.Factor, signalDefinition.Offset, signalDefinition.IsMultiRange, signalDefinition.UpdateBit, signalDefinition.HasTextEncodedValueTable, signalDefinition.HasLinearConversion);
			signalDefinitionHIGH.SetSignal(signalDefinition.Message, startBit, signalDefinition.Length - num, signalDefinition.IsMotorola, signalDefinition.IsSigned, signalDefinition.IsIntegerType, signalDefinition.Unit, signalDefinition.Factor * (double)num2, signalDefinition.Offset, signalDefinition.IsMultiRange, signalDefinition.UpdateBit, signalDefinition.HasTextEncodedValueTable, signalDefinition.HasLinearConversion);
			if (signalDefinition.IsMultiplexed)
			{
				signalDefinitionHIGH.SetMultiplexor(signalDefinition.Multiplexor, signalDefinition.MultiplexorValue);
				signalDefinitionLOW.SetMultiplexor(signalDefinition.Multiplexor, signalDefinition.MultiplexorValue);
			}
			return LTLGenerator.LTLGenerationResult.OK;
		}

		public static uint GetHighPartLength(SignalDefinition signalDefinition)
		{
			if (signalDefinition.Length <= 16u)
			{
				return 0u;
			}
			return signalDefinition.Length - LTLUtil.GetLowPartLength(signalDefinition);
		}

		public static uint GetLowPartLength(SignalDefinition signalDefinition)
		{
			if (signalDefinition.Length <= 16u)
			{
				return signalDefinition.Length;
			}
			uint result = 16u;
			bool flag = signalDefinition.Length == 17u && signalDefinition.IsSigned;
			if (flag)
			{
				result = 15u;
			}
			return result;
		}

		public static int HighPart(double value, bool is17BitSigned)
		{
			long value2 = Convert.ToInt64(value);
			return LTLUtil.HighPart(value2, is17BitSigned);
		}

		private static int HighPart(long value, bool is17bitSigned)
		{
			long value2;
			if (is17bitSigned)
			{
				value2 = value >> 15;
			}
			else
			{
				value2 = value >> 16;
			}
			return Convert.ToInt32(value2);
		}

		public static ushort LowPart(double value, bool is17bitSigned)
		{
			long value2 = Convert.ToInt64(value);
			return LTLUtil.LowPart(value2, is17bitSigned);
		}

		private static ushort LowPart(long value, bool is17bitSigned)
		{
			if (is17bitSigned)
			{
				return (ushort)(value & 32767L);
			}
			return (ushort)(value & 65535L);
		}

		public static LTLGenerator.LTLGenerationResult GenerateDiagSignal(out StringBuilder ltlDiagSignal, string serviceQualifier, SignalDefinition signalDefinition, string signalBaseName, bool doAddSignalPostfixToName)
		{
			ltlDiagSignal = new StringBuilder();
			string text = signalBaseName + (doAddSignalPostfixToName ? "_Signal" : "");
			if (signalDefinition.Length > 16u && signalDefinition.Length <= 32u)
			{
				SignalDefinition signalDefinition2 = new SignalDefinition();
				SignalDefinition signalDefinition3 = new SignalDefinition();
				LTLGenerator.LTLGenerationResult result;
				if ((result = LTLUtil.SplitSignalsIntoHighAndLowPart(signalDefinition, out signalDefinition3, out signalDefinition2)) != LTLGenerator.LTLGenerationResult.OK)
				{
					return result;
				}
				string signalBaseName2 = text + ".LOW";
				StringBuilder value;
				if ((result = LTLUtil.GenerateDiagSignal(out value, serviceQualifier, signalDefinition2, signalBaseName2, false)) != LTLGenerator.LTLGenerationResult.OK)
				{
					return result;
				}
				ltlDiagSignal.Append(value);
				ltlDiagSignal.AppendLine();
				string signalBaseName3 = text + ".HIGH";
				StringBuilder value2;
				if ((result = LTLUtil.GenerateDiagSignal(out value2, serviceQualifier, signalDefinition3, signalBaseName3, false)) != LTLGenerator.LTLGenerationResult.OK)
				{
					return result;
				}
				ltlDiagSignal.Append(value2);
				ltlDiagSignal.AppendLine();
			}
			else
			{
				uint startBit = signalDefinition.StartBit - 8u;
				string signalPosition;
				if ((signalPosition = LTLUtil.GetSignalPosition(startBit, signalDefinition.Length, signalDefinition.IsMotorola)) == string.Empty)
				{
					return LTLGenerator.LTLGenerationResult.DiagError;
				}
				StringBuilder stringBuilder = new StringBuilder();
				string source = signalDefinition.Factor.ToString(CultureInfo.CreateSpecificCulture("en-US"));
				if (source.Contains('E'))
				{
					source = LTLUtil.GetFloatingPointString(signalDefinition.Factor);
				}
				string source2 = signalDefinition.Offset.ToString(CultureInfo.CreateSpecificCulture("en-US"));
				if (source2.Contains('E'))
				{
					source2 = LTLUtil.GetFloatingPointString(signalDefinition.Offset);
				}
				stringBuilder.AppendFormat("{{ Signal \"{0}\" ({1}): }}", signalBaseName, signalDefinition.IsMotorola ? "Motorola" : "Intel");
				stringBuilder.AppendLine();
				stringBuilder.AppendFormat("VAR {0} = DIAG {1} [{2}] ", text, serviceQualifier, signalPosition);
				ltlDiagSignal = stringBuilder;
			}
			return LTLGenerator.LTLGenerationResult.OK;
		}

		private static string GetFloatingPointString(double value)
		{
			string text = value.ToString(CultureInfo.CreateSpecificCulture("en-US"));
			Regex regex = new Regex("(-)?(\\d)+(\\.\\d+)?E(-?\\d+)");
			Match match = regex.Match(text);
			if (match.Success)
			{
				string arg_41_0 = match.Groups[1].Value;
				string value2 = match.Groups[2].Value;
				string text2 = match.Groups[3].Value.TrimStart(new char[]
				{
					'.'
				});
				string value3 = match.Groups[4].Value;
				int num = Convert.ToInt32(value3);
				int num2;
				if (value3.StartsWith("-"))
				{
					num2 = 1 + Math.Abs(num) + text2.Length;
				}
				else
				{
					num2 = value2.Length + num;
				}
				return value.ToString(string.Format("F{0:D}", num2 + 5), CultureInfo.CreateSpecificCulture("en-US"));
			}
			return text;
		}

		public static string GetFormattedHeaderComment(string headerComment)
		{
			StringBuilder stringBuilder = new StringBuilder("{-------------------- " + headerComment + " ");
			if (stringBuilder.Length < 80)
			{
				stringBuilder.Append('-', 80 - stringBuilder.Length);
				stringBuilder.Append(" }");
			}
			else
			{
				stringBuilder.Append(" -----}");
			}
			return stringBuilder.ToString();
		}

		public static string GetFormattedSubsectionHeaderComment(string headerComment)
		{
			return "{---- " + headerComment + " ----}";
		}

		public static string GetCANErrorrateVariableName(uint channel)
		{
			return string.Format("CAN{0:D}Errorrate", channel);
		}

		public static void AppendTextWithLineLengthLimit(ref StringBuilder builder, string text, int maxLineLength, int indentLength)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(' ', indentLength);
			string str = stringBuilder.ToString();
			int num = 0;
			int num2 = maxLineLength - indentLength;
			while (text.Length - num > num2)
			{
				int i = text.IndexOf('\n', num, num2);
				if (i >= 0)
				{
					builder.Append(str + text.Substring(num, i - num + 1));
					num = i + 1;
					if (num + 1 >= text.Length)
					{
						return;
					}
				}
				else
				{
					int num3 = text.LastIndexOfAny(LTLUtil.WhiteSpaceChars, num + num2 - 1, num2);
					if (num3 < 0)
					{
						num3 = text.IndexOfAny(LTLUtil.WhiteSpaceChars, num);
						if (num3 < 0)
						{
							builder.AppendLine(str + text.Substring(num));
							return;
						}
					}
					while (num3 + 1 < text.Length && char.IsWhiteSpace(text[num3 + 1]))
					{
						num3++;
					}
					if (num3 + 1 < text.Length)
					{
						num3++;
					}
					builder.AppendLine(str + text.Substring(num, num3 - num));
					num = num3;
				}
			}
			if (num + 1 < text.Length)
			{
				for (int i = text.IndexOf('\n', num); i >= 0; i = text.IndexOf('\n', num))
				{
					builder.Append(str + text.Substring(num, i - num + 1));
					num = i + 1;
					if (num + 1 >= text.Length)
					{
						return;
					}
				}
				builder.AppendLine(str + text.Substring(num));
			}
		}

		public static string AdjustStringLength(string text, int length)
		{
			if (text.Length > length)
			{
				return text.Substring(0, length);
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(text);
			stringBuilder.Append(' ', length - text.Length);
			return stringBuilder.ToString();
		}

		public static string GetSignAlignedText(string text, char sign)
		{
			return LTLUtil.GetSignAlignedText(text, sign.ToString());
		}

		public static string GetSignAlignedText(string text, string sign)
		{
			StringBuilder stringBuilder = new StringBuilder();
			string[] array = text.Split(new char[]
			{
				'\n'
			});
			if (array.Length <= 0)
			{
				return text;
			}
			int num = 0;
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string text2 = array2[i];
				int num2 = text2.IndexOf(sign);
				if (num2 > num)
				{
					num = num2;
				}
			}
			uint num3 = 0u;
			string[] array3 = array;
			for (int j = 0; j < array3.Length; j++)
			{
				string text3 = array3[j];
				StringBuilder stringBuilder2 = new StringBuilder(text3);
				int num4 = text3.IndexOf(sign);
				if (num4 > 0 && num4 < num)
				{
					stringBuilder2.Insert(num4 - 1, " ", num - num4);
				}
				num3 += 1u;
				if ((ulong)num3 < (ulong)((long)array.Length) || text3.Length >= 1)
				{
					stringBuilder.AppendLine(stringBuilder2.ToString().Trim(new char[]
					{
						'\n',
						'\r'
					}));
				}
			}
			return stringBuilder.ToString();
		}

		public static bool GetLinChipConfigFromLdfDatabase(DatabaseConfiguration dbConfig, IApplicationDatabaseManager appDbManager, string configFolderPath, uint channelNr, out int baudrate, out string protocol, out int numDatabases)
		{
			baudrate = 0;
			protocol = string.Empty;
			IList<Database> databases = dbConfig.GetDatabases(BusType.Bt_LIN, channelNr);
			numDatabases = databases.Count;
			if (numDatabases != 1)
			{
				return false;
			}
			string absolutePath = FileSystemServices.GetAbsolutePath(databases.First<Database>().FilePath.Value, configFolderPath);
			return appDbManager.GetLinChipConfigFromLdfDatabase(absolutePath, out baudrate, out protocol);
		}
	}
}
