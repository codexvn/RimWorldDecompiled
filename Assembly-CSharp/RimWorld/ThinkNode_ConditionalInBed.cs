using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020017C8 RID: 6088
	public class ThinkNode_ConditionalInBed : ThinkNode_Conditional
	{
		// Token: 0x06008DE2 RID: 36322 RVA: 0x002A1743 File Offset: 0x0029F943
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.InBed();
		}
	}
}
