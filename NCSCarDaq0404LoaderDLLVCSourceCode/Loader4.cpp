// Loader4.cpp
// (c) 2005 National Control Systems, Inc.
// Portions (c) 2004 Drew Technologies, Inc.
// Dynamic J2534 v04.04 dll loader for VB

// This program is free software; you can redistribute it and/or
// modify it under the terms of the GNU General Public License
// as published by the Free Software Foundation; either version 2
// of the License, or any later version.

// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.

// You should have received a copy of the GNU General Public License
// along with this program; if not, write to:
// the Free Software Foundation, Inc.
// 51 Franklin Street, Fifth Floor
// Boston, MA  02110-1301, USA

// National Control Systems, Inc.
// 10737 Hamburg Rd
// Hamburg, MI 48139
// 810-231-2901

// Drew Technologies, Inc.
// 7012  E.M -36, Suite 3B
// Whitmore Lake, MI 48189
// 810-231-3171

#define STRICT
#include "Loader4.h"

using namespace std;
int GetTxMsg(int TxID, CString TxMsg, unsigned char TxData[], int protocol)
{
	CString str_TxMsg("");
	if (ISO15765 == protocol)
	{
		CString str_TxID;
		str_TxID.Format("%x", TxID);

		str_TxMsg = "00000";
		str_TxMsg.Append(str_TxID);
	}
	else;

	str_TxMsg.Append(TxMsg);
	string strC = str_TxMsg.GetString();

	char   sendbuf[50];
	int   apdu_len = strC.size() / 2;
	for (int i = 0; i < apdu_len; i++)
	{
		string   stmp = strC.substr(i * 2, 2);
		sendbuf[i] = strtoul(stmp.c_str(), NULL, 16);
	}
	memcpy(TxData, sendbuf, apdu_len);

	return apdu_len;
}
int StartFilter(int iRxID, int iTxID, int protocol, int ChannelID)
{
	int istatus;
	CString str_Tx = "";

	PASSTHRU_MSG pMaskMsg;
	pMaskMsg.ProtocolID = protocol;
	if (protocol == ISO15765)
		pMaskMsg.TxFlags = ISO15765_FRAME_PAD;//0x00000040
	else
		pMaskMsg.TxFlags = 0;
	pMaskMsg.DataSize = 4;

	PASSTHRU_MSG pPatternMsg;
	pPatternMsg.ProtocolID = protocol;
	if (protocol == ISO15765)
		pPatternMsg.TxFlags = ISO15765_FRAME_PAD;
	else
		pPatternMsg.TxFlags = 0;
	pPatternMsg.DataSize = 4;

	unsigned long pMsgID = 1;

	PassThruIoctl(ChannelID, CLEAR_RX_BUFFER, NULL, NULL);
	PassThruIoctl(ChannelID, CLEAR_TX_BUFFER, NULL, NULL);

	if (ISO15765 == protocol)				//当选择的是UDS时，过滤ID
	{
		pMaskMsg.Data[0] = 0xFF;
		pMaskMsg.Data[1] = 0xFF;
		pMaskMsg.Data[2] = 0xFF;
		pMaskMsg.Data[3] = 0xFF;

		GetTxMsg(iRxID, str_Tx, pPatternMsg.Data, protocol);

		PASSTHRU_MSG  pFlowControlMsg;
		pFlowControlMsg.ProtocolID = protocol;
		pFlowControlMsg.TxFlags = ISO15765_FRAME_PAD;
		pFlowControlMsg.DataSize = 4;
		GetTxMsg(iTxID, str_Tx, pFlowControlMsg.Data, protocol);

		istatus = PassThruStartMsgFilter(ChannelID, FLOW_CONTROL_FILTER, &pMaskMsg, &pPatternMsg,	//打开flow control filter
			&pFlowControlMsg, &pMsgID);
	}

	else if (ISO14230 == protocol)					//当选择的是KWP时，只过滤源地址和目标地址
	{
		pMaskMsg.Data[0] = 0x00;
		pMaskMsg.Data[1] = 0xFF;
		pMaskMsg.Data[2] = 0xFF;
		pMaskMsg.Data[3] = 0x00;

		pPatternMsg.Data[0] = 0x00;
		pPatternMsg.Data[1] = iRxID;
		pPatternMsg.Data[2] = iTxID;
		pPatternMsg.Data[3] = 0x00;

		int temp = PassThruStartMsgFilter(ChannelID, PASS_FILTER, &pMaskMsg, &pPatternMsg,		//打开pass filter 
			NULL, &pMsgID);
	}
	return istatus;
}


