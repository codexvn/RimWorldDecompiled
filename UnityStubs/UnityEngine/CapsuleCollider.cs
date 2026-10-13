namespace UnityEngine;

public class CapsuleCollider : Collider
{
	public Vector3 center { get; set; }

	public float radius { get; set; } = 0.5f;

	public float height { get; set; } = 2f;

	public int direction { get; set; } = 1;

	public override bool Raycast(Ray ray, out RaycastHit hitInfo, float maxDistance)
	{
		hitInfo = default(RaycastHit);
		Matrix4x4 worldToLocalMatrix = base.transform.worldToLocalMatrix;
		Vector3 vector = worldToLocalMatrix.MultiplyVector(ray.direction);
		float magnitude = vector.magnitude;
		if (magnitude < 1E-08f)
		{
			return false;
		}
		Vector3 ro = worldToLocalMatrix.MultiplyPoint3x4(ray.origin);
		Vector3 rd = vector / magnitude;
		Vector3 zero = Vector3.zero;
		zero[Mathf.Clamp(direction, 0, 2)] = Mathf.Max(0f, height * 0.5f - radius);
		Vector3 vector2 = center - zero;
		Vector3 vector3 = center + zero;
		float best = float.PositiveInfinity;
		Vector3 normal = Vector3.zero;
		IntersectBody(ro, rd, vector2, vector3, radius, ref best, ref normal);
		IntersectSphere(ro, rd, vector2, radius, ref best, ref normal);
		IntersectSphere(ro, rd, vector3, radius, ref best, ref normal);
		float num = best / magnitude;
		if (best < float.PositiveInfinity && num <= maxDistance)
		{
			return LocalHit(ray, num, normal, out hitInfo);
		}
		return false;
	}

	private static void IntersectBody(Vector3 ro, Vector3 rd, Vector3 pa, Vector3 pb, float r, ref float best, ref Vector3 normal)
	{
		Vector3 vector = pb - pa;
		Vector3 vector2 = ro - pa;
		float num = Vector3.Dot(vector, vector);
		float num2 = Vector3.Dot(vector, rd);
		float num3 = Vector3.Dot(vector, vector2);
		float num4 = num - num2 * num2;
		if (num <= 0f || num4 < 1E-08f)
		{
			return;
		}
		float num5 = num * Vector3.Dot(rd, vector2) - num3 * num2;
		float num6 = num * Vector3.Dot(vector2, vector2) - num3 * num3 - r * r * num;
		float num7 = num5 * num5 - num4 * num6;
		if (!(num7 < 0f))
		{
			float num8 = (0f - num5 - Mathf.Sqrt(num7)) / num4;
			float num9 = num3 + num8 * num2;
			if (!(num8 < 0f) && !(num8 >= best) && !(num9 <= 0f) && !(num9 >= num))
			{
				best = num8;
				normal = ro + rd * num8 - (pa + vector * (num9 / num));
			}
		}
	}

	private static void IntersectSphere(Vector3 ro, Vector3 rd, Vector3 centre, float r, ref float best, ref Vector3 normal)
	{
		Vector3 vector = ro - centre;
		float num = Vector3.Dot(vector, rd);
		float num2 = num * num - (Vector3.Dot(vector, vector) - r * r);
		if (!(num2 < 0f))
		{
			float num3 = 0f - num - Mathf.Sqrt(num2);
			if (!(num3 < 0f) && !(num3 >= best))
			{
				best = num3;
				normal = ro + rd * num3 - centre;
			}
		}
	}
}
