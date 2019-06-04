
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
char dtcPath[100] = {0};	//DTC�ļ���·��
char subfbuf[255];				//�����б�
char subflen;							//�������
char sendFailedCnt = 0;

char o2sensorDefine = 0;					//������� 0=PID13 - 1=PID1D
char o2sensorLocationBuf[4] = {0};			//��������λ��
char o2sensorLocation = 0;				//��������λ��
char o2sensorNumber = 0;					//������������

static int 	MessageCount1 = 0;
static int 	MessageCount3 = 0;
static int 	MessageCount5 = 0;
static int 	MessageCount7 = 0;
static int 	MessageCount9 = 0;

extern int KwpRequest(char *tx_buf, int tx_len, char *rx_buf, int *rx_len, void *param);

int frameNum = 0;

//ASCII�ַ� ת Int����
unsigned char HexCharToInt(unsigned char c)
{
	if (c >= 97 && c <= 102)
		return (c - 97) + 10;
	else if(c >= 65 && c <= 70)
		return (c - 65) + 10;
	else if (c >= 48 && c <= 57)
		return (c - 48);

	return 0xFF;//���ַ�����
}

//0xXX ASCII�ַ� ת 0xXX����  ת�����ƣ�ת������ 0x��ʼ  '/r'��β
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

//ASCII�ַ�תCHAR������
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

//��ȡ�����ļ� �����������ļ�
int parseCofigFile(char *path, DiagnoseStr *diaInfo)
{
	//���ļ�
	char tempbuf[128];
	uint8_t index = 0;
	uint8_t num = 0;

	num = SD_Read(path, tempbuf, index, 128);		//��ȡ����
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

//��ȡ����ID��---��ȡ�ַ������
char getServerID(char *sidname)//ID���ַ���
{
	char idbuf[3];

	memcpy(idbuf, sidname, 2);
	return StringToNum(idbuf,16);
}


//char�� ת�ַ��� ��д
unsigned char Num2ASCII(unsigned char c)
{
	if (c > 9)
		return (c + 87);//87 Сд  //55��д
	else
		return (c + 0x30);
}

//int���� ת �ַ�����ʾ ����ʾ�������� ʮ�����ƽ�����ʾ��д
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
		dec[i] = Num2ASCII((c/temp)); //�õ����λ
		c %= temp;
	}

	return bits;
}

//ת�������ַ��� ���ַ���+P�ַ�
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

//�ַ����� ת INT������
void rxbuf2dtcbuf(int *dtcbuf, unsigned short *dtclen, unsigned char *rxbuf, unsigned short rxlen, unsigned char isotype)
{
	uint16_t offset = 0;
	uint16_t i = 0;

	if(isotype == 0)
	{
		while(rxlen > 0)//������
		{
			dtcbuf[i] = rxbuf[offset]<<8;			//DTC M
			dtcbuf[i] |= rxbuf[offset+1];			//DTC L
			offset += 2;
			i += 1;
			if(rxlen >= 2)
				rxlen -= 2;
			else
				rxlen = 0;//�������һ���ֽ�
		}
			*dtclen = i;
	}

	return;
}

//�ַ����� ת �ַ���������ʾ?
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
		checknum = IntToChar(dtcdata[i], showDiagnosBuf[i+1], protocol);//ת��Ϊ�ַ���
	}

	do{
		ret = SD_Read(path,rxbuf,index,70);	//��ȡDTC�����б�
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
				{//��֤ʧ��
					if(j != dtclen+1)
						j++;
					else
						break;
				}
			}while(1);
		}
	}while(ret != 0);

	showDiagnosLen = len+1;//��ʾ����

	IntToSysChar(len, displaylen, 10);
	strcpy(showDiagnosBuf[0], "���������Ϊ��");
	strcat(showDiagnosBuf[0], displaylen);
	return ret;
}

void analysis_NegativeResponseCode(int code)
{
	switch(code)
	{
		case 0x01:
			strcpy(showDiagnosBuf[showDiagnosLen], "����ʧ��,������·�Ƿ���ϣ�");
			showDiagnosLen++;
			break;
		case 0x02:
			strcpy(showDiagnosBuf[showDiagnosLen], "У��ʧ��");
			showDiagnosLen++;
			break;
		case 0x10:
			strcpy(showDiagnosBuf[showDiagnosLen], "�ܾ���ͻ����ṩ����");
			showDiagnosLen++;
			break;
		case 0x11:
			strcpy(showDiagnosBuf[showDiagnosLen], "���������ÿͻ����������Ϸ���");
			showDiagnosLen++;
			break;
		case 0x12:
			strcpy(showDiagnosBuf[showDiagnosLen], "���������ÿͻ������������ӹ���");
			showDiagnosLen++;
			break;
		case 0x13:
			strcpy(showDiagnosBuf[showDiagnosLen], "��������Ϊ�ͻ��˵������ĵ����ݳ���(���߸�ʽ)�����ϱ���׼");
			showDiagnosLen++;
			break;
		case 0x21:
			strcpy(showDiagnosBuf[showDiagnosLen], "��������æ���޷�����ͻ��˷��������󡣴˷���Ӧ������Ϸ������");
			showDiagnosLen++;
			break;
		case 0x22:
			strcpy(showDiagnosBuf[showDiagnosLen], "������ִ����Ϸ��������������");
			showDiagnosLen++;
			break;
		case 0x23:
			break;
		case 0x24:
			strcpy(showDiagnosBuf[showDiagnosLen], "��������Ϊ��Ϸ��������(����ִ��)˳�����");
			showDiagnosLen++;
			break;
		case 0x31:
			strcpy(showDiagnosBuf[showDiagnosLen], "������û�пͻ�����������ݣ��˷���Ӧ�������������ݶ���д�����߸������ݵ������ܵķ�����");
			showDiagnosLen++;
			break;
		case 0x32:
			break;
		case 0x33:
			strcpy(showDiagnosBuf[showDiagnosLen], "��������ֹ�ͻ��˵�������Ϸ�������ԭ�������.. �������Ĳ�������������.. �������İ�ȫ״̬��������״̬");
			showDiagnosLen++;
			break;
		case 0x34:		 //
			break;
		case 0x35:
			strcpy(showDiagnosBuf[showDiagnosLen], "��������Ϊ�ͻ��˷��ص���Կ����");
			showDiagnosLen++;
			break;
		case 0x36:
			strcpy(showDiagnosBuf[showDiagnosLen], "��������Ϊ�ͻ��˳��԰�ȫ����(����)��ʧ�ܴ�������");
			showDiagnosLen++;
			break;
		case 0x37:
			strcpy(showDiagnosBuf[showDiagnosLen], "�������ܾ��ͻ��˵İ�ȫ����������Ϊ�����������������ļ�ʱ��δ��ʱ");
			showDiagnosLen++;
			break;
		case 0x70:
			strcpy(showDiagnosBuf[showDiagnosLen], "����������ĳ�ֹ��϶��ܾ��ͻ��˶Է������ڴ���ϴ�/���ز���");
			showDiagnosLen++;
			break;
		case 0x71:
			strcpy(showDiagnosBuf[showDiagnosLen], "����������ĳ�ֹ��϶���ֹ���������е����ݴ���");
			showDiagnosLen++;
			break;
		case 0x72:
			strcpy(showDiagnosBuf[showDiagnosLen], "�ٲ���������д����ʧ���ڴ�Ĺ����У����������ڷ��ִ������ֹ��Ϸ���");
			showDiagnosLen++;
			break;
		case 0x73:
			strcpy(showDiagnosBuf[showDiagnosLen], "���������ֿͻ��˵ķ�������(SID = 0x36)�����ĵ�blockSequenceCounter ��������");
			showDiagnosLen++;
			break;
		case 0x78:
			strcpy(showDiagnosBuf[showDiagnosLen], "��������ȷ���յ��ͻ��˷��͵��������ڴ����У�����δ�����꣬�˷���Ӧ�ķ���ʱ��Ӧ���㱾��׼��9.3.1��P2CAN_Server ��Ҫ�󣬲��ҷ�����Ӧ�ظ����ʹ˷���Ӧ��ֱ����ɲ�����");
			showDiagnosLen++;
			break;
		case 0x7e:
			strcpy(showDiagnosBuf[showDiagnosLen], "�ڵ�ǰ���ģʽ�£����������ÿͻ������������ӹ���");
			showDiagnosLen++;
			break;
		case 0x7f:
			strcpy(showDiagnosBuf[showDiagnosLen], "�ڵ�ǰ���ģʽ�£����������ÿͻ��������SID");
			showDiagnosLen++;
			break;
		default:
			strcpy(showDiagnosBuf[showDiagnosLen], "δ���յ���Ӧ��Ϣ");
			showDiagnosLen++;
			break;
	}
}


//ͨѶ��ʼ��
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
		USARTChangeBuand(USART1, diaInfo->Baud);//�ı䲨����

		if(inittype)
			return KwpFastInit(USART1,diaInfo->tgtAddr,diaInfo->funAddr);
		else
			return Kwp5BaudInit(USART1, diaInfo->funAddr, rxbuf, &rxlen);
	}
	
	return 0;
}

//K�߷��� ֡��ʽ
int kwpFmt(unsigned char *txbuf, int len)
{
	int offset = 0;
	//0xd6 2���ֽ�
	if((KB1&0x04) != 0)
	{//�Ƿ�����һ�ֽ�
//		txbuf[offset] &= 0x7F;	//�׵�ַ��һ��Ϊ0
		if((KB1&0x01) != 0)
		{//���ø�ʽ�ֽ��еĳ���
			txbuf[offset] = 0;
			txbuf[offset++] |= len;		//1�ֽ�
		}
		else if((KB1&0x02)!= 0)
		{//���ø��ӳ�����Ϣ
			txbuf[offset++] = 0;
			txbuf[offset++] = len;		//2�ֽ�
		}
		else
		{//��֪��
		}
	}
	else if((KB1&0x04) == 0)
	{//����һ�ֽ�
		if((KB1&0x02)!= 0)
		{//���ø��ӳ���
			txbuf[offset++] = 0;
			txbuf[offset++] = len;	//2�ֽ�
		}
		else if((KB1&0x08)!= 0 && (KB1&0x01) != 0)
		{//��ӵ�ַ�����ø�ʽ�ļ�����
			txbuf[offset++] |= len;
			txbuf[offset++] = diagnose.tgtAddr;
			txbuf[offset++] = diagnose.srcAddr;	//3�ֽ�
		}
		else
		{//��֪��
		}
	}
	return offset;
}


//����֧�ֵ��ӹ��ܷ���
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
		ret = 0;//����
	}
	else
	{
		ret = 1;
	}

	*rpNum = rpNumTemp;
	return ret;
}

//������ds �����Ϣ�ṹ�� idbuf ���֧�ֹ����� rpNum ֧�ֹ��������
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
		{//������ѯ 
			ret = uds_service_referSuppport(&commbuf[(rxlen-4)], idbuf, rpNum, referCmd);
			if(ret == 0)
			{
				referCmd += 0x20;
				vTaskDelay(10);
			}
			else
			{
				return 0;//��ѯ����
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

//�������
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
		return 0;//CAN��Ŀǰ�����������
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
					return 2;//ECU ��Ӧʧ��
				}
			}
		}
		else
		{
			if(againCnt++ >= 2)
			{
				againCnt = 0;
				return 1;//�˳�ѭ������
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
	

	//Դ��ַȷ��
	ret = UdsRequest1(diagnose.funAddr, 0, (unsigned char *)commbuf, txlen, rxbuf, (unsigned short *)rxlen);
	
	if(ret == 0)
	{
		for(i = 0; i < checklen; i++)
		{
			if(checkbuf[i] != rxbuf[i])
			{
				return 2;//У��ʧ��
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
	commbuf[txlen++] = 0x10;//Ŀ���ַ
	commbuf[txlen++] = 0xF1;//Դ��ַ
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
					return 1;//�������մ���
				}
			}
			else
			{
				return ret;
			}
		}
		else
		{
//			txlen = kwpFmt((unsigned char *)commbuf, 2);//ͷ����Ϣ
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
					return 1;//�������մ���
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

	//��һ������
	if(data[offset]&0x80)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid01 [Abit7] ����ָʾ�ƣ�ON");
	}
	else
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid01 [Abit7] ����ָʾ�ƣ�OFF");
	}
	showDiagnosLen++;
	DtcNum = data[offset]&0x7F;
	IntToSysChar(DtcNum, Numbuf, 10);
	strcpy(showDiagnosBuf[showDiagnosLen], "pid01 [Abits6-0] DTC������");
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	showDiagnosLen++;

	//DATAB
	offset++;	//��һ������

	if(data[offset]&0x01)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid01 [Bbit0]����ʧ����");
		if(data[offset]&0x10)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "��pid01 [Bbit4]���δ���");
		}
		else
		{
			strcat(showDiagnosBuf[showDiagnosLen], "��pid01 [Bbit4]������");
		}
	}
	else
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid01 [Bbit0]����ʧ����");
	}
	showDiagnosLen++;

	if(data[offset]&0x02)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid01 [Bbit1]����ȼ��ϵͳ���");
		if(data[offset]&0x20)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "��pid01 [Bbit5]���δ���");
		}
		else
		{
			strcat(showDiagnosBuf[showDiagnosLen], "��pid01 [Bbit5]������");
		}
	}
	else
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid01 [Bbit1]����ȼ��ϵͳ���");
	}
	showDiagnosLen++;

	if(data[offset]&0x04)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid01 [Bbit2]����ȫ��������");
		if(data[offset]&0x40)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "��pid01 [Bbit6]���δ���");
		}
		else
		{
			strcat(showDiagnosBuf[showDiagnosLen], "��pid01 [Bbit6]������");
		}
	}
	else
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid01 [Bbit2]����ȫ��������");
	}
	showDiagnosLen++;

	//DATAC
	offset++;	//��һ������
	if(data[offset]&0x01)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid01 [Cbit0]���ô߻������");
		if(data[offset+1]&0x01)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "��pid01 [Dbit0]���δ���");
		}
		else
		{
			strcat(showDiagnosBuf[showDiagnosLen], "��pid01 [Dbit0]������");
		}
	}
	else
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid01 [Cbit0]���ô߻������");
	}
	showDiagnosLen++;

	if(data[offset]&0x02)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid01 [Cbit1]���ü��ȴ߻������");
		if(data[offset+1]&0x02)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "��pid01 [Dbit1]���δ���");
		}
		else
		{
			strcat(showDiagnosBuf[showDiagnosLen], "��pid01 [Dbit1]������");
		}
	}
	else
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid01 [Cbit1]���ü��ȴ߻������");
	}
	showDiagnosLen++;

	if(data[offset]&0x04)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid01 [Cbit2]��������ϵͳ���");
		if(data[offset+1]&0x04)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "��pid01 [Dbit2]���δ���");
		}
		else
		{
			strcat(showDiagnosBuf[showDiagnosLen], "��pid01 [Dbit2]������");
		}
	}
	else
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid01 [Cbit2]��������ϵͳ���");
	}
	showDiagnosLen++;

	if(data[offset]&0x08)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid01 [Cbit3]���ö��ο���ϵͳ���");
		if(data[offset+1]&0x08)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "��pid01 [Dbit3]���δ���");
		}
		else
		{
			strcat(showDiagnosBuf[showDiagnosLen], "��pid01 [Dbit3]������");
		}
	}
	else
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid01 [Cbit3]���ö��ο���ϵͳ���");
	}
	showDiagnosLen++;

	if(data[offset]&0x10)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid01 [Cbit4]����A/Cϵͳ��������");
		if(data[offset+1]&0x10)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "��pid01 [Dbit4]���δ���");
		}
		else
		{
			strcat(showDiagnosBuf[showDiagnosLen], "��pid01 [Dbit4]������");
		}
	}
	else
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid01 [Dbit4]����A/Cϵͳ��������");
	}
	showDiagnosLen++;

	if(data[offset]&0x20)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid01 [Cbit5]���������������");
		if(data[offset+1]&0x20)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "��pid01 [Dbit5]���δ���");
		}
		else
		{
			strcat(showDiagnosBuf[showDiagnosLen], "��pid01 [Dbit5]������");
		}
	}
	else
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid01 [Cbit5]���������������");
	}
	showDiagnosLen++;

	if(data[offset]&0x40)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid01 [Cbit6]���������м��������");
		if(data[offset+1]&0x40)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "��pid01 [Dbit6]���δ���");
		}
		else
		{
			strcat(showDiagnosBuf[showDiagnosLen], "��pid01 [Dbit6]������");
		}
	}
	else
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid01 [Cbit6]���������м��������");
	}
	showDiagnosLen++;

	if(data[offset]&0x80)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid01 [Cbit7]����EGRϵͳ���");

		if(data[offset+1]&0x80)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "��pid01 [Dbit7]���δ���");
		}
		else
		{
			strcat(showDiagnosBuf[showDiagnosLen], "��pid01 [Dbit7]������");
		}
	}
	else
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid01 [Cbit7]����EGRϵͳ���");
	}
	showDiagnosLen++;
}

