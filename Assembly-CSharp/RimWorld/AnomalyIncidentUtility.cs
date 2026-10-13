using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D24 RID: 7460
	public static class AnomalyIncidentUtility
	{
		// Token: 0x0600A6DF RID: 42719 RVA: 0x002FCB18 File Offset: 0x002FAD18
		public static bool IncidentShardChance(float combatPoints)
		{
			return Rand.Chance(AnomalyIncidentUtility.CombatPointShardChanceCurve.Evaluate(combatPoints));
		}

		// Token: 0x0600A6E0 RID: 42720 RVA: 0x002FCB2C File Offset: 0x002FAD2C
		public static void PawnShardOnDeath(Pawn pawn)
		{
			pawn.health.AddHediff(HediffDefOf.ShardHolder, null, null, null);
		}

		// Token: 0x0400738B RID: 29579
		private static readonly SimpleCurve CombatPointShardChanceCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0.15f),
				true
			},
			{
				new CurvePoint(500f, 0.3f),
				true
			},
			{
				new CurvePoint(1000f, 0.5f),
				true
			},
			{
				new CurvePoint(5000f, 0.8f),
				true
			}
		};
	}
}
