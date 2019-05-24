using System;
using System.Collections.Generic;

namespace Vector.VLConfig.Data.ApplicationData
{
	public class FileConversionProfileList : IApplicationDataSettings
	{
		private List<string> mProfiles;

		string IApplicationDataSettings.SettingName
		{
			get
			{
				return "FileConversionProfileList";
			}
		}

		public List<string> Profiles
		{
			get
			{
				List<string> arg_1B_0;
				if ((arg_1B_0 = this.mProfiles) == null)
				{
					arg_1B_0 = (this.mProfiles = new List<string>());
				}
				return arg_1B_0;
			}
			set
			{
				this.mProfiles = value;
			}
		}
	}
}
