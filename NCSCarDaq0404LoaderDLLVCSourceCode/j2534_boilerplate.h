/*****************************************************************************
 * THIS PROGRAM IS PROVIDED 'AS IS' WITHOUT WARRANTY OF ANY KIND, EITHER     *
 * EXPRESSED OR IMPLIED, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED          *
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE.       *
 * IN NO EVENT SHALL DREWTECH BE LIABLE FOR DAMAGES, INCLUDING ANY GENERAL,  *
 * SPECIAL, INCIDENTAL OR CONSEQUENTIAL DAMAGES ARISING OUT OF THE USE OR    *
 * INABILITY TO USE THE PROGRAM (INCLUDING BUT NOT LIMITED TO LOSS OF DATA   *
 * OR DATA BEING RENDERED INACCURATE OR LOSSES SUSTAINED BY YOU OR THIRD     *
 * PARTIES OR A FAILURE OF THE PROGRAM TO OPERATE WITH ANY OTHER PROGRAMS),  *
 * EVEN IF SUCH HOLDER OR OTHER PARTY HAS BEEN ADVISED OF THE POSSIBILITY    *
 * OF SUCH DAMAGES.                                                          *
 *                                                                           *
 * This file is free to use as long as this header stays intact.             *
 * Author: Mark Wine                                                         *
 ****************************************************************************/

/* Lists used to find the DLLs */
#ifndef __J2534_BOILERPLATE_H
#define __J2534_BOILERPLATE_H

#define MAX_NUM_DEVICES     20
#define MAX_STRING_LENGTH   128
typedef struct
{
    char VendorName[MAX_STRING_LENGTH];
    char DeviceName[MAX_STRING_LENGTH];
    char LibraryName[MAX_STRING_LENGTH];
} J2534DEVICELIST;


long EnumerateJ2534DLLs(void);

#endif
