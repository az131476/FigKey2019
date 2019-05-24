using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Xml.XPath;
using System.Xml.Xsl.Runtime;

namespace Vector.VLConfig.Data.ConfigurationPersistency.ConfigurationReader.VersionConverter.XSLT
{
	[GeneratedCode("System.Xml.Xsl.XslCompiledTransform", "2.0.0.0"), ComVisible(true)]
	public static class Convert_5_to_6
	{
		private static $ArrayType$382 __staticData;

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
			6,
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
			43,
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
			65,
			117,
			116,
			111,
			99,
			111,
			110,
			110,
			101,
			99,
			116,
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
			65,
			117,
			116,
			111,
			99,
			111,
			110,
			110,
			101,
			99,
			116,
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
			17,
			97,
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
			18,
			97,
			117,
			116,
			111,
			67,
			111,
			110,
			110,
			101,
			99,
			116,
			69,
			110,
			97,
			98,
			108,
			101,
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
			Convert_5_to_6.Root({urn:schemas-microsoft-com:xslt-debug}runtime);
		}

		private static void Root(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			output.WriteStartRoot();
			Convert_5_to_6.<xsl:apply-templates> (2)({urn:schemas-microsoft-com:xslt-debug}runtime, {urn:schemas-microsoft-com:xslt-debug}runtime.ExternalContext.DefaultDataSource);
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
			Convert_5_to_6.<xsl:template name="newline">({urn:schemas-microsoft-com:xslt-debug}runtime);
			output.WriteString(indent);
			if (output.StartCopy({urn:schemas-microsoft-com:xslt-debug}current))
			{
				output.EndCopy({urn:schemas-microsoft-com:xslt-debug}current);
			}
		}

