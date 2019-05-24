using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Xml.XPath;
using System.Xml.Xsl.Runtime;

namespace Vector.VLConfig.Data.ConfigurationPersistency.ConfigurationReader.VersionConverter.XSLT
{
	[GeneratedCode("System.Xml.Xsl.XslCompiledTransform", "2.0.0.0"), ComVisible(true)]
	public static class Convert_6_to_7
	{
		private static $ArrayType$459 __staticData;

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
			12,
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
			14,
			68,
			97,
			116,
			97,
			98,
			97,
			115,
			101,
			67,
			80,
			84,
			121,
			112,
			101,
			5,
			86,
			97,
			108,
			117,
			101,
			21,
			68,
			97,
			116,
			97,
			98,
			97,
			115,
			101,
			72,
			97,
			115,
			83,
			101,
			101,
			100,
			65,
			110,
			100,
			75,
			101,
			121,
			26,
			68,
			97,
			116,
			97,
			98,
			97,
			115,
			101,
			83,
			101,
			101,
			100,
			65,
			110,
			100,
			75,
			101,
			121,
			70,
			105,
			108,
			101,
			80,
			97,
			116,
			104,
			8,
			68,
			97,
			116,
			97,
			98,
			97,
			115,
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
			Convert_6_to_7.Root({urn:schemas-microsoft-com:xslt-debug}runtime);
		}

		private static void Root(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			output.WriteStartRoot();
			Convert_6_to_7.<xsl:apply-templates> (2)({urn:schemas-microsoft-com:xslt-debug}runtime, {urn:schemas-microsoft-com:xslt-debug}runtime.ExternalContext.DefaultDataSource);
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
			Convert_6_to_7.<xsl:template name="newline">({urn:schemas-microsoft-com:xslt-debug}runtime);
			output.WriteString(indent);
			if (output.StartCopy({urn:schemas-microsoft-com:xslt-debug}current))
			{
				output.EndCopy({urn:schemas-microsoft-com:xslt-debug}current);
			}
		}

