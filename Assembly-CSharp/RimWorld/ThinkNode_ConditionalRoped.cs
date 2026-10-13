using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020017C4 RID: 6084
	public class ThinkNode_ConditionalRoped : ThinkNode_Conditional
	{
		// Token: 0x06008DD9 RID: 36313 RVA: 0x002A1687 File Offset: 0x0029F887
		protected override bool Satisfied(Pawn pawn)
		{
			Pawn_RopeTracker roping = pawn.roping;
			return roping != null && roping.IsRoped;
		}
	}
}
