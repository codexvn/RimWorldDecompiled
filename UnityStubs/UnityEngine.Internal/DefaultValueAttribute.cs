using System;

namespace UnityEngine.Internal;

[AttributeUsage(AttributeTargets.All)]
public class DefaultValueAttribute : Attribute
{
	public object Value { get; }

	public DefaultValueAttribute(string value)
	{
		Value = value;
	}
}
