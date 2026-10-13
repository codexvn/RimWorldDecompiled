using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020017AB RID: 6059
	public class ThinkNode_ConditionalNeedPercentageAbove : ThinkNode_Conditional
	{
		// Token: 0x06008DA2 RID: 36258 RVA: 0x002A0E91 File Offset: 0x0029F091
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalNeedPercentageAbove thinkNode_ConditionalNeedPercentageAbove = (ThinkNode_ConditionalNeedPercentageAbove)base.DeepCopy(resolve);
			thinkNode_ConditionalNeedPercentageAbove.need = this.need;
			thinkNode_ConditionalNeedPercentageAbove.threshold = this.threshold;
			return thinkNode_ConditionalNeedPercentageAbove;
		}

		// Token: 0x06008DA3 RID: 36259 RVA: 0x002A0EB8 File Offset: 0x0029F0B8
		protected override bool Satisfied(Pawn pawn)
		{
			Need need;
			if (!pawn.needs.TryGetNeed(this.need, out need))
			{
				return false;
			}
			if (need is Need_Seeker)
			{
				return need.CurInstantLevelPercentage > this.threshold;
			}
			return need.CurLevelPercentage > this.threshold;
		}

		// Token: 0x04005BCF RID: 23503
		private NeedDef need;

		// Token: 0x04005BD0 RID: 23504
		private float threshold;
	}
}
