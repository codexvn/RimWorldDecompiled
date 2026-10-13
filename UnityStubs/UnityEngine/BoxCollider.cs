namespace UnityEngine;

public class BoxCollider : Collider
{
	public Vector3 center { get; set; }

	public Vector3 size { get; set; } = Vector3.one;

	public Vector3 extents
	{
		get
		{
			return size * 0.5f;
		}
		set
		{
			size = value * 2f;
		}
	}

	public override bool Raycast(Ray ray, out RaycastHit hitInfo, float maxDistance)
	{
		hitInfo = default(RaycastHit);
		Matrix4x4 worldToLocalMatrix = base.transform.worldToLocalMatrix;
		Vector3 vector = worldToLocalMatrix.MultiplyPoint3x4(ray.origin) - center;
		Vector3 vector2 = worldToLocalMatrix.MultiplyVector(ray.direction);
		Vector3 vector3 = extents;
		float num = 0f;
		float num2 = maxDistance;
		int num3 = -1;
		for (int i = 0; i < 3; i++)
		{
			float num4 = Mathf.Abs(vector3[i]);
			if (Mathf.Abs(vector2[i]) < 1E-08f)
			{
				if (vector[i] < 0f - num4 || vector[i] > num4)
				{
					return false;
				}
				continue;
			}
			float num5 = (0f - num4 - vector[i]) / vector2[i];
			float num6 = (num4 - vector[i]) / vector2[i];
			if (num5 > num6)
			{
				float num7 = num6;
				num6 = num5;
				num5 = num7;
			}
			if (num5 > num)
			{
				num = num5;
				num3 = i;
			}
			num2 = Mathf.Min(num2, num6);
			if (num > num2)
			{
				return false;
			}
		}
		if (num3 < 0)
		{
			return false;
		}
		Vector3 zero = Vector3.zero;
		zero[num3] = 0f - Mathf.Sign(vector2[num3]);
		return LocalHit(ray, num, zero, out hitInfo);
	}
}
