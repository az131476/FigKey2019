using System;

namespace Vector.VLConfig.Data.ApplicationData
{
	public class MessageDefinition
	{
		private uint canDbMessageId;

		private bool isCyclic;

		private int cycleTime;

		private int frBaseCycle;

		private int frCycleRepetition;

		private uint dlc;

		private string name;

		private string namePrefix = string.Empty;

		private static readonly uint ExtendedIdBitMask = 2147483648u;

		public uint CanDbMessageId
		{
			get
			{
				return this.canDbMessageId;
			}
			set
			{
				this.canDbMessageId = value;
			}
		}

		public int FrBaseCycle
		{
			get
			{
				return this.frBaseCycle;
			}
			set
			{
				this.frBaseCycle = value;
			}
		}

		public int FrCycleRepetition
		{
			get
			{
				return this.frCycleRepetition;
			}
			set
			{
				this.frCycleRepetition = value;
			}
		}

		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		public string NamePrefix
		{
			get
			{
				return this.namePrefix;
			}
			set
			{
				this.namePrefix = value;
			}
		}

		public uint ActualMessageId
		{
			get
			{
				return this.canDbMessageId & ~MessageDefinition.ExtendedIdBitMask;
			}
		}

		public bool IsExtendedId
		{
			get
			{
				return (this.canDbMessageId & MessageDefinition.ExtendedIdBitMask) != 0u;
			}
		}

		public bool IsCyclic
		{
			get
			{
				return this.isCyclic;
			}
			set
			{
				this.isCyclic = value;
			}
		}

		public int CycleTime
		{
			get
			{
				return this.cycleTime;
			}
			set
			{
				this.cycleTime = value;
			}
		}

		public uint DLC
		{
			get
			{
				return this.dlc;
			}
			set
			{
				this.dlc = value;
			}
		}

		public MessageDefinition(uint canDbMessageId, int frBaseCycle, int frCycleRepetition)
		{
			this.canDbMessageId = canDbMessageId;
			this.frBaseCycle = frBaseCycle;
			this.frCycleRepetition = frCycleRepetition;
			this.name = "";
			this.isCyclic = false;
			this.cycleTime = 0;
			this.dlc = 0u;
		}

		public MessageDefinition(uint canDbMessageId)
		{
			this.canDbMessageId = canDbMessageId;
			this.frBaseCycle = 0;
			this.frCycleRepetition = 1;
			this.cycleTime = 0;
			this.dlc = 0u;
		}

		public MessageDefinition(uint actualMessageId, bool isExtended)
		{
			this.canDbMessageId = actualMessageId;
			if (isExtended)
			{
				this.canDbMessageId |= MessageDefinition.ExtendedIdBitMask;
			}
			this.cycleTime = 0;
		}
	}
}
