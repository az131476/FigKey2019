using System;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.CaplGeneration.DataPresenter
{
	internal class DpHardwareChannelDigitalInput : DpHardwareChannel
	{
		private bool isRecordingActive;

		public DigitalInput DigitalInput
		{
			get;
			private set;
		}

		public override string NameOfCaplEventHandler
		{
			get
			{
				return string.Format("on sysvar_update sysvar::IO::VN1600_1::DIN{0}", this.ChannelString);
			}
		}

		public override string ForwardEventFunctionCall
		{
			get
			{
				if (this.isRecordingActive)
				{
					return string.Format("logWriteDigitalInput(timeNowInt64(), {0}, @this)", this.ChannelString);
				}
				return string.Empty;
			}
		}

		public DpHardwareChannelDigitalInput(DigitalInput channel, uint channelNumber, bool isRecordingActive) : base(channelNumber)
		{
			this.DigitalInput = channel;
			this.isRecordingActive = isRecordingActive;
		}
	}
}
