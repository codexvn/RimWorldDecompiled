using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020017A3 RID: 6051
	public class ThinkNode_ConditionalExitTimedOut : ThinkNode_Conditional
	{
		// Token: 0x06008D91 RID: 36241 RVA: 0x002A0D51 File Offset: 0x0029EF51
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.exitMapAfterTick >= 0 && Find.TickManager.TicksGame > pawn.mindState.exitMapAfterTick;
		}
	}
}
