using System;

namespace Waylith.Rendering;

public sealed class DrawPropertyArena
{
	private DrawProperty[] _properties = new DrawProperty[System.Math.Max(1, initialCapacity)];

	private int _count;

	public int Count => _count;

	public DrawPropertyArena(int initialCapacity = 4096)
	{
	}

	public void Clear()
	{
		if (_count > 0)
		{
			Array.Clear(_properties, 0, _count);
		}
		_count = 0;
	}

	public Span<DrawProperty> Allocate(int count, out DrawPropertyRange range)
	{
		if (count <= 0)
		{
			range = default(DrawPropertyRange);
			return Span<DrawProperty>.Empty;
		}
		EnsureCapacity(_count + count);
		range = new DrawPropertyRange(_count, count);
		Span<DrawProperty> result = _properties.AsSpan(_count, count);
		_count += count;
		return result;
	}

	public DrawPropertyRange Copy(DrawPropertyBlock block)
	{
		return Copy(block.Properties);
	}

	public DrawPropertyRange Copy(ReadOnlySpan<DrawProperty> properties)
	{
		if (properties.IsEmpty)
		{
			return default(DrawPropertyRange);
		}
		DrawPropertyRange range;
		Span<DrawProperty> destination = Allocate(properties.Length, out range);
		properties.CopyTo(destination);
		return range;
	}

	public DrawPropertyArena Clone()
	{
		DrawPropertyArena drawPropertyArena = new DrawPropertyArena(System.Math.Max(1, _count));
		if (_count > 0)
		{
			DrawPropertyRange range;
			Span<DrawProperty> destination = drawPropertyArena.Allocate(_count, out range);
			_properties.AsSpan(0, _count).CopyTo(destination);
		}
		return drawPropertyArena;
	}

	public ReadOnlySpan<DrawProperty> GetProperties(DrawPropertyRange range)
	{
		if (range.Count <= 0)
		{
			return ReadOnlySpan<DrawProperty>.Empty;
		}
		if ((uint)range.Start > (uint)_count || range.Start + range.Count > _count)
		{
			return ReadOnlySpan<DrawProperty>.Empty;
		}
		return _properties.AsSpan(range.Start, range.Count);
	}

	private void EnsureCapacity(int capacity)
	{
		if (capacity > _properties.Length)
		{
			int num;
			for (num = _properties.Length; num < capacity; num *= 2)
			{
			}
			Array.Resize(ref _properties, num);
		}
	}
}
