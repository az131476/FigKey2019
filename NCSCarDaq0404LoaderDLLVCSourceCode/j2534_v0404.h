/*
 * This is the Drew Technologies Mongoose header file.
 * Each Mongoose product supports the full J2534-1 v0404 API
 * for the protocols available on the device.
 * Each Mongoose device does not support all J2534-1 protocols.
 * Some extensions are supported. The extensions are marked
 * as follows:
 *    "-2" = Feature from the J2534-2 Specification
 *    "v2" = Backwards-compatiable feature from J2534-1 v0202 API
 *    "DT" = DrewTech-specific feature, not yet standardized
 *
 * This file is free to use as long as this header stays intact.
 * Author: Mark Wine
 */

#ifndef __J2534_H
#define __J2534_H


/**************************/
/* ProtocolID definitions */
/**************************/
#define J1850VPW				1 // Mongoose VPW/CAN, SCI/CAN and GM
#define J1850PWM				2 // Mongoose PWM/CAN and PWMF
#define ISO9141					3 // Mongoose ISO/CAN and MFC
#define ISO14230				4 // Mongoose ISO/CAN and MFC
#define CAN				        5 // Mongoose all
#define ISO15765				6 // Mongoose All
#define SCI_A_ENGINE			7 // Mongoose SCI/CAN
#define SCI_A_TRANS				8 // Mongoose SCI/CAN
#define SCI_B_ENGINE			9 // Mongoose SCI/CAN
#define SCI_B_TRANS				10 // Mongoose SCI/CAN

// J2534-2 Pin Switched ProtocolIDs
#define J1850VPW_PS			/*-2*/			0x8000 // Not supported
#define J1850PWM_PS			/*-2*/			0x8001 // Not supported
#define ISO9141_PS			/*-2*/			0x8002 // Mongoose JLR
#define ISO14230_PS			/*-2*/			0x8003 // Mongoose JLR
#define CAN_PS				/*-2*/			0x8004 // Mongoose JLR
#define ISO15765_PS			/*-2*/			0x8005 // Not supported
#define J2610_PS			/*-2*/			0x8006 // Not supported
#define SW_ISO15765_PS		/*-2*/			0x8007 // Mongoose GM
#define SW_CAN_PS			/*-2*/			0x8008 // Mongoose GM
#define GM_UART_PS			/*-2*/			0x8009 // Not supported (YET)
#define UART_ECHO_BYTE_PS	/*-2*/			0x800A // Mongoose ISO/CAN and MFC
#define HONDA_DIAGH_PS  	/*-2*/			0x800B // Not supported
#define J1939_PS  	        /*-2*/			0x800C // Not supported
#define J1708_PS  	        /*-2*/			0x800D // TVIT
#define TP2_0_PS  	        /*-2*/			0x800E // Not supported
#define FT_CAN_PS  	        /*-2*/			0x800F // Not supported
#define FT_ISO15765_PS  	/*-2*/			0x8010 // Not supported

#define ANALOG_IN_1			/*-2*/			0xC000
#define ANALOG_IN_2			/*-2*/			0xC001 // Supported on AVIT only
#define ANALOG_IN_32		/*-2*/			0xC01F // Not supported

#define CEC1_PS_DT			/*DT*/			0x10000002 // Supported on TVIT

/*************/
/* IOCTL IDs */
/*************/
#define GET_CONFIG					0x01
#define SET_CONFIG					0x02
#define READ_VBATT				    0x03
#define FIVE_BAUD_INIT				0x04
#define FAST_INIT					0x05
// unused							0x06
#define CLEAR_TX_BUFFER				0x07
#define CLEAR_RX_BUFFER				0x08
#define CLEAR_PERIODIC_MSGS			0x09
#define CLEAR_MSG_FILTERS			0x0A
#define CLEAR_FUNCT_MSG_LOOKUP_TABLE		0x0B
#define ADD_TO_FUNCT_MSG_LOOKUP_TABLE		0x0C
#define DELETE_FROM_FUNCT_MSG_LOOKUP_TABLE	0x0D
#define READ_PROG_VOLTAGE					0x0E
// J2534-2 SWCAN
#define SW_CAN_HS			/*-2*/			0x8000 // Mongoose GM
#define SW_CAN_NS			/*-2*/			0x8001 // Mongoose GM
// J2534-2 Discovery Mechanism
#define GET_DEVICE_INFO                 	0x800C
#define GET_PROTOCOL_INFO               	0x800D

