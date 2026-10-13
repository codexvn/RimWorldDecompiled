using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200179C RID: 6044
	public class ThinkNode_ConditionalDrafted : ThinkNode_Conditional
	{
		// Token: 0x06008D82 RID: 36226 RVA: 0x002A0C2B File Offset: 0x0029EE2B
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Drafted;
		}
	}
}
