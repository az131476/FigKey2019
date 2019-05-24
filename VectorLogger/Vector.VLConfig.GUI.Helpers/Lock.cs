using System;
using System.Collections.Generic;

namespace Vector.VLConfig.GUI.Helpers
{
	internal class Lock : IDisposable
	{
		private static readonly Dictionary<object, Lock> sLockMap = new Dictionary<object, Lock>();

		private readonly object mLockedObject;

		public static bool IsActive(object obj)
		{
			return GlobalLock.IsActive || obj == null || Lock.sLockMap.ContainsKey(obj);
		}

		public static Lock Activate(object obj)
		{
			if (obj == null)
			{
				return null;
			}
			if (GlobalLock.IsActive)
			{
				return null;
			}
			if (Lock.sLockMap.ContainsKey(obj))
			{
				return null;
			}
			Lock @lock = new Lock(obj);
			Lock.sLockMap.Add(obj, @lock);
			return @lock;
		}

		private static void Remove(object obj)
		{
			if (obj != null && Lock.sLockMap.ContainsKey(obj))
			{
				Lock.sLockMap.Remove(obj);
			}
		}

		private Lock(object obj)
		{
			this.mLockedObject = obj;
		}

		public void Dispose()
		{
			Lock.Remove(this.mLockedObject);
		}
	}
}
