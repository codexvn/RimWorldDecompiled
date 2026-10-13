using System;

namespace UnityEngine.Bindings;

[AttributeUsage(AttributeTargets.All)]
public class NativeNameAttribute : Attribute
{
	public NativeNameAttribute()
	{
	}

	public NativeNameAttribute(string name)
	{
	}
}
