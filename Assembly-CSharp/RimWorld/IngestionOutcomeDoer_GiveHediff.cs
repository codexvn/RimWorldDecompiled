using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020019A5 RID: 6565
	public class IngestionOutcomeDoer_GiveHediff : IngestionOutcomeDoer
	{
		// Token: 0x0600948C RID: 38028 RVA: 0x002B9F00 File Offset: 0x002B8100
		protected override void DoIngestionOutcomeSpecial(Pawn pawn, Thing ingested, int ingestedCount)
		{
			Hediff hediff = HediffMaker.MakeHediff(this.hediffDef, pawn, null);
			float initialSeverity;
			if (this.severity > 0f)
			{
				initialSeverity = this.severity;
			}
			else
			{
				initialSeverity = this.hediffDef.initialSeverity;
			}
			AddictionUtility.ModifyChemicalEffectForToleranceAndBodySize(pawn, this.toleranceChemical, ref initialSeverity, this.multiplyByGeneToleranceFactors, this.divideByBodySize);
			hediff.Severity = initialSeverity;
			pawn.health.AddHediff(hediff, null, null, null);
		}

		// Token: 0x0600948D RID: 38029 RVA: 0x002B9F75 File Offset: 0x002B8175
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(ThingDef parentDef)
		{
			if (parentDef.IsDrug && this.chance >= 1f)
			{
				foreach (StatDrawEntry statDrawEntry in this.hediffDef.SpecialDisplayStats(StatRequest.ForEmpty()))
				{
					yield return statDrawEntry;
				}
				IEnumerator<StatDrawEntry> enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x04006078 RID: 24696
		public HediffDef hediffDef;

		// Token: 0x04006079 RID: 24697
		public float severity = -1f;

		// Token: 0x0400607A RID: 24698
		public ChemicalDef toleranceChemical;

		// Token: 0x0400607B RID: 24699
		private bool divideByBodySize;

		// Token: 0x0400607C RID: 24700
		public bool multiplyByGeneToleranceFactors;
	}
}
