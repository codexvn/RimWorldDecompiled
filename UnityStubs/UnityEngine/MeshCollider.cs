namespace UnityEngine;

public class MeshCollider : Collider
{
	public bool convex { get; set; }

	public Mesh sharedMesh { get; set; }

	public override bool Raycast(Ray ray, out RaycastHit hitInfo, float maxDistance)
	{
		Matrix4x4 worldToLocal = base.transform.worldToLocalMatrix;
		return MeshRaycastUtility.Raycast(ray, sharedMesh, in worldToLocal, maxDistance, this, out hitInfo);
	}
}
