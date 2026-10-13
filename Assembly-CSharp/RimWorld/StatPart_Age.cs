using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020036DD RID: 14045
	public class StatPart_Age : StatPart
	{
		// Token: 0x060137C9 RID: 79817 RVA: 0x005C93B6 File Offset: 0x005C75B6
		private bool ActiveFor(Pawn pawn)
		{
			return pawn.ageTracker != null && (!this.humanlikeOnly || pawn.RaceProps.Humanlike);
		}

		// Token: 0x060137CA RID: 79818 RVA: 0x005C93DC File Offset: 0x005C75DC
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && this.ActiveFor(pawn))
				{
					val *= this.AgeMultiplier(pawn);
				}
			}
		}

		// Token: 0x060137CB RID: 79819 RVA: 0x005C9418 File Offset: 0x005C7618
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && this.ActiveFor(pawn))
				{
					return "StatsReport_AgeMultiplier".Translate(pawn.ageTracker.AgeBiologicalYears) + ": x" + this.AgeMultiplier(pawn).ToStringPercent();
				}
			}
			return null;
		}

		// Token: 0x060137CC RID: 79820 RVA: 0x005C9484 File Offset: 0x005C7684
		private float AgeMultiplier(Pawn pawn)
		{
			if (!this.useBiologicalYears)
			{
				return this.curve.Evaluate((float)pawn.ageTracker.AgeBiologicalYears / pawn.RaceProps.lifeExpectancy);
			}
			return this.curve.Evaluate((float)pawn.ageTracker.AgeBiologicalYears);
		}

		// Token: 0x060137CD RID: 79821 RVA: 0x005C94D4 File Offset: 0x005C76D4
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.curve == null)
			{
				yield return "curve is null.";
			}
			yield break;
		}

		// Token: 0x0400D881 RID: 55425
		private SimpleCurve curve;

		// Token: 0x0400D882 RID: 55426
		private bool useBiologicalYears;

		// Token: 0x0400D883 RID: 55427
		private bool humanlikeOnly;
	}
}
