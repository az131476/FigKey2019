using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "FilterConfiguration")]
	public class FilterConfiguration : Feature, IFeatureSymbolicDefinitions
	{
		[DataMember(Name = "FilterConfigurationFilterList")]
		private List<Filter> filterList;

		private IList<ISymbolicMessage> symbolicMsgs;

		private IList<ISymbolicSignal> symbolicSignals;

		private List<DiagnosticAction> diagActions;

		public int MemoryNr;

		public override IFeatureReferencedFiles ReferencedFiles
		{
			get
			{
				return null;
			}
		}

		public override IFeatureSymbolicDefinitions SymbolicDefinitions
		{
			get
			{
				return this;
			}
		}

		public override IFeatureCcpXcpSignalDefinitions CcpXcpSignalDefinitions
		{
			get
			{
				return null;
			}
		}

		public override IFeatureTransmitMessages TransmitMessages
		{
			get
			{
				return null;
			}
		}

		public override IFeatureVirtualLogMessages VirtualLogMessages
		{
			get
			{
				return null;
			}
		}

		public override IFeatureEvents Events
		{
			get
			{
				return null;
			}
		}

		IList<ISymbolicMessage> IFeatureSymbolicDefinitions.SymbolicMessages
		{
			get
			{
				if (this.symbolicMsgs == null)
				{
					this.symbolicMsgs = new List<ISymbolicMessage>();
				}
				this.symbolicMsgs.Clear();
				foreach (Filter current in this.filterList)
				{
					if (current is ISymbolicMessage)
					{
						this.symbolicMsgs.Add(current as ISymbolicMessage);
					}
				}
				return this.symbolicMsgs;
			}
		}

		IList<ISymbolicSignal> IFeatureSymbolicDefinitions.SymbolicSignals
		{
			get
			{
				if (this.symbolicSignals == null)
				{
					this.symbolicSignals = new List<ISymbolicSignal>();
				}
				return this.symbolicSignals;
			}
		}

		IList<DiagnosticAction> IFeatureSymbolicDefinitions.SymbolicDiagnosticActions
		{
			get
			{
				if (this.diagActions == null)
				{
					this.diagActions = new List<DiagnosticAction>();
				}
				return this.diagActions;
			}
		}

		public ReadOnlyCollection<Filter> Filters
		{
			get
			{
				return new ReadOnlyCollection<Filter>(this.filterList);
			}
		}

		public ReadOnlyCollection<CANIdFilter> CANIdFilters
		{
			get
			{
				return new ReadOnlyCollection<CANIdFilter>((from Filter filter in this.filterList
				where null != filter as CANIdFilter
				select filter as CANIdFilter).ToList<CANIdFilter>());
			}
		}

		public ReadOnlyCollection<LINIdFilter> LINIdFilters
		{
			get
			{
				return new ReadOnlyCollection<LINIdFilter>((from Filter filter in this.filterList
				where null != filter as LINIdFilter
				select filter as LINIdFilter).ToList<LINIdFilter>());
			}
		}

		public ReadOnlyCollection<DefaultFilter> DefaultFilters
		{
			get
			{
				return new ReadOnlyCollection<DefaultFilter>((from Filter filter in this.filterList
				where null != filter as DefaultFilter
				select filter as DefaultFilter).ToList<DefaultFilter>());
			}
		}

		public DefaultFilter DefaultFilter
		{
			get
			{
				if (this.DefaultFilters.Count >= 1)
				{
					return this.DefaultFilters[0];
				}
				return null;
			}
		}

		public ReadOnlyCollection<SymbolicMessageFilter> SymbolicMessageFilters
		{
			get
			{
				return new ReadOnlyCollection<SymbolicMessageFilter>((from Filter filter in this.filterList
				where null != filter as SymbolicMessageFilter
				select filter as SymbolicMessageFilter).ToList<SymbolicMessageFilter>());
			}
		}

		public override bool UpdateOnDependency(Feature otherFeature, IUpdateServiceForFeature updateService)
		{
			if (otherFeature is FilterConfiguration || otherFeature is TriggerConfiguration || otherFeature is CANChannelConfiguration || otherFeature is LINChannelConfiguration || otherFeature is FlexrayChannelConfiguration || otherFeature is MultibusChannelConfiguration || otherFeature is DatabaseConfiguration)
			{
				updateService.Notify<FilterConfiguration>(this);
				return true;
			}
			return false;
		}

		public override void Update(IUpdateServiceForFeature updateService)
		{
			updateService.Notify<FilterConfiguration>(this);
		}

		public FilterConfiguration(int memNr)
		{
			this.MemoryNr = memNr;
			this.filterList = new List<Filter>();
		}

		public IList<SymbolicMessageFilter> GetSymbolicFiltersForDatabaseOnChannel(string databasePath, uint channel, BusType busType)
		{
			List<SymbolicMessageFilter> list = new List<SymbolicMessageFilter>();
			foreach (SymbolicMessageFilter current in this.SymbolicMessageFilters)
			{
				if (string.Compare(current.DatabasePath.Value, databasePath, true) == 0 && ((current.ChannelNumber.Value == channel && current.BusType.Value == busType) || (current.BusType.Value == BusType.Bt_FlexRay && channel == Database.ChannelNumber_FlexrayAB)))
				{
					list.Add(current);
				}
			}
			return list;
		}

		public bool InsertFilter(int idx, Filter filter)
		{
			if (idx >= 0 && idx < this.filterList.Count)
			{
				this.filterList.Insert(idx, filter);
				return true;
			}
			if (idx == this.filterList.Count)
			{
				this.filterList.Add(filter);
				return true;
			}
			return false;
		}

		public void AddFilter(Filter filter)
		{
			this.filterList.Add(filter);
		}

		public CANIdFilter AddCANIdFilter()
		{
			CANIdFilter cANIdFilter = new CANIdFilter();
			this.filterList.Add(cANIdFilter);
			return cANIdFilter;
		}

		public LINIdFilter AddLINIdFilter()
		{
			LINIdFilter lINIdFilter = new LINIdFilter();
			this.filterList.Add(lINIdFilter);
			return lINIdFilter;
		}

		public DefaultFilter AddDefaultFilter()
		{
			DefaultFilter defaultFilter = new DefaultFilter();
			this.filterList.Add(defaultFilter);
			return defaultFilter;
		}

		public bool RemoveFilter(Filter filter)
		{
			if (this.filterList.Contains(filter))
			{
				this.filterList.Remove(filter);
				return true;
			}
			return false;
		}

		public void ClearFilters()
		{
			this.filterList.Clear();
		}
	}
}
