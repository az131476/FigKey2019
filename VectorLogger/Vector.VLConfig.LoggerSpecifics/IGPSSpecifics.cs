using System;

namespace Vector.VLConfig.LoggerSpecifics
{
	public interface IGPSSpecifics
	{
		bool HasSerialGPS
		{
			get;
		}

		bool HasCANgpsSupport
		{
			get;
		}

		uint DefaultLogSerialGPSStartCANId
		{
			get;
		}

		bool DefaultLogSerialGPSIsExtendedStartCANId
		{
			get;
		}

		uint DefaultLogGPSChannel
		{
			get;
		}
	}
}
