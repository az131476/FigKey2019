using System;

namespace Vector.VLConfig.LoggerSpecifics
{
	public class LoggerSpecificsFactory
	{
		private static GL1000 gl1000 = new GL1000();

		private static GL1020FTE gl1020fte = new GL1020FTE();

		private static GL2000 gl2000 = new GL2000();

		private static GL3000 gl3000 = new GL3000();

		private static GL4000 gl4000 = new GL4000();

		private static VN16XXlog vn16XXlog = new VN16XXlog();

		public static ILoggerSpecifics CreateLoggerSpecifics(LoggerType loggerType)
		{
			switch (loggerType)
			{
			case LoggerType.GL1000:
				return LoggerSpecificsFactory.gl1000;
			case LoggerType.GL1020FTE:
				return LoggerSpecificsFactory.gl1020fte;
			case LoggerType.GL2000:
				return LoggerSpecificsFactory.gl2000;
			case LoggerType.GL3000:
				return LoggerSpecificsFactory.gl3000;
			case LoggerType.GL4000:
				return LoggerSpecificsFactory.gl4000;
			case LoggerType.VN1630log:
				return LoggerSpecificsFactory.vn16XXlog;
			default:
				return null;
			}
		}

		public static ILoggerSpecifics CreateLoggerSpecifics(uint devType)
		{
			if (((IDeviceAccessSpecifics)LoggerSpecificsFactory.gl1000).DeviceType == devType)
			{
				return LoggerSpecificsFactory.gl1000;
			}
			if (((IDeviceAccessSpecifics)LoggerSpecificsFactory.gl1020fte).DeviceType == devType)
			{
				return LoggerSpecificsFactory.gl1020fte;
			}
			if (((IDeviceAccessSpecifics)LoggerSpecificsFactory.gl2000).DeviceType == devType)
			{
				return LoggerSpecificsFactory.gl2000;
			}
			if (((IDeviceAccessSpecifics)LoggerSpecificsFactory.gl3000).DeviceType == devType)
			{
				return LoggerSpecificsFactory.gl3000;
			}
			if (((IDeviceAccessSpecifics)LoggerSpecificsFactory.gl4000).DeviceType == devType)
			{
				return LoggerSpecificsFactory.gl4000;
			}
			if (((IDeviceAccessSpecifics)LoggerSpecificsFactory.vn16XXlog).DeviceType == devType)
			{
				return LoggerSpecificsFactory.vn16XXlog;
			}
			return null;
		}
	}
}
