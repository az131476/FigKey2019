using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using CANManager.Model;
using CommonUtils.Logger;

namespace CANManager.CAN
{
    public struct Diagnose
    {
        public uint dignostypes;  //诊断协议 		0:15031 1:14229 2:KWP2000
        public uint commtypes;        //通讯类型 		0:CAN 1:K-Line
        public uint frameType;        //帧类型	 		0:标准帧 1:扩展帧
        public uint initType;         //初始化方式	0:快速初始化 1:5Buad初始化

        public uint tgtAddr;               //ECU标识符		
        public uint srcAddr;               //诊断仪标识符
        public uint funAddr;               //功能寻址
        public uint Baud;                  //波特率

        public uint server_id;        //服务ID
        public uint sub_function; //服务子功能
        public uint client;               //客户
    }

    public enum ModelSID
    {
        SID_01 = 01,
        SID_02 = 02,
        SID_03 = 03,
        SID_04 = 04,
        SID_05 = 05,
        SID_06 = 06,
        SID_07 = 07,
        SID_08 = 08,
        SID_09 = 09,
        SID_0A = 0X0A
    }

    public enum ReadReturnCode
    {
        SUCCESS,
        /// <summary>
        /// 不匹配实际代码时，重新执行一次
        /// </summary>
        NOT_MATCH,
        FAIL
    }
    public enum ModelRebackValueSID
    {
        NONE,
        PID_OX41 = 0X41

    }

    public enum Model_PID
    {
        PID_0X01 = 0X01,
        PID_0X02 = 0X02,
        PID_OX03 = 0X03,
        PID_OX04 = 0X04,
        PID_0X05 = 0X05,
        PID_0X06 = 0X06,
        PID_0X07 = 0X07,
        PID_0X08 = 0X08,
        PID_0X09 = 0X09,
        PID_0X0A = 0X0A,
        PID_0X0B = 0X0B,
        PID_0X0C = 0X0C,
        PID_0X0D = 0X0D,
        PID_0X0E = 0X0E,
        PID_0X0F = 0X0F,
        PID_0X10 = 0X10,
        PID_0X11 = 0X11,
        PID_0X12 = 0X12,
        PID_0X13 = 0X13,
        PID_0X14 = 0X14,
        PID_0X15 = 0X15,
        PID_0X16 = 0X16,
        PID_0X17 = 0X17,
        PID_0X18 = 0X18,
        PID_0X19 = 0X19,
        PID_0X1A = 0X1A,
        PID_0X1B = 0X1B,
        PID_0X1C = 0X1C,
        PID_0X1D = 0X1D,
        PID_0X1E = 0X1E,
        PID_0X1F = 0X1F,
        PID_0X21 = 0X21,
        PID_0X22 = 0X22,
        PID_0X23 = 0X23,
        PID_0X24 = 0X24,
        PID_0X25 = 0X25,
        PID_0X26 = 0X26,
        PID_0X27 = 0X27,
        PID_0X28 = 0X28,
        PID_0X29 = 0X29,
        PID_0X2A = 0X2A,
        PID_0X2B = 0X2B,
        PID_0X2C = 0X2C,
        PID_0X2D = 0X2D,
        PID_0X2E = 0X2E,
        PID_0X2F = 0X2F,
        PID_0X30 = 0X30,
        PID_0X31 = 0X31,
        PID_0X32 = 0X32,
        PID_0X33 = 0X33,
        PID_0X34 = 0X34,
        PID_0X35 = 0X35,
        PID_0X36 = 0X36,
        PID_0X37 = 0X37,
        PID_0X38 = 0X38,
        PID_0X39 = 0X39,
        PID_0X3A = 0X3A,
        PID_0X3B = 0X3B,
        PID_0X3C = 0X3C,
        PID_0X3D = 0X3D,
        PID_0X3E = 0X3E,
        PID_0X3F = 0X3F,
        PID_0X40 = 0X40,
        PID_0X41 = 0X41,
        PID_0X42 = 0X42,
        PID_0X43 = 0X43,
        PID_0X44 = 0X44,
        PID_0X45 = 0X45,
        PID_0X46 = 0X46,
        PID_0X47 = 0X47,
        PID_0X48 = 0X48,
        PID_0X49 = 0X49,
        PID_0X4A = 0X4A,
        PID_0X4B = 0X4B,
        PID_0X4C = 0X4C,
        PID_0X4D = 0X4D,
        PID_0X4E = 0X4E,
        PID_0X4F = 0X4F,
        PID_0X50 = 0X50,
        PID_0X51 = 0X51,
        PID_0X52 = 0X52,
        PID_0X53 = 0X53,
        PID_0X54 = 0X54,
        PID_0X55 = 0X55,
        PID_0X56 = 0X56,
        PID_0X57 = 0X57,
        PID_0X58 = 0X58,
        PID_0X59 = 0X59,
        PID_0X5A = 0X5A,
        PID_0X5B = 0X5B,
        PID_0XFD = 0XFD,
        PID_0XFE = 0XFE,
        PID_0XFF = 0XFF
    }

    public struct PID_MEAN
    {
        #region PID=01
        public const string PID_01_DATAA_0_6 = "与排放相关的DTC数量 ";
        public const string PID_01_DATAA_07 = "故障指示灯";

        public const string PID_01_DATAB_00 = "失火监控";
        public const string PID_01_DATAB_01 = "燃油系统监控";
        public const string PID_01_DATAB_02 = "全面组件监控";
        public const string PID_01_DATAB_03 = "保留";

        public const string PID_01_DATAC_00 = "催化剂监测";
        public const string PID_01_DATAC_01 = "加热催化器监测";
        public const string PID_01_DATAC_02 = "蒸发系统监测";
        public const string PID_01_DATAC_03 = "二次空气系统监测";
        public const string PID_01_DATAC_04 = "A/C系统制冷剂监控";
        public const string PID_01_DATAC_05 = "氧传感器监测";
        public const string PID_01_DATAC_06 = "氧传感器加热器监测";
        public const string PID_01_DATAC_07 = "EGR系统";

        public const string PID_01_DATAD_00 = "催化剂监测";
        public const string PID_01_DATAD_01 = "加热催化器监测";
        public const string PID_01_DATAD_02 = "蒸发系统监测";
        public const string PID_01_DATAD_03 = "二次空气系统监测";
        public const string PID_01_DATAD_04 = "A/C系统制冷剂监控";
        public const string PID_01_DATAD_05 = "氧传感器监测";
        public const string PID_01_DATAD_06 = "氧传感器加热器监测";
        public const string PID_01_DATAD_07 = "EGR系统";
        #endregion

        #region PID=02
        public const string PID_02_DATAB_00 = "导致需要冻结帧数据存储的DTC";
        #endregion

        #region PID=03
        public const string PID_03_DATAA_00 = "开环-还未达到闭环条件";
        public const string PID_03_DATAA_01 = "闭环-使用氧传感器作为燃料控制的反馈";
        public const string PID_03_DATAA_02 = "开环-由于驱动条件（e.g动力富集、减速堆积";
        public const string PID_03_DATAA_03 = "开环-检测到系统故障";
        public const string PID_03_DATAA_04 = "闭环-至少一个氧传感器故障（可能使用单一氧传感器燃料控制";
        public const string PID_03_DATAA_05 = "保留";
        public const string PID_03_DATAA_06 = "保留";
        public const string PID_03_DATAA_07 = "保留";

        public const string PID_03_DATAB_00 = "开环-还未达到闭环条件";
        public const string PID_03_DATAB_01 = "闭环-使用氧传感器作为燃料控制的反馈";
        public const string PID_03_DATAB_02 = "开环-由于驱动条件（e.g动力富集、减速堆积）";
        public const string PID_03_DATAB_03 = "开环-检测到系统故障 ";
        public const string PID_03_DATAB_04 = "闭环-至少一个氧传感器故障（可能使用单一氧传感器燃料控制）";
        public const string PID_03_DATAB_05 = "保留";
        public const string PID_03_DATAB_06 = "保留";
        public const string PID_03_DATAB_07 = "保留";
        #endregion

        #region PID=04
        public const string PID_04_DATAA = "计算负荷量（min:0 max:100）";
        #endregion

        #region PID=05
        public const string PID_05_DATAA = "发动机冷却液温度（min:40℃ max:215℃）";
        #endregion

        #region PID=06-09
        public const string PID_06_DATAA = "短期燃料调整BANK1 短期燃料调整BANK3 ";
        public const string PID_07_DATAA = "长期燃料调整BANK1 长期燃料调整BANK3 ";
        public const string PID_08_DATAA = "短期燃料调整BANK2 短期燃料调整BANK4 ";
        public const string PID_09_DATAA = "长期燃料调整BANK2 长期燃料调整BANK4 ";
        #endregion

        #region PID=0A-11
        public const string PID_0A_DATAA = "油压（min:0 max:765kPa）";
        public const string PID_0B_DATAA = "进气歧管绝对压力(min:0 max:255kPa) ";

        public const string PID_0C_DATAA_B = "发动机转速(min:0 max:16383.75rPm) ";

        public const string PID_0D_DATAA = "车辆速度(0-255) ";
        public const string PID_0E_DATAA = "1号气缸点火正时提前(-64-63.5°) ";
        public const string PID_0F_DATAA = "摄入空气温度（-40-215℃）";
        public const string PID_10_DATAA = "空气流量（0-655.35grams/sec）";
        public const string PID_11_DATAA = "绝对的节气门位置(0-100%)";
        #endregion

        #region PID=12
        public const string PID_12_DATAA_00 = "UPS催化转化器上游 ";
        public const string PID_12_DATAA_01 = "DNS催化转化器下游  ";
        public const string PID_12_DATAA_02 = "OFF 大气关闭 ";
        public const string PID_12_DATAA_03 = "保留";
        public const string PID_12_DATAA_04 = "保留 ";
        public const string PID_12_DATAA_05 = "保留 ";
        public const string PID_12_DATAA_06 = "保留 ";
        public const string PID_12_DATAA_07 = "保留 ";
        #endregion

        #region PID=13 氧传感器位置，PID$13不支持时才可由给定车辆支持
        public const string PID_13_DATAA_00 = "通道1，传感器1";
        public const string PID_13_DATAA_01 = "通道1，传感器2";
        public const string PID_13_DATAA_02 = "通道1，传感器3";
        public const string PID_13_DATAA_03 = "通道1，传感器4";
        public const string PID_13_DATAA_04 = "通道2，传感器1";
        public const string PID_13_DATAA_05 = "通道2，传感器2";
        public const string PID_13_DATAA_06 = "通道2，传感器3";
        public const string PID_13_DATAA_07 = "通道2，传感器2";
        #endregion

        #region PID=14-1B 
        public const string PID_14_DATA = "通道1，传感器1";
        public const string PID_15_DATA = "通道1，传感器2";
        public const string PID_16_DATA = "通道1，传感器3";
        public const string PID_17_DATA = "通道1，传感器4";
        public const string PID_18_DATA = "通道2，传感器1";
        public const string PID_19_DATA = "通道2，传感器2";
        public const string PID_1A_DATA = "通道2，传感器3";
        public const string PID_1B_DATA = "通道2，传感器4";
        #endregion

        #region PID=14-1B 
        public const string PID_1C_DATA_01 = "OBD 2 ";
        public const string PID_1C_DATA_02 = "OBD  ";
        public const string PID_1C_DATA_03 = "OBD OBD2 ";
        public const string PID_1C_DATA_04 = "OBD 1 ";
        public const string PID_1C_DATA_05 = "NO OBD ";
        public const string PID_1C_DATA_06 = "EOBD ";
        public const string PID_1C_DATA_07 = "EOBD OBD2 ";
        public const string PID_1C_DATA_08 = "EOBD OBD ";
        public const string PID_1C_DATA_09 = "EOBD OBD OBD2  ";
        public const string PID_1C_DATA_0A = "JOBD  ";
        public const string PID_1C_DATA_0B = "JOBD OBD2  ";
        public const string PID_1C_DATA_0C = "JOBD EOBD  ";
        public const string PID_1C_DATA_0D = "JOBD EOBD OBD2  ";
        public const string PID_1C_DATA_0E = "EURO IV B1  ";
        public const string PID_1C_DATA_0F = "EURO IV B2  ";
        public const string PID_1C_DATA_10 = "EURO C  ";
        public const string PID_1C_DATA_11 = "EMD  ";
        #endregion

        #region PID=1D 氧传感器位置
        public const string PID_1D_DATAA_00 = "通道1，传感器1";
        public const string PID_1D_DATAA_01 = "通道1，传感器2 ";
        public const string PID_1D_DATAA_02 = "通道2，传感器1";
        public const string PID_1D_DATAA_03 = "通道2，传感器2";
        public const string PID_1D_DATAA_04 = "通道3，传感器1";
        public const string PID_1D_DATAA_05 = "通道3，传感器2 ";
        public const string PID_1D_DATAA_06 = "通道4，传感器1";
        public const string PID_1D_DATAA_07 = "通道4，传感器2";
        #endregion

        #region PID=1E
        public const string PID_1E_DATA_A_00 = "电源关闭状态";
        public const string PID_1E_DATA_A_01_07 = "保留";
        #endregion

        #region PID=1F-23
        public const string PID_1F_DATA = "引擎启动后运行时间（0-65535秒）";
        public const string PID_21_DATA = "MIL灯点亮后车辆行驶里程";
        public const string PID_22_DATA = "燃料轨压力相对于管道真空(0-5177.265kPa)";
        public const string PID_23_DATA = "油轨压力(柴油或汽油直喷)(0-655350kPa)";
        #endregion

        #region PID=24-2B,仅适用于PID13定义的氧传感器位置 1-2bank 1-4传感器
        public const string PID_24_2B_DATA_13_AB = "等效比(AB)";
        public const string PID_24_2B_DATA_13_CD = "氧传感器电压(CD)";
        #endregion

        #region PID=24,仅适用于PID1D定义的氧传感器位置 1-4bank 1-2传感器
        public const string PID_24_DATA_1D = "AB-等效比,CD-氧传感器电压";
        public const string PID_25_DATA_1D = "AB-等效比,CD-氧传感器电压";
        public const string PID_26_DATA_1D = "AB-等效比,CD-氧传感器电压";
        public const string PID_27_DATA_1D = "AB-等效比,CD-氧传感器电压";
        public const string PID_28_DATA_1D = "AB-等效比,CD-氧传感器电压";
        public const string PID_29_DATA_1D = "AB-等效比,CD-氧传感器电压";
        public const string PID_2A_DATA_1D = "AB-等效比,CD-氧传感器电压";
        public const string PID_2B_DATA_1D = "AB-等效比,CD-氧传感器电压";
        #endregion

