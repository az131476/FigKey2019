using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Xml.XPath;
using System.Xml.Xsl.Runtime;

namespace Vector.VLConfig.Data.ConfigurationPersistency.ConfigurationReader.VersionConverter.XSLT
{
	[GeneratedCode("System.Xml.Xsl.XslCompiledTransform", "2.0.0.0"), ComVisible(true)]
	public static class Convert_17_to_18
	{
		private static $ArrayType$807 __staticData;

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
			22,
			0,
			0,
			0,
			11,
			65,
			99,
			116,
			105,
			111,
			110,
			69,
			118,
			101,
			110,
			116,
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
			35,
			68,
			97,
			116,
			97,
			84,
			114,
			97,
			110,
			115,
			102,
			101,
			114,
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
			84,
			114,
			97,
			110,
			115,
			102,
			101,
			114,
			77,
			111,
			100,
			101,
			5,
			86,
			97,
			108,
			117,
			101,
			13,
			65,
			99,
			116,
			105,
			111,
			110,
			67,
			111,
			109,
			109,
			101,
			110,
			116,
			14,
			65,
			99,
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
			14,
			65,
			99,
			116,
			105,
			111,
			110,
			83,
			116,
			111,
			112,
			84,
			121,
			112,
			101,
			43,
			68,
			97,
			116,
			97,
			84,
			114,
			97,
			110,
			115,
			102,
			101,
			114,
			84,
			114,
			105,
			103,
			103,
			101,
			114,
			73,
			115,
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
			69,
			110,
			97,
			98,
			108,
			101,
			100,
			44,
			68,
			97,
			116,
			97,
			84,
			114,
			97,
			110,
			115,
			102,
			101,
			114,
			84,
			114,
			105,
			103,
			103,
			101,
			114,
			73,
			115,
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
			69,
			110,
			97,
			98,
			108,
			101,
			100,
			46,
			68,
			97,
			116,
			97,
			84,
			114,
			97,
			110,
			115,
			102,
			101,
			114,
			84,
			114,
			105,
			103,
			103,
			101,
			114,
			73,
			115,
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
			37,
			68,
			97,
			116,
			97,
			84,
			114,
			97,
			110,
			115,
			102,
			101,
			114,
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
			105,
			101,
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
			19,
			68,
			97,
			116,
			97,
			84,
			114,
			97,
			110,
			115,
			102,
			101,
			114,
			84,
			114,
			105,
			103,
			103,
			101,
			114,
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
			85,
			115,
			101,
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
			30,
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
			80,
			111,
			114,
			116,
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
			0,
			0,
			0,
			0,
			0,
			0,
			0,
			0,
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
			Convert_17_to_18.Root({urn:schemas-microsoft-com:xslt-debug}runtime);
		}

		private static void Root(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			output.WriteStartRoot();
			Convert_17_to_18.<xsl:apply-templates> (2)({urn:schemas-microsoft-com:xslt-debug}runtime, {urn:schemas-microsoft-com:xslt-debug}runtime.ExternalContext.DefaultDataSource);
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
			Convert_17_to_18.<xsl:template name="newline">({urn:schemas-microsoft-com:xslt-debug}runtime);
			output.WriteString(indent);
			if (output.StartCopy({urn:schemas-microsoft-com:xslt-debug}current))
			{
				output.EndCopy({urn:schemas-microsoft-com:xslt-debug}current);
			}
		}

