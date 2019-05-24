using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.LoggerSpecifics;

namespace Vector.VLConfig.GUI.TriggersPage
{
	internal class BufferSizeCalculator
	{
		public class Constants
		{
			public const double MegaBytes = 1048576.0;

			public const double KiloBytes = 1024.0;

			public const double Milliseconds = 1000.0;

			public const int MaxPreTriggerSeconds = 86400;

			public const double CanMsgsPerSecAtDefaultSpeedRate = 4000.0;

			public const double CanDefaultSpeedRate = 500000.0;

			public const double LinMsgsPerSecAtDefaultSpeedRate = 200.0;

			public const double LinDefaultSpeedRate = 19200.0;

			public const uint CanLinBytesPerMessage = 20u;

			public const int CanLinDefaultBusload = 50;

			public const uint FlexrayDefaultSpeedRate = 10000000u;

			public const int FlexrayDefaultMsgsPerSec = 1000;

			public const int FlexrayDefaultPayload = 42;

			public const int FlexrayMaxPayload = 255;

			public const uint FlexrayBytesPerMessageHeader = 12u;

			public static readonly uint CanFdBytesPerMessage = 85u;

			public static readonly double CanFdMsgsPerSecAtDefaultSpeedRate = 3012.0;

			public static readonly double CanFdDefaultSpeedRate = 2000000.0;
		}

		private readonly IConfigurationManagerService mConfigurationManagerService;

		private readonly BufferSizeCalculatorSettings mSettings;

		private uint mMsgsPerSecDateTime;

		private uint mMsgsPerSecAnalogDigitalDateTime;

		private uint mTotalTimeMilliseconds;

		private ILoggerSpecifics LoggerSpecifics
		{
			get
			{
				return this.mConfigurationManagerService.LoggerSpecifics;
			}
		}

		public uint MsgsPerSecAnalog
		{
			get;
			private set;
		}

		public uint MsgsPerSecDigital
		{
			get;
			private set;
		}

		public uint EstimatedRingBufferSizeMB
		{
			get;
			private set;
		}

		public bool MetaInformationChanged
		{
			get
			{
				return this.mSettings.Changed;
			}
		}

		public uint PreTriggerTimeSeconds
		{
			get
			{
				return this.mSettings.PreTriggerTimeSeconds;
			}
			set
			{
				this.mSettings.PreTriggerTimeSeconds = value;
				this.mTotalTimeMilliseconds = (this.LoggerSpecifics.DataStorage.RingBufferSizeAppliesToPreTriggerTimeOnly ? ((uint)Math.Ceiling(value * 1000.0)) : ((uint)Math.Ceiling(value * 1000.0 + this.PostTriggerTimeMilliseconds)));
			}
		}

		public uint PostTriggerTimeMilliseconds
		{
			get
			{
				return this.mSettings.PostTriggerTimeMilliseconds;
			}
			set
			{
				this.mSettings.PostTriggerTimeMilliseconds = value;
			}
		}

		public uint NumberOfConfiguredCANChannels
		{
			get
			{
				return this.mSettings.NumberOfConfiguredCANChannels;
			}
		}

		public uint NumberOfConfiguredLINChannels
		{
			get
			{
				return this.mSettings.NumberOfConfiguredLINChannels;
			}
		}

		public uint NumberOfConfiguredFlexrayChannels
		{
			get
			{
				return this.mSettings.NumberOfConfiguredFlexrayChannels;
			}
		}

		public ReadOnlyCollection<ChannelItem> ActiveChannelItemsCanLin
		{
			get
			{
				return this.mSettings.ActiveChannelItemsCanLin;
			}
		}

		public ReadOnlyCollection<ChannelItem> ActiveChannelItemsFlexray
		{
			get
			{
				return this.mSettings.ActiveChannelItemsFlexray;
			}
		}

