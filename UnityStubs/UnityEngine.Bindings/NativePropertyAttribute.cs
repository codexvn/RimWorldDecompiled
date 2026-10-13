using System;

namespace UnityEngine.Bindings;

[AttributeUsage(AttributeTargets.All)]
public class NativePropertyAttribute : Attribute
{
	public NativePropertyAttribute()
	{
	}

	public NativePropertyAttribute(string name)
	{
	}

	public NativePropertyAttribute(string name, TargetType targetType)
	{
	}

	public NativePropertyAttribute(string name, bool isThreadSafe, TargetType targetType)
	{
	}
}
