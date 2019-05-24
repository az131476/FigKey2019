using System;

namespace Vector.VLConfig.CaplGeneration.DataPresenter
{
	internal class DpHardware
	{
		private uint mMaxFileSizeMB;

		public uint TimerRateUs
		{
			get;
			set;
		}

		public uint IoCycleTimeUs
		{
			get;
			set;
		}

		public long PreTriggerTimeNs
		{
			get;
			set;
		}

		public uint PreTriggerTimeMs
		{
			get;
			set;
		}

		public uint PostTriggerTimeMs
		{
			get;
			set;
		}

		public long OverflowRetireTimeNs
		{
			get;
			set;
		}

		public long OverflowRepetitionTimeNs
		{
			get;
			set;
		}

		public uint StartupTimeMs
		{
			get;
			set;
		}

		public uint MaxFileSizeMB
		{
			get
			{
				return this.mMaxFileSizeMB;
			}
			set
			{
				this.mMaxFileSizeMB = Math.Min(value, 4096u);
			}
		}

		public bool BeepOnError
		{
			get;
			set;
		}

		public bool BeepOnOverflow
		{
			get;
			set;
		}

		public bool HasDAIO
		{
			get;
			set;
		}

		public bool IsPermanentLogging
		{
			get;
			set;
		}

		public bool IsOnOffLogging
		{
			get;
			set;
		}

		public bool IsTriggeredLogging
		{
			get;
			set;
		}

		public DpHardware(uint maxFileSizeMB)
		{
			this.TimerRateUs = 1000u;
			this.IoCycleTimeUs = 1000000u;
			this.PreTriggerTimeNs = 0L;
			this.PreTriggerTimeMs = 0u;
			this.PostTriggerTimeMs = 0u;
			this.OverflowRetireTimeNs = 200000000L;
			this.OverflowRepetitionTimeNs = 100000000L;
			this.StartupTimeMs = 2000u;
			this.MaxFileSizeMB = maxFileSizeMB;
			this.BeepOnError = true;
			this.BeepOnOverflow = true;
			this.HasDAIO = false;
			this.IsPermanentLogging = false;
			this.IsOnOffLogging = false;
			this.IsTriggeredLogging = false;
		}
	}
}