void analysis_sid0102_pid02(char *data)
{
	int DtcRxbuf[2];
	int dtclen = 0;

	//��һ������
	strcpy(showDiagnosBuf[showDiagnosLen], "pid02 [A]����֡��DTC��");
	showDiagnosLen++;

	rxbuf2dtcbuf(DtcRxbuf, (unsigned short *)&dtclen, (unsigned char *)data, 2, 0);

	AnalysisDtc(dtcPath, DtcRxbuf, dtclen, 0);
}

void analysis_sid0102_pid03(char *data)
{
	int offset = 0;

	//��һ������
	strcpy(showDiagnosBuf[showDiagnosLen], "pid03 [A]ȼ��ϵͳ1��");

	if(data[offset]&0x01)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid03 [Abit0]����-��δ�ﵽ�ջ�����");
		showDiagnosLen++;
	}

	if(data[offset]&0x02)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid03 [Abit1]�ջ�-ʹ������������Ϊȼ�Ͽ��Ƶķ���");
		showDiagnosLen++;
	}

	if(data[offset]&0x04)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid03 [Abit2]����-��������������e.g�������������ٶѻ���");
		showDiagnosLen++;
	}

	if(data[offset]&0x08)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid03 [Abit3]����-��⵽ϵͳ����");
		showDiagnosLen++;
	}

	if(data[offset]&0x10)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid03 [Abit4]�ջ�-����һ�������������ϣ�����ʹ�õ�һ��������ȼ�Ͽ��ƣ�");
		showDiagnosLen++;
	}

	if(0 == (data[offset]&0x1F))
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid03 [Abit5]����");
		showDiagnosLen++;
	}

	//DATAB
	offset++;	//��һ������
	strcpy(showDiagnosBuf[showDiagnosLen], "pid03 [B]ȼ��ϵͳ2��");

	if(data[offset]&0x01)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid03 [Bbit0]����-��δ�ﵽ�ջ�����");
		showDiagnosLen++;
	}

	if(data[offset]&0x02)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid03 [Bbit1]�ջ�-ʹ������������Ϊȼ�Ͽ��Ƶķ���");
		showDiagnosLen++;
	}

	if(data[offset]&0x04)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid03 [Bbit2]����-��������������e.g�������������ٶѻ���");
		showDiagnosLen++;
	}

	if(data[offset]&0x08)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid03 [Bbit3]����-��⵽ϵͳ����");
		showDiagnosLen++;
	}

	if(data[offset]&0x10)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid03 [Bbit4]�ջ�-����һ�������������ϣ�����ʹ�õ�һ��������ȼ�Ͽ��ƣ�");
		showDiagnosLen++;
	}

	if(0 == (data[offset]&0x1F))
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid03 [Bbit5]����");
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

	//��һ������
	strcpy(showDiagnosBuf[showDiagnosLen], "pid04 [A]��������");

	tempVal = data[offset];
	tempVal *= 1000;
	tempVal /= 255;

	//����
	tempVal_i = tempVal/10;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//С������
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

	strcpy(showDiagnosBuf[showDiagnosLen], "pid05 [A]��������ȴҺ�¶ȣ�");
	//��һ������
	tempVal = data[offset];
	//����
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
	strcat(showDiagnosBuf[showDiagnosLen], "��");

	showDiagnosLen++;
}

void analysis_sid0102_pid06(char *data)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[3] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "pid06 [A]����ȼ�ϵ���BANK1��");
	memset(Numbuf, '\0', 3);
	tempVal = data[offset];//bit
	//�Ƿ���
	if(tempVal < 128)
	{
		strcat(showDiagnosBuf[showDiagnosLen], "-");
		tempVal = 128 - tempVal;
	}
	else
	{
		tempVal -= 128;
	}

	tempVal *= 10000;//�Ŵ�10000��
	tempVal /= 128;
	//��������
	tempVal_i = tempVal/100;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);
	//С������
	tempVal_f = tempVal%100;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "%");
	showDiagnosLen++;

	if(o2sensorDefine != 0)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid06 [B]����ȼ�ϵ���BANK3��");
		memset(Numbuf, '\0', 3);
		tempVal = data[offset+1];//bit
		//�Ƿ���
		if(tempVal < 128)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "-");
			tempVal = 128 - tempVal;
		}
		else
		{
			tempVal -= 128;
		}

		tempVal *= 10000;//�Ŵ�10000��
		tempVal /= 128;
		//��������
		tempVal_i = tempVal/100;
		IntToSysChar(tempVal_i, Numbuf, 10);
		strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

		strcat(showDiagnosBuf[showDiagnosLen], ".");
		memset(Numbuf, '\0', 3);
		//С������
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

	strcpy(showDiagnosBuf[showDiagnosLen], "pid07 [A]����ȼ�ϵ���BANK1��");
	memset(Numbuf, '\0', 3);
	tempVal = data[offset];//bit
	//�Ƿ���
	if(tempVal < 128)
	{
		strcat(showDiagnosBuf[showDiagnosLen], "-");
		tempVal = 128 - tempVal;
	}
	else
	{
		tempVal -= 128;
	}

	tempVal *= 10000;//�Ŵ�10000��
	tempVal /= 128;
	//��������
	tempVal_i = tempVal/100;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);
	//С������
	tempVal_f = tempVal%100;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "%");
	showDiagnosLen++;

	if(o2sensorDefine != 0)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid07 [B]����ȼ�ϵ���BANK3��");
		memset(Numbuf, '\0', 3);
		tempVal = data[offset+1];//bit
		//�Ƿ���
		if(tempVal < 128)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "-");
			tempVal = 128 - tempVal;
		}
		else
		{
			tempVal -= 128;
		}

		tempVal *= 10000;//�Ŵ�10000��
		tempVal /= 128;
		//��������
		tempVal_i = tempVal/100;
		IntToSysChar(tempVal_i, Numbuf, 10);
		strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

		strcat(showDiagnosBuf[showDiagnosLen], ".");
		memset(Numbuf, '\0', 3);
		//С������
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

	strcpy(showDiagnosBuf[showDiagnosLen], "pid08 [A]����ȼ�ϵ���BANK2��");
	memset(Numbuf, '\0', 3);
	tempVal = data[offset];//bit
	//�Ƿ���
	if(tempVal < 128)
	{
		strcat(showDiagnosBuf[showDiagnosLen], "-");
		tempVal = 128 - tempVal;
	}
	else
	{
		tempVal -= 128;
	}

	tempVal *= 10000;//�Ŵ�10000��
	tempVal /= 128;
	//��������
	tempVal_i = tempVal/100;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);
	//С������
	tempVal_f = tempVal%100;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "%");
	showDiagnosLen++;

	if(o2sensorDefine != 0)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid08 [B]����ȼ�ϵ���BANK4��");
		memset(Numbuf, '\0', 3);
		tempVal = data[offset+1];//bit
		//�Ƿ���
		if(tempVal < 128)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "-");
			tempVal = 128 - tempVal;
		}
		else
		{
			tempVal -= 128;
		}

		tempVal *= 10000;//�Ŵ�10000��
		tempVal /= 128;
		//��������
		tempVal_i = tempVal/100;
		IntToSysChar(tempVal_i, Numbuf, 10);
		strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

		strcat(showDiagnosBuf[showDiagnosLen], ".");
		memset(Numbuf, '\0', 3);
		//С������
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

	strcpy(showDiagnosBuf[showDiagnosLen], "pid09 [A]����ȼ�ϵ���BANK2��");
	memset(Numbuf, '\0', 3);
	tempVal = data[offset];//bit
	//�Ƿ���
	if(tempVal < 128)
	{
		strcat(showDiagnosBuf[showDiagnosLen], "-");
		tempVal = 128 - tempVal;
	}
	else
	{
		tempVal -= 128;
	}

	tempVal *= 10000;//�Ŵ�10000��
	tempVal /= 128;
	//��������
	tempVal_i = tempVal/100;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);
	//С������
	tempVal_f = tempVal%100;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "%");
	showDiagnosLen++;
	
	if(o2sensorDefine != 0)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid09 [B]����ȼ�ϵ���BANK4��");
		memset(Numbuf, '\0', 3);
		tempVal = data[offset+1];//bit
		//�Ƿ���
		if(tempVal < 128)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "-");
			tempVal = 128 - tempVal;
		}
		else
		{
			tempVal -= 128;
		}

		tempVal *= 10000;//�Ŵ�10000��
		tempVal /= 128;
		//��������
		tempVal_i = tempVal/100;
		IntToSysChar(tempVal_i, Numbuf, 10);
		strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

		strcat(showDiagnosBuf[showDiagnosLen], ".");
		memset(Numbuf, '\0', 3);
		//С������
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

	strcpy(showDiagnosBuf[showDiagnosLen], "pid0A [A]��ѹ��");
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

	strcpy(showDiagnosBuf[showDiagnosLen], "pid0B [A]������ܾ���ѹ����");
	//��һ������
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

	strcpy(showDiagnosBuf[showDiagnosLen], "pid0C [AB]������ת�٣�");
	//��������
	tempVal = data[offset];
	tempVal <<= 8;
	tempVal |= data[offset+1];//bit

	//��������
	tempVal_i =  tempVal/4;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);
	//С������
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

	strcpy(showDiagnosBuf[showDiagnosLen], "pid0D [A]���٣�");
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

	strcpy(showDiagnosBuf[showDiagnosLen], "pid0E [A]1�����׵����ʱ��ǰ��");
	//��һ������
	tempVal = data[offset];//bit

	//�Ƿ���
	if(tempVal < 128)
	{
		strcat(showDiagnosBuf[showDiagnosLen], "-");
		tempVal = 128 - tempVal;
	}
	else
	{
		tempVal -= 128;
	}

	//��������
	tempVal_i =  tempVal/2;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);
	//С������
	tempVal_f = tempVal%2;
	tempVal_f *= 5;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "��");

	showDiagnosLen++;
}

void analysis_sid0102_pid0F(char *data)
{
	int offset = 0;
	int tempVal = 0;
	char Numbuf[3] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "pid0F [A]��������¶ȣ�");
	//��һ������
	tempVal = data[offset];
	//����
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
	strcat(showDiagnosBuf[showDiagnosLen], "��");

	showDiagnosLen++;
}

