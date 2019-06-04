
#include "diagnose.h"
#include "SD.h"
#include "include.h"
#include "can1.h"
#include "CanTp.h"
#include "KWP2000.h"
#include "fattester.h"

DiagnoseStr diagnose;
int showDiagnosLen;
char showDiagnosBuf[250][MAX_LINE_LEN];
char dtcPath[100] = {0};	//DTC文件表路径
char subfbuf[255];				//服务列表
char subflen;							//服务个数
char sendFailedCnt = 0;

char o2sensorDefine = 0;					//区别诊断 0=PID13 - 1=PID1D
char o2sensorLocationBuf[4] = {0};			//氧传感器位置
char o2sensorLocation = 0;				//氧传感器位置
char o2sensorNumber = 0;					//氧传感器数量

static int 	MessageCount1 = 0;
static int 	MessageCount3 = 0;
static int 	MessageCount5 = 0;
static int 	MessageCount7 = 0;
static int 	MessageCount9 = 0;

extern int KwpRequest(char *tx_buf, int tx_len, char *rx_buf, int *rx_len, void *param);

int frameNum = 0;

//ASCII字符 转 Int数据
unsigned char HexCharToInt(unsigned char c)
{
	if (c >= 97 && c <= 102)
		return (c - 97) + 10;
	else if(c >= 65 && c <= 70)
		return (c - 65) + 10;
	else if (c >= 48 && c <= 57)
		return (c - 48);

	return 0xFF;//非字符数据
}

//0xXX ASCII字符 转 0xXX数据  转换限制：转换数据 0x开始  '/r'结尾
int CharToHexInt(int *dtc, char *str)
{
	int i = 0;
	int temp = 0;
	do
	{
		if(0 == strncmp(str+i, "0x", 2))
		{
			i+= 2;
			break;
		}
		else
		{
			i++;
		}
	}while(1);

	while(str[i] != '\r')
	{
		temp <<= 4;										//*16
		temp |= HexCharToInt(str[i]);	//+
		i++;
	}
	*dtc = temp;
	return i;
}

//ASCII字符转CHAR型数据
int StringToNum(char *str, char system)
{
	int i = 0;
	int temp = 0;

	temp |= HexCharToInt(str[i])*system;
	i++;
	temp |= HexCharToInt(str[i]);

	if(temp > 0xFF)
		return 0xFF;
	return temp;
}

//读取配置文件 并解析配置文件
int parseCofigFile(char *path, DiagnoseStr *diaInfo)
{
	//读文件
	char tempbuf[128];
	uint8_t index = 0;
	uint8_t num = 0;

	num = SD_Read(path, tempbuf, index, 128);		//读取数据
	if(num == 0)
		return 1;

	if(0 == strncmp(&tempbuf[index], "dignosPrtcl: ", 13))
	{
		index += CharToHexInt((int *)&diaInfo->dignostypes,&tempbuf[index]);
		index+=2;
	}
	else
		return 1;

	if(0 == strncmp(&tempbuf[index], "commType: ", 10))
	{
		index += CharToHexInt((int *)&diaInfo->commtypes,&tempbuf[index]);
		index+=2;
	}
	else
		return 1;

	if(0 == strncmp(&tempbuf[index], "frameType: ", 11))
	{
		index += CharToHexInt((int *)&diaInfo->frameType,&tempbuf[index]);
		index += 2;
	}
	else
		return 1;

	if(0 == strncmp(&tempbuf[index], "tgtAddr: ", 9))
	{
		index += CharToHexInt((int *)&diaInfo->tgtAddr,&tempbuf[index]);
		index+=2;
	}
	else
		return 1;

	if(0 == strncmp(&tempbuf[index], "srcAddr: ", 9))
	{
		index += CharToHexInt((int *)&diaInfo->srcAddr,&tempbuf[index]);
		index+=2;
	}
	else
		return 1;

	if(0 == strncmp(&tempbuf[index], "funAddr: ", 9))
	{
		index += CharToHexInt((int *)&diaInfo->funAddr,&tempbuf[index]);
		index += 2;
	}
	else
		return 1;

	if(0 == strncmp(&tempbuf[index], "baud: ", 6))
	{
		index += CharToHexInt((int *)&diaInfo->Baud,&tempbuf[index]);
		index += 2;
	}
	else
		return 1;

	if(0 == strncmp(&tempbuf[index], "client: ", 8))
	{
		index += CharToHexInt((int *)&diaInfo->client,&tempbuf[index]);
		index += 2;
	}
	else
		return 1;
	
	return 0;
}

//获取服务ID号---获取字符串序号
char getServerID(char *sidname)//ID：字符串
{
	char idbuf[3];

	memcpy(idbuf, sidname, 2);
	return StringToNum(idbuf,16);
}


//char型 转字符串 大写
unsigned char Num2ASCII(unsigned char c)
{
	if (c > 9)
		return (c + 87);//87 小写  //55大写
	else
		return (c + 0x30);
}

//int数据 转 字符串显示 可显示进制区别 十六进制仅能显示大写
int IntToSysChar(int c, char *dec, char system)
{
	int temp = system;
	int bits = 1;
	int i;
	do{
		if(c >= temp)
		{
			bits++;
			temp *= system;
		}
	}while(c >= temp);

	for(i = 0; i < bits; i++)
	{
		temp /= system;
		dec[i] = Num2ASCII((c/temp)); //得到最高位
		c %= temp;
	}

	return bits;
}

//转成特殊字符串 首字符串+P字符
int IntToChar(int dtc, char *str, int isotype)
{
	int ret;
	int i;
	int temp;

	str[0] = 'P';
	ret = 1;
	for(i=0; i < 2; i++)
	{
		temp = dtc>>((1-i)*8)&0xF0;
		str[1+2*i] = Num2ASCII(temp>>4);
		ret += 1;
		temp = dtc>>((1-i)*8)&0x0F;
		str[2+2*i] = Num2ASCII(temp);
		ret += 1;
	}
	str[ret] = '\0';

	return ret;
}

//字符数据 转 INT型数据
void rxbuf2dtcbuf(int *dtcbuf, unsigned short *dtclen, unsigned char *rxbuf, unsigned short rxlen, unsigned char isotype)
{
	uint16_t offset = 0;
	uint16_t i = 0;

	if(isotype == 0)
	{
		while(rxlen > 0)//不谨慎
		{
			dtcbuf[i] = rxbuf[offset]<<8;			//DTC M
			dtcbuf[i] |= rxbuf[offset+1];			//DTC L
			offset += 2;
			i += 1;
			if(rxlen >= 2)
				rxlen -= 2;
			else
				rxlen = 0;//忽略最后一个字节
		}
			*dtclen = i;
	}

	return;
}

//字符数据 转 字符串数据显示?
void rxbuf2String(char *rxbuf, char rxlen, unsigned char system, char *show)
{
	char tempbuf[50];
	char offset = 0;
//	char temp;
	int i = 0;

	for(i = 0; i < rxlen; i++)
	{
		offset += IntToSysChar(rxbuf[i], &tempbuf[offset], system);
	}

	memcpy(showDiagnosBuf[showDiagnosLen], show, MAX_LINE_LEN);
	strcat(showDiagnosBuf[showDiagnosLen], tempbuf);
	showDiagnosLen += 1;
}

/*******************************************************************************
 * Name: AnalysisDtc
 * Description: Analysis DTC code
 * Parameters:
 * Return: int
 *******************************************************************************/
int AnalysisDtc(char *path, int *dtcdata, unsigned short dtclen, int protocol)
{
	int ret;
	int len;
	char rxbuf[70];
	int index;
	char displaylen[4] = {0};
	int i;
	int j;
	int checknum;
	index = 0;
	len = 0;

	for(i = 0; i < dtclen; i++)
	{
		checknum = IntToChar(dtcdata[i], showDiagnosBuf[i+1], protocol);//转换为字符串
	}

	do{
		ret = SD_Read(path,rxbuf,index,70);	//读取DTC故障列表
		if(ret != 0)
		{
			for(i = 0; i<ret; i++)
			{
				if(rxbuf[i] == '\r')
				{
					break;
				}
			}
			index = index+i+2;

			j = 0;
			do{
				ret = strncmp(showDiagnosBuf[j+1], rxbuf, sizeof(char)*checknum);

				if(ret == 0)
				{
					memset(showDiagnosBuf[j+1], '\0', MAX_LINE_LEN);
					memcpy(showDiagnosBuf[j+1], rxbuf, i);
					len++;
					if(len != dtclen+1)
					{
						ret = 1;
						break;
					}
				}
				else
				{//验证失败
					if(j != dtclen+1)
						j++;
					else
						break;
				}
			}while(1);
		}
	}while(ret != 0);

	showDiagnosLen = len+1;//显示行数

	IntToSysChar(len, displaylen, 10);
	strcpy(showDiagnosBuf[0], "故障码个数为：");
	strcat(showDiagnosBuf[0], displaylen);
	return ret;
}

void analysis_NegativeResponseCode(int code)
{
	switch(code)
	{
		case 0x01:
			strcpy(showDiagnosBuf[showDiagnosLen], "发送失败,请检查线路是否故障！");
			showDiagnosLen++;
			break;
		case 0x02:
			strcpy(showDiagnosBuf[showDiagnosLen], "校验失败");
			showDiagnosLen++;
			break;
		case 0x10:
			strcpy(showDiagnosBuf[showDiagnosLen], "拒绝向客户端提供服务");
			showDiagnosLen++;
			break;
		case 0x11:
			strcpy(showDiagnosBuf[showDiagnosLen], "服务器禁用客户端请求的诊断服务");
			showDiagnosLen++;
			break;
		case 0x12:
			strcpy(showDiagnosBuf[showDiagnosLen], "服务器禁用客户端请求服务的子功能");
			showDiagnosLen++;
			break;
		case 0x13:
			strcpy(showDiagnosBuf[showDiagnosLen], "服务器认为客户端的请求报文的数据长度(或者格式)不符合本标准");
			showDiagnosLen++;
			break;
		case 0x21:
			strcpy(showDiagnosBuf[showDiagnosLen], "服务器正忙，无法处理客户端发出的请求。此否定响应表明诊断服务结束");
			showDiagnosLen++;
			break;
		case 0x22:
			strcpy(showDiagnosBuf[showDiagnosLen], "服务器执行诊断服务的条件不满足");
			showDiagnosLen++;
			break;
		case 0x23:
			break;
		case 0x24:
			strcpy(showDiagnosBuf[showDiagnosLen], "服务器认为诊断服务的请求(或者执行)顺序错误");
			showDiagnosLen++;
			break;
		case 0x31:
			strcpy(showDiagnosBuf[showDiagnosLen], "服务器没有客户端请求的数据，此否定响应适用于启用数据读、写，或者根据数据调整功能的服务器");
			showDiagnosLen++;
			break;
		case 0x32:
			break;
		case 0x33:
			strcpy(showDiagnosBuf[showDiagnosLen], "服务器阻止客户端的受限诊断服务请求，原因包括：.. 服务器的测试条件不满足.. 服务器的安全状态处于锁定状态");
			showDiagnosLen++;
			break;
		case 0x34:		 //
			break;
		case 0x35:
			strcpy(showDiagnosBuf[showDiagnosLen], "服务器认为客户端返回的密钥错误");
			showDiagnosLen++;
			break;
		case 0x36:
			strcpy(showDiagnosBuf[showDiagnosLen], "服务器认为客户端尝试安全访问(解锁)的失败次数超标");
			showDiagnosLen++;
			break;
		case 0x37:
			strcpy(showDiagnosBuf[showDiagnosLen], "服务器拒绝客户端的安全访问请求，因为服务器允许接收请求的计时器未到时");
			showDiagnosLen++;
			break;
		case 0x70:
			strcpy(showDiagnosBuf[showDiagnosLen], "服务器由于某种故障而拒绝客户端对服务器内存的上传/下载操作");
			showDiagnosLen++;
			break;
		case 0x71:
			strcpy(showDiagnosBuf[showDiagnosLen], "服务器由于某种故障而终止了正在运行的数据传输");
			showDiagnosLen++;
			break;
		case 0x72:
			strcpy(showDiagnosBuf[showDiagnosLen], "再擦除或者烧写非易失性内存的过程中，服务器由于发现错误而终止诊断服务");
			showDiagnosLen++;
			break;
		case 0x73:
			strcpy(showDiagnosBuf[showDiagnosLen], "服务器发现客户端的发送数据(SID = 0x36)请求报文的blockSequenceCounter 计数错误");
			showDiagnosLen++;
			break;
		case 0x78:
			strcpy(showDiagnosBuf[showDiagnosLen], "服务器正确接收到客户端发送的请求，正在处理中，但尚未处理完，此否定响应的发送时间应满足本标准第9.3.1节P2CAN_Server 的要求，并且服务器应重复发送此否定响应，直到完成操作。");
			showDiagnosLen++;
			break;
		case 0x7e:
			strcpy(showDiagnosBuf[showDiagnosLen], "在当前诊断模式下，服务器禁用客户端请求服务的子功能");
			showDiagnosLen++;
			break;
		case 0x7f:
			strcpy(showDiagnosBuf[showDiagnosLen], "在当前诊断模式下，服务器禁用客户端请求的SID");
			showDiagnosLen++;
			break;
		default:
			strcpy(showDiagnosBuf[showDiagnosLen], "未接收到响应消息");
			showDiagnosLen++;
			break;
	}
}


//通讯初始化
extern uint32_t UdsTxId;
char commbuf[256];
extern void USARTChangeBuand(USART_TypeDef* USARTx,uint32_t buand);
int CommuntionInit(DiagnoseStr *diaInfo)
{
	int inittype = 0;
	char rxbuf[1];
	int rxlen;

	if(diaInfo->commtypes == 0)
	{
		NL_Init(diaInfo->Baud, diaInfo->funAddr, diaInfo->tgtAddr, NULL);
		UdsTxId = diaInfo->srcAddr;
	}
	else if(diaInfo->commtypes == 1)
	{
		USARTChangeBuand(USART1, diaInfo->Baud);//改变波特率

		if(inittype)
			return KwpFastInit(USART1,diaInfo->tgtAddr,diaInfo->funAddr);
		else
			return Kwp5BaudInit(USART1, diaInfo->funAddr, rxbuf, &rxlen);
	}
	
	return 0;
}

//K线服务 帧格式
int kwpFmt(unsigned char *txbuf, int len)
{
	int offset = 0;
	//0xd6 2个字节
	if((KB1&0x04) != 0)
	{//是否启用一字节
//		txbuf[offset] &= 0x7F;	//首地址第一个为0
		if((KB1&0x01) != 0)
		{//启用格式字节中的长度
			txbuf[offset] = 0;
			txbuf[offset++] |= len;		//1字节
		}
		else if((KB1&0x02)!= 0)
		{//启用附加长度信息
			txbuf[offset++] = 0;
			txbuf[offset++] = len;		//2字节
		}
		else
		{//鬼知道
		}
	}
	else if((KB1&0x04) == 0)
	{//禁用一字节
		if((KB1&0x02)!= 0)
		{//启用附加长度
			txbuf[offset++] = 0;
			txbuf[offset++] = len;	//2字节
		}
		else if((KB1&0x08)!= 0 && (KB1&0x01) != 0)
		{//添加地址且启用格式文件长度
			txbuf[offset++] |= len;
			txbuf[offset++] = diagnose.tgtAddr;
			txbuf[offset++] = diagnose.srcAddr;	//3字节
		}
		else
		{//鬼知道
		}
	}
	return offset;
}


//解析支持的子功能服务
static int uds_service_referSuppport(char *rxbuf, char *referBuf, char *rpNum, char referid)
{
	int ret = 0;
	int offset = 0;
	int bMask = 0x80;
	int i = 0;

	int rpNumTemp = *rpNum;

	do
	{
		for(i = 0; i < 8; i++)
		{
			if(rxbuf[offset] & (bMask>>i))
			{
				referBuf[rpNumTemp] = i+(1+offset*8)+referid;
				if(referBuf[rpNumTemp] == 0x13)
				{
					o2sensorDefine = 0;
				}
				else if(referBuf[rpNumTemp] == 0x1D)
				{
					o2sensorDefine = 1;
				}
				rpNumTemp++;
			}
		}
		offset++;
	}while(offset < 4);

	if(referBuf[rpNumTemp-1] == referid+0x20)
	{
		rpNumTemp -= 1;
		ret = 0;//继续
	}
	else
	{
		ret = 1;
	}

	*rpNum = rpNumTemp;
	return ret;
}

//参数：ds 诊断信息结构体 idbuf 诊断支持功能码 rpNum 支持功能码个数
int uds_service_request(DiagnoseStr *ds, char *idbuf, char *rpNum)
{
	int ret = 0;
	
	int rxlen = 0;

	unsigned char referCmd = 0x00;//ds->sub_function;
	sendFailedCnt = 0;
	
	while(ret == 0)
	{
		ret = can_service_sid(ds->server_id, referCmd, (unsigned char *)commbuf, &rxlen);
		if(ret == 0)
		{//开启查询 
			ret = uds_service_referSuppport(&commbuf[(rxlen-4)], idbuf, rpNum, referCmd);
			if(ret == 0)
			{
				referCmd += 0x20;
				vTaskDelay(10);
			}
			else
			{
				return 0;//查询结束
			}
		}
		else if(ret == 1)
		{
			if(sendFailedCnt++ >= 2)
				return 1;
		}
	}

	return ret;
}

//诊断在线
unsigned char diagOnlineSts = OFF_LINE;
int uds_Diagnosis_of_online(void)
{
	int ret = 0;

	int i = 0;
	int txlen = 0;
	int checklen = 0;
	int checkbuf[10];
	char rxbuf[10];
	int rxlen;
	static int againCnt = 0;

	if(diagnose.commtypes == 0)
	{
		againCnt = 0;
		return 0;//CAN线目前无需诊断在线
	}
	
	do{
		txlen = 0;
		commbuf[txlen++] = 0x82;
		commbuf[txlen++] = 0x10;
		commbuf[txlen++] = 0xF1;
		commbuf[txlen++] = 01;
		commbuf[txlen++] = 00;
		checklen = 0;
		checkbuf[checklen++] = 01 + 0x40;
		checkbuf[checklen++] = 00;
		ret = KwpRequest(commbuf, txlen, (char *)rxbuf, &rxlen, NULL);
		if(ret == 0)
		{
			for(i = 0; i < checklen; i++)
			{
				if(checkbuf[i] != rxbuf[i])
				{
					againCnt = 0;
					return 2;//ECU 响应失败
				}
			}
		}
		else
		{
			if(againCnt++ >= 2)
			{
				againCnt = 0;
				return 1;//退出循环发送
			}
		}
	}while(ret != 0);

	againCnt = 0;
	return ret;
}

int can_service_sid(int sid, int subid, unsigned char *rxbuf, int *rxlen)
{
	int ret = 0;
	int i = 0;
	int txlen = 0;
	int checklen = 0;
	int checkbuf[2];
	
	commbuf[txlen++] = sid;
	checkbuf[checklen++] = sid + 0x40;

	if(sid == 0x02)
	{
		commbuf[txlen++] = subid;
		commbuf[txlen++] = 0x00;
		checkbuf[checklen++] = subid;
		checkbuf[checklen++] = 0x00;
	}
	else if(sid == 0x01
				||sid == 0x06
				||sid == 0x08
				||sid == 0x09
				)
	{
		commbuf[txlen++] = subid;
		checkbuf[checklen++] = subid;
	}
	

	//源地址确定
	ret = UdsRequest1(diagnose.funAddr, 0, (unsigned char *)commbuf, txlen, rxbuf, (unsigned short *)rxlen);
	
	if(ret == 0)
	{
		for(i = 0; i < checklen; i++)
		{
			if(checkbuf[i] != rxbuf[i])
			{
				return 2;//校验失败
			}
		}
	}

	return ret;
}

int kwp_service_sid(int sid, int subid, unsigned char *rxbuf, int *rxlen)
{
	int ret = 0;
	int i = 0;
	int txlen = 0;
	int checklen = 0;
	int checkbuf[3];
	unsigned char rxbuftmp[20];
	int rxlentmp;
	int MsgCount = 0;
	
	commbuf[txlen++] = 0x82;
	commbuf[txlen++] = 0x10;//目标地址
	commbuf[txlen++] = 0xF1;//源地址
	commbuf[txlen++] = sid;
	checkbuf[checklen++] = sid+0x40;
	if(sid == 0x02)
	{
		commbuf[txlen++] = subid;
		commbuf[txlen++] = 0x00;
		checkbuf[checklen++] = subid;
		checkbuf[checklen++] = 0x00;
		commbuf[0] = 0x83;
	}
	else if(sid == 0x05)
	{
		commbuf[txlen++] = subid;
		commbuf[txlen++] = o2sensorLocation;
		checkbuf[checklen++] = subid;
		checkbuf[checklen++] = o2sensorLocation;
		commbuf[0] = 0x83;
	}
	else if(sid == 0x01
				||sid == 0x06
				||sid == 0x08
				||sid == 0x09
				)
	{
		commbuf[txlen++] = subid;
		checkbuf[checklen++] = subid;
	}
	
	ret = KwpRequest(commbuf, txlen, (char *)rxbuf, rxlen, NULL);
	if(ret == 0)
	{
		for(i = 0; i < checklen; i++)
		{
			if(checkbuf[i] != rxbuf[i])
			{
				return 2;
			}
		}
	
		if(sid == 0x09)
		{
			switch(subid)
			{
				case 0x02:
					MsgCount = MessageCount1;
					break;
				case 0x04:
					MsgCount = MessageCount3;
					break;
				case 0x06:
					MsgCount = MessageCount5;
					break;
				case 0x09:
					MsgCount = MessageCount7;
					break;
				case 0x0A:
					MsgCount = MessageCount9;
					break;
			}
			rxlentmp += *rxlen;
			for(i = 0; i < MsgCount; i++)
			{
				ret = KwpRx(USART1,(uint8_t *)rxbuftmp,(uint16_t *)rxlen, 200);	//normal P2
				if(ret == 0)
				{
					for(i = 0; i < checklen; i++)
					{
						if(checkbuf[i] != rxbuftmp[i])
						{
							return 2;	
						}
					}
					
					for(i = 0; i < (*rxlen)-2; i++)
					{
						rxbuf[rxlentmp+i] = rxbuftmp[2+i];
					}
					rxlentmp += *rxlen-1;
				}
			}
			*rxlen = rxlentmp;
		}
	}

	return ret;

}


int uds_service_sid01(unsigned char *rxbuf, int *rxlen)
{
	int ret = 0;
	int i = 0;
	int txlen = 0;
	int checklen = 0;
	int checkbuf[2];

	sendFailedCnt = 0;
	do{
		txlen = 0;
		checklen = 0;
		if(diagnose.commtypes == 0)
		{
			commbuf[txlen++] = diagnose.server_id;
			commbuf[txlen++] = diagnose.sub_function;
			checkbuf[checklen++] = diagnose.server_id + 0x40;
			checkbuf[checklen++] = diagnose.sub_function;
			ret = UdsRequest1(diagnose.funAddr, 0, (unsigned char *)commbuf, txlen, rxbuf, (unsigned short *)rxlen);
			if(ret == 0)
			{
				for(i = 0; i < checklen; i++)
				{
					if(checkbuf[i] != rxbuf[i])
					{
						return 2;
					}
				}
			}
			else if(ret == 1)
			{
				if(sendFailedCnt++ >= 3)
				{
					return 1;//发生接收错误
				}
			}
			else
			{
				return ret;
			}
		}
		else
		{
//			txlen = kwpFmt((unsigned char *)commbuf, 2);//头部信息
			commbuf[txlen++] = 0x82;
			commbuf[txlen++] = 0x10;
			commbuf[txlen++] = 0xF1;
			commbuf[txlen++] = diagnose.server_id;
			commbuf[txlen++] = diagnose.sub_function;
			checkbuf[checklen++] = diagnose.server_id + 0x40;
			checkbuf[checklen++] = diagnose.sub_function;
			ret = KwpRequest(commbuf, txlen, (char *)rxbuf, rxlen, NULL);
			if(ret == 0)
			{
				for(i = 0; i < checklen; i++)
				{
					if(checkbuf[i] != rxbuf[i])
					{
						return 2;
					}
				}
			}
			else if(ret == 1)
			{
				if(sendFailedCnt++ >= 3)
				{
					return 1;//发生接收错误
				}
			}
			else
			{
				return ret;
			}
		}
	}while(ret != 0);

	return ret;
}

