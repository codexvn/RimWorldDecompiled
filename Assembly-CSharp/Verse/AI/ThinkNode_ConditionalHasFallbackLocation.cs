using System;

namespace Verse.AI
{
	// Token: 0x02000FE6 RID: 4070
	public class ThinkNode_ConditionalHasFallbackLocation : ThinkNode_Conditional
	{
		// Token: 0x06006456 RID: 25686 RVA: 0x001FD3C0 File Offset: 0x001FB5C0
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.duty != null && pawn.mindState.duty.focusSecond.IsValid;
		}
	}
}
