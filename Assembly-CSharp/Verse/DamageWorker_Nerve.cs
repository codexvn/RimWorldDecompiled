using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000673 RID: 1651
	public class DamageWorker_Nerve : DamageWorker_AddInjury
	{
		// Token: 0x06002977 RID: 10615 RVA: 0x000DA948 File Offset: 0x000D8B48
		protected override void ApplySpecialEffectsToPart(Pawn pawn, float totalDamage, DamageInfo dinfo, DamageWorker.DamageResult result)
		{
			base.ApplySpecialEffectsToPart(pawn, totalDamage, dinfo, result);
			if (!pawn.RaceProps.IsMechanoid && !pawn.Dead && result.totalDamageDealt > 0f)
			{
				DamageInfo dinfo2 = dinfo;
				dinfo2.Def = DamageDefOf.NerveStun;
				float amount = DamageWorker_Nerve.NerveStunSecondsFromBodySizeCurve.Evaluate(pawn.BodySize) * 2f * this.StunDamageFactorFromQuality(dinfo.WeaponQuality);
				dinfo2.SetAmount(amount);
				pawn.TakeDamage(dinfo2);
			}
		}

		// Token: 0x06002978 RID: 10616 RVA: 0x000DA9C6 File Offset: 0x000D8BC6
		private float StunDamageFactorFromQuality(QualityCategory category)
		{
			if (category == QualityCategory.Awful)
			{
				return 0.75f;
			}
			if (category == QualityCategory.Masterwork)
			{
				return 1.25f;
			}
			if (category != QualityCategory.Legendary)
			{
				return 1f;
			}
			return 1.5f;
		}

		// Token: 0x040021C1 RID: 8641
		private const float StunDamageFactor_Awful = 0.75f;

		// Token: 0x040021C2 RID: 8642
		private const float StunDamageFactor_Default = 1f;

		// Token: 0x040021C3 RID: 8643
		private const float StunDamageFactor_Masterwork = 1.25f;

		// Token: 0x040021C4 RID: 8644
		private const float StunDamageFactor_Legendary = 1.5f;

		// Token: 0x040021C5 RID: 8645
		private static readonly SimpleCurve NerveStunSecondsFromBodySizeCurve = new SimpleCurve
		{
			{
				new CurvePoint(0.01f, 3f),
				true
			},
			{
				new CurvePoint(1f, 2f),
				true
			},
			{
				new CurvePoint(2f, 2f),
				true
			},
			{
				new CurvePoint(4f, 1f),
				true
			}
		};
	}
}
