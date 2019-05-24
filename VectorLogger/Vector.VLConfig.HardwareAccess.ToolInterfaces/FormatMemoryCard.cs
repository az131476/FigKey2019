using System;
using System.IO;
using Vector.VLConfig.LoggerSpecifics;

namespace Vector.VLConfig.HardwareAccess.ToolInterfaces
{
	public class FormatMemoryCard : GenericToolInterface
	{
		public FormatMemoryCard()
		{
			base.FileName = "CMD.exe";
		}

		public bool FormatFAT32(string drivePath, ulong clusterSize, string defaultVolumeName)
		{
			if (string.IsNullOrEmpty(drivePath) || drivePath.Length > 3 || (drivePath.Length > 1 && drivePath[1] != Path.VolumeSeparatorChar))
			{
				return false;
			}
			if (drivePath.Length > 2)
			{
				drivePath = drivePath.Substring(0, 2);
			}
			else if (drivePath.Length < 2)
			{
				drivePath += Path.VolumeSeparatorChar;
			}
			DriveInfo driveInfo = null;
			try
			{
				driveInfo = new DriveInfo(drivePath);
			}
			catch (Exception)
			{
				bool result = false;
				return result;
			}
			if (!driveInfo.IsReady || driveInfo.DriveType != DriveType.Removable)
			{
				return false;
			}
			string text = defaultVolumeName;
			if (!string.IsNullOrEmpty(driveInfo.VolumeLabel))
			{
				text = driveInfo.VolumeLabel;
			}
			if (text.Length > 11)
			{
				text = text.Substring(0, 11);
			}
			base.DeleteCommandLineArguments();
			base.AddCommandLineArgument("/C format");
			base.AddCommandLineArgument(drivePath);
			base.AddCommandLineArgument("/Q");
			base.AddCommandLineArgument("/Y");
			base.AddCommandLineArgument("/X");
			base.AddCommandLineArgument("/V:" + text);
			base.AddCommandLineArgument("/FS:FAT32");
			base.AddCommandLineArgument("/A:" + this.GetClusterSizeParamString(clusterSize));
			base.RunSynchronous();
			string arg_147_0 = base.LastStdOut;
			if (base.LastExitCode != 0)
			{
				string arg_159_0 = base.LastStdErr;
				return false;
			}
			return true;
		}

		private string GetClusterSizeParamString(ulong clusterSize)
		{
			if (clusterSize <= Constants.ClusterSize512_Bytes)
			{
				return "512";
			}
			if (clusterSize <= Constants.ClusterSize1k_Bytes)
			{
				return "1024";
			}
			if (clusterSize <= Constants.ClusterSize2k_Bytes)
			{
				return "2048";
			}
			if (clusterSize <= Constants.ClusterSize4k_Bytes)
			{
				return "4096";
			}
			if (clusterSize <= Constants.ClusterSize8k_Bytes)
			{
				return "8192";
			}
			if (clusterSize <= Constants.ClusterSize16k_Bytes)
			{
				return "16K";
			}
			if (clusterSize <= Constants.ClusterSize32k_Bytes)
			{
				return "32K";
			}
			return "64K";
		}
	}
}
