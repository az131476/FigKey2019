#include"datatype.h"

static XCPDataRecordType xcp10ms[] = 
{
	"abak","share factor wall wetting for acceleration enrichment","",V_UINT,1,0,0,0,3489668403,0.0039063,0,
	"abaov","Number accelerations without adaptation adjustment","",V_UINT,1,0,0,0,3489668414,1.0000000,0,
	"abgivctr","Exhaust gas interval counter (DMD) without misfire error","",V_UINT,1,0,0,0,3489668156,1.0000000,0,
	"abmf","injection cut off pattern at fixed position","",V_UINT,1,0,0,0,3489667585,1.0000000,0,
};

static XCPDataRecordType xcp100ms[] = 
{
	"Air_tCACDs","Temperature down stream of charged air cooler","deg C",V_INT,2,0,0,0,3489681474,0.1000000,-273.1400000,
	"genlrespt_ua","generator load response ramp time","s",V_UINT,1,0,0,0,3489668490,0.8474576,0.3983051,
	"genspvout1_ua","CR665 Vriant B","V",V_UINT,1,0,0,0,3489668491,0.0250000,10.6000000,
	"ibatt1_ua","The actual measured current1 with different resolution","A",V_UINT,2,0,0,0,3489665838,0.0078125,-256.0000000,
	"ibatt2_ua","The actual measured current2 with different resolution","A",V_UINT,2,0,0,0,3489665840,0.0625000,-2048.0000000,
	"mdverl_nm_c","engine torque loss sended to CAN","Nm",V_UINT,2,0,0,0,3489667478,0.2500000,-200.0000000,
	"MeanRawIndicatedDriverReqTorq","t.b.d.","",V_UINT,2,0,0,0,3489667480,0.0030518,-100.0000000,
	"mkist_ua","actual engine torque","%",V_UINT,1,0,0,0,3489668500,0.5000000,-25.0000000,
	"psdss_c","Actual manifold pressure (bar)","",V_UINT,2,0,0,0,3489667528,4.0000000,100.0000000,
	"stwhtorque_c","SteeringTorque","Nm",V_UINT,1,0,0,0,3489670344,0.1794000,-22.7838000,
	"thhat","Uncorrected switch off time for lambda sensor heating post cat","s",V_UINT,1,0,0,0,3489668625,0.0100000,-0.0500000,
	"usbeh_w","Voltage (word) lambda sensor loaded with resistance downstream catalyst","V",V_UINT,2,0,0,0,3489663950,0.0048828,-1.0000000,
	"ushk","output voltage oxygen sensor downstream catalyst","V",V_UINT,1,0,0,0,3489668576,0.0052157,-0.2000000,
	"vehaxoffs_c","longitudinal acceleration offset","m/(s^2)",V_UINT,2,0,0,0,3489667100,0.0271267,-21.5928819,
	"vsfrk","Correction of the relative fuel mass by adjusting systems","",V_UINT,1,0,0,0,3489669841,0.0019531,0.7500000,
	"z_flags_um","t.b.d.","",V_UINT,1,0,0,0,3489693986,1.0000000,0,
};
///begin TP_BLOB
      /* CCP version     */ //0x201
      /* Blob version    */ //0x204
      /* CAN msg ID-send */ //0x6AB
      /* CAN msg ID-recv */ //0x6A9
      /* station address */ //0xAD01
      /* byte order      */ //2
static ExInfoType ExInfo[] = 
{
		SLAVER_ID_TYPE,0x6a9,0,//rxid
		MASTER_ID_TYPE,0x6ab,0,//txid
		DAQ100_ID_TYPE,0x6af,0,//rxdaqid
		DAQ10_ID_TYPE,0x6ae,0,//rxdaqid
		DAQ100_TAB_TYPE,(uint32_t)xcp100ms,16,
		DAQ10_TAB_TYPE,(uint32_t)xcp10ms,4,
		CCP_ECUADDR_TYPE,0xAD01,0,//station address
};
static ExInfoType ExInfo1[] =
{
		SLAVER_ID_TYPE,0x6a9,0,//rxid  
		MASTER_ID_TYPE,0x6ab,0,//txid	
		DAQ100_ID_TYPE,0x6af,0,//rxdaqid 
		DAQ10_ID_TYPE,0x6ae,0,//rxdaqid  
		DAQ100_TAB_TYPE,(uint32_t)xcp100ms,16,
		DAQ10_TAB_TYPE,(uint32_t)xcp10ms,4,
		CCP_ECUADDR_TYPE,0xAD01,0,//station address 
};

/*
#define MASTER_ID_TYPE 1
#define SLAVER_ID_TYPE 0
#define DAQ10_ID_TYPE  2
#define DAQ100_ID_TYPE 3
#define DAQ10_TAB_TYPE 4
#define DAQ100_TAB_TYPE 5
#define MORNITOR_TAB_TYPE 6
#define CCP_ECUADDR_TYPE 7
*/

//MasterID
//SlaveID
//DAQ100ID
//DAQ10ID
//DAQ10Tab
//DAQ1000TAB
//StationAddr
//MonitorTAB
//static uint32_t RxidTab[] = {(uint32_t)(&ExInfo[0]),(uint32_t)(&ExInfo[1]),(uint32_t)(&ExInfo[2]),(uint32_t)(&ExInfo[3]),(uint32_t)(&ExInfo[4]),(uint32_t)(&ExInfo[5]),(uint32_t)(&ExInfo[6])};

CanChInfo INFO[2] = 
{
	1,500000,(uint32_t)ExInfo,7,
	1,500000,(uint32_t)ExInfo1,7,
};



