using System;

namespace Unity.Collections.LowLevel.Unsafe;

[AttributeUsage(AttributeTargets.Struct)]
public sealed class NativeContainerIsReadOnlyAttribute : Attribute
{
}
