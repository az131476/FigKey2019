using System;
using System.Globalization;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.CaplGeneration.DataPresenter
{
	internal class DpFilterConditionCanId : DpFilterCondition
	{
		public override string NameOfCaplEventHandler
		{
			get
			{
				return CaplHelper.MakeCanMsgHandlerString(this.ChannelString);
			}
		}

		public override string TemplateEventHandler
		{
			get
			{
				return "TEventHandler_Filter_MsgId";
			}
		}

		public virtual string ChannelString
		{
			get
			{
				return this.Filter.ChannelNumber.Value.ToString(CultureInfo.InvariantCulture);
			}
		}

		public CANIdFilter Filter
		{
			get;
			private set;
		}

		public string MsgConditionString
		{
			get
			{
				CondRelation relation = CondRelation.Equal;
				if (this.Filter.IsIdRange.Value)
				{
					relation = CondRelation.InRange;
				}
				return CaplHelper.MakeConditionStringCanId(relation, "this.ID", this.Filter.CANId.Value, this.Filter.CANIdLast.Value, this.Filter.IsExtendedId.Value);
			}
		}

		public DpFilterConditionCanId(CANIdFilter filter) : base(filter)
		{
			this.Filter = filter;
		}
	}
}
