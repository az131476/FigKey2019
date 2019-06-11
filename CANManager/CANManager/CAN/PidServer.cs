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
        public const string PID_01_DATAA_0_6 = "pid=01;dataA binaryBit=0-6 意义：与排放相关的DTC数量 结果：";
        public const string PID_01_DATAA_07 = "pid=01;dataA binaryBit=07 意义：故障指示灯状态 结果：";

        public const string PID_01_DATAB_00 = "pid=01;dataB binaryBit=00 意义：失火监控支持 结果：";
        public const string PID_01_DATAB_01 = "pid=01;dataB binaryBit=01 意义：燃油系统监控支持 结果：";
        public const string PID_01_DATAB_02 = "pid=01;dataB binaryBit=02 意义：支持全面组件监控 结果：";
        public const string PID_01_DATAB_03 = "pid=01;dataB binaryBit=03 意义：保留";
        public const string PID_01_DATAB_04 = "pid=01;dataB binaryBit=04 意义：失火检测就绪 结果：";
        public const string PID_01_DATAB_05 = "pid=01;dataB binaryBit=05 意义：燃油系统检测就绪 结果：";
        public const string PID_01_DATAB_06 = "pid=01;dataB binaryBit=06 意义：综合组件检测就绪 结果：";
        public const string PID_01_DATAB_07 = "pid=01;dataB binaryBit=07 意义：保留";

        public const string PID_01_DATAC_00 = "pid=01;dataC binaryBit=00 意义：催化剂监测支持 结果：";
        public const string PID_01_DATAC_01 = "pid=01;dataC binaryBit=01 意义：加热催化器监测支持 结果：";
        public const string PID_01_DATAC_02 = "pid=01;dataC binaryBit=02 意义：蒸发系统监测支持 结果：";
        public const string PID_01_DATAC_03 = "pid=01;dataC binaryBit=03 意义：二次空气系统监测支持 结果：";
        public const string PID_01_DATAC_04 = "pid=01;dataC binaryBit=04 意义：A/C系统制冷剂监控支持 结果：";
        public const string PID_01_DATAC_05 = "pid=01;dataC binaryBit=05 意义：氧传感器监测支持 结果：";
        public const string PID_01_DATAC_06 = "pid=01;dataC binaryBit=06 意义：氧传感器加热器监测支持 结果：";
        public const string PID_01_DATAC_07 = "pid=01;dataC binaryBit=07 意义：EGR系统支持 结果：";

        public const string PID_01_DATAD_00 = "pid=01;dataD binaryBit=00 意义：催化剂监测就绪 结果：";
        public const string PID_01_DATAD_01 = "pid=01;dataD binaryBit=01 意义：加热催化器监测就绪 结果：";
        public const string PID_01_DATAD_02 = "pid=01;dataD binaryBit=02 意义：蒸发系统监测就绪 结果：";
        public const string PID_01_DATAD_03 = "pid=01;dataD binaryBit=03 意义：二次空气系统监测就绪 结果：";
        public const string PID_01_DATAD_04 = "pid=01;dataD binaryBit=04 意义：A/C系统制冷剂监控就绪 结果：";
        public const string PID_01_DATAD_05 = "pid=01;dataD binaryBit=05 意义：氧传感器监测就绪 结果：";
        public const string PID_01_DATAD_06 = "pid=01;dataD binaryBit=06 意义：氧传感器加热器监测就绪 结果：";
        public const string PID_01_DATAD_07 = "pid=01;dataD binaryBit=07 意义：EGR系统就绪 结果：";
        #endregion

        #region PID=02
        public const string PID_02_DATAB_00 = "pid=02;dataA/B  意义：导致需要冻结帧数据存储的DTC 结果：";
        #endregion

        #region PID=03
        public const string PID_03_DATAA_00 = "pid=03;dataA binaryBit=00 意义：开环-还未达到闭环条件 结果：";
        public const string PID_03_DATAA_01 = "pid=03;dataA binaryBit=01 意义：闭环-使用氧传感器作为燃料控制的反馈 结果：";
        public const string PID_03_DATAA_02 = "pid=03;dataA binaryBit=02 意义：开环-由于驱动条件（e.g动力富集、减速堆积） 结果：";
        public const string PID_03_DATAA_03 = "pid=03;dataA binaryBit=03 意义：开环-检测到系统故障 结果：";
        public const string PID_03_DATAA_04 = "pid=03;dataA binaryBit=04 意义：闭环-至少一个氧传感器故障（可能使用单一氧传感器燃料控制） 结果：";
        public const string PID_03_DATAA_05 = "pid=03;dataA binaryBit=05 意义：保留 结果：";
        public const string PID_03_DATAA_06 = "pid=03;dataA binaryBit=06 意义：保留 结果：";
        public const string PID_03_DATAA_07 = "pid=03;dataA binaryBit=07 意义：保留 结果：";

        public const string PID_03_DATAB_00 = "pid=03;dataA binaryBit=05 意义：开环-还未达到闭环条件 结果：";
        public const string PID_03_DATAB_01 = "pid=03;dataA binaryBit=05 意义：闭环-使用氧传感器作为燃料控制的反馈 结果：";
        public const string PID_03_DATAB_02 = "pid=03;dataA binaryBit=05 意义：开环-由于驱动条件（e.g动力富集、减速堆积） 结果：";
        public const string PID_03_DATAB_03 = "pid=03;dataA binaryBit=05 意义：开环-检测到系统故障 结果：";
        public const string PID_03_DATAB_04 = "pid=03;dataA binaryBit=05 意义：闭环-至少一个氧传感器故障（可能使用单一氧传感器燃料控制） 结果：";
        public const string PID_03_DATAB_05 = "pid=03;dataA binaryBit=05 意义：保留 结果：";
        public const string PID_03_DATAB_06 = "pid=03;dataA binaryBit=05 意义：保留 结果：";
        public const string PID_03_DATAB_07 = "pid=03;dataA binaryBit=05 意义：保留 结果：";
        #endregion

        #region PID=04
        public const string PID_04_DATAA = "pid=04;dataA binaryBit=00 意义：计算负荷量（min:0 max:100） 结果：";
        #endregion

        #region PID=05
        public const string PID_05_DATAA = "pid=05;dataA binaryBit=00 意义：发动机冷却液温度（min:40℃ max:215℃） 结果：";
        #endregion

        #region PID=06-09
        public const string PID_06_DATAA = "pid=06;dataA 意义：短期燃料调整BANK1 短期燃料调整BANK3 结果：";
        public const string PID_07_DATAA = "pid=06;dataA 意义：长期燃料调整BANK1 长期燃料调整BANK3 结果：";
        public const string PID_08_DATAA = "pid=06;dataA 意义：短期燃料调整BANK2 短期燃料调整BANK4 结果：";
        public const string PID_09_DATAA = "pid=06;dataA 意义：长期燃料调整BANK2 长期燃料调整BANK4 结果：";
        #endregion

        #region PID=0A-11
        public const string PID_0A_DATAA = "pid=0A;dataA 意义：油压（min:0 max:765kPa） 结果：";
        public const string PID_0B_DATAA = "pid=0B;dataA 意义：进气歧管绝对压力(min:0 max:255kPa) 结果：";

        public const string PID_0C_DATAA_B = "pid=0C;dataA&B 意义：发动机转速(min:0 max:16383.75rPm) 结果：";

        public const string PID_0D_DATAA = "pid=0D;dataA 意义：车辆速度(0-255) 结果：";
        public const string PID_0E_DATAA = "pid=0E;dataA 意义：1号气缸点火正时提前(-64-63.5°) 结果：";
        public const string PID_0F_DATAA = "pid=0F;dataA 意义：摄入空气温度（-40-215℃） 结果：";
        public const string PID_10_DATAA = "pid=10;dataA 意义：空气流量（0-655.35grams/sec） 结果：";
        public const string PID_11_DATAA = "pid=11;dataA 意义：绝对的节气门位置(0-100%) 结果：";
        #endregion

        #region PID=12
        public const string PID_12_DATAA_00 = "pid=12;dataA binaryBit=00 意义：UPS催化转化器上游 结果：";
        public const string PID_12_DATAA_01 = "pid=12;dataA binaryBit=01 意义：DNS催化转化器下游 结果：";
        public const string PID_12_DATAA_02 = "pid=12;dataA binaryBit=02 意义：OFF 大气关闭 结果：";
        public const string PID_12_DATAA_03 = "pid=12;dataA binaryBit=03 意义：保留 结果：";
        public const string PID_12_DATAA_04 = "pid=12;dataA binaryBit=04 意义：保留 结果：";
        public const string PID_12_DATAA_05 = "pid=12;dataA binaryBit=05 意义：保留 结果：";
        public const string PID_12_DATAA_06 = "pid=12;dataA binaryBit=06 意义：保留 结果：";
        public const string PID_12_DATAA_07 = "pid=12;dataA binaryBit=07 意义：保留 结果：";
        #endregion

        #region PID=13 氧传感器位置，PID$13不支持时才可由给定车辆支持
        public const string PID_13_DATAA_00 = "pid=13;dataA binaryBit=00 意义：O2s11 结果：";
        public const string PID_13_DATAA_01 = "pid=13;dataA binaryBit=01 意义：O2s12 结果：";
        public const string PID_13_DATAA_02 = "pid=13;dataA binaryBit=02 意义：O2s13 结果：";
        public const string PID_13_DATAA_03 = "pid=13;dataA binaryBit=03 意义：O2s14 结果：";
        public const string PID_13_DATAA_04 = "pid=13;dataA binaryBit=04 意义：O2s21 结果：";
        public const string PID_13_DATAA_05 = "pid=13;dataA binaryBit=05 意义：O2s22 结果：";
        public const string PID_13_DATAA_06 = "pid=13;dataA binaryBit=06 意义：O2s23 结果：";
        public const string PID_13_DATAA_07 = "pid=13;dataA binaryBit=07 意义：O2s24 结果：";
        #endregion

        #region PID=14-1B 
        public const string PID_14_DATA = "pid=14;dataA&B 意义：BANK1 – SENSOR1 结果：";
        public const string PID_15_DATA = "pid=15;dataA&B 意义：BANK1 – SENSOR2 结果：";
        public const string PID_16_DATA = "pid=16;dataA&B 意义：BANK1 – SENSOR3 结果：";
        public const string PID_17_DATA = "pid=17;dataA&B 意义：BANK1 – SENSOR4 结果：";
        public const string PID_18_DATA = "pid=18;dataA&B 意义：BANK2 – SENSOR1 结果：";
        public const string PID_19_DATA = "pid=19;dataA&B 意义：BANK2 – SENSOR2 结果：";
        public const string PID_1a_DATA = "pid=1a;dataA&B 意义：BANK2 – SENSOR3 结果：";
        public const string PID_1b_DATA = "pid=1b;dataA&B 意义：BANK2 – SENSOR4 DATA A(氧传感器输出电压)，DATA B（短期燃料平衡） 结果：";
        #endregion

        #region PID=14-1B 
        public const string PID_1C_DATA_01 = "pid=1C;binaryBit=01 data 意义：OBD 2 结果：";
        public const string PID_1C_DATA_02 = "pid=1C;binaryBit=02 data 意义：OBD   结果：";
        public const string PID_1C_DATA_03 = "pid=1C;binaryBit=03 data 意义：OBD OBD2 结果：";
        public const string PID_1C_DATA_04 = "pid=1C;binaryBit=04 data 意义：OBD 1 结果：";
        public const string PID_1C_DATA_05 = "pid=1C;binaryBit=05 data 意义：NO OBD 结果：";
        public const string PID_1C_DATA_06 = "pid=1C;binaryBit=06 data 意义：EOBD 结果：";
        public const string PID_1C_DATA_07 = "pid=1C;binaryBit=07 data 意义：EOBD OBD2 结果：";
        public const string PID_1C_DATA_08 = "pid=1C;binaryBit=08 data 意义：EOBD OBD 结果：";
        public const string PID_1C_DATA_09 = "pid=1C;binaryBit=09 data 意义：EOBD OBD OBD2 结果：";
        public const string PID_1C_DATA_0A = "pid=1C;binaryBit=0A data 意义：JOBD 结果：";
        public const string PID_1C_DATA_0B = "pid=1C;binaryBit=0B data 意义：JOBD OBD2 结果：";
        public const string PID_1C_DATA_0C = "pid=1C;binaryBit=0C data 意义：JOBD EOBD 结果：";
        public const string PID_1C_DATA_0D = "pid=1C;binaryBit=0D data 意义：JOBD EOBD OBD2 结果：";
        public const string PID_1C_DATA_0E = "pid=1C;binaryBit=0E data 意义：EURO IV B1 结果：";
        public const string PID_1C_DATA_0F = "pid=1C;binaryBit=0F data 意义：EURO IV B2 结果：";
        public const string PID_1C_DATA_10 = "pid=1C;binaryBit=10 data 意义：EURO C 结果：";
        public const string PID_1C_DATA_11 = "pid=1C;binaryBit=11 data 意义：EMD 结果：";
        #endregion

        #region PID=1D 氧传感器位置
        public const string PID_1D_DATAA_00 = "pid=1D;dataA binaryBit=00 意义：O2s11 结果：";
        public const string PID_1D_DATAA_01 = "pid=1D;dataA binaryBit=01 意义：O2s12 结果：";
        public const string PID_1D_DATAA_02 = "pid=1D;dataA binaryBit=02 意义：O2s13 结果：";
        public const string PID_1D_DATAA_03 = "pid=1D;dataA binaryBit=03 意义：O2s14 结果：";
        public const string PID_1D_DATAA_04 = "pid=1D;dataA binaryBit=04 意义：O2s21 结果：";
        public const string PID_1D_DATAA_05 = "pid=1D;dataA binaryBit=05 意义：O2s22 结果：";
        public const string PID_1D_DATAA_06 = "pid=1D;dataA binaryBit=06 意义：O2s23 结果：";
        public const string PID_1D_DATAA_07 = "pid=1D;dataA binaryBit=07 意义：O2s24 结果：";
        #endregion

        #region PID=1E
        public const string PID_1E_DATA_A_00 = "pid=1D;dataA binaryBit=00 意义：电源关闭状态 结果：";
        public const string PID_1E_DATA_A_01 = "pid=1D;dataA binaryBit=01 意义：保留 ";
        public const string PID_1E_DATA_A_02 = "pid=1D;dataA binaryBit=02 意义：保留 ";
        public const string PID_1E_DATA_A_03 = "pid=1D;dataA binaryBit=03 意义：保留 ";
        public const string PID_1E_DATA_A_04 = "pid=1D;dataA binaryBit=04 意义：保留 ";
        public const string PID_1E_DATA_A_05 = "pid=1D;dataA binaryBit=05 意义：保留 ";
        public const string PID_1E_DATA_A_06 = "pid=1D;dataA binaryBit=06 意义：保留 ";
        public const string PID_1E_DATA_A_07 = "pid=1D;dataA binaryBit=07 意义：保留 ";
        #endregion

        #region PID=1F-23
        public const string PID_1F_DATA = "pid=1D;字节长度=2 dataA&B 意义：引擎启动后运行时间（0-65535秒） 结果：";
        public const string PID_21_DATA = "pid=21;字节长度=2 dataA&B 意义：MIL灯点亮后车辆行驶里程 结果：";
        public const string PID_22_DATA = "pid=22;字节长度=2 dataA&B 意义：燃料轨压力相对于管道真空(0-5177.265kPa) 结果：";
        public const string PID_23_DATA = "pid=23;字节长度=2 dataA&B 意义：油轨压力(柴油或汽油直喷)(0-655350kPa) 结果：";
        #endregion

        #region PID=24-2B,仅适用于PID13定义的氧传感器位置 1-2bank 1-4传感器
        public const string PID_24_DATA_13 = "pid=24;字节长度=4 dataA&B&C&D 意义：AB-等效比,CD-氧传感器电压 结果：";
        public const string PID_25_DATA_13 = "pid=25;字节长度=4 dataA&B&C&D 意义：AB-等效比,CD-氧传感器电压 结果：";
        public const string PID_26_DATA_13 = "pid=26;字节长度=4 dataA&B&C&D 意义：AB-等效比,CD-氧传感器电压 结果：";
        public const string PID_27_DATA_13 = "pid=27;字节长度=4 dataA&B&C&D 意义：AB-等效比,CD-氧传感器电压 结果：";
        public const string PID_28_DATA_13 = "pid=28;字节长度=4 dataA&B&C&D 意义：AB-等效比,CD-氧传感器电压 结果：";
        public const string PID_29_DATA_13 = "pid=29;字节长度=4 dataA&B&C&D 意义：AB-等效比,CD-氧传感器电压 结果：";
        public const string PID_2A_DATA_13 = "pid=2A;字节长度=4 dataA&B&C&D 意义：AB-等效比,CD-氧传感器电压 结果：";
        public const string PID_2B_DATA_13 = "pid=2B;字节长度=4 dataA&B&C&D 意义：AB-等效比,CD-氧传感器电压 结果：";
        #endregion

        #region PID=24,仅适用于PID1D定义的氧传感器位置 1-4bank 1-2传感器
        public const string PID_24_DATA_1D = "pid=24;字节长度=4 dataA&B&C&D 意义：AB-等效比,CD-氧传感器电压结果：";
        public const string PID_25_DATA_1D = "pid=25;字节长度=4 dataA&B&C&D 意义：AB-等效比,CD-氧传感器电压结果：";
        public const string PID_26_DATA_1D = "pid=26;字节长度=4 dataA&B&C&D 意义：AB-等效比,CD-氧传感器电压结果：";
        public const string PID_27_DATA_1D = "pid=27;字节长度=4 dataA&B&C&D 意义：AB-等效比,CD-氧传感器电压结果：";
        public const string PID_28_DATA_1D = "pid=28;字节长度=4 dataA&B&C&D 意义：AB-等效比,CD-氧传感器电压结果：";
        public const string PID_29_DATA_1D = "pid=29;字节长度=4 dataA&B&C&D 意义：AB-等效比,CD-氧传感器电压结果：";
        public const string PID_2A_DATA_1D = "pid=2A;字节长度=4 dataA&B&C&D 意义：AB-等效比,CD-氧传感器电压结果：";
        public const string PID_2B_DATA_1D = "pid=2B;字节长度=4 dataA&B&C&D 意义：AB-等效比,CD-氧传感器电压结果：";
        #endregion

        #region PID=2C-33
        public const string PID_2C_DATA = "pid=2C;字节长度=1 dataA 意义：EGR(废弃循环)命令(0-100%) 结果：";
        public const string PID_2D_DATA = "pid=2C;字节长度=1 dataA 意义：EGR(废弃循环)错误(-100-99.22%) 结果：";
        public const string PID_2E_DATA = "pid=2C;字节长度=1 dataA 意义：蒸发净化命令(0-100%) 结果：";
        public const string PID_2F_DATA = "pid=2C;字节长度=1 dataA 意义：油量液位情况(0-100%) 结果：";
        public const string PID_30_DATA = "pid=2C;字节长度=1 dataA 意义：自诊断故障清除后热身次数(0-255) 结果：";
        public const string PID_31_DATA = "pid=2C;字节长度=2 dataA 意义：自诊断故障清除后行驶距离(0-65535km) 结果：";
        public const string PID_32_DATA = "pid=2C;字节长度=2 dataA 意义：EVAP 系统蒸汽压力(-8192-8192Pa) 结果：";
        public const string PID_33_DATA = "pid=2C;字节长度=1 dataA 意义：大气压(0-255kPa) 结果：";

        #endregion

        #region PID=34-3B
        public const string PID_34_DATA = "pid=2C;字节长度=4 dataA 意义：油量液位情况(0-100%) 结果：";
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
        private static List<byte> funCodeList = new List<byte>();
        private static int frameAddressDef = 1;
        private static int frameAddressIndex;
        private static int frameSupportNext = 0;//是否继续支持下一帧,1-support
        private static bool IsSearchAllAddress;//查询所有支持的服务地址
        private static StringBuilder analysisContent = new StringBuilder();//解析数据缓存

        unsafe public static void PassThruStartMsgFilter(DeviceInfo device)
        {
            PassthruMsg pMaskMsg = new PassthruMsg();
            pMaskMsg.ProtocolID = (uint)device.ProtocolID;
            pMaskMsg.DataSize = 4;
            if (pMaskMsg.ProtocolID == (uint)DeviceInfo.ProtocolType.ISO15765)
            {
                pMaskMsg.TxFlags = ISO15765_FRAME_PAD;
                pMaskMsg.Data[0] = 0xFF;
                pMaskMsg.Data[1] = 0xFF;
                pMaskMsg.Data[2] = 0xFF;
                pMaskMsg.Data[3] = 0xFF;
            }
            else if(pMaskMsg.ProtocolID == (uint)DeviceInfo.ProtocolType.ISO14230)
            {
                pMaskMsg.TxFlags = 0;
                pMaskMsg.Data[0] = 0x00;
                pMaskMsg.Data[1] = 0xFF;
                pMaskMsg.Data[2] = 0xFF;
                pMaskMsg.Data[3] = 0x00;
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
            int res_startMsgFilter = MonGoose.MonStartMsgFilter(device.ChannelID, FLOW_CONTROL_FILTER,ref pMaskMsg, ref pPatternMsg, ref pFlowControlMsg, ref pMsgID);
            LogHelper.Log.Info($"res_startMsgFilter:{res_startMsgFilter}");
        }

        unsafe public static void CommandMode(DeviceInfo deviceInfo)
        {
            device = deviceInfo;

            switch (device.ModelSidType)
            {
                case DeviceInfo.ModelType.MODEL1:
                    PassThruWriteMsgs(false);
                    break;
                case DeviceInfo.ModelType.MODEL2:

                    break;
                case DeviceInfo.ModelType.MODEL3:

                    break;
                case DeviceInfo.ModelType.MODEL4:

                    break;
                case DeviceInfo.ModelType.MODEL5:

                    break;
                case DeviceInfo.ModelType.MODEL6:

                    break;
                case DeviceInfo.ModelType.MODEL7:

                    break;
                case DeviceInfo.ModelType.MODEL8:

                    break;
                case DeviceInfo.ModelType.MODEL9:

                    break;
                case DeviceInfo.ModelType.MODELA:

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
            int res_send = MonGoose.MonWriteMsgs(device.ChannelID, ref writeStruct, ref pNumMsg, timeout);
            LogHelper.Log.Info($"res_send:{res_send}");
            //发送完成后，去读返回值
            if (!IsAllSupport)
            {
                PassThruReadMsgs();
            }
            else
            {
                AnalysisReceive();
            }
        }

        unsafe private static void PassThruReadMsgs()
        {
            //读数据
            string framePerBinary = "";
            PassthruMsg Msg = new PassthruMsg();
            Msg.TxFlags = ISO15765_FRAME_PAD;
            Msg.ProtocolID = (uint)device.ProtocolID;
            uint pnumMsg = 1;
            uint timeout = 100;
            int res_read = MonGoose.MonReadMsgs(device.ChannelID, ref Msg, ref pnumMsg, timeout);
            if (Msg.Data[4] == (int)ModelRebackValueSID.PID_OX41)
            {
                if (Msg.Data[5] == pid)
                {
                    framePerBinary = Msg.Data[6].ToString() + Msg.Data[7].ToString() + Msg.Data[8].ToString() + Msg.Data[9].ToString();
                    LogHelper.Log.Info($"framePerString:" + framePerBinary);
                    framePerBinary = Convert.ToString(Convert.ToInt32(framePerBinary),2);
                    LogHelper.Log.Info($"framePerBinary:"+framePerBinary);
                    char[] curFrameArray = framePerBinary.ToCharArray();
                    //将该帧中支持的地址添加到集合
                    for (frameAddressIndex = frameAddressDef; frameAddressIndex <= curFrameArray.Length; frameAddressIndex++)
                    {
                        if (curFrameArray[frameAddressIndex] == 1)//1-support,0-not support address
                        {
                            funCodeList.Add(Convert.ToByte(Convert.ToString(frameAddressIndex, 16)));
                        }
                    }
                    //判断帧最后一位是否支持
                    if (frameAddressIndex == curFrameArray.Length)
                    {
                        if (curFrameArray[frameAddressIndex] == 1)
                        {
                            frameAddressDef += 32;
                            frameSupportNext = 1;//继续
                            pid += 0x20;
                            PassThruWriteMsgs(false);
                        }
                        else
                        {
                            //查询不到下一帧有支持的地址
                            //查询所有已知的服务地址，并解析
                            frameSupportNext = 0;
                            ExcuteAllSupportAddress();
                        }
                    }
                }
            }
        }

        unsafe private static void AnalysisReceive()
        {
            //读数据
            PassthruMsg Msg = new PassthruMsg();
            Msg.TxFlags = ISO15765_FRAME_PAD;
            Msg.ProtocolID = (uint)device.ProtocolID;
            uint pnumMsg = 1;
            uint timeout = 100;
            int res_read = MonGoose.MonReadMsgs(device.ChannelID, ref Msg, ref pnumMsg, timeout);
            if (Msg.Data[4] == (int)ModelRebackValueSID.PID_OX41)
            {
                if (Msg.Data[5] == pid)
                {
                    string framePerBinaryDataA = Msg.Data[6].ToString();
                    string framePerBinaryDataB = Msg.Data[7].ToString();
                    string framePerBinaryDataC = Msg.Data[8].ToString();
                    string framePerBinaryDataD = Msg.Data[9].ToString();

                    framePerBinaryDataA = Convert.ToString(Convert.ToInt32(framePerBinaryDataA), 2);
                    framePerBinaryDataB = Convert.ToString(Convert.ToInt32(framePerBinaryDataB), 2);
                    framePerBinaryDataC = Convert.ToString(Convert.ToInt32(framePerBinaryDataC), 2);
                    framePerBinaryDataD = Convert.ToString(Convert.ToInt32(framePerBinaryDataD), 2);

                    switch (pid)
                    {
                        case (int)Model_PID.PID_0X01:
                            PID_0XO1(framePerBinaryDataA.ToCharArray(),framePerBinaryDataB.ToCharArray(),framePerBinaryDataC.ToCharArray(),framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X02:
                            PID_0XO2(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_OX03:
                            PID_0XO3(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_OX04:
                            PID_0XO4(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X05:
                            PID_0XO5(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X06:
                            PID_0XO6(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X07:
                            PID_0XO7(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X08:
                            PID_0XO8(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X09:
                            PID_0XO9(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X0A:
                            PID_0X0A(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X0B:
                            PID_0X0B(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X0C:
                            PID_0X0C(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X0D:
                            PID_0X0D(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X0E:
                            PID_0X0E(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X0F:
                            PID_0X0F(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X10:
                            PID_0X10(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X11:
                            PID_0X11(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X12:
                            PID_0X12(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X13:
                            PID_0X13(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X14:
                            PID_0X14(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X15:
                            PID_0X15(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X16:
                            PID_0X16(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X17:
                            PID_0X17(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X18:
                            PID_0X18(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X19:
                            PID_0X19(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X1A:
                            PID_0X1A(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X1B:
                            PID_0X1B(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X1C:
                            PID_0X1C(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X1D:
                            PID_0X1D(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X1E:
                            PID_0X1E(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X1F:
                            PID_0X1F(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X21:
                            PID_0X21(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X22:
                            PID_0X22(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X23:
                            PID_0X23(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
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
                            PID_0X2C(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X2D:
                            PID_0X2D(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X2E:
                            PID_0X2E(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X2F:
                            PID_0X2F(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X30:
                            PID_0X30(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X31:
                            PID_0X31(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X32:
                            PID_0X32(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X33:
                            PID_0X33(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
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
                            PID_0X3C(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X3D:
                            PID_0X3D(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X3E:
                            PID_0X3E(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X3F:
                            PID_0X3F(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X40:
                            PID_0X40(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X41:
                            PID_0X41(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X42:
                            PID_0X42(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X43:
                            PID_0X43(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X44:
                            PID_0X44(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X45:
                            PID_0X45(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X46:
                            PID_0X46(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X47:
                            PID_0X47(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X48:
                            PID_0X48(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X49:
                            PID_0X49(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X4A:
                            PID_0X4A(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X4B:
                            PID_0X4B(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X4C:
                            PID_0X4C(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X4D:
                            PID_0X4D(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X4E:
                            PID_0X4E(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X4F:
                            PID_0X4F(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X50:
                            PID_0X50(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X51:
                            PID_0X51(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X52:
                            PID_0X52(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X53:
                            PID_0X53(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X54:
                            PID_0X54(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X55:
                            PID_0X55(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X56:
                            PID_0X56(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X57:
                            PID_0X57(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X58:
                            PID_0X58(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X59:
                            PID_0X59(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X5A:
                            PID_0X5A(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
                            break;
                        case (int)Model_PID.PID_0X5B:
                            PID_0X5B(framePerBinaryDataA.ToCharArray(), framePerBinaryDataB.ToCharArray(), framePerBinaryDataC.ToCharArray(), framePerBinaryDataD.ToCharArray());
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
                    device.TempBuffer = analysisContent;
                }
            }
        }

        #region pid 01-0f
        private static void PID_0XO1(char[] dataA,char[] dataB,char[] dataC,char[] dataD)
        {
            for (int i = 0; i < dataA.Length; i++)
            {
                switch (i)
                {
                    //i=0-6 dtc number
                    case 7:
                        if (dataA[i] == 1)
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_01_DATAA_07} ON");
                        }
                        else
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_01_DATAA_07} OFF");
                        }
                        break;
                }
            }
            for (int i = 0; i < dataB.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        if (dataB[i] == 1)
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_01_DATAB_00} 支持");
                        }
                        else
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_01_DATAB_00} 不支持");
                        }
                        break;
                    case 1:
                        if (dataB[i] == 1)
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_01_DATAB_01} 支持");
                        }
                        else
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_01_DATAB_01} 不支持");
                        }
                        break;
                    case 2:
                        if (dataB[i] == 1)
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_01_DATAB_02} 支持");
                        }
                        else
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_01_DATAB_02} 不支持");
                        }
                        break;
                    case 3:
                        if (dataB[i] == 1)
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_01_DATAB_03} ");
                        }
                        else
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_01_DATAB_03} ");
                        }
                        break;
                    case 4:
                        if (dataB[i] == 1)
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_01_DATAB_04} 就绪");
                        }
                        else
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_01_DATAB_04} 未完成");
                        }
                        break;
                    case 5:
                        if (dataB[i] == 1)
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_01_DATAB_05} 就绪");
                        }
                        else
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_01_DATAB_05} 未完成");
                        }
                        break;
                    case 6:
                        if (dataB[i] == 1)
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_01_DATAB_06} 就绪");
                        }
                        else
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_01_DATAB_06} 未完成");
                        }
                        break;
                    case 7:
                        if (dataB[i] == 1)
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_01_DATAB_07} ");
                        }
                        else
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_01_DATAB_07} ");
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
                            analysisContent.AppendLine($"{PID_MEAN.PID_01_DATAC_00} 支持");
                        }
                        else
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_01_DATAC_00} 不支持");
                        }
                        break;
                    case 1:
                        if (dataC[i] == 1)
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_01_DATAC_01} 支持");
                        }
                        else
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_01_DATAC_01} 不支持");
                        }
                        break;
                    case 2:
                        if (dataC[i] == 1)
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_01_DATAC_02} 支持");
                        }
                        else
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_01_DATAC_02} 不支持");
                        }
                        break;
                    case 3:
                        if (dataC[i] == 1)
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_01_DATAC_03} 支持");
                        }
                        else
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_01_DATAC_03} 不支持");
                        }
                        break;
                    case 4:
                        if (dataC[i] == 1)
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_01_DATAC_04} 支持");
                        }
                        else
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_01_DATAC_04} 不支持");
                        }
                        break;
                    case 5:
                        if (dataC[i] == 1)
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_01_DATAC_05} 支持");
                        }
                        else
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_01_DATAC_05} 不支持");
                        }
                        break;
                    case 6:
                        if (dataC[i] == 1)
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_01_DATAC_06} 支持");
                        }
                        else
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_01_DATAC_06} 不支持");
                        }
                        break;
                    case 7:
                        if (dataC[i] == 1)
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_01_DATAC_07} 支持");
                        }
                        else
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_01_DATAC_07} 不支持");
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
                            analysisContent.AppendLine($"{PID_MEAN.PID_01_DATAD_00} 就绪");
                        }
                        else
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_01_DATAD_00} 未完成");
                        }
                        break;
                    case 1:
                        if (dataD[i] == 1)
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_01_DATAD_01} 就绪");
                        }
                        else
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_01_DATAD_01} 未完成");
                        }
                        break;
                    case 2:
                        if (dataD[i] == 1)
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_01_DATAD_02} 就绪");
                        }
                        else
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_01_DATAD_02} 未完成");
                        }
                        break;
                    case 3:
                        if (dataD[i] == 1)
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_01_DATAD_03} 就绪");
                        }
                        else
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_01_DATAD_03} 未完成");
                        }
                        break;
                    case 4:
                        if (dataD[i] == 1)
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_01_DATAD_04} 就绪");
                        }
                        else
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_01_DATAD_04} 未完成");
                        }
                        break;
                    case 5:
                        if (dataD[i] == 1)
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_01_DATAD_05} 就绪");
                        }
                        else
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_01_DATAD_05} 未完成");
                        }
                        break;
                    case 6:
                        if (dataD[i] == 1)
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_01_DATAD_06} 就绪");
                        }
                        else
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_01_DATAD_06} 未完成");
                        }
                        break;
                    case 7:
                        if (dataD[i] == 1)
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_01_DATAD_07} 就绪");
                        }
                        else
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_01_DATAD_07} 未完成");
                        }
                        break;
                }
            }
        }

        private static void PID_0XO2(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {
            analysisContent.AppendLine($"{PID_MEAN.PID_02_DATAB_00} dataA:{dataA.ToString()} dataB:{dataB.ToString()} dataC:{dataC.ToString()} dataD:{dataD.ToString()}");
        }

        private static void PID_0XO3(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {
            for (int i=0; i < dataA.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        if (dataA[i] == 1)
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_03_DATAA_00} 显示");
                        }
                        else
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_03_DATAA_00} 正常");
                        }
                        break;
                    case 1:
                        if (dataA[i] == 1)
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_03_DATAA_01} 显示");
                        }
                        else
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_03_DATAA_01} 正常");
                        }
                        break;
                    case 2:
                        if (dataA[i] == 1)
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_03_DATAA_02} 显示");
                        }
                        else
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_03_DATAA_02} 正常");
                        }
                        break;
                    case 3:
                        if (dataA[i] == 1)
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_03_DATAA_03} 显示");
                        }
                        else
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_03_DATAA_03} 正常");
                        }
                        break;
                    case 4:
                        if (dataA[i] == 1)
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_03_DATAA_04} 显示");
                        }
                        else
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_03_DATAA_04} 正常");
                        }
                        break;
                    case 5:
                        if (dataA[i] == 1)
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_03_DATAA_05} 无");
                        }
                        else
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_03_DATAA_05} 无");
                        }
                        break;
                    case 6:
                        if (dataA[i] == 1)
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_03_DATAA_06} 无");
                        }
                        else
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_03_DATAA_06} 无");
                        }
                        break;
                    case 7:
                        if (dataA[i] == 1)
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_03_DATAA_00} 无");
                        }
                        else
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_03_DATAA_00} 无");
                        }
                        break;
                }
            }

            for (int i = 0; i < dataB.Length; i++)
            {
                switch (i)
                {
                    case 0:
                        if (dataB[i] == 1)
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_03_DATAB_00} 显示");
                        }
                        else
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_03_DATAB_00} 正常");
                        }
                        break;
                    case 1:
                        if (dataB[i] == 1)
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_03_DATAB_01} 显示");
                        }
                        else
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_03_DATAB_01} 正常");
                        }
                        break;
                    case 2:
                        if (dataB[i] == 1)
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_03_DATAB_02} 显示");
                        }
                        else
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_03_DATAB_02} 正常");
                        }
                        break;
                    case 3:
                        if (dataB[i] == 1)
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_03_DATAB_03} 显示");
                        }
                        else
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_03_DATAB_03} 正常");
                        }
                        break;
                    case 4:
                        if (dataB[i] == 1)
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_03_DATAB_04} 显示");
                        }
                        else
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_03_DATAB_04} 正常");
                        }
                        break;
                    case 5:
                        if (dataB[i] == 1)
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_03_DATAB_05} 无");
                        }
                        else
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_03_DATAB_05} 无");
                        }
                        break;
                    case 6:
                        if (dataB[i] == 1)
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_03_DATAB_06} 无");
                        }
                        else
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_03_DATAB_06} 无");
                        }
                        break;
                    case 7:
                        if (dataB[i] == 1)
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_03_DATAB_07} 无");
                        }
                        else
                        {
                            analysisContent.AppendLine($"{PID_MEAN.PID_03_DATAB_07} 无");
                        }
                        break;
                }
            }
        }

        private static void PID_0XO4(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {
            //计算负荷值
            //公式：A * 100/255
            double v = Convert.ToInt32(dataA.ToString()) * (100 / 255.00);
            analysisContent.AppendLine($"{PID_MEAN.PID_04_DATAA} {v}%");
        }

        /// <summary>
        /// 发动机冷却液温度
        /// </summary>
        /// <param name="dataA"></param>
        private static void PID_0XO5(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {
            //公式：A-40
            double c = Convert.ToInt32(dataA.ToString()) - 40;
            analysisContent.AppendLine($"{PID_MEAN.PID_05_DATAA} {c}℃");
        }

        /// <summary>
        /// 短期燃料调整BANK1/3
        /// </summary>
        /// <param name="dataA"></param>
        private static void PID_0XO6(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {
            //公式：（A-128）(100/128)
            double k = (Convert.ToInt32(dataA.ToString())-128) * (100 / 128.00);
            analysisContent.AppendLine($"{PID_MEAN.PID_06_DATAA} {k}%");
        }

        /// <summary>
        /// 长期燃料调整BANK1/3
        /// </summary>
        /// <param name="dataA"></param>
        private static void PID_0XO7(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {
            double k = (Convert.ToInt32(dataA.ToString()) - 128) * (100 / 128.00);
            analysisContent.AppendLine($"{PID_MEAN.PID_07_DATAA} {k}%");
        }

        /// <summary>
        /// 短期燃料调整BANK2/4
        /// </summary>
        /// <param name="dataA"></param>
        private static void PID_0XO8(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {
            double k = (Convert.ToInt32(dataA.ToString()) - 128) * (100 / 128.00);
            analysisContent.AppendLine($"{PID_MEAN.PID_08_DATAA} {k}%");
        }

        /// <summary>
        /// 长期燃料调整BANK2/4
        /// </summary>
        /// <param name="dataA"></param>
        private static void PID_0XO9(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {
            double k = (Convert.ToInt32(dataA.ToString()) - 128) * (100 / 128);
            analysisContent.AppendLine($"{PID_MEAN.PID_09_DATAA} {k}%");
        }

        /// <summary>
        /// 燃油压力
        /// </summary>
        /// <param name="dataA"></param>
        private static void PID_0X0A(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {
            //公式：A*3
            double k = Convert.ToInt32(dataA.ToString(), 16) * 3;
            analysisContent.AppendLine($"{PID_MEAN.PID_0A_DATAA} {k}kPa");
        }

        /// <summary>
        /// 油箱压力绝对值
        /// </summary>
        /// <param name="dataA"></param>
        private static void PID_0X0B(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {
            double v = Convert.ToInt32(dataA.ToString());
            analysisContent.AppendLine($"{PID_MEAN.PID_0B_DATAA} {v}kPa");
        }

        /// <summary>
        /// 引擎转速
        /// </summary>
        /// <param name="dataA"></param>
        /// <param name="dataB"></param>
        private static void PID_0X0C(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {
            //公式：（A*256+B）/ 4
            double a = Convert.ToInt32(dataA.ToString());
            double b = Convert.ToInt32(dataB.ToString());
            double v = (a * 256 + b) / 4;
            analysisContent.AppendLine($"{PID_MEAN.PID_0C_DATAA_B} {v}rpm");
        }

        /// <summary>
        /// 车辆速度
        /// </summary>
        /// <param name="dataA"></param>
        private static void PID_0X0D(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {
            double s = Convert.ToInt32(dataA.ToString());
            analysisContent.AppendLine($"{PID_MEAN.PID_0D_DATAA} {s} km/h");
        }

        /// <summary>
        /// 点火提前值
        /// </summary>
        /// <param name="dataA"></param>
        private static void PID_0X0E(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {
            //公式：(A-128) /2
            double v = (Convert.ToInt32(dataA.ToString()) - 128) / 2;
            analysisContent.AppendLine($"{PID_MEAN.PID_0E_DATAA} {v}°");
        }

        /// <summary>
        /// 油箱空气温度
        /// </summary>
        /// <param name="dataA"></param>
        private static void PID_0X0F(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {
            //公式：A-40
            double v = Convert.ToInt32(dataA.ToString()) - 40;
            analysisContent.AppendLine($"{PID_MEAN.PID_0F_DATAA}");
        }
        #endregion

        #region pid 10-1f

        /// <summary>
        /// MAF空气流量速度
        /// </summary>
        /// <param name="dataA"></param>
        /// <param name="dataB"></param>
        private static void PID_0X10(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {
            //公式：（A * 256 + b） / 100
            double a = Convert.ToInt32(dataA.ToString());
            double b = Convert.ToInt32(dataB.ToString());
            double v = (a * 256 + b) / 100;
            analysisContent.AppendLine($"{PID_MEAN.PID_10_DATAA} {v}grams/sec");
        }

        /// <summary>
        /// 节气门位置
        /// </summary>
        /// <param name="dataA"></param>
        private static void PID_0X11(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {
            //公式:A * (100/255)
            double v = Convert.ToInt32(dataA.ToString()) * (100 / 255.00);
            analysisContent.AppendLine($"{PID_MEAN.PID_11_DATAA} {v}%");
        }

        private static void PID_0X12(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {
            for (int i = 0; i < dataA.Length; i++)
            {
                switch (i)
                {
                    case 0:

                        break;
                    case 1:

                        break;
                    case 2:

                        break;
                    case 3:

                        break;
                    case 4:

                        break;
                    case 5:

                        break;
                    case 6:

                        break;
                    case 7:

                        break;
                }
            }
        }

        private static void PID_0X13(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X14(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X15(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X16(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X17(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X18(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X19(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X1A(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X1B(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X1C(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X1D(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X1E(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X1F(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        #endregion

        #region pid 21-2f
        private static void PID_0X21(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X22(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X23(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X24(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X25(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X26(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X27(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X28(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X29(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X2A(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X2B(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X2C(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X2D(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X2E(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X2F(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        #endregion

        #region pid 30-3f
        private static void PID_0X30(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X31(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X32(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X33(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X34(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X35(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X36(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X37(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X38(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X39(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X3A(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X3B(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X3C(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X3D(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X3E(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X3F(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }
        #endregion

        #region pid 40-4f
        private static void PID_0X40(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X41(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X42(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X43(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X44(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X45(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X46(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X47(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X48(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X49(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X4A(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X4B(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X4C(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X4D(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X4E(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X4F(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }
        #endregion

        #region pid 50-ff
        private static void PID_0X50(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X51(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X52(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X53(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X54(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X55(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X56(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X57(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X58(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X59(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X5A(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }

        private static void PID_0X5B(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

        }


        private static void PID_0XFD(char[] dataA, char[] dataB, char[] dataC, char[] dataD)
        {

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
                PassThruWriteMsgs(true);
            }
        }
    }
}
