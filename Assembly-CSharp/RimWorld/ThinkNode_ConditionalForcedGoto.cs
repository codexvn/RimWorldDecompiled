using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020017A4 RID: 6052
	public class ThinkNode_ConditionalForcedGoto : ThinkNode_Conditional
	{
		// Token: 0x06008D93 RID: 36243 RVA: 0x002A0D7A File Offset: 0x0029EF7A
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.mindState.forcedGotoPosition.IsValid;
		}
	}
}
