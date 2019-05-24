using System;

namespace Vector.VLConfig.ValidationFramework
{
	internal interface IValidatable
	{
		IValidationHost ValidationHost
		{
			get;
			set;
		}

		bool ValidateInput(ref bool valueChanged);
	}
}
