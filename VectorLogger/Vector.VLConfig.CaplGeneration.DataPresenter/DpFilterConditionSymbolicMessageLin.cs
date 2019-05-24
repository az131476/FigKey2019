using System;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.CaplGeneration.DataPresenter
{
	internal class DpFilterConditionSymbolicMessageLin : DpFilterConditionSymbolicMessage
	{
		public override string NameOfCaplEventHandler
		{
			get
			{
				return CaplHelper.MakeLinMsgHandlerString(base.ChannelString);
			}
		}

		public override string MsgSymbolString
		{
			get
			{
				return CaplHelper.MakeLinMsgSymbolString(base.ChannelString, base.Filter.MessageName.Value);
			}
		}

		public DpFilterConditionSymbolicMessageLin(SymbolicMessageFilter filter) : base(filter)
		{
		}
	}
}
