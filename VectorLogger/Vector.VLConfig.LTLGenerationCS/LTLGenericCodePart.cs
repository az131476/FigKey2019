using System;
using System.Text;

namespace Vector.VLConfig.LTLGenerationCS
{
	public abstract class LTLGenericCodePart
	{
		private StringBuilder ltlCode;

		private StringBuilder ltlSystemCode;

		public StringBuilder LtlCode
		{
			get
			{
				if (this.ltlCode != null)
				{
					return this.ltlCode;
				}
				return new StringBuilder();
			}
			protected set
			{
				this.ltlCode = value;
			}
		}

		public StringBuilder LtlSystemCode
		{
			get
			{
				if (this.ltlCode != null)
				{
					return this.ltlSystemCode;
				}
				return new StringBuilder();
			}
			protected set
			{
				this.ltlSystemCode = value;
			}
		}

		public LTLGenericCodePart()
		{
			this.LtlCode = new StringBuilder();
			this.LtlSystemCode = new StringBuilder();
		}
	}
}
