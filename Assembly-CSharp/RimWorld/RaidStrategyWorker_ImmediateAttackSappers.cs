using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001757 RID: 5975
	public class RaidStrategyWorker_ImmediateAttackSappers : RaidStrategyWorker_WithRequiredPawnKinds
	{
		// Token: 0x06008C36 RID: 35894 RVA: 0x0029B94C File Offset: 0x00299B4C
		protected override bool MatchesRequiredPawnKind(PawnKindDef kind)
		{
			return kind.canBeSapper;
		}

		// Token: 0x06008C37 RID: 35895 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override int MinRequiredPawnsForPoints(float pointsTotal, Faction faction = null)
		{
			return 1;
		}

		// Token: 0x06008C38 RID: 35896 RVA: 0x0029B954 File Offset: 0x00299B54
		public override bool CanUsePawn(float pointsTotal, Pawn p, List<Pawn> otherPawns)
		{
			return (otherPawns.Count != 0 || SappersUtility.IsGoodSapper(p) || SappersUtility.IsGoodBackupSapper(p)) && (!p.kindDef.canBeSapper || !SappersUtility.HasBuildingDestroyerWeapon(p) || SappersUtility.IsGoodSapper(p));
		}

		// Token: 0x06008C39 RID: 35897 RVA: 0x0029B990 File Offset: 0x00299B90
		protected override LordJob MakeLordJob(IncidentParms parms, Map map, List<Pawn> pawns, int raidSeed)
		{
			Faction faction = parms.faction;
			bool canTimeoutOrFlee = parms.canTimeoutOrFlee;
			return new LordJob_AssaultColony(faction, parms.canKidnap, canTimeoutOrFlee, true, true, parms.canSteal, false, false);
		}
	}
}
