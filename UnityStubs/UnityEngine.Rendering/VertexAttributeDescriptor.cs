using System;

namespace UnityEngine.Rendering;

public struct VertexAttributeDescriptor : IEquatable<VertexAttributeDescriptor>
{
	private VertexAttribute _attribute;

	private VertexAttributeFormat _format;

	private int _dimension;

	private int _stream;

	public VertexAttribute attribute => _attribute;

	public int dimension => _dimension;

	public VertexAttributeFormat format => _format;

	public int stream => _stream;

	public VertexAttributeDescriptor(VertexAttribute attribute = VertexAttribute.Position, VertexAttributeFormat format = VertexAttributeFormat.Float32, int dimension = 3, int stream = 0)
	{
		_attribute = attribute;
		_format = format;
		_dimension = dimension;
		_stream = stream;
	}

	public bool Equals(VertexAttributeDescriptor other)
	{
		if (_attribute == other._attribute && _format == other._format && _dimension == other._dimension)
		{
			return _stream == other._stream;
		}
		return false;
	}
}
