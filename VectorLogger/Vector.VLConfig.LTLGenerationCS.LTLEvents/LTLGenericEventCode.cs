using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.LTLGenerationCS.LTLEvents
{
	public abstract class LTLGenericEventCode
	{
		private string comment;

		private string preEventCode;

		private string postEventCode;

		private string triggerEvent;

		private string whenCondition;

		private string additionalCodeBeforeFlagSet;

		private string additionalCodeAfterFlagSet;

		private bool isNeverActiveTrigger;

		protected string eventFlagName;

		private IList<string> blockConditions;

		private IList<string> externalCodeForEventBlockBeforeFlagSet;

		private IList<string> externalCodeForEventBlockAfterFlagSet;

		protected string Comment
		{
			get
			{
				return this.comment;
			}
			set
			{
				this.comment = value;
			}
		}

		protected string PreEventCode
		{
			get
			{
				return this.preEventCode;
			}
			set
			{
				this.preEventCode = value;
			}
		}

		protected string PostEventCode
		{
			get
			{
				return this.postEventCode;
			}
			set
			{
				this.postEventCode = value;
			}
		}

		protected string TriggerEvent
		{
			get
			{
				return this.triggerEvent;
			}
			set
			{
				this.triggerEvent = value;
			}
		}

		protected string WhenCondition
		{
			get
			{
				return this.whenCondition;
			}
			set
			{
				this.whenCondition = value;
			}
		}

		protected string AdditionalCodeBeforeFlagSet
		{
			get
			{
				return this.additionalCodeBeforeFlagSet;
			}
			set
			{
				this.additionalCodeBeforeFlagSet = value;
			}
		}

		protected string AdditionalCodeAfterFlagSet
		{
			get
			{
				return this.additionalCodeAfterFlagSet;
			}
			set
			{
				this.additionalCodeAfterFlagSet = value;
			}
		}

		public bool IsNeverActiveTrigger
		{
			get
			{
				return this.isNeverActiveTrigger;
			}
			protected set
			{
				this.isNeverActiveTrigger = value;
			}
		}

		public LTLGenericEventCode(string eventFlagName)
		{
			this.comment = null;
			this.preEventCode = null;
			this.postEventCode = null;
			this.triggerEvent = null;
			this.whenCondition = null;
			this.additionalCodeBeforeFlagSet = null;
			this.additionalCodeAfterFlagSet = null;
			this.eventFlagName = eventFlagName;
			this.isNeverActiveTrigger = false;
			this.blockConditions = new List<string>();
			this.externalCodeForEventBlockBeforeFlagSet = new List<string>();
			this.externalCodeForEventBlockAfterFlagSet = new List<string>();
		}

		public virtual void AddBlockCondition(string blockCondition)
		{
			this.blockConditions.Add(blockCondition);
		}

		public void AddExternalCodeForEventBlock(string ltlCode, bool beforeFlagSet = false)
		{
			if (beforeFlagSet)
			{
				this.externalCodeForEventBlockBeforeFlagSet.Add(ltlCode);
				return;
			}
			this.externalCodeForEventBlockAfterFlagSet.Add(ltlCode);
		}

		public virtual LTLGenerator.LTLGenerationResult GenerateCode(out string ltlCode)
		{
			ltlCode = this.BuildTriggerEventBlock();
			return LTLGenerator.LTLGenerationResult.TriggerError;
		}

		public virtual LTLGenerator.LTLGenerationResult GetCurrentStateFlag(out string ltlVariable)
		{
			ltlVariable = "";
			return LTLGenerator.LTLGenerationResult.TriggerError;
		}

		protected virtual string GetComment()
		{
			return string.Empty;
		}

		protected string BuildTriggerEventBlock()
		{
			StringBuilder stringBuilder = new StringBuilder();
			this.Comment = this.GetComment();
			if (this.Comment != null)
			{
				stringBuilder.AppendFormat("{{ {0} }}", this.Comment);
				stringBuilder.AppendLine();
			}
			if (this.PreEventCode != null)
			{
				stringBuilder.Append(this.PreEventCode);
				stringBuilder.AppendLine();
			}
			stringBuilder.AppendFormat("EVENT ON {0} BEGIN", this.TriggerEvent);
			stringBuilder.AppendLine();
			if (this.AdditionalCodeBeforeFlagSet != null)
			{
				stringBuilder.Append(this.AdditionalCodeBeforeFlagSet);
				stringBuilder.AppendLine();
			}
			if (this.externalCodeForEventBlockBeforeFlagSet.Any<string>())
			{
				stringBuilder.AppendLine();
				foreach (string current in this.externalCodeForEventBlockBeforeFlagSet)
				{
					stringBuilder.AppendLine(current);
				}
				stringBuilder.AppendLine();
			}
			stringBuilder.AppendFormat("  CALC {0} = (1) ", this.eventFlagName);
			string text = this.BuildTotalWhenCondition();
			if (text.Length > 0)
			{
				stringBuilder.AppendFormat("WHEN ({0})", text);
			}
			stringBuilder.AppendLine();
			if (this.AdditionalCodeAfterFlagSet != null)
			{
				stringBuilder.Append(this.AdditionalCodeAfterFlagSet);
				stringBuilder.AppendLine();
			}
			if (this.externalCodeForEventBlockAfterFlagSet.Any<string>())
			{
				stringBuilder.AppendLine();
				foreach (string current2 in this.externalCodeForEventBlockAfterFlagSet)
				{
					stringBuilder.AppendLine(current2);
				}
			}
			stringBuilder.AppendLine("END");
			if (this.PostEventCode != null)
			{
				stringBuilder.Append(this.PostEventCode);
				stringBuilder.AppendLine();
			}
			return stringBuilder.ToString();
		}

		private string BuildTotalWhenCondition()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.WhenCondition != null)
			{
				stringBuilder.Append(this.WhenCondition);
			}
			foreach (string current in this.blockConditions)
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.Append(" AND ");
				}
				stringBuilder.AppendFormat("NOT {0}", current);
			}
			return stringBuilder.ToString();
		}

		protected string GetEventFlagNameWithoutDot()
		{
			if (this.eventFlagName.Contains('.'))
			{
				string text = this.eventFlagName;
				return text.Replace('.', '_');
			}
			return this.eventFlagName;
		}

		protected static string GetCommentCompareOperatorStringForSignals(CondRelation condRelation, double lowValue, double highValue)
		{
			switch (condRelation)
			{
			case CondRelation.Equal:
				return string.Format("= {0}", lowValue);
			case CondRelation.NotEqual:
				return string.Format("!= {0}", lowValue);
			case CondRelation.LessThan:
				return string.Format("< {0}", lowValue);
			case CondRelation.LessThanOrEqual:
				return string.Format("<= {0}", lowValue);
			case CondRelation.GreaterThan:
				return string.Format("> {0}", lowValue);
			case CondRelation.GreaterThanOrEqual:
				return string.Format(">= {0}", lowValue);
			case CondRelation.InRange:
				return string.Format("IN RANGE [{0}; {1}]", lowValue, highValue);
			case CondRelation.NotInRange:
				return string.Format("OUT OF RANGE [{0}; {1}]", lowValue, highValue);
			case CondRelation.OnChange:
				return string.Format("on change", new object[0]);
			default:
				return "no compare";
			}
		}
	}
}
