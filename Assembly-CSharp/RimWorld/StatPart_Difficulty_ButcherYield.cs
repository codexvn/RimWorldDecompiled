using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020036F3 RID: 14067
	public class StatPart_Difficulty_ButcherYield : StatPart
	{
		// Token: 0x06013827 RID: 79911 RVA: 0x005CA200 File Offset: 0x005C8400
		public override void TransformValue(StatRequest req, ref float val)
		{
			float num = (Find.Storyteller != null) ? Find.Storyteller.difficulty.butcherYieldFactor : 1f;
			val *= num;
		}

		// Token: 0x06013828 RID: 79912 RVA: 0x005CA234 File Offset: 0x005C8434
		public override string ExplanationPart(StatRequest req)
		{
			return "StatsReport_DifficultyMultiplier".Translate(Find.Storyteller.difficultyDef.label) + ": " + Find.Storyteller.difficulty.butcherYieldFactor.ToStringByStyle(ToStringStyle.PercentZero, ToStringNumberSense.Factor);
		}
	}
}