#pragma region  pid01-

 
void analysis_sid0102_pid01(char *data)
{
	int offset = 0;
	int DtcNum = 0;
	char Numbuf[3] = {0};

	//第一个数据
	if(data[offset]&0x80)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid01 [Abit7] 故障指示灯：ON");
	}
	else
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid01 [Abit7] 故障指示灯：OFF");
	}
	showDiagnosLen++;
	DtcNum = data[offset]&0x7F;
	IntToSysChar(DtcNum, Numbuf, 10);
	strcpy(showDiagnosBuf[showDiagnosLen], "pid01 [Abits6-0] DTC数量：");
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	showDiagnosLen++;

	//DATAB
	offset++;	//下一个数据

	if(data[offset]&0x01)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid01 [Bbit0]启用失火监测");
		if(data[offset]&0x10)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "：pid01 [Bbit4]监测未完成");
		}
		else
		{
			strcat(showDiagnosBuf[showDiagnosLen], "：pid01 [Bbit4]监测完成");
		}
	}
	else
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid01 [Bbit0]禁用失火监测");
	}
	showDiagnosLen++;

	if(data[offset]&0x02)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid01 [Bbit1]启用燃油系统监测");
		if(data[offset]&0x20)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "：pid01 [Bbit5]监测未完成");
		}
		else
		{
			strcat(showDiagnosBuf[showDiagnosLen], "：pid01 [Bbit5]监测完成");
		}
	}
	else
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid01 [Bbit1]禁用燃油系统监测");
	}
	showDiagnosLen++;

	if(data[offset]&0x04)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid01 [Bbit2]启用全面组件监测");
		if(data[offset]&0x40)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "：pid01 [Bbit6]监测未完成");
		}
		else
		{
			strcat(showDiagnosBuf[showDiagnosLen], "：pid01 [Bbit6]监测完成");
		}
	}
	else
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid01 [Bbit2]禁用全面组件监测");
	}
	showDiagnosLen++;

	//DATAC
	offset++;	//下一个数据
	if(data[offset]&0x01)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid01 [Cbit0]启用催化剂监测");
		if(data[offset+1]&0x01)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "：pid01 [Dbit0]监测未完成");
		}
		else
		{
			strcat(showDiagnosBuf[showDiagnosLen], "：pid01 [Dbit0]监测完成");
		}
	}
	else
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid01 [Cbit0]禁用催化剂监测");
	}
	showDiagnosLen++;

	if(data[offset]&0x02)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid01 [Cbit1]启用加热催化剂监测");
		if(data[offset+1]&0x02)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "：pid01 [Dbit1]监测未完成");
		}
		else
		{
			strcat(showDiagnosBuf[showDiagnosLen], "：pid01 [Dbit1]监测完成");
		}
	}
	else
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid01 [Cbit1]禁用加热催化剂监测");
	}
	showDiagnosLen++;

	if(data[offset]&0x04)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid01 [Cbit2]启用蒸发系统监测");
		if(data[offset+1]&0x04)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "：pid01 [Dbit2]监测未完成");
		}
		else
		{
			strcat(showDiagnosBuf[showDiagnosLen], "：pid01 [Dbit2]监测完成");
		}
	}
	else
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid01 [Cbit2]禁用蒸发系统监测");
	}
	showDiagnosLen++;

	if(data[offset]&0x08)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid01 [Cbit3]启用二次空气系统监测");
		if(data[offset+1]&0x08)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "：pid01 [Dbit3]监测未完成");
		}
		else
		{
			strcat(showDiagnosBuf[showDiagnosLen], "：pid01 [Dbit3]监测完成");
		}
	}
	else
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid01 [Cbit3]禁用二次空气系统监测");
	}
	showDiagnosLen++;

	if(data[offset]&0x10)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid01 [Cbit4]启用A/C系统制冷剂监测");
		if(data[offset+1]&0x10)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "：pid01 [Dbit4]监测未完成");
		}
		else
		{
			strcat(showDiagnosBuf[showDiagnosLen], "：pid01 [Dbit4]监测完成");
		}
	}
	else
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid01 [Dbit4]禁用A/C系统制冷剂监测");
	}
	showDiagnosLen++;

	if(data[offset]&0x20)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid01 [Cbit5]启用氧传感器监测");
		if(data[offset+1]&0x20)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "：pid01 [Dbit5]监测未完成");
		}
		else
		{
			strcat(showDiagnosBuf[showDiagnosLen], "：pid01 [Dbit5]监测完成");
		}
	}
	else
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid01 [Cbit5]禁用氧传感器监测");
	}
	showDiagnosLen++;

	if(data[offset]&0x40)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid01 [Cbit6]启用氧传感加热器监测");
		if(data[offset+1]&0x40)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "：pid01 [Dbit6]监测未完成");
		}
		else
		{
			strcat(showDiagnosBuf[showDiagnosLen], "：pid01 [Dbit6]监测完成");
		}
	}
	else
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid01 [Cbit6]禁用氧传感加热器监测");
	}
	showDiagnosLen++;

	if(data[offset]&0x80)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid01 [Cbit7]启用EGR系统监测");

		if(data[offset+1]&0x80)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "：pid01 [Dbit7]监测未完成");
		}
		else
		{
			strcat(showDiagnosBuf[showDiagnosLen], "：pid01 [Dbit7]监测完成");
		}
	}
	else
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid01 [Cbit7]禁用EGR系统监测");
	}
	showDiagnosLen++;
}

void analysis_sid0102_pid02(char *data)
{
	int DtcRxbuf[2];
	int dtclen = 0;

	//第一个数据
	strcpy(showDiagnosBuf[showDiagnosLen], "pid02 [A]冻结帧的DTC：");
	showDiagnosLen++;

	rxbuf2dtcbuf(DtcRxbuf, (unsigned short *)&dtclen, (unsigned char *)data, 2, 0);

	AnalysisDtc(dtcPath, DtcRxbuf, dtclen, 0);
}

void analysis_sid0102_pid03(char *data)
{
	int offset = 0;

	//第一个数据
	strcpy(showDiagnosBuf[showDiagnosLen], "pid03 [A]燃油系统1：");

	if(data[offset]&0x01)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid03 [Abit0]开环-还未达到闭环条件");
		showDiagnosLen++;
	}

	if(data[offset]&0x02)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid03 [Abit1]闭环-使用氧传感器作为燃料控制的反馈");
		showDiagnosLen++;
	}

	if(data[offset]&0x04)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid03 [Abit2]开环-由于驱动条件（e.g动力富集、减速堆积）");
		showDiagnosLen++;
	}

	if(data[offset]&0x08)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid03 [Abit3]开环-检测到系统故障");
		showDiagnosLen++;
	}

	if(data[offset]&0x10)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid03 [Abit4]闭环-至少一个氧传感器故障（可能使用单一氧传感器燃料控制）");
		showDiagnosLen++;
	}

	if(0 == (data[offset]&0x1F))
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid03 [Abit5]正常");
		showDiagnosLen++;
	}

	//DATAB
	offset++;	//下一个数据
	strcpy(showDiagnosBuf[showDiagnosLen], "pid03 [B]燃油系统2：");

	if(data[offset]&0x01)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid03 [Bbit0]开环-还未达到闭环条件");
		showDiagnosLen++;
	}

	if(data[offset]&0x02)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid03 [Bbit1]闭环-使用氧传感器作为燃料控制的反馈");
		showDiagnosLen++;
	}

	if(data[offset]&0x04)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid03 [Bbit2]开环-由于驱动条件（e.g动力富集、减速堆积）");
		showDiagnosLen++;
	}

	if(data[offset]&0x08)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid03 [Bbit3]开环-检测到系统故障");
		showDiagnosLen++;
	}

	if(data[offset]&0x10)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid03 [Bbit4]闭环-至少一个氧传感器故障（可能使用单一氧传感器燃料控制）");
		showDiagnosLen++;
	}

	if(0 == (data[offset]&0x1F))
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid03 [Bbit5]正常");
		showDiagnosLen++;
	}

}

void analysis_sid0102_pid04(char *data)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[3] = {0};

	//test
//	data[0] = 0xe5;
//	data[1] = 0x34;

	//第一个数据
	strcpy(showDiagnosBuf[showDiagnosLen], "pid04 [A]负荷量：");

	tempVal = data[offset];
	tempVal *= 1000;
	tempVal /= 255;

	//整数
	tempVal_i = tempVal/10;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//小数部分
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%10;
	memset(Numbuf, '\0', 3);
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "%");

	showDiagnosLen++;
}

void analysis_sid0102_pid05(char *data)
{
	int offset = 0;
	int tempVal = 0;
	char Numbuf[3] = {0};

	//test
//	data[0] = 0xe5;
//	data[1] = 0x34;

	strcpy(showDiagnosBuf[showDiagnosLen], "pid05 [A]发动机冷却液温度：");
	//第一个数据
	tempVal = data[offset];
	//负数
	if(tempVal < 40)
	{
		strcat(showDiagnosBuf[showDiagnosLen], "-");
		tempVal = 40-tempVal;
	}
	else
	{
		tempVal -= 40;
	}

	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "℃");

	showDiagnosLen++;
}

void analysis_sid0102_pid06(char *data)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[3] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "pid06 [A]短期燃料调整BANK1：");
	memset(Numbuf, '\0', 3);
	tempVal = data[offset];//bit
	//是否负数
	if(tempVal < 128)
	{
		strcat(showDiagnosBuf[showDiagnosLen], "-");
		tempVal = 128 - tempVal;
	}
	else
	{
		tempVal -= 128;
	}

	tempVal *= 10000;//放大10000倍
	tempVal /= 128;
	//整数部分
	tempVal_i = tempVal/100;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);
	//小数部分
	tempVal_f = tempVal%100;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "%");
	showDiagnosLen++;

	if(o2sensorDefine != 0)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid06 [B]短期燃料调整BANK3：");
		memset(Numbuf, '\0', 3);
		tempVal = data[offset+1];//bit
		//是否负数
		if(tempVal < 128)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "-");
			tempVal = 128 - tempVal;
		}
		else
		{
			tempVal -= 128;
		}

		tempVal *= 10000;//放大10000倍
		tempVal /= 128;
		//整数部分
		tempVal_i = tempVal/100;
		IntToSysChar(tempVal_i, Numbuf, 10);
		strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

		strcat(showDiagnosBuf[showDiagnosLen], ".");
		memset(Numbuf, '\0', 3);
		//小数部分
		tempVal_f = tempVal%100;
		IntToSysChar(tempVal_f, Numbuf, 10);
		strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
		strcat(showDiagnosBuf[showDiagnosLen], "%");
		showDiagnosLen++;
	}

}

void analysis_sid0102_pid07(char *data)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[3] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "pid07 [A]长期燃料调整BANK1：");
	memset(Numbuf, '\0', 3);
	tempVal = data[offset];//bit
	//是否负数
	if(tempVal < 128)
	{
		strcat(showDiagnosBuf[showDiagnosLen], "-");
		tempVal = 128 - tempVal;
	}
	else
	{
		tempVal -= 128;
	}

	tempVal *= 10000;//放大10000倍
	tempVal /= 128;
	//整数部分
	tempVal_i = tempVal/100;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);
	//小数部分
	tempVal_f = tempVal%100;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "%");
	showDiagnosLen++;

	if(o2sensorDefine != 0)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid07 [B]长期燃料调整BANK3：");
		memset(Numbuf, '\0', 3);
		tempVal = data[offset+1];//bit
		//是否负数
		if(tempVal < 128)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "-");
			tempVal = 128 - tempVal;
		}
		else
		{
			tempVal -= 128;
		}

		tempVal *= 10000;//放大10000倍
		tempVal /= 128;
		//整数部分
		tempVal_i = tempVal/100;
		IntToSysChar(tempVal_i, Numbuf, 10);
		strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

		strcat(showDiagnosBuf[showDiagnosLen], ".");
		memset(Numbuf, '\0', 3);
		//小数部分
		tempVal_f = tempVal%100;
		IntToSysChar(tempVal_f, Numbuf, 10);
		strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
		strcat(showDiagnosBuf[showDiagnosLen], "%");
		showDiagnosLen++;
	}
}

void analysis_sid0102_pid08(char *data)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[3] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "pid08 [A]短期燃料调整BANK2：");
	memset(Numbuf, '\0', 3);
	tempVal = data[offset];//bit
	//是否负数
	if(tempVal < 128)
	{
		strcat(showDiagnosBuf[showDiagnosLen], "-");
		tempVal = 128 - tempVal;
	}
	else
	{
		tempVal -= 128;
	}

	tempVal *= 10000;//放大10000倍
	tempVal /= 128;
	//整数部分
	tempVal_i = tempVal/100;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);
	//小数部分
	tempVal_f = tempVal%100;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "%");
	showDiagnosLen++;

	if(o2sensorDefine != 0)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid08 [B]短期燃料调整BANK4：");
		memset(Numbuf, '\0', 3);
		tempVal = data[offset+1];//bit
		//是否负数
		if(tempVal < 128)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "-");
			tempVal = 128 - tempVal;
		}
		else
		{
			tempVal -= 128;
		}

		tempVal *= 10000;//放大10000倍
		tempVal /= 128;
		//整数部分
		tempVal_i = tempVal/100;
		IntToSysChar(tempVal_i, Numbuf, 10);
		strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

		strcat(showDiagnosBuf[showDiagnosLen], ".");
		memset(Numbuf, '\0', 3);
		//小数部分
		tempVal_f = tempVal%100;
		IntToSysChar(tempVal_f, Numbuf, 10);
		strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
		strcat(showDiagnosBuf[showDiagnosLen], "%");
		showDiagnosLen++;
	}
}

void analysis_sid0102_pid09(char *data)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[3] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "pid09 [A]长期燃料调整BANK2：");
	memset(Numbuf, '\0', 3);
	tempVal = data[offset];//bit
	//是否负数
	if(tempVal < 128)
	{
		strcat(showDiagnosBuf[showDiagnosLen], "-");
		tempVal = 128 - tempVal;
	}
	else
	{
		tempVal -= 128;
	}

	tempVal *= 10000;//放大10000倍
	tempVal /= 128;
	//整数部分
	tempVal_i = tempVal/100;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);
	//小数部分
	tempVal_f = tempVal%100;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "%");
	showDiagnosLen++;
	
	if(o2sensorDefine != 0)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid09 [B]长期燃料调整BANK4：");
		memset(Numbuf, '\0', 3);
		tempVal = data[offset+1];//bit
		//是否负数
		if(tempVal < 128)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "-");
			tempVal = 128 - tempVal;
		}
		else
		{
			tempVal -= 128;
		}

		tempVal *= 10000;//放大10000倍
		tempVal /= 128;
		//整数部分
		tempVal_i = tempVal/100;
		IntToSysChar(tempVal_i, Numbuf, 10);
		strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

		strcat(showDiagnosBuf[showDiagnosLen], ".");
		memset(Numbuf, '\0', 3);
		//小数部分
		tempVal_f = tempVal%100;
		IntToSysChar(tempVal_f, Numbuf, 10);
		strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
		strcat(showDiagnosBuf[showDiagnosLen], "%");
		showDiagnosLen++;
	}
}

void analysis_sid0102_pid0A(char *data)
{
	int offset = 0;
	int tempVal = 0;
	char Numbuf[3] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "pid0A [A]油压：");
	memset(Numbuf, '\0', 3);
	tempVal = data[offset];//bit
	tempVal *= 3;

	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "kPa");
	showDiagnosLen++;

}

void analysis_sid0102_pid0B(char *data)
{
	int offset = 0;
	int tempVal = 0;
	char Numbuf[3] = {0};

	//test
//	data[0] = 0xe5;
//	data[1] = 0x34;

	strcpy(showDiagnosBuf[showDiagnosLen], "pid0B [A]进气歧管绝对压力：");
	//第一个数据
	tempVal = data[offset];

	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "kPa");

	showDiagnosLen++;
}

void analysis_sid0102_pid0C(char *data)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[3] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "pid0C [AB]发动机转速：");
	//两个数据
	tempVal = data[offset];
	tempVal <<= 8;
	tempVal |= data[offset+1];//bit

	//整数部分
	tempVal_i =  tempVal/4;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);
	//小数部分
	tempVal_f = tempVal%4;
	tempVal_f *= 25;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "r/min");

	showDiagnosLen++;
}

void analysis_sid0102_pid0D(char *data)
{

	int offset = 0;
	int tempVal = 0;
	char Numbuf[3] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "pid0D [A]车速：");
	memset(Numbuf, '\0', 3);
	tempVal = data[offset];//bit

	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "Km/h");
	showDiagnosLen++;

}

void analysis_sid0102_pid0E(char *data)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[3] = {0};

	//test
//	data[0] = 0xe5;
//	data[1] = 0x34;

	strcpy(showDiagnosBuf[showDiagnosLen], "pid0E [A]1号气缸点火正时提前：");
	//第一个数据
	tempVal = data[offset];//bit

	//是否负数
	if(tempVal < 128)
	{
		strcat(showDiagnosBuf[showDiagnosLen], "-");
		tempVal = 128 - tempVal;
	}
	else
	{
		tempVal -= 128;
	}

	//整数部分
	tempVal_i =  tempVal/2;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);
	//小数部分
	tempVal_f = tempVal%2;
	tempVal_f *= 5;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "°");

	showDiagnosLen++;
}

void analysis_sid0102_pid0F(char *data)
{
	int offset = 0;
	int tempVal = 0;
	char Numbuf[3] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "pid0F [A]摄入空气温度：");
	//第一个数据
	tempVal = data[offset];
	//负数
	if(tempVal < 40)
	{
		strcat(showDiagnosBuf[showDiagnosLen], "-");
		tempVal = 40-tempVal;
	}
	else
	{
		tempVal -= 40;
	}

	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "℃");

	showDiagnosLen++;
}

void analysis_sid0102_pid10(char *data)
{
	int offset = 0;
	int tempVal = 0;
	int temmVal_i = 0;
	int temmVal_f = 0;
	char Numbuf[5] = {0};

	//第一个数据
	strcpy(showDiagnosBuf[showDiagnosLen], "pid10 [AB]空气流量：");
	tempVal = data[offset];
	tempVal = (tempVal<<8) | data[offset+1];

	//整数部分
	temmVal_i = tempVal/100;
	IntToSysChar(temmVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 5);
	//小数部分
	temmVal_f = tempVal%100;
	IntToSysChar(temmVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "g/s");
	showDiagnosLen++;

}

void analysis_sid0102_pid11(char *data)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[3] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "pid11 [A]绝对的节气门位置：");
	//第一个数据
	tempVal = data[offset];
	tempVal *= 1000;
	tempVal /= 255;

	//整数
	tempVal_i = tempVal/10;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//小数部分
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%10;
	memset(Numbuf, '\0', 3);
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "%");

	showDiagnosLen++;
}

void analysis_sid0102_pid12(char *data)
{
	int offset = 0;
	int tempVal = 0;

	strcpy(showDiagnosBuf[showDiagnosLen], "pid12 [A]二级空气状况：");
	//第一个数据
	tempVal = data[offset];

	if(tempVal & 0x01)
	{
		strcat(showDiagnosBuf[showDiagnosLen], "pid12 [Abit0]第一催化转化器上游");
	}
	else if(tempVal & 0x02)
	{
		strcat(showDiagnosBuf[showDiagnosLen], "pid12 [Abit1]第一催化转化器下游");
	}
	else if(tempVal & 0x04)
	{
		strcat(showDiagnosBuf[showDiagnosLen], "pid12 [Abit2]大气关闭");
	}
	else
	{
		strcat(showDiagnosBuf[showDiagnosLen], "");
	}

	showDiagnosLen++;
}

void analysis_sid0102_pid13(char *data)
{
	int offset = 0;
	int tempVal = 0;
	char Numbuf[3] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "pid13 [A]氧传感器位置：");
	//第一个数据
	tempVal = data[offset];
	o2sensorNumber = 0;

	if(tempVal & 0x01)
	{
		showDiagnosLen++;
		strcat(showDiagnosBuf[showDiagnosLen], "pid13 [Abit0]氧传感器1在BANK1区");
		o2sensorLocationBuf[o2sensorNumber++] = 0;
	}
	else if(tempVal & 0x10)
	{
		showDiagnosLen++;
		strcat(showDiagnosBuf[showDiagnosLen], "pid13 [Abit4]氧传感器1在BANK2区");
		o2sensorLocationBuf[o2sensorNumber++] = 4;
	}

	if(tempVal & 0x02)
	{
		showDiagnosLen++;
		strcat(showDiagnosBuf[showDiagnosLen], "pid13 [Abit1]氧传感器2在BANK1区");
		o2sensorLocationBuf[o2sensorNumber++] = 1;
	}
	else if(tempVal & 0x20)
	{
		showDiagnosLen++;
		strcat(showDiagnosBuf[showDiagnosLen], "pid13 [Abit5]氧传感器2在BANK2区");
		o2sensorLocationBuf[o2sensorNumber++] = 5;
	}

	if(tempVal & 0x04)
	{
		showDiagnosLen++;
		strcat(showDiagnosBuf[showDiagnosLen], "pid13 [Abit2]氧传感器3在BANK1区");
		o2sensorLocationBuf[o2sensorNumber++] = 2;
	}
	else if(tempVal & 0x40)
	{
		showDiagnosLen++;
		strcat(showDiagnosBuf[showDiagnosLen], "pid13 [Abit6]氧传感器3在BANK2区");
		o2sensorLocationBuf[o2sensorNumber++] = 6;
	}

	if(tempVal & 0x08)
	{
		showDiagnosLen++;
		strcat(showDiagnosBuf[showDiagnosLen], "pid13 [Abit3]氧传感器4在BANK1区");
		o2sensorLocationBuf[o2sensorNumber++] = 3;
	}
	else if(tempVal & 0x80)
	{
		showDiagnosLen++;
		strcat(showDiagnosBuf[showDiagnosLen], "pid13 [Abit7]氧传感器4在BANK2区");
		o2sensorLocationBuf[o2sensorNumber++] = 7;
	}
	showDiagnosLen++;
	
	strcpy(showDiagnosBuf[showDiagnosLen], "pid13 [A]氧传感器数量：");
	tempVal = o2sensorNumber;
	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	showDiagnosLen++;
}

void analysis_sid0102_pid14(char *data, short len)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[3] = {0};
	
	//第一个数据
	strcpy(showDiagnosBuf[showDiagnosLen], "pid14 [A]BANK1氧传感器1输出电压：");

	tempVal = data[offset];//bit
	tempVal *= 5;

	//整数
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//小数部分
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "V");
	showDiagnosLen++;

	if(len != 1)
	{
		//第二个数据
		strcpy(showDiagnosBuf[showDiagnosLen], "pid14 [B]短期燃料平衡：");
		memset(Numbuf, '\0', 3);
		tempVal = data[offset+1];//bit
		//是否负数
		if(tempVal < 128)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "-");
			tempVal = 128 - tempVal;
		}
		else
		{
			tempVal -= 128;
		}

		tempVal *= 10000;//放大10000倍
		tempVal /= 128;
		//整数部分
		tempVal_i = tempVal/100;
		IntToSysChar(tempVal_i, Numbuf, 10);
		strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

		strcat(showDiagnosBuf[showDiagnosLen], ".");
		memset(Numbuf, '\0', 3);
		//小数部分
		tempVal_f = tempVal%100;
		IntToSysChar(tempVal_f, Numbuf, 10);
		strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
		strcat(showDiagnosBuf[showDiagnosLen], "%");

		showDiagnosLen++;
	}
}

void analysis_sid0102_pid15(char *data, short len)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[3] = {0};

	//第一个数据
	strcpy(showDiagnosBuf[showDiagnosLen], "pid15 [A]BANK1氧传感器2输出电压：");

	tempVal = data[offset];//bit
	tempVal *= 5;

	//整数
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//小数部分
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "V");
	showDiagnosLen++;

	if(len != 1)
	{
		//第二个数据
		strcpy(showDiagnosBuf[showDiagnosLen], "pid15 [B]短期燃料平衡：");
		memset(Numbuf, '\0', 3);
		tempVal = data[offset+1];//bit
		//是否负数
		if(tempVal < 128)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "-");
			tempVal = 128 - tempVal;
		}
		else
		{
			tempVal -= 128;
		}

		tempVal *= 10000;//放大10000倍
		tempVal /= 128;
		//整数部分
		tempVal_i = tempVal/100;
		IntToSysChar(tempVal_i, Numbuf, 10);
		strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

		strcat(showDiagnosBuf[showDiagnosLen], ".");
		memset(Numbuf, '\0', 3);
		//小数部分
		tempVal_f = tempVal%100;
		IntToSysChar(tempVal_f, Numbuf, 10);
		strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
		strcat(showDiagnosBuf[showDiagnosLen], "%");

		showDiagnosLen++;
	}
}

void analysis_sid0102_pid16(char *data, short len)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[3] = {0};

	//第一个数据
	if(o2sensorDefine == 0)
		strcpy(showDiagnosBuf[showDiagnosLen], "pid16 [A]BANK1氧传感器3输出电压：");
	else
		strcpy(showDiagnosBuf[showDiagnosLen], "pid16 [A]BANK2氧传感器1输出电压：");


	tempVal = data[offset];//bit
	tempVal *= 5;

	//整数
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//小数部分
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "V");
	showDiagnosLen++;

	if(len != 1)
	{
		//第二个数据
		strcpy(showDiagnosBuf[showDiagnosLen], "pid16 [B]短期燃料平衡：");
		memset(Numbuf, '\0', 3);
		tempVal = data[offset+1];//bit
		//是否负数
		if(tempVal < 128)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "-");
			tempVal = 128 - tempVal;
		}
		else
		{
			tempVal -= 128;
		}

		tempVal *= 10000;//放大10000倍
		tempVal /= 128;
		//整数部分
		tempVal_i = tempVal/100;
		IntToSysChar(tempVal_i, Numbuf, 10);
		strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

		strcat(showDiagnosBuf[showDiagnosLen], ".");
		memset(Numbuf, '\0', 3);
		//小数部分
		tempVal_f = tempVal%100;
		IntToSysChar(tempVal_f, Numbuf, 10);
		strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
		strcat(showDiagnosBuf[showDiagnosLen], "%");

		showDiagnosLen++;
	}
}

void analysis_sid0102_pid17(char *data, short len)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[3] = {0};

	//第一个数据
	if(o2sensorDefine == 0)
		strcpy(showDiagnosBuf[showDiagnosLen], "pid17 [A]BANK1氧传感器4输出电压：");
	else
		strcpy(showDiagnosBuf[showDiagnosLen], "pid16 [A]BANK2氧传感器2输出电压：");

	tempVal = data[offset];//bit
	tempVal *= 5;

	//整数
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//小数部分
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "V");
	showDiagnosLen++;

	if(len != 1)
	{
		//第二个数据
		strcpy(showDiagnosBuf[showDiagnosLen], "pid17 [B]短期燃料平衡：");
		memset(Numbuf, '\0', 3);
		tempVal = data[offset+1];//bit
		//是否负数
		if(tempVal < 128)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "-");
			tempVal = 128 - tempVal;
		}
		else
		{
			tempVal -= 128;
		}

		tempVal *= 10000;//放大10000倍
		tempVal /= 128;
		//整数部分
		tempVal_i = tempVal/100;
		IntToSysChar(tempVal_i, Numbuf, 10);
		strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

		strcat(showDiagnosBuf[showDiagnosLen], ".");
		memset(Numbuf, '\0', 3);
		//小数部分
		tempVal_f = tempVal%100;
		IntToSysChar(tempVal_f, Numbuf, 10);
		strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
		strcat(showDiagnosBuf[showDiagnosLen], "%");

		showDiagnosLen++;
	}
}

