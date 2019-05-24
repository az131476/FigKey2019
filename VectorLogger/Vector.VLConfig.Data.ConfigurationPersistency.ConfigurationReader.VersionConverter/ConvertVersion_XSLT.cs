using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Xsl;

namespace Vector.VLConfig.Data.ConfigurationPersistency.ConfigurationReader.VersionConverter
{
	public abstract class ConvertVersion_XSLT : IConvertVersion
	{
		private Type xsltTransformationClass;

		public ConvertVersion_XSLT(Type xsltTransformationClass)
		{
			this.xsltTransformationClass = xsltTransformationClass;
		}

		protected void Transform(Stream inputXML, ref Stream transformedXML)
		{
			XslCompiledTransform xslCompiledTransform = new XslCompiledTransform();
			xslCompiledTransform.Load(this.xsltTransformationClass);
			inputXML.Seek(0L, SeekOrigin.Begin);
			StreamReader input = new StreamReader(inputXML, Encoding.UTF8);
			XmlReader input2 = new XmlTextReader(input);
			XsltArgumentList arguments = new XsltArgumentList();
			transformedXML = new MemoryStream();
			StreamWriter results = new StreamWriter(transformedXML, Encoding.UTF8);
			xslCompiledTransform.Transform(input2, arguments, results);
		}

		public abstract bool Convert(Stream inputXML, ref Stream convertedXML, ref string errors);
	}
}
