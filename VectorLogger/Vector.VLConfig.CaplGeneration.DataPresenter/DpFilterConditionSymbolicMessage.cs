using System;
using System.Globalization;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.CaplGeneration.DataPresenter
{
	internal abstract class DpFilterConditionSymbolicMessage : DpFilterCondition
	{
		public override string TemplateEventHandler
		{
			get
			{
				return "TEventHandler_Filter_MsgId";
			}
		}

		public abstract string MsgSymbolString
		{
			get;
		}

		public SymbolicMessageFilter Filter
		{
			get;
			private set;
		}

		public string ChannelString
		{
			get
			{
				return this.Filter.ChannelNumber.Value.ToString(CultureInfo.InvariantCulture);
			}
		}

		public string MsgConditionString
		{
			get
			{
				return CaplHelper.MakeConditionString(CondRelation.Equal, "this.ID", this.MsgSymbolString + ".ID", null);
			}
		}

		protected DpFilterConditionSymbolicMessage(SymbolicMessageFilter filter) : base(filter)
		{
			this.Filter = filter;
		}
	}
}
