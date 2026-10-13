namespace UnityEngine.Rendering;

public struct SubMeshDescriptor
{
	public int baseVertex;

	public int indexCount;

	public int indexStart;

	public MeshTopology topology;

	public Bounds bounds { get; set; }

	public int vertexCount { get; set; }

	public SubMeshDescriptor(int indexStart, int indexCount, MeshTopology topology = MeshTopology.Triangles)
	{
		this.indexStart = indexStart;
		this.indexCount = indexCount;
		this.topology = topology;
		baseVertex = 0;
		bounds = default(Bounds);
		vertexCount = 0;
	}
}
