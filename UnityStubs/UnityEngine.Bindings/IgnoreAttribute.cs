using System;

namespace UnityEngine.Bindings;

[AttributeUsage(AttributeTargets.All)]
public class IgnoreAttribute : Attribute
{
	public bool DoesNotContributeToSize { get; set; }
}
