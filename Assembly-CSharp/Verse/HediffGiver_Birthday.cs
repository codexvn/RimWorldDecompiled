using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000776 RID: 1910
	public class HediffGiver_Birthday : HediffGiver
	{
		// Token: 0x06002EDA RID: 11994 RVA: 0x000F0C64 File Offset: 0x000EEE64
		public void TryApplyAndSimulateSeverityChange(Pawn pawn, float gotAtAge, bool tryNotToKillPawn)
		{
			HediffGiver_Birthday.addedHediffs.Clear();
			if (!base.TryApply(pawn, HediffGiver_Birthday.addedHediffs))
			{
				return;
			}
			if (this.averageSeverityPerDayBeforeGeneration != 0f)
			{
				float num = (pawn.ageTracker.AgeBiologicalYearsFloat - gotAtAge) * 60f;
				if (num < 0f)
				{
					Log.Error("daysPassed < 0, pawn=" + ((pawn != null) ? pawn.ToString() : null) + ", gotAtAge=" + gotAtAge.ToString());
					return;
				}
				for (int i = 0; i < HediffGiver_Birthday.addedHediffs.Count; i++)
				{
					this.SimulateSeverityChange(pawn, HediffGiver_Birthday.addedHediffs[i], num, tryNotToKillPawn);
				}
			}
			HediffGiver_Birthday.addedHediffs.Clear();
		}

		// Token: 0x06002EDB RID: 11995 RVA: 0x000F0D10 File Offset: 0x000EEF10
		private void SimulateSeverityChange(Pawn pawn, Hediff hediff, float daysPassed, bool tryNotToKillPawn)
		{
			float num = this.averageSeverityPerDayBeforeGeneration * daysPassed;
			num *= Rand.Range(0.5f, 1.4f);
			num += hediff.def.initialSeverity;
			if (tryNotToKillPawn)
			{
				this.AvoidLifeThreateningStages(ref num, hediff.def.stages);
			}
			hediff.Severity = num;
			pawn.health.Notify_HediffChanged(hediff);
		}

		// Token: 0x06002EDC RID: 11996 RVA: 0x000F0D70 File Offset: 0x000EEF70
		private void AvoidLifeThreateningStages(ref float severity, List<HediffStage> stages)
		{
			if (stages.NullOrEmpty<HediffStage>())
			{
				return;
			}
			int num = -1;
			for (int i = 0; i < stages.Count; i++)
			{
				if (stages[i].lifeThreatening)
				{
					num = i;
					break;
				}
			}
			if (num >= 0)
			{
				if (num == 0)
				{
					severity = Mathf.Min(severity, stages[num].minSeverity);
					return;
				}
				severity = Mathf.Min(severity, (stages[num].minSeverity + stages[num - 1].minSeverity) / 2f);
			}
		}

		// Token: 0x06002EDD RID: 11997 RVA: 0x000F0DF4 File Offset: 0x000EEFF4
		public float DebugChanceToHaveAtAge(Pawn pawn, int age)
		{
			float num = 1f;
			for (int i = 1; i <= age; i++)
			{
				float x = (float)i / pawn.RaceProps.lifeExpectancy;
				num *= 1f - this.ageFractionChanceCurve.Evaluate(x);
			}
			return 1f - num;
		}

		// Token: 0x040024A4 RID: 9380
		public SimpleCurve ageFractionChanceCurve;

		// Token: 0x040024A5 RID: 9381
		public float averageSeverityPerDayBeforeGeneration;

		// Token: 0x040024A6 RID: 9382
		private static List<Hediff> addedHediffs = new List<Hediff>();
	}
}
