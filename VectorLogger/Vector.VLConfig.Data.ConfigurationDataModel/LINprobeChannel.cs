using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "LINprobeChannel")]
	public class LINprobeChannel : LINChannel
	{
		[DataMember(Name = "LINprobeChannelUseFixLINprobeBaudrate")]
		public ValidatedProperty<bool> UseFixLINprobeBaudrate
		{
			get;
			set;
		}

		public LINprobeChannel()
		{
			this.UseFixLINprobeBaudrate = new ValidatedProperty<bool>(true);
		}
	}
}
