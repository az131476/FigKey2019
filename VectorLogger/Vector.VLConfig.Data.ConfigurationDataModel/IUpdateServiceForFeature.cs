using System;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	public interface IUpdateServiceForFeature
	{
		void Notify<T>(T entity) where T : Feature;
	}
}
