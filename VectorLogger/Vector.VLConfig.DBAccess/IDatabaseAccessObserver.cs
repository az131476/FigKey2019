using System;

namespace Vector.VLConfig.DBAccess
{
	public interface IDatabaseAccessObserver
	{
		bool LoadDatabases();

		void OnDatabaseChange(string absDbFilePath);

		void OnDatabaseLoaded();
	}
}
