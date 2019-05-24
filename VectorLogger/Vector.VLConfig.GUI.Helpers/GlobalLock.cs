using System;

namespace Vector.VLConfig.GUI.Helpers
{
	internal class GlobalLock : IDisposable
	{
		private static bool sGlobalLock;

		public static bool IsActive
		{
			get
			{
				return GlobalLock.sGlobalLock;
			}
		}

		public static GlobalLock Activate()
		{
			if (!GlobalLock.sGlobalLock)
			{
				return new GlobalLock();
			}
			return null;
		}

		private GlobalLock()
		{
			GlobalLock.sGlobalLock = true;
		}

		public void Dispose()
		{
			GlobalLock.sGlobalLock = false;
		}
	}
}
