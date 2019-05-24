using System;
using System.Runtime.Serialization;
using Vector.VLConfig.LoggerSpecifics;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "MemoryRingBuffer")]
	public class MemoryRingBuffer
	{
		[DataMember(Name = "MemoryRingBufferIsActive")]
		public ValidatedProperty<bool> IsActive
		{
			get;
			set;
		}

		[DataMember(Name = "MemoryRingBufferSize")]
		public ValidatedProperty<uint> Size
		{
			get;
			set;
		}

		[DataMember(Name = "MemoryRingBufferStoreRAM")]
		public ValidatedProperty<bool> StoreRAMOnShutdown
		{
			get;
			set;
		}

		[DataMember(Name = "MemoryRingBufferMaxNumOfFiles")]
		public ValidatedProperty<uint> MaxNumberOfFiles
		{
			get;
			set;
		}

		[DataMember(Name = "MemoryRingBufferOperatingMode")]
		public ValidatedProperty<RingBufferOperatingMode> OperatingMode
		{
			get;
			set;
		}

		public MemoryRingBuffer()
		{
			this.IsActive = new ValidatedProperty<bool>(true);
			this.Size = new ValidatedProperty<uint>(0u);
			this.StoreRAMOnShutdown = new ValidatedProperty<bool>(false);
			this.MaxNumberOfFiles = new ValidatedProperty<uint>(0u);
			this.OperatingMode = new ValidatedProperty<RingBufferOperatingMode>(RingBufferOperatingMode.overwriteOldest);
		}
	}
}
