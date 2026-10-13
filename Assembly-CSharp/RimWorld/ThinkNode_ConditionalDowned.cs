using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001788 RID: 6024
	public class ThinkNode_ConditionalDowned : ThinkNode_Conditional
	{
		// Token: 0x06008D59 RID: 36185 RVA: 0x002A0A10 File Offset: 0x0029EC10
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Downed;
		}
	}
}
