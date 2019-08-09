using System;
using System.Drawing;
namespace GDIDrawFlow
{
	/// <summary>
	/// MakeMiniature ��ժҪ˵����
	/// </summary>
	public class MakeMiniature
	{
		public MakeMiniature()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
		}

		/**//// <summary>
		/// ��������ͼ
		/// </summary>
		/// <param name="originalImage">Դͼ</param>
		/// <param name="thumbnailPath">����ͼ·��������·����</param>
		/// <param name="width">����ͼ���</param>
		/// <param name="height">����ͼ�߶�</param>
		/// <param name="mode">��������ͼ�ķ�ʽ</param>    
		public static void MakeThumbnail(Image image, string thumbnailPath, int width, int height, string mode)
		{
			Image originalImage=image;
			int towidth = width;
			int toheight = height;
        
			int x = 0;
			int y = 0;
			int ow = originalImage.Width;
			int oh = originalImage.Height;        

			switch (mode)
			{        
				case "HW"://ָ���߿����ţ����ܱ��Σ�                
					break;
				case "W"://ָ�����߰�����                    
					toheight = originalImage.Height * width/originalImage.Width;
					break;
				case "H"://ָ���ߣ�������
					towidth = originalImage.Width * height/originalImage.Height;                    
					break;        
				case "Cut"://ָ���߿�ü��������Σ�                
					if((double)originalImage.Width/(double)originalImage.Height > (double)towidth/(double)toheight)
					{
						oh = originalImage.Height;
						ow = originalImage.Height*towidth/toheight;
						y = 0;
						x = (originalImage.Width - ow)/2;
					}
					else
					{
						ow = originalImage.Width;
						oh = originalImage.Width*height/towidth;
						x = 0;
						y = (originalImage.Height - oh)/2;
					}
					break;                    
				default :
					break;
			}    
            
			//�½�һ��bmpͼƬ
			Image bitmap = new System.Drawing.Bitmap(towidth,toheight);

			//�½�һ������
			Graphics g = System.Drawing.Graphics.FromImage(bitmap);

			//���ø�������ֵ��
			g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

			//���ø�����,���ٶȳ���ƽ���̶�
			g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

			//��ջ�������͸������ɫ���
			g.Clear(Color.Transparent);        

			//��ָ��λ�ò��Ұ�ָ����С����ԭͼƬ��ָ������
			g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight), 
				new Rectangle(x, y, ow,oh),
				GraphicsUnit.Pixel);

			try
			{            
				//��jpg��ʽ��������ͼ
				bitmap.Save(thumbnailPath, System.Drawing.Imaging.ImageFormat.Jpeg);
			}
			catch(System.Exception e)
			{
				throw e;
			}
			finally
			{
				//originalImage.Dispose();
				bitmap.Dispose();                        
				g.Dispose();
			}
		}

		/**//// <summary>
		/// ��������ͼ
		/// </summary>
		/// <param name="originalImage">Դͼ</param>
		/// <param name="width">����ͼ���</param>
		/// <param name="height">����ͼ�߶�</param>
		/// <param name="mode">��������ͼ�ķ�ʽ</param>    
		public static Image MakeThumbnail(Image image, int width, int height, string mode)
		{
			Image originalImage=image;
			int towidth = width;
			int toheight = height;
        
			int x = 0;
			int y = 0;
			int ow = originalImage.Width;
			int oh = originalImage.Height;        

			switch (mode)
			{        
				case "HW"://ָ���߿����ţ����ܱ��Σ�                
					break;
				case "W"://ָ�����߰�����                    
					toheight = originalImage.Height * width/originalImage.Width;
					break;
				case "H"://ָ���ߣ�������
					towidth = originalImage.Width * height/originalImage.Height;                    
					break;        
				case "Cut"://ָ���߿�ü��������Σ�                
					if((double)originalImage.Width/(double)originalImage.Height > (double)towidth/(double)toheight)
					{
						oh = originalImage.Height;
						ow = originalImage.Height*towidth/toheight;
						y = 0;
						x = (originalImage.Width - ow)/2;
					}
					else
					{
						ow = originalImage.Width;
						oh = originalImage.Width*height/towidth;
						x = 0;
						y = (originalImage.Height - oh)/2;
					}
					break;                    
				default :
					break;
			}    
            
			//�½�һ��bmpͼƬ
			Image bitmap = new System.Drawing.Bitmap(towidth,toheight);

			//�½�һ������
			Graphics g = System.Drawing.Graphics.FromImage(bitmap);

			//���ø�������ֵ��
			g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;

			//���ø�����,���ٶȳ���ƽ���̶�
			g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

			//��ջ�������͸������ɫ���
			g.Clear(Color.Transparent);        

			//��ָ��λ�ò��Ұ�ָ����С����ԭͼƬ��ָ������
			g.DrawImage(originalImage, new Rectangle(0, 0, towidth, toheight), 
				new Rectangle(x, y, ow,oh),
				GraphicsUnit.Pixel);

			try
			{            
				//��jpg��ʽ��������ͼ
				return bitmap;
			}
			catch(System.Exception e)
			{
				throw e;
			}
			finally
			{
				//originalImage.Dispose();
				//bitmap.Dispose();                        
				g.Dispose();
			}
		}

	}
}
