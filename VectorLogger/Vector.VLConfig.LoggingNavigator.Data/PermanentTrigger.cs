using System;

namespace Vector.VLConfig.LoggingNavigator.Data
{
	public class PermanentTrigger : Trigger
	{
		private static uint fixedPermID = 4294967295u;

		public override string Name
		{
			get
			{
				return "Permanent Measurement";
			}
		}

		public override string Label
		{
			get
			{
				return "";
			}
		}

		public override string LabelAndOccurences
		{
			get
			{
				return this.Label;
			}
		}

		public override int LabelOccurenceCount
		{
			get
			{
				return base.Measurement.Number;
			}
		}

		public override uint Type
		{
			get
			{
				return PermanentTrigger.fixedPermID;
			}
		}

		public override uint Instance
		{
			get
			{
				return PermanentTrigger.fixedPermID;
			}
		}

		public override uint ID
		{
			get
			{
				return PermanentTrigger.fixedPermID;
			}
		}

		public PermanentTrigger(Measurement measurement, IdManager idManager) : base(0u, false, 0u, 0u, measurement.Begin, measurement, idManager)
		{
		}
	}
}
