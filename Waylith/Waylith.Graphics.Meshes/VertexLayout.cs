using System;
using System.Collections.Generic;

namespace Waylith.Graphics.Meshes;

public sealed class VertexLayout
{
	private readonly struct LayoutIdKey : IEquatable<LayoutIdKey>
	{
		private readonly VertexAttribute[] _attributes;

		private readonly int _streamCount;

		private readonly int _hashCode;

		public LayoutIdKey(VertexAttribute[] attributes, int streamCount, int hashCode)
		{
			_attributes = attributes;
			_streamCount = streamCount;
			_hashCode = hashCode;
		}

		public bool Equals(LayoutIdKey other)
		{
			if (_streamCount != other._streamCount || _attributes.Length != other._attributes.Length)
			{
				return false;
			}
			for (int i = 0; i < _attributes.Length; i++)
			{
				if (!AttributeEquals(_attributes[i], other._attributes[i]))
				{
					return false;
				}
			}
			return true;
		}

		public override bool Equals(object? obj)
		{
			if (obj is LayoutIdKey other)
			{
				return Equals(other);
			}
			return false;
		}

		public override int GetHashCode()
		{
			return _hashCode;
		}

		private static bool AttributeEquals(VertexAttribute left, VertexAttribute right)
		{
			if (left.Semantic == right.Semantic && left.Format == right.Format && left.Dimension == right.Dimension && left.Stream == right.Stream)
			{
				return left.OffsetBytes == right.OffsetBytes;
			}
			return false;
		}
	}

	private static readonly VertexAttribute[] EmptyAttributes = Array.Empty<VertexAttribute>();

	private static readonly int[] EmptyStrides = Array.Empty<int>();

	private static readonly Dictionary<LayoutIdKey, int> LayoutIds = new Dictionary<LayoutIdKey, int>();

	private static readonly object LayoutIdLock = new object();

	private static int NextLayoutId = 1;

	private readonly VertexAttribute[] _attributes;

	private readonly int[] _streamStrides;

	private readonly uint _attributeMask;

	private readonly int _id;

	public static VertexLayout Empty { get; } = new VertexLayout(Array.Empty<VertexAttribute>());

	public ReadOnlySpan<VertexAttribute> Attributes => _attributes;

	public int StreamCount => _streamStrides.Length;

	public int Id => _id;

	public VertexLayout(IReadOnlyList<VertexAttribute> attributes)
	{
		if (attributes == null || attributes.Count == 0)
		{
			_attributes = EmptyAttributes;
			_streamStrides = EmptyStrides;
			_attributeMask = 0u;
			_id = 0;
			return;
		}
		int num = 0;
		for (int i = 0; i < attributes.Count; i++)
		{
			num = System.Math.Max(num, attributes[i].Stream);
		}
		_streamStrides = new int[num + 1];
		_attributes = new VertexAttribute[attributes.Count];
		uint num2 = 0u;
		int num3 = _streamStrides.Length;
		for (int j = 0; j < attributes.Count; j++)
		{
			VertexAttribute vertexAttribute = attributes[j];
			int num4 = ((vertexAttribute.OffsetBytes >= 0) ? vertexAttribute.OffsetBytes : _streamStrides[vertexAttribute.Stream]);
			_attributes[j] = vertexAttribute.WithOffset(num4);
			vertexAttribute = _attributes[j];
			if ((uint)vertexAttribute.Semantic < 32u)
			{
				num2 |= (uint)(1 << (int)vertexAttribute.Semantic);
			}
			num3 = HashCode.Combine(num3, vertexAttribute.Semantic, vertexAttribute.Format, vertexAttribute.Dimension, vertexAttribute.Stream, vertexAttribute.OffsetBytes);
			_streamStrides[vertexAttribute.Stream] = System.Math.Max(_streamStrides[vertexAttribute.Stream], num4 + GetAttributeByteSize(vertexAttribute));
		}
		_attributeMask = num2;
		_id = GetOrCreateLayoutId(_attributes, _streamStrides.Length, num3);
	}

	public int GetStreamStride(int stream)
	{
		if ((uint)stream >= (uint)_streamStrides.Length)
		{
			throw new ArgumentOutOfRangeException("stream");
		}
		return _streamStrides[stream];
	}

	public bool HasAttribute(VertexAttributeSemantic semantic)
	{
		return ContainsSemantic(_attributeMask, semantic);
	}

	private static bool ContainsSemantic(uint attributeMask, VertexAttributeSemantic semantic)
	{
		if ((uint)semantic < 32u)
		{
			return (attributeMask & (uint)(1 << (int)semantic)) != 0;
		}
		return false;
	}

	private static int GetOrCreateLayoutId(VertexAttribute[] attributes, int streamCount, int hashCode)
	{
		LayoutIdKey key = new LayoutIdKey(attributes, streamCount, hashCode);
		lock (LayoutIdLock)
		{
			if (LayoutIds.TryGetValue(key, out var value))
			{
				return value;
			}
			value = NextLayoutId++;
			LayoutIds.Add(key, value);
			return value;
		}
	}

	public bool TryGetAttribute(VertexAttributeSemantic semantic, out VertexAttribute attribute)
	{
		for (int i = 0; i < _attributes.Length; i++)
		{
			if (_attributes[i].Semantic == semantic)
			{
				attribute = _attributes[i];
				return true;
			}
		}
		attribute = default(VertexAttribute);
		return false;
	}

	public static int GetAttributeByteSize(VertexAttribute attribute)
	{
		return attribute.Dimension * GetComponentByteSize(attribute.Format);
	}

	public static int GetComponentByteSize(VertexAttributeFormat format)
	{
		return format switch
		{
			VertexAttributeFormat.Float32 => 4, 
			VertexAttributeFormat.Float16 => 2, 
			VertexAttributeFormat.UNorm8 => 1, 
			VertexAttributeFormat.SNorm8 => 1, 
			VertexAttributeFormat.UInt8 => 1, 
			VertexAttributeFormat.SInt8 => 1, 
			VertexAttributeFormat.UNorm16 => 2, 
			VertexAttributeFormat.SNorm16 => 2, 
			VertexAttributeFormat.UInt16 => 2, 
			VertexAttributeFormat.SInt16 => 2, 
			VertexAttributeFormat.UInt32 => 4, 
			VertexAttributeFormat.SInt32 => 4, 
			_ => throw new ArgumentOutOfRangeException("format", format, null), 
		};
	}
}