HINSTANCE hDLL = NULL;
unsigned long pDeviceID[3];
unsigned long pChannelID[3];
long gDeviceCount = 0;
J2534DEVICELIST DeviceList[MAX_NUM_DEVICES];
char J2534BoilerplateErrorResult[MAX_STRING_LENGTH];
//BOOL bIsCorrectVersion = FALSE;

BOOL WINAPI DllMain(HINSTANCE hInstA, DWORD dwReason, LPVOID lpvReserved)
{
	switch (dwReason) {
	case DLL_PROCESS_ATTACH:
		// The DLL is being mapped into the process's address space

	case DLL_THREAD_ATTACH:
		// A thread is being created
	    break;

	case DLL_THREAD_DETACH:
		// A thread is exiting cleanly
	    break;

	case DLL_PROCESS_DETACH:
		// The DLL is being unmapped from the process's address space
	    break;
	}

	return TRUE;
}


long WINAPI LoadJ2534Dll(char *sLib)
{
	long lFuncList = 0;

	if (hDLL != NULL) UnloadJ2534Dll();
	hDLL = LoadLibrary (sLib); 
	if (hDLL == NULL) return ERR_NO_DLL;
	
	LocalOpen = (PTOPEN)(GetProcAddress(hDLL, "PassThruOpen"));
	if (LocalOpen == NULL) lFuncList = lFuncList | ERR_NO_PTOPEN;
	
	LocalClose = (PTCLOSE)(GetProcAddress(hDLL, "PassThruClose"));
	if (LocalClose == NULL) lFuncList = lFuncList | ERR_NO_PTCLOSE;
	
	LocalConnect = (PTCONNECT)(GetProcAddress(hDLL,"PassThruConnect"));
	if (LocalConnect == NULL) lFuncList = lFuncList | ERR_NO_PTCONNECT;
	
	LocalDisconnect = (PTDISCONNECT)(GetProcAddress(hDLL,"PassThruDisconnect"));
	if (LocalDisconnect == NULL) lFuncList = lFuncList | ERR_NO_PTDISCONNECT;
	
	LocalReadMsgs = (PTREADMSGS)(GetProcAddress(hDLL,"PassThruReadMsgs"));
	if (LocalReadMsgs == NULL) lFuncList = lFuncList | ERR_NO_PTREADMSGS;

	LocalWriteMsgs = (PTWRITEMSGS)(GetProcAddress(hDLL,"PassThruWriteMsgs"));
	if (LocalWriteMsgs == NULL) lFuncList = lFuncList | ERR_NO_PTWRITEMSGS;
	
	LocalStartPeriodicMsg = (PTSTARTPERIODICMSG)(GetProcAddress(hDLL,"PassThruStartPeriodicMsg"));
	if (LocalStartPeriodicMsg == NULL) lFuncList = lFuncList | ERR_NO_PTSTARTPERIODICMSG;

	LocalStopPeriodicMsg = (PTSTOPPERIODICMSG)(GetProcAddress(hDLL,"PassThruStopPeriodicMsg"));
	if (LocalStopPeriodicMsg == NULL) lFuncList = lFuncList | ERR_NO_PTSTOPPERIODICMSG;

	LocalStartMsgFilter = (PTSTARTMSGFILTER)(GetProcAddress(hDLL,"PassThruStartMsgFilter"));
	if (LocalStartPeriodicMsg == NULL) lFuncList = lFuncList | ERR_NO_PTSTARTMSGFILTER;

	LocalStopMsgFilter = (PTSTOPMSGFILTER)(GetProcAddress(hDLL,"PassThruStopMsgFilter"));
	if (LocalStopMsgFilter == NULL) lFuncList = lFuncList | ERR_NO_PTSTOPMSGFILTER;

	LocalSetProgrammingVoltage = (PTSETPROGRAMMINGVOLTAGE)(GetProcAddress(hDLL,"PassThruSetProgrammingVoltage"));
	if (LocalSetProgrammingVoltage == NULL) lFuncList = lFuncList | ERR_NO_PTSETPROGRAMMINGVOLTAGE;

	LocalReadVersion = (PTREADVERSION)(GetProcAddress(hDLL,"PassThruReadVersion"));
	if (LocalReadVersion == NULL) lFuncList = lFuncList | ERR_NO_PTREADVERSION;

	LocalGetLastError = (PTGETLASTERROR)(GetProcAddress(hDLL,"PassThruGetLastError"));
	if (LocalGetLastError == NULL) lFuncList = lFuncList | ERR_NO_PTGETLASTERROR;
	
	LocalIoctl = (PTIOCTL)(GetProcAddress(hDLL,"PassThruIoctl"));
	if (LocalIoctl == NULL) lFuncList = lFuncList | ERR_NO_PTIOCTL;

	if (lFuncList == ERR_NO_FUNCTIONS) return ERR_WRONG_DLL_VER;

	return lFuncList;
}

