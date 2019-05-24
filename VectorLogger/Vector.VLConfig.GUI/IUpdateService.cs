using System;
using Vector.VLConfig.Data.ApplicationData;
using Vector.VLConfig.Data.ConfigurationDataModel;
using Vector.VLConfig.LoggerSpecifics;

namespace Vector.VLConfig.GUI
{
	public interface IUpdateService : IUpdateServiceForFeature
	{
		void AddUpdateObserver(IUpdateObserver observer, UpdateContext updateContext);

		void RemoveUpdateObserver(IUpdateObserver observer);

		void Notify<T>(T entity, IUpdateObserver observer) where T : Feature;

		void NotifyLoggerType(LoggerType type);

		void NotifyDisplayMode(DisplayMode mode);
	}
}
