using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000784 RID: 1924
	public class HediffGiver_VacuumBurn : HediffGiver
	{
		// Token: 0x06002EFD RID: 12029 RVA: 0x000F175C File Offset: 0x000EF95C
		public override void OnIntervalPassed(Pawn pawn, Hediff cause)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			if (!pawn.Spawned || !pawn.Map.Biome.inVacuum)
			{
				return;
			}
			if (!pawn.HarmedByVacuum)
			{
				return;
			}
			float vacuum = pawn.Position.GetVacuum(pawn.Map);
			if (vacuum < 0.5f)
			{
				return;
			}
			int num = GenTicks.TicksGame - pawn.lastVacuumBurntTick;
			int num2 = HediffGiver_VacuumBurn.VacuumSecondsBurnRate.Evaluate(vacuum).SecondsToTicks();
			if (num < num2)
			{
				return;
			}
			pawn.lastVacuumBurntTick = GenTicks.TicksGame;
			BodyPartRecord hitPart;
			if (VacuumUtility.TryGetVacuumBurnablePart(pawn, out hitPart))
			{
				DamageInfo dinfo = new DamageInfo(DamageDefOf.VacuumBurn, (float)HediffGiver_VacuumBurn.BurnDamageRange.RandomInRange, 999f, -1f, null, hitPart, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false);
				pawn.TakeDamage(dinfo);
			}
		}

		// Token: 0x040024BC RID: 9404
		private static readonly IntRange BurnDamageRange = new IntRange(1, 2);

		// Token: 0x040024BD RID: 9405
		private static readonly SimpleCurve VacuumSecondsBurnRate = new SimpleCurve
		{
			{
				new CurvePoint(0.5f, 20f),
				true
			},
			{
				new CurvePoint(1f, 5f),
				true
			}
		};
	}
}
