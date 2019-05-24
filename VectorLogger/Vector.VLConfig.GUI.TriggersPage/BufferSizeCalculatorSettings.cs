using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Vector.VLConfig.BusinessLogic.Configuration;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.LoggerSpecifics;

namespace Vector.VLConfig.GUI.TriggersPage
{
	internal class BufferSizeCalculatorSettings
	{
		private readonly IConfigurationManagerService mConfigurationManagerService;

		private readonly List<ChannelItem> mChannelItems = new List<ChannelItem>();

		public bool Changed
		{
			get;
			private set;
		}

		public uint PostTriggerTimeMilliseconds
		{
			get;
			set;
		}

		public uint PreTriggerTimeSeconds
		{
			get;
			set;
		}

		private BufferSizeCalculatorInformation BufferSizeCalculatorInformation
		{
			get
			{
				return this.mConfigurationManagerService.MetaInformation.BufferSizeCalculatorInformation;
			}
		}

		public ReadOnlyCollection<ChannelItem> ActiveChannelItemsCanLin
		{
			get;
			private set;
		}

		public ReadOnlyCollection<ChannelItem> ActiveChannelItemsFlexray
		{
			get;
			private set;
		}

		private ILoggerSpecifics LoggerSpecifics
		{
			get
			{
				return this.mConfigurationManagerService.LoggerSpecifics;
			}
		}

		private MultibusChannelConfiguration MultibusChannelConfiguration
		{
			get
			{
				return this.mConfigurationManagerService.MultibusChannelConfiguration;
			}
		}

		private bool UsesMultibusConfiguration
		{
			get
			{
				return this.LoggerSpecifics.Multibus.NumberOfChannels > 0u;
			}
		}

		public uint MaxNumberOfCANChannels
		{
			get
			{
				if (!this.UsesMultibusConfiguration)
				{
					return this.LoggerSpecifics.CAN.NumberOfChannels;
				}
				return (uint)this.MultibusChannelConfiguration.CANChannels.Count;
			}
		}

		public uint NumberOfConfiguredCANChannels
		{
			get
			{
				if (!this.UsesMultibusConfiguration)
				{
					return (uint)this.mConfigurationManagerService.CANChannelConfiguration.CANChannels.Count;
				}
				return (uint)this.MultibusChannelConfiguration.CANChannels.Count;
			}
		}

		public uint MaxNumberOfLINChannels
		{
			get
			{
				if (!this.UsesMultibusConfiguration)
				{
					return this.mConfigurationManagerService.LoggerSpecifics.LIN.NumberOfChannels;
				}
				return (uint)this.MultibusChannelConfiguration.LINChannels.Count;
			}
		}

		public uint NumberOfConfiguredLINChannels
		{
			get
			{
				if (!this.UsesMultibusConfiguration)
				{
					return (uint)this.mConfigurationManagerService.LINChannelConfiguration.LINChannels.Count;
				}
				return (uint)this.MultibusChannelConfiguration.LINChannels.Count;
			}
		}

		public uint MaxNumberOfFlexrayChannels
		{
			get
			{
				if (!this.UsesMultibusConfiguration)
				{
					return this.mConfigurationManagerService.LoggerSpecifics.Flexray.NumberOfChannels;
				}
				return 0u;
			}
		}

		public uint NumberOfConfiguredFlexrayChannels
		{
			get
			{
				if (!this.UsesMultibusConfiguration)
				{
					return (uint)this.mConfigurationManagerService.FlexrayChannelConfiguration.FlexrayChannels.Count;
				}
				return 0u;
			}
		}

		public CANChannel GetCANChannel(uint channelNumber)
		{
			if (!this.UsesMultibusConfiguration)
			{
				return this.mConfigurationManagerService.CANChannelConfiguration.GetCANChannel(channelNumber);
			}
			return this.MultibusChannelConfiguration.CANChannels[channelNumber];
		}

		public LINChannel GetLINChannel(uint channelNumber)
		{
			if (!this.UsesMultibusConfiguration)
			{
				return this.mConfigurationManagerService.LINChannelConfiguration.GetLINChannel(channelNumber);
			}
			return this.MultibusChannelConfiguration.LINChannels[channelNumber];
		}

		public FlexrayChannel GetFlexrayChannel(uint channelNumber)
		{
			if (!this.UsesMultibusConfiguration)
			{
				return this.mConfigurationManagerService.FlexrayChannelConfiguration.GetFlexrayChannel(channelNumber);
			}
			return null;
		}

		public BufferSizeCalculatorSettings(IConfigurationManagerService configurationManagerService)
		{
			this.mConfigurationManagerService = configurationManagerService;
			this.PreTriggerTimeSeconds = this.BufferSizeCalculatorInformation.PreTriggerTimeSeconds.Value;
			this.InitChannelItems();
		}

