using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.DBAccess;
using Vector.VLConfig.LoggerSpecifics;
using Vector.VLConfig.LTLGenerationCS.LTLEvents;

namespace Vector.VLConfig.LTLGenerationCS
{
	internal abstract class LTLGenericAction : LTLGenericCodePart
	{
		protected enum resetType
		{
			never,
			onSeparateEvent,
			onReset,
			afterTime
		}

		protected ILoggerSpecifics loggerSpecifics;

		protected IApplicationDatabaseManager databaseManager;

		protected string configurationFolderPath;

		protected bool usesUserComment;

		protected virtual string SectionHeaderComment
		{
			get
			{
				return "";
			}
		}

		public LTLGenericAction(ILoggerSpecifics loggerSpecifics, IApplicationDatabaseManager databaseManager, string configurationFolderPath)
		{
			this.loggerSpecifics = loggerSpecifics;
			this.databaseManager = databaseManager;
			this.configurationFolderPath = configurationFolderPath;
			this.usesUserComment = true;
		}

		public LTLGenerator.LTLGenerationResult GenerateLTLActionCode()
		{
			base.LtlCode = new StringBuilder();
			if (!this.IsFeatureSupportedByLogger())
			{
				return LTLGenerator.LTLGenerationResult.OK;
			}
			string text;
			if (this.IsNoActionAvtive(out text))
			{
				return LTLGenerator.LTLGenerationResult.OK;
			}
			base.LtlCode.AppendLine();
			base.LtlCode.AppendLine(LTLUtil.GetFormattedHeaderComment(this.SectionHeaderComment));
			base.LtlCode.AppendLine();
			StringBuilder stringBuilder = new StringBuilder();
			string value;
			LTLGenerator.LTLGenerationResult result;
			if ((result = this.BuildCodeBeforeActions(out value)) != LTLGenerator.LTLGenerationResult.OK)
			{
				return result;
			}
			base.LtlCode.Append(value);
			base.LtlCode.AppendLine();
			List<Vector.VLConfig.Data.ConfigurationDataModel.Action> actionList = this.GetActionList();
			bool flag = true;
			int i = 0;
			for (i = 1; i <= actionList.Count; i++)
			{
				Vector.VLConfig.Data.ConfigurationDataModel.Action action = actionList[i - 1];
				if (!action.IsActive.Value)
				{
					string text2;
					this.BuildInactiveActionCode(action, i, out text2);
					if (text2.Length > 0)
					{
						stringBuilder.AppendLine(text2);
					}
				}
				else
				{
					if (!flag)
					{
						stringBuilder.AppendLine();
						stringBuilder.AppendLine();
					}
					flag = false;
					Event @event = action.Event;
					this.GetResetType(action.StopType);
					stringBuilder.Append(LTLUtil.GetFormattedSubsectionHeaderComment(this.GetActionItemSubSectionComment(action, i)));
					stringBuilder.AppendLine();
					if (this.usesUserComment)
					{
						stringBuilder.Append("{ User comment: ");
						if (action.Comment.Value != null)
						{
							if (action.Comment.Value.IndexOf("\n") >= 0)
							{
								stringBuilder.AppendLine();
							}
							stringBuilder.AppendFormat("{0}", action.Comment.Value);
						}
						stringBuilder.AppendLine(" }");
					}
					string optionalCodeBeforeEvent = this.GetOptionalCodeBeforeEvent(action, i);
					if (optionalCodeBeforeEvent.Length > 0)
					{
						stringBuilder.AppendLine(optionalCodeBeforeEvent);
					}
					stringBuilder.AppendFormat("VAR {0} = FREE[1]", this.GetSetVariableName(i));
					stringBuilder.AppendLine();
					LTLGenericEventCode lTLGenericEventCode;
					LTLEventFactory.GetLtlEventCodeObject(@event, this.GetSetVariableName(i), this.databaseManager, this.configurationFolderPath, out lTLGenericEventCode);
					IList<string> blockConditions = this.GetBlockConditions(action, i);
					if (blockConditions.Any<string>())
					{
						foreach (string current in blockConditions)
						{
							lTLGenericEventCode.AddBlockCondition(current);
						}
					}
					string value2;
					if ((result = lTLGenericEventCode.GenerateCode(out value2)) != LTLGenerator.LTLGenerationResult.OK)
					{
						return result;
					}
					stringBuilder.AppendLine(value2);
					if (action.StopType == null)
					{
						stringBuilder.AppendFormat("FLAG {0}  SET = ({1})", this.GetSetResetFlagName(i), this.GetSetVariableName(i));
					}
					else if (!(action.StopType is StopImmediate))
					{
						if (action.StopType is StopOnEvent)
						{
							StopOnEvent stopOnEvent = action.StopType as StopOnEvent;
							Event event2 = stopOnEvent.Event;
							LTLGenericEventCode lTLGenericEventCode2;
							LTLEventFactory.GetLtlEventCodeObject(event2, this.GetResetVariableName(i), this.databaseManager, this.configurationFolderPath, out lTLGenericEventCode2);
							string value3;
							if ((result = lTLGenericEventCode2.GenerateCode(out value3)) != LTLGenerator.LTLGenerationResult.OK)
							{
								return result;
							}
							stringBuilder.AppendFormat("VAR {0} = FREE[1]", this.GetResetVariableName(i));
							stringBuilder.AppendLine();
							stringBuilder.AppendLine(value3);
							stringBuilder.AppendFormat("VAR {0} = FREE[1]", this.GetSetResetFlagName(i));
							stringBuilder.AppendLine();
							stringBuilder.AppendFormat("EVENT ON SET ({0}) BEGIN", this.GetSetVariableName(i));
							stringBuilder.AppendLine();
							stringBuilder.AppendFormat("  CALC {0} = (1)", this.GetSetResetFlagName(i));
							stringBuilder.AppendLine();
							stringBuilder.AppendFormat("       {0} = (0)", this.GetSetVariableName(i));
							stringBuilder.AppendLine();
							stringBuilder.AppendFormat("       {0} = (0)", this.GetResetVariableName(i));
							stringBuilder.AppendLine();
							stringBuilder.AppendLine("END");
							stringBuilder.AppendFormat("EVENT ON SET ({0}) BEGIN", this.GetResetVariableName(i));
							stringBuilder.AppendLine();
							stringBuilder.AppendFormat("  CALC {0} = (0)", this.GetSetResetFlagName(i));
							stringBuilder.AppendLine();
							stringBuilder.AppendFormat("       {0} = (0)", this.GetSetVariableName(i));
							stringBuilder.AppendLine();
							stringBuilder.AppendFormat("       {0} = (0)", this.GetResetVariableName(i));
							stringBuilder.AppendLine();
							stringBuilder.AppendLine("END");
						}
						else if (action.StopType is StopOnStartEvent)
						{
							string arg;
							if ((result = lTLGenericEventCode.GetCurrentStateFlag(out arg)) != LTLGenerator.LTLGenerationResult.OK)
							{
								return result;
							}
							stringBuilder.AppendFormat("FLAG {0}  = ({1})", this.GetSetResetFlagName(i), arg);
						}
						else
						{
							if (!(action.StopType is StopOnTimer))
							{
								return LTLGenerator.LTLGenerationResult.ActionsDigitalOutputError;
							}
							StopOnTimer stopOnTimer = action.StopType as StopOnTimer;
							stringBuilder.AppendFormat("TIMER {0} TIME = {1:D} ({2})", this.GetResetVariableName(i), stopOnTimer.Duration.Value + this.GetAdditionalStopTimerDuration(action), this.GetSetVariableName(i));
							stringBuilder.AppendLine();
							stringBuilder.AppendFormat("FLAG {0}  SET = ({1} AND NOT {2})  RESET = ({2})", this.GetSetResetFlagName(i), this.GetSetVariableName(i), this.GetResetVariableName(i));
							stringBuilder.AppendLine();
							stringBuilder.AppendFormat("EVENT ON RESET ({0}) BEGIN", this.GetSetResetFlagName(i));
							stringBuilder.AppendLine();
							stringBuilder.AppendFormat("  CALC {0} = (0)", this.GetSetVariableName(i));
							stringBuilder.AppendLine();
							stringBuilder.AppendLine("END");
						}
					}
					string text3;
					if ((result = this.BuildActionCode(action, i, out text3)) != LTLGenerator.LTLGenerationResult.OK)
					{
						return result;
					}
					if (text3.Length > 0)
					{
						stringBuilder.Append(text3);
					}
				}
			}
			base.LtlCode.Append(stringBuilder);
			base.LtlCode.AppendLine();
			base.LtlCode.AppendLine();
			string text4;
			if ((result = this.BuildCodeAfterActions(out text4)) != LTLGenerator.LTLGenerationResult.OK)
			{
				return result;
			}
			if (text4.Length > 0)
			{
				base.LtlCode.Append(text4);
				base.LtlCode.AppendLine();
				base.LtlCode.AppendLine();
			}
			return LTLGenerator.LTLGenerationResult.OK;
		}

