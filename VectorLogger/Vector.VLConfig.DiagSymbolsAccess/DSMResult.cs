using System;

namespace Vector.VLConfig.DiagSymbolsAccess
{
	public enum DSMResult
	{
		OK,
		DuplicateEcuQualifier,
		NoEcuInDatabase,
		UnknownFileType,
		FileNotFound,
		DuplicateDatabase,
		FailedToLoadDescFile,
		FailedToLoadCddDescFile,
		ServiceTooLong = 10,
		ServiceTooComplexIterator,
		ServiceTooComplexMultiplexor,
		ServiceNotSupported
	}
}
