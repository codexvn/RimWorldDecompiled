using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020017B7 RID: 6071
	public class ThinkNode_ConditionalInNonPlayerHomeMap : ThinkNode_Conditional
	{
		// Token: 0x06008DBD RID: 36285 RVA: 0x002A1234 File Offset: 0x0029F434
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.MapHeld != null && !pawn.MapHeld.IsPlayerHome;
		}
	}
}
