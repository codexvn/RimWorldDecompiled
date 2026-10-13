namespace Waylith.Graphics.Meshes;

public interface IGpuMeshResourceOwner
{
	void Release(GpuMeshResource resource);

	void Rename(GpuMeshResource resource, string debugName);
}
