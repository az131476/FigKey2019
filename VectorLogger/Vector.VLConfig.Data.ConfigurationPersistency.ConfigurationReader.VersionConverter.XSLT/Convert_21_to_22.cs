using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Xml.XPath;
using System.Xml.Xsl.Runtime;

namespace Vector.VLConfig.Data.ConfigurationPersistency.ConfigurationReader.VersionConverter.XSLT
{
	[GeneratedCode("System.Xml.Xsl.XslCompiledTransform", "2.0.0.0"), ComVisible(true)]
	public static class Convert_21_to_22
	{
		private static $ArrayType$338 __staticData;

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
			13,
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
			Convert_21_to_22.Root({urn:schemas-microsoft-com:xslt-debug}runtime);
		}

		private static void Root(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			output.WriteStartRoot();
			Convert_21_to_22.<xsl:apply-templates> (2)({urn:schemas-microsoft-com:xslt-debug}runtime, {urn:schemas-microsoft-com:xslt-debug}runtime.ExternalContext.DefaultDataSource);
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
			Convert_21_to_22.<xsl:template name="newline">({urn:schemas-microsoft-com:xslt-debug}runtime);
			output.WriteString(indent);
			if (output.StartCopy({urn:schemas-microsoft-com:xslt-debug}current))
			{
				output.EndCopy({urn:schemas-microsoft-com:xslt-debug}current);
			}
		}

