using OPCAutomation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OPCDAAutoTest
{
    class OPC_Client
    {
        OPCServer MyServer;
        OPCGroups MyGroups;
        OPCGroup MyGroup;
        OPCItems MyItems;
        OPCItem[] MyItem;
        public void Init()
        {
            MyServer = new OPCServer();
            //初始化item数组
            MyItem = new OPCItem[4];
            MyServer.Connect("KEPware.KEPServerEx.V4", "127.0.0.1");//连接本地服务器：服务器名+主机名或IP
            if (MyServer.ServerState == (int)OPCServerState.OPCRunning)
            {
                Console.WriteLine("已连接到：{0}", MyServer.ServerName);
                Console.WriteLine("几个组：{0}", MyServer.OPCGroups.Count);
            }
            else
            {
                //这里你可以根据返回的状态来自定义显示信息，请查看自动化接口API文档
                string errMessage = MyServer.ServerState.ToString();
                throw new Exception(errMessage);
            }
            MyGroups = MyServer.OPCGroups;
            MyGroup = MyServer.OPCGroups.Add("GE.GE330.GEPLC");
            MyGroup.UpdateRate = 250;
            MyGroup.IsActive = true;
            MyGroup.IsSubscribed = true;
            //MyGroup.DataChange += new DIOPCGroupEvent_DataChangeEventHandler(GroupDataChange);
            MyItems = MyGroup.OPCItems;
            //添加item
            MyItem[0] = MyItems.AddItem("GE.GE330.GEPLC.M8001", 0);//bool-buildable
            MyItem[1] = MyItems.AddItem("GE.GE330.GEPLC.M801", 1);//bool-
            MyItem[2] = MyItems.AddItem("GE.GE330.GEPLC.M802", 2);//bool-
            MyItem[3] = MyItems.AddItem("GE.GE330.GEPLC.PLC_PC_S", 3);//string
        }
        public bool BuildAble()
        {
            object ItemValues; object Qualities; object TimeStamps;//同步读的临时变量：值、质量、时间戳
            MyItem[0].Read(1, out ItemValues, out Qualities, out TimeStamps);//同步读，第一个参数只能为1或2
            bool q0 = Convert.ToBoolean(ItemValues);//转换后获取item值
            return q0;
        }
        public void ClearResult()
        {
            MyItem[1].Write(false);
            MyItem[2].Write(false);
        }
        public void SendResult(bool ok)
        {
            if (ok)
            {
                MyItem[1].Write(true);
                MyItem[2].Write(false);
            }
            else
            {
                MyItem[1].Write(false);
                MyItem[2].Write(true);
            }
        }
        public string ReadSerialNumber()
        {
            object ItemValues; object Qualities; object TimeStamps;//同步读的临时变量：值、质量、时间戳
            MyItem[3].Read(1, out ItemValues, out Qualities, out TimeStamps);//同步读，第一个参数只能为1或2
            string q3 = Convert.ToString(ItemValues);//转换后获取item值，为防止读到的值为空，不用ItemValues.ToString()
            return q3;
        }
        public string[] Rx()
        {
            object ItemValues; object Qualities; object TimeStamps;//同步读的临时变量：值、质量、时间戳
            MyItem[0].Read(1, out ItemValues, out Qualities, out TimeStamps);//同步读，第一个参数只能为1或2
            bool q0 = Convert.ToBoolean(ItemValues);//转换后获取item值
            MyItem[1].Read(1, out ItemValues, out Qualities, out TimeStamps);//同步读，第一个参数只能为1或2
            bool q1 = Convert.ToBoolean(ItemValues);//转换后获取item值
            MyItem[2].Read(1, out ItemValues, out Qualities, out TimeStamps);//同步读，第一个参数只能为1或2
            bool q2 = Convert.ToBoolean(ItemValues);//转换后获取item值
            MyItem[3].Read(1, out ItemValues, out Qualities, out TimeStamps);//同步读，第一个参数只能为1或2
            string q3 = Convert.ToString(ItemValues);//转换后获取item值，为防止读到的值为空，不用ItemValues.ToString()
            string s1 = q0.ToString();
            string s2 = q1.ToString();
            string s3 = q2.ToString();
            return new string[] { s1,s2,s3,q3 };
        }
        public void Wx(int i,object v)
        {
            MyItem[i].Write(v);
        }
        public void Clean()
        {
            if (MyServer.ServerState == (int)OPCServerState.OPCRunning)
            {
                //释放所有组资源
                MyServer.OPCGroups.RemoveAll();
                //断开服务器
                MyServer.Disconnect();
            }
        }
    }
}
