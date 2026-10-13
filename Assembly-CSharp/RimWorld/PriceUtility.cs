using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02003678 RID: 13944
	public static class PriceUtility
	{
		// Token: 0x060135A1 RID: 79265 RVA: 0x005C2840 File Offset: 0x005C0A40
		public static float PawnQualityPriceFactor(Pawn pawn, StringBuilder explanation = null)
		{
			float num = 1f;
			num *= Mathf.Lerp(0.19999999f, 1f, pawn.health.summaryHealth.SummaryHealthPercent);
			List<PawnCapacityDef> allDefsListForReading = DefDatabase<PawnCapacityDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				if (!pawn.health.capacities.CapableOf(allDefsListForReading[i]))
				{
					num *= 0.6f;
				}
				else
				{
					float t = PawnCapacityUtility.CalculateCapacityLevel(pawn.health.hediffSet, allDefsListForReading[i], null, true);
					num *= Mathf.Lerp(0.5f, 1f, t);
				}
			}
			if (pawn.skills != null)
			{
				num *= PriceUtility.AverageSkillCurve.Evaluate(pawn.skills.skills.Average((SkillRecord sk) => (float)sk.Level));
			}
			num *= pawn.ageTracker.CurLifeStage.marketValueFactor;
			if (pawn.story != null && pawn.story.traits != null)
			{
				for (int j = 0; j < pawn.story.traits.allTraits.Count; j++)
				{
					Trait trait = pawn.story.traits.allTraits[j];
					if (!trait.Suppressed)
					{
						num += trait.CurrentData.marketValueFactorOffset;
					}
				}
			}
			num += pawn.GetStatValue(StatDefOf.PawnBeauty, true, -1) * 0.2f;
			if (num < 0.1f)
			{
				num = 0.1f;
			}
			if (explanation != null)
			{
				explanation.AppendLine("StatsReport_CharacterQuality".Translate() + ": x" + num.ToStringPercent());
			}
			return num;
		}

		// Token: 0x060135A2 RID: 79266 RVA: 0x005C29F0 File Offset: 0x005C0BF0
		public static float PawnQualityPriceOffset(Pawn pawn, StringBuilder explanation = null)
		{
			float num = 0f;
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].def.priceImpact || hediffs[i].def.priceOffset != 0f)
				{
					float num2 = hediffs[i].def.priceOffset;
					if (num2 == 0f && hediffs[i].def.spawnThingOnRemoved != null)
					{
						num2 = hediffs[i].def.spawnThingOnRemoved.BaseMarketValue;
					}
					if (num2 >= 1f || num2 <= -1f)
					{
						num += num2;
						if (explanation != null)
						{
							explanation.AppendLine(hediffs[i].LabelBaseCap + ": " + num2.ToStringMoneyOffset(null));
						}
					}
				}
			}
			return num;
		}

		// Token: 0x0400D6B5 RID: 54965
		private const float MinFactor = 0.1f;

		// Token: 0x0400D6B6 RID: 54966
		private const float SummaryHealthImpact = 0.8f;

		// Token: 0x0400D6B7 RID: 54967
		private const float CapacityImpact = 0.5f;

		// Token: 0x0400D6B8 RID: 54968
		private const float MissingCapacityFactor = 0.6f;

		// Token: 0x0400D6B9 RID: 54969
		private static readonly SimpleCurve AverageSkillCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0.2f),
				true
			},
			{
				new CurvePoint(5.5f, 1f),
				true
			},
			{
				new CurvePoint(20f, 3f),
				true
			}
		};
	}
}
