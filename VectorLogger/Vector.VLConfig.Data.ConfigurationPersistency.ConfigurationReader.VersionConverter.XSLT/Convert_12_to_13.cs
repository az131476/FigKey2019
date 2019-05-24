using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Xml.XPath;
using System.Xml.Xsl.Runtime;

namespace Vector.VLConfig.Data.ConfigurationPersistency.ConfigurationReader.VersionConverter.XSLT
{
	[GeneratedCode("System.Xml.Xsl.XslCompiledTransform", "2.0.0.0"), ComVisible(true)]
	public static class Convert_12_to_13
	{
		private static $ArrayType$2512 __staticData;

		private static object staticData = new byte[]
		{
			0,
			233,
			253,
			0,
			0,
			0,
			0,
			1,
			2,
			13,
			10,
			255,
			1,
			2,
			32,
			32,
			0,
			0,
			0,
			1,
			0,
			0,
			0,
			0,
			0,
			1,
			1,
			8,
			116,
			101,
			120,
			116,
			47,
			120,
			109,
			108,
			0,
			0,
			0,
			1,
			1,
			0,
			0,
			0,
			0,
			83,
			0,
			0,
			0,
			32,
			67,
			111,
			110,
			102,
			105,
			103,
			117,
			114,
			97,
			116,
			105,
			111,
			110,
			68,
			97,
			116,
			97,
			77,
			111,
			100,
			101,
			108,
			76,
			111,
			103,
			103,
			101,
			114,
			84,
			121,
			112,
			101,
			83,
			104,
			116,
			116,
			112,
			58,
			47,
			47,
			115,
			99,
			104,
			101,
			109,
			97,
			115,
			46,
			100,
			97,
			116,
			97,
			99,
			111,
			110,
			116,
			114,
			97,
			99,
			116,
			46,
			111,
			114,
			103,
			47,
			50,
			48,
			48,
			52,
			47,
			48,
			55,
			47,
			86,
			101,
			99,
			116,
			111,
			114,
			46,
			86,
			76,
			67,
			111,
			110,
			102,
			105,
			103,
			46,
			68,
			97,
			116,
			97,
			46,
			67,
			111,
			110,
			102,
			105,
			103,
			117,
			114,
			97,
			116,
			105,
			111,
			110,
			68,
			97,
			116,
			97,
			77,
			111,
			100,
			101,
			108,
			37,
			76,
			111,
			103,
			68,
			97,
			116,
			97,
			83,
			116,
			111,
			114,
			97,
			103,
			101,
			73,
			115,
			69,
			110,
			116,
			101,
			114,
			83,
			108,
			101,
			101,
			112,
			77,
			111,
			100,
			101,
			69,
			110,
			97,
			98,
			108,
			101,
			100,
			33,
			76,
			111,
			103,
			68,
			97,
			116,
			97,
			83,
			116,
			111,
			114,
			97,
			103,
			101,
			73,
			115,
			70,
			97,
			115,
			116,
			87,
			97,
			107,
			101,
			85,
			112,
			69,
			110,
			97,
			98,
			108,
			101,
			100,
			37,
			76,
			111,
			103,
			68,
			97,
			116,
			97,
			83,
			116,
			111,
			114,
			97,
			103,
			101,
			82,
			105,
			110,
			103,
			66,
			117,
			102,
			102,
			101,
			114,
			79,
			112,
			101,
			114,
			97,
			116,
			105,
			110,
			103,
			77,
			111,
			100,
			101,
			58,
			68,
			105,
			97,
			103,
			110,
			111,
			115,
			116,
			105,
			99,
			65,
			99,
			116,
			105,
			111,
			110,
			115,
			67,
			111,
			110,
			102,
			105,
			103,
			117,
			114,
			97,
			116,
			105,
			111,
			110,
			83,
			116,
			111,
			112,
			67,
			121,
			99,
			108,
			105,
			99,
			67,
			111,
			109,
			109,
			117,
			110,
			105,
			99,
			97,
			116,
			105,
			111,
			110,
			69,
			118,
			101,
			110,
			116,
			4,
			116,
			121,
			112,
			101,
			41,
			104,
			116,
			116,
			112,
			58,
			47,
			47,
			119,
			119,
			119,
			46,
			119,
			51,
			46,
			111,
			114,
			103,
			47,
			50,
			48,
			48,
			49,
			47,
			88,
			77,
			76,
			83,
			99,
			104,
			101,
			109,
			97,
			45,
			105,
			110,
			115,
			116,
			97,
			110,
			99,
			101,
			28,
			68,
			105,
			103,
			105,
			116,
			97,
			108,
			73,
			110,
			112,
			117,
			116,
			69,
			118,
			101,
			110,
			116,
			68,
			105,
			103,
			105,
			116,
			97,
			73,
			110,
			112,
			117,
			116,
			21,
			68,
			105,
			103,
			105,
			116,
			97,
			108,
			73,
			110,
			112,
			117,
			116,
			69,
			118,
			101,
			110,
			116,
			69,
			100,
			103,
			101,
			26,
			83,
			121,
			109,
			98,
			111,
			108,
			105,
			99,
			83,
			105,
			103,
			110,
			97,
			108,
			69,
			118,
			101,
			110,
			116,
			66,
			117,
			115,
			84,
			121,
			112,
			101,
			32,
			83,
			121,
			109,
			98,
			111,
			108,
			105,
			99,
			83,
			105,
			103,
			110,
			97,
			108,
			69,
			118,
			101,
			110,
			116,
			67,
			104,
			97,
			110,
			110,
			101,
			108,
			78,
			117,
			109,
			98,
			101,
			114,
			31,
			83,
			121,
			109,
			98,
			111,
			108,
			105,
			99,
			83,
			105,
			103,
			110,
			97,
			108,
			69,
			118,
			101,
			110,
			116,
			68,
			97,
			116,
			97,
			98,
			97,
			115,
			101,
			78,
			97,
			109,
			101,
			31,
			83,
			121,
			109,
			98,
			111,
			108,
			105,
			99,
			83,
			105,
			103,
			110,
			97,
			108,
			69,
			118,
			101,
			110,
			116,
			68,
			97,
			116,
			97,
			98,
			97,
			115,
			101,
			80,
			97,
			116,
			104,
			28,
			83,
			121,
			109,
			98,
			111,
			108,
			105,
			99,
			83,
			105,
			103,
			110,
			97,
			108,
			69,
			118,
			101,
			110,
			116,
			72,
			105,
			103,
			104,
			86,
			97,
			108,
			117,
			101,
			31,
			83,
			121,
			109,
			98,
			111,
			108,
			105,
			99,
			83,
			105,
			103,
			110,
			97,
			108,
			69,
			118,
			101,
			110,
			116,
			73,
			115,
			70,
			108,
			101,
			120,
			114,
			97,
			121,
			80,
			68,
			85,
			27,
			83,
			121,
			109,
			98,
			111,
			108,
			105,
			99,
			83,
			105,
			103,
			110,
			97,
			108,
			69,
			118,
			101,
			110,
			116,
			76,
			111,
			119,
			86,
			97,
			108,
			117,
			101,
			30,
			83,
			121,
			109,
			98,
			111,
			108,
			105,
			99,
			83,
			105,
			103,
			110,
			97,
			108,
			69,
			118,
			101,
			110,
			116,
			77,
			101,
			115,
			115,
			97,
			103,
			101,
			78,
			97,
			109,
			101,
			5,
			86,
			97,
			108,
			117,
			101,
			30,
			83,
			121,
			109,
			98,
			111,
			108,
			105,
			99,
			83,
			105,
			103,
			110,
			97,
			108,
			69,
			118,
			101,
			110,
			116,
			78,
			101,
			116,
			119,
			111,
			114,
			107,
			78,
			97,
			109,
			101,
			3,
			110,
			105,
			108,
			27,
			83,
			121,
			109,
			98,
			111,
			108,
			105,
			99,
			83,
			105,
			103,
			110,
			97,
			108,
			69,
			118,
			101,
			110,
			116,
			82,
			101,
			108,
			97,
			116,
			105,
			111,
			110,
			29,
			83,
			121,
			109,
			98,
			111,
			108,
			105,
			99,
			83,
			105,
			103,
			110,
			97,
			108,
			69,
			118,
			101,
			110,
			116,
			83,
			105,
			103,
			110,
			97,
			108,
			78,
			97,
			109,
			101,
			28,
			76,
			111,
			103,
			68,
			97,
			116,
			97,
			83,
			116,
			111,
			114,
			97,
			103,
			101,
			84,
			105,
			109,
			101,
			111,
			117,
			116,
			84,
			111,
			83,
			108,
			101,
			101,
			112,
			32,
			76,
			111,
			103,
			68,
			97,
			116,
			97,
			83,
			116,
			111,
			114,
			97,
			103,
			101,
			85,
			115,
			101,
			68,
			97,
			116,
			97,
			67,
			111,
			109,
			112,
			114,
			101,
			115,
			115,
			105,
			111,
			110,
			49,
			68,
			105,
			97,
			103,
			110,
			111,
			115,
			116,
			105,
			99,
			65,
			99,
			116,
			105,
			111,
			110,
			115,
			67,
			111,
			110,
			102,
			105,
			103,
			117,
			114,
			97,
			116,
			105,
			111,
			110,
			68,
			105,
			97,
			103,
			67,
			111,
			109,
			109,
			82,
			101,
			115,
			116,
			97,
			114,
			116,
			84,
			105,
			109,
			101,
			54,
			68,
			105,
			97,
			103,
			110,
			111,
			115,
			116,
			105,
			99,
			65,
			99,
			116,
			105,
			111,
			110,
			115,
			67,
			111,
			110,
			102,
			105,
			103,
			117,
			114,
			97,
			116,
			105,
			111,
			110,
			73,
			115,
			68,
			105,
			97,
			103,
			67,
			111,
			109,
			109,
			82,
			101,
			115,
			116,
			97,
			114,
			116,
			69,
			110,
			97,
			98,
			108,
			101,
			100,
			52,
			68,
			105,
			97,
			103,
			110,
			111,
			115,
			116,
			105,
			99,
			65,
			99,
			116,
			105,
			111,
			110,
			115,
			67,
			111,
			110,
			102,
			105,
			103,
			117,
			114,
			97,
			116,
			105,
			111,
			110,
			82,
			101,
			99,
			111,
			114,
			100,
			68,
			105,
			97,
			103,
			67,
			111,
			109,
			109,
			77,
			101,
			109,
			111,
			114,
			105,
			101,
			115,
			48,
			68,
			105,
			97,
			103,
			110,
			111,
			115,
			116,
			105,
			99,
			65,
			99,
			116,
			105,
			111,
			110,
			115,
			67,
			111,
			110,
			102,
			105,
			103,
			117,
			114,
			97,
			116,
			105,
			111,
			110,
			84,
			114,
			105,
			103,
			103,
			101,
			114,
			101,
			100,
			83,
			101,
			113,
			117,
			101,
			110,
			99,
			101,
			115,
			33,
			84,
			114,
			105,
			103,
			103,
			101,
			114,
			101,
			100,
			68,
			105,
			97,
			103,
			110,
			111,
			115,
			116,
			105,
			99,
			65,
			99,
			116,
			105,
			111,
			110,
			83,
			101,
			113,
			117,
			101,
			110,
			99,
			101,
			40,
			84,
			114,
			105,
			103,
			103,
			101,
			114,
			101,
			100,
			68,
			105,
			97,
			103,
			110,
			111,
			115,
			116,
			105,
			99,
			65,
			99,
			116,
			105,
			111,
			110,
			83,
			101,
			113,
			117,
			101,
			110,
			99,
			101,
			65,
			99,
			116,
			105,
			111,
			110,
			115,
			38,
			84,
			114,
			105,
			103,
			103,
			101,
			114,
			101,
			100,
			68,
			105,
			97,
			103,
			110,
			111,
			115,
			116,
			105,
			99,
			65,
			99,
			116,
			105,
			111,
			110,
			83,
			101,
			113,
			117,
			101,
			110,
			99,
			101,
			69,
			118,
			101,
			110,
			116,
			22,
			67,
			111,
			110,
			102,
			105,
			103,
			117,
			114,
			97,
			116,
			105,
			111,
			110,
			68,
			97,
			116,
			97,
			77,
			111,
			100,
			101,
			108,
			43,
			67,
			111,
			110,
			102,
			105,
			103,
			117,
			114,
			97,
			116,
			105,
			111,
			110,
			68,
			97,
			116,
			97,
			77,
			111,
			100,
			101,
			108,
			72,
			97,
			114,
			100,
			119,
			97,
			114,
			101,
			67,
			111,
			110,
			102,
			105,
			103,
			117,
			114,
			97,
			116,
			105,
			111,
			110,
			35,
			72,
			97,
			114,
			100,
			119,
			97,
			114,
			101,
			67,
			111,
			110,
			102,
			105,
			103,
			117,
			114,
			97,
			116,
			105,
			111,
			110,
			76,
			111,
			103,
			68,
			97,
			116,
			97,
			83,
			116,
			111,
			114,
			97,
			103,
			101,
			34,
			76,
			111,
			103,
			68,
			97,
			116,
			97,
			83,
			116,
			111,
			114,
			97,
			103,
			101,
			77,
			101,
			109,
			111,
			114,
			121,
			82,
			105,
			110,
			103,
			66,
			117,
			102,
			102,
			101,
			114,
			76,
			105,
			115,
			116,
			16,
			77,
			101,
			109,
			111,
			114,
			121,
			82,
			105,
			110,
			103,
			66,
			117,
			102,
			102,
			101,
			114,
			35,
			84,
			114,
			105,
			103,
			103,
			101,
			114,
			67,
			111,
			110,
			102,
			105,
			103,
			117,
			114,
			97,
			116,
			105,
			111,
			110,
			80,
			111,
			115,
			116,
			84,
			114,
			105,
			103,
			103,
			101,
			114,
			84,
			105,
			109,
			101,
			31,
			84,
			114,
			105,
			103,
			103,
			101,
			114,
			67,
			111,
			110,
			102,
			105,
			103,
			117,
			114,
			97,
			116,
			105,
			111,
			110,
			84,
			114,
			105,
			103,
			103,
			101,
			114,
			76,
			105,
			115,
			116,
			13,
			82,
			101,
			99,
			111,
			114,
			100,
			84,
			114,
			105,
			103,
			103,
			101,
			114,
			19,
			82,
			101,
			99,
			111,
			114,
			100,
			84,
			114,
			105,
			103,
			103,
			101,
			114,
			77,
			101,
			109,
			111,
			114,
			121,
			36,
			84,
			114,
			105,
			103,
			103,
			101,
			114,
			67,
			111,
			110,
			102,
			105,
			103,
			117,
			114,
			97,
			116,
			105,
			111,
			110,
			84,
			114,
			105,
			103,
			103,
			101,
			114,
			76,
			105,
			115,
			116,
			79,
			110,
			79,
			102,
			102,
			31,
			84,
			114,
			105,
			103,
			103,
			101,
			114,
			67,
			111,
			110,
			102,
			105,
			103,
			117,
			114,
			97,
			116,
			105,
			111,
			110,
			84,
			114,
			105,
			103,
			103,
			101,
			114,
			77,
			111,
			100,
			101,
			20,
			77,
			101,
			109,
			111,
			114,
			121,
			82,
			105,
			110,
			103,
			66,
			117,
			102,
			102,
			101,
			114,
			83,
			105,
			122,
			101,
			24,
			77,
			101,
			109,
			111,
			114,
			121,
			82,
			105,
			110,
			103,
			66,
			117,
			102,
			102,
			101,
			114,
			83,
			116,
			111,
			114,
			101,
			82,
			65,
			77,
			19,
			82,
			101,
			99,
			111,
			114,
			100,
			84,
			114,
			105,
			103,
			103,
			101,
			114,
			65,
			99,
			116,
			105,
			111,
			110,
			20,
			82,
			101,
			99,
			111,
			114,
			100,
			84,
			114,
			105,
			103,
			103,
			101,
			114,
			67,
			111,
			109,
			109,
			101,
			110,
			116,
			19,
			82,
			101,
			99,
			111,
			114,
			100,
			84,
			114,
			105,
			103,
			103,
			101,
			114,
			69,
			102,
			102,
			101,
			99,
			116,
			18,
			82,
			101,
			99,
			111,
			114,
			100,
			84,
			114,
			105,
			103,
			103,
			101,
			114,
			69,
			118,
			101,
			110,
			116,
			21,
			82,
			101,
			99,
			111,
			114,
			100,
			84,
			114,
			105,
			103,
			103,
			101,
			114,
			73,
			115,
			65,
			99,
			116,
			105,
			118,
			101,
			29,
			70,
			105,
			108,
			116,
			101,
			114,
			67,
			111,
			110,
			102,
			105,
			103,
			117,
			114,
			97,
			116,
			105,
			111,
			110,
			70,
			105,
			108,
			116,
			101,
			114,
			76,
			105,
			115,
			116,
			6,
			70,
			105,
			108,
			116,
			101,
			114,
			12,
			70,
			105,
			108,
			116,
			101,
			114,
			77,
			101,
			109,
			111,
			114,
			121,
			24,
			77,
			101,
			109,
			111,
			114,
			121,
			82,
			105,
			110,
			103,
			66,
			117,
			102,
			102,
			101,
			114,
			73,
			115,
			65,
			99,
			116,
			105,
			118,
			101,
			18,
			70,
			105,
			108,
			116,
			101,
			114,
			65,
			99,
			116,
			105,
			111,
			110,
			73,
			115,
			83,
			116,
			111,
			112,
			14,
			70,
			105,
			108,
			116,
			101,
			114,
			73,
			115,
			65,
			99,
			116,
			105,
			118,
			101,
			15,
			70,
			105,
			108,
			116,
			101,
			114,
			66,
			97,
			115,
			101,
			67,
			121,
			99,
			108,
			101,
			19,
			70,
			105,
			108,
			116,
			101,
			114,
			67,
			104,
			97,
			110,
			110,
			101,
			108,
			78,
			117,
			109,
			98,
			101,
			114,
			21,
			70,
			105,
			108,
			116,
			101,
			114,
			67,
			121,
			99,
			108,
			101,
			82,
			101,
			112,
			101,
			116,
			105,
			116,
			105,
			111,
			110,
			15,
			70,
			105,
			108,
			116,
			101,
			114,
			70,
			108,
			101,
			120,
			114,
			97,
			121,
			73,
			100,
			19,
			70,
			105,
			108,
			116,
			101,
			114,
			70,
			108,
			101,
			120,
			114,
			97,
			121,
			73,
			100,
			76,
			97,
			115,
			116,
			15,
			70,
			105,
			108,
			116,
			101,
			114,
			73,
			115,
			73,
			100,
			82,
			97,
			110,
			103,
			101,
			11,
			70,
			105,
			108,
			116,
			101,
			114,
			76,
			73,
			78,
			73,
			100,
			15,
			70,
			105,
			108,
			116,
			101,
			114,
			76,
			73,
			78,
			73,
			100,
			76,
			97,
			115,
			116,
			11,
			70,
			105,
			108,
			116,
			101,
			114,
			67,
			65,
			78,
			73,
			100,
			15,
			70,
			105,
			108,
			116,
			101,
			114,
			67,
			65,
			78,
			73,
			100,
			76,
			97,
			115,
			116,
			18,
			70,
			105,
			108,
			116,
			101,
			114,
			73,
			115,
			69,
			120,
			116,
			101,
			110,
			100,
			101,
			100,
			73,
			100,
			13,
			70,
			105,
			108,
			116,
			101,
			114,
			66,
			117,
			115,
			84,
			121,
			112,
			101,
			18,
			70,
			105,
			108,
			116,
			101,
			114,
			68,
			97,
			116,
			97,
			98,
			97,
			115,
			101,
			78,
			97,
			109,
			101,
			18,
			70,
			105,
			108,
			116,
			101,
			114,
			68,
			97,
			116,
			97,
			98,
			97,
			115,
			101,
			80,
			97,
			116,
			104,
			25,
			70,
			105,
			108,
			116,
			101,
			114,
			77,
			101,
			115,
			115,
			97,
			103,
			101,
			73,
			115,
			70,
			108,
			101,
			120,
			114,
			97,
			121,
			80,
			68,
			85,
			17,
			70,
			105,
			108,
			116,
			101,
			114,
			77,
			101,
			115,
			115,
			97,
			103,
			101,
			78,
			97,
			109,
			101,
			17,
			70,
			105,
			108,
			116,
			101,
			114,
			78,
			101,
			116,
			119,
			111,
			114,
			107,
			78,
			97,
			109,
			101,
			20,
			67,
			104,
			97,
			110,
			110,
			101,
			108,
			70,
			105,
			108,
			116,
			101,
			114,
			66,
			117,
			115,
			84,
			121,
			112,
			101,
			26,
			67,
			104,
			97,
			110,
			110,
			101,
			108,
			70,
			105,
			108,
			116,
			101,
			114,
			67,
			104,
			97,
			110,
			110,
			101,
			108,
			78,
			117,
			109,
			98,
			101,
			114,
			37,
			72,
			97,
			114,
			100,
			119,
			97,
			114,
			101,
			67,
			111,
			110,
			102,
			105,
			103,
			117,
			114,
			97,
			116,
			105,
			111,
			110,
			76,
			69,
			68,
			67,
			111,
			110,
			102,
			105,
			103,
			117,
			114,
			97,
			116,
			105,
			111,
			110,
			48,
			72,
			97,
			114,
			100,
			119,
			97,
			114,
			101,
			67,
			111,
			110,
			102,
			105,
			103,
			117,
			114,
			97,
			116,
			105,
			111,
			110,
			70,
			108,
			101,
			120,
			114,
			97,
			121,
			67,
			104,
			97,
			110,
			110,
			101,
			108,
			67,
			111,
			110,
			102,
			105,
			103,
			117,
			114,
			97,
			116,
			105,
			111,
			110,
			42,
			67,
			111,
			110,
			102,
			105,
			103,
			117,
			114,
			97,
			116,
			105,
			111,
			110,
			68,
			97,
			116,
			97,
			77,
			111,
			100,
			101,
			108,
			76,
			111,
			103,
			103,
			105,
			110,
			103,
			67,
			111,
			110,
			102,
			105,
			103,
			117,
			114,
			97,
			116,
			105,
			111,
			110,
			43,
			68,
			105,
			97,
			103,
			110,
			111,
			115,
			116,
			105,
			99,
			67,
			111,
			109,
			109,
			80,
			97,
			114,
			97,
			109,
			115,
			69,
			67,
			85,
			68,
			101,
			102,
			97,
			117,
			108,
			116,
			83,
			101,
			115,
			115,
			105,
			111,
			110,
			83,
			111,
			117,
			114,
			99,
			101,
			39,
			76,
			111,
			103,
			103,
			105,
			110,
			103,
			67,
			111,
			110,
			102,
			105,
			103,
			117,
			114,
			97,
			116,
			105,
			111,
			110,
			70,
			105,
			108,
			116,
			101,
			114,
			67,
			111,
			110,
			102,
			105,
			103,
			117,
			114,
			97,
			116,
			105,
			111,
			110,
			40,
			76,
			111,
			103,
			103,
			105,
			110,
			103,
			67,
			111,
			110,
			102,
			105,
			103,
			117,
			114,
			97,
			116,
			105,
			111,
			110,
			84,
			114,
			105,
			103,
			103,
			101,
			114,
			67,
			111,
			110,
			102,
			105,
			103,
			117,
			114,
			97,
			116,
			105,
			111,
			110,
			50,
			76,
			111,
			103,
			103,
			105,
			110,
			103,
			67,
			111,
			110,
			102,
			105,
			103,
			117,
			114,
			97,
			116,
			105,
			111,
			110,
			68,
			105,
			97,
			103,
			110,
			111,
			115,
			116,
			105,
			99,
			65,
			99,
			116,
			105,
			111,
			110,
			115,
			67,
			111,
			110,
			102,
			105,
			103,
			117,
			114,
			97,
			116,
			105,
			111,
			110,
			17,
			70,
			105,
			108,
			101,
			70,
			111,
			114,
			109,
			97,
			116,
			86,
			101,
			114,
			115,
			105,
			111,
			110,
			0,
			0,
			0,
			0,
			17,
			0,
			0,
			0,
			0,
			1,
			5,
			1,
			8,
			1,
			9,
			1,
			10,
			1,
			11,
			1,
			12,
			1,
			13,
			1,
			14,
			1,
			15,
			1,
			16,
			1,
			17,
			1,
			18,
			1,
			19,
			1,
			21,
			1,
			22,
			1,
			75,
			1,
			2,
			0,
			0,
			0,
			2,
			0,
			0,
			1,
			0,
			7,
			0,
			12,
			1,
			0,
			0,
			0,
			0,
			2,
			0,
			0,
			0,
			16,
			105,
			110,
			100,
			101,
			110,
			116,
			45,
			105,
			110,
			99,
			114,
			101,
			109,
			101,
			110,
			116,
			14,
			76,
			111,
			103,
			103,
			101,
			114,
			84,
			121,
			112,
			101,
			78,
			97,
			109,
			101,
			0,
			0,
			0,
			0
		};