void analysis_sid0102_pid10(char *data)
{
	int offset = 0;
	int tempVal = 0;
	int temmVal_i = 0;
	int temmVal_f = 0;
	char Numbuf[5] = {0};

	//��һ������
	strcpy(showDiagnosBuf[showDiagnosLen], "pid10 [AB]����������");
	tempVal = data[offset];
	tempVal = (tempVal<<8) | data[offset+1];

	//��������
	temmVal_i = tempVal/100;
	IntToSysChar(temmVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 5);
	//С������
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

	strcpy(showDiagnosBuf[showDiagnosLen], "pid11 [A]���ԵĽ�����λ�ã�");
	//��һ������
	tempVal = data[offset];
	tempVal *= 1000;
	tempVal /= 255;

	//����
	tempVal_i = tempVal/10;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//С������
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

	strcpy(showDiagnosBuf[showDiagnosLen], "pid12 [A]��������״����");
	//��һ������
	tempVal = data[offset];

	if(tempVal & 0x01)
	{
		strcat(showDiagnosBuf[showDiagnosLen], "pid12 [Abit0]��һ�߻�ת��������");
	}
	else if(tempVal & 0x02)
	{
		strcat(showDiagnosBuf[showDiagnosLen], "pid12 [Abit1]��һ�߻�ת��������");
	}
	else if(tempVal & 0x04)
	{
		strcat(showDiagnosBuf[showDiagnosLen], "pid12 [Abit2]�����ر�");
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

	strcpy(showDiagnosBuf[showDiagnosLen], "pid13 [A]��������λ�ã�");
	//��һ������
	tempVal = data[offset];
	o2sensorNumber = 0;

	if(tempVal & 0x01)
	{
		showDiagnosLen++;
		strcat(showDiagnosBuf[showDiagnosLen], "pid13 [Abit0]��������1��BANK1��");
		o2sensorLocationBuf[o2sensorNumber++] = 0;
	}
	else if(tempVal & 0x10)
	{
		showDiagnosLen++;
		strcat(showDiagnosBuf[showDiagnosLen], "pid13 [Abit4]��������1��BANK2��");
		o2sensorLocationBuf[o2sensorNumber++] = 4;
	}

	if(tempVal & 0x02)
	{
		showDiagnosLen++;
		strcat(showDiagnosBuf[showDiagnosLen], "pid13 [Abit1]��������2��BANK1��");
		o2sensorLocationBuf[o2sensorNumber++] = 1;
	}
	else if(tempVal & 0x20)
	{
		showDiagnosLen++;
		strcat(showDiagnosBuf[showDiagnosLen], "pid13 [Abit5]��������2��BANK2��");
		o2sensorLocationBuf[o2sensorNumber++] = 5;
	}

	if(tempVal & 0x04)
	{
		showDiagnosLen++;
		strcat(showDiagnosBuf[showDiagnosLen], "pid13 [Abit2]��������3��BANK1��");
		o2sensorLocationBuf[o2sensorNumber++] = 2;
	}
	else if(tempVal & 0x40)
	{
		showDiagnosLen++;
		strcat(showDiagnosBuf[showDiagnosLen], "pid13 [Abit6]��������3��BANK2��");
		o2sensorLocationBuf[o2sensorNumber++] = 6;
	}

	if(tempVal & 0x08)
	{
		showDiagnosLen++;
		strcat(showDiagnosBuf[showDiagnosLen], "pid13 [Abit3]��������4��BANK1��");
		o2sensorLocationBuf[o2sensorNumber++] = 3;
	}
	else if(tempVal & 0x80)
	{
		showDiagnosLen++;
		strcat(showDiagnosBuf[showDiagnosLen], "pid13 [Abit7]��������4��BANK2��");
		o2sensorLocationBuf[o2sensorNumber++] = 7;
	}
	showDiagnosLen++;
	
	strcpy(showDiagnosBuf[showDiagnosLen], "pid13 [A]��������������");
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
	
	//��һ������
	strcpy(showDiagnosBuf[showDiagnosLen], "pid14 [A]BANK1��������1�����ѹ��");

	tempVal = data[offset];//bit
	tempVal *= 5;

	//����
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//С������
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "V");
	showDiagnosLen++;

	if(len != 1)
	{
		//�ڶ�������
		strcpy(showDiagnosBuf[showDiagnosLen], "pid14 [B]����ȼ��ƽ�⣺");
		memset(Numbuf, '\0', 3);
		tempVal = data[offset+1];//bit
		//�Ƿ���
		if(tempVal < 128)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "-");
			tempVal = 128 - tempVal;
		}
		else
		{
			tempVal -= 128;
		}

		tempVal *= 10000;//�Ŵ�10000��
		tempVal /= 128;
		//��������
		tempVal_i = tempVal/100;
		IntToSysChar(tempVal_i, Numbuf, 10);
		strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

		strcat(showDiagnosBuf[showDiagnosLen], ".");
		memset(Numbuf, '\0', 3);
		//С������
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

	//��һ������
	strcpy(showDiagnosBuf[showDiagnosLen], "pid15 [A]BANK1��������2�����ѹ��");

	tempVal = data[offset];//bit
	tempVal *= 5;

	//����
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//С������
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "V");
	showDiagnosLen++;

	if(len != 1)
	{
		//�ڶ�������
		strcpy(showDiagnosBuf[showDiagnosLen], "pid15 [B]����ȼ��ƽ�⣺");
		memset(Numbuf, '\0', 3);
		tempVal = data[offset+1];//bit
		//�Ƿ���
		if(tempVal < 128)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "-");
			tempVal = 128 - tempVal;
		}
		else
		{
			tempVal -= 128;
		}

		tempVal *= 10000;//�Ŵ�10000��
		tempVal /= 128;
		//��������
		tempVal_i = tempVal/100;
		IntToSysChar(tempVal_i, Numbuf, 10);
		strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

		strcat(showDiagnosBuf[showDiagnosLen], ".");
		memset(Numbuf, '\0', 3);
		//С������
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

	//��һ������
	if(o2sensorDefine == 0)
		strcpy(showDiagnosBuf[showDiagnosLen], "pid16 [A]BANK1��������3�����ѹ��");
	else
		strcpy(showDiagnosBuf[showDiagnosLen], "pid16 [A]BANK2��������1�����ѹ��");


	tempVal = data[offset];//bit
	tempVal *= 5;

	//����
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//С������
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "V");
	showDiagnosLen++;

	if(len != 1)
	{
		//�ڶ�������
		strcpy(showDiagnosBuf[showDiagnosLen], "pid16 [B]����ȼ��ƽ�⣺");
		memset(Numbuf, '\0', 3);
		tempVal = data[offset+1];//bit
		//�Ƿ���
		if(tempVal < 128)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "-");
			tempVal = 128 - tempVal;
		}
		else
		{
			tempVal -= 128;
		}

		tempVal *= 10000;//�Ŵ�10000��
		tempVal /= 128;
		//��������
		tempVal_i = tempVal/100;
		IntToSysChar(tempVal_i, Numbuf, 10);
		strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

		strcat(showDiagnosBuf[showDiagnosLen], ".");
		memset(Numbuf, '\0', 3);
		//С������
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

	//��һ������
	if(o2sensorDefine == 0)
		strcpy(showDiagnosBuf[showDiagnosLen], "pid17 [A]BANK1��������4�����ѹ��");
	else
		strcpy(showDiagnosBuf[showDiagnosLen], "pid16 [A]BANK2��������2�����ѹ��");

	tempVal = data[offset];//bit
	tempVal *= 5;

	//����
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//С������
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "V");
	showDiagnosLen++;

	if(len != 1)
	{
		//�ڶ�������
		strcpy(showDiagnosBuf[showDiagnosLen], "pid17 [B]����ȼ��ƽ�⣺");
		memset(Numbuf, '\0', 3);
		tempVal = data[offset+1];//bit
		//�Ƿ���
		if(tempVal < 128)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "-");
			tempVal = 128 - tempVal;
		}
		else
		{
			tempVal -= 128;
		}

		tempVal *= 10000;//�Ŵ�10000��
		tempVal /= 128;
		//��������
		tempVal_i = tempVal/100;
		IntToSysChar(tempVal_i, Numbuf, 10);
		strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

		strcat(showDiagnosBuf[showDiagnosLen], ".");
		memset(Numbuf, '\0', 3);
		//С������
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

	//��һ������
	if(o2sensorDefine == 0)
		strcpy(showDiagnosBuf[showDiagnosLen], "pid18 [A]BANK2��������1�����ѹ��");
	else
		strcpy(showDiagnosBuf[showDiagnosLen], "pid18 [A]BANK3��������1�����ѹ��");

	tempVal = data[offset];//bit
	tempVal *= 5;

	//����
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//С������
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "V");
	showDiagnosLen++;

	if(len != 1)
	{
		//�ڶ�������
		strcpy(showDiagnosBuf[showDiagnosLen], "pid18 [B]����ȼ��ƽ�⣺");
		memset(Numbuf, '\0', 3);
		tempVal = data[offset+1];//bit
		//�Ƿ���
		if(tempVal < 128)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "-");
			tempVal = 128 - tempVal;
		}
		else
		{
			tempVal -= 128;
		}

		tempVal *= 10000;//�Ŵ�10000��
		tempVal /= 128;
		//��������
		tempVal_i = tempVal/100;
		IntToSysChar(tempVal_i, Numbuf, 10);
		strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

		strcat(showDiagnosBuf[showDiagnosLen], ".");
		memset(Numbuf, '\0', 3);
		//С������
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

	//��һ������
	if(o2sensorDefine == 0)
		strcpy(showDiagnosBuf[showDiagnosLen], "pid19 [A]BANK2��������2�����ѹ��");
	else
		strcpy(showDiagnosBuf[showDiagnosLen], "pid19 [A]BANK3��������2�����ѹ��");

	tempVal = data[offset];//bit
	tempVal *= 5;

	//����
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//С������
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "V");
	showDiagnosLen++;
	
	if(len != 1)
	{
		//�ڶ�������
		strcpy(showDiagnosBuf[showDiagnosLen], "pid19 [B]����ȼ��ƽ�⣺");
		memset(Numbuf, '\0', 3);
		tempVal = data[offset+1];//bit
		//�Ƿ���
		if(tempVal < 128)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "-");
			tempVal = 128 - tempVal;
		}
		else
		{
			tempVal -= 128;
		}

		tempVal *= 10000;//�Ŵ�10000��
		tempVal /= 128;
		//��������
		tempVal_i = tempVal/100;
		IntToSysChar(tempVal_i, Numbuf, 10);
		strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

		strcat(showDiagnosBuf[showDiagnosLen], ".");
		memset(Numbuf, '\0', 3);
		//С������
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

	//��һ������
	if(o2sensorDefine == 0)
		strcpy(showDiagnosBuf[showDiagnosLen], "pid1A [A]BANK2��������3�����ѹ��");
	else
		strcpy(showDiagnosBuf[showDiagnosLen], "pid1A [A]BANK4��������1�����ѹ��");

	tempVal = data[offset];//bit
	tempVal *= 5;

	//����
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//С������
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "V");
	showDiagnosLen++;

	if(len != 1)
	{
		//�ڶ�������
		strcpy(showDiagnosBuf[showDiagnosLen], "pid1A [B]����ȼ��ƽ�⣺");
		memset(Numbuf, '\0', 3);
		tempVal = data[offset+1];//bit
		//�Ƿ���
		if(tempVal < 128)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "-");
			tempVal = 128 - tempVal;
		}
		else
		{
			tempVal -= 128;
		}

		tempVal *= 10000;//�Ŵ�10000��
		tempVal /= 128;
		//��������
		tempVal_i = tempVal/100;
		IntToSysChar(tempVal_i, Numbuf, 10);
		strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

		strcat(showDiagnosBuf[showDiagnosLen], ".");
		memset(Numbuf, '\0', 3);
		//С������
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

	//��һ������
	if(o2sensorDefine == 0)
		strcpy(showDiagnosBuf[showDiagnosLen], "pid1B [A]BANK2��������4�����ѹ��");
	else
		strcpy(showDiagnosBuf[showDiagnosLen], "pid1B [A]BANK4��������2�����ѹ��");

	tempVal = data[offset];//bit
	tempVal *= 5;

	//����
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//С������
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "V");
	showDiagnosLen++;

	if(len != 1)
	{
		//�ڶ�������
		strcpy(showDiagnosBuf[showDiagnosLen], "pid1B [B]����ȼ��ƽ�⣺");
		memset(Numbuf, '\0', 3);
		tempVal = data[offset+1];//bit
		//�Ƿ���
		if(tempVal < 128)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "-");
			tempVal = 128 - tempVal;
		}
		else
		{
			tempVal -= 128;
		}

		tempVal *= 10000;//�Ŵ�10000��
		tempVal /= 128;
		//��������
		tempVal_i = tempVal/100;
		IntToSysChar(tempVal_i, Numbuf, 10);
		strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

		strcat(showDiagnosBuf[showDiagnosLen], ".");
		memset(Numbuf, '\0', 3);
		//С������
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

	strcpy(showDiagnosBuf[showDiagnosLen], "pid1C [A]OBDҪ��������ƣ�");
	//��һ������
	tempVal = data[offset];

	switch(tempVal)
	{
		case 0x01:
			strcat(showDiagnosBuf[showDiagnosLen], "OBD ��");
			break;
		case 0x02:
			strcat(showDiagnosBuf[showDiagnosLen], "OBD");
			break;
		case 0x03:
			strcat(showDiagnosBuf[showDiagnosLen], "OBD and OBD ��");
			break;
		case 0x04:
			strcat(showDiagnosBuf[showDiagnosLen], "OBD ��");
			break;
		case 0x05:
			strcat(showDiagnosBuf[showDiagnosLen], "NO OBD");
			break;
		case 0x06:
			strcat(showDiagnosBuf[showDiagnosLen], "EOBD");
			break;
		case 0x07:
			strcat(showDiagnosBuf[showDiagnosLen], "EOBD and OBD ��");
			break;
		case 0x08:
			strcat(showDiagnosBuf[showDiagnosLen], "EOBD and OBD");
			break;
		case 0x09:
			strcat(showDiagnosBuf[showDiagnosLen], "EOBD,OBD and OBD ��");
			break;
		case 0x0a:
			strcat(showDiagnosBuf[showDiagnosLen], "JOBD");
			break;
		case 0x0b:
			strcat(showDiagnosBuf[showDiagnosLen], "JOBD and OBD ��");
			break;
		case 0x0c:
			strcat(showDiagnosBuf[showDiagnosLen], "JOBD and EOBD");
			break;
		case 0x0d:
			strcat(showDiagnosBuf[showDiagnosLen], "JOBD,EOBD,and OBD ��");
			break;
		case 0x0e:
			strcat(showDiagnosBuf[showDiagnosLen], "EURO �� B1");
			break;
		case 0x0f:
			strcat(showDiagnosBuf[showDiagnosLen], "EURO �� B2");
			break;
		case 0x10:
			strcat(showDiagnosBuf[showDiagnosLen], "EURO �� C");
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

	strcpy(showDiagnosBuf[showDiagnosLen], "pid1D [A]��������λ�ã�");

	//��һ������
	tempVal = data[offset];
	o2sensorNumber = 0;

	if(tempVal & 0x01)
	{
		showDiagnosLen++;
		strcat(showDiagnosBuf[showDiagnosLen], "pid1D [Abit0]��������1��BANK1��");
		o2sensorLocationBuf[o2sensorNumber++] = 0;
	}
	else if(tempVal & 0x10)
	{
		showDiagnosLen++;
		strcat(showDiagnosBuf[showDiagnosLen], "pid1D [Abit4]��������1��BANK3��");
		o2sensorLocationBuf[o2sensorNumber++] = 4;
	}
	else if(tempVal & 0x04)
	{
		showDiagnosLen++;
		strcat(showDiagnosBuf[showDiagnosLen], "pid1D [Abit2]��������1��BANK2��");
		o2sensorLocationBuf[o2sensorNumber++] = 2;
	}
	else if(tempVal & 0x40)
	{
		showDiagnosLen++;
		strcat(showDiagnosBuf[showDiagnosLen], "pid1D [Abit6]��������1��BANK4��");
		o2sensorLocationBuf[o2sensorNumber++] = 6;
	}

	if(tempVal & 0x02)
	{
		showDiagnosLen++;
		strcat(showDiagnosBuf[showDiagnosLen], "pid1D [Abit1]��������2��BANK1��");
		o2sensorLocationBuf[o2sensorNumber++] = 1;
	}
	else if(tempVal & 0x20)
	{
		showDiagnosLen++;
		strcat(showDiagnosBuf[showDiagnosLen], "pid1D [Abit5]��������2��BANK3��");
		o2sensorLocationBuf[o2sensorNumber++] = 5;
	}
	else if(tempVal & 0x08)
	{
		showDiagnosLen++;
		strcat(showDiagnosBuf[showDiagnosLen], "pid1D [Abit3]��������2��BANK2��");
		o2sensorLocationBuf[o2sensorNumber++] = 3;
	}
	else if(tempVal & 0x80)
	{
		showDiagnosLen++;
		strcat(showDiagnosBuf[showDiagnosLen], "pid1D [Abit7]��������2��BANK4��");
		o2sensorLocationBuf[o2sensorNumber++] = 7;
	}
	showDiagnosLen++;
	
	strcpy(showDiagnosBuf[showDiagnosLen], "pid1D [A]��������������");
	tempVal = o2sensorNumber;
	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	showDiagnosLen++;
}

void analysis_sid0102_pid1E(char *data)
{

	int offset = 0;
	int tempVal = 0;

	strcpy(showDiagnosBuf[showDiagnosLen], "pid1E [Abit0]��������״̬PTO��");

	tempVal = data[offset];

	if(tempVal & 0x01)
		strcat(showDiagnosBuf[showDiagnosLen], "����");
	else
		strcat(showDiagnosBuf[showDiagnosLen], "�ر�");

	showDiagnosLen++;
}

