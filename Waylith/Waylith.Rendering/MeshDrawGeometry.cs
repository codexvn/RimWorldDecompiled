using Brutal.VulkanApi;
using Waylith.Graphics.Meshes;

namespace Waylith.Rendering;

public class MeshDrawGeometry(int MeshId, int SubmeshIndex, GpuMeshBuffer[] VertexStreams, string DebugName, VertexLayout Layout, GpuMeshBuffer? IndexStream, VkIndexType IndexType, int IndexCount, int FirstIndex, int BaseVertex, MeshTopology Topology, MeshBounds Bounds)
{
	public VkBuffer IndexBuffer => IndexStream?.Buffer ?? default(VkBuffer);

	public int MeshId { get; init; } = MeshId;

	public int SubmeshIndex { get; init; } = SubmeshIndex;

	public GpuMeshBuffer[] VertexStreams { get; init; } = VertexStreams;

	public string DebugName { get; init; } = DebugName;

	public VertexLayout Layout { get; init; } = Layout;

	public GpuMeshBuffer? IndexStream { get; init; } = IndexStream;

	public VkIndexType IndexType { get; init; } = IndexType;

	public int IndexCount { get; init; } = IndexCount;

	public int FirstIndex { get; init; } = FirstIndex;

	public int BaseVertex { get; init; } = BaseVertex;

	public MeshTopology Topology { get; init; } = Topology;

	public MeshBounds Bounds { get; init; } = Bounds;
}
