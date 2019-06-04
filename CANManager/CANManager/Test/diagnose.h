#ifndef _DIAGNOSE_H_
#define _DIAGNOSE_H_

/*
 *	ISO 诊断应用层
 */

#include "fattester.h"
#include "sdUser.h"

#define CAN_BUFF_LEN 10

#define LIANDIAN 0X01
#define SANXIN 0x02

typedef enum{
	OFF_LINE = 0,
	ON_LINE
}onlineSts;


typedef struct{
	
	unsigned char dignostypes;	//诊断协议 		0:15031 1:14229 2:KWP2000
	unsigned char commtypes;		//通讯类型 		0:CAN 1:K-Line
	unsigned char frameType;		//帧类型	 		0:标准帧 1:扩展帧
	unsigned char initType;			//初始化方式	0:快速初始化 1:5Buad初始化

	unsigned int tgtAddr;				//ECU标识符		
	unsigned int srcAddr;				//诊断仪标识符
	unsigned int funAddr;				//功能寻址
	unsigned int Baud;					//波特率
	
	unsigned char server_id;		//服务ID
	unsigned char sub_function;	//服务子功能
	
	unsigned char client;				//客户
	
}DiagnoseStr;

typedef struct{
	char num[6];
	char id[8];
	char len[2];
	char data[40];
}StrCanMsg;

extern unsigned char diagOnlineSts;
extern DiagnoseStr diagnose;
extern int showDiagnosLen;
extern char showDiagnosBuf[250][MAX_LINE_LEN];
extern char dtcPath[100];

extern char subfbuf[255];//服务列表
extern char subflen;			//服务个数

//extern StrCanMsg showCanBuf;
extern int frameNum;

//extern void stringSort(char *data, char *num, int len);
unsigned char Num2ASCII(unsigned char c);
int IntToSysChar(int c, char *dec, char system);
int parseCofigFile(char *path, DiagnoseStr *diaInfo);
void rxbuf2dtcbuf(int *dtcbuf, unsigned short *dtclen, unsigned char *rxbuf, unsigned short rxlen, unsigned char isotype);
void rxbuf2String(char *rxbuf, char rxlen, unsigned char system, char *show);
int AnalysisDtc(char *path, int *dtcdata, unsigned short dtclen, int protocol);
void analysis_NegativeResponseCode(int code);
char getServerID(char *sidname);

int CommuntionInit(DiagnoseStr *diaInfo);
int kwpFmt(unsigned char *txbuf, int len);
int uds_Diagnosis_of_online(void);
int uds_service_request(DiagnoseStr *ds, char *idbuf, char *rpNum);

int can_service_sid(int sid, int subid, unsigned char *rxbuf, int *rxlen);
int kwp_service_sid(int sid, int subid, unsigned char *rxbuf, int *rxlen);

void analysis_pid(char *rxbuf, int len);
void analysis_tid(char *rxbuf, int len);
int Query_o2sensor_location(void);
int KwpQueryPidServer(unsigned char sid);

int uds_service_sid01(unsigned char *rxbuf, int *rxlen);
int uds_service_sid02(unsigned char *rxbuf, int *rxlen);
int uds_service_sid03(unsigned char *rxbuf, int *rxlen);
int uds_service_sid04(unsigned char *rxbuf, int *rxlen);
int uds_service_sid05(unsigned char *rxbuf, int *rxlen);
int uds_service_sid06(unsigned char *rxbuf, int *rxlen);
int uds_service_sid07(unsigned char *rxbuf, int *rxlen);
int uds_service_sid08(unsigned char *rxbuf, int *rxlen);
int uds_service_sid09(unsigned char *rxbuf, int *rxlen);

void analysis_sid0102_pid01(char *data);
void analysis_sid0102_pid02(char *data);
void analysis_sid0102_pid03(char *data);
void analysis_sid0102_pid04(char *data);
void analysis_sid0102_pid05(char *data);
void analysis_sid0102_pid06(char *data);
void analysis_sid0102_pid07(char *data);
void analysis_sid0102_pid08(char *data);
void analysis_sid0102_pid09(char *data);
void analysis_sid0102_pid0A(char *data);
void analysis_sid0102_pid0B(char *data);
void analysis_sid0102_pid0C(char *data);
void analysis_sid0102_pid0D(char *data);
void analysis_sid0102_pid0E(char *data);
void analysis_sid0102_pid0F(char *data);

