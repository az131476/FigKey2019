using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MesAPI.Model
{
    public class TestResultItemContent
    {
        public const string TurnItem = "烧录";
        public const string Voltage_12V_Item = "12V电压测试";
        public const string Voltage_5V_Item = "5V电压测试";
        public const string Voltage_33_1V_Item = "3.3V电压测试点-1";
        public const string Voltage_33_2V_Item = "3.3V电压测试点-2";
        public const string Work_Electric_Test = "工作电流";

        public const string PartNumber = "零件号";
        public const string HardWareVersion = "硬件版本";
        public const string SoftVersion = "软件版本";
        public const string ECUID = "ECU ID";
        public const string BootloaderVersion = "报文Bootloader版本号";
        public const string RadioFreq = "射频测试";
        public const string DormantElect = "休眠电流";
        public const string FrontCover = "A01前盖组装";
        public const string BackCover = "A01后盖组装";
        public const string PCBScrew1 = "PCB螺丝1";
        public const string PCBScrew2 = "PCB螺丝2";
        public const string PCBScrew3 = "PCB螺丝3";
        public const string PCBScrew4 = "PCB螺丝4";
        public const string ShellScrew1 = "外壳螺丝1";
        public const string ShellScrew2 = "外壳螺丝2";
        public const string ShellScrew3 = "外壳螺丝3";
        public const string ShellScrew4 = "外壳螺丝4";
        public const string AirtightTest = "气密测试";
        public const string StentScrew1 = "A02支架螺丝1";
        public const string StentScrew2 = "A02支架螺丝2";
        public const string Stent = "A01支架";
        public const string LeftStent = "A01左支架";
        public const string RightStent = "A01右支架";

        public const string Order = "序号";
        public const string PcbaSN = "PCBA_SN";
        public const string ProductSN = "成品SN";
        public const string ProductTypeNo = "产品型号";
        public const string FinalResultValue = "最终结果";
        public const string StationName = "工位名称";
        public const string StationInDate = "进站时间";
        public const string StationOutDate = "出站时间";
        public const string TestResultValue = "工位结果";
        public const string UserTeamLeader = "操作员";
        public const string UserAdmin = "管理员";
    }
    public class TestReulstDetail
    {
        #region basicParams
        public int Order { get; set; }
        public string PcbaSn { get; set; }
        public string ProductSn { get; set; }
        public string ProductTypeNo { get; set; }
        public string FinalResultValue { get; set; }
        public string StationName { get; set; }
        public string StationInDate { get; set; }
        public string StationOutDate { get; set; }
        public string TestResultValue { get; set; }
        public string UserTeamLeader { get; set; }
        public string UserAdmin { get; set; }
        #endregion

        //各个工位测试项
        public string BurnItem { get; set; }
        public string Voltage12VItem { get; set; }
        public string Voltage5VItem { get; set; }
        public string Voltage_33_1V_Item { get; set; }
        public string Voltage_33_2V_Item { get; set; }
        public string MainSoftVersion { get; set; }
        public string WorkElectricTest { get; set; }

        public string PartNumber { get; set; }
        public string HardWareVersion { get; set; }
        public string SoftVersion { get; set; }
        public string ECUID { get; set; }
        public string BootloaderVersion { get; set; }
        public string RadioFreq { get; set; }
        public string DormantElect { get; set; }
        public string FrontCover { get; set; }
        public string BackCover { get; set; }
        public string PCBScrew1 { get; set; }
        public string PCBScrew2 { get; set; }
        public string PCBScrew3 { get; set; }
        public string PCBScrew4 { get; set; }
        public string ShellScrew1 { get; set; }
        public string ShellScrew2 { get; set; }
        public string ShellScrew3 { get; set; }
        public string ShellScrew4 { get; set; }
        public string AirtightTest { get; set; }

        public string StentScrew1 { get; set; }
        public string StentScrew2 { get; set; }
        public string Stent { get; set; }
        public string LeftStent { get; set; }
        public string RightStent { get; set; }
    }
}