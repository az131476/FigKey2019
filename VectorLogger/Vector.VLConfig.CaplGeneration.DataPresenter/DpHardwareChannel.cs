using System;
using System.Globalization;

namespace Vector.VLConfig.CaplGeneration.DataPresenter
{
	internal abstract class DpHardwareChannel
	{
		public abstract string NameOfCaplEventHandler
		{
			get;
		}

		public abstract string ForwardEventFunctionCall
		{
			get;
		}

		public virtual string ChannelString
		{
			get;
			private set;
		}

		protected DpHardwareChannel(uint channelNumber)
		{
			this.ChannelString = channelNumber.ToString(CultureInfo.InvariantCulture);
		}
	}
}
