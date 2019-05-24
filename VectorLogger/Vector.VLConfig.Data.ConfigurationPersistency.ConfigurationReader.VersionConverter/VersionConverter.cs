using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;

namespace Vector.VLConfig.Data.ConfigurationPersistency.ConfigurationReader.VersionConverter
{
	public class VersionConverter
	{
		private IList<IConvertVersion> versionConverters;

		private uint currentFormatVersion;

		private readonly string idPrefix = "z:Id=\"";

		private readonly string refPrefix = "z:Ref=\"";

		private Dictionary<uint, uint> oldIdToNewId;

		public VersionConverter(uint currentFormatVersion)
		{
			this.currentFormatVersion = currentFormatVersion;
			this.InitVersionConverters(currentFormatVersion);
			this.oldIdToNewId = new Dictionary<uint, uint>();
		}

		private void InitVersionConverters(uint currentFormatVersion)
		{
			this.versionConverters = new List<IConvertVersion>();
			this.versionConverters.Add(new ConvertVersion_22_to_23());
			this.versionConverters.Add(new ConvertVersion_21_to_22());
			this.versionConverters.Add(new ConvertVersion_20_to_21());
			this.versionConverters.Add(new ConvertVersion_19_to_20());
			this.versionConverters.Add(new ConvertVersion_18_to_19());
			this.versionConverters.Add(new ConvertVersion_17_to_18());
			this.versionConverters.Add(new ConvertVersion_16_to_17());
			this.versionConverters.Add(new ConvertVersion_15_to_16());
			this.versionConverters.Add(new ConvertVersion_14_to_15());
			this.versionConverters.Add(new ConvertVersion_13_to_14());
			this.versionConverters.Add(new ConvertVersion_12_to_13());
			this.versionConverters.Add(new ConvertVersion_11_to_12());
			this.versionConverters.Add(new ConvertVersion_10_to_11());
			this.versionConverters.Add(new ConvertVersion_9_to_10());
			this.versionConverters.Add(new ConvertVersion_8_to_9());
			this.versionConverters.Add(new ConvertVersion_7_to_8());
			this.versionConverters.Add(new ConvertVersion_6_to_7());
			this.versionConverters.Add(new ConvertVersion_5_to_6());
			this.versionConverters.Add(new ConvertVersion_4_to_5());
			this.versionConverters.Add(new ConvertVersion_3_to_4());
			this.versionConverters.Add(new ConvertVersion_2_to_3());
			this.versionConverters.Add(new ConvertVersion_1_to_2());
		}

		public bool Convert(Stream inputXML, ref Stream convertedXML, out bool isIncompatibleFileVersion)
		{
			isIncompatibleFileVersion = false;
			uint formatVersionFromStream = this.GetFormatVersionFromStream(inputXML);
			if (formatVersionFromStream == this.currentFormatVersion)
			{
				convertedXML = inputXML;
				return true;
			}
			if (formatVersionFromStream > this.currentFormatVersion)
			{
				isIncompatibleFileVersion = true;
				return false;
			}
			string text = "";
			for (int i = (int)(this.currentFormatVersion - formatVersionFromStream - 1u); i >= 0; i--)
			{
				IConvertVersion convertVersion = this.versionConverters[i];
				if (!convertVersion.Convert(inputXML, ref convertedXML, ref text))
				{
					return false;
				}
				inputXML = convertedXML;
			}
			Stream stream = null;
			if (!this.RenumberIds(convertedXML, out stream))
			{
				return false;
			}
			convertedXML = stream;
			return true;
		}

