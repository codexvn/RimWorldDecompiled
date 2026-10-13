using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001752 RID: 5970
	public class RaidStrategyWorker_StageThenAttack : RaidStrategyWorker
	{
		// Token: 0x06008C1D RID: 35869 RVA: 0x0029B4A8 File Offset: 0x002996A8
		protected override LordJob MakeLordJob(IncidentParms parms, Map map, List<Pawn> pawns, int raidSeed)
		{
			IntVec3 stageLoc = RCellFinder.FindSiegePositionFrom(parms.spawnCenter.IsValid ? parms.spawnCenter : pawns[0].PositionHeld, map, true, false, null, true);
			if (!stageLoc.IsValid)
			{
				stageLoc = pawns[0].PositionHeld;
			}
			return new LordJob_StageThenAttack(parms.faction, stageLoc, raidSeed, true, true, true, null);
		}

		// Token: 0x06008C1E RID: 35870 RVA: 0x0029B511 File Offset: 0x00299711
		public override bool CanUseWith(IncidentParms parms, PawnGroupKindDef groupKind)
		{
			return base.CanUseWith(parms, groupKind) && parms.faction.def.canStageAttacks;
		}

		// Token: 0x06008C1F RID: 35871 RVA: 0x0029B52F File Offset: 0x0029972F
		public override bool CanUsePawnGenOption(float pointsTotal, PawnGenOption g, List<PawnGenOptionWithXenotype> chosenGroups, Faction faction = null)
		{
			return !g.kind.RaceProps.Animal && base.CanUsePawnGenOption(pointsTotal, g, chosenGroups, faction);
		}
	}
}