		private void InitChannelItems()
		{
			Dictionary<string, ChannelItem> dictionary = this.BufferSizeCalculatorInformation.ChannelItems.ToDictionary((BufferSizeCalculatorChannelItem t) => t.Name.Value, (BufferSizeCalculatorChannelItem t) => new ChannelItem(t));
			if (this.UsesMultibusConfiguration)
			{
				for (int i = 0; i < this.MultibusChannelConfiguration.Channels.Count; i++)
				{
					this.InitChannelItem(this.MultibusChannelConfiguration.Channels[i], (uint)(i + 1), dictionary);
				}
			}
			else
			{
				uint num = Math.Min(this.NumberOfConfiguredCANChannels, this.MaxNumberOfCANChannels);
				for (uint num2 = 1u; num2 <= num; num2 += 1u)
				{
					this.InitChannelItem(this.GetCANChannel(num2), num2, dictionary);
				}
				num = Math.Min(this.NumberOfConfiguredLINChannels, this.MaxNumberOfLINChannels);
				for (uint num3 = 1u; num3 <= num; num3 += 1u)
				{
					this.InitChannelItem(this.GetLINChannel(num3), num3, dictionary);
				}
				num = Math.Min(this.NumberOfConfiguredFlexrayChannels, this.MaxNumberOfFlexrayChannels);
				for (uint num4 = 1u; num4 <= num; num4 += 1u)
				{
					this.InitChannelItem(this.GetFlexrayChannel(num4), num4, dictionary);
				}
			}
			this.mChannelItems.AddRange(dictionary.Values);
			this.ActiveChannelItemsCanLin = new ReadOnlyCollection<ChannelItem>((from t in this.mChannelItems
			where t.IsActive && (t.BusType == BusType.Bt_CAN || t.BusType == BusType.Bt_LIN)
			select t).ToList<ChannelItem>());
			this.ActiveChannelItemsFlexray = new ReadOnlyCollection<ChannelItem>((from t in this.mChannelItems
			where t.IsActive && t.BusType == BusType.Bt_FlexRay
			select t).ToList<ChannelItem>());
		}

		private void InitChannelItem(HardwareChannel hardwareChannel, uint index, Dictionary<string, ChannelItem> channelItems)
		{
			if (hardwareChannel == null)
			{
				return;
			}
			string text = BufferSizeCalculatorChannelItem.CreateName(hardwareChannel, index);
			if (channelItems.ContainsKey(text))
			{
				channelItems[text].Update(hardwareChannel, false);
				return;
			}
			BufferSizeCalculatorChannelItem bufferSizeCalculatorChannelItem = new BufferSizeCalculatorChannelItem
			{
				Name = 
				{
					Value = text
				}
			};
			channelItems.Add(bufferSizeCalculatorChannelItem.Name.Value, new ChannelItem(bufferSizeCalculatorChannelItem));
			channelItems[text].Update(hardwareChannel, true);
		}

		public void UpdateModel()
		{
			this.Changed = false;
			this.Changed |= (this.BufferSizeCalculatorInformation.PreTriggerTimeSeconds.Value != this.PreTriggerTimeSeconds);
			this.BufferSizeCalculatorInformation.PreTriggerTimeSeconds.Value = this.PreTriggerTimeSeconds;
			if (this.mChannelItems.Count != this.BufferSizeCalculatorInformation.ChannelItems.Count)
			{
				this.Changed = true;
				this.BufferSizeCalculatorInformation.ChannelItems.Clear();
				using (List<ChannelItem>.Enumerator enumerator = this.mChannelItems.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ChannelItem current = enumerator.Current;
						BufferSizeCalculatorChannelItem item = new BufferSizeCalculatorChannelItem
						{
							Name = 
							{
								Value = current.Name
							},
							BusLoad = 
							{
								Value = current.BusLoad
							},
							MsgsPerSec = 
							{
								Value = current.MsgsPerSec
							},
							Payload = 
							{
								Value = current.Payload
							}
						};
						this.BufferSizeCalculatorInformation.ChannelItems.Add(item);
					}
					goto IL_24E;
				}
			}
			using (List<BufferSizeCalculatorChannelItem>.Enumerator enumerator2 = this.BufferSizeCalculatorInformation.ChannelItems.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					BufferSizeCalculatorChannelItem tmpPersistentItem = enumerator2.Current;
					ChannelItem channelItem = this.mChannelItems.FirstOrDefault((ChannelItem t) => t.Name.Equals(tmpPersistentItem.Name.Value));
					if (channelItem != null)
					{
						this.Changed |= (tmpPersistentItem.BusLoad.Value != channelItem.BusLoad);
						tmpPersistentItem.BusLoad.Value = channelItem.BusLoad;
						this.Changed |= (tmpPersistentItem.MsgsPerSec.Value != channelItem.MsgsPerSec);
						tmpPersistentItem.MsgsPerSec.Value = channelItem.MsgsPerSec;
						this.Changed |= (tmpPersistentItem.Payload.Value != channelItem.Payload);
						tmpPersistentItem.Payload.Value = channelItem.Payload;
					}
				}
			}
			IL_24E:
			this.mConfigurationManagerService.DataModelHasChanged(this.mConfigurationManagerService.MetaInformation);
		}
	}
}
