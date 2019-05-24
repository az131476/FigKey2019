using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.GeneralUtil;
using Vector.VLConfig.Properties;
using Vector.VLConfig.VN16XXlogWrapper;
using Vector.XlApiNet;

namespace Vector.VLConfig.HardwareAccess.HardwareAbstraction.VN16XXlog
{
	internal static class VN16XXlogUtils
	{
		private static readonly string sDefaultHwTypeName = XlUtils.GetHwName(XlHwType.XL_HWTYPE_VN1630_LOG);

		public static Result ReadFirmwareImage(string filePath, out byte[] data, out Version imageVersion, out string errorText)
		{
			imageVersion = new Version();
			if (VN16XXlogUtils.ReadFile(filePath, out data, out errorText) != Result.OK)
			{
				return Result.Error;
			}
			if (ImageEncryption.DecryptImage(data) != 0)
			{
				errorText = string.Format(Resources.ErrorInvalidFirmwareImage, filePath);
				return Result.Error;
			}
			int trailerLength = VN16XXlogUtils.GetTrailerLength();
			int count = data.Length - trailerLength;
			Vn1630LogFwImageInfoTrailer vn1630LogFwImageInfoTrailer = XlUtils.BytesToStruct<Vn1630LogFwImageInfoTrailer>(data.Skip(count).Take(trailerLength).ToArray<byte>());
			imageVersion = new Version((int)vn1630LogFwImageInfoTrailer.versionFwImage.major, (int)vn1630LogFwImageInfoTrailer.versionFwImage.minor, (int)vn1630LogFwImageInfoTrailer.versionFwImage.build);
			return Result.OK;
		}

		public static Result DownloadFirmwareImage(byte[] firmwareImage, VN16XXlogDevice device, out string errorText)
		{
			errorText = string.Empty;
			if (device == null)
			{
				errorText = string.Format(Resources.ErrorUnableToAccessDevice, VN16XXlogUtils.sDefaultHwTypeName);
				return Result.Error;
			}
			s_xl_channel_config s_xl_channel_config;
			if (device.HwInfo == null || !device.HwInfo.GetFirstCanChannel(out s_xl_channel_config))
			{
				errorText = string.Format(Resources.ErrorUnableToAccessDevice, device.GetHwTypeName());
				return Result.Error;
			}
			XlApi xlApi = new XlApi();
			using (XlPort xlPort = new XlPort(xlApi, Application.ProductName, s_xl_channel_config.channelMask, 131072u, 0u, s_xl_channel_config.busParams.busType))
			{
				if (xlPort.Status != XlStatus.XL_SUCCESS)
				{
					errorText = string.Format(Resources.ErrorUnableToAccessDevice, device.GetHwTypeName());
					Result result = Result.Error;
					return result;
				}
				int num;
				if (xlApi.XlCobFwkLoadCobBinary(xlPort.PortHandle, xlPort.AccessMask, firmwareImage, (uint)firmwareImage.Length, out num) != XlStatus.XL_SUCCESS || num != 0)
				{
					errorText = string.Format(Resources.ErrorUnableToAccessDevice, device.GetHwTypeName());
					Result result = Result.Error;
					return result;
				}
			}
			return Result.OK;
		}

		public static Result EncryptCodeFile(string baseFilePath, out string errorText)
		{
			string text = baseFilePath + Vocabulary.FileExtensionDotCOB;
			string filePath = baseFilePath + Vocabulary.FileExtensionDotBIN;
			byte[] array;
			if (VN16XXlogUtils.ReadFile(text, out array, out errorText) != Result.OK)
			{
				return Result.Error;
			}
			if (ImageEncryption.EncryptImage(array) != 0)
			{
				errorText = string.Format(Resources.ErrorInvalidFirmwareImage, text);
				return Result.Error;
			}
			if (VN16XXlogUtils.WriteFile(filePath, array, out errorText) != Result.OK)
			{
				return Result.Error;
			}
			return Result.OK;
		}

		private static Result ReadFile(string filePath, out byte[] data, out string errorText)
		{
			errorText = string.Empty;
			data = new byte[0];
			try
			{
				data = File.ReadAllBytes(filePath);
			}
			catch
			{
				errorText = string.Format(Resources.ErrorUnableToReadFile, filePath);
				return Result.Error;
			}
			return Result.OK;
		}

		private static Result WriteFile(string filePath, byte[] data, out string errorText)
		{
			errorText = string.Empty;
			try
			{
				File.WriteAllBytes(filePath, data);
			}
			catch
			{
				errorText = string.Format(Resources.ErrorUnableToWriteFile, filePath);
				return Result.Error;
			}
			return Result.OK;
		}

		private static int GetTrailerLength()
		{
			return Marshal.SizeOf(default(Vn1630LogFwImageInfoTrailer));
		}
	}
}
