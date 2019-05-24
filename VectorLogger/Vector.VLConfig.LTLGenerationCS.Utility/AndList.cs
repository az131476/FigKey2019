using System;
using System.Linq;
using System.Text;

namespace Vector.VLConfig.LTLGenerationCS.Utility
{
	public class AndList : LogicalList
	{
		public AndList()
		{
		}

		public AndList(params string[] elements) : base(elements)
		{
		}

		public override string ToLTLCode()
		{
			if (this.list.Any<ILogicalListElement>())
			{
				bool flag = false;
				StringBuilder stringBuilder = new StringBuilder();
				foreach (ILogicalListElement current in this.list)
				{
					string text = current.ToLTL();
					if (stringBuilder.Length > 0)
					{
						text = string.Format(" AND {0}", text);
					}
					LogicalCondition.LogicalConditionType conditionType = current.GetConditionType();
					if (conditionType != LogicalCondition.LogicalConditionType.Variable)
					{
						text = string.Format(" {{ {0} }}", text);
						if (conditionType == LogicalCondition.LogicalConditionType.ConstFalse)
						{
							flag = true;
						}
					}
					stringBuilder.Append(text);
				}
				if (flag)
				{
					stringBuilder.Insert(0, " 0 { --- list of conditions is constantly false:   ");
					stringBuilder.Append(" --- }");
				}
				return string.Format("({0})", stringBuilder.ToString());
			}
			return string.Empty;
		}
	}
}
