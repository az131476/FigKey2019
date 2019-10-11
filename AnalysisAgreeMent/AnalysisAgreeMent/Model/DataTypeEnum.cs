using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnalysisAgreeMent.Model
{
    /// <summary>
    /// 协议类型
    /// </summary>
    public enum AgreementType
    {
        CCP,
        XCP,
        DBC,
        other
    }

    /// <summary>
    /// 决定要导出的CAN类型
    /// </summary>
    public enum CurrentCanType
    {
        /// <summary>
        /// 仅选择CAN1
        /// </summary>
        CAN1,
        /// <summary>
        /// 仅选择CAN2
        /// </summary>
        CAN2,
        /// <summary>
        /// CAN1与CAN2都选择
        /// </summary>
        CAN1_CAN2,
        /// <summary>
        /// 未选择CAN
        /// </summary>
        NONE
    }

    public enum FileType
    {
        DBC =1,
        A2L =2,
        TXT =3,
        XML =4,
        INI =5,
        OTHER =6
    }

    public enum ByteOrder
    {
        BYTE_ORDER_MSB_LAST,
        BYTE_ORDER_MSB_FIRST
    };

    /// <summary>
    /// 保存到文件的数据类型
    /// </summary>
    public enum SaveDataTypeEnum
    {
        V_UINT = 0,
        V_INT = 1,
        V_FL4 = 2,
        V_FL8 = 3,
        V_STR = 7,
        V_char = 8,
        V_NULL
    }

    /// <summary>
    /// ASAP2协议数据类型，根据此数据类型计算数据保存长度
    /// </summary>
    public enum DataTypeEnum
    {
        /// <summary>
        /// unsigned 8 Bit      
        /// </summary>
        UBYTE,
        /// <summary>
        /// signed 8 Bit  
        /// </summary>
        SBYTE,
        /// <summary>
        /// unsigned integer 16 Bit
        /// </summary>
        UWORD,
        /// <summary>
        /// signed integer 16 Bit
        /// </summary>
        SWORD,
        /// <summary>
        /// unsigned integer 32 Bit
        /// </summary>
        ULONG,
        /// <summary>
        /// signed integer 32 Bit
        /// </summary>
        SLONG,
        /// <summary>
        /// float 32 Bit 
        /// </summary>
        FLOAT32_IEEE
    }
}
