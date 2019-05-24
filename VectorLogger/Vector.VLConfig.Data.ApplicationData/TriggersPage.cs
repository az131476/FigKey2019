using System;

namespace Vector.VLConfig.Data.ApplicationData
{
	public sealed class TriggersPage : IApplicationDataSettings
	{
		public int MemoryNr
		{
			get;
			set;
		}

		string IApplicationDataSettings.SettingName
		{
			get
			{
				return string.Format("Triggers{0}Page", this.MemoryNr);
			}
		}

		public string TriggersTreeLayout
		{
			get;
			set;
		}
	}
}
