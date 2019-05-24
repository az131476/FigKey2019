using System;
using System.Collections.Generic;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	public interface IFeatureReferencedFiles
	{
		IList<IFile> ReferencedFiles
		{
			get;
		}
	}
}
