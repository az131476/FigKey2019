using System;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.CaplGeneration.DataPresenter
{
	internal class DpFilterConditionSymbolicMessageCan : DpFilterConditionSymbolicMessage
	{
		public override string NameOfCaplEventHandler
		{
			get
			{
				return CaplHelper.MakeCanMsgHandlerString(base.ChannelString);
			}
		}

		public override string MsgSymbolString
		{
			get
			{
				return CaplHelper.MakeCanMsgSymbolString(base.ChannelString, base.Filter.MessageName.Value);
			}
		}

		public DpFilterConditionSymbolicMessageCan(SymbolicMessageFilter filter) : base(filter)
		{
		}
	}
}
