using System;
using System.Runtime.Serialization;
using Vector.VLConfig.LoggerSpecifics;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "LEDConfigListItem")]
	public class LEDConfigListItem
	{
		[DataMember(Name = "LEDConfigListItemLEDState")]
		public ValidatedProperty<LEDState> State
		{
			get;
			set;
		}

		public BusType UsedBusType
		{
			get
			{
				switch (this.State.Value)
				{
				case LEDState.CANError:
					return BusType.Bt_CAN;
				case LEDState.LINError:
					return BusType.Bt_LIN;
				default:
					return BusType.Bt_None;
				}
			}
		}

		public LEDItemParameter UsedParameters
		{
			get
			{
				switch (this.State.Value)
				{
				case LEDState.TriggerActive:
					return LEDItemParameter.MemoryWithORWildcard;
				case LEDState.LoggingActive:
				case LEDState.EndOfMeasurement:
				case LEDState.MemoryFull:
					return LEDItemParameter.MemoryWithORWildcard | LEDItemParameter.MemoryWithANDWildcard;
				case LEDState.CANError:
				case LEDState.LINError:
					return LEDItemParameter.ChannelWithWildcard;
				}
				return LEDItemParameter.None;
			}
		}

		[DataMember(Name = "LEDConfigListItemParameterChannelNumber")]
		public ValidatedProperty<uint> ParameterChannelNumber
		{
			get;
			set;
		}

		[DataMember(Name = "LEDConfigListItemParameterMemory")]
		public ValidatedProperty<uint> ParameterMemory
		{
			get;
			set;
		}

		public LEDConfigListItem()
		{
			this.State = new ValidatedProperty<LEDState>(LEDState.Disabled);
			this.ParameterChannelNumber = new ValidatedProperty<uint>(0u);
			this.ParameterMemory = new ValidatedProperty<uint>(0u);
		}
	}
}
