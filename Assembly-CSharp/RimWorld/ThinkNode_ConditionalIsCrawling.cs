using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001789 RID: 6025
	public class ThinkNode_ConditionalIsCrawling : ThinkNode_Conditional
	{
		// Token: 0x06008D5B RID: 36187 RVA: 0x002A0A18 File Offset: 0x0029EC18
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.Crawling;
		}
	}
}