void analysis_sid0102_pid10(char *data);
void analysis_sid0102_pid11(char *data);
void analysis_sid0102_pid12(char *data);
void analysis_sid0102_pid13(char *data);
void analysis_sid0102_pid14(char *data, short len);
void analysis_sid0102_pid15(char *data, short len);
void analysis_sid0102_pid16(char *data, short len);
void analysis_sid0102_pid17(char *data, short len);
void analysis_sid0102_pid18(char *data, short len);
void analysis_sid0102_pid19(char *data, short len);
void analysis_sid0102_pid1A(char *data, short len);
void analysis_sid0102_pid1B(char *data, short len);
void analysis_sid0102_pid1C(char *data);
void analysis_sid0102_pid1D(char *data);
void analysis_sid0102_pid1E(char *data);
void analysis_sid0102_pid1F(char *data);

void analysis_sid0102_pid21(char *data);
void analysis_sid0102_pid22(char *data);
void analysis_sid0102_pid23(char *data);
void analysis_sid0102_pid24(char *data);
void analysis_sid0102_pid25(char *data);
void analysis_sid0102_pid26(char *data);
void analysis_sid0102_pid27(char *data);
void analysis_sid0102_pid28(char *data);
void analysis_sid0102_pid29(char *data);
void analysis_sid0102_pid2A(char *data);
void analysis_sid0102_pid2B(char *data);
void analysis_sid0102_pid2C(char *data);
void analysis_sid0102_pid2D(char *data);
void analysis_sid0102_pid2E(char *data);
void analysis_sid0102_pid2F(char *data);

void analysis_sid0102_pid30(char *data);
void analysis_sid0102_pid31(char *data);
void analysis_sid0102_pid32(char *data);
void analysis_sid0102_pid33(char *data);
void analysis_sid0102_pid34(char *data);
void analysis_sid0102_pid35(char *data);
void analysis_sid0102_pid36(char *data);
void analysis_sid0102_pid37(char *data);
void analysis_sid0102_pid38(char *data);
void analysis_sid0102_pid39(char *data);
void analysis_sid0102_pid3A(char *data);
void analysis_sid0102_pid3B(char *data);
void analysis_sid0102_pid3C(char *data);
void analysis_sid0102_pid3D(char *data);
void analysis_sid0102_pid3E(char *data);
void analysis_sid0102_pid3F(char *data);

void analysis_sid0102_pid41(char *data);
void analysis_sid0102_pid42(char *data);
void analysis_sid0102_pid43(char *data);
void analysis_sid0102_pid44(char *data);
void analysis_sid0102_pid45(char *data);
void analysis_sid0102_pid46(char *data);
void analysis_sid0102_pid47(char *data);
void analysis_sid0102_pid48(char *data);
void analysis_sid0102_pid49(char *data);
void analysis_sid0102_pid4A(char *data);
void analysis_sid0102_pid4B(char *data);
void analysis_sid0102_pid4C(char *data);
void analysis_sid0102_pid4D(char *data);
void analysis_sid0102_pid4E(char *data);
void analysis_sid0102_pid4F(char *data);

void analysis_sid0102_pid50(char *data);
void analysis_sid0102_pid51(char *data);
void analysis_sid0102_pid52(char *data);
void analysis_sid0102_pid53(char *data);
void analysis_sid0102_pid54(char *data);
void analysis_sid0102_pid55(char *data);
void analysis_sid0102_pid56(char *data);
void analysis_sid0102_pid57(char *data);
void analysis_sid0102_pid58(char *data);
void analysis_sid0102_pid59(char *data);
void analysis_sid0102_pid5A(char *data);


void analysis_sid0102_pidFD(char *data);
void analysis_sid0102_pidFE(char *data);
void analysis_sid0102_pidFF(char *data);

void analysis_sid05_tid01(char *data);
void analysis_sid05_tid02(char *data);
void analysis_sid05_tid03(char *data);
void analysis_sid05_tid04(char *data);
void analysis_sid05_tid05(char *data);
void analysis_sid05_tid06(char *data);
void analysis_sid05_tid07(char *data);
void analysis_sid05_tid08(char *data);
void analysis_sid05_tid09(char *data);
void analysis_sid05_tid0A(char *data);

void analysis_sid09_tid01(char *data);
void analysis_sid09_tid02(char *data);
void analysis_sid09_tid03(char *data);
void analysis_sid09_tid04(char *data);
void analysis_sid09_tid05(char *data);
void analysis_sid09_tid06(char *data);
void analysis_sid09_tid07(char *data);
void analysis_sid09_tid08(char *data);
void analysis_sid09_tid09(char *data);
void analysis_sid09_tid0A(char *data);

#endif