		private static void <xsl:template match="text()">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current, string indent)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			Convert_6_to_7.<xsl:template name="newline">({urn:schemas-microsoft-com:xslt-debug}runtime);
			output.WriteString(indent);
			output.WriteString(XsltFunctions.NormalizeSpace({urn:schemas-microsoft-com:xslt-debug}current.Value));
		}

		private static void <xsl:template match="VLConfig:FileFormatVersion" priority="1">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			output.WriteStartElement("", "FileFormatVersion", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("7");
			output.WriteEndElement();
		}

		private static void <xsl:template match="VLConfig:HardwareConfigurationCANChannelConfiguration" priority="1">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current, string indent)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			Convert_6_to_7.<xsl:template name="PrettyPrintNodeRecursive">({urn:schemas-microsoft-com:xslt-debug}runtime, {urn:schemas-microsoft-com:xslt-debug}current, indent);
			XPathNavigator xPathNavigator = Convert_6_to_7.SyncToNavigator(xPathNavigator, {urn:schemas-microsoft-com:xslt-debug}current);
			xPathNavigator.MoveToRoot();
			DescendantIterator descendantIterator;
			descendantIterator.Create(xPathNavigator, {urn:schemas-microsoft-com:xslt-debug}runtime.GetNameFilter(0), false);
			Convert_6_to_7.<xsl:template name="InsertFlexrayChannels">({urn:schemas-microsoft-com:xslt-debug}runtime, (!descendantIterator.MoveNext()) ? "" : descendantIterator.Current.Value);
		}

		private static void <xsl:template name="InsertFlexrayChannels">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, string LoggerTypeName)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			if (string.Equals(LoggerTypeName, "GL4000"))
			{
				output.WriteStartElement("", "HardwareConfigurationFlexrayChannelConfiguration", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "FlexrayChannelConfigurationChannelList", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.WriteStartAttributeUnchecked("z", "Size", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("2");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "FlexrayChannel", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "ChannelIsActive", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteStringUnchecked("false");
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "ChannelIsActive", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "FlexrayChannelIsKeepAwakeActive", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteStringUnchecked("true");
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "FlexrayChannelIsKeepAwakeActive", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "FlexrayChannelIsWakeUpEnabled", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteStringUnchecked("true");
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "FlexrayChannelIsWakeUpEnabled", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "FlexrayChannel", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "FlexrayChannel", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "ChannelIsActive", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteStringUnchecked("false");
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "ChannelIsActive", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "FlexrayChannelIsKeepAwakeActive", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteStringUnchecked("true");
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "FlexrayChannelIsKeepAwakeActive", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "FlexrayChannelIsWakeUpEnabled", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteStringUnchecked("true");
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "FlexrayChannelIsWakeUpEnabled", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "FlexrayChannel", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "FlexrayChannelConfigurationChannelList", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElement();
			}
			else
			{
				output.WriteStartElement("", "HardwareConfigurationFlexrayChannelConfiguration", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "FlexrayChannelConfigurationChannelList", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.WriteStartAttributeUnchecked("z", "Size", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteEndElementUnchecked("", "FlexrayChannelConfigurationChannelList", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElement();
			}
		}

		private static void <xsl:template match="VLConfig:WLANConfigurationIsDownloadRingbufferEnabled" priority="1">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current, string indent)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			Convert_6_to_7.<xsl:template name="PrettyPrintNodeRecursive">({urn:schemas-microsoft-com:xslt-debug}runtime, {urn:schemas-microsoft-com:xslt-debug}current, indent);
			output.WriteStartElement("", "WLANConfigurationIsPartialDownloadEnabled", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
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

		private static void <xsl:template match="VLConfig:FilterDatabasePath" priority="1">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current, string indent)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			Convert_6_to_7.<xsl:template name="PrettyPrintNodeRecursive">({urn:schemas-microsoft-com:xslt-debug}runtime, {urn:schemas-microsoft-com:xslt-debug}current, indent);
			output.WriteStartElement("", "FilterMessageIsFlexrayPDU", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("true");
			output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteEndElement();
		}

		private static void <xsl:template match="VLConfig:SymbolicMessageTriggerDatabasePath" priority="1">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current, string indent)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			Convert_6_to_7.<xsl:template name="PrettyPrintNodeRecursive">({urn:schemas-microsoft-com:xslt-debug}runtime, {urn:schemas-microsoft-com:xslt-debug}current, indent);
			output.WriteStartElement("", "SymbolicMessageTriggerIsFlexrayPDU", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("true");
			output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteEndElement();
		}

		private static void <xsl:template match="VLConfig:Database" priority="1">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current, string indent)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			if (output.StartCopy({urn:schemas-microsoft-com:xslt-debug}current))
			{
				AttributeIterator attributeIterator;
				attributeIterator.Create({urn:schemas-microsoft-com:xslt-debug}current);
				while (attributeIterator.MoveNext())
				{
					XPathItem current = attributeIterator.Current;
					output.WriteItem(current);
				}
				NodeKindContentIterator nodeKindContentIterator;
				nodeKindContentIterator.Create({urn:schemas-microsoft-com:xslt-debug}current, XPathNodeType.Element);
				while (nodeKindContentIterator.MoveNext())
				{
					string prefix = nodeKindContentIterator.Current.Prefix;
					string localName = nodeKindContentIterator.Current.LocalName;
					if (string.Equals((prefix.Length != 0) ? (prefix + ":" + localName) : localName, "DatabaseChannelNumber"))
					{
						Convert_6_to_7.<xsl:template name="PrettyPrintNodeRecursive">({urn:schemas-microsoft-com:xslt-debug}runtime, nodeKindContentIterator.Current, indent);
						ElementContentIterator elementContentIterator;
						elementContentIterator.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(2), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
						string arg_1BF_1;
						while (elementContentIterator.MoveNext())
						{
							ElementContentIterator elementContentIterator2;
							elementContentIterator2.Create(elementContentIterator.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
							if (elementContentIterator2.MoveNext())
							{
								arg_1BF_1 = elementContentIterator2.Current.Value;
								IL_105:
								ElementContentIterator elementContentIterator3;
								elementContentIterator3.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(4), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
								string arg_1BF_2;
								while (elementContentIterator3.MoveNext())
								{
									ElementContentIterator elementContentIterator4;
									elementContentIterator4.Create(elementContentIterator3.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
									if (elementContentIterator4.MoveNext())
									{
										arg_1BF_2 = elementContentIterator4.Current.Value;
										IL_162:
										ElementContentIterator elementContentIterator5;
										elementContentIterator5.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(5), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
										string arg_1BF_3;
										while (elementContentIterator5.MoveNext())
										{
											ElementContentIterator elementContentIterator6;
											elementContentIterator6.Create(elementContentIterator5.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
											if (elementContentIterator6.MoveNext())
											{
												arg_1BF_3 = elementContentIterator6.Current.Value;
												IL_1BF:
												Convert_6_to_7.<xsl:template name="InsertCpProtectionList">({urn:schemas-microsoft-com:xslt-debug}runtime, arg_1BF_1, arg_1BF_2, arg_1BF_3);
												goto IL_37A;
											}
										}
										arg_1BF_3 = "";
										goto IL_1BF;
									}
								}
								arg_1BF_2 = "";
								goto IL_162;
							}
						}
						arg_1BF_1 = "";
						goto IL_105;
					}
					string prefix2 = nodeKindContentIterator.Current.Prefix;
					string localName2 = nodeKindContentIterator.Current.LocalName;
					if (!string.Equals((prefix2.Length != 0) ? (prefix2 + ":" + localName2) : localName2, "DatabaseHasSeedAndKey"))
					{
						string prefix3 = nodeKindContentIterator.Current.Prefix;
						string localName3 = nodeKindContentIterator.Current.LocalName;
						if (!string.Equals((prefix3.Length != 0) ? (prefix3 + ":" + localName3) : localName3, "DatabaseSeedAndKeyFilePath"))
						{
							string prefix4 = nodeKindContentIterator.Current.Prefix;
							string localName4 = nodeKindContentIterator.Current.LocalName;
							if (string.Equals((prefix4.Length != 0) ? (prefix4 + ":" + localName4) : localName4, "DatabaseFilePath"))
							{
								Convert_6_to_7.<xsl:template name="PrettyPrintNodeRecursive">({urn:schemas-microsoft-com:xslt-debug}runtime, nodeKindContentIterator.Current, indent);
								output.WriteStartElement("", "DatabaseIsCPActive", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
								output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
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
							else
							{
								Convert_6_to_7.<xsl:template name="PrettyPrintNodeRecursive">({urn:schemas-microsoft-com:xslt-debug}runtime, nodeKindContentIterator.Current, indent);
							}
						}
					}
					IL_37A:;
				}
				output.EndCopy({urn:schemas-microsoft-com:xslt-debug}current);
			}
		}

		private static void <xsl:template name="InsertCpProtectionList">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, string CPType, string HasSeedAndKey, string SeedAndKeyFilePath)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			if (!string.Equals(CPType, "None"))
			{
				output.WriteStartElement("", "DatabaseCpProtectionList", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.WriteStartAttributeUnchecked("z", "Size", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("1");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "CPProtection", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "CPProtectionECUName", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
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
				output.WriteEndElementUnchecked("", "CPProtectionECUName", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "CPProtectionHasSeedAndKey", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteStringUnchecked(HasSeedAndKey);
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "CPProtectionHasSeedAndKey", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "CPProtectionSeedAndKeyFilePath", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStringUnchecked(SeedAndKeyFilePath);
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "CPProtectionSeedAndKeyFilePath", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "CPProtection", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElement();
			}
			else
			{
				output.WriteStartElement("", "DatabaseCpProtectionList", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.WriteStartAttributeUnchecked("z", "Size", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.WriteEndElement();
			}
		}

		private static void <xsl:template name="PrettyPrintNodeRecursive">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current, string indent)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			Convert_6_to_7.<xsl:template name="newline">({urn:schemas-microsoft-com:xslt-debug}runtime);
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
							Convert_6_to_7.<xsl:apply-templates>({urn:schemas-microsoft-com:xslt-debug}runtime, unionIterator.Current, indent + XsltConvert.ToString(Convert_6_to_7.indent-increment({urn:schemas-microsoft-com:xslt-debug}runtime)));
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
					Convert_6_to_7.<xsl:template name="newline">({urn:schemas-microsoft-com:xslt-debug}runtime);
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
			Convert_6_to_7.<xsl:template name="PrettyPrintNodeRecursive">({urn:schemas-microsoft-com:xslt-debug}runtime, {urn:schemas-microsoft-com:xslt-debug}current, indent);
		}

		private static void <xsl:apply-templates>(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator xPathNavigator, string indent)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			int arg_100_0;
			if (xPathNavigator.NodeType == XPathNodeType.Element)
			{
				int num = (!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 6, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 7, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 8, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 9, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 10, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 11, 1)) ? -1 : 5) : 6) : 7) : 8) : 9) : 10;
				arg_100_0 = ((num <= 4) ? 4 : num);
			}
			else
			{
				arg_100_0 = (((1 << (int)xPathNavigator.NodeType & 112) == 0) ? ((xPathNavigator.NodeType != XPathNodeType.Comment) ? ((xPathNavigator.NodeType != XPathNodeType.ProcessingInstruction) ? -1 : 0) : 1) : ((!string.Equals(XsltFunctions.NormalizeSpace(xPathNavigator.Value), "")) ? 2 : 3));
			}
			switch (arg_100_0)
			{
			case 0:
				Convert_6_to_7.<xsl:template match="comment() | processing-instruction()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, indent);
				return;
			case 1:
				Convert_6_to_7.<xsl:template match="comment() | processing-instruction()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, indent);
				return;
			case 2:
				Convert_6_to_7.<xsl:template match="text()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, indent);
				return;
			case 3:
				break;
			case 4:
				Convert_6_to_7.<xsl:template match="*" priority="0.5">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, indent);
				return;
			case 5:
				Convert_6_to_7.<xsl:template match="VLConfig:FileFormatVersion" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime);
				return;
			case 6:
				Convert_6_to_7.<xsl:template match="VLConfig:HardwareConfigurationCANChannelConfiguration" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, indent);
				return;
			case 7:
				Convert_6_to_7.<xsl:template match="VLConfig:WLANConfigurationIsDownloadRingbufferEnabled" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, indent);
				return;
			case 8:
				Convert_6_to_7.<xsl:template match="VLConfig:FilterDatabasePath" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, indent);
				return;
			case 9:
				Convert_6_to_7.<xsl:template match="VLConfig:SymbolicMessageTriggerDatabasePath" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, indent);
				return;
			case 10:
				Convert_6_to_7.<xsl:template match="VLConfig:Database" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, indent);
				return;
			default:
				if ((1 << (int)xPathNavigator.NodeType & 508) == 0)
				{
					ContentIterator contentIterator;
					contentIterator.Create(xPathNavigator);
					while (contentIterator.MoveNext())
					{
						Convert_6_to_7.<xsl:apply-templates> (2)({urn:schemas-microsoft-com:xslt-debug}runtime, contentIterator.Current);
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
			int arg_100_0;
			if (xPathNavigator.NodeType == XPathNodeType.Element)
			{
				int num = (!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 6, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 7, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 8, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 9, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 10, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 11, 1)) ? -1 : 5) : 6) : 7) : 8) : 9) : 10;
				arg_100_0 = ((num <= 4) ? 4 : num);
			}
			else
			{
				arg_100_0 = (((1 << (int)xPathNavigator.NodeType & 112) == 0) ? ((xPathNavigator.NodeType != XPathNodeType.Comment) ? ((xPathNavigator.NodeType != XPathNodeType.ProcessingInstruction) ? -1 : 0) : 1) : ((!string.Equals(XsltFunctions.NormalizeSpace(xPathNavigator.Value), "")) ? 2 : 3));
			}
			switch (arg_100_0)
			{
			case 0:
				Convert_6_to_7.<xsl:template match="comment() | processing-instruction()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, "");
				return;
			case 1:
				Convert_6_to_7.<xsl:template match="comment() | processing-instruction()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, "");
				return;
			case 2:
				Convert_6_to_7.<xsl:template match="text()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, "");
				return;
			case 3:
				break;
			case 4:
				Convert_6_to_7.<xsl:template match="*" priority="0.5">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, "");
				return;
			case 5:
				Convert_6_to_7.<xsl:template match="VLConfig:FileFormatVersion" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime);
				return;
			case 6:
				Convert_6_to_7.<xsl:template match="VLConfig:HardwareConfigurationCANChannelConfiguration" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, "");
				return;
			case 7:
				Convert_6_to_7.<xsl:template match="VLConfig:WLANConfigurationIsDownloadRingbufferEnabled" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, "");
				return;
			case 8:
				Convert_6_to_7.<xsl:template match="VLConfig:FilterDatabasePath" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, "");
				return;
			case 9:
				Convert_6_to_7.<xsl:template match="VLConfig:SymbolicMessageTriggerDatabasePath" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, "");
				return;
			case 10:
				Convert_6_to_7.<xsl:template match="VLConfig:Database" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, "");
				return;
			default:
				if ((1 << (int)xPathNavigator.NodeType & 508) == 0)
				{
					ContentIterator contentIterator;
					contentIterator.Create(xPathNavigator);
					while (contentIterator.MoveNext())
					{
						Convert_6_to_7.<xsl:apply-templates> (2)({urn:schemas-microsoft-com:xslt-debug}runtime, contentIterator.Current);
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
