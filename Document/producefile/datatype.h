#ifndef _DATATYPE_H_
#define _DATATYPE_H_

#define V_UINT 0
#define V_INT  1
#define V_FL4  2
#define V_FL8  3
#define V_STR  7
#define C_CHAR 8
    /* exact-width signed integer types */
typedef   signed          char int8_t;
typedef   signed short     int int16_t;
typedef   signed           int int32_t;

    /* exact-width unsigned integer types */
typedef unsigned          char uint8_t;
typedef unsigned short     int uint16_t;
typedef unsigned           int uint32_t;

typedef  struct XCPDataRecordType
{
	char*    name;
	char*    desc;
	char*    unit;
	uint8_t  datatype;   //定义如上 V_UINT  V_INT V_FL4 V_FL8
	uint8_t  datalen;    //在MDF中保存数据长度 
//	A2l解析时候 根据数据类型定义 每8bit 为1  定义如下  DBC解析时根据长度长度
/*   A2ML       ASAP2          Windows  Explanation                              */
/*   ---------------------------------------------------------                   */
/*   uchar      UBYTE          BYTE     unsigned 8 Bit                           */
/*   char       SBYTE          char     signed 8 Bit                             */
/*   uint       UWORD          WORD     unsigned integer 16 Bit                  */
/*   int        SWORD          int      signed integer 16 Bit                    */
/*   ulong      ULONG          DWORD    unsigned integer 32 Bit                  */
/*   long       SLONG          LONG     signed integer 32 Bit                    */
/*   float      FLOAT32_IEEE            float 32 Bit                             */

	uint8_t  is_motorola;//是否intel格式 
	uint8_t  dataexaddr; //截取开始地址  DBC有用  A2l 无用
	uint8_t  databitlen; //截取长度	    DBC有用  
	uint32_t dataaddr;   //A2l 数据地址  Mornitor 的CANID
	double   changea;    //factor
	double   changeb;    //offset
	//name + describle+unit+dataType+dataLen+IsMotorola+dataStartIndex+dataBitLen+dataAddress+factor+offset
}XCPDataRecordType;

typedef struct ExInfoType
{
		uint8_t  idtype;//0:a2l 的CAN_ID_SLAVE 或DAQID   1:A2l 的CAN_ID_MASTER  2 描述数组的地址
		uint32_t idval; 
	  uint8_t  idsubinfo; //当idtype=2 时 该值有效  值为描述数组的个数
}ExInfoType;
//(int8)(type=0/1/2),int32(val),int8(type=2时有效=当前数组长度)

typedef struct CanChInfo
{
		uint8_t  protocol; //协议类型 0 通道关闭  1：CCP 2：XCP 3：CanMonnitor
		uint32_t baud;     //通道波特率
	  uint32_t rxidtab;  //映射到传输的信息数组地址
		uint8_t  rxidnum;  //信息中数据的个数
}CanChInfo;


#endif
