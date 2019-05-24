using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Xml.XPath;
using System.Xml.Xsl.Runtime;

namespace Vector.VLConfig.Data.ConfigurationPersistency.ConfigurationReader.VersionConverter.XSLT
{
	[GeneratedCode("System.Xml.Xsl.XslCompiledTransform", "2.0.0.0"), ComVisible(true)]
	public static class Convert_8_to_9
	{
		private static $ArrayType$1880 __staticData;

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
			67,
			0,
			0,
			0,
			3,
			82,
			101,
			102,
			51,
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
			109,
			105,
			99,
			114,
			111,
			115,
			111,
			102,
			116,
			46,
			99,
			111,
			109,
			47,
			50,
			48,
			48,
			51,
			47,
			49,
			48,
			47,
			83,
			101,
			114,
			105,
			97,
			108,
			105,
			122,
			97,
			116,
			105,
			111,
			110,
			47,
			5,
			86,
			97,
			108,
			117,
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
			2,
			73,
			100,
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
			13,
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
			14,
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
			13,
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
			22,
			73,
			100,
			84,
			114,
			105,
			103,
			103,
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
			15,
			73,
			100,
			84,
			114,
			105,
			103,
			103,
			101,
			114,
			72,
			105,
			103,
			104,
			73,
			100,
			19,
			73,
			100,
			84,
			114,
			105,
			103,
			103,
			101,
			114,
			73,
			100,
			82,
			101,
			108,
			97,
			116,
			105,
			111,
			110,
			14,
			73,
			100,
			84,
			114,
			105,
			103,
			103,
			101,
			114,
			76,
			111,
			119,
			73,
			100,
			24,
			67,
			65,
			78,
			73,
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
			16,
			84,
			114,
			105,
			103,
			103,
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
			22,
			84,
			114,
			105,
			103,
			103,
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
			27,
			67,
			65,
			78,
			68,
			97,
			116,
			97,
			84,
			114,
			105,
			103,
			103,
			101,
			114,
			82,
			97,
			119,
			68,
			97,
			116,
			97,
			83,
			105,
			103,
			110,
			97,
			108,
			27,
			67,
			65,
			78,
			68,
			97,
			116,
			97,
			84,
			114,
			105,
			103,
			103,
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
			23,
			67,
			65,
			78,
			68,
			97,
			116,
			97,
			84,
			114,
			105,
			103,
			103,
			101,
			114,
			72,
			105,
			103,
			104,
			86,
			97,
			108,
			117,
			101,
			16,
			67,
			65,
			78,
			68,
			97,
			116,
			97,
			84,
			114,
			105,
			103,
			103,
			101,
			114,
			73,
			68,
			26,
			67,
			65,
			78,
			68,
			97,
			116,
			97,
			84,
			114,
			105,
			103,
			103,
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
			22,
			67,
			65,
			78,
			68,
			97,
			116,
			97,
			84,
			114,
			105,
			103,
			103,
			101,
			114,
			76,
			111,
			119,
			86,
			97,
			108,
			117,
			101,
			11,
			68,
			97,
			116,
			97,
			66,
			121,
			116,
			101,
			80,
			111,
			115,
			10,
			73,
			115,
			77,
			111,
			116,
			111,
			114,
			111,
			108,
			97,
			6,
			76,
			101,
			110,
			103,
			116,
			104,
			11,
			83,
			116,
			97,
			114,
			116,
			98,
			105,
			116,
			80,
			111,
			115,
			22,
			67,
			65,
			78,
			68,
			97,
			116,
			97,
			84,
			114,
			105,
			103,
			103,
			101,
			114,
			82,
			101,
			108,
			97,
			116,
			105,
			111,
			110,
			27,
			76,
			73,
			78,
			68,
			97,
			116,
			97,
			84,
			114,
			105,
			103,
			103,
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
			23,
			76,
			73,
			78,
			68,
			97,
			116,
			97,
			84,
			114,
			105,
			103,
			103,
			101,
			114,
			72,
			105,
			103,
			104,
			86,
			97,
			108,
			117,
			101,
			16,
			76,
			73,
			78,
			68,
			97,
			116,
			97,
			84,
			114,
			105,
			103,
			103,
			101,
			114,
			73,
			68,
			22,
			76,
			73,
			78,
			68,
			97,
			116,
			97,
			84,
			114,
			105,
			103,
			103,
			101,
			114,
			76,
			111,
			119,
			86,
			97,
			108,
			117,
			101,
			22,
			76,
			73,
			78,
			68,
			97,
			116,
			97,
			84,
			114,
			105,
			103,
			103,
			101,
			114,
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
			77,
			101,
			115,
			115,
			97,
			103,
			101,
			84,
			114,
			105,
			103,
			103,
			101,
			114,
			66,
			117,
			115,
			84,
			121,
			112,
			101,
			35,
			83,
			121,
			109,
			98,
			111,
			108,
			105,
			99,
			77,
			101,
			115,
			115,
			97,
			103,
			101,
			84,
			114,
			105,
			103,
			103,
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
			34,
			83,
			121,
			109,
			98,
			111,
			108,
			105,
			99,
			77,
			101,
			115,
			115,
			97,
			103,
			101,
			84,
			114,
			105,
			103,
			103,
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
			34,
			83,
			121,
			109,
			98,
			111,
			108,
			105,
			99,
			77,
			101,
			115,
			115,
			97,
			103,
			101,
			84,
			114,
			105,
			103,
			103,
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
			34,
			83,
			121,
			109,
			98,
			111,
			108,
			105,
			99,
			77,
			101,
			115,
			115,
			97,
			103,
			101,
			84,
			114,
			105,
			103,
			103,
			101,
			114,
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
			33,
			83,
			121,
			109,
			98,
			111,
			108,
			105,
			99,
			77,
			101,
			115,
			115,
			97,
			103,
			101,
			84,
			114,
			105,
			103,
			103,
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
			84,
			114,
			105,
			103,
			103,
			101,
			114,
			66,
			117,
			115,
			84,
			121,
			112,
			101,
			34,
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
			84,
			114,
			105,
			103,
			103,
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
			33,
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
			84,
			114,
			105,
			103,
			103,
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
			33,
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
			84,
			114,
			105,
			103,
			103,
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
			84,
			114,
			105,
			103,
			103,
			101,
			114,
			72,
			105,
			103,
			104,
			86,
			97,
			108,
			117,
			101,
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
			84,
			114,
			105,
			103,
			103,
			101,
			114,
			76,
			111,
			119,
			86,
			97,
			108,
			117,
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
			84,
			114,
			105,
			103,
			103,
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
			84,
			114,
			105,
			103,
			103,
			101,
			114,
			82,
			101,
			108,
			97,
			116,
			105,
			111,
			110,
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
			84,
			114,
			105,
			103,
			103,
			101,
			114,
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
			34,
			67,
			65,
			78,
			66,
			117,
			115,
			83,
			116,
			97,
			116,
			105,
			115,
			116,
			105,
			99,
			115,
			84,
			114,
			105,
			103,
			103,
			101,
			114,
			66,
			117,
			115,
			108,
			111,
			97,
			100,
			72,
			105,
			103,
			104,
			33,
			67,
			65,
			78,
			66,
			117,
			115,
			83,
			116,
			97,
			116,
			105,
			115,
			116,
			105,
			99,
			115,
			84,
			114,
			105,
			103,
			103,
			101,
			114,
			66,
			117,
			115,
			108,
			111,
			97,
			100,
			76,
			111,
			119,
			38,
			67,
			65,
			78,
			66,
			117,
			115,
			83,
			116,
			97,
			116,
			105,
			115,
			116,
			105,
			99,
			115,
			84,
			114,
			105,
			103,
			103,
			101,
			114,
			66,
			117,
			115,
			108,
			111,
			97,
			100,
			82,
			101,
			108,
			97,
			116,
			105,
			111,
			110,
			36,
			67,
			65,
			78,
			66,
			117,
			115,
			83,
			116,
			97,
			116,
			105,
			115,
			116,
			105,
			99,
			115,
			84,
			114,
			105,
			103,
			103,
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
			38,
			67,
			65,
			78,
			66,
			117,
			115,
			83,
			116,
			97,
			116,
			105,
			115,
			116,
			105,
			99,
			115,
			84,
			114,
			105,
			103,
			103,
			101,
			114,
			69,
			114,
			114,
			111,
			114,
			70,
			114,
			97,
			109,
			101,
			115,
			72,
			105,
			103,
			104,
			37,
			67,
			65,
			78,
			66,
			117,
			115,
			83,
			116,
			97,
			116,
			105,
			115,
			116,
			105,
			99,
			115,
			84,
			114,
			105,
			103,
			103,
			101,
			114,
			69,
			114,
			114,
			111,
			114,
			70,
			114,
			97,
			109,
			101,
			115,
			76,
			111,
			119,
			35,
			67,
			65,
			78,
			66,
			117,
			115,
			83,
			116,
			97,
			116,
			105,
			115,
			116,
			105,
			99,
			115,
			69,
			114,
			114,
			111,
			114,
			70,
			114,
			97,
			109,
			101,
			115,
			82,
			101,
			108,
			97,
			116,
			105,
			111,
			110,
			39,
			67,
			65,
			78,
			66,
			117,
			115,
			83,
			116,
			97,
			116,
			105,
			115,
			116,
			105,
			99,
			115,
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
			78,
			68,
			67,
			111,
			110,
			106,
			117,
			110,
			99,
			116,
			105,
			111,
			110,
			39,
			67,
			65,
			78,
			66,
			117,
			115,
			83,
			116,
			97,
			116,
			105,
			115,
			116,
			105,
			99,
			115,
			84,
			114,
			105,
			103,
			103,
			101,
			114,
			73,
			115,
			66,
			117,
			115,
			108,
			111,
			97,
			100,
			69,
			110,
			97,
			98,
			108,
			101,
			100,
			36,
			67,
			65,
			78,
			66,
			117,
			115,
			83,
			116,
			97,
			116,
			105,
			115,
			116,
			105,
			99,
			115,
			73,
			115,
			69,
			114,
			114,
			111,
			114,
			70,
			114,
			97,
			109,
			101,
			115,
			69,
			110,
			97,
			98,
			108,
			101,
			100,
			30,
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
			84,
			114,
			105,
			103,
			103,
			101,
			114,
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
			23,
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
			84,
			114,
			105,
			103,
			103,
			101,
			114,
			69,
			100,
			103,
			101,
			19,
			75,
			101,
			121,
			84,
			114,
			105,
			103,
			103,
			101,
			114,
			73,
			115,
			79,
			110,
			80,
			97,
			110,
			101,
			108,
			16,
			75,
			101,
			121,
			84,
			114,
			105,
			103,
			103,
			101,
			114,
			78,
			117,
			109,
			98,
			101,
			114,
			13,
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
			41,
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
			97,
			116,
			97,
			98,
			97,
			115,
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
			31,
			87,
			76,
			65,
			78,
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
			97,
			114,
			116,
			87,
			76,
			65,
			78,
			69,
			118,
			101,
			110,
			116,
			7,
			84,
			114,
			105,
			103,
			103,
			101,
			114,
			0,
			0,
			0,
			0,
			1,
			0,
			0,
			0,
			2,
			3,
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
			1,
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
			Convert_8_to_9.Root({urn:schemas-microsoft-com:xslt-debug}runtime);
		}

