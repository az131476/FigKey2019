using System;

namespace Vector.VLConfig.CANwinAccess
{
	internal class Vocabulary
	{
		public static readonly string ProcessNameCANoe32 = "CANoe32";

		public static readonly string ProcessNameCANoe64 = "CANoe64";

		public static readonly string ProcessNameCANalyzer32 = "CANw32";

		public static readonly string ProcessNameCANalyzer64 = "CANw64";

		public static readonly string ProductNameCANoe = "CANoe";

		public static readonly string ProductNameCANalyzer = "CANalyzer";

		public static readonly string ProgIdCANoe = "CANoe.Application";

		public static readonly string ProgIdCANalyzer = "CANalyzer.Application";

		public static readonly string OfflineTemplateCANoe = "CANoe_Offline.tcn";

		public static readonly string OfflineTemplateCANalyzer = "CANalyzer_Offline.tcw";

		public static readonly string ConfigNameFormat = "{0}_QuickView.cfg";

		public static readonly string AssertObjectDisposed = "Object derived from 'ComBase' was not disposed!";

		public static readonly string DebugInfoComException = "COM error 0x{0}:" + Environment.NewLine + "{1}";

		public static readonly string DebugInfoGeneralException = "Error:" + Environment.NewLine + "{0}";

		public static readonly string DebugInfoFunctionFailed = "{0}() failed.";

		public static readonly string DebugInfoFunctionFailedWithIndex = "{0}() failed (step {1})";
	}
}
