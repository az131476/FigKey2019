using System;
using System.Collections.Generic;
using System.Linq;

namespace Vector.VLConfig.LTLGenerationCS.Utility
{
	public abstract class LogicalList
	{
		protected IList<ILogicalListElement> list;

		public LogicalList()
		{
			this.list = new List<ILogicalListElement>();
		}

		public LogicalList(params string[] elements)
		{
			for (int i = 0; i < elements.Length; i++)
			{
				string element = elements[i];
				this.Add(element);
			}
		}

		public void Add(string element)
		{
			this.list.Add(new LogicalListElementString(element));
		}

		public void Add(LogicalCondition logicalCondition)
		{
			this.list.Add(new LogicalListElementCondition(logicalCondition));
		}

		public void Clear()
		{
			this.list.Clear();
		}

		public int Count()
		{
			if (this.list == null)
			{
				return 0;
			}
			return this.list.Count<ILogicalListElement>();
		}

		public virtual string ToLTLCode()
		{
			throw new NotImplementedException();
		}
	}
}
