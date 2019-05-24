using System;

namespace Vector.VLConfig.CANwinAccess
{
	internal class Constants
	{
		public static readonly int NumCANChannels = 16;

		public static readonly int NumLinChannels = 2;

		public static readonly int NumMOSTChannels = 1;

		public static readonly int NumFlexrayChannels = 2;

		public static readonly Version ProductMinVersion = new Version(8, 2, 80);

		public static readonly string ProductMinVersionName = Constants.ProductMinVersion + " (SP3)";
	}
}
