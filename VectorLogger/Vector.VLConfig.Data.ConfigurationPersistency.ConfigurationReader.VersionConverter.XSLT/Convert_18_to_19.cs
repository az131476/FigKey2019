using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Xml.XPath;
using System.Xml.Xsl.Runtime;

namespace Vector.VLConfig.Data.ConfigurationPersistency.ConfigurationReader.VersionConverter.XSLT
{
	[GeneratedCode("System.Xml.Xsl.XslCompiledTransform", "2.0.0.0"), ComVisible(true)]
	public static class Convert_18_to_19
	{
		private static $ArrayType$1191 __staticData;

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
			37,
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
			4,
			83,
			105,
			122,
			101,
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
			9,
			109,
			95,
			65,
			100,
			100,
			114,
			101,
			115,
			115,
			50,
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
			83,
			121,
			115,
			116,
			101,
			109,
			46,
			78,
			101,
			116,
			36,
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
			77,
			97,
			112,
			112,
			105,
			110,
			103,
			77,
			111,
			100,
			101,
			35,
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
			67,
			97,
			110,
			67,
			104,
			97,
			110,
			110,
			101,
			108,
			35,
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
			110,
			112,
			117,
			116,
			115,
			76,
			105,
			115,
			116,
			26,
			71,
			80,
			83,
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
			67,
			65,
			78,
			67,
			104,
			97,
			110,
			110,
			101,
			108,
			36,
			71,
			80,
			83,
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
			69,
			120,
			116,
			101,
			110,
			100,
			101,
			100,
			83,
			116,
			97,
			114,
			116,
			67,
			65,
			78,
			73,
			100,
			24,
			71,
			80,
			83,
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
			65,
			99,
			116,
			105,
			118,
			101,
			26,
			71,
			80,
			83,
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
			67,
			65,
			78,
			73,
			100,
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
			17,
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
			78,
			97,
			109,
			101,
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
			71,
			80,
			83,
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
			45,
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
			27,
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
			117,
			98,
			110,
			101,
			116,
			77,
			97,
			115,
			107,
			27,
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
			77,
			76,
			115,
			101,
			114,
			118,
			101,
			114,
			73,
			112,
			25,
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
			76,
			111,
			103,
			103,
			101,
			114,
			73,
			112,
			26,
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
			71,
			97,
			116,
			101,
			119,
			97,
			121,
			73,
			112,
			36,
			73,
			110,
			116,
			101,
			114,
			102,
			97,
			99,
			101,
			77,
			111,
			100,
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
			83,
			117,
			98,
			110,
			101,
			116,
			77,
			97,
			115,
			107,
			35,
			73,
			110,
			116,
			101,
			114,
			102,
			97,
			99,
			101,
			77,
			111,
			100,
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
			73,
			112,
			65,
			100,
			100,
			114,
			101,
			115,
			115,
			20,
			67,
			99,
			112,
			88,
			99,
			112,
			69,
			99,
			117,
			85,
			115,
			101,
			68,
			98,
			80,
			97,
			114,
			97,
			109,
			115,
			29,
			67,
			99,
			112,
			88,
			99,
			112,
			69,
			99,
			117,
			83,
			101,
			110,
			100,
			83,
			101,
			116,
			83,
			101,
			115,
			115,
			105,
			111,
			110,
			83,
			116,
			97,
			116,
			117,
			115,
			21,
			67,
			99,
			112,
			88,
			99,
			112,
			69,
			99,
			117,
			69,
			116,
			104,
			101,
			114,
			110,
			101,
			116,
			80,
			111,
			114,
			116,
			46,
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
			23,
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
			76,
			69,
			68,
			76,
			105,
			115,
			116,
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
			1,
			0,
			0,
			0,
			0,
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
			Convert_18_to_19.Root({urn:schemas-microsoft-com:xslt-debug}runtime);
		}

		private static void Root(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			output.WriteStartRoot();
			Convert_18_to_19.<xsl:apply-templates> (2)({urn:schemas-microsoft-com:xslt-debug}runtime, {urn:schemas-microsoft-com:xslt-debug}runtime.ExternalContext.DefaultDataSource);
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
			Convert_18_to_19.<xsl:template name="newline">({urn:schemas-microsoft-com:xslt-debug}runtime);
			output.WriteString(indent);
			if (output.StartCopy({urn:schemas-microsoft-com:xslt-debug}current))
			{
				output.EndCopy({urn:schemas-microsoft-com:xslt-debug}current);
			}
		}

