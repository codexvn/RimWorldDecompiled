using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02002392 RID: 9106
	public class PawnsArrivalModeWorker_EdgeDrop : PawnsArrivalModeWorker
	{
		// Token: 0x0600C54E RID: 50510 RVA: 0x00396756 File Offset: 0x00394956
		public override void Arrive(List<Pawn> pawns, IncidentParms parms)
		{
			PawnsArrivalModeWorkerUtility.DropInDropPodsNearSpawnCenter(parms, pawns);
		}

		// Token: 0x0600C54F RID: 50511 RVA: 0x00396908 File Offset: 0x00394B08
		public override void TravellingTransportersArrived(List<ActiveTransporterInfo> transporters, Map map)
		{
			IntVec3 near = DropCellFinder.FindRaidDropCenterDistant(map, false, !transporters.IsShuttle());
			if (transporters.IsShuttle())
			{
				TransportersArrivalActionUtility.DropShuttle(transporters[0], map, near, null, null);
				return;
			}
			TransportersArrivalActionUtility.DropTravellingDropPods(transporters, near, map);
		}

		// Token: 0x0600C550 RID: 50512 RVA: 0x00396950 File Offset: 0x00394B50
		public override bool TryResolveRaidSpawnCenter(IncidentParms parms)
		{
			Map map = (Map)parms.target;
			if (!parms.spawnCenter.IsValid)
			{
				parms.spawnCenter = DropCellFinder.FindRaidDropCenterDistant(map, false, true);
			}
			parms.spawnRotation = Rot4.Random;
			return true;
		}
	}
}
