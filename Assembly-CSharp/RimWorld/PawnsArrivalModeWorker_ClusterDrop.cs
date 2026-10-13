using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02002391 RID: 9105
	public class PawnsArrivalModeWorker_ClusterDrop : PawnsArrivalModeWorker
	{
		// Token: 0x0600C54A RID: 50506 RVA: 0x000026EA File Offset: 0x000008EA
		public override void Arrive(List<Pawn> pawns, IncidentParms parms)
		{
		}

		// Token: 0x0600C54B RID: 50507 RVA: 0x0039689C File Offset: 0x00394A9C
		public override void TravellingTransportersArrived(List<ActiveTransporterInfo> transporters, Map map)
		{
			IntVec3 near = DropCellFinder.FindRaidDropCenterDistant(map, false, true);
			TransportersArrivalActionUtility.DropTravellingDropPods(transporters, near, map);
		}

		// Token: 0x0600C54C RID: 50508 RVA: 0x003968BC File Offset: 0x00394ABC
		public override bool TryResolveRaidSpawnCenter(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			if (!parms.spawnCenter.IsValid)
			{
				parms.spawnCenter = MechClusterUtility.FindClusterPosition(map, parms.mechClusterSketch, 100, 0.5f);
			}
			parms.spawnRotation = Rot4.Random;
			return true;
		}
	}
}
