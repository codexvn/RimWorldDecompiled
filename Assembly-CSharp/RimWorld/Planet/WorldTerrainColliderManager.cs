using System;
using System.Collections.Generic;
using UnityEngine;

namespace RimWorld.Planet
{
	// Token: 0x02003CAD RID: 15533
	public static class WorldTerrainColliderManager
	{
		// Token: 0x060151E8 RID: 86504 RVA: 0x00652434 File Offset: 0x00650634
		public static void ClearCache()
		{
			foreach (KeyValuePair<PlanetLayer, Dictionary<int, GameObject>> keyValuePair in WorldTerrainColliderManager.layerWorldTerrainColliders)
			{
				PlanetLayer planetLayer;
				Dictionary<int, GameObject> dictionary;
				keyValuePair.Deconstruct(out planetLayer, out dictionary);
				foreach (KeyValuePair<int, GameObject> keyValuePair2 in dictionary)
				{
					int num;
					GameObject obj;
					keyValuePair2.Deconstruct(out num, out obj);
					UnityEngine.Object.Destroy(obj);
				}
			}
			WorldTerrainColliderManager.layerWorldTerrainColliders.Clear();
		}

		// Token: 0x060151E9 RID: 86505 RVA: 0x006524E0 File Offset: 0x006506E0
		public static void EnsureRaycastCollidersUpdated()
		{
			foreach (KeyValuePair<PlanetLayer, Dictionary<int, GameObject>> keyValuePair in WorldTerrainColliderManager.layerWorldTerrainColliders)
			{
				PlanetLayer planetLayer;
				Dictionary<int, GameObject> dictionary;
				keyValuePair.Deconstruct(out planetLayer, out dictionary);
				PlanetLayer planetLayer2 = planetLayer;
				foreach (KeyValuePair<int, GameObject> keyValuePair2 in dictionary)
				{
					int num;
					GameObject gameObject;
					keyValuePair2.Deconstruct(out num, out gameObject);
					gameObject.SetActive(planetLayer2.Raycastable);
				}
			}
		}

		// Token: 0x060151EA RID: 86506 RVA: 0x0065258C File Offset: 0x0065078C
		private static GameObject CreateGameObject(PlanetLayer planetLayer, int layer)
		{
			GameObject gameObject = new GameObject(string.Format("{0} WorldTerrainCollider layer {1}", planetLayer, layer));
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			gameObject.layer = layer;
			return gameObject;
		}

		// Token: 0x060151EB RID: 86507 RVA: 0x006525B4 File Offset: 0x006507B4
		public static GameObject Get(PlanetLayer planetLayer, int layer)
		{
			Dictionary<int, GameObject> dictionary;
			if (!WorldTerrainColliderManager.layerWorldTerrainColliders.TryGetValue(planetLayer, out dictionary))
			{
				dictionary = (WorldTerrainColliderManager.layerWorldTerrainColliders[planetLayer] = new Dictionary<int, GameObject>());
			}
			GameObject gameObject;
			if (!dictionary.TryGetValue(layer, out gameObject))
			{
				gameObject = (dictionary[layer] = WorldTerrainColliderManager.CreateGameObject(planetLayer, layer));
			}
			gameObject.SetActive(false);
			return gameObject;
		}

		// Token: 0x0400EB9A RID: 60314
		private static readonly Dictionary<PlanetLayer, Dictionary<int, GameObject>> layerWorldTerrainColliders = new Dictionary<PlanetLayer, Dictionary<int, GameObject>>();
	}
}
