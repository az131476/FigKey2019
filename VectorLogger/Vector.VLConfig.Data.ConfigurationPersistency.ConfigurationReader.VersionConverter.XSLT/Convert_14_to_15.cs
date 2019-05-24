using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Xml.XPath;
using System.Xml.Xsl.Runtime;

namespace Vector.VLConfig.Data.ConfigurationPersistency.ConfigurationReader.VersionConverter.XSLT
{
	[GeneratedCode("System.Xml.Xsl.XslCompiledTransform", "2.0.0.0"), ComVisible(true)]
	public static class Convert_14_to_15
	{
		private static $ArrayType$1271 __staticData;

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
			26,
			0,
			0,
			0,
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
			44,
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
			73,
			115,
			83,
			116,
			97,
			114,
			116,
			84,
			104,
			114,
			101,
			101,
			71,
			79,
			110,
			69,
			118,
			101,
			110,
			116,
			69,
			110,
			97,
			98,
			108,
			101,
			100,
			41,
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
			73,
			115,
			80,
			97,
			114,
			116,
			105,
			97,
			108,
			68,
			111,
			119,
			110,
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
			34,
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
			65,
			110,
			97,
			108,
			111,
			103,
			73,
			110,
			112,
			117,
			116,
			78,
			117,
			109,
			98,
			101,
			114,
			42,
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
			73,
			115,
			83,
			116,
			97,
			114,
			116,
			87,
			76,
			65,
			78,
			79,
			110,
			69,
			118,
			101,
			110,
			116,
			69,
			110,
			97,
			98,
			108,
			101,
			100,
			49,
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
			73,
			115,
			83,
			116,
			97,
			114,
			116,
			87,
			76,
			65,
			78,
			111,
			114,
			51,
			71,
			79,
			110,
			83,
			104,
			117,
			116,
			100,
			111,
			119,
			110,
			69,
			110,
			97,
			98,
			108,
			101,
			100,
			52,
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
			73,
			115,
			87,
			76,
			65,
			78,
			68,
			111,
			119,
			110,
			108,
			111,
			97,
			100,
			67,
			108,
			97,
			115,
			115,
			105,
			102,
			105,
			99,
			97,
			116,
			105,
			111,
			110,
			69,
			110,
			97,
			98,
			108,
			101,
			100,
			51,
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
			73,
			115,
			87,
			76,
			65,
			78,
			68,
			111,
			119,
			110,
			108,
			111,
			97,
			100,
			68,
			114,
			105,
			118,
			101,
			82,
			101,
			99,
			111,
			114,
			100,
			101,
			114,
			69,
			110,
			97,
			98,
			108,
			101,
			100,
			48,
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
			73,
			115,
			87,
			76,
			65,
			78,
			68,
			111,
			119,
			110,
			108,
			111,
			97,
			100,
			82,
			105,
			110,
			103,
			98,
			117,
			102,
			102,
			101,
			114,
			69,
			110,
			97,
			98,
			108,
			101,
			100,
			42,
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
			73,
			115,
			87,
			76,
			65,
			78,
			70,
			97,
			108,
			108,
			98,
			97,
			99,
			107,
			84,
			111,
			51,
			71,
			69,
			110,
			97,
			98,
			108,
			101,
			100,
			56,
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
			73,
			115,
			87,
			76,
			65,
			78,
			111,
			114,
			51,
			71,
			68,
			111,
			119,
			110,
			108,
			111,
			97,
			100,
			67,
			108,
			97,
			115,
			115,
			105,
			102,
			105,
			99,
			97,
			116,
			105,
			111,
			110,
			69,
			110,
			97,
			98,
			108,
			101,
			100,
			55,
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
			73,
			115,
			87,
			76,
			65,
			78,
			111,
			114,
			51,
			71,
			68,
			111,
			119,
			110,
			108,
			111,
			97,
			100,
			68,
			114,
			105,
			118,
			101,
			82,
			101,
			99,
			111,
			114,
			100,
			101,
			114,
			69,
			110,
			97,
			98,
			108,
			101,
			100,
			52,
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
			73,
			115,
			87,
			76,
			65,
			78,
			111,
			114,
			51,
			71,
			68,
			111,
			119,
			110,
			108,
			111,
			97,
			100,
			82,
			105,
			110,
			103,
			98,
			117,
			102,
			102,
			101,
			114,
			69,
			110,
			97,
			98,
			108,
			101,
			100,
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
			33,
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
			84,
			104,
			114,
			101,
			101,
			71,
			69,
			118,
			101,
			110,
			116,
			17,
			75,
			101,
			121,
			69,
			118,
			101,
			110,
			116,
			73,
			115,
			79,
			110,
			80,
			97,
			110,
			101,
			108,
			14,
			75,
			101,
			121,
			69,
			118,
			101,
			110,
			116,
			78,
			117,
			109,
			98,
			101,
			114,
			54,
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
			73,
			115,
			84,
			104,
			114,
			101,
			101,
			71,
			68,
			111,
			119,
			110,
			108,
			111,
			97,
			100,
			67,
			108,
			97,
			115,
			115,
			105,
			102,
			105,
			99,
			97,
			116,
			105,
			111,
			110,
			69,
			110,
			97,
			98,
			108,
			101,
			100,
			53,
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
			73,
			115,
			84,
			104,
			114,
			101,
			101,
			71,
			68,
			111,
			119,
			110,
			108,
			111,
			97,
			100,
			68,
			114,
			105,
			118,
			101,
			82,
			101,
			99,
			111,
			114,
			100,
			101,
			114,
			69,
			110,
			97,
			98,
			108,
			101,
			100,
			50,
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
			73,
			115,
			84,
			104,
			114,
			101,
			101,
			71,
			68,
			111,
			119,
			110,
			108,
			111,
			97,
			100,
			82,
			105,
			110,
			103,
			98,
			117,
			102,
			102,
			101,
			114,
			69,
			110,
			97,
			98,
			108,
			101,
			100,
			44,
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
			84,
			104,
			114,
			101,
			101,
			71,
			82,
			105,
			110,
			103,
			98,
			117,
			102,
			102,
			101,
			114,
			115,
			84,
			111,
			68,
			111,
			119,
			110,
			108,
			111,
			97,
			100,
			42,
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
			87,
			76,
			65,
			78,
			82,
			105,
			110,
			103,
			98,
			117,
			102,
			102,
			101,
			114,
			115,
			84,
			111,
			68,
			111,
			119,
			110,
			108,
			111,
			97,
			100,
			46,
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
			87,
			76,
			65,
			78,
			111,
			114,
			51,
			71,
			82,
			105,
			110,
			103,
			98,
			117,
			102,
			102,
			101,
			114,
			115,
			84,
			111,
			68,
			111,
			119,
			110,
			108,
			111,
			97,
			100,
			37,
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
			3,
			0,
			0,
			0,
			0,
			1,
			2,
			1,
			3,
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
			3,
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
			19,
			87,
			76,
			65,
			78,
			83,
			101,
			116,
			116,
			105,
			110,
			103,
			115,
			83,
			116,
			97,
			114,
			116,
			51,
			71,
			27,
			87,
			76,
			65,
			78,
			83,
			101,
			116,
			116,
			105,
			110,
			103,
			115,
			80,
			97,
			114,
			116,
			105,
			97,
			108,
			68,
			111,
			119,
			110,
			108,
			111,
			97,
			100,
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
			Convert_14_to_15.Root({urn:schemas-microsoft-com:xslt-debug}runtime);
		}

