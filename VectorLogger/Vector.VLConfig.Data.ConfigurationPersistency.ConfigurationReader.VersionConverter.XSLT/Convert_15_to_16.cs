using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Xml.XPath;
using System.Xml.Xsl.Runtime;

namespace Vector.VLConfig.Data.ConfigurationPersistency.ConfigurationReader.VersionConverter.XSLT
{
	[GeneratedCode("System.Xml.Xsl.XslCompiledTransform", "2.0.0.0"), ComVisible(true)]
	public static class Convert_15_to_16
	{
		private static $ArrayType$724 __staticData;

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
			21,
			0,
			0,
			0,
			20,
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
			40,
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
			80,
			101,
			114,
			109,
			97,
			110,
			101,
			110,
			116,
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
			46,
			83,
			112,
			101,
			99,
			105,
			97,
			108,
			70,
			101,
			97,
			116,
			117,
			114,
			101,
			115,
			73,
			115,
			67,
			108,
			111,
			115,
			101,
			76,
			111,
			103,
			70,
			105,
			108,
			101,
			79,
			110,
			86,
			66,
			97,
			116,
			116,
			79,
			102,
			102,
			69,
			110,
			97,
			98,
			108,
			101,
			100,
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
			76,
			105,
			115,
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
			Convert_15_to_16.Root({urn:schemas-microsoft-com:xslt-debug}runtime);
		}

		private static void Root(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			output.WriteStartRoot();
			Convert_15_to_16.<xsl:apply-templates> (2)({urn:schemas-microsoft-com:xslt-debug}runtime, {urn:schemas-microsoft-com:xslt-debug}runtime.ExternalContext.DefaultDataSource);
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
			Convert_15_to_16.<xsl:template name="newline">({urn:schemas-microsoft-com:xslt-debug}runtime);
			output.WriteString(indent);
			if (output.StartCopy({urn:schemas-microsoft-com:xslt-debug}current))
			{
				output.EndCopy({urn:schemas-microsoft-com:xslt-debug}current);
			}
		}

