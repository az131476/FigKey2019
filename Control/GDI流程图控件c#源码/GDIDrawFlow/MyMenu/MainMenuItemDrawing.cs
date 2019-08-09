using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.ComponentModel;

namespace  System.MyControl.MyMenu
{
	/// <summary>
	/// ���Ʋ˵���
	/// </summary>
	public class MainMenuItemDrawing
	{
		/// <summary>
		/// �������˵���
		/// </summary>
		public static void DrawMenuItem(System.Windows.Forms.DrawItemEventArgs e, MenuItem mi)
		{

			if ( (e.State & DrawItemState.HotLight) == DrawItemState.HotLight )
			{
				
				DrawHoverRect(e, mi);
			} 
			else if ( (e.State & DrawItemState.Selected) == DrawItemState.Selected ) 
			{
				DrawSelectionRect(e, mi);
			} 
			else 
			{
				Rectangle rect = new Rectangle(e.Bounds.X, e.Bounds.Y, e.Bounds.Width, 
				e.Bounds.Height -1);
				e.Graphics.FillRectangle(new SolidBrush(Globals.MainColor), rect);
				e.Graphics.DrawRectangle(new Pen(Globals.MainColor), rect);
			}
			
			StringFormat sf = new StringFormat();

			//�������־���
			sf.LineAlignment = StringAlignment.Center;
			sf.Alignment = StringAlignment.Center;

			//��������
			e.Graphics.DrawString(mi.Text, 
				Globals.menuFont, 
				new SolidBrush(Globals.TextColor), 
				e.Bounds, 
				sf);	
		}

		/// <summary>
		/// ���������������ʱ�ķ��
		/// </summary>
		private static void DrawHoverRect(System.Windows.Forms.DrawItemEventArgs e, MenuItem mi)
		{
			// ���������������ʱ�ľ���
			Rectangle rect = new Rectangle(e.Bounds.X, 
				e.Bounds.Y + 1, 
				e.Bounds.Width, 
				e.Bounds.Height - 2);

			// ���������������ʱ�ı�ˢ
			Brush b = new LinearGradientBrush(rect, 
				Color.White, 
				Globals.CheckBoxColor,
				90f, false);

			e.Graphics.FillRectangle(b, rect);

			//���Ʊ߿�
			e.Graphics.DrawRectangle(new Pen(Color.Black), rect);
		}

		/// <summary>
		/// ����ѡ��ʱ�ķ��
		/// </summary>
		private static void DrawSelectionRect(System.Windows.Forms.DrawItemEventArgs e, MenuItem mi)
		{
			// ����ѡ��ʱ�ľ��ο�
			Rectangle rect = new Rectangle(e.Bounds.X, 
				e.Bounds.Y + 1, 
				e.Bounds.Width, 
				e.Bounds.Height - 2);

			Brush b = new LinearGradientBrush(rect, 
				Globals.MenuBgColor, 
				Globals.MenuDarkColor2,
				90f, false);

			e.Graphics.FillRectangle(b, rect);

			e.Graphics.DrawRectangle(new Pen(Color.Black), rect);
		}
	}


	/// <summary>
	/// ���Ʋ˵���ķ���
	/// </summary>
	public class MenuItemDrawing
	{

		/// <summary>
		///���Ʋ˵���ʱ��������
		/// </summary>
		public static void DrawMenuItem(System.Windows.Forms.DrawItemEventArgs e, MenuItem mi)
		{
			if ( (e.State & DrawItemState.Selected) == DrawItemState.Selected ) 
			{

				DrawSelectionRect(e, mi);	
			} 
			else 
			{

				e.Graphics.FillRectangle(new SolidBrush(Globals.MenuBgColor), e.Bounds);

				DrawPictureArea(e, mi);
			}

			if ( (e.State & DrawItemState.Checked) == DrawItemState.Checked ) 
			{
				DrawCheckBox(e, mi);
			}

			DrawMenuText(e, mi);

			DrawItemPicture(e, mi);

		}

