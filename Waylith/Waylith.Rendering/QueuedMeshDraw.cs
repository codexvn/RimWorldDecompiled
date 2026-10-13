using System.Numerics;
using Waylith.Graphics.Meshes;

namespace Waylith.Rendering;

public readonly struct QueuedMeshDraw
{
	public Mesh Mesh { get; }

	public Material Material { get; }

	public DrawPropertyRange PropertyRange { get; }

	public Matrix4x4 Matrix { get; }

	public int Layer { get; }

	public int SubmeshIndex { get; }

	public int RenderQueueOverride { get; }

	public InstanceStorageBinding[]? Storage { get; }

	public QueuedMeshDraw(Mesh mesh, Material material, DrawPropertyRange propertyRange, Matrix4x4 matrix, int layer, int submeshIndex, int renderQueueOverride = 0, InstanceStorageBinding[]? storage = null)
	{
		Mesh = mesh;
		Material = material;
		PropertyRange = propertyRange;
		Matrix = matrix;
		Layer = layer;
		SubmeshIndex = submeshIndex;
		RenderQueueOverride = renderQueueOverride;
		Storage = storage;
	}
}
