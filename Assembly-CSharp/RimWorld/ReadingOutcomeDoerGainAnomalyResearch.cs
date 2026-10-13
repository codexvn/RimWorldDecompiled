using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200192A RID: 6442
	public class ReadingOutcomeDoerGainAnomalyResearch : ReadingOutcomeDoerGainResearch
	{
		// Token: 0x17001630 RID: 5680
		// (get) Token: 0x06009281 RID: 37505 RVA: 0x002B278B File Offset: 0x002B098B
		public new BookOutcomeProperties_GainAnomalyResearch Props
		{
			get
			{
				return (BookOutcomeProperties_GainAnomalyResearch)this.props;
			}
		}

		// Token: 0x17001631 RID: 5681
		// (get) Token: 0x06009282 RID: 37506 RVA: 0x002B2798 File Offset: 0x002B0998
		public override int RoundTo { get; }

		// Token: 0x06009283 RID: 37507 RVA: 0x002B27A0 File Offset: 0x002B09A0
		protected override float GetBaseValue()
		{
			return BookUtility.GetAnomalyExpForQuality(base.Quality);
		}

		// Token: 0x06009284 RID: 37508 RVA: 0x002B27B0 File Offset: 0x002B09B0
		public override bool DoesProvidesOutcome(Pawn reader)
		{
			foreach (KeyValuePair<ResearchProjectDef, float> keyValuePair in this.values)
			{
				ResearchProjectDef researchProjectDef;
				float num;
				keyValuePair.Deconstruct(out researchProjectDef, out num);
				if (researchProjectDef.CanStartNow)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06009285 RID: 37509 RVA: 0x002B2818 File Offset: 0x002B0A18
		public override void OnReadingTick(Pawn reader, float factor)
		{
			foreach (KeyValuePair<ResearchProjectDef, float> keyValuePair in this.values)
			{
				ResearchProjectDef researchProjectDef;
				float num;
				keyValuePair.Deconstruct(out researchProjectDef, out num);
				ResearchProjectDef researchProjectDef2 = researchProjectDef;
				float num2 = num;
				if (researchProjectDef2.CanStartNow)
				{
					Find.ResearchManager.ApplyKnowledge(researchProjectDef2, num2 * factor, out num);
				}
			}
		}
	}
}
