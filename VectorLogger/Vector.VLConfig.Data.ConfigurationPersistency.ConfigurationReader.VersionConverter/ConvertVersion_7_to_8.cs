using System;
using System.IO;
using Vector.VLConfig.Data.ConfigurationPersistency.ConfigurationReader.VersionConverter.XSLT;

namespace Vector.VLConfig.Data.ConfigurationPersistency.ConfigurationReader.VersionConverter
{
	public class ConvertVersion_7_to_8 : ConvertVersion_XSLT
	{
		protected Type xsltTransformationClass;

		public ConvertVersion_7_to_8() : base(typeof(Convert_7_to_8))
		{
		}

		public override bool Convert(Stream inputXML, ref Stream convertedXML, ref string errors)
		{
			bool result;
			try
			{
				base.Transform(inputXML, ref convertedXML);
				result = true;
			}
			catch (Exception ex)
			{
				errors = ex.Message;
				result = false;
			}
			return result;
		}
	}
}
