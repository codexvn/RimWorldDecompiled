using System;

namespace UnityEngine;

public class Physics
{
	private static Collider[] _colliders = Array.Empty<Collider>();

	private static int _collidersFrame = -1;

	public static bool queriesHitTriggers { get; set; } = true;

	public static void BakeMesh(int meshID, bool convex)
	{
		BakeMesh(meshID, convex, MeshColliderCookingOptions.CookForFasterSimulation | MeshColliderCookingOptions.EnableMeshCleaning | MeshColliderCookingOptions.WeldColocatedVertices | MeshColliderCookingOptions.UseFastMidphase);
	}

	public static void BakeMesh(int meshID, bool convex, MeshColliderCookingOptions cookingOptions)
	{
	}

	public static void SyncTransforms()
	{
	}

	private static Collider[] Colliders()
	{
		if (_collidersFrame != Time.frameCount)
		{
			_colliders = Object.FindObjectsByType<Collider>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
			_collidersFrame = Time.frameCount;
		}
		return _colliders;
	}

	private static bool Queryable(Collider collider, int layerMask, QueryTriggerInteraction queryTriggerInteraction)
	{
		if (collider == null || !collider.enabled)
		{
			return false;
		}
		if (collider.isTrigger && (queryTriggerInteraction == QueryTriggerInteraction.Ignore || (queryTriggerInteraction == QueryTriggerInteraction.UseGlobal && !queriesHitTriggers)))
		{
			return false;
		}
		GameObject gameObject = collider.gameObject;
		if (gameObject != null)
		{
			return (layerMask & (1 << gameObject.layer)) != 0;
		}
		return false;
	}

	public static bool Raycast(Ray ray, out RaycastHit hitInfo, float maxDistance, int layerMask)
	{
		return Raycast(ray, out hitInfo, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal);
	}

	public static bool Raycast(Ray ray, out RaycastHit hitInfo, float maxDistance, int layerMask, QueryTriggerInteraction queryTriggerInteraction)
	{
		hitInfo = default(RaycastHit);
		bool result = false;
		float maxDistance2 = maxDistance;
		Collider[] array = Colliders();
		foreach (Collider collider in array)
		{
			if (Queryable(collider, layerMask, queryTriggerInteraction) && collider.Raycast(ray, out var hitInfo2, maxDistance2))
			{
				hitInfo = hitInfo2;
				maxDistance2 = hitInfo2.distance;
				result = true;
			}
		}
		return result;
	}

	public static int RaycastNonAlloc(Ray ray, RaycastHit[] results, float maxDistance, int layerMask, QueryTriggerInteraction queryTriggerInteraction)
	{
		int num = 0;
		Collider[] array = Colliders();
		foreach (Collider collider in array)
		{
			if (num >= results.Length)
			{
				break;
			}
			if (Queryable(collider, layerMask, queryTriggerInteraction) && collider.Raycast(ray, out var hitInfo, maxDistance))
			{
				results[num++] = hitInfo;
			}
		}
		return num;
	}
}
