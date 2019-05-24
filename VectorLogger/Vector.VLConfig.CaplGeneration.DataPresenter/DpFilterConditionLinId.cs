using System;
using System.Globalization;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.CaplGeneration.DataPresenter
{
	internal class DpFilterConditionLinId : DpFilterCondition
	{
		public override string NameOfCaplEventHandler
		{
			get
			{
				return CaplHelper.MakeLinMsgHandlerString(this.ChannelString);
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

		public LINIdFilter Filter
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
				return CaplHelper.MakeConditionStringLinId(relation, "this.ID", this.Filter.LINId.Value, this.Filter.LINIdLast.Value);
			}
		}

		public DpFilterConditionLinId(LINIdFilter filter) : base(filter)
		{
			this.Filter = filter;
		}
	}
}
