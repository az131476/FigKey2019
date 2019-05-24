using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "CANStdChipConfiguration")]
	public class CANStdChipConfiguration : ICANChipConfiguration
	{
		[DataMember(Name = "CANStdChipConfigurationBTR0")]
		private ValidatedProperty<uint> btr0;

		[DataMember(Name = "CANStdChipConfigurationBTR1")]
		private ValidatedProperty<uint> btr1;

		[DataMember(Name = "CANStdChipConfigurationQuartzFreq")]
		private ValidatedProperty<long> quartzFreq;

		private ValidatedProperty<uint> sjw;

		private ValidatedProperty<uint> tseg1;

		private ValidatedProperty<uint> tseg2;

		private ValidatedProperty<uint> prescaler;

		private ValidatedProperty<uint> sam;

		private uint baudrate;

		public bool IsCANFD
		{
			get
			{
				return false;
			}
		}

		public ValidatedProperty<uint> SAM
		{
			get
			{
				this.CalculateDependentProperties();
				return this.sam;
			}
		}

		public ValidatedProperty<uint> SJW
		{
			get
			{
				this.CalculateDependentProperties();
				return this.sjw;
			}
		}

		public ValidatedProperty<uint> TSeg1
		{
			get
			{
				this.CalculateDependentProperties();
				return this.tseg1;
			}
		}

		public ValidatedProperty<uint> TSeg2
		{
			get
			{
				this.CalculateDependentProperties();
				return this.tseg2;
			}
		}

		public ValidatedProperty<uint> Prescaler
		{
			get
			{
				this.CalculateDependentProperties();
				return this.prescaler;
			}
		}

		public uint Baudrate
		{
			get
			{
				this.CalculateDependentProperties();
				return this.baudrate;
			}
		}

		public ValidatedProperty<uint> BTR0
		{
			get
			{
				return this.btr0;
			}
			set
			{
				this.btr0 = value;
			}
		}

		public ValidatedProperty<uint> BTR1
		{
			get
			{
				return this.btr1;
			}
			set
			{
				this.btr1 = value;
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

		public CANStdChipConfiguration()
		{
			this.btr0 = new ValidatedProperty<uint>(0u);
			this.btr1 = new ValidatedProperty<uint>(0u);
			this.quartzFreq = new ValidatedProperty<long>(0L);
			this.sjw = new ValidatedProperty<uint>(0u);
			this.tseg1 = new ValidatedProperty<uint>(0u);
			this.tseg2 = new ValidatedProperty<uint>(0u);
			this.prescaler = new ValidatedProperty<uint>(0u);
			this.sam = new ValidatedProperty<uint>(0u);
			this.baudrate = 0u;
		}

		public CANStdChipConfiguration(uint btr0Val, uint btr1Val, long quartzFreqVal) : this()
		{
			this.btr0.Value = btr0Val;
			this.btr1.Value = btr1Val;
			this.quartzFreq.Value = quartzFreqVal;
			this.CalculateDependentProperties();
		}

		public CANStdChipConfiguration(CANStdChipConfiguration other) : this()
		{
			this.Assign(other);
		}

		private void CalculateDependentProperties()
		{
			if (this.tseg1 == null)
			{
				this.tseg1 = new ValidatedProperty<uint>(0u);
			}
			this.tseg1.Value = (this.btr1.Value & 15u) + 1u;
			if (this.tseg2 == null)
			{
				this.tseg2 = new ValidatedProperty<uint>(0u);
			}
			this.tseg2.Value = (this.btr1.Value >> 4 & 7u) + 1u;
			if (this.prescaler == null)
			{
				this.prescaler = new ValidatedProperty<uint>(0u);
			}
			this.prescaler.Value = (this.btr0.Value & 63u) + 1u;
			if (this.sjw == null)
			{
				this.sjw = new ValidatedProperty<uint>(0u);
			}
			this.sjw.Value = (this.btr0.Value >> 6 & 3u) + 1u;
			if (this.sam == null)
			{
				this.sam = new ValidatedProperty<uint>(0u);
			}
			this.sam.Value = 1u;
			if ((this.btr1.Value >> 7 & 1u) == 1u)
			{
				this.sam.Value = 3u;
			}
			this.baudrate = (uint)(1.0 / (1.0 / (double)(this.quartzFreq.Value * 1000L / 2L / (long)((ulong)this.prescaler.Value)) * (this.tseg1.Value + this.tseg2.Value + 1u)) + 0.05);
		}

		public override bool Equals(object obj)
		{
			if (obj == null || base.GetType() != obj.GetType())
			{
				return false;
			}
			CANStdChipConfiguration cANStdChipConfiguration = obj as CANStdChipConfiguration;
			return cANStdChipConfiguration != null && ValidatedProperty<uint>.IsEqual(this.btr0, cANStdChipConfiguration.btr0) && ValidatedProperty<uint>.IsEqual(this.btr1, cANStdChipConfiguration.btr1) && ValidatedProperty<long>.IsEqual(this.quartzFreq, cANStdChipConfiguration.quartzFreq);
		}

		public override int GetHashCode()
		{
			int num = 0;
			if (this.btr0 != null)
			{
				num ^= this.btr0.Value.GetHashCode();
			}
			if (this.btr1 != null)
			{
				num ^= this.btr1.Value.GetHashCode();
			}
			if (this.quartzFreq != null)
			{
				num ^= this.quartzFreq.GetHashCode();
			}
			return num;
		}

		public void Assign(CANStdChipConfiguration other)
		{
			this.btr0 = ((other.btr0 == null) ? null : new ValidatedProperty<uint>(other.btr0.Value));
			this.btr1 = ((other.btr1 == null) ? null : new ValidatedProperty<uint>(other.btr1.Value));
			this.quartzFreq = ((other.quartzFreq == null) ? null : new ValidatedProperty<long>(other.quartzFreq.Value));
		}
	}
}