		private static void <xsl:template match="text()">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current, string indent)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			Convert_5_to_6.<xsl:template name="newline">({urn:schemas-microsoft-com:xslt-debug}runtime);
			output.WriteString(indent);
			output.WriteString(XsltFunctions.NormalizeSpace({urn:schemas-microsoft-com:xslt-debug}current.Value));
		}

		private static void <xsl:template match="VLConfig:LoggingConfigurationTriggerConfiguration" priority="1">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current, string indent)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			Convert_5_to_6.<xsl:template name="PrettyPrintNodeRecursive">({urn:schemas-microsoft-com:xslt-debug}runtime, {urn:schemas-microsoft-com:xslt-debug}current, indent);
			Convert_5_to_6.<xsl:template name="newline">({urn:schemas-microsoft-com:xslt-debug}runtime);
			output.WriteStartElement("", "LoggingConfigurationWLANConfiguration", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteNamespaceDeclarationUnchecked("i", "http://www.w3.org/2001/XMLSchema-instance");
			output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("", "WLANConfigurationAnalogInputNumber", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.StartElementContentUnchecked();
			XmlQueryOutput arg_112_0 = output;
			IList<XPathNavigator> list = Convert_5_to_6.analogInputNumber({urn:schemas-microsoft-com:xslt-debug}runtime);
			int num = -1;
			num++;
			arg_112_0.WriteStringUnchecked((num >= list.Count) ? "" : list[num].Value);
			output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteEndElementUnchecked("", "WLANConfigurationAnalogInputNumber", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartElementUnchecked("", "WLANConfigurationIsDownloadClassificationEnabled", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("true");
			output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteEndElementUnchecked("", "WLANConfigurationIsDownloadClassificationEnabled", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartElementUnchecked("", "WLANConfigurationIsDownloadDriveRecorderEnabled", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("true");
			output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteEndElementUnchecked("", "WLANConfigurationIsDownloadDriveRecorderEnabled", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartElementUnchecked("", "WLANConfigurationIsDownloadRingbufferEnabled", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("true");
			output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteEndElementUnchecked("", "WLANConfigurationIsDownloadRingbufferEnabled", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartElementUnchecked("", "WLANConfigurationIsStartWLANOnEventEnabled", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("false");
			output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteEndElementUnchecked("", "WLANConfigurationIsStartWLANOnEventEnabled", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartElementUnchecked("", "WLANConfigurationIsStartWLANOnShutdownEnabled", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.StartElementContentUnchecked();
			XmlQueryOutput arg_416_0 = output;
			IList<XPathNavigator> list2 = Convert_5_to_6.autoConnectEnabled({urn:schemas-microsoft-com:xslt-debug}runtime);
			int num2 = -1;
			num2++;
			arg_416_0.WriteStringUnchecked((num2 >= list2.Count) ? "" : list2[num2].Value);
			output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteEndElementUnchecked("", "WLANConfigurationIsStartWLANOnShutdownEnabled", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartElementUnchecked("", "WLANConfigurationRingbuffersToDownload", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("2147483647");
			output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteEndElementUnchecked("", "WLANConfigurationRingbuffersToDownload", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartElementUnchecked("", "WLANConfigurationStartWLANEvent", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("i", "nil", "http://www.w3.org/2001/XMLSchema-instance");
			output.WriteStringUnchecked("true");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteEndElementUnchecked("", "WLANConfigurationStartWLANEvent", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteEndElement();
		}

		private static void <xsl:template match="VLConfig:FileFormatVersion" priority="1">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			output.WriteStartElement("", "FileFormatVersion", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteNamespaceDeclarationUnchecked("i", "http://www.w3.org/2001/XMLSchema-instance");
			output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("6");
			output.WriteEndElement();
		}

		private static void <xsl:template name="PrettyPrintNodeRecursive">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current, string indent)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			Convert_5_to_6.<xsl:template name="newline">({urn:schemas-microsoft-com:xslt-debug}runtime);
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
							Convert_5_to_6.<xsl:apply-templates>({urn:schemas-microsoft-com:xslt-debug}runtime, unionIterator.Current, indent + XsltConvert.ToString(Convert_5_to_6.indent-increment({urn:schemas-microsoft-com:xslt-debug}runtime)));
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
					Convert_5_to_6.<xsl:template name="newline">({urn:schemas-microsoft-com:xslt-debug}runtime);
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
			Convert_5_to_6.<xsl:template name="PrettyPrintNodeRecursive">({urn:schemas-microsoft-com:xslt-debug}runtime, {urn:schemas-microsoft-com:xslt-debug}current, indent);
		}

		private static void <xsl:apply-templates>(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator xPathNavigator, string indent)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			int arg_D3_0;
			if (xPathNavigator.NodeType == XPathNodeType.Element)
			{
				int num = (!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 4, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 5, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 3, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 2, 1)) ? -1 : 5) : 6) : 7) : 8;
				arg_D3_0 = ((num <= 4) ? 4 : num);
			}
			else
			{
				arg_D3_0 = (((1 << (int)xPathNavigator.NodeType & 112) == 0) ? ((xPathNavigator.NodeType != XPathNodeType.Comment) ? ((xPathNavigator.NodeType != XPathNodeType.ProcessingInstruction) ? -1 : 0) : 1) : ((!string.Equals(XsltFunctions.NormalizeSpace(xPathNavigator.Value), "")) ? 2 : 3));
			}
			switch (arg_D3_0)
			{
			case 0:
				Convert_5_to_6.<xsl:template match="comment() | processing-instruction()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, indent);
				return;
			case 1:
				Convert_5_to_6.<xsl:template match="comment() | processing-instruction()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, indent);
				return;
			case 2:
				Convert_5_to_6.<xsl:template match="text()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, indent);
				return;
			case 3:
				break;
			case 4:
				Convert_5_to_6.<xsl:template match="*" priority="0.5">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, indent);
				return;
			case 5:
				break;
			case 6:
				break;
			case 7:
				Convert_5_to_6.<xsl:template match="VLConfig:LoggingConfigurationTriggerConfiguration" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, indent);
				return;
			case 8:
				Convert_5_to_6.<xsl:template match="VLConfig:FileFormatVersion" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime);
				return;
			default:
				if ((1 << (int)xPathNavigator.NodeType & 508) == 0)
				{
					ContentIterator contentIterator;
					contentIterator.Create(xPathNavigator);
					while (contentIterator.MoveNext())
					{
						Convert_5_to_6.<xsl:apply-templates> (2)({urn:schemas-microsoft-com:xslt-debug}runtime, contentIterator.Current);
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
			int arg_D3_0;
			if (xPathNavigator.NodeType == XPathNodeType.Element)
			{
				int num = (!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 4, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 5, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 3, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 2, 1)) ? -1 : 5) : 6) : 7) : 8;
				arg_D3_0 = ((num <= 4) ? 4 : num);
			}
			else
			{
				arg_D3_0 = (((1 << (int)xPathNavigator.NodeType & 112) == 0) ? ((xPathNavigator.NodeType != XPathNodeType.Comment) ? ((xPathNavigator.NodeType != XPathNodeType.ProcessingInstruction) ? -1 : 0) : 1) : ((!string.Equals(XsltFunctions.NormalizeSpace(xPathNavigator.Value), "")) ? 2 : 3));
			}
			switch (arg_D3_0)
			{
			case 0:
				Convert_5_to_6.<xsl:template match="comment() | processing-instruction()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, "");
				return;
			case 1:
				Convert_5_to_6.<xsl:template match="comment() | processing-instruction()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, "");
				return;
			case 2:
				Convert_5_to_6.<xsl:template match="text()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, "");
				return;
			case 3:
				break;
			case 4:
				Convert_5_to_6.<xsl:template match="*" priority="0.5">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, "");
				return;
			case 5:
				break;
			case 6:
				break;
			case 7:
				Convert_5_to_6.<xsl:template match="VLConfig:LoggingConfigurationTriggerConfiguration" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, "");
				return;
			case 8:
				Convert_5_to_6.<xsl:template match="VLConfig:FileFormatVersion" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime);
				return;
			default:
				if ((1 << (int)xPathNavigator.NodeType & 508) == 0)
				{
					ContentIterator contentIterator;
					contentIterator.Create(xPathNavigator);
					while (contentIterator.MoveNext())
					{
						Convert_5_to_6.<xsl:apply-templates> (2)({urn:schemas-microsoft-com:xslt-debug}runtime, contentIterator.Current);
					}
				}
				else if ((1 << (int)xPathNavigator.NodeType & 395) == 0)
				{
					output.WriteString(xPathNavigator.Value);
				}
				break;
			}
		}

		private unsafe static IList<XPathNavigator> analogInputNumber(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime)
		{
			if (!{urn:schemas-microsoft-com:xslt-debug}runtime.IsGlobalComputed(1))
			{
				int arg_C1_1 = 1;
				XmlQueryNodeSequence xmlQueryNodeSequence = XmlQueryNodeSequence.CreateOrReuse(xmlQueryNodeSequence);
				XmlQueryNodeSequence arg_B7_0 = xmlQueryNodeSequence;
				ContentMergeIterator contentMergeIterator;
				contentMergeIterator.Create({urn:schemas-microsoft-com:xslt-debug}runtime.GetTypeFilter(XPathNodeType.Text));
				ContentMergeIterator contentMergeIterator2;
				contentMergeIterator2.Create({urn:schemas-microsoft-com:xslt-debug}runtime.GetNameFilter(0));
				XPathNavigator xPathNavigator = Convert_5_to_6.SyncToNavigator(xPathNavigator, {urn:schemas-microsoft-com:xslt-debug}runtime.ExternalContext.DefaultDataSource);
				xPathNavigator.MoveToRoot();
				DescendantIterator descendantIterator;
				descendantIterator.Create(xPathNavigator, {urn:schemas-microsoft-com:xslt-debug}runtime.GetNameFilter(1), false);
				while (true)
				{
					if (!descendantIterator.MoveNext())
					{
						goto IL_78;
					}
					ContentMergeIterator* arg_7B_0 = ref contentMergeIterator2;
					XPathNavigator arg_7B_1 = descendantIterator.Current;
					IL_7B:
					ContentMergeIterator* arg_9E_0;
					XPathNavigator arg_9E_1;
					switch (arg_7B_0.MoveNext(arg_7B_1))
					{
					case IteratorResult.NoMoreNodes:
						IL_9B:
						arg_9E_0 = ref contentMergeIterator;
						arg_9E_1 = null;
						break;
					case IteratorResult.NeedInputNode:
						continue;
					default:
						arg_9E_0 = ref contentMergeIterator;
						arg_9E_1 = contentMergeIterator2.Current;
						break;
					}
					switch (arg_9E_0.MoveNext(arg_9E_1))
					{
					case IteratorResult.NoMoreNodes:
						goto IL_BF;
					case IteratorResult.NeedInputNode:
						IL_78:
						arg_7B_0 = ref contentMergeIterator2;
						arg_7B_1 = null;
						goto IL_7B;
					default:
						arg_B7_0.AddClone(contentMergeIterator.Current);
						arg_B7_0 = xmlQueryNodeSequence;
						goto IL_9B;
					}
				}
				IL_BF:
				{urn:schemas-microsoft-com:xslt-debug}runtime.SetGlobalValue(arg_C1_1, xmlQueryNodeSequence);
			}
			return (IList<XPathNavigator>){urn:schemas-microsoft-com:xslt-debug}runtime.GetGlobalValue(1);
		}

		private unsafe static IList<XPathNavigator> autoConnectEnabled(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime)
		{
			if (!{urn:schemas-microsoft-com:xslt-debug}runtime.IsGlobalComputed(2))
			{
				int arg_C1_1 = 2;
				XmlQueryNodeSequence xmlQueryNodeSequence = XmlQueryNodeSequence.CreateOrReuse(xmlQueryNodeSequence);
				XmlQueryNodeSequence arg_B7_0 = xmlQueryNodeSequence;
				ContentMergeIterator contentMergeIterator;
				contentMergeIterator.Create({urn:schemas-microsoft-com:xslt-debug}runtime.GetTypeFilter(XPathNodeType.Text));
				ContentMergeIterator contentMergeIterator2;
				contentMergeIterator2.Create({urn:schemas-microsoft-com:xslt-debug}runtime.GetNameFilter(0));
				XPathNavigator xPathNavigator = Convert_5_to_6.SyncToNavigator(xPathNavigator, {urn:schemas-microsoft-com:xslt-debug}runtime.ExternalContext.DefaultDataSource);
				xPathNavigator.MoveToRoot();
				DescendantIterator descendantIterator;
				descendantIterator.Create(xPathNavigator, {urn:schemas-microsoft-com:xslt-debug}runtime.GetNameFilter(2), false);
				while (true)
				{
					if (!descendantIterator.MoveNext())
					{
						goto IL_78;
					}
					ContentMergeIterator* arg_7B_0 = ref contentMergeIterator2;
					XPathNavigator arg_7B_1 = descendantIterator.Current;
					IL_7B:
					ContentMergeIterator* arg_9E_0;
					XPathNavigator arg_9E_1;
					switch (arg_7B_0.MoveNext(arg_7B_1))
					{
					case IteratorResult.NoMoreNodes:
						IL_9B:
						arg_9E_0 = ref contentMergeIterator;
						arg_9E_1 = null;
						break;
					case IteratorResult.NeedInputNode:
						continue;
					default:
						arg_9E_0 = ref contentMergeIterator;
						arg_9E_1 = contentMergeIterator2.Current;
						break;
					}
					switch (arg_9E_0.MoveNext(arg_9E_1))
					{
					case IteratorResult.NoMoreNodes:
						goto IL_BF;
					case IteratorResult.NeedInputNode:
						IL_78:
						arg_7B_0 = ref contentMergeIterator2;
						arg_7B_1 = null;
						goto IL_7B;
					default:
						arg_B7_0.AddClone(contentMergeIterator.Current);
						arg_B7_0 = xmlQueryNodeSequence;
						goto IL_9B;
					}
				}
				IL_BF:
				{urn:schemas-microsoft-com:xslt-debug}runtime.SetGlobalValue(arg_C1_1, xmlQueryNodeSequence);
			}
			return (IList<XPathNavigator>){urn:schemas-microsoft-com:xslt-debug}runtime.GetGlobalValue(2);
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
