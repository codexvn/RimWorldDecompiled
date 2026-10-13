using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200371E RID: 14110
	public class StatPart_RevenantSpeed : StatPart
	{
		// Token: 0x060138F5 RID: 80117 RVA: 0x005CCEB4 File Offset: 0x005CB0B4
		protected virtual bool ActiveFor(Thing t)
		{
			Pawn pawn = t as Pawn;
			return pawn != null && pawn.kindDef == PawnKindDefOf.Revenant && !pawn.IsPsychologicallyInvisible() && Mathf.Max(pawn.mindState.lastBecameVisibleTick, pawn.mindState.lastForcedVisibleTick) > 0;
		}

		// Token: 0x060138F6 RID: 80118 RVA: 0x005CCF00 File Offset: 0x005CB100
		private float GetFactor(Pawn p)
		{
			int num = Mathf.Max(p.mindState.lastBecameVisibleTick, p.mindState.lastForcedVisibleTick);
			int numTicks = Find.TickManager.TicksGame - num;
			return RevenantUtility.SpeedRangeFromBecameVisibleCurve.Evaluate(numTicks.TicksToSeconds());
		}

		// Token: 0x060138F7 RID: 80119 RVA: 0x005CCF46 File Offset: 0x005CB146
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (!req.HasThing)
			{
				return;
			}
			if (this.ActiveFor(req.Thing))
			{
				val *= this.GetFactor((Pawn)req.Thing);
			}
		}

		// Token: 0x060138F8 RID: 80120 RVA: 0x00002C42 File Offset: 0x00000E42
		public override string ExplanationPart(StatRequest req)
		{
			return null;
		}
	}
}
