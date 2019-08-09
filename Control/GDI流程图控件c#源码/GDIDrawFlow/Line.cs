using System;
using System.Drawing;

namespace GDIDrawFlow
{
	/// <summary>
	/// Line ��ժҪ˵����
	/// </summary>
	public class Line
	{
		private int[] iLineNode;//����洢
		private int iLineListIndex;//���������е�ID
		private int iLastCount=0;//������洢�����е����һ��
		private Node firstNode,secondNode;//�߿�ʼ �ͽ���  �˵� ���ӵĽڵ�
		private int iFirNodeInterfaceIndex,iSecNodeInterfaceIndex;//���ӵ��ڵ�����ӿ�
		private Color lineColor;//�ߺ�����˵������ɫ
		private int iLineSize=1;//�ߵĴ�С
		private DrawObjectType drawObjectType;//�ߵ�����
		private string strContent="����";//����˵��
		private Font lineTextFont;//����˵��������
		private int iTextSize=10;//����˵���Ĵ�С
		private bool b_FoldModality;//����ת�����ʽ����-|_   false   |-|  true

		public Line(DrawObjectType drawObjectType,int index)
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//
			this.iLineListIndex=index;
			this.iLineNode=new int[10];
			lineColor=Color.Black;
			this.drawObjectType=drawObjectType;
			lineTextFont=new Font("����",iTextSize);
		}

		/// <summary>
		/// �ߵ�����
		/// </summary>
		public  enum DrawObjectType
		{
			/// <summary>
			/// ֱ��
			/// </summary>
			DrawBeeLine,
			/// <summary>
			///  ����
			/// </summary>
			DrawFoldLine
		}
		/// <summary>
		/// ��ȡ�������ߵ�����
		/// </summary>
		public DrawObjectType ObjectType
		{
			get
			{
				return drawObjectType;
			}
			set
			{
				this.drawObjectType=value;
			}
		}

		/// <summary>
		/// ��ȡ�ߵ�������Ϣ
		/// </summary>
		/// <param name="index">�˵� ��ʶ</param>
		/// <returns></returns>
		public int GetLineNodeInfo(int index)
		{
			return this.iLineNode[index];
		}
		
		/// <summary>
		/// ���������еĴ洢ID
		/// </summary>
		public int LineListIndex
		{
			get
			{
				return this.iLineListIndex;
			}
			set
			{
				this.iLineListIndex=value;
			}
		}

		/// <summary>
		/// ����� ����
		/// </summary>
		/// <param name="x">X ����</param>
		/// <param name="y">Y ����</param>
		public void addPoint(int x,int y)
		{
			this.iLineNode[this.iLastCount]=x;
			this.iLineNode[this.iLastCount+1]=y;
		}

		/// <summary>
		/// �޸��ߵ����� 
		/// </summary>
		/// <param name="pointIndex"></param>
		/// <param name="x">X ����</param>
		/// <param name="y">Y ����</param>
		public void setPoint(int pointIndex,int x,int y)
		{
			this.iLineNode[pointIndex*2]=x;
			this.iLineNode[pointIndex*2+1]=y;
		}

		/// <summary>
		/// ֱ�߻����ߵı�ʾ   4 ֱ��  8 ����
		/// </summary>
		public int LineNodeCount
		{
			get
			{
				return this.iLastCount;
			}
			set
			{
				this.iLastCount=value;
			}
		}
	
		/// <summary>
		/// ��ʼ�˵�����ӽڵ�
		/// </summary>
		public Node FirstNode
		{
			get
			{
				return this.firstNode;
			}
			set
			{
				this.firstNode=value;
			}
		}

		/// <summary>
		/// �����˵�����ӽڵ�
		/// </summary>
		public Node SecondNode
		{
			get
			{
				return this.secondNode;
			}
			set
			{
				this.secondNode=value;
			}
		}

		/// <summary>
		/// ��ʼ�˵�  ���ӵĽڵ�����ӿ�
		/// </summary>
		public int FirNodeInterfaceIndex
		{
			get
			{
				return this.iFirNodeInterfaceIndex;
			}
			set
			{
				this.iFirNodeInterfaceIndex=value;
			}
		}

		/// <summary>
		/// �����˵�  ���ӵĽڵ�����ӿ�
		/// </summary>
		public int SecNodeInterfaceIndex
		{
			get
			{
				return this.iSecNodeInterfaceIndex;
			}
			set
			{
				this.iSecNodeInterfaceIndex=value;
			}
		}

		#region �ߵ�����
		public int X0
		{
			get
			{
				return this.iLineNode[0];
			}
			set
			{
				this.iLineNode[0]=value;
			}
		}

		public int Y0
		{
			get
			{
				return this.iLineNode[1];
			}
			set
			{
				this.iLineNode[1]=value;
			}
		}

		public int X1
		{
			get
			{
				return this.iLineNode[2];
			}
			set
			{
				this.iLineNode[2]=value;
			}
		}

		public int Y1
		{
			get
			{
				return this.iLineNode[3];
			}
			set
			{
				this.iLineNode[3]=value;
			}
		}

		public int X2
		{
			get
			{
				return this.iLineNode[4];
			}
			set
			{
				this.iLineNode[4]=value;
			}
		}

		public int Y2
		{
			get
			{
				return this.iLineNode[5];
			}
			set
			{
				this.iLineNode[5]=value;
			}
		}

		public int X3
		{
			get
			{
				return this.iLineNode[6];
			}
			set
			{
				this.iLineNode[6]=value;
			}
		}

		public int Y3
		{
			get
			{
				return this.iLineNode[7];
			}
			set
			{
				this.iLineNode[7]=value;
			}
		}
		#endregion
		/// <summary>
		/// �߼��ߵĵ�����˵������ɫ
		/// </summary>
		public Color LineColor
		{
			get
			{
				return this.lineColor;
			}
			set
			{
				this.lineColor=value;
			}
		}

		/// <summary>
		/// �ߵĴ�С
		/// </summary>
		public int LineSize
		{
			get
			{
				return this.iLineSize;
			}
			set
			{
				this.iLineSize=value;
			}
		}
		/// <summary>
		///  �ߵ�����˵��������
		/// </summary>
		public  string Content
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
		///  �ߵ�����˵��������
		/// </summary>
		public Font LineTextFont
		{
			get
			{
				this.lineTextFont=new Font("����",iTextSize);
				return this.lineTextFont;
			}
			set
			{
				this.lineTextFont=value;
			}
		}

		/// <summary>
		///  �ߵ�����˵���Ĵ�С
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
		/// ��ȡ���������ߵ���ʽ 
		/// </summary>
		public bool Modality
		{
			get
			{
				return this.b_FoldModality;
			}
			set
			{
				this.b_FoldModality=value;
			}
		}
	}
}
