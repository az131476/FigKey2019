using System;

namespace Vector.VLConfig.LoggerSpecifics
{
	public interface IDataTransferSpecifics
	{
		bool HasWLAN
		{
			get;
		}

		bool Has3G
		{
			get;
		}

		bool HasInterfaceMode
		{
			get;
		}

		bool HasWebServer
		{
			get;
		}

		bool IsMLserverSetupInLTL
		{
			get;
		}

		bool HasDifferentConnectionRequestTypes
		{
			get;
		}
	}
}
