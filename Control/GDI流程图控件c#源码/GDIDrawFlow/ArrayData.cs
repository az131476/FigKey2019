using System;
using System.Collections;


namespace GDIDrawFlow
{
	/// <summary>
	/// ArrayData ��ժҪ˵����
	/// </summary>
	/// 
	[Serializable()]
	public class ArrayData
	{
		public  ArrayList arrLineList;//�ߵ�����
		public  ArrayList arrNodeList;//�ڵ������
		public  ArrayList arrDrawStringList;//д�ְ������
		
		public  ArrayList arrLineSelectList;//�ߵ�����
		public  ArrayList arrNodeSelectList;//�ڵ������
		public  ArrayList arrDrawStringSelectList;//д�ְ������	

		public  ArrayList arrLineNotSelectList;//û�б�ѡ����ߵ�����
		public  ArrayList arrNodeNotSelectList;//û�б�ѡ��Ľڵ������
		public  ArrayList arrDrawStringNotSelectList;//û�б�ѡ���д�ְ������

		public  ArrayList arrLineConnectNode;//���ӵ���ڵ����	

		public ArrayData()
		{
			//
			// TODO: �ڴ˴���ӹ��캯���߼�
			//

			this.arrLineList=new ArrayList();
			this.arrNodeList=new ArrayList();
			this.arrDrawStringList=new ArrayList();

			this.arrLineSelectList=new ArrayList();
			this.arrNodeSelectList=new ArrayList();
			this.arrDrawStringSelectList=new ArrayList();

			this.arrLineNotSelectList=new ArrayList();
			this.arrNodeNotSelectList=new ArrayList();
			this.arrDrawStringNotSelectList=new ArrayList();

			this.arrLineConnectNode=new ArrayList();
		}
	}
}