void analysis_sid0102_pid18(char *data, short len)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[3] = {0};

	//第一个数据
	if(o2sensorDefine == 0)
		strcpy(showDiagnosBuf[showDiagnosLen], "pid18 [A]BANK2氧传感器1输出电压：");
	else
		strcpy(showDiagnosBuf[showDiagnosLen], "pid18 [A]BANK3氧传感器1输出电压：");

	tempVal = data[offset];//bit
	tempVal *= 5;

	//整数
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//小数部分
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "V");
	showDiagnosLen++;

	if(len != 1)
	{
		//第二个数据
		strcpy(showDiagnosBuf[showDiagnosLen], "pid18 [B]短期燃料平衡：");
		memset(Numbuf, '\0', 3);
		tempVal = data[offset+1];//bit
		//是否负数
		if(tempVal < 128)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "-");
			tempVal = 128 - tempVal;
		}
		else
		{
			tempVal -= 128;
		}

		tempVal *= 10000;//放大10000倍
		tempVal /= 128;
		//整数部分
		tempVal_i = tempVal/100;
		IntToSysChar(tempVal_i, Numbuf, 10);
		strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

		strcat(showDiagnosBuf[showDiagnosLen], ".");
		memset(Numbuf, '\0', 3);
		//小数部分
		tempVal_f = tempVal%100;
		IntToSysChar(tempVal_f, Numbuf, 10);
		strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
		strcat(showDiagnosBuf[showDiagnosLen], "%");

		showDiagnosLen++;
	}
}

void analysis_sid0102_pid19(char *data, short len)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[3] = {0};

	//第一个数据
	if(o2sensorDefine == 0)
		strcpy(showDiagnosBuf[showDiagnosLen], "pid19 [A]BANK2氧传感器2输出电压：");
	else
		strcpy(showDiagnosBuf[showDiagnosLen], "pid19 [A]BANK3氧传感器2输出电压：");

	tempVal = data[offset];//bit
	tempVal *= 5;

	//整数
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//小数部分
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "V");
	showDiagnosLen++;
	
	if(len != 1)
	{
		//第二个数据
		strcpy(showDiagnosBuf[showDiagnosLen], "pid19 [B]短期燃料平衡：");
		memset(Numbuf, '\0', 3);
		tempVal = data[offset+1];//bit
		//是否负数
		if(tempVal < 128)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "-");
			tempVal = 128 - tempVal;
		}
		else
		{
			tempVal -= 128;
		}

		tempVal *= 10000;//放大10000倍
		tempVal /= 128;
		//整数部分
		tempVal_i = tempVal/100;
		IntToSysChar(tempVal_i, Numbuf, 10);
		strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

		strcat(showDiagnosBuf[showDiagnosLen], ".");
		memset(Numbuf, '\0', 3);
		//小数部分
		tempVal_f = tempVal%100;
		IntToSysChar(tempVal_f, Numbuf, 10);
		strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
		strcat(showDiagnosBuf[showDiagnosLen], "%");

		showDiagnosLen++;
	}
}

void analysis_sid0102_pid1A(char *data, short len)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[3] = {0};

	//第一个数据
	if(o2sensorDefine == 0)
		strcpy(showDiagnosBuf[showDiagnosLen], "pid1A [A]BANK2氧传感器3输出电压：");
	else
		strcpy(showDiagnosBuf[showDiagnosLen], "pid1A [A]BANK4氧传感器1输出电压：");

	tempVal = data[offset];//bit
	tempVal *= 5;

	//整数
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//小数部分
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "V");
	showDiagnosLen++;

	if(len != 1)
	{
		//第二个数据
		strcpy(showDiagnosBuf[showDiagnosLen], "pid1A [B]短期燃料平衡：");
		memset(Numbuf, '\0', 3);
		tempVal = data[offset+1];//bit
		//是否负数
		if(tempVal < 128)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "-");
			tempVal = 128 - tempVal;
		}
		else
		{
			tempVal -= 128;
		}

		tempVal *= 10000;//放大10000倍
		tempVal /= 128;
		//整数部分
		tempVal_i = tempVal/100;
		IntToSysChar(tempVal_i, Numbuf, 10);
		strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

		strcat(showDiagnosBuf[showDiagnosLen], ".");
		memset(Numbuf, '\0', 3);
		//小数部分
		tempVal_f = tempVal%100;
		IntToSysChar(tempVal_f, Numbuf, 10);
		strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
		strcat(showDiagnosBuf[showDiagnosLen], "%");

		showDiagnosLen++;
	}
}

void analysis_sid0102_pid1B(char *data, short len)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[3] = {0};

	//第一个数据
	if(o2sensorDefine == 0)
		strcpy(showDiagnosBuf[showDiagnosLen], "pid1B [A]BANK2氧传感器4输出电压：");
	else
		strcpy(showDiagnosBuf[showDiagnosLen], "pid1B [A]BANK4氧传感器2输出电压：");

	tempVal = data[offset];//bit
	tempVal *= 5;

	//整数
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//小数部分
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "V");
	showDiagnosLen++;

	if(len != 1)
	{
		//第二个数据
		strcpy(showDiagnosBuf[showDiagnosLen], "pid1B [B]短期燃料平衡：");
		memset(Numbuf, '\0', 3);
		tempVal = data[offset+1];//bit
		//是否负数
		if(tempVal < 128)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "-");
			tempVal = 128 - tempVal;
		}
		else
		{
			tempVal -= 128;
		}

		tempVal *= 10000;//放大10000倍
		tempVal /= 128;
		//整数部分
		tempVal_i = tempVal/100;
		IntToSysChar(tempVal_i, Numbuf, 10);
		strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

		strcat(showDiagnosBuf[showDiagnosLen], ".");
		memset(Numbuf, '\0', 3);
		//小数部分
		tempVal_f = tempVal%100;
		IntToSysChar(tempVal_f, Numbuf, 10);
		strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
		strcat(showDiagnosBuf[showDiagnosLen], "%");

		showDiagnosLen++;
	}
}

void analysis_sid0102_pid1C(char *data)
{
	int offset = 0;
	int tempVal = 0;
//	int tempVal_i = 0;
//	int tempVal_f = 0;
//	char Numbuf[3] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "pid1C [A]OBD要求车辆的设计：");
	//第一个数据
	tempVal = data[offset];

	switch(tempVal)
	{
		case 0x01:
			strcat(showDiagnosBuf[showDiagnosLen], "OBD Ⅱ");
			break;
		case 0x02:
			strcat(showDiagnosBuf[showDiagnosLen], "OBD");
			break;
		case 0x03:
			strcat(showDiagnosBuf[showDiagnosLen], "OBD and OBD Ⅱ");
			break;
		case 0x04:
			strcat(showDiagnosBuf[showDiagnosLen], "OBD Ⅰ");
			break;
		case 0x05:
			strcat(showDiagnosBuf[showDiagnosLen], "NO OBD");
			break;
		case 0x06:
			strcat(showDiagnosBuf[showDiagnosLen], "EOBD");
			break;
		case 0x07:
			strcat(showDiagnosBuf[showDiagnosLen], "EOBD and OBD Ⅱ");
			break;
		case 0x08:
			strcat(showDiagnosBuf[showDiagnosLen], "EOBD and OBD");
			break;
		case 0x09:
			strcat(showDiagnosBuf[showDiagnosLen], "EOBD,OBD and OBD Ⅱ");
			break;
		case 0x0a:
			strcat(showDiagnosBuf[showDiagnosLen], "JOBD");
			break;
		case 0x0b:
			strcat(showDiagnosBuf[showDiagnosLen], "JOBD and OBD Ⅱ");
			break;
		case 0x0c:
			strcat(showDiagnosBuf[showDiagnosLen], "JOBD and EOBD");
			break;
		case 0x0d:
			strcat(showDiagnosBuf[showDiagnosLen], "JOBD,EOBD,and OBD Ⅱ");
			break;
		case 0x0e:
			strcat(showDiagnosBuf[showDiagnosLen], "EURO Ⅳ B1");
			break;
		case 0x0f:
			strcat(showDiagnosBuf[showDiagnosLen], "EURO Ⅴ B2");
			break;
		case 0x10:
			strcat(showDiagnosBuf[showDiagnosLen], "EURO Ⅴ C");
			break;
		case 0x11:
			strcat(showDiagnosBuf[showDiagnosLen], "EMD");
			break;
		default:
			strcat(showDiagnosBuf[showDiagnosLen], "SAE J1939 special meaning");
			break;
	}

	showDiagnosLen++;
}

void analysis_sid0102_pid1D(char *data)
{
	int offset = 0;
	int tempVal = 0;
	char Numbuf[3] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "pid1D [A]氧传感器位置：");

	//第一个数据
	tempVal = data[offset];
	o2sensorNumber = 0;

	if(tempVal & 0x01)
	{
		showDiagnosLen++;
		strcat(showDiagnosBuf[showDiagnosLen], "pid1D [Abit0]氧传感器1在BANK1区");
		o2sensorLocationBuf[o2sensorNumber++] = 0;
	}
	else if(tempVal & 0x10)
	{
		showDiagnosLen++;
		strcat(showDiagnosBuf[showDiagnosLen], "pid1D [Abit4]氧传感器1在BANK3区");
		o2sensorLocationBuf[o2sensorNumber++] = 4;
	}
	else if(tempVal & 0x04)
	{
		showDiagnosLen++;
		strcat(showDiagnosBuf[showDiagnosLen], "pid1D [Abit2]氧传感器1在BANK2区");
		o2sensorLocationBuf[o2sensorNumber++] = 2;
	}
	else if(tempVal & 0x40)
	{
		showDiagnosLen++;
		strcat(showDiagnosBuf[showDiagnosLen], "pid1D [Abit6]氧传感器1在BANK4区");
		o2sensorLocationBuf[o2sensorNumber++] = 6;
	}

	if(tempVal & 0x02)
	{
		showDiagnosLen++;
		strcat(showDiagnosBuf[showDiagnosLen], "pid1D [Abit1]氧传感器2在BANK1区");
		o2sensorLocationBuf[o2sensorNumber++] = 1;
	}
	else if(tempVal & 0x20)
	{
		showDiagnosLen++;
		strcat(showDiagnosBuf[showDiagnosLen], "pid1D [Abit5]氧传感器2在BANK3区");
		o2sensorLocationBuf[o2sensorNumber++] = 5;
	}
	else if(tempVal & 0x08)
	{
		showDiagnosLen++;
		strcat(showDiagnosBuf[showDiagnosLen], "pid1D [Abit3]氧传感器2在BANK2区");
		o2sensorLocationBuf[o2sensorNumber++] = 3;
	}
	else if(tempVal & 0x80)
	{
		showDiagnosLen++;
		strcat(showDiagnosBuf[showDiagnosLen], "pid1D [Abit7]氧传感器2在BANK4区");
		o2sensorLocationBuf[o2sensorNumber++] = 7;
	}
	showDiagnosLen++;
	
	strcpy(showDiagnosBuf[showDiagnosLen], "pid1D [A]氧传感器数量：");
	tempVal = o2sensorNumber;
	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	showDiagnosLen++;
}

void analysis_sid0102_pid1E(char *data)
{

	int offset = 0;
	int tempVal = 0;

	strcpy(showDiagnosBuf[showDiagnosLen], "pid1E [Abit0]辅助输入状态PTO：");

	tempVal = data[offset];

	if(tempVal & 0x01)
		strcat(showDiagnosBuf[showDiagnosLen], "激活");
	else
		strcat(showDiagnosBuf[showDiagnosLen], "关闭");

	showDiagnosLen++;
}

void analysis_sid0102_pid1F(char *data)
{
	int offset = 0;
	int tempVal = 0;
	char Numbuf[5] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "pid1E [AB]启动时间：");
	//第一个数据
	tempVal = data[offset];
	tempVal <<= 8;
	tempVal |= data[offset+1];//bit

	//整数部分
	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "sec");

	showDiagnosLen++;
}

void analysis_sid0102_pid21(char *data)
{
	int offset = 0;
	int tempVal = 0;
	char Numbuf[5] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "pid21 [AB]当mil被激活时距离移动：");
	//第一个数据
	tempVal = data[offset];
	tempVal <<= 8;
	tempVal |= data[offset+1];//bit

	//整数部分
	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "KM");

	showDiagnosLen++;
}

void analysis_sid0102_pid22(char *data)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[5] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "pid22 [AB]燃料轨压力相对于管道真空：");
	//第一个数据
	tempVal = data[offset];
	tempVal <<= 8;
	tempVal |= data[offset+1];//bit

	tempVal *= 79;

	//整数部分
	tempVal_i =tempVal / 1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, 0, 5);

	//小数部分
	tempVal_f = tempVal / 1000;

	//四舍五入
	if((tempVal_f % 10) >= 5)
	{
		tempVal_f /= 10;
		tempVal_f += 1;
	}
	else
		tempVal_f /= 10;

	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "kpa");

	showDiagnosLen++;
}

void analysis_sid0102_pid23(char *data)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[5] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "pid23 [AB]油轨压力：");
	//第一个数据
	tempVal = data[offset];
	tempVal <<= 8;
	tempVal |= data[offset+1];//bit

	tempVal *= 79;

	//整数部分
	tempVal_i =tempVal / 1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, 0, 5);

	//小数部分
	tempVal_f = tempVal / 1000;

	//四舍五入
	if((tempVal_f % 10) >= 5)
	{
		tempVal_f /= 10;
		tempVal_f += 1;
	}
	else
		tempVal_f /= 10;

	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "kpa");

	showDiagnosLen++;
}

void analysis_sid0102_pid24(char *data)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[3] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "pid24 BANK1 氧传感器1：");
	showDiagnosLen++;

	//第一个数据
	strcpy(showDiagnosBuf[showDiagnosLen], "[A]等效比：");

	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];

	tempVal *= 305;

	//整数
	tempVal_i = tempVal/10000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//小数部分
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%10000000;
	tempVal_f /= 1000;

	//四舍五入
	if((tempVal_f % 10) >= 5)
	{
		tempVal_f /= 10;
		tempVal_f += 1;
	}
	else
		tempVal_f /= 10;

	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	showDiagnosLen++;

	//第二个数据
	strcpy(showDiagnosBuf[showDiagnosLen], "[B]电压：");
	memset(Numbuf, '\0', 3);
	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];

	tempVal *= 122;

	//整数
	tempVal_i = tempVal/1000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//小数部分
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%1000000;
	tempVal_f /= 1000;

	//四舍五入 特殊处理最大值
	if(tempVal == 65535)
	{
		tempVal_f = 999;
	}
//	if((tempVal_f % 10) >= 5)
//	{
//		tempVal_f /= 10;
//		tempVal_f += 1;
//	}
//	else
//		tempVal_f /= 10;

	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "V");

	showDiagnosLen++;
}

void analysis_sid0102_pid25(char *data)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[3] = {0};

	//第一个数据
	strcpy(showDiagnosBuf[showDiagnosLen], "pid25 BANK1氧传感器2：");
	showDiagnosLen++;

	//第一个数据
	strcpy(showDiagnosBuf[showDiagnosLen], "[A]等效比：");

	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];

	tempVal *= 305;

	//整数
	tempVal_i = tempVal/10000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//小数部分
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%10000000;
	tempVal_f /= 1000;

	//四舍五入
	if((tempVal_f % 10) >= 5)
	{
		tempVal_f /= 10;
		tempVal_f += 1;
	}
	else
		tempVal_f /= 10;

	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	showDiagnosLen++;

	//第二个数据
	strcpy(showDiagnosBuf[showDiagnosLen], "[B]电压：");
	memset(Numbuf, '\0', 3);
	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];

	tempVal *= 122;

	//整数
	tempVal_i = tempVal/1000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//小数部分
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%1000000;
	tempVal_f /= 1000;

	//四舍五入 特殊处理最大值
	if(tempVal == 65535)
	{
		tempVal_f = 999;
	}
//	if((tempVal_f % 10) >= 5)
//	{
//		tempVal_f /= 10;
//		tempVal_f += 1;
//	}
//	else
//		tempVal_f /= 10;

	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "V");

	showDiagnosLen++;
}

void analysis_sid0102_pid26(char *data)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[3] = {0};

	//第一个数据
	if(o2sensorDefine == 0)
		strcpy(showDiagnosBuf[showDiagnosLen], "pid26 BANK1氧传感器3输出电压：");
	else
		strcpy(showDiagnosBuf[showDiagnosLen], "pid26 BANK2氧传感器1输出电压：");

	showDiagnosLen++;

	//第一个数据
	strcpy(showDiagnosBuf[showDiagnosLen], "[A]等效比：");

	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];

	tempVal *= 305;

	//整数
	tempVal_i = tempVal/10000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//小数部分
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%10000000;
	tempVal_f /= 1000;

	//四舍五入
	if((tempVal_f % 10) >= 5)
	{
		tempVal_f /= 10;
		tempVal_f += 1;
	}
	else
		tempVal_f /= 10;

	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	showDiagnosLen++;

	//第二个数据
	strcpy(showDiagnosBuf[showDiagnosLen], "[B]电压：");
	memset(Numbuf, '\0', 3);
	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];

	tempVal *= 122;

	//整数
	tempVal_i = tempVal/1000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//小数部分
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%1000000;
	tempVal_f /= 1000;

	//四舍五入 特殊处理最大值
	if(tempVal == 65535)
	{
		tempVal_f = 999;
	}
//	if((tempVal_f % 10) >= 5)
//	{
//		tempVal_f /= 10;
//		tempVal_f += 1;
//	}
//	else
//		tempVal_f /= 10;

	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "V");

	showDiagnosLen++;

}

void analysis_sid0102_pid27(char *data)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[3] = {0};

	//第一个数据
	if(o2sensorDefine == 0)
		strcpy(showDiagnosBuf[showDiagnosLen], "pid27 BANK1氧传感器4输出电压：");
	else
		strcpy(showDiagnosBuf[showDiagnosLen], "pid27 BANK2氧传感器2输出电压：");

	showDiagnosLen++;

	//第一个数据
	strcpy(showDiagnosBuf[showDiagnosLen], "[A]等效比：");

	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];

	tempVal *= 305;

	//整数
	tempVal_i = tempVal/10000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//小数部分
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%10000000;
	tempVal_f /= 1000;

	//四舍五入
	if((tempVal_f % 10) >= 5)
	{
		tempVal_f /= 10;
		tempVal_f += 1;
	}
	else
		tempVal_f /= 10;

	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	showDiagnosLen++;

	//第二个数据
	strcpy(showDiagnosBuf[showDiagnosLen], "[B]电压：");
	memset(Numbuf, '\0', 3);
	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];

	tempVal *= 122;

	//整数
	tempVal_i = tempVal/1000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//小数部分
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%1000000;
	tempVal_f /= 1000;

	//四舍五入 特殊处理最大值
	if(tempVal == 65535)
	{
		tempVal_f = 999;
	}
//	if((tempVal_f % 10) >= 5)
//	{
//		tempVal_f /= 10;
//		tempVal_f += 1;
//	}
//	else
//		tempVal_f /= 10;

	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "V");

	showDiagnosLen++;

}

void analysis_sid0102_pid28(char *data)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[3] = {0};

	//第一个数据
	if(o2sensorDefine == 0)
		strcpy(showDiagnosBuf[showDiagnosLen], "pid28 BANK2氧传感器1输出电压：");
	else
		strcpy(showDiagnosBuf[showDiagnosLen], "pid28 BANK3氧传感器1输出电压：");

	showDiagnosLen++;

	//第一个数据
	strcpy(showDiagnosBuf[showDiagnosLen], "[A]等效比：");

	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];

	tempVal *= 305;

	//整数
	tempVal_i = tempVal/10000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//小数部分
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%10000000;
	tempVal_f /= 1000;

	//四舍五入
	if((tempVal_f % 10) >= 5)
	{
		tempVal_f /= 10;
		tempVal_f += 1;
	}
	else
		tempVal_f /= 10;

	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	showDiagnosLen++;

	//第二个数据
	strcpy(showDiagnosBuf[showDiagnosLen], "[B]电压：");
	memset(Numbuf, '\0', 3);
	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];

	tempVal *= 122;

	//整数
	tempVal_i = tempVal/1000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//小数部分
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%1000000;
	tempVal_f /= 1000;

	//四舍五入 特殊处理最大值
	if(tempVal == 65535)
	{
		tempVal_f = 999;
	}
//	if((tempVal_f % 10) >= 5)
//	{
//		tempVal_f /= 10;
//		tempVal_f += 1;
//	}
//	else
//		tempVal_f /= 10;

	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "V");

	showDiagnosLen++;

}

void analysis_sid0102_pid29(char *data)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[3] = {0};

	//第一个数据
	if(o2sensorDefine == 0)
		strcpy(showDiagnosBuf[showDiagnosLen], "pid29 BANK2氧传感器2输出电压：");
	else
		strcpy(showDiagnosBuf[showDiagnosLen], "pid29 BANK3氧传感器2输出电压：");

	showDiagnosLen++;

	//第一个数据
	strcpy(showDiagnosBuf[showDiagnosLen], "[A]等效比：");

	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];

	tempVal *= 305;

	//整数
	tempVal_i = tempVal/10000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//小数部分
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%10000000;
	tempVal_f /= 1000;

	//四舍五入
	if((tempVal_f % 10) >= 5)
	{
		tempVal_f /= 10;
		tempVal_f += 1;
	}
	else
		tempVal_f /= 10;

	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	showDiagnosLen++;

	//第二个数据
	strcpy(showDiagnosBuf[showDiagnosLen], "[B]电压：");
	memset(Numbuf, '\0', 3);
	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];

	tempVal *= 122;

	//整数
	tempVal_i = tempVal/1000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//小数部分
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%1000000;
	tempVal_f /= 1000;

	//四舍五入 特殊处理最大值
	if(tempVal == 65535)
	{
		tempVal_f = 999;
	}
//	if((tempVal_f % 10) >= 5)
//	{
//		tempVal_f /= 10;
//		tempVal_f += 1;
//	}
//	else
//		tempVal_f /= 10;

	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "V");

	showDiagnosLen++;

}

void analysis_sid0102_pid2A(char *data)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[3] = {0};

	//第一个数据
	if(o2sensorDefine == 0)
		strcpy(showDiagnosBuf[showDiagnosLen], "pid2A BANK2氧传感器3输出电压：");
	else
		strcpy(showDiagnosBuf[showDiagnosLen], "pid2A BANK4氧传感器1输出电压：");

	showDiagnosLen++;

	//第一个数据
	strcpy(showDiagnosBuf[showDiagnosLen], "[A]等效比：");

	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];

	tempVal *= 305;

	//整数
	tempVal_i = tempVal/10000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//小数部分
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%10000000;
	tempVal_f /= 1000;

	//四舍五入
	if((tempVal_f % 10) >= 5)
	{
		tempVal_f /= 10;
		tempVal_f += 1;
	}
	else
		tempVal_f /= 10;

	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	showDiagnosLen++;

	//第二个数据
	strcpy(showDiagnosBuf[showDiagnosLen], "[B]电压：");
	memset(Numbuf, '\0', 3);
	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];

	tempVal *= 122;

	//整数
	tempVal_i = tempVal/1000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//小数部分
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%1000000;
	tempVal_f /= 1000;

	//四舍五入 特殊处理最大值
	if(tempVal == 65535)
	{
		tempVal_f = 999;
	}
//	if((tempVal_f % 10) >= 5)
//	{
//		tempVal_f /= 10;
//		tempVal_f += 1;
//	}
//	else
//		tempVal_f /= 10;

	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "V");

	showDiagnosLen++;

}

void analysis_sid0102_pid2B(char *data)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[3] = {0};

	//第一个数据
	if(o2sensorDefine == 0)
		strcpy(showDiagnosBuf[showDiagnosLen], "pid2B BANK2氧传感器4输出电压：");
	else
		strcpy(showDiagnosBuf[showDiagnosLen], "pid2B BANK4氧传感器2输出电压：");

	showDiagnosLen++;

	//第一个数据
	strcpy(showDiagnosBuf[showDiagnosLen], "[A]等效比：");

	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];

	tempVal *= 305;

	//整数
	tempVal_i = tempVal/10000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//小数部分
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%10000000;
	tempVal_f /= 1000;

	//四舍五入
	if((tempVal_f % 10) >= 5)
	{
		tempVal_f /= 10;
		tempVal_f += 1;
	}
	else
		tempVal_f /= 10;

	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	showDiagnosLen++;

	//第二个数据
	strcpy(showDiagnosBuf[showDiagnosLen], "[B]电压：");
	memset(Numbuf, '\0', 3);
	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];

	tempVal *= 122;

	//整数
	tempVal_i = tempVal/1000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//小数部分
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%1000000;
	tempVal_f /= 1000;

	//四舍五入 特殊处理最大值
	if(tempVal == 65535)
	{
		tempVal_f = 999;
	}
//	if((tempVal_f % 10) >= 5)
//	{
//		tempVal_f /= 10;
//		tempVal_f += 1;
//	}
//	else
//		tempVal_f /= 10;

	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "V");

	showDiagnosLen++;

}

void analysis_sid0102_pid2C(char *data)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[3] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "pid2C [A]命令ERG：");
	//第一个数据
	tempVal = data[offset];
	tempVal *= 1000;
	tempVal /= 255;

	//整数
	tempVal_i = tempVal/10;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//小数部分
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%10;
	memset(Numbuf, '\0', 3);
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "%");

	showDiagnosLen++;
}

void analysis_sid0102_pid2D(char *data)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[3] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "pid2D [A]ERG错误：");
	//第一个数据
	tempVal = data[offset];//bit
	//是否负数
	if(tempVal < 128)
	{
		strcat(showDiagnosBuf[showDiagnosLen], "-");
		tempVal = 128 - tempVal;
	}
	else
	{
		tempVal -= 128;
	}

	tempVal *= 10000;//放大10000倍
	tempVal /= 128;

	//整数部分
	tempVal_i = tempVal/100;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);
	//小数部分
	tempVal_f = tempVal%100;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "%");

	showDiagnosLen++;
}

void analysis_sid0102_pid2E(char *data)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[3] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "pid2E [A]命令蒸发净化：");
	//第一个数据
	tempVal = data[offset];
	tempVal *= 1000;
	tempVal /= 255;

	//整数
	tempVal_i = tempVal/10;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//小数部分
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%10;
	memset(Numbuf, '\0', 3);
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "%");

	showDiagnosLen++;
}

void analysis_sid0102_pid2F(char *data)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[3] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "pid2F [A]燃油输入：");
	//第一个数据
	tempVal = data[offset];
	tempVal *= 1000;
	tempVal /= 255;

	//整数
	tempVal_i = tempVal/10;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//小数部分
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%10;
	memset(Numbuf, '\0', 3);
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "%");

	showDiagnosLen++;
}

