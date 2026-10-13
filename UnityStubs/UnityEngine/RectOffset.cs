using System;
using System.Runtime.CompilerServices;

namespace UnityEngine;

public class RectOffset : IFormattable
{
	public int left { get; set; }

	public int right { get; set; }

	public int top { get; set; }

	public int bottom { get; set; }

	public int horizontal => left + right;

	public int vertical => top + bottom;

	public RectOffset()
	{
	}

	public RectOffset(int left, int right, int top, int bottom)
	{
		this.left = left;
		this.right = right;
		this.top = top;
		this.bottom = bottom;
	}

	public Rect Add(Rect rect)
	{
		return Rect.MinMaxRect(rect.xMin - (float)left, rect.yMin - (float)top, rect.xMax + (float)right, rect.yMax + (float)bottom);
	}

	public Rect Remove(Rect rect)
	{
		return Rect.MinMaxRect(rect.xMin + (float)left, rect.yMin + (float)top, rect.xMax - (float)right, rect.yMax - (float)bottom);
	}

	public override string ToString()
	{
		return ToString(null, null);
	}

	public string ToString(string format)
	{
		return ToString(format, null);
	}

	public string ToString(string format, IFormatProvider formatProvider)
	{
		InlineArray4<object> buffer = default(InlineArray4<object>);
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray4<object>, object>(ref buffer, 0) = left;
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray4<object>, object>(ref buffer, 1) = right;
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray4<object>, object>(ref buffer, 2) = top;
		global::_003CPrivateImplementationDetails_003E.InlineArrayElementRef<InlineArray4<object>, object>(ref buffer, 3) = bottom;
		return string.Format(formatProvider, "RectOffset (l:{0} r:{1} t:{2} b:{3})", global::_003CPrivateImplementationDetails_003E.InlineArrayAsReadOnlySpan<InlineArray4<object>, object>(in buffer, 4));
	}
}