long WINAPI UnloadJ2534Dll()
{
	if (FreeLibrary(hDLL))
	{
		hDLL = NULL;
		LocalOpen = NULL;
		LocalClose = NULL;
		LocalConnect = NULL;
		LocalDisconnect = NULL;
		LocalReadMsgs = NULL;
		LocalWriteMsgs = NULL;
		LocalStartPeriodicMsg = NULL;
		LocalStopPeriodicMsg = NULL;
		LocalStartMsgFilter = NULL;
		LocalStopMsgFilter = NULL;
		LocalSetProgrammingVoltage = NULL;
		LocalReadVersion = NULL;
		LocalGetLastError = NULL;
		LocalIoctl = NULL;
		return 0;
	}
	return ERR_NO_DLL;
}

long WINAPI PassThruOpen(void *pName, unsigned long *pDeviceID)
{
	if (LocalOpen == NULL) return ERR_FUNC_MISSING;
	return LocalOpen(pName, pDeviceID);
}

long WINAPI PassThruClose(unsigned long DeviceID)
{
	if (LocalOpen == NULL) return ERR_FUNC_MISSING;
	return LocalClose(DeviceID);
}

long WINAPI PassThruConnect(unsigned long DeviceID, unsigned long ProtocolID, unsigned long Flags, unsigned long Baudrate, unsigned long *pChannelID)
{
	if (LocalConnect == NULL) return ERR_FUNC_MISSING;
	return LocalConnect(DeviceID, ProtocolID, Flags, Baudrate, pChannelID);
}

long WINAPI PassThruDisconnect(unsigned long ChannelID)
{
	if (LocalDisconnect == NULL) return ERR_FUNC_MISSING;
	return LocalDisconnect(ChannelID);
}

long WINAPI PassThruReadMsgs(unsigned long ChannelID, void *pMsg, unsigned long *pNumMsgs, unsigned long Timeout)
{
	if (LocalReadMsgs == NULL) return ERR_FUNC_MISSING;
	return LocalReadMsgs(ChannelID, pMsg, pNumMsgs, Timeout);
}
						
long WINAPI PassThruWriteMsgs(unsigned long ChannelID, void *pMsg, unsigned long *pNumMsgs, unsigned long Timeout)
{
	if (LocalWriteMsgs == NULL) return ERR_FUNC_MISSING;
	return LocalWriteMsgs(ChannelID, pMsg, pNumMsgs, Timeout);
}

long WINAPI PassThruStartPeriodicMsg(unsigned long ChannelID, void *pMsg, unsigned long *pMsgID, unsigned long TimeInterval)
{
	if (LocalStartPeriodicMsg == NULL) return ERR_FUNC_MISSING;
	return LocalStartPeriodicMsg(ChannelID, pMsg, pMsgID, TimeInterval);
}

long WINAPI PassThruStopPeriodicMsg(unsigned long ChannelID, unsigned long MsgID)
{
	if (LocalStopPeriodicMsg == NULL) return ERR_FUNC_MISSING;
	return LocalStopPeriodicMsg(ChannelID, MsgID);
}

