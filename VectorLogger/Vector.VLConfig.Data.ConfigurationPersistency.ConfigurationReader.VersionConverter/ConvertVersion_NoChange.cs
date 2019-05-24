using System;
using System.IO;

namespace Vector.VLConfig.Data.ConfigurationPersistency.ConfigurationReader.VersionConverter
{
	public abstract class ConvertVersion_NoChange : IConvertVersion
	{
		public bool Convert(Stream inputXML, ref Stream convertedXML, ref string errors)
		{
			errors = "";
			convertedXML = inputXML;
			return true;
		}
	}
}
