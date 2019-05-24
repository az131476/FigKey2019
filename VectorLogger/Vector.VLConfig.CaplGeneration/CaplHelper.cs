using System;
using System.Globalization;
using System.Text;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.CaplGeneration
{
	internal class CaplHelper
	{
		public static string MakeConditionString(CondRelation relation, string variableString, string valueLowString, string valueHighString = null)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append("(");
			string text = CaplHelper.OperatorString(relation);
			if (text != null)
			{
				stringBuilder.Append(variableString);
				stringBuilder.Append(text);
				stringBuilder.Append(valueLowString);
			}
			else if (relation == CondRelation.InRange || relation == CondRelation.NotInRange)
			{
				stringBuilder.Append(variableString);
				if (relation == CondRelation.InRange)
				{
					stringBuilder.Append(">=");
				}
				else if (relation == CondRelation.NotInRange)
				{
					stringBuilder.Append("<");
				}
				stringBuilder.Append(valueLowString);
				if (relation == CondRelation.InRange)
				{
					stringBuilder.Append(" && ");
				}
				else if (relation == CondRelation.NotInRange)
				{
					stringBuilder.Append(" || ");
				}
				stringBuilder.Append(variableString);
				if (relation == CondRelation.InRange)
				{
					stringBuilder.Append("<=");
				}
				else if (relation == CondRelation.NotInRange)
				{
					stringBuilder.Append(">");
				}
				stringBuilder.Append(valueHighString);
			}
			else if (relation == CondRelation.OnChange)
			{
				stringBuilder.Append(variableString);
			}
			else
			{
				stringBuilder.Append("# unknown CondRelation #");
			}
			stringBuilder.Append(")");
			return stringBuilder.ToString();
		}

		public static string MakeConditionStringCanId(CondRelation relation, string variableString, uint valueLow, uint valueHigh, bool isExtendedId)
		{
			string valueLowString;
			string valueHighString;
			if (isExtendedId)
			{
				valueLowString = "mkExtId(" + valueLow.ToString(CultureInfo.InvariantCulture) + ")";
				valueHighString = "mkExtId(" + valueHigh.ToString(CultureInfo.InvariantCulture) + ")";
			}
			else
			{
				valueLowString = valueLow.ToString(CultureInfo.InvariantCulture);
				valueHighString = valueHigh.ToString(CultureInfo.InvariantCulture);
			}
			return CaplHelper.MakeConditionString(relation, variableString, valueLowString, valueHighString);
		}

		public static string MakeConditionStringLinId(CondRelation relation, string variableString, uint valueLow, uint valueHigh)
		{
			string valueLowString = valueLow.ToString(CultureInfo.InvariantCulture);
			string valueHighString = valueHigh.ToString(CultureInfo.InvariantCulture);
			return CaplHelper.MakeConditionString(relation, variableString, valueLowString, valueHighString);
		}

		public static string OperatorString(CondRelation relation)
		{
			switch (relation)
			{
			case CondRelation.Equal:
				return "==";
			case CondRelation.NotEqual:
				return "!=";
			case CondRelation.LessThan:
				return "<";
			case CondRelation.LessThanOrEqual:
				return "<=";
			case CondRelation.GreaterThan:
				return ">";
			case CondRelation.GreaterThanOrEqual:
				return ">=";
			default:
				return null;
			}
		}

		public static string MakeCanMsgHandlerString(string channelNumberString)
		{
			return string.Format("on message CAN{0}.[*]", channelNumberString);
		}

		public static string MakeLinMsgHandlerString(string channelNumberString)
		{
			return string.Format("on linmessage LIN{0}.*", channelNumberString);
		}

		public static string MakeCanMsgSymbolString(string channelNumberString, string messageName)
		{
			return string.Format("CAN{0}::{1}", channelNumberString, messageName);
		}

		public static string MakeLinMsgSymbolString(string channelNumberString, string messageName)
		{
			return string.Format("LIN{0}::{1}", channelNumberString, messageName);
		}
	}
}