		private static Type[] ebTypes;

		private static XPathNavigator SyncToNavigator(XPathNavigator xPathNavigator, XPathNavigator xPathNavigator2)
		{
			if (xPathNavigator != null && xPathNavigator.MoveTo(xPathNavigator2))
			{
				return xPathNavigator;
			}
			return xPathNavigator2.Clone();
		}

		private static void Execute(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime)
		{
			Convert_12_to_13.Root({urn:schemas-microsoft-com:xslt-debug}runtime);
		}

		private static void Root(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			output.WriteStartRoot();
			Convert_12_to_13.<xsl:apply-templates> (2)({urn:schemas-microsoft-com:xslt-debug}runtime, {urn:schemas-microsoft-com:xslt-debug}runtime.ExternalContext.DefaultDataSource);
			output.WriteEndRoot();
		}

		private static void <xsl:template name="newline">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			output.WriteRaw("\n    ");
		}

		private static void <xsl:template match="comment() | processing-instruction()">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current, string indent)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			Convert_12_to_13.<xsl:template name="newline">({urn:schemas-microsoft-com:xslt-debug}runtime);
			output.WriteString(indent);
			if (output.StartCopy({urn:schemas-microsoft-com:xslt-debug}current))
			{
				output.EndCopy({urn:schemas-microsoft-com:xslt-debug}current);
			}
		}

		private static void <xsl:template match="text()">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current, string indent)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			Convert_12_to_13.<xsl:template name="newline">({urn:schemas-microsoft-com:xslt-debug}runtime);
			output.WriteString(indent);
			output.WriteString(XsltFunctions.NormalizeSpace({urn:schemas-microsoft-com:xslt-debug}current.Value));
		}

		private static void <xsl:template match="VLConfig:FileFormatVersion" priority="1">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			output.WriteStartElement("", "FileFormatVersion", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteNamespaceDeclarationUnchecked("i", "http://www.w3.org/2001/XMLSchema-instance");
			output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("13");
			output.WriteEndElement();
		}

		private unsafe static void <xsl:template match="VLConfig:HardwareConfigurationLogDataStorage" priority="1">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			output.WriteStartElement("", "HardwareConfigurationLogDataStorage", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteNamespaceDeclarationUnchecked("i", "http://www.w3.org/2001/XMLSchema-instance");
			output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("", "LogDataStorageEventActivationDelayAfterStart", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("2000");
			output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteEndElementUnchecked("", "LogDataStorageEventActivationDelayAfterStart", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			ElementContentIterator elementContentIterator;
			elementContentIterator.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(2), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
			while (elementContentIterator.MoveNext())
			{
				XPathItem current = elementContentIterator.Current;
				output.WriteItem(current);
			}
			ElementContentIterator elementContentIterator2;
			elementContentIterator2.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
			while (elementContentIterator2.MoveNext())
			{
				XPathItem current2 = elementContentIterator2.Current;
				output.WriteItem(current2);
			}
			ElementContentIterator elementContentIterator3;
			elementContentIterator3.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(4), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
			while (elementContentIterator3.MoveNext())
			{
				XPathItem current3 = elementContentIterator3.Current;
				output.WriteItem(current3);
			}
			XPathNavigator xPathNavigator = Convert_12_to_13.SyncToNavigator(xPathNavigator, {urn:schemas-microsoft-com:xslt-debug}current);
			xPathNavigator.MoveToRoot();
			DescendantIterator descendantIterator;
			descendantIterator.Create(xPathNavigator, {urn:schemas-microsoft-com:xslt-debug}runtime.GetNameFilter(1), false);
			string arg_20C_0;
			while (descendantIterator.MoveNext())
			{
				XPathNavigator xPathNavigator2 = Convert_12_to_13.SyncToNavigator(xPathNavigator2, descendantIterator.Current);
				if (xPathNavigator2.MoveToAttribute({urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(6), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(7)))
				{
					arg_20C_0 = xPathNavigator2.Value;
					IL_20C:
					string text = arg_20C_0;
					if (string.Equals(text, ""))
					{
						output.WriteStartElementUnchecked("", "LogDataStorageStopCyclicCommunicationEvent", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteStartAttributeUnchecked("i", "nil", "http://www.w3.org/2001/XMLSchema-instance");
						output.WriteStringUnchecked("true");
						output.WriteEndAttributeUnchecked();
						output.StartElementContentUnchecked();
						output.WriteEndElementUnchecked("", "LogDataStorageStopCyclicCommunicationEvent", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					}
					else
					{
						output.WriteStartElement("", "LogDataStorageStopCyclicCommunicationEvent", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
						output.WriteStringUnchecked("0");
						output.WriteEndAttributeUnchecked();
						output.WriteStartAttributeUnchecked("i", "type", "http://www.w3.org/2001/XMLSchema-instance");
						output.WriteStringUnchecked(text);
						output.WriteEndAttributeUnchecked();
						if (string.Equals(text, "DigitalInputEvent"))
						{
							output.StartElementContentUnchecked();
							output.WriteStartElementUnchecked("", "DigitalInputDebounceTime", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
							output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
							output.WriteStringUnchecked("0");
							output.WriteEndAttributeUnchecked();
							output.StartElementContentUnchecked();
							output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
							output.StartElementContentUnchecked();
							output.WriteStringUnchecked("50");
							output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
							output.WriteEndElementUnchecked("", "DigitalInputDebounceTime", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
							ContentMergeIterator contentMergeIterator;
							contentMergeIterator.Create({urn:schemas-microsoft-com:xslt-debug}runtime.GetNameFilter(2));
							XPathNavigator xPathNavigator3 = Convert_12_to_13.SyncToNavigator(xPathNavigator3, {urn:schemas-microsoft-com:xslt-debug}current);
							xPathNavigator3.MoveToRoot();
							DescendantIterator descendantIterator2;
							descendantIterator2.Create(xPathNavigator3, {urn:schemas-microsoft-com:xslt-debug}runtime.GetNameFilter(1), false);
							while (true)
							{
								if (!descendantIterator2.MoveNext())
								{
									goto IL_3CA;
								}
								ContentMergeIterator* arg_3CD_0 = ref contentMergeIterator;
								XPathNavigator arg_3CD_1 = descendantIterator2.Current;
								IL_3CD:
								switch (arg_3CD_0.MoveNext(arg_3CD_1))
								{
								case IteratorResult.NoMoreNodes:
									goto IL_3F2;
								case IteratorResult.NeedInputNode:
									continue;
								default:
								{
									XPathItem current4 = contentMergeIterator.Current;
									output.WriteItem(current4);
									break;
								}
								}
								IL_3CA:
								arg_3CD_0 = ref contentMergeIterator;
								arg_3CD_1 = null;
								goto IL_3CD;
							}
							IL_3F2:
							ContentMergeIterator contentMergeIterator2;
							contentMergeIterator2.Create({urn:schemas-microsoft-com:xslt-debug}runtime.GetNameFilter(3));
							XPathNavigator xPathNavigator4 = Convert_12_to_13.SyncToNavigator(xPathNavigator4, {urn:schemas-microsoft-com:xslt-debug}current);
							xPathNavigator4.MoveToRoot();
							DescendantIterator descendantIterator3;
							descendantIterator3.Create(xPathNavigator4, {urn:schemas-microsoft-com:xslt-debug}runtime.GetNameFilter(1), false);
							while (true)
							{
								if (!descendantIterator3.MoveNext())
								{
									goto IL_43C;
								}
								ContentMergeIterator* arg_43F_0 = ref contentMergeIterator2;
								XPathNavigator arg_43F_1 = descendantIterator3.Current;
								IL_43F:
								switch (arg_43F_0.MoveNext(arg_43F_1))
								{
								case IteratorResult.NoMoreNodes:
									goto IL_464;
								case IteratorResult.NeedInputNode:
									continue;
								default:
								{
									XPathItem current5 = contentMergeIterator2.Current;
									output.WriteItem(current5);
									break;
								}
								}
								IL_43C:
								arg_43F_0 = ref contentMergeIterator2;
								arg_43F_1 = null;
								goto IL_43F;
							}
							IL_464:
							output.WriteStartElementUnchecked("", "DigitalInputIsDebounceActive", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
							output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
							output.WriteStringUnchecked("0");
							output.WriteEndAttributeUnchecked();
							output.StartElementContentUnchecked();
							output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
							output.StartElementContentUnchecked();
							output.WriteStringUnchecked("true");
							output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
							output.WriteEndElementUnchecked("", "DigitalInputIsDebounceActive", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						}
						else if (string.Equals(text, "SymbolicSignalEvent"))
						{
							ContentMergeIterator contentMergeIterator3;
							contentMergeIterator3.Create({urn:schemas-microsoft-com:xslt-debug}runtime.GetNameFilter(4));
							XPathNavigator xPathNavigator5 = Convert_12_to_13.SyncToNavigator(xPathNavigator5, {urn:schemas-microsoft-com:xslt-debug}current);
							xPathNavigator5.MoveToRoot();
							DescendantIterator descendantIterator4;
							descendantIterator4.Create(xPathNavigator5, {urn:schemas-microsoft-com:xslt-debug}runtime.GetNameFilter(1), false);
							while (true)
							{
								if (!descendantIterator4.MoveNext())
								{
									goto IL_555;
								}
								ContentMergeIterator* arg_558_0 = ref contentMergeIterator3;
								XPathNavigator arg_558_1 = descendantIterator4.Current;
								IL_558:
								switch (arg_558_0.MoveNext(arg_558_1))
								{
								case IteratorResult.NoMoreNodes:
									goto IL_57D;
								case IteratorResult.NeedInputNode:
									continue;
								default:
								{
									XPathItem current6 = contentMergeIterator3.Current;
									output.WriteItem(current6);
									break;
								}
								}
								IL_555:
								arg_558_0 = ref contentMergeIterator3;
								arg_558_1 = null;
								goto IL_558;
							}
							IL_57D:
							ContentMergeIterator contentMergeIterator4;
							contentMergeIterator4.Create({urn:schemas-microsoft-com:xslt-debug}runtime.GetNameFilter(5));
							XPathNavigator xPathNavigator6 = Convert_12_to_13.SyncToNavigator(xPathNavigator6, {urn:schemas-microsoft-com:xslt-debug}current);
							xPathNavigator6.MoveToRoot();
							DescendantIterator descendantIterator5;
							descendantIterator5.Create(xPathNavigator6, {urn:schemas-microsoft-com:xslt-debug}runtime.GetNameFilter(1), false);
							while (true)
							{
								if (!descendantIterator5.MoveNext())
								{
									goto IL_5C7;
								}
								ContentMergeIterator* arg_5CA_0 = ref contentMergeIterator4;
								XPathNavigator arg_5CA_1 = descendantIterator5.Current;
								IL_5CA:
								switch (arg_5CA_0.MoveNext(arg_5CA_1))
								{
								case IteratorResult.NoMoreNodes:
									goto IL_5EF;
								case IteratorResult.NeedInputNode:
									continue;
								default:
								{
									XPathItem current7 = contentMergeIterator4.Current;
									output.WriteItem(current7);
									break;
								}
								}
								IL_5C7:
								arg_5CA_0 = ref contentMergeIterator4;
								arg_5CA_1 = null;
								goto IL_5CA;
							}
							IL_5EF:
							ContentMergeIterator contentMergeIterator5;
							contentMergeIterator5.Create({urn:schemas-microsoft-com:xslt-debug}runtime.GetNameFilter(6));
							XPathNavigator xPathNavigator7 = Convert_12_to_13.SyncToNavigator(xPathNavigator7, {urn:schemas-microsoft-com:xslt-debug}current);
							xPathNavigator7.MoveToRoot();
							DescendantIterator descendantIterator6;
							descendantIterator6.Create(xPathNavigator7, {urn:schemas-microsoft-com:xslt-debug}runtime.GetNameFilter(1), false);
							while (true)
							{
								if (!descendantIterator6.MoveNext())
								{
									goto IL_639;
								}
								ContentMergeIterator* arg_63C_0 = ref contentMergeIterator5;
								XPathNavigator arg_63C_1 = descendantIterator6.Current;
								IL_63C:
								switch (arg_63C_0.MoveNext(arg_63C_1))
								{
								case IteratorResult.NoMoreNodes:
									goto IL_661;
								case IteratorResult.NeedInputNode:
									continue;
								default:
								{
									XPathItem current8 = contentMergeIterator5.Current;
									output.WriteItem(current8);
									break;
								}
								}
								IL_639:
								arg_63C_0 = ref contentMergeIterator5;
								arg_63C_1 = null;
								goto IL_63C;
							}
							IL_661:
							ContentMergeIterator contentMergeIterator6;
							contentMergeIterator6.Create({urn:schemas-microsoft-com:xslt-debug}runtime.GetNameFilter(7));
							XPathNavigator xPathNavigator8 = Convert_12_to_13.SyncToNavigator(xPathNavigator8, {urn:schemas-microsoft-com:xslt-debug}current);
							xPathNavigator8.MoveToRoot();
							DescendantIterator descendantIterator7;
							descendantIterator7.Create(xPathNavigator8, {urn:schemas-microsoft-com:xslt-debug}runtime.GetNameFilter(1), false);
							while (true)
							{
								if (!descendantIterator7.MoveNext())
								{
									goto IL_6AB;
								}
								ContentMergeIterator* arg_6AE_0 = ref contentMergeIterator6;
								XPathNavigator arg_6AE_1 = descendantIterator7.Current;
								IL_6AE:
								switch (arg_6AE_0.MoveNext(arg_6AE_1))
								{
								case IteratorResult.NoMoreNodes:
									goto IL_6D3;
								case IteratorResult.NeedInputNode:
									continue;
								default:
								{
									XPathItem current9 = contentMergeIterator6.Current;
									output.WriteItem(current9);
									break;
								}
								}
								IL_6AB:
								arg_6AE_0 = ref contentMergeIterator6;
								arg_6AE_1 = null;
								goto IL_6AE;
							}
							IL_6D3:
							ContentMergeIterator contentMergeIterator7;
							contentMergeIterator7.Create({urn:schemas-microsoft-com:xslt-debug}runtime.GetNameFilter(8));
							XPathNavigator xPathNavigator9 = Convert_12_to_13.SyncToNavigator(xPathNavigator9, {urn:schemas-microsoft-com:xslt-debug}current);
							xPathNavigator9.MoveToRoot();
							DescendantIterator descendantIterator8;
							descendantIterator8.Create(xPathNavigator9, {urn:schemas-microsoft-com:xslt-debug}runtime.GetNameFilter(1), false);
							while (true)
							{
								if (!descendantIterator8.MoveNext())
								{
									goto IL_71D;
								}
								ContentMergeIterator* arg_720_0 = ref contentMergeIterator7;
								XPathNavigator arg_720_1 = descendantIterator8.Current;
								IL_720:
								switch (arg_720_0.MoveNext(arg_720_1))
								{
								case IteratorResult.NoMoreNodes:
									goto IL_745;
								case IteratorResult.NeedInputNode:
									continue;
								default:
								{
									XPathItem current10 = contentMergeIterator7.Current;
									output.WriteItem(current10);
									break;
								}
								}
								IL_71D:
								arg_720_0 = ref contentMergeIterator7;
								arg_720_1 = null;
								goto IL_720;
							}
							IL_745:
							ContentMergeIterator contentMergeIterator8;
							contentMergeIterator8.Create({urn:schemas-microsoft-com:xslt-debug}runtime.GetNameFilter(9));
							XPathNavigator xPathNavigator10 = Convert_12_to_13.SyncToNavigator(xPathNavigator10, {urn:schemas-microsoft-com:xslt-debug}current);
							xPathNavigator10.MoveToRoot();
							DescendantIterator descendantIterator9;
							descendantIterator9.Create(xPathNavigator10, {urn:schemas-microsoft-com:xslt-debug}runtime.GetNameFilter(1), false);
							while (true)
							{
								if (!descendantIterator9.MoveNext())
								{
									goto IL_790;
								}
								ContentMergeIterator* arg_793_0 = ref contentMergeIterator8;
								XPathNavigator arg_793_1 = descendantIterator9.Current;
								IL_793:
								switch (arg_793_0.MoveNext(arg_793_1))
								{
								case IteratorResult.NoMoreNodes:
									goto IL_7B8;
								case IteratorResult.NeedInputNode:
									continue;
								default:
								{
									XPathItem current11 = contentMergeIterator8.Current;
									output.WriteItem(current11);
									break;
								}
								}
								IL_790:
								arg_793_0 = ref contentMergeIterator8;
								arg_793_1 = null;
								goto IL_793;
							}
							IL_7B8:
							ContentMergeIterator contentMergeIterator9;
							contentMergeIterator9.Create({urn:schemas-microsoft-com:xslt-debug}runtime.GetNameFilter(10));
							XPathNavigator xPathNavigator11 = Convert_12_to_13.SyncToNavigator(xPathNavigator11, {urn:schemas-microsoft-com:xslt-debug}current);
							xPathNavigator11.MoveToRoot();
							DescendantIterator descendantIterator10;
							descendantIterator10.Create(xPathNavigator11, {urn:schemas-microsoft-com:xslt-debug}runtime.GetNameFilter(1), false);
							while (true)
							{
								if (!descendantIterator10.MoveNext())
								{
									goto IL_803;
								}
								ContentMergeIterator* arg_806_0 = ref contentMergeIterator9;
								XPathNavigator arg_806_1 = descendantIterator10.Current;
								IL_806:
								switch (arg_806_0.MoveNext(arg_806_1))
								{
								case IteratorResult.NoMoreNodes:
									goto IL_82B;
								case IteratorResult.NeedInputNode:
									continue;
								default:
								{
									XPathItem current12 = contentMergeIterator9.Current;
									output.WriteItem(current12);
									break;
								}
								}
								IL_803:
								arg_806_0 = ref contentMergeIterator9;
								arg_806_1 = null;
								goto IL_806;
							}
							IL_82B:
							ContentMergeIterator contentMergeIterator10;
							contentMergeIterator10.Create({urn:schemas-microsoft-com:xslt-debug}runtime.GetNameFilter(11));
							XPathNavigator xPathNavigator12 = Convert_12_to_13.SyncToNavigator(xPathNavigator12, {urn:schemas-microsoft-com:xslt-debug}current);
							xPathNavigator12.MoveToRoot();
							DescendantIterator descendantIterator11;
							descendantIterator11.Create(xPathNavigator12, {urn:schemas-microsoft-com:xslt-debug}runtime.GetNameFilter(1), false);
							while (true)
							{
								if (!descendantIterator11.MoveNext())
								{
									goto IL_876;
								}
								ContentMergeIterator* arg_879_0 = ref contentMergeIterator10;
								XPathNavigator arg_879_1 = descendantIterator11.Current;
								IL_879:
								switch (arg_879_0.MoveNext(arg_879_1))
								{
								case IteratorResult.NoMoreNodes:
									goto IL_89E;
								case IteratorResult.NeedInputNode:
									continue;
								default:
								{
									XPathItem current13 = contentMergeIterator10.Current;
									output.WriteItem(current13);
									break;
								}
								}
								IL_876:
								arg_879_0 = ref contentMergeIterator10;
								arg_879_1 = null;
								goto IL_879;
							}
							IL_89E:
							output.WriteStartElement("", "SymbolicSignalEventNetworkName", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
							output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
							output.WriteStringUnchecked("0");
							output.WriteEndAttributeUnchecked();
							ContentMergeIterator contentMergeIterator11;
							contentMergeIterator11.Create({urn:schemas-microsoft-com:xslt-debug}runtime.GetNameFilter(12));
							ContentMergeIterator contentMergeIterator12;
							contentMergeIterator12.Create({urn:schemas-microsoft-com:xslt-debug}runtime.GetNameFilter(13));
							XPathNavigator xPathNavigator13 = Convert_12_to_13.SyncToNavigator(xPathNavigator13, {urn:schemas-microsoft-com:xslt-debug}current);
							xPathNavigator13.MoveToRoot();
							DescendantIterator descendantIterator12;
							descendantIterator12.Create(xPathNavigator13, {urn:schemas-microsoft-com:xslt-debug}runtime.GetNameFilter(1), false);
							while (true)
							{
								if (!descendantIterator12.MoveNext())
								{
									goto IL_933;
								}
								ContentMergeIterator* arg_936_0 = ref contentMergeIterator12;
								XPathNavigator arg_936_1 = descendantIterator12.Current;
								IL_936:
								ContentMergeIterator* arg_959_0;
								XPathNavigator arg_959_1;
								switch (arg_936_0.MoveNext(arg_936_1))
								{
								case IteratorResult.NoMoreNodes:
									IL_956:
									arg_959_0 = ref contentMergeIterator11;
									arg_959_1 = null;
									break;
								case IteratorResult.NeedInputNode:
									continue;
								default:
									arg_959_0 = ref contentMergeIterator11;
									arg_959_1 = contentMergeIterator12.Current;
									break;
								}
								switch (arg_959_0.MoveNext(arg_959_1))
								{
								case IteratorResult.NoMoreNodes:
									goto IL_998;
								case IteratorResult.NeedInputNode:
									IL_933:
									arg_936_0 = ref contentMergeIterator12;
									arg_936_1 = null;
									goto IL_936;
								default:
								{
									XPathNavigator xPathNavigator14 = Convert_12_to_13.SyncToNavigator(xPathNavigator14, contentMergeIterator11.Current);
									if (xPathNavigator14.MoveToAttribute({urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(20), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(7)))
									{
										goto Block_32;
									}
									goto IL_956;
								}
								}
							}
							Block_32:
							output.StartElementContentUnchecked();
							output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
							output.StartElementContentUnchecked();
							output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
							goto IL_A78;
							IL_998:
							ContentMergeIterator contentMergeIterator13;
							contentMergeIterator13.Create({urn:schemas-microsoft-com:xslt-debug}runtime.GetNameFilter(12));
							ContentMergeIterator contentMergeIterator14;
							contentMergeIterator14.Create({urn:schemas-microsoft-com:xslt-debug}runtime.GetNameFilter(13));
							XPathNavigator xPathNavigator15 = Convert_12_to_13.SyncToNavigator(xPathNavigator15, {urn:schemas-microsoft-com:xslt-debug}current);
							xPathNavigator15.MoveToRoot();
							DescendantIterator descendantIterator13;
							descendantIterator13.Create(xPathNavigator15, {urn:schemas-microsoft-com:xslt-debug}runtime.GetNameFilter(1), false);
							while (true)
							{
								if (!descendantIterator13.MoveNext())
								{
									goto IL_9F2;
								}
								ContentMergeIterator* arg_9F5_0 = ref contentMergeIterator14;
								XPathNavigator arg_9F5_1 = descendantIterator13.Current;
								IL_9F5:
								ContentMergeIterator* arg_A18_0;
								XPathNavigator arg_A18_1;
								switch (arg_9F5_0.MoveNext(arg_9F5_1))
								{
								case IteratorResult.NoMoreNodes:
									IL_A15:
									arg_A18_0 = ref contentMergeIterator13;
									arg_A18_1 = null;
									break;
								case IteratorResult.NeedInputNode:
									continue;
								default:
									arg_A18_0 = ref contentMergeIterator13;
									arg_A18_1 = contentMergeIterator14.Current;
									break;
								}
								switch (arg_A18_0.MoveNext(arg_A18_1))
								{
								case IteratorResult.NoMoreNodes:
									goto IL_A3D;
								case IteratorResult.NeedInputNode:
									IL_9F2:
									arg_9F5_0 = ref contentMergeIterator14;
									arg_9F5_1 = null;
									goto IL_9F5;
								default:
								{
									XPathItem current14 = contentMergeIterator13.Current;
									output.WriteItem(current14);
									goto IL_A15;
								}
								}
							}
							IL_A3D:
							IL_A78:
							output.WriteEndElement();
							ContentMergeIterator contentMergeIterator15;
							contentMergeIterator15.Create({urn:schemas-microsoft-com:xslt-debug}runtime.GetNameFilter(14));
							XPathNavigator xPathNavigator16 = Convert_12_to_13.SyncToNavigator(xPathNavigator16, {urn:schemas-microsoft-com:xslt-debug}current);
							xPathNavigator16.MoveToRoot();
							DescendantIterator descendantIterator14;
							descendantIterator14.Create(xPathNavigator16, {urn:schemas-microsoft-com:xslt-debug}runtime.GetNameFilter(1), false);
							while (true)
							{
								if (!descendantIterator14.MoveNext())
								{
									goto IL_AC9;
								}
								ContentMergeIterator* arg_ACC_0 = ref contentMergeIterator15;
								XPathNavigator arg_ACC_1 = descendantIterator14.Current;
								IL_ACC:
								switch (arg_ACC_0.MoveNext(arg_ACC_1))
								{
								case IteratorResult.NoMoreNodes:
									goto IL_AF1;
								case IteratorResult.NeedInputNode:
									continue;
								default:
								{
									XPathItem current15 = contentMergeIterator15.Current;
									output.WriteItem(current15);
									break;
								}
								}
								IL_AC9:
								arg_ACC_0 = ref contentMergeIterator15;
								arg_ACC_1 = null;
								goto IL_ACC;
							}
							IL_AF1:
							ContentMergeIterator contentMergeIterator16;
							contentMergeIterator16.Create({urn:schemas-microsoft-com:xslt-debug}runtime.GetNameFilter(15));
							XPathNavigator xPathNavigator17 = Convert_12_to_13.SyncToNavigator(xPathNavigator17, {urn:schemas-microsoft-com:xslt-debug}current);
							xPathNavigator17.MoveToRoot();
							DescendantIterator descendantIterator15;
							descendantIterator15.Create(xPathNavigator17, {urn:schemas-microsoft-com:xslt-debug}runtime.GetNameFilter(1), false);
							while (true)
							{
								if (!descendantIterator15.MoveNext())
								{
									goto IL_B3C;
								}
								ContentMergeIterator* arg_B3F_0 = ref contentMergeIterator16;
								XPathNavigator arg_B3F_1 = descendantIterator15.Current;
								IL_B3F:
								switch (arg_B3F_0.MoveNext(arg_B3F_1))
								{
								case IteratorResult.NoMoreNodes:
									goto IL_B64;
								case IteratorResult.NeedInputNode:
									continue;
								default:
								{
									XPathItem current16 = contentMergeIterator16.Current;
									output.WriteItem(current16);
									break;
								}
								}
								IL_B3C:
								arg_B3F_0 = ref contentMergeIterator16;
								arg_B3F_1 = null;
								goto IL_B3F;
							}
							IL_B64:;
						}
						else
						{
							ContentMergeIterator contentMergeIterator17;
							contentMergeIterator17.Create({urn:schemas-microsoft-com:xslt-debug}runtime.GetTypeFilter(XPathNodeType.Element));
							XPathNavigator xPathNavigator18 = Convert_12_to_13.SyncToNavigator(xPathNavigator18, {urn:schemas-microsoft-com:xslt-debug}current);
							xPathNavigator18.MoveToRoot();
							DescendantIterator descendantIterator16;
							descendantIterator16.Create(xPathNavigator18, {urn:schemas-microsoft-com:xslt-debug}runtime.GetNameFilter(1), false);
							while (true)
							{
								if (!descendantIterator16.MoveNext())
								{
									goto IL_BB3;
								}
								ContentMergeIterator* arg_BB6_0 = ref contentMergeIterator17;
								XPathNavigator arg_BB6_1 = descendantIterator16.Current;
								IL_BB6:
								switch (arg_BB6_0.MoveNext(arg_BB6_1))
								{
								case IteratorResult.NoMoreNodes:
									goto IL_BDB;
								case IteratorResult.NeedInputNode:
									continue;
								default:
								{
									XPathItem current17 = contentMergeIterator17.Current;
									output.WriteItem(current17);
									break;
								}
								}
								IL_BB3:
								arg_BB6_0 = ref contentMergeIterator17;
								arg_BB6_1 = null;
								goto IL_BB6;
							}
						}
						IL_BDB:
						output.WriteEndElement();
					}
					ElementContentIterator elementContentIterator4;
					elementContentIterator4.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(23), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
					while (elementContentIterator4.MoveNext())
					{
						XPathItem current18 = elementContentIterator4.Current;
						output.WriteItem(current18);
					}
					ElementContentIterator elementContentIterator5;
					elementContentIterator5.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(24), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
					while (elementContentIterator5.MoveNext())
					{
						XPathItem current19 = elementContentIterator5.Current;
						output.WriteItem(current19);
					}
					output.WriteEndElement();
					return;
				}
			}
			arg_20C_0 = "";
			goto IL_20C;
		}

		private static void <xsl:template match="VLConfig:LoggingConfigurationDiagnosticActionsConfiguration" priority="1">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			output.WriteStartElement("", "LoggingConfigurationDiagnosticActionsConfiguration", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteNamespaceDeclarationUnchecked("i", "http://www.w3.org/2001/XMLSchema-instance");
			output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			ElementContentIterator elementContentIterator;
			elementContentIterator.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(25), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
			while (elementContentIterator.MoveNext())
			{
				XPathItem current = elementContentIterator.Current;
				output.WriteItem(current);
			}
			ElementContentIterator elementContentIterator2;
			elementContentIterator2.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(26), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
			while (elementContentIterator2.MoveNext())
			{
				XPathItem current2 = elementContentIterator2.Current;
				output.WriteItem(current2);
			}
			ElementContentIterator elementContentIterator3;
			elementContentIterator3.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(27), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
			while (elementContentIterator3.MoveNext())
			{
				XPathItem current3 = elementContentIterator3.Current;
				output.WriteItem(current3);
			}
			output.WriteStartElement("", "DiagnosticActionsConfigurationTriggeredSequences", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.WriteStartAttributeUnchecked("z", "Size", "http://schemas.microsoft.com/2003/10/Serialization/");
			XmlQueryOutput arg_1BB_0 = output;
			int arg_1AE_0;
			int arg_1B1_0 = arg_1AE_0 = 0;
			ElementContentIterator elementContentIterator4;
			elementContentIterator4.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(28), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
			while (elementContentIterator4.MoveNext())
			{
				ElementContentIterator elementContentIterator5;
				elementContentIterator5.Create(elementContentIterator4.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(29), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
				while (elementContentIterator5.MoveNext())
				{
					arg_1B1_0 = ++arg_1AE_0;
				}
			}
			arg_1BB_0.WriteStringUnchecked(XsltConvert.ToString(XsltConvert.ToDouble(arg_1B1_0)));
			output.WriteEndAttributeUnchecked();
			ElementContentIterator elementContentIterator6;
			elementContentIterator6.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(28), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
			while (elementContentIterator6.MoveNext())
			{
				ElementContentIterator elementContentIterator7;
				elementContentIterator7.Create(elementContentIterator6.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(29), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
				IL_206:
				while (elementContentIterator7.MoveNext())
				{
					output.WriteStartElement("", "TriggeredDiagnosticActionSequence", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
					output.WriteStringUnchecked("0");
					output.WriteEndAttributeUnchecked();
					ElementContentIterator elementContentIterator8;
					elementContentIterator8.Create(elementContentIterator7.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(30), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
					while (elementContentIterator8.MoveNext())
					{
						XPathItem current4 = elementContentIterator8.Current;
						output.WriteItem(current4);
					}
					ElementContentIterator elementContentIterator9;
					elementContentIterator9.Create(elementContentIterator7.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(31), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
					while (elementContentIterator9.MoveNext())
					{
						XPathNavigator xPathNavigator = Convert_12_to_13.SyncToNavigator(xPathNavigator, elementContentIterator9.Current);
						if (xPathNavigator.MoveToAttribute({urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(6), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(7)) && string.Equals(xPathNavigator.Value, "DigitalInputEvent"))
						{
							output.WriteStartElement("", "TriggeredDiagnosticActionSequenceEvent", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
							output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
							output.WriteStringUnchecked("0");
							output.WriteEndAttributeUnchecked();
							output.WriteStartAttributeUnchecked("i", "type", "http://www.w3.org/2001/XMLSchema-instance");
							output.WriteStringUnchecked("DigitalInputEvent");
							output.WriteEndAttributeUnchecked();
							output.StartElementContentUnchecked();
							output.WriteStartElementUnchecked("", "DigitalInputDebounceTime", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
							output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
							output.WriteStringUnchecked("0");
							output.WriteEndAttributeUnchecked();
							output.StartElementContentUnchecked();
							output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
							output.StartElementContentUnchecked();
							output.WriteStringUnchecked("50");
							output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
							output.WriteEndElementUnchecked("", "DigitalInputDebounceTime", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
							ElementContentIterator elementContentIterator10;
							elementContentIterator10.Create(elementContentIterator7.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(31), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
							while (elementContentIterator10.MoveNext())
							{
								ElementContentIterator elementContentIterator11;
								elementContentIterator11.Create(elementContentIterator10.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(8), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
								while (elementContentIterator11.MoveNext())
								{
									XPathItem current5 = elementContentIterator11.Current;
									output.WriteItem(current5);
								}
							}
							ElementContentIterator elementContentIterator12;
							elementContentIterator12.Create(elementContentIterator7.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(31), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
							while (elementContentIterator12.MoveNext())
							{
								ElementContentIterator elementContentIterator13;
								elementContentIterator13.Create(elementContentIterator12.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(9), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
								while (elementContentIterator13.MoveNext())
								{
									XPathItem current6 = elementContentIterator13.Current;
									output.WriteItem(current6);
								}
							}
							output.WriteStartElementUnchecked("", "DigitalInputIsDebounceActive", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
							output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
							output.WriteStringUnchecked("0");
							output.WriteEndAttributeUnchecked();
							output.StartElementContentUnchecked();
							output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
							output.StartElementContentUnchecked();
							output.WriteStringUnchecked("true");
							output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
							output.WriteEndElementUnchecked("", "DigitalInputIsDebounceActive", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
							output.WriteEndElement();
							IL_581:
							output.WriteEndElement();
							goto IL_206;
						}
					}
					ElementContentIterator elementContentIterator14;
					elementContentIterator14.Create(elementContentIterator7.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(31), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
					while (elementContentIterator14.MoveNext())
					{
						XPathItem current7 = elementContentIterator14.Current;
						output.WriteItem(current7);
					}
					goto IL_581;
				}
			}
			output.WriteEndElement();
			output.WriteEndElement();
		}

		private static void <xsl:template match="VLConfig:LoggingConfigurationTriggerConfiguration" priority="1">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current, string indent)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			if (string.Equals(Convert_12_to_13.LoggerTypeName({urn:schemas-microsoft-com:xslt-debug}runtime), "GL1000") || string.Equals(Convert_12_to_13.LoggerTypeName({urn:schemas-microsoft-com:xslt-debug}runtime), "GL2000"))
			{
				output.WriteStartElement("", "LoggingConfigurationTriggerConfigList", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteNamespaceDeclarationUnchecked("i", "http://www.w3.org/2001/XMLSchema-instance");
				output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.WriteStartAttributeUnchecked("z", "Size", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("1");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "TriggerConfiguration", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.WriteStartElement("", "TriggerConfigurationMemoryRingBuffer", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				XPathNavigator xPathNavigator = Convert_12_to_13.SyncToNavigator(xPathNavigator, {urn:schemas-microsoft-com:xslt-debug}current);
				xPathNavigator.MoveToRoot();
				ElementContentIterator elementContentIterator;
				elementContentIterator.Create(xPathNavigator, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(32), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
				while (elementContentIterator.MoveNext())
				{
					ElementContentIterator elementContentIterator2;
					elementContentIterator2.Create(elementContentIterator.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(33), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
					while (elementContentIterator2.MoveNext())
					{
						ElementContentIterator elementContentIterator3;
						elementContentIterator3.Create(elementContentIterator2.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(34), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
						while (elementContentIterator3.MoveNext())
						{
							ElementContentIterator elementContentIterator4;
							elementContentIterator4.Create(elementContentIterator3.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(35), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
							while (elementContentIterator4.MoveNext())
							{
								int num = 0;
								ElementContentIterator elementContentIterator5;
								elementContentIterator5.Create(elementContentIterator4.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(36), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
								while (elementContentIterator5.MoveNext())
								{
									num++;
									if (num > 1)
									{
										break;
									}
									if (num == 1)
									{
										NodeKindContentIterator nodeKindContentIterator;
										nodeKindContentIterator.Create(elementContentIterator5.Current, XPathNodeType.Element);
										while (nodeKindContentIterator.MoveNext())
										{
											XPathItem current = nodeKindContentIterator.Current;
											output.WriteItem(current);
										}
									}
								}
							}
						}
					}
				}
				output.WriteEndElement();
				output.WriteStartElement("", "TriggerConfigurationPostTriggerTime", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				ElementContentIterator elementContentIterator6;
				elementContentIterator6.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(37), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
				while (elementContentIterator6.MoveNext())
				{
					NodeKindContentIterator nodeKindContentIterator2;
					nodeKindContentIterator2.Create(elementContentIterator6.Current, XPathNodeType.Element);
					while (nodeKindContentIterator2.MoveNext())
					{
						XPathItem current2 = nodeKindContentIterator2.Current;
						output.WriteItem(current2);
					}
				}
				output.WriteEndElement();
				output.WriteStartElement("", "TriggerConfigurationTriggerList", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.WriteStartAttributeUnchecked("z", "Size", "http://schemas.microsoft.com/2003/10/Serialization/");
				XmlQueryOutput arg_3EA_0 = output;
				int arg_3DD_0;
				int arg_3E0_0 = arg_3DD_0 = 0;
				ElementContentIterator elementContentIterator7;
				elementContentIterator7.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(38), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
				while (elementContentIterator7.MoveNext())
				{
					ElementContentIterator elementContentIterator8;
					elementContentIterator8.Create(elementContentIterator7.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(39), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
					while (elementContentIterator8.MoveNext())
					{
						ElementContentIterator elementContentIterator9;
						elementContentIterator9.Create(elementContentIterator8.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(40), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
						while (elementContentIterator9.MoveNext())
						{
							NodeKindContentIterator nodeKindContentIterator3;
							nodeKindContentIterator3.Create(elementContentIterator9.Current, XPathNodeType.Element);
							if (XsltConvert.ToDouble((!nodeKindContentIterator3.MoveNext()) ? "" : nodeKindContentIterator3.Current.Value) == 1.0)
							{
								arg_3E0_0 = ++arg_3DD_0;
							}
						}
					}
				}
				arg_3EA_0.WriteStringUnchecked(XsltConvert.ToString(XsltConvert.ToDouble(arg_3E0_0)));
				output.WriteEndAttributeUnchecked();
				Convert_12_to_13.<xsl:template name="CopyRecordTriggersInTriggeredLoggingForMemory">({urn:schemas-microsoft-com:xslt-debug}runtime, {urn:schemas-microsoft-com:xslt-debug}current, 1.0);
				output.WriteEndElement();
				output.WriteStartElement("", "TriggerConfigurationTriggerListOnOff", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.WriteStartAttributeUnchecked("z", "Size", "http://schemas.microsoft.com/2003/10/Serialization/");
				XmlQueryOutput arg_51B_0 = output;
				int arg_50E_0;
				int arg_511_0 = arg_50E_0 = 0;
				ElementContentIterator elementContentIterator10;
				elementContentIterator10.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(41), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
				while (elementContentIterator10.MoveNext())
				{
					ElementContentIterator elementContentIterator11;
					elementContentIterator11.Create(elementContentIterator10.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(39), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
					while (elementContentIterator11.MoveNext())
					{
						ElementContentIterator elementContentIterator12;
						elementContentIterator12.Create(elementContentIterator11.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(40), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
						while (elementContentIterator12.MoveNext())
						{
							NodeKindContentIterator nodeKindContentIterator4;
							nodeKindContentIterator4.Create(elementContentIterator12.Current, XPathNodeType.Element);
							if (XsltConvert.ToDouble((!nodeKindContentIterator4.MoveNext()) ? "" : nodeKindContentIterator4.Current.Value) == 1.0)
							{
								arg_511_0 = ++arg_50E_0;
							}
						}
					}
				}
				arg_51B_0.WriteStringUnchecked(XsltConvert.ToString(XsltConvert.ToDouble(arg_511_0)));
				output.WriteEndAttributeUnchecked();
				Convert_12_to_13.<xsl:template name="CopyRecordTriggersInOnOffLoggingForMemory">({urn:schemas-microsoft-com:xslt-debug}runtime, {urn:schemas-microsoft-com:xslt-debug}current, 1.0);
				output.WriteEndElement();
				output.WriteStartElement("", "TriggerConfigurationTriggerMode", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				ElementContentIterator elementContentIterator13;
				elementContentIterator13.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(42), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
				while (elementContentIterator13.MoveNext())
				{
					NodeKindContentIterator nodeKindContentIterator5;
					nodeKindContentIterator5.Create(elementContentIterator13.Current, XPathNodeType.Element);
					while (nodeKindContentIterator5.MoveNext())
					{
						XPathItem current3 = nodeKindContentIterator5.Current;
						output.WriteItem(current3);
					}
				}
				output.WriteEndElement();
				output.WriteEndElementUnchecked("", "TriggerConfiguration", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElement();
			}
			else
			{
				XmlQueryNodeSequence xmlQueryNodeSequence = XmlQueryNodeSequence.CreateOrReuse(xmlQueryNodeSequence);
				XmlQueryNodeSequence arg_646_0 = xmlQueryNodeSequence;
				ElementContentIterator elementContentIterator14;
				elementContentIterator14.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(42), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
				while (elementContentIterator14.MoveNext())
				{
					ElementContentIterator elementContentIterator15;
					elementContentIterator15.Create(elementContentIterator14.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(18), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
					while (elementContentIterator15.MoveNext())
					{
						arg_646_0.AddClone(elementContentIterator15.Current);
						arg_646_0 = xmlQueryNodeSequence;
					}
				}
				int arg_702_0;
				int arg_705_0 = arg_702_0 = 0;
				ElementContentIterator elementContentIterator16;
				elementContentIterator16.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(41), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
				while (elementContentIterator16.MoveNext())
				{
					ElementContentIterator elementContentIterator17;
					elementContentIterator17.Create(elementContentIterator16.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(39), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
					while (elementContentIterator17.MoveNext())
					{
						ElementContentIterator elementContentIterator18;
						elementContentIterator18.Create(elementContentIterator17.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(40), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
						while (elementContentIterator18.MoveNext())
						{
							NodeKindContentIterator nodeKindContentIterator6;
							nodeKindContentIterator6.Create(elementContentIterator18.Current, XPathNodeType.Element);
							if (XsltConvert.ToDouble((!nodeKindContentIterator6.MoveNext()) ? "" : nodeKindContentIterator6.Current.Value) == 1.0)
							{
								arg_705_0 = ++arg_702_0;
							}
						}
					}
				}
				double num2 = XsltConvert.ToDouble(arg_705_0);
				int arg_7BE_0;
				int arg_7C1_0 = arg_7BE_0 = 0;
				ElementContentIterator elementContentIterator19;
				elementContentIterator19.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(41), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
				while (elementContentIterator19.MoveNext())
				{
					ElementContentIterator elementContentIterator20;
					elementContentIterator20.Create(elementContentIterator19.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(39), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
					while (elementContentIterator20.MoveNext())
					{
						ElementContentIterator elementContentIterator21;
						elementContentIterator21.Create(elementContentIterator20.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(40), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
						while (elementContentIterator21.MoveNext())
						{
							NodeKindContentIterator nodeKindContentIterator7;
							nodeKindContentIterator7.Create(elementContentIterator21.Current, XPathNodeType.Element);
							if (XsltConvert.ToDouble((!nodeKindContentIterator7.MoveNext()) ? "" : nodeKindContentIterator7.Current.Value) == 2.0)
							{
								arg_7C1_0 = ++arg_7BE_0;
							}
						}
					}
				}
				double num3 = XsltConvert.ToDouble(arg_7C1_0);
				output.WriteStartElement("", "LoggingConfigurationTriggerConfigList", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteNamespaceDeclarationUnchecked("i", "http://www.w3.org/2001/XMLSchema-instance");
				output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.WriteStartAttributeUnchecked("z", "Size", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("2");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "TriggerConfiguration", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				int num4 = -1;
				XPathNavigator xPathNavigator2;
				do
				{
					num4++;
					if (num4 >= ((ICollection<XPathNavigator>)xmlQueryNodeSequence).Count)
					{
						goto IL_C13;
					}
					xPathNavigator2 = ((IList<XPathNavigator>)xmlQueryNodeSequence)[num4];
				}
				while (!string.Equals(xPathNavigator2.Value, "OnOff"));
				if (num2 == 0.0 && num3 > 0.0)
				{
					output.StartElementContentUnchecked();
					output.WriteStartElementUnchecked("", "TriggerConfigurationMemoryRingBuffer", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
					output.WriteStringUnchecked("0");
					output.WriteEndAttributeUnchecked();
					output.StartElementContentUnchecked();
					output.WriteStartElementUnchecked("", "MemoryRingBufferIsActive", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
					output.WriteStringUnchecked("0");
					output.WriteEndAttributeUnchecked();
					output.StartElementContentUnchecked();
					output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.StartElementContentUnchecked();
					output.WriteStringUnchecked("false");
					output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.WriteEndElementUnchecked("", "MemoryRingBufferIsActive", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					XPathNavigator xPathNavigator3 = Convert_12_to_13.SyncToNavigator(xPathNavigator3, {urn:schemas-microsoft-com:xslt-debug}current);
					xPathNavigator3.MoveToRoot();
					ElementContentIterator elementContentIterator22;
					elementContentIterator22.Create(xPathNavigator3, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(32), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
					while (elementContentIterator22.MoveNext())
					{
						ElementContentIterator elementContentIterator23;
						elementContentIterator23.Create(elementContentIterator22.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(33), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
						while (elementContentIterator23.MoveNext())
						{
							ElementContentIterator elementContentIterator24;
							elementContentIterator24.Create(elementContentIterator23.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(34), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
							while (elementContentIterator24.MoveNext())
							{
								ElementContentIterator elementContentIterator25;
								elementContentIterator25.Create(elementContentIterator24.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(35), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
								while (elementContentIterator25.MoveNext())
								{
									int num5 = 0;
									ElementContentIterator elementContentIterator26;
									elementContentIterator26.Create(elementContentIterator25.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(36), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
									while (elementContentIterator26.MoveNext())
									{
										num5++;
										if (num5 > 1)
										{
											break;
										}
										if (num5 == 1)
										{
											ElementContentIterator elementContentIterator27;
											elementContentIterator27.Create(elementContentIterator26.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(43), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
											while (elementContentIterator27.MoveNext())
											{
												XPathItem current4 = elementContentIterator27.Current;
												output.WriteItem(current4);
											}
										}
									}
								}
							}
						}
					}
					XPathNavigator xPathNavigator4 = Convert_12_to_13.SyncToNavigator(xPathNavigator4, {urn:schemas-microsoft-com:xslt-debug}current);
					xPathNavigator4.MoveToRoot();
					ElementContentIterator elementContentIterator28;
					elementContentIterator28.Create(xPathNavigator4, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(32), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
					while (elementContentIterator28.MoveNext())
					{
						ElementContentIterator elementContentIterator29;
						elementContentIterator29.Create(elementContentIterator28.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(33), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
						while (elementContentIterator29.MoveNext())
						{
							ElementContentIterator elementContentIterator30;
							elementContentIterator30.Create(elementContentIterator29.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(34), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
							while (elementContentIterator30.MoveNext())
							{
								ElementContentIterator elementContentIterator31;
								elementContentIterator31.Create(elementContentIterator30.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(35), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
								while (elementContentIterator31.MoveNext())
								{
									int num6 = 0;
									ElementContentIterator elementContentIterator32;
									elementContentIterator32.Create(elementContentIterator31.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(36), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
									while (elementContentIterator32.MoveNext())
									{
										num6++;
										if (num6 > 1)
										{
											break;
										}
										if (num6 == 1)
										{
											ElementContentIterator elementContentIterator33;
											elementContentIterator33.Create(elementContentIterator32.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(44), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
											while (elementContentIterator33.MoveNext())
											{
												XPathItem current5 = elementContentIterator33.Current;
												output.WriteItem(current5);
											}
										}
									}
								}
							}
						}
					}
					output.WriteEndElementUnchecked("", "TriggerConfigurationMemoryRingBuffer", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					goto IL_D5F;
				}
				IL_C13:
				output.WriteStartElement("", "TriggerConfigurationMemoryRingBuffer", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				XPathNavigator xPathNavigator5 = Convert_12_to_13.SyncToNavigator(xPathNavigator5, {urn:schemas-microsoft-com:xslt-debug}current);
				xPathNavigator5.MoveToRoot();
				ElementContentIterator elementContentIterator34;
				elementContentIterator34.Create(xPathNavigator5, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(32), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
				while (elementContentIterator34.MoveNext())
				{
					ElementContentIterator elementContentIterator35;
					elementContentIterator35.Create(elementContentIterator34.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(33), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
					while (elementContentIterator35.MoveNext())
					{
						ElementContentIterator elementContentIterator36;
						elementContentIterator36.Create(elementContentIterator35.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(34), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
						while (elementContentIterator36.MoveNext())
						{
							ElementContentIterator elementContentIterator37;
							elementContentIterator37.Create(elementContentIterator36.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(35), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
							while (elementContentIterator37.MoveNext())
							{
								int num7 = 0;
								ElementContentIterator elementContentIterator38;
								elementContentIterator38.Create(elementContentIterator37.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(36), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
								while (elementContentIterator38.MoveNext())
								{
									num7++;
									if (num7 > 1)
									{
										break;
									}
									if (num7 == 1)
									{
										NodeKindContentIterator nodeKindContentIterator8;
										nodeKindContentIterator8.Create(elementContentIterator38.Current, XPathNodeType.Element);
										while (nodeKindContentIterator8.MoveNext())
										{
											XPathItem current6 = nodeKindContentIterator8.Current;
											output.WriteItem(current6);
										}
									}
								}
							}
						}
					}
				}
				output.WriteEndElement();
				IL_D5F:
				output.WriteStartElement("", "TriggerConfigurationPostTriggerTime", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				ElementContentIterator elementContentIterator39;
				elementContentIterator39.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(37), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
				while (elementContentIterator39.MoveNext())
				{
					NodeKindContentIterator nodeKindContentIterator9;
					nodeKindContentIterator9.Create(elementContentIterator39.Current, XPathNodeType.Element);
					while (nodeKindContentIterator9.MoveNext())
					{
						XPathItem current7 = nodeKindContentIterator9.Current;
						output.WriteItem(current7);
					}
				}
				output.WriteEndElement();
				output.WriteStartElement("", "TriggerConfigurationTriggerList", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.WriteStartAttributeUnchecked("z", "Size", "http://schemas.microsoft.com/2003/10/Serialization/");
				XmlQueryOutput arg_EFE_0 = output;
				int arg_EF1_0;
				int arg_EF4_0 = arg_EF1_0 = 0;
				ElementContentIterator elementContentIterator40;
				elementContentIterator40.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(38), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
				while (elementContentIterator40.MoveNext())
				{
					ElementContentIterator elementContentIterator41;
					elementContentIterator41.Create(elementContentIterator40.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(39), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
					while (elementContentIterator41.MoveNext())
					{
						ElementContentIterator elementContentIterator42;
						elementContentIterator42.Create(elementContentIterator41.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(40), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
						while (elementContentIterator42.MoveNext())
						{
							NodeKindContentIterator nodeKindContentIterator10;
							nodeKindContentIterator10.Create(elementContentIterator42.Current, XPathNodeType.Element);
							if (XsltConvert.ToDouble((!nodeKindContentIterator10.MoveNext()) ? "" : nodeKindContentIterator10.Current.Value) == 1.0)
							{
								arg_EF4_0 = ++arg_EF1_0;
							}
						}
					}
				}
				arg_EFE_0.WriteStringUnchecked(XsltConvert.ToString(XsltConvert.ToDouble(arg_EF4_0)));
				output.WriteEndAttributeUnchecked();
				Convert_12_to_13.<xsl:template name="CopyRecordTriggersInTriggeredLoggingForMemory">({urn:schemas-microsoft-com:xslt-debug}runtime, {urn:schemas-microsoft-com:xslt-debug}current, 1.0);
				output.WriteEndElement();
				output.WriteStartElement("", "TriggerConfigurationTriggerListOnOff", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.WriteStartAttributeUnchecked("z", "Size", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked(XsltConvert.ToString(num2));
				output.WriteEndAttributeUnchecked();
				Convert_12_to_13.<xsl:template name="CopyRecordTriggersInOnOffLoggingForMemory">({urn:schemas-microsoft-com:xslt-debug}runtime, {urn:schemas-microsoft-com:xslt-debug}current, 1.0);
				output.WriteEndElement();
				output.WriteStartElement("", "TriggerConfigurationTriggerMode", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				ElementContentIterator elementContentIterator43;
				elementContentIterator43.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(42), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
				while (elementContentIterator43.MoveNext())
				{
					NodeKindContentIterator nodeKindContentIterator11;
					nodeKindContentIterator11.Create(elementContentIterator43.Current, XPathNodeType.Element);
					while (nodeKindContentIterator11.MoveNext())
					{
						XPathItem current8 = nodeKindContentIterator11.Current;
						output.WriteItem(current8);
					}
				}
				output.WriteEndElement();
				output.WriteEndElementUnchecked("", "TriggerConfiguration", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "TriggerConfiguration", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				int num8 = -1;
				XPathNavigator xPathNavigator6;
				do
				{
					num8++;
					if (num8 >= ((ICollection<XPathNavigator>)xmlQueryNodeSequence).Count)
					{
						goto IL_10B1;
					}
					xPathNavigator6 = ((IList<XPathNavigator>)xmlQueryNodeSequence)[num8];
				}
				while (!string.Equals(xPathNavigator6.Value, "Permanent"));
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "TriggerConfigurationMemoryRingBuffer", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "MemoryRingBufferIsActive", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteStringUnchecked("false");
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "MemoryRingBufferIsActive", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				XPathNavigator xPathNavigator7 = Convert_12_to_13.SyncToNavigator(xPathNavigator7, {urn:schemas-microsoft-com:xslt-debug}current);
				xPathNavigator7.MoveToRoot();
				ElementContentIterator elementContentIterator44;
				elementContentIterator44.Create(xPathNavigator7, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(32), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
				while (elementContentIterator44.MoveNext())
				{
					ElementContentIterator elementContentIterator45;
					elementContentIterator45.Create(elementContentIterator44.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(33), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
					while (elementContentIterator45.MoveNext())
					{
						ElementContentIterator elementContentIterator46;
						elementContentIterator46.Create(elementContentIterator45.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(34), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
						while (elementContentIterator46.MoveNext())
						{
							ElementContentIterator elementContentIterator47;
							elementContentIterator47.Create(elementContentIterator46.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(35), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
							while (elementContentIterator47.MoveNext())
							{
								int num9 = 0;
								ElementContentIterator elementContentIterator48;
								elementContentIterator48.Create(elementContentIterator47.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(36), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
								while (elementContentIterator48.MoveNext())
								{
									num9++;
									if (num9 > 2)
									{
										break;
									}
									if (num9 == 2)
									{
										ElementContentIterator elementContentIterator49;
										elementContentIterator49.Create(elementContentIterator48.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(43), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
										while (elementContentIterator49.MoveNext())
										{
											XPathItem current9 = elementContentIterator49.Current;
											output.WriteItem(current9);
										}
									}
								}
							}
						}
					}
				}
				XPathNavigator xPathNavigator8 = Convert_12_to_13.SyncToNavigator(xPathNavigator8, {urn:schemas-microsoft-com:xslt-debug}current);
				xPathNavigator8.MoveToRoot();
				ElementContentIterator elementContentIterator50;
				elementContentIterator50.Create(xPathNavigator8, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(32), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
				while (elementContentIterator50.MoveNext())
				{
					ElementContentIterator elementContentIterator51;
					elementContentIterator51.Create(elementContentIterator50.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(33), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
					while (elementContentIterator51.MoveNext())
					{
						ElementContentIterator elementContentIterator52;
						elementContentIterator52.Create(elementContentIterator51.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(34), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
						while (elementContentIterator52.MoveNext())
						{
							ElementContentIterator elementContentIterator53;
							elementContentIterator53.Create(elementContentIterator52.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(35), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
							while (elementContentIterator53.MoveNext())
							{
								int num10 = 0;
								ElementContentIterator elementContentIterator54;
								elementContentIterator54.Create(elementContentIterator53.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(36), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
								while (elementContentIterator54.MoveNext())
								{
									num10++;
									if (num10 > 2)
									{
										break;
									}
									if (num10 == 2)
									{
										ElementContentIterator elementContentIterator55;
										elementContentIterator55.Create(elementContentIterator54.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(44), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
										while (elementContentIterator55.MoveNext())
										{
											XPathItem current10 = elementContentIterator55.Current;
											output.WriteItem(current10);
										}
									}
								}
							}
						}
					}
				}
				output.WriteEndElementUnchecked("", "TriggerConfigurationMemoryRingBuffer", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				goto IL_189A;
				IL_10B1:
				int num11 = -1;
				XPathNavigator xPathNavigator9;
				do
				{
					num11++;
					if (num11 >= ((ICollection<XPathNavigator>)xmlQueryNodeSequence).Count)
					{
						goto IL_142A;
					}
					xPathNavigator9 = ((IList<XPathNavigator>)xmlQueryNodeSequence)[num11];
				}
				while (!string.Equals(xPathNavigator9.Value, "OnOff"));
				if (num2 > 0.0 && num3 == 0.0)
				{
					output.StartElementContentUnchecked();
					output.WriteStartElementUnchecked("", "TriggerConfigurationMemoryRingBuffer", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
					output.WriteStringUnchecked("0");
					output.WriteEndAttributeUnchecked();
					output.StartElementContentUnchecked();
					output.WriteStartElementUnchecked("", "MemoryRingBufferIsActive", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
					output.WriteStringUnchecked("0");
					output.WriteEndAttributeUnchecked();
					output.StartElementContentUnchecked();
					output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.StartElementContentUnchecked();
					output.WriteStringUnchecked("false");
					output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.WriteEndElementUnchecked("", "MemoryRingBufferIsActive", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					XPathNavigator xPathNavigator10 = Convert_12_to_13.SyncToNavigator(xPathNavigator10, {urn:schemas-microsoft-com:xslt-debug}current);
					xPathNavigator10.MoveToRoot();
					ElementContentIterator elementContentIterator56;
					elementContentIterator56.Create(xPathNavigator10, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(32), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
					while (elementContentIterator56.MoveNext())
					{
						ElementContentIterator elementContentIterator57;
						elementContentIterator57.Create(elementContentIterator56.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(33), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
						while (elementContentIterator57.MoveNext())
						{
							ElementContentIterator elementContentIterator58;
							elementContentIterator58.Create(elementContentIterator57.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(34), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
							while (elementContentIterator58.MoveNext())
							{
								ElementContentIterator elementContentIterator59;
								elementContentIterator59.Create(elementContentIterator58.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(35), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
								while (elementContentIterator59.MoveNext())
								{
									int num12 = 0;
									ElementContentIterator elementContentIterator60;
									elementContentIterator60.Create(elementContentIterator59.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(36), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
									while (elementContentIterator60.MoveNext())
									{
										num12++;
										if (num12 > 2)
										{
											break;
										}
										if (num12 == 2)
										{
											ElementContentIterator elementContentIterator61;
											elementContentIterator61.Create(elementContentIterator60.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(43), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
											while (elementContentIterator61.MoveNext())
											{
												XPathItem current11 = elementContentIterator61.Current;
												output.WriteItem(current11);
											}
										}
									}
								}
							}
						}
					}
					XPathNavigator xPathNavigator11 = Convert_12_to_13.SyncToNavigator(xPathNavigator11, {urn:schemas-microsoft-com:xslt-debug}current);
					xPathNavigator11.MoveToRoot();
					ElementContentIterator elementContentIterator62;
					elementContentIterator62.Create(xPathNavigator11, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(32), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
					while (elementContentIterator62.MoveNext())
					{
						ElementContentIterator elementContentIterator63;
						elementContentIterator63.Create(elementContentIterator62.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(33), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
						while (elementContentIterator63.MoveNext())
						{
							ElementContentIterator elementContentIterator64;
							elementContentIterator64.Create(elementContentIterator63.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(34), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
							while (elementContentIterator64.MoveNext())
							{
								ElementContentIterator elementContentIterator65;
								elementContentIterator65.Create(elementContentIterator64.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(35), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
								while (elementContentIterator65.MoveNext())
								{
									int num13 = 0;
									ElementContentIterator elementContentIterator66;
									elementContentIterator66.Create(elementContentIterator65.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(36), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
									while (elementContentIterator66.MoveNext())
									{
										num13++;
										if (num13 > 2)
										{
											break;
										}
										if (num13 == 2)
										{
											ElementContentIterator elementContentIterator67;
											elementContentIterator67.Create(elementContentIterator66.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(44), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
											while (elementContentIterator67.MoveNext())
											{
												XPathItem current12 = elementContentIterator67.Current;
												output.WriteItem(current12);
											}
										}
									}
								}
							}
						}
					}
					output.WriteEndElementUnchecked("", "TriggerConfigurationMemoryRingBuffer", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					goto IL_1576;
				}
				IL_142A:
				output.WriteStartElement("", "TriggerConfigurationMemoryRingBuffer", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				XPathNavigator xPathNavigator12 = Convert_12_to_13.SyncToNavigator(xPathNavigator12, {urn:schemas-microsoft-com:xslt-debug}current);
				xPathNavigator12.MoveToRoot();
				ElementContentIterator elementContentIterator68;
				elementContentIterator68.Create(xPathNavigator12, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(32), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
				while (elementContentIterator68.MoveNext())
				{
					ElementContentIterator elementContentIterator69;
					elementContentIterator69.Create(elementContentIterator68.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(33), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
					while (elementContentIterator69.MoveNext())
					{
						ElementContentIterator elementContentIterator70;
						elementContentIterator70.Create(elementContentIterator69.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(34), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
						while (elementContentIterator70.MoveNext())
						{
							ElementContentIterator elementContentIterator71;
							elementContentIterator71.Create(elementContentIterator70.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(35), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
							while (elementContentIterator71.MoveNext())
							{
								int num14 = 0;
								ElementContentIterator elementContentIterator72;
								elementContentIterator72.Create(elementContentIterator71.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(36), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
								while (elementContentIterator72.MoveNext())
								{
									num14++;
									if (num14 > 2)
									{
										break;
									}
									if (num14 == 2)
									{
										NodeKindContentIterator nodeKindContentIterator12;
										nodeKindContentIterator12.Create(elementContentIterator72.Current, XPathNodeType.Element);
										while (nodeKindContentIterator12.MoveNext())
										{
											XPathItem current13 = nodeKindContentIterator12.Current;
											output.WriteItem(current13);
										}
									}
								}
							}
						}
					}
				}
				output.WriteEndElement();
				IL_1576:
				IL_189A:
				output.WriteStartElement("", "TriggerConfigurationPostTriggerTime", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				ElementContentIterator elementContentIterator73;
				elementContentIterator73.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(37), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
				while (elementContentIterator73.MoveNext())
				{
					NodeKindContentIterator nodeKindContentIterator13;
					nodeKindContentIterator13.Create(elementContentIterator73.Current, XPathNodeType.Element);
					while (nodeKindContentIterator13.MoveNext())
					{
						XPathItem current14 = nodeKindContentIterator13.Current;
						output.WriteItem(current14);
					}
				}
				output.WriteEndElement();
				output.WriteStartElement("", "TriggerConfigurationTriggerList", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.WriteStartAttributeUnchecked("z", "Size", "http://schemas.microsoft.com/2003/10/Serialization/");
				XmlQueryOutput arg_1A39_0 = output;
				int arg_1A2C_0;
				int arg_1A2F_0 = arg_1A2C_0 = 0;
				ElementContentIterator elementContentIterator74;
				elementContentIterator74.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(38), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
				while (elementContentIterator74.MoveNext())
				{
					ElementContentIterator elementContentIterator75;
					elementContentIterator75.Create(elementContentIterator74.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(39), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
					while (elementContentIterator75.MoveNext())
					{
						ElementContentIterator elementContentIterator76;
						elementContentIterator76.Create(elementContentIterator75.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(40), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
						while (elementContentIterator76.MoveNext())
						{
							NodeKindContentIterator nodeKindContentIterator14;
							nodeKindContentIterator14.Create(elementContentIterator76.Current, XPathNodeType.Element);
							if (XsltConvert.ToDouble((!nodeKindContentIterator14.MoveNext()) ? "" : nodeKindContentIterator14.Current.Value) == 2.0)
							{
								arg_1A2F_0 = ++arg_1A2C_0;
							}
						}
					}
				}
				arg_1A39_0.WriteStringUnchecked(XsltConvert.ToString(XsltConvert.ToDouble(arg_1A2F_0)));
				output.WriteEndAttributeUnchecked();
				Convert_12_to_13.<xsl:template name="CopyRecordTriggersInTriggeredLoggingForMemory">({urn:schemas-microsoft-com:xslt-debug}runtime, {urn:schemas-microsoft-com:xslt-debug}current, 2.0);
				output.WriteEndElement();
				output.WriteStartElement("", "TriggerConfigurationTriggerListOnOff", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.WriteStartAttributeUnchecked("z", "Size", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked(XsltConvert.ToString(num3));
				output.WriteEndAttributeUnchecked();
				Convert_12_to_13.<xsl:template name="CopyRecordTriggersInOnOffLoggingForMemory">({urn:schemas-microsoft-com:xslt-debug}runtime, {urn:schemas-microsoft-com:xslt-debug}current, 2.0);
				output.WriteEndElement();
				output.WriteStartElement("", "TriggerConfigurationTriggerMode", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				ElementContentIterator elementContentIterator77;
				elementContentIterator77.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(42), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
				while (elementContentIterator77.MoveNext())
				{
					NodeKindContentIterator nodeKindContentIterator15;
					nodeKindContentIterator15.Create(elementContentIterator77.Current, XPathNodeType.Element);
					while (nodeKindContentIterator15.MoveNext())
					{
						XPathItem current15 = nodeKindContentIterator15.Current;
						output.WriteItem(current15);
					}
				}
				output.WriteEndElement();
				output.WriteEndElementUnchecked("", "TriggerConfiguration", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElement();
			}
		}

		private static void <xsl:template name="CopyRecordTriggersInTriggeredLoggingForMemory">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current, double MemoryNr)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			ElementContentIterator elementContentIterator;
			elementContentIterator.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(38), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
			while (elementContentIterator.MoveNext())
			{
				ElementContentIterator elementContentIterator2;
				elementContentIterator2.Create(elementContentIterator.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(39), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
				IL_47:
				while (elementContentIterator2.MoveNext())
				{
					ElementContentIterator elementContentIterator3;
					elementContentIterator3.Create(elementContentIterator2.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(40), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
					string arg_B6_0;
					while (elementContentIterator3.MoveNext())
					{
						ElementContentIterator elementContentIterator4;
						elementContentIterator4.Create(elementContentIterator3.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(18), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
						if (elementContentIterator4.MoveNext())
						{
							arg_B6_0 = elementContentIterator4.Current.Value;
							IL_B6:
							if (MemoryNr == XsltConvert.ToDouble(arg_B6_0))
							{
								output.WriteStartElement("", "RecordTrigger", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
								output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
								output.WriteNamespaceDeclarationUnchecked("i", "http://www.w3.org/2001/XMLSchema-instance");
								output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
								output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
								output.WriteStringUnchecked("0");
								output.WriteEndAttributeUnchecked();
								ElementContentIterator elementContentIterator5;
								elementContentIterator5.Create(elementContentIterator2.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(45), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
								while (elementContentIterator5.MoveNext())
								{
									XPathItem current = elementContentIterator5.Current;
									output.WriteItem(current);
								}
								ElementContentIterator elementContentIterator6;
								elementContentIterator6.Create(elementContentIterator2.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(46), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
								while (elementContentIterator6.MoveNext())
								{
									XPathItem current2 = elementContentIterator6.Current;
									output.WriteItem(current2);
								}
								ElementContentIterator elementContentIterator7;
								elementContentIterator7.Create(elementContentIterator2.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(47), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
								while (elementContentIterator7.MoveNext())
								{
									XPathItem current3 = elementContentIterator7.Current;
									output.WriteItem(current3);
								}
								ElementContentIterator elementContentIterator8;
								elementContentIterator8.Create(elementContentIterator2.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(48), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
								while (elementContentIterator8.MoveNext())
								{
									XPathNavigator xPathNavigator = Convert_12_to_13.SyncToNavigator(xPathNavigator, elementContentIterator8.Current);
									if (xPathNavigator.MoveToAttribute({urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(6), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(7)) && string.Equals(xPathNavigator.Value, "DigitalInputEvent"))
									{
										output.WriteStartElement("", "RecordTriggerEvent", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
										output.WriteStartAttributeUnchecked("z", "id", "http://schemas.microsoft.com/2003/10/Serialization/");
										output.WriteStringUnchecked("0");
										output.WriteEndAttributeUnchecked();
										output.WriteStartAttributeUnchecked("i", "type", "http://www.w3.org/2001/XMLSchema-instance");
										output.WriteStringUnchecked("DigitalInputEvent");
										output.WriteEndAttributeUnchecked();
										output.StartElementContentUnchecked();
										output.WriteStartElementUnchecked("", "DigitalInputDebounceTime", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
										output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
										output.WriteStringUnchecked("0");
										output.WriteEndAttributeUnchecked();
										output.StartElementContentUnchecked();
										output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
										output.StartElementContentUnchecked();
										output.WriteStringUnchecked("50");
										output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
										output.WriteEndElementUnchecked("", "DigitalInputDebounceTime", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
										ElementContentIterator elementContentIterator9;
										elementContentIterator9.Create(elementContentIterator2.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(48), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
										while (elementContentIterator9.MoveNext())
										{
											ElementContentIterator elementContentIterator10;
											elementContentIterator10.Create(elementContentIterator9.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(8), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
											while (elementContentIterator10.MoveNext())
											{
												XPathItem current4 = elementContentIterator10.Current;
												output.WriteItem(current4);
											}
										}
										ElementContentIterator elementContentIterator11;
										elementContentIterator11.Create(elementContentIterator2.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(48), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
										while (elementContentIterator11.MoveNext())
										{
											ElementContentIterator elementContentIterator12;
											elementContentIterator12.Create(elementContentIterator11.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(9), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
											while (elementContentIterator12.MoveNext())
											{
												XPathItem current5 = elementContentIterator12.Current;
												output.WriteItem(current5);
											}
										}
										output.WriteStartElementUnchecked("", "DigitalInputIsDebounceActive", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
										output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
										output.WriteStringUnchecked("0");
										output.WriteEndAttributeUnchecked();
										output.StartElementContentUnchecked();
										output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
										output.StartElementContentUnchecked();
										output.WriteStringUnchecked("true");
										output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
										output.WriteEndElementUnchecked("", "DigitalInputIsDebounceActive", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
										output.WriteEndElement();
										IL_4D7:
										ElementContentIterator elementContentIterator13;
										elementContentIterator13.Create(elementContentIterator2.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(49), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
										while (elementContentIterator13.MoveNext())
										{
											XPathItem current6 = elementContentIterator13.Current;
											output.WriteItem(current6);
										}
										output.WriteEndElement();
										goto IL_47;
									}
								}
								ElementContentIterator elementContentIterator14;
								elementContentIterator14.Create(elementContentIterator2.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(48), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
								while (elementContentIterator14.MoveNext())
								{
									XPathItem current7 = elementContentIterator14.Current;
									output.WriteItem(current7);
								}
								goto IL_4D7;
							}
							goto IL_47;
						}
					}
					arg_B6_0 = "";
					goto IL_B6;
				}
			}
		}

		private static void <xsl:template name="CopyRecordTriggersInOnOffLoggingForMemory">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current, double MemoryNr)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			ElementContentIterator elementContentIterator;
			elementContentIterator.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(41), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
			while (elementContentIterator.MoveNext())
			{
				ElementContentIterator elementContentIterator2;
				elementContentIterator2.Create(elementContentIterator.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(39), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
				IL_47:
				while (elementContentIterator2.MoveNext())
				{
					ElementContentIterator elementContentIterator3;
					elementContentIterator3.Create(elementContentIterator2.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(40), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
					string arg_B6_0;
					while (elementContentIterator3.MoveNext())
					{
						ElementContentIterator elementContentIterator4;
						elementContentIterator4.Create(elementContentIterator3.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(18), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
						if (elementContentIterator4.MoveNext())
						{
							arg_B6_0 = elementContentIterator4.Current.Value;
							IL_B6:
							if (MemoryNr == XsltConvert.ToDouble(arg_B6_0))
							{
								output.WriteStartElement("", "RecordTrigger", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
								output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
								output.WriteNamespaceDeclarationUnchecked("i", "http://www.w3.org/2001/XMLSchema-instance");
								output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
								output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
								output.WriteStringUnchecked("0");
								output.WriteEndAttributeUnchecked();
								ElementContentIterator elementContentIterator5;
								elementContentIterator5.Create(elementContentIterator2.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(45), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
								while (elementContentIterator5.MoveNext())
								{
									XPathItem current = elementContentIterator5.Current;
									output.WriteItem(current);
								}
								ElementContentIterator elementContentIterator6;
								elementContentIterator6.Create(elementContentIterator2.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(46), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
								while (elementContentIterator6.MoveNext())
								{
									XPathItem current2 = elementContentIterator6.Current;
									output.WriteItem(current2);
								}
								ElementContentIterator elementContentIterator7;
								elementContentIterator7.Create(elementContentIterator2.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(47), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
								while (elementContentIterator7.MoveNext())
								{
									XPathItem current3 = elementContentIterator7.Current;
									output.WriteItem(current3);
								}
								ElementContentIterator elementContentIterator8;
								elementContentIterator8.Create(elementContentIterator2.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(48), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
								while (elementContentIterator8.MoveNext())
								{
									XPathNavigator xPathNavigator = Convert_12_to_13.SyncToNavigator(xPathNavigator, elementContentIterator8.Current);
									if (xPathNavigator.MoveToAttribute({urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(6), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(7)) && string.Equals(xPathNavigator.Value, "DigitalInputEvent"))
									{
										output.WriteStartElement("", "RecordTriggerEvent", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
										output.WriteStartAttributeUnchecked("z", "id", "http://schemas.microsoft.com/2003/10/Serialization/");
										output.WriteStringUnchecked("0");
										output.WriteEndAttributeUnchecked();
										output.WriteStartAttributeUnchecked("i", "type", "http://www.w3.org/2001/XMLSchema-instance");
										output.WriteStringUnchecked("DigitalInputEvent");
										output.WriteEndAttributeUnchecked();
										output.StartElementContentUnchecked();
										output.WriteStartElementUnchecked("", "DigitalInputDebounceTime", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
										output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
										output.WriteStringUnchecked("0");
										output.WriteEndAttributeUnchecked();
										output.StartElementContentUnchecked();
										output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
										output.StartElementContentUnchecked();
										output.WriteStringUnchecked("50");
										output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
										output.WriteEndElementUnchecked("", "DigitalInputDebounceTime", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
										ElementContentIterator elementContentIterator9;
										elementContentIterator9.Create(elementContentIterator2.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(48), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
										while (elementContentIterator9.MoveNext())
										{
											ElementContentIterator elementContentIterator10;
											elementContentIterator10.Create(elementContentIterator9.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(8), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
											while (elementContentIterator10.MoveNext())
											{
												XPathItem current4 = elementContentIterator10.Current;
												output.WriteItem(current4);
											}
										}
										ElementContentIterator elementContentIterator11;
										elementContentIterator11.Create(elementContentIterator2.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(48), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
										while (elementContentIterator11.MoveNext())
										{
											ElementContentIterator elementContentIterator12;
											elementContentIterator12.Create(elementContentIterator11.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(9), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
											while (elementContentIterator12.MoveNext())
											{
												XPathItem current5 = elementContentIterator12.Current;
												output.WriteItem(current5);
											}
										}
										output.WriteStartElementUnchecked("", "DigitalInputIsDebounceActive", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
										output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
										output.WriteStringUnchecked("0");
										output.WriteEndAttributeUnchecked();
										output.StartElementContentUnchecked();
										output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
										output.StartElementContentUnchecked();
										output.WriteStringUnchecked("true");
										output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
										output.WriteEndElementUnchecked("", "DigitalInputIsDebounceActive", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
										output.WriteEndElement();
										IL_4D7:
										ElementContentIterator elementContentIterator13;
										elementContentIterator13.Create(elementContentIterator2.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(49), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
										while (elementContentIterator13.MoveNext())
										{
											XPathItem current6 = elementContentIterator13.Current;
											output.WriteItem(current6);
										}
										output.WriteEndElement();
										goto IL_47;
									}
								}
								ElementContentIterator elementContentIterator14;
								elementContentIterator14.Create(elementContentIterator2.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(48), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
								while (elementContentIterator14.MoveNext())
								{
									XPathItem current7 = elementContentIterator14.Current;
									output.WriteItem(current7);
								}
								goto IL_4D7;
							}
							goto IL_47;
						}
					}
					arg_B6_0 = "";
					goto IL_B6;
				}
			}
		}

		private static void <xsl:template match="VLConfig:LoggingConfigurationFilterConfiguration" priority="1">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current, string indent)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			if (!string.Equals(Convert_12_to_13.LoggerTypeName({urn:schemas-microsoft-com:xslt-debug}runtime), "GL1000") && !string.Equals(Convert_12_to_13.LoggerTypeName({urn:schemas-microsoft-com:xslt-debug}runtime), "GL2000"))
			{
				output.WriteStartElement("", "LoggingConfigurationFilterConfigList", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteNamespaceDeclarationUnchecked("i", "http://www.w3.org/2001/XMLSchema-instance");
				output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.WriteStartAttributeUnchecked("z", "Size", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("2");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "FilterConfiguration", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				int arg_3D8_0;
				int arg_3DB_0 = arg_3D8_0 = 0;
				ElementContentIterator elementContentIterator;
				elementContentIterator.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(50), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
				while (elementContentIterator.MoveNext())
				{
					ElementContentIterator elementContentIterator2;
					elementContentIterator2.Create(elementContentIterator.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(51), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
					while (elementContentIterator2.MoveNext())
					{
						ElementContentIterator elementContentIterator3;
						elementContentIterator3.Create(elementContentIterator2.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(52), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
						while (elementContentIterator3.MoveNext())
						{
							NodeKindContentIterator nodeKindContentIterator;
							nodeKindContentIterator.Create(elementContentIterator3.Current, XPathNodeType.Element);
							if (XsltConvert.ToDouble((!nodeKindContentIterator.MoveNext()) ? "" : nodeKindContentIterator.Current.Value) == 1.0)
							{
								arg_3DB_0 = ++arg_3D8_0;
							}
						}
					}
				}
				double num = XsltConvert.ToDouble(arg_3DB_0);
				XPathNavigator xPathNavigator = Convert_12_to_13.SyncToNavigator(xPathNavigator, {urn:schemas-microsoft-com:xslt-debug}current);
				xPathNavigator.MoveToRoot();
				ElementContentIterator elementContentIterator4;
				elementContentIterator4.Create(xPathNavigator, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(32), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
				while (elementContentIterator4.MoveNext())
				{
					ElementContentIterator elementContentIterator5;
					elementContentIterator5.Create(elementContentIterator4.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(33), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
					while (elementContentIterator5.MoveNext())
					{
						ElementContentIterator elementContentIterator6;
						elementContentIterator6.Create(elementContentIterator5.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(34), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
						while (elementContentIterator6.MoveNext())
						{
							ElementContentIterator elementContentIterator7;
							elementContentIterator7.Create(elementContentIterator6.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(35), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
							while (elementContentIterator7.MoveNext())
							{
								int num2 = 0;
								ElementContentIterator elementContentIterator8;
								elementContentIterator8.Create(elementContentIterator7.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(36), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
								while (elementContentIterator8.MoveNext())
								{
									num2++;
									if (num2 > 1)
									{
										break;
									}
									if (num2 == 1)
									{
										ElementContentIterator elementContentIterator9;
										elementContentIterator9.Create(elementContentIterator8.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(53), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
										while (elementContentIterator9.MoveNext())
										{
											ElementContentIterator elementContentIterator10;
											elementContentIterator10.Create(elementContentIterator9.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(18), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
											while (elementContentIterator10.MoveNext())
											{
												if (string.Equals(elementContentIterator10.Current.Value, "false"))
												{
													output.WriteStartElement("", "FilterConfigurationFilterList", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
													output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
													output.WriteStringUnchecked("0");
													output.WriteEndAttributeUnchecked();
													output.WriteStartAttributeUnchecked("z", "Size", "http://schemas.microsoft.com/2003/10/Serialization/");
													output.WriteStringUnchecked(XsltConvert.ToString(num + 1.0));
													output.WriteEndAttributeUnchecked();
													output.StartElementContentUnchecked();
													output.WriteStartElementUnchecked("", "Filter", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
													output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
													output.WriteStringUnchecked("0");
													output.WriteEndAttributeUnchecked();
													output.WriteStartAttributeUnchecked("i", "type", "http://www.w3.org/2001/XMLSchema-instance");
													output.WriteStringUnchecked("DefaultFilter");
													output.WriteEndAttributeUnchecked();
													output.StartElementContentUnchecked();
													output.WriteStartElementUnchecked("", "FilterAction", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
													output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
													output.WriteStringUnchecked("0");
													output.WriteEndAttributeUnchecked();
													output.StartElementContentUnchecked();
													output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
													output.StartElementContentUnchecked();
													output.WriteStringUnchecked("Pass");
													output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
													output.WriteEndElementUnchecked("", "FilterAction", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
													output.WriteStartElementUnchecked("", "FilterIsActive", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
													output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
													output.WriteStringUnchecked("0");
													output.WriteEndAttributeUnchecked();
													output.StartElementContentUnchecked();
													output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
													output.StartElementContentUnchecked();
													output.WriteStringUnchecked("true");
													output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
													output.WriteEndElementUnchecked("", "FilterIsActive", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
													output.WriteStartElementUnchecked("", "FilterLimitIntervalPerFrame", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
													output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
													output.WriteStringUnchecked("0");
													output.WriteEndAttributeUnchecked();
													output.StartElementContentUnchecked();
													output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
													output.StartElementContentUnchecked();
													output.WriteStringUnchecked("1");
													output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
													output.WriteEndElementUnchecked("", "FilterLimitIntervalPerFrame", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
													output.WriteEndElementUnchecked("", "Filter", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
													Convert_12_to_13.<xsl:template name="CopyFilterForMemory">({urn:schemas-microsoft-com:xslt-debug}runtime, {urn:schemas-microsoft-com:xslt-debug}current, 1.0);
													output.WriteEndElement();
													goto IL_861;
												}
											}
										}
									}
								}
							}
						}
					}
					continue;
					IL_861:
					output.WriteEndElementUnchecked("", "FilterConfiguration", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.WriteStartElementUnchecked("", "FilterConfiguration", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
					output.WriteStringUnchecked("0");
					output.WriteEndAttributeUnchecked();
					int arg_963_0;
					int arg_966_0 = arg_963_0 = 0;
					ElementContentIterator elementContentIterator11;
					elementContentIterator11.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(50), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
					while (elementContentIterator11.MoveNext())
					{
						ElementContentIterator elementContentIterator12;
						elementContentIterator12.Create(elementContentIterator11.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(51), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
						while (elementContentIterator12.MoveNext())
						{
							ElementContentIterator elementContentIterator13;
							elementContentIterator13.Create(elementContentIterator12.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(52), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
							while (elementContentIterator13.MoveNext())
							{
								NodeKindContentIterator nodeKindContentIterator2;
								nodeKindContentIterator2.Create(elementContentIterator13.Current, XPathNodeType.Element);
								if (XsltConvert.ToDouble((!nodeKindContentIterator2.MoveNext()) ? "" : nodeKindContentIterator2.Current.Value) == 2.0)
								{
									arg_966_0 = ++arg_963_0;
								}
							}
						}
					}
					double num3 = XsltConvert.ToDouble(arg_966_0);
					XPathNavigator xPathNavigator2 = Convert_12_to_13.SyncToNavigator(xPathNavigator2, {urn:schemas-microsoft-com:xslt-debug}current);
					xPathNavigator2.MoveToRoot();
					ElementContentIterator elementContentIterator14;
					elementContentIterator14.Create(xPathNavigator2, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(32), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
					while (elementContentIterator14.MoveNext())
					{
						ElementContentIterator elementContentIterator15;
						elementContentIterator15.Create(elementContentIterator14.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(33), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
						while (elementContentIterator15.MoveNext())
						{
							ElementContentIterator elementContentIterator16;
							elementContentIterator16.Create(elementContentIterator15.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(34), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
							while (elementContentIterator16.MoveNext())
							{
								ElementContentIterator elementContentIterator17;
								elementContentIterator17.Create(elementContentIterator16.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(35), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
								while (elementContentIterator17.MoveNext())
								{
									int num4 = 0;
									ElementContentIterator elementContentIterator18;
									elementContentIterator18.Create(elementContentIterator17.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(36), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
									while (elementContentIterator18.MoveNext())
									{
										num4++;
										if (num4 > 2)
										{
											break;
										}
										if (num4 == 2)
										{
											ElementContentIterator elementContentIterator19;
											elementContentIterator19.Create(elementContentIterator18.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(53), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
											while (elementContentIterator19.MoveNext())
											{
												ElementContentIterator elementContentIterator20;
												elementContentIterator20.Create(elementContentIterator19.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(18), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
												while (elementContentIterator20.MoveNext())
												{
													if (string.Equals(elementContentIterator20.Current.Value, "false"))
													{
														output.WriteStartElement("", "FilterConfigurationFilterList", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
														output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
														output.WriteStringUnchecked("0");
														output.WriteEndAttributeUnchecked();
														output.WriteStartAttributeUnchecked("z", "Size", "http://schemas.microsoft.com/2003/10/Serialization/");
														output.WriteStringUnchecked(XsltConvert.ToString(num3 + 1.0));
														output.WriteEndAttributeUnchecked();
														output.StartElementContentUnchecked();
														output.WriteStartElementUnchecked("", "Filter", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
														output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
														output.WriteStringUnchecked("0");
														output.WriteEndAttributeUnchecked();
														output.WriteStartAttributeUnchecked("i", "type", "http://www.w3.org/2001/XMLSchema-instance");
														output.WriteStringUnchecked("DefaultFilter");
														output.WriteEndAttributeUnchecked();
														output.StartElementContentUnchecked();
														output.WriteStartElementUnchecked("", "FilterAction", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
														output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
														output.WriteStringUnchecked("0");
														output.WriteEndAttributeUnchecked();
														output.StartElementContentUnchecked();
														output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
														output.StartElementContentUnchecked();
														output.WriteStringUnchecked("Pass");
														output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
														output.WriteEndElementUnchecked("", "FilterAction", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
														output.WriteStartElementUnchecked("", "FilterIsActive", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
														output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
														output.WriteStringUnchecked("0");
														output.WriteEndAttributeUnchecked();
														output.StartElementContentUnchecked();
														output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
														output.StartElementContentUnchecked();
														output.WriteStringUnchecked("true");
														output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
														output.WriteEndElementUnchecked("", "FilterIsActive", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
														output.WriteStartElementUnchecked("", "FilterLimitIntervalPerFrame", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
														output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
														output.WriteStringUnchecked("0");
														output.WriteEndAttributeUnchecked();
														output.StartElementContentUnchecked();
														output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
														output.StartElementContentUnchecked();
														output.WriteStringUnchecked("1");
														output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
														output.WriteEndElementUnchecked("", "FilterLimitIntervalPerFrame", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
														output.WriteEndElementUnchecked("", "Filter", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
														Convert_12_to_13.<xsl:template name="CopyFilterForMemory">({urn:schemas-microsoft-com:xslt-debug}runtime, {urn:schemas-microsoft-com:xslt-debug}current, 2.0);
														output.WriteEndElement();
														goto IL_DEC;
													}
												}
											}
										}
									}
								}
							}
						}
						continue;
						IL_DEC:
						output.WriteEndElementUnchecked("", "FilterConfiguration", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteEndElement();
						return;
					}
					output.WriteStartElement("", "FilterConfigurationFilterList", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
					output.WriteStringUnchecked("0");
					output.WriteEndAttributeUnchecked();
					output.WriteStartAttributeUnchecked("z", "Size", "http://schemas.microsoft.com/2003/10/Serialization/");
					output.WriteStringUnchecked(XsltConvert.ToString(num3));
					output.WriteEndAttributeUnchecked();
					Convert_12_to_13.<xsl:template name="CopyFilterForMemory">({urn:schemas-microsoft-com:xslt-debug}runtime, {urn:schemas-microsoft-com:xslt-debug}current, 2.0);
					output.WriteEndElement();
					goto IL_DEC;
				}
				output.WriteStartElement("", "FilterConfigurationFilterList", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.WriteStartAttributeUnchecked("z", "Size", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked(XsltConvert.ToString(num));
				output.WriteEndAttributeUnchecked();
				Convert_12_to_13.<xsl:template name="CopyFilterForMemory">({urn:schemas-microsoft-com:xslt-debug}runtime, {urn:schemas-microsoft-com:xslt-debug}current, 1.0);
				output.WriteEndElement();
				goto IL_861;
			}
			output.WriteStartElement("", "LoggingConfigurationFilterConfigList", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteNamespaceDeclarationUnchecked("i", "http://www.w3.org/2001/XMLSchema-instance");
			output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.WriteStartAttributeUnchecked("z", "Size", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("1");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("", "FilterConfiguration", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.WriteStartElement("", "FilterConfigurationFilterList", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.WriteStartAttributeUnchecked("z", "Size", "http://schemas.microsoft.com/2003/10/Serialization/");
			XmlQueryOutput arg_213_0 = output;
			int arg_206_0;
			int arg_209_0 = arg_206_0 = 0;
			ElementContentIterator elementContentIterator21;
			elementContentIterator21.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(50), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
			while (elementContentIterator21.MoveNext())
			{
				ElementContentIterator elementContentIterator22;
				elementContentIterator22.Create(elementContentIterator21.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(51), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
				while (elementContentIterator22.MoveNext())
				{
					ElementContentIterator elementContentIterator23;
					elementContentIterator23.Create(elementContentIterator22.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(52), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
					while (elementContentIterator23.MoveNext())
					{
						NodeKindContentIterator nodeKindContentIterator3;
						nodeKindContentIterator3.Create(elementContentIterator23.Current, XPathNodeType.Element);
						if (XsltConvert.ToDouble((!nodeKindContentIterator3.MoveNext()) ? "" : nodeKindContentIterator3.Current.Value) == 1.0)
						{
							arg_209_0 = ++arg_206_0;
						}
					}
				}
			}
			arg_213_0.WriteStringUnchecked(XsltConvert.ToString(XsltConvert.ToDouble(arg_209_0)));
			output.WriteEndAttributeUnchecked();
			Convert_12_to_13.<xsl:template name="CopyFilterForMemory">({urn:schemas-microsoft-com:xslt-debug}runtime, {urn:schemas-microsoft-com:xslt-debug}current, 1.0);
			output.WriteEndElement();
			output.WriteEndElementUnchecked("", "FilterConfiguration", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteEndElement();
		}

		private static void <xsl:template name="CopyFilterForMemory">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current, double MemoryNr)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			ElementContentIterator elementContentIterator;
			elementContentIterator.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(50), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
			while (elementContentIterator.MoveNext())
			{
				ElementContentIterator elementContentIterator2;
				elementContentIterator2.Create(elementContentIterator.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(51), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
				IL_47:
				while (elementContentIterator2.MoveNext())
				{
					ElementContentIterator elementContentIterator3;
					elementContentIterator3.Create(elementContentIterator2.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(52), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
					string arg_B6_0;
					while (elementContentIterator3.MoveNext())
					{
						ElementContentIterator elementContentIterator4;
						elementContentIterator4.Create(elementContentIterator3.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(18), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
						if (elementContentIterator4.MoveNext())
						{
							arg_B6_0 = elementContentIterator4.Current.Value;
							IL_B6:
							if (MemoryNr == XsltConvert.ToDouble(arg_B6_0))
							{
								XmlQueryNodeSequence xmlQueryNodeSequence = XmlQueryNodeSequence.CreateOrReuse(xmlQueryNodeSequence);
								XmlQueryNodeSequence arg_F4_0 = xmlQueryNodeSequence;
								XPathNavigator xPathNavigator = Convert_12_to_13.SyncToNavigator(xPathNavigator, elementContentIterator2.Current);
								if (xPathNavigator.MoveToAttribute({urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(6), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(7)))
								{
									arg_F4_0.AddClone(xPathNavigator);
								}
								output.WriteStartElement("", "Filter", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
								output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
								output.WriteNamespaceDeclarationUnchecked("i", "http://www.w3.org/2001/XMLSchema-instance");
								output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
								output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
								output.WriteStringUnchecked("0");
								output.WriteEndAttributeUnchecked();
								output.WriteStartAttributeUnchecked("i", "type", "http://www.w3.org/2001/XMLSchema-instance");
								XmlQueryOutput arg_1AC_0 = output;
								int num = -1;
								num++;
								arg_1AC_0.WriteStringUnchecked((num >= ((ICollection<XPathNavigator>)xmlQueryNodeSequence).Count) ? "" : ((IList<XPathNavigator>)xmlQueryNodeSequence)[num].Value);
								output.WriteEndAttributeUnchecked();
								output.StartElementContentUnchecked();
								output.WriteStartElementUnchecked("", "FilterAction", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
								output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
								output.WriteStringUnchecked("0");
								output.WriteEndAttributeUnchecked();
								ElementContentIterator elementContentIterator5;
								elementContentIterator5.Create(elementContentIterator2.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(54), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
								while (elementContentIterator5.MoveNext())
								{
									ElementContentIterator elementContentIterator6;
									elementContentIterator6.Create(elementContentIterator5.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(18), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
									while (elementContentIterator6.MoveNext())
									{
										if (string.Equals(elementContentIterator6.Current.Value, "true"))
										{
											output.StartElementContentUnchecked();
											output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
											output.StartElementContentUnchecked();
											output.WriteStringUnchecked("Stop");
											output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
											goto IL_2EB;
										}
									}
									continue;
									IL_2EB:
									output.WriteEndElementUnchecked("", "FilterAction", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
									ElementContentIterator elementContentIterator7;
									elementContentIterator7.Create(elementContentIterator2.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(55), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
									while (elementContentIterator7.MoveNext())
									{
										XPathItem current = elementContentIterator7.Current;
										output.WriteItem(current);
									}
									output.WriteStartElementUnchecked("", "FilterLimitIntervalPerFrame", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
									output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
									output.WriteStringUnchecked("0");
									output.WriteEndAttributeUnchecked();
									output.StartElementContentUnchecked();
									output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
									output.StartElementContentUnchecked();
									output.WriteStringUnchecked("1");
									output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
									output.WriteEndElementUnchecked("", "FilterLimitIntervalPerFrame", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
									int num2 = -1;
									while (true)
									{
										num2++;
										if (num2 >= ((ICollection<XPathNavigator>)xmlQueryNodeSequence).Count)
										{
											goto IL_407;
										}
										XPathNavigator xPathNavigator2 = ((IList<XPathNavigator>)xmlQueryNodeSequence)[num2];
										if (string.Equals(xPathNavigator2.Value, "ChannelFilter"))
										{
											goto Block_13;
										}
									}
									IL_AA8:
									output.WriteEndElement();
									goto IL_47;
									Block_13:
									ElementContentIterator elementContentIterator8;
									elementContentIterator8.Create(elementContentIterator2.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(73), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
									while (elementContentIterator8.MoveNext())
									{
										XPathItem current2 = elementContentIterator8.Current;
										output.WriteItem(current2);
									}
									ElementContentIterator elementContentIterator9;
									elementContentIterator9.Create(elementContentIterator2.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(74), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
									while (elementContentIterator9.MoveNext())
									{
										XPathItem current3 = elementContentIterator9.Current;
										output.WriteItem(current3);
									}
									goto IL_AA8;
									IL_407:
									int num3 = -1;
									while (true)
									{
										num3++;
										if (num3 >= ((ICollection<XPathNavigator>)xmlQueryNodeSequence).Count)
										{
											goto IL_441;
										}
										XPathNavigator xPathNavigator3 = ((IList<XPathNavigator>)xmlQueryNodeSequence)[num3];
										if (string.Equals(xPathNavigator3.Value, "SymbolicMessageFilter"))
										{
											goto Block_15;
										}
									}
									IL_A2B:
									goto IL_AA8;
									Block_15:
									ElementContentIterator elementContentIterator10;
									elementContentIterator10.Create(elementContentIterator2.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(67), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
									while (elementContentIterator10.MoveNext())
									{
										XPathItem current4 = elementContentIterator10.Current;
										output.WriteItem(current4);
									}
									ElementContentIterator elementContentIterator11;
									elementContentIterator11.Create(elementContentIterator2.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(57), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
									while (elementContentIterator11.MoveNext())
									{
										XPathItem current5 = elementContentIterator11.Current;
										output.WriteItem(current5);
									}
									ElementContentIterator elementContentIterator12;
									elementContentIterator12.Create(elementContentIterator2.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(68), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
									while (elementContentIterator12.MoveNext())
									{
										XPathItem current6 = elementContentIterator12.Current;
										output.WriteItem(current6);
									}
									ElementContentIterator elementContentIterator13;
									elementContentIterator13.Create(elementContentIterator2.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(69), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
									while (elementContentIterator13.MoveNext())
									{
										XPathItem current7 = elementContentIterator13.Current;
										output.WriteItem(current7);
									}
									ElementContentIterator elementContentIterator14;
									elementContentIterator14.Create(elementContentIterator2.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(70), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
									while (elementContentIterator14.MoveNext())
									{
										XPathItem current8 = elementContentIterator14.Current;
										output.WriteItem(current8);
									}
									ElementContentIterator elementContentIterator15;
									elementContentIterator15.Create(elementContentIterator2.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(71), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
									while (elementContentIterator15.MoveNext())
									{
										XPathItem current9 = elementContentIterator15.Current;
										output.WriteItem(current9);
									}
									ElementContentIterator elementContentIterator16;
									elementContentIterator16.Create(elementContentIterator2.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(72), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
									while (elementContentIterator16.MoveNext())
									{
										XPathItem current10 = elementContentIterator16.Current;
										output.WriteItem(current10);
									}
									goto IL_A2B;
									IL_441:
									int num4 = -1;
									while (true)
									{
										num4++;
										if (num4 >= ((ICollection<XPathNavigator>)xmlQueryNodeSequence).Count)
										{
											goto IL_47B;
										}
										XPathNavigator xPathNavigator4 = ((IList<XPathNavigator>)xmlQueryNodeSequence)[num4];
										if (string.Equals(xPathNavigator4.Value, "CANIdFilter"))
										{
											goto Block_17;
										}
									}
									IL_882:
									goto IL_A2B;
									Block_17:
									ElementContentIterator elementContentIterator17;
									elementContentIterator17.Create(elementContentIterator2.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(64), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
									while (elementContentIterator17.MoveNext())
									{
										XPathItem current11 = elementContentIterator17.Current;
										output.WriteItem(current11);
									}
									ElementContentIterator elementContentIterator18;
									elementContentIterator18.Create(elementContentIterator2.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(65), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
									while (elementContentIterator18.MoveNext())
									{
										XPathItem current12 = elementContentIterator18.Current;
										output.WriteItem(current12);
									}
									ElementContentIterator elementContentIterator19;
									elementContentIterator19.Create(elementContentIterator2.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(57), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
									while (elementContentIterator19.MoveNext())
									{
										XPathItem current13 = elementContentIterator19.Current;
										output.WriteItem(current13);
									}
									ElementContentIterator elementContentIterator20;
									elementContentIterator20.Create(elementContentIterator2.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(66), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
									while (elementContentIterator20.MoveNext())
									{
										XPathItem current14 = elementContentIterator20.Current;
										output.WriteItem(current14);
									}
									ElementContentIterator elementContentIterator21;
									elementContentIterator21.Create(elementContentIterator2.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(61), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
									while (elementContentIterator21.MoveNext())
									{
										XPathItem current15 = elementContentIterator21.Current;
										output.WriteItem(current15);
									}
									goto IL_882;
									IL_47B:
									int num5 = -1;
									while (true)
									{
										num5++;
										if (num5 >= ((ICollection<XPathNavigator>)xmlQueryNodeSequence).Count)
										{
											goto IL_4B5;
										}
										XPathNavigator xPathNavigator5 = ((IList<XPathNavigator>)xmlQueryNodeSequence)[num5];
										if (string.Equals(xPathNavigator5.Value, "LINIdFilter"))
										{
											goto Block_19;
										}
									}
									IL_751:
									goto IL_882;
									Block_19:
									ElementContentIterator elementContentIterator22;
									elementContentIterator22.Create(elementContentIterator2.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(57), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
									while (elementContentIterator22.MoveNext())
									{
										XPathItem current16 = elementContentIterator22.Current;
										output.WriteItem(current16);
									}
									ElementContentIterator elementContentIterator23;
									elementContentIterator23.Create(elementContentIterator2.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(61), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
									while (elementContentIterator23.MoveNext())
									{
										XPathItem current17 = elementContentIterator23.Current;
										output.WriteItem(current17);
									}
									ElementContentIterator elementContentIterator24;
									elementContentIterator24.Create(elementContentIterator2.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(62), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
									while (elementContentIterator24.MoveNext())
									{
										XPathItem current18 = elementContentIterator24.Current;
										output.WriteItem(current18);
									}
									ElementContentIterator elementContentIterator25;
									elementContentIterator25.Create(elementContentIterator2.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(63), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
									while (elementContentIterator25.MoveNext())
									{
										XPathItem current19 = elementContentIterator25.Current;
										output.WriteItem(current19);
									}
									goto IL_751;
									IL_4B5:
									int num6 = -1;
									while (true)
									{
										num6++;
										if (num6 >= ((ICollection<XPathNavigator>)xmlQueryNodeSequence).Count)
										{
											break;
										}
										XPathNavigator xPathNavigator6 = ((IList<XPathNavigator>)xmlQueryNodeSequence)[num6];
										if (string.Equals(xPathNavigator6.Value, "FlexrayIdFilter"))
										{
											goto Block_21;
										}
									}
									IL_65C:
									goto IL_751;
									Block_21:
									ElementContentIterator elementContentIterator26;
									elementContentIterator26.Create(elementContentIterator2.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(56), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
									while (elementContentIterator26.MoveNext())
									{
										XPathItem current20 = elementContentIterator26.Current;
										output.WriteItem(current20);
									}
									ElementContentIterator elementContentIterator27;
									elementContentIterator27.Create(elementContentIterator2.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(57), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
									while (elementContentIterator27.MoveNext())
									{
										XPathItem current21 = elementContentIterator27.Current;
										output.WriteItem(current21);
									}
									ElementContentIterator elementContentIterator28;
									elementContentIterator28.Create(elementContentIterator2.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(58), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
									while (elementContentIterator28.MoveNext())
									{
										XPathItem current22 = elementContentIterator28.Current;
										output.WriteItem(current22);
									}
									ElementContentIterator elementContentIterator29;
									elementContentIterator29.Create(elementContentIterator2.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(59), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
									while (elementContentIterator29.MoveNext())
									{
										XPathItem current23 = elementContentIterator29.Current;
										output.WriteItem(current23);
									}
									ElementContentIterator elementContentIterator30;
									elementContentIterator30.Create(elementContentIterator2.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(60), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
									while (elementContentIterator30.MoveNext())
									{
										XPathItem current24 = elementContentIterator30.Current;
										output.WriteItem(current24);
									}
									ElementContentIterator elementContentIterator31;
									elementContentIterator31.Create(elementContentIterator2.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(61), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
									while (elementContentIterator31.MoveNext())
									{
										XPathItem current25 = elementContentIterator31.Current;
										output.WriteItem(current25);
									}
									goto IL_65C;
								}
								output.StartElementContentUnchecked();
								output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
								output.StartElementContentUnchecked();
								output.WriteStringUnchecked("Pass");
								output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
								goto IL_2EB;
							}
							goto IL_47;
						}
					}
					arg_B6_0 = "";
					goto IL_B6;
				}
			}
		}

		private static void <xsl:template match="VLConfig:DiagnosticCommParamsECUDefaultSessionSource" priority="1">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current, string indent)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			Convert_12_to_13.<xsl:template name="PrettyPrintNodeRecursive">({urn:schemas-microsoft-com:xslt-debug}runtime, {urn:schemas-microsoft-com:xslt-debug}current, indent);
			output.WriteStartElement("", "DiagnosticCommParamsECUDiagAddressingMode", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteNamespaceDeclarationUnchecked("i", "http://www.w3.org/2001/XMLSchema-instance");
			output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("Normal");
			output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteEndElement();
			output.WriteStartElement("", "DiagnosticCommParamsECUDiagExtAddressingModeRqExt", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteNamespaceDeclarationUnchecked("i", "http://www.w3.org/2001/XMLSchema-instance");
			output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("0");
			output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteEndElement();
			output.WriteStartElement("", "DiagnosticCommParamsECUDiagExtAddressingModeRsExt", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteNamespaceDeclarationUnchecked("i", "http://www.w3.org/2001/XMLSchema-instance");
			output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("0");
			output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteEndElement();
		}

		private unsafe static void <xsl:template match="VLConfig:ConfigurationDataModelLoggingConfiguration" priority="1">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current, string indent)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			Convert_12_to_13.<xsl:template name="PrettyPrintNodeRecursive">({urn:schemas-microsoft-com:xslt-debug}runtime, {urn:schemas-microsoft-com:xslt-debug}current, indent);
			output.WriteStartElement("", "ConfigurationDataModelOutputConfiguration", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteNamespaceDeclarationUnchecked("i", "http://www.w3.org/2001/XMLSchema-instance");
			output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.WriteStartElement("", "OutputConfigurationDigitalOutputsConfiguration", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			if (string.Equals(Convert_12_to_13.LoggerTypeName({urn:schemas-microsoft-com:xslt-debug}runtime), "GL1000"))
			{
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "DigitalOutputsConfigurationActionList", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.WriteStartAttributeUnchecked("z", "Size", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("2");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "ActionDigitalOutput", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "ActionComment", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "ActionComment", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "ActionEvent", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.WriteStartAttributeUnchecked("i", "type", "http://www.w3.org/2001/XMLSchema-instance");
				output.WriteStringUnchecked("OnStartEvent");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "OnStartEventDelay", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteStringUnchecked("0");
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "OnStartEventDelay", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "ActionEvent", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "ActionIsActive", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteStringUnchecked("false");
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "ActionIsActive", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "ActionStopType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("i", "nil", "http://www.w3.org/2001/XMLSchema-instance");
				output.WriteStringUnchecked("true");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteEndElementUnchecked("", "ActionStopType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "ActionDigitalOutput", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "ActionDigitalOutput", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "ActionComment", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "ActionComment", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "ActionEvent", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.WriteStartAttributeUnchecked("i", "type", "http://www.w3.org/2001/XMLSchema-instance");
				output.WriteStringUnchecked("OnStartEvent");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "OnStartEventDelay", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteStringUnchecked("0");
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "OnStartEventDelay", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "ActionEvent", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "ActionIsActive", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteStringUnchecked("false");
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "ActionIsActive", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "ActionStopType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("i", "nil", "http://www.w3.org/2001/XMLSchema-instance");
				output.WriteStringUnchecked("true");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteEndElementUnchecked("", "ActionStopType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "ActionDigitalOutput", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "DigitalOutputsConfigurationActionList", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			}
			else if (string.Equals(Convert_12_to_13.LoggerTypeName({urn:schemas-microsoft-com:xslt-debug}runtime), "GL2000"))
			{
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "DigitalOutputsConfigurationActionList", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.WriteStartAttributeUnchecked("z", "Size", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("4");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "ActionDigitalOutput", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "ActionComment", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "ActionComment", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "ActionEvent", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.WriteStartAttributeUnchecked("i", "type", "http://www.w3.org/2001/XMLSchema-instance");
				output.WriteStringUnchecked("OnStartEvent");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "OnStartEventDelay", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteStringUnchecked("0");
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "OnStartEventDelay", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "ActionEvent", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "ActionIsActive", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteStringUnchecked("false");
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "ActionIsActive", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "ActionStopType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("i", "nil", "http://www.w3.org/2001/XMLSchema-instance");
				output.WriteStringUnchecked("true");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteEndElementUnchecked("", "ActionStopType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "ActionDigitalOutput", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "ActionDigitalOutput", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "ActionComment", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "ActionComment", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "ActionEvent", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.WriteStartAttributeUnchecked("i", "type", "http://www.w3.org/2001/XMLSchema-instance");
				output.WriteStringUnchecked("OnStartEvent");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "OnStartEventDelay", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteStringUnchecked("0");
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "OnStartEventDelay", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "ActionEvent", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "ActionIsActive", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteStringUnchecked("false");
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "ActionIsActive", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "ActionStopType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("i", "nil", "http://www.w3.org/2001/XMLSchema-instance");
				output.WriteStringUnchecked("true");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteEndElementUnchecked("", "ActionStopType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "ActionDigitalOutput", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "ActionDigitalOutput", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "ActionComment", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "ActionComment", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "ActionEvent", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.WriteStartAttributeUnchecked("i", "type", "http://www.w3.org/2001/XMLSchema-instance");
				output.WriteStringUnchecked("OnStartEvent");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "OnStartEventDelay", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteStringUnchecked("0");
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "OnStartEventDelay", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "ActionEvent", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "ActionIsActive", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteStringUnchecked("false");
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "ActionIsActive", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "ActionStopType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("i", "nil", "http://www.w3.org/2001/XMLSchema-instance");
				output.WriteStringUnchecked("true");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteEndElementUnchecked("", "ActionStopType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "ActionDigitalOutput", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "ActionDigitalOutput", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "ActionComment", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "ActionComment", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "ActionEvent", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.WriteStartAttributeUnchecked("i", "type", "http://www.w3.org/2001/XMLSchema-instance");
				output.WriteStringUnchecked("OnStartEvent");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "OnStartEventDelay", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteStringUnchecked("0");
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "OnStartEventDelay", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "ActionEvent", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "ActionIsActive", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteStringUnchecked("false");
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "ActionIsActive", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "ActionStopType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("i", "nil", "http://www.w3.org/2001/XMLSchema-instance");
				output.WriteStringUnchecked("true");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteEndElementUnchecked("", "ActionStopType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "ActionDigitalOutput", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "DigitalOutputsConfigurationActionList", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			}
			else if (string.Equals(Convert_12_to_13.LoggerTypeName({urn:schemas-microsoft-com:xslt-debug}runtime), "GL3000") || string.Equals(Convert_12_to_13.LoggerTypeName({urn:schemas-microsoft-com:xslt-debug}runtime), "GL4000"))
			{
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "DigitalOutputsConfigurationActionList", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.WriteStartAttributeUnchecked("z", "Size", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("8");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "ActionDigitalOutput", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "ActionComment", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "ActionComment", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "ActionEvent", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.WriteStartAttributeUnchecked("i", "type", "http://www.w3.org/2001/XMLSchema-instance");
				output.WriteStringUnchecked("OnStartEvent");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "OnStartEventDelay", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteStringUnchecked("0");
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "OnStartEventDelay", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "ActionEvent", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "ActionIsActive", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteStringUnchecked("false");
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "ActionIsActive", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "ActionStopType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("i", "nil", "http://www.w3.org/2001/XMLSchema-instance");
				output.WriteStringUnchecked("true");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteEndElementUnchecked("", "ActionStopType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "ActionDigitalOutput", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "ActionDigitalOutput", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "ActionComment", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "ActionComment", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "ActionEvent", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.WriteStartAttributeUnchecked("i", "type", "http://www.w3.org/2001/XMLSchema-instance");
				output.WriteStringUnchecked("OnStartEvent");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "OnStartEventDelay", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteStringUnchecked("0");
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "OnStartEventDelay", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "ActionEvent", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "ActionIsActive", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteStringUnchecked("false");
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "ActionIsActive", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "ActionStopType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("i", "nil", "http://www.w3.org/2001/XMLSchema-instance");
				output.WriteStringUnchecked("true");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteEndElementUnchecked("", "ActionStopType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "ActionDigitalOutput", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "ActionDigitalOutput", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "ActionComment", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "ActionComment", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "ActionEvent", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.WriteStartAttributeUnchecked("i", "type", "http://www.w3.org/2001/XMLSchema-instance");
				output.WriteStringUnchecked("OnStartEvent");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "OnStartEventDelay", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteStringUnchecked("0");
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "OnStartEventDelay", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "ActionEvent", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "ActionIsActive", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteStringUnchecked("false");
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "ActionIsActive", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "ActionStopType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("i", "nil", "http://www.w3.org/2001/XMLSchema-instance");
				output.WriteStringUnchecked("true");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteEndElementUnchecked("", "ActionStopType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "ActionDigitalOutput", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "ActionDigitalOutput", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "ActionComment", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "ActionComment", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "ActionEvent", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.WriteStartAttributeUnchecked("i", "type", "http://www.w3.org/2001/XMLSchema-instance");
				output.WriteStringUnchecked("OnStartEvent");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "OnStartEventDelay", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteStringUnchecked("0");
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "OnStartEventDelay", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "ActionEvent", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "ActionIsActive", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteStringUnchecked("false");
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "ActionIsActive", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "ActionStopType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("i", "nil", "http://www.w3.org/2001/XMLSchema-instance");
				output.WriteStringUnchecked("true");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteEndElementUnchecked("", "ActionStopType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "ActionDigitalOutput", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "ActionDigitalOutput", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "ActionComment", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "ActionComment", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "ActionEvent", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.WriteStartAttributeUnchecked("i", "type", "http://www.w3.org/2001/XMLSchema-instance");
				output.WriteStringUnchecked("OnStartEvent");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "OnStartEventDelay", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteStringUnchecked("0");
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "OnStartEventDelay", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "ActionEvent", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "ActionIsActive", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteStringUnchecked("false");
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "ActionIsActive", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "ActionStopType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("i", "nil", "http://www.w3.org/2001/XMLSchema-instance");
				output.WriteStringUnchecked("true");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteEndElementUnchecked("", "ActionStopType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "ActionDigitalOutput", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "ActionDigitalOutput", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "ActionComment", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "ActionComment", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "ActionEvent", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.WriteStartAttributeUnchecked("i", "type", "http://www.w3.org/2001/XMLSchema-instance");
				output.WriteStringUnchecked("OnStartEvent");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "OnStartEventDelay", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteStringUnchecked("0");
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "OnStartEventDelay", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "ActionEvent", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "ActionIsActive", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteStringUnchecked("false");
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "ActionIsActive", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "ActionStopType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("i", "nil", "http://www.w3.org/2001/XMLSchema-instance");
				output.WriteStringUnchecked("true");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteEndElementUnchecked("", "ActionStopType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "ActionDigitalOutput", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "ActionDigitalOutput", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "ActionComment", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "ActionComment", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "ActionEvent", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.WriteStartAttributeUnchecked("i", "type", "http://www.w3.org/2001/XMLSchema-instance");
				output.WriteStringUnchecked("OnStartEvent");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "OnStartEventDelay", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteStringUnchecked("0");
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "OnStartEventDelay", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "ActionEvent", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "ActionIsActive", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteStringUnchecked("false");
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "ActionIsActive", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "ActionStopType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("i", "nil", "http://www.w3.org/2001/XMLSchema-instance");
				output.WriteStringUnchecked("true");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteEndElementUnchecked("", "ActionStopType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "ActionDigitalOutput", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "ActionDigitalOutput", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "ActionComment", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "ActionComment", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "ActionEvent", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.WriteStartAttributeUnchecked("i", "type", "http://www.w3.org/2001/XMLSchema-instance");
				output.WriteStringUnchecked("OnStartEvent");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "OnStartEventDelay", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteStringUnchecked("0");
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "OnStartEventDelay", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "ActionEvent", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "ActionIsActive", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteStringUnchecked("false");
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "ActionIsActive", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "ActionStopType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("i", "nil", "http://www.w3.org/2001/XMLSchema-instance");
				output.WriteStringUnchecked("true");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteEndElementUnchecked("", "ActionStopType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "ActionDigitalOutput", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "DigitalOutputsConfigurationActionList", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			}
			output.WriteEndElement();
			output.WriteStartElement("", "OutputConfigurationLEDConfiguration", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			ContentMergeIterator contentMergeIterator;
			contentMergeIterator.Create({urn:schemas-microsoft-com:xslt-debug}runtime.GetTypeFilter(XPathNodeType.Element));
			XPathNavigator xPathNavigator = Convert_12_to_13.SyncToNavigator(xPathNavigator, {urn:schemas-microsoft-com:xslt-debug}current);
			xPathNavigator.MoveToRoot();
			DescendantIterator descendantIterator;
			descendantIterator.Create(xPathNavigator, {urn:schemas-microsoft-com:xslt-debug}runtime.GetNameFilter(16), false);
			while (true)
			{
				if (!descendantIterator.MoveNext())
				{
					goto IL_2A81;
				}
				ContentMergeIterator* arg_2A84_0 = ref contentMergeIterator;
				XPathNavigator arg_2A84_1 = descendantIterator.Current;
				IL_2A84:
				switch (arg_2A84_0.MoveNext(arg_2A84_1))
				{
				case IteratorResult.NoMoreNodes:
					goto IL_2AA9;
				case IteratorResult.NeedInputNode:
					continue;
				default:
				{
					XPathItem current = contentMergeIterator.Current;
					output.WriteItem(current);
					break;
				}
				}
				IL_2A81:
				arg_2A84_0 = ref contentMergeIterator;
				arg_2A84_1 = null;
				goto IL_2A84;
			}
			IL_2AA9:
			output.WriteEndElement();
			output.WriteStartElementUnchecked("", "OutputConfigurationSendMessageConfiguration", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("", "SendMessageConfigurationActionList", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.WriteStartAttributeUnchecked("z", "Size", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteEndElementUnchecked("", "SendMessageConfigurationActionList", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteEndElementUnchecked("", "OutputConfigurationSendMessageConfiguration", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteEndElement();
		}

		private static void <xsl:template match="VLConfig:HardwareConfigurationFlexrayChannelConfiguration" priority="1">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current, string indent)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			Convert_12_to_13.<xsl:template name="PrettyPrintNodeRecursive">({urn:schemas-microsoft-com:xslt-debug}runtime, {urn:schemas-microsoft-com:xslt-debug}current, indent);
			output.WriteStartElement("", "HardwareConfigurationGPSConfiguration", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteNamespaceDeclarationUnchecked("i", "http://www.w3.org/2001/XMLSchema-instance");
			output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("", "GPSConfigurationCANChannel", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			if (string.Equals(Convert_12_to_13.LoggerTypeName({urn:schemas-microsoft-com:xslt-debug}runtime), "GL3000") || string.Equals(Convert_12_to_13.LoggerTypeName({urn:schemas-microsoft-com:xslt-debug}runtime), "GL4000"))
			{
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteStringUnchecked("10");
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			}
			else
			{
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteStringUnchecked("6");
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			}
			output.WriteEndElementUnchecked("", "GPSConfigurationCANChannel", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartElementUnchecked("", "GPSConfigurationIsActive", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("false");
			output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteEndElementUnchecked("", "GPSConfigurationIsActive", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartElementUnchecked("", "GPSConfigurationIsExtendedStartCANId", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("true");
			output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteEndElementUnchecked("", "GPSConfigurationIsExtendedStartCANId", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartElementUnchecked("", "GPSConfigurationStartCANId", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("536870880");
			output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteEndElementUnchecked("", "GPSConfigurationStartCANId", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteEndElement();
		}

		private static void <xsl:template name="PrettyPrintNodeRecursive">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current, string indent)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			Convert_12_to_13.<xsl:template name="newline">({urn:schemas-microsoft-com:xslt-debug}runtime);
			if (indent.Length > 0)
			{
				output.WriteString(indent);
			}
			NodeKindContentIterator nodeKindContentIterator;
			nodeKindContentIterator.Create({urn:schemas-microsoft-com:xslt-debug}current, XPathNodeType.Element);
			if (nodeKindContentIterator.MoveNext())
			{
				if (output.StartCopy({urn:schemas-microsoft-com:xslt-debug}current))
				{
					AttributeIterator attributeIterator;
					attributeIterator.Create({urn:schemas-microsoft-com:xslt-debug}current);
					while (attributeIterator.MoveNext())
					{
						XPathItem current = attributeIterator.Current;
						output.WriteItem(current);
					}
					UnionIterator unionIterator;
					unionIterator.Create({urn:schemas-microsoft-com:xslt-debug}runtime);
					UnionIterator unionIterator2;
					unionIterator2.Create({urn:schemas-microsoft-com:xslt-debug}runtime);
					NodeKindContentIterator nodeKindContentIterator2;
					nodeKindContentIterator2.Create({urn:schemas-microsoft-com:xslt-debug}current, XPathNodeType.Element);
					while (true)
					{
						if (!nodeKindContentIterator2.MoveNext())
						{
							goto IL_CF;
						}
						XPathNavigator nestedNavigator = nodeKindContentIterator2.Current;
						if (!true)
						{
							goto IL_AB;
						}
						IL_D2:
						XPathNavigator nestedNavigator2;
						NodeKindContentIterator nodeKindContentIterator3;
						switch (unionIterator2.MoveNext(nestedNavigator))
						{
						case SetIteratorResult.NoMoreNodes:
							IL_123:
							nestedNavigator2 = null;
							break;
						case SetIteratorResult.InitRightIterator:
							IL_AB:
							nodeKindContentIterator3.Create({urn:schemas-microsoft-com:xslt-debug}current, XPathNodeType.Text);
							goto IL_B4;
						case SetIteratorResult.NeedLeftNode:
							continue;
						case SetIteratorResult.NeedRightNode:
							goto IL_B4;
						default:
							nestedNavigator2 = unionIterator2.Current;
							if (!true)
							{
								goto IL_FF;
							}
							break;
						}
						IL_126:
						NodeKindContentIterator nodeKindContentIterator4;
						switch (unionIterator.MoveNext(nestedNavigator2))
						{
						case SetIteratorResult.NoMoreNodes:
							goto IL_164;
						case SetIteratorResult.InitRightIterator:
							IL_FF:
							nodeKindContentIterator4.Create({urn:schemas-microsoft-com:xslt-debug}current, XPathNodeType.Comment);
							break;
						case SetIteratorResult.NeedLeftNode:
							IL_CF:
							nestedNavigator = null;
							goto IL_D2;
						case SetIteratorResult.NeedRightNode:
							break;
						default:
							Convert_12_to_13.<xsl:apply-templates>({urn:schemas-microsoft-com:xslt-debug}runtime, unionIterator.Current, indent + XsltConvert.ToString(Convert_12_to_13.indent-increment({urn:schemas-microsoft-com:xslt-debug}runtime)));
							goto IL_123;
						}
						if (!nodeKindContentIterator4.MoveNext())
						{
							goto IL_123;
						}
						nestedNavigator2 = nodeKindContentIterator4.Current;
						if (!true)
						{
							goto IL_123;
						}
						goto IL_126;
						IL_B4:
						if (!nodeKindContentIterator3.MoveNext())
						{
							goto IL_CF;
						}
						nestedNavigator = nodeKindContentIterator3.Current;
						if (!true)
						{
							goto IL_CF;
						}
						goto IL_D2;
					}
					IL_164:
					Convert_12_to_13.<xsl:template name="newline">({urn:schemas-microsoft-com:xslt-debug}runtime);
					output.WriteString(indent);
					output.EndCopy({urn:schemas-microsoft-com:xslt-debug}current);
				}
			}
			else
			{
				output.XsltCopyOf({urn:schemas-microsoft-com:xslt-debug}current);
			}
		}

		private static void 5">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current, string indent)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			Convert_12_to_13.<xsl:template name="PrettyPrintNodeRecursive">({urn:schemas-microsoft-com:xslt-debug}runtime, {urn:schemas-microsoft-com:xslt-debug}current, indent);
		}

		private static void <xsl:apply-templates>(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator xPathNavigator, string indent)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			int arg_17C_0;
			if (xPathNavigator.NodeType == XPathNodeType.Element)
			{
				int arg_10D_0;
				if ({urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 76, 1))
				{
					arg_10D_0 = 13;
				}
				else if ({urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 75, 1))
				{
					AncestorIterator ancestorIterator;
					ancestorIterator.Create(xPathNavigator, {urn:schemas-microsoft-com:xslt-debug}runtime.GetTypeFilter(XPathNodeType.All), false);
					while (ancestorIterator.MoveNext())
					{
						if (ancestorIterator.Current.NodeType == XPathNodeType.Root)
						{
							arg_10D_0 = 12;
							goto IL_10D;
						}
					}
					arg_10D_0 = -1;
				}
				else
				{
					arg_10D_0 = ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 77, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 78, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 79, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 80, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 81, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 34, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 82, 1)) ? -1 : 5) : 6) : 7) : 8) : 9) : 10) : 11);
				}
				IL_10D:
				int num = arg_10D_0;
				arg_17C_0 = ((num <= 4) ? 4 : num);
			}
			else
			{
				arg_17C_0 = (((1 << (int)xPathNavigator.NodeType & 112) == 0) ? ((xPathNavigator.NodeType != XPathNodeType.Comment) ? ((xPathNavigator.NodeType != XPathNodeType.ProcessingInstruction) ? -1 : 0) : 1) : ((!string.Equals(XsltFunctions.NormalizeSpace(xPathNavigator.Value), "")) ? 2 : 3));
			}
			switch (arg_17C_0)
			{
			case 0:
				Convert_12_to_13.<xsl:template match="comment() | processing-instruction()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, indent);
				return;
			case 1:
				Convert_12_to_13.<xsl:template match="comment() | processing-instruction()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, indent);
				return;
			case 2:
				Convert_12_to_13.<xsl:template match="text()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, indent);
				return;
			case 3:
				break;
			case 4:
				Convert_12_to_13.<xsl:template match="*" priority="0.5">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, indent);
				return;
			case 5:
				Convert_12_to_13.<xsl:template match="VLConfig:FileFormatVersion" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime);
				return;
			case 6:
				Convert_12_to_13.<xsl:template match="VLConfig:HardwareConfigurationLogDataStorage" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			case 7:
				Convert_12_to_13.<xsl:template match="VLConfig:LoggingConfigurationDiagnosticActionsConfiguration" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			case 8:
				Convert_12_to_13.<xsl:template match="VLConfig:LoggingConfigurationTriggerConfiguration" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, indent);
				return;
			case 9:
				Convert_12_to_13.<xsl:template match="VLConfig:LoggingConfigurationFilterConfiguration" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, indent);
				return;
			case 10:
				Convert_12_to_13.<xsl:template match="VLConfig:DiagnosticCommParamsECUDefaultSessionSource" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, indent);
				return;
			case 11:
				Convert_12_to_13.<xsl:template match="VLConfig:ConfigurationDataModelLoggingConfiguration" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, indent);
				return;
			case 12:
				break;
			case 13:
				Convert_12_to_13.<xsl:template match="VLConfig:HardwareConfigurationFlexrayChannelConfiguration" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, indent);
				return;
			default:
				if ((1 << (int)xPathNavigator.NodeType & 508) == 0)
				{
					ContentIterator contentIterator;
					contentIterator.Create(xPathNavigator);
					while (contentIterator.MoveNext())
					{
						Convert_12_to_13.<xsl:apply-templates> (2)({urn:schemas-microsoft-com:xslt-debug}runtime, contentIterator.Current);
					}
				}
				else if ((1 << (int)xPathNavigator.NodeType & 395) == 0)
				{
					output.WriteString(xPathNavigator.Value);
				}
				break;
			}
		}

		private static void <xsl:apply-templates> (2)(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator xPathNavigator)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			int arg_17C_0;
			if (xPathNavigator.NodeType == XPathNodeType.Element)
			{
				int arg_10D_0;
				if ({urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 76, 1))
				{
					arg_10D_0 = 13;
				}
				else if ({urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 75, 1))
				{
					AncestorIterator ancestorIterator;
					ancestorIterator.Create(xPathNavigator, {urn:schemas-microsoft-com:xslt-debug}runtime.GetTypeFilter(XPathNodeType.All), false);
					while (ancestorIterator.MoveNext())
					{
						if (ancestorIterator.Current.NodeType == XPathNodeType.Root)
						{
							arg_10D_0 = 12;
							goto IL_10D;
						}
					}
					arg_10D_0 = -1;
				}
				else
				{
					arg_10D_0 = ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 77, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 78, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 79, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 80, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 81, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 34, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 82, 1)) ? -1 : 5) : 6) : 7) : 8) : 9) : 10) : 11);
				}
				IL_10D:
				int num = arg_10D_0;
				arg_17C_0 = ((num <= 4) ? 4 : num);
			}
			else
			{
				arg_17C_0 = (((1 << (int)xPathNavigator.NodeType & 112) == 0) ? ((xPathNavigator.NodeType != XPathNodeType.Comment) ? ((xPathNavigator.NodeType != XPathNodeType.ProcessingInstruction) ? -1 : 0) : 1) : ((!string.Equals(XsltFunctions.NormalizeSpace(xPathNavigator.Value), "")) ? 2 : 3));
			}
			switch (arg_17C_0)
			{
			case 0:
				Convert_12_to_13.<xsl:template match="comment() | processing-instruction()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, "");
				return;
			case 1:
				Convert_12_to_13.<xsl:template match="comment() | processing-instruction()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, "");
				return;
			case 2:
				Convert_12_to_13.<xsl:template match="text()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, "");
				return;
			case 3:
				break;
			case 4:
				Convert_12_to_13.<xsl:template match="*" priority="0.5">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, "");
				return;
			case 5:
				Convert_12_to_13.<xsl:template match="VLConfig:FileFormatVersion" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime);
				return;
			case 6:
				Convert_12_to_13.<xsl:template match="VLConfig:HardwareConfigurationLogDataStorage" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			case 7:
				Convert_12_to_13.<xsl:template match="VLConfig:LoggingConfigurationDiagnosticActionsConfiguration" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			case 8:
				Convert_12_to_13.<xsl:template match="VLConfig:LoggingConfigurationTriggerConfiguration" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, "");
				return;
			case 9:
				Convert_12_to_13.<xsl:template match="VLConfig:LoggingConfigurationFilterConfiguration" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, "");
				return;
			case 10:
				Convert_12_to_13.<xsl:template match="VLConfig:DiagnosticCommParamsECUDefaultSessionSource" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, "");
				return;
			case 11:
				Convert_12_to_13.<xsl:template match="VLConfig:ConfigurationDataModelLoggingConfiguration" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, "");
				return;
			case 12:
				break;
			case 13:
				Convert_12_to_13.<xsl:template match="VLConfig:HardwareConfigurationFlexrayChannelConfiguration" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, "");
				return;
			default:
				if ((1 << (int)xPathNavigator.NodeType & 508) == 0)
				{
					ContentIterator contentIterator;
					contentIterator.Create(xPathNavigator);
					while (contentIterator.MoveNext())
					{
						Convert_12_to_13.<xsl:apply-templates> (2)({urn:schemas-microsoft-com:xslt-debug}runtime, contentIterator.Current);
					}
				}
				else if ((1 << (int)xPathNavigator.NodeType & 395) == 0)
				{
					output.WriteString(xPathNavigator.Value);
				}
				break;
			}
		}

		private static string LoggerTypeName(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime)
		{
			if (!{urn:schemas-microsoft-com:xslt-debug}runtime.IsGlobalComputed(1))
			{
				int arg_66_1 = 1;
				XPathNavigator xPathNavigator;
				XPathNavigator arg_2A_0 = xPathNavigator;
				XPathNavigator xPathNavigator2 = Convert_12_to_13.SyncToNavigator(xPathNavigator2, {urn:schemas-microsoft-com:xslt-debug}runtime.ExternalContext.DefaultDataSource);
				xPathNavigator2.MoveToRoot();
				xPathNavigator = Convert_12_to_13.SyncToNavigator(arg_2A_0, xPathNavigator2);
				xPathNavigator.MoveToRoot();
				DescendantIterator descendantIterator;
				descendantIterator.Create(xPathNavigator, {urn:schemas-microsoft-com:xslt-debug}runtime.GetNameFilter(0), false);
				{urn:schemas-microsoft-com:xslt-debug}runtime.SetGlobalValue(arg_66_1, (!descendantIterator.MoveNext()) ? "" : descendantIterator.Current.Value);
			}
			return (string){urn:schemas-microsoft-com:xslt-debug}runtime.GetGlobalValue(1);
		}

		private static IList<XPathItem> indent-increment(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime)
		{
			if (!{urn:schemas-microsoft-com:xslt-debug}runtime.IsGlobalComputed(0))
			{
				object parameter = {urn:schemas-microsoft-com:xslt-debug}runtime.ExternalContext.GetParameter("indent-increment", "");
				if (parameter != null)
				{
					{urn:schemas-microsoft-com:xslt-debug}runtime.SetGlobalValue(0, {urn:schemas-microsoft-com:xslt-debug}runtime.ChangeTypeXsltResult(0, parameter));
				}
				else
				{
					int arg_52_1 = 0;
					XmlQueryItemSequence xmlQueryItemSequence = XmlQueryItemSequence.CreateOrReuse(xmlQueryItemSequence, XmlILStorageConverter.StringToAtomicValue("  ", 1, {urn:schemas-microsoft-com:xslt-debug}runtime));
					{urn:schemas-microsoft-com:xslt-debug}runtime.SetGlobalValue(arg_52_1, xmlQueryItemSequence);
				}
			}
			return (IList<XPathItem>){urn:schemas-microsoft-com:xslt-debug}runtime.GetGlobalValue(0);
		}
	}
}
