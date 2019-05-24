using System;
using System.Collections.Generic;
using Vector.VLConfig.LoggerSpecifics;

namespace Vector.VLConfig.BusinessLogic
{
	public class GlobalRuntimeSettings
	{
		public enum ApplicationMode
		{
			Default,
			BatchMode
		}

		private static GlobalRuntimeSettings _instance;

		private GlobalRuntimeSettings.ApplicationMode m_applicationMode;

		private string m_batchModePath;

		private LoggerType m_loggerType;

		private bool m_recoverMeasurements;

		private Dictionary<string, string> m_batchModePathList;

		private bool m_standaloneExportMode;

		public GlobalRuntimeSettings.ApplicationMode ActiveApplicationMode
		{
			get
			{
				return this.m_applicationMode;
			}
			set
			{
				this.m_applicationMode = value;
			}
		}

		public string BatchModePath
		{
			get
			{
				return this.m_batchModePath;
			}
			set
			{
				this.m_batchModePath = value;
			}
		}

		public LoggerType LoggerType
		{
			get
			{
				return this.m_loggerType;
			}
			set
			{
				this.m_loggerType = value;
			}
		}

		public bool RecoverMeasurements
		{
			get
			{
				return this.m_recoverMeasurements;
			}
			set
			{
				this.m_recoverMeasurements = value;
			}
		}

		public bool HasMultipleBatchFiles
		{
			get
			{
				return this.m_batchModePathList != null && this.m_batchModePathList.Count > 0;
			}
		}

		public Dictionary<string, string> BatchModePathList
		{
			get
			{
				return this.m_batchModePathList;
			}
			set
			{
				this.m_batchModePathList = value;
			}
		}

		public bool StandaloneExportMode
		{
			get
			{
				return this.m_standaloneExportMode;
			}
			set
			{
				this.m_standaloneExportMode = value;
			}
		}

		public static GlobalRuntimeSettings GetInstance()
		{
			if (GlobalRuntimeSettings._instance == null)
			{
				GlobalRuntimeSettings._instance = new GlobalRuntimeSettings();
			}
			return GlobalRuntimeSettings._instance;
		}

		private GlobalRuntimeSettings()
		{
			this.ActiveApplicationMode = GlobalRuntimeSettings.ApplicationMode.Default;
			this.BatchModePath = string.Empty;
			this.LoggerType = LoggerType.Unknown;
			this.RecoverMeasurements = true;
			this.m_batchModePathList = new Dictionary<string, string>();
			this.m_standaloneExportMode = false;
		}
	}
}
