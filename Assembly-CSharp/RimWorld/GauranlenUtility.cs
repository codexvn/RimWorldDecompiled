using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002EB0 RID: 11952
	public static class GauranlenUtility
	{
		// Token: 0x06010DB3 RID: 69043 RVA: 0x004DAB44 File Offset: 0x004D8D44
		public static bool CocoonAndPodCellValidator(IntVec3 c, Map map, ThingDef thingDefToSpawn = null)
		{
			Thing thing;
			if (((thingDefToSpawn != null) ? thingDefToSpawn.plant : null) != null && thingDefToSpawn.CanEverPlantAt(c, map, out thing, false, true, false).Accepted)
			{
				return false;
			}
			if (!c.InBounds(map) || !c.Standable(map))
			{
				return false;
			}
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].def.category == ThingCategory.Building || thingList[i].def.IsFrame)
				{
					return false;
				}
				if (thingList[i].def == ThingDefOf.Plant_TreeGauranlen || thingList[i].def == ThingDefOf.GaumakerCocoon || thingList[i].def == ThingDefOf.Plant_PodGauranlen)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06010DB4 RID: 69044 RVA: 0x004DAC0A File Offset: 0x004D8E0A
		public static List<Thing> BuildingsAffectingConnectionStrengthAt(IntVec3 pos, Map map, CompProperties_TreeConnection props)
		{
			return map.listerArtificialBuildingsForMeditation.GetForCell(pos, props.radiusToBuildingForConnectionStrengthLoss);
		}

		// Token: 0x06010DB5 RID: 69045 RVA: 0x004DAC1E File Offset: 0x004D8E1E
		public static IEnumerable<Thing> GetConnectionsAffectedByBuilding(Map map, ThingDef def, Faction faction, IntVec3 pos, Rot4 rotation)
		{
			if (!MeditationUtility.CountsAsArtificialBuilding(def, faction))
			{
				yield break;
			}
			foreach (Thing thing in map.listerThings.ThingsMatching(ThingRequest.ForGroup(ThingRequestGroup.DryadSpawner)))
			{
				CompTreeConnection compTreeConnection = thing.TryGetComp<CompTreeConnection>();
				if (compTreeConnection != null && compTreeConnection.WillBeAffectedBy(def, faction, pos, rotation))
				{
					yield return thing;
				}
			}
			List<Thing>.Enumerator enumerator = default(List<Thing>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06010DB6 RID: 69046 RVA: 0x004DAC4C File Offset: 0x004D8E4C
		public static void DrawConnectionsAffectedByBuildingOverlay(Map map, ThingDef def, Faction faction, IntVec3 pos, Rot4 rotation)
		{
			CompProperties_TreeConnection compProperties_TreeConnection = (CompProperties_TreeConnection)def.CompDefFor<CompTreeConnection>();
			int num = 0;
			foreach (Thing t in GauranlenUtility.GetConnectionsAffectedByBuilding(map, def, faction, pos, rotation))
			{
				if (num++ > 10)
				{
					break;
				}
				GenAdj.OccupiedRect(pos, rotation, def.size);
				GenDraw.DrawLineBetween(GenThing.TrueCenter(pos, rotation, def.size, def.Altitude), t.TrueCenter(), SimpleColor.Red, 0.2f);
			}
		}
	}
}
