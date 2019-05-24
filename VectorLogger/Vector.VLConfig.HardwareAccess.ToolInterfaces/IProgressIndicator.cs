using System;

namespace Vector.VLConfig.HardwareAccess.ToolInterfaces
{
	public interface IProgressIndicator
	{
		void Activate();

		void Deactivate();

		void SetMinMax(int min, int max);

		void SetValue(int value);

		void SetStatusText(string text);

		bool Cancelled();
	}
}
