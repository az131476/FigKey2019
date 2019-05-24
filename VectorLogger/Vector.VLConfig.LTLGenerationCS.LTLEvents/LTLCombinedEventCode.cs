using System;
using System.Collections.Generic;
using System.Text;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.LTLGenerationCS.Utility;

namespace Vector.VLConfig.LTLGenerationCS.LTLEvents
{
	public class LTLCombinedEventCode : LTLGenericEventCode
	{
		private CombinedEvent combinedEvent;

		private IApplicationDatabaseManager dbManager;

		private string configurationFolderPath;

		private static List<int> currentLevel;

		private static string majorCheckGroupVar;

		private static string recursionMajorEventFlagName;

		private static Dictionary<string, string> subChecks;

		public static void StaticClear()
		{
			LTLCombinedEventCode.currentLevel = new List<int>();
			LTLCombinedEventCode.majorCheckGroupVar = string.Empty;
			LTLCombinedEventCode.recursionMajorEventFlagName = string.Empty;
			LTLCombinedEventCode.subChecks = new Dictionary<string, string>();
		}

		private static void RecursionAddLevel(string checkGroupVar)
		{
			LTLCombinedEventCode.currentLevel.Add(0);
			if (LTLCombinedEventCode.currentLevel.Count == 1)
			{
				LTLCombinedEventCode.majorCheckGroupVar = checkGroupVar;
			}
		}

		private static void RecursionRemoveLevel()
		{
			if (LTLCombinedEventCode.currentLevel.Count > 0)
			{
				LTLCombinedEventCode.currentLevel.RemoveAt(LTLCombinedEventCode.currentLevel.Count - 1);
			}
		}

		private static void RecursionSetStepInCurrentLevel(int i)
		{
			if (LTLCombinedEventCode.currentLevel.Count > 0)
			{
				LTLCombinedEventCode.currentLevel[LTLCombinedEventCode.currentLevel.Count - 1] = i;
			}
		}

		private static int RecursionGetStepInCurrentLevel()
		{
			if (LTLCombinedEventCode.currentLevel.Count > 0)
			{
				return LTLCombinedEventCode.currentLevel[LTLCombinedEventCode.currentLevel.Count - 1];
			}
			return 0;
		}

		private static string RecursionGetCurrentLevelAndStepString()
		{
			string text = "";
			foreach (int current in LTLCombinedEventCode.currentLevel)
			{
				text += string.Format(".{0:D}", current);
			}
			return text.TrimStart(new char[]
			{
				'.'
			});
		}

		private static bool RecursionIsCurrentTopLevel()
		{
			return LTLCombinedEventCode.currentLevel.Count == 0;
		}

		public static string RecursionGetMajorCheckGroupVar()
		{
			return LTLCombinedEventCode.majorCheckGroupVar;
		}

		public LTLCombinedEventCode(string eventFlagName, CombinedEvent ev, IApplicationDatabaseManager dbManager, string configurationFolderPath) : base(eventFlagName)
		{
			this.combinedEvent = ev;
			this.dbManager = dbManager;
			this.configurationFolderPath = configurationFolderPath;
		}

