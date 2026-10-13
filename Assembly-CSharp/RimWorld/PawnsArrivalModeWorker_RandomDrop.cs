using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020023A1 RID: 9121
	public class PawnsArrivalModeWorker_RandomDrop : PawnsArrivalModeWorker
	{
		// Token: 0x0600C582 RID: 50562 RVA: 0x003975F0 File Offset: 0x003957F0
		public override void Arrive(List<Pawn> pawns, IncidentParms parms)
		{
			Map map = (Map)parms.target;
			bool flag = parms.faction != null && parms.faction.HostileTo(Faction.OfPlayer);
			for (int i = 0; i < pawns.Count; i++)
			{
				DropPodUtility.DropThingsNear(DropCellFinder.RandomDropSpot(map, true), map, Gen.YieldSingle<Thing>(pawns[i]), parms.podOpenDelay, false, true, parms.canRoofPunch ?? flag, true, true, parms.faction);
			}
		}

		// Token: 0x0600C583 RID: 50563 RVA: 0x00397679 File Offset: 0x00395879
		public override bool TryResolveRaidSpawnCenter(IncidentParms parms)
		{
			if (!parms.raidArrivalModeForQuickMilitaryAid)
			{
				parms.podOpenDelay = 520;
			}
			parms.spawnRotation = Rot4.Random;
			return true;
		}
	}
}
