using System;

namespace Vector.VLConfig.GUI
{
	public class PageTypeHelper
	{
		public static PageType GetTriggerPageForMemory(int memoryNr)
		{
			if (memoryNr > 1)
			{
				return PageType.Triggers2;
			}
			return PageType.Triggers1;
		}

		public static PageType GetFilterPageForMemory(int memoryNr)
		{
			if (memoryNr > 1)
			{
				return PageType.Filters2;
			}
			return PageType.Filters1;
		}
	}
}
