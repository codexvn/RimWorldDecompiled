using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DEE RID: 7662
	public static class CocoonInfestationUtility
	{
		// Token: 0x0600AAB3 RID: 43699 RVA: 0x0030CC7C File Offset: 0x0030AE7C
		public static bool TryFindCocoonSpawnPositionNear(IntVec3 root, Map map, ThingDef cocoon, out IntVec3 result)
		{
			result = CellFinder.FindNoWipeSpawnLocNear(root, map, cocoon, Rot4.North, Mathf.FloorToInt(GenRadial.MaxRadialPatternRadius), (IntVec3 c) => CocoonInfestationUtility.CanSpawnCocoonAt(c, map));
			return result.IsValid;
		}

		// Token: 0x0600AAB4 RID: 43700 RVA: 0x0030CCCC File Offset: 0x0030AECC
		public static bool CanSpawnCocoonAt(IntVec3 c, Map map)
		{
			if (!c.Walkable(map) || c.GetFirstThing(map, ThingDefOf.InsectJelly) != null || c.GetFirstThing(map, ThingDefOf.GlowPod) != null)
			{
				return false;
			}
			foreach (IntVec3 c2 in GenRadial.RadialCellsAround(c, 2f, true))
			{
				if (c2.InBounds(map))
				{
					List<Thing> thingList = c2.GetThingList(map);
					for (int i = 0; i < thingList.Count; i++)
					{
						if (thingList[i] is CocoonSpawner)
						{
							return false;
						}
						if (thingList[i].def.building != null)
						{
							if (thingList[i].def.building.isInsectCocoon)
							{
								return false;
							}
							if (thingList[i].def.passability == Traversability.Impassable)
							{
								return false;
							}
						}
					}
				}
			}
			return true;
		}

		// Token: 0x0600AAB5 RID: 43701 RVA: 0x0030CDC8 File Offset: 0x0030AFC8
		public static IEnumerable<ThingDef> GetCocoonsToSpawn(float points)
		{
			IEnumerable<ThingDef> allCocoons = from t in DefDatabase<ThingDef>.AllDefs
			where t.building != null && t.building.isInsectCocoon
			select t;
			int tries = 0;
			ThingDef cocoonDef;
			while (points > 0f && allCocoons.TryRandomElement(out cocoonDef))
			{
				yield return cocoonDef;
				points -= cocoonDef.building.combatPower;
				int num = tries;
				tries = num + 1;
				if (tries > 1000)
				{
					Log.Warning("Failed to spend all points when selecing cocoons to spawn.");
					break;
				}
				cocoonDef = null;
			}
			yield break;
		}

		// Token: 0x0600AAB6 RID: 43702 RVA: 0x0030CDD8 File Offset: 0x0030AFD8
		public static List<Thing> SpawnCocoonInfestation(IntVec3 root, Map map, float points)
		{
			List<Thing> list = new List<Thing>();
			if (!ModLister.CheckBiotech("Spawn cocoon infestation"))
			{
				return list;
			}
			CocoonInfestationUtility.tmpCocoonsToSpawn.Clear();
			CocoonInfestationUtility.tmpCocoonsToSpawn.AddRange(CocoonInfestationUtility.GetCocoonsToSpawn(points));
			if (CocoonInfestationUtility.tmpCocoonsToSpawn.Count == 0)
			{
				Log.Warning("Failed to find cocoons to spawn with points:" + points.ToString());
				return list;
			}
			int nextCocoonGroupID = Find.UniqueIDsManager.GetNextCocoonGroupID();
			int num = 0;
			IntVec3 loc;
			while (num < CocoonInfestationUtility.tmpCocoonsToSpawn.Count && CocoonInfestationUtility.TryFindCocoonSpawnPositionNear(root, map, CocoonInfestationUtility.tmpCocoonsToSpawn[num], out loc))
			{
				list.Add(GenSpawn.Spawn(CocoonInfestationUtility.MakeCocoon(CocoonInfestationUtility.tmpCocoonsToSpawn[num], nextCocoonGroupID), loc, map, WipeMode.FullRefund));
				num++;
			}
			CocoonInfestationUtility.tmpCocoonsToSpawn.Clear();
			return list;
		}

		// Token: 0x0600AAB7 RID: 43703 RVA: 0x0030CE97 File Offset: 0x0030B097
		private static Thing MakeCocoon(ThingDef cocoonDef, int groupID)
		{
			CocoonSpawner cocoonSpawner = (CocoonSpawner)ThingMaker.MakeThing(ThingDefOf.CocoonSpawner, null);
			cocoonSpawner.cocoon = cocoonDef;
			cocoonSpawner.groupID = groupID;
			return cocoonSpawner;
		}

		// Token: 0x0400759C RID: 30108
		private const float MinDistanceBetweenCocoons = 2f;

		// Token: 0x0400759D RID: 30109
		private static List<ThingDef> tmpCocoonsToSpawn = new List<ThingDef>();
	}
}
