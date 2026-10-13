using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200370D RID: 14093
	public class StatPart_Mood : StatPart
	{
		// Token: 0x060138A8 RID: 80040 RVA: 0x005CBE31 File Offset: 0x005CA031
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.factorFromMoodCurve == null)
			{
				yield return "curve is null.";
			}
			yield break;
		}

		// Token: 0x060138A9 RID: 80041 RVA: 0x005CBE44 File Offset: 0x005CA044
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && this.ActiveFor(pawn))
				{
					val *= this.FactorFromMood(pawn);
				}
			}
		}

		// Token: 0x060138AA RID: 80042 RVA: 0x005CBE80 File Offset: 0x005CA080
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && this.ActiveFor(pawn))
				{
					return "StatsReport_MoodMultiplier".Translate(pawn.needs.mood.CurLevel.ToStringPercent()) + ": x" + this.FactorFromMood(pawn).ToStringPercent();
				}
			}
			return null;
		}

		// Token: 0x060138AB RID: 80043 RVA: 0x002D7342 File Offset: 0x002D5542
		private bool ActiveFor(Pawn pawn)
		{
			return pawn.needs.mood != null;
		}

		// Token: 0x060138AC RID: 80044 RVA: 0x005CBEF5 File Offset: 0x005CA0F5
		private float FactorFromMood(Pawn pawn)
		{
			return this.factorFromMoodCurve.Evaluate(pawn.needs.mood.CurLevel);
		}

		// Token: 0x0400D8D0 RID: 55504
		private SimpleCurve factorFromMoodCurve;
	}
}
