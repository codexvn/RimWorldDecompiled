using System;

namespace Waylith.Rendering;

[AttributeUsage(AttributeTargets.Field)]
public sealed class TextureUsageAttribute(TextureUsage usage) : Attribute()
{
	public TextureUsage Usage { get; } = usage;
}
