using System;

namespace Waylith.Rendering;

public readonly ref struct DrawPropertyBlock
{
	private readonly ReadOnlySpan<DrawProperty> _properties;

	public static DrawPropertyBlock Empty => new DrawPropertyBlock(ReadOnlySpan<DrawProperty>.Empty);

	public int Count => _properties.Length;

	public bool IsEmpty => _properties.IsEmpty;

	public ReadOnlySpan<DrawProperty> Properties => _properties;

	public DrawPropertyBlock(ReadOnlySpan<DrawProperty> properties)
	{
		_properties = properties;
	}
}
