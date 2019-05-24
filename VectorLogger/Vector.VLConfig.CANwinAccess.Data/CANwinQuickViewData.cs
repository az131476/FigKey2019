using System;
using System.Collections.Generic;
using System.IO;
using Vector.VLConfig.GeneralUtil.QuickView;

namespace Vector.VLConfig.CANwinAccess.Data
{
	public class CANwinQuickViewData
	{
		private const string cDatabasesSubFolder = "databases";

		private CANwinServerType mServerType;

		public CANwinServerType ServerType
		{
			get
			{
				return this.mServerType;
			}
			set
			{
				this.mServerType = value;
				this.ProductName = CreateOfflineConfigJob.GetProductName(this.mServerType);
			}
		}

		public string ProductName
		{
			get;
			private set;
		}

		public List<CANwinBusItem> BusConfiguration
		{
			get;
			private set;
		}

		public List<string> SysvarFiles
		{
			get;
			private set;
		}

		public OfflineSourceConfig OfflineSourceConfig
		{
			get;
			private set;
		}

		public string BaseConfigFolder
		{
			get;
			set;
		}

		public string ConfigFolder
		{
			get
			{
				return Path.Combine(this.BaseConfigFolder, this.ServerType.ToString());
			}
		}

		public string DatabasesFolder
		{
			get
			{
				return Path.Combine(this.ConfigFolder, "databases");
			}
		}

		public bool UseUserDefinedTemplate
		{
			get;
			set;
		}

		public string UserDefinedTemplate
		{
			get;
			set;
		}

		public CANwinQuickViewData()
		{
			this.mServerType = CANwinServerType.CANoe;
			this.BusConfiguration = new List<CANwinBusItem>();
			this.SysvarFiles = new List<string>();
			this.OfflineSourceConfig = new OfflineSourceConfig();
		}
	}
}
