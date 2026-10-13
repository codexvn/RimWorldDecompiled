using System;

namespace UnityEngine.Bindings;

[AttributeUsage(AttributeTargets.All)]
public class NativeMethodAttribute : Attribute
{
	public NativeMethodAttribute()
	{
	}

	public NativeMethodAttribute(string name)
	{
	}

	public NativeMethodAttribute(string name, bool isThreadSafe)
	{
	}

	public NativeMethodAttribute(string name, bool isThreadSafe, bool isFreeFunction)
	{
	}
}
