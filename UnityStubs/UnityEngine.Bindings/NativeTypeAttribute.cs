using System;

namespace UnityEngine.Bindings;

[AttributeUsage(AttributeTargets.All)]
public class NativeTypeAttribute : Attribute
{
	public NativeTypeAttribute()
	{
	}

	public NativeTypeAttribute(string header)
	{
	}

	public NativeTypeAttribute(CodegenOptions codegenOptions)
	{
	}
}
