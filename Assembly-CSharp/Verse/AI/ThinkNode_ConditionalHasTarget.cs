using System;

namespace Verse.AI
{
	// Token: 0x02000FE3 RID: 4067
	public class ThinkNode_ConditionalHasTarget : ThinkNode_Conditional
	{
		// Token: 0x0600644E RID: 25678 RVA: 0x001FD2EE File Offset: 0x001FB4EE
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.enemyTarget != null;
		}
	}
}
