#include"datatype.h"
/*
static XCPDataRecordType xcpSegMent[] = 
{
	"acfrpsettemp_c","air conditioning front passenger set temperature","Grad",V_UINT,1,0,0,0,1879070182,0.5000000,-16.0000000,
	"ASDdc_tEng","antijerk engine temperature","deg C",V_UINT,2,0,0,0,1610619756,0.1000000,-273.1400000,
	"genspvout1_ua","CR665 Vriant B","V",V_UINT,1,0,0,0,1879055385,0.0250000,10.6000000,
	"ibattlp_ua","the current after filter","A",V_UINT,2,0,0,0,1879069256,0.0625000,-2048.0000000,
	"mdverl_nm_c","engine torque loss sended to CAN","Nm",V_UINT,2,0,0,0,1879067410,0.2500000,-200.0000000,
	"miasrl_c","CAN-signal: request ""slow ASR intervention""","%",V_UINT,2,0,0,0,1879067638,0.0030518,-100.0000000,
	"UEGO_CJ135CtrlS1B1_I.iIsqrDes","","A",V_UINT,1,0,0,0,1610625377,0.0000061,0.0003600,
	"vehaxoffs_w_ua","offset of longitudinal acceleration","m/s^2",V_UINT,2,0,0,0,1879067506,0.0271267,-21.5928819,
	"CtT_stWrmUpDiagSecSnsr","state of the state-machine oft the 2-sensor warmup thermostat-diagnosis","",V_UINT,1,0,0,0,1610638848,1.0000000,0,
};


static ExInfoType ExInfo[] = 
{
		0,0x7f1,0,
		1,0x7f0,0,
		2,(uint32_t)xcpSegMent,8,
};

static uint32_t RxidTab[] = {(uint32_t)(&ExInfo[0]),(uint32_t)(&ExInfo[1]),(uint32_t)(&ExInfo[2]),};

CanChInfo INFO[2] = 
{
	2,500000,(uint32_t)RxidTab,3,
	0,0,0,0
};
*/
static XCPDataRecordType xcp10ms[] = 
{
	"abgivctr","Exhaust gas interval counter (DMD) without misfire error","",V_UINT,1,0,0,0,1879070793,1.0000000,0,
	"abmf_msg","injection cut off pattern at fixed position","",V_UINT,1,0,0,0,1879063792,1.0000000,0,
	"abmhdev","Cut off pattern high-pressure injection-valve","",V_UINT,1,0,0,0,1879069897,1.0000000,0,
	"abmhdev_msg","Cut off pattern high-pressure injection-valve","",V_UINT,1,0,0,0,1879063791,1.0000000,0,
	"abmm","Injection Cutoff pattern for Torque Reduction","",V_UINT,1,0,0,0,1879062806,1.0000000,0,
	"abmznd","cut off pattern on ignition circuit error","",V_UINT,1,0,0,0,1879070893,1.0000000,0,
	"abmznd_msg","cut off pattern on ignition circuit error","",V_UINT,1,0,0,0,1879055403,1.0000000,0,
	"abo_msg","value for amount of fuel in oil (no physical unit)","",V_UINT,1,0,0,0,1610620224,1.0000000,0,
	"acblwrspdr_c","air conditioning rear blower speed","",V_UINT,1,0,0,0,1879070702,1.0000000,0,
	"acblwrspd_c","air conditioning front blower speed","",V_UINT,1,0,0,0,1879070184,1.0000000,0,
};

