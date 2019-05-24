using System;
using System.Globalization;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.CaplGeneration.DataPresenter
{
	internal abstract class DpFilterConditionChannel : DpFilterCondition
	{
		public override string TemplateEventHandler
		{
			get
			{
				return "TEventHandler_Filter_Channel";
			}
		}

		public virtual string ChannelString
		{
			get
			{
				return this.Filter.ChannelNumber.Value.ToString(CultureInfo.InvariantCulture);
			}
		}

		public ChannelFilter Filter
		{
			get;
			private set;
		}

		protected DpFilterConditionChannel(ChannelFilter filter) : base(filter)
		{
			this.Filter = filter;
		}
	}
}
