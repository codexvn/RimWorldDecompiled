using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200174F RID: 5967
	public class RaidStrategyWorker_ImmediateAttackBreaching : RaidStrategyWorker_WithRequiredPawnKinds
	{
		// Token: 0x06008C15 RID: 35861 RVA: 0x0029B3B7 File Offset: 0x002995B7
		protected override bool MatchesRequiredPawnKind(PawnKindDef kind)
		{
			return kind.isGoodBreacher;
		}

		// Token: 0x06008C16 RID: 35862 RVA: 0x0029B3BF File Offset: 0x002995BF
		protected override int MinRequiredPawnsForPoints(float pointsTotal, Faction faction)
		{
			if (faction == null || faction != Faction.OfMechanoids)
			{
				return Mathf.RoundToInt(RaidStrategyWorker_ImmediateAttackBreaching.MinGoodBreachersFromPointCurve.Evaluate(pointsTotal));
			}
			return 1;
		}

		// Token: 0x06008C17 RID: 35863 RVA: 0x0029B3E0 File Offset: 0x002995E0
		protected override LordJob MakeLordJob(IncidentParms parms, Map map, List<Pawn> pawns, int raidSeed)
		{
			Faction faction = parms.faction;
			bool canTimeoutOrFlee = parms.canTimeoutOrFlee;
			bool canKidnap = parms.canKidnap;
			bool canTimeoutOrFlee2 = canTimeoutOrFlee;
			bool sappers = false;
			bool canSteal = parms.canSteal;
			return new LordJob_AssaultColony(faction, canKidnap, canTimeoutOrFlee2, sappers, this.useAvoidGridSmart, canSteal, true, false);
		}

		// Token: 0x04005B07 RID: 23303
		private static SimpleCurve MinGoodBreachersFromPointCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(200f, 1f),
				true
			},
			{
				new CurvePoint(1000f, 3f),
				true
			},
			{
				new CurvePoint(4000f, 4f),
				true
			}
		};

		// Token: 0x04005B08 RID: 23304
		private const int MaxGoodBreachesForMechanoids = 1;

		// Token: 0x04005B09 RID: 23305
		protected bool useAvoidGridSmart;
	}
}
