using System;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.LTLGenerationCS
{
	internal class PseudoActionTrigger : Vector.VLConfig.Data.ConfigurationDataModel.Action
	{
		private RecordTrigger trigger;

		private int memoryNumber;

		public RecordTrigger Trigger
		{
			get
			{
				return this.trigger;
			}
		}

		public int MemoryNumber
		{
			get
			{
				return this.memoryNumber;
			}
		}

		public PseudoActionTrigger(RecordTrigger trigger, int memoryNumber)
		{
			this.Event = trigger.Event;
			this.IsActive = trigger.IsActive;
			this.Comment = trigger.Comment;
			this.StopType = new StopImmediate();
			this.trigger = trigger;
			this.memoryNumber = memoryNumber;
		}
	}
}
