using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003721 RID: 14113
	public class StatPart_ShamblerCorpse : StatPart
	{
		// Token: 0x06013901 RID: 80129 RVA: 0x005CD111 File Offset: 0x005CB311
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (this.ActiveFor(req.Thing))
			{
				val *= this.multiplier;
			}
		}

		// Token: 0x06013902 RID: 80130 RVA: 0x005CD130 File Offset: 0x005CB330
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && this.ActiveFor(req.Thing))
			{
				return "StatsReport_MultiplierFor".Translate(HediffDefOf.ShamblerCorpse.label) + (": x" + this.multiplier.ToStringPercent());
			}
			return null;
		}

		// Token: 0x06013903 RID: 80131 RVA: 0x005CD190 File Offset: 0x005CB390
		private bool ActiveFor(Thing t)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return false;
			}
			if (t == null)
			{
				return false;
			}
			Corpse corpse = t as Corpse;
			return corpse != null && corpse.InnerPawn.health.hediffSet.HasHediff(HediffDefOf.ShamblerCorpse, false);
		}

		// Token: 0x0400D900 RID: 55552
		private float multiplier = 1f;
	}
}
