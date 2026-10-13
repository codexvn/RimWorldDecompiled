using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001753 RID: 5971
	public class RaidStrategyWorker_Siege : RaidStrategyWorker
	{
		// Token: 0x06008C21 RID: 35873 RVA: 0x0029B550 File Offset: 0x00299750
		protected override LordJob MakeLordJob(IncidentParms parms, Map map, List<Pawn> pawns, int raidSeed)
		{
			IntVec3 siegeSpot = RCellFinder.FindSiegePositionFrom(parms.spawnCenter.IsValid ? parms.spawnCenter : pawns[0].PositionHeld, map, false, true, null, true);
			float num = parms.points * Rand.Range(0.2f, 0.3f);
			if (num < 60f)
			{
				num = 60f;
			}
			return new LordJob_Siege(parms.faction, siegeSpot, num);
		}

		// Token: 0x06008C22 RID: 35874 RVA: 0x0029B5BB File Offset: 0x002997BB
		public override bool CanUseWith(IncidentParms parms, PawnGroupKindDef groupKind)
		{
			return base.CanUseWith(parms, groupKind) && parms.faction.def.canSiege;
		}

		// Token: 0x06008C23 RID: 35875 RVA: 0x0029B52F File Offset: 0x0029972F
		public override bool CanUsePawnGenOption(float pointsTotal, PawnGenOption g, List<PawnGenOptionWithXenotype> chosenGroups, Faction faction = null)
		{
			return !g.kind.RaceProps.Animal && base.CanUsePawnGenOption(pointsTotal, g, chosenGroups, faction);
		}
	}
}