void analysis_sid0102_pid30(char *data)
{
	int offset = 0;
	int tempVal = 0;
	char Numbuf[3] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "pid30 [A]自诊断故障清除后热身次数：");
	//第一个数据
	tempVal = data[offset];//bit

	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "Km/h");
	showDiagnosLen++;

}

void analysis_sid0102_pid31(char *data)
{
	int offset = 0;
	int tempVal = 0;
	char Numbuf[5] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "pid31 [AB]自诊断故障清除后行驶距离：");
	//第一个数据
	tempVal = data[offset];
	tempVal <<= 8;
	tempVal |= data[offset+1];

	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "Km");
	showDiagnosLen++;
}

void analysis_sid0102_pid32(char *data)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[3] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "pid32 [AB]EVAP 系统蒸汽压力：");
	//第一个数据
	tempVal = data[offset];
	tempVal <<= 8;
	tempVal |= data[offset+1];//bit

	//整数部分
	tempVal_i =  tempVal/4;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);
	//小数部分
	tempVal_f = tempVal%4;
	tempVal_f *= 25;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "kpa");

	showDiagnosLen++;
}

void analysis_sid0102_pid33(char *data)
{
	int offset = 0;
	int tempVal = 0;
	char Numbuf[3] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "pid33 [A]气压：");
	//第一个数据
	tempVal = data[offset];//bit

	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "kpa");
	showDiagnosLen++;
}

void analysis_sid0102_pid34(char *data)
{
	int offset = 0;
	long long temp = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[3] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "pid34 BANK1 氧传感器1：");
	showDiagnosLen++;

	//第一个数据
	strcpy(showDiagnosBuf[showDiagnosLen], "[A]等效比：");

	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];

	tempVal *= 305;

	//整数
	tempVal_i = tempVal/10000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//小数部分
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%10000000;
	tempVal_f /= 1000;

	//四舍五入
	if((tempVal_f % 10) >= 5)
	{
		tempVal_f /= 10;
		tempVal_f += 1;
	}
	else
		tempVal_f /= 10;


	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	showDiagnosLen++;

	//第二个数据
	strcpy(showDiagnosBuf[showDiagnosLen], "[B]电流：");
	memset(Numbuf, '\0', 3);
	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];
	//正数 负数
	if(tempVal > 32768)
	{
		tempVal -= 32768;
	}
	else
	{
		strcat(showDiagnosBuf[showDiagnosLen], "-");
		tempVal = 32768 - tempVal;
	}

	temp = tempVal * 390625;

	//整数
	tempVal_i = temp/100000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], ".");

	//小数部分
	memset(Numbuf, '\0', 3);
	tempVal_f = temp%100000000;
	tempVal_f /= 100;

	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "V");

	showDiagnosLen++;
}

void analysis_sid0102_pid35(char *data)
{
	int offset = 0;
	long long temp = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[3] = {0};

	//第一个数据
	strcpy(showDiagnosBuf[showDiagnosLen], "pid35 BANK1氧传感器2：");
	showDiagnosLen++;

	//第一个数据
	strcpy(showDiagnosBuf[showDiagnosLen], "[A]等效比：");

	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];

	tempVal *= 305;

	//整数
	tempVal_i = tempVal/10000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//小数部分
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%10000000;
	tempVal_f /= 1000;

	//四舍五入
	if((tempVal_f % 10) >= 5)
	{
		tempVal_f /= 10;
		tempVal_f += 1;
	}
	else
		tempVal_f /= 10;

	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	showDiagnosLen++;

		//第二个数据
	strcpy(showDiagnosBuf[showDiagnosLen], "[B]电流：");
	memset(Numbuf, '\0', 3);
	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];
	//正数 负数
	if(tempVal > 32768)
	{
		tempVal -= 32768;
	}
	else
	{
		strcat(showDiagnosBuf[showDiagnosLen], "-");
		tempVal = 32768 - tempVal;
	}

	temp = tempVal * 390625;

	//整数
	tempVal_i = temp/100000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], ".");

	//小数部分
	memset(Numbuf, '\0', 3);
	tempVal_f = temp%100000000;
	tempVal_f /= 100;

	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "V");

	showDiagnosLen++;
}

void analysis_sid0102_pid36(char *data)
{
	int offset = 0;
	long long temp = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[3] = {0};

	//第一个数据
	if(o2sensorDefine == 0)
		strcpy(showDiagnosBuf[showDiagnosLen], "pid36 BANK1氧传感器3输出电压：");
	else
		strcpy(showDiagnosBuf[showDiagnosLen], "pid36 BANK2氧传感器1输出电压：");

	showDiagnosLen++;

	//第一个数据
	strcpy(showDiagnosBuf[showDiagnosLen], "[A]等效比：");

	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];

	tempVal *= 305;

	//整数
	tempVal_i = tempVal/10000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//小数部分
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%10000000;
	tempVal_f /= 1000;

	//四舍五入
	if((tempVal_f % 10) >= 5)
	{
		tempVal_f /= 10;
		tempVal_f += 1;
	}
	else
		tempVal_f /= 10;

	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	showDiagnosLen++;

	//第二个数据
	strcpy(showDiagnosBuf[showDiagnosLen], "[B]电流：");
	memset(Numbuf, '\0', 3);
	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];
	//正数 负数
	if(tempVal > 32768)
	{
		tempVal -= 32768;
	}
	else
	{
		strcat(showDiagnosBuf[showDiagnosLen], "-");
		tempVal = 32768 - tempVal;
	}

	temp = tempVal * 390625;

	//整数
	tempVal_i = temp/100000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], ".");

	//小数部分
	memset(Numbuf, '\0', 3);
	tempVal_f = temp%100000000;
	tempVal_f /= 100;

	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "V");

	showDiagnosLen++;

}

void analysis_sid0102_pid37(char *data)
{
	int offset = 0;
	long long temp = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[3] = {0};

	//第一个数据
	if(o2sensorDefine == 0)
		strcpy(showDiagnosBuf[showDiagnosLen], "pid37 BANK1氧传感器4输出电压：");
	else
		strcpy(showDiagnosBuf[showDiagnosLen], "pid37 BANK2氧传感器2输出电压：");

	showDiagnosLen++;

	//第一个数据
	strcpy(showDiagnosBuf[showDiagnosLen], "[A]等效比：");

	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];

	tempVal *= 305;

	//整数
	tempVal_i = tempVal/10000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//小数部分
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%10000000;
	tempVal_f /= 1000;

	//四舍五入
	if((tempVal_f % 10) >= 5)
	{
		tempVal_f /= 10;
		tempVal_f += 1;
	}
	else
		tempVal_f /= 10;

	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	showDiagnosLen++;

	//第二个数据
	strcpy(showDiagnosBuf[showDiagnosLen], "[B]电流：");
	memset(Numbuf, '\0', 3);
	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];
	//正数 负数
	if(tempVal > 32768)
	{
		tempVal -= 32768;
	}
	else
	{
		strcat(showDiagnosBuf[showDiagnosLen], "-");
		tempVal = 32768 - tempVal;
	}

	temp = tempVal * 390625;

	//整数
	tempVal_i = temp/100000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], ".");

	//小数部分
	memset(Numbuf, '\0', 3);
	tempVal_f = temp%100000000;
	tempVal_f /= 100;

	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "V");

	showDiagnosLen++;

}

void analysis_sid0102_pid38(char *data)
{
	int offset = 0;
	long long temp = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[3] = {0};

	//第一个数据
	if(o2sensorDefine == 0)
		strcpy(showDiagnosBuf[showDiagnosLen], "pid38 BANK2氧传感器1输出电压：");
	else
		strcpy(showDiagnosBuf[showDiagnosLen], "pid38 BANK3氧传感器1输出电压：");

	showDiagnosLen++;

	//第一个数据
	strcpy(showDiagnosBuf[showDiagnosLen], "[A]等效比：");

	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];

	tempVal *= 305;

	//整数
	tempVal_i = tempVal/10000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//小数部分
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%10000000;
	tempVal_f /= 1000;

	//四舍五入
	if((tempVal_f % 10) >= 5)
	{
		tempVal_f /= 10;
		tempVal_f += 1;
	}
	else
		tempVal_f /= 10;

	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	showDiagnosLen++;

		//第二个数据
	strcpy(showDiagnosBuf[showDiagnosLen], "[B]电流：");
	memset(Numbuf, '\0', 3);
	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];
	//正数 负数
	if(tempVal > 32768)
	{
		tempVal -= 32768;
	}
	else
	{
		strcat(showDiagnosBuf[showDiagnosLen], "-");
		tempVal = 32768 - tempVal;
	}

	temp = tempVal * 390625;

	//整数
	tempVal_i = temp/100000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], ".");

	//小数部分
	memset(Numbuf, '\0', 3);
	tempVal_f = temp%100000000;
	tempVal_f /= 100;

	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "V");

	showDiagnosLen++;

}

void analysis_sid0102_pid39(char *data)
{
	int offset = 0;
	long long temp = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[3] = {0};

	//第一个数据
	if(o2sensorDefine == 0)
		strcpy(showDiagnosBuf[showDiagnosLen], "pid39 BANK2氧传感器2输出电压：");
	else
		strcpy(showDiagnosBuf[showDiagnosLen], "pid39 BANK3氧传感器2输出电压：");

	showDiagnosLen++;

	//第一个数据
	strcpy(showDiagnosBuf[showDiagnosLen], "[A]等效比：");

	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];

	tempVal *= 305;

	//整数
	tempVal_i = tempVal/10000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//小数部分
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%10000000;
	tempVal_f /= 1000;

	//四舍五入
	if((tempVal_f % 10) >= 5)
	{
		tempVal_f /= 10;
		tempVal_f += 1;
	}
	else
		tempVal_f /= 10;

	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	showDiagnosLen++;

		//第二个数据
	strcpy(showDiagnosBuf[showDiagnosLen], "[B]电流：");
	memset(Numbuf, '\0', 3);
	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];
	//正数 负数
	if(tempVal > 32768)
	{
		tempVal -= 32768;
	}
	else
	{
		strcat(showDiagnosBuf[showDiagnosLen], "-");
		tempVal = 32768 - tempVal;
	}

	temp = tempVal * 390625;

	//整数
	tempVal_i = temp/100000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], ".");

	//小数部分
	memset(Numbuf, '\0', 3);
	tempVal_f = temp%100000000;
	tempVal_f /= 100;

	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "V");

	showDiagnosLen++;

}

void analysis_sid0102_pid3A(char *data)
{
	int offset = 0;
	long long temp = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[3] = {0};

	//第一个数据
	if(o2sensorDefine == 0)
		strcpy(showDiagnosBuf[showDiagnosLen], "pid3A BANK2氧传感器3输出电压：");
	else
		strcpy(showDiagnosBuf[showDiagnosLen], "pid3A BANK4氧传感器1输出电压：");

	showDiagnosLen++;

	//第一个数据
	strcpy(showDiagnosBuf[showDiagnosLen], "[A]等效比：");

	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];

	tempVal *= 305;

	//整数
	tempVal_i = tempVal/10000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//小数部分
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%10000000;
	tempVal_f /= 1000;

	//四舍五入
	if((tempVal_f % 10) >= 5)
	{
		tempVal_f /= 10;
		tempVal_f += 1;
	}
	else
		tempVal_f /= 10;

	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	showDiagnosLen++;

	//第二个数据
		//第二个数据
	strcpy(showDiagnosBuf[showDiagnosLen], "[B]电流：");
	memset(Numbuf, '\0', 3);
	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];
	//正数 负数
	if(tempVal > 32768)
	{
		tempVal -= 32768;
	}
	else
	{
		strcat(showDiagnosBuf[showDiagnosLen], "-");
		tempVal = 32768 - tempVal;
	}

	temp = tempVal * 390625;

	//整数
	tempVal_i = temp/100000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], ".");

	//小数部分
	memset(Numbuf, '\0', 3);
	tempVal_f = temp%100000000;
	tempVal_f /= 100;

	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "V");

	showDiagnosLen++;

}

void analysis_sid0102_pid3B(char *data)
{
	int offset = 0;
	long long temp = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[3] = {0};

	//第一个数据
	if(o2sensorDefine == 0)
		strcpy(showDiagnosBuf[showDiagnosLen], "pid3B BANK2氧传感器4输出电压：");
	else
		strcpy(showDiagnosBuf[showDiagnosLen], "pid3B BANK4氧传感器2输出电压：");

	showDiagnosLen++;

	//第一个数据
	strcpy(showDiagnosBuf[showDiagnosLen], "[A]等效比：");

	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];

	tempVal *= 305;

	//整数
	tempVal_i = tempVal/10000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//小数部分
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%10000000;
	tempVal_f /= 1000;

	//四舍五入
	if((tempVal_f % 10) >= 5)
	{
		tempVal_f /= 10;
		tempVal_f += 1;
	}
	else
		tempVal_f /= 10;

	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	showDiagnosLen++;

	//第二个数据
		//第二个数据
	strcpy(showDiagnosBuf[showDiagnosLen], "[B]电流：");
	memset(Numbuf, '\0', 3);
	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];
	//正数 负数
	if(tempVal > 32768)
	{
		tempVal -= 32768;
	}
	else
	{
		strcat(showDiagnosBuf[showDiagnosLen], "-");
		tempVal = 32768 - tempVal;
	}

	temp = tempVal * 390625;

	//整数
	tempVal_i = temp/100000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], ".");

	//小数部分
	memset(Numbuf, '\0', 3);
	tempVal_f = temp%100000000;
	tempVal_f /= 100;

	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "V");

	showDiagnosLen++;

}

void analysis_sid0102_pid3C(char *data)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;

	char Numbuf[5] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "pid3C [AB]传感器1 催化剂温度组1：");
	//第一个数据
	tempVal = data[offset];
	tempVal <<= 8;
	tempVal |= data[offset+1];

	//负数
	if(tempVal < 400)
	{
		strcat(showDiagnosBuf[showDiagnosLen], "-");
		tempVal = 400-tempVal;
	}
	else
	{
		tempVal -= 400;
	}

	//整数部分

	tempVal_i = tempVal/10;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf, 0, 5);
	//小数部分
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%10;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "℃");

	showDiagnosLen++;
}

void analysis_sid0102_pid3D(char *data)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;

	char Numbuf[5] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "pid3D [AB]传感器1 催化剂温度组2：");
	//第一个数据
	tempVal = data[offset];
	tempVal <<= 8;
	tempVal |= data[offset+1];

	//负数
	if(tempVal < 400)
	{
		strcat(showDiagnosBuf[showDiagnosLen], "-");
		tempVal = 400-tempVal;
	}
	else
	{
		tempVal -= 400;
	}

	//整数部分

	tempVal_i = tempVal/10;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf, 0, 5);
	//小数部分
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%10;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "℃");

	showDiagnosLen++;
}

void analysis_sid0102_pid3E(char *data)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;

	char Numbuf[5] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "pid3E [AB]传感器2 催化剂温度组1：");
	//第一个数据
	tempVal = data[offset];
	tempVal <<= 8;
	tempVal |= data[offset+1];

	//负数
	if(tempVal < 400)
	{
		strcat(showDiagnosBuf[showDiagnosLen], "-");
		tempVal = 400-tempVal;
	}
	else
	{
		tempVal -= 400;
	}

	//整数部分

	tempVal_i = tempVal/10;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf, 0, 5);
	//小数部分
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%10;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "℃");

	showDiagnosLen++;
}

void analysis_sid0102_pid3F(char *data)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;

	char Numbuf[5] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "pid3F [AB]传感器2 催化剂温度组2：");
	//第一个数据
	tempVal = data[offset];
	tempVal <<= 8;
	tempVal |= data[offset+1];

	//负数
	if(tempVal < 400)
	{
		strcat(showDiagnosBuf[showDiagnosLen], "-");
		tempVal = 400-tempVal;
	}
	else
	{
		tempVal -= 400;
	}

	//整数部分

	tempVal_i = tempVal/10;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf, 0, 5);
	//小数部分
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%10;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "℃");

	showDiagnosLen++;
}

void analysis_sid0102_pid41(char *data)
{
	int offset = 0;

	//DATAB
	offset++;	//下一个数据

	if(data[offset]&0x01)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid41 [Bbit0]启用失火监测");
		if(data[offset]&0x10)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "：[Bbit4]监测未完成");
		}
		else
		{
			strcat(showDiagnosBuf[showDiagnosLen], "：[Bbit4]监测完成");
		}
	}
	else
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid41 [Bbit0]禁用失火监测");
	}
	showDiagnosLen++;

	if(data[offset]&0x02)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid41 [Bbit1]启用燃油系统监测");
		if(data[offset]&0x20)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "：[Bbit5]监测未完成");
		}
		else
		{
			strcat(showDiagnosBuf[showDiagnosLen], "：[Bbit5]监测完成");
		}
	}
	else
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid41 [Bbit1]禁用燃油系统监测");
	}
	showDiagnosLen++;

	if(data[offset]&0x04)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid41 [Bbit2]启用全面组件监测");
		if(data[offset]&0x40)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "：[Bbit6]监测未完成");
		}
		else
		{
			strcat(showDiagnosBuf[showDiagnosLen], "：[Bbit6]监测完成");
		}
	}
	else
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid41 [Bbit2]禁用全面组件监测");
	}
	showDiagnosLen++;

	//DATAC
	offset++;	//下一个数据
	if(data[offset]&0x01)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid41 [Cbit0]启用催化剂监测");
		if(data[offset+1]&0x01)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "：[Dbit0]监测未完成");
		}
		else
		{
			strcat(showDiagnosBuf[showDiagnosLen], "：[Dbit0]监测完成");
		}
	}
	else
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid41 [Cbit0]禁用催化剂监测");
	}
	showDiagnosLen++;

	if(data[offset]&0x02)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid41 [Cbit1]启用加热催化剂监测");
		if(data[offset+1]&0x02)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "：[Dbit1]监测未完成");
		}
		else
		{
			strcat(showDiagnosBuf[showDiagnosLen], "：[Dbit1]监测完成");
		}
	}
	else
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid41 [Cbit1]禁用加热催化剂监测");
	}
	showDiagnosLen++;

	if(data[offset]&0x04)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid41 [Cbit2]启用蒸发系统监测");
		if(data[offset+1]&0x04)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "：[Dbit2]监测未完成");
		}
		else
		{
			strcat(showDiagnosBuf[showDiagnosLen], "：[Dbit2]监测完成");
		}
	}
	else
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid41 [Cbit2]禁用蒸发系统监测");
	}
	showDiagnosLen++;

	if(data[offset]&0x08)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid41 [Cbit3]启用二次空气系统监测");
		if(data[offset+1]&0x08)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "：[Dbit3]监测未完成");
		}
		else
		{
			strcat(showDiagnosBuf[showDiagnosLen], "：[Dbit3]监测完成");
		}
	}
	else
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid41 [Cbit3]禁用二次空气系统监测");
	}
	showDiagnosLen++;

	if(data[offset]&0x10)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid41 [Cbit4]启用空调系统制冷剂监测");
		if(data[offset+1]&0x10)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "：[Dbit4]监测未完成");
		}
		else
		{
			strcat(showDiagnosBuf[showDiagnosLen], "：[Dbit4]监测完成");
		}
	}
	else
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid41 [Cbit4]禁用空调系统制冷剂监测");
	}
	showDiagnosLen++;

	if(data[offset]&0x20)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid41 [Cbit5]启用氧传感器监测");
		if(data[offset+1]&0x20)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "：[Dbit5]监测未完成");
		}
		else
		{
			strcat(showDiagnosBuf[showDiagnosLen], "：[Dbit5]监测完成");
		}
	}
	else
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid41 [Cbit5]禁用氧传感器监测");
	}
	showDiagnosLen++;

	if(data[offset]&0x40)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid41 [Cbit6]启用氧传感加热器监测");
		if(data[offset+1]&0x40)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "：[Dbit6]监测未完成");
		}
		else
		{
			strcat(showDiagnosBuf[showDiagnosLen], "：[Dbit6]监测完成");
		}
	}
	else
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid41 [Cbit6]禁用氧传感加热器监测");
	}
	showDiagnosLen++;

	if(data[offset]&0x80)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid41 [Cbit7]启用EGR系统监测");

		if(data[offset+1]&0x80)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "：[Dbit7]监测未完成");
		}
		else
		{
			strcat(showDiagnosBuf[showDiagnosLen], "：[Dbit7]监测完成");
		}
	}
	else
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid41 [Cbit7]禁用EGR系统监测");
	}
	showDiagnosLen++;
}

void analysis_sid0102_pid42(char *data)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[3] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "pid42 [AB]电压控制模块：");
	//第一个数据
	tempVal = data[offset];
	tempVal <<= 8;
	tempVal |= data[offset+1];

	//整数部分
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf, 0, 3);
	//小数部分
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

	strcat(showDiagnosBuf[showDiagnosLen], "V");
	showDiagnosLen++;
}

void analysis_sid0102_pid43(char *data)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[5] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "pid43 [AB]绝对的负荷值:");
	//第一个数据
	tempVal = data[offset];
	tempVal <<= 8;
	tempVal |= data[offset+1];

	tempVal *= 10000;
	tempVal /= 255;

	//整数部分
	tempVal_i = tempVal/100;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf, 0, 5);
	//小数部分
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%100;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

	strcat(showDiagnosBuf[showDiagnosLen], "%");
	showDiagnosLen++;
}

void analysis_sid0102_pid44(char *data)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[3] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "pid44 [AB]命令等效比：");

	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];

	tempVal *= 305;

	//整数
	tempVal_i = tempVal/10000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//小数部分
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%10000000;
	tempVal_f /= 1000;

	//四舍五入
	if((tempVal_f % 10) >= 5)
	{
		tempVal_f /= 10;
		tempVal_f += 1;
	}
	else
		tempVal_f /= 10;


	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	showDiagnosLen++;
}

void analysis_sid0102_pid45(char *data)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[3] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "pid45 [A]相对节气门位置：");
	//第一个数据
	tempVal = data[offset];
	tempVal *= 1000;
	tempVal /= 255;

	//整数
	tempVal_i = tempVal/10;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//小数部分
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%10;
	memset(Numbuf, '\0', 3);
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "%");

	showDiagnosLen++;
}

void analysis_sid0102_pid46(char *data)
{
	int offset = 0;
	int tempVal = 0;
	char Numbuf[3] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "pid46 [A]环境空气温度：");
	//第一个数据
	tempVal = data[offset];
	//负数
	if(tempVal < 40)
	{
		strcat(showDiagnosBuf[showDiagnosLen], "-");
		tempVal = 40-tempVal;
	}
	else
	{
		tempVal -= 40;
	}

	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "℃");

	showDiagnosLen++;
}

void analysis_sid0102_pid47(char *data)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[3] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "pid47 [A]绝对气流阀位置B：");
	//第一个数据
	tempVal = data[offset];
	tempVal *= 1000;
	tempVal /= 255;

	//整数
	tempVal_i = tempVal/10;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//小数部分
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%10;
	memset(Numbuf, '\0', 3);
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "%");

	showDiagnosLen++;
}

void analysis_sid0102_pid48(char *data)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[3] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "pid48 [A]绝对气流阀位置C：");
	//第一个数据
	tempVal = data[offset];
	tempVal *= 1000;
	tempVal /= 255;

	//整数
	tempVal_i = tempVal/10;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//小数部分
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%10;
	memset(Numbuf, '\0', 3);
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "%");

	showDiagnosLen++;
}

void analysis_sid0102_pid49(char *data)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[3] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "pid49 [A]绝对气流阀位置D：");
	//第一个数据
	tempVal = data[offset];
	tempVal *= 1000;
	tempVal /= 255;

	//整数
	tempVal_i = tempVal/10;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//小数部分
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%10;
	memset(Numbuf, '\0', 3);
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "%");

	showDiagnosLen++;
}

void analysis_sid0102_pid4A(char *data)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[3] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "pid4A [A]绝对气流阀位置E：");
	//第一个数据
	tempVal = data[offset];
	tempVal *= 1000;
	tempVal /= 255;

	//整数
	tempVal_i = tempVal/10;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//小数部分
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%10;
	memset(Numbuf, '\0', 3);
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "%");

	showDiagnosLen++;
}

void analysis_sid0102_pid4B(char *data)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[3] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "pid4B [A]绝对气流阀位置F：");
	//第一个数据
	tempVal = data[offset];
	tempVal *= 1000;
	tempVal /= 255;

	//整数
	tempVal_i = tempVal/10;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//小数部分
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%10;
	memset(Numbuf, '\0', 3);
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "%");

	showDiagnosLen++;
}

void analysis_sid0102_pid4C(char *data)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[3] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "pid4C [A]通讯节流阀执行机构控制：");
	//第一个数据
	tempVal = data[offset];
	tempVal *= 1000;
	tempVal /= 255;

	//整数
	tempVal_i = tempVal/10;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//小数部分
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%10;
	memset(Numbuf, '\0', 3);
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "%");

	showDiagnosLen++;
}

void analysis_sid0102_pid4D(char *data)
{
	int offset = 0;
	int tempVal = 0;
	char Numbuf[5] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "pid4D [AB]启动MIL时引擎运行的时间：");
	//第一个数据
	tempVal = data[offset];
	tempVal <<= 8;
	tempVal |= data[offset+1];//bit

	//整数部分
	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "min");

	showDiagnosLen++;
}

void analysis_sid0102_pid4E(char *data)
{
	int offset = 0;
	int tempVal = 0;
	char Numbuf[5] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "pid4E [AB]诊断故障代码清除以来的时间：");
	//第一个数据
	tempVal = data[offset];
	tempVal <<= 8;
	tempVal |= data[offset+1];//bit

	//整数部分
	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "min");

	showDiagnosLen++;
}

void analysis_sid0102_pid4F(char *data)
{
	int offset = 0;
	int tempVal = 0;
	char Numbuf[4] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "pid4F [A]等值比的最大值：");
	//第一个数据
	tempVal = data[offset++];//bit

	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	showDiagnosLen++;
	memset(Numbuf,0,4);

	strcpy(showDiagnosBuf[showDiagnosLen], "pid4F [B]氧传感器电压最大值：");
	//第二个数据
	tempVal = data[offset++];//bit

	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "V");
	showDiagnosLen++;

	memset(Numbuf,0,4);
	strcpy(showDiagnosBuf[showDiagnosLen], "pid4F [C]氧传感器电流最大值：");
	//第三个数据
	tempVal = data[offset++];//bit

	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "mA");
	showDiagnosLen++;

	memset(Numbuf,0,4);
	strcpy(showDiagnosBuf[showDiagnosLen], "pid4F [D]进气歧管绝对压力的最大值：");
	//第四个数据
	tempVal = data[offset++];//bit
	tempVal *= 10;

	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "kpa");
	showDiagnosLen++;
}