		public override LTLGenerator.LTLGenerationResult GenerateCode(out string ltlCode)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("VAR {0} = FREE[1]", this.GetCheckGroupVarName());
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			IList<string> list = new List<string>();
			LogicalList logicalList;
			if (this.combinedEvent.IsConjunction.Value)
			{
				logicalList = new AndList();
			}
			else
			{
				logicalList = new OrList();
			}
			bool arg_62_0 = this.combinedEvent.IsPointInTime.Value;
			bool value = this.combinedEvent.IsConjunction.Value;
			bool arg_84_0 = this.combinedEvent.IsConjunction.Value;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			if (LTLCombinedEventCode.RecursionIsCurrentTopLevel())
			{
				LTLCombinedEventCode.StaticClear();
				LTLCombinedEventCode.recursionMajorEventFlagName = this.eventFlagName;
			}
			LTLCombinedEventCode.RecursionAddLevel(this.GetCheckGroupVarName());
			foreach (Event current in this.combinedEvent)
			{
				num++;
				LTLCombinedEventCode.RecursionSetStepInCurrentLevel(num);
				if (!this.combinedEvent.ChildIsActive(current))
				{
					stringBuilder.AppendFormat("{{ - Sub-Condition {0}: deactiveated - }}", LTLCombinedEventCode.RecursionGetCurrentLevelAndStepString());
					stringBuilder.AppendLine();
					stringBuilder.AppendLine();
				}
				else
				{
					stringBuilder.AppendFormat("{{ - Sub-Condition {0}: - }}", LTLCombinedEventCode.RecursionGetCurrentLevelAndStepString());
					stringBuilder.AppendLine();
					string text = string.Format("{0}_Sub_{1}", LTLCombinedEventCode.recursionMajorEventFlagName, LTLCombinedEventCode.RecursionGetCurrentLevelAndStepString().Replace('.', '_'));
					stringBuilder.AppendFormat("VAR {0} = FREE[1]", text);
					stringBuilder.AppendLine();
					LTLGenericEventCode lTLGenericEventCode;
					LTLEventFactory.GetLtlEventCodeObject(current, text, this.dbManager, this.configurationFolderPath, out lTLGenericEventCode);
					if (!(current is CombinedEvent) && current.IsPointInTime.Value)
					{
						string text2 = string.Format("{0}_Sub_{1}_State", LTLCombinedEventCode.recursionMajorEventFlagName, LTLCombinedEventCode.RecursionGetCurrentLevelAndStepString().Replace('.', '_'));
						stringBuilder.AppendFormat("VAR {0} = FREE[1]", text2);
						stringBuilder.AppendLine();
						lTLGenericEventCode.AddExternalCodeForEventBlock(string.Format("  CALC {0} = (1)", text2), true);
						lTLGenericEventCode.AddExternalCodeForEventBlock(string.Format("  CALC {0} = (1) WHEN ({1})", LTLCombinedEventCode.majorCheckGroupVar, text), false);
						lTLGenericEventCode.AddExternalCodeForEventBlock(string.Format("  CALC {0} = (0)", text), false);
						lTLGenericEventCode.AddExternalCodeForEventBlock(string.Format("  CALC {0} = (0)", text2), false);
						logicalList.Add(text2);
						num3++;
					}
					else
					{
						lTLGenericEventCode.AddExternalCodeForEventBlock(string.Format("  CALC {0} = (1) WHEN ({1})", LTLCombinedEventCode.majorCheckGroupVar, text), false);
						lTLGenericEventCode.AddExternalCodeForEventBlock(string.Format("  CALC {0} = (0)", text), false);
					}
					string value2;
					LTLGenerator.LTLGenerationResult lTLGenerationResult;
					if ((lTLGenerationResult = lTLGenericEventCode.GenerateCode(out value2)) != LTLGenerator.LTLGenerationResult.OK)
					{
						ltlCode = string.Empty;
						LTLGenerator.LTLGenerationResult result = lTLGenerationResult;
						return result;
					}
					if (!(current is CombinedEvent) && !current.IsPointInTime.Value)
					{
						string empty = string.Empty;
						if ((lTLGenerationResult = lTLGenericEventCode.GetCurrentStateFlag(out empty)) != LTLGenerator.LTLGenerationResult.OK)
						{
							ltlCode = string.Empty;
							LTLGenerator.LTLGenerationResult result = lTLGenerationResult;
							return result;
						}
						logicalList.Add(empty);
						num2++;
					}
					stringBuilder.Append(value2);
					stringBuilder.AppendLine();
					if (lTLGenericEventCode is LTLCombinedEventCode)
					{
						string checkGroupVarName = (lTLGenericEventCode as LTLCombinedEventCode).GetCheckGroupVarName();
						list.Add(checkGroupVarName);
					}
				}
			}
			LTLCombinedEventCode.RecursionRemoveLevel();
			if (LTLCombinedEventCode.RecursionIsCurrentTopLevel())
			{
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("{- Combination of sub conditions: -}");
			}
			base.PreEventCode = stringBuilder.ToString();
			base.TriggerEvent = string.Format("CALC ({0} = (1))", this.GetCheckGroupVarName());
			StringBuilder stringBuilder2 = new StringBuilder();
			if (!LTLCombinedEventCode.RecursionIsCurrentTopLevel())
			{
				StringBuilder stringBuilder3 = new StringBuilder();
				string text3 = LTLCombinedEventCode.recursionMajorEventFlagName + "_Sub_" + LTLCombinedEventCode.RecursionGetCurrentLevelAndStepString() + "_calc";
				stringBuilder3.AppendFormat("  VAR  {0} = FREE[1]", text3);
				stringBuilder3.AppendLine();
				stringBuilder3.AppendFormat("  CALC {0} = {1}", text3, logicalList.ToLTLCode());
				LTLCombinedEventCode.subChecks.Add(text3, stringBuilder3.ToString());
			}
			else
			{
				foreach (KeyValuePair<string, string> current2 in LTLCombinedEventCode.subChecks)
				{
					stringBuilder2.AppendLine(current2.Value);
					logicalList.Add(current2.Key);
				}
				stringBuilder2.AppendFormat("  VAR  {0} = FREE[1]", this.GetCurrentStateVar());
				stringBuilder2.AppendLine();
				stringBuilder2.AppendFormat("  CALC {0} = {1}", this.GetCurrentStateVar(), logicalList.ToLTLCode());
			}
			base.AdditionalCodeBeforeFlagSet = stringBuilder2.ToString();
			if (value)
			{
				base.WhenCondition = string.Format("{0}", this.GetCurrentStateVar());
			}
			else
			{
				base.WhenCondition = null;
			}
			StringBuilder stringBuilder4 = new StringBuilder();
			stringBuilder4.AppendLine();
			stringBuilder4.AppendFormat("  CALC {0} = (0)", LTLCombinedEventCode.majorCheckGroupVar);
			base.AdditionalCodeAfterFlagSet = stringBuilder4.ToString();
			base.PostEventCode = null;
			if (!LTLCombinedEventCode.RecursionIsCurrentTopLevel())
			{
				StringBuilder stringBuilder5 = new StringBuilder();
				base.Comment = this.GetComment();
				stringBuilder5.AppendFormat("{{ {0} }}", base.Comment);
				stringBuilder5.AppendLine();
				stringBuilder5.AppendLine(base.PreEventCode);
				ltlCode = stringBuilder5.ToString();
			}
			else
			{
				ltlCode = base.BuildTriggerEventBlock();
			}
			return LTLGenerator.LTLGenerationResult.OK;
		}

		protected override string GetComment()
		{
			if (this.combinedEvent.IsConjunction.Value)
			{
				return "Event on AND group";
			}
			return "Event on OR group";
		}

		public override LTLGenerator.LTLGenerationResult GetCurrentStateFlag(out string ltlVariable)
		{
			ltlVariable = this.GetLastStateVar();
			return LTLGenerator.LTLGenerationResult.OK;
		}

		private string GetCheckGroupVarName()
		{
			return string.Format("Check_Group_{0}", base.GetEventFlagNameWithoutDot());
		}

		private string GetLastStateVar()
		{
			return string.Format("{0}_LastState", base.GetEventFlagNameWithoutDot());
		}

		private string GetCurrentStateVar()
		{
			return string.Format("{0}_CurrentState", base.GetEventFlagNameWithoutDot());
		}
	}
}
