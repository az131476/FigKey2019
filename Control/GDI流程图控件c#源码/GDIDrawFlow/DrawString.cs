using System;
using System.Drawing;

namespace GDIDrawFlow
{
	/// <summary>
	/// DrawString ��ժҪ˵����
	/// </summary>
	public class DrawString
	{
		private string strContent="��˫���������ݡ�";//����
		private int x,y;// ����
		private int width,height;//����
		private int iDrawStringListIndex;//д�ְ�������Ĵ洢ID
		private int iTextSize=10;//�����С
		private Font dSTextFont;//������Ϣ
		private Color dSTextColor;//������ɫ

		public DrawString(int index)
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
			this.iDrawStringListIndex=index;
			dSTextFont=new Font("����",iTextSize);
			this.dSTextColor=Color.Black;
		}

		/// <summary>
		/// ����
		/// </summary>
		public string Content
		{
			get
			{
				return this.strContent;
			}
			set
			{
				this.strContent=value;
			}
		}

		/// <summary>
		///д�ְ��������е�ID
		///</summary>
		public int DrawStrListIndex
		{
			get
			{
				return this.iDrawStringListIndex;
			}
			set
			{
				this.iDrawStringListIndex=value;
			}
		}


		#region д�ְ�Ĵ�С λ��
		/// <summary>
		/// д�ְ��X����
		/// </summary>
		public int X
		{
			get
			{
				return this.x;
			}
			set
			{
				this.x=value;
			}
		}

		/// <summary>
		/// д�ְ��Y����
		/// </summary>
		public int Y
		{
			get
			{
				return this.y;
			}
			set
			{
				this.y=value;
			}
		}

		/// <summary>
		/// д�ְ�Ŀ��
		/// </summary>
		public int Width
		{
			get
			{
				return this.width;
			}
			set
			{
				this.width=value;
			}
		}
	
		/// <summary>
		/// д�ְ�ĸ߶�
		/// </summary>
		public int Height
		{
			get
			{
				return this.height;
			}
			set
			{
				this.height=value;
			}
		}
		
		#endregion
		#region ������Ϣ
		/// <summary>
		/// �����С
		/// </summary>
		public int TextSize
		{
			get
			{
				return this.iTextSize;
			}
			set
			{
				this.iTextSize=value;
			}
		}

		/// <summary>
		/// ������Ϣ
		/// </summary>
		public Font DSTextFont
		{
			get
			{
				this.DSTextFont=new Font("����",iTextSize);
				return this.dSTextFont;
			}
			set
			{
				this.dSTextFont=value;
			}
		}

		/// <summary>
		/// ������ɫ
		/// </summary>
		public Color DSTextColor
		{
			get
			{
				return this.dSTextColor;
			}
			set
			{
				this.dSTextColor=value;
			}
		}
		#endregion 
	}
}