void analysis_sid0102_pid50(char *data)
{
	int offset = 0;
	int tempVal = 0;
	char Numbuf[4] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "pid50 [A]空气流量最大值质量空气流量的速率传感器：");

	tempVal = data[offset++];//bit
	tempVal *= 10;

	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "g/s");
	showDiagnosLen++;
}


void analysis_sid0102_pid51(char *data)
{
	int offset = 0;
	int tempVal = 0;

	strcpy(showDiagnosBuf[showDiagnosLen], "pid51 [A]车辆目前使用的燃料类型：");
	//第一个数据
	tempVal = data[offset];

	switch(tempVal)
	{
		case 0x01:
			strcat(showDiagnosBuf[showDiagnosLen], "汽油");
			break;
		case 0x02:
			strcat(showDiagnosBuf[showDiagnosLen], "甲醛");
			break;
		case 0x03:
			strcat(showDiagnosBuf[showDiagnosLen], "乙醇");
			break;
		case 0x04:
			strcat(showDiagnosBuf[showDiagnosLen], "柴油");
			break;
		case 0x05:
			strcat(showDiagnosBuf[showDiagnosLen], "石油气");
			break;
		case 0x06:
			strcat(showDiagnosBuf[showDiagnosLen], "压缩天然气");
			break;
		case 0x07:
			strcat(showDiagnosBuf[showDiagnosLen], "丙烷");
			break;
		case 0x08:
			strcat(showDiagnosBuf[showDiagnosLen], "电池");
			break;
		case 0x09:
			strcat(showDiagnosBuf[showDiagnosLen], "使用汽油的双燃料车辆");
			break;
		case 0x0a:
			strcat(showDiagnosBuf[showDiagnosLen], "使用甲醛的双燃料车辆");
			break;
		case 0x0b:
			strcat(showDiagnosBuf[showDiagnosLen], "使用乙醇的双燃料车辆");
			break;
		case 0x0c:
			strcat(showDiagnosBuf[showDiagnosLen], "使用石油气的双燃料车辆");
			break;
		case 0x0d:
			strcat(showDiagnosBuf[showDiagnosLen], "使用压缩天然气的双燃料车辆");
			break;
		case 0x0e:
			strcat(showDiagnosBuf[showDiagnosLen], "使用丙烷的双燃料车辆");
			break;
		case 0x0f:
			strcat(showDiagnosBuf[showDiagnosLen], "使用电池的双燃料车辆");
			break;
		default:
			strcat(showDiagnosBuf[showDiagnosLen], "ISO/SAE Reserved");
			break;
	}

	showDiagnosLen++;
}

void analysis_sid0102_pid52(char *data)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[3] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "pid52 [A]酒精燃料比例：");
	//第一个数据
	tempVal = data[offset];
	tempVal *= 1000;
	tempVal /= 255;

	//整数
	tempVal_i = tempVal/10;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//小数部分
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%10;
	memset(Numbuf, '\0', 3);
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "%");

	showDiagnosLen++;
}

void analysis_sid0102_pid53(char *data)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[3] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "pid53 [AB]绝对Evap系统蒸汽压力：");
	//第一个数据
	tempVal = data[offset];
	tempVal <<= 8;
	tempVal |= data[offset+1];

	tempVal *= 10;
	tempVal /= 2;

	//整数部分
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf, 0, 3);
	//小数部分
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

	strcat(showDiagnosBuf[showDiagnosLen], "kpa");
	showDiagnosLen++;
}

void analysis_sid0102_pid54(char *data)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
//	int tempVal_f = 0;
	char Numbuf[5] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "pid54 [AB]Evap系统蒸汽压力：");
	//第一个数据
	tempVal = data[offset];
	tempVal <<= 8;
	tempVal |= data[offset+1];

	if(tempVal > 32767)
	{
		tempVal -= 32767;
	}
	else
	{
		tempVal = 32767-tempVal;
		strcat(showDiagnosBuf[showDiagnosLen], "-");
	}

	//整数部分
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "kpa");
	showDiagnosLen++;
}

void analysis_sid0102_pid55(char *data)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[3] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "pid55 [A]短时间内二级氧传感器燃料修整器BANK1：");
	//第一个数据
	tempVal = data[offset];//bit
	//是否负数
	if(tempVal < 128)
	{
		strcat(showDiagnosBuf[showDiagnosLen], "-");
		tempVal = 128 - tempVal;
	}
	else
	{
		tempVal -= 128;
	}

	tempVal *= 10000;//放大10000倍
	tempVal /= 128;

	//整数部分
	tempVal_i = tempVal/100;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);
	//小数部分
	tempVal_f = tempVal%100;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "%");
	showDiagnosLen++;

	strcpy(showDiagnosBuf[showDiagnosLen], "pid55 [B]短时间内二级氧传感器燃料修整器BANK3：");
	//第2个数据
	tempVal = data[offset+1];//bit
	//是否负数
	if(tempVal < 128)
	{
		strcat(showDiagnosBuf[showDiagnosLen], "-");
		tempVal = 128 - tempVal;
	}
	else
	{
		tempVal -= 128;
	}

	tempVal *= 10000;//放大10000倍
	tempVal /= 128;

	//整数部分
	tempVal_i = tempVal/100;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);
	//小数部分
	tempVal_f = tempVal%100;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "%");
	showDiagnosLen++;
}

void analysis_sid0102_pid56(char *data)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[3] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "pid56 [A]长时间内二级氧传感器燃料修整器BANK1：");
	//第一个数据
	tempVal = data[offset];//bit
	//是否负数
	if(tempVal < 128)
	{
		strcat(showDiagnosBuf[showDiagnosLen], "-");
		tempVal = 128 - tempVal;
	}
	else
	{
		tempVal -= 128;
	}

	tempVal *= 10000;//放大10000倍
	tempVal /= 128;

	//整数部分
	tempVal_i = tempVal/100;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);
	//小数部分
	tempVal_f = tempVal%100;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "%");
	showDiagnosLen++;

	strcpy(showDiagnosBuf[showDiagnosLen], "pid56 [B]长时间内二级氧传感器燃料修整器BANK3：");
	//第2个数据
	tempVal = data[offset+1];//bit
	//是否负数
	if(tempVal < 128)
	{
		strcat(showDiagnosBuf[showDiagnosLen], "-");
		tempVal = 128 - tempVal;
	}
	else
	{
		tempVal -= 128;
	}

	tempVal *= 10000;//放大10000倍
	tempVal /= 128;

	//整数部分
	tempVal_i = tempVal/100;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);
	//小数部分
	tempVal_f = tempVal%100;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "%");
	showDiagnosLen++;
}

void analysis_sid0102_pid57(char *data)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[3] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "pid57 [A]短时间内二级氧传感器燃料修整器BANK2：");
	//第一个数据
	tempVal = data[offset];//bit
	//是否负数
	if(tempVal < 128)
	{
		strcat(showDiagnosBuf[showDiagnosLen], "-");
		tempVal = 128 - tempVal;
	}
	else
	{
		tempVal -= 128;
	}

	tempVal *= 10000;//放大10000倍
	tempVal /= 128;

	//整数部分
	tempVal_i = tempVal/100;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);
	//小数部分
	tempVal_f = tempVal%100;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "%");
	showDiagnosLen++;

	strcpy(showDiagnosBuf[showDiagnosLen], "pid57 [B]短时间内二级氧传感器燃料修整器BANK4：");
	//第2个数据
	tempVal = data[offset+1];//bit
	//是否负数
	if(tempVal < 128)
	{
		strcat(showDiagnosBuf[showDiagnosLen], "-");
		tempVal = 128 - tempVal;
	}
	else
	{
		tempVal -= 128;
	}

	tempVal *= 10000;//放大10000倍
	tempVal /= 128;

	//整数部分
	tempVal_i = tempVal/100;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);
	//小数部分
	tempVal_f = tempVal%100;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "%");
	showDiagnosLen++;
}

void analysis_sid0102_pid58(char *data)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[3] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "pid58 [A]长时间内二级氧传感器燃料修整器BANK2：");
	//第一个数据
	tempVal = data[offset];//bit
	//是否负数
	if(tempVal < 128)
	{
		strcat(showDiagnosBuf[showDiagnosLen], "-");
		tempVal = 128 - tempVal;
	}
	else
	{
		tempVal -= 128;
	}

	tempVal *= 10000;//放大10000倍
	tempVal /= 128;

	//整数部分
	tempVal_i = tempVal/100;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);
	//小数部分
	tempVal_f = tempVal%100;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "%");
	showDiagnosLen++;

	strcpy(showDiagnosBuf[showDiagnosLen], "pid58 [B]长时间内二级氧传感器燃料修整器BANK4：");
	//第2个数据
	tempVal = data[offset+1];//bit
	//是否负数
	if(tempVal < 128)
	{
		strcat(showDiagnosBuf[showDiagnosLen], "-");
		tempVal = 128 - tempVal;
	}
	else
	{
		tempVal -= 128;
	}

	tempVal *= 10000;//放大10000倍
	tempVal /= 128;

	//整数部分
	tempVal_i = tempVal/100;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);
	//小数部分
	tempVal_f = tempVal%100;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "%");
	showDiagnosLen++;
}

void analysis_sid0102_pid59(char *data)
{
	int offset = 0;
	int tempVal = 0;
	char Numbuf[6] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "pid59 [AB]燃料轨压力绝对：");
	//第一个数据
	tempVal = data[offset];
	tempVal <<= 8;
	tempVal |= data[offset+1];//bit

	tempVal *= 10;

	//整数部分
	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "kpa");

	showDiagnosLen++;
}

void analysis_sid0102_pid5A(char *data)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[3] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "pid5A [A]相对油门踏板：");
	//第一个数据
	tempVal = data[offset];
	tempVal *= 1000;
	tempVal /= 255;

	//整数
	tempVal_i = tempVal/10;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//小数部分
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%10;
	memset(Numbuf, '\0', 3);
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "%");

	showDiagnosLen++;
}



/** *************************************特殊定义************************************* **/
void analysis_sid0102_pidFD(char *data)
{
	int offset = 0;
	int tempVal = 0;

	char Numbuf[5] = {0};

	//第一个数据
	strcpy(showDiagnosBuf[showDiagnosLen], "ADP气流抵消");

	tempVal = data[offset];
	tempVal = (tempVal<<8) | data[offset+1];

	if(tempVal > 32768)
	{
		tempVal -= 32768;
	}
	else
	{
		strcat(showDiagnosBuf[showDiagnosLen], "-");
		tempVal = 32768 - tempVal;
	}

	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

	memset(Numbuf, '\0', 5);

	showDiagnosLen++;
}

void analysis_sid0102_pidFE(char *data)
{
	int offset = 0;
	int tempVal = 0;

	char Numbuf[5] = {0};

	//第一个数据
	strcpy(showDiagnosBuf[showDiagnosLen], "ADP燃料乘数");

	tempVal = data[offset];
	tempVal = (tempVal<<8) | data[offset+1];

	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

	memset(Numbuf, '\0', 5);

	showDiagnosLen++;
}

void analysis_sid0102_pidFF(char *data)
{
	int offset = 0;
	int tempVal = 0;

	char Numbuf[5] = {0};

	//第一个数据
	strcpy(showDiagnosBuf[showDiagnosLen], "ADP燃料抵消");

	tempVal = data[offset];
	tempVal = (tempVal<<8) | data[offset+1];

	if(tempVal > 32768)
	{
		tempVal -= 32768;
	}
	else
	{
		strcat(showDiagnosBuf[showDiagnosLen], "-");
		tempVal = 32768 - tempVal;
	}

	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

	memset(Numbuf, '\0', 5);

	showDiagnosLen++;
}
#pragma endregion

int uds_service_sid02(unsigned char *rxbuf, int *rxlen)
{
	int ret = 0;
	int i = 0;
	int txlen = 0;
	int checklen = 0;
	int checkbuf[2];

	sendFailedCnt = 0;
	do{
		txlen = 0;
		if(diagnose.commtypes == 0)
		{
			commbuf[txlen++] = diagnose.server_id;
			commbuf[txlen++] = diagnose.sub_function;
			checkbuf[checklen++] = diagnose.server_id + 0x40;
			checkbuf[checklen++] = diagnose.sub_function;
			//特殊PID
			if(diagnose.server_id == 0x02)
			{
				commbuf[txlen++] = 0x00;
				checkbuf[checklen++] = 0x00;
			}
			ret = UdsRequest1(diagnose.funAddr, 0, (unsigned char *)commbuf, txlen, rxbuf, (unsigned short *)rxlen);
			if(ret == 0)
			{
				for(i = 0; i < checklen; i++)
				{
					if(checkbuf[i] != rxbuf[i])
					{
						return 2;
					}
				}
			}
			else if(ret == 1)
			{
				if(sendFailedCnt++ >= 3)
				{
					return 1;//发生接收错误
				}
			}
			else
			{
				return ret;
			}
		}
		else
		{
//			txlen = kwpFmt((unsigned char *)commbuf, 2);//头部信息
			commbuf[txlen++] = 0x83;
			commbuf[txlen++] = 0x10;
			commbuf[txlen++] = 0xF1;
			commbuf[txlen++] = diagnose.server_id;
			commbuf[txlen++] = diagnose.sub_function;
			checkbuf[checklen++] = diagnose.server_id + 0x40;
			checkbuf[checklen++] = diagnose.sub_function;
			//特殊PID
			if(diagnose.server_id == 0x02)
			{
				commbuf[txlen++] = 0x00;
				checkbuf[checklen++] = 0x00;
			}
			ret = KwpRequest(commbuf, txlen, (char *)rxbuf, rxlen, NULL);
			if(ret == 0)
			{
				for(i = 0; i < checklen; i++)
				{
					if(checkbuf[i] != rxbuf[i])
					{
						return 2;
					}
				}
			}
			else if(ret == 1)
			{
				if(sendFailedCnt++ >= 3)
				{
					return 1;//发生接收错误
				}
			}
			else
			{
				return ret;
			}
		}
	}while(ret != 0);

	return ret;
}

int uds_service_sid03(unsigned char *rxbuf, int *rxlen)
{
	int ret = 0;
	int i = 0;
	int txlen = 0;
	char rxbuftmp[10] = {0};
	int rxlentmp = 0;
	int checklen = 0;
	int checkbuf[2];

	sendFailedCnt = 0;
	do{
		txlen = 0;
		if(diagnose.commtypes == 0)
		{
			commbuf[txlen++] = diagnose.server_id;
			checkbuf[checklen++] = diagnose.server_id + 0x40;
			ret = UdsRequest1(diagnose.funAddr, 0, (unsigned char *)commbuf, txlen, rxbuf, (unsigned short *)rxlen);
			if(ret == 0)
			{
				for(i = 0; i < checklen; i++)
				{
					if(checkbuf[i] != rxbuf[i])
					{
						return 2;
					}
				}
			}
			else if(ret == 1)
			{
				if(sendFailedCnt++ >= 3)
				{
					return 1;//发生接收错误
				}
			}
			else
			{
				return ret;
			}
		}
		else
		{
			commbuf[txlen++] = 0x81;
			commbuf[txlen++] = 0x10;
			commbuf[txlen++] = 0xF1;
			commbuf[txlen++] = diagnose.server_id;
			checkbuf[checklen++] = diagnose.server_id + 0x40;
			ret = KwpRequest(commbuf, txlen, (char *)rxbuf, rxlen, NULL);
			if(ret == 0)
			{
				for(i = 0; i < checklen; i++)
				{
					if(checkbuf[i] != rxbuf[i])
					{
						return 2;
					}
				}
				rxlentmp += *rxlen;
				while(ret == 0)
				{
					ret = KwpRx(USART1,(uint8_t *)rxbuftmp,(uint16_t *)rxlen, 200);	//normal P2
					if(ret == 0)
					{
						for(i = 0; i < checklen; i++)
						{
							if(checkbuf[i] != rxbuftmp[i])
							{
								return 2;
							}
						}
						
						for(i = 0; i < (*rxlen)-1; i++)
						{
							rxbuf[rxlentmp+i] = rxbuftmp[1+i];
						}
						rxlentmp += *rxlen-1;
					}
				}
				*rxlen = rxlentmp;
				return 0;
			}
			else if(ret == 1)
			{
				if(sendFailedCnt++ >= 3)
				{
					return 1;//发生接收错误
				}
			}
			else
			{
				return ret;
			}
		}
	}while(ret != 0);

	return ret;
}

int uds_service_sid04(unsigned char *rxbuf, int *rxlen)
{
	int ret = 0;
	int i = 0;
	int txlen = 0;
	int checklen = 0;
	int checkbuf[2];

	sendFailedCnt = 0;
	do{
		txlen = 0;
		if(diagnose.commtypes == 0)
		{
			commbuf[txlen++] = diagnose.server_id;
			checkbuf[checklen++] = diagnose.server_id + 0x40;
			ret = UdsRequest1(diagnose.funAddr, 0, (unsigned char *)commbuf, txlen, rxbuf, (uint16_t*)&rxlen);
			if(ret == 0)
			{
				for(i = 0; i < checklen; i++)
				{
					if(checkbuf[i] != rxbuf[i])
					{
						return 2;
					}
				}
			}
			else if(ret == 1)
			{
				if(sendFailedCnt++ >= 3)
				{
					return 1;//发生接收错误
				}
			}
			else
			{
				return ret;
			}
		}
		else
		{
//			txlen = kwpFmt((unsigned char *)commbuf, 1);//头部信息
			commbuf[txlen++] = 0x81;
			commbuf[txlen++] = 0x10;
			commbuf[txlen++] = 0xF1;
			commbuf[txlen++] = diagnose.server_id;
			checkbuf[checklen++] = diagnose.server_id + 0x40;
			ret = KwpRequest(commbuf, txlen, (char *)rxbuf, rxlen, NULL);
			if(ret == 0)
			{
				for(i = 0; i < checklen; i++)
				{
					if(checkbuf[i] != rxbuf[i])
					{
						return 2;
					}
				}
			}
			else if(ret == 1)
			{
				if(sendFailedCnt++ >= 3)
				{
					return 1;//发生接收错误
				}
			}
			else
			{
				return ret;
			}
		}
	}while(ret != 0);

	return ret;
}

int uds_service_sid05(unsigned char *rxbuf, int *rxlen)
{
	int ret = 0;
	int i = 0;
	int txlen = 0;
	int checklen = 0;
	int checkbuf[2];

	sendFailedCnt = 0;
	do{
		txlen = 0;
		if(diagnose.commtypes == 0)
		{
			commbuf[txlen++] = diagnose.server_id;
			commbuf[txlen++] = diagnose.sub_function;
			checkbuf[checklen++] = diagnose.server_id + 0x40;
			checkbuf[checklen++] = diagnose.sub_function;

			if(diagnose.sub_function == 0x05)
			{
				//氧传感器位置
				commbuf[txlen++] = 00;
				checkbuf[checklen++] = 00;
			}

			ret = UdsRequest1(diagnose.funAddr, 0, (unsigned char *)commbuf, txlen, rxbuf, (unsigned short *)rxlen);
			if(ret == 0)
			{
				for(i = 0; i < checklen; i++)
				{
					if(checkbuf[i] != rxbuf[i])
					{
						return 2;
					}
				}
			}
			else if(ret == 1)
			{
				if(sendFailedCnt++ >= 3)
				{
					return 1;//发生接收错误
				}
			}
			else
			{
				return ret;
			}
		}
		else
		{
//			txlen = kwpFmt((unsigned char *)commbuf, 2);//头部信息
			commbuf[txlen++] = 0x82;
			commbuf[txlen++] = 0x10;
			commbuf[txlen++] = 0xF1;
			commbuf[txlen++] = diagnose.server_id;
			commbuf[txlen++] = diagnose.sub_function;
			checkbuf[checklen++] = diagnose.server_id + 0x40;
			checkbuf[checklen++] = diagnose.sub_function;
			if(diagnose.sub_function == 0x05)
			{
				//氧传感器位置
				commbuf[txlen++] = 00;
				checkbuf[checklen++] = 00;
			}
			ret = KwpRequest(commbuf, txlen, (char *)rxbuf, rxlen, NULL);
			if(ret == 0)
			{
				for(i = 0; i < checklen; i++)
				{
					if(checkbuf[i] != rxbuf[i])
					{
						return 2;
					}
				}
			}
			else if(ret == 1)
			{
				if(sendFailedCnt++ >= 3)
				{
					return 1;//发生接收错误
				}
			}
			else
			{
				return ret;
			}
		}
	}while(ret != 0);

	return ret;
}

void analysis_sid05_tid01(char *data)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[3] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "从稀到浓传感器阈值电压：");
	showDiagnosLen++;

	strcpy(showDiagnosBuf[showDiagnosLen], "Test Value:");
	//第一个数据
	tempVal = data[offset++];//bit
	tempVal *= 5;
	//整数
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf,0,3);
	//小数
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "V");
	showDiagnosLen++;
	memset(Numbuf,0,3);
	
	strcpy(showDiagnosBuf[showDiagnosLen], "minimum Limit:");
	//第二个数据
	tempVal = data[offset++];//bit
	tempVal *= 5;
	//整数
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf,0,3);
	//小数
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "V");
	showDiagnosLen++;
	memset(Numbuf,0,3);
	
	strcpy(showDiagnosBuf[showDiagnosLen], "maximum Limit:");
	//第三个数据
	tempVal = data[offset++];//bit
	tempVal *= 5;
	//整数
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf,0,3);
	//小数
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "V");
	showDiagnosLen++;
	memset(Numbuf,0,3);
}

void analysis_sid05_tid02(char *data)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[3] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "从浓到稀传感器阈值电压：");
	showDiagnosLen++;

	strcpy(showDiagnosBuf[showDiagnosLen], "Test Value:");
	//第一个数据
	tempVal = data[offset++];//bit
	tempVal *= 5;
	//整数
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf,0,3);
	//小数
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "V");
	showDiagnosLen++;
	memset(Numbuf,0,3);
	
	strcpy(showDiagnosBuf[showDiagnosLen], "minimum Limit:");
	//第二个数据
	tempVal = data[offset++];//bit
	tempVal *= 5;
	//整数
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf,0,3);
	//小数
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "V");
	showDiagnosLen++;
	memset(Numbuf,0,3);
	
	strcpy(showDiagnosBuf[showDiagnosLen], "maximum Limit:");
	//第三个数据
	tempVal = data[offset++];//bit
	tempVal *= 5;
	//整数
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf,0,3);
	//小数
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "V");
	showDiagnosLen++;
	memset(Numbuf,0,3);
}

void analysis_sid05_tid03(char *data)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[3] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "进行切换时间计算的传感器低电压：");
	showDiagnosLen++;

	strcpy(showDiagnosBuf[showDiagnosLen], "Test Value:");
	//第一个数据
	tempVal = data[offset++];//bit
	tempVal *= 5;
	//整数
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf,0,3);
	//小数
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "V");
	showDiagnosLen++;
	memset(Numbuf,0,3);
	
	strcpy(showDiagnosBuf[showDiagnosLen], "minimum Limit:");
	//第二个数据
	tempVal = data[offset++];//bit
	tempVal *= 5;
	//整数
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf,0,3);
	//小数
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "V");
	showDiagnosLen++;
	memset(Numbuf,0,3);
	
	strcpy(showDiagnosBuf[showDiagnosLen], "maximum Limit:");
	//第三个数据
	tempVal = data[offset++];//bit
	tempVal *= 5;
	//整数
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf,0,3);
	//小数
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "V");
	showDiagnosLen++;
}

void analysis_sid05_tid04(char *data)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[3] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "进行切换时间计算的传感器高电压：");
	showDiagnosLen++;

	strcpy(showDiagnosBuf[showDiagnosLen], "Test Value:");
	//第一个数据
	tempVal = data[offset++];//bit
	tempVal *= 5;
	//整数
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf,0,3);
	//小数
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "V");
	showDiagnosLen++;
	memset(Numbuf,0,3);
	
	strcpy(showDiagnosBuf[showDiagnosLen], "minimum Limit:");
	//第二个数据
	tempVal = data[offset++];//bit
	tempVal *= 5;
	//整数
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf,0,3);
	//小数
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "V");
	showDiagnosLen++;
	memset(Numbuf,0,3);
	
	strcpy(showDiagnosBuf[showDiagnosLen], "maximum Limit:");
	//第三个数据
	tempVal = data[offset++];//bit
	tempVal *= 5;
	//整数
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf,0,3);
	//小数
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "V");
	showDiagnosLen++;
}

void analysis_sid05_tid05(char *data)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[3] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "从浓到稀传感器的切换时间：");
	showDiagnosLen++;
	
	strcpy(showDiagnosBuf[showDiagnosLen], "Test Value:");
	//第一个数据
	tempVal = data[offset++];//bit
	tempVal *= 4;
	//整数
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf,0,3);
	//小数
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "s");
	showDiagnosLen++;
	memset(Numbuf,0,3);
	
	strcpy(showDiagnosBuf[showDiagnosLen], "minimum Limit:");
	//第二个数据
	tempVal = data[offset++];//bit
	tempVal *= 4;
	//整数
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf,0,3);
	//小数
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "s");
	showDiagnosLen++;
	memset(Numbuf,0,3);
	
	strcpy(showDiagnosBuf[showDiagnosLen], "maximum Limit:");
	//第三个数据
	tempVal = data[offset++];//bit
	tempVal *= 4;
	//整数
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf,0,3);
	//小数
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "s");
	showDiagnosLen++;
}

