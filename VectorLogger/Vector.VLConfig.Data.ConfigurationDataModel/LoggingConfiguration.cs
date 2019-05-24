using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "LoggingConfiguration")]
	public class LoggingConfiguration
	{
		[DataMember(Name = "LoggingConfigurationDatabaseConfiguration")]
		private DatabaseConfiguration databaseConfiguration;

		[DataMember(Name = "LoggingConfigurationCcpXcpSignalConfiguration")]
		private CcpXcpSignalConfiguration ccpXcpSignalConfiguration;

		[DataMember(Name = "LoggingConfigurationTriggerConfigList")]
		private List<TriggerConfiguration> triggerConfigurationList;

		[DataMember(Name = "LoggingConfigurationFilterConfigList")]
		private List<FilterConfiguration> filterConfigurationList;

		[DataMember(Name = "LoggingConfigurationIncludeFileConfiguration")]
		private IncludeFileConfiguration includeFileConfiguration;

		[DataMember(Name = "LoggingConfigurationSpecialFeaturesConfiguration")]
		private SpecialFeaturesConfiguration specialFeaturesConfiguration;

		[DataMember(Name = "LoggingConfigurationWLANConfiguration")]
		private WLANConfiguration wlanConfiguration;

		[DataMember(Name = "LoggingConfigurationCommentConfiguration")]
		private CommentConfiguration commentConfiguration;

		[DataMember(Name = "LoggingConfigurationDiagnosticsDatabaseConfiguration")]
		private DiagnosticsDatabaseConfiguration diagnosticsDatabaseConfiguration;

		[DataMember(Name = "LoggingConfigurationDiagnosticActionsConfiguration")]
		private DiagnosticActionsConfiguration diagnosticActionsConfiguration;

		public DatabaseConfiguration DatabaseConfiguration
		{
			get
			{
				if (this.databaseConfiguration == null)
				{
					this.databaseConfiguration = new DatabaseConfiguration();
				}
				return this.databaseConfiguration;
			}
		}

		public CcpXcpSignalConfiguration CcpXcpSignalConfiguration
		{
			get
			{
				if (this.ccpXcpSignalConfiguration == null)
				{
					this.ccpXcpSignalConfiguration = new CcpXcpSignalConfiguration();
				}
				return this.ccpXcpSignalConfiguration;
			}
		}

		public ReadOnlyCollection<TriggerConfiguration> TriggerConfigurations
		{
			get
			{
				return new ReadOnlyCollection<TriggerConfiguration>(this.triggerConfigurationList);
			}
		}

		public ReadOnlyCollection<TriggerConfiguration> TriggerConfigurationsOfActiveMemories
		{
			get
			{
				List<TriggerConfiguration> list = new List<TriggerConfiguration>();
				foreach (TriggerConfiguration current in this.triggerConfigurationList)
				{
					if (current.MemoryRingBuffer.IsActive.Value)
					{
						list.Add(current);
					}
				}
				return new ReadOnlyCollection<TriggerConfiguration>(list);
			}
		}

		public ReadOnlyCollection<FilterConfiguration> FilterConfigurations
		{
			get
			{
				return new ReadOnlyCollection<FilterConfiguration>(this.filterConfigurationList);
			}
		}

		public ReadOnlyCollection<FilterConfiguration> FilterConfigurationsOfActiveMemories
		{
			get
			{
				List<FilterConfiguration> list = new List<FilterConfiguration>();
				int num = 0;
				while ((long)num < (long)((ulong)this.NumberOfMemoryUnits))
				{
					if (this.triggerConfigurationList[num].MemoryRingBuffer.IsActive.Value)
					{
						list.Add(this.filterConfigurationList[num]);
					}
					num++;
				}
				return new ReadOnlyCollection<FilterConfiguration>(list);
			}
		}

		public IncludeFileConfiguration IncludeFileConfiguration
		{
			get
			{
				if (this.includeFileConfiguration == null)
				{
					this.includeFileConfiguration = new IncludeFileConfiguration();
				}
				return this.includeFileConfiguration;
			}
		}

		public SpecialFeaturesConfiguration SpecialFeaturesConfiguration
		{
			get
			{
				if (this.specialFeaturesConfiguration == null)
				{
					this.specialFeaturesConfiguration = new SpecialFeaturesConfiguration();
				}
				return this.specialFeaturesConfiguration;
			}
		}

		public WLANConfiguration WLANConfiguration
		{
			get
			{
				if (this.wlanConfiguration == null)
				{
					this.wlanConfiguration = new WLANConfiguration();
				}
				return this.wlanConfiguration;
			}
		}

		public CommentConfiguration CommentConfiguration
		{
			get
			{
				if (this.commentConfiguration == null)
				{
					this.commentConfiguration = new CommentConfiguration();
				}
				return this.commentConfiguration;
			}
		}

		public DiagnosticsDatabaseConfiguration DiagnosticsDatabaseConfiguration
		{
			get
			{
				if (this.diagnosticsDatabaseConfiguration == null)
				{
					this.diagnosticsDatabaseConfiguration = new DiagnosticsDatabaseConfiguration();
				}
				return this.diagnosticsDatabaseConfiguration;
			}
		}

		public DiagnosticActionsConfiguration DiagnosticActionsConfiguration
		{
			get
			{
				if (this.diagnosticActionsConfiguration == null)
				{
					this.diagnosticActionsConfiguration = new DiagnosticActionsConfiguration();
				}
				return this.diagnosticActionsConfiguration;
			}
		}

		public uint NumberOfMemoryUnits
		{
			get
			{
				return (uint)this.triggerConfigurationList.Count;
			}
			set
			{
				int count = this.triggerConfigurationList.Count;
				if ((ulong)value > (ulong)((long)count))
				{
					int num = 0;
					while ((long)num < (long)((ulong)value - (ulong)((long)count)))
					{
						this.triggerConfigurationList.Add(new TriggerConfiguration(count + 1 + num));
						this.filterConfigurationList.Add(new FilterConfiguration(count + 1 + num));
						num++;
					}
				}
				if ((ulong)value < (ulong)((long)count))
				{
					int num2 = count;
					while ((long)num2 > (long)((ulong)value))
					{
						this.triggerConfigurationList.RemoveAt(num2 - 1);
						this.filterConfigurationList.RemoveAt(num2 - 1);
						num2--;
					}
				}
			}
		}

		public LoggingConfiguration()
		{
			this.triggerConfigurationList = new List<TriggerConfiguration>();
			this.filterConfigurationList = new List<FilterConfiguration>();
		}
	}
}