// Drew Tech defined IOCTLs
#define READ_TIMESTAMP		/*DT*/			0x10100 // CDP, AVIT
#define DT_READ_CABLE_ID    /*DT*/		    0x10102 // CDP CDM Only input = NULL, output = ptr long
#define DT_READ_VPPRAW      /*DT*/		    0x10103 // CDM only input = NULL, output = ptr long (voltage in mv)
#define DT_READ_V_VEHICLE   /*DT*/	    	0x10104 // CDM only input = NULL, output = ptr long (voltage in mv)
#define DT_READ_BROWNOUT    /*DT*/  		0x10105 // CDM only input = NULL, output = ptr long (brown out state)
#define DT_COMMIT_CHANGES		/*DT*/		0x30000 // CDP input = NULL, Output = NULL
#define GET_DEVICE_BYTE_ARRAY	/*DT*/		0x30001 // CDP input = SBYTE_ARRAY, output = define
#define SET_DEVICE_BYTE_ARRAY	/*DT*/		0x30002 // CDP input = SBYTE_ARRAY, output = define 
#define DT_IOCTL_VVSTATS	/*DT*/			0x20000000
#define DT_READ_DIO			    /*DT*/		0x20000001 // AVIT only input = NULL, output = ptr long
#define DT_WRITE_DIO			/*DT*/		0x20000002 // AVIT only input = ptr long, output = NULL
#define DT_ANALOG_IGNORE_CAL	/*DT*/		0x20000003 // AVIT only


/*******************************/
/* Configuration Parameter IDs */
/*******************************/
#define DATA_RATE					0x01
// unused							0x02
#define LOOPBACK					0x03
#define NODE_ADDRESS				0x04
#define NETWORK_LINE				0x05 // Not supported
#define P1_MIN						0x06 // Don't use
#define P1_MAX						0x07
#define P2_MIN						0x08 // Don't use
#define P2_MAX						0x09 // Don't use
#define P3_MIN						0x0A
#define P3_MAX						0x0B // Don't use
#define P4_MIN						0x0C
#define P4_MAX						0x0D // Don't use
// See W0 = 0x19
#define W1							0x0E
#define W2							0x0F
#define W3							0x10
#define W4							0x11
#define W5							0x12
#define TIDLE						0x13
#define TINIL						0x14
#define TWUP						0x15
#define PARITY						0x16
#define BIT_SAMPLE_POINT			0x17
#define SYNC_JUMP_WIDTH				0x18
#define W0						    0x19
#define T1_MAX						0x1A
#define T2_MAX						0x1B
// See T3_MAX						0x24
#define T4_MAX						0x1C
#define T5_MAX						0x1D
#define ISO15765_BS					0x1E
#define ISO15765_STMIN				0x1F
#define DATA_BITS					0x20
#define FIVE_BAUD_MOD				0x21
#define BS_TX						0x22
#define STMIN_TX					0x23
#define T3_MAX						0x24
#define ISO15765_WFT_MAX			0x25
// Future -1 values (not approved yet)
#define N_BR_MIN			        0x2A
#define ISO15765_PAD_VALUE          0x2B
#define N_AS_MAX			        0x2C
#define N_AR_MAX			        0x2D
#define N_BS_MAX			        0x2E
#define N_CR_MAX			        0x2F
#define N_CS_MIN			        0x30

