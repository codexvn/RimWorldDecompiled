using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003724 RID: 14116
	public class StatPart_Slave : StatPart
	{
		// Token: 0x0601390D RID: 80141 RVA: 0x005CD3A6 File Offset: 0x005CB5A6
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (this.ActiveFor(req.Thing))
			{
				val *= this.factor;
			}
		}

		// Token: 0x0601390E RID: 80142 RVA: 0x005CD3C4 File Offset: 0x005CB5C4
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing && this.ActiveFor(req.Thing))
			{
				return "StatsReport_Slave".Translate() + (": x" + this.factor.ToStringPercent());
			}
			return null;
		}

		// Token: 0x0601390F RID: 80143 RVA: 0x005CD414 File Offset: 0x005CB614
		private bool ActiveFor(Thing t)
		{
			Pawn pawn = t as Pawn;
			return pawn != null && pawn.IsSlave;
		}

		// Token: 0x0400D906 RID: 55558
		private float factor = 0.75f;
	}
}
