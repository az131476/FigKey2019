#include"datatype.h"

//数据格式：
//名称+描述+单位+数据类型+数据长度+字节顺序+截取开始地址(dbc有用)+
//截取长度+数据地址(a2l-ecu地址，monitor-canid)+系数+偏移量
static   XCPDataRecordType DataPart[]=
{
	"Name01","Describle1","unit1",V_INT, 2,0,0,0,0xD0001D30 ,0,0.1,     
	"Name02","Describle2","unit2",V_UINT,2,0,0,0,0xD0001D32 ,0,0.2,
	"Name03","Describle3","unit3",V_INT, 2,0,0,0,0xD0001D34 ,0,0.3,
	"Name04","Describle4","unit4",V_UINT,2,0,0,0,0xD0001D36 ,0,0.4,
	"Name05","Describle1","unit1",V_INT, 2,0,0,0,0xD0001D38 ,0,0.1,     
	"Name06","Describle2","unit2",V_UINT,2,0,0,0,0xD0001D3a ,0,0.2,
	"Name07","Describle3","unit3",V_INT, 2,0,0,0,0xD0001D3c ,0,0.3,
	"Name08","Describle4","unit4",V_UINT,1,0,0,0,0xD0001D3e ,0,0.4,
};

static   XCPDataRecordType DBCTab[] ={
	"Name01","Describle1","unit1",V_INT, 4,0,0,8,0x123 ,0,0.1,     
};


static   XCPDataRecordType DBCTab1[] ={
	"Name11","Describle1","unit1",V_INT, 4,0,0,8,0x124 ,0,0.1,     
	"Name12","Describle2","unit2",V_UINT,4,0,8,8,0x124 ,0,0.2,
};

static   XCPDataRecordType DBCTab2[] ={
	"Name21","Describle1","unit1",V_INT, 4,0,0,8,0x125 ,0,0.1,     
	"Name22","Describle2","unit2",V_UINT,4,0,8,8,0x125 ,0,0.2,
	"Name23","Describle3","unit3",V_INT, 4,0,16,8,0x125 ,0,0.3,
};

static   XCPDataRecordType DBCTab3[] ={
	"Name31","Describle1","unit1",V_INT, 4,0,0,8,0x126 ,0,0.1,     
	"Name32","Describle2","unit2",V_UINT,4,0,8,8,0x126 ,0,0.2,
	"Name33","Describle3","unit3",V_INT, 4,0,16,8,0x126 ,0,0.3,
	"Name34","Describle4","unit4",V_UINT,4,0,24,8,0x126 ,0,0.4,
};

static   ExInfoType ExInfo[]=
{
		 0,0x7f1,0,
	   1,0x7f0,0,
	   2,(uint32_t)DataPart,8,
	
	   2,(uint32_t)DBCTab,1,
	   2,(uint32_t)DBCTab1,2,
		 2,(uint32_t)DBCTab2,3,
		 2,(uint32_t)DBCTab3,4,
};

static   uint32_t RxidTab1[]={(uint32_t)(&ExInfo[0]),(uint32_t)(&ExInfo[1]),(uint32_t)(&ExInfo[2])};
static   uint32_t RxidTab2[]={(uint32_t)(&ExInfo[3]),(uint32_t)(&ExInfo[4]),(uint32_t)(&ExInfo[5]),(uint32_t)(&ExInfo[6])};

CanChInfo INFO[2]=
{
		2,500000,(uint32_t)(RxidTab1),3,
	  3,500000,(uint32_t)(RxidTab2),4
};




