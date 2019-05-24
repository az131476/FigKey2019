using System;

namespace Vector.VLConfig.Data.ConfigurationPersistency
{
	public delegate void ProjectImportFileOverwriteHandler(string fileName, ref bool overwrite);
}