		private static void <xsl:template match="text()">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current, string indent)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			Convert_17_to_18.<xsl:template name="newline">({urn:schemas-microsoft-com:xslt-debug}runtime);
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
			output.WriteStringUnchecked("18");
			output.WriteEndElement();
		}

		private static void <xsl:template match="VLConfig:InterfaceModeConfigurationIpAddress" priority="1">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			output.WriteStartElement("", "InterfaceModeConfigurationCustomWebDisplay", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteNamespaceDeclarationUnchecked("i", "http://www.w3.org/2001/XMLSchema-instance");
			output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("", "WebDisplayFilePath", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
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
			output.WriteEndElementUnchecked("", "WebDisplayFilePath", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteEndElement();
			output.WriteStartElement("", "InterfaceModeConfigurationEnableAlwaysOnline", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteNamespaceDeclarationUnchecked("i", "http://www.w3.org/2001/XMLSchema-instance");
			output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("false");
			output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteEndElement();
			output.WriteStartElement("", "InterfaceModeConfigurationExportCycle", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteNamespaceDeclarationUnchecked("i", "http://www.w3.org/2001/XMLSchema-instance");
			output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("200");
			output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteEndElement();
			output.XsltCopyOf({urn:schemas-microsoft-com:xslt-debug}current);
		}

		private static void <xsl:template match="VLConfig:InterfaceModeConfigurationPort" priority="1">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			output.XsltCopyOf({urn:schemas-microsoft-com:xslt-debug}current);
			output.WriteStartElement("", "InterfaceModeConfigurationSignalExportList", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteNamespaceDeclarationUnchecked("i", "http://www.w3.org/2001/XMLSchema-instance");
			output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.WriteStartAttributeUnchecked("z", "Size", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.WriteEndElement();
		}

		private static void <xsl:template match="VLConfig:InterfaceModeConfigurationSubnetMask" priority="1">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			output.XsltCopyOf({urn:schemas-microsoft-com:xslt-debug}current);
			output.WriteStartElement("", "InterfaceModeConfigurationUseCustomWebDisplay", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteNamespaceDeclarationUnchecked("i", "http://www.w3.org/2001/XMLSchema-instance");
			output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
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

		private static void <xsl:template match="VLConfig:InterfaceModeConfigurationUseInterfaceMode" priority="1">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			output.XsltCopyOf({urn:schemas-microsoft-com:xslt-debug}current);
			output.WriteStartElement("", "InterfaceModeConfigurationUseSignalExport", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteNamespaceDeclarationUnchecked("i", "http://www.w3.org/2001/XMLSchema-instance");
			output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
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

		private static void <xsl:template match="VLConfig:WLANConfigurationAnalogInputNumber" priority="1">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			output.XsltCopyOf({urn:schemas-microsoft-com:xslt-debug}current);
			output.WriteStartElement("", "WLANConfigurationGatewayIp", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteNamespaceDeclarationUnchecked("i", "http://www.w3.org/2001/XMLSchema-instance");
			output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("d5p1", "http://schemas.datacontract.org/2004/07/System.Net");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("d5p1", "m_Address", "http://schemas.datacontract.org/2004/07/System.Net");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("520988864");
			output.WriteEndElementUnchecked("d5p1", "m_Address", "http://schemas.datacontract.org/2004/07/System.Net");
			output.WriteStartElementUnchecked("d5p1", "m_Family", "http://schemas.datacontract.org/2004/07/System.Net");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("InterNetwork");
			output.WriteEndElementUnchecked("d5p1", "m_Family", "http://schemas.datacontract.org/2004/07/System.Net");
			output.WriteStartElementUnchecked("d5p1", "m_HashCode", "http://schemas.datacontract.org/2004/07/System.Net");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("0");
			output.WriteEndElementUnchecked("d5p1", "m_HashCode", "http://schemas.datacontract.org/2004/07/System.Net");
			output.WriteStartElementUnchecked("d5p1", "m_Numbers", "http://schemas.datacontract.org/2004/07/System.Net");
			output.WriteNamespaceDeclarationUnchecked("d6p1", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.WriteStartAttributeUnchecked("z", "Size", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("8");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("d6p1", "unsignedShort", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("0");
			output.WriteEndElementUnchecked("d6p1", "unsignedShort", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.WriteStartElementUnchecked("d6p1", "unsignedShort", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("0");
			output.WriteEndElementUnchecked("d6p1", "unsignedShort", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.WriteStartElementUnchecked("d6p1", "unsignedShort", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("0");
			output.WriteEndElementUnchecked("d6p1", "unsignedShort", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.WriteStartElementUnchecked("d6p1", "unsignedShort", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("0");
			output.WriteEndElementUnchecked("d6p1", "unsignedShort", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.WriteStartElementUnchecked("d6p1", "unsignedShort", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("0");
			output.WriteEndElementUnchecked("d6p1", "unsignedShort", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.WriteStartElementUnchecked("d6p1", "unsignedShort", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("0");
			output.WriteEndElementUnchecked("d6p1", "unsignedShort", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.WriteStartElementUnchecked("d6p1", "unsignedShort", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("0");
			output.WriteEndElementUnchecked("d6p1", "unsignedShort", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.WriteStartElementUnchecked("d6p1", "unsignedShort", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("0");
			output.WriteEndElementUnchecked("d6p1", "unsignedShort", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.WriteEndElementUnchecked("d5p1", "m_Numbers", "http://schemas.datacontract.org/2004/07/System.Net");
			output.WriteStartElementUnchecked("d5p1", "m_ScopeId", "http://schemas.datacontract.org/2004/07/System.Net");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("0");
			output.WriteEndElementUnchecked("d5p1", "m_ScopeId", "http://schemas.datacontract.org/2004/07/System.Net");
			output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteEndElement();
		}

		private static void <xsl:template match="VLConfig:WLANConfigurationIsWLANor3GDownloadRingbufferEnabled" priority="1">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			output.XsltCopyOf({urn:schemas-microsoft-com:xslt-debug}current);
			output.WriteStartElement("", "WLANConfigurationLoggerIp", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteNamespaceDeclarationUnchecked("i", "http://www.w3.org/2001/XMLSchema-instance");
			output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("d5p1", "http://schemas.datacontract.org/2004/07/System.Net");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("d5p1", "m_Address", "http://schemas.datacontract.org/2004/07/System.Net");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("504211648");
			output.WriteEndElementUnchecked("d5p1", "m_Address", "http://schemas.datacontract.org/2004/07/System.Net");
			output.WriteStartElementUnchecked("d5p1", "m_Family", "http://schemas.datacontract.org/2004/07/System.Net");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("InterNetwork");
			output.WriteEndElementUnchecked("d5p1", "m_Family", "http://schemas.datacontract.org/2004/07/System.Net");
			output.WriteStartElementUnchecked("d5p1", "m_HashCode", "http://schemas.datacontract.org/2004/07/System.Net");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("0");
			output.WriteEndElementUnchecked("d5p1", "m_HashCode", "http://schemas.datacontract.org/2004/07/System.Net");
			output.WriteStartElementUnchecked("d5p1", "m_Numbers", "http://schemas.datacontract.org/2004/07/System.Net");
			output.WriteNamespaceDeclarationUnchecked("d6p1", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.WriteStartAttributeUnchecked("z", "Size", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("8");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("d6p1", "unsignedShort", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("0");
			output.WriteEndElementUnchecked("d6p1", "unsignedShort", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.WriteStartElementUnchecked("d6p1", "unsignedShort", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("0");
			output.WriteEndElementUnchecked("d6p1", "unsignedShort", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.WriteStartElementUnchecked("d6p1", "unsignedShort", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("0");
			output.WriteEndElementUnchecked("d6p1", "unsignedShort", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.WriteStartElementUnchecked("d6p1", "unsignedShort", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("0");
			output.WriteEndElementUnchecked("d6p1", "unsignedShort", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.WriteStartElementUnchecked("d6p1", "unsignedShort", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("0");
			output.WriteEndElementUnchecked("d6p1", "unsignedShort", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.WriteStartElementUnchecked("d6p1", "unsignedShort", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("0");
			output.WriteEndElementUnchecked("d6p1", "unsignedShort", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.WriteStartElementUnchecked("d6p1", "unsignedShort", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("0");
			output.WriteEndElementUnchecked("d6p1", "unsignedShort", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.WriteStartElementUnchecked("d6p1", "unsignedShort", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("0");
			output.WriteEndElementUnchecked("d6p1", "unsignedShort", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.WriteEndElementUnchecked("d5p1", "m_Numbers", "http://schemas.datacontract.org/2004/07/System.Net");
			output.WriteStartElementUnchecked("d5p1", "m_ScopeId", "http://schemas.datacontract.org/2004/07/System.Net");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("0");
			output.WriteEndElementUnchecked("d5p1", "m_ScopeId", "http://schemas.datacontract.org/2004/07/System.Net");
			output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteEndElement();
			output.WriteStartElement("", "WLANConfigurationMLserverIp", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteNamespaceDeclarationUnchecked("i", "http://www.w3.org/2001/XMLSchema-instance");
			output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("d5p1", "http://schemas.datacontract.org/2004/07/System.Net");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("d5p1", "m_Address", "http://schemas.datacontract.org/2004/07/System.Net");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("16777343");
			output.WriteEndElementUnchecked("d5p1", "m_Address", "http://schemas.datacontract.org/2004/07/System.Net");
			output.WriteStartElementUnchecked("d5p1", "m_Family", "http://schemas.datacontract.org/2004/07/System.Net");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("InterNetwork");
			output.WriteEndElementUnchecked("d5p1", "m_Family", "http://schemas.datacontract.org/2004/07/System.Net");
			output.WriteStartElementUnchecked("d5p1", "m_HashCode", "http://schemas.datacontract.org/2004/07/System.Net");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("0");
			output.WriteEndElementUnchecked("d5p1", "m_HashCode", "http://schemas.datacontract.org/2004/07/System.Net");
			output.WriteStartElementUnchecked("d5p1", "m_Numbers", "http://schemas.datacontract.org/2004/07/System.Net");
			output.WriteNamespaceDeclarationUnchecked("d6p1", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.WriteStartAttributeUnchecked("z", "Size", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("8");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("d6p1", "unsignedShort", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("0");
			output.WriteEndElementUnchecked("d6p1", "unsignedShort", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.WriteStartElementUnchecked("d6p1", "unsignedShort", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("0");
			output.WriteEndElementUnchecked("d6p1", "unsignedShort", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.WriteStartElementUnchecked("d6p1", "unsignedShort", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("0");
			output.WriteEndElementUnchecked("d6p1", "unsignedShort", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.WriteStartElementUnchecked("d6p1", "unsignedShort", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("0");
			output.WriteEndElementUnchecked("d6p1", "unsignedShort", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.WriteStartElementUnchecked("d6p1", "unsignedShort", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("0");
			output.WriteEndElementUnchecked("d6p1", "unsignedShort", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.WriteStartElementUnchecked("d6p1", "unsignedShort", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("0");
			output.WriteEndElementUnchecked("d6p1", "unsignedShort", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.WriteStartElementUnchecked("d6p1", "unsignedShort", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("0");
			output.WriteEndElementUnchecked("d6p1", "unsignedShort", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.WriteStartElementUnchecked("d6p1", "unsignedShort", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("0");
			output.WriteEndElementUnchecked("d6p1", "unsignedShort", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.WriteEndElementUnchecked("d5p1", "m_Numbers", "http://schemas.datacontract.org/2004/07/System.Net");
			output.WriteStartElementUnchecked("d5p1", "m_ScopeId", "http://schemas.datacontract.org/2004/07/System.Net");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("0");
			output.WriteEndElementUnchecked("d5p1", "m_ScopeId", "http://schemas.datacontract.org/2004/07/System.Net");
			output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteEndElement();
		}

		private static void <xsl:template match="VLConfig:WLANConfigurationStartWLANEvent">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			output.XsltCopyOf({urn:schemas-microsoft-com:xslt-debug}current);
			output.WriteStartElement("", "WLANConfigurationSubnetMask", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteNamespaceDeclarationUnchecked("i", "http://www.w3.org/2001/XMLSchema-instance");
			output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("d5p1", "http://schemas.datacontract.org/2004/07/System.Net");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("d5p1", "m_Address", "http://schemas.datacontract.org/2004/07/System.Net");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("16777215");
			output.WriteEndElementUnchecked("d5p1", "m_Address", "http://schemas.datacontract.org/2004/07/System.Net");
			output.WriteStartElementUnchecked("d5p1", "m_Family", "http://schemas.datacontract.org/2004/07/System.Net");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("InterNetwork");
			output.WriteEndElementUnchecked("d5p1", "m_Family", "http://schemas.datacontract.org/2004/07/System.Net");
			output.WriteStartElementUnchecked("d5p1", "m_HashCode", "http://schemas.datacontract.org/2004/07/System.Net");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("0");
			output.WriteEndElementUnchecked("d5p1", "m_HashCode", "http://schemas.datacontract.org/2004/07/System.Net");
			output.WriteStartElementUnchecked("d5p1", "m_Numbers", "http://schemas.datacontract.org/2004/07/System.Net");
			output.WriteNamespaceDeclarationUnchecked("d6p1", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.WriteStartAttributeUnchecked("z", "Size", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("8");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("d6p1", "unsignedShort", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("0");
			output.WriteEndElementUnchecked("d6p1", "unsignedShort", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.WriteStartElementUnchecked("d6p1", "unsignedShort", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("0");
			output.WriteEndElementUnchecked("d6p1", "unsignedShort", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.WriteStartElementUnchecked("d6p1", "unsignedShort", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("0");
			output.WriteEndElementUnchecked("d6p1", "unsignedShort", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.WriteStartElementUnchecked("d6p1", "unsignedShort", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("0");
			output.WriteEndElementUnchecked("d6p1", "unsignedShort", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.WriteStartElementUnchecked("d6p1", "unsignedShort", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("0");
			output.WriteEndElementUnchecked("d6p1", "unsignedShort", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.WriteStartElementUnchecked("d6p1", "unsignedShort", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("0");
			output.WriteEndElementUnchecked("d6p1", "unsignedShort", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.WriteStartElementUnchecked("d6p1", "unsignedShort", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("0");
			output.WriteEndElementUnchecked("d6p1", "unsignedShort", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.WriteStartElementUnchecked("d6p1", "unsignedShort", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("0");
			output.WriteEndElementUnchecked("d6p1", "unsignedShort", "http://schemas.microsoft.com/2003/10/Serialization/Arrays");
			output.WriteEndElementUnchecked("d5p1", "m_Numbers", "http://schemas.datacontract.org/2004/07/System.Net");
			output.WriteStartElementUnchecked("d5p1", "m_ScopeId", "http://schemas.datacontract.org/2004/07/System.Net");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("0");
			output.WriteEndElementUnchecked("d5p1", "m_ScopeId", "http://schemas.datacontract.org/2004/07/System.Net");
			output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteEndElement();
		}

		private static void <xsl:template match="VLConfig:DataTransferTrigger" priority="1">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			ElementContentIterator elementContentIterator;
			elementContentIterator.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(0), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
			string arg_62_0;
			while (elementContentIterator.MoveNext())
			{
				XPathNavigator xPathNavigator = Convert_17_to_18.SyncToNavigator(xPathNavigator, elementContentIterator.Current);
				if (xPathNavigator.MoveToAttribute({urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(2), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3)))
				{
					arg_62_0 = xPathNavigator.Value;
					IL_5D:
					if (string.Equals(arg_62_0, "NextLogSessionStartEvent"))
					{
						ElementContentIterator elementContentIterator2;
						elementContentIterator2.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(4), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
						string arg_CE_0;
						while (elementContentIterator2.MoveNext())
						{
							ElementContentIterator elementContentIterator3;
							elementContentIterator3.Create(elementContentIterator2.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(5), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
							if (elementContentIterator3.MoveNext())
							{
								arg_CE_0 = elementContentIterator3.Current.Value;
								IL_C9:
								if (string.Equals(arg_CE_0, "StopWhileDataTransfer"))
								{
									output.WriteStartElement("", "DataTransferTrigger", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
									output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
									output.WriteNamespaceDeclarationUnchecked("i", "http://www.w3.org/2001/XMLSchema-instance");
									output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
									output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
									output.WriteStringUnchecked("0");
									output.WriteEndAttributeUnchecked();
									ElementContentIterator elementContentIterator4;
									elementContentIterator4.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(6), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
									while (elementContentIterator4.MoveNext())
									{
										XPathItem current = elementContentIterator4.Current;
										output.WriteItem(current);
									}
									output.WriteStartElement("", "ActionEvent", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
									output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
									output.WriteStringUnchecked("0");
									output.WriteEndAttributeUnchecked();
									output.WriteStartAttributeUnchecked("i", "type", "http://www.w3.org/2001/XMLSchema-instance");
									output.WriteStringUnchecked("OnShutdownEvent");
									output.WriteEndAttributeUnchecked();
									output.WriteEndElement();
									ElementContentIterator elementContentIterator5;
									elementContentIterator5.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(7), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
									while (elementContentIterator5.MoveNext())
									{
										XPathItem current2 = elementContentIterator5.Current;
										output.WriteItem(current2);
									}
									ElementContentIterator elementContentIterator6;
									elementContentIterator6.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(8), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
									while (elementContentIterator6.MoveNext())
									{
										XPathItem current3 = elementContentIterator6.Current;
										output.WriteItem(current3);
									}
									ElementContentIterator elementContentIterator7;
									elementContentIterator7.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(4), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
									while (elementContentIterator7.MoveNext())
									{
										XPathItem current4 = elementContentIterator7.Current;
										output.WriteItem(current4);
									}
									ElementContentIterator elementContentIterator8;
									elementContentIterator8.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(9), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
									while (elementContentIterator8.MoveNext())
									{
										XPathItem current5 = elementContentIterator8.Current;
										output.WriteItem(current5);
									}
									ElementContentIterator elementContentIterator9;
									elementContentIterator9.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(10), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
									while (elementContentIterator9.MoveNext())
									{
										XPathItem current6 = elementContentIterator9.Current;
										output.WriteItem(current6);
									}
									ElementContentIterator elementContentIterator10;
									elementContentIterator10.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(11), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
									while (elementContentIterator10.MoveNext())
									{
										XPathItem current7 = elementContentIterator10.Current;
										output.WriteItem(current7);
									}
									ElementContentIterator elementContentIterator11;
									elementContentIterator11.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(12), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
									while (elementContentIterator11.MoveNext())
									{
										XPathItem current8 = elementContentIterator11.Current;
										output.WriteItem(current8);
									}
									output.WriteEndElement();
									return;
								}
								goto IL_361;
							}
						}
						arg_CE_0 = "";
						goto IL_C9;
					}
					IL_361:
					output.XsltCopyOf({urn:schemas-microsoft-com:xslt-debug}current);
					return;
				}
			}
			arg_62_0 = "";
			goto IL_5D;
		}

		private static void <xsl:template match="node()">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current, string indent)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			Convert_17_to_18.<xsl:template name="newline">({urn:schemas-microsoft-com:xslt-debug}runtime);
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
							Convert_17_to_18.<xsl:apply-templates>({urn:schemas-microsoft-com:xslt-debug}runtime, unionIterator.Current, indent + XsltConvert.ToString(Convert_17_to_18.indent-increment({urn:schemas-microsoft-com:xslt-debug}runtime)));
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
					Convert_17_to_18.<xsl:template name="newline">({urn:schemas-microsoft-com:xslt-debug}runtime);
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
			int num = (xPathNavigator.NodeType != XPathNodeType.Element) ? (((1 << (int)xPathNavigator.NodeType & 112) == 0) ? ((xPathNavigator.NodeType != XPathNodeType.Comment) ? ((xPathNavigator.NodeType != XPathNodeType.ProcessingInstruction) ? -1 : 0) : 1) : ((!string.Equals(XsltFunctions.NormalizeSpace(xPathNavigator.Value), "")) ? 2 : 5)) : ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 13, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 14, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 15, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 16, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 17, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 18, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 19, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 20, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 21, 1)) ? -1 : 4) : 6) : 7) : 8) : 9) : 10) : 11) : 12) : 13);
			switch ((num <= 3) ? (((1 << (int)xPathNavigator.NodeType & 13) != 0) ? ((num <= -1) ? -1 : num) : 3) : num)
			{
			case 0:
				Convert_17_to_18.<xsl:template match="comment() | processing-instruction()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, indent);
				return;
			case 1:
				Convert_17_to_18.<xsl:template match="comment() | processing-instruction()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, indent);
				return;
			case 2:
				Convert_17_to_18.<xsl:template match="text()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, indent);
				return;
			case 3:
				Convert_17_to_18.<xsl:template match="node()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, indent);
				return;
			case 4:
				Convert_17_to_18.<xsl:template match="VLConfig:WLANConfigurationStartWLANEvent">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			case 5:
				break;
			case 6:
				Convert_17_to_18.<xsl:template match="VLConfig:FileFormatVersion" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime);
				return;
			case 7:
				Convert_17_to_18.<xsl:template match="VLConfig:InterfaceModeConfigurationIpAddress" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			case 8:
				Convert_17_to_18.<xsl:template match="VLConfig:InterfaceModeConfigurationPort" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			case 9:
				Convert_17_to_18.<xsl:template match="VLConfig:InterfaceModeConfigurationSubnetMask" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			case 10:
				Convert_17_to_18.<xsl:template match="VLConfig:InterfaceModeConfigurationUseInterfaceMode" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			case 11:
				Convert_17_to_18.<xsl:template match="VLConfig:WLANConfigurationAnalogInputNumber" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			case 12:
				Convert_17_to_18.<xsl:template match="VLConfig:WLANConfigurationIsWLANor3GDownloadRingbufferEnabled" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			case 13:
				Convert_17_to_18.<xsl:template match="VLConfig:DataTransferTrigger" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			default:
				if ((1 << (int)xPathNavigator.NodeType & 508) == 0)
				{
					ContentIterator contentIterator;
					contentIterator.Create(xPathNavigator);
					while (contentIterator.MoveNext())
					{
						Convert_17_to_18.<xsl:apply-templates> (2)({urn:schemas-microsoft-com:xslt-debug}runtime, contentIterator.Current);
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
			int num = (xPathNavigator.NodeType != XPathNodeType.Element) ? (((1 << (int)xPathNavigator.NodeType & 112) == 0) ? ((xPathNavigator.NodeType != XPathNodeType.Comment) ? ((xPathNavigator.NodeType != XPathNodeType.ProcessingInstruction) ? -1 : 0) : 1) : ((!string.Equals(XsltFunctions.NormalizeSpace(xPathNavigator.Value), "")) ? 2 : 5)) : ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 13, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 14, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 15, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 16, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 17, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 18, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 19, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 20, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 21, 1)) ? -1 : 4) : 6) : 7) : 8) : 9) : 10) : 11) : 12) : 13);
			switch ((num <= 3) ? (((1 << (int)xPathNavigator.NodeType & 13) != 0) ? ((num <= -1) ? -1 : num) : 3) : num)
			{
			case 0:
				Convert_17_to_18.<xsl:template match="comment() | processing-instruction()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, "");
				return;
			case 1:
				Convert_17_to_18.<xsl:template match="comment() | processing-instruction()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, "");
				return;
			case 2:
				Convert_17_to_18.<xsl:template match="text()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, "");
				return;
			case 3:
				Convert_17_to_18.<xsl:template match="node()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, "");
				return;
			case 4:
				Convert_17_to_18.<xsl:template match="VLConfig:WLANConfigurationStartWLANEvent">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			case 5:
				break;
			case 6:
				Convert_17_to_18.<xsl:template match="VLConfig:FileFormatVersion" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime);
				return;
			case 7:
				Convert_17_to_18.<xsl:template match="VLConfig:InterfaceModeConfigurationIpAddress" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			case 8:
				Convert_17_to_18.<xsl:template match="VLConfig:InterfaceModeConfigurationPort" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			case 9:
				Convert_17_to_18.<xsl:template match="VLConfig:InterfaceModeConfigurationSubnetMask" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			case 10:
				Convert_17_to_18.<xsl:template match="VLConfig:InterfaceModeConfigurationUseInterfaceMode" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			case 11:
				Convert_17_to_18.<xsl:template match="VLConfig:WLANConfigurationAnalogInputNumber" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			case 12:
				Convert_17_to_18.<xsl:template match="VLConfig:WLANConfigurationIsWLANor3GDownloadRingbufferEnabled" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			case 13:
				Convert_17_to_18.<xsl:template match="VLConfig:DataTransferTrigger" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			default:
				if ((1 << (int)xPathNavigator.NodeType & 508) == 0)
				{
					ContentIterator contentIterator;
					contentIterator.Create(xPathNavigator);
					while (contentIterator.MoveNext())
					{
						Convert_17_to_18.<xsl:apply-templates> (2)({urn:schemas-microsoft-com:xslt-debug}runtime, contentIterator.Current);
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