        #region PID=2C-33
        public const string PID_2C_DATA = "EGR(废弃循环)命令(0-100%)";
        public const string PID_2D_DATA = "EGR(废弃循环)错误(-100-99.22%)";
        public const string PID_2E_DATA = "蒸发净化命令(0-100%)";
        public const string PID_2F_DATA = "油量液位情况(0-100%)";
        public const string PID_30_DATA = "自诊断故障清除后热身次数(0-255)";
        public const string PID_31_DATA = "自诊断故障清除后行驶距离(0-65535km)";
        public const string PID_32_DATA = "EVAP 系统蒸汽压力(-8192-8192Pa)";
        public const string PID_33_DATA = "大气压(0-255kPa)";

        #endregion

        #region PID=34-3B 仅适用于PID13定义的氧传感器位置 1-4bank 1-2传感器
        public const string PID_34_3B_13_DATA_AB = "等效比(0-1.999N/A)-AB";
        public const string PID_34_3B_13_DATA_CD = "氧传感器电流(-128-127.99ma)-CD";
        #endregion

        #region PID=34-3B 仅适用于PID1D定义的氧传感器位置 1-4bank 1-2传感器
        public const string PID_34_1D_DATA = "AB-等效比(0-1.999N/A)，CD-氧传感器电流(-128-127.99ma)";
        public const string PID_35_1D_DATA = "AB-等效比(0-1.999N/A)，CD-氧传感器电流(-128-127.99ma)";
        public const string PID_36_1D_DATA = "AB-等效比(0-1.999N/A)，CD-氧传感器电流(-128-127.99ma)";
        public const string PID_37_1D_DATA = "AB-等效比(0-1.999N/A)，CD-氧传感器电流(-128-127.99ma)";
        public const string PID_38_1D_DATA = "AB-等效比(0-1.999N/A)，CD-氧传感器电流(-128-127.99ma)";
        public const string PID_39_1D_DATA = "AB-等效比(0-1.999N/A)，CD-氧传感器电流(-128-127.99ma)";
        public const string PID_3A_1D_DATA = "AB-等效比(0-1.999N/A)，CD-氧传感器电流(-128-127.99ma)";
        public const string PID_3B_1D_DATA = "AB-等效比(0-1.999N/A)，CD-氧传感器电流(-128-127.99ma)";
        #endregion

        #region PID=3C-3F 
        public const string PID_3C_DATA = "催化剂温度(通道1，传感器1)(-40-6513.5℃)";
        public const string PID_3D_DATA = "催化剂温度(通道2，传感器1)(-40-6513.5℃)";
        public const string PID_3E_DATA = "催化剂温度(通道1，传感器2)(-40-6513.5℃)";
        public const string PID_3F_DATA = "催化剂温度(通道2，传感器2)(-40-6513.5℃)";
        #endregion

        #region PID=41
        public const string PID_41_DATA_A0_A7 = "保留";

        public const string PID_41_DATA_B0 = "失火监测";
        public const string PID_41_DATA_B1 = "燃油系统监测";
        public const string PID_41_DATA_B2 = "全面组件监测";
        public const string PID_41_DATA_B3 = "保留 ";

        public const string PID_41_DATA_CD0 = "催化剂监控";
        public const string PID_41_DATA_CD1 = "催化剂加热";
        public const string PID_41_DATA_CD2 = "蒸发系统监控";
        public const string PID_41_DATA_CD3 = "二次空气系统检测仪";
        public const string PID_41_DATA_CD4 = "空调系统制冷剂监测";
        public const string PID_41_DATA_CD5 = "氧传感器监测";
        public const string PID_41_DATA_CD6 = "氧传感器加热器监控";
        public const string PID_41_DATA_CD7 = "EGR系统监控";
        #endregion

        #region PID=42-4F
        public const string PID_42_DATA = "电压控制模块(0-65.535V)";
        public const string PID_43_DATA = "绝对负荷值(0-25700%)";
        public const string PID_44_DATA = "等效比命令(0-2N/A)";
        public const string PID_45_DATA = "相对节气门位置(0-100%)";
        public const string PID_46_DATA = "环境空气温度(-40-215℃)";
        public const string PID_47_DATA = "绝对气流阀位置B(0-100%)";
        public const string PID_48_DATA = "绝对气流阀位置C(0-100%)";
        public const string PID_49_DATA = "绝对气流阀位置D(0-100%)";
        public const string PID_4A_DATA = "绝对气流阀位置E(0-100%)";
        public const string PID_4B_DATA = "绝对气流阀位置F(0-100%)";
        public const string PID_4C_DATA = "通讯节流阀执行机构控制(0-100%)";
        public const string PID_4D_DATA = "启动MIL时引擎运行的时间(0-65535分钟)";
        public const string PID_4E_DATA = "诊断故障代码清除以来的时间(0-65535分钟)";

        public const string PID_4F_DATA_A = "等值比的最大值(0-255)";
        public const string PID_4F_DATA_B = "氧传感器电压最大值(0-255V)";
        public const string PID_4F_DATA_C = "氧传感器电流最大值(0-255mA)";
        public const string PID_4F_DATA_D = "进气歧管绝对压力的最大值(0-2550kPa)";
        #endregion

        #region PID=51-FF ，5B-FF保留
        public const string PID_51_DATA = "车辆目前使用的燃料类型（01-0f）";
        public const string PID_52_DATA = "酒精燃料比例（0-100%）";
        public const string PID_53_DATA = "绝对Evap系统蒸汽压（0-327.675kPa）";
        public const string PID_54_DATA = "Evap系统蒸汽压力（-32767-32768pa）";

        public const string PID_55_DATA_A = "短时间内二级氧传感器燃料修整器BANK1（-100-99.22%）";
        public const string PID_55_DATA_B = "短时间内二级氧传感器燃料修整器BANK3（-100-99.22%）";

        public const string PID_56_DATA_A = "长时间内二级氧传感器燃料修整器BANK1（-100-99.22%）";
        public const string PID_56_DATA_B = "长时间内二级氧传感器燃料修整器BANK3（-100-99.22%）";

        public const string PID_57_DATA_A = "短时间内二级氧传感器燃料修整器BANK2（-100-99.22%）";
        public const string PID_57_DATA_B = "短时间内二级氧传感器燃料修整器BANK4（-100-99.22%）";

        public const string PID_58_DATA_A = "长时间内二级氧传感器燃料修整器BANK2（-100-99.22%）";
        public const string PID_58_DATA_B = "长时间内二级氧传感器燃料修整器BANK4（-100-99.22%）";

        public const string PID_59_DATA = "燃料轨压力绝对（0-655350kpa）";

        public const string PID_5A_DATA = "相对油门踏板（0-100%）";
        //5B-FF 保留
        public const string PID_5B_FF_DATA = "保留";

        public const string PID_FD_DATA = "ADP 气流抵消（-32768-32767）";
        public const string PID_FE_DATA = "ADP燃料乘数（65535）";
        public const string PID_FF_DATA = "ADP 燃料抵消（-32768-32767）";

        #endregion
    }

    public class PidServer
    {
        private static DeviceInfo device;
        public delegate void MsgDelegate(StringBuilder msg);
        private static uint FLOW_CONTROL_FILTER = 0x00000003;
        private static uint ISO15765_FRAME_PAD = 0x00000040;
        private static uint pMsgID = 1;
        private static uint CLEAR_RX_BUFFER = 0x08;
        private static uint CLEAR_TX_BUFFER = 0x07;
        private static byte sid = 0x01;
        private static byte pid = 0x00;
        private static List<byte> funCodeList;
        private static int frameAddressDef = 0;
        private static int frameAddressIndex;
        private static int framePidNextStartIndex;
        private static int frameSupportNext = 0;//是否继续支持下一帧,1-support
        private static bool IsSearchAllAddress;//查询所有支持的服务地址


        unsafe public static int PassThruStartMsgFilter(DeviceInfo device)
        {
            PassthruMsg pMaskMsg = new PassthruMsg();
            pMaskMsg.ProtocolID = (uint)device.ProtocolID;
            pMaskMsg.DataSize = 4;
            if (pMaskMsg.ProtocolID == (uint)DeviceInfo.ProtocolType.ISO15765)
            {
                pMaskMsg.TxFlags = ISO15765_FRAME_PAD;
            }
            else if(pMaskMsg.ProtocolID == (uint)DeviceInfo.ProtocolType.ISO14230)
            {
                pMaskMsg.TxFlags = 0;
            }

            PassthruMsg pPatternMsg = new PassthruMsg();
            pPatternMsg.ProtocolID = (uint)device.ProtocolID;
            if (pPatternMsg.ProtocolID == (uint)DeviceInfo.ProtocolType.ISO15765)
                pPatternMsg.TxFlags = ISO15765_FRAME_PAD;
            else
                pPatternMsg.TxFlags = 0;
            pPatternMsg.DataSize = 4;
            pPatternMsg.Data[0] = 0;
            pPatternMsg.Data[1] = 0;
            pPatternMsg.Data[2] = 7;
            pPatternMsg.Data[3] = 0xe8;

            int res_Icotl_rx = MonGoose.MonIcotl(device.ChannelID, CLEAR_RX_BUFFER, new IntPtr(0), new IntPtr(0));
            int res_Icotl_tx = MonGoose.MonIcotl(device.ChannelID, CLEAR_TX_BUFFER, new IntPtr(0), new IntPtr(0));

            LogHelper.Log.Info($"res_Icotl_rx:{res_Icotl_rx}+ res_Icotl_tx:{res_Icotl_tx}");

            if (pMaskMsg.ProtocolID == (uint)DeviceInfo.ProtocolType.ISO15765)
            {
                pMaskMsg.Data[0] = 0xFF;
                pMaskMsg.Data[1] = 0xFF;
                pMaskMsg.Data[2] = 0xFF;
                pMaskMsg.Data[3] = 0xFF;
            }
            else if (pMaskMsg.ProtocolID == (uint)DeviceInfo.ProtocolType.ISO14230)
            {
                pMaskMsg.Data[0] = 0x00;
                pMaskMsg.Data[1] = 0xFF;
                pMaskMsg.Data[2] = 0xFF;
                pMaskMsg.Data[3] = 0x00;
            }

            PassthruMsg pFlowControlMsg = new PassthruMsg();
            pFlowControlMsg.ProtocolID = (uint)device.ProtocolID;
            pFlowControlMsg.TxFlags = ISO15765_FRAME_PAD;
            pFlowControlMsg.DataSize = 4;
            if (device.ProtocolID == DeviceInfo.ProtocolType.ISO15765)//当选择的是UDS时，过滤ID
            {
                pFlowControlMsg.Data[0] = 0;
                pFlowControlMsg.Data[1] = 0;
                pFlowControlMsg.Data[2] = 7;
                pFlowControlMsg.Data[3] = 0xe0;
            }
            else if (device.ProtocolID == DeviceInfo.ProtocolType.ISO14230)//当选择的是KWP时，只过滤源地址和目标地址
            {
                pPatternMsg.Data[0] = 0x00;
                pPatternMsg.Data[1] = 0x00;//iRxID;
                pPatternMsg.Data[2] = 0x00;// iTxID;
                pPatternMsg.Data[3] = 0x00;
            }
            int r = MonGoose.MonStartMsgFilter(device.ChannelID, FLOW_CONTROL_FILTER, ref pMaskMsg, ref pPatternMsg, ref pFlowControlMsg, ref pMsgID);
            LogHelper.Log.Info($"FilterResult:{r} channelID={device.ChannelID} protocol={device.ProtocolID}");
            return r;
        }

        unsafe public static void CommandMode(DeviceInfo deviceInfo)
        {
            device = deviceInfo;

            switch (device.ModelSidType)
            {
                case DeviceInfo.ModelType.MODEL1:
                    //pid 重新初始化
                    sid = 0x02;
                    pid = 0x00;
                    framePidNextStartIndex = 0;
                    funCodeList = new List<byte>();
                    PassThruWriteMsgs(false);
                    break;
                case DeviceInfo.ModelType.MODEL2:
                    //pid 重新初始化
                    sid = 0x02;
                    pid = 0x00;
                    framePidNextStartIndex = 0;
                    funCodeList = new List<byte>();
                    PassThruWriteMsgs(false);
                    break;
                case DeviceInfo.ModelType.MODEL3:
                    PassThruWriteMsgs(false);
                    break;
                case DeviceInfo.ModelType.MODEL4:
                    PassThruWriteMsgs(false);
                    break;
                case DeviceInfo.ModelType.MODEL5:
                    PassThruWriteMsgs(false);
                    break;
                case DeviceInfo.ModelType.MODEL6:
                    PassThruWriteMsgs(false);
                    break;
                case DeviceInfo.ModelType.MODEL7:
                    PassThruWriteMsgs(false);
                    break;
                case DeviceInfo.ModelType.MODEL8:
                    PassThruWriteMsgs(false);
                    break;
                case DeviceInfo.ModelType.MODEL9:
                    PassThruWriteMsgs(false);
                    break;
                case DeviceInfo.ModelType.MODELA:
                    PassThruWriteMsgs(false);
                    break;
            }
        }
        unsafe private static void PassThruWriteMsgs(bool IsAllSupport)
        {
            //写数据
            PassthruMsg writeStruct = new PassthruMsg();

            writeStruct.ProtocolID = (uint)device.ProtocolID;
            writeStruct.RxStatus = 0;
            if (device.ProtocolID == DeviceInfo.ProtocolType.ISO15765)
                writeStruct.TxFlags = ISO15765_FRAME_PAD;
            else
                writeStruct.TxFlags = 0x00;
            writeStruct.Timestamp = 0;
            //前4位为头
            writeStruct.Data[0] = 0x00;
            writeStruct.Data[1] = 0x00;
            writeStruct.Data[2] = 0x07;
            writeStruct.Data[3] = 0xdf;//7df  7e0

            writeStruct.Data[4] = sid;
            writeStruct.Data[5] = pid;

            writeStruct.DataSize = 6;
            uint pNumMsg = 1;
            uint timeout = 100;

            int res_Icotl_tx = MonGoose.MonIcotl(device.ChannelID, CLEAR_TX_BUFFER, new IntPtr(0), new IntPtr(0));
            LogHelper.Log.Info($"res_Icotl_tx:{res_Icotl_tx}");
            LogHelper.Log.Info($"sendMsg:dataSize={writeStruct.DataSize} data:{writeStruct.Data[0].ToString()} {writeStruct.Data[1].ToString() + " " + writeStruct.Data[2].ToString() + " " + writeStruct.Data[3] + " " + writeStruct.Data[4] + " " + writeStruct.Data[5].ToString()}");
            int res_send = MonGoose.MonWriteMsgs(device.ChannelID, ref writeStruct, ref pNumMsg, timeout);
            LogHelper.Log.Info($"WriteMsgsReturn:{res_send}");
            LogHelper.Log.Info($"The IcotlResult value before WriteMsgs:{res_Icotl_tx}");
            LogHelper.Log.Info($"The WriteMsgs Result value:{res_send}");
            if (res_send != 0)
            {
                LogHelper.Log.Info($"发送失败！{res_send}");
                LogHelper.Log.Info($"发送失败！{res_send}");
                return;
            }
            LogHelper.Log.Info($"发送成功！");
            LogHelper.Log.Info($"发送成功！");
            //发送完成后，去读返回值
            if (!IsAllSupport)
            {
                if (PassThruReadMsgs() == ReadReturnCode.NOT_MATCH)
                {
                    PassThruReadMsgs();
                }
            }
            else
            {
                //解析PID
                if (AnalysisReceive() == ReadReturnCode.NOT_MATCH)
                {
                    AnalysisReceive();
                }
            }
        }