#define DT_PULLUP_VALUE          /*DT*/ 0x10008 // Mongoose JLR  (Value requested by customer)

#define DT_ISO15765_SIMULTANEOUS  /*DT*/    0x10000000 // Not supported
#define DT_ISO15765_PAD_BYTE      /*DT*/    0x10000001
#define DT_FILTER_FREQ			  /*DT*/	0x10000002 // Not supported
#define DT_PARAM_RX_BUFFER_SIZE	  /*DT*/	0x10000003 // Not supported
#define DT_PARAM_TX_BUFFER_SIZE	  /*DT*/	0x10000004 // Not supported
#define DT_J1939_CTS_BS     	  /*DT*/	0x10000005 // Set a block size for CTS message, 0 = no block size

// J2534-2
#define CAN_MIXED_FORMAT		/*-2*/		0x8000
#define J1962_PINS			    /*-2*/		0x8001
#define SW_CAN_HS_DATA_RATE		/*-2*/		0x8010 // Mongoose GM
#define SW_CAN_SPEEDCHANGE_ENABLE	/*-2*/	0x8011 // Mongoose GM
#define SW_CAN_RES_SWITCH		/*-2*/		0x8012 // Mongoose GM
#define ACTIVE_CHANNELS			/*-2*/		0x8020 // Not supported
#define SAMPLE_RATE			    /*-2*/		0x8021 // Not supported
#define SAMPLES_PER_READING		/*-2*/		0x8022 // Not supported
#define READINGS_PER_MSG		/*-2*/		0x8023 // Not supported
#define AVERAGING_METHOD		/*-2*/		0x8024 // Not supported
#define SAMPLE_RESOLUTION		/*-2*/		0x8025 // Not supported
#define INPUT_RANGE_LOW			/*-2*/		0x8026 // Not supported
#define INPUT_RANGE_HIGH		/*-2*/		0x8027 // Not supported
// old DT analogs
#define ADC_READINGS_PER_SECOND		/*DT*/		0x10000 // Not supported
#define ADC_READINGS_PER_SAMPLE		/*DT*/		0x20000 // Not supported

// UART Echo Byte protocol parameters
#define UEB_T0_MIN             	/*-2*/			0x8028
#define UEB_T1_MAX             	/*-2*/			0x8029
#define UEB_T2_MAX             	/*-2*/			0x802A
#define UEB_T3_MAX             	/*-2*/			0x802B
#define UEB_T4_MIN             	/*-2*/			0x802C
#define UEB_T5_MAX             	/*-2*/			0x802D
#define UEB_T6_MAX             	/*-2*/			0x802E
#define UEB_T7_MIN             	/*-2*/			0x802F
#define UEB_T7_MAX             	/*-2*/			0x8030
#define UEB_T9_MIN             	/*-2*/			0x8031

// Pin selection
#define J1939_PINS             	/*-2*/			0x803D
#define J1708_PINS             	/*-2*/			0x803E

