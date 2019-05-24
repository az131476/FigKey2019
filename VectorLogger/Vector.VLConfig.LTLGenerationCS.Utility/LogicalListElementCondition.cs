using System;

namespace Vector.VLConfig.LTLGenerationCS.Utility
{
	public class LogicalListElementCondition : ILogicalListElement
	{
		private LogicalCondition logicalCondition;

		public LogicalListElementCondition(LogicalCondition logicalCondition)
		{
			this.logicalCondition = logicalCondition;
		}

		public string ToLTL()
		{
			return this.logicalCondition.GetLTLCode();
		}

		public LogicalCondition.LogicalConditionType GetConditionType()
		{
			return this.logicalCondition.GetConditionType();
		}
	}
}
