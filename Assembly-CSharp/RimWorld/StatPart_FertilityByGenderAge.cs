using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020036F7 RID: 14071
	public class StatPart_FertilityByGenderAge : StatPart
	{
		// Token: 0x06013834 RID: 79924 RVA: 0x005CA5FC File Offset: 0x005C87FC
		public override string ExplanationPart(StatRequest req)
		{
			Pawn pawn = req.Thing as Pawn;
			if (pawn != null && pawn != null)
			{
				return "StatsReport_FertilityAgeFactor".Translate() + ": x" + this.AgeFactor(pawn).ToStringPercent();
			}
			return null;
		}

		// Token: 0x06013835 RID: 79925 RVA: 0x005CA648 File Offset: 0x005C8848
		public override void TransformValue(StatRequest req, ref float val)
		{
			Pawn pawn = req.Thing as Pawn;
			if (pawn != null && pawn != null)
			{
				val *= this.AgeFactor(pawn);
			}
		}

		// Token: 0x06013836 RID: 79926 RVA: 0x005CA674 File Offset: 0x005C8874
		private float AgeFactor(Pawn pawn)
		{
			return ((pawn.gender == Gender.Female) ? this.femaleFertilityAgeFactor : this.maleFertilityAgeFactor).Evaluate(pawn.ageTracker.AgeBiologicalYearsFloat);
		}

		// Token: 0x0400D89E RID: 55454
		protected SimpleCurve maleFertilityAgeFactor;

		// Token: 0x0400D89F RID: 55455
		protected SimpleCurve femaleFertilityAgeFactor;
	}
}
