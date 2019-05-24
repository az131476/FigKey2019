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
	uint8_t  datatype;   //�������� V_UINT  V_INT V_FL4 V_FL8
	uint8_t  datalen;    //��MDF�б������ݳ��� 
//	A2l����ʱ�� �����������Ͷ��� ÿ8bit Ϊ1  ��������  DBC����ʱ���ݳ��ȳ���
/*   A2ML       ASAP2          Windows  Explanation                              */
/*   ---------------------------------------------------------                   */
/*   uchar      UBYTE          BYTE     unsigned 8 Bit                           */
/*   char       SBYTE          char     signed 8 Bit                             */
/*   uint       UWORD          WORD     unsigned integer 16 Bit                  */
/*   int        SWORD          int      signed integer 16 Bit                    */
/*   ulong      ULONG          DWORD    unsigned integer 32 Bit                  */
/*   long       SLONG          LONG     signed integer 32 Bit                    */
/*   float      FLOAT32_IEEE            float 32 Bit                             */

	uint8_t  is_motorola;//�Ƿ�intel��ʽ 
	uint8_t  dataexaddr; //��ȡ��ʼ��ַ  DBC����  A2l ����
	uint8_t  databitlen; //��ȡ����	    DBC����  
	uint32_t dataaddr;   //A2l ���ݵ�ַ  Mornitor ��CANID
	double   changea;    //factor
	double   changeb;    //offset
	//name + describle+unit+dataType+dataLen+IsMotorola+dataStartIndex+dataBitLen+dataAddress+factor+offset
}XCPDataRecordType;

typedef struct ExInfoType
{
		uint8_t  idtype;//0:a2l ��CAN_ID_SLAVE ��DAQID   1:A2l ��CAN_ID_MASTER  2 ��������ĵ�ַ
		uint32_t idval; 
	  uint8_t  idsubinfo; //��idtype=2 ʱ ��ֵ��Ч  ֵΪ��������ĸ���
}ExInfoType;
//(int8)(type=0/1/2),int32(val),int8(type=2ʱ��Ч=��ǰ���鳤��)

typedef struct CanChInfo
{
		uint8_t  protocol; //Э������ 0 ͨ���ر�  1��CCP 2��XCP 3��CanMonnitor
		uint32_t baud;     //ͨ��������
	  uint32_t rxidtab;  //ӳ�䵽�������Ϣ�����ַ
		uint8_t  rxidnum;  //��Ϣ�����ݵĸ���
}CanChInfo;


#endif
