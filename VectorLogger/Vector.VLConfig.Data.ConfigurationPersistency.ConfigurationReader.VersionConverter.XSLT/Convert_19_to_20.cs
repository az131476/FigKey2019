using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Xml.XPath;
using System.Xml.Xsl.Runtime;

namespace Vector.VLConfig.Data.ConfigurationPersistency.ConfigurationReader.VersionConverter.XSLT
{
	[GeneratedCode("System.Xml.Xsl.XslCompiledTransform", "2.0.0.0"), ComVisible(true)]
	public static class Convert_19_to_20
	{
		private static $ArrayType$1201 __staticData;

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
			44,
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
			5,
			86,
			97,
			108,
			117,
			101,
			2,
			73,
			100,
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
			4,
			83,
			105,
			122,
			101,
			10,
			76,
			73,
			78,
			67,
			104,
			97,
			110,
			110,
			101,
			108,
			15,
			67,
			104,
			97,
			110,
			110,
			101,
			108,
			73,
			115,
			65,
			99,
			116,
			105,
			118,
			101,
			18,
			76,
			73,
			78,
			67,
			104,
			97,
			110,
			110,
			101,
			108,
			66,
			97,
			117,
			100,
			114,
			97,
			116,
			101,
			27,
			76,
			73,
			78,
			67,
			104,
			97,
			110,
			110,
			101,
			108,
			73,
			115,
			75,
			101,
			101,
			112,
			65,
			119,
			97,
			107,
			101,
			65,
			99,
			116,
			105,
			118,
			101,
			25,
			76,
			73,
			78,
			67,
			104,
			97,
			110,
			110,
			101,
			108,
			73,
			115,
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
			18,
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
			66,
			97,
			117,
			100,
			114,
			97,
			116,
			101,
			27,
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
			73,
			115,
			75,
			101,
			101,
			112,
			65,
			119,
			97,
			107,
			101,
			65,
			99,
			116,
			105,
			118,
			101,
			24,
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
			73,
			115,
			79,
			117,
			116,
			112,
			117,
			116,
			65,
			99,
			116,
			105,
			118,
			101,
			25,
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
			73,
			115,
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
			76,
			111,
			103,
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
			19,
			67,
			99,
			112,
			88,
			99,
			112,
			69,
			99,
			117,
			67,
			97,
			110,
			70,
			105,
			114,
			115,
			116,
			73,
			68,
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
			67,
			97,
			110,
			82,
			101,
			113,
			117,
			101,
			115,
			116,
			73,
			68,
			22,
			67,
			99,
			112,
			88,
			99,
			112,
			69,
			99,
			117,
			67,
			97,
			110,
			82,
			101,
			115,
			112,
			111,
			110,
			115,
			101,
			73,
			68,
			19,
			67,
			99,
			112,
			88,
			99,
			112,
			69,
			99,
			117,
			68,
			97,
			113,
			84,
			105,
			109,
			101,
			111,
			117,
			116,
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
			68,
			105,
			115,
			112,
			108,
			97,
			121,
			78,
			97,
			109,
			101,
			3,
			82,
			101,
			102,
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
			72,
			111,
			115,
			116,
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
			22,
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
			50,
			25,
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
			114,
			111,
			116,
			111,
			99,
			111,
			108,
			26,
			67,
			99,
			112,
			88,
			99,
			112,
			69,
			99,
			117,
			70,
			108,
			101,
			120,
			82,
			97,
			121,
			69,
			99,
			117,
			84,
			120,
			81,
			117,
			101,
			117,
			101,
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
			70,
			108,
			101,
			120,
			82,
			97,
			121,
			88,
			99,
			112,
			78,
			111,
			100,
			101,
			65,
			100,
			114,
			101,
			115,
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
			73,
			115,
			67,
			97,
			110,
			70,
			105,
			114,
			115,
			116,
			73,
			100,
			69,
			120,
			116,
			101,
			110,
			100,
			101,
			100,
			31,
			67,
			99,
			112,
			88,
			99,
			112,
			69,
			99,
			117,
			73,
			115,
			67,
			97,
			110,
			82,
			101,
			113,
			117,
			101,
			115,
			116,
			73,
			100,
			69,
			120,
			116,
			101,
			110,
			100,
			101,
			100,
			32,
			67,
			99,
			112,
			88,
			99,
			112,
			69,
			99,
			117,
			73,
			115,
			67,
			97,
			110,
			82,
			101,
			115,
			112,
			111,
			110,
			115,
			101,
			73,
			100,
			69,
			120,
			116,
			101,
			110,
			100,
			101,
			100,
			25,
			67,
			99,
			112,
			88,
			99,
			112,
			69,
			99,
			117,
			73,
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
			85,
			115,
			101,
			100,
			15,
			67,
			99,
			112,
			88,
			99,
			112,
			69,
			99,
			117,
			77,
			97,
			120,
			67,
			84,
			79,
			15,
			67,
			99,
			112,
			88,
			99,
			112,
			69,
			99,
			117,
			77,
			97,
			120,
			68,
			84,
			79,
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
			35,
			67,
			99,
			112,
			88,
			99,
			112,
			69,
			99,
			117,
			84,
			114,
			97,
			110,
			115,
			112,
			111,
			114,
			116,
			76,
			97,
			121,
			101,
			114,
			73,
			110,
			115,
			116,
			97,
			110,
			99,
			101,
			78,
			97,
			109,
			101,
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
			86,
			120,
			77,
			111,
			100,
			117,
			108,
			101,
			9,
			67,
			99,
			112,
			88,
			99,
			112,
			69,
			99,
			117,
			16,
			67,
			99,
			112,
			88,
			99,
			112,
			83,
			105,
			103,
			110,
			97,
			108,
			76,
			105,
			115,
			116,
			10,
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
			37,
			76,
			73,
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
			76,
			73,
			78,
			67,
			104,
			97,
			110,
			110,
			101,
			108,
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
			77,
			79,
			83,
			84,
			49,
			53,
			48,
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
			0,
			0,
			0,
			0,
			2,
			0,
			0,
			0,
			0,
			1,
			2,
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
			Convert_19_to_20.Root({urn:schemas-microsoft-com:xslt-debug}runtime);
		}

		private static void Root(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			output.WriteStartRoot();
			Convert_19_to_20.<xsl:apply-templates> (2)({urn:schemas-microsoft-com:xslt-debug}runtime, {urn:schemas-microsoft-com:xslt-debug}runtime.ExternalContext.DefaultDataSource);
			output.WriteEndRoot();
		}