long WINAPI PassThruStartMsgFilter(unsigned long ChannelID, unsigned long FilterType, void *pMaskMsg, void *pPatternMsg, void *pFlowControlMsg, unsigned long *pFilterID)
{
	if (LocalStartMsgFilter == NULL) return ERR_FUNC_MISSING;
	return LocalStartMsgFilter(ChannelID, FilterType, pMaskMsg, pPatternMsg, pFlowControlMsg, pFilterID);
}

long WINAPI PassThruStopMsgFilter(unsigned long ChannelID, unsigned long FilterID)
{
	if (LocalStopMsgFilter == NULL) return ERR_FUNC_MISSING;
	return LocalStopMsgFilter(ChannelID, FilterID);
}

long WINAPI PassThruSetProgrammingVoltage(unsigned long DeviceID, unsigned long PinNumber, unsigned long Voltage)
{
	if (LocalSetProgrammingVoltage == NULL) return ERR_FUNC_MISSING;
	return LocalSetProgrammingVoltage(DeviceID, PinNumber, Voltage);
}

long WINAPI PassThruReadVersion(unsigned long DeviceID, char *pFirmwareVersion, char *pDllVersion, char *pApiVersion)
{
	if (LocalReadVersion == NULL) return ERR_FUNC_MISSING;
	return LocalReadVersion(DeviceID, pFirmwareVersion, pDllVersion, pApiVersion);
}

long WINAPI PassThruGetLastError(char *pErrorDescription)
{
	if (LocalGetLastError == NULL) return ERR_FUNC_MISSING;
	return LocalGetLastError(pErrorDescription);
}

