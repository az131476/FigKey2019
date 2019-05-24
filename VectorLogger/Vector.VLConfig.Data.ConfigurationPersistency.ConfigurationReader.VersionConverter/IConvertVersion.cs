using System;
using System.IO;

namespace Vector.VLConfig.Data.ConfigurationPersistency.ConfigurationReader.VersionConverter
{
	internal interface IConvertVersion
	{
		bool Convert(Stream inputXML, ref Stream convertedXML, ref string errors);
	}
}