        unsafe private static ReadReturnCode PassThruReadMsgs()
        {
            //读数据
            try
            {
                string framePerBinary = "";
                PassthruMsg Msg = new PassthruMsg();
                Msg.TxFlags = ISO15765_FRAME_PAD;
                Msg.ProtocolID = (uint)device.ProtocolID;
                uint pnumMsg = 1;
                uint timeout = 100;
                int res_read = MonGoose.MonReadMsgs(device.ChannelID, ref Msg, ref pnumMsg, timeout);
                if (res_read != 0)
                {
                    LogHelper.Log.Info($"读取失败！(hex){Convert.ToString(res_read,16)}");
                    return ReadReturnCode.FAIL;
                }
                LogHelper.Log.Info($"读取成功！");
                LogHelper.Log.Info($"ReadMsg:dataSize={Msg.DataSize} data:{Msg.Data[0].ToString()} {Msg.Data[1].ToString() + " " + Msg.Data[2].ToString() + " " + Msg.Data[3] + " " + Msg.Data[4] + " " + Msg.Data[5] + " " + Msg.Data[6] + " " + Msg.Data[7] + " " + Msg.Data[8] + " " + Msg.Data[9]}");

                if (Msg.Data[4] != (int)ModelRebackValueSID.PID_OX41)
                {
                    LogHelper.Log.Info("读取数据错误，尝试重新读取...");
                    return ReadReturnCode.NOT_MATCH;
                }
                else
                {
                    if (Msg.Data[5] == pid)
                    {
                        //to hex
                        framePerBinary = Convert.ToString(Msg.Data[6], 16).PadLeft(2,'0') + Convert.ToString(Msg.Data[7], 16).PadLeft(2,'0') + Convert.ToString(Msg.Data[8], 16).PadLeft(2,'0') + Convert.ToString(Msg.Data[9], 16).PadLeft(2,'0');
                        LogHelper.Log.Info($"framePerString:" + framePerBinary);
                        LogHelper.Log.Info($"framePerBinary(hex)={framePerBinary}");
                        framePerBinary = Convert.ToString(Convert.ToInt32(framePerBinary, 16), 2);
                        LogHelper.Log.Info($"framePerBinary:" + framePerBinary);
                        char[] curFrameArray = framePerBinary.Replace(" ", "").ToCharArray();
                        //将该帧中支持的地址添加到集合
                        for (frameAddressIndex = frameAddressDef; frameAddressIndex < curFrameArray.Length; frameAddressIndex++)
                        {
                            if (curFrameArray[frameAddressIndex] == '1')//1-support,0-not support address
                            {
                                //帧起始位为1
                                string frameHex = Convert.ToString(frameAddressIndex+ framePidNextStartIndex + 1, 16);
                                funCodeList.Add(Convert.ToByte(frameHex,16));
                            }
                        }
                        //判断帧最后一位是否支持
                        if (curFrameArray[frameAddressIndex-1] == '1')
                        {
                            //frameAddressDef += 32;
                            framePidNextStartIndex += 32;
                            frameSupportNext = 1;//继续
                            pid += 0x20;
                            PassThruWriteMsgs(false);
                        }
                        else
                        {
                            //查询不到下一帧有支持的地址
                            //查询所有已知的服务地址，并解析
                            LogHelper.Log.Info("【所有支持的地址查询完毕】数量=" + funCodeList.Count+" 开始查询所有支持的服务地址........");
                            frameSupportNext = 0;
                            ExcuteAllSupportAddress();
                        }
                    }
                }
                return ReadReturnCode.SUCCESS;
            }
            catch (Exception ex)
            {
                LogHelper.Log.Error($"{ex.Message+ex.StackTrace}");
                return ReadReturnCode.FAIL;
            }
        }