long WINAPI PassThruIoctl(unsigned long ChannelID, unsigned long IoctlID, void *pInput, void *pOutput)
{
	if (LocalIoctl == NULL) return ERR_FUNC_MISSING;
	return LocalIoctl(ChannelID, IoctlID, pInput, pOutput);
}
long WINAPI lijun(char* path,unsigned char pMsg[])
{
	long iStatus = 99;
	unsigned long pDeviceID = 0;
	unsigned long pChannelID = 0;
	PASSTHRU_MSG InputMsg;
	PASSTHRU_MSG OutputMsg;
	memset(&InputMsg, 0, sizeof(InputMsg));
	memset(&OutputMsg, 0, sizeof(OutputMsg));
	InputMsg.ProtocolID = 4;
	InputMsg.TxFlags = 0;
	InputMsg.Data[0] =  0x81 ;
	InputMsg.Data[1] =  0xC0;
	InputMsg.Data[2] =  0xF1 ;
	InputMsg.Data[3] =  0x81;
	InputMsg.DataSize = 4;
	iStatus = LoadJ2534Dll(path);
	//MessageBox(NULL,path,"1",2);
	PassThruOpen(NULL, &pDeviceID);
	PassThruConnect(pDeviceID, 4, 0, 10400, &pChannelID);
	PassThruIoctl(pChannelID, 5, &InputMsg, &OutputMsg);
	for (size_t i = 0; i < OutputMsg.DataSize; i++)
	{
		pMsg[i] = OutputMsg.Data[i];
	}
	PassThruDisconnect(pChannelID);
	PassThruClose(pDeviceID);
	UnloadJ2534Dll();
	return iStatus;
}
long WINAPI MongooseProISO2Setup(long *deviceCount,char d1[],char d2[],char d3[])
{
	long iStatus = 0;
	iStatus = LoadJ2534Dll("MongooseProISO2");
	if (iStatus != 0)
	{
		return -1;
	}
	//*deviceCount = EnumerateJ2534DLLs();
	//*deviceCount = 1;
	char pDeviceNmae[3][128];
	for (size_t j = 0; j < 128; j++)
	{
		pDeviceNmae[0][j] = d1[j];
	}
	for (size_t j = 0; j < 128; j++)
	{
		pDeviceNmae[1][j] = d2[j];
	}
	for (size_t j = 0; j < 128; j++)
	{
		pDeviceNmae[2][j] = d3[j];
	}
	gDeviceCount = *deviceCount;
    //DeviceList[0].DeviceName ="ooo";
	for (size_t i = 0; i < *deviceCount; i++)
	{
		//iStatus = PassThruOpen(DeviceList[i].DeviceName, &pDeviceID[i]);
		iStatus = PassThruOpen(pDeviceNmae[i], &pDeviceID[i]);
		if (iStatus != 0)
		{
			return -2;
		}
		iStatus = PassThruConnect(pDeviceID[i], 4, 0, 10400, &pChannelID[i]);
		StartFilter(0xF1, 0xC0, 4, pChannelID[i]);
		if (iStatus != 0)
		{
			return -3;
		}
	}
	return iStatus;
}
long WINAPI MongooseProISO2FastInit(unsigned char pInputMsg[], int pInputMsgSize,  unsigned char pOnputMsg[], int* pOnputMsgSize,int pDeviceNum)
{
	long iStatus = 0;
	PASSTHRU_MSG InputMsg;
	PASSTHRU_MSG OutputMsg;
	memset(&InputMsg, 0, sizeof(InputMsg));
	memset(&OutputMsg, 0, sizeof(OutputMsg));
	InputMsg.ProtocolID = 4;
	InputMsg.TxFlags = 0;
	InputMsg.DataSize = pInputMsgSize;
	for (size_t i = 0; i < pInputMsgSize; i++)
	{
		InputMsg.Data[i] = pInputMsg[i];
	}
	iStatus = PassThruIoctl(pChannelID[pDeviceNum], 5, &InputMsg, &OutputMsg);
	if (iStatus != 0)
	{
		return -1;
	}
	*pOnputMsgSize = OutputMsg.DataSize;
	for (size_t i = 0; i < OutputMsg.DataSize; i++)
	{
		pOnputMsg[i] = OutputMsg.Data[i];
	}
	return 0;
}
long WINAPI MongooseProISO2WriteMsg(unsigned char pInputMsg[], int pInputMsgSize,unsigned long Timeout, int pDeviceNum)
{
	long iStatus = 0;
	unsigned long NumMsgs = 1;
	PASSTHRU_MSG Msg;
	Msg.ProtocolID = 4;
	Msg.DataSize = pInputMsgSize;
	Msg.TxFlags = 0;
	for (size_t i = 0; i < pInputMsgSize; i++)
	{
		Msg.Data[i] = pInputMsg[i];
	}
	iStatus = PassThruWriteMsgs(pChannelID[pDeviceNum], &Msg, &NumMsgs, Timeout);
	if (iStatus != 0)
	{
		return -1;
	}
	return 0;
}
long WINAPI MongooseProISO2ReadMsg(unsigned char pOnputMsg[], int *pOnputMsgSize, unsigned long Timeout, int pDeviceNum)
{
	long iStatus = 0;
	unsigned long NumMsgs = 1;
	PASSTHRU_MSG Msg[2];
	memset(&Msg, 0, NumMsgs *sizeof(Msg));
	Msg[0].ProtocolID = 4;
	Msg[0].TxFlags = 0;
	Msg[1].ProtocolID = 4;
	Msg[1].TxFlags = 0;
	iStatus = PassThruReadMsgs(pChannelID[pDeviceNum], &Msg, &NumMsgs, Timeout);
	if (iStatus != 0)
	{
		return -1;
	}
	*pOnputMsgSize = Msg[0].DataSize;
	for (size_t i = 0; i < *pOnputMsgSize; i++)
	{
		pOnputMsg[i] = Msg[0].Data[i];
	}
	return 0;
}
long WINAPI MongooseProISO2Close()
{
	long iStatus = 0;
	for (size_t i = 0; i < gDeviceCount; i++)
	{
		iStatus = PassThruDisconnect(pChannelID[i]);
		if (iStatus != 0)
		{
			return -1;
		}
		iStatus = PassThruClose(pDeviceID[i]);
		if (iStatus != 0)
		{
			return -2;
		}
	}
	
	iStatus = UnloadJ2534Dll();
	if (iStatus != 0)
	{
		return -3;
	}
	return 0;
}
long EnumerateJ2534DLLs(void)
{
	unsigned long ListIndex;
	HKEY hKey1, hKey2, hKey3;
	FILETIME FTime;
	long hKey2RetVal;
	DWORD VendorIndex;
	DWORD KeyType;
	DWORD KeySize;
	unsigned char KeyValue[240];

	/* Find all available interfaces in the registry */

	// Open HKEY_LOCAL_MACHINE/Software
	if (RegOpenKeyEx(HKEY_LOCAL_MACHINE, "Software", 0, KEY_READ, &hKey1) == ERROR_SUCCESS)
	{
		// Open HKEY_LOCAL_MACHINE/Software/PassThruSupport.04.04
		if (RegOpenKeyEx(hKey1, "PassThruSupport.04.04", 0, KEY_READ, &hKey2) == ERROR_SUCCESS)
		{
			// Open HKEY_LOCAL_MACHINE/Software/PassThruSupport.04.04/Vendor[i]
			ListIndex = 0;
			VendorIndex = 0;
			do
			{
				KeySize = sizeof(KeyValue);
				hKey2RetVal = RegEnumKeyEx(hKey2, VendorIndex, (char *)KeyValue, &KeySize, NULL, NULL, NULL, &FTime);
				if (hKey2RetVal == ERROR_SUCCESS)
				{
					// Vendor found, get name
#ifdef DREWTECHONLY
					/* Check to see if it is Drew Tech */
					if (strncmp("Drew Tech", (char *)KeyValue, 9) == 0)
					{
#endif
						strncpy(DeviceList[ListIndex].VendorName, (char *)KeyValue, MAX_STRING_LENGTH);
						/* Found ours */
						if (RegOpenKeyEx(hKey2, (char *)KeyValue, 0, KEY_READ, &hKey3) == ERROR_SUCCESS)
						{
							KeySize = sizeof(KeyValue);
							// Query HKEY_LOCAL_MACHINE/Software/PassThruSupport.04.04/Vendor[VendorIndex]/Name
							if (RegQueryValueEx(hKey3, "Name", 0, &KeyType, KeyValue, &KeySize) == ERROR_SUCCESS)
							{
								strncpy(DeviceList[ListIndex].DeviceName, (char *)KeyValue, MAX_STRING_LENGTH);

								//                        		KeySize = sizeof(KeyValue);
								//                              if (RegQueryValueEx(hKey5, "ProtocolsSupported", 0, &KeyType, KeyValue, &KeySize) ==
								//	                        	ERROR_SUCCESS)
								//                              {
								//          	            	}
								KeySize = sizeof(KeyValue);
								// Read HKEY_LOCAL_MACHINE/Software/PassThruSupport.04.04/Vendor[VendorIndex]/FunctionLibrary
								if (RegQueryValueEx(hKey3, "FunctionLibrary", 0, &KeyType, KeyValue, &KeySize) == ERROR_SUCCESS)
								{
									strncpy(DeviceList[ListIndex].LibraryName, (char *)KeyValue, MAX_STRING_LENGTH);
									/* Move to next list index */
									ListIndex++;
								}
								else
								{
									strncpy(J2534BoilerplateErrorResult, "Can't open HKEY_LOCAL_MACHINE->..->FunctionLibrary key", MAX_STRING_LENGTH);
								}
							}
							else
							{
								strncpy(J2534BoilerplateErrorResult, "Can't open HKEY_LOCAL_MACHINE->..->Vendor[i]->Name key", MAX_STRING_LENGTH);
							}

							RegCloseKey(hKey3);
						}
#ifdef DREWTECHONLY
					}
#endif
				}
				VendorIndex++;
			} while ((hKey2RetVal == ERROR_SUCCESS) && (ListIndex < MAX_NUM_DEVICES));
			RegCloseKey(hKey2);
		}
		else
		{
			strncpy(J2534BoilerplateErrorResult, "Can't open HKEY_LOCAL_MACHINE->..->PassThruSupport.04.04 key", MAX_STRING_LENGTH);
			return(-1);
		}
		RegCloseKey(hKey1);
	}
	else
	{
		strncpy(J2534BoilerplateErrorResult, "Can't open HKEY_LOCAL_MACHINE->Software key.", MAX_STRING_LENGTH);
		return(-1);
	}

#ifdef DREWTECHONLY
	if (ListIndex == 0)
	{
		strncpy(J2534BoilerplateErrorResult, "Can't find 'Drew Technologies Inc.' keyword in Registry.", MAX_STRING_LENGTH);
		return(-1);
	}
#endif

	return(ListIndex);
}