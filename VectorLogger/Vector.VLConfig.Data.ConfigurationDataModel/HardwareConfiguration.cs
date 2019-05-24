using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "HardwareConfiguration")]
	public class HardwareConfiguration
	{
		[DataMember(Name = "HardwareConfigurationCANChannelConfiguration")]
		private CANChannelConfiguration canChannelConfiguration;

		[DataMember(Name = "HardwareConfigurationLINChannelConfiguration")]
		private LINChannelConfiguration linChannelConfiguration;

		[DataMember(Name = "HardwareConfigurationFlexrayChannelConfiguration")]
		private FlexrayChannelConfiguration flexrayChannelConfiguration;

		[DataMember(Name = "HardwareConfigurationMOST150ChannelConfiguration")]
		private MOST150ChannelConfiguration most150ChannelConfiguration;

		[DataMember(Name = "HardwareConfigurationMultibusChannelConfiguration")]
		private MultibusChannelConfiguration multibusChannelConfiguration;

		[DataMember(Name = "HardwareConfigurationLogDataStorage")]
		private LogDataStorage logDataStorage;

		[DataMember(Name = "HardwareConfigurationAnalogInputConfiguration")]
		private AnalogInputConfiguration analogInputConfiguration;

		[DataMember(Name = "HardwareConfigurationDigitalInputConfiguration")]
		private DigitalInputConfiguration digitalInputConfiguration;

		[DataMember(Name = "HardwareConfigurationGPSConfiguration")]
		private GPSConfiguration gpsConfiguration;

		[DataMember(Name = "HardwareConfigurationInterfaceModeConfiguration")]
		private InterfaceModeConfiguration interfaceModeConfiguration;

		[DataMember(Name = "HardwareConfigurationEthernetConfiguration")]
		private EthernetConfiguration ethernetConfiguration;

		public CANChannelConfiguration CANChannelConfiguration
		{
			get
			{
				if (this.canChannelConfiguration == null)
				{
					this.canChannelConfiguration = new CANChannelConfiguration();
				}
				return this.canChannelConfiguration;
			}
		}

		public LINChannelConfiguration LINChannelConfiguration
		{
			get
			{
				if (this.linChannelConfiguration == null)
				{
					this.linChannelConfiguration = new LINChannelConfiguration();
				}
				return this.linChannelConfiguration;
			}
		}

		public FlexrayChannelConfiguration FlexrayChannelConfiguration
		{
			get
			{
				if (this.flexrayChannelConfiguration == null)
				{
					this.flexrayChannelConfiguration = new FlexrayChannelConfiguration();
				}
				return this.flexrayChannelConfiguration;
			}
		}

		public MOST150ChannelConfiguration MOST150ChannelConfiguration
		{
			get
			{
				if (this.most150ChannelConfiguration == null)
				{
					this.most150ChannelConfiguration = new MOST150ChannelConfiguration();
				}
				return this.most150ChannelConfiguration;
			}
		}

		public MultibusChannelConfiguration MultibusChannelConfiguration
		{
			get
			{
				if (this.multibusChannelConfiguration == null)
				{
					this.multibusChannelConfiguration = new MultibusChannelConfiguration();
				}
				return this.multibusChannelConfiguration;
			}
		}

		public LogDataStorage LogDataStorage
		{
			get
			{
				if (this.logDataStorage == null)
				{
					this.logDataStorage = new LogDataStorage();
				}
				return this.logDataStorage;
			}
		}

		public AnalogInputConfiguration AnalogInputConfiguration
		{
			get
			{
				if (this.analogInputConfiguration == null)
				{
					this.analogInputConfiguration = new AnalogInputConfiguration();
				}
				return this.analogInputConfiguration;
			}
		}

		public DigitalInputConfiguration DigitalInputConfiguration
		{
			get
			{
				if (this.digitalInputConfiguration == null)
				{
					this.digitalInputConfiguration = new DigitalInputConfiguration();
				}
				return this.digitalInputConfiguration;
			}
		}

		public GPSConfiguration GPSConfiguration
		{
			get
			{
				if (this.gpsConfiguration == null)
				{
					this.gpsConfiguration = new GPSConfiguration();
				}
				return this.gpsConfiguration;
			}
		}

		public EthernetConfiguration EthernetConfiguration
		{
			get
			{
				if (this.ethernetConfiguration == null)
				{
					this.ethernetConfiguration = new EthernetConfiguration();
				}
				return this.ethernetConfiguration;
			}
		}

		public InterfaceModeConfiguration InterfaceModeConfiguration
		{
			get
			{
				if (this.interfaceModeConfiguration == null)
				{
					this.interfaceModeConfiguration = new InterfaceModeConfiguration();
				}
				return this.interfaceModeConfiguration;
			}
		}
	}
}
