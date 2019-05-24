using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Vector.VLConfig.BusinessLogic;
using Vector.VLConfig.Properties;

namespace Vector.VLConfig.GUI.Common
{
	public class StorageCapacityBar : ProgressBar
	{
		private Font mFont = new Font("Microsoft Sans Serif", 8.25f);

		private Color mForeGroundColor = SystemColors.Highlight;

		private Color mForeGroundColorFull = Color.Red;

		private int mMinimumFreeSpace = 85;

		private bool mUseGradient;

		private string mText;

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new int MarqueeAnimationSpeed
		{
			get;
			set;
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new ProgressBarStyle Style
		{
			get;
			set;
		}

		public StorageCapacityBar()
		{
			base.SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint, true);
		}

		public void SetCapacityValues(long free, long total)
		{
			if (total <= 0L)
			{
				this.Clear();
				return;
			}
			long num = total - free;
			base.Value = (int)((double)num / (double)total * 100.0);
			string sizeStringMBForBytes = GUIUtil.GetSizeStringMBForBytes(free);
			string percentageStringForFreeSpace = GUIUtil.GetPercentageStringForFreeSpace(total, free);
			this.mText = string.Format(Resources.StorageCapacityBarText, sizeStringMBForBytes, percentageStringForFreeSpace);
			base.Invalidate();
		}

		public void Clear()
		{
			base.Value = 0;
			this.mText = string.Empty;
			base.Invalidate();
		}

		protected override void OnPaint(PaintEventArgs e)
		{
			Rectangle clientRectangle = base.ClientRectangle;
			using (Image image = new Bitmap(base.Width, base.Height))
			{
				using (Graphics graphics = Graphics.FromImage(image))
				{
					if (ProgressBarRenderer.IsSupported)
					{
						ProgressBarRenderer.DrawHorizontalBar(graphics, clientRectangle);
					}
					clientRectangle.Inflate(new Size(-2, -2));
					clientRectangle.Width = (int)((double)clientRectangle.Width * ((double)base.Value / (double)base.Maximum));
					if (clientRectangle.Width == 0)
					{
						clientRectangle.Width = 1;
					}
					Color color;
					if (base.Value < this.mMinimumFreeSpace)
					{
						color = this.mForeGroundColor;
					}
					else
					{
						color = this.mForeGroundColorFull;
					}
					Brush brush;
					if (this.mUseGradient)
					{
						brush = new LinearGradientBrush(clientRectangle, this.BackColor, color, LinearGradientMode.Vertical);
					}
					else
					{
						brush = new SolidBrush(color);
					}
					graphics.FillRectangle(brush, 2, 2, clientRectangle.Width, clientRectangle.Height);
					SizeF sizeF = graphics.MeasureString(this.mText, this.mFont);
					Point p = new Point(Convert.ToInt32((float)(base.Width / 2) - sizeF.Width / 2f), Convert.ToInt32((float)(base.Height / 2) - sizeF.Height / 2f));
					graphics.DrawString(this.mText, this.mFont, Brushes.Black, p);
					e.Graphics.DrawImage(image, 0, 0);
					image.Dispose();
				}
			}
		}

		[DllImport("uxtheme.dll")]
		private static extern int SetWindowTheme(IntPtr hWnd, string appname, string idlist);

		protected override void OnHandleCreated(EventArgs e)
		{
			StorageCapacityBar.SetWindowTheme(base.Handle, "", "");
			base.OnHandleCreated(e);
		}
	}
}
