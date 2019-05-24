using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "CANFDChipConfiguration")]
	public class CANFDChipConfiguration : ICANChipConfiguration
	{
		[DataMember(Name = "CANFDChipConfigurationArbSJW")]
		private ValidatedProperty<uint> arbSJW;

		[DataMember(Name = "CANFDChipConfigurationArbTSeg1")]
		private ValidatedProperty<uint> arbTSeg1;

		[DataMember(Name = "CANFDChipConfigurationArbTSeg2")]
		private ValidatedProperty<uint> arbTSeg2;

		[DataMember(Name = "CANFDChipConfigurationArbPrescaler")]
		private ValidatedProperty<uint> arbPrescaler;

		[DataMember(Name = "CANFDChipConfigurationDataSJW")]
		private ValidatedProperty<uint> dataSJW;

		[DataMember(Name = "CANFDChipConfigurationDataTSeg1")]
		private ValidatedProperty<uint> dataTSeg1;

		[DataMember(Name = "CANFDChipConfigurationDataTSeg2")]
		private ValidatedProperty<uint> dataTSeg2;

		[DataMember(Name = "CANFDChipConfigurationDataPrescaler")]
		private ValidatedProperty<uint> dataPrescaler;

		[DataMember(Name = "CANFDChipConfigurationQuartzFreq")]
		private ValidatedProperty<long> quartzFreq;

		[DataMember(Name = "CANFDChipConfigurationSamplingPoints")]
		private ValidatedProperty<uint> sam;

		private uint arbBaudrate;

		private uint dataBaudrate;

		public bool IsCANFD
		{
			get
			{
				return true;
			}
		}

		public ValidatedProperty<uint> SAM
		{
			get
			{
				return this.sam;
			}
		}

		public ValidatedProperty<uint> SJW
		{
			get
			{
				return this.arbSJW;
			}
			set
			{
				this.arbSJW = value;
			}
		}

		public ValidatedProperty<uint> TSeg1
		{
			get
			{
				return this.arbTSeg1;
			}
			set
			{
				this.arbTSeg1 = value;
			}
		}

		public ValidatedProperty<uint> TSeg2
		{
			get
			{
				return this.arbTSeg2;
			}
			set
			{
				this.arbTSeg2 = value;
			}
		}

		public ValidatedProperty<uint> Prescaler
		{
			get
			{
				return this.arbPrescaler;
			}
			set
			{
				this.arbPrescaler = value;
			}
		}

		public uint Baudrate
		{
			get
			{
				this.CalculateDependentProperties();
				return this.arbBaudrate;
			}
		}

		public ValidatedProperty<uint> DataSJW
		{
			get
			{
				return this.dataSJW;
			}
			set
			{
				this.dataSJW = value;
			}
		}

		public ValidatedProperty<uint> DataTSeg1
		{
			get
			{
				return this.dataTSeg1;
			}
			set
			{
				this.dataTSeg1 = value;
			}
		}

		public ValidatedProperty<uint> DataTSeg2
		{
			get
			{
				return this.dataTSeg2;
			}
			set
			{
				this.dataTSeg2 = value;
			}
		}

		public ValidatedProperty<uint> DataPrescaler
		{
			get
			{
				return this.dataPrescaler;
			}
			set
			{
				this.dataPrescaler = value;
			}
		}

		public ValidatedProperty<long> QuartzFreq
		{
			get
			{
				return this.quartzFreq;
			}
			set
			{
				this.quartzFreq = value;
			}
		}

		public uint DataBaudrate
		{
			get
			{
				this.CalculateDependentProperties();
				return this.dataBaudrate;
			}
		}

		public CANFDChipConfiguration()
		{
			this.arbSJW = new ValidatedProperty<uint>(0u);
			this.arbTSeg1 = new ValidatedProperty<uint>(0u);
			this.arbTSeg2 = new ValidatedProperty<uint>(0u);
			this.arbPrescaler = new ValidatedProperty<uint>(0u);
			this.dataSJW = new ValidatedProperty<uint>(0u);
			this.dataTSeg1 = new ValidatedProperty<uint>(0u);
			this.dataTSeg2 = new ValidatedProperty<uint>(0u);
			this.dataPrescaler = new ValidatedProperty<uint>(0u);
			this.quartzFreq = new ValidatedProperty<long>(0L);
			this.sam = new ValidatedProperty<uint>(1u);
			this.arbBaudrate = 0u;
			this.dataBaudrate = 0u;
		}

		public CANFDChipConfiguration(uint arbTSeg1Val, uint arbTSeg2Val, uint arbSJWVal, uint arbPrescalerVal, uint dataTSeg1Val, uint dataTSeg2Val, uint dataSJWVal, uint dataPrescalerVal, long quartzFreqVal) : this()
		{
			this.arbTSeg1.Value = arbTSeg1Val;
			this.arbTSeg2.Value = arbTSeg2Val;
			this.arbSJW.Value = arbSJWVal;
			this.arbPrescaler.Value = arbPrescalerVal;
			this.dataTSeg1.Value = dataTSeg1Val;
			this.dataTSeg2.Value = dataTSeg2Val;
			this.dataSJW.Value = dataSJWVal;
			this.dataPrescaler.Value = dataPrescalerVal;
			this.quartzFreq.Value = quartzFreqVal;
		}

		public CANFDChipConfiguration(CANFDChipConfiguration other) : this()
		{
			this.Assign(other);
		}

		private void CalculateDependentProperties()
		{
			this.arbBaudrate = (uint)Math.Round(1.0 / (1.0 / (double)(this.quartzFreq.Value * 1000L / (long)((ulong)this.arbPrescaler.Value)) * (this.arbTSeg1.Value + this.arbTSeg2.Value + 1u)), MidpointRounding.AwayFromZero);
			this.dataBaudrate = (uint)Math.Round(1.0 / (1.0 / (double)(this.quartzFreq.Value * 1000L / (long)((ulong)this.dataPrescaler.Value)) * (this.dataTSeg1.Value + this.dataTSeg2.Value + 1u)), MidpointRounding.AwayFromZero);
		}

		public override bool Equals(object obj)
		{
			if (obj == null || base.GetType() != obj.GetType())
			{
				return false;
			}
			CANFDChipConfiguration other = obj as CANFDChipConfiguration;
			return this.EqualsArbSettings(other) && this.EqualsDataSettings(other);
		}

		public bool EqualsArbSettings(CANFDChipConfiguration other)
		{
			return ValidatedProperty<uint>.IsEqual(this.arbTSeg1, other.arbTSeg1) && ValidatedProperty<uint>.IsEqual(this.arbTSeg2, other.arbTSeg2) && ValidatedProperty<uint>.IsEqual(this.arbSJW, other.arbSJW) && ValidatedProperty<uint>.IsEqual(this.arbPrescaler, other.arbPrescaler) && ValidatedProperty<long>.IsEqual(this.quartzFreq, other.quartzFreq);
		}

		public bool EqualsDataSettings(CANFDChipConfiguration other)
		{
			return ValidatedProperty<uint>.IsEqual(this.dataTSeg1, other.dataTSeg1) && ValidatedProperty<uint>.IsEqual(this.dataTSeg2, other.dataTSeg2) && ValidatedProperty<uint>.IsEqual(this.dataSJW, other.dataSJW) && ValidatedProperty<uint>.IsEqual(this.dataPrescaler, other.dataPrescaler) && ValidatedProperty<long>.IsEqual(this.quartzFreq, other.quartzFreq);
		}

		public override int GetHashCode()
		{
			int num = 0;
			if (this.arbTSeg1 != null)
			{
				num ^= this.arbTSeg1.Value.GetHashCode();
			}
			if (this.arbTSeg2 != null)
			{
				num ^= this.arbTSeg2.Value.GetHashCode();
			}
			if (this.arbSJW != null)
			{
				num ^= this.arbSJW.Value.GetHashCode();
			}
			if (this.arbPrescaler != null)
			{
				num ^= this.arbPrescaler.Value.GetHashCode();
			}
			if (this.dataTSeg1 != null)
			{
				num ^= this.dataTSeg1.Value.GetHashCode();
			}
			if (this.dataTSeg2 != null)
			{
				num ^= this.dataTSeg2.Value.GetHashCode();
			}
			if (this.dataSJW != null)
			{
				num ^= this.dataSJW.Value.GetHashCode();
			}
			if (this.dataPrescaler != null)
			{
				num ^= this.dataPrescaler.Value.GetHashCode();
			}
			if (this.quartzFreq != null)
			{
				num ^= this.quartzFreq.GetHashCode();
			}
			return num;
		}

		public void Assign(CANFDChipConfiguration other)
		{
			this.AssignArbSettings(other);
			this.AssignDataSettings(other);
			this.quartzFreq = ((other.quartzFreq == null) ? null : new ValidatedProperty<long>(other.quartzFreq.Value));
		}

		public void AssignArbSettings(CANFDChipConfiguration other)
		{
			this.arbTSeg1 = ((other.arbTSeg1 == null) ? null : new ValidatedProperty<uint>(other.arbTSeg1.Value));
			this.arbTSeg2 = ((other.arbTSeg2 == null) ? null : new ValidatedProperty<uint>(other.arbTSeg2.Value));
			this.arbSJW = ((other.arbSJW == null) ? null : new ValidatedProperty<uint>(other.arbSJW.Value));
			this.arbPrescaler = ((other.arbPrescaler == null) ? null : new ValidatedProperty<uint>(other.arbPrescaler.Value));
			this.quartzFreq = ((other.quartzFreq == null) ? null : new ValidatedProperty<long>(other.quartzFreq.Value));
		}

		public void AssignDataSettings(CANFDChipConfiguration other)
		{
			this.dataTSeg1 = ((other.dataTSeg1 == null) ? null : new ValidatedProperty<uint>(other.dataTSeg1.Value));
			this.dataTSeg2 = ((other.dataTSeg2 == null) ? null : new ValidatedProperty<uint>(other.dataTSeg2.Value));
			this.dataSJW = ((other.dataSJW == null) ? null : new ValidatedProperty<uint>(other.dataSJW.Value));
			this.dataPrescaler = ((other.dataPrescaler == null) ? null : new ValidatedProperty<uint>(other.dataPrescaler.Value));
			this.quartzFreq = ((other.quartzFreq == null) ? null : new ValidatedProperty<long>(other.quartzFreq.Value));
		}
	}
}
