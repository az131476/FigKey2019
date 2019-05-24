using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "VoCanRecordingEvent")]
	public class VoCanRecordingEvent : Event
	{
		private ValidatedProperty<bool> mIsPointInTime;

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

		[DataMember(Name = "VoCanRecordingEventIsUsingCASM2T3L")]
		public ValidatedProperty<bool> IsUsingCASM2T3L
		{
			get;
			set;
		}

		[DataMember(Name = "VoCanRecordingEventIsFixedRecordDuration")]
		public ValidatedProperty<bool> IsFixedRecordDuration
		{
			get;
			set;
		}

		[DataMember(Name = "VoCanRecordingEventDuration")]
		public ValidatedProperty<uint> Duration_s
		{
			get;
			set;
		}

		[DataMember(Name = "VoCanRecordingEventIsBeepOnEndOn")]
		public ValidatedProperty<bool> IsBeepOnEndOn
		{
			get;
			set;
		}

		[DataMember(Name = "VoCanRecordingEventIsRecordingLEDActive")]
		public ValidatedProperty<bool> IsRecordingLEDActive
		{
			get;
			set;
		}

		public VoCanRecordingEvent()
		{
			this.IsUsingCASM2T3L = new ValidatedProperty<bool>(false);
			this.IsFixedRecordDuration = new ValidatedProperty<bool>(false);
			this.Duration_s = new ValidatedProperty<uint>(5u);
			this.IsBeepOnEndOn = new ValidatedProperty<bool>(true);
			this.IsRecordingLEDActive = new ValidatedProperty<bool>(true);
		}

		public VoCanRecordingEvent(VoCanRecordingEvent other) : this()
		{
			this.Assign(other);
		}

		public override object Clone()
		{
			return new VoCanRecordingEvent(this);
		}

		public override bool Equals(object obj)
		{
			if (obj == null || base.GetType() != obj.GetType())
			{
				return false;
			}
			VoCanRecordingEvent voCanRecordingEvent = (VoCanRecordingEvent)obj;
			return this.IsUsingCASM2T3L.Value == voCanRecordingEvent.IsUsingCASM2T3L.Value && this.IsFixedRecordDuration.Value == voCanRecordingEvent.IsFixedRecordDuration.Value && this.Duration_s.Value == voCanRecordingEvent.Duration_s.Value && this.IsBeepOnEndOn.Value == voCanRecordingEvent.IsBeepOnEndOn.Value && this.IsRecordingLEDActive.Value == voCanRecordingEvent.IsRecordingLEDActive.Value;
		}

		public override int GetHashCode()
		{
			return this.IsUsingCASM2T3L.Value.GetHashCode() ^ this.IsFixedRecordDuration.Value.GetHashCode() ^ this.Duration_s.Value.GetHashCode() ^ this.IsBeepOnEndOn.Value.GetHashCode() ^ this.IsRecordingLEDActive.Value.GetHashCode();
		}

		public void Assign(VoCanRecordingEvent other)
		{
			this.IsUsingCASM2T3L.Value = other.IsUsingCASM2T3L.Value;
			this.IsFixedRecordDuration.Value = other.IsFixedRecordDuration.Value;
			this.Duration_s.Value = other.Duration_s.Value;
			this.IsBeepOnEndOn.Value = other.IsBeepOnEndOn.Value;
			this.IsRecordingLEDActive.Value = other.IsRecordingLEDActive.Value;
		}
	}
}
