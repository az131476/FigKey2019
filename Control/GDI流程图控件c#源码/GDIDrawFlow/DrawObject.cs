using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;


namespace GDIDrawFlow
{
	/// <summary>
	/// DrawLine ��ժҪ˵����
	/// </summary>

	public class DrawObject
	{
		#region ��������
		private DrawFlowControl control;//������
		public  ArrayList arrLineList;//�ߵ�����
		private ArrayList arrNodeList;//�ڵ������
		private ArrayList arrLineConnectNode;//���ӵ���ڵ����
		private ArrayList arrDrawStringList;//д�ְ������
		private Line lineThisLine;//��ǰ���ڱ༭����(���ڶ��ߵĴ���)
		private Node nodeThisNode;//��ǰ���ڱ༭�Ľڵ�(���ڶԽڵ�Ĵ���)
		private int iLineFirstNodeX,iLineFirstNodeY,iLineSecondNodeX,iLineSecondNodeY,iLine3thNodeX,iLine3thNodeY,iLine4thNodeX,iLine4thNodeY;//���ڱ༭���ߵ�������Ϣ
		private Line lineTempLine;//��ʱ��ʾ���ߡ����������仯����ߣ�
		private Line lineSelectLine;//ѡ�е���(�����ߵ��޸�)
		private int iTempNodeIndex;//��ʱ��ʾ���ߵ�ĳһ��
		private int iSelectNodeIndex;//ѡ�е��ߵ�ĳһ��
		private int iTempNodeX=0,iTempNodeY=0;//���ڵ��ʱ��Ľڵ��������
		private bool bDrawLine=false;//�Ƿ�ʼ�ػ��ߣ�paint�¼���
		private int iMouseInitX,iMouseInitY;//����ƶ��ĳ�ʼ���꣨ÿ���ƶ�ˢ�£�
		private int iNodeX,iNodeY,iNodeWidth,iNodeHeight;//��ǰ���ƵĽڵ���Ϣ
		private bool bDrawNode=false;//�Ƿ񻭽ڵ�
		private Node nodeSelectNode;//ѡ�еĽڵ�
		private bool bDrawConnectNodeLine=false;//�Ƿ��ػ����ӵ���ڵ�������ߣ��ƶ��ڵ㣩
		private Color bckColor;//����ɫ
		private Bitmap bitmapMemeory,bitmapBackGroupMap;//���� ������ͼ
		private Graphics graDrawPanel;//����
		private Graphics graDrawLine;//������
		private Pen penDrawLine;//���߱�
		private Pen penDrawNode;//���ڵ��
		private Pen penDrawPoint;//�����
		private Pen penDrawString;//д�ֱ�
		private Pen penDrawBeeLine;//��ֱ�߱�
		private SelectPoint selectPoint;//ѡ�񣨻����ʾ��
		private int iMouseDownX,iMouseDownY;//��¼���İ������꣨���� �ƶ����ӵ��ڵ���ߵ��ƶ������жϣ�
		public Object lastEdit;//��¼�ϴα༭��Ԫ��
		private int iAutoConnectLinePointIndex=9;//Ӧ��Ϊ��ɫ���ߵĻ��ʾ��ʶ
		private Node nodeTempNode=null;//�ڵ����ʱѡ��
		private int iResizeNodeMouseIndex=99;//�༭�ڵ��С��  �༭��ʽ �磺���������ı�
		private int iResizeDSMouseIndex=99;//�༭д�ְ��С��  �༭��ʽ �磺���������ı�
		//private bool bResizeNode=false;//�Ƿ�Ҫ�ı�ڵ�Ĵ�С
		private DrawString drawStringThisDS;//��ǰҪ��д�ְ�
		private DrawString drawStringSelectDS;//ѡ����д�ְ�
		private DrawString drawStringTempDS;//���ڸı�д�ְ�Ĵ�С
		private int iDrawStringX,iDrawStringY,iDrawStringWidth,iDrawStringHeight;//��ǰ��д�ְ������ͳ�����
		private bool bDrawString;//�Ƿ�д�ְ�
		private int iTempDSX=0,iTempDSY=0;//��д�ְ��ʱ������һ�ΰ��µ�����
		private Font fontDrawString;//д�ְ��������Ϣ
		private TextBox TBDrawStringContent;//д�ְ�����ݱ༭
		private TextBox TBnodeContent;//�ڵ�����Ʊ༭
		private TextBox TBLineContent;//�ߵ����Ʊ༭
		private string strDSContent;//��ǰд�ְ������
		private RectangleF re;//д�ְ�д������
		//private bool bResizeDS;//�Ƿ�ı�д�ְ�Ĵ�С
		private GDIDrawFlow.Node.DrawObjectType currentObjectType; //���嵱ǰ�ڵ������
		public Bitmap bgImage;//����ͼƬ
		private Line newLine;//Ҫ������
		private Node newNode;//Ҫ���Ľڵ�
		private DrawString newDrawString;//Ҫ����д�ְ� 
		private Object objPrepareDel;//Ҫɾ����Ԫ��
		private bool bSelectAll=false;//�Ƿ�ѡ����ȫ����Ԫ��
		private bool bSelectAllReday=false;//�Ƿ��Ѿ�ѡ����ȫ����Ԫ��
		private ArrayList arrLineSelectList;//�ߵ�����
		private ArrayList arrNodeSelectList;//�ڵ������
		private ArrayList arrLineNotSelectList;//û�б�ѡ����ߵ�����
		private ArrayList arrNodeNotSelectList;//û�б�ѡ��Ľڵ������
		private ArrayList arrDrawStringSelectList;//д�ְ������
		private ArrayList arrDrawStringNotSelectList;//û�б�ѡ���д�ְ������
		private bool bNotSelectAnyOne=false;//��갴�µ�ʱ��û��ѡ�����κ�һ���ڵ�
		private int iMouseSelectX,iMouseSelectY;//����ѡ�������
		private int iMouseSelectWidth,iMouseSelectHeight;//����ѡ��ĳ��ȺͿ��
		private bool bDrawSelectRectangle=false;//����ѡ�����߿�
		private bool bDrawSelectElement=false;//�Ƿ񻭳�ѡ�񶨵�Ԫ��
		private bool bSelectRectangleReday=false;//�Ƿ��Ѿ�ѡ���������Ԫ��
		private bool bIsAtSelect=false;//�Ƿ�����ѡ������ѡ������򡣡����ڻ�����ѡ���ߣ�
		private Size size;//��Ļ�ߴ�
		public  bool bStartFlag=false;//�Ƿ��Ѿ����˿�ʼ�ڵ�
		public  bool bEndFlag=false;//�Ƿ��Ѿ����˽����ڵ�
		private ArrayData arrayData;//���л�����
		private GetTypeImage getTypeImage;//��ȡ�ڵ�ͼƬ
		private Image image;// Ҫ���Ƶ�ͼƬ  �ڵ��ϵ�ʹ��
		private int iOnLineSegment=9;//��������߶��ϵı�ʶ
		private GetTypeCursor getTypeCursor;//�Զ������ͼ��
		public  Point location=new Point(100,100);
		private bool b_IsDownMouseRight=false;//�Ƿ�������Ҽ�
		public  int i_GeneralCount=0;//��ͨ�ڵ�����֡�
		public  object obj_SeriesDrawEle;//��������ͼԪ
		public  int[] iArr_ShowHelpTip;//�洢�ڵ��������Ϣ��ֻһ�Σ�
		public  Node node_OperationTemp;//��ʱ�༭�Ľڵ�
		public  Line line_OperationTemp;//��ʱ�༭����
		public  DrawString ds_OperationTemp;//��ʱ�༭��д�ְ�
		public  bool b_PressDirctKey;//�Ƿ��·����
		DrawFlowTable dft;
		public bool isAlpha=true; //ͼԪ�Ƿ�֧�ְ�͸��
		#endregion

		#region ���췽��
		public DrawObject(DrawFlowControl control)
		{
			this.control=control;
			init();
		}
	
		#endregion

		#region ��ʼ��
		private void init()
		{//��ȡ�����

			this.SerializeInit();
			this.arrLineList=this.arrayData.arrLineList;
			this.arrNodeList=this.arrayData.arrNodeList;
			this.arrLineConnectNode=this.arrayData.arrLineConnectNode;
			this.arrDrawStringList=this.arrayData.arrDrawStringList;
			this.arrLineSelectList=this.arrayData.arrLineSelectList;
			this.arrNodeSelectList=this.arrayData.arrNodeSelectList;
			this.arrDrawStringSelectList=this.arrayData.arrDrawStringSelectList;
			this.arrLineNotSelectList=this.arrayData.arrLineNotSelectList;
			this.arrNodeNotSelectList=this.arrayData.arrNodeNotSelectList;
			this.arrDrawStringNotSelectList=this.arrayData.arrDrawStringNotSelectList;

			this.bitmapBackGroupMap=this.control.bitmapBackGroupMap;
			this.bitmapMemeory=this.control.bitmapMemeory;
			this.graDrawLine=this.control.graDrawLine;
			this.graDrawPanel=this.control.graDrawPanel;
			this.penDrawLine=this.control.penDrawLine;
			this.penDrawNode=this.control.penDrawNode;
			this.penDrawPoint=this.control.penDrawPoint;
			this.penDrawString=this.control.penDrawString;
			this.penDrawBeeLine=this.control.penDrawBeeLine;
			this.bckColor = this.control.BackColor;
			this.fontDrawString=this.control.fontDrawString;
			this.TBDrawStringContent=this.control.drawStringContent;
			this.TBnodeContent=this.control.TBnodeContent;
			this.TBLineContent=this.control.TBLineContent;
			this.bgImage=this.control.bgImage;
			this.size=this.control.Size;
			this.getTypeCursor=new GetTypeCursor();
			this.iArr_ShowHelpTip=new int[20];
			//			this.iLineFirstNodeX=150;
			//			this.iLineFirstNodeY=100;			this.iLineSecondNodeX=200;			this.iLineSecondNodeY=100;			this.iLine3thNodeX=200;			this.iLine3thNodeY=200;			this.iLine4thNodeX=250;			this.iLine4thNodeY=200;
			this.iNodeWidth=50;
			this.iNodeHeight=55;
			this.iDrawStringWidth=50;
			this.iDrawStringHeight=55;
			this.AttriShow();
			dft=new DrawFlowTable (this);
			DrawBackGround();
			this.RefreshBackground();
		}

		private void SerializeInit()
		{
			this.arrayData=new ArrayData();
			//			Stream stream = File.Open("/arrayData.xml", FileMode.Create);
			//����		BinaryFormatter formatter = new BinaryFormatter();
			//			formatter.Serialize(stream, this.arrayData);
			//����		stream.Close();
			//			this.arrayData=null;
			//����		stream = File.Open("/arrayData.xml", FileMode.Open);
			//����		formatter = new BinaryFormatter();
			//			this.arrayData=((ArrayData)formatter.Deserialize(stream));
			//			stream.Close();
			/****************************
			MessageForm.Show("123");
			byte[] byt=new byte[1024];
			MemoryStream stream=new MemoryStream (byt);
			BinaryFormatter formatter = new BinaryFormatter();
			MessageForm.Show("123456");
			formatter.Serialize(stream,this.arrayData);
����		MessageForm.Show("123789");
			this.arrayData=null;
����		//stream = File.Open("arrayData.xml", FileMode.Open);
����		formatter = new BinaryFormatter();
			
			MemoryStream ms=new MemoryStream (byt);
			MessageForm.Show(byt.Length.ToString());
			this.arrayData=((ArrayData)formatter.Deserialize(ms));
			
			MessageForm.Show("abxcsdjfhasdkjfh");

			***************************/
			this.selectPoint=new SelectPoint();
			//			stream = File.Open("/selectPoint.xml", FileMode.Create);
			//����		formatter = new BinaryFormatter();
			//			formatter.Serialize(stream, this.selectPoint);
			//����		stream.Close();
			//			this.selectPoint=null;
			//����		stream = File.Open("/selectPoint.xml", FileMode.Open);
			//����		formatter = new BinaryFormatter();
			//			this.selectPoint=((SelectPoint)formatter.Deserialize(stream));
			//			stream.Close();
			//			
			this.getTypeImage=new GetTypeImage();
			//			stream = File.Open("/getTypeImage.xml", FileMode.Create);
			//����		formatter = new BinaryFormatter();
			//			formatter.Serialize(stream, this.getTypeImage);
			//����		stream.Close();
			//			this.getTypeImage=null;
			//����		stream = File.Open("/getTypeImage.xml", FileMode.Open);
			//����		formatter = new BinaryFormatter();
			//			this.getTypeImage=((GetTypeImage)formatter.Deserialize(stream));
			//			stream.Close();
			
		}
		#endregion

