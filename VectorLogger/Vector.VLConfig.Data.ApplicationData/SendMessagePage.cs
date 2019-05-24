using System;

namespace Vector.VLConfig.Data.ApplicationData
{
	public sealed class SendMessagePage : IApplicationDataSettings
	{
		string IApplicationDataSettings.SettingName
		{
			get
			{
				return "SendMessagePage";
			}
		}

		public string SendMessageGridLayout
		{
			get;
			set;
		}
	}
}
