using System;

namespace UnityEngine.Bindings;

[AttributeUsage(AttributeTargets.All)]
public class NativeConditionalAttribute : Attribute
{
	public NativeConditionalAttribute()
	{
	}

	public NativeConditionalAttribute(string condition)
	{
	}

	public NativeConditionalAttribute(bool enabled)
	{
	}
}
