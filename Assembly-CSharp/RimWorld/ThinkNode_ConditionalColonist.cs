using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001790 RID: 6032
	public class ThinkNode_ConditionalColonist : ThinkNode_Conditional
	{
		// Token: 0x06008D69 RID: 36201 RVA: 0x00141B46 File Offset: 0x0013FD46
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.IsColonist;
		}
	}
}
