using System;
using System.Collections.Generic;
using System.Linq;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.CANwinAccess.Data
{
	public class CANwinBusItem
	{
		public CANwinBusType BusType
		{
			get;
			private set;
		}

		public int ChannelNumber
		{
			get;
			private set;
		}

		public string GenericNetworkName
		{
			get;
			private set;
		}

		public IList<string> AbsoluteDatabaseFilePaths
		{
			get;
			private set;
		}

		public CANwinBusItem(CANwinBusType busType, int channelNumber)
		{
			this.BusType = busType;
			this.ChannelNumber = channelNumber;
			this.GenericNetworkName = CANwinBusItem.GetGenericNetworkName(this.BusType, this.ChannelNumber);
			this.AbsoluteDatabaseFilePaths = new List<string>();
		}

		public CANwinBusItem(BusType busType, int channelNumber)
		{
			this.BusType = CANwinBusItem.Convert(busType);
			this.ChannelNumber = channelNumber;
			this.GenericNetworkName = CANwinBusItem.GetGenericNetworkName(this.BusType, this.ChannelNumber);
			this.AbsoluteDatabaseFilePaths = new List<string>();
		}

		public static CANwinBusType Convert(BusType busType)
		{
			switch (busType)
			{
			case Vector.VLConfig.Data.ConfigurationDataModel.BusType.Bt_CAN:
				return CANwinBusType.CAN;
			case Vector.VLConfig.Data.ConfigurationDataModel.BusType.Bt_LIN:
				return CANwinBusType.LIN;
			case Vector.VLConfig.Data.ConfigurationDataModel.BusType.Bt_FlexRay:
				return CANwinBusType.FLEXRAY;
			case Vector.VLConfig.Data.ConfigurationDataModel.BusType.Bt_Ethernet:
				return CANwinBusType.Ethernet;
			case Vector.VLConfig.Data.ConfigurationDataModel.BusType.Bt_MOST:
				return CANwinBusType.MOST;
			}
			return CANwinBusType.Invalid;
		}

		public static BusType Convert(CANwinBusType busType)
		{
			switch (busType)
			{
			case CANwinBusType.CAN:
				return Vector.VLConfig.Data.ConfigurationDataModel.BusType.Bt_CAN;
			case CANwinBusType.J1939:
			case CANwinBusType.VAN:
			case CANwinBusType.TTP:
				break;
			case CANwinBusType.LIN:
				return Vector.VLConfig.Data.ConfigurationDataModel.BusType.Bt_LIN;
			case CANwinBusType.MOST:
				return Vector.VLConfig.Data.ConfigurationDataModel.BusType.Bt_MOST;
			case CANwinBusType.FLEXRAY:
				return Vector.VLConfig.Data.ConfigurationDataModel.BusType.Bt_FlexRay;
			default:
				if (busType == CANwinBusType.Ethernet)
				{
					return Vector.VLConfig.Data.ConfigurationDataModel.BusType.Bt_Ethernet;
				}
				break;
			}
			return Vector.VLConfig.Data.ConfigurationDataModel.BusType.Bt_None;
		}

		public static int SortByChannelAscending(CANwinBusItem i1, CANwinBusItem i2)
		{
			int num = i1.ChannelNumber.CompareTo(i2.ChannelNumber);
			if (num == 0)
			{
				return string.Compare(i1.GenericNetworkName, i2.GenericNetworkName, StringComparison.Ordinal);
			}
			return num;
		}

		public static int SortByChannelDescending(CANwinBusItem i1, CANwinBusItem i2)
		{
			return CANwinBusItem.SortByChannelAscending(i2, i1);
		}

		public static int GetMaxChannelNumber(CANwinBusType busType, IEnumerable<CANwinBusItem> busConfiguration)
		{
			int num = 0;
			List<CANwinBusItem> list = (from bi in busConfiguration
			where bi.BusType == busType
			select bi).ToList<CANwinBusItem>();
			foreach (CANwinBusItem current in list)
			{
				if (current.ChannelNumber > num)
				{
					num = current.ChannelNumber;
				}
			}
			return num;
		}

		public static string GetGenericNetworkName(CANwinBusType busType, int channelNumber)
		{
			return busType.ToString().ToUpperInvariant() + channelNumber;
		}
	}
}
