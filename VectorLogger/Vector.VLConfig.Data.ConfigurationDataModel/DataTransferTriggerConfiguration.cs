using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "DataTransferTriggerConfiguration")]
	public class DataTransferTriggerConfiguration
	{
		[DataMember(Name = "WLANConfigurationDataTranferTriggerList")]
		private List<DataTransferTrigger> dataTranferTriggerList;

		public ReadOnlyCollection<DataTransferTrigger> DataTransferTriggers
		{
			get
			{
				return new ReadOnlyCollection<DataTransferTrigger>(this.dataTranferTriggerList);
			}
		}

		public ReadOnlyCollection<DataTransferTrigger> ActiveDataTransferTriggers
		{
			get
			{
				return new ReadOnlyCollection<DataTransferTrigger>((from DataTransferTrigger trigger in this.dataTranferTriggerList
				where trigger.IsActive.Value
				select trigger).ToList<DataTransferTrigger>());
			}
		}

		public DataTransferTriggerConfiguration()
		{
			this.dataTranferTriggerList = new List<DataTransferTrigger>();
		}

		public void AddDataTransferTrigger(DataTransferTrigger trigger)
		{
			this.dataTranferTriggerList.Add(trigger);
		}

		public bool RemoveDataTransferTrigger(DataTransferTrigger trigger)
		{
			if (this.dataTranferTriggerList.Contains(trigger))
			{
				this.dataTranferTriggerList.Remove(trigger);
				return true;
			}
			return false;
		}

		public IEnumerable<Event> GetEvents(bool bActiveEventsOnly)
		{
			return from dtt in this.dataTranferTriggerList
			where !bActiveEventsOnly || dtt.IsActive.Value
			select dtt.Event;
		}
	}
}