// J2534-2 GET_DEVICE_INFO defines
#define SERIAL_NUMBER         	/*-2*/		0x0001
#define J1850PWM_SUPPORTED     	/*-2*/  	0x0002
#define J1850VPW_SUPPORTED     	/*-2*/  	0x0003
#define ISO9141_SUPPORTED      	/*-2*/  	0x0004
#define ISO14230_SUPPORTED     	/*-2*/  	0x0005
#define CAN_SUPPORTED  	        /*-2*/  	0x0006
#define ISO15765_SUPPORTED     	/*-2*/  	0x0007
#define SCI_A_ENGINE_SUPPORTED 	/*-2*/  	0x0008
#define SCI_A_TRANS_SUPPORTED  	/*-2*/  	0x0009
#define SCI_B_ENGINE_SUPPORTED 	/*-2*/  	0x000A
#define SCI_B_TRANS_SUPPORTED  	/*-2*/  	0x000B
#define SW_ISO15765_SUPPORTED  	/*-2*/  	0x000C
#define SW_CAN_SUPPORTED      	/*-2*/  	0x000D
#define GM_UART_SUPPORTED       /*-2*/      0x000E
#define UART_ECHO_BYTE_SUPPORTED   	/*-2*/  	0x000F
#define HONDA_DIAGH_SUPPORTED   /*-2*/      0x0010
#define J1939_SUPPORTED      	/*-2*/  	0x0011
#define J1708_SUPPORTED      	/*-2*/  	0x0012
#define TP2_0_SUPPORTED      	/*-2*/  	0x0013
#define J2610_SUPPORTED         /*-2*/      0x0014
#define ANALOG_IN_SUPPORTED     /*-2*/  	0x0015
#define MAX_NON_VOLATILE_STORAGE   	/*-2*/  	0x0016
#define SHORT_TO_GND_J1962      /*-2*/  	0x0017
#define PGM_VOLTAGE_J1962      	/*-2*/  	0x0018
#define J1850PWM_PS_J1962		/*-2*/  	0x0019
#define J1850VPW_PS_J1962		/*-2*/  	0x001A
#define ISO9141_PS_K_LINE_J1962 /*-2*/  	0x001B
#define ISO14230_PS_K_LINE_J1962 /*-2*/  	0x001C
#define ISO9141_PS_L_LINE_J1962 /*-2*/  	0x001D
#define ISO14230_PS_L_LINE_J1962 /*-2*/  	0x001E
#define CAN_PS_J1962            /*-2*/  	0x001F
#define ISO15765_PS_J1962      	/*-2*/  	0x0020
#define SW_CAN_PS_J1962      	/*-2*/  	0x0021
#define SW_ISO15765_PS_J1962    /*-2*/  	0x0022
#define GM_UART_PS_J1962        /*-2*/      0x0023
#define UART_ECHO_BYTE_PS_J1962 /*-2*/  	0x0024
#define HONDA_DIAGH_PS_J1962    /*-2*/      0x0025
#define J1939_PS_J1962      	/*-2*/  	0x0026
#define J1708_PS_J1962      	/*-2*/  	0x0027
#define TP2_0_PS_J1962      	/*-2*/  	0x0028
#define J2610_PS_J1962          /*-2*/      0x0029
#define J1939_PS_J1939      	/*-2*/  	0x002A
#define J1708_PS_J1939      	/*-2*/  	0x002B
#define ISO14230_PS_L_LINE_J1939 /*-2*/     0x002F
#define J1708_PS_J1708      	/*-2*/  	0x0030
#define J1850PWM_SIMULTANEOUS  	/*-2*/  	0x0035
#define J1850VPW_SIMULTANEOUS  	/*-2*/  	0x0036
#define ISO9141_SIMULTANEOUS  	/*-2*/  	0x0037
#define ISO14230_SIMULTANEOUS  	/*-2*/  	0x0038
#define CAN_SIMULTANEOUS  	    /*-2*/  	0x0039
#define ISO15765_SIMULTANEOUS  	/*-2*/  	0x003A
#define SCI_A_ENGINE_SIMULTANEOUS  	/*-2*/  0x003B
#define SCI_A_TRANS_SIMULTANEOUS  	/*-2*/  0x003C
#define SCI_B_ENGINE_SIMULTANEOUS  	/*-2*/  0x003D
#define SCI_B_TRANS_SIMULTANEOUS  	/*-2*/  0x003E
#define SW_ISO15765_SIMULTANEOUS  	/*-2*/  0x003F
#define SW_CAN_SIMULTANEOUS  	    /*-2*/  0x0040
#define GM_UART_SIMULTANEOUS        /*-2*/  0x0041
#define UART_ECHO_BYTE_SIMULTANEOUS /*-2*/  0x0042
#define HONDA_DIAGH_SIMULTANEOUS    /*-2*/  0x0043
#define J1939_SIMULTANEOUS      /*-2*/      0x0044
#define J1708_SIMULTANEOUS      /*-2*/      0x0045
#define TP2_0_SIMULTANEOUS      /*-2*/      0x0046
#define J2610_SIMULTANEOUS      /*-2*/      0x0047
#define ANALOG_IN_SIMULTANEOUS  /*-2*/      0x0048
#define PART_NUMBER             /*-2*/      0x0049

