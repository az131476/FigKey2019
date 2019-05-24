using System;

namespace Vector.VLConfig.HardwareAccess.ToolInterfaces
{
	public struct GL2000Infos
	{
		public string serialNumber;

		public string configName;

		public DateTime compileDateTime;

		public string firmwareVersion;

		public string licenses;

		public uint memCardSizeMB;

		public uint recordingBufs;

		public uint triggeredBufs;

		public bool isSingleFile;

		public uint freeSpaceMB;

		public string can1Baby;

		public string can2Baby;
	}
}
