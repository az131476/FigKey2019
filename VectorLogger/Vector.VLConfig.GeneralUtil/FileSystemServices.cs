using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using Vector.Converter.CANoe;
using Vector.VLConfig.GeneralUtil.FileSystem;

namespace Vector.VLConfig.GeneralUtil
{
	public class FileSystemServices
	{
		public enum EnumAssemblyType
		{
			CallingAssembly,
			EntryAssembly,
			ExecutingAssembly
		}

		public static readonly string FileSystemFormatFAT32 = "FAT32";

		public static readonly string FileSystemFormatFAT = "FAT";

		public static readonly string SystemVolumeInformationFolderName = "System Volume Information";

		private static char[] csvDelimiterChars = new char[]
		{
			' ',
			',',
			'.',
			':',
			';',
			'\t'
		};

		private static uint NumberOfPreviewLines = 5u;

		public static readonly string DataFileStandardName = "Data";

		public static readonly string TriggerFileStandardName = "Trigger";

		public static readonly string TriggerFileOpenBufferName = "OpenBuffer";

		private static readonly Regex _RegExImageFileNameFormat = new Regex("[a-zA-Z]+(?<id>\\d+)_(?<year>\\d{4})-(?<month>\\d{2})-(?<day>\\d{2})_(?<hour>\\d{2})-(?<min>\\d{2})-(?<sec>\\d{2})-(?<msec>\\d+).jpg");

		private static readonly Regex _RegExImageArchiveNameFormat = new Regex("[a-zA-Z]+(?<id>\\d+)_(?<year>\\d{4})-(?<month>\\d{2})-(?<day>\\d{2})_(?<hour>\\d{2})-(?<min>\\d{2})-(?<sec>\\d{2})-(?<msec>\\d+).jpg.zip");

		private static readonly Regex _RegExAudioFileNameFormat = new Regex("(?<year>\\d{4})-(?<month>\\d{2})-(?<day>\\d{2})_(?<hour>\\d{2})-(?<min>\\d{2})-(?<sec>\\d{2})-(?<msec>\\d+).wav");

		private static readonly string _LicenseFileSearchPattern = "*.LIC";

		private static readonly string _LicenseFileExt = ".LIC";

		private static StreamWriter _protocolWriter = null;

		private static readonly string ProtocolFileName = "VLConfigSessionProtocol.log";

		[DllImport("kernel32.dll", SetLastError = true)]
		private static extern int GetDiskFreeSpaceW([MarshalAs(UnmanagedType.LPWStr)] [In] string lpRootPathName, out uint lpSectorsPerCluster, out uint lpBytesPerSector, out uint lpNumberOfFreeClusters, out uint lpTotalNumberOfClusters);

		public static IEnumerable<FileInfoProxy> GetFiles(string path)
		{
			if (!DirectoryProxy.Exists(path))
			{
				throw new DirectoryNotFoundException();
			}
			List<FileInfoProxy> list = new List<FileInfoProxy>();
			try
			{
				string[] files = DirectoryProxy.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly);
				string[] array = files;
				for (int i = 0; i < array.Length; i++)
				{
					string path2 = array[i];
					list.Add(new FileInfoProxy(path2));
				}
			}
			catch (Exception)
			{
			}
			return list;
		}

		public static IEnumerable<string> GetAccessibleFiles(string path, string pattern, SearchOption searchOption)
		{
			IEnumerable<string> enumerable = Enumerable.Empty<string>();
			try
			{
				if ((File.GetAttributes(path) & FileAttributes.ReparsePoint) != FileAttributes.ReparsePoint)
				{
					enumerable = enumerable.Concat(Directory.EnumerateFiles(path, pattern));
					if (searchOption == SearchOption.AllDirectories)
					{
						IEnumerable<string> enumerable2 = Directory.EnumerateDirectories(path);
						foreach (string current in enumerable2)
						{
							enumerable = enumerable.Concat(FileSystemServices.GetAccessibleFiles(current, pattern, searchOption));
						}
					}
				}
			}
			catch
			{
			}
			return enumerable;
		}

		public static bool HasSubFolders(string basePath, string folderNameSeachPattern, out List<string> subFolderPathList)
		{
			subFolderPathList = new List<string>();
			try
			{
				IEnumerable<string> enumerable = DirectoryProxy.EnumerateDirectories(basePath, folderNameSeachPattern, SearchOption.TopDirectoryOnly);
				foreach (string current in enumerable)
				{
					string text = Path.GetFileName(current) ?? string.Empty;
					if (text.IndexOf('.') < 0)
					{
						subFolderPathList.Add(Path.Combine(basePath, text));
					}
				}
			}
			catch (Exception)
			{
				return false;
			}
			return subFolderPathList.Count > 0;
		}

