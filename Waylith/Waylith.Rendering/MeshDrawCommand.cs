using System.Numerics;
using Waylith.Graphics.Meshes;

namespace Waylith.Rendering;

public readonly struct MeshDrawCommand
{
	public Mesh Mesh { get; }

	public Material Material { get; }

	public DrawPropertyRange PropertyRange { get; }

	public Matrix4x4 Matrix { get; }

	public int SubmeshIndex { get; }

	public byte PassId { get; }

	public int Layer { get; }

	public InstancedDrawInfo? Instancing { get; }

	public int RenderQueueOverride { get; }

	public InstanceStorageBinding[]? Storage { get; }

	public MeshDrawCommand(Mesh mesh, Material material, DrawPropertyRange propertyRange, Matrix4x4 matrix, int submeshIndex, byte passId = 0, InstancedDrawInfo? instancing = null, int renderQueueOverride = 0, int layer = 0, InstanceStorageBinding[]? storage = null)
	{
		Mesh = mesh;
		Material = material;
		PropertyRange = propertyRange;
		Matrix = matrix;
		SubmeshIndex = submeshIndex;
		PassId = passId;
		Layer = layer;
		Instancing = instancing;
		RenderQueueOverride = renderQueueOverride;
		Storage = storage;
	}
}
