using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000FF2 RID: 4082
	public class ThinkNode_ConditionalRecharging : ThinkNode_Conditional
	{
		// Token: 0x06006470 RID: 25712 RVA: 0x001FD5AC File Offset: 0x001FB7AC
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.IsCharging();
		}
	}
}
