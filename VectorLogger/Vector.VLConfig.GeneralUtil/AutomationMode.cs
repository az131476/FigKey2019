using System;

namespace Vector.VLConfig.GeneralUtil
{
	public class AutomationMode : IDisposable
	{
		private bool mPreviousValue;

		public static bool IsActive
		{
			get;
			private set;
		}

		public AutomationMode()
		{
			this.mPreviousValue = AutomationMode.IsActive;
			AutomationMode.IsActive = true;
		}

		public void Dispose()
		{
			AutomationMode.IsActive = this.mPreviousValue;
		}
	}
}
