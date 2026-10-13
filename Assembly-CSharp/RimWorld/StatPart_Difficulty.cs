using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020036F2 RID: 14066
	[Obsolete]
	public class StatPart_Difficulty : StatPart
	{
		// Token: 0x06013823 RID: 79907 RVA: 0x005CA1A1 File Offset: 0x005C83A1
		public override void TransformValue(StatRequest req, ref float val)
		{
			val *= this.Multiplier(Find.Storyteller.difficultyDef);
		}

		// Token: 0x06013824 RID: 79908 RVA: 0x005CA1B8 File Offset: 0x005C83B8
		public override string ExplanationPart(StatRequest req)
		{
			return "StatsReport_DifficultyMultiplier".Translate() + ": x" + this.Multiplier(Find.Storyteller.difficultyDef).ToStringPercent();
		}

		// Token: 0x06013825 RID: 79909 RVA: 0x0004FC72 File Offset: 0x0004DE72
		private float Multiplier(DifficultyDef d)
		{
			return 1f;
		}

		// Token: 0x0400D89A RID: 55450
		private List<float> factorsPerDifficulty = new List<float>();
	}
}