// J2534-2 GET_PROTOCOL_INFO defines
#define MAX_RX_BUFFER_SIZE      /*-2*/	    0x0001
#define MAX_PASS_FILTER         /*-2*/	    0x0002
#define MAX_BLOCK_FILTER        /*-2*/	    0x0003
#define MAX_FILTER_MSG_LENGTH   /*-2*/	    0x0004
#define MAX_PERIODIC_MSGS       /*-2*/	    0x0005
#define MAX_PERIODIC_MSG_LENGTH /*-2*/	    0x0006
#define DESIRED_DATA_RATE       /*-2*/	    0x0007
#define MAX_REPEAT_MESSAGING    /*-2*/	    0x0008
#define MAX_REPEAT_MESSAGING_LENGTH     /*-2*/	    0x0009
#define NETWORK_LINE_SUPPORTED  /*-2*/	    0x000A
#define MAX_FUNCT_MSG_LOOKUP    /*-2*/	    0x000B
#define PARITY_SUPPORTED        /*-2*/	    0x000C
#define DATA_BITS_SUPPORTED     /*-2*/	    0x000D
#define FIVE_BAUD_MOD_SUPPORTED /*-2*/	    0x000E
#define L_LINE_SUPPORTED        /*-2*/	    0x000F
#define CAN_11_29_IDS_SUPPORTED /*-2*/	    0x0010
#define CAN_MIXED_FORMAT_SUPPORTED     /*-2*/	    0x0011
#define MAX_FLOW_CONTROL_FILTER /*-2*/	    0x0012
#define MAX_ISO15765_WFT_MAX    /*-2*/	    0x0013
#define RESOURCE_GROUP          /*-2*/	    0x001A
#define TIMESTAMP_RESOLUTION    /*-2*/	    0x001B

/*************/
/* Error IDs */
/*************/
#define STATUS_NOERROR						0x00
#define ERR_NOT_SUPPORTED					0x01
#define ERR_INVALID_CHANNEL_ID				0x02
#define ERR_INVALID_PROTOCOL_ID				0x03
#define ERR_NULL_PARAMETER					0x04
#define ERR_INVALID_IOCTL_VALUE				0x05
#define ERR_INVALID_FLAGS					0x06
#define ERR_FAILED						    0x07
#define ERR_DEVICE_NOT_CONNECTED			0x08
#define ERR_TIMEOUT						    0x09
#define ERR_INVALID_MSG						0x0A
#define ERR_INVALID_TIME_INTERVAL			0x0B
#define ERR_EXCEEDED_LIMIT					0x0C
#define ERR_INVALID_MSG_ID					0x0D
#define ERR_DEVICE_IN_USE					0x0E
#define ERR_INVALID_IOCTL_ID				0x0F
#define ERR_BUFFER_EMPTY					0x10
#define ERR_BUFFER_FULL						0x11
#define ERR_BUFFER_OVERFLOW					0x12
#define ERR_PIN_INVALID						0x13
#define ERR_CHANNEL_IN_USE					0x14
#define ERR_MSG_PROTOCOL_ID					0x15
#define ERR_INVALID_FILTER_ID				0x16
#define ERR_NO_FLOW_CONTROL					0x17
#define ERR_NOT_UNIQUE						0x18
#define ERR_INVALID_BAUDRATE				0x19
#define ERR_INVALID_DEVICE_ID				0x1A

#define ERR_NULLPARAMETER		/*v2*/		ERR_NULL_PARAMETER


/*****************************/
/* Miscellaneous definitions */
/*****************************/
#define SHORT_TO_GROUND					0xFFFFFFFE
#define VOLTAGE_OFF						0xFFFFFFFF