		private static void <xsl:template match="text()">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current, string indent)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			Convert_18_to_19.<xsl:template name="newline">({urn:schemas-microsoft-com:xslt-debug}runtime);
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
			output.WriteNamespaceDeclarationUnchecked("d5p1", "http://schemas.datacontract.org/2004/07/System.Net");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("19");
			output.WriteEndElement();
		}

		private static void <xsl:template match="VLConfig:LEDConfigurationLEDList" priority="1">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			if (string.Equals(Convert_18_to_19.LoggerTypeName({urn:schemas-microsoft-com:xslt-debug}runtime), "GL2000") || string.Equals(Convert_18_to_19.LoggerTypeName({urn:schemas-microsoft-com:xslt-debug}runtime), "GL3000") || string.Equals(Convert_18_to_19.LoggerTypeName({urn:schemas-microsoft-com:xslt-debug}runtime), "GL4000"))
			{
				output.WriteStartElement("", "LEDConfigurationLEDList", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteNamespaceDeclarationUnchecked("i", "http://www.w3.org/2001/XMLSchema-instance");
				output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteNamespaceDeclarationUnchecked("d5p1", "http://schemas.datacontract.org/2004/07/System.Net");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.WriteStartAttributeUnchecked("z", "Size", "http://schemas.microsoft.com/2003/10/Serialization/");
				XmlQueryOutput arg_11C_0 = output;
				XPathNavigator xPathNavigator = Convert_18_to_19.SyncToNavigator(xPathNavigator, {urn:schemas-microsoft-com:xslt-debug}current);
				arg_11C_0.WriteStringUnchecked(XsltConvert.ToString(XsltConvert.ToDouble((!xPathNavigator.MoveToAttribute({urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(2), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3))) ? "" : xPathNavigator.Value) + 4.0));
				output.WriteEndAttributeUnchecked();
				NodeKindContentIterator nodeKindContentIterator;
				nodeKindContentIterator.Create({urn:schemas-microsoft-com:xslt-debug}current, XPathNodeType.Element);
				while (nodeKindContentIterator.MoveNext())
				{
					XPathItem current = nodeKindContentIterator.Current;
					output.WriteItem(current);
				}
				output.WriteStartElement("", "ValidatedProperty", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteStringUnchecked("Disabled");
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElement();
				output.WriteStartElementUnchecked("", "ValidatedProperty", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteStringUnchecked("Disabled");
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "ValidatedProperty", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "ValidatedProperty", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteStringUnchecked("Disabled");
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "ValidatedProperty", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "ValidatedProperty", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteStringUnchecked("Disabled");
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "ValidatedProperty", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElement();
			}
			else
			{
				output.XsltCopyOf({urn:schemas-microsoft-com:xslt-debug}current);
			}
		}

		private static void <xsl:template match="VLConfig:HardwareConfigurationDigitalInputConfiguration" priority="1">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			output.XsltCopyOf({urn:schemas-microsoft-com:xslt-debug}current);
			output.WriteStartElement("", "HardwareConfigurationEthernetConfiguration", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteNamespaceDeclarationUnchecked("i", "http://www.w3.org/2001/XMLSchema-instance");
			output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("d5p1", "http://schemas.datacontract.org/2004/07/System.Net");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("", "EthernetConfigurationEth1Ip", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("192.168.165.10");
			output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteEndElementUnchecked("", "EthernetConfigurationEth1Ip", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartElementUnchecked("", "EthernetConfigurationEth1KeepAwake", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("false");
			output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteEndElementUnchecked("", "EthernetConfigurationEth1KeepAwake", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteEndElement();
		}

		private static void <xsl:template match="VLConfig:CcpXcpEcuEthernetPort" priority="1">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			output.XsltCopyOf({urn:schemas-microsoft-com:xslt-debug}current);
			output.WriteStartElement("", "CcpXcpEcuEthernetPort2", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteNamespaceDeclarationUnchecked("i", "http://www.w3.org/2001/XMLSchema-instance");
			output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("d5p1", "http://schemas.datacontract.org/2004/07/System.Net");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("5554");
			output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteEndElement();
		}

		private static void <xsl:template match="VLConfig:CcpXcpEcuSendSetSessionStatus" priority="1">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			output.XsltCopyOf({urn:schemas-microsoft-com:xslt-debug}current);
			output.WriteStartElement("", "CcpXcpEcuTransportLayerInstanceName", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteNamespaceDeclarationUnchecked("i", "http://www.w3.org/2001/XMLSchema-instance");
			output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("d5p1", "http://schemas.datacontract.org/2004/07/System.Net");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteEndElement();
		}

		private static void <xsl:template match="VLConfig:CcpXcpEcuUseDbParams" priority="1">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			output.XsltCopyOf({urn:schemas-microsoft-com:xslt-debug}current);
			output.WriteStartElement("", "CcpXcpEcuUseVxModule", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteNamespaceDeclarationUnchecked("i", "http://www.w3.org/2001/XMLSchema-instance");
			output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("d5p1", "http://schemas.datacontract.org/2004/07/System.Net");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("false");
			output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteEndElement();
		}

		private static void <xsl:template match="VLConfig:InterfaceModeConfigurationIpAddress" priority="1">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			output.WriteStartElement("", "InterfaceModeConfigurationIpAddress", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteNamespaceDeclarationUnchecked("i", "http://www.w3.org/2001/XMLSchema-instance");
			output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("d5p1", "http://schemas.datacontract.org/2004/07/System.Net");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.WriteStartElement("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			ElementContentIterator elementContentIterator;
			elementContentIterator.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(4), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
			Convert_18_to_19.<xsl:template name="DecNumberToIPStrReverse">({urn:schemas-microsoft-com:xslt-debug}runtime, XsltConvert.ToDouble((!elementContentIterator.MoveNext()) ? "" : elementContentIterator.Current.Value));
			output.WriteEndElement();
			output.WriteEndElement();
		}

		private static void <xsl:template match="VLConfig:InterfaceModeConfigurationSubnetMask" priority="1">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			output.WriteStartElement("", "InterfaceModeConfigurationSubnetMask", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteNamespaceDeclarationUnchecked("i", "http://www.w3.org/2001/XMLSchema-instance");
			output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("d5p1", "http://schemas.datacontract.org/2004/07/System.Net");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.WriteStartElement("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			ElementContentIterator elementContentIterator;
			elementContentIterator.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(4), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
			Convert_18_to_19.<xsl:template name="DecNumberToIPStrReverse">({urn:schemas-microsoft-com:xslt-debug}runtime, XsltConvert.ToDouble((!elementContentIterator.MoveNext()) ? "" : elementContentIterator.Current.Value));
			output.WriteEndElement();
			output.WriteEndElement();
		}

		private static void <xsl:template name="DecNumberToIPStrReverse">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, double DecIPValue)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			double num = Math.Floor(DecIPValue / 16777216.0);
			double num2 = Math.Floor((DecIPValue - num * 16777216.0) / 65536.0);
			output.WriteString(XsltConvert.ToString(num));
			output.WriteString(".");
			output.WriteString(XsltConvert.ToString(num2));
			output.WriteString(".");
			output.WriteString(XsltConvert.ToString(Math.Floor((DecIPValue - num * 16777216.0 - num2 * 65536.0) / 256.0)));
			output.WriteString(".");
			output.WriteString(XsltConvert.ToString(Math.Floor(DecIPValue % 256.0)));
		}

		private static void <xsl:template match="VLConfig:WLANConfigurationGatewayIp" priority="1">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			output.WriteStartElement("", "WLANConfigurationGatewayIp", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteNamespaceDeclarationUnchecked("i", "http://www.w3.org/2001/XMLSchema-instance");
			output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("d5p1", "http://schemas.datacontract.org/2004/07/System.Net");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.WriteStartElement("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			ElementContentIterator elementContentIterator;
			elementContentIterator.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(4), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
			string arg_11B_0;
			while (elementContentIterator.MoveNext())
			{
				ElementContentIterator elementContentIterator2;
				elementContentIterator2.Create(elementContentIterator.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(5), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(6));
				if (elementContentIterator2.MoveNext())
				{
					arg_11B_0 = elementContentIterator2.Current.Value;
					IL_11B:
					Convert_18_to_19.<xsl:template name="DecNumberToIPStr">({urn:schemas-microsoft-com:xslt-debug}runtime, XsltConvert.ToDouble(arg_11B_0));
					output.WriteEndElement();
					output.WriteEndElement();
					return;
				}
			}
			arg_11B_0 = "";
			goto IL_11B;
		}

		private static void <xsl:template match="VLConfig:WLANConfigurationLoggerIp" priority="1">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			output.WriteStartElement("", "WLANConfigurationLoggerIp", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteNamespaceDeclarationUnchecked("i", "http://www.w3.org/2001/XMLSchema-instance");
			output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("d5p1", "http://schemas.datacontract.org/2004/07/System.Net");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.WriteStartElement("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			ElementContentIterator elementContentIterator;
			elementContentIterator.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(4), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
			string arg_11B_0;
			while (elementContentIterator.MoveNext())
			{
				ElementContentIterator elementContentIterator2;
				elementContentIterator2.Create(elementContentIterator.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(5), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(6));
				if (elementContentIterator2.MoveNext())
				{
					arg_11B_0 = elementContentIterator2.Current.Value;
					IL_11B:
					Convert_18_to_19.<xsl:template name="DecNumberToIPStr">({urn:schemas-microsoft-com:xslt-debug}runtime, XsltConvert.ToDouble(arg_11B_0));
					output.WriteEndElement();
					output.WriteEndElement();
					return;
				}
			}
			arg_11B_0 = "";
			goto IL_11B;
		}

		private static void <xsl:template match="VLConfig:WLANConfigurationMLserverIp" priority="1">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			output.WriteStartElement("", "WLANConfigurationMLserverIp", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteNamespaceDeclarationUnchecked("i", "http://www.w3.org/2001/XMLSchema-instance");
			output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("d5p1", "http://schemas.datacontract.org/2004/07/System.Net");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.WriteStartElement("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			ElementContentIterator elementContentIterator;
			elementContentIterator.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(4), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
			string arg_11B_0;
			while (elementContentIterator.MoveNext())
			{
				ElementContentIterator elementContentIterator2;
				elementContentIterator2.Create(elementContentIterator.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(5), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(6));
				if (elementContentIterator2.MoveNext())
				{
					arg_11B_0 = elementContentIterator2.Current.Value;
					IL_11B:
					Convert_18_to_19.<xsl:template name="DecNumberToIPStr">({urn:schemas-microsoft-com:xslt-debug}runtime, XsltConvert.ToDouble(arg_11B_0));
					output.WriteEndElement();
					output.WriteEndElement();
					return;
				}
			}
			arg_11B_0 = "";
			goto IL_11B;
		}

		private static void <xsl:template match="VLConfig:WLANConfigurationSubnetMask" priority="1">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			output.WriteStartElement("", "WLANConfigurationSubnetMask", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteNamespaceDeclarationUnchecked("i", "http://www.w3.org/2001/XMLSchema-instance");
			output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("d5p1", "http://schemas.datacontract.org/2004/07/System.Net");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.WriteStartElement("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			ElementContentIterator elementContentIterator;
			elementContentIterator.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(4), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
			string arg_11B_0;
			while (elementContentIterator.MoveNext())
			{
				ElementContentIterator elementContentIterator2;
				elementContentIterator2.Create(elementContentIterator.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(5), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(6));
				if (elementContentIterator2.MoveNext())
				{
					arg_11B_0 = elementContentIterator2.Current.Value;
					IL_11B:
					Convert_18_to_19.<xsl:template name="DecNumberToIPStr">({urn:schemas-microsoft-com:xslt-debug}runtime, XsltConvert.ToDouble(arg_11B_0));
					output.WriteEndElement();
					output.WriteEndElement();
					return;
				}
			}
			arg_11B_0 = "";
			goto IL_11B;
		}

		private static void <xsl:template name="DecNumberToIPStr">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, double DecIPValue)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			double num = Math.Floor(DecIPValue / 16777216.0);
			double num2 = Math.Floor((DecIPValue - num * 16777216.0) / 65536.0);
			output.WriteString(XsltConvert.ToString(Math.Floor(DecIPValue % 256.0)));
			output.WriteString(".");
			output.WriteString(XsltConvert.ToString(Math.Floor((DecIPValue - num * 16777216.0 - num2 * 65536.0) / 256.0)));
			output.WriteString(".");
			output.WriteString(XsltConvert.ToString(num2));
			output.WriteString(".");
			output.WriteString(XsltConvert.ToString(num));
		}

		private static void <xsl:template match="VLConfig:HardwareConfigurationAnalogInputConfiguration" priority="1">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			output.WriteStartElement("", "HardwareConfigurationAnalogInputConfiguration", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteNamespaceDeclarationUnchecked("i", "http://www.w3.org/2001/XMLSchema-instance");
			output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("d5p1", "http://schemas.datacontract.org/2004/07/System.Net");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("", "AnalogInputsConfigurationAveraging", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("true");
			output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteEndElementUnchecked("", "AnalogInputsConfigurationAveraging", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartElement("", "AnalogInputsConfigurationCANMappingMode", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			ElementContentIterator elementContentIterator;
			elementContentIterator.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(7), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
			while (elementContentIterator.MoveNext())
			{
				ElementContentIterator elementContentIterator2;
				elementContentIterator2.Create(elementContentIterator.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(4), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
				while (elementContentIterator2.MoveNext())
				{
					XPathItem current = elementContentIterator2.Current;
					output.WriteItem(current);
				}
			}
			output.WriteEndElement();
			ElementContentIterator elementContentIterator3;
			elementContentIterator3.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(8), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
			while (elementContentIterator3.MoveNext())
			{
				XPathItem current2 = elementContentIterator3.Current;
				output.WriteItem(current2);
			}
			ElementContentIterator elementContentIterator4;
			elementContentIterator4.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(9), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
			while (elementContentIterator4.MoveNext())
			{
				XPathItem current3 = elementContentIterator4.Current;
				output.WriteItem(current3);
			}
			output.WriteStartElementUnchecked("", "AnalogInputsConfigurationMapToCANMessage", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("true");
			output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteEndElementUnchecked("", "AnalogInputsConfigurationMapToCANMessage", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartElementUnchecked("", "AnalogInputsConfigurationMapToSystemChanel", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("false");
			output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteEndElementUnchecked("", "AnalogInputsConfigurationMapToSystemChanel", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteEndElement();
		}

		private static void <xsl:template match="VLConfig:HardwareConfigurationGPSConfiguration" priority="1">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			output.WriteStartElement("", "HardwareConfigurationGPSConfiguration", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteNamespaceDeclarationUnchecked("i", "http://www.w3.org/2001/XMLSchema-instance");
			output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("d5p1", "http://schemas.datacontract.org/2004/07/System.Net");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			ElementContentIterator elementContentIterator;
			elementContentIterator.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(10), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
			while (elementContentIterator.MoveNext())
			{
				XPathItem current = elementContentIterator.Current;
				output.WriteItem(current);
			}
			output.WriteStartElement("", "GPSConfigurationDatabase", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteEndElement();
			ElementContentIterator elementContentIterator2;
			elementContentIterator2.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(11), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
			while (elementContentIterator2.MoveNext())
			{
				XPathItem current2 = elementContentIterator2.Current;
				output.WriteItem(current2);
			}
			output.WriteStartElementUnchecked("", "GPSConfigurationMapToCANMessage", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			ElementContentIterator elementContentIterator3;
			elementContentIterator3.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(12), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
			while (elementContentIterator3.MoveNext())
			{
				ElementContentIterator elementContentIterator4;
				elementContentIterator4.Create(elementContentIterator3.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(4), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
				while (elementContentIterator4.MoveNext())
				{
					XPathItem current3 = elementContentIterator4.Current;
					output.WriteItem(current3);
				}
			}
			output.WriteStartElement("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("false");
			output.WriteEndElement();
			output.WriteEndElementUnchecked("", "GPSConfigurationMapToCANMessage", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartElementUnchecked("", "GPSConfigurationMapToSystemChannel", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("false");
			output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteEndElementUnchecked("", "GPSConfigurationMapToSystemChannel", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			ElementContentIterator elementContentIterator5;
			elementContentIterator5.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(13), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
			while (elementContentIterator5.MoveNext())
			{
				XPathItem current4 = elementContentIterator5.Current;
				output.WriteItem(current4);
			}
			output.WriteEndElement();
		}

		private static void <xsl:template match="VLConfig:RecordTrigger" priority="1">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			output.WriteStartElement("", "RecordTrigger", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteNamespaceDeclarationUnchecked("i", "http://www.w3.org/2001/XMLSchema-instance");
			output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("d5p1", "http://schemas.datacontract.org/2004/07/System.Net");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.WriteStartElement("", "ActionComment", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			ElementContentIterator elementContentIterator;
			elementContentIterator.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(14), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
			while (elementContentIterator.MoveNext())
			{
				ElementContentIterator elementContentIterator2;
				elementContentIterator2.Create(elementContentIterator.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(4), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
				while (elementContentIterator2.MoveNext())
				{
					XPathItem current = elementContentIterator2.Current;
					output.WriteItem(current);
				}
			}
			output.WriteEndElement();
			output.WriteStartElement("", "ActionEvent", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.WriteStartAttributeUnchecked("i", "type", "http://www.w3.org/2001/XMLSchema-instance");
			XmlQueryOutput arg_1CA_0 = output;
			ElementContentIterator elementContentIterator3;
			elementContentIterator3.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(15), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
			string arg_1CA_1;
			while (elementContentIterator3.MoveNext())
			{
				XPathNavigator xPathNavigator = Convert_18_to_19.SyncToNavigator(xPathNavigator, elementContentIterator3.Current);
				if (xPathNavigator.MoveToAttribute({urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(16), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(17)))
				{
					arg_1CA_1 = xPathNavigator.Value;
					IL_1CA:
					arg_1CA_0.WriteStringUnchecked(arg_1CA_1);
					output.WriteEndAttributeUnchecked();
					ElementContentIterator elementContentIterator4;
					elementContentIterator4.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(15), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
					while (elementContentIterator4.MoveNext())
					{
						NodeKindContentIterator nodeKindContentIterator;
						nodeKindContentIterator.Create(elementContentIterator4.Current, XPathNodeType.Element);
						while (nodeKindContentIterator.MoveNext())
						{
							XPathItem current2 = nodeKindContentIterator.Current;
							output.WriteItem(current2);
						}
					}
					output.WriteEndElement();
					output.WriteStartElement("", "ActionIsActive", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
					output.WriteStringUnchecked("0");
					output.WriteEndAttributeUnchecked();
					ElementContentIterator elementContentIterator5;
					elementContentIterator5.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(18), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
					while (elementContentIterator5.MoveNext())
					{
						ElementContentIterator elementContentIterator6;
						elementContentIterator6.Create(elementContentIterator5.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(4), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
						while (elementContentIterator6.MoveNext())
						{
							XPathItem current3 = elementContentIterator6.Current;
							output.WriteItem(current3);
						}
					}
					output.WriteEndElement();
					output.WriteStartElementUnchecked("", "ActionStopType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.WriteStartAttributeUnchecked("i", "nil", "http://www.w3.org/2001/XMLSchema-instance");
					output.WriteStringUnchecked("true");
					output.WriteEndAttributeUnchecked();
					output.StartElementContentUnchecked();
					output.WriteEndElementUnchecked("", "ActionStopType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					ElementContentIterator elementContentIterator7;
					elementContentIterator7.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(19), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
					while (elementContentIterator7.MoveNext())
					{
						XPathItem current4 = elementContentIterator7.Current;
						output.WriteItem(current4);
					}
					ElementContentIterator elementContentIterator8;
					elementContentIterator8.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(20), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
					while (elementContentIterator8.MoveNext())
					{
						XPathItem current5 = elementContentIterator8.Current;
						output.WriteItem(current5);
					}
					ElementContentIterator elementContentIterator9;
					elementContentIterator9.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(21), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
					while (elementContentIterator9.MoveNext())
					{
						XPathItem current6 = elementContentIterator9.Current;
						output.WriteItem(current6);
					}
					output.WriteEndElement();
					return;
				}
			}
			arg_1CA_1 = "";
			goto IL_1CA;
		}

		private static void <xsl:template match="node()">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current, string indent)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			Convert_18_to_19.<xsl:template name="newline">({urn:schemas-microsoft-com:xslt-debug}runtime);
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
							Convert_18_to_19.<xsl:apply-templates>({urn:schemas-microsoft-com:xslt-debug}runtime, unionIterator.Current, indent + XsltConvert.ToString(Convert_18_to_19.indent-increment({urn:schemas-microsoft-com:xslt-debug}runtime)));
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
					Convert_18_to_19.<xsl:template name="newline">({urn:schemas-microsoft-com:xslt-debug}runtime);
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
			int num = (xPathNavigator.NodeType != XPathNodeType.Element) ? (((1 << (int)xPathNavigator.NodeType & 112) == 0) ? ((xPathNavigator.NodeType != XPathNodeType.Comment) ? ((xPathNavigator.NodeType != XPathNodeType.ProcessingInstruction) ? -1 : 0) : 1) : ((!string.Equals(XsltFunctions.NormalizeSpace(xPathNavigator.Value), "")) ? 2 : 4)) : ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 22, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 23, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 24, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 25, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 26, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 27, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 28, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 29, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 30, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 31, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 32, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 33, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 34, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 35, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 36, 1)) ? -1 : 5) : 6) : 7) : 8) : 9) : 10) : 11) : 12) : 13) : 14) : 15) : 16) : 17) : 18) : 19);
			switch ((num <= 3) ? (((1 << (int)xPathNavigator.NodeType & 13) != 0) ? ((num <= -1) ? -1 : num) : 3) : num)
			{
			case 0:
				Convert_18_to_19.<xsl:template match="comment() | processing-instruction()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, indent);
				return;
			case 1:
				Convert_18_to_19.<xsl:template match="comment() | processing-instruction()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, indent);
				return;
			case 2:
				Convert_18_to_19.<xsl:template match="text()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, indent);
				return;
			case 3:
				Convert_18_to_19.<xsl:template match="node()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, indent);
				return;
			case 4:
				break;
			case 5:
				Convert_18_to_19.<xsl:template match="VLConfig:FileFormatVersion" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime);
				return;
			case 6:
				Convert_18_to_19.<xsl:template match="VLConfig:LEDConfigurationLEDList" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			case 7:
				Convert_18_to_19.<xsl:template match="VLConfig:HardwareConfigurationDigitalInputConfiguration" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			case 8:
				Convert_18_to_19.<xsl:template match="VLConfig:CcpXcpEcuEthernetPort" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			case 9:
				Convert_18_to_19.<xsl:template match="VLConfig:CcpXcpEcuSendSetSessionStatus" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			case 10:
				Convert_18_to_19.<xsl:template match="VLConfig:CcpXcpEcuUseDbParams" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			case 11:
				Convert_18_to_19.<xsl:template match="VLConfig:InterfaceModeConfigurationIpAddress" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			case 12:
				Convert_18_to_19.<xsl:template match="VLConfig:InterfaceModeConfigurationSubnetMask" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			case 13:
				Convert_18_to_19.<xsl:template match="VLConfig:WLANConfigurationGatewayIp" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			case 14:
				Convert_18_to_19.<xsl:template match="VLConfig:WLANConfigurationLoggerIp" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			case 15:
				Convert_18_to_19.<xsl:template match="VLConfig:WLANConfigurationMLserverIp" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			case 16:
				Convert_18_to_19.<xsl:template match="VLConfig:WLANConfigurationSubnetMask" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			case 17:
				Convert_18_to_19.<xsl:template match="VLConfig:HardwareConfigurationAnalogInputConfiguration" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			case 18:
				Convert_18_to_19.<xsl:template match="VLConfig:HardwareConfigurationGPSConfiguration" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			case 19:
				Convert_18_to_19.<xsl:template match="VLConfig:RecordTrigger" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			default:
				if ((1 << (int)xPathNavigator.NodeType & 508) == 0)
				{
					ContentIterator contentIterator;
					contentIterator.Create(xPathNavigator);
					while (contentIterator.MoveNext())
					{
						Convert_18_to_19.<xsl:apply-templates> (2)({urn:schemas-microsoft-com:xslt-debug}runtime, contentIterator.Current);
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
			int num = (xPathNavigator.NodeType != XPathNodeType.Element) ? (((1 << (int)xPathNavigator.NodeType & 112) == 0) ? ((xPathNavigator.NodeType != XPathNodeType.Comment) ? ((xPathNavigator.NodeType != XPathNodeType.ProcessingInstruction) ? -1 : 0) : 1) : ((!string.Equals(XsltFunctions.NormalizeSpace(xPathNavigator.Value), "")) ? 2 : 4)) : ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 22, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 23, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 24, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 25, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 26, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 27, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 28, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 29, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 30, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 31, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 32, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 33, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 34, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 35, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 36, 1)) ? -1 : 5) : 6) : 7) : 8) : 9) : 10) : 11) : 12) : 13) : 14) : 15) : 16) : 17) : 18) : 19);
			switch ((num <= 3) ? (((1 << (int)xPathNavigator.NodeType & 13) != 0) ? ((num <= -1) ? -1 : num) : 3) : num)
			{
			case 0:
				Convert_18_to_19.<xsl:template match="comment() | processing-instruction()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, "");
				return;
			case 1:
				Convert_18_to_19.<xsl:template match="comment() | processing-instruction()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, "");
				return;
			case 2:
				Convert_18_to_19.<xsl:template match="text()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, "");
				return;
			case 3:
				Convert_18_to_19.<xsl:template match="node()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, "");
				return;
			case 4:
				break;
			case 5:
				Convert_18_to_19.<xsl:template match="VLConfig:FileFormatVersion" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime);
				return;
			case 6:
				Convert_18_to_19.<xsl:template match="VLConfig:LEDConfigurationLEDList" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			case 7:
				Convert_18_to_19.<xsl:template match="VLConfig:HardwareConfigurationDigitalInputConfiguration" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			case 8:
				Convert_18_to_19.<xsl:template match="VLConfig:CcpXcpEcuEthernetPort" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			case 9:
				Convert_18_to_19.<xsl:template match="VLConfig:CcpXcpEcuSendSetSessionStatus" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			case 10:
				Convert_18_to_19.<xsl:template match="VLConfig:CcpXcpEcuUseDbParams" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			case 11:
				Convert_18_to_19.<xsl:template match="VLConfig:InterfaceModeConfigurationIpAddress" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			case 12:
				Convert_18_to_19.<xsl:template match="VLConfig:InterfaceModeConfigurationSubnetMask" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			case 13:
				Convert_18_to_19.<xsl:template match="VLConfig:WLANConfigurationGatewayIp" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			case 14:
				Convert_18_to_19.<xsl:template match="VLConfig:WLANConfigurationLoggerIp" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			case 15:
				Convert_18_to_19.<xsl:template match="VLConfig:WLANConfigurationMLserverIp" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			case 16:
				Convert_18_to_19.<xsl:template match="VLConfig:WLANConfigurationSubnetMask" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			case 17:
				Convert_18_to_19.<xsl:template match="VLConfig:HardwareConfigurationAnalogInputConfiguration" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			case 18:
				Convert_18_to_19.<xsl:template match="VLConfig:HardwareConfigurationGPSConfiguration" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			case 19:
				Convert_18_to_19.<xsl:template match="VLConfig:RecordTrigger" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			default:
				if ((1 << (int)xPathNavigator.NodeType & 508) == 0)
				{
					ContentIterator contentIterator;
					contentIterator.Create(xPathNavigator);
					while (contentIterator.MoveNext())
					{
						Convert_18_to_19.<xsl:apply-templates> (2)({urn:schemas-microsoft-com:xslt-debug}runtime, contentIterator.Current);
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
				XPathNavigator xPathNavigator2 = Convert_18_to_19.SyncToNavigator(xPathNavigator2, {urn:schemas-microsoft-com:xslt-debug}runtime.ExternalContext.DefaultDataSource);
				xPathNavigator2.MoveToRoot();
				xPathNavigator = Convert_18_to_19.SyncToNavigator(arg_2A_0, xPathNavigator2);
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