        unsafe private static ReadReturnCode AnalysisReceive()
        {
            //读数据
            LogHelper.Log.Info($"开始解析...");
            PassthruMsg Msg = new PassthruMsg();
            Msg.TxFlags = ISO15765_FRAME_PAD;
            Msg.ProtocolID = (uint)device.ProtocolID;
            uint pnumMsg = 1;
            uint timeout = 100;
            int res_read = MonGoose.MonReadMsgs(device.ChannelID, ref Msg, ref pnumMsg, timeout);
            if (res_read != 0)
            {
                return ReadReturnCode.FAIL;
            }

            if (Msg.Data[4] != (int)ModelRebackValueSID.PID_OX41)
            {
                return ReadReturnCode.NOT_MATCH;
            }
            else
            {
                if (Msg.Data[5] == pid)
                {
                    string framePerBinaryDataA = Msg.Data[6].ToString();
                    string framePerBinaryDataB = Msg.Data[7].ToString();
                    string framePerBinaryDataC = Msg.Data[8].ToString();
                    string framePerBinaryDataD = Msg.Data[9].ToString();

                    framePerBinaryDataA = Convert.ToString(Convert.ToInt32(framePerBinaryDataA), 2).PadLeft(8, '0');
                    framePerBinaryDataB = Convert.ToString(Convert.ToInt32(framePerBinaryDataB), 2).PadLeft(8, '0');
                    framePerBinaryDataC = Convert.ToString(Convert.ToInt32(framePerBinaryDataC), 2).PadLeft(8, '0');
                    framePerBinaryDataD = Convert.ToString(Convert.ToInt32(framePerBinaryDataD), 2).PadLeft(8, '0');

                    switch (pid)
                    {
                        case (int)Model_PID.PID_0X01:
                            PID_0XO1(framePerBinaryDataA.ToCharArray(),framePerBinaryDataB.ToCharArray(),framePerBinaryDataC.ToCharArray(),framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X02:
                            PID_0XO2(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_OX03:
                            PID_0XO3(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray());
                            break;
                        case (int)Model_PID.PID_OX04:
                            PID_0XO4(framePerBinaryDataA.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X05:
                            PID_0XO5(framePerBinaryDataA.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X06:
                            PID_0XO6(framePerBinaryDataA.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X07:
                            PID_0XO7(framePerBinaryDataA.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X08:
                            PID_0XO8(framePerBinaryDataA.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X09:
                            PID_0XO9(framePerBinaryDataA.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X0A:
                            PID_0X0A(framePerBinaryDataA.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X0B:
                            PID_0X0B(framePerBinaryDataA.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X0C:
                            PID_0X0C(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X0D:
                            PID_0X0D(framePerBinaryDataA.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X0E:
                            PID_0X0E(framePerBinaryDataA.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X0F:
                            PID_0X0F(framePerBinaryDataA.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X10:
                            PID_0X10(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X11:
                            PID_0X11(framePerBinaryDataA.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X12:
                            PID_0X12(framePerBinaryDataA.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X13:
                            PID_0X13(framePerBinaryDataA.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X14:
                            PID_0X14(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X15:
                            PID_0X15(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X16:
                            PID_0X16(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X17:
                            PID_0X17(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X18:
                            PID_0X18(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X19:
                            PID_0X19(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X1A:
                            PID_0X1A(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X1B:
                            PID_0X1B(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X1C:
                            PID_0X1C(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X1D:
                            PID_0X1D(framePerBinaryDataA.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X1E:
                            PID_0X1E(framePerBinaryDataA.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X1F:
                            PID_0X1F(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X21:
                            PID_0X21(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X22:
                            PID_0X22(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X23:
                            PID_0X23(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X24:
                            PID_0X24(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X25:
                            PID_0X25(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X26:
                            PID_0X26(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X27:
                            PID_0X27(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X28:
                            PID_0X28(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X29:
                            PID_0X29(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X2A:
                            PID_0X2A(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X2B:
                            PID_0X2B(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X2C:
                            PID_0X2C(framePerBinaryDataA.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X2D:
                            PID_0X2D(framePerBinaryDataA.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X2E:
                            PID_0X2E(framePerBinaryDataA.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X2F:
                            PID_0X2F(framePerBinaryDataA.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X30:
                            PID_0X30(framePerBinaryDataA.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X31:
                            PID_0X31(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X32:
                            PID_0X32(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X33:
                            PID_0X33(framePerBinaryDataA.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X34:
                            PID_0X34(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X35:
                            PID_0X35(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X36:
                            PID_0X36(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X37:
                            PID_0X37(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X38:
                            PID_0X38(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X39:
                            PID_0X39(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X3A:
                            PID_0X3A(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X3B:
                            PID_0X3B(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X3C:
                            PID_0X3C(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X3D:
                            PID_0X3D(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X3E:
                            PID_0X3E(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X3F:
                            PID_0X3F(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X40:
                            PID_0X40(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X41:
                            PID_0X41(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X42:
                            PID_0X42(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X43:
                            PID_0X43(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X44:
                            PID_0X44(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X45:
                            PID_0X45(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X46:
                            PID_0X46(framePerBinaryDataA.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X47:
                            PID_0X47(framePerBinaryDataA.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X48:
                            PID_0X48(framePerBinaryDataA.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X49:
                            PID_0X49(framePerBinaryDataA.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X4A:
                            PID_0X4A(framePerBinaryDataA.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X4B:
                            PID_0X4B(framePerBinaryDataA.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X4C:
                            PID_0X4C(framePerBinaryDataA.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X4D:
                            PID_0X4D(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X4E:
                            PID_0X4E(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X4F:
                            PID_0X4F(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X50:
                            PID_0X50(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X51:
                            PID_0X51(framePerBinaryDataA.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X52:
                            PID_0X52(framePerBinaryDataA.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X53:
                            PID_0X53(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X54:
                            PID_0X54(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X55:
                            PID_0X55(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X56:
                            PID_0X56(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X57:
                            PID_0X57(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X58:
                            PID_0X58(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X59:
                            PID_0X59(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X5A:
                            PID_0X5A(framePerBinaryDataA.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X5B:
                            PID_0X5B(framePerBinaryDataA.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0XFD:
                            PID_0XFD(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0XFE:
                            PID_0XFE(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0XFF:
                            PID_0XFF(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                    }
                    //解析完成
                    device.TempBuffer = device.TempBuffer;
                }
            }
            return ReadReturnCode.SUCCESS;
        }

        #region pid 01-0f
        private static void PID_0XO1(char[] dataA,char[] dataB,char[] dataC,char[] dataD)
        {
            string dtcNum = "";
            string ledRes = "";
            for (int i = 0; i < dataA.Length; i++)
            {
                if (i <= 6)
                {
                    dtcNum += dataA[i];
                }
                switch (i)
                {
                    case 7:
                        if (dataA[i] == 1)
                        {
                            ledRes = "（打开）";
                        }
                        else
                        {
                            ledRes = "（关闭）";
                        }
                        break;
                }
            }
            //data A 结果
            int dtc = Convert.ToInt32(dtcNum,2);
            WriteContent("PID 01", "", "", "DTC情况指标");
            WriteContent("",dtc.ToString(),"",PID_MEAN.PID_01_DATAA_0_6);
            WriteContent("",CharToString(dataA),"",PID_MEAN.PID_01_DATAA_07+ledRes);

            string[] arrayDataB = new string[4];
            string[] arrayDataCD = new string[8];

            for (int i = 0; i < dataB.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        if (dataB[i] == 1)
                        {
                            arrayDataB[0] = "支持";
                        }
                        else
                        {
                            arrayDataB[0] = "不支持";
                        }
                        break;
                    case 1:
                        if (dataB[i] == 1)
                        {
                            arrayDataB[1] = "支持";
                        }
                        else
                        {
                            arrayDataB[1] = "不支持";
                        }
                        break;
                    case 2:
                        if (dataB[i] == 1)
                        {
                            arrayDataB[2] = "支持";
                        }
                        else
                        {
                            arrayDataB[2] = "不支持";
                        }
                        break;
                    case 3:
                        if (dataB[i] == 1)
                        {
                            arrayDataB[3] = "";
                        }
                        else
                        {
                            arrayDataB[3] = "";
                        }
                        break;
                    case 4:
                        if (dataB[i] == 1)
                        {
                            arrayDataB[0] += "，且完成";
                        }
                        else
                        {
                            arrayDataB[0] += "，且未完成";
                        }
                        break;
                    case 5:
                        if (dataB[i] == 1)
                        {
                            arrayDataB[1] += "，且完成";
                        }
                        else
                        {
                            arrayDataB[1] += "，且未完成";
                        }
                        break;
                    case 6:
                        if (dataB[i] == 1)
                        {
                            arrayDataB[2] += "，且完成";
                        }
                        else
                        {
                            arrayDataB[2] += "，且未完成";
                        }
                        break;
                    case 7:
                        if (dataB[i] == 1)
                        {
                            arrayDataB[3] += "，且完成";
                        }
                        else
                        {
                            arrayDataB[3] += "，且未完成";
                        }
                        break;
                }
            }
            for (int i = 0; i < dataC.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        if (dataC[i] == 1)
                        {
                            arrayDataCD[0] = "支持";
                        }
                        else
                        {
                            arrayDataCD[0] = "不支持";
                        }
                        break;
                    case 1:
                        if (dataC[i] == 1)
                        {
                            arrayDataCD[1] = "支持";
                        }
                        else
                        {
                            arrayDataCD[1] = "不支持";
                        }
                        break;
                    case 2:
                        if (dataC[i] == 1)
                        {
                            arrayDataCD[2] = "支持";
                        }
                        else
                        {
                            arrayDataCD[2] = "不支持";
                        }
                        break;
                    case 3:
                        if (dataC[i] == 1)
                        {
                            arrayDataCD[3] = "支持";
                        }
                        else
                        {
                            arrayDataCD[3] = "不支持";
                        }
                        break;
                    case 4:
                        if (dataC[i] == 1)
                        {
                            arrayDataCD[4] = "支持";
                        }
                        else
                        {
                            arrayDataCD[4] = "不支持";
                        }
                        break;
                    case 5:
                        if (dataC[i] == 1)
                        {
                            arrayDataCD[5] = "支持";
                        }
                        else
                        {
                            arrayDataCD[5] = "不支持";
                        }
                        break;
                    case 6:
                        if (dataC[i] == 1)
                        {
                            arrayDataCD[6] = "支持";
                        }
                        else
                        {
                            arrayDataCD[6] = "不支持";
                        }
                        break;
                    case 7:
                        if (dataC[i] == 1)
                        {
                            arrayDataCD[7] = "支持";
                        }
                        else
                        {
                            arrayDataCD[7] = "不支持";
                        }
                        break;
                }
            }
            for (int i = 0; i < dataD.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        if (dataD[i] == 1)
                        {
                            arrayDataCD[0] += "且完成";
                        }
                        else
                        {
                            arrayDataCD[0] += "且未完成";
                        }
                        break;
                    case 1:
                        if (dataD[i] == 1)
                        {
                            arrayDataCD[1] += "且完成";
                        }
                        else
                        {
                            arrayDataCD[1] += "且未完成";
                        }
                        break;
                    case 2:
                        if (dataD[i] == 1)
                        {
                            arrayDataCD[2] += "且完成";
                        }
                        else
                        {
                            arrayDataCD[2] += "且未完成";
                        }
                        break;
                    case 3:
                        if (dataD[i] == 1)
                        {
                            arrayDataCD[3] += "且完成";
                        }
                        else
                        {
                            arrayDataCD[3] += "且未完成";
                        }
                        break;
                    case 4:
                        if (dataD[i] == 1)
                        {
                            arrayDataCD[4] += "且完成";
                        }
                        else
                        {
                            arrayDataCD[4] += "且未完成";
                        }
                        break;
                    case 5:
                        if (dataD[i] == 1)
                        {
                            arrayDataCD[5] += "且完成";
                        }
                        else
                        {
                            arrayDataCD[5] += "且未完成";
                        }
                        break;
                    case 6:
                        if (dataD[i] == 1)
                        {
                            arrayDataCD[6] += "且完成";
                        }
                        else
                        {
                            arrayDataCD[6] += "且未完成";
                        }
                        break;
                    case 7:
                        if (dataD[i] == 1)
                        {
                            arrayDataCD[7] += "且完成";
                        }
                        else
                        {
                            arrayDataCD[7] += "且未完成";
                        }
                        break;
                }
            }
            //B
            WriteContent("",CharToString(dataB),"",PID_MEAN.PID_01_DATAB_00+arrayDataB[0]);
            WriteContent("", "", "", PID_MEAN.PID_01_DATAB_00 + arrayDataB[1]);
            WriteContent("", "", "", PID_MEAN.PID_01_DATAB_00 + arrayDataB[2]);
            WriteContent("", "", "", PID_MEAN.PID_01_DATAB_00 + arrayDataB[3]);

            //C D 合并显示
            WriteContent("", CharToString(dataC), "", PID_MEAN.PID_01_DATAC_00 + arrayDataCD[0]);
            WriteContent("", CharToString(dataD), "", PID_MEAN.PID_01_DATAC_01 + arrayDataCD[1]);
            WriteContent("", "", "", PID_MEAN.PID_01_DATAC_02 + arrayDataCD[2]);
            WriteContent("", "", "", PID_MEAN.PID_01_DATAC_03 + arrayDataCD[3]);
            WriteContent("", "", "", PID_MEAN.PID_01_DATAC_04 + arrayDataCD[4]);
            WriteContent("", "", "", PID_MEAN.PID_01_DATAC_05 + arrayDataCD[5]);
            WriteContent("", "", "", PID_MEAN.PID_01_DATAC_06 + arrayDataCD[6]);
            WriteContent("", "", "", PID_MEAN.PID_01_DATAC_07 + arrayDataCD[7]);
        }


        private static void PID_0XO2(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {
            device.TempBuffer.AppendLine($"{PID_MEAN.PID_02_DATAB_00} dataA:{CharToString(dataA)} dataB:{CharToString(dataB)} dataC:{CharToString(dataC)} dataD:{CharToString(dataD)}");
        }

        private static void PID_0XO3(char[] dataA, char[] dataB)
        {
            //燃油系统1
            WriteContent("PID 03",CharToString(dataA),"","燃油系统状态A：");
            for (int i=0; i < dataA.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        if (dataA[i] == 1)
                        {
                            WriteContent("","","",PID_MEAN.PID_03_DATAA_00+"显示");
                        }
                        else
                        {
                            WriteContent("", "", "", PID_MEAN.PID_03_DATAA_00 + "正常");
                        }
                        break;
                    case 1:
                        if (dataA[i] == 1)
                        {
                            WriteContent("", "", "", PID_MEAN.PID_03_DATAA_01 + "显示");
                        }
                        else
                        {
                            WriteContent("", "", "", PID_MEAN.PID_03_DATAA_01 + "正常");
                        }
                        break;
                    case 2:
                        if (dataA[i] == 1)
                        {
                            WriteContent("", "", "", PID_MEAN.PID_03_DATAA_02 + "显示");
                        }
                        else
                        {
                            WriteContent("", "", "", PID_MEAN.PID_03_DATAA_02 + "正常");
                        }
                        break;
                    case 3:
                        if (dataA[i] == 1)
                        {
                            WriteContent("", "", "", PID_MEAN.PID_03_DATAA_03 + "显示");
                        }
                        else
                        {
                            WriteContent("", "", "", PID_MEAN.PID_03_DATAA_03 + "正常");
                        }
                        break;
                    case 4:
                        if (dataA[i] == 1)
                        {
                            WriteContent("", "", "", PID_MEAN.PID_03_DATAA_04 + "显示");
                        }
                        else
                        {
                            WriteContent("", "", "", PID_MEAN.PID_03_DATAA_04 + "正常");
                        }
                        break;
                    case 5:
                        if (dataA[i] == 1)
                        {
                            WriteContent("", "", "", PID_MEAN.PID_03_DATAA_05);
                        }
                        else
                        {
                            WriteContent("", "", "", PID_MEAN.PID_03_DATAA_05);
                        }
                        break;
                    case 6:
                        if (dataA[i] == 1)
                        {
                            WriteContent("", "", "", PID_MEAN.PID_03_DATAA_06);
                        }
                        else
                        {
                            WriteContent("", "", "", PID_MEAN.PID_03_DATAA_06);
                        }
                        break;
                    case 7:
                        if (dataA[i] == 1)
                        {
                            WriteContent("", "", "", PID_MEAN.PID_03_DATAA_07);
                        }
                        else
                        {
                            WriteContent("", "", "", PID_MEAN.PID_03_DATAA_07);
                        }
                        break;
                }
            }
            //燃油系统2
            WriteContent("", CharToString(dataB), "", "燃油系统状态B：");
            for (int i = 0; i < dataB.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        if (dataB[i] == 1)
                        {
                            WriteContent("", "", "", PID_MEAN.PID_03_DATAB_00+"显示");
                        }
                        else
                        {
                            WriteContent("", "", "", PID_MEAN.PID_03_DATAB_00 + "正常");
                        }
                        break;
                    case 1:
                        if (dataB[i] == 1)
                        {
                            WriteContent("", "", "", PID_MEAN.PID_03_DATAB_01 + "显示");
                        }
                        else
                        {
                            WriteContent("", "", "", PID_MEAN.PID_03_DATAB_01 + "正常");
                        }
                        break;
                    case 2:
                        if (dataB[i] == 1)
                        {
                            WriteContent("", "", "", PID_MEAN.PID_03_DATAB_02 + "显示");
                        }
                        else
                        {
                            WriteContent("", "", "", PID_MEAN.PID_03_DATAB_02 + "正常");
                        }
                        break;
                    case 3:
                        if (dataB[i] == 1)
                        {
                            WriteContent("", "", "", PID_MEAN.PID_03_DATAB_03 + "显示");
                        }
                        else
                        {
                            WriteContent("", "", "", PID_MEAN.PID_03_DATAB_03 + "正常");
                        }
                        break;
                    case 4:
                        if (dataB[i] == 1)
                        {
                            WriteContent("", "", "", PID_MEAN.PID_03_DATAB_04 + "显示");
                        }
                        else
                        {
                            WriteContent("", "", "", PID_MEAN.PID_03_DATAB_04 + "正常");
                        }
                        break;
                    case 5:
                        if (dataB[i] == 1)
                        {
                            WriteContent("", "", "", PID_MEAN.PID_03_DATAB_05);
                        }
                        else
                        {
                            WriteContent("", "", "", PID_MEAN.PID_03_DATAB_05);
                        }
                        break;
                    case 6:
                        if (dataB[i] == 1)
                        {
                            WriteContent("", "", "", PID_MEAN.PID_03_DATAB_06);
                        }
                        else
                        {
                            WriteContent("", "", "", PID_MEAN.PID_03_DATAB_06);
                        }
                        break;
                    case 7:
                        if (dataB[i] == 1)
                        {
                            WriteContent("", "", "", PID_MEAN.PID_03_DATAB_07);
                        }
                        else
                        {
                            WriteContent("", "", "", PID_MEAN.PID_03_DATAB_07);
                        }
                        break;
                }
            }
        }

        private static void PID_0XO4(char[] dataA)
        {
            //计算负荷值
            //公式：A * 100/255
            double v = Convert.ToInt32(CharToString(dataA), 2) * (100 / 255.00);
            WriteContent("PID 04", v.ToString("f3"), "%", PID_MEAN.PID_04_DATAA);
        }
        /// <summary>
        /// 发动机冷却液温度
        /// </summary>
        /// <param name="dataA"></param>
        private static void PID_0XO5(char[] dataA)
        {
            //公式：A-40
            double c = Convert.ToInt32(CharToString(dataA), 2) - 40;
            WriteContent("PID 05",c.ToString("f3"), "℃", PID_MEAN.PID_05_DATAA);
        }

        /// <summary>
        /// 短期燃料调整BANK1/3
        /// </summary>
        /// <param name="dataA"></param>
        private static void PID_0XO6(char[] dataA)
        {
            //公式：（A-128）(100/128)
            double k = (Convert.ToInt32(CharToString(dataA), 2) -128) * (100 / 128.00);
            WriteContent("PID 06", k.ToString("f3"), "%", PID_MEAN.PID_06_DATAA);
        }

        /// <summary>
        /// 长期燃料调整BANK1/3
        /// </summary>
        /// <param name="dataA"></param>
        private static void PID_0XO7(char[] dataA)
        {
            double k = (Convert.ToInt32(CharToString(dataA), 2) - 128) * (100 / 128.00);
            WriteContent("PID 07", k.ToString("f3"), "%", PID_MEAN.PID_07_DATAA);
        }

        /// <summary>
        /// 短期燃料调整BANK2/4
        /// </summary>
        /// <param name="dataA"></param>
        private static void PID_0XO8(char[] dataA)
        {
            double k = (Convert.ToInt32(CharToString(dataA), 2) - 128) * (100 / 128.00);
            WriteContent("PID 08", k.ToString("f3"), "%", PID_MEAN.PID_08_DATAA);
        }

        /// <summary>
        /// 长期燃料调整BANK2/4
        /// </summary>
        /// <param name="dataA"></param>
        private static void PID_0XO9(char[] dataA)
        {
            double k = (Convert.ToInt32(CharToString(dataA), 2) - 128) * (100 / 128);
            WriteContent("PID 09", k.ToString("f3"), "kPa", PID_MEAN.PID_09_DATAA);
        }

        /// <summary>
        /// 燃油压力
        /// </summary>
        /// <param name="dataA"></param>
        private static void PID_0X0A(char[] dataA)
        {
            //公式：A*3
            double k = Convert.ToInt32(CharToString(dataA), 2) * 3;
            WriteContent("PID 0A", k.ToString("f3"), "%", PID_MEAN.PID_0A_DATAA);
        }

        /// <summary>
        /// 油箱压力绝对值
        /// </summary>
        /// <param name="dataA"></param>
        private static void PID_0X0B(char[] dataA)
        {
            double v = Convert.ToInt32(CharToString(dataA), 2);
            WriteContent("PID 0B", v.ToString("f3"), "kPa", PID_MEAN.PID_0B_DATAA);
        }

        /// <summary>
        /// 引擎转速
        /// </summary>
        /// <param name="dataA"></param>
        /// <param name="dataB"></param>
        private static void PID_0X0C(char[] dataA, char[] dataB)
        {
            //公式：（A*256+B）/ 4
            double a = Convert.ToInt32(CharToString(dataA), 2);
            double b = Convert.ToInt32(CharToString(dataB), 2);
            double v = (a * 256 + b) / 4;
            WriteContent("PID 0C", v.ToString("f3"), "rpm", PID_MEAN.PID_0C_DATAA_B);
        }

        /// <summary>
        /// 车辆速度
        /// </summary>
        /// <param name="dataA"></param>
        private static void PID_0X0D(char[] dataA)
        {
            double s = Convert.ToInt32(CharToString(dataA), 2);
            WriteContent("PID 0D", s.ToString("f3"), "km/h", PID_MEAN.PID_0D_DATAA);
        }

        /// <summary>
        /// 点火提前值
        /// </summary>
        /// <param name="dataA"></param>
        private static void PID_0X0E(char[] dataA)
        {
            //公式：(A-128) /2
            double v = (Convert.ToInt32(CharToString(dataA), 2) - 128) / 2;
            WriteContent("PID 0E", v.ToString("f3"), "°", PID_MEAN.PID_0E_DATAA);
        }

        /// <summary>
        /// 油箱空气温度
        /// </summary>
        /// <param name="dataA"></param>
        private static void PID_0X0F(char[] dataA)
        {
            //公式：A-40
            double v = Convert.ToInt32(CharToString(dataA), 2) - 40;
            WriteContent("PID 0D", v.ToString("f3"), "℃", PID_MEAN.PID_0F_DATAA);
        }
        #endregion

        #region pid 10-1f

        /// <summary>
        /// MAF空气流量速度
        /// </summary>
        /// <param name="dataA"></param>
        /// <param name="dataB"></param>
        private static void PID_0X10(char[] dataA, char[] dataB)
        {
            //公式：（A * 256 + b） / 100
            double a = Convert.ToInt32(CharToString(dataA), 2);
            double b = Convert.ToInt32(CharToString(dataB), 2);
            double v = (a * 256 + b) / 100;
            WriteContent("PID 10", v.ToString("f3"), "grams/sec", PID_MEAN.PID_10_DATAA);
        }

        /// <summary>
        /// 节气门位置
        /// </summary>
        /// <param name="dataA"></param>
        private static void PID_0X11(char[] dataA)
        {
            //公式:A * (100/255)
            double v = Convert.ToInt32(CharToString(dataA), 2) * (100 / 255.00);
            WriteContent("PID 11", v.ToString("f3"), "%", PID_MEAN.PID_11_DATAA);
        }

        private static void PID_0X12(char[] dataA)
        {
            WriteContent("PID 12",CharToString(dataA),"","二次空气状态");
            for (int i = 0; i < dataA.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        WriteContent("", "AIR_STATUS UPS", "", PID_MEAN.PID_12_DATAA_00);
                        break;
                    case 1:
                        WriteContent("", "AIR_STATUS DNS", "", PID_MEAN.PID_12_DATAA_01);
                        break;
                    case 2:
                        WriteContent("", "AIR_STATUS OFF", "", PID_MEAN.PID_12_DATAA_02);
                        break;
                    case 3:
                        WriteContent("", "", "", PID_MEAN.PID_12_DATAA_03);
                        break;
                    case 4:
                        WriteContent("", "", "", PID_MEAN.PID_12_DATAA_04);
                        break;
                    case 5:
                        WriteContent("", "", "", PID_MEAN.PID_12_DATAA_05);
                        break;
                    case 6:
                        WriteContent("", "", "", PID_MEAN.PID_12_DATAA_06);
                        break;
                    case 7:
                        WriteContent("", "", "", PID_MEAN.PID_12_DATAA_07);
                        break;
                }
            }
        }

        /// <summary>
        /// 氧传感器存在情况
        /// </summary>
        /// <param name="dataA"></param>
        private static void PID_0X13(char[] dataA)
        {
            for (int i = 0; i < dataA.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        if (dataA[i] == 1)
                        {
                            WriteContent("PID 13",CharToString(dataA),"",PID_MEAN.PID_13_DATAA_00+"存在");
                        }
                        else
                        {
                            WriteContent("PID 13", CharToString(dataA), "", PID_MEAN.PID_13_DATAA_00 + "不存在");
                        }
                        break;

                    case 1:
                        if (dataA[i] == 1)
                        {
                            WriteContent("PID 13", "", "", PID_MEAN.PID_13_DATAA_01 + "存在");
                        }
                        else
                        {
                            WriteContent("PID 13", "", "", PID_MEAN.PID_13_DATAA_01 + "不存在");
                        }
                        break;
                    case 2:
                        if (dataA[i] == 1)
                        {
                            WriteContent("PID 13", "", "", PID_MEAN.PID_13_DATAA_02 + "存在");
                        }
                        else
                        {
                            WriteContent("PID 13", "", "", PID_MEAN.PID_13_DATAA_01 + "不存在");
                        }
                        break;
                    case 3:
                        if (dataA[i] == 1)
                        {
                            WriteContent("PID 13", "", "", PID_MEAN.PID_13_DATAA_02 + "存在");
                        }
                        else
                        {
                            WriteContent("PID 13", "", "", PID_MEAN.PID_13_DATAA_02 + "不存在");
                        }
                        break;
                    case 4:
                        if (dataA[i] == 1)
                        {
                            WriteContent("PID 13", "", "", PID_MEAN.PID_13_DATAA_03 + "存在");
                        }
                        else
                        {
                            WriteContent("PID 13", "", "", PID_MEAN.PID_13_DATAA_04 + "不存在");
                        }
                        break;
                    case 5:
                        if (dataA[i] == 1)
                        {
                            WriteContent("PID 13", "", "", PID_MEAN.PID_13_DATAA_05 + "存在");
                        }
                        else
                        {
                            WriteContent("PID 13", "", "", PID_MEAN.PID_13_DATAA_05 + "不存在");
                        }
                        break;
                    case 6:
                        if (dataA[i] == 1)
                        {
                            WriteContent("PID 13", "", "", PID_MEAN.PID_13_DATAA_06 + "存在");
                        }
                        else
                        {
                            WriteContent("PID 13", "", "", PID_MEAN.PID_13_DATAA_06 + "不存在");
                        }
                        break;
                    case 7:
                        if (dataA[i] == 1)
                        {
                            WriteContent("PID 13", "", "", PID_MEAN.PID_13_DATAA_07 + "存在");
                        }
                        else
                        {
                            WriteContent("PID 13", "", "", PID_MEAN.PID_13_DATAA_07 + "不存在");
                        }
                        break;
                }
            }
        }
        /// <summary>
        /// 通道1，传感器1
        /// </summary>
        /// <param name="dataA"></param>
        /// <param name="dataB"></param>
        private static void PID_0X14(char[] dataA, char[] dataB)
        {
            ///公式：A/200 Volts;
            ///(B-128) * 100/128  %
            double va = Convert.ToInt32(CharToString(dataA),2) / 200.00;
            double vb = (Convert.ToInt32(CharToString(dataB),2) - 128) * (100 / 128.00);
            WriteContent("PID 14", "", "", PID_MEAN.PID_14_DATA);
            WriteContent("", va.ToString("f3"), "Volts", "氧传感器电压");
            WriteContent("", vb.ToString("f3"), "%", "短期燃油修正");
        }

        /// <summary>
        /// 通道1，传感器2
        /// </summary>
        /// <param name="dataA"></param>
        /// <param name="dataB"></param>
        private static void PID_0X15(char[] dataA, char[] dataB)
        {
            ///公式：A/200 Volts;
            ///(B-128) * 100/128  %
            double va = Convert.ToInt32(CharToString(dataA), 2) / 200.00;
            double vb = (Convert.ToInt32(CharToString(dataB), 2) - 128) * (100 / 128.00);
            WriteContent("PID 15", "", "", PID_MEAN.PID_15_DATA);
            WriteContent("", va.ToString("f3"), "Volts", "氧传感器电压");
            WriteContent("", vb.ToString("f3"), "%", "短期燃油修正");
        }

        /// <summary>
        /// 通道1，传感器3
        /// </summary>
        /// <param name="dataA"></param>
        /// <param name="dataB"></param>
        private static void PID_0X16(char[] dataA, char[] dataB)
        {
            ///公式：A/200 Volts;
            ///(B-128) * 100/128  %
            double va = Convert.ToInt32(CharToString(dataA), 2) / 200.00;
            double vb = (Convert.ToInt32(CharToString(dataB), 2) - 128) * (100 / 128.00);
            WriteContent("PID 16", "", "", PID_MEAN.PID_16_DATA);
            WriteContent("", va.ToString("f3"), "Volts", "氧传感器电压");
            WriteContent("", vb.ToString("f3"), "%", "短期燃油修正");
        }

        /// <summary>
        /// 通道1，传感器4
        /// </summary>
        /// <param name="dataA"></param>
        /// <param name="dataB"></param>
        private static void PID_0X17(char[] dataA, char[] dataB)
        {
            ///公式：A/200 Volts;
            ///(B-128) * 100/128  %
            double va = Convert.ToInt32(CharToString(dataA), 2) / 200.00;
            double vb = (Convert.ToInt32(CharToString(dataB), 2) - 128) * (100 / 128.00);
            WriteContent("PID 17", "", "", PID_MEAN.PID_17_DATA);
            WriteContent("", va.ToString("f3"), "Volts", "氧传感器电压");
            WriteContent("", vb.ToString("f3"), "%", "短期燃油修正");
        }

        /// <summary>
        /// 通道2，传感器1
        /// </summary>
        /// <param name="dataA"></param>
        /// <param name="dataB"></param>
        private static void PID_0X18(char[] dataA, char[] dataB)
        {
            ///公式：A/200 Volts;
            ///(B-128) * 100/128  %
            double va = Convert.ToInt32(CharToString(dataA), 2) / 200.00;
            double vb = (Convert.ToInt32(CharToString(dataB), 2) - 128) * (100 / 128.00);
            WriteContent("PID 18", "", "", PID_MEAN.PID_18_DATA);
            WriteContent("", va.ToString("f3"), "Volts", "氧传感器电压");
            WriteContent("", vb.ToString("f3"), "%", "短期燃油修正");
        }

        /// <summary>
        /// 通道2，传感器2
        /// </summary>
        /// <param name="dataA"></param>
        /// <param name="dataB"></param>
        private static void PID_0X19(char[] dataA, char[] dataB)
        {
            ///公式：A/200 Volts;
            ///(B-128) * 100/128  %
            double va = Convert.ToInt32(CharToString(dataA), 2) / 200.00;
            double vb = (Convert.ToInt32(CharToString(dataB), 2) - 128) * (100 / 128.00);
            WriteContent("PID 19", "", "", PID_MEAN.PID_19_DATA);
            WriteContent("", va.ToString("f3"), "Volts", "氧传感器电压");
            WriteContent("", vb.ToString("f3"), "%", "短期燃油修正");
        }

        /// <summary>
        /// 通道2，传感器3
        /// </summary>
        /// <param name="dataA"></param>
        /// <param name="dataB"></param>
        private static void PID_0X1A(char[] dataA, char[] dataB)
        {
            ///公式：A/200 Volts;
            ///(B-128) * 100/128  %
            double va = Convert.ToInt32(CharToString(dataA), 2) / 200.00;
            double vb = (Convert.ToInt32(CharToString(dataB), 2) - 128) * (100 / 128.00);
            WriteContent("PID 1A", "", "", PID_MEAN.PID_1A_DATA);
            WriteContent("", va.ToString("f3"), "Volts", "氧传感器电压");
            WriteContent("", vb.ToString("f3"), "%", "短期燃油修正");
        }

        /// <summary>
        /// 通道2，传感器4
        /// </summary>
        /// <param name="dataA"></param>
        /// <param name="dataB"></param>
        private static void PID_0X1B(char[] dataA, char[] dataB)
        {
            ///公式：A/200 Volts;
            ///(B-128) * 100/128  %
            double va = Convert.ToInt32(CharToString(dataA), 2) / 200.00;
            double vb = (Convert.ToInt32(CharToString(dataB), 2) - 128) * (100 / 128.00);
            WriteContent("PID 1B", "", "", PID_MEAN.PID_1B_DATA);
            WriteContent("", va.ToString("f3"), "Volts", "氧传感器电压");
            WriteContent("", vb.ToString("f3"), "%", "短期燃油修正");
        }

        private static void PID_0X1C(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {
            for (int i = 0; i < dataA.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        WriteContent("PID 1C", CharToString(dataA), "", PID_MEAN.PID_1C_DATA_01);
                        break;
                    case 1:
                        WriteContent("", "", "", PID_MEAN.PID_1C_DATA_02);
                        break;
                    case 2:
                        WriteContent("", "", "", PID_MEAN.PID_1C_DATA_03);
                        break;
                    case 3:
                        WriteContent("", "", "", PID_MEAN.PID_1C_DATA_04);
                        break;
                    case 4:
                        WriteContent("", "", "", PID_MEAN.PID_1C_DATA_05);
                        break;
                    case 5:
                        WriteContent("", "", "", PID_MEAN.PID_1C_DATA_06); ;
                        break;
                    case 6:
                        WriteContent("", "", "", PID_MEAN.PID_1C_DATA_07);
                        break;
                    case 7:
                        WriteContent("", "", "", PID_MEAN.PID_1C_DATA_08);
                        break;
                }
            }

            for (int i = 0; i < dataB.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        WriteContent("", CharToString(dataB), "", PID_MEAN.PID_1C_DATA_09);
                        break;
                    case 1:
                        WriteContent("", "", "", PID_MEAN.PID_1C_DATA_0A);
                        break;
                    case 2:
                        WriteContent("", "", "", PID_MEAN.PID_1C_DATA_0B);
                        break;
                    case 3:
                        WriteContent("", "", "", PID_MEAN.PID_1C_DATA_0C);
                        break;
                    case 4:
                        WriteContent("", "", "", PID_MEAN.PID_1C_DATA_0D);
                        break;
                    case 5:
                        WriteContent("", "", "", PID_MEAN.PID_1C_DATA_0E);
                        break;
                    case 6:
                        WriteContent("", "", "", PID_MEAN.PID_1C_DATA_0F);
                        break;
                    case 7:
                        WriteContent("", "", "", PID_MEAN.PID_1C_DATA_10);
                        break;
                }
            }

            for (int i = 0; i < dataC.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        WriteContent("", CharToString(dataC), "", PID_MEAN.PID_1C_DATA_11);
                        break;
                }
            }
        }

        /// <summary>
        /// 氧传感器位置存在情况
        /// </summary>
        /// <param name="dataA"></param>
        private static void PID_0X1D(char[] dataA)
        {
            for (int i = 0; i < dataA.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        if (dataA[i] == 1)
                        {
                            WriteContent("PID 1D",CharToString(dataA),"",PID_MEAN.PID_1D_DATAA_00+"存在");
                        }
                        else
                        {
                            WriteContent("PID 1D", CharToString(dataA), "", PID_MEAN.PID_1D_DATAA_00 + "不存在");
                        }
                        break;

                    case 1:
                        if (dataA[i] == 1)
                        {
                            WriteContent("PID 1D", "", "", PID_MEAN.PID_1D_DATAA_01 + "存在");
                        }
                        else
                        {
                            WriteContent("PID 1D", "", "", PID_MEAN.PID_1D_DATAA_01 + "不存在");
                        }
                        break;
                    case 2:
                        if (dataA[i] == 1)
                        {
                            WriteContent("PID 1D", "", "", PID_MEAN.PID_1D_DATAA_02 + "存在");
                        }
                        else
                        {
                            WriteContent("PID 1D", "", "", PID_MEAN.PID_1D_DATAA_02 + "不存在");
                        }
                        break;
                    case 3:
                        if (dataA[i] == 1)
                        {
                            WriteContent("PID 1D", "", "", PID_MEAN.PID_1D_DATAA_03 + "存在");
                        }
                        else
                        {
                            WriteContent("PID 1D", "", "", PID_MEAN.PID_1D_DATAA_03 + "不存在");
                        }
                        break;
                    case 4:
                        if (dataA[i] == 1)
                        {
                            WriteContent("PID 1D", "", "", PID_MEAN.PID_1D_DATAA_04 + "存在");
                        }
                        else
                        {
                            WriteContent("PID 1D", "", "", PID_MEAN.PID_1D_DATAA_04 + "不存在");
                        }
                        break;
                    case 5:
                        if (dataA[i] == 1)
                        {
                            WriteContent("PID 1D", "", "", PID_MEAN.PID_1D_DATAA_05 + "存在");
                        }
                        else
                        {
                            WriteContent("PID 1D", "", "", PID_MEAN.PID_1D_DATAA_05 + "不存在");
                        }
                        break;
                    case 6:
                        if (dataA[i] == 1)
                        {
                            WriteContent("PID 1D", "", "", PID_MEAN.PID_1D_DATAA_06 + "存在");
                        }
                        else
                        {
                            WriteContent("PID 1D", "", "", PID_MEAN.PID_1D_DATAA_06 + "不存在");
                        }
                        break;
                    case 7:
                        if (dataA[i] == 1)
                        {
                            WriteContent("PID 1D", "", "", PID_MEAN.PID_1D_DATAA_07 + "存在");
                        }
                        else
                        {
                            WriteContent("PID 1D", "", "", PID_MEAN.PID_1D_DATAA_07 + "不存在");
                        }
                        break;
                }
            }
        }

        /// <summary>
        /// 辅助输入状态
        /// </summary>
        /// <param name="dataA"></param>
        private static void PID_0X1E(char[] dataA)
        {
            for (int i = 0; i < dataA.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        if (dataA[i] == 1)
                        {
                            WriteContent("PID 1E", CharToString(dataA), "", PID_MEAN.PID_1E_DATA_A_00 + "（激活）");
                        }
                        else
                        {
                            WriteContent("PID 1E", "", "", PID_MEAN.PID_1E_DATA_A_00 + "（关闭）");
                        }
                        break;
                    default:
                        //WriteContent("PID 1E", "", "", PID_MEAN.PID_1E_DATA_A_01_07);
                        break;
                }
            }
        }

        /// <summary>
        /// 引擎启动后的运行时间
        /// </summary>
        /// <param name="dataA"></param>
        /// <param name="dataB"></param>
        private static void PID_0X1F(char[] dataA, char[] dataB)
        {
            ///公式：A*256 +b
            double a = Convert.ToInt32(CharToString(dataA), 2);
            double b = Convert.ToUInt32(CharToString(dataB), 2);
            double v = a * 256 + b;
            WriteContent("PID 1F",v.ToString("f3"),"秒",PID_MEAN.PID_1F_DATA);
        }

        #endregion

        #region pid 21-2f
        /// <summary>
        /// MIL灯点亮后车辆行驶里程
        /// </summary>
        /// <param name="dataA"></param>
        /// <param name="dataB"></param>
        private static void PID_0X21(char[] dataA, char[] dataB)
        {
            ///a*256+b
            double a = Convert.ToInt32(CharToString(dataA), 2);
            double b = Convert.ToInt32(CharToString(dataB), 2);
            double v = a * 256 + b;
            WriteContent("PID 21", v.ToString("f3"), "Km", PID_MEAN.PID_21_DATA);
        }

        /// <summary>
        /// 油轨压力（相对于歧管真空度）
        /// </summary>
        /// <param name="dataA"></param>
        /// <param name="dataB"></param>
        private static void PID_0X22(char[] dataA, char[] dataB)
        {
            ///(a*256+b)*0.079
            double a = Convert.ToInt32(CharToString(dataA), 2);
            double b = Convert.ToInt32(CharToString(dataB), 2);
            double v = (a * 256 + b) * 0.079;
            WriteContent("PID 22", v.ToString("f3"), "kPa", PID_MEAN.PID_22_DATA);
        }

        /// <summary>
        /// 油轨压力（柴油和汽油直喷）
        /// </summary>
        /// <param name="dataA"></param>
        /// <param name="dataB"></param>
        private static void PID_0X23(char[] dataA, char[] dataB)
        {
            ///（a*256+b）*10
            double a = Convert.ToInt32(CharToString(dataA), 2);
            double b = Convert.ToInt32(CharToString(dataB), 2);
            double v = (a * 256 + b) * 10;
            WriteContent("PID 23", v.ToString("f3"), "kPa", PID_MEAN.PID_23_DATA);
        }
        //24-2B 仅适用于PID13定义的氧传感器位置 1-2BANK 1-4传感器

        /// <summary>
        /// 仅适用于PID13定义的氧传感器位置
        /// </summary>
        /// <param name="dataA"></param>
        /// <param name="dataB"></param>
        /// <param name="dataC"></param>
        /// <param name="dataD"></param>
        private static void PID_0X24(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {
            //v1 = (a * 256 +b) * 2/65535  or (a*256+b)/32768
            //v2 = (c*256 +d)*8/65535  or (c*256 + d) /8192
            double a = Convert.ToInt32(CharToString(dataA), 2);
            double b = Convert.ToInt32(CharToString(dataB), 2);
            double c = Convert.ToInt32(CharToString(dataC), 2);
            double d = Convert.ToInt32(CharToString(dataD), 2);
            double v1 = (a * 256 + b) * (2 / 65535.00);
            double v2 = (c * 256 + d) * (8 / 65535.00);
            WriteContent("PID 24", v1.ToString("f3"), "N/A", PID_MEAN.PID_24_2B_DATA_13_AB);
            WriteContent("", v2.ToString("f3"), "V", PID_MEAN.PID_24_2B_DATA_13_CD);
        }

        private static void PID_0X25(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {
            double a = Convert.ToInt32(CharToString(dataA), 2);
            double b = Convert.ToInt32(CharToString(dataB), 2);
            double c = Convert.ToInt32(CharToString(dataC), 2);
            double d = Convert.ToInt32(CharToString(dataD), 2);
            double v1 = (a * 256 + b) * (2 / 65535.00);
            double v2 = (c * 256 + d) * (8 / 65535.00);
            WriteContent("PID 25", v1.ToString("f3"), "N/A", PID_MEAN.PID_24_2B_DATA_13_AB);
            WriteContent("", v2.ToString("f3"), "V", PID_MEAN.PID_24_2B_DATA_13_CD);
        }

        private static void PID_0X26(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {
            double a = Convert.ToInt32(CharToString(dataA), 2);
            double b = Convert.ToInt32(CharToString(dataB), 2);
            double c = Convert.ToInt32(CharToString(dataC), 2);
            double d = Convert.ToInt32(CharToString(dataD), 2);
            double v1 = (a * 256 + b) * (2 / 65535.00);
            double v2 = (c * 256 + d) * (8 / 65535.00);
            WriteContent("PID 26", v1.ToString("f3"), "N/A", PID_MEAN.PID_24_2B_DATA_13_AB);
            WriteContent("", v2.ToString("f3"), "V", PID_MEAN.PID_24_2B_DATA_13_CD);
        }

        private static void PID_0X27(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {
            double a = Convert.ToInt32(CharToString(dataA), 2);
            double b = Convert.ToInt32(CharToString(dataB), 2);
            double c = Convert.ToInt32(CharToString(dataC), 2);
            double d = Convert.ToInt32(CharToString(dataD), 2);
            double v1 = (a * 256 + b) * (2 / 65535.00);
            double v2 = (c * 256 + d) * (8 / 65535.00);
            WriteContent("PID 27", v1.ToString("f3"), "N/A", PID_MEAN.PID_24_2B_DATA_13_AB);
            WriteContent("", v2.ToString("f3"), "V", PID_MEAN.PID_24_2B_DATA_13_CD);
        }

        private static void PID_0X28(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {
            double a = Convert.ToInt32(CharToString(dataA), 2);
            double b = Convert.ToInt32(CharToString(dataB), 2);
            double c = Convert.ToInt32(CharToString(dataC), 2);
            double d = Convert.ToInt32(CharToString(dataD), 2);
            double v1 = (a * 256 + b) * (2 / 65535.00);
            double v2 = (c * 256 + d) * (8 / 65535.00);
            WriteContent("PID 28", v1.ToString("f3"), "N/A", PID_MEAN.PID_24_2B_DATA_13_AB);
            WriteContent("", v2.ToString("f3"), "V", PID_MEAN.PID_24_2B_DATA_13_CD);
        }

        private static void PID_0X29(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {
            double a = Convert.ToInt32(CharToString(dataA), 2);
            double b = Convert.ToInt32(CharToString(dataB), 2);
            double c = Convert.ToInt32(CharToString(dataC), 2);
            double d = Convert.ToInt32(CharToString(dataD), 2);
            double v1 = (a * 256 + b) * (2 / 65535.00);
            double v2 = (c * 256 + d) * (8 / 65535.00);
            WriteContent("PID 29", v1.ToString("f3"), "N/A", PID_MEAN.PID_24_2B_DATA_13_AB);
            WriteContent("", v2.ToString("f3"), "V", PID_MEAN.PID_24_2B_DATA_13_CD);
        }

        private static void PID_0X2A(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {
            double a = Convert.ToInt32(CharToString(dataA), 2);
            double b = Convert.ToInt32(CharToString(dataB), 2);
            double c = Convert.ToInt32(CharToString(dataC), 2);
            double d = Convert.ToInt32(CharToString(dataD), 2);
            double v1 = (a * 256 + b) * (2 / 65535.00);
            double v2 = (c * 256 + d) * (8 / 65535.00);
            WriteContent("PID 2A", v1.ToString("f3"), "N/A", PID_MEAN.PID_24_2B_DATA_13_AB);
            WriteContent("", v2.ToString("f3"), "V", PID_MEAN.PID_24_2B_DATA_13_CD);
        }

        private static void PID_0X2B(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {
            double a = Convert.ToInt32(CharToString(dataA), 2);
            double b = Convert.ToInt32(CharToString(dataB), 2);
            double c = Convert.ToInt32(CharToString(dataC),2);
            double d = Convert.ToInt32(CharToString(dataD),2);
            double v1 = (a * 256 + b) * (2 / 65535.00);
            double v2 = (c * 256 + d) * (8 / 65535.00);
            WriteContent("PID 2B", v1.ToString("f3"), "N/A", PID_MEAN.PID_24_2B_DATA_13_AB);
            WriteContent("", v2.ToString("f3"), "V", PID_MEAN.PID_24_2B_DATA_13_CD);
        }

        /// <summary>
        /// 废弃循环命令
        /// </summary>
        /// <param name="dataA"></param>
        private static void PID_0X2C(char[] dataA)
        {
            //公式：a * 100/255
            double v = Convert.ToInt32(CharToString(dataA),2) * (100 / 255.00);
            WriteContent("PID 2C", v.ToString("f3"), "%", PID_MEAN.PID_2C_DATA);
        }

        /// <summary>
        /// 废弃循环错误
        /// </summary>
        /// <param name="dataA"></param>
        private static void PID_0X2D(char[] dataA)
        {
            //(a-128) * 100 / 128
            double a = Convert.ToInt32(CharToString(dataA), 2);
            double v = (a - 128) * 100 / 128.00;
            WriteContent("PID 2D", v.ToString("f3"), "%", PID_MEAN.PID_2D_DATA);
        }

        /// <summary>
        /// 蒸发净化命令
        /// </summary>
        /// <param name="dataA"></param>
        private static void PID_0X2E(char[] dataA)
        {
            //a * 100 / 128
            double a = Convert.ToInt32(CharToString(dataA), 2);
            double v = a * 100 / 128.00;
            WriteContent("PID 2E", v.ToString("f3"), "%", PID_MEAN.PID_2E_DATA);
        }

        /// <summary>
        /// 油量液化情况
        /// </summary>
        /// <param name="dataA"></param>
        private static void PID_0X2F(char[] dataA)
        {
            //a * 100 / 128
            double a = Convert.ToInt32(CharToString(dataA), 2);
            double v = a * 100 / 128.00;
            WriteContent("PID 2F", v.ToString("f3"), "%", PID_MEAN.PID_2F_DATA);
        }

        #endregion

        #region pid 30-3f
        /// <summary>
        /// 自诊断故障清除后热身次数
        /// </summary>
        /// <param name="dataA"></param>
        private static void PID_0X30(char[] dataA)
        {
            //A
            double a = Convert.ToInt32(CharToString(dataA), 2);
            WriteContent("PID 30", a.ToString("f3"), "N/A", PID_MEAN.PID_30_DATA);
        }

        /// <summary>
        /// 故障码清除后行驶里程
        /// </summary>
        /// <param name="dataA"></param>
        private static void PID_0X31(char[] dataA,char[] dataB)
        {
            //a * 256 +b
            double a = Convert.ToInt32(CharToString(dataA), 2);
            double b = Convert.ToInt32(CharToString(dataB), 2);
            double v = a * 256 + b;
            WriteContent("PID 31", v.ToString("f3"), "Km", PID_MEAN.PID_31_DATA);
        }

        /// <summary>
        /// 系统蒸汽压力
        /// </summary>
        /// <param name="dataA"></param>
        /// <param name="dataB"></param>
        private static void PID_0X32(char[] dataA, char[] dataB)
        {
            //（a * 256 +b）/4
            double a = Convert.ToInt32(CharToString(dataA), 2);
            double b = Convert.ToInt32(CharToString(dataB), 2);
            double v = (a * 256 + b) / 4.00;
            WriteContent("PID 32", v.ToString("f3"), "Pa", PID_MEAN.PID_32_DATA);
        }

        /// <summary>
        /// 大气压
        /// </summary>
        /// <param name="dataA"></param>
        private static void PID_0X33(char[] dataA)
        {
            //a * 256 +b
            double a = Convert.ToInt32(CharToString(dataA), 2);
            WriteContent("PID 33", a.ToString("f3"), "kPa", PID_MEAN.PID_33_DATA);
        }
        //34-3B 仅适用于PID13定义的氧传感器位置 1-2BANK 1-4传感器

        /// <summary>
        /// 34-3B 仅适用于PID13定义的氧传感器位置
        /// </summary>
        /// <param name="dataA"></param>
        /// <param name="dataB"></param>
        /// <param name="dataC"></param>
        /// <param name="dataD"></param>
        private static void PID_0X34(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {
            //v1 = (a*256+b) / 32768
            //v2 = (c*256 +d) /256 - 128
            double a = Convert.ToInt32(CharToString(dataA), 2);
            double b = Convert.ToInt32(CharToString(dataB), 2);
            double c = Convert.ToInt32(CharToString(dataC), 2);
            double d = Convert.ToInt32(CharToString(dataD), 2);
            double v1 = (a * 256 + b) / 32768.00;
            double v2 = (c * 256 + d) / 256.00 - 128;
            WriteContent("PID 34", v1.ToString("f3"), "N/A", PID_MEAN.PID_34_3B_13_DATA_AB);
            WriteContent("", v1.ToString("f3"), "mA", PID_MEAN.PID_34_3B_13_DATA_CD);
        }

        private static void PID_0X35(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {
            double a = Convert.ToInt32(CharToString(dataA), 2);
            double b = Convert.ToInt32(CharToString(dataB), 2);
            double c = Convert.ToInt32(CharToString(dataC), 2);
            double d = Convert.ToInt32(CharToString(dataD), 2);
            double v1 = (a * 256 + b) / 32768.00;
            double v2 = (c * 256 + d) / 256.00 - 128;
            WriteContent("PID 35", v1.ToString("f3"), "N/A", PID_MEAN.PID_34_3B_13_DATA_AB);
            WriteContent("", v1.ToString("f3"), "mA", PID_MEAN.PID_34_3B_13_DATA_CD);
        }

        private static void PID_0X36(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {
            double a = Convert.ToInt32(CharToString(dataA), 2);
            double b = Convert.ToInt32(CharToString(dataB), 2);
            double c = Convert.ToInt32(CharToString(dataC), 2);
            double d = Convert.ToInt32(CharToString(dataD), 2);
            double v1 = (a * 256 + b) / 32768.00;
            double v2 = (c * 256 + d) / 256.00 - 128;
            WriteContent("PID 36", v1.ToString("f3"), "N/A", PID_MEAN.PID_34_3B_13_DATA_AB);
            WriteContent("", v1.ToString("f3"), "mA", PID_MEAN.PID_34_3B_13_DATA_CD);
        }

        private static void PID_0X37(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {
            double a = Convert.ToInt32(CharToString(dataA), 2);
            double b = Convert.ToInt32(CharToString(dataB), 2);
            double c = Convert.ToInt32(CharToString(dataC), 2);
            double d = Convert.ToInt32(CharToString(dataD), 2);
            double v1 = (a * 256 + b) / 32768.00;
            double v2 = (c * 256 + d) / 256.00 - 128;
            WriteContent("PID 37", v1.ToString("f3"), "N/A", PID_MEAN.PID_34_3B_13_DATA_AB);
            WriteContent("", v1.ToString("f3"), "mA", PID_MEAN.PID_34_3B_13_DATA_CD);
        }

        private static void PID_0X38(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {
            double a = Convert.ToInt32(CharToString(dataA), 2);
            double b = Convert.ToInt32(CharToString(dataB), 2);
            double c = Convert.ToInt32(CharToString(dataC), 2);
            double d = Convert.ToInt32(CharToString(dataD), 2);
            double v1 = (a * 256 + b) / 32768.00;
            double v2 = (c * 256 + d) / 256.00 - 128;
            WriteContent("PID 38", v1.ToString("f3"), "N/A", PID_MEAN.PID_34_3B_13_DATA_AB);
            WriteContent("", v1.ToString("f3"), "mA", PID_MEAN.PID_34_3B_13_DATA_CD);
        }

        private static void PID_0X39(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {
            double a = Convert.ToInt32(CharToString(dataA), 2);
            double b = Convert.ToInt32(CharToString(dataB), 2);
            double c = Convert.ToInt32(CharToString(dataC), 2);
            double d = Convert.ToInt32(CharToString(dataD), 2);
            double v1 = (a * 256 + b) / 32768.00;
            double v2 = (c * 256 + d) / 256.00 - 128;
            WriteContent("PID 39", v1.ToString("f3"), "N/A", PID_MEAN.PID_34_3B_13_DATA_AB);
            WriteContent("", v1.ToString("f3"), "mA", PID_MEAN.PID_34_3B_13_DATA_CD);
        }

        private static void PID_0X3A(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {
            double a = Convert.ToInt32(CharToString(dataA), 2);
            double b = Convert.ToInt32(CharToString(dataB), 2);
            double c = Convert.ToInt32(CharToString(dataC), 2);
            double d = Convert.ToInt32(CharToString(dataD), 2);
            double v1 = (a * 256 + b) / 32768.00;
            double v2 = (c * 256 + d) / 256.00 - 128;
            WriteContent("PID 3A", v1.ToString("f3"), "N/A", PID_MEAN.PID_34_3B_13_DATA_AB);
            WriteContent("", v1.ToString("f3"), "mA", PID_MEAN.PID_34_3B_13_DATA_CD);
        }

        private static void PID_0X3B(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {
            double a = Convert.ToInt32(CharToString(dataA), 2);
            double b = Convert.ToInt32(CharToString(dataB), 2);
            double c = Convert.ToInt32(CharToString(dataC), 2);
            double d = Convert.ToInt32(CharToString(dataD), 2);
            double v1 = (a * 256 + b) / 32768.00;
            double v2 = (c * 256 + d) / 256.00 - 128;
            WriteContent("PID 3B", v1.ToString("f3"), "N/A", PID_MEAN.PID_34_3B_13_DATA_AB);
            WriteContent("", v1.ToString("f3"), "mA", PID_MEAN.PID_34_3B_13_DATA_CD);
        }

        /// <summary>
        /// 催化剂温度（通道1，传感器1）
        /// </summary>
        /// <param name="dataA"></param>
        /// <param name="dataB"></param>
        private static void PID_0X3C(char[] dataA, char[] dataB)
        {
            //s = (a * 256+b) / 10 -40;
            double a = Convert.ToInt32(CharToString(dataA), 2);
            double b = Convert.ToInt32(CharToString(dataB), 2);
            double v = (a * 256 + b) / 10.00 - 40;
            WriteContent("PID 3C", v.ToString("f3"), "℃", PID_MEAN.PID_3C_DATA);
        }

        /// <summary>
        /// 催化剂温度（通道2，传感器1）
        /// </summary>
        /// <param name="dataA"></param>
        /// <param name="dataB"></param>
        private static void PID_0X3D(char[] dataA, char[] dataB)
        {
            //s = (a * 256+b) / 10 -40;
            double a = Convert.ToInt32(CharToString(dataA), 2);
            double b = Convert.ToInt32(CharToString(dataB), 2);
            double v = (a * 256 + b) / 10.00 - 40;
            WriteContent("PID 3D", v.ToString("f3"), "℃", PID_MEAN.PID_3D_DATA);
        }

        /// <summary>
        /// 催化剂温度（通道1，传感器2）
        /// </summary>
        /// <param name="dataA"></param>
        /// <param name="dataB"></param>
        private static void PID_0X3E(char[] dataA, char[] dataB)
        {
            //s = (a * 256+b) / 10 -40;
            double a = Convert.ToInt32(CharToString(dataA), 2);
            double b = Convert.ToInt32(CharToString(dataB), 2);
            double v = (a * 256 + b) / 10.00 - 40;
            WriteContent("PID 3E", v.ToString("f3"), "℃", PID_MEAN.PID_3E_DATA);
        }

        /// <summary>
        /// 催化剂温度（通道2，传感器2）
        /// </summary>
        /// <param name="dataA"></param>
        /// <param name="dataB"></param>
        private static void PID_0X3F(char[] dataA, char[] dataB)
        {
            //s = (a * 256+b) / 10 -40;
            double a = Convert.ToInt32(CharToString(dataA), 2);
            double b = Convert.ToInt32(CharToString(dataB), 2);
            double v = (a * 256 + b) / 10.00 - 40;
            WriteContent("PID 3F", v.ToString("f3"), "℃", PID_MEAN.PID_3F_DATA);
        }
        #endregion

        #region pid 40-4f
        private static void PID_0X40(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {
            
        }

        private static void PID_0X41(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {
            WriteContent("PID 41", "", "", "本次行程监控状态");
            WriteContent("", CharToString(dataA), "", PID_MEAN.PID_41_DATA_A0_A7);
            string[] arrayA = new string[4];
            string[] arrayCD = new string[8];

            for (int i = 0; i < dataB.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        if (dataB[i] == 1)
                        {
                            arrayA[0] = "已启用";
                        }
                        else
                        {
                            arrayA[0] = "未启用";
                        }
                        break;
                    case 1:
                        if (dataB[i] == 1)
                        {
                            arrayA[1] = "已启用";
                        }
                        else
                        {
                            arrayA[1] = "未启用";
                        }
                        break;
                    case 2:
                        if (dataB[i] == 1)
                        {
                            arrayA[2] = "已启用";
                        }
                        else
                        {
                            arrayA[2] = "未启用";
                        }
                        break;
                    case 3:
                        arrayA[3] = "";
                        break;
                    case 4:
                        if (dataB[i] == 1)
                        {
                            arrayA[0] += ",且未完成";
                        }
                        else
                        {
                            arrayA[0] += ",且完成";
                        }
                        break;
                    case 5:
                        if (dataB[i] == 1)
                        {
                            arrayA[1] = ",且未完成";
                        }
                        else
                        {
                            arrayA[1] += ",且完成";
                        }
                        break;
                    case 6:
                        if (dataB[i] == 1)
                        {
                            arrayA[2] += ",且未完成";
                        }
                        else
                        {
                            arrayA[2] += ",且完成";
                        }
                        break;
                    case 7:
                        arrayA[3] += "";
                        break;
                }
            }
            for (int i = 0; i < dataC.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        if (dataC[i] == 1)
                        {
                            arrayCD[0] = "已启用";
                        }
                        else
                        {
                            arrayCD[0] = "未启用";
                        }
                        break;
                    case 1:
                        if (dataC[i] == 1)
                        {
                            arrayCD[1] = "已启用";
                        }
                        else
                        {
                            arrayCD[1] = "未启用";
                        }
                        break;
                    case 2:
                        if (dataC[i] == 1)
                        {
                            arrayCD[2] = "已启用";
                        }
                        else
                        {
                            arrayCD[2] = "未启用";
                        }
                        break;
                    case 3:
                        if (dataC[i] == 1)
                        {
                            arrayCD[3] = "已启用";
                        }
                        else
                        {
                            arrayCD[3] = "已未启用";
                        }
                        break;
                    case 4:
                        if (dataC[i] == 1)
                        {
                            arrayCD[4] = "已启用";
                        }
                        else
                        {
                            arrayCD[4] = "未启用";
                        }
                        break;
                    case 5:
                        if (dataC[i] == 1)
                        {
                            arrayCD[5] = "已启用";
                        }
                        else
                        {
                            arrayCD[5] = "未启用";
                        }
                        break;
                    case 6:
                        if (dataC[i] == 1)
                        {
                            arrayCD[6] = "已启用";
                        }
                        else
                        {
                            arrayCD[6] = "未启用";
                        }
                        break;
                    case 7:
                        if (dataC[i] == 1)
                        {
                            arrayCD[7] = "已启用";
                        }
                        else
                        {
                            arrayCD[7] = "未启用";
                        }
                        break;
                }
            }
            for (int i = 0; i < dataD.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        if (dataD[i] == 1)
                        {
                            arrayCD[0] += "，且已完成";
                        }
                        else
                        {
                            arrayCD[0] += "，且未完成";
                        }
                        break;
                    case 1:
                        if (dataD[i] == 1)
                        {
                            arrayCD[1] += "，且已完成";
                        }
                        else
                        {
                            arrayCD[1] += "，且未完成";
                        }
                        break;
                    case 2:
                        if (dataD[i] == 1)
                        {
                            arrayCD[2] += "，且已完成";
                        }
                        else
                        {
                            arrayCD[2] += "，且未完成";
                        }
                        break;
                    case 3:
                        if (dataD[i] == 1)
                        {
                            arrayCD[3] += "，且已完成";
                        }
                        else
                        {
                            arrayCD[3] += "，且未完成";
                        }
                        break;
                    case 4:
                        if (dataD[i] == 1)
                        {
                            arrayCD[4] += "，且未完成";
                        }
                        else
                        {
                            arrayCD[4] += "，且未完成";
                        }
                        break;
                    case 5:
                        if (dataD[i] == 1)
                        {
                            arrayCD[5] += "，且已完成";
                        }
                        else
                        {
                            arrayCD[5] += "，且未完成";
                        }
                        break;
                    case 6:
                        if (dataD[i] == 1)
                        {
                            arrayCD[6] += "，且已完成";
                        }
                        else
                        {
                            arrayCD[6] += "，且未完成";
                        }
                        break;
                    case 7:
                        if (dataD[i] == 1)
                        {
                            arrayCD[7] += "，且已完成";
                        }
                        else
                        {
                            arrayCD[7] += "，且未完成";
                        }
                        break;
                }
            }

            //B
            WriteContent("", CharToString(dataB), "", PID_MEAN.PID_41_DATA_B0+arrayA[0]);
            WriteContent("", "", "", PID_MEAN.PID_41_DATA_B1 + arrayA[1]);
            WriteContent("", "", "", PID_MEAN.PID_41_DATA_B2 + arrayA[2]);
            WriteContent("", "", "", PID_MEAN.PID_41_DATA_B3 + arrayA[3]);
            //C+D
            WriteContent("", CharToString(dataC), "", PID_MEAN.PID_41_DATA_CD0 + arrayCD[0]);
            WriteContent("", CharToString(dataD), "", PID_MEAN.PID_41_DATA_CD1 + arrayCD[1]);
            WriteContent("", "", "", PID_MEAN.PID_41_DATA_CD2 + arrayCD[2]);
            WriteContent("", "", "", PID_MEAN.PID_41_DATA_CD3 + arrayCD[3]);
            WriteContent("", "", "", PID_MEAN.PID_41_DATA_CD4 + arrayCD[4]);
            WriteContent("", "", "", PID_MEAN.PID_41_DATA_CD5 + arrayCD[5]);
            WriteContent("", "", "", PID_MEAN.PID_41_DATA_CD6 + arrayCD[6]);
            WriteContent("", "", "", PID_MEAN.PID_41_DATA_CD7 + arrayCD[7]);
        }

        /// <summary>
        /// 控制模块电压
        /// </summary>
        /// <param name="dataA"></param>
        /// <param name="dataB"></param>
        private static void PID_0X42(char[] dataA, char[] dataB)
        {
            //公式：(a * 256+b) / 1000
            double a = Convert.ToInt32(CharToString(dataA),2);
            double b = Convert.ToInt32(CharToString(dataB),2);
            double v = (a * 256 + b) / 1000.00;
            WriteContent("PID 42",v.ToString("f3"),"V",PID_MEAN.PID_42_DATA);
        }

        /// <summary>
        /// 绝对负荷
        /// </summary>
        /// <param name="dataA"></param>
        /// <param name="dataB"></param>
        private static void PID_0X43(char[] dataA, char[] dataB)
        {
            double a = Convert.ToInt32(CharToString(dataA), 2);
            double b = Convert.ToInt32(CharToString(dataB), 2);
            double v = (a * 256 + b) * (100 / 255.00);
            WriteContent("PID 43", v.ToString("f3"), "%", PID_MEAN.PID_43_DATA);
        }

        /// <summary>
        /// 等效比命令
        /// </summary>
        /// <param name="dataA"></param>
        /// <param name="dataB"></param>
        private static void PID_0X44(char[] dataA, char[] dataB)
        {
            double a = Convert.ToInt32(CharToString(dataA), 2);
            double b = Convert.ToInt32(CharToString(dataB), 2);
            double v = (a * 256 + b) / 32768.00;
            WriteContent("PID 44", v.ToString("f3"), "N/A", PID_MEAN.PID_44_DATA);
        }

        /// <summary>
        /// 相对节气门位置
        /// </summary>
        /// <param name="dataA"></param>
        /// <param name="dataB"></param>
        private static void PID_0X45(char[] dataA, char[] dataB)
        {
            double a = Convert.ToInt32(CharToString(dataA), 2);
            double b = Convert.ToInt32(CharToString(dataB), 2);
            double v = a * (100 / 255.00);
            WriteContent("PID 45", v.ToString("f3"), "%", PID_MEAN.PID_45_DATA);
        }

        /// <summary>
        /// 环境空气温度
        /// </summary>
        /// <param name="dataA"></param>
        private static void PID_0X46(char[] dataA)
        {
            double a = Convert.ToInt32(CharToString(dataA), 2);
            double v = a - 40;
            WriteContent("PID 46", v.ToString("f3"), "℃", PID_MEAN.PID_46_DATA);
        }

        /// <summary>
        /// 绝对气流阀位置B
        /// </summary>
        /// <param name="dataA"></param>
        private static void PID_0X47(char[] dataA)
        {
            double a = Convert.ToInt32(CharToString(dataA), 2);
            double v = a * (100 / 255.00);
            WriteContent("PID 47", v.ToString("f3"), "%", PID_MEAN.PID_47_DATA);
        }
        /// <summary>
        /// 绝对气流阀位置C
        /// </summary>
        /// <param name="dataA"></param>
        private static void PID_0X48(char[] dataA)
        {
            double a = Convert.ToInt32(CharToString(dataA), 2);
            double v = a * (100 / 255.00);
            WriteContent("PID 48", v.ToString("f3"), "%", PID_MEAN.PID_48_DATA);
        }

        /// <summary>
        /// 绝对气流阀位置D
        /// </summary>
        /// <param name="dataA"></param>
        private static void PID_0X49(char[] dataA)
        {
            double a = Convert.ToInt32(CharToString(dataA), 2);
            double v = a * (100 / 255.00);
            WriteContent("PID 49", v.ToString("f3"), "%", PID_MEAN.PID_49_DATA);
        }

        /// <summary>
        /// 绝对气流阀位置E
        /// </summary>
        /// <param name="dataA"></param>
        private static void PID_0X4A(char[] dataA)
        {
            double a = Convert.ToInt32(CharToString(dataA), 2);
            double v = a * (100 / 255.00);
            WriteContent("PID 4A", v.ToString("f3"), "%", PID_MEAN.PID_4A_DATA);
        }

        /// <summary>
        /// 绝对气流阀位置F
        /// </summary>
        /// <param name="dataA"></param>
        private static void PID_0X4B(char[] dataA)
        {
            double a = Convert.ToInt32(CharToString(dataA), 2);
            double v = a * (100 / 255.00);
            WriteContent("PID 4B", v.ToString("f3"), "%", PID_MEAN.PID_4B_DATA);
        }

        /// <summary>
        /// 通讯节流阀执行机构控制
        /// </summary>
        /// <param name="dataA"></param>
        private static void PID_0X4C(char[] dataA)
        {
            double a = Convert.ToInt32(CharToString(dataA), 2);
            double v = a * (100 / 255.00);
            WriteContent("PID 4C", v.ToString("f3"), "%", PID_MEAN.PID_4C_DATA);
        }

        /// <summary>
        /// 启动MIL时引擎运行的时间
        /// </summary>
        /// <param name="dataA"></param>
        /// <param name="dataB"></param>
        private static void PID_0X4D(char[] dataA, char[] dataB)
        {
            // a * 256 + b
            double a = Convert.ToInt32(CharToString(dataA), 2);
            double b = Convert.ToInt32(CharToString(dataB), 2);
            double v = a * 256 + b;
            WriteContent("PID 4D", v.ToString("f3"), "分钟", PID_MEAN.PID_4D_DATA);
        }

        /// <summary>
        /// 诊断故障码清除以来的时间
        /// </summary>
        /// <param name="dataA"></param>
        /// <param name="dataB"></param>
        private static void PID_0X4E(char[] dataA, char[] dataB)
        {
            double a = Convert.ToInt32(CharToString(dataA), 2);
            double b = Convert.ToInt32(CharToString(dataB), 2);
            double v = a * 256 + b;
            WriteContent("PID 4E", v.ToString("f3"), "分钟", PID_MEAN.PID_4E_DATA);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataA">等值比的最大值</param>
        /// <param name="dataB">氧传感器电压最大值</param>
        /// <param name="dataC">氧传感器电流最大值</param>
        /// <param name="dataD">进气岐管绝对压力的最大值</param>
        private static void PID_0X4F(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {
            double a = Convert.ToInt32(CharToString(dataA),2);
            double b = Convert.ToInt32(CharToString(dataB), 2);
            double c = Convert.ToInt32(CharToString(dataC), 2);
            double d = Convert.ToInt32(CharToString(dataD), 2);

            WriteContent("PID 4F", a.ToString("f3"), "", PID_MEAN.PID_4F_DATA_A);
            WriteContent("", b.ToString("f3"), "V", PID_MEAN.PID_4F_DATA_B);
            WriteContent("", c.ToString("f3"), "mA", PID_MEAN.PID_4F_DATA_C);
            WriteContent("", d.ToString("f3"), "kPa", PID_MEAN.PID_4F_DATA_D);
        }
        #endregion

        #region pid 50-ff
        private static void PID_0X50(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {
            
        }
        /// <summary>
        /// 车辆目前使用的燃料类型
        /// </summary>
        /// <param name="dataA"></param>
        private static void PID_0X51(char[] dataA)
        {
            WriteContent("PID 51", CharToString(dataA), "", PID_MEAN.PID_51_DATA);
        }

        private static void PID_0X52(char[] dataA)
        {
            double a = Convert.ToInt32(CharToString(dataA),2);
            double v = a * (100 / 255.00);
            WriteContent("PID 52", v.ToString("f3"), "%", PID_MEAN.PID_52_DATA);
        }

        /// <summary>
        /// 酒精燃料比例
        /// </summary>
        /// <param name="dataA"></param>
        /// <param name="dataB"></param>
        private static void PID_0X53(char[] dataA, char[] dataB)
        {
            double a = Convert.ToInt32(CharToString(dataA), 16);
            double b = Convert.ToInt32(CharToString(dataB), 16);
            double v = (a * 256 + b) / 200.00;
            WriteContent("PID 53", v.ToString("f3"), "%", PID_MEAN.PID_53_DATA);
        }

        /// <summary>
        /// 蒸汽系统压力
        /// </summary>
        /// <param name="dataA"></param>
        /// <param name="dataB"></param>
        private static void PID_0X54(char[] dataA, char[] dataB)
        {
            double a = Convert.ToInt32(CharToString(dataA), 16);
            double b = Convert.ToInt32(CharToString(dataB), 16);
            double v = (a * 256 + b) - 32767;
            WriteContent("PID 54", v.ToString("f3"), "Pa", PID_MEAN.PID_54_DATA);
        }
        /// <summary>
        /// 短期二次氧传感器微调组1和组3
        /// </summary>
        /// <param name="dataA"></param>
        /// <param name="dataB"></param>
        private static void PID_0X55(char[] dataA, char[] dataB)
        {
            double a = Convert.ToInt32(CharToString(dataA), 2);
            double b = Convert.ToInt32(CharToString(dataB), 2);
            double v1 = (a - 128) * (100 / 128.00);
            double v2 = (b - 128) * (100 / 128.00);
            WriteContent("PID 55", v1.ToString("f3"), "%", PID_MEAN.PID_55_DATA_A);
            WriteContent("", v2.ToString("f3"), "%", PID_MEAN.PID_55_DATA_B);
        }
        /// <summary>
        /// 长期二次氧传感器微调组1和组3
        /// </summary>
        /// <param name="dataA"></param>
        /// <param name="dataB"></param>
        private static void PID_0X56(char[] dataA, char[] dataB)
        {
            double a = Convert.ToInt32(CharToString(dataA), 2);
            double b = Convert.ToInt32(CharToString(dataB), 2);
            double v1 = (a - 128) * (100 / 128.00);
            double v2 = (b - 128) * (100 / 128.00);
            WriteContent("PID 56", v1.ToString("f3"), "%", PID_MEAN.PID_56_DATA_A);
            WriteContent("", v2.ToString("f3"), "%", PID_MEAN.PID_56_DATA_B);
        }
        /// <summary>
        /// 短期二次氧传感器微调组2和4
        /// </summary>
        /// <param name="dataA"></param>
        /// <param name="dataB"></param>
        private static void PID_0X57(char[] dataA, char[] dataB)
        {
            double a = Convert.ToInt32(CharToString(dataA), 2);
            double b = Convert.ToInt32(CharToString(dataB), 2);
            double v1 = (a - 128) * (100 / 128.00);
            double v2 = (b - 128) * (100 / 128.00);
            WriteContent("PID 57", v1.ToString("f3"), "%", PID_MEAN.PID_57_DATA_A);
            WriteContent("", v2.ToString("f3"), "%", PID_MEAN.PID_57_DATA_B);
        }
        /// <summary>
        /// 长期二次氧传感器微调组2和组4
        /// </summary>
        /// <param name="dataA"></param>
        /// <param name="dataB"></param>
        private static void PID_0X58(char[] dataA, char[] dataB)
        {
            double a = Convert.ToInt32(CharToString(dataA), 2);
            double b = Convert.ToInt32(CharToString(dataB), 2);
            double v1 = (a - 128) * (100 / 128.00);
            double v2 = (b - 128) * (100 / 128.00);
            WriteContent("PID 58", v1.ToString("f3"), "%", PID_MEAN.PID_58_DATA_A);
            WriteContent("", v2.ToString("f3"), "%", PID_MEAN.PID_58_DATA_B);
        }
        /// <summary>
        /// 绝对油轨压力
        /// </summary>
        /// <param name="dataA"></param>
        /// <param name="dataB"></param>
        private static void PID_0X59(char[] dataA, char[] dataB)
        {
            double a = Convert.ToInt32(CharToString(dataA), 2);
            double b = Convert.ToInt32(CharToString(dataB), 2);
            double v1 = (a * 256 + b) * 10;
            WriteContent("PID 59", v1.ToString("f3"), "%", PID_MEAN.PID_59_DATA);
        }

        /// <summary>
        /// 相对加速踏板位置
        /// </summary>
        /// <param name="dataA"></param>
        private static void PID_0X5A(char[] dataA)
        {
            double a = Convert.ToInt32(CharToString(dataA), 2);
            double v1 = a * (100 / 255.00);
            device.TempBuffer.AppendLine($"{PID_MEAN.PID_5A_DATA} {v1}%");
            WriteContent("PID 5A", v1.ToString("f3"), "%", PID_MEAN.PID_5A_DATA);

        }

        private static void PID_0X5B(char[] dataA)
        {
            
        }


        private static void PID_0XFD(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {
            double a = Convert.ToInt32(CharToString(dataA), 16);

        }

        private static void PID_0XFE(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0XFF(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }
        #endregion


        private static void ExcuteAllSupportAddress()
        {
            for (int i = 0; i < funCodeList.Count; i++)
            {
                pid = funCodeList[i];
                LogHelper.Log.Info($"【PID】={pid}({Convert.ToString(pid,16)})");
                PassThruWriteMsgs(true);
            }
        }

        /// <summary>
        /// 字符数组转字符串
        /// </summary>
        /// <param name="ary"></param>
        /// <returns></returns>
        private static string CharToString(char[] ary)
        {
            string strTemp = "";
            foreach (var v in ary)
            {
                strTemp += v;
            }
            return strTemp;
        }

        /// <summary>
        /// 传入字符串参数，设置定长输出
        /// </summary>
        /// <param name="pid"></param>
        /// <param name="calRes"></param>
        /// <param name="unit"></param>
        /// <param name="explain"></param>
        private static void WriteContent(string pid, string calRes, string unit, string explain)
        {
            byte[] ognPid = Encoding.Default.GetBytes(pid);
            byte[] ognCalRes = Encoding.Default.GetBytes(calRes);
            byte[] ognUnit = Encoding.Default.GetBytes(unit);
            byte[] ognExplain = Encoding.Default.GetBytes(explain);

            //声明指定长度字符串
            byte[] arrayPid = new byte[10];
            byte[] arrayCalRes = new byte[20];
            byte[] arrayUnit = new byte[10];
            byte[] arrayExplain = new byte[ognExplain.Length + 5];

            //pid
            for (int i = 0; i < arrayPid.Length; i++)
            {
                if (i < ognPid.Length)
                {
                    arrayPid[i] = ognPid[i];
                }
                else
                {
                    arrayPid[i] = 0x20;
                }
            }

            //calRes
            for (int i = 0; i < arrayCalRes.Length; i++)
            {
                if (i < ognCalRes.Length)
                {
                    arrayCalRes[i] = ognCalRes[i];
                }
                else
                {
                    arrayCalRes[i] = 0x20;
                }
            }

            //unit
            for (int i = 0; i < arrayUnit.Length; i++)
            {
                if (i < ognUnit.Length)
                {
                    arrayUnit[i] = ognUnit[i];
                }
                else
                {
                    arrayUnit[i] = 0x20;
                }
            }

            //explain
            for (int i = 0; i < arrayExplain.Length; i++)
            {
                if (i < ognExplain.Length)
                {
                    arrayExplain[i] = ognExplain[i];
                }
                else
                {
                    arrayExplain[i] = 0x20;
                }
            }

            pid = Encoding.Default.GetString(arrayPid);
            calRes = Encoding.Default.GetString(arrayCalRes);
            unit = Encoding.Default.GetString(arrayUnit);
            explain = Encoding.Default.GetString(arrayExplain);

            device.TempBuffer.AppendLine($"{pid + calRes + unit + explain}");
        }
    }
}