		private static void Root(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			output.WriteStartRoot();
			Convert_8_to_9.<xsl:apply-templates>({urn:schemas-microsoft-com:xslt-debug}runtime, {urn:schemas-microsoft-com:xslt-debug}runtime.ExternalContext.DefaultDataSource);
			output.WriteEndRoot();
		}

		private static void <xsl:template name="newline">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			output.WriteRaw("\n");
		}

		private static void <xsl:template match="comment() | processing-instruction()">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current, string indent)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			Convert_8_to_9.<xsl:template name="newline">({urn:schemas-microsoft-com:xslt-debug}runtime);
			output.WriteString(indent);
			if (output.StartCopy({urn:schemas-microsoft-com:xslt-debug}current))
			{
				output.EndCopy({urn:schemas-microsoft-com:xslt-debug}current);
			}
		}

		private static void <xsl:template match="text()">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current, string indent)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			Convert_8_to_9.<xsl:template name="newline">({urn:schemas-microsoft-com:xslt-debug}runtime);
			output.WriteString(indent);
			output.WriteString(XsltFunctions.NormalizeSpace({urn:schemas-microsoft-com:xslt-debug}current.Value));
		}

		private static void <xsl:template match="VLConfig:FileFormatVersion" priority="1">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			output.WriteStartElement("", "FileFormatVersion", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("i", "http://www.w3.org/2001/XMLSchema-instance");
			output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("9");
			output.WriteEndElement();
		}

		private static void <xsl:template match="VLConfig:Value" priority="1">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			XPathNavigator xPathNavigator = Convert_8_to_9.SyncToNavigator(xPathNavigator, {urn:schemas-microsoft-com:xslt-debug}current);
			if (xPathNavigator.MoveToAttribute({urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(0), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1)))
			{
				output.WriteStartElement("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteNamespaceDeclarationUnchecked("i", "http://www.w3.org/2001/XMLSchema-instance");
				output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				XmlQueryOutput arg_148_0 = output;
				XPathNavigator xPathNavigator2 = Convert_8_to_9.SyncToNavigator(xPathNavigator2, {urn:schemas-microsoft-com:xslt-debug}current);
				xPathNavigator2.MoveToRoot();
				DescendantIterator descendantIterator;
				descendantIterator.Create(xPathNavigator2, {urn:schemas-microsoft-com:xslt-debug}runtime.GetNameFilter(0), false);
				string arg_148_1;
				while (descendantIterator.MoveNext())
				{
					XPathNavigator xPathNavigator3 = Convert_8_to_9.SyncToNavigator(xPathNavigator3, descendantIterator.Current);
					if (xPathNavigator3.MoveToAttribute({urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(4), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1)))
					{
						XPathNavigator xPathNavigator4 = Convert_8_to_9.SyncToNavigator(xPathNavigator4, {urn:schemas-microsoft-com:xslt-debug}current);
						if (xPathNavigator4.MoveToAttribute({urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(0), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1)) && string.Equals(xPathNavigator3.Value, xPathNavigator4.Value))
						{
							arg_148_1 = descendantIterator.Current.Value;
							IL_148:
							arg_148_0.WriteStringUnchecked(arg_148_1);
							output.WriteEndElement();
							return;
						}
					}
				}
				arg_148_1 = "";
				goto IL_148;
			}
			output.XsltCopyOf({urn:schemas-microsoft-com:xslt-debug}current);
		}

		private static void 9">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current, string indent)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			XPathNavigator xPathNavigator = Convert_8_to_9.SyncToNavigator(xPathNavigator, {urn:schemas-microsoft-com:xslt-debug}current);
			string a = (!xPathNavigator.MoveToAttribute({urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(5), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(6))) ? "" : xPathNavigator.Value;
			output.WriteStartElement("", "RecordTrigger", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("i", "http://www.w3.org/2001/XMLSchema-instance");
			output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.WriteStartElement("", "RecordTriggerAction", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			ElementContentIterator elementContentIterator;
			elementContentIterator.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(7), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
			while (elementContentIterator.MoveNext())
			{
				NodeKindContentIterator nodeKindContentIterator;
				nodeKindContentIterator.Create(elementContentIterator.Current, XPathNodeType.Element);
				while (nodeKindContentIterator.MoveNext())
				{
					XPathItem current = nodeKindContentIterator.Current;
					output.WriteItem(current);
				}
			}
			output.WriteEndElement();
			output.WriteStartElement("", "RecordTriggerComment", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			ElementContentIterator elementContentIterator2;
			elementContentIterator2.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(8), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
			while (elementContentIterator2.MoveNext())
			{
				NodeKindContentIterator nodeKindContentIterator2;
				nodeKindContentIterator2.Create(elementContentIterator2.Current, XPathNodeType.Element);
				while (nodeKindContentIterator2.MoveNext())
				{
					Convert_8_to_9.<xsl:apply-templates>({urn:schemas-microsoft-com:xslt-debug}runtime, nodeKindContentIterator2.Current);
				}
			}
			output.WriteEndElement();
			output.WriteStartElement("", "RecordTriggerEffect", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			ElementContentIterator elementContentIterator3;
			elementContentIterator3.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(9), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
			while (elementContentIterator3.MoveNext())
			{
				NodeKindContentIterator nodeKindContentIterator3;
				nodeKindContentIterator3.Create(elementContentIterator3.Current, XPathNodeType.Element);
				while (nodeKindContentIterator3.MoveNext())
				{
					XPathItem current2 = nodeKindContentIterator3.Current;
					output.WriteItem(current2);
				}
			}
			output.WriteEndElement();
			if (string.Equals(a, "CANIdTrigger"))
			{
				output.WriteStartElementUnchecked("", "RecordTriggerEvent", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.WriteStartAttributeUnchecked("i", "type", "http://www.w3.org/2001/XMLSchema-instance");
				output.WriteStringUnchecked("CANIdEvent");
				output.WriteEndAttributeUnchecked();
				output.WriteStartElement("", "IdEventChannelNumber", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				ElementContentIterator elementContentIterator4;
				elementContentIterator4.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(10), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
				while (elementContentIterator4.MoveNext())
				{
					NodeKindContentIterator nodeKindContentIterator4;
					nodeKindContentIterator4.Create(elementContentIterator4.Current, XPathNodeType.Element);
					while (nodeKindContentIterator4.MoveNext())
					{
						XPathItem current3 = nodeKindContentIterator4.Current;
						output.WriteItem(current3);
					}
				}
				output.WriteEndElement();
				output.WriteStartElement("", "IdEventHighId", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				ElementContentIterator elementContentIterator5;
				elementContentIterator5.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(11), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
				while (elementContentIterator5.MoveNext())
				{
					NodeKindContentIterator nodeKindContentIterator5;
					nodeKindContentIterator5.Create(elementContentIterator5.Current, XPathNodeType.Element);
					while (nodeKindContentIterator5.MoveNext())
					{
						XPathItem current4 = nodeKindContentIterator5.Current;
						output.WriteItem(current4);
					}
				}
				output.WriteEndElement();
				output.WriteStartElement("", "IdEventIdRelation", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				ElementContentIterator elementContentIterator6;
				elementContentIterator6.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(12), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
				while (elementContentIterator6.MoveNext())
				{
					NodeKindContentIterator nodeKindContentIterator6;
					nodeKindContentIterator6.Create(elementContentIterator6.Current, XPathNodeType.Element);
					while (nodeKindContentIterator6.MoveNext())
					{
						XPathItem current5 = nodeKindContentIterator6.Current;
						output.WriteItem(current5);
					}
				}
				output.WriteEndElement();
				output.WriteStartElement("", "IdEventLowId", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				ElementContentIterator elementContentIterator7;
				elementContentIterator7.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(13), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
				while (elementContentIterator7.MoveNext())
				{
					NodeKindContentIterator nodeKindContentIterator7;
					nodeKindContentIterator7.Create(elementContentIterator7.Current, XPathNodeType.Element);
					while (nodeKindContentIterator7.MoveNext())
					{
						XPathItem current6 = nodeKindContentIterator7.Current;
						output.WriteItem(current6);
					}
				}
				output.WriteEndElement();
				output.WriteStartElement("", "CANIdEventIsExtendedId", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				ElementContentIterator elementContentIterator8;
				elementContentIterator8.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(14), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
				while (elementContentIterator8.MoveNext())
				{
					NodeKindContentIterator nodeKindContentIterator8;
					nodeKindContentIterator8.Create(elementContentIterator8.Current, XPathNodeType.Element);
					while (nodeKindContentIterator8.MoveNext())
					{
						XPathItem current7 = nodeKindContentIterator8.Current;
						output.WriteItem(current7);
					}
				}
				output.WriteEndElement();
				output.WriteEndElementUnchecked("", "RecordTriggerEvent", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			}
			else if (string.Equals(a, "LINIdTrigger"))
			{
				output.WriteStartElementUnchecked("", "RecordTriggerEvent", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.WriteStartAttributeUnchecked("i", "type", "http://www.w3.org/2001/XMLSchema-instance");
				output.WriteStringUnchecked("LINIdEvent");
				output.WriteEndAttributeUnchecked();
				output.WriteStartElement("", "IdEventChannelNumber", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				ElementContentIterator elementContentIterator9;
				elementContentIterator9.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(10), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
				while (elementContentIterator9.MoveNext())
				{
					NodeKindContentIterator nodeKindContentIterator9;
					nodeKindContentIterator9.Create(elementContentIterator9.Current, XPathNodeType.Element);
					while (nodeKindContentIterator9.MoveNext())
					{
						XPathItem current8 = nodeKindContentIterator9.Current;
						output.WriteItem(current8);
					}
				}
				output.WriteEndElement();
				output.WriteStartElement("", "IdEventHighId", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				ElementContentIterator elementContentIterator10;
				elementContentIterator10.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(11), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
				while (elementContentIterator10.MoveNext())
				{
					NodeKindContentIterator nodeKindContentIterator10;
					nodeKindContentIterator10.Create(elementContentIterator10.Current, XPathNodeType.Element);
					while (nodeKindContentIterator10.MoveNext())
					{
						XPathItem current9 = nodeKindContentIterator10.Current;
						output.WriteItem(current9);
					}
				}
				output.WriteEndElement();
				output.WriteStartElement("", "IdEventIdRelation", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				ElementContentIterator elementContentIterator11;
				elementContentIterator11.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(12), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
				while (elementContentIterator11.MoveNext())
				{
					NodeKindContentIterator nodeKindContentIterator11;
					nodeKindContentIterator11.Create(elementContentIterator11.Current, XPathNodeType.Element);
					while (nodeKindContentIterator11.MoveNext())
					{
						XPathItem current10 = nodeKindContentIterator11.Current;
						output.WriteItem(current10);
					}
				}
				output.WriteEndElement();
				output.WriteStartElement("", "IdEventLowId", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				ElementContentIterator elementContentIterator12;
				elementContentIterator12.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(13), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
				while (elementContentIterator12.MoveNext())
				{
					NodeKindContentIterator nodeKindContentIterator12;
					nodeKindContentIterator12.Create(elementContentIterator12.Current, XPathNodeType.Element);
					while (nodeKindContentIterator12.MoveNext())
					{
						XPathItem current11 = nodeKindContentIterator12.Current;
						output.WriteItem(current11);
					}
				}
				output.WriteEndElement();
				output.WriteEndElementUnchecked("", "RecordTriggerEvent", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			}
			else if (string.Equals(a, "FlexrayIdTrigger"))
			{
				output.WriteStartElementUnchecked("", "RecordTriggerEvent", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.WriteStartAttributeUnchecked("i", "type", "http://www.w3.org/2001/XMLSchema-instance");
				output.WriteStringUnchecked("FlexrayIdEvent");
				output.WriteEndAttributeUnchecked();
				output.WriteStartElement("", "IdEventChannelNumber", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				ElementContentIterator elementContentIterator13;
				elementContentIterator13.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(10), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
				while (elementContentIterator13.MoveNext())
				{
					NodeKindContentIterator nodeKindContentIterator13;
					nodeKindContentIterator13.Create(elementContentIterator13.Current, XPathNodeType.Element);
					while (nodeKindContentIterator13.MoveNext())
					{
						XPathItem current12 = nodeKindContentIterator13.Current;
						output.WriteItem(current12);
					}
				}
				output.WriteEndElement();
				output.WriteStartElement("", "IdEventHighId", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				ElementContentIterator elementContentIterator14;
				elementContentIterator14.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(11), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
				while (elementContentIterator14.MoveNext())
				{
					NodeKindContentIterator nodeKindContentIterator14;
					nodeKindContentIterator14.Create(elementContentIterator14.Current, XPathNodeType.Element);
					while (nodeKindContentIterator14.MoveNext())
					{
						XPathItem current13 = nodeKindContentIterator14.Current;
						output.WriteItem(current13);
					}
				}
				output.WriteEndElement();
				output.WriteStartElement("", "IdEventIdRelation", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				ElementContentIterator elementContentIterator15;
				elementContentIterator15.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(12), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
				while (elementContentIterator15.MoveNext())
				{
					NodeKindContentIterator nodeKindContentIterator15;
					nodeKindContentIterator15.Create(elementContentIterator15.Current, XPathNodeType.Element);
					while (nodeKindContentIterator15.MoveNext())
					{
						XPathItem current14 = nodeKindContentIterator15.Current;
						output.WriteItem(current14);
					}
				}
				output.WriteEndElement();
				output.WriteStartElement("", "IdEventLowId", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				ElementContentIterator elementContentIterator16;
				elementContentIterator16.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(13), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
				while (elementContentIterator16.MoveNext())
				{
					NodeKindContentIterator nodeKindContentIterator16;
					nodeKindContentIterator16.Create(elementContentIterator16.Current, XPathNodeType.Element);
					while (nodeKindContentIterator16.MoveNext())
					{
						XPathItem current15 = nodeKindContentIterator16.Current;
						output.WriteItem(current15);
					}
				}
				output.WriteEndElement();
				output.WriteStartElement("", "FlexrayIdEventBaseCycle", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				ElementContentIterator elementContentIterator17;
				elementContentIterator17.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(15), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
				while (elementContentIterator17.MoveNext())
				{
					NodeKindContentIterator nodeKindContentIterator17;
					nodeKindContentIterator17.Create(elementContentIterator17.Current, XPathNodeType.Element);
					while (nodeKindContentIterator17.MoveNext())
					{
						XPathItem current16 = nodeKindContentIterator17.Current;
						output.WriteItem(current16);
					}
				}
				output.WriteEndElement();
				output.WriteStartElement("", "FlexrayIdEventCycleRepetition", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				ElementContentIterator elementContentIterator18;
				elementContentIterator18.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(16), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
				while (elementContentIterator18.MoveNext())
				{
					NodeKindContentIterator nodeKindContentIterator18;
					nodeKindContentIterator18.Create(elementContentIterator18.Current, XPathNodeType.Element);
					while (nodeKindContentIterator18.MoveNext())
					{
						XPathItem current17 = nodeKindContentIterator18.Current;
						output.WriteItem(current17);
					}
				}
				output.WriteEndElement();
				output.WriteEndElementUnchecked("", "RecordTriggerEvent", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			}
			else
			{
				if (string.Equals(a, "CANDataTrigger"))
				{
					ElementContentIterator elementContentIterator19;
					elementContentIterator19.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(17), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
					string arg_CB8_0;
					while (elementContentIterator19.MoveNext())
					{
						XPathNavigator xPathNavigator2 = Convert_8_to_9.SyncToNavigator(xPathNavigator2, elementContentIterator19.Current);
						if (xPathNavigator2.MoveToAttribute({urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(5), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(6)))
						{
							arg_CB8_0 = xPathNavigator2.Value;
							IL_CB8:
							string a2 = arg_CB8_0;
							output.WriteStartElementUnchecked("", "RecordTriggerEvent", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
							output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
							output.WriteStringUnchecked("0");
							output.WriteEndAttributeUnchecked();
							output.WriteStartAttributeUnchecked("i", "type", "http://www.w3.org/2001/XMLSchema-instance");
							output.WriteStringUnchecked("CANDataEvent");
							output.WriteEndAttributeUnchecked();
							output.WriteStartElement("", "CANDataEventChannelNumber", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
							output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
							output.WriteStringUnchecked("0");
							output.WriteEndAttributeUnchecked();
							ElementContentIterator elementContentIterator20;
							elementContentIterator20.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(18), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
							while (elementContentIterator20.MoveNext())
							{
								NodeKindContentIterator nodeKindContentIterator19;
								nodeKindContentIterator19.Create(elementContentIterator20.Current, XPathNodeType.Element);
								while (nodeKindContentIterator19.MoveNext())
								{
									XPathItem current18 = nodeKindContentIterator19.Current;
									output.WriteItem(current18);
								}
							}
							output.WriteEndElement();
							output.WriteStartElement("", "CANDataEventHighValue", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
							output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
							output.WriteStringUnchecked("0");
							output.WriteEndAttributeUnchecked();
							ElementContentIterator elementContentIterator21;
							elementContentIterator21.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(19), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
							while (elementContentIterator21.MoveNext())
							{
								NodeKindContentIterator nodeKindContentIterator20;
								nodeKindContentIterator20.Create(elementContentIterator21.Current, XPathNodeType.Element);
								while (nodeKindContentIterator20.MoveNext())
								{
									XPathItem current19 = nodeKindContentIterator20.Current;
									output.WriteItem(current19);
								}
							}
							output.WriteEndElement();
							output.WriteStartElement("", "CANDataEventID", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
							output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
							output.WriteStringUnchecked("0");
							output.WriteEndAttributeUnchecked();
							ElementContentIterator elementContentIterator22;
							elementContentIterator22.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(20), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
							while (elementContentIterator22.MoveNext())
							{
								NodeKindContentIterator nodeKindContentIterator21;
								nodeKindContentIterator21.Create(elementContentIterator22.Current, XPathNodeType.Element);
								while (nodeKindContentIterator21.MoveNext())
								{
									XPathItem current20 = nodeKindContentIterator21.Current;
									output.WriteItem(current20);
								}
							}
							output.WriteEndElement();
							output.WriteStartElement("", "CANDataEventIsExtendedId", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
							output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
							output.WriteStringUnchecked("0");
							output.WriteEndAttributeUnchecked();
							ElementContentIterator elementContentIterator23;
							elementContentIterator23.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(21), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
							while (elementContentIterator23.MoveNext())
							{
								NodeKindContentIterator nodeKindContentIterator22;
								nodeKindContentIterator22.Create(elementContentIterator23.Current, XPathNodeType.Element);
								while (nodeKindContentIterator22.MoveNext())
								{
									XPathItem current21 = nodeKindContentIterator22.Current;
									output.WriteItem(current21);
								}
							}
							output.WriteEndElement();
							output.WriteStartElement("", "CANDataEventLowValue", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
							output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
							output.WriteStringUnchecked("0");
							output.WriteEndAttributeUnchecked();
							ElementContentIterator elementContentIterator24;
							elementContentIterator24.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(22), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
							while (elementContentIterator24.MoveNext())
							{
								NodeKindContentIterator nodeKindContentIterator23;
								nodeKindContentIterator23.Create(elementContentIterator24.Current, XPathNodeType.Element);
								while (nodeKindContentIterator23.MoveNext())
								{
									XPathItem current22 = nodeKindContentIterator23.Current;
									output.WriteItem(current22);
								}
							}
							output.WriteEndElement();
							if (string.Equals(a2, "RawDataSignalByte"))
							{
								output.WriteStartElementUnchecked("", "CANDataEventRawDataSignal", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
								output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
								output.WriteStringUnchecked("0");
								output.WriteEndAttributeUnchecked();
								output.WriteStartAttributeUnchecked("i", "type", "http://www.w3.org/2001/XMLSchema-instance");
								output.WriteStringUnchecked("RawDataSignalByte");
								output.WriteEndAttributeUnchecked();
								output.WriteStartElement("", "DataBytePos", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
								output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
								output.WriteStringUnchecked("0");
								output.WriteEndAttributeUnchecked();
								ElementContentIterator elementContentIterator25;
								elementContentIterator25.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(17), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
								while (elementContentIterator25.MoveNext())
								{
									ElementContentIterator elementContentIterator26;
									elementContentIterator26.Create(elementContentIterator25.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(23), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
									while (elementContentIterator26.MoveNext())
									{
										NodeKindContentIterator nodeKindContentIterator24;
										nodeKindContentIterator24.Create(elementContentIterator26.Current, XPathNodeType.Element);
										while (nodeKindContentIterator24.MoveNext())
										{
											XPathItem current23 = nodeKindContentIterator24.Current;
											output.WriteItem(current23);
										}
									}
								}
								output.WriteEndElement();
								output.WriteEndElementUnchecked("", "CANDataEventRawDataSignal", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
							}
							else if (string.Equals(a2, "RawDataSignalStartbitLength"))
							{
								output.WriteStartElementUnchecked("", "CANDataEventRawDataSignal", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
								output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
								output.WriteStringUnchecked("0");
								output.WriteEndAttributeUnchecked();
								output.WriteStartAttributeUnchecked("i", "type", "http://www.w3.org/2001/XMLSchema-instance");
								output.WriteStringUnchecked("RawDataSignalStartbitLength");
								output.WriteEndAttributeUnchecked();
								output.WriteStartElement("", "IsMotorola", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
								output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
								output.WriteStringUnchecked("0");
								output.WriteEndAttributeUnchecked();
								ElementContentIterator elementContentIterator27;
								elementContentIterator27.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(17), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
								while (elementContentIterator27.MoveNext())
								{
									ElementContentIterator elementContentIterator28;
									elementContentIterator28.Create(elementContentIterator27.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(24), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
									while (elementContentIterator28.MoveNext())
									{
										NodeKindContentIterator nodeKindContentIterator25;
										nodeKindContentIterator25.Create(elementContentIterator28.Current, XPathNodeType.Element);
										while (nodeKindContentIterator25.MoveNext())
										{
											XPathItem current24 = nodeKindContentIterator25.Current;
											output.WriteItem(current24);
										}
									}
								}
								output.WriteEndElement();
								output.WriteStartElement("", "Length", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
								output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
								output.WriteStringUnchecked("0");
								output.WriteEndAttributeUnchecked();
								ElementContentIterator elementContentIterator29;
								elementContentIterator29.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(17), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
								while (elementContentIterator29.MoveNext())
								{
									ElementContentIterator elementContentIterator30;
									elementContentIterator30.Create(elementContentIterator29.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(25), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
									while (elementContentIterator30.MoveNext())
									{
										NodeKindContentIterator nodeKindContentIterator26;
										nodeKindContentIterator26.Create(elementContentIterator30.Current, XPathNodeType.Element);
										while (nodeKindContentIterator26.MoveNext())
										{
											XPathItem current25 = nodeKindContentIterator26.Current;
											output.WriteItem(current25);
										}
									}
								}
								output.WriteEndElement();
								output.WriteStartElement("", "StartbitPos", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
								output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
								output.WriteStringUnchecked("0");
								output.WriteEndAttributeUnchecked();
								ElementContentIterator elementContentIterator31;
								elementContentIterator31.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(17), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
								while (elementContentIterator31.MoveNext())
								{
									ElementContentIterator elementContentIterator32;
									elementContentIterator32.Create(elementContentIterator31.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(26), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
									while (elementContentIterator32.MoveNext())
									{
										NodeKindContentIterator nodeKindContentIterator27;
										nodeKindContentIterator27.Create(elementContentIterator32.Current, XPathNodeType.Element);
										while (nodeKindContentIterator27.MoveNext())
										{
											XPathItem current26 = nodeKindContentIterator27.Current;
											output.WriteItem(current26);
										}
									}
								}
								output.WriteEndElement();
								output.WriteEndElementUnchecked("", "CANDataEventRawDataSignal", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
							}
							output.WriteStartElement("", "CANDataEventRelation", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
							output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
							output.WriteStringUnchecked("0");
							output.WriteEndAttributeUnchecked();
							ElementContentIterator elementContentIterator33;
							elementContentIterator33.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(27), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
							while (elementContentIterator33.MoveNext())
							{
								NodeKindContentIterator nodeKindContentIterator28;
								nodeKindContentIterator28.Create(elementContentIterator33.Current, XPathNodeType.Element);
								while (nodeKindContentIterator28.MoveNext())
								{
									XPathItem current27 = nodeKindContentIterator28.Current;
									output.WriteItem(current27);
								}
							}
							output.WriteEndElement();
							output.WriteEndElementUnchecked("", "RecordTriggerEvent", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
							goto IL_2F34;
						}
					}
					arg_CB8_0 = "";
					goto IL_CB8;
				}
				if (string.Equals(a, "LINDataTrigger"))
				{
					ElementContentIterator elementContentIterator34;
					elementContentIterator34.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(17), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
					string arg_14E1_0;
					while (elementContentIterator34.MoveNext())
					{
						XPathNavigator xPathNavigator3 = Convert_8_to_9.SyncToNavigator(xPathNavigator3, elementContentIterator34.Current);
						if (xPathNavigator3.MoveToAttribute({urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(5), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(6)))
						{
							arg_14E1_0 = xPathNavigator3.Value;
							IL_14E1:
							string a3 = arg_14E1_0;
							output.WriteStartElementUnchecked("", "RecordTriggerEvent", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
							output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
							output.WriteStringUnchecked("0");
							output.WriteEndAttributeUnchecked();
							output.WriteStartAttributeUnchecked("i", "type", "http://www.w3.org/2001/XMLSchema-instance");
							output.WriteStringUnchecked("LINDataEvent");
							output.WriteEndAttributeUnchecked();
							output.WriteStartElement("", "LINDataEventChannelNumber", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
							output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
							output.WriteStringUnchecked("0");
							output.WriteEndAttributeUnchecked();
							ElementContentIterator elementContentIterator35;
							elementContentIterator35.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(28), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
							while (elementContentIterator35.MoveNext())
							{
								NodeKindContentIterator nodeKindContentIterator29;
								nodeKindContentIterator29.Create(elementContentIterator35.Current, XPathNodeType.Element);
								while (nodeKindContentIterator29.MoveNext())
								{
									XPathItem current28 = nodeKindContentIterator29.Current;
									output.WriteItem(current28);
								}
							}
							output.WriteEndElement();
							output.WriteStartElement("", "LINDataEventHighValue", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
							output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
							output.WriteStringUnchecked("0");
							output.WriteEndAttributeUnchecked();
							ElementContentIterator elementContentIterator36;
							elementContentIterator36.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(29), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
							while (elementContentIterator36.MoveNext())
							{
								NodeKindContentIterator nodeKindContentIterator30;
								nodeKindContentIterator30.Create(elementContentIterator36.Current, XPathNodeType.Element);
								while (nodeKindContentIterator30.MoveNext())
								{
									XPathItem current29 = nodeKindContentIterator30.Current;
									output.WriteItem(current29);
								}
							}
							output.WriteEndElement();
							output.WriteStartElement("", "LINDataEventID", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
							output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
							output.WriteStringUnchecked("0");
							output.WriteEndAttributeUnchecked();
							ElementContentIterator elementContentIterator37;
							elementContentIterator37.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(30), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
							while (elementContentIterator37.MoveNext())
							{
								NodeKindContentIterator nodeKindContentIterator31;
								nodeKindContentIterator31.Create(elementContentIterator37.Current, XPathNodeType.Element);
								while (nodeKindContentIterator31.MoveNext())
								{
									XPathItem current30 = nodeKindContentIterator31.Current;
									output.WriteItem(current30);
								}
							}
							output.WriteEndElement();
							output.WriteStartElement("", "LINDataEventLowValue", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
							output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
							output.WriteStringUnchecked("0");
							output.WriteEndAttributeUnchecked();
							ElementContentIterator elementContentIterator38;
							elementContentIterator38.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(31), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
							while (elementContentIterator38.MoveNext())
							{
								NodeKindContentIterator nodeKindContentIterator32;
								nodeKindContentIterator32.Create(elementContentIterator38.Current, XPathNodeType.Element);
								while (nodeKindContentIterator32.MoveNext())
								{
									XPathItem current31 = nodeKindContentIterator32.Current;
									output.WriteItem(current31);
								}
							}
							output.WriteEndElement();
							if (string.Equals(a3, "RawDataSignalByte"))
							{
								output.WriteStartElementUnchecked("", "LINDataEventRawDataSignal", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
								output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
								output.WriteStringUnchecked("0");
								output.WriteEndAttributeUnchecked();
								output.WriteStartAttributeUnchecked("i", "type", "http://www.w3.org/2001/XMLSchema-instance");
								output.WriteStringUnchecked("RawDataSignalByte");
								output.WriteEndAttributeUnchecked();
								output.WriteStartElement("", "DataBytePos", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
								output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
								output.WriteStringUnchecked("0");
								output.WriteEndAttributeUnchecked();
								ElementContentIterator elementContentIterator39;
								elementContentIterator39.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(17), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
								while (elementContentIterator39.MoveNext())
								{
									ElementContentIterator elementContentIterator40;
									elementContentIterator40.Create(elementContentIterator39.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(23), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
									while (elementContentIterator40.MoveNext())
									{
										NodeKindContentIterator nodeKindContentIterator33;
										nodeKindContentIterator33.Create(elementContentIterator40.Current, XPathNodeType.Element);
										while (nodeKindContentIterator33.MoveNext())
										{
											XPathItem current32 = nodeKindContentIterator33.Current;
											output.WriteItem(current32);
										}
									}
								}
								output.WriteEndElement();
								output.WriteEndElementUnchecked("", "LINDataEventRawDataSignal", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
							}
							else if (string.Equals(a3, "RawDataSignalStartbitLength"))
							{
								output.WriteStartElementUnchecked("", "LINDataEventRawDataSignal", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
								output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
								output.WriteStringUnchecked("0");
								output.WriteEndAttributeUnchecked();
								output.WriteStartAttributeUnchecked("i", "type", "http://www.w3.org/2001/XMLSchema-instance");
								output.WriteStringUnchecked("RawDataSignalStartbitLength");
								output.WriteEndAttributeUnchecked();
								output.WriteStartElement("", "IsMotorola", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
								output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
								output.WriteStringUnchecked("0");
								output.WriteEndAttributeUnchecked();
								ElementContentIterator elementContentIterator41;
								elementContentIterator41.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(17), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
								while (elementContentIterator41.MoveNext())
								{
									ElementContentIterator elementContentIterator42;
									elementContentIterator42.Create(elementContentIterator41.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(24), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
									while (elementContentIterator42.MoveNext())
									{
										NodeKindContentIterator nodeKindContentIterator34;
										nodeKindContentIterator34.Create(elementContentIterator42.Current, XPathNodeType.Element);
										while (nodeKindContentIterator34.MoveNext())
										{
											XPathItem current33 = nodeKindContentIterator34.Current;
											output.WriteItem(current33);
										}
									}
								}
								output.WriteEndElement();
								output.WriteStartElement("", "Length", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
								output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
								output.WriteStringUnchecked("0");
								output.WriteEndAttributeUnchecked();
								ElementContentIterator elementContentIterator43;
								elementContentIterator43.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(17), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
								while (elementContentIterator43.MoveNext())
								{
									ElementContentIterator elementContentIterator44;
									elementContentIterator44.Create(elementContentIterator43.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(25), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
									while (elementContentIterator44.MoveNext())
									{
										NodeKindContentIterator nodeKindContentIterator35;
										nodeKindContentIterator35.Create(elementContentIterator44.Current, XPathNodeType.Element);
										while (nodeKindContentIterator35.MoveNext())
										{
											XPathItem current34 = nodeKindContentIterator35.Current;
											output.WriteItem(current34);
										}
									}
								}
								output.WriteEndElement();
								output.WriteStartElement("", "StartbitPos", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
								output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
								output.WriteStringUnchecked("0");
								output.WriteEndAttributeUnchecked();
								ElementContentIterator elementContentIterator45;
								elementContentIterator45.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(17), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
								while (elementContentIterator45.MoveNext())
								{
									ElementContentIterator elementContentIterator46;
									elementContentIterator46.Create(elementContentIterator45.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(26), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
									while (elementContentIterator46.MoveNext())
									{
										NodeKindContentIterator nodeKindContentIterator36;
										nodeKindContentIterator36.Create(elementContentIterator46.Current, XPathNodeType.Element);
										while (nodeKindContentIterator36.MoveNext())
										{
											XPathItem current35 = nodeKindContentIterator36.Current;
											output.WriteItem(current35);
										}
									}
								}
								output.WriteEndElement();
								output.WriteEndElementUnchecked("", "LINDataEventRawDataSignal", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
							}
							output.WriteStartElement("", "LINDataEventRelation", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
							output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
							output.WriteStringUnchecked("0");
							output.WriteEndAttributeUnchecked();
							ElementContentIterator elementContentIterator47;
							elementContentIterator47.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(32), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
							while (elementContentIterator47.MoveNext())
							{
								NodeKindContentIterator nodeKindContentIterator37;
								nodeKindContentIterator37.Create(elementContentIterator47.Current, XPathNodeType.Element);
								while (nodeKindContentIterator37.MoveNext())
								{
									XPathItem current36 = nodeKindContentIterator37.Current;
									output.WriteItem(current36);
								}
							}
							output.WriteEndElement();
							output.WriteEndElementUnchecked("", "RecordTriggerEvent", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
							goto IL_2F34;
						}
					}
					arg_14E1_0 = "";
					goto IL_14E1;
				}
				if (string.Equals(a, "SymbolicMessageTrigger"))
				{
					output.WriteStartElementUnchecked("", "RecordTriggerEvent", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
					output.WriteStringUnchecked("0");
					output.WriteEndAttributeUnchecked();
					output.WriteStartAttributeUnchecked("i", "type", "http://www.w3.org/2001/XMLSchema-instance");
					output.WriteStringUnchecked("SymbolicMessageEvent");
					output.WriteEndAttributeUnchecked();
					output.WriteStartElement("", "SymbolicMessageEventBusType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
					output.WriteStringUnchecked("0");
					output.WriteEndAttributeUnchecked();
					ElementContentIterator elementContentIterator48;
					elementContentIterator48.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(33), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
					while (elementContentIterator48.MoveNext())
					{
						NodeKindContentIterator nodeKindContentIterator38;
						nodeKindContentIterator38.Create(elementContentIterator48.Current, XPathNodeType.Element);
						while (nodeKindContentIterator38.MoveNext())
						{
							XPathItem current37 = nodeKindContentIterator38.Current;
							output.WriteItem(current37);
						}
					}
					output.WriteEndElement();
					output.WriteStartElement("", "SymbolicMessageEventChannelNumber", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
					output.WriteStringUnchecked("0");
					output.WriteEndAttributeUnchecked();
					ElementContentIterator elementContentIterator49;
					elementContentIterator49.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(34), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
					while (elementContentIterator49.MoveNext())
					{
						NodeKindContentIterator nodeKindContentIterator39;
						nodeKindContentIterator39.Create(elementContentIterator49.Current, XPathNodeType.Element);
						while (nodeKindContentIterator39.MoveNext())
						{
							XPathItem current38 = nodeKindContentIterator39.Current;
							output.WriteItem(current38);
						}
					}
					output.WriteEndElement();
					output.WriteStartElement("", "SymbolicMessageEventDatabaseName", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
					output.WriteStringUnchecked("0");
					output.WriteEndAttributeUnchecked();
					ElementContentIterator elementContentIterator50;
					elementContentIterator50.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(35), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
					while (elementContentIterator50.MoveNext())
					{
						ElementContentIterator elementContentIterator51;
						elementContentIterator51.Create(elementContentIterator50.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(2), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
						while (elementContentIterator51.MoveNext())
						{
							Convert_8_to_9.<xsl:apply-templates>({urn:schemas-microsoft-com:xslt-debug}runtime, elementContentIterator51.Current);
						}
					}
					output.WriteEndElement();
					output.WriteStartElement("", "SymbolicMessageEventDatabasePath", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
					output.WriteStringUnchecked("0");
					output.WriteEndAttributeUnchecked();
					ElementContentIterator elementContentIterator52;
					elementContentIterator52.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(36), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
					while (elementContentIterator52.MoveNext())
					{
						ElementContentIterator elementContentIterator53;
						elementContentIterator53.Create(elementContentIterator52.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(2), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
						while (elementContentIterator53.MoveNext())
						{
							Convert_8_to_9.<xsl:apply-templates>({urn:schemas-microsoft-com:xslt-debug}runtime, elementContentIterator53.Current);
						}
					}
					output.WriteEndElement();
					output.WriteStartElement("", "SymbolicMessageEventIsFlexrayPDU", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
					output.WriteStringUnchecked("0");
					output.WriteEndAttributeUnchecked();
					ElementContentIterator elementContentIterator54;
					elementContentIterator54.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(37), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
					while (elementContentIterator54.MoveNext())
					{
						NodeKindContentIterator nodeKindContentIterator40;
						nodeKindContentIterator40.Create(elementContentIterator54.Current, XPathNodeType.Element);
						while (nodeKindContentIterator40.MoveNext())
						{
							XPathItem current39 = nodeKindContentIterator40.Current;
							output.WriteItem(current39);
						}
					}
					output.WriteEndElement();
					output.WriteStartElement("", "SymbolicMessageEventMessageName", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
					output.WriteStringUnchecked("0");
					output.WriteEndAttributeUnchecked();
					ElementContentIterator elementContentIterator55;
					elementContentIterator55.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(38), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
					while (elementContentIterator55.MoveNext())
					{
						ElementContentIterator elementContentIterator56;
						elementContentIterator56.Create(elementContentIterator55.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(2), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
						while (elementContentIterator56.MoveNext())
						{
							Convert_8_to_9.<xsl:apply-templates>({urn:schemas-microsoft-com:xslt-debug}runtime, elementContentIterator56.Current);
						}
					}
					output.WriteEndElement();
					output.WriteEndElementUnchecked("", "RecordTriggerEvent", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				}
				else if (string.Equals(a, "SymbolicSignalTrigger"))
				{
					output.WriteStartElementUnchecked("", "RecordTriggerEvent", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
					output.WriteStringUnchecked("0");
					output.WriteEndAttributeUnchecked();
					output.WriteStartAttributeUnchecked("i", "type", "http://www.w3.org/2001/XMLSchema-instance");
					output.WriteStringUnchecked("SymbolicSignalEvent");
					output.WriteEndAttributeUnchecked();
					output.WriteStartElement("", "SymbolicSignalEventBusType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
					output.WriteStringUnchecked("0");
					output.WriteEndAttributeUnchecked();
					ElementContentIterator elementContentIterator57;
					elementContentIterator57.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(39), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
					while (elementContentIterator57.MoveNext())
					{
						NodeKindContentIterator nodeKindContentIterator41;
						nodeKindContentIterator41.Create(elementContentIterator57.Current, XPathNodeType.Element);
						while (nodeKindContentIterator41.MoveNext())
						{
							XPathItem current40 = nodeKindContentIterator41.Current;
							output.WriteItem(current40);
						}
					}
					output.WriteEndElement();
					output.WriteStartElement("", "SymbolicSignalEventChannelNumber", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
					output.WriteStringUnchecked("0");
					output.WriteEndAttributeUnchecked();
					ElementContentIterator elementContentIterator58;
					elementContentIterator58.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(40), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
					while (elementContentIterator58.MoveNext())
					{
						NodeKindContentIterator nodeKindContentIterator42;
						nodeKindContentIterator42.Create(elementContentIterator58.Current, XPathNodeType.Element);
						while (nodeKindContentIterator42.MoveNext())
						{
							XPathItem current41 = nodeKindContentIterator42.Current;
							output.WriteItem(current41);
						}
					}
					output.WriteEndElement();
					output.WriteStartElement("", "SymbolicSignalEventDatabaseName", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
					output.WriteStringUnchecked("0");
					output.WriteEndAttributeUnchecked();
					ElementContentIterator elementContentIterator59;
					elementContentIterator59.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(41), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
					while (elementContentIterator59.MoveNext())
					{
						ElementContentIterator elementContentIterator60;
						elementContentIterator60.Create(elementContentIterator59.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(2), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
						while (elementContentIterator60.MoveNext())
						{
							Convert_8_to_9.<xsl:apply-templates>({urn:schemas-microsoft-com:xslt-debug}runtime, elementContentIterator60.Current);
						}
					}
					output.WriteEndElement();
					output.WriteStartElement("", "SymbolicSignalEventDatabasePath", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
					output.WriteStringUnchecked("0");
					output.WriteEndAttributeUnchecked();
					ElementContentIterator elementContentIterator61;
					elementContentIterator61.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(42), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
					while (elementContentIterator61.MoveNext())
					{
						ElementContentIterator elementContentIterator62;
						elementContentIterator62.Create(elementContentIterator61.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(2), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
						while (elementContentIterator62.MoveNext())
						{
							Convert_8_to_9.<xsl:apply-templates>({urn:schemas-microsoft-com:xslt-debug}runtime, elementContentIterator62.Current);
						}
					}
					output.WriteEndElement();
					output.WriteStartElement("", "SymbolicSignalEventHighValue", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
					output.WriteStringUnchecked("0");
					output.WriteEndAttributeUnchecked();
					ElementContentIterator elementContentIterator63;
					elementContentIterator63.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(43), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
					while (elementContentIterator63.MoveNext())
					{
						NodeKindContentIterator nodeKindContentIterator43;
						nodeKindContentIterator43.Create(elementContentIterator63.Current, XPathNodeType.Element);
						while (nodeKindContentIterator43.MoveNext())
						{
							XPathItem current42 = nodeKindContentIterator43.Current;
							output.WriteItem(current42);
						}
					}
					output.WriteEndElement();
					output.WriteStartElement("", "SymbolicSignalEventLowValue", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
					output.WriteStringUnchecked("0");
					output.WriteEndAttributeUnchecked();
					ElementContentIterator elementContentIterator64;
					elementContentIterator64.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(44), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
					while (elementContentIterator64.MoveNext())
					{
						NodeKindContentIterator nodeKindContentIterator44;
						nodeKindContentIterator44.Create(elementContentIterator64.Current, XPathNodeType.Element);
						while (nodeKindContentIterator44.MoveNext())
						{
							XPathItem current43 = nodeKindContentIterator44.Current;
							output.WriteItem(current43);
						}
					}
					output.WriteEndElement();
					output.WriteStartElement("", "SymbolicSignalEventMessageName", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
					output.WriteStringUnchecked("0");
					output.WriteEndAttributeUnchecked();
					ElementContentIterator elementContentIterator65;
					elementContentIterator65.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(45), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
					while (elementContentIterator65.MoveNext())
					{
						ElementContentIterator elementContentIterator66;
						elementContentIterator66.Create(elementContentIterator65.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(2), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
						while (elementContentIterator66.MoveNext())
						{
							Convert_8_to_9.<xsl:apply-templates>({urn:schemas-microsoft-com:xslt-debug}runtime, elementContentIterator66.Current);
						}
					}
					output.WriteEndElement();
					output.WriteStartElement("", "SymbolicSignalEventRelation", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
					output.WriteStringUnchecked("0");
					output.WriteEndAttributeUnchecked();
					ElementContentIterator elementContentIterator67;
					elementContentIterator67.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(46), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
					while (elementContentIterator67.MoveNext())
					{
						NodeKindContentIterator nodeKindContentIterator45;
						nodeKindContentIterator45.Create(elementContentIterator67.Current, XPathNodeType.Element);
						while (nodeKindContentIterator45.MoveNext())
						{
							XPathItem current44 = nodeKindContentIterator45.Current;
							output.WriteItem(current44);
						}
					}
					output.WriteEndElement();
					output.WriteStartElement("", "SymbolicSignalEventSignalName", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
					output.WriteStringUnchecked("0");
					output.WriteEndAttributeUnchecked();
					ElementContentIterator elementContentIterator68;
					elementContentIterator68.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(47), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
					while (elementContentIterator68.MoveNext())
					{
						ElementContentIterator elementContentIterator69;
						elementContentIterator69.Create(elementContentIterator68.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(2), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
						while (elementContentIterator69.MoveNext())
						{
							Convert_8_to_9.<xsl:apply-templates>({urn:schemas-microsoft-com:xslt-debug}runtime, elementContentIterator69.Current);
						}
					}
					output.WriteEndElement();
					output.WriteEndElementUnchecked("", "RecordTriggerEvent", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				}
				else if (string.Equals(a, "CANBusStatisticsTrigger"))
				{
					output.WriteStartElementUnchecked("", "RecordTriggerEvent", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
					output.WriteStringUnchecked("0");
					output.WriteEndAttributeUnchecked();
					output.WriteStartAttributeUnchecked("i", "type", "http://www.w3.org/2001/XMLSchema-instance");
					output.WriteStringUnchecked("CANBusStatisticsEvent");
					output.WriteEndAttributeUnchecked();
					output.WriteStartElement("", "CANBusStatisticsEventBusloadHigh", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
					output.WriteStringUnchecked("0");
					output.WriteEndAttributeUnchecked();
					ElementContentIterator elementContentIterator70;
					elementContentIterator70.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(48), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
					while (elementContentIterator70.MoveNext())
					{
						NodeKindContentIterator nodeKindContentIterator46;
						nodeKindContentIterator46.Create(elementContentIterator70.Current, XPathNodeType.Element);
						while (nodeKindContentIterator46.MoveNext())
						{
							XPathItem current45 = nodeKindContentIterator46.Current;
							output.WriteItem(current45);
						}
					}
					output.WriteEndElement();
					output.WriteStartElement("", "CANBusStatisticsEventBusloadLow", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
					output.WriteStringUnchecked("0");
					output.WriteEndAttributeUnchecked();
					ElementContentIterator elementContentIterator71;
					elementContentIterator71.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(49), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
					while (elementContentIterator71.MoveNext())
					{
						NodeKindContentIterator nodeKindContentIterator47;
						nodeKindContentIterator47.Create(elementContentIterator71.Current, XPathNodeType.Element);
						while (nodeKindContentIterator47.MoveNext())
						{
							XPathItem current46 = nodeKindContentIterator47.Current;
							output.WriteItem(current46);
						}
					}
					output.WriteEndElement();
					output.WriteStartElement("", "CANBusStatisticsEventBusloadRelation", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
					output.WriteStringUnchecked("0");
					output.WriteEndAttributeUnchecked();
					ElementContentIterator elementContentIterator72;
					elementContentIterator72.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(50), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
					while (elementContentIterator72.MoveNext())
					{
						NodeKindContentIterator nodeKindContentIterator48;
						nodeKindContentIterator48.Create(elementContentIterator72.Current, XPathNodeType.Element);
						while (nodeKindContentIterator48.MoveNext())
						{
							XPathItem current47 = nodeKindContentIterator48.Current;
							output.WriteItem(current47);
						}
					}
					output.WriteEndElement();
					output.WriteStartElement("", "CANBusStatisticsEventChannelNumber", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
					output.WriteStringUnchecked("0");
					output.WriteEndAttributeUnchecked();
					ElementContentIterator elementContentIterator73;
					elementContentIterator73.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(51), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
					while (elementContentIterator73.MoveNext())
					{
						NodeKindContentIterator nodeKindContentIterator49;
						nodeKindContentIterator49.Create(elementContentIterator73.Current, XPathNodeType.Element);
						while (nodeKindContentIterator49.MoveNext())
						{
							XPathItem current48 = nodeKindContentIterator49.Current;
							output.WriteItem(current48);
						}
					}
					output.WriteEndElement();
					output.WriteStartElement("", "CANBusStatisticsEventErrorFramesHigh", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
					output.WriteStringUnchecked("0");
					output.WriteEndAttributeUnchecked();
					ElementContentIterator elementContentIterator74;
					elementContentIterator74.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(52), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
					while (elementContentIterator74.MoveNext())
					{
						NodeKindContentIterator nodeKindContentIterator50;
						nodeKindContentIterator50.Create(elementContentIterator74.Current, XPathNodeType.Element);
						while (nodeKindContentIterator50.MoveNext())
						{
							XPathItem current49 = nodeKindContentIterator50.Current;
							output.WriteItem(current49);
						}
					}
					output.WriteEndElement();
					output.WriteStartElement("", "CANBusStatisticsEventErrorFramesLow", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
					output.WriteStringUnchecked("0");
					output.WriteEndAttributeUnchecked();
					ElementContentIterator elementContentIterator75;
					elementContentIterator75.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(53), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
					while (elementContentIterator75.MoveNext())
					{
						NodeKindContentIterator nodeKindContentIterator51;
						nodeKindContentIterator51.Create(elementContentIterator75.Current, XPathNodeType.Element);
						while (nodeKindContentIterator51.MoveNext())
						{
							XPathItem current50 = nodeKindContentIterator51.Current;
							output.WriteItem(current50);
						}
					}
					output.WriteEndElement();
					output.WriteStartElement("", "CANBusStatisticsEventErrorFramesRelation", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
					output.WriteStringUnchecked("0");
					output.WriteEndAttributeUnchecked();
					ElementContentIterator elementContentIterator76;
					elementContentIterator76.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(54), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
					while (elementContentIterator76.MoveNext())
					{
						NodeKindContentIterator nodeKindContentIterator52;
						nodeKindContentIterator52.Create(elementContentIterator76.Current, XPathNodeType.Element);
						while (nodeKindContentIterator52.MoveNext())
						{
							XPathItem current51 = nodeKindContentIterator52.Current;
							output.WriteItem(current51);
						}
					}
					output.WriteEndElement();
					output.WriteStartElement("", "CANBusStatisticsEventIsANDConjunction", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
					output.WriteStringUnchecked("0");
					output.WriteEndAttributeUnchecked();
					ElementContentIterator elementContentIterator77;
					elementContentIterator77.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(55), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
					while (elementContentIterator77.MoveNext())
					{
						NodeKindContentIterator nodeKindContentIterator53;
						nodeKindContentIterator53.Create(elementContentIterator77.Current, XPathNodeType.Element);
						while (nodeKindContentIterator53.MoveNext())
						{
							XPathItem current52 = nodeKindContentIterator53.Current;
							output.WriteItem(current52);
						}
					}
					output.WriteEndElement();
					output.WriteStartElement("", "CANBusStatisticsEventIsBusloadEnabled", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
					output.WriteStringUnchecked("0");
					output.WriteEndAttributeUnchecked();
					ElementContentIterator elementContentIterator78;
					elementContentIterator78.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(56), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
					while (elementContentIterator78.MoveNext())
					{
						NodeKindContentIterator nodeKindContentIterator54;
						nodeKindContentIterator54.Create(elementContentIterator78.Current, XPathNodeType.Element);
						while (nodeKindContentIterator54.MoveNext())
						{
							XPathItem current53 = nodeKindContentIterator54.Current;
							output.WriteItem(current53);
						}
					}
					output.WriteEndElement();
					output.WriteStartElement("", "CANBusStatisticsEventIsErrorFramesEnabled", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
					output.WriteStringUnchecked("0");
					output.WriteEndAttributeUnchecked();
					ElementContentIterator elementContentIterator79;
					elementContentIterator79.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(57), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
					while (elementContentIterator79.MoveNext())
					{
						NodeKindContentIterator nodeKindContentIterator55;
						nodeKindContentIterator55.Create(elementContentIterator79.Current, XPathNodeType.Element);
						while (nodeKindContentIterator55.MoveNext())
						{
							XPathItem current54 = nodeKindContentIterator55.Current;
							output.WriteItem(current54);
						}
					}
					output.WriteEndElement();
					output.WriteEndElementUnchecked("", "RecordTriggerEvent", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				}
				else if (string.Equals(a, "DigitalInputTrigger"))
				{
					output.WriteStartElementUnchecked("", "RecordTriggerEvent", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
					output.WriteStringUnchecked("0");
					output.WriteEndAttributeUnchecked();
					output.WriteStartAttributeUnchecked("i", "type", "http://www.w3.org/2001/XMLSchema-instance");
					output.WriteStringUnchecked("DigitalInputEvent");
					output.WriteEndAttributeUnchecked();
					output.WriteStartElement("", "DigitalInputEventDigitaInput", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
					output.WriteStringUnchecked("0");
					output.WriteEndAttributeUnchecked();
					ElementContentIterator elementContentIterator80;
					elementContentIterator80.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(58), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
					while (elementContentIterator80.MoveNext())
					{
						NodeKindContentIterator nodeKindContentIterator56;
						nodeKindContentIterator56.Create(elementContentIterator80.Current, XPathNodeType.Element);
						while (nodeKindContentIterator56.MoveNext())
						{
							XPathItem current55 = nodeKindContentIterator56.Current;
							output.WriteItem(current55);
						}
					}
					output.WriteEndElement();
					output.WriteStartElement("", "DigitalInputEventEdge", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
					output.WriteStringUnchecked("0");
					output.WriteEndAttributeUnchecked();
					ElementContentIterator elementContentIterator81;
					elementContentIterator81.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(59), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
					while (elementContentIterator81.MoveNext())
					{
						NodeKindContentIterator nodeKindContentIterator57;
						nodeKindContentIterator57.Create(elementContentIterator81.Current, XPathNodeType.Element);
						while (nodeKindContentIterator57.MoveNext())
						{
							XPathItem current56 = nodeKindContentIterator57.Current;
							output.WriteItem(current56);
						}
					}
					output.WriteEndElement();
					output.WriteEndElementUnchecked("", "RecordTriggerEvent", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				}
				else if (string.Equals(a, "KeyTrigger"))
				{
					output.WriteStartElementUnchecked("", "RecordTriggerEvent", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
					output.WriteStringUnchecked("0");
					output.WriteEndAttributeUnchecked();
					output.WriteStartAttributeUnchecked("i", "type", "http://www.w3.org/2001/XMLSchema-instance");
					output.WriteStringUnchecked("KeyEvent");
					output.WriteEndAttributeUnchecked();
					output.WriteStartElement("", "KeyEventIsOnPanel", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
					output.WriteStringUnchecked("0");
					output.WriteEndAttributeUnchecked();
					ElementContentIterator elementContentIterator82;
					elementContentIterator82.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(60), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
					while (elementContentIterator82.MoveNext())
					{
						NodeKindContentIterator nodeKindContentIterator58;
						nodeKindContentIterator58.Create(elementContentIterator82.Current, XPathNodeType.Element);
						while (nodeKindContentIterator58.MoveNext())
						{
							XPathItem current57 = nodeKindContentIterator58.Current;
							output.WriteItem(current57);
						}
					}
					output.WriteEndElement();
					output.WriteStartElement("", "KeyEventNumber", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
					output.WriteStringUnchecked("0");
					output.WriteEndAttributeUnchecked();
					ElementContentIterator elementContentIterator83;
					elementContentIterator83.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(61), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
					while (elementContentIterator83.MoveNext())
					{
						NodeKindContentIterator nodeKindContentIterator59;
						nodeKindContentIterator59.Create(elementContentIterator83.Current, XPathNodeType.Element);
						while (nodeKindContentIterator59.MoveNext())
						{
							XPathItem current58 = nodeKindContentIterator59.Current;
							output.WriteItem(current58);
						}
					}
					output.WriteEndElement();
					output.WriteEndElementUnchecked("", "RecordTriggerEvent", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				}
			}
			IL_2F34:
			output.WriteStartElement("", "RecordTriggerMemory", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			ElementContentIterator elementContentIterator84;
			elementContentIterator84.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(62), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
			while (elementContentIterator84.MoveNext())
			{
				NodeKindContentIterator nodeKindContentIterator60;
				nodeKindContentIterator60.Create(elementContentIterator84.Current, XPathNodeType.Element);
				while (nodeKindContentIterator60.MoveNext())
				{
					XPathItem current59 = nodeKindContentIterator60.Current;
					output.WriteItem(current59);
				}
			}
			output.WriteEndElement();
			output.WriteEndElement();
		}

		private static void 9">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current, string indent)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			output.WriteStartElement("", "WLANConfigurationStartWLANEvent", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("i", "http://www.w3.org/2001/XMLSchema-instance");
			output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.WriteStartAttributeUnchecked("i", "type", "http://www.w3.org/2001/XMLSchema-instance");
			output.WriteStringUnchecked("KeyEvent");
			output.WriteEndAttributeUnchecked();
			output.WriteStartElement("", "KeyEventIsOnPanel", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			ElementContentIterator elementContentIterator;
			elementContentIterator.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(60), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
			while (elementContentIterator.MoveNext())
			{
				NodeKindContentIterator nodeKindContentIterator;
				nodeKindContentIterator.Create(elementContentIterator.Current, XPathNodeType.Element);
				while (nodeKindContentIterator.MoveNext())
				{
					XPathItem current = nodeKindContentIterator.Current;
					output.WriteItem(current);
				}
			}
			output.WriteEndElement();
			output.WriteStartElement("", "KeyEventNumber", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			ElementContentIterator elementContentIterator2;
			elementContentIterator2.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(61), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3));
			while (elementContentIterator2.MoveNext())
			{
				NodeKindContentIterator nodeKindContentIterator2;
				nodeKindContentIterator2.Create(elementContentIterator2.Current, XPathNodeType.Element);
				while (nodeKindContentIterator2.MoveNext())
				{
					XPathItem current2 = nodeKindContentIterator2.Current;
					output.WriteItem(current2);
				}
			}
			output.WriteEndElement();
			output.WriteEndElement();
		}

		private static void 9">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current, string indent)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			Convert_8_to_9.<xsl:template name="PrettyPrintNodeRecursive">({urn:schemas-microsoft-com:xslt-debug}runtime, {urn:schemas-microsoft-com:xslt-debug}current, indent);
			output.WriteStartElement("", "LoggingConfigurationDiagnosticActionsConfiguration", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("i", "http://www.w3.org/2001/XMLSchema-instance");
			output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("", "DiagnosticActionsConfigurationDiagCommRestartTime", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("1");
			output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteEndElementUnchecked("", "DiagnosticActionsConfigurationDiagCommRestartTime", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartElementUnchecked("", "DiagnosticActionsConfigurationIsDiagCommRestartEnabled", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("true");
			output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteEndElementUnchecked("", "DiagnosticActionsConfigurationIsDiagCommRestartEnabled", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartElementUnchecked("", "DiagnosticActionsConfigurationRecordDiagCommMemories", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.WriteStartAttributeUnchecked("z", "Size", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("2");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("", "ValidatedProperty", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("true");
			output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteEndElementUnchecked("", "ValidatedProperty", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartElementUnchecked("", "ValidatedProperty", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("true");
			output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteEndElementUnchecked("", "ValidatedProperty", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteEndElementUnchecked("", "DiagnosticActionsConfigurationRecordDiagCommMemories", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartElementUnchecked("", "DiagnosticActionsConfigurationTriggeredSequences", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.WriteStartAttributeUnchecked("z", "Size", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteEndElementUnchecked("", "DiagnosticActionsConfigurationTriggeredSequences", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteEndElement();
			output.WriteStartElement("", "LoggingConfigurationDiagnosticsDatabaseConfiguration", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("i", "http://www.w3.org/2001/XMLSchema-instance");
			output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("", "DiagnosticsDatabaseConfigurationDatabaseList", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.WriteStartAttributeUnchecked("z", "Size", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteEndElementUnchecked("", "DiagnosticsDatabaseConfigurationDatabaseList", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteEndElement();
		}

		private static void <xsl:template name="PrettyPrintNodeRecursive">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current, string indent)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			Convert_8_to_9.<xsl:template name="newline">({urn:schemas-microsoft-com:xslt-debug}runtime);
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
							Convert_8_to_9.<xsl:apply-templates> (2)({urn:schemas-microsoft-com:xslt-debug}runtime, unionIterator.Current, indent + XsltConvert.ToString(Convert_8_to_9.indent-increment({urn:schemas-microsoft-com:xslt-debug}runtime)));
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
					Convert_8_to_9.<xsl:template name="newline">({urn:schemas-microsoft-com:xslt-debug}runtime);
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
			Convert_8_to_9.<xsl:template name="PrettyPrintNodeRecursive">({urn:schemas-microsoft-com:xslt-debug}runtime, {urn:schemas-microsoft-com:xslt-debug}current, indent);
		}

		private static void <xsl:apply-templates>(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator xPathNavigator)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			int arg_EC_0;
			if (xPathNavigator.NodeType == XPathNodeType.Element)
			{
				int num = (!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 2, 3)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 63, 3)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 64, 3)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 65, 3)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 66, 3)) ? -1 : 5) : 6) : 7) : 8) : 9;
				arg_EC_0 = ((num <= 4) ? 4 : num);
			}
			else
			{
				arg_EC_0 = (((1 << (int)xPathNavigator.NodeType & 112) == 0) ? ((xPathNavigator.NodeType != XPathNodeType.Comment) ? ((xPathNavigator.NodeType != XPathNodeType.ProcessingInstruction) ? -1 : 0) : 1) : ((!string.Equals(XsltFunctions.NormalizeSpace(xPathNavigator.Value), "")) ? 2 : 3));
			}
			switch (arg_EC_0)
			{
			case 0:
				Convert_8_to_9.<xsl:template match="comment() | processing-instruction()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, "");
				return;
			case 1:
				Convert_8_to_9.<xsl:template match="comment() | processing-instruction()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, "");
				return;
			case 2:
				Convert_8_to_9.<xsl:template match="text()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, "");
				return;
			case 3:
				break;
			case 4:
				Convert_8_to_9.<xsl:template match="*" priority="0.5">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, "");
				return;
			case 5:
				Convert_8_to_9.<xsl:template match="VLConfig:Trigger" priority="0.9">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, "");
				return;
			case 6:
				Convert_8_to_9.<xsl:template match="VLConfig:WLANConfigurationStartWLANEvent" priority="0.9">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, "");
				return;
			case 7:
				Convert_8_to_9.<xsl:template match="VLConfig:LoggingConfigurationDatabaseConfiguration" priority="0.9">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, "");
				return;
			case 8:
				Convert_8_to_9.<xsl:template match="VLConfig:FileFormatVersion" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime);
				return;
			case 9:
				Convert_8_to_9.<xsl:template match="VLConfig:Value" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			default:
				if ((1 << (int)xPathNavigator.NodeType & 508) == 0)
				{
					ContentIterator contentIterator;
					contentIterator.Create(xPathNavigator);
					while (contentIterator.MoveNext())
					{
						Convert_8_to_9.<xsl:apply-templates>({urn:schemas-microsoft-com:xslt-debug}runtime, contentIterator.Current);
					}
				}
				else if ((1 << (int)xPathNavigator.NodeType & 395) == 0)
				{
					output.WriteString(xPathNavigator.Value);
				}
				break;
			}
		}

		private static void <xsl:apply-templates> (2)(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator xPathNavigator, string indent)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			int arg_EC_0;
			if (xPathNavigator.NodeType == XPathNodeType.Element)
			{
				int num = (!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 2, 3)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 63, 3)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 64, 3)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 65, 3)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 66, 3)) ? -1 : 5) : 6) : 7) : 8) : 9;
				arg_EC_0 = ((num <= 4) ? 4 : num);
			}
			else
			{
				arg_EC_0 = (((1 << (int)xPathNavigator.NodeType & 112) == 0) ? ((xPathNavigator.NodeType != XPathNodeType.Comment) ? ((xPathNavigator.NodeType != XPathNodeType.ProcessingInstruction) ? -1 : 0) : 1) : ((!string.Equals(XsltFunctions.NormalizeSpace(xPathNavigator.Value), "")) ? 2 : 3));
			}
			switch (arg_EC_0)
			{
			case 0:
				Convert_8_to_9.<xsl:template match="comment() | processing-instruction()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, indent);
				return;
			case 1:
				Convert_8_to_9.<xsl:template match="comment() | processing-instruction()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, indent);
				return;
			case 2:
				Convert_8_to_9.<xsl:template match="text()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, indent);
				return;
			case 3:
				break;
			case 4:
				Convert_8_to_9.<xsl:template match="*" priority="0.5">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, indent);
				return;
			case 5:
				Convert_8_to_9.<xsl:template match="VLConfig:Trigger" priority="0.9">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, indent);
				return;
			case 6:
				Convert_8_to_9.<xsl:template match="VLConfig:WLANConfigurationStartWLANEvent" priority="0.9">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, indent);
				return;
			case 7:
				Convert_8_to_9.<xsl:template match="VLConfig:LoggingConfigurationDatabaseConfiguration" priority="0.9">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, indent);
				return;
			case 8:
				Convert_8_to_9.<xsl:template match="VLConfig:FileFormatVersion" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime);
				return;
			case 9:
				Convert_8_to_9.<xsl:template match="VLConfig:Value" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			default:
				if ((1 << (int)xPathNavigator.NodeType & 508) == 0)
				{
					ContentIterator contentIterator;
					contentIterator.Create(xPathNavigator);
					while (contentIterator.MoveNext())
					{
						Convert_8_to_9.<xsl:apply-templates>({urn:schemas-microsoft-com:xslt-debug}runtime, contentIterator.Current);
					}
				}
				else if ((1 << (int)xPathNavigator.NodeType & 395) == 0)
				{
					output.WriteString(xPathNavigator.Value);
				}
				break;
			}
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
