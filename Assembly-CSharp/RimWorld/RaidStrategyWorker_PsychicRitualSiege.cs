using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001755 RID: 5973
	public class RaidStrategyWorker_PsychicRitualSiege : RaidStrategyWorker_WithRequiredPawnKinds
	{
		// Token: 0x06008C2B RID: 35883 RVA: 0x0029B668 File Offset: 0x00299868
		public override void MakeLords(IncidentParms parms, List<Pawn> pawns)
		{
			RaidStrategyWorker_PsychicRitualSiege.<>c__DisplayClass1_0 CS$<>8__locals1 = new RaidStrategyWorker_PsychicRitualSiege.<>c__DisplayClass1_0();
			CS$<>8__locals1.map = (Map)parms.target;
			Pawn pawn = this.BestInvoker(pawns);
			if (AnomalyIncidentUtility.IncidentShardChance(parms.points))
			{
				AnomalyIncidentUtility.PawnShardOnDeath(pawn);
			}
			parms.spawnCenter = RCellFinder.FindSiegePositionFrom(parms.spawnCenter.IsValid ? parms.spawnCenter : pawn.PositionHeld, CS$<>8__locals1.map, true, true, new Func<IntVec3, bool>(CS$<>8__locals1.<MakeLords>g__Validator|0), false);
			Lord lord = LordMaker.MakeNewLord(parms.faction, this.MakeLordJob(parms, CS$<>8__locals1.map, pawns, Rand.Int), CS$<>8__locals1.map, pawns);
			lord.inSignalLeave = parms.inSignalEnd;
			QuestUtility.AddQuestTag(lord, parms.questTag);
			pawn.health.AddHediff(HediffDefOf.PsychicTrance, null, null, null);
		}

		// Token: 0x06008C2C RID: 35884 RVA: 0x0029B73C File Offset: 0x0029993C
		protected override LordJob MakeLordJob(IncidentParms parms, Map map, List<Pawn> pawns, int raidSeed)
		{
			Pawn pawn = this.BestInvoker(pawns);
			PsychicRitualRoleAssignments psychicRitualRoleAssignments = parms.psychicRitualDef.BuildRoleAssignments(new TargetInfo(parms.spawnCenter, map, false));
			psychicRitualRoleAssignments.AddForcedRole(pawn, PsychicRitualRoleDefOf.Invoker, PsychicRitualRoleDef.Context.NonPlayerFaction);
			for (int i = 0; i < pawns.Count; i++)
			{
				if (pawns[i] != pawn)
				{
					psychicRitualRoleAssignments.AddForcedRole(pawns[i], PsychicRitualRoleDefOf.Defender, PsychicRitualRoleDef.Context.NonPlayerFaction);
				}
			}
			return new LordJob_PsychicRitualRepeating(parms.psychicRitualDef, psychicRitualRoleAssignments, RaidStrategyWorker_PsychicRitualSiege.PsychicRitualIterationsCountRange.RandomInRange, parms.spawnCenter, parms.points);
		}

		// Token: 0x06008C2D RID: 35885 RVA: 0x0029B7CC File Offset: 0x002999CC
		private Pawn BestInvoker(List<Pawn> pawns)
		{
			Pawn pawn = pawns[0];
			for (int i = 1; i < pawns.Count; i++)
			{
				if (pawns[i].GetStatValue(StatDefOf.PsychicSensitivity, true, -1) > 0f && pawns[i].kindDef.isGoodPsychicRitualInvoker && pawns[i].kindDef.combatPower > pawn.kindDef.combatPower)
				{
					pawn = pawns[i];
				}
			}
			return pawn;
		}

		// Token: 0x06008C2E RID: 35886 RVA: 0x0029B846 File Offset: 0x00299A46
		public override bool CanUseWith(IncidentParms parms, PawnGroupKindDef groupKind)
		{
			return base.CanUseWith(parms, groupKind) && ModsConfig.AnomalyActive && parms.faction.def.canPsychicRitualSiege;
		}

		// Token: 0x06008C2F RID: 35887 RVA: 0x0029B872 File Offset: 0x00299A72
		public override bool CanUsePawn(float pointsTotal, Pawn p, List<Pawn> otherPawns)
		{
			return otherPawns.Count != 0 || (p.kindDef.isGoodPsychicRitualInvoker && p.GetStatValue(StatDefOf.PsychicSensitivity, true, -1) > 0f);
		}

		// Token: 0x06008C30 RID: 35888 RVA: 0x0029B8A1 File Offset: 0x00299AA1
		protected override bool MatchesRequiredPawnKind(PawnKindDef kind)
		{
			return kind.isGoodPsychicRitualInvoker;
		}

		// Token: 0x06008C31 RID: 35889 RVA: 0x000028E7 File Offset: 0x00000AE7
		protected override int MinRequiredPawnsForPoints(float pointsTotal, Faction faction = null)
		{
			return 1;
		}

		// Token: 0x04005B0A RID: 23306
		private static readonly IntRange PsychicRitualIterationsCountRange = new IntRange(3, 5);
	}
}