void analysis_sid0102_pid1F(char *data)
{
	int offset = 0;
	int tempVal = 0;
	char Numbuf[5] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "pid1E [AB]����ʱ�䣺");
	//��һ������
	tempVal = data[offset];
	tempVal <<= 8;
	tempVal |= data[offset+1];//bit

	//��������
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

	strcpy(showDiagnosBuf[showDiagnosLen], "pid21 [AB]��mil������ʱ�����ƶ���");
	//��һ������
	tempVal = data[offset];
	tempVal <<= 8;
	tempVal |= data[offset+1];//bit

	//��������
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

	strcpy(showDiagnosBuf[showDiagnosLen], "pid22 [AB]ȼ�Ϲ�ѹ������ڹܵ���գ�");
	//��һ������
	tempVal = data[offset];
	tempVal <<= 8;
	tempVal |= data[offset+1];//bit

	tempVal *= 79;

	//��������
	tempVal_i =tempVal / 1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, 0, 5);

	//С������
	tempVal_f = tempVal / 1000;

	//��������
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

	strcpy(showDiagnosBuf[showDiagnosLen], "pid23 [AB]�͹�ѹ����");
	//��һ������
	tempVal = data[offset];
	tempVal <<= 8;
	tempVal |= data[offset+1];//bit

	tempVal *= 79;

	//��������
	tempVal_i =tempVal / 1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, 0, 5);

	//С������
	tempVal_f = tempVal / 1000;

	//��������
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

	strcpy(showDiagnosBuf[showDiagnosLen], "pid24 BANK1 ��������1��");
	showDiagnosLen++;

	//��һ������
	strcpy(showDiagnosBuf[showDiagnosLen], "[A]��Ч�ȣ�");

	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];

	tempVal *= 305;

	//����
	tempVal_i = tempVal/10000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//С������
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%10000000;
	tempVal_f /= 1000;

	//��������
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

	//�ڶ�������
	strcpy(showDiagnosBuf[showDiagnosLen], "[B]��ѹ��");
	memset(Numbuf, '\0', 3);
	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];

	tempVal *= 122;

	//����
	tempVal_i = tempVal/1000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//С������
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%1000000;
	tempVal_f /= 1000;

	//�������� ���⴦�����ֵ
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

	//��һ������
	strcpy(showDiagnosBuf[showDiagnosLen], "pid25 BANK1��������2��");
	showDiagnosLen++;

	//��һ������
	strcpy(showDiagnosBuf[showDiagnosLen], "[A]��Ч�ȣ�");

	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];

	tempVal *= 305;

	//����
	tempVal_i = tempVal/10000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//С������
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%10000000;
	tempVal_f /= 1000;

	//��������
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

	//�ڶ�������
	strcpy(showDiagnosBuf[showDiagnosLen], "[B]��ѹ��");
	memset(Numbuf, '\0', 3);
	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];

	tempVal *= 122;

	//����
	tempVal_i = tempVal/1000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//С������
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%1000000;
	tempVal_f /= 1000;

	//�������� ���⴦�����ֵ
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

	//��һ������
	if(o2sensorDefine == 0)
		strcpy(showDiagnosBuf[showDiagnosLen], "pid26 BANK1��������3�����ѹ��");
	else
		strcpy(showDiagnosBuf[showDiagnosLen], "pid26 BANK2��������1�����ѹ��");

	showDiagnosLen++;

	//��һ������
	strcpy(showDiagnosBuf[showDiagnosLen], "[A]��Ч�ȣ�");

	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];

	tempVal *= 305;

	//����
	tempVal_i = tempVal/10000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//С������
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%10000000;
	tempVal_f /= 1000;

	//��������
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

	//�ڶ�������
	strcpy(showDiagnosBuf[showDiagnosLen], "[B]��ѹ��");
	memset(Numbuf, '\0', 3);
	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];

	tempVal *= 122;

	//����
	tempVal_i = tempVal/1000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//С������
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%1000000;
	tempVal_f /= 1000;

	//�������� ���⴦�����ֵ
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

	//��һ������
	if(o2sensorDefine == 0)
		strcpy(showDiagnosBuf[showDiagnosLen], "pid27 BANK1��������4�����ѹ��");
	else
		strcpy(showDiagnosBuf[showDiagnosLen], "pid27 BANK2��������2�����ѹ��");

	showDiagnosLen++;

	//��һ������
	strcpy(showDiagnosBuf[showDiagnosLen], "[A]��Ч�ȣ�");

	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];

	tempVal *= 305;

	//����
	tempVal_i = tempVal/10000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//С������
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%10000000;
	tempVal_f /= 1000;

	//��������
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

	//�ڶ�������
	strcpy(showDiagnosBuf[showDiagnosLen], "[B]��ѹ��");
	memset(Numbuf, '\0', 3);
	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];

	tempVal *= 122;

	//����
	tempVal_i = tempVal/1000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//С������
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%1000000;
	tempVal_f /= 1000;

	//�������� ���⴦�����ֵ
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

	//��һ������
	if(o2sensorDefine == 0)
		strcpy(showDiagnosBuf[showDiagnosLen], "pid28 BANK2��������1�����ѹ��");
	else
		strcpy(showDiagnosBuf[showDiagnosLen], "pid28 BANK3��������1�����ѹ��");

	showDiagnosLen++;

	//��һ������
	strcpy(showDiagnosBuf[showDiagnosLen], "[A]��Ч�ȣ�");

	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];

	tempVal *= 305;

	//����
	tempVal_i = tempVal/10000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//С������
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%10000000;
	tempVal_f /= 1000;

	//��������
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

	//�ڶ�������
	strcpy(showDiagnosBuf[showDiagnosLen], "[B]��ѹ��");
	memset(Numbuf, '\0', 3);
	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];

	tempVal *= 122;

	//����
	tempVal_i = tempVal/1000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//С������
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%1000000;
	tempVal_f /= 1000;

	//�������� ���⴦�����ֵ
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

	//��һ������
	if(o2sensorDefine == 0)
		strcpy(showDiagnosBuf[showDiagnosLen], "pid29 BANK2��������2�����ѹ��");
	else
		strcpy(showDiagnosBuf[showDiagnosLen], "pid29 BANK3��������2�����ѹ��");

	showDiagnosLen++;

	//��һ������
	strcpy(showDiagnosBuf[showDiagnosLen], "[A]��Ч�ȣ�");

	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];

	tempVal *= 305;

	//����
	tempVal_i = tempVal/10000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//С������
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%10000000;
	tempVal_f /= 1000;

	//��������
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

	//�ڶ�������
	strcpy(showDiagnosBuf[showDiagnosLen], "[B]��ѹ��");
	memset(Numbuf, '\0', 3);
	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];

	tempVal *= 122;

	//����
	tempVal_i = tempVal/1000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//С������
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%1000000;
	tempVal_f /= 1000;

	//�������� ���⴦�����ֵ
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

	//��һ������
	if(o2sensorDefine == 0)
		strcpy(showDiagnosBuf[showDiagnosLen], "pid2A BANK2��������3�����ѹ��");
	else
		strcpy(showDiagnosBuf[showDiagnosLen], "pid2A BANK4��������1�����ѹ��");

	showDiagnosLen++;

	//��һ������
	strcpy(showDiagnosBuf[showDiagnosLen], "[A]��Ч�ȣ�");

	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];

	tempVal *= 305;

	//����
	tempVal_i = tempVal/10000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//С������
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%10000000;
	tempVal_f /= 1000;

	//��������
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

	//�ڶ�������
	strcpy(showDiagnosBuf[showDiagnosLen], "[B]��ѹ��");
	memset(Numbuf, '\0', 3);
	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];

	tempVal *= 122;

	//����
	tempVal_i = tempVal/1000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//С������
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%1000000;
	tempVal_f /= 1000;

	//�������� ���⴦�����ֵ
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

	//��һ������
	if(o2sensorDefine == 0)
		strcpy(showDiagnosBuf[showDiagnosLen], "pid2B BANK2��������4�����ѹ��");
	else
		strcpy(showDiagnosBuf[showDiagnosLen], "pid2B BANK4��������2�����ѹ��");

	showDiagnosLen++;

	//��һ������
	strcpy(showDiagnosBuf[showDiagnosLen], "[A]��Ч�ȣ�");

	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];

	tempVal *= 305;

	//����
	tempVal_i = tempVal/10000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//С������
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%10000000;
	tempVal_f /= 1000;

	//��������
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

	//�ڶ�������
	strcpy(showDiagnosBuf[showDiagnosLen], "[B]��ѹ��");
	memset(Numbuf, '\0', 3);
	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];

	tempVal *= 122;

	//����
	tempVal_i = tempVal/1000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//С������
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%1000000;
	tempVal_f /= 1000;

	//�������� ���⴦�����ֵ
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

	strcpy(showDiagnosBuf[showDiagnosLen], "pid2C [A]����ERG��");
	//��һ������
	tempVal = data[offset];
	tempVal *= 1000;
	tempVal /= 255;

	//����
	tempVal_i = tempVal/10;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//С������
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

	strcpy(showDiagnosBuf[showDiagnosLen], "pid2D [A]ERG����");
	//��һ������
	tempVal = data[offset];//bit
	//�Ƿ���
	if(tempVal < 128)
	{
		strcat(showDiagnosBuf[showDiagnosLen], "-");
		tempVal = 128 - tempVal;
	}
	else
	{
		tempVal -= 128;
	}

	tempVal *= 10000;//�Ŵ�10000��
	tempVal /= 128;

	//��������
	tempVal_i = tempVal/100;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);
	//С������
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

	strcpy(showDiagnosBuf[showDiagnosLen], "pid2E [A]��������������");
	//��һ������
	tempVal = data[offset];
	tempVal *= 1000;
	tempVal /= 255;

	//����
	tempVal_i = tempVal/10;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//С������
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

	strcpy(showDiagnosBuf[showDiagnosLen], "pid2F [A]ȼ�����룺");
	//��һ������
	tempVal = data[offset];
	tempVal *= 1000;
	tempVal /= 255;

	//����
	tempVal_i = tempVal/10;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//С������
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

	strcpy(showDiagnosBuf[showDiagnosLen], "pid30 [A]����Ϲ�����������������");
	//��һ������
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

	strcpy(showDiagnosBuf[showDiagnosLen], "pid31 [AB]����Ϲ����������ʻ���룺");
	//��һ������
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

	strcpy(showDiagnosBuf[showDiagnosLen], "pid32 [AB]EVAP ϵͳ����ѹ����");
	//��һ������
	tempVal = data[offset];
	tempVal <<= 8;
	tempVal |= data[offset+1];//bit

	//��������
	tempVal_i =  tempVal/4;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);
	//С������
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

	strcpy(showDiagnosBuf[showDiagnosLen], "pid33 [A]��ѹ��");
	//��һ������
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

	strcpy(showDiagnosBuf[showDiagnosLen], "pid34 BANK1 ��������1��");
	showDiagnosLen++;

	//��һ������
	strcpy(showDiagnosBuf[showDiagnosLen], "[A]��Ч�ȣ�");

	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];

	tempVal *= 305;

	//����
	tempVal_i = tempVal/10000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//С������
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%10000000;
	tempVal_f /= 1000;

	//��������
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

	//�ڶ�������
	strcpy(showDiagnosBuf[showDiagnosLen], "[B]������");
	memset(Numbuf, '\0', 3);
	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];
	//���� ����
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

	//����
	tempVal_i = temp/100000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], ".");

	//С������
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

	//��һ������
	strcpy(showDiagnosBuf[showDiagnosLen], "pid35 BANK1��������2��");
	showDiagnosLen++;

	//��һ������
	strcpy(showDiagnosBuf[showDiagnosLen], "[A]��Ч�ȣ�");

	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];

	tempVal *= 305;

	//����
	tempVal_i = tempVal/10000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//С������
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%10000000;
	tempVal_f /= 1000;

	//��������
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

		//�ڶ�������
	strcpy(showDiagnosBuf[showDiagnosLen], "[B]������");
	memset(Numbuf, '\0', 3);
	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];
	//���� ����
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

	//����
	tempVal_i = temp/100000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], ".");

	//С������
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

	//��һ������
	if(o2sensorDefine == 0)
		strcpy(showDiagnosBuf[showDiagnosLen], "pid36 BANK1��������3�����ѹ��");
	else
		strcpy(showDiagnosBuf[showDiagnosLen], "pid36 BANK2��������1�����ѹ��");

	showDiagnosLen++;

	//��һ������
	strcpy(showDiagnosBuf[showDiagnosLen], "[A]��Ч�ȣ�");

	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];

	tempVal *= 305;

	//����
	tempVal_i = tempVal/10000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//С������
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%10000000;
	tempVal_f /= 1000;

	//��������
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

	//�ڶ�������
	strcpy(showDiagnosBuf[showDiagnosLen], "[B]������");
	memset(Numbuf, '\0', 3);
	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];
	//���� ����
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

	//����
	tempVal_i = temp/100000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], ".");

	//С������
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

	//��һ������
	if(o2sensorDefine == 0)
		strcpy(showDiagnosBuf[showDiagnosLen], "pid37 BANK1��������4�����ѹ��");
	else
		strcpy(showDiagnosBuf[showDiagnosLen], "pid37 BANK2��������2�����ѹ��");

	showDiagnosLen++;

	//��һ������
	strcpy(showDiagnosBuf[showDiagnosLen], "[A]��Ч�ȣ�");

	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];

	tempVal *= 305;

	//����
	tempVal_i = tempVal/10000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//С������
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%10000000;
	tempVal_f /= 1000;

	//��������
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

	//�ڶ�������
	strcpy(showDiagnosBuf[showDiagnosLen], "[B]������");
	memset(Numbuf, '\0', 3);
	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];
	//���� ����
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

	//����
	tempVal_i = temp/100000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], ".");

	//С������
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

	//��һ������
	if(o2sensorDefine == 0)
		strcpy(showDiagnosBuf[showDiagnosLen], "pid38 BANK2��������1�����ѹ��");
	else
		strcpy(showDiagnosBuf[showDiagnosLen], "pid38 BANK3��������1�����ѹ��");

	showDiagnosLen++;

	//��һ������
	strcpy(showDiagnosBuf[showDiagnosLen], "[A]��Ч�ȣ�");

	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];

	tempVal *= 305;

	//����
	tempVal_i = tempVal/10000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//С������
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%10000000;
	tempVal_f /= 1000;

	//��������
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

		//�ڶ�������
	strcpy(showDiagnosBuf[showDiagnosLen], "[B]������");
	memset(Numbuf, '\0', 3);
	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];
	//���� ����
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

	//����
	tempVal_i = temp/100000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], ".");

	//С������
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

	//��һ������
	if(o2sensorDefine == 0)
		strcpy(showDiagnosBuf[showDiagnosLen], "pid39 BANK2��������2�����ѹ��");
	else
		strcpy(showDiagnosBuf[showDiagnosLen], "pid39 BANK3��������2�����ѹ��");

	showDiagnosLen++;

	//��һ������
	strcpy(showDiagnosBuf[showDiagnosLen], "[A]��Ч�ȣ�");

	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];

	tempVal *= 305;

	//����
	tempVal_i = tempVal/10000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//С������
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%10000000;
	tempVal_f /= 1000;

	//��������
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

		//�ڶ�������
	strcpy(showDiagnosBuf[showDiagnosLen], "[B]������");
	memset(Numbuf, '\0', 3);
	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];
	//���� ����
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

	//����
	tempVal_i = temp/100000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], ".");

	//С������
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

	//��һ������
	if(o2sensorDefine == 0)
		strcpy(showDiagnosBuf[showDiagnosLen], "pid3A BANK2��������3�����ѹ��");
	else
		strcpy(showDiagnosBuf[showDiagnosLen], "pid3A BANK4��������1�����ѹ��");

	showDiagnosLen++;

	//��һ������
	strcpy(showDiagnosBuf[showDiagnosLen], "[A]��Ч�ȣ�");

	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];

	tempVal *= 305;

	//����
	tempVal_i = tempVal/10000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//С������
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%10000000;
	tempVal_f /= 1000;

	//��������
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

	//�ڶ�������
		//�ڶ�������
	strcpy(showDiagnosBuf[showDiagnosLen], "[B]������");
	memset(Numbuf, '\0', 3);
	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];
	//���� ����
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

	//����
	tempVal_i = temp/100000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], ".");

	//С������
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

	//��һ������
	if(o2sensorDefine == 0)
		strcpy(showDiagnosBuf[showDiagnosLen], "pid3B BANK2��������4�����ѹ��");
	else
		strcpy(showDiagnosBuf[showDiagnosLen], "pid3B BANK4��������2�����ѹ��");

	showDiagnosLen++;

	//��һ������
	strcpy(showDiagnosBuf[showDiagnosLen], "[A]��Ч�ȣ�");

	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];

	tempVal *= 305;

	//����
	tempVal_i = tempVal/10000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//С������
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%10000000;
	tempVal_f /= 1000;

	//��������
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

	//�ڶ�������
		//�ڶ�������
	strcpy(showDiagnosBuf[showDiagnosLen], "[B]������");
	memset(Numbuf, '\0', 3);
	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];
	//���� ����
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

	//����
	tempVal_i = temp/100000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], ".");

	//С������
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

	strcpy(showDiagnosBuf[showDiagnosLen], "pid3C [AB]������1 �߻����¶���1��");
	//��һ������
	tempVal = data[offset];
	tempVal <<= 8;
	tempVal |= data[offset+1];

	//����
	if(tempVal < 400)
	{
		strcat(showDiagnosBuf[showDiagnosLen], "-");
		tempVal = 400-tempVal;
	}
	else
	{
		tempVal -= 400;
	}

	//��������

	tempVal_i = tempVal/10;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf, 0, 5);
	//С������
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%10;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "��");

	showDiagnosLen++;
}

void analysis_sid0102_pid3D(char *data)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;

	char Numbuf[5] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "pid3D [AB]������1 �߻����¶���2��");
	//��һ������
	tempVal = data[offset];
	tempVal <<= 8;
	tempVal |= data[offset+1];

	//����
	if(tempVal < 400)
	{
		strcat(showDiagnosBuf[showDiagnosLen], "-");
		tempVal = 400-tempVal;
	}
	else
	{
		tempVal -= 400;
	}

	//��������

	tempVal_i = tempVal/10;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf, 0, 5);
	//С������
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%10;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "��");

	showDiagnosLen++;
}

void analysis_sid0102_pid3E(char *data)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;

	char Numbuf[5] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "pid3E [AB]������2 �߻����¶���1��");
	//��һ������
	tempVal = data[offset];
	tempVal <<= 8;
	tempVal |= data[offset+1];

	//����
	if(tempVal < 400)
	{
		strcat(showDiagnosBuf[showDiagnosLen], "-");
		tempVal = 400-tempVal;
	}
	else
	{
		tempVal -= 400;
	}

	//��������

	tempVal_i = tempVal/10;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf, 0, 5);
	//С������
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%10;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "��");

	showDiagnosLen++;
}

void analysis_sid0102_pid3F(char *data)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;

	char Numbuf[5] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "pid3F [AB]������2 �߻����¶���2��");
	//��һ������
	tempVal = data[offset];
	tempVal <<= 8;
	tempVal |= data[offset+1];

	//����
	if(tempVal < 400)
	{
		strcat(showDiagnosBuf[showDiagnosLen], "-");
		tempVal = 400-tempVal;
	}
	else
	{
		tempVal -= 400;
	}

	//��������

	tempVal_i = tempVal/10;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf, 0, 5);
	//С������
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%10;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "��");

	showDiagnosLen++;
}

