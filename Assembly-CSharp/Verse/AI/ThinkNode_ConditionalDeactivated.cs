using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000FED RID: 4077
	public class ThinkNode_ConditionalDeactivated : ThinkNode_Conditional
	{
		// Token: 0x06006466 RID: 25702 RVA: 0x001FD561 File Offset: 0x001FB761
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.IsDeactivated();
		}
	}
}
