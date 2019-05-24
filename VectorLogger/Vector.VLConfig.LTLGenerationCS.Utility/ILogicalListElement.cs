using System;

namespace Vector.VLConfig.LTLGenerationCS.Utility
{
	public interface ILogicalListElement
	{
		string ToLTL();

		LogicalCondition.LogicalConditionType GetConditionType();
	}
}
