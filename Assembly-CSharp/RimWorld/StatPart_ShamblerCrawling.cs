using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003722 RID: 14114
	public class StatPart_ShamblerCrawling : StatPart
	{
		// Token: 0x06013905 RID: 80133 RVA: 0x005CD1E8 File Offset: 0x005CB3E8
		private bool ActiveFor(Thing t)
		{
			if (!t.Spawned)
			{
				return false;
			}
			Pawn pawn = t as Pawn;
			return pawn != null && pawn.IsShambler && pawn.Crawling;
		}

		// Token: 0x06013906 RID: 80134 RVA: 0x005CD219 File Offset: 0x005CB419
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (!req.HasThing)
			{
				return;
			}
			if (this.ActiveFor(req.Thing))
			{
				val *= this.factor;
			}
		}

		// Token: 0x06013907 RID: 80135 RVA: 0x005CD240 File Offset: 0x005CB440
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && this.ActiveFor(req.Thing))
			{
				return "StatsReport_ShamblerCrawling".Translate() + ": x" + this.factor.ToStringPercent();
			}
			return null;
		}

		// Token: 0x0400D901 RID: 55553
		public float factor;
	}
}
