using System;

namespace Vector.VLConfig.Data.ApplicationData
{
	public sealed class FiltersPage : IApplicationDataSettings
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
				return string.Format("Filters{0}Page", this.MemoryNr);
			}
		}

		public string FiltersGridLayout
		{
			get;
			set;
		}
	}
}
