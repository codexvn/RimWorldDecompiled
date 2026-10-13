using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020017C6 RID: 6086
	public class ThinkNode_ConditionalHasHediff : ThinkNode_Conditional
	{
		// Token: 0x06008DDD RID: 36317 RVA: 0x002A16A7 File Offset: 0x0029F8A7
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalHasHediff thinkNode_ConditionalHasHediff = (ThinkNode_ConditionalHasHediff)base.DeepCopy(resolve);
			thinkNode_ConditionalHasHediff.hediff = this.hediff;
			thinkNode_ConditionalHasHediff.severityRange = this.severityRange;
			return thinkNode_ConditionalHasHediff;
		}

		// Token: 0x06008DDE RID: 36318 RVA: 0x002A16D0 File Offset: 0x0029F8D0
		protected override bool Satisfied(Pawn pawn)
		{
			Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(this.hediff, false);
			return firstHediffOfDef != null && firstHediffOfDef.Severity >= this.severityRange.RandomInRange;
		}

		// Token: 0x04005BD9 RID: 23513
		public HediffDef hediff;

		// Token: 0x04005BDA RID: 23514
		public FloatRange severityRange = FloatRange.Zero;
	}
}