		protected virtual bool IsFeatureSupportedByLogger()
		{
			return false;
		}

		protected virtual bool IsNoActionAvtive(out string textForComment)
		{
			textForComment = string.Empty;
			return true;
		}

		protected virtual List<Vector.VLConfig.Data.ConfigurationDataModel.Action> GetActionList()
		{
			return new List<Vector.VLConfig.Data.ConfigurationDataModel.Action>();
		}

		protected virtual string GetSetVariableName(int index)
		{
			return "";
		}

		protected virtual string GetResetVariableName(int index)
		{
			return "";
		}

		protected virtual string GetSetResetFlagName(int index)
		{
			return "";
		}

		protected virtual string GetActionItemSubSectionComment(Vector.VLConfig.Data.ConfigurationDataModel.Action action, int index)
		{
			return "";
		}

		protected virtual LTLGenerator.LTLGenerationResult BuildCodeBeforeActions(out string code)
		{
			code = string.Empty;
			return LTLGenerator.LTLGenerationResult.ActionError;
		}

		protected virtual string GetOptionalCodeBeforeEvent(Vector.VLConfig.Data.ConfigurationDataModel.Action action, int index)
		{
			return string.Empty;
		}

		protected virtual LTLGenerator.LTLGenerationResult BuildActionCode(Vector.VLConfig.Data.ConfigurationDataModel.Action action, int index, out string code)
		{
			code = string.Empty;
			return LTLGenerator.LTLGenerationResult.ActionError;
		}