		/// <summary>
		///���Ʋ˵�����ı�����
		/// </summary>
		private static void DrawMenuText(System.Windows.Forms.DrawItemEventArgs e, MenuItem mi)
		{
			Brush textBrush = new SolidBrush(Globals.TextColor);

			if ( mi.Text == "-" ) 
			{
				e.Graphics.DrawLine(new Pen(Globals.MenuLightColor), e.Bounds.X + Globals.PIC_AREA_SIZE + 3, e.Bounds.Y + 2, e.Bounds.Width, e.Bounds.Y + 2);
			} 
			else 
			{
				StringFormat sf = new StringFormat();
				sf.LineAlignment = StringAlignment.Center;

				RectangleF rect = new Rectangle(Globals.PIC_AREA_SIZE + 2, e.Bounds.Y, e.Bounds.Width, e.Bounds.Height);

				string miText = mi.Text.Replace("&","");

				if ( mi.Enabled )
					textBrush = new SolidBrush(Globals.TextColor);	
				else
					textBrush = new SolidBrush(Globals.TextDisabledColor);	
				
				e.Graphics.DrawString(miText, Globals.menuFont, textBrush, rect, sf);

				DrawShortCutText(e, mi);
			}
		}

		/// <summary>
		/// ���Ʋ˵���Ŀ�����ķ���
		/// </summary>
		private static void DrawShortCutText(System.Windows.Forms.DrawItemEventArgs e, MenuItem mi)
		{
			if ( mi.Shortcut != Shortcut.None && mi.ShowShortcut == true)
			{
				SizeF scSize = 
					e.Graphics.MeasureString(mi.Shortcut.ToString(), 
					Globals.menuFont);

				Rectangle rect = 
					new Rectangle(e.Bounds.Width - Convert.ToInt32(scSize.Width) - Globals.PIC_AREA_SIZE,
					e.Bounds.Y,
					Convert.ToInt32(scSize.Width) + 5,
					e.Bounds.Height);

				StringFormat sf = new StringFormat();
				sf.FormatFlags = StringFormatFlags.DirectionRightToLeft;
				sf.LineAlignment = StringAlignment.Center;

				if ( mi.Enabled )
					e.Graphics.DrawString(mi.Shortcut.ToString(), 
						Globals.menuFont, 
						new SolidBrush(Globals.TextColor), 
						rect, 
						sf);
				else 
				{	
					e.Graphics.DrawString(mi.Shortcut.ToString(), 
						Globals.menuFont, 
						new SolidBrush(Globals.TextDisabledColor), 
						rect, 
						sf);
				}
			}
		}

		/// <summary>
		/// ���Ʋ˵���ͼƬ�ķ���
		/// </summary>
		private static void DrawPictureArea(System.Windows.Forms.DrawItemEventArgs e, MenuItem mi)
		{
			Rectangle rect = new Rectangle(e.Bounds.X - 1, 
				e.Bounds.Y, 
				Globals.PIC_AREA_SIZE, 
				e.Bounds.Height);

			Brush b = new LinearGradientBrush(rect, 
				Globals.MenuDarkColor2, 
				Globals.MenuLightColor2,
				180f, 
				false);

			e.Graphics.FillRectangle(b, rect);
		}

		/// <summary>
		/// �������ϵ�ͼƬ�ķ���
		/// </summary>
		private static void DrawItemPicture(System.Windows.Forms.DrawItemEventArgs e, MenuItem mi)
		{
			const int MAX_PIC_SIZE = 16;

			Image img = Menus.GetItemPicture(mi);

			if ( img != null ) 
			{
				int width = img.Width > MAX_PIC_SIZE ? MAX_PIC_SIZE : img.Width;
				int height = img.Height > MAX_PIC_SIZE ? MAX_PIC_SIZE : img.Height;
				
				int x = e.Bounds.X + 2;
				int y = e.Bounds.Y + ((e.Bounds.Height - height) / 2);
				
				Rectangle rect = new Rectangle(x, y, width, height);
				
				if ( mi.Enabled ) 
				{
					e.Graphics.DrawImage(img, x, y, width, height);
				} 
				else 
				{
					ColorMatrix myColorMatrix = new ColorMatrix();
					myColorMatrix.Matrix00 = 1.00f; // Red
					myColorMatrix.Matrix11 = 1.00f; // Green
					myColorMatrix.Matrix22 = 1.00f; // Blue
					myColorMatrix.Matrix33 = 1.30f; // alpha
					myColorMatrix.Matrix44 = 1.00f; // w

					ImageAttributes imageAttr = new ImageAttributes();
					imageAttr.SetColorMatrix(myColorMatrix);

					e.Graphics.DrawImage(img,
						rect,
						0, 
						0, 
						width, 
						height, 
						GraphicsUnit.Pixel, 
						imageAttr);
				}
			}
		}