		public static bool TryMoveFile(string sourceFilePath, string destFolderPath)
		{
			if (!File.Exists(sourceFilePath) || !Directory.Exists(destFolderPath))
			{
				return false;
			}
			if (destFolderPath.Last<char>() != Path.DirectorySeparatorChar)
			{
				destFolderPath += Path.DirectorySeparatorChar;
			}
			string destFileName = Path.Combine(destFolderPath, Path.GetFileName(sourceFilePath));
			try
			{
				File.Move(sourceFilePath, destFileName);
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}

		public static bool TryCopyDirectoryRecursive(string sourceDirectoryPath, string destDirectoryPath)
		{
			if (!Directory.Exists(destDirectoryPath))
			{
				try
				{
					Directory.CreateDirectory(destDirectoryPath);
				}
				catch
				{
					bool result = false;
					return result;
				}
			}
			string[] files = Directory.GetFiles(sourceDirectoryPath);
			for (int i = 0; i < files.Length; i++)
			{
				string text = files[i];
				string destFileName = Path.Combine(destDirectoryPath, Path.GetFileName(text));
				try
				{
					File.Copy(text, destFileName);
				}
				catch
				{
					bool result = false;
					return result;
				}
			}
			string[] directories = Directory.GetDirectories(sourceDirectoryPath);
			for (int j = 0; j < directories.Length; j++)
			{
				string text2 = directories[j];
				string sourceDirectoryPath2 = text2;
				string fileName = Path.GetFileName(text2);
				string destDirectoryPath2 = Path.Combine(destDirectoryPath, fileName);
				if (!FileSystemServices.TryCopyDirectoryRecursive(sourceDirectoryPath2, destDirectoryPath2))
				{
					bool result = false;
					return result;
				}
			}
			return true;
		}

		public static bool TryCreateDirectory(string dirPath)
		{
			try
			{
				Directory.CreateDirectory(dirPath);
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}

		public static bool TryDeleteDirectory(string dirPath)
		{
			try
			{
				Directory.Delete(dirPath, true);
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}

		public static bool TryDeleteDirectoryRecursive(string dirPath, bool resetAttributes = true, bool includeTopmostDirectory = true)
		{
			bool result;
			try
			{
				DirectoryInfo fileSystemInfo = new DirectoryInfo(dirPath);
				result = FileSystemServices.TryDeleteDirectoryRecursive(fileSystemInfo, resetAttributes, includeTopmostDirectory);
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		public static bool TryDeleteDirectoryRecursive(FileSystemInfo fileSystemInfo, bool resetAttributes = true, bool includeTopmostDirectory = true)
		{
			DirectoryInfo directoryInfo = fileSystemInfo as DirectoryInfo;
			if (directoryInfo != null)
			{
				FileSystemInfo[] fileSystemInfos = directoryInfo.GetFileSystemInfos();
				for (int i = 0; i < fileSystemInfos.Length; i++)
				{
					FileSystemInfo fileSystemInfo2 = fileSystemInfos[i];
					if (!FileSystemServices.TryDeleteDirectoryRecursive(fileSystemInfo2, resetAttributes, true))
					{
						bool result = false;
						return result;
					}
				}
				if (!includeTopmostDirectory)
				{
					return true;
				}
			}
			try
			{
				fileSystemInfo.Attributes = FileAttributes.Normal;
				fileSystemInfo.Delete();
			}
			catch (Exception)
			{
				bool result = false;
				return result;
			}
			return true;
		}

		public static bool TryDeleteFile(string filePath)
		{
			try
			{
				File.Delete(filePath);
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}

		public static bool TrySetNormalFileAttributes(string filepath)
		{
			if (File.Exists(filepath))
			{
				try
				{
					File.SetAttributes(filepath, FileAttributes.Normal);
				}
				catch (Exception)
				{
					return false;
				}
				return true;
			}
			return false;
		}

		public static bool TryCreateTempDirectory(string targetFolder, out string fullTempDirPath)
		{
			fullTempDirPath = "";
			string text = Path.Combine(targetFolder, Path.GetRandomFileName());
			int num = 10;
			while (Directory.Exists(text) && num > 0)
			{
				text = Path.Combine(targetFolder, Path.GetRandomFileName());
				num--;
			}
			if (FileSystemServices.EnsureDirectoryExistence(text))
			{
				fullTempDirPath = text;
				return true;
			}
			return false;
		}

		public static bool EnsureDirectoryExistence(string folderPath)
		{
			if (!Directory.Exists(folderPath))
			{
				try
				{
					Directory.CreateDirectory(folderPath);
				}
				catch (Exception)
				{
					return false;
				}
				return true;
			}
			return true;
		}

		public static string CreateGenericFileTypeName(string filename)
		{
			int num = filename.LastIndexOf('.');
			if (num < 0)
			{
				return Resources_General.Unknown;
			}
			return string.Format(Resources_General.GenericFileTypeName, filename.Substring(num + 1).ToUpper());
		}

		public static string MakeFilenameCompatible(string filename, bool doReplaceSpaces = false, int maxLength = 250)
		{
			string text = filename.Replace(Path.DirectorySeparatorChar.ToString(), "_");
			text = text.Replace("/", "_");
			text = text.Replace(":", "_");
			text = text.Replace("*", "_");
			text = text.Replace("\"", "_");
			text = text.Replace("<", "_");
			text = text.Replace(">", "_");
			text = text.Replace("|", "_");
			if (doReplaceSpaces)
			{
				text = text.Replace(" ", "_");
			}
			if (text.Length > maxLength)
			{
				text = text.Substring(0, maxLength);
			}
			return text;
		}

		public static string PathCombineWithNameLimitation(string directoryPath, string filenameWithoutExtenstion, string extension, int lengthLimit)
		{
			if (directoryPath.Length + 1 + filenameWithoutExtenstion.Length + extension.Length <= lengthLimit)
			{
				return Path.Combine(directoryPath, filenameWithoutExtenstion + extension);
			}
			int num = lengthLimit - directoryPath.Length - 1 - extension.Length;
			if (num > 0)
			{
				string str = filenameWithoutExtenstion.Substring(0, num);
				return Path.Combine(directoryPath, str + extension);
			}
			throw new NotSupportedException();
		}

		public static bool ReplicateFileWithRelativePathInFolder(string absPathToRelativeFilePath, string relativeFilePathToReplicate, string absTargetFolderPath, bool overwriteExisting)
		{
			if (string.IsNullOrEmpty(absPathToRelativeFilePath) || string.IsNullOrEmpty(relativeFilePathToReplicate) || string.IsNullOrEmpty(absTargetFolderPath))
			{
				return false;
			}
			absPathToRelativeFilePath = absPathToRelativeFilePath.TrimEnd(new char[]
			{
				Path.DirectorySeparatorChar
			});
			relativeFilePathToReplicate = relativeFilePathToReplicate.TrimStart(new char[]
			{
				Path.DirectorySeparatorChar
			});
			string text = Path.Combine(absPathToRelativeFilePath, relativeFilePathToReplicate);
			if (!Directory.Exists(absPathToRelativeFilePath) || !Directory.Exists(absTargetFolderPath) || !File.Exists(text))
			{
				return false;
			}
			string text2 = absTargetFolderPath;
			string[] array = relativeFilePathToReplicate.Split(new char[]
			{
				Path.DirectorySeparatorChar
			});
			int num = array.Count<string>() - 1;
			if (num > 0)
			{
				for (int i = 0; i < num; i++)
				{
					text2 = Path.Combine(text2, array[i]);
					if (!Directory.Exists(text2))
					{
						text2 = Path.GetDirectoryName(Path.Combine(absTargetFolderPath, relativeFilePathToReplicate));
						try
						{
							Directory.CreateDirectory(text2);
						}
						catch
						{
							bool result = false;
							return result;
						}
					}
				}
			}
			try
			{
				File.Copy(text, Path.Combine(text2, Path.GetFileName(text)), overwriteExisting);
			}
			catch
			{
				bool result = false;
				return result;
			}
			return true;
		}

		public static bool GetDirectorySize(string dirPath, out long dirSizeInBytes)
		{
			dirSizeInBytes = 0L;
			if (!Directory.Exists(dirPath))
			{
				return false;
			}
			bool result;
			try
			{
				DirectoryInfo directoryInfo = new DirectoryInfo(dirPath);
				IEnumerable<FileInfo> enumerable = directoryInfo.EnumerateFiles("*", SearchOption.AllDirectories);
				foreach (FileInfo current in enumerable)
				{
					dirSizeInBytes += current.Length;
				}
				result = true;
			}
			catch (Exception)
			{
				result = false;
			}
			return result;
		}

		public static bool GetVolumeSizes(string volumePath, out long totalCapacityInBytes, out long availableFreeSizeInBytes)
		{
			totalCapacityInBytes = 0L;
			availableFreeSizeInBytes = 0L;
			try
			{
				string pathRoot = Path.GetPathRoot(volumePath);
				if (!string.IsNullOrEmpty(pathRoot))
				{
					DriveInfo driveInfo = new DriveInfo(pathRoot);
					totalCapacityInBytes = driveInfo.TotalSize;
					availableFreeSizeInBytes = driveInfo.AvailableFreeSpace;
					bool result = true;
					return result;
				}
			}
			catch (Exception)
			{
				bool result = false;
				return result;
			}
			return false;
		}

		public static IEnumerable<DriveInfo> GetAvailableFixedDrives()
		{
			List<DriveInfo> list = new List<DriveInfo>();
			DriveInfo[] drives = DriveInfo.GetDrives();
			DriveInfo[] array = drives;
			for (int i = 0; i < array.Length; i++)
			{
				DriveInfo driveInfo = array[i];
				if (driveInfo.DriveType == DriveType.Fixed && driveInfo.IsReady && (driveInfo.DriveFormat == FileSystemServices.FileSystemFormatFAT32 || driveInfo.DriveFormat == FileSystemServices.FileSystemFormatFAT))
				{
					list.Add(driveInfo);
				}
			}
			return list;
		}

		public static bool HasFAT32FileSystem(string path)
		{
			bool result;
			try
			{
				string pathRoot = Path.GetPathRoot(path);
				DriveInfo driveInfo = new DriveInfo(pathRoot);
				if (driveInfo != null && driveInfo.DriveFormat == FileSystemServices.FileSystemFormatFAT32)
				{
					result = true;
				}
				else
				{
					result = false;
				}
			}
			catch
			{
				result = false;
			}
			return result;
		}

		public static IEnumerable<DriveInfo> GetAvailableRemovableDrives(bool cardReadersOnly)
		{
			List<DriveInfo> list = new List<DriveInfo>();
			DriveInfo[] drives = DriveInfo.GetDrives();
			ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive WHERE InterfaceType='USB'");
			managementObjectSearcher.Get();
			if (!cardReadersOnly)
			{
				ManagementObjectSearcher managementObjectSearcher2 = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive WHERE InterfaceType='IDE'");
				ManagementObjectCollection managementObjectCollection = managementObjectSearcher2.Get();
				List<string> list2 = new List<string>();
				using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = managementObjectCollection.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ManagementObject managementObject = (ManagementObject)enumerator.Current;
						using (ManagementObjectCollection.ManagementObjectEnumerator enumerator2 = managementObject.GetRelated("Win32_DiskPartition").GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								ManagementObject managementObject2 = (ManagementObject)enumerator2.Current;
								using (ManagementObjectCollection.ManagementObjectEnumerator enumerator3 = managementObject2.GetRelated("Win32_LogicalDisk").GetEnumerator())
								{
									while (enumerator3.MoveNext())
									{
										ManagementObject managementObject3 = (ManagementObject)enumerator3.Current;
										list2.Add(Convert.ToString(managementObject3["Name"]) + Path.DirectorySeparatorChar);
									}
								}
							}
						}
					}
				}
				string item = "C" + Path.VolumeSeparatorChar + Path.DirectorySeparatorChar;
				if (!list2.Contains(item))
				{
					list2.Add(item);
				}
				DriveInfo[] array = drives;
				for (int i = 0; i < array.Length; i++)
				{
					DriveInfo driveInfo = array[i];
					if (!list2.Contains(driveInfo.Name) && driveInfo.DriveType == DriveType.Fixed && driveInfo.IsReady && (driveInfo.DriveFormat == FileSystemServices.FileSystemFormatFAT32 || driveInfo.DriveFormat == FileSystemServices.FileSystemFormatFAT))
					{
						list.Add(driveInfo);
					}
				}
			}
			DriveInfo[] array2 = drives;
			for (int j = 0; j < array2.Length; j++)
			{
				DriveInfo driveInfo2 = array2[j];
				if (driveInfo2.DriveType == DriveType.Removable && driveInfo2.IsReady && (driveInfo2.DriveFormat == FileSystemServices.FileSystemFormatFAT32 || driveInfo2.DriveFormat == FileSystemServices.FileSystemFormatFAT))
				{
					list.Add(driveInfo2);
				}
			}
			return list;
		}

		public static IEnumerable<DriveInfo> GetUSBCardReaderDrivesOfModel(IList<string> modelNames, string vid_pid, bool includeOnlyFATFormattedDrives)
		{
			List<DriveInfo> list = new List<DriveInfo>();
			try
			{
				ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_DiskDrive WHERE InterfaceType='USB'");
				ManagementObjectCollection managementObjectCollection = managementObjectSearcher.Get();
				List<string> list2 = new List<string>();
				using (ManagementObjectCollection.ManagementObjectEnumerator enumerator = managementObjectCollection.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ManagementObject managementObject = (ManagementObject)enumerator.Current;
						if (managementObject.GetPropertyValue("Model") != null && modelNames.Contains(managementObject.GetPropertyValue("Model").ToString()))
						{
							if (!string.IsNullOrEmpty(vid_pid))
							{
								if (managementObject.GetPropertyValue("PNPDeviceID") == null)
								{
									continue;
								}
								string text = managementObject.GetPropertyValue("PNPDeviceID").ToString();
								if (string.IsNullOrEmpty(text))
								{
									continue;
								}
								bool flag = false;
								string value = "";
								ManagementObjectSearcher managementObjectSearcher2 = new ManagementObjectSearcher("Select * From Win32_USBHub");
								ManagementObjectCollection managementObjectCollection2 = managementObjectSearcher2.Get();
								using (ManagementObjectCollection.ManagementObjectEnumerator enumerator2 = managementObjectCollection2.GetEnumerator())
								{
									while (enumerator2.MoveNext())
									{
										ManagementObject managementObject2 = (ManagementObject)enumerator2.Current;
										if (managementObject2.GetPropertyValue("DeviceID") != null)
										{
											string text2 = managementObject2.GetPropertyValue("DeviceID").ToString();
											int num = text2.IndexOf(vid_pid);
											if (num >= 0)
											{
												flag = true;
												int num2 = num + vid_pid.Length + 1;
												if (num2 < text2.Length)
												{
													value = text2.Substring(num2);
													break;
												}
												break;
											}
										}
									}
								}
								if (!flag || string.IsNullOrEmpty(value) || text.IndexOf(value) < 0)
								{
									continue;
								}
							}
							bool flag2 = false;
							using (ManagementObjectCollection.ManagementObjectEnumerator enumerator3 = managementObject.GetRelated("Win32_DiskPartition").GetEnumerator())
							{
								while (enumerator3.MoveNext())
								{
									ManagementObject managementObject3 = (ManagementObject)enumerator3.Current;
									using (ManagementObjectCollection.ManagementObjectEnumerator enumerator4 = managementObject3.GetRelated("Win32_LogicalDisk").GetEnumerator())
									{
										if (enumerator4.MoveNext())
										{
											ManagementObject managementObject4 = (ManagementObject)enumerator4.Current;
											list2.Add(Convert.ToString(managementObject4["Name"]) + Path.DirectorySeparatorChar);
											flag2 = true;
										}
									}
									if (flag2)
									{
										break;
									}
								}
							}
						}
					}
				}
				DriveInfo[] drives = DriveInfo.GetDrives();
				DriveInfo[] array = drives;
				for (int i = 0; i < array.Length; i++)
				{
					DriveInfo driveInfo = array[i];
					if (!includeOnlyFATFormattedDrives)
					{
						if (driveInfo.DriveType == DriveType.Removable && list2.Contains(driveInfo.Name))
						{
							list.Add(driveInfo);
						}
					}
					else if (driveInfo.IsReady && list2.Contains(driveInfo.Name) && (driveInfo.DriveFormat == FileSystemServices.FileSystemFormatFAT32 || driveInfo.DriveFormat == FileSystemServices.FileSystemFormatFAT))
					{
						list.Add(driveInfo);
					}
				}
			}
			catch
			{
			}
			return list;
		}

		public static string GetCardReaderDisplayName(string driveName)
		{
			string result = driveName;
			DriveInfo driveInfo = null;
			try
			{
				driveInfo = new DriveInfo(driveName);
			}
			catch (Exception)
			{
				return result;
			}
			if (driveInfo != null)
			{
				result = FileSystemServices.GetCardReaderDisplayName(driveInfo);
				driveInfo = null;
			}
			return result;
		}

		public static string GetCardReaderDisplayName(DriveInfo driveInfo)
		{
			if (driveInfo == null)
			{
				return string.Empty;
			}
			if (!driveInfo.IsReady || string.IsNullOrEmpty(driveInfo.VolumeLabel))
			{
				return driveInfo.Name;
			}
			return string.Format("{0} ({1})", driveInfo.VolumeLabel, driveInfo.Name);
		}

		public static bool IsDriveAvailable(string deviceHardwareKey)
		{
			string driveName = deviceHardwareKey;
			if (deviceHardwareKey.Length > 2)
			{
				driveName = deviceHardwareKey.Substring(0, 2);
			}
			else if (deviceHardwareKey.Length == 1)
			{
				driveName = deviceHardwareKey + ":";
			}
			try
			{
				DriveInfo driveInfo = new DriveInfo(driveName);
				if (!driveInfo.IsReady)
				{
					bool result = false;
					return result;
				}
			}
			catch
			{
				bool result = false;
				return result;
			}
			return true;
		}

		public static bool GetMemoryCardClusterSize(string drivePath, out ulong clusterSizeInBytes, out ulong totalSizeInBytes)
		{
			clusterSizeInBytes = 0uL;
			totalSizeInBytes = 0uL;
			uint num;
			uint num2;
			uint num3;
			uint num4;
			int diskFreeSpaceW = FileSystemServices.GetDiskFreeSpaceW(drivePath, out num, out num2, out num3, out num4);
			if (diskFreeSpaceW != 0)
			{
				clusterSizeInBytes = (ulong)(num * num2);
				totalSizeInBytes = clusterSizeInBytes * (ulong)num4;
				return true;
			}
			return false;
		}

		public static bool IsDriveExistingAndEmptyExceptSystemVolumeInformation(string absPath)
		{
			string driveName = absPath;
			if (absPath.Length == 1)
			{
				driveName = string.Format("{0}{1}{2}", absPath, Path.VolumeSeparatorChar, Path.DirectorySeparatorChar);
			}
			else if (absPath.Length > 3)
			{
				driveName = Path.GetPathRoot(absPath);
			}
			FileSystemInfo[] fileSystemInfos;
			try
			{
				DriveInfo driveInfo = new DriveInfo(driveName);
				fileSystemInfos = driveInfo.RootDirectory.GetFileSystemInfos();
			}
			catch
			{
				bool result = false;
				return result;
			}
			if (fileSystemInfos.Count<FileSystemInfo>() == 0)
			{
				return true;
			}
			bool flag = false;
			FileSystemInfo[] array = fileSystemInfos;
			for (int i = 0; i < array.Length; i++)
			{
				FileSystemInfo fileSystemInfo = array[i];
				try
				{
					if (string.Compare(fileSystemInfo.Name, FileSystemServices.SystemVolumeInformationFolderName, true) != 0 || (fileSystemInfo.Attributes & FileAttributes.Hidden) != FileAttributes.Hidden || (fileSystemInfo.Attributes & FileAttributes.System) != FileAttributes.System || (fileSystemInfo.Attributes & FileAttributes.Directory) != FileAttributes.Directory)
					{
						flag = true;
						break;
					}
				}
				catch
				{
				}
			}
			return !flag;
		}

		public static bool TryDeleteSystemVolumeInformation(string absPath)
		{
			string path = absPath;
			if (absPath.Length == 1)
			{
				path = string.Format("{0}{1}{2}", absPath, Path.VolumeSeparatorChar, Path.DirectorySeparatorChar);
			}
			else if (absPath.Length > 3)
			{
				path = Path.GetPathRoot(absPath);
			}
			bool flag = false;
			string text = Path.Combine(path, FileSystemServices.SystemVolumeInformationFolderName);
			if (Directory.Exists(text))
			{
				try
				{
					DirectoryInfo directoryInfo = new DirectoryInfo(text);
					if ((directoryInfo.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden && (directoryInfo.Attributes & FileAttributes.System) == FileAttributes.System)
					{
						flag = true;
					}
				}
				catch (Exception)
				{
				}
			}
			return flag && FileSystemServices.TryDeleteDirectory(text);
		}

		public static bool GetIniFilePropertyValue(string filepath, string propertyName, out string propertyValue)
		{
			propertyValue = "";
			if (!FileProxy.Exists(filepath))
			{
				return false;
			}
			bool result;
			try
			{
				using (StreamReader streamReader = FileProxy.OpenText(filepath))
				{
					string text;
					while ((text = streamReader.ReadLine()) != null)
					{
						if (text.Length > 0 && text.IndexOf(propertyName) == 0)
						{
							propertyValue = text.Substring(propertyName.Length + 1, text.Length - (propertyName.Length + 1));
							streamReader.Close();
							result = true;
							return result;
						}
					}
					streamReader.Close();
				}
				result = false;
			}
			catch
			{
				result = false;
			}
			return result;
		}

		public static bool GetIniFilePropertiesAndValues(string filepath, ref Dictionary<string, string> propertiesAndValues)
		{
			if (!FileProxy.Exists(filepath))
			{
				return false;
			}
			bool result;
			try
			{
				using (StreamReader streamReader = FileProxy.OpenText(filepath))
				{
					string text;
					while ((text = streamReader.ReadLine()) != null)
					{
						if (text.Length > 0)
						{
							int num = text.IndexOf('=');
							if (num >= 0)
							{
								string key = text.Substring(0, num);
								string value = "";
								if (num + 1 < text.Length)
								{
									value = text.Substring(num + 1);
								}
								if (!propertiesAndValues.ContainsKey(key))
								{
									propertiesAndValues.Add(key, value);
								}
							}
						}
					}
					streamReader.Close();
				}
				result = true;
			}
			catch
			{
				result = false;
			}
			return result;
		}

		public static bool GenerateDestinationFolderNameFromIniFile(string filepath, out string folderName)
		{
			folderName = "";
			string text = "";
			string text2 = "";
			if (!FileProxy.Exists(filepath))
			{
				return false;
			}
			string iniFileDateAttribute = Vocabulary.IniFileDateAttribute;
			string iniFileTimeAttribute = Vocabulary.IniFileTimeAttribute;
			using (StreamReader streamReader = FileProxy.OpenText(filepath))
			{
				string text3;
				while ((text3 = streamReader.ReadLine()) != null)
				{
					if (text3.Length > 0)
					{
						if (text3.IndexOf(iniFileDateAttribute) == 0)
						{
							text = text3.Substring(iniFileDateAttribute.Length);
						}
						else if (text3.IndexOf(iniFileTimeAttribute) == 0)
						{
							text2 = text3.Substring(iniFileTimeAttribute.Length);
						}
					}
					if (text.Length > 0 && text2.Length > 0)
					{
						break;
					}
				}
			}
			if (text.Length == 0 || text2.Length == 0)
			{
				return false;
			}
			string s = text + " " + text2;
			CultureInfo provider = CultureInfo.CreateSpecificCulture("de-DE");
			DateTimeStyles styles = DateTimeStyles.AssumeLocal;
			DateTime dateTime;
			if (DateTime.TryParse(s, provider, styles, out dateTime))
			{
				string format = "yyyy-MM-dd_HH-mm-ss";
				folderName = dateTime.ToString(format);
				return true;
			}
			return false;
		}

		public static bool TryMakeFilePathRelativeToConfiguration(string configurationFolderPath, ref string filePath)
		{
			if (string.IsNullOrEmpty(filePath))
			{
				return false;
			}
			if (string.IsNullOrEmpty(configurationFolderPath))
			{
				return true;
			}
			configurationFolderPath = configurationFolderPath.TrimEnd(new char[]
			{
				Path.DirectorySeparatorChar
			});
			if (filePath.Length > configurationFolderPath.Length && filePath.ToUpper(CultureInfo.InvariantCulture).IndexOf(configurationFolderPath.ToUpper(CultureInfo.InvariantCulture)) == 0)
			{
				string text = filePath.Substring(configurationFolderPath.Length);
				if (text.IndexOf(Path.DirectorySeparatorChar) == 0)
				{
					filePath = text;
					return true;
				}
			}
			return false;
		}

		public static string GetAbsolutePath(string referencedFilePath, string configFolderPath)
		{
			if (string.IsNullOrEmpty(referencedFilePath) || referencedFilePath.Length <= 0)
			{
				return string.Empty;
			}
			string pathRoot = Path.GetPathRoot(referencedFilePath);
			if (pathRoot.Length > 1)
			{
				return referencedFilePath;
			}
			if (string.IsNullOrEmpty(configFolderPath))
			{
				return referencedFilePath;
			}
			configFolderPath = configFolderPath.TrimEnd(new char[]
			{
				Path.DirectorySeparatorChar
			});
			return configFolderPath + referencedFilePath;
		}

		public static string GetFolderPathFromFilePath(string filepath)
		{
			if (string.IsNullOrEmpty(filepath))
			{
				return "";
			}
			string text = Path.GetDirectoryName(filepath);
			if (!string.IsNullOrEmpty(text))
			{
				return text;
			}
			text = Path.GetPathRoot(filepath);
			if (!string.IsNullOrEmpty(text))
			{
				return text;
			}
			return "";
		}

		public static string AdjustReferencedFilePath(string referencedFilePath, string oldConfigFolderPath, string newConfigFolderPath)
		{
			if (string.IsNullOrEmpty(referencedFilePath))
			{
				return referencedFilePath;
			}
			string pathRoot = Path.GetPathRoot(referencedFilePath);
			if (pathRoot.Length > 1)
			{
				string text = referencedFilePath;
				if (FileSystemServices.TryMakeFilePathRelativeToConfiguration(newConfigFolderPath, ref text))
				{
					referencedFilePath = text;
				}
			}
			else if (!string.IsNullOrEmpty(oldConfigFolderPath))
			{
				string text2 = oldConfigFolderPath.TrimEnd(new char[]
				{
					Path.DirectorySeparatorChar
				}) + referencedFilePath;
				if (FileSystemServices.TryMakeFilePathRelativeToConfiguration(newConfigFolderPath, ref text2))
				{
					referencedFilePath = text2;
				}
				else
				{
					referencedFilePath = oldConfigFolderPath.TrimEnd(new char[]
					{
						Path.DirectorySeparatorChar
					}) + referencedFilePath;
				}
			}
			return referencedFilePath;
		}

		public static bool IsAbsolutePath(string path)
		{
			return path.StartsWith(Path.DirectorySeparatorChar.ToString() + Path.DirectorySeparatorChar.ToString()) || path.IndexOf(Path.VolumeSeparatorChar.ToString() + Path.DirectorySeparatorChar.ToString()) == 1 || (!path.StartsWith(Path.DirectorySeparatorChar.ToString()) && path.StartsWith("." + Path.DirectorySeparatorChar.ToString()) && false);
		}

		public static string GetApplicationPath()
		{
			Assembly entryAssembly = Assembly.GetEntryAssembly();
			if (entryAssembly != null)
			{
				return Path.GetDirectoryName(entryAssembly.Location);
			}
			Assembly executingAssembly = Assembly.GetExecutingAssembly();
			if (executingAssembly != null)
			{
				return Path.GetDirectoryName(executingAssembly.CodeBase.Substring(8));
			}
			return string.Empty;
		}

		public static bool GetNumberOfColumnsInCSVFile(string filepath, out uint colCount, out string previewText)
		{
			colCount = 0u;
			StringBuilder stringBuilder = new StringBuilder();
			uint num = 0u;
			FileInfo fileInfo = new FileInfo(filepath);
			if (fileInfo.Exists)
			{
				try
				{
					using (TextReader textReader = new StreamReader(filepath))
					{
						string text;
						while ((text = textReader.ReadLine()) != null && num < FileSystemServices.NumberOfPreviewLines)
						{
							stringBuilder.Append(text);
							stringBuilder.Append("\r\n");
							string[] source = text.Split(FileSystemServices.csvDelimiterChars);
							colCount = Math.Max((uint)source.Count<string>(), colCount);
							num += 1u;
						}
						stringBuilder.Append("...");
					}
				}
				catch (Exception)
				{
					previewText = stringBuilder.ToString();
					return false;
				}
			}
			previewText = stringBuilder.ToString();
			return true;
		}

		public static bool GetNumberOfColumnsInCSVFile(string filepath, out uint colCount)
		{
			colCount = 0u;
			FileInfo fileInfo = new FileInfo(filepath);
			if (fileInfo.Exists)
			{
				try
				{
					using (TextReader textReader = new StreamReader(filepath))
					{
						string text;
						if ((text = textReader.ReadLine()) != null)
						{
							string[] source = text.Split(FileSystemServices.csvDelimiterChars);
							colCount = (uint)source.Count<string>();
						}
					}
				}
				catch (Exception)
				{
					return false;
				}
				return true;
			}
			return true;
		}

		public static bool ReadSymbolsInColumnFromCSVFile(string filepath, uint column, out List<string> symbolsInColumn)
		{
			symbolsInColumn = new List<string>();
			FileInfo fileInfo = new FileInfo(filepath);
			if (fileInfo.Exists)
			{
				try
				{
					using (TextReader textReader = new StreamReader(filepath))
					{
						string text;
						while ((text = textReader.ReadLine()) != null)
						{
							string[] array = text.Split(FileSystemServices.csvDelimiterChars);
							if ((long)array.Count<string>() > (long)((ulong)column))
							{
								symbolsInColumn.Add(array[(int)((UIntPtr)column)]);
							}
						}
					}
				}
				catch (Exception)
				{
					return false;
				}
				return true;
			}
			return true;
		}

		public static bool LaunchDirectoryBrowser(string directoryPath)
		{
			if (DirectoryProxy.Exists(directoryPath))
			{
				try
				{
					Process.Start(directoryPath);
				}
				catch (Exception)
				{
					return false;
				}
				return true;
			}
			return true;
		}

		public static bool LaunchFile(string filePath)
		{
			FileInfo fileInfo = null;
			try
			{
				fileInfo = new FileInfo(filePath);
			}
			catch (Exception)
			{
				bool result = false;
				return result;
			}
			if (fileInfo == null)
			{
				return false;
			}
			if (fileInfo.Exists)
			{
				try
				{
					Process.Start(filePath);
				}
				catch (Exception)
				{
					bool result = false;
					return result;
				}
			}
			return true;
		}

		public static bool GetRenamedLogDataFileName(bool isPrefix, string prefixOrNewName, string oldFilePath, out string newFilePath)
		{
			string text = string.Empty;
			newFilePath = string.Empty;
			if (string.IsNullOrEmpty(oldFilePath))
			{
				return false;
			}
			string directoryName = Path.GetDirectoryName(oldFilePath);
			string fileName = Path.GetFileName(oldFilePath);
			if (isPrefix)
			{
				text = prefixOrNewName + fileName;
			}
			else
			{
				string text2 = FileSystemServices.DataFileStandardName;
				int num = fileName.ToLower().IndexOf(text2.ToLower());
				if (num != 0)
				{
					text2 = FileSystemServices.TriggerFileStandardName;
					num = fileName.ToLower().IndexOf(text2.ToLower());
					if (num != 0)
					{
						text2 = FileSystemServices.TriggerFileOpenBufferName;
						num = fileName.ToLower().IndexOf(text2.ToLower());
						if (num != 0)
						{
							return false;
						}
					}
				}
				if (text2 == FileSystemServices.TriggerFileOpenBufferName)
				{
					text = prefixOrNewName + fileName;
				}
				else
				{
					string str = fileName.Substring(text2.Length);
					text = prefixOrNewName + str;
				}
			}
			if (!string.IsNullOrEmpty(directoryName))
			{
				newFilePath = Path.Combine(directoryName, text);
			}
			else
			{
				newFilePath = text;
			}
			return true;
		}

		public static IList<string> GetImageFilePaths(string sourcePath, DateTime startTime, int durationInSeconds)
		{
			List<string> list = new List<string>();
			if (string.IsNullOrEmpty(sourcePath) || !DirectoryProxy.Exists(sourcePath))
			{
				return list;
			}
			string[] array = null;
			try
			{
				array = DirectoryProxy.GetFiles(sourcePath, "*.jpg", SearchOption.AllDirectories);
			}
			catch
			{
				IList<string> result = list;
				return result;
			}
			if (array.Count<string>() == 0)
			{
				return list;
			}
			return FileSystemServices.GetPathsOfFilesWithinInterval(array, startTime, durationInSeconds, FileSystemServices._RegExImageFileNameFormat);
		}

		public static IList<string> GetAudioFilePaths(string sourcePath, DateTime startTime, int durationInSeconds)
		{
			List<string> list = new List<string>();
			if (string.IsNullOrEmpty(sourcePath) || !DirectoryProxy.Exists(sourcePath))
			{
				return list;
			}
			string[] array = null;
			try
			{
				array = DirectoryProxy.GetFiles(sourcePath, "*.wav", SearchOption.AllDirectories);
			}
			catch
			{
				IList<string> result = list;
				return result;
			}
			if (array.Count<string>() == 0)
			{
				return list;
			}
			return FileSystemServices.GetPathsOfFilesWithinInterval(array, startTime, durationInSeconds, FileSystemServices._RegExAudioFileNameFormat);
		}

		public static IList<string> GetImageArchivePaths(string sourcePath, DateTime startTime, int durationInSeconds)
		{
			List<string> list = new List<string>();
			if (string.IsNullOrEmpty(sourcePath) || !DirectoryProxy.Exists(sourcePath))
			{
				return list;
			}
			string[] array = null;
			try
			{
				array = DirectoryProxy.GetFiles(sourcePath, "*.zip");
			}
			catch
			{
				IList<string> result = list;
				return result;
			}
			if (array.Count<string>() == 0)
			{
				return list;
			}
			SortedDictionary<DateTime, string> sortedDictionary = new SortedDictionary<DateTime, string>();
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string text = array2[i];
				string fileName = Path.GetFileName(text);
				Match match = FileSystemServices._RegExImageArchiveNameFormat.Match(fileName);
				DateTime key;
				if (FileSystemServices.TryCreateDateTimeFromMatch(match, out key) && !sortedDictionary.ContainsKey(key))
				{
					sortedDictionary.Add(key, text);
				}
			}
			DateTime t = startTime + new TimeSpan(0, 0, durationInSeconds);
			string text2 = null;
			for (int j = 0; j < sortedDictionary.Count; j++)
			{
				if (startTime <= sortedDictionary.ElementAt(j).Key && sortedDictionary.ElementAt(j).Key <= t)
				{
					list.Add(sortedDictionary.ElementAt(j).Value);
					if (list.Count == 1 && j > 0)
					{
						text2 = sortedDictionary.ElementAt(j - 1).Value;
					}
				}
			}
			string text3;
			if (!string.IsNullOrEmpty(text2) && FileSystemServices.TryCreateTempDirectory(Path.GetTempPath(), out text3))
			{
				string text4 = Path.Combine(text3, Path.GetFileName(text2));
				try
				{
					FileProxy.Copy(text2, text4);
				}
				catch
				{
					FileSystemServices.TryDeleteDirectory(text3);
				}
				if (File.Exists(text4))
				{
					string[] array3 = null;
					try
					{
						array3 = DirectoryProxy.GetFiles(text4);
					}
					catch
					{
						FileSystemServices.TryDeleteDirectory(text3);
					}
					if (array3.Count<string>() > 0)
					{
						if (FileSystemServices.GetPathsOfFilesWithinInterval(array3, startTime, durationInSeconds, FileSystemServices._RegExImageFileNameFormat).Count > 0)
						{
							list.Insert(0, text2);
						}
						FileSystemServices.TryDeleteDirectory(text3);
					}
				}
			}
			return list;
		}

		private static IList<string> GetPathsOfFilesWithinInterval(string[] filePaths, DateTime startTime, int durationInSec, Regex regExFileNamePatternToMatch)
		{
			List<string> list = new List<string>();
			DateTime t = startTime + new TimeSpan(0, 0, durationInSec);
			for (int i = 0; i < filePaths.Length; i++)
			{
				string text = filePaths[i];
				string fileName = Path.GetFileName(text);
				Match match = regExFileNamePatternToMatch.Match(fileName);
				DateTime dateTime;
				if (FileSystemServices.TryCreateDateTimeFromMatch(match, out dateTime) && startTime <= dateTime && dateTime <= t)
				{
					list.Add(text);
				}
			}
			return list;
		}

		private static bool TryCreateDateTimeFromMatch(Match match, out DateTime timestamp)
		{
			timestamp = new DateTime(DateTime.MinValue.Ticks);
			if (match == null)
			{
				return false;
			}
			if (match.Groups["year"] != null && match.Groups["month"] != null && match.Groups["day"] != null && match.Groups["hour"] != null && match.Groups["min"] != null && match.Groups["sec"] != null)
			{
				try
				{
					int year = Convert.ToInt32(match.Groups["year"].ToString());
					int month = Convert.ToInt32(match.Groups["month"].ToString());
					int day = Convert.ToInt32(match.Groups["day"].ToString());
					int hour = Convert.ToInt32(match.Groups["hour"].ToString());
					int minute = Convert.ToInt32(match.Groups["min"].ToString());
					int second = Convert.ToInt32(match.Groups["sec"].ToString());
					int millisecond = Convert.ToInt32(match.Groups["msec"].ToString());
					timestamp = new DateTime(year, month, day, hour, minute, second, millisecond);
					bool result = true;
					return result;
				}
				catch
				{
					bool result = false;
					return result;
				}
				return false;
			}
			return false;
		}

		public static IList<string> GetFilesNotPresentInDestFolder(string destFolderPath, IList<string> filePaths, out long totalSizeInBytes)
		{
			List<string> list = new List<string>();
			totalSizeInBytes = 0L;
			if (string.IsNullOrEmpty(destFolderPath) || !Directory.Exists(destFolderPath) || filePaths == null)
			{
				return list;
			}
			foreach (string current in filePaths)
			{
				string fileName = Path.GetFileName(current);
				string path = Path.Combine(destFolderPath, fileName);
				if (!File.Exists(path))
				{
					list.Add(current);
					try
					{
						FileInfoProxy fileInfoProxy = new FileInfoProxy(current);
						totalSizeInBytes += fileInfoProxy.Length;
					}
					catch
					{
					}
				}
			}
			return list;
		}

		public static string GenerateUniqueDbcFileName(string sourceDbFileName, string networkName, int indexPosOfDbInConfig)
		{
			return string.Format("{0}_{1}_{2}_converted.dbc", sourceDbFileName, networkName, indexPosOfDbInConfig);
		}

		public static bool SaveAutosarNetworkAsDbc(string autosarDBFilePath, string networkName, string dbcFilePath)
		{
			Transform transform = new Transform();
			string value;
			string text;
			if (transform.Execute("SystemTemplateToDBC", networkName, autosarDBFilePath, "", out value, out text))
			{
				try
				{
					using (StreamWriter streamWriter = File.CreateText(dbcFilePath))
					{
						streamWriter.Write(value);
						streamWriter.Flush();
						streamWriter.Close();
					}
				}
				catch (Exception)
				{
					return false;
				}
				return true;
			}
			return false;
		}

		public static int NaturalCompare(string name1, string name2)
		{
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			while (num < name1.Length && num2 < name2.Length)
			{
				if (!char.IsDigit(name1[num]) && !char.IsDigit(name2[num2]))
				{
					num3 = string.Compare(name1[num].ToString(), name2[num2].ToString(), true);
				}
				else if (char.IsDigit(name1[num]) && !char.IsDigit(name2[num2]))
				{
					num3 = -1;
				}
				else if (!char.IsDigit(name1[num]) && char.IsDigit(name2[num2]))
				{
					num3 = 1;
				}
				else
				{
					num3 = FileSystemServices.CompareNumber(name1, ref num, name2, ref num2);
				}
				if (num3 != 0)
				{
					break;
				}
				num++;
				num2++;
			}
			if (num3 == 0)
			{
				if (name1.Length - num < name2.Length - num2)
				{
					num3 = -1;
				}
				else if (name2.Length - num2 < name1.Length - num)
				{
					num3 = 1;
				}
				else
				{
					num3 = 0;
				}
			}
			return num3;
		}

		private static int CompareNumber(string name1, ref int pos1, string name2, ref int pos2)
		{
			ulong num = 0uL;
			ulong num2 = 0uL;
			int num3 = pos1;
			int num4 = pos2;
			while (pos1 + 1 < name1.Length)
			{
				if (!char.IsDigit(name1[pos1 + 1]))
				{
					break;
				}
				pos1++;
			}
			while (pos2 + 1 < name2.Length)
			{
				if (!char.IsDigit(name2[pos2 + 1]))
				{
					break;
				}
				pos2++;
			}
			try
			{
				num = ulong.Parse(name1.Substring(num3, pos1 + 1 - num3));
				num2 = ulong.Parse(name2.Substring(num4, pos2 + 1 - num4));
			}
			catch (Exception)
			{
				int result = string.Compare(name1.Substring(num3, pos1 + 1 - num3), name2.Substring(num4, pos2 + 1 - num4));
				return result;
			}
			if (num < num2)
			{
				return -1;
			}
			if (num > num2)
			{
				return 1;
			}
			if (pos1 - num3 > pos2 - num4)
			{
				return -1;
			}
			if (pos1 - num3 < pos2 - num4)
			{
				return 1;
			}
			return 0;
		}

		public static bool HasLicenseFilesInFolder(string folderPath, out IList<string> licenseFiles)
		{
			licenseFiles = new List<string>();
			try
			{
				string[] files = Directory.GetFiles(folderPath, FileSystemServices._LicenseFileSearchPattern);
				string[] array = files;
				for (int i = 0; i < array.Length; i++)
				{
					string text = array[i];
					if (string.Compare(Path.GetExtension(text), FileSystemServices._LicenseFileExt, true) == 0)
					{
						licenseFiles.Add(text);
					}
				}
			}
			catch
			{
				return false;
			}
			return licenseFiles.Count > 0;
		}

		public static bool OpenProtocol()
		{
			if (FileSystemServices._protocolWriter == null)
			{
				string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), FileSystemServices.ProtocolFileName);
				if (!File.Exists(path))
				{
					return false;
				}
				try
				{
					FileSystemServices._protocolWriter = File.CreateText(path);
				}
				catch (Exception)
				{
					return false;
				}
				return true;
			}
			return true;
		}

