using System;

namespace Vector.VLConfig.CANwinAccess.COM
{
	internal class CANalyzerApplication : CANwinApplication
	{
		public override CANwinServerType ServerType
		{
			get
			{
				return CANwinServerType.CANalyzer;
			}
		}

		protected override string ProgId
		{
			get
			{
				return Vocabulary.ProgIdCANalyzer;
			}
		}
	}
}
