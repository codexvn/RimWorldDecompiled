using System;

namespace Verse.AI
{
	// Token: 0x02000FE2 RID: 4066
	public class ThinkNode_ConditionalNoTarget : ThinkNode_Conditional
	{
		// Token: 0x0600644C RID: 25676 RVA: 0x001FD2DE File Offset: 0x001FB4DE
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.enemyTarget == null;
		}
	}
}
