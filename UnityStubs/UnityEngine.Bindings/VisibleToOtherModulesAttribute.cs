using System;

namespace UnityEngine.Bindings;

[AttributeUsage(AttributeTargets.All)]
public class VisibleToOtherModulesAttribute : Attribute
{
	public VisibleToOtherModulesAttribute()
	{
	}

	public VisibleToOtherModulesAttribute(params string[] modules)
	{
	}
}
