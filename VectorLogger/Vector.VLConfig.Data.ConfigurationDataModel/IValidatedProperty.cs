using System;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	public interface IValidatedProperty
	{
	}
	public interface IValidatedProperty<T> : IValidatedProperty
	{
		T Value
		{
			get;
			set;
		}
	}
}
