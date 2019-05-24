using System;

namespace Vector.VLConfig.GUI
{
	public interface IUpdateObserver
	{
	}
	public interface IUpdateObserver<T> : IUpdateObserver
	{
		void Update(T data);
	}
}
