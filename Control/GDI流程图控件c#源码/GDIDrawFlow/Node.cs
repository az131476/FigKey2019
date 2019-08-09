using System;
using System.Drawing;
using System.Collections;

namespace GDIDrawFlow
{
	/// <summary>
	/// Node ��ժҪ˵����
	/// </summary>
	/// 

	public class Node
	{
		private int x,y;//�ڵ�����
		private int width,height;//�ڵ����
		private int iNodeListIndex;//�ڵ��������е�ID
		private DrawObjectType drawObjectType;//�ڵ�����
		private string nodeText;//�ڵ�����
		private int iTextSize=10;//�ڵ��������ִ�С
		private Font nodeTextFont;//�ڵ�������������
		private Color nodeTextColor;//�ڵ�����������ɫ
		private Color cBorderColor;//����  �� ��Բ�� �߿���ɫ
		private Color cFillColor;//����  �� ��Բ�� �����ɫ
		private int iConnectInCount=0;//���������
		private int iConnectOutCount=0;//����������
		private DateTime dt_inFlowTime;//����ʱ��
		private DateTime dt_outFlowTime;//����ʱ��
		private string str_Function="δ����";//����˵��
		private string str_OperationRole="δ����";//ҵ���ɫ
		private string str_FunctionInfo="δ����";//����˵��
		private string str_Info="δ����";//��ϸ��Ϣ
		private ArrayList arr_InFlowNodeID;//����ڵ��ID
		private ArrayList arr_OutFlowNodeID;//�����ڵ��ID
			
		/// <param name="index">����</param>
		/// <param name="drawObjectType">�ڵ�����</param>
		/// <param name="nodeText">�ڵ��ı�</param>
		public Node(int index,DrawObjectType drawObjectType,string nodeText)
		{
			this.iNodeListIndex=index;
			this.drawObjectType=drawObjectType;
			this.nodeText=nodeText;
			nodeTextFont=new Font("����",iTextSize);
			nodeTextColor=Color.Black;
			cBorderColor=Color.Black;
			cFillColor=Color.Transparent;
			this.dt_inFlowTime=DateTime.Now;
			this.dt_outFlowTime=DateTime.Now;
			this.arr_InFlowNodeID=new ArrayList();
			this.arr_OutFlowNodeID=new ArrayList();
		}

		/// <summary>
		/// �ڵ�����
		/// </summary>
		public  enum DrawObjectType
		{
			/// <summary>
			/// ��ʼͼԪ
			/// </summary>
			DrawNodeBegin,
			/// <summary>
			/// �ڵ�ͼԪ
			/// </summary>
			DrawNodeGeneral,
			/// <summary>
			/// �ض�����ͼԪ
			/// </summary>
			DrawSpecificallyOperation,
			/// <summary>
			/// ˳��ͼԪ
			/// </summary>
			DrawGradation,
			/// <summary>
			/// ͬ��ͼԪ
			/// </summary>
			DrawSynchronization,
			/// <summary>
			/// ��֧ͼԪ
			/// </summary>
			DrawAsunder,
			/// <summary>
			/// ���ͼԪ
			/// </summary>
			DrawConverge,
			/// <summary>
			/// ��������ͼԪ
			/// </summary>
			DrawGather,
			/// <summary>
			/// �ж�ͼԪ
			/// </summary>
			DrawJudgement,
			/// <summary>
			/// ����ͼԪ
			/// </summary>
			DrawDataNode,
			/// <summary>
			/// ����ͼԪ
			/// </summary>
			DrawNodeEnd,
			/// <summary>
			/// ���ƾ���
			/// </summary>
			DrawRectangle,
			/// <summary>
			/// ������Բ
			/// </summary>
			DrawEllipse
		}
		/// <summary>
		///����ȡ�����ýڵ�����
		/// </summary>
		public string NodeText
		{
			get
			{
				return this.nodeText;
			}
			set
			{
				this.nodeText=value;
			}
		}
		/// <summary>
		/// ��ȡ�����ýڵ�����
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
		/// ��ȡ�����ýڵ�����
		/// </summary>
		public Font NodeTextFont
		{
			get
			{
				nodeTextFont=new Font("����",iTextSize);
				return this.nodeTextFont;
			}
			set
			{
				this.nodeTextFont=value;
			}
		}
		/// <summary>
		/// ��ȡ�����ýڵ�������ɫ
		/// </summary>
		public Color NodeTextColor
		{
			get
			{
				return this.nodeTextColor;
			}
			set
			{
				this.nodeTextColor=value;
			}
		}

