using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "CcpXcpSignal")]
	public class CcpXcpSignal : ICcpXcpSignal
	{
		public bool DatabaseDisabled;

		private uint byteCount;

		[DataMember(Name = "IsActive")]
		public ValidatedProperty<bool> IsActive
		{
			get;
			set;
		}

		[DataMember(Name = "Name")]
		public ValidatedProperty<string> Name
		{
			get;
			set;
		}

		[DataMember(Name = "EcuName")]
		public ValidatedProperty<string> EcuName
		{
			get;
			set;
		}

		[DataMember(Name = "MeasurementMode")]
		public ValidatedProperty<CcpXcpMeasurementMode> MeasurementMode
		{
			get;
			set;
		}

		[DataMember(Name = "DaqEventId")]
		public ValidatedProperty<uint> DaqEventId
		{
			get;
			set;
		}

		[DataMember(Name = "PollingTime")]
		public ValidatedProperty<string> PollingTime
		{
			get;
			set;
		}

		public Dictionary<uint, string> DaqEvents
		{
			get;
			set;
		}

		public uint ByteCount
		{
			get
			{
				return this.byteCount;
			}
		}

		public ActionCcpXcp ActionCcpXcp
		{
			get;
			set;
		}

		public CcpXcpSignal(string name)
		{
			this.IsActive = new ValidatedProperty<bool>(true);
			if (!string.IsNullOrEmpty(name.Trim()))
			{
				this.Name = new ValidatedProperty<string>(name);
			}
			else
			{
				this.Name = new ValidatedProperty<string>("-");
			}
			this.EcuName = new ValidatedProperty<string>("");
			this.MeasurementMode = new ValidatedProperty<CcpXcpMeasurementMode>(CcpXcpMeasurementMode.DAQ);
			this.DaqEventId = new ValidatedProperty<uint>(0u);
			this.PollingTime = new ValidatedProperty<string>("100");
			this.DaqEvents = new Dictionary<uint, string>();
			this.DatabaseDisabled = false;
		}

		public void Refresh(Dictionary<uint, string> daqEvents)
		{
			this.DaqEvents = daqEvents;
		}

		public void ResetByteCount()
		{
			this.byteCount = 0u;
		}

		public void SetByteCount(uint bytes)
		{
			this.byteCount = bytes;
		}
	}
}
