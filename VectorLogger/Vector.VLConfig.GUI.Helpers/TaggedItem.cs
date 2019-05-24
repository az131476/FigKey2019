using System;

namespace Vector.VLConfig.GUI.Helpers
{
	public class TaggedItem<T>
	{
		public string Name;

		public T Tag;

		public TaggedItem(string name, T tag)
		{
			this.Name = name;
			this.Tag = tag;
		}

		public override string ToString()
		{
			return this.Name;
		}
	}
}
