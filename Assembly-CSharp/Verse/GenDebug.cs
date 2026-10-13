using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000CF1 RID: 3313
	public static class GenDebug
	{
		// Token: 0x06005489 RID: 21641 RVA: 0x001B37E0 File Offset: 0x001B19E0
		public static void DebugPlaceSphere(Vector3 Loc, float Scale)
		{
			GameObject gameObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
			gameObject.transform.position = Loc;
			gameObject.transform.localScale = new Vector3(Scale, Scale, Scale);
		}

		// Token: 0x0600548A RID: 21642 RVA: 0x001B3808 File Offset: 0x001B1A08
		public static void LogList<T>(IEnumerable<T> list)
		{
			foreach (T t in list)
			{
				Log.Message("    " + t.ToString());
			}
		}

		// Token: 0x0600548B RID: 21643 RVA: 0x001B3868 File Offset: 0x001B1A68
		public static void ClearArea(CellRect r, Map map)
		{
			r.ClipInsideMap(map);
			foreach (IntVec3 c in r)
			{
				map.roofGrid.SetRoof(c, null);
			}
			foreach (IntVec3 c2 in r)
			{
				foreach (Thing thing in c2.GetThingList(map).ToList<Thing>())
				{
					if (thing.def.destroyable)
					{
						thing.Destroy(DestroyMode.Vanish);
					}
				}
			}
		}

		// Token: 0x0600548C RID: 21644 RVA: 0x001B3950 File Offset: 0x001B1B50
		public static void SpawnArea(CellRect r, Map map, ThingDef def)
		{
			foreach (IntVec3 loc in r)
			{
				GenSpawn.Spawn(def, loc, map, WipeMode.Vanish);
			}
		}
	}
}