		/// <summary>
		/// ��ȡ�����ýڵ�߿���ɫ  ������  �� ��Բ��
		/// </summary>
		public Color BorderColor
		{
			get
			{
				return this.cBorderColor;
			}
			set
			{
				this.cBorderColor=value;
			}
		}

		/// <summary>
		/// ��ȡ�����ýڵ������ɫ  ������  �� ��Բ��
		/// </summary>
		public Color FillColor
		{
			get
			{
				return this.cFillColor;
			}
			set
			{
				this.cFillColor=value;
			}
		}

		#region �ڵ�λ��  ��С��Ϣ
		/// <summary>
		/// �ڵ��X����
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
		/// �ڵ��Y���� 
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
		/// �ڵ�Ŀ��
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
		/// �ڵ�ĸ߶�
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
		/// <summary>
		/// �ڵ�洢�������е�ID
		/// </summary>
		public int NodeListIndex
		{
			get 
			{
				return this.iNodeListIndex;
			}
			set
			{
				this.iNodeListIndex=value;
			}
		}

		/// <summary>
		/// ����ڵ���߸���
		/// </summary>
		public int ConnectInCount
		{
			get
			{
				return this.iConnectInCount;
			}
			set
			{
				this.iConnectInCount=value;
			}
		}

		/// <summary>
		/// �����ڵ��ߵĸ���
		/// </summary>
		public int ConnectOutCount
		{
			get
			{
				return this.iConnectOutCount;
			}
			set
			{
				this.iConnectOutCount=value;
			}
		}

		/// <summary>
		/// �ڵ�����
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
		/// �ڵ�����ʱ��
		/// </summary>
		public DateTime InFolwTime
		{
			get
			{
				return this.dt_inFlowTime;
			}
			set
			{
				this.dt_inFlowTime=value;
			}
		}

		/// <summary>
		/// �ڵ�����ʱ��
		/// </summary>
		public DateTime OutFlowTime
		{
			get
			{
				return this.dt_outFlowTime;
			}
			set
			{
				this.dt_outFlowTime=value;
			}
		}

		/// <summary>
		/// ����˵��
		/// </summary>
		public string Function
		{
			get
			{
				return this.str_Function;
			}
			set
			{
				this.str_Function=value;
			}
		}

		/// <summary>
		/// ҵ���ɫ
		/// </summary>
		public string OperationRole
		{
			get
			{
				return this.str_OperationRole;
			}
			set
			{
				this.str_OperationRole=value;
			}
		}

		/// <summary>
		/// ����˵��
		/// </summary>
		public string FunctionInfo
		{
			get
			{
				return this.str_FunctionInfo;
			}
			set
			{
				this.str_FunctionInfo=value;
			}
		}
		
		/// <summary>
		/// ��ϸ˵��
		/// </summary>
		public string Info
		{
			get
			{
				return this.str_Info;
			}
			set
			{
				this.str_Info=value;
			}
		}
		/// <summary>
		///  ��ȡ���趨����ڵ��ID
		/// </summary>
		public ArrayList InFlowNodeID
		{
			get
			{
				return this.arr_InFlowNodeID;
			}
			set
			{
				this.arr_InFlowNodeID=value;
			}
		}
		/// <summary>
		/// ��ȡ���趨�����ڵ��ID
		/// </summary>
		public ArrayList OutFlowNodeID
		{
			get
			{
				return this.arr_OutFlowNodeID;
			}
			set
			{
				this.arr_OutFlowNodeID=value;
			}
		}
	}
}
