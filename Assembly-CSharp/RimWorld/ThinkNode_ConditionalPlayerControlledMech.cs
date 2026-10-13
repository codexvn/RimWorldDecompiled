using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001796 RID: 6038
	public class ThinkNode_ConditionalPlayerControlledMech : ThinkNode_Conditional
	{
		// Token: 0x06008D75 RID: 36213 RVA: 0x002A0B4C File Offset: 0x0029ED4C
		protected override bool Satisfied(Pawn pawn)
		{
			return pawn.IsColonyMechPlayerControlled;
		}
	}
}
