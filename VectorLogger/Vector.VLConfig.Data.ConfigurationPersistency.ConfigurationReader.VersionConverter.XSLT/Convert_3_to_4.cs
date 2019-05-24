using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Xml.XPath;
using System.Xml.Xsl.Runtime;

namespace Vector.VLConfig.Data.ConfigurationPersistency.ConfigurationReader.VersionConverter.XSLT
{
	[GeneratedCode("System.Xml.Xsl.XslCompiledTransform", "2.0.0.0"), ComVisible(true)]
	public static class Convert_3_to_4
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
			10,
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
			44,
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
			73,
			115,
			80,
			101,
			114,
			109,
			97,
			110,
			101,
			110,
			116,
			76,
			111,
			103,
			103,
			105,
			110,
			103,
			65,
			99,
			116,
			105,
			118,
			101,
			38,
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
			73,
			110,
			99,
			108,
			117,
			100,
			101,
			76,
			84,
			76,
			67,
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
			68,
			97,
			116,
			97,
			98,
			97,
			115,
			101,
			76,
			105,
			115,
			116,
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
			24,
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
			84,
			121,
			112,
			101,
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
			Convert_3_to_4.Root({urn:schemas-microsoft-com:xslt-debug}runtime);
		}

		private static void Root(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			output.WriteStartRoot();
			Convert_3_to_4.<xsl:apply-templates> (2)({urn:schemas-microsoft-com:xslt-debug}runtime, {urn:schemas-microsoft-com:xslt-debug}runtime.ExternalContext.DefaultDataSource);
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
			Convert_3_to_4.<xsl:template name="newline">({urn:schemas-microsoft-com:xslt-debug}runtime);
			output.WriteString(indent);
			if (output.StartCopy({urn:schemas-microsoft-com:xslt-debug}current))
			{
				output.EndCopy({urn:schemas-microsoft-com:xslt-debug}current);
			}
		}

		private static void <xsl:template match="text()">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current, string indent)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			Convert_3_to_4.<xsl:template name="newline">({urn:schemas-microsoft-com:xslt-debug}runtime);
			output.WriteString(indent);
			output.WriteString(XsltFunctions.NormalizeSpace({urn:schemas-microsoft-com:xslt-debug}current.Value));
		}

		private static void <xsl:template match="VLConfig:ConfigurationDataModelLoggerType" priority="1">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current, string indent)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			Convert_3_to_4.<xsl:template name="newline">({urn:schemas-microsoft-com:xslt-debug}runtime);
			XmlQueryNodeSequence xmlQueryNodeSequence = XmlQueryNodeSequence.CreateOrReuse(xmlQueryNodeSequence);
			XmlQueryNodeSequence arg_31_0 = xmlQueryNodeSequence;
			NodeKindContentIterator nodeKindContentIterator;
			nodeKindContentIterator.Create({urn:schemas-microsoft-com:xslt-debug}current, XPathNodeType.Text);
			while (nodeKindContentIterator.MoveNext())
			{
				arg_31_0.AddClone(nodeKindContentIterator.Current);
				arg_31_0 = xmlQueryNodeSequence;
			}
			int num = -1;
			XPathNavigator xPathNavigator;
			do
			{
				num++;
				if (num >= ((ICollection<XPathNavigator>)xmlQueryNodeSequence).Count)
				{
					goto IL_6D;
				}
				xPathNavigator = ((IList<XPathNavigator>)xmlQueryNodeSequence)[num];
			}
			while (!string.Equals(xPathNavigator.Value, "GL4200"));
			output.WriteStartElement("", "ConfigurationDataModelLoggerType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("GL4000");
			output.WriteEndElement();
			return;
			IL_6D:
			output.WriteStartElement("", "ConfigurationDataModelLoggerType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.StartElementContentUnchecked();
			XmlQueryOutput arg_D6_0 = output;
			int num2 = -1;
			num2++;
			arg_D6_0.WriteStringUnchecked((num2 >= ((ICollection<XPathNavigator>)xmlQueryNodeSequence).Count) ? "" : ((IList<XPathNavigator>)xmlQueryNodeSequence)[num2].Value);
			output.WriteEndElement();
		}

		private static void <xsl:template match="VLConfig:LogDataStorageMemoryType" priority="1">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current, string indent)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			Convert_3_to_4.<xsl:template name="newline">({urn:schemas-microsoft-com:xslt-debug}runtime);
			ElementContentIterator elementContentIterator;
			elementContentIterator.Create({urn:schemas-microsoft-com:xslt-debug}current, {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(0), {urn:schemas-microsoft-com:xslt-debug}runtime.GetAtomizedName(1));
			while (elementContentIterator.MoveNext())
			{
				NodeKindContentIterator nodeKindContentIterator;
				nodeKindContentIterator.Create(elementContentIterator.Current, XPathNodeType.Text);
				while (nodeKindContentIterator.MoveNext())
				{
					if (string.Equals(nodeKindContentIterator.Current.Value, "HardDisk"))
					{
						output.WriteStartElement("", "LogDataStorageMemoryType", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
						output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
						output.WriteStringUnchecked("0");
						output.WriteEndAttributeUnchecked();
						output.StartElementContentUnchecked();
						output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.StartElementContentUnchecked();
						output.WriteStringUnchecked("CFCardUSB");
						output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
						output.WriteEndElement();
						return;
					}
				}
			}
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
						goto IL_EF;
					}
					XPathNavigator nestedNavigator = nodeKindContentIterator2.Current;
					if (!true)
					{
						goto IL_CB;
					}
					IL_F2:
					XPathNavigator nestedNavigator2;
					NodeKindContentIterator nodeKindContentIterator3;
					switch (unionIterator2.MoveNext(nestedNavigator))
					{
					case SetIteratorResult.NoMoreNodes:
						IL_143:
						nestedNavigator2 = null;
						break;
					case SetIteratorResult.InitRightIterator:
						IL_CB:
						nodeKindContentIterator3.Create({urn:schemas-microsoft-com:xslt-debug}current, XPathNodeType.Text);
						goto IL_D4;
					case SetIteratorResult.NeedLeftNode:
						continue;
					case SetIteratorResult.NeedRightNode:
						goto IL_D4;
					default:
						nestedNavigator2 = unionIterator2.Current;
						if (!true)
						{
							goto IL_11F;
						}
						break;
					}
					IL_146:
					NodeKindContentIterator nodeKindContentIterator4;
					switch (unionIterator.MoveNext(nestedNavigator2))
					{
					case SetIteratorResult.NoMoreNodes:
						goto IL_184;
					case SetIteratorResult.InitRightIterator:
						IL_11F:
						nodeKindContentIterator4.Create({urn:schemas-microsoft-com:xslt-debug}current, XPathNodeType.Comment);
						break;
					case SetIteratorResult.NeedLeftNode:
						IL_EF:
						nestedNavigator = null;
						goto IL_F2;
					case SetIteratorResult.NeedRightNode:
						break;
					default:
						Convert_3_to_4.<xsl:apply-templates>({urn:schemas-microsoft-com:xslt-debug}runtime, unionIterator.Current, indent + XsltConvert.ToString(Convert_3_to_4.indent-increment({urn:schemas-microsoft-com:xslt-debug}runtime)));
						goto IL_143;
					}
					if (!nodeKindContentIterator4.MoveNext())
					{
						goto IL_143;
					}
					nestedNavigator2 = nodeKindContentIterator4.Current;
					if (!true)
					{
						goto IL_143;
					}
					goto IL_146;
					IL_D4:
					if (!nodeKindContentIterator3.MoveNext())
					{
						goto IL_EF;
					}
					nestedNavigator = nodeKindContentIterator3.Current;
					if (!true)
					{
						goto IL_EF;
					}
					goto IL_F2;
				}
				IL_184:
				Convert_3_to_4.<xsl:template name="newline">({urn:schemas-microsoft-com:xslt-debug}runtime);
				output.WriteString(indent);
				output.EndCopy({urn:schemas-microsoft-com:xslt-debug}current);
			}
		}

		private static void <xsl:template match="VLConfig:ConfigurationDataModelHardwareConfiguration" priority="1">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current, string indent)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			output.WriteStartElement("", "ApplicationVersion", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("2.1.2.0");
			output.WriteEndElement();
			Convert_3_to_4.<xsl:template name="PrettyPrintNodeRecursive">({urn:schemas-microsoft-com:xslt-debug}runtime, {urn:schemas-microsoft-com:xslt-debug}current, indent);
		}

		private static void <xsl:template match="VLConfig:DatabaseConfigurationDatabaseList" priority="1">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current, string indent)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			Convert_3_to_4.<xsl:template name="PrettyPrintNodeRecursive">({urn:schemas-microsoft-com:xslt-debug}runtime, {urn:schemas-microsoft-com:xslt-debug}current, indent);
			output.WriteStartElement("", "DatabaseConfigurationEnableExchangeIdHandling", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
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

		private static void <xsl:template match="VLConfig:SpecialFeaturesIsIncludeLTLCodeEnabled" priority="1">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current, string indent)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			output.WriteStartElement("", "SpecialFeaturesIsCloseLogFileOnVBattOffEnabled", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
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
			output.WriteStartElement("", "SpecialFeaturesIsIgnitionKeepsLoggerAwakeEnabled", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
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
			Convert_3_to_4.<xsl:template name="PrettyPrintNodeRecursive">({urn:schemas-microsoft-com:xslt-debug}runtime, {urn:schemas-microsoft-com:xslt-debug}current, indent);
		}

		private static void <xsl:template match="VLConfig:TriggerConfigurationTriggerList" priority="1">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current, string indent)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			Convert_3_to_4.<xsl:template name="PrettyPrintNodeRecursive">({urn:schemas-microsoft-com:xslt-debug}runtime, {urn:schemas-microsoft-com:xslt-debug}current, indent);
			output.WriteStartElement("", "TriggerConfigurationTriggerListOnOff", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
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

		private static void <xsl:template match="VLConfig:FileFormatVersion" priority="1">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			output.WriteStartElement("", "FileFormatVersion", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("4");
			output.WriteEndElement();
		}

		private static void <xsl:template name="PrettyPrintNodeRecursive">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current, string indent)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			Convert_3_to_4.<xsl:template name="newline">({urn:schemas-microsoft-com:xslt-debug}runtime);
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
							Convert_3_to_4.<xsl:apply-templates>({urn:schemas-microsoft-com:xslt-debug}runtime, unionIterator.Current, indent + XsltConvert.ToString(Convert_3_to_4.indent-increment({urn:schemas-microsoft-com:xslt-debug}runtime)));
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
					Convert_3_to_4.<xsl:template name="newline">({urn:schemas-microsoft-com:xslt-debug}runtime);
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
			Convert_3_to_4.<xsl:template name="PrettyPrintNodeRecursive">({urn:schemas-microsoft-com:xslt-debug}runtime, {urn:schemas-microsoft-com:xslt-debug}current, indent);
		}

		private static void <xsl:apply-templates>(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator xPathNavigator, string indent)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			int arg_128_0;
			if (xPathNavigator.NodeType == XPathNodeType.Element)
			{
				int num = (!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 2, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 3, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 4, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 5, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 6, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 7, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 8, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 9, 1)) ? -1 : 5) : 6) : 7) : 8) : 9) : 10) : 11) : 12;
				arg_128_0 = ((num <= 4) ? 4 : num);
			}
			else
			{
				arg_128_0 = (((1 << (int)xPathNavigator.NodeType & 112) == 0) ? ((xPathNavigator.NodeType != XPathNodeType.Comment) ? ((xPathNavigator.NodeType != XPathNodeType.ProcessingInstruction) ? -1 : 0) : 1) : ((!string.Equals(XsltFunctions.NormalizeSpace(xPathNavigator.Value), "")) ? 2 : 3));
			}
			switch (arg_128_0)
			{
			case 0:
				Convert_3_to_4.<xsl:template match="comment() | processing-instruction()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, indent);
				return;
			case 1:
				Convert_3_to_4.<xsl:template match="comment() | processing-instruction()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, indent);
				return;
			case 2:
				Convert_3_to_4.<xsl:template match="text()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, indent);
				return;
			case 3:
				break;
			case 4:
				Convert_3_to_4.<xsl:template match="*" priority="0.5">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, indent);
				return;
			case 5:
				Convert_3_to_4.<xsl:template match="VLConfig:ConfigurationDataModelLoggerType" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, indent);
				return;
			case 6:
				Convert_3_to_4.<xsl:template match="VLConfig:LogDataStorageMemoryType" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, indent);
				return;
			case 7:
				Convert_3_to_4.<xsl:template match="VLConfig:ConfigurationDataModelHardwareConfiguration" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, indent);
				return;
			case 8:
				Convert_3_to_4.<xsl:template match="VLConfig:DatabaseConfigurationDatabaseList" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, indent);
				return;
			case 9:
				Convert_3_to_4.<xsl:template match="VLConfig:SpecialFeaturesIsIncludeLTLCodeEnabled" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, indent);
				return;
			case 10:
				break;
			case 11:
				Convert_3_to_4.<xsl:template match="VLConfig:TriggerConfigurationTriggerList" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, indent);
				return;
			case 12:
				Convert_3_to_4.<xsl:template match="VLConfig:FileFormatVersion" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime);
				return;
			default:
				if ((1 << (int)xPathNavigator.NodeType & 508) == 0)
				{
					ContentIterator contentIterator;
					contentIterator.Create(xPathNavigator);
					while (contentIterator.MoveNext())
					{
						Convert_3_to_4.<xsl:apply-templates> (2)({urn:schemas-microsoft-com:xslt-debug}runtime, contentIterator.Current);
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
			int arg_128_0;
			if (xPathNavigator.NodeType == XPathNodeType.Element)
			{
				int num = (!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 2, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 3, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 4, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 5, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 6, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 7, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 8, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 9, 1)) ? -1 : 5) : 6) : 7) : 8) : 9) : 10) : 11) : 12;
				arg_128_0 = ((num <= 4) ? 4 : num);
			}
			else
			{
				arg_128_0 = (((1 << (int)xPathNavigator.NodeType & 112) == 0) ? ((xPathNavigator.NodeType != XPathNodeType.Comment) ? ((xPathNavigator.NodeType != XPathNodeType.ProcessingInstruction) ? -1 : 0) : 1) : ((!string.Equals(XsltFunctions.NormalizeSpace(xPathNavigator.Value), "")) ? 2 : 3));
			}
			switch (arg_128_0)
			{
			case 0:
				Convert_3_to_4.<xsl:template match="comment() | processing-instruction()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, "");
				return;
			case 1:
				Convert_3_to_4.<xsl:template match="comment() | processing-instruction()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, "");
				return;
			case 2:
				Convert_3_to_4.<xsl:template match="text()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, "");
				return;
			case 3:
				break;
			case 4:
				Convert_3_to_4.<xsl:template match="*" priority="0.5">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, "");
				return;
			case 5:
				Convert_3_to_4.<xsl:template match="VLConfig:ConfigurationDataModelLoggerType" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, "");
				return;
			case 6:
				Convert_3_to_4.<xsl:template match="VLConfig:LogDataStorageMemoryType" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, "");
				return;
			case 7:
				Convert_3_to_4.<xsl:template match="VLConfig:ConfigurationDataModelHardwareConfiguration" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, "");
				return;
			case 8:
				Convert_3_to_4.<xsl:template match="VLConfig:DatabaseConfigurationDatabaseList" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, "");
				return;
			case 9:
				Convert_3_to_4.<xsl:template match="VLConfig:SpecialFeaturesIsIncludeLTLCodeEnabled" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, "");
				return;
			case 10:
				break;
			case 11:
				Convert_3_to_4.<xsl:template match="VLConfig:TriggerConfigurationTriggerList" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, "");
				return;
			case 12:
				Convert_3_to_4.<xsl:template match="VLConfig:FileFormatVersion" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime);
				return;
			default:
				if ((1 << (int)xPathNavigator.NodeType & 508) == 0)
				{
					ContentIterator contentIterator;
					contentIterator.Create(xPathNavigator);
					while (contentIterator.MoveNext())
					{
						Convert_3_to_4.<xsl:apply-templates> (2)({urn:schemas-microsoft-com:xslt-debug}runtime, contentIterator.Current);
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
