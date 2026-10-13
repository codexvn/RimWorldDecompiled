using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02002DB9 RID: 11705
	public class CompProducesBioferrite : ThingComp
	{
		// Token: 0x170029F4 RID: 10740
		// (get) Token: 0x06010741 RID: 67393 RVA: 0x004BEEF4 File Offset: 0x004BD0F4
		public CompProperties_ProducesBioferrite Props
		{
			get
			{
				return (CompProperties_ProducesBioferrite)this.props;
			}
		}

		// Token: 0x06010742 RID: 67394 RVA: 0x004BEF01 File Offset: 0x004BD101
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			StatDrawEntry statDrawEntry = CompProducesBioferrite.BioferriteStatDrawEntry(this.parent as Pawn);
			statDrawEntry.overridesHideStats = true;
			yield return statDrawEntry;
			yield break;
		}

		// Token: 0x06010743 RID: 67395 RVA: 0x004BEF14 File Offset: 0x004BD114
		public static float BioferritePerDay(Pawn pawn)
		{
			CompProducesBioferrite compProducesBioferrite = pawn.TryGetComp<CompProducesBioferrite>();
			if (compProducesBioferrite == null && (!pawn.IsMutant || !pawn.mutant.Def.producesBioferrite))
			{
				return 0f;
			}
			if (pawn.health.hediffSet.HasHediff(HediffDefOf.BioferriteExtracted, false))
			{
				return 0f;
			}
			float num = (compProducesBioferrite != null) ? compProducesBioferrite.Props.bioferriteDensity : 1f;
			return pawn.BodySize * num;
		}

		// Token: 0x06010744 RID: 67396 RVA: 0x004BEF88 File Offset: 0x004BD188
		public static StatDrawEntry BioferriteStatDrawEntry(Pawn pawn)
		{
			CompProducesBioferrite compProducesBioferrite = pawn.TryGetComp<CompProducesBioferrite>();
			StringBuilder stringBuilder = new StringBuilder("StatsReport_BioferriteGeneration_Desc".Translate());
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("StatsReport_BaseValue".Translate() + ": 1");
			stringBuilder.AppendLine();
			stringBuilder.AppendLine("StatsReport_BodySize".Translate(pawn.BodySize.ToString("F2")) + ": x" + pawn.BodySize.ToStringPercent());
			if (compProducesBioferrite != null)
			{
				stringBuilder.AppendLine("StatsReport_BioferriteDensityMultiplier".Translate() + ": x" + compProducesBioferrite.Props.bioferriteDensity.ToStringPercent());
			}
			if (pawn.health.hediffSet.HasHediff(HediffDefOf.BioferriteExtracted, false))
			{
				stringBuilder.AppendLine("StatsReport_BioferriteExtractedMultiplier".Translate() + ": x" + 0f.ToStringPercent());
			}
			stringBuilder.AppendLine();
			stringBuilder.Append("StatsReport_FinalValue".Translate() + ": " + CompProducesBioferrite.BioferritePerDay(pawn).ToString("F1"));
			return new StatDrawEntry(StatCategoryDefOf.Containment, "StatsReport_BioferriteGeneration".Translate(), CompProducesBioferrite.BioferritePerDay(pawn).ToString(), stringBuilder.ToString(), 100, null, null, false, false);
		}
	}
}
