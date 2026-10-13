namespace Waylith.Graphics.Meshes;

public readonly struct VertexAttribute
{
	public readonly VertexAttributeSemantic Semantic;

	public readonly VertexAttributeFormat Format;

	public readonly int Dimension;

	public readonly int Stream;

	public readonly int OffsetBytes;

	public VertexAttribute(VertexAttributeSemantic semantic, VertexAttributeFormat format, int dimension, int stream = 0, int offsetBytes = -1)
	{
		Semantic = semantic;
		Format = format;
		Dimension = dimension;
		Stream = stream;
		OffsetBytes = offsetBytes;
	}

	internal VertexAttribute WithOffset(int offsetBytes)
	{
		return new VertexAttribute(Semantic, Format, Dimension, Stream, offsetBytes);
	}
}
