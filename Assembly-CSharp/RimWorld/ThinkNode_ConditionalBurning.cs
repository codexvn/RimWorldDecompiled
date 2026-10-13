using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200179E RID: 6046
	public class ThinkNode_ConditionalBurning : ThinkNode_Conditional
	{
		// Token: 0x06008D87 RID: 36231 RVA: 0x002A0CF7 File Offset: 0x0029EEF7
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.HasAttachment(ThingDefOf.Fire);
		}
	}
}
