using System;
using RimWorld.Planet;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020017CF RID: 6095
	public class ThinkNode_ConditionalNotFormingCaravan : ThinkNode_Conditional
	{
		// Token: 0x06008DF2 RID: 36338 RVA: 0x002A19B6 File Offset: 0x0029FBB6
		protected override bool Satisfied(Pawn pawn)
		{
			return !pawn.IsFormingCaravan();
		}
	}
}
