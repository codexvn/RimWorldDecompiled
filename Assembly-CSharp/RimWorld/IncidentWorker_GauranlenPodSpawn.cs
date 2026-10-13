using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D9F RID: 7583
	public class IncidentWorker_GauranlenPodSpawn : IncidentWorker
	{
		// Token: 0x0600A99A RID: 43418 RVA: 0x00308A99 File Offset: 0x00306C99
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			return base.CanFireNowSub(parms) && ModsConfig.IdeologyActive;
		}

		// Token: 0x0600A99B RID: 43419 RVA: 0x00308AAC File Offset: 0x00306CAC
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 cell;
			if (!IncidentWorker_GauranlenPodSpawn.TryFindRootCell(map, out cell))
			{
				return false;
			}
			Thing thing;
			if (!this.TrySpawnAt(cell, map, out thing))
			{
				return false;
			}
			((Plant)thing).Growth = 1f;
			base.SendStandardLetter(parms, thing, Array.Empty<NamedArgument>());
			return true;
		}

		// Token: 0x0600A99C RID: 43420 RVA: 0x00308B04 File Offset: 0x00306D04
		public static bool IsGoodBiome(BiomeDef biomeDef)
		{
			IncidentDef gauranlenPodSpawn = IncidentDefOf.GauranlenPodSpawn;
			return (gauranlenPodSpawn.disallowedBiomes == null || !gauranlenPodSpawn.disallowedBiomes.Contains(biomeDef)) && (gauranlenPodSpawn.allowedBiomes == null || gauranlenPodSpawn.allowedBiomes.Contains(biomeDef));
		}

		// Token: 0x0600A99D RID: 43421 RVA: 0x00308B48 File Offset: 0x00306D48
		private static bool CanSpawnPodAt(IntVec3 c, Map map)
		{
			if (!c.Standable(map) || c.Fogged(map) || !c.GetRoom(map).PsychologicallyOutdoors || c.Roofed(map))
			{
				return false;
			}
			Plant plant = c.GetPlant(map);
			if (plant != null && plant.def.plant.growDays > 10f)
			{
				return false;
			}
			List<Thing> thingList = c.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (thingList[i].def == ThingDefOf.Plant_PodGauranlen)
				{
					return false;
				}
			}
			return map.reachability.CanReachFactionBase(c, map.ParentFaction) && !c.GetTerrain(map).avoidWander && c.GetFertility(map) >= ThingDefOf.Plant_PodGauranlen.plant.fertilityMin;
		}

		// Token: 0x0600A99E RID: 43422 RVA: 0x00308C14 File Offset: 0x00306E14
		public static bool TryFindRootCell(Map map, out IntVec3 cell)
		{
			return CellFinderLoose.TryFindRandomNotEdgeCellWith(10, (IntVec3 x) => IncidentWorker_GauranlenPodSpawn.CanSpawnPodAt(x, map), map, out cell);
		}

		// Token: 0x0600A99F RID: 43423 RVA: 0x00308C48 File Offset: 0x00306E48
		private bool TrySpawnAt(IntVec3 cell, Map map, out Thing plant)
		{
			Plant plant2 = cell.GetPlant(map);
			if (plant2 != null)
			{
				plant2.Destroy(DestroyMode.Vanish);
			}
			plant = GenSpawn.Spawn(ThingDefOf.Plant_PodGauranlen, cell, map, WipeMode.Vanish);
			return plant != null;
		}
	}
}
