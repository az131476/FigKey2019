using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace Vector.VLConfig
{
	public static class ImageUtils
	{
		public enum OverlayAligment
		{
			Center,
			TopLeft,
			TopRight,
			BottomLeft,
			BottomRight
		}

		public enum OverlayPos
		{
			TopRight,
			TopLeft,
			BottomLeft,
			BottomRight
		}

		public static Image MakeGrayscale(Image original)
		{
			Bitmap bitmap = new Bitmap(original.Width, original.Height);
			Graphics graphics = Graphics.FromImage(bitmap);
			float[][] array = new float[5][];
			array[0] = new float[]
			{
				0.3f,
				0.3f,
				0.3f,
				0f,
				0f
			};
			array[1] = new float[]
			{
				0.59f,
				0.59f,
				0.59f,
				0f,
				0f
			};
			array[2] = new float[]
			{
				0.11f,
				0.11f,
				0.11f,
				0f,
				0f
			};
			float[][] arg_6E_0 = array;
			int arg_6E_1 = 3;
			float[] array2 = new float[5];
			array2[3] = 1f;
			arg_6E_0[arg_6E_1] = array2;
			array[4] = new float[]
			{
				0f,
				0f,
				0f,
				0f,
				1f
			};
			ColorMatrix colorMatrix = new ColorMatrix(array);
			ImageAttributes imageAttributes = new ImageAttributes();
			imageAttributes.SetColorMatrix(colorMatrix);
			graphics.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height), 0, 0, original.Width, original.Height, GraphicsUnit.Pixel, imageAttributes);
			graphics.Dispose();
			return bitmap;
		}

		public static Image MakeOverlay(Image imgOriginal, Image imgOverlay, ImageUtils.OverlayAligment alignment = ImageUtils.OverlayAligment.TopLeft)
		{
			Rectangle rect = new Rectangle(Point.Empty, imgOriginal.Size);
			Bitmap bitmap;
			Graphics graphics;
			if (imgOriginal.Size == imgOverlay.Size)
			{
				bitmap = (imgOriginal.Clone() as Bitmap);
				graphics = Graphics.FromImage(bitmap);
			}
			else
			{
				int width = (imgOriginal.Width > imgOverlay.Width) ? imgOriginal.Width : imgOverlay.Width;
				int height = (imgOriginal.Height > imgOverlay.Height) ? imgOriginal.Height : imgOverlay.Height;
				bitmap = new Bitmap(width, height);
				graphics = Graphics.FromImage(bitmap);
				graphics.DrawImageUnscaled(imgOriginal, rect);
			}
			switch (alignment)
			{
			case ImageUtils.OverlayAligment.TopLeft:
				rect = new Rectangle(Point.Empty, imgOverlay.Size);
				break;
			case ImageUtils.OverlayAligment.TopRight:
				rect = new Rectangle(new Point(bitmap.Width - imgOverlay.Width, 0), imgOverlay.Size);
				break;
			case ImageUtils.OverlayAligment.BottomLeft:
				rect = new Rectangle(new Point(0, bitmap.Height - imgOverlay.Height), imgOverlay.Size);
				break;
			case ImageUtils.OverlayAligment.BottomRight:
				rect = new Rectangle(new Point(bitmap.Width - imgOverlay.Width, bitmap.Height - imgOverlay.Height), imgOverlay.Size);
				break;
			default:
				rect = new Rectangle(new Point((bitmap.Width - imgOverlay.Width) / 2, bitmap.Height - imgOverlay.Height / 2), imgOverlay.Size);
				break;
			}
			graphics.DrawImage(imgOverlay, rect);
			graphics.Dispose();
			return bitmap;
		}

		public static Image ScaleImageForOverlay(Image imgOriginal, ImageUtils.OverlayPos pos = ImageUtils.OverlayPos.BottomRight, int overlaySize = 3)
		{
			Bitmap bitmap = new Bitmap(imgOriginal.Width, imgOriginal.Height);
			Graphics graphics = Graphics.FromImage(bitmap);
			if (overlaySize < 0)
			{
				overlaySize = 0;
			}
			if (overlaySize > Math.Min(imgOriginal.Width, imgOriginal.Height) / 2 - 1)
			{
				overlaySize = Math.Min(imgOriginal.Width, imgOriginal.Height) / 2 - 1;
			}
			int x = imgOriginal.Width / 2 - overlaySize;
			int y = imgOriginal.Height / 2 - overlaySize;
			int width = imgOriginal.Width / 2 + overlaySize;
			int height = imgOriginal.Height / 2 + overlaySize;
			Point location;
			switch (pos)
			{
			case ImageUtils.OverlayPos.TopRight:
				location = new Point(x, 0);
				break;
			case ImageUtils.OverlayPos.TopLeft:
				location = new Point(0, 0);
				break;
			case ImageUtils.OverlayPos.BottomLeft:
				location = new Point(0, y);
				break;
			default:
				location = new Point(x, y);
				break;
			}
			Size size = new Size(width, height);
			graphics.DrawImage(imgOriginal, new Rectangle(location, size), new Rectangle(0, 0, imgOriginal.Width, imgOriginal.Height), GraphicsUnit.Pixel);
			return bitmap;
		}
	}
}
