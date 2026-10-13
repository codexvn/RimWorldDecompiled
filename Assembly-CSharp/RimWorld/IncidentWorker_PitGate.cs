using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DBF RID: 7615
	public class IncidentWorker_PitGate : IncidentWorker
	{
		// Token: 0x0600AA07 RID: 43527 RVA: 0x0030A460 File Offset: 0x00308660
		protected override bool CanFireNowSub(IncidentParms parms)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return false;
			}
			Map map = (Map)parms.target;
			return base.CanFireNowSub(parms) && map.listerThings.ThingsOfDef(ThingDefOf.PitGate).Count == 0 && map.listerThings.ThingsOfDef(ThingDefOf.PitGateSpawner).Count == 0;
		}

		// Token: 0x0600AA08 RID: 43528 RVA: 0x0030A4BC File Offset: 0x003086BC
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			IntVec3 loc;
			if (!LargeBuildingCellFinder.TryFindCell(out loc, map, IncidentWorker_PitGate.PitGateSpawnParms.ForThing(ThingDefOf.PitGate), null, null, false))
			{
				return false;
			}
			BuildingGroundSpawner buildingGroundSpawner = (BuildingGroundSpawner)ThingMaker.MakeThing(ThingDefOf.PitGateSpawner, null);
			PitGate pitGate = buildingGroundSpawner.ThingToSpawn as PitGate;
			pitGate.SetFaction(Faction.OfEntities, null);
			pitGate.pointsMultiplier = parms.pointMultiplier;
			GenSpawn.Spawn(buildingGroundSpawner, loc, map, WipeMode.Vanish);
			base.SendStandardLetter(parms, buildingGroundSpawner, Array.Empty<NamedArgument>());
			return true;
		}

		// Token: 0x0400756A RID: 30058
		public static readonly LargeBuildingSpawnParms PitGateSpawnParms = new LargeBuildingSpawnParms
		{
			ignoreTerrainAffordance = true
		};
	}
}
