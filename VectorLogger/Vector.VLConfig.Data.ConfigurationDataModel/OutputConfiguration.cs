using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "OutputConfiguration")]
	public class OutputConfiguration
	{
		[DataMember(Name = "OutputConfigurationLEDConfiguration")]
		private LEDConfiguration ledConfiguration;

		[DataMember(Name = "OutputConfigurationSendMessageConfiguration")]
		private SendMessageConfiguration sendMessageConfiguration;

		[DataMember(Name = "OutputConfigurationDigitalOutputsConfiguration")]
		private DigitalOutputsConfiguration digitalOutputsConfiguration;

		public LEDConfiguration LEDConfiguration
		{
			get
			{
				if (this.ledConfiguration == null)
				{
					this.ledConfiguration = new LEDConfiguration();
				}
				return this.ledConfiguration;
			}
		}

		public SendMessageConfiguration SendMessageConfiguration
		{
			get
			{
				if (this.sendMessageConfiguration == null)
				{
					this.sendMessageConfiguration = new SendMessageConfiguration();
				}
				return this.sendMessageConfiguration;
			}
		}

		public DigitalOutputsConfiguration DigitalOutputsConfiguration
		{
			get
			{
				if (this.digitalOutputsConfiguration == null)
				{
					this.digitalOutputsConfiguration = new DigitalOutputsConfiguration();
				}
				return this.digitalOutputsConfiguration;
			}
		}
	}
}