void analysis_sid0102_pid41(char *data)
{
	int offset = 0;

	//DATAB
	offset++;	//��һ������

	if(data[offset]&0x01)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid41 [Bbit0]����ʧ����");
		if(data[offset]&0x10)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "��[Bbit4]���δ���");
		}
		else
		{
			strcat(showDiagnosBuf[showDiagnosLen], "��[Bbit4]������");
		}
	}
	else
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid41 [Bbit0]����ʧ����");
	}
	showDiagnosLen++;

	if(data[offset]&0x02)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid41 [Bbit1]����ȼ��ϵͳ���");
		if(data[offset]&0x20)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "��[Bbit5]���δ���");
		}
		else
		{
			strcat(showDiagnosBuf[showDiagnosLen], "��[Bbit5]������");
		}
	}
	else
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid41 [Bbit1]����ȼ��ϵͳ���");
	}
	showDiagnosLen++;

	if(data[offset]&0x04)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid41 [Bbit2]����ȫ��������");
		if(data[offset]&0x40)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "��[Bbit6]���δ���");
		}
		else
		{
			strcat(showDiagnosBuf[showDiagnosLen], "��[Bbit6]������");
		}
	}
	else
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid41 [Bbit2]����ȫ��������");
	}
	showDiagnosLen++;

	//DATAC
	offset++;	//��һ������
	if(data[offset]&0x01)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid41 [Cbit0]���ô߻������");
		if(data[offset+1]&0x01)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "��[Dbit0]���δ���");
		}
		else
		{
			strcat(showDiagnosBuf[showDiagnosLen], "��[Dbit0]������");
		}
	}
	else
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid41 [Cbit0]���ô߻������");
	}
	showDiagnosLen++;

	if(data[offset]&0x02)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid41 [Cbit1]���ü��ȴ߻������");
		if(data[offset+1]&0x02)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "��[Dbit1]���δ���");
		}
		else
		{
			strcat(showDiagnosBuf[showDiagnosLen], "��[Dbit1]������");
		}
	}
	else
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid41 [Cbit1]���ü��ȴ߻������");
	}
	showDiagnosLen++;

	if(data[offset]&0x04)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid41 [Cbit2]��������ϵͳ���");
		if(data[offset+1]&0x04)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "��[Dbit2]���δ���");
		}
		else
		{
			strcat(showDiagnosBuf[showDiagnosLen], "��[Dbit2]������");
		}
	}
	else
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid41 [Cbit2]��������ϵͳ���");
	}
	showDiagnosLen++;

	if(data[offset]&0x08)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid41 [Cbit3]���ö��ο���ϵͳ���");
		if(data[offset+1]&0x08)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "��[Dbit3]���δ���");
		}
		else
		{
			strcat(showDiagnosBuf[showDiagnosLen], "��[Dbit3]������");
		}
	}
	else
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid41 [Cbit3]���ö��ο���ϵͳ���");
	}
	showDiagnosLen++;

	if(data[offset]&0x10)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid41 [Cbit4]���ÿյ�ϵͳ��������");
		if(data[offset+1]&0x10)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "��[Dbit4]���δ���");
		}
		else
		{
			strcat(showDiagnosBuf[showDiagnosLen], "��[Dbit4]������");
		}
	}
	else
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid41 [Cbit4]���ÿյ�ϵͳ��������");
	}
	showDiagnosLen++;

	if(data[offset]&0x20)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid41 [Cbit5]���������������");
		if(data[offset+1]&0x20)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "��[Dbit5]���δ���");
		}
		else
		{
			strcat(showDiagnosBuf[showDiagnosLen], "��[Dbit5]������");
		}
	}
	else
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid41 [Cbit5]���������������");
	}
	showDiagnosLen++;

	if(data[offset]&0x40)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid41 [Cbit6]���������м��������");
		if(data[offset+1]&0x40)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "��[Dbit6]���δ���");
		}
		else
		{
			strcat(showDiagnosBuf[showDiagnosLen], "��[Dbit6]������");
		}
	}
	else
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid41 [Cbit6]���������м��������");
	}
	showDiagnosLen++;

	if(data[offset]&0x80)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid41 [Cbit7]����EGRϵͳ���");

		if(data[offset+1]&0x80)
		{
			strcat(showDiagnosBuf[showDiagnosLen], "��[Dbit7]���δ���");
		}
		else
		{
			strcat(showDiagnosBuf[showDiagnosLen], "��[Dbit7]������");
		}
	}
	else
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "pid41 [Cbit7]����EGRϵͳ���");
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

	strcpy(showDiagnosBuf[showDiagnosLen], "pid42 [AB]��ѹ����ģ�飺");
	//��һ������
	tempVal = data[offset];
	tempVal <<= 8;
	tempVal |= data[offset+1];

	//��������
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf, 0, 3);
	//С������
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

	strcpy(showDiagnosBuf[showDiagnosLen], "pid43 [AB]���Եĸ���ֵ:");
	//��һ������
	tempVal = data[offset];
	tempVal <<= 8;
	tempVal |= data[offset+1];

	tempVal *= 10000;
	tempVal /= 255;

	//��������
	tempVal_i = tempVal/100;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf, 0, 5);
	//С������
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

	strcpy(showDiagnosBuf[showDiagnosLen], "pid44 [AB]�����Ч�ȣ�");

	tempVal = data[offset++];//bit
	tempVal <<= 8;
	tempVal |= data[offset++];

	tempVal *= 305;

	//����
	tempVal_i = tempVal/10000000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//С������
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);

	tempVal_f = tempVal%10000000;
	tempVal_f /= 1000;

	//��������
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

	strcpy(showDiagnosBuf[showDiagnosLen], "pid45 [A]��Խ�����λ�ã�");
	//��һ������
	tempVal = data[offset];
	tempVal *= 1000;
	tempVal /= 255;

	//����
	tempVal_i = tempVal/10;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//С������
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

	strcpy(showDiagnosBuf[showDiagnosLen], "pid46 [A]���������¶ȣ�");
	//��һ������
	tempVal = data[offset];
	//����
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
	strcat(showDiagnosBuf[showDiagnosLen], "��");

	showDiagnosLen++;
}

void analysis_sid0102_pid47(char *data)
{
	int offset = 0;
	int tempVal = 0;
	int tempVal_i = 0;
	int tempVal_f = 0;
	char Numbuf[3] = {0};

	strcpy(showDiagnosBuf[showDiagnosLen], "pid47 [A]����������λ��B��");
	//��һ������
	tempVal = data[offset];
	tempVal *= 1000;
	tempVal /= 255;

	//����
	tempVal_i = tempVal/10;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//С������
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

	strcpy(showDiagnosBuf[showDiagnosLen], "pid48 [A]����������λ��C��");
	//��һ������
	tempVal = data[offset];
	tempVal *= 1000;
	tempVal /= 255;

	//����
	tempVal_i = tempVal/10;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//С������
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

	strcpy(showDiagnosBuf[showDiagnosLen], "pid49 [A]����������λ��D��");
	//��һ������
	tempVal = data[offset];
	tempVal *= 1000;
	tempVal /= 255;

	//����
	tempVal_i = tempVal/10;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//С������
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

	strcpy(showDiagnosBuf[showDiagnosLen], "pid4A [A]����������λ��E��");
	//��һ������
	tempVal = data[offset];
	tempVal *= 1000;
	tempVal /= 255;

	//����
	tempVal_i = tempVal/10;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//С������
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

	strcpy(showDiagnosBuf[showDiagnosLen], "pid4B [A]����������λ��F��");
	//��һ������
	tempVal = data[offset];
	tempVal *= 1000;
	tempVal /= 255;

	//����
	tempVal_i = tempVal/10;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//С������
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

	strcpy(showDiagnosBuf[showDiagnosLen], "pid4C [A]ͨѶ������ִ�л������ƣ�");
	//��һ������
	tempVal = data[offset];
	tempVal *= 1000;
	tempVal /= 255;

	//����
	tempVal_i = tempVal/10;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//С������
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

	strcpy(showDiagnosBuf[showDiagnosLen], "pid4D [AB]����MILʱ�������е�ʱ�䣺");
	//��һ������
	tempVal = data[offset];
	tempVal <<= 8;
	tempVal |= data[offset+1];//bit

	//��������
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

	strcpy(showDiagnosBuf[showDiagnosLen], "pid4E [AB]��Ϲ��ϴ������������ʱ�䣺");
	//��һ������
	tempVal = data[offset];
	tempVal <<= 8;
	tempVal |= data[offset+1];//bit

	//��������
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

	strcpy(showDiagnosBuf[showDiagnosLen], "pid4F [A]��ֵ�ȵ����ֵ��");
	//��һ������
	tempVal = data[offset++];//bit

	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	showDiagnosLen++;
	memset(Numbuf,0,4);

	strcpy(showDiagnosBuf[showDiagnosLen], "pid4F [B]����������ѹ���ֵ��");
	//�ڶ�������
	tempVal = data[offset++];//bit

	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "V");
	showDiagnosLen++;

	memset(Numbuf,0,4);
	strcpy(showDiagnosBuf[showDiagnosLen], "pid4F [C]���������������ֵ��");
	//����������
	tempVal = data[offset++];//bit

	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "mA");
	showDiagnosLen++;

	memset(Numbuf,0,4);
	strcpy(showDiagnosBuf[showDiagnosLen], "pid4F [D]������ܾ���ѹ�������ֵ��");
	//���ĸ�����
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

	strcpy(showDiagnosBuf[showDiagnosLen], "pid50 [A]�����������ֵ�����������������ʴ�������");

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

	strcpy(showDiagnosBuf[showDiagnosLen], "pid51 [A]����Ŀǰʹ�õ�ȼ�����ͣ�");
	//��һ������
	tempVal = data[offset];

	switch(tempVal)
	{
		case 0x01:
			strcat(showDiagnosBuf[showDiagnosLen], "����");
			break;
		case 0x02:
			strcat(showDiagnosBuf[showDiagnosLen], "��ȩ");
			break;
		case 0x03:
			strcat(showDiagnosBuf[showDiagnosLen], "�Ҵ�");
			break;
		case 0x04:
			strcat(showDiagnosBuf[showDiagnosLen], "����");
			break;
		case 0x05:
			strcat(showDiagnosBuf[showDiagnosLen], "ʯ����");
			break;
		case 0x06:
			strcat(showDiagnosBuf[showDiagnosLen], "ѹ����Ȼ��");
			break;
		case 0x07:
			strcat(showDiagnosBuf[showDiagnosLen], "����");
			break;
		case 0x08:
			strcat(showDiagnosBuf[showDiagnosLen], "���");
			break;
		case 0x09:
			strcat(showDiagnosBuf[showDiagnosLen], "ʹ�����͵�˫ȼ�ϳ���");
			break;
		case 0x0a:
			strcat(showDiagnosBuf[showDiagnosLen], "ʹ�ü�ȩ��˫ȼ�ϳ���");
			break;
		case 0x0b:
			strcat(showDiagnosBuf[showDiagnosLen], "ʹ���Ҵ���˫ȼ�ϳ���");
			break;
		case 0x0c:
			strcat(showDiagnosBuf[showDiagnosLen], "ʹ��ʯ������˫ȼ�ϳ���");
			break;
		case 0x0d:
			strcat(showDiagnosBuf[showDiagnosLen], "ʹ��ѹ����Ȼ����˫ȼ�ϳ���");
			break;
		case 0x0e:
			strcat(showDiagnosBuf[showDiagnosLen], "ʹ�ñ����˫ȼ�ϳ���");
			break;
		case 0x0f:
			strcat(showDiagnosBuf[showDiagnosLen], "ʹ�õ�ص�˫ȼ�ϳ���");
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

	strcpy(showDiagnosBuf[showDiagnosLen], "pid52 [A]�ƾ�ȼ�ϱ�����");
	//��һ������
	tempVal = data[offset];
	tempVal *= 1000;
	tempVal /= 255;

	//����
	tempVal_i = tempVal/10;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//С������
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

	strcpy(showDiagnosBuf[showDiagnosLen], "pid53 [AB]����Evapϵͳ����ѹ����");
	//��һ������
	tempVal = data[offset];
	tempVal <<= 8;
	tempVal |= data[offset+1];

	tempVal *= 10;
	tempVal /= 2;

	//��������
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf, 0, 3);
	//С������
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

	strcpy(showDiagnosBuf[showDiagnosLen], "pid54 [AB]Evapϵͳ����ѹ����");
	//��һ������
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

	//��������
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

	strcpy(showDiagnosBuf[showDiagnosLen], "pid55 [A]��ʱ���ڶ�����������ȼ��������BANK1��");
	//��һ������
	tempVal = data[offset];//bit
	//�Ƿ���
	if(tempVal < 128)
	{
		strcat(showDiagnosBuf[showDiagnosLen], "-");
		tempVal = 128 - tempVal;
	}
	else
	{
		tempVal -= 128;
	}

	tempVal *= 10000;//�Ŵ�10000��
	tempVal /= 128;

	//��������
	tempVal_i = tempVal/100;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);
	//С������
	tempVal_f = tempVal%100;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "%");
	showDiagnosLen++;

	strcpy(showDiagnosBuf[showDiagnosLen], "pid55 [B]��ʱ���ڶ�����������ȼ��������BANK3��");
	//��2������
	tempVal = data[offset+1];//bit
	//�Ƿ���
	if(tempVal < 128)
	{
		strcat(showDiagnosBuf[showDiagnosLen], "-");
		tempVal = 128 - tempVal;
	}
	else
	{
		tempVal -= 128;
	}

	tempVal *= 10000;//�Ŵ�10000��
	tempVal /= 128;

	//��������
	tempVal_i = tempVal/100;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);
	//С������
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

	strcpy(showDiagnosBuf[showDiagnosLen], "pid56 [A]��ʱ���ڶ�����������ȼ��������BANK1��");
	//��һ������
	tempVal = data[offset];//bit
	//�Ƿ���
	if(tempVal < 128)
	{
		strcat(showDiagnosBuf[showDiagnosLen], "-");
		tempVal = 128 - tempVal;
	}
	else
	{
		tempVal -= 128;
	}

	tempVal *= 10000;//�Ŵ�10000��
	tempVal /= 128;

	//��������
	tempVal_i = tempVal/100;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);
	//С������
	tempVal_f = tempVal%100;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "%");
	showDiagnosLen++;

	strcpy(showDiagnosBuf[showDiagnosLen], "pid56 [B]��ʱ���ڶ�����������ȼ��������BANK3��");
	//��2������
	tempVal = data[offset+1];//bit
	//�Ƿ���
	if(tempVal < 128)
	{
		strcat(showDiagnosBuf[showDiagnosLen], "-");
		tempVal = 128 - tempVal;
	}
	else
	{
		tempVal -= 128;
	}

	tempVal *= 10000;//�Ŵ�10000��
	tempVal /= 128;

	//��������
	tempVal_i = tempVal/100;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);
	//С������
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

	strcpy(showDiagnosBuf[showDiagnosLen], "pid57 [A]��ʱ���ڶ�����������ȼ��������BANK2��");
	//��һ������
	tempVal = data[offset];//bit
	//�Ƿ���
	if(tempVal < 128)
	{
		strcat(showDiagnosBuf[showDiagnosLen], "-");
		tempVal = 128 - tempVal;
	}
	else
	{
		tempVal -= 128;
	}

	tempVal *= 10000;//�Ŵ�10000��
	tempVal /= 128;

	//��������
	tempVal_i = tempVal/100;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);
	//С������
	tempVal_f = tempVal%100;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "%");
	showDiagnosLen++;

	strcpy(showDiagnosBuf[showDiagnosLen], "pid57 [B]��ʱ���ڶ�����������ȼ��������BANK4��");
	//��2������
	tempVal = data[offset+1];//bit
	//�Ƿ���
	if(tempVal < 128)
	{
		strcat(showDiagnosBuf[showDiagnosLen], "-");
		tempVal = 128 - tempVal;
	}
	else
	{
		tempVal -= 128;
	}

	tempVal *= 10000;//�Ŵ�10000��
	tempVal /= 128;

	//��������
	tempVal_i = tempVal/100;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);
	//С������
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

	strcpy(showDiagnosBuf[showDiagnosLen], "pid58 [A]��ʱ���ڶ�����������ȼ��������BANK2��");
	//��һ������
	tempVal = data[offset];//bit
	//�Ƿ���
	if(tempVal < 128)
	{
		strcat(showDiagnosBuf[showDiagnosLen], "-");
		tempVal = 128 - tempVal;
	}
	else
	{
		tempVal -= 128;
	}

	tempVal *= 10000;//�Ŵ�10000��
	tempVal /= 128;

	//��������
	tempVal_i = tempVal/100;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);
	//С������
	tempVal_f = tempVal%100;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "%");
	showDiagnosLen++;

	strcpy(showDiagnosBuf[showDiagnosLen], "pid58 [B]��ʱ���ڶ�����������ȼ��������BANK4��");
	//��2������
	tempVal = data[offset+1];//bit
	//�Ƿ���
	if(tempVal < 128)
	{
		strcat(showDiagnosBuf[showDiagnosLen], "-");
		tempVal = 128 - tempVal;
	}
	else
	{
		tempVal -= 128;
	}

	tempVal *= 10000;//�Ŵ�10000��
	tempVal /= 128;

	//��������
	tempVal_i = tempVal/100;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

	strcat(showDiagnosBuf[showDiagnosLen], ".");
	memset(Numbuf, '\0', 3);
	//С������
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

	strcpy(showDiagnosBuf[showDiagnosLen], "pid59 [AB]ȼ�Ϲ�ѹ�����ԣ�");
	//��һ������
	tempVal = data[offset];
	tempVal <<= 8;
	tempVal |= data[offset+1];//bit

	tempVal *= 10;

	//��������
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

	strcpy(showDiagnosBuf[showDiagnosLen], "pid5A [A]�������̤�壺");
	//��һ������
	tempVal = data[offset];
	tempVal *= 1000;
	tempVal /= 255;

	//����
	tempVal_i = tempVal/10;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	//С������
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%10;
	memset(Numbuf, '\0', 3);
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "%");

	showDiagnosLen++;
}



