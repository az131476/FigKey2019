// Loader2.cpp
// (c) 2005 National Control Systems, Inc.
// Portions (c) 2004 Drew Technologies, Inc.
// Dynamic J2534 v02.02 dll loader for VB

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
#include <windows.h>
#include "Loader2.h"


HINSTANCE hDLL = NULL;
BOOL bIsCorrectVersion = FALSE;

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


WINAPI LoadJ2534Dll(char *sLib)
{
	long lFuncList = 0, lRet = 0;
	char sReqVer[] = "02.02";
	char sFW[80], sDll[80], sAPI[80];

	if (hDLL != NULL) UnloadJ2534Dll();
	//{
	//	FreeLibrary(hDLL);
	//	hDLL = NULL;
	//}
	hDLL = LoadLibrary (sLib); 
	if (hDLL == NULL) return ERR_NO_DLL;
	
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

	if (LocalReadVersion != NULL)
	{
		lRet = LocalReadVersion(sFW, sDll, sAPI);
		if (lRet != STATUS_NOERROR) return lRet;
		if (strcmp(sAPI,sReqVer)) return ERR_WRONG_DLL_VER;
		bIsCorrectVersion = TRUE;
	}

	return lFuncList;
}

WINAPI UnloadJ2534Dll()
{
	if (FreeLibrary(hDLL))
	{
		hDLL = NULL;
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

WINAPI PassThruConnect(unsigned long ProtocolID, unsigned long Flags, unsigned long *pChannelID)
{
	if (LocalConnect == NULL) return ERR_FUNC_MISSING;
	if (!bIsCorrectVersion) return ERR_WRONG_DLL_VER;
	return LocalConnect(ProtocolID, Flags, pChannelID);
}

WINAPI PassThruDisconnect(unsigned long ChannelID)
{
	if (LocalDisconnect == NULL) return ERR_FUNC_MISSING;
	if (!bIsCorrectVersion) return ERR_WRONG_DLL_VER;
	return LocalDisconnect(ChannelID);
}

WINAPI PassThruReadMsgs(unsigned long ChannelID, void *pMsg, unsigned long *pNumMsgs, unsigned long Timeout)
{
	if (LocalReadMsgs == NULL) return ERR_FUNC_MISSING;
	if (!bIsCorrectVersion) return ERR_WRONG_DLL_VER;
	return LocalReadMsgs(ChannelID, pMsg, pNumMsgs, Timeout);
}
						
WINAPI PassThruWriteMsgs(unsigned long ChannelID, void *pMsg, unsigned long *pNumMsgs, unsigned long Timeout)
{
	if (LocalWriteMsgs == NULL) return ERR_FUNC_MISSING;
	if (!bIsCorrectVersion) return ERR_WRONG_DLL_VER;
	return LocalWriteMsgs(ChannelID, pMsg, pNumMsgs, Timeout);
}

WINAPI PassThruStartPeriodicMsg(unsigned long ChannelID, void *pMsg, unsigned long *pMsgID, unsigned long TimeInterval)
{
	if (LocalStartPeriodicMsg == NULL) return ERR_FUNC_MISSING;
	if (!bIsCorrectVersion) return ERR_WRONG_DLL_VER;
	return LocalStartPeriodicMsg(ChannelID, pMsg, pMsgID, TimeInterval);
}

WINAPI PassThruStopPeriodicMsg(unsigned long ChannelID, unsigned long MsgID)
{
	if (LocalStopPeriodicMsg == NULL) return ERR_FUNC_MISSING;
	if (!bIsCorrectVersion) return ERR_WRONG_DLL_VER;
	return LocalStopPeriodicMsg(ChannelID, MsgID);
}

WINAPI PassThruStartMsgFilter(unsigned long ChannelID, unsigned long FilterType, void *pMaskMsg, void *pPatternMsg, void *pFlowControlMsg, unsigned long *pMsgID)
{
	if (LocalStartMsgFilter == NULL) return ERR_FUNC_MISSING;
	if (!bIsCorrectVersion) return ERR_WRONG_DLL_VER;
	return LocalStartMsgFilter(ChannelID, FilterType, pMaskMsg, pPatternMsg, pFlowControlMsg, pMsgID);
}

WINAPI PassThruStopMsgFilter(unsigned long ChannelID, unsigned long MsgID)
{
	if (LocalStopMsgFilter == NULL) return ERR_FUNC_MISSING;
	if (!bIsCorrectVersion) return ERR_WRONG_DLL_VER;
	return LocalStopMsgFilter(ChannelID, MsgID);
}

WINAPI PassThruSetProgrammingVoltage(unsigned long Pin, unsigned long Voltage)
{
	if (LocalSetProgrammingVoltage == NULL) return ERR_FUNC_MISSING;
	if (!bIsCorrectVersion) return ERR_WRONG_DLL_VER;
	return LocalSetProgrammingVoltage(Pin, Voltage);
}

WINAPI PassThruReadVersion(char *pFirmwareVersion, char *pDllVersion, char *pApiVersion)
{
	if (LocalReadVersion == NULL) return ERR_FUNC_MISSING;
	if (!bIsCorrectVersion) return ERR_WRONG_DLL_VER;
	return LocalReadVersion(pFirmwareVersion, pDllVersion, pApiVersion);
}

WINAPI PassThruGetLastError(char *pErrorDescription)
{
	if (LocalGetLastError == NULL) return ERR_FUNC_MISSING;
	if (!bIsCorrectVersion) return ERR_WRONG_DLL_VER;
	return LocalGetLastError(pErrorDescription);
}

WINAPI PassThruIoctl(unsigned long ChannelID, unsigned long IoctlID, void *pInput, void *pOutput)
{
	if (LocalIoctl == NULL) return ERR_FUNC_MISSING;
	if (!bIsCorrectVersion) return ERR_WRONG_DLL_VER;
	return LocalIoctl(ChannelID, IoctlID, pInput, pOutput);
}