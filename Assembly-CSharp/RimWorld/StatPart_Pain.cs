using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02003715 RID: 14101
	public class StatPart_Pain : StatPart
	{
		// Token: 0x060138CF RID: 80079 RVA: 0x005CC418 File Offset: 0x005CA618
		public override void TransformValue(StatRequest req, ref float val)
		{
			Pawn pawn = req.Thing as Pawn;
			if (pawn == null)
			{
				return;
			}
			val *= this.PainFactor(pawn);
		}

		// Token: 0x060138D0 RID: 80080 RVA: 0x005CC442 File Offset: 0x005CA642
		public float PainFactor(Pawn pawn)
		{
			return 1f + pawn.health.hediffSet.PainTotal * this.factor;
		}

		// Token: 0x060138D1 RID: 80081 RVA: 0x005CC464 File Offset: 0x005CA664
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null)
				{
					return "StatsReport_Pain".Translate() + (": " + this.PainFactor(pawn).ToStringPercent("F0"));
				}
			}
			return null;
		}

		// Token: 0x0400D8DE RID: 55518
		private float factor = 1f;
	}
}