/** *************************************���ⶨ��************************************* **/
void analysis_sid0102_pidFD(char *data)
{
	int offset = 0;
	int tempVal = 0;

	char Numbuf[5] = {0};

	//��һ������
	strcpy(showDiagnosBuf[showDiagnosLen], "ADP��������");

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

	//��һ������
	strcpy(showDiagnosBuf[showDiagnosLen], "ADPȼ�ϳ���");

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

	//��һ������
	strcpy(showDiagnosBuf[showDiagnosLen], "ADPȼ�ϵ���");

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
			//����PID
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
					return 1;//�������մ���
				}
			}
			else
			{
				return ret;
			}
		}
		else
		{
//			txlen = kwpFmt((unsigned char *)commbuf, 2);//ͷ����Ϣ
			commbuf[txlen++] = 0x83;
			commbuf[txlen++] = 0x10;
			commbuf[txlen++] = 0xF1;
			commbuf[txlen++] = diagnose.server_id;
			commbuf[txlen++] = diagnose.sub_function;
			checkbuf[checklen++] = diagnose.server_id + 0x40;
			checkbuf[checklen++] = diagnose.sub_function;
			//����PID
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
					return 1;//�������մ���
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
					return 1;//�������մ���
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
					return 1;//�������մ���
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
					return 1;//�������մ���
				}
			}
			else
			{
				return ret;
			}
		}
		else
		{
//			txlen = kwpFmt((unsigned char *)commbuf, 1);//ͷ����Ϣ
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
					return 1;//�������մ���
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
				//��������λ��
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
					return 1;//�������մ���
				}
			}
			else
			{
				return ret;
			}
		}
		else
		{
//			txlen = kwpFmt((unsigned char *)commbuf, 2);//ͷ����Ϣ
			commbuf[txlen++] = 0x82;
			commbuf[txlen++] = 0x10;
			commbuf[txlen++] = 0xF1;
			commbuf[txlen++] = diagnose.server_id;
			commbuf[txlen++] = diagnose.sub_function;
			checkbuf[checklen++] = diagnose.server_id + 0x40;
			checkbuf[checklen++] = diagnose.sub_function;
			if(diagnose.sub_function == 0x05)
			{
				//��������λ��
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
					return 1;//�������մ���
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

	strcpy(showDiagnosBuf[showDiagnosLen], "��ϡ��Ũ��������ֵ��ѹ��");
	showDiagnosLen++;

	strcpy(showDiagnosBuf[showDiagnosLen], "Test Value:");
	//��һ������
	tempVal = data[offset++];//bit
	tempVal *= 5;
	//����
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf,0,3);
	//С��
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "V");
	showDiagnosLen++;
	memset(Numbuf,0,3);
	
	strcpy(showDiagnosBuf[showDiagnosLen], "minimum Limit:");
	//�ڶ�������
	tempVal = data[offset++];//bit
	tempVal *= 5;
	//����
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf,0,3);
	//С��
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "V");
	showDiagnosLen++;
	memset(Numbuf,0,3);
	
	strcpy(showDiagnosBuf[showDiagnosLen], "maximum Limit:");
	//����������
	tempVal = data[offset++];//bit
	tempVal *= 5;
	//����
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf,0,3);
	//С��
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

	strcpy(showDiagnosBuf[showDiagnosLen], "��Ũ��ϡ��������ֵ��ѹ��");
	showDiagnosLen++;

	strcpy(showDiagnosBuf[showDiagnosLen], "Test Value:");
	//��һ������
	tempVal = data[offset++];//bit
	tempVal *= 5;
	//����
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf,0,3);
	//С��
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "V");
	showDiagnosLen++;
	memset(Numbuf,0,3);
	
	strcpy(showDiagnosBuf[showDiagnosLen], "minimum Limit:");
	//�ڶ�������
	tempVal = data[offset++];//bit
	tempVal *= 5;
	//����
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf,0,3);
	//С��
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "V");
	showDiagnosLen++;
	memset(Numbuf,0,3);
	
	strcpy(showDiagnosBuf[showDiagnosLen], "maximum Limit:");
	//����������
	tempVal = data[offset++];//bit
	tempVal *= 5;
	//����
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf,0,3);
	//С��
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

	strcpy(showDiagnosBuf[showDiagnosLen], "�����л�ʱ�����Ĵ������͵�ѹ��");
	showDiagnosLen++;

	strcpy(showDiagnosBuf[showDiagnosLen], "Test Value:");
	//��һ������
	tempVal = data[offset++];//bit
	tempVal *= 5;
	//����
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf,0,3);
	//С��
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "V");
	showDiagnosLen++;
	memset(Numbuf,0,3);
	
	strcpy(showDiagnosBuf[showDiagnosLen], "minimum Limit:");
	//�ڶ�������
	tempVal = data[offset++];//bit
	tempVal *= 5;
	//����
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf,0,3);
	//С��
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "V");
	showDiagnosLen++;
	memset(Numbuf,0,3);
	
	strcpy(showDiagnosBuf[showDiagnosLen], "maximum Limit:");
	//����������
	tempVal = data[offset++];//bit
	tempVal *= 5;
	//����
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf,0,3);
	//С��
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

	strcpy(showDiagnosBuf[showDiagnosLen], "�����л�ʱ�����Ĵ������ߵ�ѹ��");
	showDiagnosLen++;

	strcpy(showDiagnosBuf[showDiagnosLen], "Test Value:");
	//��һ������
	tempVal = data[offset++];//bit
	tempVal *= 5;
	//����
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf,0,3);
	//С��
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "V");
	showDiagnosLen++;
	memset(Numbuf,0,3);
	
	strcpy(showDiagnosBuf[showDiagnosLen], "minimum Limit:");
	//�ڶ�������
	tempVal = data[offset++];//bit
	tempVal *= 5;
	//����
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf,0,3);
	//С��
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "V");
	showDiagnosLen++;
	memset(Numbuf,0,3);
	
	strcpy(showDiagnosBuf[showDiagnosLen], "maximum Limit:");
	//����������
	tempVal = data[offset++];//bit
	tempVal *= 5;
	//����
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf,0,3);
	//С��
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

	strcpy(showDiagnosBuf[showDiagnosLen], "��Ũ��ϡ���������л�ʱ�䣺");
	showDiagnosLen++;
	
	strcpy(showDiagnosBuf[showDiagnosLen], "Test Value:");
	//��һ������
	tempVal = data[offset++];//bit
	tempVal *= 4;
	//����
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf,0,3);
	//С��
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "s");
	showDiagnosLen++;
	memset(Numbuf,0,3);
	
	strcpy(showDiagnosBuf[showDiagnosLen], "minimum Limit:");
	//�ڶ�������
	tempVal = data[offset++];//bit
	tempVal *= 4;
	//����
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf,0,3);
	//С��
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "s");
	showDiagnosLen++;
	memset(Numbuf,0,3);
	
	strcpy(showDiagnosBuf[showDiagnosLen], "maximum Limit:");
	//����������
	tempVal = data[offset++];//bit
	tempVal *= 4;
	//����
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf,0,3);
	//С��
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

	strcpy(showDiagnosBuf[showDiagnosLen], "��ϡ��Ũ���������л�ʱ�䣺");
	showDiagnosLen++;
	
	strcpy(showDiagnosBuf[showDiagnosLen], "Test Value:");
	//��һ������
	tempVal = data[offset++];//bit
	tempVal *= 4;
	//����
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf,0,3);
	//С��
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "s");
	showDiagnosLen++;
	memset(Numbuf,0,3);
	
	strcpy(showDiagnosBuf[showDiagnosLen], "minimum Limit:");
	//�ڶ�������
	tempVal = data[offset++];//bit
	tempVal *= 4;
	//����
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf,0,3);
	//С��
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "s");
	showDiagnosLen++;
	memset(Numbuf,0,3);
	
	strcpy(showDiagnosBuf[showDiagnosLen], "maximum Limit:");
	//����������
	tempVal = data[offset++];//bit
	tempVal *= 4;
	//����
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf,0,3);
	//С��
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

	strcpy(showDiagnosBuf[showDiagnosLen], "����ѭ���е���С��������ѹ��");
	showDiagnosLen++;

	strcpy(showDiagnosBuf[showDiagnosLen], "Test Value:");
	//��һ������
	tempVal = data[offset++];//bit
	tempVal *= 5;
	//����
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf,0,3);
	//С��
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "V");
	showDiagnosLen++;
	memset(Numbuf,0,3);
	
	strcpy(showDiagnosBuf[showDiagnosLen], "minimum Limit:");
	//�ڶ�������
	tempVal = data[offset++];//bit
	tempVal *= 5;
	//����
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf,0,3);
	//С��
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "V");
	showDiagnosLen++;
	memset(Numbuf,0,3);
	
	strcpy(showDiagnosBuf[showDiagnosLen], "maximum Limit:");
	//����������
	tempVal = data[offset++];//bit
	tempVal *= 5;
	//����
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf,0,3);
	//С��
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

	strcpy(showDiagnosBuf[showDiagnosLen], "����ѭ���е���󴫸�����ѹ��");
	showDiagnosLen++;

	strcpy(showDiagnosBuf[showDiagnosLen], "Test Value:");
	//��һ������
	tempVal = data[offset++];//bit
	tempVal *= 5;
	//����
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf,0,3);
	//С��
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "V");
	showDiagnosLen++;
	memset(Numbuf,0,3);
	
	strcpy(showDiagnosBuf[showDiagnosLen], "minimum Limit:");
	//�ڶ�������
	tempVal = data[offset++];//bit
	tempVal *= 5;
	//����
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf,0,3);
	//С��
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "V");
	showDiagnosLen++;
	memset(Numbuf,0,3);
	
	strcpy(showDiagnosBuf[showDiagnosLen], "maximum Limit:");
	//����������
	tempVal = data[offset++];//bit
	tempVal *= 5;
	//����
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf,0,3);
	//С��
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

	strcpy(showDiagnosBuf[showDiagnosLen], "���������л�ʱ�䣺");
	showDiagnosLen++;
	
	strcpy(showDiagnosBuf[showDiagnosLen], "Test Value:");
	//��һ������
	tempVal = data[offset++];//bit
	tempVal *= 4;
	//����
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf,0,3);
	//С��
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "s");
	showDiagnosLen++;
	memset(Numbuf,0,3);
	
	strcpy(showDiagnosBuf[showDiagnosLen], "minimum Limit:");
	//�ڶ�������
	tempVal = data[offset++];//bit
	tempVal *= 4;
	//����
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf,0,3);
	//С��
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "s");
	showDiagnosLen++;
	memset(Numbuf,0,3);
	
	strcpy(showDiagnosBuf[showDiagnosLen], "maximum Limit:");
	//����������
	tempVal = data[offset++];//bit
	tempVal *= 4;
	//����
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf,0,3);
	//С��
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

	strcpy(showDiagnosBuf[showDiagnosLen], "�����������ڣ�");
	showDiagnosLen++;
	
	strcpy(showDiagnosBuf[showDiagnosLen], "Test Value:");
	//��һ������
	tempVal = data[offset++];//bit
	tempVal *= 4;
	//����
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf,0,3);
	//С��
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "s");
	showDiagnosLen++;
	memset(Numbuf,0,3);
	
	strcpy(showDiagnosBuf[showDiagnosLen], "minimum Limit:");
	//�ڶ�������
	tempVal = data[offset++];//bit
	tempVal *= 4;
	//����
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf,0,3);
	//С��
	strcat(showDiagnosBuf[showDiagnosLen], ".");
	tempVal_f = tempVal%1000;
	IntToSysChar(tempVal_f, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	strcat(showDiagnosBuf[showDiagnosLen], "s");
	showDiagnosLen++;
	memset(Numbuf,0,3);
	
	strcpy(showDiagnosBuf[showDiagnosLen], "maximum Limit:");
	//����������
	tempVal = data[offset++];//bit
	tempVal *= 4;
	//����
	tempVal_i = tempVal/1000;
	IntToSysChar(tempVal_i, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf,0,3);
	//С��
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
					return 1;//�������մ���
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
					return 1;//�������մ���
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
			strcpy(showDiagnosBuf[showDiagnosLen], "��ϡ��Ũ��������ֵ��ѹ����ֵ��");
			IntToSysChar(tempVal, Numbuf, 10);
			strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
			strcat(showDiagnosBuf[showDiagnosLen], "V");
			showDiagnosLen++;
			break;
		case 0x02:
			strcpy(showDiagnosBuf[showDiagnosLen], "��Ũ��ϡ��������ֵ��ѹ����ֵ��");
			IntToSysChar(tempVal, Numbuf, 10);
			strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
			strcat(showDiagnosBuf[showDiagnosLen], "V");
			showDiagnosLen++;
			break;
		case 0x03:
			strcpy(showDiagnosBuf[showDiagnosLen], "�����л�ʱ�����Ĵ������͵�ѹ��");
			IntToSysChar(tempVal, Numbuf, 10);
			strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
			strcat(showDiagnosBuf[showDiagnosLen], "V");
			showDiagnosLen++;

			break;
		case 0x04:
			strcpy(showDiagnosBuf[showDiagnosLen], "�����л�ʱ�����Ĵ������ߵ�ѹ��");
			IntToSysChar(tempVal, Numbuf, 10);
			strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
			strcat(showDiagnosBuf[showDiagnosLen], "V");
			showDiagnosLen++;
			break;
		case 0x05:
			strcpy(showDiagnosBuf[showDiagnosLen], "��Ũ��ϡ���������л�ʱ�䣺");
			IntToSysChar(tempVal, Numbuf, 10);
			strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
			strcat(showDiagnosBuf[showDiagnosLen], "s");
			showDiagnosLen++;
			break;
		case 0x06:
			strcpy(showDiagnosBuf[showDiagnosLen], "��ϡ��Ũ���������л�ʱ�䣺");
			IntToSysChar(tempVal, Numbuf, 10);
			strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
			strcat(showDiagnosBuf[showDiagnosLen], "s");
			showDiagnosLen++;
			break;
		case 0x07:
			strcpy(showDiagnosBuf[showDiagnosLen], "����ѭ���е���С��������ѹ��");
			IntToSysChar(tempVal, Numbuf, 10);
			strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
			strcat(showDiagnosBuf[showDiagnosLen], "V");
			showDiagnosLen++;
			break;
		case 0x08:
			strcpy(showDiagnosBuf[showDiagnosLen], "����ѭ���е���󴫸�����ѹ��");
			IntToSysChar(tempVal, Numbuf, 10);
			strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
			strcat(showDiagnosBuf[showDiagnosLen], "V");
			showDiagnosLen++;
			break;
		case 0x09:
			strcpy(showDiagnosBuf[showDiagnosLen], "���������л�ʱ�䣺");
			IntToSysChar(tempVal, Numbuf, 10);
			strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
			strcat(showDiagnosBuf[showDiagnosLen], "s");
			showDiagnosLen++;
			break;
		case 0x0A:
			strcpy(showDiagnosBuf[showDiagnosLen], "���������ڣ�");
			IntToSysChar(tempVal, Numbuf, 10);
			strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
			strcat(showDiagnosBuf[showDiagnosLen], "s");
			showDiagnosLen++;
			break;
		default:
			break;
	}

	//����
	strcpy(showDiagnosBuf[showDiagnosLen], "���Բ���ID��");

	IntToSysChar((tempType&0x7F), Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	showDiagnosLen++;
	memset(Numbuf, 0, 5);
	
	strcpy(showDiagnosBuf[showDiagnosLen], "����ֵ��");
	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	showDiagnosLen++;
	memset(Numbuf, 0, 5);
	//���Ե�ֵ���/��С
	if(tempType & 0x80)
	{//��С
		
		strcpy(showDiagnosBuf[showDiagnosLen], "��С����ֵ��");
		IntToSysChar(tempLimitVal, Numbuf, 10);
		strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
		showDiagnosLen++;
		memset(Numbuf, 0, 5);
		
		if(tempVal >= tempLimitVal)
			strcpy(showDiagnosBuf[showDiagnosLen], "���Գɹ�");
		else
			strcpy(showDiagnosBuf[showDiagnosLen], "����ʧ��");
		showDiagnosLen++;
	}
	else
	{//���
		strcpy(showDiagnosBuf[showDiagnosLen], "�����ֵ��");
		IntToSysChar(tempLimitVal, Numbuf, 10);
		strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
		showDiagnosLen++;
		memset(Numbuf, 0, 5);

		if(tempVal <= tempLimitVal)
			strcpy(showDiagnosBuf[showDiagnosLen], "���Գɹ�");
		else
			strcpy(showDiagnosBuf[showDiagnosLen], "����ʧ��");
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
	//ϵ������
	int tempcoefft = 0;
	int tempfbits = 0;

	//����
	int tempSymbol = 0;
	//ʱ����ʾ����
	int tempTimeDel = 0;

	char Numbuf[6] = {0};
	char unitbuf[10] = {0};

	while(offset < len-1)
	{
		tempOBDMID = data[offset++];
		switch(tempOBDMID)
		{
			case 0x01:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [01]BANK1����������1�������");
				showDiagnosLen++;
				break;
			case 0x02:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [02]BANK1����������2�������");
				showDiagnosLen++;
				break;
			case 0x03:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [03]BANK1����������3�������");
				showDiagnosLen++;
				break;
			case 0x04:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [04]BANK1����������4�������");
				showDiagnosLen++;
				break;
			case 0x05:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [05]BANK2����������1�������");
				showDiagnosLen++;
				break;
			case 0x06:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [06]BANK2����������2�������");
				showDiagnosLen++;
				break;
			case 0x07:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [07]BANK2����������3�������");
				showDiagnosLen++;
				break;
			case 0x08:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [08]BANK2����������4�������");
				showDiagnosLen++;
				break;
			case 0x09:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [09]BANK3����������1�������");
				showDiagnosLen++;
				break;
			case 0x0a:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [0A]BANK3����������2�������");
				showDiagnosLen++;
				break;
			case 0x0b:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [0B]BANK3����������3�������");
				showDiagnosLen++;
				break;
			case 0x0c:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [0C]BANK3����������4�������");
				showDiagnosLen++;
				break;
			case 0x0d:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [0D]BANK4����������1�������");
				showDiagnosLen++;
				break;
			case 0x0e:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [0E]BANK4����������2�������");
				showDiagnosLen++;
				break;
			case 0x0f:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [0F]BANK4����������3�������");
				showDiagnosLen++;
				break;
			case 0x10:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [10]BANK4����������4�������");
				showDiagnosLen++;
				break;
			case 0x21:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [21]BANK1�д߻����������");
				showDiagnosLen++;
				break;
			case 0x22:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [22]BANK2�д߻����������");
				showDiagnosLen++;
				break;
			case 0x23:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [23]BANK3�д߻����������");
				showDiagnosLen++;
				break;
			case 0x24:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [24]BANK4�д߻����������");
				showDiagnosLen++;
				break;
			case 0x31:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [31]BANK1��EGR�������");
				showDiagnosLen++;
				break;
			case 0x32:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [32]BANK2��EGR�������");
				showDiagnosLen++;
				break;
			case 0x33:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [33]BANK3��EGR�������");
				showDiagnosLen++;
				break;
			case 0x34:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [34]BANK4��EGR�������");
				showDiagnosLen++;
				break;
			case 0x39:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [39]EVAP�����(�ر�)��");
				showDiagnosLen++;
				break;
			case 0x3a:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [3A]EVAP�����(0,090)��");
				showDiagnosLen++;
				break;
			case 0x3b:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [3B]EVAP�����(0,040)��");
				showDiagnosLen++;
				break;
			case 0x3c:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [3C]EVAP�����(0,020)��");
				showDiagnosLen++;
				break;
			case 0x3d:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [3D]��ϴ�����������");
				showDiagnosLen++;
				break;
			case 0x41:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [41]BANK1 �����м��ȼ����1��");
				showDiagnosLen++;
				break;
			case 0x42:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [42]BANK1 �����м��ȼ����2��");
				showDiagnosLen++;
				break;
			case 0x43:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [43]BANK1 �����м��ȼ����3��");
				showDiagnosLen++;
				break;
			case 0x44:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [44]BANK1 �����м��ȼ����4��");
				showDiagnosLen++;
				break;
			case 0x45:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [45]BANK2 �����м��ȼ����1��");
				showDiagnosLen++;
				break;
			case 0x46:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [46]BANK2 �����м��ȼ����2��");
				showDiagnosLen++;
				break;
			case 0x47:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [47]BANK2 �����м��ȼ����3��");
				showDiagnosLen++;
				break;
			case 0x48:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [48]BANK2 �����м��ȼ����4��");
				showDiagnosLen++;
				break;
			case 0x49:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [49]BANK3 �����м��ȼ����1��");
				showDiagnosLen++;
				break;
			case 0x4a:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [4A]BANK3 �����м��ȼ����2��");
				showDiagnosLen++;
				break;
			case 0x4b:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [4B]BANK3 �����м��ȼ����3��");
				showDiagnosLen++;
				break;
			case 0x4c:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [4C]BANK3 �����м��ȼ����4��");
				showDiagnosLen++;
				break;
			case 0x4d:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [4D]BANK4 �����м��ȼ����1��");
				showDiagnosLen++;
				break;
			case 0x4e:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [4E]BANK4 �����м��ȼ����2��");
				showDiagnosLen++;
				break;
			case 0x4f:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [4F]BANK4 �����м��ȼ����3��");
				showDiagnosLen++;
				break;
			case 0x50:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [50]BANK4 �����м��ȼ����4��");
				showDiagnosLen++;
				break;
			case 0x61:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [61]BANK1 ���ȴ߻����������");
				showDiagnosLen++;
				break;
			case 0x62:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [62]BANK2 ���ȴ߻����������");
				showDiagnosLen++;
				break;
			case 0x63:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [63]BANK3 ���ȴ߻����������");
				showDiagnosLen++;
				break;
			case 0x64:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [64]BANK4 ���ȴ߻����������");
				showDiagnosLen++;
				break;
			case 0x71:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [71]���ο��������1��");
				showDiagnosLen++;
				break;
			case 0x72:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [72]���ο��������2��");
				showDiagnosLen++;
				break;
			case 0x73:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [73]���ο��������3��");
				showDiagnosLen++;
				break;
			case 0x74:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [74]���ο��������4��");
				showDiagnosLen++;
				break;
			case 0x81:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [81]ȼ��ϵͳ�����BANK1��");
				showDiagnosLen++;
				break;
			case 0x82:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [82]ȼ��ϵͳ�����BANK2��");
				showDiagnosLen++;
				break;
			case 0x83:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [83]ȼ��ϵͳ�����BANK3��");
				showDiagnosLen++;
				break;
			case 0x84:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [84]ȼ��ϵͳ�����BANK4��");
				showDiagnosLen++;
				break;
			case 0xa1:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [A1]ʧ�����������ݣ�");
				showDiagnosLen++;
				break;
			case 0xa2:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [A2]ʧ����������1��");
				showDiagnosLen++;
				break;
			case 0xa3:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [A3]ʧ����������2��");
				showDiagnosLen++;
				break;
			case 0xa4:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [A4]ʧ����������3��");
				showDiagnosLen++;
				break;
			case 0xa5:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [A5]ʧ����������4��");
				showDiagnosLen++;
				break;
			case 0xa6:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [A6]ʧ����������5��");
				showDiagnosLen++;
				break;
			case 0xa7:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [A7]ʧ����������6��");
				showDiagnosLen++;
				break;
			case 0xa8:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [A8]ʧ����������7��");
				showDiagnosLen++;
				break;
			case 0xa9:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [A9]ʧ����������8��");
				showDiagnosLen++;
				break;
			case 0xaa:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [AA]ʧ����������9��");
				showDiagnosLen++;
				break;
			case 0xab:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [AB]ʧ����������10��");
				showDiagnosLen++;
				break;
			case 0xac:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [AC]ʧ����������11��");
				showDiagnosLen++;
				break;
			case 0xad:
				strcpy(showDiagnosBuf[showDiagnosLen], "OBDMID [AD]ʧ����������12��");
				showDiagnosLen++;
				break;
			default:
				strcpy(showDiagnosBuf[showDiagnosLen], "Can't identify OBDMID��0x");
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
				strcpy(showDiagnosBuf[showDiagnosLen], "TID [01]��ϡ��Ũ��������ֵ��ѹ");
				showDiagnosLen++;
				break;
			case 0x02:
				strcpy(showDiagnosBuf[showDiagnosLen], "TID [02]��Ũ��ϡ��������ֵ��ѹ");
				showDiagnosLen++;
				break;
			case 0x03:
				strcpy(showDiagnosBuf[showDiagnosLen], "TID [03]�����л�ʱ�����Ĵ������͵�ѹ");
				showDiagnosLen++;
				break;
			case 0x04:
				strcpy(showDiagnosBuf[showDiagnosLen], "TID [04]�����л�ʱ�����Ĵ������ߵ�ѹ");
				showDiagnosLen++;
				break;
			case 0x05:
				strcpy(showDiagnosBuf[showDiagnosLen], "TID [05]��Ũ��ϡ���������л�ʱ��");
				showDiagnosLen++;
				break;
			case 0x06:
				strcpy(showDiagnosBuf[showDiagnosLen], "TID [06]��ϡ��Ũ���������л�ʱ��");
				showDiagnosLen++;
				break;
			case 0x07:
				strcpy(showDiagnosBuf[showDiagnosLen], "TID [07]����ѭ���е���С��������ѹ");
				showDiagnosLen++;
				break;
			case 0x08:
				strcpy(showDiagnosBuf[showDiagnosLen], "TID [08]����ѭ���е���󴫸�����ѹ");
				showDiagnosLen++;
				break;
			case 0x09:
				strcpy(showDiagnosBuf[showDiagnosLen], "TID [09]���������л�ʱ��");
				showDiagnosLen++;
				break;
			case 0x0a:
				strcpy(showDiagnosBuf[showDiagnosLen], "TID [0A]������������");
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
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [07]��תƵ��.");
				tempcoefft = 25;
				tempfbits = 100;
				strcpy(unitbuf, "rpm");
				break;
			case 0x08:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [08]�ٶ�.");
				tempcoefft = 62137;
				tempfbits = 10000000;
				strcpy(unitbuf, "mph");
				break;
			case 0x09:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [09]�ٶ�.");
				tempcoefft = 62137;
				tempfbits = 100000;
				strcpy(unitbuf, "mph");
				break;
			case 0x0a:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [0A]��ѹ.");
				tempcoefft = 122;
				tempfbits = 1000000;
				strcpy(unitbuf, "V");
				break;
			case 0x0b:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [0B]��ѹ.");
				tempcoefft = 1;
				tempfbits = 1000;
				strcpy(unitbuf, "V");
				break;
			case 0x0c:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [0C]��ѹ.");
				tempcoefft = 1;
				tempfbits = 100;
				strcpy(unitbuf, "V");
				break;
			case 0x0d:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [0D]����.");
				tempcoefft = 390625;
				tempfbits = 100000000;
				strcpy(unitbuf, "mA");
				break;
			case 0x0e:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [0E]����.");
				tempcoefft = 1;
				tempfbits = 1000;
				strcpy(unitbuf, "A");
				break;
			case 0x0f:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [0F]����.");
				tempcoefft = 1;
				tempfbits = 100;
				strcpy(unitbuf, "A");
				break;
			case 0x10:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [10]ʱ��.");
				tempcoefft = 1;
				tempfbits = 1000;
				tempTimeDel = 1;
				strcpy(unitbuf, "s");		//����
				break;
			case 0x11:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [11]ʱ��.");
				tempcoefft = 1;
				tempfbits = 10;
				tempTimeDel = 1;
				strcpy(unitbuf, "s");		//����
				break;
			case 0x12:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [12]ʱ��.");
				tempcoefft = 1;
				tempfbits = 1;
				tempTimeDel = 1;
				strcpy(unitbuf, "s");		//����
				break;
			case 0x13:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [13]����.");
				tempcoefft = 1;
				tempfbits = 1000;
				strcpy(unitbuf, "Ohm");
			case 0x14:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [14]����.");
				tempcoefft = 1;
				tempfbits = 1000;
				strcpy(unitbuf, "kOhm");
				break;
			case 0x15:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [15]����.");
				tempcoefft = 1;
				tempfbits = 1;
				strcpy(unitbuf, "kOhm");
				break;
			case 0x16:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [16]�¶�.");
				tempcoefft = 1;
				tempfbits = 10;
				if(tempVal > 400)
					tempVal -= 400;
				else
				{
					tempSymbol = 1;
					tempVal = 400 - tempVal;
				}
				strcpy(unitbuf, "��");
				break;
			case 0x17:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [17]ѹ����.");
				tempcoefft = 1;
				tempfbits = 100;
				strcpy(unitbuf, "kpa");
				break;
			case 0x18:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [18]ѹ������ѹ��.");
				tempcoefft = 117;
				tempfbits = 10000;
				strcpy(unitbuf, "kpa");
				break;
			case 0x19:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [19]ѹ����ȼ�ϣ�.");
				tempcoefft = 79;
				tempfbits = 1000;
				strcpy(unitbuf, "kpa");
				break;
			case 0x1a:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [1A]ѹ����.");
				tempcoefft = 1;
				tempfbits = 1;
				strcpy(unitbuf, "kpa");
				break;
			case 0x1b:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [1B]ѹ�������ͣ�.");
				tempcoefft = 10;
				tempfbits = 1;
				strcpy(unitbuf, "kpa");
				break;
			case 0x1c:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [1C]�Ƕ�.");
				tempcoefft = 1;
				tempfbits = 100;
				strcpy(unitbuf, "��");
				break;
			case 0x1d:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [1D]�Ƕ�.");
				tempcoefft = 5;
				tempfbits = 10;
				strcpy(unitbuf, "��");
				break;
			case 0x1e:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [1E]�ȼ۱�.");
				tempcoefft = 305;
				tempfbits = 10000000;
				strcpy(unitbuf, "");
				break;
			case 0x1f:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [1F]����/ȼ�ϱ�.");
				tempcoefft = 5;
				tempfbits = 100;
				strcpy(unitbuf, "");
				break;
			case 0x20:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [20]����.");
				tempcoefft = 39062;
				tempfbits = 10000000;
				strcpy(unitbuf, "");
				break;
			case 0x21:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [21]Ƶ��.");
				tempcoefft = 1;
				tempfbits = 1000;
				strcpy(unitbuf, "Hz");
				break;
			case 0x22:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [22]Ƶ��.");
				tempcoefft = 1;
				tempfbits = 1;
				strcpy(unitbuf, "Hz");
				break;
			case 0x23:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [23]Ƶ��.");
				tempcoefft = 1;
				tempfbits = 1000;
				strcpy(unitbuf, "MHz");
				break;
			case 0x24:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [24]����.");
				tempcoefft = 1;
				tempfbits = 1;
				strcpy(unitbuf, "");
				break;
			case 0x25:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [25]����.");
				tempcoefft = 62137;
				tempfbits = 100000;
				strcpy(unitbuf, "miles");
				break;
			case 0x26:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [26]��ѹ/ʱ��.");
				tempcoefft = 1;
				tempfbits = 10000;
				strcpy(unitbuf, "V/ms");
				break;
			case 0x27:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [27]����/ʱ��.");
				tempcoefft = 22046;
				tempfbits = 1000000000;
				strcpy(unitbuf, "lb/s");
				break;
			case 0x28:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [28]����/ʱ��.");
				tempcoefft = 22046;
				tempfbits = 10000000;
				strcpy(unitbuf, "lb/s");
				break;
			case 0x29:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [29]��ѹ/ʱ��.");
				tempcoefft = 25;
				tempfbits = 100000;
				strcpy(unitbuf, "kpa/s");
				break;
			case 0x2a:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [2A]����/ʱ��.");
				tempcoefft = 1;
				tempfbits = 1000;
				strcpy(unitbuf, "kg/h");
				break;
			case 0x2b:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [2B]����.");
				tempcoefft = 1;
				tempfbits = 1;
				strcpy(unitbuf, "");
				break;
			case 0x2c:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [2C]����/��.");
				tempcoefft = 1;
				tempfbits = 100;
				strcpy(unitbuf, "g/cyl");
				break;
			case 0x2d:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [2D]����/��.");
				tempcoefft = 1;
				tempfbits = 100;
				strcpy(unitbuf, "mg/stroke");
				break;
			case 0x2e:
				//true or false
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [2E]��/��.");

				if(tempVal & 0x01)
					strcat(showDiagnosBuf[showDiagnosLen], "��");
				else
					strcat(showDiagnosBuf[showDiagnosLen], "��");
				showDiagnosLen++;
				return;
			case 0x2f:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [2F]�ٷֱ�.");
				tempcoefft = 1;
				tempfbits = 100;
				strcpy(unitbuf, "%");
				break;
			case 0x30:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [30]�ٷֱ�.");
				tempcoefft = 1526;
				tempfbits = 1000000;
				strcpy(unitbuf, "%");
				break;
			case 0x31:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [31]���.");
				tempcoefft = 1;
				tempfbits = 1000;
				strcpy(unitbuf, "L");
				break;
			case 0x32:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [32]����.");
				tempcoefft = 305;
				tempfbits = 10000000;
				strcpy(unitbuf, "inch");
				break;
			case 0x33:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [33]�ȼ۱���.");
				tempcoefft = 24414;
				tempfbits = 100000000;
				strcpy(unitbuf, "");
				break;
			case 0x34:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [34]ʱ��.");
				tempcoefft = 1;
				tempfbits = 1;
				tempTimeDel = 1;
				strcpy(unitbuf, "min");
				break;
			case 0x35:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [35]ʱ��.");
				tempcoefft = 1;
				tempfbits = 100;
				tempTimeDel = 1;
				strcpy(unitbuf, "s");			//����
				break;
			case 0x36:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [36]���.");
				tempcoefft = 22075;
				tempfbits = 1000000000;
				strcpy(unitbuf, "lbs");
				break;
			case 0x37:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [37]���.");
				tempcoefft = 22075;
				tempfbits = 100000000;
				strcpy(unitbuf, "lbs");
				break;
			case 0x38:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [38]���.");
				tempcoefft = 22075;
				tempfbits = 10000000;
				strcpy(unitbuf, "lbs");
				break;
			case 0x39:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [39]�ٷֱ�.");
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
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [8A]��ѹ.");
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
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [8B]��ѹ.");
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
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [8C]��ѹ.");
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
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [8D]����.");
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
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [8E]����.");
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
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [90]ʱ��.");
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
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [96]�¶�.");
				tempcoefft = 1;
				tempfbits = 10;

				if(tempVal & 0x8000)
				{
					tempVal &= 0x7FFF;
					tempSymbol = 1;
					tempVal = 0x8000 - tempVal;
				}
				strcpy(unitbuf, "��");
				break;
			case 0x9c:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [9C]�Ƕ�.");
				tempcoefft = 1;
				tempfbits = 100;

				if(tempVal & 0x8000)
				{
					tempVal &= 0x7FFF;
					tempSymbol = 1;
					tempVal = 0x8000 - tempVal;
				}
				strcpy(unitbuf, "��");
				break;
			case 0x9d:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [9D]�Ƕ�.");
				tempcoefft = 5;
				tempfbits = 10;

				if(tempVal & 0x8000)
				{
					tempVal &= 0x7FFF;
					tempSymbol = 1;
					tempVal = 0x8000 - tempVal;
				}
				strcpy(unitbuf, "��");
				break;
			case 0xa8:
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [A8]����/ʱ��.");
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
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [A9]ѹ��/ʱ��.");
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
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [AF]�ٷֱ�.");
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
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [B0]�ٷֱ�.");
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
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [B1]��ѹ/ʱ��.");
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
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [FD]����ѹ��.");
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
				strcpy(showDiagnosBuf[showDiagnosLen], "UASID [FE]���ԣ���գ�.");
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
				strcpy(showDiagnosBuf[showDiagnosLen], "Can't identify UASID��0x");
				IntToSysChar(unitScalID, Numbuf, 16);
				strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
				tempcoefft = 1;
				memset(Numbuf, 0, 6);
				break;
		}
		showDiagnosLen++;
		
		if(tempSymbol == 1)//����
			strcat(showDiagnosBuf[showDiagnosLen], "-");
		if(tempTimeDel == 0 
			&& tempTimeDel == 0)//���� �Ҳ���ʱ����
		{
			strcat(showDiagnosBuf[showDiagnosLen], "Test Value��");
			temp = tempVal * tempcoefft;
			//����
			tempVal_i = temp/tempfbits;
			IntToSysChar(tempVal_i, Numbuf, 10);
			strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
			strcat(showDiagnosBuf[showDiagnosLen], ".");
			memset(Numbuf, 0, 6);
			//С��
			if(tempfbits > 1)
			{
				tempVal_f = temp%tempfbits;
				while(tempVal_f >= 10000)
					tempVal_f	/= 10;

				IntToSysChar(tempVal_f, Numbuf, 10);
				strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
				memset(Numbuf, 0, 6);
			}
			//��λ
			strcat(showDiagnosBuf[showDiagnosLen], unitbuf);
			showDiagnosLen++;
			strcat(showDiagnosBuf[showDiagnosLen], "MIN.Test Limit��");
			temp = tempMin * tempcoefft;
			
			//����
			tempVal_i = temp/tempfbits;
			IntToSysChar(tempVal_i, Numbuf, 10);
			strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
			strcat(showDiagnosBuf[showDiagnosLen], ".");
			memset(Numbuf, 0, 6);
			//С��
			if(tempfbits > 1)
			{
				tempVal_f = temp%tempfbits;
				while(tempVal_f >= 10000)
					tempVal_f	/= 10;

				IntToSysChar(tempVal_f, Numbuf, 10);
				strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
				memset(Numbuf, 0, 6);
			}
			//��λ
			strcat(showDiagnosBuf[showDiagnosLen], unitbuf);
			showDiagnosLen++;
			strcat(showDiagnosBuf[showDiagnosLen], "MAX.Test Limit��");
			temp = tempMax * tempcoefft;
			//����
			tempVal_i = temp/tempfbits;
			IntToSysChar(tempVal_i, Numbuf, 10);
			strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
			strcat(showDiagnosBuf[showDiagnosLen], ".");
			memset(Numbuf, 0, 6);
			//С��
			if(tempfbits > 1)
			{
				tempVal_f = temp%tempfbits;
				while(tempVal_f >= 10000)
					tempVal_f	/= 10;

				IntToSysChar(tempVal_f, Numbuf, 10);
				strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
				memset(Numbuf, 0, 6);
			}
			//��λ
			strcat(showDiagnosBuf[showDiagnosLen], unitbuf);
			showDiagnosLen++;
		}
		else if(tempTimeDel == 1)//ʱ��
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
					return 1;//�������մ���
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
					return 1;//�������մ���
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
					return 1;//�������մ���
				}
			}
			else
			{
				return ret;
			}
		}
		else
		{
//			txlen = kwpFmt((unsigned char *)commbuf, 2);//ͷ����Ϣ
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
					return 1;//�������մ���
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
					return 1;//�������մ���
				}
			}
			else
			{
				return ret;
			}
		}
		else
		{
//			txlen = kwpFmt((unsigned char *)commbuf, 2);//ͷ����Ϣ
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
					return 1;//�������մ���
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

	//��һ������
	strcpy(showDiagnosBuf[showDiagnosLen], "info01 [A]VIN��Ϣ������");
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
	//��һ������
	strcpy(showDiagnosBuf[showDiagnosLen], "info02 [17Bytes]VIN��");
	if(diagnose.commtypes == 0)
	{
		memcpy(Numbuf, &data[1], 17);
		strcat(showDiagnosBuf[showDiagnosLen], (char *)Numbuf);//�������
	}
	else
	{
		memcpy(Numbuf, &data[3], 17);
		strcat(showDiagnosBuf[showDiagnosLen], (char *)Numbuf);//���3��0x00
	}
	showDiagnosLen++;
}