void analysis_sid05_tid06(char *data)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[3] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "从稀到浓传感器的切换时间：");
	showDiagnosLen++;
	
	strcpy(showDiagnosBuf[showDiagnosLen], "Test Value:");
	//第一个数据
	tempVal = data[offset++];//bit
	tempVal *= 4;
	//整数
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf,0,3);
	//小数
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "s");
	showDiagnosLen++;
	memset(Numbuf,0,3);
	
	strcpy(showDiagnosBuf[showDiagnosLen], "minimum Limit:");
	//第二个数据
	tempVal = data[offset++];//bit
	tempVal *= 4;
	//整数
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf,0,3);
	//小数
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "s");
	showDiagnosLen++;
	memset(Numbuf,0,3);
	
	strcpy(showDiagnosBuf[showDiagnosLen], "maximum Limit:");
	//第三个数据
	tempVal = data[offset++];//bit
	tempVal *= 4;
	//整数
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf,0,3);
	//小数
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "s");
	showDiagnosLen++;
}

void analysis_sid05_tid07(char *data)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[3] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "测试循环中的最小传感器电压：");
	showDiagnosLen++;

	strcpy(showDiagnosBuf[showDiagnosLen], "Test Value:");
	//第一个数据
	tempVal = data[offset++];//bit
	tempVal *= 5;
	//整数
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf,0,3);
	//小数
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "V");
	showDiagnosLen++;
	memset(Numbuf,0,3);
	
	strcpy(showDiagnosBuf[showDiagnosLen], "minimum Limit:");
	//第二个数据
	tempVal = data[offset++];//bit
	tempVal *= 5;
	//整数
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf,0,3);
	//小数
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "V");
	showDiagnosLen++;
	memset(Numbuf,0,3);
	
	strcpy(showDiagnosBuf[showDiagnosLen], "maximum Limit:");
	//第三个数据
	tempVal = data[offset++];//bit
	tempVal *= 5;
	//整数
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf,0,3);
	//小数
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "V");
	showDiagnosLen++;
	memset(Numbuf,0,3);
}

void analysis_sid05_tid08(char *data)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[3] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "测试循环中的最大传感器电压：");
	showDiagnosLen++;

	strcpy(showDiagnosBuf[showDiagnosLen], "Test Value:");
	//第一个数据
	tempVal = data[offset++];//bit
	tempVal *= 5;
	//整数
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf,0,3);
	//小数
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "V");
	showDiagnosLen++;
	memset(Numbuf,0,3);
	
	strcpy(showDiagnosBuf[showDiagnosLen], "minimum Limit:");
	//第二个数据
	tempVal = data[offset++];//bit
	tempVal *= 5;
	//整数
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf,0,3);
	//小数
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "V");
	showDiagnosLen++;
	memset(Numbuf,0,3);
	
	strcpy(showDiagnosBuf[showDiagnosLen], "maximum Limit:");
	//第三个数据
	tempVal = data[offset++];//bit
	tempVal *= 5;
	//整数
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf,0,3);
	//小数
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "V");
	showDiagnosLen++;
	memset(Numbuf,0,3);
}

void analysis_sid05_tid09(char *data)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[3] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "传感器的切换时间：");
	showDiagnosLen++;
	
	strcpy(showDiagnosBuf[showDiagnosLen], "Test Value:");
	//第一个数据
	tempVal = data[offset++];//bit
	tempVal *= 4;
	//整数
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf,0,3);
	//小数
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "s");
	showDiagnosLen++;
	memset(Numbuf,0,3);
	
	strcpy(showDiagnosBuf[showDiagnosLen], "minimum Limit:");
	//第二个数据
	tempVal = data[offset++];//bit
	tempVal *= 4;
	//整数
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf,0,3);
	//小数
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "s");
	showDiagnosLen++;
	memset(Numbuf,0,3);
	
	strcpy(showDiagnosBuf[showDiagnosLen], "maximum Limit:");
	//第三个数据
	tempVal = data[offset++];//bit
	tempVal *= 4;
	//整数
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf,0,3);
	//小数
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "s");
	showDiagnosLen++;
	memset(Numbuf,0,3);
}

void analysis_sid05_tid0A(char *data)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[3] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "传感器的周期：");
	showDiagnosLen++;
	
	strcpy(showDiagnosBuf[showDiagnosLen], "Test Value:");
	//第一个数据
	tempVal = data[offset++];//bit
	tempVal *= 4;
	//整数
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf,0,3);
	//小数
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "s");
	showDiagnosLen++;
	memset(Numbuf,0,3);
	
	strcpy(showDiagnosBuf[showDiagnosLen], "minimum Limit:");
	//第二个数据
	tempVal = data[offset++];//bit
	tempVal *= 4;
	//整数
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf,0,3);
	//小数
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "s");
	showDiagnosLen++;
	memset(Numbuf,0,3);
	
	strcpy(showDiagnosBuf[showDiagnosLen], "maximum Limit:");
	//第三个数据
	tempVal = data[offset++];//bit
	tempVal *= 4;
	//整数
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf,0,3);
	//小数
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "s");
	showDiagnosLen++;
	memset(Numbuf,0,3);
}


int uds_service_sid06(unsigned char *rxbuf, int *rxlen)
{
	int ret = 0;
	int i = 0;
	int txlen = 0;
	int checklen = 0;
	int checkbuf[2];

	sendFailedCnt = 0;
	do{
		txlen = 0;
		if(diagnose.commtypes == 0)
		{
			commbuf[txlen++] = diagnose.server_id;
			commbuf[txlen++] = diagnose.sub_function;
			checkbuf[checklen++] = diagnose.server_id + 0x40;
			checkbuf[checklen++] = diagnose.sub_function;
			ret = UdsRequest1(diagnose.funAddr, 0, (unsigned char *)commbuf, txlen, rxbuf, (unsigned short *)rxlen);
			if(ret == 0)
			{
				for(i = 0; i < checklen; i++)
				{
					if(checkbuf[i] != rxbuf[i])
					{
						return 2;
					}
				}
			}
			else if(ret == 1)
			{
				if(sendFailedCnt++ >= 3)
				{
					return 1;//发生接收错误
				}
			}
			else
			{
				return ret;
			}
		}
		else
		{
			commbuf[txlen++] = 0x82;
			commbuf[txlen++] = 0x10;
			commbuf[txlen++] = 0xF1;
			commbuf[txlen++] = diagnose.server_id;
			commbuf[txlen++] = diagnose.sub_function;
			checkbuf[checklen++] = diagnose.server_id + 0x40;
			checkbuf[checklen++] = diagnose.sub_function;
			ret = KwpRequest(commbuf, txlen, (char *)rxbuf, rxlen, NULL);
			if(ret == 0)
			{
				for(i = 0; i < checklen; i++)
				{
					if(checkbuf[i] != rxbuf[i])
					{
						return 2;
					}
				}
			}
			else if(ret == 1)
			{
				if(sendFailedCnt++ >= 3)
				{
					return 1;//发生接收错误
				}
			}
			else
			{
				return ret;
			}
		}
	}while(ret != 0);

	return ret;
}

