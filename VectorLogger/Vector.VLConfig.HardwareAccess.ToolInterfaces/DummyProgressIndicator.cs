using System;

namespace Vector.VLConfig.HardwareAccess.ToolInterfaces
{
	internal class DummyProgressIndicator : IProgressIndicator
	{
		public void Activate()
		{
		}

		public void Deactivate()
		{
		}

		public void SetMinMax(int min, int max)
		{
		}

		public void SetValue(int value)
		{
		}

		public void SetStatusText(string text)
		{
		}

		public bool Cancelled()
		{
			return true;
		}
	}
}
