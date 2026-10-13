using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200174E RID: 5966
	public class RaidStrategyWorker_ImmediateAttackSmart : RaidStrategyWorker
	{
		// Token: 0x06008C12 RID: 35858 RVA: 0x0029B364 File Offset: 0x00299564
		protected override LordJob MakeLordJob(IncidentParms parms, Map map, List<Pawn> pawns, int raidSeed)
		{
			Faction faction = parms.faction;
			bool canTimeoutOrFlee = parms.canTimeoutOrFlee;
			return new LordJob_AssaultColony(faction, parms.canKidnap, canTimeoutOrFlee, false, true, parms.canSteal, false, false);
		}

		// Token: 0x06008C13 RID: 35859 RVA: 0x0029B394 File Offset: 0x00299594
		public override bool CanUseWith(IncidentParms parms, PawnGroupKindDef groupKind)
		{
			return base.CanUseWith(parms, groupKind) && parms.faction.def.canUseAvoidGrid;
		}
	}
}