void analysis_sid06_forKLine(char *data)
{
	int offset = 0;
	int tempTid = 0;
	int tempVal = 0;
	int tempType = 0;
	int tempLimitVal = 0;
	char Numbuf[5] = {0};

	tempTid = data[offset++];
	tempType = data[offset++];

	tempVal = data[offset++];
	tempVal <<= 8;
	tempVal |= data[offset++];

	tempLimitVal = data[offset++];
	tempLimitVal <<= 8;
	tempLimitVal |= data[offset++];

	switch(tempTid)
	{
		case 0x01:
			strcpy(showDiagnosBuf[showDiagnosLen], "从稀到浓传感器阈值电压测试值：");
			IntToSysChar(tempVal, Numbuf, 10);
			strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
			strcat(showDiagnosBuf[showDiagnosLen], "V");
			showDiagnosLen++;
			break;
		case 0x02:
			strcpy(showDiagnosBuf[showDiagnosLen], "从浓到稀传感器阈值电压测试值：");
			IntToSysChar(tempVal, Numbuf, 10);
			strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
			strcat(showDiagnosBuf[showDiagnosLen], "V");
			showDiagnosLen++;
			break;
		case 0x03:
			strcpy(showDiagnosBuf[showDiagnosLen], "进行切换时间计算的传感器低电压：");
			IntToSysChar(tempVal, Numbuf, 10);
			strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
			strcat(showDiagnosBuf[showDiagnosLen], "V");
			showDiagnosLen++;

			break;
		case 0x04:
			strcpy(showDiagnosBuf[showDiagnosLen], "进行切换时间计算的传感器高电压：");
			IntToSysChar(tempVal, Numbuf, 10);
			strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
			strcat(showDiagnosBuf[showDiagnosLen], "V");
			showDiagnosLen++;
			break;
		case 0x05:
			strcpy(showDiagnosBuf[showDiagnosLen], "从浓到稀传感器的切换时间：");
			IntToSysChar(tempVal, Numbuf, 10);
			strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
			strcat(showDiagnosBuf[showDiagnosLen], "s");
			showDiagnosLen++;
			break;
		case 0x06:
			strcpy(showDiagnosBuf[showDiagnosLen], "从稀到浓传感器的切换时间：");
			IntToSysChar(tempVal, Numbuf, 10);
			strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
			strcat(showDiagnosBuf[showDiagnosLen], "s");
			showDiagnosLen++;
			break;
		case 0x07:
			strcpy(showDiagnosBuf[showDiagnosLen], "测试循环中的最小传感器电压：");
			IntToSysChar(tempVal, Numbuf, 10);
			strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
			strcat(showDiagnosBuf[showDiagnosLen], "V");
			showDiagnosLen++;
			break;
		case 0x08:
			strcpy(showDiagnosBuf[showDiagnosLen], "测试循环中的最大传感器电压：");
			IntToSysChar(tempVal, Numbuf, 10);
			strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
			strcat(showDiagnosBuf[showDiagnosLen], "V");
			showDiagnosLen++;
			break;
		case 0x09:
			strcpy(showDiagnosBuf[showDiagnosLen], "传感器的切换时间：");
			IntToSysChar(tempVal, Numbuf, 10);
			strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
			strcat(showDiagnosBuf[showDiagnosLen], "s");
			showDiagnosLen++;
			break;
		case 0x0A:
			strcpy(showDiagnosBuf[showDiagnosLen], "传感器周期：");
			IntToSysChar(tempVal, Numbuf, 10);
			strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
			strcat(showDiagnosBuf[showDiagnosLen], "s");
			showDiagnosLen++;
			break;
		default:
			break;
	}

	//部件
	strcpy(showDiagnosBuf[showDiagnosLen], "测试部件ID：");

	IntToSysChar((tempType&0x7F), Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	showDiagnosLen++;
	memset(Numbuf, 0, 5);
	
	strcpy(showDiagnosBuf[showDiagnosLen], "测试值：");
	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	showDiagnosLen++;
	memset(Numbuf, 0, 5);
	//测试的值最大/最小
	if(tempType & 0x80)
	{//最小
		
		strcpy(showDiagnosBuf[showDiagnosLen], "最小极限值：");
		IntToSysChar(tempLimitVal, Numbuf, 10);
		strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
		showDiagnosLen++;
		memset(Numbuf, 0, 5);
		
		if(tempVal >= tempLimitVal)
			strcpy(showDiagnosBuf[showDiagnosLen], "测试成功");
		else
			strcpy(showDiagnosBuf[showDiagnosLen], "测试失败");
		showDiagnosLen++;
	}
	else
	{//最大
		strcpy(showDiagnosBuf[showDiagnosLen], "最大极限值：");
		IntToSysChar(tempLimitVal, Numbuf, 10);
		strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
		showDiagnosLen++;
		memset(Numbuf, 0, 5);

		if(tempVal <= tempLimitVal)
			strcpy(showDiagnosBuf[showDiagnosLen], "测试成功");
		else
			strcpy(showDiagnosBuf[showDiagnosLen], "测试失败");
		showDiagnosLen++;
	}

}

void analysis_sid06_OBDMID(char *data, int len)
{
	int offset = 0;
	int tempOBDMID = 0;
	int testID = 0;
	int unitScalID = 0;

	long long temp = 0;
	int tempVal = 0, tempMax = 0, tempMin = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	//系数倍率
	int tempcoefft = 0;
	int tempfbits = 0;

	//正负
	int tempSymbol = 0;
	//时间显示处理
	int tempTimeDel = 0;

	char Numbuf[6] = {0};
	char unitbuf[10] = {0};

	while(offset < len-1)
	{
		tempOBDMID = data[offset++];
		switch(tempOBDMID)
		{
			case 0x01:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [01]BANK1中氧传感器1监控器：");
				showDiagnosLen++;
				break;
			case 0x02:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [02]BANK1中氧传感器2监控器：");
				showDiagnosLen++;
				break;
			case 0x03:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [03]BANK1中氧传感器3监控器：");
				showDiagnosLen++;
				break;
			case 0x04:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [04]BANK1中氧传感器4监控器：");
				showDiagnosLen++;
				break;
			case 0x05:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [05]BANK2中氧传感器1监控器：");
				showDiagnosLen++;
				break;
			case 0x06:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [06]BANK2中氧传感器2监控器：");
				showDiagnosLen++;
				break;
			case 0x07:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [07]BANK2中氧传感器3监控器：");
				showDiagnosLen++;
				break;
			case 0x08:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [08]BANK2中氧传感器4监控器：");
				showDiagnosLen++;
				break;
			case 0x09:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [09]BANK3中氧传感器1监控器：");
				showDiagnosLen++;
				break;
			case 0x0a:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [0A]BANK3中氧传感器2监控器：");
				showDiagnosLen++;
				break;
			case 0x0b:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [0B]BANK3中氧传感器3监控器：");
				showDiagnosLen++;
				break;
			case 0x0c:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [0C]BANK3中氧传感器4监控器：");
				showDiagnosLen++;
				break;
			case 0x0d:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [0D]BANK4中氧传感器1监控器：");
				showDiagnosLen++;
				break;
			case 0x0e:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [0E]BANK4中氧传感器2监控器：");
				showDiagnosLen++;
				break;
			case 0x0f:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [0F]BANK4中氧传感器3监控器：");
				showDiagnosLen++;
				break;
			case 0x10:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [10]BANK4中氧传感器4监控器：");
				showDiagnosLen++;
				break;
			case 0x21:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [21]BANK1中催化剂监控器：");
				showDiagnosLen++;
				break;
			case 0x22:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [22]BANK2中催化剂监控器：");
				showDiagnosLen++;
				break;
			case 0x23:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [23]BANK3中催化剂监控器：");
				showDiagnosLen++;
				break;
			case 0x24:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [24]BANK4中催化剂监控器：");
				showDiagnosLen++;
				break;
			case 0x31:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [31]BANK1中EGR监控器：");
				showDiagnosLen++;
				break;
			case 0x32:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [32]BANK2中EGR监控器：");
				showDiagnosLen++;
				break;
			case 0x33:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [33]BANK3中EGR监控器：");
				showDiagnosLen++;
				break;
			case 0x34:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [34]BANK4中EGR监控器：");
				showDiagnosLen++;
				break;
			case 0x39:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [39]EVAP监控器(关闭)：");
				showDiagnosLen++;
				break;
			case 0x3a:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [3A]EVAP监控器(0,090)：");
				showDiagnosLen++;
				break;
			case 0x3b:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [3B]EVAP监控器(0,040)：");
				showDiagnosLen++;
				break;
			case 0x3c:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [3C]EVAP监控器(0,020)：");
				showDiagnosLen++;
				break;
			case 0x3d:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [3D]清洗流量监控器：");
				showDiagnosLen++;
				break;
			case 0x41:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [41]BANK1 氧传感加热监控器1：");
				showDiagnosLen++;
				break;
			case 0x42:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [42]BANK1 氧传感加热监控器2：");
				showDiagnosLen++;
				break;
			case 0x43:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [43]BANK1 氧传感加热监控器3：");
				showDiagnosLen++;
				break;
			case 0x44:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [44]BANK1 氧传感加热监控器4：");
				showDiagnosLen++;
				break;
			case 0x45:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [45]BANK2 氧传感加热监控器1：");
				showDiagnosLen++;
				break;
			case 0x46:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [46]BANK2 氧传感加热监控器2：");
				showDiagnosLen++;
				break;
			case 0x47:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [47]BANK2 氧传感加热监控器3：");
				showDiagnosLen++;
				break;
			case 0x48:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [48]BANK2 氧传感加热监控器4：");
				showDiagnosLen++;
				break;
			case 0x49:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [49]BANK3 氧传感加热监控器1：");
				showDiagnosLen++;
				break;
			case 0x4a:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [4A]BANK3 氧传感加热监控器2：");
				showDiagnosLen++;
				break;
			case 0x4b:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [4B]BANK3 氧传感加热监控器3：");
				showDiagnosLen++;
				break;
			case 0x4c:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [4C]BANK3 氧传感加热监控器4：");
				showDiagnosLen++;
				break;
			case 0x4d:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [4D]BANK4 氧传感加热监控器1：");
				showDiagnosLen++;
				break;
			case 0x4e:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [4E]BANK4 氧传感加热监控器2：");
				showDiagnosLen++;
				break;
			case 0x4f:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [4F]BANK4 氧传感加热监控器3：");
				showDiagnosLen++;
				break;
			case 0x50:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [50]BANK4 氧传感加热监控器4：");
				showDiagnosLen++;
				break;
			case 0x61:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [61]BANK1 加热催化剂监控器：");
				showDiagnosLen++;
				break;
			case 0x62:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [62]BANK2 加热催化剂监控器：");
				showDiagnosLen++;
				break;
			case 0x63:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [63]BANK3 加热催化剂监控器：");
				showDiagnosLen++;
				break;
			case 0x64:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [64]BANK4 加热催化剂监控器：");
				showDiagnosLen++;
				break;
			case 0x71:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [71]二次空气监控器1：");
				showDiagnosLen++;
				break;
			case 0x72:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [72]二次空气监控器2：");
				showDiagnosLen++;
				break;
			case 0x73:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [73]二次空气监控器3：");
				showDiagnosLen++;
				break;
			case 0x74:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [74]二次空气监控器4：");
				showDiagnosLen++;
				break;
			case 0x81:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [81]燃料系统监控器BANK1：");
				showDiagnosLen++;
				break;
			case 0x82:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [82]燃料系统监控器BANK2：");
				showDiagnosLen++;
				break;
			case 0x83:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [83]燃料系统监控器BANK3：");
				showDiagnosLen++;
				break;
			case 0x84:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [84]燃料系统监控器BANK4：");
				showDiagnosLen++;
				break;
			case 0xa1:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [A1]失火监控正常数据：");
				showDiagnosLen++;
				break;
			case 0xa2:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [A2]失火气缸数据1：");
				showDiagnosLen++;
				break;
			case 0xa3:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [A3]失火气缸数据2：");
				showDiagnosLen++;
				break;
			case 0xa4:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [A4]失火气缸数据3：");
				showDiagnosLen++;
				break;
			case 0xa5:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [A5]失火气缸数据4：");
				showDiagnosLen++;
				break;
			case 0xa6:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [A6]失火气缸数据5：");
				showDiagnosLen++;
				break;
			case 0xa7:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [A7]失火气缸数据6：");
				showDiagnosLen++;
				break;
			case 0xa8:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [A8]失火气缸数据7：");
				showDiagnosLen++;
				break;
			case 0xa9:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [A9]失火气缸数据8：");
				showDiagnosLen++;
				break;
			case 0xaa:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [AA]失火气缸数据9：");
				showDiagnosLen++;
				break;
			case 0xab:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [AB]失火气缸数据10：");
				showDiagnosLen++;
				break;
			case 0xac:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [AC]失火气缸数据11：");
				showDiagnosLen++;
				break;
			case 0xad:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [AD]失火气缸数据12：");
				showDiagnosLen++;
				break;
			default:
				strcpy(showDiagnosBuf[showDiagnosLen], "Can't identify OBDMID：0x");
				IntToSysChar(tempOBDMID, Numbuf, 16);
				strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
				memset(Numbuf, 0, 6);
				showDiagnosLen++;
				
				break;
		}
		testID = data[offset++];
		switch(testID)
		{
			case 0x01:
				strcpy(showDiagnosBuf[showDiagnosLen], "TID [01]从稀到浓传感器阈值电压");
				showDiagnosLen++;
				break;
			case 0x02:
				strcpy(showDiagnosBuf[showDiagnosLen], "TID [02]从浓到稀传感器阈值电压");
				showDiagnosLen++;
				break;
			case 0x03:
				strcpy(showDiagnosBuf[showDiagnosLen], "TID [03]进行切换时间计算的传感器低电压");
				showDiagnosLen++;
				break;
			case 0x04:
				strcpy(showDiagnosBuf[showDiagnosLen], "TID [04]进行切换时间计算的传感器高电压");
				showDiagnosLen++;
				break;
			case 0x05:
				strcpy(showDiagnosBuf[showDiagnosLen], "TID [05]从浓到稀传感器的切换时间");
				showDiagnosLen++;
				break;
			case 0x06:
				strcpy(showDiagnosBuf[showDiagnosLen], "TID [06]从稀到浓传感器的切换时间");
				showDiagnosLen++;
				break;
			case 0x07:
				strcpy(showDiagnosBuf[showDiagnosLen], "TID [07]测试循环中的最小传感器电压");
				showDiagnosLen++;
				break;
			case 0x08:
				strcpy(showDiagnosBuf[showDiagnosLen], "TID [08]测试循环中的最大传感器电压");
				showDiagnosLen++;
				break;
			case 0x09:
				strcpy(showDiagnosBuf[showDiagnosLen], "TID [09]传感器的切换时间");
				showDiagnosLen++;
				break;
			case 0x0a:
				strcpy(showDiagnosBuf[showDiagnosLen], "TID [0A]传感器的周期");
				showDiagnosLen++;
				break;
			default:
				strcpy(showDiagnosBuf[showDiagnosLen], "Can't identify TID : 0x");
				IntToSysChar(testID, Numbuf, 16);
				strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
				memset(Numbuf, 0, 6);
				showDiagnosLen++;
				break;
		}
		
		unitScalID = data[offset++];
		//TEST Value
		tempVal = data[offset++];
		tempVal <<= 8;
		tempVal |= data[offset++];
		//MIN Value
		tempMin = data[offset++];
		tempMin <<= 8;
		tempMin |= data[offset++];
		//MAX Value
		tempMax = data[offset++];
		tempMax <<= 8;
		tempMax |= data[offset++];
		
		switch(unitScalID)
		{
			case 0x01:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [01]Raw Value.");
				tempcoefft = 1;
				tempfbits = 1;
				strcpy(unitbuf, "");
				break;
			case 0x02:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [02]Raw Value.");
				tempcoefft = 1;
				tempfbits = 10;
				strcpy(unitbuf, "");
				break;
			case 0x03:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [03]Raw Value.");
				tempcoefft = 1;
				tempfbits = 100;
				strcpy(unitbuf, "");
				break;
			case 0x04:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [04]Raw Value.");
				tempcoefft = 1;
				tempfbits = 1000;
				strcpy(unitbuf, "");
				break;
			case 0x05:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [05]Raw Value.");
				tempcoefft = 305;
				tempfbits = 10000000;
				strcpy(unitbuf, "");
				break;
			case 0x06:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [06]Raw Value.");
				tempcoefft = 305;
				tempfbits = 1000000;
				strcpy(unitbuf, "");
				break;
			case 0x07:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [07]旋转频率.");
				tempcoefft = 25;
				tempfbits = 100;
				strcpy(unitbuf, "rpm");
				break;
			case 0x08:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [08]速度.");
				tempcoefft = 62137;
				tempfbits = 10000000;
				strcpy(unitbuf, "mph");
				break;
			case 0x09:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [09]速度.");
				tempcoefft = 62137;
				tempfbits = 100000;
				strcpy(unitbuf, "mph");
				break;
			case 0x0a:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [0A]电压.");
				tempcoefft = 122;
				tempfbits = 1000000;
				strcpy(unitbuf, "V");
				break;
			case 0x0b:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [0B]电压.");
				tempcoefft = 1;
				tempfbits = 1000;
				strcpy(unitbuf, "V");
				break;
			case 0x0c:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [0C]电压.");
				tempcoefft = 1;
				tempfbits = 100;
				strcpy(unitbuf, "V");
				break;
			case 0x0d:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [0D]电流.");
				tempcoefft = 390625;
				tempfbits = 100000000;
				strcpy(unitbuf, "mA");
				break;
			case 0x0e:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [0E]电流.");
				tempcoefft = 1;
				tempfbits = 1000;
				strcpy(unitbuf, "A");
				break;
			case 0x0f:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [0F]电流.");
				tempcoefft = 1;
				tempfbits = 100;
				strcpy(unitbuf, "A");
				break;
			case 0x10:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [10]时间.");
				tempcoefft = 1;
				tempfbits = 1000;
				tempTimeDel = 1;
				strcpy(unitbuf, "s");		//处理
				break;
			case 0x11:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [11]时间.");
				tempcoefft = 1;
				tempfbits = 10;
				tempTimeDel = 1;
				strcpy(unitbuf, "s");		//处理
				break;
			case 0x12:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [12]时间.");
				tempcoefft = 1;
				tempfbits = 1;
				tempTimeDel = 1;
				strcpy(unitbuf, "s");		//处理
				break;
			case 0x13:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [13]电阻.");
				tempcoefft = 1;
				tempfbits = 1000;
				strcpy(unitbuf, "Ohm");
			case 0x14:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [14]电阻.");
				tempcoefft = 1;
				tempfbits = 1000;
				strcpy(unitbuf, "kOhm");
				break;
			case 0x15:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [15]电阻.");
				tempcoefft = 1;
				tempfbits = 1;
				strcpy(unitbuf, "kOhm");
				break;
			case 0x16:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [16]温度.");
				tempcoefft = 1;
				tempfbits = 10;
				if(tempVal > 400)
					tempVal -= 400;
				else
				{
					tempSymbol = 1;
					tempVal = 400 - tempVal;
				}
				strcpy(unitbuf, "℃");
				break;
			case 0x17:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [17]压力计.");
				tempcoefft = 1;
				tempfbits = 100;
				strcpy(unitbuf, "kpa");
				break;
			case 0x18:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [18]压力（气压）.");
				tempcoefft = 117;
				tempfbits = 10000;
				strcpy(unitbuf, "kpa");
				break;
			case 0x19:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [19]压力（燃料）.");
				tempcoefft = 79;
				tempfbits = 1000;
				strcpy(unitbuf, "kpa");
				break;
			case 0x1a:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [1A]压力计.");
				tempcoefft = 1;
				tempfbits = 1;
				strcpy(unitbuf, "kpa");
				break;
			case 0x1b:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [1B]压力（柴油）.");
				tempcoefft = 10;
				tempfbits = 1;
				strcpy(unitbuf, "kpa");
				break;
			case 0x1c:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [1C]角度.");
				tempcoefft = 1;
				tempfbits = 100;
				strcpy(unitbuf, "°");
				break;
			case 0x1d:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [1D]角度.");
				tempcoefft = 5;
				tempfbits = 10;
				strcpy(unitbuf, "°");
				break;
			case 0x1e:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [1E]等价比.");
				tempcoefft = 305;
				tempfbits = 10000000;
				strcpy(unitbuf, "");
				break;
			case 0x1f:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [1F]空气/燃料比.");
				tempcoefft = 5;
				tempfbits = 100;
				strcpy(unitbuf, "");
				break;
			case 0x20:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [20]比例.");
				tempcoefft = 39062;
				tempfbits = 10000000;
				strcpy(unitbuf, "");
				break;
			case 0x21:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [21]频率.");
				tempcoefft = 1;
				tempfbits = 1000;
				strcpy(unitbuf, "Hz");
				break;
			case 0x22:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [22]频率.");
				tempcoefft = 1;
				tempfbits = 1;
				strcpy(unitbuf, "Hz");
				break;
			case 0x23:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [23]频率.");
				tempcoefft = 1;
				tempfbits = 1000;
				strcpy(unitbuf, "MHz");
				break;
			case 0x24:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [24]次数.");
				tempcoefft = 1;
				tempfbits = 1;
				strcpy(unitbuf, "");
				break;
			case 0x25:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [25]距离.");
				tempcoefft = 62137;
				tempfbits = 100000;
				strcpy(unitbuf, "miles");
				break;
			case 0x26:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [26]电压/时间.");
				tempcoefft = 1;
				tempfbits = 10000;
				strcpy(unitbuf, "V/ms");
				break;
			case 0x27:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [27]质量/时间.");
				tempcoefft = 22046;
				tempfbits = 1000000000;
				strcpy(unitbuf, "lb/s");
				break;
			case 0x28:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [28]质量/时间.");
				tempcoefft = 22046;
				tempfbits = 10000000;
				strcpy(unitbuf, "lb/s");
				break;
			case 0x29:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [29]气压/时间.");
				tempcoefft = 25;
				tempfbits = 100000;
				strcpy(unitbuf, "kpa/s");
				break;
			case 0x2a:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [2A]质量/时间.");
				tempcoefft = 1;
				tempfbits = 1000;
				strcpy(unitbuf, "kg/h");
				break;
			case 0x2b:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [2B]开关.");
				tempcoefft = 1;
				tempfbits = 1;
				strcpy(unitbuf, "");
				break;
			case 0x2c:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [2C]质量/缸.");
				tempcoefft = 1;
				tempfbits = 100;
				strcpy(unitbuf, "g/cyl");
				break;
			case 0x2d:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [2D]质量/划.");
				tempcoefft = 1;
				tempfbits = 100;
				strcpy(unitbuf, "mg/stroke");
				break;
			case 0x2e:
				//true or false
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [2E]真/假.");

				if(tempVal & 0x01)
					strcat(showDiagnosBuf[showDiagnosLen], "真");
				else
					strcat(showDiagnosBuf[showDiagnosLen], "假");
				showDiagnosLen++;
				return;
			case 0x2f:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [2F]百分比.");
				tempcoefft = 1;
				tempfbits = 100;
				strcpy(unitbuf, "%");
				break;
			case 0x30:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [30]百分比.");
				tempcoefft = 1526;
				tempfbits = 1000000;
				strcpy(unitbuf, "%");
				break;
			case 0x31:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [31]体积.");
				tempcoefft = 1;
				tempfbits = 1000;
				strcpy(unitbuf, "L");
				break;
			case 0x32:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [32]长度.");
				tempcoefft = 305;
				tempfbits = 10000000;
				strcpy(unitbuf, "inch");
				break;
			case 0x33:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [33]等价比率.");
				tempcoefft = 24414;
				tempfbits = 100000000;
				strcpy(unitbuf, "");
				break;
			case 0x34:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [34]时间.");
				tempcoefft = 1;
				tempfbits = 1;
				tempTimeDel = 1;
				strcpy(unitbuf, "min");
				break;
			case 0x35:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [35]时间.");
				tempcoefft = 1;
				tempfbits = 100;
				tempTimeDel = 1;
				strcpy(unitbuf, "s");			//处理
				break;
			case 0x36:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [36]宽度.");
				tempcoefft = 22075;
				tempfbits = 1000000000;
				strcpy(unitbuf, "lbs");
				break;
			case 0x37:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [37]宽度.");
				tempcoefft = 22075;
				tempfbits = 100000000;
				strcpy(unitbuf, "lbs");
				break;
			case 0x38:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [38]宽度.");
				tempcoefft = 22075;
				tempfbits = 10000000;
				strcpy(unitbuf, "lbs");
				break;
			case 0x39:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [39]百分比.");
				tempcoefft = 1;
				tempfbits = 100;

				if(tempVal > 32768)
					tempVal -= 32768;
				else
				{
					tempSymbol = 1;
					tempVal = 32768 - tempVal;
				}
				strcpy(unitbuf, "%");
				break;
			case 0x81:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [81]Raw Value.");
				tempcoefft = 1;
				tempfbits = 1;

				if(tempVal & 0x8000)
				{
					tempVal &= 0x7FFF;
					tempSymbol = 1;
					tempVal = 0x8000 - tempVal;
				}
				strcpy(unitbuf, "");
				break;
			case 0x82:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [82]Raw Value.");
				tempcoefft = 1;
				tempfbits = 10;

				if(tempVal & 0x8000)
				{
					tempVal &= 0x7FFF;
					tempSymbol = 1;
					tempVal = 0x8000 - tempVal;
				}
				strcpy(unitbuf, "");
				break;
			case 0x83:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [83]Raw Value.");
				tempcoefft = 1;
				tempfbits = 100;

				if(tempVal & 0x8000)
				{
					tempVal &= 0x7FFF;
					tempSymbol = 1;
					tempVal = 0x8000 - tempVal;
				}
				strcpy(unitbuf, "");
				break;
			case 0x84:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [84]Raw Value.");
				tempcoefft = 1;
				tempfbits = 1000;

				if(tempVal & 0x8000)
				{
					tempVal &= 0x7FFF;
					tempSymbol = 1;
					tempVal = 0x8000 - tempVal;
				}
				strcpy(unitbuf, "");
				break;
			case 0x85:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [85]Raw Value.");
				tempcoefft = 305;
				tempfbits = 10000000;

				if(tempVal & 0x8000)
				{
					tempVal &= 0x7FFF;
					tempSymbol = 1;
					tempVal = 0x8000 - tempVal;
				}
				strcpy(unitbuf, "");
				break;
			case 0x86:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [86]Raw Value.");
				tempcoefft = 305;
				tempfbits = 1000000;

				if(tempVal & 0x8000)
				{
					tempVal &= 0x7FFF;
					tempSymbol = 1;
					tempVal = 0x8000 - tempVal;
				}
				strcpy(unitbuf, "");
				break;
			case 0x8a:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [8A]电压.");
				tempcoefft = 122;
				tempfbits = 1000000;

				if(tempVal & 0x8000)
				{
					tempVal &= 0x7FFF;
					tempSymbol = 1;
					tempVal = 0x8000 - tempVal;
				}
				strcpy(unitbuf, "V");
				break;
			case 0x8b:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [8B]电压.");
				tempcoefft = 1;
				tempfbits = 1000;

				if(tempVal & 0x8000)
				{
					tempVal &= 0x7FFF;
					tempSymbol = 1;
					tempVal = 0x8000 - tempVal;
				}
				strcpy(unitbuf, "V");
				break;
			case 0x8c:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [8C]电压.");
				tempcoefft = 1;
				tempfbits = 100;

				if(tempVal & 0x8000)
				{
					tempVal &= 0x7FFF;
					tempSymbol = 1;
					tempVal = 0x8000 - tempVal;
				}
				strcpy(unitbuf, "V");
				break;
			case 0x8d:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [8D]电流.");
				tempcoefft = 390625;
				tempfbits = 100000000;

				if(tempVal & 0x8000)
				{
					tempVal &= 0x7FFF;
					tempSymbol = 1;
					tempVal = 0x8000 - tempVal;
				}
				strcpy(unitbuf, "mA");
				break;
			case 0x8e:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [8E]电流.");
				tempcoefft = 1;
				tempfbits = 1000;

				if(tempVal & 0x8000)
				{
					tempVal &= 0x7FFF;
					tempSymbol = 1;
					tempVal = 0x8000 - tempVal;
				}
				strcpy(unitbuf, "A");
				break;
			case 0x90:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [90]时间.");
				tempcoefft = 1;
				tempfbits = 1000;

				if(tempVal & 0x8000)
				{
					tempVal &= 0x7FFF;
					tempSymbol = 1;
					tempVal = 0x8000 - tempVal;
				}
				strcpy(unitbuf, "s");
				break;
			case 0x96:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [96]温度.");
				tempcoefft = 1;
				tempfbits = 10;

				if(tempVal & 0x8000)
				{
					tempVal &= 0x7FFF;
					tempSymbol = 1;
					tempVal = 0x8000 - tempVal;
				}
				strcpy(unitbuf, "℃");
				break;
			case 0x9c:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [9C]角度.");
				tempcoefft = 1;
				tempfbits = 100;

				if(tempVal & 0x8000)
				{
					tempVal &= 0x7FFF;
					tempSymbol = 1;
					tempVal = 0x8000 - tempVal;
				}
				strcpy(unitbuf, "°");
				break;
			case 0x9d:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [9D]角度.");
				tempcoefft = 5;
				tempfbits = 10;

				if(tempVal & 0x8000)
				{
					tempVal &= 0x7FFF;
					tempSymbol = 1;
					tempVal = 0x8000 - tempVal;
				}
				strcpy(unitbuf, "°");
				break;
			case 0xa8:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [A8]质量/时间.");
				tempcoefft = 22046;
				tempfbits = 10000000;

				if(tempVal & 0x8000)
				{
					tempVal &= 0x7FFF;
					tempSymbol = 1;
					tempVal = 0x8000 - tempVal;
				}
				strcpy(unitbuf, "lb/s");
				break;
			case 0xa9:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [A9]压力/时间.");
				tempcoefft = 100365;
				tempfbits = 100000000;

				if(tempVal & 0x8000)
				{
					tempVal &= 0x7FFF;
					tempSymbol = 1;
					tempVal = 0x8000 - tempVal;
				}
				strcpy(unitbuf, "inH2O/s");
				break;
			case 0xaf:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [AF]百分比.");
				tempcoefft = 1;
				tempfbits = 100;

				if(tempVal & 0x8000)
				{
					tempVal &= 0x7FFF;
					tempSymbol = 1;
					tempVal = 0x8000 - tempVal;
				}
				strcpy(unitbuf, "%");
				break;
			case 0xb0:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [B0]百分比.");
				tempcoefft = 3052;
				tempfbits = 1000000;

				if(tempVal & 0x8000)
				{
					tempVal &= 0x7FFF;
					tempSymbol = 1;
					tempVal = 0x8000 - tempVal;
				}
				strcpy(unitbuf, "%");
				break;
			case 0xb1:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [B1]电压/时间.");
				tempcoefft = 2;
				tempfbits = 1;

				if(tempVal & 0x8000)
				{
					tempVal &= 0x7FFF;
					tempSymbol = 1;
					tempVal = 0x8000 - tempVal;
				}
				strcpy(unitbuf, "mV/s");
				break;
			case 0xfd:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [FD]绝对压力.");
				tempcoefft = 1;
				tempfbits = 1000;

				if(tempVal & 0x8000)
				{
					tempVal &= 0x7FFF;
					tempSymbol = 1;
					tempVal = 0x8000 - tempVal;
				}
				strcpy(unitbuf, "kpa");
				break;
			case 0xfe:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [FE]绝对（真空）.");
				tempcoefft = 100365;
				tempfbits = 100000000;

				if(tempVal & 0x8000)
				{
					tempVal &= 0x7FFF;
					tempSymbol = 1;
					tempVal = 0x8000 - tempVal;
				}
				strcpy(unitbuf, "inH2O");
				break;
			default:
				strcpy(showDiagnosBuf[showDiagnosLen], "Can't identify UASID：0x");
				IntToSysChar(unitScalID, Numbuf, 16);
				strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
				tempcoefft = 1;
				memset(Numbuf, 0, 6);
				break;
		}
		showDiagnosLen++;
		
		if(tempSymbol == 1)//负数
			strcat(showDiagnosBuf[showDiagnosLen], "-");
		if(tempTimeDel == 0 
			&& tempTimeDel == 0)//正数 且不是时间数
		{
			strcat(showDiagnosBuf[showDiagnosLen], "Test Value：");
			temp = tempVal * tempcoefft;
			//整数
			tempVal_i = temp/tempfbits;
			IntToSysChar(tempVal_i, Numbuf, 10);
			strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
			strcat(showDiagnosBuf[showDiagnosLen], ".");
			memset(Numbuf, 0, 6);
			//小数
			if(tempfbits > 1)
			{
				tempVal_f = temp%tempfbits;
				while(tempVal_f >= 10000)
					tempVal_f	/= 10;

				IntToSysChar(tempVal_f, Numbuf, 10);
				strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
				memset(Numbuf, 0, 6);
			}
			//单位
			strcat(showDiagnosBuf[showDiagnosLen], unitbuf);
			showDiagnosLen++;
			strcat(showDiagnosBuf[showDiagnosLen], "MIN.Test Limit：");
			temp = tempMin * tempcoefft;
			
			//整数
			tempVal_i = temp/tempfbits;
			IntToSysChar(tempVal_i, Numbuf, 10);
			strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
			strcat(showDiagnosBuf[showDiagnosLen], ".");
			memset(Numbuf, 0, 6);
			//小数
			if(tempfbits > 1)
			{
				tempVal_f = temp%tempfbits;
				while(tempVal_f >= 10000)
					tempVal_f	/= 10;

				IntToSysChar(tempVal_f, Numbuf, 10);
				strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
				memset(Numbuf, 0, 6);
			}
			//单位
			strcat(showDiagnosBuf[showDiagnosLen], unitbuf);
			showDiagnosLen++;
			strcat(showDiagnosBuf[showDiagnosLen], "MAX.Test Limit：");
			temp = tempMax * tempcoefft;
			//整数
			tempVal_i = temp/tempfbits;
			IntToSysChar(tempVal_i, Numbuf, 10);
			strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
			strcat(showDiagnosBuf[showDiagnosLen], ".");
			memset(Numbuf, 0, 6);
			//小数
			if(tempfbits > 1)
			{
				tempVal_f = temp%tempfbits;
				while(tempVal_f >= 10000)
					tempVal_f	/= 10;

				IntToSysChar(tempVal_f, Numbuf, 10);
				strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
				memset(Numbuf, 0, 6);
			}
			//单位
			strcat(showDiagnosBuf[showDiagnosLen], unitbuf);
			showDiagnosLen++;
		}
		else if(tempTimeDel == 1)//时间
		{
		if(0 == strcmp(unitbuf,"min"))
		{
			tempVal_i = temp/tempfbits;
			tempVal_i /= 60;
			tempVal_i /= 24;
			//days
			IntToSysChar(tempVal_i, Numbuf, 10);
			strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
			strcat(showDiagnosBuf[showDiagnosLen], "days");
			memset(Numbuf, 0, 6);
			//hour
			tempVal_i = temp/tempfbits;
			tempVal_i /= 60;
			tempVal_i %= 24;

			IntToSysChar(tempVal_i, Numbuf, 10);
			strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
			strcat(showDiagnosBuf[showDiagnosLen], "h");
			memset(Numbuf, 0, 6);

			//min
			tempVal_i = temp/tempfbits;
			tempVal_i %= 60;
			IntToSysChar(tempVal_i, Numbuf, 10);
			strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
			strcat(showDiagnosBuf[showDiagnosLen], "min");
			memset(Numbuf, 0, 6);
		}
		else if(0 == strcmp(unitbuf,"s"))
		{
			tempVal_i = temp/tempfbits;
			//hour
			tempVal_i = temp/tempfbits;
			tempVal_i /= 60;
			tempVal_i /= 60;
			IntToSysChar(tempVal_i, Numbuf, 10);
			strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
			strcat(showDiagnosBuf[showDiagnosLen], "h");
			memset(Numbuf, 0, 6);
			//min
			tempVal_i = temp/tempfbits;
			tempVal_i /= 60;
			tempVal_i %= 60;
			IntToSysChar(tempVal_i, Numbuf, 10);
			strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
			strcat(showDiagnosBuf[showDiagnosLen], "min");
			memset(Numbuf, 0, 6);
			//second
			tempVal_i = temp/tempfbits;
			tempVal_i %= 60;
			IntToSysChar(tempVal_i, Numbuf, 10);
			strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
			strcat(showDiagnosBuf[showDiagnosLen], "s");
			memset(Numbuf, 0, 6);
		}
	}
	}
}

int uds_service_sid07(unsigned char *rxbuf, int *rxlen)
{
int ret = 0;
	int i = 0;
	int txlen = 0;
	int checklen = 0;
	int checkbuf[2];

	sendFailedCnt = 0;
	do{
		txlen = 0;
		if(diagnose.commtypes == 0)
		{
			commbuf[txlen++] = diagnose.server_id;
			checkbuf[checklen++] = diagnose.server_id + 0x40;
			ret = UdsRequest1(diagnose.funAddr, 0, (unsigned char *)commbuf, txlen, rxbuf, (unsigned short *)rxlen);
			if(ret == 0)
			{
				for(i = 0; i < checklen; i++)
				{
					if(checkbuf[i] != rxbuf[i])
					{
						return 2;
					}
				}
			}
			else if(ret == 1)
			{
				if(sendFailedCnt++ >= 3)
				{
					return 1;//发生接收错误
				}
			}
			else
			{
				return ret;
			}
		}
		else
		{
			commbuf[txlen++] = 0x81;
			commbuf[txlen++] = 0x10;
			commbuf[txlen++] = 0xF1;
			commbuf[txlen++] = diagnose.server_id;
			checkbuf[checklen++] = diagnose.server_id + 0x40;
			ret = KwpRequest(commbuf, txlen, (char *)rxbuf, rxlen, NULL);
			if(ret == 0)
			{
				for(i = 0; i < checklen; i++)
				{
					if(checkbuf[i] != rxbuf[i])
					{
						return 2;
					}
				}
			}
			else if(ret == 1)
			{
				if(sendFailedCnt++ >= 3)
				{
					return 1;//发生接收错误
				}
			}
			else
			{
				return ret;
			}
		}
	}while(ret != 0);

	return ret;
}

int uds_service_sid08(unsigned char *rxbuf, int *rxlen)
{
	int ret = 0;
	int i = 0;
	int txlen = 0;
	int checklen = 0;
	int checkbuf[2];

	sendFailedCnt = 0;
	do{
		txlen = 0;
		if(diagnose.commtypes == 0)
		{
			commbuf[txlen++] = diagnose.server_id;
			commbuf[txlen++] = diagnose.sub_function;
			checkbuf[checklen++] = diagnose.server_id + 0x40;
			checkbuf[checklen++] = diagnose.sub_function;
			ret = UdsRequest1(diagnose.funAddr, 0, (unsigned char *)commbuf, txlen, rxbuf, (unsigned short *)rxlen);
			if(ret == 0)
			{
				for(i = 0; i < checklen; i++)
				{
					if(checkbuf[i] != rxbuf[i])
					{
						return 2;
					}
				}
			}
			else if(ret == 1)
			{
				if(sendFailedCnt++ >= 3)
				{
					return 1;//发生接收错误
				}
			}
			else
			{
				return ret;
			}
		}
		else
		{
//			txlen = kwpFmt((unsigned char *)commbuf, 2);//头部信息
			commbuf[txlen++] = 0x82;
			commbuf[txlen++] = 0x10;
			commbuf[txlen++] = 0xF1;
			commbuf[txlen++] = diagnose.server_id;
			commbuf[txlen++] = diagnose.sub_function;
			checkbuf[checklen++] = diagnose.server_id + 0x40;
			checkbuf[checklen++] = diagnose.sub_function;
			ret = KwpRequest(commbuf, txlen, (char *)rxbuf, rxlen, NULL);
			if(ret == 0)
			{
				for(i = 0; i < checklen; i++)
				{
					if(checkbuf[i] != rxbuf[i])
					{
						return 2;
					}
				}
			}
			else if(ret == 1)
			{
				if(sendFailedCnt++ >= 3)
				{
					return 1;//发生接收错误
				}
			}
			else
			{
				return ret;
			}
		}
	}while(ret != 0);

	return ret;
}

int uds_service_sid09(unsigned char *rxbuf, int *rxlen)
{
	int ret = 0;
	int i = 0;
	int txlen = 0;
	int checklen = 0;
	int checkbuf[2];

	sendFailedCnt = 0;
	do{
		txlen = 0;
		if(diagnose.commtypes == 0)
		{
			commbuf[txlen++] = diagnose.server_id;
			commbuf[txlen++] = diagnose.sub_function;
			checkbuf[checklen++] = diagnose.server_id + 0x40;
			checkbuf[checklen++] = diagnose.sub_function;
			ret = UdsRequest1(diagnose.funAddr, 0, (unsigned char *)commbuf, txlen, rxbuf, (unsigned short *)rxlen);
			if(ret == 0)
			{
				for(i = 0; i < checklen; i++)
				{
					if(checkbuf[i] != rxbuf[i])
					{
						return 2;
					}
				}
			}
			else if(ret == 1)
			{
				if(sendFailedCnt++ >= 3)
				{
					return 1;//发生接收错误
				}
			}
			else
			{
				return ret;
			}
		}
		else
		{
//			txlen = kwpFmt((unsigned char *)commbuf, 2);//头部信息
			commbuf[txlen++] = 0x82;
			commbuf[txlen++] = 0x10;
			commbuf[txlen++] = 0xF1;
			commbuf[txlen++] = diagnose.server_id;
			commbuf[txlen++] = diagnose.sub_function;
			checkbuf[checklen++] = diagnose.server_id + 0x40;
			checkbuf[checklen++] = diagnose.sub_function;
			ret = KwpRequest(commbuf, txlen, (char *)rxbuf, rxlen, NULL);
			if(ret == 0)
			{
				for(i = 0; i < checklen; i++)
				{
					if(checkbuf[i] != rxbuf[i])
					{
						return 2;
					}
				}
			}
			else if(ret == 1)
			{
				if(sendFailedCnt++ >= 3)
				{
					return 1;//发生接收错误
				}
			}
			else
			{
				return ret;
			}
		}
	}while(ret != 0);

	return ret;
}

void analysis_sid09_tid01(char *data)
{
	int offset = 0;
	int tempVal = 0;
	char Numbuf[3] = {0};

	//第一个数据
	strcpy(showDiagnosBuf[showDiagnosLen], "info01 [A]VIN消息个数：");
	tempVal = data[offset];

	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

	MessageCount1 = tempVal;
	
	showDiagnosLen++;
}

void analysis_sid09_tid02(char *data)
{
//	int i=0;
	char Numbuf[18] = {0};
	//第一个数据
	strcpy(showDiagnosBuf[showDiagnosLen], "info02 [17Bytes]VIN：");
	if(diagnose.commtypes == 0)
	{
		memcpy(Numbuf, &data[1], 17);
		strcat(showDiagnosBuf[showDiagnosLen], (char *)Numbuf);//无需填充
	}
	else
	{
		memcpy(Numbuf, &data[3], 17);
		strcat(showDiagnosBuf[showDiagnosLen], (char *)Numbuf);//填充3个0x00
	}
	showDiagnosLen++;
}

void analysis_sid09_tid03(char *data)
{
	int offset = 0;
	int tempVal = 0;
	char Numbuf[3] = {0};

	//第一个数据
	strcpy(showDiagnosBuf[showDiagnosLen], "info03 [A]CALID消息个数：");
	tempVal = data[offset];

	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

	MessageCount3 = tempVal;
	
	showDiagnosLen++;
}


void analysis_sid09_tid04(char *data)
{
	int offset = 0;
	int num = 0;
	int No = 0;
	int i = 0;
	char Numbuf[16];

	strcpy(showDiagnosBuf[showDiagnosLen], "info04 [16bytes]Calibration ID Number：");
	if(diagnose.commtypes == 0)
	{
		num = data[offset++];
		IntToSysChar(num, Numbuf, 10);
		strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
		showDiagnosLen++;
		No++;
	}
	else
	{
		num = (MessageCount3/4);
		IntToSysChar(num, Numbuf, 10);
		strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
		showDiagnosLen++;
		No++;		
	}
	
	while(num > 0)
	{
		memset(Numbuf, '\0', 16);
		IntToSysChar(No++, Numbuf, 10);
		strcat(showDiagnosBuf[showDiagnosLen], "info04 NO.");
		strcat(showDiagnosBuf[showDiagnosLen], Numbuf);//num
		strcat(showDiagnosBuf[showDiagnosLen], "：");
		memcpy(Numbuf, &data[1 + i * 16], 16);
		strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
		showDiagnosLen++;
		num--;
		i++;
	}
}

void analysis_sid09_tid05(char *data)
{
	int offset = 0;
	int tempVal = 0;
	char Numbuf[3] = {0};

	//第一个数据
	strcpy(showDiagnosBuf[showDiagnosLen], "info05 [A]CVN消息个数：");
	tempVal = data[offset];

	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

	MessageCount5 = tempVal;
	
	showDiagnosLen++;
}


void analysis_sid09_tid06(char *data)
{
	int offset = 0;
	int num = 1;
	int i = 0;
	int No = 0;
	char Numbuf[4] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "info06 [4bytes]Calibration Verification Number：");
	if(diagnose.commtypes == 0)
	{
		num = data[offset++];
		IntToSysChar(num, Numbuf, 10);
		strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
		showDiagnosLen++;
		No++;
	}
	else
	{
		num = MessageCount5;
		IntToSysChar(num, Numbuf, 10);
		strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
		showDiagnosLen++;
		No++;		
	}
	while(num > 0)
	{
		memset(Numbuf, '\0', 4);
		IntToSysChar(No++, Numbuf, 10);
		strcat(showDiagnosBuf[showDiagnosLen], "info06 NO.");
		strcat(showDiagnosBuf[showDiagnosLen], Numbuf);//num
		strcat(showDiagnosBuf[showDiagnosLen], "：0x");
		for(i = 0; i < 4; i++)
		{
			memset(Numbuf, '\0', 2);
			if(data[offset] <= 0x0F)
			{
				strcat(showDiagnosBuf[showDiagnosLen], "0");
			}
			IntToSysChar(data[offset++], Numbuf, 16);
			strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
		}
		showDiagnosLen++;
		num--;
	}
}

