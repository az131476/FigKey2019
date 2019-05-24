using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "Filter")]
	public abstract class Filter
	{
		[DataMember(Name = "FilterAction")]
		public ValidatedProperty<FilterActionType> Action
		{
			get;
			set;
		}

		[DataMember(Name = "FilterLimitIntervalPerFrame")]
		public ValidatedProperty<uint> LimitIntervalPerFrame
		{
			get;
			set;
		}

		[DataMember(Name = "FilterIsActive")]
		public ValidatedProperty<bool> IsActive
		{
			get;
			set;
		}

		public Filter()
		{
			this.Action = new ValidatedProperty<FilterActionType>(FilterActionType.Pass);
			this.LimitIntervalPerFrame = new ValidatedProperty<uint>(1u);
			this.IsActive = new ValidatedProperty<bool>(true);
		}
	}
}
