using System;

namespace UnityEngine.Scripting;

[AttributeUsage(AttributeTargets.All)]
public class UsedByNativeCodeAttribute : Attribute
{
	public UsedByNativeCodeAttribute()
	{
	}

	public UsedByNativeCodeAttribute(string name)
	{
	}
}