#define NO_PARITY						0
#define ODD_PARITY						1
#define EVEN_PARITY						2

//SWCAN
#define DISBLE_SPDCHANGE		/*-2*/		0 // Mongoose GM
#define ENABLE_SPDCHANGE		/*-2*/		1 // Mongoose GM
#define DISCONNECT_RESISTOR		/*-2*/		0 // Mongoose GM
#define CONNECT_RESISTOR		/*-2*/		1 // Mongoose GM
#define AUTO_RESISTOR			/*-2*/		2 // Mongoose GM

//Mixed Mode
#define CAN_MIXED_FORMAT_OFF		/*-2*/	0
#define CAN_MIXED_FORMAT_ON		    /*-2*/	1
#define CAN_MIXED_FORMAT_ALL_FRAMES	/*-2*/	2 // Not supported

// -2 Discovery
#define NOT_SUPPORTED       0
#define SUPPORTED           1

/*******************************/
/* PassThruConnect definitions */
/*******************************/
#define CAN_29BIT_ID						0x00000100
#define ISO9141_NO_CHECKSUM					0x00000200
#define NO_CHECKSUM				/*-2*/      0x00000200
#define CAN_ID_BOTH						    0x00000800
#define ISO9141_K_LINE_ONLY					0x00001000

#define DT_ISO9141_LISTEN_L_LINE    /*DT*/  0x08000000  // DT - use L Line for COMM
#define SNIFF_MODE			        /*DT*/ 	0x10000000 //DT - J1850PWM
#define LISTEN_ONLY_DT      	    /*DT*/	0x10000000 //DT - listen only mode CAN
#define ISO9141_FORD_HEADER		    /*DT*/ 	0x20000000 //DT
#define ISO9141_NO_CHECKSUM_DT		/*DT*/	0x40000000 //compat with CarDAQ2534
//#define CONNECT_ETHERNET_ONLY		/*DT*/	0x80000000 // Not supported


/************************/
/* RxStatus definitions */
/************************/
#define TX_MSG_TYPE						0x00000001
#define START_OF_MESSAGE				0x00000002
#define ISO15765_FIRST_FRAME	/*v2*/	0x00000002 //compat from v0202
#define ISO15765_EXT_ADDR		/*DT*/	0x00000080 // Accidentally refered to in spec
#define RX_BREAK						0x00000004
#define TX_DONE							0x00000008 // old name
#define TX_INDICATION					0x00000008
#define ISO15765_PADDING_ERROR			0x00000010
#define ISO15765_ADDR_TYPE				0x00000080
//	CAN_29BIT_ID						0x00000100  defined above
#define	SW_CAN_NS_RX			/*-2*/	0x00040000 // Mongoose GM
#define	SW_CAN_HS_RX			/*-2*/	0x00020000 // Mongoose GM
#define	SW_CAN_HV_RX			/*-2*/	0x00010000 // Mongoose GM
#define DT_ISO9141_L_LINE		/*DT*/	0x04000000


/***********************/
/* TxFlags definitions */
/***********************/
#define ISO15765_FRAME_PAD			0x00000040
//      ISO15765_ADDR_TYPE			0x00000080  defined above
//	CAN_29BIT_ID				    0x00000100  defined above
#define WAIT_P3_MIN_ONLY			0x00000200
#define SW_CAN_HV_TX		/*-2*/	0x00000400
#define SCI_MODE			        0x00400000
#define SCI_TX_VOLTAGE				0x00800000
//#define DT_ISO9141_L_LINE	/*DT*/	0x04000000 //defined above


/**********************/
/* Filter definitions */
/**********************/
#define PASS_FILTER						0x00000001
#define BLOCK_FILTER					0x00000002
#define FLOW_CONTROL_FILTER				0x00000003
#define PASS_FILTER_WITH_TRIGGER	/*DT*/	0x10000005 //DT Not Supported
#define BLOCK_FILTER_WITH_TRIGGER	/*DT*/	0x10000006 //DT Not Supported


