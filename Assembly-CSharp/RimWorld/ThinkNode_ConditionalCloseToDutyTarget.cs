using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020017B0 RID: 6064
	public class ThinkNode_ConditionalCloseToDutyTarget : ThinkNode_Conditional
	{
		// Token: 0x06008DAD RID: 36269 RVA: 0x002A0FAD File Offset: 0x0029F1AD
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalCloseToDutyTarget thinkNode_ConditionalCloseToDutyTarget = (ThinkNode_ConditionalCloseToDutyTarget)base.DeepCopy(resolve);
			thinkNode_ConditionalCloseToDutyTarget.maxDistToDutyTarget = this.maxDistToDutyTarget;
			return thinkNode_ConditionalCloseToDutyTarget;
		}

		// Token: 0x06008DAE RID: 36270 RVA: 0x002A0FC8 File Offset: 0x0029F1C8
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.duty.focus.IsValid && pawn.Position.InHorDistOf(pawn.mindState.duty.focus.Cell, this.maxDistToDutyTarget);
		}

		// Token: 0x04005BD1 RID: 23505
		private float maxDistToDutyTarget = 10f;
	}
}
