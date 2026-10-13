using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020036F1 RID: 14065
	public class StatPart_Deathresting : StatPart
	{
		// Token: 0x06013820 RID: 79904 RVA: 0x005CA104 File Offset: 0x005C8304
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && pawn.Deathresting)
				{
					val *= this.factor;
				}
			}
		}

		// Token: 0x06013821 RID: 79905 RVA: 0x005CA140 File Offset: 0x005C8340
		public override string ExplanationPart(StatRequest req)
		{
			if (req.HasThing)
			{
				Pawn pawn = req.Thing as Pawn;
				if (pawn != null && pawn.Deathresting)
				{
					return "Deathresting".Translate().CapitalizeFirst() + ": x" + this.factor.ToStringPercent();
				}
			}
			return null;
		}

		// Token: 0x0400D899 RID: 55449
		public float factor;
	}
}