		private static void <xsl:template match="text()">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current, string indent)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			Convert_15_to_16.<xsl:template name="newline">({urn:schemas-microsoft-com:xslt-debug}runtime);
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
			output.WriteStringUnchecked("16");
			output.WriteEndElement();
			output.WriteStartElement("", "MetaInformation", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteNamespaceDeclarationUnchecked("i", "http://www.w3.org/2001/XMLSchema-instance");
			output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("", "MetaInformationBufferSizeCalculatorInformation", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("", "BufferSizeCalculatorInformationChannelItems", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.WriteStartAttributeUnchecked("z", "Size", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteEndElementUnchecked("", "BufferSizeCalculatorInformationChannelItems", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartElementUnchecked("", "BufferSizeCalculatorInformationPreTriggerTimeSeconds", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("0");
			output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteEndElementUnchecked("", "BufferSizeCalculatorInformationPreTriggerTimeSeconds", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteEndElementUnchecked("", "MetaInformationBufferSizeCalculatorInformation", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteEndElement();
		}

		private static void <xsl:template match="VLConfig:LoggingConfigurationTriggerConfigList">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			int arg_2B_0;
			int arg_2E_0 = arg_2B_0 = 0;
			ElementContentIterator elementContentIterator;
			elementContentIterator.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(0), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
			while (elementContentIterator.MoveNext())
			{
				arg_2E_0 = ++arg_2B_0;
			}
			double num = XsltConvert.ToDouble(arg_2E_0);
			output.WriteStartElement("", "LoggingConfigurationTriggerConfigList", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclaration("z", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteNamespaceDeclaration("i", "http://www.w3.org/2001/XMLSchema-instance");
			output.WriteNamespaceDeclaration("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.WriteStartAttributeUnchecked("z", "Size", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked(XsltConvert.ToString(num));
			output.WriteEndAttributeUnchecked();
			int num2 = 0;
			ElementContentIterator elementContentIterator2;
			elementContentIterator2.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(0), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
			while (elementContentIterator2.MoveNext())
			{
				num2++;
				Convert_15_to_16.<xsl:template name="CopyTriggerConfigurationAndAddNames">({urn:schemas-microsoft-com:xslt-debug}runtime, elementContentIterator2.Current, num, XsltConvert.ToDouble(num2));
			}
			output.WriteEndElement();
		}

		private static void <xsl:template name="CopyTriggerConfigurationAndAddNames">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current, double numOfMemories, double currentMemory)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			output.WriteStartElement("", "TriggerConfiguration", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteNamespaceDeclarationUnchecked("i", "http://www.w3.org/2001/XMLSchema-instance");
			output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("", "TriggerConfigurationMemoryRingBuffer", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			ElementContentIterator elementContentIterator;
			elementContentIterator.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(2), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
			while (elementContentIterator.MoveNext())
			{
				ElementContentIterator elementContentIterator2;
				elementContentIterator2.Create(elementContentIterator.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(3), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
				while (elementContentIterator2.MoveNext())
				{
					XPathItem current = elementContentIterator2.Current;
					output.WriteItem(current);
				}
			}
			output.WriteStartElement("", "MemoryRingBufferSize", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.StartElementContentUnchecked();
			XmlQueryOutput arg_1D9_0 = output;
			ElementContentIterator elementContentIterator3;
			elementContentIterator3.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(2), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
			string arg_1C5_0;
			while (elementContentIterator3.MoveNext())
			{
				ElementContentIterator elementContentIterator4;
				elementContentIterator4.Create(elementContentIterator3.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(4), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
				if (elementContentIterator4.MoveNext())
				{
					arg_1C5_0 = elementContentIterator4.Current.Value;
					IL_1C5:
					arg_1D9_0.WriteStringUnchecked(XsltConvert.ToString(XsltConvert.ToDouble(arg_1C5_0) * 1024.0));
					output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.WriteEndElement();
					ElementContentIterator elementContentIterator5;
					elementContentIterator5.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(2), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
					while (elementContentIterator5.MoveNext())
					{
						ElementContentIterator elementContentIterator6;
						elementContentIterator6.Create(elementContentIterator5.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(5), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
						while (elementContentIterator6.MoveNext())
						{
							XPathItem current2 = elementContentIterator6.Current;
							output.WriteItem(current2);
						}
					}
					output.WriteEndElementUnchecked("", "TriggerConfigurationMemoryRingBuffer", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					ElementContentIterator elementContentIterator7;
					elementContentIterator7.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(6), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
					while (elementContentIterator7.MoveNext())
					{
						XPathItem current3 = elementContentIterator7.Current;
						output.WriteItem(current3);
					}
					output.WriteStartElement("", "TriggerConfigurationTriggerList", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
					output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
					output.WriteStringUnchecked("0");
					output.WriteEndAttributeUnchecked();
					output.WriteStartAttributeUnchecked("z", "Size", "http://schemas.microsoft.com/2003/10/Serialization/");
					XmlQueryOutput arg_344_0 = output;
					int arg_337_0;
					int arg_33A_0 = arg_337_0 = 0;
					ElementContentIterator elementContentIterator8;
					elementContentIterator8.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(7), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
					while (elementContentIterator8.MoveNext())
					{
						ElementContentIterator elementContentIterator9;
						elementContentIterator9.Create(elementContentIterator8.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(8), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
						while (elementContentIterator9.MoveNext())
						{
							arg_33A_0 = ++arg_337_0;
						}
					}
					arg_344_0.WriteStringUnchecked(XsltConvert.ToString(XsltConvert.ToDouble(arg_33A_0)));
					output.WriteEndAttributeUnchecked();
					int num = 0;
					ElementContentIterator elementContentIterator10;
					elementContentIterator10.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(7), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
					while (elementContentIterator10.MoveNext())
					{
						ElementContentIterator elementContentIterator11;
						elementContentIterator11.Create(elementContentIterator10.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(8), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
						while (elementContentIterator11.MoveNext())
						{
							num++;
							output.WriteStartElement("", "RecordTrigger", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
							output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
							output.WriteStringUnchecked("0");
							output.WriteEndAttributeUnchecked();
							ElementContentIterator elementContentIterator12;
							elementContentIterator12.Create(elementContentIterator11.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(9), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
							while (elementContentIterator12.MoveNext())
							{
								XPathItem current4 = elementContentIterator12.Current;
								output.WriteItem(current4);
							}
							ElementContentIterator elementContentIterator13;
							elementContentIterator13.Create(elementContentIterator11.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(10), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
							while (elementContentIterator13.MoveNext())
							{
								XPathItem current5 = elementContentIterator13.Current;
								output.WriteItem(current5);
							}
							ElementContentIterator elementContentIterator14;
							elementContentIterator14.Create(elementContentIterator11.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(11), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
							while (elementContentIterator14.MoveNext())
							{
								XPathItem current6 = elementContentIterator14.Current;
								output.WriteItem(current6);
							}
							ElementContentIterator elementContentIterator15;
							elementContentIterator15.Create(elementContentIterator11.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(12), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
							while (elementContentIterator15.MoveNext())
							{
								XPathItem current7 = elementContentIterator15.Current;
								output.WriteItem(current7);
							}
							ElementContentIterator elementContentIterator16;
							elementContentIterator16.Create(elementContentIterator11.Current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(13), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
							while (elementContentIterator16.MoveNext())
							{
								XPathItem current8 = elementContentIterator16.Current;
								output.WriteItem(current8);
							}
							output.WriteStartElement("", "RecordTriggerName", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
							output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
							output.WriteStringUnchecked("0");
							output.WriteEndAttributeUnchecked();
							output.StartElementContentUnchecked();
							output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
							output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
							output.WriteStringUnchecked("0");
							output.WriteEndAttributeUnchecked();
							if (numOfMemories > 1.0)
							{
								output.StartElementContentUnchecked();
								output.WriteStringUnchecked("Trigger_");
								output.WriteStringUnchecked(XsltConvert.ToString(currentMemory));
								output.WriteStringUnchecked("_");
								output.WriteStringUnchecked(XsltConvert.ToString(XsltConvert.ToDouble(num)));
							}
							else
							{
								output.StartElementContentUnchecked();
								output.WriteStringUnchecked("Trigger_");
								output.WriteStringUnchecked(XsltConvert.ToString(XsltConvert.ToDouble(num)));
							}
							output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
							output.WriteEndElement();
							output.WriteEndElement();
						}
					}
					output.WriteEndElement();
					ElementContentIterator elementContentIterator17;
					elementContentIterator17.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(14), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
					while (elementContentIterator17.MoveNext())
					{
						XPathItem current9 = elementContentIterator17.Current;
						output.WriteItem(current9);
					}
					ElementContentIterator elementContentIterator18;
					elementContentIterator18.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(15), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
					while (elementContentIterator18.MoveNext())
					{
						XPathItem current10 = elementContentIterator18.Current;
						output.WriteItem(current10);
					}
					ElementContentIterator elementContentIterator19;
					elementContentIterator19.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(16), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
					while (elementContentIterator19.MoveNext())
					{
						XPathItem current11 = elementContentIterator19.Current;
						output.WriteItem(current11);
					}
					output.WriteEndElement();
					return;
				}
			}
			arg_1C5_0 = "";
			goto IL_1C5;
		}

		private static void <xsl:template match="VLConfig:DatabaseCPType" priority="1">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			output.XsltCopyOf({urn:schemas-microsoft-com:xslt-debug}current);
			output.WriteStartElement("", "DatabaseCcpXcpEcuList", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
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

		private static void <xsl:template match="node()">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current, string indent)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			Convert_15_to_16.<xsl:template name="newline">({urn:schemas-microsoft-com:xslt-debug}runtime);
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
							Convert_15_to_16.<xsl:apply-templates>({urn:schemas-microsoft-com:xslt-debug}runtime, unionIterator.Current, indent + XsltConvert.ToString(Convert_15_to_16.indent-increment({urn:schemas-microsoft-com:xslt-debug}runtime)));
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
					Convert_15_to_16.<xsl:template name="newline">({urn:schemas-microsoft-com:xslt-debug}runtime);
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
			int num = (xPathNavigator.NodeType != XPathNodeType.Element) ? (((1 << (int)xPathNavigator.NodeType & 112) == 0) ? ((xPathNavigator.NodeType != XPathNodeType.Comment) ? ((xPathNavigator.NodeType != XPathNodeType.ProcessingInstruction) ? -1 : 0) : 1) : ((!string.Equals(XsltFunctions.NormalizeSpace(xPathNavigator.Value), "")) ? 2 : 5)) : ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 17, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 18, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 19, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 20, 1)) ? -1 : 4) : 6) : 7) : 8);
			switch ((num <= 3) ? (((1 << (int)xPathNavigator.NodeType & 13) != 0) ? ((num <= -1) ? -1 : num) : 3) : num)
			{
			case 0:
				Convert_15_to_16.<xsl:template match="comment() | processing-instruction()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, indent);
				return;
			case 1:
				Convert_15_to_16.<xsl:template match="comment() | processing-instruction()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, indent);
				return;
			case 2:
				Convert_15_to_16.<xsl:template match="text()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, indent);
				return;
			case 3:
				Convert_15_to_16.<xsl:template match="node()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, indent);
				return;
			case 4:
				Convert_15_to_16.<xsl:template match="VLConfig:LoggingConfigurationTriggerConfigList">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			case 5:
				break;
			case 6:
				Convert_15_to_16.<xsl:template match="VLConfig:FileFormatVersion" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime);
				return;
			case 7:
				break;
			case 8:
				Convert_15_to_16.<xsl:template match="VLConfig:DatabaseCPType" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			default:
				if ((1 << (int)xPathNavigator.NodeType & 508) == 0)
				{
					ContentIterator contentIterator;
					contentIterator.Create(xPathNavigator);
					while (contentIterator.MoveNext())
					{
						Convert_15_to_16.<xsl:apply-templates> (2)({urn:schemas-microsoft-com:xslt-debug}runtime, contentIterator.Current);
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
			int num = (xPathNavigator.NodeType != XPathNodeType.Element) ? (((1 << (int)xPathNavigator.NodeType & 112) == 0) ? ((xPathNavigator.NodeType != XPathNodeType.Comment) ? ((xPathNavigator.NodeType != XPathNodeType.ProcessingInstruction) ? -1 : 0) : 1) : ((!string.Equals(XsltFunctions.NormalizeSpace(xPathNavigator.Value), "")) ? 2 : 5)) : ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 17, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 18, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 19, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 20, 1)) ? -1 : 4) : 6) : 7) : 8);
			switch ((num <= 3) ? (((1 << (int)xPathNavigator.NodeType & 13) != 0) ? ((num <= -1) ? -1 : num) : 3) : num)
			{
			case 0:
				Convert_15_to_16.<xsl:template match="comment() | processing-instruction()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, "");
				return;
			case 1:
				Convert_15_to_16.<xsl:template match="comment() | processing-instruction()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, "");
				return;
			case 2:
				Convert_15_to_16.<xsl:template match="text()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, "");
				return;
			case 3:
				Convert_15_to_16.<xsl:template match="node()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, "");
				return;
			case 4:
				Convert_15_to_16.<xsl:template match="VLConfig:LoggingConfigurationTriggerConfigList">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			case 5:
				break;
			case 6:
				Convert_15_to_16.<xsl:template match="VLConfig:FileFormatVersion" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime);
				return;
			case 7:
				break;
			case 8:
				Convert_15_to_16.<xsl:template match="VLConfig:DatabaseCPType" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			default:
				if ((1 << (int)xPathNavigator.NodeType & 508) == 0)
				{
					ContentIterator contentIterator;
					contentIterator.Create(xPathNavigator);
					while (contentIterator.MoveNext())
					{
						Convert_15_to_16.<xsl:apply-templates> (2)({urn:schemas-microsoft-com:xslt-debug}runtime, contentIterator.Current);
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
