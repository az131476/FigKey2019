using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "DataTransferTrigger")]
	public class DataTransferTrigger : Action
	{
		[DataMember(Name = "DataTransferTriggerIsDownloadRingbufferEnabled")]
		public ValidatedProperty<bool> IsDownloadRingbufferEnabled
		{
			get;
			set;
		}

		[DataMember(Name = "DataTransferTriggerMemoriesToDownload")]
		public ValidatedProperty<uint> MemoriesToDownload
		{
			get;
			set;
		}

		[DataMember(Name = "DataTransferTriggerIsDownloadClassifEnabled")]
		public ValidatedProperty<bool> IsDownloadClassifEnabled
		{
			get;
			set;
		}

		[DataMember(Name = "DataTransferTriggerIsDownloadDriveRecEnabled")]
		public ValidatedProperty<bool> IsDownloadDriveRecEnabled
		{
			get;
			set;
		}

		[DataMember(Name = "DataTransferTriggerDataTransferMode")]
		public ValidatedProperty<DataTransferModeType> DataTransferMode
		{
			get;
			set;
		}

		public static DataTransferTrigger CreateNextLogSessionStartTrigger()
		{
			return new DataTransferTrigger
			{
				Event = new NextLogSessionStartEvent(),
				DataTransferMode = 
				{
					Value = DataTransferModeType.RecordWhileDataTransfer
				}
			};
		}

		public static DataTransferTrigger CreateOnShutdownTrigger()
		{
			return new DataTransferTrigger
			{
				Event = new OnShutdownEvent(),
				DataTransferMode = 
				{
					Value = DataTransferModeType.StopWhileDataTransfer
				}
			};
		}

		public static DataTransferTrigger CreateKeyTrigger()
		{
			return new DataTransferTrigger
			{
				Event = new KeyEvent()
			};
		}

		public static DataTransferTrigger CreateClockTimedTrigger()
		{
			return new DataTransferTrigger
			{
				Event = new ClockTimedEvent()
			};
		}

		public static DataTransferTrigger CreateReferencedRecordTriggerNameTrigger()
		{
			return new DataTransferTrigger
			{
				Event = new ReferencedRecordTriggerNameEvent()
			};
		}

		public DataTransferTrigger(DataTransferTrigger other) : this()
		{
			base.Assign(other);
			this.Assign(other);
		}

		private DataTransferTrigger()
		{
			this.IsDownloadRingbufferEnabled = new ValidatedProperty<bool>(true);
			this.MemoriesToDownload = new ValidatedProperty<uint>(2147483647u);
			this.IsDownloadClassifEnabled = new ValidatedProperty<bool>(true);
			this.IsDownloadDriveRecEnabled = new ValidatedProperty<bool>(true);
			this.DataTransferMode = new ValidatedProperty<DataTransferModeType>(DataTransferModeType.RecordWhileDataTransfer);
			this.StopType = new StopImmediate();
		}

		public override object Clone()
		{
			return new DataTransferTrigger(this);
		}

		public override bool Equals(Action action)
		{
			if (action == null || base.GetType() != action.GetType())
			{
				return false;
			}
			DataTransferTrigger dataTransferTrigger = (DataTransferTrigger)action;
			return this.IsDownloadRingbufferEnabled.Value == dataTransferTrigger.IsDownloadRingbufferEnabled.Value && this.MemoriesToDownload.Value == dataTransferTrigger.MemoriesToDownload.Value && this.IsDownloadClassifEnabled.Value == dataTransferTrigger.IsDownloadClassifEnabled.Value && this.IsDownloadDriveRecEnabled.Value == dataTransferTrigger.IsDownloadDriveRecEnabled.Value && this.DataTransferMode.Value == dataTransferTrigger.DataTransferMode.Value && base.Equals(action);
		}

		public override int GetHashCode()
		{
			return this.IsDownloadRingbufferEnabled.Value.GetHashCode() ^ this.MemoriesToDownload.Value.GetHashCode() ^ this.IsDownloadClassifEnabled.Value.GetHashCode() ^ this.IsDownloadDriveRecEnabled.Value.GetHashCode() ^ base.GetHashCode();
		}

		public void Assign(DataTransferTrigger other)
		{
			this.IsDownloadRingbufferEnabled.Value = other.IsDownloadRingbufferEnabled.Value;
			this.MemoriesToDownload.Value = other.MemoriesToDownload.Value;
			this.IsDownloadClassifEnabled.Value = other.IsDownloadClassifEnabled.Value;
			this.IsDownloadDriveRecEnabled.Value = other.IsDownloadDriveRecEnabled.Value;
			base.Assign(other);
		}
	}
}