void analysis_sid09_tid07(char *data)
{
	int offset = 0;
	int tempVal = 0;
	char Numbuf[3] = {0};

	//第一个数据
	strcpy(showDiagnosBuf[showDiagnosLen], "info07 [A]IPT消息个数：");
	tempVal = data[offset];

	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

	MessageCount7 = tempVal;
	
	showDiagnosLen++;
}

void analysis_sid09_tid08(char *data)
{
	int offset = 0;
	int tempVal = 0;
	char Numbuf[5] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "info08 [0]性能跟踪个数：");
	
	if(diagnose.commtypes == 0)
	{
		tempVal = data[offset++];
		IntToSysChar(tempVal, Numbuf, 10);
		strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
		showDiagnosLen++;
		memset(Numbuf, 0, 5);
	}
	else
	{
		tempVal = MessageCount7/2;
		IntToSysChar(tempVal, Numbuf, 10);
		strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
		showDiagnosLen++;
		memset(Numbuf, 0, 5);
	}

	strcpy(showDiagnosBuf[showDiagnosLen], "info08 [AB]OBD监视条件遇到计数：");
	tempVal = data[offset++];
	tempVal <<= 8;
	tempVal |= data[offset++];

	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf, 0, 5);
	showDiagnosLen++;

	strcpy(showDiagnosBuf[showDiagnosLen], "info08 [AB]点火周期计数器：");
	tempVal = data[offset++];
	tempVal <<= 8;
	tempVal |= data[offset++];

	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf, 0, 5);
	showDiagnosLen++;

	strcpy(showDiagnosBuf[showDiagnosLen], "info08 [AB]Bank1催化剂监控完成次数：");
	tempVal = data[offset++];
	tempVal <<= 8;
	tempVal |= data[offset++];

	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf, 0, 5);
	showDiagnosLen++;

	strcpy(showDiagnosBuf[showDiagnosLen], "info08 [AB]Bank1催化剂监控遇到的次数：");
	tempVal = data[offset++];
	tempVal <<= 8;
	tempVal |= data[offset++];

	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf, 0, 5);
	showDiagnosLen++;

	strcpy(showDiagnosBuf[showDiagnosLen], "info08 [AB]Bank2催化剂监控完成次数：");
	tempVal = data[offset++];
	tempVal <<= 8;
	tempVal |= data[offset++];

	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf, 0, 5);
	showDiagnosLen++;

	strcpy(showDiagnosBuf[showDiagnosLen], "info08 [AB]Bank2催化剂监控遇到的次数：");
	tempVal = data[offset++];
	tempVal <<= 8;
	tempVal |= data[offset++];

	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf, 0, 5);
	showDiagnosLen++;

	strcpy(showDiagnosBuf[showDiagnosLen], "info08 [AB]Bank1氧传感器监控完成次数：");
	tempVal = data[offset++];
	tempVal <<= 8;
	tempVal |= data[offset++];

	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf, 0, 5);
	showDiagnosLen++;

	strcpy(showDiagnosBuf[showDiagnosLen], "info08 [AB]Bank1氧传感器监控遇到的次数：");
	tempVal = data[offset++];
	tempVal <<= 8;
	tempVal |= data[offset++];

	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf, 0, 5);
	showDiagnosLen++;

	strcpy(showDiagnosBuf[showDiagnosLen], "info08 [AB]Bank2氧传感器监控完成次数：");
	tempVal = data[offset++];
	tempVal <<= 8;
	tempVal |= data[offset++];

	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf, 0, 5);
	showDiagnosLen++;

	strcpy(showDiagnosBuf[showDiagnosLen], "info08 [AB]Bank2氧传感器监控遇到的次数：");
	tempVal = data[offset++];
	tempVal <<= 8;
	tempVal |= data[offset++];

	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf, 0, 5);
	showDiagnosLen++;

	strcpy(showDiagnosBuf[showDiagnosLen], "info08 [AB]ERG/VVT监控完成次数：");
	tempVal = data[offset++];
	tempVal <<= 8;
	tempVal |= data[offset++];

	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf, 0, 5);
	showDiagnosLen++;

	strcpy(showDiagnosBuf[showDiagnosLen], "info08 [AB]ERG/VVT监控遇到的次数：");
	tempVal = data[offset++];
	tempVal <<= 8;
	tempVal |= data[offset++];

	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf, 0, 5);
	showDiagnosLen++;

	strcpy(showDiagnosBuf[showDiagnosLen], "info08 [AB]空气监控完成次数：");
	tempVal = data[offset++];
	tempVal <<= 8;
	tempVal |= data[offset++];

	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf, 0, 5);
	showDiagnosLen++;

	strcpy(showDiagnosBuf[showDiagnosLen], "info08 [AB]空气监控遇到的次数：");
	tempVal = data[offset++];
	tempVal <<= 8;
	tempVal |= data[offset++];

	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf, 0, 5);
	showDiagnosLen++;

	strcpy(showDiagnosBuf[showDiagnosLen], "info08 [AB]EVAP监控完成次数：");
	tempVal = data[offset++];
	tempVal <<= 8;
	tempVal |= data[offset++];

	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf, 0, 5);
	showDiagnosLen++;

	strcpy(showDiagnosBuf[showDiagnosLen], "info08 [AB]EVAP监控遇到的次数：");
	tempVal = data[offset++];
	tempVal <<= 8;
	tempVal |= data[offset++];

	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf, 0, 5);
	showDiagnosLen++;
}

void analysis_sid09_tid09(char *data)
{
	int offset = 0;
	int tempVal = 0;
	char Numbuf[3] = {0};

	//第一个数据
	strcpy(showDiagnosBuf[showDiagnosLen], "info09 [A]ECUNAME消息个数：");
	tempVal = data[offset];

	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

	MessageCount9 = tempVal;
	
	showDiagnosLen++;
}

void analysis_sid09_tid0A(char *data)
{
	//第一个数据
	strcpy(showDiagnosBuf[showDiagnosLen], "info0A [20Bytes]Ecu-EcuName:");
	if(diagnose.commtypes == 0)
	{
		strcat(showDiagnosBuf[showDiagnosLen], &data[1]);
		strcat(showDiagnosBuf[showDiagnosLen], &data[5]);//补全后续名称
		showDiagnosLen++;
	}
	else
	{
		strcat(showDiagnosBuf[showDiagnosLen], &data[0]);
		strcat(showDiagnosBuf[showDiagnosLen], &data[4]);//补全后续名称
		showDiagnosLen++;
	}
}

void analysis_tid(char *rxbuf, int len)
{
	//未接收到数据
	if(len <= 0)
	{
		return ;
	}
	
	if(o2sensorDefine == 0)
	{
		switch(rxbuf[2])
		{
			case 0:
				strcpy(showDiagnosBuf[showDiagnosLen], "Bank1-Sensor1:");
				showDiagnosLen++;
				break;
			case 1:
				strcpy(showDiagnosBuf[showDiagnosLen], "Bank1-Sensor2:");
				showDiagnosLen++;
				break;
			case 2:
				strcpy(showDiagnosBuf[showDiagnosLen], "Bank1-Sensor3:");
				showDiagnosLen++;
				break;
			case 3:
				strcpy(showDiagnosBuf[showDiagnosLen], "Bank1-Sensor4:");
				showDiagnosLen++;
				break;
			case 4:
				strcpy(showDiagnosBuf[showDiagnosLen], "Bank2-Sensor1:");
				showDiagnosLen++;
				break;
			case 5:
				strcpy(showDiagnosBuf[showDiagnosLen], "Bank2-Sensor2:");
				showDiagnosLen++;
				break;
			case 6:
				strcpy(showDiagnosBuf[showDiagnosLen], "Bank2-Sensor3:");
				showDiagnosLen++;
				break;
			case 7:
				strcpy(showDiagnosBuf[showDiagnosLen], "Bank2-Sensor4:");
				showDiagnosLen++;
				break;
		}
	}
	else
	{
		switch(rxbuf[2])
		{
			case 0:
				strcpy(showDiagnosBuf[showDiagnosLen], "Bank1-Sensor1:");
				showDiagnosLen++;
				break;
			case 1:
				strcpy(showDiagnosBuf[showDiagnosLen], "Bank1-Sensor2:");
				showDiagnosLen++;
				break;
			case 2:
				strcpy(showDiagnosBuf[showDiagnosLen], "Bank2-Sensor1:");
				showDiagnosLen++;
				break;
			case 3:
				strcpy(showDiagnosBuf[showDiagnosLen], "Bank2-Sensor2:");
				showDiagnosLen++;
				break;
			case 4:
				strcpy(showDiagnosBuf[showDiagnosLen], "Bank3-Sensor1:");
				showDiagnosLen++;
				break;
			case 5:
				strcpy(showDiagnosBuf[showDiagnosLen], "Bank3-Sensor2:");
				showDiagnosLen++;
				break;
			case 6:
				strcpy(showDiagnosBuf[showDiagnosLen], "Bank4-Sensor1:");
				showDiagnosLen++;
				break;
			case 7:
				strcpy(showDiagnosBuf[showDiagnosLen], "Bank4-Sensor2:");
				showDiagnosLen++;
				break;
		}
	}
	
	switch(rxbuf[1])
	{
		case 1:
			analysis_sid05_tid01(&rxbuf[3]);
			break;
		case 2:
			analysis_sid05_tid02(&rxbuf[3]);
			break;
		case 3:
			analysis_sid05_tid03(&rxbuf[3]);
			break;
		case 4:
			analysis_sid05_tid04(&rxbuf[3]);
			break;
		case 5:
			analysis_sid05_tid05(&rxbuf[3]);
			break;
		case 6:
			analysis_sid05_tid06(&rxbuf[3]);
			break;
		case 7:
			analysis_sid05_tid07(&rxbuf[3]);
			break;
		case 8:
			analysis_sid05_tid08(&rxbuf[3]);
			break;
		case 9:
			analysis_sid05_tid09(&rxbuf[3]);
			break;
		case 0xA:
			analysis_sid05_tid0A(&rxbuf[3]);
			break;
	}
	
}

void analysis_infotypes(char *rxbuf, int len)
{
	//未接收到数据
	if(len <= 0)
	{
		return ;
	}
	
	switch((unsigned char)rxbuf[1])
	{
		case 0x01:
			analysis_sid09_tid01(&rxbuf[2]);
			break;
		case 0x02:
			analysis_sid09_tid02(&rxbuf[2]);
			break;
		case 0x03:
			analysis_sid09_tid03(&rxbuf[2]);
			break;
		case 0x04:
			analysis_sid09_tid04(&rxbuf[2]);
			break;
		case 0x05:
			analysis_sid09_tid05(&rxbuf[2]);
			break;
		case 0x06:
			analysis_sid09_tid06(&rxbuf[2]);
			break;
		case 0x07:
			analysis_sid09_tid07(&rxbuf[2]);
			break;
		case 0x08:
			analysis_sid09_tid08(&rxbuf[2]);
			break;
		case 0x09:
			analysis_sid09_tid09(&rxbuf[2]);
			break;
		case 0x0A:
			analysis_sid09_tid0A(&rxbuf[2]);
			break;
	}
}

//氧传感器检测结果查询
int Query_o2sensor_location(void)
{
	int ret = 0;
	int i = 0;
	unsigned char subBuf[256] = {0};
	char subLen = 0;
	int subid = 0;
	char rxbuf[100] = {0};
	int rxlen = 0;
	int o2sensorBe = 0;
	
	diagnose.server_id = 0x01;
	ret = uds_service_request(&diagnose, (char *)subBuf, &subLen);
	if(0 != ret)
	{
		return ret;//查询失败
	}
	
	#if 0
		subLen = 1;
		subBuf[0] = 0x13;
	#endif
	for(i = 0; i < subLen; i++)
	{
		subid = subBuf[i];
		if(subid == 0x13
			|| subid == 0x1D)
		{
			o2sensorBe = 1;
			if(diagnose.commtypes)//K线通讯方式
			{
				diagnose.server_id = 0x05;
				if(0 == kwp_service_sid(0x01, subid, (unsigned char *)rxbuf, &rxlen))
				{//解析响应请求
					if(subid== 0x13)
					{
						analysis_sid0102_pid13(&rxbuf[2]);
						for(i = 0; i < o2sensorNumber; i++)
						{
							o2sensorLocation = o2sensorLocationBuf[i];
							//请求氧传感器监测结果
							KwpQueryPidServer(0x05);
							if(ret != 0)
							{
								analysis_NegativeResponseCode(ret);
							}
						}
					}
					else if(subid == 0x1D)
					{
						analysis_sid0102_pid1D(&rxbuf[2]);
						for(i = 0; i < o2sensorNumber; i++)
						{
							o2sensorLocation = o2sensorLocationBuf[i];
							//请求氧传感器监测结果
							ret = KwpQueryPidServer(0x05);
							if(ret != 0)
							{
								analysis_NegativeResponseCode(ret);
							}
						}
					}
				}
				else
				{
					#if 0
						rxbuf[0] = 0x41;
						rxbuf[1] = 0x13;
						rxbuf[2] = 0x21;
						rxlen = 3;
						analysis_sid0102_pid13(&rxbuf[2]);
						for(i = 0; i < o2sensorNumber; i++)
						{
							o2sensorLocation = o2sensorLocationBuf[i];
							//请求氧传感器监测结果
							ret = KwpQueryPidServer(0x05);
							if(ret != 0)
							{
								rxbuf[0] = 0x45;
								rxbuf[1] = 0x01;
								rxbuf[2] = o2sensorLocation;
								rxbuf[3] = 0x5A;
								rxbuf[4] = 0x5A;
								rxbuf[5] = 0x5A;
								analysis_tid(rxbuf, rxlen);
							}
						}
					#endif
				}
			}
			else
			{
				strcpy(showDiagnosBuf[showDiagnosLen], "ISO 15765不支持查询05服务");
				showDiagnosLen++;
				return ret;//查询失败
			}
		}
	}
	
	if(o2sensorBe == 0)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "未收到氧传感器位置");
		showDiagnosLen++;
	}
	
	return ret;
}

//三信电子
//多项查询
int KwpQueryPidServer(unsigned char sid)
{
	//received buf
	unsigned char subBuf[256] = {0};
	char rxbuf[100] = {0};
	//received len
	char subLen = 0;
	char tempsid = 0;
	int rxlen = 0;
	int subid = 0;
	int i = 0;
	int ret;
	
	tempsid = sid;
	//查询子功能服务指令
	ret = uds_service_request(&diagnose, (char *)subBuf, &subLen);
	if(0 != ret)
	{
		return ret;//查询失败
	}
	
	vTaskDelay(100);
	
	//发送多个请求指令
	for(i = 0; i < subLen; i++)
	{
		subid = subBuf[i];
		{
			if(0 == can_service_sid(tempsid, subid, (unsigned char *)rxbuf, &rxlen))
			{//解析响应请求
				switch(tempsid)
				{
					case 0x01:
					case 0x02:
						analysis_pid(rxbuf, rxlen);
						break;
					case 0x05://无该服务
						break;
					case 0x06:
						analysis_sid06_OBDMID(&rxbuf[1], rxlen);
						break;
					case 0x08:
						break;
					case 0x09:
						analysis_infotypes(rxbuf, rxlen);
						break;
				}	
			}
			else
			{//查询该PID失败 未做处理
				
			}
			vTaskDelay(10);
		}
	}
	return 0;
}

//解析
void analysis_pid(char* rxbuf, int len)
{
	int i = 0;
	//未接收到数据
	if (len <= 0)
	{
		return;
	}
	if (rxbuf[0] == 0x41)
	{//服务1
		i = 0;
	}
	else if (rxbuf[0] == 0x42)
	{//服务2
		i = 1;
	}
	//解析回复信息
	switch ((unsigned char)rxbuf[1])
	{
	case 0x01:
		analysis_sid0102_pid01((char*)& rxbuf[2 + i]);
		break;
	case 0x02:
		analysis_sid0102_pid02((char*)& rxbuf[2 + i]);
		break;
	case 0x03:
		analysis_sid0102_pid03((char*)& rxbuf[2 + i]);
		break;
	case 0x04:
		analysis_sid0102_pid04((char*)& rxbuf[2 + i]);
		break;
	case 0x05:
		analysis_sid0102_pid05((char*)& rxbuf[2 + i]);
		break;
	case 0x06:
		analysis_sid0102_pid06((char*)& rxbuf[2 + i]);
		break;
	case 0x07:
		analysis_sid0102_pid07((char*)& rxbuf[2 + i]);
		break;
	case 0x08:
		analysis_sid0102_pid08((char*)& rxbuf[2 + i]);
		break;
	case 0x09:
		analysis_sid0102_pid09((char*)& rxbuf[2 + i]);
		break;
	case 0x0a:
		analysis_sid0102_pid0A((char*)& rxbuf[2 + i]);
		break;
	case 0x0b:
		analysis_sid0102_pid0B((char*)& rxbuf[2 + i]);
		break;
	case 0x0c:
		analysis_sid0102_pid0C((char*)& rxbuf[2 + i]);
		break;
	case 0x0d:
		analysis_sid0102_pid0D((char*)& rxbuf[2 + i]);
		break;
	case 0x0e:
		analysis_sid0102_pid0E((char*)& rxbuf[2 + i]);
		break;
	case 0x0f:
		analysis_sid0102_pid0F((char*)& rxbuf[2 + i]);
		break;
	case 0x10:
		analysis_sid0102_pid10((char*)& rxbuf[2 + i]);
		break;
	case 0x11:
		analysis_sid0102_pid11((char*)& rxbuf[2 + i]);
		break;
	case 0x12:
		analysis_sid0102_pid12((char*)& rxbuf[2 + i]);
		break;
	case 0x13:
		analysis_sid0102_pid13((char*)& rxbuf[2 + i]);
		break;
	case 0x14:
		len = len - 2 - i;
		analysis_sid0102_pid14((char*)& rxbuf[2 + i], len);
		break;
	case 0x15:
		len = len - 2 - i;
		analysis_sid0102_pid15((char*)& rxbuf[2 + i], len);
		break;
	case 0x16:
		len = len - 2 - i;
		analysis_sid0102_pid16((char*)& rxbuf[2 + i], len);
		break;
	case 0x17:
		len = len - 2 - i;
		analysis_sid0102_pid17((char*)& rxbuf[2 + i], len);
		break;
	case 0x18:
		len = len - 2 - i;
		analysis_sid0102_pid18((char*)& rxbuf[2 + i], len);
		break;
	case 0x19:
		len = len - 2 - i;
		analysis_sid0102_pid19((char*)& rxbuf[2 + i], len);
		break;
	case 0x1a:
		len = len - 2 - i;
		analysis_sid0102_pid1A((char*)& rxbuf[2 + i], len);
		break;
	case 0x1b:
		len = len - 2 - i;
		analysis_sid0102_pid1B((char*)& rxbuf[2 + i], len);
		break;
	case 0x1c:
		analysis_sid0102_pid1C((char*)& rxbuf[2 + i]);
		break;
	case 0x1d:
		analysis_sid0102_pid1D((char*)& rxbuf[2 + i]);
		break;
	case 0x1e:
		analysis_sid0102_pid1E((char*)& rxbuf[2 + i]);
		break;
	case 0x1f:
		analysis_sid0102_pid1F((char*)& rxbuf[2 + i]);
		break;
	case 0x21:
		analysis_sid0102_pid21((char*)& rxbuf[2 + i]);
		break;
	case 0x22:
		analysis_sid0102_pid22((char*)& rxbuf[2 + i]);
		break;
	case 0x23:
		analysis_sid0102_pid23((char*)& rxbuf[2 + i]);
		break;
	case 0x24:
		analysis_sid0102_pid24((char*)& rxbuf[2 + i]);
		break;
	case 0x25:
		analysis_sid0102_pid25((char*)& rxbuf[2 + i]);
		break;
	case 0x26:
		analysis_sid0102_pid26((char*)& rxbuf[2 + i]);
		break;
	case 0x27:
		analysis_sid0102_pid27((char*)& rxbuf[2 + i]);
		break;
	case 0x28:
		analysis_sid0102_pid28((char*)& rxbuf[2 + i]);
		break;
	case 0x29:
		analysis_sid0102_pid29((char*)& rxbuf[2 + i]);
		break;
	case 0x2a:
		analysis_sid0102_pid2A((char*)& rxbuf[2 + i]);
		break;
	case 0x2b:
		analysis_sid0102_pid2B((char*)& rxbuf[2 + i]);
		break;
	case 0x2c:
		analysis_sid0102_pid2C((char*)& rxbuf[2 + i]);
		break;
	case 0x2d:
		analysis_sid0102_pid2D((char*)& rxbuf[2 + i]);
		break;
	case 0x2e:
		analysis_sid0102_pid2E((char*)& rxbuf[2 + i]);
		break;
	case 0x2f:
		analysis_sid0102_pid2F((char*)& rxbuf[2 + i]);
		break;
	case 0x30:
		analysis_sid0102_pid30((char*)& rxbuf[2 + i]);
		break;
	case 0x31:
		analysis_sid0102_pid31((char*)& rxbuf[2 + i]);
		break;
	case 0x32:
		analysis_sid0102_pid32((char*)& rxbuf[2 + i]);
		break;
	case 0x33:
		analysis_sid0102_pid33((char*)& rxbuf[2 + i]);
		break;
	case 0x34:
		analysis_sid0102_pid34((char*)& rxbuf[2 + i]);
		break;
	case 0x35:
		analysis_sid0102_pid35((char*)& rxbuf[2 + i]);
		break;
	case 0x36:
		analysis_sid0102_pid36((char*)& rxbuf[2 + i]);
		break;
	case 0x37:
		analysis_sid0102_pid37((char*)& rxbuf[2 + i]);
		break;
	case 0x38:
		analysis_sid0102_pid38((char*)& rxbuf[2 + i]);
		break;
	case 0x39:
		analysis_sid0102_pid39((char*)& rxbuf[2 + i]);
		break;
	case 0x3a:
		analysis_sid0102_pid3A((char*)& rxbuf[2 + i]);
		break;
	case 0x3b:
		analysis_sid0102_pid3B((char*)& rxbuf[2 + i]);
		break;
	case 0x3c:
		analysis_sid0102_pid3C((char*)& rxbuf[2 + i]);
		break;
	case 0x3d:
		analysis_sid0102_pid3D((char*)& rxbuf[2 + i]);
		break;
	case 0x3e:
		analysis_sid0102_pid3E((char*)& rxbuf[2 + i]);
		break;
	case 0x3f:
		analysis_sid0102_pid3F((char*)& rxbuf[2 + i]);
		break;
	case 0x41:
		analysis_sid0102_pid41((char*)& rxbuf[2 + i]);
		break;
	case 0x42:
		analysis_sid0102_pid42((char*)& rxbuf[2 + i]);
		break;
	case 0x43:
		analysis_sid0102_pid43((char*)& rxbuf[2 + i]);
		break;
	case 0x44:
		analysis_sid0102_pid44((char*)& rxbuf[2 + i]);
		break;
	case 0x45:
		analysis_sid0102_pid45((char*)& rxbuf[2 + i]);
		break;
	case 0x46:
		analysis_sid0102_pid46((char*)& rxbuf[2 + i]);
		break;
	case 0x47:
		analysis_sid0102_pid47((char*)& rxbuf[2 + i]);
		break;
	case 0x48:
		analysis_sid0102_pid48((char*)& rxbuf[2 + i]);
		break;
	case 0x49:
		analysis_sid0102_pid49((char*)& rxbuf[2 + i]);
		break;
	case 0x4a:
		analysis_sid0102_pid4A((char*)& rxbuf[2 + i]);
		break;
	case 0x4b:
		analysis_sid0102_pid4B((char*)& rxbuf[2 + i]);
		break;
	case 0x4c:
		analysis_sid0102_pid4C((char*)& rxbuf[2 + i]);
		break;
	case 0x4d:
		analysis_sid0102_pid4D((char*)& rxbuf[2 + i]);
		break;
	case 0x4e:
		analysis_sid0102_pid4E((char*)& rxbuf[2 + i]);
		break;
	case 0x4f:
		analysis_sid0102_pid4F((char*)& rxbuf[2 + i]);
		break;
	case 0x50:
		analysis_sid0102_pid50((char*)& rxbuf[2 + i]);
		break;
	case 0x51:
		analysis_sid0102_pid51((char*)& rxbuf[2 + i]);
		break;
	case 0x52:
		analysis_sid0102_pid52((char*)& rxbuf[2 + i]);
		break;
	case 0x53:
		analysis_sid0102_pid53((char*)& rxbuf[2 + i]);
		break;
	case 0x54:
		analysis_sid0102_pid54((char*)& rxbuf[2 + i]);
		break;
	case 0x55:
		analysis_sid0102_pid55((char*)& rxbuf[2 + i]);
		break;
	case 0x56:
		analysis_sid0102_pid56((char*)& rxbuf[2 + i]);
		break;
	case 0x57:
		analysis_sid0102_pid57((char*)& rxbuf[2 + i]);
		break;
	case 0x58:
		analysis_sid0102_pid58((char*)& rxbuf[2 + i]);
		break;
	case 0x59:
		analysis_sid0102_pid59((char*)& rxbuf[2 + i]);
		break;
	case 0x5a:
		analysis_sid0102_pid5A((char*)& rxbuf[2 + i]);
		break;
	case 0xFD:
		analysis_sid0102_pidFD((char*)& rxbuf[2 + i]);
		break;
	case 0xFE:
		analysis_sid0102_pidFE((char*)& rxbuf[2 + i]);
		break;
	case 0xFF:
		analysis_sid0102_pidFF((char*)& rxbuf[2 + i]);
		break;
	default:
		break;
	}
}


