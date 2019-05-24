using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using Vector.VLConfig.HardwareAccess.ToolInterfaces;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.BusinessLogic
{
	public class FileSplitMerge
	{
		public delegate void ProcessExitedDelegate();

		private static string _sourceFilePath;

		private static string _destFolderPath;

		private static string _destFilePath;

		private static int _fileSizeInBytes;

		private static IProgressIndicator _progressIndicator;

		private static string _errorText;

		private static FileSplitMerge.ProcessExitedDelegate _processExitedDelegate;

		private static readonly long ProgressScale = 10000L;

		private static readonly int BufferSize = 65535;

		private static readonly int MagicIdLength = 20;

		private static readonly int ExtensionLength = 20;

		private static readonly int ReservedLength = 100;

		private static readonly string MagicIdKeyword = "SplitJoin";

		private static readonly int HeaderVersion = 1;

		public static bool SplitFile(string filePath, uint sizeInKB, IProgressIndicator pi, FileSplitMerge.ProcessExitedDelegate processExitedDelegate)
		{
			if (!File.Exists(filePath))
			{
				return false;
			}
			FileSplitMerge._sourceFilePath = filePath;
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(filePath);
			FileSplitMerge._destFolderPath = Path.Combine(Path.GetDirectoryName(filePath), fileNameWithoutExtension);
			FileSplitMerge._processExitedDelegate = processExitedDelegate;
			if (Directory.Exists(FileSplitMerge._destFolderPath))
			{
				if (Directory.GetFiles(FileSplitMerge._destFolderPath, string.Format("{0}.*", fileNameWithoutExtension)).Count<string>() > 0)
				{
					FileSplitMerge._errorText = string.Format(Resources.ErrorDestDirNotEmptySplitAborted, FileSplitMerge._destFolderPath);
					return false;
				}
			}
			else
			{
				try
				{
					Directory.CreateDirectory(FileSplitMerge._destFolderPath);
				}
				catch (Exception)
				{
					FileSplitMerge._errorText = string.Format(Resources.ErrorFailedCreateDestDir, FileSplitMerge._destFolderPath);
					return false;
				}
			}
			FileSplitMerge._fileSizeInBytes = (int)(sizeInKB * 1024u);
			FileSplitMerge._progressIndicator = pi;
			FileSplitMerge._progressIndicator.SetMinMax(0, (int)FileSplitMerge.ProgressScale);
			Thread thread = new Thread(new ThreadStart(FileSplitMerge.ProcessSplitFile));
			thread.Start();
			return true;
		}

		public static bool PeekStartSplitFile(string startSplitFilePath, out string fileExtension)
		{
			fileExtension = "";
			if (!File.Exists(startSplitFilePath))
			{
				return false;
			}
			FileStream fileStream = null;
			try
			{
				fileStream = File.Open(startSplitFilePath, FileMode.Open, FileAccess.Read);
			}
			catch (Exception)
			{
				FileSplitMerge._errorText = string.Format(Resources.ErrorFailedOpenSplitFile, startSplitFilePath);
				bool result = false;
				return result;
			}
			int num;
			bool flag = FileSplitMerge.ReadFirstFileHeader(ref fileStream, out fileExtension, out num);
			fileStream.Close();
			if (!flag)
			{
				return false;
			}
			for (int i = 1; i < num; i++)
			{
				string path = Path.ChangeExtension(startSplitFilePath, string.Format("{0:d3}", i));
				if (!File.Exists(path))
				{
					FileSplitMerge._errorText = string.Format(Resources.ErrorMissingSplitFiles, num, startSplitFilePath);
					return false;
				}
			}
			return true;
		}

		public static bool MergeFiles(string startSplitFilePath, string destFilePath, IProgressIndicator pi, FileSplitMerge.ProcessExitedDelegate processExitedDelegate)
		{
			if (!File.Exists(startSplitFilePath))
			{
				return false;
			}
			FileSplitMerge._sourceFilePath = startSplitFilePath;
			FileSplitMerge._destFilePath = destFilePath;
			if (File.Exists(destFilePath))
			{
				try
				{
					File.Delete(destFilePath);
				}
				catch (Exception)
				{
					FileSplitMerge._errorText = string.Format(Resources.ErrorFailedOverwrite, FileSplitMerge._destFilePath);
					return false;
				}
			}
			FileSplitMerge._progressIndicator = pi;
			FileSplitMerge._progressIndicator.SetMinMax(0, (int)FileSplitMerge.ProgressScale);
			FileSplitMerge._processExitedDelegate = processExitedDelegate;
			Thread thread = new Thread(new ThreadStart(FileSplitMerge.ProcessMergeFiles));
			thread.Start();
			return true;
		}

		public static string GetLastErrorText()
		{
			return FileSplitMerge._errorText;
		}

		private static void ProcessSplitFile()
		{
			string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(FileSplitMerge._sourceFilePath);
			int num = 0;
			bool flag = false;
			string text = "";
			int num2 = 0;
			int num3 = 0;
			FileStream fileStream = null;
			FileStream fileStream2 = null;
			FileSplitMerge._errorText = "";
			int num4 = 0;
			try
			{
				fileStream = File.Open(FileSplitMerge._sourceFilePath, FileMode.Open, FileAccess.Read);
				FileInfo fileInfo = new FileInfo(FileSplitMerge._sourceFilePath);
				num4 = (int)(fileInfo.Length / (long)FileSplitMerge._fileSizeInBytes);
				if (fileInfo.Length % (long)FileSplitMerge._fileSizeInBytes > 0L)
				{
					num4++;
				}
				num3 = (int)(fileInfo.Length / 1024L);
			}
			catch (Exception)
			{
				FileSplitMerge._errorText = string.Format(Resources.ErrorFailedOpenForSplit, FileSplitMerge._sourceFilePath);
				return;
			}
			byte[] array = new byte[FileSplitMerge.BufferSize];
			int num5 = 0;
			int num6 = 0;
			int num7 = 0;
			while (!flag)
			{
				if (num6 == 0)
				{
					goto IL_D5;
				}
				if (num6 == num7)
				{
					goto Block_3;
				}
				IL_107:
				if (!FileSplitMerge._progressIndicator.Cancelled())
				{
					if (num6 > 0)
					{
						if (num5 >= FileSplitMerge._fileSizeInBytes && fileStream2 != null)
						{
							fileStream2.Close();
							num5 = 0;
						}
						if (num5 == 0)
						{
							text = string.Format("{0}.{1:d3}", fileNameWithoutExtension, num);
							string path = Path.Combine(FileSplitMerge._destFolderPath, text);
							num++;
							try
							{
								fileStream2 = File.Create(path, FileSplitMerge.BufferSize, FileOptions.WriteThrough);
								FileSplitMerge._progressIndicator.SetStatusText(string.Format(Resources.WritingFileNumOfTotal, text, num, num4));
							}
							catch (Exception)
							{
								FileSplitMerge._errorText = string.Format(Resources.ErrorFailedCreateDestFile, text);
								break;
							}
							if (num == 1)
							{
								int num8;
								if (!FileSplitMerge.WriteFirstFileHeader(ref fileStream2, Path.GetExtension(FileSplitMerge._sourceFilePath), num4, out num8))
								{
									FileSplitMerge._errorText = string.Format(Resources.ErrorFailedWriteDestFile, text);
									break;
								}
								num5 += num8;
							}
						}
						int num9 = num6 - num7;
						if (num5 + (num6 - num7) >= FileSplitMerge._fileSizeInBytes)
						{
							num9 = FileSplitMerge._fileSizeInBytes - num5;
						}
						if (fileStream2 == null)
						{
							FileSplitMerge._errorText = string.Format(Resources.ErrorFailedCreateDestFile, text);
							break;
						}
						try
						{
							fileStream2.Write(array, num7, num9);
							num5 += num9;
							num7 += num9;
							num2 += num9 / 1024;
							FileSplitMerge._progressIndicator.SetValue((int)(FileSplitMerge.ProgressScale * (long)num2 / (long)num3));
							continue;
						}
						catch (Exception)
						{
							FileSplitMerge._errorText = string.Format(Resources.ErrorFailedWriteDestFile, text);
							break;
						}
					}
					FileSplitMerge._errorText = "";
					flag = true;
					continue;
				}
				break;
				Block_3:
				try
				{
					IL_D5:
					num6 = fileStream.Read(array, 0, array.Length);
					num7 = 0;
				}
				catch (Exception)
				{
					FileSplitMerge._errorText = string.Format(Resources.ErrorFailedReadSourceFile, FileSplitMerge._sourceFilePath);
					break;
				}
				goto IL_107;
			}
			fileStream.Close();
			if (fileStream2 != null)
			{
				fileStream2.Close();
			}
			FileSplitMerge._processExitedDelegate();
		}

		private static void ProcessMergeFiles()
		{
			FileStream fileStream = null;
			FileStream fileStream2 = null;
			int num = 0;
			FileSplitMerge._errorText = "";
			try
			{
				fileStream2 = File.Create(FileSplitMerge._destFilePath, FileSplitMerge.BufferSize, FileOptions.WriteThrough);
			}
			catch (Exception)
			{
				FileSplitMerge._errorText = string.Format(Resources.ErrorFailedWriteDestFile, FileSplitMerge._destFilePath);
				FileSplitMerge._processExitedDelegate();
				return;
			}
			try
			{
				fileStream = File.Open(FileSplitMerge._sourceFilePath, FileMode.Open, FileAccess.Read);
				FileInfo fileInfo = new FileInfo(FileSplitMerge._sourceFilePath);
				num = (int)(fileInfo.Length / 1024L);
			}
			catch (Exception)
			{
				FileSplitMerge._errorText = string.Format(Resources.ErrorFailedOpenSplitFile, FileSplitMerge._sourceFilePath);
				FileSplitMerge._processExitedDelegate();
				return;
			}
			string text;
			int num2;
			if (FileSplitMerge.ReadFirstFileHeader(ref fileStream, out text, out num2))
			{
				int num3 = num2 * num;
				FileSplitMerge._progressIndicator.SetStatusText(string.Format(Resources.ReadingFileNumOfTotal, Path.GetFileName(FileSplitMerge._sourceFilePath), 1, num2));
				byte[] array = new byte[FileSplitMerge.BufferSize];
				int num4 = 0;
				for (int i = 0; i < num2; i++)
				{
					if (i > 0)
					{
						fileStream.Close();
						FileSplitMerge._sourceFilePath = Path.ChangeExtension(FileSplitMerge._sourceFilePath, string.Format("{0:d3}", i));
						try
						{
							fileStream = File.Open(FileSplitMerge._sourceFilePath, FileMode.Open, FileAccess.Read);
						}
						catch (Exception)
						{
							FileSplitMerge._errorText = string.Format(Resources.ErrorFailedOpenSplitFile, FileSplitMerge._sourceFilePath);
							break;
						}
						FileSplitMerge._progressIndicator.SetStatusText(string.Format(Resources.ReadingFileNumOfTotal, Path.GetFileName(FileSplitMerge._sourceFilePath), i + 1, num2));
					}
					bool flag = false;
					int num5 = 0;
					while (!flag)
					{
						try
						{
							num5 = fileStream.Read(array, 0, array.Length);
						}
						catch (Exception)
						{
							FileSplitMerge._errorText = string.Format(Resources.ErrorFailedReadSourceFile, FileSplitMerge._sourceFilePath);
							break;
						}
						if (num5 == 0)
						{
							flag = true;
							break;
						}
						try
						{
							fileStream2.Write(array, 0, num5);
							num4 += num5 / 1024;
							FileSplitMerge._progressIndicator.SetValue((int)(FileSplitMerge.ProgressScale * (long)num4 / (long)num3));
						}
						catch (Exception)
						{
							FileSplitMerge._errorText = string.Format(Resources.ErrorFailedWriteDestFile, FileSplitMerge._destFilePath);
							break;
						}
					}
					if (!flag)
					{
						break;
					}
				}
				fileStream2.Close();
				fileStream.Close();
				FileSplitMerge._processExitedDelegate();
				return;
			}
			fileStream.Close();
			FileSplitMerge._processExitedDelegate();
		}

		private static bool WriteFirstFileHeader(ref FileStream fs, string fileExtension, int numberOfFiles, out int numberOfByesWritten)
		{
			numberOfByesWritten = 0;
			byte[] buffer = FileSplitMerge.StringToByteArray(FileSplitMerge.MagicIdKeyword);
			Array.Resize<byte>(ref buffer, FileSplitMerge.MagicIdLength);
			byte[] bytes = BitConverter.GetBytes(FileSplitMerge.HeaderVersion);
			byte[] bytes2 = BitConverter.GetBytes(numberOfFiles);
			byte[] buffer2 = FileSplitMerge.StringToByteArray(fileExtension);
			Array.Resize<byte>(ref buffer2, FileSplitMerge.ExtensionLength);
			byte[] buffer3 = new byte[FileSplitMerge.ReservedLength];
			try
			{
				fs.Write(buffer, 0, FileSplitMerge.MagicIdLength);
				numberOfByesWritten += FileSplitMerge.MagicIdLength;
				fs.Write(bytes, 0, 4);
				numberOfByesWritten += 4;
				fs.Write(bytes2, 0, 4);
				numberOfByesWritten += 4;
				fs.Write(buffer2, 0, FileSplitMerge.ExtensionLength);
				numberOfByesWritten += FileSplitMerge.ExtensionLength;
				fs.Write(buffer3, 0, FileSplitMerge.ReservedLength);
				numberOfByesWritten += FileSplitMerge.ReservedLength;
			}
			catch (Exception)
			{
				return false;
			}
			return true;
		}

		private static bool ReadFirstFileHeader(ref FileStream fs, out string fileExtension, out int numberOfFiles)
		{
			numberOfFiles = 0;
			fileExtension = "";
			byte[] array = new byte[FileSplitMerge.MagicIdLength];
			byte[] array2 = new byte[4];
			byte[] array3 = new byte[4];
			byte[] array4 = new byte[FileSplitMerge.ExtensionLength];
			byte[] array5 = new byte[FileSplitMerge.ReservedLength];
			try
			{
				fs.Read(array, 0, array.Length);
				fs.Read(array2, 0, array2.Length);
				fs.Read(array3, 0, array3.Length);
				fs.Read(array4, 0, array4.Length);
				fs.Read(array5, 0, array5.Length);
			}
			catch (Exception)
			{
				FileSplitMerge._errorText = string.Format(Resources.ErrorFailedReadSourceFile, FileSplitMerge._sourceFilePath);
				bool result = false;
				return result;
			}
			string text = FileSplitMerge.ByteArrayToString(array);
			if (!text.StartsWith(FileSplitMerge.MagicIdKeyword))
			{
				FileSplitMerge._errorText = string.Format(Resources.ErrorIsNotASplitFile, FileSplitMerge._sourceFilePath);
				return false;
			}
			int num = BitConverter.ToInt32(array2, 0);
			if (num > FileSplitMerge.HeaderVersion)
			{
				FileSplitMerge._errorText = string.Format(Resources.ErrorContainsWrongHeaderVer, FileSplitMerge._sourceFilePath);
				return false;
			}
			numberOfFiles = BitConverter.ToInt32(array3, 0);
			fileExtension = FileSplitMerge.ByteArrayToString(array4);
			return true;
		}

		private static byte[] StringToByteArray(string str)
		{
			ASCIIEncoding aSCIIEncoding = new ASCIIEncoding();
			return aSCIIEncoding.GetBytes(str);
		}

		private static string ByteArrayToString(byte[] arr)
		{
			ASCIIEncoding aSCIIEncoding = new ASCIIEncoding();
			string text = aSCIIEncoding.GetString(arr);
			int num = text.IndexOf('\0');
			if (num >= 0)
			{
				text = text.Substring(0, num);
			}
			return text;
		}
	}
}
