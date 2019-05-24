using System;

namespace Vector.VLConfig.HardwareAccess.ToolInterfaces
{
	public struct GL1000Infos
	{
		public string serialNumber;

		public string vehicleName;

		public string configName;

		public DateTime compileDateTime;

		public string firmwareVersion;

		public string licenses;

		public uint cardWriteCacheKB;

		public uint memCardSizeMB;

		public uint recordingBufs;

		public uint triggeredBufs;

		public bool isSingleFile;

		public uint freeSpaceMB;

		public string can1Baby;

		public string can2Baby;

		public string userDataFileName;

		public uint dataSizeKB;
	}
}
