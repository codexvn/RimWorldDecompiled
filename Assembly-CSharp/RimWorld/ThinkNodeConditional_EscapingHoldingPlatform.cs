using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020017D5 RID: 6101
	public class ThinkNodeConditional_EscapingHoldingPlatform : ThinkNode_Conditional
	{
		// Token: 0x06008E00 RID: 36352 RVA: 0x002A1A84 File Offset: 0x0029FC84
		protected override bool Satisfied(Pawn pawn)
		{
			CompHoldingPlatformTarget compHoldingPlatformTarget = pawn.TryGetComp<CompHoldingPlatformTarget>();
			return compHoldingPlatformTarget != null && compHoldingPlatformTarget.isEscaping;
		}
	}
}
