using System.Numerics;
using Waylith.Graphics.Meshes;

namespace Waylith.Rendering;

public sealed class ShaderVertexInputSlot
{
	public int location;

	public VertexAttributeSemantic semantic;

	public bool required;

	public int defaultOffsetBytes;

	public Vector4 defaultValue;
}
