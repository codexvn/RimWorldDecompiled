using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020017B3 RID: 6067
	public class ThinkNode_ConditionalCanDoConstantThinkTreeJobNow : ThinkNode_Conditional
	{
		// Token: 0x06008DB4 RID: 36276 RVA: 0x002A113C File Offset: 0x0029F33C
		protected override bool Satisfied(Pawn pawn)
		{
			return (!pawn.Downed || pawn.health.CanCrawl) && !pawn.IsBurning() && !pawn.InMentalState && !pawn.Drafted && pawn.Awake();
		}
	}
}
