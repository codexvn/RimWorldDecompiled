using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020017BF RID: 6079
	public class ThinkNode_ConditionalPackAnimalHasColonistToFollowWhilePacking : ThinkNode_Conditional
	{
		// Token: 0x06008DCF RID: 36303 RVA: 0x002A1392 File Offset: 0x0029F592
		protected override bool Satisfied(Pawn pawn)
		{
			return JobGiver_PackAnimalFollowColonists.GetPawnToFollow(pawn) != null;
		}
	}
}
