using System;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.CaplGeneration.DataPresenter
{
	internal class DpFilterConditionChannelCan : DpFilterConditionChannel
	{
		public override string NameOfCaplEventHandler
		{
			get
			{
				return CaplHelper.MakeCanMsgHandlerString(this.ChannelString);
			}
		}

		public DpFilterConditionChannelCan(ChannelFilter filter) : base(filter)
		{
		}
	}
}
