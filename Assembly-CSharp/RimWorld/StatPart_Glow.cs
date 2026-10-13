using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02003701 RID: 14081
	public class StatPart_Glow : StatPart
	{
		// Token: 0x0601386C RID: 79980 RVA: 0x005CB543 File Offset: 0x005C9743
		public override IEnumerable<string> ConfigErrors()
		{
			if (this.factorFromGlowCurve == null)
			{
				yield return "factorFromLightCurve is null.";
			}
			yield break;
		}

		// Token: 0x0601386D RID: 79981 RVA: 0x005CB553 File Offset: 0x005C9753
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && this.ActiveFor(req.Thing))
			{
				val *= this.FactorFromGlow(req.Thing);
			}
		}

		// Token: 0x0601386E RID: 79982 RVA: 0x005CB580 File Offset: 0x005C9780
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && this.ActiveFor(req.Thing))
			{
				return "StatsReport_LightMultiplier".Translate(this.GlowLevel(req.Thing).ToStringPercent()) + ": x" + this.FactorFromGlow(req.Thing).ToStringPercent();
			}
			return null;
		}

		// Token: 0x0601386F RID: 79983 RVA: 0x005CB5F0 File Offset: 0x005C97F0
		private bool ActiveFor(Thing t)
		{
			Pawn pawn = t as Pawn;
			if (pawn != null)
			{
				if (this.humanlikeOnly && !pawn.RaceProps.Humanlike)
				{
					return false;
				}
				if (pawn.IsShambler)
				{
					return false;
				}
				if (this.ignoreIfIncapableOfSight && PawnUtility.IsBiologicallyOrArtificiallyBlind(pawn))
				{
					return false;
				}
				if (this.ignoreIfPrefersDarkness)
				{
					if (pawn.Ideo != null && pawn.Ideo.IdeoPrefersDarkness())
					{
						return false;
					}
					if (pawn.genes != null && !pawn.genes.AffectedByDarkness)
					{
						return false;
					}
				}
			}
			else if (this.pawnOnly)
			{
				return false;
			}
			return t.Spawned;
		}

		// Token: 0x06013870 RID: 79984 RVA: 0x005CB680 File Offset: 0x005C9880
		private float GlowLevel(Thing t)
		{
			return t.Map.glowGrid.GroundGlowAt(t.Position, false, false);
		}

		// Token: 0x06013871 RID: 79985 RVA: 0x005CB69A File Offset: 0x005C989A
		private float FactorFromGlow(Thing t)
		{
			return this.factorFromGlowCurve.Evaluate(this.GlowLevel(t));
		}

		// Token: 0x0400D8BC RID: 55484
		private bool pawnOnly;

		// Token: 0x0400D8BD RID: 55485
		private bool humanlikeOnly;

		// Token: 0x0400D8BE RID: 55486
		private SimpleCurve factorFromGlowCurve;

		// Token: 0x0400D8BF RID: 55487
		private bool ignoreIfPrefersDarkness;

		// Token: 0x0400D8C0 RID: 55488
		private bool ignoreIfIncapableOfSight;
	}
}
