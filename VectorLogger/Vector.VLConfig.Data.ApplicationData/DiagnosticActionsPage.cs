using System;

namespace Vector.VLConfig.Data.ApplicationData
{
	public sealed class DiagnosticActionsPage : IApplicationDataSettings
	{
		string IApplicationDataSettings.SettingName
		{
			get
			{
				return "DiagnosticActionsPage";
			}
		}

		public string ActionsGridLayout
		{
			get;
			set;
		}

		public string SignalSelectionDialogGridLayout
		{
			get;
			set;
		}

		public int SignalSelectionDialogHeight
		{
			get;
			set;
		}

		public int SignalSelectionDialogWidth
		{
			get;
			set;
		}

		public int SignalSelectionDialogY
		{
			get;
			set;
		}

		public int SignalSelectionDialogX
		{
			get;
			set;
		}
	}
}