/*********************/
/* Message Structure */
/*********************/
typedef struct
{
	unsigned long ProtocolID;
	unsigned long RxStatus;
	unsigned long TxFlags;
	unsigned long Timestamp;
	unsigned long DataSize;
	unsigned long ExtraDataIndex;
	unsigned char Data[4128];
} PASSTHRU_MSG;


/********************/
/* IOCTL Structures */
/********************/
typedef struct
{
	unsigned long Parameter;
	unsigned long Value;
} SCONFIG;

typedef struct
{
	unsigned long NumOfParams;
	SCONFIG *ConfigPtr;
} SCONFIG_LIST;

typedef struct
{
	unsigned long NumOfBytes;
	unsigned char *BytePtr;
} SBYTE_ARRAY;

typedef struct
{
	unsigned long Parameter;
	unsigned long Value;
    unsigned long Supported;
} SPARAM;

typedef struct
{
	unsigned long NumOfParams;
	SPARAM *ParamPtr;
} SPARAM_LIST;


/************************/
/* Function Definitions */
/************************/
#ifdef _WIN32
//#include <windows.h>
#include <afx.h>
#include <string>
#include "j2534_boilerplate.h"
#ifndef BUILDING_DLL
// J2534 Windows Interface API defines
typedef long (CALLBACK* PTOPEN)(void *, unsigned long *);
typedef long (CALLBACK* PTCLOSE)(unsigned long);
typedef long (CALLBACK* PTCONNECT)(unsigned long, unsigned long, unsigned long, unsigned long, unsigned long *);
typedef long (CALLBACK* PTDISCONNECT)(unsigned long);
typedef long (CALLBACK* PTREADMSGS)(unsigned long, void *, unsigned long *, unsigned long);
typedef long (CALLBACK* PTWRITEMSGS)(unsigned long, void *, unsigned long *, unsigned long);
typedef long (CALLBACK* PTSTARTPERIODICMSG)(unsigned long, void *, unsigned long *, unsigned long);
typedef long (CALLBACK* PTSTOPPERIODICMSG)(unsigned long, unsigned long);
typedef long (CALLBACK* PTSTARTMSGFILTER)(unsigned long, unsigned long, void *, void *, void *, unsigned long *);
typedef long (CALLBACK* PTSTOPMSGFILTER)(unsigned long, unsigned long);
typedef long (CALLBACK* PTSETPROGRAMMINGVOLTAGE)(unsigned long, unsigned long, unsigned long);
typedef long (CALLBACK* PTREADVERSION)(unsigned long, char *, char *, char *);
typedef long (CALLBACK* PTGETLASTERROR)(char *);
typedef long (CALLBACK* PTIOCTL)(unsigned long, unsigned long, void *, void *);
// Drew Tech specific function calls
typedef long (CALLBACK* PTLOADFIRMWARE)(void);
typedef long (CALLBACK* PTRECOVERFIRMWARE)(void);
typedef long (CALLBACK* PTREADIPSETUP)(unsigned long DeviceID, char *host_name, char *ip_addr, char *subnet_mask,
                      char *gateway, char *dhcp_addr);
typedef long (CALLBACK* PTWRITEIPSETUP)(unsigned long DeviceID, char *host_name, char *ip_addr, char *subnet_mask,
                      char *gateway, char *dhcp_addr);
