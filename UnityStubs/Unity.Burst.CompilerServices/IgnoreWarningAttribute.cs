using System;

namespace Unity.Burst.CompilerServices;

public class IgnoreWarningAttribute : Attribute
{
	public IgnoreWarningAttribute(int warning)
	{
	}
}
