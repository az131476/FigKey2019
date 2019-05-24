using System;

namespace Vector.VLConfig.Data.ApplicationData
{
	public sealed class CcpXcpSignalRequestsPage : IApplicationDataSettings
	{
		string IApplicationDataSettings.SettingName
		{
			get
			{
				return "CcpXcpSignalRequestsPage";
			}
		}

		public string CcpXcpSignalRequestsPageGridLayout
		{
			get;
			set;
		}

		public string CcpXcpSignalExplorerShowGroups
		{
			get;
			set;
		}

		public string CcpXcpSignalExplorerHeight
		{
			get;
			set;
		}

		public string CcpXcpSignalExplorerWidth
		{
			get;
			set;
		}

		public string StatisticsTreeLayout
		{
			get;
			set;
		}

		public string StatisticsSplitterExpanded
		{
			get;
			set;
		}

		public string StatisticsSplitterPosition
		{
			get;
			set;
		}
	}
}
