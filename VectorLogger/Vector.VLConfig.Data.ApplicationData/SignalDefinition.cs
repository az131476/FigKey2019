using System;
using System.Globalization;

namespace Vector.VLConfig.Data.ApplicationData
{
	public class SignalDefinition
	{
		private MessageDefinition message;

		private uint startBit;

		private uint length;

		private bool isMotorola;

		private bool isIntegerType;

		private bool isSigned;

		private int displayPrecision;

		private bool isMultiRange;

		private double factor;

		private double offset;

		private bool hasLinearConversion;

		private bool hasTextEncodedValueTable;

		private string unit;

		private SignalDefinition multiplexor;

		private uint multiplexorValue;

		private int updateBitPosition;

		public static readonly int NoUpdateBit = -1;

		public MessageDefinition Message
		{
			get
			{
				return this.message;
			}
		}

		public uint StartBit
		{
			get
			{
				return this.startBit;
			}
			set
			{
				this.startBit = value;
			}
		}

		public uint Length
		{
			get
			{
				return this.length;
			}
			set
			{
				this.length = value;
			}
		}

		public bool IsMotorola
		{
			get
			{
				return this.isMotorola;
			}
		}

		public bool IsSigned
		{
			get
			{
				return this.isSigned;
			}
			set
			{
				this.isSigned = value;
			}
		}

		public bool IsIntegerType
		{
			get
			{
				return this.isIntegerType;
			}
		}

		public double Factor
		{
			get
			{
				return this.factor;
			}
			set
			{
				this.factor = value;
			}
		}

		public double Offset
		{
			get
			{
				return this.offset;
			}
		}

		public bool HasLinearConversion
		{
			get
			{
				return this.hasLinearConversion;
			}
		}

		public string Unit
		{
			get
			{
				return this.unit;
			}
		}

		public SignalDefinition Multiplexor
		{
			get
			{
				return this.multiplexor;
			}
		}

		public uint MultiplexorValue
		{
			get
			{
				return this.multiplexorValue;
			}
		}

		public bool IsMultiplexed
		{
			get
			{
				return this.Multiplexor != null;
			}
		}

		public bool IsMultiRange
		{
			get
			{
				return this.isMultiRange;
			}
		}

		public int UpdateBit
		{
			get
			{
				return this.updateBitPosition;
			}
		}

		public bool HasUpdateBit
		{
			get
			{
				return this.updateBitPosition > 0;
			}
		}

		public bool HasTextEncodedValueTable
		{
			get
			{
				return this.hasTextEncodedValueTable;
			}
		}

		public SignalDefinition()
		{
			this.message = new MessageDefinition(0u);
			this.startBit = 0u;
			this.length = 0u;
			this.isMotorola = false;
			this.isSigned = false;
			this.isIntegerType = true;
			this.factor = 1.0;
			this.offset = 0.0;
			this.unit = null;
			this.multiplexor = null;
			this.displayPrecision = 0;
			this.isMultiRange = false;
			this.hasLinearConversion = true;
			this.hasTextEncodedValueTable = false;
		}

		public SignalDefinition(uint length, bool isSigned, bool isIntegerType, double factor, double offset, bool hasLinearConversion)
		{
			this.SetSignal(new MessageDefinition(0u), 0u, length, false, isSigned, isIntegerType, string.Empty, factor, offset, false, 0, false, hasLinearConversion);
		}

		public SignalDefinition(uint messageId, uint startBit, uint length, bool isMotorola, bool isSigned, bool isIntegerType)
		{
			this.SetSignal(new MessageDefinition(messageId), startBit, length, isMotorola, isSigned, isIntegerType, null, 1.0, 0.0, false, SignalDefinition.NoUpdateBit, false, true);
		}

		public void SetSignal(MessageDefinition message, uint startBit, uint length, bool isMotorola, bool isSigned, bool isIntegerType, string unit, double factor, double offset, bool isMultiRanged, int updateBit, bool hasValueTable, bool hasLinearConversion = true)
		{
			this.message = message;
			this.startBit = startBit;
			this.length = length;
			this.isMotorola = isMotorola;
			this.isSigned = isSigned;
			this.isIntegerType = isIntegerType;
			this.factor = factor;
			this.offset = offset;
			this.displayPrecision = 0;
			double num = 1.0;
			if (factor == 1.0)
			{
				if (offset == 0.0)
				{
					goto IL_A8;
				}
			}
			while (Math.Abs(this.factor) < num)
			{
				this.displayPrecision++;
				if (this.displayPrecision > 11)
				{
					break;
				}
				num /= 10.0;
			}
			IL_A8:
			this.isMultiRange = isMultiRanged;
			this.unit = unit;
			this.updateBitPosition = updateBit;
			this.hasLinearConversion = hasLinearConversion;
			this.hasTextEncodedValueTable = hasValueTable;
		}

		public void SetSignal(MessageDefinition message, uint startBit, uint length, bool isMotorola, bool isSigned, bool isIntegerType)
		{
			this.SetSignal(message, startBit, length, isMotorola, isSigned, isIntegerType, null, 1.0, 0.0, false, SignalDefinition.NoUpdateBit, false, true);
		}

		public void SetDummySignal()
		{
			this.SetSignal(new MessageDefinition(0u), 0u, 16u, false, false, true, null, 1.0, 0.0, false, SignalDefinition.NoUpdateBit, false, true);
		}

		public void SetMultiplexor(SignalDefinition multiplexorSignal, uint multiplexorValue)
		{
			this.multiplexor = multiplexorSignal;
			this.multiplexorValue = multiplexorValue;
		}

		public string PhysicalValueToString(double physicalValue)
		{
			string format = string.Format("F{0}", this.displayPrecision);
			return physicalValue.ToString(format, CultureInfo.InvariantCulture);
		}

		public SignalDefinition Clone()
		{
			return (SignalDefinition)base.MemberwiseClone();
		}
	}
}