		protected virtual IList<string> GetBlockConditions(Vector.VLConfig.Data.ConfigurationDataModel.Action action, int index)
		{
			return new List<string>();
		}

		protected virtual LTLGenerator.LTLGenerationResult BuildInactiveActionCode(Vector.VLConfig.Data.ConfigurationDataModel.Action action, int index, out string code)
		{
			code = string.Empty;
			return LTLGenerator.LTLGenerationResult.ActionError;
		}

		protected virtual LTLGenerator.LTLGenerationResult BuildCodeAfterActions(out string code)
		{
			code = string.Empty;
			return LTLGenerator.LTLGenerationResult.ActionError;
		}

		protected virtual uint GetAdditionalStopTimerDuration(Vector.VLConfig.Data.ConfigurationDataModel.Action action)
		{
			return 0u;
		}

		protected string GetResetTypeStringForComment(LTLGenericAction.resetType resetType)
		{
			string result = "";
			switch (resetType)
			{
			case LTLGenericAction.resetType.never:
				result = "reset never";
				break;
			case LTLGenericAction.resetType.onSeparateEvent:
				result = "reset on separate event";
				break;
			case LTLGenericAction.resetType.onReset:
				result = "reset when SET event is reset";
				break;
			case LTLGenericAction.resetType.afterTime:
				result = "reset after timeout";
				break;
			}
			return result;
		}

		protected LTLGenericAction.resetType GetResetType(StopType stopType)
		{
			if (stopType is StopOnTimer)
			{
				return LTLGenericAction.resetType.afterTime;
			}
			if (stopType is StopImmediate)
			{
				return LTLGenericAction.resetType.never;
			}
			if (stopType is StopOnStartEvent)
			{
				return LTLGenericAction.resetType.onReset;
			}
			if (stopType is StopOnEvent)
			{
				return LTLGenericAction.resetType.onSeparateEvent;
			}
			return LTLGenericAction.resetType.never;
		}
	}
}