		private static void Root(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			output.WriteStartRoot();
			Convert_14_to_15.<xsl:apply-templates> (2)({urn:schemas-microsoft-com:xslt-debug}runtime, {urn:schemas-microsoft-com:xslt-debug}runtime.ExternalContext.DefaultDataSource);
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
			Convert_14_to_15.<xsl:template name="newline">({urn:schemas-microsoft-com:xslt-debug}runtime);
			output.WriteString(indent);
			if (output.StartCopy({urn:schemas-microsoft-com:xslt-debug}current))
			{
				output.EndCopy({urn:schemas-microsoft-com:xslt-debug}current);
			}
		}

		private static void <xsl:template match="text()">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current, string indent)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			Convert_14_to_15.<xsl:template name="newline">({urn:schemas-microsoft-com:xslt-debug}runtime);
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
			output.WriteStringUnchecked("15");
			output.WriteEndElement();
		}

		private static void <xsl:template match="VLConfig:LoggingConfigurationWLANConfiguration" priority="1">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			output.WriteStartElement("", "LoggingConfigurationWLANConfiguration", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteNamespaceDeclarationUnchecked("i", "http://www.w3.org/2001/XMLSchema-instance");
			output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			ElementContentIterator elementContentIterator;
			elementContentIterator.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(4), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
			while (elementContentIterator.MoveNext())
			{
				XPathItem current = elementContentIterator.Current;
				output.WriteItem(current);
			}
			ElementContentIterator elementContentIterator2;
			elementContentIterator2.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(2), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
			while (elementContentIterator2.MoveNext())
			{
				XPathItem current2 = elementContentIterator2.Current;
				output.WriteItem(current2);
			}
			ElementContentIterator elementContentIterator3;
			elementContentIterator3.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(5), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
			while (elementContentIterator3.MoveNext())
			{
				XPathItem current3 = elementContentIterator3.Current;
				output.WriteItem(current3);
			}
			ElementContentIterator elementContentIterator4;
			elementContentIterator4.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(6), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
			while (elementContentIterator4.MoveNext())
			{
				XPathItem current4 = elementContentIterator4.Current;
				output.WriteItem(current4);
			}
			ElementContentIterator elementContentIterator5;
			elementContentIterator5.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(7), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
			while (elementContentIterator5.MoveNext())
			{
				XPathItem current5 = elementContentIterator5.Current;
				output.WriteItem(current5);
			}
			ElementContentIterator elementContentIterator6;
			elementContentIterator6.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(8), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
			while (elementContentIterator6.MoveNext())
			{
				XPathItem current6 = elementContentIterator6.Current;
				output.WriteItem(current6);
			}
			ElementContentIterator elementContentIterator7;
			elementContentIterator7.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(9), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
			while (elementContentIterator7.MoveNext())
			{
				XPathItem current7 = elementContentIterator7.Current;
				output.WriteItem(current7);
			}
			ElementContentIterator elementContentIterator8;
			elementContentIterator8.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(10), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
			while (elementContentIterator8.MoveNext())
			{
				XPathItem current8 = elementContentIterator8.Current;
				output.WriteItem(current8);
			}
			ElementContentIterator elementContentIterator9;
			elementContentIterator9.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(11), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
			while (elementContentIterator9.MoveNext())
			{
				XPathItem current9 = elementContentIterator9.Current;
				output.WriteItem(current9);
			}
			ElementContentIterator elementContentIterator10;
			elementContentIterator10.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(12), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
			while (elementContentIterator10.MoveNext())
			{
				XPathItem current10 = elementContentIterator10.Current;
				output.WriteItem(current10);
			}
			ElementContentIterator elementContentIterator11;
			elementContentIterator11.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(13), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
			while (elementContentIterator11.MoveNext())
			{
				XPathItem current11 = elementContentIterator11.Current;
				output.WriteItem(current11);
			}
			output.WriteStartElement("", "WLANConfigurationPartialDownload", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			if (string.Equals(Convert_14_to_15.WLANSettingsPartialDownload({urn:schemas-microsoft-com:xslt-debug}runtime), "true"))
			{
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteStringUnchecked("PartialDownloadOn");
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			}
			else
			{
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteStringUnchecked("PartialDownloadOff");
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			}
			output.WriteEndElement();
			ElementContentIterator elementContentIterator12;
			elementContentIterator12.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(14), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
			while (elementContentIterator12.MoveNext())
			{
				XPathItem current12 = elementContentIterator12.Current;
				output.WriteItem(current12);
			}
			output.WriteStartElementUnchecked("", "WLANConfigurationThreeGDataTransferTriggerConfiguration", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			if (string.Equals(Convert_14_to_15.WLANSettingsStart3G({urn:schemas-microsoft-com:xslt-debug}runtime), "true"))
			{
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "WLANConfigurationDataTranferTriggerList", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.WriteStartAttributeUnchecked("z", "Size", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("1");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "DataTransferTrigger", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
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
				output.WriteStartElement("", "ActionEvent", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.WriteStartAttributeUnchecked("i", "type", "http://www.w3.org/2001/XMLSchema-instance");
				output.WriteStringUnchecked("KeyEvent");
				output.WriteEndAttributeUnchecked();
				ElementContentIterator elementContentIterator13;
				elementContentIterator13.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(15), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
				while (elementContentIterator13.MoveNext())
				{
					ElementContentIterator elementContentIterator14;
					elementContentIterator14.Create(elementContentIterator13.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(16), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
					while (elementContentIterator14.MoveNext())
					{
						XPathItem current13 = elementContentIterator14.Current;
						output.WriteItem(current13);
					}
				}
				ElementContentIterator elementContentIterator15;
				elementContentIterator15.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(15), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
				while (elementContentIterator15.MoveNext())
				{
					ElementContentIterator elementContentIterator16;
					elementContentIterator16.Create(elementContentIterator15.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(17), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
					while (elementContentIterator16.MoveNext())
					{
						XPathItem current14 = elementContentIterator16.Current;
						output.WriteItem(current14);
					}
				}
				output.WriteEndElement();
				output.WriteStartElementUnchecked("", "ActionIsActive", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteStringUnchecked("true");
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "ActionIsActive", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "ActionStopType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("i", "type", "http://www.w3.org/2001/XMLSchema-instance");
				output.WriteStringUnchecked("StopImmediate");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteEndElementUnchecked("", "ActionStopType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "DataTransferTriggerDataTransferMode", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteStringUnchecked("StopWhileDataTransfer");
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "DataTransferTriggerDataTransferMode", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElement("", "DataTransferTriggerIsDownloadClassifEnabled", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				ElementContentIterator elementContentIterator17;
				elementContentIterator17.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(18), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
				while (elementContentIterator17.MoveNext())
				{
					ElementContentIterator elementContentIterator18;
					elementContentIterator18.Create(elementContentIterator17.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(0), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
					while (elementContentIterator18.MoveNext())
					{
						XPathItem current15 = elementContentIterator18.Current;
						output.WriteItem(current15);
					}
				}
				output.WriteEndElement();
				output.WriteStartElement("", "DataTransferTriggerIsDownloadDriveRecEnabled", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				ElementContentIterator elementContentIterator19;
				elementContentIterator19.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(19), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
				while (elementContentIterator19.MoveNext())
				{
					ElementContentIterator elementContentIterator20;
					elementContentIterator20.Create(elementContentIterator19.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(0), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
					while (elementContentIterator20.MoveNext())
					{
						XPathItem current16 = elementContentIterator20.Current;
						output.WriteItem(current16);
					}
				}
				output.WriteEndElement();
				output.WriteStartElement("", "DataTransferTriggerIsDownloadRingbufferEnabled", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				ElementContentIterator elementContentIterator21;
				elementContentIterator21.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(20), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
				while (elementContentIterator21.MoveNext())
				{
					ElementContentIterator elementContentIterator22;
					elementContentIterator22.Create(elementContentIterator21.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(0), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
					while (elementContentIterator22.MoveNext())
					{
						XPathItem current17 = elementContentIterator22.Current;
						output.WriteItem(current17);
					}
				}
				output.WriteEndElement();
				output.WriteStartElement("", "DataTransferTriggerMemoriesToDownload", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				ElementContentIterator elementContentIterator23;
				elementContentIterator23.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(21), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
				while (elementContentIterator23.MoveNext())
				{
					ElementContentIterator elementContentIterator24;
					elementContentIterator24.Create(elementContentIterator23.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(0), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
					while (elementContentIterator24.MoveNext())
					{
						XPathItem current18 = elementContentIterator24.Current;
						output.WriteItem(current18);
					}
				}
				output.WriteEndElement();
				output.WriteEndElementUnchecked("", "DataTransferTrigger", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "WLANConfigurationDataTranferTriggerList", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			}
			else
			{
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "WLANConfigurationDataTranferTriggerList", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.WriteStartAttributeUnchecked("z", "Size", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteEndElementUnchecked("", "WLANConfigurationDataTranferTriggerList", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			}
			output.WriteEndElementUnchecked("", "WLANConfigurationThreeGDataTransferTriggerConfiguration", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			ElementContentIterator elementContentIterator25;
			elementContentIterator25.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(22), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
			while (elementContentIterator25.MoveNext())
			{
				XPathItem current19 = elementContentIterator25.Current;
				output.WriteItem(current19);
			}
			ElementContentIterator elementContentIterator26;
			elementContentIterator26.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(23), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
			while (elementContentIterator26.MoveNext())
			{
				XPathItem current20 = elementContentIterator26.Current;
				output.WriteItem(current20);
			}
			output.WriteEndElement();
		}

		private static void <xsl:template match="node()">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current, string indent)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			Convert_14_to_15.<xsl:template name="newline">({urn:schemas-microsoft-com:xslt-debug}runtime);
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
							Convert_14_to_15.<xsl:apply-templates>({urn:schemas-microsoft-com:xslt-debug}runtime, unionIterator.Current, indent + XsltConvert.ToString(Convert_14_to_15.indent-increment({urn:schemas-microsoft-com:xslt-debug}runtime)));
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
					Convert_14_to_15.<xsl:template name="newline">({urn:schemas-microsoft-com:xslt-debug}runtime);
					output.WriteString(indent);
					output.EndCopy({urn:schemas-microsoft-com:xslt-debug}current);
				}
			}
			else
			{
				output.XsltCopyOf({urn:schemas-microsoft-com:xslt-debug}current);
			}
		}

		private static void <xsl:apply-templates>(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator xPathNavigator, string indent)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			int num = (xPathNavigator.NodeType != XPathNodeType.Element) ? (((1 << (int)xPathNavigator.NodeType & 112) == 0) ? ((xPathNavigator.NodeType != XPathNodeType.Comment) ? ((xPathNavigator.NodeType != XPathNodeType.ProcessingInstruction) ? -1 : 0) : 1) : ((!string.Equals(XsltFunctions.NormalizeSpace(xPathNavigator.Value), "")) ? 2 : 4)) : ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 24, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 25, 1)) ? -1 : 5) : 6);
			switch ((num <= 3) ? (((1 << (int)xPathNavigator.NodeType & 13) != 0) ? ((num <= -1) ? -1 : num) : 3) : num)
			{
			case 0:
				Convert_14_to_15.<xsl:template match="comment() | processing-instruction()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, indent);
				return;
			case 1:
				Convert_14_to_15.<xsl:template match="comment() | processing-instruction()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, indent);
				return;
			case 2:
				Convert_14_to_15.<xsl:template match="text()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, indent);
				return;
			case 3:
				Convert_14_to_15.<xsl:template match="node()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, indent);
				return;
			case 4:
				break;
			case 5:
				Convert_14_to_15.<xsl:template match="VLConfig:FileFormatVersion" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime);
				return;
			case 6:
				Convert_14_to_15.<xsl:template match="VLConfig:LoggingConfigurationWLANConfiguration" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			default:
				if ((1 << (int)xPathNavigator.NodeType & 508) == 0)
				{
					ContentIterator contentIterator;
					contentIterator.Create(xPathNavigator);
					while (contentIterator.MoveNext())
					{
						Convert_14_to_15.<xsl:apply-templates> (2)({urn:schemas-microsoft-com:xslt-debug}runtime, contentIterator.Current);
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
			int num = (xPathNavigator.NodeType != XPathNodeType.Element) ? (((1 << (int)xPathNavigator.NodeType & 112) == 0) ? ((xPathNavigator.NodeType != XPathNodeType.Comment) ? ((xPathNavigator.NodeType != XPathNodeType.ProcessingInstruction) ? -1 : 0) : 1) : ((!string.Equals(XsltFunctions.NormalizeSpace(xPathNavigator.Value), "")) ? 2 : 4)) : ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 24, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 25, 1)) ? -1 : 5) : 6);
			switch ((num <= 3) ? (((1 << (int)xPathNavigator.NodeType & 13) != 0) ? ((num <= -1) ? -1 : num) : 3) : num)
			{
			case 0:
				Convert_14_to_15.<xsl:template match="comment() | processing-instruction()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, "");
				return;
			case 1:
				Convert_14_to_15.<xsl:template match="comment() | processing-instruction()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, "");
				return;
			case 2:
				Convert_14_to_15.<xsl:template match="text()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, "");
				return;
			case 3:
				Convert_14_to_15.<xsl:template match="node()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, "");
				return;
			case 4:
				break;
			case 5:
				Convert_14_to_15.<xsl:template match="VLConfig:FileFormatVersion" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime);
				return;
			case 6:
				Convert_14_to_15.<xsl:template match="VLConfig:LoggingConfigurationWLANConfiguration" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			default:
				if ((1 << (int)xPathNavigator.NodeType & 508) == 0)
				{
					ContentIterator contentIterator;
					contentIterator.Create(xPathNavigator);
					while (contentIterator.MoveNext())
					{
						Convert_14_to_15.<xsl:apply-templates> (2)({urn:schemas-microsoft-com:xslt-debug}runtime, contentIterator.Current);
					}
				}
				else if ((1 << (int)xPathNavigator.NodeType & 395) == 0)
				{
					output.WriteString(xPathNavigator.Value);
				}
				break;
			}
		}

		private unsafe static string WLANSettingsStart3G(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime)
		{
			if (!{urn:schemas-microsoft-com:xslt-debug}runtime.IsGlobalComputed(1))
			{
				int arg_97_1 = 1;
				ContentMergeIterator contentMergeIterator;
				contentMergeIterator.Create({urn:schemas-microsoft-com:xslt-debug}runtime.GetNameFilter(0));
				XPathNavigator xPathNavigator;
				XPathNavigator arg_38_0 = xPathNavigator;
				XPathNavigator xPathNavigator2 = Convert_14_to_15.SyncToNavigator(xPathNavigator2, {urn:schemas-microsoft-com:xslt-debug}runtime.ExternalContext.DefaultDataSource);
				xPathNavigator2.MoveToRoot();
				xPathNavigator = Convert_14_to_15.SyncToNavigator(arg_38_0, xPathNavigator2);
				xPathNavigator.MoveToRoot();
				DescendantIterator descendantIterator;
				descendantIterator.Create(xPathNavigator, {urn:schemas-microsoft-com:xslt-debug}runtime.GetNameFilter(1), false);
				while (true)
				{
					ContentMergeIterator* arg_6F_0;
					XPathNavigator arg_6F_1;
					if (descendantIterator.MoveNext())
					{
						arg_6F_0 = ref contentMergeIterator;
						arg_6F_1 = descendantIterator.Current;
					}
					else
					{
						arg_6F_0 = ref contentMergeIterator;
						arg_6F_1 = null;
					}
					switch (arg_6F_0.MoveNext(arg_6F_1))
					{
					case IteratorResult.NoMoreNodes:
						goto IL_92;
					case IteratorResult.NeedInputNode:
						continue;
					}
					break;
				}
				object arg_97_2 = contentMergeIterator.Current.Value;
				goto IL_97;
				IL_92:
				arg_97_2 = "";
				IL_97:
				{urn:schemas-microsoft-com:xslt-debug}runtime.SetGlobalValue(arg_97_1, arg_97_2);
			}
			return (string){urn:schemas-microsoft-com:xslt-debug}runtime.GetGlobalValue(1);
		}

		private unsafe static string WLANSettingsPartialDownload(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime)
		{
			if (!{urn:schemas-microsoft-com:xslt-debug}runtime.IsGlobalComputed(2))
			{
				int arg_97_1 = 2;
				ContentMergeIterator contentMergeIterator;
				contentMergeIterator.Create({urn:schemas-microsoft-com:xslt-debug}runtime.GetNameFilter(0));
				XPathNavigator xPathNavigator;
				XPathNavigator arg_38_0 = xPathNavigator;
				XPathNavigator xPathNavigator2 = Convert_14_to_15.SyncToNavigator(xPathNavigator2, {urn:schemas-microsoft-com:xslt-debug}runtime.ExternalContext.DefaultDataSource);
				xPathNavigator2.MoveToRoot();
				xPathNavigator = Convert_14_to_15.SyncToNavigator(arg_38_0, xPathNavigator2);
				xPathNavigator.MoveToRoot();
				DescendantIterator descendantIterator;
				descendantIterator.Create(xPathNavigator, {urn:schemas-microsoft-com:xslt-debug}runtime.GetNameFilter(2), false);
				while (true)
				{
					ContentMergeIterator* arg_6F_0;
					XPathNavigator arg_6F_1;
					if (descendantIterator.MoveNext())
					{
						arg_6F_0 = ref contentMergeIterator;
						arg_6F_1 = descendantIterator.Current;
					}
					else
					{
						arg_6F_0 = ref contentMergeIterator;
						arg_6F_1 = null;
					}
					switch (arg_6F_0.MoveNext(arg_6F_1))
					{
					case IteratorResult.NoMoreNodes:
						goto IL_92;
					case IteratorResult.NeedInputNode:
						continue;
					}
					break;
				}
				object arg_97_2 = contentMergeIterator.Current.Value;
				goto IL_97;
				IL_92:
				arg_97_2 = "";
				IL_97:
				{urn:schemas-microsoft-com:xslt-debug}runtime.SetGlobalValue(arg_97_1, arg_97_2);
			}
			return (string){urn:schemas-microsoft-com:xslt-debug}runtime.GetGlobalValue(2);
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
