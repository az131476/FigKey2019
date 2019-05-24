using System;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.LTLGenerationCS.Utility
{
	public class LogicalCondition
	{
		public enum LogicalConditionType
		{
			Variable,
			ConstTrue,
			ConstFalse
		}

		private Variable variable;

		private CondRelation relation;

		private int compareValue;

		public Variable Variable
		{
			get
			{
				return this.variable;
			}
			set
			{
				this.variable = value;
			}
		}

		public CondRelation CompareRelation
		{
			get
			{
				return this.relation;
			}
			set
			{
				this.relation = value;
			}
		}

		public int CompareValue
		{
			get
			{
				return this.compareValue;
			}
			set
			{
				this.compareValue = value;
			}
		}

		public LogicalCondition()
		{
		}

		public LogicalCondition(Variable variable, CondRelation relation, int compareValue)
		{
			this.Variable = variable;
			this.CompareRelation = relation;
			this.CompareValue = compareValue;
		}

		public string GetLTLCode()
		{
			if (this.Variable == null || this.Variable.Name.Length <= 0)
			{
				return "0 {{missing input!?}}";
			}
			string arg;
			if (!LTLUtil.GetLTLCompareOperatorString(this.CompareRelation, out arg))
			{
				return "0 {{unsupported operator!?}}";
			}
			string text = string.Format("{0} {1} {2:D}", this.Variable.Name, arg, this.CompareValue);
			switch (this.GetConditionType())
			{
			case LogicalCondition.LogicalConditionType.ConstTrue:
				text = string.Format("1 {{ {0}  is constantly true}}", text);
				break;
			case LogicalCondition.LogicalConditionType.ConstFalse:
				text = string.Format("0 {{ {0}  is constantly false}}", text);
				break;
			}
			return text;
		}

		public LogicalCondition.LogicalConditionType GetConditionType()
		{
			if ((this.CompareRelation == CondRelation.LessThan && this.CompareValue <= this.Variable.GetMinValue()) || (this.CompareRelation == CondRelation.LessThanOrEqual && this.CompareValue <= this.Variable.GetMinValue() - 1) || (this.CompareRelation == CondRelation.GreaterThan && this.CompareValue >= this.Variable.GetMaxValue()) || (this.CompareRelation == CondRelation.GreaterThanOrEqual && this.CompareValue >= this.Variable.GetMaxValue() + 1))
			{
				return LogicalCondition.LogicalConditionType.ConstFalse;
			}
			if ((this.CompareRelation == CondRelation.LessThan && this.CompareValue >= this.Variable.GetMaxValue() + 1) || (this.CompareRelation == CondRelation.LessThanOrEqual && this.CompareValue >= this.Variable.GetMaxValue()) || (this.CompareRelation == CondRelation.GreaterThan && this.CompareValue <= this.Variable.GetMinValue() - 1) || (this.CompareRelation == CondRelation.GreaterThanOrEqual && this.CompareValue <= this.Variable.GetMinValue()))
			{
				return LogicalCondition.LogicalConditionType.ConstTrue;
			}
			return LogicalCondition.LogicalConditionType.Variable;
		}
	}
}
