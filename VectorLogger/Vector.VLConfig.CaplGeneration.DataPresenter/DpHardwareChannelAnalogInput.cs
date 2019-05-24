using System;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.CaplGeneration.DataPresenter
{
	internal class DpHardwareChannelAnalogInput : DpHardwareChannel
	{
		private bool isRecordingActive;

		public AnalogInput AnalogInput
		{
			get;
			private set;
		}

		public override string NameOfCaplEventHandler
		{
			get
			{
				return "on sysvar_update sysvar::IO::VN1600_1::AIN";
			}
		}

		public override string ForwardEventFunctionCall
		{
			get
			{
				if (this.isRecordingActive)
				{
					return string.Format("logWriteAnalogInput(timeNowInt64(), {0}, @this)", this.ChannelString);
				}
				return string.Empty;
			}
		}

		public DpHardwareChannelAnalogInput(AnalogInput channel, uint channelNumber, bool isRecordingActive) : base(channelNumber)
		{
			this.AnalogInput = channel;
			this.isRecordingActive = isRecordingActive;
		}
	}
}