		private static void <xsl:template match="text()">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current, string indent)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			Convert_21_to_22.<xsl:template name="newline">({urn:schemas-microsoft-com:xslt-debug}runtime);
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
			output.WriteStringUnchecked("22");
			output.WriteEndElement();
		}

		private static void <xsl:template match="VLConfig:MemoryRingBufferIsActive" priority="1">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			output.XsltCopyOf({urn:schemas-microsoft-com:xslt-debug}current);
			output.WriteStartElement("", "MemoryRingBufferMaxNumOfFiles", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteNamespaceDeclarationUnchecked("i", "http://www.w3.org/2001/XMLSchema-instance");
			output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked("30000");
			output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteEndElement();
			output.WriteStartElement("", "MemoryRingBufferOperatingMode", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteNamespaceDeclarationUnchecked("z", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteNamespaceDeclarationUnchecked("i", "http://www.w3.org/2001/XMLSchema-instance");
			output.WriteNamespaceDeclarationUnchecked("VLConfig", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteStartAttributeUnchecked("z", "Id", "http://schemas.microsoft.com/2003/10/Serialization/");
			output.WriteStringUnchecked("0");
			output.WriteEndAttributeUnchecked();
			output.StartElementContentUnchecked();
			output.WriteStartElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.StartElementContentUnchecked();
			output.WriteStringUnchecked(Convert_21_to_22.OperatingMode({urn:schemas-microsoft-com:xslt-debug}runtime));
			output.WriteEndElementUnchecked("", "Value", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			output.WriteEndElement();
		}

		private static void <xsl:template match="node()">(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime, XPathNavigator {urn:schemas-microsoft-com:xslt-debug}current, string indent)
		{
			XmlQueryOutput output = {urn:schemas-microsoft-com:xslt-debug}runtime.Output;
			Convert_21_to_22.<xsl:template name="newline">({urn:schemas-microsoft-com:xslt-debug}runtime);
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
							Convert_21_to_22.<xsl:apply-templates>({urn:schemas-microsoft-com:xslt-debug}runtime, unionIterator.Current, indent + XsltConvert.ToString(Convert_21_to_22.indent-increment({urn:schemas-microsoft-com:xslt-debug}runtime)));
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
					Convert_21_to_22.<xsl:template name="newline">({urn:schemas-microsoft-com:xslt-debug}runtime);
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
			int num = (xPathNavigator.NodeType != XPathNodeType.Element) ? (((1 << (int)xPathNavigator.NodeType & 112) == 0) ? ((xPathNavigator.NodeType != XPathNodeType.Comment) ? ((xPathNavigator.NodeType != XPathNodeType.ProcessingInstruction) ? -1 : 0) : 1) : ((!string.Equals(XsltFunctions.NormalizeSpace(xPathNavigator.Value), "")) ? 2 : 5)) : ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 4, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 5, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 3, 1)) ? -1 : 4) : 6) : 7);
			switch ((num <= 3) ? (((1 << (int)xPathNavigator.NodeType & 13) != 0) ? ((num <= -1) ? -1 : num) : 3) : num)
			{
			case 0:
				Convert_21_to_22.<xsl:template match="comment() | processing-instruction()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, indent);
				return;
			case 1:
				Convert_21_to_22.<xsl:template match="comment() | processing-instruction()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, indent);
				return;
			case 2:
				Convert_21_to_22.<xsl:template match="text()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, indent);
				return;
			case 3:
				Convert_21_to_22.<xsl:template match="node()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, indent);
				return;
			case 4:
				break;
			case 5:
				break;
			case 6:
				Convert_21_to_22.<xsl:template match="VLConfig:FileFormatVersion" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime);
				return;
			case 7:
				Convert_21_to_22.<xsl:template match="VLConfig:MemoryRingBufferIsActive" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			default:
				if ((1 << (int)xPathNavigator.NodeType & 508) == 0)
				{
					ContentIterator contentIterator;
					contentIterator.Create(xPathNavigator);
					while (contentIterator.MoveNext())
					{
						Convert_21_to_22.<xsl:apply-templates> (2)({urn:schemas-microsoft-com:xslt-debug}runtime, contentIterator.Current);
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
			int num = (xPathNavigator.NodeType != XPathNodeType.Element) ? (((1 << (int)xPathNavigator.NodeType & 112) == 0) ? ((xPathNavigator.NodeType != XPathNodeType.Comment) ? ((xPathNavigator.NodeType != XPathNodeType.ProcessingInstruction) ? -1 : 0) : 1) : ((!string.Equals(XsltFunctions.NormalizeSpace(xPathNavigator.Value), "")) ? 2 : 5)) : ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 4, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 5, 1)) ? ((!{urn:schemas-microsoft-com:xslt-debug}runtime.IsQNameEqual(xPathNavigator, 3, 1)) ? -1 : 4) : 6) : 7);
			switch ((num <= 3) ? (((1 << (int)xPathNavigator.NodeType & 13) != 0) ? ((num <= -1) ? -1 : num) : 3) : num)
			{
			case 0:
				Convert_21_to_22.<xsl:template match="comment() | processing-instruction()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, "");
				return;
			case 1:
				Convert_21_to_22.<xsl:template match="comment() | processing-instruction()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, "");
				return;
			case 2:
				Convert_21_to_22.<xsl:template match="text()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, "");
				return;
			case 3:
				Convert_21_to_22.<xsl:template match="node()">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator, "");
				return;
			case 4:
				break;
			case 5:
				break;
			case 6:
				Convert_21_to_22.<xsl:template match="VLConfig:FileFormatVersion" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime);
				return;
			case 7:
				Convert_21_to_22.<xsl:template match="VLConfig:MemoryRingBufferIsActive" priority="1">({urn:schemas-microsoft-com:xslt-debug}runtime, xPathNavigator);
				return;
			default:
				if ((1 << (int)xPathNavigator.NodeType & 508) == 0)
				{
					ContentIterator contentIterator;
					contentIterator.Create(xPathNavigator);
					while (contentIterator.MoveNext())
					{
						Convert_21_to_22.<xsl:apply-templates> (2)({urn:schemas-microsoft-com:xslt-debug}runtime, contentIterator.Current);
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
				XPathNavigator xPathNavigator2 = Convert_21_to_22.SyncToNavigator(xPathNavigator2, {urn:schemas-microsoft-com:xslt-debug}runtime.ExternalContext.DefaultDataSource);
				xPathNavigator2.MoveToRoot();
				xPathNavigator = Convert_21_to_22.SyncToNavigator(arg_2A_0, xPathNavigator2);
				xPathNavigator.MoveToRoot();
				DescendantIterator descendantIterator;
				descendantIterator.Create(xPathNavigator, {urn:schemas-microsoft-com:xslt-debug}runtime.GetNameFilter(0), false);
				{urn:schemas-microsoft-com:xslt-debug}runtime.SetGlobalValue(arg_66_1, (!descendantIterator.MoveNext()) ? "" : descendantIterator.Current.Value);
			}
			return (string){urn:schemas-microsoft-com:xslt-debug}runtime.GetGlobalValue(1);
		}

		private unsafe static string OperatingMode(XmlQueryRuntime {urn:schemas-microsoft-com:xslt-debug}runtime)
		{
			if (!{urn:schemas-microsoft-com:xslt-debug}runtime.IsGlobalComputed(2))
			{
				int arg_97_1 = 2;
				ContentMergeIterator contentMergeIterator;
				contentMergeIterator.Create({urn:schemas-microsoft-com:xslt-debug}runtime.GetNameFilter(1));
				XPathNavigator xPathNavigator;
				XPathNavigator arg_38_0 = xPathNavigator;
				XPathNavigator xPathNavigator2 = Convert_21_to_22.SyncToNavigator(xPathNavigator2, {urn:schemas-microsoft-com:xslt-debug}runtime.ExternalContext.DefaultDataSource);
				xPathNavigator2.MoveToRoot();
				xPathNavigator = Convert_21_to_22.SyncToNavigator(arg_38_0, xPathNavigator2);
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
