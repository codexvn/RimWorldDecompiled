using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200178F RID: 6031
	public class ThinkNode_ConditionalReleased : ThinkNode_Conditional
	{
		// Token: 0x06008D67 RID: 36199 RVA: 0x002A0AD4 File Offset: 0x0029ECD4
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.guest != null && pawn.guest.Released;
		}
	}
}
