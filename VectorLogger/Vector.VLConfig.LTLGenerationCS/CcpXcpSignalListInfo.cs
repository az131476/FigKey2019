using System;
using System.Collections.Generic;
using Vector.VLConfig.Data.ConfigurationDataModel;

namespace Vector.VLConfig.LTLGenerationCS
{
	public class CcpXcpSignalListInfo
	{
		public List<CcpXcpSignal> Signals
		{
			get;
			set;
		}

		public bool IsPolling
		{
			get;
			private set;
		}

		public string NameIfTriggeredPollingList
		{
			get;
			set;
		}

		public ActionCcpXcp ActionReference
		{
			get;
			private set;
		}

		public bool IsSecondarySlaveList
		{
			get;
			set;
		}

		public CcpXcpSignalListInfo(bool bIsPolling, ActionCcpXcp actionCcpXcp)
		{
			this.Signals = new List<CcpXcpSignal>();
			this.IsPolling = bIsPolling;
			this.ActionReference = actionCcpXcp;
			this.IsSecondarySlaveList = false;
		}

		public bool IsAlwaysActive()
		{
			return (this.ActionReference == null && !this.IsPolling) || (this.ActionReference != null && this.ActionReference.IsActive.Value && this.ActionReference.Event == null);
		}
	}
}
