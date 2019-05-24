using System;

namespace Vector.VLConfig.Data.ApplicationData
{
	public class CANIdInterval
	{
		public uint Begin;

		public uint End;

		public bool IsExtended;

		public bool IsSingleValue
		{
			get
			{
				return this.Begin == this.End;
			}
		}

		public CANIdInterval(uint singleValue, bool isExtended)
		{
			this.Begin = singleValue;
			this.End = singleValue;
			this.IsExtended = isExtended;
		}

		public CANIdInterval(uint beginValue, uint endValue, bool isExtended)
		{
			if (beginValue <= endValue)
			{
				this.Begin = beginValue;
				this.End = endValue;
			}
			else
			{
				this.Begin = endValue;
				this.End = beginValue;
			}
			this.IsExtended = isExtended;
		}

		public bool Contains(CANIdInterval other)
		{
			return other.IsExtended == this.IsExtended && ((other.Begin >= this.Begin && other.Begin <= this.End) || (other.End >= this.Begin && other.End <= this.End) || (other.Begin < this.Begin && other.End > this.End));
		}

		public bool MergeWith(CANIdInterval other)
		{
			if (other.IsExtended != this.IsExtended)
			{
				return false;
			}
			if (other.Begin >= this.Begin && other.Begin <= this.End)
			{
				if (other.End > this.End)
				{
					this.End = other.End;
				}
				return true;
			}
			if (other.End >= this.Begin && other.End <= this.End)
			{
				if (other.Begin < this.Begin)
				{
					this.Begin = other.Begin;
				}
				return true;
			}
			return other.Begin < this.Begin && other.End > this.End;
		}

		public bool MergeWith(uint singleCanId, bool isExtended)
		{
			return isExtended == this.IsExtended && (singleCanId >= this.Begin && singleCanId <= this.End);
		}

		public bool MergeWith(uint canIdBegin, uint canIdEnd, bool isExtended)
		{
			if (isExtended != this.IsExtended)
			{
				return false;
			}
			if (canIdBegin >= this.Begin && canIdBegin <= this.End)
			{
				if (canIdEnd > this.End)
				{
					this.End = canIdEnd;
				}
				return true;
			}
			if (canIdEnd >= this.Begin && canIdEnd <= this.End)
			{
				if (canIdBegin < this.Begin)
				{
					this.Begin = canIdBegin;
				}
				return true;
			}
			return canIdBegin < this.Begin && canIdEnd > this.End;
		}
	}
}
