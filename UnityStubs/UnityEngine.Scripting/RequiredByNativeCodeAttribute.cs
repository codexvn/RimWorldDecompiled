using System;

namespace UnityEngine.Scripting;

[AttributeUsage(AttributeTargets.All)]
public class RequiredByNativeCodeAttribute : Attribute
{
	public bool Optional { get; set; }

	public bool GenerateProxy { get; set; }

	public RequiredByNativeCodeAttribute()
	{
	}

	public RequiredByNativeCodeAttribute(string name)
	{
	}

	public RequiredByNativeCodeAttribute(bool optional)
	{
	}

	public RequiredByNativeCodeAttribute(string name, bool optional)
	{
	}
}