void analysis_sid09_tid03(char *data)
{
	int offset = 0;
	int tempVal = 0;
	char Numbuf[3] = {0};

	//��һ������
	strcpy(showDiagnosBuf[showDiagnosLen], "info03 [A]CALID��Ϣ������");
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

	strcpy(showDiagnosBuf[showDiagnosLen], "info04 [16bytes]Calibration ID Number��");
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
		strcat(showDiagnosBuf[showDiagnosLen], "��");
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

	//��һ������
	strcpy(showDiagnosBuf[showDiagnosLen], "info05 [A]CVN��Ϣ������");
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

	strcpy(showDiagnosBuf[showDiagnosLen], "info06 [4bytes]Calibration Verification Number��");
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
		strcat(showDiagnosBuf[showDiagnosLen], "��0x");
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

	//��һ������
	strcpy(showDiagnosBuf[showDiagnosLen], "info07 [A]IPT��Ϣ������");
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

	strcpy(showDiagnosBuf[showDiagnosLen], "info08 [0]���ܸ��ٸ�����");
	
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

	strcpy(showDiagnosBuf[showDiagnosLen], "info08 [AB]OBD������������������");
	tempVal = data[offset++];
	tempVal <<= 8;
	tempVal |= data[offset++];

	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf, 0, 5);
	showDiagnosLen++;

	strcpy(showDiagnosBuf[showDiagnosLen], "info08 [AB]������ڼ�������");
	tempVal = data[offset++];
	tempVal <<= 8;
	tempVal |= data[offset++];

	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf, 0, 5);
	showDiagnosLen++;

	strcpy(showDiagnosBuf[showDiagnosLen], "info08 [AB]Bank1�߻��������ɴ�����");
	tempVal = data[offset++];
	tempVal <<= 8;
	tempVal |= data[offset++];

	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf, 0, 5);
	showDiagnosLen++;

	strcpy(showDiagnosBuf[showDiagnosLen], "info08 [AB]Bank1�߻�����������Ĵ�����");
	tempVal = data[offset++];
	tempVal <<= 8;
	tempVal |= data[offset++];

	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf, 0, 5);
	showDiagnosLen++;

	strcpy(showDiagnosBuf[showDiagnosLen], "info08 [AB]Bank2�߻��������ɴ�����");
	tempVal = data[offset++];
	tempVal <<= 8;
	tempVal |= data[offset++];

	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf, 0, 5);
	showDiagnosLen++;

	strcpy(showDiagnosBuf[showDiagnosLen], "info08 [AB]Bank2�߻�����������Ĵ�����");
	tempVal = data[offset++];
	tempVal <<= 8;
	tempVal |= data[offset++];

	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf, 0, 5);
	showDiagnosLen++;

	strcpy(showDiagnosBuf[showDiagnosLen], "info08 [AB]Bank1�������������ɴ�����");
	tempVal = data[offset++];
	tempVal <<= 8;
	tempVal |= data[offset++];

	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf, 0, 5);
	showDiagnosLen++;

	strcpy(showDiagnosBuf[showDiagnosLen], "info08 [AB]Bank1����������������Ĵ�����");
	tempVal = data[offset++];
	tempVal <<= 8;
	tempVal |= data[offset++];

	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf, 0, 5);
	showDiagnosLen++;

	strcpy(showDiagnosBuf[showDiagnosLen], "info08 [AB]Bank2�������������ɴ�����");
	tempVal = data[offset++];
	tempVal <<= 8;
	tempVal |= data[offset++];

	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf, 0, 5);
	showDiagnosLen++;

	strcpy(showDiagnosBuf[showDiagnosLen], "info08 [AB]Bank2����������������Ĵ�����");
	tempVal = data[offset++];
	tempVal <<= 8;
	tempVal |= data[offset++];

	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf, 0, 5);
	showDiagnosLen++;

	strcpy(showDiagnosBuf[showDiagnosLen], "info08 [AB]ERG/VVT�����ɴ�����");
	tempVal = data[offset++];
	tempVal <<= 8;
	tempVal |= data[offset++];

	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf, 0, 5);
	showDiagnosLen++;

	strcpy(showDiagnosBuf[showDiagnosLen], "info08 [AB]ERG/VVT��������Ĵ�����");
	tempVal = data[offset++];
	tempVal <<= 8;
	tempVal |= data[offset++];

	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf, 0, 5);
	showDiagnosLen++;

	strcpy(showDiagnosBuf[showDiagnosLen], "info08 [AB]���������ɴ�����");
	tempVal = data[offset++];
	tempVal <<= 8;
	tempVal |= data[offset++];

	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf, 0, 5);
	showDiagnosLen++;

	strcpy(showDiagnosBuf[showDiagnosLen], "info08 [AB]������������Ĵ�����");
	tempVal = data[offset++];
	tempVal <<= 8;
	tempVal |= data[offset++];

	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf, 0, 5);
	showDiagnosLen++;

	strcpy(showDiagnosBuf[showDiagnosLen], "info08 [AB]EVAP�����ɴ�����");
	tempVal = data[offset++];
	tempVal <<= 8;
	tempVal |= data[offset++];

	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);
	memset(Numbuf, 0, 5);
	showDiagnosLen++;

	strcpy(showDiagnosBuf[showDiagnosLen], "info08 [AB]EVAP��������Ĵ�����");
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

	//��һ������
	strcpy(showDiagnosBuf[showDiagnosLen], "info09 [A]ECUNAME��Ϣ������");
	tempVal = data[offset];

	IntToSysChar(tempVal, Numbuf, 10);
	strcat(showDiagnosBuf[showDiagnosLen], Numbuf);

	MessageCount9 = tempVal;
	
	showDiagnosLen++;
}

