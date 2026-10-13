using System;

namespace UnityEngine.Bindings;

[AttributeUsage(AttributeTargets.All)]
public class StaticAccessorAttribute : Attribute
{
	public StaticAccessorAttribute()
	{
	}

	public StaticAccessorAttribute(string name)
	{
	}

	public StaticAccessorAttribute(string name, StaticAccessorType type)
	{
	}
}
