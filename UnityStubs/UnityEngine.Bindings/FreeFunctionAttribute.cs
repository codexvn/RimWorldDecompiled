using System;

namespace UnityEngine.Bindings;

[AttributeUsage(AttributeTargets.All)]
public class FreeFunctionAttribute : Attribute
{
	public FreeFunctionAttribute()
	{
	}

	public FreeFunctionAttribute(string name)
	{
	}

	public FreeFunctionAttribute(string name, bool isThreadSafe)
	{
	}
}