		public BufferSizeCalculator(IConfigurationManagerService configurationManagerService, uint postTriggerTimeMilliseconds)
		{
			this.mConfigurationManagerService = configurationManagerService;
			this.mSettings = new BufferSizeCalculatorSettings(configurationManagerService)
			{
				PostTriggerTimeMilliseconds = postTriggerTimeMilliseconds
			};
			this.PreTriggerTimeSeconds = this.PreTriggerTimeSeconds;
			this.InitLocalData();
		}

		private void InitLocalData()
		{
			this.MsgsPerSecAnalog = 0u;
			List<uint> list = new List<uint>();
			IList<AnalogInput> list2 = (from t in this.mConfigurationManagerService.AnalogInputConfiguration.AnalogInputs
			where t.IsActive.Value
			select t).ToList<AnalogInput>();
			foreach (AnalogInput current in list2)
			{
				if (!list.Contains(current.MappedCANId.Value))
				{
					list.Add(current.MappedCANId.Value);
					this.MsgsPerSecAnalog += current.Frequency.Value;
				}
			}
			this.MsgsPerSecDigital = 0u;
			list.Clear();
			IList<DigitalInput> list3 = (from t in this.mConfigurationManagerService.DigitalInputConfiguration.DigitalInputs
			where t.IsActiveOnChange.Value
			select t).ToList<DigitalInput>();
			foreach (DigitalInput current2 in list3)
			{
				if (!list.Contains(current2.MappedCANId.Value))
				{
					list.Add(current2.MappedCANId.Value);
					this.MsgsPerSecDigital += 1000u;
				}
			}
			IList<DigitalInput> list4 = (from t in this.mConfigurationManagerService.DigitalInputConfiguration.DigitalInputs
			where !t.IsActiveOnChange.Value && t.IsActiveFrequency.Value
			select t).ToList<DigitalInput>();
			foreach (DigitalInput current3 in list4)
			{
				if (!list.Contains(current3.MappedCANId.Value))
				{
					list.Add(current3.MappedCANId.Value);
					this.MsgsPerSecDigital += current3.Frequency.Value;
				}
			}
			this.mMsgsPerSecDateTime = (this.mConfigurationManagerService.SpecialFeaturesConfiguration.IsLogDateTimeEnabled.Value ? 1u : 0u);
			this.mMsgsPerSecAnalogDigitalDateTime = this.MsgsPerSecAnalog + this.MsgsPerSecDigital + this.mMsgsPerSecDateTime;
		}

		public void UpdateModel()
		{
			this.mSettings.UpdateModel();
		}

		public bool Calculate()
		{
			uint num = this.mMsgsPerSecAnalogDigitalDateTime * 20u;
			foreach (ChannelItem current in this.ActiveChannelItemsCanLin)
			{
				num += current.BytesPerSec;
			}
			foreach (ChannelItem current2 in this.ActiveChannelItemsFlexray)
			{
				num += current2.BytesPerSec;
			}
			double num2 = num / 1048576.0;
			uint val = (uint)Math.Ceiling(num2 * (this.mTotalTimeMilliseconds / 1000.0));
			uint num3 = this.LoggerSpecifics.DataStorage.MinRingBufferSize / 1024u;
			this.EstimatedRingBufferSizeMB = Math.Max(val, (num3 < 1u) ? 1u : num3);
			return this.EstimatedRingBufferSizeMB <= this.LoggerSpecifics.DataStorage.MaxRingBufferSize / 1024u;
		}

		public static string DataRate(uint bytesPerSec)
		{
			if (bytesPerSec >= 1048576u)
			{
				return string.Format("{0} MB/s", (bytesPerSec / 1048576.0).ToString("N2"));
			}
			if (bytesPerSec >= 1024u)
			{
				return string.Format("{0} kB/s", (bytesPerSec / 1024.0).ToString("N2"));
			}
			return string.Format("{0} Bytes/s", bytesPerSec);
		}
	}
}
