using System;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	public class VirtualCANLogMessage : IVirtualCANLogMessage
	{
		public ValidatedProperty<uint> Id
		{
			get;
			set;
		}

		public ValidatedProperty<bool> IsIdExtended
		{
			get;
			set;
		}

		public ValidatedProperty<uint> ChannelNr
		{
			get;
			set;
		}

		public VirtualCANLogMessage(ValidatedProperty<uint> id, ValidatedProperty<bool> isIdExtended, ValidatedProperty<uint> channelNr)
		{
			this.Id = id;
			this.IsIdExtended = isIdExtended;
			this.ChannelNr = channelNr;
		}
	}
}
