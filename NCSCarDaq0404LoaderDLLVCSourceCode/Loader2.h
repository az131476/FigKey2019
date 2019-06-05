// Loader2.h
// (c) 2005 National Control Systems, Inc.
// Portions (c) 2004 Drew Technologies, Inc.

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

typedef long (CALLBACK* PTCONNECT)(unsigned long, unsigned long, unsigned long *);
typedef long (CALLBACK* PTDISCONNECT)(unsigned long);
typedef long (CALLBACK* PTREADMSGS)(unsigned long, void *, unsigned long *, unsigned long);
typedef long (CALLBACK* PTWRITEMSGS)(unsigned long, void *, unsigned long *, unsigned long);
typedef long (CALLBACK* PTSTARTPERIODICMSG)(unsigned long, void *, unsigned long *, unsigned long);
typedef long (CALLBACK* PTSTOPPERIODICMSG)(unsigned long, unsigned long);
typedef long (CALLBACK* PTSTARTMSGFILTER)(unsigned long, unsigned long, void *, void *, void *, unsigned long *);
typedef long (CALLBACK* PTSTOPMSGFILTER)(unsigned long, unsigned long);
typedef long (CALLBACK* PTSETPROGRAMMINGVOLTAGE)(unsigned long, unsigned long);
typedef long (CALLBACK* PTREADVERSION)(char *, char *, char *);
typedef long (CALLBACK* PTGETLASTERROR)(char *);
typedef long (CALLBACK* PTIOCTL)(unsigned long, unsigned long, void *, void *);

PTCONNECT LocalConnect;
PTDISCONNECT LocalDisconnect;
PTREADMSGS LocalReadMsgs;
PTWRITEMSGS LocalWriteMsgs;
PTSTARTPERIODICMSG LocalStartPeriodicMsg;
PTSTOPPERIODICMSG LocalStopPeriodicMsg;
PTSTARTMSGFILTER LocalStartMsgFilter;
PTSTOPMSGFILTER LocalStopMsgFilter;
PTSETPROGRAMMINGVOLTAGE LocalSetProgrammingVoltage;
PTREADVERSION LocalReadVersion;
PTGETLASTERROR LocalGetLastError;
PTIOCTL LocalIoctl;

// J2534 Functions
WINAPI PassThruConnect(unsigned long, unsigned long, unsigned long*);
WINAPI PassThruDisconnect(unsigned long);
WINAPI PassThruReadMsgs(unsigned long, void *, unsigned long *, unsigned long);
WINAPI PassThruWriteMsgs(unsigned long, void *, unsigned long *, unsigned long);
WINAPI PassThruStartPeriodicMsg(unsigned long, void *, unsigned long *, unsigned long);
WINAPI PassThruStopPeriodicMsg(unsigned long, unsigned long);
WINAPI PassThruStartMsgFilter(unsigned long, unsigned long, void *, void *, void *, unsigned long *);
WINAPI PassThruStopMsgFilter(unsigned long, unsigned long);
WINAPI PassThruSetProgrammingVoltage(unsigned long, unsigned long);
WINAPI PassThruReadVersion(char *, char *, char *);
WINAPI PassThruGetLastError(char *);
WINAPI PassThruIoctl(unsigned long, unsigned long, void *, void *);

//Other Functions
WINAPI LoadJ2534Dll(char *);
WINAPI UnloadJ2534Dll();

// NCS Returns of any functions not found
//#define ERR_NO_PTOPEN					0x0001
//#define ERR_NO_PTCLOSE				0x0002
#define ERR_NO_PTCONNECT				0x0004
#define ERR_NO_PTDISCONNECT				0x0008
#define ERR_NO_PTREADMSGS				0x0010
#define ERR_NO_PTWRITEMSGS				0x0020
#define ERR_NO_PTSTARTPERIODICMSG		0x0040
#define ERR_NO_PTSTOPPERIODICMSG		0x0080
#define ERR_NO_PTSTARTMSGFILTER			0x0100
#define ERR_NO_PTSTOPMSGFILTER			0x0200
#define ERR_NO_PTSETPROGRAMMINGVOLTAGE	0x0400
#define ERR_NO_PTREADVERSION			0x0800
#define ERR_NO_PTGETLASTERROR			0x1000
#define ERR_NO_PTIOCTL					0x2000
#define ERR_NO_DLL						-1
#define ERR_WRONG_DLL_VER				-2
#define ERR_FUNC_MISSING				-3

//Spec:
/*************/
/* Error IDs */
/*************/
#define STATUS_NOERROR						0x00
#define ERR_NOT_SUPPORTED					0x01
#define ERR_INVALID_CHANNEL_ID				0x02
#define ERR_INVALID_PROTOCOL_ID				0x03
#define ERR_NULL_PARAMETER		/*DT*/		0x04 //DT name
#define ERR_NULLPARAMETER					0x04 // brain-dead spec name
#define ERR_INVALID_IOCTL_VALUE				0x05
#define ERR_INVALID_FLAGS					0x06
#define ERR_FAILED							0x07
#define ERR_DEVICE_NOT_CONNECTED			0x08
#define ERR_TIMEOUT							0x09
#define ERR_INVALID_MSG						0x0A
#define ERR_INVALID_TIME_INTERVAL			0x0B
#define ERR_EXCEEDED_LIMIT					0x0C
#define ERR_INVALID_MSG_ID					0x0D
#define ERR_INVALID_ERROR_ID				0x0E /*?*/
#define ERR_INVALID_IOCTL_ID				0x0F
#define ERR_BUFFER_EMPTY					0x10
#define ERR_BUFFER_FULL						0x11
#define ERR_BUFFER_OVERFLOW					0x12
#define ERR_PIN_INVALID						0x13
#define ERR_CHANNEL_IN_USE					0x14
#define ERR_MSG_PROTOCOL_ID					0x15