static XCPDataRecordType xcp100ms[] = 
{
	"ACCI_aReq","acceleration request from ACC","m/s^2",V_INT,2,0,0,0,1879055258,0.0010000,0,
	"ACCI_stInActv","status: Application parametr for ACC is inactive (for monitoring)","",V_UINT,1,0,0,0,1879055452,1.0000000,0,
	"ACCI_stReq","Status of acceleration request","",V_UINT,1,0,0,0,1879055451,1.0000000,0,
	"ACCI_trqDes","desired torque from ACC","Nm",V_INT,4,0,0,0,1879054440,0.1000000,0,
	"ACCompModReq","Air Conditioning Compressor Mode Request","",V_UINT,1,0,0,0,1879055450,1.0000000,0,
	"ACComp_trqDesFltAbv_mp","desired torque filtered","Nm",V_INT,2,0,0,0,1879100260,0.1000000,0,
	"ACComp_trqDesFlt_mp","desired torque filtered","Nm",V_INT,2,0,0,0,1879100258,0.1000000,0,
	"ACComp_trqDyn","Dynamic torque compensation for AC","Nm",V_INT,2,0,0,0,1879062810,0.1000000,0,
	"ACComp_trqDynFltRs","T.B.D","Nm",V_INT,2,0,0,0,1610630140,0.1000000,0,
	"ACComp_trqDynFltRsSum","T.B.D","Nm",V_INT,2,0,0,0,1610627664,0.1000000,0,
	"ACComp_trqDyn_mp","dynamic torque","Nm",V_INT,2,0,0,0,1879100256,0.1000000,0,
	"ACComp_trqStat","Static torque compensation for AC","Nm",V_INT,2,0,0,0,1879062808,0.1000000,0,
	"ACComp_trqStat_mp","static torque","Nm",V_INT,2,0,0,0,1879100254,0.1000000,0,
	"AccPed_facCompAcs","Factor ""Overrun Ramp""","",V_INT,2,0,0,0,1610632340,0.0001221,0,
	"AccPed_factDesGearTemp","AccPed_factDesGear_temp","",V_INT,2,0,0,0,1610619784,0.0078125,0,
};
//TRANSPORT_LAYER_INSTANCE "Vehicle CAN (APPL)"

/************************ start of CAN *********************/

    //0x0102                                                /* XCP on CAN version */
    //CAN_ID_MASTER 0x7F0                                   /* CMD/STIM CAN-ID */
                                                          /* master -> slave */
    //CAN_ID_SLAVE 0x7F1                                    /* RES/ERR/EV/SERV/DAQ CAN-ID */
                                                          /* slave -> master */
                                                          /* Bit31= 1: extended identifier */
    //BAUDRATE 500000                                      /* BAUDRATE [Hz] */
    //SAMPLE_POINT 80                                       /* sample point */
                                                          /* [% complete bit time] */
    //SAMPLE_RATE SINGLE                                    /* 1 sample per bit */
    //BTL_CYCLES 20                                         /* BTL_CYCLES */
                                                          /* [slots per bit time] */
    //SJW 4                                                 /* length synchr. segment */
                                                          /* [BTL_CYCLES] */
    //SYNC_EDGE SINGLE                                      /* on falling edge only */
    //MAX_BUS_LOAD  20                                     /* maximum available bus */

static ExInfoType ExInfo[] = 
{
		SLAVER_ID_TYPE,0x7f1,0,//rxid
		MASTER_ID_TYPE,0x7f0,0,//txid
		//DAQ100_ID_TYPE,0x6af,0,//rxdaqid
		//DAQ10_ID_TYPE,0x6ae,0,//rxdaqid
		DAQ100_TAB_TYPE,(uint32_t)xcp100ms,15,
		DAQ10_TAB_TYPE,(uint32_t)xcp10ms,10,
		//CCP_ECUADDR_TYPE,0xAD01,0,//station address
};
//MasterID
//SlaveID
//DAQ100ID
//DAQ10ID
//DAQ10Tab
//DAQ1000TAB
//StationAddr
//MonitorTAB
static uint32_t RxidTab[] = {(uint32_t)(&ExInfo[0]),(uint32_t)(&ExInfo[1]),(uint32_t)(&ExInfo[2]),(uint32_t)(&ExInfo[3])};

CanChInfo INFO[2] = 
{
	2,500000,(uint32_t)RxidTab,4,
	0,0,0,0,
};



