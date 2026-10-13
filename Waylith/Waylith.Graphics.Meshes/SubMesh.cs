namespace Waylith.Graphics.Meshes;

public readonly struct SubMesh
{
	public readonly int IndexStart;

	public readonly int IndexCount;

	public readonly int BaseVertex;

	public readonly MeshTopology Topology;

	public readonly MeshBounds Bounds;

	public SubMesh(int indexStart, int indexCount, MeshTopology topology = MeshTopology.Triangles, int baseVertex = 0, MeshBounds bounds = default(MeshBounds))
	{
		IndexStart = indexStart;
		IndexCount = indexCount;
		BaseVertex = baseVertex;
		Topology = topology;
		Bounds = bounds;
	}
}
