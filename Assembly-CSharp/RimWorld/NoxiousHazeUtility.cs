using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D4A RID: 7498
	public static class NoxiousHazeUtility
	{
		// Token: 0x0600A801 RID: 43009 RVA: 0x00301994 File Offset: 0x002FFB94
		public static bool IsExposedToNoxiousHaze(Thing thing)
		{
			return thing.SpawnedOrAnyParentSpawned && NoxiousHazeUtility.IsExposedToNoxiousHaze(thing, thing.PositionHeld, thing.MapHeld);
		}

		// Token: 0x0600A802 RID: 43010 RVA: 0x003019B4 File Offset: 0x002FFBB4
		public static bool IsExposedToNoxiousHaze(Thing thing, IntVec3 cell, Map map)
		{
			if (!ModsConfig.BiotechActive)
			{
				return false;
			}
			GameCondition activeCondition = map.gameConditionManager.GetActiveCondition(GameConditionDefOf.NoxiousHaze);
			if (activeCondition == null || activeCondition.HiddenByOtherCondition(map))
			{
				return false;
			}
			Pawn pawn = thing as Pawn;
			if (pawn != null && pawn.kindDef.immuneToGameConditionEffects)
			{
				return false;
			}
			if (thing.def.category == ThingCategory.Item)
			{
				return !cell.Roofed(map);
			}
			return !cell.Roofed(map) || cell.GetRoom(map).PsychologicallyOutdoors;
		}

		// Token: 0x0600A803 RID: 43011 RVA: 0x00301A34 File Offset: 0x002FFC34
		public static bool TryGetNoxiousHazeMTB(PlanetTile tile, out float mtb)
		{
			float num = WorldPollutionUtility.CalculateNearbyPollutionScore(tile);
			if (num > 0f && num >= GameConditionDefOf.NoxiousHaze.minNearbyPollution)
			{
				mtb = GameConditionDefOf.NoxiousHaze.mtbOverNearbyPollutionCurve.Evaluate(num);
				return true;
			}
			mtb = 0f;
			return false;
		}
	}
}
