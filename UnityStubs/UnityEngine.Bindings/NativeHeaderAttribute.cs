using System;

namespace UnityEngine.Bindings;

[AttributeUsage(AttributeTargets.All)]
public class NativeHeaderAttribute : Attribute
{
	public NativeHeaderAttribute()
	{
	}

	public NativeHeaderAttribute(string header)
	{
	}
}
