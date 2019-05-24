using System;

namespace Vector.VLConfig.CANwinAccess.COM
{
	internal class CANoeApplication : CANwinApplication
	{
		public override CANwinServerType ServerType
		{
			get
			{
				return CANwinServerType.CANoe;
			}
		}

		protected override string ProgId
		{
			get
			{
				return Vocabulary.ProgIdCANoe;
			}
		}
	}
}
