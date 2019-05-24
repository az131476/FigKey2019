using System;
using System.Collections.Generic;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.LTLGenerationCS
{
	public class CcpXcpEcuInfo
	{
		public uint EffectiveCanRequestID;

		public bool EffectiveIsCanRequestIdExtended;

		public Database Database
		{
			get;
			private set;
		}

		public CcpXcpEcu Ecu
		{
			get;
			private set;
		}

		public bool PreventDeconcatinationForFirstXcpOnUdpSlave
		{
			get;
			set;
		}

		public List<CcpXcpSignalListInfo> SignalListInfos
		{
			get;
			private set;
		}

		public CcpXcpEcuInfo(Database aDatabase, CcpXcpEcu aEcu)
		{
			this.Database = aDatabase;
			this.Ecu = aEcu;
			this.PreventDeconcatinationForFirstXcpOnUdpSlave = false;
			this.SignalListInfos = new List<CcpXcpSignalListInfo>();
		}
	}
}
