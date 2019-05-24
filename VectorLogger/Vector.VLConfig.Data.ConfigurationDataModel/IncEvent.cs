using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "IncEvent")]
	public class IncEvent : Event
	{
		private ValidatedProperty<bool> mIsPointInTime;

		private string mLtlName;

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

		[DataMember(Name = "IncEventFilePath")]
		public ValidatedProperty<string> FilePath
		{
			get;
			set;
		}

		[DataMember(Name = "IncEventInstanceValue")]
		public ValidatedProperty<string> InstanceValue
		{
			get;
			set;
		}

		[DataMember(Name = "IncEventParamIndex")]
		public ValidatedProperty<int> ParamIndex
		{
			get;
			set;
		}

		public string LtlName
		{
			get
			{
				return this.mLtlName ?? string.Empty;
			}
			set
			{
				this.mLtlName = (value ?? string.Empty);
			}
		}

		[DataMember(Name = "IncEventRelation")]
		public ValidatedProperty<CondRelation> Relation
		{
			get;
			set;
		}

		[DataMember(Name = "IncEventLowValue")]
		public ValidatedProperty<int> LowValue
		{
			get;
			set;
		}

		[DataMember(Name = "IncEventHighValue")]
		public ValidatedProperty<int> HighValue
		{
			get;
			set;
		}

		public IncEvent()
		{
			this.FilePath = new ValidatedProperty<string>(string.Empty);
			this.InstanceValue = new ValidatedProperty<string>(string.Empty);
			this.ParamIndex = new ValidatedProperty<int>(0);
			this.Relation = new ValidatedProperty<CondRelation>(CondRelation.Equal);
			this.LowValue = new ValidatedProperty<int>(0);
			this.HighValue = new ValidatedProperty<int>(0);
		}

		public IncEvent(IncEvent other) : this()
		{
			this.Assign(other);
		}

		public override object Clone()
		{
			return new IncEvent(this);
		}

		public void Reset()
		{
			this.FilePath.Value = string.Empty;
			this.InstanceValue.Value = string.Empty;
			this.ParamIndex.Value = 0;
			this.Relation.Value = CondRelation.Equal;
			this.LowValue.Value = 0;
			this.HighValue.Value = 0;
		}

		public override bool Equals(object obj)
		{
			if (obj == null || base.GetType() != obj.GetType())
			{
				return false;
			}
			IncEvent incEvent = obj as IncEvent;
			return incEvent != null && this.FilePath.Value.Equals(incEvent.FilePath.Value, StringComparison.OrdinalIgnoreCase) && this.InstanceValue.Value.Equals(incEvent.InstanceValue.Value, StringComparison.OrdinalIgnoreCase) && this.ParamIndex.Value.Equals(incEvent.ParamIndex.Value) && this.Relation.Value.Equals(incEvent.Relation.Value) && this.LowValue.Value.Equals(incEvent.LowValue.Value) && this.HighValue.Value.Equals(incEvent.HighValue.Value);
		}

		public override int GetHashCode()
		{
			return this.FilePath.Value.GetHashCode() ^ this.InstanceValue.Value.GetHashCode() ^ this.ParamIndex.Value.GetHashCode() ^ this.Relation.Value.GetHashCode() ^ this.LowValue.Value.GetHashCode() ^ this.HighValue.Value.GetHashCode();
		}

		public void Assign(IncEvent other)
		{
			this.FilePath.Value = other.FilePath.Value;
			this.InstanceValue.Value = other.InstanceValue.Value;
			this.ParamIndex.Value = other.ParamIndex.Value;
			this.Relation.Value = other.Relation.Value;
			this.LowValue.Value = other.LowValue.Value;
			this.HighValue.Value = other.HighValue.Value;
		}
	}
}
