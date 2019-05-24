using System;
using System.Runtime.Serialization;

namespace Vector.VLConfig.Data.ConfigurationDataModel
{
	public enum CondRelation
	{
		[EnumMember(Value = "Equal")]
		Equal,
		[EnumMember(Value = "NotEqual")]
		NotEqual,
		[EnumMember(Value = "LessThan")]
		LessThan,
		[EnumMember(Value = "LessThanOrEqual")]
		LessThanOrEqual,
		[EnumMember(Value = "GreaterThan")]
		GreaterThan,
		[EnumMember(Value = "GreaterThanOrEqual")]
		GreaterThanOrEqual,
		[EnumMember(Value = "InRange")]
		InRange,
		[EnumMember(Value = "NotInRange")]
		NotInRange,
		[EnumMember(Value = "OnChange")]
		OnChange
	}
}
