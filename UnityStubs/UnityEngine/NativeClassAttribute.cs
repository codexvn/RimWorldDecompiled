using System;

namespace UnityEngine;

[AttributeUsage(AttributeTargets.All)]
public class NativeClassAttribute : Attribute
{
	public NativeClassAttribute(string qualifiedNativeName)
	{
	}

	public NativeClassAttribute(string qualifiedNativeName, string declaration)
	{
	}
}
