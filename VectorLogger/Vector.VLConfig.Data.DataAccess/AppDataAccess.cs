using System;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ApplicationDataPersistency;

namespace Vector.VLConfig.Data.DataAccess
{
	public class AppDataAccess
	{
		private AppDataRoot appDataRoot;

		public AppDataRoot AppDataRoot
		{
			get
			{
				return this.appDataRoot;
			}
		}

		public AppDataAccess()
		{
			this.appDataRoot = new AppDataRoot();
		}

		public bool SaveAppDataSettings()
		{
			return AppDataWriter.SaveAppDataToSettingsFile(this.appDataRoot);
		}

		public bool LoadAppDataSettings()
		{
			return AppDataReader.ReadAppDataFromSettingsFile(ref this.appDataRoot);
		}
	}
}
