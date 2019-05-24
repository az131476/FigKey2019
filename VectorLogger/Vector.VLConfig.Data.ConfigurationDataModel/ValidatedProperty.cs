using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	[DataContract(Name = "ValidatedProperty")]
	public class ValidatedProperty<T> : IValidatedProperty<T>, IValidatedProperty
	{
		[DataMember(Name = "Value")]
		private T val;

		public T Value
		{
			get
			{
				return this.val;
			}
			set
			{
				this.val = value;
			}
		}

		public static bool IsEqual(ValidatedProperty<T> prop1, ValidatedProperty<T> prop2)
		{
			if ((prop1 == null && prop2 != null) || (prop1 != null && prop2 == null))
			{
				return false;
			}
			if (prop1 != null && prop2 != null)
			{
				T value = prop1.Value;
				if (!value.Equals(prop2.Value))
				{
					return false;
				}
			}
			return true;
		}

		public ValidatedProperty()
		{
		}

		public ValidatedProperty(T value)
		{
			this.val = value;
		}

		public override string ToString()
		{
			return this.val.ToString();
		}
	}
}
