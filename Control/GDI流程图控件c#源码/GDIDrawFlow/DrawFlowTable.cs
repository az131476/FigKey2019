using System;
using System.Data;
using System.Collections;


namespace GDIDrawFlow
{
	/// <summary>
	/// DrawFlowTable ��ժҪ˵����
	/// </summary>
	public class DrawFlowTable
	{
		DrawObject drawObject;
		public DrawFlowTable(DrawObject indrawObject)
		{
			drawObject=indrawObject;
			flowTable.Columns.Add (colName);
			colName.MaxLength=2000;
			flowTable.Columns.Add(colInNode);
			flowTable.Columns.Add (colOutNode);
			flowTable.Columns.Add(colFunction);
			flowTable.Columns.Add (colRole);
			flowTable.Columns.Add(colInfo);
			nodeList=drawObject.NodeList;
		}
		DataTable flowTable=new DataTable ("FlowTable");
		DataColumn colName=new DataColumn ("�ڵ�����");
		DataColumn colInNode=new DataColumn ("ǰ�����");
		DataColumn colOutNode=new DataColumn ("��̽��");
		DataColumn colFunction=new DataColumn ("ʵ�ֹ���");
		DataColumn colRole=new DataColumn ("�û���ɫ");
		DataColumn colInfo=new DataColumn ("����˵��");
		ArrayList nodeInList=new ArrayList ();
		ArrayList nodeOutList=new ArrayList ();
		ArrayList nodeList;
		public DataTable CreateFlowTable()
		{
			this.flowTable.Rows.Clear();
			nodeList=drawObject.NodeList;
			string outNode="";	  
			for(int i=0;i<nodeList.Count;i++)
			{
				Node node=(Node)nodeList[i];
				nodeInList=node.InFlowNodeID; //ǰ��ID����
				nodeOutList=node.OutFlowNodeID;//���ID����
				int nodeInID;
				int nodeOutID;
				if(nodeInList.Count>0)
				{
					for(int j=0;j<nodeInList.Count;j++)
					{
						outNode="";
						DataRow rowIn=flowTable.NewRow();
						nodeInID=(int)nodeInList[j];
						Node nodeInTemp=(Node)nodeList[nodeInID];
						rowIn["�ڵ�����"]=node.NodeText;
						rowIn["ǰ�����"]=nodeInTemp.NodeText;
						rowIn["ʵ�ֹ���"]=node.Function;
						rowIn["�û���ɫ"]=node.OperationRole;
						rowIn["����˵��"]=node.FunctionInfo;
						for(int k=0;k<nodeOutList.Count;k++)
						{
							nodeOutID=(int)nodeOutList[k];
							Node nodeOutTemp=(Node)nodeList[nodeOutID];
							outNode+=nodeOutTemp.NodeText+",";
						}
						if(outNode.Length>0 && outNode.Substring(outNode.Length-1)==",")
						{
							outNode=outNode.Remove(outNode.Length-1,1);
						}
						rowIn["��̽��"]=outNode;
					
						flowTable.Rows.Add (rowIn);
					}
				}
				else
				{
					for(int j=0;j<nodeOutList.Count;j++)
					{
						outNode="";
						DataRow rowOut=flowTable.NewRow();
						nodeInID=(int)nodeOutList[j];
						Node nodeOutTemp=(Node)nodeList[nodeInID];
						rowOut["�ڵ�����"]=node.NodeText;
						rowOut["��̽��"]=nodeOutTemp.NodeText;
						rowOut["ʵ�ֹ���"]=node.Function;
						rowOut["�û���ɫ"]=node.OperationRole;
						rowOut["����˵��"]=node.FunctionInfo;
						for(int k=0;k<nodeInList.Count;k++)
						{
							nodeOutID=(int)nodeInList[k];
							Node nodeInTemp=(Node)nodeList[nodeOutID];
							outNode+=nodeInTemp.NodeText+",";
						}
						//outNode=outNode.Remove(outNode.Length-1,1);
						rowOut["ǰ�����"]=outNode;
					
						flowTable.Rows.Add (rowOut);
					}
				}
			}
			return flowTable;

		}
	}
}
