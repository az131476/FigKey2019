using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "ReferencedRecordTriggerNameEvent")]
	public class ReferencedRecordTriggerNameEvent : Event
	{
		public static readonly string WildcardTriggerNameOnMemory = "*{0}";

		private ValidatedProperty<bool> mIsPointInTime;

		[DataMember(Name = "ReferencedRecordTriggerNameEventNameOfTrigger")]
		public ValidatedProperty<string> NameOfTrigger
		{
			get;
			set;
		}

		public override ValidatedProperty<bool> IsPointInTime
		{
			get
			{
				ValidatedProperty<bool> arg_1C_0;
				if ((arg_1C_0 = this.mIsPointInTime) == null)
				{
					arg_1C_0 = (this.mIsPointInTime = new ValidatedProperty<bool>(true));
				}
				return arg_1C_0;
			}
		}

		public ulong UniqueIdOfTrigger
		{
			get;
			set;
		}

		public static bool IsWildcardTriggerNameOnMemory(string wildcardTriggerNameStr, out uint memoryNr)
		{
			memoryNr = 0u;
			if (string.IsNullOrEmpty(wildcardTriggerNameStr))
			{
				return false;
			}
			if (wildcardTriggerNameStr.Length > 1 && wildcardTriggerNameStr[0] == '*')
			{
				string s = wildcardTriggerNameStr.Substring(1);
				uint num = 0u;
				if (uint.TryParse(s, out num))
				{
					memoryNr = num;
					return true;
				}
			}
			return false;
		}

		public ReferencedRecordTriggerNameEvent()
		{
			this.NameOfTrigger = new ValidatedProperty<string>(string.Format(ReferencedRecordTriggerNameEvent.WildcardTriggerNameOnMemory, 1));
			this.UniqueIdOfTrigger = 0uL;
		}

		public ReferencedRecordTriggerNameEvent(ReferencedRecordTriggerNameEvent other) : this()
		{
			this.Assign(other);
		}

		public override object Clone()
		{
			return new ReferencedRecordTriggerNameEvent(this);
		}

		public override bool Equals(object obj)
		{
			if (obj == null || base.GetType() != obj.GetType())
			{
				return false;
			}
			ReferencedRecordTriggerNameEvent referencedRecordTriggerNameEvent = (ReferencedRecordTriggerNameEvent)obj;
			return !(this.NameOfTrigger.Value != referencedRecordTriggerNameEvent.NameOfTrigger.Value);
		}

		public override int GetHashCode()
		{
			return this.NameOfTrigger.Value.GetHashCode();
		}

		public void Assign(ReferencedRecordTriggerNameEvent other)
		{
			this.NameOfTrigger.Value = other.NameOfTrigger.Value;
			this.UniqueIdOfTrigger = other.UniqueIdOfTrigger;
		}
	}
}
