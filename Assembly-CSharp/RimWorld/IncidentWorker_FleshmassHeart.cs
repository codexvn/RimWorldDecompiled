using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D9C RID: 7580
	public class IncidentWorker_FleshmassHeart : IncidentWorker
	{
		// Token: 0x0600A98F RID: 43407 RVA: 0x003087C0 File Offset: 0x003069C0
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			return ModsConfig.AnomalyActive && base.CanFireNowSub(parms);
		}

		// Token: 0x0600A990 RID: 43408 RVA: 0x003087D4 File Offset: 0x003069D4
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 loc;
			if (!LargeBuildingCellFinder.TryFindCell(out loc, map, IncidentWorker_FleshmassHeart.HeartSpawnParms.ForThing(ThingDefOf.FleshmassHeart), null, null, false))
			{
				return false;
			}
			BuildingGroundSpawner buildingGroundSpawner = (BuildingGroundSpawner)ThingMaker.MakeThing(ThingDefOf.FleshmassHeartSpawner, null);
			Building_FleshmassHeart building_FleshmassHeart = buildingGroundSpawner.ThingToSpawn as Building_FleshmassHeart;
			building_FleshmassHeart.Comp.threatPoints = parms.points;
			building_FleshmassHeart.SetFaction(Faction.OfEntities, null);
			GenSpawn.Spawn(buildingGroundSpawner, loc, map, WipeMode.Vanish);
			base.SendStandardLetter(parms, buildingGroundSpawner, Array.Empty<NamedArgument>());
			return true;
		}

		// Token: 0x04007545 RID: 30021
		public static readonly LargeBuildingSpawnParms HeartSpawnParms = new LargeBuildingSpawnParms
		{
			minDistToEdge = 10,
			minDistanceToColonyBuilding = 20f,
			preferFarFromColony = true
		};
	}
}