typedef long (CALLBACK* PTREADPCSETUP)(char *host_name, char *ip_addr);
typedef long (CALLBACK* PTGETPOINTER)(long vb_pointer);
typedef long (CALLBACK* PTGETNEXTCARDAQ)(char **name, unsigned long *version, char **addr);
/*
extern PTOPEN PassThruOpen;
extern PTCLOSE PassThruClose;
extern PTCONNECT PassThruConnect;
extern PTDISCONNECT PassThruDisconnect;
extern PTREADMSGS PassThruReadMsgs;
extern PTWRITEMSGS PassThruWriteMsgs;
extern PTSTARTPERIODICMSG PassThruStartPeriodicMsg;
extern PTSTOPPERIODICMSG PassThruStopPeriodicMsg;
extern PTSTARTMSGFILTER PassThruStartMsgFilter;
extern PTSTOPMSGFILTER PassThruStopMsgFilter;
extern PTSETPROGRAMMINGVOLTAGE PassThruSetProgrammingVoltage;
extern PTREADVERSION PassThruReadVersion;
extern PTGETLASTERROR PassThruGetLastError;
extern PTIOCTL PassThruIoctl;
extern PTLOADFIRMWARE PassThruLoadFirmware;
extern PTRECOVERFIRMWARE PassThruRecoverFirmware;
extern PTREADIPSETUP PassThruReadIPSetup;
extern PTWRITEIPSETUP PassThruWriteIPSetup;
extern PTREADPCSETUP PassThruReadPCSetup;
extern PTGETPOINTER PassThruGetPointer;
extern PTGETNEXTCARDAQ PassThruGetNextCarDAQ;
*/
#endif // Not Building the DLL

#else
#define JTYPE extern long /* Linux */

JTYPE PassThruOpen(void *pName, unsigned long *pDeviceID);
JTYPE PassThruClose(unsigned long DeviceID);
JTYPE PassThruConnect(unsigned long DeviceID, unsigned long ProtocolID, unsigned long Flags, unsigned long Baudrate, unsigned long *pChannelID);
JTYPE PassThruDisconnect(unsigned long ChannelID);
JTYPE PassThruReadMsgs(unsigned long ChannelID, PASSTHRU_MSG *pMsg, unsigned long *pNumMsgs, unsigned long Timeout);
JTYPE PassThruWriteMsgs(unsigned long ChannelID, PASSTHRU_MSG *pMsg, unsigned long *pNumMsgs, unsigned long Timeout);
JTYPE PassThruStartPeriodicMsg(unsigned long ChannelID, PASSTHRU_MSG *pMsg,
                      unsigned long *pMsgID, unsigned long TimeInterval);
JTYPE PassThruStopPeriodicMsg(unsigned long ChannelID, unsigned long MsgID);
JTYPE PassThruStartMsgFilter(unsigned long ChannelID,
                      unsigned long FilterType, PASSTHRU_MSG *pMaskMsg, PASSTHRU_MSG *pPatternMsg,
					  PASSTHRU_MSG *pFlowControlMsg, unsigned long *pMsgID);
JTYPE PassThruStopMsgFilter(unsigned long ChannelID, unsigned long MsgID);
JTYPE PassThruSetProgrammingVoltage(unsigned long DeviceID, unsigned long Pin, unsigned long Voltage);
JTYPE PassThruReadVersion(unsigned long DeviceID, char *pFirmwareVersion, char *pDllVersion, char *pApiVersion);
JTYPE PassThruGetLastError(char *pErrorDescription);
JTYPE PassThruIoctl(unsigned long ChannelID, unsigned long IoctlID,
                      void *pInput, void *pOutput);
// DrewTech Only
JTYPE PassThruLoadFirmware(void);
JTYPE PassThruRecoverFirmware(void);
JTYPE PassThruReadIPSetup(unsigned long DeviceID, char *host_name, char *ip_addr, char *subnet_mask, char *gateway, char *dhcp_addr);
JTYPE PassThruWriteIPSetup(unsigned long DeviceID, char *host_name, char *ip_addr, char *subnet_mask, char *gateway, char *dhcp_addr);
//JTYPE PassThruReadPCSetup(char *host_name, char *ip_addr);
//JTYPE PassThruGetPointer(long vb_pointer);
JTYPE PassThruGetNextCarDAQ(char **name, unsigned long *version, char **addr);

#endif


#endif /* __J2534_H */

