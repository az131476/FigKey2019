using System;
using System.Runtime.InteropServices;

namespace Vector.VLConfig.HardwareAccess.HardwareAbstraction.VN16XXlog
{
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	internal struct Vn1630LogFwImageInfoTrailer
	{
		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		public struct Version
		{
			public ushort build;

			public byte minor;

			public byte major;
		}

		public Vn1630LogFwImageInfoTrailer.Version versionFwImage;

		public Vn1630LogFwImageInfoTrailer.Version versionFpgaImage;

		public uint targetHwType;

		public uint crcCheckSum;
	}
}
