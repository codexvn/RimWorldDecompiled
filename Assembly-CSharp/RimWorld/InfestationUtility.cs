using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DAD RID: 7597
	public static class InfestationUtility
	{
		// Token: 0x0600A9CD RID: 43469 RVA: 0x00309688 File Offset: 0x00307888
		private static IntVec3 FindRootTunnelLoc(Map map, bool spawnAnywhereIfNoGoodCell = false, bool ignoreRoofIfNoGoodCell = false)
		{
			IntVec3 result;
			if (InfestationCellFinder.TryFindCell(out result, map))
			{
				return result;
			}
			if (!spawnAnywhereIfNoGoodCell)
			{
				return IntVec3.Invalid;
			}
			Func<IntVec3, bool, bool> validator = delegate(IntVec3 x, bool canIgnoreRoof)
			{
				if (!x.Standable(map) || x.Fogged(map))
				{
					return false;
				}
				if (!canIgnoreRoof)
				{
					bool flag = false;
					int num = GenRadial.NumCellsInRadius(3f);
					for (int i = 0; i < num; i++)
					{
						IntVec3 c = x + GenRadial.RadialPattern[i];
						if (c.InBounds(map))
						{
							RoofDef roof = c.GetRoof(map);
							if (roof != null && roof.isThickRoof)
							{
								flag = true;
								break;
							}
						}
					}
					if (!flag)
					{
						return false;
					}
				}
				return true;
			};
			if (RCellFinder.TryFindRandomCellNearTheCenterOfTheMapWith((IntVec3 x) => validator(x, false), map, out result))
			{
				return result;
			}
			if (ignoreRoofIfNoGoodCell && RCellFinder.TryFindRandomCellNearTheCenterOfTheMapWith((IntVec3 x) => validator(x, true), map, out result))
			{
				return result;
			}
			return IntVec3.Invalid;
		}

		// Token: 0x0600A9CE RID: 43470 RVA: 0x00309710 File Offset: 0x00307910
		public static Thing SpawnTunnels(int hiveCount, Map map, bool spawnAnywhereIfNoGoodCell = false, bool ignoreRoofedRequirement = false, string questTag = null, IntVec3? overrideLoc = null, float? insectsPoints = null)
		{
			IntVec3 loc = (overrideLoc != null) ? overrideLoc.Value : default(IntVec3);
			if (overrideLoc == null)
			{
				loc = InfestationUtility.FindRootTunnelLoc(map, spawnAnywhereIfNoGoodCell, false);
			}
			if (!loc.IsValid)
			{
				return null;
			}
			TunnelHiveSpawner tunnelHiveSpawner = (TunnelHiveSpawner)ThingMaker.MakeThing(ThingDefOf.TunnelHiveSpawner, null);
			Thing thing = GenSpawn.Spawn(tunnelHiveSpawner, loc, map, WipeMode.FullRefund);
			if (insectsPoints != null)
			{
				tunnelHiveSpawner.insectsPoints = insectsPoints.Value;
			}
			QuestUtility.AddQuestTag(thing, questTag);
			for (int i = 0; i < hiveCount - 1; i++)
			{
				loc = CompSpawnerHives.FindChildHiveLocation(thing.Position, map, ThingDefOf.Hive, ThingDefOf.Hive.GetCompProperties<CompProperties_SpawnerHives>(), ignoreRoofedRequirement, true);
				if (loc.IsValid)
				{
					tunnelHiveSpawner = (TunnelHiveSpawner)ThingMaker.MakeThing(ThingDefOf.TunnelHiveSpawner, null);
					thing = GenSpawn.Spawn(tunnelHiveSpawner, loc, map, WipeMode.FullRefund);
					if (insectsPoints != null)
					{
						tunnelHiveSpawner.insectsPoints = insectsPoints.Value;
					}
					QuestUtility.AddQuestTag(thing, questTag);
				}
			}
			return thing;
		}

		// Token: 0x0600A9CF RID: 43471 RVA: 0x00309804 File Offset: 0x00307A04
		public static Thing SpawnJellyTunnels(int tunnelCount, int jellyCount, Map map)
		{
			IntVec3 loc = InfestationUtility.FindRootTunnelLoc(map, true, true);
			if (!loc.IsValid)
			{
				return null;
			}
			TunnelJellySpawner tunnelJellySpawner = (TunnelJellySpawner)ThingMaker.MakeThing(ThingDefOf.TunnelJellySpawner, null);
			tunnelJellySpawner.jellyCount = jellyCount / tunnelCount;
			int num = jellyCount - tunnelJellySpawner.jellyCount;
			GenSpawn.Spawn(tunnelJellySpawner, loc, map, WipeMode.FullRefund);
			for (int i = 0; i < tunnelCount - 1; i++)
			{
				loc = CompSpawnerHives.FindChildHiveLocation(tunnelJellySpawner.Position, map, ThingDefOf.Hive, ThingDefOf.Hive.GetCompProperties<CompProperties_SpawnerHives>(), true, false);
				if (loc.IsValid)
				{
					tunnelJellySpawner = (TunnelJellySpawner)ThingMaker.MakeThing(ThingDefOf.TunnelJellySpawner, null);
					if (i < tunnelCount - 2)
					{
						tunnelJellySpawner.jellyCount = jellyCount / tunnelCount;
						num -= tunnelJellySpawner.jellyCount;
					}
					else
					{
						tunnelJellySpawner.jellyCount = num;
					}
					GenSpawn.Spawn(tunnelJellySpawner, loc, map, WipeMode.FullRefund);
				}
			}
			return tunnelJellySpawner;
		}
	}
}