void analysis_sid09_tid0A(char *data)
{
	//��һ������
	strcpy(showDiagnosBuf[showDiagnosLen], "info0A [20Bytes]Ecu-EcuName:");
	if(diagnose.commtypes == 0)
	{
		strcat(showDiagnosBuf[showDiagnosLen], &data[1]);
		strcat(showDiagnosBuf[showDiagnosLen], &data[5]);//��ȫ��������
		showDiagnosLen++;
	}
	else
	{
		strcat(showDiagnosBuf[showDiagnosLen], &data[0]);
		strcat(showDiagnosBuf[showDiagnosLen], &data[4]);//��ȫ��������
		showDiagnosLen++;
	}
}

void analysis_tid(char *rxbuf, int len)
{
	//δ���յ�����
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
	//δ���յ�����
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

//���������������ѯ
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
		return ret;//��ѯʧ��
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
			if(diagnose.commtypes)//K��ͨѶ��ʽ
			{
				diagnose.server_id = 0x05;
				if(0 == kwp_service_sid(0x01, subid, (unsigned char *)rxbuf, &rxlen))
				{//������Ӧ����
					if(subid== 0x13)
					{
						analysis_sid0102_pid13(&rxbuf[2]);
						for(i = 0; i < o2sensorNumber; i++)
						{
							o2sensorLocation = o2sensorLocationBuf[i];
							//�����������������
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
							//�����������������
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
							//�����������������
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
				strcpy(showDiagnosBuf[showDiagnosLen], "ISO 15765��֧�ֲ�ѯ05����");
				showDiagnosLen++;
				return ret;//��ѯʧ��
			}
		}
	}
	
	if(o2sensorBe == 0)
	{
		strcpy(showDiagnosBuf[showDiagnosLen], "δ�յ���������λ��");
		showDiagnosLen++;
	}
	
	return ret;
}

//���ŵ���
//�����ѯ
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
	//��ѯ�ӹ��ܷ���ָ��
	ret = uds_service_request(&diagnose, (char *)subBuf, &subLen);
	if(0 != ret)
	{
		return ret;//��ѯʧ��
	}
	
	vTaskDelay(100);
	
	//���Ͷ������ָ��
	for(i = 0; i < subLen; i++)
	{
		subid = subBuf[i];
		{
			if(0 == can_service_sid(tempsid, subid, (unsigned char *)rxbuf, &rxlen))
			{//������Ӧ����
				switch(tempsid)
				{
					case 0x01:
					case 0x02:
						analysis_pid(rxbuf, rxlen);
						break;
					case 0x05://�޸÷���
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
			{//��ѯ��PIDʧ�� δ������
				
			}
			vTaskDelay(10);
		}
	}
	return 0;
}

//����
void analysis_pid(char* rxbuf, int len)
{
	int i = 0;
	//δ���յ�����
	if (len <= 0)
	{
		return;
	}
	if (rxbuf[0] == 0x41)
	{//����1
		i = 0;
	}
	else if (rxbuf[0] == 0x42)
	{//����2
		i = 1;
	}
	//�����ظ���Ϣ
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


