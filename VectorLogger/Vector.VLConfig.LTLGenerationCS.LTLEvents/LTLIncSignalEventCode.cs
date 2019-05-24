using System;
using System.IO;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.LTLGenerationCS.LTLEvents
{
	public class LTLIncSignalEventCode : LTLGenericEventCode
	{
		private IncEvent mIncEvent;

		public LTLIncSignalEventCode(string eventFlagName, IncEvent incEvent) : base(eventFlagName)
		{
			this.mIncEvent = incEvent;
		}

		public override LTLGenerator.LTLGenerationResult GenerateCode(out string ltlCode)
		{
			string eventFlagNameWithoutDot = base.GetEventFlagNameWithoutDot();
			string preEventCode = string.Empty;
			string text = this.mIncEvent.LtlName + this.mIncEvent.InstanceValue.Value;
			switch (this.mIncEvent.Relation.Value)
			{
			case CondRelation.Equal:
			case CondRelation.NotEqual:
			case CondRelation.LessThan:
			case CondRelation.LessThanOrEqual:
			case CondRelation.GreaterThan:
			case CondRelation.GreaterThanOrEqual:
			{
				string text2;
				if (!LTLUtil.GetLTLCompareOperatorString(this.mIncEvent.Relation.Value, out text2))
				{
					ltlCode = "";
					return LTLGenerator.LTLGenerationResult.TriggerError;
				}
				preEventCode = string.Format("FLAG {0}_Condition = ({1} {2} {3})", new object[]
				{
					eventFlagNameWithoutDot,
					text,
					text2,
					this.mIncEvent.LowValue.Value
				});
				goto IL_193;
			}
			case CondRelation.InRange:
			{
				string arg = string.Format("{0} >= {1:D}", text, this.mIncEvent.LowValue.Value);
				string arg2 = string.Format("{0} <= {1:D}", text, this.mIncEvent.HighValue.Value);
				preEventCode = string.Format("FLAG {0}_Condition = ({1} AND {2})", eventFlagNameWithoutDot, arg, arg2);
				goto IL_193;
			}
			case CondRelation.NotInRange:
			{
				string arg3 = string.Format("{0} < {1:D}", text, this.mIncEvent.LowValue.Value);
				string arg4 = string.Format("{0} > {1:D}", text, this.mIncEvent.HighValue.Value);
				preEventCode = string.Format("FLAG {0}_Condition = ({1}) OR ({2})", eventFlagNameWithoutDot, arg3, arg4);
				goto IL_193;
			}
			}
			ltlCode = "";
			return LTLGenerator.LTLGenerationResult.TriggerError;
			IL_193:
			base.PreEventCode = preEventCode;
			base.TriggerEvent = string.Format("SET ({0}_Condition)", eventFlagNameWithoutDot);
			base.AdditionalCodeBeforeFlagSet = null;
			base.AdditionalCodeAfterFlagSet = null;
			base.PostEventCode = null;
			ltlCode = base.BuildTriggerEventBlock();
			return LTLGenerator.LTLGenerationResult.OK;
		}

		protected override string GetComment()
		{
			return string.Format("Event on INC \"{0}\"{1} return value \"{2}\" {3}", new object[]
			{
				Path.GetFileName(this.mIncEvent.FilePath.Value),
				(this.mIncEvent.InstanceValue.Value.Length > 0) ? ("(" + this.mIncEvent.InstanceValue.Value + ")") : string.Empty,
				this.mIncEvent.LtlName,
				LTLGenericEventCode.GetCommentCompareOperatorStringForSignals(this.mIncEvent.Relation.Value, (double)this.mIncEvent.LowValue.Value, (double)this.mIncEvent.HighValue.Value)
			});
		}
	}
}
