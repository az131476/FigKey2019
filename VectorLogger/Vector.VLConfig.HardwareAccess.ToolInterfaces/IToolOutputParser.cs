using System;

namespace Vector.VLConfig.HardwareAccess.ToolInterfaces
{
	public interface IToolOutputParser
	{
		bool IsWaitingForInput(string outputStringFromToolProcess, out int interactionPhase, ref int auxId, ref string auxString);

		void DoFeedUserInput(int interactionPhase, int auxId, string auxString);
	}
}