		private static IList<XPathNavigator> <xsl:key name="IdToValue" match="VLConfig:Value" use="@z:Id">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator xPathNavigator, string key)
		{
			XmlILIndex xmlILIndex;
			if (!{urn:schemas-microsoft-com:xslt-debug}runtime.FindIndex(xPathNavigator, 0, out xmlILIndex))
			{
				int arg_76_2 = 0;
				XmlILIndex arg_6E_0;
				XmlILIndex arg_76_3 = arg_6E_0 = xmlILIndex;
				XPathNavigator xPathNavigator2 = Convert_19_to_20.SyncToNavigator(xPathNavigator2, xPathNavigator);
				xPathNavigator2.MoveToRoot();
				DescendantIterator descendantIterator;
				descendantIterator.Create(xPathNavigator2, {urn:schemas-microsoft-com:xslt-debug}runtime.GetNameFilter(1), false);
				while (descendantIterator.MoveNext())
				{
					XPathNavigator xPathNavigator3 = Convert_19_to_20.SyncToNavigator(xPathNavigator3, descendantIterator.Current);
					if (xPathNavigator3.MoveToAttribute({urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(4)))
					{
						arg_6E_0.Add(xPathNavigator3.Value, descendantIterator.Current);
						arg_76_3 = (arg_6E_0 = xmlILIndex);
					}
				}
				{urn:schemas-microsoft-com:xslt-debug}runtime.AddNewIndex(xPathNavigator, arg_76_2, arg_76_3);
			}
			return xmlILIndex.Lookup(key);
		}

		private static void <xsl:template name="newline">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			output.WriteRaw("\n");
		}

		private static void <xsl:template match="comment() | processing-instruction()">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current, string indent)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			Convert_19_to_20.<xsl:template name="newline">({urn:schemas-microsoft-com:xslt-debug}runtime);
			output.WriteString(indent);
			if (output.StartCopy({urn:schemas-microsoft-com:xslt-debug}current))
			{
				output.EndCopy({urn:schemas-microsoft-com:xslt-debug}current);
			}
		}

		private static void <xsl:template match="text()">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current, string indent)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			Convert_19_to_20.<xsl:template name="newline">({urn:schemas-microsoft-com:xslt-debug}runtime);
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
			output.WriteStringUnchecked("20");
			output.WriteEndElement();
		}

		private static void 9">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			output.XsltCopyOf({urn:schemas-microsoft-com:xslt-debug}current);
			output.WriteStartElement("", "HardwareConfigurationMultibusChannelConfiguration", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteNamespaceDeclarationUnchecked("i", "http://www.w3.org/2001/XMLSchema-instance");
			output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("", "MultibusChannelConfigurationChannelList", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.WriteStartAttributeUnchecked("z", "Size", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteEndElementUnchecked("", "MultibusChannelConfigurationChannelList", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteEndElement();
		}

		private static void <xsl:template match="VLConfig:LINChannelConfigurationLINChannelList" priority="1">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			output.WriteStartElement("", "LINChannelConfigurationCANChannelNrUsedForLINProbe", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteNamespaceDeclarationUnchecked("i", "http://www.w3.org/2001/XMLSchema-instance");
			output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			if (string.Equals(Convert_19_to_20.LoggerTypeName({urn:schemas-microsoft-com:xslt-debug}runtime), "GL1000"))
			{
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteStringUnchecked("2");
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			}
			else if (string.Equals(Convert_19_to_20.LoggerTypeName({urn:schemas-microsoft-com:xslt-debug}runtime), "GL2000"))
			{
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteStringUnchecked("4");
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			}
			else
			{
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteStringUnchecked("9");
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			}
			output.WriteEndElement();
			output.WriteStartElement("", "LINChannelConfigurationIsUsingLINProbe", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
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
			output.WriteStartElement("", "LINChannelConfigurationLINChannelList", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteNamespaceDeclarationUnchecked("i", "http://www.w3.org/2001/XMLSchema-instance");
			output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.WriteStartAttributeUnchecked("z", "Size", "http://schemas.microsoft.com/2003/10/Serialization/");
			XmlQueryOutput arg_2D3_0 = output;
			XPathNavigator xPathNavigator = Convert_19_to_20.SyncToNavigator(xPathNavigator, {urn:schemas-microsoft-com:xslt-debug}current);
			arg_2D3_0.WriteStringUnchecked((!xPathNavigator.MoveToAttribute({urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(5), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(4))) ? "" : xPathNavigator.Value);
			output.WriteEndAttributeUnchecked();
			ElementContentIterator elementContentIterator;
			elementContentIterator.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(6), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
			while (elementContentIterator.MoveNext())
			{
				output.WriteStartElement("", "LINChannel", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				ElementContentIterator elementContentIterator2;
				elementContentIterator2.Create(elementContentIterator.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(7), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
				while (elementContentIterator2.MoveNext())
				{
					XPathItem current = elementContentIterator2.Current;
					output.WriteItem(current);
				}
				ElementContentIterator elementContentIterator3;
				elementContentIterator3.Create(elementContentIterator.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(8), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
				while (elementContentIterator3.MoveNext())
				{
					XPathItem current2 = elementContentIterator3.Current;
					output.WriteItem(current2);
				}
				ElementContentIterator elementContentIterator4;
				elementContentIterator4.Create(elementContentIterator.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(9), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
				while (elementContentIterator4.MoveNext())
				{
					XPathItem current3 = elementContentIterator4.Current;
					output.WriteItem(current3);
				}
				ElementContentIterator elementContentIterator5;
				elementContentIterator5.Create(elementContentIterator.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(10), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
				while (elementContentIterator5.MoveNext())
				{
					XPathItem current4 = elementContentIterator5.Current;
					output.WriteItem(current4);
				}
				output.WriteStartElement("", "LINChannelProtocolVersion", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteStringUnchecked("6");
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElement();
				output.WriteStartElementUnchecked("", "LINChannelUseDbConfigValues", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteStringUnchecked("false");
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "LINChannelUseDbConfigValues", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElement();
			}
			output.WriteEndElement();
			if (string.Equals(Convert_19_to_20.LoggerTypeName({urn:schemas-microsoft-com:xslt-debug}runtime), "GL1000") || string.Equals(Convert_19_to_20.LoggerTypeName({urn:schemas-microsoft-com:xslt-debug}runtime), "GL2000"))
			{
				output.WriteStartElement("", "LINChannelConfigurationLINprobeChannelList", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteNamespaceDeclaration("z", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteNamespaceDeclaration("i", "http://www.w3.org/2001/XMLSchema-instance");
				output.WriteNamespaceDeclaration("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.WriteStartAttributeUnchecked("z", "Size", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("10");
				output.WriteEndAttributeUnchecked();
				Convert_19_to_20.<xsl:template name="InsertLINprobeChannels">({urn:schemas-microsoft-com:xslt-debug}runtime, 10.0);
				output.WriteEndElement();
			}
			else if (string.Equals(Convert_19_to_20.LoggerTypeName({urn:schemas-microsoft-com:xslt-debug}runtime), "GL3000") || string.Equals(Convert_19_to_20.LoggerTypeName({urn:schemas-microsoft-com:xslt-debug}runtime), "GL4000"))
			{
				output.WriteStartElement("", "LINChannelConfigurationLINprobeChannelList", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteNamespaceDeclaration("z", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteNamespaceDeclaration("i", "http://www.w3.org/2001/XMLSchema-instance");
				output.WriteNamespaceDeclaration("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.WriteStartAttributeUnchecked("z", "Size", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("14");
				output.WriteEndAttributeUnchecked();
				Convert_19_to_20.<xsl:template name="InsertLINprobeChannels">({urn:schemas-microsoft-com:xslt-debug}runtime, 14.0);
				output.WriteEndElement();
			}
			else
			{
				output.WriteStartElement("", "LINChannelConfigurationLINprobeChannelList", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
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
		}

		private static void <xsl:template name="InsertLINprobeChannels">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, double NumberOfChannels)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			if (NumberOfChannels > 0.0)
			{
				output.WriteStartElement("", "LINprobeChannel", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteNamespaceDeclarationUnchecked("i", "http://www.w3.org/2001/XMLSchema-instance");
				output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
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
				output.WriteStartElementUnchecked("", "LINChannelBaudrate", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteStringUnchecked("19200");
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "LINChannelBaudrate", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "LINChannelIsKeepAwakeActive", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteStringUnchecked("false");
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "LINChannelIsKeepAwakeActive", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "LINChannelIsWakeUpEnabled", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteStringUnchecked("true");
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "LINChannelIsWakeUpEnabled", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "LINChannelProtocolVersion", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteStringUnchecked("0");
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "LINChannelProtocolVersion", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "LINChannelUseDbConfigValues", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteStringUnchecked("false");
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "LINChannelUseDbConfigValues", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "LINprobeChannelUseFixLINprobeBaudrate", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				output.WriteStringUnchecked("true");
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "LINprobeChannelUseFixLINprobeBaudrate", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElement();
				Convert_19_to_20.<xsl:template name="InsertLINprobeChannels">({urn:schemas-microsoft-com:xslt-debug}runtime, NumberOfChannels - 1.0);
				return;
			}
		}

		private static void <xsl:template match="VLConfig:CANChannel" priority="1">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			ElementContentIterator elementContentIterator;
			elementContentIterator.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(11), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
			string arg_65_0;
			while (elementContentIterator.MoveNext())
			{
				ElementContentIterator elementContentIterator2;
				elementContentIterator2.Create(elementContentIterator.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(2), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
				if (elementContentIterator2.MoveNext())
				{
					arg_65_0 = elementContentIterator2.Current.Value;
					IL_65:
					double num = XsltConvert.ToDouble(arg_65_0);
					output.WriteStartElement("", "CANChannel", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
					output.WriteNamespaceDeclarationUnchecked("i", "http://www.w3.org/2001/XMLSchema-instance");
					output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
					output.WriteStringUnchecked("0");
					output.WriteEndAttributeUnchecked();
					ElementContentIterator elementContentIterator3;
					elementContentIterator3.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(7), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
					while (elementContentIterator3.MoveNext())
					{
						XPathItem current = elementContentIterator3.Current;
						output.WriteItem(current);
					}
					output.WriteStartElement("", "CANChannelChipConfiguration", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
					output.WriteStringUnchecked("0");
					output.WriteEndAttributeUnchecked();
					output.WriteStartAttributeUnchecked("i", "type", "http://www.w3.org/2001/XMLSchema-instance");
					output.WriteStringUnchecked("CANStdChipConfiguration");
					output.WriteEndAttributeUnchecked();
					if (num == 33333.0)
					{
						output.StartElementContentUnchecked();
						output.WriteStartElementUnchecked("", "CANStdChipConfigurationBTR0", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
						output.WriteStringUnchecked("0");
						output.WriteEndAttributeUnchecked();
						output.StartElementContentUnchecked();
						output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.StartElementContentUnchecked();
						output.WriteStringUnchecked("93");
						output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteEndElementUnchecked("", "CANStdChipConfigurationBTR0", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteStartElementUnchecked("", "CANStdChipConfigurationBTR1", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
						output.WriteStringUnchecked("0");
						output.WriteEndAttributeUnchecked();
						output.StartElementContentUnchecked();
						output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.StartElementContentUnchecked();
						output.WriteStringUnchecked("20");
						output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteEndElementUnchecked("", "CANStdChipConfigurationBTR1", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					}
					else if (num == 50000.0)
					{
						output.StartElementContentUnchecked();
						output.WriteStartElementUnchecked("", "CANStdChipConfigurationBTR0", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
						output.WriteStringUnchecked("0");
						output.WriteEndAttributeUnchecked();
						output.StartElementContentUnchecked();
						output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.StartElementContentUnchecked();
						output.WriteStringUnchecked("83");
						output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteEndElementUnchecked("", "CANStdChipConfigurationBTR0", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteStartElementUnchecked("", "CANStdChipConfigurationBTR1", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
						output.WriteStringUnchecked("0");
						output.WriteEndAttributeUnchecked();
						output.StartElementContentUnchecked();
						output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.StartElementContentUnchecked();
						output.WriteStringUnchecked("20");
						output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteEndElementUnchecked("", "CANStdChipConfigurationBTR1", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					}
					else if (num == 62500.0)
					{
						output.StartElementContentUnchecked();
						output.WriteStartElementUnchecked("", "CANStdChipConfigurationBTR0", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
						output.WriteStringUnchecked("0");
						output.WriteEndAttributeUnchecked();
						output.StartElementContentUnchecked();
						output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.StartElementContentUnchecked();
						output.WriteStringUnchecked("79");
						output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteEndElementUnchecked("", "CANStdChipConfigurationBTR0", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteStartElementUnchecked("", "CANStdChipConfigurationBTR1", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
						output.WriteStringUnchecked("0");
						output.WriteEndAttributeUnchecked();
						output.StartElementContentUnchecked();
						output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.StartElementContentUnchecked();
						output.WriteStringUnchecked("20");
						output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteEndElementUnchecked("", "CANStdChipConfigurationBTR1", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					}
					else if (num == 83333.0)
					{
						output.StartElementContentUnchecked();
						output.WriteStartElementUnchecked("", "CANStdChipConfigurationBTR0", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
						output.WriteStringUnchecked("0");
						output.WriteEndAttributeUnchecked();
						output.StartElementContentUnchecked();
						output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.StartElementContentUnchecked();
						output.WriteStringUnchecked("75");
						output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteEndElementUnchecked("", "CANStdChipConfigurationBTR0", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteStartElementUnchecked("", "CANStdChipConfigurationBTR1", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
						output.WriteStringUnchecked("0");
						output.WriteEndAttributeUnchecked();
						output.StartElementContentUnchecked();
						output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.StartElementContentUnchecked();
						output.WriteStringUnchecked("20");
						output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteEndElementUnchecked("", "CANStdChipConfigurationBTR1", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					}
					else if (num == 100000.0)
					{
						output.StartElementContentUnchecked();
						output.WriteStartElementUnchecked("", "CANStdChipConfigurationBTR0", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
						output.WriteStringUnchecked("0");
						output.WriteEndAttributeUnchecked();
						output.StartElementContentUnchecked();
						output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.StartElementContentUnchecked();
						output.WriteStringUnchecked("73");
						output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteEndElementUnchecked("", "CANStdChipConfigurationBTR0", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteStartElementUnchecked("", "CANStdChipConfigurationBTR1", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
						output.WriteStringUnchecked("0");
						output.WriteEndAttributeUnchecked();
						output.StartElementContentUnchecked();
						output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.StartElementContentUnchecked();
						output.WriteStringUnchecked("20");
						output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteEndElementUnchecked("", "CANStdChipConfigurationBTR1", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					}
					else if (num == 125000.0)
					{
						output.StartElementContentUnchecked();
						output.WriteStartElementUnchecked("", "CANStdChipConfigurationBTR0", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
						output.WriteStringUnchecked("0");
						output.WriteEndAttributeUnchecked();
						output.StartElementContentUnchecked();
						output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.StartElementContentUnchecked();
						output.WriteStringUnchecked("71");
						output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteEndElementUnchecked("", "CANStdChipConfigurationBTR0", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteStartElementUnchecked("", "CANStdChipConfigurationBTR1", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
						output.WriteStringUnchecked("0");
						output.WriteEndAttributeUnchecked();
						output.StartElementContentUnchecked();
						output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.StartElementContentUnchecked();
						output.WriteStringUnchecked("20");
						output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteEndElementUnchecked("", "CANStdChipConfigurationBTR1", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					}
					else if (num == 250000.0)
					{
						output.StartElementContentUnchecked();
						output.WriteStartElementUnchecked("", "CANStdChipConfigurationBTR0", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
						output.WriteStringUnchecked("0");
						output.WriteEndAttributeUnchecked();
						output.StartElementContentUnchecked();
						output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.StartElementContentUnchecked();
						output.WriteStringUnchecked("67");
						output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteEndElementUnchecked("", "CANStdChipConfigurationBTR0", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteStartElementUnchecked("", "CANStdChipConfigurationBTR1", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
						output.WriteStringUnchecked("0");
						output.WriteEndAttributeUnchecked();
						output.StartElementContentUnchecked();
						output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.StartElementContentUnchecked();
						output.WriteStringUnchecked("20");
						output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteEndElementUnchecked("", "CANStdChipConfigurationBTR1", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					}
					else if (num == 500000.0)
					{
						output.StartElementContentUnchecked();
						output.WriteStartElementUnchecked("", "CANStdChipConfigurationBTR0", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
						output.WriteStringUnchecked("0");
						output.WriteEndAttributeUnchecked();
						output.StartElementContentUnchecked();
						output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.StartElementContentUnchecked();
						output.WriteStringUnchecked("65");
						output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteEndElementUnchecked("", "CANStdChipConfigurationBTR0", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteStartElementUnchecked("", "CANStdChipConfigurationBTR1", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
						output.WriteStringUnchecked("0");
						output.WriteEndAttributeUnchecked();
						output.StartElementContentUnchecked();
						output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.StartElementContentUnchecked();
						output.WriteStringUnchecked("20");
						output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteEndElementUnchecked("", "CANStdChipConfigurationBTR1", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					}
					else if (num == 666667.0)
					{
						output.StartElementContentUnchecked();
						output.WriteStartElementUnchecked("", "CANStdChipConfigurationBTR0", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
						output.WriteStringUnchecked("0");
						output.WriteEndAttributeUnchecked();
						output.StartElementContentUnchecked();
						output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.StartElementContentUnchecked();
						output.WriteStringUnchecked("64");
						output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteEndElementUnchecked("", "CANStdChipConfigurationBTR0", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteStartElementUnchecked("", "CANStdChipConfigurationBTR1", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
						output.WriteStringUnchecked("0");
						output.WriteEndAttributeUnchecked();
						output.StartElementContentUnchecked();
						output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.StartElementContentUnchecked();
						output.WriteStringUnchecked("24");
						output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteEndElementUnchecked("", "CANStdChipConfigurationBTR1", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					}
					else if (num == 800000.0)
					{
						output.StartElementContentUnchecked();
						output.WriteStartElementUnchecked("", "CANStdChipConfigurationBTR0", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
						output.WriteStringUnchecked("0");
						output.WriteEndAttributeUnchecked();
						output.StartElementContentUnchecked();
						output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.StartElementContentUnchecked();
						output.WriteStringUnchecked("64");
						output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteEndElementUnchecked("", "CANStdChipConfigurationBTR0", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteStartElementUnchecked("", "CANStdChipConfigurationBTR1", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
						output.WriteStringUnchecked("0");
						output.WriteEndAttributeUnchecked();
						output.StartElementContentUnchecked();
						output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.StartElementContentUnchecked();
						output.WriteStringUnchecked("22");
						output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteEndElementUnchecked("", "CANStdChipConfigurationBTR1", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					}
					else if (num == 1000000.0)
					{
						output.StartElementContentUnchecked();
						output.WriteStartElementUnchecked("", "CANStdChipConfigurationBTR0", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
						output.WriteStringUnchecked("0");
						output.WriteEndAttributeUnchecked();
						output.StartElementContentUnchecked();
						output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.StartElementContentUnchecked();
						output.WriteStringUnchecked("64");
						output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteEndElementUnchecked("", "CANStdChipConfigurationBTR0", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteStartElementUnchecked("", "CANStdChipConfigurationBTR1", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
						output.WriteStringUnchecked("0");
						output.WriteEndAttributeUnchecked();
						output.StartElementContentUnchecked();
						output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.StartElementContentUnchecked();
						output.WriteStringUnchecked("20");
						output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteEndElementUnchecked("", "CANStdChipConfigurationBTR1", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					}
					else
					{
						output.StartElementContentUnchecked();
						output.WriteStartElementUnchecked("", "CANStdChipConfigurationBTR0", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
						output.WriteStringUnchecked("0");
						output.WriteEndAttributeUnchecked();
						output.StartElementContentUnchecked();
						output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.StartElementContentUnchecked();
						output.WriteStringUnchecked("83");
						output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteEndElementUnchecked("", "CANStdChipConfigurationBTR0", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteStartElementUnchecked("", "CANStdChipConfigurationBTR1", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
						output.WriteStringUnchecked("0");
						output.WriteEndAttributeUnchecked();
						output.StartElementContentUnchecked();
						output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.StartElementContentUnchecked();
						output.WriteStringUnchecked("20");
						output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteEndElementUnchecked("", "CANStdChipConfigurationBTR1", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					}
					output.WriteStartElementUnchecked("", "CANStdChipConfigurationQuartzFreq", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
					output.WriteStringUnchecked("0");
					output.WriteEndAttributeUnchecked();
					output.StartElementContentUnchecked();
					output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.StartElementContentUnchecked();
					output.WriteStringUnchecked("16000");
					output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.WriteEndElementUnchecked("", "CANStdChipConfigurationQuartzFreq", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.WriteEndElement();
					ElementContentIterator elementContentIterator4;
					elementContentIterator4.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(12), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
					while (elementContentIterator4.MoveNext())
					{
						XPathItem current2 = elementContentIterator4.Current;
						output.WriteItem(current2);
					}
					ElementContentIterator elementContentIterator5;
					elementContentIterator5.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(13), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
					while (elementContentIterator5.MoveNext())
					{
						XPathItem current3 = elementContentIterator5.Current;
						output.WriteItem(current3);
					}
					ElementContentIterator elementContentIterator6;
					elementContentIterator6.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(14), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
					while (elementContentIterator6.MoveNext())
					{
						XPathItem current4 = elementContentIterator6.Current;
						output.WriteItem(current4);
					}
					output.WriteStartElementUnchecked("", "CANChannelLogErrorFrames", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
					output.WriteStringUnchecked("0");
					output.WriteEndAttributeUnchecked();
					output.StartElementContentUnchecked();
					output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.StartElementContentUnchecked();
					XmlQueryOutput arg_124A_0 = output;
					XPathNavigator xPathNavigator = Convert_19_to_20.SyncToNavigator(xPathNavigator, {urn:schemas-microsoft-com:xslt-debug}current);
					string arg_124A_1;
					if (xPathNavigator.MoveToParent())
					{
						XPathNavigator xPathNavigator2 = Convert_19_to_20.SyncToNavigator(xPathNavigator2, xPathNavigator);
						if (xPathNavigator2.MoveToParent())
						{
							ElementContentIterator elementContentIterator7;
							elementContentIterator7.Create(xPathNavigator2, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(15), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
							while (elementContentIterator7.MoveNext())
							{
								ElementContentIterator elementContentIterator8;
								elementContentIterator8.Create(elementContentIterator7.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(2), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
								if (elementContentIterator8.MoveNext())
								{
									arg_124A_1 = elementContentIterator8.Current.Value;
									goto IL_124A;
								}
							}
						}
					}
					arg_124A_1 = "";
					IL_124A:
					arg_124A_0.WriteStringUnchecked(arg_124A_1);
					output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.WriteEndElementUnchecked("", "CANChannelLogErrorFrames", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.WriteEndElement();
					return;
				}
			}
			arg_65_0 = "";
			goto IL_65;
		}

		private static void 9">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			if (string.Equals(Convert_19_to_20.LoggerTypeName({urn:schemas-microsoft-com:xslt-debug}runtime), "GL3000") || string.Equals(Convert_19_to_20.LoggerTypeName({urn:schemas-microsoft-com:xslt-debug}runtime), "GL4000"))
			{
				output.WriteStartElement("", "CANChannelConfigurationLogErrorFramesOnMemories", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
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
				output.WriteStartElementUnchecked("", "ValidatedProperty", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				XmlQueryOutput arg_15D_0 = output;
				ElementContentIterator elementContentIterator;
				elementContentIterator.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(2), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
				arg_15D_0.WriteStringUnchecked((!elementContentIterator.MoveNext()) ? "" : elementContentIterator.Current.Value);
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "ValidatedProperty", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartElementUnchecked("", "ValidatedProperty", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.StartElementContentUnchecked();
				XmlQueryOutput arg_221_0 = output;
				ElementContentIterator elementContentIterator2;
				elementContentIterator2.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(2), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
				arg_221_0.WriteStringUnchecked((!elementContentIterator2.MoveNext()) ? "" : elementContentIterator2.Current.Value);
				output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElementUnchecked("", "ValidatedProperty", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteEndElement();
			}
			else
			{
				output.WriteStartElement("", "CANChannelConfigurationLogErrorFramesOnMemories", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
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
				output.WriteEndElement();
			}
		}

		private static void <xsl:template match="VLConfig:CcpXcpSignalList" priority="1">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			output.WriteStartElement("", "CcpXcpSignalConfigurationCycleTimeForNonCyclicDaqEvents", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteNamespaceDeclarationUnchecked("i", "http://www.w3.org/2001/XMLSchema-instance");
			output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("10000");
			output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteEndElement();
			output.XsltCopyOf({urn:schemas-microsoft-com:xslt-debug}current);
		}

		private unsafe static void <xsl:template match="VLConfig:CcpXcpEcu" priority="1">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			output.WriteStartElement("", "CcpXcpEcu", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteNamespaceDeclarationUnchecked("i", "http://www.w3.org/2001/XMLSchema-instance");
			output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("", "CcpXcpEcuSettings", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.WriteStartAttributeUnchecked("z", "Size", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("21");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("", "CcpXcpEcuSetting", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("", "CcpXcpEcuSettingType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("CanFirstID");
			output.WriteEndElementUnchecked("", "CcpXcpEcuSettingType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartElementUnchecked("", "CcpXcpEcuSettingValue", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			XmlQueryOutput arg_23C_0 = output;
			ElementContentIterator elementContentIterator;
			elementContentIterator.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(16), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
			string arg_23C_1;
			while (elementContentIterator.MoveNext())
			{
				ElementContentIterator elementContentIterator2;
				elementContentIterator2.Create(elementContentIterator.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(2), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
				if (elementContentIterator2.MoveNext())
				{
					arg_23C_1 = elementContentIterator2.Current.Value;
					IL_23C:
					arg_23C_0.WriteStringUnchecked(arg_23C_1);
					output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.WriteEndElementUnchecked("", "CcpXcpEcuSettingValue", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.WriteEndElementUnchecked("", "CcpXcpEcuSetting", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.WriteStartElementUnchecked("", "CcpXcpEcuSetting", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
					output.WriteStringUnchecked("0");
					output.WriteEndAttributeUnchecked();
					output.StartElementContentUnchecked();
					output.WriteStartElementUnchecked("", "CcpXcpEcuSettingType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.StartElementContentUnchecked();
					output.WriteStringUnchecked("CanRequestID");
					output.WriteEndElementUnchecked("", "CcpXcpEcuSettingType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.WriteStartElementUnchecked("", "CcpXcpEcuSettingValue", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
					output.WriteStringUnchecked("0");
					output.WriteEndAttributeUnchecked();
					output.StartElementContentUnchecked();
					output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
					output.WriteStringUnchecked("0");
					output.WriteEndAttributeUnchecked();
					output.StartElementContentUnchecked();
					XmlQueryOutput arg_3DD_0 = output;
					ElementContentIterator elementContentIterator3;
					elementContentIterator3.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(17), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
					string arg_3DD_1;
					while (elementContentIterator3.MoveNext())
					{
						ElementContentIterator elementContentIterator4;
						elementContentIterator4.Create(elementContentIterator3.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(2), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
						if (elementContentIterator4.MoveNext())
						{
							arg_3DD_1 = elementContentIterator4.Current.Value;
							IL_3DD:
							arg_3DD_0.WriteStringUnchecked(arg_3DD_1);
							output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
							output.WriteEndElementUnchecked("", "CcpXcpEcuSettingValue", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
							output.WriteEndElementUnchecked("", "CcpXcpEcuSetting", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
							output.WriteStartElementUnchecked("", "CcpXcpEcuSetting", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
							output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
							output.WriteStringUnchecked("0");
							output.WriteEndAttributeUnchecked();
							output.StartElementContentUnchecked();
							output.WriteStartElementUnchecked("", "CcpXcpEcuSettingType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
							output.StartElementContentUnchecked();
							output.WriteStringUnchecked("CanResponseID");
							output.WriteEndElementUnchecked("", "CcpXcpEcuSettingType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
							output.WriteStartElementUnchecked("", "CcpXcpEcuSettingValue", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
							output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
							output.WriteStringUnchecked("0");
							output.WriteEndAttributeUnchecked();
							output.StartElementContentUnchecked();
							output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
							output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
							output.WriteStringUnchecked("0");
							output.WriteEndAttributeUnchecked();
							output.StartElementContentUnchecked();
							XmlQueryOutput arg_57E_0 = output;
							ElementContentIterator elementContentIterator5;
							elementContentIterator5.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(18), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
							string arg_57E_1;
							while (elementContentIterator5.MoveNext())
							{
								ElementContentIterator elementContentIterator6;
								elementContentIterator6.Create(elementContentIterator5.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(2), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
								if (elementContentIterator6.MoveNext())
								{
									arg_57E_1 = elementContentIterator6.Current.Value;
									IL_57E:
									arg_57E_0.WriteStringUnchecked(arg_57E_1);
									output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
									output.WriteEndElementUnchecked("", "CcpXcpEcuSettingValue", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
									output.WriteEndElementUnchecked("", "CcpXcpEcuSetting", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
									output.WriteStartElementUnchecked("", "CcpXcpEcuSetting", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
									output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
									output.WriteStringUnchecked("0");
									output.WriteEndAttributeUnchecked();
									output.StartElementContentUnchecked();
									output.WriteStartElementUnchecked("", "CcpXcpEcuSettingType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
									output.StartElementContentUnchecked();
									output.WriteStringUnchecked("DaqTimeout");
									output.WriteEndElementUnchecked("", "CcpXcpEcuSettingType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
									output.WriteStartElementUnchecked("", "CcpXcpEcuSettingValue", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
									output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
									output.WriteStringUnchecked("0");
									output.WriteEndAttributeUnchecked();
									output.StartElementContentUnchecked();
									output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
									output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
									output.WriteStringUnchecked("0");
									output.WriteEndAttributeUnchecked();
									output.StartElementContentUnchecked();
									XmlQueryOutput arg_71F_0 = output;
									ElementContentIterator elementContentIterator7;
									elementContentIterator7.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(19), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
									string arg_71F_1;
									while (elementContentIterator7.MoveNext())
									{
										ElementContentIterator elementContentIterator8;
										elementContentIterator8.Create(elementContentIterator7.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(2), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
										if (elementContentIterator8.MoveNext())
										{
											arg_71F_1 = elementContentIterator8.Current.Value;
											IL_71F:
											arg_71F_0.WriteStringUnchecked(arg_71F_1);
											output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
											output.WriteEndElementUnchecked("", "CcpXcpEcuSettingValue", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
											output.WriteEndElementUnchecked("", "CcpXcpEcuSetting", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
											output.WriteStartElementUnchecked("", "CcpXcpEcuSetting", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
											output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
											output.WriteStringUnchecked("0");
											output.WriteEndAttributeUnchecked();
											output.StartElementContentUnchecked();
											output.WriteStartElementUnchecked("", "CcpXcpEcuSettingType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
											output.StartElementContentUnchecked();
											output.WriteStringUnchecked("EcuDisplayName");
											output.WriteEndElementUnchecked("", "CcpXcpEcuSettingType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
											output.WriteStartElementUnchecked("", "CcpXcpEcuSettingValue", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
											output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
											output.WriteStringUnchecked("0");
											output.WriteEndAttributeUnchecked();
											output.StartElementContentUnchecked();
											output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
											output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
											output.WriteStringUnchecked("0");
											output.WriteEndAttributeUnchecked();
											output.StartElementContentUnchecked();
											XmlQueryOutput arg_9FD_0 = output;
											ElementContentIterator elementContentIterator9;
											elementContentIterator9.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(20), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
											string arg_9FD_1;
											while (elementContentIterator9.MoveNext())
											{
												ElementContentIterator elementContentIterator10;
												elementContentIterator10.Create(elementContentIterator9.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(2), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
												while (elementContentIterator10.MoveNext())
												{
													XPathNavigator xPathNavigator = Convert_19_to_20.SyncToNavigator(xPathNavigator, elementContentIterator10.Current);
													if (xPathNavigator.MoveToAttribute({urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(21), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(4)))
													{
														ElementContentIterator elementContentIterator11;
														elementContentIterator11.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(20), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
														string arg_9BD_0;
														while (elementContentIterator11.MoveNext())
														{
															ElementContentIterator elementContentIterator12;
															elementContentIterator12.Create(elementContentIterator11.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(2), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
															while (elementContentIterator12.MoveNext())
															{
																XPathNavigator xPathNavigator2 = Convert_19_to_20.SyncToNavigator(xPathNavigator2, elementContentIterator12.Current);
																if (xPathNavigator2.MoveToAttribute({urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(21), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(4)))
																{
																	arg_9BD_0 = xPathNavigator2.Value;
																	goto IL_9BD;
																}
															}
															continue;
															IL_9BD:
															IList<XPathNavigator> list = Convert_19_to_20.<xsl:key name="IdToValue" match="VLConfig:Value" use="@z:Id">({urn:schemas-microsoft-com:xslt-debug}runtime, {urn:schemas-microsoft-com:xslt-debug}current, XsltConvert.ToString(XsltConvert.ToDouble(arg_9BD_0)));
															int num = -1;
															num++;
															arg_9FD_1 = ((num >= list.Count) ? "" : list[num].Value);
															goto IL_9FD;
														}
														arg_9BD_0 = "";
														goto IL_9BD;
													}
												}
												continue;
												IL_9FD:
												arg_9FD_0.WriteStringUnchecked(arg_9FD_1);
												output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
												output.WriteEndElementUnchecked("", "CcpXcpEcuSettingValue", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
												output.WriteEndElementUnchecked("", "CcpXcpEcuSetting", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
												output.WriteStartElementUnchecked("", "CcpXcpEcuSetting", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
												output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
												output.WriteStringUnchecked("0");
												output.WriteEndAttributeUnchecked();
												output.StartElementContentUnchecked();
												output.WriteStartElementUnchecked("", "CcpXcpEcuSettingType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
												output.StartElementContentUnchecked();
												output.WriteStringUnchecked("EthernetHost");
												output.WriteEndElementUnchecked("", "CcpXcpEcuSettingType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
												output.WriteStartElementUnchecked("", "CcpXcpEcuSettingValue", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
												output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
												output.WriteStringUnchecked("0");
												output.WriteEndAttributeUnchecked();
												output.StartElementContentUnchecked();
												output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
												output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
												output.WriteStringUnchecked("0");
												output.WriteEndAttributeUnchecked();
												output.StartElementContentUnchecked();
												XmlQueryOutput arg_CDB_0 = output;
												ElementContentIterator elementContentIterator13;
												elementContentIterator13.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(22), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
												string arg_CDB_1;
												while (elementContentIterator13.MoveNext())
												{
													ElementContentIterator elementContentIterator14;
													elementContentIterator14.Create(elementContentIterator13.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(2), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
													while (elementContentIterator14.MoveNext())
													{
														XPathNavigator xPathNavigator3 = Convert_19_to_20.SyncToNavigator(xPathNavigator3, elementContentIterator14.Current);
														if (xPathNavigator3.MoveToAttribute({urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(21), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(4)))
														{
															ElementContentIterator elementContentIterator15;
															elementContentIterator15.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(22), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
															string arg_C9B_0;
															while (elementContentIterator15.MoveNext())
															{
																ElementContentIterator elementContentIterator16;
																elementContentIterator16.Create(elementContentIterator15.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(2), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
																while (elementContentIterator16.MoveNext())
																{
																	XPathNavigator xPathNavigator4 = Convert_19_to_20.SyncToNavigator(xPathNavigator4, elementContentIterator16.Current);
																	if (xPathNavigator4.MoveToAttribute({urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(21), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(4)))
																	{
																		arg_C9B_0 = xPathNavigator4.Value;
																		goto IL_C9B;
																	}
																}
																continue;
																IL_C9B:
																IList<XPathNavigator> list2 = Convert_19_to_20.<xsl:key name="IdToValue" match="VLConfig:Value" use="@z:Id">({urn:schemas-microsoft-com:xslt-debug}runtime, {urn:schemas-microsoft-com:xslt-debug}current, XsltConvert.ToString(XsltConvert.ToDouble(arg_C9B_0)));
																int num2 = -1;
																num2++;
																arg_CDB_1 = ((num2 >= list2.Count) ? "" : list2[num2].Value);
																goto IL_CDB;
															}
															arg_C9B_0 = "";
															goto IL_C9B;
														}
													}
													continue;
													IL_CDB:
													arg_CDB_0.WriteStringUnchecked(arg_CDB_1);
													output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
													output.WriteEndElementUnchecked("", "CcpXcpEcuSettingValue", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
													output.WriteEndElementUnchecked("", "CcpXcpEcuSetting", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
													output.WriteStartElementUnchecked("", "CcpXcpEcuSetting", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
													output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
													output.WriteStringUnchecked("0");
													output.WriteEndAttributeUnchecked();
													output.StartElementContentUnchecked();
													output.WriteStartElementUnchecked("", "CcpXcpEcuSettingType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
													output.StartElementContentUnchecked();
													output.WriteStringUnchecked("EthernetPort");
													output.WriteEndElementUnchecked("", "CcpXcpEcuSettingType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
													output.WriteStartElementUnchecked("", "CcpXcpEcuSettingValue", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
													output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
													output.WriteStringUnchecked("0");
													output.WriteEndAttributeUnchecked();
													output.StartElementContentUnchecked();
													output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
													output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
													output.WriteStringUnchecked("0");
													output.WriteEndAttributeUnchecked();
													output.StartElementContentUnchecked();
													XmlQueryOutput arg_E7C_0 = output;
													ElementContentIterator elementContentIterator17;
													elementContentIterator17.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(23), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
													string arg_E7C_1;
													while (elementContentIterator17.MoveNext())
													{
														ElementContentIterator elementContentIterator18;
														elementContentIterator18.Create(elementContentIterator17.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(2), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
														if (elementContentIterator18.MoveNext())
														{
															arg_E7C_1 = elementContentIterator18.Current.Value;
															IL_E7C:
															arg_E7C_0.WriteStringUnchecked(arg_E7C_1);
															output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
															output.WriteEndElementUnchecked("", "CcpXcpEcuSettingValue", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
															output.WriteEndElementUnchecked("", "CcpXcpEcuSetting", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
															output.WriteStartElementUnchecked("", "CcpXcpEcuSetting", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
															output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
															output.WriteStringUnchecked("0");
															output.WriteEndAttributeUnchecked();
															output.StartElementContentUnchecked();
															output.WriteStartElementUnchecked("", "CcpXcpEcuSettingType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
															output.StartElementContentUnchecked();
															output.WriteStringUnchecked("EthernetPort2");
															output.WriteEndElementUnchecked("", "CcpXcpEcuSettingType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
															output.WriteStartElementUnchecked("", "CcpXcpEcuSettingValue", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
															output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
															output.WriteStringUnchecked("0");
															output.WriteEndAttributeUnchecked();
															output.StartElementContentUnchecked();
															output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
															output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
															output.WriteStringUnchecked("0");
															output.WriteEndAttributeUnchecked();
															output.StartElementContentUnchecked();
															XmlQueryOutput arg_101D_0 = output;
															ElementContentIterator elementContentIterator19;
															elementContentIterator19.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(24), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
															string arg_101D_1;
															while (elementContentIterator19.MoveNext())
															{
																ElementContentIterator elementContentIterator20;
																elementContentIterator20.Create(elementContentIterator19.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(2), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
																if (elementContentIterator20.MoveNext())
																{
																	arg_101D_1 = elementContentIterator20.Current.Value;
																	IL_101D:
																	arg_101D_0.WriteStringUnchecked(arg_101D_1);
																	output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																	output.WriteEndElementUnchecked("", "CcpXcpEcuSettingValue", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																	output.WriteEndElementUnchecked("", "CcpXcpEcuSetting", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																	output.WriteStartElementUnchecked("", "CcpXcpEcuSetting", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																	output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
																	output.WriteStringUnchecked("0");
																	output.WriteEndAttributeUnchecked();
																	output.StartElementContentUnchecked();
																	output.WriteStartElementUnchecked("", "CcpXcpEcuSettingType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																	output.StartElementContentUnchecked();
																	output.WriteStringUnchecked("EthernetProtocol");
																	output.WriteEndElementUnchecked("", "CcpXcpEcuSettingType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																	output.WriteStartElementUnchecked("", "CcpXcpEcuSettingValue", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																	output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
																	output.WriteStringUnchecked("0");
																	output.WriteEndAttributeUnchecked();
																	output.StartElementContentUnchecked();
																	output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																	output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
																	output.WriteStringUnchecked("0");
																	output.WriteEndAttributeUnchecked();
																	output.StartElementContentUnchecked();
																	XmlQueryOutput arg_11BE_0 = output;
																	ElementContentIterator elementContentIterator21;
																	elementContentIterator21.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(25), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
																	string arg_11BE_1;
																	while (elementContentIterator21.MoveNext())
																	{
																		ElementContentIterator elementContentIterator22;
																		elementContentIterator22.Create(elementContentIterator21.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(2), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
																		if (elementContentIterator22.MoveNext())
																		{
																			arg_11BE_1 = elementContentIterator22.Current.Value;
																			IL_11BE:
																			arg_11BE_0.WriteStringUnchecked(arg_11BE_1);
																			output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																			output.WriteEndElementUnchecked("", "CcpXcpEcuSettingValue", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																			output.WriteEndElementUnchecked("", "CcpXcpEcuSetting", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																			output.WriteStartElementUnchecked("", "CcpXcpEcuSetting", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
																			output.WriteStringUnchecked("0");
																			output.WriteEndAttributeUnchecked();
																			output.StartElementContentUnchecked();
																			output.WriteStartElementUnchecked("", "CcpXcpEcuSettingType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																			output.StartElementContentUnchecked();
																			output.WriteStringUnchecked("FlexRayEcuTxQueue");
																			output.WriteEndElementUnchecked("", "CcpXcpEcuSettingType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																			output.WriteStartElementUnchecked("", "CcpXcpEcuSettingValue", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
																			output.WriteStringUnchecked("0");
																			output.WriteEndAttributeUnchecked();
																			output.StartElementContentUnchecked();
																			output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
																			output.WriteStringUnchecked("0");
																			output.WriteEndAttributeUnchecked();
																			output.StartElementContentUnchecked();
																			XmlQueryOutput arg_135F_0 = output;
																			ElementContentIterator elementContentIterator23;
																			elementContentIterator23.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(26), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
																			string arg_135F_1;
																			while (elementContentIterator23.MoveNext())
																			{
																				ElementContentIterator elementContentIterator24;
																				elementContentIterator24.Create(elementContentIterator23.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(2), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
																				if (elementContentIterator24.MoveNext())
																				{
																					arg_135F_1 = elementContentIterator24.Current.Value;
																					IL_135F:
																					arg_135F_0.WriteStringUnchecked(arg_135F_1);
																					output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																					output.WriteEndElementUnchecked("", "CcpXcpEcuSettingValue", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																					output.WriteEndElementUnchecked("", "CcpXcpEcuSetting", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																					output.WriteStartElementUnchecked("", "CcpXcpEcuSetting", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																					output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
																					output.WriteStringUnchecked("0");
																					output.WriteEndAttributeUnchecked();
																					output.StartElementContentUnchecked();
																					output.WriteStartElementUnchecked("", "CcpXcpEcuSettingType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																					output.StartElementContentUnchecked();
																					output.WriteStringUnchecked("FlexRayXcpNodeAdress");
																					output.WriteEndElementUnchecked("", "CcpXcpEcuSettingType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																					output.WriteStartElementUnchecked("", "CcpXcpEcuSettingValue", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																					output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
																					output.WriteStringUnchecked("0");
																					output.WriteEndAttributeUnchecked();
																					output.StartElementContentUnchecked();
																					output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																					output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
																					output.WriteStringUnchecked("0");
																					output.WriteEndAttributeUnchecked();
																					output.StartElementContentUnchecked();
																					XmlQueryOutput arg_1500_0 = output;
																					ElementContentIterator elementContentIterator25;
																					elementContentIterator25.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(27), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
																					string arg_1500_1;
																					while (elementContentIterator25.MoveNext())
																					{
																						ElementContentIterator elementContentIterator26;
																						elementContentIterator26.Create(elementContentIterator25.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(2), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
																						if (elementContentIterator26.MoveNext())
																						{
																							arg_1500_1 = elementContentIterator26.Current.Value;
																							IL_1500:
																							arg_1500_0.WriteStringUnchecked(arg_1500_1);
																							output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																							output.WriteEndElementUnchecked("", "CcpXcpEcuSettingValue", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																							output.WriteEndElementUnchecked("", "CcpXcpEcuSetting", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																							output.WriteStartElementUnchecked("", "CcpXcpEcuSetting", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																							output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
																							output.WriteStringUnchecked("0");
																							output.WriteEndAttributeUnchecked();
																							output.StartElementContentUnchecked();
																							output.WriteStartElementUnchecked("", "CcpXcpEcuSettingType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																							output.StartElementContentUnchecked();
																							output.WriteStringUnchecked("IsCanFirstIdExtended");
																							output.WriteEndElementUnchecked("", "CcpXcpEcuSettingType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																							output.WriteStartElementUnchecked("", "CcpXcpEcuSettingValue", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																							output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
																							output.WriteStringUnchecked("0");
																							output.WriteEndAttributeUnchecked();
																							output.StartElementContentUnchecked();
																							output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																							output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
																							output.WriteStringUnchecked("0");
																							output.WriteEndAttributeUnchecked();
																							output.StartElementContentUnchecked();
																							XmlQueryOutput arg_16A1_0 = output;
																							ElementContentIterator elementContentIterator27;
																							elementContentIterator27.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(28), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
																							string arg_16A1_1;
																							while (elementContentIterator27.MoveNext())
																							{
																								ElementContentIterator elementContentIterator28;
																								elementContentIterator28.Create(elementContentIterator27.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(2), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
																								if (elementContentIterator28.MoveNext())
																								{
																									arg_16A1_1 = elementContentIterator28.Current.Value;
																									IL_16A1:
																									arg_16A1_0.WriteStringUnchecked(arg_16A1_1);
																									output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																									output.WriteEndElementUnchecked("", "CcpXcpEcuSettingValue", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																									output.WriteEndElementUnchecked("", "CcpXcpEcuSetting", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																									output.WriteStartElementUnchecked("", "CcpXcpEcuSetting", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																									output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
																									output.WriteStringUnchecked("0");
																									output.WriteEndAttributeUnchecked();
																									output.StartElementContentUnchecked();
																									output.WriteStartElementUnchecked("", "CcpXcpEcuSettingType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																									output.StartElementContentUnchecked();
																									output.WriteStringUnchecked("IsCanRequestIdExtended");
																									output.WriteEndElementUnchecked("", "CcpXcpEcuSettingType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																									output.WriteStartElementUnchecked("", "CcpXcpEcuSettingValue", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																									output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
																									output.WriteStringUnchecked("0");
																									output.WriteEndAttributeUnchecked();
																									output.StartElementContentUnchecked();
																									output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																									output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
																									output.WriteStringUnchecked("0");
																									output.WriteEndAttributeUnchecked();
																									output.StartElementContentUnchecked();
																									XmlQueryOutput arg_1842_0 = output;
																									ElementContentIterator elementContentIterator29;
																									elementContentIterator29.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(29), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
																									string arg_1842_1;
																									while (elementContentIterator29.MoveNext())
																									{
																										ElementContentIterator elementContentIterator30;
																										elementContentIterator30.Create(elementContentIterator29.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(2), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
																										if (elementContentIterator30.MoveNext())
																										{
																											arg_1842_1 = elementContentIterator30.Current.Value;
																											IL_1842:
																											arg_1842_0.WriteStringUnchecked(arg_1842_1);
																											output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																											output.WriteEndElementUnchecked("", "CcpXcpEcuSettingValue", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																											output.WriteEndElementUnchecked("", "CcpXcpEcuSetting", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																											output.WriteStartElementUnchecked("", "CcpXcpEcuSetting", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																											output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
																											output.WriteStringUnchecked("0");
																											output.WriteEndAttributeUnchecked();
																											output.StartElementContentUnchecked();
																											output.WriteStartElementUnchecked("", "CcpXcpEcuSettingType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																											output.StartElementContentUnchecked();
																											output.WriteStringUnchecked("IsCanResponseIdExtended");
																											output.WriteEndElementUnchecked("", "CcpXcpEcuSettingType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																											output.WriteStartElementUnchecked("", "CcpXcpEcuSettingValue", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																											output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
																											output.WriteStringUnchecked("0");
																											output.WriteEndAttributeUnchecked();
																											output.StartElementContentUnchecked();
																											output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																											output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
																											output.WriteStringUnchecked("0");
																											output.WriteEndAttributeUnchecked();
																											output.StartElementContentUnchecked();
																											XmlQueryOutput arg_19E3_0 = output;
																											ElementContentIterator elementContentIterator31;
																											elementContentIterator31.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(30), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
																											string arg_19E3_1;
																											while (elementContentIterator31.MoveNext())
																											{
																												ElementContentIterator elementContentIterator32;
																												elementContentIterator32.Create(elementContentIterator31.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(2), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
																												if (elementContentIterator32.MoveNext())
																												{
																													arg_19E3_1 = elementContentIterator32.Current.Value;
																													IL_19E3:
																													arg_19E3_0.WriteStringUnchecked(arg_19E3_1);
																													output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																													output.WriteEndElementUnchecked("", "CcpXcpEcuSettingValue", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																													output.WriteEndElementUnchecked("", "CcpXcpEcuSetting", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																													output.WriteStartElementUnchecked("", "CcpXcpEcuSetting", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																													output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
																													output.WriteStringUnchecked("0");
																													output.WriteEndAttributeUnchecked();
																													output.StartElementContentUnchecked();
																													output.WriteStartElementUnchecked("", "CcpXcpEcuSettingType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																													output.StartElementContentUnchecked();
																													output.WriteStringUnchecked("IsSeedAndKeyUsed");
																													output.WriteEndElementUnchecked("", "CcpXcpEcuSettingType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																													output.WriteStartElementUnchecked("", "CcpXcpEcuSettingValue", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																													output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
																													output.WriteStringUnchecked("0");
																													output.WriteEndAttributeUnchecked();
																													output.StartElementContentUnchecked();
																													output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																													output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
																													output.WriteStringUnchecked("0");
																													output.WriteEndAttributeUnchecked();
																													output.StartElementContentUnchecked();
																													XmlQueryOutput arg_1B84_0 = output;
																													ElementContentIterator elementContentIterator33;
																													elementContentIterator33.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(31), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
																													string arg_1B84_1;
																													while (elementContentIterator33.MoveNext())
																													{
																														ElementContentIterator elementContentIterator34;
																														elementContentIterator34.Create(elementContentIterator33.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(2), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
																														if (elementContentIterator34.MoveNext())
																														{
																															arg_1B84_1 = elementContentIterator34.Current.Value;
																															IL_1B84:
																															arg_1B84_0.WriteStringUnchecked(arg_1B84_1);
																															output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																															output.WriteEndElementUnchecked("", "CcpXcpEcuSettingValue", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																															output.WriteEndElementUnchecked("", "CcpXcpEcuSetting", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																															output.WriteStartElementUnchecked("", "CcpXcpEcuSetting", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																															output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
																															output.WriteStringUnchecked("0");
																															output.WriteEndAttributeUnchecked();
																															output.StartElementContentUnchecked();
																															output.WriteStartElementUnchecked("", "CcpXcpEcuSettingType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																															output.StartElementContentUnchecked();
																															output.WriteStringUnchecked("MaxCTO");
																															output.WriteEndElementUnchecked("", "CcpXcpEcuSettingType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																															output.WriteStartElementUnchecked("", "CcpXcpEcuSettingValue", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																															output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
																															output.WriteStringUnchecked("0");
																															output.WriteEndAttributeUnchecked();
																															output.StartElementContentUnchecked();
																															output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																															output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
																															output.WriteStringUnchecked("0");
																															output.WriteEndAttributeUnchecked();
																															output.StartElementContentUnchecked();
																															XmlQueryOutput arg_1D25_0 = output;
																															ElementContentIterator elementContentIterator35;
																															elementContentIterator35.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(32), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
																															string arg_1D25_1;
																															while (elementContentIterator35.MoveNext())
																															{
																																ElementContentIterator elementContentIterator36;
																																elementContentIterator36.Create(elementContentIterator35.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(2), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
																																if (elementContentIterator36.MoveNext())
																																{
																																	arg_1D25_1 = elementContentIterator36.Current.Value;
																																	IL_1D25:
																																	arg_1D25_0.WriteStringUnchecked(arg_1D25_1);
																																	output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																																	output.WriteEndElementUnchecked("", "CcpXcpEcuSettingValue", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																																	output.WriteEndElementUnchecked("", "CcpXcpEcuSetting", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																																	output.WriteStartElementUnchecked("", "CcpXcpEcuSetting", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																																	output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
																																	output.WriteStringUnchecked("0");
																																	output.WriteEndAttributeUnchecked();
																																	output.StartElementContentUnchecked();
																																	output.WriteStartElementUnchecked("", "CcpXcpEcuSettingType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																																	output.StartElementContentUnchecked();
																																	output.WriteStringUnchecked("MaxDTO");
																																	output.WriteEndElementUnchecked("", "CcpXcpEcuSettingType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																																	output.WriteStartElementUnchecked("", "CcpXcpEcuSettingValue", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																																	output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
																																	output.WriteStringUnchecked("0");
																																	output.WriteEndAttributeUnchecked();
																																	output.StartElementContentUnchecked();
																																	output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																																	output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
																																	output.WriteStringUnchecked("0");
																																	output.WriteEndAttributeUnchecked();
																																	output.StartElementContentUnchecked();
																																	XmlQueryOutput arg_1EC6_0 = output;
																																	ElementContentIterator elementContentIterator37;
																																	elementContentIterator37.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(33), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
																																	string arg_1EC6_1;
																																	while (elementContentIterator37.MoveNext())
																																	{
																																		ElementContentIterator elementContentIterator38;
																																		elementContentIterator38.Create(elementContentIterator37.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(2), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
																																		if (elementContentIterator38.MoveNext())
																																		{
																																			arg_1EC6_1 = elementContentIterator38.Current.Value;
																																			IL_1EC6:
																																			arg_1EC6_0.WriteStringUnchecked(arg_1EC6_1);
																																			output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																																			output.WriteEndElementUnchecked("", "CcpXcpEcuSettingValue", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																																			output.WriteEndElementUnchecked("", "CcpXcpEcuSetting", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																																			output.WriteStartElementUnchecked("", "CcpXcpEcuSetting", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																																			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
																																			output.WriteStringUnchecked("0");
																																			output.WriteEndAttributeUnchecked();
																																			output.StartElementContentUnchecked();
																																			output.WriteStartElementUnchecked("", "CcpXcpEcuSettingType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																																			output.StartElementContentUnchecked();
																																			output.WriteStringUnchecked("SendSetSessionStatus");
																																			output.WriteEndElementUnchecked("", "CcpXcpEcuSettingType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																																			output.WriteStartElementUnchecked("", "CcpXcpEcuSettingValue", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																																			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
																																			output.WriteStringUnchecked("0");
																																			output.WriteEndAttributeUnchecked();
																																			output.StartElementContentUnchecked();
																																			output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																																			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
																																			output.WriteStringUnchecked("0");
																																			output.WriteEndAttributeUnchecked();
																																			output.StartElementContentUnchecked();
																																			XmlQueryOutput arg_2067_0 = output;
																																			ElementContentIterator elementContentIterator39;
																																			elementContentIterator39.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(34), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
																																			string arg_2067_1;
																																			while (elementContentIterator39.MoveNext())
																																			{
																																				ElementContentIterator elementContentIterator40;
																																				elementContentIterator40.Create(elementContentIterator39.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(2), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
																																				if (elementContentIterator40.MoveNext())
																																				{
																																					arg_2067_1 = elementContentIterator40.Current.Value;
																																					IL_2067:
																																					arg_2067_0.WriteStringUnchecked(arg_2067_1);
																																					output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																																					output.WriteEndElementUnchecked("", "CcpXcpEcuSettingValue", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																																					output.WriteEndElementUnchecked("", "CcpXcpEcuSetting", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																																					output.WriteStartElementUnchecked("", "CcpXcpEcuSetting", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																																					output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
																																					output.WriteStringUnchecked("0");
																																					output.WriteEndAttributeUnchecked();
																																					output.StartElementContentUnchecked();
																																					output.WriteStartElementUnchecked("", "CcpXcpEcuSettingType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																																					output.StartElementContentUnchecked();
																																					output.WriteStringUnchecked("TransportLayerInstanceName");
																																					output.WriteEndElementUnchecked("", "CcpXcpEcuSettingType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																																					output.WriteStartElementUnchecked("", "CcpXcpEcuSettingValue", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																																					output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
																																					output.WriteStringUnchecked("0");
																																					output.WriteEndAttributeUnchecked();
																																					output.StartElementContentUnchecked();
																																					output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																																					output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
																																					output.WriteStringUnchecked("0");
																																					output.WriteEndAttributeUnchecked();
																																					output.StartElementContentUnchecked();
																																					XmlQueryOutput arg_2349_0 = output;
																																					ElementContentIterator elementContentIterator41;
																																					elementContentIterator41.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(35), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
																																					string arg_2349_1;
																																					while (elementContentIterator41.MoveNext())
																																					{
																																						ElementContentIterator elementContentIterator42;
																																						elementContentIterator42.Create(elementContentIterator41.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(2), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
																																						while (elementContentIterator42.MoveNext())
																																						{
																																							XPathNavigator xPathNavigator5 = Convert_19_to_20.SyncToNavigator(xPathNavigator5, elementContentIterator42.Current);
																																							if (xPathNavigator5.MoveToAttribute({urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(21), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(4)))
																																							{
																																								DodSequenceMerge dodSequenceMerge;
																																								dodSequenceMerge.Create({urn:schemas-microsoft-com:xslt-debug}runtime);
																																								DodSequenceMerge* arg_2313_0 = ref dodSequenceMerge;
																																								DodSequenceMerge* arg_230A_0 = ref dodSequenceMerge;
																																								ElementContentIterator elementContentIterator43;
																																								elementContentIterator43.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(35), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
																																								while (elementContentIterator43.MoveNext())
																																								{
																																									ElementContentIterator elementContentIterator44;
																																									elementContentIterator44.Create(elementContentIterator43.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(2), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
																																									while (elementContentIterator44.MoveNext())
																																									{
																																										XPathNavigator xPathNavigator6 = Convert_19_to_20.SyncToNavigator(xPathNavigator6, elementContentIterator44.Current);
																																										if (xPathNavigator6.MoveToAttribute({urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(21), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(4)))
																																										{
																																											arg_230A_0.AddSequence(Convert_19_to_20.<xsl:key name="IdToValue" match="VLConfig:Value" use="@z:Id">({urn:schemas-microsoft-com:xslt-debug}runtime, {urn:schemas-microsoft-com:xslt-debug}current, xPathNavigator6.Value));
																																											arg_2313_0 = ref dodSequenceMerge;
																																											arg_230A_0 = ref dodSequenceMerge;
																																										}
																																									}
																																								}
																																								IList<XPathNavigator> list3 = arg_2313_0.MergeSequences();
																																								int num3 = -1;
																																								num3++;
																																								arg_2349_1 = ((num3 >= list3.Count) ? "" : list3[num3].Value);
																																								goto IL_2349;
																																							}
																																						}
																																						continue;
																																						IL_2349:
																																						arg_2349_0.WriteStringUnchecked(arg_2349_1);
																																						output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																																						output.WriteEndElementUnchecked("", "CcpXcpEcuSettingValue", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																																						output.WriteEndElementUnchecked("", "CcpXcpEcuSetting", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																																						output.WriteStartElementUnchecked("", "CcpXcpEcuSetting", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																																						output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
																																						output.WriteStringUnchecked("0");
																																						output.WriteEndAttributeUnchecked();
																																						output.StartElementContentUnchecked();
																																						output.WriteStartElementUnchecked("", "CcpXcpEcuSettingType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																																						output.StartElementContentUnchecked();
																																						output.WriteStringUnchecked("UseDbParams");
																																						output.WriteEndElementUnchecked("", "CcpXcpEcuSettingType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																																						output.WriteStartElementUnchecked("", "CcpXcpEcuSettingValue", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																																						output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
																																						output.WriteStringUnchecked("0");
																																						output.WriteEndAttributeUnchecked();
																																						output.StartElementContentUnchecked();
																																						output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																																						output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
																																						output.WriteStringUnchecked("0");
																																						output.WriteEndAttributeUnchecked();
																																						output.StartElementContentUnchecked();
																																						XmlQueryOutput arg_24EA_0 = output;
																																						ElementContentIterator elementContentIterator45;
																																						elementContentIterator45.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(36), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
																																						string arg_24EA_1;
																																						while (elementContentIterator45.MoveNext())
																																						{
																																							ElementContentIterator elementContentIterator46;
																																							elementContentIterator46.Create(elementContentIterator45.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(2), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
																																							if (elementContentIterator46.MoveNext())
																																							{
																																								arg_24EA_1 = elementContentIterator46.Current.Value;
																																								IL_24EA:
																																								arg_24EA_0.WriteStringUnchecked(arg_24EA_1);
																																								output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																																								output.WriteEndElementUnchecked("", "CcpXcpEcuSettingValue", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																																								output.WriteEndElementUnchecked("", "CcpXcpEcuSetting", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																																								output.WriteStartElementUnchecked("", "CcpXcpEcuSetting", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																																								output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
																																								output.WriteStringUnchecked("0");
																																								output.WriteEndAttributeUnchecked();
																																								output.StartElementContentUnchecked();
																																								output.WriteStartElementUnchecked("", "CcpXcpEcuSettingType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																																								output.StartElementContentUnchecked();
																																								output.WriteStringUnchecked("UseVxModule");
																																								output.WriteEndElementUnchecked("", "CcpXcpEcuSettingType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																																								output.WriteStartElementUnchecked("", "CcpXcpEcuSettingValue", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																																								output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
																																								output.WriteStringUnchecked("0");
																																								output.WriteEndAttributeUnchecked();
																																								output.StartElementContentUnchecked();
																																								output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																																								output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
																																								output.WriteStringUnchecked("0");
																																								output.WriteEndAttributeUnchecked();
																																								output.StartElementContentUnchecked();
																																								XmlQueryOutput arg_268B_0 = output;
																																								ElementContentIterator elementContentIterator47;
																																								elementContentIterator47.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(37), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
																																								string arg_268B_1;
																																								while (elementContentIterator47.MoveNext())
																																								{
																																									ElementContentIterator elementContentIterator48;
																																									elementContentIterator48.Create(elementContentIterator47.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(2), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
																																									if (elementContentIterator48.MoveNext())
																																									{
																																										arg_268B_1 = elementContentIterator48.Current.Value;
																																										IL_268B:
																																										arg_268B_0.WriteStringUnchecked(arg_268B_1);
																																										output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																																										output.WriteEndElementUnchecked("", "CcpXcpEcuSettingValue", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																																										output.WriteEndElementUnchecked("", "CcpXcpEcuSetting", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																																										output.WriteEndElementUnchecked("", "CcpXcpEcuSettings", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
																																										output.WriteEndElement();
																																										return;
																																									}
																																								}
																																								arg_268B_1 = "";
																																								goto IL_268B;
																																							}
																																						}
																																						arg_24EA_1 = "";
																																						goto IL_24EA;
																																					}
																																					ElementContentIterator elementContentIterator49;
																																					elementContentIterator49.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(35), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
																																					while (elementContentIterator49.MoveNext())
																																					{
																																						ElementContentIterator elementContentIterator50;
																																						elementContentIterator50.Create(elementContentIterator49.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(2), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
																																						if (elementContentIterator50.MoveNext())
																																						{
																																							arg_2349_1 = elementContentIterator50.Current.Value;
																																							IL_227D:
																																							goto IL_2349;
																																						}
																																					}
																																					arg_2349_1 = "";
																																					goto IL_227D;
																																				}
																																			}
																																			arg_2067_1 = "";
																																			goto IL_2067;
																																		}
																																	}
																																	arg_1EC6_1 = "";
																																	goto IL_1EC6;
																																}
																															}
																															arg_1D25_1 = "";
																															goto IL_1D25;
																														}
																													}
																													arg_1B84_1 = "";
																													goto IL_1B84;
																												}
																											}
																											arg_19E3_1 = "";
																											goto IL_19E3;
																										}
																									}
																									arg_1842_1 = "";
																									goto IL_1842;
																								}
																							}
																							arg_16A1_1 = "";
																							goto IL_16A1;
																						}
																					}
																					arg_1500_1 = "";
																					goto IL_1500;
																				}
																			}
																			arg_135F_1 = "";
																			goto IL_135F;
																		}
																	}
																	arg_11BE_1 = "";
																	goto IL_11BE;
																}
															}
															arg_101D_1 = "";
															goto IL_101D;
														}
													}
													arg_E7C_1 = "";
													goto IL_E7C;
												}
												ElementContentIterator elementContentIterator51;
												elementContentIterator51.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(22), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
												while (elementContentIterator51.MoveNext())
												{
													ElementContentIterator elementContentIterator52;
													elementContentIterator52.Create(elementContentIterator51.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(2), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
													if (elementContentIterator52.MoveNext())
													{
														arg_CDB_1 = elementContentIterator52.Current.Value;
														IL_C13:
														goto IL_CDB;
													}
												}
												arg_CDB_1 = "";
												goto IL_C13;
											}
											ElementContentIterator elementContentIterator53;
											elementContentIterator53.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(20), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
											while (elementContentIterator53.MoveNext())
											{
												ElementContentIterator elementContentIterator54;
												elementContentIterator54.Create(elementContentIterator53.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(2), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
												if (elementContentIterator54.MoveNext())
												{
													arg_9FD_1 = elementContentIterator54.Current.Value;
													IL_935:
													goto IL_9FD;
												}
											}
											arg_9FD_1 = "";
											goto IL_935;
										}
									}
									arg_71F_1 = "";
									goto IL_71F;
								}
							}
							arg_57E_1 = "";
							goto IL_57E;
						}
					}
					arg_3DD_1 = "";
					goto IL_3DD;
				}
			}
			arg_23C_1 = "";
			goto IL_23C;
		}

		private static void 5">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			XPathNavigator xPathNavigator = Convert_19_to_20.SyncToNavigator(xPathNavigator, {urn:schemas-microsoft-com:xslt-debug}current);
			if (xPathNavigator.MoveToAttribute({urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(21), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(4)))
			{
				output.WriteStartElement("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteNamespaceDeclarationUnchecked("i", "http://www.w3.org/2001/XMLSchema-instance");
				output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
				output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
				output.WriteStringUnchecked("0");
				output.WriteEndAttributeUnchecked();
				output.StartElementContentUnchecked();
				XmlQueryOutput arg_109_0 = output;
				XPathNavigator xPathNavigator2 = Convert_19_to_20.SyncToNavigator(xPathNavigator2, {urn:schemas-microsoft-com:xslt-debug}current);
				string arg_109_1;
				if (xPathNavigator2.MoveToAttribute({urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(21), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(4)))
				{
					IList<XPathNavigator> list = Convert_19_to_20.<xsl:key name="IdToValue" match="VLConfig:Value" use="@z:Id">({urn:schemas-microsoft-com:xslt-debug}runtime, {urn:schemas-microsoft-com:xslt-debug}current, xPathNavigator2.Value);
					int num = -1;
					num++;
					if (num < list.Count)
					{
						arg_109_1 = list[num].Value;
						goto IL_109;
					}
				}
				arg_109_1 = "";
				IL_109:
				arg_109_0.WriteStringUnchecked(arg_109_1);
				output.WriteEndElement();
			}
			else
			{
				output.XsltCopyOf({urn:schemas-microsoft-com:xslt-debug}current);
			}
		}

		private static void <xsl:template match="node()">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current, string indent)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			Convert_19_to_20.<xsl:template name="newline">({urn:schemas-microsoft-com:xslt-debug}runtime);
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
							Convert_19_to_20.<xsl:apply-templates>({urn:schemas-microsoft-com:xslt-debug}runtime, unionIterator.Current, indent + XsltConvert.ToString(Convert_19_to_20.indent-increment({urn:schemas-microsoft-com:xslt-debug}runtime)));
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
					Convert_19_to_20.<xsl:template name="newline">({urn:schemas-microsoft-com:xslt-debug}runtime);
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
			int num = (xPathNavigator.NodeType != XPathNodeType.Element) ? (((1 << (int)xPathNavigator.NodeType & 112) == 0) ? ((xPathNavigator.NodeType != XPathNodeType.Comment) ? ((xPathNavigator.NodeType != XPathNodeType.ProcessingInstruction) ? -1 : 0) : 1) : ((!string.Equals(XsltFunctions.NormalizeSpace(xPathNavigator.Value), "")) ? 2 : 4)) : ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 38, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 39, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 40, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 41, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 42, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 15, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 43, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 2, 1)) ? -1 : 5) : 6) : 7) : 8) : 9) : 10) : 11) : 12);
			switch ((num <= 3) ? (((1 << (int)xPathNavigator.NodeType & 13) != 0) ? ((num <= -1) ? -1 : num) : 3) : num)
			{
			case 0:
				Convert_19_to_20.<xsl:template match="comment() | processing-instruction()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, indent);
				return;
			case 1:
				Convert_19_to_20.<xsl:template match="comment() | processing-instruction()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, indent);
				return;
			case 2:
				Convert_19_to_20.<xsl:template match="text()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, indent);
				return;
			case 3:
				Convert_19_to_20.<xsl:template match="node()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, indent);
				return;
			case 4:
				break;
			case 5:
				Convert_19_to_20.<xsl:template match="VLConfig:Value" priority="0.5">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			case 6:
				Convert_19_to_20.<xsl:template match="VLConfig:HardwareConfigurationMOST150ChannelConfiguration" priority="0.9">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			case 7:
				Convert_19_to_20.<xsl:template match="VLConfig:CANChannelConfigurationLogErrorFrames" priority="0.9">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			case 8:
				Convert_19_to_20.<xsl:template match="VLConfig:FileFormatVersion" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime);
				return;
			case 9:
				Convert_19_to_20.<xsl:template match="VLConfig:LINChannelConfigurationLINChannelList" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			case 10:
				Convert_19_to_20.<xsl:template match="VLConfig:CANChannel" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			case 11:
				Convert_19_to_20.<xsl:template match="VLConfig:CcpXcpSignalList" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			case 12:
				Convert_19_to_20.<xsl:template match="VLConfig:CcpXcpEcu" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			default:
				if ((1 << (int)xPathNavigator.NodeType & 508) == 0)
				{
					ContentIterator contentIterator;
					contentIterator.Create(xPathNavigator);
					while (contentIterator.MoveNext())
					{
						Convert_19_to_20.<xsl:apply-templates> (2)({urn:schemas-microsoft-com:xslt-debug}runtime, contentIterator.Current);
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
			int num = (xPathNavigator.NodeType != XPathNodeType.Element) ? (((1 << (int)xPathNavigator.NodeType & 112) == 0) ? ((xPathNavigator.NodeType != XPathNodeType.Comment) ? ((xPathNavigator.NodeType != XPathNodeType.ProcessingInstruction) ? -1 : 0) : 1) : ((!string.Equals(XsltFunctions.NormalizeSpace(xPathNavigator.Value), "")) ? 2 : 4)) : ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 38, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 39, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 40, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 41, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 42, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 15, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 43, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 2, 1)) ? -1 : 5) : 6) : 7) : 8) : 9) : 10) : 11) : 12);
			switch ((num <= 3) ? (((1 << (int)xPathNavigator.NodeType & 13) != 0) ? ((num <= -1) ? -1 : num) : 3) : num)
			{
			case 0:
				Convert_19_to_20.<xsl:template match="comment() | processing-instruction()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, "");
				return;
			case 1:
				Convert_19_to_20.<xsl:template match="comment() | processing-instruction()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, "");
				return;
			case 2:
				Convert_19_to_20.<xsl:template match="text()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, "");
				return;
			case 3:
				Convert_19_to_20.<xsl:template match="node()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, "");
				return;
			case 4:
				break;
			case 5:
				Convert_19_to_20.<xsl:template match="VLConfig:Value" priority="0.5">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			case 6:
				Convert_19_to_20.<xsl:template match="VLConfig:HardwareConfigurationMOST150ChannelConfiguration" priority="0.9">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			case 7:
				Convert_19_to_20.<xsl:template match="VLConfig:CANChannelConfigurationLogErrorFrames" priority="0.9">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			case 8:
				Convert_19_to_20.<xsl:template match="VLConfig:FileFormatVersion" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime);
				return;
			case 9:
				Convert_19_to_20.<xsl:template match="VLConfig:LINChannelConfigurationLINChannelList" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			case 10:
				Convert_19_to_20.<xsl:template match="VLConfig:CANChannel" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			case 11:
				Convert_19_to_20.<xsl:template match="VLConfig:CcpXcpSignalList" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			case 12:
				Convert_19_to_20.<xsl:template match="VLConfig:CcpXcpEcu" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			default:
				if ((1 << (int)xPathNavigator.NodeType & 508) == 0)
				{
					ContentIterator contentIterator;
					contentIterator.Create(xPathNavigator);
					while (contentIterator.MoveNext())
					{
						Convert_19_to_20.<xsl:apply-templates> (2)({urn:schemas-microsoft-com:xslt-debug}runtime, contentIterator.Current);
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
				XPathNavigator xPathNavigator2 = Convert_19_to_20.SyncToNavigator(xPathNavigator2, {urn:schemas-microsoft-com:xslt-debug}runtime.ExternalContext.DefaultDataSource);
				xPathNavigator2.MoveToRoot();
				xPathNavigator = Convert_19_to_20.SyncToNavigator(arg_2A_0, xPathNavigator2);
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
