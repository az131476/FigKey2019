using System;

namespace Vector.VLConfig.BusinessLogic
{
	public enum EnumProfileError
	{
		None,
		FileDoesNotExist,
		InvalidFileContent,
		TagMissing,
		ArrayValueMissing,
		InvalidValue,
		MarkerRestoration,
		TriggerRestoration,
		ViewTypeMissmatch,
		LoggerTypeMissmatch
	}
}
