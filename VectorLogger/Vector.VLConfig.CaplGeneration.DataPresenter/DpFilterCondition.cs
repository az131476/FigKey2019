using System;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.CaplGeneration.DataPresenter
{
	internal abstract class DpFilterCondition
	{
		public uint Pass
		{
			get;
			private set;
		}

		public abstract string NameOfCaplEventHandler
		{
			get;
		}

		public abstract string TemplateEventHandler
		{
			get;
		}

		protected DpFilterCondition(Filter filter)
		{
			this.Pass = ((filter.Action.Value == FilterActionType.Pass) ? 1u : 0u);
		}

		public static DpFilterCondition Create(Filter filter)
		{
			DpFilterCondition result = null;
			if (filter is ChannelFilter)
			{
				ChannelFilter channelFilter = filter as ChannelFilter;
				if (channelFilter.BusType.Value == BusType.Bt_CAN)
				{
					result = new DpFilterConditionChannelCan(channelFilter);
				}
				else if (channelFilter.BusType.Value == BusType.Bt_LIN)
				{
					result = new DpFilterConditionChannelLin(channelFilter);
				}
			}
			else if (filter is CANIdFilter)
			{
				result = new DpFilterConditionCanId(filter as CANIdFilter);
			}
			else if (filter is LINIdFilter)
			{
				result = new DpFilterConditionLinId(filter as LINIdFilter);
			}
			else if (filter is SymbolicMessageFilter)
			{
				SymbolicMessageFilter symbolicMessageFilter = filter as SymbolicMessageFilter;
				if (symbolicMessageFilter.BusType.Value == BusType.Bt_CAN)
				{
					result = new DpFilterConditionSymbolicMessageCan(symbolicMessageFilter);
				}
				if (symbolicMessageFilter.BusType.Value == BusType.Bt_LIN)
				{
					result = new DpFilterConditionSymbolicMessageLin(symbolicMessageFilter);
				}
			}
			return result;
		}
	}
}
