using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "HardwareChannel")]
	public abstract class HardwareChannel
	{
		private ValidatedProperty<BusType> busType;

		[DataMember(Name = "ChannelIsActive")]
		public ValidatedProperty<bool> IsActive
		{
			get;
			set;
		}

		public ValidatedProperty<BusType> BusType
		{
			get
			{
				BusType value = Vector.VLConfig.Data.ConfigurationDataModel.BusType.Bt_Wildcard;
				if (this is CANChannel)
				{
					value = Vector.VLConfig.Data.ConfigurationDataModel.BusType.Bt_CAN;
				}
				else if (this is LINChannel)
				{
					value = Vector.VLConfig.Data.ConfigurationDataModel.BusType.Bt_LIN;
				}
				else if (this is FlexrayChannel)
				{
					value = Vector.VLConfig.Data.ConfigurationDataModel.BusType.Bt_FlexRay;
				}
				else if (this is J1708Channel)
				{
					value = Vector.VLConfig.Data.ConfigurationDataModel.BusType.Bt_J1708;
				}
				if (this.busType == null)
				{
					this.busType = new ValidatedProperty<BusType>(value);
				}
				else
				{
					this.busType.Value = value;
				}
				return this.busType;
			}
		}

		public HardwareChannel()
		{
			this.IsActive = new ValidatedProperty<bool>(false);
		}
	}
}