		#region ���󴴽�		public void NewLine(Line.DrawObjectType type)		{			if(this.lineThisLine!=null)			{				if(this.lineThisLine.ObjectType==type)				{					return;				}			}			if(!this.bIsAtSelect)			{				this.bSelectRectangleReday=false;				this.newLine=new Line(type,arrLineList.Count);				this.lineThisLine=this.newLine;				if(type==Line.DrawObjectType.DrawBeeLine)				{					this.control.Cursor=this.getTypeCursor.GetCursor(GetTypeCursor.DrawType.BeeLine);				}
				else				{					this.control.Cursor=this.getTypeCursor.GetCursor(GetTypeCursor.DrawType.FoldLine);				}				this.nodeThisNode=null;				this.drawStringThisDS=null;				if(this.obj_SeriesDrawEle!=null)				{					this.obj_SeriesDrawEle=this.lineThisLine;				}			}		}		public void NewDefaultLine(Line.DrawObjectType type,Point location)		{			if(!this.bIsAtSelect)			{				this.bSelectRectangleReday=false;				this.newLine=new Line(type,arrLineList.Count);				if(type==Line.DrawObjectType.DrawBeeLine)				{					this.newLine.X0=location.X;					this.newLine.Y0=location.Y;					this.newLine.X1=location.X+150;					this.newLine.Y1=location.Y+100;					this.newLine.LineNodeCount=4;					this.lastEdit=this.newLine;					this.selectPoint.SetLinePoint(this.newLine.X0,this.newLine.Y0,this.newLine.X1,this.newLine.Y1);				}
				else if(type==Line.DrawObjectType.DrawFoldLine)				{					this.newLine.X0=location.X;					this.newLine.Y0=location.Y;					this.newLine.X1=location.X+150;					this.newLine.Y1=location.Y;					this.newLine.X2=location.X+150;					this.newLine.Y2=location.Y+100;					this.newLine.X3=location.X+300;					this.newLine.Y3=location.Y+100;;					this.newLine.LineNodeCount=8;					this.lastEdit=this.newLine;					this.selectPoint.SetFlodLinePoint(this.newLine);				}				this.arrLineList.Add(this.newLine);				this.reDrawBitmap(this.graDrawPanel,10,10);				this.RefreshBackground();				this.lineThisLine=null;				this.control.Cursor=Cursors.Default;				//				this.control.Invalidate();			}		}		public void NewNode(Node.DrawObjectType type)		{			if(this.nodeThisNode!=null)			{				if(this.nodeThisNode.ObjectType==type)				{					return;				}			}			if(!this.bIsAtSelect)			{				this.bSelectRectangleReday=false;				this.newNode=new Node(this.arrNodeList.Count,type,getDefaultText(type));				this.nodeThisNode=this.newNode;				this.SetNodeCursor(type);				this.lineThisLine=null;					this.drawStringThisDS=null;				this.currentObjectType=type;				if(this.obj_SeriesDrawEle!=null)				{					this.obj_SeriesDrawEle=this.nodeThisNode;					}							}		}		public void NewDefaultNode(Node.DrawObjectType type,Point location)		{			if(type==Node.DrawObjectType.DrawNodeBegin && this.bStartFlag)			{				MessageForm.Show("�Բ��𣬿�ʼ�ڵ㴴�������ơ�","DrawFlow");				return;			}			if(type==Node.DrawObjectType.DrawNodeEnd && this.bEndFlag)			{				MessageForm.Show("�Բ��𣬽����ڵ㴴�������ơ�","DrawFlow");				return;			}						if(!this.bIsAtSelect)			{				this.bSelectRectangleReday=false;				if(this.i_GeneralCount>=20 && type==Node.DrawObjectType.DrawNodeGeneral)				{					MessageForm.Show("�Բ�����Ŀǰѡ��Ľڵ�����Ϊ20��.","DrawFlow");					return;				}				this.newNode=new Node(this.arrNodeList.Count,type,getDefaultText(type));				this.newNode.X=location.X;				this.newNode.Y=location.Y;				this.newNode.Width=50;				this.newNode.Height=55;				this.lastEdit=this.newNode;				this.selectPoint.SetRectanglePoint(this.newNode.X,this.newNode.Y,this.newNode.Width,this.newNode.Height);				this.arrNodeList.Add(this.newNode);				this.currentObjectType=type;				this.reDrawBitmap(this.graDrawPanel,10,10);				this.RefreshBackground();				this.nodeThisNode=null;				this.control.Cursor=Cursors.Default;				if(type==Node.DrawObjectType.DrawNodeBegin)				{					this.bStartFlag=true;				}
				else if(type==Node.DrawObjectType.DrawNodeEnd)				{					this.bEndFlag=true;				}
				else if(type==Node.DrawObjectType.DrawNodeGeneral)				{					this.i_GeneralCount++;				}				//				this.control.Invalidate();							}			//			else			//			{			//				if(this.drawStringThisDS!=null || this.nodeThisNode!=null || this.lineThisLine!=null)			//				{			//					MessageForm.Show("�Բ������Ѿ�ѡ�����Զ��廭ͼ��","DrawFlow");			//				}			//			}		}		public void NewDrawString()		{			if(this.drawStringThisDS==null && !this.bIsAtSelect)			{				this.bSelectRectangleReday=false;				this.newDrawString=new DrawString(this.arrDrawStringList.Count);				this.drawStringThisDS=this.newDrawString;				this.control.Cursor=this.getTypeCursor.GetCursor(GetTypeCursor.DrawType.String);				this.strDSContent=this.drawStringThisDS.Content;				this.lineThisLine=null;				this.nodeThisNode=null;				if(this.obj_SeriesDrawEle!=null)				{					this.obj_SeriesDrawEle=this.drawStringThisDS;				}			}		}		public void NewDefaultDrawString(Point location)		{			if(!this.bIsAtSelect)			{				this.bSelectRectangleReday=false;				this.newDrawString=new DrawString(this.arrDrawStringList.Count);				this.newDrawString.X=location.X;				this.newDrawString.Y=location.Y;				this.newDrawString.Width=80;				this.newDrawString.Height=60;				this.strDSContent=this.newDrawString.Content;				this.lastEdit=this.newDrawString;				this.selectPoint.SetRectanglePoint(this.newDrawString.X,this.newDrawString.Y,this.newDrawString.Width,this.newDrawString.Height);				this.arrDrawStringList.Add(this.newDrawString);				this.reDrawBitmap(this.graDrawPanel,10,10);				this.RefreshBackground();				this.drawStringThisDS=null;				this.control.Cursor=Cursors.Default;				//				this.control.Invalidate();				}						//			else			//			{			//				if(this.drawStringThisDS!=null || this.nodeThisNode!=null || this.lineThisLine!=null)			//				{			//					MessageForm.Show("�Բ������Ѿ�ѡ�����Զ��廭ͼ��","DrawFlow");			//				}			//			}		}		public void SetNodeCursor(Node.DrawObjectType type)		{			if(type==Node.DrawObjectType.DrawNodeBegin)			{				this.control.Cursor=this.getTypeCursor.GetCursor(GetTypeCursor.DrawType.NodeBegin);			}			else if(type==Node.DrawObjectType.DrawNodeGeneral)			{				this.control.Cursor=this.getTypeCursor.GetCursor(GetTypeCursor.DrawType.NodeGeneral);			}			else if(type==Node.DrawObjectType.DrawSpecificallyOperation)			{				this.control.Cursor=this.getTypeCursor.GetCursor(GetTypeCursor.DrawType.SpecificallyOperation);			}			else if(type==Node.DrawObjectType.DrawGradation)			{				this.control.Cursor=this.getTypeCursor.GetCursor(GetTypeCursor.DrawType.Gradation);			}			else if(type==Node.DrawObjectType.DrawSynchronization)			{				this.control.Cursor=this.getTypeCursor.GetCursor(GetTypeCursor.DrawType.Synchronization);			}			else if(type==Node.DrawObjectType.DrawAsunder)			{				this.control.Cursor=this.getTypeCursor.GetCursor(GetTypeCursor.DrawType.Asunder);			}			else if(type==Node.DrawObjectType.DrawConverge)			{				this.control.Cursor=this.getTypeCursor.GetCursor(GetTypeCursor.DrawType.Converge);			}			else if(type==Node.DrawObjectType.DrawGather)			{				this.control.Cursor=this.getTypeCursor.GetCursor(GetTypeCursor.DrawType.Gather);			}			else if(type==Node.DrawObjectType.DrawJudgement)			{				this.control.Cursor=this.getTypeCursor.GetCursor(GetTypeCursor.DrawType.Judgement);			}			else if(type==Node.DrawObjectType.DrawDataNode)			{				this.control.Cursor=this.getTypeCursor.GetCursor(GetTypeCursor.DrawType.DataNode);			}			else if(type==Node.DrawObjectType.DrawNodeEnd)			{				this.control.Cursor=this.getTypeCursor.GetCursor(GetTypeCursor.DrawType.NodeEnd);			}			else if(type==Node.DrawObjectType.DrawRectangle)			{				this.control.Cursor=this.getTypeCursor.GetCursor(GetTypeCursor.DrawType.Rectangle);			}
			else if(type==Node.DrawObjectType.DrawEllipse)			{				this.control.Cursor=this.getTypeCursor.GetCursor(GetTypeCursor.DrawType.Eillpse);			}			}		#endregion 

		#region ���ƽ���
		public static string getDefaultText(Node.DrawObjectType type)		{			switch(type)			{				case Node.DrawObjectType.DrawNodeBegin:					return "��ʼ";				case Node.DrawObjectType.DrawNodeGeneral:					return "����";				case Node.DrawObjectType.DrawSpecificallyOperation:					return "�ض�����";				case Node.DrawObjectType.DrawGradation:					return "˳��";				case Node.DrawObjectType.DrawSynchronization:					return "ͬ��";				case Node.DrawObjectType.DrawAsunder:					return "��֧";					case Node.DrawObjectType.DrawConverge:					return "���";				case Node.DrawObjectType.DrawGather:					return "��������";				case Node.DrawObjectType.DrawJudgement:					return "�ж�";				case Node.DrawObjectType.DrawDataNode:					return "Ӧ������";				case Node.DrawObjectType.DrawNodeEnd:					return "����";					case Node.DrawObjectType.DrawRectangle:					return "����";				case Node.DrawObjectType.DrawEllipse:					return "��Բ��";			}			return "";		} 

		public string GetLineDefaultText(Line.DrawObjectType type)
		{
			switch(type)
			{
				case Line.DrawObjectType.DrawBeeLine:
					return "ֱ��";
				case Line.DrawObjectType.DrawFoldLine:
					return "����";
			}
			return "";
		}
		/// <summary>
		/// ���Ʊ�������
		/// </summary>
		private void DrawBackGround()
		{
			int width=this.bgImage.Width;
			int height=this.bgImage.Height;
			for(int i=0;i<this.control.Width;i+=width)
			{
				for(int j=0 ;j<this.control.Height;j+=height)
				{
					graDrawPanel.DrawImage(this.bgImage,i,j,width,height);
				}
			}
		}
		/// <summary>
		/// �ػ�����Ԫ�أ�û�б������Ԫ�أ�
		/// </summary>
		/// <param name="g">����</param>
		/// <param name="drawCode">Ԫ�ش����ʾdrawCode=0��ʾ�ڵ㣬drawCode=1 ��ʾֱ�ߣ�drawCode=1 ��ʾд�ְ�</param>
		/// <param name="index">������Ԫ�ص������±�</param>
		public void reDrawBitmap(Graphics g,int drawCode,int index)//drawCode=0��ʾ�ڵ㣬drawCode=1 ��ʾֱ�ߣ�drawCode=1 ��ʾд�ְ�
		{
			DrawBackGround();
			Line line;
			Node node;
			DrawString drawString;
			for(int i=0;i<this.arrLineList.Count;i++)
			{	
				if(drawCode!=1 || i!=index)//�Ƿ���Ƹ���
				{
					line=((Line)arrLineList[i]);
					this.penDrawLine.Color=line.LineColor;
					this.penDrawLine.Width=line.LineSize;
					if(line.ObjectType==Line.DrawObjectType.DrawBeeLine)
					{

						g.DrawLine(this.penDrawLine,line.X0,line.Y0,line.X1,line.Y1);
						this.control.testDrawLength.Text=line.Content;
						int stringleft=(line.X0+line.X1)/2-this.control.testDrawLength.Width/2+3;
						g.DrawString(line.Content,line.LineTextFont,new SolidBrush(line.LineColor),stringleft,(line.Y0+line.Y1)/2);
					}
					else
					{
						this.penDrawBeeLine.Color=line.LineColor;
						this.penDrawBeeLine.Width=line.LineSize;
						g.DrawLine(this.penDrawBeeLine,line.GetLineNodeInfo(0),line.GetLineNodeInfo(1),line.GetLineNodeInfo(2),line.GetLineNodeInfo(3));
						g.DrawLine(this.penDrawBeeLine,line.GetLineNodeInfo(2),line.GetLineNodeInfo(3),line.GetLineNodeInfo(4),line.GetLineNodeInfo(5));
						g.DrawLine(this.penDrawLine,line.GetLineNodeInfo(4),line.GetLineNodeInfo(5),line.GetLineNodeInfo(6),line.GetLineNodeInfo(7));
						this.control.testDrawLength.Text=line.Content;
						int stringleft=(line.X1+line.X2)/2-this.control.testDrawLength.Width/2+3;
						g.DrawString(line.Content,line.LineTextFont,new SolidBrush(line.LineColor),stringleft,(line.Y1+line.Y2)/2);
					}
				}
			}

			for(int i=0;i<this.arrNodeList.Count;i++)
			{	
				if(drawCode!=0 || i!=index)//�Ƿ���Ƹýڵ�
				{
					node=((Node)arrNodeList[i]);
					this.image=this.getTypeImage.GetImage(node.ObjectType);
					if(this.image!=null)
					{
						DrawImage(g,this.image,node.X,node.Y,node.Width,node.Height);

					}
					else
					{
						if(node.ObjectType==Node.DrawObjectType.DrawRectangle)
						{
							this.penDrawNode.Color=node.BorderColor;
							g.DrawRectangle(this.penDrawNode,node.X,node.Y,node.Width,node.Height);
							this.penDrawNode.Color=node.FillColor;
							g.FillRectangle(this.penDrawNode.Brush,node.X,node.Y,node.Width,node.Height);
						}
						else
						{
							this.penDrawNode.Color=node.BorderColor;
							g.DrawEllipse(this.penDrawNode,node.X,node.Y,node.Width,node.Height);
							this.penDrawNode.Color=node.FillColor;
							g.FillEllipse(this.penDrawNode.Brush,node.X,node.Y,node.Width,node.Height);							
						}
					}
					//g.DrawString(node.NodeText,new Font("����",10),new SolidBrush(Color.Black),node.X,node.Y);
					this.control.testDrawLength.Font=node.NodeTextFont;
					this.control.testDrawLength.Text=node.NodeText;
					int stringleft=node.X+node.Width/2-this.control.testDrawLength.Width/2+3;
					g.DrawString(node.NodeText,node.NodeTextFont,new SolidBrush(node.NodeTextColor),stringleft,node.Y+node.Height);
				}
			}
			
			for(int i=0;i<this.arrDrawStringList.Count;i++)
			{
				if(drawCode!=2 || i!=index)//�Ƿ���Ƹ�д�ְ�
				{
					drawString =((DrawString)this.arrDrawStringList[i]);
					int width,height;
					if(drawString.Width<=2)
					{
						width=2;
					}
					else 
					{
						width=drawString.Width;
					}

					if(drawString.Height<=2)
					{
						height=2;
					}
					else
					{
						height=drawString.Height;
					}
					this.re=new RectangleF(drawString.X,drawString.Y,width,height);
					this.penDrawString.Color=drawString.DSTextColor;
					g.DrawString(drawString.Content,drawString.DSTextFont,this.penDrawString.Brush,this.re);
				}
			}
		}		
		/// <summary>
		/// ����û�б�ѡ�е�Ԫ�� ��δ��������Ϊ������
		/// </summary>
		/// <param name="g">����</param>
		public void ReDrawNotSelect(Graphics g)
		{
			DrawBackGround();
			Line line;
			Node node;
			DrawString drawString;
			for(int i=0;i<this.arrLineNotSelectList.Count;i++)
			{	
				line=(Line)this.arrLineNotSelectList[i];
				this.penDrawLine.Color=line.LineColor;
				this.penDrawLine.Width=line.LineSize;
				if(line.ObjectType==Line.DrawObjectType.DrawBeeLine)
				{
					for(int j=0;j<line.LineNodeCount-2;j+=2)
					{
						g.DrawLine(this.penDrawLine,line.GetLineNodeInfo(j),line.GetLineNodeInfo(j+1),line.GetLineNodeInfo(j+2),line.GetLineNodeInfo(j+3));
					}
					this.control.testDrawLength.Text=line.Content;
					int stringleft=(line.X0+line.X1)/2-this.control.testDrawLength.Width/2+3;
					g.DrawString(line.Content,line.LineTextFont,new SolidBrush(line.LineColor),stringleft,(line.Y0+line.Y1)/2);
				}
				else
				{
					this.penDrawBeeLine.Color=line.LineColor;
					this.penDrawBeeLine.Width=line.LineSize;
					g.DrawLine(this.penDrawBeeLine,line.GetLineNodeInfo(0),line.GetLineNodeInfo(1),line.GetLineNodeInfo(2),line.GetLineNodeInfo(3));
					g.DrawLine(this.penDrawBeeLine,line.GetLineNodeInfo(2),line.GetLineNodeInfo(3),line.GetLineNodeInfo(4),line.GetLineNodeInfo(5));
					g.DrawLine(this.penDrawLine,line.GetLineNodeInfo(4),line.GetLineNodeInfo(5),line.GetLineNodeInfo(6),line.GetLineNodeInfo(7));
					this.control.testDrawLength.Text=line.Content;
					int stringleft=(line.X1+line.X2)/2-this.control.testDrawLength.Width/2+3;
					g.DrawString(line.Content,line.LineTextFont,new SolidBrush(line.LineColor),stringleft,(line.Y1+line.Y2)/2);
				}
			}

			for(int i=0;i<this.arrNodeNotSelectList.Count;i++)
			{	
				node=((Node)arrNodeNotSelectList[i]);
				this.image=this.getTypeImage.GetImage(node.ObjectType);
				if(this.image!=null)
				{
					DrawImage(g,this.image,node.X,node.Y,node.Width,node.Height);
				}
				else
				{
					if(node.ObjectType==Node.DrawObjectType.DrawRectangle)
					{
						this.penDrawNode.Color=node.BorderColor;
						g.DrawRectangle(this.penDrawNode,node.X,node.Y,node.Width,node.Height);
						this.penDrawNode.Color=node.FillColor;
						g.FillRectangle(this.penDrawNode.Brush,node.X,node.Y,node.Width,node.Height);
					}
					else
					{
						this.penDrawNode.Color=node.BorderColor;
						g.DrawEllipse(this.penDrawNode,node.X,node.Y,node.Width,node.Height);
						this.penDrawNode.Color=node.FillColor;
						g.FillEllipse(this.penDrawNode.Brush,node.X,node.Y,node.Width,node.Height);							
					}
				}
				//g.DrawString(node.NodeText,new Font("����",10),new SolidBrush(Color.Black),node.X,node.Y);
				this.control.testDrawLength.Font=node.NodeTextFont;
				this.control.testDrawLength.Text=node.NodeText;
				int stringleft=node.X+node.Width/2-this.control.testDrawLength.Width/2+3;
				g.DrawString(node.NodeText,node.NodeTextFont,new SolidBrush(node.NodeTextColor),stringleft,node.Y+node.Height);

			}

			for(int i=0;i<this.arrDrawStringNotSelectList.Count;i++)
			{
				drawString =((DrawString)this.arrDrawStringNotSelectList[i]);
				int width,height;
				if(drawString.Width<=2)
				{
					width=2;
				}
				else 
				{
					width=drawString.Width;
				}

				if(drawString.Height<=2)
				{
					height=2;
				}
				else
				{
					height=drawString.Height;
				}
				this.re=new RectangleF(drawString.X,drawString.Y,width,height);
				this.penDrawString.Color=drawString.DSTextColor;
				g.DrawString(drawString.Content,drawString.DSTextFont,this.penDrawString.Brush,this.re);
			}			
		}

		public void ReDrawBitmapNodeMove(Graphics g,int index)//���������ӵ���ǰ�Ľڵ���
		{
			DrawBackGround();
			Line line;
			Node node;
			DrawString drawString;
			for(int i=0;i<this.arrLineList.Count;i++)
			{	
				bool bConectNode=false;
				for(int j=0;j<this.arrLineConnectNode.Count;j++)
				{
					if(((Line)this.arrLineList[i]).Equals(((Line)this.arrLineConnectNode[j])))
					{
						bConectNode=true;
					}
				}

				if(!bConectNode)
				{
					line=((Line)arrLineList[i]);
					this.penDrawLine.Color=line.LineColor;
					this.penDrawLine.Width=line.LineSize;
					if(line.ObjectType==Line.DrawObjectType.DrawBeeLine)
					{
						for(int j=0;j<line.LineNodeCount-2;j+=2)
						{
							g.DrawLine(this.penDrawLine,line.GetLineNodeInfo(j),line.GetLineNodeInfo(j+1),line.GetLineNodeInfo(j+2),line.GetLineNodeInfo(j+3));
						}
						this.control.testDrawLength.Text=line.Content;
						int stringleft=(line.X0+line.X1)/2-this.control.testDrawLength.Width/2+3;
						g.DrawString(line.Content,line.LineTextFont,new SolidBrush(line.LineColor),stringleft,(line.Y0+line.Y1)/2);
					}
					else
					{
						this.penDrawBeeLine.Color=line.LineColor;
						this.penDrawBeeLine.Width=line.LineSize;
						g.DrawLine(this.penDrawBeeLine,line.GetLineNodeInfo(0),line.GetLineNodeInfo(1),line.GetLineNodeInfo(2),line.GetLineNodeInfo(3));
						g.DrawLine(this.penDrawBeeLine,line.GetLineNodeInfo(2),line.GetLineNodeInfo(3),line.GetLineNodeInfo(4),line.GetLineNodeInfo(5));
						g.DrawLine(this.penDrawLine,line.GetLineNodeInfo(4),line.GetLineNodeInfo(5),line.GetLineNodeInfo(6),line.GetLineNodeInfo(7));
						this.control.testDrawLength.Text=line.Content;
						int stringleft=(line.X1+line.X2)/2-this.control.testDrawLength.Width/2+3;
						g.DrawString(line.Content,line.LineTextFont,new SolidBrush(line.LineColor),stringleft,(line.Y1+line.Y2)/2);
					}
				}
			}

			for(int i=0;i<this.arrNodeList.Count;i++)
			{	
				if( i!=index)
				{
					node=((Node)arrNodeList[i]);
					this.image=this.getTypeImage.GetImage(node.ObjectType);
					if(this.image!=null)
					{
						DrawImage(g,this.image,node.X,node.Y,node.Width,node.Height);
					}
					else
					{
						if(node.ObjectType==Node.DrawObjectType.DrawRectangle)
						{
							this.penDrawNode.Color=node.BorderColor;
							g.DrawRectangle(this.penDrawNode,node.X,node.Y,node.Width,node.Height);
							this.penDrawNode.Color=node.FillColor;
							g.FillRectangle(this.penDrawNode.Brush,node.X,node.Y,node.Width,node.Height);
						}
						else
						{
							this.penDrawNode.Color=node.BorderColor;
							g.DrawEllipse(this.penDrawNode,node.X,node.Y,node.Width,node.Height);
							this.penDrawNode.Color=node.FillColor;
							g.FillEllipse(this.penDrawNode.Brush,node.X,node.Y,node.Width,node.Height);							
						}
					}
					//g.DrawString(node.NodeText,new Font("����",10),new SolidBrush(Color.Black),node.X,node.Y);
					this.control.testDrawLength.Font=node.NodeTextFont;
					this.control.testDrawLength.Text=node.NodeText;
					int stringleft=node.X+node.Width/2-this.control.testDrawLength.Width/2+3;
					g.DrawString(node.NodeText,node.NodeTextFont,new SolidBrush(node.NodeTextColor),stringleft,node.Y+node.Height);
				}
			}		
			for(int i=0;i<this.arrDrawStringList.Count;i++)
			{
				drawString =((DrawString)this.arrDrawStringList[i]);
				int width,height;
				if(drawString.Width<=2)
				{
					width=2;
				}
				else 
				{
					width=drawString.Width;
				}

				if(drawString.Height<=2)
				{
					height=2;
				}
				else
				{
					height=drawString.Height;
				}
				this.re=new RectangleF(drawString.X,drawString.Y,width,height);
				this.penDrawString.Color=drawString.DSTextColor;
				g.DrawString(drawString.Content,drawString.DSTextFont,this.penDrawString.Brush,this.re);
			}
		}

		public void RefreshBackground()
		{
			Size sz = this.control.Size;
			if(sz.Width<=0) sz.Width=1;
			if(sz.Height<=0) sz.Height=1;
			Rectangle rt = new Rectangle(0, 0, sz.Width, sz.Height);
			this.bitmapBackGroupMap = this.bitmapMemeory.Clone(rt, this.bitmapMemeory.PixelFormat);
			this.control.BackgroundImage = this.bitmapBackGroupMap;
		}
		#endregion

		#region ����Ѿ�����������
		public void ClearLineConnectNode()
		{	
			Line line;
			for(int i=0;i<this.arrLineNotSelectList.Count;i++)
			{
				line=(Line)this.arrLineNotSelectList[i];
				if(this.isNodeInNodeList(line.FirstNode,this.arrNodeSelectList))
				{
					line.FirstNode.ConnectOutCount--;
					line.FirstNode=null;
					line.FirNodeInterfaceIndex=9;
				}
				if(this.isNodeInNodeList(line.SecondNode,this.arrNodeSelectList))
				{
					line.SecondNode.ConnectInCount--;
					line.SecondNode=null;
					line.SecNodeInterfaceIndex=9;
				}
			}
		}

		public void ClearNodeConnectLine()
		{
			Line line;
			for(int i=0;i<this.arrLineSelectList.Count;i++)
			{
				line=(Line)this.arrLineSelectList[i];
				if(this.isNodeInNodeList(line.FirstNode,this.arrNodeNotSelectList))
				{
					line.FirstNode.ConnectOutCount--;
					line.FirstNode=null;
					line.FirNodeInterfaceIndex=9;
				}
				if(this.isNodeInNodeList(line.SecondNode,this.arrNodeNotSelectList))
				{
					line.SecondNode.ConnectInCount--;
					line.SecondNode=null;
					line.SecNodeInterfaceIndex=9;
				}
			}
		}

		public void ClearLineConnectAttrChange(Line line,int LinePointIndex)
		{
			if(LinePointIndex==0)
			{
				if(line.FirstNode!=null)
				{
					line.FirstNode.ConnectOutCount--;
					line.FirstNode=null;
					line.FirNodeInterfaceIndex=9;
				}
			}
			else if(LinePointIndex==1)
			{
				if(line.SecondNode!=null)
				{
					line.SecondNode.ConnectInCount--;
					line.SecondNode=null;
					line.SecNodeInterfaceIndex=9;
				}
			}

		}
		#endregion

		#region �ж�������ڵ�λ��
		public bool isNodeInNodeList(Node node,ArrayList arrayList)//�Ƿ�ڵ���ĳ�ڵ�������
		{
			for(int i=0;i<arrayList.Count;i++)
			{
				Node node_=(Node)arrayList[i];
				if(node==node_)
				{
					return true;
				}
			}
			return false;
		}

		public Line IsMouseOnLine(int x,int y,ArrayList arrayList)
		{
			ArrayList arrLineList=arrayList;
			Line line;
			for(int i=0;i<arrLineList.Count;i++)
			{
				line=((Line)arrLineList[i]);
				if(line.ObjectType==Line.DrawObjectType.DrawBeeLine)
				{
					int x0=line.GetLineNodeInfo(0);
					int y0=line.GetLineNodeInfo(1);
					int x1=line.GetLineNodeInfo(2);
					int y1=line.GetLineNodeInfo(3);
					double douMLocToFir=this.GetDistance(new Point(x0,y0),new Point(x,y));
					double douMLocToSec=this.GetDistance(new Point(x1,y1),new Point(x,y));
					double douFirToSec=this.GetDistance(new Point(x0,y0),new Point(x1,y1));
					if(0<(douMLocToFir+douMLocToSec-douFirToSec) && (douMLocToFir+douMLocToSec-douFirToSec)<1)
					{
						this.control.Cursor=Cursors.SizeAll;
						return line;
					}
					else
					{
						this.control.Cursor=Cursors.Default;
					}
				}
				else
				{
					for(int j=0;j<line.LineNodeCount-2;j+=2)
					{
						int x0=line.GetLineNodeInfo(j);
						int y0=line.GetLineNodeInfo(j+1);
						int x1=line.GetLineNodeInfo(j+2);
						int y1=line.GetLineNodeInfo(j+3);
						double douMLocToFir=this.GetDistance(new Point(x0,y0),new Point(x,y));
						double douMLocToSec=this.GetDistance(new Point(x1,y1),new Point(x,y));
						double douFirToSec=this.GetDistance(new Point(x0,y0),new Point(x1,y1));
						if(0<(douMLocToFir+douMLocToSec-douFirToSec) && (douMLocToFir+douMLocToSec-douFirToSec)<1)
						{
							this.control.Cursor=Cursors.SizeAll;
							this.iOnLineSegment=j/2;
							return line;
						}
						else
						{
							this.control.Cursor=Cursors.Default;
						}	
					}
				}
			}
			this.control.Cursor=Cursors.Default;
			this.iOnLineSegment=9;
			return null;
		}

		public double GetDistance(Point p1,Point p2)//�ж������ľ���		{			return Math.Sqrt(Math.Pow((p1.X - p2.X),2) + Math.Pow((p1.Y  - p2.Y ),2));		}

		public Node IsMouseOnNode(int x,int y,ArrayList arrayList)//�ж�����Ƿ��ڽڵ���
		{
			ArrayList arrNodeList=arrayList;
			for(int i=arrNodeList.Count;i>0;i--)//���� ��ѯ  ��󻭵Ľڵ�  λ�������� 
			{
				Node node=((Node)arrNodeList[i-1]);
				if(node.X<x && x<node.X+node.Width && node.Y<y && y<node.Y+node.Height)
				{
					this.control.Cursor=Cursors.SizeAll;
					return node;
				}
				else 
				{
					this.control.Cursor=Cursors.Default;
				}
			}
			this.control.Cursor=Cursors.Default;
			return null;
		}
		
		public DrawString IsMouseOnDrawString(int x,int y,ArrayList arrayList)
		{
			ArrayList arrDrawStringList=arrayList;
			for(int i=arrDrawStringList.Count;i>0;i--)//�����ѯ  ��󻭵�д�ְ�λ�������� 
			{
				DrawString drawString=((DrawString)arrDrawStringList[i-1]);
				if(drawString.X<x && x<drawString.X+drawString.Width && drawString.Y<y && y<drawString.Y+drawString.Height)
				{
					this.control.Cursor=Cursors.SizeAll;
					return drawString;
				}
				else 
				{
					this.control.Cursor=Cursors.Default;
				}
			}
			this.control.Cursor=Cursors.Default;
			return null;		
		}
		
		public bool IsMouseOnAnyControl(int x,int y)
		{
			Line line=null;
			Node node=null;
			DrawString drawString=null;

			if(this.bSelectAllReday || this.bSelectRectangleReday)//ȫ��ѡ��ĵ���ж�
			{
				line=this.IsMouseOnLine(x,y,this.arrLineList);
				node=this.IsMouseOnNode(x,y,this.arrNodeList);
				drawString=this.IsMouseOnDrawString(x,y,this.arrDrawStringList);
				if(line==null && node==null && drawString==null)
				{
//					this.control.menuItem6.Enabled=false;
					this.control.menuItem2.Enabled=false;
//					this.control.menuItem7.Enabled=false;
//					this.control.menuItem8.Enabled=false;
				}
				else
				{
//					this.control.menuItem6.Enabled=true;
					this.control.menuItem2.Enabled=true;
//					this.control.menuItem7.Enabled=true;
//					this.control.menuItem8.Enabled=true;
				}
				this.control.menuItem7.Enabled=false;
				this.control.menuItem8.Enabled=false;
			}
			else
			{
				ArrayList arrayList=new ArrayList();;
				if( this.lastEdit is Line)
				{
					arrayList.Add((Line)this.lastEdit);
					line=this.IsMouseOnLine(x,y,arrayList);
				}
				else if(this.lastEdit is Node)
				{
					arrayList.Add((Node)this.lastEdit);
					node=this.IsMouseOnNode(x,y,arrayList);
				}
				else if(this.lastEdit is DrawString)
				{
					arrayList.Add((DrawString)this.lastEdit);
					drawString=this.IsMouseOnDrawString(x,y,arrayList);
				}

				if(line==null && node==null && drawString==null)
				{
					this.control.menuItem2.Enabled=false;
					this.control.menuItem7.Enabled=false;
					this.control.menuItem8.Enabled=false;
				}
				else
				{
					this.control.menuItem2.Enabled=true;
					this.control.menuItem7.Enabled=true;
					this.control.menuItem8.Enabled=true;
				}
			}
			if(drawString!=null)
			{
				this.objPrepareDel=drawString;
				return true;
			}
			else if(node!=null)
			{
				this.objPrepareDel=node;
				return true;
			}
			else if(line!=null)
			{
				this.objPrepareDel=line;
				return true;
			}
			this.objPrepareDel=null;
			return false;
		}

		public bool IsMouseOnAnySelectControl(int x,int y)
		{		
			Line line=this.IsMouseOnLine(x,y,this.arrLineSelectList);
			Node node=this.IsMouseOnNode(x,y,this.arrNodeSelectList);
			DrawString drawString=this.IsMouseOnDrawString(x,y,this.arrDrawStringSelectList);

			if(drawString!=null)
			{
				return true;
			}
			else if(node!=null)
			{
				return true;
			}
			else if(line!=null)
			{
				return true;
			}
			return false;
		}

		#endregion

		#region �洢�����еĲ���
		/// <summary>
		/// ��������Ԫ��ID
		/// </summary>
		/// <param name="arrayListIndex"></param>
		public void FlashArrayList(int arrayListIndex)//arrayListIndex=0 ��ʾ�ߣ�arrayListIndex=1��ʾ�ڵ㣬arrayListIndex=2��ʾд�ְ�
		{
			Line line;
			Node node;
			DrawString drawString;
			if(arrayListIndex==0)
			{
				for(int i=0;i<this.arrLineList.Count;i++)
				{
					line=(Line)this.arrLineList[i];
					line.LineListIndex=i;
				}
			}
			else if(arrayListIndex==1)
			{
				for(int i=0;i<this.arrNodeList.Count;i++)
				{
					node=(Node)this.arrNodeList[i];
					node.NodeListIndex=i;
				}
			}
			else if(arrayListIndex==2)
			{
				for(int i=0;i<this.arrDrawStringList.Count;i++)
				{
					drawString=(DrawString)this.arrDrawStringList[i];
					drawString.DrawStrListIndex=i;
				}			
			}
		}

		/// <summary>
		/// �ö�
		/// </summary>
		public void SetTop()
		{
			if(this.objPrepareDel is Line)
			{
				Line line=(Line)this.objPrepareDel;
				this.arrLineList.Add(line);
				this.arrLineList.Remove(line);
				this.FlashArrayList(0);
			}
			else if(this.objPrepareDel is Node)
			{
				Node node=(Node)this.objPrepareDel;
				this.arrNodeList.Add(node);
				this.arrNodeList.Remove(node);
				this.FlashArrayList(1);
			}
			else if(this.objPrepareDel is DrawString)
			{
				DrawString drawString=(DrawString)this.objPrepareDel;
				this.arrDrawStringList.Add(drawString);
				this.arrDrawStringList.Remove(drawString);
				this.FlashArrayList(2);
			}
			this.reDrawBitmap(this.graDrawPanel,10,10);			this.RefreshBackground();
			GC.Collect();
		}
		/// <summary>
		/// �õ�
		/// </summary>
		public void SetDown()
		{
			if(this.objPrepareDel is Line)
			{
				Line line=(Line)this.objPrepareDel;
				this.arrLineList.Remove(line);
				this.arrLineList.Insert(0,line);
				this.FlashArrayList(0);
			}
			else if(this.objPrepareDel is Node)
			{
				Node node=(Node)this.objPrepareDel;
				this.arrNodeList.Remove(node);
				this.arrNodeList.Insert(0,node);
				this.FlashArrayList(1);
			}
			else if(this.objPrepareDel is DrawString)
			{
				DrawString drawString=(DrawString)this.objPrepareDel;
				this.arrDrawStringList.Remove(drawString);
				this.arrDrawStringList.Insert(0,drawString);
				this.FlashArrayList(2);
			}
			this.reDrawBitmap(this.graDrawPanel,10,10);			this.RefreshBackground();
			GC.Collect();			
		}

		#endregion

		#region �ڵ��ƶ� �߸��Ÿı� ����
		/// <summary>
		/// ��ȡ���ӵ�ѡ�нڵ����
		/// </summary>
		/// <param name="node"></param>
		public void GetConnectNodeLine(Node node)//��ȡ���ӵ�ѡ�нڵ����
		{
			this.arrLineConnectNode.Clear();
			for(int i=0;i<this.arrLineList.Count;i++)
			{
				if(((Line)this.arrLineList[i]).FirstNode==node || ((Line)this.arrLineList[i]).SecondNode==node)
				{
					this.arrLineConnectNode.Add(((Line)this.arrLineList[i]));
				}
			}
		}
		/// <summary>
		/// ��̬�ı�ڵ��С�����ӵ�������߸����ƶ�
		/// </summary>
		/// <param name="node">�ڵ�</param>
		public void ReSizeNodeConLine(Node node)//��̬�ı�ڵ��С�����ӵ�������߸����ƶ�
		{
			Line line;
			for(int i=0;i<this.arrLineConnectNode.Count;i++)
			{
				line=((Line)this.arrLineConnectNode[i]);
				if(line.FirstNode==node)
				{
					switch(line.FirNodeInterfaceIndex)
					{
						case 0: line.X0=node.X+node.Width/2;line.Y0=node.Y; break;
						case 1: line.X0=node.X; line.Y0=node.Y+node.Height/2; break;
						case 2: line.X0=node.X+node.Width; line.Y0=node.Y+node.Height/2; break;
						case 3: line.X0=node.X+node.Width/2; line.Y0=node.Y+node.Height; break;
					}
					if(line.ObjectType==Line.DrawObjectType.DrawFoldLine)
					{
						switch(line.FirNodeInterfaceIndex)
						{
							case 0: line.Y1=node.Y; break;
							case 1: line.Y1=node.Y+node.Height/2; break;
							case 2: line.Y1=node.Y+node.Height/2; break;
							case 3: line.Y1=node.Y+node.Height; break;
						}
					}
				}
				if(line.SecondNode==node)
				{
					if(line.ObjectType==Line.DrawObjectType.DrawBeeLine)
					{
						switch(line.SecNodeInterfaceIndex)
						{
							case 0: line.X1=node.X+node.Width/2; line.Y1=node.Y; break;
							case 1: line.X1=node.X; line.Y1=node.Y+node.Height/2; break;
							case 2: line.X1=node.X+node.Width; line.Y1=node.Y+node.Height/2; break;
							case 3: line.X1=node.X+node.Width/2; line.Y1=node.Y+node.Height; break;
						}
					}
					else
					{
						switch(line.SecNodeInterfaceIndex)
						{
							case 0: line.X3=node.X+node.Width/2; line.Y3=node.Y; line.Y2=node.Y; break;
							case 1: line.X3=node.X; line.Y3=node.Y+node.Height/2; line.Y2=node.Y+node.Height/2; break;
							case 2: line.X3=node.X+node.Width; line.Y3=node.Y+node.Height/2; line.Y2=node.Y+node.Height/2; break;
							case 3: line.X3=node.X+node.Width/2; line.Y3=node.Y+node.Height; line.Y2=node.Y+node.Height; break;
						}
					}
				}
			}
		}

		#endregion

		#region �ڵ��С�ĸı�
		/// <summary>
		/// �ڵ��С�ı��ʱ���X������趨
		/// </summary>
		/// <param name="x"></param>
		public void ChangeNodeX(int x)
		{
			if(x<this.iTempNodeX)
			{
				this.iNodeX=x;
				this.iNodeWidth=this.iTempNodeX-x;
			}
			else
			{
				this.iNodeX=this.iTempNodeX;
				this.iNodeWidth=x-this.iTempNodeX;
			}
		}
		/// <summary>
		/// �ڵ��С�ı��ʱ���Y������趨
		/// </summary>
		/// <param name="y"></param>
		public void ChangeNodeY(int y)
		{
			if(y<this.iTempNodeY)
			{
				this.iNodeY=y;
				this.iNodeHeight=this.iTempNodeY-y;
			}
			else
			{
				this.iNodeY=this.iTempNodeY;
				this.iNodeHeight=y-this.iTempNodeY;
			}
		}

		/// <summary>
		/// �ڵ�ı��С
		/// </summary>
		/// <param name="e"></param>
		public void ChangeNodeSize(System.Windows.Forms.MouseEventArgs e)
		{
			if(this.iResizeNodeMouseIndex==0)
			{
				this.ChangeNodeX(e.X);
				this.ChangeNodeY(e.Y);
			}
			else if(this.iResizeNodeMouseIndex==1)
			{
				this.ChangeNodeY(e.Y);
			}
			else if(this.iResizeNodeMouseIndex==2)
			{
				this.ChangeNodeX(e.X);
				this.ChangeNodeY(e.Y);
			}
			else if(this.iResizeNodeMouseIndex==3)
			{
				this.ChangeNodeX(e.X);
			}
			else if(this.iResizeNodeMouseIndex==4)
			{
				this.ChangeNodeX(e.X);
			}
			else if(this.iResizeNodeMouseIndex==5)
			{
				this.ChangeNodeX(e.X);
				this.ChangeNodeY(e.Y);
			}
			else if(this.iResizeNodeMouseIndex==6)
			{
				this.ChangeNodeY(e.Y);
			}
			else if(this.iResizeNodeMouseIndex==7)
			{
				this.ChangeNodeX(e.X);
				this.ChangeNodeY(e.Y);
			}				
		}
		#endregion

		#region д�ְ��С�ĸı�
		/// <summary>
		///  д�ְ��С�ĸı� X������趨
		/// </summary>
		/// <param name="x"></param>
		public void ChangeDrawStringX(int x)
		{
			if(x<this.iTempDSX)
			{
				this.iDrawStringX=x;
				this.iDrawStringWidth=this.iTempDSX-x;
			}
			else
			{
				this.iDrawStringX=this.iTempDSX;
				this.iDrawStringWidth=x-this.iTempDSX;
			}			
		}

		/// <summary>
		///  д�ְ��С�ĸı� Y������趨
		/// </summary>
		/// <param name="y"></param>
		public void ChangeDrawStringY(int y)
		{
			if(y<this.iTempDSY)
			{
				this.iDrawStringY=y;
				this.iDrawStringHeight=this.iTempDSY-y;
			}
			else
			{
				this.iDrawStringY=this.iTempDSY;
				this.iDrawStringHeight=y-this.iTempDSY;
			}		
		}

		/// <summary>
		///  д�ְ��С�ĸı�
		/// </summary>
		/// <param name="e"></param>
		public void ChangeDSSize(System.Windows.Forms.MouseEventArgs e)
		{
			if(this.iResizeDSMouseIndex==0)
			{
				this.ChangeDrawStringX(e.X);
				this.ChangeDrawStringY(e.Y);
			}
			else if(this.iResizeDSMouseIndex==1)
			{
				this.ChangeDrawStringY(e.Y);
			}
			else if(this.iResizeDSMouseIndex==2)
			{
				this.ChangeDrawStringX(e.X);
				this.ChangeDrawStringY(e.Y);
			}
			else if(this.iResizeDSMouseIndex==3)
			{
				this.ChangeDrawStringX(e.X);
			}
			else if(this.iResizeDSMouseIndex==4)
			{
				this.ChangeDrawStringX(e.X);
			}
			else if(this.iResizeDSMouseIndex==5)
			{
				this.ChangeDrawStringX(e.X);
				this.ChangeDrawStringY(e.Y);
			}
			else if(this.iResizeDSMouseIndex==6)
			{
				this.ChangeDrawStringY(e.Y);
			}
			else if(this.iResizeDSMouseIndex==7)
			{
				this.ChangeDrawStringX(e.X);
				this.ChangeDrawStringY(e.Y);
			}			
		}

		#endregion

		#region �ı�����
		/// <summary>
		/// �ı�����
		/// </summary>
		/// <param name="line">�ı������</param>
		/// <param name="iLinePointIndex">�˵�����ı仯  ��Ϊ9��ʱ��</param>
		/// <param name="iLineIndex">������ı仯  ��Ϊ9��ʱ��</param>
		/// <param name="x">��ǰ�����X���� </param>
		/// <param name="y">��ǰ�����Y���� </param>
		public void ChangeFlodLine(Line line,int iLinePointIndex,int iLineIndex,int x,int y)
		{
			if(iLinePointIndex==0)
			{
				int distanceFir_Sec,distanceSec_3th;
				if(!line.Modality)
				{
					distanceFir_Sec=Math.Abs(this.iLineSecondNodeX-this.iLineFirstNodeX);
					distanceSec_3th=Math.Abs(this.iLine3thNodeY-this.iLineSecondNodeY);
					if(distanceFir_Sec*2<distanceSec_3th)
					{
						line.Modality=true;
						this.iLineSecondNodeX=x;
						this.iLineSecondNodeY=(this.iLineFirstNodeY+this.iLine4thNodeY)/2;
						this.iLine3thNodeX=this.iLine4thNodeX;
						this.iLine3thNodeY=this.iLineSecondNodeY;
						this.selectPoint.LinePoint[1].X=this.iLineSecondNodeX;
						this.selectPoint.LinePoint[1].Y=this.iLineSecondNodeY;
						this.selectPoint.LinePoint[2].X=this.iLine3thNodeX;
						this.selectPoint.LinePoint[2].Y=this.iLine3thNodeY;
					}
				}
				else
				{
					distanceFir_Sec=Math.Abs(this.iLineSecondNodeY-this.iLineFirstNodeY);
					distanceSec_3th=Math.Abs(this.iLine3thNodeX-this.iLineSecondNodeX);
					if(distanceFir_Sec*2<distanceSec_3th)
					{
						line.Modality=false;
						this.iLineSecondNodeX=(this.iLineFirstNodeX+this.iLine4thNodeX)/2;
						this.iLineSecondNodeY=this.iLineFirstNodeY;
						this.iLine3thNodeX=this.iLineSecondNodeX;
						this.iLine3thNodeY=this.iLine4thNodeY;
						this.selectPoint.LinePoint[1].X=this.iLineSecondNodeX;
						this.selectPoint.LinePoint[1].Y=this.iLineSecondNodeY;
						this.selectPoint.LinePoint[2].X=this.iLine3thNodeX;
						this.selectPoint.LinePoint[2].Y=this.iLine3thNodeY;
					}
				}
			}

			if(iLinePointIndex!=9 && iLineIndex==9)
			{
				if(iLinePointIndex==0)
				{
					if(!line.Modality)
					{
						this.iLineFirstNodeX=x;
						this.iLineFirstNodeY=y;
						this.iLineSecondNodeY=y;
						this.selectPoint.LinePoint[0].X=x;
						this.selectPoint.LinePoint[0].Y=y;
						this.selectPoint.LinePoint[1].Y=y;
					}
					else
					{
						this.iLineFirstNodeX=x;
						this.iLineFirstNodeY=y;
						this.iLineSecondNodeX=x;
						this.selectPoint.LinePoint[0].X=x;
						this.selectPoint.LinePoint[0].Y=y;
						this.selectPoint.LinePoint[1].X=x;
					}
				}
				else if(iLinePointIndex==1)
				{
					if(!line.Modality)
					{
						this.iLineFirstNodeY=y;
						this.iLineSecondNodeX=x;
						this.iLineSecondNodeY=y;
						this.iLine3thNodeX=x;
						this.selectPoint.LinePoint[0].Y=y;
						this.selectPoint.LinePoint[1].X=x;
						this.selectPoint.LinePoint[1].Y=y;
						this.selectPoint.LinePoint[2].X=x;
					}
					else
					{
						this.iLineFirstNodeX=x;
						this.iLineSecondNodeX=x;
						this.iLineSecondNodeY=y;
						this.iLine3thNodeY=y;
						this.selectPoint.LinePoint[0].X=x;
						this.selectPoint.LinePoint[1].X=x;
						this.selectPoint.LinePoint[1].Y=y;
						this.selectPoint.LinePoint[2].Y=y;						
					}
				}
				else if(iLinePointIndex==2)
				{
					if(!line.Modality)
					{
						this.iLineSecondNodeX=x;
						this.iLine3thNodeX=x;
						this.iLine3thNodeY=y;
						this.iLine4thNodeY=y;
						this.selectPoint.LinePoint[1].X=x;
						this.selectPoint.LinePoint[2].X=x;
						this.selectPoint.LinePoint[2].Y=y;
						this.selectPoint.LinePoint[3].Y=y;
					}
					else
					{
						this.iLineSecondNodeY=y;
						this.iLine3thNodeX=x;
						this.iLine3thNodeY=y;
						this.iLine4thNodeX=x;
						this.selectPoint.LinePoint[1].Y=y;
						this.selectPoint.LinePoint[2].X=x;
						this.selectPoint.LinePoint[2].Y=y;
						this.selectPoint.LinePoint[3].X=x;					
					}
				}
				else if(iLinePointIndex==3)
				{
					if(!line.Modality)
					{					
						this.iLine3thNodeY=y;
						this.iLine4thNodeX=x;
						this.iLine4thNodeY=y;
						this.selectPoint.LinePoint[2].Y=y;
						this.selectPoint.LinePoint[3].X=x;
						this.selectPoint.LinePoint[3].Y=y;
					}
					else
					{
						this.iLine3thNodeX=x;
						this.iLine4thNodeX=x;
						this.iLine4thNodeY=y;
						this.selectPoint.LinePoint[2].X=x;
						this.selectPoint.LinePoint[3].X=x;
						this.selectPoint.LinePoint[3].Y=y;					
					}
				}
			}
			else if(iLinePointIndex==9 && iLineIndex!=9)
			{
				if(iLineIndex==0)
				{
					if(!line.Modality)
					{
						this.iLineFirstNodeY=y;
						this.iLineSecondNodeY=y;
						this.selectPoint.LinePoint[0].Y=y;
						this.selectPoint.LinePoint[1].Y=y;
						if(x<this.iLineFirstNodeX)
						{
							this.iLineFirstNodeX=x;
							this.selectPoint.LinePoint[0].X=x;
						}
					}
					else
					{
						this.iLineFirstNodeX=x;
						this.iLineSecondNodeX=x;
						this.selectPoint.LinePoint[0].X=x;
						this.selectPoint.LinePoint[1].X=x;
					}
				}
				else if(iLineIndex==1)
				{
					
				}
				else if(iLineIndex==2)
				{
					if(!line.Modality)
					{
						this.iLine3thNodeY=y;
						this.iLine4thNodeY=y;
						this.selectPoint.LinePoint[2].Y=y;
						this.selectPoint.LinePoint[3].Y=y;
						if(x>this.iLine4thNodeX)
						{
							this.iLine4thNodeX=x;
							this.selectPoint.LinePoint[3].X=x;
						}
					}
					else
					{
						this.iLine3thNodeX=x;
						this.iLine4thNodeX=x;
						this.selectPoint.LinePoint[2].X=x;
						this.selectPoint.LinePoint[3].X=x;
					}
				}
			}
		}

		/// <summary>
		/// �ƶ��ڵ��ʱ��ı����ߵ�����
		/// </summary>
		/// <param name="line">����</param>
		/// <param name="iLinePointIndex">�˵�ID </param>
		/// <param name="node">���ӵ��Ľڵ�</param>
		/// <param name="iConnectIndex">�ڵ������ӵ��ID</param>
		public void MoveNodeChangeLine(Line line,int iLinePointIndex,Node node,int iConnectIndex)
		{
			int x=0,y=0;
			switch(iConnectIndex)
			{
				case 0:
					x=this.iNodeX+this.iNodeWidth/2;
					y=this.iNodeY;
					break;
				case 1:
					x=this.iNodeX;
					y=this.iNodeY+this.iNodeHeight/2;
					break;
				case 2:
					x=this.iNodeX+this.iNodeWidth;
					y=this.iNodeY+this.iNodeHeight/2;
					break;
				case 3:
					x=this.iNodeX+this.iNodeWidth/2;
					y=this.iNodeY+this.iNodeHeight;
					break;
			}
			if(iLinePointIndex==0)
			{
				line.X0=x;
				line.Y0=y;
				if(!line.Modality)
				{
					line.Y1=y;
				}
				else
				{
					line.X1=x;
				}
			}
			else if(iLinePointIndex==1)
			{
				line.X0=y;
				line.X1=x;
				line.Y1=y;
				line.X2=x;
			}
			else if(iLinePointIndex==2)
			{
				line.X1=x;
				line.X2=x;
				line.Y2=y;
				line.Y3=y;
			}
			else if(iLinePointIndex==3)
			{
				if(!line.Modality)
				{
					line.Y2=y;
				}
				else
				{
					line.X2=x;
				}
				line.X3=x;
				line.Y3=y;
			}			
		}

		#endregion

		#region �ڵ㹦����Ϣ����
		/// <summary>
		/// ����������ʾ����
		/// </summary>
		public void AttriShow()		{			if(this.lastEdit is Node || this.nodeSelectNode!=null)			{				this.control.epd_backGround.Hide();				this.control.epd_lineProperty.Hide();				this.control.epd_nodeProperty.Show();				this.control.epd_nodeProperty.Top=12;				this.control.epd_stringProperty.Hide();				Node node;				if(this.nodeSelectNode!=null)				{					node=this.nodeSelectNode;					this.control.node_X.Text=""+this.iNodeX;					this.control.node_Y.Text=""+this.iNodeY;					this.control.node_Width.Text=""+this.iNodeWidth;					this.control.node_Height.Text=""+this.iNodeHeight;					this.control.node_ID.Text=""+node.NodeListIndex;					this.control.node_Type.Text=""+getDefaultText(node.ObjectType);					this.control.node_Name.Text=""+node.NodeText;					this.control.node_Size.Text=""+node.TextSize;					this.control.node_Font_Color.BackColor=node.NodeTextColor;					if(node.ObjectType==Node.DrawObjectType.DrawRectangle ||node.ObjectType==Node.DrawObjectType.DrawEllipse)					{						this.control.node_Border_Color.Visible=true;						this.control.node_Fill_Color.Visible=true;						this.control.label11.Visible=true;						this.control.label12.Visible=true;						this.control.node_Border_Color.BackColor=node.BorderColor;						this.control.node_Fill_Color.BackColor=node.FillColor;						this.control.epd_nodeProperty.Height=280;					}					else					{						this.control.node_Border_Color.Visible=false;						this.control.node_Fill_Color.Visible=false;						this.control.label11.Visible=false;						this.control.label12.Visible=false;						this.control.epd_nodeProperty.Height=230;					}				}				else 				{					node=(Node)this.lastEdit;					this.control.node_X.Text=""+node.X;					this.control.node_Y.Text=""+node.Y;					this.control.node_Width.Text=""+node.Width;					this.control.node_Height.Text=""+node.Height;					this.control.node_ID.Text=""+node.NodeListIndex;					this.control.node_Name.Text=""+node.NodeText;					this.control.node_Size.Text=""+node.TextSize;					this.control.node_Font_Color.BackColor=node.NodeTextColor;					if(node.ObjectType==Node.DrawObjectType.DrawRectangle ||node.ObjectType==Node.DrawObjectType.DrawEllipse)					{						this.control.node_Border_Color.Visible=true;						this.control.node_Fill_Color.Visible=true;						this.control.label11.Visible=true;						this.control.label12.Visible=true;						this.control.node_Border_Color.BackColor=node.BorderColor;						this.control.node_Fill_Color.BackColor=node.FillColor;						this.control.epd_nodeProperty.Height=280;					}					else					{						this.control.node_Border_Color.Visible=false;						this.control.node_Fill_Color.Visible=false;						this.control.label11.Visible=false;						this.control.label12.Visible=false;						this.control.epd_nodeProperty.Height=230;					}				}				return;			}			else if(this.lastEdit is Line || this.lineSelectLine!=null)			{				this.control.epd_backGround.Hide();				this.control.epd_lineProperty.Show();				this.control.epd_lineProperty.Top=12;				this.control.epd_nodeProperty.Hide();				this.control.epd_stringProperty.Hide();				Line line;				if(this.lineSelectLine!=null)				{					line=this.lineSelectLine;					this.control.line_X0.Text=""+line.X0;					this.control.line_Y0.Text=""+line.Y0;					this.control.line_X1.Text=""+line.X1;					this.control.line_Y1.Text=""+line.Y1;					this.control.line_ID.Text=""+line.LineListIndex;					this.control.line_color.BackColor=line.LineColor;					if(line.FirstNode!=null)					{						this.control.line_FirNode_ID.Text=""+line.FirstNode.NodeListIndex;					}					else 					{						this.control.line_FirNode_ID.Text="";					}					if(line.SecondNode!=null)					{						this.control.line_SecNode_ID.Text=""+line.SecondNode.NodeListIndex;					}					else					{						this.control.line_SecNode_ID.Text="";					}					this.control.line_size.Text=""+line.LineSize;					this.control.line_Type.Text=this.GetLineDefaultText(line.ObjectType);					this.control.tb_lineName.Text=line.Content;				}				else				{					line=(Line)this.lastEdit;					this.control.line_X0.Text=""+line.X0;					this.control.line_Y0.Text=""+line.Y0;					this.control.line_X1.Text=""+line.X1;					this.control.line_Y1.Text=""+line.Y1;					this.control.line_ID.Text=""+line.LineListIndex;					if(line.FirstNode!=null)					{						this.control.line_FirNode_ID.Text=""+line.FirstNode.NodeListIndex;					}					else 					{						this.control.line_FirNode_ID.Text="";					}					if(line.SecondNode!=null)					{						this.control.line_SecNode_ID.Text=""+line.SecondNode.NodeListIndex;					}					else					{						this.control.line_SecNode_ID.Text="";					}					this.control.line_size.Text=""+line.LineSize;					this.control.line_Type.Text=this.GetLineDefaultText(line.ObjectType);					this.control.tb_lineName.Text=line.Content;				}				return;			}			else if(this.lastEdit is DrawString || this.drawStringSelectDS!=null)			{				this.control.epd_backGround.Hide();				this.control.epd_lineProperty.Hide();				this.control.epd_nodeProperty.Hide();				this.control.epd_stringProperty.Show();				this.control.epd_stringProperty.Top=12;				DrawString drawString;				if(this.drawStringSelectDS!=null)				{					drawString=this.drawStringSelectDS;				}				else				{					drawString=(DrawString)this.lastEdit;				}				this.control.DS_X.Text=""+drawString.X;				this.control.DS_Y.Text=""+drawString.Y;				this.control.DS_Width.Text=""+drawString.Width;				this.control.DS_Height.Text=""+drawString.Height;				this.control.DS_ID.Text=""+drawString.DrawStrListIndex;				this.control.DS_Content.Text=""+drawString.Content;				this.control.DS_Size.Text=""+drawString.TextSize;				this.control.DS_Color.BackColor=drawString.DSTextColor;				return;			}			else if(this.lastEdit==null)			{				this.control.epd_backGround.Show();				this.control.epd_backGround.Top=12;				this.control.epd_lineProperty.Hide();				this.control.epd_nodeProperty.Hide();				this.control.epd_stringProperty.Hide();				return;			}		} 
		/// <summary>
		/// ��ʾ��ǰѡ��Ŀؼ���������
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void AttriTBChange(object sender, System.Windows.Forms.KeyEventArgs e)		{			TextBox textBox=(TextBox)sender;//��ǰ�༭���ı���			if(this.lastEdit is Node || this.nodeSelectNode!=null)			{				Node node;				if(this.nodeSelectNode!=null)				{					node=this.nodeSelectNode;				}				else				{					node=(Node)this.lastEdit;				}								if(textBox.Equals(this.control.node_X))				{					node.X=this.CheckNumber(node.X,this.control.node_X.Text,9);					this.GetConnectNodeLine(node);					this.ReSizeNodeConLine(node);					this.arrLineConnectNode.Clear();				}				else if(textBox.Equals(this.control.node_Y))				{					node.Y=this.CheckNumber(node.Y,this.control.node_Y.Text,9);					this.GetConnectNodeLine(node);					this.ReSizeNodeConLine(node);					this.arrLineConnectNode.Clear();				}				else if(textBox.Equals(this.control.node_Width))				{					node.Width=this.CheckNumber(node.Width,this.control.node_Width.Text,9);					this.GetConnectNodeLine(node);					this.ReSizeNodeConLine(node);					this.arrLineConnectNode.Clear();				}				else if(textBox.Equals(this.control.node_Height))				{					node.Height=this.CheckNumber(node.Height,this.control.node_Height.Text,9);					this.GetConnectNodeLine(node);					this.ReSizeNodeConLine(node);					this.arrLineConnectNode.Clear();				}				else if(textBox.Equals(this.control.node_Name))				{					node.NodeText=this.control.node_Name.Text;					if(this.TBnodeContent.Visible==true)					{						this.TBnodeContent.Text=this.control.node_Name.Text;					}				}				else if(textBox.Equals(this.control.node_Size))				{					node.TextSize=this.CheckNumber(node.TextSize,this.control.node_Size.Text,1);					if(this.TBnodeContent.Visible==true)					{						node.NodeText=this.TBnodeContent.Text;						this.TBnodeContent.Font=node.NodeTextFont;					}				}				this.reDrawBitmap(this.graDrawPanel,10,10);				this.RefreshBackground();					return;			}			else if(this.lastEdit is Line || this.lineSelectLine!=null)			{				Line line;				if(this.lineSelectLine!=null)				{					line=this.lineSelectLine;				}				else				{					line=(Line)this.lastEdit;				}								if(textBox.Equals(this.control.line_X0))				{					line.X0=this.CheckNumber(line.X0,this.control.line_X0.Text,9);					this.ClearLineConnectAttrChange(line,0);				}				else if(textBox.Equals(this.control.line_Y0))				{					line.Y0=this.CheckNumber(line.Y0,this.control.line_Y0.Text,9);					this.ClearLineConnectAttrChange(line,0);				}				else if(textBox.Equals(this.control.line_X1))				{					line.X1=this.CheckNumber(line.X1,this.control.line_X1.Text,9);					this.ClearLineConnectAttrChange(line,1);				}				else if(textBox.Equals(this.control.line_Y1))				{					line.Y1=this.CheckNumber(line.Y1,this.control.line_Y1.Text,9);					this.ClearLineConnectAttrChange(line,1);				}				else if(textBox.Equals(this.control.line_size))				{					line.LineSize=this.CheckNumber(line.LineSize,this.control.line_size.Text,3);				}				else if(textBox.Equals(this.control.tb_lineName))				{					line.Content=this.control.tb_lineName.Text;				}				this.reDrawBitmap(this.graDrawPanel,10,10);				this.RefreshBackground();					return;			}			else if(this.lastEdit is DrawString || this.drawStringSelectDS !=null)			{				DrawString drawString=(DrawString)this.lastEdit;				if(this.drawStringSelectDS!=null)				{					drawString=this.drawStringSelectDS;				}				else				{					drawString=(DrawString)this.lastEdit;				}				if(textBox.Equals(this.control.DS_X))				{					drawString.X=this.CheckNumber(drawString.X,this.control.DS_X.Text,9);				}				else if(textBox.Equals(this.control.DS_Y))				{					drawString.Y=this.CheckNumber(drawString.Y,this.control.DS_Y.Text,9);				}				else if(textBox.Equals(this.control.DS_Width))				{					drawString.Width=this.CheckNumber(drawString.Width,this.control.DS_Width.Text,9);				}				else if(textBox.Equals(this.control.DS_Height))				{					drawString.Height=this.CheckNumber(drawString.Height,this.control.DS_Height.Text,9);				}				else if(textBox.Equals(this.control.DS_Content))				{					drawString.Content=this.control.DS_Content.Text;					if(this.TBDrawStringContent.Visible==true)					{						this.TBDrawStringContent.Text=this.control.DS_Content.Text;					}				}				else if(textBox.Equals(this.control.DS_Size))				{					drawString.TextSize=this.CheckNumber(drawString.TextSize,this.control.DS_Size.Text,2);					if(this.TBDrawStringContent.Visible==true)					{						drawString.Content=this.TBDrawStringContent.Text;						this.TBDrawStringContent.Font=drawString.DSTextFont;					}				}				this.reDrawBitmap(this.graDrawPanel,10,10);				this.RefreshBackground();					return;			}		} 
		private int CheckNumber(int baseNumber,string checkString,int checkIndex)
		{
			char[] cchar=checkString.ToCharArray();
			foreach(char cc in cchar)
			{
				if((int)cc<48 ||(int)cc>57)
				{
					//MessageForm.Show("���õ�����ֵ��Ч,����Ϊ����.");
					MessageForm.Show("���õ�����ֵ��Ч,����Ϊ����.",ButtonType.ConfirmOnly);
					return baseNumber;
				}
			}
			if(checkIndex==1)
			{
				if(10>int.Parse(checkString) || int.Parse(checkString)>30)
				{
					MessageForm.Show("������10~30�������.");
					return baseNumber;				
				}
			}
			else if(checkIndex==2)
			{
				if(10>int.Parse(checkString) || int.Parse(checkString)>50)
				{
					MessageForm.Show("������10~50�������.");
					return baseNumber;				
				}			
			}
			else if(checkIndex==3)
			{
				if(1>int.Parse(checkString) || int.Parse(checkString)>15)
				{
					MessageForm.Show("������1~15�������.");
					return baseNumber;				
				}			
			}
			return int.Parse(checkString);
		} 

		/// <summary>
		/// �������Ϲ�����ɫ�ĸı䡡
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void ColorChange(object sender, System.EventArgs e)
		{
			Panel panel=(Panel)sender;
			if(this.lastEdit is Node || this.nodeSelectNode!=null)
			{
				Node node;
				if(this.nodeSelectNode!=null)
				{
					node=this.nodeSelectNode;
				}
				else
				{
					node=(Node)this.lastEdit;
				}
				if(panel.Equals(this.control.node_Font_Color))
				{
					node.NodeTextColor=this.control.node_Font_Color.BackColor;
				}
				else if(panel.Equals(this.control.node_Border_Color))
				{
					node.BorderColor=this.control.node_Border_Color.BackColor;
				}
				else if(panel.Equals(this.control.node_Fill_Color))
				{
					node.FillColor=this.control.node_Fill_Color.BackColor;
				}
				this.reDrawBitmap(this.graDrawPanel,10,10);
				this.RefreshBackground();	
				return;
			}
			else if(this.lastEdit is Line || this.lineSelectLine!=null)
			{
				Line line;
				if(this.lineSelectLine!=null)
				{
					line=this.lineSelectLine;
				}
				else
				{
					line=(Line)this.lastEdit;
				}
				line.LineColor=this.control.line_color.BackColor;
				this.reDrawBitmap(this.graDrawPanel,10,10);
				this.RefreshBackground();	
				return;
			}
			else if(this.lastEdit is DrawString || this.drawStringSelectDS!=null)
			{
				DrawString drawString;
				if(this.drawStringSelectDS!=null)
				{
					drawString=this.drawStringSelectDS;
				}
				else
				{
					drawString=(DrawString)this.lastEdit;
				}
				drawString.DSTextColor=this.control.DS_Color.BackColor;
				this.reDrawBitmap(this.graDrawPanel,10,10);
				this.RefreshBackground();	
				return;
			}
		}
		/// <summary>
		/// ��Ӧ�ڵ�Ľӿ�
		/// </summary>
		public void Attribute()
		{
			if(this.lastEdit is Node)
			{
				if(this.b_IsDownMouseRight)
				{
					if(this.lastEdit.Equals(this.objPrepareDel))
					{
						flowAttribute fAttr=new flowAttribute((Node)this.lastEdit,this.control);
						//					fAttr.Left=((Node)this.lastEdit).X;
						//					fAttr.Top=((Node)this.lastEdit).Y;
						fAttr.ShowDialog();
					}
				}
				else if(!this.b_IsDownMouseRight)
				{
					flowAttribute fAttr=new flowAttribute((Node)this.lastEdit,this.control);
					//					fAttr.Left=((Node)this.lastEdit).X;
					//					fAttr.Top=((Node)this.lastEdit).Y;
					fAttr.ShowDialog();
				}
			}
			this.b_IsDownMouseRight=false;
		}

		#endregion

		#region ���Զ����ڵ�
		/// <summary>
		/// �Ƿ��ߵ�ĳһ�˵� ��ĳ�ڵ�����ӵ���ϣ��Զ�����
		/// </summary>
		public void isLineRoundNode(int x,int y,Line line,int LineNodeIndex)//�Ƿ��ߵ�ĳһ�˵� ��ĳ�ڵ�����ӵ���ϣ��Զ�����
		{
			for(int i=0;i<this.arrNodeList.Count;i++)
			{
				Node node=((Node)this.arrNodeList[i]);
				int lineX=0,lineY=0;
				switch(LineNodeIndex)
				{	
					case 0:
					{
						lineX=this.iLineFirstNodeX;
						lineY=this.iLineFirstNodeY;
						break;
					}
					case 1:
					{
						lineX=this.iLineSecondNodeX;
						lineY=this.iLineSecondNodeY;
						break; 
					}
					case 3:
					{
						lineX=this.iLine4thNodeX;
						lineY=this.iLine4thNodeY;
						break;
					}
				}
				if(node.X+node.Width/2-10<lineX && lineX<node.X+node.Width/2+10 && node.Y-10<lineY && lineY<node.Y+10)//�����ڵ㱱�����
				{
					int iMoveWidth,iMoveHeight;
					iMoveWidth=node.X+node.Width/2-lineX;
					iMoveHeight=node.Y-lineY;
					if(iMoveWidth!=0 || iMoveHeight!=0)
					{
						this.iMouseDownX=x;
						this.iMouseDownY=y;
					}
					if(LineNodeIndex==0)
					{
						if(this.JudgeConnect(node,line,0))
						{
							this.iLineFirstNodeX=node.X+node.Width/2;
							this.iLineFirstNodeY=node.Y;
							if(line.ObjectType==Line.DrawObjectType.DrawFoldLine)
							{
								if(!line.Modality)
								{
									this.iLineSecondNodeY=node.Y;
									this.selectPoint.LinePoint[1].Y=node.Y;
								}
								else
								{
									this.iLineSecondNodeX=node.X+node.Width/2;
									this.selectPoint.LinePoint[1].X=node.X+node.Width/2;
								}
							}
							this.selectPoint.LinePoint[0].X=node.X+node.Width/2;
							this.selectPoint.LinePoint[0].Y=node.Y;
							line.FirNodeInterfaceIndex=0;
							line.FirstNode=node;
							node.ConnectOutCount++;
							this.iAutoConnectLinePointIndex=0;
						}
						else
						{
							this.AlertConLineLocation(line,LineNodeIndex);
						}
					}
					else if(LineNodeIndex==1)
					{
						if(this.JudgeConnect(node,line,1))
						{
							this.iLineSecondNodeX=node.X+node.Width/2;
							this.iLineSecondNodeY=node.Y;
							this.selectPoint.LinePoint[1].X=node.X+node.Width/2;
							this.selectPoint.LinePoint[1].Y=node.Y;
							line.SecNodeInterfaceIndex=0;
							line.SecondNode=node;
							node.ConnectInCount++;
							this.iAutoConnectLinePointIndex=1;
						}
						else
						{
							this.AlertConLineLocation(line,LineNodeIndex);
						}
					}
					else if(LineNodeIndex==3)
					{
						if(this.JudgeConnect(node,line,3))
						{
							this.iLine4thNodeX=node.X+node.Width/2;
							this.iLine4thNodeY=node.Y;
							if(!line.Modality)
							{
								this.iLine3thNodeY=node.Y;
								this.selectPoint.LinePoint[2].Y=node.Y;
							}
							else
							{
								this.iLine3thNodeX=node.X+node.Width/2;
								this.selectPoint.LinePoint[2].X=node.X+node.Width/2;
							}
							this.selectPoint.LinePoint[3].X=node.X+node.Width/2;
							this.selectPoint.LinePoint[3].Y=node.Y;

							line.SecNodeInterfaceIndex=0;
							line.SecondNode=node;
							node.ConnectInCount++;
							this.iAutoConnectLinePointIndex=3;
						}
						else
						{
							this.AlertConLineLocation(line,LineNodeIndex);
						}
					}
					break;
				}
				else if(node.X-10<lineX && lineX<node.X+10 && node.Y+node.Height/2-10<lineY && lineY<node.Y+node.Height/2+10)//�����ڵ��������
				{
					int iMoveWidth,iMoveHeight;
					iMoveWidth=node.X-lineX;
					iMoveHeight=node.Y+node.Height/2-lineY;
					if(iMoveWidth!=0 || iMoveHeight!=0)
					{
						this.iMouseDownX=x;
						this.iMouseDownY=y;
					}
					if(LineNodeIndex==0)
					{
						if(this.JudgeConnect(node,line,0))
						{
							this.iLineFirstNodeX=node.X;
							this.iLineFirstNodeY=node.Y+node.Height/2;
							if(line.ObjectType==Line.DrawObjectType.DrawFoldLine)
							{
								if(!line.Modality)
								{
									this.iLineSecondNodeY=node.Y+node.Height/2;
									this.selectPoint.LinePoint[1].Y=node.Y+node.Height/2;
								}
								else
								{
									this.iLineSecondNodeX=node.X;
									this.selectPoint.LinePoint[1].X=node.X;
								}
							}
							this.selectPoint.LinePoint[0].X=node.X;
							this.selectPoint.LinePoint[0].Y=node.Y+node.Height/2;
							line.FirNodeInterfaceIndex=1;
							line.FirstNode=node;
							node.ConnectOutCount++;
							this.iAutoConnectLinePointIndex=0;
						}
						else
						{
							this.AlertConLineLocation(line,LineNodeIndex);
						}
					}
					else if(LineNodeIndex==1)
					{
						if(this.JudgeConnect(node,line,1))
						{
							this.iLineSecondNodeX=node.X;
							this.iLineSecondNodeY=node.Y+node.Height/2;
							this.selectPoint.LinePoint[1].X=node.X;
							this.selectPoint.LinePoint[1].Y=node.Y+node.Height/2;
							line.SecNodeInterfaceIndex=1;
							line.SecondNode=node;
							node.ConnectInCount++;
							this.iAutoConnectLinePointIndex=1;
						}
						else
						{
							this.AlertConLineLocation(line,LineNodeIndex);
						}
					}
					else if(LineNodeIndex==3)
					{
						if(this.JudgeConnect(node,line,3))
						{
							this.iLine4thNodeX=node.X;
							this.iLine4thNodeY=node.Y+node.Height/2;
							if(!line.Modality)
							{
								this.iLine3thNodeY=node.Y+node.Height/2;
								this.selectPoint.LinePoint[2].Y=node.Y+node.Height/2;
							}
							else
							{
								this.iLine3thNodeX=node.X;
								this.selectPoint.LinePoint[2].X=node.X;
							}
							this.selectPoint.LinePoint[3].X=node.X;
							this.selectPoint.LinePoint[3].Y=node.Y+node.Height/2;

							line.SecNodeInterfaceIndex=1;
							line.SecondNode=node;
							node.ConnectInCount++;
							this.iAutoConnectLinePointIndex=3;
						}
						else
						{
							this.AlertConLineLocation(line,LineNodeIndex);
						}
					}
					break;
				}
				else if(node.X+node.Width-10<lineX && lineX<node.X+node.Width+10 && node.Y+node.Height/2-10<lineY && lineY<node.Y+node.Height/2+10)//�����ڵ㶫�����
				{
					int iMoveWidth,iMoveHeight;
					iMoveWidth=node.X+node.Width-lineX;
					iMoveHeight=node.Y+node.Height/2-lineY;
					if(iMoveWidth!=0 || iMoveHeight!=0)
					{
						this.iMouseDownX=x;
						this.iMouseDownY=y;
					}
					if(LineNodeIndex==0)
					{
						if(this.JudgeConnect(node,line,0))
						{
							this.iLineFirstNodeX=node.X+node.Width;
							this.iLineFirstNodeY=node.Y+node.Height/2;
							if(line.ObjectType==Line.DrawObjectType.DrawFoldLine)
							{
								if(!line.Modality)
								{
									this.iLineSecondNodeY=node.Y+node.Height/2;
									this.selectPoint.LinePoint[1].Y=node.Y+node.Height/2;
								}
								else
								{
									this.iLineSecondNodeX=node.X+node.Width;
									this.selectPoint.LinePoint[1].X=node.X+node.Width;
								}
							}
							this.selectPoint.LinePoint[0].X=node.X+node.Width;
							this.selectPoint.LinePoint[0].Y=node.Y+node.Height/2;
							line.FirNodeInterfaceIndex=2;
							line.FirstNode=node;
							node.ConnectOutCount++;
							this.iAutoConnectLinePointIndex=0;
						}
						else
						{
							this.AlertConLineLocation(line,LineNodeIndex);
						}
					}
					else if(LineNodeIndex==1)
					{
						if(this.JudgeConnect(node,line,1))
						{
							this.iLineSecondNodeX=node.X+node.Width;
							this.iLineSecondNodeY=node.Y+node.Height/2;
							this.selectPoint.LinePoint[1].X=node.X+node.Width;
							this.selectPoint.LinePoint[1].Y=node.Y+node.Height/2;
							line.SecNodeInterfaceIndex=2;
							line.SecondNode=node;
							node.ConnectInCount++;
							this.iAutoConnectLinePointIndex=1;
						}
						else
						{
							this.AlertConLineLocation(line,LineNodeIndex);
						}
					}
					else if(LineNodeIndex==3)
					{
						if(this.JudgeConnect(node,line,3))
						{
							this.iLine4thNodeX=node.X+node.Width;
							this.iLine4thNodeY=node.Y+node.Height/2;
							if(!line.Modality)
							{
								this.iLine3thNodeY=node.Y+node.Height/2;
								this.selectPoint.LinePoint[2].Y=node.Y+node.Height/2;
							}
							else
							{
								this.iLine3thNodeX=node.X+node.Width;
								this.selectPoint.LinePoint[2].X=node.X+node.Width;
							}
							this.selectPoint.LinePoint[3].X=node.X+node.Width;
							this.selectPoint.LinePoint[3].Y=node.Y+node.Height/2;

							line.SecNodeInterfaceIndex=2;
							line.SecondNode=node;
							node.ConnectInCount++;
							this.iAutoConnectLinePointIndex=3;
						}
						else
						{
							this.AlertConLineLocation(line,LineNodeIndex);
						}
					}
					break;
				}
				else if(node.X+node.Width/2-10<lineX && lineX<node.X+node.Width/2+10 && node.Y+node.Height-10<lineY && lineY<node.Y+node.Height+10)//�����ڵ��������
				{
					int iMoveWidth,iMoveHeight;
					iMoveWidth=node.X+node.Width/2-lineX;
					iMoveHeight=node.Y+node.Height-lineY;
					if(iMoveWidth!=0 || iMoveHeight!=0)
					{
						this.iMouseDownX=x;
						this.iMouseDownY=y;
					}
					if(LineNodeIndex==0)
					{
						if(this.JudgeConnect(node,line,0))
						{
							this.iLineFirstNodeX=node.X+node.Width/2;
							this.iLineFirstNodeY=node.Y+node.Height;
							if(line.ObjectType==Line.DrawObjectType.DrawFoldLine)
							{
								if(!line.Modality)
								{
									this.iLineSecondNodeY=node.Y+node.Height;
									this.selectPoint.LinePoint[1].Y=node.Y+node.Height;
								}
								else
								{
									this.iLineSecondNodeX=node.X+node.Width/2;
									this.selectPoint.LinePoint[1].X=node.X+node.Width/2;
								}
							}
							this.selectPoint.LinePoint[0].X=node.X+node.Width/2;
							this.selectPoint.LinePoint[0].Y=node.Y+node.Height;
							line.FirNodeInterfaceIndex=3;
							line.FirstNode=node;
							node.ConnectOutCount++;
							this.iAutoConnectLinePointIndex=0;
						}
						else
						{
							this.AlertConLineLocation(line,LineNodeIndex);
						}
					}
					else if(LineNodeIndex==1)
					{
						if(this.JudgeConnect(node,line,1))
						{
							this.iLineSecondNodeX=node.X+node.Width/2;
							this.iLineSecondNodeY=node.Y+node.Height;
							this.selectPoint.LinePoint[1].X=node.X+node.Width/2;
							this.selectPoint.LinePoint[1].Y=node.Y+node.Height;
							line.SecNodeInterfaceIndex=3;
							line.SecondNode=node;
							node.ConnectInCount++;
							this.iAutoConnectLinePointIndex=1;
						}
						else
						{
							this.AlertConLineLocation(line,LineNodeIndex);
						}
					}
					else if(LineNodeIndex==3)
					{
						if(this.JudgeConnect(node,line,3))
						{
							this.iLine4thNodeX=node.X+node.Width/2;
							this.iLine4thNodeY=node.Y+node.Height;
							if(!line.Modality)
							{
								this.iLine3thNodeY=node.Y+node.Height;
								this.selectPoint.LinePoint[2].Y=node.Y+node.Height;
							}
							else
							{
								this.iLine3thNodeX=node.X+node.Width/2;
								this.selectPoint.LinePoint[2].X=node.X+node.Width/2;					
							}
							this.selectPoint.LinePoint[3].X=node.X+node.Width/2;
							this.selectPoint.LinePoint[3].Y=node.Y+node.Height;
							line.SecNodeInterfaceIndex=3;
							line.SecondNode=node;
							node.ConnectInCount++;
							this.iAutoConnectLinePointIndex=3;
						}
						else
						{
							this.AlertConLineLocation(line,LineNodeIndex);
						}
					}
					break;
				}
				else
				{
					if(LineNodeIndex==0)
					{
						if(line.FirstNode!=null)
						{
							line.FirstNode.ConnectOutCount--;
							line.FirstNode=null;
							line.FirNodeInterfaceIndex=9;
						}
					}
					else if(LineNodeIndex==1)
					{
						if(line.SecondNode!=null)
						{
							line.SecondNode.ConnectInCount--;
							line.SecondNode=null;
							line.SecNodeInterfaceIndex=9;
						}
					}
					else if(LineNodeIndex==3)
					{
						if(line.SecondNode!=null)
						{
							line.SecondNode.ConnectInCount--;
							line.SecondNode=null;
							line.SecNodeInterfaceIndex=9;
						}
					}
				}
			}
		}


		public void MinLineMoveRoundNode(Line line,int LineNodeIndex)
		{
			for(int i=0;i<this.arrNodeList.Count;i++)
			{
				Node node=((Node)this.arrNodeList[i]);
				int lineX=0,lineY=0;
				switch(LineNodeIndex)
				{	
					case 0:
					{
						lineX=line.X0;
						lineY=line.Y0;
						break;
					}
					case 1:
					{
						lineX=line.X1;
						lineY=line.Y1;
						break; 
					}
					case 3:
					{
						lineX=line.X3;
						lineY=line.Y3;
						break;
					}
				}				
				if(node.X+node.Width/2-10<lineX && lineX<node.X+node.Width/2+10 && node.Y-10<lineY && lineY<node.Y+10)//�����ڵ㱱�����
				{
					if(LineNodeIndex==0)
					{
						line.X0=node.X+node.Width/2;
						line.Y0=node.Y;
						line.FirNodeInterfaceIndex=0;
						line.FirstNode=node;
						node.ConnectOutCount++;
						this.iAutoConnectLinePointIndex=0;
						return;
					}
					else if(LineNodeIndex==1)
					{
						line.X1=node.X+node.Width/2;
						line.Y1=node.Y;
						line.FirNodeInterfaceIndex=1;
						line.SecondNode=node;
						node.ConnectInCount++;
						this.iAutoConnectLinePointIndex=1;
						return;
					}
					else if(LineNodeIndex==3)
					{
						line.X3=node.X+node.Width/2;
						line.Y3=node.Y;
						line.FirNodeInterfaceIndex=0;
						line.SecondNode=node;
						node.ConnectInCount++;
						this.iAutoConnectLinePointIndex=3;
						return;
					}
				}
				else if(node.X-10<lineX && lineX<node.X+10 && node.Y+node.Height/2-10<lineY && lineY<node.Y+node.Height/2+10)//�����ڵ��������
				{
					if(LineNodeIndex==0)
					{
						line.X0=node.X;
						line.Y0=node.Y+node.Height/2;
						line.FirNodeInterfaceIndex=1;
						line.FirstNode=node;
						node.ConnectOutCount++;
						this.iAutoConnectLinePointIndex=0;
						return;
					}
					else if(LineNodeIndex==1)
					{
						line.X1=node.X;
						line.Y1=node.Y+node.Height/2;
						line.FirNodeInterfaceIndex=1;
						line.SecondNode=node;
						node.ConnectInCount++;
						this.iAutoConnectLinePointIndex=1;
						return;
					}
					else if(LineNodeIndex==3)
					{
						line.X3=node.X;
						line.Y3=node.Y+node.Height/2;
						line.FirNodeInterfaceIndex=1;
						line.SecondNode=node;
						node.ConnectInCount++;
						this.iAutoConnectLinePointIndex=3;
						return;
					}				
				}
				else if(node.X+node.Width-10<lineX && lineX<node.X+node.Width+10 && node.Y+node.Height/2-10<lineY && lineY<node.Y+node.Height/2+10)//�����ڵ㶫�����
				{
					if(LineNodeIndex==0)
					{
						line.X0=node.X+node.Width;
						line.Y0=node.Y+node.Height/2;
						line.FirNodeInterfaceIndex=2;
						line.FirstNode=node;
						node.ConnectOutCount++;
						this.iAutoConnectLinePointIndex=0;
						return;
					}
					else if(LineNodeIndex==1)
					{
						line.X1=node.X+node.Width;
						line.Y1=node.Y+node.Height/2;
						line.FirNodeInterfaceIndex=2;
						line.SecondNode=node;
						node.ConnectInCount++;
						this.iAutoConnectLinePointIndex=1;
						return;
					}
					else if(LineNodeIndex==3)
					{
						line.X3=node.X+node.Width;
						line.Y3=node.Y+node.Height/2;
						line.FirNodeInterfaceIndex=2;
						line.SecondNode=node;
						node.ConnectInCount++;
						this.iAutoConnectLinePointIndex=3;
						return;
					}
				}
				else if(node.X+node.Width/2-10<lineX && lineX<node.X+node.Width/2+10 && node.Y+node.Height-10<lineY && lineY<node.Y+node.Height+10)//�����ڵ��������
				{
					if(LineNodeIndex==0)
					{
						line.X0=node.X+node.Width/2;
						line.Y0=node.Y+node.Height;
						line.FirNodeInterfaceIndex=3;
						line.FirstNode=node;
						node.ConnectOutCount++;
						this.iAutoConnectLinePointIndex=0;
						return;
					}
					else if(LineNodeIndex==1)
					{
						line.X1=node.X+node.Width/2;
						line.Y1=node.Y+node.Height;
						line.FirNodeInterfaceIndex=3;
						line.SecondNode=node;
						node.ConnectInCount++;
						this.iAutoConnectLinePointIndex=1;
						return;
					}
					else if(LineNodeIndex==3)
					{
						line.X3=node.X+node.Width/2;
						line.Y3=node.Y+node.Height;
						line.FirNodeInterfaceIndex=3;
						line.SecondNode=node;
						node.ConnectInCount++;
						this.iAutoConnectLinePointIndex=3;
						return;
					}
				}
			}
			if(line.ObjectType==Line.DrawObjectType.DrawBeeLine)
			{
				this.selectPoint.SetLinePoint(line.X0,line.Y0,line.X1,line.Y1);
			}
			else
			{
				this.selectPoint.SetFlodLinePoint(line);
			}
		}
		/// <summary>
		/// �����߻��ڽڵ��ϵ�ʱ����Զ�����
		/// </summary>
		/// <param name="x">���ĺ�����</param>
		/// <param name="y">����������</param>
		/// <param name="LinePointIndex">�ߵĶ˵��־</param>
		public void AutoConnectNode(int x,int y,int LinePointIndex)//�����߻��ڽڵ��ϵ�ʱ����Զ�����
		{
			if(LinePointIndex==1 || LinePointIndex==3)//�ߵĽ���������ж�����
			{
				for(int i=this.arrNodeList.Count;i>0;i--)//���� ��ѯ  ��󻭵Ľڵ�  λ�������� 
				{
					Node node=((Node)this.arrNodeList[i-1]);
					if(node.X<x && x<node.X+node.Width && node.Y<y && y<node.Y+node.Height)
					{
						if(this.JudgeConnect(node,this.lineThisLine,1))
						{
							this.lineThisLine.SecondNode=node;
							node.ConnectInCount++;
							if(this.lineThisLine.X0<node.X)
							{
								if(this.lineThisLine.Y0<node.Y)
								{
									this.lineThisLine.SecNodeInterfaceIndex=0;
									if(this.lineThisLine.ObjectType==Line.DrawObjectType.DrawBeeLine)
									{
										this.iLineSecondNodeX=node.X+node.Width/2;
										this.iLineSecondNodeY=node.Y;
									}
									else
									{
										this.iLine4thNodeX=node.X+node.Width/2;
										this.iLine4thNodeY=node.Y;
										this.iLine3thNodeY=node.Y;
									}
								}
								else if(this.lineThisLine.Y0>node.Y && this.lineThisLine.Y0<node.Y+node.Height)
								{
									this.lineThisLine.SecNodeInterfaceIndex=1;
									if(this.lineThisLine.ObjectType==Line.DrawObjectType.DrawBeeLine)
									{
										this.iLineSecondNodeX=node.X;
										this.iLineSecondNodeY=node.Y+node.Height/2;
									}
									else
									{
										this.iLine4thNodeX=node.X;
										this.iLine4thNodeY=node.Y+node.Height/2;
										this.iLine3thNodeY=node.Y+node.Height/2;
									}
								}
								else
								{
									this.lineThisLine.SecNodeInterfaceIndex=3;
									if(this.lineThisLine.ObjectType==Line.DrawObjectType.DrawBeeLine)
									{
										this.iLineSecondNodeX=node.X+node.Width/2;
										this.iLineSecondNodeY=node.Y+node.Height;
									}
									else
									{
										this.iLine4thNodeX=node.X+node.Width/2;
										this.iLine4thNodeY=node.Y+node.Height;
										this.iLine3thNodeY=node.Y+node.Height;										
									}
								}
							}
							else
							{
								if(this.lineThisLine.Y0<node.Y)
								{
									this.lineThisLine.SecNodeInterfaceIndex=0;
									if(this.lineThisLine.ObjectType==Line.DrawObjectType.DrawBeeLine)
									{
										this.iLineSecondNodeX=node.X+node.Width/2;
										this.iLineSecondNodeY=node.Y;
									}
									else
									{
										this.iLine4thNodeX=node.X+node.Width/2;
										this.iLine4thNodeY=node.Y;
										this.iLine3thNodeY=node.Y;	
									}
								}
								else if(this.lineThisLine.Y0>node.Y && this.lineThisLine.Y0<node.Y+node.Height)
								{
									this.lineThisLine.SecNodeInterfaceIndex=2;
									if(this.lineThisLine.ObjectType==Line.DrawObjectType.DrawBeeLine)
									{
										this.iLineSecondNodeX=node.X+node.Width;
										this.iLineSecondNodeY=node.Y+node.Height/2;
									}
									else
									{
										this.iLine4thNodeX=node.X+node.Width;
										this.iLine4thNodeY=node.Y+node.Height/2;
										this.iLine3thNodeY=node.Y+node.Height/2;										
									}
								}
								else
								{
									this.lineThisLine.SecNodeInterfaceIndex=3;
									if(this.lineThisLine.ObjectType==Line.DrawObjectType.DrawBeeLine)
									{
										this.iLineSecondNodeX=node.X+node.Width/2;
										this.iLineSecondNodeY=node.Y+node.Height;
									}
									else
									{
										this.iLine4thNodeX=node.X+node.Width/2;
										this.iLine4thNodeY=node.Y+node.Height;
										this.iLine3thNodeY=node.Y+node.Height;
									}
								}						
							}
						}
						return;
					}
				}	
			}
			else
			{
				for(int i=this.arrNodeList.Count;i>0;i--)//���� ��ѯ  ��󻭵Ľڵ�  λ�������� 
				{
					Node node=((Node)this.arrNodeList[i-1]);
					if(node.X<x && x<node.X+node.Width && node.Y<y && y<node.Y+node.Height)
					{
						if(this.JudgeConnect(node,this.lineThisLine,0))
						{
							if(this.lineThisLine.SecondNode==node)
							{
								return;
							}
							this.lineThisLine.FirstNode=node;
							node.ConnectOutCount++;
							if(this.iLineSecondNodeX<node.X+node.Width/2)
							{
								if(this.iLineSecondNodeY<node.Y)
								{
									this.lineThisLine.FirNodeInterfaceIndex=0;
									this.iLineFirstNodeX=node.X+node.Width/2;
									this.iLineFirstNodeY=node.Y;
									if(this.lineThisLine.ObjectType==Line.DrawObjectType.DrawFoldLine)
									{
										this.iLineSecondNodeY=node.Y;
									}
								}
								else if(this.iLineSecondNodeY>node.Y && this.iLineSecondNodeY<node.Y+node.Height)
								{
									this.lineThisLine.FirNodeInterfaceIndex=1;
									this.iLineFirstNodeX=node.X;
									this.iLineFirstNodeY=node.Y+node.Height/2;
									if(this.lineThisLine.ObjectType==Line.DrawObjectType.DrawFoldLine)
									{
										this.iLineSecondNodeY=node.Y+node.Height/2;
									}
								}
								else
								{
									this.lineThisLine.FirNodeInterfaceIndex=3;
									this.iLineFirstNodeX=node.X+node.Width/2;
									this.iLineFirstNodeY=node.Y+node.Height;
									if(this.lineThisLine.ObjectType==Line.DrawObjectType.DrawFoldLine)
									{
										this.iLineSecondNodeY=node.Y+node.Height;
									}
								}
							}
							else
							{
								if(this.iLineSecondNodeY<node.Y)
								{
									this.lineThisLine.FirNodeInterfaceIndex=0;
									this.iLineFirstNodeX=node.X+node.Width/2;
									this.iLineFirstNodeY=node.Y;
									if(this.lineThisLine.ObjectType==Line.DrawObjectType.DrawFoldLine)
									{
										this.iLineSecondNodeY=node.Y;
									}
								}
								else if(this.iLineSecondNodeY>node.Y && this.iLineSecondNodeY<node.Y+node.Height)
								{
									this.lineThisLine.FirNodeInterfaceIndex=2;
									this.iLineFirstNodeX=node.X+node.Width;
									this.iLineFirstNodeY=node.Y+node.Height/2;
									if(this.lineThisLine.ObjectType==Line.DrawObjectType.DrawFoldLine)
									{
										this.iLineSecondNodeY=node.Y+node.Height/2;
									}
								}
								else
								{
									this.lineThisLine.FirNodeInterfaceIndex=3;
									this.iLineFirstNodeX=node.X+node.Width/2;
									this.iLineFirstNodeY=node.Y+node.Height;
									if(this.lineThisLine.ObjectType==Line.DrawObjectType.DrawFoldLine)
									{
										this.iLineSecondNodeY=node.Y+node.Height;
									}
								}						
							}
						}
						return;
					}
				}
			}
		}

		/// <summary>
		/// �����ӵ������¶�λ
		/// </summary>
		/// <param name="line"></param>
		public void AlertConLineLocation(Line line,int iLinePointIndex)
		{
			if(line.FirstNode!=null && iLinePointIndex==0)
			{
				switch(line.FirNodeInterfaceIndex)
				{
					case 0: this.iLineFirstNodeX=line.FirstNode.X+line.FirstNode.Width/2;
						this.iLineFirstNodeY=line.FirstNode.Y; 
						this.selectPoint.LinePoint[0].X=this.iLineFirstNodeX;
						this.selectPoint.LinePoint[0].Y=this.iLineFirstNodeY;
						this.iAutoConnectLinePointIndex=0;
						if(line.ObjectType==Line.DrawObjectType.DrawFoldLine)
						{
							this.iLineSecondNodeY=line.FirstNode.Y;
							this.selectPoint.LinePoint[1].Y=line.FirstNode.Y;
						}	
						break;
					case 1: this.iLineFirstNodeX=line.FirstNode.X;
						this.iLineFirstNodeY=line.FirstNode.Y+line.FirstNode.Height/2; 
						this.selectPoint.LinePoint[0].X=this.iLineFirstNodeX;
						this.selectPoint.LinePoint[0].Y=this.iLineFirstNodeY;
						this.iAutoConnectLinePointIndex=0;
						if(line.ObjectType==Line.DrawObjectType.DrawFoldLine)
						{
							this.iLineSecondNodeY=line.FirstNode.Y+line.FirstNode.Height/2;
							this.selectPoint.LinePoint[1].Y=line.FirstNode.Y+line.FirstNode.Height/2;
						}						
						break;
					case 2: this.iLineFirstNodeX=line.FirstNode.X+line.FirstNode.Width; 
						this.iLineFirstNodeY=line.FirstNode.Y+line.FirstNode.Height/2; 
						this.selectPoint.LinePoint[0].X=this.iLineFirstNodeX;
						this.selectPoint.LinePoint[0].Y=this.iLineFirstNodeY;
						this.iAutoConnectLinePointIndex=0;
						if(line.ObjectType==Line.DrawObjectType.DrawFoldLine)
						{
							this.iLineSecondNodeY=line.FirstNode.Y+line.FirstNode.Height/2;
							this.selectPoint.LinePoint[1].Y=line.FirstNode.Y+line.FirstNode.Height/2;
						}
						break;
					case 3: this.iLineFirstNodeX=line.FirstNode.X+line.FirstNode.Width/2;
						this.iLineFirstNodeY=line.FirstNode.Y+line.FirstNode.Height;
						this.selectPoint.LinePoint[0].X=this.iLineFirstNodeX;
						this.selectPoint.LinePoint[0].Y=this.iLineFirstNodeY;
						this.iAutoConnectLinePointIndex=0;
						if(line.ObjectType==Line.DrawObjectType.DrawFoldLine)
						{
							this.iLineSecondNodeY=line.FirstNode.Y+line.FirstNode.Height;
							this.selectPoint.LinePoint[1].Y=line.FirstNode.Y+line.FirstNode.Height;
						}
						break;					
				}
			} 
			if(line.SecondNode!=null && iLinePointIndex==1)
			{
				if(line.ObjectType==Line.DrawObjectType.DrawBeeLine)
				{
					switch(line.SecNodeInterfaceIndex)
					{
						case 0: this.iLineSecondNodeX=line.SecondNode.X+line.SecondNode.Width/2;
							this.iLineSecondNodeY=line.SecondNode.Y; 
							this.selectPoint.LinePoint[1].X=this.iLineSecondNodeX;
							this.selectPoint.LinePoint[1].Y=this.iLineSecondNodeY;
							this.iAutoConnectLinePointIndex=1;
							break;
						case 1: this.iLineSecondNodeX=line.SecondNode.X; 
							this.iLineSecondNodeY=line.SecondNode.Y+line.SecondNode.Height/2; 
							this.selectPoint.LinePoint[1].X=this.iLineSecondNodeX;
							this.selectPoint.LinePoint[1].Y=this.iLineSecondNodeY;		
							this.iAutoConnectLinePointIndex=1;
							break;
						case 2: this.iLineSecondNodeX=line.SecondNode.X+line.SecondNode.Width;
							this.iLineSecondNodeY=line.SecondNode.Y+line.SecondNode.Height/2;
							this.selectPoint.LinePoint[1].X=this.iLineSecondNodeX;
							this.selectPoint.LinePoint[1].Y=this.iLineSecondNodeY;	
							this.iAutoConnectLinePointIndex=1;
							break;
						case 3: this.iLineSecondNodeX=line.SecondNode.X+line.SecondNode.Width/2;
							this.iLineSecondNodeY =line.SecondNode.Y+line.SecondNode.Height; 
							this.selectPoint.LinePoint[1].X=this.iLineSecondNodeX;
							this.selectPoint.LinePoint[1].Y=this.iLineSecondNodeY;
							this.iAutoConnectLinePointIndex=1;
							break;					
					}
				}
			}
			else if(line.SecondNode!=null && iLinePointIndex==3) 
			{
				switch(line.SecNodeInterfaceIndex)
				{
					case 0: this.iLine4thNodeX=line.SecondNode.X+line.SecondNode.Width/2;
						this.iLine4thNodeY=line.SecondNode.Y;
						this.selectPoint.LinePoint[3].X=this.iLine4thNodeX;
						this.selectPoint.LinePoint[3].Y=this.iLine4thNodeY;
						if(!line.Modality)
						{
							this.iLine3thNodeY=line.SecondNode.Y;
							this.selectPoint.LinePoint[2].Y=this.iLine3thNodeY;
						}
						else
						{
							this.iLine3thNodeX=line.SecondNode.X+line.SecondNode.Width/2;
							this.selectPoint.LinePoint[2].X=this.iLine3thNodeX;
						}
						this.iAutoConnectLinePointIndex=3;
						break;
					case 1: this.iLine4thNodeX=line.SecondNode.X; 
						this.iLine4thNodeY=line.SecondNode.Y+line.SecondNode.Height/2; 
						if(!line.Modality)
						{
							this.iLine3thNodeY=line.SecondNode.Y+line.SecondNode.Height/2; 
							this.selectPoint.LinePoint[2].Y=this.iLine3thNodeY; 
						}
						else
						{
							this.iLine3thNodeX=line.SecondNode.X;
							this.selectPoint.LinePoint[2].X=this.iLine3thNodeX;
						}
						this.selectPoint.LinePoint[3].X=this.iLine4thNodeX;
						this.selectPoint.LinePoint[3].Y=this.iLine4thNodeY;	
						this.iAutoConnectLinePointIndex=3;
						break;
					case 2: this.iLine4thNodeX=line.SecondNode.X+line.SecondNode.Width;
						this.iLine4thNodeY=line.SecondNode.Y+line.SecondNode.Height/2;
						if(!line.Modality)
						{
							this.iLine3thNodeY=line.SecondNode.Y+line.SecondNode.Height/2;
							this.selectPoint.LinePoint[2].Y=this.iLine3thNodeY;
						}
						else
						{
							this.iLine3thNodeX=line.SecondNode.X+line.SecondNode.Width;
							this.selectPoint.LinePoint[2].X=this.iLine3thNodeX;
						}
						this.selectPoint.LinePoint[3].X=this.iLine4thNodeX;
						this.selectPoint.LinePoint[3].Y=this.iLine4thNodeY;		

						this.iAutoConnectLinePointIndex=3;
						break;
					case 3: this.iLine4thNodeX=line.SecondNode.X+line.SecondNode.Width/2;
						this.iLine4thNodeY =line.SecondNode.Y+line.SecondNode.Height; 
						if(!line.Modality)
						{
							this.iLine3thNodeY=line.SecondNode.Y+line.SecondNode.Height; 
							this.selectPoint.LinePoint[2].Y=this.iLine3thNodeY;
						}
						else
						{
							this.iLine3thNodeX=line.SecondNode.X+line.SecondNode.Width/2;
							this.selectPoint.LinePoint[2].X=this.iLine3thNodeX;
						}
						this.selectPoint.LinePoint[3].X=this.iLine4thNodeX;
						this.selectPoint.LinePoint[3].Y=this.iLine4thNodeY;
						this.iAutoConnectLinePointIndex=3;
						break;
				}					
			}
		}
		/// <summary>
		/// �ж�����ڵ��Ƿ��������  LinePointIndex=1��ʾ���� ��LinePointIndex=0��ʾ�ӳ�
		/// </summary>
		/// <param name="node">�ڵ�</param>
		/// <param name="line">��</param>
		/// <param name="LinePointIndex">�ߵĶ˵���</param>
		/// <returns></returns>
		public bool JudgeConnect(Node node,Line line,int LinePointIndex) //�ж�����ڵ��Ƿ��������  LinePointIndex=1��ʾ���� ��LinePointIndex=0��ʾ�ӳ�
		{
			if(node.ObjectType==Node.DrawObjectType.DrawNodeBegin)//��ʼ
			{//�޽����
				if(LinePointIndex==1 || LinePointIndex==3)
				{
					if(this.iArr_ShowHelpTip[(int)Node.DrawObjectType.DrawNodeBegin]<1)
					{
						this.iArr_ShowHelpTip[(int)Node.DrawObjectType.DrawNodeBegin]++;
						this.control.ShowTip("��ʼ�ڵ�û��ǰ�����");
					}
					return false;
				}
				
				if(LinePointIndex==0)
				{
					return true;
				}
			}
			else if(node.ObjectType==Node.DrawObjectType.DrawNodeGeneral)//�ڵ�
			{//������
				return true;
			}
			else if(node.ObjectType==Node.DrawObjectType.DrawSpecificallyOperation)//�ض�����
			{//һ���޳�
				if(node.ConnectInCount>=1 && (LinePointIndex==1 || LinePointIndex==3))
				{
					if(this.iArr_ShowHelpTip[(int)Node.DrawObjectType.DrawSpecificallyOperation]<1)
					{
						this.iArr_ShowHelpTip[(int)Node.DrawObjectType.DrawSpecificallyOperation]++;
						this.control.ShowTip("�ض�����ֻ����һ��ǰ����㡣");
					}
					return false;
				}
				else if(node.ConnectInCount<1 && (LinePointIndex==1 || LinePointIndex==3))
				{
					return true;
				}

				if( LinePointIndex==0)
				{
					if(this.iArr_ShowHelpTip[10+(int)Node.DrawObjectType.DrawSpecificallyOperation]<1)
					{
						this.iArr_ShowHelpTip[10+(int)Node.DrawObjectType.DrawSpecificallyOperation]++;
						this.control.ShowTip("�ض�����û�к�̽�㡣");
					}
					return false;
				}
			}
			else if(node.ObjectType==Node.DrawObjectType.DrawGradation)//˳��
			{//һ��һ��
				if(node.ConnectInCount>=1 && (LinePointIndex==1 || LinePointIndex==3))
				{
					if(this.iArr_ShowHelpTip[(int)Node.DrawObjectType.DrawGradation]<1)
					{
						this.iArr_ShowHelpTip[(int)Node.DrawObjectType.DrawGradation]++;
						this.control.ShowTip("˳��ڵ�ֻ����һ��ǰ����㡣");
					}
					return false;
				}
				else if(node.ConnectInCount<1 && (LinePointIndex==1 || LinePointIndex==3))
				{
					return true;
				}

				if(node.ConnectOutCount>=1 && LinePointIndex==0)
				{
					if(this.iArr_ShowHelpTip[10+(int)Node.DrawObjectType.DrawGradation]<1)
					{
						this.iArr_ShowHelpTip[10+(int)Node.DrawObjectType.DrawGradation]++;
						this.control.ShowTip("˳��ڵ�ֻ����һ����̽�㡣");
					}
					return false;
				}
				else if(node.ConnectOutCount<1 && LinePointIndex==0)
				{
					return true;
				}
			}
			else if(node.ObjectType==Node.DrawObjectType.DrawSynchronization)//ͬ��
			{//����һ��
				if(node.ConnectInCount>=2 && (LinePointIndex==1 || LinePointIndex==3))
				{
					if(this.iArr_ShowHelpTip[(int)Node.DrawObjectType.DrawSynchronization]<1)
					{
						this.iArr_ShowHelpTip[(int)Node.DrawObjectType.DrawSynchronization]++;
						this.control.ShowTip("ͬ���ڵ�ֻ��������ǰ����㡣");
					}
					return false;
				}
				else if(node.ConnectInCount<2 && (LinePointIndex==1 || LinePointIndex==3))
				{
					return true;
				}
				
				if(node.ConnectOutCount<1 && LinePointIndex==0)
				{
					return true;
				}
				else if(node.ConnectOutCount>=1 && LinePointIndex==0)
				{
					if(this.iArr_ShowHelpTip[10+(int)Node.DrawObjectType.DrawSynchronization]<1)
					{
						this.iArr_ShowHelpTip[10+(int)Node.DrawObjectType.DrawSynchronization]++;
						this.control.ShowTip("ͬ���ڵ�ֻ����һ����̽�㡣");
					}
					return false;					
				}
			}
			else if(node.ObjectType==Node.DrawObjectType.DrawAsunder)//��֧
			{// һ�����
				if(node.ConnectInCount>=1 && (LinePointIndex==1 || LinePointIndex==3))
				{
					if(this.iArr_ShowHelpTip[(int)Node.DrawObjectType.DrawAsunder]<1)
					{
						this.iArr_ShowHelpTip[(int)Node.DrawObjectType.DrawAsunder]++;
						this.control.ShowTip("��֧�ڵ�ֻ����һ��ǰ����㡣");
					}
					return false;
				}
				else if(node.ConnectInCount<1 && (LinePointIndex==1 || LinePointIndex==3))
				{
					return true;
				}

				if(LinePointIndex==0)
				{
					return true;		
				}
			}
			else if(node.ObjectType==Node.DrawObjectType.DrawConverge)//���
			{// ���һ��
				if((LinePointIndex==1 || LinePointIndex==3))
				{
					return true;
				}

				if(node.ConnectOutCount>=1 && LinePointIndex==0)
				{
					if(this.iArr_ShowHelpTip[(int)Node.DrawObjectType.DrawConverge]<1)
					{
						this.iArr_ShowHelpTip[(int)Node.DrawObjectType.DrawConverge]++;
						this.control.ShowTip("��۽ڵ�ֻ����һ����̽�㡣");
					}
					return false;
				}
				else if(node.ConnectOutCount<1 && LinePointIndex==0)
				{
					return true;
				}

			}
			else if(node.ObjectType==Node.DrawObjectType.DrawGather)//��������
			{// ���һ��
				if((LinePointIndex==1 || LinePointIndex==3))
				{
					return true;
				}

				if(node.ConnectOutCount>=1 && LinePointIndex==0)
				{
					if(this.iArr_ShowHelpTip[(int)Node.DrawObjectType.DrawGather]<1)
					{
						this.iArr_ShowHelpTip[(int)Node.DrawObjectType.DrawGather]++;
						this.control.ShowTip("���ܽڵ�ֻ����һ����̽�㡣");
					}
					return false;
				}
				else if(node.ConnectOutCount<1 && LinePointIndex==0)
				{
					return true;
				}
			}
			else if(node.ObjectType==Node.DrawObjectType.DrawJudgement)//�ж�
			{// ������
				return true;
			}
			else if(node.ObjectType==Node.DrawObjectType.DrawDataNode)//����
			{//�޽����
				if(LinePointIndex==1 || LinePointIndex==3)
				{
					if(this.iArr_ShowHelpTip[(int)Node.DrawObjectType.DrawDataNode]<1)
					{
						this.iArr_ShowHelpTip[(int)Node.DrawObjectType.DrawDataNode]++;
						this.control.ShowTip("���ݽڵ�û�п��Ժ�̽�㡣");
					}
					return false;
				}
				
				if(LinePointIndex==0)
				{
					return true;
				}
			}
			else if(node.ObjectType==Node.DrawObjectType.DrawNodeEnd)//����
			{//�������
				if((LinePointIndex==1 || LinePointIndex==3))
				{
					return true;
				}

				if(LinePointIndex==0)
				{
					if(this.iArr_ShowHelpTip[(int)Node.DrawObjectType.DrawNodeEnd]<1)
					{
						this.iArr_ShowHelpTip[(int)Node.DrawObjectType.DrawNodeEnd]++;
						this.control.ShowTip("�����ڵ�û�п��Ժ�̽�㡣");
					}
					return false;
				}
			}
			else if(node.ObjectType==Node.DrawObjectType.DrawRectangle)//����
			{//������
				return true;
			}
			else if(node.ObjectType==Node.DrawObjectType.DrawEllipse)//��Բ��
			{//������
				return true;
			}
			return false;
		}

		#endregion

		#region ѡ�� Ԫ��
		/// <summary>
		/// ����ѡ���Ԫ�ع���
		/// </summary>
		/// <param name="x">����Ŀ�ʼ������</param>
		/// <param name="y">����Ŀ�ʼ������</param>
		/// <param name="width">����Ŀ��</param>
		/// <param name="height">����ĳ���</param>
		public void RegionSelect(int x,int y,int width,int height)
		{
			this.arrLineSelectList.Clear();
			this.arrNodeSelectList.Clear();
			this.arrDrawStringSelectList.Clear();
			this.arrLineNotSelectList.Clear();
			this.arrNodeNotSelectList.Clear();
			this.arrDrawStringNotSelectList.Clear();
			Line line;
			Node node;
			DrawString drawString;
			for(int i=0;i<this.arrLineList.Count;i++)
			{
				line=(Line)this.arrLineList[i];
				if(((x<line.X0 && line.X0<x+width)&&(y<line.Y0 &&  line.Y0<y+height))||((x<line.X1 && line.X1<x+width)&&(y<line.Y1 &&  line.Y1<y+height)))
				{
					this.arrLineSelectList.Add(line);
				}
				else
				{
					this.arrLineNotSelectList.Add(line);
				}
			}

			for(int i=0;i<this.arrNodeList.Count;i++)
			{
				node=(Node)this.arrNodeList[i];
				if(((x<node.X && node.X<x+width)&&(y<node.Y &&  node.Y<y+height))||((x<node.X+node.Width && node.X+node.Width<x+width)&&(y<node.Y &&  node.Y<y+height))||((x<node.X && node.X<x+width)&&(y<node.Y+node.Height &&  node.Y+node.Height<y+height))||((x<node.X+node.Width && node.X+node.Width<x+width)&&(y<node.Y+node.Height&&  node.Y+node.Height<y+height)))
				{
					this.arrNodeSelectList.Add(node);
				}
				else
				{
					this.arrNodeNotSelectList.Add(node);
				}
			}

			for(int i=0;i<this.arrDrawStringList.Count;i++)
			{
				drawString=(DrawString)this.arrDrawStringList[i];
				if(((x<drawString.X && drawString.X<x+width)&&(y<drawString.Y &&  drawString.Y<y+height))||((x<drawString.X+drawString.Width && drawString.X+drawString.Width<x+width)&&(y<drawString.Y &&  drawString.Y<y+height))||((x<drawString.X && drawString.X<x+width)&&(y<drawString.Y+drawString.Height &&  drawString.Y+drawString.Height<y+height))||((x<drawString.X+drawString.Width && drawString.X+drawString.Width<x+width)&&(y<drawString.Y+drawString.Height&&  drawString.Y+drawString.Height<y+height)))
				{
					this.arrDrawStringSelectList.Add(drawString);
				}
				else
				{
					this.arrDrawStringNotSelectList.Add(drawString);
				}
			}
		}
		/// <summary>
		/// ȫ��ѡ��
		/// </summary>
		public void SelectAll()
		{
			if(this.lineSelectLine!=null || this.lineThisLine!=null || this.nodeSelectNode!=null || this.nodeThisNode!=null || this.drawStringThisDS!=null || this.drawStringSelectDS!=null)
			{
				return;
			}
			if(this.TBDrawStringContent!=null || this.TBLineContent!=null || this.TBnodeContent !=null)
			{
				this.InputContentEnter();
			}
			this.bSelectAll=true;
			this.lastEdit=null;
			this.AttriShow();
			this.control.Invalidate();
		}

		#endregion

		#region ɾ��Ԫ��
		/// <summary>
		/// ɾ������Ԫ��
		/// </summary>
		public void DeleteAll()
		{
			if(this.lineThisLine!=null || this.nodeThisNode!=null || this.drawStringThisDS!=null || this.lineSelectLine!=null || this.nodeSelectNode!=null || this.drawStringSelectDS!=null || this.lineTempLine!=null || this.nodeTempNode!=null || this.TBDrawStringContent.Visible!=false || this.TBnodeContent.Visible!=false)
			{
				return;
			}
			this.arrLineList.Clear();
			this.arrNodeList.Clear();
			this.arrDrawStringList.Clear();
			this.arrLineSelectList.Clear();
			this.arrNodeSelectList.Clear();
			this.arrDrawStringSelectList.Clear();
			this.arrLineNotSelectList.Clear();
			this.arrNodeNotSelectList.Clear();
			this.arrDrawStringNotSelectList.Clear();
			this.bSelectAllReday=false;
			this.bStartFlag=false;
			this.bEndFlag=false;
			this.i_GeneralCount=0;
			this.lastEdit=null;
			this.AttriShow();
			ReflashNodeIn_OutNodeID();
			GetTableList();
			this.reDrawBitmap(this.graDrawPanel,10,10);
			this.RefreshBackground();
			this.control.Invalidate();
			GC.Collect();
		}
		/// <summary>
		/// ɾ������ѡ���Ԫ��
		/// </summary>
		public void DeleteSelectRegion()
		{
			Line line;
			Node node;
			DrawString drawString;
			for(int i=0;i<this.arrLineSelectList.Count;i++)
			{
				line=((Line)this.arrLineSelectList[i]);
				this.arrLineList.Remove(line);
				this.FlashArrayList(0);

			}

			for(int i=0;i<this.arrNodeSelectList.Count;i++)
			{
				node=(Node)this.arrNodeSelectList[i];
				if(node.ObjectType==Node.DrawObjectType.DrawNodeBegin)
				{
					this.bStartFlag=false;
				}
				else if(node.ObjectType==Node.DrawObjectType.DrawNodeEnd)
				{
					this.bEndFlag=false;
				}
				else if(node.ObjectType==Node.DrawObjectType.DrawNodeGeneral)
				{
					this.i_GeneralCount--;
				}
				this.DelLineNode(node);	
				this.arrNodeList.Remove(node);
				this.FlashArrayList(1);
			}
				
			for(int i=0;i<this.arrDrawStringSelectList.Count;i++)
			{
				drawString=(DrawString)this.arrDrawStringSelectList[i];
				this.arrDrawStringList.Remove(drawString);
				this.FlashArrayList(2);
			}
			this.arrLineSelectList.Clear();
			this.arrNodeSelectList.Clear();
			this.arrDrawStringSelectList.Clear();
			this.arrLineNotSelectList.Clear();
			this.arrNodeNotSelectList.Clear();
			this.arrDrawStringNotSelectList.Clear();
			this.lastEdit=null;
			this.AttriShow();
			ReflashNodeIn_OutNodeID();
			GetTableList();
			this.reDrawBitmap(this.graDrawPanel,10,10);
			this.RefreshBackground();
			this.control.Invalidate();
		}
		/// <summary>
		/// ɾ��ѡ����Ԫ��
		/// </summary>
		public void DeleteSelectElement()//ɾ��ѡ�е�Ԫ��
		{
			if(this.lineThisLine!=null || this.nodeThisNode!=null || this.drawStringThisDS!=null || this.lineSelectLine!=null || this.nodeSelectNode!=null || this.drawStringSelectDS!=null || this.lineTempLine!=null || this.nodeTempNode!=null || this.drawStringTempDS!=null)
			{
				return;
			}
			if(this.TBDrawStringContent.Visible==true)
			{
				this.TBDrawStringContent.Text="";
				return;
			}
			else if(this.TBLineContent.Visible==true)
			{
				this.TBLineContent.Text="";
				return;
			}
			else if(this.TBnodeContent.Visible==true)
			{	
				this.TBnodeContent.Text="";
				return;
			}
			Object objectEle=this.lastEdit;
			if(this.bSelectAllReday)
			{
				this.DeleteAll();
			}
			else if(this.bSelectRectangleReday)
			{
				this.DeleteSelectRegion();
			}
			else 
			{
				if(objectEle is Line)
				{
					this.arrLineList.Remove((Line)objectEle);
					this.FlashArrayList(0);
								ReflashNodeIn_OutNodeID();
					GetTableList();
				}
				else if(objectEle is Node)
				{
					if(this.TBnodeContent.Visible==true)
					{
						return;
					}
					this.arrNodeList.Remove((Node)objectEle);
					if(((Node)objectEle).ObjectType==Node.DrawObjectType.DrawNodeBegin)
					{
						this.bStartFlag=false;
					}
					else if(((Node)objectEle).ObjectType==Node.DrawObjectType.DrawNodeEnd)
					{
						this.bEndFlag=false;
					}			
					else if(((Node)objectEle).ObjectType==Node.DrawObjectType.DrawNodeGeneral)
					{
						this.i_GeneralCount--;
					}
					this.FlashArrayList(1);
					this.DelLineNode((Node)objectEle);
								ReflashNodeIn_OutNodeID();
					GetTableList();
				}
				else if(objectEle is DrawString)
				{
					if(this.TBDrawStringContent.Visible==true)
					{
						return;
					}
					this.arrDrawStringList.Remove((DrawString)objectEle);
					this.FlashArrayList(2);				
				}
				this.objPrepareDel=null;
				this.lastEdit=null;
				this.AttriShow();
				this.reDrawBitmap(this.graDrawPanel,10,10);
				this.ReflashNodeIn_OutNodeID();
				this.RefreshBackground();
				this.control.Invalidate();
			}
		}

		/// <summary>
		///������ӵ�ĳһ�ڵ��ϵ��� ������
		/// </summary>
		/// <param name="node">�ڵ�</param>
		public void DelLineNode(Node node)
		{
			Line line;
			for(int i=0;i<this.arrLineList.Count;i++)
			{
				line=(Line)this.arrLineList[i];
				if(line.FirstNode==node)
				{
					line.FirstNode.ConnectOutCount--;
					line.FirstNode=null;
					line.FirNodeInterfaceIndex=9;
				}
				if(line.SecondNode==node)
				{
					line.SecondNode.ConnectInCount--;
					line.SecondNode=null;
					line.SecNodeInterfaceIndex=9;
				}
			}
		}

		#endregion

		#region ����
		/// <summary>
		/// �ߣ��ڵ㣬д�ְ��ϵ�������������Ĵ���
		/// </summary>
		public void InputContentEnter()
		{
			if(this.TBDrawStringContent.Visible==true)
			{
				if(this.lastEdit is DrawString)
				{
					((DrawString)this.lastEdit).Content=this.TBDrawStringContent.Text;
					this.TBDrawStringContent.Visible=false;
					this.reDrawBitmap(this.graDrawPanel,10,10);
					this.RefreshBackground();
					//					this.control.Invalidate();
				}
			}
			else if(this.TBnodeContent.Visible==true)
			{
				if(this.lastEdit is Node)
				{
					((Node)this.lastEdit).NodeText=this.TBnodeContent.Text;
					this.TBnodeContent.Visible=false;
					this.reDrawBitmap(this.graDrawPanel,10,10);
					this.RefreshBackground();
					//					this.control.Invalidate();
				}				
			}
			else if(this.TBLineContent.Visible==true)
			{
				if(this.lastEdit is Line)
				{
					((Line)this.lastEdit).Content=this.TBLineContent.Text;
					this.TBLineContent.Visible=false;
					this.reDrawBitmap(this.graDrawPanel,10,10);
					this.RefreshBackground();
				}				
			}
		}		

		/// <summary>
		/// ����ѡ���ʱ��ֻѡ��һ��Ԫ��
		/// </summary>
		public void ifSelectAngle()
		{
			if(this.arrLineSelectList.Count+this.arrNodeSelectList.Count+this.arrDrawStringSelectList.Count==1)
			{
				if(this.arrLineSelectList.Count==1)
				{
					this.lastEdit=(Line)this.arrLineSelectList[0];
					this.AttriShow();
					//					this.lineSelectLine=(Line)this.arrLineSelectList[0];
					return;
				}
				else if(this.arrNodeSelectList.Count==1)
				{
					this.lastEdit=(Node)this.arrNodeSelectList[0];
					this.AttriShow();
					//					this.nodeSelectNode=(Node)this.arrNodeSelectList[0];
					return;
				}
				else if(this.arrDrawStringSelectList.Count==1)
				{
					this.lastEdit=(DrawString)this.arrDrawStringSelectList[0];
					this.AttriShow();
					//					this.drawStringSelectDS=(DrawString)this.arrDrawStringSelectList[0];
					return;
				}
			}
			else		
			{
				this.lastEdit=null;
				this.AttriShow();
			}
		}

		/// <summary>
		/// ˢ�����нڵ��ڵ����������ڵ��ID
		/// </summary>
		public void ReflashNodeIn_OutNodeID()
		{
			for(int i=0;i<this.arrNodeList.Count;i++)
			{
				Node node=(Node)this.arrNodeList[i];
				node.InFlowNodeID.Clear();
				node.OutFlowNodeID.Clear();
			}
			for(int i=0;i<this.arrLineList.Count;i++)
			{
				Line line=(Line)this.arrLineList[i];
				if(line.FirstNode!=null && line.SecondNode!=null)
				{
					line.FirstNode.OutFlowNodeID.Add(line.SecondNode.NodeListIndex);
					line.SecondNode.InFlowNodeID.Add(line.FirstNode.NodeListIndex);
				}
			}
		}

		/// <summary>
		/// ��¼���ڱ༭��Ԫ��
		/// </summary>
		public void MemThisObject()
		{
			if(this.lineThisLine!=null)
			{
				this.obj_SeriesDrawEle=this.lineThisLine;
			}
			else if(this.nodeThisNode!=null)
			{
				this.obj_SeriesDrawEle=this.nodeThisNode;
			}
			else if(this.drawStringThisDS!=null)
			{
				this.obj_SeriesDrawEle=this.drawStringThisDS;
			}
		}
		/// <summary>
		/// ����༭��Ԫ��
		/// </summary>
		public void ClearThisObject()
		{
			this.obj_SeriesDrawEle=null;
			this.control.Cursor=Cursors.Default;
		}
		/// <summary>
		/// ��ʾ������ʽ
		/// </summary>
		public void ShowCursor()
		{
			if(this.obj_SeriesDrawEle!=null)
			{
				if(this.obj_SeriesDrawEle is Line)
				{
					if(((Line)this.obj_SeriesDrawEle).ObjectType==Line.DrawObjectType.DrawBeeLine)					{						this.control.Cursor=this.getTypeCursor.GetCursor(GetTypeCursor.DrawType.BeeLine);						return;					}
					else					{						this.control.Cursor=this.getTypeCursor.GetCursor(GetTypeCursor.DrawType.FoldLine);						return;					}					
				}
				else if(this.obj_SeriesDrawEle is Node)
				{
					this.SetNodeCursor(((Node)this.obj_SeriesDrawEle).ObjectType);
					return;
				}
				else if(this.obj_SeriesDrawEle is DrawString)
				{
					this.control.Cursor=this.getTypeCursor.GetCursor(GetTypeCursor.DrawType.String);
					return;
				}
				return;
			}
			else if(this.lineThisLine!=null || this.nodeThisNode!=null || this.drawStringThisDS!=null)
			{
				if(this.lineThisLine!=null)
				{
					if(this.lineThisLine.ObjectType==Line.DrawObjectType.DrawBeeLine)					{						this.control.Cursor=this.getTypeCursor.GetCursor(GetTypeCursor.DrawType.BeeLine);						return;					}
					else					{						this.control.Cursor=this.getTypeCursor.GetCursor(GetTypeCursor.DrawType.FoldLine);						return;					}	
				}
				else if(this.nodeThisNode!=null)
				{
					this.SetNodeCursor(this.nodeThisNode.ObjectType);
					return;
				}
				else if(this.drawStringThisDS!=null)
				{
					this.control.Cursor=this.getTypeCursor.GetCursor(GetTypeCursor.DrawType.String);
					return;
				}
			}
			this.control.Cursor=Cursors.Default;
		}
		/// <summary>
		/// Ԫ�ص������Լ��
		/// </summary>
		public bool CheckIntegrity()
		{
			if(!this.bStartFlag)
			{
				MessageForm.Show("��û�д�����ʼ�ڵ�.","DrawFlow");
				return false;					
			}
			else if(!this.bEndFlag)
			{
				MessageForm.Show("��û�д��������ڵ�.","DrawFlow");
				return false;						
			}
			for(int i=0;i<this.arrNodeList.Count;i++)
			{
				Node node=(Node)this.arrNodeList[i];
				if(node.ObjectType==Node.DrawObjectType.DrawNodeBegin)
				{
					if(node.ConnectOutCount<1)
					{
						MessageForm.Show("��ʼ���δ�к�̽��.","DrawFlow");
						return false;
					}
				}
				else if(node.ObjectType==Node.DrawObjectType.DrawNodeEnd)
				{
					if(node.ConnectInCount<1)
					{
						MessageForm.Show("�������δ��ǰ�����.","DrawFlow");
						return false;
					}					
				}
				else if(node.ObjectType==Node.DrawObjectType.DrawNodeGeneral)
				{
					if(node.ConnectInCount<1)
					{
						MessageForm.Show("���IDΪ"+node.NodeListIndex+"��������δ��ǰ�����.","DrawFlow");
						return false;
					}
					if(node.ConnectOutCount<1)
					{
						MessageForm.Show("���IDΪ"+node.NodeListIndex+"��������δ�к�̽��.","DrawFlow");
						return false;
					}
				}
				else if(node.ObjectType==Node.DrawObjectType.DrawSpecificallyOperation)
				{
					if(node.ConnectInCount<1)
					{
						MessageForm.Show("���IDΪ"+node.NodeListIndex+"���ض��������δ��ǰ�����.","DrawFlow");
						return false;
					}
				}
				else if(node.ObjectType==Node.DrawObjectType.DrawGradation)
				{
					if(node.ConnectInCount<1)
					{
						MessageForm.Show("���IDΪ"+node.NodeListIndex+"��˳����δ��ǰ�����.","DrawFlow");
						return false;
					}
					if(node.ConnectOutCount<1)
					{
						MessageForm.Show("���IDΪ"+node.NodeListIndex+"��˳����δ�к�̽��.","DrawFlow");
						return false;
					}
				}
				else if(node.ObjectType==Node.DrawObjectType.DrawSynchronization)
				{
					if(node.ConnectInCount<1)
					{
						MessageForm.Show("���IDΪ"+node.NodeListIndex+"��ͬ�����δ��ǰ�����.","DrawFlow");
						return false;
					}
					if(node.ConnectOutCount<1)
					{
						MessageForm.Show("���IDΪ"+node.NodeListIndex+"��ͬ�����δ�к�̽��.","DrawFlow");
						return false;
					}
				}
				else if(node.ObjectType==Node.DrawObjectType.DrawAsunder)
				{
					if(node.ConnectInCount<1)
					{
						MessageForm.Show("���IDΪ"+node.NodeListIndex+"�ķ�֧���δ��ǰ�����.","DrawFlow");
						return false;
					}
					if(node.ConnectOutCount<1)
					{
						MessageForm.Show("���IDΪ"+node.NodeListIndex+"�ķ�֧���δ�к�̽��.","DrawFlow");
						return false;
					}
				}
				else if(node.ObjectType==Node.DrawObjectType.DrawConverge)
				{
					if(node.ConnectInCount<1)
					{
						MessageForm.Show("���IDΪ"+node.NodeListIndex+"�Ļ�۽��δ��ǰ�����.","DrawFlow");
						return false;
					}
					if(node.ConnectOutCount<1)
					{
						MessageForm.Show("���IDΪ"+node.NodeListIndex+"�Ļ�۽��δ�к�̽��.","DrawFlow");
						return false;
					}
				}
				else if(node.ObjectType==Node.DrawObjectType.DrawGather)
				{
					if(node.ConnectInCount<1)
					{
						MessageForm.Show("���IDΪ"+node.NodeListIndex+"�Ļ������ӽ��δ��ǰ�����.","DrawFlow");
						return false;
					}
					if(node.ConnectOutCount<1)
					{
						MessageForm.Show("���IDΪ"+node.NodeListIndex+"�Ļ������ӽ��δ�к�̽��.","DrawFlow");
						return false;
					}
				}
				else if(node.ObjectType==Node.DrawObjectType.DrawJudgement)
				{
					if(node.ConnectInCount<1)
					{
						MessageForm.Show("���IDΪ"+node.NodeListIndex+"���жϽ��δ��ǰ�����.","DrawFlow");
						return false;
					}
					if(node.ConnectOutCount<1)
					{
						MessageForm.Show("���IDΪ"+node.NodeListIndex+"���жϽ��δ�к�̽��.","DrawFlow");
						return false;
					}
				}
				else if(node.ObjectType==Node.DrawObjectType.DrawDataNode)
				{
					if(node.ConnectOutCount<1)
					{
						MessageForm.Show("���IDΪ"+node.NodeListIndex+"��Ӧ�����ݽ��δ�к�̽��.","DrawFlow");
						return false;
					}
				}
			}
			return true;
		}
		/// <summary>
		/// 
		/// </summary>
		public void MinMoveElement(System.Windows.Forms.Keys key)		{			if(this.lastEdit is Line)			{				this.line_OperationTemp=(Line)this.lastEdit;				switch(key)				{					case Keys.Up:					{						this.line_OperationTemp.Y0--;						this.line_OperationTemp.Y1--;						if(this.line_OperationTemp.ObjectType==Line.DrawObjectType.DrawFoldLine)						{							this.line_OperationTemp.Y2--;							this.line_OperationTemp.Y3--;						}						break;					}					case Keys.Down:					{						this.line_OperationTemp.Y0++;						this.line_OperationTemp.Y1++;						if(this.line_OperationTemp.ObjectType==Line.DrawObjectType.DrawFoldLine)						{							this.line_OperationTemp.Y2++;							this.line_OperationTemp.Y3++;						}						break;					}					case Keys.Right:					{						this.line_OperationTemp.X0++;						this.line_OperationTemp.X1++;						if(this.line_OperationTemp.ObjectType==Line.DrawObjectType.DrawFoldLine)						{							this.line_OperationTemp.X2++;							this.line_OperationTemp.X3++;						}						break;										}					case Keys.Left:					{						this.line_OperationTemp.X0--;						this.line_OperationTemp.X1--;						if(this.line_OperationTemp.ObjectType==Line.DrawObjectType.DrawFoldLine)						{							this.line_OperationTemp.X2--;							this.line_OperationTemp.X3--;						}						break;					}				}				this.MinLineMoveRoundNode(this.line_OperationTemp,0);				if(this.line_OperationTemp.ObjectType==Line.DrawObjectType.DrawBeeLine)				{					this.MinLineMoveRoundNode(this.line_OperationTemp,1);				}				else				{					this.MinLineMoveRoundNode(this.line_OperationTemp,3);				}								this.reDrawBitmap(this.graDrawPanel,10,10);				this.RefreshBackground();			}			else if(this.lastEdit is Node)			{				this.node_OperationTemp=(Node)this.lastEdit;				int iMoveWidth=0,iMoveHeight=0;				switch(key)				{					case Keys.Up:this.node_OperationTemp.Y--;iMoveWidth=0;iMoveHeight=-1;break;					case Keys.Down:this.node_OperationTemp.Y++;iMoveWidth=0;iMoveHeight=1;break;					case Keys.Right:this.node_OperationTemp.X++;iMoveWidth=1;iMoveHeight=0;break;					case Keys.Left:this.node_OperationTemp.X--;iMoveWidth=-1;iMoveHeight=0;break;				}				this.GetConnectNodeLine(this.node_OperationTemp);				if(this.arrLineConnectNode.Count>0)				{					Line line;					for(int i=0;i<this.arrLineConnectNode.Count;i++)//ʹ���ӵ��ýڵ����Ҳת��					{						line=((Line)this.arrLineConnectNode[i]);						if(line.FirstNode==	this.node_OperationTemp)						{							if(line.ObjectType==Line.DrawObjectType.DrawBeeLine)							{								line.X0+=iMoveWidth;								line.Y0+=iMoveHeight;							}							else							{								this.iNodeX=this.node_OperationTemp.X;								this.iNodeY=this.node_OperationTemp.Y;								this.iNodeWidth=this.node_OperationTemp.Width;								this.iNodeHeight=this.node_OperationTemp.Height;																this.MoveNodeChangeLine(line,0,this.node_OperationTemp,line.FirNodeInterfaceIndex);							}						}						if(line.SecondNode==this.node_OperationTemp)						{							if(line.ObjectType==Line.DrawObjectType.DrawBeeLine)							{								line.X1+=iMoveWidth;								line.Y1+=iMoveHeight;							}							else							{								this.iNodeX=this.node_OperationTemp.X;								this.iNodeY=this.node_OperationTemp.Y;								this.iNodeWidth=this.node_OperationTemp.Width;								this.iNodeHeight=this.node_OperationTemp.Height;								this.MoveNodeChangeLine(line,3,this.node_OperationTemp,line.SecNodeInterfaceIndex);							}						}					}				}								this.reDrawBitmap(this.graDrawPanel,10,10);				this.RefreshBackground();			}			else if(this.lastEdit is DrawString)			{				this.ds_OperationTemp=(DrawString)this.lastEdit;				switch(key)				{					case Keys.Up:this.ds_OperationTemp.Y--;break;					case Keys.Down:this.ds_OperationTemp.Y++;break;					case Keys.Right:this.ds_OperationTemp.X++;break;					case Keys.Left:this.ds_OperationTemp.X--;break;				}				this.reDrawBitmap(this.graDrawPanel,10,10);				this.RefreshBackground();			}		} 		/// <summary>		/// ����Ƿ���Ի��ƽڵ�		/// </summary>		public void CheckCanDraw()		{			if(this.control.isNodeLimit)			{				if(nodeThisNode.ObjectType==Node.DrawObjectType.DrawNodeBegin && this.bStartFlag)				{					this.nodeThisNode=null;					MessageForm.Show("�Բ��𣬿�ʼ�ڵ㴴�������ơ�","DrawFlow");					return;				}				else if(nodeThisNode.ObjectType==Node.DrawObjectType.DrawNodeEnd && this.bEndFlag)				{					this.nodeThisNode=null;					MessageForm.Show("�Բ��𣬽����ڵ㴴�������ơ�","DrawFlow");					return;				}				else if(this.i_GeneralCount>=20 && nodeThisNode.ObjectType==Node.DrawObjectType.DrawNodeGeneral)				{					this.nodeThisNode=null;					MessageForm.Show("�Բ�����Ŀǰѡ��Ľڵ�����Ϊ20��.","DrawFlow");					return;				}			}				} 
		/// <summary>		/// ��ʾ���ָ��		/// </summary>		public void CursorsDefault()		{			this.lineThisLine=null;			this.lineSelectLine=null;			this.nodeThisNode=null;			this.nodeSelectNode=null;			this.drawStringThisDS=null;			this.drawStringSelectDS=null;			this.control.Cursor=Cursors.Default;		} 
		#endregion

		#region ������
		/// <summary>
		/// �������Ҽ�����
		/// </summary>
		/// <param name="e"></param>
		public void MouseDown_Right(System.Windows.Forms.MouseEventArgs e)
		{
			location.X=e.X;
			location.Y=e.Y;
			this.b_IsDownMouseRight=true;
			this.IsMouseOnAnyControl(e.X,e.Y);
			if(this.arrLineList.Count<=0 && this.arrNodeList.Count<=0 && this.arrDrawStringList.Count<=0)
			{
				this.control.menuItem1.Enabled=false;
				this.control.menuItem3.Enabled=false;
			}
			else
			{
				this.control.menuItem1.Enabled=true;
				this.control.menuItem3.Enabled=true;
			}
		}
		/// <summary>
		/// ��갴���¼�
		/// </summary>
		/// <param name="e"></param>
		public void MouseDown(System.Windows.Forms.MouseEventArgs e)
		{
			if(this.TBDrawStringContent.Visible==true)
			{
				if(this.lastEdit is DrawString)
				{
					((DrawString)this.lastEdit).Content=this.TBDrawStringContent.Text;
					this.TBDrawStringContent.Visible=false;
					this.reDrawBitmap(this.graDrawPanel,10,10);
					this.RefreshBackground();
//					this.control.Invalidate();
				}
			}
			else if(this.TBnodeContent.Visible==true)
			{
				if(this.lastEdit is Node)
				{
					((Node)this.lastEdit).NodeText=this.TBnodeContent.Text;
					this.TBnodeContent.Visible=false;
					this.reDrawBitmap(this.graDrawPanel,10,10);
					this.RefreshBackground();
//					this.control.Invalidate();
				}				
			}
			else if(this.TBLineContent.Visible==true)
			{
				if(this.lastEdit is Line)
				{
					((Line)this.lastEdit).Content=this.TBLineContent.Text;
					this.TBLineContent.Visible=false;
					this.reDrawBitmap(this.graDrawPanel,10,10);
					this.RefreshBackground();
				}				
			}

			if(e.Button==MouseButtons.Left)
			{
				if(this.obj_SeriesDrawEle!=null && (this.lineThisLine==null && this.nodeThisNode==null && this.drawStringThisDS==null))
				{
					if(this.obj_SeriesDrawEle is Line)
					{
						this.lineThisLine=new Line(((Line)this.obj_SeriesDrawEle).ObjectType,this.arrLineList.Count);
						this.newLine=this.lineThisLine;
					}
					else if(this.obj_SeriesDrawEle is Node)
					{
						this.nodeThisNode=new Node(this.arrNodeList.Count,((Node)this.obj_SeriesDrawEle).ObjectType,getDefaultText(((Node)this.obj_SeriesDrawEle).ObjectType));
						this.newNode=this.nodeThisNode;
					}
					else if(this.obj_SeriesDrawEle is DrawString)
					{
						this.drawStringThisDS=new DrawString(this.arrDrawStringList.Count);
						this.newDrawString=this.drawStringThisDS;
					}
				}
				else if(this.obj_SeriesDrawEle!=null && (this.lineThisLine!=null || this.nodeThisNode!=null || this.drawStringThisDS!=null))
				{
					if(this.lineThisLine!=null)
					{
						this.obj_SeriesDrawEle=this.lineThisLine;
					}
					else if(this.nodeThisNode!=null)
					{
						this.CheckCanDraw();
						this.obj_SeriesDrawEle=this.nodeThisNode;
					}
					else if(this.drawStringThisDS!=null)
					{
						this.obj_SeriesDrawEle=this.drawStringThisDS;
					}
					this.ShowCursor();
				}

				if(this.bSelectAllReday)//�Ƿ��Ѿ�ȫ��ѡ��
				{
					if(this.IsMouseOnAnyControl(e.X,e.Y))
					{
						this.control.Cursor=Cursors.SizeAll;
						this.iMouseInitX=e.X;
						this.iMouseInitY=e.Y;
						this.bSelectAll=true;
						this.DrawBackGround();
						this.RefreshBackground();
						this.bSelectRectangleReday=false;
						return;
					}
				}
				else if(this.lineThisLine!=null)//������������ ����lineThisLineΪnull��ʾ���ڻ���
				{
					this.arrLineList.Add(this.newLine);
					this.newLine=null;
					this.iLineFirstNodeX=e.X;
					this.iLineFirstNodeY=e.Y;
					this.selectPoint.LinePoint[0].X=e.X;
					this.selectPoint.LinePoint[0].Y=e.Y;
					this.lineThisLine.addPoint(this.iLineFirstNodeX,this.iLineFirstNodeY);
					this.lineThisLine.LineNodeCount=2;
					return;
				}
				else if(this.nodeThisNode!=null)//���������½ڵ� ����nodeThisNodeΪnull��ʾ���ڻ��ڵ�
				{
					if(this.control.isNodeLimit)
					{
						if(nodeThisNode.ObjectType==Node.DrawObjectType.DrawNodeBegin && this.bStartFlag)						{							this.nodeThisNode=null;							MessageForm.Show("�Բ��𣬿�ʼ�ڵ㴴�������ơ�","DrawFlow");							return;						}						else if(nodeThisNode.ObjectType==Node.DrawObjectType.DrawNodeEnd && this.bEndFlag)						{							this.nodeThisNode=null;							MessageForm.Show("�Բ��𣬽����ڵ㴴�������ơ�","DrawFlow");							return;						}						else if(this.i_GeneralCount>=20 && nodeThisNode.ObjectType==Node.DrawObjectType.DrawNodeGeneral)						{							this.nodeThisNode=null;							MessageForm.Show("�Բ�����Ŀǰѡ��Ľڵ�����Ϊ20��.","DrawFlow");							return;						}
					}
					this.arrNodeList.Add(this.newNode);
					this.newNode=null;
					this.iNodeX=e.X;
					this.iNodeY=e.Y;
					this.iTempNodeX=e.X;
					this.iTempNodeY=e.Y;
					this.nodeThisNode.X=this.iNodeX;
					this.nodeThisNode.Y=this.iNodeY;
					return;
				}
				else if(this.drawStringThisDS!=null)//������д�ְ�
				{
					this.arrDrawStringList.Add(this.newDrawString);
					this.newDrawString=null;
					this.iDrawStringX=e.X;
					this.iDrawStringY=e.Y;
					this.iTempDSX=e.X;
					this.iTempDSY=e.Y;
					this.drawStringThisDS.X=this.iDrawStringX;
					this.drawStringThisDS.Y=this.iDrawStringY;
					return;
				}
				else if(this.nodeTempNode!=null)
				{
					this.nodeSelectNode=this.nodeTempNode;
					this.nodeTempNode=null; 
					this.iMouseInitX=e.X;
					this.iMouseInitY=e.Y;

					this.iNodeX=this.nodeSelectNode.X;
					this.iNodeY=this.nodeSelectNode.Y;
					this.iNodeWidth=this.nodeSelectNode.Width;
					this.iNodeHeight=this.nodeSelectNode.Height;

					this.GetConnectNodeLine(this.nodeSelectNode);
					this.selectPoint.SetRectanglePoint(this.iNodeX,this.iNodeY,this.iNodeWidth,this.iNodeHeight);
					if(this.arrLineConnectNode.Count!=0)//�ýڵ����Ƿ��������ӵ�  0��ʾû��������  ����ֵ��ʾ��count��������
					{//��������  �øýڵ��Ϊ��ڵ�  ��  ���������ӵ��߳�Ϊ���
						this.ReDrawBitmapNodeMove(this.graDrawPanel,this.nodeSelectNode.NodeListIndex);
					}
					else
					{//û��������  �øýڵ��Ϊ��ڵ�
						this.reDrawBitmap(this.graDrawPanel,0,this.nodeSelectNode.NodeListIndex);
					}
					this.bDrawConnectNodeLine=true;
					this.bDrawNode=true;
					this.RefreshBackground();
					//					this.control.Invalidate();
					return;
				}
				else if(this.drawStringTempDS!=null)
				{
					this.drawStringSelectDS=this.drawStringTempDS;
					this.drawStringTempDS=null;
					this.iMouseInitX=e.X;
					this.iMouseInitY=e.Y;

					this.iDrawStringX=this.drawStringSelectDS.X;
					this.iDrawStringY=this.drawStringSelectDS.Y;
					this.iDrawStringWidth=this.drawStringSelectDS.Width;
					this.iDrawStringHeight=this.drawStringSelectDS.Height;
					this.strDSContent=this.drawStringSelectDS.Content;
					this.selectPoint.SetRectanglePoint(this.iDrawStringX,this.iDrawStringY,this.iDrawStringWidth,this.iDrawStringHeight);
					this.reDrawBitmap(this.graDrawPanel,2,this.drawStringSelectDS.DrawStrListIndex);
					this.bDrawString=true;
					this.RefreshBackground();
					//					this.control.Invalidate();
					return;

				}
				else if(this.lineTempLine!=null)//�����ĳ���ı仯����
				{
					this.lineSelectLine=this.lineTempLine;
					this.iSelectNodeIndex=this.iTempNodeIndex;

					this.iLineFirstNodeX=this.lineSelectLine.GetLineNodeInfo(0);
					this.iLineFirstNodeY=this.lineSelectLine.GetLineNodeInfo(1);
					this.iLineSecondNodeX=this.lineSelectLine.GetLineNodeInfo(2);
					this.iLineSecondNodeY=this.lineSelectLine.GetLineNodeInfo(3);
					if(this.lineSelectLine.ObjectType==Line.DrawObjectType.DrawBeeLine)
					{
						this.selectPoint.SetLinePoint(this.iLineFirstNodeX,this.iLineFirstNodeY,this.iLineSecondNodeX,this.iLineSecondNodeY);
					}
					else
					{
						this.iLine3thNodeX=this.lineSelectLine.GetLineNodeInfo(4);
						this.iLine3thNodeY=this.lineSelectLine.GetLineNodeInfo(5);
						this.iLine4thNodeX=this.lineSelectLine.GetLineNodeInfo(6);
						this.iLine4thNodeY=this.lineSelectLine.GetLineNodeInfo(7);
						this.selectPoint.SetFlodLinePoint(this.lineSelectLine);
					}
					this.reDrawBitmap(this.graDrawPanel,1,this.lineSelectLine.LineListIndex);
					this.bDrawLine=true;
					this.RefreshBackground();
//					this.control.Invalidate();
					return;
				}						
				else 
				{
					if(this.bSelectRectangleReday)
					{
						if(this.IsMouseOnAnySelectControl(e.X,e.Y))
						{
							this.control.Cursor=Cursors.SizeAll;
							this.iMouseInitX=e.X;
							this.iMouseInitY=e.Y;
							this.ReDrawNotSelect(this.graDrawPanel);
							this.bDrawSelectElement=true;
							//							this.control.Invalidate();
							this.RefreshBackground();
							return;
						}
						else if(!this.IsMouseOnAnyControl(e.X,e.Y))
						{
							this.iMouseDownX=e.X;
							this.iMouseDownY=e.Y;
							this.iMouseSelectX=e.X;
							this.iMouseSelectY=e.Y;
							this.bNotSelectAnyOne=true;
							this.bSelectRectangleReday=false;
							this.arrLineSelectList.Clear();
							this.arrNodeSelectList.Clear();
							this.arrDrawStringSelectList.Clear();
							this.arrLineNotSelectList.Clear();
							this.arrNodeNotSelectList.Clear();
							this.arrDrawStringNotSelectList.Clear();
							this.lastEdit=null;
							this.AttriShow();
							this.control.Invalidate();
							return;
						}
						else
						{
							this.bNotSelectAnyOne=true;
							this.bSelectRectangleReday=false;
							this.arrLineSelectList.Clear();
							this.arrNodeSelectList.Clear();
							this.arrDrawStringSelectList.Clear();
							this.arrLineNotSelectList.Clear();
							this.arrNodeNotSelectList.Clear();
							this.arrDrawStringNotSelectList.Clear();
							this.lastEdit=null;
							this.AttriShow();
							this.control.Invalidate();
							return;
						}
					}
					this.nodeSelectNode=this.IsMouseOnNode(e.X,e.Y,this.arrNodeList);//����Ƿ��ڽڵ���
					if(this.nodeSelectNode!=null && this.nodeTempNode==null)//��nodeSelectNode��Ϊ�գ���ʾ����ڸýڵ���
					{
						if(e.Clicks==2)
						{
							this.control.testDrawLength.Text=this.nodeSelectNode.NodeText;
							if(this.control.testDrawLength.Width<50)
							{
								this.TBnodeContent.Width=50;
							}
							else
							{
								this.TBnodeContent.Width=this.control.testDrawLength.Width+3;
							}
							this.TBnodeContent.Font=this.nodeSelectNode.NodeTextFont;
							this.TBnodeContent.Location=new Point(this.nodeSelectNode.X+this.nodeSelectNode.Width/2-this.TBnodeContent.Width/2,this.nodeSelectNode.Y+this.nodeSelectNode.Height);
							this.TBnodeContent.Text=this.nodeSelectNode.NodeText;
							this.TBnodeContent.Visible=true;
							this.TBnodeContent.Focus();
							this.TBnodeContent.SelectAll();
							this.RefreshBackground();
							return;
						}
						else
						{
							this.iMouseInitX=e.X;
							this.iMouseInitY=e.Y;
							this.iNodeX=this.nodeSelectNode.X;
							this.iNodeY=this.nodeSelectNode.Y;
							this.iNodeWidth=this.nodeSelectNode.Width;
							this.iNodeHeight=this.nodeSelectNode.Height;
							this.GetConnectNodeLine(this.nodeSelectNode);
							this.selectPoint.SetRectanglePoint(this.iNodeX,this.iNodeY,this.iNodeWidth,this.iNodeHeight);
							if(this.arrLineConnectNode.Count!=0)//�ýڵ����Ƿ��������ӵ�  0��ʾû��������  ����ֵ��ʾ��count��������
							{//��������  �øýڵ��Ϊ��ڵ�  ��  ���������ӵ��߳�Ϊ���
								this.ReDrawBitmapNodeMove(this.graDrawPanel,this.nodeSelectNode.NodeListIndex);
							}
							else
							{//û��������  �øýڵ��Ϊ��ڵ�
								this.reDrawBitmap(this.graDrawPanel,0,this.nodeSelectNode.NodeListIndex);
							}
							this.bDrawConnectNodeLine=true;
							this.bDrawNode=true;
							this.RefreshBackground();
							//							this.control.Invalidate();
							return;
						}
					}
					else if(this.nodeSelectNode==null)
					{
						this.drawStringSelectDS=this.IsMouseOnDrawString(e.X,e.Y,this.arrDrawStringList);
						if(this.drawStringSelectDS!=null)//�ƶ�����
						{
							if(e.Clicks==2)
							{
								this.TBDrawStringContent.Font=this.drawStringSelectDS.DSTextFont;
								this.TBDrawStringContent.SetBounds(this.drawStringSelectDS.X,this.drawStringSelectDS.Y,this.drawStringSelectDS.Width,this.drawStringSelectDS.Height);
								this.TBDrawStringContent.Text=this.strDSContent;
								this.TBDrawStringContent.Visible=true;
								this.TBDrawStringContent.Focus();
								this.TBDrawStringContent.SelectAll();
								return;
							}
							else
							{
								this.iMouseInitX=e.X;
								this.iMouseInitY=e.Y;
								this.iDrawStringX=this.drawStringSelectDS.X;
								this.iDrawStringY=this.drawStringSelectDS.Y;
								this.iDrawStringWidth=this.drawStringSelectDS.Width;
								this.iDrawStringHeight=this.drawStringSelectDS.Height;
								this.strDSContent=this.drawStringSelectDS.Content;
								this.selectPoint.SetRectanglePoint(this.iDrawStringX,this.iDrawStringY,this.iDrawStringWidth,this.iDrawStringHeight);
								this.reDrawBitmap(this.graDrawPanel,2,this.drawStringSelectDS.DrawStrListIndex);
								this.bDrawString=true;
								this.RefreshBackground();
								//								this.control.Invalidate();
								return;
							}
						}
						else if(this.drawStringSelectDS==null && this.nodeSelectNode==null)
						{
							this.lineSelectLine=this.IsMouseOnLine(e.X,e.Y,this.arrLineList);
							if(this.lineSelectLine!=null)
							{
								if(e.Clicks==2)
								{
									this.control.testDrawLength.Text=this.lineSelectLine.Content;
									if(this.control.testDrawLength.Width<50)
									{
										this.TBLineContent.Width=50;
									}
									else
									{
										this.TBLineContent.Width=this.control.testDrawLength.Width+3;
									}
									this.TBLineContent.Font=this.lineSelectLine.LineTextFont;
									if(this.lineSelectLine.ObjectType==Line.DrawObjectType.DrawBeeLine)
									{
										this.TBLineContent.Location=new Point((this.lineSelectLine.X0+this.lineSelectLine.X1)/2-this.TBLineContent.Width/2,(this.lineSelectLine.Y0+this.lineSelectLine.Y1)/2);
									}
									else
									{
										this.TBLineContent.Location=new Point((this.lineSelectLine.X1+this.lineSelectLine.X2)/2-this.control.testDrawLength.Width/2,(this.lineSelectLine.Y1+this.lineSelectLine.Y2)/2);
									}
									this.TBLineContent.Text=this.lineSelectLine.Content;
									this.TBLineContent.Visible=true;
									if(this.lineSelectLine.Modality)									{										this.iLineFirstNodeX=this.lineSelectLine.GetLineNodeInfo(0);										this.iLineFirstNodeY=this.lineSelectLine.GetLineNodeInfo(1);										this.iLineSecondNodeX=this.lineSelectLine.GetLineNodeInfo(2);										this.iLineSecondNodeY=this.lineSelectLine.GetLineNodeInfo(3);										this.iLine3thNodeX=this.lineSelectLine.GetLineNodeInfo(4);										this.iLine3thNodeY=this.lineSelectLine.GetLineNodeInfo(5);										this.iLine4thNodeX=this.lineSelectLine.GetLineNodeInfo(6);										this.iLine4thNodeY=this.lineSelectLine.GetLineNodeInfo(7);																		}
									this.TBLineContent.Focus();
									this.TBLineContent.SelectAll();
									this.RefreshBackground();
									return;
								}
								this.iSelectNodeIndex=99;
								this.iMouseInitX=e.X;
								this.iMouseInitY=e.Y;
								this.iMouseDownX=e.X;
								this.iMouseDownY=e.Y;
								this.reDrawBitmap(this.graDrawPanel,1,this.lineSelectLine.LineListIndex);
								this.RefreshBackground();
								if(this.lineSelectLine.ObjectType==Line.DrawObjectType.DrawBeeLine)
								{
									this.iLineFirstNodeX=this.lineSelectLine.GetLineNodeInfo(0);
									this.iLineFirstNodeY=this.lineSelectLine.GetLineNodeInfo(1);
									this.iLineSecondNodeX=this.lineSelectLine.GetLineNodeInfo(2);
									this.iLineSecondNodeY=this.lineSelectLine.GetLineNodeInfo(3);
									this.selectPoint.SetLinePoint(this.iLineFirstNodeX,this.iLineFirstNodeY,this.iLineSecondNodeX,this.iLineSecondNodeY);
								}
								else
								{
									this.iLineFirstNodeX=this.lineSelectLine.GetLineNodeInfo(0);
									this.iLineFirstNodeY=this.lineSelectLine.GetLineNodeInfo(1);
									this.iLineSecondNodeX=this.lineSelectLine.GetLineNodeInfo(2);
									this.iLineSecondNodeY=this.lineSelectLine.GetLineNodeInfo(3);
									this.iLine3thNodeX=this.lineSelectLine.GetLineNodeInfo(4);
									this.iLine3thNodeY=this.lineSelectLine.GetLineNodeInfo(5);
									this.iLine4thNodeX=this.lineSelectLine.GetLineNodeInfo(6);
									this.iLine4thNodeY=this.lineSelectLine.GetLineNodeInfo(7);
									this.selectPoint.SetFlodLinePoint(this.lineSelectLine);
								}
								this.bDrawLine=true;
								this.control.Invalidate();
								return;
							}
							else
							{
								this.iMouseDownX=e.X;
								this.iMouseDownY=e.Y;
								this.iMouseSelectX=e.X;
								this.iMouseSelectY=e.Y;
								this.bNotSelectAnyOne=true;
								this.bSelectRectangleReday=false;
								this.arrLineSelectList.Clear();
								this.arrNodeSelectList.Clear();
								this.arrDrawStringSelectList.Clear();
								this.arrLineNotSelectList.Clear();
								this.arrNodeNotSelectList.Clear();
								this.arrDrawStringNotSelectList.Clear();
								this.lastEdit=null;
								this.AttriShow();
								this.control.Invalidate();
							}
						}
					}
				}
			}		
		}
		/// <summary>
		/// ����ƶ��¼�
		/// </summary>
		/// <param name="e"></param>
		public void MouseMove(System.Windows.Forms.MouseEventArgs e)
		{
			if(e.Button==MouseButtons.Left)
			{
				if(this.bSelectAllReday && this.control.Cursor==Cursors.SizeAll)
				{
					int iMoveWidth=e.X-this.iMouseInitX;
					int iMoveHeight=e.Y-this.iMouseInitY;
					Line line;
					Node node;
					DrawString drawString;
					for(int i=0;i<this.arrLineList.Count;i++)
					{
						line=(Line)this.arrLineList[i];
						line.X0+=iMoveWidth;
						line.Y0+=iMoveHeight;
						line.X1+=iMoveWidth;
						line.Y1+=iMoveHeight;
						if(line.ObjectType==Line.DrawObjectType.DrawFoldLine)
						{
							line.X2+=iMoveWidth;
							line.Y2+=iMoveHeight;
							line.X3+=iMoveWidth;
							line.Y3+=iMoveHeight;
						}
					}
					for(int i=0;i<this.arrNodeList.Count;i++)
					{
						node=(Node)this.arrNodeList[i];
						node.X+=iMoveWidth;
						node.Y+=iMoveHeight;
					}
					
					for(int i=0;i<this.arrDrawStringList.Count;i++)
					{
						drawString=(DrawString)this.arrDrawStringList[i];
						drawString.X+=iMoveWidth;
						drawString.Y+=iMoveHeight;
					}
					this.bSelectAll=true;
					this.control.Invalidate();
					this.iMouseInitX=e.X;
					this.iMouseInitY=e.Y;
					return;
				}
				else if(this.lineThisLine!=null)//������������
				{
					this.size=this.control.Size;
					if(0<e.X && e.X<this.size.Width && 0<e.Y && e.Y<this.size.Height)
					{
						if(this.lineThisLine.ObjectType==Line.DrawObjectType.DrawBeeLine)
						{
							this.iLineSecondNodeX=e.X;
							this.iLineSecondNodeY=e.Y;
							this.selectPoint.LinePoint[1].X=e.X;
							this.selectPoint.LinePoint[1].Y=e.Y;
							this.isLineRoundNode(e.X,e.Y,this.lineThisLine,1);//�ж��Ƿ񿿽�ĳ�ڵ�����ӵ�
						}
						else 
						{
							this.iLineSecondNodeX=(this.iLineFirstNodeX+e.X)/2;
							this.iLineSecondNodeY=this.iLineFirstNodeY;
							this.iLine3thNodeX=(this.iLineFirstNodeX+e.X)/2;
							this.iLine3thNodeY=e.Y;
							this.iLine4thNodeX=e.X;
							this.iLine4thNodeY=e.Y;
							this.selectPoint.LinePoint[1].X=(this.iLineFirstNodeX+e.X)/2;
							this.selectPoint.LinePoint[1].Y=this.iLineFirstNodeY;
							this.selectPoint.LinePoint[2].X=(this.iLineFirstNodeX+e.X)/2;
							this.selectPoint.LinePoint[2].Y=e.Y;
							this.selectPoint.LinePoint[3].X=e.X;
							this.selectPoint.LinePoint[3].Y=e.Y;
							this.isLineRoundNode(e.X,e.Y,this.lineThisLine,3);//�ж��Ƿ񿿽�ĳ�ڵ�����ӵ�
						}
						this.bDrawLine=true;
						this.control.Invalidate();
					}
					return;
				}
				else if(this.nodeThisNode!=null)//���������½ڵ�
				{
					this.size=this.control.Size;
					if(e.X<=this.iTempNodeX)
					{
						this.iNodeX=e.X;
						this.iNodeWidth=this.iTempNodeX-e.X;
					}
					else
					{
						this.iNodeX=this.iTempNodeX;
						this.iNodeWidth=e.X-this.iTempNodeX;
					}

					if(e.Y<=this.iTempNodeY)
					{
						this.iNodeY=e.Y;
						this.iNodeHeight=this.iTempNodeY-e.Y;
					}
					else
					{
						this.iNodeY=this.iTempNodeY;
						this.iNodeHeight=e.Y-this.iTempNodeY;
					}
					this.selectPoint.SetRectanglePoint(this.iNodeX,this.iNodeY,this.iNodeWidth,this.iNodeHeight);
					this.bDrawNode=true;
					this.control.Invalidate();
					return;
				}
				else if(this.drawStringThisDS!=null)//����������д�ְ�
				{
					this.size=this.control.Size;
					if(0<e.X && e.X<this.size.Width && 0<e.Y && e.Y<this.size.Height)
					{
						if(e.X<=this.iTempDSX)
						{
							this.iDrawStringX=e.X;
							this.iDrawStringWidth=this.iTempDSX-e.X;
						}
						else
						{
							this.iDrawStringX=this.iTempDSX;
							this.iDrawStringWidth=e.X-this.iTempDSX;
						}

						if(e.Y<=this.iTempDSY)
						{
							this.iDrawStringY=e.Y;
							this.iDrawStringHeight=this.iTempDSY-e.Y;
						}
						else
						{
							this.iDrawStringY=this.iTempDSY;
							this.iDrawStringHeight=e.Y-this.iTempDSY;
						}					
						this.selectPoint.SetRectanglePoint(this.iDrawStringX,this.iDrawStringY,this.iDrawStringWidth,this.iDrawStringHeight);
						this.bDrawString=true;
						this.control.Invalidate();
					}
					return;
				}
				else if(this.lineSelectLine!=null && this.iSelectNodeIndex!=99 && this.TBLineContent.Visible==false)//���ڱ༭ѡ�е��ߣ����ͣ�
				{
					this.size=this.control.Size;
					if(0<e.X && e.X<this.size.Width && 0<e.Y && e.Y<this.size.Height)
					{
						if(this.lineSelectLine.ObjectType==Line.DrawObjectType.DrawBeeLine)
						{
							switch(this.iSelectNodeIndex)
							{
								case 0:
								{
									this.iLineFirstNodeX=e.X;
									this.iLineFirstNodeY=e.Y;
									this.selectPoint.LinePoint[0].X=e.X;
									this.selectPoint.LinePoint[0].Y=e.Y;
									this.isLineRoundNode(e.X,e.Y,this.lineSelectLine,0);//�ж��Ƿ񿿽�ĳ�ڵ�����ӵ�
									break;
								}
								case 1:	
								{
									this.iLineSecondNodeX=e.X;
									this.iLineSecondNodeY=e.Y;
									this.selectPoint.LinePoint[1].X=e.X;
									this.selectPoint.LinePoint[1].Y=e.Y;
									this.isLineRoundNode(e.X,e.Y,this.lineSelectLine,1);//�ж��Ƿ񿿽�ĳ�ڵ�����ӵ�
									break;					
								}
							}
						}
						else
						{
							this.ChangeFlodLine(this.lineSelectLine,this.iSelectNodeIndex,9,e.X,e.Y);
							if(this.iSelectNodeIndex==0 || this.iSelectNodeIndex==3)
							{
								this.isLineRoundNode(e.X,e.Y,this.lineSelectLine,this.iSelectNodeIndex);
							}
							else if(this.iSelectNodeIndex==1)
							{
								this.isLineRoundNode(e.X,e.Y,this.lineSelectLine,0);
							}
							else if(this.iSelectNodeIndex==2)
							{
								this.isLineRoundNode(e.X,e.Y,this.lineSelectLine,3);
							}
						}
						this.bDrawLine=true;
						this.control.Invalidate();
					}
					return;
				}
				else if(this.nodeSelectNode!=null  && this.TBnodeContent.Visible==false)//ѡ��ĳ�ڵ㣨�ƶ��ڵ㣩
				{
					this.size=this.control.Size;
					if(0<e.X && e.X<this.size.Width && 0<e.Y && e.Y<this.size.Height)
					{
						if(this.control.Cursor==Cursors.SizeAll)
						{
							int iMoveWidth=e.X-this.iMouseInitX;
							int iMoveHeight=e.Y-this.iMouseInitY;
							this.iNodeX+=iMoveWidth;
							this.iNodeY+=iMoveHeight;
							Line line;
							if(this.arrLineConnectNode.Count>0)
							{
								for(int i=0;i<this.arrLineConnectNode.Count;i++)//ʹ���ӵ��ýڵ����Ҳת��
								{
									line=((Line)this.arrLineConnectNode[i]);
									if(line.FirstNode==	this.nodeSelectNode)
									{
										if(line.ObjectType==Line.DrawObjectType.DrawBeeLine)
										{
											line.X0+=iMoveWidth;
											line.Y0+=iMoveHeight;
										}
										else
										{
											this.MoveNodeChangeLine(line,0,this.nodeSelectNode,line.FirNodeInterfaceIndex);
										}
									}
									if(line.SecondNode==this.nodeSelectNode)
									{
										if(line.ObjectType==Line.DrawObjectType.DrawBeeLine)
										{
											line.X1+=iMoveWidth;
											line.Y1+=iMoveHeight;
										}
										else
										{
											this.MoveNodeChangeLine(line,3,this.nodeSelectNode,line.SecNodeInterfaceIndex);
										}
									}
								}
								this.bDrawConnectNodeLine=true;
							}
							this.selectPoint.AddDataRectanglePoint(iMoveWidth,iMoveHeight);
							this.bDrawNode=true;
							this.control.Invalidate();
							this.iMouseInitX=e.X;
							this.iMouseInitY=e.Y;
							return;
						}
						else
						{
							this.ChangeNodeSize(e);
							this.selectPoint.SetRectanglePoint(this.iNodeX,this.iNodeY,this.iNodeWidth,this.iNodeHeight);
							this.ReSizeNodeConLine(this.nodeSelectNode);
							this.nodeSelectNode.X=this.iNodeX;
							this.nodeSelectNode.Y=this.iNodeY;
							this.nodeSelectNode.Width=this.iNodeWidth;
							this.nodeSelectNode.Height=this.iNodeHeight;
							
							this.bDrawConnectNodeLine=true;
							this.bDrawNode=true;
							this.control.Invalidate();
						}
					}
				}
				else if(this.drawStringSelectDS!=null && this.TBDrawStringContent.Visible==false)
				{
					this.size=this.control.Size;
					if(0<e.X && e.X<this.size.Width && 0<e.Y && e.Y<this.size.Height)
					{
						if(this.control.Cursor==Cursors.SizeAll)
						{
							int iMoveWidth=e.X-this.iMouseInitX;
							int iMoveHeight=e.Y-this.iMouseInitY;
							this.iDrawStringX+=iMoveWidth;
							this.iDrawStringY+=iMoveHeight;
							
							this.selectPoint.AddDataRectanglePoint(iMoveWidth,iMoveHeight);
							this.bDrawString=true;
							this.control.Invalidate();
							this.iMouseInitX=e.X;
							this.iMouseInitY=e.Y;	
						}
						else
						{
							this.ChangeDSSize(e);
							this.selectPoint.SetRectanglePoint(this.iDrawStringX,this.iDrawStringY,this.iDrawStringWidth,this.iDrawStringHeight);
							this.drawStringSelectDS.X=this.iDrawStringX;
							this.drawStringSelectDS.Y=this.iDrawStringY;
							this.drawStringSelectDS.Width=this.iDrawStringWidth;
							this.drawStringSelectDS.Height=this.iDrawStringHeight;
							this.bDrawString=true;
							this.control.Invalidate();					
						}
					}
				}
				else if(this.lineSelectLine!=null && this.iSelectNodeIndex==99 && this.TBLineContent.Visible==false)//ѡ��ĳ���ߣ��ƶ��ߣ�
				{
					this.size=this.control.Size;
					if(0<e.X && e.X<this.size.Width && 0<e.Y && e.Y<this.size.Height)
					{
						if((this.lineSelectLine.FirstNode==null)&&(this.lineSelectLine.SecondNode==null))
						{
							int iMoveWidth=e.X-this.iMouseInitX;
							int iMoveHeight=e.Y-this.iMouseInitY;

							if(this.lineSelectLine.ObjectType==Line.DrawObjectType.DrawBeeLine)
							{
								this.selectPoint.LinePoint[0].X+=iMoveWidth;
								this.selectPoint.LinePoint[0].Y+=iMoveHeight;
								this.selectPoint.LinePoint[1].X+=iMoveWidth;
								this.selectPoint.LinePoint[1].Y+=iMoveHeight;
								this.iLineFirstNodeX+=iMoveWidth;
								this.iLineFirstNodeY+=iMoveHeight;
								this.iLineSecondNodeX+=iMoveWidth;
								this.iLineSecondNodeY+=iMoveHeight;
							}
							else
							{
								this.ChangeFlodLine(this.lineSelectLine,9,this.iOnLineSegment,e.X,e.Y);
								if(this.iOnLineSegment==1)
								{
									//									int iMoveWidth=e.X-this.iMouseInitX;
									//									int iMoveHeight=e.Y-this.iMouseInitY;
									this.iLineFirstNodeX+=iMoveWidth;
									this.iLineFirstNodeY+=iMoveHeight;
									this.iLineSecondNodeX+=iMoveWidth;
									this.iLineSecondNodeY+=iMoveHeight;	
									this.iLine3thNodeX+=iMoveWidth;
									this.iLine3thNodeY+=iMoveHeight;
									this.iLine4thNodeX+=iMoveWidth;
									this.iLine4thNodeY+=iMoveHeight;	
									this.selectPoint.LinePoint[0].X+=iMoveWidth;
									this.selectPoint.LinePoint[0].Y+=iMoveHeight;
									this.selectPoint.LinePoint[1].X+=iMoveWidth;
									this.selectPoint.LinePoint[1].Y+=iMoveHeight;
									this.selectPoint.LinePoint[2].X+=iMoveWidth;
									this.selectPoint.LinePoint[2].Y+=iMoveHeight;
									this.selectPoint.LinePoint[3].X+=iMoveWidth;
									this.selectPoint.LinePoint[3].Y+=iMoveHeight;						
								}
							}
						}
						else
						{
							if(((this.lineSelectLine.FirstNode!=null)||(this.lineSelectLine.SecondNode!=null))&&((Math.Abs(e.X-this.iMouseDownX)>10)||(Math.Abs(e.Y-this.iMouseDownY)>10)))
							{
								int iMoveWidth=e.X-this.iMouseDownX;
								int iMoveHeight=e.Y-this.iMouseDownY;
								this.iLineFirstNodeX+=iMoveWidth;
								this.iLineFirstNodeY+=iMoveHeight;
								this.iLineSecondNodeX+=iMoveWidth;
								this.iLineSecondNodeY+=iMoveHeight;
								if(this.lineSelectLine.ObjectType==Line.DrawObjectType.DrawFoldLine)
								{
									this.iLine3thNodeX+=iMoveWidth;
									this.iLine3thNodeY+=iMoveHeight;
									this.iLine4thNodeX+=iMoveWidth;
									this.iLine4thNodeY+=iMoveHeight;
									this.selectPoint.LinePoint[2].X+=iMoveWidth;
									this.selectPoint.LinePoint[2].Y+=iMoveHeight;
									this.selectPoint.LinePoint[3].X+=iMoveWidth;
									this.selectPoint.LinePoint[3].Y+=iMoveHeight;								
								}
								this.selectPoint.LinePoint[0].X+=iMoveWidth;
								this.selectPoint.LinePoint[0].Y+=iMoveHeight;
								this.selectPoint.LinePoint[1].X+=iMoveWidth;
								this.selectPoint.LinePoint[1].Y+=iMoveHeight;
							}
						}
						this.isLineRoundNode(e.X,e.Y,this.lineSelectLine,0);
						if(this.lineSelectLine.ObjectType==Line.DrawObjectType.DrawBeeLine)
						{
							this.isLineRoundNode(e.X,e.Y,this.lineSelectLine,1);
						}
						else
						{
							this.isLineRoundNode(e.X,e.Y,this.lineSelectLine,3);
						}
						this.bDrawLine=true;
						this.control.Invalidate();
						this.iMouseInitX=e.X;
						this.iMouseInitY=e.Y;
					}
				}
				else if(this.bSelectRectangleReday && this.control.Cursor==Cursors.SizeAll)
				{
					int iMoveWidth=e.X-this.iMouseInitX;
					int iMoveHeight=e.Y-this.iMouseInitY;
					Line line;
					Node node;
					DrawString drawString;
					for(int i=0;i<this.arrLineSelectList.Count;i++)
					{
						line=(Line)this.arrLineSelectList[i];
						line.X0+=iMoveWidth;
						line.Y0+=iMoveHeight;
						line.X1+=iMoveWidth;
						line.Y1+=iMoveHeight;
						if(line.ObjectType==Line.DrawObjectType.DrawFoldLine)
						{
							line.X2+=iMoveWidth;
							line.Y2+=iMoveHeight;
							line.X3+=iMoveWidth;
							line.Y3+=iMoveHeight;
						}
					}

					for(int i=0;i<this.arrNodeSelectList.Count;i++)
					{
						node=(Node)this.arrNodeSelectList[i];
						node.X+=iMoveWidth;
						node.Y+=iMoveHeight;
					}
					
					for(int i=0;i<this.arrDrawStringSelectList.Count;i++)
					{
						drawString=(DrawString)this.arrDrawStringSelectList[i];
						drawString.X+=iMoveWidth;
						drawString.Y+=iMoveHeight;
					}
					bDrawSelectElement=true;
					this.control.Invalidate();
					this.iMouseInitX=e.X;
					this.iMouseInitY=e.Y;
				}
				else if(this.bNotSelectAnyOne)
				{
					if(e.X<=this.iMouseDownX)
					{
						this.iMouseSelectX=e.X;
						this.iMouseSelectWidth=this.iMouseDownX-e.X;
					}
					else
					{
						this.iMouseSelectX=this.iMouseDownX;
						this.iMouseSelectWidth=e.X-this.iMouseDownX;
					}

					if(e.Y<=this.iMouseDownY)
					{
						this.iMouseSelectY=e.Y;
						this.iMouseSelectHeight=this.iMouseDownY-e.Y;
					}
					else
					{
						this.iMouseSelectY=this.iMouseDownY;
						this.iMouseSelectHeight=e.Y-this.iMouseDownY;
					}
					this.bIsAtSelect=true;
					this.bDrawSelectRectangle=true;
					this.control.Invalidate();					
				}
			}
			else if(this.lineThisLine==null && this.lineSelectLine==null && this.nodeThisNode==null && this.nodeSelectNode==null)//û����������Ҳû�б༭ѡ�е���
			{
				if((this.lastEdit is Node))
				{
					Node node=((Node)this.lastEdit);
					if(e.X>node.X-6 && e.X<node.X+6 && e.Y>node.Y-6 && e.Y<node.Y+6)
					{
						this.control.Cursor=Cursors.SizeNWSE;
						this.nodeTempNode=node;
						this.iResizeNodeMouseIndex=0;
						this.iTempNodeX=node.X+node.Width;
						this.iTempNodeY=node.Y+node.Height;
					}
					else if(e.X>node.X+node.Width/2-6 && e.X<node.X+node.Width/2+6 && e.Y>node.Y-6 && e.Y<node.Y+6)
					{
						this.control.Cursor=Cursors.SizeNS;
						this.nodeTempNode=node;
						this.iResizeNodeMouseIndex=1;
						this.iTempNodeY=node.Y+node.Height;
					}
					else if(e.X>node.X+node.Width-6 && e.X<node.X+node.Width+6 && e.Y>node.Y-6 && e.Y<node.Y+6)
					{
						this.control.Cursor=Cursors.SizeNESW;
						this.nodeTempNode=node;
						this.iResizeNodeMouseIndex=2;
						this.iTempNodeX=node.X;
						this.iTempNodeY=node.Y+node.Height;
					}
					else if(e.X>node.X-6 && e.X<node.X+6 && e.Y>node.Y+node.Height/2-6 && e.Y<node.Y+node.Height/2+6)
					{
						this.control.Cursor=Cursors.SizeWE;
						this.nodeTempNode=node;
						this.iResizeNodeMouseIndex=3;
						this.iTempNodeX=node.X+node.Width;
					}
					else if(e.X>node.X+node.Width-6 && e.X<node.X+node.Width+6 && e.Y>node.Y+node.Height/2-6 && e.Y<node.Y+node.Height/2+6)
					{
						this.control.Cursor=Cursors.SizeWE;
						this.nodeTempNode=node;
						this.iResizeNodeMouseIndex=4;
						this.iTempNodeX=node.X;
					}
					else if(e.X>node.X-6 && e.X<node.X+6 && e.Y>node.Y+node.Height-6 && e.Y<node.Y+node.Height+6)
					{
						this.control.Cursor=Cursors.SizeNESW;
						this.nodeTempNode=node;
						this.iResizeNodeMouseIndex=5;
						this.iTempNodeX=node.X+node.Width;
						this.iTempNodeY=node.Y;
					}
					else if(e.X>node.X+node.Width/2-6 && e.X<node.X+node.Width/2+6 && e.Y>node.Y+node.Height-6 && e.Y<node.Y+node.Height+6)
					{
						this.control.Cursor=Cursors.SizeNS;
						this.nodeTempNode=node;
						this.iResizeNodeMouseIndex=6;
						this.iTempNodeY=node.Y;
					}
					else if(e.X>node.X+node.Width-6 && e.X<node.X+node.Width+6 && e.Y>node.Y+node.Height-6 && e.Y<node.Y+node.Height+6)
					{
						this.control.Cursor=Cursors.SizeNWSE;
						this.nodeTempNode=node;
						this.iResizeNodeMouseIndex=7;
						this.iTempNodeX=node.X;
						this.iTempNodeY=node.Y;
					}
					else
					{
//						this.control.Cursor=Cursors.Default;
						this.ShowCursor();
						this.nodeTempNode=null;
					}
				}
				else if((this.lastEdit is DrawString))
				{
					DrawString drawString=((DrawString)this.lastEdit);
					if(e.X>drawString.X-6 && e.X<drawString.X+6 && e.Y>drawString.Y-6 && e.Y<drawString.Y+6)
					{
						this.control.Cursor=Cursors.SizeNWSE;
						drawStringTempDS=drawString;
						this.iResizeDSMouseIndex=0;
						this.iTempDSX=drawString.X+drawString.Width;
						this.iTempDSY=drawString.Y+drawString.Height;
					}
					else if(e.X>drawString.X+drawString.Width/2-6 && e.X<drawString.X+drawString.Width/2+6 && e.Y>drawString.Y-6 && e.Y<drawString.Y+6)
					{
						this.control.Cursor=Cursors.SizeNS;
						drawStringTempDS=drawString;
						this.iResizeDSMouseIndex=1;
						this.iTempDSY=drawString.Y+drawString.Height;
					}
					else if(e.X>drawString.X+drawString.Width-6 && e.X<drawString.X+drawString.Width+6 && e.Y>drawString.Y-6 && e.Y<drawString.Y+6)
					{
						this.control.Cursor=Cursors.SizeNESW;
						drawStringTempDS=drawString;
						this.iResizeDSMouseIndex=2;
						this.iTempDSX=drawString.X;
						this.iTempDSY=drawString.Y+drawString.Height;
					}
					else if(e.X>drawString.X-6 && e.X<drawString.X+6 && e.Y>drawString.Y+drawString.Height/2-6 && e.Y<drawString.Y+drawString.Height/2+6)
					{
						this.control.Cursor=Cursors.SizeWE;
						drawStringTempDS=drawString;
						this.iResizeDSMouseIndex=3;
						this.iTempDSX=drawString.X+drawString.Width;
					}
					else if(e.X>drawString.X+drawString.Width-6 && e.X<drawString.X+drawString.Width+6 && e.Y>drawString.Y+drawString.Height/2-6 && e.Y<drawString.Y+drawString.Height/2+6)
					{
						this.control.Cursor=Cursors.SizeWE;
						drawStringTempDS=drawString;
						this.iResizeDSMouseIndex=4;
						this.iTempDSX=drawString.X;
					}
					else if(e.X>drawString.X-6 && e.X<drawString.X+6 && e.Y>drawString.Y+drawString.Height-6 && e.Y<drawString.Y+drawString.Height+6)
					{
						this.control.Cursor=Cursors.SizeNESW;
						drawStringTempDS=drawString;
						this.iResizeDSMouseIndex=5;
						this.iTempDSX=drawString.X+drawString.Width;
						this.iTempDSY=drawString.Y;
					}
					else if(e.X>drawString.X+drawString.Width/2-6 && e.X<drawString.X+drawString.Width/2+6 && e.Y>drawString.Y+drawString.Height-6 && e.Y<drawString.Y+drawString.Height+6)
					{
						this.control.Cursor=Cursors.SizeNS;
						drawStringTempDS=drawString;
						this.iResizeDSMouseIndex=6;
						this.iTempDSY=drawString.Y;
					}
					else if(e.X>drawString.X+drawString.Width-6 && e.X<drawString.X+drawString.Width+6 && e.Y>drawString.Y+drawString.Height-6 && e.Y<drawString.Y+drawString.Height+6)
					{
						this.control.Cursor=Cursors.SizeNWSE;
						drawStringTempDS=drawString;
						this.iResizeDSMouseIndex=7;
						this.iTempDSX=drawString.X;
						this.iTempDSY=drawString.Y;
					}
					else
					{
//						this.control.Cursor=Cursors.Default;
						this.ShowCursor();
						drawStringTempDS=null;
					}
				}
				if(this.control.Cursor==Cursors.Default || this.control.Cursor==Cursors.Hand)
				{
					Line line;
					for(int i=0;i<this.arrLineList.Count;i++)//����Ƿ񾭹��ߵĸı��
					{
						line=((Line)this.arrLineList[i]);
						for(int j=0;j<line.LineNodeCount;j+=2)
						{
							int x=line.GetLineNodeInfo(j);
							int y=line.GetLineNodeInfo(j+1);
							if(e.X>x-5 && e.X<x+5 && e.Y>y-5 && e.Y<y+5)//����Ƿ񾭹��ߵĸı��
							{
								this.control.Cursor=Cursors.Hand;
								this.iTempNodeIndex=j/2;
								this.lineTempLine=line;
								return;
							}
							else//����ڿհ�����
							{
//								this.control.Cursor=Cursors.Default;
								this.ShowCursor();
								this.lineTempLine=null;
								this.iTempNodeIndex=99;
							}
						}
					}
				}
			}
		}

		public void MouseUp(System.Windows.Forms.MouseEventArgs e)
		{
			this.control.Cursor=Cursors.Default;
//			this.ShowCursor();
			if(e.Button==MouseButtons.Left)
			{
				GC.Collect();
				if(this.bSelectAllReday)
				{
					this.reDrawBitmap(this.graDrawPanel,10,10);
					this.RefreshBackground();
					this.control.Invalidate();
					this.bSelectAllReday=false;
				}
				else if(this.lineSelectLine!=null && this.iSelectNodeIndex!=99)//���ڱ༭ѡ�е��ߣ����ͣ�������
				{	
					if(this.lineSelectLine.ObjectType==Line.DrawObjectType.DrawBeeLine)
					{
						switch(this.iSelectNodeIndex)
						{
							case 0:
							{
								this.lineSelectLine.setPoint(this.iSelectNodeIndex,this.iLineFirstNodeX,this.iLineFirstNodeY);
								break;
							}
							case 1:	
							{
								this.lineSelectLine.setPoint(this.iSelectNodeIndex,this.iLineSecondNodeX,this.iLineSecondNodeY);
								break;					
							}
						}
						this.graDrawPanel.DrawLine(this.penDrawLine,this.iLineFirstNodeX,this.iLineFirstNodeY,this.iLineSecondNodeX,this.iLineSecondNodeY);
						this.control.testDrawLength.Text=this.lineSelectLine.Content;
						int stringleft=(this.iLineFirstNodeX+this.iLineSecondNodeX)/2-this.control.testDrawLength.Width/2+3;
						this.graDrawPanel.DrawString(this.lineSelectLine.Content,this.lineSelectLine.LineTextFont,new SolidBrush(this.lineSelectLine.LineColor),stringleft,(this.iLineFirstNodeY+this.iLineSecondNodeY)/2);
					}
					else
					{
						this.lineSelectLine.setPoint(0,this.iLineFirstNodeX,this.iLineFirstNodeY);
						this.lineSelectLine.setPoint(1,this.iLineSecondNodeX,this.iLineSecondNodeY);
						this.lineSelectLine.setPoint(2,this.iLine3thNodeX,this.iLine3thNodeY);
						this.lineSelectLine.setPoint(3,this.iLine4thNodeX,this.iLine4thNodeY);
						this.graDrawPanel.DrawLine(this.penDrawBeeLine,this.iLineFirstNodeX,this.iLineFirstNodeY,this.iLineSecondNodeX,this.iLineSecondNodeY);
						this.graDrawPanel.DrawLine(this.penDrawBeeLine,this.iLineSecondNodeX,this.iLineSecondNodeY,this.iLine3thNodeX,this.iLine3thNodeY);
						this.graDrawPanel.DrawLine(this.penDrawLine,this.iLine3thNodeX,this.iLine3thNodeY,this.iLine4thNodeX,this.iLine4thNodeY);
						this.control.testDrawLength.Text=this.lineSelectLine.Content;
						int stringleft=(this.iLineSecondNodeX+this.iLine3thNodeX)/2-this.control.testDrawLength.Width/2+3;
						this.graDrawPanel.DrawString(this.lineSelectLine.Content,this.lineSelectLine.LineTextFont,new SolidBrush(this.lineSelectLine.LineColor),stringleft,(this.iLineSecondNodeY+this.iLine3thNodeY)/2);
					}
					this.iSelectNodeIndex=99;
					this.lastEdit=this.lineSelectLine;
					this.AttriShow();
					this.ReflashNodeIn_OutNodeID();
					if(this.lineSelectLine.ObjectType==Line.DrawObjectType.DrawBeeLine)					{						this.iLine3thNodeX=this.iLineSecondNodeX;						this.iLine4thNodeY=this.iLine3thNodeY;					}					else					{						if(this.lineSelectLine.Modality)						{							this.iLine3thNodeX=this.iLineSecondNodeX;							this.iLine4thNodeY=this.iLine3thNodeY;													}					}
					this.lineSelectLine=null;
					this.bDrawLine=false;
					this.RefreshBackground();
					ReflashNodeIn_OutNodeID();
					GetTableList();
				}
				else if(this.lineSelectLine!=null && this.iSelectNodeIndex==99)//�ƶ��ߣ�ʧȥ����  �ػ���������
				{
					this.penDrawLine.Color=this.lineSelectLine.LineColor;
					this.penDrawLine.Width=this.lineSelectLine.LineSize;
					if(this.lineSelectLine.ObjectType==Line.DrawObjectType.DrawBeeLine)
					{
						this.lineSelectLine.setPoint(0,this.iLineFirstNodeX,this.iLineFirstNodeY);
						this.lineSelectLine.setPoint(1,this.iLineSecondNodeX,this.iLineSecondNodeY);
						this.graDrawPanel.DrawLine(this.penDrawLine,this.iLineFirstNodeX,this.iLineFirstNodeY,this.iLineSecondNodeX,this.iLineSecondNodeY);
						this.control.testDrawLength.Text=this.lineSelectLine.Content;
						int stringleft=(this.iLineFirstNodeX+this.iLineSecondNodeX)/2-this.control.testDrawLength.Width/2+3;
						this.graDrawPanel.DrawString(this.lineSelectLine.Content,this.lineSelectLine.LineTextFont,new SolidBrush(this.lineSelectLine.LineColor),stringleft,(this.iLineFirstNodeY+this.iLineSecondNodeY)/2);
					}
					else
					{
						this.lineSelectLine.setPoint(0,this.iLineFirstNodeX,this.iLineFirstNodeY);
						this.lineSelectLine.setPoint(1,this.iLineSecondNodeX,this.iLineSecondNodeY);
						this.lineSelectLine.setPoint(2,this.iLine3thNodeX,this.iLine3thNodeY);
						this.lineSelectLine.setPoint(3,this.iLine4thNodeX,this.iLine4thNodeY);
						this.graDrawPanel.DrawLine(this.penDrawBeeLine,this.iLineFirstNodeX,this.iLineFirstNodeY,this.iLineSecondNodeX,this.iLineSecondNodeY);
						this.graDrawPanel.DrawLine(this.penDrawBeeLine,this.iLineSecondNodeX,this.iLineSecondNodeY,this.iLine3thNodeX,this.iLine3thNodeY);
						this.graDrawPanel.DrawLine(this.penDrawLine,this.iLine3thNodeX,this.iLine3thNodeY,this.iLine4thNodeX,this.iLine4thNodeY);
						this.control.testDrawLength.Text=this.lineSelectLine.Content;
						int stringleft=(this.iLineSecondNodeX+this.iLine3thNodeX)/2-this.control.testDrawLength.Width/2+3;
						this.graDrawPanel.DrawString(this.lineSelectLine.Content,this.lineSelectLine.LineTextFont,new SolidBrush(this.lineSelectLine.LineColor),stringleft,(this.iLineSecondNodeY+this.iLine3thNodeY)/2);
					}
					this.iSelectNodeIndex=99;
					this.lastEdit=this.lineSelectLine;
					this.AttriShow();
					this.ReflashNodeIn_OutNodeID();
					if(this.lineSelectLine.ObjectType==Line.DrawObjectType.DrawBeeLine)					{						this.iLine3thNodeX=this.iLineSecondNodeX;						this.iLine4thNodeY=this.iLine3thNodeY;					}					else					{						if(this.lineSelectLine.Modality)						{							this.iLine3thNodeX=this.iLineSecondNodeX;							this.iLine4thNodeY=this.iLine3thNodeY;													}					}
					this.lineSelectLine=null;
					this.bDrawLine=false;
					this.RefreshBackground();
					this.control.Invalidate();
					ReflashNodeIn_OutNodeID();
					GetTableList();
				}
				else if(this.nodeSelectNode!=null)//�ƶ��ڵ㣨ʧȥ����  �ػ���������  �����ӵ��ýڵ�����ػ�������
				{
					Line line;
					if(this.control.Cursor==Cursors.SizeAll)
					{
						int iMoveWidth=e.X-this.iMouseInitX;
						int iMoveHeight=e.Y-this.iMouseInitY;
						this.iNodeX+=iMoveWidth;
						this.iNodeY+=iMoveHeight;
					}

					for(int i=0;i<this.arrLineConnectNode.Count;i++)//���ӵ��ýڵ�����ػ�������
					{
						line=((Line)this.arrLineConnectNode[i]);
						this.penDrawLine.Width=line.LineSize;
						this.penDrawLine.Color=line.LineColor;
						if(line.ObjectType==Line.DrawObjectType.DrawBeeLine)
						{
							this.graDrawPanel.DrawLine(this.penDrawLine,line.X0,line.Y0,line.X1,line.Y1);
							this.control.testDrawLength.Text=line.Content;
							int stringleft=(line.X0+line.X1)/2-this.control.testDrawLength.Width/2+3;
							this.graDrawPanel.DrawString(line.Content,this.fontDrawString,new SolidBrush(line.LineColor),stringleft,(line.Y0+line.Y1)/2);
						}
						else
						{
							this.graDrawPanel.DrawLine(this.penDrawBeeLine,line.X0,line.Y0,line.X1,line.Y1);
							this.graDrawPanel.DrawLine(this.penDrawBeeLine,line.X1,line.Y1,line.X2,line.Y2);
							this.graDrawPanel.DrawLine(this.penDrawLine,line.X2,line.Y2,line.X3,line.Y3);
							this.control.testDrawLength.Text=line.Content;
							int stringleft=(line.X1+line.X2)/2-this.control.testDrawLength.Width/2+3;
							this.graDrawPanel.DrawString(line.Content,line.LineTextFont,new SolidBrush(line.LineColor),stringleft,(line.Y1+line.Y2)/2);
						}
					}
					this.arrLineConnectNode.Clear();
					this.nodeSelectNode.X=this.iNodeX;
					this.nodeSelectNode.Y=this.iNodeY;
					this.image=this.getTypeImage.GetImage(nodeSelectNode.ObjectType);
					if(this.image!=null)
					{
						DrawImage(graDrawPanel,this.image,this.iNodeX,this.iNodeY,this.iNodeWidth,this.iNodeHeight);
					}
					else
					{
						if(this.nodeSelectNode.ObjectType==Node.DrawObjectType.DrawRectangle)
						{
							this.penDrawNode.Color=this.nodeSelectNode.BorderColor;
							this.graDrawPanel.DrawRectangle(this.penDrawNode,this.iNodeX,this.iNodeY,this.iNodeWidth,this.iNodeHeight);
							this.penDrawNode.Color=this.nodeSelectNode.FillColor;
							this.graDrawPanel.FillRectangle(this.penDrawNode.Brush,this.iNodeX,this.iNodeY,this.iNodeWidth,this.iNodeHeight);
						}
						else
						{
							this.penDrawNode.Color=this.nodeSelectNode.BorderColor;
							this.graDrawPanel.DrawEllipse(this.penDrawNode,this.iNodeX,this.iNodeY,this.iNodeWidth,this.iNodeHeight);
							this.penDrawNode.Color=this.nodeSelectNode.FillColor;
							this.graDrawPanel.FillEllipse(this.penDrawNode.Brush,this.iNodeX,this.iNodeY,this.iNodeWidth,this.iNodeHeight);							
						}
					}					
					//this.graDrawPanel.DrawString(this.nodeSelectNode.NodeText,new Font("����",10),new SolidBrush(Color.Black),this.iNodeX,this.iNodeY);
					this.control.testDrawLength.Font=this.nodeSelectNode.NodeTextFont;
					this.control.testDrawLength.Text=this.nodeSelectNode.NodeText;
					int stringX = iNodeX+iNodeWidth/2-this.control.testDrawLength.Width/2+3;
					this.graDrawPanel.DrawString(this.nodeSelectNode.NodeText,this.nodeSelectNode.NodeTextFont,new SolidBrush(this.nodeSelectNode.NodeTextColor),stringX,this.iNodeY+this.iNodeHeight);
					this.RefreshBackground();
					this.lastEdit=this.nodeSelectNode;
					this.AttriShow();
					this.nodeSelectNode=null;
				}
				else if(this.drawStringSelectDS!=null)
				{
					this.drawStringSelectDS.X=this.iDrawStringX;
					this.drawStringSelectDS.Y=this.iDrawStringY;
					this.drawStringSelectDS.Width=this.iDrawStringWidth;
					this.drawStringSelectDS.Height=this.iDrawStringHeight;
					int width,height;
					if(this.iDrawStringWidth<=2)
					{
						width=2;
					}
					else 
					{
						width=this.iDrawStringWidth;
					}

					if(this.iDrawStringHeight<=2)
					{
						height=2;
					}
					else
					{
						height=this.iDrawStringHeight;
					}
					this.re=new RectangleF(this.iDrawStringX,this.iDrawStringY,width,height);
					this.penDrawString.Color=this.drawStringSelectDS.DSTextColor;
					this.graDrawPanel.DrawString(this.strDSContent,this.drawStringSelectDS.DSTextFont,this.penDrawString.Brush,this.re);
					this.RefreshBackground();
					this.lastEdit=this.drawStringSelectDS;
					this.AttriShow();
					this.drawStringSelectDS=null;
				}
				else if(this.lineThisLine!=null)//�����������ߣ�����
				{
					//					this.iLineSecondNodeX=this.iLineSecondNodeX;
					//					this.iLineSecondNodeY=this.iLineSecondNodeY;
					this.AutoConnectNode(this.iLineFirstNodeX,this.iLineFirstNodeY,0);
					this.penDrawLine.Color=this.lineThisLine.LineColor;
					this.penDrawLine.Width=this.lineThisLine.LineSize;
					if(this.lineThisLine.ObjectType==Line.DrawObjectType.DrawBeeLine)
					{
						this.AutoConnectNode(this.iLineSecondNodeX,this.iLineSecondNodeY,1);
						this.lineThisLine.setPoint(0,this.iLineFirstNodeX,this.iLineFirstNodeY);
						this.lineThisLine.addPoint(this.iLineSecondNodeX,this.iLineSecondNodeY);
						this.lineThisLine.LineNodeCount=4;
						this.selectPoint.SetLinePoint(this.lineThisLine.X0,this.lineThisLine.Y0,this.lineThisLine.X1,this.lineThisLine.Y1);
						this.graDrawPanel.DrawLine(this.penDrawLine,this.iLineFirstNodeX,this.iLineFirstNodeY,this.lineThisLine.X1,this.lineThisLine.Y1);
						this.control.testDrawLength.Text=this.lineThisLine.Content;
						int stringleft=(this.iLineFirstNodeX+this.iLineSecondNodeX)/2-this.control.testDrawLength.Width/2+3;
						this.graDrawPanel.DrawString(this.lineThisLine.Content,this.lineThisLine.LineTextFont,new SolidBrush(this.lineThisLine.LineColor),stringleft,(this.iLineFirstNodeY+this.iLineSecondNodeY)/2);
					}
					else
					{
						this.AutoConnectNode(this.iLine4thNodeX,this.iLine4thNodeY,3);
						this.iLineSecondNodeY=this.iLineFirstNodeY;
						this.lineThisLine.setPoint(0,this.iLineFirstNodeX,this.iLineFirstNodeY);
						this.lineThisLine.addPoint(this.iLineSecondNodeX,this.iLineSecondNodeY);
						this.lineThisLine.LineNodeCount=4;
						this.lineThisLine.addPoint(this.iLine3thNodeX,this.iLine3thNodeY);
						this.lineThisLine.LineNodeCount=6;
						this.lineThisLine.addPoint(this.iLine4thNodeX,this.iLine4thNodeY);
						this.lineThisLine.LineNodeCount=8;
						this.graDrawPanel.DrawLine(this.penDrawBeeLine,this.iLineFirstNodeX,this.iLineFirstNodeY,this.iLineSecondNodeX,this.iLineSecondNodeY);
						this.graDrawPanel.DrawLine(this.penDrawBeeLine,this.iLineSecondNodeX,this.iLineSecondNodeY,this.iLine3thNodeX,this.iLine3thNodeY);
						this.graDrawPanel.DrawLine(this.penDrawLine,this.iLine3thNodeX,this.iLine3thNodeY,this.iLine4thNodeX,this.iLine4thNodeY);
						this.control.testDrawLength.Text=this.lineThisLine.Content;
						int stringleft=(this.iLineSecondNodeX+this.iLine3thNodeX)/2-this.control.testDrawLength.Width/2+3;
						this.graDrawPanel.DrawString(this.lineThisLine.Content,this.lineThisLine.LineTextFont,new SolidBrush(this.lineThisLine.LineColor),stringleft,(this.iLineSecondNodeY+this.iLine3thNodeY)/2);
					}
					
					this.lastEdit=this.lineThisLine;
					this.AttriShow();
					this.ReflashNodeIn_OutNodeID();
					if(this.lineThisLine.ObjectType==Line.DrawObjectType.DrawBeeLine)					{						this.iLine3thNodeX=this.iLineSecondNodeX;						this.iLine4thNodeY=this.iLine3thNodeY;					}					else					{						if(this.lineThisLine.Modality)						{							this.iLine3thNodeX=this.iLineSecondNodeX;							this.iLine4thNodeY=this.iLine3thNodeY;													}					}
					this.lineThisLine=null;
					this.bDrawLine=false;
					this.RefreshBackground();
					ReflashNodeIn_OutNodeID();
					GetTableList();
				}
				else if(this.nodeThisNode!=null)//���������½ڵ㣬����
				{
					this.nodeThisNode.X=this.iNodeX;					this.nodeThisNode.Y=this.iNodeY;					this.nodeThisNode.Width=this.iNodeWidth;					this.nodeThisNode.Height=this.iNodeHeight;
					if(this.nodeThisNode.ObjectType==Node.DrawObjectType.DrawNodeBegin)
					{
						this.bStartFlag=true;
					}
					if(this.nodeThisNode.ObjectType==Node.DrawObjectType.DrawNodeEnd)
					{
						this.bEndFlag=true;
					}
					else if(this.nodeThisNode.ObjectType==Node.DrawObjectType.DrawNodeGeneral)
					{
						this.i_GeneralCount++;
					}

					this.image=this.getTypeImage.GetImage(this.currentObjectType);
					if(this.image!=null)
					{
						DrawImage(graDrawPanel,this.image,this.iNodeX,this.iNodeY,this.iNodeWidth,this.iNodeHeight);
					}
					else
					{
						if(this.nodeThisNode.ObjectType==Node.DrawObjectType.DrawRectangle)
						{
							this.penDrawNode.Color=this.nodeThisNode.BorderColor;
							this.graDrawPanel.DrawRectangle(this.penDrawNode,this.iNodeX,this.iNodeY,this.iNodeWidth,this.iNodeHeight);
							this.penDrawNode.Color=this.nodeThisNode.FillColor;
							this.graDrawPanel.FillRectangle(this.penDrawNode.Brush,this.iNodeX,this.iNodeY,this.iNodeWidth,this.iNodeHeight);
						}
						else
						{
							this.penDrawNode.Color=this.nodeThisNode.BorderColor;
							this.graDrawPanel.DrawEllipse(this.penDrawNode,this.iNodeX,this.iNodeY,this.iNodeWidth,this.iNodeHeight);
							this.penDrawNode.Color=this.nodeThisNode.FillColor;
							this.graDrawPanel.FillEllipse(this.penDrawNode.Brush,this.iNodeX,this.iNodeY,this.iNodeWidth,this.iNodeHeight);							
						}
					} 
					//this.graDrawPanel.DrawString(this.getDefaultText(this.currentObjectType),new Font("����",10),new SolidBrush(Color.Black),this.iNodeX,this.iNodeY);
					this.control.testDrawLength.Font=this.nodeThisNode.NodeTextFont;
					this.control.testDrawLength.Text=this.nodeThisNode.NodeText;
					int stringleft=iNodeX+iNodeWidth/2-this.control.testDrawLength.Width/2+3;
					this.graDrawPanel.DrawString(this.nodeThisNode.NodeText,this.nodeThisNode.NodeTextFont,new SolidBrush(this.nodeThisNode.NodeTextColor),stringleft,this.iNodeY+this.iNodeHeight);
					this.RefreshBackground();
					this.lastEdit=this.nodeThisNode;
					this.AttriShow();
					this.nodeThisNode=null;
				}
				else if(this.drawStringThisDS!=null)
				{
					this.drawStringThisDS.X=this.iDrawStringX;
					this.drawStringThisDS.Y=this.iDrawStringY;
					this.drawStringThisDS.Width=this.iDrawStringWidth;
					this.drawStringThisDS.Height=this.iDrawStringHeight;
					int width,height;
					if(this.iDrawStringWidth<=2)
					{
						width=2;
					}
					else 
					{
						width=this.iDrawStringWidth;
					}

					if(this.iDrawStringHeight<=2)
					{
						height=2;
					}
					else
					{
						height=this.iDrawStringHeight;
					}
					this.re=new RectangleF(this.iDrawStringX,this.iDrawStringY,width,height);
					this.penDrawString.Color=this.drawStringThisDS.DSTextColor;
					this.graDrawPanel.DrawString(this.drawStringThisDS.Content,this.drawStringThisDS.DSTextFont,this.penDrawString.Brush,this.re);
					this.RefreshBackground();
					this.lastEdit=this.drawStringThisDS;
					this.AttriShow();
					this.drawStringThisDS=null;
				}
				else if(this.bSelectRectangleReday)
				{
					this.bSelectRectangleReday=false;
					this.reDrawBitmap(this.graDrawPanel,10,10);
					this.RefreshBackground();
					//					this.control.Invalidate();
					this.ClearLineConnectNode();
					this.ClearNodeConnectLine();
					this.arrLineSelectList.Clear();
					this.arrNodeSelectList.Clear();
					this.arrDrawStringSelectList.Clear();
					this.arrLineNotSelectList.Clear();
					this.arrNodeNotSelectList.Clear();
					this.arrDrawStringNotSelectList.Clear();
					this.ReflashNodeIn_OutNodeID();
				}
				else if(this.bNotSelectAnyOne && this.bIsAtSelect)
				{
					this.RegionSelect(this.iMouseSelectX,this.iMouseSelectY,this.iMouseSelectWidth,this.iMouseSelectHeight);
					this.ifSelectAngle();
					this.bNotSelectAnyOne=false;
					this.bDrawSelectRectangle=false;
					this.bDrawSelectElement=true;
					this.bIsAtSelect=false;
					if(this.arrLineSelectList.Count>0 || this.arrNodeSelectList.Count>0 ||this.arrDrawStringSelectList.Count>0)
					{
						this.bSelectRectangleReday=true;
					}
					this.control.Invalidate();
				}
			}		
		}
		#endregion 
		
		#region ��̬ͼ���Ʋ���
		public void AutoRepaintForm(Graphics gra)
		{
			//�ػ���ڵ�
			if(this.bDrawNode)
			{
				Graphics g=gra;
				g.SmoothingMode=SmoothingMode.AntiAlias;
				if(this.nodeSelectNode!=null)
				{
					this.image=this.getTypeImage.GetImage(this.nodeSelectNode.ObjectType);
					if(this.image!=null)
					{
						DrawImage(g,this.image,this.iNodeX,this.iNodeY,this.iNodeWidth,this.iNodeHeight);
					}
					else 
					{
						if(this.nodeSelectNode.ObjectType==Node.DrawObjectType.DrawRectangle)
						{
							this.penDrawNode.Color=this.nodeSelectNode.BorderColor;
							g.DrawRectangle(this.penDrawNode,this.iNodeX,this.iNodeY,this.iNodeWidth,this.iNodeHeight);
							this.penDrawNode.Color=this.nodeSelectNode.FillColor;
							g.FillRectangle(this.penDrawNode.Brush,this.iNodeX,this.iNodeY,this.iNodeWidth,this.iNodeHeight);
						}
						else
						{
							this.penDrawNode.Color=this.nodeSelectNode.BorderColor;
							g.DrawEllipse(this.penDrawNode,this.iNodeX,this.iNodeY,this.iNodeWidth,this.iNodeHeight);
							this.penDrawNode.Color=this.nodeSelectNode.FillColor;
							g.FillEllipse(this.penDrawNode.Brush,this.iNodeX,this.iNodeY,this.iNodeWidth,this.iNodeHeight);							
						}
					}
					//g.DrawImage(GetTypeImage(this.nodeSelectNode.ObjectType),this.iNodeX,this.iNodeY,this.iNodeWidth,this.iNodeHeight);
					//g.DrawString(this.nodeSelectNode.NodeText,new Font("����",10),new SolidBrush(Color.Black),this.iNodeX,this.iNodeY);
					this.control.testDrawLength.Font=this.nodeSelectNode.NodeTextFont;
					this.control.testDrawLength.Text=this.nodeSelectNode.NodeText;
					int stringleft=iNodeX+iNodeWidth/2-this.control.testDrawLength.Width/2+3;
					g.DrawString(this.nodeSelectNode.NodeText,this.nodeSelectNode.NodeTextFont,new SolidBrush(this.nodeSelectNode.NodeTextColor),stringleft,this.iNodeY+this.iNodeHeight);
				}
				else
				{
					if(this.nodeThisNode!=null)
					{
						this.image=this.getTypeImage.GetImage(this.nodeThisNode.ObjectType);
						if(this.image!=null)
						{
							DrawImage(g,this.image,this.iNodeX,this.iNodeY,this.iNodeWidth,this.iNodeHeight);
						}
						else 
						{
							if(this.nodeThisNode.ObjectType==Node.DrawObjectType.DrawRectangle)
							{
								this.penDrawNode.Color=this.nodeThisNode.BorderColor;
								g.DrawRectangle(this.penDrawNode,this.iNodeX,this.iNodeY,this.iNodeWidth,this.iNodeHeight);
								this.penDrawNode.Color=this.nodeThisNode.FillColor;
								g.FillRectangle(this.penDrawNode.Brush,this.iNodeX,this.iNodeY,this.iNodeWidth,this.iNodeHeight);
							}
							else
							{
								this.penDrawNode.Color=this.nodeThisNode.BorderColor;
								g.DrawEllipse(this.penDrawNode,this.iNodeX,this.iNodeY,this.iNodeWidth,this.iNodeHeight);
								this.penDrawNode.Color=this.nodeThisNode.FillColor;
								g.FillEllipse(this.penDrawNode.Brush,this.iNodeX,this.iNodeY,this.iNodeWidth,this.iNodeHeight);								
							}
						}
						//g.DrawImage(GetTypeImage(currentObjectType),this.iNodeX,this.iNodeY,this.iNodeWidth,this.iNodeHeight);
						//g.DrawString(this.nodeThisNode.NodeText,new Font("����",10),new SolidBrush(Color.Black),this.iNodeX,this.iNodeY);
						this.control.testDrawLength.Font=this.nodeThisNode.NodeTextFont;
						this.control.testDrawLength.Text=this.nodeThisNode.NodeText;
						int stringleft=iNodeX+iNodeWidth/2-this.control.testDrawLength.Width/2+3;
						g.DrawString(this.nodeThisNode.NodeText,this.nodeThisNode.NodeTextFont,new SolidBrush(this.nodeThisNode.NodeTextColor),stringleft,this.iNodeY+this.iNodeHeight);
					}
				}
				for(int i=0;i<8;i++)
				{
					g.DrawLine(this.penDrawPoint,this.selectPoint.RectanglePoint[i].X,this.selectPoint.RectanglePoint[i].Y,this.selectPoint.RectanglePoint[i].X,this.selectPoint.RectanglePoint[i].Y+6);
				}
				this.bDrawNode=false;
			}

			//д�ְ��ػ�
			if(this.bDrawString)
			{
				Graphics g=gra;
				int width,height;
				if(this.iDrawStringWidth<=2)
				{
					width=2;
				}
				else 
				{
					width=this.iDrawStringWidth;
				}

				if(this.iDrawStringHeight<=2)
				{
					height=2;
				}
				else
				{
					height=this.iDrawStringHeight;
				}
				this.re=new RectangleF(this.iDrawStringX,this.iDrawStringY,width,height);
				if(this.drawStringThisDS!=null)
				{
					this.penDrawString.Color=this.drawStringThisDS.DSTextColor;
					g.DrawString(this.strDSContent,this.drawStringThisDS.DSTextFont,this.penDrawString.Brush,this.re);
				}
				else if(this.drawStringSelectDS!=null)
				{
					this.penDrawString.Color=this.drawStringSelectDS.DSTextColor;
					g.DrawString(this.strDSContent,this.drawStringSelectDS.DSTextFont,this.penDrawString.Brush,this.re);
				}
				for(int i=0;i<8;i++)
				{
					g.DrawLine(this.penDrawString,this.selectPoint.RectanglePoint[i].X,this.selectPoint.RectanglePoint[i].Y,this.selectPoint.RectanglePoint[i].X,this.selectPoint.RectanglePoint[i].Y+6);
				}
				this.bDrawString=false;
				return;
			}

			//�ػ���ڵ�����ӵ���������ߣ��ƶ��ڵ㣩
			if(this.bDrawConnectNodeLine)
			{
				Graphics g=gra;
				Line line;
				g.SmoothingMode=SmoothingMode.AntiAlias;
				for(int i=0;i<this.arrLineConnectNode.Count;i++)
				{
					line=((Line)this.arrLineConnectNode[i]);
					this.penDrawLine.Width=line.LineSize;
					this.penDrawLine.Color=line.LineColor;
					if(line.ObjectType==Line.DrawObjectType.DrawBeeLine)
					{
						g.DrawLine(this.penDrawLine,line.X0,line.Y0,line.X1,line.Y1);
						this.control.testDrawLength.Text=line.Content;
						int stringleft=(line.X0+line.X1)/2-this.control.testDrawLength.Width/2+3;
						g.DrawString(line.Content,this.fontDrawString,new SolidBrush(line.LineColor),stringleft,(line.Y0+line.Y1)/2);
					}
					else
					{
						this.penDrawBeeLine.Color=line.LineColor;
						this.penDrawBeeLine.Width=line.LineSize;
						g.DrawLine(this.penDrawBeeLine,line.X0,line.Y0,line.X1,line.Y1);
						g.DrawLine(this.penDrawBeeLine,line.X1,line.Y1,line.X2,line.Y2);
						g.DrawLine(this.penDrawLine,line.X2,line.Y2,line.X3,line.Y3);
						this.control.testDrawLength.Text=line.Content;
						int stringleft=(line.X1+line.X2)/2-this.control.testDrawLength.Width/2+3;
						g.DrawString(line.Content,line.LineTextFont,new SolidBrush(line.LineColor),stringleft,(line.Y1+line.Y2)/2);
					}
				}
				this.bDrawConnectNodeLine=false;
				return;
			}

			//�ػ����
			if(this.bDrawLine)
			{
				Graphics g=gra;
				g.SmoothingMode=SmoothingMode.AntiAlias;
				if(this.iAutoConnectLinePointIndex==0)
				{
					this.penDrawPoint.Color=Color.Red;
					g.DrawLine(this.penDrawPoint,this.selectPoint.LinePoint[0].X-3,this.selectPoint.LinePoint[0].Y,this.selectPoint.LinePoint[0].X+3,this.selectPoint.LinePoint[0].Y);
					this.penDrawPoint.Color=Color.Black;
					this.iAutoConnectLinePointIndex=9;
				}
				else
				{
					g.DrawLine(this.penDrawPoint,this.selectPoint.LinePoint[0].X-3,this.selectPoint.LinePoint[0].Y,this.selectPoint.LinePoint[0].X+3,this.selectPoint.LinePoint[0].Y);
				}
				if(this.iAutoConnectLinePointIndex==1)
				{
					this.penDrawPoint.Color=Color.Red;
					g.DrawLine(this.penDrawPoint,this.selectPoint.LinePoint[1].X-3,this.selectPoint.LinePoint[1].Y,this.selectPoint.LinePoint[1].X+3,this.selectPoint.LinePoint[1].Y);
					this.penDrawPoint.Color=Color.Black;
					this.iAutoConnectLinePointIndex=9;
				}
				else
				{
					g.DrawLine(this.penDrawPoint,this.selectPoint.LinePoint[1].X-3,this.selectPoint.LinePoint[1].Y,this.selectPoint.LinePoint[1].X+3,this.selectPoint.LinePoint[1].Y);
				}
				if(this.lineSelectLine!=null)
				{
					this.penDrawLine.Color=this.lineSelectLine.LineColor;
					this.penDrawLine.Width=this.lineSelectLine.LineSize;
					if(this.lineSelectLine.ObjectType==Line.DrawObjectType.DrawBeeLine)
					{
						g.DrawLine(this.penDrawLine,this.iLineFirstNodeX,this.iLineFirstNodeY,this.iLineSecondNodeX,this.iLineSecondNodeY);
						this.control.testDrawLength.Text=this.lineSelectLine.Content;
						int stringleft=(this.iLineFirstNodeX+this.iLineSecondNodeX)/2-this.control.testDrawLength.Width/2+3;
						g.DrawString(this.lineSelectLine.Content,this.lineSelectLine.LineTextFont,new SolidBrush(this.lineSelectLine.LineColor),stringleft,(this.iLineFirstNodeY+this.iLineSecondNodeY)/2);
					}
					else
					{
						this.penDrawBeeLine.Color=this.lineSelectLine.LineColor;
						this.penDrawBeeLine.Width=this.lineSelectLine.LineSize;
						g.DrawLine(this.penDrawBeeLine,this.iLineFirstNodeX,this.iLineFirstNodeY,this.iLineSecondNodeX,this.iLineSecondNodeY);
						g.DrawLine(this.penDrawBeeLine,this.iLineSecondNodeX,this.iLineSecondNodeY,this.iLine3thNodeX,this.iLine3thNodeY);
						g.DrawLine(this.penDrawLine,this.iLine3thNodeX,this.iLine3thNodeY,this.iLine4thNodeX,this.iLine4thNodeY);
						this.control.testDrawLength.Text=this.lineSelectLine.Content;
						int stringleft=(this.iLineSecondNodeX+this.iLine3thNodeX)/2-this.control.testDrawLength.Width/2+3;
						g.DrawString(this.lineSelectLine.Content,this.lineSelectLine.LineTextFont,new SolidBrush(this.lineSelectLine.LineColor),stringleft,(this.iLineSecondNodeY+this.iLine3thNodeY)/2);
						g.DrawLine(this.penDrawPoint,this.selectPoint.LinePoint[2].X-3,this.selectPoint.LinePoint[2].Y,this.selectPoint.LinePoint[2].X+3,this.selectPoint.LinePoint[2].Y);
						if(this.iAutoConnectLinePointIndex==3)
						{
							this.penDrawPoint.Color=Color.Red;
							g.DrawLine(this.penDrawPoint,this.selectPoint.LinePoint[3].X-3,this.selectPoint.LinePoint[3].Y,this.selectPoint.LinePoint[3].X+3,this.selectPoint.LinePoint[3].Y);
							this.penDrawPoint.Color=Color.Black;
							this.iAutoConnectLinePointIndex=9;
						}
						else
						{
							g.DrawLine(this.penDrawPoint,this.selectPoint.LinePoint[3].X-3,this.selectPoint.LinePoint[3].Y,this.selectPoint.LinePoint[3].X+3,this.selectPoint.LinePoint[3].Y);
						}
					}
				}
				else if(this.lineThisLine!=null)
				{
					this.penDrawLine.Color=this.lineThisLine.LineColor;
					this.penDrawLine.Width=this.lineThisLine.LineSize;
					if(this.lineThisLine.ObjectType==Line.DrawObjectType.DrawBeeLine)
					{
						g.DrawLine(this.penDrawLine,this.iLineFirstNodeX,this.iLineFirstNodeY,this.iLineSecondNodeX,this.iLineSecondNodeY);
						this.control.testDrawLength.Text=this.lineThisLine.Content;
						int stringleft=(this.iLineFirstNodeX+this.iLineSecondNodeX)/2-this.control.testDrawLength.Width/2+3;
						g.DrawString(this.lineThisLine.Content,this.lineThisLine.LineTextFont,new SolidBrush(this.lineThisLine.LineColor),stringleft,(this.iLineFirstNodeY+this.iLineSecondNodeY)/2);
					}
					else
					{
						this.penDrawBeeLine.Color=this.lineThisLine.LineColor;
						this.penDrawBeeLine.Width=this.lineThisLine.LineSize;
						g.DrawLine(this.penDrawBeeLine,this.iLineFirstNodeX,this.iLineFirstNodeY,this.iLineSecondNodeX,this.iLineSecondNodeY);
						g.DrawLine(this.penDrawBeeLine,this.iLineSecondNodeX,this.iLineSecondNodeY,this.iLine3thNodeX,this.iLine3thNodeY);
						g.DrawLine(this.penDrawLine,this.iLine3thNodeX,this.iLine3thNodeY,this.iLine4thNodeX,this.iLine4thNodeY);
						this.control.testDrawLength.Text=this.lineThisLine.Content;
						int stringleft=(this.iLineSecondNodeX+this.iLine3thNodeX)/2-this.control.testDrawLength.Width/2+3;
						g.DrawString(this.lineThisLine.Content,this.lineThisLine.LineTextFont,new SolidBrush(this.lineThisLine.LineColor),stringleft,(this.iLineSecondNodeY+this.iLine3thNodeY)/2);

						g.DrawLine(this.penDrawPoint,this.selectPoint.LinePoint[2].X-3,this.selectPoint.LinePoint[2].Y,this.selectPoint.LinePoint[2].X+3,this.selectPoint.LinePoint[2].Y);
						if(this.iAutoConnectLinePointIndex==3)
						{
							this.penDrawPoint.Color=Color.Red;
							g.DrawLine(this.penDrawPoint,this.selectPoint.LinePoint[3].X-3,this.selectPoint.LinePoint[3].Y,this.selectPoint.LinePoint[3].X+3,this.selectPoint.LinePoint[3].Y);
							this.penDrawPoint.Color=Color.Black;
							this.iAutoConnectLinePointIndex=9;
						}
						else
						{
							g.DrawLine(this.penDrawPoint,this.selectPoint.LinePoint[3].X-3,this.selectPoint.LinePoint[3].Y,this.selectPoint.LinePoint[3].X+3,this.selectPoint.LinePoint[3].Y);
						}
					}
				}
				//				g.DrawLine(this.penDrawLine,this.iLineFirstNodeX,this.iLineFirstNodeY,this.iLineSecondNodeX,this.iLineSecondNodeY);
				//����ѡ����ʾ��

				//				if(this.lineSelectLine!=null)
				//				{
				//					if(this.lineSelectLine.ObjectType==Line.DrawObjectType.DrawFoldLine)
				//					{
				//						g.DrawLine(this.penDrawPoint,this.selectPoint.LinePoint[2].X-3,this.selectPoint.LinePoint[2].Y,this.selectPoint.LinePoint[2].X+3,this.selectPoint.LinePoint[2].Y);
				//						g.DrawLine(this.penDrawPoint,this.selectPoint.LinePoint[3].X-3,this.selectPoint.LinePoint[3].Y,this.selectPoint.LinePoint[3].X+3,this.selectPoint.LinePoint[3].Y);
				//					}
				//				}
				//				else if(this.lineThisLine!=null)
				//				{
				//					if(this.lineThisLine.ObjectType==Line.DrawObjectType.DrawFoldLine)
				//					{
				//						g.DrawLine(this.penDrawPoint,this.selectPoint.LinePoint[2].X-3,this.selectPoint.LinePoint[2].Y,this.selectPoint.LinePoint[2].X+3,this.selectPoint.LinePoint[2].Y);
				//						g.DrawLine(this.penDrawPoint,this.selectPoint.LinePoint[3].X-3,this.selectPoint.LinePoint[3].Y,this.selectPoint.LinePoint[3].X+3,this.selectPoint.LinePoint[3].Y);
				//					}
				//				}
				this.bDrawLine=false;
				return;
			}


			//ȫѡ�ƶ�
			if(this.bSelectAll)
			{
				Graphics g=gra;
				g.SmoothingMode=SmoothingMode.AntiAlias;
				Line line;
				Node node;
				DrawString drawString;
				for(int i=0;i<this.arrLineList.Count;i++)
				{
					line=(Line)this.arrLineList[i];
					this.penDrawLine.Color=line.LineColor;
					this.penDrawLine.Width=line.LineSize;
					if(line.ObjectType==Line.DrawObjectType.DrawBeeLine)
					{
						g.DrawLine(this.penDrawLine,line.X0,line.Y0,line.X1,line.Y1);
						g.DrawLine(this.penDrawPoint,line.X0-3,line.Y0,line.X0+3,line.Y0);
						g.DrawLine(this.penDrawPoint,line.X1-3,line.Y1,line.X1+3,line.Y1);
						this.control.testDrawLength.Text=line.Content;
						int stringleft=(line.X0+line.X1)/2-this.control.testDrawLength.Width/2+3;
						g.DrawString(line.Content,line.LineTextFont,new SolidBrush(line.LineColor),stringleft,(line.Y0+line.Y1)/2);
					}
					else
					{
						this.penDrawBeeLine.Color=line.LineColor;
						this.penDrawBeeLine.Width=line.LineSize;
						g.DrawLine(this.penDrawBeeLine,line.X0,line.Y0,line.X1,line.Y1);
						g.DrawLine(this.penDrawBeeLine,line.X1,line.Y1,line.X2,line.Y2);
						g.DrawLine(this.penDrawLine,line.X2,line.Y2,line.X3,line.Y3);
						this.control.testDrawLength.Text=line.Content;
						int stringleft=(line.X1+line.X2)/2-this.control.testDrawLength.Width/2+3;
						g.DrawString(line.Content,line.LineTextFont,new SolidBrush(line.LineColor),stringleft,(line.Y1+line.Y2)/2);
						this.selectPoint.SetFlodLinePoint(line);
						g.DrawLine(this.penDrawPoint,this.selectPoint.LinePoint[0].X-3,this.selectPoint.LinePoint[0].Y,this.selectPoint.LinePoint[0].X+3,this.selectPoint.LinePoint[0].Y);
						g.DrawLine(this.penDrawPoint,this.selectPoint.LinePoint[1].X-3,this.selectPoint.LinePoint[1].Y,this.selectPoint.LinePoint[1].X+3,this.selectPoint.LinePoint[1].Y);
						g.DrawLine(this.penDrawPoint,this.selectPoint.LinePoint[2].X-3,this.selectPoint.LinePoint[2].Y,this.selectPoint.LinePoint[2].X+3,this.selectPoint.LinePoint[2].Y);
						g.DrawLine(this.penDrawPoint,this.selectPoint.LinePoint[3].X-3,this.selectPoint.LinePoint[3].Y,this.selectPoint.LinePoint[3].X+3,this.selectPoint.LinePoint[3].Y);
					}
				}
				for(int i=0;i<this.arrNodeList.Count;i++)
				{
					node=(Node)this.arrNodeList[i];
					this.selectPoint.SetRectanglePoint(node.X,node.Y,node.Width,node.Height);
					//g.DrawRectangle(this.penDrawNode,node.X,node.Y,node.Width,node.Height);
					this.image=this.getTypeImage.GetImage(node.ObjectType);
					if(this.image!=null)
					{
						DrawImage(g,this.image,node.X,node.Y,node.Width,node.Height);
					}
					else
					{
						if(node.ObjectType==Node.DrawObjectType.DrawRectangle)
						{
							this.penDrawNode.Color=node.BorderColor;
							g.DrawRectangle(this.penDrawNode,node.X,node.Y,node.Width,node.Height);
							this.penDrawNode.Color=node.FillColor;
							g.FillRectangle(this.penDrawNode.Brush,node.X,node.Y,node.Width,node.Height);
						}
						else
						{
							this.penDrawNode.Color=node.BorderColor;
							g.DrawEllipse(this.penDrawNode,node.X,node.Y,node.Width,node.Height);
							this.penDrawNode.Color=node.FillColor;
							g.FillEllipse(this.penDrawNode.Brush,node.X,node.Y,node.Width,node.Height);							
						}
					}
					this.control.testDrawLength.Text=node.NodeText;
					int stringleft=node.X+node.Width/2-this.control.testDrawLength.Width/2+3;
					g.DrawString(node.NodeText,node.NodeTextFont,new SolidBrush(node.NodeTextColor),stringleft,node.Y+node.Height);
					//					g.DrawString(node.NodeText,new Font("����",10),new SolidBrush(Color.Black),node.X,node.Y);
					for(int j=0;j<8;j++)
					{
						g.DrawLine(this.penDrawPoint,this.selectPoint.RectanglePoint[j].X,this.selectPoint.RectanglePoint[j].Y,this.selectPoint.RectanglePoint[j].X,this.selectPoint.RectanglePoint[j].Y+6);
					}
				}


				
				for(int i=0;i<this.arrDrawStringList.Count;i++)
				{
					drawString=(DrawString)this.arrDrawStringList[i];
					int width,height;
					if(drawString.Width<=2)
					{
						width=2;
					}
					else 
					{
						width=drawString.Width;
					}

					if(drawString.Height<=2)
					{
						height=2;
					}
					else
					{
						height=drawString.Height;
					}
					this.re=new RectangleF(drawString.X,drawString.Y,width,height);
					this.penDrawString.Color=drawString.DSTextColor;
					g.DrawString(drawString.Content,drawString.DSTextFont,this.penDrawString.Brush,this.re);
					this.selectPoint.SetRectanglePoint(drawString.X,drawString.Y,drawString.Width,drawString.Height);
					for(int j=0;j<8;j++)
					{
						g.DrawLine(this.penDrawPoint,this.selectPoint.RectanglePoint[j].X,this.selectPoint.RectanglePoint[j].Y,this.selectPoint.RectanglePoint[j].X,this.selectPoint.RectanglePoint[j].Y+6);
					}
				}
				this.bSelectAll=false;
				if(!this.bSelectAllReday)
				{
					this.bSelectAllReday=true;
				}
			}

			if(this.bDrawSelectElement)
			{
				Graphics g=gra;
				Line line;
				Node node;
				DrawString drawString;
				g.SmoothingMode=SmoothingMode.AntiAlias;
				for(int i=0;i<this.arrLineSelectList.Count;i++)
				{
					line=(Line)this.arrLineSelectList[i];
					this.selectPoint.SetLinePoint(line.X0,line.Y0,line.X1,line.Y1);
					this.penDrawLine.Color=line.LineColor;
					this.penDrawLine.Width=line.LineSize;
					if(line.ObjectType==Line.DrawObjectType.DrawBeeLine)
					{
						g.DrawLine(this.penDrawLine,line.X0,line.Y0,line.X1,line.Y1);
						g.DrawLine(this.penDrawPoint,line.X0,line.Y0,line.X0+6,line.Y0);
						g.DrawLine(this.penDrawPoint,line.X1,line.Y1,line.X1+6,line.Y1);
						this.control.testDrawLength.Text=line.Content;
						int stringleft=(line.X0+line.X1)/2-this.control.testDrawLength.Width/2+3;
						g.DrawString(line.Content,line.LineTextFont,new SolidBrush(line.LineColor),stringleft,(line.Y0+line.Y1)/2);
					}
					else
					{
						this.penDrawBeeLine.Color=line.LineColor;
						this.penDrawBeeLine.Width=line.LineSize;
						g.DrawLine(this.penDrawBeeLine,line.X0,line.Y0,line.X1,line.Y1);
						g.DrawLine(this.penDrawBeeLine,line.X1,line.Y1,line.X2,line.Y2);
						g.DrawLine(this.penDrawLine,line.X2,line.Y2,line.X3,line.Y3);
						this.control.testDrawLength.Text=line.Content;
						int stringleft=(line.X1+line.X2)/2-this.control.testDrawLength.Width/2+3;
						g.DrawString(line.Content,line.LineTextFont,new SolidBrush(line.LineColor),stringleft,(line.Y1+line.Y2)/2);
						this.selectPoint.SetFlodLinePoint(line);
						g.DrawLine(this.penDrawPoint,this.selectPoint.LinePoint[0].X-3,this.selectPoint.LinePoint[0].Y,this.selectPoint.LinePoint[0].X+3,this.selectPoint.LinePoint[0].Y);
						g.DrawLine(this.penDrawPoint,this.selectPoint.LinePoint[1].X-3,this.selectPoint.LinePoint[1].Y,this.selectPoint.LinePoint[1].X+3,this.selectPoint.LinePoint[1].Y);
						g.DrawLine(this.penDrawPoint,this.selectPoint.LinePoint[2].X-3,this.selectPoint.LinePoint[2].Y,this.selectPoint.LinePoint[2].X+3,this.selectPoint.LinePoint[2].Y);
						g.DrawLine(this.penDrawPoint,this.selectPoint.LinePoint[3].X-3,this.selectPoint.LinePoint[3].Y,this.selectPoint.LinePoint[3].X+3,this.selectPoint.LinePoint[3].Y);
					}
				}

				for(int i=0;i<this.arrNodeSelectList.Count;i++)
				{
					node=(Node)this.arrNodeSelectList[i];
					this.selectPoint.SetRectanglePoint(node.X,node.Y,node.Width,node.Height);
					//					g.DrawRectangle(this.penDrawNode,node.X,node.Y,node.Width,node.Height);
					this.image=this.getTypeImage.GetImage(node.ObjectType);
					if(this.image!=null)
					{
						DrawImage(g,this.image,node.X,node.Y,node.Width,node.Height);
					}
					else
					{
						if(node.ObjectType==Node.DrawObjectType.DrawRectangle)
						{
							this.penDrawNode.Color=node.BorderColor;
							g.DrawRectangle(this.penDrawNode,node.X,node.Y,node.Width,node.Height);
							this.penDrawNode.Color=node.FillColor;
							g.FillRectangle(this.penDrawNode.Brush,node.X,node.Y,node.Width,node.Height);
						}
						else
						{
							this.penDrawNode.Color=node.BorderColor;
							g.DrawEllipse(this.penDrawNode,node.X,node.Y,node.Width,node.Height);
							this.penDrawNode.Color=node.FillColor;
							g.FillEllipse(this.penDrawNode.Brush,node.X,node.Y,node.Width,node.Height);							
						}
					}
					this.control.testDrawLength.Text=node.NodeText;
					int stringleft=node.X+node.Width/2-this.control.testDrawLength.Width/2+3;
					g.DrawString(node.NodeText,node.NodeTextFont,new SolidBrush(node.NodeTextColor),stringleft,node.Y+node.Height);
					for(int j=0;j<8;j++)
					{
						g.DrawLine(this.penDrawPoint,this.selectPoint.RectanglePoint[j].X,this.selectPoint.RectanglePoint[j].Y,this.selectPoint.RectanglePoint[j].X,this.selectPoint.RectanglePoint[j].Y+6);
					}
				}
				
				for(int i=0;i<this.arrDrawStringSelectList.Count;i++)
				{
					drawString=(DrawString)this.arrDrawStringSelectList[i];
					int width,height;
					if(drawString.Width<=2)
					{
						width=2;
					}
					else 
					{
						width=drawString.Width;
					}

					if(drawString.Height<=2)
					{
						height=2;
					}
					else
					{
						height=drawString.Height;
					}
					this.re=new RectangleF(drawString.X,drawString.Y,width,height);
					this.penDrawString.Color=drawString.DSTextColor;
					g.DrawString(drawString.Content,drawString.DSTextFont,this.penDrawString.Brush,this.re);
					this.selectPoint.SetRectanglePoint(drawString.X,drawString.Y,drawString.Width,drawString.Height);
					for(int j=0;j<8;j++)
					{
						g.DrawLine(this.penDrawPoint,this.selectPoint.RectanglePoint[j].X,this.selectPoint.RectanglePoint[j].Y,this.selectPoint.RectanglePoint[j].X,this.selectPoint.RectanglePoint[j].Y+6);
					}
				}
				this.bDrawSelectElement=false;
				return;
			}

			if(this.bDrawSelectRectangle)
			{
				Graphics g=gra;
				Rectangle rect=new Rectangle(this.iMouseSelectX,this.iMouseSelectY,this.iMouseSelectWidth,this.iMouseSelectHeight);
				//g.DrawRectangle(new Pen(Color.Black,1),this.iMouseSelectX,this.iMouseSelectY,this.iMouseSelectWidth,this.iMouseSelectHeight);
				ControlPaint.DrawFocusRectangle(g,rect,this.bckColor,Color.Transparent);
				this.bDrawSelectRectangle=false;
				return;
			}

			//��ڵ��д�ְ��ػ�  ������ѡ�б�ʾ�㣩
			if(this.nodeSelectNode==null && this.nodeThisNode==null && this.lineSelectLine==null && this.lineThisLine==null && (this.arrNodeList.Count>0 || this.arrDrawStringList.Count>0) && !this.b_PressDirctKey &&(this.lastEdit is Node || this.lastEdit is DrawString))
			{
				Graphics g=gra;
				if(this.lastEdit is Node)
				{
					Node node=(Node)this.lastEdit;
					this.selectPoint.SetRectanglePoint(node.X,node.Y,node.Width,node.Height);
					for(int i=0;i<8;i++)
					{
						g.DrawLine(this.penDrawPoint,this.selectPoint.RectanglePoint[i].X,this.selectPoint.RectanglePoint[i].Y,this.selectPoint.RectanglePoint[i].X,this.selectPoint.RectanglePoint[i].Y+6);
					}
				}
				else
				{
					DrawString drawString=(DrawString)this.lastEdit;
					this.selectPoint.SetRectanglePoint(drawString.X,drawString.Y,drawString.Width,drawString.Height);
					for(int i=0;i<8;i++)
					{
						g.DrawLine(this.penDrawString,this.selectPoint.RectanglePoint[i].X,this.selectPoint.RectanglePoint[i].Y,this.selectPoint.RectanglePoint[i].X,this.selectPoint.RectanglePoint[i].Y+6);
					}
				}
			}

				//����ػ� ������ѡ�б�ʾ�㣩
			else if(this.lineSelectLine==null && this.lineThisLine==null && this.nodeSelectNode==null && this.nodeThisNode==null && this.arrLineList.Count>0 && this.lastEdit is Line)
			{
				Line line=(Line)this.lastEdit;
				if(line.ObjectType==Line.DrawObjectType.DrawBeeLine)
				{
					this.selectPoint.SetLinePoint(line.X0,line.Y0,line.X1,line.Y1);
				}
				else
				{
					this.selectPoint.SetFlodLinePoint(line);
				}
				Graphics g=gra;
				g.SmoothingMode=SmoothingMode.AntiAlias;
				if(this.iAutoConnectLinePointIndex==0)
				{
					this.penDrawPoint.Color=Color.Red;
					g.DrawLine(this.penDrawPoint,this.selectPoint.LinePoint[0].X-3,this.selectPoint.LinePoint[0].Y,this.selectPoint.LinePoint[0].X+3,this.selectPoint.LinePoint[0].Y);
					this.penDrawPoint.Color=Color.Black;
					this.iAutoConnectLinePointIndex=9;
				}
				else
				{
					g.DrawLine(this.penDrawPoint,this.selectPoint.LinePoint[0].X-3,this.selectPoint.LinePoint[0].Y,this.selectPoint.LinePoint[0].X+3,this.selectPoint.LinePoint[0].Y);
				}
				if(this.iAutoConnectLinePointIndex==1)
				{
					this.penDrawPoint.Color=Color.Red;
					g.DrawLine(this.penDrawPoint,this.selectPoint.LinePoint[1].X-3,this.selectPoint.LinePoint[1].Y,this.selectPoint.LinePoint[1].X+3,this.selectPoint.LinePoint[1].Y);
					this.penDrawPoint.Color=Color.Black;
					this.iAutoConnectLinePointIndex=9;
				}
				else
				{
					g.DrawLine(this.penDrawPoint,this.selectPoint.LinePoint[1].X-3,this.selectPoint.LinePoint[1].Y,this.selectPoint.LinePoint[1].X+3,this.selectPoint.LinePoint[1].Y);
				}
				
				if(line.ObjectType==Line.DrawObjectType.DrawFoldLine)
				{
					g.DrawLine(this.penDrawPoint,this.selectPoint.LinePoint[2].X-3,this.selectPoint.LinePoint[2].Y,this.selectPoint.LinePoint[2].X+3,this.selectPoint.LinePoint[2].Y);
					if(this.iAutoConnectLinePointIndex==3)
					{
						this.penDrawPoint.Color=Color.Red;
						g.DrawLine(this.penDrawPoint,this.selectPoint.LinePoint[3].X-3,this.selectPoint.LinePoint[3].Y,this.selectPoint.LinePoint[3].X+3,this.selectPoint.LinePoint[3].Y);
						this.penDrawPoint.Color=Color.Black;
						this.iAutoConnectLinePointIndex=9;
					}
					else
					{
						g.DrawLine(this.penDrawPoint,this.selectPoint.LinePoint[3].X-3,this.selectPoint.LinePoint[3].Y,this.selectPoint.LinePoint[3].X+3,this.selectPoint.LinePoint[3].Y);
					}
				}
			}
		}
		#endregion 

		public void GetTableList()
		{
			this.control.dataGrid1.DataSource=dft.CreateFlowTable();
		}
		/// <summary>
		/// ��ȡ��������
		/// </summary>
		public ArrayList NodeList
		{
			get{return arrNodeList;}
			set{arrNodeList=value;
			}
		}

		/// <summary>
		/// ��ȡ�ߵ�����
		/// </summary>
		public ArrayList LineList
		{
			get{return arrLineList;}
			set{arrLineList=value;}
		}
		
		/// <summary>
		/// ��ȡд�ְ������
		/// </summary>
		public ArrayList StringList
		{
			get{return arrDrawStringList;}
			set{arrDrawStringList=value;}
		}
		private void DrawImage(Graphics g,Image img,int sx,int sy,int ex,int ey)
		{
			if(!isAlpha)
			{
				g.DrawImage(img,sx,sy,ex,ey);
			}
				g.DrawImage(img,sx,sy,ex,ey);
		}

	}
}
