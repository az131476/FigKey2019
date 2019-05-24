#ifndef _DATA_H
#define _DATA_H

static const MdfDataRecordType XcpTestTab[]=
{
	"ActMod_trqClth", "rat_func",	"Nm"			, 1, 2, 0, 0x52800614, 0		,  0.1	 		,
	"gangi"					, "rat_func",	" "				, 0, 1, 0, 0x52802CDA, 0		,  1				,
	"nmot"					, "rat_func", "1/min"		, 0, 1, 0, 0x52802A10, 0		,  40 			,
	"psr_w"					,	"rat_func", "hPa"			, 0, 2, 0, 0x528013E4, 0		,  0.078125	,
	"tans"					, "rat_func", "Grad C"	, 0, 1, 0, 0x52802B5B, -48	,  0.75			,
	"tmot"					, "rat_func", "Grad C"	, 0, 1, 0, 0x52802755, -48	,  0.75			,
	"vfzg"					, "rat_func", "km/h"		, 0, 1, 0, 0x52802ACB, 0		,  1.25			,
	"wdkba"					, "rat_func", "%DK"			, 0, 1, 0, 0x508068AF, 0		,  0.392157	,
	"wped"   				, "rat_func", "%PED"		, 0, 1, 0, 0x5280247C, 0		,  0.392157	,
	"zwout"					, "rat_func", "Grad KW"	, 1, 1, 0, 0x52802CE5, 0		,  0.75			,
};
static const uint16_t XcpTestLenNum = (uint16_t)sizeof(XcpTestTab)/sizeof(MdfDataRecordType);	

#endif
