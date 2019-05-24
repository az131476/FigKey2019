using System;

namespace Vector.VLConfig.HardwareAccess.ToolInterfaces
{
	public interface IDoubleProgressIndicator : IProgressIndicator
	{
		void SetMasterMinMax(int min, int max);

		void SetMasterValue(int value);

		void SetMasterStatusText(string text);
	}
}
