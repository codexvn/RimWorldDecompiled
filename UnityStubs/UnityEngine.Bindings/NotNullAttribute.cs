using System;

namespace UnityEngine.Bindings;

[AttributeUsage(AttributeTargets.All)]
public class NotNullAttribute : Attribute
{
	public NotNullAttribute()
	{
	}

	public NotNullAttribute(string exception)
	{
	}
}
