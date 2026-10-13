namespace UnityEngine;

public class Collider : Component
{
	public bool enabled { get; set; } = true;

	public bool isTrigger { get; set; }

	public virtual bool Raycast(Ray ray, out RaycastHit hitInfo, float maxDistance)
	{
		hitInfo = default(RaycastHit);
		return false;
	}

	private protected bool LocalHit(Ray ray, float t, Vector3 localNormal, out RaycastHit hitInfo)
	{
		hitInfo = new RaycastHit
		{
			collider = this,
			distance = t,
			point = ray.origin + ray.direction * t,
			normal = base.transform.worldToLocalMatrix.transpose.MultiplyVector(localNormal).normalized
		};
		return true;
	}
}