		/// <summary>
		/// ����ѡ�к�ľ��η���
		/// </summary>
		private static void DrawSelectionRect(System.Windows.Forms.DrawItemEventArgs e, MenuItem mi)
		{
			if ( mi.Enabled ) 
			{
				e.Graphics.FillRectangle(new SolidBrush(Globals.SelectionColor), 
					e.Bounds);

				e.Graphics.DrawRectangle(new Pen(Globals.MenuDarkColor), 
					e.Bounds.X, 
					e.Bounds.Y, 
					e.Bounds.Width - 1, 
					e.Bounds.Height - 1);
			}
		}

		/// <summary>
		/// ���ƿؼ���ѡ��򷽷�
		/// </summary>
		private static void DrawCheckBox(System.Windows.Forms.DrawItemEventArgs e, MenuItem mi)
		{
			int cbSize = Globals.PIC_AREA_SIZE - 5;

			e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

			Rectangle rect = new Rectangle(e.Bounds.X + 1, 
				e.Bounds.Y + ((e.Bounds.Height - cbSize) / 2), 
				cbSize, 
				cbSize);

			Pen pen = new Pen(Color.Black,1.7f);

			if ( (e.State & DrawItemState.Selected) == DrawItemState.Selected )
				e.Graphics.FillRectangle(new SolidBrush(Globals.DarkCheckBoxColor), rect);
			else
				e.Graphics.FillRectangle(new SolidBrush(Globals.CheckBoxColor), rect);

			e.Graphics.DrawRectangle(new Pen(Globals.MenuDarkColor), rect);
			
			Bitmap img = Menus.GetItemPicture(mi);

			if ( img == null ) 
			{
				e.Graphics.DrawLine(pen, e.Bounds.X + 7, 
					e.Bounds.Y + 10, 
					e.Bounds.X + 10, 
					e.Bounds.Y + 14);

				e.Graphics.DrawLine(pen, 
					e.Bounds.X + 10, 
					e.Bounds.Y + 14, 
					e.Bounds.X + 15, 
					e.Bounds.Y + 9);
			}
		}
	}

	/// <summary>
	/// ȫ�ֱ���
	/// </summary>
	public class Globals
	{
		public static int PIC_AREA_SIZE = 24;
		public static int MIN_MENU_HEIGHT = 22;
		public static Font menuFont = System.Windows.Forms.SystemInformation.MenuFont; 
		public static Color CheckBoxColor = Color.FromArgb(255, 192, 111);
		public static Color DarkCheckBoxColor = Color.FromArgb(254, 128, 62);
		public static Color SelectionColor = Color.FromArgb(255,238,140);
		public static Color TextColor = Color.FromKnownColor(KnownColor.MenuText);
		public static Color TextDisabledColor = Color.FromKnownColor(KnownColor.GrayText);
		public static Color MenuBgColor = Color.White;
		public static Color MainColor = Color.FromKnownColor(KnownColor.Control);
		public static Color MenuDarkColor = Color.FromKnownColor(KnownColor.ActiveCaption);
		public static Color MenuDarkColor2 = Color.FromArgb(110, Color.FromKnownColor(KnownColor.ActiveCaption));
		public static Color MenuLightColor = Color.FromKnownColor(KnownColor.InactiveCaption);
		public static Color MenuLightColor2 = Color.FromArgb(50, Color.FromKnownColor(KnownColor.InactiveCaption));
	}
}
