using System;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.CaplGeneration.DataPresenter
{
	internal class DpHardwareChannelLin : DpHardwareChannel
	{
		public LINChannel LinChannel
		{
			get;
			private set;
		}

		public bool UseManualConfigValues
		{
			get
			{
				return !this.LinChannel.UseDbConfigValues.Value;
			}
		}

		public uint Baudrate
		{
			get
			{
				return this.LinChannel.SpeedRate.Value;
			}
		}

		public int ProtocolVerison
		{
			get
			{
				return this.LinChannel.ProtocolVersion.Value;
			}
		}

		public override string NameOfCaplEventHandler
		{
			get
			{
				return CaplHelper.MakeLinMsgHandlerString(this.ChannelString);
			}
		}

		public override string ForwardEventFunctionCall
		{
			get
			{
				return "logWriteEvent(this)";
			}
		}

		public DpHardwareChannelLin(LINChannel channel, uint channelNumber) : base(channelNumber)
		{
			this.LinChannel = channel;
		}
	}
}
