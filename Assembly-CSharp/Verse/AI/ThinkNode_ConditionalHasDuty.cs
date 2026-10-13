using System;

namespace Verse.AI
{
	// Token: 0x02000FF4 RID: 4084
	public class ThinkNode_ConditionalHasDuty : ThinkNode_Conditional
	{
		// Token: 0x06006475 RID: 25717 RVA: 0x001FD5DE File Offset: 0x001FB7DE
		protected override bool Satisfied(Pawn pawn)
		{
			Pawn_MindState mindState = pawn.mindState;
			DutyDef dutyDef;
			if (mindState == null)
			{
				dutyDef = null;
			}
			else
			{
				PawnDuty duty = mindState.duty;
				dutyDef = ((duty != null) ? duty.def : null);
			}
			return dutyDef == this.dutyDef;
		}

		// Token: 0x06006476 RID: 25718 RVA: 0x001FD606 File Offset: 0x001FB806
		public override ThinkNode DeepCopy(bool resolve = true)
		{
			ThinkNode_ConditionalHasDuty thinkNode_ConditionalHasDuty = (ThinkNode_ConditionalHasDuty)base.DeepCopy(resolve);
			thinkNode_ConditionalHasDuty.dutyDef = this.dutyDef;
			return thinkNode_ConditionalHasDuty;
		}

		// Token: 0x040047D5 RID: 18389
		public DutyDef dutyDef;
	}
}
