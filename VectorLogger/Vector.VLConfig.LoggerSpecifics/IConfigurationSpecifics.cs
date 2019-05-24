using System;

namespace Vector.VLConfig.LoggerSpecifics
{
	public interface IConfigurationSpecifics
	{
		EnumCompilerType CompilerType
		{
			get;
		}

		string ConfigurationDirectoryName
		{
			get;
		}

		EnumCodeLanguage CodeLanguage
		{
			get;
		}

		bool IsARXMLDatabaseConfigurationSupported
		{
			get;
		}

		bool SupportsAnalysisPackage
		{
			get;
		}

		bool SupportsPackAndGoImport
		{
			get;
		}
	}
}
