using System.Numerics;

namespace Waylith.Graphics.Meshes;

public struct MeshRaycastHit
{
	public Vector3 point;

	public Vector3 normal;

	public float distance;

	public int triangleIndex;
}
