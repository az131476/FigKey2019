using System;

namespace Vector.VLConfig.GUI
{
	public class UpdateContextHelper
	{
		public static UpdateContext GetFilterUpdateContextForForMemoryNr(int memNr)
		{
			if (memNr == 2)
			{
				return UpdateContext.Filters2;
			}
			return UpdateContext.Filters1;
		}

		public static UpdateContext GetTriggerUpdateContextForMemoryNr(int memNr)
		{
			if (memNr == 2)
			{
				return UpdateContext.Triggers2;
			}
			return UpdateContext.Triggers1;
		}
	}
}
