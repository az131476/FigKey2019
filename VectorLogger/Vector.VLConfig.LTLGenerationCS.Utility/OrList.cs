using System;
using System.Linq;
using System.Text;

namespace Vector.VLConfig.LTLGenerationCS.Utility
{
	public class OrList : LogicalList
	{
		public OrList()
		{
		}

		public OrList(params string[] elements) : base(elements)
		{
		}

		public override string ToLTLCode()
		{
			if (this.list.Any<ILogicalListElement>())
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (ILogicalListElement current in this.list)
				{
					string text = string.Format("({0})", current.ToLTL());
					if (stringBuilder.Length > 0)
					{
						text = string.Format(" OR {0}", text);
					}
					LogicalCondition.LogicalConditionType conditionType = current.GetConditionType();
					if (conditionType != LogicalCondition.LogicalConditionType.Variable)
					{
						text = string.Format(" {{ {0} }} ", text);
					}
					stringBuilder.Append(text);
				}
				return stringBuilder.ToString();
			}
			return string.Empty;
		}
	}
}
