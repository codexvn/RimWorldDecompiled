using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020036DF RID: 14047
	public class StatPart_AgeOffset : StatPart
	{
		// Token: 0x060137D7 RID: 79831 RVA: 0x005C95A3 File Offset: 0x005C77A3
		private bool ActiveFor(Pawn pawn)
		{
			return pawn.ageTracker != null && (!this.humanlikeOnly || pawn.RaceProps.Humanlike);
		}

		// Token: 0x060137D8 RID: 79832 RVA: 0x005C95C8 File Offset: 0x005C77C8
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && this.ActiveFor(pawn))
				{
					val += this.AgeOffset(pawn);
				}
			}
		}

		// Token: 0x060137D9 RID: 79833 RVA: 0x005C9604 File Offset: 0x005C7804
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && this.ActiveFor(pawn))
				{
					return "StatsReport_AgeOffset".Translate(pawn.ageTracker.AgeBiologicalYears) + ": " + this.AgeOffset(pawn).ToStringByStyle(this.parentStat.toStringStyle, this.parentStat.toStringNumberSense);
				}
			}
			return null;
		}

		// Token: 0x060137DA RID: 79834 RVA: 0x005C9688 File Offset: 0x005C7888
		private float AgeOffset(Pawn pawn)
		{
			if (!this.useBiologicalYears)
			{
				return this.curve.Evaluate((float)pawn.ageTracker.AgeBiologicalYears / pawn.RaceProps.lifeExpectancy);
			}
			return this.curve.Evaluate((float)pawn.ageTracker.AgeBiologicalYears);
		}

		// Token: 0x060137DB RID: 79835 RVA: 0x005C96D8 File Offset: 0x005C78D8
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.curve == null)
			{
				yield return "curve is null.";
			}
			yield break;
		}

		// Token: 0x0400D888 RID: 55432
		private SimpleCurve curve;

		// Token: 0x0400D889 RID: 55433
		private bool useBiologicalYears;

		// Token: 0x0400D88A RID: 55434
		private bool humanlikeOnly;
	}
}
