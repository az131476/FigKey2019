using DevExpress.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using Vector.UtilityFunctions;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig
{
	public class MainImageList
	{
		public enum IconIndex
		{
			NoImage,
			Error,
			Warning,
			Info,
			RawMessageCAN,
			RawMessageFlexray,
			RawMessageLIN,
			SymbolicMessageCAN,
			SymbolicMessageFlexray,
			SymbolicMessageLIN,
			SymbolicSignalCAN,
			SymbolicSignalFlexray,
			SymbolicSignalLIN,
			Key,
			DigitalInput,
			AnalogInput,
			CANBusStatistics,
			DataTriggerCAN,
			DataTriggerFlexray,
			DataTriggerLIN,
			OnIgnition,
			VoCanRecoding,
			MessageTimeoutCAN,
			MessageTimeoutFlexray,
			MessageTimeoutLIN,
			ConditionGroup,
			ChannelFilterCAN,
			ChannelFilterFlexray,
			ChannelFilterLIN,
			Start,
			CyclicTimer,
			CcpXcpSignal,
			DiagnosticSignal,
			SystemVariable,
			LtlSystemInformation,
			SymbolicSignalCANList,
			CANoe,
			CANalyzer,
			InstanceParameter,
			InputParameter,
			OutputParameter,
			IncludeFile,
			CANape,
			vSignalyzer
		}

		private static MainImageList sInstance;

		private readonly XtraImageCollection mImageList;

		private readonly int mMainImageListCount;

		private readonly Dictionary<int, int> mDictOverlay = new Dictionary<int, int>();

		public static MainImageList Instance
		{
			get
			{
				MainImageList arg_17_0;
				if ((arg_17_0 = MainImageList.sInstance) == null)
				{
					arg_17_0 = (MainImageList.sInstance = new MainImageList());
				}
				return arg_17_0;
			}
		}

		public Image this[int idx]
		{
			get
			{
				return this.mImageList[idx];
			}
		}

		public ImageList ImageList
		{
			get
			{
				return this.mImageList.ImageList;
			}
		}

		public ImageCollection ImageCollection
		{
			get
			{
				return this.mImageList.ImageCollection;
			}
		}

		private MainImageList()
		{
			Application.ApplicationExit += new EventHandler(MainImageList.OnApplicationExit);
			this.mImageList = new XtraImageCollection();
			this.mImageList.Add<MainImageList.IconIndex>(MainImageList.IconIndex.NoImage, new Bitmap(16, 16, PixelFormat.Format32bppArgb));
			this.mImageList.Add<MainImageList.IconIndex>(MainImageList.IconIndex.Error, Resources.IconError);
			this.mImageList.Add<MainImageList.IconIndex>(MainImageList.IconIndex.Warning, Resources.IconWarning);
			this.mImageList.Add<MainImageList.IconIndex>(MainImageList.IconIndex.Info, Resources.ImageAbout);
			this.mImageList.Add<MainImageList.IconIndex>(MainImageList.IconIndex.RawMessageCAN, Resources.ImageIDMessageCAN);
			this.mImageList.Add<MainImageList.IconIndex>(MainImageList.IconIndex.RawMessageFlexray, Resources.ImageIDMessageFlexray);
			this.mImageList.Add<MainImageList.IconIndex>(MainImageList.IconIndex.RawMessageLIN, Resources.ImageIDMessageLIN);
			this.mImageList.Add<MainImageList.IconIndex>(MainImageList.IconIndex.SymbolicMessageCAN, Resources.ImageSymbMessageCAN);
			this.mImageList.Add<MainImageList.IconIndex>(MainImageList.IconIndex.SymbolicMessageFlexray, Resources.ImageSymbMessageFlexray);
			this.mImageList.Add<MainImageList.IconIndex>(MainImageList.IconIndex.SymbolicMessageLIN, Resources.ImageSymbMessageLIN);
			this.mImageList.Add<MainImageList.IconIndex>(MainImageList.IconIndex.SymbolicSignalCAN, Resources.ImageSymbSignalCAN);
			this.mImageList.Add<MainImageList.IconIndex>(MainImageList.IconIndex.SymbolicSignalFlexray, Resources.ImageSymbSignalFlexRay);
			this.mImageList.Add<MainImageList.IconIndex>(MainImageList.IconIndex.SymbolicSignalLIN, Resources.ImageSymbSignalLIN);
			this.mImageList.Add<MainImageList.IconIndex>(MainImageList.IconIndex.Key, Resources.ImageKey);
			this.mImageList.Add<MainImageList.IconIndex>(MainImageList.IconIndex.DigitalInput, Resources.ImageDigitalInput);
			this.mImageList.Add<MainImageList.IconIndex>(MainImageList.IconIndex.AnalogInput, Resources.ImageAnalogInputSignal);
			this.mImageList.Add<MainImageList.IconIndex>(MainImageList.IconIndex.CANBusStatistics, Resources.ImageCANBusStatistics);
			this.mImageList.Add<MainImageList.IconIndex>(MainImageList.IconIndex.DataTriggerCAN, Resources.ImageDataTriggerCAN);
			this.mImageList.Add<MainImageList.IconIndex>(MainImageList.IconIndex.DataTriggerFlexray, Resources.ImageDataTriggerFlexray);
			this.mImageList.Add<MainImageList.IconIndex>(MainImageList.IconIndex.DataTriggerLIN, Resources.ImageDataTriggerLIN);
			this.mImageList.Add<MainImageList.IconIndex>(MainImageList.IconIndex.OnIgnition, Resources.ImageOnIgnition);
			this.mImageList.Add<MainImageList.IconIndex>(MainImageList.IconIndex.VoCanRecoding, Resources.ImageVoCanRecoding);
			this.mImageList.Add<MainImageList.IconIndex>(MainImageList.IconIndex.MessageTimeoutCAN, Resources.ImageCycMsgTimeoutCAN);
			this.mImageList.Add<MainImageList.IconIndex>(MainImageList.IconIndex.MessageTimeoutFlexray, Resources.ImageCycMsgTimeoutFlexray);
			this.mImageList.Add<MainImageList.IconIndex>(MainImageList.IconIndex.MessageTimeoutLIN, Resources.ImageCycMsgTimeoutLIN);
			this.mImageList.Add<MainImageList.IconIndex>(MainImageList.IconIndex.ConditionGroup, Resources.ImageConditionGroup);
			this.mImageList.Add<MainImageList.IconIndex>(MainImageList.IconIndex.ChannelFilterCAN, Resources.ImageChnFilterCAN);
			this.mImageList.Add<MainImageList.IconIndex>(MainImageList.IconIndex.ChannelFilterFlexray, Resources.ImageChnFilterFlexray);
			this.mImageList.Add<MainImageList.IconIndex>(MainImageList.IconIndex.ChannelFilterLIN, Resources.ImageChnFilterLIN);
			this.mImageList.Add<MainImageList.IconIndex>(MainImageList.IconIndex.Start, Resources.ImageStart);
			this.mImageList.Add<MainImageList.IconIndex>(MainImageList.IconIndex.CyclicTimer, Resources.ImageCyclicTimer);
			this.mImageList.Add<MainImageList.IconIndex>(MainImageList.IconIndex.CcpXcpSignal, Resources.ImageSymbSignalCcpXcp);
			this.mImageList.Add<MainImageList.IconIndex>(MainImageList.IconIndex.DiagnosticSignal, Resources.ImageDiagSignal);
			this.mImageList.Add<MainImageList.IconIndex>(MainImageList.IconIndex.SystemVariable, Resources.ImageSystemVariable);
			this.mImageList.Add<MainImageList.IconIndex>(MainImageList.IconIndex.LtlSystemInformation, Resources.IconDeviceInformation.ToBitmap());
			this.mImageList.Add<MainImageList.IconIndex>(MainImageList.IconIndex.SymbolicSignalCANList, Resources.ImageSymbSignalList);
			this.mImageList.Add<MainImageList.IconIndex>(MainImageList.IconIndex.CANoe, Resources.IconCANoe);
			this.mImageList.Add<MainImageList.IconIndex>(MainImageList.IconIndex.CANalyzer, Resources.IconCANalyzer);
			this.mImageList.Add<MainImageList.IconIndex>(MainImageList.IconIndex.InstanceParameter, Resources.ImageInstanceParameter);
			this.mImageList.Add<MainImageList.IconIndex>(MainImageList.IconIndex.InputParameter, Resources.ImageInputParameter);
			this.mImageList.Add<MainImageList.IconIndex>(MainImageList.IconIndex.OutputParameter, Resources.ImageOutputParameter);
			this.mImageList.Add<MainImageList.IconIndex>(MainImageList.IconIndex.IncludeFile, Resources.ImageIncludeFile);
			this.mImageList.Add<MainImageList.IconIndex>(MainImageList.IconIndex.CANape, Resources.IconCANape);
			this.mImageList.Add<MainImageList.IconIndex>(MainImageList.IconIndex.vSignalyzer, Resources.IconvSignalyzer);
			this.mMainImageListCount = this.mImageList.ImageCollection.Images.Count;
		}

		public static void ReleaseSingleton()
		{
			if (MainImageList.sInstance != null)
			{
				MainImageList.sInstance.mImageList.Dispose();
			}
			MainImageList.sInstance = null;
		}

		public Image GetImage(MainImageList.IconIndex baseImgIndex)
		{
			return this.ImageList.Images[this.GetImageIndex(baseImgIndex, false)];
		}

		public int GetImageIndex(MainImageList.IconIndex baseImgIndex, bool grayscale = false)
		{
			return this.GetImageIndex((int)baseImgIndex, grayscale);
		}

		private int GetImageIndex(int baseImgIndex, bool grayscale = false)
		{
			if (baseImgIndex < 0 || baseImgIndex >= this.mMainImageListCount)
			{
				return 0;
			}
			if (!grayscale)
			{
				return baseImgIndex;
			}
			int key = this.mMainImageListCount + baseImgIndex;
			int num;
			if (this.mDictOverlay.TryGetValue(key, out num))
			{
				return num;
			}
			Image img = ImageUtils.MakeGrayscale(this.mImageList.GetImage(baseImgIndex));
			num = this.mMainImageListCount + this.mDictOverlay.Count;
			this.mImageList.Add(num, img);
			this.mDictOverlay[key] = num;
			return num;
		}

		public int GetImageIndex(MainImageList.IconIndex baseImgIndex, MainImageList.IconIndex overlayIndex, ImageUtils.OverlayPos overlayPos = ImageUtils.OverlayPos.BottomRight, bool baseImageAsGrayscale = false)
		{
			return this.GetImageIndex((int)baseImgIndex, (int)overlayIndex, overlayPos, baseImageAsGrayscale);
		}

		private int GetImageIndex(int baseImgIndex, int overlayIndex, ImageUtils.OverlayPos overlayPos = ImageUtils.OverlayPos.BottomRight, bool baseImageAsGrayscale = false)
		{
			if (baseImgIndex < 0 || baseImgIndex >= this.mMainImageListCount)
			{
				return 0;
			}
			if (overlayIndex < 0 || overlayIndex >= this.mMainImageListCount)
			{
				return 0;
			}
			int num = baseImageAsGrayscale ? 2 : 1;
			int key = this.mMainImageListCount + this.mMainImageListCount * (overlayIndex + 1) * (int)overlayPos * num + baseImgIndex;
			int num2;
			if (this.mDictOverlay.TryGetValue(key, out num2))
			{
				return num2;
			}
			Image image = this.mImageList.GetImage(overlayIndex);
			image = ImageUtils.ScaleImageForOverlay(image, overlayPos, 3);
			Image imgOriginal = baseImageAsGrayscale ? this.mImageList.GetImage(this.GetImageIndex(baseImgIndex, true)) : this.mImageList.GetImage(baseImgIndex);
			Image img = ImageUtils.MakeOverlay(imgOriginal, image, ImageUtils.OverlayAligment.TopLeft);
			num2 = this.mMainImageListCount + this.mDictOverlay.Count;
			this.mImageList.Add(num2, img);
			this.mDictOverlay[key] = num2;
			return num2;
		}

		private static void OnApplicationExit(object sender, EventArgs e)
		{
			MainImageList.ReleaseSingleton();
		}
	}
}
