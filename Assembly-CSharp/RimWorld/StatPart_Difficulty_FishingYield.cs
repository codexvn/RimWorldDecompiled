using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020036F4 RID: 14068
	public class StatPart_Difficulty_FishingYield : StatPart
	{
		// Token: 0x0601382A RID: 79914 RVA: 0x005CA28C File Offset: 0x005C848C
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num = (Find.Storyteller != null) ? Find.Storyteller.difficulty.fishingYieldFactor : 1f;
			val *= num;
		}

		// Token: 0x0601382B RID: 79915 RVA: 0x005CA2C0 File Offset: 0x005C84C0
		public override string ExplanationPart(StatRequest req)
		{
			return "StatsReport_DifficultyMultiplier".Translate(Find.Storyteller.difficultyDef.label) + ": " + Find.Storyteller.difficulty.fishingYieldFactor.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Factor);
		}
	}
}
