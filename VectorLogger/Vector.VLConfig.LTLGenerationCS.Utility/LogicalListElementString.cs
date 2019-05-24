using System;

namespace Vector.VLConfig.LTLGenerationCS.Utility
{
	public class LogicalListElementString : ILogicalListElement
	{
		private string element;

		public LogicalListElementString(string element)
		{
			this.element = element;
		}

		public string ToLTL()
		{
			return this.element;
		}

		public LogicalCondition.LogicalConditionType GetConditionType()
		{
			return LogicalCondition.LogicalConditionType.Variable;
		}
	}
}
