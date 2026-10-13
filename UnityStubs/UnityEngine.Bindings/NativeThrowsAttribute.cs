using System;

namespace UnityEngine.Bindings;

[AttributeUsage(AttributeTargets.All)]
public class NativeThrowsAttribute : Attribute
{
	public NativeThrowsAttribute()
	{
	}

	public NativeThrowsAttribute(bool throwsException)
	{
	}
}
