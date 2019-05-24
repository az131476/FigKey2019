using System;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	public interface IFile
	{
		ValidatedProperty<string> FilePath
		{
			get;
		}
	}
}