		public static bool WriteProtocolLine(string line)
		{
			if (FileSystemServices._protocolWriter == null)
			{
				return false;
			}
			FileSystemServices._protocolWriter.WriteLine(line);
			FileSystemServices._protocolWriter.Flush();
			return true;
		}

		public static bool WriteProtocolText(string text)
		{
			if (FileSystemServices._protocolWriter == null)
			{
				return false;
			}
			FileSystemServices._protocolWriter.Write(text);
			FileSystemServices._protocolWriter.Flush();
			return true;
		}

		public static bool CloseProtocol()
		{
			if (FileSystemServices._protocolWriter == null)
			{
				return false;
			}
			try
			{
				FileSystemServices._protocolWriter.Close();
			}
			catch (Exception)
			{
				return false;
			}
			FileSystemServices._protocolWriter = null;
			return true;
		}

		public static bool WriteEmbeddedResourceToFile(string resourceName, string targetFilePath, FileSystemServices.EnumAssemblyType assemblyType = FileSystemServices.EnumAssemblyType.CallingAssembly)
		{
			Assembly assembly;
			switch (assemblyType)
			{
			case FileSystemServices.EnumAssemblyType.CallingAssembly:
				assembly = Assembly.GetCallingAssembly();
				break;
			case FileSystemServices.EnumAssemblyType.EntryAssembly:
				assembly = Assembly.GetEntryAssembly();
				break;
			case FileSystemServices.EnumAssemblyType.ExecutingAssembly:
				assembly = Assembly.GetExecutingAssembly();
				break;
			default:
				assembly = Assembly.GetCallingAssembly();
				break;
			}
			if (assembly == null)
			{
				return false;
			}
			using (Stream manifestResourceStream = assembly.GetManifestResourceStream(resourceName))
			{
				if (manifestResourceStream == null)
				{
					bool result = false;
					return result;
				}
				string value;
				using (StreamReader streamReader = new StreamReader(manifestResourceStream))
				{
					value = streamReader.ReadToEnd();
				}
				if (string.IsNullOrEmpty(value))
				{
					bool result = false;
					return result;
				}
				using (StreamWriter streamWriter = new StreamWriter(targetFilePath, false, Encoding.UTF8))
				{
					streamWriter.Write(value);
				}
			}
			return true;
		}

