using System;

namespace Vector.VLConfig.LTLGenerationCS.Utility
{
	public class Variable
	{
		private string name;

		private byte bitlength;

		private bool isSigned;

		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}

		public byte Bitlength
		{
			get
			{
				return this.bitlength;
			}
			set
			{
				this.bitlength = value;
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

		public Variable(string name, byte bitlength, bool isSigned)
		{
			this.Name = name;
			this.Bitlength = bitlength;
			this.IsSigned = isSigned;
		}

		public int GetMinValue()
		{
			int result;
			if (this.IsSigned)
			{
				result = -(1 << (int)(this.bitlength - 1));
			}
			else
			{
				result = 0;
			}
			return result;
		}

		public int GetMaxValue()
		{
			int result;
			if (this.IsSigned)
			{
				result = (1 << (int)(this.bitlength - 1)) - 1;
			}
			else
			{
				result = (1 << (int)this.Bitlength) - 1;
			}
			return result;
		}
	}
}
