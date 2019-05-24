using System;

namespace Vector.VLConfig.Data.ApplicationData
{
	public sealed class CcpXcpDescriptionsPage : IApplicationDataSettings
	{
		string IApplicationDataSettings.SettingName
		{
			get
			{
				return "CcpXcpDescriptionsPage";
			}
		}

		public string CcpXcpDescriptionsPageGridLayout
		{
			get;
			set;
		}
	}
}