		public static DateTime ExtractCODCompileTime(string codPath)
		{
			FileStream fileStream = null;
			DateTime result = default(DateTime);
			try
			{
				fileStream = new FileStream(codPath, FileMode.Open, FileAccess.Read);
				BinaryReader binaryReader = new BinaryReader(fileStream);
				fileStream.Seek(-24L, SeekOrigin.End);
				uint num = binaryReader.ReadUInt32();
				if (num == 1990398367u)
				{
					uint num2 = binaryReader.ReadUInt32();
					int second = Convert.ToInt32(num2 % 60u);
					num2 /= 60u;
					int minute = Convert.ToInt32(num2 % 60u);
					num2 /= 60u;
					int hour = Convert.ToInt32(num2 % 24u);
					num2 /= 24u;
					int day = Convert.ToInt32(num2 % 31u) + 1;
					num2 /= 31u;
					int month = Convert.ToInt32(num2 % 12u) + 1;
					num2 /= 12u;
					int year = (num2 >= 80u) ? (Convert.ToInt32(num2) + 1900) : (Convert.ToInt32(num2) + 2000);
					result = new DateTime(year, month, day, hour, minute, second);
				}
				else
				{
					FileInfo fileInfo = new FileInfo(codPath);
					result = fileInfo.LastWriteTime;
				}
			}
			catch
			{
			}
			if (fileStream != null)
			{
				fileStream.Close();
			}
			return result;
		}
	}
}