		private bool RenumberIds(Stream inputXML, out Stream renumberedXML)
		{
			this.oldIdToNewId.Clear();
			renumberedXML = null;
			inputXML.Seek(0L, SeekOrigin.Begin);
			StreamReader streamReader = new StreamReader(inputXML, Encoding.UTF8);
			string text = streamReader.ReadToEnd();
			Regex regex = new Regex("[<][^<>]+\\s+z:Id=\"\\d+\"[^<>]*[>]");
			Match match = regex.Match(text);
			uint num = 1u;
			StringBuilder stringBuilder = new StringBuilder(text.Length);
			int num2 = 0;
			while (match.Success)
			{
				int num3 = text.IndexOf(this.idPrefix, match.Index, match.Length);
				if (num3 < 0)
				{
					return false;
				}
				stringBuilder.Append(text.Substring(num2, num3 - num2 + this.idPrefix.Length));
				stringBuilder.Append(num.ToString());
				int num4 = num3 + this.idPrefix.Length;
				num3 = text.IndexOf("\"", num4, match.Index + match.Length - num4);
				if (num3 < 0)
				{
					return false;
				}
				string text2 = text.Substring(num4, num3 - num4);
				if (string.IsNullOrEmpty(text2))
				{
					return false;
				}
				uint num5 = uint.Parse(text2);
				if (num5 > 0u)
				{
					this.oldIdToNewId.Add(num5, num);
				}
				num2 = num3;
				match = match.NextMatch();
				num += 1u;
			}
			stringBuilder.Append(text.Substring(num2, text.Length - num2));
			text = stringBuilder.ToString();
			stringBuilder.Length = 0;
			Regex regex2 = new Regex("[<][^<>]+\\s+z:Ref=\"\\d+\"[^<>]*[>]");
			match = regex2.Match(text);
			num2 = 0;
			while (match.Success)
			{
				int num6 = text.IndexOf(this.refPrefix, match.Index, match.Length);
				if (num6 < 0)
				{
					return false;
				}
				stringBuilder.Append(text.Substring(num2, num6 - num2 + this.refPrefix.Length));
				int num7 = num6 + this.refPrefix.Length;
				num6 = text.IndexOf("\"", num7, match.Index + match.Length - num7);
				if (num6 < 0)
				{
					return false;
				}
				string text3 = text.Substring(num7, num6 - num7);
				if (string.IsNullOrEmpty(text3))
				{
					return false;
				}
				uint num8 = uint.Parse(text3);
				if (!this.oldIdToNewId.Keys.Contains(num8))
				{
					return false;
				}
				stringBuilder.Append(this.oldIdToNewId[num8]);
				num2 = num6;
				match = match.NextMatch();
			}
			stringBuilder.Append(text.Substring(num2, text.Length - num2));
			byte[] bytes = Encoding.UTF8.GetBytes(stringBuilder.ToString());
			renumberedXML = new MemoryStream(bytes);
			renumberedXML.Seek(0L, SeekOrigin.Begin);
			return true;
		}

		public uint GetFormatVersionFromStream(Stream inputXML)
		{
			inputXML.Seek(0L, SeekOrigin.Begin);
			XmlReader reader = XmlReader.Create(inputXML);
			XPathDocument xPathDocument = new XPathDocument(reader);
			XPathNavigator xPathNavigator = xPathDocument.CreateNavigator();
			XmlNamespaceManager xmlNamespaceManager = new XmlNamespaceManager(xPathNavigator.NameTable);
			xmlNamespaceManager.AddNamespace("default", "http://schemas.datacontract.org/2004/07/Vector.VLConfig.Data.ConfigurationDataModel");
			xmlNamespaceManager.AddNamespace("i", "http://www.w3.org/2001/XMLSchema-instance");
			xmlNamespaceManager.AddNamespace("z", "http://schemas.microsoft.com/2003/10/Serialization/");
			XPathExpression xPathExpression = XPathExpression.Compile("//default:FileFormatVersion");
			xPathExpression.SetContext(xmlNamespaceManager);
			XPathNavigator xPathNavigator2 = xPathNavigator.SelectSingleNode("//default:FileFormatVersion", xmlNamespaceManager);
			if (xPathNavigator2 != null)
			{
				return (uint)xPathNavigator2.ValueAsInt;
			}
			return 1u;
		}

		private static void ReadWriteStream(Stream readStream, Stream writeStream)
		{
			int num = 256;
			byte[] buffer = new byte[num];
			readStream.Seek(0L, SeekOrigin.Begin);
			for (int i = readStream.Read(buffer, 0, num); i > 0; i = readStream.Read(buffer, 0, num))
			{
				writeStream.Write(buffer, 0, i);
			}
			readStream.Seek(0L, SeekOrigin.Begin);
			writeStream.Close();
		}

		private static void DumpStream(Stream streamToDump, string filename)
		{
			FileStream writeStream = new FileStream(filename, FileMode.Create, FileAccess.Write);
			VersionConverter.ReadWriteStream(streamToDump, writeStream);
		}
	}
}
