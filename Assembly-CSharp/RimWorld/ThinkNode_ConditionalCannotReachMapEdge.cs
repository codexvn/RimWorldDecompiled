using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020017AA RID: 6058
	public class ThinkNode_ConditionalCannotReachMapEdge : ThinkNode_Conditional
	{
		// Token: 0x06008DA0 RID: 36256 RVA: 0x002A0E86 File Offset: 0x0029F086
		protected override bool Satisfied(Pawn pawn)
		{
			return !pawn.CanReachMapEdge();
		}
	}
}
